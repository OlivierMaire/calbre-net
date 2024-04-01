using System.Drawing;
using calibre_net.Shared.Contracts;
using MudBlazor;

namespace calibre_net.Client.ApiClients;
public partial class BookClient : BaseApiClient
{
    public System.Threading.Tasks.Task<GetPageResponse?> PageDataAsync(int bookId, string bookFormat, int pageId)
    {
        return PageDataAsync(bookId, bookFormat, pageId, System.Threading.CancellationToken.None);
    }

    public async System.Threading.Tasks.Task<GetPageResponse?> PageDataAsync(int bookId, string bookFormat, int pageId, System.Threading.CancellationToken cancellationToken)
    {
        // if (bookId == null)
        //     throw new System.ArgumentNullException("bookId");

        if (bookFormat == null)
            throw new System.ArgumentNullException("bookFormat");

        // if (pageId == null)
        //     throw new System.ArgumentNullException("pageId");

        var client_ = await CreateHttpClientAsync(cancellationToken).ConfigureAwait(false);
        var disposeClient_ = true;
        try
        {
            using (var request_ = new System.Net.Http.HttpRequestMessage())
            {
                request_.Method = new System.Net.Http.HttpMethod("GET");
                request_.Headers.Accept.Add(System.Net.Http.Headers.MediaTypeWithQualityHeaderValue.Parse("application/json"));

                var urlBuilder_ = new System.Text.StringBuilder();

                // Operation Path: "api/v1/book/{bookId}/{bookFormat}/page/{pageId}"
                urlBuilder_.Append("api/v1/book/");
                urlBuilder_.Append(System.Uri.EscapeDataString(ConvertToString(bookId, System.Globalization.CultureInfo.InvariantCulture)));
                urlBuilder_.Append('/');
                urlBuilder_.Append(System.Uri.EscapeDataString(ConvertToString(bookFormat, System.Globalization.CultureInfo.InvariantCulture)));
                urlBuilder_.Append("/page/");
                urlBuilder_.Append(System.Uri.EscapeDataString(ConvertToString(pageId, System.Globalization.CultureInfo.InvariantCulture)));

                PrepareRequest(client_, request_, urlBuilder_);

                var url_ = urlBuilder_.ToString();
                request_.RequestUri = new System.Uri(url_, System.UriKind.RelativeOrAbsolute);

                PrepareRequest(client_, request_, url_);

                var response_ = await client_.SendAsync(request_, System.Net.Http.HttpCompletionOption.ResponseHeadersRead, cancellationToken).ConfigureAwait(false);
                var disposeResponse_ = true;
                try
                {
                    var headers_ = new System.Collections.Generic.Dictionary<string, System.Collections.Generic.IEnumerable<string>>();
                    foreach (var item_ in response_.Headers)
                        headers_[item_.Key] = item_.Value;
                    if (response_.Content != null && response_.Content.Headers != null)
                    {
                        foreach (var item_ in response_.Content.Headers)
                            headers_[item_.Key] = item_.Value;
                    }

                    ProcessResponse(client_, response_);

                    var status_ = (int)response_.StatusCode;
                    if (status_ == 200)
                    {

                        headers_.TryGetValue("Content-Type", out var contentType);
                        headers_.TryGetValue("X-FileInfo-Width", out var FileInfo_Width);
                        headers_.TryGetValue("X-FileInfo-Height", out var FileInfo_Height);
                        headers_.TryGetValue("X-FileInfo-LeftColor", out var FileInfo_LeftColor);
                        headers_.TryGetValue("X-FileInfo-RightColor", out var FileInfo_RightColor);

                        System.Drawing.Size? size = null;
                        if (int.TryParse(FileInfo_Width?.FirstOrDefault(), out var FileInfo_WidthInt) &&
                        int.TryParse(FileInfo_Height?.FirstOrDefault(), out var FileInfo_HeightInt))
                            size = new System.Drawing.Size(FileInfo_WidthInt, FileInfo_HeightInt);


                        using var stream = response_.Content != null ? (await response_.Content.ReadAsStreamAsync(cancellationToken)) : null;



                        // var objectResponse_ = await ReadObjectResponseAsync<byte[]>(response_, headers_, cancellationToken).ConfigureAwait(false);
                        if (stream == null)
                        {
                            throw new ApiException("Response was null which was not expected.", status_, null, headers_, null);
                        }
                        else
                        {
                            var ms = new MemoryStream();
                            stream.CopyTo(ms);

                            var bytes = ms.ToArray();
                            return new GetPageResponse(bytes, contentType?.FirstOrDefault() ?? string.Empty, size,
                            FileInfo_LeftColor?.FirstOrDefault(), FileInfo_RightColor?.FirstOrDefault());
                        }
                    }
                    else
                    if (status_ == 401)
                    {
                        string responseText_ = (response_.Content == null) ? string.Empty : await response_.Content.ReadAsStringAsync().ConfigureAwait(false);
                        throw new ApiException("Unauthorized", status_, responseText_, headers_, null);
                    }
                    else
                    {
                        var responseData_ = response_.Content == null ? null : await response_.Content.ReadAsStringAsync().ConfigureAwait(false);
                        throw new ApiException("The HTTP status code of the response was not expected (" + status_ + ").", status_, responseData_, headers_, null);
                    }
                }
                finally
                {
                    if (disposeResponse_)
                        response_.Dispose();
                }
            }
        }
        finally
        {
            if (disposeClient_)
                client_.Dispose();
        }
    }

    /// <returns>Success</returns>
    /// <exception cref="ApiException">A server side error occurred.</exception>
    public virtual System.Threading.Tasks.Task<GetBookDataResponse?> DownloadDataAsync(int id, string format)
    {
        return DownloadDataAsync(id, format, System.Threading.CancellationToken.None);
    }

    /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>Success</returns>
    /// <exception cref="ApiException">A server side error occurred.</exception>
    public virtual async System.Threading.Tasks.Task<GetBookDataResponse?> DownloadDataAsync(int id, string format, System.Threading.CancellationToken cancellationToken)
    {
        // if (id == null)
        //     throw new System.ArgumentNullException("id");

        if (format == null)
            throw new System.ArgumentNullException("format");

        var client_ = await CreateHttpClientAsync(cancellationToken).ConfigureAwait(false);
        var disposeClient_ = true;
        try
        {
            using (var request_ = new System.Net.Http.HttpRequestMessage())
            {
                request_.Method = new System.Net.Http.HttpMethod("GET");
                request_.Headers.Accept.Add(System.Net.Http.Headers.MediaTypeWithQualityHeaderValue.Parse("application/json"));

                var urlBuilder_ = new System.Text.StringBuilder();

                // Operation Path: "api/v1/book/download/{id}/{format}"
                urlBuilder_.Append("api/v1/book/download/");
                urlBuilder_.Append(System.Uri.EscapeDataString(ConvertToString(id, System.Globalization.CultureInfo.InvariantCulture)));
                urlBuilder_.Append('/');
                urlBuilder_.Append(System.Uri.EscapeDataString(ConvertToString(format, System.Globalization.CultureInfo.InvariantCulture)));

                PrepareRequest(client_, request_, urlBuilder_);

                var url_ = urlBuilder_.ToString();
                request_.RequestUri = new System.Uri(url_, System.UriKind.RelativeOrAbsolute);

                PrepareRequest(client_, request_, url_);

                var response_ = await client_.SendAsync(request_, System.Net.Http.HttpCompletionOption.ResponseHeadersRead, cancellationToken).ConfigureAwait(false);
                var disposeResponse_ = true;
                try
                {
                    var headers_ = new System.Collections.Generic.Dictionary<string, System.Collections.Generic.IEnumerable<string>>();
                    foreach (var item_ in response_.Headers)
                        headers_[item_.Key] = item_.Value;
                    if (response_.Content != null && response_.Content.Headers != null)
                    {
                        foreach (var item_ in response_.Content.Headers)
                            headers_[item_.Key] = item_.Value;
                    }

                    ProcessResponse(client_, response_);

                    var status_ = (int)response_.StatusCode;
                    if (status_ == 200)
                    {
                        headers_.TryGetValue("Content-Type", out var contentType);

                        using var stream = response_.Content != null ? (await response_.Content.ReadAsStreamAsync(cancellationToken)) : null;

                        // var objectResponse_ = await ReadObjectResponseAsync<byte[]>(response_, headers_, cancellationToken).ConfigureAwait(false);
                        if (stream == null)
                        {
                            throw new ApiException("Response was null which was not expected.", status_, null, headers_, null);
                        }
                        else
                        {
                            var ms = new MemoryStream();
                            stream.CopyTo(ms);

                            var bytes = ms.ToArray();
                            return new GetBookDataResponse(bytes, contentType?.FirstOrDefault() ?? string.Empty);
                        }


                        // if (objectResponse_.Object == null)
                        // {
                        //     throw new ApiException("Response was null which was not expected.", status_, objectResponse_.Text, headers_, null);
                        // }
                        // return objectResponse_.Object;
                    }
                    else
                    if (status_ == 401)
                    {
                        string responseText_ = (response_.Content == null) ? string.Empty : await response_.Content.ReadAsStringAsync().ConfigureAwait(false);
                        throw new ApiException("Unauthorized", status_, responseText_, headers_, null);
                    }
                    else
                    {
                        var responseData_ = response_.Content == null ? null : await response_.Content.ReadAsStringAsync().ConfigureAwait(false);
                        throw new ApiException("The HTTP status code of the response was not expected (" + status_ + ").", status_, responseData_, headers_, null);
                    }
                }
                finally
                {
                    if (disposeResponse_)
                        response_.Dispose();
                }
            }
        }
        finally
        {
            if (disposeClient_)
                client_.Dispose();
        }
    }
}