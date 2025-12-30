using System.Security.Claims;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAuthentication()
    .AddJwtBearer(options =>
    {
        options.Authority = "http://localhost:5001";
        // https://docs.duendesoftware.com/identityserver/apis/aspnetcore/jwt/#adding-audience-validation
        options.TokenValidationParameters.ValidateAudience = false;
        options.Audience = "api1";
        options.RequireHttpsMetadata = false;
    });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("ApiScope", policy =>
    {
        policy.RequireAuthenticatedUser();
        policy.RequireClaim("scope", "api1");
    });
});

var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.MapGet("/identity", (ClaimsPrincipal user) =>
{
    return user.Claims.Select(c => new { c.Type, c.Value });
}).RequireAuthorization("ApiScope");

app.Run();
