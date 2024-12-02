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

namespace Gs2.Gs2Inventory
{
	public class Gs2InventoryWebSocketClient : AbstractGs2Client
	{

		public static string Endpoint = "inventory";

        protected Gs2WebSocketSession Gs2WebSocketSession => (Gs2WebSocketSession) Gs2Session;

		public Gs2InventoryWebSocketClient(Gs2WebSocketSession Gs2WebSocketSession) : base(Gs2WebSocketSession)
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
                if (request.AcquireScript != null)
                {
                    jsonWriter.WritePropertyName("acquireScript");
                    request.AcquireScript.WriteJson(jsonWriter);
                }
                if (request.OverflowScript != null)
                {
                    jsonWriter.WritePropertyName("overflowScript");
                    request.OverflowScript.WriteJson(jsonWriter);
                }
                if (request.ConsumeScript != null)
                {
                    jsonWriter.WritePropertyName("consumeScript");
                    request.ConsumeScript.WriteJson(jsonWriter);
                }
                if (request.SimpleItemAcquireScript != null)
                {
                    jsonWriter.WritePropertyName("simpleItemAcquireScript");
                    request.SimpleItemAcquireScript.WriteJson(jsonWriter);
                }
                if (request.SimpleItemConsumeScript != null)
                {
                    jsonWriter.WritePropertyName("simpleItemConsumeScript");
                    request.SimpleItemConsumeScript.WriteJson(jsonWriter);
                }
                if (request.BigItemAcquireScript != null)
                {
                    jsonWriter.WritePropertyName("bigItemAcquireScript");
                    request.BigItemAcquireScript.WriteJson(jsonWriter);
                }
                if (request.BigItemConsumeScript != null)
                {
                    jsonWriter.WritePropertyName("bigItemConsumeScript");
                    request.BigItemConsumeScript.WriteJson(jsonWriter);
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
                    "inventory",
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
                if (request.DryRun)
                {
                    jsonWriter.WritePropertyName("xGs2DryRun");
                    jsonWriter.Write("true");
                }

                AddHeader(
                    Session.Credential,
                    "inventory",
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
                if (request.AcquireScript != null)
                {
                    jsonWriter.WritePropertyName("acquireScript");
                    request.AcquireScript.WriteJson(jsonWriter);
                }
                if (request.OverflowScript != null)
                {
                    jsonWriter.WritePropertyName("overflowScript");
                    request.OverflowScript.WriteJson(jsonWriter);
                }
                if (request.ConsumeScript != null)
                {
                    jsonWriter.WritePropertyName("consumeScript");
                    request.ConsumeScript.WriteJson(jsonWriter);
                }
                if (request.SimpleItemAcquireScript != null)
                {
                    jsonWriter.WritePropertyName("simpleItemAcquireScript");
                    request.SimpleItemAcquireScript.WriteJson(jsonWriter);
                }
                if (request.SimpleItemConsumeScript != null)
                {
                    jsonWriter.WritePropertyName("simpleItemConsumeScript");
                    request.SimpleItemConsumeScript.WriteJson(jsonWriter);
                }
                if (request.BigItemAcquireScript != null)
                {
                    jsonWriter.WritePropertyName("bigItemAcquireScript");
                    request.BigItemAcquireScript.WriteJson(jsonWriter);
                }
                if (request.BigItemConsumeScript != null)
                {
                    jsonWriter.WritePropertyName("bigItemConsumeScript");
                    request.BigItemConsumeScript.WriteJson(jsonWriter);
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
                    "inventory",
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
                    "inventory",
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


        public class DumpUserDataByUserIdTask : Gs2WebSocketSessionTask<Request.DumpUserDataByUserIdRequest, Result.DumpUserDataByUserIdResult>
        {
	        public DumpUserDataByUserIdTask(IGs2Session session, Request.DumpUserDataByUserIdRequest request) : base(session, request)
	        {
	        }

            protected override IGs2SessionRequest CreateRequest(Request.DumpUserDataByUserIdRequest request)
            {
                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);

                jsonWriter.WriteObjectStart();

                if (request.UserId != null)
                {
                    jsonWriter.WritePropertyName("userId");
                    jsonWriter.Write(request.UserId.ToString());
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
                if (request.DryRun)
                {
                    jsonWriter.WritePropertyName("xGs2DryRun");
                    jsonWriter.Write("true");
                }

                AddHeader(
                    Session.Credential,
                    "inventory",
                    "namespace",
                    "dumpUserDataByUserId",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator DumpUserDataByUserId(
                Request.DumpUserDataByUserIdRequest request,
                UnityAction<AsyncResult<Result.DumpUserDataByUserIdResult>> callback
        )
		{
			var task = new DumpUserDataByUserIdTask(
			    Gs2WebSocketSession,
			    request
            );
            yield return task;
            callback.Invoke(new AsyncResult<Result.DumpUserDataByUserIdResult>(task.Result, task.Error));
        }

		public IFuture<Result.DumpUserDataByUserIdResult> DumpUserDataByUserIdFuture(
                Request.DumpUserDataByUserIdRequest request
        )
		{
			return new DumpUserDataByUserIdTask(
			    Gs2WebSocketSession,
			    request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.DumpUserDataByUserIdResult> DumpUserDataByUserIdAsync(
            Request.DumpUserDataByUserIdRequest request
        )
		{
		    var task = new DumpUserDataByUserIdTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
    #else
		public DumpUserDataByUserIdTask DumpUserDataByUserIdAsync(
                Request.DumpUserDataByUserIdRequest request
        )
		{
			return new DumpUserDataByUserIdTask(
                Gs2WebSocketSession,
			    request
            );
        }
    #endif
#else
		public async Task<Result.DumpUserDataByUserIdResult> DumpUserDataByUserIdAsync(
            Request.DumpUserDataByUserIdRequest request
        )
		{
		    var task = new DumpUserDataByUserIdTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
#endif


        public class CheckDumpUserDataByUserIdTask : Gs2WebSocketSessionTask<Request.CheckDumpUserDataByUserIdRequest, Result.CheckDumpUserDataByUserIdResult>
        {
	        public CheckDumpUserDataByUserIdTask(IGs2Session session, Request.CheckDumpUserDataByUserIdRequest request) : base(session, request)
	        {
	        }

            protected override IGs2SessionRequest CreateRequest(Request.CheckDumpUserDataByUserIdRequest request)
            {
                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);

                jsonWriter.WriteObjectStart();

                if (request.UserId != null)
                {
                    jsonWriter.WritePropertyName("userId");
                    jsonWriter.Write(request.UserId.ToString());
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
                if (request.DryRun)
                {
                    jsonWriter.WritePropertyName("xGs2DryRun");
                    jsonWriter.Write("true");
                }

                AddHeader(
                    Session.Credential,
                    "inventory",
                    "namespace",
                    "checkDumpUserDataByUserId",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator CheckDumpUserDataByUserId(
                Request.CheckDumpUserDataByUserIdRequest request,
                UnityAction<AsyncResult<Result.CheckDumpUserDataByUserIdResult>> callback
        )
		{
			var task = new CheckDumpUserDataByUserIdTask(
			    Gs2WebSocketSession,
			    request
            );
            yield return task;
            callback.Invoke(new AsyncResult<Result.CheckDumpUserDataByUserIdResult>(task.Result, task.Error));
        }

		public IFuture<Result.CheckDumpUserDataByUserIdResult> CheckDumpUserDataByUserIdFuture(
                Request.CheckDumpUserDataByUserIdRequest request
        )
		{
			return new CheckDumpUserDataByUserIdTask(
			    Gs2WebSocketSession,
			    request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.CheckDumpUserDataByUserIdResult> CheckDumpUserDataByUserIdAsync(
            Request.CheckDumpUserDataByUserIdRequest request
        )
		{
		    var task = new CheckDumpUserDataByUserIdTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
    #else
		public CheckDumpUserDataByUserIdTask CheckDumpUserDataByUserIdAsync(
                Request.CheckDumpUserDataByUserIdRequest request
        )
		{
			return new CheckDumpUserDataByUserIdTask(
                Gs2WebSocketSession,
			    request
            );
        }
    #endif
#else
		public async Task<Result.CheckDumpUserDataByUserIdResult> CheckDumpUserDataByUserIdAsync(
            Request.CheckDumpUserDataByUserIdRequest request
        )
		{
		    var task = new CheckDumpUserDataByUserIdTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
#endif


        public class CleanUserDataByUserIdTask : Gs2WebSocketSessionTask<Request.CleanUserDataByUserIdRequest, Result.CleanUserDataByUserIdResult>
        {
	        public CleanUserDataByUserIdTask(IGs2Session session, Request.CleanUserDataByUserIdRequest request) : base(session, request)
	        {
	        }

            protected override IGs2SessionRequest CreateRequest(Request.CleanUserDataByUserIdRequest request)
            {
                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);

                jsonWriter.WriteObjectStart();

                if (request.UserId != null)
                {
                    jsonWriter.WritePropertyName("userId");
                    jsonWriter.Write(request.UserId.ToString());
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
                if (request.DryRun)
                {
                    jsonWriter.WritePropertyName("xGs2DryRun");
                    jsonWriter.Write("true");
                }

                AddHeader(
                    Session.Credential,
                    "inventory",
                    "namespace",
                    "cleanUserDataByUserId",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator CleanUserDataByUserId(
                Request.CleanUserDataByUserIdRequest request,
                UnityAction<AsyncResult<Result.CleanUserDataByUserIdResult>> callback
        )
		{
			var task = new CleanUserDataByUserIdTask(
			    Gs2WebSocketSession,
			    request
            );
            yield return task;
            callback.Invoke(new AsyncResult<Result.CleanUserDataByUserIdResult>(task.Result, task.Error));
        }

		public IFuture<Result.CleanUserDataByUserIdResult> CleanUserDataByUserIdFuture(
                Request.CleanUserDataByUserIdRequest request
        )
		{
			return new CleanUserDataByUserIdTask(
			    Gs2WebSocketSession,
			    request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.CleanUserDataByUserIdResult> CleanUserDataByUserIdAsync(
            Request.CleanUserDataByUserIdRequest request
        )
		{
		    var task = new CleanUserDataByUserIdTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
    #else
		public CleanUserDataByUserIdTask CleanUserDataByUserIdAsync(
                Request.CleanUserDataByUserIdRequest request
        )
		{
			return new CleanUserDataByUserIdTask(
                Gs2WebSocketSession,
			    request
            );
        }
    #endif
#else
		public async Task<Result.CleanUserDataByUserIdResult> CleanUserDataByUserIdAsync(
            Request.CleanUserDataByUserIdRequest request
        )
		{
		    var task = new CleanUserDataByUserIdTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
#endif


        public class CheckCleanUserDataByUserIdTask : Gs2WebSocketSessionTask<Request.CheckCleanUserDataByUserIdRequest, Result.CheckCleanUserDataByUserIdResult>
        {
	        public CheckCleanUserDataByUserIdTask(IGs2Session session, Request.CheckCleanUserDataByUserIdRequest request) : base(session, request)
	        {
	        }

            protected override IGs2SessionRequest CreateRequest(Request.CheckCleanUserDataByUserIdRequest request)
            {
                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);

                jsonWriter.WriteObjectStart();

                if (request.UserId != null)
                {
                    jsonWriter.WritePropertyName("userId");
                    jsonWriter.Write(request.UserId.ToString());
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
                if (request.DryRun)
                {
                    jsonWriter.WritePropertyName("xGs2DryRun");
                    jsonWriter.Write("true");
                }

                AddHeader(
                    Session.Credential,
                    "inventory",
                    "namespace",
                    "checkCleanUserDataByUserId",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator CheckCleanUserDataByUserId(
                Request.CheckCleanUserDataByUserIdRequest request,
                UnityAction<AsyncResult<Result.CheckCleanUserDataByUserIdResult>> callback
        )
		{
			var task = new CheckCleanUserDataByUserIdTask(
			    Gs2WebSocketSession,
			    request
            );
            yield return task;
            callback.Invoke(new AsyncResult<Result.CheckCleanUserDataByUserIdResult>(task.Result, task.Error));
        }

		public IFuture<Result.CheckCleanUserDataByUserIdResult> CheckCleanUserDataByUserIdFuture(
                Request.CheckCleanUserDataByUserIdRequest request
        )
		{
			return new CheckCleanUserDataByUserIdTask(
			    Gs2WebSocketSession,
			    request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.CheckCleanUserDataByUserIdResult> CheckCleanUserDataByUserIdAsync(
            Request.CheckCleanUserDataByUserIdRequest request
        )
		{
		    var task = new CheckCleanUserDataByUserIdTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
    #else
		public CheckCleanUserDataByUserIdTask CheckCleanUserDataByUserIdAsync(
                Request.CheckCleanUserDataByUserIdRequest request
        )
		{
			return new CheckCleanUserDataByUserIdTask(
                Gs2WebSocketSession,
			    request
            );
        }
    #endif
#else
		public async Task<Result.CheckCleanUserDataByUserIdResult> CheckCleanUserDataByUserIdAsync(
            Request.CheckCleanUserDataByUserIdRequest request
        )
		{
		    var task = new CheckCleanUserDataByUserIdTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
#endif


        public class ImportUserDataByUserIdTask : Gs2WebSocketSessionTask<Request.ImportUserDataByUserIdRequest, Result.ImportUserDataByUserIdResult>
        {
	        public ImportUserDataByUserIdTask(IGs2Session session, Request.ImportUserDataByUserIdRequest request) : base(session, request)
	        {
	        }

            protected override IGs2SessionRequest CreateRequest(Request.ImportUserDataByUserIdRequest request)
            {
                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);

                jsonWriter.WriteObjectStart();

                if (request.UserId != null)
                {
                    jsonWriter.WritePropertyName("userId");
                    jsonWriter.Write(request.UserId.ToString());
                }
                if (request.UploadToken != null)
                {
                    jsonWriter.WritePropertyName("uploadToken");
                    jsonWriter.Write(request.UploadToken.ToString());
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
                if (request.DryRun)
                {
                    jsonWriter.WritePropertyName("xGs2DryRun");
                    jsonWriter.Write("true");
                }

                AddHeader(
                    Session.Credential,
                    "inventory",
                    "namespace",
                    "importUserDataByUserId",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator ImportUserDataByUserId(
                Request.ImportUserDataByUserIdRequest request,
                UnityAction<AsyncResult<Result.ImportUserDataByUserIdResult>> callback
        )
		{
			var task = new ImportUserDataByUserIdTask(
			    Gs2WebSocketSession,
			    request
            );
            yield return task;
            callback.Invoke(new AsyncResult<Result.ImportUserDataByUserIdResult>(task.Result, task.Error));
        }

		public IFuture<Result.ImportUserDataByUserIdResult> ImportUserDataByUserIdFuture(
                Request.ImportUserDataByUserIdRequest request
        )
		{
			return new ImportUserDataByUserIdTask(
			    Gs2WebSocketSession,
			    request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.ImportUserDataByUserIdResult> ImportUserDataByUserIdAsync(
            Request.ImportUserDataByUserIdRequest request
        )
		{
		    var task = new ImportUserDataByUserIdTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
    #else
		public ImportUserDataByUserIdTask ImportUserDataByUserIdAsync(
                Request.ImportUserDataByUserIdRequest request
        )
		{
			return new ImportUserDataByUserIdTask(
                Gs2WebSocketSession,
			    request
            );
        }
    #endif
#else
		public async Task<Result.ImportUserDataByUserIdResult> ImportUserDataByUserIdAsync(
            Request.ImportUserDataByUserIdRequest request
        )
		{
		    var task = new ImportUserDataByUserIdTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
#endif


        public class CheckImportUserDataByUserIdTask : Gs2WebSocketSessionTask<Request.CheckImportUserDataByUserIdRequest, Result.CheckImportUserDataByUserIdResult>
        {
	        public CheckImportUserDataByUserIdTask(IGs2Session session, Request.CheckImportUserDataByUserIdRequest request) : base(session, request)
	        {
	        }

            protected override IGs2SessionRequest CreateRequest(Request.CheckImportUserDataByUserIdRequest request)
            {
                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);

                jsonWriter.WriteObjectStart();

                if (request.UserId != null)
                {
                    jsonWriter.WritePropertyName("userId");
                    jsonWriter.Write(request.UserId.ToString());
                }
                if (request.UploadToken != null)
                {
                    jsonWriter.WritePropertyName("uploadToken");
                    jsonWriter.Write(request.UploadToken.ToString());
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
                if (request.DryRun)
                {
                    jsonWriter.WritePropertyName("xGs2DryRun");
                    jsonWriter.Write("true");
                }

                AddHeader(
                    Session.Credential,
                    "inventory",
                    "namespace",
                    "checkImportUserDataByUserId",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator CheckImportUserDataByUserId(
                Request.CheckImportUserDataByUserIdRequest request,
                UnityAction<AsyncResult<Result.CheckImportUserDataByUserIdResult>> callback
        )
		{
			var task = new CheckImportUserDataByUserIdTask(
			    Gs2WebSocketSession,
			    request
            );
            yield return task;
            callback.Invoke(new AsyncResult<Result.CheckImportUserDataByUserIdResult>(task.Result, task.Error));
        }

		public IFuture<Result.CheckImportUserDataByUserIdResult> CheckImportUserDataByUserIdFuture(
                Request.CheckImportUserDataByUserIdRequest request
        )
		{
			return new CheckImportUserDataByUserIdTask(
			    Gs2WebSocketSession,
			    request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.CheckImportUserDataByUserIdResult> CheckImportUserDataByUserIdAsync(
            Request.CheckImportUserDataByUserIdRequest request
        )
		{
		    var task = new CheckImportUserDataByUserIdTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
    #else
		public CheckImportUserDataByUserIdTask CheckImportUserDataByUserIdAsync(
                Request.CheckImportUserDataByUserIdRequest request
        )
		{
			return new CheckImportUserDataByUserIdTask(
                Gs2WebSocketSession,
			    request
            );
        }
    #endif
#else
		public async Task<Result.CheckImportUserDataByUserIdResult> CheckImportUserDataByUserIdAsync(
            Request.CheckImportUserDataByUserIdRequest request
        )
		{
		    var task = new CheckImportUserDataByUserIdTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
#endif


        public class CreateInventoryModelMasterTask : Gs2WebSocketSessionTask<Request.CreateInventoryModelMasterRequest, Result.CreateInventoryModelMasterResult>
        {
	        public CreateInventoryModelMasterTask(IGs2Session session, Request.CreateInventoryModelMasterRequest request) : base(session, request)
	        {
	        }

            protected override IGs2SessionRequest CreateRequest(Request.CreateInventoryModelMasterRequest request)
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
                if (request.InitialCapacity != null)
                {
                    jsonWriter.WritePropertyName("initialCapacity");
                    jsonWriter.Write(request.InitialCapacity.ToString());
                }
                if (request.MaxCapacity != null)
                {
                    jsonWriter.WritePropertyName("maxCapacity");
                    jsonWriter.Write(request.MaxCapacity.ToString());
                }
                if (request.ProtectReferencedItem != null)
                {
                    jsonWriter.WritePropertyName("protectReferencedItem");
                    jsonWriter.Write(request.ProtectReferencedItem.ToString());
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
                    "inventory",
                    "inventoryModelMaster",
                    "createInventoryModelMaster",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator CreateInventoryModelMaster(
                Request.CreateInventoryModelMasterRequest request,
                UnityAction<AsyncResult<Result.CreateInventoryModelMasterResult>> callback
        )
		{
			var task = new CreateInventoryModelMasterTask(
			    Gs2WebSocketSession,
			    request
            );
            yield return task;
            callback.Invoke(new AsyncResult<Result.CreateInventoryModelMasterResult>(task.Result, task.Error));
        }

		public IFuture<Result.CreateInventoryModelMasterResult> CreateInventoryModelMasterFuture(
                Request.CreateInventoryModelMasterRequest request
        )
		{
			return new CreateInventoryModelMasterTask(
			    Gs2WebSocketSession,
			    request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.CreateInventoryModelMasterResult> CreateInventoryModelMasterAsync(
            Request.CreateInventoryModelMasterRequest request
        )
		{
		    var task = new CreateInventoryModelMasterTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
    #else
		public CreateInventoryModelMasterTask CreateInventoryModelMasterAsync(
                Request.CreateInventoryModelMasterRequest request
        )
		{
			return new CreateInventoryModelMasterTask(
                Gs2WebSocketSession,
			    request
            );
        }
    #endif
#else
		public async Task<Result.CreateInventoryModelMasterResult> CreateInventoryModelMasterAsync(
            Request.CreateInventoryModelMasterRequest request
        )
		{
		    var task = new CreateInventoryModelMasterTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
#endif


        public class GetInventoryModelMasterTask : Gs2WebSocketSessionTask<Request.GetInventoryModelMasterRequest, Result.GetInventoryModelMasterResult>
        {
	        public GetInventoryModelMasterTask(IGs2Session session, Request.GetInventoryModelMasterRequest request) : base(session, request)
	        {
	        }

            protected override IGs2SessionRequest CreateRequest(Request.GetInventoryModelMasterRequest request)
            {
                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);

                jsonWriter.WriteObjectStart();

                if (request.NamespaceName != null)
                {
                    jsonWriter.WritePropertyName("namespaceName");
                    jsonWriter.Write(request.NamespaceName.ToString());
                }
                if (request.InventoryName != null)
                {
                    jsonWriter.WritePropertyName("inventoryName");
                    jsonWriter.Write(request.InventoryName.ToString());
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
                    "inventory",
                    "inventoryModelMaster",
                    "getInventoryModelMaster",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator GetInventoryModelMaster(
                Request.GetInventoryModelMasterRequest request,
                UnityAction<AsyncResult<Result.GetInventoryModelMasterResult>> callback
        )
		{
			var task = new GetInventoryModelMasterTask(
			    Gs2WebSocketSession,
			    request
            );
            yield return task;
            callback.Invoke(new AsyncResult<Result.GetInventoryModelMasterResult>(task.Result, task.Error));
        }

		public IFuture<Result.GetInventoryModelMasterResult> GetInventoryModelMasterFuture(
                Request.GetInventoryModelMasterRequest request
        )
		{
			return new GetInventoryModelMasterTask(
			    Gs2WebSocketSession,
			    request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.GetInventoryModelMasterResult> GetInventoryModelMasterAsync(
            Request.GetInventoryModelMasterRequest request
        )
		{
		    var task = new GetInventoryModelMasterTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
    #else
		public GetInventoryModelMasterTask GetInventoryModelMasterAsync(
                Request.GetInventoryModelMasterRequest request
        )
		{
			return new GetInventoryModelMasterTask(
                Gs2WebSocketSession,
			    request
            );
        }
    #endif
#else
		public async Task<Result.GetInventoryModelMasterResult> GetInventoryModelMasterAsync(
            Request.GetInventoryModelMasterRequest request
        )
		{
		    var task = new GetInventoryModelMasterTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
#endif


        public class UpdateInventoryModelMasterTask : Gs2WebSocketSessionTask<Request.UpdateInventoryModelMasterRequest, Result.UpdateInventoryModelMasterResult>
        {
	        public UpdateInventoryModelMasterTask(IGs2Session session, Request.UpdateInventoryModelMasterRequest request) : base(session, request)
	        {
	        }

            protected override IGs2SessionRequest CreateRequest(Request.UpdateInventoryModelMasterRequest request)
            {
                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);

                jsonWriter.WriteObjectStart();

                if (request.NamespaceName != null)
                {
                    jsonWriter.WritePropertyName("namespaceName");
                    jsonWriter.Write(request.NamespaceName.ToString());
                }
                if (request.InventoryName != null)
                {
                    jsonWriter.WritePropertyName("inventoryName");
                    jsonWriter.Write(request.InventoryName.ToString());
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
                if (request.InitialCapacity != null)
                {
                    jsonWriter.WritePropertyName("initialCapacity");
                    jsonWriter.Write(request.InitialCapacity.ToString());
                }
                if (request.MaxCapacity != null)
                {
                    jsonWriter.WritePropertyName("maxCapacity");
                    jsonWriter.Write(request.MaxCapacity.ToString());
                }
                if (request.ProtectReferencedItem != null)
                {
                    jsonWriter.WritePropertyName("protectReferencedItem");
                    jsonWriter.Write(request.ProtectReferencedItem.ToString());
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
                    "inventory",
                    "inventoryModelMaster",
                    "updateInventoryModelMaster",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator UpdateInventoryModelMaster(
                Request.UpdateInventoryModelMasterRequest request,
                UnityAction<AsyncResult<Result.UpdateInventoryModelMasterResult>> callback
        )
		{
			var task = new UpdateInventoryModelMasterTask(
			    Gs2WebSocketSession,
			    request
            );
            yield return task;
            callback.Invoke(new AsyncResult<Result.UpdateInventoryModelMasterResult>(task.Result, task.Error));
        }

		public IFuture<Result.UpdateInventoryModelMasterResult> UpdateInventoryModelMasterFuture(
                Request.UpdateInventoryModelMasterRequest request
        )
		{
			return new UpdateInventoryModelMasterTask(
			    Gs2WebSocketSession,
			    request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.UpdateInventoryModelMasterResult> UpdateInventoryModelMasterAsync(
            Request.UpdateInventoryModelMasterRequest request
        )
		{
		    var task = new UpdateInventoryModelMasterTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
    #else
		public UpdateInventoryModelMasterTask UpdateInventoryModelMasterAsync(
                Request.UpdateInventoryModelMasterRequest request
        )
		{
			return new UpdateInventoryModelMasterTask(
                Gs2WebSocketSession,
			    request
            );
        }
    #endif
#else
		public async Task<Result.UpdateInventoryModelMasterResult> UpdateInventoryModelMasterAsync(
            Request.UpdateInventoryModelMasterRequest request
        )
		{
		    var task = new UpdateInventoryModelMasterTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
#endif


        public class DeleteInventoryModelMasterTask : Gs2WebSocketSessionTask<Request.DeleteInventoryModelMasterRequest, Result.DeleteInventoryModelMasterResult>
        {
	        public DeleteInventoryModelMasterTask(IGs2Session session, Request.DeleteInventoryModelMasterRequest request) : base(session, request)
	        {
	        }

            protected override IGs2SessionRequest CreateRequest(Request.DeleteInventoryModelMasterRequest request)
            {
                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);

                jsonWriter.WriteObjectStart();

                if (request.NamespaceName != null)
                {
                    jsonWriter.WritePropertyName("namespaceName");
                    jsonWriter.Write(request.NamespaceName.ToString());
                }
                if (request.InventoryName != null)
                {
                    jsonWriter.WritePropertyName("inventoryName");
                    jsonWriter.Write(request.InventoryName.ToString());
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
                    "inventory",
                    "inventoryModelMaster",
                    "deleteInventoryModelMaster",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator DeleteInventoryModelMaster(
                Request.DeleteInventoryModelMasterRequest request,
                UnityAction<AsyncResult<Result.DeleteInventoryModelMasterResult>> callback
        )
		{
			var task = new DeleteInventoryModelMasterTask(
			    Gs2WebSocketSession,
			    request
            );
            yield return task;
            callback.Invoke(new AsyncResult<Result.DeleteInventoryModelMasterResult>(task.Result, task.Error));
        }

		public IFuture<Result.DeleteInventoryModelMasterResult> DeleteInventoryModelMasterFuture(
                Request.DeleteInventoryModelMasterRequest request
        )
		{
			return new DeleteInventoryModelMasterTask(
			    Gs2WebSocketSession,
			    request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.DeleteInventoryModelMasterResult> DeleteInventoryModelMasterAsync(
            Request.DeleteInventoryModelMasterRequest request
        )
		{
		    var task = new DeleteInventoryModelMasterTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
    #else
		public DeleteInventoryModelMasterTask DeleteInventoryModelMasterAsync(
                Request.DeleteInventoryModelMasterRequest request
        )
		{
			return new DeleteInventoryModelMasterTask(
                Gs2WebSocketSession,
			    request
            );
        }
    #endif
#else
		public async Task<Result.DeleteInventoryModelMasterResult> DeleteInventoryModelMasterAsync(
            Request.DeleteInventoryModelMasterRequest request
        )
		{
		    var task = new DeleteInventoryModelMasterTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
#endif


        public class CreateItemModelMasterTask : Gs2WebSocketSessionTask<Request.CreateItemModelMasterRequest, Result.CreateItemModelMasterResult>
        {
	        public CreateItemModelMasterTask(IGs2Session session, Request.CreateItemModelMasterRequest request) : base(session, request)
	        {
	        }

            protected override IGs2SessionRequest CreateRequest(Request.CreateItemModelMasterRequest request)
            {
                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);

                jsonWriter.WriteObjectStart();

                if (request.NamespaceName != null)
                {
                    jsonWriter.WritePropertyName("namespaceName");
                    jsonWriter.Write(request.NamespaceName.ToString());
                }
                if (request.InventoryName != null)
                {
                    jsonWriter.WritePropertyName("inventoryName");
                    jsonWriter.Write(request.InventoryName.ToString());
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
                if (request.StackingLimit != null)
                {
                    jsonWriter.WritePropertyName("stackingLimit");
                    jsonWriter.Write(request.StackingLimit.ToString());
                }
                if (request.AllowMultipleStacks != null)
                {
                    jsonWriter.WritePropertyName("allowMultipleStacks");
                    jsonWriter.Write(request.AllowMultipleStacks.ToString());
                }
                if (request.SortValue != null)
                {
                    jsonWriter.WritePropertyName("sortValue");
                    jsonWriter.Write(request.SortValue.ToString());
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
                    "inventory",
                    "itemModelMaster",
                    "createItemModelMaster",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator CreateItemModelMaster(
                Request.CreateItemModelMasterRequest request,
                UnityAction<AsyncResult<Result.CreateItemModelMasterResult>> callback
        )
		{
			var task = new CreateItemModelMasterTask(
			    Gs2WebSocketSession,
			    request
            );
            yield return task;
            callback.Invoke(new AsyncResult<Result.CreateItemModelMasterResult>(task.Result, task.Error));
        }

		public IFuture<Result.CreateItemModelMasterResult> CreateItemModelMasterFuture(
                Request.CreateItemModelMasterRequest request
        )
		{
			return new CreateItemModelMasterTask(
			    Gs2WebSocketSession,
			    request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.CreateItemModelMasterResult> CreateItemModelMasterAsync(
            Request.CreateItemModelMasterRequest request
        )
		{
		    var task = new CreateItemModelMasterTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
    #else
		public CreateItemModelMasterTask CreateItemModelMasterAsync(
                Request.CreateItemModelMasterRequest request
        )
		{
			return new CreateItemModelMasterTask(
                Gs2WebSocketSession,
			    request
            );
        }
    #endif
#else
		public async Task<Result.CreateItemModelMasterResult> CreateItemModelMasterAsync(
            Request.CreateItemModelMasterRequest request
        )
		{
		    var task = new CreateItemModelMasterTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
#endif


        public class GetItemModelMasterTask : Gs2WebSocketSessionTask<Request.GetItemModelMasterRequest, Result.GetItemModelMasterResult>
        {
	        public GetItemModelMasterTask(IGs2Session session, Request.GetItemModelMasterRequest request) : base(session, request)
	        {
	        }

            protected override IGs2SessionRequest CreateRequest(Request.GetItemModelMasterRequest request)
            {
                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);

                jsonWriter.WriteObjectStart();

                if (request.NamespaceName != null)
                {
                    jsonWriter.WritePropertyName("namespaceName");
                    jsonWriter.Write(request.NamespaceName.ToString());
                }
                if (request.InventoryName != null)
                {
                    jsonWriter.WritePropertyName("inventoryName");
                    jsonWriter.Write(request.InventoryName.ToString());
                }
                if (request.ItemName != null)
                {
                    jsonWriter.WritePropertyName("itemName");
                    jsonWriter.Write(request.ItemName.ToString());
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
                    "inventory",
                    "itemModelMaster",
                    "getItemModelMaster",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator GetItemModelMaster(
                Request.GetItemModelMasterRequest request,
                UnityAction<AsyncResult<Result.GetItemModelMasterResult>> callback
        )
		{
			var task = new GetItemModelMasterTask(
			    Gs2WebSocketSession,
			    request
            );
            yield return task;
            callback.Invoke(new AsyncResult<Result.GetItemModelMasterResult>(task.Result, task.Error));
        }

		public IFuture<Result.GetItemModelMasterResult> GetItemModelMasterFuture(
                Request.GetItemModelMasterRequest request
        )
		{
			return new GetItemModelMasterTask(
			    Gs2WebSocketSession,
			    request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.GetItemModelMasterResult> GetItemModelMasterAsync(
            Request.GetItemModelMasterRequest request
        )
		{
		    var task = new GetItemModelMasterTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
    #else
		public GetItemModelMasterTask GetItemModelMasterAsync(
                Request.GetItemModelMasterRequest request
        )
		{
			return new GetItemModelMasterTask(
                Gs2WebSocketSession,
			    request
            );
        }
    #endif
#else
		public async Task<Result.GetItemModelMasterResult> GetItemModelMasterAsync(
            Request.GetItemModelMasterRequest request
        )
		{
		    var task = new GetItemModelMasterTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
#endif


        public class UpdateItemModelMasterTask : Gs2WebSocketSessionTask<Request.UpdateItemModelMasterRequest, Result.UpdateItemModelMasterResult>
        {
	        public UpdateItemModelMasterTask(IGs2Session session, Request.UpdateItemModelMasterRequest request) : base(session, request)
	        {
	        }

            protected override IGs2SessionRequest CreateRequest(Request.UpdateItemModelMasterRequest request)
            {
                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);

                jsonWriter.WriteObjectStart();

                if (request.NamespaceName != null)
                {
                    jsonWriter.WritePropertyName("namespaceName");
                    jsonWriter.Write(request.NamespaceName.ToString());
                }
                if (request.InventoryName != null)
                {
                    jsonWriter.WritePropertyName("inventoryName");
                    jsonWriter.Write(request.InventoryName.ToString());
                }
                if (request.ItemName != null)
                {
                    jsonWriter.WritePropertyName("itemName");
                    jsonWriter.Write(request.ItemName.ToString());
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
                if (request.StackingLimit != null)
                {
                    jsonWriter.WritePropertyName("stackingLimit");
                    jsonWriter.Write(request.StackingLimit.ToString());
                }
                if (request.AllowMultipleStacks != null)
                {
                    jsonWriter.WritePropertyName("allowMultipleStacks");
                    jsonWriter.Write(request.AllowMultipleStacks.ToString());
                }
                if (request.SortValue != null)
                {
                    jsonWriter.WritePropertyName("sortValue");
                    jsonWriter.Write(request.SortValue.ToString());
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
                    "inventory",
                    "itemModelMaster",
                    "updateItemModelMaster",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator UpdateItemModelMaster(
                Request.UpdateItemModelMasterRequest request,
                UnityAction<AsyncResult<Result.UpdateItemModelMasterResult>> callback
        )
		{
			var task = new UpdateItemModelMasterTask(
			    Gs2WebSocketSession,
			    request
            );
            yield return task;
            callback.Invoke(new AsyncResult<Result.UpdateItemModelMasterResult>(task.Result, task.Error));
        }

		public IFuture<Result.UpdateItemModelMasterResult> UpdateItemModelMasterFuture(
                Request.UpdateItemModelMasterRequest request
        )
		{
			return new UpdateItemModelMasterTask(
			    Gs2WebSocketSession,
			    request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.UpdateItemModelMasterResult> UpdateItemModelMasterAsync(
            Request.UpdateItemModelMasterRequest request
        )
		{
		    var task = new UpdateItemModelMasterTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
    #else
		public UpdateItemModelMasterTask UpdateItemModelMasterAsync(
                Request.UpdateItemModelMasterRequest request
        )
		{
			return new UpdateItemModelMasterTask(
                Gs2WebSocketSession,
			    request
            );
        }
    #endif
#else
		public async Task<Result.UpdateItemModelMasterResult> UpdateItemModelMasterAsync(
            Request.UpdateItemModelMasterRequest request
        )
		{
		    var task = new UpdateItemModelMasterTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
#endif


        public class DeleteItemModelMasterTask : Gs2WebSocketSessionTask<Request.DeleteItemModelMasterRequest, Result.DeleteItemModelMasterResult>
        {
	        public DeleteItemModelMasterTask(IGs2Session session, Request.DeleteItemModelMasterRequest request) : base(session, request)
	        {
	        }

            protected override IGs2SessionRequest CreateRequest(Request.DeleteItemModelMasterRequest request)
            {
                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);

                jsonWriter.WriteObjectStart();

                if (request.NamespaceName != null)
                {
                    jsonWriter.WritePropertyName("namespaceName");
                    jsonWriter.Write(request.NamespaceName.ToString());
                }
                if (request.InventoryName != null)
                {
                    jsonWriter.WritePropertyName("inventoryName");
                    jsonWriter.Write(request.InventoryName.ToString());
                }
                if (request.ItemName != null)
                {
                    jsonWriter.WritePropertyName("itemName");
                    jsonWriter.Write(request.ItemName.ToString());
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
                    "inventory",
                    "itemModelMaster",
                    "deleteItemModelMaster",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator DeleteItemModelMaster(
                Request.DeleteItemModelMasterRequest request,
                UnityAction<AsyncResult<Result.DeleteItemModelMasterResult>> callback
        )
		{
			var task = new DeleteItemModelMasterTask(
			    Gs2WebSocketSession,
			    request
            );
            yield return task;
            callback.Invoke(new AsyncResult<Result.DeleteItemModelMasterResult>(task.Result, task.Error));
        }

		public IFuture<Result.DeleteItemModelMasterResult> DeleteItemModelMasterFuture(
                Request.DeleteItemModelMasterRequest request
        )
		{
			return new DeleteItemModelMasterTask(
			    Gs2WebSocketSession,
			    request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.DeleteItemModelMasterResult> DeleteItemModelMasterAsync(
            Request.DeleteItemModelMasterRequest request
        )
		{
		    var task = new DeleteItemModelMasterTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
    #else
		public DeleteItemModelMasterTask DeleteItemModelMasterAsync(
                Request.DeleteItemModelMasterRequest request
        )
		{
			return new DeleteItemModelMasterTask(
                Gs2WebSocketSession,
			    request
            );
        }
    #endif
#else
		public async Task<Result.DeleteItemModelMasterResult> DeleteItemModelMasterAsync(
            Request.DeleteItemModelMasterRequest request
        )
		{
		    var task = new DeleteItemModelMasterTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
#endif


        public class GetItemModelTask : Gs2WebSocketSessionTask<Request.GetItemModelRequest, Result.GetItemModelResult>
        {
	        public GetItemModelTask(IGs2Session session, Request.GetItemModelRequest request) : base(session, request)
	        {
	        }

            protected override IGs2SessionRequest CreateRequest(Request.GetItemModelRequest request)
            {
                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);

                jsonWriter.WriteObjectStart();

                if (request.NamespaceName != null)
                {
                    jsonWriter.WritePropertyName("namespaceName");
                    jsonWriter.Write(request.NamespaceName.ToString());
                }
                if (request.InventoryName != null)
                {
                    jsonWriter.WritePropertyName("inventoryName");
                    jsonWriter.Write(request.InventoryName.ToString());
                }
                if (request.ItemName != null)
                {
                    jsonWriter.WritePropertyName("itemName");
                    jsonWriter.Write(request.ItemName.ToString());
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
                    "inventory",
                    "itemModel",
                    "getItemModel",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator GetItemModel(
                Request.GetItemModelRequest request,
                UnityAction<AsyncResult<Result.GetItemModelResult>> callback
        )
		{
			var task = new GetItemModelTask(
			    Gs2WebSocketSession,
			    request
            );
            yield return task;
            callback.Invoke(new AsyncResult<Result.GetItemModelResult>(task.Result, task.Error));
        }

		public IFuture<Result.GetItemModelResult> GetItemModelFuture(
                Request.GetItemModelRequest request
        )
		{
			return new GetItemModelTask(
			    Gs2WebSocketSession,
			    request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.GetItemModelResult> GetItemModelAsync(
            Request.GetItemModelRequest request
        )
		{
		    var task = new GetItemModelTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
    #else
		public GetItemModelTask GetItemModelAsync(
                Request.GetItemModelRequest request
        )
		{
			return new GetItemModelTask(
                Gs2WebSocketSession,
			    request
            );
        }
    #endif
#else
		public async Task<Result.GetItemModelResult> GetItemModelAsync(
            Request.GetItemModelRequest request
        )
		{
		    var task = new GetItemModelTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
#endif


        public class CreateSimpleInventoryModelMasterTask : Gs2WebSocketSessionTask<Request.CreateSimpleInventoryModelMasterRequest, Result.CreateSimpleInventoryModelMasterResult>
        {
	        public CreateSimpleInventoryModelMasterTask(IGs2Session session, Request.CreateSimpleInventoryModelMasterRequest request) : base(session, request)
	        {
	        }

            protected override IGs2SessionRequest CreateRequest(Request.CreateSimpleInventoryModelMasterRequest request)
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
                    "inventory",
                    "simpleInventoryModelMaster",
                    "createSimpleInventoryModelMaster",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator CreateSimpleInventoryModelMaster(
                Request.CreateSimpleInventoryModelMasterRequest request,
                UnityAction<AsyncResult<Result.CreateSimpleInventoryModelMasterResult>> callback
        )
		{
			var task = new CreateSimpleInventoryModelMasterTask(
			    Gs2WebSocketSession,
			    request
            );
            yield return task;
            callback.Invoke(new AsyncResult<Result.CreateSimpleInventoryModelMasterResult>(task.Result, task.Error));
        }

		public IFuture<Result.CreateSimpleInventoryModelMasterResult> CreateSimpleInventoryModelMasterFuture(
                Request.CreateSimpleInventoryModelMasterRequest request
        )
		{
			return new CreateSimpleInventoryModelMasterTask(
			    Gs2WebSocketSession,
			    request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.CreateSimpleInventoryModelMasterResult> CreateSimpleInventoryModelMasterAsync(
            Request.CreateSimpleInventoryModelMasterRequest request
        )
		{
		    var task = new CreateSimpleInventoryModelMasterTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
    #else
		public CreateSimpleInventoryModelMasterTask CreateSimpleInventoryModelMasterAsync(
                Request.CreateSimpleInventoryModelMasterRequest request
        )
		{
			return new CreateSimpleInventoryModelMasterTask(
                Gs2WebSocketSession,
			    request
            );
        }
    #endif
#else
		public async Task<Result.CreateSimpleInventoryModelMasterResult> CreateSimpleInventoryModelMasterAsync(
            Request.CreateSimpleInventoryModelMasterRequest request
        )
		{
		    var task = new CreateSimpleInventoryModelMasterTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
#endif


        public class GetSimpleInventoryModelMasterTask : Gs2WebSocketSessionTask<Request.GetSimpleInventoryModelMasterRequest, Result.GetSimpleInventoryModelMasterResult>
        {
	        public GetSimpleInventoryModelMasterTask(IGs2Session session, Request.GetSimpleInventoryModelMasterRequest request) : base(session, request)
	        {
	        }

            protected override IGs2SessionRequest CreateRequest(Request.GetSimpleInventoryModelMasterRequest request)
            {
                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);

                jsonWriter.WriteObjectStart();

                if (request.NamespaceName != null)
                {
                    jsonWriter.WritePropertyName("namespaceName");
                    jsonWriter.Write(request.NamespaceName.ToString());
                }
                if (request.InventoryName != null)
                {
                    jsonWriter.WritePropertyName("inventoryName");
                    jsonWriter.Write(request.InventoryName.ToString());
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
                    "inventory",
                    "simpleInventoryModelMaster",
                    "getSimpleInventoryModelMaster",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator GetSimpleInventoryModelMaster(
                Request.GetSimpleInventoryModelMasterRequest request,
                UnityAction<AsyncResult<Result.GetSimpleInventoryModelMasterResult>> callback
        )
		{
			var task = new GetSimpleInventoryModelMasterTask(
			    Gs2WebSocketSession,
			    request
            );
            yield return task;
            callback.Invoke(new AsyncResult<Result.GetSimpleInventoryModelMasterResult>(task.Result, task.Error));
        }

		public IFuture<Result.GetSimpleInventoryModelMasterResult> GetSimpleInventoryModelMasterFuture(
                Request.GetSimpleInventoryModelMasterRequest request
        )
		{
			return new GetSimpleInventoryModelMasterTask(
			    Gs2WebSocketSession,
			    request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.GetSimpleInventoryModelMasterResult> GetSimpleInventoryModelMasterAsync(
            Request.GetSimpleInventoryModelMasterRequest request
        )
		{
		    var task = new GetSimpleInventoryModelMasterTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
    #else
		public GetSimpleInventoryModelMasterTask GetSimpleInventoryModelMasterAsync(
                Request.GetSimpleInventoryModelMasterRequest request
        )
		{
			return new GetSimpleInventoryModelMasterTask(
                Gs2WebSocketSession,
			    request
            );
        }
    #endif
#else
		public async Task<Result.GetSimpleInventoryModelMasterResult> GetSimpleInventoryModelMasterAsync(
            Request.GetSimpleInventoryModelMasterRequest request
        )
		{
		    var task = new GetSimpleInventoryModelMasterTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
#endif


        public class UpdateSimpleInventoryModelMasterTask : Gs2WebSocketSessionTask<Request.UpdateSimpleInventoryModelMasterRequest, Result.UpdateSimpleInventoryModelMasterResult>
        {
	        public UpdateSimpleInventoryModelMasterTask(IGs2Session session, Request.UpdateSimpleInventoryModelMasterRequest request) : base(session, request)
	        {
	        }

            protected override IGs2SessionRequest CreateRequest(Request.UpdateSimpleInventoryModelMasterRequest request)
            {
                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);

                jsonWriter.WriteObjectStart();

                if (request.NamespaceName != null)
                {
                    jsonWriter.WritePropertyName("namespaceName");
                    jsonWriter.Write(request.NamespaceName.ToString());
                }
                if (request.InventoryName != null)
                {
                    jsonWriter.WritePropertyName("inventoryName");
                    jsonWriter.Write(request.InventoryName.ToString());
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
                    "inventory",
                    "simpleInventoryModelMaster",
                    "updateSimpleInventoryModelMaster",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator UpdateSimpleInventoryModelMaster(
                Request.UpdateSimpleInventoryModelMasterRequest request,
                UnityAction<AsyncResult<Result.UpdateSimpleInventoryModelMasterResult>> callback
        )
		{
			var task = new UpdateSimpleInventoryModelMasterTask(
			    Gs2WebSocketSession,
			    request
            );
            yield return task;
            callback.Invoke(new AsyncResult<Result.UpdateSimpleInventoryModelMasterResult>(task.Result, task.Error));
        }

		public IFuture<Result.UpdateSimpleInventoryModelMasterResult> UpdateSimpleInventoryModelMasterFuture(
                Request.UpdateSimpleInventoryModelMasterRequest request
        )
		{
			return new UpdateSimpleInventoryModelMasterTask(
			    Gs2WebSocketSession,
			    request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.UpdateSimpleInventoryModelMasterResult> UpdateSimpleInventoryModelMasterAsync(
            Request.UpdateSimpleInventoryModelMasterRequest request
        )
		{
		    var task = new UpdateSimpleInventoryModelMasterTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
    #else
		public UpdateSimpleInventoryModelMasterTask UpdateSimpleInventoryModelMasterAsync(
                Request.UpdateSimpleInventoryModelMasterRequest request
        )
		{
			return new UpdateSimpleInventoryModelMasterTask(
                Gs2WebSocketSession,
			    request
            );
        }
    #endif
#else
		public async Task<Result.UpdateSimpleInventoryModelMasterResult> UpdateSimpleInventoryModelMasterAsync(
            Request.UpdateSimpleInventoryModelMasterRequest request
        )
		{
		    var task = new UpdateSimpleInventoryModelMasterTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
#endif


        public class DeleteSimpleInventoryModelMasterTask : Gs2WebSocketSessionTask<Request.DeleteSimpleInventoryModelMasterRequest, Result.DeleteSimpleInventoryModelMasterResult>
        {
	        public DeleteSimpleInventoryModelMasterTask(IGs2Session session, Request.DeleteSimpleInventoryModelMasterRequest request) : base(session, request)
	        {
	        }

            protected override IGs2SessionRequest CreateRequest(Request.DeleteSimpleInventoryModelMasterRequest request)
            {
                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);

                jsonWriter.WriteObjectStart();

                if (request.NamespaceName != null)
                {
                    jsonWriter.WritePropertyName("namespaceName");
                    jsonWriter.Write(request.NamespaceName.ToString());
                }
                if (request.InventoryName != null)
                {
                    jsonWriter.WritePropertyName("inventoryName");
                    jsonWriter.Write(request.InventoryName.ToString());
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
                    "inventory",
                    "simpleInventoryModelMaster",
                    "deleteSimpleInventoryModelMaster",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator DeleteSimpleInventoryModelMaster(
                Request.DeleteSimpleInventoryModelMasterRequest request,
                UnityAction<AsyncResult<Result.DeleteSimpleInventoryModelMasterResult>> callback
        )
		{
			var task = new DeleteSimpleInventoryModelMasterTask(
			    Gs2WebSocketSession,
			    request
            );
            yield return task;
            callback.Invoke(new AsyncResult<Result.DeleteSimpleInventoryModelMasterResult>(task.Result, task.Error));
        }

		public IFuture<Result.DeleteSimpleInventoryModelMasterResult> DeleteSimpleInventoryModelMasterFuture(
                Request.DeleteSimpleInventoryModelMasterRequest request
        )
		{
			return new DeleteSimpleInventoryModelMasterTask(
			    Gs2WebSocketSession,
			    request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.DeleteSimpleInventoryModelMasterResult> DeleteSimpleInventoryModelMasterAsync(
            Request.DeleteSimpleInventoryModelMasterRequest request
        )
		{
		    var task = new DeleteSimpleInventoryModelMasterTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
    #else
		public DeleteSimpleInventoryModelMasterTask DeleteSimpleInventoryModelMasterAsync(
                Request.DeleteSimpleInventoryModelMasterRequest request
        )
		{
			return new DeleteSimpleInventoryModelMasterTask(
                Gs2WebSocketSession,
			    request
            );
        }
    #endif
#else
		public async Task<Result.DeleteSimpleInventoryModelMasterResult> DeleteSimpleInventoryModelMasterAsync(
            Request.DeleteSimpleInventoryModelMasterRequest request
        )
		{
		    var task = new DeleteSimpleInventoryModelMasterTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
#endif


        public class CreateSimpleItemModelMasterTask : Gs2WebSocketSessionTask<Request.CreateSimpleItemModelMasterRequest, Result.CreateSimpleItemModelMasterResult>
        {
	        public CreateSimpleItemModelMasterTask(IGs2Session session, Request.CreateSimpleItemModelMasterRequest request) : base(session, request)
	        {
	        }

            protected override IGs2SessionRequest CreateRequest(Request.CreateSimpleItemModelMasterRequest request)
            {
                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);

                jsonWriter.WriteObjectStart();

                if (request.NamespaceName != null)
                {
                    jsonWriter.WritePropertyName("namespaceName");
                    jsonWriter.Write(request.NamespaceName.ToString());
                }
                if (request.InventoryName != null)
                {
                    jsonWriter.WritePropertyName("inventoryName");
                    jsonWriter.Write(request.InventoryName.ToString());
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
                    "inventory",
                    "simpleItemModelMaster",
                    "createSimpleItemModelMaster",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator CreateSimpleItemModelMaster(
                Request.CreateSimpleItemModelMasterRequest request,
                UnityAction<AsyncResult<Result.CreateSimpleItemModelMasterResult>> callback
        )
		{
			var task = new CreateSimpleItemModelMasterTask(
			    Gs2WebSocketSession,
			    request
            );
            yield return task;
            callback.Invoke(new AsyncResult<Result.CreateSimpleItemModelMasterResult>(task.Result, task.Error));
        }

		public IFuture<Result.CreateSimpleItemModelMasterResult> CreateSimpleItemModelMasterFuture(
                Request.CreateSimpleItemModelMasterRequest request
        )
		{
			return new CreateSimpleItemModelMasterTask(
			    Gs2WebSocketSession,
			    request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.CreateSimpleItemModelMasterResult> CreateSimpleItemModelMasterAsync(
            Request.CreateSimpleItemModelMasterRequest request
        )
		{
		    var task = new CreateSimpleItemModelMasterTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
    #else
		public CreateSimpleItemModelMasterTask CreateSimpleItemModelMasterAsync(
                Request.CreateSimpleItemModelMasterRequest request
        )
		{
			return new CreateSimpleItemModelMasterTask(
                Gs2WebSocketSession,
			    request
            );
        }
    #endif
#else
		public async Task<Result.CreateSimpleItemModelMasterResult> CreateSimpleItemModelMasterAsync(
            Request.CreateSimpleItemModelMasterRequest request
        )
		{
		    var task = new CreateSimpleItemModelMasterTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
#endif


        public class GetSimpleItemModelMasterTask : Gs2WebSocketSessionTask<Request.GetSimpleItemModelMasterRequest, Result.GetSimpleItemModelMasterResult>
        {
	        public GetSimpleItemModelMasterTask(IGs2Session session, Request.GetSimpleItemModelMasterRequest request) : base(session, request)
	        {
	        }

            protected override IGs2SessionRequest CreateRequest(Request.GetSimpleItemModelMasterRequest request)
            {
                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);

                jsonWriter.WriteObjectStart();

                if (request.NamespaceName != null)
                {
                    jsonWriter.WritePropertyName("namespaceName");
                    jsonWriter.Write(request.NamespaceName.ToString());
                }
                if (request.InventoryName != null)
                {
                    jsonWriter.WritePropertyName("inventoryName");
                    jsonWriter.Write(request.InventoryName.ToString());
                }
                if (request.ItemName != null)
                {
                    jsonWriter.WritePropertyName("itemName");
                    jsonWriter.Write(request.ItemName.ToString());
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
                    "inventory",
                    "simpleItemModelMaster",
                    "getSimpleItemModelMaster",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator GetSimpleItemModelMaster(
                Request.GetSimpleItemModelMasterRequest request,
                UnityAction<AsyncResult<Result.GetSimpleItemModelMasterResult>> callback
        )
		{
			var task = new GetSimpleItemModelMasterTask(
			    Gs2WebSocketSession,
			    request
            );
            yield return task;
            callback.Invoke(new AsyncResult<Result.GetSimpleItemModelMasterResult>(task.Result, task.Error));
        }

		public IFuture<Result.GetSimpleItemModelMasterResult> GetSimpleItemModelMasterFuture(
                Request.GetSimpleItemModelMasterRequest request
        )
		{
			return new GetSimpleItemModelMasterTask(
			    Gs2WebSocketSession,
			    request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.GetSimpleItemModelMasterResult> GetSimpleItemModelMasterAsync(
            Request.GetSimpleItemModelMasterRequest request
        )
		{
		    var task = new GetSimpleItemModelMasterTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
    #else
		public GetSimpleItemModelMasterTask GetSimpleItemModelMasterAsync(
                Request.GetSimpleItemModelMasterRequest request
        )
		{
			return new GetSimpleItemModelMasterTask(
                Gs2WebSocketSession,
			    request
            );
        }
    #endif
#else
		public async Task<Result.GetSimpleItemModelMasterResult> GetSimpleItemModelMasterAsync(
            Request.GetSimpleItemModelMasterRequest request
        )
		{
		    var task = new GetSimpleItemModelMasterTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
#endif


        public class UpdateSimpleItemModelMasterTask : Gs2WebSocketSessionTask<Request.UpdateSimpleItemModelMasterRequest, Result.UpdateSimpleItemModelMasterResult>
        {
	        public UpdateSimpleItemModelMasterTask(IGs2Session session, Request.UpdateSimpleItemModelMasterRequest request) : base(session, request)
	        {
	        }

            protected override IGs2SessionRequest CreateRequest(Request.UpdateSimpleItemModelMasterRequest request)
            {
                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);

                jsonWriter.WriteObjectStart();

                if (request.NamespaceName != null)
                {
                    jsonWriter.WritePropertyName("namespaceName");
                    jsonWriter.Write(request.NamespaceName.ToString());
                }
                if (request.InventoryName != null)
                {
                    jsonWriter.WritePropertyName("inventoryName");
                    jsonWriter.Write(request.InventoryName.ToString());
                }
                if (request.ItemName != null)
                {
                    jsonWriter.WritePropertyName("itemName");
                    jsonWriter.Write(request.ItemName.ToString());
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
                    "inventory",
                    "simpleItemModelMaster",
                    "updateSimpleItemModelMaster",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator UpdateSimpleItemModelMaster(
                Request.UpdateSimpleItemModelMasterRequest request,
                UnityAction<AsyncResult<Result.UpdateSimpleItemModelMasterResult>> callback
        )
		{
			var task = new UpdateSimpleItemModelMasterTask(
			    Gs2WebSocketSession,
			    request
            );
            yield return task;
            callback.Invoke(new AsyncResult<Result.UpdateSimpleItemModelMasterResult>(task.Result, task.Error));
        }

		public IFuture<Result.UpdateSimpleItemModelMasterResult> UpdateSimpleItemModelMasterFuture(
                Request.UpdateSimpleItemModelMasterRequest request
        )
		{
			return new UpdateSimpleItemModelMasterTask(
			    Gs2WebSocketSession,
			    request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.UpdateSimpleItemModelMasterResult> UpdateSimpleItemModelMasterAsync(
            Request.UpdateSimpleItemModelMasterRequest request
        )
		{
		    var task = new UpdateSimpleItemModelMasterTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
    #else
		public UpdateSimpleItemModelMasterTask UpdateSimpleItemModelMasterAsync(
                Request.UpdateSimpleItemModelMasterRequest request
        )
		{
			return new UpdateSimpleItemModelMasterTask(
                Gs2WebSocketSession,
			    request
            );
        }
    #endif
#else
		public async Task<Result.UpdateSimpleItemModelMasterResult> UpdateSimpleItemModelMasterAsync(
            Request.UpdateSimpleItemModelMasterRequest request
        )
		{
		    var task = new UpdateSimpleItemModelMasterTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
#endif


        public class DeleteSimpleItemModelMasterTask : Gs2WebSocketSessionTask<Request.DeleteSimpleItemModelMasterRequest, Result.DeleteSimpleItemModelMasterResult>
        {
	        public DeleteSimpleItemModelMasterTask(IGs2Session session, Request.DeleteSimpleItemModelMasterRequest request) : base(session, request)
	        {
	        }

            protected override IGs2SessionRequest CreateRequest(Request.DeleteSimpleItemModelMasterRequest request)
            {
                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);

                jsonWriter.WriteObjectStart();

                if (request.NamespaceName != null)
                {
                    jsonWriter.WritePropertyName("namespaceName");
                    jsonWriter.Write(request.NamespaceName.ToString());
                }
                if (request.InventoryName != null)
                {
                    jsonWriter.WritePropertyName("inventoryName");
                    jsonWriter.Write(request.InventoryName.ToString());
                }
                if (request.ItemName != null)
                {
                    jsonWriter.WritePropertyName("itemName");
                    jsonWriter.Write(request.ItemName.ToString());
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
                    "inventory",
                    "simpleItemModelMaster",
                    "deleteSimpleItemModelMaster",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator DeleteSimpleItemModelMaster(
                Request.DeleteSimpleItemModelMasterRequest request,
                UnityAction<AsyncResult<Result.DeleteSimpleItemModelMasterResult>> callback
        )
		{
			var task = new DeleteSimpleItemModelMasterTask(
			    Gs2WebSocketSession,
			    request
            );
            yield return task;
            callback.Invoke(new AsyncResult<Result.DeleteSimpleItemModelMasterResult>(task.Result, task.Error));
        }

		public IFuture<Result.DeleteSimpleItemModelMasterResult> DeleteSimpleItemModelMasterFuture(
                Request.DeleteSimpleItemModelMasterRequest request
        )
		{
			return new DeleteSimpleItemModelMasterTask(
			    Gs2WebSocketSession,
			    request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.DeleteSimpleItemModelMasterResult> DeleteSimpleItemModelMasterAsync(
            Request.DeleteSimpleItemModelMasterRequest request
        )
		{
		    var task = new DeleteSimpleItemModelMasterTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
    #else
		public DeleteSimpleItemModelMasterTask DeleteSimpleItemModelMasterAsync(
                Request.DeleteSimpleItemModelMasterRequest request
        )
		{
			return new DeleteSimpleItemModelMasterTask(
                Gs2WebSocketSession,
			    request
            );
        }
    #endif
#else
		public async Task<Result.DeleteSimpleItemModelMasterResult> DeleteSimpleItemModelMasterAsync(
            Request.DeleteSimpleItemModelMasterRequest request
        )
		{
		    var task = new DeleteSimpleItemModelMasterTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
#endif


        public class GetSimpleItemModelTask : Gs2WebSocketSessionTask<Request.GetSimpleItemModelRequest, Result.GetSimpleItemModelResult>
        {
	        public GetSimpleItemModelTask(IGs2Session session, Request.GetSimpleItemModelRequest request) : base(session, request)
	        {
	        }

            protected override IGs2SessionRequest CreateRequest(Request.GetSimpleItemModelRequest request)
            {
                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);

                jsonWriter.WriteObjectStart();

                if (request.NamespaceName != null)
                {
                    jsonWriter.WritePropertyName("namespaceName");
                    jsonWriter.Write(request.NamespaceName.ToString());
                }
                if (request.InventoryName != null)
                {
                    jsonWriter.WritePropertyName("inventoryName");
                    jsonWriter.Write(request.InventoryName.ToString());
                }
                if (request.ItemName != null)
                {
                    jsonWriter.WritePropertyName("itemName");
                    jsonWriter.Write(request.ItemName.ToString());
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
                    "inventory",
                    "simpleItemModel",
                    "getSimpleItemModel",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator GetSimpleItemModel(
                Request.GetSimpleItemModelRequest request,
                UnityAction<AsyncResult<Result.GetSimpleItemModelResult>> callback
        )
		{
			var task = new GetSimpleItemModelTask(
			    Gs2WebSocketSession,
			    request
            );
            yield return task;
            callback.Invoke(new AsyncResult<Result.GetSimpleItemModelResult>(task.Result, task.Error));
        }

		public IFuture<Result.GetSimpleItemModelResult> GetSimpleItemModelFuture(
                Request.GetSimpleItemModelRequest request
        )
		{
			return new GetSimpleItemModelTask(
			    Gs2WebSocketSession,
			    request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.GetSimpleItemModelResult> GetSimpleItemModelAsync(
            Request.GetSimpleItemModelRequest request
        )
		{
		    var task = new GetSimpleItemModelTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
    #else
		public GetSimpleItemModelTask GetSimpleItemModelAsync(
                Request.GetSimpleItemModelRequest request
        )
		{
			return new GetSimpleItemModelTask(
                Gs2WebSocketSession,
			    request
            );
        }
    #endif
#else
		public async Task<Result.GetSimpleItemModelResult> GetSimpleItemModelAsync(
            Request.GetSimpleItemModelRequest request
        )
		{
		    var task = new GetSimpleItemModelTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
#endif


        public class CreateBigInventoryModelMasterTask : Gs2WebSocketSessionTask<Request.CreateBigInventoryModelMasterRequest, Result.CreateBigInventoryModelMasterResult>
        {
	        public CreateBigInventoryModelMasterTask(IGs2Session session, Request.CreateBigInventoryModelMasterRequest request) : base(session, request)
	        {
	        }

            protected override IGs2SessionRequest CreateRequest(Request.CreateBigInventoryModelMasterRequest request)
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
                    "inventory",
                    "bigInventoryModelMaster",
                    "createBigInventoryModelMaster",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator CreateBigInventoryModelMaster(
                Request.CreateBigInventoryModelMasterRequest request,
                UnityAction<AsyncResult<Result.CreateBigInventoryModelMasterResult>> callback
        )
		{
			var task = new CreateBigInventoryModelMasterTask(
			    Gs2WebSocketSession,
			    request
            );
            yield return task;
            callback.Invoke(new AsyncResult<Result.CreateBigInventoryModelMasterResult>(task.Result, task.Error));
        }

		public IFuture<Result.CreateBigInventoryModelMasterResult> CreateBigInventoryModelMasterFuture(
                Request.CreateBigInventoryModelMasterRequest request
        )
		{
			return new CreateBigInventoryModelMasterTask(
			    Gs2WebSocketSession,
			    request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.CreateBigInventoryModelMasterResult> CreateBigInventoryModelMasterAsync(
            Request.CreateBigInventoryModelMasterRequest request
        )
		{
		    var task = new CreateBigInventoryModelMasterTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
    #else
		public CreateBigInventoryModelMasterTask CreateBigInventoryModelMasterAsync(
                Request.CreateBigInventoryModelMasterRequest request
        )
		{
			return new CreateBigInventoryModelMasterTask(
                Gs2WebSocketSession,
			    request
            );
        }
    #endif
#else
		public async Task<Result.CreateBigInventoryModelMasterResult> CreateBigInventoryModelMasterAsync(
            Request.CreateBigInventoryModelMasterRequest request
        )
		{
		    var task = new CreateBigInventoryModelMasterTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
#endif


        public class GetBigInventoryModelMasterTask : Gs2WebSocketSessionTask<Request.GetBigInventoryModelMasterRequest, Result.GetBigInventoryModelMasterResult>
        {
	        public GetBigInventoryModelMasterTask(IGs2Session session, Request.GetBigInventoryModelMasterRequest request) : base(session, request)
	        {
	        }

            protected override IGs2SessionRequest CreateRequest(Request.GetBigInventoryModelMasterRequest request)
            {
                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);

                jsonWriter.WriteObjectStart();

                if (request.NamespaceName != null)
                {
                    jsonWriter.WritePropertyName("namespaceName");
                    jsonWriter.Write(request.NamespaceName.ToString());
                }
                if (request.InventoryName != null)
                {
                    jsonWriter.WritePropertyName("inventoryName");
                    jsonWriter.Write(request.InventoryName.ToString());
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
                    "inventory",
                    "bigInventoryModelMaster",
                    "getBigInventoryModelMaster",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator GetBigInventoryModelMaster(
                Request.GetBigInventoryModelMasterRequest request,
                UnityAction<AsyncResult<Result.GetBigInventoryModelMasterResult>> callback
        )
		{
			var task = new GetBigInventoryModelMasterTask(
			    Gs2WebSocketSession,
			    request
            );
            yield return task;
            callback.Invoke(new AsyncResult<Result.GetBigInventoryModelMasterResult>(task.Result, task.Error));
        }

		public IFuture<Result.GetBigInventoryModelMasterResult> GetBigInventoryModelMasterFuture(
                Request.GetBigInventoryModelMasterRequest request
        )
		{
			return new GetBigInventoryModelMasterTask(
			    Gs2WebSocketSession,
			    request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.GetBigInventoryModelMasterResult> GetBigInventoryModelMasterAsync(
            Request.GetBigInventoryModelMasterRequest request
        )
		{
		    var task = new GetBigInventoryModelMasterTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
    #else
		public GetBigInventoryModelMasterTask GetBigInventoryModelMasterAsync(
                Request.GetBigInventoryModelMasterRequest request
        )
		{
			return new GetBigInventoryModelMasterTask(
                Gs2WebSocketSession,
			    request
            );
        }
    #endif
#else
		public async Task<Result.GetBigInventoryModelMasterResult> GetBigInventoryModelMasterAsync(
            Request.GetBigInventoryModelMasterRequest request
        )
		{
		    var task = new GetBigInventoryModelMasterTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
#endif


        public class UpdateBigInventoryModelMasterTask : Gs2WebSocketSessionTask<Request.UpdateBigInventoryModelMasterRequest, Result.UpdateBigInventoryModelMasterResult>
        {
	        public UpdateBigInventoryModelMasterTask(IGs2Session session, Request.UpdateBigInventoryModelMasterRequest request) : base(session, request)
	        {
	        }

            protected override IGs2SessionRequest CreateRequest(Request.UpdateBigInventoryModelMasterRequest request)
            {
                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);

                jsonWriter.WriteObjectStart();

                if (request.NamespaceName != null)
                {
                    jsonWriter.WritePropertyName("namespaceName");
                    jsonWriter.Write(request.NamespaceName.ToString());
                }
                if (request.InventoryName != null)
                {
                    jsonWriter.WritePropertyName("inventoryName");
                    jsonWriter.Write(request.InventoryName.ToString());
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
                    "inventory",
                    "bigInventoryModelMaster",
                    "updateBigInventoryModelMaster",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator UpdateBigInventoryModelMaster(
                Request.UpdateBigInventoryModelMasterRequest request,
                UnityAction<AsyncResult<Result.UpdateBigInventoryModelMasterResult>> callback
        )
		{
			var task = new UpdateBigInventoryModelMasterTask(
			    Gs2WebSocketSession,
			    request
            );
            yield return task;
            callback.Invoke(new AsyncResult<Result.UpdateBigInventoryModelMasterResult>(task.Result, task.Error));
        }

		public IFuture<Result.UpdateBigInventoryModelMasterResult> UpdateBigInventoryModelMasterFuture(
                Request.UpdateBigInventoryModelMasterRequest request
        )
		{
			return new UpdateBigInventoryModelMasterTask(
			    Gs2WebSocketSession,
			    request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.UpdateBigInventoryModelMasterResult> UpdateBigInventoryModelMasterAsync(
            Request.UpdateBigInventoryModelMasterRequest request
        )
		{
		    var task = new UpdateBigInventoryModelMasterTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
    #else
		public UpdateBigInventoryModelMasterTask UpdateBigInventoryModelMasterAsync(
                Request.UpdateBigInventoryModelMasterRequest request
        )
		{
			return new UpdateBigInventoryModelMasterTask(
                Gs2WebSocketSession,
			    request
            );
        }
    #endif
#else
		public async Task<Result.UpdateBigInventoryModelMasterResult> UpdateBigInventoryModelMasterAsync(
            Request.UpdateBigInventoryModelMasterRequest request
        )
		{
		    var task = new UpdateBigInventoryModelMasterTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
#endif


        public class DeleteBigInventoryModelMasterTask : Gs2WebSocketSessionTask<Request.DeleteBigInventoryModelMasterRequest, Result.DeleteBigInventoryModelMasterResult>
        {
	        public DeleteBigInventoryModelMasterTask(IGs2Session session, Request.DeleteBigInventoryModelMasterRequest request) : base(session, request)
	        {
	        }

            protected override IGs2SessionRequest CreateRequest(Request.DeleteBigInventoryModelMasterRequest request)
            {
                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);

                jsonWriter.WriteObjectStart();

                if (request.NamespaceName != null)
                {
                    jsonWriter.WritePropertyName("namespaceName");
                    jsonWriter.Write(request.NamespaceName.ToString());
                }
                if (request.InventoryName != null)
                {
                    jsonWriter.WritePropertyName("inventoryName");
                    jsonWriter.Write(request.InventoryName.ToString());
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
                    "inventory",
                    "bigInventoryModelMaster",
                    "deleteBigInventoryModelMaster",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator DeleteBigInventoryModelMaster(
                Request.DeleteBigInventoryModelMasterRequest request,
                UnityAction<AsyncResult<Result.DeleteBigInventoryModelMasterResult>> callback
        )
		{
			var task = new DeleteBigInventoryModelMasterTask(
			    Gs2WebSocketSession,
			    request
            );
            yield return task;
            callback.Invoke(new AsyncResult<Result.DeleteBigInventoryModelMasterResult>(task.Result, task.Error));
        }

		public IFuture<Result.DeleteBigInventoryModelMasterResult> DeleteBigInventoryModelMasterFuture(
                Request.DeleteBigInventoryModelMasterRequest request
        )
		{
			return new DeleteBigInventoryModelMasterTask(
			    Gs2WebSocketSession,
			    request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.DeleteBigInventoryModelMasterResult> DeleteBigInventoryModelMasterAsync(
            Request.DeleteBigInventoryModelMasterRequest request
        )
		{
		    var task = new DeleteBigInventoryModelMasterTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
    #else
		public DeleteBigInventoryModelMasterTask DeleteBigInventoryModelMasterAsync(
                Request.DeleteBigInventoryModelMasterRequest request
        )
		{
			return new DeleteBigInventoryModelMasterTask(
                Gs2WebSocketSession,
			    request
            );
        }
    #endif
#else
		public async Task<Result.DeleteBigInventoryModelMasterResult> DeleteBigInventoryModelMasterAsync(
            Request.DeleteBigInventoryModelMasterRequest request
        )
		{
		    var task = new DeleteBigInventoryModelMasterTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
#endif


        public class CreateBigItemModelMasterTask : Gs2WebSocketSessionTask<Request.CreateBigItemModelMasterRequest, Result.CreateBigItemModelMasterResult>
        {
	        public CreateBigItemModelMasterTask(IGs2Session session, Request.CreateBigItemModelMasterRequest request) : base(session, request)
	        {
	        }

            protected override IGs2SessionRequest CreateRequest(Request.CreateBigItemModelMasterRequest request)
            {
                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);

                jsonWriter.WriteObjectStart();

                if (request.NamespaceName != null)
                {
                    jsonWriter.WritePropertyName("namespaceName");
                    jsonWriter.Write(request.NamespaceName.ToString());
                }
                if (request.InventoryName != null)
                {
                    jsonWriter.WritePropertyName("inventoryName");
                    jsonWriter.Write(request.InventoryName.ToString());
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
                    "inventory",
                    "bigItemModelMaster",
                    "createBigItemModelMaster",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator CreateBigItemModelMaster(
                Request.CreateBigItemModelMasterRequest request,
                UnityAction<AsyncResult<Result.CreateBigItemModelMasterResult>> callback
        )
		{
			var task = new CreateBigItemModelMasterTask(
			    Gs2WebSocketSession,
			    request
            );
            yield return task;
            callback.Invoke(new AsyncResult<Result.CreateBigItemModelMasterResult>(task.Result, task.Error));
        }

		public IFuture<Result.CreateBigItemModelMasterResult> CreateBigItemModelMasterFuture(
                Request.CreateBigItemModelMasterRequest request
        )
		{
			return new CreateBigItemModelMasterTask(
			    Gs2WebSocketSession,
			    request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.CreateBigItemModelMasterResult> CreateBigItemModelMasterAsync(
            Request.CreateBigItemModelMasterRequest request
        )
		{
		    var task = new CreateBigItemModelMasterTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
    #else
		public CreateBigItemModelMasterTask CreateBigItemModelMasterAsync(
                Request.CreateBigItemModelMasterRequest request
        )
		{
			return new CreateBigItemModelMasterTask(
                Gs2WebSocketSession,
			    request
            );
        }
    #endif
#else
		public async Task<Result.CreateBigItemModelMasterResult> CreateBigItemModelMasterAsync(
            Request.CreateBigItemModelMasterRequest request
        )
		{
		    var task = new CreateBigItemModelMasterTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
#endif


        public class GetBigItemModelMasterTask : Gs2WebSocketSessionTask<Request.GetBigItemModelMasterRequest, Result.GetBigItemModelMasterResult>
        {
	        public GetBigItemModelMasterTask(IGs2Session session, Request.GetBigItemModelMasterRequest request) : base(session, request)
	        {
	        }

            protected override IGs2SessionRequest CreateRequest(Request.GetBigItemModelMasterRequest request)
            {
                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);

                jsonWriter.WriteObjectStart();

                if (request.NamespaceName != null)
                {
                    jsonWriter.WritePropertyName("namespaceName");
                    jsonWriter.Write(request.NamespaceName.ToString());
                }
                if (request.InventoryName != null)
                {
                    jsonWriter.WritePropertyName("inventoryName");
                    jsonWriter.Write(request.InventoryName.ToString());
                }
                if (request.ItemName != null)
                {
                    jsonWriter.WritePropertyName("itemName");
                    jsonWriter.Write(request.ItemName.ToString());
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
                    "inventory",
                    "bigItemModelMaster",
                    "getBigItemModelMaster",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator GetBigItemModelMaster(
                Request.GetBigItemModelMasterRequest request,
                UnityAction<AsyncResult<Result.GetBigItemModelMasterResult>> callback
        )
		{
			var task = new GetBigItemModelMasterTask(
			    Gs2WebSocketSession,
			    request
            );
            yield return task;
            callback.Invoke(new AsyncResult<Result.GetBigItemModelMasterResult>(task.Result, task.Error));
        }

		public IFuture<Result.GetBigItemModelMasterResult> GetBigItemModelMasterFuture(
                Request.GetBigItemModelMasterRequest request
        )
		{
			return new GetBigItemModelMasterTask(
			    Gs2WebSocketSession,
			    request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.GetBigItemModelMasterResult> GetBigItemModelMasterAsync(
            Request.GetBigItemModelMasterRequest request
        )
		{
		    var task = new GetBigItemModelMasterTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
    #else
		public GetBigItemModelMasterTask GetBigItemModelMasterAsync(
                Request.GetBigItemModelMasterRequest request
        )
		{
			return new GetBigItemModelMasterTask(
                Gs2WebSocketSession,
			    request
            );
        }
    #endif
#else
		public async Task<Result.GetBigItemModelMasterResult> GetBigItemModelMasterAsync(
            Request.GetBigItemModelMasterRequest request
        )
		{
		    var task = new GetBigItemModelMasterTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
#endif


        public class UpdateBigItemModelMasterTask : Gs2WebSocketSessionTask<Request.UpdateBigItemModelMasterRequest, Result.UpdateBigItemModelMasterResult>
        {
	        public UpdateBigItemModelMasterTask(IGs2Session session, Request.UpdateBigItemModelMasterRequest request) : base(session, request)
	        {
	        }

            protected override IGs2SessionRequest CreateRequest(Request.UpdateBigItemModelMasterRequest request)
            {
                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);

                jsonWriter.WriteObjectStart();

                if (request.NamespaceName != null)
                {
                    jsonWriter.WritePropertyName("namespaceName");
                    jsonWriter.Write(request.NamespaceName.ToString());
                }
                if (request.InventoryName != null)
                {
                    jsonWriter.WritePropertyName("inventoryName");
                    jsonWriter.Write(request.InventoryName.ToString());
                }
                if (request.ItemName != null)
                {
                    jsonWriter.WritePropertyName("itemName");
                    jsonWriter.Write(request.ItemName.ToString());
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
                    "inventory",
                    "bigItemModelMaster",
                    "updateBigItemModelMaster",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator UpdateBigItemModelMaster(
                Request.UpdateBigItemModelMasterRequest request,
                UnityAction<AsyncResult<Result.UpdateBigItemModelMasterResult>> callback
        )
		{
			var task = new UpdateBigItemModelMasterTask(
			    Gs2WebSocketSession,
			    request
            );
            yield return task;
            callback.Invoke(new AsyncResult<Result.UpdateBigItemModelMasterResult>(task.Result, task.Error));
        }

		public IFuture<Result.UpdateBigItemModelMasterResult> UpdateBigItemModelMasterFuture(
                Request.UpdateBigItemModelMasterRequest request
        )
		{
			return new UpdateBigItemModelMasterTask(
			    Gs2WebSocketSession,
			    request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.UpdateBigItemModelMasterResult> UpdateBigItemModelMasterAsync(
            Request.UpdateBigItemModelMasterRequest request
        )
		{
		    var task = new UpdateBigItemModelMasterTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
    #else
		public UpdateBigItemModelMasterTask UpdateBigItemModelMasterAsync(
                Request.UpdateBigItemModelMasterRequest request
        )
		{
			return new UpdateBigItemModelMasterTask(
                Gs2WebSocketSession,
			    request
            );
        }
    #endif
#else
		public async Task<Result.UpdateBigItemModelMasterResult> UpdateBigItemModelMasterAsync(
            Request.UpdateBigItemModelMasterRequest request
        )
		{
		    var task = new UpdateBigItemModelMasterTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
#endif


        public class DeleteBigItemModelMasterTask : Gs2WebSocketSessionTask<Request.DeleteBigItemModelMasterRequest, Result.DeleteBigItemModelMasterResult>
        {
	        public DeleteBigItemModelMasterTask(IGs2Session session, Request.DeleteBigItemModelMasterRequest request) : base(session, request)
	        {
	        }

            protected override IGs2SessionRequest CreateRequest(Request.DeleteBigItemModelMasterRequest request)
            {
                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);

                jsonWriter.WriteObjectStart();

                if (request.NamespaceName != null)
                {
                    jsonWriter.WritePropertyName("namespaceName");
                    jsonWriter.Write(request.NamespaceName.ToString());
                }
                if (request.InventoryName != null)
                {
                    jsonWriter.WritePropertyName("inventoryName");
                    jsonWriter.Write(request.InventoryName.ToString());
                }
                if (request.ItemName != null)
                {
                    jsonWriter.WritePropertyName("itemName");
                    jsonWriter.Write(request.ItemName.ToString());
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
                    "inventory",
                    "bigItemModelMaster",
                    "deleteBigItemModelMaster",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator DeleteBigItemModelMaster(
                Request.DeleteBigItemModelMasterRequest request,
                UnityAction<AsyncResult<Result.DeleteBigItemModelMasterResult>> callback
        )
		{
			var task = new DeleteBigItemModelMasterTask(
			    Gs2WebSocketSession,
			    request
            );
            yield return task;
            callback.Invoke(new AsyncResult<Result.DeleteBigItemModelMasterResult>(task.Result, task.Error));
        }

		public IFuture<Result.DeleteBigItemModelMasterResult> DeleteBigItemModelMasterFuture(
                Request.DeleteBigItemModelMasterRequest request
        )
		{
			return new DeleteBigItemModelMasterTask(
			    Gs2WebSocketSession,
			    request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.DeleteBigItemModelMasterResult> DeleteBigItemModelMasterAsync(
            Request.DeleteBigItemModelMasterRequest request
        )
		{
		    var task = new DeleteBigItemModelMasterTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
    #else
		public DeleteBigItemModelMasterTask DeleteBigItemModelMasterAsync(
                Request.DeleteBigItemModelMasterRequest request
        )
		{
			return new DeleteBigItemModelMasterTask(
                Gs2WebSocketSession,
			    request
            );
        }
    #endif
#else
		public async Task<Result.DeleteBigItemModelMasterResult> DeleteBigItemModelMasterAsync(
            Request.DeleteBigItemModelMasterRequest request
        )
		{
		    var task = new DeleteBigItemModelMasterTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
#endif


        public class GetBigItemModelTask : Gs2WebSocketSessionTask<Request.GetBigItemModelRequest, Result.GetBigItemModelResult>
        {
	        public GetBigItemModelTask(IGs2Session session, Request.GetBigItemModelRequest request) : base(session, request)
	        {
	        }

            protected override IGs2SessionRequest CreateRequest(Request.GetBigItemModelRequest request)
            {
                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);

                jsonWriter.WriteObjectStart();

                if (request.NamespaceName != null)
                {
                    jsonWriter.WritePropertyName("namespaceName");
                    jsonWriter.Write(request.NamespaceName.ToString());
                }
                if (request.InventoryName != null)
                {
                    jsonWriter.WritePropertyName("inventoryName");
                    jsonWriter.Write(request.InventoryName.ToString());
                }
                if (request.ItemName != null)
                {
                    jsonWriter.WritePropertyName("itemName");
                    jsonWriter.Write(request.ItemName.ToString());
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
                    "inventory",
                    "bigItemModel",
                    "getBigItemModel",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator GetBigItemModel(
                Request.GetBigItemModelRequest request,
                UnityAction<AsyncResult<Result.GetBigItemModelResult>> callback
        )
		{
			var task = new GetBigItemModelTask(
			    Gs2WebSocketSession,
			    request
            );
            yield return task;
            callback.Invoke(new AsyncResult<Result.GetBigItemModelResult>(task.Result, task.Error));
        }

		public IFuture<Result.GetBigItemModelResult> GetBigItemModelFuture(
                Request.GetBigItemModelRequest request
        )
		{
			return new GetBigItemModelTask(
			    Gs2WebSocketSession,
			    request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.GetBigItemModelResult> GetBigItemModelAsync(
            Request.GetBigItemModelRequest request
        )
		{
		    var task = new GetBigItemModelTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
    #else
		public GetBigItemModelTask GetBigItemModelAsync(
                Request.GetBigItemModelRequest request
        )
		{
			return new GetBigItemModelTask(
                Gs2WebSocketSession,
			    request
            );
        }
    #endif
#else
		public async Task<Result.GetBigItemModelResult> GetBigItemModelAsync(
            Request.GetBigItemModelRequest request
        )
		{
		    var task = new GetBigItemModelTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
#endif


        public class GetInventoryTask : Gs2WebSocketSessionTask<Request.GetInventoryRequest, Result.GetInventoryResult>
        {
	        public GetInventoryTask(IGs2Session session, Request.GetInventoryRequest request) : base(session, request)
	        {
	        }

            protected override IGs2SessionRequest CreateRequest(Request.GetInventoryRequest request)
            {
                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);

                jsonWriter.WriteObjectStart();

                if (request.NamespaceName != null)
                {
                    jsonWriter.WritePropertyName("namespaceName");
                    jsonWriter.Write(request.NamespaceName.ToString());
                }
                if (request.InventoryName != null)
                {
                    jsonWriter.WritePropertyName("inventoryName");
                    jsonWriter.Write(request.InventoryName.ToString());
                }
                if (request.AccessToken != null)
                {
                    jsonWriter.WritePropertyName("accessToken");
                    jsonWriter.Write(request.AccessToken.ToString());
                }
                if (request.ContextStack != null)
                {
                    jsonWriter.WritePropertyName("contextStack");
                    jsonWriter.Write(request.ContextStack.ToString());
                }
                if (request.AccessToken != null)
                {
                    jsonWriter.WritePropertyName("xGs2AccessToken");
                    jsonWriter.Write(request.AccessToken);
                }
                if (request.DryRun)
                {
                    jsonWriter.WritePropertyName("xGs2DryRun");
                    jsonWriter.Write("true");
                }

                AddHeader(
                    Session.Credential,
                    "inventory",
                    "inventory",
                    "getInventory",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator GetInventory(
                Request.GetInventoryRequest request,
                UnityAction<AsyncResult<Result.GetInventoryResult>> callback
        )
		{
			var task = new GetInventoryTask(
			    Gs2WebSocketSession,
			    request
            );
            yield return task;
            callback.Invoke(new AsyncResult<Result.GetInventoryResult>(task.Result, task.Error));
        }

		public IFuture<Result.GetInventoryResult> GetInventoryFuture(
                Request.GetInventoryRequest request
        )
		{
			return new GetInventoryTask(
			    Gs2WebSocketSession,
			    request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.GetInventoryResult> GetInventoryAsync(
            Request.GetInventoryRequest request
        )
		{
		    var task = new GetInventoryTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
    #else
		public GetInventoryTask GetInventoryAsync(
                Request.GetInventoryRequest request
        )
		{
			return new GetInventoryTask(
                Gs2WebSocketSession,
			    request
            );
        }
    #endif
#else
		public async Task<Result.GetInventoryResult> GetInventoryAsync(
            Request.GetInventoryRequest request
        )
		{
		    var task = new GetInventoryTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
#endif


        public class GetInventoryByUserIdTask : Gs2WebSocketSessionTask<Request.GetInventoryByUserIdRequest, Result.GetInventoryByUserIdResult>
        {
	        public GetInventoryByUserIdTask(IGs2Session session, Request.GetInventoryByUserIdRequest request) : base(session, request)
	        {
	        }

            protected override IGs2SessionRequest CreateRequest(Request.GetInventoryByUserIdRequest request)
            {
                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);

                jsonWriter.WriteObjectStart();

                if (request.NamespaceName != null)
                {
                    jsonWriter.WritePropertyName("namespaceName");
                    jsonWriter.Write(request.NamespaceName.ToString());
                }
                if (request.InventoryName != null)
                {
                    jsonWriter.WritePropertyName("inventoryName");
                    jsonWriter.Write(request.InventoryName.ToString());
                }
                if (request.UserId != null)
                {
                    jsonWriter.WritePropertyName("userId");
                    jsonWriter.Write(request.UserId.ToString());
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
                if (request.DryRun)
                {
                    jsonWriter.WritePropertyName("xGs2DryRun");
                    jsonWriter.Write("true");
                }

                AddHeader(
                    Session.Credential,
                    "inventory",
                    "inventory",
                    "getInventoryByUserId",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator GetInventoryByUserId(
                Request.GetInventoryByUserIdRequest request,
                UnityAction<AsyncResult<Result.GetInventoryByUserIdResult>> callback
        )
		{
			var task = new GetInventoryByUserIdTask(
			    Gs2WebSocketSession,
			    request
            );
            yield return task;
            callback.Invoke(new AsyncResult<Result.GetInventoryByUserIdResult>(task.Result, task.Error));
        }

		public IFuture<Result.GetInventoryByUserIdResult> GetInventoryByUserIdFuture(
                Request.GetInventoryByUserIdRequest request
        )
		{
			return new GetInventoryByUserIdTask(
			    Gs2WebSocketSession,
			    request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.GetInventoryByUserIdResult> GetInventoryByUserIdAsync(
            Request.GetInventoryByUserIdRequest request
        )
		{
		    var task = new GetInventoryByUserIdTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
    #else
		public GetInventoryByUserIdTask GetInventoryByUserIdAsync(
                Request.GetInventoryByUserIdRequest request
        )
		{
			return new GetInventoryByUserIdTask(
                Gs2WebSocketSession,
			    request
            );
        }
    #endif
#else
		public async Task<Result.GetInventoryByUserIdResult> GetInventoryByUserIdAsync(
            Request.GetInventoryByUserIdRequest request
        )
		{
		    var task = new GetInventoryByUserIdTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
#endif


        public class AddCapacityByUserIdTask : Gs2WebSocketSessionTask<Request.AddCapacityByUserIdRequest, Result.AddCapacityByUserIdResult>
        {
	        public AddCapacityByUserIdTask(IGs2Session session, Request.AddCapacityByUserIdRequest request) : base(session, request)
	        {
	        }

            protected override IGs2SessionRequest CreateRequest(Request.AddCapacityByUserIdRequest request)
            {
                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);

                jsonWriter.WriteObjectStart();

                if (request.NamespaceName != null)
                {
                    jsonWriter.WritePropertyName("namespaceName");
                    jsonWriter.Write(request.NamespaceName.ToString());
                }
                if (request.InventoryName != null)
                {
                    jsonWriter.WritePropertyName("inventoryName");
                    jsonWriter.Write(request.InventoryName.ToString());
                }
                if (request.UserId != null)
                {
                    jsonWriter.WritePropertyName("userId");
                    jsonWriter.Write(request.UserId.ToString());
                }
                if (request.AddCapacityValue != null)
                {
                    jsonWriter.WritePropertyName("addCapacityValue");
                    jsonWriter.Write(request.AddCapacityValue.ToString());
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
                    "inventory",
                    "inventory",
                    "addCapacityByUserId",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator AddCapacityByUserId(
                Request.AddCapacityByUserIdRequest request,
                UnityAction<AsyncResult<Result.AddCapacityByUserIdResult>> callback
        )
		{
			var task = new AddCapacityByUserIdTask(
			    Gs2WebSocketSession,
			    request
            );
            yield return task;
            callback.Invoke(new AsyncResult<Result.AddCapacityByUserIdResult>(task.Result, task.Error));
        }

		public IFuture<Result.AddCapacityByUserIdResult> AddCapacityByUserIdFuture(
                Request.AddCapacityByUserIdRequest request
        )
		{
			return new AddCapacityByUserIdTask(
			    Gs2WebSocketSession,
			    request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.AddCapacityByUserIdResult> AddCapacityByUserIdAsync(
            Request.AddCapacityByUserIdRequest request
        )
		{
		    var task = new AddCapacityByUserIdTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
    #else
		public AddCapacityByUserIdTask AddCapacityByUserIdAsync(
                Request.AddCapacityByUserIdRequest request
        )
		{
			return new AddCapacityByUserIdTask(
                Gs2WebSocketSession,
			    request
            );
        }
    #endif
#else
		public async Task<Result.AddCapacityByUserIdResult> AddCapacityByUserIdAsync(
            Request.AddCapacityByUserIdRequest request
        )
		{
		    var task = new AddCapacityByUserIdTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
#endif


        public class SetCapacityByUserIdTask : Gs2WebSocketSessionTask<Request.SetCapacityByUserIdRequest, Result.SetCapacityByUserIdResult>
        {
	        public SetCapacityByUserIdTask(IGs2Session session, Request.SetCapacityByUserIdRequest request) : base(session, request)
	        {
	        }

            protected override IGs2SessionRequest CreateRequest(Request.SetCapacityByUserIdRequest request)
            {
                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);

                jsonWriter.WriteObjectStart();

                if (request.NamespaceName != null)
                {
                    jsonWriter.WritePropertyName("namespaceName");
                    jsonWriter.Write(request.NamespaceName.ToString());
                }
                if (request.InventoryName != null)
                {
                    jsonWriter.WritePropertyName("inventoryName");
                    jsonWriter.Write(request.InventoryName.ToString());
                }
                if (request.UserId != null)
                {
                    jsonWriter.WritePropertyName("userId");
                    jsonWriter.Write(request.UserId.ToString());
                }
                if (request.NewCapacityValue != null)
                {
                    jsonWriter.WritePropertyName("newCapacityValue");
                    jsonWriter.Write(request.NewCapacityValue.ToString());
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
                    "inventory",
                    "inventory",
                    "setCapacityByUserId",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator SetCapacityByUserId(
                Request.SetCapacityByUserIdRequest request,
                UnityAction<AsyncResult<Result.SetCapacityByUserIdResult>> callback
        )
		{
			var task = new SetCapacityByUserIdTask(
			    Gs2WebSocketSession,
			    request
            );
            yield return task;
            callback.Invoke(new AsyncResult<Result.SetCapacityByUserIdResult>(task.Result, task.Error));
        }

		public IFuture<Result.SetCapacityByUserIdResult> SetCapacityByUserIdFuture(
                Request.SetCapacityByUserIdRequest request
        )
		{
			return new SetCapacityByUserIdTask(
			    Gs2WebSocketSession,
			    request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.SetCapacityByUserIdResult> SetCapacityByUserIdAsync(
            Request.SetCapacityByUserIdRequest request
        )
		{
		    var task = new SetCapacityByUserIdTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
    #else
		public SetCapacityByUserIdTask SetCapacityByUserIdAsync(
                Request.SetCapacityByUserIdRequest request
        )
		{
			return new SetCapacityByUserIdTask(
                Gs2WebSocketSession,
			    request
            );
        }
    #endif
#else
		public async Task<Result.SetCapacityByUserIdResult> SetCapacityByUserIdAsync(
            Request.SetCapacityByUserIdRequest request
        )
		{
		    var task = new SetCapacityByUserIdTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
#endif


        public class DeleteInventoryByUserIdTask : Gs2WebSocketSessionTask<Request.DeleteInventoryByUserIdRequest, Result.DeleteInventoryByUserIdResult>
        {
	        public DeleteInventoryByUserIdTask(IGs2Session session, Request.DeleteInventoryByUserIdRequest request) : base(session, request)
	        {
	        }

            protected override IGs2SessionRequest CreateRequest(Request.DeleteInventoryByUserIdRequest request)
            {
                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);

                jsonWriter.WriteObjectStart();

                if (request.NamespaceName != null)
                {
                    jsonWriter.WritePropertyName("namespaceName");
                    jsonWriter.Write(request.NamespaceName.ToString());
                }
                if (request.InventoryName != null)
                {
                    jsonWriter.WritePropertyName("inventoryName");
                    jsonWriter.Write(request.InventoryName.ToString());
                }
                if (request.UserId != null)
                {
                    jsonWriter.WritePropertyName("userId");
                    jsonWriter.Write(request.UserId.ToString());
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
                    "inventory",
                    "inventory",
                    "deleteInventoryByUserId",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator DeleteInventoryByUserId(
                Request.DeleteInventoryByUserIdRequest request,
                UnityAction<AsyncResult<Result.DeleteInventoryByUserIdResult>> callback
        )
		{
			var task = new DeleteInventoryByUserIdTask(
			    Gs2WebSocketSession,
			    request
            );
            yield return task;
            callback.Invoke(new AsyncResult<Result.DeleteInventoryByUserIdResult>(task.Result, task.Error));
        }

		public IFuture<Result.DeleteInventoryByUserIdResult> DeleteInventoryByUserIdFuture(
                Request.DeleteInventoryByUserIdRequest request
        )
		{
			return new DeleteInventoryByUserIdTask(
			    Gs2WebSocketSession,
			    request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.DeleteInventoryByUserIdResult> DeleteInventoryByUserIdAsync(
            Request.DeleteInventoryByUserIdRequest request
        )
		{
		    var task = new DeleteInventoryByUserIdTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
    #else
		public DeleteInventoryByUserIdTask DeleteInventoryByUserIdAsync(
                Request.DeleteInventoryByUserIdRequest request
        )
		{
			return new DeleteInventoryByUserIdTask(
                Gs2WebSocketSession,
			    request
            );
        }
    #endif
#else
		public async Task<Result.DeleteInventoryByUserIdResult> DeleteInventoryByUserIdAsync(
            Request.DeleteInventoryByUserIdRequest request
        )
		{
		    var task = new DeleteInventoryByUserIdTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
#endif


        public class VerifyInventoryCurrentMaxCapacityTask : Gs2WebSocketSessionTask<Request.VerifyInventoryCurrentMaxCapacityRequest, Result.VerifyInventoryCurrentMaxCapacityResult>
        {
	        public VerifyInventoryCurrentMaxCapacityTask(IGs2Session session, Request.VerifyInventoryCurrentMaxCapacityRequest request) : base(session, request)
	        {
	        }

            protected override IGs2SessionRequest CreateRequest(Request.VerifyInventoryCurrentMaxCapacityRequest request)
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
                if (request.InventoryName != null)
                {
                    jsonWriter.WritePropertyName("inventoryName");
                    jsonWriter.Write(request.InventoryName.ToString());
                }
                if (request.VerifyType != null)
                {
                    jsonWriter.WritePropertyName("verifyType");
                    jsonWriter.Write(request.VerifyType.ToString());
                }
                if (request.CurrentInventoryMaxCapacity != null)
                {
                    jsonWriter.WritePropertyName("currentInventoryMaxCapacity");
                    jsonWriter.Write(request.CurrentInventoryMaxCapacity.ToString());
                }
                if (request.MultiplyValueSpecifyingQuantity != null)
                {
                    jsonWriter.WritePropertyName("multiplyValueSpecifyingQuantity");
                    jsonWriter.Write(request.MultiplyValueSpecifyingQuantity.ToString());
                }
                if (request.ContextStack != null)
                {
                    jsonWriter.WritePropertyName("contextStack");
                    jsonWriter.Write(request.ContextStack.ToString());
                }
                if (request.AccessToken != null)
                {
                    jsonWriter.WritePropertyName("xGs2AccessToken");
                    jsonWriter.Write(request.AccessToken);
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
                    "inventory",
                    "inventory",
                    "verifyInventoryCurrentMaxCapacity",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator VerifyInventoryCurrentMaxCapacity(
                Request.VerifyInventoryCurrentMaxCapacityRequest request,
                UnityAction<AsyncResult<Result.VerifyInventoryCurrentMaxCapacityResult>> callback
        )
		{
			var task = new VerifyInventoryCurrentMaxCapacityTask(
			    Gs2WebSocketSession,
			    request
            );
            yield return task;
            callback.Invoke(new AsyncResult<Result.VerifyInventoryCurrentMaxCapacityResult>(task.Result, task.Error));
        }

		public IFuture<Result.VerifyInventoryCurrentMaxCapacityResult> VerifyInventoryCurrentMaxCapacityFuture(
                Request.VerifyInventoryCurrentMaxCapacityRequest request
        )
		{
			return new VerifyInventoryCurrentMaxCapacityTask(
			    Gs2WebSocketSession,
			    request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.VerifyInventoryCurrentMaxCapacityResult> VerifyInventoryCurrentMaxCapacityAsync(
            Request.VerifyInventoryCurrentMaxCapacityRequest request
        )
		{
		    var task = new VerifyInventoryCurrentMaxCapacityTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
    #else
		public VerifyInventoryCurrentMaxCapacityTask VerifyInventoryCurrentMaxCapacityAsync(
                Request.VerifyInventoryCurrentMaxCapacityRequest request
        )
		{
			return new VerifyInventoryCurrentMaxCapacityTask(
                Gs2WebSocketSession,
			    request
            );
        }
    #endif
#else
		public async Task<Result.VerifyInventoryCurrentMaxCapacityResult> VerifyInventoryCurrentMaxCapacityAsync(
            Request.VerifyInventoryCurrentMaxCapacityRequest request
        )
		{
		    var task = new VerifyInventoryCurrentMaxCapacityTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
#endif


        public class VerifyInventoryCurrentMaxCapacityByUserIdTask : Gs2WebSocketSessionTask<Request.VerifyInventoryCurrentMaxCapacityByUserIdRequest, Result.VerifyInventoryCurrentMaxCapacityByUserIdResult>
        {
	        public VerifyInventoryCurrentMaxCapacityByUserIdTask(IGs2Session session, Request.VerifyInventoryCurrentMaxCapacityByUserIdRequest request) : base(session, request)
	        {
	        }

            protected override IGs2SessionRequest CreateRequest(Request.VerifyInventoryCurrentMaxCapacityByUserIdRequest request)
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
                if (request.InventoryName != null)
                {
                    jsonWriter.WritePropertyName("inventoryName");
                    jsonWriter.Write(request.InventoryName.ToString());
                }
                if (request.VerifyType != null)
                {
                    jsonWriter.WritePropertyName("verifyType");
                    jsonWriter.Write(request.VerifyType.ToString());
                }
                if (request.CurrentInventoryMaxCapacity != null)
                {
                    jsonWriter.WritePropertyName("currentInventoryMaxCapacity");
                    jsonWriter.Write(request.CurrentInventoryMaxCapacity.ToString());
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
                    "inventory",
                    "inventory",
                    "verifyInventoryCurrentMaxCapacityByUserId",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator VerifyInventoryCurrentMaxCapacityByUserId(
                Request.VerifyInventoryCurrentMaxCapacityByUserIdRequest request,
                UnityAction<AsyncResult<Result.VerifyInventoryCurrentMaxCapacityByUserIdResult>> callback
        )
		{
			var task = new VerifyInventoryCurrentMaxCapacityByUserIdTask(
			    Gs2WebSocketSession,
			    request
            );
            yield return task;
            callback.Invoke(new AsyncResult<Result.VerifyInventoryCurrentMaxCapacityByUserIdResult>(task.Result, task.Error));
        }

		public IFuture<Result.VerifyInventoryCurrentMaxCapacityByUserIdResult> VerifyInventoryCurrentMaxCapacityByUserIdFuture(
                Request.VerifyInventoryCurrentMaxCapacityByUserIdRequest request
        )
		{
			return new VerifyInventoryCurrentMaxCapacityByUserIdTask(
			    Gs2WebSocketSession,
			    request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.VerifyInventoryCurrentMaxCapacityByUserIdResult> VerifyInventoryCurrentMaxCapacityByUserIdAsync(
            Request.VerifyInventoryCurrentMaxCapacityByUserIdRequest request
        )
		{
		    var task = new VerifyInventoryCurrentMaxCapacityByUserIdTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
    #else
		public VerifyInventoryCurrentMaxCapacityByUserIdTask VerifyInventoryCurrentMaxCapacityByUserIdAsync(
                Request.VerifyInventoryCurrentMaxCapacityByUserIdRequest request
        )
		{
			return new VerifyInventoryCurrentMaxCapacityByUserIdTask(
                Gs2WebSocketSession,
			    request
            );
        }
    #endif
#else
		public async Task<Result.VerifyInventoryCurrentMaxCapacityByUserIdResult> VerifyInventoryCurrentMaxCapacityByUserIdAsync(
            Request.VerifyInventoryCurrentMaxCapacityByUserIdRequest request
        )
		{
		    var task = new VerifyInventoryCurrentMaxCapacityByUserIdTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
#endif


        public class AddCapacityByStampSheetTask : Gs2WebSocketSessionTask<Request.AddCapacityByStampSheetRequest, Result.AddCapacityByStampSheetResult>
        {
	        public AddCapacityByStampSheetTask(IGs2Session session, Request.AddCapacityByStampSheetRequest request) : base(session, request)
	        {
	        }

            protected override IGs2SessionRequest CreateRequest(Request.AddCapacityByStampSheetRequest request)
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
                if (request.DryRun)
                {
                    jsonWriter.WritePropertyName("xGs2DryRun");
                    jsonWriter.Write("true");
                }

                AddHeader(
                    Session.Credential,
                    "inventory",
                    "inventory",
                    "addCapacityByStampSheet",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator AddCapacityByStampSheet(
                Request.AddCapacityByStampSheetRequest request,
                UnityAction<AsyncResult<Result.AddCapacityByStampSheetResult>> callback
        )
		{
			var task = new AddCapacityByStampSheetTask(
			    Gs2WebSocketSession,
			    request
            );
            yield return task;
            callback.Invoke(new AsyncResult<Result.AddCapacityByStampSheetResult>(task.Result, task.Error));
        }

		public IFuture<Result.AddCapacityByStampSheetResult> AddCapacityByStampSheetFuture(
                Request.AddCapacityByStampSheetRequest request
        )
		{
			return new AddCapacityByStampSheetTask(
			    Gs2WebSocketSession,
			    request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.AddCapacityByStampSheetResult> AddCapacityByStampSheetAsync(
            Request.AddCapacityByStampSheetRequest request
        )
		{
		    var task = new AddCapacityByStampSheetTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
    #else
		public AddCapacityByStampSheetTask AddCapacityByStampSheetAsync(
                Request.AddCapacityByStampSheetRequest request
        )
		{
			return new AddCapacityByStampSheetTask(
                Gs2WebSocketSession,
			    request
            );
        }
    #endif
#else
		public async Task<Result.AddCapacityByStampSheetResult> AddCapacityByStampSheetAsync(
            Request.AddCapacityByStampSheetRequest request
        )
		{
		    var task = new AddCapacityByStampSheetTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
#endif


        public class SetCapacityByStampSheetTask : Gs2WebSocketSessionTask<Request.SetCapacityByStampSheetRequest, Result.SetCapacityByStampSheetResult>
        {
	        public SetCapacityByStampSheetTask(IGs2Session session, Request.SetCapacityByStampSheetRequest request) : base(session, request)
	        {
	        }

            protected override IGs2SessionRequest CreateRequest(Request.SetCapacityByStampSheetRequest request)
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
                if (request.DryRun)
                {
                    jsonWriter.WritePropertyName("xGs2DryRun");
                    jsonWriter.Write("true");
                }

                AddHeader(
                    Session.Credential,
                    "inventory",
                    "inventory",
                    "setCapacityByStampSheet",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator SetCapacityByStampSheet(
                Request.SetCapacityByStampSheetRequest request,
                UnityAction<AsyncResult<Result.SetCapacityByStampSheetResult>> callback
        )
		{
			var task = new SetCapacityByStampSheetTask(
			    Gs2WebSocketSession,
			    request
            );
            yield return task;
            callback.Invoke(new AsyncResult<Result.SetCapacityByStampSheetResult>(task.Result, task.Error));
        }

		public IFuture<Result.SetCapacityByStampSheetResult> SetCapacityByStampSheetFuture(
                Request.SetCapacityByStampSheetRequest request
        )
		{
			return new SetCapacityByStampSheetTask(
			    Gs2WebSocketSession,
			    request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.SetCapacityByStampSheetResult> SetCapacityByStampSheetAsync(
            Request.SetCapacityByStampSheetRequest request
        )
		{
		    var task = new SetCapacityByStampSheetTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
    #else
		public SetCapacityByStampSheetTask SetCapacityByStampSheetAsync(
                Request.SetCapacityByStampSheetRequest request
        )
		{
			return new SetCapacityByStampSheetTask(
                Gs2WebSocketSession,
			    request
            );
        }
    #endif
#else
		public async Task<Result.SetCapacityByStampSheetResult> SetCapacityByStampSheetAsync(
            Request.SetCapacityByStampSheetRequest request
        )
		{
		    var task = new SetCapacityByStampSheetTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
#endif


        public class AcquireItemSetWithGradeByUserIdTask : Gs2WebSocketSessionTask<Request.AcquireItemSetWithGradeByUserIdRequest, Result.AcquireItemSetWithGradeByUserIdResult>
        {
	        public AcquireItemSetWithGradeByUserIdTask(IGs2Session session, Request.AcquireItemSetWithGradeByUserIdRequest request) : base(session, request)
	        {
	        }

            protected override IGs2SessionRequest CreateRequest(Request.AcquireItemSetWithGradeByUserIdRequest request)
            {
                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);

                jsonWriter.WriteObjectStart();

                if (request.NamespaceName != null)
                {
                    jsonWriter.WritePropertyName("namespaceName");
                    jsonWriter.Write(request.NamespaceName.ToString());
                }
                if (request.InventoryName != null)
                {
                    jsonWriter.WritePropertyName("inventoryName");
                    jsonWriter.Write(request.InventoryName.ToString());
                }
                if (request.ItemName != null)
                {
                    jsonWriter.WritePropertyName("itemName");
                    jsonWriter.Write(request.ItemName.ToString());
                }
                if (request.UserId != null)
                {
                    jsonWriter.WritePropertyName("userId");
                    jsonWriter.Write(request.UserId.ToString());
                }
                if (request.GradeModelId != null)
                {
                    jsonWriter.WritePropertyName("gradeModelId");
                    jsonWriter.Write(request.GradeModelId.ToString());
                }
                if (request.GradeValue != null)
                {
                    jsonWriter.WritePropertyName("gradeValue");
                    jsonWriter.Write(request.GradeValue.ToString());
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
                    "inventory",
                    "itemSet",
                    "acquireItemSetWithGradeByUserId",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }

            public override void OnError(Gs2.Core.Exception.Gs2Exception error)
            {
                if (error.Errors.Count(v => v.code == "itemSet.operation.conflict") > 0) {
                    base.OnError(new Exception.ConflictException(error));
                }
                else {
                    base.OnError(error);
                }
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator AcquireItemSetWithGradeByUserId(
                Request.AcquireItemSetWithGradeByUserIdRequest request,
                UnityAction<AsyncResult<Result.AcquireItemSetWithGradeByUserIdResult>> callback
        )
		{
			var task = new AcquireItemSetWithGradeByUserIdTask(
			    Gs2WebSocketSession,
			    request
            );
            yield return task;
            callback.Invoke(new AsyncResult<Result.AcquireItemSetWithGradeByUserIdResult>(task.Result, task.Error));
        }

		public IFuture<Result.AcquireItemSetWithGradeByUserIdResult> AcquireItemSetWithGradeByUserIdFuture(
                Request.AcquireItemSetWithGradeByUserIdRequest request
        )
		{
			return new AcquireItemSetWithGradeByUserIdTask(
			    Gs2WebSocketSession,
			    request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.AcquireItemSetWithGradeByUserIdResult> AcquireItemSetWithGradeByUserIdAsync(
            Request.AcquireItemSetWithGradeByUserIdRequest request
        )
		{
		    var task = new AcquireItemSetWithGradeByUserIdTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
    #else
		public AcquireItemSetWithGradeByUserIdTask AcquireItemSetWithGradeByUserIdAsync(
                Request.AcquireItemSetWithGradeByUserIdRequest request
        )
		{
			return new AcquireItemSetWithGradeByUserIdTask(
                Gs2WebSocketSession,
			    request
            );
        }
    #endif
#else
		public async Task<Result.AcquireItemSetWithGradeByUserIdResult> AcquireItemSetWithGradeByUserIdAsync(
            Request.AcquireItemSetWithGradeByUserIdRequest request
        )
		{
		    var task = new AcquireItemSetWithGradeByUserIdTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
#endif


        public class VerifyItemSetTask : Gs2WebSocketSessionTask<Request.VerifyItemSetRequest, Result.VerifyItemSetResult>
        {
	        public VerifyItemSetTask(IGs2Session session, Request.VerifyItemSetRequest request) : base(session, request)
	        {
	        }

            protected override IGs2SessionRequest CreateRequest(Request.VerifyItemSetRequest request)
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
                if (request.InventoryName != null)
                {
                    jsonWriter.WritePropertyName("inventoryName");
                    jsonWriter.Write(request.InventoryName.ToString());
                }
                if (request.ItemName != null)
                {
                    jsonWriter.WritePropertyName("itemName");
                    jsonWriter.Write(request.ItemName.ToString());
                }
                if (request.VerifyType != null)
                {
                    jsonWriter.WritePropertyName("verifyType");
                    jsonWriter.Write(request.VerifyType.ToString());
                }
                if (request.ItemSetName != null)
                {
                    jsonWriter.WritePropertyName("itemSetName");
                    jsonWriter.Write(request.ItemSetName.ToString());
                }
                if (request.Count != null)
                {
                    jsonWriter.WritePropertyName("count");
                    jsonWriter.Write(request.Count.ToString());
                }
                if (request.MultiplyValueSpecifyingQuantity != null)
                {
                    jsonWriter.WritePropertyName("multiplyValueSpecifyingQuantity");
                    jsonWriter.Write(request.MultiplyValueSpecifyingQuantity.ToString());
                }
                if (request.ContextStack != null)
                {
                    jsonWriter.WritePropertyName("contextStack");
                    jsonWriter.Write(request.ContextStack.ToString());
                }
                if (request.AccessToken != null)
                {
                    jsonWriter.WritePropertyName("xGs2AccessToken");
                    jsonWriter.Write(request.AccessToken);
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
                    "inventory",
                    "itemSet",
                    "verifyItemSet",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator VerifyItemSet(
                Request.VerifyItemSetRequest request,
                UnityAction<AsyncResult<Result.VerifyItemSetResult>> callback
        )
		{
			var task = new VerifyItemSetTask(
			    Gs2WebSocketSession,
			    request
            );
            yield return task;
            callback.Invoke(new AsyncResult<Result.VerifyItemSetResult>(task.Result, task.Error));
        }

		public IFuture<Result.VerifyItemSetResult> VerifyItemSetFuture(
                Request.VerifyItemSetRequest request
        )
		{
			return new VerifyItemSetTask(
			    Gs2WebSocketSession,
			    request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.VerifyItemSetResult> VerifyItemSetAsync(
            Request.VerifyItemSetRequest request
        )
		{
		    var task = new VerifyItemSetTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
    #else
		public VerifyItemSetTask VerifyItemSetAsync(
                Request.VerifyItemSetRequest request
        )
		{
			return new VerifyItemSetTask(
                Gs2WebSocketSession,
			    request
            );
        }
    #endif
#else
		public async Task<Result.VerifyItemSetResult> VerifyItemSetAsync(
            Request.VerifyItemSetRequest request
        )
		{
		    var task = new VerifyItemSetTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
#endif


        public class VerifyItemSetByUserIdTask : Gs2WebSocketSessionTask<Request.VerifyItemSetByUserIdRequest, Result.VerifyItemSetByUserIdResult>
        {
	        public VerifyItemSetByUserIdTask(IGs2Session session, Request.VerifyItemSetByUserIdRequest request) : base(session, request)
	        {
	        }

            protected override IGs2SessionRequest CreateRequest(Request.VerifyItemSetByUserIdRequest request)
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
                if (request.InventoryName != null)
                {
                    jsonWriter.WritePropertyName("inventoryName");
                    jsonWriter.Write(request.InventoryName.ToString());
                }
                if (request.ItemName != null)
                {
                    jsonWriter.WritePropertyName("itemName");
                    jsonWriter.Write(request.ItemName.ToString());
                }
                if (request.VerifyType != null)
                {
                    jsonWriter.WritePropertyName("verifyType");
                    jsonWriter.Write(request.VerifyType.ToString());
                }
                if (request.ItemSetName != null)
                {
                    jsonWriter.WritePropertyName("itemSetName");
                    jsonWriter.Write(request.ItemSetName.ToString());
                }
                if (request.Count != null)
                {
                    jsonWriter.WritePropertyName("count");
                    jsonWriter.Write(request.Count.ToString());
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
                    "inventory",
                    "itemSet",
                    "verifyItemSetByUserId",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator VerifyItemSetByUserId(
                Request.VerifyItemSetByUserIdRequest request,
                UnityAction<AsyncResult<Result.VerifyItemSetByUserIdResult>> callback
        )
		{
			var task = new VerifyItemSetByUserIdTask(
			    Gs2WebSocketSession,
			    request
            );
            yield return task;
            callback.Invoke(new AsyncResult<Result.VerifyItemSetByUserIdResult>(task.Result, task.Error));
        }

		public IFuture<Result.VerifyItemSetByUserIdResult> VerifyItemSetByUserIdFuture(
                Request.VerifyItemSetByUserIdRequest request
        )
		{
			return new VerifyItemSetByUserIdTask(
			    Gs2WebSocketSession,
			    request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.VerifyItemSetByUserIdResult> VerifyItemSetByUserIdAsync(
            Request.VerifyItemSetByUserIdRequest request
        )
		{
		    var task = new VerifyItemSetByUserIdTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
    #else
		public VerifyItemSetByUserIdTask VerifyItemSetByUserIdAsync(
                Request.VerifyItemSetByUserIdRequest request
        )
		{
			return new VerifyItemSetByUserIdTask(
                Gs2WebSocketSession,
			    request
            );
        }
    #endif
#else
		public async Task<Result.VerifyItemSetByUserIdResult> VerifyItemSetByUserIdAsync(
            Request.VerifyItemSetByUserIdRequest request
        )
		{
		    var task = new VerifyItemSetByUserIdTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
#endif


        public class AcquireItemSetWithGradeByStampSheetTask : Gs2WebSocketSessionTask<Request.AcquireItemSetWithGradeByStampSheetRequest, Result.AcquireItemSetWithGradeByStampSheetResult>
        {
	        public AcquireItemSetWithGradeByStampSheetTask(IGs2Session session, Request.AcquireItemSetWithGradeByStampSheetRequest request) : base(session, request)
	        {
	        }

            protected override IGs2SessionRequest CreateRequest(Request.AcquireItemSetWithGradeByStampSheetRequest request)
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
                if (request.DryRun)
                {
                    jsonWriter.WritePropertyName("xGs2DryRun");
                    jsonWriter.Write("true");
                }

                AddHeader(
                    Session.Credential,
                    "inventory",
                    "itemSet",
                    "acquireItemSetWithGradeByStampSheet",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator AcquireItemSetWithGradeByStampSheet(
                Request.AcquireItemSetWithGradeByStampSheetRequest request,
                UnityAction<AsyncResult<Result.AcquireItemSetWithGradeByStampSheetResult>> callback
        )
		{
			var task = new AcquireItemSetWithGradeByStampSheetTask(
			    Gs2WebSocketSession,
			    request
            );
            yield return task;
            callback.Invoke(new AsyncResult<Result.AcquireItemSetWithGradeByStampSheetResult>(task.Result, task.Error));
        }

		public IFuture<Result.AcquireItemSetWithGradeByStampSheetResult> AcquireItemSetWithGradeByStampSheetFuture(
                Request.AcquireItemSetWithGradeByStampSheetRequest request
        )
		{
			return new AcquireItemSetWithGradeByStampSheetTask(
			    Gs2WebSocketSession,
			    request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.AcquireItemSetWithGradeByStampSheetResult> AcquireItemSetWithGradeByStampSheetAsync(
            Request.AcquireItemSetWithGradeByStampSheetRequest request
        )
		{
		    var task = new AcquireItemSetWithGradeByStampSheetTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
    #else
		public AcquireItemSetWithGradeByStampSheetTask AcquireItemSetWithGradeByStampSheetAsync(
                Request.AcquireItemSetWithGradeByStampSheetRequest request
        )
		{
			return new AcquireItemSetWithGradeByStampSheetTask(
                Gs2WebSocketSession,
			    request
            );
        }
    #endif
#else
		public async Task<Result.AcquireItemSetWithGradeByStampSheetResult> AcquireItemSetWithGradeByStampSheetAsync(
            Request.AcquireItemSetWithGradeByStampSheetRequest request
        )
		{
		    var task = new AcquireItemSetWithGradeByStampSheetTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
#endif


        public class GetSimpleItemTask : Gs2WebSocketSessionTask<Request.GetSimpleItemRequest, Result.GetSimpleItemResult>
        {
	        public GetSimpleItemTask(IGs2Session session, Request.GetSimpleItemRequest request) : base(session, request)
	        {
	        }

            protected override IGs2SessionRequest CreateRequest(Request.GetSimpleItemRequest request)
            {
                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);

                jsonWriter.WriteObjectStart();

                if (request.NamespaceName != null)
                {
                    jsonWriter.WritePropertyName("namespaceName");
                    jsonWriter.Write(request.NamespaceName.ToString());
                }
                if (request.InventoryName != null)
                {
                    jsonWriter.WritePropertyName("inventoryName");
                    jsonWriter.Write(request.InventoryName.ToString());
                }
                if (request.AccessToken != null)
                {
                    jsonWriter.WritePropertyName("accessToken");
                    jsonWriter.Write(request.AccessToken.ToString());
                }
                if (request.ItemName != null)
                {
                    jsonWriter.WritePropertyName("itemName");
                    jsonWriter.Write(request.ItemName.ToString());
                }
                if (request.ContextStack != null)
                {
                    jsonWriter.WritePropertyName("contextStack");
                    jsonWriter.Write(request.ContextStack.ToString());
                }
                if (request.AccessToken != null)
                {
                    jsonWriter.WritePropertyName("xGs2AccessToken");
                    jsonWriter.Write(request.AccessToken);
                }
                if (request.DryRun)
                {
                    jsonWriter.WritePropertyName("xGs2DryRun");
                    jsonWriter.Write("true");
                }

                AddHeader(
                    Session.Credential,
                    "inventory",
                    "simpleItem",
                    "getSimpleItem",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator GetSimpleItem(
                Request.GetSimpleItemRequest request,
                UnityAction<AsyncResult<Result.GetSimpleItemResult>> callback
        )
		{
			var task = new GetSimpleItemTask(
			    Gs2WebSocketSession,
			    request
            );
            yield return task;
            callback.Invoke(new AsyncResult<Result.GetSimpleItemResult>(task.Result, task.Error));
        }

		public IFuture<Result.GetSimpleItemResult> GetSimpleItemFuture(
                Request.GetSimpleItemRequest request
        )
		{
			return new GetSimpleItemTask(
			    Gs2WebSocketSession,
			    request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.GetSimpleItemResult> GetSimpleItemAsync(
            Request.GetSimpleItemRequest request
        )
		{
		    var task = new GetSimpleItemTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
    #else
		public GetSimpleItemTask GetSimpleItemAsync(
                Request.GetSimpleItemRequest request
        )
		{
			return new GetSimpleItemTask(
                Gs2WebSocketSession,
			    request
            );
        }
    #endif
#else
		public async Task<Result.GetSimpleItemResult> GetSimpleItemAsync(
            Request.GetSimpleItemRequest request
        )
		{
		    var task = new GetSimpleItemTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
#endif


        public class GetSimpleItemByUserIdTask : Gs2WebSocketSessionTask<Request.GetSimpleItemByUserIdRequest, Result.GetSimpleItemByUserIdResult>
        {
	        public GetSimpleItemByUserIdTask(IGs2Session session, Request.GetSimpleItemByUserIdRequest request) : base(session, request)
	        {
	        }

            protected override IGs2SessionRequest CreateRequest(Request.GetSimpleItemByUserIdRequest request)
            {
                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);

                jsonWriter.WriteObjectStart();

                if (request.NamespaceName != null)
                {
                    jsonWriter.WritePropertyName("namespaceName");
                    jsonWriter.Write(request.NamespaceName.ToString());
                }
                if (request.InventoryName != null)
                {
                    jsonWriter.WritePropertyName("inventoryName");
                    jsonWriter.Write(request.InventoryName.ToString());
                }
                if (request.UserId != null)
                {
                    jsonWriter.WritePropertyName("userId");
                    jsonWriter.Write(request.UserId.ToString());
                }
                if (request.ItemName != null)
                {
                    jsonWriter.WritePropertyName("itemName");
                    jsonWriter.Write(request.ItemName.ToString());
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
                if (request.DryRun)
                {
                    jsonWriter.WritePropertyName("xGs2DryRun");
                    jsonWriter.Write("true");
                }

                AddHeader(
                    Session.Credential,
                    "inventory",
                    "simpleItem",
                    "getSimpleItemByUserId",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator GetSimpleItemByUserId(
                Request.GetSimpleItemByUserIdRequest request,
                UnityAction<AsyncResult<Result.GetSimpleItemByUserIdResult>> callback
        )
		{
			var task = new GetSimpleItemByUserIdTask(
			    Gs2WebSocketSession,
			    request
            );
            yield return task;
            callback.Invoke(new AsyncResult<Result.GetSimpleItemByUserIdResult>(task.Result, task.Error));
        }

		public IFuture<Result.GetSimpleItemByUserIdResult> GetSimpleItemByUserIdFuture(
                Request.GetSimpleItemByUserIdRequest request
        )
		{
			return new GetSimpleItemByUserIdTask(
			    Gs2WebSocketSession,
			    request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.GetSimpleItemByUserIdResult> GetSimpleItemByUserIdAsync(
            Request.GetSimpleItemByUserIdRequest request
        )
		{
		    var task = new GetSimpleItemByUserIdTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
    #else
		public GetSimpleItemByUserIdTask GetSimpleItemByUserIdAsync(
                Request.GetSimpleItemByUserIdRequest request
        )
		{
			return new GetSimpleItemByUserIdTask(
                Gs2WebSocketSession,
			    request
            );
        }
    #endif
#else
		public async Task<Result.GetSimpleItemByUserIdResult> GetSimpleItemByUserIdAsync(
            Request.GetSimpleItemByUserIdRequest request
        )
		{
		    var task = new GetSimpleItemByUserIdTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
#endif


        public class DeleteSimpleItemsByUserIdTask : Gs2WebSocketSessionTask<Request.DeleteSimpleItemsByUserIdRequest, Result.DeleteSimpleItemsByUserIdResult>
        {
	        public DeleteSimpleItemsByUserIdTask(IGs2Session session, Request.DeleteSimpleItemsByUserIdRequest request) : base(session, request)
	        {
	        }

            protected override IGs2SessionRequest CreateRequest(Request.DeleteSimpleItemsByUserIdRequest request)
            {
                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);

                jsonWriter.WriteObjectStart();

                if (request.NamespaceName != null)
                {
                    jsonWriter.WritePropertyName("namespaceName");
                    jsonWriter.Write(request.NamespaceName.ToString());
                }
                if (request.InventoryName != null)
                {
                    jsonWriter.WritePropertyName("inventoryName");
                    jsonWriter.Write(request.InventoryName.ToString());
                }
                if (request.UserId != null)
                {
                    jsonWriter.WritePropertyName("userId");
                    jsonWriter.Write(request.UserId.ToString());
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
                    "inventory",
                    "simpleItem",
                    "deleteSimpleItemsByUserId",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator DeleteSimpleItemsByUserId(
                Request.DeleteSimpleItemsByUserIdRequest request,
                UnityAction<AsyncResult<Result.DeleteSimpleItemsByUserIdResult>> callback
        )
		{
			var task = new DeleteSimpleItemsByUserIdTask(
			    Gs2WebSocketSession,
			    request
            );
            yield return task;
            callback.Invoke(new AsyncResult<Result.DeleteSimpleItemsByUserIdResult>(task.Result, task.Error));
        }

		public IFuture<Result.DeleteSimpleItemsByUserIdResult> DeleteSimpleItemsByUserIdFuture(
                Request.DeleteSimpleItemsByUserIdRequest request
        )
		{
			return new DeleteSimpleItemsByUserIdTask(
			    Gs2WebSocketSession,
			    request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.DeleteSimpleItemsByUserIdResult> DeleteSimpleItemsByUserIdAsync(
            Request.DeleteSimpleItemsByUserIdRequest request
        )
		{
		    var task = new DeleteSimpleItemsByUserIdTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
    #else
		public DeleteSimpleItemsByUserIdTask DeleteSimpleItemsByUserIdAsync(
                Request.DeleteSimpleItemsByUserIdRequest request
        )
		{
			return new DeleteSimpleItemsByUserIdTask(
                Gs2WebSocketSession,
			    request
            );
        }
    #endif
#else
		public async Task<Result.DeleteSimpleItemsByUserIdResult> DeleteSimpleItemsByUserIdAsync(
            Request.DeleteSimpleItemsByUserIdRequest request
        )
		{
		    var task = new DeleteSimpleItemsByUserIdTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
#endif


        public class VerifySimpleItemTask : Gs2WebSocketSessionTask<Request.VerifySimpleItemRequest, Result.VerifySimpleItemResult>
        {
	        public VerifySimpleItemTask(IGs2Session session, Request.VerifySimpleItemRequest request) : base(session, request)
	        {
	        }

            protected override IGs2SessionRequest CreateRequest(Request.VerifySimpleItemRequest request)
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
                if (request.InventoryName != null)
                {
                    jsonWriter.WritePropertyName("inventoryName");
                    jsonWriter.Write(request.InventoryName.ToString());
                }
                if (request.ItemName != null)
                {
                    jsonWriter.WritePropertyName("itemName");
                    jsonWriter.Write(request.ItemName.ToString());
                }
                if (request.VerifyType != null)
                {
                    jsonWriter.WritePropertyName("verifyType");
                    jsonWriter.Write(request.VerifyType.ToString());
                }
                if (request.Count != null)
                {
                    jsonWriter.WritePropertyName("count");
                    jsonWriter.Write(request.Count.ToString());
                }
                if (request.MultiplyValueSpecifyingQuantity != null)
                {
                    jsonWriter.WritePropertyName("multiplyValueSpecifyingQuantity");
                    jsonWriter.Write(request.MultiplyValueSpecifyingQuantity.ToString());
                }
                if (request.ContextStack != null)
                {
                    jsonWriter.WritePropertyName("contextStack");
                    jsonWriter.Write(request.ContextStack.ToString());
                }
                if (request.AccessToken != null)
                {
                    jsonWriter.WritePropertyName("xGs2AccessToken");
                    jsonWriter.Write(request.AccessToken);
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
                    "inventory",
                    "simpleItem",
                    "verifySimpleItem",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator VerifySimpleItem(
                Request.VerifySimpleItemRequest request,
                UnityAction<AsyncResult<Result.VerifySimpleItemResult>> callback
        )
		{
			var task = new VerifySimpleItemTask(
			    Gs2WebSocketSession,
			    request
            );
            yield return task;
            callback.Invoke(new AsyncResult<Result.VerifySimpleItemResult>(task.Result, task.Error));
        }

		public IFuture<Result.VerifySimpleItemResult> VerifySimpleItemFuture(
                Request.VerifySimpleItemRequest request
        )
		{
			return new VerifySimpleItemTask(
			    Gs2WebSocketSession,
			    request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.VerifySimpleItemResult> VerifySimpleItemAsync(
            Request.VerifySimpleItemRequest request
        )
		{
		    var task = new VerifySimpleItemTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
    #else
		public VerifySimpleItemTask VerifySimpleItemAsync(
                Request.VerifySimpleItemRequest request
        )
		{
			return new VerifySimpleItemTask(
                Gs2WebSocketSession,
			    request
            );
        }
    #endif
#else
		public async Task<Result.VerifySimpleItemResult> VerifySimpleItemAsync(
            Request.VerifySimpleItemRequest request
        )
		{
		    var task = new VerifySimpleItemTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
#endif


        public class VerifySimpleItemByUserIdTask : Gs2WebSocketSessionTask<Request.VerifySimpleItemByUserIdRequest, Result.VerifySimpleItemByUserIdResult>
        {
	        public VerifySimpleItemByUserIdTask(IGs2Session session, Request.VerifySimpleItemByUserIdRequest request) : base(session, request)
	        {
	        }

            protected override IGs2SessionRequest CreateRequest(Request.VerifySimpleItemByUserIdRequest request)
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
                if (request.InventoryName != null)
                {
                    jsonWriter.WritePropertyName("inventoryName");
                    jsonWriter.Write(request.InventoryName.ToString());
                }
                if (request.ItemName != null)
                {
                    jsonWriter.WritePropertyName("itemName");
                    jsonWriter.Write(request.ItemName.ToString());
                }
                if (request.VerifyType != null)
                {
                    jsonWriter.WritePropertyName("verifyType");
                    jsonWriter.Write(request.VerifyType.ToString());
                }
                if (request.Count != null)
                {
                    jsonWriter.WritePropertyName("count");
                    jsonWriter.Write(request.Count.ToString());
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
                    "inventory",
                    "simpleItem",
                    "verifySimpleItemByUserId",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator VerifySimpleItemByUserId(
                Request.VerifySimpleItemByUserIdRequest request,
                UnityAction<AsyncResult<Result.VerifySimpleItemByUserIdResult>> callback
        )
		{
			var task = new VerifySimpleItemByUserIdTask(
			    Gs2WebSocketSession,
			    request
            );
            yield return task;
            callback.Invoke(new AsyncResult<Result.VerifySimpleItemByUserIdResult>(task.Result, task.Error));
        }

		public IFuture<Result.VerifySimpleItemByUserIdResult> VerifySimpleItemByUserIdFuture(
                Request.VerifySimpleItemByUserIdRequest request
        )
		{
			return new VerifySimpleItemByUserIdTask(
			    Gs2WebSocketSession,
			    request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.VerifySimpleItemByUserIdResult> VerifySimpleItemByUserIdAsync(
            Request.VerifySimpleItemByUserIdRequest request
        )
		{
		    var task = new VerifySimpleItemByUserIdTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
    #else
		public VerifySimpleItemByUserIdTask VerifySimpleItemByUserIdAsync(
                Request.VerifySimpleItemByUserIdRequest request
        )
		{
			return new VerifySimpleItemByUserIdTask(
                Gs2WebSocketSession,
			    request
            );
        }
    #endif
#else
		public async Task<Result.VerifySimpleItemByUserIdResult> VerifySimpleItemByUserIdAsync(
            Request.VerifySimpleItemByUserIdRequest request
        )
		{
		    var task = new VerifySimpleItemByUserIdTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
#endif


        public class GetBigItemTask : Gs2WebSocketSessionTask<Request.GetBigItemRequest, Result.GetBigItemResult>
        {
	        public GetBigItemTask(IGs2Session session, Request.GetBigItemRequest request) : base(session, request)
	        {
	        }

            protected override IGs2SessionRequest CreateRequest(Request.GetBigItemRequest request)
            {
                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);

                jsonWriter.WriteObjectStart();

                if (request.NamespaceName != null)
                {
                    jsonWriter.WritePropertyName("namespaceName");
                    jsonWriter.Write(request.NamespaceName.ToString());
                }
                if (request.InventoryName != null)
                {
                    jsonWriter.WritePropertyName("inventoryName");
                    jsonWriter.Write(request.InventoryName.ToString());
                }
                if (request.AccessToken != null)
                {
                    jsonWriter.WritePropertyName("accessToken");
                    jsonWriter.Write(request.AccessToken.ToString());
                }
                if (request.ItemName != null)
                {
                    jsonWriter.WritePropertyName("itemName");
                    jsonWriter.Write(request.ItemName.ToString());
                }
                if (request.ContextStack != null)
                {
                    jsonWriter.WritePropertyName("contextStack");
                    jsonWriter.Write(request.ContextStack.ToString());
                }
                if (request.AccessToken != null)
                {
                    jsonWriter.WritePropertyName("xGs2AccessToken");
                    jsonWriter.Write(request.AccessToken);
                }
                if (request.DryRun)
                {
                    jsonWriter.WritePropertyName("xGs2DryRun");
                    jsonWriter.Write("true");
                }

                AddHeader(
                    Session.Credential,
                    "inventory",
                    "bigItem",
                    "getBigItem",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator GetBigItem(
                Request.GetBigItemRequest request,
                UnityAction<AsyncResult<Result.GetBigItemResult>> callback
        )
		{
			var task = new GetBigItemTask(
			    Gs2WebSocketSession,
			    request
            );
            yield return task;
            callback.Invoke(new AsyncResult<Result.GetBigItemResult>(task.Result, task.Error));
        }

		public IFuture<Result.GetBigItemResult> GetBigItemFuture(
                Request.GetBigItemRequest request
        )
		{
			return new GetBigItemTask(
			    Gs2WebSocketSession,
			    request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.GetBigItemResult> GetBigItemAsync(
            Request.GetBigItemRequest request
        )
		{
		    var task = new GetBigItemTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
    #else
		public GetBigItemTask GetBigItemAsync(
                Request.GetBigItemRequest request
        )
		{
			return new GetBigItemTask(
                Gs2WebSocketSession,
			    request
            );
        }
    #endif
#else
		public async Task<Result.GetBigItemResult> GetBigItemAsync(
            Request.GetBigItemRequest request
        )
		{
		    var task = new GetBigItemTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
#endif


        public class GetBigItemByUserIdTask : Gs2WebSocketSessionTask<Request.GetBigItemByUserIdRequest, Result.GetBigItemByUserIdResult>
        {
	        public GetBigItemByUserIdTask(IGs2Session session, Request.GetBigItemByUserIdRequest request) : base(session, request)
	        {
	        }

            protected override IGs2SessionRequest CreateRequest(Request.GetBigItemByUserIdRequest request)
            {
                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);

                jsonWriter.WriteObjectStart();

                if (request.NamespaceName != null)
                {
                    jsonWriter.WritePropertyName("namespaceName");
                    jsonWriter.Write(request.NamespaceName.ToString());
                }
                if (request.InventoryName != null)
                {
                    jsonWriter.WritePropertyName("inventoryName");
                    jsonWriter.Write(request.InventoryName.ToString());
                }
                if (request.UserId != null)
                {
                    jsonWriter.WritePropertyName("userId");
                    jsonWriter.Write(request.UserId.ToString());
                }
                if (request.ItemName != null)
                {
                    jsonWriter.WritePropertyName("itemName");
                    jsonWriter.Write(request.ItemName.ToString());
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
                if (request.DryRun)
                {
                    jsonWriter.WritePropertyName("xGs2DryRun");
                    jsonWriter.Write("true");
                }

                AddHeader(
                    Session.Credential,
                    "inventory",
                    "bigItem",
                    "getBigItemByUserId",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator GetBigItemByUserId(
                Request.GetBigItemByUserIdRequest request,
                UnityAction<AsyncResult<Result.GetBigItemByUserIdResult>> callback
        )
		{
			var task = new GetBigItemByUserIdTask(
			    Gs2WebSocketSession,
			    request
            );
            yield return task;
            callback.Invoke(new AsyncResult<Result.GetBigItemByUserIdResult>(task.Result, task.Error));
        }

		public IFuture<Result.GetBigItemByUserIdResult> GetBigItemByUserIdFuture(
                Request.GetBigItemByUserIdRequest request
        )
		{
			return new GetBigItemByUserIdTask(
			    Gs2WebSocketSession,
			    request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.GetBigItemByUserIdResult> GetBigItemByUserIdAsync(
            Request.GetBigItemByUserIdRequest request
        )
		{
		    var task = new GetBigItemByUserIdTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
    #else
		public GetBigItemByUserIdTask GetBigItemByUserIdAsync(
                Request.GetBigItemByUserIdRequest request
        )
		{
			return new GetBigItemByUserIdTask(
                Gs2WebSocketSession,
			    request
            );
        }
    #endif
#else
		public async Task<Result.GetBigItemByUserIdResult> GetBigItemByUserIdAsync(
            Request.GetBigItemByUserIdRequest request
        )
		{
		    var task = new GetBigItemByUserIdTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
#endif


        public class AcquireBigItemByUserIdTask : Gs2WebSocketSessionTask<Request.AcquireBigItemByUserIdRequest, Result.AcquireBigItemByUserIdResult>
        {
	        public AcquireBigItemByUserIdTask(IGs2Session session, Request.AcquireBigItemByUserIdRequest request) : base(session, request)
	        {
	        }

            protected override IGs2SessionRequest CreateRequest(Request.AcquireBigItemByUserIdRequest request)
            {
                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);

                jsonWriter.WriteObjectStart();

                if (request.NamespaceName != null)
                {
                    jsonWriter.WritePropertyName("namespaceName");
                    jsonWriter.Write(request.NamespaceName.ToString());
                }
                if (request.InventoryName != null)
                {
                    jsonWriter.WritePropertyName("inventoryName");
                    jsonWriter.Write(request.InventoryName.ToString());
                }
                if (request.UserId != null)
                {
                    jsonWriter.WritePropertyName("userId");
                    jsonWriter.Write(request.UserId.ToString());
                }
                if (request.ItemName != null)
                {
                    jsonWriter.WritePropertyName("itemName");
                    jsonWriter.Write(request.ItemName.ToString());
                }
                if (request.AcquireCount != null)
                {
                    jsonWriter.WritePropertyName("acquireCount");
                    jsonWriter.Write(request.AcquireCount.ToString());
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
                    "inventory",
                    "bigItem",
                    "acquireBigItemByUserId",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }

            public override void OnError(Gs2.Core.Exception.Gs2Exception error)
            {
                if (error.Errors.Count(v => v.code == "itemSet.operation.conflict") > 0) {
                    base.OnError(new Exception.ConflictException(error));
                }
                else {
                    base.OnError(error);
                }
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator AcquireBigItemByUserId(
                Request.AcquireBigItemByUserIdRequest request,
                UnityAction<AsyncResult<Result.AcquireBigItemByUserIdResult>> callback
        )
		{
			var task = new AcquireBigItemByUserIdTask(
			    Gs2WebSocketSession,
			    request
            );
            yield return task;
            callback.Invoke(new AsyncResult<Result.AcquireBigItemByUserIdResult>(task.Result, task.Error));
        }

		public IFuture<Result.AcquireBigItemByUserIdResult> AcquireBigItemByUserIdFuture(
                Request.AcquireBigItemByUserIdRequest request
        )
		{
			return new AcquireBigItemByUserIdTask(
			    Gs2WebSocketSession,
			    request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.AcquireBigItemByUserIdResult> AcquireBigItemByUserIdAsync(
            Request.AcquireBigItemByUserIdRequest request
        )
		{
		    var task = new AcquireBigItemByUserIdTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
    #else
		public AcquireBigItemByUserIdTask AcquireBigItemByUserIdAsync(
                Request.AcquireBigItemByUserIdRequest request
        )
		{
			return new AcquireBigItemByUserIdTask(
                Gs2WebSocketSession,
			    request
            );
        }
    #endif
#else
		public async Task<Result.AcquireBigItemByUserIdResult> AcquireBigItemByUserIdAsync(
            Request.AcquireBigItemByUserIdRequest request
        )
		{
		    var task = new AcquireBigItemByUserIdTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
#endif


        public class ConsumeBigItemTask : Gs2WebSocketSessionTask<Request.ConsumeBigItemRequest, Result.ConsumeBigItemResult>
        {
	        public ConsumeBigItemTask(IGs2Session session, Request.ConsumeBigItemRequest request) : base(session, request)
	        {
	        }

            protected override IGs2SessionRequest CreateRequest(Request.ConsumeBigItemRequest request)
            {
                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);

                jsonWriter.WriteObjectStart();

                if (request.NamespaceName != null)
                {
                    jsonWriter.WritePropertyName("namespaceName");
                    jsonWriter.Write(request.NamespaceName.ToString());
                }
                if (request.InventoryName != null)
                {
                    jsonWriter.WritePropertyName("inventoryName");
                    jsonWriter.Write(request.InventoryName.ToString());
                }
                if (request.AccessToken != null)
                {
                    jsonWriter.WritePropertyName("accessToken");
                    jsonWriter.Write(request.AccessToken.ToString());
                }
                if (request.ItemName != null)
                {
                    jsonWriter.WritePropertyName("itemName");
                    jsonWriter.Write(request.ItemName.ToString());
                }
                if (request.ConsumeCount != null)
                {
                    jsonWriter.WritePropertyName("consumeCount");
                    jsonWriter.Write(request.ConsumeCount.ToString());
                }
                if (request.ContextStack != null)
                {
                    jsonWriter.WritePropertyName("contextStack");
                    jsonWriter.Write(request.ContextStack.ToString());
                }
                if (request.AccessToken != null)
                {
                    jsonWriter.WritePropertyName("xGs2AccessToken");
                    jsonWriter.Write(request.AccessToken);
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
                    "inventory",
                    "bigItem",
                    "consumeBigItem",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }

            public override void OnError(Gs2.Core.Exception.Gs2Exception error)
            {
                if (error.Errors.Count(v => v.code == "itemSet.operation.conflict") > 0) {
                    base.OnError(new Exception.ConflictException(error));
                }
                else if (error.Errors.Count(v => v.code == "itemSet.count.insufficient") > 0) {
                    base.OnError(new Exception.InsufficientException(error));
                }
                else {
                    base.OnError(error);
                }
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator ConsumeBigItem(
                Request.ConsumeBigItemRequest request,
                UnityAction<AsyncResult<Result.ConsumeBigItemResult>> callback
        )
		{
			var task = new ConsumeBigItemTask(
			    Gs2WebSocketSession,
			    request
            );
            yield return task;
            callback.Invoke(new AsyncResult<Result.ConsumeBigItemResult>(task.Result, task.Error));
        }

		public IFuture<Result.ConsumeBigItemResult> ConsumeBigItemFuture(
                Request.ConsumeBigItemRequest request
        )
		{
			return new ConsumeBigItemTask(
			    Gs2WebSocketSession,
			    request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.ConsumeBigItemResult> ConsumeBigItemAsync(
            Request.ConsumeBigItemRequest request
        )
		{
		    var task = new ConsumeBigItemTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
    #else
		public ConsumeBigItemTask ConsumeBigItemAsync(
                Request.ConsumeBigItemRequest request
        )
		{
			return new ConsumeBigItemTask(
                Gs2WebSocketSession,
			    request
            );
        }
    #endif
#else
		public async Task<Result.ConsumeBigItemResult> ConsumeBigItemAsync(
            Request.ConsumeBigItemRequest request
        )
		{
		    var task = new ConsumeBigItemTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
#endif


        public class ConsumeBigItemByUserIdTask : Gs2WebSocketSessionTask<Request.ConsumeBigItemByUserIdRequest, Result.ConsumeBigItemByUserIdResult>
        {
	        public ConsumeBigItemByUserIdTask(IGs2Session session, Request.ConsumeBigItemByUserIdRequest request) : base(session, request)
	        {
	        }

            protected override IGs2SessionRequest CreateRequest(Request.ConsumeBigItemByUserIdRequest request)
            {
                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);

                jsonWriter.WriteObjectStart();

                if (request.NamespaceName != null)
                {
                    jsonWriter.WritePropertyName("namespaceName");
                    jsonWriter.Write(request.NamespaceName.ToString());
                }
                if (request.InventoryName != null)
                {
                    jsonWriter.WritePropertyName("inventoryName");
                    jsonWriter.Write(request.InventoryName.ToString());
                }
                if (request.UserId != null)
                {
                    jsonWriter.WritePropertyName("userId");
                    jsonWriter.Write(request.UserId.ToString());
                }
                if (request.ItemName != null)
                {
                    jsonWriter.WritePropertyName("itemName");
                    jsonWriter.Write(request.ItemName.ToString());
                }
                if (request.ConsumeCount != null)
                {
                    jsonWriter.WritePropertyName("consumeCount");
                    jsonWriter.Write(request.ConsumeCount.ToString());
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
                    "inventory",
                    "bigItem",
                    "consumeBigItemByUserId",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }

            public override void OnError(Gs2.Core.Exception.Gs2Exception error)
            {
                if (error.Errors.Count(v => v.code == "itemSet.operation.conflict") > 0) {
                    base.OnError(new Exception.ConflictException(error));
                }
                else if (error.Errors.Count(v => v.code == "itemSet.count.insufficient") > 0) {
                    base.OnError(new Exception.InsufficientException(error));
                }
                else {
                    base.OnError(error);
                }
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator ConsumeBigItemByUserId(
                Request.ConsumeBigItemByUserIdRequest request,
                UnityAction<AsyncResult<Result.ConsumeBigItemByUserIdResult>> callback
        )
		{
			var task = new ConsumeBigItemByUserIdTask(
			    Gs2WebSocketSession,
			    request
            );
            yield return task;
            callback.Invoke(new AsyncResult<Result.ConsumeBigItemByUserIdResult>(task.Result, task.Error));
        }

		public IFuture<Result.ConsumeBigItemByUserIdResult> ConsumeBigItemByUserIdFuture(
                Request.ConsumeBigItemByUserIdRequest request
        )
		{
			return new ConsumeBigItemByUserIdTask(
			    Gs2WebSocketSession,
			    request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.ConsumeBigItemByUserIdResult> ConsumeBigItemByUserIdAsync(
            Request.ConsumeBigItemByUserIdRequest request
        )
		{
		    var task = new ConsumeBigItemByUserIdTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
    #else
		public ConsumeBigItemByUserIdTask ConsumeBigItemByUserIdAsync(
                Request.ConsumeBigItemByUserIdRequest request
        )
		{
			return new ConsumeBigItemByUserIdTask(
                Gs2WebSocketSession,
			    request
            );
        }
    #endif
#else
		public async Task<Result.ConsumeBigItemByUserIdResult> ConsumeBigItemByUserIdAsync(
            Request.ConsumeBigItemByUserIdRequest request
        )
		{
		    var task = new ConsumeBigItemByUserIdTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
#endif


        public class SetBigItemByUserIdTask : Gs2WebSocketSessionTask<Request.SetBigItemByUserIdRequest, Result.SetBigItemByUserIdResult>
        {
	        public SetBigItemByUserIdTask(IGs2Session session, Request.SetBigItemByUserIdRequest request) : base(session, request)
	        {
	        }

            protected override IGs2SessionRequest CreateRequest(Request.SetBigItemByUserIdRequest request)
            {
                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);

                jsonWriter.WriteObjectStart();

                if (request.NamespaceName != null)
                {
                    jsonWriter.WritePropertyName("namespaceName");
                    jsonWriter.Write(request.NamespaceName.ToString());
                }
                if (request.InventoryName != null)
                {
                    jsonWriter.WritePropertyName("inventoryName");
                    jsonWriter.Write(request.InventoryName.ToString());
                }
                if (request.UserId != null)
                {
                    jsonWriter.WritePropertyName("userId");
                    jsonWriter.Write(request.UserId.ToString());
                }
                if (request.ItemName != null)
                {
                    jsonWriter.WritePropertyName("itemName");
                    jsonWriter.Write(request.ItemName.ToString());
                }
                if (request.Count != null)
                {
                    jsonWriter.WritePropertyName("count");
                    jsonWriter.Write(request.Count.ToString());
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
                    "inventory",
                    "bigItem",
                    "setBigItemByUserId",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }

            public override void OnError(Gs2.Core.Exception.Gs2Exception error)
            {
                if (error.Errors.Count(v => v.code == "itemSet.operation.conflict") > 0) {
                    base.OnError(new Exception.ConflictException(error));
                }
                else {
                    base.OnError(error);
                }
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator SetBigItemByUserId(
                Request.SetBigItemByUserIdRequest request,
                UnityAction<AsyncResult<Result.SetBigItemByUserIdResult>> callback
        )
		{
			var task = new SetBigItemByUserIdTask(
			    Gs2WebSocketSession,
			    request
            );
            yield return task;
            callback.Invoke(new AsyncResult<Result.SetBigItemByUserIdResult>(task.Result, task.Error));
        }

		public IFuture<Result.SetBigItemByUserIdResult> SetBigItemByUserIdFuture(
                Request.SetBigItemByUserIdRequest request
        )
		{
			return new SetBigItemByUserIdTask(
			    Gs2WebSocketSession,
			    request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.SetBigItemByUserIdResult> SetBigItemByUserIdAsync(
            Request.SetBigItemByUserIdRequest request
        )
		{
		    var task = new SetBigItemByUserIdTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
    #else
		public SetBigItemByUserIdTask SetBigItemByUserIdAsync(
                Request.SetBigItemByUserIdRequest request
        )
		{
			return new SetBigItemByUserIdTask(
                Gs2WebSocketSession,
			    request
            );
        }
    #endif
#else
		public async Task<Result.SetBigItemByUserIdResult> SetBigItemByUserIdAsync(
            Request.SetBigItemByUserIdRequest request
        )
		{
		    var task = new SetBigItemByUserIdTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
#endif


        public class DeleteBigItemByUserIdTask : Gs2WebSocketSessionTask<Request.DeleteBigItemByUserIdRequest, Result.DeleteBigItemByUserIdResult>
        {
	        public DeleteBigItemByUserIdTask(IGs2Session session, Request.DeleteBigItemByUserIdRequest request) : base(session, request)
	        {
	        }

            protected override IGs2SessionRequest CreateRequest(Request.DeleteBigItemByUserIdRequest request)
            {
                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);

                jsonWriter.WriteObjectStart();

                if (request.NamespaceName != null)
                {
                    jsonWriter.WritePropertyName("namespaceName");
                    jsonWriter.Write(request.NamespaceName.ToString());
                }
                if (request.InventoryName != null)
                {
                    jsonWriter.WritePropertyName("inventoryName");
                    jsonWriter.Write(request.InventoryName.ToString());
                }
                if (request.UserId != null)
                {
                    jsonWriter.WritePropertyName("userId");
                    jsonWriter.Write(request.UserId.ToString());
                }
                if (request.ItemName != null)
                {
                    jsonWriter.WritePropertyName("itemName");
                    jsonWriter.Write(request.ItemName.ToString());
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
                    "inventory",
                    "bigItem",
                    "deleteBigItemByUserId",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator DeleteBigItemByUserId(
                Request.DeleteBigItemByUserIdRequest request,
                UnityAction<AsyncResult<Result.DeleteBigItemByUserIdResult>> callback
        )
		{
			var task = new DeleteBigItemByUserIdTask(
			    Gs2WebSocketSession,
			    request
            );
            yield return task;
            callback.Invoke(new AsyncResult<Result.DeleteBigItemByUserIdResult>(task.Result, task.Error));
        }

		public IFuture<Result.DeleteBigItemByUserIdResult> DeleteBigItemByUserIdFuture(
                Request.DeleteBigItemByUserIdRequest request
        )
		{
			return new DeleteBigItemByUserIdTask(
			    Gs2WebSocketSession,
			    request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.DeleteBigItemByUserIdResult> DeleteBigItemByUserIdAsync(
            Request.DeleteBigItemByUserIdRequest request
        )
		{
		    var task = new DeleteBigItemByUserIdTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
    #else
		public DeleteBigItemByUserIdTask DeleteBigItemByUserIdAsync(
                Request.DeleteBigItemByUserIdRequest request
        )
		{
			return new DeleteBigItemByUserIdTask(
                Gs2WebSocketSession,
			    request
            );
        }
    #endif
#else
		public async Task<Result.DeleteBigItemByUserIdResult> DeleteBigItemByUserIdAsync(
            Request.DeleteBigItemByUserIdRequest request
        )
		{
		    var task = new DeleteBigItemByUserIdTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
#endif


        public class VerifyBigItemTask : Gs2WebSocketSessionTask<Request.VerifyBigItemRequest, Result.VerifyBigItemResult>
        {
	        public VerifyBigItemTask(IGs2Session session, Request.VerifyBigItemRequest request) : base(session, request)
	        {
	        }

            protected override IGs2SessionRequest CreateRequest(Request.VerifyBigItemRequest request)
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
                if (request.InventoryName != null)
                {
                    jsonWriter.WritePropertyName("inventoryName");
                    jsonWriter.Write(request.InventoryName.ToString());
                }
                if (request.ItemName != null)
                {
                    jsonWriter.WritePropertyName("itemName");
                    jsonWriter.Write(request.ItemName.ToString());
                }
                if (request.VerifyType != null)
                {
                    jsonWriter.WritePropertyName("verifyType");
                    jsonWriter.Write(request.VerifyType.ToString());
                }
                if (request.Count != null)
                {
                    jsonWriter.WritePropertyName("count");
                    jsonWriter.Write(request.Count.ToString());
                }
                if (request.MultiplyValueSpecifyingQuantity != null)
                {
                    jsonWriter.WritePropertyName("multiplyValueSpecifyingQuantity");
                    jsonWriter.Write(request.MultiplyValueSpecifyingQuantity.ToString());
                }
                if (request.ContextStack != null)
                {
                    jsonWriter.WritePropertyName("contextStack");
                    jsonWriter.Write(request.ContextStack.ToString());
                }
                if (request.AccessToken != null)
                {
                    jsonWriter.WritePropertyName("xGs2AccessToken");
                    jsonWriter.Write(request.AccessToken);
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
                    "inventory",
                    "bigItem",
                    "verifyBigItem",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator VerifyBigItem(
                Request.VerifyBigItemRequest request,
                UnityAction<AsyncResult<Result.VerifyBigItemResult>> callback
        )
		{
			var task = new VerifyBigItemTask(
			    Gs2WebSocketSession,
			    request
            );
            yield return task;
            callback.Invoke(new AsyncResult<Result.VerifyBigItemResult>(task.Result, task.Error));
        }

		public IFuture<Result.VerifyBigItemResult> VerifyBigItemFuture(
                Request.VerifyBigItemRequest request
        )
		{
			return new VerifyBigItemTask(
			    Gs2WebSocketSession,
			    request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.VerifyBigItemResult> VerifyBigItemAsync(
            Request.VerifyBigItemRequest request
        )
		{
		    var task = new VerifyBigItemTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
    #else
		public VerifyBigItemTask VerifyBigItemAsync(
                Request.VerifyBigItemRequest request
        )
		{
			return new VerifyBigItemTask(
                Gs2WebSocketSession,
			    request
            );
        }
    #endif
#else
		public async Task<Result.VerifyBigItemResult> VerifyBigItemAsync(
            Request.VerifyBigItemRequest request
        )
		{
		    var task = new VerifyBigItemTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
#endif


        public class VerifyBigItemByUserIdTask : Gs2WebSocketSessionTask<Request.VerifyBigItemByUserIdRequest, Result.VerifyBigItemByUserIdResult>
        {
	        public VerifyBigItemByUserIdTask(IGs2Session session, Request.VerifyBigItemByUserIdRequest request) : base(session, request)
	        {
	        }

            protected override IGs2SessionRequest CreateRequest(Request.VerifyBigItemByUserIdRequest request)
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
                if (request.InventoryName != null)
                {
                    jsonWriter.WritePropertyName("inventoryName");
                    jsonWriter.Write(request.InventoryName.ToString());
                }
                if (request.ItemName != null)
                {
                    jsonWriter.WritePropertyName("itemName");
                    jsonWriter.Write(request.ItemName.ToString());
                }
                if (request.VerifyType != null)
                {
                    jsonWriter.WritePropertyName("verifyType");
                    jsonWriter.Write(request.VerifyType.ToString());
                }
                if (request.Count != null)
                {
                    jsonWriter.WritePropertyName("count");
                    jsonWriter.Write(request.Count.ToString());
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
                    "inventory",
                    "bigItem",
                    "verifyBigItemByUserId",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator VerifyBigItemByUserId(
                Request.VerifyBigItemByUserIdRequest request,
                UnityAction<AsyncResult<Result.VerifyBigItemByUserIdResult>> callback
        )
		{
			var task = new VerifyBigItemByUserIdTask(
			    Gs2WebSocketSession,
			    request
            );
            yield return task;
            callback.Invoke(new AsyncResult<Result.VerifyBigItemByUserIdResult>(task.Result, task.Error));
        }

		public IFuture<Result.VerifyBigItemByUserIdResult> VerifyBigItemByUserIdFuture(
                Request.VerifyBigItemByUserIdRequest request
        )
		{
			return new VerifyBigItemByUserIdTask(
			    Gs2WebSocketSession,
			    request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.VerifyBigItemByUserIdResult> VerifyBigItemByUserIdAsync(
            Request.VerifyBigItemByUserIdRequest request
        )
		{
		    var task = new VerifyBigItemByUserIdTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
    #else
		public VerifyBigItemByUserIdTask VerifyBigItemByUserIdAsync(
                Request.VerifyBigItemByUserIdRequest request
        )
		{
			return new VerifyBigItemByUserIdTask(
                Gs2WebSocketSession,
			    request
            );
        }
    #endif
#else
		public async Task<Result.VerifyBigItemByUserIdResult> VerifyBigItemByUserIdAsync(
            Request.VerifyBigItemByUserIdRequest request
        )
		{
		    var task = new VerifyBigItemByUserIdTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
#endif


        public class AcquireBigItemByStampSheetTask : Gs2WebSocketSessionTask<Request.AcquireBigItemByStampSheetRequest, Result.AcquireBigItemByStampSheetResult>
        {
	        public AcquireBigItemByStampSheetTask(IGs2Session session, Request.AcquireBigItemByStampSheetRequest request) : base(session, request)
	        {
	        }

            protected override IGs2SessionRequest CreateRequest(Request.AcquireBigItemByStampSheetRequest request)
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
                if (request.DryRun)
                {
                    jsonWriter.WritePropertyName("xGs2DryRun");
                    jsonWriter.Write("true");
                }

                AddHeader(
                    Session.Credential,
                    "inventory",
                    "bigItem",
                    "acquireBigItemByStampSheet",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator AcquireBigItemByStampSheet(
                Request.AcquireBigItemByStampSheetRequest request,
                UnityAction<AsyncResult<Result.AcquireBigItemByStampSheetResult>> callback
        )
		{
			var task = new AcquireBigItemByStampSheetTask(
			    Gs2WebSocketSession,
			    request
            );
            yield return task;
            callback.Invoke(new AsyncResult<Result.AcquireBigItemByStampSheetResult>(task.Result, task.Error));
        }

		public IFuture<Result.AcquireBigItemByStampSheetResult> AcquireBigItemByStampSheetFuture(
                Request.AcquireBigItemByStampSheetRequest request
        )
		{
			return new AcquireBigItemByStampSheetTask(
			    Gs2WebSocketSession,
			    request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.AcquireBigItemByStampSheetResult> AcquireBigItemByStampSheetAsync(
            Request.AcquireBigItemByStampSheetRequest request
        )
		{
		    var task = new AcquireBigItemByStampSheetTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
    #else
		public AcquireBigItemByStampSheetTask AcquireBigItemByStampSheetAsync(
                Request.AcquireBigItemByStampSheetRequest request
        )
		{
			return new AcquireBigItemByStampSheetTask(
                Gs2WebSocketSession,
			    request
            );
        }
    #endif
#else
		public async Task<Result.AcquireBigItemByStampSheetResult> AcquireBigItemByStampSheetAsync(
            Request.AcquireBigItemByStampSheetRequest request
        )
		{
		    var task = new AcquireBigItemByStampSheetTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
#endif


        public class SetBigItemByStampSheetTask : Gs2WebSocketSessionTask<Request.SetBigItemByStampSheetRequest, Result.SetBigItemByStampSheetResult>
        {
	        public SetBigItemByStampSheetTask(IGs2Session session, Request.SetBigItemByStampSheetRequest request) : base(session, request)
	        {
	        }

            protected override IGs2SessionRequest CreateRequest(Request.SetBigItemByStampSheetRequest request)
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
                if (request.DryRun)
                {
                    jsonWriter.WritePropertyName("xGs2DryRun");
                    jsonWriter.Write("true");
                }

                AddHeader(
                    Session.Credential,
                    "inventory",
                    "bigItem",
                    "setBigItemByStampSheet",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator SetBigItemByStampSheet(
                Request.SetBigItemByStampSheetRequest request,
                UnityAction<AsyncResult<Result.SetBigItemByStampSheetResult>> callback
        )
		{
			var task = new SetBigItemByStampSheetTask(
			    Gs2WebSocketSession,
			    request
            );
            yield return task;
            callback.Invoke(new AsyncResult<Result.SetBigItemByStampSheetResult>(task.Result, task.Error));
        }

		public IFuture<Result.SetBigItemByStampSheetResult> SetBigItemByStampSheetFuture(
                Request.SetBigItemByStampSheetRequest request
        )
		{
			return new SetBigItemByStampSheetTask(
			    Gs2WebSocketSession,
			    request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.SetBigItemByStampSheetResult> SetBigItemByStampSheetAsync(
            Request.SetBigItemByStampSheetRequest request
        )
		{
		    var task = new SetBigItemByStampSheetTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
    #else
		public SetBigItemByStampSheetTask SetBigItemByStampSheetAsync(
                Request.SetBigItemByStampSheetRequest request
        )
		{
			return new SetBigItemByStampSheetTask(
                Gs2WebSocketSession,
			    request
            );
        }
    #endif
#else
		public async Task<Result.SetBigItemByStampSheetResult> SetBigItemByStampSheetAsync(
            Request.SetBigItemByStampSheetRequest request
        )
		{
		    var task = new SetBigItemByStampSheetTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
#endif
	}
}