using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Identity.Web;
using Azure.Identity;
using FileTransferService.Spa.Hosted.Server.Services;

var builder = WebApplication.CreateBuilder(args);

string? uploadAppConfigurationConnString = builder.Configuration.GetSection("UploadAppConfigurationConnString").Value;
if (uploadAppConfigurationConnString != null)
{
    builder.Configuration.AddAzureAppConfiguration(options =>
    {
        options.Connect(uploadAppConfigurationConnString)
            .ConfigureKeyVault(kv =>
            {
                kv.SetCredential(new DefaultAzureCredential(
                        new DefaultAzureCredentialOptions
                        {
                            AuthorityHost = AzureAuthorityHosts.AzureGovernment
                        }
                    ));
            });
    });
}

string? eventAppConfigurationConnString = builder.Configuration.GetSection("EventAppConfigurationConnString").Value;
if (eventAppConfigurationConnString != null)
{
    builder.Configuration.AddAzureAppConfiguration(options =>
    {
        options.Connect(eventAppConfigurationConnString)
            .ConfigureKeyVault(kv =>
            {
                kv.SetCredential(new DefaultAzureCredential(
                        new DefaultAzureCredentialOptions
                        {
                            AuthorityHost = AzureAuthorityHosts.AzureGovernment
                        }
                    ));
            });
    });
}

// Add services to the container.
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddMicrosoftIdentityWebApi(builder.Configuration.GetSection("AzureAd"));

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

builder.Services.AddSingleton<IEventsService, EventsService>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", builder =>
    {
        builder.AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseBlazorFrameworkFiles();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();


app.MapRazorPages();
app.MapControllers();
app.MapFallbackToFile("index.html");

app.Run();
