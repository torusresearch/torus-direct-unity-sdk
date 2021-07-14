using System;

[Serializable]
public class TorusResponse
{
    [Serializable]
    public class Reason
    {
        public string name;
        public string message;
    }

    [Serializable]
    public class Value
    {
        public string privateKey;
        public string publicAddress;
    }

    public string status;
    public Value value = null;
    public Reason reason = null;
}
