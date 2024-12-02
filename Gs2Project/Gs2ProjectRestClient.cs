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
using Gs2.Gs2Project.Request;
using Gs2.Gs2Project.Result;
using Gs2.Util.LitJson;

namespace Gs2.Gs2Project
{
	public class Gs2ProjectRestClient : AbstractGs2Client
	{
#if UNITY_2017_1_OR_NEWER
		private readonly CertificateHandler _certificateHandler;
#endif

		public static string Endpoint = "project";

        protected Gs2RestSession Gs2RestSession => (Gs2RestSession) Gs2Session;

		public Gs2ProjectRestClient(Gs2RestSession Gs2RestSession) : base(Gs2RestSession)
		{

		}

#if UNITY_2017_1_OR_NEWER
		public Gs2ProjectRestClient(Gs2RestSession gs2RestSession, CertificateHandler certificateHandler) : base(gs2RestSession)
		{
			_certificateHandler = certificateHandler;
		}
#endif


        public class CreateAccountTask : Gs2RestSessionTask<CreateAccountRequest, CreateAccountResult>
        {
            public CreateAccountTask(IGs2Session session, RestSessionRequestFactory factory, CreateAccountRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(CreateAccountRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "project")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/account";

                var sessionRequest = Factory.Post(url);

                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);
                jsonWriter.WriteObjectStart();
                if (request.Email != null)
                {
                    jsonWriter.WritePropertyName("email");
                    jsonWriter.Write(request.Email);
                }
                if (request.FullName != null)
                {
                    jsonWriter.WritePropertyName("fullName");
                    jsonWriter.Write(request.FullName);
                }
                if (request.CompanyName != null)
                {
                    jsonWriter.WritePropertyName("companyName");
                    jsonWriter.Write(request.CompanyName);
                }
                if (request.Password != null)
                {
                    jsonWriter.WritePropertyName("password");
                    jsonWriter.Write(request.Password);
                }
                if (request.Lang != null)
                {
                    jsonWriter.WritePropertyName("lang");
                    jsonWriter.Write(request.Lang);
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
		public IEnumerator CreateAccount(
                Request.CreateAccountRequest request,
                UnityAction<AsyncResult<Result.CreateAccountResult>> callback
        )
		{
			var task = new CreateAccountTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.CreateAccountResult>(task.Result, task.Error));
        }

		public IFuture<Result.CreateAccountResult> CreateAccountFuture(
                Request.CreateAccountRequest request
        )
		{
			return new CreateAccountTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.CreateAccountResult> CreateAccountAsync(
                Request.CreateAccountRequest request
        )
		{
            AsyncResult<Result.CreateAccountResult> result = null;
			await CreateAccount(
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
		public CreateAccountTask CreateAccountAsync(
                Request.CreateAccountRequest request
        )
		{
			return new CreateAccountTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.CreateAccountResult> CreateAccountAsync(
                Request.CreateAccountRequest request
        )
		{
			var task = new CreateAccountTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class VerifyTask : Gs2RestSessionTask<VerifyRequest, VerifyResult>
        {
            public VerifyTask(IGs2Session session, RestSessionRequestFactory factory, VerifyRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(VerifyRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "project")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/account/verify";

                var sessionRequest = Factory.Post(url);

                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);
                jsonWriter.WriteObjectStart();
                if (request.VerifyToken != null)
                {
                    jsonWriter.WritePropertyName("verifyToken");
                    jsonWriter.Write(request.VerifyToken);
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
		public IEnumerator Verify(
                Request.VerifyRequest request,
                UnityAction<AsyncResult<Result.VerifyResult>> callback
        )
		{
			var task = new VerifyTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.VerifyResult>(task.Result, task.Error));
        }

		public IFuture<Result.VerifyResult> VerifyFuture(
                Request.VerifyRequest request
        )
		{
			return new VerifyTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.VerifyResult> VerifyAsync(
                Request.VerifyRequest request
        )
		{
            AsyncResult<Result.VerifyResult> result = null;
			await Verify(
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
		public VerifyTask VerifyAsync(
                Request.VerifyRequest request
        )
		{
			return new VerifyTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.VerifyResult> VerifyAsync(
                Request.VerifyRequest request
        )
		{
			var task = new VerifyTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class SignInTask : Gs2RestSessionTask<SignInRequest, SignInResult>
        {
            public SignInTask(IGs2Session session, RestSessionRequestFactory factory, SignInRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(SignInRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "project")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/account/signIn";

                var sessionRequest = Factory.Post(url);

                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);
                jsonWriter.WriteObjectStart();
                if (request.Email != null)
                {
                    jsonWriter.WritePropertyName("email");
                    jsonWriter.Write(request.Email);
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
		public IEnumerator SignIn(
                Request.SignInRequest request,
                UnityAction<AsyncResult<Result.SignInResult>> callback
        )
		{
			var task = new SignInTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.SignInResult>(task.Result, task.Error));
        }

		public IFuture<Result.SignInResult> SignInFuture(
                Request.SignInRequest request
        )
		{
			return new SignInTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.SignInResult> SignInAsync(
                Request.SignInRequest request
        )
		{
            AsyncResult<Result.SignInResult> result = null;
			await SignIn(
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
		public SignInTask SignInAsync(
                Request.SignInRequest request
        )
		{
			return new SignInTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.SignInResult> SignInAsync(
                Request.SignInRequest request
        )
		{
			var task = new SignInTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class ForgetTask : Gs2RestSessionTask<ForgetRequest, ForgetResult>
        {
            public ForgetTask(IGs2Session session, RestSessionRequestFactory factory, ForgetRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(ForgetRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "project")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/account/forget";

                var sessionRequest = Factory.Post(url);

                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);
                jsonWriter.WriteObjectStart();
                if (request.Email != null)
                {
                    jsonWriter.WritePropertyName("email");
                    jsonWriter.Write(request.Email);
                }
                if (request.Lang != null)
                {
                    jsonWriter.WritePropertyName("lang");
                    jsonWriter.Write(request.Lang);
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
		public IEnumerator Forget(
                Request.ForgetRequest request,
                UnityAction<AsyncResult<Result.ForgetResult>> callback
        )
		{
			var task = new ForgetTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.ForgetResult>(task.Result, task.Error));
        }

		public IFuture<Result.ForgetResult> ForgetFuture(
                Request.ForgetRequest request
        )
		{
			return new ForgetTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.ForgetResult> ForgetAsync(
                Request.ForgetRequest request
        )
		{
            AsyncResult<Result.ForgetResult> result = null;
			await Forget(
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
		public ForgetTask ForgetAsync(
                Request.ForgetRequest request
        )
		{
			return new ForgetTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.ForgetResult> ForgetAsync(
                Request.ForgetRequest request
        )
		{
			var task = new ForgetTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class IssuePasswordTask : Gs2RestSessionTask<IssuePasswordRequest, IssuePasswordResult>
        {
            public IssuePasswordTask(IGs2Session session, RestSessionRequestFactory factory, IssuePasswordRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(IssuePasswordRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "project")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/account/password/issue";

                var sessionRequest = Factory.Post(url);

                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);
                jsonWriter.WriteObjectStart();
                if (request.IssuePasswordToken != null)
                {
                    jsonWriter.WritePropertyName("issuePasswordToken");
                    jsonWriter.Write(request.IssuePasswordToken);
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
		public IEnumerator IssuePassword(
                Request.IssuePasswordRequest request,
                UnityAction<AsyncResult<Result.IssuePasswordResult>> callback
        )
		{
			var task = new IssuePasswordTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.IssuePasswordResult>(task.Result, task.Error));
        }

		public IFuture<Result.IssuePasswordResult> IssuePasswordFuture(
                Request.IssuePasswordRequest request
        )
		{
			return new IssuePasswordTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.IssuePasswordResult> IssuePasswordAsync(
                Request.IssuePasswordRequest request
        )
		{
            AsyncResult<Result.IssuePasswordResult> result = null;
			await IssuePassword(
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
		public IssuePasswordTask IssuePasswordAsync(
                Request.IssuePasswordRequest request
        )
		{
			return new IssuePasswordTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.IssuePasswordResult> IssuePasswordAsync(
                Request.IssuePasswordRequest request
        )
		{
			var task = new IssuePasswordTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class UpdateAccountTask : Gs2RestSessionTask<UpdateAccountRequest, UpdateAccountResult>
        {
            public UpdateAccountTask(IGs2Session session, RestSessionRequestFactory factory, UpdateAccountRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(UpdateAccountRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "project")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/account";

                var sessionRequest = Factory.Put(url);

                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);
                jsonWriter.WriteObjectStart();
                if (request.Email != null)
                {
                    jsonWriter.WritePropertyName("email");
                    jsonWriter.Write(request.Email);
                }
                if (request.FullName != null)
                {
                    jsonWriter.WritePropertyName("fullName");
                    jsonWriter.Write(request.FullName);
                }
                if (request.CompanyName != null)
                {
                    jsonWriter.WritePropertyName("companyName");
                    jsonWriter.Write(request.CompanyName);
                }
                if (request.Password != null)
                {
                    jsonWriter.WritePropertyName("password");
                    jsonWriter.Write(request.Password);
                }
                if (request.AccountToken != null)
                {
                    jsonWriter.WritePropertyName("accountToken");
                    jsonWriter.Write(request.AccountToken);
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
		public IEnumerator UpdateAccount(
                Request.UpdateAccountRequest request,
                UnityAction<AsyncResult<Result.UpdateAccountResult>> callback
        )
		{
			var task = new UpdateAccountTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.UpdateAccountResult>(task.Result, task.Error));
        }

		public IFuture<Result.UpdateAccountResult> UpdateAccountFuture(
                Request.UpdateAccountRequest request
        )
		{
			return new UpdateAccountTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.UpdateAccountResult> UpdateAccountAsync(
                Request.UpdateAccountRequest request
        )
		{
            AsyncResult<Result.UpdateAccountResult> result = null;
			await UpdateAccount(
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
		public UpdateAccountTask UpdateAccountAsync(
                Request.UpdateAccountRequest request
        )
		{
			return new UpdateAccountTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.UpdateAccountResult> UpdateAccountAsync(
                Request.UpdateAccountRequest request
        )
		{
			var task = new UpdateAccountTask(
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
                    .Replace("{service}", "project")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/account/mfa";

                var sessionRequest = Factory.Post(url);

                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);
                jsonWriter.WriteObjectStart();
                if (request.AccountToken != null)
                {
                    jsonWriter.WritePropertyName("accountToken");
                    jsonWriter.Write(request.AccountToken);
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
                    .Replace("{service}", "project")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/account/mfa/challenge";

                var sessionRequest = Factory.Post(url);

                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);
                jsonWriter.WriteObjectStart();
                if (request.AccountToken != null)
                {
                    jsonWriter.WritePropertyName("accountToken");
                    jsonWriter.Write(request.AccountToken);
                }
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
                    .Replace("{service}", "project")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/account/mfa";

                var sessionRequest = Factory.Delete(url);
                if (request.ContextStack != null)
                {
                    sessionRequest.AddQueryString("contextStack", request.ContextStack);
                }
                if (request.AccountToken != null) {
                    sessionRequest.AddQueryString("accountToken", $"{request.AccountToken}");
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


        public class DeleteAccountTask : Gs2RestSessionTask<DeleteAccountRequest, DeleteAccountResult>
        {
            public DeleteAccountTask(IGs2Session session, RestSessionRequestFactory factory, DeleteAccountRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DeleteAccountRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "project")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/account";

                var sessionRequest = Factory.Delete(url);
                if (request.ContextStack != null)
                {
                    sessionRequest.AddQueryString("contextStack", request.ContextStack);
                }
                if (request.AccountToken != null) {
                    sessionRequest.AddQueryString("accountToken", $"{request.AccountToken}");
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
		public IEnumerator DeleteAccount(
                Request.DeleteAccountRequest request,
                UnityAction<AsyncResult<Result.DeleteAccountResult>> callback
        )
		{
			var task = new DeleteAccountTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.DeleteAccountResult>(task.Result, task.Error));
        }

		public IFuture<Result.DeleteAccountResult> DeleteAccountFuture(
                Request.DeleteAccountRequest request
        )
		{
			return new DeleteAccountTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.DeleteAccountResult> DeleteAccountAsync(
                Request.DeleteAccountRequest request
        )
		{
            AsyncResult<Result.DeleteAccountResult> result = null;
			await DeleteAccount(
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
		public DeleteAccountTask DeleteAccountAsync(
                Request.DeleteAccountRequest request
        )
		{
			return new DeleteAccountTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.DeleteAccountResult> DeleteAccountAsync(
                Request.DeleteAccountRequest request
        )
		{
			var task = new DeleteAccountTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class DescribeProjectsTask : Gs2RestSessionTask<DescribeProjectsRequest, DescribeProjectsResult>
        {
            public DescribeProjectsTask(IGs2Session session, RestSessionRequestFactory factory, DescribeProjectsRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DescribeProjectsRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "project")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/account/me/project";

                var sessionRequest = Factory.Get(url);
                if (request.ContextStack != null)
                {
                    sessionRequest.AddQueryString("contextStack", request.ContextStack);
                }
                if (request.AccountToken != null) {
                    sessionRequest.AddQueryString("accountToken", $"{request.AccountToken}");
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
		public IEnumerator DescribeProjects(
                Request.DescribeProjectsRequest request,
                UnityAction<AsyncResult<Result.DescribeProjectsResult>> callback
        )
		{
			var task = new DescribeProjectsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.DescribeProjectsResult>(task.Result, task.Error));
        }

		public IFuture<Result.DescribeProjectsResult> DescribeProjectsFuture(
                Request.DescribeProjectsRequest request
        )
		{
			return new DescribeProjectsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.DescribeProjectsResult> DescribeProjectsAsync(
                Request.DescribeProjectsRequest request
        )
		{
            AsyncResult<Result.DescribeProjectsResult> result = null;
			await DescribeProjects(
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
		public DescribeProjectsTask DescribeProjectsAsync(
                Request.DescribeProjectsRequest request
        )
		{
			return new DescribeProjectsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.DescribeProjectsResult> DescribeProjectsAsync(
                Request.DescribeProjectsRequest request
        )
		{
			var task = new DescribeProjectsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class CreateProjectTask : Gs2RestSessionTask<CreateProjectRequest, CreateProjectResult>
        {
            public CreateProjectTask(IGs2Session session, RestSessionRequestFactory factory, CreateProjectRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(CreateProjectRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "project")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/account/me/project";

                var sessionRequest = Factory.Post(url);

                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);
                jsonWriter.WriteObjectStart();
                if (request.AccountToken != null)
                {
                    jsonWriter.WritePropertyName("accountToken");
                    jsonWriter.Write(request.AccountToken);
                }
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
                if (request.Plan != null)
                {
                    jsonWriter.WritePropertyName("plan");
                    jsonWriter.Write(request.Plan);
                }
                if (request.Currency != null)
                {
                    jsonWriter.WritePropertyName("currency");
                    jsonWriter.Write(request.Currency);
                }
                if (request.ActivateRegionName != null)
                {
                    jsonWriter.WritePropertyName("activateRegionName");
                    jsonWriter.Write(request.ActivateRegionName);
                }
                if (request.BillingMethodName != null)
                {
                    jsonWriter.WritePropertyName("billingMethodName");
                    jsonWriter.Write(request.BillingMethodName);
                }
                if (request.EnableEventBridge != null)
                {
                    jsonWriter.WritePropertyName("enableEventBridge");
                    jsonWriter.Write(request.EnableEventBridge);
                }
                if (request.EventBridgeAwsAccountId != null)
                {
                    jsonWriter.WritePropertyName("eventBridgeAwsAccountId");
                    jsonWriter.Write(request.EventBridgeAwsAccountId);
                }
                if (request.EventBridgeAwsRegion != null)
                {
                    jsonWriter.WritePropertyName("eventBridgeAwsRegion");
                    jsonWriter.Write(request.EventBridgeAwsRegion);
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
		public IEnumerator CreateProject(
                Request.CreateProjectRequest request,
                UnityAction<AsyncResult<Result.CreateProjectResult>> callback
        )
		{
			var task = new CreateProjectTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.CreateProjectResult>(task.Result, task.Error));
        }

		public IFuture<Result.CreateProjectResult> CreateProjectFuture(
                Request.CreateProjectRequest request
        )
		{
			return new CreateProjectTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.CreateProjectResult> CreateProjectAsync(
                Request.CreateProjectRequest request
        )
		{
            AsyncResult<Result.CreateProjectResult> result = null;
			await CreateProject(
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
		public CreateProjectTask CreateProjectAsync(
                Request.CreateProjectRequest request
        )
		{
			return new CreateProjectTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.CreateProjectResult> CreateProjectAsync(
                Request.CreateProjectRequest request
        )
		{
			var task = new CreateProjectTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class GetProjectTask : Gs2RestSessionTask<GetProjectRequest, GetProjectResult>
        {
            public GetProjectTask(IGs2Session session, RestSessionRequestFactory factory, GetProjectRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(GetProjectRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "project")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/account/me/project/{projectName}";

                url = url.Replace("{projectName}", !string.IsNullOrEmpty(request.ProjectName) ? request.ProjectName.ToString() : "null");

                var sessionRequest = Factory.Get(url);
                if (request.ContextStack != null)
                {
                    sessionRequest.AddQueryString("contextStack", request.ContextStack);
                }
                if (request.AccountToken != null) {
                    sessionRequest.AddQueryString("accountToken", $"{request.AccountToken}");
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
		public IEnumerator GetProject(
                Request.GetProjectRequest request,
                UnityAction<AsyncResult<Result.GetProjectResult>> callback
        )
		{
			var task = new GetProjectTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.GetProjectResult>(task.Result, task.Error));
        }

		public IFuture<Result.GetProjectResult> GetProjectFuture(
                Request.GetProjectRequest request
        )
		{
			return new GetProjectTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.GetProjectResult> GetProjectAsync(
                Request.GetProjectRequest request
        )
		{
            AsyncResult<Result.GetProjectResult> result = null;
			await GetProject(
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
		public GetProjectTask GetProjectAsync(
                Request.GetProjectRequest request
        )
		{
			return new GetProjectTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.GetProjectResult> GetProjectAsync(
                Request.GetProjectRequest request
        )
		{
			var task = new GetProjectTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class GetProjectTokenTask : Gs2RestSessionTask<GetProjectTokenRequest, GetProjectTokenResult>
        {
            public GetProjectTokenTask(IGs2Session session, RestSessionRequestFactory factory, GetProjectTokenRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(GetProjectTokenRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "project")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/project/{projectName}/projectToken";

                url = url.Replace("{projectName}", !string.IsNullOrEmpty(request.ProjectName) ? request.ProjectName.ToString() : "null");

                var sessionRequest = Factory.Post(url);

                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);
                jsonWriter.WriteObjectStart();
                if (request.AccountToken != null)
                {
                    jsonWriter.WritePropertyName("accountToken");
                    jsonWriter.Write(request.AccountToken);
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
		public IEnumerator GetProjectToken(
                Request.GetProjectTokenRequest request,
                UnityAction<AsyncResult<Result.GetProjectTokenResult>> callback
        )
		{
			var task = new GetProjectTokenTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.GetProjectTokenResult>(task.Result, task.Error));
        }

		public IFuture<Result.GetProjectTokenResult> GetProjectTokenFuture(
                Request.GetProjectTokenRequest request
        )
		{
			return new GetProjectTokenTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.GetProjectTokenResult> GetProjectTokenAsync(
                Request.GetProjectTokenRequest request
        )
		{
            AsyncResult<Result.GetProjectTokenResult> result = null;
			await GetProjectToken(
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
		public GetProjectTokenTask GetProjectTokenAsync(
                Request.GetProjectTokenRequest request
        )
		{
			return new GetProjectTokenTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.GetProjectTokenResult> GetProjectTokenAsync(
                Request.GetProjectTokenRequest request
        )
		{
			var task = new GetProjectTokenTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class GetProjectTokenByIdentifierTask : Gs2RestSessionTask<GetProjectTokenByIdentifierRequest, GetProjectTokenByIdentifierResult>
        {
            public GetProjectTokenByIdentifierTask(IGs2Session session, RestSessionRequestFactory factory, GetProjectTokenByIdentifierRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(GetProjectTokenByIdentifierRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "project")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/account/{accountName}/project/{projectName}/user/{userName}/projectToken";

                url = url.Replace("{accountName}", !string.IsNullOrEmpty(request.AccountName) ? request.AccountName.ToString() : "null");
                url = url.Replace("{projectName}", !string.IsNullOrEmpty(request.ProjectName) ? request.ProjectName.ToString() : "null");
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
		public IEnumerator GetProjectTokenByIdentifier(
                Request.GetProjectTokenByIdentifierRequest request,
                UnityAction<AsyncResult<Result.GetProjectTokenByIdentifierResult>> callback
        )
		{
			var task = new GetProjectTokenByIdentifierTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.GetProjectTokenByIdentifierResult>(task.Result, task.Error));
        }

		public IFuture<Result.GetProjectTokenByIdentifierResult> GetProjectTokenByIdentifierFuture(
                Request.GetProjectTokenByIdentifierRequest request
        )
		{
			return new GetProjectTokenByIdentifierTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.GetProjectTokenByIdentifierResult> GetProjectTokenByIdentifierAsync(
                Request.GetProjectTokenByIdentifierRequest request
        )
		{
            AsyncResult<Result.GetProjectTokenByIdentifierResult> result = null;
			await GetProjectTokenByIdentifier(
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
		public GetProjectTokenByIdentifierTask GetProjectTokenByIdentifierAsync(
                Request.GetProjectTokenByIdentifierRequest request
        )
		{
			return new GetProjectTokenByIdentifierTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.GetProjectTokenByIdentifierResult> GetProjectTokenByIdentifierAsync(
                Request.GetProjectTokenByIdentifierRequest request
        )
		{
			var task = new GetProjectTokenByIdentifierTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class UpdateProjectTask : Gs2RestSessionTask<UpdateProjectRequest, UpdateProjectResult>
        {
            public UpdateProjectTask(IGs2Session session, RestSessionRequestFactory factory, UpdateProjectRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(UpdateProjectRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "project")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/account/me/project/{projectName}";

                url = url.Replace("{projectName}", !string.IsNullOrEmpty(request.ProjectName) ? request.ProjectName.ToString() : "null");

                var sessionRequest = Factory.Put(url);

                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);
                jsonWriter.WriteObjectStart();
                if (request.AccountToken != null)
                {
                    jsonWriter.WritePropertyName("accountToken");
                    jsonWriter.Write(request.AccountToken);
                }
                if (request.Description != null)
                {
                    jsonWriter.WritePropertyName("description");
                    jsonWriter.Write(request.Description);
                }
                if (request.Plan != null)
                {
                    jsonWriter.WritePropertyName("plan");
                    jsonWriter.Write(request.Plan);
                }
                if (request.BillingMethodName != null)
                {
                    jsonWriter.WritePropertyName("billingMethodName");
                    jsonWriter.Write(request.BillingMethodName);
                }
                if (request.EnableEventBridge != null)
                {
                    jsonWriter.WritePropertyName("enableEventBridge");
                    jsonWriter.Write(request.EnableEventBridge);
                }
                if (request.EventBridgeAwsAccountId != null)
                {
                    jsonWriter.WritePropertyName("eventBridgeAwsAccountId");
                    jsonWriter.Write(request.EventBridgeAwsAccountId);
                }
                if (request.EventBridgeAwsRegion != null)
                {
                    jsonWriter.WritePropertyName("eventBridgeAwsRegion");
                    jsonWriter.Write(request.EventBridgeAwsRegion);
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
		public IEnumerator UpdateProject(
                Request.UpdateProjectRequest request,
                UnityAction<AsyncResult<Result.UpdateProjectResult>> callback
        )
		{
			var task = new UpdateProjectTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.UpdateProjectResult>(task.Result, task.Error));
        }

		public IFuture<Result.UpdateProjectResult> UpdateProjectFuture(
                Request.UpdateProjectRequest request
        )
		{
			return new UpdateProjectTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.UpdateProjectResult> UpdateProjectAsync(
                Request.UpdateProjectRequest request
        )
		{
            AsyncResult<Result.UpdateProjectResult> result = null;
			await UpdateProject(
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
		public UpdateProjectTask UpdateProjectAsync(
                Request.UpdateProjectRequest request
        )
		{
			return new UpdateProjectTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.UpdateProjectResult> UpdateProjectAsync(
                Request.UpdateProjectRequest request
        )
		{
			var task = new UpdateProjectTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class ActivateRegionTask : Gs2RestSessionTask<ActivateRegionRequest, ActivateRegionResult>
        {
            public ActivateRegionTask(IGs2Session session, RestSessionRequestFactory factory, ActivateRegionRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(ActivateRegionRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "project")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/account/me/project/{projectName}/region/{regionName}/activate";

                url = url.Replace("{projectName}", !string.IsNullOrEmpty(request.ProjectName) ? request.ProjectName.ToString() : "null");
                url = url.Replace("{regionName}", !string.IsNullOrEmpty(request.RegionName) ? request.RegionName.ToString() : "null");

                var sessionRequest = Factory.Put(url);

                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);
                jsonWriter.WriteObjectStart();
                if (request.AccountToken != null)
                {
                    jsonWriter.WritePropertyName("accountToken");
                    jsonWriter.Write(request.AccountToken);
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
		public IEnumerator ActivateRegion(
                Request.ActivateRegionRequest request,
                UnityAction<AsyncResult<Result.ActivateRegionResult>> callback
        )
		{
			var task = new ActivateRegionTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.ActivateRegionResult>(task.Result, task.Error));
        }

		public IFuture<Result.ActivateRegionResult> ActivateRegionFuture(
                Request.ActivateRegionRequest request
        )
		{
			return new ActivateRegionTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.ActivateRegionResult> ActivateRegionAsync(
                Request.ActivateRegionRequest request
        )
		{
            AsyncResult<Result.ActivateRegionResult> result = null;
			await ActivateRegion(
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
		public ActivateRegionTask ActivateRegionAsync(
                Request.ActivateRegionRequest request
        )
		{
			return new ActivateRegionTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.ActivateRegionResult> ActivateRegionAsync(
                Request.ActivateRegionRequest request
        )
		{
			var task = new ActivateRegionTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class WaitActivateRegionTask : Gs2RestSessionTask<WaitActivateRegionRequest, WaitActivateRegionResult>
        {
            public WaitActivateRegionTask(IGs2Session session, RestSessionRequestFactory factory, WaitActivateRegionRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(WaitActivateRegionRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "project")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/account/me/project/{projectName}/region/{regionName}/activate/wait";

                url = url.Replace("{projectName}", !string.IsNullOrEmpty(request.ProjectName) ? request.ProjectName.ToString() : "null");
                url = url.Replace("{regionName}", !string.IsNullOrEmpty(request.RegionName) ? request.RegionName.ToString() : "null");

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
		public IEnumerator WaitActivateRegion(
                Request.WaitActivateRegionRequest request,
                UnityAction<AsyncResult<Result.WaitActivateRegionResult>> callback
        )
		{
			var task = new WaitActivateRegionTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.WaitActivateRegionResult>(task.Result, task.Error));
        }

		public IFuture<Result.WaitActivateRegionResult> WaitActivateRegionFuture(
                Request.WaitActivateRegionRequest request
        )
		{
			return new WaitActivateRegionTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.WaitActivateRegionResult> WaitActivateRegionAsync(
                Request.WaitActivateRegionRequest request
        )
		{
            AsyncResult<Result.WaitActivateRegionResult> result = null;
			await WaitActivateRegion(
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
		public WaitActivateRegionTask WaitActivateRegionAsync(
                Request.WaitActivateRegionRequest request
        )
		{
			return new WaitActivateRegionTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.WaitActivateRegionResult> WaitActivateRegionAsync(
                Request.WaitActivateRegionRequest request
        )
		{
			var task = new WaitActivateRegionTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class DeleteProjectTask : Gs2RestSessionTask<DeleteProjectRequest, DeleteProjectResult>
        {
            public DeleteProjectTask(IGs2Session session, RestSessionRequestFactory factory, DeleteProjectRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DeleteProjectRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "project")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/account/me/project/{projectName}";

                url = url.Replace("{projectName}", !string.IsNullOrEmpty(request.ProjectName) ? request.ProjectName.ToString() : "null");

                var sessionRequest = Factory.Delete(url);
                if (request.ContextStack != null)
                {
                    sessionRequest.AddQueryString("contextStack", request.ContextStack);
                }
                if (request.AccountToken != null) {
                    sessionRequest.AddQueryString("accountToken", $"{request.AccountToken}");
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
		public IEnumerator DeleteProject(
                Request.DeleteProjectRequest request,
                UnityAction<AsyncResult<Result.DeleteProjectResult>> callback
        )
		{
			var task = new DeleteProjectTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.DeleteProjectResult>(task.Result, task.Error));
        }

		public IFuture<Result.DeleteProjectResult> DeleteProjectFuture(
                Request.DeleteProjectRequest request
        )
		{
			return new DeleteProjectTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.DeleteProjectResult> DeleteProjectAsync(
                Request.DeleteProjectRequest request
        )
		{
            AsyncResult<Result.DeleteProjectResult> result = null;
			await DeleteProject(
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
		public DeleteProjectTask DeleteProjectAsync(
                Request.DeleteProjectRequest request
        )
		{
			return new DeleteProjectTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.DeleteProjectResult> DeleteProjectAsync(
                Request.DeleteProjectRequest request
        )
		{
			var task = new DeleteProjectTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class DescribeBillingMethodsTask : Gs2RestSessionTask<DescribeBillingMethodsRequest, DescribeBillingMethodsResult>
        {
            public DescribeBillingMethodsTask(IGs2Session session, RestSessionRequestFactory factory, DescribeBillingMethodsRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DescribeBillingMethodsRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "project")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/account/me/billingMethod";

                var sessionRequest = Factory.Get(url);
                if (request.ContextStack != null)
                {
                    sessionRequest.AddQueryString("contextStack", request.ContextStack);
                }
                if (request.AccountToken != null) {
                    sessionRequest.AddQueryString("accountToken", $"{request.AccountToken}");
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
		public IEnumerator DescribeBillingMethods(
                Request.DescribeBillingMethodsRequest request,
                UnityAction<AsyncResult<Result.DescribeBillingMethodsResult>> callback
        )
		{
			var task = new DescribeBillingMethodsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.DescribeBillingMethodsResult>(task.Result, task.Error));
        }

		public IFuture<Result.DescribeBillingMethodsResult> DescribeBillingMethodsFuture(
                Request.DescribeBillingMethodsRequest request
        )
		{
			return new DescribeBillingMethodsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.DescribeBillingMethodsResult> DescribeBillingMethodsAsync(
                Request.DescribeBillingMethodsRequest request
        )
		{
            AsyncResult<Result.DescribeBillingMethodsResult> result = null;
			await DescribeBillingMethods(
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
		public DescribeBillingMethodsTask DescribeBillingMethodsAsync(
                Request.DescribeBillingMethodsRequest request
        )
		{
			return new DescribeBillingMethodsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.DescribeBillingMethodsResult> DescribeBillingMethodsAsync(
                Request.DescribeBillingMethodsRequest request
        )
		{
			var task = new DescribeBillingMethodsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class CreateBillingMethodTask : Gs2RestSessionTask<CreateBillingMethodRequest, CreateBillingMethodResult>
        {
            public CreateBillingMethodTask(IGs2Session session, RestSessionRequestFactory factory, CreateBillingMethodRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(CreateBillingMethodRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "project")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/account/me/billingMethod";

                var sessionRequest = Factory.Post(url);

                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);
                jsonWriter.WriteObjectStart();
                if (request.AccountToken != null)
                {
                    jsonWriter.WritePropertyName("accountToken");
                    jsonWriter.Write(request.AccountToken);
                }
                if (request.Description != null)
                {
                    jsonWriter.WritePropertyName("description");
                    jsonWriter.Write(request.Description);
                }
                if (request.MethodType != null)
                {
                    jsonWriter.WritePropertyName("methodType");
                    jsonWriter.Write(request.MethodType);
                }
                if (request.CardCustomerId != null)
                {
                    jsonWriter.WritePropertyName("cardCustomerId");
                    jsonWriter.Write(request.CardCustomerId);
                }
                if (request.PartnerId != null)
                {
                    jsonWriter.WritePropertyName("partnerId");
                    jsonWriter.Write(request.PartnerId);
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
		public IEnumerator CreateBillingMethod(
                Request.CreateBillingMethodRequest request,
                UnityAction<AsyncResult<Result.CreateBillingMethodResult>> callback
        )
		{
			var task = new CreateBillingMethodTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.CreateBillingMethodResult>(task.Result, task.Error));
        }

		public IFuture<Result.CreateBillingMethodResult> CreateBillingMethodFuture(
                Request.CreateBillingMethodRequest request
        )
		{
			return new CreateBillingMethodTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.CreateBillingMethodResult> CreateBillingMethodAsync(
                Request.CreateBillingMethodRequest request
        )
		{
            AsyncResult<Result.CreateBillingMethodResult> result = null;
			await CreateBillingMethod(
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
		public CreateBillingMethodTask CreateBillingMethodAsync(
                Request.CreateBillingMethodRequest request
        )
		{
			return new CreateBillingMethodTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.CreateBillingMethodResult> CreateBillingMethodAsync(
                Request.CreateBillingMethodRequest request
        )
		{
			var task = new CreateBillingMethodTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class GetBillingMethodTask : Gs2RestSessionTask<GetBillingMethodRequest, GetBillingMethodResult>
        {
            public GetBillingMethodTask(IGs2Session session, RestSessionRequestFactory factory, GetBillingMethodRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(GetBillingMethodRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "project")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/account/me/billingMethod/{billingMethodName}";

                url = url.Replace("{billingMethodName}", !string.IsNullOrEmpty(request.BillingMethodName) ? request.BillingMethodName.ToString() : "null");

                var sessionRequest = Factory.Get(url);
                if (request.ContextStack != null)
                {
                    sessionRequest.AddQueryString("contextStack", request.ContextStack);
                }
                if (request.AccountToken != null) {
                    sessionRequest.AddQueryString("accountToken", $"{request.AccountToken}");
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
		public IEnumerator GetBillingMethod(
                Request.GetBillingMethodRequest request,
                UnityAction<AsyncResult<Result.GetBillingMethodResult>> callback
        )
		{
			var task = new GetBillingMethodTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.GetBillingMethodResult>(task.Result, task.Error));
        }

		public IFuture<Result.GetBillingMethodResult> GetBillingMethodFuture(
                Request.GetBillingMethodRequest request
        )
		{
			return new GetBillingMethodTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.GetBillingMethodResult> GetBillingMethodAsync(
                Request.GetBillingMethodRequest request
        )
		{
            AsyncResult<Result.GetBillingMethodResult> result = null;
			await GetBillingMethod(
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
		public GetBillingMethodTask GetBillingMethodAsync(
                Request.GetBillingMethodRequest request
        )
		{
			return new GetBillingMethodTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.GetBillingMethodResult> GetBillingMethodAsync(
                Request.GetBillingMethodRequest request
        )
		{
			var task = new GetBillingMethodTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class UpdateBillingMethodTask : Gs2RestSessionTask<UpdateBillingMethodRequest, UpdateBillingMethodResult>
        {
            public UpdateBillingMethodTask(IGs2Session session, RestSessionRequestFactory factory, UpdateBillingMethodRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(UpdateBillingMethodRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "project")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/account/me/billingMethod/{billingMethodName}";

                url = url.Replace("{billingMethodName}", !string.IsNullOrEmpty(request.BillingMethodName) ? request.BillingMethodName.ToString() : "null");

                var sessionRequest = Factory.Put(url);

                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);
                jsonWriter.WriteObjectStart();
                if (request.AccountToken != null)
                {
                    jsonWriter.WritePropertyName("accountToken");
                    jsonWriter.Write(request.AccountToken);
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
		public IEnumerator UpdateBillingMethod(
                Request.UpdateBillingMethodRequest request,
                UnityAction<AsyncResult<Result.UpdateBillingMethodResult>> callback
        )
		{
			var task = new UpdateBillingMethodTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.UpdateBillingMethodResult>(task.Result, task.Error));
        }

		public IFuture<Result.UpdateBillingMethodResult> UpdateBillingMethodFuture(
                Request.UpdateBillingMethodRequest request
        )
		{
			return new UpdateBillingMethodTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.UpdateBillingMethodResult> UpdateBillingMethodAsync(
                Request.UpdateBillingMethodRequest request
        )
		{
            AsyncResult<Result.UpdateBillingMethodResult> result = null;
			await UpdateBillingMethod(
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
		public UpdateBillingMethodTask UpdateBillingMethodAsync(
                Request.UpdateBillingMethodRequest request
        )
		{
			return new UpdateBillingMethodTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.UpdateBillingMethodResult> UpdateBillingMethodAsync(
                Request.UpdateBillingMethodRequest request
        )
		{
			var task = new UpdateBillingMethodTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class DeleteBillingMethodTask : Gs2RestSessionTask<DeleteBillingMethodRequest, DeleteBillingMethodResult>
        {
            public DeleteBillingMethodTask(IGs2Session session, RestSessionRequestFactory factory, DeleteBillingMethodRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DeleteBillingMethodRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "project")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/account/me/billingMethod/{billingMethodName}";

                url = url.Replace("{billingMethodName}", !string.IsNullOrEmpty(request.BillingMethodName) ? request.BillingMethodName.ToString() : "null");

                var sessionRequest = Factory.Delete(url);
                if (request.ContextStack != null)
                {
                    sessionRequest.AddQueryString("contextStack", request.ContextStack);
                }
                if (request.AccountToken != null) {
                    sessionRequest.AddQueryString("accountToken", $"{request.AccountToken}");
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
		public IEnumerator DeleteBillingMethod(
                Request.DeleteBillingMethodRequest request,
                UnityAction<AsyncResult<Result.DeleteBillingMethodResult>> callback
        )
		{
			var task = new DeleteBillingMethodTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.DeleteBillingMethodResult>(task.Result, task.Error));
        }

		public IFuture<Result.DeleteBillingMethodResult> DeleteBillingMethodFuture(
                Request.DeleteBillingMethodRequest request
        )
		{
			return new DeleteBillingMethodTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.DeleteBillingMethodResult> DeleteBillingMethodAsync(
                Request.DeleteBillingMethodRequest request
        )
		{
            AsyncResult<Result.DeleteBillingMethodResult> result = null;
			await DeleteBillingMethod(
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
		public DeleteBillingMethodTask DeleteBillingMethodAsync(
                Request.DeleteBillingMethodRequest request
        )
		{
			return new DeleteBillingMethodTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.DeleteBillingMethodResult> DeleteBillingMethodAsync(
                Request.DeleteBillingMethodRequest request
        )
		{
			var task = new DeleteBillingMethodTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class DescribeReceiptsTask : Gs2RestSessionTask<DescribeReceiptsRequest, DescribeReceiptsResult>
        {
            public DescribeReceiptsTask(IGs2Session session, RestSessionRequestFactory factory, DescribeReceiptsRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DescribeReceiptsRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "project")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/account/me/receipt";

                var sessionRequest = Factory.Get(url);
                if (request.ContextStack != null)
                {
                    sessionRequest.AddQueryString("contextStack", request.ContextStack);
                }
                if (request.AccountToken != null) {
                    sessionRequest.AddQueryString("accountToken", $"{request.AccountToken}");
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
		public IEnumerator DescribeReceipts(
                Request.DescribeReceiptsRequest request,
                UnityAction<AsyncResult<Result.DescribeReceiptsResult>> callback
        )
		{
			var task = new DescribeReceiptsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.DescribeReceiptsResult>(task.Result, task.Error));
        }

		public IFuture<Result.DescribeReceiptsResult> DescribeReceiptsFuture(
                Request.DescribeReceiptsRequest request
        )
		{
			return new DescribeReceiptsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.DescribeReceiptsResult> DescribeReceiptsAsync(
                Request.DescribeReceiptsRequest request
        )
		{
            AsyncResult<Result.DescribeReceiptsResult> result = null;
			await DescribeReceipts(
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
		public DescribeReceiptsTask DescribeReceiptsAsync(
                Request.DescribeReceiptsRequest request
        )
		{
			return new DescribeReceiptsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.DescribeReceiptsResult> DescribeReceiptsAsync(
                Request.DescribeReceiptsRequest request
        )
		{
			var task = new DescribeReceiptsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class DescribeBillingsTask : Gs2RestSessionTask<DescribeBillingsRequest, DescribeBillingsResult>
        {
            public DescribeBillingsTask(IGs2Session session, RestSessionRequestFactory factory, DescribeBillingsRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DescribeBillingsRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "project")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/account/me/billing/{projectName}/{year}/{month}";

                url = url.Replace("{projectName}", !string.IsNullOrEmpty(request.ProjectName) ? request.ProjectName.ToString() : "null");
                url = url.Replace("{year}",request.Year != null ? request.Year.ToString() : "null");
                url = url.Replace("{month}",request.Month != null ? request.Month.ToString() : "null");

                var sessionRequest = Factory.Get(url);
                if (request.ContextStack != null)
                {
                    sessionRequest.AddQueryString("contextStack", request.ContextStack);
                }
                if (request.AccountToken != null) {
                    sessionRequest.AddQueryString("accountToken", $"{request.AccountToken}");
                }
                if (request.Region != null) {
                    sessionRequest.AddQueryString("region", $"{request.Region}");
                }
                if (request.Service != null) {
                    sessionRequest.AddQueryString("service", $"{request.Service}");
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
		public IEnumerator DescribeBillings(
                Request.DescribeBillingsRequest request,
                UnityAction<AsyncResult<Result.DescribeBillingsResult>> callback
        )
		{
			var task = new DescribeBillingsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.DescribeBillingsResult>(task.Result, task.Error));
        }

		public IFuture<Result.DescribeBillingsResult> DescribeBillingsFuture(
                Request.DescribeBillingsRequest request
        )
		{
			return new DescribeBillingsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.DescribeBillingsResult> DescribeBillingsAsync(
                Request.DescribeBillingsRequest request
        )
		{
            AsyncResult<Result.DescribeBillingsResult> result = null;
			await DescribeBillings(
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
		public DescribeBillingsTask DescribeBillingsAsync(
                Request.DescribeBillingsRequest request
        )
		{
			return new DescribeBillingsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.DescribeBillingsResult> DescribeBillingsAsync(
                Request.DescribeBillingsRequest request
        )
		{
			var task = new DescribeBillingsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class DescribeDumpProgressesTask : Gs2RestSessionTask<DescribeDumpProgressesRequest, DescribeDumpProgressesResult>
        {
            public DescribeDumpProgressesTask(IGs2Session session, RestSessionRequestFactory factory, DescribeDumpProgressesRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DescribeDumpProgressesRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "project")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/account/me/project/dump/progress";

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
		public IEnumerator DescribeDumpProgresses(
                Request.DescribeDumpProgressesRequest request,
                UnityAction<AsyncResult<Result.DescribeDumpProgressesResult>> callback
        )
		{
			var task = new DescribeDumpProgressesTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.DescribeDumpProgressesResult>(task.Result, task.Error));
        }

		public IFuture<Result.DescribeDumpProgressesResult> DescribeDumpProgressesFuture(
                Request.DescribeDumpProgressesRequest request
        )
		{
			return new DescribeDumpProgressesTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.DescribeDumpProgressesResult> DescribeDumpProgressesAsync(
                Request.DescribeDumpProgressesRequest request
        )
		{
            AsyncResult<Result.DescribeDumpProgressesResult> result = null;
			await DescribeDumpProgresses(
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
		public DescribeDumpProgressesTask DescribeDumpProgressesAsync(
                Request.DescribeDumpProgressesRequest request
        )
		{
			return new DescribeDumpProgressesTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.DescribeDumpProgressesResult> DescribeDumpProgressesAsync(
                Request.DescribeDumpProgressesRequest request
        )
		{
			var task = new DescribeDumpProgressesTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class GetDumpProgressTask : Gs2RestSessionTask<GetDumpProgressRequest, GetDumpProgressResult>
        {
            public GetDumpProgressTask(IGs2Session session, RestSessionRequestFactory factory, GetDumpProgressRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(GetDumpProgressRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "project")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/account/me/project/dump/progress/{transactionId}";

                url = url.Replace("{transactionId}", !string.IsNullOrEmpty(request.TransactionId) ? request.TransactionId.ToString() : "null");

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
		public IEnumerator GetDumpProgress(
                Request.GetDumpProgressRequest request,
                UnityAction<AsyncResult<Result.GetDumpProgressResult>> callback
        )
		{
			var task = new GetDumpProgressTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.GetDumpProgressResult>(task.Result, task.Error));
        }

		public IFuture<Result.GetDumpProgressResult> GetDumpProgressFuture(
                Request.GetDumpProgressRequest request
        )
		{
			return new GetDumpProgressTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.GetDumpProgressResult> GetDumpProgressAsync(
                Request.GetDumpProgressRequest request
        )
		{
            AsyncResult<Result.GetDumpProgressResult> result = null;
			await GetDumpProgress(
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
		public GetDumpProgressTask GetDumpProgressAsync(
                Request.GetDumpProgressRequest request
        )
		{
			return new GetDumpProgressTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.GetDumpProgressResult> GetDumpProgressAsync(
                Request.GetDumpProgressRequest request
        )
		{
			var task = new GetDumpProgressTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class WaitDumpUserDataTask : Gs2RestSessionTask<WaitDumpUserDataRequest, WaitDumpUserDataResult>
        {
            public WaitDumpUserDataTask(IGs2Session session, RestSessionRequestFactory factory, WaitDumpUserDataRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(WaitDumpUserDataRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "project")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/account/me/project/dump/progress/{transactionId}/wait";

                url = url.Replace("{transactionId}", !string.IsNullOrEmpty(request.TransactionId) ? request.TransactionId.ToString() : "null");

                var sessionRequest = Factory.Post(url);

                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);
                jsonWriter.WriteObjectStart();
                if (request.UserId != null)
                {
                    jsonWriter.WritePropertyName("userId");
                    jsonWriter.Write(request.UserId);
                }
                if (request.MicroserviceName != null)
                {
                    jsonWriter.WritePropertyName("microserviceName");
                    jsonWriter.Write(request.MicroserviceName);
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
                if (request.DuplicationAvoider != null)
                {
                    sessionRequest.AddHeader("X-GS2-DUPLICATION-AVOIDER", request.DuplicationAvoider);
                }
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
		public IEnumerator WaitDumpUserData(
                Request.WaitDumpUserDataRequest request,
                UnityAction<AsyncResult<Result.WaitDumpUserDataResult>> callback
        )
		{
			var task = new WaitDumpUserDataTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.WaitDumpUserDataResult>(task.Result, task.Error));
        }

		public IFuture<Result.WaitDumpUserDataResult> WaitDumpUserDataFuture(
                Request.WaitDumpUserDataRequest request
        )
		{
			return new WaitDumpUserDataTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.WaitDumpUserDataResult> WaitDumpUserDataAsync(
                Request.WaitDumpUserDataRequest request
        )
		{
            AsyncResult<Result.WaitDumpUserDataResult> result = null;
			await WaitDumpUserData(
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
		public WaitDumpUserDataTask WaitDumpUserDataAsync(
                Request.WaitDumpUserDataRequest request
        )
		{
			return new WaitDumpUserDataTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.WaitDumpUserDataResult> WaitDumpUserDataAsync(
                Request.WaitDumpUserDataRequest request
        )
		{
			var task = new WaitDumpUserDataTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class ArchiveDumpUserDataTask : Gs2RestSessionTask<ArchiveDumpUserDataRequest, ArchiveDumpUserDataResult>
        {
            public ArchiveDumpUserDataTask(IGs2Session session, RestSessionRequestFactory factory, ArchiveDumpUserDataRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(ArchiveDumpUserDataRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "project")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/account/me/project/dump/progress/{transactionId}/archive";

                url = url.Replace("{transactionId}", !string.IsNullOrEmpty(request.TransactionId) ? request.TransactionId.ToString() : "null");

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
		public IEnumerator ArchiveDumpUserData(
                Request.ArchiveDumpUserDataRequest request,
                UnityAction<AsyncResult<Result.ArchiveDumpUserDataResult>> callback
        )
		{
			var task = new ArchiveDumpUserDataTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.ArchiveDumpUserDataResult>(task.Result, task.Error));
        }

		public IFuture<Result.ArchiveDumpUserDataResult> ArchiveDumpUserDataFuture(
                Request.ArchiveDumpUserDataRequest request
        )
		{
			return new ArchiveDumpUserDataTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.ArchiveDumpUserDataResult> ArchiveDumpUserDataAsync(
                Request.ArchiveDumpUserDataRequest request
        )
		{
            AsyncResult<Result.ArchiveDumpUserDataResult> result = null;
			await ArchiveDumpUserData(
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
		public ArchiveDumpUserDataTask ArchiveDumpUserDataAsync(
                Request.ArchiveDumpUserDataRequest request
        )
		{
			return new ArchiveDumpUserDataTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.ArchiveDumpUserDataResult> ArchiveDumpUserDataAsync(
                Request.ArchiveDumpUserDataRequest request
        )
		{
			var task = new ArchiveDumpUserDataTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class DumpUserDataTask : Gs2RestSessionTask<DumpUserDataRequest, DumpUserDataResult>
        {
            public DumpUserDataTask(IGs2Session session, RestSessionRequestFactory factory, DumpUserDataRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DumpUserDataRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "project")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/account/me/project/dump/{userId}";

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
                if (request.DuplicationAvoider != null)
                {
                    sessionRequest.AddHeader("X-GS2-DUPLICATION-AVOIDER", request.DuplicationAvoider);
                }
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
		public IEnumerator DumpUserData(
                Request.DumpUserDataRequest request,
                UnityAction<AsyncResult<Result.DumpUserDataResult>> callback
        )
		{
			var task = new DumpUserDataTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.DumpUserDataResult>(task.Result, task.Error));
        }

		public IFuture<Result.DumpUserDataResult> DumpUserDataFuture(
                Request.DumpUserDataRequest request
        )
		{
			return new DumpUserDataTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.DumpUserDataResult> DumpUserDataAsync(
                Request.DumpUserDataRequest request
        )
		{
            AsyncResult<Result.DumpUserDataResult> result = null;
			await DumpUserData(
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
		public DumpUserDataTask DumpUserDataAsync(
                Request.DumpUserDataRequest request
        )
		{
			return new DumpUserDataTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.DumpUserDataResult> DumpUserDataAsync(
                Request.DumpUserDataRequest request
        )
		{
			var task = new DumpUserDataTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class GetDumpUserDataTask : Gs2RestSessionTask<GetDumpUserDataRequest, GetDumpUserDataResult>
        {
            public GetDumpUserDataTask(IGs2Session session, RestSessionRequestFactory factory, GetDumpUserDataRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(GetDumpUserDataRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "project")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/account/me/project/dump/{transactionId}";

                url = url.Replace("{transactionId}", !string.IsNullOrEmpty(request.TransactionId) ? request.TransactionId.ToString() : "null");

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
		public IEnumerator GetDumpUserData(
                Request.GetDumpUserDataRequest request,
                UnityAction<AsyncResult<Result.GetDumpUserDataResult>> callback
        )
		{
			var task = new GetDumpUserDataTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.GetDumpUserDataResult>(task.Result, task.Error));
        }

		public IFuture<Result.GetDumpUserDataResult> GetDumpUserDataFuture(
                Request.GetDumpUserDataRequest request
        )
		{
			return new GetDumpUserDataTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.GetDumpUserDataResult> GetDumpUserDataAsync(
                Request.GetDumpUserDataRequest request
        )
		{
            AsyncResult<Result.GetDumpUserDataResult> result = null;
			await GetDumpUserData(
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
		public GetDumpUserDataTask GetDumpUserDataAsync(
                Request.GetDumpUserDataRequest request
        )
		{
			return new GetDumpUserDataTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.GetDumpUserDataResult> GetDumpUserDataAsync(
                Request.GetDumpUserDataRequest request
        )
		{
			var task = new GetDumpUserDataTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class DescribeCleanProgressesTask : Gs2RestSessionTask<DescribeCleanProgressesRequest, DescribeCleanProgressesResult>
        {
            public DescribeCleanProgressesTask(IGs2Session session, RestSessionRequestFactory factory, DescribeCleanProgressesRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DescribeCleanProgressesRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "project")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/account/me/project/clean/progress";

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
		public IEnumerator DescribeCleanProgresses(
                Request.DescribeCleanProgressesRequest request,
                UnityAction<AsyncResult<Result.DescribeCleanProgressesResult>> callback
        )
		{
			var task = new DescribeCleanProgressesTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.DescribeCleanProgressesResult>(task.Result, task.Error));
        }

		public IFuture<Result.DescribeCleanProgressesResult> DescribeCleanProgressesFuture(
                Request.DescribeCleanProgressesRequest request
        )
		{
			return new DescribeCleanProgressesTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.DescribeCleanProgressesResult> DescribeCleanProgressesAsync(
                Request.DescribeCleanProgressesRequest request
        )
		{
            AsyncResult<Result.DescribeCleanProgressesResult> result = null;
			await DescribeCleanProgresses(
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
		public DescribeCleanProgressesTask DescribeCleanProgressesAsync(
                Request.DescribeCleanProgressesRequest request
        )
		{
			return new DescribeCleanProgressesTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.DescribeCleanProgressesResult> DescribeCleanProgressesAsync(
                Request.DescribeCleanProgressesRequest request
        )
		{
			var task = new DescribeCleanProgressesTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class GetCleanProgressTask : Gs2RestSessionTask<GetCleanProgressRequest, GetCleanProgressResult>
        {
            public GetCleanProgressTask(IGs2Session session, RestSessionRequestFactory factory, GetCleanProgressRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(GetCleanProgressRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "project")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/account/me/project/clean/progress/{transactionId}";

                url = url.Replace("{transactionId}", !string.IsNullOrEmpty(request.TransactionId) ? request.TransactionId.ToString() : "null");

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
		public IEnumerator GetCleanProgress(
                Request.GetCleanProgressRequest request,
                UnityAction<AsyncResult<Result.GetCleanProgressResult>> callback
        )
		{
			var task = new GetCleanProgressTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.GetCleanProgressResult>(task.Result, task.Error));
        }

		public IFuture<Result.GetCleanProgressResult> GetCleanProgressFuture(
                Request.GetCleanProgressRequest request
        )
		{
			return new GetCleanProgressTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.GetCleanProgressResult> GetCleanProgressAsync(
                Request.GetCleanProgressRequest request
        )
		{
            AsyncResult<Result.GetCleanProgressResult> result = null;
			await GetCleanProgress(
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
		public GetCleanProgressTask GetCleanProgressAsync(
                Request.GetCleanProgressRequest request
        )
		{
			return new GetCleanProgressTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.GetCleanProgressResult> GetCleanProgressAsync(
                Request.GetCleanProgressRequest request
        )
		{
			var task = new GetCleanProgressTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class WaitCleanUserDataTask : Gs2RestSessionTask<WaitCleanUserDataRequest, WaitCleanUserDataResult>
        {
            public WaitCleanUserDataTask(IGs2Session session, RestSessionRequestFactory factory, WaitCleanUserDataRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(WaitCleanUserDataRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "project")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/account/me/project/clean/progress/{transactionId}/wait";

                url = url.Replace("{transactionId}", !string.IsNullOrEmpty(request.TransactionId) ? request.TransactionId.ToString() : "null");

                var sessionRequest = Factory.Post(url);

                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);
                jsonWriter.WriteObjectStart();
                if (request.UserId != null)
                {
                    jsonWriter.WritePropertyName("userId");
                    jsonWriter.Write(request.UserId);
                }
                if (request.MicroserviceName != null)
                {
                    jsonWriter.WritePropertyName("microserviceName");
                    jsonWriter.Write(request.MicroserviceName);
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
                if (request.DuplicationAvoider != null)
                {
                    sessionRequest.AddHeader("X-GS2-DUPLICATION-AVOIDER", request.DuplicationAvoider);
                }
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
		public IEnumerator WaitCleanUserData(
                Request.WaitCleanUserDataRequest request,
                UnityAction<AsyncResult<Result.WaitCleanUserDataResult>> callback
        )
		{
			var task = new WaitCleanUserDataTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.WaitCleanUserDataResult>(task.Result, task.Error));
        }

		public IFuture<Result.WaitCleanUserDataResult> WaitCleanUserDataFuture(
                Request.WaitCleanUserDataRequest request
        )
		{
			return new WaitCleanUserDataTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.WaitCleanUserDataResult> WaitCleanUserDataAsync(
                Request.WaitCleanUserDataRequest request
        )
		{
            AsyncResult<Result.WaitCleanUserDataResult> result = null;
			await WaitCleanUserData(
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
		public WaitCleanUserDataTask WaitCleanUserDataAsync(
                Request.WaitCleanUserDataRequest request
        )
		{
			return new WaitCleanUserDataTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.WaitCleanUserDataResult> WaitCleanUserDataAsync(
                Request.WaitCleanUserDataRequest request
        )
		{
			var task = new WaitCleanUserDataTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class CleanUserDataTask : Gs2RestSessionTask<CleanUserDataRequest, CleanUserDataResult>
        {
            public CleanUserDataTask(IGs2Session session, RestSessionRequestFactory factory, CleanUserDataRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(CleanUserDataRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "project")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/account/me/project/clean/{userId}";

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
                if (request.DuplicationAvoider != null)
                {
                    sessionRequest.AddHeader("X-GS2-DUPLICATION-AVOIDER", request.DuplicationAvoider);
                }
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
		public IEnumerator CleanUserData(
                Request.CleanUserDataRequest request,
                UnityAction<AsyncResult<Result.CleanUserDataResult>> callback
        )
		{
			var task = new CleanUserDataTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.CleanUserDataResult>(task.Result, task.Error));
        }

		public IFuture<Result.CleanUserDataResult> CleanUserDataFuture(
                Request.CleanUserDataRequest request
        )
		{
			return new CleanUserDataTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.CleanUserDataResult> CleanUserDataAsync(
                Request.CleanUserDataRequest request
        )
		{
            AsyncResult<Result.CleanUserDataResult> result = null;
			await CleanUserData(
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
		public CleanUserDataTask CleanUserDataAsync(
                Request.CleanUserDataRequest request
        )
		{
			return new CleanUserDataTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.CleanUserDataResult> CleanUserDataAsync(
                Request.CleanUserDataRequest request
        )
		{
			var task = new CleanUserDataTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class DescribeImportProgressesTask : Gs2RestSessionTask<DescribeImportProgressesRequest, DescribeImportProgressesResult>
        {
            public DescribeImportProgressesTask(IGs2Session session, RestSessionRequestFactory factory, DescribeImportProgressesRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DescribeImportProgressesRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "project")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/account/me/project/import/progress";

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
		public IEnumerator DescribeImportProgresses(
                Request.DescribeImportProgressesRequest request,
                UnityAction<AsyncResult<Result.DescribeImportProgressesResult>> callback
        )
		{
			var task = new DescribeImportProgressesTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.DescribeImportProgressesResult>(task.Result, task.Error));
        }

		public IFuture<Result.DescribeImportProgressesResult> DescribeImportProgressesFuture(
                Request.DescribeImportProgressesRequest request
        )
		{
			return new DescribeImportProgressesTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.DescribeImportProgressesResult> DescribeImportProgressesAsync(
                Request.DescribeImportProgressesRequest request
        )
		{
            AsyncResult<Result.DescribeImportProgressesResult> result = null;
			await DescribeImportProgresses(
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
		public DescribeImportProgressesTask DescribeImportProgressesAsync(
                Request.DescribeImportProgressesRequest request
        )
		{
			return new DescribeImportProgressesTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.DescribeImportProgressesResult> DescribeImportProgressesAsync(
                Request.DescribeImportProgressesRequest request
        )
		{
			var task = new DescribeImportProgressesTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class GetImportProgressTask : Gs2RestSessionTask<GetImportProgressRequest, GetImportProgressResult>
        {
            public GetImportProgressTask(IGs2Session session, RestSessionRequestFactory factory, GetImportProgressRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(GetImportProgressRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "project")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/account/me/project/import/progress/{transactionId}";

                url = url.Replace("{transactionId}", !string.IsNullOrEmpty(request.TransactionId) ? request.TransactionId.ToString() : "null");

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
		public IEnumerator GetImportProgress(
                Request.GetImportProgressRequest request,
                UnityAction<AsyncResult<Result.GetImportProgressResult>> callback
        )
		{
			var task = new GetImportProgressTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.GetImportProgressResult>(task.Result, task.Error));
        }

		public IFuture<Result.GetImportProgressResult> GetImportProgressFuture(
                Request.GetImportProgressRequest request
        )
		{
			return new GetImportProgressTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.GetImportProgressResult> GetImportProgressAsync(
                Request.GetImportProgressRequest request
        )
		{
            AsyncResult<Result.GetImportProgressResult> result = null;
			await GetImportProgress(
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
		public GetImportProgressTask GetImportProgressAsync(
                Request.GetImportProgressRequest request
        )
		{
			return new GetImportProgressTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.GetImportProgressResult> GetImportProgressAsync(
                Request.GetImportProgressRequest request
        )
		{
			var task = new GetImportProgressTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class WaitImportUserDataTask : Gs2RestSessionTask<WaitImportUserDataRequest, WaitImportUserDataResult>
        {
            public WaitImportUserDataTask(IGs2Session session, RestSessionRequestFactory factory, WaitImportUserDataRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(WaitImportUserDataRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "project")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/account/me/project/import/progress/{transactionId}/wait";

                url = url.Replace("{transactionId}", !string.IsNullOrEmpty(request.TransactionId) ? request.TransactionId.ToString() : "null");

                var sessionRequest = Factory.Post(url);

                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);
                jsonWriter.WriteObjectStart();
                if (request.UserId != null)
                {
                    jsonWriter.WritePropertyName("userId");
                    jsonWriter.Write(request.UserId);
                }
                if (request.MicroserviceName != null)
                {
                    jsonWriter.WritePropertyName("microserviceName");
                    jsonWriter.Write(request.MicroserviceName);
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
                if (request.DuplicationAvoider != null)
                {
                    sessionRequest.AddHeader("X-GS2-DUPLICATION-AVOIDER", request.DuplicationAvoider);
                }
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
		public IEnumerator WaitImportUserData(
                Request.WaitImportUserDataRequest request,
                UnityAction<AsyncResult<Result.WaitImportUserDataResult>> callback
        )
		{
			var task = new WaitImportUserDataTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.WaitImportUserDataResult>(task.Result, task.Error));
        }

		public IFuture<Result.WaitImportUserDataResult> WaitImportUserDataFuture(
                Request.WaitImportUserDataRequest request
        )
		{
			return new WaitImportUserDataTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.WaitImportUserDataResult> WaitImportUserDataAsync(
                Request.WaitImportUserDataRequest request
        )
		{
            AsyncResult<Result.WaitImportUserDataResult> result = null;
			await WaitImportUserData(
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
		public WaitImportUserDataTask WaitImportUserDataAsync(
                Request.WaitImportUserDataRequest request
        )
		{
			return new WaitImportUserDataTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.WaitImportUserDataResult> WaitImportUserDataAsync(
                Request.WaitImportUserDataRequest request
        )
		{
			var task = new WaitImportUserDataTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class PrepareImportUserDataTask : Gs2RestSessionTask<PrepareImportUserDataRequest, PrepareImportUserDataResult>
        {
            public PrepareImportUserDataTask(IGs2Session session, RestSessionRequestFactory factory, PrepareImportUserDataRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(PrepareImportUserDataRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "project")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/account/me/project/import/{userId}/prepare";

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
                if (request.DuplicationAvoider != null)
                {
                    sessionRequest.AddHeader("X-GS2-DUPLICATION-AVOIDER", request.DuplicationAvoider);
                }
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
		public IEnumerator PrepareImportUserData(
                Request.PrepareImportUserDataRequest request,
                UnityAction<AsyncResult<Result.PrepareImportUserDataResult>> callback
        )
		{
			var task = new PrepareImportUserDataTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.PrepareImportUserDataResult>(task.Result, task.Error));
        }

		public IFuture<Result.PrepareImportUserDataResult> PrepareImportUserDataFuture(
                Request.PrepareImportUserDataRequest request
        )
		{
			return new PrepareImportUserDataTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.PrepareImportUserDataResult> PrepareImportUserDataAsync(
                Request.PrepareImportUserDataRequest request
        )
		{
            AsyncResult<Result.PrepareImportUserDataResult> result = null;
			await PrepareImportUserData(
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
		public PrepareImportUserDataTask PrepareImportUserDataAsync(
                Request.PrepareImportUserDataRequest request
        )
		{
			return new PrepareImportUserDataTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.PrepareImportUserDataResult> PrepareImportUserDataAsync(
                Request.PrepareImportUserDataRequest request
        )
		{
			var task = new PrepareImportUserDataTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class ImportUserDataTask : Gs2RestSessionTask<ImportUserDataRequest, ImportUserDataResult>
        {
            public ImportUserDataTask(IGs2Session session, RestSessionRequestFactory factory, ImportUserDataRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(ImportUserDataRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "project")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/account/me/project/import/{userId}";

                url = url.Replace("{userId}", !string.IsNullOrEmpty(request.UserId) ? request.UserId.ToString() : "null");

                var sessionRequest = Factory.Post(url);

                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);
                jsonWriter.WriteObjectStart();
                if (request.UploadToken != null)
                {
                    jsonWriter.WritePropertyName("uploadToken");
                    jsonWriter.Write(request.UploadToken);
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
                if (request.DuplicationAvoider != null)
                {
                    sessionRequest.AddHeader("X-GS2-DUPLICATION-AVOIDER", request.DuplicationAvoider);
                }
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
		public IEnumerator ImportUserData(
                Request.ImportUserDataRequest request,
                UnityAction<AsyncResult<Result.ImportUserDataResult>> callback
        )
		{
			var task = new ImportUserDataTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.ImportUserDataResult>(task.Result, task.Error));
        }

		public IFuture<Result.ImportUserDataResult> ImportUserDataFuture(
                Request.ImportUserDataRequest request
        )
		{
			return new ImportUserDataTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.ImportUserDataResult> ImportUserDataAsync(
                Request.ImportUserDataRequest request
        )
		{
            AsyncResult<Result.ImportUserDataResult> result = null;
			await ImportUserData(
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
		public ImportUserDataTask ImportUserDataAsync(
                Request.ImportUserDataRequest request
        )
		{
			return new ImportUserDataTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.ImportUserDataResult> ImportUserDataAsync(
                Request.ImportUserDataRequest request
        )
		{
			var task = new ImportUserDataTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class DescribeImportErrorLogsTask : Gs2RestSessionTask<DescribeImportErrorLogsRequest, DescribeImportErrorLogsResult>
        {
            public DescribeImportErrorLogsTask(IGs2Session session, RestSessionRequestFactory factory, DescribeImportErrorLogsRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DescribeImportErrorLogsRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "project")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/account/me/project/import/progress/{transactionId}/log";

                url = url.Replace("{transactionId}", !string.IsNullOrEmpty(request.TransactionId) ? request.TransactionId.ToString() : "null");

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
		public IEnumerator DescribeImportErrorLogs(
                Request.DescribeImportErrorLogsRequest request,
                UnityAction<AsyncResult<Result.DescribeImportErrorLogsResult>> callback
        )
		{
			var task = new DescribeImportErrorLogsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.DescribeImportErrorLogsResult>(task.Result, task.Error));
        }

		public IFuture<Result.DescribeImportErrorLogsResult> DescribeImportErrorLogsFuture(
                Request.DescribeImportErrorLogsRequest request
        )
		{
			return new DescribeImportErrorLogsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.DescribeImportErrorLogsResult> DescribeImportErrorLogsAsync(
                Request.DescribeImportErrorLogsRequest request
        )
		{
            AsyncResult<Result.DescribeImportErrorLogsResult> result = null;
			await DescribeImportErrorLogs(
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
		public DescribeImportErrorLogsTask DescribeImportErrorLogsAsync(
                Request.DescribeImportErrorLogsRequest request
        )
		{
			return new DescribeImportErrorLogsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.DescribeImportErrorLogsResult> DescribeImportErrorLogsAsync(
                Request.DescribeImportErrorLogsRequest request
        )
		{
			var task = new DescribeImportErrorLogsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class GetImportErrorLogTask : Gs2RestSessionTask<GetImportErrorLogRequest, GetImportErrorLogResult>
        {
            public GetImportErrorLogTask(IGs2Session session, RestSessionRequestFactory factory, GetImportErrorLogRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(GetImportErrorLogRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "project")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/account/me/project/import/progress/{transactionId}/log/{errorLogName}";

                url = url.Replace("{transactionId}", !string.IsNullOrEmpty(request.TransactionId) ? request.TransactionId.ToString() : "null");
                url = url.Replace("{errorLogName}", !string.IsNullOrEmpty(request.ErrorLogName) ? request.ErrorLogName.ToString() : "null");

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
		public IEnumerator GetImportErrorLog(
                Request.GetImportErrorLogRequest request,
                UnityAction<AsyncResult<Result.GetImportErrorLogResult>> callback
        )
		{
			var task = new GetImportErrorLogTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.GetImportErrorLogResult>(task.Result, task.Error));
        }

		public IFuture<Result.GetImportErrorLogResult> GetImportErrorLogFuture(
                Request.GetImportErrorLogRequest request
        )
		{
			return new GetImportErrorLogTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.GetImportErrorLogResult> GetImportErrorLogAsync(
                Request.GetImportErrorLogRequest request
        )
		{
            AsyncResult<Result.GetImportErrorLogResult> result = null;
			await GetImportErrorLog(
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
		public GetImportErrorLogTask GetImportErrorLogAsync(
                Request.GetImportErrorLogRequest request
        )
		{
			return new GetImportErrorLogTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.GetImportErrorLogResult> GetImportErrorLogAsync(
                Request.GetImportErrorLogRequest request
        )
		{
			var task = new GetImportErrorLogTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif
	}
}