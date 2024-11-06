using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace ArLib.Components;
public abstract partial class AObject : ComponentBase, IAsyncDisposable
{
    const string Vec_Zero = "0 0 0";
    const string AFrameChar = "A";
    [Parameter] public string Id { get; set; } = GenerateAlphaPrefixedGuid();
    [Parameter] public string Position { get; set; } = Vec_Zero;
    [Parameter] public string Rotation { get; set; } = Vec_Zero;
    [Parameter] public virtual double Height { get; set; } = 1;
    [Parameter] public virtual double Width { get; set; } = 1;
    [Parameter] public double Scale { get; set; } = 1;
    [Parameter] public string Color { get; set; } = "#FFFFFF";
    [Parameter] public virtual RenderFragment? ChildContent { get; set; }
    [Parameter] public EventCallback<AObject> OnClickEvent { get; set; }
    [Parameter] public string Class { get; set; } = string.Empty;
    [Inject] ArInterop ArInterop { get; set; } = default!;
    [Inject] NavigationManager Nav { get; set; } = default!;
    public DotNetObjectReference<AObject> ARef { get; }
    public static string GenerateAlphaPrefixedGuid()
    {
        var guid = Guid.NewGuid().ToString("N"); // 生成不带连字符的 GUID
        // 如果第一个字符是A开始
        return AFrameChar + guid.Substring(1);
    }
    protected AObject() { ARef = DotNetObjectReference.Create(this); }
    protected double GetRatio(double value, double ratio) => value * ratio;
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            await ArInterop.RegClickable(Id, ARef);
        }
    }
    public ValueTask DisposeAsync()
    {
        ARef.Dispose();
        return ValueTask.CompletedTask;
    }
    [JSInvokable] public async Task OnClick()
    {
        if (OnClickEvent.HasDelegate) 
            await OnClickEvent.InvokeAsync(this);
    }
}