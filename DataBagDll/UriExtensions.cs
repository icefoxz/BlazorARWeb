using System.Linq;

namespace DataBagDll
{
    public static class UriExtensions
    {
        public static string UriJoin(this string baseUri, string relativeUri)
        {
            if (!string.IsNullOrEmpty(baseUri))
                return string.IsNullOrEmpty(relativeUri)
                    ? baseUri
                    : $"{baseUri.TrimEnd('/')}/{relativeUri.TrimStart('/')}";
            return relativeUri;
        }
    }
}