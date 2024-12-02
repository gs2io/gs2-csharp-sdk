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
    #if GS2_ENABLE_UNITASK
using Cysharp.Threading.Tasks;
    #endif
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


        public class DescribeUsersTask : Gs2RestSessionTask<DescribeUsersRequest, DescribeUsersResult>
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
                if (request.DryRun)
                {
                    sessionRequest.AddHeader("X-GS2-DRY-RUN", "true");
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
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.DescribeUsersResult>(task.Result, task.Error));
        }

		public IFuture<Result.DescribeUsersResult> DescribeUsersFuture(
                Request.DescribeUsersRequest request
        )
		{
			return new DescribeUsersTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.DescribeUsersResult> DescribeUsersAsync(
                Request.DescribeUsersRequest request
        )
		{
            AsyncResult<Result.DescribeUsersResult> result = null;
			await DescribeUsers(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
    #else
		public DescribeUsersTask DescribeUsersAsync(
                Request.DescribeUsersRequest request
        )
		{
			return new DescribeUsersTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
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


        public class CreateUserTask : Gs2RestSessionTask<CreateUserRequest, CreateUserResult>
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
                if (request.DryRun)
                {
                    sessionRequest.AddHeader("X-GS2-DRY-RUN", "true");
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
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.CreateUserResult>(task.Result, task.Error));
        }

		public IFuture<Result.CreateUserResult> CreateUserFuture(
                Request.CreateUserRequest request
        )
		{
			return new CreateUserTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.CreateUserResult> CreateUserAsync(
                Request.CreateUserRequest request
        )
		{
            AsyncResult<Result.CreateUserResult> result = null;
			await CreateUser(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
    #else
		public CreateUserTask CreateUserAsync(
                Request.CreateUserRequest request
        )
		{
			return new CreateUserTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
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


        public class UpdateUserTask : Gs2RestSessionTask<UpdateUserRequest, UpdateUserResult>
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
                if (request.DryRun)
                {
                    sessionRequest.AddHeader("X-GS2-DRY-RUN", "true");
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
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.UpdateUserResult>(task.Result, task.Error));
        }

		public IFuture<Result.UpdateUserResult> UpdateUserFuture(
                Request.UpdateUserRequest request
        )
		{
			return new UpdateUserTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.UpdateUserResult> UpdateUserAsync(
                Request.UpdateUserRequest request
        )
		{
            AsyncResult<Result.UpdateUserResult> result = null;
			await UpdateUser(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
    #else
		public UpdateUserTask UpdateUserAsync(
                Request.UpdateUserRequest request
        )
		{
			return new UpdateUserTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
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


        public class GetUserTask : Gs2RestSessionTask<GetUserRequest, GetUserResult>
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
                if (request.DryRun)
                {
                    sessionRequest.AddHeader("X-GS2-DRY-RUN", "true");
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
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.GetUserResult>(task.Result, task.Error));
        }

		public IFuture<Result.GetUserResult> GetUserFuture(
                Request.GetUserRequest request
        )
		{
			return new GetUserTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.GetUserResult> GetUserAsync(
                Request.GetUserRequest request
        )
		{
            AsyncResult<Result.GetUserResult> result = null;
			await GetUser(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
    #else
		public GetUserTask GetUserAsync(
                Request.GetUserRequest request
        )
		{
			return new GetUserTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
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


        public class DeleteUserTask : Gs2RestSessionTask<DeleteUserRequest, DeleteUserResult>
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
                if (request.DryRun)
                {
                    sessionRequest.AddHeader("X-GS2-DRY-RUN", "true");
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
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.DeleteUserResult>(task.Result, task.Error));
        }

		public IFuture<Result.DeleteUserResult> DeleteUserFuture(
                Request.DeleteUserRequest request
        )
		{
			return new DeleteUserTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.DeleteUserResult> DeleteUserAsync(
                Request.DeleteUserRequest request
        )
		{
            AsyncResult<Result.DeleteUserResult> result = null;
			await DeleteUser(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
    #else
		public DeleteUserTask DeleteUserAsync(
                Request.DeleteUserRequest request
        )
		{
			return new DeleteUserTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
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


        public class DescribeSecurityPoliciesTask : Gs2RestSessionTask<DescribeSecurityPoliciesRequest, DescribeSecurityPoliciesResult>
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
                if (request.DryRun)
                {
                    sessionRequest.AddHeader("X-GS2-DRY-RUN", "true");
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
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.DescribeSecurityPoliciesResult>(task.Result, task.Error));
        }

		public IFuture<Result.DescribeSecurityPoliciesResult> DescribeSecurityPoliciesFuture(
                Request.DescribeSecurityPoliciesRequest request
        )
		{
			return new DescribeSecurityPoliciesTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.DescribeSecurityPoliciesResult> DescribeSecurityPoliciesAsync(
                Request.DescribeSecurityPoliciesRequest request
        )
		{
            AsyncResult<Result.DescribeSecurityPoliciesResult> result = null;
			await DescribeSecurityPolicies(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
    #else
		public DescribeSecurityPoliciesTask DescribeSecurityPoliciesAsync(
                Request.DescribeSecurityPoliciesRequest request
        )
		{
			return new DescribeSecurityPoliciesTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
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


        public class DescribeCommonSecurityPoliciesTask : Gs2RestSessionTask<DescribeCommonSecurityPoliciesRequest, DescribeCommonSecurityPoliciesResult>
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
                if (request.DryRun)
                {
                    sessionRequest.AddHeader("X-GS2-DRY-RUN", "true");
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
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.DescribeCommonSecurityPoliciesResult>(task.Result, task.Error));
        }

		public IFuture<Result.DescribeCommonSecurityPoliciesResult> DescribeCommonSecurityPoliciesFuture(
                Request.DescribeCommonSecurityPoliciesRequest request
        )
		{
			return new DescribeCommonSecurityPoliciesTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.DescribeCommonSecurityPoliciesResult> DescribeCommonSecurityPoliciesAsync(
                Request.DescribeCommonSecurityPoliciesRequest request
        )
		{
            AsyncResult<Result.DescribeCommonSecurityPoliciesResult> result = null;
			await DescribeCommonSecurityPolicies(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
    #else
		public DescribeCommonSecurityPoliciesTask DescribeCommonSecurityPoliciesAsync(
                Request.DescribeCommonSecurityPoliciesRequest request
        )
		{
			return new DescribeCommonSecurityPoliciesTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
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


        public class CreateSecurityPolicyTask : Gs2RestSessionTask<CreateSecurityPolicyRequest, CreateSecurityPolicyResult>
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
                if (request.DryRun)
                {
                    sessionRequest.AddHeader("X-GS2-DRY-RUN", "true");
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
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.CreateSecurityPolicyResult>(task.Result, task.Error));
        }

		public IFuture<Result.CreateSecurityPolicyResult> CreateSecurityPolicyFuture(
                Request.CreateSecurityPolicyRequest request
        )
		{
			return new CreateSecurityPolicyTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.CreateSecurityPolicyResult> CreateSecurityPolicyAsync(
                Request.CreateSecurityPolicyRequest request
        )
		{
            AsyncResult<Result.CreateSecurityPolicyResult> result = null;
			await CreateSecurityPolicy(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
    #else
		public CreateSecurityPolicyTask CreateSecurityPolicyAsync(
                Request.CreateSecurityPolicyRequest request
        )
		{
			return new CreateSecurityPolicyTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
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


        public class UpdateSecurityPolicyTask : Gs2RestSessionTask<UpdateSecurityPolicyRequest, UpdateSecurityPolicyResult>
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
                if (request.DryRun)
                {
                    sessionRequest.AddHeader("X-GS2-DRY-RUN", "true");
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
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.UpdateSecurityPolicyResult>(task.Result, task.Error));
        }

		public IFuture<Result.UpdateSecurityPolicyResult> UpdateSecurityPolicyFuture(
                Request.UpdateSecurityPolicyRequest request
        )
		{
			return new UpdateSecurityPolicyTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.UpdateSecurityPolicyResult> UpdateSecurityPolicyAsync(
                Request.UpdateSecurityPolicyRequest request
        )
		{
            AsyncResult<Result.UpdateSecurityPolicyResult> result = null;
			await UpdateSecurityPolicy(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
    #else
		public UpdateSecurityPolicyTask UpdateSecurityPolicyAsync(
                Request.UpdateSecurityPolicyRequest request
        )
		{
			return new UpdateSecurityPolicyTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
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


        public class GetSecurityPolicyTask : Gs2RestSessionTask<GetSecurityPolicyRequest, GetSecurityPolicyResult>
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
                if (request.DryRun)
                {
                    sessionRequest.AddHeader("X-GS2-DRY-RUN", "true");
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
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.GetSecurityPolicyResult>(task.Result, task.Error));
        }

		public IFuture<Result.GetSecurityPolicyResult> GetSecurityPolicyFuture(
                Request.GetSecurityPolicyRequest request
        )
		{
			return new GetSecurityPolicyTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.GetSecurityPolicyResult> GetSecurityPolicyAsync(
                Request.GetSecurityPolicyRequest request
        )
		{
            AsyncResult<Result.GetSecurityPolicyResult> result = null;
			await GetSecurityPolicy(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
    #else
		public GetSecurityPolicyTask GetSecurityPolicyAsync(
                Request.GetSecurityPolicyRequest request
        )
		{
			return new GetSecurityPolicyTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
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


        public class DeleteSecurityPolicyTask : Gs2RestSessionTask<DeleteSecurityPolicyRequest, DeleteSecurityPolicyResult>
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
                if (request.DryRun)
                {
                    sessionRequest.AddHeader("X-GS2-DRY-RUN", "true");
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
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.DeleteSecurityPolicyResult>(task.Result, task.Error));
        }

		public IFuture<Result.DeleteSecurityPolicyResult> DeleteSecurityPolicyFuture(
                Request.DeleteSecurityPolicyRequest request
        )
		{
			return new DeleteSecurityPolicyTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.DeleteSecurityPolicyResult> DeleteSecurityPolicyAsync(
                Request.DeleteSecurityPolicyRequest request
        )
		{
            AsyncResult<Result.DeleteSecurityPolicyResult> result = null;
			await DeleteSecurityPolicy(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
    #else
		public DeleteSecurityPolicyTask DeleteSecurityPolicyAsync(
                Request.DeleteSecurityPolicyRequest request
        )
		{
			return new DeleteSecurityPolicyTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
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


        public class DescribeIdentifiersTask : Gs2RestSessionTask<DescribeIdentifiersRequest, DescribeIdentifiersResult>
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
                if (request.DryRun)
                {
                    sessionRequest.AddHeader("X-GS2-DRY-RUN", "true");
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
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.DescribeIdentifiersResult>(task.Result, task.Error));
        }

		public IFuture<Result.DescribeIdentifiersResult> DescribeIdentifiersFuture(
                Request.DescribeIdentifiersRequest request
        )
		{
			return new DescribeIdentifiersTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.DescribeIdentifiersResult> DescribeIdentifiersAsync(
                Request.DescribeIdentifiersRequest request
        )
		{
            AsyncResult<Result.DescribeIdentifiersResult> result = null;
			await DescribeIdentifiers(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
    #else
		public DescribeIdentifiersTask DescribeIdentifiersAsync(
                Request.DescribeIdentifiersRequest request
        )
		{
			return new DescribeIdentifiersTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
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


        public class CreateIdentifierTask : Gs2RestSessionTask<CreateIdentifierRequest, CreateIdentifierResult>
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
                if (request.DryRun)
                {
                    sessionRequest.AddHeader("X-GS2-DRY-RUN", "true");
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
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.CreateIdentifierResult>(task.Result, task.Error));
        }

		public IFuture<Result.CreateIdentifierResult> CreateIdentifierFuture(
                Request.CreateIdentifierRequest request
        )
		{
			return new CreateIdentifierTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.CreateIdentifierResult> CreateIdentifierAsync(
                Request.CreateIdentifierRequest request
        )
		{
            AsyncResult<Result.CreateIdentifierResult> result = null;
			await CreateIdentifier(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
    #else
		public CreateIdentifierTask CreateIdentifierAsync(
                Request.CreateIdentifierRequest request
        )
		{
			return new CreateIdentifierTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
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


        public class GetIdentifierTask : Gs2RestSessionTask<GetIdentifierRequest, GetIdentifierResult>
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
                if (request.DryRun)
                {
                    sessionRequest.AddHeader("X-GS2-DRY-RUN", "true");
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
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.GetIdentifierResult>(task.Result, task.Error));
        }

		public IFuture<Result.GetIdentifierResult> GetIdentifierFuture(
                Request.GetIdentifierRequest request
        )
		{
			return new GetIdentifierTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.GetIdentifierResult> GetIdentifierAsync(
                Request.GetIdentifierRequest request
        )
		{
            AsyncResult<Result.GetIdentifierResult> result = null;
			await GetIdentifier(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
    #else
		public GetIdentifierTask GetIdentifierAsync(
                Request.GetIdentifierRequest request
        )
		{
			return new GetIdentifierTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
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


        public class DeleteIdentifierTask : Gs2RestSessionTask<DeleteIdentifierRequest, DeleteIdentifierResult>
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
                if (request.DryRun)
                {
                    sessionRequest.AddHeader("X-GS2-DRY-RUN", "true");
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
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.DeleteIdentifierResult>(task.Result, task.Error));
        }

		public IFuture<Result.DeleteIdentifierResult> DeleteIdentifierFuture(
                Request.DeleteIdentifierRequest request
        )
		{
			return new DeleteIdentifierTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.DeleteIdentifierResult> DeleteIdentifierAsync(
                Request.DeleteIdentifierRequest request
        )
		{
            AsyncResult<Result.DeleteIdentifierResult> result = null;
			await DeleteIdentifier(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
    #else
		public DeleteIdentifierTask DeleteIdentifierAsync(
                Request.DeleteIdentifierRequest request
        )
		{
			return new DeleteIdentifierTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
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


        public class DescribeAttachedGuardsTask : Gs2RestSessionTask<DescribeAttachedGuardsRequest, DescribeAttachedGuardsResult>
        {
            public DescribeAttachedGuardsTask(IGs2Session session, RestSessionRequestFactory factory, DescribeAttachedGuardsRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DescribeAttachedGuardsRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "identifier")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/user/{userName}/identifier/{clientId}/guard";

                url = url.Replace("{clientId}", !string.IsNullOrEmpty(request.ClientId) ? request.ClientId.ToString() : "null");
                url = url.Replace("{userName}", !string.IsNullOrEmpty(request.UserName) ? request.UserName.ToString() : "null");

                var sessionRequest = Factory.Get(url);
                if (request.ContextStack != null)
                {
                    sessionRequest.AddQueryString("contextStack", request.ContextStack);
                }
                if (request.DryRun)
                {
                    sessionRequest.AddHeader("X-GS2-DRY-RUN", "true");
                }

                AddHeader(
                    Session.Credential,
                    sessionRequest
                );

                return sessionRequest;
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator DescribeAttachedGuards(
                Request.DescribeAttachedGuardsRequest request,
                UnityAction<AsyncResult<Result.DescribeAttachedGuardsResult>> callback
        )
		{
			var task = new DescribeAttachedGuardsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.DescribeAttachedGuardsResult>(task.Result, task.Error));
        }

		public IFuture<Result.DescribeAttachedGuardsResult> DescribeAttachedGuardsFuture(
                Request.DescribeAttachedGuardsRequest request
        )
		{
			return new DescribeAttachedGuardsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.DescribeAttachedGuardsResult> DescribeAttachedGuardsAsync(
                Request.DescribeAttachedGuardsRequest request
        )
		{
            AsyncResult<Result.DescribeAttachedGuardsResult> result = null;
			await DescribeAttachedGuards(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
    #else
		public DescribeAttachedGuardsTask DescribeAttachedGuardsAsync(
                Request.DescribeAttachedGuardsRequest request
        )
		{
			return new DescribeAttachedGuardsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.DescribeAttachedGuardsResult> DescribeAttachedGuardsAsync(
                Request.DescribeAttachedGuardsRequest request
        )
		{
			var task = new DescribeAttachedGuardsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class AttachGuardTask : Gs2RestSessionTask<AttachGuardRequest, AttachGuardResult>
        {
            public AttachGuardTask(IGs2Session session, RestSessionRequestFactory factory, AttachGuardRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(AttachGuardRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "identifier")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/user/{userName}/identifier/{clientId}/guard";

                url = url.Replace("{userName}", !string.IsNullOrEmpty(request.UserName) ? request.UserName.ToString() : "null");
                url = url.Replace("{clientId}", !string.IsNullOrEmpty(request.ClientId) ? request.ClientId.ToString() : "null");

                var sessionRequest = Factory.Post(url);

                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);
                jsonWriter.WriteObjectStart();
                if (request.GuardNamespaceId != null)
                {
                    jsonWriter.WritePropertyName("guardNamespaceId");
                    jsonWriter.Write(request.GuardNamespaceId);
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
                if (request.DryRun)
                {
                    sessionRequest.AddHeader("X-GS2-DRY-RUN", "true");
                }

                AddHeader(
                    Session.Credential,
                    sessionRequest
                );

                return sessionRequest;
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator AttachGuard(
                Request.AttachGuardRequest request,
                UnityAction<AsyncResult<Result.AttachGuardResult>> callback
        )
		{
			var task = new AttachGuardTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.AttachGuardResult>(task.Result, task.Error));
        }

		public IFuture<Result.AttachGuardResult> AttachGuardFuture(
                Request.AttachGuardRequest request
        )
		{
			return new AttachGuardTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.AttachGuardResult> AttachGuardAsync(
                Request.AttachGuardRequest request
        )
		{
            AsyncResult<Result.AttachGuardResult> result = null;
			await AttachGuard(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
    #else
		public AttachGuardTask AttachGuardAsync(
                Request.AttachGuardRequest request
        )
		{
			return new AttachGuardTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.AttachGuardResult> AttachGuardAsync(
                Request.AttachGuardRequest request
        )
		{
			var task = new AttachGuardTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class DetachGuardTask : Gs2RestSessionTask<DetachGuardRequest, DetachGuardResult>
        {
            public DetachGuardTask(IGs2Session session, RestSessionRequestFactory factory, DetachGuardRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DetachGuardRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "identifier")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/user/{userName}/identifier/{clientId}/guard/{guardNamespaceId}";

                url = url.Replace("{userName}", !string.IsNullOrEmpty(request.UserName) ? request.UserName.ToString() : "null");
                url = url.Replace("{clientId}", !string.IsNullOrEmpty(request.ClientId) ? request.ClientId.ToString() : "null");
                url = url.Replace("{guardNamespaceId}", !string.IsNullOrEmpty(request.GuardNamespaceId) ? request.GuardNamespaceId.ToString() : "null");

                var sessionRequest = Factory.Delete(url);
                if (request.ContextStack != null)
                {
                    sessionRequest.AddQueryString("contextStack", request.ContextStack);
                }
                if (request.DryRun)
                {
                    sessionRequest.AddHeader("X-GS2-DRY-RUN", "true");
                }

                AddHeader(
                    Session.Credential,
                    sessionRequest
                );

                return sessionRequest;
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator DetachGuard(
                Request.DetachGuardRequest request,
                UnityAction<AsyncResult<Result.DetachGuardResult>> callback
        )
		{
			var task = new DetachGuardTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.DetachGuardResult>(task.Result, task.Error));
        }

		public IFuture<Result.DetachGuardResult> DetachGuardFuture(
                Request.DetachGuardRequest request
        )
		{
			return new DetachGuardTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.DetachGuardResult> DetachGuardAsync(
                Request.DetachGuardRequest request
        )
		{
            AsyncResult<Result.DetachGuardResult> result = null;
			await DetachGuard(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
    #else
		public DetachGuardTask DetachGuardAsync(
                Request.DetachGuardRequest request
        )
		{
			return new DetachGuardTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.DetachGuardResult> DetachGuardAsync(
                Request.DetachGuardRequest request
        )
		{
			var task = new DetachGuardTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class CreatePasswordTask : Gs2RestSessionTask<CreatePasswordRequest, CreatePasswordResult>
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
                if (request.DryRun)
                {
                    sessionRequest.AddHeader("X-GS2-DRY-RUN", "true");
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
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.CreatePasswordResult>(task.Result, task.Error));
        }

		public IFuture<Result.CreatePasswordResult> CreatePasswordFuture(
                Request.CreatePasswordRequest request
        )
		{
			return new CreatePasswordTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.CreatePasswordResult> CreatePasswordAsync(
                Request.CreatePasswordRequest request
        )
		{
            AsyncResult<Result.CreatePasswordResult> result = null;
			await CreatePassword(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
    #else
		public CreatePasswordTask CreatePasswordAsync(
                Request.CreatePasswordRequest request
        )
		{
			return new CreatePasswordTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
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


        public class GetPasswordTask : Gs2RestSessionTask<GetPasswordRequest, GetPasswordResult>
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
                if (request.DryRun)
                {
                    sessionRequest.AddHeader("X-GS2-DRY-RUN", "true");
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
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.GetPasswordResult>(task.Result, task.Error));
        }

		public IFuture<Result.GetPasswordResult> GetPasswordFuture(
                Request.GetPasswordRequest request
        )
		{
			return new GetPasswordTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.GetPasswordResult> GetPasswordAsync(
                Request.GetPasswordRequest request
        )
		{
            AsyncResult<Result.GetPasswordResult> result = null;
			await GetPassword(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
    #else
		public GetPasswordTask GetPasswordAsync(
                Request.GetPasswordRequest request
        )
		{
			return new GetPasswordTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
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


        public class EnableMfaTask : Gs2RestSessionTask<EnableMfaRequest, EnableMfaResult>
        {
            public EnableMfaTask(IGs2Session session, RestSessionRequestFactory factory, EnableMfaRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(EnableMfaRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "identifier")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/user/{userName}/mfa";

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
                if (request.DryRun)
                {
                    sessionRequest.AddHeader("X-GS2-DRY-RUN", "true");
                }

                AddHeader(
                    Session.Credential,
                    sessionRequest
                );

                return sessionRequest;
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator EnableMfa(
                Request.EnableMfaRequest request,
                UnityAction<AsyncResult<Result.EnableMfaResult>> callback
        )
		{
			var task = new EnableMfaTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.EnableMfaResult>(task.Result, task.Error));
        }

		public IFuture<Result.EnableMfaResult> EnableMfaFuture(
                Request.EnableMfaRequest request
        )
		{
			return new EnableMfaTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.EnableMfaResult> EnableMfaAsync(
                Request.EnableMfaRequest request
        )
		{
            AsyncResult<Result.EnableMfaResult> result = null;
			await EnableMfa(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
    #else
		public EnableMfaTask EnableMfaAsync(
                Request.EnableMfaRequest request
        )
		{
			return new EnableMfaTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.EnableMfaResult> EnableMfaAsync(
                Request.EnableMfaRequest request
        )
		{
			var task = new EnableMfaTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class ChallengeMfaTask : Gs2RestSessionTask<ChallengeMfaRequest, ChallengeMfaResult>
        {
            public ChallengeMfaTask(IGs2Session session, RestSessionRequestFactory factory, ChallengeMfaRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(ChallengeMfaRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "identifier")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/user/{userName}/mfa/challenge";

                url = url.Replace("{userName}", !string.IsNullOrEmpty(request.UserName) ? request.UserName.ToString() : "null");

                var sessionRequest = Factory.Post(url);

                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);
                jsonWriter.WriteObjectStart();
                if (request.Passcode != null)
                {
                    jsonWriter.WritePropertyName("passcode");
                    jsonWriter.Write(request.Passcode);
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
                if (request.DryRun)
                {
                    sessionRequest.AddHeader("X-GS2-DRY-RUN", "true");
                }

                AddHeader(
                    Session.Credential,
                    sessionRequest
                );

                return sessionRequest;
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator ChallengeMfa(
                Request.ChallengeMfaRequest request,
                UnityAction<AsyncResult<Result.ChallengeMfaResult>> callback
        )
		{
			var task = new ChallengeMfaTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.ChallengeMfaResult>(task.Result, task.Error));
        }

		public IFuture<Result.ChallengeMfaResult> ChallengeMfaFuture(
                Request.ChallengeMfaRequest request
        )
		{
			return new ChallengeMfaTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.ChallengeMfaResult> ChallengeMfaAsync(
                Request.ChallengeMfaRequest request
        )
		{
            AsyncResult<Result.ChallengeMfaResult> result = null;
			await ChallengeMfa(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
    #else
		public ChallengeMfaTask ChallengeMfaAsync(
                Request.ChallengeMfaRequest request
        )
		{
			return new ChallengeMfaTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.ChallengeMfaResult> ChallengeMfaAsync(
                Request.ChallengeMfaRequest request
        )
		{
			var task = new ChallengeMfaTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class DisableMfaTask : Gs2RestSessionTask<DisableMfaRequest, DisableMfaResult>
        {
            public DisableMfaTask(IGs2Session session, RestSessionRequestFactory factory, DisableMfaRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DisableMfaRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "identifier")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/user/{userName}/mfa";

                url = url.Replace("{userName}", !string.IsNullOrEmpty(request.UserName) ? request.UserName.ToString() : "null");

                var sessionRequest = Factory.Delete(url);
                if (request.ContextStack != null)
                {
                    sessionRequest.AddQueryString("contextStack", request.ContextStack);
                }
                if (request.DryRun)
                {
                    sessionRequest.AddHeader("X-GS2-DRY-RUN", "true");
                }

                AddHeader(
                    Session.Credential,
                    sessionRequest
                );

                return sessionRequest;
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator DisableMfa(
                Request.DisableMfaRequest request,
                UnityAction<AsyncResult<Result.DisableMfaResult>> callback
        )
		{
			var task = new DisableMfaTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.DisableMfaResult>(task.Result, task.Error));
        }

		public IFuture<Result.DisableMfaResult> DisableMfaFuture(
                Request.DisableMfaRequest request
        )
		{
			return new DisableMfaTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.DisableMfaResult> DisableMfaAsync(
                Request.DisableMfaRequest request
        )
		{
            AsyncResult<Result.DisableMfaResult> result = null;
			await DisableMfa(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
    #else
		public DisableMfaTask DisableMfaAsync(
                Request.DisableMfaRequest request
        )
		{
			return new DisableMfaTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.DisableMfaResult> DisableMfaAsync(
                Request.DisableMfaRequest request
        )
		{
			var task = new DisableMfaTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class DeletePasswordTask : Gs2RestSessionTask<DeletePasswordRequest, DeletePasswordResult>
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
                if (request.DryRun)
                {
                    sessionRequest.AddHeader("X-GS2-DRY-RUN", "true");
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
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.DeletePasswordResult>(task.Result, task.Error));
        }

		public IFuture<Result.DeletePasswordResult> DeletePasswordFuture(
                Request.DeletePasswordRequest request
        )
		{
			return new DeletePasswordTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.DeletePasswordResult> DeletePasswordAsync(
                Request.DeletePasswordRequest request
        )
		{
            AsyncResult<Result.DeletePasswordResult> result = null;
			await DeletePassword(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
    #else
		public DeletePasswordTask DeletePasswordAsync(
                Request.DeletePasswordRequest request
        )
		{
			return new DeletePasswordTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
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


        public class GetHasSecurityPolicyTask : Gs2RestSessionTask<GetHasSecurityPolicyRequest, GetHasSecurityPolicyResult>
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
                if (request.DryRun)
                {
                    sessionRequest.AddHeader("X-GS2-DRY-RUN", "true");
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
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.GetHasSecurityPolicyResult>(task.Result, task.Error));
        }

		public IFuture<Result.GetHasSecurityPolicyResult> GetHasSecurityPolicyFuture(
                Request.GetHasSecurityPolicyRequest request
        )
		{
			return new GetHasSecurityPolicyTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.GetHasSecurityPolicyResult> GetHasSecurityPolicyAsync(
                Request.GetHasSecurityPolicyRequest request
        )
		{
            AsyncResult<Result.GetHasSecurityPolicyResult> result = null;
			await GetHasSecurityPolicy(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
    #else
		public GetHasSecurityPolicyTask GetHasSecurityPolicyAsync(
                Request.GetHasSecurityPolicyRequest request
        )
		{
			return new GetHasSecurityPolicyTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
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


        public class AttachSecurityPolicyTask : Gs2RestSessionTask<AttachSecurityPolicyRequest, AttachSecurityPolicyResult>
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
                if (request.DryRun)
                {
                    sessionRequest.AddHeader("X-GS2-DRY-RUN", "true");
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
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.AttachSecurityPolicyResult>(task.Result, task.Error));
        }

		public IFuture<Result.AttachSecurityPolicyResult> AttachSecurityPolicyFuture(
                Request.AttachSecurityPolicyRequest request
        )
		{
			return new AttachSecurityPolicyTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.AttachSecurityPolicyResult> AttachSecurityPolicyAsync(
                Request.AttachSecurityPolicyRequest request
        )
		{
            AsyncResult<Result.AttachSecurityPolicyResult> result = null;
			await AttachSecurityPolicy(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
    #else
		public AttachSecurityPolicyTask AttachSecurityPolicyAsync(
                Request.AttachSecurityPolicyRequest request
        )
		{
			return new AttachSecurityPolicyTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
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


        public class DetachSecurityPolicyTask : Gs2RestSessionTask<DetachSecurityPolicyRequest, DetachSecurityPolicyResult>
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
                if (request.DryRun)
                {
                    sessionRequest.AddHeader("X-GS2-DRY-RUN", "true");
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
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.DetachSecurityPolicyResult>(task.Result, task.Error));
        }

		public IFuture<Result.DetachSecurityPolicyResult> DetachSecurityPolicyFuture(
                Request.DetachSecurityPolicyRequest request
        )
		{
			return new DetachSecurityPolicyTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.DetachSecurityPolicyResult> DetachSecurityPolicyAsync(
                Request.DetachSecurityPolicyRequest request
        )
		{
            AsyncResult<Result.DetachSecurityPolicyResult> result = null;
			await DetachSecurityPolicy(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
    #else
		public DetachSecurityPolicyTask DetachSecurityPolicyAsync(
                Request.DetachSecurityPolicyRequest request
        )
		{
			return new DetachSecurityPolicyTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
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


        public class LoginTask : Gs2RestSessionTask<LoginRequest, LoginResult>
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
                if (request.DryRun)
                {
                    sessionRequest.AddHeader("X-GS2-DRY-RUN", "true");
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
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.LoginResult>(task.Result, task.Error));
        }

		public IFuture<Result.LoginResult> LoginFuture(
                Request.LoginRequest request
        )
		{
			return new LoginTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.LoginResult> LoginAsync(
                Request.LoginRequest request
        )
		{
            AsyncResult<Result.LoginResult> result = null;
			await Login(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
    #else
		public LoginTask LoginAsync(
                Request.LoginRequest request
        )
		{
			return new LoginTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
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


        public class LoginByUserTask : Gs2RestSessionTask<LoginByUserRequest, LoginByUserResult>
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
                if (request.Otp != null)
                {
                    jsonWriter.WritePropertyName("otp");
                    jsonWriter.Write(request.Otp);
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
                if (request.DryRun)
                {
                    sessionRequest.AddHeader("X-GS2-DRY-RUN", "true");
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
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.LoginByUserResult>(task.Result, task.Error));
        }

		public IFuture<Result.LoginByUserResult> LoginByUserFuture(
                Request.LoginByUserRequest request
        )
		{
			return new LoginByUserTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.LoginByUserResult> LoginByUserAsync(
                Request.LoginByUserRequest request
        )
		{
            AsyncResult<Result.LoginByUserResult> result = null;
			await LoginByUser(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
    #else
		public LoginByUserTask LoginByUserAsync(
                Request.LoginByUserRequest request
        )
		{
			return new LoginByUserTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
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