import Foundation

@objc public class TorusDirectUnity: NSObject
{
    @objc public static let shared = TorusDirectUnity()

    @objc public func initialize(browserRedirectUri: String, network: String, redirectUri: String)
    {
        print("TorusDirectUnity.initialize: browserRedirectUri=\(browserRedirectUri), network=\(network), redirectUri=\(redirectUri)")
    }

    @objc public func triggerLogin(callbackGameObject: String, callbackMethod: String) {
        print("TorusDirectUnity.triggerLogin")
        UnitySendMessage(callbackGameObject, callbackMethod, "{\"status\":\"rejected\",\"reason\":{\"name\":\"NotImplementedException\",\"message\":\"Method is not implemented.\"}}")
    }
}