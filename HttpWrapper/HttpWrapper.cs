using System.Net.Http.Headers;
using System.Text;

namespace HttpWrapper;

public class HttpWrapper : IHttpWrapper
{
    public async Task<string> GetWithBasicAuthentication(string baseAddress, string requestUri, string username, string password)
    {
        var httpClient = new HttpClient
        {
            BaseAddress = new Uri(baseAddress)
        };

        httpClient.DefaultRequestHeaders.Add("Authorization", $"Basic {Base64Encode($"{username}:{password}")}");

        httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        var response = await httpClient.GetAsync(requestUri);
        var responseContent = await response.Content.ReadAsStringAsync();

        return responseContent;
    }

    private string Base64Encode(string textToEncode)
    {
        var textAsBytes = Encoding.GetEncoding("ISO-8859-1").GetBytes(textToEncode);
        return Convert.ToBase64String(textAsBytes);
    }

    public async Task<string> PostWithBasicAuthentication(string baseAddress, string requestUri, string body, string username, string password)
    {
        var httpClient = new HttpClient
        {
            BaseAddress = new Uri(baseAddress)
        };

        httpClient.DefaultRequestHeaders.Add("Authorization", $"Basic {Base64Encode($"{username}:{password}")}");

        httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        var response = await httpClient.PostAsync(requestUri, new StringContent(body));
        var responseContent = await response.Content.ReadAsStringAsync();

        return responseContent;
    }
}
