#if !UNITY_2017_1_OR_NEWER
using System;
using System.Collections;
using System.Linq;
using System.Net.Http;
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
        public override async Task<RestResult> Invoke()
        {
            var uri = QueryStrings.Count == 0 ? 
                Url : 
                Url + '?' + string.Join("&", QueryStrings.Select(
                    item => $"{item.Key}={HttpUtility.UrlEncode(item.Value)}").ToArray());
            var contentType = Headers.Where(item => item.Key.ToLower() == "content-type").Select(item => item.Value).FirstOrDefault();
            var request = new HttpRequestMessage(
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

            if (Method == HttpMethod.Post || Method == HttpMethod.Put)
            {
                request.Content = new StringContent(Body, Encoding.UTF8, contentType);
            }

            var response = await new HttpClient().SendAsync(request);

            if ((int) response.StatusCode == 500)
            {
                return await Invoke();
            }

            var result = new RestResult(
                (int) response.StatusCode,
                await response.Content.ReadAsStringAsync()
            );
            OnComplete(result);

            request.Dispose();

            return Result;
        }

        public override IEnumerator Action()
        {
            throw new NotImplementedException();
        }
    }
}
#endif