using System.Text;
using Gs2.Core.Model;
using Gs2.Core.Model.Internal;
using Gs2.Util.LitJson;

namespace Gs2.Core.Net
{
    public class RestOpenTask : Gs2RestSessionTask<LoginRequest, LoginResult>
    {
        public RestOpenTask(IGs2Session session, RestSessionRequestFactory factory, LoginRequest request) : base(session, factory, request)
        {
            Session = session;
        }
        
        protected override IGs2SessionRequest CreateRequest(LoginRequest request)
        {
            var url = Gs2RestSession.EndpointHost
                          .Replace("{service}", "identifier")
                          .Replace("{region}", Session.Region.DisplayName())
                      + "/projectToken/login";

            var restRequest = Factory.Post(url);
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