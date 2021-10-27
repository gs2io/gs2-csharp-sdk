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
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Gs2.Core;
using Gs2.Core.Model;
using Gs2.Core.Net;
using Gs2.Util.LitJson;

#if UNITY_2017_1_OR_NEWER
using System.Collections;
using UnityEngine.Events;
using UnityEngine.Networking;
#else
using System.Threading.Tasks;
using System.Threading;
#endif

namespace Gs2.Gs2Watch
{
	public class Gs2WatchWebSocketClient : AbstractGs2Client
	{

		public static string Endpoint = "watch";

        protected Gs2WebSocketSession Gs2WebSocketSession => (Gs2WebSocketSession) Gs2Session;

		public Gs2WatchWebSocketClient(Gs2WebSocketSession Gs2WebSocketSession) : base(Gs2WebSocketSession)
		{

		}


        private class GetCumulativeTask : Gs2WebSocketSessionTask<Request.GetCumulativeRequest, Result.GetCumulativeResult>
        {
	        public GetCumulativeTask(IGs2Session session, Request.GetCumulativeRequest request) : base(session, request)
	        {
	        }

            protected override IGs2SessionRequest CreateRequest(Request.GetCumulativeRequest request)
            {
                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);

                jsonWriter.WriteObjectStart();

                if (request.Name != null)
                {
                    jsonWriter.WritePropertyName("name");
                    jsonWriter.Write(request.Name.ToString());
                }
                if (request.ResourceGrn != null)
                {
                    jsonWriter.WritePropertyName("resourceGrn");
                    jsonWriter.Write(request.ResourceGrn.ToString());
                }
                if (request.ContextStack != null)
                {
                    jsonWriter.WritePropertyName("contextStack");
                    jsonWriter.Write(request.ContextStack.ToString());
                }
                if (request.RequestId != null)
                {
                    jsonWriter.WritePropertyName("xGs2RequestId");
                    jsonWriter.Write(request.RequestId);
                }

                AddHeader(
                    Session.Credential,
                    "watch",
                    "cumulative",
                    "getCumulative",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator GetCumulative(
                Request.GetCumulativeRequest request,
                UnityAction<AsyncResult<Result.GetCumulativeResult>> callback
        )
		{
			var task = new GetCumulativeTask(
			    Gs2WebSocketSession,
			    request
            );
            yield return task;
            callback.Invoke(new AsyncResult<Result.GetCumulativeResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.GetCumulativeResult> GetCumulative(
            Request.GetCumulativeRequest request
        )
		{
		    var task = new GetCumulativeTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
#endif


        private class GetBillingActivityTask : Gs2WebSocketSessionTask<Request.GetBillingActivityRequest, Result.GetBillingActivityResult>
        {
	        public GetBillingActivityTask(IGs2Session session, Request.GetBillingActivityRequest request) : base(session, request)
	        {
	        }

            protected override IGs2SessionRequest CreateRequest(Request.GetBillingActivityRequest request)
            {
                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);

                jsonWriter.WriteObjectStart();

                if (request.Year != null)
                {
                    jsonWriter.WritePropertyName("year");
                    jsonWriter.Write(request.Year.ToString());
                }
                if (request.Month != null)
                {
                    jsonWriter.WritePropertyName("month");
                    jsonWriter.Write(request.Month.ToString());
                }
                if (request.Service != null)
                {
                    jsonWriter.WritePropertyName("service");
                    jsonWriter.Write(request.Service.ToString());
                }
                if (request.ActivityType != null)
                {
                    jsonWriter.WritePropertyName("activityType");
                    jsonWriter.Write(request.ActivityType.ToString());
                }
                if (request.ContextStack != null)
                {
                    jsonWriter.WritePropertyName("contextStack");
                    jsonWriter.Write(request.ContextStack.ToString());
                }
                if (request.RequestId != null)
                {
                    jsonWriter.WritePropertyName("xGs2RequestId");
                    jsonWriter.Write(request.RequestId);
                }

                AddHeader(
                    Session.Credential,
                    "watch",
                    "billingActivity",
                    "getBillingActivity",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator GetBillingActivity(
                Request.GetBillingActivityRequest request,
                UnityAction<AsyncResult<Result.GetBillingActivityResult>> callback
        )
		{
			var task = new GetBillingActivityTask(
			    Gs2WebSocketSession,
			    request
            );
            yield return task;
            callback.Invoke(new AsyncResult<Result.GetBillingActivityResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.GetBillingActivityResult> GetBillingActivity(
            Request.GetBillingActivityRequest request
        )
		{
		    var task = new GetBillingActivityTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
#endif
	}
}