using Newtonsoft.Json;
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
        private bool isHTTPS = false;

        public bool EnableHTTPS
        {
            get { return isHTTPS; }
            set
            {
                if (value)
                {
                    this.isHTTPS = true;
                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
                }
                else
                {
                    this.isHTTPS = false;
                    ServicePointManager.SecurityProtocol = SecurityProtocolType.SystemDefault;
                }
            }
        }

        public string BaseURL { get => _baseURL; }

        private string _baseURL = "";

        //Model.RequestTokenCredentials _requestTokenCredentials = null;
        Authentication.AuthentificationManager _authManager;

        public REST_Client(string baseURL, Model.Auth.OAuth requestTokenCredentials)
        {
            this._baseURL = baseURL;
            this._authManager = new Authentication.AuthentificationManager(requestTokenCredentials);
        }

    }
}
