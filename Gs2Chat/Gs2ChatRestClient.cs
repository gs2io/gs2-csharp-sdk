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
using Gs2.Gs2Chat.Request;
using Gs2.Gs2Chat.Result;
using Gs2.Util.LitJson;

namespace Gs2.Gs2Chat
{
	public class Gs2ChatRestClient : AbstractGs2Client
	{
#if UNITY_2017_1_OR_NEWER
		private readonly CertificateHandler _certificateHandler;
#endif

		public static string Endpoint = "chat";

        protected Gs2RestSession Gs2RestSession => (Gs2RestSession) Gs2Session;

		public Gs2ChatRestClient(Gs2RestSession Gs2RestSession) : base(Gs2RestSession)
		{

		}

#if UNITY_2017_1_OR_NEWER
		public Gs2ChatRestClient(Gs2RestSession gs2RestSession, CertificateHandler certificateHandler) : base(gs2RestSession)
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
                    .Replace("{service}", "chat")
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
                    .Replace("{service}", "chat")
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
                if (request.AllowCreateRoom != null)
                {
                    jsonWriter.WritePropertyName("allowCreateRoom");
                    jsonWriter.Write(request.AllowCreateRoom.ToString());
                }
                if (request.PostMessageScript != null)
                {
                    jsonWriter.WritePropertyName("postMessageScript");
                    request.PostMessageScript.WriteJson(jsonWriter);
                }
                if (request.CreateRoomScript != null)
                {
                    jsonWriter.WritePropertyName("createRoomScript");
                    request.CreateRoomScript.WriteJson(jsonWriter);
                }
                if (request.DeleteRoomScript != null)
                {
                    jsonWriter.WritePropertyName("deleteRoomScript");
                    request.DeleteRoomScript.WriteJson(jsonWriter);
                }
                if (request.SubscribeRoomScript != null)
                {
                    jsonWriter.WritePropertyName("subscribeRoomScript");
                    request.SubscribeRoomScript.WriteJson(jsonWriter);
                }
                if (request.UnsubscribeRoomScript != null)
                {
                    jsonWriter.WritePropertyName("unsubscribeRoomScript");
                    request.UnsubscribeRoomScript.WriteJson(jsonWriter);
                }
                if (request.PostNotification != null)
                {
                    jsonWriter.WritePropertyName("postNotification");
                    request.PostNotification.WriteJson(jsonWriter);
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
                    .Replace("{service}", "chat")
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
                    .Replace("{service}", "chat")
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
                    .Replace("{service}", "chat")
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
                if (request.AllowCreateRoom != null)
                {
                    jsonWriter.WritePropertyName("allowCreateRoom");
                    jsonWriter.Write(request.AllowCreateRoom.ToString());
                }
                if (request.PostMessageScript != null)
                {
                    jsonWriter.WritePropertyName("postMessageScript");
                    request.PostMessageScript.WriteJson(jsonWriter);
                }
                if (request.CreateRoomScript != null)
                {
                    jsonWriter.WritePropertyName("createRoomScript");
                    request.CreateRoomScript.WriteJson(jsonWriter);
                }
                if (request.DeleteRoomScript != null)
                {
                    jsonWriter.WritePropertyName("deleteRoomScript");
                    request.DeleteRoomScript.WriteJson(jsonWriter);
                }
                if (request.SubscribeRoomScript != null)
                {
                    jsonWriter.WritePropertyName("subscribeRoomScript");
                    request.SubscribeRoomScript.WriteJson(jsonWriter);
                }
                if (request.UnsubscribeRoomScript != null)
                {
                    jsonWriter.WritePropertyName("unsubscribeRoomScript");
                    request.UnsubscribeRoomScript.WriteJson(jsonWriter);
                }
                if (request.PostNotification != null)
                {
                    jsonWriter.WritePropertyName("postNotification");
                    request.PostNotification.WriteJson(jsonWriter);
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
                    .Replace("{service}", "chat")
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


        private class DescribeRoomsTask : Gs2RestSessionTask<DescribeRoomsRequest, DescribeRoomsResult>
        {
            public DescribeRoomsTask(IGs2Session session, RestSessionRequestFactory factory, DescribeRoomsRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DescribeRoomsRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "chat")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/room";

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
		public IEnumerator DescribeRooms(
                Request.DescribeRoomsRequest request,
                UnityAction<AsyncResult<Result.DescribeRoomsResult>> callback
        )
		{
			var task = new DescribeRoomsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.DescribeRoomsResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.DescribeRoomsResult> DescribeRoomsAsync(
                Request.DescribeRoomsRequest request
        )
		{
			var task = new DescribeRoomsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        private class CreateRoomTask : Gs2RestSessionTask<CreateRoomRequest, CreateRoomResult>
        {
            public CreateRoomTask(IGs2Session session, RestSessionRequestFactory factory, CreateRoomRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(CreateRoomRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "chat")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/room/user";

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
                if (request.Password != null)
                {
                    jsonWriter.WritePropertyName("password");
                    jsonWriter.Write(request.Password);
                }
                if (request.WhiteListUserIds != null)
                {
                    jsonWriter.WritePropertyName("whiteListUserIds");
                    jsonWriter.WriteArrayStart();
                    foreach(var item in request.WhiteListUserIds)
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
		public IEnumerator CreateRoom(
                Request.CreateRoomRequest request,
                UnityAction<AsyncResult<Result.CreateRoomResult>> callback
        )
		{
			var task = new CreateRoomTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.CreateRoomResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.CreateRoomResult> CreateRoomAsync(
                Request.CreateRoomRequest request
        )
		{
			var task = new CreateRoomTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        private class CreateRoomFromBackendTask : Gs2RestSessionTask<CreateRoomFromBackendRequest, CreateRoomFromBackendResult>
        {
            public CreateRoomFromBackendTask(IGs2Session session, RestSessionRequestFactory factory, CreateRoomFromBackendRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(CreateRoomFromBackendRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "chat")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/room";

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
                if (request.UserId != null)
                {
                    jsonWriter.WritePropertyName("userId");
                    jsonWriter.Write(request.UserId);
                }
                if (request.Metadata != null)
                {
                    jsonWriter.WritePropertyName("metadata");
                    jsonWriter.Write(request.Metadata);
                }
                if (request.Password != null)
                {
                    jsonWriter.WritePropertyName("password");
                    jsonWriter.Write(request.Password);
                }
                if (request.WhiteListUserIds != null)
                {
                    jsonWriter.WritePropertyName("whiteListUserIds");
                    jsonWriter.WriteArrayStart();
                    foreach(var item in request.WhiteListUserIds)
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
		public IEnumerator CreateRoomFromBackend(
                Request.CreateRoomFromBackendRequest request,
                UnityAction<AsyncResult<Result.CreateRoomFromBackendResult>> callback
        )
		{
			var task = new CreateRoomFromBackendTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.CreateRoomFromBackendResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.CreateRoomFromBackendResult> CreateRoomFromBackendAsync(
                Request.CreateRoomFromBackendRequest request
        )
		{
			var task = new CreateRoomFromBackendTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        private class GetRoomTask : Gs2RestSessionTask<GetRoomRequest, GetRoomResult>
        {
            public GetRoomTask(IGs2Session session, RestSessionRequestFactory factory, GetRoomRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(GetRoomRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "chat")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/room/{roomName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{roomName}", !string.IsNullOrEmpty(request.RoomName) ? request.RoomName.ToString() : "null");

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
		public IEnumerator GetRoom(
                Request.GetRoomRequest request,
                UnityAction<AsyncResult<Result.GetRoomResult>> callback
        )
		{
			var task = new GetRoomTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.GetRoomResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.GetRoomResult> GetRoomAsync(
                Request.GetRoomRequest request
        )
		{
			var task = new GetRoomTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        private class UpdateRoomTask : Gs2RestSessionTask<UpdateRoomRequest, UpdateRoomResult>
        {
            public UpdateRoomTask(IGs2Session session, RestSessionRequestFactory factory, UpdateRoomRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(UpdateRoomRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "chat")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/room/{roomName}/user";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{roomName}", !string.IsNullOrEmpty(request.RoomName) ? request.RoomName.ToString() : "null");

                var sessionRequest = Factory.Put(url);

                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);
                jsonWriter.WriteObjectStart();
                if (request.Metadata != null)
                {
                    jsonWriter.WritePropertyName("metadata");
                    jsonWriter.Write(request.Metadata);
                }
                if (request.Password != null)
                {
                    jsonWriter.WritePropertyName("password");
                    jsonWriter.Write(request.Password);
                }
                if (request.WhiteListUserIds != null)
                {
                    jsonWriter.WritePropertyName("whiteListUserIds");
                    jsonWriter.WriteArrayStart();
                    foreach(var item in request.WhiteListUserIds)
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
		public IEnumerator UpdateRoom(
                Request.UpdateRoomRequest request,
                UnityAction<AsyncResult<Result.UpdateRoomResult>> callback
        )
		{
			var task = new UpdateRoomTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.UpdateRoomResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.UpdateRoomResult> UpdateRoomAsync(
                Request.UpdateRoomRequest request
        )
		{
			var task = new UpdateRoomTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        private class UpdateRoomFromBackendTask : Gs2RestSessionTask<UpdateRoomFromBackendRequest, UpdateRoomFromBackendResult>
        {
            public UpdateRoomFromBackendTask(IGs2Session session, RestSessionRequestFactory factory, UpdateRoomFromBackendRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(UpdateRoomFromBackendRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "chat")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/room/{roomName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{roomName}", !string.IsNullOrEmpty(request.RoomName) ? request.RoomName.ToString() : "null");

                var sessionRequest = Factory.Put(url);

                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);
                jsonWriter.WriteObjectStart();
                if (request.Metadata != null)
                {
                    jsonWriter.WritePropertyName("metadata");
                    jsonWriter.Write(request.Metadata);
                }
                if (request.Password != null)
                {
                    jsonWriter.WritePropertyName("password");
                    jsonWriter.Write(request.Password);
                }
                if (request.WhiteListUserIds != null)
                {
                    jsonWriter.WritePropertyName("whiteListUserIds");
                    jsonWriter.WriteArrayStart();
                    foreach(var item in request.WhiteListUserIds)
                    {
                        jsonWriter.Write(item);
                    }
                    jsonWriter.WriteArrayEnd();
                }
                if (request.UserId != null)
                {
                    jsonWriter.WritePropertyName("userId");
                    jsonWriter.Write(request.UserId);
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
		public IEnumerator UpdateRoomFromBackend(
                Request.UpdateRoomFromBackendRequest request,
                UnityAction<AsyncResult<Result.UpdateRoomFromBackendResult>> callback
        )
		{
			var task = new UpdateRoomFromBackendTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.UpdateRoomFromBackendResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.UpdateRoomFromBackendResult> UpdateRoomFromBackendAsync(
                Request.UpdateRoomFromBackendRequest request
        )
		{
			var task = new UpdateRoomFromBackendTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        private class DeleteRoomTask : Gs2RestSessionTask<DeleteRoomRequest, DeleteRoomResult>
        {
            public DeleteRoomTask(IGs2Session session, RestSessionRequestFactory factory, DeleteRoomRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DeleteRoomRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "chat")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/room/{roomName}/user";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{roomName}", !string.IsNullOrEmpty(request.RoomName) ? request.RoomName.ToString() : "null");

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
		public IEnumerator DeleteRoom(
                Request.DeleteRoomRequest request,
                UnityAction<AsyncResult<Result.DeleteRoomResult>> callback
        )
		{
			var task = new DeleteRoomTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.DeleteRoomResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.DeleteRoomResult> DeleteRoomAsync(
                Request.DeleteRoomRequest request
        )
		{
			var task = new DeleteRoomTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        private class DeleteRoomFromBackendTask : Gs2RestSessionTask<DeleteRoomFromBackendRequest, DeleteRoomFromBackendResult>
        {
            public DeleteRoomFromBackendTask(IGs2Session session, RestSessionRequestFactory factory, DeleteRoomFromBackendRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DeleteRoomFromBackendRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "chat")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/room/{roomName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{roomName}", !string.IsNullOrEmpty(request.RoomName) ? request.RoomName.ToString() : "null");

                var sessionRequest = Factory.Delete(url);
                if (request.ContextStack != null)
                {
                    sessionRequest.AddQueryString("contextStack", request.ContextStack);
                }
                if (request.UserId != null) {
                    sessionRequest.AddQueryString("userId", $"{request.UserId}");
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
		public IEnumerator DeleteRoomFromBackend(
                Request.DeleteRoomFromBackendRequest request,
                UnityAction<AsyncResult<Result.DeleteRoomFromBackendResult>> callback
        )
		{
			var task = new DeleteRoomFromBackendTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.DeleteRoomFromBackendResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.DeleteRoomFromBackendResult> DeleteRoomFromBackendAsync(
                Request.DeleteRoomFromBackendRequest request
        )
		{
			var task = new DeleteRoomFromBackendTask(
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
                    .Replace("{service}", "chat")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/room/{roomName}/message";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{roomName}", !string.IsNullOrEmpty(request.RoomName) ? request.RoomName.ToString() : "null");

                var sessionRequest = Factory.Get(url);
                if (request.ContextStack != null)
                {
                    sessionRequest.AddQueryString("contextStack", request.ContextStack);
                }
                if (request.Password != null) {
                    sessionRequest.AddQueryString("password", $"{request.Password}");
                }
                if (request.StartAt != null) {
                    sessionRequest.AddQueryString("startAt", $"{request.StartAt}");
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
                    .Replace("{service}", "chat")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/room/{roomName}/message/get";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{roomName}", !string.IsNullOrEmpty(request.RoomName) ? request.RoomName.ToString() : "null");

                var sessionRequest = Factory.Get(url);
                if (request.ContextStack != null)
                {
                    sessionRequest.AddQueryString("contextStack", request.ContextStack);
                }
                if (request.Password != null) {
                    sessionRequest.AddQueryString("password", $"{request.Password}");
                }
                if (request.UserId != null) {
                    sessionRequest.AddQueryString("userId", $"{request.UserId}");
                }
                if (request.StartAt != null) {
                    sessionRequest.AddQueryString("startAt", $"{request.StartAt}");
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


        private class PostTask : Gs2RestSessionTask<PostRequest, PostResult>
        {
            public PostTask(IGs2Session session, RestSessionRequestFactory factory, PostRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(PostRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "chat")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/room/{roomName}/message";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{roomName}", !string.IsNullOrEmpty(request.RoomName) ? request.RoomName.ToString() : "null");

                var sessionRequest = Factory.Post(url);

                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);
                jsonWriter.WriteObjectStart();
                if (request.Category != null)
                {
                    jsonWriter.WritePropertyName("category");
                    jsonWriter.Write(request.Category.ToString());
                }
                if (request.Metadata != null)
                {
                    jsonWriter.WritePropertyName("metadata");
                    jsonWriter.Write(request.Metadata);
                }
                if (request.Password != null)
                {
                    jsonWriter.WritePropertyName("password");
                    jsonWriter.Write(request.Password);
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
		public IEnumerator Post(
                Request.PostRequest request,
                UnityAction<AsyncResult<Result.PostResult>> callback
        )
		{
			var task = new PostTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.PostResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.PostResult> PostAsync(
                Request.PostRequest request
        )
		{
			var task = new PostTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        private class PostByUserIdTask : Gs2RestSessionTask<PostByUserIdRequest, PostByUserIdResult>
        {
            public PostByUserIdTask(IGs2Session session, RestSessionRequestFactory factory, PostByUserIdRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(PostByUserIdRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "chat")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/room/{roomName}/message/user/{userId}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{roomName}", !string.IsNullOrEmpty(request.RoomName) ? request.RoomName.ToString() : "null");
                url = url.Replace("{userId}", !string.IsNullOrEmpty(request.UserId) ? request.UserId.ToString() : "null");

                var sessionRequest = Factory.Post(url);

                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);
                jsonWriter.WriteObjectStart();
                if (request.Category != null)
                {
                    jsonWriter.WritePropertyName("category");
                    jsonWriter.Write(request.Category.ToString());
                }
                if (request.Metadata != null)
                {
                    jsonWriter.WritePropertyName("metadata");
                    jsonWriter.Write(request.Metadata);
                }
                if (request.Password != null)
                {
                    jsonWriter.WritePropertyName("password");
                    jsonWriter.Write(request.Password);
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
		public IEnumerator PostByUserId(
                Request.PostByUserIdRequest request,
                UnityAction<AsyncResult<Result.PostByUserIdResult>> callback
        )
		{
			var task = new PostByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.PostByUserIdResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.PostByUserIdResult> PostByUserIdAsync(
                Request.PostByUserIdRequest request
        )
		{
			var task = new PostByUserIdTask(
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
                    .Replace("{service}", "chat")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/room/{roomName}/message/{messageName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{roomName}", !string.IsNullOrEmpty(request.RoomName) ? request.RoomName.ToString() : "null");
                url = url.Replace("{messageName}", !string.IsNullOrEmpty(request.MessageName) ? request.MessageName.ToString() : "null");

                var sessionRequest = Factory.Get(url);
                if (request.ContextStack != null)
                {
                    sessionRequest.AddQueryString("contextStack", request.ContextStack);
                }
                if (request.Password != null) {
                    sessionRequest.AddQueryString("password", $"{request.Password}");
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
                    .Replace("{service}", "chat")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/room/{roomName}/message/{messageName}/get";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{roomName}", !string.IsNullOrEmpty(request.RoomName) ? request.RoomName.ToString() : "null");
                url = url.Replace("{messageName}", !string.IsNullOrEmpty(request.MessageName) ? request.MessageName.ToString() : "null");

                var sessionRequest = Factory.Get(url);
                if (request.ContextStack != null)
                {
                    sessionRequest.AddQueryString("contextStack", request.ContextStack);
                }
                if (request.Password != null) {
                    sessionRequest.AddQueryString("password", $"{request.Password}");
                }
                if (request.UserId != null) {
                    sessionRequest.AddQueryString("userId", $"{request.UserId}");
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


        private class DeleteMessageTask : Gs2RestSessionTask<DeleteMessageRequest, DeleteMessageResult>
        {
            public DeleteMessageTask(IGs2Session session, RestSessionRequestFactory factory, DeleteMessageRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DeleteMessageRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "chat")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/room/{roomName}/message/{messageName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{roomName}", !string.IsNullOrEmpty(request.RoomName) ? request.RoomName.ToString() : "null");
                url = url.Replace("{messageName}", !string.IsNullOrEmpty(request.MessageName) ? request.MessageName.ToString() : "null");

                var sessionRequest = Factory.Delete(url);
                if (request.ContextStack != null)
                {
                    sessionRequest.AddQueryString("contextStack", request.ContextStack);
                }
                if (request.UserId != null) {
                    sessionRequest.AddQueryString("userId", $"{request.UserId}");
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


        private class DescribeSubscribesTask : Gs2RestSessionTask<DescribeSubscribesRequest, DescribeSubscribesResult>
        {
            public DescribeSubscribesTask(IGs2Session session, RestSessionRequestFactory factory, DescribeSubscribesRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DescribeSubscribesRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "chat")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/me/room/subscribe";

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
		public IEnumerator DescribeSubscribes(
                Request.DescribeSubscribesRequest request,
                UnityAction<AsyncResult<Result.DescribeSubscribesResult>> callback
        )
		{
			var task = new DescribeSubscribesTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.DescribeSubscribesResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.DescribeSubscribesResult> DescribeSubscribesAsync(
                Request.DescribeSubscribesRequest request
        )
		{
			var task = new DescribeSubscribesTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        private class DescribeSubscribesByUserIdTask : Gs2RestSessionTask<DescribeSubscribesByUserIdRequest, DescribeSubscribesByUserIdResult>
        {
            public DescribeSubscribesByUserIdTask(IGs2Session session, RestSessionRequestFactory factory, DescribeSubscribesByUserIdRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DescribeSubscribesByUserIdRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "chat")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/{userId}/room/subscribe";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{userId}", !string.IsNullOrEmpty(request.UserId) ? request.UserId.ToString() : "null");

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
		public IEnumerator DescribeSubscribesByUserId(
                Request.DescribeSubscribesByUserIdRequest request,
                UnityAction<AsyncResult<Result.DescribeSubscribesByUserIdResult>> callback
        )
		{
			var task = new DescribeSubscribesByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.DescribeSubscribesByUserIdResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.DescribeSubscribesByUserIdResult> DescribeSubscribesByUserIdAsync(
                Request.DescribeSubscribesByUserIdRequest request
        )
		{
			var task = new DescribeSubscribesByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        private class DescribeSubscribesByRoomNameTask : Gs2RestSessionTask<DescribeSubscribesByRoomNameRequest, DescribeSubscribesByRoomNameResult>
        {
            public DescribeSubscribesByRoomNameTask(IGs2Session session, RestSessionRequestFactory factory, DescribeSubscribesByRoomNameRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DescribeSubscribesByRoomNameRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "chat")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/room/{roomName}/subscribe";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{roomName}", !string.IsNullOrEmpty(request.RoomName) ? request.RoomName.ToString() : "null");

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
		public IEnumerator DescribeSubscribesByRoomName(
                Request.DescribeSubscribesByRoomNameRequest request,
                UnityAction<AsyncResult<Result.DescribeSubscribesByRoomNameResult>> callback
        )
		{
			var task = new DescribeSubscribesByRoomNameTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.DescribeSubscribesByRoomNameResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.DescribeSubscribesByRoomNameResult> DescribeSubscribesByRoomNameAsync(
                Request.DescribeSubscribesByRoomNameRequest request
        )
		{
			var task = new DescribeSubscribesByRoomNameTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        private class SubscribeTask : Gs2RestSessionTask<SubscribeRequest, SubscribeResult>
        {
            public SubscribeTask(IGs2Session session, RestSessionRequestFactory factory, SubscribeRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(SubscribeRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "chat")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/me/room/{roomName}/subscribe";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{roomName}", !string.IsNullOrEmpty(request.RoomName) ? request.RoomName.ToString() : "null");

                var sessionRequest = Factory.Post(url);

                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);
                jsonWriter.WriteObjectStart();
                if (request.NotificationTypes != null)
                {
                    jsonWriter.WritePropertyName("notificationTypes");
                    jsonWriter.WriteArrayStart();
                    foreach(var item in request.NotificationTypes)
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
		public IEnumerator Subscribe(
                Request.SubscribeRequest request,
                UnityAction<AsyncResult<Result.SubscribeResult>> callback
        )
		{
			var task = new SubscribeTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.SubscribeResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.SubscribeResult> SubscribeAsync(
                Request.SubscribeRequest request
        )
		{
			var task = new SubscribeTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        private class SubscribeByUserIdTask : Gs2RestSessionTask<SubscribeByUserIdRequest, SubscribeByUserIdResult>
        {
            public SubscribeByUserIdTask(IGs2Session session, RestSessionRequestFactory factory, SubscribeByUserIdRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(SubscribeByUserIdRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "chat")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/{userId}/room/{roomName}/subscribe";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{roomName}", !string.IsNullOrEmpty(request.RoomName) ? request.RoomName.ToString() : "null");
                url = url.Replace("{userId}", !string.IsNullOrEmpty(request.UserId) ? request.UserId.ToString() : "null");

                var sessionRequest = Factory.Post(url);

                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);
                jsonWriter.WriteObjectStart();
                if (request.NotificationTypes != null)
                {
                    jsonWriter.WritePropertyName("notificationTypes");
                    jsonWriter.WriteArrayStart();
                    foreach(var item in request.NotificationTypes)
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
		public IEnumerator SubscribeByUserId(
                Request.SubscribeByUserIdRequest request,
                UnityAction<AsyncResult<Result.SubscribeByUserIdResult>> callback
        )
		{
			var task = new SubscribeByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.SubscribeByUserIdResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.SubscribeByUserIdResult> SubscribeByUserIdAsync(
                Request.SubscribeByUserIdRequest request
        )
		{
			var task = new SubscribeByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        private class GetSubscribeTask : Gs2RestSessionTask<GetSubscribeRequest, GetSubscribeResult>
        {
            public GetSubscribeTask(IGs2Session session, RestSessionRequestFactory factory, GetSubscribeRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(GetSubscribeRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "chat")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/me/room/{roomName}/subscribe";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{roomName}", !string.IsNullOrEmpty(request.RoomName) ? request.RoomName.ToString() : "null");

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
		public IEnumerator GetSubscribe(
                Request.GetSubscribeRequest request,
                UnityAction<AsyncResult<Result.GetSubscribeResult>> callback
        )
		{
			var task = new GetSubscribeTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.GetSubscribeResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.GetSubscribeResult> GetSubscribeAsync(
                Request.GetSubscribeRequest request
        )
		{
			var task = new GetSubscribeTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        private class GetSubscribeByUserIdTask : Gs2RestSessionTask<GetSubscribeByUserIdRequest, GetSubscribeByUserIdResult>
        {
            public GetSubscribeByUserIdTask(IGs2Session session, RestSessionRequestFactory factory, GetSubscribeByUserIdRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(GetSubscribeByUserIdRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "chat")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/{userId}/room/{roomName}/subscribe";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{roomName}", !string.IsNullOrEmpty(request.RoomName) ? request.RoomName.ToString() : "null");
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
		public IEnumerator GetSubscribeByUserId(
                Request.GetSubscribeByUserIdRequest request,
                UnityAction<AsyncResult<Result.GetSubscribeByUserIdResult>> callback
        )
		{
			var task = new GetSubscribeByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.GetSubscribeByUserIdResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.GetSubscribeByUserIdResult> GetSubscribeByUserIdAsync(
                Request.GetSubscribeByUserIdRequest request
        )
		{
			var task = new GetSubscribeByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        private class UpdateNotificationTypeTask : Gs2RestSessionTask<UpdateNotificationTypeRequest, UpdateNotificationTypeResult>
        {
            public UpdateNotificationTypeTask(IGs2Session session, RestSessionRequestFactory factory, UpdateNotificationTypeRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(UpdateNotificationTypeRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "chat")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/me/room/{roomName}/subscribe/notification";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{roomName}", !string.IsNullOrEmpty(request.RoomName) ? request.RoomName.ToString() : "null");

                var sessionRequest = Factory.Post(url);

                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);
                jsonWriter.WriteObjectStart();
                if (request.NotificationTypes != null)
                {
                    jsonWriter.WritePropertyName("notificationTypes");
                    jsonWriter.WriteArrayStart();
                    foreach(var item in request.NotificationTypes)
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
		public IEnumerator UpdateNotificationType(
                Request.UpdateNotificationTypeRequest request,
                UnityAction<AsyncResult<Result.UpdateNotificationTypeResult>> callback
        )
		{
			var task = new UpdateNotificationTypeTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.UpdateNotificationTypeResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.UpdateNotificationTypeResult> UpdateNotificationTypeAsync(
                Request.UpdateNotificationTypeRequest request
        )
		{
			var task = new UpdateNotificationTypeTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        private class UpdateNotificationTypeByUserIdTask : Gs2RestSessionTask<UpdateNotificationTypeByUserIdRequest, UpdateNotificationTypeByUserIdResult>
        {
            public UpdateNotificationTypeByUserIdTask(IGs2Session session, RestSessionRequestFactory factory, UpdateNotificationTypeByUserIdRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(UpdateNotificationTypeByUserIdRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "chat")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/{userId}/room/{roomName}/subscribe/notification";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{roomName}", !string.IsNullOrEmpty(request.RoomName) ? request.RoomName.ToString() : "null");
                url = url.Replace("{userId}", !string.IsNullOrEmpty(request.UserId) ? request.UserId.ToString() : "null");

                var sessionRequest = Factory.Post(url);

                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);
                jsonWriter.WriteObjectStart();
                if (request.NotificationTypes != null)
                {
                    jsonWriter.WritePropertyName("notificationTypes");
                    jsonWriter.WriteArrayStart();
                    foreach(var item in request.NotificationTypes)
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
		public IEnumerator UpdateNotificationTypeByUserId(
                Request.UpdateNotificationTypeByUserIdRequest request,
                UnityAction<AsyncResult<Result.UpdateNotificationTypeByUserIdResult>> callback
        )
		{
			var task = new UpdateNotificationTypeByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.UpdateNotificationTypeByUserIdResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.UpdateNotificationTypeByUserIdResult> UpdateNotificationTypeByUserIdAsync(
                Request.UpdateNotificationTypeByUserIdRequest request
        )
		{
			var task = new UpdateNotificationTypeByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        private class UnsubscribeTask : Gs2RestSessionTask<UnsubscribeRequest, UnsubscribeResult>
        {
            public UnsubscribeTask(IGs2Session session, RestSessionRequestFactory factory, UnsubscribeRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(UnsubscribeRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "chat")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/me/room/{roomName}/subscribe";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{roomName}", !string.IsNullOrEmpty(request.RoomName) ? request.RoomName.ToString() : "null");

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
		public IEnumerator Unsubscribe(
                Request.UnsubscribeRequest request,
                UnityAction<AsyncResult<Result.UnsubscribeResult>> callback
        )
		{
			var task = new UnsubscribeTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.UnsubscribeResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.UnsubscribeResult> UnsubscribeAsync(
                Request.UnsubscribeRequest request
        )
		{
			var task = new UnsubscribeTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        private class UnsubscribeByUserIdTask : Gs2RestSessionTask<UnsubscribeByUserIdRequest, UnsubscribeByUserIdResult>
        {
            public UnsubscribeByUserIdTask(IGs2Session session, RestSessionRequestFactory factory, UnsubscribeByUserIdRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(UnsubscribeByUserIdRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "chat")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/{userId}/room/{roomName}/subscribe";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{roomName}", !string.IsNullOrEmpty(request.RoomName) ? request.RoomName.ToString() : "null");
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
		public IEnumerator UnsubscribeByUserId(
                Request.UnsubscribeByUserIdRequest request,
                UnityAction<AsyncResult<Result.UnsubscribeByUserIdResult>> callback
        )
		{
			var task = new UnsubscribeByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.UnsubscribeByUserIdResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.UnsubscribeByUserIdResult> UnsubscribeByUserIdAsync(
                Request.UnsubscribeByUserIdRequest request
        )
		{
			var task = new UnsubscribeByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif
	}
}