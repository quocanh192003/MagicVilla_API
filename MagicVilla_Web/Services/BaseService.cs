﻿using MagicVilla_Utility;
using MagicVilla_Web.Models;
using MagicVilla_Web.Services.IServices;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace MagicVilla_Web.Services
{
    public class BaseService : IBaseService
    {
        public APIResponse responseModel { get; set; }
        public IHttpClientFactory httpClient { get; set; }
        public BaseService(IHttpClientFactory httpClient)
        {
            this.responseModel = new ();
            this.httpClient = httpClient;
        }
        public async Task<T> SendAsync<T> (APIRequest apiRequest)
        {
            try
            {
                var client = httpClient.CreateClient("MagicAPI");
                HttpRequestMessage message = new HttpRequestMessage();
                message.Headers.Add("Accept", "application/json");
                message.RequestUri = new Uri(apiRequest.Url);
                if(apiRequest.data != null){
                    message.Content = new StringContent(JsonConvert.SerializeObject(apiRequest.data),
                        Encoding.UTF8, "application/json"
                        );
                        
                }
                switch (apiRequest.apiType)
                {
                    case SD.ApiType.POST:
                        message.Method = HttpMethod.Post;
                        break;
                    case SD.ApiType.DELETE:
                        message.Method = HttpMethod.Delete;
                        break;
                    case SD.ApiType.PUT:
                        message.Method = HttpMethod.Put;
                        break;
                    default:
                        message.Method = HttpMethod.Get;
                        break;
                }
                HttpResponseMessage apiresponse = null;
                if (!string.IsNullOrEmpty(apiRequest.Token))
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiRequest.Token);
                }
                apiresponse = await client.SendAsync(message);
                var apicontent = await apiresponse.Content.ReadAsStringAsync();
                try
                {
                    APIResponse ApiResponse = JsonConvert.DeserializeObject<APIResponse>(apicontent);
                    if(apiresponse.StatusCode == HttpStatusCode.BadRequest 
                        || apiresponse.StatusCode == HttpStatusCode.NotFound)
                    {
						ApiResponse.Status = HttpStatusCode.BadRequest;
						ApiResponse.IsSuccess = false;
						var res = JsonConvert.SerializeObject(ApiResponse);
						var returnObj = JsonConvert.DeserializeObject<T>(res);
						return returnObj;
					}
                }
                catch (Exception ex)
                {
					var excep = JsonConvert.DeserializeObject<T>(apicontent);
					return excep;
				}

                var APIResponse = JsonConvert.DeserializeObject<T>(apicontent);
                return APIResponse;
            }
            catch (Exception ex)
            {
                var dto = new APIResponse{
                    ErrorMessages = new List<string>{Convert.ToString(ex.Message)},
                    IsSuccess = false
                };
                var res = JsonConvert.SerializeObject(dto);
                var APIResponse = JsonConvert.DeserializeObject<T>(res);
                return APIResponse;
            }

            
        }
    }
}
