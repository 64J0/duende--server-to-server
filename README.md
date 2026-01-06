# [Duende] Protecting an API with client credentials

This is the [first quickstart project](https://docs.duendesoftware.com/identityserver/quickstarts/1-client-credentials) from the Duende website.

> This first quickstart provides step-by-step instructions to set up IdentityServer in the most basic scenario: protecting APIs for server-to-server communication. You will create a solution containing three projects:
>
> - An Identity Server
> - An API that requires authentication
> - A client that accesses that API
>
> The client will request an access token from IdentityServer using its client ID and secret and then use the token to gain access to the API.

*Id est*, in this example we have a machine-to-machine (or server-to-server) scenario, where two "machines" talk to each other, and there is no interactive user present. In protocol terminology this is called *Client Credentials Flow*.

## Client credentials flow

```mermaid
sequenceDiagram
    participant Client as Client<br>Console App
    participant IDP as Identity Provider<br>ASP.NET Core with<br>IdentityServer
    participant API as API<br>ASP.NET Core
    Client->>+IDP: Give me the access token (using client secret)
    IDP-->>-Client: Verified credentials and return the access token
    Client->>+API: Give me the protected data from the API using access token
    API-->>-Client: Here is the protected data
```

## How to run

> [!WARNING]
> This project is not properly configured for a production environment.
> Notice that the processes are using HTTP instead of HTTPS, and we're not using a strong secret.

1. Start the IDP server: `dotnet run --project src/IdentityProvider`
2. Start the API server: `dotnet run --project src/Api`
3. Use the Client: `dotnet run --project src/Client`

> [!NOTE]
> On first startup, IdentityServer will use its automatic key management feature to create a signing key and store it in the src/IdentityServer/keys directory. To avoid accidentally disclosing cryptographic secrets, the entire keys directory should be excluded from source control. It will be recreated if it is not present.
>
> --- [Protecting An API With Client Credentials](https://docs.duendesoftware.com/identityserver/quickstarts/1-client-credentials/#configuring-identityserver)

You'll get a response like:

```bash
$ dotnet run --project src/Client
Access Token retrieved from IDP: eyJhbGciOiJSUzI1NiIsImtpZCI6IjhERUIwQjFDMEMxN0YwOTIyNUQ2ODJBMjU3OUI0Rjk2IiwidHlwIjoiYXQrand0In0.eyJpc3MiOiJodHRwOi8vbG9jYWxob3N0OjUwMDEiLCJuYmYiOjE3NjcxMDY3NDcsImlhdCI6MTc2NzEwNjc0NywiZXhwIjoxNzY3MTEwMzQ3LCJzY29wZSI6WyJhcGkxIl0sImNsaWVudF9pZCI6ImNsaWVudCIsImp0aSI6IkQzNTVDOUU5REQxQUI3RDBDOTc4Mjg4REI3MDAwNUQ2In0.LhiQtNJeGCFlkhrCbRpsS0cI2oaHUCHFJtMmy082htgRyVA6tuw1GQNCpArjDx3rUkNmMlad-doiiH9vnZVPpTtM2DyxA-jFWilqPQjjVdMPnFgC6R2X1sf-19HWROB2sCMYf8mzTDfOULNqckrs-9E-kCU29co-tkShu-q2q3aFBif3g2xVX4QrNl5oidmRSH2f6BvR4lNviJHgJskav9rUBEJB6RTk3G7sipIGJ6AWVJRDLt-d4WraXRgLmMd38Kcsrg9hfwIHXp2vZoz4i1gPZrqq09xnqaiVhRNpz_Ec2a08HuZiYLx8kBKmCGglx1uPtfL44ZIsGo1xZoTMhA
[
  {
    "type": "iss",
    "value": "http://localhost:5001"
  },
  {
    "type": "nbf",
    "value": "1767106747"
  },
  {
    "type": "iat",
    "value": "1767106747"
  },
  {
    "type": "exp",
    "value": "1767110347"
  },
  {
    "type": "scope",
    "value": "api1"
  },
  {
    "type": "client_id",
    "value": "client"
  },
  {
    "type": "jti",
    "value": "D355C9E9DD1AB7D0C978288DB70005D6"
  }
]
```

## Checking the OpenID configuration

After starting the IDP server, you can find the OpenID configuration at:

- <https://localhost:5001/.well-known/openid-configuration>.

Which is known as the "discovery endpoint".
