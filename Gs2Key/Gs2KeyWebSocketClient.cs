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

namespace Gs2.Gs2Key
{
	public class Gs2KeyWebSocketClient : AbstractGs2Client
	{

		public static string Endpoint = "key";

        protected Gs2WebSocketSession Gs2WebSocketSession => (Gs2WebSocketSession) Gs2Session;

		public Gs2KeyWebSocketClient(Gs2WebSocketSession Gs2WebSocketSession) : base(Gs2WebSocketSession)
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
                    "key",
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
                    "key",
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
                    "key",
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
                    "key",
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


        private class CreateKeyTask : Gs2WebSocketSessionTask<Request.CreateKeyRequest, Result.CreateKeyResult>
        {
	        public CreateKeyTask(IGs2Session session, Request.CreateKeyRequest request) : base(session, request)
	        {
	        }

            protected override IGs2SessionRequest CreateRequest(Request.CreateKeyRequest request)
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
                    "key",
                    "key",
                    "createKey",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator CreateKey(
                Request.CreateKeyRequest request,
                UnityAction<AsyncResult<Result.CreateKeyResult>> callback
        )
		{
			var task = new CreateKeyTask(
			    Gs2WebSocketSession,
			    request
            );
            yield return task;
            callback.Invoke(new AsyncResult<Result.CreateKeyResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.CreateKeyResult> CreateKey(
            Request.CreateKeyRequest request
        )
		{
		    var task = new CreateKeyTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
#endif


        private class UpdateKeyTask : Gs2WebSocketSessionTask<Request.UpdateKeyRequest, Result.UpdateKeyResult>
        {
	        public UpdateKeyTask(IGs2Session session, Request.UpdateKeyRequest request) : base(session, request)
	        {
	        }

            protected override IGs2SessionRequest CreateRequest(Request.UpdateKeyRequest request)
            {
                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);

                jsonWriter.WriteObjectStart();

                if (request.NamespaceName != null)
                {
                    jsonWriter.WritePropertyName("namespaceName");
                    jsonWriter.Write(request.NamespaceName.ToString());
                }
                if (request.KeyName != null)
                {
                    jsonWriter.WritePropertyName("keyName");
                    jsonWriter.Write(request.KeyName.ToString());
                }
                if (request.Description != null)
                {
                    jsonWriter.WritePropertyName("description");
                    jsonWriter.Write(request.Description.ToString());
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
                    "key",
                    "key",
                    "updateKey",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator UpdateKey(
                Request.UpdateKeyRequest request,
                UnityAction<AsyncResult<Result.UpdateKeyResult>> callback
        )
		{
			var task = new UpdateKeyTask(
			    Gs2WebSocketSession,
			    request
            );
            yield return task;
            callback.Invoke(new AsyncResult<Result.UpdateKeyResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.UpdateKeyResult> UpdateKey(
            Request.UpdateKeyRequest request
        )
		{
		    var task = new UpdateKeyTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
#endif


        private class GetKeyTask : Gs2WebSocketSessionTask<Request.GetKeyRequest, Result.GetKeyResult>
        {
	        public GetKeyTask(IGs2Session session, Request.GetKeyRequest request) : base(session, request)
	        {
	        }

            protected override IGs2SessionRequest CreateRequest(Request.GetKeyRequest request)
            {
                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);

                jsonWriter.WriteObjectStart();

                if (request.NamespaceName != null)
                {
                    jsonWriter.WritePropertyName("namespaceName");
                    jsonWriter.Write(request.NamespaceName.ToString());
                }
                if (request.KeyName != null)
                {
                    jsonWriter.WritePropertyName("keyName");
                    jsonWriter.Write(request.KeyName.ToString());
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
                    "key",
                    "key",
                    "getKey",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator GetKey(
                Request.GetKeyRequest request,
                UnityAction<AsyncResult<Result.GetKeyResult>> callback
        )
		{
			var task = new GetKeyTask(
			    Gs2WebSocketSession,
			    request
            );
            yield return task;
            callback.Invoke(new AsyncResult<Result.GetKeyResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.GetKeyResult> GetKey(
            Request.GetKeyRequest request
        )
		{
		    var task = new GetKeyTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
#endif


        private class DeleteKeyTask : Gs2WebSocketSessionTask<Request.DeleteKeyRequest, Result.DeleteKeyResult>
        {
	        public DeleteKeyTask(IGs2Session session, Request.DeleteKeyRequest request) : base(session, request)
	        {
	        }

            protected override IGs2SessionRequest CreateRequest(Request.DeleteKeyRequest request)
            {
                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);

                jsonWriter.WriteObjectStart();

                if (request.NamespaceName != null)
                {
                    jsonWriter.WritePropertyName("namespaceName");
                    jsonWriter.Write(request.NamespaceName.ToString());
                }
                if (request.KeyName != null)
                {
                    jsonWriter.WritePropertyName("keyName");
                    jsonWriter.Write(request.KeyName.ToString());
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
                    "key",
                    "key",
                    "deleteKey",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator DeleteKey(
                Request.DeleteKeyRequest request,
                UnityAction<AsyncResult<Result.DeleteKeyResult>> callback
        )
		{
			var task = new DeleteKeyTask(
			    Gs2WebSocketSession,
			    request
            );
            yield return task;
            callback.Invoke(new AsyncResult<Result.DeleteKeyResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.DeleteKeyResult> DeleteKey(
            Request.DeleteKeyRequest request
        )
		{
		    var task = new DeleteKeyTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
#endif


        private class CreateGitHubApiKeyTask : Gs2WebSocketSessionTask<Request.CreateGitHubApiKeyRequest, Result.CreateGitHubApiKeyResult>
        {
	        public CreateGitHubApiKeyTask(IGs2Session session, Request.CreateGitHubApiKeyRequest request) : base(session, request)
	        {
	        }

            protected override IGs2SessionRequest CreateRequest(Request.CreateGitHubApiKeyRequest request)
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
                if (request.ApiKey != null)
                {
                    jsonWriter.WritePropertyName("apiKey");
                    jsonWriter.Write(request.ApiKey.ToString());
                }
                if (request.EncryptionKeyName != null)
                {
                    jsonWriter.WritePropertyName("encryptionKeyName");
                    jsonWriter.Write(request.EncryptionKeyName.ToString());
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
                    "key",
                    "gitHubApiKey",
                    "createGitHubApiKey",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator CreateGitHubApiKey(
                Request.CreateGitHubApiKeyRequest request,
                UnityAction<AsyncResult<Result.CreateGitHubApiKeyResult>> callback
        )
		{
			var task = new CreateGitHubApiKeyTask(
			    Gs2WebSocketSession,
			    request
            );
            yield return task;
            callback.Invoke(new AsyncResult<Result.CreateGitHubApiKeyResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.CreateGitHubApiKeyResult> CreateGitHubApiKey(
            Request.CreateGitHubApiKeyRequest request
        )
		{
		    var task = new CreateGitHubApiKeyTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
#endif


        private class UpdateGitHubApiKeyTask : Gs2WebSocketSessionTask<Request.UpdateGitHubApiKeyRequest, Result.UpdateGitHubApiKeyResult>
        {
	        public UpdateGitHubApiKeyTask(IGs2Session session, Request.UpdateGitHubApiKeyRequest request) : base(session, request)
	        {
	        }

            protected override IGs2SessionRequest CreateRequest(Request.UpdateGitHubApiKeyRequest request)
            {
                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);

                jsonWriter.WriteObjectStart();

                if (request.NamespaceName != null)
                {
                    jsonWriter.WritePropertyName("namespaceName");
                    jsonWriter.Write(request.NamespaceName.ToString());
                }
                if (request.ApiKeyName != null)
                {
                    jsonWriter.WritePropertyName("apiKeyName");
                    jsonWriter.Write(request.ApiKeyName.ToString());
                }
                if (request.Description != null)
                {
                    jsonWriter.WritePropertyName("description");
                    jsonWriter.Write(request.Description.ToString());
                }
                if (request.ApiKey != null)
                {
                    jsonWriter.WritePropertyName("apiKey");
                    jsonWriter.Write(request.ApiKey.ToString());
                }
                if (request.EncryptionKeyName != null)
                {
                    jsonWriter.WritePropertyName("encryptionKeyName");
                    jsonWriter.Write(request.EncryptionKeyName.ToString());
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
                    "key",
                    "gitHubApiKey",
                    "updateGitHubApiKey",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator UpdateGitHubApiKey(
                Request.UpdateGitHubApiKeyRequest request,
                UnityAction<AsyncResult<Result.UpdateGitHubApiKeyResult>> callback
        )
		{
			var task = new UpdateGitHubApiKeyTask(
			    Gs2WebSocketSession,
			    request
            );
            yield return task;
            callback.Invoke(new AsyncResult<Result.UpdateGitHubApiKeyResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.UpdateGitHubApiKeyResult> UpdateGitHubApiKey(
            Request.UpdateGitHubApiKeyRequest request
        )
		{
		    var task = new UpdateGitHubApiKeyTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
#endif


        private class GetGitHubApiKeyTask : Gs2WebSocketSessionTask<Request.GetGitHubApiKeyRequest, Result.GetGitHubApiKeyResult>
        {
	        public GetGitHubApiKeyTask(IGs2Session session, Request.GetGitHubApiKeyRequest request) : base(session, request)
	        {
	        }

            protected override IGs2SessionRequest CreateRequest(Request.GetGitHubApiKeyRequest request)
            {
                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);

                jsonWriter.WriteObjectStart();

                if (request.NamespaceName != null)
                {
                    jsonWriter.WritePropertyName("namespaceName");
                    jsonWriter.Write(request.NamespaceName.ToString());
                }
                if (request.ApiKeyName != null)
                {
                    jsonWriter.WritePropertyName("apiKeyName");
                    jsonWriter.Write(request.ApiKeyName.ToString());
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
                    "key",
                    "gitHubApiKey",
                    "getGitHubApiKey",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator GetGitHubApiKey(
                Request.GetGitHubApiKeyRequest request,
                UnityAction<AsyncResult<Result.GetGitHubApiKeyResult>> callback
        )
		{
			var task = new GetGitHubApiKeyTask(
			    Gs2WebSocketSession,
			    request
            );
            yield return task;
            callback.Invoke(new AsyncResult<Result.GetGitHubApiKeyResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.GetGitHubApiKeyResult> GetGitHubApiKey(
            Request.GetGitHubApiKeyRequest request
        )
		{
		    var task = new GetGitHubApiKeyTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
#endif


        private class DeleteGitHubApiKeyTask : Gs2WebSocketSessionTask<Request.DeleteGitHubApiKeyRequest, Result.DeleteGitHubApiKeyResult>
        {
	        public DeleteGitHubApiKeyTask(IGs2Session session, Request.DeleteGitHubApiKeyRequest request) : base(session, request)
	        {
	        }

            protected override IGs2SessionRequest CreateRequest(Request.DeleteGitHubApiKeyRequest request)
            {
                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);

                jsonWriter.WriteObjectStart();

                if (request.NamespaceName != null)
                {
                    jsonWriter.WritePropertyName("namespaceName");
                    jsonWriter.Write(request.NamespaceName.ToString());
                }
                if (request.ApiKeyName != null)
                {
                    jsonWriter.WritePropertyName("apiKeyName");
                    jsonWriter.Write(request.ApiKeyName.ToString());
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
                    "key",
                    "gitHubApiKey",
                    "deleteGitHubApiKey",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator DeleteGitHubApiKey(
                Request.DeleteGitHubApiKeyRequest request,
                UnityAction<AsyncResult<Result.DeleteGitHubApiKeyResult>> callback
        )
		{
			var task = new DeleteGitHubApiKeyTask(
			    Gs2WebSocketSession,
			    request
            );
            yield return task;
            callback.Invoke(new AsyncResult<Result.DeleteGitHubApiKeyResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.DeleteGitHubApiKeyResult> DeleteGitHubApiKey(
            Request.DeleteGitHubApiKeyRequest request
        )
		{
		    var task = new DeleteGitHubApiKeyTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
#endif
	}
}