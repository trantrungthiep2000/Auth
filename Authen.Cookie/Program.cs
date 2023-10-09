using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// authen
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme; // cookies
    options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme; // cookies

    //options.DefaultAuthenticateScheme = "second-cookies"; // second-cookies
    //options.DefaultChallengeScheme = "second-cookies"; // second-cookies
}).AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>
{
    options.Cookie = new CookieBuilder()
    {
        Name = "AccessTokenCookie",
    };
    options.LoginPath = "/api/authens/GetUnauthorized";
    options.LogoutPath = "/api/authens/GetLogout";
    options.AccessDeniedPath = "/api/authens/GetForbidden";

    options.Events.OnValidatePrincipal = (context) =>
    {
        return Task.CompletedTask;
    };
});
//.AddCookie("second-cookies", options =>
//{
//    options.LoginPath = "/api/authens/GetUnauthorizedV2";

//    options.Events.OnValidatePrincipal = (context) =>
//    {
//        return Task.CompletedTask;
//    };
//});

//builder.Services.AddAuthorization(options =>
//{
//    var defaultAuthorizationPolicyBuilder = new AuthorizationPolicyBuilder(CookieAuthenticationDefaults.AuthenticationScheme, "second-cookies");

//    defaultAuthorizationPolicyBuilder = defaultAuthorizationPolicyBuilder.RequireAuthenticatedUser();
//    options.DefaultPolicy = defaultAuthorizationPolicyBuilder.Build();
//});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
