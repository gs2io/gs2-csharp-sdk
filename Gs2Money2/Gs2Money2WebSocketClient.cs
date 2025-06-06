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

namespace Gs2.Gs2Money2
{
	public class Gs2Money2WebSocketClient : AbstractGs2Client
	{

		public static string Endpoint = "money2";

        protected Gs2WebSocketSession Gs2WebSocketSession => (Gs2WebSocketSession) Gs2Session;

		public Gs2Money2WebSocketClient(Gs2WebSocketSession Gs2WebSocketSession) : base(Gs2WebSocketSession)
		{

		}


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
                    "money2",
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
                    "money2",
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
                    "money2",
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
                    "money2",
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
                    "money2",
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
                    "money2",
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
                    "money2",
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
                    "money2",
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
                    "money2",
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


        public class GetWalletTask : Gs2WebSocketSessionTask<Request.GetWalletRequest, Result.GetWalletResult>
        {
	        public GetWalletTask(IGs2Session session, Request.GetWalletRequest request) : base(session, request)
	        {
	        }

            protected override IGs2SessionRequest CreateRequest(Request.GetWalletRequest request)
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
                if (request.Slot != null)
                {
                    jsonWriter.WritePropertyName("slot");
                    jsonWriter.Write(request.Slot.ToString());
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
                    "money2",
                    "wallet",
                    "getWallet",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator GetWallet(
                Request.GetWalletRequest request,
                UnityAction<AsyncResult<Result.GetWalletResult>> callback
        )
		{
			var task = new GetWalletTask(
			    Gs2WebSocketSession,
			    request
            );
            yield return task;
            callback.Invoke(new AsyncResult<Result.GetWalletResult>(task.Result, task.Error));
        }

		public IFuture<Result.GetWalletResult> GetWalletFuture(
                Request.GetWalletRequest request
        )
		{
			return new GetWalletTask(
			    Gs2WebSocketSession,
			    request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.GetWalletResult> GetWalletAsync(
            Request.GetWalletRequest request
        )
		{
		    var task = new GetWalletTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
    #else
		public GetWalletTask GetWalletAsync(
                Request.GetWalletRequest request
        )
		{
			return new GetWalletTask(
                Gs2WebSocketSession,
			    request
            );
        }
    #endif
#else
		public async Task<Result.GetWalletResult> GetWalletAsync(
            Request.GetWalletRequest request
        )
		{
		    var task = new GetWalletTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
#endif


        public class GetWalletByUserIdTask : Gs2WebSocketSessionTask<Request.GetWalletByUserIdRequest, Result.GetWalletByUserIdResult>
        {
	        public GetWalletByUserIdTask(IGs2Session session, Request.GetWalletByUserIdRequest request) : base(session, request)
	        {
	        }

            protected override IGs2SessionRequest CreateRequest(Request.GetWalletByUserIdRequest request)
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
                if (request.Slot != null)
                {
                    jsonWriter.WritePropertyName("slot");
                    jsonWriter.Write(request.Slot.ToString());
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
                    "money2",
                    "wallet",
                    "getWalletByUserId",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator GetWalletByUserId(
                Request.GetWalletByUserIdRequest request,
                UnityAction<AsyncResult<Result.GetWalletByUserIdResult>> callback
        )
		{
			var task = new GetWalletByUserIdTask(
			    Gs2WebSocketSession,
			    request
            );
            yield return task;
            callback.Invoke(new AsyncResult<Result.GetWalletByUserIdResult>(task.Result, task.Error));
        }

		public IFuture<Result.GetWalletByUserIdResult> GetWalletByUserIdFuture(
                Request.GetWalletByUserIdRequest request
        )
		{
			return new GetWalletByUserIdTask(
			    Gs2WebSocketSession,
			    request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.GetWalletByUserIdResult> GetWalletByUserIdAsync(
            Request.GetWalletByUserIdRequest request
        )
		{
		    var task = new GetWalletByUserIdTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
    #else
		public GetWalletByUserIdTask GetWalletByUserIdAsync(
                Request.GetWalletByUserIdRequest request
        )
		{
			return new GetWalletByUserIdTask(
                Gs2WebSocketSession,
			    request
            );
        }
    #endif
#else
		public async Task<Result.GetWalletByUserIdResult> GetWalletByUserIdAsync(
            Request.GetWalletByUserIdRequest request
        )
		{
		    var task = new GetWalletByUserIdTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
#endif


        public class DepositByUserIdTask : Gs2WebSocketSessionTask<Request.DepositByUserIdRequest, Result.DepositByUserIdResult>
        {
	        public DepositByUserIdTask(IGs2Session session, Request.DepositByUserIdRequest request) : base(session, request)
	        {
	        }

            protected override IGs2SessionRequest CreateRequest(Request.DepositByUserIdRequest request)
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
                if (request.Slot != null)
                {
                    jsonWriter.WritePropertyName("slot");
                    jsonWriter.Write(request.Slot.ToString());
                }
                if (request.DepositTransactions != null)
                {
                    jsonWriter.WritePropertyName("depositTransactions");
                    jsonWriter.WriteArrayStart();
                    foreach(var item in request.DepositTransactions)
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
                    "money2",
                    "wallet",
                    "depositByUserId",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }

            public override void OnError(Gs2.Core.Exception.Gs2Exception error)
            {
                if (error.Errors.Count(v => v.code == "wallet.operation.conflict") > 0) {
                    base.OnError(new Exception.ConflictException(error));
                }
                else {
                    base.OnError(error);
                }
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator DepositByUserId(
                Request.DepositByUserIdRequest request,
                UnityAction<AsyncResult<Result.DepositByUserIdResult>> callback
        )
		{
			var task = new DepositByUserIdTask(
			    Gs2WebSocketSession,
			    request
            );
            yield return task;
            callback.Invoke(new AsyncResult<Result.DepositByUserIdResult>(task.Result, task.Error));
        }

		public IFuture<Result.DepositByUserIdResult> DepositByUserIdFuture(
                Request.DepositByUserIdRequest request
        )
		{
			return new DepositByUserIdTask(
			    Gs2WebSocketSession,
			    request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.DepositByUserIdResult> DepositByUserIdAsync(
            Request.DepositByUserIdRequest request
        )
		{
		    var task = new DepositByUserIdTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
    #else
		public DepositByUserIdTask DepositByUserIdAsync(
                Request.DepositByUserIdRequest request
        )
		{
			return new DepositByUserIdTask(
                Gs2WebSocketSession,
			    request
            );
        }
    #endif
#else
		public async Task<Result.DepositByUserIdResult> DepositByUserIdAsync(
            Request.DepositByUserIdRequest request
        )
		{
		    var task = new DepositByUserIdTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
#endif


        public class DepositByStampSheetTask : Gs2WebSocketSessionTask<Request.DepositByStampSheetRequest, Result.DepositByStampSheetResult>
        {
	        public DepositByStampSheetTask(IGs2Session session, Request.DepositByStampSheetRequest request) : base(session, request)
	        {
	        }

            protected override IGs2SessionRequest CreateRequest(Request.DepositByStampSheetRequest request)
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
                    "money2",
                    "wallet",
                    "depositByStampSheet",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator DepositByStampSheet(
                Request.DepositByStampSheetRequest request,
                UnityAction<AsyncResult<Result.DepositByStampSheetResult>> callback
        )
		{
			var task = new DepositByStampSheetTask(
			    Gs2WebSocketSession,
			    request
            );
            yield return task;
            callback.Invoke(new AsyncResult<Result.DepositByStampSheetResult>(task.Result, task.Error));
        }

		public IFuture<Result.DepositByStampSheetResult> DepositByStampSheetFuture(
                Request.DepositByStampSheetRequest request
        )
		{
			return new DepositByStampSheetTask(
			    Gs2WebSocketSession,
			    request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.DepositByStampSheetResult> DepositByStampSheetAsync(
            Request.DepositByStampSheetRequest request
        )
		{
		    var task = new DepositByStampSheetTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
    #else
		public DepositByStampSheetTask DepositByStampSheetAsync(
                Request.DepositByStampSheetRequest request
        )
		{
			return new DepositByStampSheetTask(
                Gs2WebSocketSession,
			    request
            );
        }
    #endif
#else
		public async Task<Result.DepositByStampSheetResult> DepositByStampSheetAsync(
            Request.DepositByStampSheetRequest request
        )
		{
		    var task = new DepositByStampSheetTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
#endif


        public class GetRefundHistoryTask : Gs2WebSocketSessionTask<Request.GetRefundHistoryRequest, Result.GetRefundHistoryResult>
        {
	        public GetRefundHistoryTask(IGs2Session session, Request.GetRefundHistoryRequest request) : base(session, request)
	        {
	        }

            protected override IGs2SessionRequest CreateRequest(Request.GetRefundHistoryRequest request)
            {
                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);

                jsonWriter.WriteObjectStart();

                if (request.NamespaceName != null)
                {
                    jsonWriter.WritePropertyName("namespaceName");
                    jsonWriter.Write(request.NamespaceName.ToString());
                }
                if (request.TransactionId != null)
                {
                    jsonWriter.WritePropertyName("transactionId");
                    jsonWriter.Write(request.TransactionId.ToString());
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
                    "money2",
                    "refundHistory",
                    "getRefundHistory",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator GetRefundHistory(
                Request.GetRefundHistoryRequest request,
                UnityAction<AsyncResult<Result.GetRefundHistoryResult>> callback
        )
		{
			var task = new GetRefundHistoryTask(
			    Gs2WebSocketSession,
			    request
            );
            yield return task;
            callback.Invoke(new AsyncResult<Result.GetRefundHistoryResult>(task.Result, task.Error));
        }

		public IFuture<Result.GetRefundHistoryResult> GetRefundHistoryFuture(
                Request.GetRefundHistoryRequest request
        )
		{
			return new GetRefundHistoryTask(
			    Gs2WebSocketSession,
			    request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.GetRefundHistoryResult> GetRefundHistoryAsync(
            Request.GetRefundHistoryRequest request
        )
		{
		    var task = new GetRefundHistoryTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
    #else
		public GetRefundHistoryTask GetRefundHistoryAsync(
                Request.GetRefundHistoryRequest request
        )
		{
			return new GetRefundHistoryTask(
                Gs2WebSocketSession,
			    request
            );
        }
    #endif
#else
		public async Task<Result.GetRefundHistoryResult> GetRefundHistoryAsync(
            Request.GetRefundHistoryRequest request
        )
		{
		    var task = new GetRefundHistoryTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
#endif


        public class GetStoreContentModelTask : Gs2WebSocketSessionTask<Request.GetStoreContentModelRequest, Result.GetStoreContentModelResult>
        {
	        public GetStoreContentModelTask(IGs2Session session, Request.GetStoreContentModelRequest request) : base(session, request)
	        {
	        }

            protected override IGs2SessionRequest CreateRequest(Request.GetStoreContentModelRequest request)
            {
                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);

                jsonWriter.WriteObjectStart();

                if (request.NamespaceName != null)
                {
                    jsonWriter.WritePropertyName("namespaceName");
                    jsonWriter.Write(request.NamespaceName.ToString());
                }
                if (request.ContentName != null)
                {
                    jsonWriter.WritePropertyName("contentName");
                    jsonWriter.Write(request.ContentName.ToString());
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
                    "money2",
                    "storeContentModel",
                    "getStoreContentModel",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator GetStoreContentModel(
                Request.GetStoreContentModelRequest request,
                UnityAction<AsyncResult<Result.GetStoreContentModelResult>> callback
        )
		{
			var task = new GetStoreContentModelTask(
			    Gs2WebSocketSession,
			    request
            );
            yield return task;
            callback.Invoke(new AsyncResult<Result.GetStoreContentModelResult>(task.Result, task.Error));
        }

		public IFuture<Result.GetStoreContentModelResult> GetStoreContentModelFuture(
                Request.GetStoreContentModelRequest request
        )
		{
			return new GetStoreContentModelTask(
			    Gs2WebSocketSession,
			    request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.GetStoreContentModelResult> GetStoreContentModelAsync(
            Request.GetStoreContentModelRequest request
        )
		{
		    var task = new GetStoreContentModelTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
    #else
		public GetStoreContentModelTask GetStoreContentModelAsync(
                Request.GetStoreContentModelRequest request
        )
		{
			return new GetStoreContentModelTask(
                Gs2WebSocketSession,
			    request
            );
        }
    #endif
#else
		public async Task<Result.GetStoreContentModelResult> GetStoreContentModelAsync(
            Request.GetStoreContentModelRequest request
        )
		{
		    var task = new GetStoreContentModelTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
#endif


        public class CreateStoreContentModelMasterTask : Gs2WebSocketSessionTask<Request.CreateStoreContentModelMasterRequest, Result.CreateStoreContentModelMasterResult>
        {
	        public CreateStoreContentModelMasterTask(IGs2Session session, Request.CreateStoreContentModelMasterRequest request) : base(session, request)
	        {
	        }

            protected override IGs2SessionRequest CreateRequest(Request.CreateStoreContentModelMasterRequest request)
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
                if (request.AppleAppStore != null)
                {
                    jsonWriter.WritePropertyName("appleAppStore");
                    request.AppleAppStore.WriteJson(jsonWriter);
                }
                if (request.GooglePlay != null)
                {
                    jsonWriter.WritePropertyName("googlePlay");
                    request.GooglePlay.WriteJson(jsonWriter);
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
                    "money2",
                    "storeContentModelMaster",
                    "createStoreContentModelMaster",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator CreateStoreContentModelMaster(
                Request.CreateStoreContentModelMasterRequest request,
                UnityAction<AsyncResult<Result.CreateStoreContentModelMasterResult>> callback
        )
		{
			var task = new CreateStoreContentModelMasterTask(
			    Gs2WebSocketSession,
			    request
            );
            yield return task;
            callback.Invoke(new AsyncResult<Result.CreateStoreContentModelMasterResult>(task.Result, task.Error));
        }

		public IFuture<Result.CreateStoreContentModelMasterResult> CreateStoreContentModelMasterFuture(
                Request.CreateStoreContentModelMasterRequest request
        )
		{
			return new CreateStoreContentModelMasterTask(
			    Gs2WebSocketSession,
			    request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.CreateStoreContentModelMasterResult> CreateStoreContentModelMasterAsync(
            Request.CreateStoreContentModelMasterRequest request
        )
		{
		    var task = new CreateStoreContentModelMasterTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
    #else
		public CreateStoreContentModelMasterTask CreateStoreContentModelMasterAsync(
                Request.CreateStoreContentModelMasterRequest request
        )
		{
			return new CreateStoreContentModelMasterTask(
                Gs2WebSocketSession,
			    request
            );
        }
    #endif
#else
		public async Task<Result.CreateStoreContentModelMasterResult> CreateStoreContentModelMasterAsync(
            Request.CreateStoreContentModelMasterRequest request
        )
		{
		    var task = new CreateStoreContentModelMasterTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
#endif


        public class GetStoreContentModelMasterTask : Gs2WebSocketSessionTask<Request.GetStoreContentModelMasterRequest, Result.GetStoreContentModelMasterResult>
        {
	        public GetStoreContentModelMasterTask(IGs2Session session, Request.GetStoreContentModelMasterRequest request) : base(session, request)
	        {
	        }

            protected override IGs2SessionRequest CreateRequest(Request.GetStoreContentModelMasterRequest request)
            {
                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);

                jsonWriter.WriteObjectStart();

                if (request.NamespaceName != null)
                {
                    jsonWriter.WritePropertyName("namespaceName");
                    jsonWriter.Write(request.NamespaceName.ToString());
                }
                if (request.ContentName != null)
                {
                    jsonWriter.WritePropertyName("contentName");
                    jsonWriter.Write(request.ContentName.ToString());
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
                    "money2",
                    "storeContentModelMaster",
                    "getStoreContentModelMaster",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator GetStoreContentModelMaster(
                Request.GetStoreContentModelMasterRequest request,
                UnityAction<AsyncResult<Result.GetStoreContentModelMasterResult>> callback
        )
		{
			var task = new GetStoreContentModelMasterTask(
			    Gs2WebSocketSession,
			    request
            );
            yield return task;
            callback.Invoke(new AsyncResult<Result.GetStoreContentModelMasterResult>(task.Result, task.Error));
        }

		public IFuture<Result.GetStoreContentModelMasterResult> GetStoreContentModelMasterFuture(
                Request.GetStoreContentModelMasterRequest request
        )
		{
			return new GetStoreContentModelMasterTask(
			    Gs2WebSocketSession,
			    request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.GetStoreContentModelMasterResult> GetStoreContentModelMasterAsync(
            Request.GetStoreContentModelMasterRequest request
        )
		{
		    var task = new GetStoreContentModelMasterTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
    #else
		public GetStoreContentModelMasterTask GetStoreContentModelMasterAsync(
                Request.GetStoreContentModelMasterRequest request
        )
		{
			return new GetStoreContentModelMasterTask(
                Gs2WebSocketSession,
			    request
            );
        }
    #endif
#else
		public async Task<Result.GetStoreContentModelMasterResult> GetStoreContentModelMasterAsync(
            Request.GetStoreContentModelMasterRequest request
        )
		{
		    var task = new GetStoreContentModelMasterTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
#endif


        public class UpdateStoreContentModelMasterTask : Gs2WebSocketSessionTask<Request.UpdateStoreContentModelMasterRequest, Result.UpdateStoreContentModelMasterResult>
        {
	        public UpdateStoreContentModelMasterTask(IGs2Session session, Request.UpdateStoreContentModelMasterRequest request) : base(session, request)
	        {
	        }

            protected override IGs2SessionRequest CreateRequest(Request.UpdateStoreContentModelMasterRequest request)
            {
                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);

                jsonWriter.WriteObjectStart();

                if (request.NamespaceName != null)
                {
                    jsonWriter.WritePropertyName("namespaceName");
                    jsonWriter.Write(request.NamespaceName.ToString());
                }
                if (request.ContentName != null)
                {
                    jsonWriter.WritePropertyName("contentName");
                    jsonWriter.Write(request.ContentName.ToString());
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
                if (request.AppleAppStore != null)
                {
                    jsonWriter.WritePropertyName("appleAppStore");
                    request.AppleAppStore.WriteJson(jsonWriter);
                }
                if (request.GooglePlay != null)
                {
                    jsonWriter.WritePropertyName("googlePlay");
                    request.GooglePlay.WriteJson(jsonWriter);
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
                    "money2",
                    "storeContentModelMaster",
                    "updateStoreContentModelMaster",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator UpdateStoreContentModelMaster(
                Request.UpdateStoreContentModelMasterRequest request,
                UnityAction<AsyncResult<Result.UpdateStoreContentModelMasterResult>> callback
        )
		{
			var task = new UpdateStoreContentModelMasterTask(
			    Gs2WebSocketSession,
			    request
            );
            yield return task;
            callback.Invoke(new AsyncResult<Result.UpdateStoreContentModelMasterResult>(task.Result, task.Error));
        }

		public IFuture<Result.UpdateStoreContentModelMasterResult> UpdateStoreContentModelMasterFuture(
                Request.UpdateStoreContentModelMasterRequest request
        )
		{
			return new UpdateStoreContentModelMasterTask(
			    Gs2WebSocketSession,
			    request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.UpdateStoreContentModelMasterResult> UpdateStoreContentModelMasterAsync(
            Request.UpdateStoreContentModelMasterRequest request
        )
		{
		    var task = new UpdateStoreContentModelMasterTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
    #else
		public UpdateStoreContentModelMasterTask UpdateStoreContentModelMasterAsync(
                Request.UpdateStoreContentModelMasterRequest request
        )
		{
			return new UpdateStoreContentModelMasterTask(
                Gs2WebSocketSession,
			    request
            );
        }
    #endif
#else
		public async Task<Result.UpdateStoreContentModelMasterResult> UpdateStoreContentModelMasterAsync(
            Request.UpdateStoreContentModelMasterRequest request
        )
		{
		    var task = new UpdateStoreContentModelMasterTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
#endif


        public class DeleteStoreContentModelMasterTask : Gs2WebSocketSessionTask<Request.DeleteStoreContentModelMasterRequest, Result.DeleteStoreContentModelMasterResult>
        {
	        public DeleteStoreContentModelMasterTask(IGs2Session session, Request.DeleteStoreContentModelMasterRequest request) : base(session, request)
	        {
	        }

            protected override IGs2SessionRequest CreateRequest(Request.DeleteStoreContentModelMasterRequest request)
            {
                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);

                jsonWriter.WriteObjectStart();

                if (request.NamespaceName != null)
                {
                    jsonWriter.WritePropertyName("namespaceName");
                    jsonWriter.Write(request.NamespaceName.ToString());
                }
                if (request.ContentName != null)
                {
                    jsonWriter.WritePropertyName("contentName");
                    jsonWriter.Write(request.ContentName.ToString());
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
                    "money2",
                    "storeContentModelMaster",
                    "deleteStoreContentModelMaster",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator DeleteStoreContentModelMaster(
                Request.DeleteStoreContentModelMasterRequest request,
                UnityAction<AsyncResult<Result.DeleteStoreContentModelMasterResult>> callback
        )
		{
			var task = new DeleteStoreContentModelMasterTask(
			    Gs2WebSocketSession,
			    request
            );
            yield return task;
            callback.Invoke(new AsyncResult<Result.DeleteStoreContentModelMasterResult>(task.Result, task.Error));
        }

		public IFuture<Result.DeleteStoreContentModelMasterResult> DeleteStoreContentModelMasterFuture(
                Request.DeleteStoreContentModelMasterRequest request
        )
		{
			return new DeleteStoreContentModelMasterTask(
			    Gs2WebSocketSession,
			    request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.DeleteStoreContentModelMasterResult> DeleteStoreContentModelMasterAsync(
            Request.DeleteStoreContentModelMasterRequest request
        )
		{
		    var task = new DeleteStoreContentModelMasterTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
    #else
		public DeleteStoreContentModelMasterTask DeleteStoreContentModelMasterAsync(
                Request.DeleteStoreContentModelMasterRequest request
        )
		{
			return new DeleteStoreContentModelMasterTask(
                Gs2WebSocketSession,
			    request
            );
        }
    #endif
#else
		public async Task<Result.DeleteStoreContentModelMasterResult> DeleteStoreContentModelMasterAsync(
            Request.DeleteStoreContentModelMasterRequest request
        )
		{
		    var task = new DeleteStoreContentModelMasterTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
#endif


        public class GetStoreSubscriptionContentModelTask : Gs2WebSocketSessionTask<Request.GetStoreSubscriptionContentModelRequest, Result.GetStoreSubscriptionContentModelResult>
        {
	        public GetStoreSubscriptionContentModelTask(IGs2Session session, Request.GetStoreSubscriptionContentModelRequest request) : base(session, request)
	        {
	        }

            protected override IGs2SessionRequest CreateRequest(Request.GetStoreSubscriptionContentModelRequest request)
            {
                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);

                jsonWriter.WriteObjectStart();

                if (request.NamespaceName != null)
                {
                    jsonWriter.WritePropertyName("namespaceName");
                    jsonWriter.Write(request.NamespaceName.ToString());
                }
                if (request.ContentName != null)
                {
                    jsonWriter.WritePropertyName("contentName");
                    jsonWriter.Write(request.ContentName.ToString());
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
                    "money2",
                    "storeSubscriptionContentModel",
                    "getStoreSubscriptionContentModel",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator GetStoreSubscriptionContentModel(
                Request.GetStoreSubscriptionContentModelRequest request,
                UnityAction<AsyncResult<Result.GetStoreSubscriptionContentModelResult>> callback
        )
		{
			var task = new GetStoreSubscriptionContentModelTask(
			    Gs2WebSocketSession,
			    request
            );
            yield return task;
            callback.Invoke(new AsyncResult<Result.GetStoreSubscriptionContentModelResult>(task.Result, task.Error));
        }

		public IFuture<Result.GetStoreSubscriptionContentModelResult> GetStoreSubscriptionContentModelFuture(
                Request.GetStoreSubscriptionContentModelRequest request
        )
		{
			return new GetStoreSubscriptionContentModelTask(
			    Gs2WebSocketSession,
			    request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.GetStoreSubscriptionContentModelResult> GetStoreSubscriptionContentModelAsync(
            Request.GetStoreSubscriptionContentModelRequest request
        )
		{
		    var task = new GetStoreSubscriptionContentModelTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
    #else
		public GetStoreSubscriptionContentModelTask GetStoreSubscriptionContentModelAsync(
                Request.GetStoreSubscriptionContentModelRequest request
        )
		{
			return new GetStoreSubscriptionContentModelTask(
                Gs2WebSocketSession,
			    request
            );
        }
    #endif
#else
		public async Task<Result.GetStoreSubscriptionContentModelResult> GetStoreSubscriptionContentModelAsync(
            Request.GetStoreSubscriptionContentModelRequest request
        )
		{
		    var task = new GetStoreSubscriptionContentModelTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
#endif


        public class CreateStoreSubscriptionContentModelMasterTask : Gs2WebSocketSessionTask<Request.CreateStoreSubscriptionContentModelMasterRequest, Result.CreateStoreSubscriptionContentModelMasterResult>
        {
	        public CreateStoreSubscriptionContentModelMasterTask(IGs2Session session, Request.CreateStoreSubscriptionContentModelMasterRequest request) : base(session, request)
	        {
	        }

            protected override IGs2SessionRequest CreateRequest(Request.CreateStoreSubscriptionContentModelMasterRequest request)
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
                if (request.ScheduleNamespaceId != null)
                {
                    jsonWriter.WritePropertyName("scheduleNamespaceId");
                    jsonWriter.Write(request.ScheduleNamespaceId.ToString());
                }
                if (request.TriggerName != null)
                {
                    jsonWriter.WritePropertyName("triggerName");
                    jsonWriter.Write(request.TriggerName.ToString());
                }
                if (request.TriggerExtendMode != null)
                {
                    jsonWriter.WritePropertyName("triggerExtendMode");
                    jsonWriter.Write(request.TriggerExtendMode.ToString());
                }
                if (request.RollupHour != null)
                {
                    jsonWriter.WritePropertyName("rollupHour");
                    jsonWriter.Write(request.RollupHour.ToString());
                }
                if (request.ReallocateSpanDays != null)
                {
                    jsonWriter.WritePropertyName("reallocateSpanDays");
                    jsonWriter.Write(request.ReallocateSpanDays.ToString());
                }
                if (request.AppleAppStore != null)
                {
                    jsonWriter.WritePropertyName("appleAppStore");
                    request.AppleAppStore.WriteJson(jsonWriter);
                }
                if (request.GooglePlay != null)
                {
                    jsonWriter.WritePropertyName("googlePlay");
                    request.GooglePlay.WriteJson(jsonWriter);
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
                    "money2",
                    "storeSubscriptionContentModelMaster",
                    "createStoreSubscriptionContentModelMaster",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator CreateStoreSubscriptionContentModelMaster(
                Request.CreateStoreSubscriptionContentModelMasterRequest request,
                UnityAction<AsyncResult<Result.CreateStoreSubscriptionContentModelMasterResult>> callback
        )
		{
			var task = new CreateStoreSubscriptionContentModelMasterTask(
			    Gs2WebSocketSession,
			    request
            );
            yield return task;
            callback.Invoke(new AsyncResult<Result.CreateStoreSubscriptionContentModelMasterResult>(task.Result, task.Error));
        }

		public IFuture<Result.CreateStoreSubscriptionContentModelMasterResult> CreateStoreSubscriptionContentModelMasterFuture(
                Request.CreateStoreSubscriptionContentModelMasterRequest request
        )
		{
			return new CreateStoreSubscriptionContentModelMasterTask(
			    Gs2WebSocketSession,
			    request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.CreateStoreSubscriptionContentModelMasterResult> CreateStoreSubscriptionContentModelMasterAsync(
            Request.CreateStoreSubscriptionContentModelMasterRequest request
        )
		{
		    var task = new CreateStoreSubscriptionContentModelMasterTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
    #else
		public CreateStoreSubscriptionContentModelMasterTask CreateStoreSubscriptionContentModelMasterAsync(
                Request.CreateStoreSubscriptionContentModelMasterRequest request
        )
		{
			return new CreateStoreSubscriptionContentModelMasterTask(
                Gs2WebSocketSession,
			    request
            );
        }
    #endif
#else
		public async Task<Result.CreateStoreSubscriptionContentModelMasterResult> CreateStoreSubscriptionContentModelMasterAsync(
            Request.CreateStoreSubscriptionContentModelMasterRequest request
        )
		{
		    var task = new CreateStoreSubscriptionContentModelMasterTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
#endif


        public class GetStoreSubscriptionContentModelMasterTask : Gs2WebSocketSessionTask<Request.GetStoreSubscriptionContentModelMasterRequest, Result.GetStoreSubscriptionContentModelMasterResult>
        {
	        public GetStoreSubscriptionContentModelMasterTask(IGs2Session session, Request.GetStoreSubscriptionContentModelMasterRequest request) : base(session, request)
	        {
	        }

            protected override IGs2SessionRequest CreateRequest(Request.GetStoreSubscriptionContentModelMasterRequest request)
            {
                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);

                jsonWriter.WriteObjectStart();

                if (request.NamespaceName != null)
                {
                    jsonWriter.WritePropertyName("namespaceName");
                    jsonWriter.Write(request.NamespaceName.ToString());
                }
                if (request.ContentName != null)
                {
                    jsonWriter.WritePropertyName("contentName");
                    jsonWriter.Write(request.ContentName.ToString());
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
                    "money2",
                    "storeSubscriptionContentModelMaster",
                    "getStoreSubscriptionContentModelMaster",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator GetStoreSubscriptionContentModelMaster(
                Request.GetStoreSubscriptionContentModelMasterRequest request,
                UnityAction<AsyncResult<Result.GetStoreSubscriptionContentModelMasterResult>> callback
        )
		{
			var task = new GetStoreSubscriptionContentModelMasterTask(
			    Gs2WebSocketSession,
			    request
            );
            yield return task;
            callback.Invoke(new AsyncResult<Result.GetStoreSubscriptionContentModelMasterResult>(task.Result, task.Error));
        }

		public IFuture<Result.GetStoreSubscriptionContentModelMasterResult> GetStoreSubscriptionContentModelMasterFuture(
                Request.GetStoreSubscriptionContentModelMasterRequest request
        )
		{
			return new GetStoreSubscriptionContentModelMasterTask(
			    Gs2WebSocketSession,
			    request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.GetStoreSubscriptionContentModelMasterResult> GetStoreSubscriptionContentModelMasterAsync(
            Request.GetStoreSubscriptionContentModelMasterRequest request
        )
		{
		    var task = new GetStoreSubscriptionContentModelMasterTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
    #else
		public GetStoreSubscriptionContentModelMasterTask GetStoreSubscriptionContentModelMasterAsync(
                Request.GetStoreSubscriptionContentModelMasterRequest request
        )
		{
			return new GetStoreSubscriptionContentModelMasterTask(
                Gs2WebSocketSession,
			    request
            );
        }
    #endif
#else
		public async Task<Result.GetStoreSubscriptionContentModelMasterResult> GetStoreSubscriptionContentModelMasterAsync(
            Request.GetStoreSubscriptionContentModelMasterRequest request
        )
		{
		    var task = new GetStoreSubscriptionContentModelMasterTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
#endif


        public class UpdateStoreSubscriptionContentModelMasterTask : Gs2WebSocketSessionTask<Request.UpdateStoreSubscriptionContentModelMasterRequest, Result.UpdateStoreSubscriptionContentModelMasterResult>
        {
	        public UpdateStoreSubscriptionContentModelMasterTask(IGs2Session session, Request.UpdateStoreSubscriptionContentModelMasterRequest request) : base(session, request)
	        {
	        }

            protected override IGs2SessionRequest CreateRequest(Request.UpdateStoreSubscriptionContentModelMasterRequest request)
            {
                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);

                jsonWriter.WriteObjectStart();

                if (request.NamespaceName != null)
                {
                    jsonWriter.WritePropertyName("namespaceName");
                    jsonWriter.Write(request.NamespaceName.ToString());
                }
                if (request.ContentName != null)
                {
                    jsonWriter.WritePropertyName("contentName");
                    jsonWriter.Write(request.ContentName.ToString());
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
                if (request.ScheduleNamespaceId != null)
                {
                    jsonWriter.WritePropertyName("scheduleNamespaceId");
                    jsonWriter.Write(request.ScheduleNamespaceId.ToString());
                }
                if (request.TriggerName != null)
                {
                    jsonWriter.WritePropertyName("triggerName");
                    jsonWriter.Write(request.TriggerName.ToString());
                }
                if (request.TriggerExtendMode != null)
                {
                    jsonWriter.WritePropertyName("triggerExtendMode");
                    jsonWriter.Write(request.TriggerExtendMode.ToString());
                }
                if (request.RollupHour != null)
                {
                    jsonWriter.WritePropertyName("rollupHour");
                    jsonWriter.Write(request.RollupHour.ToString());
                }
                if (request.ReallocateSpanDays != null)
                {
                    jsonWriter.WritePropertyName("reallocateSpanDays");
                    jsonWriter.Write(request.ReallocateSpanDays.ToString());
                }
                if (request.AppleAppStore != null)
                {
                    jsonWriter.WritePropertyName("appleAppStore");
                    request.AppleAppStore.WriteJson(jsonWriter);
                }
                if (request.GooglePlay != null)
                {
                    jsonWriter.WritePropertyName("googlePlay");
                    request.GooglePlay.WriteJson(jsonWriter);
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
                    "money2",
                    "storeSubscriptionContentModelMaster",
                    "updateStoreSubscriptionContentModelMaster",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator UpdateStoreSubscriptionContentModelMaster(
                Request.UpdateStoreSubscriptionContentModelMasterRequest request,
                UnityAction<AsyncResult<Result.UpdateStoreSubscriptionContentModelMasterResult>> callback
        )
		{
			var task = new UpdateStoreSubscriptionContentModelMasterTask(
			    Gs2WebSocketSession,
			    request
            );
            yield return task;
            callback.Invoke(new AsyncResult<Result.UpdateStoreSubscriptionContentModelMasterResult>(task.Result, task.Error));
        }

		public IFuture<Result.UpdateStoreSubscriptionContentModelMasterResult> UpdateStoreSubscriptionContentModelMasterFuture(
                Request.UpdateStoreSubscriptionContentModelMasterRequest request
        )
		{
			return new UpdateStoreSubscriptionContentModelMasterTask(
			    Gs2WebSocketSession,
			    request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.UpdateStoreSubscriptionContentModelMasterResult> UpdateStoreSubscriptionContentModelMasterAsync(
            Request.UpdateStoreSubscriptionContentModelMasterRequest request
        )
		{
		    var task = new UpdateStoreSubscriptionContentModelMasterTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
    #else
		public UpdateStoreSubscriptionContentModelMasterTask UpdateStoreSubscriptionContentModelMasterAsync(
                Request.UpdateStoreSubscriptionContentModelMasterRequest request
        )
		{
			return new UpdateStoreSubscriptionContentModelMasterTask(
                Gs2WebSocketSession,
			    request
            );
        }
    #endif
#else
		public async Task<Result.UpdateStoreSubscriptionContentModelMasterResult> UpdateStoreSubscriptionContentModelMasterAsync(
            Request.UpdateStoreSubscriptionContentModelMasterRequest request
        )
		{
		    var task = new UpdateStoreSubscriptionContentModelMasterTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
#endif


        public class DeleteStoreSubscriptionContentModelMasterTask : Gs2WebSocketSessionTask<Request.DeleteStoreSubscriptionContentModelMasterRequest, Result.DeleteStoreSubscriptionContentModelMasterResult>
        {
	        public DeleteStoreSubscriptionContentModelMasterTask(IGs2Session session, Request.DeleteStoreSubscriptionContentModelMasterRequest request) : base(session, request)
	        {
	        }

            protected override IGs2SessionRequest CreateRequest(Request.DeleteStoreSubscriptionContentModelMasterRequest request)
            {
                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);

                jsonWriter.WriteObjectStart();

                if (request.NamespaceName != null)
                {
                    jsonWriter.WritePropertyName("namespaceName");
                    jsonWriter.Write(request.NamespaceName.ToString());
                }
                if (request.ContentName != null)
                {
                    jsonWriter.WritePropertyName("contentName");
                    jsonWriter.Write(request.ContentName.ToString());
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
                    "money2",
                    "storeSubscriptionContentModelMaster",
                    "deleteStoreSubscriptionContentModelMaster",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator DeleteStoreSubscriptionContentModelMaster(
                Request.DeleteStoreSubscriptionContentModelMasterRequest request,
                UnityAction<AsyncResult<Result.DeleteStoreSubscriptionContentModelMasterResult>> callback
        )
		{
			var task = new DeleteStoreSubscriptionContentModelMasterTask(
			    Gs2WebSocketSession,
			    request
            );
            yield return task;
            callback.Invoke(new AsyncResult<Result.DeleteStoreSubscriptionContentModelMasterResult>(task.Result, task.Error));
        }

		public IFuture<Result.DeleteStoreSubscriptionContentModelMasterResult> DeleteStoreSubscriptionContentModelMasterFuture(
                Request.DeleteStoreSubscriptionContentModelMasterRequest request
        )
		{
			return new DeleteStoreSubscriptionContentModelMasterTask(
			    Gs2WebSocketSession,
			    request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.DeleteStoreSubscriptionContentModelMasterResult> DeleteStoreSubscriptionContentModelMasterAsync(
            Request.DeleteStoreSubscriptionContentModelMasterRequest request
        )
		{
		    var task = new DeleteStoreSubscriptionContentModelMasterTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
    #else
		public DeleteStoreSubscriptionContentModelMasterTask DeleteStoreSubscriptionContentModelMasterAsync(
                Request.DeleteStoreSubscriptionContentModelMasterRequest request
        )
		{
			return new DeleteStoreSubscriptionContentModelMasterTask(
                Gs2WebSocketSession,
			    request
            );
        }
    #endif
#else
		public async Task<Result.DeleteStoreSubscriptionContentModelMasterResult> DeleteStoreSubscriptionContentModelMasterAsync(
            Request.DeleteStoreSubscriptionContentModelMasterRequest request
        )
		{
		    var task = new DeleteStoreSubscriptionContentModelMasterTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
#endif


        public class PreUpdateCurrentModelMasterTask : Gs2WebSocketSessionTask<Request.PreUpdateCurrentModelMasterRequest, Result.PreUpdateCurrentModelMasterResult>
        {
	        public PreUpdateCurrentModelMasterTask(IGs2Session session, Request.PreUpdateCurrentModelMasterRequest request) : base(session, request)
	        {
	        }

            protected override IGs2SessionRequest CreateRequest(Request.PreUpdateCurrentModelMasterRequest request)
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
                    "money2",
                    "currentModelMaster",
                    "preUpdateCurrentModelMaster",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator PreUpdateCurrentModelMaster(
                Request.PreUpdateCurrentModelMasterRequest request,
                UnityAction<AsyncResult<Result.PreUpdateCurrentModelMasterResult>> callback
        )
		{
			var task = new PreUpdateCurrentModelMasterTask(
			    Gs2WebSocketSession,
			    request
            );
            yield return task;
            callback.Invoke(new AsyncResult<Result.PreUpdateCurrentModelMasterResult>(task.Result, task.Error));
        }

		public IFuture<Result.PreUpdateCurrentModelMasterResult> PreUpdateCurrentModelMasterFuture(
                Request.PreUpdateCurrentModelMasterRequest request
        )
		{
			return new PreUpdateCurrentModelMasterTask(
			    Gs2WebSocketSession,
			    request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.PreUpdateCurrentModelMasterResult> PreUpdateCurrentModelMasterAsync(
            Request.PreUpdateCurrentModelMasterRequest request
        )
		{
		    var task = new PreUpdateCurrentModelMasterTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
    #else
		public PreUpdateCurrentModelMasterTask PreUpdateCurrentModelMasterAsync(
                Request.PreUpdateCurrentModelMasterRequest request
        )
		{
			return new PreUpdateCurrentModelMasterTask(
                Gs2WebSocketSession,
			    request
            );
        }
    #endif
#else
		public async Task<Result.PreUpdateCurrentModelMasterResult> PreUpdateCurrentModelMasterAsync(
            Request.PreUpdateCurrentModelMasterRequest request
        )
		{
		    var task = new PreUpdateCurrentModelMasterTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
#endif


        public class GetDailyTransactionHistoryTask : Gs2WebSocketSessionTask<Request.GetDailyTransactionHistoryRequest, Result.GetDailyTransactionHistoryResult>
        {
	        public GetDailyTransactionHistoryTask(IGs2Session session, Request.GetDailyTransactionHistoryRequest request) : base(session, request)
	        {
	        }

            protected override IGs2SessionRequest CreateRequest(Request.GetDailyTransactionHistoryRequest request)
            {
                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);

                jsonWriter.WriteObjectStart();

                if (request.NamespaceName != null)
                {
                    jsonWriter.WritePropertyName("namespaceName");
                    jsonWriter.Write(request.NamespaceName.ToString());
                }
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
                if (request.Day != null)
                {
                    jsonWriter.WritePropertyName("day");
                    jsonWriter.Write(request.Day.ToString());
                }
                if (request.Currency != null)
                {
                    jsonWriter.WritePropertyName("currency");
                    jsonWriter.Write(request.Currency.ToString());
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
                    "money2",
                    "dailyTransactionHistory",
                    "getDailyTransactionHistory",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator GetDailyTransactionHistory(
                Request.GetDailyTransactionHistoryRequest request,
                UnityAction<AsyncResult<Result.GetDailyTransactionHistoryResult>> callback
        )
		{
			var task = new GetDailyTransactionHistoryTask(
			    Gs2WebSocketSession,
			    request
            );
            yield return task;
            callback.Invoke(new AsyncResult<Result.GetDailyTransactionHistoryResult>(task.Result, task.Error));
        }

		public IFuture<Result.GetDailyTransactionHistoryResult> GetDailyTransactionHistoryFuture(
                Request.GetDailyTransactionHistoryRequest request
        )
		{
			return new GetDailyTransactionHistoryTask(
			    Gs2WebSocketSession,
			    request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.GetDailyTransactionHistoryResult> GetDailyTransactionHistoryAsync(
            Request.GetDailyTransactionHistoryRequest request
        )
		{
		    var task = new GetDailyTransactionHistoryTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
    #else
		public GetDailyTransactionHistoryTask GetDailyTransactionHistoryAsync(
                Request.GetDailyTransactionHistoryRequest request
        )
		{
			return new GetDailyTransactionHistoryTask(
                Gs2WebSocketSession,
			    request
            );
        }
    #endif
#else
		public async Task<Result.GetDailyTransactionHistoryResult> GetDailyTransactionHistoryAsync(
            Request.GetDailyTransactionHistoryRequest request
        )
		{
		    var task = new GetDailyTransactionHistoryTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
#endif


        public class GetUnusedBalanceTask : Gs2WebSocketSessionTask<Request.GetUnusedBalanceRequest, Result.GetUnusedBalanceResult>
        {
	        public GetUnusedBalanceTask(IGs2Session session, Request.GetUnusedBalanceRequest request) : base(session, request)
	        {
	        }

            protected override IGs2SessionRequest CreateRequest(Request.GetUnusedBalanceRequest request)
            {
                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);

                jsonWriter.WriteObjectStart();

                if (request.NamespaceName != null)
                {
                    jsonWriter.WritePropertyName("namespaceName");
                    jsonWriter.Write(request.NamespaceName.ToString());
                }
                if (request.Currency != null)
                {
                    jsonWriter.WritePropertyName("currency");
                    jsonWriter.Write(request.Currency.ToString());
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
                    "money2",
                    "unusedBalance",
                    "getUnusedBalance",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator GetUnusedBalance(
                Request.GetUnusedBalanceRequest request,
                UnityAction<AsyncResult<Result.GetUnusedBalanceResult>> callback
        )
		{
			var task = new GetUnusedBalanceTask(
			    Gs2WebSocketSession,
			    request
            );
            yield return task;
            callback.Invoke(new AsyncResult<Result.GetUnusedBalanceResult>(task.Result, task.Error));
        }

		public IFuture<Result.GetUnusedBalanceResult> GetUnusedBalanceFuture(
                Request.GetUnusedBalanceRequest request
        )
		{
			return new GetUnusedBalanceTask(
			    Gs2WebSocketSession,
			    request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.GetUnusedBalanceResult> GetUnusedBalanceAsync(
            Request.GetUnusedBalanceRequest request
        )
		{
		    var task = new GetUnusedBalanceTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
    #else
		public GetUnusedBalanceTask GetUnusedBalanceAsync(
                Request.GetUnusedBalanceRequest request
        )
		{
			return new GetUnusedBalanceTask(
                Gs2WebSocketSession,
			    request
            );
        }
    #endif
#else
		public async Task<Result.GetUnusedBalanceResult> GetUnusedBalanceAsync(
            Request.GetUnusedBalanceRequest request
        )
		{
		    var task = new GetUnusedBalanceTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
#endif
	}
}