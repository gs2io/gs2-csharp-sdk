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
#if UNITY_2017_1_OR_NEWER
using UnityEngine.Events;
using UnityEngine.Networking;
#else
using System.Web;
using System.Net.Http;
using System.Threading.Tasks;
#endif

using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Gs2.Core;
using Gs2.Core.Model;
using Gs2.Core.Net;
using Gs2.Gs2Inbox.Request;
using Gs2.Gs2Inbox.Result;
using Gs2.Util.LitJson;

namespace Gs2.Gs2Inbox
{
	public class Gs2InboxRestClient : AbstractGs2Client
	{
#if UNITY_2017_1_OR_NEWER
		private readonly CertificateHandler _certificateHandler;
#endif

		public static string Endpoint = "inbox";

        protected Gs2RestSession Gs2RestSession => (Gs2RestSession) Gs2Session;

		public Gs2InboxRestClient(Gs2RestSession Gs2RestSession) : base(Gs2RestSession)
		{

		}

#if UNITY_2017_1_OR_NEWER
		public Gs2InboxRestClient(Gs2RestSession gs2RestSession, CertificateHandler certificateHandler) : base(gs2RestSession)
		{
			_certificateHandler = certificateHandler;
		}
#endif


        private class DescribeNamespacesTask : Gs2RestSessionTask<DescribeNamespacesRequest, DescribeNamespacesResult>
        {
            public DescribeNamespacesTask(IGs2Session session, RestSessionRequestFactory factory, DescribeNamespacesRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DescribeNamespacesRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "inbox")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/";

                var sessionRequest = Factory.Get(url);
                if (request.ContextStack != null)
                {
                    sessionRequest.AddQueryString("contextStack", request.ContextStack);
                }
                if (request.PageToken != null) {
                    sessionRequest.AddQueryString("pageToken", $"{request.PageToken}");
                }
                if (request.Limit != null) {
                    sessionRequest.AddQueryString("limit", $"{request.Limit}");
                }

                if (request.RequestId != null)
                {
                    sessionRequest.AddHeader("X-GS2-REQUEST-ID", request.RequestId);
                }

                AddHeader(
                    Session.Credential,
                    sessionRequest
                );

                return sessionRequest;
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator DescribeNamespaces(
                Request.DescribeNamespacesRequest request,
                UnityAction<AsyncResult<Result.DescribeNamespacesResult>> callback
        )
		{
			var task = new DescribeNamespacesTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.DescribeNamespacesResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.DescribeNamespacesResult> DescribeNamespacesAsync(
                Request.DescribeNamespacesRequest request
        )
		{
			var task = new DescribeNamespacesTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        private class CreateNamespaceTask : Gs2RestSessionTask<CreateNamespaceRequest, CreateNamespaceResult>
        {
            public CreateNamespaceTask(IGs2Session session, RestSessionRequestFactory factory, CreateNamespaceRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(CreateNamespaceRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "inbox")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/";

                var sessionRequest = Factory.Post(url);

                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);
                jsonWriter.WriteObjectStart();
                if (request.Name != null)
                {
                    jsonWriter.WritePropertyName("name");
                    jsonWriter.Write(request.Name);
                }
                if (request.Description != null)
                {
                    jsonWriter.WritePropertyName("description");
                    jsonWriter.Write(request.Description);
                }
                if (request.IsAutomaticDeletingEnabled != null)
                {
                    jsonWriter.WritePropertyName("isAutomaticDeletingEnabled");
                    jsonWriter.Write(request.IsAutomaticDeletingEnabled.ToString());
                }
                if (request.ReceiveMessageScript != null)
                {
                    jsonWriter.WritePropertyName("receiveMessageScript");
                    request.ReceiveMessageScript.WriteJson(jsonWriter);
                }
                if (request.ReadMessageScript != null)
                {
                    jsonWriter.WritePropertyName("readMessageScript");
                    request.ReadMessageScript.WriteJson(jsonWriter);
                }
                if (request.DeleteMessageScript != null)
                {
                    jsonWriter.WritePropertyName("deleteMessageScript");
                    request.DeleteMessageScript.WriteJson(jsonWriter);
                }
                if (request.QueueNamespaceId != null)
                {
                    jsonWriter.WritePropertyName("queueNamespaceId");
                    jsonWriter.Write(request.QueueNamespaceId);
                }
                if (request.KeyId != null)
                {
                    jsonWriter.WritePropertyName("keyId");
                    jsonWriter.Write(request.KeyId);
                }
                if (request.ReceiveNotification != null)
                {
                    jsonWriter.WritePropertyName("receiveNotification");
                    request.ReceiveNotification.WriteJson(jsonWriter);
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
                jsonWriter.WriteObjectEnd();

                var body = stringBuilder.ToString();
                if (!string.IsNullOrEmpty(body))
                {
                    sessionRequest.Body = body;
                }
                sessionRequest.AddHeader("Content-Type", "application/json");

                if (request.RequestId != null)
                {
                    sessionRequest.AddHeader("X-GS2-REQUEST-ID", request.RequestId);
                }

                AddHeader(
                    Session.Credential,
                    sessionRequest
                );

                return sessionRequest;
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator CreateNamespace(
                Request.CreateNamespaceRequest request,
                UnityAction<AsyncResult<Result.CreateNamespaceResult>> callback
        )
		{
			var task = new CreateNamespaceTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.CreateNamespaceResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.CreateNamespaceResult> CreateNamespaceAsync(
                Request.CreateNamespaceRequest request
        )
		{
			var task = new CreateNamespaceTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        private class GetNamespaceStatusTask : Gs2RestSessionTask<GetNamespaceStatusRequest, GetNamespaceStatusResult>
        {
            public GetNamespaceStatusTask(IGs2Session session, RestSessionRequestFactory factory, GetNamespaceStatusRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(GetNamespaceStatusRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "inbox")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/status";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");

                var sessionRequest = Factory.Get(url);
                if (request.ContextStack != null)
                {
                    sessionRequest.AddQueryString("contextStack", request.ContextStack);
                }

                if (request.RequestId != null)
                {
                    sessionRequest.AddHeader("X-GS2-REQUEST-ID", request.RequestId);
                }

                AddHeader(
                    Session.Credential,
                    sessionRequest
                );

                return sessionRequest;
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator GetNamespaceStatus(
                Request.GetNamespaceStatusRequest request,
                UnityAction<AsyncResult<Result.GetNamespaceStatusResult>> callback
        )
		{
			var task = new GetNamespaceStatusTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.GetNamespaceStatusResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.GetNamespaceStatusResult> GetNamespaceStatusAsync(
                Request.GetNamespaceStatusRequest request
        )
		{
			var task = new GetNamespaceStatusTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        private class GetNamespaceTask : Gs2RestSessionTask<GetNamespaceRequest, GetNamespaceResult>
        {
            public GetNamespaceTask(IGs2Session session, RestSessionRequestFactory factory, GetNamespaceRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(GetNamespaceRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "inbox")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");

                var sessionRequest = Factory.Get(url);
                if (request.ContextStack != null)
                {
                    sessionRequest.AddQueryString("contextStack", request.ContextStack);
                }

                if (request.RequestId != null)
                {
                    sessionRequest.AddHeader("X-GS2-REQUEST-ID", request.RequestId);
                }

                AddHeader(
                    Session.Credential,
                    sessionRequest
                );

                return sessionRequest;
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator GetNamespace(
                Request.GetNamespaceRequest request,
                UnityAction<AsyncResult<Result.GetNamespaceResult>> callback
        )
		{
			var task = new GetNamespaceTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.GetNamespaceResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.GetNamespaceResult> GetNamespaceAsync(
                Request.GetNamespaceRequest request
        )
		{
			var task = new GetNamespaceTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        private class UpdateNamespaceTask : Gs2RestSessionTask<UpdateNamespaceRequest, UpdateNamespaceResult>
        {
            public UpdateNamespaceTask(IGs2Session session, RestSessionRequestFactory factory, UpdateNamespaceRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(UpdateNamespaceRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "inbox")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");

                var sessionRequest = Factory.Put(url);

                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);
                jsonWriter.WriteObjectStart();
                if (request.Description != null)
                {
                    jsonWriter.WritePropertyName("description");
                    jsonWriter.Write(request.Description);
                }
                if (request.IsAutomaticDeletingEnabled != null)
                {
                    jsonWriter.WritePropertyName("isAutomaticDeletingEnabled");
                    jsonWriter.Write(request.IsAutomaticDeletingEnabled.ToString());
                }
                if (request.ReceiveMessageScript != null)
                {
                    jsonWriter.WritePropertyName("receiveMessageScript");
                    request.ReceiveMessageScript.WriteJson(jsonWriter);
                }
                if (request.ReadMessageScript != null)
                {
                    jsonWriter.WritePropertyName("readMessageScript");
                    request.ReadMessageScript.WriteJson(jsonWriter);
                }
                if (request.DeleteMessageScript != null)
                {
                    jsonWriter.WritePropertyName("deleteMessageScript");
                    request.DeleteMessageScript.WriteJson(jsonWriter);
                }
                if (request.QueueNamespaceId != null)
                {
                    jsonWriter.WritePropertyName("queueNamespaceId");
                    jsonWriter.Write(request.QueueNamespaceId);
                }
                if (request.KeyId != null)
                {
                    jsonWriter.WritePropertyName("keyId");
                    jsonWriter.Write(request.KeyId);
                }
                if (request.ReceiveNotification != null)
                {
                    jsonWriter.WritePropertyName("receiveNotification");
                    request.ReceiveNotification.WriteJson(jsonWriter);
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
                jsonWriter.WriteObjectEnd();

                var body = stringBuilder.ToString();
                if (!string.IsNullOrEmpty(body))
                {
                    sessionRequest.Body = body;
                }
                sessionRequest.AddHeader("Content-Type", "application/json");

                if (request.RequestId != null)
                {
                    sessionRequest.AddHeader("X-GS2-REQUEST-ID", request.RequestId);
                }

                AddHeader(
                    Session.Credential,
                    sessionRequest
                );

                return sessionRequest;
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator UpdateNamespace(
                Request.UpdateNamespaceRequest request,
                UnityAction<AsyncResult<Result.UpdateNamespaceResult>> callback
        )
		{
			var task = new UpdateNamespaceTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.UpdateNamespaceResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.UpdateNamespaceResult> UpdateNamespaceAsync(
                Request.UpdateNamespaceRequest request
        )
		{
			var task = new UpdateNamespaceTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        private class DeleteNamespaceTask : Gs2RestSessionTask<DeleteNamespaceRequest, DeleteNamespaceResult>
        {
            public DeleteNamespaceTask(IGs2Session session, RestSessionRequestFactory factory, DeleteNamespaceRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DeleteNamespaceRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "inbox")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");

                var sessionRequest = Factory.Delete(url);
                if (request.ContextStack != null)
                {
                    sessionRequest.AddQueryString("contextStack", request.ContextStack);
                }

                if (request.RequestId != null)
                {
                    sessionRequest.AddHeader("X-GS2-REQUEST-ID", request.RequestId);
                }

                AddHeader(
                    Session.Credential,
                    sessionRequest
                );

                return sessionRequest;
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator DeleteNamespace(
                Request.DeleteNamespaceRequest request,
                UnityAction<AsyncResult<Result.DeleteNamespaceResult>> callback
        )
		{
			var task = new DeleteNamespaceTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.DeleteNamespaceResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.DeleteNamespaceResult> DeleteNamespaceAsync(
                Request.DeleteNamespaceRequest request
        )
		{
			var task = new DeleteNamespaceTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        private class DescribeMessagesTask : Gs2RestSessionTask<DescribeMessagesRequest, DescribeMessagesResult>
        {
            public DescribeMessagesTask(IGs2Session session, RestSessionRequestFactory factory, DescribeMessagesRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DescribeMessagesRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "inbox")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/me/message";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");

                var sessionRequest = Factory.Get(url);
                if (request.ContextStack != null)
                {
                    sessionRequest.AddQueryString("contextStack", request.ContextStack);
                }
                if (request.PageToken != null) {
                    sessionRequest.AddQueryString("pageToken", $"{request.PageToken}");
                }
                if (request.Limit != null) {
                    sessionRequest.AddQueryString("limit", $"{request.Limit}");
                }

                if (request.RequestId != null)
                {
                    sessionRequest.AddHeader("X-GS2-REQUEST-ID", request.RequestId);
                }
                if (request.AccessToken != null)
                {
                    sessionRequest.AddHeader("X-GS2-ACCESS-TOKEN", request.AccessToken);
                }

                AddHeader(
                    Session.Credential,
                    sessionRequest
                );

                return sessionRequest;
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator DescribeMessages(
                Request.DescribeMessagesRequest request,
                UnityAction<AsyncResult<Result.DescribeMessagesResult>> callback
        )
		{
			var task = new DescribeMessagesTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.DescribeMessagesResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.DescribeMessagesResult> DescribeMessagesAsync(
                Request.DescribeMessagesRequest request
        )
		{
			var task = new DescribeMessagesTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        private class DescribeMessagesByUserIdTask : Gs2RestSessionTask<DescribeMessagesByUserIdRequest, DescribeMessagesByUserIdResult>
        {
            public DescribeMessagesByUserIdTask(IGs2Session session, RestSessionRequestFactory factory, DescribeMessagesByUserIdRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DescribeMessagesByUserIdRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "inbox")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/message";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");

                var sessionRequest = Factory.Get(url);
                if (request.ContextStack != null)
                {
                    sessionRequest.AddQueryString("contextStack", request.ContextStack);
                }
                if (request.UserId != null) {
                    sessionRequest.AddQueryString("userId", $"{request.UserId}");
                }
                if (request.PageToken != null) {
                    sessionRequest.AddQueryString("pageToken", $"{request.PageToken}");
                }
                if (request.Limit != null) {
                    sessionRequest.AddQueryString("limit", $"{request.Limit}");
                }

                if (request.RequestId != null)
                {
                    sessionRequest.AddHeader("X-GS2-REQUEST-ID", request.RequestId);
                }

                AddHeader(
                    Session.Credential,
                    sessionRequest
                );

                return sessionRequest;
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator DescribeMessagesByUserId(
                Request.DescribeMessagesByUserIdRequest request,
                UnityAction<AsyncResult<Result.DescribeMessagesByUserIdResult>> callback
        )
		{
			var task = new DescribeMessagesByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.DescribeMessagesByUserIdResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.DescribeMessagesByUserIdResult> DescribeMessagesByUserIdAsync(
                Request.DescribeMessagesByUserIdRequest request
        )
		{
			var task = new DescribeMessagesByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        private class SendMessageByUserIdTask : Gs2RestSessionTask<SendMessageByUserIdRequest, SendMessageByUserIdResult>
        {
            public SendMessageByUserIdTask(IGs2Session session, RestSessionRequestFactory factory, SendMessageByUserIdRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(SendMessageByUserIdRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "inbox")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/{userId}/message";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{userId}", !string.IsNullOrEmpty(request.UserId) ? request.UserId.ToString() : "null");

                var sessionRequest = Factory.Post(url);

                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);
                jsonWriter.WriteObjectStart();
                if (request.Metadata != null)
                {
                    jsonWriter.WritePropertyName("metadata");
                    jsonWriter.Write(request.Metadata);
                }
                if (request.ReadAcquireActions != null)
                {
                    jsonWriter.WritePropertyName("readAcquireActions");
                    jsonWriter.WriteArrayStart();
                    foreach(var item in request.ReadAcquireActions)
                    {
                        item.WriteJson(jsonWriter);
                    }
                    jsonWriter.WriteArrayEnd();
                }
                if (request.ExpiresAt != null)
                {
                    jsonWriter.WritePropertyName("expiresAt");
                    jsonWriter.Write(request.ExpiresAt.ToString());
                }
                if (request.ExpiresTimeSpan != null)
                {
                    jsonWriter.WritePropertyName("expiresTimeSpan");
                    request.ExpiresTimeSpan.WriteJson(jsonWriter);
                }
                if (request.ContextStack != null)
                {
                    jsonWriter.WritePropertyName("contextStack");
                    jsonWriter.Write(request.ContextStack.ToString());
                }
                jsonWriter.WriteObjectEnd();

                var body = stringBuilder.ToString();
                if (!string.IsNullOrEmpty(body))
                {
                    sessionRequest.Body = body;
                }
                sessionRequest.AddHeader("Content-Type", "application/json");

                if (request.RequestId != null)
                {
                    sessionRequest.AddHeader("X-GS2-REQUEST-ID", request.RequestId);
                }

                AddHeader(
                    Session.Credential,
                    sessionRequest
                );

                return sessionRequest;
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator SendMessageByUserId(
                Request.SendMessageByUserIdRequest request,
                UnityAction<AsyncResult<Result.SendMessageByUserIdResult>> callback
        )
		{
			var task = new SendMessageByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.SendMessageByUserIdResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.SendMessageByUserIdResult> SendMessageByUserIdAsync(
                Request.SendMessageByUserIdRequest request
        )
		{
			var task = new SendMessageByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        private class GetMessageTask : Gs2RestSessionTask<GetMessageRequest, GetMessageResult>
        {
            public GetMessageTask(IGs2Session session, RestSessionRequestFactory factory, GetMessageRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(GetMessageRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "inbox")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/me/{messageName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{messageName}", !string.IsNullOrEmpty(request.MessageName) ? request.MessageName.ToString() : "null");

                var sessionRequest = Factory.Get(url);
                if (request.ContextStack != null)
                {
                    sessionRequest.AddQueryString("contextStack", request.ContextStack);
                }

                if (request.RequestId != null)
                {
                    sessionRequest.AddHeader("X-GS2-REQUEST-ID", request.RequestId);
                }
                if (request.AccessToken != null)
                {
                    sessionRequest.AddHeader("X-GS2-ACCESS-TOKEN", request.AccessToken);
                }

                AddHeader(
                    Session.Credential,
                    sessionRequest
                );

                return sessionRequest;
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator GetMessage(
                Request.GetMessageRequest request,
                UnityAction<AsyncResult<Result.GetMessageResult>> callback
        )
		{
			var task = new GetMessageTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.GetMessageResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.GetMessageResult> GetMessageAsync(
                Request.GetMessageRequest request
        )
		{
			var task = new GetMessageTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        private class GetMessageByUserIdTask : Gs2RestSessionTask<GetMessageByUserIdRequest, GetMessageByUserIdResult>
        {
            public GetMessageByUserIdTask(IGs2Session session, RestSessionRequestFactory factory, GetMessageByUserIdRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(GetMessageByUserIdRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "inbox")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/{userId}/{messageName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{userId}", !string.IsNullOrEmpty(request.UserId) ? request.UserId.ToString() : "null");
                url = url.Replace("{messageName}", !string.IsNullOrEmpty(request.MessageName) ? request.MessageName.ToString() : "null");

                var sessionRequest = Factory.Get(url);
                if (request.ContextStack != null)
                {
                    sessionRequest.AddQueryString("contextStack", request.ContextStack);
                }

                if (request.RequestId != null)
                {
                    sessionRequest.AddHeader("X-GS2-REQUEST-ID", request.RequestId);
                }

                AddHeader(
                    Session.Credential,
                    sessionRequest
                );

                return sessionRequest;
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator GetMessageByUserId(
                Request.GetMessageByUserIdRequest request,
                UnityAction<AsyncResult<Result.GetMessageByUserIdResult>> callback
        )
		{
			var task = new GetMessageByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.GetMessageByUserIdResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.GetMessageByUserIdResult> GetMessageByUserIdAsync(
                Request.GetMessageByUserIdRequest request
        )
		{
			var task = new GetMessageByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        private class ReceiveGlobalMessageTask : Gs2RestSessionTask<ReceiveGlobalMessageRequest, ReceiveGlobalMessageResult>
        {
            public ReceiveGlobalMessageTask(IGs2Session session, RestSessionRequestFactory factory, ReceiveGlobalMessageRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(ReceiveGlobalMessageRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "inbox")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/me/message/globalMessage/receive";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");

                var sessionRequest = Factory.Post(url);

                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);
                jsonWriter.WriteObjectStart();
                if (request.ContextStack != null)
                {
                    jsonWriter.WritePropertyName("contextStack");
                    jsonWriter.Write(request.ContextStack.ToString());
                }
                jsonWriter.WriteObjectEnd();

                var body = stringBuilder.ToString();
                if (!string.IsNullOrEmpty(body))
                {
                    sessionRequest.Body = body;
                }
                sessionRequest.AddHeader("Content-Type", "application/json");

                if (request.RequestId != null)
                {
                    sessionRequest.AddHeader("X-GS2-REQUEST-ID", request.RequestId);
                }
                if (request.AccessToken != null)
                {
                    sessionRequest.AddHeader("X-GS2-ACCESS-TOKEN", request.AccessToken);
                }

                AddHeader(
                    Session.Credential,
                    sessionRequest
                );

                return sessionRequest;
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator ReceiveGlobalMessage(
                Request.ReceiveGlobalMessageRequest request,
                UnityAction<AsyncResult<Result.ReceiveGlobalMessageResult>> callback
        )
		{
			var task = new ReceiveGlobalMessageTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.ReceiveGlobalMessageResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.ReceiveGlobalMessageResult> ReceiveGlobalMessageAsync(
                Request.ReceiveGlobalMessageRequest request
        )
		{
			var task = new ReceiveGlobalMessageTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        private class ReceiveGlobalMessageByUserIdTask : Gs2RestSessionTask<ReceiveGlobalMessageByUserIdRequest, ReceiveGlobalMessageByUserIdResult>
        {
            public ReceiveGlobalMessageByUserIdTask(IGs2Session session, RestSessionRequestFactory factory, ReceiveGlobalMessageByUserIdRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(ReceiveGlobalMessageByUserIdRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "inbox")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/{userId}/message/globalMessage/receive";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{userId}", !string.IsNullOrEmpty(request.UserId) ? request.UserId.ToString() : "null");

                var sessionRequest = Factory.Post(url);

                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);
                jsonWriter.WriteObjectStart();
                if (request.ContextStack != null)
                {
                    jsonWriter.WritePropertyName("contextStack");
                    jsonWriter.Write(request.ContextStack.ToString());
                }
                jsonWriter.WriteObjectEnd();

                var body = stringBuilder.ToString();
                if (!string.IsNullOrEmpty(body))
                {
                    sessionRequest.Body = body;
                }
                sessionRequest.AddHeader("Content-Type", "application/json");

                if (request.RequestId != null)
                {
                    sessionRequest.AddHeader("X-GS2-REQUEST-ID", request.RequestId);
                }

                AddHeader(
                    Session.Credential,
                    sessionRequest
                );

                return sessionRequest;
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator ReceiveGlobalMessageByUserId(
                Request.ReceiveGlobalMessageByUserIdRequest request,
                UnityAction<AsyncResult<Result.ReceiveGlobalMessageByUserIdResult>> callback
        )
		{
			var task = new ReceiveGlobalMessageByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.ReceiveGlobalMessageByUserIdResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.ReceiveGlobalMessageByUserIdResult> ReceiveGlobalMessageByUserIdAsync(
                Request.ReceiveGlobalMessageByUserIdRequest request
        )
		{
			var task = new ReceiveGlobalMessageByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        private class OpenMessageTask : Gs2RestSessionTask<OpenMessageRequest, OpenMessageResult>
        {
            public OpenMessageTask(IGs2Session session, RestSessionRequestFactory factory, OpenMessageRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(OpenMessageRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "inbox")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/me/{messageName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{messageName}", !string.IsNullOrEmpty(request.MessageName) ? request.MessageName.ToString() : "null");

                var sessionRequest = Factory.Post(url);

                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);
                jsonWriter.WriteObjectStart();
                if (request.ContextStack != null)
                {
                    jsonWriter.WritePropertyName("contextStack");
                    jsonWriter.Write(request.ContextStack.ToString());
                }
                jsonWriter.WriteObjectEnd();

                var body = stringBuilder.ToString();
                if (!string.IsNullOrEmpty(body))
                {
                    sessionRequest.Body = body;
                }
                sessionRequest.AddHeader("Content-Type", "application/json");

                if (request.RequestId != null)
                {
                    sessionRequest.AddHeader("X-GS2-REQUEST-ID", request.RequestId);
                }
                if (request.AccessToken != null)
                {
                    sessionRequest.AddHeader("X-GS2-ACCESS-TOKEN", request.AccessToken);
                }

                AddHeader(
                    Session.Credential,
                    sessionRequest
                );

                return sessionRequest;
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator OpenMessage(
                Request.OpenMessageRequest request,
                UnityAction<AsyncResult<Result.OpenMessageResult>> callback
        )
		{
			var task = new OpenMessageTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.OpenMessageResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.OpenMessageResult> OpenMessageAsync(
                Request.OpenMessageRequest request
        )
		{
			var task = new OpenMessageTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        private class OpenMessageByUserIdTask : Gs2RestSessionTask<OpenMessageByUserIdRequest, OpenMessageByUserIdResult>
        {
            public OpenMessageByUserIdTask(IGs2Session session, RestSessionRequestFactory factory, OpenMessageByUserIdRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(OpenMessageByUserIdRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "inbox")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/{userId}/{messageName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{userId}", !string.IsNullOrEmpty(request.UserId) ? request.UserId.ToString() : "null");
                url = url.Replace("{messageName}", !string.IsNullOrEmpty(request.MessageName) ? request.MessageName.ToString() : "null");

                var sessionRequest = Factory.Post(url);

                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);
                jsonWriter.WriteObjectStart();
                if (request.ContextStack != null)
                {
                    jsonWriter.WritePropertyName("contextStack");
                    jsonWriter.Write(request.ContextStack.ToString());
                }
                jsonWriter.WriteObjectEnd();

                var body = stringBuilder.ToString();
                if (!string.IsNullOrEmpty(body))
                {
                    sessionRequest.Body = body;
                }
                sessionRequest.AddHeader("Content-Type", "application/json");

                if (request.RequestId != null)
                {
                    sessionRequest.AddHeader("X-GS2-REQUEST-ID", request.RequestId);
                }

                AddHeader(
                    Session.Credential,
                    sessionRequest
                );

                return sessionRequest;
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator OpenMessageByUserId(
                Request.OpenMessageByUserIdRequest request,
                UnityAction<AsyncResult<Result.OpenMessageByUserIdResult>> callback
        )
		{
			var task = new OpenMessageByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.OpenMessageByUserIdResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.OpenMessageByUserIdResult> OpenMessageByUserIdAsync(
                Request.OpenMessageByUserIdRequest request
        )
		{
			var task = new OpenMessageByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        private class ReadMessageTask : Gs2RestSessionTask<ReadMessageRequest, ReadMessageResult>
        {
            public ReadMessageTask(IGs2Session session, RestSessionRequestFactory factory, ReadMessageRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(ReadMessageRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "inbox")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/me/{messageName}/read";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{messageName}", !string.IsNullOrEmpty(request.MessageName) ? request.MessageName.ToString() : "null");

                var sessionRequest = Factory.Post(url);

                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);
                jsonWriter.WriteObjectStart();
                if (request.Config != null)
                {
                    jsonWriter.WritePropertyName("config");
                    jsonWriter.WriteArrayStart();
                    foreach(var item in request.Config)
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
                jsonWriter.WriteObjectEnd();

                var body = stringBuilder.ToString();
                if (!string.IsNullOrEmpty(body))
                {
                    sessionRequest.Body = body;
                }
                sessionRequest.AddHeader("Content-Type", "application/json");

                if (request.RequestId != null)
                {
                    sessionRequest.AddHeader("X-GS2-REQUEST-ID", request.RequestId);
                }
                if (request.AccessToken != null)
                {
                    sessionRequest.AddHeader("X-GS2-ACCESS-TOKEN", request.AccessToken);
                }

                AddHeader(
                    Session.Credential,
                    sessionRequest
                );

                return sessionRequest;
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator ReadMessage(
                Request.ReadMessageRequest request,
                UnityAction<AsyncResult<Result.ReadMessageResult>> callback
        )
		{
			var task = new ReadMessageTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.ReadMessageResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.ReadMessageResult> ReadMessageAsync(
                Request.ReadMessageRequest request
        )
		{
			var task = new ReadMessageTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        private class ReadMessageByUserIdTask : Gs2RestSessionTask<ReadMessageByUserIdRequest, ReadMessageByUserIdResult>
        {
            public ReadMessageByUserIdTask(IGs2Session session, RestSessionRequestFactory factory, ReadMessageByUserIdRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(ReadMessageByUserIdRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "inbox")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/{userId}/{messageName}/read";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{userId}", !string.IsNullOrEmpty(request.UserId) ? request.UserId.ToString() : "null");
                url = url.Replace("{messageName}", !string.IsNullOrEmpty(request.MessageName) ? request.MessageName.ToString() : "null");

                var sessionRequest = Factory.Post(url);

                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);
                jsonWriter.WriteObjectStart();
                if (request.Config != null)
                {
                    jsonWriter.WritePropertyName("config");
                    jsonWriter.WriteArrayStart();
                    foreach(var item in request.Config)
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
                jsonWriter.WriteObjectEnd();

                var body = stringBuilder.ToString();
                if (!string.IsNullOrEmpty(body))
                {
                    sessionRequest.Body = body;
                }
                sessionRequest.AddHeader("Content-Type", "application/json");

                if (request.RequestId != null)
                {
                    sessionRequest.AddHeader("X-GS2-REQUEST-ID", request.RequestId);
                }

                AddHeader(
                    Session.Credential,
                    sessionRequest
                );

                return sessionRequest;
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator ReadMessageByUserId(
                Request.ReadMessageByUserIdRequest request,
                UnityAction<AsyncResult<Result.ReadMessageByUserIdResult>> callback
        )
		{
			var task = new ReadMessageByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.ReadMessageByUserIdResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.ReadMessageByUserIdResult> ReadMessageByUserIdAsync(
                Request.ReadMessageByUserIdRequest request
        )
		{
			var task = new ReadMessageByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        private class DeleteMessageTask : Gs2RestSessionTask<DeleteMessageRequest, DeleteMessageResult>
        {
            public DeleteMessageTask(IGs2Session session, RestSessionRequestFactory factory, DeleteMessageRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DeleteMessageRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "inbox")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/me/{messageName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{messageName}", !string.IsNullOrEmpty(request.MessageName) ? request.MessageName.ToString() : "null");

                var sessionRequest = Factory.Delete(url);
                if (request.ContextStack != null)
                {
                    sessionRequest.AddQueryString("contextStack", request.ContextStack);
                }

                if (request.RequestId != null)
                {
                    sessionRequest.AddHeader("X-GS2-REQUEST-ID", request.RequestId);
                }
                if (request.AccessToken != null)
                {
                    sessionRequest.AddHeader("X-GS2-ACCESS-TOKEN", request.AccessToken);
                }

                AddHeader(
                    Session.Credential,
                    sessionRequest
                );

                return sessionRequest;
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator DeleteMessage(
                Request.DeleteMessageRequest request,
                UnityAction<AsyncResult<Result.DeleteMessageResult>> callback
        )
		{
			var task = new DeleteMessageTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.DeleteMessageResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.DeleteMessageResult> DeleteMessageAsync(
                Request.DeleteMessageRequest request
        )
		{
			var task = new DeleteMessageTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        private class DeleteMessageByUserIdTask : Gs2RestSessionTask<DeleteMessageByUserIdRequest, DeleteMessageByUserIdResult>
        {
            public DeleteMessageByUserIdTask(IGs2Session session, RestSessionRequestFactory factory, DeleteMessageByUserIdRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DeleteMessageByUserIdRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "inbox")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/{userId}/{messageName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{userId}", !string.IsNullOrEmpty(request.UserId) ? request.UserId.ToString() : "null");
                url = url.Replace("{messageName}", !string.IsNullOrEmpty(request.MessageName) ? request.MessageName.ToString() : "null");

                var sessionRequest = Factory.Delete(url);
                if (request.ContextStack != null)
                {
                    sessionRequest.AddQueryString("contextStack", request.ContextStack);
                }

                if (request.RequestId != null)
                {
                    sessionRequest.AddHeader("X-GS2-REQUEST-ID", request.RequestId);
                }

                AddHeader(
                    Session.Credential,
                    sessionRequest
                );

                return sessionRequest;
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator DeleteMessageByUserId(
                Request.DeleteMessageByUserIdRequest request,
                UnityAction<AsyncResult<Result.DeleteMessageByUserIdResult>> callback
        )
		{
			var task = new DeleteMessageByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.DeleteMessageByUserIdResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.DeleteMessageByUserIdResult> DeleteMessageByUserIdAsync(
                Request.DeleteMessageByUserIdRequest request
        )
		{
			var task = new DeleteMessageByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        private class SendByStampSheetTask : Gs2RestSessionTask<SendByStampSheetRequest, SendByStampSheetResult>
        {
            public SendByStampSheetTask(IGs2Session session, RestSessionRequestFactory factory, SendByStampSheetRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(SendByStampSheetRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "inbox")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/stamp/send";

                var sessionRequest = Factory.Post(url);

                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);
                jsonWriter.WriteObjectStart();
                if (request.StampSheet != null)
                {
                    jsonWriter.WritePropertyName("stampSheet");
                    jsonWriter.Write(request.StampSheet);
                }
                if (request.KeyId != null)
                {
                    jsonWriter.WritePropertyName("keyId");
                    jsonWriter.Write(request.KeyId);
                }
                if (request.ContextStack != null)
                {
                    jsonWriter.WritePropertyName("contextStack");
                    jsonWriter.Write(request.ContextStack.ToString());
                }
                jsonWriter.WriteObjectEnd();

                var body = stringBuilder.ToString();
                if (!string.IsNullOrEmpty(body))
                {
                    sessionRequest.Body = body;
                }
                sessionRequest.AddHeader("Content-Type", "application/json");

                if (request.RequestId != null)
                {
                    sessionRequest.AddHeader("X-GS2-REQUEST-ID", request.RequestId);
                }

                AddHeader(
                    Session.Credential,
                    sessionRequest
                );

                return sessionRequest;
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator SendByStampSheet(
                Request.SendByStampSheetRequest request,
                UnityAction<AsyncResult<Result.SendByStampSheetResult>> callback
        )
		{
			var task = new SendByStampSheetTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.SendByStampSheetResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.SendByStampSheetResult> SendByStampSheetAsync(
                Request.SendByStampSheetRequest request
        )
		{
			var task = new SendByStampSheetTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        private class OpenByStampTaskTask : Gs2RestSessionTask<OpenByStampTaskRequest, OpenByStampTaskResult>
        {
            public OpenByStampTaskTask(IGs2Session session, RestSessionRequestFactory factory, OpenByStampTaskRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(OpenByStampTaskRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "inbox")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/stamp/open";

                var sessionRequest = Factory.Post(url);

                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);
                jsonWriter.WriteObjectStart();
                if (request.StampTask != null)
                {
                    jsonWriter.WritePropertyName("stampTask");
                    jsonWriter.Write(request.StampTask);
                }
                if (request.KeyId != null)
                {
                    jsonWriter.WritePropertyName("keyId");
                    jsonWriter.Write(request.KeyId);
                }
                if (request.ContextStack != null)
                {
                    jsonWriter.WritePropertyName("contextStack");
                    jsonWriter.Write(request.ContextStack.ToString());
                }
                jsonWriter.WriteObjectEnd();

                var body = stringBuilder.ToString();
                if (!string.IsNullOrEmpty(body))
                {
                    sessionRequest.Body = body;
                }
                sessionRequest.AddHeader("Content-Type", "application/json");

                if (request.RequestId != null)
                {
                    sessionRequest.AddHeader("X-GS2-REQUEST-ID", request.RequestId);
                }

                AddHeader(
                    Session.Credential,
                    sessionRequest
                );

                return sessionRequest;
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator OpenByStampTask(
                Request.OpenByStampTaskRequest request,
                UnityAction<AsyncResult<Result.OpenByStampTaskResult>> callback
        )
		{
			var task = new OpenByStampTaskTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.OpenByStampTaskResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.OpenByStampTaskResult> OpenByStampTaskAsync(
                Request.OpenByStampTaskRequest request
        )
		{
			var task = new OpenByStampTaskTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        private class ExportMasterTask : Gs2RestSessionTask<ExportMasterRequest, ExportMasterResult>
        {
            public ExportMasterTask(IGs2Session session, RestSessionRequestFactory factory, ExportMasterRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(ExportMasterRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "inbox")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/master/export";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");

                var sessionRequest = Factory.Get(url);
                if (request.ContextStack != null)
                {
                    sessionRequest.AddQueryString("contextStack", request.ContextStack);
                }

                if (request.RequestId != null)
                {
                    sessionRequest.AddHeader("X-GS2-REQUEST-ID", request.RequestId);
                }

                AddHeader(
                    Session.Credential,
                    sessionRequest
                );

                return sessionRequest;
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator ExportMaster(
                Request.ExportMasterRequest request,
                UnityAction<AsyncResult<Result.ExportMasterResult>> callback
        )
		{
			var task = new ExportMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.ExportMasterResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.ExportMasterResult> ExportMasterAsync(
                Request.ExportMasterRequest request
        )
		{
			var task = new ExportMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        private class GetCurrentMessageMasterTask : Gs2RestSessionTask<GetCurrentMessageMasterRequest, GetCurrentMessageMasterResult>
        {
            public GetCurrentMessageMasterTask(IGs2Session session, RestSessionRequestFactory factory, GetCurrentMessageMasterRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(GetCurrentMessageMasterRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "inbox")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/master";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");

                var sessionRequest = Factory.Get(url);
                if (request.ContextStack != null)
                {
                    sessionRequest.AddQueryString("contextStack", request.ContextStack);
                }

                if (request.RequestId != null)
                {
                    sessionRequest.AddHeader("X-GS2-REQUEST-ID", request.RequestId);
                }

                AddHeader(
                    Session.Credential,
                    sessionRequest
                );

                return sessionRequest;
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator GetCurrentMessageMaster(
                Request.GetCurrentMessageMasterRequest request,
                UnityAction<AsyncResult<Result.GetCurrentMessageMasterResult>> callback
        )
		{
			var task = new GetCurrentMessageMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.GetCurrentMessageMasterResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.GetCurrentMessageMasterResult> GetCurrentMessageMasterAsync(
                Request.GetCurrentMessageMasterRequest request
        )
		{
			var task = new GetCurrentMessageMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        private class UpdateCurrentMessageMasterTask : Gs2RestSessionTask<UpdateCurrentMessageMasterRequest, UpdateCurrentMessageMasterResult>
        {
            public UpdateCurrentMessageMasterTask(IGs2Session session, RestSessionRequestFactory factory, UpdateCurrentMessageMasterRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(UpdateCurrentMessageMasterRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "inbox")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/master";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");

                var sessionRequest = Factory.Put(url);

                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);
                jsonWriter.WriteObjectStart();
                if (request.Settings != null)
                {
                    jsonWriter.WritePropertyName("settings");
                    jsonWriter.Write(request.Settings);
                }
                if (request.ContextStack != null)
                {
                    jsonWriter.WritePropertyName("contextStack");
                    jsonWriter.Write(request.ContextStack.ToString());
                }
                jsonWriter.WriteObjectEnd();

                var body = stringBuilder.ToString();
                if (!string.IsNullOrEmpty(body))
                {
                    sessionRequest.Body = body;
                }
                sessionRequest.AddHeader("Content-Type", "application/json");

                if (request.RequestId != null)
                {
                    sessionRequest.AddHeader("X-GS2-REQUEST-ID", request.RequestId);
                }

                AddHeader(
                    Session.Credential,
                    sessionRequest
                );

                return sessionRequest;
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator UpdateCurrentMessageMaster(
                Request.UpdateCurrentMessageMasterRequest request,
                UnityAction<AsyncResult<Result.UpdateCurrentMessageMasterResult>> callback
        )
		{
			var task = new UpdateCurrentMessageMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.UpdateCurrentMessageMasterResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.UpdateCurrentMessageMasterResult> UpdateCurrentMessageMasterAsync(
                Request.UpdateCurrentMessageMasterRequest request
        )
		{
			var task = new UpdateCurrentMessageMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        private class UpdateCurrentMessageMasterFromGitHubTask : Gs2RestSessionTask<UpdateCurrentMessageMasterFromGitHubRequest, UpdateCurrentMessageMasterFromGitHubResult>
        {
            public UpdateCurrentMessageMasterFromGitHubTask(IGs2Session session, RestSessionRequestFactory factory, UpdateCurrentMessageMasterFromGitHubRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(UpdateCurrentMessageMasterFromGitHubRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "inbox")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/master/from_git_hub";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");

                var sessionRequest = Factory.Put(url);

                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);
                jsonWriter.WriteObjectStart();
                if (request.CheckoutSetting != null)
                {
                    jsonWriter.WritePropertyName("checkoutSetting");
                    request.CheckoutSetting.WriteJson(jsonWriter);
                }
                if (request.ContextStack != null)
                {
                    jsonWriter.WritePropertyName("contextStack");
                    jsonWriter.Write(request.ContextStack.ToString());
                }
                jsonWriter.WriteObjectEnd();

                var body = stringBuilder.ToString();
                if (!string.IsNullOrEmpty(body))
                {
                    sessionRequest.Body = body;
                }
                sessionRequest.AddHeader("Content-Type", "application/json");

                if (request.RequestId != null)
                {
                    sessionRequest.AddHeader("X-GS2-REQUEST-ID", request.RequestId);
                }

                AddHeader(
                    Session.Credential,
                    sessionRequest
                );

                return sessionRequest;
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator UpdateCurrentMessageMasterFromGitHub(
                Request.UpdateCurrentMessageMasterFromGitHubRequest request,
                UnityAction<AsyncResult<Result.UpdateCurrentMessageMasterFromGitHubResult>> callback
        )
		{
			var task = new UpdateCurrentMessageMasterFromGitHubTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.UpdateCurrentMessageMasterFromGitHubResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.UpdateCurrentMessageMasterFromGitHubResult> UpdateCurrentMessageMasterFromGitHubAsync(
                Request.UpdateCurrentMessageMasterFromGitHubRequest request
        )
		{
			var task = new UpdateCurrentMessageMasterFromGitHubTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        private class DescribeGlobalMessageMastersTask : Gs2RestSessionTask<DescribeGlobalMessageMastersRequest, DescribeGlobalMessageMastersResult>
        {
            public DescribeGlobalMessageMastersTask(IGs2Session session, RestSessionRequestFactory factory, DescribeGlobalMessageMastersRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DescribeGlobalMessageMastersRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "inbox")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/master/globalMessage";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");

                var sessionRequest = Factory.Get(url);
                if (request.ContextStack != null)
                {
                    sessionRequest.AddQueryString("contextStack", request.ContextStack);
                }
                if (request.PageToken != null) {
                    sessionRequest.AddQueryString("pageToken", $"{request.PageToken}");
                }
                if (request.Limit != null) {
                    sessionRequest.AddQueryString("limit", $"{request.Limit}");
                }

                if (request.RequestId != null)
                {
                    sessionRequest.AddHeader("X-GS2-REQUEST-ID", request.RequestId);
                }

                AddHeader(
                    Session.Credential,
                    sessionRequest
                );

                return sessionRequest;
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator DescribeGlobalMessageMasters(
                Request.DescribeGlobalMessageMastersRequest request,
                UnityAction<AsyncResult<Result.DescribeGlobalMessageMastersResult>> callback
        )
		{
			var task = new DescribeGlobalMessageMastersTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.DescribeGlobalMessageMastersResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.DescribeGlobalMessageMastersResult> DescribeGlobalMessageMastersAsync(
                Request.DescribeGlobalMessageMastersRequest request
        )
		{
			var task = new DescribeGlobalMessageMastersTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        private class CreateGlobalMessageMasterTask : Gs2RestSessionTask<CreateGlobalMessageMasterRequest, CreateGlobalMessageMasterResult>
        {
            public CreateGlobalMessageMasterTask(IGs2Session session, RestSessionRequestFactory factory, CreateGlobalMessageMasterRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(CreateGlobalMessageMasterRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "inbox")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/master/globalMessage";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");

                var sessionRequest = Factory.Post(url);

                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);
                jsonWriter.WriteObjectStart();
                if (request.Name != null)
                {
                    jsonWriter.WritePropertyName("name");
                    jsonWriter.Write(request.Name);
                }
                if (request.Metadata != null)
                {
                    jsonWriter.WritePropertyName("metadata");
                    jsonWriter.Write(request.Metadata);
                }
                if (request.ReadAcquireActions != null)
                {
                    jsonWriter.WritePropertyName("readAcquireActions");
                    jsonWriter.WriteArrayStart();
                    foreach(var item in request.ReadAcquireActions)
                    {
                        item.WriteJson(jsonWriter);
                    }
                    jsonWriter.WriteArrayEnd();
                }
                if (request.ExpiresTimeSpan != null)
                {
                    jsonWriter.WritePropertyName("expiresTimeSpan");
                    request.ExpiresTimeSpan.WriteJson(jsonWriter);
                }
                if (request.ExpiresAt != null)
                {
                    jsonWriter.WritePropertyName("expiresAt");
                    jsonWriter.Write(request.ExpiresAt.ToString());
                }
                if (request.ContextStack != null)
                {
                    jsonWriter.WritePropertyName("contextStack");
                    jsonWriter.Write(request.ContextStack.ToString());
                }
                jsonWriter.WriteObjectEnd();

                var body = stringBuilder.ToString();
                if (!string.IsNullOrEmpty(body))
                {
                    sessionRequest.Body = body;
                }
                sessionRequest.AddHeader("Content-Type", "application/json");

                if (request.RequestId != null)
                {
                    sessionRequest.AddHeader("X-GS2-REQUEST-ID", request.RequestId);
                }

                AddHeader(
                    Session.Credential,
                    sessionRequest
                );

                return sessionRequest;
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator CreateGlobalMessageMaster(
                Request.CreateGlobalMessageMasterRequest request,
                UnityAction<AsyncResult<Result.CreateGlobalMessageMasterResult>> callback
        )
		{
			var task = new CreateGlobalMessageMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.CreateGlobalMessageMasterResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.CreateGlobalMessageMasterResult> CreateGlobalMessageMasterAsync(
                Request.CreateGlobalMessageMasterRequest request
        )
		{
			var task = new CreateGlobalMessageMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        private class GetGlobalMessageMasterTask : Gs2RestSessionTask<GetGlobalMessageMasterRequest, GetGlobalMessageMasterResult>
        {
            public GetGlobalMessageMasterTask(IGs2Session session, RestSessionRequestFactory factory, GetGlobalMessageMasterRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(GetGlobalMessageMasterRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "inbox")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/master/globalMessage/{globalMessageName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{globalMessageName}", !string.IsNullOrEmpty(request.GlobalMessageName) ? request.GlobalMessageName.ToString() : "null");

                var sessionRequest = Factory.Get(url);
                if (request.ContextStack != null)
                {
                    sessionRequest.AddQueryString("contextStack", request.ContextStack);
                }

                if (request.RequestId != null)
                {
                    sessionRequest.AddHeader("X-GS2-REQUEST-ID", request.RequestId);
                }

                AddHeader(
                    Session.Credential,
                    sessionRequest
                );

                return sessionRequest;
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator GetGlobalMessageMaster(
                Request.GetGlobalMessageMasterRequest request,
                UnityAction<AsyncResult<Result.GetGlobalMessageMasterResult>> callback
        )
		{
			var task = new GetGlobalMessageMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.GetGlobalMessageMasterResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.GetGlobalMessageMasterResult> GetGlobalMessageMasterAsync(
                Request.GetGlobalMessageMasterRequest request
        )
		{
			var task = new GetGlobalMessageMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        private class UpdateGlobalMessageMasterTask : Gs2RestSessionTask<UpdateGlobalMessageMasterRequest, UpdateGlobalMessageMasterResult>
        {
            public UpdateGlobalMessageMasterTask(IGs2Session session, RestSessionRequestFactory factory, UpdateGlobalMessageMasterRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(UpdateGlobalMessageMasterRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "inbox")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/master/globalMessage/{globalMessageName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{globalMessageName}", !string.IsNullOrEmpty(request.GlobalMessageName) ? request.GlobalMessageName.ToString() : "null");

                var sessionRequest = Factory.Put(url);

                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);
                jsonWriter.WriteObjectStart();
                if (request.Metadata != null)
                {
                    jsonWriter.WritePropertyName("metadata");
                    jsonWriter.Write(request.Metadata);
                }
                if (request.ReadAcquireActions != null)
                {
                    jsonWriter.WritePropertyName("readAcquireActions");
                    jsonWriter.WriteArrayStart();
                    foreach(var item in request.ReadAcquireActions)
                    {
                        item.WriteJson(jsonWriter);
                    }
                    jsonWriter.WriteArrayEnd();
                }
                if (request.ExpiresTimeSpan != null)
                {
                    jsonWriter.WritePropertyName("expiresTimeSpan");
                    request.ExpiresTimeSpan.WriteJson(jsonWriter);
                }
                if (request.ExpiresAt != null)
                {
                    jsonWriter.WritePropertyName("expiresAt");
                    jsonWriter.Write(request.ExpiresAt.ToString());
                }
                if (request.ContextStack != null)
                {
                    jsonWriter.WritePropertyName("contextStack");
                    jsonWriter.Write(request.ContextStack.ToString());
                }
                jsonWriter.WriteObjectEnd();

                var body = stringBuilder.ToString();
                if (!string.IsNullOrEmpty(body))
                {
                    sessionRequest.Body = body;
                }
                sessionRequest.AddHeader("Content-Type", "application/json");

                if (request.RequestId != null)
                {
                    sessionRequest.AddHeader("X-GS2-REQUEST-ID", request.RequestId);
                }

                AddHeader(
                    Session.Credential,
                    sessionRequest
                );

                return sessionRequest;
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator UpdateGlobalMessageMaster(
                Request.UpdateGlobalMessageMasterRequest request,
                UnityAction<AsyncResult<Result.UpdateGlobalMessageMasterResult>> callback
        )
		{
			var task = new UpdateGlobalMessageMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.UpdateGlobalMessageMasterResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.UpdateGlobalMessageMasterResult> UpdateGlobalMessageMasterAsync(
                Request.UpdateGlobalMessageMasterRequest request
        )
		{
			var task = new UpdateGlobalMessageMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        private class DeleteGlobalMessageMasterTask : Gs2RestSessionTask<DeleteGlobalMessageMasterRequest, DeleteGlobalMessageMasterResult>
        {
            public DeleteGlobalMessageMasterTask(IGs2Session session, RestSessionRequestFactory factory, DeleteGlobalMessageMasterRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DeleteGlobalMessageMasterRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "inbox")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/master/globalMessage/{globalMessageName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{globalMessageName}", !string.IsNullOrEmpty(request.GlobalMessageName) ? request.GlobalMessageName.ToString() : "null");

                var sessionRequest = Factory.Delete(url);
                if (request.ContextStack != null)
                {
                    sessionRequest.AddQueryString("contextStack", request.ContextStack);
                }

                if (request.RequestId != null)
                {
                    sessionRequest.AddHeader("X-GS2-REQUEST-ID", request.RequestId);
                }

                AddHeader(
                    Session.Credential,
                    sessionRequest
                );

                return sessionRequest;
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator DeleteGlobalMessageMaster(
                Request.DeleteGlobalMessageMasterRequest request,
                UnityAction<AsyncResult<Result.DeleteGlobalMessageMasterResult>> callback
        )
		{
			var task = new DeleteGlobalMessageMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.DeleteGlobalMessageMasterResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.DeleteGlobalMessageMasterResult> DeleteGlobalMessageMasterAsync(
                Request.DeleteGlobalMessageMasterRequest request
        )
		{
			var task = new DeleteGlobalMessageMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        private class DescribeGlobalMessagesTask : Gs2RestSessionTask<DescribeGlobalMessagesRequest, DescribeGlobalMessagesResult>
        {
            public DescribeGlobalMessagesTask(IGs2Session session, RestSessionRequestFactory factory, DescribeGlobalMessagesRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DescribeGlobalMessagesRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "inbox")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/globalMessage";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");

                var sessionRequest = Factory.Get(url);
                if (request.ContextStack != null)
                {
                    sessionRequest.AddQueryString("contextStack", request.ContextStack);
                }

                if (request.RequestId != null)
                {
                    sessionRequest.AddHeader("X-GS2-REQUEST-ID", request.RequestId);
                }

                AddHeader(
                    Session.Credential,
                    sessionRequest
                );

                return sessionRequest;
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator DescribeGlobalMessages(
                Request.DescribeGlobalMessagesRequest request,
                UnityAction<AsyncResult<Result.DescribeGlobalMessagesResult>> callback
        )
		{
			var task = new DescribeGlobalMessagesTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.DescribeGlobalMessagesResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.DescribeGlobalMessagesResult> DescribeGlobalMessagesAsync(
                Request.DescribeGlobalMessagesRequest request
        )
		{
			var task = new DescribeGlobalMessagesTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        private class GetGlobalMessageTask : Gs2RestSessionTask<GetGlobalMessageRequest, GetGlobalMessageResult>
        {
            public GetGlobalMessageTask(IGs2Session session, RestSessionRequestFactory factory, GetGlobalMessageRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(GetGlobalMessageRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "inbox")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/globalMessage/{globalMessageName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{globalMessageName}", !string.IsNullOrEmpty(request.GlobalMessageName) ? request.GlobalMessageName.ToString() : "null");

                var sessionRequest = Factory.Get(url);
                if (request.ContextStack != null)
                {
                    sessionRequest.AddQueryString("contextStack", request.ContextStack);
                }

                if (request.RequestId != null)
                {
                    sessionRequest.AddHeader("X-GS2-REQUEST-ID", request.RequestId);
                }

                AddHeader(
                    Session.Credential,
                    sessionRequest
                );

                return sessionRequest;
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator GetGlobalMessage(
                Request.GetGlobalMessageRequest request,
                UnityAction<AsyncResult<Result.GetGlobalMessageResult>> callback
        )
		{
			var task = new GetGlobalMessageTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.GetGlobalMessageResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.GetGlobalMessageResult> GetGlobalMessageAsync(
                Request.GetGlobalMessageRequest request
        )
		{
			var task = new GetGlobalMessageTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        private class GetReceivedByUserIdTask : Gs2RestSessionTask<GetReceivedByUserIdRequest, GetReceivedByUserIdResult>
        {
            public GetReceivedByUserIdTask(IGs2Session session, RestSessionRequestFactory factory, GetReceivedByUserIdRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(GetReceivedByUserIdRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "inbox")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/{userId}/received";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{userId}", !string.IsNullOrEmpty(request.UserId) ? request.UserId.ToString() : "null");

                var sessionRequest = Factory.Get(url);
                if (request.ContextStack != null)
                {
                    sessionRequest.AddQueryString("contextStack", request.ContextStack);
                }

                if (request.RequestId != null)
                {
                    sessionRequest.AddHeader("X-GS2-REQUEST-ID", request.RequestId);
                }

                AddHeader(
                    Session.Credential,
                    sessionRequest
                );

                return sessionRequest;
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator GetReceivedByUserId(
                Request.GetReceivedByUserIdRequest request,
                UnityAction<AsyncResult<Result.GetReceivedByUserIdResult>> callback
        )
		{
			var task = new GetReceivedByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.GetReceivedByUserIdResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.GetReceivedByUserIdResult> GetReceivedByUserIdAsync(
                Request.GetReceivedByUserIdRequest request
        )
		{
			var task = new GetReceivedByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        private class UpdateReceivedByUserIdTask : Gs2RestSessionTask<UpdateReceivedByUserIdRequest, UpdateReceivedByUserIdResult>
        {
            public UpdateReceivedByUserIdTask(IGs2Session session, RestSessionRequestFactory factory, UpdateReceivedByUserIdRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(UpdateReceivedByUserIdRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "inbox")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/{userId}/received";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{userId}", !string.IsNullOrEmpty(request.UserId) ? request.UserId.ToString() : "null");

                var sessionRequest = Factory.Put(url);

                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);
                jsonWriter.WriteObjectStart();
                if (request.ReceivedGlobalMessageNames != null)
                {
                    jsonWriter.WritePropertyName("receivedGlobalMessageNames");
                    jsonWriter.WriteArrayStart();
                    foreach(var item in request.ReceivedGlobalMessageNames)
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
                jsonWriter.WriteObjectEnd();

                var body = stringBuilder.ToString();
                if (!string.IsNullOrEmpty(body))
                {
                    sessionRequest.Body = body;
                }
                sessionRequest.AddHeader("Content-Type", "application/json");

                if (request.RequestId != null)
                {
                    sessionRequest.AddHeader("X-GS2-REQUEST-ID", request.RequestId);
                }

                AddHeader(
                    Session.Credential,
                    sessionRequest
                );

                return sessionRequest;
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator UpdateReceivedByUserId(
                Request.UpdateReceivedByUserIdRequest request,
                UnityAction<AsyncResult<Result.UpdateReceivedByUserIdResult>> callback
        )
		{
			var task = new UpdateReceivedByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.UpdateReceivedByUserIdResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.UpdateReceivedByUserIdResult> UpdateReceivedByUserIdAsync(
                Request.UpdateReceivedByUserIdRequest request
        )
		{
			var task = new UpdateReceivedByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        private class DeleteReceivedByUserIdTask : Gs2RestSessionTask<DeleteReceivedByUserIdRequest, DeleteReceivedByUserIdResult>
        {
            public DeleteReceivedByUserIdTask(IGs2Session session, RestSessionRequestFactory factory, DeleteReceivedByUserIdRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DeleteReceivedByUserIdRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "inbox")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/{userId}/received";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{userId}", !string.IsNullOrEmpty(request.UserId) ? request.UserId.ToString() : "null");

                var sessionRequest = Factory.Delete(url);
                if (request.ContextStack != null)
                {
                    sessionRequest.AddQueryString("contextStack", request.ContextStack);
                }

                if (request.RequestId != null)
                {
                    sessionRequest.AddHeader("X-GS2-REQUEST-ID", request.RequestId);
                }

                AddHeader(
                    Session.Credential,
                    sessionRequest
                );

                return sessionRequest;
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator DeleteReceivedByUserId(
                Request.DeleteReceivedByUserIdRequest request,
                UnityAction<AsyncResult<Result.DeleteReceivedByUserIdResult>> callback
        )
		{
			var task = new DeleteReceivedByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.DeleteReceivedByUserIdResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.DeleteReceivedByUserIdResult> DeleteReceivedByUserIdAsync(
                Request.DeleteReceivedByUserIdRequest request
        )
		{
			var task = new DeleteReceivedByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif
	}
}