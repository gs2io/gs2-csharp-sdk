#if !UNITY_2017_1_OR_NEWER
using System;
using System.Collections;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
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
    
    public class DotNetRestSessionRequest : RestSessionRequestFuture
    {
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
                using var httpClient = new HttpClient();
                using var response = await httpClient.SendAsync(request);

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

                var result = new RestResult(
                    (int) response.StatusCode,
                    responseBody
                );
                OnComplete(result);
                return Result;

            }
            catch (OperationCanceledException e)
            {
                var result = new RestResult(
                    0, // NoInternetConnectionException
                    "",
                    0,
                    e.Message
                );
                OnComplete(result);
                return Result;
            }
            catch (System.Net.Http.HttpRequestException e)
            {
                var result = new RestResult(
                    0, // NoInternetConnectionException
                    "",
                    0,
                    e.Message
                );
                OnComplete(result);
                return Result;
            }
        }

        public override IEnumerator Action()
        {
            throw new NotImplementedException();
        }
    }
}
#endif