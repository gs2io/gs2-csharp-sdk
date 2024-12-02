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
using Gs2.Gs2Auth.Request;
using Gs2.Gs2Auth.Result;
using Gs2.Util.LitJson;

namespace Gs2.Gs2Auth
{
	public class Gs2AuthRestClient : AbstractGs2Client
	{
#if UNITY_2017_1_OR_NEWER
		private readonly CertificateHandler _certificateHandler;
#endif

		public static string Endpoint = "auth";

        protected Gs2RestSession Gs2RestSession => (Gs2RestSession) Gs2Session;

		public Gs2AuthRestClient(Gs2RestSession Gs2RestSession) : base(Gs2RestSession)
		{

		}

#if UNITY_2017_1_OR_NEWER
		public Gs2AuthRestClient(Gs2RestSession gs2RestSession, CertificateHandler certificateHandler) : base(gs2RestSession)
		{
			_certificateHandler = certificateHandler;
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
                    .Replace("{service}", "auth")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/login";

                var sessionRequest = Factory.Post(url);

                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);
                jsonWriter.WriteObjectStart();
                if (request.UserId != null)
                {
                    jsonWriter.WritePropertyName("userId");
                    jsonWriter.Write(request.UserId);
                }
                if (request.TimeOffset != null)
                {
                    jsonWriter.WritePropertyName("timeOffset");
                    jsonWriter.Write(request.TimeOffset.ToString());
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
                if (request.TimeOffsetToken != null)
                {
                    sessionRequest.AddHeader("X-GS2-TIME-OFFSET-TOKEN", request.TimeOffsetToken);
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


        public class LoginBySignatureTask : Gs2RestSessionTask<LoginBySignatureRequest, LoginBySignatureResult>
        {
            public LoginBySignatureTask(IGs2Session session, RestSessionRequestFactory factory, LoginBySignatureRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(LoginBySignatureRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "auth")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/login/signed";

                var sessionRequest = Factory.Post(url);

                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);
                jsonWriter.WriteObjectStart();
                if (request.KeyId != null)
                {
                    jsonWriter.WritePropertyName("keyId");
                    jsonWriter.Write(request.KeyId);
                }
                if (request.Body != null)
                {
                    jsonWriter.WritePropertyName("body");
                    jsonWriter.Write(request.Body);
                }
                if (request.Signature != null)
                {
                    jsonWriter.WritePropertyName("signature");
                    jsonWriter.Write(request.Signature);
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
		public IEnumerator LoginBySignature(
                Request.LoginBySignatureRequest request,
                UnityAction<AsyncResult<Result.LoginBySignatureResult>> callback
        )
		{
			var task = new LoginBySignatureTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.LoginBySignatureResult>(task.Result, task.Error));
        }

		public IFuture<Result.LoginBySignatureResult> LoginBySignatureFuture(
                Request.LoginBySignatureRequest request
        )
		{
			return new LoginBySignatureTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.LoginBySignatureResult> LoginBySignatureAsync(
                Request.LoginBySignatureRequest request
        )
		{
            AsyncResult<Result.LoginBySignatureResult> result = null;
			await LoginBySignature(
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
		public LoginBySignatureTask LoginBySignatureAsync(
                Request.LoginBySignatureRequest request
        )
		{
			return new LoginBySignatureTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.LoginBySignatureResult> LoginBySignatureAsync(
                Request.LoginBySignatureRequest request
        )
		{
			var task = new LoginBySignatureTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class FederationTask : Gs2RestSessionTask<FederationRequest, FederationResult>
        {
            public FederationTask(IGs2Session session, RestSessionRequestFactory factory, FederationRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(FederationRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "auth")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/federation";

                var sessionRequest = Factory.Post(url);

                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);
                jsonWriter.WriteObjectStart();
                if (request.OriginalUserId != null)
                {
                    jsonWriter.WritePropertyName("originalUserId");
                    jsonWriter.Write(request.OriginalUserId);
                }
                if (request.UserId != null)
                {
                    jsonWriter.WritePropertyName("userId");
                    jsonWriter.Write(request.UserId);
                }
                if (request.PolicyDocument != null)
                {
                    jsonWriter.WritePropertyName("policyDocument");
                    jsonWriter.Write(request.PolicyDocument);
                }
                if (request.TimeOffset != null)
                {
                    jsonWriter.WritePropertyName("timeOffset");
                    jsonWriter.Write(request.TimeOffset.ToString());
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
                if (request.TimeOffsetToken != null)
                {
                    sessionRequest.AddHeader("X-GS2-TIME-OFFSET-TOKEN", request.TimeOffsetToken);
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
		public IEnumerator Federation(
                Request.FederationRequest request,
                UnityAction<AsyncResult<Result.FederationResult>> callback
        )
		{
			var task = new FederationTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.FederationResult>(task.Result, task.Error));
        }

		public IFuture<Result.FederationResult> FederationFuture(
                Request.FederationRequest request
        )
		{
			return new FederationTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.FederationResult> FederationAsync(
                Request.FederationRequest request
        )
		{
            AsyncResult<Result.FederationResult> result = null;
			await Federation(
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
		public FederationTask FederationAsync(
                Request.FederationRequest request
        )
		{
			return new FederationTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.FederationResult> FederationAsync(
                Request.FederationRequest request
        )
		{
			var task = new FederationTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class IssueTimeOffsetTokenByUserIdTask : Gs2RestSessionTask<IssueTimeOffsetTokenByUserIdRequest, IssueTimeOffsetTokenByUserIdResult>
        {
            public IssueTimeOffsetTokenByUserIdTask(IGs2Session session, RestSessionRequestFactory factory, IssueTimeOffsetTokenByUserIdRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(IssueTimeOffsetTokenByUserIdRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "auth")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/timeoffset/token";

                var sessionRequest = Factory.Post(url);

                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);
                jsonWriter.WriteObjectStart();
                if (request.UserId != null)
                {
                    jsonWriter.WritePropertyName("userId");
                    jsonWriter.Write(request.UserId);
                }
                if (request.TimeOffset != null)
                {
                    jsonWriter.WritePropertyName("timeOffset");
                    jsonWriter.Write(request.TimeOffset.ToString());
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
                if (request.TimeOffsetToken != null)
                {
                    sessionRequest.AddHeader("X-GS2-TIME-OFFSET-TOKEN", request.TimeOffsetToken);
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
		public IEnumerator IssueTimeOffsetTokenByUserId(
                Request.IssueTimeOffsetTokenByUserIdRequest request,
                UnityAction<AsyncResult<Result.IssueTimeOffsetTokenByUserIdResult>> callback
        )
		{
			var task = new IssueTimeOffsetTokenByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.IssueTimeOffsetTokenByUserIdResult>(task.Result, task.Error));
        }

		public IFuture<Result.IssueTimeOffsetTokenByUserIdResult> IssueTimeOffsetTokenByUserIdFuture(
                Request.IssueTimeOffsetTokenByUserIdRequest request
        )
		{
			return new IssueTimeOffsetTokenByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.IssueTimeOffsetTokenByUserIdResult> IssueTimeOffsetTokenByUserIdAsync(
                Request.IssueTimeOffsetTokenByUserIdRequest request
        )
		{
            AsyncResult<Result.IssueTimeOffsetTokenByUserIdResult> result = null;
			await IssueTimeOffsetTokenByUserId(
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
		public IssueTimeOffsetTokenByUserIdTask IssueTimeOffsetTokenByUserIdAsync(
                Request.IssueTimeOffsetTokenByUserIdRequest request
        )
		{
			return new IssueTimeOffsetTokenByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.IssueTimeOffsetTokenByUserIdResult> IssueTimeOffsetTokenByUserIdAsync(
                Request.IssueTimeOffsetTokenByUserIdRequest request
        )
		{
			var task = new IssueTimeOffsetTokenByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif
	}
}