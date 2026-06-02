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

#pragma warning disable CS0618 // Obsolete with a message

#if UNITY_2017_1_OR_NEWER
using UnityEngine.Events;
using UnityEngine.Networking;
    #if GS2_ENABLE_UNITASK
using Cysharp.Threading.Tasks;
    #endif
#else
using System.Web;
using System.Net.Http;
#endif

using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using Gs2.Core;
using Gs2.Core.Model;
using Gs2.Core.Net;
using Gs2.Core.Util;
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
        ) =>
            new DescribeUsersTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.DescribeUsersResult> DescribeUsersFuture(
                Request.DescribeUsersRequest request
        ) =>
            new DescribeUsersTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.DescribeUsersResult> DescribeUsersAsync(
                Request.DescribeUsersRequest request
        ) =>
            new DescribeUsersTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.DescribeUsersResult>();
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
		public Task<Result.DescribeUsersResult> DescribeUsersAsync(
                Request.DescribeUsersRequest request
        ) =>
            new DescribeUsersTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
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
        ) =>
            new CreateUserTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.CreateUserResult> CreateUserFuture(
                Request.CreateUserRequest request
        ) =>
            new CreateUserTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.CreateUserResult> CreateUserAsync(
                Request.CreateUserRequest request
        ) =>
            new CreateUserTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.CreateUserResult>();
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
		public Task<Result.CreateUserResult> CreateUserAsync(
                Request.CreateUserRequest request
        ) =>
            new CreateUserTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
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
        ) =>
            new UpdateUserTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.UpdateUserResult> UpdateUserFuture(
                Request.UpdateUserRequest request
        ) =>
            new UpdateUserTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.UpdateUserResult> UpdateUserAsync(
                Request.UpdateUserRequest request
        ) =>
            new UpdateUserTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.UpdateUserResult>();
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
		public Task<Result.UpdateUserResult> UpdateUserAsync(
                Request.UpdateUserRequest request
        ) =>
            new UpdateUserTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
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
        ) =>
            new GetUserTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.GetUserResult> GetUserFuture(
                Request.GetUserRequest request
        ) =>
            new GetUserTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.GetUserResult> GetUserAsync(
                Request.GetUserRequest request
        ) =>
            new GetUserTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.GetUserResult>();
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
		public Task<Result.GetUserResult> GetUserAsync(
                Request.GetUserRequest request
        ) =>
            new GetUserTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
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
        ) =>
            new DeleteUserTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.DeleteUserResult> DeleteUserFuture(
                Request.DeleteUserRequest request
        ) =>
            new DeleteUserTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.DeleteUserResult> DeleteUserAsync(
                Request.DeleteUserRequest request
        ) =>
            new DeleteUserTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.DeleteUserResult>();
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
		public Task<Result.DeleteUserResult> DeleteUserAsync(
                Request.DeleteUserRequest request
        ) =>
            new DeleteUserTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
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
        ) =>
            new DescribeSecurityPoliciesTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.DescribeSecurityPoliciesResult> DescribeSecurityPoliciesFuture(
                Request.DescribeSecurityPoliciesRequest request
        ) =>
            new DescribeSecurityPoliciesTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.DescribeSecurityPoliciesResult> DescribeSecurityPoliciesAsync(
                Request.DescribeSecurityPoliciesRequest request
        ) =>
            new DescribeSecurityPoliciesTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.DescribeSecurityPoliciesResult>();
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
		public Task<Result.DescribeSecurityPoliciesResult> DescribeSecurityPoliciesAsync(
                Request.DescribeSecurityPoliciesRequest request
        ) =>
            new DescribeSecurityPoliciesTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
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
        ) =>
            new DescribeCommonSecurityPoliciesTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.DescribeCommonSecurityPoliciesResult> DescribeCommonSecurityPoliciesFuture(
                Request.DescribeCommonSecurityPoliciesRequest request
        ) =>
            new DescribeCommonSecurityPoliciesTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.DescribeCommonSecurityPoliciesResult> DescribeCommonSecurityPoliciesAsync(
                Request.DescribeCommonSecurityPoliciesRequest request
        ) =>
            new DescribeCommonSecurityPoliciesTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.DescribeCommonSecurityPoliciesResult>();
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
		public Task<Result.DescribeCommonSecurityPoliciesResult> DescribeCommonSecurityPoliciesAsync(
                Request.DescribeCommonSecurityPoliciesRequest request
        ) =>
            new DescribeCommonSecurityPoliciesTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
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
        ) =>
            new CreateSecurityPolicyTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.CreateSecurityPolicyResult> CreateSecurityPolicyFuture(
                Request.CreateSecurityPolicyRequest request
        ) =>
            new CreateSecurityPolicyTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.CreateSecurityPolicyResult> CreateSecurityPolicyAsync(
                Request.CreateSecurityPolicyRequest request
        ) =>
            new CreateSecurityPolicyTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.CreateSecurityPolicyResult>();
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
		public Task<Result.CreateSecurityPolicyResult> CreateSecurityPolicyAsync(
                Request.CreateSecurityPolicyRequest request
        ) =>
            new CreateSecurityPolicyTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
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
        ) =>
            new UpdateSecurityPolicyTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.UpdateSecurityPolicyResult> UpdateSecurityPolicyFuture(
                Request.UpdateSecurityPolicyRequest request
        ) =>
            new UpdateSecurityPolicyTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.UpdateSecurityPolicyResult> UpdateSecurityPolicyAsync(
                Request.UpdateSecurityPolicyRequest request
        ) =>
            new UpdateSecurityPolicyTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.UpdateSecurityPolicyResult>();
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
		public Task<Result.UpdateSecurityPolicyResult> UpdateSecurityPolicyAsync(
                Request.UpdateSecurityPolicyRequest request
        ) =>
            new UpdateSecurityPolicyTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
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
        ) =>
            new GetSecurityPolicyTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.GetSecurityPolicyResult> GetSecurityPolicyFuture(
                Request.GetSecurityPolicyRequest request
        ) =>
            new GetSecurityPolicyTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.GetSecurityPolicyResult> GetSecurityPolicyAsync(
                Request.GetSecurityPolicyRequest request
        ) =>
            new GetSecurityPolicyTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.GetSecurityPolicyResult>();
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
		public Task<Result.GetSecurityPolicyResult> GetSecurityPolicyAsync(
                Request.GetSecurityPolicyRequest request
        ) =>
            new GetSecurityPolicyTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
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
        ) =>
            new DeleteSecurityPolicyTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.DeleteSecurityPolicyResult> DeleteSecurityPolicyFuture(
                Request.DeleteSecurityPolicyRequest request
        ) =>
            new DeleteSecurityPolicyTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.DeleteSecurityPolicyResult> DeleteSecurityPolicyAsync(
                Request.DeleteSecurityPolicyRequest request
        ) =>
            new DeleteSecurityPolicyTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.DeleteSecurityPolicyResult>();
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
		public Task<Result.DeleteSecurityPolicyResult> DeleteSecurityPolicyAsync(
                Request.DeleteSecurityPolicyRequest request
        ) =>
            new DeleteSecurityPolicyTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
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
        ) =>
            new DescribeIdentifiersTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.DescribeIdentifiersResult> DescribeIdentifiersFuture(
                Request.DescribeIdentifiersRequest request
        ) =>
            new DescribeIdentifiersTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.DescribeIdentifiersResult> DescribeIdentifiersAsync(
                Request.DescribeIdentifiersRequest request
        ) =>
            new DescribeIdentifiersTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.DescribeIdentifiersResult>();
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
		public Task<Result.DescribeIdentifiersResult> DescribeIdentifiersAsync(
                Request.DescribeIdentifiersRequest request
        ) =>
            new DescribeIdentifiersTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
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
        ) =>
            new CreateIdentifierTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.CreateIdentifierResult> CreateIdentifierFuture(
                Request.CreateIdentifierRequest request
        ) =>
            new CreateIdentifierTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.CreateIdentifierResult> CreateIdentifierAsync(
                Request.CreateIdentifierRequest request
        ) =>
            new CreateIdentifierTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.CreateIdentifierResult>();
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
		public Task<Result.CreateIdentifierResult> CreateIdentifierAsync(
                Request.CreateIdentifierRequest request
        ) =>
            new CreateIdentifierTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
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
        ) =>
            new GetIdentifierTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.GetIdentifierResult> GetIdentifierFuture(
                Request.GetIdentifierRequest request
        ) =>
            new GetIdentifierTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.GetIdentifierResult> GetIdentifierAsync(
                Request.GetIdentifierRequest request
        ) =>
            new GetIdentifierTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.GetIdentifierResult>();
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
		public Task<Result.GetIdentifierResult> GetIdentifierAsync(
                Request.GetIdentifierRequest request
        ) =>
            new GetIdentifierTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
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
        ) =>
            new DeleteIdentifierTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.DeleteIdentifierResult> DeleteIdentifierFuture(
                Request.DeleteIdentifierRequest request
        ) =>
            new DeleteIdentifierTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.DeleteIdentifierResult> DeleteIdentifierAsync(
                Request.DeleteIdentifierRequest request
        ) =>
            new DeleteIdentifierTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.DeleteIdentifierResult>();
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
		public Task<Result.DeleteIdentifierResult> DeleteIdentifierAsync(
                Request.DeleteIdentifierRequest request
        ) =>
            new DeleteIdentifierTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
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
        ) =>
            new DescribeAttachedGuardsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.DescribeAttachedGuardsResult> DescribeAttachedGuardsFuture(
                Request.DescribeAttachedGuardsRequest request
        ) =>
            new DescribeAttachedGuardsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.DescribeAttachedGuardsResult> DescribeAttachedGuardsAsync(
                Request.DescribeAttachedGuardsRequest request
        ) =>
            new DescribeAttachedGuardsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.DescribeAttachedGuardsResult>();
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
		public Task<Result.DescribeAttachedGuardsResult> DescribeAttachedGuardsAsync(
                Request.DescribeAttachedGuardsRequest request
        ) =>
            new DescribeAttachedGuardsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
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
        ) =>
            new AttachGuardTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.AttachGuardResult> AttachGuardFuture(
                Request.AttachGuardRequest request
        ) =>
            new AttachGuardTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.AttachGuardResult> AttachGuardAsync(
                Request.AttachGuardRequest request
        ) =>
            new AttachGuardTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.AttachGuardResult>();
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
		public Task<Result.AttachGuardResult> AttachGuardAsync(
                Request.AttachGuardRequest request
        ) =>
            new AttachGuardTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
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
        ) =>
            new DetachGuardTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.DetachGuardResult> DetachGuardFuture(
                Request.DetachGuardRequest request
        ) =>
            new DetachGuardTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.DetachGuardResult> DetachGuardAsync(
                Request.DetachGuardRequest request
        ) =>
            new DetachGuardTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.DetachGuardResult>();
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
		public Task<Result.DetachGuardResult> DetachGuardAsync(
                Request.DetachGuardRequest request
        ) =>
            new DetachGuardTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
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
        ) =>
            new CreatePasswordTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.CreatePasswordResult> CreatePasswordFuture(
                Request.CreatePasswordRequest request
        ) =>
            new CreatePasswordTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.CreatePasswordResult> CreatePasswordAsync(
                Request.CreatePasswordRequest request
        ) =>
            new CreatePasswordTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.CreatePasswordResult>();
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
		public Task<Result.CreatePasswordResult> CreatePasswordAsync(
                Request.CreatePasswordRequest request
        ) =>
            new CreatePasswordTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
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
        ) =>
            new GetPasswordTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.GetPasswordResult> GetPasswordFuture(
                Request.GetPasswordRequest request
        ) =>
            new GetPasswordTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.GetPasswordResult> GetPasswordAsync(
                Request.GetPasswordRequest request
        ) =>
            new GetPasswordTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.GetPasswordResult>();
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
		public Task<Result.GetPasswordResult> GetPasswordAsync(
                Request.GetPasswordRequest request
        ) =>
            new GetPasswordTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
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
        ) =>
            new EnableMfaTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.EnableMfaResult> EnableMfaFuture(
                Request.EnableMfaRequest request
        ) =>
            new EnableMfaTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.EnableMfaResult> EnableMfaAsync(
                Request.EnableMfaRequest request
        ) =>
            new EnableMfaTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.EnableMfaResult>();
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
		public Task<Result.EnableMfaResult> EnableMfaAsync(
                Request.EnableMfaRequest request
        ) =>
            new EnableMfaTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
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
        ) =>
            new ChallengeMfaTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.ChallengeMfaResult> ChallengeMfaFuture(
                Request.ChallengeMfaRequest request
        ) =>
            new ChallengeMfaTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.ChallengeMfaResult> ChallengeMfaAsync(
                Request.ChallengeMfaRequest request
        ) =>
            new ChallengeMfaTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.ChallengeMfaResult>();
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
		public Task<Result.ChallengeMfaResult> ChallengeMfaAsync(
                Request.ChallengeMfaRequest request
        ) =>
            new ChallengeMfaTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
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
        ) =>
            new DisableMfaTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.DisableMfaResult> DisableMfaFuture(
                Request.DisableMfaRequest request
        ) =>
            new DisableMfaTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.DisableMfaResult> DisableMfaAsync(
                Request.DisableMfaRequest request
        ) =>
            new DisableMfaTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.DisableMfaResult>();
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
		public Task<Result.DisableMfaResult> DisableMfaAsync(
                Request.DisableMfaRequest request
        ) =>
            new DisableMfaTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
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
        ) =>
            new DeletePasswordTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.DeletePasswordResult> DeletePasswordFuture(
                Request.DeletePasswordRequest request
        ) =>
            new DeletePasswordTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.DeletePasswordResult> DeletePasswordAsync(
                Request.DeletePasswordRequest request
        ) =>
            new DeletePasswordTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.DeletePasswordResult>();
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
		public Task<Result.DeletePasswordResult> DeletePasswordAsync(
                Request.DeletePasswordRequest request
        ) =>
            new DeletePasswordTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
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
        ) =>
            new GetHasSecurityPolicyTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.GetHasSecurityPolicyResult> GetHasSecurityPolicyFuture(
                Request.GetHasSecurityPolicyRequest request
        ) =>
            new GetHasSecurityPolicyTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.GetHasSecurityPolicyResult> GetHasSecurityPolicyAsync(
                Request.GetHasSecurityPolicyRequest request
        ) =>
            new GetHasSecurityPolicyTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.GetHasSecurityPolicyResult>();
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
		public Task<Result.GetHasSecurityPolicyResult> GetHasSecurityPolicyAsync(
                Request.GetHasSecurityPolicyRequest request
        ) =>
            new GetHasSecurityPolicyTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
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
        ) =>
            new AttachSecurityPolicyTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.AttachSecurityPolicyResult> AttachSecurityPolicyFuture(
                Request.AttachSecurityPolicyRequest request
        ) =>
            new AttachSecurityPolicyTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.AttachSecurityPolicyResult> AttachSecurityPolicyAsync(
                Request.AttachSecurityPolicyRequest request
        ) =>
            new AttachSecurityPolicyTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.AttachSecurityPolicyResult>();
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
		public Task<Result.AttachSecurityPolicyResult> AttachSecurityPolicyAsync(
                Request.AttachSecurityPolicyRequest request
        ) =>
            new AttachSecurityPolicyTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
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
        ) =>
            new DetachSecurityPolicyTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.DetachSecurityPolicyResult> DetachSecurityPolicyFuture(
                Request.DetachSecurityPolicyRequest request
        ) =>
            new DetachSecurityPolicyTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.DetachSecurityPolicyResult> DetachSecurityPolicyAsync(
                Request.DetachSecurityPolicyRequest request
        ) =>
            new DetachSecurityPolicyTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.DetachSecurityPolicyResult>();
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
		public Task<Result.DetachSecurityPolicyResult> DetachSecurityPolicyAsync(
                Request.DetachSecurityPolicyRequest request
        ) =>
            new DetachSecurityPolicyTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
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
                    jsonWriter.WritePropertyName("clientId");
                    jsonWriter.Write(request.ClientId);
                }
                if (request.ClientSecret != null)
                {
                    jsonWriter.WritePropertyName("clientSecret");
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
        ) =>
            new LoginTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.LoginResult> LoginFuture(
                Request.LoginRequest request
        ) =>
            new LoginTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.LoginResult> LoginAsync(
                Request.LoginRequest request
        ) =>
            new LoginTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.LoginResult>();
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
		public Task<Result.LoginResult> LoginAsync(
                Request.LoginRequest request
        ) =>
            new LoginTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
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
        ) =>
            new LoginByUserTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.LoginByUserResult> LoginByUserFuture(
                Request.LoginByUserRequest request
        ) =>
            new LoginByUserTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.LoginByUserResult> LoginByUserAsync(
                Request.LoginByUserRequest request
        ) =>
            new LoginByUserTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.LoginByUserResult>();
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
		public Task<Result.LoginByUserResult> LoginByUserAsync(
                Request.LoginByUserRequest request
        ) =>
            new LoginByUserTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif
	}
}