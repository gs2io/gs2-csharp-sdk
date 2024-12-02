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

namespace Gs2.Gs2Ranking2
{
	public class Gs2Ranking2WebSocketClient : AbstractGs2Client
	{

		public static string Endpoint = "ranking2";

        protected Gs2WebSocketSession Gs2WebSocketSession => (Gs2WebSocketSession) Gs2Session;

		public Gs2Ranking2WebSocketClient(Gs2WebSocketSession Gs2WebSocketSession) : base(Gs2WebSocketSession)
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
                    "ranking2",
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
                    "ranking2",
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
                    "ranking2",
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
                    "ranking2",
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
                    "ranking2",
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
                    "ranking2",
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
                    "ranking2",
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
                    "ranking2",
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
                    "ranking2",
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
                    "ranking2",
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


        public class PutGlobalRankingScoreTask : Gs2WebSocketSessionTask<Request.PutGlobalRankingScoreRequest, Result.PutGlobalRankingScoreResult>
        {
	        public PutGlobalRankingScoreTask(IGs2Session session, Request.PutGlobalRankingScoreRequest request) : base(session, request)
	        {
	        }

            protected override IGs2SessionRequest CreateRequest(Request.PutGlobalRankingScoreRequest request)
            {
                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);

                jsonWriter.WriteObjectStart();

                if (request.NamespaceName != null)
                {
                    jsonWriter.WritePropertyName("namespaceName");
                    jsonWriter.Write(request.NamespaceName.ToString());
                }
                if (request.RankingName != null)
                {
                    jsonWriter.WritePropertyName("rankingName");
                    jsonWriter.Write(request.RankingName.ToString());
                }
                if (request.AccessToken != null)
                {
                    jsonWriter.WritePropertyName("accessToken");
                    jsonWriter.Write(request.AccessToken.ToString());
                }
                if (request.Score != null)
                {
                    jsonWriter.WritePropertyName("score");
                    jsonWriter.Write(request.Score.ToString());
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
                    "ranking2",
                    "globalRankingScore",
                    "putGlobalRankingScore",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator PutGlobalRankingScore(
                Request.PutGlobalRankingScoreRequest request,
                UnityAction<AsyncResult<Result.PutGlobalRankingScoreResult>> callback
        )
		{
			var task = new PutGlobalRankingScoreTask(
			    Gs2WebSocketSession,
			    request
            );
            yield return task;
            callback.Invoke(new AsyncResult<Result.PutGlobalRankingScoreResult>(task.Result, task.Error));
        }

		public IFuture<Result.PutGlobalRankingScoreResult> PutGlobalRankingScoreFuture(
                Request.PutGlobalRankingScoreRequest request
        )
		{
			return new PutGlobalRankingScoreTask(
			    Gs2WebSocketSession,
			    request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.PutGlobalRankingScoreResult> PutGlobalRankingScoreAsync(
            Request.PutGlobalRankingScoreRequest request
        )
		{
		    var task = new PutGlobalRankingScoreTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
    #else
		public PutGlobalRankingScoreTask PutGlobalRankingScoreAsync(
                Request.PutGlobalRankingScoreRequest request
        )
		{
			return new PutGlobalRankingScoreTask(
                Gs2WebSocketSession,
			    request
            );
        }
    #endif
#else
		public async Task<Result.PutGlobalRankingScoreResult> PutGlobalRankingScoreAsync(
            Request.PutGlobalRankingScoreRequest request
        )
		{
		    var task = new PutGlobalRankingScoreTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
#endif


        public class PutGlobalRankingScoreByUserIdTask : Gs2WebSocketSessionTask<Request.PutGlobalRankingScoreByUserIdRequest, Result.PutGlobalRankingScoreByUserIdResult>
        {
	        public PutGlobalRankingScoreByUserIdTask(IGs2Session session, Request.PutGlobalRankingScoreByUserIdRequest request) : base(session, request)
	        {
	        }

            protected override IGs2SessionRequest CreateRequest(Request.PutGlobalRankingScoreByUserIdRequest request)
            {
                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);

                jsonWriter.WriteObjectStart();

                if (request.NamespaceName != null)
                {
                    jsonWriter.WritePropertyName("namespaceName");
                    jsonWriter.Write(request.NamespaceName.ToString());
                }
                if (request.RankingName != null)
                {
                    jsonWriter.WritePropertyName("rankingName");
                    jsonWriter.Write(request.RankingName.ToString());
                }
                if (request.UserId != null)
                {
                    jsonWriter.WritePropertyName("userId");
                    jsonWriter.Write(request.UserId.ToString());
                }
                if (request.Score != null)
                {
                    jsonWriter.WritePropertyName("score");
                    jsonWriter.Write(request.Score.ToString());
                }
                if (request.Metadata != null)
                {
                    jsonWriter.WritePropertyName("metadata");
                    jsonWriter.Write(request.Metadata.ToString());
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
                    "ranking2",
                    "globalRankingScore",
                    "putGlobalRankingScoreByUserId",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator PutGlobalRankingScoreByUserId(
                Request.PutGlobalRankingScoreByUserIdRequest request,
                UnityAction<AsyncResult<Result.PutGlobalRankingScoreByUserIdResult>> callback
        )
		{
			var task = new PutGlobalRankingScoreByUserIdTask(
			    Gs2WebSocketSession,
			    request
            );
            yield return task;
            callback.Invoke(new AsyncResult<Result.PutGlobalRankingScoreByUserIdResult>(task.Result, task.Error));
        }

		public IFuture<Result.PutGlobalRankingScoreByUserIdResult> PutGlobalRankingScoreByUserIdFuture(
                Request.PutGlobalRankingScoreByUserIdRequest request
        )
		{
			return new PutGlobalRankingScoreByUserIdTask(
			    Gs2WebSocketSession,
			    request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.PutGlobalRankingScoreByUserIdResult> PutGlobalRankingScoreByUserIdAsync(
            Request.PutGlobalRankingScoreByUserIdRequest request
        )
		{
		    var task = new PutGlobalRankingScoreByUserIdTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
    #else
		public PutGlobalRankingScoreByUserIdTask PutGlobalRankingScoreByUserIdAsync(
                Request.PutGlobalRankingScoreByUserIdRequest request
        )
		{
			return new PutGlobalRankingScoreByUserIdTask(
                Gs2WebSocketSession,
			    request
            );
        }
    #endif
#else
		public async Task<Result.PutGlobalRankingScoreByUserIdResult> PutGlobalRankingScoreByUserIdAsync(
            Request.PutGlobalRankingScoreByUserIdRequest request
        )
		{
		    var task = new PutGlobalRankingScoreByUserIdTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
#endif


        public class GetGlobalRankingScoreTask : Gs2WebSocketSessionTask<Request.GetGlobalRankingScoreRequest, Result.GetGlobalRankingScoreResult>
        {
	        public GetGlobalRankingScoreTask(IGs2Session session, Request.GetGlobalRankingScoreRequest request) : base(session, request)
	        {
	        }

            protected override IGs2SessionRequest CreateRequest(Request.GetGlobalRankingScoreRequest request)
            {
                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);

                jsonWriter.WriteObjectStart();

                if (request.NamespaceName != null)
                {
                    jsonWriter.WritePropertyName("namespaceName");
                    jsonWriter.Write(request.NamespaceName.ToString());
                }
                if (request.RankingName != null)
                {
                    jsonWriter.WritePropertyName("rankingName");
                    jsonWriter.Write(request.RankingName.ToString());
                }
                if (request.AccessToken != null)
                {
                    jsonWriter.WritePropertyName("accessToken");
                    jsonWriter.Write(request.AccessToken.ToString());
                }
                if (request.Season != null)
                {
                    jsonWriter.WritePropertyName("season");
                    jsonWriter.Write(request.Season.ToString());
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
                    "ranking2",
                    "globalRankingScore",
                    "getGlobalRankingScore",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator GetGlobalRankingScore(
                Request.GetGlobalRankingScoreRequest request,
                UnityAction<AsyncResult<Result.GetGlobalRankingScoreResult>> callback
        )
		{
			var task = new GetGlobalRankingScoreTask(
			    Gs2WebSocketSession,
			    request
            );
            yield return task;
            callback.Invoke(new AsyncResult<Result.GetGlobalRankingScoreResult>(task.Result, task.Error));
        }

		public IFuture<Result.GetGlobalRankingScoreResult> GetGlobalRankingScoreFuture(
                Request.GetGlobalRankingScoreRequest request
        )
		{
			return new GetGlobalRankingScoreTask(
			    Gs2WebSocketSession,
			    request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.GetGlobalRankingScoreResult> GetGlobalRankingScoreAsync(
            Request.GetGlobalRankingScoreRequest request
        )
		{
		    var task = new GetGlobalRankingScoreTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
    #else
		public GetGlobalRankingScoreTask GetGlobalRankingScoreAsync(
                Request.GetGlobalRankingScoreRequest request
        )
		{
			return new GetGlobalRankingScoreTask(
                Gs2WebSocketSession,
			    request
            );
        }
    #endif
#else
		public async Task<Result.GetGlobalRankingScoreResult> GetGlobalRankingScoreAsync(
            Request.GetGlobalRankingScoreRequest request
        )
		{
		    var task = new GetGlobalRankingScoreTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
#endif


        public class GetGlobalRankingScoreByUserIdTask : Gs2WebSocketSessionTask<Request.GetGlobalRankingScoreByUserIdRequest, Result.GetGlobalRankingScoreByUserIdResult>
        {
	        public GetGlobalRankingScoreByUserIdTask(IGs2Session session, Request.GetGlobalRankingScoreByUserIdRequest request) : base(session, request)
	        {
	        }

            protected override IGs2SessionRequest CreateRequest(Request.GetGlobalRankingScoreByUserIdRequest request)
            {
                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);

                jsonWriter.WriteObjectStart();

                if (request.NamespaceName != null)
                {
                    jsonWriter.WritePropertyName("namespaceName");
                    jsonWriter.Write(request.NamespaceName.ToString());
                }
                if (request.RankingName != null)
                {
                    jsonWriter.WritePropertyName("rankingName");
                    jsonWriter.Write(request.RankingName.ToString());
                }
                if (request.UserId != null)
                {
                    jsonWriter.WritePropertyName("userId");
                    jsonWriter.Write(request.UserId.ToString());
                }
                if (request.Season != null)
                {
                    jsonWriter.WritePropertyName("season");
                    jsonWriter.Write(request.Season.ToString());
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
                    "ranking2",
                    "globalRankingScore",
                    "getGlobalRankingScoreByUserId",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator GetGlobalRankingScoreByUserId(
                Request.GetGlobalRankingScoreByUserIdRequest request,
                UnityAction<AsyncResult<Result.GetGlobalRankingScoreByUserIdResult>> callback
        )
		{
			var task = new GetGlobalRankingScoreByUserIdTask(
			    Gs2WebSocketSession,
			    request
            );
            yield return task;
            callback.Invoke(new AsyncResult<Result.GetGlobalRankingScoreByUserIdResult>(task.Result, task.Error));
        }

		public IFuture<Result.GetGlobalRankingScoreByUserIdResult> GetGlobalRankingScoreByUserIdFuture(
                Request.GetGlobalRankingScoreByUserIdRequest request
        )
		{
			return new GetGlobalRankingScoreByUserIdTask(
			    Gs2WebSocketSession,
			    request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.GetGlobalRankingScoreByUserIdResult> GetGlobalRankingScoreByUserIdAsync(
            Request.GetGlobalRankingScoreByUserIdRequest request
        )
		{
		    var task = new GetGlobalRankingScoreByUserIdTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
    #else
		public GetGlobalRankingScoreByUserIdTask GetGlobalRankingScoreByUserIdAsync(
                Request.GetGlobalRankingScoreByUserIdRequest request
        )
		{
			return new GetGlobalRankingScoreByUserIdTask(
                Gs2WebSocketSession,
			    request
            );
        }
    #endif
#else
		public async Task<Result.GetGlobalRankingScoreByUserIdResult> GetGlobalRankingScoreByUserIdAsync(
            Request.GetGlobalRankingScoreByUserIdRequest request
        )
		{
		    var task = new GetGlobalRankingScoreByUserIdTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
#endif


        public class DeleteGlobalRankingScoreByUserIdTask : Gs2WebSocketSessionTask<Request.DeleteGlobalRankingScoreByUserIdRequest, Result.DeleteGlobalRankingScoreByUserIdResult>
        {
	        public DeleteGlobalRankingScoreByUserIdTask(IGs2Session session, Request.DeleteGlobalRankingScoreByUserIdRequest request) : base(session, request)
	        {
	        }

            protected override IGs2SessionRequest CreateRequest(Request.DeleteGlobalRankingScoreByUserIdRequest request)
            {
                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);

                jsonWriter.WriteObjectStart();

                if (request.NamespaceName != null)
                {
                    jsonWriter.WritePropertyName("namespaceName");
                    jsonWriter.Write(request.NamespaceName.ToString());
                }
                if (request.RankingName != null)
                {
                    jsonWriter.WritePropertyName("rankingName");
                    jsonWriter.Write(request.RankingName.ToString());
                }
                if (request.UserId != null)
                {
                    jsonWriter.WritePropertyName("userId");
                    jsonWriter.Write(request.UserId.ToString());
                }
                if (request.Season != null)
                {
                    jsonWriter.WritePropertyName("season");
                    jsonWriter.Write(request.Season.ToString());
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
                    "ranking2",
                    "globalRankingScore",
                    "deleteGlobalRankingScoreByUserId",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator DeleteGlobalRankingScoreByUserId(
                Request.DeleteGlobalRankingScoreByUserIdRequest request,
                UnityAction<AsyncResult<Result.DeleteGlobalRankingScoreByUserIdResult>> callback
        )
		{
			var task = new DeleteGlobalRankingScoreByUserIdTask(
			    Gs2WebSocketSession,
			    request
            );
            yield return task;
            callback.Invoke(new AsyncResult<Result.DeleteGlobalRankingScoreByUserIdResult>(task.Result, task.Error));
        }

		public IFuture<Result.DeleteGlobalRankingScoreByUserIdResult> DeleteGlobalRankingScoreByUserIdFuture(
                Request.DeleteGlobalRankingScoreByUserIdRequest request
        )
		{
			return new DeleteGlobalRankingScoreByUserIdTask(
			    Gs2WebSocketSession,
			    request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.DeleteGlobalRankingScoreByUserIdResult> DeleteGlobalRankingScoreByUserIdAsync(
            Request.DeleteGlobalRankingScoreByUserIdRequest request
        )
		{
		    var task = new DeleteGlobalRankingScoreByUserIdTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
    #else
		public DeleteGlobalRankingScoreByUserIdTask DeleteGlobalRankingScoreByUserIdAsync(
                Request.DeleteGlobalRankingScoreByUserIdRequest request
        )
		{
			return new DeleteGlobalRankingScoreByUserIdTask(
                Gs2WebSocketSession,
			    request
            );
        }
    #endif
#else
		public async Task<Result.DeleteGlobalRankingScoreByUserIdResult> DeleteGlobalRankingScoreByUserIdAsync(
            Request.DeleteGlobalRankingScoreByUserIdRequest request
        )
		{
		    var task = new DeleteGlobalRankingScoreByUserIdTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
#endif


        public class VerifyGlobalRankingScoreTask : Gs2WebSocketSessionTask<Request.VerifyGlobalRankingScoreRequest, Result.VerifyGlobalRankingScoreResult>
        {
	        public VerifyGlobalRankingScoreTask(IGs2Session session, Request.VerifyGlobalRankingScoreRequest request) : base(session, request)
	        {
	        }

            protected override IGs2SessionRequest CreateRequest(Request.VerifyGlobalRankingScoreRequest request)
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
                if (request.RankingName != null)
                {
                    jsonWriter.WritePropertyName("rankingName");
                    jsonWriter.Write(request.RankingName.ToString());
                }
                if (request.VerifyType != null)
                {
                    jsonWriter.WritePropertyName("verifyType");
                    jsonWriter.Write(request.VerifyType.ToString());
                }
                if (request.Season != null)
                {
                    jsonWriter.WritePropertyName("season");
                    jsonWriter.Write(request.Season.ToString());
                }
                if (request.Score != null)
                {
                    jsonWriter.WritePropertyName("score");
                    jsonWriter.Write(request.Score.ToString());
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
                    "ranking2",
                    "globalRankingScore",
                    "verifyGlobalRankingScore",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator VerifyGlobalRankingScore(
                Request.VerifyGlobalRankingScoreRequest request,
                UnityAction<AsyncResult<Result.VerifyGlobalRankingScoreResult>> callback
        )
		{
			var task = new VerifyGlobalRankingScoreTask(
			    Gs2WebSocketSession,
			    request
            );
            yield return task;
            callback.Invoke(new AsyncResult<Result.VerifyGlobalRankingScoreResult>(task.Result, task.Error));
        }

		public IFuture<Result.VerifyGlobalRankingScoreResult> VerifyGlobalRankingScoreFuture(
                Request.VerifyGlobalRankingScoreRequest request
        )
		{
			return new VerifyGlobalRankingScoreTask(
			    Gs2WebSocketSession,
			    request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.VerifyGlobalRankingScoreResult> VerifyGlobalRankingScoreAsync(
            Request.VerifyGlobalRankingScoreRequest request
        )
		{
		    var task = new VerifyGlobalRankingScoreTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
    #else
		public VerifyGlobalRankingScoreTask VerifyGlobalRankingScoreAsync(
                Request.VerifyGlobalRankingScoreRequest request
        )
		{
			return new VerifyGlobalRankingScoreTask(
                Gs2WebSocketSession,
			    request
            );
        }
    #endif
#else
		public async Task<Result.VerifyGlobalRankingScoreResult> VerifyGlobalRankingScoreAsync(
            Request.VerifyGlobalRankingScoreRequest request
        )
		{
		    var task = new VerifyGlobalRankingScoreTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
#endif


        public class VerifyGlobalRankingScoreByUserIdTask : Gs2WebSocketSessionTask<Request.VerifyGlobalRankingScoreByUserIdRequest, Result.VerifyGlobalRankingScoreByUserIdResult>
        {
	        public VerifyGlobalRankingScoreByUserIdTask(IGs2Session session, Request.VerifyGlobalRankingScoreByUserIdRequest request) : base(session, request)
	        {
	        }

            protected override IGs2SessionRequest CreateRequest(Request.VerifyGlobalRankingScoreByUserIdRequest request)
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
                if (request.RankingName != null)
                {
                    jsonWriter.WritePropertyName("rankingName");
                    jsonWriter.Write(request.RankingName.ToString());
                }
                if (request.VerifyType != null)
                {
                    jsonWriter.WritePropertyName("verifyType");
                    jsonWriter.Write(request.VerifyType.ToString());
                }
                if (request.Season != null)
                {
                    jsonWriter.WritePropertyName("season");
                    jsonWriter.Write(request.Season.ToString());
                }
                if (request.Score != null)
                {
                    jsonWriter.WritePropertyName("score");
                    jsonWriter.Write(request.Score.ToString());
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
                    "ranking2",
                    "globalRankingScore",
                    "verifyGlobalRankingScoreByUserId",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator VerifyGlobalRankingScoreByUserId(
                Request.VerifyGlobalRankingScoreByUserIdRequest request,
                UnityAction<AsyncResult<Result.VerifyGlobalRankingScoreByUserIdResult>> callback
        )
		{
			var task = new VerifyGlobalRankingScoreByUserIdTask(
			    Gs2WebSocketSession,
			    request
            );
            yield return task;
            callback.Invoke(new AsyncResult<Result.VerifyGlobalRankingScoreByUserIdResult>(task.Result, task.Error));
        }

		public IFuture<Result.VerifyGlobalRankingScoreByUserIdResult> VerifyGlobalRankingScoreByUserIdFuture(
                Request.VerifyGlobalRankingScoreByUserIdRequest request
        )
		{
			return new VerifyGlobalRankingScoreByUserIdTask(
			    Gs2WebSocketSession,
			    request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.VerifyGlobalRankingScoreByUserIdResult> VerifyGlobalRankingScoreByUserIdAsync(
            Request.VerifyGlobalRankingScoreByUserIdRequest request
        )
		{
		    var task = new VerifyGlobalRankingScoreByUserIdTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
    #else
		public VerifyGlobalRankingScoreByUserIdTask VerifyGlobalRankingScoreByUserIdAsync(
                Request.VerifyGlobalRankingScoreByUserIdRequest request
        )
		{
			return new VerifyGlobalRankingScoreByUserIdTask(
                Gs2WebSocketSession,
			    request
            );
        }
    #endif
#else
		public async Task<Result.VerifyGlobalRankingScoreByUserIdResult> VerifyGlobalRankingScoreByUserIdAsync(
            Request.VerifyGlobalRankingScoreByUserIdRequest request
        )
		{
		    var task = new VerifyGlobalRankingScoreByUserIdTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
#endif


        public class CreateGlobalRankingReceivedRewardTask : Gs2WebSocketSessionTask<Request.CreateGlobalRankingReceivedRewardRequest, Result.CreateGlobalRankingReceivedRewardResult>
        {
	        public CreateGlobalRankingReceivedRewardTask(IGs2Session session, Request.CreateGlobalRankingReceivedRewardRequest request) : base(session, request)
	        {
	        }

            protected override IGs2SessionRequest CreateRequest(Request.CreateGlobalRankingReceivedRewardRequest request)
            {
                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);

                jsonWriter.WriteObjectStart();

                if (request.NamespaceName != null)
                {
                    jsonWriter.WritePropertyName("namespaceName");
                    jsonWriter.Write(request.NamespaceName.ToString());
                }
                if (request.RankingName != null)
                {
                    jsonWriter.WritePropertyName("rankingName");
                    jsonWriter.Write(request.RankingName.ToString());
                }
                if (request.AccessToken != null)
                {
                    jsonWriter.WritePropertyName("accessToken");
                    jsonWriter.Write(request.AccessToken.ToString());
                }
                if (request.Season != null)
                {
                    jsonWriter.WritePropertyName("season");
                    jsonWriter.Write(request.Season.ToString());
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
                    "ranking2",
                    "globalRankingReceivedReward",
                    "createGlobalRankingReceivedReward",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator CreateGlobalRankingReceivedReward(
                Request.CreateGlobalRankingReceivedRewardRequest request,
                UnityAction<AsyncResult<Result.CreateGlobalRankingReceivedRewardResult>> callback
        )
		{
			var task = new CreateGlobalRankingReceivedRewardTask(
			    Gs2WebSocketSession,
			    request
            );
            yield return task;
            callback.Invoke(new AsyncResult<Result.CreateGlobalRankingReceivedRewardResult>(task.Result, task.Error));
        }

		public IFuture<Result.CreateGlobalRankingReceivedRewardResult> CreateGlobalRankingReceivedRewardFuture(
                Request.CreateGlobalRankingReceivedRewardRequest request
        )
		{
			return new CreateGlobalRankingReceivedRewardTask(
			    Gs2WebSocketSession,
			    request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.CreateGlobalRankingReceivedRewardResult> CreateGlobalRankingReceivedRewardAsync(
            Request.CreateGlobalRankingReceivedRewardRequest request
        )
		{
		    var task = new CreateGlobalRankingReceivedRewardTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
    #else
		public CreateGlobalRankingReceivedRewardTask CreateGlobalRankingReceivedRewardAsync(
                Request.CreateGlobalRankingReceivedRewardRequest request
        )
		{
			return new CreateGlobalRankingReceivedRewardTask(
                Gs2WebSocketSession,
			    request
            );
        }
    #endif
#else
		public async Task<Result.CreateGlobalRankingReceivedRewardResult> CreateGlobalRankingReceivedRewardAsync(
            Request.CreateGlobalRankingReceivedRewardRequest request
        )
		{
		    var task = new CreateGlobalRankingReceivedRewardTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
#endif


        public class CreateGlobalRankingReceivedRewardByUserIdTask : Gs2WebSocketSessionTask<Request.CreateGlobalRankingReceivedRewardByUserIdRequest, Result.CreateGlobalRankingReceivedRewardByUserIdResult>
        {
	        public CreateGlobalRankingReceivedRewardByUserIdTask(IGs2Session session, Request.CreateGlobalRankingReceivedRewardByUserIdRequest request) : base(session, request)
	        {
	        }

            protected override IGs2SessionRequest CreateRequest(Request.CreateGlobalRankingReceivedRewardByUserIdRequest request)
            {
                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);

                jsonWriter.WriteObjectStart();

                if (request.NamespaceName != null)
                {
                    jsonWriter.WritePropertyName("namespaceName");
                    jsonWriter.Write(request.NamespaceName.ToString());
                }
                if (request.RankingName != null)
                {
                    jsonWriter.WritePropertyName("rankingName");
                    jsonWriter.Write(request.RankingName.ToString());
                }
                if (request.UserId != null)
                {
                    jsonWriter.WritePropertyName("userId");
                    jsonWriter.Write(request.UserId.ToString());
                }
                if (request.Season != null)
                {
                    jsonWriter.WritePropertyName("season");
                    jsonWriter.Write(request.Season.ToString());
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
                    "ranking2",
                    "globalRankingReceivedReward",
                    "createGlobalRankingReceivedRewardByUserId",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator CreateGlobalRankingReceivedRewardByUserId(
                Request.CreateGlobalRankingReceivedRewardByUserIdRequest request,
                UnityAction<AsyncResult<Result.CreateGlobalRankingReceivedRewardByUserIdResult>> callback
        )
		{
			var task = new CreateGlobalRankingReceivedRewardByUserIdTask(
			    Gs2WebSocketSession,
			    request
            );
            yield return task;
            callback.Invoke(new AsyncResult<Result.CreateGlobalRankingReceivedRewardByUserIdResult>(task.Result, task.Error));
        }

		public IFuture<Result.CreateGlobalRankingReceivedRewardByUserIdResult> CreateGlobalRankingReceivedRewardByUserIdFuture(
                Request.CreateGlobalRankingReceivedRewardByUserIdRequest request
        )
		{
			return new CreateGlobalRankingReceivedRewardByUserIdTask(
			    Gs2WebSocketSession,
			    request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.CreateGlobalRankingReceivedRewardByUserIdResult> CreateGlobalRankingReceivedRewardByUserIdAsync(
            Request.CreateGlobalRankingReceivedRewardByUserIdRequest request
        )
		{
		    var task = new CreateGlobalRankingReceivedRewardByUserIdTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
    #else
		public CreateGlobalRankingReceivedRewardByUserIdTask CreateGlobalRankingReceivedRewardByUserIdAsync(
                Request.CreateGlobalRankingReceivedRewardByUserIdRequest request
        )
		{
			return new CreateGlobalRankingReceivedRewardByUserIdTask(
                Gs2WebSocketSession,
			    request
            );
        }
    #endif
#else
		public async Task<Result.CreateGlobalRankingReceivedRewardByUserIdResult> CreateGlobalRankingReceivedRewardByUserIdAsync(
            Request.CreateGlobalRankingReceivedRewardByUserIdRequest request
        )
		{
		    var task = new CreateGlobalRankingReceivedRewardByUserIdTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
#endif


        public class GetGlobalRankingReceivedRewardTask : Gs2WebSocketSessionTask<Request.GetGlobalRankingReceivedRewardRequest, Result.GetGlobalRankingReceivedRewardResult>
        {
	        public GetGlobalRankingReceivedRewardTask(IGs2Session session, Request.GetGlobalRankingReceivedRewardRequest request) : base(session, request)
	        {
	        }

            protected override IGs2SessionRequest CreateRequest(Request.GetGlobalRankingReceivedRewardRequest request)
            {
                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);

                jsonWriter.WriteObjectStart();

                if (request.NamespaceName != null)
                {
                    jsonWriter.WritePropertyName("namespaceName");
                    jsonWriter.Write(request.NamespaceName.ToString());
                }
                if (request.RankingName != null)
                {
                    jsonWriter.WritePropertyName("rankingName");
                    jsonWriter.Write(request.RankingName.ToString());
                }
                if (request.AccessToken != null)
                {
                    jsonWriter.WritePropertyName("accessToken");
                    jsonWriter.Write(request.AccessToken.ToString());
                }
                if (request.Season != null)
                {
                    jsonWriter.WritePropertyName("season");
                    jsonWriter.Write(request.Season.ToString());
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
                    "ranking2",
                    "globalRankingReceivedReward",
                    "getGlobalRankingReceivedReward",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator GetGlobalRankingReceivedReward(
                Request.GetGlobalRankingReceivedRewardRequest request,
                UnityAction<AsyncResult<Result.GetGlobalRankingReceivedRewardResult>> callback
        )
		{
			var task = new GetGlobalRankingReceivedRewardTask(
			    Gs2WebSocketSession,
			    request
            );
            yield return task;
            callback.Invoke(new AsyncResult<Result.GetGlobalRankingReceivedRewardResult>(task.Result, task.Error));
        }

		public IFuture<Result.GetGlobalRankingReceivedRewardResult> GetGlobalRankingReceivedRewardFuture(
                Request.GetGlobalRankingReceivedRewardRequest request
        )
		{
			return new GetGlobalRankingReceivedRewardTask(
			    Gs2WebSocketSession,
			    request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.GetGlobalRankingReceivedRewardResult> GetGlobalRankingReceivedRewardAsync(
            Request.GetGlobalRankingReceivedRewardRequest request
        )
		{
		    var task = new GetGlobalRankingReceivedRewardTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
    #else
		public GetGlobalRankingReceivedRewardTask GetGlobalRankingReceivedRewardAsync(
                Request.GetGlobalRankingReceivedRewardRequest request
        )
		{
			return new GetGlobalRankingReceivedRewardTask(
                Gs2WebSocketSession,
			    request
            );
        }
    #endif
#else
		public async Task<Result.GetGlobalRankingReceivedRewardResult> GetGlobalRankingReceivedRewardAsync(
            Request.GetGlobalRankingReceivedRewardRequest request
        )
		{
		    var task = new GetGlobalRankingReceivedRewardTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
#endif


        public class GetGlobalRankingReceivedRewardByUserIdTask : Gs2WebSocketSessionTask<Request.GetGlobalRankingReceivedRewardByUserIdRequest, Result.GetGlobalRankingReceivedRewardByUserIdResult>
        {
	        public GetGlobalRankingReceivedRewardByUserIdTask(IGs2Session session, Request.GetGlobalRankingReceivedRewardByUserIdRequest request) : base(session, request)
	        {
	        }

            protected override IGs2SessionRequest CreateRequest(Request.GetGlobalRankingReceivedRewardByUserIdRequest request)
            {
                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);

                jsonWriter.WriteObjectStart();

                if (request.NamespaceName != null)
                {
                    jsonWriter.WritePropertyName("namespaceName");
                    jsonWriter.Write(request.NamespaceName.ToString());
                }
                if (request.RankingName != null)
                {
                    jsonWriter.WritePropertyName("rankingName");
                    jsonWriter.Write(request.RankingName.ToString());
                }
                if (request.UserId != null)
                {
                    jsonWriter.WritePropertyName("userId");
                    jsonWriter.Write(request.UserId.ToString());
                }
                if (request.Season != null)
                {
                    jsonWriter.WritePropertyName("season");
                    jsonWriter.Write(request.Season.ToString());
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
                    "ranking2",
                    "globalRankingReceivedReward",
                    "getGlobalRankingReceivedRewardByUserId",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator GetGlobalRankingReceivedRewardByUserId(
                Request.GetGlobalRankingReceivedRewardByUserIdRequest request,
                UnityAction<AsyncResult<Result.GetGlobalRankingReceivedRewardByUserIdResult>> callback
        )
		{
			var task = new GetGlobalRankingReceivedRewardByUserIdTask(
			    Gs2WebSocketSession,
			    request
            );
            yield return task;
            callback.Invoke(new AsyncResult<Result.GetGlobalRankingReceivedRewardByUserIdResult>(task.Result, task.Error));
        }

		public IFuture<Result.GetGlobalRankingReceivedRewardByUserIdResult> GetGlobalRankingReceivedRewardByUserIdFuture(
                Request.GetGlobalRankingReceivedRewardByUserIdRequest request
        )
		{
			return new GetGlobalRankingReceivedRewardByUserIdTask(
			    Gs2WebSocketSession,
			    request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.GetGlobalRankingReceivedRewardByUserIdResult> GetGlobalRankingReceivedRewardByUserIdAsync(
            Request.GetGlobalRankingReceivedRewardByUserIdRequest request
        )
		{
		    var task = new GetGlobalRankingReceivedRewardByUserIdTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
    #else
		public GetGlobalRankingReceivedRewardByUserIdTask GetGlobalRankingReceivedRewardByUserIdAsync(
                Request.GetGlobalRankingReceivedRewardByUserIdRequest request
        )
		{
			return new GetGlobalRankingReceivedRewardByUserIdTask(
                Gs2WebSocketSession,
			    request
            );
        }
    #endif
#else
		public async Task<Result.GetGlobalRankingReceivedRewardByUserIdResult> GetGlobalRankingReceivedRewardByUserIdAsync(
            Request.GetGlobalRankingReceivedRewardByUserIdRequest request
        )
		{
		    var task = new GetGlobalRankingReceivedRewardByUserIdTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
#endif


        public class DeleteGlobalRankingReceivedRewardByUserIdTask : Gs2WebSocketSessionTask<Request.DeleteGlobalRankingReceivedRewardByUserIdRequest, Result.DeleteGlobalRankingReceivedRewardByUserIdResult>
        {
	        public DeleteGlobalRankingReceivedRewardByUserIdTask(IGs2Session session, Request.DeleteGlobalRankingReceivedRewardByUserIdRequest request) : base(session, request)
	        {
	        }

            protected override IGs2SessionRequest CreateRequest(Request.DeleteGlobalRankingReceivedRewardByUserIdRequest request)
            {
                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);

                jsonWriter.WriteObjectStart();

                if (request.NamespaceName != null)
                {
                    jsonWriter.WritePropertyName("namespaceName");
                    jsonWriter.Write(request.NamespaceName.ToString());
                }
                if (request.RankingName != null)
                {
                    jsonWriter.WritePropertyName("rankingName");
                    jsonWriter.Write(request.RankingName.ToString());
                }
                if (request.UserId != null)
                {
                    jsonWriter.WritePropertyName("userId");
                    jsonWriter.Write(request.UserId.ToString());
                }
                if (request.Season != null)
                {
                    jsonWriter.WritePropertyName("season");
                    jsonWriter.Write(request.Season.ToString());
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
                    "ranking2",
                    "globalRankingReceivedReward",
                    "deleteGlobalRankingReceivedRewardByUserId",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator DeleteGlobalRankingReceivedRewardByUserId(
                Request.DeleteGlobalRankingReceivedRewardByUserIdRequest request,
                UnityAction<AsyncResult<Result.DeleteGlobalRankingReceivedRewardByUserIdResult>> callback
        )
		{
			var task = new DeleteGlobalRankingReceivedRewardByUserIdTask(
			    Gs2WebSocketSession,
			    request
            );
            yield return task;
            callback.Invoke(new AsyncResult<Result.DeleteGlobalRankingReceivedRewardByUserIdResult>(task.Result, task.Error));
        }

		public IFuture<Result.DeleteGlobalRankingReceivedRewardByUserIdResult> DeleteGlobalRankingReceivedRewardByUserIdFuture(
                Request.DeleteGlobalRankingReceivedRewardByUserIdRequest request
        )
		{
			return new DeleteGlobalRankingReceivedRewardByUserIdTask(
			    Gs2WebSocketSession,
			    request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.DeleteGlobalRankingReceivedRewardByUserIdResult> DeleteGlobalRankingReceivedRewardByUserIdAsync(
            Request.DeleteGlobalRankingReceivedRewardByUserIdRequest request
        )
		{
		    var task = new DeleteGlobalRankingReceivedRewardByUserIdTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
    #else
		public DeleteGlobalRankingReceivedRewardByUserIdTask DeleteGlobalRankingReceivedRewardByUserIdAsync(
                Request.DeleteGlobalRankingReceivedRewardByUserIdRequest request
        )
		{
			return new DeleteGlobalRankingReceivedRewardByUserIdTask(
                Gs2WebSocketSession,
			    request
            );
        }
    #endif
#else
		public async Task<Result.DeleteGlobalRankingReceivedRewardByUserIdResult> DeleteGlobalRankingReceivedRewardByUserIdAsync(
            Request.DeleteGlobalRankingReceivedRewardByUserIdRequest request
        )
		{
		    var task = new DeleteGlobalRankingReceivedRewardByUserIdTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
#endif


        public class GetGlobalRankingTask : Gs2WebSocketSessionTask<Request.GetGlobalRankingRequest, Result.GetGlobalRankingResult>
        {
	        public GetGlobalRankingTask(IGs2Session session, Request.GetGlobalRankingRequest request) : base(session, request)
	        {
	        }

            protected override IGs2SessionRequest CreateRequest(Request.GetGlobalRankingRequest request)
            {
                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);

                jsonWriter.WriteObjectStart();

                if (request.NamespaceName != null)
                {
                    jsonWriter.WritePropertyName("namespaceName");
                    jsonWriter.Write(request.NamespaceName.ToString());
                }
                if (request.RankingName != null)
                {
                    jsonWriter.WritePropertyName("rankingName");
                    jsonWriter.Write(request.RankingName.ToString());
                }
                if (request.AccessToken != null)
                {
                    jsonWriter.WritePropertyName("accessToken");
                    jsonWriter.Write(request.AccessToken.ToString());
                }
                if (request.Season != null)
                {
                    jsonWriter.WritePropertyName("season");
                    jsonWriter.Write(request.Season.ToString());
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
                    "ranking2",
                    "globalRankingData",
                    "getGlobalRanking",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator GetGlobalRanking(
                Request.GetGlobalRankingRequest request,
                UnityAction<AsyncResult<Result.GetGlobalRankingResult>> callback
        )
		{
			var task = new GetGlobalRankingTask(
			    Gs2WebSocketSession,
			    request
            );
            yield return task;
            callback.Invoke(new AsyncResult<Result.GetGlobalRankingResult>(task.Result, task.Error));
        }

		public IFuture<Result.GetGlobalRankingResult> GetGlobalRankingFuture(
                Request.GetGlobalRankingRequest request
        )
		{
			return new GetGlobalRankingTask(
			    Gs2WebSocketSession,
			    request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.GetGlobalRankingResult> GetGlobalRankingAsync(
            Request.GetGlobalRankingRequest request
        )
		{
		    var task = new GetGlobalRankingTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
    #else
		public GetGlobalRankingTask GetGlobalRankingAsync(
                Request.GetGlobalRankingRequest request
        )
		{
			return new GetGlobalRankingTask(
                Gs2WebSocketSession,
			    request
            );
        }
    #endif
#else
		public async Task<Result.GetGlobalRankingResult> GetGlobalRankingAsync(
            Request.GetGlobalRankingRequest request
        )
		{
		    var task = new GetGlobalRankingTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
#endif


        public class GetGlobalRankingByUserIdTask : Gs2WebSocketSessionTask<Request.GetGlobalRankingByUserIdRequest, Result.GetGlobalRankingByUserIdResult>
        {
	        public GetGlobalRankingByUserIdTask(IGs2Session session, Request.GetGlobalRankingByUserIdRequest request) : base(session, request)
	        {
	        }

            protected override IGs2SessionRequest CreateRequest(Request.GetGlobalRankingByUserIdRequest request)
            {
                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);

                jsonWriter.WriteObjectStart();

                if (request.NamespaceName != null)
                {
                    jsonWriter.WritePropertyName("namespaceName");
                    jsonWriter.Write(request.NamespaceName.ToString());
                }
                if (request.RankingName != null)
                {
                    jsonWriter.WritePropertyName("rankingName");
                    jsonWriter.Write(request.RankingName.ToString());
                }
                if (request.UserId != null)
                {
                    jsonWriter.WritePropertyName("userId");
                    jsonWriter.Write(request.UserId.ToString());
                }
                if (request.Season != null)
                {
                    jsonWriter.WritePropertyName("season");
                    jsonWriter.Write(request.Season.ToString());
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
                    "ranking2",
                    "globalRankingData",
                    "getGlobalRankingByUserId",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator GetGlobalRankingByUserId(
                Request.GetGlobalRankingByUserIdRequest request,
                UnityAction<AsyncResult<Result.GetGlobalRankingByUserIdResult>> callback
        )
		{
			var task = new GetGlobalRankingByUserIdTask(
			    Gs2WebSocketSession,
			    request
            );
            yield return task;
            callback.Invoke(new AsyncResult<Result.GetGlobalRankingByUserIdResult>(task.Result, task.Error));
        }

		public IFuture<Result.GetGlobalRankingByUserIdResult> GetGlobalRankingByUserIdFuture(
                Request.GetGlobalRankingByUserIdRequest request
        )
		{
			return new GetGlobalRankingByUserIdTask(
			    Gs2WebSocketSession,
			    request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.GetGlobalRankingByUserIdResult> GetGlobalRankingByUserIdAsync(
            Request.GetGlobalRankingByUserIdRequest request
        )
		{
		    var task = new GetGlobalRankingByUserIdTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
    #else
		public GetGlobalRankingByUserIdTask GetGlobalRankingByUserIdAsync(
                Request.GetGlobalRankingByUserIdRequest request
        )
		{
			return new GetGlobalRankingByUserIdTask(
                Gs2WebSocketSession,
			    request
            );
        }
    #endif
#else
		public async Task<Result.GetGlobalRankingByUserIdResult> GetGlobalRankingByUserIdAsync(
            Request.GetGlobalRankingByUserIdRequest request
        )
		{
		    var task = new GetGlobalRankingByUserIdTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
#endif


        public class PutClusterRankingScoreTask : Gs2WebSocketSessionTask<Request.PutClusterRankingScoreRequest, Result.PutClusterRankingScoreResult>
        {
	        public PutClusterRankingScoreTask(IGs2Session session, Request.PutClusterRankingScoreRequest request) : base(session, request)
	        {
	        }

            protected override IGs2SessionRequest CreateRequest(Request.PutClusterRankingScoreRequest request)
            {
                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);

                jsonWriter.WriteObjectStart();

                if (request.NamespaceName != null)
                {
                    jsonWriter.WritePropertyName("namespaceName");
                    jsonWriter.Write(request.NamespaceName.ToString());
                }
                if (request.RankingName != null)
                {
                    jsonWriter.WritePropertyName("rankingName");
                    jsonWriter.Write(request.RankingName.ToString());
                }
                if (request.ClusterName != null)
                {
                    jsonWriter.WritePropertyName("clusterName");
                    jsonWriter.Write(request.ClusterName.ToString());
                }
                if (request.AccessToken != null)
                {
                    jsonWriter.WritePropertyName("accessToken");
                    jsonWriter.Write(request.AccessToken.ToString());
                }
                if (request.Score != null)
                {
                    jsonWriter.WritePropertyName("score");
                    jsonWriter.Write(request.Score.ToString());
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
                    "ranking2",
                    "clusterRankingScore",
                    "putClusterRankingScore",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator PutClusterRankingScore(
                Request.PutClusterRankingScoreRequest request,
                UnityAction<AsyncResult<Result.PutClusterRankingScoreResult>> callback
        )
		{
			var task = new PutClusterRankingScoreTask(
			    Gs2WebSocketSession,
			    request
            );
            yield return task;
            callback.Invoke(new AsyncResult<Result.PutClusterRankingScoreResult>(task.Result, task.Error));
        }

		public IFuture<Result.PutClusterRankingScoreResult> PutClusterRankingScoreFuture(
                Request.PutClusterRankingScoreRequest request
        )
		{
			return new PutClusterRankingScoreTask(
			    Gs2WebSocketSession,
			    request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.PutClusterRankingScoreResult> PutClusterRankingScoreAsync(
            Request.PutClusterRankingScoreRequest request
        )
		{
		    var task = new PutClusterRankingScoreTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
    #else
		public PutClusterRankingScoreTask PutClusterRankingScoreAsync(
                Request.PutClusterRankingScoreRequest request
        )
		{
			return new PutClusterRankingScoreTask(
                Gs2WebSocketSession,
			    request
            );
        }
    #endif
#else
		public async Task<Result.PutClusterRankingScoreResult> PutClusterRankingScoreAsync(
            Request.PutClusterRankingScoreRequest request
        )
		{
		    var task = new PutClusterRankingScoreTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
#endif


        public class PutClusterRankingScoreByUserIdTask : Gs2WebSocketSessionTask<Request.PutClusterRankingScoreByUserIdRequest, Result.PutClusterRankingScoreByUserIdResult>
        {
	        public PutClusterRankingScoreByUserIdTask(IGs2Session session, Request.PutClusterRankingScoreByUserIdRequest request) : base(session, request)
	        {
	        }

            protected override IGs2SessionRequest CreateRequest(Request.PutClusterRankingScoreByUserIdRequest request)
            {
                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);

                jsonWriter.WriteObjectStart();

                if (request.NamespaceName != null)
                {
                    jsonWriter.WritePropertyName("namespaceName");
                    jsonWriter.Write(request.NamespaceName.ToString());
                }
                if (request.RankingName != null)
                {
                    jsonWriter.WritePropertyName("rankingName");
                    jsonWriter.Write(request.RankingName.ToString());
                }
                if (request.ClusterName != null)
                {
                    jsonWriter.WritePropertyName("clusterName");
                    jsonWriter.Write(request.ClusterName.ToString());
                }
                if (request.UserId != null)
                {
                    jsonWriter.WritePropertyName("userId");
                    jsonWriter.Write(request.UserId.ToString());
                }
                if (request.Score != null)
                {
                    jsonWriter.WritePropertyName("score");
                    jsonWriter.Write(request.Score.ToString());
                }
                if (request.Metadata != null)
                {
                    jsonWriter.WritePropertyName("metadata");
                    jsonWriter.Write(request.Metadata.ToString());
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
                    "ranking2",
                    "clusterRankingScore",
                    "putClusterRankingScoreByUserId",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator PutClusterRankingScoreByUserId(
                Request.PutClusterRankingScoreByUserIdRequest request,
                UnityAction<AsyncResult<Result.PutClusterRankingScoreByUserIdResult>> callback
        )
		{
			var task = new PutClusterRankingScoreByUserIdTask(
			    Gs2WebSocketSession,
			    request
            );
            yield return task;
            callback.Invoke(new AsyncResult<Result.PutClusterRankingScoreByUserIdResult>(task.Result, task.Error));
        }

		public IFuture<Result.PutClusterRankingScoreByUserIdResult> PutClusterRankingScoreByUserIdFuture(
                Request.PutClusterRankingScoreByUserIdRequest request
        )
		{
			return new PutClusterRankingScoreByUserIdTask(
			    Gs2WebSocketSession,
			    request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.PutClusterRankingScoreByUserIdResult> PutClusterRankingScoreByUserIdAsync(
            Request.PutClusterRankingScoreByUserIdRequest request
        )
		{
		    var task = new PutClusterRankingScoreByUserIdTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
    #else
		public PutClusterRankingScoreByUserIdTask PutClusterRankingScoreByUserIdAsync(
                Request.PutClusterRankingScoreByUserIdRequest request
        )
		{
			return new PutClusterRankingScoreByUserIdTask(
                Gs2WebSocketSession,
			    request
            );
        }
    #endif
#else
		public async Task<Result.PutClusterRankingScoreByUserIdResult> PutClusterRankingScoreByUserIdAsync(
            Request.PutClusterRankingScoreByUserIdRequest request
        )
		{
		    var task = new PutClusterRankingScoreByUserIdTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
#endif


        public class GetClusterRankingScoreTask : Gs2WebSocketSessionTask<Request.GetClusterRankingScoreRequest, Result.GetClusterRankingScoreResult>
        {
	        public GetClusterRankingScoreTask(IGs2Session session, Request.GetClusterRankingScoreRequest request) : base(session, request)
	        {
	        }

            protected override IGs2SessionRequest CreateRequest(Request.GetClusterRankingScoreRequest request)
            {
                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);

                jsonWriter.WriteObjectStart();

                if (request.NamespaceName != null)
                {
                    jsonWriter.WritePropertyName("namespaceName");
                    jsonWriter.Write(request.NamespaceName.ToString());
                }
                if (request.RankingName != null)
                {
                    jsonWriter.WritePropertyName("rankingName");
                    jsonWriter.Write(request.RankingName.ToString());
                }
                if (request.ClusterName != null)
                {
                    jsonWriter.WritePropertyName("clusterName");
                    jsonWriter.Write(request.ClusterName.ToString());
                }
                if (request.AccessToken != null)
                {
                    jsonWriter.WritePropertyName("accessToken");
                    jsonWriter.Write(request.AccessToken.ToString());
                }
                if (request.Season != null)
                {
                    jsonWriter.WritePropertyName("season");
                    jsonWriter.Write(request.Season.ToString());
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
                    "ranking2",
                    "clusterRankingScore",
                    "getClusterRankingScore",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator GetClusterRankingScore(
                Request.GetClusterRankingScoreRequest request,
                UnityAction<AsyncResult<Result.GetClusterRankingScoreResult>> callback
        )
		{
			var task = new GetClusterRankingScoreTask(
			    Gs2WebSocketSession,
			    request
            );
            yield return task;
            callback.Invoke(new AsyncResult<Result.GetClusterRankingScoreResult>(task.Result, task.Error));
        }

		public IFuture<Result.GetClusterRankingScoreResult> GetClusterRankingScoreFuture(
                Request.GetClusterRankingScoreRequest request
        )
		{
			return new GetClusterRankingScoreTask(
			    Gs2WebSocketSession,
			    request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.GetClusterRankingScoreResult> GetClusterRankingScoreAsync(
            Request.GetClusterRankingScoreRequest request
        )
		{
		    var task = new GetClusterRankingScoreTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
    #else
		public GetClusterRankingScoreTask GetClusterRankingScoreAsync(
                Request.GetClusterRankingScoreRequest request
        )
		{
			return new GetClusterRankingScoreTask(
                Gs2WebSocketSession,
			    request
            );
        }
    #endif
#else
		public async Task<Result.GetClusterRankingScoreResult> GetClusterRankingScoreAsync(
            Request.GetClusterRankingScoreRequest request
        )
		{
		    var task = new GetClusterRankingScoreTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
#endif


        public class GetClusterRankingScoreByUserIdTask : Gs2WebSocketSessionTask<Request.GetClusterRankingScoreByUserIdRequest, Result.GetClusterRankingScoreByUserIdResult>
        {
	        public GetClusterRankingScoreByUserIdTask(IGs2Session session, Request.GetClusterRankingScoreByUserIdRequest request) : base(session, request)
	        {
	        }

            protected override IGs2SessionRequest CreateRequest(Request.GetClusterRankingScoreByUserIdRequest request)
            {
                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);

                jsonWriter.WriteObjectStart();

                if (request.NamespaceName != null)
                {
                    jsonWriter.WritePropertyName("namespaceName");
                    jsonWriter.Write(request.NamespaceName.ToString());
                }
                if (request.RankingName != null)
                {
                    jsonWriter.WritePropertyName("rankingName");
                    jsonWriter.Write(request.RankingName.ToString());
                }
                if (request.ClusterName != null)
                {
                    jsonWriter.WritePropertyName("clusterName");
                    jsonWriter.Write(request.ClusterName.ToString());
                }
                if (request.UserId != null)
                {
                    jsonWriter.WritePropertyName("userId");
                    jsonWriter.Write(request.UserId.ToString());
                }
                if (request.Season != null)
                {
                    jsonWriter.WritePropertyName("season");
                    jsonWriter.Write(request.Season.ToString());
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
                    "ranking2",
                    "clusterRankingScore",
                    "getClusterRankingScoreByUserId",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator GetClusterRankingScoreByUserId(
                Request.GetClusterRankingScoreByUserIdRequest request,
                UnityAction<AsyncResult<Result.GetClusterRankingScoreByUserIdResult>> callback
        )
		{
			var task = new GetClusterRankingScoreByUserIdTask(
			    Gs2WebSocketSession,
			    request
            );
            yield return task;
            callback.Invoke(new AsyncResult<Result.GetClusterRankingScoreByUserIdResult>(task.Result, task.Error));
        }

		public IFuture<Result.GetClusterRankingScoreByUserIdResult> GetClusterRankingScoreByUserIdFuture(
                Request.GetClusterRankingScoreByUserIdRequest request
        )
		{
			return new GetClusterRankingScoreByUserIdTask(
			    Gs2WebSocketSession,
			    request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.GetClusterRankingScoreByUserIdResult> GetClusterRankingScoreByUserIdAsync(
            Request.GetClusterRankingScoreByUserIdRequest request
        )
		{
		    var task = new GetClusterRankingScoreByUserIdTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
    #else
		public GetClusterRankingScoreByUserIdTask GetClusterRankingScoreByUserIdAsync(
                Request.GetClusterRankingScoreByUserIdRequest request
        )
		{
			return new GetClusterRankingScoreByUserIdTask(
                Gs2WebSocketSession,
			    request
            );
        }
    #endif
#else
		public async Task<Result.GetClusterRankingScoreByUserIdResult> GetClusterRankingScoreByUserIdAsync(
            Request.GetClusterRankingScoreByUserIdRequest request
        )
		{
		    var task = new GetClusterRankingScoreByUserIdTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
#endif


        public class DeleteClusterRankingScoreByUserIdTask : Gs2WebSocketSessionTask<Request.DeleteClusterRankingScoreByUserIdRequest, Result.DeleteClusterRankingScoreByUserIdResult>
        {
	        public DeleteClusterRankingScoreByUserIdTask(IGs2Session session, Request.DeleteClusterRankingScoreByUserIdRequest request) : base(session, request)
	        {
	        }

            protected override IGs2SessionRequest CreateRequest(Request.DeleteClusterRankingScoreByUserIdRequest request)
            {
                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);

                jsonWriter.WriteObjectStart();

                if (request.NamespaceName != null)
                {
                    jsonWriter.WritePropertyName("namespaceName");
                    jsonWriter.Write(request.NamespaceName.ToString());
                }
                if (request.RankingName != null)
                {
                    jsonWriter.WritePropertyName("rankingName");
                    jsonWriter.Write(request.RankingName.ToString());
                }
                if (request.ClusterName != null)
                {
                    jsonWriter.WritePropertyName("clusterName");
                    jsonWriter.Write(request.ClusterName.ToString());
                }
                if (request.UserId != null)
                {
                    jsonWriter.WritePropertyName("userId");
                    jsonWriter.Write(request.UserId.ToString());
                }
                if (request.Season != null)
                {
                    jsonWriter.WritePropertyName("season");
                    jsonWriter.Write(request.Season.ToString());
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
                    "ranking2",
                    "clusterRankingScore",
                    "deleteClusterRankingScoreByUserId",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator DeleteClusterRankingScoreByUserId(
                Request.DeleteClusterRankingScoreByUserIdRequest request,
                UnityAction<AsyncResult<Result.DeleteClusterRankingScoreByUserIdResult>> callback
        )
		{
			var task = new DeleteClusterRankingScoreByUserIdTask(
			    Gs2WebSocketSession,
			    request
            );
            yield return task;
            callback.Invoke(new AsyncResult<Result.DeleteClusterRankingScoreByUserIdResult>(task.Result, task.Error));
        }

		public IFuture<Result.DeleteClusterRankingScoreByUserIdResult> DeleteClusterRankingScoreByUserIdFuture(
                Request.DeleteClusterRankingScoreByUserIdRequest request
        )
		{
			return new DeleteClusterRankingScoreByUserIdTask(
			    Gs2WebSocketSession,
			    request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.DeleteClusterRankingScoreByUserIdResult> DeleteClusterRankingScoreByUserIdAsync(
            Request.DeleteClusterRankingScoreByUserIdRequest request
        )
		{
		    var task = new DeleteClusterRankingScoreByUserIdTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
    #else
		public DeleteClusterRankingScoreByUserIdTask DeleteClusterRankingScoreByUserIdAsync(
                Request.DeleteClusterRankingScoreByUserIdRequest request
        )
		{
			return new DeleteClusterRankingScoreByUserIdTask(
                Gs2WebSocketSession,
			    request
            );
        }
    #endif
#else
		public async Task<Result.DeleteClusterRankingScoreByUserIdResult> DeleteClusterRankingScoreByUserIdAsync(
            Request.DeleteClusterRankingScoreByUserIdRequest request
        )
		{
		    var task = new DeleteClusterRankingScoreByUserIdTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
#endif


        public class VerifyClusterRankingScoreTask : Gs2WebSocketSessionTask<Request.VerifyClusterRankingScoreRequest, Result.VerifyClusterRankingScoreResult>
        {
	        public VerifyClusterRankingScoreTask(IGs2Session session, Request.VerifyClusterRankingScoreRequest request) : base(session, request)
	        {
	        }

            protected override IGs2SessionRequest CreateRequest(Request.VerifyClusterRankingScoreRequest request)
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
                if (request.RankingName != null)
                {
                    jsonWriter.WritePropertyName("rankingName");
                    jsonWriter.Write(request.RankingName.ToString());
                }
                if (request.ClusterName != null)
                {
                    jsonWriter.WritePropertyName("clusterName");
                    jsonWriter.Write(request.ClusterName.ToString());
                }
                if (request.VerifyType != null)
                {
                    jsonWriter.WritePropertyName("verifyType");
                    jsonWriter.Write(request.VerifyType.ToString());
                }
                if (request.Season != null)
                {
                    jsonWriter.WritePropertyName("season");
                    jsonWriter.Write(request.Season.ToString());
                }
                if (request.Score != null)
                {
                    jsonWriter.WritePropertyName("score");
                    jsonWriter.Write(request.Score.ToString());
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
                    "ranking2",
                    "clusterRankingScore",
                    "verifyClusterRankingScore",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator VerifyClusterRankingScore(
                Request.VerifyClusterRankingScoreRequest request,
                UnityAction<AsyncResult<Result.VerifyClusterRankingScoreResult>> callback
        )
		{
			var task = new VerifyClusterRankingScoreTask(
			    Gs2WebSocketSession,
			    request
            );
            yield return task;
            callback.Invoke(new AsyncResult<Result.VerifyClusterRankingScoreResult>(task.Result, task.Error));
        }

		public IFuture<Result.VerifyClusterRankingScoreResult> VerifyClusterRankingScoreFuture(
                Request.VerifyClusterRankingScoreRequest request
        )
		{
			return new VerifyClusterRankingScoreTask(
			    Gs2WebSocketSession,
			    request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.VerifyClusterRankingScoreResult> VerifyClusterRankingScoreAsync(
            Request.VerifyClusterRankingScoreRequest request
        )
		{
		    var task = new VerifyClusterRankingScoreTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
    #else
		public VerifyClusterRankingScoreTask VerifyClusterRankingScoreAsync(
                Request.VerifyClusterRankingScoreRequest request
        )
		{
			return new VerifyClusterRankingScoreTask(
                Gs2WebSocketSession,
			    request
            );
        }
    #endif
#else
		public async Task<Result.VerifyClusterRankingScoreResult> VerifyClusterRankingScoreAsync(
            Request.VerifyClusterRankingScoreRequest request
        )
		{
		    var task = new VerifyClusterRankingScoreTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
#endif


        public class VerifyClusterRankingScoreByUserIdTask : Gs2WebSocketSessionTask<Request.VerifyClusterRankingScoreByUserIdRequest, Result.VerifyClusterRankingScoreByUserIdResult>
        {
	        public VerifyClusterRankingScoreByUserIdTask(IGs2Session session, Request.VerifyClusterRankingScoreByUserIdRequest request) : base(session, request)
	        {
	        }

            protected override IGs2SessionRequest CreateRequest(Request.VerifyClusterRankingScoreByUserIdRequest request)
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
                if (request.RankingName != null)
                {
                    jsonWriter.WritePropertyName("rankingName");
                    jsonWriter.Write(request.RankingName.ToString());
                }
                if (request.ClusterName != null)
                {
                    jsonWriter.WritePropertyName("clusterName");
                    jsonWriter.Write(request.ClusterName.ToString());
                }
                if (request.VerifyType != null)
                {
                    jsonWriter.WritePropertyName("verifyType");
                    jsonWriter.Write(request.VerifyType.ToString());
                }
                if (request.Season != null)
                {
                    jsonWriter.WritePropertyName("season");
                    jsonWriter.Write(request.Season.ToString());
                }
                if (request.Score != null)
                {
                    jsonWriter.WritePropertyName("score");
                    jsonWriter.Write(request.Score.ToString());
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
                    "ranking2",
                    "clusterRankingScore",
                    "verifyClusterRankingScoreByUserId",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator VerifyClusterRankingScoreByUserId(
                Request.VerifyClusterRankingScoreByUserIdRequest request,
                UnityAction<AsyncResult<Result.VerifyClusterRankingScoreByUserIdResult>> callback
        )
		{
			var task = new VerifyClusterRankingScoreByUserIdTask(
			    Gs2WebSocketSession,
			    request
            );
            yield return task;
            callback.Invoke(new AsyncResult<Result.VerifyClusterRankingScoreByUserIdResult>(task.Result, task.Error));
        }

		public IFuture<Result.VerifyClusterRankingScoreByUserIdResult> VerifyClusterRankingScoreByUserIdFuture(
                Request.VerifyClusterRankingScoreByUserIdRequest request
        )
		{
			return new VerifyClusterRankingScoreByUserIdTask(
			    Gs2WebSocketSession,
			    request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.VerifyClusterRankingScoreByUserIdResult> VerifyClusterRankingScoreByUserIdAsync(
            Request.VerifyClusterRankingScoreByUserIdRequest request
        )
		{
		    var task = new VerifyClusterRankingScoreByUserIdTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
    #else
		public VerifyClusterRankingScoreByUserIdTask VerifyClusterRankingScoreByUserIdAsync(
                Request.VerifyClusterRankingScoreByUserIdRequest request
        )
		{
			return new VerifyClusterRankingScoreByUserIdTask(
                Gs2WebSocketSession,
			    request
            );
        }
    #endif
#else
		public async Task<Result.VerifyClusterRankingScoreByUserIdResult> VerifyClusterRankingScoreByUserIdAsync(
            Request.VerifyClusterRankingScoreByUserIdRequest request
        )
		{
		    var task = new VerifyClusterRankingScoreByUserIdTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
#endif


        public class CreateClusterRankingReceivedRewardTask : Gs2WebSocketSessionTask<Request.CreateClusterRankingReceivedRewardRequest, Result.CreateClusterRankingReceivedRewardResult>
        {
	        public CreateClusterRankingReceivedRewardTask(IGs2Session session, Request.CreateClusterRankingReceivedRewardRequest request) : base(session, request)
	        {
	        }

            protected override IGs2SessionRequest CreateRequest(Request.CreateClusterRankingReceivedRewardRequest request)
            {
                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);

                jsonWriter.WriteObjectStart();

                if (request.NamespaceName != null)
                {
                    jsonWriter.WritePropertyName("namespaceName");
                    jsonWriter.Write(request.NamespaceName.ToString());
                }
                if (request.RankingName != null)
                {
                    jsonWriter.WritePropertyName("rankingName");
                    jsonWriter.Write(request.RankingName.ToString());
                }
                if (request.ClusterName != null)
                {
                    jsonWriter.WritePropertyName("clusterName");
                    jsonWriter.Write(request.ClusterName.ToString());
                }
                if (request.AccessToken != null)
                {
                    jsonWriter.WritePropertyName("accessToken");
                    jsonWriter.Write(request.AccessToken.ToString());
                }
                if (request.Season != null)
                {
                    jsonWriter.WritePropertyName("season");
                    jsonWriter.Write(request.Season.ToString());
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
                    "ranking2",
                    "clusterRankingReceivedReward",
                    "createClusterRankingReceivedReward",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator CreateClusterRankingReceivedReward(
                Request.CreateClusterRankingReceivedRewardRequest request,
                UnityAction<AsyncResult<Result.CreateClusterRankingReceivedRewardResult>> callback
        )
		{
			var task = new CreateClusterRankingReceivedRewardTask(
			    Gs2WebSocketSession,
			    request
            );
            yield return task;
            callback.Invoke(new AsyncResult<Result.CreateClusterRankingReceivedRewardResult>(task.Result, task.Error));
        }

		public IFuture<Result.CreateClusterRankingReceivedRewardResult> CreateClusterRankingReceivedRewardFuture(
                Request.CreateClusterRankingReceivedRewardRequest request
        )
		{
			return new CreateClusterRankingReceivedRewardTask(
			    Gs2WebSocketSession,
			    request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.CreateClusterRankingReceivedRewardResult> CreateClusterRankingReceivedRewardAsync(
            Request.CreateClusterRankingReceivedRewardRequest request
        )
		{
		    var task = new CreateClusterRankingReceivedRewardTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
    #else
		public CreateClusterRankingReceivedRewardTask CreateClusterRankingReceivedRewardAsync(
                Request.CreateClusterRankingReceivedRewardRequest request
        )
		{
			return new CreateClusterRankingReceivedRewardTask(
                Gs2WebSocketSession,
			    request
            );
        }
    #endif
#else
		public async Task<Result.CreateClusterRankingReceivedRewardResult> CreateClusterRankingReceivedRewardAsync(
            Request.CreateClusterRankingReceivedRewardRequest request
        )
		{
		    var task = new CreateClusterRankingReceivedRewardTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
#endif


        public class CreateClusterRankingReceivedRewardByUserIdTask : Gs2WebSocketSessionTask<Request.CreateClusterRankingReceivedRewardByUserIdRequest, Result.CreateClusterRankingReceivedRewardByUserIdResult>
        {
	        public CreateClusterRankingReceivedRewardByUserIdTask(IGs2Session session, Request.CreateClusterRankingReceivedRewardByUserIdRequest request) : base(session, request)
	        {
	        }

            protected override IGs2SessionRequest CreateRequest(Request.CreateClusterRankingReceivedRewardByUserIdRequest request)
            {
                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);

                jsonWriter.WriteObjectStart();

                if (request.NamespaceName != null)
                {
                    jsonWriter.WritePropertyName("namespaceName");
                    jsonWriter.Write(request.NamespaceName.ToString());
                }
                if (request.RankingName != null)
                {
                    jsonWriter.WritePropertyName("rankingName");
                    jsonWriter.Write(request.RankingName.ToString());
                }
                if (request.ClusterName != null)
                {
                    jsonWriter.WritePropertyName("clusterName");
                    jsonWriter.Write(request.ClusterName.ToString());
                }
                if (request.UserId != null)
                {
                    jsonWriter.WritePropertyName("userId");
                    jsonWriter.Write(request.UserId.ToString());
                }
                if (request.Season != null)
                {
                    jsonWriter.WritePropertyName("season");
                    jsonWriter.Write(request.Season.ToString());
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
                    "ranking2",
                    "clusterRankingReceivedReward",
                    "createClusterRankingReceivedRewardByUserId",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator CreateClusterRankingReceivedRewardByUserId(
                Request.CreateClusterRankingReceivedRewardByUserIdRequest request,
                UnityAction<AsyncResult<Result.CreateClusterRankingReceivedRewardByUserIdResult>> callback
        )
		{
			var task = new CreateClusterRankingReceivedRewardByUserIdTask(
			    Gs2WebSocketSession,
			    request
            );
            yield return task;
            callback.Invoke(new AsyncResult<Result.CreateClusterRankingReceivedRewardByUserIdResult>(task.Result, task.Error));
        }

		public IFuture<Result.CreateClusterRankingReceivedRewardByUserIdResult> CreateClusterRankingReceivedRewardByUserIdFuture(
                Request.CreateClusterRankingReceivedRewardByUserIdRequest request
        )
		{
			return new CreateClusterRankingReceivedRewardByUserIdTask(
			    Gs2WebSocketSession,
			    request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.CreateClusterRankingReceivedRewardByUserIdResult> CreateClusterRankingReceivedRewardByUserIdAsync(
            Request.CreateClusterRankingReceivedRewardByUserIdRequest request
        )
		{
		    var task = new CreateClusterRankingReceivedRewardByUserIdTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
    #else
		public CreateClusterRankingReceivedRewardByUserIdTask CreateClusterRankingReceivedRewardByUserIdAsync(
                Request.CreateClusterRankingReceivedRewardByUserIdRequest request
        )
		{
			return new CreateClusterRankingReceivedRewardByUserIdTask(
                Gs2WebSocketSession,
			    request
            );
        }
    #endif
#else
		public async Task<Result.CreateClusterRankingReceivedRewardByUserIdResult> CreateClusterRankingReceivedRewardByUserIdAsync(
            Request.CreateClusterRankingReceivedRewardByUserIdRequest request
        )
		{
		    var task = new CreateClusterRankingReceivedRewardByUserIdTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
#endif


        public class GetClusterRankingReceivedRewardTask : Gs2WebSocketSessionTask<Request.GetClusterRankingReceivedRewardRequest, Result.GetClusterRankingReceivedRewardResult>
        {
	        public GetClusterRankingReceivedRewardTask(IGs2Session session, Request.GetClusterRankingReceivedRewardRequest request) : base(session, request)
	        {
	        }

            protected override IGs2SessionRequest CreateRequest(Request.GetClusterRankingReceivedRewardRequest request)
            {
                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);

                jsonWriter.WriteObjectStart();

                if (request.NamespaceName != null)
                {
                    jsonWriter.WritePropertyName("namespaceName");
                    jsonWriter.Write(request.NamespaceName.ToString());
                }
                if (request.RankingName != null)
                {
                    jsonWriter.WritePropertyName("rankingName");
                    jsonWriter.Write(request.RankingName.ToString());
                }
                if (request.ClusterName != null)
                {
                    jsonWriter.WritePropertyName("clusterName");
                    jsonWriter.Write(request.ClusterName.ToString());
                }
                if (request.AccessToken != null)
                {
                    jsonWriter.WritePropertyName("accessToken");
                    jsonWriter.Write(request.AccessToken.ToString());
                }
                if (request.Season != null)
                {
                    jsonWriter.WritePropertyName("season");
                    jsonWriter.Write(request.Season.ToString());
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
                    "ranking2",
                    "clusterRankingReceivedReward",
                    "getClusterRankingReceivedReward",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator GetClusterRankingReceivedReward(
                Request.GetClusterRankingReceivedRewardRequest request,
                UnityAction<AsyncResult<Result.GetClusterRankingReceivedRewardResult>> callback
        )
		{
			var task = new GetClusterRankingReceivedRewardTask(
			    Gs2WebSocketSession,
			    request
            );
            yield return task;
            callback.Invoke(new AsyncResult<Result.GetClusterRankingReceivedRewardResult>(task.Result, task.Error));
        }

		public IFuture<Result.GetClusterRankingReceivedRewardResult> GetClusterRankingReceivedRewardFuture(
                Request.GetClusterRankingReceivedRewardRequest request
        )
		{
			return new GetClusterRankingReceivedRewardTask(
			    Gs2WebSocketSession,
			    request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.GetClusterRankingReceivedRewardResult> GetClusterRankingReceivedRewardAsync(
            Request.GetClusterRankingReceivedRewardRequest request
        )
		{
		    var task = new GetClusterRankingReceivedRewardTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
    #else
		public GetClusterRankingReceivedRewardTask GetClusterRankingReceivedRewardAsync(
                Request.GetClusterRankingReceivedRewardRequest request
        )
		{
			return new GetClusterRankingReceivedRewardTask(
                Gs2WebSocketSession,
			    request
            );
        }
    #endif
#else
		public async Task<Result.GetClusterRankingReceivedRewardResult> GetClusterRankingReceivedRewardAsync(
            Request.GetClusterRankingReceivedRewardRequest request
        )
		{
		    var task = new GetClusterRankingReceivedRewardTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
#endif


        public class GetClusterRankingReceivedRewardByUserIdTask : Gs2WebSocketSessionTask<Request.GetClusterRankingReceivedRewardByUserIdRequest, Result.GetClusterRankingReceivedRewardByUserIdResult>
        {
	        public GetClusterRankingReceivedRewardByUserIdTask(IGs2Session session, Request.GetClusterRankingReceivedRewardByUserIdRequest request) : base(session, request)
	        {
	        }

            protected override IGs2SessionRequest CreateRequest(Request.GetClusterRankingReceivedRewardByUserIdRequest request)
            {
                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);

                jsonWriter.WriteObjectStart();

                if (request.NamespaceName != null)
                {
                    jsonWriter.WritePropertyName("namespaceName");
                    jsonWriter.Write(request.NamespaceName.ToString());
                }
                if (request.RankingName != null)
                {
                    jsonWriter.WritePropertyName("rankingName");
                    jsonWriter.Write(request.RankingName.ToString());
                }
                if (request.ClusterName != null)
                {
                    jsonWriter.WritePropertyName("clusterName");
                    jsonWriter.Write(request.ClusterName.ToString());
                }
                if (request.UserId != null)
                {
                    jsonWriter.WritePropertyName("userId");
                    jsonWriter.Write(request.UserId.ToString());
                }
                if (request.Season != null)
                {
                    jsonWriter.WritePropertyName("season");
                    jsonWriter.Write(request.Season.ToString());
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
                    "ranking2",
                    "clusterRankingReceivedReward",
                    "getClusterRankingReceivedRewardByUserId",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator GetClusterRankingReceivedRewardByUserId(
                Request.GetClusterRankingReceivedRewardByUserIdRequest request,
                UnityAction<AsyncResult<Result.GetClusterRankingReceivedRewardByUserIdResult>> callback
        )
		{
			var task = new GetClusterRankingReceivedRewardByUserIdTask(
			    Gs2WebSocketSession,
			    request
            );
            yield return task;
            callback.Invoke(new AsyncResult<Result.GetClusterRankingReceivedRewardByUserIdResult>(task.Result, task.Error));
        }

		public IFuture<Result.GetClusterRankingReceivedRewardByUserIdResult> GetClusterRankingReceivedRewardByUserIdFuture(
                Request.GetClusterRankingReceivedRewardByUserIdRequest request
        )
		{
			return new GetClusterRankingReceivedRewardByUserIdTask(
			    Gs2WebSocketSession,
			    request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.GetClusterRankingReceivedRewardByUserIdResult> GetClusterRankingReceivedRewardByUserIdAsync(
            Request.GetClusterRankingReceivedRewardByUserIdRequest request
        )
		{
		    var task = new GetClusterRankingReceivedRewardByUserIdTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
    #else
		public GetClusterRankingReceivedRewardByUserIdTask GetClusterRankingReceivedRewardByUserIdAsync(
                Request.GetClusterRankingReceivedRewardByUserIdRequest request
        )
		{
			return new GetClusterRankingReceivedRewardByUserIdTask(
                Gs2WebSocketSession,
			    request
            );
        }
    #endif
#else
		public async Task<Result.GetClusterRankingReceivedRewardByUserIdResult> GetClusterRankingReceivedRewardByUserIdAsync(
            Request.GetClusterRankingReceivedRewardByUserIdRequest request
        )
		{
		    var task = new GetClusterRankingReceivedRewardByUserIdTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
#endif


        public class DeleteClusterRankingReceivedRewardByUserIdTask : Gs2WebSocketSessionTask<Request.DeleteClusterRankingReceivedRewardByUserIdRequest, Result.DeleteClusterRankingReceivedRewardByUserIdResult>
        {
	        public DeleteClusterRankingReceivedRewardByUserIdTask(IGs2Session session, Request.DeleteClusterRankingReceivedRewardByUserIdRequest request) : base(session, request)
	        {
	        }

            protected override IGs2SessionRequest CreateRequest(Request.DeleteClusterRankingReceivedRewardByUserIdRequest request)
            {
                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);

                jsonWriter.WriteObjectStart();

                if (request.NamespaceName != null)
                {
                    jsonWriter.WritePropertyName("namespaceName");
                    jsonWriter.Write(request.NamespaceName.ToString());
                }
                if (request.RankingName != null)
                {
                    jsonWriter.WritePropertyName("rankingName");
                    jsonWriter.Write(request.RankingName.ToString());
                }
                if (request.ClusterName != null)
                {
                    jsonWriter.WritePropertyName("clusterName");
                    jsonWriter.Write(request.ClusterName.ToString());
                }
                if (request.UserId != null)
                {
                    jsonWriter.WritePropertyName("userId");
                    jsonWriter.Write(request.UserId.ToString());
                }
                if (request.Season != null)
                {
                    jsonWriter.WritePropertyName("season");
                    jsonWriter.Write(request.Season.ToString());
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
                    "ranking2",
                    "clusterRankingReceivedReward",
                    "deleteClusterRankingReceivedRewardByUserId",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator DeleteClusterRankingReceivedRewardByUserId(
                Request.DeleteClusterRankingReceivedRewardByUserIdRequest request,
                UnityAction<AsyncResult<Result.DeleteClusterRankingReceivedRewardByUserIdResult>> callback
        )
		{
			var task = new DeleteClusterRankingReceivedRewardByUserIdTask(
			    Gs2WebSocketSession,
			    request
            );
            yield return task;
            callback.Invoke(new AsyncResult<Result.DeleteClusterRankingReceivedRewardByUserIdResult>(task.Result, task.Error));
        }

		public IFuture<Result.DeleteClusterRankingReceivedRewardByUserIdResult> DeleteClusterRankingReceivedRewardByUserIdFuture(
                Request.DeleteClusterRankingReceivedRewardByUserIdRequest request
        )
		{
			return new DeleteClusterRankingReceivedRewardByUserIdTask(
			    Gs2WebSocketSession,
			    request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.DeleteClusterRankingReceivedRewardByUserIdResult> DeleteClusterRankingReceivedRewardByUserIdAsync(
            Request.DeleteClusterRankingReceivedRewardByUserIdRequest request
        )
		{
		    var task = new DeleteClusterRankingReceivedRewardByUserIdTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
    #else
		public DeleteClusterRankingReceivedRewardByUserIdTask DeleteClusterRankingReceivedRewardByUserIdAsync(
                Request.DeleteClusterRankingReceivedRewardByUserIdRequest request
        )
		{
			return new DeleteClusterRankingReceivedRewardByUserIdTask(
                Gs2WebSocketSession,
			    request
            );
        }
    #endif
#else
		public async Task<Result.DeleteClusterRankingReceivedRewardByUserIdResult> DeleteClusterRankingReceivedRewardByUserIdAsync(
            Request.DeleteClusterRankingReceivedRewardByUserIdRequest request
        )
		{
		    var task = new DeleteClusterRankingReceivedRewardByUserIdTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
#endif


        public class GetClusterRankingTask : Gs2WebSocketSessionTask<Request.GetClusterRankingRequest, Result.GetClusterRankingResult>
        {
	        public GetClusterRankingTask(IGs2Session session, Request.GetClusterRankingRequest request) : base(session, request)
	        {
	        }

            protected override IGs2SessionRequest CreateRequest(Request.GetClusterRankingRequest request)
            {
                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);

                jsonWriter.WriteObjectStart();

                if (request.NamespaceName != null)
                {
                    jsonWriter.WritePropertyName("namespaceName");
                    jsonWriter.Write(request.NamespaceName.ToString());
                }
                if (request.RankingName != null)
                {
                    jsonWriter.WritePropertyName("rankingName");
                    jsonWriter.Write(request.RankingName.ToString());
                }
                if (request.ClusterName != null)
                {
                    jsonWriter.WritePropertyName("clusterName");
                    jsonWriter.Write(request.ClusterName.ToString());
                }
                if (request.AccessToken != null)
                {
                    jsonWriter.WritePropertyName("accessToken");
                    jsonWriter.Write(request.AccessToken.ToString());
                }
                if (request.Season != null)
                {
                    jsonWriter.WritePropertyName("season");
                    jsonWriter.Write(request.Season.ToString());
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
                    "ranking2",
                    "clusterRankingData",
                    "getClusterRanking",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator GetClusterRanking(
                Request.GetClusterRankingRequest request,
                UnityAction<AsyncResult<Result.GetClusterRankingResult>> callback
        )
		{
			var task = new GetClusterRankingTask(
			    Gs2WebSocketSession,
			    request
            );
            yield return task;
            callback.Invoke(new AsyncResult<Result.GetClusterRankingResult>(task.Result, task.Error));
        }

		public IFuture<Result.GetClusterRankingResult> GetClusterRankingFuture(
                Request.GetClusterRankingRequest request
        )
		{
			return new GetClusterRankingTask(
			    Gs2WebSocketSession,
			    request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.GetClusterRankingResult> GetClusterRankingAsync(
            Request.GetClusterRankingRequest request
        )
		{
		    var task = new GetClusterRankingTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
    #else
		public GetClusterRankingTask GetClusterRankingAsync(
                Request.GetClusterRankingRequest request
        )
		{
			return new GetClusterRankingTask(
                Gs2WebSocketSession,
			    request
            );
        }
    #endif
#else
		public async Task<Result.GetClusterRankingResult> GetClusterRankingAsync(
            Request.GetClusterRankingRequest request
        )
		{
		    var task = new GetClusterRankingTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
#endif


        public class GetClusterRankingByUserIdTask : Gs2WebSocketSessionTask<Request.GetClusterRankingByUserIdRequest, Result.GetClusterRankingByUserIdResult>
        {
	        public GetClusterRankingByUserIdTask(IGs2Session session, Request.GetClusterRankingByUserIdRequest request) : base(session, request)
	        {
	        }

            protected override IGs2SessionRequest CreateRequest(Request.GetClusterRankingByUserIdRequest request)
            {
                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);

                jsonWriter.WriteObjectStart();

                if (request.NamespaceName != null)
                {
                    jsonWriter.WritePropertyName("namespaceName");
                    jsonWriter.Write(request.NamespaceName.ToString());
                }
                if (request.RankingName != null)
                {
                    jsonWriter.WritePropertyName("rankingName");
                    jsonWriter.Write(request.RankingName.ToString());
                }
                if (request.ClusterName != null)
                {
                    jsonWriter.WritePropertyName("clusterName");
                    jsonWriter.Write(request.ClusterName.ToString());
                }
                if (request.UserId != null)
                {
                    jsonWriter.WritePropertyName("userId");
                    jsonWriter.Write(request.UserId.ToString());
                }
                if (request.Season != null)
                {
                    jsonWriter.WritePropertyName("season");
                    jsonWriter.Write(request.Season.ToString());
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
                    "ranking2",
                    "clusterRankingData",
                    "getClusterRankingByUserId",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator GetClusterRankingByUserId(
                Request.GetClusterRankingByUserIdRequest request,
                UnityAction<AsyncResult<Result.GetClusterRankingByUserIdResult>> callback
        )
		{
			var task = new GetClusterRankingByUserIdTask(
			    Gs2WebSocketSession,
			    request
            );
            yield return task;
            callback.Invoke(new AsyncResult<Result.GetClusterRankingByUserIdResult>(task.Result, task.Error));
        }

		public IFuture<Result.GetClusterRankingByUserIdResult> GetClusterRankingByUserIdFuture(
                Request.GetClusterRankingByUserIdRequest request
        )
		{
			return new GetClusterRankingByUserIdTask(
			    Gs2WebSocketSession,
			    request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.GetClusterRankingByUserIdResult> GetClusterRankingByUserIdAsync(
            Request.GetClusterRankingByUserIdRequest request
        )
		{
		    var task = new GetClusterRankingByUserIdTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
    #else
		public GetClusterRankingByUserIdTask GetClusterRankingByUserIdAsync(
                Request.GetClusterRankingByUserIdRequest request
        )
		{
			return new GetClusterRankingByUserIdTask(
                Gs2WebSocketSession,
			    request
            );
        }
    #endif
#else
		public async Task<Result.GetClusterRankingByUserIdResult> GetClusterRankingByUserIdAsync(
            Request.GetClusterRankingByUserIdRequest request
        )
		{
		    var task = new GetClusterRankingByUserIdTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
#endif


        public class GetSubscribeRankingModelTask : Gs2WebSocketSessionTask<Request.GetSubscribeRankingModelRequest, Result.GetSubscribeRankingModelResult>
        {
	        public GetSubscribeRankingModelTask(IGs2Session session, Request.GetSubscribeRankingModelRequest request) : base(session, request)
	        {
	        }

            protected override IGs2SessionRequest CreateRequest(Request.GetSubscribeRankingModelRequest request)
            {
                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);

                jsonWriter.WriteObjectStart();

                if (request.NamespaceName != null)
                {
                    jsonWriter.WritePropertyName("namespaceName");
                    jsonWriter.Write(request.NamespaceName.ToString());
                }
                if (request.RankingName != null)
                {
                    jsonWriter.WritePropertyName("rankingName");
                    jsonWriter.Write(request.RankingName.ToString());
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
                    "ranking2",
                    "subscribeRankingModel",
                    "getSubscribeRankingModel",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator GetSubscribeRankingModel(
                Request.GetSubscribeRankingModelRequest request,
                UnityAction<AsyncResult<Result.GetSubscribeRankingModelResult>> callback
        )
		{
			var task = new GetSubscribeRankingModelTask(
			    Gs2WebSocketSession,
			    request
            );
            yield return task;
            callback.Invoke(new AsyncResult<Result.GetSubscribeRankingModelResult>(task.Result, task.Error));
        }

		public IFuture<Result.GetSubscribeRankingModelResult> GetSubscribeRankingModelFuture(
                Request.GetSubscribeRankingModelRequest request
        )
		{
			return new GetSubscribeRankingModelTask(
			    Gs2WebSocketSession,
			    request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.GetSubscribeRankingModelResult> GetSubscribeRankingModelAsync(
            Request.GetSubscribeRankingModelRequest request
        )
		{
		    var task = new GetSubscribeRankingModelTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
    #else
		public GetSubscribeRankingModelTask GetSubscribeRankingModelAsync(
                Request.GetSubscribeRankingModelRequest request
        )
		{
			return new GetSubscribeRankingModelTask(
                Gs2WebSocketSession,
			    request
            );
        }
    #endif
#else
		public async Task<Result.GetSubscribeRankingModelResult> GetSubscribeRankingModelAsync(
            Request.GetSubscribeRankingModelRequest request
        )
		{
		    var task = new GetSubscribeRankingModelTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
#endif


        public class CreateSubscribeRankingModelMasterTask : Gs2WebSocketSessionTask<Request.CreateSubscribeRankingModelMasterRequest, Result.CreateSubscribeRankingModelMasterResult>
        {
	        public CreateSubscribeRankingModelMasterTask(IGs2Session session, Request.CreateSubscribeRankingModelMasterRequest request) : base(session, request)
	        {
	        }

            protected override IGs2SessionRequest CreateRequest(Request.CreateSubscribeRankingModelMasterRequest request)
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
                if (request.MinimumValue != null)
                {
                    jsonWriter.WritePropertyName("minimumValue");
                    jsonWriter.Write(request.MinimumValue.ToString());
                }
                if (request.MaximumValue != null)
                {
                    jsonWriter.WritePropertyName("maximumValue");
                    jsonWriter.Write(request.MaximumValue.ToString());
                }
                if (request.Sum != null)
                {
                    jsonWriter.WritePropertyName("sum");
                    jsonWriter.Write(request.Sum.ToString());
                }
                if (request.ScoreTtlDays != null)
                {
                    jsonWriter.WritePropertyName("scoreTtlDays");
                    jsonWriter.Write(request.ScoreTtlDays.ToString());
                }
                if (request.OrderDirection != null)
                {
                    jsonWriter.WritePropertyName("orderDirection");
                    jsonWriter.Write(request.OrderDirection.ToString());
                }
                if (request.EntryPeriodEventId != null)
                {
                    jsonWriter.WritePropertyName("entryPeriodEventId");
                    jsonWriter.Write(request.EntryPeriodEventId.ToString());
                }
                if (request.AccessPeriodEventId != null)
                {
                    jsonWriter.WritePropertyName("accessPeriodEventId");
                    jsonWriter.Write(request.AccessPeriodEventId.ToString());
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
                    "ranking2",
                    "subscribeRankingModelMaster",
                    "createSubscribeRankingModelMaster",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator CreateSubscribeRankingModelMaster(
                Request.CreateSubscribeRankingModelMasterRequest request,
                UnityAction<AsyncResult<Result.CreateSubscribeRankingModelMasterResult>> callback
        )
		{
			var task = new CreateSubscribeRankingModelMasterTask(
			    Gs2WebSocketSession,
			    request
            );
            yield return task;
            callback.Invoke(new AsyncResult<Result.CreateSubscribeRankingModelMasterResult>(task.Result, task.Error));
        }

		public IFuture<Result.CreateSubscribeRankingModelMasterResult> CreateSubscribeRankingModelMasterFuture(
                Request.CreateSubscribeRankingModelMasterRequest request
        )
		{
			return new CreateSubscribeRankingModelMasterTask(
			    Gs2WebSocketSession,
			    request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.CreateSubscribeRankingModelMasterResult> CreateSubscribeRankingModelMasterAsync(
            Request.CreateSubscribeRankingModelMasterRequest request
        )
		{
		    var task = new CreateSubscribeRankingModelMasterTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
    #else
		public CreateSubscribeRankingModelMasterTask CreateSubscribeRankingModelMasterAsync(
                Request.CreateSubscribeRankingModelMasterRequest request
        )
		{
			return new CreateSubscribeRankingModelMasterTask(
                Gs2WebSocketSession,
			    request
            );
        }
    #endif
#else
		public async Task<Result.CreateSubscribeRankingModelMasterResult> CreateSubscribeRankingModelMasterAsync(
            Request.CreateSubscribeRankingModelMasterRequest request
        )
		{
		    var task = new CreateSubscribeRankingModelMasterTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
#endif


        public class GetSubscribeRankingModelMasterTask : Gs2WebSocketSessionTask<Request.GetSubscribeRankingModelMasterRequest, Result.GetSubscribeRankingModelMasterResult>
        {
	        public GetSubscribeRankingModelMasterTask(IGs2Session session, Request.GetSubscribeRankingModelMasterRequest request) : base(session, request)
	        {
	        }

            protected override IGs2SessionRequest CreateRequest(Request.GetSubscribeRankingModelMasterRequest request)
            {
                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);

                jsonWriter.WriteObjectStart();

                if (request.NamespaceName != null)
                {
                    jsonWriter.WritePropertyName("namespaceName");
                    jsonWriter.Write(request.NamespaceName.ToString());
                }
                if (request.RankingName != null)
                {
                    jsonWriter.WritePropertyName("rankingName");
                    jsonWriter.Write(request.RankingName.ToString());
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
                    "ranking2",
                    "subscribeRankingModelMaster",
                    "getSubscribeRankingModelMaster",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator GetSubscribeRankingModelMaster(
                Request.GetSubscribeRankingModelMasterRequest request,
                UnityAction<AsyncResult<Result.GetSubscribeRankingModelMasterResult>> callback
        )
		{
			var task = new GetSubscribeRankingModelMasterTask(
			    Gs2WebSocketSession,
			    request
            );
            yield return task;
            callback.Invoke(new AsyncResult<Result.GetSubscribeRankingModelMasterResult>(task.Result, task.Error));
        }

		public IFuture<Result.GetSubscribeRankingModelMasterResult> GetSubscribeRankingModelMasterFuture(
                Request.GetSubscribeRankingModelMasterRequest request
        )
		{
			return new GetSubscribeRankingModelMasterTask(
			    Gs2WebSocketSession,
			    request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.GetSubscribeRankingModelMasterResult> GetSubscribeRankingModelMasterAsync(
            Request.GetSubscribeRankingModelMasterRequest request
        )
		{
		    var task = new GetSubscribeRankingModelMasterTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
    #else
		public GetSubscribeRankingModelMasterTask GetSubscribeRankingModelMasterAsync(
                Request.GetSubscribeRankingModelMasterRequest request
        )
		{
			return new GetSubscribeRankingModelMasterTask(
                Gs2WebSocketSession,
			    request
            );
        }
    #endif
#else
		public async Task<Result.GetSubscribeRankingModelMasterResult> GetSubscribeRankingModelMasterAsync(
            Request.GetSubscribeRankingModelMasterRequest request
        )
		{
		    var task = new GetSubscribeRankingModelMasterTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
#endif


        public class UpdateSubscribeRankingModelMasterTask : Gs2WebSocketSessionTask<Request.UpdateSubscribeRankingModelMasterRequest, Result.UpdateSubscribeRankingModelMasterResult>
        {
	        public UpdateSubscribeRankingModelMasterTask(IGs2Session session, Request.UpdateSubscribeRankingModelMasterRequest request) : base(session, request)
	        {
	        }

            protected override IGs2SessionRequest CreateRequest(Request.UpdateSubscribeRankingModelMasterRequest request)
            {
                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);

                jsonWriter.WriteObjectStart();

                if (request.NamespaceName != null)
                {
                    jsonWriter.WritePropertyName("namespaceName");
                    jsonWriter.Write(request.NamespaceName.ToString());
                }
                if (request.RankingName != null)
                {
                    jsonWriter.WritePropertyName("rankingName");
                    jsonWriter.Write(request.RankingName.ToString());
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
                if (request.MinimumValue != null)
                {
                    jsonWriter.WritePropertyName("minimumValue");
                    jsonWriter.Write(request.MinimumValue.ToString());
                }
                if (request.MaximumValue != null)
                {
                    jsonWriter.WritePropertyName("maximumValue");
                    jsonWriter.Write(request.MaximumValue.ToString());
                }
                if (request.Sum != null)
                {
                    jsonWriter.WritePropertyName("sum");
                    jsonWriter.Write(request.Sum.ToString());
                }
                if (request.ScoreTtlDays != null)
                {
                    jsonWriter.WritePropertyName("scoreTtlDays");
                    jsonWriter.Write(request.ScoreTtlDays.ToString());
                }
                if (request.OrderDirection != null)
                {
                    jsonWriter.WritePropertyName("orderDirection");
                    jsonWriter.Write(request.OrderDirection.ToString());
                }
                if (request.EntryPeriodEventId != null)
                {
                    jsonWriter.WritePropertyName("entryPeriodEventId");
                    jsonWriter.Write(request.EntryPeriodEventId.ToString());
                }
                if (request.AccessPeriodEventId != null)
                {
                    jsonWriter.WritePropertyName("accessPeriodEventId");
                    jsonWriter.Write(request.AccessPeriodEventId.ToString());
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
                    "ranking2",
                    "subscribeRankingModelMaster",
                    "updateSubscribeRankingModelMaster",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator UpdateSubscribeRankingModelMaster(
                Request.UpdateSubscribeRankingModelMasterRequest request,
                UnityAction<AsyncResult<Result.UpdateSubscribeRankingModelMasterResult>> callback
        )
		{
			var task = new UpdateSubscribeRankingModelMasterTask(
			    Gs2WebSocketSession,
			    request
            );
            yield return task;
            callback.Invoke(new AsyncResult<Result.UpdateSubscribeRankingModelMasterResult>(task.Result, task.Error));
        }

		public IFuture<Result.UpdateSubscribeRankingModelMasterResult> UpdateSubscribeRankingModelMasterFuture(
                Request.UpdateSubscribeRankingModelMasterRequest request
        )
		{
			return new UpdateSubscribeRankingModelMasterTask(
			    Gs2WebSocketSession,
			    request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.UpdateSubscribeRankingModelMasterResult> UpdateSubscribeRankingModelMasterAsync(
            Request.UpdateSubscribeRankingModelMasterRequest request
        )
		{
		    var task = new UpdateSubscribeRankingModelMasterTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
    #else
		public UpdateSubscribeRankingModelMasterTask UpdateSubscribeRankingModelMasterAsync(
                Request.UpdateSubscribeRankingModelMasterRequest request
        )
		{
			return new UpdateSubscribeRankingModelMasterTask(
                Gs2WebSocketSession,
			    request
            );
        }
    #endif
#else
		public async Task<Result.UpdateSubscribeRankingModelMasterResult> UpdateSubscribeRankingModelMasterAsync(
            Request.UpdateSubscribeRankingModelMasterRequest request
        )
		{
		    var task = new UpdateSubscribeRankingModelMasterTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
#endif


        public class DeleteSubscribeRankingModelMasterTask : Gs2WebSocketSessionTask<Request.DeleteSubscribeRankingModelMasterRequest, Result.DeleteSubscribeRankingModelMasterResult>
        {
	        public DeleteSubscribeRankingModelMasterTask(IGs2Session session, Request.DeleteSubscribeRankingModelMasterRequest request) : base(session, request)
	        {
	        }

            protected override IGs2SessionRequest CreateRequest(Request.DeleteSubscribeRankingModelMasterRequest request)
            {
                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);

                jsonWriter.WriteObjectStart();

                if (request.NamespaceName != null)
                {
                    jsonWriter.WritePropertyName("namespaceName");
                    jsonWriter.Write(request.NamespaceName.ToString());
                }
                if (request.RankingName != null)
                {
                    jsonWriter.WritePropertyName("rankingName");
                    jsonWriter.Write(request.RankingName.ToString());
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
                    "ranking2",
                    "subscribeRankingModelMaster",
                    "deleteSubscribeRankingModelMaster",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator DeleteSubscribeRankingModelMaster(
                Request.DeleteSubscribeRankingModelMasterRequest request,
                UnityAction<AsyncResult<Result.DeleteSubscribeRankingModelMasterResult>> callback
        )
		{
			var task = new DeleteSubscribeRankingModelMasterTask(
			    Gs2WebSocketSession,
			    request
            );
            yield return task;
            callback.Invoke(new AsyncResult<Result.DeleteSubscribeRankingModelMasterResult>(task.Result, task.Error));
        }

		public IFuture<Result.DeleteSubscribeRankingModelMasterResult> DeleteSubscribeRankingModelMasterFuture(
                Request.DeleteSubscribeRankingModelMasterRequest request
        )
		{
			return new DeleteSubscribeRankingModelMasterTask(
			    Gs2WebSocketSession,
			    request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.DeleteSubscribeRankingModelMasterResult> DeleteSubscribeRankingModelMasterAsync(
            Request.DeleteSubscribeRankingModelMasterRequest request
        )
		{
		    var task = new DeleteSubscribeRankingModelMasterTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
    #else
		public DeleteSubscribeRankingModelMasterTask DeleteSubscribeRankingModelMasterAsync(
                Request.DeleteSubscribeRankingModelMasterRequest request
        )
		{
			return new DeleteSubscribeRankingModelMasterTask(
                Gs2WebSocketSession,
			    request
            );
        }
    #endif
#else
		public async Task<Result.DeleteSubscribeRankingModelMasterResult> DeleteSubscribeRankingModelMasterAsync(
            Request.DeleteSubscribeRankingModelMasterRequest request
        )
		{
		    var task = new DeleteSubscribeRankingModelMasterTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
#endif


        public class AddSubscribeTask : Gs2WebSocketSessionTask<Request.AddSubscribeRequest, Result.AddSubscribeResult>
        {
	        public AddSubscribeTask(IGs2Session session, Request.AddSubscribeRequest request) : base(session, request)
	        {
	        }

            protected override IGs2SessionRequest CreateRequest(Request.AddSubscribeRequest request)
            {
                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);

                jsonWriter.WriteObjectStart();

                if (request.NamespaceName != null)
                {
                    jsonWriter.WritePropertyName("namespaceName");
                    jsonWriter.Write(request.NamespaceName.ToString());
                }
                if (request.RankingName != null)
                {
                    jsonWriter.WritePropertyName("rankingName");
                    jsonWriter.Write(request.RankingName.ToString());
                }
                if (request.AccessToken != null)
                {
                    jsonWriter.WritePropertyName("accessToken");
                    jsonWriter.Write(request.AccessToken.ToString());
                }
                if (request.TargetUserId != null)
                {
                    jsonWriter.WritePropertyName("targetUserId");
                    jsonWriter.Write(request.TargetUserId.ToString());
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
                    "ranking2",
                    "subscribe",
                    "addSubscribe",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator AddSubscribe(
                Request.AddSubscribeRequest request,
                UnityAction<AsyncResult<Result.AddSubscribeResult>> callback
        )
		{
			var task = new AddSubscribeTask(
			    Gs2WebSocketSession,
			    request
            );
            yield return task;
            callback.Invoke(new AsyncResult<Result.AddSubscribeResult>(task.Result, task.Error));
        }

		public IFuture<Result.AddSubscribeResult> AddSubscribeFuture(
                Request.AddSubscribeRequest request
        )
		{
			return new AddSubscribeTask(
			    Gs2WebSocketSession,
			    request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.AddSubscribeResult> AddSubscribeAsync(
            Request.AddSubscribeRequest request
        )
		{
		    var task = new AddSubscribeTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
    #else
		public AddSubscribeTask AddSubscribeAsync(
                Request.AddSubscribeRequest request
        )
		{
			return new AddSubscribeTask(
                Gs2WebSocketSession,
			    request
            );
        }
    #endif
#else
		public async Task<Result.AddSubscribeResult> AddSubscribeAsync(
            Request.AddSubscribeRequest request
        )
		{
		    var task = new AddSubscribeTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
#endif


        public class AddSubscribeByUserIdTask : Gs2WebSocketSessionTask<Request.AddSubscribeByUserIdRequest, Result.AddSubscribeByUserIdResult>
        {
	        public AddSubscribeByUserIdTask(IGs2Session session, Request.AddSubscribeByUserIdRequest request) : base(session, request)
	        {
	        }

            protected override IGs2SessionRequest CreateRequest(Request.AddSubscribeByUserIdRequest request)
            {
                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);

                jsonWriter.WriteObjectStart();

                if (request.NamespaceName != null)
                {
                    jsonWriter.WritePropertyName("namespaceName");
                    jsonWriter.Write(request.NamespaceName.ToString());
                }
                if (request.RankingName != null)
                {
                    jsonWriter.WritePropertyName("rankingName");
                    jsonWriter.Write(request.RankingName.ToString());
                }
                if (request.UserId != null)
                {
                    jsonWriter.WritePropertyName("userId");
                    jsonWriter.Write(request.UserId.ToString());
                }
                if (request.TargetUserId != null)
                {
                    jsonWriter.WritePropertyName("targetUserId");
                    jsonWriter.Write(request.TargetUserId.ToString());
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
                    "ranking2",
                    "subscribe",
                    "addSubscribeByUserId",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator AddSubscribeByUserId(
                Request.AddSubscribeByUserIdRequest request,
                UnityAction<AsyncResult<Result.AddSubscribeByUserIdResult>> callback
        )
		{
			var task = new AddSubscribeByUserIdTask(
			    Gs2WebSocketSession,
			    request
            );
            yield return task;
            callback.Invoke(new AsyncResult<Result.AddSubscribeByUserIdResult>(task.Result, task.Error));
        }

		public IFuture<Result.AddSubscribeByUserIdResult> AddSubscribeByUserIdFuture(
                Request.AddSubscribeByUserIdRequest request
        )
		{
			return new AddSubscribeByUserIdTask(
			    Gs2WebSocketSession,
			    request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.AddSubscribeByUserIdResult> AddSubscribeByUserIdAsync(
            Request.AddSubscribeByUserIdRequest request
        )
		{
		    var task = new AddSubscribeByUserIdTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
    #else
		public AddSubscribeByUserIdTask AddSubscribeByUserIdAsync(
                Request.AddSubscribeByUserIdRequest request
        )
		{
			return new AddSubscribeByUserIdTask(
                Gs2WebSocketSession,
			    request
            );
        }
    #endif
#else
		public async Task<Result.AddSubscribeByUserIdResult> AddSubscribeByUserIdAsync(
            Request.AddSubscribeByUserIdRequest request
        )
		{
		    var task = new AddSubscribeByUserIdTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
#endif


        public class PutSubscribeRankingScoreTask : Gs2WebSocketSessionTask<Request.PutSubscribeRankingScoreRequest, Result.PutSubscribeRankingScoreResult>
        {
	        public PutSubscribeRankingScoreTask(IGs2Session session, Request.PutSubscribeRankingScoreRequest request) : base(session, request)
	        {
	        }

            protected override IGs2SessionRequest CreateRequest(Request.PutSubscribeRankingScoreRequest request)
            {
                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);

                jsonWriter.WriteObjectStart();

                if (request.NamespaceName != null)
                {
                    jsonWriter.WritePropertyName("namespaceName");
                    jsonWriter.Write(request.NamespaceName.ToString());
                }
                if (request.RankingName != null)
                {
                    jsonWriter.WritePropertyName("rankingName");
                    jsonWriter.Write(request.RankingName.ToString());
                }
                if (request.AccessToken != null)
                {
                    jsonWriter.WritePropertyName("accessToken");
                    jsonWriter.Write(request.AccessToken.ToString());
                }
                if (request.Score != null)
                {
                    jsonWriter.WritePropertyName("score");
                    jsonWriter.Write(request.Score.ToString());
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
                    "ranking2",
                    "subscribeRankingScore",
                    "putSubscribeRankingScore",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator PutSubscribeRankingScore(
                Request.PutSubscribeRankingScoreRequest request,
                UnityAction<AsyncResult<Result.PutSubscribeRankingScoreResult>> callback
        )
		{
			var task = new PutSubscribeRankingScoreTask(
			    Gs2WebSocketSession,
			    request
            );
            yield return task;
            callback.Invoke(new AsyncResult<Result.PutSubscribeRankingScoreResult>(task.Result, task.Error));
        }

		public IFuture<Result.PutSubscribeRankingScoreResult> PutSubscribeRankingScoreFuture(
                Request.PutSubscribeRankingScoreRequest request
        )
		{
			return new PutSubscribeRankingScoreTask(
			    Gs2WebSocketSession,
			    request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.PutSubscribeRankingScoreResult> PutSubscribeRankingScoreAsync(
            Request.PutSubscribeRankingScoreRequest request
        )
		{
		    var task = new PutSubscribeRankingScoreTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
    #else
		public PutSubscribeRankingScoreTask PutSubscribeRankingScoreAsync(
                Request.PutSubscribeRankingScoreRequest request
        )
		{
			return new PutSubscribeRankingScoreTask(
                Gs2WebSocketSession,
			    request
            );
        }
    #endif
#else
		public async Task<Result.PutSubscribeRankingScoreResult> PutSubscribeRankingScoreAsync(
            Request.PutSubscribeRankingScoreRequest request
        )
		{
		    var task = new PutSubscribeRankingScoreTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
#endif


        public class PutSubscribeRankingScoreByUserIdTask : Gs2WebSocketSessionTask<Request.PutSubscribeRankingScoreByUserIdRequest, Result.PutSubscribeRankingScoreByUserIdResult>
        {
	        public PutSubscribeRankingScoreByUserIdTask(IGs2Session session, Request.PutSubscribeRankingScoreByUserIdRequest request) : base(session, request)
	        {
	        }

            protected override IGs2SessionRequest CreateRequest(Request.PutSubscribeRankingScoreByUserIdRequest request)
            {
                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);

                jsonWriter.WriteObjectStart();

                if (request.NamespaceName != null)
                {
                    jsonWriter.WritePropertyName("namespaceName");
                    jsonWriter.Write(request.NamespaceName.ToString());
                }
                if (request.RankingName != null)
                {
                    jsonWriter.WritePropertyName("rankingName");
                    jsonWriter.Write(request.RankingName.ToString());
                }
                if (request.UserId != null)
                {
                    jsonWriter.WritePropertyName("userId");
                    jsonWriter.Write(request.UserId.ToString());
                }
                if (request.Score != null)
                {
                    jsonWriter.WritePropertyName("score");
                    jsonWriter.Write(request.Score.ToString());
                }
                if (request.Metadata != null)
                {
                    jsonWriter.WritePropertyName("metadata");
                    jsonWriter.Write(request.Metadata.ToString());
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
                    "ranking2",
                    "subscribeRankingScore",
                    "putSubscribeRankingScoreByUserId",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator PutSubscribeRankingScoreByUserId(
                Request.PutSubscribeRankingScoreByUserIdRequest request,
                UnityAction<AsyncResult<Result.PutSubscribeRankingScoreByUserIdResult>> callback
        )
		{
			var task = new PutSubscribeRankingScoreByUserIdTask(
			    Gs2WebSocketSession,
			    request
            );
            yield return task;
            callback.Invoke(new AsyncResult<Result.PutSubscribeRankingScoreByUserIdResult>(task.Result, task.Error));
        }

		public IFuture<Result.PutSubscribeRankingScoreByUserIdResult> PutSubscribeRankingScoreByUserIdFuture(
                Request.PutSubscribeRankingScoreByUserIdRequest request
        )
		{
			return new PutSubscribeRankingScoreByUserIdTask(
			    Gs2WebSocketSession,
			    request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.PutSubscribeRankingScoreByUserIdResult> PutSubscribeRankingScoreByUserIdAsync(
            Request.PutSubscribeRankingScoreByUserIdRequest request
        )
		{
		    var task = new PutSubscribeRankingScoreByUserIdTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
    #else
		public PutSubscribeRankingScoreByUserIdTask PutSubscribeRankingScoreByUserIdAsync(
                Request.PutSubscribeRankingScoreByUserIdRequest request
        )
		{
			return new PutSubscribeRankingScoreByUserIdTask(
                Gs2WebSocketSession,
			    request
            );
        }
    #endif
#else
		public async Task<Result.PutSubscribeRankingScoreByUserIdResult> PutSubscribeRankingScoreByUserIdAsync(
            Request.PutSubscribeRankingScoreByUserIdRequest request
        )
		{
		    var task = new PutSubscribeRankingScoreByUserIdTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
#endif


        public class GetSubscribeRankingScoreTask : Gs2WebSocketSessionTask<Request.GetSubscribeRankingScoreRequest, Result.GetSubscribeRankingScoreResult>
        {
	        public GetSubscribeRankingScoreTask(IGs2Session session, Request.GetSubscribeRankingScoreRequest request) : base(session, request)
	        {
	        }

            protected override IGs2SessionRequest CreateRequest(Request.GetSubscribeRankingScoreRequest request)
            {
                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);

                jsonWriter.WriteObjectStart();

                if (request.NamespaceName != null)
                {
                    jsonWriter.WritePropertyName("namespaceName");
                    jsonWriter.Write(request.NamespaceName.ToString());
                }
                if (request.RankingName != null)
                {
                    jsonWriter.WritePropertyName("rankingName");
                    jsonWriter.Write(request.RankingName.ToString());
                }
                if (request.AccessToken != null)
                {
                    jsonWriter.WritePropertyName("accessToken");
                    jsonWriter.Write(request.AccessToken.ToString());
                }
                if (request.Season != null)
                {
                    jsonWriter.WritePropertyName("season");
                    jsonWriter.Write(request.Season.ToString());
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
                    "ranking2",
                    "subscribeRankingScore",
                    "getSubscribeRankingScore",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator GetSubscribeRankingScore(
                Request.GetSubscribeRankingScoreRequest request,
                UnityAction<AsyncResult<Result.GetSubscribeRankingScoreResult>> callback
        )
		{
			var task = new GetSubscribeRankingScoreTask(
			    Gs2WebSocketSession,
			    request
            );
            yield return task;
            callback.Invoke(new AsyncResult<Result.GetSubscribeRankingScoreResult>(task.Result, task.Error));
        }

		public IFuture<Result.GetSubscribeRankingScoreResult> GetSubscribeRankingScoreFuture(
                Request.GetSubscribeRankingScoreRequest request
        )
		{
			return new GetSubscribeRankingScoreTask(
			    Gs2WebSocketSession,
			    request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.GetSubscribeRankingScoreResult> GetSubscribeRankingScoreAsync(
            Request.GetSubscribeRankingScoreRequest request
        )
		{
		    var task = new GetSubscribeRankingScoreTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
    #else
		public GetSubscribeRankingScoreTask GetSubscribeRankingScoreAsync(
                Request.GetSubscribeRankingScoreRequest request
        )
		{
			return new GetSubscribeRankingScoreTask(
                Gs2WebSocketSession,
			    request
            );
        }
    #endif
#else
		public async Task<Result.GetSubscribeRankingScoreResult> GetSubscribeRankingScoreAsync(
            Request.GetSubscribeRankingScoreRequest request
        )
		{
		    var task = new GetSubscribeRankingScoreTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
#endif


        public class GetSubscribeRankingScoreByUserIdTask : Gs2WebSocketSessionTask<Request.GetSubscribeRankingScoreByUserIdRequest, Result.GetSubscribeRankingScoreByUserIdResult>
        {
	        public GetSubscribeRankingScoreByUserIdTask(IGs2Session session, Request.GetSubscribeRankingScoreByUserIdRequest request) : base(session, request)
	        {
	        }

            protected override IGs2SessionRequest CreateRequest(Request.GetSubscribeRankingScoreByUserIdRequest request)
            {
                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);

                jsonWriter.WriteObjectStart();

                if (request.NamespaceName != null)
                {
                    jsonWriter.WritePropertyName("namespaceName");
                    jsonWriter.Write(request.NamespaceName.ToString());
                }
                if (request.RankingName != null)
                {
                    jsonWriter.WritePropertyName("rankingName");
                    jsonWriter.Write(request.RankingName.ToString());
                }
                if (request.UserId != null)
                {
                    jsonWriter.WritePropertyName("userId");
                    jsonWriter.Write(request.UserId.ToString());
                }
                if (request.Season != null)
                {
                    jsonWriter.WritePropertyName("season");
                    jsonWriter.Write(request.Season.ToString());
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
                    "ranking2",
                    "subscribeRankingScore",
                    "getSubscribeRankingScoreByUserId",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator GetSubscribeRankingScoreByUserId(
                Request.GetSubscribeRankingScoreByUserIdRequest request,
                UnityAction<AsyncResult<Result.GetSubscribeRankingScoreByUserIdResult>> callback
        )
		{
			var task = new GetSubscribeRankingScoreByUserIdTask(
			    Gs2WebSocketSession,
			    request
            );
            yield return task;
            callback.Invoke(new AsyncResult<Result.GetSubscribeRankingScoreByUserIdResult>(task.Result, task.Error));
        }

		public IFuture<Result.GetSubscribeRankingScoreByUserIdResult> GetSubscribeRankingScoreByUserIdFuture(
                Request.GetSubscribeRankingScoreByUserIdRequest request
        )
		{
			return new GetSubscribeRankingScoreByUserIdTask(
			    Gs2WebSocketSession,
			    request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.GetSubscribeRankingScoreByUserIdResult> GetSubscribeRankingScoreByUserIdAsync(
            Request.GetSubscribeRankingScoreByUserIdRequest request
        )
		{
		    var task = new GetSubscribeRankingScoreByUserIdTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
    #else
		public GetSubscribeRankingScoreByUserIdTask GetSubscribeRankingScoreByUserIdAsync(
                Request.GetSubscribeRankingScoreByUserIdRequest request
        )
		{
			return new GetSubscribeRankingScoreByUserIdTask(
                Gs2WebSocketSession,
			    request
            );
        }
    #endif
#else
		public async Task<Result.GetSubscribeRankingScoreByUserIdResult> GetSubscribeRankingScoreByUserIdAsync(
            Request.GetSubscribeRankingScoreByUserIdRequest request
        )
		{
		    var task = new GetSubscribeRankingScoreByUserIdTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
#endif


        public class DeleteSubscribeRankingScoreByUserIdTask : Gs2WebSocketSessionTask<Request.DeleteSubscribeRankingScoreByUserIdRequest, Result.DeleteSubscribeRankingScoreByUserIdResult>
        {
	        public DeleteSubscribeRankingScoreByUserIdTask(IGs2Session session, Request.DeleteSubscribeRankingScoreByUserIdRequest request) : base(session, request)
	        {
	        }

            protected override IGs2SessionRequest CreateRequest(Request.DeleteSubscribeRankingScoreByUserIdRequest request)
            {
                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);

                jsonWriter.WriteObjectStart();

                if (request.NamespaceName != null)
                {
                    jsonWriter.WritePropertyName("namespaceName");
                    jsonWriter.Write(request.NamespaceName.ToString());
                }
                if (request.RankingName != null)
                {
                    jsonWriter.WritePropertyName("rankingName");
                    jsonWriter.Write(request.RankingName.ToString());
                }
                if (request.UserId != null)
                {
                    jsonWriter.WritePropertyName("userId");
                    jsonWriter.Write(request.UserId.ToString());
                }
                if (request.Season != null)
                {
                    jsonWriter.WritePropertyName("season");
                    jsonWriter.Write(request.Season.ToString());
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
                    "ranking2",
                    "subscribeRankingScore",
                    "deleteSubscribeRankingScoreByUserId",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator DeleteSubscribeRankingScoreByUserId(
                Request.DeleteSubscribeRankingScoreByUserIdRequest request,
                UnityAction<AsyncResult<Result.DeleteSubscribeRankingScoreByUserIdResult>> callback
        )
		{
			var task = new DeleteSubscribeRankingScoreByUserIdTask(
			    Gs2WebSocketSession,
			    request
            );
            yield return task;
            callback.Invoke(new AsyncResult<Result.DeleteSubscribeRankingScoreByUserIdResult>(task.Result, task.Error));
        }

		public IFuture<Result.DeleteSubscribeRankingScoreByUserIdResult> DeleteSubscribeRankingScoreByUserIdFuture(
                Request.DeleteSubscribeRankingScoreByUserIdRequest request
        )
		{
			return new DeleteSubscribeRankingScoreByUserIdTask(
			    Gs2WebSocketSession,
			    request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.DeleteSubscribeRankingScoreByUserIdResult> DeleteSubscribeRankingScoreByUserIdAsync(
            Request.DeleteSubscribeRankingScoreByUserIdRequest request
        )
		{
		    var task = new DeleteSubscribeRankingScoreByUserIdTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
    #else
		public DeleteSubscribeRankingScoreByUserIdTask DeleteSubscribeRankingScoreByUserIdAsync(
                Request.DeleteSubscribeRankingScoreByUserIdRequest request
        )
		{
			return new DeleteSubscribeRankingScoreByUserIdTask(
                Gs2WebSocketSession,
			    request
            );
        }
    #endif
#else
		public async Task<Result.DeleteSubscribeRankingScoreByUserIdResult> DeleteSubscribeRankingScoreByUserIdAsync(
            Request.DeleteSubscribeRankingScoreByUserIdRequest request
        )
		{
		    var task = new DeleteSubscribeRankingScoreByUserIdTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
#endif


        public class VerifySubscribeRankingScoreTask : Gs2WebSocketSessionTask<Request.VerifySubscribeRankingScoreRequest, Result.VerifySubscribeRankingScoreResult>
        {
	        public VerifySubscribeRankingScoreTask(IGs2Session session, Request.VerifySubscribeRankingScoreRequest request) : base(session, request)
	        {
	        }

            protected override IGs2SessionRequest CreateRequest(Request.VerifySubscribeRankingScoreRequest request)
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
                if (request.RankingName != null)
                {
                    jsonWriter.WritePropertyName("rankingName");
                    jsonWriter.Write(request.RankingName.ToString());
                }
                if (request.VerifyType != null)
                {
                    jsonWriter.WritePropertyName("verifyType");
                    jsonWriter.Write(request.VerifyType.ToString());
                }
                if (request.Season != null)
                {
                    jsonWriter.WritePropertyName("season");
                    jsonWriter.Write(request.Season.ToString());
                }
                if (request.Score != null)
                {
                    jsonWriter.WritePropertyName("score");
                    jsonWriter.Write(request.Score.ToString());
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
                    "ranking2",
                    "subscribeRankingScore",
                    "verifySubscribeRankingScore",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator VerifySubscribeRankingScore(
                Request.VerifySubscribeRankingScoreRequest request,
                UnityAction<AsyncResult<Result.VerifySubscribeRankingScoreResult>> callback
        )
		{
			var task = new VerifySubscribeRankingScoreTask(
			    Gs2WebSocketSession,
			    request
            );
            yield return task;
            callback.Invoke(new AsyncResult<Result.VerifySubscribeRankingScoreResult>(task.Result, task.Error));
        }

		public IFuture<Result.VerifySubscribeRankingScoreResult> VerifySubscribeRankingScoreFuture(
                Request.VerifySubscribeRankingScoreRequest request
        )
		{
			return new VerifySubscribeRankingScoreTask(
			    Gs2WebSocketSession,
			    request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.VerifySubscribeRankingScoreResult> VerifySubscribeRankingScoreAsync(
            Request.VerifySubscribeRankingScoreRequest request
        )
		{
		    var task = new VerifySubscribeRankingScoreTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
    #else
		public VerifySubscribeRankingScoreTask VerifySubscribeRankingScoreAsync(
                Request.VerifySubscribeRankingScoreRequest request
        )
		{
			return new VerifySubscribeRankingScoreTask(
                Gs2WebSocketSession,
			    request
            );
        }
    #endif
#else
		public async Task<Result.VerifySubscribeRankingScoreResult> VerifySubscribeRankingScoreAsync(
            Request.VerifySubscribeRankingScoreRequest request
        )
		{
		    var task = new VerifySubscribeRankingScoreTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
#endif


        public class VerifySubscribeRankingScoreByUserIdTask : Gs2WebSocketSessionTask<Request.VerifySubscribeRankingScoreByUserIdRequest, Result.VerifySubscribeRankingScoreByUserIdResult>
        {
	        public VerifySubscribeRankingScoreByUserIdTask(IGs2Session session, Request.VerifySubscribeRankingScoreByUserIdRequest request) : base(session, request)
	        {
	        }

            protected override IGs2SessionRequest CreateRequest(Request.VerifySubscribeRankingScoreByUserIdRequest request)
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
                if (request.RankingName != null)
                {
                    jsonWriter.WritePropertyName("rankingName");
                    jsonWriter.Write(request.RankingName.ToString());
                }
                if (request.VerifyType != null)
                {
                    jsonWriter.WritePropertyName("verifyType");
                    jsonWriter.Write(request.VerifyType.ToString());
                }
                if (request.Season != null)
                {
                    jsonWriter.WritePropertyName("season");
                    jsonWriter.Write(request.Season.ToString());
                }
                if (request.Score != null)
                {
                    jsonWriter.WritePropertyName("score");
                    jsonWriter.Write(request.Score.ToString());
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
                    "ranking2",
                    "subscribeRankingScore",
                    "verifySubscribeRankingScoreByUserId",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator VerifySubscribeRankingScoreByUserId(
                Request.VerifySubscribeRankingScoreByUserIdRequest request,
                UnityAction<AsyncResult<Result.VerifySubscribeRankingScoreByUserIdResult>> callback
        )
		{
			var task = new VerifySubscribeRankingScoreByUserIdTask(
			    Gs2WebSocketSession,
			    request
            );
            yield return task;
            callback.Invoke(new AsyncResult<Result.VerifySubscribeRankingScoreByUserIdResult>(task.Result, task.Error));
        }

		public IFuture<Result.VerifySubscribeRankingScoreByUserIdResult> VerifySubscribeRankingScoreByUserIdFuture(
                Request.VerifySubscribeRankingScoreByUserIdRequest request
        )
		{
			return new VerifySubscribeRankingScoreByUserIdTask(
			    Gs2WebSocketSession,
			    request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.VerifySubscribeRankingScoreByUserIdResult> VerifySubscribeRankingScoreByUserIdAsync(
            Request.VerifySubscribeRankingScoreByUserIdRequest request
        )
		{
		    var task = new VerifySubscribeRankingScoreByUserIdTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
    #else
		public VerifySubscribeRankingScoreByUserIdTask VerifySubscribeRankingScoreByUserIdAsync(
                Request.VerifySubscribeRankingScoreByUserIdRequest request
        )
		{
			return new VerifySubscribeRankingScoreByUserIdTask(
                Gs2WebSocketSession,
			    request
            );
        }
    #endif
#else
		public async Task<Result.VerifySubscribeRankingScoreByUserIdResult> VerifySubscribeRankingScoreByUserIdAsync(
            Request.VerifySubscribeRankingScoreByUserIdRequest request
        )
		{
		    var task = new VerifySubscribeRankingScoreByUserIdTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
#endif


        public class GetSubscribeRankingTask : Gs2WebSocketSessionTask<Request.GetSubscribeRankingRequest, Result.GetSubscribeRankingResult>
        {
	        public GetSubscribeRankingTask(IGs2Session session, Request.GetSubscribeRankingRequest request) : base(session, request)
	        {
	        }

            protected override IGs2SessionRequest CreateRequest(Request.GetSubscribeRankingRequest request)
            {
                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);

                jsonWriter.WriteObjectStart();

                if (request.NamespaceName != null)
                {
                    jsonWriter.WritePropertyName("namespaceName");
                    jsonWriter.Write(request.NamespaceName.ToString());
                }
                if (request.RankingName != null)
                {
                    jsonWriter.WritePropertyName("rankingName");
                    jsonWriter.Write(request.RankingName.ToString());
                }
                if (request.AccessToken != null)
                {
                    jsonWriter.WritePropertyName("accessToken");
                    jsonWriter.Write(request.AccessToken.ToString());
                }
                if (request.Season != null)
                {
                    jsonWriter.WritePropertyName("season");
                    jsonWriter.Write(request.Season.ToString());
                }
                if (request.ScorerUserId != null)
                {
                    jsonWriter.WritePropertyName("scorerUserId");
                    jsonWriter.Write(request.ScorerUserId.ToString());
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
                    "ranking2",
                    "subscribeRankingData",
                    "getSubscribeRanking",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator GetSubscribeRanking(
                Request.GetSubscribeRankingRequest request,
                UnityAction<AsyncResult<Result.GetSubscribeRankingResult>> callback
        )
		{
			var task = new GetSubscribeRankingTask(
			    Gs2WebSocketSession,
			    request
            );
            yield return task;
            callback.Invoke(new AsyncResult<Result.GetSubscribeRankingResult>(task.Result, task.Error));
        }

		public IFuture<Result.GetSubscribeRankingResult> GetSubscribeRankingFuture(
                Request.GetSubscribeRankingRequest request
        )
		{
			return new GetSubscribeRankingTask(
			    Gs2WebSocketSession,
			    request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.GetSubscribeRankingResult> GetSubscribeRankingAsync(
            Request.GetSubscribeRankingRequest request
        )
		{
		    var task = new GetSubscribeRankingTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
    #else
		public GetSubscribeRankingTask GetSubscribeRankingAsync(
                Request.GetSubscribeRankingRequest request
        )
		{
			return new GetSubscribeRankingTask(
                Gs2WebSocketSession,
			    request
            );
        }
    #endif
#else
		public async Task<Result.GetSubscribeRankingResult> GetSubscribeRankingAsync(
            Request.GetSubscribeRankingRequest request
        )
		{
		    var task = new GetSubscribeRankingTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
#endif


        public class GetSubscribeRankingByUserIdTask : Gs2WebSocketSessionTask<Request.GetSubscribeRankingByUserIdRequest, Result.GetSubscribeRankingByUserIdResult>
        {
	        public GetSubscribeRankingByUserIdTask(IGs2Session session, Request.GetSubscribeRankingByUserIdRequest request) : base(session, request)
	        {
	        }

            protected override IGs2SessionRequest CreateRequest(Request.GetSubscribeRankingByUserIdRequest request)
            {
                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);

                jsonWriter.WriteObjectStart();

                if (request.NamespaceName != null)
                {
                    jsonWriter.WritePropertyName("namespaceName");
                    jsonWriter.Write(request.NamespaceName.ToString());
                }
                if (request.RankingName != null)
                {
                    jsonWriter.WritePropertyName("rankingName");
                    jsonWriter.Write(request.RankingName.ToString());
                }
                if (request.UserId != null)
                {
                    jsonWriter.WritePropertyName("userId");
                    jsonWriter.Write(request.UserId.ToString());
                }
                if (request.Season != null)
                {
                    jsonWriter.WritePropertyName("season");
                    jsonWriter.Write(request.Season.ToString());
                }
                if (request.ScorerUserId != null)
                {
                    jsonWriter.WritePropertyName("scorerUserId");
                    jsonWriter.Write(request.ScorerUserId.ToString());
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
                    "ranking2",
                    "subscribeRankingData",
                    "getSubscribeRankingByUserId",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator GetSubscribeRankingByUserId(
                Request.GetSubscribeRankingByUserIdRequest request,
                UnityAction<AsyncResult<Result.GetSubscribeRankingByUserIdResult>> callback
        )
		{
			var task = new GetSubscribeRankingByUserIdTask(
			    Gs2WebSocketSession,
			    request
            );
            yield return task;
            callback.Invoke(new AsyncResult<Result.GetSubscribeRankingByUserIdResult>(task.Result, task.Error));
        }

		public IFuture<Result.GetSubscribeRankingByUserIdResult> GetSubscribeRankingByUserIdFuture(
                Request.GetSubscribeRankingByUserIdRequest request
        )
		{
			return new GetSubscribeRankingByUserIdTask(
			    Gs2WebSocketSession,
			    request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.GetSubscribeRankingByUserIdResult> GetSubscribeRankingByUserIdAsync(
            Request.GetSubscribeRankingByUserIdRequest request
        )
		{
		    var task = new GetSubscribeRankingByUserIdTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
    #else
		public GetSubscribeRankingByUserIdTask GetSubscribeRankingByUserIdAsync(
                Request.GetSubscribeRankingByUserIdRequest request
        )
		{
			return new GetSubscribeRankingByUserIdTask(
                Gs2WebSocketSession,
			    request
            );
        }
    #endif
#else
		public async Task<Result.GetSubscribeRankingByUserIdResult> GetSubscribeRankingByUserIdAsync(
            Request.GetSubscribeRankingByUserIdRequest request
        )
		{
		    var task = new GetSubscribeRankingByUserIdTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
#endif


        public class GetSubscribeTask : Gs2WebSocketSessionTask<Request.GetSubscribeRequest, Result.GetSubscribeResult>
        {
	        public GetSubscribeTask(IGs2Session session, Request.GetSubscribeRequest request) : base(session, request)
	        {
	        }

            protected override IGs2SessionRequest CreateRequest(Request.GetSubscribeRequest request)
            {
                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);

                jsonWriter.WriteObjectStart();

                if (request.NamespaceName != null)
                {
                    jsonWriter.WritePropertyName("namespaceName");
                    jsonWriter.Write(request.NamespaceName.ToString());
                }
                if (request.RankingName != null)
                {
                    jsonWriter.WritePropertyName("rankingName");
                    jsonWriter.Write(request.RankingName.ToString());
                }
                if (request.AccessToken != null)
                {
                    jsonWriter.WritePropertyName("accessToken");
                    jsonWriter.Write(request.AccessToken.ToString());
                }
                if (request.TargetUserId != null)
                {
                    jsonWriter.WritePropertyName("targetUserId");
                    jsonWriter.Write(request.TargetUserId.ToString());
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
                    "ranking2",
                    "subscribeUser",
                    "getSubscribe",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator GetSubscribe(
                Request.GetSubscribeRequest request,
                UnityAction<AsyncResult<Result.GetSubscribeResult>> callback
        )
		{
			var task = new GetSubscribeTask(
			    Gs2WebSocketSession,
			    request
            );
            yield return task;
            callback.Invoke(new AsyncResult<Result.GetSubscribeResult>(task.Result, task.Error));
        }

		public IFuture<Result.GetSubscribeResult> GetSubscribeFuture(
                Request.GetSubscribeRequest request
        )
		{
			return new GetSubscribeTask(
			    Gs2WebSocketSession,
			    request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.GetSubscribeResult> GetSubscribeAsync(
            Request.GetSubscribeRequest request
        )
		{
		    var task = new GetSubscribeTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
    #else
		public GetSubscribeTask GetSubscribeAsync(
                Request.GetSubscribeRequest request
        )
		{
			return new GetSubscribeTask(
                Gs2WebSocketSession,
			    request
            );
        }
    #endif
#else
		public async Task<Result.GetSubscribeResult> GetSubscribeAsync(
            Request.GetSubscribeRequest request
        )
		{
		    var task = new GetSubscribeTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
#endif


        public class GetSubscribeByUserIdTask : Gs2WebSocketSessionTask<Request.GetSubscribeByUserIdRequest, Result.GetSubscribeByUserIdResult>
        {
	        public GetSubscribeByUserIdTask(IGs2Session session, Request.GetSubscribeByUserIdRequest request) : base(session, request)
	        {
	        }

            protected override IGs2SessionRequest CreateRequest(Request.GetSubscribeByUserIdRequest request)
            {
                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);

                jsonWriter.WriteObjectStart();

                if (request.NamespaceName != null)
                {
                    jsonWriter.WritePropertyName("namespaceName");
                    jsonWriter.Write(request.NamespaceName.ToString());
                }
                if (request.RankingName != null)
                {
                    jsonWriter.WritePropertyName("rankingName");
                    jsonWriter.Write(request.RankingName.ToString());
                }
                if (request.UserId != null)
                {
                    jsonWriter.WritePropertyName("userId");
                    jsonWriter.Write(request.UserId.ToString());
                }
                if (request.TargetUserId != null)
                {
                    jsonWriter.WritePropertyName("targetUserId");
                    jsonWriter.Write(request.TargetUserId.ToString());
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
                    "ranking2",
                    "subscribeUser",
                    "getSubscribeByUserId",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator GetSubscribeByUserId(
                Request.GetSubscribeByUserIdRequest request,
                UnityAction<AsyncResult<Result.GetSubscribeByUserIdResult>> callback
        )
		{
			var task = new GetSubscribeByUserIdTask(
			    Gs2WebSocketSession,
			    request
            );
            yield return task;
            callback.Invoke(new AsyncResult<Result.GetSubscribeByUserIdResult>(task.Result, task.Error));
        }

		public IFuture<Result.GetSubscribeByUserIdResult> GetSubscribeByUserIdFuture(
                Request.GetSubscribeByUserIdRequest request
        )
		{
			return new GetSubscribeByUserIdTask(
			    Gs2WebSocketSession,
			    request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.GetSubscribeByUserIdResult> GetSubscribeByUserIdAsync(
            Request.GetSubscribeByUserIdRequest request
        )
		{
		    var task = new GetSubscribeByUserIdTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
    #else
		public GetSubscribeByUserIdTask GetSubscribeByUserIdAsync(
                Request.GetSubscribeByUserIdRequest request
        )
		{
			return new GetSubscribeByUserIdTask(
                Gs2WebSocketSession,
			    request
            );
        }
    #endif
#else
		public async Task<Result.GetSubscribeByUserIdResult> GetSubscribeByUserIdAsync(
            Request.GetSubscribeByUserIdRequest request
        )
		{
		    var task = new GetSubscribeByUserIdTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
#endif


        public class DeleteSubscribeTask : Gs2WebSocketSessionTask<Request.DeleteSubscribeRequest, Result.DeleteSubscribeResult>
        {
	        public DeleteSubscribeTask(IGs2Session session, Request.DeleteSubscribeRequest request) : base(session, request)
	        {
	        }

            protected override IGs2SessionRequest CreateRequest(Request.DeleteSubscribeRequest request)
            {
                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);

                jsonWriter.WriteObjectStart();

                if (request.NamespaceName != null)
                {
                    jsonWriter.WritePropertyName("namespaceName");
                    jsonWriter.Write(request.NamespaceName.ToString());
                }
                if (request.RankingName != null)
                {
                    jsonWriter.WritePropertyName("rankingName");
                    jsonWriter.Write(request.RankingName.ToString());
                }
                if (request.AccessToken != null)
                {
                    jsonWriter.WritePropertyName("accessToken");
                    jsonWriter.Write(request.AccessToken.ToString());
                }
                if (request.TargetUserId != null)
                {
                    jsonWriter.WritePropertyName("targetUserId");
                    jsonWriter.Write(request.TargetUserId.ToString());
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
                    "ranking2",
                    "subscribeUser",
                    "deleteSubscribe",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator DeleteSubscribe(
                Request.DeleteSubscribeRequest request,
                UnityAction<AsyncResult<Result.DeleteSubscribeResult>> callback
        )
		{
			var task = new DeleteSubscribeTask(
			    Gs2WebSocketSession,
			    request
            );
            yield return task;
            callback.Invoke(new AsyncResult<Result.DeleteSubscribeResult>(task.Result, task.Error));
        }

		public IFuture<Result.DeleteSubscribeResult> DeleteSubscribeFuture(
                Request.DeleteSubscribeRequest request
        )
		{
			return new DeleteSubscribeTask(
			    Gs2WebSocketSession,
			    request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.DeleteSubscribeResult> DeleteSubscribeAsync(
            Request.DeleteSubscribeRequest request
        )
		{
		    var task = new DeleteSubscribeTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
    #else
		public DeleteSubscribeTask DeleteSubscribeAsync(
                Request.DeleteSubscribeRequest request
        )
		{
			return new DeleteSubscribeTask(
                Gs2WebSocketSession,
			    request
            );
        }
    #endif
#else
		public async Task<Result.DeleteSubscribeResult> DeleteSubscribeAsync(
            Request.DeleteSubscribeRequest request
        )
		{
		    var task = new DeleteSubscribeTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
#endif


        public class DeleteSubscribeByUserIdTask : Gs2WebSocketSessionTask<Request.DeleteSubscribeByUserIdRequest, Result.DeleteSubscribeByUserIdResult>
        {
	        public DeleteSubscribeByUserIdTask(IGs2Session session, Request.DeleteSubscribeByUserIdRequest request) : base(session, request)
	        {
	        }

            protected override IGs2SessionRequest CreateRequest(Request.DeleteSubscribeByUserIdRequest request)
            {
                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);

                jsonWriter.WriteObjectStart();

                if (request.NamespaceName != null)
                {
                    jsonWriter.WritePropertyName("namespaceName");
                    jsonWriter.Write(request.NamespaceName.ToString());
                }
                if (request.RankingName != null)
                {
                    jsonWriter.WritePropertyName("rankingName");
                    jsonWriter.Write(request.RankingName.ToString());
                }
                if (request.UserId != null)
                {
                    jsonWriter.WritePropertyName("userId");
                    jsonWriter.Write(request.UserId.ToString());
                }
                if (request.TargetUserId != null)
                {
                    jsonWriter.WritePropertyName("targetUserId");
                    jsonWriter.Write(request.TargetUserId.ToString());
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
                    "ranking2",
                    "subscribeUser",
                    "deleteSubscribeByUserId",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator DeleteSubscribeByUserId(
                Request.DeleteSubscribeByUserIdRequest request,
                UnityAction<AsyncResult<Result.DeleteSubscribeByUserIdResult>> callback
        )
		{
			var task = new DeleteSubscribeByUserIdTask(
			    Gs2WebSocketSession,
			    request
            );
            yield return task;
            callback.Invoke(new AsyncResult<Result.DeleteSubscribeByUserIdResult>(task.Result, task.Error));
        }

		public IFuture<Result.DeleteSubscribeByUserIdResult> DeleteSubscribeByUserIdFuture(
                Request.DeleteSubscribeByUserIdRequest request
        )
		{
			return new DeleteSubscribeByUserIdTask(
			    Gs2WebSocketSession,
			    request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.DeleteSubscribeByUserIdResult> DeleteSubscribeByUserIdAsync(
            Request.DeleteSubscribeByUserIdRequest request
        )
		{
		    var task = new DeleteSubscribeByUserIdTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
    #else
		public DeleteSubscribeByUserIdTask DeleteSubscribeByUserIdAsync(
                Request.DeleteSubscribeByUserIdRequest request
        )
		{
			return new DeleteSubscribeByUserIdTask(
                Gs2WebSocketSession,
			    request
            );
        }
    #endif
#else
		public async Task<Result.DeleteSubscribeByUserIdResult> DeleteSubscribeByUserIdAsync(
            Request.DeleteSubscribeByUserIdRequest request
        )
		{
		    var task = new DeleteSubscribeByUserIdTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
#endif
	}
}