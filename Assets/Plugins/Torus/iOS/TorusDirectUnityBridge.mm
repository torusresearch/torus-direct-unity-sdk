#import <UnityFramework/UnityFramework-Swift.h>

extern "C"
{
    void TorusDirect_iOS_init()
    {
        [[TorusDirectUnity shared] initialize];
    }
}