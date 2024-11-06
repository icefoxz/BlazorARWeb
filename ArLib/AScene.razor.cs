using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace ArLib;
public partial class AScene : ComponentBase, IDisposable
{
    [Inject] ArInterop ArInterop { get; set; } = default!;
    [Inject] IJSRuntime Js { get; set; } = default!;
    bool IsScriptLoaded { get; set; }
    bool isInit;
    const string Text_SceneId = "ascene";
    const string Text_True = "true";
    const string Text_False = "false";
    [Parameter] public string TargetSrc { get; set; } = string.Empty;
    [Parameter] public bool AutoStart { get; set; } = true;
    [Parameter] public bool ShowScanningUi { get; set; }
    /// <summary>
    /// (0.001 default) decreasing will increase the sensitivity of the filter(jitter)
    /// </summary>
    [Parameter] public double FilterMinCf { get; set; } = 0.0001;
    /// <summary>
    /// (1000 default) increasing will reduce delay
    /// </summary>
    [Parameter] public double FilterBeta { get; set; } = 1000;
    /// <summary>
    /// (5 default) increasing will reduce the warmup time
    /// </summary>
    [Parameter] public int WarmUpFrame { get; set; } = 5;
    /// <summary>
    /// (5 default) increasing will reduce the miss time
    /// </summary>
    [Parameter] public int MissFrame { get; set; } = 5;
    [Parameter] public bool ShowVrModeUi { get; set; } = false;
    [Parameter] public RenderFragment? ChildContent { get; set; }
    [Parameter] public EventCallback OnArStart { get; set; }
    string IsUiScanning => TrueFalseText(ShowScanningUi);
    string VrModeUiEnable => TrueFalseText(ShowVrModeUi);
    string IsAutoStart => TrueFalseText(AutoStart);
    string TrueFalseText(bool isTrue) => isTrue ? Text_True : Text_False;
    readonly DotNetObjectReference<AScene> _objRef;
    public AScene()
    {
        _objRef = DotNetObjectReference.Create(this);
    }
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender && !isInit)
        {
            isInit = true;
            //await Js.InvokeVoidAsync("loadLibraries");
            await ArInterop.InitAr(_objRef);
        }
    }
    [JSInvokable(nameof(OnScriptsLoaded))]
    public async void OnScriptsLoaded()
    {
        IsScriptLoaded = true;
        await InvokeAsync(StateHasChanged);
        if (AutoStart)
        {
            _ = Task.Run(async () =>
            {
                await Task.Delay(200); // 等待200毫秒
                await InitializeArAsync();
            });
            return;
        }

        //info: don't direct call ArInterop.StartAr, it will cause error!
        // 在新线程中延迟执行 StartAr()
        _ = Task.Run(async () =>
        {
            await Task.Delay(200); // 等待200毫秒
            await Ar_Start();
            await Task.Delay(100); // 等待200毫秒
            await InitializeArAsync();
        });

    }
    async Task InitializeArAsync() => await OnArStart.InvokeAsync();
    Task Ar_Start() => ArInterop.ArStart();
    Task Ar_Stop() => ArInterop.ArStop();
    public void Dispose()
    {
        _objRef.Dispose();
        if (ArInterop is IDisposable arInteropDisposable)
            arInteropDisposable.Dispose();
        else
            _ = ArInterop.DisposeAsync().AsTask();
    }
}