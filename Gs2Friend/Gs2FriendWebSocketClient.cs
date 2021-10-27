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

namespace Gs2.Gs2Friend
{
	public class Gs2FriendWebSocketClient : AbstractGs2Client
	{

		public static string Endpoint = "friend";

        protected Gs2WebSocketSession Gs2WebSocketSession => (Gs2WebSocketSession) Gs2Session;

		public Gs2FriendWebSocketClient(Gs2WebSocketSession Gs2WebSocketSession) : base(Gs2WebSocketSession)
		{

		}


        private class GetProfileTask : Gs2WebSocketSessionTask<Request.GetProfileRequest, Result.GetProfileResult>
        {
	        public GetProfileTask(IGs2Session session, Request.GetProfileRequest request) : base(session, request)
	        {
	        }

            protected override IGs2SessionRequest CreateRequest(Request.GetProfileRequest request)
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
                    "friend",
                    "profile",
                    "getProfile",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator GetProfile(
                Request.GetProfileRequest request,
                UnityAction<AsyncResult<Result.GetProfileResult>> callback
        )
		{
			var task = new GetProfileTask(
			    Gs2WebSocketSession,
			    request
            );
            yield return task;
            callback.Invoke(new AsyncResult<Result.GetProfileResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.GetProfileResult> GetProfile(
            Request.GetProfileRequest request
        )
		{
		    var task = new GetProfileTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
#endif


        private class GetProfileByUserIdTask : Gs2WebSocketSessionTask<Request.GetProfileByUserIdRequest, Result.GetProfileByUserIdResult>
        {
	        public GetProfileByUserIdTask(IGs2Session session, Request.GetProfileByUserIdRequest request) : base(session, request)
	        {
	        }

            protected override IGs2SessionRequest CreateRequest(Request.GetProfileByUserIdRequest request)
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
                    "friend",
                    "profile",
                    "getProfileByUserId",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator GetProfileByUserId(
                Request.GetProfileByUserIdRequest request,
                UnityAction<AsyncResult<Result.GetProfileByUserIdResult>> callback
        )
		{
			var task = new GetProfileByUserIdTask(
			    Gs2WebSocketSession,
			    request
            );
            yield return task;
            callback.Invoke(new AsyncResult<Result.GetProfileByUserIdResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.GetProfileByUserIdResult> GetProfileByUserId(
            Request.GetProfileByUserIdRequest request
        )
		{
		    var task = new GetProfileByUserIdTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
#endif


        private class UpdateProfileTask : Gs2WebSocketSessionTask<Request.UpdateProfileRequest, Result.UpdateProfileResult>
        {
	        public UpdateProfileTask(IGs2Session session, Request.UpdateProfileRequest request) : base(session, request)
	        {
	        }

            protected override IGs2SessionRequest CreateRequest(Request.UpdateProfileRequest request)
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
                if (request.PublicProfile != null)
                {
                    jsonWriter.WritePropertyName("publicProfile");
                    jsonWriter.Write(request.PublicProfile.ToString());
                }
                if (request.FollowerProfile != null)
                {
                    jsonWriter.WritePropertyName("followerProfile");
                    jsonWriter.Write(request.FollowerProfile.ToString());
                }
                if (request.FriendProfile != null)
                {
                    jsonWriter.WritePropertyName("friendProfile");
                    jsonWriter.Write(request.FriendProfile.ToString());
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
                    "friend",
                    "profile",
                    "updateProfile",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator UpdateProfile(
                Request.UpdateProfileRequest request,
                UnityAction<AsyncResult<Result.UpdateProfileResult>> callback
        )
		{
			var task = new UpdateProfileTask(
			    Gs2WebSocketSession,
			    request
            );
            yield return task;
            callback.Invoke(new AsyncResult<Result.UpdateProfileResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.UpdateProfileResult> UpdateProfile(
            Request.UpdateProfileRequest request
        )
		{
		    var task = new UpdateProfileTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
#endif


        private class UpdateProfileByUserIdTask : Gs2WebSocketSessionTask<Request.UpdateProfileByUserIdRequest, Result.UpdateProfileByUserIdResult>
        {
	        public UpdateProfileByUserIdTask(IGs2Session session, Request.UpdateProfileByUserIdRequest request) : base(session, request)
	        {
	        }

            protected override IGs2SessionRequest CreateRequest(Request.UpdateProfileByUserIdRequest request)
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
                if (request.PublicProfile != null)
                {
                    jsonWriter.WritePropertyName("publicProfile");
                    jsonWriter.Write(request.PublicProfile.ToString());
                }
                if (request.FollowerProfile != null)
                {
                    jsonWriter.WritePropertyName("followerProfile");
                    jsonWriter.Write(request.FollowerProfile.ToString());
                }
                if (request.FriendProfile != null)
                {
                    jsonWriter.WritePropertyName("friendProfile");
                    jsonWriter.Write(request.FriendProfile.ToString());
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
                    "friend",
                    "profile",
                    "updateProfileByUserId",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator UpdateProfileByUserId(
                Request.UpdateProfileByUserIdRequest request,
                UnityAction<AsyncResult<Result.UpdateProfileByUserIdResult>> callback
        )
		{
			var task = new UpdateProfileByUserIdTask(
			    Gs2WebSocketSession,
			    request
            );
            yield return task;
            callback.Invoke(new AsyncResult<Result.UpdateProfileByUserIdResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.UpdateProfileByUserIdResult> UpdateProfileByUserId(
            Request.UpdateProfileByUserIdRequest request
        )
		{
		    var task = new UpdateProfileByUserIdTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
#endif


        private class DeleteProfileByUserIdTask : Gs2WebSocketSessionTask<Request.DeleteProfileByUserIdRequest, Result.DeleteProfileByUserIdResult>
        {
	        public DeleteProfileByUserIdTask(IGs2Session session, Request.DeleteProfileByUserIdRequest request) : base(session, request)
	        {
	        }

            protected override IGs2SessionRequest CreateRequest(Request.DeleteProfileByUserIdRequest request)
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
                    "friend",
                    "profile",
                    "deleteProfileByUserId",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator DeleteProfileByUserId(
                Request.DeleteProfileByUserIdRequest request,
                UnityAction<AsyncResult<Result.DeleteProfileByUserIdResult>> callback
        )
		{
			var task = new DeleteProfileByUserIdTask(
			    Gs2WebSocketSession,
			    request
            );
            yield return task;
            callback.Invoke(new AsyncResult<Result.DeleteProfileByUserIdResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.DeleteProfileByUserIdResult> DeleteProfileByUserId(
            Request.DeleteProfileByUserIdRequest request
        )
		{
		    var task = new DeleteProfileByUserIdTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
#endif


        private class GetPublicProfileTask : Gs2WebSocketSessionTask<Request.GetPublicProfileRequest, Result.GetPublicProfileResult>
        {
	        public GetPublicProfileTask(IGs2Session session, Request.GetPublicProfileRequest request) : base(session, request)
	        {
	        }

            protected override IGs2SessionRequest CreateRequest(Request.GetPublicProfileRequest request)
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
                    "friend",
                    "profile",
                    "getPublicProfile",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator GetPublicProfile(
                Request.GetPublicProfileRequest request,
                UnityAction<AsyncResult<Result.GetPublicProfileResult>> callback
        )
		{
			var task = new GetPublicProfileTask(
			    Gs2WebSocketSession,
			    request
            );
            yield return task;
            callback.Invoke(new AsyncResult<Result.GetPublicProfileResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.GetPublicProfileResult> GetPublicProfile(
            Request.GetPublicProfileRequest request
        )
		{
		    var task = new GetPublicProfileTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
#endif


        private class GetFollowTask : Gs2WebSocketSessionTask<Request.GetFollowRequest, Result.GetFollowResult>
        {
	        public GetFollowTask(IGs2Session session, Request.GetFollowRequest request) : base(session, request)
	        {
	        }

            protected override IGs2SessionRequest CreateRequest(Request.GetFollowRequest request)
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
                if (request.TargetUserId != null)
                {
                    jsonWriter.WritePropertyName("targetUserId");
                    jsonWriter.Write(request.TargetUserId.ToString());
                }
                if (request.WithProfile != null)
                {
                    jsonWriter.WritePropertyName("withProfile");
                    jsonWriter.Write(request.WithProfile.ToString());
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
                    "friend",
                    "follow",
                    "getFollow",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator GetFollow(
                Request.GetFollowRequest request,
                UnityAction<AsyncResult<Result.GetFollowResult>> callback
        )
		{
			var task = new GetFollowTask(
			    Gs2WebSocketSession,
			    request
            );
            yield return task;
            callback.Invoke(new AsyncResult<Result.GetFollowResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.GetFollowResult> GetFollow(
            Request.GetFollowRequest request
        )
		{
		    var task = new GetFollowTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
#endif


        private class GetFollowByUserIdTask : Gs2WebSocketSessionTask<Request.GetFollowByUserIdRequest, Result.GetFollowByUserIdResult>
        {
	        public GetFollowByUserIdTask(IGs2Session session, Request.GetFollowByUserIdRequest request) : base(session, request)
	        {
	        }

            protected override IGs2SessionRequest CreateRequest(Request.GetFollowByUserIdRequest request)
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
                if (request.TargetUserId != null)
                {
                    jsonWriter.WritePropertyName("targetUserId");
                    jsonWriter.Write(request.TargetUserId.ToString());
                }
                if (request.WithProfile != null)
                {
                    jsonWriter.WritePropertyName("withProfile");
                    jsonWriter.Write(request.WithProfile.ToString());
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
                    "friend",
                    "follow",
                    "getFollowByUserId",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator GetFollowByUserId(
                Request.GetFollowByUserIdRequest request,
                UnityAction<AsyncResult<Result.GetFollowByUserIdResult>> callback
        )
		{
			var task = new GetFollowByUserIdTask(
			    Gs2WebSocketSession,
			    request
            );
            yield return task;
            callback.Invoke(new AsyncResult<Result.GetFollowByUserIdResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.GetFollowByUserIdResult> GetFollowByUserId(
            Request.GetFollowByUserIdRequest request
        )
		{
		    var task = new GetFollowByUserIdTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
#endif


        private class FollowTask : Gs2WebSocketSessionTask<Request.FollowRequest, Result.FollowResult>
        {
	        public FollowTask(IGs2Session session, Request.FollowRequest request) : base(session, request)
	        {
	        }

            protected override IGs2SessionRequest CreateRequest(Request.FollowRequest request)
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
                    "friend",
                    "follow",
                    "follow",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator Follow(
                Request.FollowRequest request,
                UnityAction<AsyncResult<Result.FollowResult>> callback
        )
		{
			var task = new FollowTask(
			    Gs2WebSocketSession,
			    request
            );
            yield return task;
            callback.Invoke(new AsyncResult<Result.FollowResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.FollowResult> Follow(
            Request.FollowRequest request
        )
		{
		    var task = new FollowTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
#endif


        private class FollowByUserIdTask : Gs2WebSocketSessionTask<Request.FollowByUserIdRequest, Result.FollowByUserIdResult>
        {
	        public FollowByUserIdTask(IGs2Session session, Request.FollowByUserIdRequest request) : base(session, request)
	        {
	        }

            protected override IGs2SessionRequest CreateRequest(Request.FollowByUserIdRequest request)
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
                if (request.RequestId != null)
                {
                    jsonWriter.WritePropertyName("xGs2RequestId");
                    jsonWriter.Write(request.RequestId);
                }

                AddHeader(
                    Session.Credential,
                    "friend",
                    "follow",
                    "followByUserId",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator FollowByUserId(
                Request.FollowByUserIdRequest request,
                UnityAction<AsyncResult<Result.FollowByUserIdResult>> callback
        )
		{
			var task = new FollowByUserIdTask(
			    Gs2WebSocketSession,
			    request
            );
            yield return task;
            callback.Invoke(new AsyncResult<Result.FollowByUserIdResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.FollowByUserIdResult> FollowByUserId(
            Request.FollowByUserIdRequest request
        )
		{
		    var task = new FollowByUserIdTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
#endif


        private class UnfollowTask : Gs2WebSocketSessionTask<Request.UnfollowRequest, Result.UnfollowResult>
        {
	        public UnfollowTask(IGs2Session session, Request.UnfollowRequest request) : base(session, request)
	        {
	        }

            protected override IGs2SessionRequest CreateRequest(Request.UnfollowRequest request)
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
                    "friend",
                    "follow",
                    "unfollow",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator Unfollow(
                Request.UnfollowRequest request,
                UnityAction<AsyncResult<Result.UnfollowResult>> callback
        )
		{
			var task = new UnfollowTask(
			    Gs2WebSocketSession,
			    request
            );
            yield return task;
            callback.Invoke(new AsyncResult<Result.UnfollowResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.UnfollowResult> Unfollow(
            Request.UnfollowRequest request
        )
		{
		    var task = new UnfollowTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
#endif


        private class UnfollowByUserIdTask : Gs2WebSocketSessionTask<Request.UnfollowByUserIdRequest, Result.UnfollowByUserIdResult>
        {
	        public UnfollowByUserIdTask(IGs2Session session, Request.UnfollowByUserIdRequest request) : base(session, request)
	        {
	        }

            protected override IGs2SessionRequest CreateRequest(Request.UnfollowByUserIdRequest request)
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
                if (request.RequestId != null)
                {
                    jsonWriter.WritePropertyName("xGs2RequestId");
                    jsonWriter.Write(request.RequestId);
                }

                AddHeader(
                    Session.Credential,
                    "friend",
                    "follow",
                    "unfollowByUserId",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator UnfollowByUserId(
                Request.UnfollowByUserIdRequest request,
                UnityAction<AsyncResult<Result.UnfollowByUserIdResult>> callback
        )
		{
			var task = new UnfollowByUserIdTask(
			    Gs2WebSocketSession,
			    request
            );
            yield return task;
            callback.Invoke(new AsyncResult<Result.UnfollowByUserIdResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.UnfollowByUserIdResult> UnfollowByUserId(
            Request.UnfollowByUserIdRequest request
        )
		{
		    var task = new UnfollowByUserIdTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
#endif


        private class GetFriendTask : Gs2WebSocketSessionTask<Request.GetFriendRequest, Result.GetFriendResult>
        {
	        public GetFriendTask(IGs2Session session, Request.GetFriendRequest request) : base(session, request)
	        {
	        }

            protected override IGs2SessionRequest CreateRequest(Request.GetFriendRequest request)
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
                if (request.TargetUserId != null)
                {
                    jsonWriter.WritePropertyName("targetUserId");
                    jsonWriter.Write(request.TargetUserId.ToString());
                }
                if (request.WithProfile != null)
                {
                    jsonWriter.WritePropertyName("withProfile");
                    jsonWriter.Write(request.WithProfile.ToString());
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
                    "friend",
                    "friend",
                    "getFriend",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator GetFriend(
                Request.GetFriendRequest request,
                UnityAction<AsyncResult<Result.GetFriendResult>> callback
        )
		{
			var task = new GetFriendTask(
			    Gs2WebSocketSession,
			    request
            );
            yield return task;
            callback.Invoke(new AsyncResult<Result.GetFriendResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.GetFriendResult> GetFriend(
            Request.GetFriendRequest request
        )
		{
		    var task = new GetFriendTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
#endif


        private class GetFriendByUserIdTask : Gs2WebSocketSessionTask<Request.GetFriendByUserIdRequest, Result.GetFriendByUserIdResult>
        {
	        public GetFriendByUserIdTask(IGs2Session session, Request.GetFriendByUserIdRequest request) : base(session, request)
	        {
	        }

            protected override IGs2SessionRequest CreateRequest(Request.GetFriendByUserIdRequest request)
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
                if (request.TargetUserId != null)
                {
                    jsonWriter.WritePropertyName("targetUserId");
                    jsonWriter.Write(request.TargetUserId.ToString());
                }
                if (request.WithProfile != null)
                {
                    jsonWriter.WritePropertyName("withProfile");
                    jsonWriter.Write(request.WithProfile.ToString());
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
                    "friend",
                    "friend",
                    "getFriendByUserId",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator GetFriendByUserId(
                Request.GetFriendByUserIdRequest request,
                UnityAction<AsyncResult<Result.GetFriendByUserIdResult>> callback
        )
		{
			var task = new GetFriendByUserIdTask(
			    Gs2WebSocketSession,
			    request
            );
            yield return task;
            callback.Invoke(new AsyncResult<Result.GetFriendByUserIdResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.GetFriendByUserIdResult> GetFriendByUserId(
            Request.GetFriendByUserIdRequest request
        )
		{
		    var task = new GetFriendByUserIdTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
#endif


        private class DeleteFriendTask : Gs2WebSocketSessionTask<Request.DeleteFriendRequest, Result.DeleteFriendResult>
        {
	        public DeleteFriendTask(IGs2Session session, Request.DeleteFriendRequest request) : base(session, request)
	        {
	        }

            protected override IGs2SessionRequest CreateRequest(Request.DeleteFriendRequest request)
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
                    "friend",
                    "friend",
                    "deleteFriend",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator DeleteFriend(
                Request.DeleteFriendRequest request,
                UnityAction<AsyncResult<Result.DeleteFriendResult>> callback
        )
		{
			var task = new DeleteFriendTask(
			    Gs2WebSocketSession,
			    request
            );
            yield return task;
            callback.Invoke(new AsyncResult<Result.DeleteFriendResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.DeleteFriendResult> DeleteFriend(
            Request.DeleteFriendRequest request
        )
		{
		    var task = new DeleteFriendTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
#endif


        private class DeleteFriendByUserIdTask : Gs2WebSocketSessionTask<Request.DeleteFriendByUserIdRequest, Result.DeleteFriendByUserIdResult>
        {
	        public DeleteFriendByUserIdTask(IGs2Session session, Request.DeleteFriendByUserIdRequest request) : base(session, request)
	        {
	        }

            protected override IGs2SessionRequest CreateRequest(Request.DeleteFriendByUserIdRequest request)
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
                if (request.RequestId != null)
                {
                    jsonWriter.WritePropertyName("xGs2RequestId");
                    jsonWriter.Write(request.RequestId);
                }

                AddHeader(
                    Session.Credential,
                    "friend",
                    "friend",
                    "deleteFriendByUserId",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator DeleteFriendByUserId(
                Request.DeleteFriendByUserIdRequest request,
                UnityAction<AsyncResult<Result.DeleteFriendByUserIdResult>> callback
        )
		{
			var task = new DeleteFriendByUserIdTask(
			    Gs2WebSocketSession,
			    request
            );
            yield return task;
            callback.Invoke(new AsyncResult<Result.DeleteFriendByUserIdResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.DeleteFriendByUserIdResult> DeleteFriendByUserId(
            Request.DeleteFriendByUserIdRequest request
        )
		{
		    var task = new DeleteFriendByUserIdTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
#endif


        private class GetSendRequestTask : Gs2WebSocketSessionTask<Request.GetSendRequestRequest, Result.GetSendRequestResult>
        {
	        public GetSendRequestTask(IGs2Session session, Request.GetSendRequestRequest request) : base(session, request)
	        {
	        }

            protected override IGs2SessionRequest CreateRequest(Request.GetSendRequestRequest request)
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
                    "friend",
                    "sendBox",
                    "getSendRequest",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator GetSendRequest(
                Request.GetSendRequestRequest request,
                UnityAction<AsyncResult<Result.GetSendRequestResult>> callback
        )
		{
			var task = new GetSendRequestTask(
			    Gs2WebSocketSession,
			    request
            );
            yield return task;
            callback.Invoke(new AsyncResult<Result.GetSendRequestResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.GetSendRequestResult> GetSendRequest(
            Request.GetSendRequestRequest request
        )
		{
		    var task = new GetSendRequestTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
#endif


        private class GetSendRequestByUserIdTask : Gs2WebSocketSessionTask<Request.GetSendRequestByUserIdRequest, Result.GetSendRequestByUserIdResult>
        {
	        public GetSendRequestByUserIdTask(IGs2Session session, Request.GetSendRequestByUserIdRequest request) : base(session, request)
	        {
	        }

            protected override IGs2SessionRequest CreateRequest(Request.GetSendRequestByUserIdRequest request)
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
                if (request.RequestId != null)
                {
                    jsonWriter.WritePropertyName("xGs2RequestId");
                    jsonWriter.Write(request.RequestId);
                }

                AddHeader(
                    Session.Credential,
                    "friend",
                    "sendBox",
                    "getSendRequestByUserId",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator GetSendRequestByUserId(
                Request.GetSendRequestByUserIdRequest request,
                UnityAction<AsyncResult<Result.GetSendRequestByUserIdResult>> callback
        )
		{
			var task = new GetSendRequestByUserIdTask(
			    Gs2WebSocketSession,
			    request
            );
            yield return task;
            callback.Invoke(new AsyncResult<Result.GetSendRequestByUserIdResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.GetSendRequestByUserIdResult> GetSendRequestByUserId(
            Request.GetSendRequestByUserIdRequest request
        )
		{
		    var task = new GetSendRequestByUserIdTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
#endif


        private class SendRequestTask : Gs2WebSocketSessionTask<Request.SendRequestRequest, Result.SendRequestResult>
        {
	        public SendRequestTask(IGs2Session session, Request.SendRequestRequest request) : base(session, request)
	        {
	        }

            protected override IGs2SessionRequest CreateRequest(Request.SendRequestRequest request)
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
                    "friend",
                    "sendBox",
                    "sendRequest",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator SendRequest(
                Request.SendRequestRequest request,
                UnityAction<AsyncResult<Result.SendRequestResult>> callback
        )
		{
			var task = new SendRequestTask(
			    Gs2WebSocketSession,
			    request
            );
            yield return task;
            callback.Invoke(new AsyncResult<Result.SendRequestResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.SendRequestResult> SendRequest(
            Request.SendRequestRequest request
        )
		{
		    var task = new SendRequestTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
#endif


        private class SendRequestByUserIdTask : Gs2WebSocketSessionTask<Request.SendRequestByUserIdRequest, Result.SendRequestByUserIdResult>
        {
	        public SendRequestByUserIdTask(IGs2Session session, Request.SendRequestByUserIdRequest request) : base(session, request)
	        {
	        }

            protected override IGs2SessionRequest CreateRequest(Request.SendRequestByUserIdRequest request)
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
                if (request.RequestId != null)
                {
                    jsonWriter.WritePropertyName("xGs2RequestId");
                    jsonWriter.Write(request.RequestId);
                }

                AddHeader(
                    Session.Credential,
                    "friend",
                    "sendBox",
                    "sendRequestByUserId",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator SendRequestByUserId(
                Request.SendRequestByUserIdRequest request,
                UnityAction<AsyncResult<Result.SendRequestByUserIdResult>> callback
        )
		{
			var task = new SendRequestByUserIdTask(
			    Gs2WebSocketSession,
			    request
            );
            yield return task;
            callback.Invoke(new AsyncResult<Result.SendRequestByUserIdResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.SendRequestByUserIdResult> SendRequestByUserId(
            Request.SendRequestByUserIdRequest request
        )
		{
		    var task = new SendRequestByUserIdTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
#endif


        private class DeleteRequestTask : Gs2WebSocketSessionTask<Request.DeleteRequestRequest, Result.DeleteRequestResult>
        {
	        public DeleteRequestTask(IGs2Session session, Request.DeleteRequestRequest request) : base(session, request)
	        {
	        }

            protected override IGs2SessionRequest CreateRequest(Request.DeleteRequestRequest request)
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
                    "friend",
                    "sendBox",
                    "deleteRequest",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator DeleteRequest(
                Request.DeleteRequestRequest request,
                UnityAction<AsyncResult<Result.DeleteRequestResult>> callback
        )
		{
			var task = new DeleteRequestTask(
			    Gs2WebSocketSession,
			    request
            );
            yield return task;
            callback.Invoke(new AsyncResult<Result.DeleteRequestResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.DeleteRequestResult> DeleteRequest(
            Request.DeleteRequestRequest request
        )
		{
		    var task = new DeleteRequestTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
#endif


        private class DeleteRequestByUserIdTask : Gs2WebSocketSessionTask<Request.DeleteRequestByUserIdRequest, Result.DeleteRequestByUserIdResult>
        {
	        public DeleteRequestByUserIdTask(IGs2Session session, Request.DeleteRequestByUserIdRequest request) : base(session, request)
	        {
	        }

            protected override IGs2SessionRequest CreateRequest(Request.DeleteRequestByUserIdRequest request)
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
                if (request.RequestId != null)
                {
                    jsonWriter.WritePropertyName("xGs2RequestId");
                    jsonWriter.Write(request.RequestId);
                }

                AddHeader(
                    Session.Credential,
                    "friend",
                    "sendBox",
                    "deleteRequestByUserId",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator DeleteRequestByUserId(
                Request.DeleteRequestByUserIdRequest request,
                UnityAction<AsyncResult<Result.DeleteRequestByUserIdResult>> callback
        )
		{
			var task = new DeleteRequestByUserIdTask(
			    Gs2WebSocketSession,
			    request
            );
            yield return task;
            callback.Invoke(new AsyncResult<Result.DeleteRequestByUserIdResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.DeleteRequestByUserIdResult> DeleteRequestByUserId(
            Request.DeleteRequestByUserIdRequest request
        )
		{
		    var task = new DeleteRequestByUserIdTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
#endif


        private class GetReceiveRequestTask : Gs2WebSocketSessionTask<Request.GetReceiveRequestRequest, Result.GetReceiveRequestResult>
        {
	        public GetReceiveRequestTask(IGs2Session session, Request.GetReceiveRequestRequest request) : base(session, request)
	        {
	        }

            protected override IGs2SessionRequest CreateRequest(Request.GetReceiveRequestRequest request)
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
                if (request.FromUserId != null)
                {
                    jsonWriter.WritePropertyName("fromUserId");
                    jsonWriter.Write(request.FromUserId.ToString());
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
                    "friend",
                    "inbox",
                    "getReceiveRequest",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator GetReceiveRequest(
                Request.GetReceiveRequestRequest request,
                UnityAction<AsyncResult<Result.GetReceiveRequestResult>> callback
        )
		{
			var task = new GetReceiveRequestTask(
			    Gs2WebSocketSession,
			    request
            );
            yield return task;
            callback.Invoke(new AsyncResult<Result.GetReceiveRequestResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.GetReceiveRequestResult> GetReceiveRequest(
            Request.GetReceiveRequestRequest request
        )
		{
		    var task = new GetReceiveRequestTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
#endif


        private class GetReceiveRequestByUserIdTask : Gs2WebSocketSessionTask<Request.GetReceiveRequestByUserIdRequest, Result.GetReceiveRequestByUserIdResult>
        {
	        public GetReceiveRequestByUserIdTask(IGs2Session session, Request.GetReceiveRequestByUserIdRequest request) : base(session, request)
	        {
	        }

            protected override IGs2SessionRequest CreateRequest(Request.GetReceiveRequestByUserIdRequest request)
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
                if (request.FromUserId != null)
                {
                    jsonWriter.WritePropertyName("fromUserId");
                    jsonWriter.Write(request.FromUserId.ToString());
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
                    "friend",
                    "inbox",
                    "getReceiveRequestByUserId",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator GetReceiveRequestByUserId(
                Request.GetReceiveRequestByUserIdRequest request,
                UnityAction<AsyncResult<Result.GetReceiveRequestByUserIdResult>> callback
        )
		{
			var task = new GetReceiveRequestByUserIdTask(
			    Gs2WebSocketSession,
			    request
            );
            yield return task;
            callback.Invoke(new AsyncResult<Result.GetReceiveRequestByUserIdResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.GetReceiveRequestByUserIdResult> GetReceiveRequestByUserId(
            Request.GetReceiveRequestByUserIdRequest request
        )
		{
		    var task = new GetReceiveRequestByUserIdTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
#endif


        private class AcceptRequestTask : Gs2WebSocketSessionTask<Request.AcceptRequestRequest, Result.AcceptRequestResult>
        {
	        public AcceptRequestTask(IGs2Session session, Request.AcceptRequestRequest request) : base(session, request)
	        {
	        }

            protected override IGs2SessionRequest CreateRequest(Request.AcceptRequestRequest request)
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
                if (request.FromUserId != null)
                {
                    jsonWriter.WritePropertyName("fromUserId");
                    jsonWriter.Write(request.FromUserId.ToString());
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
                    "friend",
                    "inbox",
                    "acceptRequest",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator AcceptRequest(
                Request.AcceptRequestRequest request,
                UnityAction<AsyncResult<Result.AcceptRequestResult>> callback
        )
		{
			var task = new AcceptRequestTask(
			    Gs2WebSocketSession,
			    request
            );
            yield return task;
            callback.Invoke(new AsyncResult<Result.AcceptRequestResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.AcceptRequestResult> AcceptRequest(
            Request.AcceptRequestRequest request
        )
		{
		    var task = new AcceptRequestTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
#endif


        private class AcceptRequestByUserIdTask : Gs2WebSocketSessionTask<Request.AcceptRequestByUserIdRequest, Result.AcceptRequestByUserIdResult>
        {
	        public AcceptRequestByUserIdTask(IGs2Session session, Request.AcceptRequestByUserIdRequest request) : base(session, request)
	        {
	        }

            protected override IGs2SessionRequest CreateRequest(Request.AcceptRequestByUserIdRequest request)
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
                if (request.FromUserId != null)
                {
                    jsonWriter.WritePropertyName("fromUserId");
                    jsonWriter.Write(request.FromUserId.ToString());
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
                    "friend",
                    "inbox",
                    "acceptRequestByUserId",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator AcceptRequestByUserId(
                Request.AcceptRequestByUserIdRequest request,
                UnityAction<AsyncResult<Result.AcceptRequestByUserIdResult>> callback
        )
		{
			var task = new AcceptRequestByUserIdTask(
			    Gs2WebSocketSession,
			    request
            );
            yield return task;
            callback.Invoke(new AsyncResult<Result.AcceptRequestByUserIdResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.AcceptRequestByUserIdResult> AcceptRequestByUserId(
            Request.AcceptRequestByUserIdRequest request
        )
		{
		    var task = new AcceptRequestByUserIdTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
#endif


        private class RejectRequestTask : Gs2WebSocketSessionTask<Request.RejectRequestRequest, Result.RejectRequestResult>
        {
	        public RejectRequestTask(IGs2Session session, Request.RejectRequestRequest request) : base(session, request)
	        {
	        }

            protected override IGs2SessionRequest CreateRequest(Request.RejectRequestRequest request)
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
                if (request.FromUserId != null)
                {
                    jsonWriter.WritePropertyName("fromUserId");
                    jsonWriter.Write(request.FromUserId.ToString());
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
                    "friend",
                    "inbox",
                    "rejectRequest",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator RejectRequest(
                Request.RejectRequestRequest request,
                UnityAction<AsyncResult<Result.RejectRequestResult>> callback
        )
		{
			var task = new RejectRequestTask(
			    Gs2WebSocketSession,
			    request
            );
            yield return task;
            callback.Invoke(new AsyncResult<Result.RejectRequestResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.RejectRequestResult> RejectRequest(
            Request.RejectRequestRequest request
        )
		{
		    var task = new RejectRequestTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
#endif


        private class RejectRequestByUserIdTask : Gs2WebSocketSessionTask<Request.RejectRequestByUserIdRequest, Result.RejectRequestByUserIdResult>
        {
	        public RejectRequestByUserIdTask(IGs2Session session, Request.RejectRequestByUserIdRequest request) : base(session, request)
	        {
	        }

            protected override IGs2SessionRequest CreateRequest(Request.RejectRequestByUserIdRequest request)
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
                if (request.FromUserId != null)
                {
                    jsonWriter.WritePropertyName("fromUserId");
                    jsonWriter.Write(request.FromUserId.ToString());
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
                    "friend",
                    "inbox",
                    "rejectRequestByUserId",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator RejectRequestByUserId(
                Request.RejectRequestByUserIdRequest request,
                UnityAction<AsyncResult<Result.RejectRequestByUserIdResult>> callback
        )
		{
			var task = new RejectRequestByUserIdTask(
			    Gs2WebSocketSession,
			    request
            );
            yield return task;
            callback.Invoke(new AsyncResult<Result.RejectRequestByUserIdResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.RejectRequestByUserIdResult> RejectRequestByUserId(
            Request.RejectRequestByUserIdRequest request
        )
		{
		    var task = new RejectRequestByUserIdTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
#endif
	}
}