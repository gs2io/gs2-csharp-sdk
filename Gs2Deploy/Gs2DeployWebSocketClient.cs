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

namespace Gs2.Gs2Deploy
{
	public class Gs2DeployWebSocketClient : AbstractGs2Client
	{

		public static string Endpoint = "deploy";

        protected Gs2WebSocketSession Gs2WebSocketSession => (Gs2WebSocketSession) Gs2Session;

		public Gs2DeployWebSocketClient(Gs2WebSocketSession Gs2WebSocketSession) : base(Gs2WebSocketSession)
		{

		}


        public class PreCreateStackTask : Gs2WebSocketSessionTask<Request.PreCreateStackRequest, Result.PreCreateStackResult>
        {
            public PreCreateStackTask(IGs2Session session, Request.PreCreateStackRequest request) : base(session, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(Request.PreCreateStackRequest request)
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
                    "deploy",
                    "stack",
                    "preCreateStack",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
        public IEnumerator PreCreateStack(
                Request.PreCreateStackRequest request,
                UnityAction<AsyncResult<Result.PreCreateStackResult>> callback
        ) =>
            new PreCreateStackTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToCoroutine(callback);

        public IFuture<Result.PreCreateStackResult> PreCreateStackFuture(
                Request.PreCreateStackRequest request
        ) =>
            new PreCreateStackTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
        public UniTask<Result.PreCreateStackResult> PreCreateStackAsync(
            Request.PreCreateStackRequest request
        ) =>
            new PreCreateStackTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsUniTask<Result.PreCreateStackResult>();
    #else
        public PreCreateStackTask PreCreateStackAsync(
                Request.PreCreateStackRequest request
        )
        {
            return new PreCreateStackTask(
                Gs2WebSocketSession,
                request
            );
        }
    #endif
#else
        public Task<Result.PreCreateStackResult> PreCreateStackAsync(
            Request.PreCreateStackRequest request
        ) =>
            new PreCreateStackTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsTask();
#endif


        public class PreValidateTask : Gs2WebSocketSessionTask<Request.PreValidateRequest, Result.PreValidateResult>
        {
            public PreValidateTask(IGs2Session session, Request.PreValidateRequest request) : base(session, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(Request.PreValidateRequest request)
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
                    "deploy",
                    "stack",
                    "preValidate",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
        public IEnumerator PreValidate(
                Request.PreValidateRequest request,
                UnityAction<AsyncResult<Result.PreValidateResult>> callback
        ) =>
            new PreValidateTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToCoroutine(callback);

        public IFuture<Result.PreValidateResult> PreValidateFuture(
                Request.PreValidateRequest request
        ) =>
            new PreValidateTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
        public UniTask<Result.PreValidateResult> PreValidateAsync(
            Request.PreValidateRequest request
        ) =>
            new PreValidateTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsUniTask<Result.PreValidateResult>();
    #else
        public PreValidateTask PreValidateAsync(
                Request.PreValidateRequest request
        )
        {
            return new PreValidateTask(
                Gs2WebSocketSession,
                request
            );
        }
    #endif
#else
        public Task<Result.PreValidateResult> PreValidateAsync(
            Request.PreValidateRequest request
        ) =>
            new PreValidateTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsTask();
#endif


        public class ValidateTask : Gs2WebSocketSessionTask<Request.ValidateRequest, Result.ValidateResult>
        {
            public ValidateTask(IGs2Session session, Request.ValidateRequest request) : base(session, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(Request.ValidateRequest request)
            {
                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);

                jsonWriter.WriteObjectStart();

                if (request.Mode != null)
                {
                    jsonWriter.WritePropertyName("mode");
                    jsonWriter.Write(request.Mode.ToString());
                }
                if (request.Template != null)
                {
                    jsonWriter.WritePropertyName("template");
                    jsonWriter.Write(request.Template.ToString());
                }
                if (request.UploadToken != null)
                {
                    jsonWriter.WritePropertyName("uploadToken");
                    jsonWriter.Write(request.UploadToken.ToString());
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
                    "deploy",
                    "stack",
                    "validate",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
        public IEnumerator Validate(
                Request.ValidateRequest request,
                UnityAction<AsyncResult<Result.ValidateResult>> callback
        ) =>
            new ValidateTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToCoroutine(callback);

        public IFuture<Result.ValidateResult> ValidateFuture(
                Request.ValidateRequest request
        ) =>
            new ValidateTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
        public UniTask<Result.ValidateResult> ValidateAsync(
            Request.ValidateRequest request
        ) =>
            new ValidateTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsUniTask<Result.ValidateResult>();
    #else
        public ValidateTask ValidateAsync(
                Request.ValidateRequest request
        )
        {
            return new ValidateTask(
                Gs2WebSocketSession,
                request
            );
        }
    #endif
#else
        public Task<Result.ValidateResult> ValidateAsync(
            Request.ValidateRequest request
        ) =>
            new ValidateTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsTask();
#endif


        public class PreUpdateStackTask : Gs2WebSocketSessionTask<Request.PreUpdateStackRequest, Result.PreUpdateStackResult>
        {
            public PreUpdateStackTask(IGs2Session session, Request.PreUpdateStackRequest request) : base(session, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(Request.PreUpdateStackRequest request)
            {
                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);

                jsonWriter.WriteObjectStart();

                if (request.StackName != null)
                {
                    jsonWriter.WritePropertyName("stackName");
                    jsonWriter.Write(request.StackName.ToString());
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
                    "deploy",
                    "stack",
                    "preUpdateStack",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
        public IEnumerator PreUpdateStack(
                Request.PreUpdateStackRequest request,
                UnityAction<AsyncResult<Result.PreUpdateStackResult>> callback
        ) =>
            new PreUpdateStackTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToCoroutine(callback);

        public IFuture<Result.PreUpdateStackResult> PreUpdateStackFuture(
                Request.PreUpdateStackRequest request
        ) =>
            new PreUpdateStackTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
        public UniTask<Result.PreUpdateStackResult> PreUpdateStackAsync(
            Request.PreUpdateStackRequest request
        ) =>
            new PreUpdateStackTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsUniTask<Result.PreUpdateStackResult>();
    #else
        public PreUpdateStackTask PreUpdateStackAsync(
                Request.PreUpdateStackRequest request
        )
        {
            return new PreUpdateStackTask(
                Gs2WebSocketSession,
                request
            );
        }
    #endif
#else
        public Task<Result.PreUpdateStackResult> PreUpdateStackAsync(
            Request.PreUpdateStackRequest request
        ) =>
            new PreUpdateStackTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsTask();
#endif


        public class PreChangeSetTask : Gs2WebSocketSessionTask<Request.PreChangeSetRequest, Result.PreChangeSetResult>
        {
            public PreChangeSetTask(IGs2Session session, Request.PreChangeSetRequest request) : base(session, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(Request.PreChangeSetRequest request)
            {
                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);

                jsonWriter.WriteObjectStart();

                if (request.StackName != null)
                {
                    jsonWriter.WritePropertyName("stackName");
                    jsonWriter.Write(request.StackName.ToString());
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
                    "deploy",
                    "stack",
                    "preChangeSet",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
        public IEnumerator PreChangeSet(
                Request.PreChangeSetRequest request,
                UnityAction<AsyncResult<Result.PreChangeSetResult>> callback
        ) =>
            new PreChangeSetTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToCoroutine(callback);

        public IFuture<Result.PreChangeSetResult> PreChangeSetFuture(
                Request.PreChangeSetRequest request
        ) =>
            new PreChangeSetTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
        public UniTask<Result.PreChangeSetResult> PreChangeSetAsync(
            Request.PreChangeSetRequest request
        ) =>
            new PreChangeSetTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsUniTask<Result.PreChangeSetResult>();
    #else
        public PreChangeSetTask PreChangeSetAsync(
                Request.PreChangeSetRequest request
        )
        {
            return new PreChangeSetTask(
                Gs2WebSocketSession,
                request
            );
        }
    #endif
#else
        public Task<Result.PreChangeSetResult> PreChangeSetAsync(
            Request.PreChangeSetRequest request
        ) =>
            new PreChangeSetTask(
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
                    "deploy",
                    "stack",
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