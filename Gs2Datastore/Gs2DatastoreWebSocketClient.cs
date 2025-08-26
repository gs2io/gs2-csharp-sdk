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

namespace Gs2.Gs2Datastore
{
	public class Gs2DatastoreWebSocketClient : AbstractGs2Client
	{

		public static string Endpoint = "datastore";

        protected Gs2WebSocketSession Gs2WebSocketSession => (Gs2WebSocketSession) Gs2Session;

		public Gs2DatastoreWebSocketClient(Gs2WebSocketSession Gs2WebSocketSession) : base(Gs2WebSocketSession)
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
                if (request.LogSetting != null)
                {
                    jsonWriter.WritePropertyName("logSetting");
                    request.LogSetting.WriteJson(jsonWriter);
                }
                if (request.DoneUploadScript != null)
                {
                    jsonWriter.WritePropertyName("doneUploadScript");
                    request.DoneUploadScript.WriteJson(jsonWriter);
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
                    "datastore",
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
                    "datastore",
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
                    "datastore",
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
                if (request.LogSetting != null)
                {
                    jsonWriter.WritePropertyName("logSetting");
                    request.LogSetting.WriteJson(jsonWriter);
                }
                if (request.DoneUploadScript != null)
                {
                    jsonWriter.WritePropertyName("doneUploadScript");
                    request.DoneUploadScript.WriteJson(jsonWriter);
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
                    "datastore",
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
                    "datastore",
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
                    "datastore",
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
                    "datastore",
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
                    "datastore",
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
                    "datastore",
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
                    "datastore",
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


        public class PrepareImportUserDataByUserIdTask : Gs2WebSocketSessionTask<Request.PrepareImportUserDataByUserIdRequest, Result.PrepareImportUserDataByUserIdResult>
        {
	        public PrepareImportUserDataByUserIdTask(IGs2Session session, Request.PrepareImportUserDataByUserIdRequest request) : base(session, request)
	        {
	        }

            protected override IGs2SessionRequest CreateRequest(Request.PrepareImportUserDataByUserIdRequest request)
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
                    "datastore",
                    "namespace",
                    "prepareImportUserDataByUserId",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator PrepareImportUserDataByUserId(
                Request.PrepareImportUserDataByUserIdRequest request,
                UnityAction<AsyncResult<Result.PrepareImportUserDataByUserIdResult>> callback
        )
		{
			var task = new PrepareImportUserDataByUserIdTask(
			    Gs2WebSocketSession,
			    request
            );
            yield return task;
            callback.Invoke(new AsyncResult<Result.PrepareImportUserDataByUserIdResult>(task.Result, task.Error));
        }

		public IFuture<Result.PrepareImportUserDataByUserIdResult> PrepareImportUserDataByUserIdFuture(
                Request.PrepareImportUserDataByUserIdRequest request
        )
		{
			return new PrepareImportUserDataByUserIdTask(
			    Gs2WebSocketSession,
			    request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.PrepareImportUserDataByUserIdResult> PrepareImportUserDataByUserIdAsync(
            Request.PrepareImportUserDataByUserIdRequest request
        )
		{
		    var task = new PrepareImportUserDataByUserIdTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
    #else
		public PrepareImportUserDataByUserIdTask PrepareImportUserDataByUserIdAsync(
                Request.PrepareImportUserDataByUserIdRequest request
        )
		{
			return new PrepareImportUserDataByUserIdTask(
                Gs2WebSocketSession,
			    request
            );
        }
    #endif
#else
		public async Task<Result.PrepareImportUserDataByUserIdResult> PrepareImportUserDataByUserIdAsync(
            Request.PrepareImportUserDataByUserIdRequest request
        )
		{
		    var task = new PrepareImportUserDataByUserIdTask(
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
                    "datastore",
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
                    "datastore",
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


        public class PrepareUploadTask : Gs2WebSocketSessionTask<Request.PrepareUploadRequest, Result.PrepareUploadResult>
        {
	        public PrepareUploadTask(IGs2Session session, Request.PrepareUploadRequest request) : base(session, request)
	        {
	        }

            protected override IGs2SessionRequest CreateRequest(Request.PrepareUploadRequest request)
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
                if (request.Name != null)
                {
                    jsonWriter.WritePropertyName("name");
                    jsonWriter.Write(request.Name.ToString());
                }
                if (request.ContentType != null)
                {
                    jsonWriter.WritePropertyName("contentType");
                    jsonWriter.Write(request.ContentType.ToString());
                }
                if (request.Scope != null)
                {
                    jsonWriter.WritePropertyName("scope");
                    jsonWriter.Write(request.Scope.ToString());
                }
                if (request.AllowUserIds != null)
                {
                    jsonWriter.WritePropertyName("allowUserIds");
                    jsonWriter.WriteArrayStart();
                    foreach(var item in request.AllowUserIds)
                    {
                        jsonWriter.Write(item);
                    }
                    jsonWriter.WriteArrayEnd();
                }
                if (request.UpdateIfExists != null)
                {
                    jsonWriter.WritePropertyName("updateIfExists");
                    jsonWriter.Write(request.UpdateIfExists.ToString());
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
                    "datastore",
                    "dataObject",
                    "prepareUpload",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator PrepareUpload(
                Request.PrepareUploadRequest request,
                UnityAction<AsyncResult<Result.PrepareUploadResult>> callback
        )
		{
			var task = new PrepareUploadTask(
			    Gs2WebSocketSession,
			    request
            );
            yield return task;
            callback.Invoke(new AsyncResult<Result.PrepareUploadResult>(task.Result, task.Error));
        }

		public IFuture<Result.PrepareUploadResult> PrepareUploadFuture(
                Request.PrepareUploadRequest request
        )
		{
			return new PrepareUploadTask(
			    Gs2WebSocketSession,
			    request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.PrepareUploadResult> PrepareUploadAsync(
            Request.PrepareUploadRequest request
        )
		{
		    var task = new PrepareUploadTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
    #else
		public PrepareUploadTask PrepareUploadAsync(
                Request.PrepareUploadRequest request
        )
		{
			return new PrepareUploadTask(
                Gs2WebSocketSession,
			    request
            );
        }
    #endif
#else
		public async Task<Result.PrepareUploadResult> PrepareUploadAsync(
            Request.PrepareUploadRequest request
        )
		{
		    var task = new PrepareUploadTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
#endif


        public class PrepareUploadByUserIdTask : Gs2WebSocketSessionTask<Request.PrepareUploadByUserIdRequest, Result.PrepareUploadByUserIdResult>
        {
	        public PrepareUploadByUserIdTask(IGs2Session session, Request.PrepareUploadByUserIdRequest request) : base(session, request)
	        {
	        }

            protected override IGs2SessionRequest CreateRequest(Request.PrepareUploadByUserIdRequest request)
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
                if (request.Name != null)
                {
                    jsonWriter.WritePropertyName("name");
                    jsonWriter.Write(request.Name.ToString());
                }
                if (request.ContentType != null)
                {
                    jsonWriter.WritePropertyName("contentType");
                    jsonWriter.Write(request.ContentType.ToString());
                }
                if (request.Scope != null)
                {
                    jsonWriter.WritePropertyName("scope");
                    jsonWriter.Write(request.Scope.ToString());
                }
                if (request.AllowUserIds != null)
                {
                    jsonWriter.WritePropertyName("allowUserIds");
                    jsonWriter.WriteArrayStart();
                    foreach(var item in request.AllowUserIds)
                    {
                        jsonWriter.Write(item);
                    }
                    jsonWriter.WriteArrayEnd();
                }
                if (request.UpdateIfExists != null)
                {
                    jsonWriter.WritePropertyName("updateIfExists");
                    jsonWriter.Write(request.UpdateIfExists.ToString());
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
                    "datastore",
                    "dataObject",
                    "prepareUploadByUserId",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator PrepareUploadByUserId(
                Request.PrepareUploadByUserIdRequest request,
                UnityAction<AsyncResult<Result.PrepareUploadByUserIdResult>> callback
        )
		{
			var task = new PrepareUploadByUserIdTask(
			    Gs2WebSocketSession,
			    request
            );
            yield return task;
            callback.Invoke(new AsyncResult<Result.PrepareUploadByUserIdResult>(task.Result, task.Error));
        }

		public IFuture<Result.PrepareUploadByUserIdResult> PrepareUploadByUserIdFuture(
                Request.PrepareUploadByUserIdRequest request
        )
		{
			return new PrepareUploadByUserIdTask(
			    Gs2WebSocketSession,
			    request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.PrepareUploadByUserIdResult> PrepareUploadByUserIdAsync(
            Request.PrepareUploadByUserIdRequest request
        )
		{
		    var task = new PrepareUploadByUserIdTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
    #else
		public PrepareUploadByUserIdTask PrepareUploadByUserIdAsync(
                Request.PrepareUploadByUserIdRequest request
        )
		{
			return new PrepareUploadByUserIdTask(
                Gs2WebSocketSession,
			    request
            );
        }
    #endif
#else
		public async Task<Result.PrepareUploadByUserIdResult> PrepareUploadByUserIdAsync(
            Request.PrepareUploadByUserIdRequest request
        )
		{
		    var task = new PrepareUploadByUserIdTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
#endif


        public class UpdateDataObjectTask : Gs2WebSocketSessionTask<Request.UpdateDataObjectRequest, Result.UpdateDataObjectResult>
        {
	        public UpdateDataObjectTask(IGs2Session session, Request.UpdateDataObjectRequest request) : base(session, request)
	        {
	        }

            protected override IGs2SessionRequest CreateRequest(Request.UpdateDataObjectRequest request)
            {
                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);

                jsonWriter.WriteObjectStart();

                if (request.NamespaceName != null)
                {
                    jsonWriter.WritePropertyName("namespaceName");
                    jsonWriter.Write(request.NamespaceName.ToString());
                }
                if (request.DataObjectName != null)
                {
                    jsonWriter.WritePropertyName("dataObjectName");
                    jsonWriter.Write(request.DataObjectName.ToString());
                }
                if (request.AccessToken != null)
                {
                    jsonWriter.WritePropertyName("accessToken");
                    jsonWriter.Write(request.AccessToken.ToString());
                }
                if (request.Scope != null)
                {
                    jsonWriter.WritePropertyName("scope");
                    jsonWriter.Write(request.Scope.ToString());
                }
                if (request.AllowUserIds != null)
                {
                    jsonWriter.WritePropertyName("allowUserIds");
                    jsonWriter.WriteArrayStart();
                    foreach(var item in request.AllowUserIds)
                    {
                        jsonWriter.Write(item);
                    }
                    jsonWriter.WriteArrayEnd();
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
                    "datastore",
                    "dataObject",
                    "updateDataObject",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator UpdateDataObject(
                Request.UpdateDataObjectRequest request,
                UnityAction<AsyncResult<Result.UpdateDataObjectResult>> callback
        )
		{
			var task = new UpdateDataObjectTask(
			    Gs2WebSocketSession,
			    request
            );
            yield return task;
            callback.Invoke(new AsyncResult<Result.UpdateDataObjectResult>(task.Result, task.Error));
        }

		public IFuture<Result.UpdateDataObjectResult> UpdateDataObjectFuture(
                Request.UpdateDataObjectRequest request
        )
		{
			return new UpdateDataObjectTask(
			    Gs2WebSocketSession,
			    request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.UpdateDataObjectResult> UpdateDataObjectAsync(
            Request.UpdateDataObjectRequest request
        )
		{
		    var task = new UpdateDataObjectTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
    #else
		public UpdateDataObjectTask UpdateDataObjectAsync(
                Request.UpdateDataObjectRequest request
        )
		{
			return new UpdateDataObjectTask(
                Gs2WebSocketSession,
			    request
            );
        }
    #endif
#else
		public async Task<Result.UpdateDataObjectResult> UpdateDataObjectAsync(
            Request.UpdateDataObjectRequest request
        )
		{
		    var task = new UpdateDataObjectTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
#endif


        public class UpdateDataObjectByUserIdTask : Gs2WebSocketSessionTask<Request.UpdateDataObjectByUserIdRequest, Result.UpdateDataObjectByUserIdResult>
        {
	        public UpdateDataObjectByUserIdTask(IGs2Session session, Request.UpdateDataObjectByUserIdRequest request) : base(session, request)
	        {
	        }

            protected override IGs2SessionRequest CreateRequest(Request.UpdateDataObjectByUserIdRequest request)
            {
                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);

                jsonWriter.WriteObjectStart();

                if (request.NamespaceName != null)
                {
                    jsonWriter.WritePropertyName("namespaceName");
                    jsonWriter.Write(request.NamespaceName.ToString());
                }
                if (request.DataObjectName != null)
                {
                    jsonWriter.WritePropertyName("dataObjectName");
                    jsonWriter.Write(request.DataObjectName.ToString());
                }
                if (request.UserId != null)
                {
                    jsonWriter.WritePropertyName("userId");
                    jsonWriter.Write(request.UserId.ToString());
                }
                if (request.Scope != null)
                {
                    jsonWriter.WritePropertyName("scope");
                    jsonWriter.Write(request.Scope.ToString());
                }
                if (request.AllowUserIds != null)
                {
                    jsonWriter.WritePropertyName("allowUserIds");
                    jsonWriter.WriteArrayStart();
                    foreach(var item in request.AllowUserIds)
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
                    "datastore",
                    "dataObject",
                    "updateDataObjectByUserId",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator UpdateDataObjectByUserId(
                Request.UpdateDataObjectByUserIdRequest request,
                UnityAction<AsyncResult<Result.UpdateDataObjectByUserIdResult>> callback
        )
		{
			var task = new UpdateDataObjectByUserIdTask(
			    Gs2WebSocketSession,
			    request
            );
            yield return task;
            callback.Invoke(new AsyncResult<Result.UpdateDataObjectByUserIdResult>(task.Result, task.Error));
        }

		public IFuture<Result.UpdateDataObjectByUserIdResult> UpdateDataObjectByUserIdFuture(
                Request.UpdateDataObjectByUserIdRequest request
        )
		{
			return new UpdateDataObjectByUserIdTask(
			    Gs2WebSocketSession,
			    request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.UpdateDataObjectByUserIdResult> UpdateDataObjectByUserIdAsync(
            Request.UpdateDataObjectByUserIdRequest request
        )
		{
		    var task = new UpdateDataObjectByUserIdTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
    #else
		public UpdateDataObjectByUserIdTask UpdateDataObjectByUserIdAsync(
                Request.UpdateDataObjectByUserIdRequest request
        )
		{
			return new UpdateDataObjectByUserIdTask(
                Gs2WebSocketSession,
			    request
            );
        }
    #endif
#else
		public async Task<Result.UpdateDataObjectByUserIdResult> UpdateDataObjectByUserIdAsync(
            Request.UpdateDataObjectByUserIdRequest request
        )
		{
		    var task = new UpdateDataObjectByUserIdTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
#endif


        public class DoneUploadTask : Gs2WebSocketSessionTask<Request.DoneUploadRequest, Result.DoneUploadResult>
        {
	        public DoneUploadTask(IGs2Session session, Request.DoneUploadRequest request) : base(session, request)
	        {
	        }

            protected override IGs2SessionRequest CreateRequest(Request.DoneUploadRequest request)
            {
                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);

                jsonWriter.WriteObjectStart();

                if (request.NamespaceName != null)
                {
                    jsonWriter.WritePropertyName("namespaceName");
                    jsonWriter.Write(request.NamespaceName.ToString());
                }
                if (request.DataObjectName != null)
                {
                    jsonWriter.WritePropertyName("dataObjectName");
                    jsonWriter.Write(request.DataObjectName.ToString());
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
                    "datastore",
                    "dataObject",
                    "doneUpload",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }

            public override void OnError(Gs2.Core.Exception.Gs2Exception error)
            {
                if (error.Errors.Count(v => v.code == "dataObject.status.invalid") > 0) {
                    base.OnError(new Exception.InvalidStatusException(error));
                }
                else if (error.Errors.Count(v => v.code == "dataObject.file.notUploaded") > 0) {
                    base.OnError(new Exception.NotUploadedException(error));
                }
                else {
                    base.OnError(error);
                }
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator DoneUpload(
                Request.DoneUploadRequest request,
                UnityAction<AsyncResult<Result.DoneUploadResult>> callback
        )
		{
			var task = new DoneUploadTask(
			    Gs2WebSocketSession,
			    request
            );
            yield return task;
            callback.Invoke(new AsyncResult<Result.DoneUploadResult>(task.Result, task.Error));
        }

		public IFuture<Result.DoneUploadResult> DoneUploadFuture(
                Request.DoneUploadRequest request
        )
		{
			return new DoneUploadTask(
			    Gs2WebSocketSession,
			    request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.DoneUploadResult> DoneUploadAsync(
            Request.DoneUploadRequest request
        )
		{
		    var task = new DoneUploadTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
    #else
		public DoneUploadTask DoneUploadAsync(
                Request.DoneUploadRequest request
        )
		{
			return new DoneUploadTask(
                Gs2WebSocketSession,
			    request
            );
        }
    #endif
#else
		public async Task<Result.DoneUploadResult> DoneUploadAsync(
            Request.DoneUploadRequest request
        )
		{
		    var task = new DoneUploadTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
#endif


        public class DoneUploadByUserIdTask : Gs2WebSocketSessionTask<Request.DoneUploadByUserIdRequest, Result.DoneUploadByUserIdResult>
        {
	        public DoneUploadByUserIdTask(IGs2Session session, Request.DoneUploadByUserIdRequest request) : base(session, request)
	        {
	        }

            protected override IGs2SessionRequest CreateRequest(Request.DoneUploadByUserIdRequest request)
            {
                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);

                jsonWriter.WriteObjectStart();

                if (request.NamespaceName != null)
                {
                    jsonWriter.WritePropertyName("namespaceName");
                    jsonWriter.Write(request.NamespaceName.ToString());
                }
                if (request.DataObjectName != null)
                {
                    jsonWriter.WritePropertyName("dataObjectName");
                    jsonWriter.Write(request.DataObjectName.ToString());
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
                    "datastore",
                    "dataObject",
                    "doneUploadByUserId",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }

            public override void OnError(Gs2.Core.Exception.Gs2Exception error)
            {
                if (error.Errors.Count(v => v.code == "dataObject.status.invalid") > 0) {
                    base.OnError(new Exception.InvalidStatusException(error));
                }
                else if (error.Errors.Count(v => v.code == "dataObject.file.notUploaded") > 0) {
                    base.OnError(new Exception.NotUploadedException(error));
                }
                else {
                    base.OnError(error);
                }
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator DoneUploadByUserId(
                Request.DoneUploadByUserIdRequest request,
                UnityAction<AsyncResult<Result.DoneUploadByUserIdResult>> callback
        )
		{
			var task = new DoneUploadByUserIdTask(
			    Gs2WebSocketSession,
			    request
            );
            yield return task;
            callback.Invoke(new AsyncResult<Result.DoneUploadByUserIdResult>(task.Result, task.Error));
        }

		public IFuture<Result.DoneUploadByUserIdResult> DoneUploadByUserIdFuture(
                Request.DoneUploadByUserIdRequest request
        )
		{
			return new DoneUploadByUserIdTask(
			    Gs2WebSocketSession,
			    request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.DoneUploadByUserIdResult> DoneUploadByUserIdAsync(
            Request.DoneUploadByUserIdRequest request
        )
		{
		    var task = new DoneUploadByUserIdTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
    #else
		public DoneUploadByUserIdTask DoneUploadByUserIdAsync(
                Request.DoneUploadByUserIdRequest request
        )
		{
			return new DoneUploadByUserIdTask(
                Gs2WebSocketSession,
			    request
            );
        }
    #endif
#else
		public async Task<Result.DoneUploadByUserIdResult> DoneUploadByUserIdAsync(
            Request.DoneUploadByUserIdRequest request
        )
		{
		    var task = new DoneUploadByUserIdTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
#endif


        public class DeleteDataObjectTask : Gs2WebSocketSessionTask<Request.DeleteDataObjectRequest, Result.DeleteDataObjectResult>
        {
	        public DeleteDataObjectTask(IGs2Session session, Request.DeleteDataObjectRequest request) : base(session, request)
	        {
	        }

            protected override IGs2SessionRequest CreateRequest(Request.DeleteDataObjectRequest request)
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
                if (request.DataObjectName != null)
                {
                    jsonWriter.WritePropertyName("dataObjectName");
                    jsonWriter.Write(request.DataObjectName.ToString());
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
                    "datastore",
                    "dataObject",
                    "deleteDataObject",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }

            public override void OnError(Gs2.Core.Exception.Gs2Exception error)
            {
                if (error.Errors.Count(v => v.code == "dataObject.status.invalid") > 0) {
                    base.OnError(new Exception.InvalidStatusException(error));
                }
                else {
                    base.OnError(error);
                }
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator DeleteDataObject(
                Request.DeleteDataObjectRequest request,
                UnityAction<AsyncResult<Result.DeleteDataObjectResult>> callback
        )
		{
			var task = new DeleteDataObjectTask(
			    Gs2WebSocketSession,
			    request
            );
            yield return task;
            callback.Invoke(new AsyncResult<Result.DeleteDataObjectResult>(task.Result, task.Error));
        }

		public IFuture<Result.DeleteDataObjectResult> DeleteDataObjectFuture(
                Request.DeleteDataObjectRequest request
        )
		{
			return new DeleteDataObjectTask(
			    Gs2WebSocketSession,
			    request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.DeleteDataObjectResult> DeleteDataObjectAsync(
            Request.DeleteDataObjectRequest request
        )
		{
		    var task = new DeleteDataObjectTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
    #else
		public DeleteDataObjectTask DeleteDataObjectAsync(
                Request.DeleteDataObjectRequest request
        )
		{
			return new DeleteDataObjectTask(
                Gs2WebSocketSession,
			    request
            );
        }
    #endif
#else
		public async Task<Result.DeleteDataObjectResult> DeleteDataObjectAsync(
            Request.DeleteDataObjectRequest request
        )
		{
		    var task = new DeleteDataObjectTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
#endif


        public class DeleteDataObjectByUserIdTask : Gs2WebSocketSessionTask<Request.DeleteDataObjectByUserIdRequest, Result.DeleteDataObjectByUserIdResult>
        {
	        public DeleteDataObjectByUserIdTask(IGs2Session session, Request.DeleteDataObjectByUserIdRequest request) : base(session, request)
	        {
	        }

            protected override IGs2SessionRequest CreateRequest(Request.DeleteDataObjectByUserIdRequest request)
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
                if (request.DataObjectName != null)
                {
                    jsonWriter.WritePropertyName("dataObjectName");
                    jsonWriter.Write(request.DataObjectName.ToString());
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
                    "datastore",
                    "dataObject",
                    "deleteDataObjectByUserId",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }

            public override void OnError(Gs2.Core.Exception.Gs2Exception error)
            {
                if (error.Errors.Count(v => v.code == "dataObject.status.invalid") > 0) {
                    base.OnError(new Exception.InvalidStatusException(error));
                }
                else {
                    base.OnError(error);
                }
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator DeleteDataObjectByUserId(
                Request.DeleteDataObjectByUserIdRequest request,
                UnityAction<AsyncResult<Result.DeleteDataObjectByUserIdResult>> callback
        )
		{
			var task = new DeleteDataObjectByUserIdTask(
			    Gs2WebSocketSession,
			    request
            );
            yield return task;
            callback.Invoke(new AsyncResult<Result.DeleteDataObjectByUserIdResult>(task.Result, task.Error));
        }

		public IFuture<Result.DeleteDataObjectByUserIdResult> DeleteDataObjectByUserIdFuture(
                Request.DeleteDataObjectByUserIdRequest request
        )
		{
			return new DeleteDataObjectByUserIdTask(
			    Gs2WebSocketSession,
			    request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.DeleteDataObjectByUserIdResult> DeleteDataObjectByUserIdAsync(
            Request.DeleteDataObjectByUserIdRequest request
        )
		{
		    var task = new DeleteDataObjectByUserIdTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
    #else
		public DeleteDataObjectByUserIdTask DeleteDataObjectByUserIdAsync(
                Request.DeleteDataObjectByUserIdRequest request
        )
		{
			return new DeleteDataObjectByUserIdTask(
                Gs2WebSocketSession,
			    request
            );
        }
    #endif
#else
		public async Task<Result.DeleteDataObjectByUserIdResult> DeleteDataObjectByUserIdAsync(
            Request.DeleteDataObjectByUserIdRequest request
        )
		{
		    var task = new DeleteDataObjectByUserIdTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
#endif


        public class RestoreDataObjectTask : Gs2WebSocketSessionTask<Request.RestoreDataObjectRequest, Result.RestoreDataObjectResult>
        {
	        public RestoreDataObjectTask(IGs2Session session, Request.RestoreDataObjectRequest request) : base(session, request)
	        {
	        }

            protected override IGs2SessionRequest CreateRequest(Request.RestoreDataObjectRequest request)
            {
                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);

                jsonWriter.WriteObjectStart();

                if (request.NamespaceName != null)
                {
                    jsonWriter.WritePropertyName("namespaceName");
                    jsonWriter.Write(request.NamespaceName.ToString());
                }
                if (request.DataObjectId != null)
                {
                    jsonWriter.WritePropertyName("dataObjectId");
                    jsonWriter.Write(request.DataObjectId.ToString());
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
                    "datastore",
                    "dataObject",
                    "restoreDataObject",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }

            public override void OnError(Gs2.Core.Exception.Gs2Exception error)
            {
                if (error.Errors.Count(v => v.code == "dataObject.status.invalid") > 0) {
                    base.OnError(new Exception.InvalidStatusException(error));
                }
                else {
                    base.OnError(error);
                }
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator RestoreDataObject(
                Request.RestoreDataObjectRequest request,
                UnityAction<AsyncResult<Result.RestoreDataObjectResult>> callback
        )
		{
			var task = new RestoreDataObjectTask(
			    Gs2WebSocketSession,
			    request
            );
            yield return task;
            callback.Invoke(new AsyncResult<Result.RestoreDataObjectResult>(task.Result, task.Error));
        }

		public IFuture<Result.RestoreDataObjectResult> RestoreDataObjectFuture(
                Request.RestoreDataObjectRequest request
        )
		{
			return new RestoreDataObjectTask(
			    Gs2WebSocketSession,
			    request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.RestoreDataObjectResult> RestoreDataObjectAsync(
            Request.RestoreDataObjectRequest request
        )
		{
		    var task = new RestoreDataObjectTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
    #else
		public RestoreDataObjectTask RestoreDataObjectAsync(
                Request.RestoreDataObjectRequest request
        )
		{
			return new RestoreDataObjectTask(
                Gs2WebSocketSession,
			    request
            );
        }
    #endif
#else
		public async Task<Result.RestoreDataObjectResult> RestoreDataObjectAsync(
            Request.RestoreDataObjectRequest request
        )
		{
		    var task = new RestoreDataObjectTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
#endif


        public class GetDataObjectHistoryTask : Gs2WebSocketSessionTask<Request.GetDataObjectHistoryRequest, Result.GetDataObjectHistoryResult>
        {
	        public GetDataObjectHistoryTask(IGs2Session session, Request.GetDataObjectHistoryRequest request) : base(session, request)
	        {
	        }

            protected override IGs2SessionRequest CreateRequest(Request.GetDataObjectHistoryRequest request)
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
                if (request.DataObjectName != null)
                {
                    jsonWriter.WritePropertyName("dataObjectName");
                    jsonWriter.Write(request.DataObjectName.ToString());
                }
                if (request.Generation != null)
                {
                    jsonWriter.WritePropertyName("generation");
                    jsonWriter.Write(request.Generation.ToString());
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
                    "datastore",
                    "dataObjectHistory",
                    "getDataObjectHistory",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator GetDataObjectHistory(
                Request.GetDataObjectHistoryRequest request,
                UnityAction<AsyncResult<Result.GetDataObjectHistoryResult>> callback
        )
		{
			var task = new GetDataObjectHistoryTask(
			    Gs2WebSocketSession,
			    request
            );
            yield return task;
            callback.Invoke(new AsyncResult<Result.GetDataObjectHistoryResult>(task.Result, task.Error));
        }

		public IFuture<Result.GetDataObjectHistoryResult> GetDataObjectHistoryFuture(
                Request.GetDataObjectHistoryRequest request
        )
		{
			return new GetDataObjectHistoryTask(
			    Gs2WebSocketSession,
			    request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.GetDataObjectHistoryResult> GetDataObjectHistoryAsync(
            Request.GetDataObjectHistoryRequest request
        )
		{
		    var task = new GetDataObjectHistoryTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
    #else
		public GetDataObjectHistoryTask GetDataObjectHistoryAsync(
                Request.GetDataObjectHistoryRequest request
        )
		{
			return new GetDataObjectHistoryTask(
                Gs2WebSocketSession,
			    request
            );
        }
    #endif
#else
		public async Task<Result.GetDataObjectHistoryResult> GetDataObjectHistoryAsync(
            Request.GetDataObjectHistoryRequest request
        )
		{
		    var task = new GetDataObjectHistoryTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
#endif


        public class GetDataObjectHistoryByUserIdTask : Gs2WebSocketSessionTask<Request.GetDataObjectHistoryByUserIdRequest, Result.GetDataObjectHistoryByUserIdResult>
        {
	        public GetDataObjectHistoryByUserIdTask(IGs2Session session, Request.GetDataObjectHistoryByUserIdRequest request) : base(session, request)
	        {
	        }

            protected override IGs2SessionRequest CreateRequest(Request.GetDataObjectHistoryByUserIdRequest request)
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
                if (request.DataObjectName != null)
                {
                    jsonWriter.WritePropertyName("dataObjectName");
                    jsonWriter.Write(request.DataObjectName.ToString());
                }
                if (request.Generation != null)
                {
                    jsonWriter.WritePropertyName("generation");
                    jsonWriter.Write(request.Generation.ToString());
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
                    "datastore",
                    "dataObjectHistory",
                    "getDataObjectHistoryByUserId",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator GetDataObjectHistoryByUserId(
                Request.GetDataObjectHistoryByUserIdRequest request,
                UnityAction<AsyncResult<Result.GetDataObjectHistoryByUserIdResult>> callback
        )
		{
			var task = new GetDataObjectHistoryByUserIdTask(
			    Gs2WebSocketSession,
			    request
            );
            yield return task;
            callback.Invoke(new AsyncResult<Result.GetDataObjectHistoryByUserIdResult>(task.Result, task.Error));
        }

		public IFuture<Result.GetDataObjectHistoryByUserIdResult> GetDataObjectHistoryByUserIdFuture(
                Request.GetDataObjectHistoryByUserIdRequest request
        )
		{
			return new GetDataObjectHistoryByUserIdTask(
			    Gs2WebSocketSession,
			    request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.GetDataObjectHistoryByUserIdResult> GetDataObjectHistoryByUserIdAsync(
            Request.GetDataObjectHistoryByUserIdRequest request
        )
		{
		    var task = new GetDataObjectHistoryByUserIdTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
    #else
		public GetDataObjectHistoryByUserIdTask GetDataObjectHistoryByUserIdAsync(
                Request.GetDataObjectHistoryByUserIdRequest request
        )
		{
			return new GetDataObjectHistoryByUserIdTask(
                Gs2WebSocketSession,
			    request
            );
        }
    #endif
#else
		public async Task<Result.GetDataObjectHistoryByUserIdResult> GetDataObjectHistoryByUserIdAsync(
            Request.GetDataObjectHistoryByUserIdRequest request
        )
		{
		    var task = new GetDataObjectHistoryByUserIdTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
#endif
	}
}