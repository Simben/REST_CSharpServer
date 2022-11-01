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
        /// Use FormData
        /// </summary>
        /// <param name="route"></param>
        /// <param name="jsonEntity"></param>
        /// <returns></returns>
        private async Task<Model.HTTPResponse> Post(string route, string jsonEntity)
        {
            string rawResponse = null;

            // HTTP GET.  
            using (var client = new HttpClient(new Log.LoggingHandler(new HttpClientHandler())))
            {
                //Verification des access
                var res = await this._authManager.CheckAuthToken().ConfigureAwait(false);
                if (res.errorCode != 0)
                    return new Model.HTTPResponse() { errorCode = res.errorCode, responseContent = res.errorMessage, httpStatusCode = "-1", httpStatusMessage = "" };


                // Setting Authorization.  
                this._authManager.OverrideHTTPHeaderWithAuth(client);

                // Setting Base address.  
                client.BaseAddress = new Uri(this.BaseURL);

                // Setting content type.  
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var stringContent = new StringContent(jsonEntity);
                stringContent.Headers.ContentType = MediaTypeHeaderValue.Parse("multipart/form-data");

                var content = new MultipartFormDataContent();

                content.Add(stringContent,"data");
                var response = await client.PostAsync(route, content).ConfigureAwait(false);


                // Verification  
                /*if (response.IsSuccessStatusCode)
                {
                    rawResponse = await response.Content.ReadAsStringAsync();
                    return new Model.HTTPResponse() { errorCode = 0, httpStatusCode = response.StatusCode.ToString(), httpStatusMessage = response.ReasonPhrase, responseContent = rawResponse };
                }*/
                if (response.CanReadRawBody())
                {
                    rawResponse = await response.Content.ReadAsStringAsync();
                    return new Model.HTTPResponse() { errorCode = 0, httpStatusCode = response.StatusCode.ToString(), httpStatusMessage = response.ReasonPhrase, responseContent = rawResponse };
                }
                return new Model.HTTPResponse() { errorCode = 300, httpStatusCode = response.StatusCode.ToString(), httpStatusMessage = response.ReasonPhrase, responseContent = "" };
            }
        }

        /// <summary>
        /// Use FormData
        /// </summary>
        /// <param name="route"></param>
        /// <param name="jsonEntity"></param>
        /// <returns></returns>
        private async Task<Model.HTTPResponse> PostRaw(string route, string jsonEntity)
        {
            string rawResponse = null;

            // HTTP GET.  
            using (var client = new HttpClient(new Log.LoggingHandler(new HttpClientHandler())))
            {
                //Verification des access
                var res = await this._authManager.CheckAuthToken().ConfigureAwait(false);
                if (res.errorCode != 0)
                    return new Model.HTTPResponse() { errorCode = res.errorCode, responseContent = res.errorMessage, httpStatusCode = "-1", httpStatusMessage = "" };


                // Setting Authorization.  
                this._authManager.OverrideHTTPHeaderWithAuth(client);

                // Setting Base address.  
                client.BaseAddress = new Uri(this.BaseURL);

                // Setting content type.  
                //client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                //var stringContent = new StringContent(jsonEntity);
                //stringContent.Headers.ContentType = MediaTypeHeaderValue.Parse("multipart/form-data");

                var content = new StringContent(jsonEntity, System.Text.Encoding.UTF8, "application/json");

                //content.Add(stringContent, "data");
                var response = await client.PostAsync(route, content).ConfigureAwait(false);


                // Verification  

                /*if (response.IsSuccessStatusCode)
                {
                    rawResponse = await response.Content.ReadAsStringAsync();
                    return new Model.HTTPResponse() { errorCode = 0, httpStatusCode = response.StatusCode.ToString(), httpStatusMessage = response.ReasonPhrase, responseContent = rawResponse };
                }*/
                if (response.CanReadRawBody())
                {
                    rawResponse = await response.Content.ReadAsStringAsync();
                    return new Model.HTTPResponse() { errorCode = 0, httpStatusCode = response.StatusCode.ToString(), httpStatusMessage = response.ReasonPhrase, responseContent = rawResponse };
                }
                return new Model.HTTPResponse() { errorCode = 300, httpStatusCode = response.StatusCode.ToString(), httpStatusMessage = response.ReasonPhrase, responseContent = "" };
            }
        }



        public async Task<T> Post<T,U>(string route, U entity)
        {
            //PATCH n'est pas standart dans HttpClient en .NETFramework
            //On doit faire diferreement
            //Dans un premier temps on dois serialized l'object

            string json = JsonConvert.SerializeObject(entity);

            var res = await this.Post(route, json).ConfigureAwait(false);
            if (res.isSuccess)
            {
                var responseObject = JsonConvert.DeserializeObject<T>(res.responseContent);
                return responseObject;
            }
            throw new Model.APIException(res.httpStatusCode, res.httpStatusMessage, res.responseContent);

        }

        public async Task<T> PostRaw<T, U>(string route, U entity)
        {
            //PATCH n'est pas standart dans HttpClient en .NETFramework
            //On doit faire diferreement
            //Dans un premier temps on dois serialized l'object

            string json = JsonConvert.SerializeObject(entity);
#if DEBUG
            if (!System.IO.Directory.Exists("JSON"))
                System.IO.Directory.CreateDirectory("JSON");
            using (var sw = new System.IO.StreamWriter($"JSON\\Json_Log_{DateTime.Today.ToString("ddMMyyyy")}.txt", true))
            {
                sw.WriteLine("----- NEW JSON POST");
                sw.WriteLine(json);
                sw.WriteLine("");
                sw.Close();
            }
#endif


                var res = await this.PostRaw(route, json).ConfigureAwait(false);
            if (res.isSuccess)
            {
                var responseObject = JsonConvert.DeserializeObject<T>(res.responseContent);
                return responseObject;
            }
            throw new Model.APIException(res.httpStatusCode, res.httpStatusMessage, res.responseContent);

        }

        public async Task<T> Post<T,U>(string route, U entity, char separator, params string[] param)
        {
            return await this.Post<T,U>(Helper.RouteParamManager.Concat(route, separator, param), entity).ConfigureAwait(false);
        }

        public async Task<T> PostRaw<T, U>(string route, U entity, char separator, params string[] param)
        {
            return await this.PostRaw<T, U>(Helper.RouteParamManager.Concat(route, separator, param), entity).ConfigureAwait(false);
        }
    }
}
