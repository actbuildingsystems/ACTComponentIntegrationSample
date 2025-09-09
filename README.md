# ACT Component Pricing API Sample

## Authentication

An OpenId client credentials token (as per [OpenId Spec](https://openid.net/specs/openid-connect-core-1_0.html#TokenEndpoint)) must be requested from the ACT OpenId service and included as a bearer token in the request authorization header for all calls to the ACT Pricing API.

This request can be made as follows, this assumes the `clientId` and `clientSecret` have been provided by ACT:

- Make a GET request to https://identity.actbuildingsystems.com/.well-known/openid-configuration
- Parse the `token_endpoint` url from the response
- Make a POST request to the `token_endpoint` url with the following headers
- - `Content-Type: application/x-www-form-urlencoded`
- - `Authorization: Basic <Base64Encoded(clientId:clientSecret)>`
- - Body data-urlencode: `grant_type=client_credentials`
- - body data-urlencode: `scope=actComponentService.clientComponentApi`
- Store the response and parse the `access_token` use the `expires_in` to cache the token.

If using C# then the nuget package `IdentityModel` simplifies this and is what's used in the ACT example code. Any OpenId client package is also relevant.

The example in this repository, uses C# .Net Core 6 and `IdentityModel`, however this is not a requirement for integration with ACT services. 

## Available Endpoints

-  GET/PUT `/clientcomponents/{clientId}/{?priceCategory}`

### HTTP Verb GET `/clientcomponents/{clientId}/{?priceCategory}`


Requests must always be made with a valid ClientId. The `priceCategory` is optional and should only be used with clients who have price categories defined in ACT.
This endpoint will return an array `[]` of `ClientComponent` models, this is the current state of the Client pricing information.


### HTTP Verb PUT `/clientcomponents/{clientId}/{?priceCategory}`

Requests must always be made with a valid ClientId. The `priceCategory` is optional and should only be used with clients who have price categories defined in ACT.
The ContentType should be `application/json`, with the body content being an array `[]` of `ClientComponent`. The expectation is that this endpoint is called only with items that **require** updating as each call to this endpoint will increment the client version information.

## External Integrations

The intended workflow for a ACT client to integrate is as follows:

- Request an authentication token
- Perform a GET to `/clientcomponents/{clientId}/{?priceCategory}`
- Compare the response with the Client's own information and create an array of `ClientComponent` that only contains the items to be updated
- **If required** perform a PUT to `/clientcomponents/{clientId}/{?priceCategory}` with the array of differences


## Postman Example

A Postman collection example can be found [ACTComponentIntegrationSample.postman_collection.json](ACTComponentIntegrationSample.postman_collection.json) to use it, import it into Postman, and then set the collection variables:

* ClientId (this is your 3 character ACT clientId)
* ClientSecret (this can be provided on request by ACT and is unique to your ACT client, and should be kept safe)
* ClientUserName (this can be provided on request by ACT and is unique to your ACT client)

Running `GetAuthToken` will set the variable `AccessToken` provided the `ClientSecret` and `ClientUserName` are correct.
Once you have issued a successful `GetAuthToken` request, running `GetClientComponents` will retrieve the components for your `ClientId` optionally, you can add `?priceCategory=` if you know and use ACT price categories. When a successful request of `GetClientComponents` is made the variable `ClientComponents` will also be set, and will be available for the final request, which is `UpdateClientComponents` running this without change will result in no changes to your client components, however, if you update any properties they will be reflected in a subsequent call to `GetClientComponents`.


## Models

### ClientComponent

Describes a single client component price item.

```
    public class ClientComponent
    {
        public int? Id { get; set; }
        public List<ColorItem> AvailableColors { get; set; }	
        public List<string> AvailableRegions { get; set; }	
        public string? BendAngleDegrees1Range { get; set; }	
        public string? BendAngleDegrees2Range { get; set; }	
        public double CostPerPunchEach { get; set; }	
        public double CostPerPunchSetup { get; set; }	
        public double CostPerScoreEach { get; set; }	
        public double CostPerUnit { get; set; }	
        public string? DistributorInfo { get; set; }	
        public double MaxLength { get; set; }	
        public double MinLength { get; set; }	
        public double MinLengthShortCuttingCharge { get; set; }	
        public bool PieceMarkAllowed { get; set; }	
        public string? ProductCode { get; set; }	
        public string? ProductCodeSecondary { get; set; }	
        public int ProductId { get; set; }	
        public int PunchingAllowed { get; set; }	
        public bool ScoringAllowed { get; set; }	
        public double ShortCuttingCharge { get; set; }	
        public double StandardLength { get; set; }	
        public int StandardQuantity { get; set; }	
        public string? StockLengths { get; set; }	
        public string? Supplier { get; set; }	
        public string? SupplierDescription { get; set; }	
        public string? SupplierInternal { get; set; }	
        public string? Unit { get; set; }	
        public string? VariationDescription { get; set; }	
        public double WeightPerUnit { get; set; }
    }    
```

### ClientComponent

Describes a single color.

```
    public class ColorItem
    {
        public string ColorDescription { get; set; }
        public string? ColorProductCodeModifier { get; set; }
    }
```