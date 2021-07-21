#import <UnityFramework/UnityFramework-Swift.h>

extern "C"
{
    void TorusDirect_iOS_init(const char* browserRedirectUri, const char* network, const char* redirectUri, const char* browserType)
    {
        [[TorusDirectUnity shared] initializeWithBrowserRedirectUri:[NSString stringWithUTF8String:browserRedirectUri] network:[NSString stringWithUTF8String:network] redirectUri:[NSString stringWithUTF8String:redirectUri] browserType:[NSString stringWithUTF8String:browserType]];
    }
    void TorusDirect_iOS_handleURL(const char* url) {
        [[TorusDirectUnity shared] handleURL:[NSString stringWithUTF8String:url]];
    }
    void TorusDirect_iOS_triggerLogin(const char* callbackGameObject, const char* callbackMethod, const char* json)
    {
        [[TorusDirectUnity shared] triggerLoginWithCallbackGameObject:[NSString stringWithUTF8String:callbackGameObject] callbackMethod:[NSString stringWithUTF8String:callbackMethod] json:[NSString stringWithUTF8String:json]];
    }
    void TorusDirect_iOS_getTorusKey(const char* callbackGameObject, const char* callbackMethod, const char* json)
    {
        [[TorusDirectUnity shared] getTorusKeyWithCallbackGameObject:[NSString stringWithUTF8String:callbackGameObject] callbackMethod:[NSString stringWithUTF8String:callbackMethod] json:[NSString stringWithUTF8String:json]];
    }
}
