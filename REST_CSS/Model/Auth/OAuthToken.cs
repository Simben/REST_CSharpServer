using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace REST_CSS.Model.Auth
{
    public class OAuthToken
    {
        public string access_token { get; set; }
        public string resfresh_Token { get; set; }
        public int expires_in { get; set; }
        public DateTime expire_at { get; set; }
        public string token_type { get; set; }
        public string scope { get; set; }
        public bool isRefreshTokenSupported { get; set; }
    };
}
