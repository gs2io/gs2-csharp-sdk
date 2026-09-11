using Gs2.Core.Control;
using Gs2.Core.Model;

namespace Gs2.Core.Net
{
    public abstract class Gs2RestSessionTask<TRequest, TResult> : 
        Gs2SessionTask<TRequest, TResult>
        where TRequest : Gs2Request<TRequest>
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

        /// <summary>
        /// service の接続先（https://... まで。パスは呼び手が足す）。
        /// Session が <see cref="Gs2RestSession"/> なら SteadyEndpoint を考慮し、
        /// それ以外（テストのダブル等）は従来の共有クラウドの URL を返す。
        /// </summary>
        protected string EndpointFor(string service)
        {
            return (Session as Gs2RestSession)?.EndpointFor(service)
                   ?? Gs2RestSession.SharedCloudEndpoint(service, Session.Region.DisplayName());
        }
    }

}
