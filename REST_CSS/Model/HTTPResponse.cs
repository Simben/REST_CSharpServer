using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace REST_CSS.Model
{
    public class HTTPResponse
    {
        public string httpStatusCode { get; set; }
        public string httpStatusMessage { get; set; }
        public string responseContent { get; set; }
        public int errorCode { get; set; }

        public T CastToEntity<T>()
        {
            return JsonConvert.DeserializeObject<T>(this.responseContent);
        }

        public bool isSuccess { get { return this.errorCode == 0; } }

    };
}
