using System;

namespace Torus
{
    [Serializable]
    public class TorusJWTParams
    {
        public string domain;
        public string client_id;
        public string leeway;
        public string verifierIdField;
        public bool isVerifierIdCaseSensitive;
        public string display;
        public string prompt;
        public string max_age;
        public string ui_locales;
        public string id_token_hint;
        public string login_hint;
        public string acr_values;
        public string scope;
        public string audience;
        public string connection;
        public object additionalParams;
    }
}