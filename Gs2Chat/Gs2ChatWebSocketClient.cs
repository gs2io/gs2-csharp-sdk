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

namespace Gs2.Gs2Chat
{
	public class Gs2ChatWebSocketClient : AbstractGs2Client
	{

		public static string Endpoint = "chat";

        protected Gs2WebSocketSession Gs2WebSocketSession => (Gs2WebSocketSession) Gs2Session;

		public Gs2ChatWebSocketClient(Gs2WebSocketSession Gs2WebSocketSession) : base(Gs2WebSocketSession)
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
                if (request.RequestId != null)
                {
                    jsonWriter.WritePropertyName("xGs2RequestId");
                    jsonWriter.Write(request.RequestId);
                }

                AddHeader(
                    Session.Credential,
                    "chat",
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
                    "chat",
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
                if (request.RequestId != null)
                {
                    jsonWriter.WritePropertyName("xGs2RequestId");
                    jsonWriter.Write(request.RequestId);
                }

                AddHeader(
                    Session.Credential,
                    "chat",
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
                    "chat",
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


        private class PostTask : Gs2WebSocketSessionTask<Request.PostRequest, Result.PostResult>
        {
	        public PostTask(IGs2Session session, Request.PostRequest request) : base(session, request)
	        {
	        }

            protected override IGs2SessionRequest CreateRequest(Request.PostRequest request)
            {
                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);

                jsonWriter.WriteObjectStart();

                if (request.NamespaceName != null)
                {
                    jsonWriter.WritePropertyName("namespaceName");
                    jsonWriter.Write(request.NamespaceName.ToString());
                }
                if (request.RoomName != null)
                {
                    jsonWriter.WritePropertyName("roomName");
                    jsonWriter.Write(request.RoomName.ToString());
                }
                if (request.AccessToken != null)
                {
                    jsonWriter.WritePropertyName("accessToken");
                    jsonWriter.Write(request.AccessToken.ToString());
                }
                if (request.Category != null)
                {
                    jsonWriter.WritePropertyName("category");
                    jsonWriter.Write(request.Category.ToString());
                }
                if (request.Metadata != null)
                {
                    jsonWriter.WritePropertyName("metadata");
                    jsonWriter.Write(request.Metadata.ToString());
                }
                if (request.Password != null)
                {
                    jsonWriter.WritePropertyName("password");
                    jsonWriter.Write(request.Password.ToString());
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

                AddHeader(
                    Session.Credential,
                    "chat",
                    "message",
                    "post",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator Post(
                Request.PostRequest request,
                UnityAction<AsyncResult<Result.PostResult>> callback
        )
		{
			var task = new PostTask(
			    Gs2WebSocketSession,
			    request
            );
            yield return task;
            callback.Invoke(new AsyncResult<Result.PostResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.PostResult> Post(
            Request.PostRequest request
        )
		{
		    var task = new PostTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
#endif


        private class PostByUserIdTask : Gs2WebSocketSessionTask<Request.PostByUserIdRequest, Result.PostByUserIdResult>
        {
	        public PostByUserIdTask(IGs2Session session, Request.PostByUserIdRequest request) : base(session, request)
	        {
	        }

            protected override IGs2SessionRequest CreateRequest(Request.PostByUserIdRequest request)
            {
                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);

                jsonWriter.WriteObjectStart();

                if (request.NamespaceName != null)
                {
                    jsonWriter.WritePropertyName("namespaceName");
                    jsonWriter.Write(request.NamespaceName.ToString());
                }
                if (request.RoomName != null)
                {
                    jsonWriter.WritePropertyName("roomName");
                    jsonWriter.Write(request.RoomName.ToString());
                }
                if (request.UserId != null)
                {
                    jsonWriter.WritePropertyName("userId");
                    jsonWriter.Write(request.UserId.ToString());
                }
                if (request.Category != null)
                {
                    jsonWriter.WritePropertyName("category");
                    jsonWriter.Write(request.Category.ToString());
                }
                if (request.Metadata != null)
                {
                    jsonWriter.WritePropertyName("metadata");
                    jsonWriter.Write(request.Metadata.ToString());
                }
                if (request.Password != null)
                {
                    jsonWriter.WritePropertyName("password");
                    jsonWriter.Write(request.Password.ToString());
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
                    "chat",
                    "message",
                    "postByUserId",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator PostByUserId(
                Request.PostByUserIdRequest request,
                UnityAction<AsyncResult<Result.PostByUserIdResult>> callback
        )
		{
			var task = new PostByUserIdTask(
			    Gs2WebSocketSession,
			    request
            );
            yield return task;
            callback.Invoke(new AsyncResult<Result.PostByUserIdResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.PostByUserIdResult> PostByUserId(
            Request.PostByUserIdRequest request
        )
		{
		    var task = new PostByUserIdTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
#endif


        private class GetMessageTask : Gs2WebSocketSessionTask<Request.GetMessageRequest, Result.GetMessageResult>
        {
	        public GetMessageTask(IGs2Session session, Request.GetMessageRequest request) : base(session, request)
	        {
	        }

            protected override IGs2SessionRequest CreateRequest(Request.GetMessageRequest request)
            {
                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);

                jsonWriter.WriteObjectStart();

                if (request.NamespaceName != null)
                {
                    jsonWriter.WritePropertyName("namespaceName");
                    jsonWriter.Write(request.NamespaceName.ToString());
                }
                if (request.RoomName != null)
                {
                    jsonWriter.WritePropertyName("roomName");
                    jsonWriter.Write(request.RoomName.ToString());
                }
                if (request.MessageName != null)
                {
                    jsonWriter.WritePropertyName("messageName");
                    jsonWriter.Write(request.MessageName.ToString());
                }
                if (request.Password != null)
                {
                    jsonWriter.WritePropertyName("password");
                    jsonWriter.Write(request.Password.ToString());
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

                AddHeader(
                    Session.Credential,
                    "chat",
                    "message",
                    "getMessage",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator GetMessage(
                Request.GetMessageRequest request,
                UnityAction<AsyncResult<Result.GetMessageResult>> callback
        )
		{
			var task = new GetMessageTask(
			    Gs2WebSocketSession,
			    request
            );
            yield return task;
            callback.Invoke(new AsyncResult<Result.GetMessageResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.GetMessageResult> GetMessage(
            Request.GetMessageRequest request
        )
		{
		    var task = new GetMessageTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
#endif


        private class GetMessageByUserIdTask : Gs2WebSocketSessionTask<Request.GetMessageByUserIdRequest, Result.GetMessageByUserIdResult>
        {
	        public GetMessageByUserIdTask(IGs2Session session, Request.GetMessageByUserIdRequest request) : base(session, request)
	        {
	        }

            protected override IGs2SessionRequest CreateRequest(Request.GetMessageByUserIdRequest request)
            {
                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);

                jsonWriter.WriteObjectStart();

                if (request.NamespaceName != null)
                {
                    jsonWriter.WritePropertyName("namespaceName");
                    jsonWriter.Write(request.NamespaceName.ToString());
                }
                if (request.RoomName != null)
                {
                    jsonWriter.WritePropertyName("roomName");
                    jsonWriter.Write(request.RoomName.ToString());
                }
                if (request.MessageName != null)
                {
                    jsonWriter.WritePropertyName("messageName");
                    jsonWriter.Write(request.MessageName.ToString());
                }
                if (request.Password != null)
                {
                    jsonWriter.WritePropertyName("password");
                    jsonWriter.Write(request.Password.ToString());
                }
                if (request.UserId != null)
                {
                    jsonWriter.WritePropertyName("userId");
                    jsonWriter.Write(request.UserId.ToString());
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
                    "chat",
                    "message",
                    "getMessageByUserId",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator GetMessageByUserId(
                Request.GetMessageByUserIdRequest request,
                UnityAction<AsyncResult<Result.GetMessageByUserIdResult>> callback
        )
		{
			var task = new GetMessageByUserIdTask(
			    Gs2WebSocketSession,
			    request
            );
            yield return task;
            callback.Invoke(new AsyncResult<Result.GetMessageByUserIdResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.GetMessageByUserIdResult> GetMessageByUserId(
            Request.GetMessageByUserIdRequest request
        )
		{
		    var task = new GetMessageByUserIdTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
#endif


        private class DeleteMessageTask : Gs2WebSocketSessionTask<Request.DeleteMessageRequest, Result.DeleteMessageResult>
        {
	        public DeleteMessageTask(IGs2Session session, Request.DeleteMessageRequest request) : base(session, request)
	        {
	        }

            protected override IGs2SessionRequest CreateRequest(Request.DeleteMessageRequest request)
            {
                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);

                jsonWriter.WriteObjectStart();

                if (request.NamespaceName != null)
                {
                    jsonWriter.WritePropertyName("namespaceName");
                    jsonWriter.Write(request.NamespaceName.ToString());
                }
                if (request.RoomName != null)
                {
                    jsonWriter.WritePropertyName("roomName");
                    jsonWriter.Write(request.RoomName.ToString());
                }
                if (request.UserId != null)
                {
                    jsonWriter.WritePropertyName("userId");
                    jsonWriter.Write(request.UserId.ToString());
                }
                if (request.MessageName != null)
                {
                    jsonWriter.WritePropertyName("messageName");
                    jsonWriter.Write(request.MessageName.ToString());
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
                    "chat",
                    "message",
                    "deleteMessage",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator DeleteMessage(
                Request.DeleteMessageRequest request,
                UnityAction<AsyncResult<Result.DeleteMessageResult>> callback
        )
		{
			var task = new DeleteMessageTask(
			    Gs2WebSocketSession,
			    request
            );
            yield return task;
            callback.Invoke(new AsyncResult<Result.DeleteMessageResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.DeleteMessageResult> DeleteMessage(
            Request.DeleteMessageRequest request
        )
		{
		    var task = new DeleteMessageTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
#endif


        private class SubscribeTask : Gs2WebSocketSessionTask<Request.SubscribeRequest, Result.SubscribeResult>
        {
	        public SubscribeTask(IGs2Session session, Request.SubscribeRequest request) : base(session, request)
	        {
	        }

            protected override IGs2SessionRequest CreateRequest(Request.SubscribeRequest request)
            {
                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);

                jsonWriter.WriteObjectStart();

                if (request.NamespaceName != null)
                {
                    jsonWriter.WritePropertyName("namespaceName");
                    jsonWriter.Write(request.NamespaceName.ToString());
                }
                if (request.RoomName != null)
                {
                    jsonWriter.WritePropertyName("roomName");
                    jsonWriter.Write(request.RoomName.ToString());
                }
                if (request.AccessToken != null)
                {
                    jsonWriter.WritePropertyName("accessToken");
                    jsonWriter.Write(request.AccessToken.ToString());
                }
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

                AddHeader(
                    Session.Credential,
                    "chat",
                    "subscribe",
                    "subscribe",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator Subscribe(
                Request.SubscribeRequest request,
                UnityAction<AsyncResult<Result.SubscribeResult>> callback
        )
		{
			var task = new SubscribeTask(
			    Gs2WebSocketSession,
			    request
            );
            yield return task;
            callback.Invoke(new AsyncResult<Result.SubscribeResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.SubscribeResult> Subscribe(
            Request.SubscribeRequest request
        )
		{
		    var task = new SubscribeTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
#endif


        private class SubscribeByUserIdTask : Gs2WebSocketSessionTask<Request.SubscribeByUserIdRequest, Result.SubscribeByUserIdResult>
        {
	        public SubscribeByUserIdTask(IGs2Session session, Request.SubscribeByUserIdRequest request) : base(session, request)
	        {
	        }

            protected override IGs2SessionRequest CreateRequest(Request.SubscribeByUserIdRequest request)
            {
                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);

                jsonWriter.WriteObjectStart();

                if (request.NamespaceName != null)
                {
                    jsonWriter.WritePropertyName("namespaceName");
                    jsonWriter.Write(request.NamespaceName.ToString());
                }
                if (request.RoomName != null)
                {
                    jsonWriter.WritePropertyName("roomName");
                    jsonWriter.Write(request.RoomName.ToString());
                }
                if (request.UserId != null)
                {
                    jsonWriter.WritePropertyName("userId");
                    jsonWriter.Write(request.UserId.ToString());
                }
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
                if (request.RequestId != null)
                {
                    jsonWriter.WritePropertyName("xGs2RequestId");
                    jsonWriter.Write(request.RequestId);
                }

                AddHeader(
                    Session.Credential,
                    "chat",
                    "subscribe",
                    "subscribeByUserId",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator SubscribeByUserId(
                Request.SubscribeByUserIdRequest request,
                UnityAction<AsyncResult<Result.SubscribeByUserIdResult>> callback
        )
		{
			var task = new SubscribeByUserIdTask(
			    Gs2WebSocketSession,
			    request
            );
            yield return task;
            callback.Invoke(new AsyncResult<Result.SubscribeByUserIdResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.SubscribeByUserIdResult> SubscribeByUserId(
            Request.SubscribeByUserIdRequest request
        )
		{
		    var task = new SubscribeByUserIdTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
#endif


        private class GetSubscribeTask : Gs2WebSocketSessionTask<Request.GetSubscribeRequest, Result.GetSubscribeResult>
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
                if (request.RoomName != null)
                {
                    jsonWriter.WritePropertyName("roomName");
                    jsonWriter.Write(request.RoomName.ToString());
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

                AddHeader(
                    Session.Credential,
                    "chat",
                    "subscribe",
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
#else
		public async Task<Result.GetSubscribeResult> GetSubscribe(
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


        private class GetSubscribeByUserIdTask : Gs2WebSocketSessionTask<Request.GetSubscribeByUserIdRequest, Result.GetSubscribeByUserIdResult>
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
                if (request.RoomName != null)
                {
                    jsonWriter.WritePropertyName("roomName");
                    jsonWriter.Write(request.RoomName.ToString());
                }
                if (request.UserId != null)
                {
                    jsonWriter.WritePropertyName("userId");
                    jsonWriter.Write(request.UserId.ToString());
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
                    "chat",
                    "subscribe",
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
#else
		public async Task<Result.GetSubscribeByUserIdResult> GetSubscribeByUserId(
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


        private class UpdateNotificationTypeTask : Gs2WebSocketSessionTask<Request.UpdateNotificationTypeRequest, Result.UpdateNotificationTypeResult>
        {
	        public UpdateNotificationTypeTask(IGs2Session session, Request.UpdateNotificationTypeRequest request) : base(session, request)
	        {
	        }

            protected override IGs2SessionRequest CreateRequest(Request.UpdateNotificationTypeRequest request)
            {
                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);

                jsonWriter.WriteObjectStart();

                if (request.NamespaceName != null)
                {
                    jsonWriter.WritePropertyName("namespaceName");
                    jsonWriter.Write(request.NamespaceName.ToString());
                }
                if (request.RoomName != null)
                {
                    jsonWriter.WritePropertyName("roomName");
                    jsonWriter.Write(request.RoomName.ToString());
                }
                if (request.AccessToken != null)
                {
                    jsonWriter.WritePropertyName("accessToken");
                    jsonWriter.Write(request.AccessToken.ToString());
                }
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

                AddHeader(
                    Session.Credential,
                    "chat",
                    "subscribe",
                    "updateNotificationType",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator UpdateNotificationType(
                Request.UpdateNotificationTypeRequest request,
                UnityAction<AsyncResult<Result.UpdateNotificationTypeResult>> callback
        )
		{
			var task = new UpdateNotificationTypeTask(
			    Gs2WebSocketSession,
			    request
            );
            yield return task;
            callback.Invoke(new AsyncResult<Result.UpdateNotificationTypeResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.UpdateNotificationTypeResult> UpdateNotificationType(
            Request.UpdateNotificationTypeRequest request
        )
		{
		    var task = new UpdateNotificationTypeTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
#endif


        private class UpdateNotificationTypeByUserIdTask : Gs2WebSocketSessionTask<Request.UpdateNotificationTypeByUserIdRequest, Result.UpdateNotificationTypeByUserIdResult>
        {
	        public UpdateNotificationTypeByUserIdTask(IGs2Session session, Request.UpdateNotificationTypeByUserIdRequest request) : base(session, request)
	        {
	        }

            protected override IGs2SessionRequest CreateRequest(Request.UpdateNotificationTypeByUserIdRequest request)
            {
                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);

                jsonWriter.WriteObjectStart();

                if (request.NamespaceName != null)
                {
                    jsonWriter.WritePropertyName("namespaceName");
                    jsonWriter.Write(request.NamespaceName.ToString());
                }
                if (request.RoomName != null)
                {
                    jsonWriter.WritePropertyName("roomName");
                    jsonWriter.Write(request.RoomName.ToString());
                }
                if (request.UserId != null)
                {
                    jsonWriter.WritePropertyName("userId");
                    jsonWriter.Write(request.UserId.ToString());
                }
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
                if (request.RequestId != null)
                {
                    jsonWriter.WritePropertyName("xGs2RequestId");
                    jsonWriter.Write(request.RequestId);
                }

                AddHeader(
                    Session.Credential,
                    "chat",
                    "subscribe",
                    "updateNotificationTypeByUserId",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator UpdateNotificationTypeByUserId(
                Request.UpdateNotificationTypeByUserIdRequest request,
                UnityAction<AsyncResult<Result.UpdateNotificationTypeByUserIdResult>> callback
        )
		{
			var task = new UpdateNotificationTypeByUserIdTask(
			    Gs2WebSocketSession,
			    request
            );
            yield return task;
            callback.Invoke(new AsyncResult<Result.UpdateNotificationTypeByUserIdResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.UpdateNotificationTypeByUserIdResult> UpdateNotificationTypeByUserId(
            Request.UpdateNotificationTypeByUserIdRequest request
        )
		{
		    var task = new UpdateNotificationTypeByUserIdTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
#endif


        private class UnsubscribeTask : Gs2WebSocketSessionTask<Request.UnsubscribeRequest, Result.UnsubscribeResult>
        {
	        public UnsubscribeTask(IGs2Session session, Request.UnsubscribeRequest request) : base(session, request)
	        {
	        }

            protected override IGs2SessionRequest CreateRequest(Request.UnsubscribeRequest request)
            {
                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);

                jsonWriter.WriteObjectStart();

                if (request.NamespaceName != null)
                {
                    jsonWriter.WritePropertyName("namespaceName");
                    jsonWriter.Write(request.NamespaceName.ToString());
                }
                if (request.RoomName != null)
                {
                    jsonWriter.WritePropertyName("roomName");
                    jsonWriter.Write(request.RoomName.ToString());
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

                AddHeader(
                    Session.Credential,
                    "chat",
                    "subscribe",
                    "unsubscribe",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator Unsubscribe(
                Request.UnsubscribeRequest request,
                UnityAction<AsyncResult<Result.UnsubscribeResult>> callback
        )
		{
			var task = new UnsubscribeTask(
			    Gs2WebSocketSession,
			    request
            );
            yield return task;
            callback.Invoke(new AsyncResult<Result.UnsubscribeResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.UnsubscribeResult> Unsubscribe(
            Request.UnsubscribeRequest request
        )
		{
		    var task = new UnsubscribeTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
#endif


        private class UnsubscribeByUserIdTask : Gs2WebSocketSessionTask<Request.UnsubscribeByUserIdRequest, Result.UnsubscribeByUserIdResult>
        {
	        public UnsubscribeByUserIdTask(IGs2Session session, Request.UnsubscribeByUserIdRequest request) : base(session, request)
	        {
	        }

            protected override IGs2SessionRequest CreateRequest(Request.UnsubscribeByUserIdRequest request)
            {
                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);

                jsonWriter.WriteObjectStart();

                if (request.NamespaceName != null)
                {
                    jsonWriter.WritePropertyName("namespaceName");
                    jsonWriter.Write(request.NamespaceName.ToString());
                }
                if (request.RoomName != null)
                {
                    jsonWriter.WritePropertyName("roomName");
                    jsonWriter.Write(request.RoomName.ToString());
                }
                if (request.UserId != null)
                {
                    jsonWriter.WritePropertyName("userId");
                    jsonWriter.Write(request.UserId.ToString());
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
                    "chat",
                    "subscribe",
                    "unsubscribeByUserId",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator UnsubscribeByUserId(
                Request.UnsubscribeByUserIdRequest request,
                UnityAction<AsyncResult<Result.UnsubscribeByUserIdResult>> callback
        )
		{
			var task = new UnsubscribeByUserIdTask(
			    Gs2WebSocketSession,
			    request
            );
            yield return task;
            callback.Invoke(new AsyncResult<Result.UnsubscribeByUserIdResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.UnsubscribeByUserIdResult> UnsubscribeByUserId(
            Request.UnsubscribeByUserIdRequest request
        )
		{
		    var task = new UnsubscribeByUserIdTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
#endif
	}
}