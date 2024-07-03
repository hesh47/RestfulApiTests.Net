using System;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;
using System.Text.Json;

public class RestApiTests
{
    private readonly HttpClient _httpClient;
    private string _baseUrl = "https://api.restful-api.dev/objects";

    public RestApiTests()
    {
        _httpClient = new HttpClient { BaseAddress = new Uri(_baseUrl) };
    }

    [Fact]
    public async Task Test_GetListOfAllObjects()
    {
        
        var request = new HttpRequestMessage(HttpMethod.Get, _baseUrl);

        var response = await _httpClient.SendAsync(request);
        var responseContent = await response.Content.ReadAsStringAsync();

        response.EnsureSuccessStatusCode(); 
        Assert.NotEmpty(responseContent); 

    }

    [Fact]
    public async Task Test_AddObjectUsingPost()
    {
        var newObject = new
        {
            id = 20,
            name = "iPhone 15 Pro",
            data = new
            {
                color = "Black",
                capacity = "128 GB"
            }
        };
        var jsonContent = JsonSerializer.Serialize(newObject);
        var httpContent = new StringContent(jsonContent, System.Text.Encoding.UTF8, "application/json");

        var response = await _httpClient.PostAsync(_baseUrl, httpContent);
        var responseContent = await response.Content.ReadAsStringAsync();

        response.EnsureSuccessStatusCode(); 

        using JsonDocument document = JsonDocument.Parse(responseContent);
        JsonElement root = document.RootElement;
        string addedObjectId = root.GetProperty("id").GetString();

        Assert.NotEmpty(addedObjectId);

    }

    [Fact]
    public async Task Test_GetSingleObjectUsingAddedId()
    {
        var newObject = new
        {
            id = 20,
            name = "iPhone 15 Pro",
            data = new
            {
                color = "Black",
                capacity = "128 GB"
            }
        };
        var jsonContent = JsonSerializer.Serialize(newObject);
        var httpContent = new StringContent(jsonContent, System.Text.Encoding.UTF8, "application/json");

        var postResponse = await _httpClient.PostAsync(_baseUrl, httpContent);
        var postResponseContent = await postResponse.Content.ReadAsStringAsync();

        using JsonDocument document = JsonDocument.Parse(postResponseContent);
        JsonElement root = document.RootElement;
        string addedObjectId = root.GetProperty("id").GetString();

        var getRequest = new HttpRequestMessage(HttpMethod.Get, _baseUrl + "/" + addedObjectId);
        var getResponse = await _httpClient.SendAsync(getRequest);
        var getResponseContent = await getResponse.Content.ReadAsStringAsync();

        getResponse.EnsureSuccessStatusCode();
        Assert.NotEmpty(getResponseContent); 

    }

    [Fact]
    public async Task Test_UpdateObjectUsingPut()
    {
        var newObject = new
        {
            id = 20,
            name = "iPhone 15 Pro",
            data = new
            {
                color = "Black",
                capacity = "128 GB"
            }
        };
        var jsonContent = JsonSerializer.Serialize(newObject);
        var httpContent = new StringContent(jsonContent, System.Text.Encoding.UTF8, "application/json");

        var postResponse = await _httpClient.PostAsync(_baseUrl, httpContent);
        var postResponseContent = await postResponse.Content.ReadAsStringAsync();

        using JsonDocument document = JsonDocument.Parse(postResponseContent);
        JsonElement root = document.RootElement;
        string addedObjectId = root.GetProperty("id").GetString();

        var updatedObject = new
        {
            id = 21,
            name = "iPhone 15 Pro Max",
            data = new
            {
                color = "Silver",
                capacity = "256 GB"
            }
        };
        var updatedJsonContent = JsonSerializer.Serialize(updatedObject);
        var updatedHttpContent = new StringContent(updatedJsonContent, System.Text.Encoding.UTF8, "application/json");

        var putRequest = new HttpRequestMessage(HttpMethod.Put, _baseUrl + "/" + addedObjectId);
        putRequest.Content = updatedHttpContent;
        var putResponse = await _httpClient.SendAsync(putRequest);
        var putResponseContent = await putResponse.Content.ReadAsStringAsync();

        putResponse.EnsureSuccessStatusCode();
        Assert.NotEmpty(putResponseContent); 

    }

    [Fact]
    public async Task Test_DeleteObjectUsingDelete()
    {
        var newObject = new
        {
            id = 20,
            name = "iPhone 15 Pro",
            data = new
            {
                color = "Black",
                capacity = "128 GB"
            }
        };
        var jsonContent = JsonSerializer.Serialize(newObject);
        var httpContent = new StringContent(jsonContent, System.Text.Encoding.UTF8, "application/json");

        var postResponse = await _httpClient.PostAsync(_baseUrl, httpContent);
        var postResponseContent = await postResponse.Content.ReadAsStringAsync();

        using JsonDocument document = JsonDocument.Parse(postResponseContent);
        JsonElement root = document.RootElement;
        string addedObjectId = root.GetProperty("id").GetString();

        var deleteRequest = new HttpRequestMessage(HttpMethod.Delete, _baseUrl + "/" + addedObjectId);
        var deleteResponse = await _httpClient.SendAsync(deleteRequest);

        deleteResponse.EnsureSuccessStatusCode(); 
    }

}
