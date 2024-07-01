using Gs2.Core.Model;

namespace Gs2.Core.Net
{
    public abstract class Gs2RestSessionTask<TRequest, TResult> : 
        Gs2SessionTask<TRequest, TResult>
        where TRequest : IRequest
        where TResult : IResult
    {
        protected RestSessionRequestFactory Factory;

        protected Gs2RestSessionTask(IGs2Session session, RestSessionRequestFactory factory, TRequest request) : base(session, request)
        {
            Session = session;
            Factory = factory;
        }

        protected void AddHeader(
            IGs2Credential credential,
            RestSessionRequest request
        )
        {
            request.AddHeader("X-GS2-CLIENT-ID", credential.ClientId);
            request.AddHeader("Authorization", $"Bearer {credential.ProjectToken}");
        }
    }

}