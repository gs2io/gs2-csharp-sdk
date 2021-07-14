/*
 * Copyright 2016 Game Server Services, Inc. or its affiliates. All Rights
 * Reserved.
 *
 * Licensed under the Apache License, Version 2.0 (the "License").
 * You may not use this file except in compliance with the License.
 * A copy of the License is located at
 *
 *  http://www.apache.org/licenses/LICENSE-2.0
 *
 * or in the "license" file accompanying this file. This file is distributed
 * on an "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either
 * express or implied. See the License for the specific language governing
 * permissions and limitations under the License.
 */
using UnityEngine.Events;
using UnityEngine.Networking;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Gs2.Core;
using Gs2.Core.Model;
using Gs2.Core.Net;
using Gs2.Util.LitJson;namespace Gs2.Gs2Watch
{
	public class Gs2WatchWebSocketClient : AbstractGs2Client
	{

		public static string Endpoint = "watch";

        protected Gs2WebSocketSession Gs2WebSocketSession => (Gs2WebSocketSession) Gs2Session;

		public Gs2WatchWebSocketClient(Gs2WebSocketSession Gs2WebSocketSession) : base(Gs2WebSocketSession)
		{

		}

        private class GetCumulativeTask : Gs2WebSocketSessionTask<Result.GetCumulativeResult>
        {
			private readonly Request.GetCumulativeRequest _request;

			public GetCumulativeTask(Request.GetCumulativeRequest request, UnityAction<AsyncResult<Result.GetCumulativeResult>> userCallback) : base(userCallback)
			{
				_request = request;
			}

            protected override IEnumerator ExecuteImpl(Gs2Session gs2Session)
            {
                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);

                jsonWriter.WriteObjectStart();

                if (_request.Name != null)
                {
                    jsonWriter.WritePropertyName("name");
                    jsonWriter.Write(_request.Name.ToString());
                }
                if (_request.ResourceGrn != null)
                {
                    jsonWriter.WritePropertyName("resourceGrn");
                    jsonWriter.Write(_request.ResourceGrn.ToString());
                }
                if (_request.ContextStack != null)
                {
                    jsonWriter.WritePropertyName("contextStack");
                    jsonWriter.Write(_request.ContextStack.ToString());
                }
                if (_request.RequestId != null)
                {
                    jsonWriter.WritePropertyName("xGs2RequestId");
                    jsonWriter.Write(_request.RequestId);
                }

                jsonWriter.WritePropertyName("xGs2ClientId");
                jsonWriter.Write(gs2Session.Credential.ClientId);
                jsonWriter.WritePropertyName("xGs2ProjectToken");
                jsonWriter.Write(gs2Session.ProjectToken);

                jsonWriter.WritePropertyName("x_gs2");
                jsonWriter.WriteObjectStart();
                jsonWriter.WritePropertyName("service");
                jsonWriter.Write("watch");
                jsonWriter.WritePropertyName("component");
                jsonWriter.Write("cumulative");
                jsonWriter.WritePropertyName("function");
                jsonWriter.Write("getCumulative");
                jsonWriter.WritePropertyName("contentType");
                jsonWriter.Write("application/json");
                jsonWriter.WritePropertyName("requestId");
                jsonWriter.Write(Gs2SessionTaskId.ToString());
                jsonWriter.WriteObjectEnd();

                jsonWriter.WriteObjectEnd();

                ((Gs2WebSocketSession)gs2Session).Send(stringBuilder.ToString());

                return new EmptyCoroutine();
            }
        }

		public IEnumerator GetCumulative(
                Request.GetCumulativeRequest request,
                UnityAction<AsyncResult<Result.GetCumulativeResult>> callback
        )
		{
			var task = new GetCumulativeTask(request, callback);
			return Gs2WebSocketSession.Execute(task);
        }

        private class GetBillingActivityTask : Gs2WebSocketSessionTask<Result.GetBillingActivityResult>
        {
			private readonly Request.GetBillingActivityRequest _request;

			public GetBillingActivityTask(Request.GetBillingActivityRequest request, UnityAction<AsyncResult<Result.GetBillingActivityResult>> userCallback) : base(userCallback)
			{
				_request = request;
			}

            protected override IEnumerator ExecuteImpl(Gs2Session gs2Session)
            {
                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);

                jsonWriter.WriteObjectStart();

                if (_request.Year != null)
                {
                    jsonWriter.WritePropertyName("year");
                    jsonWriter.Write(_request.Year.ToString());
                }
                if (_request.Month != null)
                {
                    jsonWriter.WritePropertyName("month");
                    jsonWriter.Write(_request.Month.ToString());
                }
                if (_request.Service != null)
                {
                    jsonWriter.WritePropertyName("service");
                    jsonWriter.Write(_request.Service.ToString());
                }
                if (_request.ActivityType != null)
                {
                    jsonWriter.WritePropertyName("activityType");
                    jsonWriter.Write(_request.ActivityType.ToString());
                }
                if (_request.ContextStack != null)
                {
                    jsonWriter.WritePropertyName("contextStack");
                    jsonWriter.Write(_request.ContextStack.ToString());
                }
                if (_request.RequestId != null)
                {
                    jsonWriter.WritePropertyName("xGs2RequestId");
                    jsonWriter.Write(_request.RequestId);
                }

                jsonWriter.WritePropertyName("xGs2ClientId");
                jsonWriter.Write(gs2Session.Credential.ClientId);
                jsonWriter.WritePropertyName("xGs2ProjectToken");
                jsonWriter.Write(gs2Session.ProjectToken);

                jsonWriter.WritePropertyName("x_gs2");
                jsonWriter.WriteObjectStart();
                jsonWriter.WritePropertyName("service");
                jsonWriter.Write("watch");
                jsonWriter.WritePropertyName("component");
                jsonWriter.Write("billingActivity");
                jsonWriter.WritePropertyName("function");
                jsonWriter.Write("getBillingActivity");
                jsonWriter.WritePropertyName("contentType");
                jsonWriter.Write("application/json");
                jsonWriter.WritePropertyName("requestId");
                jsonWriter.Write(Gs2SessionTaskId.ToString());
                jsonWriter.WriteObjectEnd();

                jsonWriter.WriteObjectEnd();

                ((Gs2WebSocketSession)gs2Session).Send(stringBuilder.ToString());

                return new EmptyCoroutine();
            }
        }

		public IEnumerator GetBillingActivity(
                Request.GetBillingActivityRequest request,
                UnityAction<AsyncResult<Result.GetBillingActivityResult>> callback
        )
		{
			var task = new GetBillingActivityTask(request, callback);
			return Gs2WebSocketSession.Execute(task);
        }
	}
}