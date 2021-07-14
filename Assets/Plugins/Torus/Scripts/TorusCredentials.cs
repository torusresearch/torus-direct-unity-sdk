namespace Torus
{
    public class TorusCredentials
    {
        public string privateKey { get; }
        public string publicAddress { get; }

        public TorusCredentials(string privateKey, string publicAddress)
        {
            this.privateKey = privateKey;
            this.publicAddress = publicAddress;
        }
    }
}