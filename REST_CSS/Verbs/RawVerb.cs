using Newtonsoft.Json;
using REST_CSS.Helper;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace REST_CSS
{

    public partial class REST_Client
    {

        /// <summary>
        /// Return Raw HTTP Response
        /// </summary>
        /// <param name="route"></param>
        /// <param name="jsonEntity"></param>
        /// <param name="methodVerb"></param>
        /// <returns></returns>
        private async Task<Model.HTTPResponse> SendAsyncRequest(string route, string jsonEntity, string methodVerb)
        {
            string rawResponse = null;

            using (var client = new HttpClient(new Log.LoggingHandler(new HttpClientHandler())))
            {
                //Verification des access

                var res = await this._authManager.CheckAuthToken().ConfigureAwait(false);
                if (res.errorCode != 0)
                    return new Model.HTTPResponse() { errorCode = res.errorCode, responseContent = res.errorMessage, httpStatusCode = "-1", httpStatusMessage = "" };

                var method = new HttpMethod(methodVerb);

                HttpRequestMessage request = null;

                if (jsonEntity != null)
                {
                    List<KeyValuePair<string, string>> allIputParams = new List<KeyValuePair<string, string>>();
                    Dictionary<string, string> DictionarizedJsonObject = JsonConvert.DeserializeObject<Dictionary<string, string>>(jsonEntity);

                    foreach (var value in DictionarizedJsonObject)
                        allIputParams.Add(new KeyValuePair<string, string>(value.Key, value.Value));

                    request = new HttpRequestMessage(method, route)
                    {
                        Content = new FormUrlEncodedContent(allIputParams)
                    };
                }
                else
                {
                    request = new HttpRequestMessage(method, route);
                }
                // Setting Authorization.  
                // Setting Authorization.  
                this._authManager.OverrideHTTPHeaderWithAuth(client);

                // Setting Base address.  
                client.BaseAddress = new Uri(this.BaseURL);

                // Setting content type.  
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                // Initialization.  
                HttpResponseMessage response = new HttpResponseMessage();

                try
                {
                    // HTTP GET  
                    response = await client.SendAsync(request).ConfigureAwait(false);
                }
                catch
                {
                    return new Model.HTTPResponse() { errorCode = 300, httpStatusCode = "404", httpStatusMessage = "", responseContent = "" };
                }
                // Verification  
                /*
                if (response.IsSuccessStatusCode)
                {
                    rawResponse = await response.Content.ReadAsStringAsync();
                    return new Model.HTTPResponse() { errorCode = 0, httpStatusCode = response.StatusCode.ToString(), httpStatusMessage = response.ReasonPhrase, responseContent = rawResponse };
                }
                return new Model.HTTPResponse() { errorCode = 300, httpStatusCode = response.StatusCode.ToString(), httpStatusMessage = response.ReasonPhrase, responseContent = "" };
                */


                if (response.CanReadRawBody())
                {
                    rawResponse = await response.Content.ReadAsStringAsync();
                    return new Model.HTTPResponse() { errorCode = 0, httpStatusCode = response.StatusCode.ToString(), httpStatusMessage = response.ReasonPhrase, responseContent = rawResponse };
                }
                return new Model.HTTPResponse() { errorCode = ((int)response.StatusCode), httpStatusCode = response.StatusCode.ToString(), httpStatusMessage = response.ReasonPhrase, responseContent = "" };

            }
        }

        private async Task<Model.HTTPResponse> SendAsyncRequestRaw(string route, string jsonEntity, string methodVerb)
        {
            string rawResponse = null;

            using (var client = new HttpClient(new Log.LoggingHandler(new HttpClientHandler())))
            {
                //Verification des access

                var res = await this._authManager.CheckAuthToken().ConfigureAwait(false);
                if (res.errorCode != 0)
                    return new Model.HTTPResponse() { errorCode = res.errorCode, responseContent = res.errorMessage, httpStatusCode = "-1", httpStatusMessage = "" };

                var method = new HttpMethod(methodVerb);

                HttpRequestMessage request = null;

                if (jsonEntity != null)
                {
                    var content = new StringContent(jsonEntity, System.Text.Encoding.UTF8, "application/json");

                    request = new HttpRequestMessage(method, route)
                    {
                        Content = content
                    };
                }
                else
                {
                    request = new HttpRequestMessage(method, route);
                }
                // Setting Authorization.  
                // Setting Authorization.  
                this._authManager.OverrideHTTPHeaderWithAuth(client);

                // Setting Base address.  
                client.BaseAddress = new Uri(this.BaseURL);

                // Setting content type.  
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                // Initialization.  
                HttpResponseMessage response = new HttpResponseMessage();

                try
                {
                    // HTTP GET  
                    response = await client.SendAsync(request).ConfigureAwait(false);
                }
                catch
                {
                    return new Model.HTTPResponse() { errorCode = 300, httpStatusCode = "404", httpStatusMessage = "", responseContent = "" };
                }
                // Verification  
                /*
                if (response.IsSuccessStatusCode)
                {
                    rawResponse = await response.Content.ReadAsStringAsync();
                    return new Model.HTTPResponse() { errorCode = 0, httpStatusCode = response.StatusCode.ToString(), httpStatusMessage = response.ReasonPhrase, responseContent = rawResponse };
                }
                return new Model.HTTPResponse() { errorCode = 300, httpStatusCode = response.StatusCode.ToString(), httpStatusMessage = response.ReasonPhrase, responseContent = "" };
                */

                if (response.CanReadRawBody())
                {
                    rawResponse = await response.Content.ReadAsStringAsync();
#if DEBUG
                    if (!System.IO.Directory.Exists("JSON"))
                        System.IO.Directory.CreateDirectory("JSON");
                    using (var sw = new System.IO.StreamWriter($"JSON\\Json_Log_{DateTime.Today.ToString("ddMMyyyy")}.txt", true))
                    {
                        sw.WriteLine("----- NEW JSON REPLY");
                        sw.WriteLine(rawResponse);
                        sw.WriteLine("");
                        sw.Close();
                    }
#endif
                    return new Model.HTTPResponse() { errorCode = 0, httpStatusCode = response.StatusCode.ToString(), httpStatusMessage = response.ReasonPhrase, responseContent = rawResponse };
                }
                return new Model.HTTPResponse() { errorCode = ((int)response.StatusCode), httpStatusCode = response.StatusCode.ToString(), httpStatusMessage = response.ReasonPhrase, responseContent = "" };

            }
        }
    }
}
