using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Linq;

namespace Futuristic.Services
{
    public class BaseService<T>
    {
        HttpClient _client;
        string _ListMethod = "List";
        string _AddUpdateMethod = "AddOrUpdate";
        string _GetByValue = "ByValue";

        string APIBaseUrl;
        public BaseService(string apiName)
        {
#if DEBUG
           APIBaseUrl = "https://192.168.0.17:44338/api/";
            APIBaseUrl = "https://futuristicapi20200429024818.azurewebsites.net/api/";
#else
            APIBaseUrl = "https://futuristicapi20200429024818.azurewebsites.net/api/";
#endif
            _client = new HttpClient();
            string className = string.Empty;
            if (!string.IsNullOrEmpty(apiName))
            {
                className = apiName;
            }
            else
            {
                className = typeof(T).Name;
            }
            _ListMethod = className + "/" + className + _ListMethod;
            _AddUpdateMethod = className + "/" + className + _AddUpdateMethod;
            _GetByValue = className + "/" + className + _GetByValue;
        }
        public BaseService()
        {

        }
        public async Task<List<T>> GetList(string filter = "")
        {

            List<T> list = new List<T>();
            try
            {
                var uri = APIBaseUrl + _GetByValue;
                if (!string.IsNullOrWhiteSpace(filter))
                {
                    uri += "?" + filter;
                }

                var response = await _client.GetAsync(uri);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    list = JsonConvert.DeserializeObject<List<T>>(content);
                }
                return list;
            }
            catch (Exception ex)
            {
                return list;
            }
        }
        public async Task<T> GetbyValue(string filter = "")
        {

            List<T> list = new List<T>();
            try
            {
                var uri = APIBaseUrl + _ListMethod;
                if (!string.IsNullOrWhiteSpace(filter))
                {
                    uri += "?" + filter;
                }

                var response = await _client.GetAsync(uri);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    list = JsonConvert.DeserializeObject<List<T>>(content);
                }
                return list.FirstOrDefault();
            }
            catch (Exception ex)
            {
                return list.FirstOrDefault();
            }
        }
        public async Task<T> AddUpdateEntity(T entity)
        {
            var Url = new Uri(string.Format(APIBaseUrl + _AddUpdateMethod));
            var json = JsonConvert.SerializeObject(entity);
            using (var request = new HttpRequestMessage(HttpMethod.Post, Url))
            using (var httpContent = CreateHttpContent(entity))
            {
                request.Content = httpContent;
                using (var response = await _client
                    .SendAsync(request, HttpCompletionOption.ResponseHeadersRead)
                    .ConfigureAwait(false))
                {
                    var content = await response.Content.ReadAsStringAsync();
                    entity = JsonConvert.DeserializeObject<T>(content);
                    response.EnsureSuccessStatusCode();
                }
            }
            return entity;
          
        }
        private static HttpContent CreateHttpContent(object content)
        {
            HttpContent httpContent = null;

            if (content != null)
            {
                var ms = new MemoryStream();
                SerializeJsonIntoStream(content, ms);
                ms.Seek(0, SeekOrigin.Begin);
                httpContent = new StreamContent(ms);
                httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            }

            return httpContent;
        }
        public static void SerializeJsonIntoStream(object value, Stream stream)
        {
            using (var sw = new StreamWriter(stream, new UTF8Encoding(false), 1024, true))
            using (var jtw = new JsonTextWriter(sw) { Formatting = Formatting.None })
            {
                var js = new JsonSerializer();
                js.Serialize(jtw, value);
                jtw.Flush();
            }
        }
        public async Task<List<T>> DeleteEntity()
        {
            List<T> list = new List<T>();
            var uri = new Uri(string.Format(APIBaseUrl + "StoreList"));
            var response = await _client.GetAsync(uri);
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                list = JsonConvert.DeserializeObject<List<T>>(content);
            }
            return list;
        }
    }
}
