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
        /// <summary>
        /// This finction sould never be used because patch verb need an entity as an input so ....
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="route"></param>
        /// <returns></returns>
        public async Task<T> Patch<T>(string route)
        {
            //PATCH verb is not standart when using HttpClient in .NETFramework
            //We need to create a specific process for this
            
            
            var res = await this.SendAsyncRequestRaw(route, null, "PATCH").ConfigureAwait(false);
            if (res.isSuccess)
            {
                var response = JsonConvert.DeserializeObject<T>(res.responseContent);
                return response;
            }
            throw new Model.APIException(res.httpStatusCode, res.httpStatusMessage, res.responseContent);
        }

        public async Task<T> Patch<T>(string route, char separator, params string[] param)
        {
            return await this.Patch<T>(Helper.RouteParamManager.Concat(route, separator, param)).ConfigureAwait(false);
        }

        public async Task<T2> Patch<T1, T2>(string route, T1 entity)
        {
            //PATCH n'est pas standart dans HttpClient en .NETFramework
            //On doit faire diferreement
            //Dans un premier temps on dois serialized l'object

            string json = JsonConvert.SerializeObject(entity);

#if DEBUG
            //if You are in debug Mode, Create a Folder in the application directory and Log All Requested route and Server answer

            if (!System.IO.Directory.Exists("JSON"))
                System.IO.Directory.CreateDirectory("JSON");
            using (var sw = new System.IO.StreamWriter($"JSON\\Json_Log_{DateTime.Today.ToString("ddMMyyyy")}.txt", true))
            {
                sw.WriteLine("----- NEW JSON POST");
                sw.WriteLine(route);
                sw.WriteLine(json);
                sw.WriteLine("");
                sw.Close();
            }
#endif


            var res = await this.SendAsyncRequestRaw(route, json, "PATCH").ConfigureAwait(false);
            
            if (res.isSuccess)
            {
                var toto = JsonConvert.DeserializeObject<T2>(res.responseContent);
                return toto;
            }
            throw new Model.APIException(res.httpStatusCode, res.httpStatusMessage, res.responseContent);
        }



        public async Task<T2> Patch<T1, T2>(string route, T1 entity, char separator, params string[] param)
        {
            return await this.Patch<T1, T2>(Helper.RouteParamManager.Concat(route, separator, param), entity).ConfigureAwait(false);
        }
    }
}
