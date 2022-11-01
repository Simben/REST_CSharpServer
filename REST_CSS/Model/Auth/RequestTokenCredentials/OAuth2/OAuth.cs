using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace REST_CSS.Model.Auth
{
    public enum OAuthGrantType
    {
        None,
        Authorization_Code,
        Authorization_Code_With_PKCE,
        Implicit,
        PassWordCredentials,
        ClientCredentials
    }

    public class OAuth
    {
        protected OAuthGrantType _grantType = OAuthGrantType.None;
        protected string _callbackURL;
        protected string _authURL;
        protected string _accessTokenURL;
        protected string _codeChallengeMethod;
        protected string _codeVerifier;
        protected string _username;
        protected string _password;
        protected string _clientID;
        protected string _clientSecret;
        protected string _scope;
        protected string _state;
        protected bool _sendAsBasicAuthHeader;

        //Disable the defualt no parameter constructor
        protected OAuth()
        { }

        public string accessURL
        {
            get
            {
                switch (this._grantType)
                {
                    case OAuthGrantType.None:
                        return "";
                    case OAuthGrantType.ClientCredentials:
                        return this._accessTokenURL;
                    case OAuthGrantType.Implicit:
                        return this._authURL;
                    default:
                        return "";
                }
            }
        }

        public bool canBeRefreshed
        {
            get {
                switch (this._grantType)
                {
                    case OAuthGrantType.None:
                    case OAuthGrantType.ClientCredentials:
                    case OAuthGrantType.Implicit:
                        return false;

                    default:
                        return false;
                }
            }
        }

        //Defini si on doit fetch un token ou pas (surtout cas du None)
        public bool isTokenNeeded { get { return this._grantType != OAuthGrantType.None; } }

        public bool sendAsBasicAuthHeader { get => _sendAsBasicAuthHeader; }

        public HttpContent GenerateRequestTokenBody()
        {
            if (this._sendAsBasicAuthHeader)
                return null;

            switch (this._grantType)
            {
                case OAuthGrantType.None: return null;
                case OAuthGrantType.ClientCredentials:
                    {
                        var InputParams = new List<KeyValuePair<string, string>>();
                        InputParams.Add(new KeyValuePair<string, string>("grant_type", "client_credentials"));
                        InputParams.Add(new KeyValuePair<string, string>("client_id", this._clientID));
                        InputParams.Add(new KeyValuePair<string, string>("client_secret", this._clientSecret));
                        InputParams.Add(new KeyValuePair<string, string>("scope", this._scope));
                        return new FormUrlEncodedContent(InputParams);
                    }
                default: return null;

            }
        }
    }
}
