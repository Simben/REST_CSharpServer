using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace REST_CCS.Model.Auth.RequestTokenCredentials.OAuth2.Method
{
    public class Implicit : REST_CSS.Model.Auth.OAuth
    {
        public Implicit(string CallbackURL, string AuthURL, string ClientID, string Scope, string State, bool SendAsBasicAuthHeader = false)
        {
            base._callbackURL = CallbackURL;
            base._authURL = AuthURL;
            base._clientID = ClientID;
            base._scope = Scope;
            base._state = State;
            base._grantType = REST_CSS.Model.Auth.OAuthGrantType.Implicit;
            base._sendAsBasicAuthHeader = SendAsBasicAuthHeader;
        }
    }
}
