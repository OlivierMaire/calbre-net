using calibre_net.Shared.Contracts;

namespace calibre_net.Client.ApiClients;
public partial class BookClient : BaseApiClient
{
    public System.Threading.Tasks.Task<GetPageResponse?> PageAsStreamAsync(int bookId, string bookFormat, int pageId)
    {
        return PageAsStreamAsync(bookId, bookFormat, pageId, System.Threading.CancellationToken.None);
    }

    public async System.Threading.Tasks.Task<GetPageResponse?> PageAsStreamAsync(int bookId, string bookFormat, int pageId, System.Threading.CancellationToken cancellationToken)
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

                        headers_.TryGetValue("content-type", out var contentType);

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
                            return new GetPageResponse(bytes, contentType?.FirstOrDefault() ?? string.Empty);
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
}