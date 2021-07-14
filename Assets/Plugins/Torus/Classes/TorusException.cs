using System;

public class TorusException : Exception
{
    public TorusException(string source, string message) : base(message)
    {
        this.Source = source;
    }
}

public class TorusUserCancelledException : TorusException
{
    public TorusUserCancelledException() : base("UserCancelledException", "User cancelled.") { }
}

public class TorusNoAllowedBrowserFoundException : TorusException
{
    public TorusNoAllowedBrowserFoundException() : base("NoAllowedBrowserFoundException", "No allowed browser found.") { }
}
