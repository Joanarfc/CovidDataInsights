using CDI.CovidApp.MVC.Extensions;
using CDI.CovidApp.MVC.Models;
using System.Text;
using System.Text.Json;

namespace CDI.CovidApp.MVC.Services
{
    public abstract class Service
    {
        protected StringContent GetContent(object data)
        {
            return new StringContent(
                JsonSerializer.Serialize(data),
                Encoding.UTF8,
                "application/json");
        }
        protected async Task<T> DeserializeObjectResponse<T>(HttpResponseMessage responseMessage)
        {
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            return JsonSerializer.Deserialize<T>(await responseMessage.Content.ReadAsStringAsync(), options);
        }
        protected bool HandleReponseErrors(HttpResponseMessage responseMessage)
        {
            switch((int)responseMessage.StatusCode) 
            {
                case 401:
                case 403:
                case 404:
                case 500:
                    throw new CustomHttpRequestException(responseMessage.StatusCode);
                case 400:
                    return false;
            }

            responseMessage.EnsureSuccessStatusCode();
            return true;
        }
        protected ResponseResultViewModel ReturnOk()
        {
            return new ResponseResultViewModel();
        }
    }
}