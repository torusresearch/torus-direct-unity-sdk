using System;

namespace Torus
{
    [Serializable]
    public class TorusJWTParams
    {
        private string domain;
        private string client_id;
        private string leeway;
        private string verifierIdField;
        private bool isVerifierIdCaseSensitive;
        private string display;
        private string prompt;
        private string max_age;
        private string ui_locales;
        private string id_token_hint;
        private string login_hint;
        private string acr_values;
        private string scope;
        private string audience;
        private string connection;
        private object additionalParams;
    }
}