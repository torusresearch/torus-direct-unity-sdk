#import <UnityFramework/UnityFramework-Swift.h>

extern "C"
{
    void TorusDirect_iOS_init(const char* browserRedirectUri, const char* network, const char* redirectUri)
    {
        [[TorusDirectUnity shared] initializeWithBrowserRedirectUri:[NSString stringWithUTF8String:browserRedirectUri] network:[NSString stringWithUTF8String:network] redirectUri:[NSString stringWithUTF8String:redirectUri]];
    }
    void TorusDirect_iOS_triggerLogin(const char* callbackGameObject, const char* callbackMethod)
    {
        [[TorusDirectUnity shared] triggerLoginWithCallbackGameObject:[NSString stringWithUTF8String:callbackGameObject] callbackMethod:[NSString stringWithUTF8String:callbackMethod]];
    }
}