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

namespace Gs2.Gs2Distributor
{
	public class Gs2DistributorWebSocketClient : AbstractGs2Client
	{

		public static string Endpoint = "distributor";

        protected Gs2WebSocketSession Gs2WebSocketSession => (Gs2WebSocketSession) Gs2Session;

		public Gs2DistributorWebSocketClient(Gs2WebSocketSession Gs2WebSocketSession) : base(Gs2WebSocketSession)
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
                if (request.AssumeUserId != null)
                {
                    jsonWriter.WritePropertyName("assumeUserId");
                    jsonWriter.Write(request.AssumeUserId.ToString());
                }
                if (request.AutoRunStampSheetNotification != null)
                {
                    jsonWriter.WritePropertyName("autoRunStampSheetNotification");
                    request.AutoRunStampSheetNotification.WriteJson(jsonWriter);
                }
                if (request.AutoRunTransactionNotification != null)
                {
                    jsonWriter.WritePropertyName("autoRunTransactionNotification");
                    request.AutoRunTransactionNotification.WriteJson(jsonWriter);
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
                if (request.DryRun)
                {
                    jsonWriter.WritePropertyName("xGs2DryRun");
                    jsonWriter.Write("true");
                }

                AddHeader(
                    Session.Credential,
                    "distributor",
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


        public class GetNamespaceStatusTask : Gs2WebSocketSessionTask<Request.GetNamespaceStatusRequest, Result.GetNamespaceStatusResult>
        {
	        public GetNamespaceStatusTask(IGs2Session session, Request.GetNamespaceStatusRequest request) : base(session, request)
	        {
	        }

            protected override IGs2SessionRequest CreateRequest(Request.GetNamespaceStatusRequest request)
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
                if (request.DryRun)
                {
                    jsonWriter.WritePropertyName("xGs2DryRun");
                    jsonWriter.Write("true");
                }

                AddHeader(
                    Session.Credential,
                    "distributor",
                    "namespace",
                    "getNamespaceStatus",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator GetNamespaceStatus(
                Request.GetNamespaceStatusRequest request,
                UnityAction<AsyncResult<Result.GetNamespaceStatusResult>> callback
        )
		{
			var task = new GetNamespaceStatusTask(
			    Gs2WebSocketSession,
			    request
            );
            yield return task;
            callback.Invoke(new AsyncResult<Result.GetNamespaceStatusResult>(task.Result, task.Error));
        }

		public IFuture<Result.GetNamespaceStatusResult> GetNamespaceStatusFuture(
                Request.GetNamespaceStatusRequest request
        )
		{
			return new GetNamespaceStatusTask(
			    Gs2WebSocketSession,
			    request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.GetNamespaceStatusResult> GetNamespaceStatusAsync(
            Request.GetNamespaceStatusRequest request
        )
		{
		    var task = new GetNamespaceStatusTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
    #else
		public GetNamespaceStatusTask GetNamespaceStatusAsync(
                Request.GetNamespaceStatusRequest request
        )
		{
			return new GetNamespaceStatusTask(
                Gs2WebSocketSession,
			    request
            );
        }
    #endif
#else
		public async Task<Result.GetNamespaceStatusResult> GetNamespaceStatusAsync(
            Request.GetNamespaceStatusRequest request
        )
		{
		    var task = new GetNamespaceStatusTask(
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
                if (request.DryRun)
                {
                    jsonWriter.WritePropertyName("xGs2DryRun");
                    jsonWriter.Write("true");
                }

                AddHeader(
                    Session.Credential,
                    "distributor",
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
                if (request.AssumeUserId != null)
                {
                    jsonWriter.WritePropertyName("assumeUserId");
                    jsonWriter.Write(request.AssumeUserId.ToString());
                }
                if (request.AutoRunStampSheetNotification != null)
                {
                    jsonWriter.WritePropertyName("autoRunStampSheetNotification");
                    request.AutoRunStampSheetNotification.WriteJson(jsonWriter);
                }
                if (request.AutoRunTransactionNotification != null)
                {
                    jsonWriter.WritePropertyName("autoRunTransactionNotification");
                    request.AutoRunTransactionNotification.WriteJson(jsonWriter);
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
                if (request.DryRun)
                {
                    jsonWriter.WritePropertyName("xGs2DryRun");
                    jsonWriter.Write("true");
                }

                AddHeader(
                    Session.Credential,
                    "distributor",
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
                if (request.DryRun)
                {
                    jsonWriter.WritePropertyName("xGs2DryRun");
                    jsonWriter.Write("true");
                }

                AddHeader(
                    Session.Credential,
                    "distributor",
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


        public class GetServiceVersionTask : Gs2WebSocketSessionTask<Request.GetServiceVersionRequest, Result.GetServiceVersionResult>
        {
	        public GetServiceVersionTask(IGs2Session session, Request.GetServiceVersionRequest request) : base(session, request)
	        {
	        }

            protected override IGs2SessionRequest CreateRequest(Request.GetServiceVersionRequest request)
            {
                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);

                jsonWriter.WriteObjectStart();

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
                    "distributor",
                    "namespace",
                    "getServiceVersion",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator GetServiceVersion(
                Request.GetServiceVersionRequest request,
                UnityAction<AsyncResult<Result.GetServiceVersionResult>> callback
        )
		{
			var task = new GetServiceVersionTask(
			    Gs2WebSocketSession,
			    request
            );
            yield return task;
            callback.Invoke(new AsyncResult<Result.GetServiceVersionResult>(task.Result, task.Error));
        }

		public IFuture<Result.GetServiceVersionResult> GetServiceVersionFuture(
                Request.GetServiceVersionRequest request
        )
		{
			return new GetServiceVersionTask(
			    Gs2WebSocketSession,
			    request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.GetServiceVersionResult> GetServiceVersionAsync(
            Request.GetServiceVersionRequest request
        )
		{
		    var task = new GetServiceVersionTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
    #else
		public GetServiceVersionTask GetServiceVersionAsync(
                Request.GetServiceVersionRequest request
        )
		{
			return new GetServiceVersionTask(
                Gs2WebSocketSession,
			    request
            );
        }
    #endif
#else
		public async Task<Result.GetServiceVersionResult> GetServiceVersionAsync(
            Request.GetServiceVersionRequest request
        )
		{
		    var task = new GetServiceVersionTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
#endif


        public class PreUpdateCurrentDistributorMasterTask : Gs2WebSocketSessionTask<Request.PreUpdateCurrentDistributorMasterRequest, Result.PreUpdateCurrentDistributorMasterResult>
        {
	        public PreUpdateCurrentDistributorMasterTask(IGs2Session session, Request.PreUpdateCurrentDistributorMasterRequest request) : base(session, request)
	        {
	        }

            protected override IGs2SessionRequest CreateRequest(Request.PreUpdateCurrentDistributorMasterRequest request)
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
                if (request.DryRun)
                {
                    jsonWriter.WritePropertyName("xGs2DryRun");
                    jsonWriter.Write("true");
                }

                AddHeader(
                    Session.Credential,
                    "distributor",
                    "currentDistributorMaster",
                    "preUpdateCurrentDistributorMaster",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator PreUpdateCurrentDistributorMaster(
                Request.PreUpdateCurrentDistributorMasterRequest request,
                UnityAction<AsyncResult<Result.PreUpdateCurrentDistributorMasterResult>> callback
        )
		{
			var task = new PreUpdateCurrentDistributorMasterTask(
			    Gs2WebSocketSession,
			    request
            );
            yield return task;
            callback.Invoke(new AsyncResult<Result.PreUpdateCurrentDistributorMasterResult>(task.Result, task.Error));
        }

		public IFuture<Result.PreUpdateCurrentDistributorMasterResult> PreUpdateCurrentDistributorMasterFuture(
                Request.PreUpdateCurrentDistributorMasterRequest request
        )
		{
			return new PreUpdateCurrentDistributorMasterTask(
			    Gs2WebSocketSession,
			    request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.PreUpdateCurrentDistributorMasterResult> PreUpdateCurrentDistributorMasterAsync(
            Request.PreUpdateCurrentDistributorMasterRequest request
        )
		{
		    var task = new PreUpdateCurrentDistributorMasterTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
    #else
		public PreUpdateCurrentDistributorMasterTask PreUpdateCurrentDistributorMasterAsync(
                Request.PreUpdateCurrentDistributorMasterRequest request
        )
		{
			return new PreUpdateCurrentDistributorMasterTask(
                Gs2WebSocketSession,
			    request
            );
        }
    #endif
#else
		public async Task<Result.PreUpdateCurrentDistributorMasterResult> PreUpdateCurrentDistributorMasterAsync(
            Request.PreUpdateCurrentDistributorMasterRequest request
        )
		{
		    var task = new PreUpdateCurrentDistributorMasterTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
#endif


        public class IfExpressionByUserIdTask : Gs2WebSocketSessionTask<Request.IfExpressionByUserIdRequest, Result.IfExpressionByUserIdResult>
        {
	        public IfExpressionByUserIdTask(IGs2Session session, Request.IfExpressionByUserIdRequest request) : base(session, request)
	        {
	        }

            protected override IGs2SessionRequest CreateRequest(Request.IfExpressionByUserIdRequest request)
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
                if (request.Condition != null)
                {
                    jsonWriter.WritePropertyName("condition");
                    request.Condition.WriteJson(jsonWriter);
                }
                if (request.TrueActions != null)
                {
                    jsonWriter.WritePropertyName("trueActions");
                    jsonWriter.WriteArrayStart();
                    foreach(var item in request.TrueActions)
                    {
                        item.WriteJson(jsonWriter);
                    }
                    jsonWriter.WriteArrayEnd();
                }
                if (request.FalseActions != null)
                {
                    jsonWriter.WritePropertyName("falseActions");
                    jsonWriter.WriteArrayStart();
                    foreach(var item in request.FalseActions)
                    {
                        item.WriteJson(jsonWriter);
                    }
                    jsonWriter.WriteArrayEnd();
                }
                if (request.MultiplyValueSpecifyingQuantity != null)
                {
                    jsonWriter.WritePropertyName("multiplyValueSpecifyingQuantity");
                    jsonWriter.Write(request.MultiplyValueSpecifyingQuantity.ToString());
                }
                if (request.TimeOffsetToken != null)
                {
                    jsonWriter.WritePropertyName("timeOffsetToken");
                    jsonWriter.Write(request.TimeOffsetToken.ToString());
                }
                if (request.ContextStack != null)
                {
                    jsonWriter.WritePropertyName("contextStack");
                    jsonWriter.Write(request.ContextStack.ToString());
                }
                if (request.DuplicationAvoider != null)
                {
                    jsonWriter.WritePropertyName("xGs2DuplicationAvoider");
                    jsonWriter.Write(request.DuplicationAvoider);
                }
                if (request.DryRun)
                {
                    jsonWriter.WritePropertyName("xGs2DryRun");
                    jsonWriter.Write("true");
                }

                AddHeader(
                    Session.Credential,
                    "distributor",
                    "expression",
                    "ifExpressionByUserId",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator IfExpressionByUserId(
                Request.IfExpressionByUserIdRequest request,
                UnityAction<AsyncResult<Result.IfExpressionByUserIdResult>> callback
        )
		{
			var task = new IfExpressionByUserIdTask(
			    Gs2WebSocketSession,
			    request
            );
            yield return task;
            callback.Invoke(new AsyncResult<Result.IfExpressionByUserIdResult>(task.Result, task.Error));
        }

		public IFuture<Result.IfExpressionByUserIdResult> IfExpressionByUserIdFuture(
                Request.IfExpressionByUserIdRequest request
        )
		{
			return new IfExpressionByUserIdTask(
			    Gs2WebSocketSession,
			    request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.IfExpressionByUserIdResult> IfExpressionByUserIdAsync(
            Request.IfExpressionByUserIdRequest request
        )
		{
		    var task = new IfExpressionByUserIdTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
    #else
		public IfExpressionByUserIdTask IfExpressionByUserIdAsync(
                Request.IfExpressionByUserIdRequest request
        )
		{
			return new IfExpressionByUserIdTask(
                Gs2WebSocketSession,
			    request
            );
        }
    #endif
#else
		public async Task<Result.IfExpressionByUserIdResult> IfExpressionByUserIdAsync(
            Request.IfExpressionByUserIdRequest request
        )
		{
		    var task = new IfExpressionByUserIdTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
#endif


        public class AndExpressionByUserIdTask : Gs2WebSocketSessionTask<Request.AndExpressionByUserIdRequest, Result.AndExpressionByUserIdResult>
        {
	        public AndExpressionByUserIdTask(IGs2Session session, Request.AndExpressionByUserIdRequest request) : base(session, request)
	        {
	        }

            protected override IGs2SessionRequest CreateRequest(Request.AndExpressionByUserIdRequest request)
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
                if (request.Actions != null)
                {
                    jsonWriter.WritePropertyName("actions");
                    jsonWriter.WriteArrayStart();
                    foreach(var item in request.Actions)
                    {
                        item.WriteJson(jsonWriter);
                    }
                    jsonWriter.WriteArrayEnd();
                }
                if (request.TimeOffsetToken != null)
                {
                    jsonWriter.WritePropertyName("timeOffsetToken");
                    jsonWriter.Write(request.TimeOffsetToken.ToString());
                }
                if (request.ContextStack != null)
                {
                    jsonWriter.WritePropertyName("contextStack");
                    jsonWriter.Write(request.ContextStack.ToString());
                }
                if (request.DuplicationAvoider != null)
                {
                    jsonWriter.WritePropertyName("xGs2DuplicationAvoider");
                    jsonWriter.Write(request.DuplicationAvoider);
                }
                if (request.DryRun)
                {
                    jsonWriter.WritePropertyName("xGs2DryRun");
                    jsonWriter.Write("true");
                }

                AddHeader(
                    Session.Credential,
                    "distributor",
                    "expression",
                    "andExpressionByUserId",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator AndExpressionByUserId(
                Request.AndExpressionByUserIdRequest request,
                UnityAction<AsyncResult<Result.AndExpressionByUserIdResult>> callback
        )
		{
			var task = new AndExpressionByUserIdTask(
			    Gs2WebSocketSession,
			    request
            );
            yield return task;
            callback.Invoke(new AsyncResult<Result.AndExpressionByUserIdResult>(task.Result, task.Error));
        }

		public IFuture<Result.AndExpressionByUserIdResult> AndExpressionByUserIdFuture(
                Request.AndExpressionByUserIdRequest request
        )
		{
			return new AndExpressionByUserIdTask(
			    Gs2WebSocketSession,
			    request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.AndExpressionByUserIdResult> AndExpressionByUserIdAsync(
            Request.AndExpressionByUserIdRequest request
        )
		{
		    var task = new AndExpressionByUserIdTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
    #else
		public AndExpressionByUserIdTask AndExpressionByUserIdAsync(
                Request.AndExpressionByUserIdRequest request
        )
		{
			return new AndExpressionByUserIdTask(
                Gs2WebSocketSession,
			    request
            );
        }
    #endif
#else
		public async Task<Result.AndExpressionByUserIdResult> AndExpressionByUserIdAsync(
            Request.AndExpressionByUserIdRequest request
        )
		{
		    var task = new AndExpressionByUserIdTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
#endif


        public class OrExpressionByUserIdTask : Gs2WebSocketSessionTask<Request.OrExpressionByUserIdRequest, Result.OrExpressionByUserIdResult>
        {
	        public OrExpressionByUserIdTask(IGs2Session session, Request.OrExpressionByUserIdRequest request) : base(session, request)
	        {
	        }

            protected override IGs2SessionRequest CreateRequest(Request.OrExpressionByUserIdRequest request)
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
                if (request.Actions != null)
                {
                    jsonWriter.WritePropertyName("actions");
                    jsonWriter.WriteArrayStart();
                    foreach(var item in request.Actions)
                    {
                        item.WriteJson(jsonWriter);
                    }
                    jsonWriter.WriteArrayEnd();
                }
                if (request.TimeOffsetToken != null)
                {
                    jsonWriter.WritePropertyName("timeOffsetToken");
                    jsonWriter.Write(request.TimeOffsetToken.ToString());
                }
                if (request.ContextStack != null)
                {
                    jsonWriter.WritePropertyName("contextStack");
                    jsonWriter.Write(request.ContextStack.ToString());
                }
                if (request.DuplicationAvoider != null)
                {
                    jsonWriter.WritePropertyName("xGs2DuplicationAvoider");
                    jsonWriter.Write(request.DuplicationAvoider);
                }
                if (request.DryRun)
                {
                    jsonWriter.WritePropertyName("xGs2DryRun");
                    jsonWriter.Write("true");
                }

                AddHeader(
                    Session.Credential,
                    "distributor",
                    "expression",
                    "orExpressionByUserId",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator OrExpressionByUserId(
                Request.OrExpressionByUserIdRequest request,
                UnityAction<AsyncResult<Result.OrExpressionByUserIdResult>> callback
        )
		{
			var task = new OrExpressionByUserIdTask(
			    Gs2WebSocketSession,
			    request
            );
            yield return task;
            callback.Invoke(new AsyncResult<Result.OrExpressionByUserIdResult>(task.Result, task.Error));
        }

		public IFuture<Result.OrExpressionByUserIdResult> OrExpressionByUserIdFuture(
                Request.OrExpressionByUserIdRequest request
        )
		{
			return new OrExpressionByUserIdTask(
			    Gs2WebSocketSession,
			    request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.OrExpressionByUserIdResult> OrExpressionByUserIdAsync(
            Request.OrExpressionByUserIdRequest request
        )
		{
		    var task = new OrExpressionByUserIdTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
    #else
		public OrExpressionByUserIdTask OrExpressionByUserIdAsync(
                Request.OrExpressionByUserIdRequest request
        )
		{
			return new OrExpressionByUserIdTask(
                Gs2WebSocketSession,
			    request
            );
        }
    #endif
#else
		public async Task<Result.OrExpressionByUserIdResult> OrExpressionByUserIdAsync(
            Request.OrExpressionByUserIdRequest request
        )
		{
		    var task = new OrExpressionByUserIdTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
#endif
	}
}