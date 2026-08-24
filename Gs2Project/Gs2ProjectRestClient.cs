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
        ) =>
            new CreateAccountTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.CreateAccountResult> CreateAccountFuture(
                Request.CreateAccountRequest request
        ) =>
            new CreateAccountTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.CreateAccountResult> CreateAccountAsync(
                Request.CreateAccountRequest request
        ) =>
            new CreateAccountTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.CreateAccountResult>();
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
		public Task<Result.CreateAccountResult> CreateAccountAsync(
                Request.CreateAccountRequest request
        ) =>
            new CreateAccountTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
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
        ) =>
            new VerifyTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.VerifyResult> VerifyFuture(
                Request.VerifyRequest request
        ) =>
            new VerifyTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.VerifyResult> VerifyAsync(
                Request.VerifyRequest request
        ) =>
            new VerifyTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.VerifyResult>();
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
		public Task<Result.VerifyResult> VerifyAsync(
                Request.VerifyRequest request
        ) =>
            new VerifyTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
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
        ) =>
            new SignInTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.SignInResult> SignInFuture(
                Request.SignInRequest request
        ) =>
            new SignInTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.SignInResult> SignInAsync(
                Request.SignInRequest request
        ) =>
            new SignInTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.SignInResult>();
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
		public Task<Result.SignInResult> SignInAsync(
                Request.SignInRequest request
        ) =>
            new SignInTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
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
        ) =>
            new ForgetTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.ForgetResult> ForgetFuture(
                Request.ForgetRequest request
        ) =>
            new ForgetTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.ForgetResult> ForgetAsync(
                Request.ForgetRequest request
        ) =>
            new ForgetTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.ForgetResult>();
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
		public Task<Result.ForgetResult> ForgetAsync(
                Request.ForgetRequest request
        ) =>
            new ForgetTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
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
        ) =>
            new IssuePasswordTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.IssuePasswordResult> IssuePasswordFuture(
                Request.IssuePasswordRequest request
        ) =>
            new IssuePasswordTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.IssuePasswordResult> IssuePasswordAsync(
                Request.IssuePasswordRequest request
        ) =>
            new IssuePasswordTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.IssuePasswordResult>();
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
		public Task<Result.IssuePasswordResult> IssuePasswordAsync(
                Request.IssuePasswordRequest request
        ) =>
            new IssuePasswordTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
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
        ) =>
            new UpdateAccountTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.UpdateAccountResult> UpdateAccountFuture(
                Request.UpdateAccountRequest request
        ) =>
            new UpdateAccountTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.UpdateAccountResult> UpdateAccountAsync(
                Request.UpdateAccountRequest request
        ) =>
            new UpdateAccountTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.UpdateAccountResult>();
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
		public Task<Result.UpdateAccountResult> UpdateAccountAsync(
                Request.UpdateAccountRequest request
        ) =>
            new UpdateAccountTask(
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
        ) =>
            new DeleteAccountTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.DeleteAccountResult> DeleteAccountFuture(
                Request.DeleteAccountRequest request
        ) =>
            new DeleteAccountTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.DeleteAccountResult> DeleteAccountAsync(
                Request.DeleteAccountRequest request
        ) =>
            new DeleteAccountTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.DeleteAccountResult>();
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
		public Task<Result.DeleteAccountResult> DeleteAccountAsync(
                Request.DeleteAccountRequest request
        ) =>
            new DeleteAccountTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class GetServiceVersionTask : Gs2RestSessionTask<GetServiceVersionRequest, GetServiceVersionResult>
        {
            public GetServiceVersionTask(IGs2Session session, RestSessionRequestFactory factory, GetServiceVersionRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(GetServiceVersionRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "project")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/system/version";

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
		public IEnumerator GetServiceVersion(
                Request.GetServiceVersionRequest request,
                UnityAction<AsyncResult<Result.GetServiceVersionResult>> callback
        ) =>
            new GetServiceVersionTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.GetServiceVersionResult> GetServiceVersionFuture(
                Request.GetServiceVersionRequest request
        ) =>
            new GetServiceVersionTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.GetServiceVersionResult> GetServiceVersionAsync(
                Request.GetServiceVersionRequest request
        ) =>
            new GetServiceVersionTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.GetServiceVersionResult>();
    #else
		public GetServiceVersionTask GetServiceVersionAsync(
                Request.GetServiceVersionRequest request
        )
		{
			return new GetServiceVersionTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.GetServiceVersionResult> GetServiceVersionAsync(
                Request.GetServiceVersionRequest request
        ) =>
            new GetServiceVersionTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
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
        ) =>
            new DescribeProjectsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.DescribeProjectsResult> DescribeProjectsFuture(
                Request.DescribeProjectsRequest request
        ) =>
            new DescribeProjectsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.DescribeProjectsResult> DescribeProjectsAsync(
                Request.DescribeProjectsRequest request
        ) =>
            new DescribeProjectsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.DescribeProjectsResult>();
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
		public Task<Result.DescribeProjectsResult> DescribeProjectsAsync(
                Request.DescribeProjectsRequest request
        ) =>
            new DescribeProjectsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
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
        ) =>
            new CreateProjectTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.CreateProjectResult> CreateProjectFuture(
                Request.CreateProjectRequest request
        ) =>
            new CreateProjectTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.CreateProjectResult> CreateProjectAsync(
                Request.CreateProjectRequest request
        ) =>
            new CreateProjectTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.CreateProjectResult>();
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
		public Task<Result.CreateProjectResult> CreateProjectAsync(
                Request.CreateProjectRequest request
        ) =>
            new CreateProjectTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
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
        ) =>
            new GetProjectTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.GetProjectResult> GetProjectFuture(
                Request.GetProjectRequest request
        ) =>
            new GetProjectTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.GetProjectResult> GetProjectAsync(
                Request.GetProjectRequest request
        ) =>
            new GetProjectTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.GetProjectResult>();
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
		public Task<Result.GetProjectResult> GetProjectAsync(
                Request.GetProjectRequest request
        ) =>
            new GetProjectTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
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
        ) =>
            new GetProjectTokenTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.GetProjectTokenResult> GetProjectTokenFuture(
                Request.GetProjectTokenRequest request
        ) =>
            new GetProjectTokenTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.GetProjectTokenResult> GetProjectTokenAsync(
                Request.GetProjectTokenRequest request
        ) =>
            new GetProjectTokenTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.GetProjectTokenResult>();
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
		public Task<Result.GetProjectTokenResult> GetProjectTokenAsync(
                Request.GetProjectTokenRequest request
        ) =>
            new GetProjectTokenTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
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
        ) =>
            new GetProjectTokenByIdentifierTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.GetProjectTokenByIdentifierResult> GetProjectTokenByIdentifierFuture(
                Request.GetProjectTokenByIdentifierRequest request
        ) =>
            new GetProjectTokenByIdentifierTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.GetProjectTokenByIdentifierResult> GetProjectTokenByIdentifierAsync(
                Request.GetProjectTokenByIdentifierRequest request
        ) =>
            new GetProjectTokenByIdentifierTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.GetProjectTokenByIdentifierResult>();
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
		public Task<Result.GetProjectTokenByIdentifierResult> GetProjectTokenByIdentifierAsync(
                Request.GetProjectTokenByIdentifierRequest request
        ) =>
            new GetProjectTokenByIdentifierTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
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
        ) =>
            new UpdateProjectTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.UpdateProjectResult> UpdateProjectFuture(
                Request.UpdateProjectRequest request
        ) =>
            new UpdateProjectTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.UpdateProjectResult> UpdateProjectAsync(
                Request.UpdateProjectRequest request
        ) =>
            new UpdateProjectTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.UpdateProjectResult>();
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
		public Task<Result.UpdateProjectResult> UpdateProjectAsync(
                Request.UpdateProjectRequest request
        ) =>
            new UpdateProjectTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
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
        ) =>
            new ActivateRegionTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.ActivateRegionResult> ActivateRegionFuture(
                Request.ActivateRegionRequest request
        ) =>
            new ActivateRegionTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.ActivateRegionResult> ActivateRegionAsync(
                Request.ActivateRegionRequest request
        ) =>
            new ActivateRegionTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.ActivateRegionResult>();
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
		public Task<Result.ActivateRegionResult> ActivateRegionAsync(
                Request.ActivateRegionRequest request
        ) =>
            new ActivateRegionTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
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
                    + "/system/{ownerId}/project/region/{regionName}/activate/wait";

                url = url.Replace("{ownerId}", !string.IsNullOrEmpty(request.OwnerId) ? request.OwnerId.ToString() : "null");
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
        ) =>
            new WaitActivateRegionTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.WaitActivateRegionResult> WaitActivateRegionFuture(
                Request.WaitActivateRegionRequest request
        ) =>
            new WaitActivateRegionTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.WaitActivateRegionResult> WaitActivateRegionAsync(
                Request.WaitActivateRegionRequest request
        ) =>
            new WaitActivateRegionTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.WaitActivateRegionResult>();
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
		public Task<Result.WaitActivateRegionResult> WaitActivateRegionAsync(
                Request.WaitActivateRegionRequest request
        ) =>
            new WaitActivateRegionTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
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
        ) =>
            new DeleteProjectTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.DeleteProjectResult> DeleteProjectFuture(
                Request.DeleteProjectRequest request
        ) =>
            new DeleteProjectTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.DeleteProjectResult> DeleteProjectAsync(
                Request.DeleteProjectRequest request
        ) =>
            new DeleteProjectTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.DeleteProjectResult>();
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
		public Task<Result.DeleteProjectResult> DeleteProjectAsync(
                Request.DeleteProjectRequest request
        ) =>
            new DeleteProjectTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
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
        ) =>
            new DescribeBillingMethodsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.DescribeBillingMethodsResult> DescribeBillingMethodsFuture(
                Request.DescribeBillingMethodsRequest request
        ) =>
            new DescribeBillingMethodsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.DescribeBillingMethodsResult> DescribeBillingMethodsAsync(
                Request.DescribeBillingMethodsRequest request
        ) =>
            new DescribeBillingMethodsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.DescribeBillingMethodsResult>();
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
		public Task<Result.DescribeBillingMethodsResult> DescribeBillingMethodsAsync(
                Request.DescribeBillingMethodsRequest request
        ) =>
            new DescribeBillingMethodsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
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
        ) =>
            new CreateBillingMethodTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.CreateBillingMethodResult> CreateBillingMethodFuture(
                Request.CreateBillingMethodRequest request
        ) =>
            new CreateBillingMethodTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.CreateBillingMethodResult> CreateBillingMethodAsync(
                Request.CreateBillingMethodRequest request
        ) =>
            new CreateBillingMethodTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.CreateBillingMethodResult>();
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
		public Task<Result.CreateBillingMethodResult> CreateBillingMethodAsync(
                Request.CreateBillingMethodRequest request
        ) =>
            new CreateBillingMethodTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
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
        ) =>
            new GetBillingMethodTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.GetBillingMethodResult> GetBillingMethodFuture(
                Request.GetBillingMethodRequest request
        ) =>
            new GetBillingMethodTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.GetBillingMethodResult> GetBillingMethodAsync(
                Request.GetBillingMethodRequest request
        ) =>
            new GetBillingMethodTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.GetBillingMethodResult>();
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
		public Task<Result.GetBillingMethodResult> GetBillingMethodAsync(
                Request.GetBillingMethodRequest request
        ) =>
            new GetBillingMethodTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
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
        ) =>
            new UpdateBillingMethodTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.UpdateBillingMethodResult> UpdateBillingMethodFuture(
                Request.UpdateBillingMethodRequest request
        ) =>
            new UpdateBillingMethodTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.UpdateBillingMethodResult> UpdateBillingMethodAsync(
                Request.UpdateBillingMethodRequest request
        ) =>
            new UpdateBillingMethodTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.UpdateBillingMethodResult>();
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
		public Task<Result.UpdateBillingMethodResult> UpdateBillingMethodAsync(
                Request.UpdateBillingMethodRequest request
        ) =>
            new UpdateBillingMethodTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
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
        ) =>
            new DeleteBillingMethodTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.DeleteBillingMethodResult> DeleteBillingMethodFuture(
                Request.DeleteBillingMethodRequest request
        ) =>
            new DeleteBillingMethodTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.DeleteBillingMethodResult> DeleteBillingMethodAsync(
                Request.DeleteBillingMethodRequest request
        ) =>
            new DeleteBillingMethodTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.DeleteBillingMethodResult>();
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
		public Task<Result.DeleteBillingMethodResult> DeleteBillingMethodAsync(
                Request.DeleteBillingMethodRequest request
        ) =>
            new DeleteBillingMethodTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
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
        ) =>
            new DescribeReceiptsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.DescribeReceiptsResult> DescribeReceiptsFuture(
                Request.DescribeReceiptsRequest request
        ) =>
            new DescribeReceiptsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.DescribeReceiptsResult> DescribeReceiptsAsync(
                Request.DescribeReceiptsRequest request
        ) =>
            new DescribeReceiptsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.DescribeReceiptsResult>();
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
		public Task<Result.DescribeReceiptsResult> DescribeReceiptsAsync(
                Request.DescribeReceiptsRequest request
        ) =>
            new DescribeReceiptsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
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
        ) =>
            new DescribeBillingsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.DescribeBillingsResult> DescribeBillingsFuture(
                Request.DescribeBillingsRequest request
        ) =>
            new DescribeBillingsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.DescribeBillingsResult> DescribeBillingsAsync(
                Request.DescribeBillingsRequest request
        ) =>
            new DescribeBillingsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.DescribeBillingsResult>();
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
		public Task<Result.DescribeBillingsResult> DescribeBillingsAsync(
                Request.DescribeBillingsRequest request
        ) =>
            new DescribeBillingsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class GetBillingsTask : Gs2RestSessionTask<GetBillingsRequest, GetBillingsResult>
        {
            public GetBillingsTask(IGs2Session session, RestSessionRequestFactory factory, GetBillingsRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(GetBillingsRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "project")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/billing/{year}/{month}";

                url = url.Replace("{year}",request.Year != null ? request.Year.ToString() : "null");
                url = url.Replace("{month}",request.Month != null ? request.Month.ToString() : "null");

                var sessionRequest = Factory.Get(url);
                if (request.ContextStack != null)
                {
                    sessionRequest.AddQueryString("contextStack", request.ContextStack);
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
		public IEnumerator GetBillings(
                Request.GetBillingsRequest request,
                UnityAction<AsyncResult<Result.GetBillingsResult>> callback
        ) =>
            new GetBillingsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.GetBillingsResult> GetBillingsFuture(
                Request.GetBillingsRequest request
        ) =>
            new GetBillingsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.GetBillingsResult> GetBillingsAsync(
                Request.GetBillingsRequest request
        ) =>
            new GetBillingsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.GetBillingsResult>();
    #else
		public GetBillingsTask GetBillingsAsync(
                Request.GetBillingsRequest request
        )
		{
			return new GetBillingsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.GetBillingsResult> GetBillingsAsync(
                Request.GetBillingsRequest request
        ) =>
            new GetBillingsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
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
        ) =>
            new DescribeDumpProgressesTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.DescribeDumpProgressesResult> DescribeDumpProgressesFuture(
                Request.DescribeDumpProgressesRequest request
        ) =>
            new DescribeDumpProgressesTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.DescribeDumpProgressesResult> DescribeDumpProgressesAsync(
                Request.DescribeDumpProgressesRequest request
        ) =>
            new DescribeDumpProgressesTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.DescribeDumpProgressesResult>();
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
		public Task<Result.DescribeDumpProgressesResult> DescribeDumpProgressesAsync(
                Request.DescribeDumpProgressesRequest request
        ) =>
            new DescribeDumpProgressesTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
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
        ) =>
            new GetDumpProgressTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.GetDumpProgressResult> GetDumpProgressFuture(
                Request.GetDumpProgressRequest request
        ) =>
            new GetDumpProgressTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.GetDumpProgressResult> GetDumpProgressAsync(
                Request.GetDumpProgressRequest request
        ) =>
            new GetDumpProgressTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.GetDumpProgressResult>();
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
		public Task<Result.GetDumpProgressResult> GetDumpProgressAsync(
                Request.GetDumpProgressRequest request
        ) =>
            new GetDumpProgressTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
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
                    + "/system/{ownerId}/project/dump/progress/{transactionId}/wait";

                url = url.Replace("{ownerId}", !string.IsNullOrEmpty(request.OwnerId) ? request.OwnerId.ToString() : "null");
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
        ) =>
            new WaitDumpUserDataTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.WaitDumpUserDataResult> WaitDumpUserDataFuture(
                Request.WaitDumpUserDataRequest request
        ) =>
            new WaitDumpUserDataTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.WaitDumpUserDataResult> WaitDumpUserDataAsync(
                Request.WaitDumpUserDataRequest request
        ) =>
            new WaitDumpUserDataTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.WaitDumpUserDataResult>();
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
		public Task<Result.WaitDumpUserDataResult> WaitDumpUserDataAsync(
                Request.WaitDumpUserDataRequest request
        ) =>
            new WaitDumpUserDataTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
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
                    + "/system/{ownerId}/project/dump/progress/{transactionId}/archive";

                url = url.Replace("{ownerId}", !string.IsNullOrEmpty(request.OwnerId) ? request.OwnerId.ToString() : "null");
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
        ) =>
            new ArchiveDumpUserDataTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.ArchiveDumpUserDataResult> ArchiveDumpUserDataFuture(
                Request.ArchiveDumpUserDataRequest request
        ) =>
            new ArchiveDumpUserDataTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.ArchiveDumpUserDataResult> ArchiveDumpUserDataAsync(
                Request.ArchiveDumpUserDataRequest request
        ) =>
            new ArchiveDumpUserDataTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.ArchiveDumpUserDataResult>();
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
		public Task<Result.ArchiveDumpUserDataResult> ArchiveDumpUserDataAsync(
                Request.ArchiveDumpUserDataRequest request
        ) =>
            new ArchiveDumpUserDataTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
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
        ) =>
            new DumpUserDataTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.DumpUserDataResult> DumpUserDataFuture(
                Request.DumpUserDataRequest request
        ) =>
            new DumpUserDataTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.DumpUserDataResult> DumpUserDataAsync(
                Request.DumpUserDataRequest request
        ) =>
            new DumpUserDataTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.DumpUserDataResult>();
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
		public Task<Result.DumpUserDataResult> DumpUserDataAsync(
                Request.DumpUserDataRequest request
        ) =>
            new DumpUserDataTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
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
        ) =>
            new GetDumpUserDataTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.GetDumpUserDataResult> GetDumpUserDataFuture(
                Request.GetDumpUserDataRequest request
        ) =>
            new GetDumpUserDataTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.GetDumpUserDataResult> GetDumpUserDataAsync(
                Request.GetDumpUserDataRequest request
        ) =>
            new GetDumpUserDataTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.GetDumpUserDataResult>();
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
		public Task<Result.GetDumpUserDataResult> GetDumpUserDataAsync(
                Request.GetDumpUserDataRequest request
        ) =>
            new GetDumpUserDataTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
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
        ) =>
            new DescribeCleanProgressesTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.DescribeCleanProgressesResult> DescribeCleanProgressesFuture(
                Request.DescribeCleanProgressesRequest request
        ) =>
            new DescribeCleanProgressesTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.DescribeCleanProgressesResult> DescribeCleanProgressesAsync(
                Request.DescribeCleanProgressesRequest request
        ) =>
            new DescribeCleanProgressesTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.DescribeCleanProgressesResult>();
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
		public Task<Result.DescribeCleanProgressesResult> DescribeCleanProgressesAsync(
                Request.DescribeCleanProgressesRequest request
        ) =>
            new DescribeCleanProgressesTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
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
        ) =>
            new GetCleanProgressTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.GetCleanProgressResult> GetCleanProgressFuture(
                Request.GetCleanProgressRequest request
        ) =>
            new GetCleanProgressTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.GetCleanProgressResult> GetCleanProgressAsync(
                Request.GetCleanProgressRequest request
        ) =>
            new GetCleanProgressTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.GetCleanProgressResult>();
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
		public Task<Result.GetCleanProgressResult> GetCleanProgressAsync(
                Request.GetCleanProgressRequest request
        ) =>
            new GetCleanProgressTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
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
                    + "/system/{ownerId}/project/clean/progress/{transactionId}/wait";

                url = url.Replace("{ownerId}", !string.IsNullOrEmpty(request.OwnerId) ? request.OwnerId.ToString() : "null");
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
        ) =>
            new WaitCleanUserDataTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.WaitCleanUserDataResult> WaitCleanUserDataFuture(
                Request.WaitCleanUserDataRequest request
        ) =>
            new WaitCleanUserDataTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.WaitCleanUserDataResult> WaitCleanUserDataAsync(
                Request.WaitCleanUserDataRequest request
        ) =>
            new WaitCleanUserDataTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.WaitCleanUserDataResult>();
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
		public Task<Result.WaitCleanUserDataResult> WaitCleanUserDataAsync(
                Request.WaitCleanUserDataRequest request
        ) =>
            new WaitCleanUserDataTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
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
        ) =>
            new CleanUserDataTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.CleanUserDataResult> CleanUserDataFuture(
                Request.CleanUserDataRequest request
        ) =>
            new CleanUserDataTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.CleanUserDataResult> CleanUserDataAsync(
                Request.CleanUserDataRequest request
        ) =>
            new CleanUserDataTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.CleanUserDataResult>();
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
		public Task<Result.CleanUserDataResult> CleanUserDataAsync(
                Request.CleanUserDataRequest request
        ) =>
            new CleanUserDataTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
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
        ) =>
            new DescribeImportProgressesTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.DescribeImportProgressesResult> DescribeImportProgressesFuture(
                Request.DescribeImportProgressesRequest request
        ) =>
            new DescribeImportProgressesTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.DescribeImportProgressesResult> DescribeImportProgressesAsync(
                Request.DescribeImportProgressesRequest request
        ) =>
            new DescribeImportProgressesTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.DescribeImportProgressesResult>();
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
		public Task<Result.DescribeImportProgressesResult> DescribeImportProgressesAsync(
                Request.DescribeImportProgressesRequest request
        ) =>
            new DescribeImportProgressesTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
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
        ) =>
            new GetImportProgressTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.GetImportProgressResult> GetImportProgressFuture(
                Request.GetImportProgressRequest request
        ) =>
            new GetImportProgressTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.GetImportProgressResult> GetImportProgressAsync(
                Request.GetImportProgressRequest request
        ) =>
            new GetImportProgressTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.GetImportProgressResult>();
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
		public Task<Result.GetImportProgressResult> GetImportProgressAsync(
                Request.GetImportProgressRequest request
        ) =>
            new GetImportProgressTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
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
                    + "/system/{ownerId}/project/import/progress/{transactionId}/wait";

                url = url.Replace("{ownerId}", !string.IsNullOrEmpty(request.OwnerId) ? request.OwnerId.ToString() : "null");
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
        ) =>
            new WaitImportUserDataTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.WaitImportUserDataResult> WaitImportUserDataFuture(
                Request.WaitImportUserDataRequest request
        ) =>
            new WaitImportUserDataTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.WaitImportUserDataResult> WaitImportUserDataAsync(
                Request.WaitImportUserDataRequest request
        ) =>
            new WaitImportUserDataTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.WaitImportUserDataResult>();
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
		public Task<Result.WaitImportUserDataResult> WaitImportUserDataAsync(
                Request.WaitImportUserDataRequest request
        ) =>
            new WaitImportUserDataTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
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
        ) =>
            new PrepareImportUserDataTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.PrepareImportUserDataResult> PrepareImportUserDataFuture(
                Request.PrepareImportUserDataRequest request
        ) =>
            new PrepareImportUserDataTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.PrepareImportUserDataResult> PrepareImportUserDataAsync(
                Request.PrepareImportUserDataRequest request
        ) =>
            new PrepareImportUserDataTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.PrepareImportUserDataResult>();
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
		public Task<Result.PrepareImportUserDataResult> PrepareImportUserDataAsync(
                Request.PrepareImportUserDataRequest request
        ) =>
            new PrepareImportUserDataTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
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
        ) =>
            new ImportUserDataTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.ImportUserDataResult> ImportUserDataFuture(
                Request.ImportUserDataRequest request
        ) =>
            new ImportUserDataTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.ImportUserDataResult> ImportUserDataAsync(
                Request.ImportUserDataRequest request
        ) =>
            new ImportUserDataTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.ImportUserDataResult>();
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
		public Task<Result.ImportUserDataResult> ImportUserDataAsync(
                Request.ImportUserDataRequest request
        ) =>
            new ImportUserDataTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
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
        ) =>
            new DescribeImportErrorLogsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.DescribeImportErrorLogsResult> DescribeImportErrorLogsFuture(
                Request.DescribeImportErrorLogsRequest request
        ) =>
            new DescribeImportErrorLogsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.DescribeImportErrorLogsResult> DescribeImportErrorLogsAsync(
                Request.DescribeImportErrorLogsRequest request
        ) =>
            new DescribeImportErrorLogsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.DescribeImportErrorLogsResult>();
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
		public Task<Result.DescribeImportErrorLogsResult> DescribeImportErrorLogsAsync(
                Request.DescribeImportErrorLogsRequest request
        ) =>
            new DescribeImportErrorLogsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
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
        ) =>
            new GetImportErrorLogTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.GetImportErrorLogResult> GetImportErrorLogFuture(
                Request.GetImportErrorLogRequest request
        ) =>
            new GetImportErrorLogTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.GetImportErrorLogResult> GetImportErrorLogAsync(
                Request.GetImportErrorLogRequest request
        ) =>
            new GetImportErrorLogTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.GetImportErrorLogResult>();
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
		public Task<Result.GetImportErrorLogResult> GetImportErrorLogAsync(
                Request.GetImportErrorLogRequest request
        ) =>
            new GetImportErrorLogTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif
	}
}