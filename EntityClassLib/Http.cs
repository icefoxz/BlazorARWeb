﻿using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Web;

namespace EntityClassLib;
public static class Http
{
    private static HttpClient HttpClient => new HttpClient();

    private const string JsonMediaType = "application/json";

    public static Uri GetUri(string url, string action, params (string, string)[] queries)
    {
        var builder = new UriBuilder(url);
        var query = HttpUtility.ParseQueryString(builder.Query);
        builder.Path = builder.Path.TrimEnd('/') + $"api/{action}";
        foreach (var (q, value) in queries)
            query[q] = value.ToString();
        builder.Query = query.ToString();
        return builder.Uri;
    }

    private static HttpContent GetContent(string stringContent) =>
        new StringContent(stringContent, Encoding.UTF8, JsonMediaType);

    public static async Task<(bool isSuccess, string content, HttpStatusCode code)> SendStringContentAsync(Uri uri, HttpMethod method,
        string content = null, string token = null)
    {
        var httpContent = content != null ? GetContent(content) : null;
        return await SendAsync(uri, method, httpContent, token);
    }
    public static async Task<(bool isSuccess, string content, HttpStatusCode code)> SendAsync(Uri uri, HttpMethod method, HttpContent content = null, string token = null)
    {
        using var request = new HttpRequestMessage(method, uri);
        request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue(JsonMediaType));

        if (!string.IsNullOrEmpty(token))
        {
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }

        if (content != null) request.Content = content;

        using var client = HttpClient;
        var response = await client.SendAsync(request);
        var responseContent = await response.Content.ReadAsStringAsync();
        return (response.IsSuccessStatusCode, responseContent, response.StatusCode);
    }
}
