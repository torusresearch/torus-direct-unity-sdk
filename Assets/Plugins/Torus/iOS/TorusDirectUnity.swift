import Foundation
import TorusSwiftDirectSDK

struct TorusDirectUnityInitArgs {
    let network: String;
    let browserRedirectUri: String;
    let redirectUri: String;
    let browserType: String;
}

struct TorusDirectUnityTriggerLoginArgs : Decodable {
    let typeOfLogin: String;
    let verifier: String;
    let clientId: String;
    var jwtParams: Dictionary<String, String>? = nil;
}

struct TorusDirectUnityResponse: Codable {
    let status: String;
    let reason: TorusDirectUnityError?;
    let value: Dictionary<String, String>?;
}

struct TorusDirectUnityError: Codable {
    let name: String;
    let messge: String;
}

@objc public class TorusDirectUnity: NSObject
{
    @objc public static let shared = TorusDirectUnity()
    
    var initArgs: TorusDirectUnityInitArgs? = nil
    
    func sendError(callbackGameObject: String, callbackMethod: String, name: String, message: String) {
        let jsonData = try! JSONEncoder().encode(TorusDirectUnityResponse(status: "rejected", reason: TorusDirectUnityError(name: name, messge: message), value: nil))
        let jsonString = String(data: jsonData, encoding: .utf8)!
        UnitySendMessage(callbackGameObject, callbackMethod, jsonString)
    }
    
    func sendResult(callbackGameObject: String, callbackMethod: String, value: Dictionary<String, String>) {
        let jsonData = try! JSONEncoder().encode(TorusDirectUnityResponse(status: "fulfilled", reason: nil, value: value))
        let jsonString = String(data: jsonData, encoding: .utf8)!
        UnitySendMessage(callbackGameObject, callbackMethod, jsonString)
    }
    
    func getLoginProvider(typeOfLogin: String) -> LoginProviders? {
        if typeOfLogin == "email_password" {
            return LoginProviders.email_password
        }
        return LoginProviders(rawValue: typeOfLogin)
    }

    @objc public func initialize(browserRedirectUri: String, network: String, redirectUri: String, browserType: String)
    {
        print("TorusDirectUnity.initialize: browserRedirectUri=\(browserRedirectUri), network=\(network), redirectUri=\(redirectUri), browserType=\(browserType)")
        self.initArgs = TorusDirectUnityInitArgs(network: network, browserRedirectUri: browserRedirectUri, redirectUri: redirectUri, browserType: browserType)
    }
    
    @objc public func handleURL(_ urlString: String) {
        guard let url = URL(string: urlString) else { return }
        TorusSwiftDirectSDK.handle(url: url)
    }

    @objc public func triggerLogin(callbackGameObject: String, callbackMethod: String, json: String) {
        print("TorusDirectUnity.triggerLogin")
        
        guard let initArgs = initArgs
        else {
            sendError(callbackGameObject: callbackGameObject, callbackMethod: callbackMethod, name: "NotInitializedException", message: "TorusDirect.Init must be called before TriggerLogin.")
            return
        }
        
        print(json)
        guard
            let jsonData = json.data(using: .utf8),
            let args = try? JSONSerialization.jsonObject(with: jsonData, options: []) as? Dictionary<String, Any>,
            let typeOfLogin = args["typeOfLogin"] as? String,
            let loginProvider = getLoginProvider(typeOfLogin: typeOfLogin),
            let verifier = args["verifier"] as? String,
            let clientId = args["clientId"] as? String
        else {
            sendError(callbackGameObject: callbackGameObject, callbackMethod: callbackMethod, name: "ArgumentException", message: "Missing or invalid argument(s).")
            return
        }
        
        let jwtParams = args["jwtParams"] as? Dictionary<String, String>
        let subVerifierDetails = SubVerifierDetails(
            loginType: .web,
            loginProvider: loginProvider,
            clientId: clientId,
            verifierName: verifier,
            redirectURL: initArgs.redirectUri,
            browserRedirectURL: initArgs.browserRedirectUri,
            extraQueryParams: [:],
            jwtParams: jwtParams ?? [:]
        )
        let torusDirectSdk = TorusSwiftDirectSDK(
            aggregateVerifierType: .singleLogin,
            aggregateVerifierName: verifier,
            subVerifierDetails: [subVerifierDetails]
        )
        torusDirectSdk.triggerLogin(browserType: .external).done { data in
            guard
                let privateKey = data["privateKey"] as? String,
                let publicAddress = data["publicAddress"] as? String
            else {
                self.sendError(callbackGameObject: callbackGameObject, callbackMethod: callbackMethod, name: "InternalException", message: "Failed to retrieve keypair from login result.")
                return
            }
            self.sendResult(callbackGameObject: callbackGameObject, callbackMethod: callbackMethod, value: ["privateKey": privateKey, "publicAddress": publicAddress])
        }.catch { err in
            self.sendError(callbackGameObject: callbackGameObject, callbackMethod: callbackMethod, name: "LoginException", message: "\(err)")
        }
    }
}
