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

using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Gs2.Core;
using Gs2.Core.Model;
using Gs2.Core.Net;
using Gs2.Core.Util;
using Gs2.Util.LitJson;

#if UNITY_2017_1_OR_NEWER
using System.Collections;
using UnityEngine.Events;
using UnityEngine.Networking;
    #if GS2_ENABLE_UNITASK
using Cysharp.Threading.Tasks;
    #endif
#else
using System.Threading.Tasks;
using System.Threading;
#endif

namespace Gs2.Gs2Auth
{
	public class Gs2AuthWebSocketClient : AbstractGs2Client
	{

		public static string Endpoint = "auth";

        protected Gs2WebSocketSession Gs2WebSocketSession => (Gs2WebSocketSession) Gs2Session;

		public Gs2AuthWebSocketClient(Gs2WebSocketSession Gs2WebSocketSession) : base(Gs2WebSocketSession)
		{

		}


        public class LoginTask : Gs2WebSocketSessionTask<Request.LoginRequest, Result.LoginResult>
        {
            public LoginTask(IGs2Session session, Request.LoginRequest request) : base(session, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(Request.LoginRequest request)
            {
                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);

                jsonWriter.WriteObjectStart();

                if (request.UserId != null)
                {
                    jsonWriter.WritePropertyName("userId");
                    jsonWriter.Write(request.UserId.ToString());
                }
                if (request.TimeOffset != null)
                {
                    jsonWriter.WritePropertyName("timeOffset");
                    jsonWriter.Write(request.TimeOffset.ToString());
                }
                if (request.TimeOffsetToken != null)
                {
                    jsonWriter.WritePropertyName("timeOffsetToken");
                    jsonWriter.Write(request.TimeOffsetToken.ToString());
                }
                if (request.ContextStack != null)
                {
                    jsonWriter.WritePropertyName("contextStack");
                    jsonWriter.Write(request.ContextStack.ToString());
                }
                if (request.DryRun)
                {
                    jsonWriter.WritePropertyName("xGs2DryRun");
                    jsonWriter.Write("true");
                }

                AddHeader(
                    Session.Credential,
                    "auth",
                    "accessToken",
                    "login",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
        public IEnumerator Login(
                Request.LoginRequest request,
                UnityAction<AsyncResult<Result.LoginResult>> callback
        ) =>
            new LoginTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToCoroutine(callback);

        public IFuture<Result.LoginResult> LoginFuture(
                Request.LoginRequest request
        ) =>
            new LoginTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
        public UniTask<Result.LoginResult> LoginAsync(
            Request.LoginRequest request
        ) =>
            new LoginTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsUniTask<Result.LoginResult>();
    #else
        public LoginTask LoginAsync(
                Request.LoginRequest request
        )
        {
            return new LoginTask(
                Gs2WebSocketSession,
                request
            );
        }
    #endif
#else
        public Task<Result.LoginResult> LoginAsync(
            Request.LoginRequest request
        ) =>
            new LoginTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsTask();
#endif


        public class LoginBySignatureTask : Gs2WebSocketSessionTask<Request.LoginBySignatureRequest, Result.LoginBySignatureResult>
        {
            public LoginBySignatureTask(IGs2Session session, Request.LoginBySignatureRequest request) : base(session, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(Request.LoginBySignatureRequest request)
            {
                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);

                jsonWriter.WriteObjectStart();

                if (request.KeyId != null)
                {
                    jsonWriter.WritePropertyName("keyId");
                    jsonWriter.Write(request.KeyId.ToString());
                }
                if (request.Body != null)
                {
                    jsonWriter.WritePropertyName("body");
                    jsonWriter.Write(request.Body.ToString());
                }
                if (request.Signature != null)
                {
                    jsonWriter.WritePropertyName("signature");
                    jsonWriter.Write(request.Signature.ToString());
                }
                if (request.ContextStack != null)
                {
                    jsonWriter.WritePropertyName("contextStack");
                    jsonWriter.Write(request.ContextStack.ToString());
                }
                if (request.DryRun)
                {
                    jsonWriter.WritePropertyName("xGs2DryRun");
                    jsonWriter.Write("true");
                }

                AddHeader(
                    Session.Credential,
                    "auth",
                    "accessToken",
                    "loginBySignature",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
        public IEnumerator LoginBySignature(
                Request.LoginBySignatureRequest request,
                UnityAction<AsyncResult<Result.LoginBySignatureResult>> callback
        ) =>
            new LoginBySignatureTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToCoroutine(callback);

        public IFuture<Result.LoginBySignatureResult> LoginBySignatureFuture(
                Request.LoginBySignatureRequest request
        ) =>
            new LoginBySignatureTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
        public UniTask<Result.LoginBySignatureResult> LoginBySignatureAsync(
            Request.LoginBySignatureRequest request
        ) =>
            new LoginBySignatureTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsUniTask<Result.LoginBySignatureResult>();
    #else
        public LoginBySignatureTask LoginBySignatureAsync(
                Request.LoginBySignatureRequest request
        )
        {
            return new LoginBySignatureTask(
                Gs2WebSocketSession,
                request
            );
        }
    #endif
#else
        public Task<Result.LoginBySignatureResult> LoginBySignatureAsync(
            Request.LoginBySignatureRequest request
        ) =>
            new LoginBySignatureTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsTask();
#endif


        public class FederationTask : Gs2WebSocketSessionTask<Request.FederationRequest, Result.FederationResult>
        {
            public FederationTask(IGs2Session session, Request.FederationRequest request) : base(session, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(Request.FederationRequest request)
            {
                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);

                jsonWriter.WriteObjectStart();

                if (request.OriginalUserId != null)
                {
                    jsonWriter.WritePropertyName("originalUserId");
                    jsonWriter.Write(request.OriginalUserId.ToString());
                }
                if (request.UserId != null)
                {
                    jsonWriter.WritePropertyName("userId");
                    jsonWriter.Write(request.UserId.ToString());
                }
                if (request.PolicyDocument != null)
                {
                    jsonWriter.WritePropertyName("policyDocument");
                    jsonWriter.Write(request.PolicyDocument.ToString());
                }
                if (request.TimeOffset != null)
                {
                    jsonWriter.WritePropertyName("timeOffset");
                    jsonWriter.Write(request.TimeOffset.ToString());
                }
                if (request.TimeOffsetToken != null)
                {
                    jsonWriter.WritePropertyName("timeOffsetToken");
                    jsonWriter.Write(request.TimeOffsetToken.ToString());
                }
                if (request.ContextStack != null)
                {
                    jsonWriter.WritePropertyName("contextStack");
                    jsonWriter.Write(request.ContextStack.ToString());
                }
                if (request.DryRun)
                {
                    jsonWriter.WritePropertyName("xGs2DryRun");
                    jsonWriter.Write("true");
                }

                AddHeader(
                    Session.Credential,
                    "auth",
                    "accessToken",
                    "federation",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
        public IEnumerator Federation(
                Request.FederationRequest request,
                UnityAction<AsyncResult<Result.FederationResult>> callback
        ) =>
            new FederationTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToCoroutine(callback);

        public IFuture<Result.FederationResult> FederationFuture(
                Request.FederationRequest request
        ) =>
            new FederationTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
        public UniTask<Result.FederationResult> FederationAsync(
            Request.FederationRequest request
        ) =>
            new FederationTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsUniTask<Result.FederationResult>();
    #else
        public FederationTask FederationAsync(
                Request.FederationRequest request
        )
        {
            return new FederationTask(
                Gs2WebSocketSession,
                request
            );
        }
    #endif
#else
        public Task<Result.FederationResult> FederationAsync(
            Request.FederationRequest request
        ) =>
            new FederationTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsTask();
#endif


        public class IssueTimeOffsetTokenByUserIdTask : Gs2WebSocketSessionTask<Request.IssueTimeOffsetTokenByUserIdRequest, Result.IssueTimeOffsetTokenByUserIdResult>
        {
            public IssueTimeOffsetTokenByUserIdTask(IGs2Session session, Request.IssueTimeOffsetTokenByUserIdRequest request) : base(session, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(Request.IssueTimeOffsetTokenByUserIdRequest request)
            {
                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);

                jsonWriter.WriteObjectStart();

                if (request.UserId != null)
                {
                    jsonWriter.WritePropertyName("userId");
                    jsonWriter.Write(request.UserId.ToString());
                }
                if (request.TimeOffset != null)
                {
                    jsonWriter.WritePropertyName("timeOffset");
                    jsonWriter.Write(request.TimeOffset.ToString());
                }
                if (request.TimeOffsetToken != null)
                {
                    jsonWriter.WritePropertyName("timeOffsetToken");
                    jsonWriter.Write(request.TimeOffsetToken.ToString());
                }
                if (request.ContextStack != null)
                {
                    jsonWriter.WritePropertyName("contextStack");
                    jsonWriter.Write(request.ContextStack.ToString());
                }
                if (request.DryRun)
                {
                    jsonWriter.WritePropertyName("xGs2DryRun");
                    jsonWriter.Write("true");
                }

                AddHeader(
                    Session.Credential,
                    "auth",
                    "accessToken",
                    "issueTimeOffsetTokenByUserId",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
        public IEnumerator IssueTimeOffsetTokenByUserId(
                Request.IssueTimeOffsetTokenByUserIdRequest request,
                UnityAction<AsyncResult<Result.IssueTimeOffsetTokenByUserIdResult>> callback
        ) =>
            new IssueTimeOffsetTokenByUserIdTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToCoroutine(callback);

        public IFuture<Result.IssueTimeOffsetTokenByUserIdResult> IssueTimeOffsetTokenByUserIdFuture(
                Request.IssueTimeOffsetTokenByUserIdRequest request
        ) =>
            new IssueTimeOffsetTokenByUserIdTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
        public UniTask<Result.IssueTimeOffsetTokenByUserIdResult> IssueTimeOffsetTokenByUserIdAsync(
            Request.IssueTimeOffsetTokenByUserIdRequest request
        ) =>
            new IssueTimeOffsetTokenByUserIdTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsUniTask<Result.IssueTimeOffsetTokenByUserIdResult>();
    #else
        public IssueTimeOffsetTokenByUserIdTask IssueTimeOffsetTokenByUserIdAsync(
                Request.IssueTimeOffsetTokenByUserIdRequest request
        )
        {
            return new IssueTimeOffsetTokenByUserIdTask(
                Gs2WebSocketSession,
                request
            );
        }
    #endif
#else
        public Task<Result.IssueTimeOffsetTokenByUserIdResult> IssueTimeOffsetTokenByUserIdAsync(
            Request.IssueTimeOffsetTokenByUserIdRequest request
        ) =>
            new IssueTimeOffsetTokenByUserIdTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsTask();
#endif


        public class GetServiceVersionTask : Gs2WebSocketSessionTask<Request.GetServiceVersionRequest, Result.GetServiceVersionResult>
        {
            public GetServiceVersionTask(IGs2Session session, Request.GetServiceVersionRequest request) : base(session, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(Request.GetServiceVersionRequest request)
            {
                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);

                jsonWriter.WriteObjectStart();

                if (request.ContextStack != null)
                {
                    jsonWriter.WritePropertyName("contextStack");
                    jsonWriter.Write(request.ContextStack.ToString());
                }
                if (request.DryRun)
                {
                    jsonWriter.WritePropertyName("xGs2DryRun");
                    jsonWriter.Write("true");
                }

                AddHeader(
                    Session.Credential,
                    "auth",
                    "accessToken",
                    "getServiceVersion",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
        public IEnumerator GetServiceVersion(
                Request.GetServiceVersionRequest request,
                UnityAction<AsyncResult<Result.GetServiceVersionResult>> callback
        ) =>
            new GetServiceVersionTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToCoroutine(callback);

        public IFuture<Result.GetServiceVersionResult> GetServiceVersionFuture(
                Request.GetServiceVersionRequest request
        ) =>
            new GetServiceVersionTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
        public UniTask<Result.GetServiceVersionResult> GetServiceVersionAsync(
            Request.GetServiceVersionRequest request
        ) =>
            new GetServiceVersionTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsUniTask<Result.GetServiceVersionResult>();
    #else
        public GetServiceVersionTask GetServiceVersionAsync(
                Request.GetServiceVersionRequest request
        )
        {
            return new GetServiceVersionTask(
                Gs2WebSocketSession,
                request
            );
        }
    #endif
#else
        public Task<Result.GetServiceVersionResult> GetServiceVersionAsync(
            Request.GetServiceVersionRequest request
        ) =>
            new GetServiceVersionTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsTask();
#endif
	}
}