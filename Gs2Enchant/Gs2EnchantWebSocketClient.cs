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

namespace Gs2.Gs2Enchant
{
	public class Gs2EnchantWebSocketClient : AbstractGs2Client
	{

		public static string Endpoint = "enchant";

        protected Gs2WebSocketSession Gs2WebSocketSession => (Gs2WebSocketSession) Gs2Session;

		public Gs2EnchantWebSocketClient(Gs2WebSocketSession Gs2WebSocketSession) : base(Gs2WebSocketSession)
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
                    "enchant",
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
                    "enchant",
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
                    "enchant",
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
                    "enchant",
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
                    "enchant",
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
                    "enchant",
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
                    "enchant",
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
                    "enchant",
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
                    "enchant",
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
                    "enchant",
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


        public class GetBalanceParameterModelTask : Gs2WebSocketSessionTask<Request.GetBalanceParameterModelRequest, Result.GetBalanceParameterModelResult>
        {
	        public GetBalanceParameterModelTask(IGs2Session session, Request.GetBalanceParameterModelRequest request) : base(session, request)
	        {
	        }

            protected override IGs2SessionRequest CreateRequest(Request.GetBalanceParameterModelRequest request)
            {
                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);

                jsonWriter.WriteObjectStart();

                if (request.NamespaceName != null)
                {
                    jsonWriter.WritePropertyName("namespaceName");
                    jsonWriter.Write(request.NamespaceName.ToString());
                }
                if (request.ParameterName != null)
                {
                    jsonWriter.WritePropertyName("parameterName");
                    jsonWriter.Write(request.ParameterName.ToString());
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
                    "enchant",
                    "balanceParameterModel",
                    "getBalanceParameterModel",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator GetBalanceParameterModel(
                Request.GetBalanceParameterModelRequest request,
                UnityAction<AsyncResult<Result.GetBalanceParameterModelResult>> callback
        )
		{
			var task = new GetBalanceParameterModelTask(
			    Gs2WebSocketSession,
			    request
            );
            yield return task;
            callback.Invoke(new AsyncResult<Result.GetBalanceParameterModelResult>(task.Result, task.Error));
        }

		public IFuture<Result.GetBalanceParameterModelResult> GetBalanceParameterModelFuture(
                Request.GetBalanceParameterModelRequest request
        )
		{
			return new GetBalanceParameterModelTask(
			    Gs2WebSocketSession,
			    request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.GetBalanceParameterModelResult> GetBalanceParameterModelAsync(
            Request.GetBalanceParameterModelRequest request
        )
		{
		    var task = new GetBalanceParameterModelTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
    #else
		public GetBalanceParameterModelTask GetBalanceParameterModelAsync(
                Request.GetBalanceParameterModelRequest request
        )
		{
			return new GetBalanceParameterModelTask(
                Gs2WebSocketSession,
			    request
            );
        }
    #endif
#else
		public async Task<Result.GetBalanceParameterModelResult> GetBalanceParameterModelAsync(
            Request.GetBalanceParameterModelRequest request
        )
		{
		    var task = new GetBalanceParameterModelTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
#endif


        public class CreateBalanceParameterModelMasterTask : Gs2WebSocketSessionTask<Request.CreateBalanceParameterModelMasterRequest, Result.CreateBalanceParameterModelMasterResult>
        {
	        public CreateBalanceParameterModelMasterTask(IGs2Session session, Request.CreateBalanceParameterModelMasterRequest request) : base(session, request)
	        {
	        }

            protected override IGs2SessionRequest CreateRequest(Request.CreateBalanceParameterModelMasterRequest request)
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
                if (request.TotalValue != null)
                {
                    jsonWriter.WritePropertyName("totalValue");
                    jsonWriter.Write(request.TotalValue.ToString());
                }
                if (request.InitialValueStrategy != null)
                {
                    jsonWriter.WritePropertyName("initialValueStrategy");
                    jsonWriter.Write(request.InitialValueStrategy.ToString());
                }
                if (request.Parameters != null)
                {
                    jsonWriter.WritePropertyName("parameters");
                    jsonWriter.WriteArrayStart();
                    foreach(var item in request.Parameters)
                    {
                        item.WriteJson(jsonWriter);
                    }
                    jsonWriter.WriteArrayEnd();
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
                    "enchant",
                    "balanceParameterModelMaster",
                    "createBalanceParameterModelMaster",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator CreateBalanceParameterModelMaster(
                Request.CreateBalanceParameterModelMasterRequest request,
                UnityAction<AsyncResult<Result.CreateBalanceParameterModelMasterResult>> callback
        )
		{
			var task = new CreateBalanceParameterModelMasterTask(
			    Gs2WebSocketSession,
			    request
            );
            yield return task;
            callback.Invoke(new AsyncResult<Result.CreateBalanceParameterModelMasterResult>(task.Result, task.Error));
        }

		public IFuture<Result.CreateBalanceParameterModelMasterResult> CreateBalanceParameterModelMasterFuture(
                Request.CreateBalanceParameterModelMasterRequest request
        )
		{
			return new CreateBalanceParameterModelMasterTask(
			    Gs2WebSocketSession,
			    request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.CreateBalanceParameterModelMasterResult> CreateBalanceParameterModelMasterAsync(
            Request.CreateBalanceParameterModelMasterRequest request
        )
		{
		    var task = new CreateBalanceParameterModelMasterTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
    #else
		public CreateBalanceParameterModelMasterTask CreateBalanceParameterModelMasterAsync(
                Request.CreateBalanceParameterModelMasterRequest request
        )
		{
			return new CreateBalanceParameterModelMasterTask(
                Gs2WebSocketSession,
			    request
            );
        }
    #endif
#else
		public async Task<Result.CreateBalanceParameterModelMasterResult> CreateBalanceParameterModelMasterAsync(
            Request.CreateBalanceParameterModelMasterRequest request
        )
		{
		    var task = new CreateBalanceParameterModelMasterTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
#endif


        public class GetBalanceParameterModelMasterTask : Gs2WebSocketSessionTask<Request.GetBalanceParameterModelMasterRequest, Result.GetBalanceParameterModelMasterResult>
        {
	        public GetBalanceParameterModelMasterTask(IGs2Session session, Request.GetBalanceParameterModelMasterRequest request) : base(session, request)
	        {
	        }

            protected override IGs2SessionRequest CreateRequest(Request.GetBalanceParameterModelMasterRequest request)
            {
                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);

                jsonWriter.WriteObjectStart();

                if (request.NamespaceName != null)
                {
                    jsonWriter.WritePropertyName("namespaceName");
                    jsonWriter.Write(request.NamespaceName.ToString());
                }
                if (request.ParameterName != null)
                {
                    jsonWriter.WritePropertyName("parameterName");
                    jsonWriter.Write(request.ParameterName.ToString());
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
                    "enchant",
                    "balanceParameterModelMaster",
                    "getBalanceParameterModelMaster",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator GetBalanceParameterModelMaster(
                Request.GetBalanceParameterModelMasterRequest request,
                UnityAction<AsyncResult<Result.GetBalanceParameterModelMasterResult>> callback
        )
		{
			var task = new GetBalanceParameterModelMasterTask(
			    Gs2WebSocketSession,
			    request
            );
            yield return task;
            callback.Invoke(new AsyncResult<Result.GetBalanceParameterModelMasterResult>(task.Result, task.Error));
        }

		public IFuture<Result.GetBalanceParameterModelMasterResult> GetBalanceParameterModelMasterFuture(
                Request.GetBalanceParameterModelMasterRequest request
        )
		{
			return new GetBalanceParameterModelMasterTask(
			    Gs2WebSocketSession,
			    request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.GetBalanceParameterModelMasterResult> GetBalanceParameterModelMasterAsync(
            Request.GetBalanceParameterModelMasterRequest request
        )
		{
		    var task = new GetBalanceParameterModelMasterTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
    #else
		public GetBalanceParameterModelMasterTask GetBalanceParameterModelMasterAsync(
                Request.GetBalanceParameterModelMasterRequest request
        )
		{
			return new GetBalanceParameterModelMasterTask(
                Gs2WebSocketSession,
			    request
            );
        }
    #endif
#else
		public async Task<Result.GetBalanceParameterModelMasterResult> GetBalanceParameterModelMasterAsync(
            Request.GetBalanceParameterModelMasterRequest request
        )
		{
		    var task = new GetBalanceParameterModelMasterTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
#endif


        public class UpdateBalanceParameterModelMasterTask : Gs2WebSocketSessionTask<Request.UpdateBalanceParameterModelMasterRequest, Result.UpdateBalanceParameterModelMasterResult>
        {
	        public UpdateBalanceParameterModelMasterTask(IGs2Session session, Request.UpdateBalanceParameterModelMasterRequest request) : base(session, request)
	        {
	        }

            protected override IGs2SessionRequest CreateRequest(Request.UpdateBalanceParameterModelMasterRequest request)
            {
                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);

                jsonWriter.WriteObjectStart();

                if (request.NamespaceName != null)
                {
                    jsonWriter.WritePropertyName("namespaceName");
                    jsonWriter.Write(request.NamespaceName.ToString());
                }
                if (request.ParameterName != null)
                {
                    jsonWriter.WritePropertyName("parameterName");
                    jsonWriter.Write(request.ParameterName.ToString());
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
                if (request.TotalValue != null)
                {
                    jsonWriter.WritePropertyName("totalValue");
                    jsonWriter.Write(request.TotalValue.ToString());
                }
                if (request.InitialValueStrategy != null)
                {
                    jsonWriter.WritePropertyName("initialValueStrategy");
                    jsonWriter.Write(request.InitialValueStrategy.ToString());
                }
                if (request.Parameters != null)
                {
                    jsonWriter.WritePropertyName("parameters");
                    jsonWriter.WriteArrayStart();
                    foreach(var item in request.Parameters)
                    {
                        item.WriteJson(jsonWriter);
                    }
                    jsonWriter.WriteArrayEnd();
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
                    "enchant",
                    "balanceParameterModelMaster",
                    "updateBalanceParameterModelMaster",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator UpdateBalanceParameterModelMaster(
                Request.UpdateBalanceParameterModelMasterRequest request,
                UnityAction<AsyncResult<Result.UpdateBalanceParameterModelMasterResult>> callback
        )
		{
			var task = new UpdateBalanceParameterModelMasterTask(
			    Gs2WebSocketSession,
			    request
            );
            yield return task;
            callback.Invoke(new AsyncResult<Result.UpdateBalanceParameterModelMasterResult>(task.Result, task.Error));
        }

		public IFuture<Result.UpdateBalanceParameterModelMasterResult> UpdateBalanceParameterModelMasterFuture(
                Request.UpdateBalanceParameterModelMasterRequest request
        )
		{
			return new UpdateBalanceParameterModelMasterTask(
			    Gs2WebSocketSession,
			    request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.UpdateBalanceParameterModelMasterResult> UpdateBalanceParameterModelMasterAsync(
            Request.UpdateBalanceParameterModelMasterRequest request
        )
		{
		    var task = new UpdateBalanceParameterModelMasterTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
    #else
		public UpdateBalanceParameterModelMasterTask UpdateBalanceParameterModelMasterAsync(
                Request.UpdateBalanceParameterModelMasterRequest request
        )
		{
			return new UpdateBalanceParameterModelMasterTask(
                Gs2WebSocketSession,
			    request
            );
        }
    #endif
#else
		public async Task<Result.UpdateBalanceParameterModelMasterResult> UpdateBalanceParameterModelMasterAsync(
            Request.UpdateBalanceParameterModelMasterRequest request
        )
		{
		    var task = new UpdateBalanceParameterModelMasterTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
#endif


        public class DeleteBalanceParameterModelMasterTask : Gs2WebSocketSessionTask<Request.DeleteBalanceParameterModelMasterRequest, Result.DeleteBalanceParameterModelMasterResult>
        {
	        public DeleteBalanceParameterModelMasterTask(IGs2Session session, Request.DeleteBalanceParameterModelMasterRequest request) : base(session, request)
	        {
	        }

            protected override IGs2SessionRequest CreateRequest(Request.DeleteBalanceParameterModelMasterRequest request)
            {
                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);

                jsonWriter.WriteObjectStart();

                if (request.NamespaceName != null)
                {
                    jsonWriter.WritePropertyName("namespaceName");
                    jsonWriter.Write(request.NamespaceName.ToString());
                }
                if (request.ParameterName != null)
                {
                    jsonWriter.WritePropertyName("parameterName");
                    jsonWriter.Write(request.ParameterName.ToString());
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
                    "enchant",
                    "balanceParameterModelMaster",
                    "deleteBalanceParameterModelMaster",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator DeleteBalanceParameterModelMaster(
                Request.DeleteBalanceParameterModelMasterRequest request,
                UnityAction<AsyncResult<Result.DeleteBalanceParameterModelMasterResult>> callback
        )
		{
			var task = new DeleteBalanceParameterModelMasterTask(
			    Gs2WebSocketSession,
			    request
            );
            yield return task;
            callback.Invoke(new AsyncResult<Result.DeleteBalanceParameterModelMasterResult>(task.Result, task.Error));
        }

		public IFuture<Result.DeleteBalanceParameterModelMasterResult> DeleteBalanceParameterModelMasterFuture(
                Request.DeleteBalanceParameterModelMasterRequest request
        )
		{
			return new DeleteBalanceParameterModelMasterTask(
			    Gs2WebSocketSession,
			    request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.DeleteBalanceParameterModelMasterResult> DeleteBalanceParameterModelMasterAsync(
            Request.DeleteBalanceParameterModelMasterRequest request
        )
		{
		    var task = new DeleteBalanceParameterModelMasterTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
    #else
		public DeleteBalanceParameterModelMasterTask DeleteBalanceParameterModelMasterAsync(
                Request.DeleteBalanceParameterModelMasterRequest request
        )
		{
			return new DeleteBalanceParameterModelMasterTask(
                Gs2WebSocketSession,
			    request
            );
        }
    #endif
#else
		public async Task<Result.DeleteBalanceParameterModelMasterResult> DeleteBalanceParameterModelMasterAsync(
            Request.DeleteBalanceParameterModelMasterRequest request
        )
		{
		    var task = new DeleteBalanceParameterModelMasterTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
#endif


        public class GetBalanceParameterStatusTask : Gs2WebSocketSessionTask<Request.GetBalanceParameterStatusRequest, Result.GetBalanceParameterStatusResult>
        {
	        public GetBalanceParameterStatusTask(IGs2Session session, Request.GetBalanceParameterStatusRequest request) : base(session, request)
	        {
	        }

            protected override IGs2SessionRequest CreateRequest(Request.GetBalanceParameterStatusRequest request)
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
                if (request.ParameterName != null)
                {
                    jsonWriter.WritePropertyName("parameterName");
                    jsonWriter.Write(request.ParameterName.ToString());
                }
                if (request.PropertyId != null)
                {
                    jsonWriter.WritePropertyName("propertyId");
                    jsonWriter.Write(request.PropertyId.ToString());
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
                    "enchant",
                    "balanceParameterStatus",
                    "getBalanceParameterStatus",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator GetBalanceParameterStatus(
                Request.GetBalanceParameterStatusRequest request,
                UnityAction<AsyncResult<Result.GetBalanceParameterStatusResult>> callback
        )
		{
			var task = new GetBalanceParameterStatusTask(
			    Gs2WebSocketSession,
			    request
            );
            yield return task;
            callback.Invoke(new AsyncResult<Result.GetBalanceParameterStatusResult>(task.Result, task.Error));
        }

		public IFuture<Result.GetBalanceParameterStatusResult> GetBalanceParameterStatusFuture(
                Request.GetBalanceParameterStatusRequest request
        )
		{
			return new GetBalanceParameterStatusTask(
			    Gs2WebSocketSession,
			    request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.GetBalanceParameterStatusResult> GetBalanceParameterStatusAsync(
            Request.GetBalanceParameterStatusRequest request
        )
		{
		    var task = new GetBalanceParameterStatusTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
    #else
		public GetBalanceParameterStatusTask GetBalanceParameterStatusAsync(
                Request.GetBalanceParameterStatusRequest request
        )
		{
			return new GetBalanceParameterStatusTask(
                Gs2WebSocketSession,
			    request
            );
        }
    #endif
#else
		public async Task<Result.GetBalanceParameterStatusResult> GetBalanceParameterStatusAsync(
            Request.GetBalanceParameterStatusRequest request
        )
		{
		    var task = new GetBalanceParameterStatusTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
#endif


        public class GetBalanceParameterStatusByUserIdTask : Gs2WebSocketSessionTask<Request.GetBalanceParameterStatusByUserIdRequest, Result.GetBalanceParameterStatusByUserIdResult>
        {
	        public GetBalanceParameterStatusByUserIdTask(IGs2Session session, Request.GetBalanceParameterStatusByUserIdRequest request) : base(session, request)
	        {
	        }

            protected override IGs2SessionRequest CreateRequest(Request.GetBalanceParameterStatusByUserIdRequest request)
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
                if (request.ParameterName != null)
                {
                    jsonWriter.WritePropertyName("parameterName");
                    jsonWriter.Write(request.ParameterName.ToString());
                }
                if (request.PropertyId != null)
                {
                    jsonWriter.WritePropertyName("propertyId");
                    jsonWriter.Write(request.PropertyId.ToString());
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
                    "enchant",
                    "balanceParameterStatus",
                    "getBalanceParameterStatusByUserId",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator GetBalanceParameterStatusByUserId(
                Request.GetBalanceParameterStatusByUserIdRequest request,
                UnityAction<AsyncResult<Result.GetBalanceParameterStatusByUserIdResult>> callback
        )
		{
			var task = new GetBalanceParameterStatusByUserIdTask(
			    Gs2WebSocketSession,
			    request
            );
            yield return task;
            callback.Invoke(new AsyncResult<Result.GetBalanceParameterStatusByUserIdResult>(task.Result, task.Error));
        }

		public IFuture<Result.GetBalanceParameterStatusByUserIdResult> GetBalanceParameterStatusByUserIdFuture(
                Request.GetBalanceParameterStatusByUserIdRequest request
        )
		{
			return new GetBalanceParameterStatusByUserIdTask(
			    Gs2WebSocketSession,
			    request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.GetBalanceParameterStatusByUserIdResult> GetBalanceParameterStatusByUserIdAsync(
            Request.GetBalanceParameterStatusByUserIdRequest request
        )
		{
		    var task = new GetBalanceParameterStatusByUserIdTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
    #else
		public GetBalanceParameterStatusByUserIdTask GetBalanceParameterStatusByUserIdAsync(
                Request.GetBalanceParameterStatusByUserIdRequest request
        )
		{
			return new GetBalanceParameterStatusByUserIdTask(
                Gs2WebSocketSession,
			    request
            );
        }
    #endif
#else
		public async Task<Result.GetBalanceParameterStatusByUserIdResult> GetBalanceParameterStatusByUserIdAsync(
            Request.GetBalanceParameterStatusByUserIdRequest request
        )
		{
		    var task = new GetBalanceParameterStatusByUserIdTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
#endif


        public class DeleteBalanceParameterStatusByUserIdTask : Gs2WebSocketSessionTask<Request.DeleteBalanceParameterStatusByUserIdRequest, Result.DeleteBalanceParameterStatusByUserIdResult>
        {
	        public DeleteBalanceParameterStatusByUserIdTask(IGs2Session session, Request.DeleteBalanceParameterStatusByUserIdRequest request) : base(session, request)
	        {
	        }

            protected override IGs2SessionRequest CreateRequest(Request.DeleteBalanceParameterStatusByUserIdRequest request)
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
                if (request.ParameterName != null)
                {
                    jsonWriter.WritePropertyName("parameterName");
                    jsonWriter.Write(request.ParameterName.ToString());
                }
                if (request.PropertyId != null)
                {
                    jsonWriter.WritePropertyName("propertyId");
                    jsonWriter.Write(request.PropertyId.ToString());
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
                    "enchant",
                    "balanceParameterStatus",
                    "deleteBalanceParameterStatusByUserId",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator DeleteBalanceParameterStatusByUserId(
                Request.DeleteBalanceParameterStatusByUserIdRequest request,
                UnityAction<AsyncResult<Result.DeleteBalanceParameterStatusByUserIdResult>> callback
        )
		{
			var task = new DeleteBalanceParameterStatusByUserIdTask(
			    Gs2WebSocketSession,
			    request
            );
            yield return task;
            callback.Invoke(new AsyncResult<Result.DeleteBalanceParameterStatusByUserIdResult>(task.Result, task.Error));
        }

		public IFuture<Result.DeleteBalanceParameterStatusByUserIdResult> DeleteBalanceParameterStatusByUserIdFuture(
                Request.DeleteBalanceParameterStatusByUserIdRequest request
        )
		{
			return new DeleteBalanceParameterStatusByUserIdTask(
			    Gs2WebSocketSession,
			    request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.DeleteBalanceParameterStatusByUserIdResult> DeleteBalanceParameterStatusByUserIdAsync(
            Request.DeleteBalanceParameterStatusByUserIdRequest request
        )
		{
		    var task = new DeleteBalanceParameterStatusByUserIdTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
    #else
		public DeleteBalanceParameterStatusByUserIdTask DeleteBalanceParameterStatusByUserIdAsync(
                Request.DeleteBalanceParameterStatusByUserIdRequest request
        )
		{
			return new DeleteBalanceParameterStatusByUserIdTask(
                Gs2WebSocketSession,
			    request
            );
        }
    #endif
#else
		public async Task<Result.DeleteBalanceParameterStatusByUserIdResult> DeleteBalanceParameterStatusByUserIdAsync(
            Request.DeleteBalanceParameterStatusByUserIdRequest request
        )
		{
		    var task = new DeleteBalanceParameterStatusByUserIdTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
#endif


        public class ReDrawBalanceParameterStatusByUserIdTask : Gs2WebSocketSessionTask<Request.ReDrawBalanceParameterStatusByUserIdRequest, Result.ReDrawBalanceParameterStatusByUserIdResult>
        {
	        public ReDrawBalanceParameterStatusByUserIdTask(IGs2Session session, Request.ReDrawBalanceParameterStatusByUserIdRequest request) : base(session, request)
	        {
	        }

            protected override IGs2SessionRequest CreateRequest(Request.ReDrawBalanceParameterStatusByUserIdRequest request)
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
                if (request.ParameterName != null)
                {
                    jsonWriter.WritePropertyName("parameterName");
                    jsonWriter.Write(request.ParameterName.ToString());
                }
                if (request.PropertyId != null)
                {
                    jsonWriter.WritePropertyName("propertyId");
                    jsonWriter.Write(request.PropertyId.ToString());
                }
                if (request.FixedParameterNames != null)
                {
                    jsonWriter.WritePropertyName("fixedParameterNames");
                    jsonWriter.WriteArrayStart();
                    foreach(var item in request.FixedParameterNames)
                    {
                        jsonWriter.Write(item);
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
                    "enchant",
                    "balanceParameterStatus",
                    "reDrawBalanceParameterStatusByUserId",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator ReDrawBalanceParameterStatusByUserId(
                Request.ReDrawBalanceParameterStatusByUserIdRequest request,
                UnityAction<AsyncResult<Result.ReDrawBalanceParameterStatusByUserIdResult>> callback
        )
		{
			var task = new ReDrawBalanceParameterStatusByUserIdTask(
			    Gs2WebSocketSession,
			    request
            );
            yield return task;
            callback.Invoke(new AsyncResult<Result.ReDrawBalanceParameterStatusByUserIdResult>(task.Result, task.Error));
        }

		public IFuture<Result.ReDrawBalanceParameterStatusByUserIdResult> ReDrawBalanceParameterStatusByUserIdFuture(
                Request.ReDrawBalanceParameterStatusByUserIdRequest request
        )
		{
			return new ReDrawBalanceParameterStatusByUserIdTask(
			    Gs2WebSocketSession,
			    request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.ReDrawBalanceParameterStatusByUserIdResult> ReDrawBalanceParameterStatusByUserIdAsync(
            Request.ReDrawBalanceParameterStatusByUserIdRequest request
        )
		{
		    var task = new ReDrawBalanceParameterStatusByUserIdTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
    #else
		public ReDrawBalanceParameterStatusByUserIdTask ReDrawBalanceParameterStatusByUserIdAsync(
                Request.ReDrawBalanceParameterStatusByUserIdRequest request
        )
		{
			return new ReDrawBalanceParameterStatusByUserIdTask(
                Gs2WebSocketSession,
			    request
            );
        }
    #endif
#else
		public async Task<Result.ReDrawBalanceParameterStatusByUserIdResult> ReDrawBalanceParameterStatusByUserIdAsync(
            Request.ReDrawBalanceParameterStatusByUserIdRequest request
        )
		{
		    var task = new ReDrawBalanceParameterStatusByUserIdTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
#endif


        public class ReDrawBalanceParameterStatusByStampSheetTask : Gs2WebSocketSessionTask<Request.ReDrawBalanceParameterStatusByStampSheetRequest, Result.ReDrawBalanceParameterStatusByStampSheetResult>
        {
	        public ReDrawBalanceParameterStatusByStampSheetTask(IGs2Session session, Request.ReDrawBalanceParameterStatusByStampSheetRequest request) : base(session, request)
	        {
	        }

            protected override IGs2SessionRequest CreateRequest(Request.ReDrawBalanceParameterStatusByStampSheetRequest request)
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
                    "enchant",
                    "balanceParameterStatus",
                    "reDrawBalanceParameterStatusByStampSheet",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator ReDrawBalanceParameterStatusByStampSheet(
                Request.ReDrawBalanceParameterStatusByStampSheetRequest request,
                UnityAction<AsyncResult<Result.ReDrawBalanceParameterStatusByStampSheetResult>> callback
        )
		{
			var task = new ReDrawBalanceParameterStatusByStampSheetTask(
			    Gs2WebSocketSession,
			    request
            );
            yield return task;
            callback.Invoke(new AsyncResult<Result.ReDrawBalanceParameterStatusByStampSheetResult>(task.Result, task.Error));
        }

		public IFuture<Result.ReDrawBalanceParameterStatusByStampSheetResult> ReDrawBalanceParameterStatusByStampSheetFuture(
                Request.ReDrawBalanceParameterStatusByStampSheetRequest request
        )
		{
			return new ReDrawBalanceParameterStatusByStampSheetTask(
			    Gs2WebSocketSession,
			    request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.ReDrawBalanceParameterStatusByStampSheetResult> ReDrawBalanceParameterStatusByStampSheetAsync(
            Request.ReDrawBalanceParameterStatusByStampSheetRequest request
        )
		{
		    var task = new ReDrawBalanceParameterStatusByStampSheetTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
    #else
		public ReDrawBalanceParameterStatusByStampSheetTask ReDrawBalanceParameterStatusByStampSheetAsync(
                Request.ReDrawBalanceParameterStatusByStampSheetRequest request
        )
		{
			return new ReDrawBalanceParameterStatusByStampSheetTask(
                Gs2WebSocketSession,
			    request
            );
        }
    #endif
#else
		public async Task<Result.ReDrawBalanceParameterStatusByStampSheetResult> ReDrawBalanceParameterStatusByStampSheetAsync(
            Request.ReDrawBalanceParameterStatusByStampSheetRequest request
        )
		{
		    var task = new ReDrawBalanceParameterStatusByStampSheetTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
#endif


        public class SetBalanceParameterStatusByUserIdTask : Gs2WebSocketSessionTask<Request.SetBalanceParameterStatusByUserIdRequest, Result.SetBalanceParameterStatusByUserIdResult>
        {
	        public SetBalanceParameterStatusByUserIdTask(IGs2Session session, Request.SetBalanceParameterStatusByUserIdRequest request) : base(session, request)
	        {
	        }

            protected override IGs2SessionRequest CreateRequest(Request.SetBalanceParameterStatusByUserIdRequest request)
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
                if (request.ParameterName != null)
                {
                    jsonWriter.WritePropertyName("parameterName");
                    jsonWriter.Write(request.ParameterName.ToString());
                }
                if (request.PropertyId != null)
                {
                    jsonWriter.WritePropertyName("propertyId");
                    jsonWriter.Write(request.PropertyId.ToString());
                }
                if (request.ParameterValues != null)
                {
                    jsonWriter.WritePropertyName("parameterValues");
                    jsonWriter.WriteArrayStart();
                    foreach(var item in request.ParameterValues)
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
                    "enchant",
                    "balanceParameterStatus",
                    "setBalanceParameterStatusByUserId",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator SetBalanceParameterStatusByUserId(
                Request.SetBalanceParameterStatusByUserIdRequest request,
                UnityAction<AsyncResult<Result.SetBalanceParameterStatusByUserIdResult>> callback
        )
		{
			var task = new SetBalanceParameterStatusByUserIdTask(
			    Gs2WebSocketSession,
			    request
            );
            yield return task;
            callback.Invoke(new AsyncResult<Result.SetBalanceParameterStatusByUserIdResult>(task.Result, task.Error));
        }

		public IFuture<Result.SetBalanceParameterStatusByUserIdResult> SetBalanceParameterStatusByUserIdFuture(
                Request.SetBalanceParameterStatusByUserIdRequest request
        )
		{
			return new SetBalanceParameterStatusByUserIdTask(
			    Gs2WebSocketSession,
			    request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.SetBalanceParameterStatusByUserIdResult> SetBalanceParameterStatusByUserIdAsync(
            Request.SetBalanceParameterStatusByUserIdRequest request
        )
		{
		    var task = new SetBalanceParameterStatusByUserIdTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
    #else
		public SetBalanceParameterStatusByUserIdTask SetBalanceParameterStatusByUserIdAsync(
                Request.SetBalanceParameterStatusByUserIdRequest request
        )
		{
			return new SetBalanceParameterStatusByUserIdTask(
                Gs2WebSocketSession,
			    request
            );
        }
    #endif
#else
		public async Task<Result.SetBalanceParameterStatusByUserIdResult> SetBalanceParameterStatusByUserIdAsync(
            Request.SetBalanceParameterStatusByUserIdRequest request
        )
		{
		    var task = new SetBalanceParameterStatusByUserIdTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
#endif


        public class SetBalanceParameterStatusByStampSheetTask : Gs2WebSocketSessionTask<Request.SetBalanceParameterStatusByStampSheetRequest, Result.SetBalanceParameterStatusByStampSheetResult>
        {
	        public SetBalanceParameterStatusByStampSheetTask(IGs2Session session, Request.SetBalanceParameterStatusByStampSheetRequest request) : base(session, request)
	        {
	        }

            protected override IGs2SessionRequest CreateRequest(Request.SetBalanceParameterStatusByStampSheetRequest request)
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
                    "enchant",
                    "balanceParameterStatus",
                    "setBalanceParameterStatusByStampSheet",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator SetBalanceParameterStatusByStampSheet(
                Request.SetBalanceParameterStatusByStampSheetRequest request,
                UnityAction<AsyncResult<Result.SetBalanceParameterStatusByStampSheetResult>> callback
        )
		{
			var task = new SetBalanceParameterStatusByStampSheetTask(
			    Gs2WebSocketSession,
			    request
            );
            yield return task;
            callback.Invoke(new AsyncResult<Result.SetBalanceParameterStatusByStampSheetResult>(task.Result, task.Error));
        }

		public IFuture<Result.SetBalanceParameterStatusByStampSheetResult> SetBalanceParameterStatusByStampSheetFuture(
                Request.SetBalanceParameterStatusByStampSheetRequest request
        )
		{
			return new SetBalanceParameterStatusByStampSheetTask(
			    Gs2WebSocketSession,
			    request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.SetBalanceParameterStatusByStampSheetResult> SetBalanceParameterStatusByStampSheetAsync(
            Request.SetBalanceParameterStatusByStampSheetRequest request
        )
		{
		    var task = new SetBalanceParameterStatusByStampSheetTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
    #else
		public SetBalanceParameterStatusByStampSheetTask SetBalanceParameterStatusByStampSheetAsync(
                Request.SetBalanceParameterStatusByStampSheetRequest request
        )
		{
			return new SetBalanceParameterStatusByStampSheetTask(
                Gs2WebSocketSession,
			    request
            );
        }
    #endif
#else
		public async Task<Result.SetBalanceParameterStatusByStampSheetResult> SetBalanceParameterStatusByStampSheetAsync(
            Request.SetBalanceParameterStatusByStampSheetRequest request
        )
		{
		    var task = new SetBalanceParameterStatusByStampSheetTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
#endif


        public class GetRarityParameterStatusTask : Gs2WebSocketSessionTask<Request.GetRarityParameterStatusRequest, Result.GetRarityParameterStatusResult>
        {
	        public GetRarityParameterStatusTask(IGs2Session session, Request.GetRarityParameterStatusRequest request) : base(session, request)
	        {
	        }

            protected override IGs2SessionRequest CreateRequest(Request.GetRarityParameterStatusRequest request)
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
                if (request.ParameterName != null)
                {
                    jsonWriter.WritePropertyName("parameterName");
                    jsonWriter.Write(request.ParameterName.ToString());
                }
                if (request.PropertyId != null)
                {
                    jsonWriter.WritePropertyName("propertyId");
                    jsonWriter.Write(request.PropertyId.ToString());
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
                    "enchant",
                    "rarityParameterStatus",
                    "getRarityParameterStatus",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator GetRarityParameterStatus(
                Request.GetRarityParameterStatusRequest request,
                UnityAction<AsyncResult<Result.GetRarityParameterStatusResult>> callback
        )
		{
			var task = new GetRarityParameterStatusTask(
			    Gs2WebSocketSession,
			    request
            );
            yield return task;
            callback.Invoke(new AsyncResult<Result.GetRarityParameterStatusResult>(task.Result, task.Error));
        }

		public IFuture<Result.GetRarityParameterStatusResult> GetRarityParameterStatusFuture(
                Request.GetRarityParameterStatusRequest request
        )
		{
			return new GetRarityParameterStatusTask(
			    Gs2WebSocketSession,
			    request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.GetRarityParameterStatusResult> GetRarityParameterStatusAsync(
            Request.GetRarityParameterStatusRequest request
        )
		{
		    var task = new GetRarityParameterStatusTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
    #else
		public GetRarityParameterStatusTask GetRarityParameterStatusAsync(
                Request.GetRarityParameterStatusRequest request
        )
		{
			return new GetRarityParameterStatusTask(
                Gs2WebSocketSession,
			    request
            );
        }
    #endif
#else
		public async Task<Result.GetRarityParameterStatusResult> GetRarityParameterStatusAsync(
            Request.GetRarityParameterStatusRequest request
        )
		{
		    var task = new GetRarityParameterStatusTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
#endif


        public class GetRarityParameterStatusByUserIdTask : Gs2WebSocketSessionTask<Request.GetRarityParameterStatusByUserIdRequest, Result.GetRarityParameterStatusByUserIdResult>
        {
	        public GetRarityParameterStatusByUserIdTask(IGs2Session session, Request.GetRarityParameterStatusByUserIdRequest request) : base(session, request)
	        {
	        }

            protected override IGs2SessionRequest CreateRequest(Request.GetRarityParameterStatusByUserIdRequest request)
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
                if (request.ParameterName != null)
                {
                    jsonWriter.WritePropertyName("parameterName");
                    jsonWriter.Write(request.ParameterName.ToString());
                }
                if (request.PropertyId != null)
                {
                    jsonWriter.WritePropertyName("propertyId");
                    jsonWriter.Write(request.PropertyId.ToString());
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
                    "enchant",
                    "rarityParameterStatus",
                    "getRarityParameterStatusByUserId",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator GetRarityParameterStatusByUserId(
                Request.GetRarityParameterStatusByUserIdRequest request,
                UnityAction<AsyncResult<Result.GetRarityParameterStatusByUserIdResult>> callback
        )
		{
			var task = new GetRarityParameterStatusByUserIdTask(
			    Gs2WebSocketSession,
			    request
            );
            yield return task;
            callback.Invoke(new AsyncResult<Result.GetRarityParameterStatusByUserIdResult>(task.Result, task.Error));
        }

		public IFuture<Result.GetRarityParameterStatusByUserIdResult> GetRarityParameterStatusByUserIdFuture(
                Request.GetRarityParameterStatusByUserIdRequest request
        )
		{
			return new GetRarityParameterStatusByUserIdTask(
			    Gs2WebSocketSession,
			    request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.GetRarityParameterStatusByUserIdResult> GetRarityParameterStatusByUserIdAsync(
            Request.GetRarityParameterStatusByUserIdRequest request
        )
		{
		    var task = new GetRarityParameterStatusByUserIdTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
    #else
		public GetRarityParameterStatusByUserIdTask GetRarityParameterStatusByUserIdAsync(
                Request.GetRarityParameterStatusByUserIdRequest request
        )
		{
			return new GetRarityParameterStatusByUserIdTask(
                Gs2WebSocketSession,
			    request
            );
        }
    #endif
#else
		public async Task<Result.GetRarityParameterStatusByUserIdResult> GetRarityParameterStatusByUserIdAsync(
            Request.GetRarityParameterStatusByUserIdRequest request
        )
		{
		    var task = new GetRarityParameterStatusByUserIdTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
#endif


        public class DeleteRarityParameterStatusByUserIdTask : Gs2WebSocketSessionTask<Request.DeleteRarityParameterStatusByUserIdRequest, Result.DeleteRarityParameterStatusByUserIdResult>
        {
	        public DeleteRarityParameterStatusByUserIdTask(IGs2Session session, Request.DeleteRarityParameterStatusByUserIdRequest request) : base(session, request)
	        {
	        }

            protected override IGs2SessionRequest CreateRequest(Request.DeleteRarityParameterStatusByUserIdRequest request)
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
                if (request.ParameterName != null)
                {
                    jsonWriter.WritePropertyName("parameterName");
                    jsonWriter.Write(request.ParameterName.ToString());
                }
                if (request.PropertyId != null)
                {
                    jsonWriter.WritePropertyName("propertyId");
                    jsonWriter.Write(request.PropertyId.ToString());
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
                    "enchant",
                    "rarityParameterStatus",
                    "deleteRarityParameterStatusByUserId",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator DeleteRarityParameterStatusByUserId(
                Request.DeleteRarityParameterStatusByUserIdRequest request,
                UnityAction<AsyncResult<Result.DeleteRarityParameterStatusByUserIdResult>> callback
        )
		{
			var task = new DeleteRarityParameterStatusByUserIdTask(
			    Gs2WebSocketSession,
			    request
            );
            yield return task;
            callback.Invoke(new AsyncResult<Result.DeleteRarityParameterStatusByUserIdResult>(task.Result, task.Error));
        }

		public IFuture<Result.DeleteRarityParameterStatusByUserIdResult> DeleteRarityParameterStatusByUserIdFuture(
                Request.DeleteRarityParameterStatusByUserIdRequest request
        )
		{
			return new DeleteRarityParameterStatusByUserIdTask(
			    Gs2WebSocketSession,
			    request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.DeleteRarityParameterStatusByUserIdResult> DeleteRarityParameterStatusByUserIdAsync(
            Request.DeleteRarityParameterStatusByUserIdRequest request
        )
		{
		    var task = new DeleteRarityParameterStatusByUserIdTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
    #else
		public DeleteRarityParameterStatusByUserIdTask DeleteRarityParameterStatusByUserIdAsync(
                Request.DeleteRarityParameterStatusByUserIdRequest request
        )
		{
			return new DeleteRarityParameterStatusByUserIdTask(
                Gs2WebSocketSession,
			    request
            );
        }
    #endif
#else
		public async Task<Result.DeleteRarityParameterStatusByUserIdResult> DeleteRarityParameterStatusByUserIdAsync(
            Request.DeleteRarityParameterStatusByUserIdRequest request
        )
		{
		    var task = new DeleteRarityParameterStatusByUserIdTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
#endif


        public class ReDrawRarityParameterStatusByUserIdTask : Gs2WebSocketSessionTask<Request.ReDrawRarityParameterStatusByUserIdRequest, Result.ReDrawRarityParameterStatusByUserIdResult>
        {
	        public ReDrawRarityParameterStatusByUserIdTask(IGs2Session session, Request.ReDrawRarityParameterStatusByUserIdRequest request) : base(session, request)
	        {
	        }

            protected override IGs2SessionRequest CreateRequest(Request.ReDrawRarityParameterStatusByUserIdRequest request)
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
                if (request.ParameterName != null)
                {
                    jsonWriter.WritePropertyName("parameterName");
                    jsonWriter.Write(request.ParameterName.ToString());
                }
                if (request.PropertyId != null)
                {
                    jsonWriter.WritePropertyName("propertyId");
                    jsonWriter.Write(request.PropertyId.ToString());
                }
                if (request.FixedParameterNames != null)
                {
                    jsonWriter.WritePropertyName("fixedParameterNames");
                    jsonWriter.WriteArrayStart();
                    foreach(var item in request.FixedParameterNames)
                    {
                        jsonWriter.Write(item);
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
                    "enchant",
                    "rarityParameterStatus",
                    "reDrawRarityParameterStatusByUserId",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator ReDrawRarityParameterStatusByUserId(
                Request.ReDrawRarityParameterStatusByUserIdRequest request,
                UnityAction<AsyncResult<Result.ReDrawRarityParameterStatusByUserIdResult>> callback
        )
		{
			var task = new ReDrawRarityParameterStatusByUserIdTask(
			    Gs2WebSocketSession,
			    request
            );
            yield return task;
            callback.Invoke(new AsyncResult<Result.ReDrawRarityParameterStatusByUserIdResult>(task.Result, task.Error));
        }

		public IFuture<Result.ReDrawRarityParameterStatusByUserIdResult> ReDrawRarityParameterStatusByUserIdFuture(
                Request.ReDrawRarityParameterStatusByUserIdRequest request
        )
		{
			return new ReDrawRarityParameterStatusByUserIdTask(
			    Gs2WebSocketSession,
			    request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.ReDrawRarityParameterStatusByUserIdResult> ReDrawRarityParameterStatusByUserIdAsync(
            Request.ReDrawRarityParameterStatusByUserIdRequest request
        )
		{
		    var task = new ReDrawRarityParameterStatusByUserIdTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
    #else
		public ReDrawRarityParameterStatusByUserIdTask ReDrawRarityParameterStatusByUserIdAsync(
                Request.ReDrawRarityParameterStatusByUserIdRequest request
        )
		{
			return new ReDrawRarityParameterStatusByUserIdTask(
                Gs2WebSocketSession,
			    request
            );
        }
    #endif
#else
		public async Task<Result.ReDrawRarityParameterStatusByUserIdResult> ReDrawRarityParameterStatusByUserIdAsync(
            Request.ReDrawRarityParameterStatusByUserIdRequest request
        )
		{
		    var task = new ReDrawRarityParameterStatusByUserIdTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
#endif


        public class ReDrawRarityParameterStatusByStampSheetTask : Gs2WebSocketSessionTask<Request.ReDrawRarityParameterStatusByStampSheetRequest, Result.ReDrawRarityParameterStatusByStampSheetResult>
        {
	        public ReDrawRarityParameterStatusByStampSheetTask(IGs2Session session, Request.ReDrawRarityParameterStatusByStampSheetRequest request) : base(session, request)
	        {
	        }

            protected override IGs2SessionRequest CreateRequest(Request.ReDrawRarityParameterStatusByStampSheetRequest request)
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
                    "enchant",
                    "rarityParameterStatus",
                    "reDrawRarityParameterStatusByStampSheet",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator ReDrawRarityParameterStatusByStampSheet(
                Request.ReDrawRarityParameterStatusByStampSheetRequest request,
                UnityAction<AsyncResult<Result.ReDrawRarityParameterStatusByStampSheetResult>> callback
        )
		{
			var task = new ReDrawRarityParameterStatusByStampSheetTask(
			    Gs2WebSocketSession,
			    request
            );
            yield return task;
            callback.Invoke(new AsyncResult<Result.ReDrawRarityParameterStatusByStampSheetResult>(task.Result, task.Error));
        }

		public IFuture<Result.ReDrawRarityParameterStatusByStampSheetResult> ReDrawRarityParameterStatusByStampSheetFuture(
                Request.ReDrawRarityParameterStatusByStampSheetRequest request
        )
		{
			return new ReDrawRarityParameterStatusByStampSheetTask(
			    Gs2WebSocketSession,
			    request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.ReDrawRarityParameterStatusByStampSheetResult> ReDrawRarityParameterStatusByStampSheetAsync(
            Request.ReDrawRarityParameterStatusByStampSheetRequest request
        )
		{
		    var task = new ReDrawRarityParameterStatusByStampSheetTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
    #else
		public ReDrawRarityParameterStatusByStampSheetTask ReDrawRarityParameterStatusByStampSheetAsync(
                Request.ReDrawRarityParameterStatusByStampSheetRequest request
        )
		{
			return new ReDrawRarityParameterStatusByStampSheetTask(
                Gs2WebSocketSession,
			    request
            );
        }
    #endif
#else
		public async Task<Result.ReDrawRarityParameterStatusByStampSheetResult> ReDrawRarityParameterStatusByStampSheetAsync(
            Request.ReDrawRarityParameterStatusByStampSheetRequest request
        )
		{
		    var task = new ReDrawRarityParameterStatusByStampSheetTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
#endif


        public class AddRarityParameterStatusByUserIdTask : Gs2WebSocketSessionTask<Request.AddRarityParameterStatusByUserIdRequest, Result.AddRarityParameterStatusByUserIdResult>
        {
	        public AddRarityParameterStatusByUserIdTask(IGs2Session session, Request.AddRarityParameterStatusByUserIdRequest request) : base(session, request)
	        {
	        }

            protected override IGs2SessionRequest CreateRequest(Request.AddRarityParameterStatusByUserIdRequest request)
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
                if (request.ParameterName != null)
                {
                    jsonWriter.WritePropertyName("parameterName");
                    jsonWriter.Write(request.ParameterName.ToString());
                }
                if (request.PropertyId != null)
                {
                    jsonWriter.WritePropertyName("propertyId");
                    jsonWriter.Write(request.PropertyId.ToString());
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
                    "enchant",
                    "rarityParameterStatus",
                    "addRarityParameterStatusByUserId",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator AddRarityParameterStatusByUserId(
                Request.AddRarityParameterStatusByUserIdRequest request,
                UnityAction<AsyncResult<Result.AddRarityParameterStatusByUserIdResult>> callback
        )
		{
			var task = new AddRarityParameterStatusByUserIdTask(
			    Gs2WebSocketSession,
			    request
            );
            yield return task;
            callback.Invoke(new AsyncResult<Result.AddRarityParameterStatusByUserIdResult>(task.Result, task.Error));
        }

		public IFuture<Result.AddRarityParameterStatusByUserIdResult> AddRarityParameterStatusByUserIdFuture(
                Request.AddRarityParameterStatusByUserIdRequest request
        )
		{
			return new AddRarityParameterStatusByUserIdTask(
			    Gs2WebSocketSession,
			    request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.AddRarityParameterStatusByUserIdResult> AddRarityParameterStatusByUserIdAsync(
            Request.AddRarityParameterStatusByUserIdRequest request
        )
		{
		    var task = new AddRarityParameterStatusByUserIdTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
    #else
		public AddRarityParameterStatusByUserIdTask AddRarityParameterStatusByUserIdAsync(
                Request.AddRarityParameterStatusByUserIdRequest request
        )
		{
			return new AddRarityParameterStatusByUserIdTask(
                Gs2WebSocketSession,
			    request
            );
        }
    #endif
#else
		public async Task<Result.AddRarityParameterStatusByUserIdResult> AddRarityParameterStatusByUserIdAsync(
            Request.AddRarityParameterStatusByUserIdRequest request
        )
		{
		    var task = new AddRarityParameterStatusByUserIdTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
#endif


        public class AddRarityParameterStatusByStampSheetTask : Gs2WebSocketSessionTask<Request.AddRarityParameterStatusByStampSheetRequest, Result.AddRarityParameterStatusByStampSheetResult>
        {
	        public AddRarityParameterStatusByStampSheetTask(IGs2Session session, Request.AddRarityParameterStatusByStampSheetRequest request) : base(session, request)
	        {
	        }

            protected override IGs2SessionRequest CreateRequest(Request.AddRarityParameterStatusByStampSheetRequest request)
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
                    "enchant",
                    "rarityParameterStatus",
                    "addRarityParameterStatusByStampSheet",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator AddRarityParameterStatusByStampSheet(
                Request.AddRarityParameterStatusByStampSheetRequest request,
                UnityAction<AsyncResult<Result.AddRarityParameterStatusByStampSheetResult>> callback
        )
		{
			var task = new AddRarityParameterStatusByStampSheetTask(
			    Gs2WebSocketSession,
			    request
            );
            yield return task;
            callback.Invoke(new AsyncResult<Result.AddRarityParameterStatusByStampSheetResult>(task.Result, task.Error));
        }

		public IFuture<Result.AddRarityParameterStatusByStampSheetResult> AddRarityParameterStatusByStampSheetFuture(
                Request.AddRarityParameterStatusByStampSheetRequest request
        )
		{
			return new AddRarityParameterStatusByStampSheetTask(
			    Gs2WebSocketSession,
			    request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.AddRarityParameterStatusByStampSheetResult> AddRarityParameterStatusByStampSheetAsync(
            Request.AddRarityParameterStatusByStampSheetRequest request
        )
		{
		    var task = new AddRarityParameterStatusByStampSheetTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
    #else
		public AddRarityParameterStatusByStampSheetTask AddRarityParameterStatusByStampSheetAsync(
                Request.AddRarityParameterStatusByStampSheetRequest request
        )
		{
			return new AddRarityParameterStatusByStampSheetTask(
                Gs2WebSocketSession,
			    request
            );
        }
    #endif
#else
		public async Task<Result.AddRarityParameterStatusByStampSheetResult> AddRarityParameterStatusByStampSheetAsync(
            Request.AddRarityParameterStatusByStampSheetRequest request
        )
		{
		    var task = new AddRarityParameterStatusByStampSheetTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
#endif


        public class VerifyRarityParameterStatusTask : Gs2WebSocketSessionTask<Request.VerifyRarityParameterStatusRequest, Result.VerifyRarityParameterStatusResult>
        {
	        public VerifyRarityParameterStatusTask(IGs2Session session, Request.VerifyRarityParameterStatusRequest request) : base(session, request)
	        {
	        }

            protected override IGs2SessionRequest CreateRequest(Request.VerifyRarityParameterStatusRequest request)
            {
                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);

                jsonWriter.WriteObjectStart();

                if (request.NamespaceName != null)
                {
                    jsonWriter.WritePropertyName("namespaceName");
                    jsonWriter.Write(request.NamespaceName.ToString());
                }
                if (request.ParameterName != null)
                {
                    jsonWriter.WritePropertyName("parameterName");
                    jsonWriter.Write(request.ParameterName.ToString());
                }
                if (request.AccessToken != null)
                {
                    jsonWriter.WritePropertyName("accessToken");
                    jsonWriter.Write(request.AccessToken.ToString());
                }
                if (request.PropertyId != null)
                {
                    jsonWriter.WritePropertyName("propertyId");
                    jsonWriter.Write(request.PropertyId.ToString());
                }
                if (request.VerifyType != null)
                {
                    jsonWriter.WritePropertyName("verifyType");
                    jsonWriter.Write(request.VerifyType.ToString());
                }
                if (request.ParameterValueName != null)
                {
                    jsonWriter.WritePropertyName("parameterValueName");
                    jsonWriter.Write(request.ParameterValueName.ToString());
                }
                if (request.ParameterCount != null)
                {
                    jsonWriter.WritePropertyName("parameterCount");
                    jsonWriter.Write(request.ParameterCount.ToString());
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
                    "enchant",
                    "rarityParameterStatus",
                    "verifyRarityParameterStatus",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator VerifyRarityParameterStatus(
                Request.VerifyRarityParameterStatusRequest request,
                UnityAction<AsyncResult<Result.VerifyRarityParameterStatusResult>> callback
        )
		{
			var task = new VerifyRarityParameterStatusTask(
			    Gs2WebSocketSession,
			    request
            );
            yield return task;
            callback.Invoke(new AsyncResult<Result.VerifyRarityParameterStatusResult>(task.Result, task.Error));
        }

		public IFuture<Result.VerifyRarityParameterStatusResult> VerifyRarityParameterStatusFuture(
                Request.VerifyRarityParameterStatusRequest request
        )
		{
			return new VerifyRarityParameterStatusTask(
			    Gs2WebSocketSession,
			    request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.VerifyRarityParameterStatusResult> VerifyRarityParameterStatusAsync(
            Request.VerifyRarityParameterStatusRequest request
        )
		{
		    var task = new VerifyRarityParameterStatusTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
    #else
		public VerifyRarityParameterStatusTask VerifyRarityParameterStatusAsync(
                Request.VerifyRarityParameterStatusRequest request
        )
		{
			return new VerifyRarityParameterStatusTask(
                Gs2WebSocketSession,
			    request
            );
        }
    #endif
#else
		public async Task<Result.VerifyRarityParameterStatusResult> VerifyRarityParameterStatusAsync(
            Request.VerifyRarityParameterStatusRequest request
        )
		{
		    var task = new VerifyRarityParameterStatusTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
#endif


        public class VerifyRarityParameterStatusByUserIdTask : Gs2WebSocketSessionTask<Request.VerifyRarityParameterStatusByUserIdRequest, Result.VerifyRarityParameterStatusByUserIdResult>
        {
	        public VerifyRarityParameterStatusByUserIdTask(IGs2Session session, Request.VerifyRarityParameterStatusByUserIdRequest request) : base(session, request)
	        {
	        }

            protected override IGs2SessionRequest CreateRequest(Request.VerifyRarityParameterStatusByUserIdRequest request)
            {
                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);

                jsonWriter.WriteObjectStart();

                if (request.NamespaceName != null)
                {
                    jsonWriter.WritePropertyName("namespaceName");
                    jsonWriter.Write(request.NamespaceName.ToString());
                }
                if (request.ParameterName != null)
                {
                    jsonWriter.WritePropertyName("parameterName");
                    jsonWriter.Write(request.ParameterName.ToString());
                }
                if (request.UserId != null)
                {
                    jsonWriter.WritePropertyName("userId");
                    jsonWriter.Write(request.UserId.ToString());
                }
                if (request.PropertyId != null)
                {
                    jsonWriter.WritePropertyName("propertyId");
                    jsonWriter.Write(request.PropertyId.ToString());
                }
                if (request.VerifyType != null)
                {
                    jsonWriter.WritePropertyName("verifyType");
                    jsonWriter.Write(request.VerifyType.ToString());
                }
                if (request.ParameterValueName != null)
                {
                    jsonWriter.WritePropertyName("parameterValueName");
                    jsonWriter.Write(request.ParameterValueName.ToString());
                }
                if (request.ParameterCount != null)
                {
                    jsonWriter.WritePropertyName("parameterCount");
                    jsonWriter.Write(request.ParameterCount.ToString());
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
                    "enchant",
                    "rarityParameterStatus",
                    "verifyRarityParameterStatusByUserId",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator VerifyRarityParameterStatusByUserId(
                Request.VerifyRarityParameterStatusByUserIdRequest request,
                UnityAction<AsyncResult<Result.VerifyRarityParameterStatusByUserIdResult>> callback
        )
		{
			var task = new VerifyRarityParameterStatusByUserIdTask(
			    Gs2WebSocketSession,
			    request
            );
            yield return task;
            callback.Invoke(new AsyncResult<Result.VerifyRarityParameterStatusByUserIdResult>(task.Result, task.Error));
        }

		public IFuture<Result.VerifyRarityParameterStatusByUserIdResult> VerifyRarityParameterStatusByUserIdFuture(
                Request.VerifyRarityParameterStatusByUserIdRequest request
        )
		{
			return new VerifyRarityParameterStatusByUserIdTask(
			    Gs2WebSocketSession,
			    request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.VerifyRarityParameterStatusByUserIdResult> VerifyRarityParameterStatusByUserIdAsync(
            Request.VerifyRarityParameterStatusByUserIdRequest request
        )
		{
		    var task = new VerifyRarityParameterStatusByUserIdTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
    #else
		public VerifyRarityParameterStatusByUserIdTask VerifyRarityParameterStatusByUserIdAsync(
                Request.VerifyRarityParameterStatusByUserIdRequest request
        )
		{
			return new VerifyRarityParameterStatusByUserIdTask(
                Gs2WebSocketSession,
			    request
            );
        }
    #endif
#else
		public async Task<Result.VerifyRarityParameterStatusByUserIdResult> VerifyRarityParameterStatusByUserIdAsync(
            Request.VerifyRarityParameterStatusByUserIdRequest request
        )
		{
		    var task = new VerifyRarityParameterStatusByUserIdTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
#endif


        public class SetRarityParameterStatusByUserIdTask : Gs2WebSocketSessionTask<Request.SetRarityParameterStatusByUserIdRequest, Result.SetRarityParameterStatusByUserIdResult>
        {
	        public SetRarityParameterStatusByUserIdTask(IGs2Session session, Request.SetRarityParameterStatusByUserIdRequest request) : base(session, request)
	        {
	        }

            protected override IGs2SessionRequest CreateRequest(Request.SetRarityParameterStatusByUserIdRequest request)
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
                if (request.ParameterName != null)
                {
                    jsonWriter.WritePropertyName("parameterName");
                    jsonWriter.Write(request.ParameterName.ToString());
                }
                if (request.PropertyId != null)
                {
                    jsonWriter.WritePropertyName("propertyId");
                    jsonWriter.Write(request.PropertyId.ToString());
                }
                if (request.ParameterValues != null)
                {
                    jsonWriter.WritePropertyName("parameterValues");
                    jsonWriter.WriteArrayStart();
                    foreach(var item in request.ParameterValues)
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
                    "enchant",
                    "rarityParameterStatus",
                    "setRarityParameterStatusByUserId",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator SetRarityParameterStatusByUserId(
                Request.SetRarityParameterStatusByUserIdRequest request,
                UnityAction<AsyncResult<Result.SetRarityParameterStatusByUserIdResult>> callback
        )
		{
			var task = new SetRarityParameterStatusByUserIdTask(
			    Gs2WebSocketSession,
			    request
            );
            yield return task;
            callback.Invoke(new AsyncResult<Result.SetRarityParameterStatusByUserIdResult>(task.Result, task.Error));
        }

		public IFuture<Result.SetRarityParameterStatusByUserIdResult> SetRarityParameterStatusByUserIdFuture(
                Request.SetRarityParameterStatusByUserIdRequest request
        )
		{
			return new SetRarityParameterStatusByUserIdTask(
			    Gs2WebSocketSession,
			    request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.SetRarityParameterStatusByUserIdResult> SetRarityParameterStatusByUserIdAsync(
            Request.SetRarityParameterStatusByUserIdRequest request
        )
		{
		    var task = new SetRarityParameterStatusByUserIdTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
    #else
		public SetRarityParameterStatusByUserIdTask SetRarityParameterStatusByUserIdAsync(
                Request.SetRarityParameterStatusByUserIdRequest request
        )
		{
			return new SetRarityParameterStatusByUserIdTask(
                Gs2WebSocketSession,
			    request
            );
        }
    #endif
#else
		public async Task<Result.SetRarityParameterStatusByUserIdResult> SetRarityParameterStatusByUserIdAsync(
            Request.SetRarityParameterStatusByUserIdRequest request
        )
		{
		    var task = new SetRarityParameterStatusByUserIdTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
#endif


        public class SetRarityParameterStatusByStampSheetTask : Gs2WebSocketSessionTask<Request.SetRarityParameterStatusByStampSheetRequest, Result.SetRarityParameterStatusByStampSheetResult>
        {
	        public SetRarityParameterStatusByStampSheetTask(IGs2Session session, Request.SetRarityParameterStatusByStampSheetRequest request) : base(session, request)
	        {
	        }

            protected override IGs2SessionRequest CreateRequest(Request.SetRarityParameterStatusByStampSheetRequest request)
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
                    "enchant",
                    "rarityParameterStatus",
                    "setRarityParameterStatusByStampSheet",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator SetRarityParameterStatusByStampSheet(
                Request.SetRarityParameterStatusByStampSheetRequest request,
                UnityAction<AsyncResult<Result.SetRarityParameterStatusByStampSheetResult>> callback
        )
		{
			var task = new SetRarityParameterStatusByStampSheetTask(
			    Gs2WebSocketSession,
			    request
            );
            yield return task;
            callback.Invoke(new AsyncResult<Result.SetRarityParameterStatusByStampSheetResult>(task.Result, task.Error));
        }

		public IFuture<Result.SetRarityParameterStatusByStampSheetResult> SetRarityParameterStatusByStampSheetFuture(
                Request.SetRarityParameterStatusByStampSheetRequest request
        )
		{
			return new SetRarityParameterStatusByStampSheetTask(
			    Gs2WebSocketSession,
			    request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.SetRarityParameterStatusByStampSheetResult> SetRarityParameterStatusByStampSheetAsync(
            Request.SetRarityParameterStatusByStampSheetRequest request
        )
		{
		    var task = new SetRarityParameterStatusByStampSheetTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
    #else
		public SetRarityParameterStatusByStampSheetTask SetRarityParameterStatusByStampSheetAsync(
                Request.SetRarityParameterStatusByStampSheetRequest request
        )
		{
			return new SetRarityParameterStatusByStampSheetTask(
                Gs2WebSocketSession,
			    request
            );
        }
    #endif
#else
		public async Task<Result.SetRarityParameterStatusByStampSheetResult> SetRarityParameterStatusByStampSheetAsync(
            Request.SetRarityParameterStatusByStampSheetRequest request
        )
		{
		    var task = new SetRarityParameterStatusByStampSheetTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
#endif
	}
}