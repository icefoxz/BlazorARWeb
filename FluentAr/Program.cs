using ArLib;
using FluentAr;
using FluentAr.Services.QrSrv;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.FluentUI.AspNetCore.Components;
using NLog;
using Radzen;
using RazorComponents;
using LogLevel = NLog.LogLevel;

var logger = LogManager.Setup().LoadConfiguration(b =>
{
    b.ForLogger().FilterMinLevel(LogLevel.Info)
       .WriteToConsole(layout: "${longdate} ${level:uppercase=true} ${message} ${exception:format=tostring}")
       .WriteToFile("logs/logfile-${shortdate}.log",
            layout: "${longdate} ${level:uppercase=true} ${message} ${exception:format=tostring}");
}).GetCurrentClassLogger();

try
{
    var builder = WebApplication.CreateBuilder(args);

    // Add services to the container.
    builder.Services.AddRazorComponents()
       .AddInteractiveServerComponents();

    builder.Services.AddFluentUIComponents();

    builder.Services.AddRadzenComponents();
    builder.Services.AddRazorPages();

    // blazor authentication state
    builder.Services.AddCascadingAuthenticationState();

    //controllers
    builder.Services.AddControllers()
       .AddNewtonsoftJson();

    //signalR
    builder.Services.AddSignalR(o => o.MaximumReceiveMessageSize = 1024 * 1024 * 2);
    builder.Services.AddSessionStorageServices();

    builder.Services.AddTransient<QrService>();
    builder.Services.AddTransient<ArInterop>();
    builder.Services.AddTransient<JsInterop>();


    var app = builder.Build();

    // Configure the HTTP request pipeline.
    if (!app.Environment.IsDevelopment())
    {
        app.UseExceptionHandler("/Error", createScopeForErrors: true);
        // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
        app.UseHsts();
    }

    app.UseHttpsRedirection();
    // 创建 FileExtensionContentTypeProvider 并添加 .mind 文件类型
    var mindTypeProvider = new FileExtensionContentTypeProvider();
    mindTypeProvider.Mappings[".mind"] = "application/octet-stream";
    mindTypeProvider.Mappings[".gltf"] = "application/octet-stream";
    mindTypeProvider.Mappings[".glb"] = "application/octet-stream";
    app.UseStaticFiles(new StaticFileOptions { ContentTypeProvider = mindTypeProvider });
    app.UseAntiforgery();

    app.MapRazorPages();
    app.MapControllers();

    app.MapRazorComponents<App>()
       .AddInteractiveServerRenderMode();

    await app.RunAsync();
}
catch (Exception ex)
{
    logger.Error(ex, "Nlog: Stopped program because of exception");
    throw;
}
finally
{
    LogManager.Shutdown();
}