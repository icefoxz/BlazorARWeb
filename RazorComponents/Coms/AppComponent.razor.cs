using Microsoft.AspNetCore.Components;

namespace RazorComponents.Coms;

public partial class AppComponent : ComponentBase,IDisposable
{
    protected bool IsLoading { get; set; }
    protected bool LoadOnce { get; set; }
    protected virtual Func<WebSite.WindowSize, Task>? Js_OnScreenSizeChanged { get; }
    [Inject] JsInterop Js { get; set; } = default!;
    // this method is used to prevent multiple requests
    // but shouldn't use in scoped injected db services
    // because it may dispose the service, cause the next request failed
    protected async void LoadInParameterSet(Func<Task> proceedFunc)
    {
        if(IsLoading) return;
        IsLoading = true;
        await proceedFunc();
        IsLoading = false;
        StateHasChanged();
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (Js_OnScreenSizeChanged != null)
            await Js.SubscribeJsScreenSize(Js_OnScreenSizeChanged);
        await base.OnAfterRenderAsync(firstRender);
    }

    protected async Task LoadOnceOnInitAsync(Func<Task> proceedFunc)
    {
        if(LoadOnce) return;
        LoadOnce = true;
        await proceedFunc();
    }
    public void Dispose()
    {
        if (Js_OnScreenSizeChanged != null)
            Js.UnsubscribeJsScreenSize(Js_OnScreenSizeChanged);
    }
}