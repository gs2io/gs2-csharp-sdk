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

namespace Gs2.Gs2Matchmaking
{
	public class Gs2MatchmakingWebSocketClient : AbstractGs2Client
	{

		public static string Endpoint = "matchmaking";

        protected Gs2WebSocketSession Gs2WebSocketSession => (Gs2WebSocketSession) Gs2Session;

		public Gs2MatchmakingWebSocketClient(Gs2WebSocketSession Gs2WebSocketSession) : base(Gs2WebSocketSession)
		{

		}


        private class CreateNamespaceTask : Gs2WebSocketSessionTask<Request.CreateNamespaceRequest, Result.CreateNamespaceResult>
        {
	        public CreateNamespaceTask(IGs2Session session, Request.CreateNamespaceRequest request) : base(session, request)
	        {
	        }

            protected override IGs2SessionRequest CreateRequest(Request.CreateNamespaceRequest request)
            {
                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);

                jsonWriter.WriteObjectStart();

                if (request.Name != null)
                {
                    jsonWriter.WritePropertyName("name");
                    jsonWriter.Write(request.Name.ToString());
                }
                if (request.Description != null)
                {
                    jsonWriter.WritePropertyName("description");
                    jsonWriter.Write(request.Description.ToString());
                }
                if (request.EnableRating != null)
                {
                    jsonWriter.WritePropertyName("enableRating");
                    jsonWriter.Write(request.EnableRating.ToString());
                }
                if (request.CreateGatheringTriggerType != null)
                {
                    jsonWriter.WritePropertyName("createGatheringTriggerType");
                    jsonWriter.Write(request.CreateGatheringTriggerType.ToString());
                }
                if (request.CreateGatheringTriggerRealtimeNamespaceId != null)
                {
                    jsonWriter.WritePropertyName("createGatheringTriggerRealtimeNamespaceId");
                    jsonWriter.Write(request.CreateGatheringTriggerRealtimeNamespaceId.ToString());
                }
                if (request.CreateGatheringTriggerScriptId != null)
                {
                    jsonWriter.WritePropertyName("createGatheringTriggerScriptId");
                    jsonWriter.Write(request.CreateGatheringTriggerScriptId.ToString());
                }
                if (request.CompleteMatchmakingTriggerType != null)
                {
                    jsonWriter.WritePropertyName("completeMatchmakingTriggerType");
                    jsonWriter.Write(request.CompleteMatchmakingTriggerType.ToString());
                }
                if (request.CompleteMatchmakingTriggerRealtimeNamespaceId != null)
                {
                    jsonWriter.WritePropertyName("completeMatchmakingTriggerRealtimeNamespaceId");
                    jsonWriter.Write(request.CompleteMatchmakingTriggerRealtimeNamespaceId.ToString());
                }
                if (request.CompleteMatchmakingTriggerScriptId != null)
                {
                    jsonWriter.WritePropertyName("completeMatchmakingTriggerScriptId");
                    jsonWriter.Write(request.CompleteMatchmakingTriggerScriptId.ToString());
                }
                if (request.JoinNotification != null)
                {
                    jsonWriter.WritePropertyName("joinNotification");
                    request.JoinNotification.WriteJson(jsonWriter);
                }
                if (request.LeaveNotification != null)
                {
                    jsonWriter.WritePropertyName("leaveNotification");
                    request.LeaveNotification.WriteJson(jsonWriter);
                }
                if (request.CompleteNotification != null)
                {
                    jsonWriter.WritePropertyName("completeNotification");
                    request.CompleteNotification.WriteJson(jsonWriter);
                }
                if (request.LogSetting != null)
                {
                    jsonWriter.WritePropertyName("logSetting");
                    request.LogSetting.WriteJson(jsonWriter);
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
                    "matchmaking",
                    "namespace",
                    "createNamespace",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator CreateNamespace(
                Request.CreateNamespaceRequest request,
                UnityAction<AsyncResult<Result.CreateNamespaceResult>> callback
        )
		{
			var task = new CreateNamespaceTask(
			    Gs2WebSocketSession,
			    request
            );
            yield return task;
            callback.Invoke(new AsyncResult<Result.CreateNamespaceResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.CreateNamespaceResult> CreateNamespace(
            Request.CreateNamespaceRequest request
        )
		{
		    var task = new CreateNamespaceTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
#endif


        private class GetNamespaceTask : Gs2WebSocketSessionTask<Request.GetNamespaceRequest, Result.GetNamespaceResult>
        {
	        public GetNamespaceTask(IGs2Session session, Request.GetNamespaceRequest request) : base(session, request)
	        {
	        }

            protected override IGs2SessionRequest CreateRequest(Request.GetNamespaceRequest request)
            {
                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);

                jsonWriter.WriteObjectStart();

                if (request.NamespaceName != null)
                {
                    jsonWriter.WritePropertyName("namespaceName");
                    jsonWriter.Write(request.NamespaceName.ToString());
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
                    "matchmaking",
                    "namespace",
                    "getNamespace",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator GetNamespace(
                Request.GetNamespaceRequest request,
                UnityAction<AsyncResult<Result.GetNamespaceResult>> callback
        )
		{
			var task = new GetNamespaceTask(
			    Gs2WebSocketSession,
			    request
            );
            yield return task;
            callback.Invoke(new AsyncResult<Result.GetNamespaceResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.GetNamespaceResult> GetNamespace(
            Request.GetNamespaceRequest request
        )
		{
		    var task = new GetNamespaceTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
#endif


        private class UpdateNamespaceTask : Gs2WebSocketSessionTask<Request.UpdateNamespaceRequest, Result.UpdateNamespaceResult>
        {
	        public UpdateNamespaceTask(IGs2Session session, Request.UpdateNamespaceRequest request) : base(session, request)
	        {
	        }

            protected override IGs2SessionRequest CreateRequest(Request.UpdateNamespaceRequest request)
            {
                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);

                jsonWriter.WriteObjectStart();

                if (request.NamespaceName != null)
                {
                    jsonWriter.WritePropertyName("namespaceName");
                    jsonWriter.Write(request.NamespaceName.ToString());
                }
                if (request.Description != null)
                {
                    jsonWriter.WritePropertyName("description");
                    jsonWriter.Write(request.Description.ToString());
                }
                if (request.EnableRating != null)
                {
                    jsonWriter.WritePropertyName("enableRating");
                    jsonWriter.Write(request.EnableRating.ToString());
                }
                if (request.CreateGatheringTriggerType != null)
                {
                    jsonWriter.WritePropertyName("createGatheringTriggerType");
                    jsonWriter.Write(request.CreateGatheringTriggerType.ToString());
                }
                if (request.CreateGatheringTriggerRealtimeNamespaceId != null)
                {
                    jsonWriter.WritePropertyName("createGatheringTriggerRealtimeNamespaceId");
                    jsonWriter.Write(request.CreateGatheringTriggerRealtimeNamespaceId.ToString());
                }
                if (request.CreateGatheringTriggerScriptId != null)
                {
                    jsonWriter.WritePropertyName("createGatheringTriggerScriptId");
                    jsonWriter.Write(request.CreateGatheringTriggerScriptId.ToString());
                }
                if (request.CompleteMatchmakingTriggerType != null)
                {
                    jsonWriter.WritePropertyName("completeMatchmakingTriggerType");
                    jsonWriter.Write(request.CompleteMatchmakingTriggerType.ToString());
                }
                if (request.CompleteMatchmakingTriggerRealtimeNamespaceId != null)
                {
                    jsonWriter.WritePropertyName("completeMatchmakingTriggerRealtimeNamespaceId");
                    jsonWriter.Write(request.CompleteMatchmakingTriggerRealtimeNamespaceId.ToString());
                }
                if (request.CompleteMatchmakingTriggerScriptId != null)
                {
                    jsonWriter.WritePropertyName("completeMatchmakingTriggerScriptId");
                    jsonWriter.Write(request.CompleteMatchmakingTriggerScriptId.ToString());
                }
                if (request.JoinNotification != null)
                {
                    jsonWriter.WritePropertyName("joinNotification");
                    request.JoinNotification.WriteJson(jsonWriter);
                }
                if (request.LeaveNotification != null)
                {
                    jsonWriter.WritePropertyName("leaveNotification");
                    request.LeaveNotification.WriteJson(jsonWriter);
                }
                if (request.CompleteNotification != null)
                {
                    jsonWriter.WritePropertyName("completeNotification");
                    request.CompleteNotification.WriteJson(jsonWriter);
                }
                if (request.LogSetting != null)
                {
                    jsonWriter.WritePropertyName("logSetting");
                    request.LogSetting.WriteJson(jsonWriter);
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
                    "matchmaking",
                    "namespace",
                    "updateNamespace",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator UpdateNamespace(
                Request.UpdateNamespaceRequest request,
                UnityAction<AsyncResult<Result.UpdateNamespaceResult>> callback
        )
		{
			var task = new UpdateNamespaceTask(
			    Gs2WebSocketSession,
			    request
            );
            yield return task;
            callback.Invoke(new AsyncResult<Result.UpdateNamespaceResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.UpdateNamespaceResult> UpdateNamespace(
            Request.UpdateNamespaceRequest request
        )
		{
		    var task = new UpdateNamespaceTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
#endif


        private class DeleteNamespaceTask : Gs2WebSocketSessionTask<Request.DeleteNamespaceRequest, Result.DeleteNamespaceResult>
        {
	        public DeleteNamespaceTask(IGs2Session session, Request.DeleteNamespaceRequest request) : base(session, request)
	        {
	        }

            protected override IGs2SessionRequest CreateRequest(Request.DeleteNamespaceRequest request)
            {
                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);

                jsonWriter.WriteObjectStart();

                if (request.NamespaceName != null)
                {
                    jsonWriter.WritePropertyName("namespaceName");
                    jsonWriter.Write(request.NamespaceName.ToString());
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
                    "matchmaking",
                    "namespace",
                    "deleteNamespace",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator DeleteNamespace(
                Request.DeleteNamespaceRequest request,
                UnityAction<AsyncResult<Result.DeleteNamespaceResult>> callback
        )
		{
			var task = new DeleteNamespaceTask(
			    Gs2WebSocketSession,
			    request
            );
            yield return task;
            callback.Invoke(new AsyncResult<Result.DeleteNamespaceResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.DeleteNamespaceResult> DeleteNamespace(
            Request.DeleteNamespaceRequest request
        )
		{
		    var task = new DeleteNamespaceTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
#endif


        private class CreateRatingModelMasterTask : Gs2WebSocketSessionTask<Request.CreateRatingModelMasterRequest, Result.CreateRatingModelMasterResult>
        {
	        public CreateRatingModelMasterTask(IGs2Session session, Request.CreateRatingModelMasterRequest request) : base(session, request)
	        {
	        }

            protected override IGs2SessionRequest CreateRequest(Request.CreateRatingModelMasterRequest request)
            {
                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);

                jsonWriter.WriteObjectStart();

                if (request.NamespaceName != null)
                {
                    jsonWriter.WritePropertyName("namespaceName");
                    jsonWriter.Write(request.NamespaceName.ToString());
                }
                if (request.Name != null)
                {
                    jsonWriter.WritePropertyName("name");
                    jsonWriter.Write(request.Name.ToString());
                }
                if (request.Description != null)
                {
                    jsonWriter.WritePropertyName("description");
                    jsonWriter.Write(request.Description.ToString());
                }
                if (request.Metadata != null)
                {
                    jsonWriter.WritePropertyName("metadata");
                    jsonWriter.Write(request.Metadata.ToString());
                }
                if (request.Volatility != null)
                {
                    jsonWriter.WritePropertyName("volatility");
                    jsonWriter.Write(request.Volatility.ToString());
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
                    "matchmaking",
                    "ratingModelMaster",
                    "createRatingModelMaster",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator CreateRatingModelMaster(
                Request.CreateRatingModelMasterRequest request,
                UnityAction<AsyncResult<Result.CreateRatingModelMasterResult>> callback
        )
		{
			var task = new CreateRatingModelMasterTask(
			    Gs2WebSocketSession,
			    request
            );
            yield return task;
            callback.Invoke(new AsyncResult<Result.CreateRatingModelMasterResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.CreateRatingModelMasterResult> CreateRatingModelMaster(
            Request.CreateRatingModelMasterRequest request
        )
		{
		    var task = new CreateRatingModelMasterTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
#endif


        private class GetRatingModelMasterTask : Gs2WebSocketSessionTask<Request.GetRatingModelMasterRequest, Result.GetRatingModelMasterResult>
        {
	        public GetRatingModelMasterTask(IGs2Session session, Request.GetRatingModelMasterRequest request) : base(session, request)
	        {
	        }

            protected override IGs2SessionRequest CreateRequest(Request.GetRatingModelMasterRequest request)
            {
                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);

                jsonWriter.WriteObjectStart();

                if (request.NamespaceName != null)
                {
                    jsonWriter.WritePropertyName("namespaceName");
                    jsonWriter.Write(request.NamespaceName.ToString());
                }
                if (request.RatingName != null)
                {
                    jsonWriter.WritePropertyName("ratingName");
                    jsonWriter.Write(request.RatingName.ToString());
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
                    "matchmaking",
                    "ratingModelMaster",
                    "getRatingModelMaster",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator GetRatingModelMaster(
                Request.GetRatingModelMasterRequest request,
                UnityAction<AsyncResult<Result.GetRatingModelMasterResult>> callback
        )
		{
			var task = new GetRatingModelMasterTask(
			    Gs2WebSocketSession,
			    request
            );
            yield return task;
            callback.Invoke(new AsyncResult<Result.GetRatingModelMasterResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.GetRatingModelMasterResult> GetRatingModelMaster(
            Request.GetRatingModelMasterRequest request
        )
		{
		    var task = new GetRatingModelMasterTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
#endif


        private class UpdateRatingModelMasterTask : Gs2WebSocketSessionTask<Request.UpdateRatingModelMasterRequest, Result.UpdateRatingModelMasterResult>
        {
	        public UpdateRatingModelMasterTask(IGs2Session session, Request.UpdateRatingModelMasterRequest request) : base(session, request)
	        {
	        }

            protected override IGs2SessionRequest CreateRequest(Request.UpdateRatingModelMasterRequest request)
            {
                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);

                jsonWriter.WriteObjectStart();

                if (request.NamespaceName != null)
                {
                    jsonWriter.WritePropertyName("namespaceName");
                    jsonWriter.Write(request.NamespaceName.ToString());
                }
                if (request.RatingName != null)
                {
                    jsonWriter.WritePropertyName("ratingName");
                    jsonWriter.Write(request.RatingName.ToString());
                }
                if (request.Description != null)
                {
                    jsonWriter.WritePropertyName("description");
                    jsonWriter.Write(request.Description.ToString());
                }
                if (request.Metadata != null)
                {
                    jsonWriter.WritePropertyName("metadata");
                    jsonWriter.Write(request.Metadata.ToString());
                }
                if (request.Volatility != null)
                {
                    jsonWriter.WritePropertyName("volatility");
                    jsonWriter.Write(request.Volatility.ToString());
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
                    "matchmaking",
                    "ratingModelMaster",
                    "updateRatingModelMaster",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator UpdateRatingModelMaster(
                Request.UpdateRatingModelMasterRequest request,
                UnityAction<AsyncResult<Result.UpdateRatingModelMasterResult>> callback
        )
		{
			var task = new UpdateRatingModelMasterTask(
			    Gs2WebSocketSession,
			    request
            );
            yield return task;
            callback.Invoke(new AsyncResult<Result.UpdateRatingModelMasterResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.UpdateRatingModelMasterResult> UpdateRatingModelMaster(
            Request.UpdateRatingModelMasterRequest request
        )
		{
		    var task = new UpdateRatingModelMasterTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
#endif


        private class DeleteRatingModelMasterTask : Gs2WebSocketSessionTask<Request.DeleteRatingModelMasterRequest, Result.DeleteRatingModelMasterResult>
        {
	        public DeleteRatingModelMasterTask(IGs2Session session, Request.DeleteRatingModelMasterRequest request) : base(session, request)
	        {
	        }

            protected override IGs2SessionRequest CreateRequest(Request.DeleteRatingModelMasterRequest request)
            {
                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);

                jsonWriter.WriteObjectStart();

                if (request.NamespaceName != null)
                {
                    jsonWriter.WritePropertyName("namespaceName");
                    jsonWriter.Write(request.NamespaceName.ToString());
                }
                if (request.RatingName != null)
                {
                    jsonWriter.WritePropertyName("ratingName");
                    jsonWriter.Write(request.RatingName.ToString());
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
                    "matchmaking",
                    "ratingModelMaster",
                    "deleteRatingModelMaster",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator DeleteRatingModelMaster(
                Request.DeleteRatingModelMasterRequest request,
                UnityAction<AsyncResult<Result.DeleteRatingModelMasterResult>> callback
        )
		{
			var task = new DeleteRatingModelMasterTask(
			    Gs2WebSocketSession,
			    request
            );
            yield return task;
            callback.Invoke(new AsyncResult<Result.DeleteRatingModelMasterResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.DeleteRatingModelMasterResult> DeleteRatingModelMaster(
            Request.DeleteRatingModelMasterRequest request
        )
		{
		    var task = new DeleteRatingModelMasterTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
#endif


        private class GetRatingModelTask : Gs2WebSocketSessionTask<Request.GetRatingModelRequest, Result.GetRatingModelResult>
        {
	        public GetRatingModelTask(IGs2Session session, Request.GetRatingModelRequest request) : base(session, request)
	        {
	        }

            protected override IGs2SessionRequest CreateRequest(Request.GetRatingModelRequest request)
            {
                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);

                jsonWriter.WriteObjectStart();

                if (request.NamespaceName != null)
                {
                    jsonWriter.WritePropertyName("namespaceName");
                    jsonWriter.Write(request.NamespaceName.ToString());
                }
                if (request.RatingName != null)
                {
                    jsonWriter.WritePropertyName("ratingName");
                    jsonWriter.Write(request.RatingName.ToString());
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
                    "matchmaking",
                    "ratingModel",
                    "getRatingModel",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator GetRatingModel(
                Request.GetRatingModelRequest request,
                UnityAction<AsyncResult<Result.GetRatingModelResult>> callback
        )
		{
			var task = new GetRatingModelTask(
			    Gs2WebSocketSession,
			    request
            );
            yield return task;
            callback.Invoke(new AsyncResult<Result.GetRatingModelResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.GetRatingModelResult> GetRatingModel(
            Request.GetRatingModelRequest request
        )
		{
		    var task = new GetRatingModelTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
#endif


        private class GetRatingTask : Gs2WebSocketSessionTask<Request.GetRatingRequest, Result.GetRatingResult>
        {
	        public GetRatingTask(IGs2Session session, Request.GetRatingRequest request) : base(session, request)
	        {
	        }

            protected override IGs2SessionRequest CreateRequest(Request.GetRatingRequest request)
            {
                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);

                jsonWriter.WriteObjectStart();

                if (request.NamespaceName != null)
                {
                    jsonWriter.WritePropertyName("namespaceName");
                    jsonWriter.Write(request.NamespaceName.ToString());
                }
                if (request.AccessToken != null)
                {
                    jsonWriter.WritePropertyName("accessToken");
                    jsonWriter.Write(request.AccessToken.ToString());
                }
                if (request.RatingName != null)
                {
                    jsonWriter.WritePropertyName("ratingName");
                    jsonWriter.Write(request.RatingName.ToString());
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
                if (request.AccessToken != null)
                {
                    jsonWriter.WritePropertyName("xGs2AccessToken");
                    jsonWriter.Write(request.AccessToken);
                }

                AddHeader(
                    Session.Credential,
                    "matchmaking",
                    "rating",
                    "getRating",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator GetRating(
                Request.GetRatingRequest request,
                UnityAction<AsyncResult<Result.GetRatingResult>> callback
        )
		{
			var task = new GetRatingTask(
			    Gs2WebSocketSession,
			    request
            );
            yield return task;
            callback.Invoke(new AsyncResult<Result.GetRatingResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.GetRatingResult> GetRating(
            Request.GetRatingRequest request
        )
		{
		    var task = new GetRatingTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
#endif


        private class GetRatingByUserIdTask : Gs2WebSocketSessionTask<Request.GetRatingByUserIdRequest, Result.GetRatingByUserIdResult>
        {
	        public GetRatingByUserIdTask(IGs2Session session, Request.GetRatingByUserIdRequest request) : base(session, request)
	        {
	        }

            protected override IGs2SessionRequest CreateRequest(Request.GetRatingByUserIdRequest request)
            {
                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);

                jsonWriter.WriteObjectStart();

                if (request.NamespaceName != null)
                {
                    jsonWriter.WritePropertyName("namespaceName");
                    jsonWriter.Write(request.NamespaceName.ToString());
                }
                if (request.UserId != null)
                {
                    jsonWriter.WritePropertyName("userId");
                    jsonWriter.Write(request.UserId.ToString());
                }
                if (request.RatingName != null)
                {
                    jsonWriter.WritePropertyName("ratingName");
                    jsonWriter.Write(request.RatingName.ToString());
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
                    "matchmaking",
                    "rating",
                    "getRatingByUserId",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator GetRatingByUserId(
                Request.GetRatingByUserIdRequest request,
                UnityAction<AsyncResult<Result.GetRatingByUserIdResult>> callback
        )
		{
			var task = new GetRatingByUserIdTask(
			    Gs2WebSocketSession,
			    request
            );
            yield return task;
            callback.Invoke(new AsyncResult<Result.GetRatingByUserIdResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.GetRatingByUserIdResult> GetRatingByUserId(
            Request.GetRatingByUserIdRequest request
        )
		{
		    var task = new GetRatingByUserIdTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
#endif


        private class DeleteRatingTask : Gs2WebSocketSessionTask<Request.DeleteRatingRequest, Result.DeleteRatingResult>
        {
	        public DeleteRatingTask(IGs2Session session, Request.DeleteRatingRequest request) : base(session, request)
	        {
	        }

            protected override IGs2SessionRequest CreateRequest(Request.DeleteRatingRequest request)
            {
                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);

                jsonWriter.WriteObjectStart();

                if (request.NamespaceName != null)
                {
                    jsonWriter.WritePropertyName("namespaceName");
                    jsonWriter.Write(request.NamespaceName.ToString());
                }
                if (request.UserId != null)
                {
                    jsonWriter.WritePropertyName("userId");
                    jsonWriter.Write(request.UserId.ToString());
                }
                if (request.RatingName != null)
                {
                    jsonWriter.WritePropertyName("ratingName");
                    jsonWriter.Write(request.RatingName.ToString());
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
                    "matchmaking",
                    "rating",
                    "deleteRating",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator DeleteRating(
                Request.DeleteRatingRequest request,
                UnityAction<AsyncResult<Result.DeleteRatingResult>> callback
        )
		{
			var task = new DeleteRatingTask(
			    Gs2WebSocketSession,
			    request
            );
            yield return task;
            callback.Invoke(new AsyncResult<Result.DeleteRatingResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.DeleteRatingResult> DeleteRating(
            Request.DeleteRatingRequest request
        )
		{
		    var task = new DeleteRatingTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
#endif


        private class VoteTask : Gs2WebSocketSessionTask<Request.VoteRequest, Result.VoteResult>
        {
	        public VoteTask(IGs2Session session, Request.VoteRequest request) : base(session, request)
	        {
	        }

            protected override IGs2SessionRequest CreateRequest(Request.VoteRequest request)
            {
                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);

                jsonWriter.WriteObjectStart();

                if (request.NamespaceName != null)
                {
                    jsonWriter.WritePropertyName("namespaceName");
                    jsonWriter.Write(request.NamespaceName.ToString());
                }
                if (request.BallotBody != null)
                {
                    jsonWriter.WritePropertyName("ballotBody");
                    jsonWriter.Write(request.BallotBody.ToString());
                }
                if (request.BallotSignature != null)
                {
                    jsonWriter.WritePropertyName("ballotSignature");
                    jsonWriter.Write(request.BallotSignature.ToString());
                }
                if (request.GameResults != null)
                {
                    jsonWriter.WritePropertyName("gameResults");
                    jsonWriter.WriteArrayStart();
                    foreach(var item in request.GameResults)
                    {
                        item.WriteJson(jsonWriter);
                    }
                    jsonWriter.WriteArrayEnd();
                }
                if (request.KeyId != null)
                {
                    jsonWriter.WritePropertyName("keyId");
                    jsonWriter.Write(request.KeyId.ToString());
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
                    "matchmaking",
                    "vote",
                    "vote",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator Vote(
                Request.VoteRequest request,
                UnityAction<AsyncResult<Result.VoteResult>> callback
        )
		{
			var task = new VoteTask(
			    Gs2WebSocketSession,
			    request
            );
            yield return task;
            callback.Invoke(new AsyncResult<Result.VoteResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.VoteResult> Vote(
            Request.VoteRequest request
        )
		{
		    var task = new VoteTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
#endif


        private class VoteMultipleTask : Gs2WebSocketSessionTask<Request.VoteMultipleRequest, Result.VoteMultipleResult>
        {
	        public VoteMultipleTask(IGs2Session session, Request.VoteMultipleRequest request) : base(session, request)
	        {
	        }

            protected override IGs2SessionRequest CreateRequest(Request.VoteMultipleRequest request)
            {
                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);

                jsonWriter.WriteObjectStart();

                if (request.NamespaceName != null)
                {
                    jsonWriter.WritePropertyName("namespaceName");
                    jsonWriter.Write(request.NamespaceName.ToString());
                }
                if (request.SignedBallots != null)
                {
                    jsonWriter.WritePropertyName("signedBallots");
                    jsonWriter.WriteArrayStart();
                    foreach(var item in request.SignedBallots)
                    {
                        item.WriteJson(jsonWriter);
                    }
                    jsonWriter.WriteArrayEnd();
                }
                if (request.GameResults != null)
                {
                    jsonWriter.WritePropertyName("gameResults");
                    jsonWriter.WriteArrayStart();
                    foreach(var item in request.GameResults)
                    {
                        item.WriteJson(jsonWriter);
                    }
                    jsonWriter.WriteArrayEnd();
                }
                if (request.KeyId != null)
                {
                    jsonWriter.WritePropertyName("keyId");
                    jsonWriter.Write(request.KeyId.ToString());
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
                    "matchmaking",
                    "vote",
                    "voteMultiple",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator VoteMultiple(
                Request.VoteMultipleRequest request,
                UnityAction<AsyncResult<Result.VoteMultipleResult>> callback
        )
		{
			var task = new VoteMultipleTask(
			    Gs2WebSocketSession,
			    request
            );
            yield return task;
            callback.Invoke(new AsyncResult<Result.VoteMultipleResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.VoteMultipleResult> VoteMultiple(
            Request.VoteMultipleRequest request
        )
		{
		    var task = new VoteMultipleTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
#endif


        private class CommitVoteTask : Gs2WebSocketSessionTask<Request.CommitVoteRequest, Result.CommitVoteResult>
        {
	        public CommitVoteTask(IGs2Session session, Request.CommitVoteRequest request) : base(session, request)
	        {
	        }

            protected override IGs2SessionRequest CreateRequest(Request.CommitVoteRequest request)
            {
                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);

                jsonWriter.WriteObjectStart();

                if (request.NamespaceName != null)
                {
                    jsonWriter.WritePropertyName("namespaceName");
                    jsonWriter.Write(request.NamespaceName.ToString());
                }
                if (request.RatingName != null)
                {
                    jsonWriter.WritePropertyName("ratingName");
                    jsonWriter.Write(request.RatingName.ToString());
                }
                if (request.GatheringName != null)
                {
                    jsonWriter.WritePropertyName("gatheringName");
                    jsonWriter.Write(request.GatheringName.ToString());
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
                    "matchmaking",
                    "vote",
                    "commitVote",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator CommitVote(
                Request.CommitVoteRequest request,
                UnityAction<AsyncResult<Result.CommitVoteResult>> callback
        )
		{
			var task = new CommitVoteTask(
			    Gs2WebSocketSession,
			    request
            );
            yield return task;
            callback.Invoke(new AsyncResult<Result.CommitVoteResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.CommitVoteResult> CommitVote(
            Request.CommitVoteRequest request
        )
		{
		    var task = new CommitVoteTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
#endif
	}
}