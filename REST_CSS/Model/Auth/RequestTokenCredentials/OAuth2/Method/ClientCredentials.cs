using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace REST_CCS.Model.Auth.RequestTokenCredentials.OAuth2.Method
{
    public class ClientCredentials : REST_CSS.Model.Auth.OAuth
    {
        public ClientCredentials(string AccessTokenURL, string ClientID, string ClientSecret, string Scope, bool SendAsBasicAuthHeader = false)
        {
            base._accessTokenURL = AccessTokenURL;
            base._clientID = ClientID;
            base._clientSecret = ClientSecret;
            base._scope = Scope;
            base._grantType = REST_CSS.Model.Auth.OAuthGrantType.ClientCredentials;
            base._sendAsBasicAuthHeader = SendAsBasicAuthHeader;
        }
    }
}
