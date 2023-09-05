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

namespace Gs2.Gs2SerialKey
{
	public class Gs2SerialKeyWebSocketClient : AbstractGs2Client
	{

		public static string Endpoint = "serial-key";

        protected Gs2WebSocketSession Gs2WebSocketSession => (Gs2WebSocketSession) Gs2Session;

		public Gs2SerialKeyWebSocketClient(Gs2WebSocketSession Gs2WebSocketSession) : base(Gs2WebSocketSession)
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
                    "serialKey",
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
                    "serialKey",
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
                    "serialKey",
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
                    "serialKey",
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


        public class GetIssueJobTask : Gs2WebSocketSessionTask<Request.GetIssueJobRequest, Result.GetIssueJobResult>
        {
	        public GetIssueJobTask(IGs2Session session, Request.GetIssueJobRequest request) : base(session, request)
	        {
	        }

            protected override IGs2SessionRequest CreateRequest(Request.GetIssueJobRequest request)
            {
                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);

                jsonWriter.WriteObjectStart();

                if (request.NamespaceName != null)
                {
                    jsonWriter.WritePropertyName("namespaceName");
                    jsonWriter.Write(request.NamespaceName.ToString());
                }
                if (request.CampaignModelName != null)
                {
                    jsonWriter.WritePropertyName("campaignModelName");
                    jsonWriter.Write(request.CampaignModelName.ToString());
                }
                if (request.IssueJobName != null)
                {
                    jsonWriter.WritePropertyName("issueJobName");
                    jsonWriter.Write(request.IssueJobName.ToString());
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
                    "serialKey",
                    "issueJob",
                    "getIssueJob",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator GetIssueJob(
                Request.GetIssueJobRequest request,
                UnityAction<AsyncResult<Result.GetIssueJobResult>> callback
        )
		{
			var task = new GetIssueJobTask(
			    Gs2WebSocketSession,
			    request
            );
            yield return task;
            callback.Invoke(new AsyncResult<Result.GetIssueJobResult>(task.Result, task.Error));
        }

		public IFuture<Result.GetIssueJobResult> GetIssueJobFuture(
                Request.GetIssueJobRequest request
        )
		{
			return new GetIssueJobTask(
			    Gs2WebSocketSession,
			    request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.GetIssueJobResult> GetIssueJobAsync(
            Request.GetIssueJobRequest request
        )
		{
		    var task = new GetIssueJobTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
    #else
		public GetIssueJobTask GetIssueJobAsync(
                Request.GetIssueJobRequest request
        )
		{
			return new GetIssueJobTask(
                Gs2WebSocketSession,
			    request
            );
        }
    #endif
#else
		public async Task<Result.GetIssueJobResult> GetIssueJobAsync(
            Request.GetIssueJobRequest request
        )
		{
		    var task = new GetIssueJobTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
#endif


        public class IssueTask : Gs2WebSocketSessionTask<Request.IssueRequest, Result.IssueResult>
        {
	        public IssueTask(IGs2Session session, Request.IssueRequest request) : base(session, request)
	        {
	        }

            protected override IGs2SessionRequest CreateRequest(Request.IssueRequest request)
            {
                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);

                jsonWriter.WriteObjectStart();

                if (request.NamespaceName != null)
                {
                    jsonWriter.WritePropertyName("namespaceName");
                    jsonWriter.Write(request.NamespaceName.ToString());
                }
                if (request.CampaignModelName != null)
                {
                    jsonWriter.WritePropertyName("campaignModelName");
                    jsonWriter.Write(request.CampaignModelName.ToString());
                }
                if (request.Metadata != null)
                {
                    jsonWriter.WritePropertyName("metadata");
                    jsonWriter.Write(request.Metadata.ToString());
                }
                if (request.IssueRequestCount != null)
                {
                    jsonWriter.WritePropertyName("issueRequestCount");
                    jsonWriter.Write(request.IssueRequestCount.ToString());
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
                    "serialKey",
                    "issueJob",
                    "issue",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator Issue(
                Request.IssueRequest request,
                UnityAction<AsyncResult<Result.IssueResult>> callback
        )
		{
			var task = new IssueTask(
			    Gs2WebSocketSession,
			    request
            );
            yield return task;
            callback.Invoke(new AsyncResult<Result.IssueResult>(task.Result, task.Error));
        }

		public IFuture<Result.IssueResult> IssueFuture(
                Request.IssueRequest request
        )
		{
			return new IssueTask(
			    Gs2WebSocketSession,
			    request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.IssueResult> IssueAsync(
            Request.IssueRequest request
        )
		{
		    var task = new IssueTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
    #else
		public IssueTask IssueAsync(
                Request.IssueRequest request
        )
		{
			return new IssueTask(
                Gs2WebSocketSession,
			    request
            );
        }
    #endif
#else
		public async Task<Result.IssueResult> IssueAsync(
            Request.IssueRequest request
        )
		{
		    var task = new IssueTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
#endif


        public class DownloadSerialCodesTask : Gs2WebSocketSessionTask<Request.DownloadSerialCodesRequest, Result.DownloadSerialCodesResult>
        {
	        public DownloadSerialCodesTask(IGs2Session session, Request.DownloadSerialCodesRequest request) : base(session, request)
	        {
	        }

            protected override IGs2SessionRequest CreateRequest(Request.DownloadSerialCodesRequest request)
            {
                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);

                jsonWriter.WriteObjectStart();

                if (request.NamespaceName != null)
                {
                    jsonWriter.WritePropertyName("namespaceName");
                    jsonWriter.Write(request.NamespaceName.ToString());
                }
                if (request.CampaignModelName != null)
                {
                    jsonWriter.WritePropertyName("campaignModelName");
                    jsonWriter.Write(request.CampaignModelName.ToString());
                }
                if (request.IssueJobName != null)
                {
                    jsonWriter.WritePropertyName("issueJobName");
                    jsonWriter.Write(request.IssueJobName.ToString());
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
                    "serialKey",
                    "serialKey",
                    "downloadSerialCodes",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator DownloadSerialCodes(
                Request.DownloadSerialCodesRequest request,
                UnityAction<AsyncResult<Result.DownloadSerialCodesResult>> callback
        )
		{
			var task = new DownloadSerialCodesTask(
			    Gs2WebSocketSession,
			    request
            );
            yield return task;
            callback.Invoke(new AsyncResult<Result.DownloadSerialCodesResult>(task.Result, task.Error));
        }

		public IFuture<Result.DownloadSerialCodesResult> DownloadSerialCodesFuture(
                Request.DownloadSerialCodesRequest request
        )
		{
			return new DownloadSerialCodesTask(
			    Gs2WebSocketSession,
			    request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.DownloadSerialCodesResult> DownloadSerialCodesAsync(
            Request.DownloadSerialCodesRequest request
        )
		{
		    var task = new DownloadSerialCodesTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
    #else
		public DownloadSerialCodesTask DownloadSerialCodesAsync(
                Request.DownloadSerialCodesRequest request
        )
		{
			return new DownloadSerialCodesTask(
                Gs2WebSocketSession,
			    request
            );
        }
    #endif
#else
		public async Task<Result.DownloadSerialCodesResult> DownloadSerialCodesAsync(
            Request.DownloadSerialCodesRequest request
        )
		{
		    var task = new DownloadSerialCodesTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
#endif


        public class GetSerialKeyTask : Gs2WebSocketSessionTask<Request.GetSerialKeyRequest, Result.GetSerialKeyResult>
        {
	        public GetSerialKeyTask(IGs2Session session, Request.GetSerialKeyRequest request) : base(session, request)
	        {
	        }

            protected override IGs2SessionRequest CreateRequest(Request.GetSerialKeyRequest request)
            {
                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);

                jsonWriter.WriteObjectStart();

                if (request.NamespaceName != null)
                {
                    jsonWriter.WritePropertyName("namespaceName");
                    jsonWriter.Write(request.NamespaceName.ToString());
                }
                if (request.Code != null)
                {
                    jsonWriter.WritePropertyName("code");
                    jsonWriter.Write(request.Code.ToString());
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
                    "serialKey",
                    "serialKey",
                    "getSerialKey",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator GetSerialKey(
                Request.GetSerialKeyRequest request,
                UnityAction<AsyncResult<Result.GetSerialKeyResult>> callback
        )
		{
			var task = new GetSerialKeyTask(
			    Gs2WebSocketSession,
			    request
            );
            yield return task;
            callback.Invoke(new AsyncResult<Result.GetSerialKeyResult>(task.Result, task.Error));
        }

		public IFuture<Result.GetSerialKeyResult> GetSerialKeyFuture(
                Request.GetSerialKeyRequest request
        )
		{
			return new GetSerialKeyTask(
			    Gs2WebSocketSession,
			    request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.GetSerialKeyResult> GetSerialKeyAsync(
            Request.GetSerialKeyRequest request
        )
		{
		    var task = new GetSerialKeyTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
    #else
		public GetSerialKeyTask GetSerialKeyAsync(
                Request.GetSerialKeyRequest request
        )
		{
			return new GetSerialKeyTask(
                Gs2WebSocketSession,
			    request
            );
        }
    #endif
#else
		public async Task<Result.GetSerialKeyResult> GetSerialKeyAsync(
            Request.GetSerialKeyRequest request
        )
		{
		    var task = new GetSerialKeyTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
#endif


        public class UseTask : Gs2WebSocketSessionTask<Request.UseRequest, Result.UseResult>
        {
	        public UseTask(IGs2Session session, Request.UseRequest request) : base(session, request)
	        {
	        }

            protected override IGs2SessionRequest CreateRequest(Request.UseRequest request)
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
                if (request.Code != null)
                {
                    jsonWriter.WritePropertyName("code");
                    jsonWriter.Write(request.Code.ToString());
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
                if (request.DuplicationAvoider != null)
                {
                    jsonWriter.WritePropertyName("xGs2DuplicationAvoider");
                    jsonWriter.Write(request.DuplicationAvoider);
                }

                AddHeader(
                    Session.Credential,
                    "serialKey",
                    "serialKey",
                    "use",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }

            public override void OnError(Gs2.Core.Exception.Gs2Exception error)
            {
                if (error.Errors.Count(v => v.code == "code.status.invalid") > 0) {
                    base.OnError(new Exception.AlreadyUsedException(error));
                }
                else if (error.Errors.Count(v => v.code == "code.code.notFound") > 0) {
                    base.OnError(new Exception.CodeNotFoundException(error));
                }
                else {
                    base.OnError(error);
                }
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator Use(
                Request.UseRequest request,
                UnityAction<AsyncResult<Result.UseResult>> callback
        )
		{
			var task = new UseTask(
			    Gs2WebSocketSession,
			    request
            );
            yield return task;
            callback.Invoke(new AsyncResult<Result.UseResult>(task.Result, task.Error));
        }

		public IFuture<Result.UseResult> UseFuture(
                Request.UseRequest request
        )
		{
			return new UseTask(
			    Gs2WebSocketSession,
			    request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.UseResult> UseAsync(
            Request.UseRequest request
        )
		{
		    var task = new UseTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
    #else
		public UseTask UseAsync(
                Request.UseRequest request
        )
		{
			return new UseTask(
                Gs2WebSocketSession,
			    request
            );
        }
    #endif
#else
		public async Task<Result.UseResult> UseAsync(
            Request.UseRequest request
        )
		{
		    var task = new UseTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
#endif


        public class UseByUserIdTask : Gs2WebSocketSessionTask<Request.UseByUserIdRequest, Result.UseByUserIdResult>
        {
	        public UseByUserIdTask(IGs2Session session, Request.UseByUserIdRequest request) : base(session, request)
	        {
	        }

            protected override IGs2SessionRequest CreateRequest(Request.UseByUserIdRequest request)
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
                if (request.Code != null)
                {
                    jsonWriter.WritePropertyName("code");
                    jsonWriter.Write(request.Code.ToString());
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
                    "serialKey",
                    "serialKey",
                    "useByUserId",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator UseByUserId(
                Request.UseByUserIdRequest request,
                UnityAction<AsyncResult<Result.UseByUserIdResult>> callback
        )
		{
			var task = new UseByUserIdTask(
			    Gs2WebSocketSession,
			    request
            );
            yield return task;
            callback.Invoke(new AsyncResult<Result.UseByUserIdResult>(task.Result, task.Error));
        }

		public IFuture<Result.UseByUserIdResult> UseByUserIdFuture(
                Request.UseByUserIdRequest request
        )
		{
			return new UseByUserIdTask(
			    Gs2WebSocketSession,
			    request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.UseByUserIdResult> UseByUserIdAsync(
            Request.UseByUserIdRequest request
        )
		{
		    var task = new UseByUserIdTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
    #else
		public UseByUserIdTask UseByUserIdAsync(
                Request.UseByUserIdRequest request
        )
		{
			return new UseByUserIdTask(
                Gs2WebSocketSession,
			    request
            );
        }
    #endif
#else
		public async Task<Result.UseByUserIdResult> UseByUserIdAsync(
            Request.UseByUserIdRequest request
        )
		{
		    var task = new UseByUserIdTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
#endif


        public class RevertUseByUserIdTask : Gs2WebSocketSessionTask<Request.RevertUseByUserIdRequest, Result.RevertUseByUserIdResult>
        {
	        public RevertUseByUserIdTask(IGs2Session session, Request.RevertUseByUserIdRequest request) : base(session, request)
	        {
	        }

            protected override IGs2SessionRequest CreateRequest(Request.RevertUseByUserIdRequest request)
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
                if (request.Code != null)
                {
                    jsonWriter.WritePropertyName("code");
                    jsonWriter.Write(request.Code.ToString());
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
                    "serialKey",
                    "serialKey",
                    "revertUseByUserId",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator RevertUseByUserId(
                Request.RevertUseByUserIdRequest request,
                UnityAction<AsyncResult<Result.RevertUseByUserIdResult>> callback
        )
		{
			var task = new RevertUseByUserIdTask(
			    Gs2WebSocketSession,
			    request
            );
            yield return task;
            callback.Invoke(new AsyncResult<Result.RevertUseByUserIdResult>(task.Result, task.Error));
        }

		public IFuture<Result.RevertUseByUserIdResult> RevertUseByUserIdFuture(
                Request.RevertUseByUserIdRequest request
        )
		{
			return new RevertUseByUserIdTask(
			    Gs2WebSocketSession,
			    request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.RevertUseByUserIdResult> RevertUseByUserIdAsync(
            Request.RevertUseByUserIdRequest request
        )
		{
		    var task = new RevertUseByUserIdTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
    #else
		public RevertUseByUserIdTask RevertUseByUserIdAsync(
                Request.RevertUseByUserIdRequest request
        )
		{
			return new RevertUseByUserIdTask(
                Gs2WebSocketSession,
			    request
            );
        }
    #endif
#else
		public async Task<Result.RevertUseByUserIdResult> RevertUseByUserIdAsync(
            Request.RevertUseByUserIdRequest request
        )
		{
		    var task = new RevertUseByUserIdTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
#endif


        public class RevertUseByStampSheetTask : Gs2WebSocketSessionTask<Request.RevertUseByStampSheetRequest, Result.RevertUseByStampSheetResult>
        {
	        public RevertUseByStampSheetTask(IGs2Session session, Request.RevertUseByStampSheetRequest request) : base(session, request)
	        {
	        }

            protected override IGs2SessionRequest CreateRequest(Request.RevertUseByStampSheetRequest request)
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
                    "serialKey",
                    "serialKey",
                    "revertUseByStampSheet",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator RevertUseByStampSheet(
                Request.RevertUseByStampSheetRequest request,
                UnityAction<AsyncResult<Result.RevertUseByStampSheetResult>> callback
        )
		{
			var task = new RevertUseByStampSheetTask(
			    Gs2WebSocketSession,
			    request
            );
            yield return task;
            callback.Invoke(new AsyncResult<Result.RevertUseByStampSheetResult>(task.Result, task.Error));
        }

		public IFuture<Result.RevertUseByStampSheetResult> RevertUseByStampSheetFuture(
                Request.RevertUseByStampSheetRequest request
        )
		{
			return new RevertUseByStampSheetTask(
			    Gs2WebSocketSession,
			    request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.RevertUseByStampSheetResult> RevertUseByStampSheetAsync(
            Request.RevertUseByStampSheetRequest request
        )
		{
		    var task = new RevertUseByStampSheetTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
    #else
		public RevertUseByStampSheetTask RevertUseByStampSheetAsync(
                Request.RevertUseByStampSheetRequest request
        )
		{
			return new RevertUseByStampSheetTask(
                Gs2WebSocketSession,
			    request
            );
        }
    #endif
#else
		public async Task<Result.RevertUseByStampSheetResult> RevertUseByStampSheetAsync(
            Request.RevertUseByStampSheetRequest request
        )
		{
		    var task = new RevertUseByStampSheetTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
#endif


        public class GetCampaignModelTask : Gs2WebSocketSessionTask<Request.GetCampaignModelRequest, Result.GetCampaignModelResult>
        {
	        public GetCampaignModelTask(IGs2Session session, Request.GetCampaignModelRequest request) : base(session, request)
	        {
	        }

            protected override IGs2SessionRequest CreateRequest(Request.GetCampaignModelRequest request)
            {
                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);

                jsonWriter.WriteObjectStart();

                if (request.NamespaceName != null)
                {
                    jsonWriter.WritePropertyName("namespaceName");
                    jsonWriter.Write(request.NamespaceName.ToString());
                }
                if (request.CampaignModelName != null)
                {
                    jsonWriter.WritePropertyName("campaignModelName");
                    jsonWriter.Write(request.CampaignModelName.ToString());
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
                    "serialKey",
                    "campaignModel",
                    "getCampaignModel",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator GetCampaignModel(
                Request.GetCampaignModelRequest request,
                UnityAction<AsyncResult<Result.GetCampaignModelResult>> callback
        )
		{
			var task = new GetCampaignModelTask(
			    Gs2WebSocketSession,
			    request
            );
            yield return task;
            callback.Invoke(new AsyncResult<Result.GetCampaignModelResult>(task.Result, task.Error));
        }

		public IFuture<Result.GetCampaignModelResult> GetCampaignModelFuture(
                Request.GetCampaignModelRequest request
        )
		{
			return new GetCampaignModelTask(
			    Gs2WebSocketSession,
			    request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.GetCampaignModelResult> GetCampaignModelAsync(
            Request.GetCampaignModelRequest request
        )
		{
		    var task = new GetCampaignModelTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
    #else
		public GetCampaignModelTask GetCampaignModelAsync(
                Request.GetCampaignModelRequest request
        )
		{
			return new GetCampaignModelTask(
                Gs2WebSocketSession,
			    request
            );
        }
    #endif
#else
		public async Task<Result.GetCampaignModelResult> GetCampaignModelAsync(
            Request.GetCampaignModelRequest request
        )
		{
		    var task = new GetCampaignModelTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
#endif


        public class CreateCampaignModelMasterTask : Gs2WebSocketSessionTask<Request.CreateCampaignModelMasterRequest, Result.CreateCampaignModelMasterResult>
        {
	        public CreateCampaignModelMasterTask(IGs2Session session, Request.CreateCampaignModelMasterRequest request) : base(session, request)
	        {
	        }

            protected override IGs2SessionRequest CreateRequest(Request.CreateCampaignModelMasterRequest request)
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
                if (request.EnableCampaignCode != null)
                {
                    jsonWriter.WritePropertyName("enableCampaignCode");
                    jsonWriter.Write(request.EnableCampaignCode.ToString());
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
                    "serialKey",
                    "campaignModelMaster",
                    "createCampaignModelMaster",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator CreateCampaignModelMaster(
                Request.CreateCampaignModelMasterRequest request,
                UnityAction<AsyncResult<Result.CreateCampaignModelMasterResult>> callback
        )
		{
			var task = new CreateCampaignModelMasterTask(
			    Gs2WebSocketSession,
			    request
            );
            yield return task;
            callback.Invoke(new AsyncResult<Result.CreateCampaignModelMasterResult>(task.Result, task.Error));
        }

		public IFuture<Result.CreateCampaignModelMasterResult> CreateCampaignModelMasterFuture(
                Request.CreateCampaignModelMasterRequest request
        )
		{
			return new CreateCampaignModelMasterTask(
			    Gs2WebSocketSession,
			    request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.CreateCampaignModelMasterResult> CreateCampaignModelMasterAsync(
            Request.CreateCampaignModelMasterRequest request
        )
		{
		    var task = new CreateCampaignModelMasterTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
    #else
		public CreateCampaignModelMasterTask CreateCampaignModelMasterAsync(
                Request.CreateCampaignModelMasterRequest request
        )
		{
			return new CreateCampaignModelMasterTask(
                Gs2WebSocketSession,
			    request
            );
        }
    #endif
#else
		public async Task<Result.CreateCampaignModelMasterResult> CreateCampaignModelMasterAsync(
            Request.CreateCampaignModelMasterRequest request
        )
		{
		    var task = new CreateCampaignModelMasterTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
#endif


        public class GetCampaignModelMasterTask : Gs2WebSocketSessionTask<Request.GetCampaignModelMasterRequest, Result.GetCampaignModelMasterResult>
        {
	        public GetCampaignModelMasterTask(IGs2Session session, Request.GetCampaignModelMasterRequest request) : base(session, request)
	        {
	        }

            protected override IGs2SessionRequest CreateRequest(Request.GetCampaignModelMasterRequest request)
            {
                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);

                jsonWriter.WriteObjectStart();

                if (request.NamespaceName != null)
                {
                    jsonWriter.WritePropertyName("namespaceName");
                    jsonWriter.Write(request.NamespaceName.ToString());
                }
                if (request.CampaignModelName != null)
                {
                    jsonWriter.WritePropertyName("campaignModelName");
                    jsonWriter.Write(request.CampaignModelName.ToString());
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
                    "serialKey",
                    "campaignModelMaster",
                    "getCampaignModelMaster",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator GetCampaignModelMaster(
                Request.GetCampaignModelMasterRequest request,
                UnityAction<AsyncResult<Result.GetCampaignModelMasterResult>> callback
        )
		{
			var task = new GetCampaignModelMasterTask(
			    Gs2WebSocketSession,
			    request
            );
            yield return task;
            callback.Invoke(new AsyncResult<Result.GetCampaignModelMasterResult>(task.Result, task.Error));
        }

		public IFuture<Result.GetCampaignModelMasterResult> GetCampaignModelMasterFuture(
                Request.GetCampaignModelMasterRequest request
        )
		{
			return new GetCampaignModelMasterTask(
			    Gs2WebSocketSession,
			    request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.GetCampaignModelMasterResult> GetCampaignModelMasterAsync(
            Request.GetCampaignModelMasterRequest request
        )
		{
		    var task = new GetCampaignModelMasterTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
    #else
		public GetCampaignModelMasterTask GetCampaignModelMasterAsync(
                Request.GetCampaignModelMasterRequest request
        )
		{
			return new GetCampaignModelMasterTask(
                Gs2WebSocketSession,
			    request
            );
        }
    #endif
#else
		public async Task<Result.GetCampaignModelMasterResult> GetCampaignModelMasterAsync(
            Request.GetCampaignModelMasterRequest request
        )
		{
		    var task = new GetCampaignModelMasterTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
#endif


        public class UpdateCampaignModelMasterTask : Gs2WebSocketSessionTask<Request.UpdateCampaignModelMasterRequest, Result.UpdateCampaignModelMasterResult>
        {
	        public UpdateCampaignModelMasterTask(IGs2Session session, Request.UpdateCampaignModelMasterRequest request) : base(session, request)
	        {
	        }

            protected override IGs2SessionRequest CreateRequest(Request.UpdateCampaignModelMasterRequest request)
            {
                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);

                jsonWriter.WriteObjectStart();

                if (request.NamespaceName != null)
                {
                    jsonWriter.WritePropertyName("namespaceName");
                    jsonWriter.Write(request.NamespaceName.ToString());
                }
                if (request.CampaignModelName != null)
                {
                    jsonWriter.WritePropertyName("campaignModelName");
                    jsonWriter.Write(request.CampaignModelName.ToString());
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
                if (request.EnableCampaignCode != null)
                {
                    jsonWriter.WritePropertyName("enableCampaignCode");
                    jsonWriter.Write(request.EnableCampaignCode.ToString());
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
                    "serialKey",
                    "campaignModelMaster",
                    "updateCampaignModelMaster",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator UpdateCampaignModelMaster(
                Request.UpdateCampaignModelMasterRequest request,
                UnityAction<AsyncResult<Result.UpdateCampaignModelMasterResult>> callback
        )
		{
			var task = new UpdateCampaignModelMasterTask(
			    Gs2WebSocketSession,
			    request
            );
            yield return task;
            callback.Invoke(new AsyncResult<Result.UpdateCampaignModelMasterResult>(task.Result, task.Error));
        }

		public IFuture<Result.UpdateCampaignModelMasterResult> UpdateCampaignModelMasterFuture(
                Request.UpdateCampaignModelMasterRequest request
        )
		{
			return new UpdateCampaignModelMasterTask(
			    Gs2WebSocketSession,
			    request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.UpdateCampaignModelMasterResult> UpdateCampaignModelMasterAsync(
            Request.UpdateCampaignModelMasterRequest request
        )
		{
		    var task = new UpdateCampaignModelMasterTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
    #else
		public UpdateCampaignModelMasterTask UpdateCampaignModelMasterAsync(
                Request.UpdateCampaignModelMasterRequest request
        )
		{
			return new UpdateCampaignModelMasterTask(
                Gs2WebSocketSession,
			    request
            );
        }
    #endif
#else
		public async Task<Result.UpdateCampaignModelMasterResult> UpdateCampaignModelMasterAsync(
            Request.UpdateCampaignModelMasterRequest request
        )
		{
		    var task = new UpdateCampaignModelMasterTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
#endif


        public class DeleteCampaignModelMasterTask : Gs2WebSocketSessionTask<Request.DeleteCampaignModelMasterRequest, Result.DeleteCampaignModelMasterResult>
        {
	        public DeleteCampaignModelMasterTask(IGs2Session session, Request.DeleteCampaignModelMasterRequest request) : base(session, request)
	        {
	        }

            protected override IGs2SessionRequest CreateRequest(Request.DeleteCampaignModelMasterRequest request)
            {
                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);

                jsonWriter.WriteObjectStart();

                if (request.NamespaceName != null)
                {
                    jsonWriter.WritePropertyName("namespaceName");
                    jsonWriter.Write(request.NamespaceName.ToString());
                }
                if (request.CampaignModelName != null)
                {
                    jsonWriter.WritePropertyName("campaignModelName");
                    jsonWriter.Write(request.CampaignModelName.ToString());
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
                    "serialKey",
                    "campaignModelMaster",
                    "deleteCampaignModelMaster",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator DeleteCampaignModelMaster(
                Request.DeleteCampaignModelMasterRequest request,
                UnityAction<AsyncResult<Result.DeleteCampaignModelMasterResult>> callback
        )
		{
			var task = new DeleteCampaignModelMasterTask(
			    Gs2WebSocketSession,
			    request
            );
            yield return task;
            callback.Invoke(new AsyncResult<Result.DeleteCampaignModelMasterResult>(task.Result, task.Error));
        }

		public IFuture<Result.DeleteCampaignModelMasterResult> DeleteCampaignModelMasterFuture(
                Request.DeleteCampaignModelMasterRequest request
        )
		{
			return new DeleteCampaignModelMasterTask(
			    Gs2WebSocketSession,
			    request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.DeleteCampaignModelMasterResult> DeleteCampaignModelMasterAsync(
            Request.DeleteCampaignModelMasterRequest request
        )
		{
		    var task = new DeleteCampaignModelMasterTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
    #else
		public DeleteCampaignModelMasterTask DeleteCampaignModelMasterAsync(
                Request.DeleteCampaignModelMasterRequest request
        )
		{
			return new DeleteCampaignModelMasterTask(
                Gs2WebSocketSession,
			    request
            );
        }
    #endif
#else
		public async Task<Result.DeleteCampaignModelMasterResult> DeleteCampaignModelMasterAsync(
            Request.DeleteCampaignModelMasterRequest request
        )
		{
		    var task = new DeleteCampaignModelMasterTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
#endif
	}
}