using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Anil.Core.Configuration;
using Anil.Core.Infrastructure;
using Anil.Web.Framework.Infrastructure.Extensions;
using Microsoft.Extensions.DependencyInjection;
using CKSource.CKFinder.Connector.Config;
using CKSource.FileSystem.Local;

var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

var builder = WebApplication.CreateBuilder(args);


builder.Configuration.AddJsonFile(AnilConfigurationDefaults.AppSettingsFilePath, true, true);
if (!string.IsNullOrEmpty(builder.Environment?.EnvironmentName))
{
    var path = string.Format(AnilConfigurationDefaults.AppSettingsEnvironmentFilePath, builder.Environment.EnvironmentName);
    builder.Configuration.AddJsonFile(path, true, true);
}
builder.Configuration.AddEnvironmentVariables();
builder.Services.AddCors();
//load application settings
builder.Services.ConfigureApplicationSettings(builder);

var appSettings = Singleton<AppSettings>.Instance;
var useAutofac = appSettings.Get<CommonConfig>().UseAutofac;

if (useAutofac)
    builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
else
    builder.Host.UseDefaultServiceProvider(options =>
    {
        //we don't validate the scopes, since at the app start and the initial configuration we need 
        //to resolve some services (registered as "scoped") through the root container
        options.ValidateScopes = false;
        options.ValidateOnBuild = true;
    });

//add services to the application and configure service provider
builder.Services.ConfigureApplicationServices(builder);

var app = builder.Build();
app.UseCors(x => x.WithOrigins("http://localhost:4200", "https://cms.hamrah-teb.com")
                .AllowAnyMethod()
                .AllowAnyHeader());
app.UseHttpsRedirection();

//configure the application HTTP request pipeline
app.ConfigureRequestPipeline();
app.StartEngine();

FileSystemFactory.RegisterFileSystem<LocalStorage>();
app.Map("/ckfinder/connector", app.SetupConnector());
app.Run();