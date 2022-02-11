using Acr.UserDialogs;
using MTNews.Helpers;
using MTNews.Models;
using Newtonsoft.Json;
using Plugin.Connectivity;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace MTNews.Services
{
    public class BaseWebService
    {
        private readonly AsyncLock asyncLock = new AsyncLock();

        HttpClient client;

        static string BaseUrL = Constants.BASE_URL + "api";
        protected static Uri BaseUri = new Uri(BaseUrL);
        protected static Uri BaseLoginUri = new Uri(Constants.Authenticate_URL);

        public BaseWebService()
        {
            client = HttpClientService.Instance.HttpClient;
        }

        protected async Task DeleteAsync(string url)
        {
            try
            {
                //await SetHeaders();
                if (CrossConnectivity.Current.IsConnected)
                {
                    var response = await client.DeleteAsync(url);

                    var responseContent = response.Content;
                    var responseString = await responseContent.ReadAsStringAsync();

                    if (!response.IsSuccessStatusCode)
                    {
                        //throw new HttpResponseException(response.StatusCode, responseString);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SendErrorLog(ex);
            }

        }

        protected async Task PutAsync(string url, string content)
        {
            try
            {
                //await SetHeaders();
                if (CrossConnectivity.Current.IsConnected)
                {
                    var response = await client.PutAsync(url, new StringContent(content, Encoding.UTF8, "application/json"));

                    var responseContent = response.Content;
                    var responseString = await responseContent.ReadAsStringAsync();

                    if (!response.IsSuccessStatusCode)
                    {
                        //throw new HttpResponseException(response.StatusCode, responseString);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SendErrorLog(ex);
            }

        }

        protected async Task<string> PostAsync(string url, string content)
        {
            var responseString = "";
            try
            {
                if (CrossConnectivity.Current.IsConnected)
                {
                    var client = new RestClient(BaseUri + url);
                    var request = new RestRequest(Method.POST);
                    request.AddHeader("content-type", "application/json");
                    request.AddHeader("accept", "*/*");
                    request.AddParameter("application/json", content, ParameterType.RequestBody);

                    IRestResponse response = await client.ExecuteAsync(request);

                    if (response != null)
                    {
                        return response.Content;
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SendErrorLog(ex);
            }

            return responseString;
        }

        protected async Task<T> PostAsync<T>(string url, string content)
        {
            var responseString = "";
            try
            {
                if (CrossConnectivity.Current.IsConnected)
                {
                    //await SetHeaders();

                    var response = await client.PostAsync(url, new StringContent(content, Encoding.UTF8, "application/json"));

                    var responseContent = response.Content;
                    responseString = await responseContent.ReadAsStringAsync();

                    if (!response.IsSuccessStatusCode)
                    {
                        //throw new HttpResponseException(response.StatusCode, responseString);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SendErrorLog(ex);
            }
            return JsonConvert.DeserializeObject<T>(responseString);
        }

        protected async Task<string> PostMultipartAsync(string uri, List<KeyValuePair<string, string>> arguments, bool hasHeader = true)
        {
            if (CrossConnectivity.Current.IsConnected)
            {
                using (await asyncLock.LockAsync())
                {
                    var request = new HttpRequestMessage
                    {
                        RequestUri = new Uri(BaseUri, uri),
                        Method = HttpMethod.Post
                    };

                    HttpResponseMessage response = null;
                    try
                    {
                        //client.MaxResponseContentBufferSize = 500000;
                        var content = new MultipartFormDataContent();

                        foreach (var keyValuePair in arguments)
                            content.Add(new StringContent(keyValuePair.Value), string.Format("\"{0}\"", keyValuePair.Key));
                        request.Content = content;
                        response = await client.SendAsync(request);
                    }
                    catch (WebException ex)
                    {
                        HttpWebResponse httpWebResponse = (HttpWebResponse)ex.Response;
                        String details = "NONE";
                        String statusCode = "NONE";
                        if (httpWebResponse != null)
                        {
                            details = httpWebResponse.StatusDescription;
                            statusCode = httpWebResponse.StatusCode.ToString();
                        }
                    }
                    catch (Exception ex)
                    {
                        Logger.SendErrorLog(ex);
                    }


                    return response.Content.ReadAsStringAsync().Result;
                }
            }

            return null;
        }

        protected async Task<string> PostMultipartAsync(string uri, List<KeyValuePair<string, string>> arguments, Stream fileStream, string fileParameterName, string fileName, bool isAuthorized = false)
        {
            if (CrossConnectivity.Current.IsConnected)
            {
                using (await asyncLock.LockAsync())
                {
                    var request = new HttpRequestMessage
                    {
                        RequestUri = new Uri(BaseUri, uri),
                        Method = HttpMethod.Post
                    };

                    //if (SettingsService.HasRegistered)
                    //{
                    //    request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", SettingsService.AuthenticationToken);
                    //}
                    HttpResponseMessage response = null;
                    try
                    {
                        var content = new MultipartFormDataContent();

                        content.Add(new StreamContent(fileStream), fileParameterName, fileName);


                        foreach (var keyValuePair in arguments)
                            content.Add(new StringContent(keyValuePair.Value), string.Format("\"{0}\"", keyValuePair.Key));
                        request.Content = content;
                        response = await client.SendAsync(request);
                    }
                    catch (Exception ex)
                    {
                        Logger.SendErrorLog(ex);
                        //throw new Exception(ex.Message);
                    }

                    return response.Content.ReadAsStringAsync().Result;
                }
            }

            return null;
        }
        protected async Task<string> UploadFile(byte[] fileData)
        {
            try
            {
                if (CrossConnectivity.Current.IsConnected)
                {
                    using (await asyncLock.LockAsync())
                    {
                        HttpClient client = new HttpClient();
                        MultipartFormDataContent content = new MultipartFormDataContent();
                        var memoryStream = new MemoryStream(fileData);

                        content.Add(new StringContent(Convert.ToBase64String(fileData)), "file");

                        var response = await client.PostAsync("yourwebservice/TransferFile", content);

                        var responseContent = response.Content;
                        var responseString = await responseContent.ReadAsStringAsync();

                        if (!response.IsSuccessStatusCode)
                        {
                            return null;
                            //throw new HttpResponseException(response.StatusCode, responseString);
                        }
                        else
                        {
                            return responseString;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SendErrorLog(ex);
                return null;
            }
            return null;
        }

        protected async Task<string> GetAsync(string url)
        {
            try
            {
                if (CrossConnectivity.Current.IsConnected)
                {
                    var client = new RestClient(BaseUri + url);
                    var request = new RestRequest(Method.GET);
                    IRestResponse response = await client.ExecuteAsync(request);

                    if (response != null)
                    {
                        return response.Content;
                    }
                }
            }
            catch (TaskCanceledException ex)
            {
                var properties = new Dictionary<string, string> { { "TaskCanceledException :: url", url } };
                Logger.SendErrorLog(ex);
            }
            catch (HttpRequestException ex)
            {
                var properties = new Dictionary<string, string> { { "HttpRequestException :: url", url } };
                Logger.SendErrorLog(ex);
            }
            catch (Exception ex)
            {
                var properties = new Dictionary<string, string> { { "Exception :: url", url } };
                Logger.SendErrorLog(ex);
            }
            return null;
        }


        public async Task<LoginResponse> AuthorizationAsync(LoginRequest req)
        {
            try
            {
                if (CrossConnectivity.Current.IsConnected)
                {

                    var client = new RestClient(Constants.Authenticate_URL);
                    client.Timeout = -1;
                    var request = new RestRequest(Method.POST);
                    request.AddHeader("Content-Type", "application/x-www-form-urlencoded");
                    request.AddHeader("Cookie", "ASP.NET_SessionId=v15wbl0y0xfsvp2zxhunzop5");
                    request.AddParameter("userName", req.userName);
                    request.AddParameter("password", req.password);
                    IRestResponse response = await client.ExecuteAsync(request);

                    var datas = XMLToJson.XmlToJSON(response.Content);
                    if(datas!=null)
                    {
                        var res = JsonConvert.DeserializeObject<LoginStringRoot>(datas);

                        if (res != null && res.@string != null && !string.IsNullOrEmpty(res.@string.value))
                        {
                            var loginRes = JsonConvert.DeserializeObject<LoginResponse>(res.@string.value);

                            return loginRes;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SendErrorLog(ex);
            }
            return null;
        }

        private static Dictionary<string, object> GetXmlData(XElement xml)
        {
            var attr = xml.Attributes().ToDictionary(d => d.Name.LocalName, d => (object)d.Value);
            if (xml.HasElements) attr.Add("_value", xml.Elements().Select(e => GetXmlData(e)));
            else if (!xml.IsEmpty) attr.Add("_value", xml.Value);

            return new Dictionary<string, object> { { xml.Name.LocalName, attr } };
        }
    }

    public class HttpResponseException : Exception
    {
        public HttpStatusCode StatusCode { get; private set; }

        public HttpResponseException(HttpStatusCode statusCode, string content) : base(content)
        {
            StatusCode = statusCode;
        }
    }
}
