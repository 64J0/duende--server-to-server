using System.Text.Json;
using Duende.IdentityModel.Client;

var client = new HttpClient();
var disco = await client.GetDiscoveryDocumentAsync("http://localhost:5001");

if (disco.IsError)
{
    Console.WriteLine($"Discovery endpoint error: {disco.Error}");
    Console.WriteLine($"Discovery endpoint exception: {disco.Exception}");
    return;
}

var tokenResponse = await client.RequestClientCredentialsTokenAsync(new ClientCredentialsTokenRequest
{
    Address = disco.TokenEndpoint,
    ClientId = "client",
    ClientSecret = "secret",
    Scope = "api1"
});

if (tokenResponse.IsError)
{
    Console.WriteLine($"Token response error: {tokenResponse.Error}");
    Console.WriteLine($"Token response error description: {tokenResponse.ErrorDescription}");
    return;
}

Console.WriteLine($"Access Token retrieved from IDP: {tokenResponse.AccessToken}");

var apiClient = new HttpClient();
apiClient.SetBearerToken(tokenResponse.AccessToken!);
var response = await apiClient.GetAsync("http://localhost:6001/identity");

if (!response.IsSuccessStatusCode)
{
    Console.WriteLine($"API response status code: {response.StatusCode}");
}
else
{
    // Pretty print the JSON response
    var doc = JsonDocument.Parse(await response.Content.ReadAsStringAsync());
    Console.WriteLine(JsonSerializer.Serialize(doc, new JsonSerializerOptions { WriteIndented = true }));
}