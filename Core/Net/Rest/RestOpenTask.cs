using System.Text;
using Gs2.Core.Model;
using Gs2.Core.Model.Internal;
using Gs2.Util.LitJson;

namespace Gs2.Core.Net
{
    public class RestOpenTask : Gs2RestSessionTask<LoginRequest, LoginResult>
    {
        private readonly object _sessionOpenToken;

        public RestOpenTask(IGs2Session session, RestSessionRequestFactory factory, LoginRequest request) : base(session, factory, request)
        {
            Session = session;
        }

        internal RestOpenTask(
            IGs2Session session,
            RestSessionRequestFactory factory,
            LoginRequest request,
            object sessionOpenToken
        ) : this(session, factory, request)
        {
            _sessionOpenToken = sessionOpenToken;
        }
        
        protected override IGs2SessionRequest CreateRequest(LoginRequest request)
        {
            // ★プロジェクトトークンのログインも Steady の基点配下（<steady>/identifier/projectToken/login）へ向ける。
            // 共有クラウドの identifier へ行かせると、Steady のアプリのログインだけ別の基点になる。
            var url = EndpointFor("identifier") + "/projectToken/login";

            var restRequest = Factory.Post(url);
            restRequest.SessionOpenToken = _sessionOpenToken;
            restRequest.AddHeader("Content-Type", "application/json");

            var stringBuilder = new StringBuilder();
            var jsonWriter = new JsonWriter(stringBuilder);
            jsonWriter.WriteObjectStart();
            if(Session.Credential.ClientId != null)
            {
                jsonWriter.WritePropertyName("client_id");
                jsonWriter.Write(Session.Credential.ClientId);
            }
            if(Session.Credential.ClientSecret != null)
            {
                jsonWriter.WritePropertyName("client_secret");
                jsonWriter.Write(Session.Credential.ClientSecret);
            }
            jsonWriter.WriteObjectEnd();

            restRequest.Body = stringBuilder.ToString();
            
            return restRequest;
        }
    }
}
