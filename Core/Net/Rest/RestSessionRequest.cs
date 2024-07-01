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

        public void AddHeader(string key, string value)
        {
            this.Headers[key] = value;
        }

        public void AddQueryString(string key, string value)
        {
            this.QueryStrings[key] = value;
        }

        public abstract Task<RestResult> Invoke();
    }
    
    public abstract class RestSessionRequestFuture : RestSessionRequest, IFuture<RestResult>
    {
        public RestResult Result { get; set; }
        public Gs2Exception Error { get; set; }
        private IEnumerator InflightAction { get; }

        protected RestSessionRequestFuture()
        {
            try
            {
                // ReSharper disable once VirtualMemberCallInConstructor
                InflightAction = Action();
            }
            catch (NotImplementedException)
            {
                
            }
        }
        
        public abstract IEnumerator Action();

        public bool IsComplete()
        {
            return Result != null || Error != null;
        }

        public void OnError(Gs2Exception error)
        {
            Error = error;
        }
        
        public void OnComplete(RestResult result)
        {
            Result = result;
        }

        public bool MoveNext()
        {
            if (InflightAction == null) throw new NotImplementedException();
            return InflightAction?.MoveNext() ?? false;
        }

        public void Reset()
        {
            if (InflightAction == null) throw new NotImplementedException();
            InflightAction?.Reset();
        }

        public object Current => InflightAction?.Current;
    }
}