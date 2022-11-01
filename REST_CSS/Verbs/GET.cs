using Newtonsoft.Json;
using REST_CSS.Helper;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace REST_CSS
{
    public partial class REST_Client
    {
        private async Task<Model.HTTPResponse> Get(string route, Dictionary<string, string> customHeaders)
        {
            string rawResponse = null;

            // HTTP GET.  
            using (var client = new HttpClient())
            {
                //Verification des access
                var res = await this._authManager.CheckAuthToken().ConfigureAwait(false);
                if (res.errorCode != 0)
                    return new Model.HTTPResponse() { errorCode = res.errorCode, responseContent = res.errorMessage, httpStatusCode = "-1", httpStatusMessage = res.errorMessage };


                // Setting Authorization.  
                this._authManager.OverrideHTTPHeaderWithAuth(client);

                // Setting Base address.  
                client.BaseAddress = new Uri(this.BaseURL);

                // Setting content type.  
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                if (customHeaders != null && customHeaders.Count > 0)
                {
                    foreach (var h in customHeaders)
                        client.DefaultRequestHeaders.Add(h.Key, h.Value);
                }

                // Initialization.  
                HttpResponseMessage response = new HttpResponseMessage();

                // HTTP GET  
                response = await client.GetAsync(route).ConfigureAwait(false);
                // Verification  

                if (response.CanReadRawBody())
                {
                    rawResponse = await response.Content.ReadAsStringAsync();
                    return new Model.HTTPResponse() { errorCode = 0, httpStatusCode = response.StatusCode.ToString(), httpStatusMessage = response.ReasonPhrase, responseContent = rawResponse };
                }

                return new Model.HTTPResponse() { errorCode = ((int)response.StatusCode), httpStatusCode = response.StatusCode.ToString(), httpStatusMessage = response.ReasonPhrase, responseContent = "" };
            }

        }

        /// <summary>
        /// throw exception
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="route"></param>
        /// <returns></returns>
        public async Task<T> Get<T>(string route, Dictionary<string, string> customHeaders)
        {
            var res = await this.Get(route, customHeaders);
            if (res.isSuccess)
                return JsonConvert.DeserializeObject<T>(res.responseContent);
            throw new Model.APIException(res.httpStatusCode, res.httpStatusMessage, res.responseContent);
        }

        /// <summary>
        /// throw exception
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="route"></param>
        /// <returns></returns>
        public async Task<T> Get<T>(string route)
        {
            var res = await this.Get(route, null);
            if (res.isSuccess)
                return JsonConvert.DeserializeObject<T>(res.responseContent);
            throw new Model.APIException(res.httpStatusCode, res.httpStatusMessage,  res.responseContent);
        }
        /// <summary>
        /// throw exception
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="route"></param>
        /// <returns></returns>
        public async Task<string> GetRaw(string route)
        {
            var res = await this.Get(route, null);
            if (res.isSuccess)
                return res.responseContent;
            throw new Model.APIException(res.httpStatusCode, res.httpStatusMessage, res.responseContent);
        }
        public async Task<string> GetRaw(string route, Dictionary<string, string> customHeaders)
        {
            var res = await this.Get(route, customHeaders);
            if (res.isSuccess)
                return res.responseContent;
            throw new Model.APIException(res.httpStatusCode, res.httpStatusMessage, res.responseContent);
        }

        /// <summary>
        /// use '%' as caracter to include param in
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="route">route vers l'entity sans la base URL</param>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<T> Get<T>(string route, char separator, params string[] param)
        {
            return await this.Get<T>(Helper.RouteParamManager.Concat(route, separator, param));
        }


        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="route"></param>
        /// <param name="separator"></param>
        /// <param name="customHeaders">Dictionnary of custom header to add to the Get Resquest</param>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<T> Get<T>(string route, char separator, Dictionary<string, string> customHeaders, params string[] param)
        {
            return await this.Get<T>(Helper.RouteParamManager.Concat(route, separator, param), customHeaders);
        }

    }
}
