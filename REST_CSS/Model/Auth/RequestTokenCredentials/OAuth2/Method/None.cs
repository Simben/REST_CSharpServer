using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace REST_CCS.Model.Auth.RequestTokenCredentials.OAuth2.Method
{
    public class None : REST_CSS.Model.Auth.OAuth
    {
        public None()
        {
            base._grantType = REST_CSS.Model.Auth.OAuthGrantType.None;
        }
    }
}
