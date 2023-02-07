using System.Text;
using Gs2.Core.Model.Internal;
using Gs2.Util.LitJson;

namespace Gs2.Core.Net
{
    public class WebSocketOpenTask : Gs2WebSocketSessionTask<LoginRequest, LoginResult>
    {
        public WebSocketOpenTask(IGs2Session session, LoginRequest request) : base(session, request)
        {
            
        }
        
        protected override IGs2SessionRequest CreateRequest(LoginRequest request)
        {
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

            AddHeader(
                Session.Credential,
                "identifier",
                "projectToken",
                "login",
                jsonWriter
            );

            jsonWriter.WriteObjectEnd();

            return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
        }

        public void NonBlockingInvoke()
        {
            var request = CreateRequest(Request);
            request.TaskId = TaskId;
#if UNITY_2017_1_OR_NEWER
            ((Gs2WebSocketSession)Session).SendNonBlocking(request);
#else
            Session.SendAsync(request);
#endif
        }
    }
}