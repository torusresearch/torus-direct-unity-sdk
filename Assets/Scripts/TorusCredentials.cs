
public class TorusCredentials
{
    public string PrivateKey { get; }
    public string PublicAddress { get; }

    public TorusCredentials(string privateKey, string publicAddress)
    {
        this.PrivateKey = privateKey;
        this.PublicAddress = publicAddress;
    }
}
