namespace RazorComponents;

public static class WebSite
{
    public enum WindowSize
    {
        XS,
        SM,
        MD,
        LG,
        XL,
        XXL
    }
    public const string DefaultConnection = "DefaultConnection";
    public const string EmptyImg = "/Media/longlama.com.png";
    public const string BodyContent = "body-content";
    public const string AntiForgeryHeader = "X-CSRF-TOKEN";
    public const string CsrfTokenId = "RequestVerificationToken";
    public const string EmptyAvatar = "/Media/empty-avatar.png";

    public static void GoToBottom(this JsInterop js) => js.GoToBottomById(BodyContent);
    public static void GoToTop(this JsInterop js) => js.GoToTopById(BodyContent);
}