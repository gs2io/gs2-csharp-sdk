#if !UNITY_2017_1_OR_NEWER
using System;
using System.Collections;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace Gs2.Core.Net
{
    public static class HttpMethodExt
    {
        public static System.Net.Http.HttpMethod Transform(this HttpMethod self)
        {
            switch (self)
            {
                case HttpMethod.Get:
                    return System.Net.Http.HttpMethod.Get;
                case HttpMethod.Post:
                    return System.Net.Http.HttpMethod.Post;
                case HttpMethod.Put:
                    return System.Net.Http.HttpMethod.Put;
                case HttpMethod.Delete:
                    return System.Net.Http.HttpMethod.Delete;
            }
            return System.Net.Http.HttpMethod.Get;
        }
    }
    
    public class DotNetRestSessionRequest : RestSessionRequest
    {
        private static readonly HttpClient RevocationCheckingHttpClient = CreateHttpClient(true);
        private static readonly HttpClient NonRevocationCheckingHttpClient = CreateHttpClient(false);

        private bool _checkCertificateRevocation;
        private HttpClient _httpClient;
        private readonly object _cancellationLock = new object();
        private CancellationTokenSource _cancellation;

        internal bool CheckCertificateRevocation => this._checkCertificateRevocation;
        internal HttpClient SharedHttpClient => this._httpClient;

        public DotNetRestSessionRequest() : this(true)
        {
        }

        public DotNetRestSessionRequest(bool checkCertificateRevocation)
        {
            ConfigureCertificateRevocation(checkCertificateRevocation);
        }

        internal void ConfigureCertificateRevocation(bool checkCertificateRevocation)
        {
            this._checkCertificateRevocation = checkCertificateRevocation;
            this._httpClient = checkCertificateRevocation
                ? RevocationCheckingHttpClient
                : NonRevocationCheckingHttpClient;
        }

        private static HttpClient CreateHttpClient(bool checkCertificateRevocation)
        {
            return new HttpClient(CreateHttpClientHandler(checkCertificateRevocation), true);
        }

        internal static HttpClientHandler CreateHttpClientHandler(bool checkCertificateRevocation)
        {
            return new HttpClientHandler
            {
                CheckCertificateRevocationList = checkCertificateRevocation,
                UseCookies = false,
            };
        }

        private static byte[] Compress(byte[] data)
        {
            using var output = new MemoryStream();
            using (var gzip = new GZipStream(output, CompressionMode.Compress))
            {
                gzip.Write(data, 0, data.Length);
            }
            return output.ToArray();
        }

        private static string Decompress(byte[] data)
        {
            using var input = new MemoryStream(data);
            using var gzip = new GZipStream(input, CompressionMode.Decompress);
            using var reader = new StreamReader(gzip, Encoding.UTF8);
            return reader.ReadToEnd();
        }

        public override async Task<RestResult> Invoke()
        {
            using var cancellation = new CancellationTokenSource();
            lock (_cancellationLock)
            {
                _cancellation = cancellation;
            }

            try
            {
            var uri = QueryStrings.Count == 0 ?
                Url :
                Url + '?' + string.Join("&", QueryStrings.Select(
                    item => $"{item.Key}={HttpUtility.UrlEncode(item.Value)}").ToArray());
            var contentType = Headers.Where(item => item.Key.ToLower() == "content-type").Select(item => item.Value).FirstOrDefault();
            using var request = new HttpRequestMessage(
                Method.Transform(),
                uri
            );
            foreach (var item in Headers)
            {
                if ((Method == HttpMethod.Post || Method == HttpMethod.Put) && item.Key.ToLower() == "content-type")
                {
                    continue;
                }

                try
                {
                    request.Headers.Add(item.Key, item.Value);
                }
                catch (InvalidOperationException e)
                {
                    Console.WriteLine(e.Message);
                }
            }

            if (EnableResponseDecompression)
            {
                request.Headers.AcceptEncoding.Add(new StringWithQualityHeaderValue("gzip"));
            }

            if (Method == HttpMethod.Post || Method == HttpMethod.Put)
            {
                var bodyBytes = Encoding.UTF8.GetBytes(Body);
                if (EnableRequestCompression)
                {
                    bodyBytes = Compress(bodyBytes);
                    var content = new ByteArrayContent(bodyBytes);
                    content.Headers.ContentType = new MediaTypeHeaderValue(contentType ?? "application/json");
                    content.Headers.ContentEncoding.Add("gzip");
                    request.Content = content;
                }
                else
                {
                    request.Content = new StringContent(Body, Encoding.UTF8, contentType);
                }
            }

            try
            {
                using var response = await this._httpClient.SendAsync(request, cancellation.Token);

                string responseBody;
                if (EnableResponseDecompression && response.Content.Headers.ContentEncoding.Contains("gzip"))
                {
                    var responseBytes = await response.Content.ReadAsByteArrayAsync();
                    responseBody = Decompress(responseBytes);
                }
                else
                {
                    responseBody = await response.Content.ReadAsStringAsync();
                }

                return new RestResult(
                    (int) response.StatusCode,
                    responseBody
                );
            }
            catch (OperationCanceledException e)
            {
                return new RestResult(
                    0, // NoInternetConnectionException
                    "",
                    0,
                    e.Message
                );
            }
            catch (System.Net.Http.HttpRequestException e)
            {
                return new RestResult(
                    0, // NoInternetConnectionException
                    "",
                    0,
                    e.Message
                );
            }
            }
            finally
            {
                lock (_cancellationLock)
                {
                    if (ReferenceEquals(_cancellation, cancellation))
                    {
                        _cancellation = null;
                    }
                }
            }
        }

        public override void Abort()
        {
            lock (_cancellationLock)
            {
                _cancellation?.Cancel();
            }
        }
    }
}
#endif
