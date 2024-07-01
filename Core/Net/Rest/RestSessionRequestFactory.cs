using System;

namespace Gs2.Core.Net
{
    public enum HttpMethod
    {
        Get,
        Post,
        Put,
        Delete,
    }
    
    public class RestSessionRequestFactory
    {
        private readonly Func<RestSessionRequest> _new;

        public RestSessionRequestFactory(Func<RestSessionRequest> @new)
        {
            this._new = @new;
        }

        public RestSessionRequest Get(string url) 
        {
            var request = this._new();
            request.Method = HttpMethod.Get;
            request.Url = url;
            return request;
        }

        public RestSessionRequest Post(string url) 
        {
            var request = this._new();
            request.Method = HttpMethod.Post;
            request.Url = url;
            request.Body = "{}";
            return request;
        }

        public RestSessionRequest Put(string url) 
        {
            var request = this._new();
            request.Method = HttpMethod.Put;
            request.Url = url;
            request.Body = "{}";
            return request;
        }

        public RestSessionRequest Delete(string url) 
        {
            var request = this._new();
            request.Method = HttpMethod.Delete;
            request.Url = url;
            return request;
        }
    }
}