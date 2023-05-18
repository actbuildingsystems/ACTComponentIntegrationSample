using System.Net.Http.Json;
using Act.ComponentIntegrationSample.Console.Models;
using IdentityModel.Client;

// For this example we'll demonstrate authentication, fetching and updating client components 

var identityClientId = "";
var identityClientSecret = "";
var actClientId = "";
var idsToUpdate = new List<int?>() {3144,  16600};

using var identityHttpClient = new HttpClient
{
    BaseAddress = new Uri("https://identity-dev.actbuildingsystems.com")
};
using var componentHttpClient = new HttpClient
{
    BaseAddress = new Uri("https://component-service-dev.actbuildingsystems.com")
};

// authenticate
await SetActBearerToken(identityHttpClient, componentHttpClient, identityClientId, identityClientSecret);

// demonstrate get 
var clientComponents = await GetComponents(componentHttpClient, actClientId);

Console.WriteLine($"Found {clientComponents.Count} components for {actClientId}");

// An integrator would now compare the ACT client components against their own data
// then create a set of client components that require updating, for example,
// for this example we'll use some pre-defined ids `idsToUpdate`
if (clientComponents.Any())
{
    var updateSet = clientComponents.Where(x => idsToUpdate.Contains(x.Id) ).ToList();
    
    Console.WriteLine("Current prices pre-update");

    // update the price, just increment by 1
    foreach (var clientComponent in updateSet)
    {
        Console.WriteLine(clientComponent);
        clientComponent.CostPerUnit++;
    }

    // demonstrate update
    await UpdateComponents(componentHttpClient, actClientId, updateSet);
}

// verify the update by calling the get again, an integrator wouldn't need to perform this step
var updatedClientComponents = (await GetComponents(componentHttpClient, actClientId))
    .Where(x => idsToUpdate.Contains(x.Id)).ToList();
Console.WriteLine("Updates prices");
updatedClientComponents.ForEach(Console.WriteLine);

Console.WriteLine("Done!");



static async Task<List<ClientComponent>> GetComponents(HttpClient componentHttpClient, string clientId)
{
    // Optional if client uses price categories /{?priceCategory}
    return await componentHttpClient
        .GetFromJsonAsync<List<ClientComponent>>($"/clientcomponents/{clientId}") ?? new List<ClientComponent>();
}

static async Task UpdateComponents(HttpClient componentHttpClient, string clientId, 
    List<ClientComponent> updatedClientComponents)
{
    // Optional if client uses price categories /{?priceCategory}
    await componentHttpClient
        .PutAsJsonAsync($"/clientcomponents/{clientId}", updatedClientComponents);
}

static async Task SetActBearerToken(
    HttpClient identityHttpClient, 
    HttpClient componentClient, 
    string clientId, string clientSecret)
{

    var disco = await identityHttpClient.GetDiscoveryDocumentAsync();
    if (disco.IsError)
    {
        throw new ApplicationException($"While contacting Identity Server {disco.Error}");
    }

    var tokenEndpoint = disco.TokenEndpoint;

    var tokenResponse = await identityHttpClient.RequestClientCredentialsTokenAsync(
        new ClientCredentialsTokenRequest
        {
            Address = tokenEndpoint,
            ClientId = clientId,
            ClientSecret = clientSecret,
            Scope = "actComponentService.clientComponentApi"
        });

    if (tokenResponse.IsError)
    {
        var error = tokenResponse.Error;
        throw new ApplicationException($"While retrieving Identity Token {error}");
    }

    componentClient.SetBearerToken(tokenResponse.AccessToken);
}
