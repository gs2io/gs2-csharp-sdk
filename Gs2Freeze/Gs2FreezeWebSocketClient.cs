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

#pragma warning disable CS0618 // Obsolete with a message

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
    #if GS2_ENABLE_UNITASK
using Cysharp.Threading.Tasks;
    #endif
#else
using System.Threading.Tasks;
using System.Threading;
#endif

namespace Gs2.Gs2Freeze
{
	public class Gs2FreezeWebSocketClient : AbstractGs2Client
	{

		public static string Endpoint = "freeze";

        protected Gs2WebSocketSession Gs2WebSocketSession => (Gs2WebSocketSession) Gs2Session;

		public Gs2FreezeWebSocketClient(Gs2WebSocketSession Gs2WebSocketSession) : base(Gs2WebSocketSession)
		{

		}


        public class PromoteStageTask : Gs2WebSocketSessionTask<Request.PromoteStageRequest, Result.PromoteStageResult>
        {
	        public PromoteStageTask(IGs2Session session, Request.PromoteStageRequest request) : base(session, request)
	        {
	        }

            protected override IGs2SessionRequest CreateRequest(Request.PromoteStageRequest request)
            {
                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);

                jsonWriter.WriteObjectStart();

                if (request.StageName != null)
                {
                    jsonWriter.WritePropertyName("stageName");
                    jsonWriter.Write(request.StageName.ToString());
                }
                if (request.ContextStack != null)
                {
                    jsonWriter.WritePropertyName("contextStack");
                    jsonWriter.Write(request.ContextStack.ToString());
                }
                if (request.DryRun)
                {
                    jsonWriter.WritePropertyName("xGs2DryRun");
                    jsonWriter.Write("true");
                }

                AddHeader(
                    Session.Credential,
                    "freeze",
                    "stage",
                    "promoteStage",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator PromoteStage(
                Request.PromoteStageRequest request,
                UnityAction<AsyncResult<Result.PromoteStageResult>> callback
        )
		{
			var task = new PromoteStageTask(
			    Gs2WebSocketSession,
			    request
            );
            yield return task;
            callback.Invoke(new AsyncResult<Result.PromoteStageResult>(task.Result, task.Error));
        }

		public IFuture<Result.PromoteStageResult> PromoteStageFuture(
                Request.PromoteStageRequest request
        )
		{
			return new PromoteStageTask(
			    Gs2WebSocketSession,
			    request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.PromoteStageResult> PromoteStageAsync(
            Request.PromoteStageRequest request
        )
		{
		    var task = new PromoteStageTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
    #else
		public PromoteStageTask PromoteStageAsync(
                Request.PromoteStageRequest request
        )
		{
			return new PromoteStageTask(
                Gs2WebSocketSession,
			    request
            );
        }
    #endif
#else
		public async Task<Result.PromoteStageResult> PromoteStageAsync(
            Request.PromoteStageRequest request
        )
		{
		    var task = new PromoteStageTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
#endif


        public class RollbackStageTask : Gs2WebSocketSessionTask<Request.RollbackStageRequest, Result.RollbackStageResult>
        {
	        public RollbackStageTask(IGs2Session session, Request.RollbackStageRequest request) : base(session, request)
	        {
	        }

            protected override IGs2SessionRequest CreateRequest(Request.RollbackStageRequest request)
            {
                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);

                jsonWriter.WriteObjectStart();

                if (request.StageName != null)
                {
                    jsonWriter.WritePropertyName("stageName");
                    jsonWriter.Write(request.StageName.ToString());
                }
                if (request.ContextStack != null)
                {
                    jsonWriter.WritePropertyName("contextStack");
                    jsonWriter.Write(request.ContextStack.ToString());
                }
                if (request.DryRun)
                {
                    jsonWriter.WritePropertyName("xGs2DryRun");
                    jsonWriter.Write("true");
                }

                AddHeader(
                    Session.Credential,
                    "freeze",
                    "stage",
                    "rollbackStage",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator RollbackStage(
                Request.RollbackStageRequest request,
                UnityAction<AsyncResult<Result.RollbackStageResult>> callback
        )
		{
			var task = new RollbackStageTask(
			    Gs2WebSocketSession,
			    request
            );
            yield return task;
            callback.Invoke(new AsyncResult<Result.RollbackStageResult>(task.Result, task.Error));
        }

		public IFuture<Result.RollbackStageResult> RollbackStageFuture(
                Request.RollbackStageRequest request
        )
		{
			return new RollbackStageTask(
			    Gs2WebSocketSession,
			    request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.RollbackStageResult> RollbackStageAsync(
            Request.RollbackStageRequest request
        )
		{
		    var task = new RollbackStageTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
    #else
		public RollbackStageTask RollbackStageAsync(
                Request.RollbackStageRequest request
        )
		{
			return new RollbackStageTask(
                Gs2WebSocketSession,
			    request
            );
        }
    #endif
#else
		public async Task<Result.RollbackStageResult> RollbackStageAsync(
            Request.RollbackStageRequest request
        )
		{
		    var task = new RollbackStageTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
#endif
	}
}