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
    #if GS2_ENABLE_UNITASK
using Cysharp.Threading.Tasks;
    #endif
#else
using System.Threading.Tasks;
using System.Threading;
#endif

namespace Gs2.Gs2Idle
{
	public class Gs2IdleWebSocketClient : AbstractGs2Client
	{

		public static string Endpoint = "idle";

        protected Gs2WebSocketSession Gs2WebSocketSession => (Gs2WebSocketSession) Gs2Session;

		public Gs2IdleWebSocketClient(Gs2WebSocketSession Gs2WebSocketSession) : base(Gs2WebSocketSession)
		{

		}


        public class CreateNamespaceTask : Gs2WebSocketSessionTask<Request.CreateNamespaceRequest, Result.CreateNamespaceResult>
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
                if (request.TransactionSetting != null)
                {
                    jsonWriter.WritePropertyName("transactionSetting");
                    request.TransactionSetting.WriteJson(jsonWriter);
                }
                if (request.ReceiveScript != null)
                {
                    jsonWriter.WritePropertyName("receiveScript");
                    request.ReceiveScript.WriteJson(jsonWriter);
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
                    "idle",
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

		public IFuture<Result.CreateNamespaceResult> CreateNamespaceFuture(
                Request.CreateNamespaceRequest request
        )
		{
			return new CreateNamespaceTask(
			    Gs2WebSocketSession,
			    request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.CreateNamespaceResult> CreateNamespaceAsync(
            Request.CreateNamespaceRequest request
        )
		{
		    var task = new CreateNamespaceTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
    #else
		public CreateNamespaceTask CreateNamespaceAsync(
                Request.CreateNamespaceRequest request
        )
		{
			return new CreateNamespaceTask(
                Gs2WebSocketSession,
			    request
            );
        }
    #endif
#else
		public async Task<Result.CreateNamespaceResult> CreateNamespaceAsync(
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


        public class GetNamespaceTask : Gs2WebSocketSessionTask<Request.GetNamespaceRequest, Result.GetNamespaceResult>
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
                    "idle",
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

		public IFuture<Result.GetNamespaceResult> GetNamespaceFuture(
                Request.GetNamespaceRequest request
        )
		{
			return new GetNamespaceTask(
			    Gs2WebSocketSession,
			    request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.GetNamespaceResult> GetNamespaceAsync(
            Request.GetNamespaceRequest request
        )
		{
		    var task = new GetNamespaceTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
    #else
		public GetNamespaceTask GetNamespaceAsync(
                Request.GetNamespaceRequest request
        )
		{
			return new GetNamespaceTask(
                Gs2WebSocketSession,
			    request
            );
        }
    #endif
#else
		public async Task<Result.GetNamespaceResult> GetNamespaceAsync(
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


        public class UpdateNamespaceTask : Gs2WebSocketSessionTask<Request.UpdateNamespaceRequest, Result.UpdateNamespaceResult>
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
                if (request.TransactionSetting != null)
                {
                    jsonWriter.WritePropertyName("transactionSetting");
                    request.TransactionSetting.WriteJson(jsonWriter);
                }
                if (request.ReceiveScript != null)
                {
                    jsonWriter.WritePropertyName("receiveScript");
                    request.ReceiveScript.WriteJson(jsonWriter);
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
                    "idle",
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

		public IFuture<Result.UpdateNamespaceResult> UpdateNamespaceFuture(
                Request.UpdateNamespaceRequest request
        )
		{
			return new UpdateNamespaceTask(
			    Gs2WebSocketSession,
			    request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.UpdateNamespaceResult> UpdateNamespaceAsync(
            Request.UpdateNamespaceRequest request
        )
		{
		    var task = new UpdateNamespaceTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
    #else
		public UpdateNamespaceTask UpdateNamespaceAsync(
                Request.UpdateNamespaceRequest request
        )
		{
			return new UpdateNamespaceTask(
                Gs2WebSocketSession,
			    request
            );
        }
    #endif
#else
		public async Task<Result.UpdateNamespaceResult> UpdateNamespaceAsync(
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


        public class DeleteNamespaceTask : Gs2WebSocketSessionTask<Request.DeleteNamespaceRequest, Result.DeleteNamespaceResult>
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
                    "idle",
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

		public IFuture<Result.DeleteNamespaceResult> DeleteNamespaceFuture(
                Request.DeleteNamespaceRequest request
        )
		{
			return new DeleteNamespaceTask(
			    Gs2WebSocketSession,
			    request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.DeleteNamespaceResult> DeleteNamespaceAsync(
            Request.DeleteNamespaceRequest request
        )
		{
		    var task = new DeleteNamespaceTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
    #else
		public DeleteNamespaceTask DeleteNamespaceAsync(
                Request.DeleteNamespaceRequest request
        )
		{
			return new DeleteNamespaceTask(
                Gs2WebSocketSession,
			    request
            );
        }
    #endif
#else
		public async Task<Result.DeleteNamespaceResult> DeleteNamespaceAsync(
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


        public class GetStatusTask : Gs2WebSocketSessionTask<Request.GetStatusRequest, Result.GetStatusResult>
        {
	        public GetStatusTask(IGs2Session session, Request.GetStatusRequest request) : base(session, request)
	        {
	        }

            protected override IGs2SessionRequest CreateRequest(Request.GetStatusRequest request)
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
                if (request.CategoryName != null)
                {
                    jsonWriter.WritePropertyName("categoryName");
                    jsonWriter.Write(request.CategoryName.ToString());
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
                    "idle",
                    "status",
                    "getStatus",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator GetStatus(
                Request.GetStatusRequest request,
                UnityAction<AsyncResult<Result.GetStatusResult>> callback
        )
		{
			var task = new GetStatusTask(
			    Gs2WebSocketSession,
			    request
            );
            yield return task;
            callback.Invoke(new AsyncResult<Result.GetStatusResult>(task.Result, task.Error));
        }

		public IFuture<Result.GetStatusResult> GetStatusFuture(
                Request.GetStatusRequest request
        )
		{
			return new GetStatusTask(
			    Gs2WebSocketSession,
			    request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.GetStatusResult> GetStatusAsync(
            Request.GetStatusRequest request
        )
		{
		    var task = new GetStatusTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
    #else
		public GetStatusTask GetStatusAsync(
                Request.GetStatusRequest request
        )
		{
			return new GetStatusTask(
                Gs2WebSocketSession,
			    request
            );
        }
    #endif
#else
		public async Task<Result.GetStatusResult> GetStatusAsync(
            Request.GetStatusRequest request
        )
		{
		    var task = new GetStatusTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
#endif


        public class GetStatusByUserIdTask : Gs2WebSocketSessionTask<Request.GetStatusByUserIdRequest, Result.GetStatusByUserIdResult>
        {
	        public GetStatusByUserIdTask(IGs2Session session, Request.GetStatusByUserIdRequest request) : base(session, request)
	        {
	        }

            protected override IGs2SessionRequest CreateRequest(Request.GetStatusByUserIdRequest request)
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
                if (request.CategoryName != null)
                {
                    jsonWriter.WritePropertyName("categoryName");
                    jsonWriter.Write(request.CategoryName.ToString());
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
                    "idle",
                    "status",
                    "getStatusByUserId",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator GetStatusByUserId(
                Request.GetStatusByUserIdRequest request,
                UnityAction<AsyncResult<Result.GetStatusByUserIdResult>> callback
        )
		{
			var task = new GetStatusByUserIdTask(
			    Gs2WebSocketSession,
			    request
            );
            yield return task;
            callback.Invoke(new AsyncResult<Result.GetStatusByUserIdResult>(task.Result, task.Error));
        }

		public IFuture<Result.GetStatusByUserIdResult> GetStatusByUserIdFuture(
                Request.GetStatusByUserIdRequest request
        )
		{
			return new GetStatusByUserIdTask(
			    Gs2WebSocketSession,
			    request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.GetStatusByUserIdResult> GetStatusByUserIdAsync(
            Request.GetStatusByUserIdRequest request
        )
		{
		    var task = new GetStatusByUserIdTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
    #else
		public GetStatusByUserIdTask GetStatusByUserIdAsync(
                Request.GetStatusByUserIdRequest request
        )
		{
			return new GetStatusByUserIdTask(
                Gs2WebSocketSession,
			    request
            );
        }
    #endif
#else
		public async Task<Result.GetStatusByUserIdResult> GetStatusByUserIdAsync(
            Request.GetStatusByUserIdRequest request
        )
		{
		    var task = new GetStatusByUserIdTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
#endif


        public class IncreaseMaximumIdleMinutesByUserIdTask : Gs2WebSocketSessionTask<Request.IncreaseMaximumIdleMinutesByUserIdRequest, Result.IncreaseMaximumIdleMinutesByUserIdResult>
        {
	        public IncreaseMaximumIdleMinutesByUserIdTask(IGs2Session session, Request.IncreaseMaximumIdleMinutesByUserIdRequest request) : base(session, request)
	        {
	        }

            protected override IGs2SessionRequest CreateRequest(Request.IncreaseMaximumIdleMinutesByUserIdRequest request)
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
                if (request.CategoryName != null)
                {
                    jsonWriter.WritePropertyName("categoryName");
                    jsonWriter.Write(request.CategoryName.ToString());
                }
                if (request.IncreaseMinutes != null)
                {
                    jsonWriter.WritePropertyName("increaseMinutes");
                    jsonWriter.Write(request.IncreaseMinutes.ToString());
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
                if (request.DuplicationAvoider != null)
                {
                    jsonWriter.WritePropertyName("xGs2DuplicationAvoider");
                    jsonWriter.Write(request.DuplicationAvoider);
                }

                AddHeader(
                    Session.Credential,
                    "idle",
                    "status",
                    "increaseMaximumIdleMinutesByUserId",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator IncreaseMaximumIdleMinutesByUserId(
                Request.IncreaseMaximumIdleMinutesByUserIdRequest request,
                UnityAction<AsyncResult<Result.IncreaseMaximumIdleMinutesByUserIdResult>> callback
        )
		{
			var task = new IncreaseMaximumIdleMinutesByUserIdTask(
			    Gs2WebSocketSession,
			    request
            );
            yield return task;
            callback.Invoke(new AsyncResult<Result.IncreaseMaximumIdleMinutesByUserIdResult>(task.Result, task.Error));
        }

		public IFuture<Result.IncreaseMaximumIdleMinutesByUserIdResult> IncreaseMaximumIdleMinutesByUserIdFuture(
                Request.IncreaseMaximumIdleMinutesByUserIdRequest request
        )
		{
			return new IncreaseMaximumIdleMinutesByUserIdTask(
			    Gs2WebSocketSession,
			    request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.IncreaseMaximumIdleMinutesByUserIdResult> IncreaseMaximumIdleMinutesByUserIdAsync(
            Request.IncreaseMaximumIdleMinutesByUserIdRequest request
        )
		{
		    var task = new IncreaseMaximumIdleMinutesByUserIdTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
    #else
		public IncreaseMaximumIdleMinutesByUserIdTask IncreaseMaximumIdleMinutesByUserIdAsync(
                Request.IncreaseMaximumIdleMinutesByUserIdRequest request
        )
		{
			return new IncreaseMaximumIdleMinutesByUserIdTask(
                Gs2WebSocketSession,
			    request
            );
        }
    #endif
#else
		public async Task<Result.IncreaseMaximumIdleMinutesByUserIdResult> IncreaseMaximumIdleMinutesByUserIdAsync(
            Request.IncreaseMaximumIdleMinutesByUserIdRequest request
        )
		{
		    var task = new IncreaseMaximumIdleMinutesByUserIdTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
#endif


        public class DecreaseMaximumIdleMinutesByUserIdTask : Gs2WebSocketSessionTask<Request.DecreaseMaximumIdleMinutesByUserIdRequest, Result.DecreaseMaximumIdleMinutesByUserIdResult>
        {
	        public DecreaseMaximumIdleMinutesByUserIdTask(IGs2Session session, Request.DecreaseMaximumIdleMinutesByUserIdRequest request) : base(session, request)
	        {
	        }

            protected override IGs2SessionRequest CreateRequest(Request.DecreaseMaximumIdleMinutesByUserIdRequest request)
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
                if (request.CategoryName != null)
                {
                    jsonWriter.WritePropertyName("categoryName");
                    jsonWriter.Write(request.CategoryName.ToString());
                }
                if (request.DecreaseMinutes != null)
                {
                    jsonWriter.WritePropertyName("decreaseMinutes");
                    jsonWriter.Write(request.DecreaseMinutes.ToString());
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
                if (request.DuplicationAvoider != null)
                {
                    jsonWriter.WritePropertyName("xGs2DuplicationAvoider");
                    jsonWriter.Write(request.DuplicationAvoider);
                }

                AddHeader(
                    Session.Credential,
                    "idle",
                    "status",
                    "decreaseMaximumIdleMinutesByUserId",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator DecreaseMaximumIdleMinutesByUserId(
                Request.DecreaseMaximumIdleMinutesByUserIdRequest request,
                UnityAction<AsyncResult<Result.DecreaseMaximumIdleMinutesByUserIdResult>> callback
        )
		{
			var task = new DecreaseMaximumIdleMinutesByUserIdTask(
			    Gs2WebSocketSession,
			    request
            );
            yield return task;
            callback.Invoke(new AsyncResult<Result.DecreaseMaximumIdleMinutesByUserIdResult>(task.Result, task.Error));
        }

		public IFuture<Result.DecreaseMaximumIdleMinutesByUserIdResult> DecreaseMaximumIdleMinutesByUserIdFuture(
                Request.DecreaseMaximumIdleMinutesByUserIdRequest request
        )
		{
			return new DecreaseMaximumIdleMinutesByUserIdTask(
			    Gs2WebSocketSession,
			    request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.DecreaseMaximumIdleMinutesByUserIdResult> DecreaseMaximumIdleMinutesByUserIdAsync(
            Request.DecreaseMaximumIdleMinutesByUserIdRequest request
        )
		{
		    var task = new DecreaseMaximumIdleMinutesByUserIdTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
    #else
		public DecreaseMaximumIdleMinutesByUserIdTask DecreaseMaximumIdleMinutesByUserIdAsync(
                Request.DecreaseMaximumIdleMinutesByUserIdRequest request
        )
		{
			return new DecreaseMaximumIdleMinutesByUserIdTask(
                Gs2WebSocketSession,
			    request
            );
        }
    #endif
#else
		public async Task<Result.DecreaseMaximumIdleMinutesByUserIdResult> DecreaseMaximumIdleMinutesByUserIdAsync(
            Request.DecreaseMaximumIdleMinutesByUserIdRequest request
        )
		{
		    var task = new DecreaseMaximumIdleMinutesByUserIdTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
#endif


        public class IncreaseMaximumIdleMinutesByStampSheetTask : Gs2WebSocketSessionTask<Request.IncreaseMaximumIdleMinutesByStampSheetRequest, Result.IncreaseMaximumIdleMinutesByStampSheetResult>
        {
	        public IncreaseMaximumIdleMinutesByStampSheetTask(IGs2Session session, Request.IncreaseMaximumIdleMinutesByStampSheetRequest request) : base(session, request)
	        {
	        }

            protected override IGs2SessionRequest CreateRequest(Request.IncreaseMaximumIdleMinutesByStampSheetRequest request)
            {
                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);

                jsonWriter.WriteObjectStart();

                if (request.StampSheet != null)
                {
                    jsonWriter.WritePropertyName("stampSheet");
                    jsonWriter.Write(request.StampSheet.ToString());
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
                    "idle",
                    "status",
                    "increaseMaximumIdleMinutesByStampSheet",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator IncreaseMaximumIdleMinutesByStampSheet(
                Request.IncreaseMaximumIdleMinutesByStampSheetRequest request,
                UnityAction<AsyncResult<Result.IncreaseMaximumIdleMinutesByStampSheetResult>> callback
        )
		{
			var task = new IncreaseMaximumIdleMinutesByStampSheetTask(
			    Gs2WebSocketSession,
			    request
            );
            yield return task;
            callback.Invoke(new AsyncResult<Result.IncreaseMaximumIdleMinutesByStampSheetResult>(task.Result, task.Error));
        }

		public IFuture<Result.IncreaseMaximumIdleMinutesByStampSheetResult> IncreaseMaximumIdleMinutesByStampSheetFuture(
                Request.IncreaseMaximumIdleMinutesByStampSheetRequest request
        )
		{
			return new IncreaseMaximumIdleMinutesByStampSheetTask(
			    Gs2WebSocketSession,
			    request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.IncreaseMaximumIdleMinutesByStampSheetResult> IncreaseMaximumIdleMinutesByStampSheetAsync(
            Request.IncreaseMaximumIdleMinutesByStampSheetRequest request
        )
		{
		    var task = new IncreaseMaximumIdleMinutesByStampSheetTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
    #else
		public IncreaseMaximumIdleMinutesByStampSheetTask IncreaseMaximumIdleMinutesByStampSheetAsync(
                Request.IncreaseMaximumIdleMinutesByStampSheetRequest request
        )
		{
			return new IncreaseMaximumIdleMinutesByStampSheetTask(
                Gs2WebSocketSession,
			    request
            );
        }
    #endif
#else
		public async Task<Result.IncreaseMaximumIdleMinutesByStampSheetResult> IncreaseMaximumIdleMinutesByStampSheetAsync(
            Request.IncreaseMaximumIdleMinutesByStampSheetRequest request
        )
		{
		    var task = new IncreaseMaximumIdleMinutesByStampSheetTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
#endif
	}
}