using ArLib.Components;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.JSInterop;

namespace ArLib;

// This class provides an example of how JavaScript functionality can be wrapped
// in a .NET class for easy consumption. The associated JavaScript module is
// loaded on demand when first needed.
//
// This class can be registered as scoped DI service and then injected into Blazor
// components for use.

public class ArInterop : IAsyncDisposable
{
    readonly Lazy<Task<IJSObjectReference>> moduleTask;
    readonly Lazy<Task<IJSObjectReference>> compilerTask;
    async Task ModuleInvokeAsync(string identifier,params object[] param)
    {
        var module = await moduleTask.Value;
        await module.InvokeVoidAsync(identifier, param);
    }    
    async Task<T> ModuleInvokeAsync<T>(string identifier,params object[] param)
    {
        var module = await moduleTask.Value;
        return await module.InvokeAsync<T>(identifier, param);
    }
    public ArInterop(IJSRuntime jsRuntime)
    {
        moduleTask = new(() => jsRuntime.InvokeAsync<IJSObjectReference>(
            "import", "./_content/ArLib/ar.lib.module.js").AsTask());
        compilerTask = new(() => jsRuntime.InvokeAsync<IJSObjectReference>(
            "import", "./_content/ArLib/mindar-compiler.js").AsTask());
    }
    public async Task<byte[]> GenerateMindFileAsync(ElementReference inputFile, DotNetObjectReference<GenMind> objRef)
    {
        var module = await compilerTask.Value;
        return await module.InvokeAsync<byte[]>("generateMindFileWithProgress", inputFile, objRef);
    }
    public async Task<int> GetFileCount(ElementReference inputFile)
    {
        var module = await compilerTask.Value;
        return await module.InvokeAsync<int>("getFileCount", inputFile);
    }
    public async Task DownloadMindFile(string dataUrl)
    {
        var module = await compilerTask.Value;
        await module.InvokeVoidAsync("downloadMindFile", dataUrl);
    }
    public async Task InitializeImagePreview()
    {
        var module = await compilerTask.Value;
        await module.InvokeVoidAsync("initializeImagePreview");
    }

    public async Task Prompt(string message)=> await ModuleInvokeAsync("showPrompt", message);
    public async ValueTask DisposeAsync()
    {
        if (compilerTask.IsValueCreated)
        {
            var compiler = await compilerTask.Value;
            await compiler.DisposeAsync();
        }
        if (moduleTask.IsValueCreated)
        {
            var module = await moduleTask.Value;
            await module.DisposeAsync();
        }
    }
    public async Task ArStart() => await ModuleInvokeAsync("startAR");
    public async Task ArStop() => await ModuleInvokeAsync("stopAR");
    public async Task InitAr(DotNetObjectReference<AScene> obj) => 
        await ModuleInvokeAsync("initAR", obj);
    public async Task RegClickable(string id, DotNetObjectReference<AObject> aRef) =>
        await ModuleInvokeAsync("regClickable", id, aRef);
    public async Task<VideoStatus> VideoPlay(string videoId) => await ModuleInvokeAsync<VideoStatus>("videoPlay", videoId);
    public async Task<VideoStatus> VideoPause(string videoId) => await ModuleInvokeAsync<VideoStatus>("videoPause", videoId);
    public async Task ASoundPlay(string id) => await ModuleInvokeAsync("aSoundPlay", id);
    public async Task ASoundPause(string id) => await ModuleInvokeAsync("aSoundPause", id);
    public async Task ASoundStop(string id) => await ModuleInvokeAsync("aSoundStop", id);
    public async Task SoundPlay(string id) => await ModuleInvokeAsync("soundPlay", id);
    public async Task SoundStop(string id) => await ModuleInvokeAsync("soundStop", id);
}