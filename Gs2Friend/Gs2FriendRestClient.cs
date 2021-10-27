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
using Gs2.Gs2Friend.Request;
using Gs2.Gs2Friend.Result;
using Gs2.Util.LitJson;

namespace Gs2.Gs2Friend
{
	public class Gs2FriendRestClient : AbstractGs2Client
	{
#if UNITY_2017_1_OR_NEWER
		private readonly CertificateHandler _certificateHandler;
#endif

		public static string Endpoint = "friend";

        protected Gs2RestSession Gs2RestSession => (Gs2RestSession) Gs2Session;

		public Gs2FriendRestClient(Gs2RestSession Gs2RestSession) : base(Gs2RestSession)
		{

		}

#if UNITY_2017_1_OR_NEWER
		public Gs2FriendRestClient(Gs2RestSession gs2RestSession, CertificateHandler certificateHandler) : base(gs2RestSession)
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
                    .Replace("{service}", "friend")
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
                    .Replace("{service}", "friend")
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
                if (request.FollowScript != null)
                {
                    jsonWriter.WritePropertyName("followScript");
                    request.FollowScript.WriteJson(jsonWriter);
                }
                if (request.UnfollowScript != null)
                {
                    jsonWriter.WritePropertyName("unfollowScript");
                    request.UnfollowScript.WriteJson(jsonWriter);
                }
                if (request.SendRequestScript != null)
                {
                    jsonWriter.WritePropertyName("sendRequestScript");
                    request.SendRequestScript.WriteJson(jsonWriter);
                }
                if (request.CancelRequestScript != null)
                {
                    jsonWriter.WritePropertyName("cancelRequestScript");
                    request.CancelRequestScript.WriteJson(jsonWriter);
                }
                if (request.AcceptRequestScript != null)
                {
                    jsonWriter.WritePropertyName("acceptRequestScript");
                    request.AcceptRequestScript.WriteJson(jsonWriter);
                }
                if (request.RejectRequestScript != null)
                {
                    jsonWriter.WritePropertyName("rejectRequestScript");
                    request.RejectRequestScript.WriteJson(jsonWriter);
                }
                if (request.DeleteFriendScript != null)
                {
                    jsonWriter.WritePropertyName("deleteFriendScript");
                    request.DeleteFriendScript.WriteJson(jsonWriter);
                }
                if (request.UpdateProfileScript != null)
                {
                    jsonWriter.WritePropertyName("updateProfileScript");
                    request.UpdateProfileScript.WriteJson(jsonWriter);
                }
                if (request.FollowNotification != null)
                {
                    jsonWriter.WritePropertyName("followNotification");
                    request.FollowNotification.WriteJson(jsonWriter);
                }
                if (request.ReceiveRequestNotification != null)
                {
                    jsonWriter.WritePropertyName("receiveRequestNotification");
                    request.ReceiveRequestNotification.WriteJson(jsonWriter);
                }
                if (request.AcceptRequestNotification != null)
                {
                    jsonWriter.WritePropertyName("acceptRequestNotification");
                    request.AcceptRequestNotification.WriteJson(jsonWriter);
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
                    .Replace("{service}", "friend")
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
                    .Replace("{service}", "friend")
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
                    .Replace("{service}", "friend")
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
                if (request.FollowScript != null)
                {
                    jsonWriter.WritePropertyName("followScript");
                    request.FollowScript.WriteJson(jsonWriter);
                }
                if (request.UnfollowScript != null)
                {
                    jsonWriter.WritePropertyName("unfollowScript");
                    request.UnfollowScript.WriteJson(jsonWriter);
                }
                if (request.SendRequestScript != null)
                {
                    jsonWriter.WritePropertyName("sendRequestScript");
                    request.SendRequestScript.WriteJson(jsonWriter);
                }
                if (request.CancelRequestScript != null)
                {
                    jsonWriter.WritePropertyName("cancelRequestScript");
                    request.CancelRequestScript.WriteJson(jsonWriter);
                }
                if (request.AcceptRequestScript != null)
                {
                    jsonWriter.WritePropertyName("acceptRequestScript");
                    request.AcceptRequestScript.WriteJson(jsonWriter);
                }
                if (request.RejectRequestScript != null)
                {
                    jsonWriter.WritePropertyName("rejectRequestScript");
                    request.RejectRequestScript.WriteJson(jsonWriter);
                }
                if (request.DeleteFriendScript != null)
                {
                    jsonWriter.WritePropertyName("deleteFriendScript");
                    request.DeleteFriendScript.WriteJson(jsonWriter);
                }
                if (request.UpdateProfileScript != null)
                {
                    jsonWriter.WritePropertyName("updateProfileScript");
                    request.UpdateProfileScript.WriteJson(jsonWriter);
                }
                if (request.FollowNotification != null)
                {
                    jsonWriter.WritePropertyName("followNotification");
                    request.FollowNotification.WriteJson(jsonWriter);
                }
                if (request.ReceiveRequestNotification != null)
                {
                    jsonWriter.WritePropertyName("receiveRequestNotification");
                    request.ReceiveRequestNotification.WriteJson(jsonWriter);
                }
                if (request.AcceptRequestNotification != null)
                {
                    jsonWriter.WritePropertyName("acceptRequestNotification");
                    request.AcceptRequestNotification.WriteJson(jsonWriter);
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
                    .Replace("{service}", "friend")
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


        private class GetProfileTask : Gs2RestSessionTask<GetProfileRequest, GetProfileResult>
        {
            public GetProfileTask(IGs2Session session, RestSessionRequestFactory factory, GetProfileRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(GetProfileRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "friend")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/me/profile";

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
		public IEnumerator GetProfile(
                Request.GetProfileRequest request,
                UnityAction<AsyncResult<Result.GetProfileResult>> callback
        )
		{
			var task = new GetProfileTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.GetProfileResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.GetProfileResult> GetProfileAsync(
                Request.GetProfileRequest request
        )
		{
			var task = new GetProfileTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        private class GetProfileByUserIdTask : Gs2RestSessionTask<GetProfileByUserIdRequest, GetProfileByUserIdResult>
        {
            public GetProfileByUserIdTask(IGs2Session session, RestSessionRequestFactory factory, GetProfileByUserIdRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(GetProfileByUserIdRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "friend")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/{userId}/profile";

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
		public IEnumerator GetProfileByUserId(
                Request.GetProfileByUserIdRequest request,
                UnityAction<AsyncResult<Result.GetProfileByUserIdResult>> callback
        )
		{
			var task = new GetProfileByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.GetProfileByUserIdResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.GetProfileByUserIdResult> GetProfileByUserIdAsync(
                Request.GetProfileByUserIdRequest request
        )
		{
			var task = new GetProfileByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        private class UpdateProfileTask : Gs2RestSessionTask<UpdateProfileRequest, UpdateProfileResult>
        {
            public UpdateProfileTask(IGs2Session session, RestSessionRequestFactory factory, UpdateProfileRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(UpdateProfileRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "friend")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/me/profile";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");

                var sessionRequest = Factory.Put(url);

                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);
                jsonWriter.WriteObjectStart();
                if (request.PublicProfile != null)
                {
                    jsonWriter.WritePropertyName("publicProfile");
                    jsonWriter.Write(request.PublicProfile);
                }
                if (request.FollowerProfile != null)
                {
                    jsonWriter.WritePropertyName("followerProfile");
                    jsonWriter.Write(request.FollowerProfile);
                }
                if (request.FriendProfile != null)
                {
                    jsonWriter.WritePropertyName("friendProfile");
                    jsonWriter.Write(request.FriendProfile);
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
		public IEnumerator UpdateProfile(
                Request.UpdateProfileRequest request,
                UnityAction<AsyncResult<Result.UpdateProfileResult>> callback
        )
		{
			var task = new UpdateProfileTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.UpdateProfileResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.UpdateProfileResult> UpdateProfileAsync(
                Request.UpdateProfileRequest request
        )
		{
			var task = new UpdateProfileTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        private class UpdateProfileByUserIdTask : Gs2RestSessionTask<UpdateProfileByUserIdRequest, UpdateProfileByUserIdResult>
        {
            public UpdateProfileByUserIdTask(IGs2Session session, RestSessionRequestFactory factory, UpdateProfileByUserIdRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(UpdateProfileByUserIdRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "friend")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/{userId}/profile";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{userId}", !string.IsNullOrEmpty(request.UserId) ? request.UserId.ToString() : "null");

                var sessionRequest = Factory.Put(url);

                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);
                jsonWriter.WriteObjectStart();
                if (request.PublicProfile != null)
                {
                    jsonWriter.WritePropertyName("publicProfile");
                    jsonWriter.Write(request.PublicProfile);
                }
                if (request.FollowerProfile != null)
                {
                    jsonWriter.WritePropertyName("followerProfile");
                    jsonWriter.Write(request.FollowerProfile);
                }
                if (request.FriendProfile != null)
                {
                    jsonWriter.WritePropertyName("friendProfile");
                    jsonWriter.Write(request.FriendProfile);
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
		public IEnumerator UpdateProfileByUserId(
                Request.UpdateProfileByUserIdRequest request,
                UnityAction<AsyncResult<Result.UpdateProfileByUserIdResult>> callback
        )
		{
			var task = new UpdateProfileByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.UpdateProfileByUserIdResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.UpdateProfileByUserIdResult> UpdateProfileByUserIdAsync(
                Request.UpdateProfileByUserIdRequest request
        )
		{
			var task = new UpdateProfileByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        private class DeleteProfileByUserIdTask : Gs2RestSessionTask<DeleteProfileByUserIdRequest, DeleteProfileByUserIdResult>
        {
            public DeleteProfileByUserIdTask(IGs2Session session, RestSessionRequestFactory factory, DeleteProfileByUserIdRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DeleteProfileByUserIdRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "friend")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/{userId}/profile";

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
		public IEnumerator DeleteProfileByUserId(
                Request.DeleteProfileByUserIdRequest request,
                UnityAction<AsyncResult<Result.DeleteProfileByUserIdResult>> callback
        )
		{
			var task = new DeleteProfileByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.DeleteProfileByUserIdResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.DeleteProfileByUserIdResult> DeleteProfileByUserIdAsync(
                Request.DeleteProfileByUserIdRequest request
        )
		{
			var task = new DeleteProfileByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        private class GetPublicProfileTask : Gs2RestSessionTask<GetPublicProfileRequest, GetPublicProfileResult>
        {
            public GetPublicProfileTask(IGs2Session session, RestSessionRequestFactory factory, GetPublicProfileRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(GetPublicProfileRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "friend")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/{userId}/profile/public";

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
		public IEnumerator GetPublicProfile(
                Request.GetPublicProfileRequest request,
                UnityAction<AsyncResult<Result.GetPublicProfileResult>> callback
        )
		{
			var task = new GetPublicProfileTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.GetPublicProfileResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.GetPublicProfileResult> GetPublicProfileAsync(
                Request.GetPublicProfileRequest request
        )
		{
			var task = new GetPublicProfileTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        private class DescribeFollowsTask : Gs2RestSessionTask<DescribeFollowsRequest, DescribeFollowsResult>
        {
            public DescribeFollowsTask(IGs2Session session, RestSessionRequestFactory factory, DescribeFollowsRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DescribeFollowsRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "friend")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/me/follow";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");

                var sessionRequest = Factory.Get(url);
                if (request.ContextStack != null)
                {
                    sessionRequest.AddQueryString("contextStack", request.ContextStack);
                }
                if (request.WithProfile != null) {
                    sessionRequest.AddQueryString("withProfile", $"{request.WithProfile}");
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
		public IEnumerator DescribeFollows(
                Request.DescribeFollowsRequest request,
                UnityAction<AsyncResult<Result.DescribeFollowsResult>> callback
        )
		{
			var task = new DescribeFollowsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.DescribeFollowsResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.DescribeFollowsResult> DescribeFollowsAsync(
                Request.DescribeFollowsRequest request
        )
		{
			var task = new DescribeFollowsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        private class DescribeFollowsByUserIdTask : Gs2RestSessionTask<DescribeFollowsByUserIdRequest, DescribeFollowsByUserIdResult>
        {
            public DescribeFollowsByUserIdTask(IGs2Session session, RestSessionRequestFactory factory, DescribeFollowsByUserIdRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DescribeFollowsByUserIdRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "friend")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/{userId}/follow";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{userId}", !string.IsNullOrEmpty(request.UserId) ? request.UserId.ToString() : "null");

                var sessionRequest = Factory.Get(url);
                if (request.ContextStack != null)
                {
                    sessionRequest.AddQueryString("contextStack", request.ContextStack);
                }
                if (request.WithProfile != null) {
                    sessionRequest.AddQueryString("withProfile", $"{request.WithProfile}");
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
		public IEnumerator DescribeFollowsByUserId(
                Request.DescribeFollowsByUserIdRequest request,
                UnityAction<AsyncResult<Result.DescribeFollowsByUserIdResult>> callback
        )
		{
			var task = new DescribeFollowsByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.DescribeFollowsByUserIdResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.DescribeFollowsByUserIdResult> DescribeFollowsByUserIdAsync(
                Request.DescribeFollowsByUserIdRequest request
        )
		{
			var task = new DescribeFollowsByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        private class GetFollowTask : Gs2RestSessionTask<GetFollowRequest, GetFollowResult>
        {
            public GetFollowTask(IGs2Session session, RestSessionRequestFactory factory, GetFollowRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(GetFollowRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "friend")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/me/follow/{targetUserId}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{targetUserId}", !string.IsNullOrEmpty(request.TargetUserId) ? request.TargetUserId.ToString() : "null");

                var sessionRequest = Factory.Get(url);
                if (request.ContextStack != null)
                {
                    sessionRequest.AddQueryString("contextStack", request.ContextStack);
                }
                if (request.WithProfile != null) {
                    sessionRequest.AddQueryString("withProfile", $"{request.WithProfile}");
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
		public IEnumerator GetFollow(
                Request.GetFollowRequest request,
                UnityAction<AsyncResult<Result.GetFollowResult>> callback
        )
		{
			var task = new GetFollowTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.GetFollowResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.GetFollowResult> GetFollowAsync(
                Request.GetFollowRequest request
        )
		{
			var task = new GetFollowTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        private class GetFollowByUserIdTask : Gs2RestSessionTask<GetFollowByUserIdRequest, GetFollowByUserIdResult>
        {
            public GetFollowByUserIdTask(IGs2Session session, RestSessionRequestFactory factory, GetFollowByUserIdRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(GetFollowByUserIdRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "friend")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/{userId}/follow/{targetUserId}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{userId}", !string.IsNullOrEmpty(request.UserId) ? request.UserId.ToString() : "null");
                url = url.Replace("{targetUserId}", !string.IsNullOrEmpty(request.TargetUserId) ? request.TargetUserId.ToString() : "null");

                var sessionRequest = Factory.Get(url);
                if (request.ContextStack != null)
                {
                    sessionRequest.AddQueryString("contextStack", request.ContextStack);
                }
                if (request.WithProfile != null) {
                    sessionRequest.AddQueryString("withProfile", $"{request.WithProfile}");
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
		public IEnumerator GetFollowByUserId(
                Request.GetFollowByUserIdRequest request,
                UnityAction<AsyncResult<Result.GetFollowByUserIdResult>> callback
        )
		{
			var task = new GetFollowByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.GetFollowByUserIdResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.GetFollowByUserIdResult> GetFollowByUserIdAsync(
                Request.GetFollowByUserIdRequest request
        )
		{
			var task = new GetFollowByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        private class FollowTask : Gs2RestSessionTask<FollowRequest, FollowResult>
        {
            public FollowTask(IGs2Session session, RestSessionRequestFactory factory, FollowRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(FollowRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "friend")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/me/follow/{targetUserId}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{targetUserId}", !string.IsNullOrEmpty(request.TargetUserId) ? request.TargetUserId.ToString() : "null");

                var sessionRequest = Factory.Put(url);

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
		public IEnumerator Follow(
                Request.FollowRequest request,
                UnityAction<AsyncResult<Result.FollowResult>> callback
        )
		{
			var task = new FollowTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.FollowResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.FollowResult> FollowAsync(
                Request.FollowRequest request
        )
		{
			var task = new FollowTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        private class FollowByUserIdTask : Gs2RestSessionTask<FollowByUserIdRequest, FollowByUserIdResult>
        {
            public FollowByUserIdTask(IGs2Session session, RestSessionRequestFactory factory, FollowByUserIdRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(FollowByUserIdRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "friend")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/{userId}/follow/{targetUserId}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{userId}", !string.IsNullOrEmpty(request.UserId) ? request.UserId.ToString() : "null");
                url = url.Replace("{targetUserId}", !string.IsNullOrEmpty(request.TargetUserId) ? request.TargetUserId.ToString() : "null");

                var sessionRequest = Factory.Put(url);

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
		public IEnumerator FollowByUserId(
                Request.FollowByUserIdRequest request,
                UnityAction<AsyncResult<Result.FollowByUserIdResult>> callback
        )
		{
			var task = new FollowByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.FollowByUserIdResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.FollowByUserIdResult> FollowByUserIdAsync(
                Request.FollowByUserIdRequest request
        )
		{
			var task = new FollowByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        private class UnfollowTask : Gs2RestSessionTask<UnfollowRequest, UnfollowResult>
        {
            public UnfollowTask(IGs2Session session, RestSessionRequestFactory factory, UnfollowRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(UnfollowRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "friend")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/me/follow/{targetUserId}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{targetUserId}", !string.IsNullOrEmpty(request.TargetUserId) ? request.TargetUserId.ToString() : "null");

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
		public IEnumerator Unfollow(
                Request.UnfollowRequest request,
                UnityAction<AsyncResult<Result.UnfollowResult>> callback
        )
		{
			var task = new UnfollowTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.UnfollowResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.UnfollowResult> UnfollowAsync(
                Request.UnfollowRequest request
        )
		{
			var task = new UnfollowTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        private class UnfollowByUserIdTask : Gs2RestSessionTask<UnfollowByUserIdRequest, UnfollowByUserIdResult>
        {
            public UnfollowByUserIdTask(IGs2Session session, RestSessionRequestFactory factory, UnfollowByUserIdRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(UnfollowByUserIdRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "friend")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/{userId}/follow/{targetUserId}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{userId}", !string.IsNullOrEmpty(request.UserId) ? request.UserId.ToString() : "null");
                url = url.Replace("{targetUserId}", !string.IsNullOrEmpty(request.TargetUserId) ? request.TargetUserId.ToString() : "null");

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
		public IEnumerator UnfollowByUserId(
                Request.UnfollowByUserIdRequest request,
                UnityAction<AsyncResult<Result.UnfollowByUserIdResult>> callback
        )
		{
			var task = new UnfollowByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.UnfollowByUserIdResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.UnfollowByUserIdResult> UnfollowByUserIdAsync(
                Request.UnfollowByUserIdRequest request
        )
		{
			var task = new UnfollowByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        private class DescribeFriendsTask : Gs2RestSessionTask<DescribeFriendsRequest, DescribeFriendsResult>
        {
            public DescribeFriendsTask(IGs2Session session, RestSessionRequestFactory factory, DescribeFriendsRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DescribeFriendsRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "friend")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/me/friend";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");

                var sessionRequest = Factory.Get(url);
                if (request.ContextStack != null)
                {
                    sessionRequest.AddQueryString("contextStack", request.ContextStack);
                }
                if (request.WithProfile != null) {
                    sessionRequest.AddQueryString("withProfile", $"{request.WithProfile}");
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
		public IEnumerator DescribeFriends(
                Request.DescribeFriendsRequest request,
                UnityAction<AsyncResult<Result.DescribeFriendsResult>> callback
        )
		{
			var task = new DescribeFriendsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.DescribeFriendsResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.DescribeFriendsResult> DescribeFriendsAsync(
                Request.DescribeFriendsRequest request
        )
		{
			var task = new DescribeFriendsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        private class DescribeFriendsByUserIdTask : Gs2RestSessionTask<DescribeFriendsByUserIdRequest, DescribeFriendsByUserIdResult>
        {
            public DescribeFriendsByUserIdTask(IGs2Session session, RestSessionRequestFactory factory, DescribeFriendsByUserIdRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DescribeFriendsByUserIdRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "friend")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/{userId}/friend";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{userId}", !string.IsNullOrEmpty(request.UserId) ? request.UserId.ToString() : "null");

                var sessionRequest = Factory.Get(url);
                if (request.ContextStack != null)
                {
                    sessionRequest.AddQueryString("contextStack", request.ContextStack);
                }
                if (request.WithProfile != null) {
                    sessionRequest.AddQueryString("withProfile", $"{request.WithProfile}");
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
		public IEnumerator DescribeFriendsByUserId(
                Request.DescribeFriendsByUserIdRequest request,
                UnityAction<AsyncResult<Result.DescribeFriendsByUserIdResult>> callback
        )
		{
			var task = new DescribeFriendsByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.DescribeFriendsByUserIdResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.DescribeFriendsByUserIdResult> DescribeFriendsByUserIdAsync(
                Request.DescribeFriendsByUserIdRequest request
        )
		{
			var task = new DescribeFriendsByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        private class GetFriendTask : Gs2RestSessionTask<GetFriendRequest, GetFriendResult>
        {
            public GetFriendTask(IGs2Session session, RestSessionRequestFactory factory, GetFriendRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(GetFriendRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "friend")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/me/friend/{targetUserId}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{targetUserId}", !string.IsNullOrEmpty(request.TargetUserId) ? request.TargetUserId.ToString() : "null");

                var sessionRequest = Factory.Get(url);
                if (request.ContextStack != null)
                {
                    sessionRequest.AddQueryString("contextStack", request.ContextStack);
                }
                if (request.WithProfile != null) {
                    sessionRequest.AddQueryString("withProfile", $"{request.WithProfile}");
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
		public IEnumerator GetFriend(
                Request.GetFriendRequest request,
                UnityAction<AsyncResult<Result.GetFriendResult>> callback
        )
		{
			var task = new GetFriendTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.GetFriendResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.GetFriendResult> GetFriendAsync(
                Request.GetFriendRequest request
        )
		{
			var task = new GetFriendTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        private class GetFriendByUserIdTask : Gs2RestSessionTask<GetFriendByUserIdRequest, GetFriendByUserIdResult>
        {
            public GetFriendByUserIdTask(IGs2Session session, RestSessionRequestFactory factory, GetFriendByUserIdRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(GetFriendByUserIdRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "friend")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/{userId}/friend/{targetUserId}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{userId}", !string.IsNullOrEmpty(request.UserId) ? request.UserId.ToString() : "null");
                url = url.Replace("{targetUserId}", !string.IsNullOrEmpty(request.TargetUserId) ? request.TargetUserId.ToString() : "null");

                var sessionRequest = Factory.Get(url);
                if (request.ContextStack != null)
                {
                    sessionRequest.AddQueryString("contextStack", request.ContextStack);
                }
                if (request.WithProfile != null) {
                    sessionRequest.AddQueryString("withProfile", $"{request.WithProfile}");
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
		public IEnumerator GetFriendByUserId(
                Request.GetFriendByUserIdRequest request,
                UnityAction<AsyncResult<Result.GetFriendByUserIdResult>> callback
        )
		{
			var task = new GetFriendByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.GetFriendByUserIdResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.GetFriendByUserIdResult> GetFriendByUserIdAsync(
                Request.GetFriendByUserIdRequest request
        )
		{
			var task = new GetFriendByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        private class DeleteFriendTask : Gs2RestSessionTask<DeleteFriendRequest, DeleteFriendResult>
        {
            public DeleteFriendTask(IGs2Session session, RestSessionRequestFactory factory, DeleteFriendRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DeleteFriendRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "friend")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/me/friend/{targetUserId}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{targetUserId}", !string.IsNullOrEmpty(request.TargetUserId) ? request.TargetUserId.ToString() : "null");

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
		public IEnumerator DeleteFriend(
                Request.DeleteFriendRequest request,
                UnityAction<AsyncResult<Result.DeleteFriendResult>> callback
        )
		{
			var task = new DeleteFriendTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.DeleteFriendResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.DeleteFriendResult> DeleteFriendAsync(
                Request.DeleteFriendRequest request
        )
		{
			var task = new DeleteFriendTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        private class DeleteFriendByUserIdTask : Gs2RestSessionTask<DeleteFriendByUserIdRequest, DeleteFriendByUserIdResult>
        {
            public DeleteFriendByUserIdTask(IGs2Session session, RestSessionRequestFactory factory, DeleteFriendByUserIdRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DeleteFriendByUserIdRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "friend")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/{userId}/friend/{targetUserId}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{userId}", !string.IsNullOrEmpty(request.UserId) ? request.UserId.ToString() : "null");
                url = url.Replace("{targetUserId}", !string.IsNullOrEmpty(request.TargetUserId) ? request.TargetUserId.ToString() : "null");

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
		public IEnumerator DeleteFriendByUserId(
                Request.DeleteFriendByUserIdRequest request,
                UnityAction<AsyncResult<Result.DeleteFriendByUserIdResult>> callback
        )
		{
			var task = new DeleteFriendByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.DeleteFriendByUserIdResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.DeleteFriendByUserIdResult> DeleteFriendByUserIdAsync(
                Request.DeleteFriendByUserIdRequest request
        )
		{
			var task = new DeleteFriendByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        private class DescribeSendRequestsTask : Gs2RestSessionTask<DescribeSendRequestsRequest, DescribeSendRequestsResult>
        {
            public DescribeSendRequestsTask(IGs2Session session, RestSessionRequestFactory factory, DescribeSendRequestsRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DescribeSendRequestsRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "friend")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/me/sendBox";

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
		public IEnumerator DescribeSendRequests(
                Request.DescribeSendRequestsRequest request,
                UnityAction<AsyncResult<Result.DescribeSendRequestsResult>> callback
        )
		{
			var task = new DescribeSendRequestsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.DescribeSendRequestsResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.DescribeSendRequestsResult> DescribeSendRequestsAsync(
                Request.DescribeSendRequestsRequest request
        )
		{
			var task = new DescribeSendRequestsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        private class DescribeSendRequestsByUserIdTask : Gs2RestSessionTask<DescribeSendRequestsByUserIdRequest, DescribeSendRequestsByUserIdResult>
        {
            public DescribeSendRequestsByUserIdTask(IGs2Session session, RestSessionRequestFactory factory, DescribeSendRequestsByUserIdRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DescribeSendRequestsByUserIdRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "friend")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/{userId}/sendBox";

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
		public IEnumerator DescribeSendRequestsByUserId(
                Request.DescribeSendRequestsByUserIdRequest request,
                UnityAction<AsyncResult<Result.DescribeSendRequestsByUserIdResult>> callback
        )
		{
			var task = new DescribeSendRequestsByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.DescribeSendRequestsByUserIdResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.DescribeSendRequestsByUserIdResult> DescribeSendRequestsByUserIdAsync(
                Request.DescribeSendRequestsByUserIdRequest request
        )
		{
			var task = new DescribeSendRequestsByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        private class GetSendRequestTask : Gs2RestSessionTask<GetSendRequestRequest, GetSendRequestResult>
        {
            public GetSendRequestTask(IGs2Session session, RestSessionRequestFactory factory, GetSendRequestRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(GetSendRequestRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "friend")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/me/sendBox/{targetUserId}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{targetUserId}", !string.IsNullOrEmpty(request.TargetUserId) ? request.TargetUserId.ToString() : "null");

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
		public IEnumerator GetSendRequest(
                Request.GetSendRequestRequest request,
                UnityAction<AsyncResult<Result.GetSendRequestResult>> callback
        )
		{
			var task = new GetSendRequestTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.GetSendRequestResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.GetSendRequestResult> GetSendRequestAsync(
                Request.GetSendRequestRequest request
        )
		{
			var task = new GetSendRequestTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        private class GetSendRequestByUserIdTask : Gs2RestSessionTask<GetSendRequestByUserIdRequest, GetSendRequestByUserIdResult>
        {
            public GetSendRequestByUserIdTask(IGs2Session session, RestSessionRequestFactory factory, GetSendRequestByUserIdRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(GetSendRequestByUserIdRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "friend")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/{userId}/sendBox/{targetUserId}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{userId}", !string.IsNullOrEmpty(request.UserId) ? request.UserId.ToString() : "null");
                url = url.Replace("{targetUserId}", !string.IsNullOrEmpty(request.TargetUserId) ? request.TargetUserId.ToString() : "null");

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
		public IEnumerator GetSendRequestByUserId(
                Request.GetSendRequestByUserIdRequest request,
                UnityAction<AsyncResult<Result.GetSendRequestByUserIdResult>> callback
        )
		{
			var task = new GetSendRequestByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.GetSendRequestByUserIdResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.GetSendRequestByUserIdResult> GetSendRequestByUserIdAsync(
                Request.GetSendRequestByUserIdRequest request
        )
		{
			var task = new GetSendRequestByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        private class SendRequestTask : Gs2RestSessionTask<SendRequestRequest, SendRequestResult>
        {
            public SendRequestTask(IGs2Session session, RestSessionRequestFactory factory, SendRequestRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(SendRequestRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "friend")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/me/sendBox/{targetUserId}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{targetUserId}", !string.IsNullOrEmpty(request.TargetUserId) ? request.TargetUserId.ToString() : "null");

                var sessionRequest = Factory.Put(url);

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
		public IEnumerator SendRequest(
                Request.SendRequestRequest request,
                UnityAction<AsyncResult<Result.SendRequestResult>> callback
        )
		{
			var task = new SendRequestTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.SendRequestResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.SendRequestResult> SendRequestAsync(
                Request.SendRequestRequest request
        )
		{
			var task = new SendRequestTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        private class SendRequestByUserIdTask : Gs2RestSessionTask<SendRequestByUserIdRequest, SendRequestByUserIdResult>
        {
            public SendRequestByUserIdTask(IGs2Session session, RestSessionRequestFactory factory, SendRequestByUserIdRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(SendRequestByUserIdRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "friend")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/{userId}/sendBox/{targetUserId}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{userId}", !string.IsNullOrEmpty(request.UserId) ? request.UserId.ToString() : "null");
                url = url.Replace("{targetUserId}", !string.IsNullOrEmpty(request.TargetUserId) ? request.TargetUserId.ToString() : "null");

                var sessionRequest = Factory.Put(url);

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
		public IEnumerator SendRequestByUserId(
                Request.SendRequestByUserIdRequest request,
                UnityAction<AsyncResult<Result.SendRequestByUserIdResult>> callback
        )
		{
			var task = new SendRequestByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.SendRequestByUserIdResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.SendRequestByUserIdResult> SendRequestByUserIdAsync(
                Request.SendRequestByUserIdRequest request
        )
		{
			var task = new SendRequestByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        private class DeleteRequestTask : Gs2RestSessionTask<DeleteRequestRequest, DeleteRequestResult>
        {
            public DeleteRequestTask(IGs2Session session, RestSessionRequestFactory factory, DeleteRequestRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DeleteRequestRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "friend")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/me/sendBox/{targetUserId}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{targetUserId}", !string.IsNullOrEmpty(request.TargetUserId) ? request.TargetUserId.ToString() : "null");

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
		public IEnumerator DeleteRequest(
                Request.DeleteRequestRequest request,
                UnityAction<AsyncResult<Result.DeleteRequestResult>> callback
        )
		{
			var task = new DeleteRequestTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.DeleteRequestResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.DeleteRequestResult> DeleteRequestAsync(
                Request.DeleteRequestRequest request
        )
		{
			var task = new DeleteRequestTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        private class DeleteRequestByUserIdTask : Gs2RestSessionTask<DeleteRequestByUserIdRequest, DeleteRequestByUserIdResult>
        {
            public DeleteRequestByUserIdTask(IGs2Session session, RestSessionRequestFactory factory, DeleteRequestByUserIdRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DeleteRequestByUserIdRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "friend")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/{userId}/sendBox/{targetUserId}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{userId}", !string.IsNullOrEmpty(request.UserId) ? request.UserId.ToString() : "null");
                url = url.Replace("{targetUserId}", !string.IsNullOrEmpty(request.TargetUserId) ? request.TargetUserId.ToString() : "null");

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
		public IEnumerator DeleteRequestByUserId(
                Request.DeleteRequestByUserIdRequest request,
                UnityAction<AsyncResult<Result.DeleteRequestByUserIdResult>> callback
        )
		{
			var task = new DeleteRequestByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.DeleteRequestByUserIdResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.DeleteRequestByUserIdResult> DeleteRequestByUserIdAsync(
                Request.DeleteRequestByUserIdRequest request
        )
		{
			var task = new DeleteRequestByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        private class DescribeReceiveRequestsTask : Gs2RestSessionTask<DescribeReceiveRequestsRequest, DescribeReceiveRequestsResult>
        {
            public DescribeReceiveRequestsTask(IGs2Session session, RestSessionRequestFactory factory, DescribeReceiveRequestsRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DescribeReceiveRequestsRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "friend")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/me/inbox";

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
		public IEnumerator DescribeReceiveRequests(
                Request.DescribeReceiveRequestsRequest request,
                UnityAction<AsyncResult<Result.DescribeReceiveRequestsResult>> callback
        )
		{
			var task = new DescribeReceiveRequestsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.DescribeReceiveRequestsResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.DescribeReceiveRequestsResult> DescribeReceiveRequestsAsync(
                Request.DescribeReceiveRequestsRequest request
        )
		{
			var task = new DescribeReceiveRequestsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        private class DescribeReceiveRequestsByUserIdTask : Gs2RestSessionTask<DescribeReceiveRequestsByUserIdRequest, DescribeReceiveRequestsByUserIdResult>
        {
            public DescribeReceiveRequestsByUserIdTask(IGs2Session session, RestSessionRequestFactory factory, DescribeReceiveRequestsByUserIdRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DescribeReceiveRequestsByUserIdRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "friend")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/{userId}/inbox";

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
		public IEnumerator DescribeReceiveRequestsByUserId(
                Request.DescribeReceiveRequestsByUserIdRequest request,
                UnityAction<AsyncResult<Result.DescribeReceiveRequestsByUserIdResult>> callback
        )
		{
			var task = new DescribeReceiveRequestsByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.DescribeReceiveRequestsByUserIdResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.DescribeReceiveRequestsByUserIdResult> DescribeReceiveRequestsByUserIdAsync(
                Request.DescribeReceiveRequestsByUserIdRequest request
        )
		{
			var task = new DescribeReceiveRequestsByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        private class GetReceiveRequestTask : Gs2RestSessionTask<GetReceiveRequestRequest, GetReceiveRequestResult>
        {
            public GetReceiveRequestTask(IGs2Session session, RestSessionRequestFactory factory, GetReceiveRequestRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(GetReceiveRequestRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "friend")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/me/inbox/{fromUserId}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{fromUserId}", !string.IsNullOrEmpty(request.FromUserId) ? request.FromUserId.ToString() : "null");

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
		public IEnumerator GetReceiveRequest(
                Request.GetReceiveRequestRequest request,
                UnityAction<AsyncResult<Result.GetReceiveRequestResult>> callback
        )
		{
			var task = new GetReceiveRequestTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.GetReceiveRequestResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.GetReceiveRequestResult> GetReceiveRequestAsync(
                Request.GetReceiveRequestRequest request
        )
		{
			var task = new GetReceiveRequestTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        private class GetReceiveRequestByUserIdTask : Gs2RestSessionTask<GetReceiveRequestByUserIdRequest, GetReceiveRequestByUserIdResult>
        {
            public GetReceiveRequestByUserIdTask(IGs2Session session, RestSessionRequestFactory factory, GetReceiveRequestByUserIdRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(GetReceiveRequestByUserIdRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "friend")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/{userId}/inbox/{fromUserId}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{userId}", !string.IsNullOrEmpty(request.UserId) ? request.UserId.ToString() : "null");
                url = url.Replace("{fromUserId}", !string.IsNullOrEmpty(request.FromUserId) ? request.FromUserId.ToString() : "null");

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
		public IEnumerator GetReceiveRequestByUserId(
                Request.GetReceiveRequestByUserIdRequest request,
                UnityAction<AsyncResult<Result.GetReceiveRequestByUserIdResult>> callback
        )
		{
			var task = new GetReceiveRequestByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.GetReceiveRequestByUserIdResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.GetReceiveRequestByUserIdResult> GetReceiveRequestByUserIdAsync(
                Request.GetReceiveRequestByUserIdRequest request
        )
		{
			var task = new GetReceiveRequestByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        private class AcceptRequestTask : Gs2RestSessionTask<AcceptRequestRequest, AcceptRequestResult>
        {
            public AcceptRequestTask(IGs2Session session, RestSessionRequestFactory factory, AcceptRequestRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(AcceptRequestRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "friend")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/me/inbox/{fromUserId}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{fromUserId}", !string.IsNullOrEmpty(request.FromUserId) ? request.FromUserId.ToString() : "null");

                var sessionRequest = Factory.Put(url);

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
		public IEnumerator AcceptRequest(
                Request.AcceptRequestRequest request,
                UnityAction<AsyncResult<Result.AcceptRequestResult>> callback
        )
		{
			var task = new AcceptRequestTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.AcceptRequestResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.AcceptRequestResult> AcceptRequestAsync(
                Request.AcceptRequestRequest request
        )
		{
			var task = new AcceptRequestTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        private class AcceptRequestByUserIdTask : Gs2RestSessionTask<AcceptRequestByUserIdRequest, AcceptRequestByUserIdResult>
        {
            public AcceptRequestByUserIdTask(IGs2Session session, RestSessionRequestFactory factory, AcceptRequestByUserIdRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(AcceptRequestByUserIdRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "friend")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/{userId}/inbox/{fromUserId}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{userId}", !string.IsNullOrEmpty(request.UserId) ? request.UserId.ToString() : "null");
                url = url.Replace("{fromUserId}", !string.IsNullOrEmpty(request.FromUserId) ? request.FromUserId.ToString() : "null");

                var sessionRequest = Factory.Put(url);

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
		public IEnumerator AcceptRequestByUserId(
                Request.AcceptRequestByUserIdRequest request,
                UnityAction<AsyncResult<Result.AcceptRequestByUserIdResult>> callback
        )
		{
			var task = new AcceptRequestByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.AcceptRequestByUserIdResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.AcceptRequestByUserIdResult> AcceptRequestByUserIdAsync(
                Request.AcceptRequestByUserIdRequest request
        )
		{
			var task = new AcceptRequestByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        private class RejectRequestTask : Gs2RestSessionTask<RejectRequestRequest, RejectRequestResult>
        {
            public RejectRequestTask(IGs2Session session, RestSessionRequestFactory factory, RejectRequestRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(RejectRequestRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "friend")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/me/inbox/{fromUserId}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{fromUserId}", !string.IsNullOrEmpty(request.FromUserId) ? request.FromUserId.ToString() : "null");

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
		public IEnumerator RejectRequest(
                Request.RejectRequestRequest request,
                UnityAction<AsyncResult<Result.RejectRequestResult>> callback
        )
		{
			var task = new RejectRequestTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.RejectRequestResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.RejectRequestResult> RejectRequestAsync(
                Request.RejectRequestRequest request
        )
		{
			var task = new RejectRequestTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        private class RejectRequestByUserIdTask : Gs2RestSessionTask<RejectRequestByUserIdRequest, RejectRequestByUserIdResult>
        {
            public RejectRequestByUserIdTask(IGs2Session session, RestSessionRequestFactory factory, RejectRequestByUserIdRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(RejectRequestByUserIdRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "friend")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/{userId}/inbox/{fromUserId}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{userId}", !string.IsNullOrEmpty(request.UserId) ? request.UserId.ToString() : "null");
                url = url.Replace("{fromUserId}", !string.IsNullOrEmpty(request.FromUserId) ? request.FromUserId.ToString() : "null");

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
		public IEnumerator RejectRequestByUserId(
                Request.RejectRequestByUserIdRequest request,
                UnityAction<AsyncResult<Result.RejectRequestByUserIdResult>> callback
        )
		{
			var task = new RejectRequestByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.RejectRequestByUserIdResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.RejectRequestByUserIdResult> RejectRequestByUserIdAsync(
                Request.RejectRequestByUserIdRequest request
        )
		{
			var task = new RejectRequestByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        private class DescribeBlackListTask : Gs2RestSessionTask<DescribeBlackListRequest, DescribeBlackListResult>
        {
            public DescribeBlackListTask(IGs2Session session, RestSessionRequestFactory factory, DescribeBlackListRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DescribeBlackListRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "friend")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/me/blackList";

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
		public IEnumerator DescribeBlackList(
                Request.DescribeBlackListRequest request,
                UnityAction<AsyncResult<Result.DescribeBlackListResult>> callback
        )
		{
			var task = new DescribeBlackListTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.DescribeBlackListResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.DescribeBlackListResult> DescribeBlackListAsync(
                Request.DescribeBlackListRequest request
        )
		{
			var task = new DescribeBlackListTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        private class DescribeBlackListByUserIdTask : Gs2RestSessionTask<DescribeBlackListByUserIdRequest, DescribeBlackListByUserIdResult>
        {
            public DescribeBlackListByUserIdTask(IGs2Session session, RestSessionRequestFactory factory, DescribeBlackListByUserIdRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DescribeBlackListByUserIdRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "friend")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/{userId}/blackList";

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
		public IEnumerator DescribeBlackListByUserId(
                Request.DescribeBlackListByUserIdRequest request,
                UnityAction<AsyncResult<Result.DescribeBlackListByUserIdResult>> callback
        )
		{
			var task = new DescribeBlackListByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.DescribeBlackListByUserIdResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.DescribeBlackListByUserIdResult> DescribeBlackListByUserIdAsync(
                Request.DescribeBlackListByUserIdRequest request
        )
		{
			var task = new DescribeBlackListByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        private class RegisterBlackListTask : Gs2RestSessionTask<RegisterBlackListRequest, RegisterBlackListResult>
        {
            public RegisterBlackListTask(IGs2Session session, RestSessionRequestFactory factory, RegisterBlackListRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(RegisterBlackListRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "friend")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/me/blackList/{targetUserId}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{targetUserId}", !string.IsNullOrEmpty(request.TargetUserId) ? request.TargetUserId.ToString() : "null");

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
		public IEnumerator RegisterBlackList(
                Request.RegisterBlackListRequest request,
                UnityAction<AsyncResult<Result.RegisterBlackListResult>> callback
        )
		{
			var task = new RegisterBlackListTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.RegisterBlackListResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.RegisterBlackListResult> RegisterBlackListAsync(
                Request.RegisterBlackListRequest request
        )
		{
			var task = new RegisterBlackListTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        private class RegisterBlackListByUserIdTask : Gs2RestSessionTask<RegisterBlackListByUserIdRequest, RegisterBlackListByUserIdResult>
        {
            public RegisterBlackListByUserIdTask(IGs2Session session, RestSessionRequestFactory factory, RegisterBlackListByUserIdRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(RegisterBlackListByUserIdRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "friend")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/{userId}/blackList/{targetUserId}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{userId}", !string.IsNullOrEmpty(request.UserId) ? request.UserId.ToString() : "null");
                url = url.Replace("{targetUserId}", !string.IsNullOrEmpty(request.TargetUserId) ? request.TargetUserId.ToString() : "null");

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
		public IEnumerator RegisterBlackListByUserId(
                Request.RegisterBlackListByUserIdRequest request,
                UnityAction<AsyncResult<Result.RegisterBlackListByUserIdResult>> callback
        )
		{
			var task = new RegisterBlackListByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.RegisterBlackListByUserIdResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.RegisterBlackListByUserIdResult> RegisterBlackListByUserIdAsync(
                Request.RegisterBlackListByUserIdRequest request
        )
		{
			var task = new RegisterBlackListByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        private class UnregisterBlackListTask : Gs2RestSessionTask<UnregisterBlackListRequest, UnregisterBlackListResult>
        {
            public UnregisterBlackListTask(IGs2Session session, RestSessionRequestFactory factory, UnregisterBlackListRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(UnregisterBlackListRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "friend")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/me/blackList/{targetUserId}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{targetUserId}", !string.IsNullOrEmpty(request.TargetUserId) ? request.TargetUserId.ToString() : "null");

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
		public IEnumerator UnregisterBlackList(
                Request.UnregisterBlackListRequest request,
                UnityAction<AsyncResult<Result.UnregisterBlackListResult>> callback
        )
		{
			var task = new UnregisterBlackListTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.UnregisterBlackListResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.UnregisterBlackListResult> UnregisterBlackListAsync(
                Request.UnregisterBlackListRequest request
        )
		{
			var task = new UnregisterBlackListTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        private class UnregisterBlackListByUserIdTask : Gs2RestSessionTask<UnregisterBlackListByUserIdRequest, UnregisterBlackListByUserIdResult>
        {
            public UnregisterBlackListByUserIdTask(IGs2Session session, RestSessionRequestFactory factory, UnregisterBlackListByUserIdRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(UnregisterBlackListByUserIdRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "friend")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/{userId}/blackList/{targetUserId}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{userId}", !string.IsNullOrEmpty(request.UserId) ? request.UserId.ToString() : "null");
                url = url.Replace("{targetUserId}", !string.IsNullOrEmpty(request.TargetUserId) ? request.TargetUserId.ToString() : "null");

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
		public IEnumerator UnregisterBlackListByUserId(
                Request.UnregisterBlackListByUserIdRequest request,
                UnityAction<AsyncResult<Result.UnregisterBlackListByUserIdResult>> callback
        )
		{
			var task = new UnregisterBlackListByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.UnregisterBlackListByUserIdResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.UnregisterBlackListByUserIdResult> UnregisterBlackListByUserIdAsync(
                Request.UnregisterBlackListByUserIdRequest request
        )
		{
			var task = new UnregisterBlackListByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif
	}
}