import Foundation

@objc public class TorusDirectUnity: NSObject
{
    @objc public static let shared = TorusDirectUnity()

    @objc public func initialize()
    {
        print("TorusDirectUnity.initialize")
    }

    @objc public func triggerLogin(callbackGameObject: String, callbackMethod: String) {
        print("TorusDirectUnity.triggerLogin")
        UnitySendMessage(callbackGameObject, callbackMethod, "{\"status\":\"rejected\",\"reason\":{\"name\":\"NotImplementedException\",\"message\":\"Method is not implemented.\"}}")
    }
}