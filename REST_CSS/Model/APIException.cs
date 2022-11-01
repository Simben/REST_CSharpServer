using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace REST_CSS.Model
{
    public class APIException : Exception
    {
        public string httpStatusCode { get; set; }
        public string httpReason { get; set; }
        public string errorMessage { get; set; }

        public APIException(string HttpStatusCode, string HTTPReason, string ErrorMessage)
        {
            this.httpStatusCode = HttpStatusCode;
            this.httpReason = HTTPReason;
            this.errorMessage = ErrorMessage;
        }

    }
}
