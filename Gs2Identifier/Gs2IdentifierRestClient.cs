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
 *
 * deny overwrite
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
using Gs2.Gs2Identifier.Request;
using Gs2.Gs2Identifier.Result;
using Gs2.Util.LitJson;

namespace Gs2.Gs2Identifier
{
	public class Gs2IdentifierRestClient : AbstractGs2Client
	{
#if UNITY_2017_1_OR_NEWER
		private readonly CertificateHandler _certificateHandler;
#endif

		public static string Endpoint = "identifier";

        protected Gs2RestSession Gs2RestSession => (Gs2RestSession) Gs2Session;

		public Gs2IdentifierRestClient(Gs2RestSession Gs2RestSession) : base(Gs2RestSession)
		{

		}

#if UNITY_2017_1_OR_NEWER
		public Gs2IdentifierRestClient(Gs2RestSession gs2RestSession, CertificateHandler certificateHandler) : base(gs2RestSession)
		{
			_certificateHandler = certificateHandler;
		}
#endif


        private class DescribeUsersTask : Gs2RestSessionTask<DescribeUsersRequest, DescribeUsersResult>
        {
            public DescribeUsersTask(IGs2Session session, RestSessionRequestFactory factory, DescribeUsersRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DescribeUsersRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "identifier")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/user";

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
		public IEnumerator DescribeUsers(
                Request.DescribeUsersRequest request,
                UnityAction<AsyncResult<Result.DescribeUsersResult>> callback
        )
		{
			var task = new DescribeUsersTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.DescribeUsersResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.DescribeUsersResult> DescribeUsersAsync(
                Request.DescribeUsersRequest request
        )
		{
			var task = new DescribeUsersTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        private class CreateUserTask : Gs2RestSessionTask<CreateUserRequest, CreateUserResult>
        {
            public CreateUserTask(IGs2Session session, RestSessionRequestFactory factory, CreateUserRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(CreateUserRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "identifier")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/user";

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
		public IEnumerator CreateUser(
                Request.CreateUserRequest request,
                UnityAction<AsyncResult<Result.CreateUserResult>> callback
        )
		{
			var task = new CreateUserTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.CreateUserResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.CreateUserResult> CreateUserAsync(
                Request.CreateUserRequest request
        )
		{
			var task = new CreateUserTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        private class UpdateUserTask : Gs2RestSessionTask<UpdateUserRequest, UpdateUserResult>
        {
            public UpdateUserTask(IGs2Session session, RestSessionRequestFactory factory, UpdateUserRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(UpdateUserRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "identifier")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/user/{userName}";

                url = url.Replace("{userName}", !string.IsNullOrEmpty(request.UserName) ? request.UserName.ToString() : "null");

                var sessionRequest = Factory.Put(url);

                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);
                jsonWriter.WriteObjectStart();
                if (request.Description != null)
                {
                    jsonWriter.WritePropertyName("description");
                    jsonWriter.Write(request.Description);
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
		public IEnumerator UpdateUser(
                Request.UpdateUserRequest request,
                UnityAction<AsyncResult<Result.UpdateUserResult>> callback
        )
		{
			var task = new UpdateUserTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.UpdateUserResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.UpdateUserResult> UpdateUserAsync(
                Request.UpdateUserRequest request
        )
		{
			var task = new UpdateUserTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        private class GetUserTask : Gs2RestSessionTask<GetUserRequest, GetUserResult>
        {
            public GetUserTask(IGs2Session session, RestSessionRequestFactory factory, GetUserRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(GetUserRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "identifier")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/user/{userName}";

                url = url.Replace("{userName}", !string.IsNullOrEmpty(request.UserName) ? request.UserName.ToString() : "null");

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
		public IEnumerator GetUser(
                Request.GetUserRequest request,
                UnityAction<AsyncResult<Result.GetUserResult>> callback
        )
		{
			var task = new GetUserTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.GetUserResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.GetUserResult> GetUserAsync(
                Request.GetUserRequest request
        )
		{
			var task = new GetUserTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        private class DeleteUserTask : Gs2RestSessionTask<DeleteUserRequest, DeleteUserResult>
        {
            public DeleteUserTask(IGs2Session session, RestSessionRequestFactory factory, DeleteUserRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DeleteUserRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "identifier")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/user/{userName}";

                url = url.Replace("{userName}", !string.IsNullOrEmpty(request.UserName) ? request.UserName.ToString() : "null");

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
		public IEnumerator DeleteUser(
                Request.DeleteUserRequest request,
                UnityAction<AsyncResult<Result.DeleteUserResult>> callback
        )
		{
			var task = new DeleteUserTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.DeleteUserResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.DeleteUserResult> DeleteUserAsync(
                Request.DeleteUserRequest request
        )
		{
			var task = new DeleteUserTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        private class DescribeSecurityPoliciesTask : Gs2RestSessionTask<DescribeSecurityPoliciesRequest, DescribeSecurityPoliciesResult>
        {
            public DescribeSecurityPoliciesTask(IGs2Session session, RestSessionRequestFactory factory, DescribeSecurityPoliciesRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DescribeSecurityPoliciesRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "identifier")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/securityPolicy";

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
		public IEnumerator DescribeSecurityPolicies(
                Request.DescribeSecurityPoliciesRequest request,
                UnityAction<AsyncResult<Result.DescribeSecurityPoliciesResult>> callback
        )
		{
			var task = new DescribeSecurityPoliciesTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.DescribeSecurityPoliciesResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.DescribeSecurityPoliciesResult> DescribeSecurityPoliciesAsync(
                Request.DescribeSecurityPoliciesRequest request
        )
		{
			var task = new DescribeSecurityPoliciesTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        private class DescribeCommonSecurityPoliciesTask : Gs2RestSessionTask<DescribeCommonSecurityPoliciesRequest, DescribeCommonSecurityPoliciesResult>
        {
            public DescribeCommonSecurityPoliciesTask(IGs2Session session, RestSessionRequestFactory factory, DescribeCommonSecurityPoliciesRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DescribeCommonSecurityPoliciesRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "identifier")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/securityPolicy/common";

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
		public IEnumerator DescribeCommonSecurityPolicies(
                Request.DescribeCommonSecurityPoliciesRequest request,
                UnityAction<AsyncResult<Result.DescribeCommonSecurityPoliciesResult>> callback
        )
		{
			var task = new DescribeCommonSecurityPoliciesTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.DescribeCommonSecurityPoliciesResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.DescribeCommonSecurityPoliciesResult> DescribeCommonSecurityPoliciesAsync(
                Request.DescribeCommonSecurityPoliciesRequest request
        )
		{
			var task = new DescribeCommonSecurityPoliciesTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        private class CreateSecurityPolicyTask : Gs2RestSessionTask<CreateSecurityPolicyRequest, CreateSecurityPolicyResult>
        {
            public CreateSecurityPolicyTask(IGs2Session session, RestSessionRequestFactory factory, CreateSecurityPolicyRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(CreateSecurityPolicyRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "identifier")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/securityPolicy";

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
                if (request.Policy != null)
                {
                    jsonWriter.WritePropertyName("policy");
                    jsonWriter.Write(request.Policy);
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
		public IEnumerator CreateSecurityPolicy(
                Request.CreateSecurityPolicyRequest request,
                UnityAction<AsyncResult<Result.CreateSecurityPolicyResult>> callback
        )
		{
			var task = new CreateSecurityPolicyTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.CreateSecurityPolicyResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.CreateSecurityPolicyResult> CreateSecurityPolicyAsync(
                Request.CreateSecurityPolicyRequest request
        )
		{
			var task = new CreateSecurityPolicyTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        private class UpdateSecurityPolicyTask : Gs2RestSessionTask<UpdateSecurityPolicyRequest, UpdateSecurityPolicyResult>
        {
            public UpdateSecurityPolicyTask(IGs2Session session, RestSessionRequestFactory factory, UpdateSecurityPolicyRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(UpdateSecurityPolicyRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "identifier")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/securityPolicy/{securityPolicyName}";

                url = url.Replace("{securityPolicyName}", !string.IsNullOrEmpty(request.SecurityPolicyName) ? request.SecurityPolicyName.ToString() : "null");

                var sessionRequest = Factory.Put(url);

                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);
                jsonWriter.WriteObjectStart();
                if (request.Description != null)
                {
                    jsonWriter.WritePropertyName("description");
                    jsonWriter.Write(request.Description);
                }
                if (request.Policy != null)
                {
                    jsonWriter.WritePropertyName("policy");
                    jsonWriter.Write(request.Policy);
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
		public IEnumerator UpdateSecurityPolicy(
                Request.UpdateSecurityPolicyRequest request,
                UnityAction<AsyncResult<Result.UpdateSecurityPolicyResult>> callback
        )
		{
			var task = new UpdateSecurityPolicyTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.UpdateSecurityPolicyResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.UpdateSecurityPolicyResult> UpdateSecurityPolicyAsync(
                Request.UpdateSecurityPolicyRequest request
        )
		{
			var task = new UpdateSecurityPolicyTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        private class GetSecurityPolicyTask : Gs2RestSessionTask<GetSecurityPolicyRequest, GetSecurityPolicyResult>
        {
            public GetSecurityPolicyTask(IGs2Session session, RestSessionRequestFactory factory, GetSecurityPolicyRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(GetSecurityPolicyRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "identifier")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/securityPolicy/{securityPolicyName}";

                url = url.Replace("{securityPolicyName}", !string.IsNullOrEmpty(request.SecurityPolicyName) ? request.SecurityPolicyName.ToString() : "null");

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
		public IEnumerator GetSecurityPolicy(
                Request.GetSecurityPolicyRequest request,
                UnityAction<AsyncResult<Result.GetSecurityPolicyResult>> callback
        )
		{
			var task = new GetSecurityPolicyTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.GetSecurityPolicyResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.GetSecurityPolicyResult> GetSecurityPolicyAsync(
                Request.GetSecurityPolicyRequest request
        )
		{
			var task = new GetSecurityPolicyTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        private class DeleteSecurityPolicyTask : Gs2RestSessionTask<DeleteSecurityPolicyRequest, DeleteSecurityPolicyResult>
        {
            public DeleteSecurityPolicyTask(IGs2Session session, RestSessionRequestFactory factory, DeleteSecurityPolicyRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DeleteSecurityPolicyRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "identifier")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/securityPolicy/{securityPolicyName}";

                url = url.Replace("{securityPolicyName}", !string.IsNullOrEmpty(request.SecurityPolicyName) ? request.SecurityPolicyName.ToString() : "null");

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
		public IEnumerator DeleteSecurityPolicy(
                Request.DeleteSecurityPolicyRequest request,
                UnityAction<AsyncResult<Result.DeleteSecurityPolicyResult>> callback
        )
		{
			var task = new DeleteSecurityPolicyTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.DeleteSecurityPolicyResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.DeleteSecurityPolicyResult> DeleteSecurityPolicyAsync(
                Request.DeleteSecurityPolicyRequest request
        )
		{
			var task = new DeleteSecurityPolicyTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        private class DescribeIdentifiersTask : Gs2RestSessionTask<DescribeIdentifiersRequest, DescribeIdentifiersResult>
        {
            public DescribeIdentifiersTask(IGs2Session session, RestSessionRequestFactory factory, DescribeIdentifiersRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DescribeIdentifiersRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "identifier")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/user/{userName}/identifier";

                url = url.Replace("{userName}", !string.IsNullOrEmpty(request.UserName) ? request.UserName.ToString() : "null");

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
		public IEnumerator DescribeIdentifiers(
                Request.DescribeIdentifiersRequest request,
                UnityAction<AsyncResult<Result.DescribeIdentifiersResult>> callback
        )
		{
			var task = new DescribeIdentifiersTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.DescribeIdentifiersResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.DescribeIdentifiersResult> DescribeIdentifiersAsync(
                Request.DescribeIdentifiersRequest request
        )
		{
			var task = new DescribeIdentifiersTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        private class CreateIdentifierTask : Gs2RestSessionTask<CreateIdentifierRequest, CreateIdentifierResult>
        {
            public CreateIdentifierTask(IGs2Session session, RestSessionRequestFactory factory, CreateIdentifierRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(CreateIdentifierRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "identifier")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/user/{userName}/identifier";

                url = url.Replace("{userName}", !string.IsNullOrEmpty(request.UserName) ? request.UserName.ToString() : "null");

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
		public IEnumerator CreateIdentifier(
                Request.CreateIdentifierRequest request,
                UnityAction<AsyncResult<Result.CreateIdentifierResult>> callback
        )
		{
			var task = new CreateIdentifierTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.CreateIdentifierResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.CreateIdentifierResult> CreateIdentifierAsync(
                Request.CreateIdentifierRequest request
        )
		{
			var task = new CreateIdentifierTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        private class GetIdentifierTask : Gs2RestSessionTask<GetIdentifierRequest, GetIdentifierResult>
        {
            public GetIdentifierTask(IGs2Session session, RestSessionRequestFactory factory, GetIdentifierRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(GetIdentifierRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "identifier")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/user/{userName}/identifier/{clientId}";

                url = url.Replace("{userName}", !string.IsNullOrEmpty(request.UserName) ? request.UserName.ToString() : "null");
                url = url.Replace("{clientId}", !string.IsNullOrEmpty(request.ClientId) ? request.ClientId.ToString() : "null");

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
		public IEnumerator GetIdentifier(
                Request.GetIdentifierRequest request,
                UnityAction<AsyncResult<Result.GetIdentifierResult>> callback
        )
		{
			var task = new GetIdentifierTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.GetIdentifierResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.GetIdentifierResult> GetIdentifierAsync(
                Request.GetIdentifierRequest request
        )
		{
			var task = new GetIdentifierTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        private class DeleteIdentifierTask : Gs2RestSessionTask<DeleteIdentifierRequest, DeleteIdentifierResult>
        {
            public DeleteIdentifierTask(IGs2Session session, RestSessionRequestFactory factory, DeleteIdentifierRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DeleteIdentifierRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "identifier")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/user/{userName}/identifier/{clientId}";

                url = url.Replace("{userName}", !string.IsNullOrEmpty(request.UserName) ? request.UserName.ToString() : "null");
                url = url.Replace("{clientId}", !string.IsNullOrEmpty(request.ClientId) ? request.ClientId.ToString() : "null");

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
		public IEnumerator DeleteIdentifier(
                Request.DeleteIdentifierRequest request,
                UnityAction<AsyncResult<Result.DeleteIdentifierResult>> callback
        )
		{
			var task = new DeleteIdentifierTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.DeleteIdentifierResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.DeleteIdentifierResult> DeleteIdentifierAsync(
                Request.DeleteIdentifierRequest request
        )
		{
			var task = new DeleteIdentifierTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        private class DescribePasswordsTask : Gs2RestSessionTask<DescribePasswordsRequest, DescribePasswordsResult>
        {
            public DescribePasswordsTask(IGs2Session session, RestSessionRequestFactory factory, DescribePasswordsRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DescribePasswordsRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "identifier")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/user/{userName}/password";

                url = url.Replace("{userName}", !string.IsNullOrEmpty(request.UserName) ? request.UserName.ToString() : "null");

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
		public IEnumerator DescribePasswords(
                Request.DescribePasswordsRequest request,
                UnityAction<AsyncResult<Result.DescribePasswordsResult>> callback
        )
		{
			var task = new DescribePasswordsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.DescribePasswordsResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.DescribePasswordsResult> DescribePasswordsAsync(
                Request.DescribePasswordsRequest request
        )
		{
			var task = new DescribePasswordsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        private class CreatePasswordTask : Gs2RestSessionTask<CreatePasswordRequest, CreatePasswordResult>
        {
            public CreatePasswordTask(IGs2Session session, RestSessionRequestFactory factory, CreatePasswordRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(CreatePasswordRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "identifier")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/user/{userName}/password";

                url = url.Replace("{userName}", !string.IsNullOrEmpty(request.UserName) ? request.UserName.ToString() : "null");

                var sessionRequest = Factory.Post(url);

                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);
                jsonWriter.WriteObjectStart();
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
		public IEnumerator CreatePassword(
                Request.CreatePasswordRequest request,
                UnityAction<AsyncResult<Result.CreatePasswordResult>> callback
        )
		{
			var task = new CreatePasswordTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.CreatePasswordResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.CreatePasswordResult> CreatePasswordAsync(
                Request.CreatePasswordRequest request
        )
		{
			var task = new CreatePasswordTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        private class GetPasswordTask : Gs2RestSessionTask<GetPasswordRequest, GetPasswordResult>
        {
            public GetPasswordTask(IGs2Session session, RestSessionRequestFactory factory, GetPasswordRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(GetPasswordRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "identifier")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/user/{userName}/password/entity";

                url = url.Replace("{userName}", !string.IsNullOrEmpty(request.UserName) ? request.UserName.ToString() : "null");

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
		public IEnumerator GetPassword(
                Request.GetPasswordRequest request,
                UnityAction<AsyncResult<Result.GetPasswordResult>> callback
        )
		{
			var task = new GetPasswordTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.GetPasswordResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.GetPasswordResult> GetPasswordAsync(
                Request.GetPasswordRequest request
        )
		{
			var task = new GetPasswordTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        private class DeletePasswordTask : Gs2RestSessionTask<DeletePasswordRequest, DeletePasswordResult>
        {
            public DeletePasswordTask(IGs2Session session, RestSessionRequestFactory factory, DeletePasswordRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DeletePasswordRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "identifier")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/user/{userName}/password/entity";

                url = url.Replace("{userName}", !string.IsNullOrEmpty(request.UserName) ? request.UserName.ToString() : "null");

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
		public IEnumerator DeletePassword(
                Request.DeletePasswordRequest request,
                UnityAction<AsyncResult<Result.DeletePasswordResult>> callback
        )
		{
			var task = new DeletePasswordTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.DeletePasswordResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.DeletePasswordResult> DeletePasswordAsync(
                Request.DeletePasswordRequest request
        )
		{
			var task = new DeletePasswordTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        private class GetHasSecurityPolicyTask : Gs2RestSessionTask<GetHasSecurityPolicyRequest, GetHasSecurityPolicyResult>
        {
            public GetHasSecurityPolicyTask(IGs2Session session, RestSessionRequestFactory factory, GetHasSecurityPolicyRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(GetHasSecurityPolicyRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "identifier")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/user/{userName}/securityPolicy";

                url = url.Replace("{userName}", !string.IsNullOrEmpty(request.UserName) ? request.UserName.ToString() : "null");

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
		public IEnumerator GetHasSecurityPolicy(
                Request.GetHasSecurityPolicyRequest request,
                UnityAction<AsyncResult<Result.GetHasSecurityPolicyResult>> callback
        )
		{
			var task = new GetHasSecurityPolicyTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.GetHasSecurityPolicyResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.GetHasSecurityPolicyResult> GetHasSecurityPolicyAsync(
                Request.GetHasSecurityPolicyRequest request
        )
		{
			var task = new GetHasSecurityPolicyTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        private class AttachSecurityPolicyTask : Gs2RestSessionTask<AttachSecurityPolicyRequest, AttachSecurityPolicyResult>
        {
            public AttachSecurityPolicyTask(IGs2Session session, RestSessionRequestFactory factory, AttachSecurityPolicyRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(AttachSecurityPolicyRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "identifier")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/user/{userName}/securityPolicy";

                url = url.Replace("{userName}", !string.IsNullOrEmpty(request.UserName) ? request.UserName.ToString() : "null");

                var sessionRequest = Factory.Post(url);

                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);
                jsonWriter.WriteObjectStart();
                if (request.SecurityPolicyId != null)
                {
                    jsonWriter.WritePropertyName("securityPolicyId");
                    jsonWriter.Write(request.SecurityPolicyId);
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
		public IEnumerator AttachSecurityPolicy(
                Request.AttachSecurityPolicyRequest request,
                UnityAction<AsyncResult<Result.AttachSecurityPolicyResult>> callback
        )
		{
			var task = new AttachSecurityPolicyTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.AttachSecurityPolicyResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.AttachSecurityPolicyResult> AttachSecurityPolicyAsync(
                Request.AttachSecurityPolicyRequest request
        )
		{
			var task = new AttachSecurityPolicyTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        private class DetachSecurityPolicyTask : Gs2RestSessionTask<DetachSecurityPolicyRequest, DetachSecurityPolicyResult>
        {
            public DetachSecurityPolicyTask(IGs2Session session, RestSessionRequestFactory factory, DetachSecurityPolicyRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DetachSecurityPolicyRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "identifier")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/user/{userName}/securityPolicy/{securityPolicyId}";

                url = url.Replace("{userName}", !string.IsNullOrEmpty(request.UserName) ? request.UserName.ToString() : "null");
                url = url.Replace("{securityPolicyId}", !string.IsNullOrEmpty(request.SecurityPolicyId) ? request.SecurityPolicyId.ToString() : "null");

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
		public IEnumerator DetachSecurityPolicy(
                Request.DetachSecurityPolicyRequest request,
                UnityAction<AsyncResult<Result.DetachSecurityPolicyResult>> callback
        )
		{
			var task = new DetachSecurityPolicyTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.DetachSecurityPolicyResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.DetachSecurityPolicyResult> DetachSecurityPolicyAsync(
                Request.DetachSecurityPolicyRequest request
        )
		{
			var task = new DetachSecurityPolicyTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        private class LoginTask : Gs2RestSessionTask<LoginRequest, LoginResult>
        {
            public LoginTask(IGs2Session session, RestSessionRequestFactory factory, LoginRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(LoginRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "identifier")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/projectToken/login";

                var sessionRequest = Factory.Post(url);

                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);
                jsonWriter.WriteObjectStart();
                if (request.ClientId != null)
                {
                    jsonWriter.WritePropertyName("client_id");
                    jsonWriter.Write(request.ClientId);
                }
                if (request.ClientSecret != null)
                {
                    jsonWriter.WritePropertyName("client_secret");
                    jsonWriter.Write(request.ClientSecret);
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
		public IEnumerator Login(
                Request.LoginRequest request,
                UnityAction<AsyncResult<Result.LoginResult>> callback
        )
		{
			var task = new LoginTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.LoginResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.LoginResult> LoginAsync(
                Request.LoginRequest request
        )
		{
			var task = new LoginTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        private class LoginByUserTask : Gs2RestSessionTask<LoginByUserRequest, LoginByUserResult>
        {
            public LoginByUserTask(IGs2Session session, RestSessionRequestFactory factory, LoginByUserRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(LoginByUserRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "identifier")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/projectToken/login/user";

                var sessionRequest = Factory.Post(url);

                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);
                jsonWriter.WriteObjectStart();
                if (request.UserName != null)
                {
                    jsonWriter.WritePropertyName("userName");
                    jsonWriter.Write(request.UserName);
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
		public IEnumerator LoginByUser(
                Request.LoginByUserRequest request,
                UnityAction<AsyncResult<Result.LoginByUserResult>> callback
        )
		{
			var task = new LoginByUserTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.LoginByUserResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.LoginByUserResult> LoginByUserAsync(
                Request.LoginByUserRequest request
        )
		{
			var task = new LoginByUserTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif
	}
}