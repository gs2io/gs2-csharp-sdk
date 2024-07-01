using Gs2.Core.Model;
using Gs2.Util.LitJson;

namespace Gs2.Core.Net
{
    public abstract class Gs2WebSocketSessionTask<TRequest, TResult> : 
        Gs2SessionTask<TRequest, TResult>
        where TRequest : IRequest
        where TResult : IResult
    {
        protected Gs2WebSocketSessionTask(IGs2Session session, TRequest request) : base(session, request)
        {
        }
        
        protected void AddHeader(
            IGs2Credential credential,
            string service,
            string component,
            string function,
            JsonWriter jsonWriter
        )
        {
            jsonWriter.WritePropertyName("xGs2ClientId");
            jsonWriter.Write(credential.ClientId);
            if (credential.ProjectToken != null)
            {
                jsonWriter.WritePropertyName("xGs2ProjectToken");
                jsonWriter.Write(credential.ProjectToken);
            }

            {
                jsonWriter.WritePropertyName("x_gs2");
                jsonWriter.WriteObjectStart();
                {
                    jsonWriter.WritePropertyName("service");
                    jsonWriter.Write(service);
                }
                {
                    jsonWriter.WritePropertyName("component");
                    jsonWriter.Write(component);
                }
                {
                    jsonWriter.WritePropertyName("function");
                    jsonWriter.Write(function);
                }
                {
                    jsonWriter.WritePropertyName("contentType");
                    jsonWriter.Write("application/json");
                }
                {
                    jsonWriter.WritePropertyName("requestId");
                    jsonWriter.Write(TaskId.ToString());
                }
                jsonWriter.WriteObjectEnd();
            }
        }
    }
}