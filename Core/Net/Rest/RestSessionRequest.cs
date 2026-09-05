using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Gs2.Core.Exception;

namespace Gs2.Core.Net
{
    public abstract class RestSessionRequest : IGs2SessionRequest
    {
        public Gs2SessionTaskId TaskId { get; set; }
        public string Url { get; set; }
        public HttpMethod Method { get; set; }
        public readonly Dictionary<string, string> Headers = new Dictionary<string, string>();
        public readonly Dictionary<string, string> QueryStrings = new Dictionary<string, string>();
        public string Body { get; set; }
        public bool EnableRequestCompression { get; set; } = true;
        public bool EnableResponseDecompression { get; set; } = true;
        internal object SessionOpenToken { get; set; }

        public void AddHeader(string key, string value)
        {
            this.Headers[key] = value;
        }

        public void AddQueryString(string key, string value)
        {
            this.QueryStrings[key] = value;
        }

        public abstract Task<RestResult> Invoke();

        public virtual void Abort()
        {
        }
    }
}
