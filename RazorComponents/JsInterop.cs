using Microsoft.JSInterop;

namespace RazorComponents
{
    // This class provides an example of how JavaScript functionality can be wrapped
    // in a .NET class for easy consumption. The associated JavaScript module is
    // loaded on demand when first needed.
    //
    // This class can be registered as scoped DI service and then injected into Blazor
    // components for use.

    public class JsInterop : IAsyncDisposable
    {
        private readonly Lazy<Task<IJSObjectReference>> moduleTask;
        private readonly IJSRuntime JsRuntime;
        private TaskCompletionSource<Stream> _previewImageCallback;
        private DotNetObjectReference<JsInterop> _dotNetObjectRef;
        public JsInterop(IJSRuntime jsRuntime)
        {
            JsRuntime = jsRuntime;
            moduleTask = new(() => jsRuntime.InvokeAsync<IJSObjectReference>(
                "import", "./_content/RazorComponents/JsInterop.js").AsTask());
            _dotNetObjectRef = DotNetObjectReference.Create(this);
        }
        //Confirm method is used to show a confirm dialog to the user.
        public async ValueTask OpenImageInNewTab(string base64Image)
        {
            var module = await moduleTask.Value;
            await module.InvokeVoidAsync("openImageInNewTab", base64Image);
        }
        //Prompt method is used to show a prompt dialog to the user.
        public async ValueTask<string> Prompt(string message)
        {
            var module = await moduleTask.Value;
            return await module.InvokeAsync<string>("showPrompt", message);
        }
        //GetLocationAsync method is used to get the location(Coordinate) of the user.
        public async ValueTask<double[]> GetLocationAsync()
        {
            var module = await moduleTask.Value;
            var location = await module.InvokeAsync<Location>("getLocation");
            return new double[] { location.Lat, location.Lon };
        }
        class Location
        {
            public double Lat { get; set; }
            public double Lon { get; set; }
        }

        //OpenInNewTab method is used to open a new tab with the specified URL.
        public async ValueTask OpenInNewTab(string url)
        {
            var module = await moduleTask.Value;
            await module.InvokeVoidAsync("openInNewTab", url);
        }

        public async Task<Stream> PreviewImage(string inputFileElementId, string imageId)
        {
            _previewImageCallback = new TaskCompletionSource<Stream>();
            var module = await JsRuntime.InvokeAsync<IJSObjectReference>("import", "./_content/RazorComponents/JsInterop.js");
            await module.InvokeVoidAsync("previewImage", inputFileElementId, imageId, 
                1,//convert >1MB
                1024,//target 1024px
                _dotNetObjectRef);
            return await _previewImageCallback.Task;
        }
        [JSInvokable] public void PreviewImageCallback(string base64Image)
        {
            // Decode the base64 string to byte array
            var imageBytes = Convert.FromBase64String(base64Image.Split(',')[1]); // Assuming it includes the data URL prefix

            // Convert byte array to Stream
            var imageStream = new MemoryStream(imageBytes);

            // Set the result of the TaskCompletionSource
            _previewImageCallback.SetResult(imageStream);
        }

        public async void GoToTop(string className)
        {
            var module = await moduleTask.Value;
            await module.InvokeVoidAsync("goToTop",className);
        }
        public async void GoToBottom(string className)
        {
            var module = await moduleTask.Value;
            await module.InvokeVoidAsync("goToBottom", className);
        }
        public async void GoToBottomById(string id)
        {
            var module = await moduleTask.Value;
            await module.InvokeVoidAsync("goToBottomById", id);
        }

        public async void GoToTopById(string id)
        {
            var module = await moduleTask.Value;
            await module.InvokeVoidAsync("goToTopById", id);
        }

        public async ValueTask DisposeAsync()
        {
            if (moduleTask.IsValueCreated)
            {
                var module = await moduleTask.Value;
                await module.DisposeAsync();
            }
            _dotNetObjectRef.Dispose();
        }

        public async void AlignToElementId(string targetId, string alignElementId, string scrollElementId)
        {
            var module = await moduleTask.Value;
            await module.InvokeVoidAsync("alignToElementId", targetId, alignElementId, scrollElementId);
        }

        public async Task<string?> GetElementById(string elementId)
        {
            var module = await moduleTask.Value;
            return await module.InvokeAsync<string>("getElementValueById", elementId);
        }

        private static event Func<WebSite.WindowSize, Task>? OnWindowSizeChanged;
        bool IsStartListeningWindowsResize = false;

        public async Task SubscribeJsScreenSize(Func<WebSite.WindowSize, Task> onWindowSizeChanged)
        {
            if (OnWindowSizeChanged == null || !OnWindowSizeChanged.GetInvocationList().Contains(onWindowSizeChanged))
            {
                OnWindowSizeChanged += onWindowSizeChanged;
            }

            if (IsStartListeningWindowsResize) return;
            IsStartListeningWindowsResize = true;
            var module = await moduleTask.Value;
            await module.InvokeVoidAsync("onResize", _dotNetObjectRef);
        }

        public void UnsubscribeJsScreenSize(Func<WebSite.WindowSize, Task> action)
        {
            if (OnWindowSizeChanged != null && OnWindowSizeChanged.GetInvocationList().Contains(action))
            {
                OnWindowSizeChanged -= action;
            }
        }

        [JSInvokable]
        public async void UpdateScreenSize(string label)
        {
            var size = label switch
            {
                "xs" => WebSite.WindowSize.XS,
                "sm" => WebSite.WindowSize.SM,
                "md" => WebSite.WindowSize.MD,
                "lg" => WebSite.WindowSize.LG,
                "xl" => WebSite.WindowSize.XL,
                "xxl" => WebSite.WindowSize.XXL,
                _ => throw new ArgumentOutOfRangeException(nameof(label), label, null)
            };
            if (OnWindowSizeChanged != null)
                await OnWindowSizeChanged(size);
        }
    }
}