using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace REST_CSS.Authentication
{
    public class AuthentificationManager
    {
        Model.Auth.OAuthToken _token;
        Model.Auth.OAuth _requestTokenCredentials = null;

        public AuthentificationManager(Model.Auth.OAuth credentials)
        {
            this._requestTokenCredentials = credentials;
        }

        private bool TryRefreshToken()
        {
            if (this._requestTokenCredentials.canBeRefreshed)
            {
                //ToDO procedeure dans le acs ou c'est possible
                return true;
            }
            else
                return false;
        }

        public async Task<Model.HTTP_Error> CheckAuthToken()
        {
            //On doit verifier si le Token est toujours Valide
            //1- Est ce quon a deja request un token ?
            if (_token != null)
            {
                if (!string.IsNullOrEmpty(_token.access_token))
                {
                    //2- On verifie si le Token est toujours valide dans ce cas on fait rien
                    if (_token.expire_at > DateTime.Now)
                        return new Model.HTTP_Error() { errorCode = 0, errorMessage = "" };


                    //3- On verifie Si le RefreshToken est supporte dans le mode en cours
                    if (_token.isRefreshTokenSupported)
                    {
                        if (TryRefreshToken())
                        {
                            // On a reussi a utiliser le refresh Token
                            return new Model.HTTP_Error() { errorCode = 0, errorMessage = "" };
                        }
                    }
                }
            }

            if (!this._requestTokenCredentials.isTokenNeeded)
                return new Model.HTTP_Error() { errorCode = 0, errorMessage = "" };

            //Dans tous les autres cas on doit re/faire la demande
            var res = (await GetAuthorizationToken().ConfigureAwait(false));
            if (res.isSuccess)
                return new Model.HTTP_Error() { errorCode = 0, errorMessage = "" };

            return new Model.HTTP_Error() { errorCode = res.errorCode, errorMessage = (res.httpStatusCode == "-1") ? res.responseContent : "HTTP error server returned :" + res.httpStatusMessage + ", when atempting to get new access token" };

        }

        private async Task<Model.HTTPResponse> GetAuthorizationToken()
        {
            if (string.IsNullOrWhiteSpace(this._requestTokenCredentials.accessURL))
                return new Model.HTTPResponse() { errorCode = 101, httpStatusCode = "-1", httpStatusMessage = "", responseContent = "Access URL is not set" };

            // Initialization.  
            string responseObj = string.Empty;

            // Posting.  
            using (var client = new HttpClient())
            {
                // Setting Base address.  
                client.BaseAddress = new Uri(this._requestTokenCredentials.accessURL);

                // Setting content type.  
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                // Initialization.  
                HttpResponseMessage response = new HttpResponseMessage();

                if (this._requestTokenCredentials.sendAsBasicAuthHeader)
                {
                    //generate Headser
                }
                else
                {
                    HttpContent requestParams = this._requestTokenCredentials.GenerateRequestTokenBody();
                    response = await client.PostAsync("", requestParams).ConfigureAwait(false);
                }

                // Verification  
                if (response.IsSuccessStatusCode)
                {
                    // Reading Response.  
                    var content = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                    try
                    {
                        var res = new Model.HTTPResponse() { errorCode = 0, responseContent = content, httpStatusCode = response.StatusCode.ToString(), httpStatusMessage = response.ReasonPhrase };
                        this._token = res.CastToEntity<Model.Auth.OAuthToken>();

                        this._token.isRefreshTokenSupported = this._requestTokenCredentials.canBeRefreshed;
                        this._token.expire_at = DateTime.Now.AddSeconds(this._token.expires_in);
                        return res;
                    }
                    catch (Exception ex)
                    {
                        return new Model.HTTPResponse() { errorCode = 102, httpStatusCode = "-1", httpStatusMessage = "", responseContent = "Unable to deserialize json token object. Exception catched : " + ex.Message };
                    }
                }
                else
                {
                    return new Model.HTTPResponse() { errorCode = 300, httpStatusCode = response.StatusCode.ToString(), httpStatusMessage = response.ReasonPhrase, responseContent = "" };
                }
            }
        }

        public void OverrideHTTPHeaderWithAuth(HttpClient client)
        {
            if (this._requestTokenCredentials.isTokenNeeded)
            {
                client.DefaultRequestHeaders.Authorization =  new AuthenticationHeaderValue("Bearer", this._token.access_token);
            }
        }

    }
}
