using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace REST_CSS.Helper
{
    public static class ResponceCode_Helper
    {
        public static bool CanReadRawBody(this HttpResponseMessage response)
        {
            var code = ((int)response.StatusCode);
            return code == 200 || (code >= 400 && code <= 500);
        }
    }
}
