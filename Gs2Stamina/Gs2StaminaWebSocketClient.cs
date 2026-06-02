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

namespace Gs2.Gs2Stamina
{
	public class Gs2StaminaWebSocketClient : AbstractGs2Client
	{

		public static string Endpoint = "stamina";

        protected Gs2WebSocketSession Gs2WebSocketSession => (Gs2WebSocketSession) Gs2Session;

		public Gs2StaminaWebSocketClient(Gs2WebSocketSession Gs2WebSocketSession) : base(Gs2WebSocketSession)
		{

		}


        public class CreateNamespaceTask : Gs2WebSocketSessionTask<Request.CreateNamespaceRequest, Result.CreateNamespaceResult>
        {
            public CreateNamespaceTask(IGs2Session session, Request.CreateNamespaceRequest request) : base(session, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(Request.CreateNamespaceRequest request)
            {
                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);

                jsonWriter.WriteObjectStart();

                if (request.Name != null)
                {
                    jsonWriter.WritePropertyName("name");
                    jsonWriter.Write(request.Name.ToString());
                }
                if (request.Description != null)
                {
                    jsonWriter.WritePropertyName("description");
                    jsonWriter.Write(request.Description.ToString());
                }
                if (request.TransactionSetting != null)
                {
                    jsonWriter.WritePropertyName("transactionSetting");
                    request.TransactionSetting.WriteJson(jsonWriter);
                }
                if (request.OverflowTriggerScript != null)
                {
                    jsonWriter.WritePropertyName("overflowTriggerScript");
                    jsonWriter.Write(request.OverflowTriggerScript.ToString());
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
                if (request.DryRun)
                {
                    jsonWriter.WritePropertyName("xGs2DryRun");
                    jsonWriter.Write("true");
                }

                AddHeader(
                    Session.Credential,
                    "stamina",
                    "namespace",
                    "createNamespace",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
        public IEnumerator CreateNamespace(
                Request.CreateNamespaceRequest request,
                UnityAction<AsyncResult<Result.CreateNamespaceResult>> callback
        ) =>
            new CreateNamespaceTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToCoroutine(callback);

        public IFuture<Result.CreateNamespaceResult> CreateNamespaceFuture(
                Request.CreateNamespaceRequest request
        ) =>
            new CreateNamespaceTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
        public UniTask<Result.CreateNamespaceResult> CreateNamespaceAsync(
            Request.CreateNamespaceRequest request
        ) =>
            new CreateNamespaceTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsUniTask<Result.CreateNamespaceResult>();
    #else
        public CreateNamespaceTask CreateNamespaceAsync(
                Request.CreateNamespaceRequest request
        )
        {
            return new CreateNamespaceTask(
                Gs2WebSocketSession,
                request
            );
        }
    #endif
#else
        public Task<Result.CreateNamespaceResult> CreateNamespaceAsync(
            Request.CreateNamespaceRequest request
        ) =>
            new CreateNamespaceTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsTask();
#endif


        public class GetNamespaceStatusTask : Gs2WebSocketSessionTask<Request.GetNamespaceStatusRequest, Result.GetNamespaceStatusResult>
        {
            public GetNamespaceStatusTask(IGs2Session session, Request.GetNamespaceStatusRequest request) : base(session, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(Request.GetNamespaceStatusRequest request)
            {
                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);

                jsonWriter.WriteObjectStart();

                if (request.NamespaceName != null)
                {
                    jsonWriter.WritePropertyName("namespaceName");
                    jsonWriter.Write(request.NamespaceName.ToString());
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
                    "stamina",
                    "namespace",
                    "getNamespaceStatus",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
        public IEnumerator GetNamespaceStatus(
                Request.GetNamespaceStatusRequest request,
                UnityAction<AsyncResult<Result.GetNamespaceStatusResult>> callback
        ) =>
            new GetNamespaceStatusTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToCoroutine(callback);

        public IFuture<Result.GetNamespaceStatusResult> GetNamespaceStatusFuture(
                Request.GetNamespaceStatusRequest request
        ) =>
            new GetNamespaceStatusTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
        public UniTask<Result.GetNamespaceStatusResult> GetNamespaceStatusAsync(
            Request.GetNamespaceStatusRequest request
        ) =>
            new GetNamespaceStatusTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsUniTask<Result.GetNamespaceStatusResult>();
    #else
        public GetNamespaceStatusTask GetNamespaceStatusAsync(
                Request.GetNamespaceStatusRequest request
        )
        {
            return new GetNamespaceStatusTask(
                Gs2WebSocketSession,
                request
            );
        }
    #endif
#else
        public Task<Result.GetNamespaceStatusResult> GetNamespaceStatusAsync(
            Request.GetNamespaceStatusRequest request
        ) =>
            new GetNamespaceStatusTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsTask();
#endif


        public class GetNamespaceTask : Gs2WebSocketSessionTask<Request.GetNamespaceRequest, Result.GetNamespaceResult>
        {
            public GetNamespaceTask(IGs2Session session, Request.GetNamespaceRequest request) : base(session, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(Request.GetNamespaceRequest request)
            {
                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);

                jsonWriter.WriteObjectStart();

                if (request.NamespaceName != null)
                {
                    jsonWriter.WritePropertyName("namespaceName");
                    jsonWriter.Write(request.NamespaceName.ToString());
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
                    "stamina",
                    "namespace",
                    "getNamespace",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
        public IEnumerator GetNamespace(
                Request.GetNamespaceRequest request,
                UnityAction<AsyncResult<Result.GetNamespaceResult>> callback
        ) =>
            new GetNamespaceTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToCoroutine(callback);

        public IFuture<Result.GetNamespaceResult> GetNamespaceFuture(
                Request.GetNamespaceRequest request
        ) =>
            new GetNamespaceTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
        public UniTask<Result.GetNamespaceResult> GetNamespaceAsync(
            Request.GetNamespaceRequest request
        ) =>
            new GetNamespaceTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsUniTask<Result.GetNamespaceResult>();
    #else
        public GetNamespaceTask GetNamespaceAsync(
                Request.GetNamespaceRequest request
        )
        {
            return new GetNamespaceTask(
                Gs2WebSocketSession,
                request
            );
        }
    #endif
#else
        public Task<Result.GetNamespaceResult> GetNamespaceAsync(
            Request.GetNamespaceRequest request
        ) =>
            new GetNamespaceTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsTask();
#endif


        public class UpdateNamespaceTask : Gs2WebSocketSessionTask<Request.UpdateNamespaceRequest, Result.UpdateNamespaceResult>
        {
            public UpdateNamespaceTask(IGs2Session session, Request.UpdateNamespaceRequest request) : base(session, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(Request.UpdateNamespaceRequest request)
            {
                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);

                jsonWriter.WriteObjectStart();

                if (request.NamespaceName != null)
                {
                    jsonWriter.WritePropertyName("namespaceName");
                    jsonWriter.Write(request.NamespaceName.ToString());
                }
                if (request.Description != null)
                {
                    jsonWriter.WritePropertyName("description");
                    jsonWriter.Write(request.Description.ToString());
                }
                if (request.TransactionSetting != null)
                {
                    jsonWriter.WritePropertyName("transactionSetting");
                    request.TransactionSetting.WriteJson(jsonWriter);
                }
                if (request.OverflowTriggerScript != null)
                {
                    jsonWriter.WritePropertyName("overflowTriggerScript");
                    jsonWriter.Write(request.OverflowTriggerScript.ToString());
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
                if (request.DryRun)
                {
                    jsonWriter.WritePropertyName("xGs2DryRun");
                    jsonWriter.Write("true");
                }

                AddHeader(
                    Session.Credential,
                    "stamina",
                    "namespace",
                    "updateNamespace",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
        public IEnumerator UpdateNamespace(
                Request.UpdateNamespaceRequest request,
                UnityAction<AsyncResult<Result.UpdateNamespaceResult>> callback
        ) =>
            new UpdateNamespaceTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToCoroutine(callback);

        public IFuture<Result.UpdateNamespaceResult> UpdateNamespaceFuture(
                Request.UpdateNamespaceRequest request
        ) =>
            new UpdateNamespaceTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
        public UniTask<Result.UpdateNamespaceResult> UpdateNamespaceAsync(
            Request.UpdateNamespaceRequest request
        ) =>
            new UpdateNamespaceTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsUniTask<Result.UpdateNamespaceResult>();
    #else
        public UpdateNamespaceTask UpdateNamespaceAsync(
                Request.UpdateNamespaceRequest request
        )
        {
            return new UpdateNamespaceTask(
                Gs2WebSocketSession,
                request
            );
        }
    #endif
#else
        public Task<Result.UpdateNamespaceResult> UpdateNamespaceAsync(
            Request.UpdateNamespaceRequest request
        ) =>
            new UpdateNamespaceTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsTask();
#endif


        public class DeleteNamespaceTask : Gs2WebSocketSessionTask<Request.DeleteNamespaceRequest, Result.DeleteNamespaceResult>
        {
            public DeleteNamespaceTask(IGs2Session session, Request.DeleteNamespaceRequest request) : base(session, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(Request.DeleteNamespaceRequest request)
            {
                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);

                jsonWriter.WriteObjectStart();

                if (request.NamespaceName != null)
                {
                    jsonWriter.WritePropertyName("namespaceName");
                    jsonWriter.Write(request.NamespaceName.ToString());
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
                    "stamina",
                    "namespace",
                    "deleteNamespace",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
        public IEnumerator DeleteNamespace(
                Request.DeleteNamespaceRequest request,
                UnityAction<AsyncResult<Result.DeleteNamespaceResult>> callback
        ) =>
            new DeleteNamespaceTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToCoroutine(callback);

        public IFuture<Result.DeleteNamespaceResult> DeleteNamespaceFuture(
                Request.DeleteNamespaceRequest request
        ) =>
            new DeleteNamespaceTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
        public UniTask<Result.DeleteNamespaceResult> DeleteNamespaceAsync(
            Request.DeleteNamespaceRequest request
        ) =>
            new DeleteNamespaceTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsUniTask<Result.DeleteNamespaceResult>();
    #else
        public DeleteNamespaceTask DeleteNamespaceAsync(
                Request.DeleteNamespaceRequest request
        )
        {
            return new DeleteNamespaceTask(
                Gs2WebSocketSession,
                request
            );
        }
    #endif
#else
        public Task<Result.DeleteNamespaceResult> DeleteNamespaceAsync(
            Request.DeleteNamespaceRequest request
        ) =>
            new DeleteNamespaceTask(
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
                    "stamina",
                    "namespace",
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


        public class DumpUserDataByUserIdTask : Gs2WebSocketSessionTask<Request.DumpUserDataByUserIdRequest, Result.DumpUserDataByUserIdResult>
        {
            public DumpUserDataByUserIdTask(IGs2Session session, Request.DumpUserDataByUserIdRequest request) : base(session, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(Request.DumpUserDataByUserIdRequest request)
            {
                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);

                jsonWriter.WriteObjectStart();

                if (request.UserId != null)
                {
                    jsonWriter.WritePropertyName("userId");
                    jsonWriter.Write(request.UserId.ToString());
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
                    "stamina",
                    "namespace",
                    "dumpUserDataByUserId",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
        public IEnumerator DumpUserDataByUserId(
                Request.DumpUserDataByUserIdRequest request,
                UnityAction<AsyncResult<Result.DumpUserDataByUserIdResult>> callback
        ) =>
            new DumpUserDataByUserIdTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToCoroutine(callback);

        public IFuture<Result.DumpUserDataByUserIdResult> DumpUserDataByUserIdFuture(
                Request.DumpUserDataByUserIdRequest request
        ) =>
            new DumpUserDataByUserIdTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
        public UniTask<Result.DumpUserDataByUserIdResult> DumpUserDataByUserIdAsync(
            Request.DumpUserDataByUserIdRequest request
        ) =>
            new DumpUserDataByUserIdTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsUniTask<Result.DumpUserDataByUserIdResult>();
    #else
        public DumpUserDataByUserIdTask DumpUserDataByUserIdAsync(
                Request.DumpUserDataByUserIdRequest request
        )
        {
            return new DumpUserDataByUserIdTask(
                Gs2WebSocketSession,
                request
            );
        }
    #endif
#else
        public Task<Result.DumpUserDataByUserIdResult> DumpUserDataByUserIdAsync(
            Request.DumpUserDataByUserIdRequest request
        ) =>
            new DumpUserDataByUserIdTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsTask();
#endif


        public class CheckDumpUserDataByUserIdTask : Gs2WebSocketSessionTask<Request.CheckDumpUserDataByUserIdRequest, Result.CheckDumpUserDataByUserIdResult>
        {
            public CheckDumpUserDataByUserIdTask(IGs2Session session, Request.CheckDumpUserDataByUserIdRequest request) : base(session, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(Request.CheckDumpUserDataByUserIdRequest request)
            {
                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);

                jsonWriter.WriteObjectStart();

                if (request.UserId != null)
                {
                    jsonWriter.WritePropertyName("userId");
                    jsonWriter.Write(request.UserId.ToString());
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
                    "stamina",
                    "namespace",
                    "checkDumpUserDataByUserId",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
        public IEnumerator CheckDumpUserDataByUserId(
                Request.CheckDumpUserDataByUserIdRequest request,
                UnityAction<AsyncResult<Result.CheckDumpUserDataByUserIdResult>> callback
        ) =>
            new CheckDumpUserDataByUserIdTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToCoroutine(callback);

        public IFuture<Result.CheckDumpUserDataByUserIdResult> CheckDumpUserDataByUserIdFuture(
                Request.CheckDumpUserDataByUserIdRequest request
        ) =>
            new CheckDumpUserDataByUserIdTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
        public UniTask<Result.CheckDumpUserDataByUserIdResult> CheckDumpUserDataByUserIdAsync(
            Request.CheckDumpUserDataByUserIdRequest request
        ) =>
            new CheckDumpUserDataByUserIdTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsUniTask<Result.CheckDumpUserDataByUserIdResult>();
    #else
        public CheckDumpUserDataByUserIdTask CheckDumpUserDataByUserIdAsync(
                Request.CheckDumpUserDataByUserIdRequest request
        )
        {
            return new CheckDumpUserDataByUserIdTask(
                Gs2WebSocketSession,
                request
            );
        }
    #endif
#else
        public Task<Result.CheckDumpUserDataByUserIdResult> CheckDumpUserDataByUserIdAsync(
            Request.CheckDumpUserDataByUserIdRequest request
        ) =>
            new CheckDumpUserDataByUserIdTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsTask();
#endif


        public class CleanUserDataByUserIdTask : Gs2WebSocketSessionTask<Request.CleanUserDataByUserIdRequest, Result.CleanUserDataByUserIdResult>
        {
            public CleanUserDataByUserIdTask(IGs2Session session, Request.CleanUserDataByUserIdRequest request) : base(session, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(Request.CleanUserDataByUserIdRequest request)
            {
                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);

                jsonWriter.WriteObjectStart();

                if (request.UserId != null)
                {
                    jsonWriter.WritePropertyName("userId");
                    jsonWriter.Write(request.UserId.ToString());
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
                    "stamina",
                    "namespace",
                    "cleanUserDataByUserId",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
        public IEnumerator CleanUserDataByUserId(
                Request.CleanUserDataByUserIdRequest request,
                UnityAction<AsyncResult<Result.CleanUserDataByUserIdResult>> callback
        ) =>
            new CleanUserDataByUserIdTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToCoroutine(callback);

        public IFuture<Result.CleanUserDataByUserIdResult> CleanUserDataByUserIdFuture(
                Request.CleanUserDataByUserIdRequest request
        ) =>
            new CleanUserDataByUserIdTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
        public UniTask<Result.CleanUserDataByUserIdResult> CleanUserDataByUserIdAsync(
            Request.CleanUserDataByUserIdRequest request
        ) =>
            new CleanUserDataByUserIdTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsUniTask<Result.CleanUserDataByUserIdResult>();
    #else
        public CleanUserDataByUserIdTask CleanUserDataByUserIdAsync(
                Request.CleanUserDataByUserIdRequest request
        )
        {
            return new CleanUserDataByUserIdTask(
                Gs2WebSocketSession,
                request
            );
        }
    #endif
#else
        public Task<Result.CleanUserDataByUserIdResult> CleanUserDataByUserIdAsync(
            Request.CleanUserDataByUserIdRequest request
        ) =>
            new CleanUserDataByUserIdTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsTask();
#endif


        public class CheckCleanUserDataByUserIdTask : Gs2WebSocketSessionTask<Request.CheckCleanUserDataByUserIdRequest, Result.CheckCleanUserDataByUserIdResult>
        {
            public CheckCleanUserDataByUserIdTask(IGs2Session session, Request.CheckCleanUserDataByUserIdRequest request) : base(session, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(Request.CheckCleanUserDataByUserIdRequest request)
            {
                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);

                jsonWriter.WriteObjectStart();

                if (request.UserId != null)
                {
                    jsonWriter.WritePropertyName("userId");
                    jsonWriter.Write(request.UserId.ToString());
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
                    "stamina",
                    "namespace",
                    "checkCleanUserDataByUserId",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
        public IEnumerator CheckCleanUserDataByUserId(
                Request.CheckCleanUserDataByUserIdRequest request,
                UnityAction<AsyncResult<Result.CheckCleanUserDataByUserIdResult>> callback
        ) =>
            new CheckCleanUserDataByUserIdTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToCoroutine(callback);

        public IFuture<Result.CheckCleanUserDataByUserIdResult> CheckCleanUserDataByUserIdFuture(
                Request.CheckCleanUserDataByUserIdRequest request
        ) =>
            new CheckCleanUserDataByUserIdTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
        public UniTask<Result.CheckCleanUserDataByUserIdResult> CheckCleanUserDataByUserIdAsync(
            Request.CheckCleanUserDataByUserIdRequest request
        ) =>
            new CheckCleanUserDataByUserIdTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsUniTask<Result.CheckCleanUserDataByUserIdResult>();
    #else
        public CheckCleanUserDataByUserIdTask CheckCleanUserDataByUserIdAsync(
                Request.CheckCleanUserDataByUserIdRequest request
        )
        {
            return new CheckCleanUserDataByUserIdTask(
                Gs2WebSocketSession,
                request
            );
        }
    #endif
#else
        public Task<Result.CheckCleanUserDataByUserIdResult> CheckCleanUserDataByUserIdAsync(
            Request.CheckCleanUserDataByUserIdRequest request
        ) =>
            new CheckCleanUserDataByUserIdTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsTask();
#endif


        public class PrepareImportUserDataByUserIdTask : Gs2WebSocketSessionTask<Request.PrepareImportUserDataByUserIdRequest, Result.PrepareImportUserDataByUserIdResult>
        {
            public PrepareImportUserDataByUserIdTask(IGs2Session session, Request.PrepareImportUserDataByUserIdRequest request) : base(session, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(Request.PrepareImportUserDataByUserIdRequest request)
            {
                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);

                jsonWriter.WriteObjectStart();

                if (request.UserId != null)
                {
                    jsonWriter.WritePropertyName("userId");
                    jsonWriter.Write(request.UserId.ToString());
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
                    "stamina",
                    "namespace",
                    "prepareImportUserDataByUserId",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
        public IEnumerator PrepareImportUserDataByUserId(
                Request.PrepareImportUserDataByUserIdRequest request,
                UnityAction<AsyncResult<Result.PrepareImportUserDataByUserIdResult>> callback
        ) =>
            new PrepareImportUserDataByUserIdTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToCoroutine(callback);

        public IFuture<Result.PrepareImportUserDataByUserIdResult> PrepareImportUserDataByUserIdFuture(
                Request.PrepareImportUserDataByUserIdRequest request
        ) =>
            new PrepareImportUserDataByUserIdTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
        public UniTask<Result.PrepareImportUserDataByUserIdResult> PrepareImportUserDataByUserIdAsync(
            Request.PrepareImportUserDataByUserIdRequest request
        ) =>
            new PrepareImportUserDataByUserIdTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsUniTask<Result.PrepareImportUserDataByUserIdResult>();
    #else
        public PrepareImportUserDataByUserIdTask PrepareImportUserDataByUserIdAsync(
                Request.PrepareImportUserDataByUserIdRequest request
        )
        {
            return new PrepareImportUserDataByUserIdTask(
                Gs2WebSocketSession,
                request
            );
        }
    #endif
#else
        public Task<Result.PrepareImportUserDataByUserIdResult> PrepareImportUserDataByUserIdAsync(
            Request.PrepareImportUserDataByUserIdRequest request
        ) =>
            new PrepareImportUserDataByUserIdTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsTask();
#endif


        public class ImportUserDataByUserIdTask : Gs2WebSocketSessionTask<Request.ImportUserDataByUserIdRequest, Result.ImportUserDataByUserIdResult>
        {
            public ImportUserDataByUserIdTask(IGs2Session session, Request.ImportUserDataByUserIdRequest request) : base(session, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(Request.ImportUserDataByUserIdRequest request)
            {
                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);

                jsonWriter.WriteObjectStart();

                if (request.UserId != null)
                {
                    jsonWriter.WritePropertyName("userId");
                    jsonWriter.Write(request.UserId.ToString());
                }
                if (request.UploadToken != null)
                {
                    jsonWriter.WritePropertyName("uploadToken");
                    jsonWriter.Write(request.UploadToken.ToString());
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
                    "stamina",
                    "namespace",
                    "importUserDataByUserId",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
        public IEnumerator ImportUserDataByUserId(
                Request.ImportUserDataByUserIdRequest request,
                UnityAction<AsyncResult<Result.ImportUserDataByUserIdResult>> callback
        ) =>
            new ImportUserDataByUserIdTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToCoroutine(callback);

        public IFuture<Result.ImportUserDataByUserIdResult> ImportUserDataByUserIdFuture(
                Request.ImportUserDataByUserIdRequest request
        ) =>
            new ImportUserDataByUserIdTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
        public UniTask<Result.ImportUserDataByUserIdResult> ImportUserDataByUserIdAsync(
            Request.ImportUserDataByUserIdRequest request
        ) =>
            new ImportUserDataByUserIdTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsUniTask<Result.ImportUserDataByUserIdResult>();
    #else
        public ImportUserDataByUserIdTask ImportUserDataByUserIdAsync(
                Request.ImportUserDataByUserIdRequest request
        )
        {
            return new ImportUserDataByUserIdTask(
                Gs2WebSocketSession,
                request
            );
        }
    #endif
#else
        public Task<Result.ImportUserDataByUserIdResult> ImportUserDataByUserIdAsync(
            Request.ImportUserDataByUserIdRequest request
        ) =>
            new ImportUserDataByUserIdTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsTask();
#endif


        public class CheckImportUserDataByUserIdTask : Gs2WebSocketSessionTask<Request.CheckImportUserDataByUserIdRequest, Result.CheckImportUserDataByUserIdResult>
        {
            public CheckImportUserDataByUserIdTask(IGs2Session session, Request.CheckImportUserDataByUserIdRequest request) : base(session, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(Request.CheckImportUserDataByUserIdRequest request)
            {
                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);

                jsonWriter.WriteObjectStart();

                if (request.UserId != null)
                {
                    jsonWriter.WritePropertyName("userId");
                    jsonWriter.Write(request.UserId.ToString());
                }
                if (request.UploadToken != null)
                {
                    jsonWriter.WritePropertyName("uploadToken");
                    jsonWriter.Write(request.UploadToken.ToString());
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
                    "stamina",
                    "namespace",
                    "checkImportUserDataByUserId",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
        public IEnumerator CheckImportUserDataByUserId(
                Request.CheckImportUserDataByUserIdRequest request,
                UnityAction<AsyncResult<Result.CheckImportUserDataByUserIdResult>> callback
        ) =>
            new CheckImportUserDataByUserIdTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToCoroutine(callback);

        public IFuture<Result.CheckImportUserDataByUserIdResult> CheckImportUserDataByUserIdFuture(
                Request.CheckImportUserDataByUserIdRequest request
        ) =>
            new CheckImportUserDataByUserIdTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
        public UniTask<Result.CheckImportUserDataByUserIdResult> CheckImportUserDataByUserIdAsync(
            Request.CheckImportUserDataByUserIdRequest request
        ) =>
            new CheckImportUserDataByUserIdTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsUniTask<Result.CheckImportUserDataByUserIdResult>();
    #else
        public CheckImportUserDataByUserIdTask CheckImportUserDataByUserIdAsync(
                Request.CheckImportUserDataByUserIdRequest request
        )
        {
            return new CheckImportUserDataByUserIdTask(
                Gs2WebSocketSession,
                request
            );
        }
    #endif
#else
        public Task<Result.CheckImportUserDataByUserIdResult> CheckImportUserDataByUserIdAsync(
            Request.CheckImportUserDataByUserIdRequest request
        ) =>
            new CheckImportUserDataByUserIdTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsTask();
#endif


        public class CreateStaminaModelMasterTask : Gs2WebSocketSessionTask<Request.CreateStaminaModelMasterRequest, Result.CreateStaminaModelMasterResult>
        {
            public CreateStaminaModelMasterTask(IGs2Session session, Request.CreateStaminaModelMasterRequest request) : base(session, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(Request.CreateStaminaModelMasterRequest request)
            {
                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);

                jsonWriter.WriteObjectStart();

                if (request.NamespaceName != null)
                {
                    jsonWriter.WritePropertyName("namespaceName");
                    jsonWriter.Write(request.NamespaceName.ToString());
                }
                if (request.Name != null)
                {
                    jsonWriter.WritePropertyName("name");
                    jsonWriter.Write(request.Name.ToString());
                }
                if (request.Description != null)
                {
                    jsonWriter.WritePropertyName("description");
                    jsonWriter.Write(request.Description.ToString());
                }
                if (request.Metadata != null)
                {
                    jsonWriter.WritePropertyName("metadata");
                    jsonWriter.Write(request.Metadata.ToString());
                }
                if (request.RecoverIntervalMinutes != null)
                {
                    jsonWriter.WritePropertyName("recoverIntervalMinutes");
                    jsonWriter.Write(request.RecoverIntervalMinutes.ToString());
                }
                if (request.RecoverValue != null)
                {
                    jsonWriter.WritePropertyName("recoverValue");
                    jsonWriter.Write(request.RecoverValue.ToString());
                }
                if (request.InitialCapacity != null)
                {
                    jsonWriter.WritePropertyName("initialCapacity");
                    jsonWriter.Write(request.InitialCapacity.ToString());
                }
                if (request.IsOverflow != null)
                {
                    jsonWriter.WritePropertyName("isOverflow");
                    jsonWriter.Write(request.IsOverflow.ToString());
                }
                if (request.MaxCapacity != null)
                {
                    jsonWriter.WritePropertyName("maxCapacity");
                    jsonWriter.Write(request.MaxCapacity.ToString());
                }
                if (request.MaxStaminaTableName != null)
                {
                    jsonWriter.WritePropertyName("maxStaminaTableName");
                    jsonWriter.Write(request.MaxStaminaTableName.ToString());
                }
                if (request.RecoverIntervalTableName != null)
                {
                    jsonWriter.WritePropertyName("recoverIntervalTableName");
                    jsonWriter.Write(request.RecoverIntervalTableName.ToString());
                }
                if (request.RecoverValueTableName != null)
                {
                    jsonWriter.WritePropertyName("recoverValueTableName");
                    jsonWriter.Write(request.RecoverValueTableName.ToString());
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
                    "stamina",
                    "staminaModelMaster",
                    "createStaminaModelMaster",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
        public IEnumerator CreateStaminaModelMaster(
                Request.CreateStaminaModelMasterRequest request,
                UnityAction<AsyncResult<Result.CreateStaminaModelMasterResult>> callback
        ) =>
            new CreateStaminaModelMasterTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToCoroutine(callback);

        public IFuture<Result.CreateStaminaModelMasterResult> CreateStaminaModelMasterFuture(
                Request.CreateStaminaModelMasterRequest request
        ) =>
            new CreateStaminaModelMasterTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
        public UniTask<Result.CreateStaminaModelMasterResult> CreateStaminaModelMasterAsync(
            Request.CreateStaminaModelMasterRequest request
        ) =>
            new CreateStaminaModelMasterTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsUniTask<Result.CreateStaminaModelMasterResult>();
    #else
        public CreateStaminaModelMasterTask CreateStaminaModelMasterAsync(
                Request.CreateStaminaModelMasterRequest request
        )
        {
            return new CreateStaminaModelMasterTask(
                Gs2WebSocketSession,
                request
            );
        }
    #endif
#else
        public Task<Result.CreateStaminaModelMasterResult> CreateStaminaModelMasterAsync(
            Request.CreateStaminaModelMasterRequest request
        ) =>
            new CreateStaminaModelMasterTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsTask();
#endif


        public class GetStaminaModelMasterTask : Gs2WebSocketSessionTask<Request.GetStaminaModelMasterRequest, Result.GetStaminaModelMasterResult>
        {
            public GetStaminaModelMasterTask(IGs2Session session, Request.GetStaminaModelMasterRequest request) : base(session, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(Request.GetStaminaModelMasterRequest request)
            {
                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);

                jsonWriter.WriteObjectStart();

                if (request.NamespaceName != null)
                {
                    jsonWriter.WritePropertyName("namespaceName");
                    jsonWriter.Write(request.NamespaceName.ToString());
                }
                if (request.StaminaName != null)
                {
                    jsonWriter.WritePropertyName("staminaName");
                    jsonWriter.Write(request.StaminaName.ToString());
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
                    "stamina",
                    "staminaModelMaster",
                    "getStaminaModelMaster",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
        public IEnumerator GetStaminaModelMaster(
                Request.GetStaminaModelMasterRequest request,
                UnityAction<AsyncResult<Result.GetStaminaModelMasterResult>> callback
        ) =>
            new GetStaminaModelMasterTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToCoroutine(callback);

        public IFuture<Result.GetStaminaModelMasterResult> GetStaminaModelMasterFuture(
                Request.GetStaminaModelMasterRequest request
        ) =>
            new GetStaminaModelMasterTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
        public UniTask<Result.GetStaminaModelMasterResult> GetStaminaModelMasterAsync(
            Request.GetStaminaModelMasterRequest request
        ) =>
            new GetStaminaModelMasterTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsUniTask<Result.GetStaminaModelMasterResult>();
    #else
        public GetStaminaModelMasterTask GetStaminaModelMasterAsync(
                Request.GetStaminaModelMasterRequest request
        )
        {
            return new GetStaminaModelMasterTask(
                Gs2WebSocketSession,
                request
            );
        }
    #endif
#else
        public Task<Result.GetStaminaModelMasterResult> GetStaminaModelMasterAsync(
            Request.GetStaminaModelMasterRequest request
        ) =>
            new GetStaminaModelMasterTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsTask();
#endif


        public class UpdateStaminaModelMasterTask : Gs2WebSocketSessionTask<Request.UpdateStaminaModelMasterRequest, Result.UpdateStaminaModelMasterResult>
        {
            public UpdateStaminaModelMasterTask(IGs2Session session, Request.UpdateStaminaModelMasterRequest request) : base(session, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(Request.UpdateStaminaModelMasterRequest request)
            {
                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);

                jsonWriter.WriteObjectStart();

                if (request.NamespaceName != null)
                {
                    jsonWriter.WritePropertyName("namespaceName");
                    jsonWriter.Write(request.NamespaceName.ToString());
                }
                if (request.StaminaName != null)
                {
                    jsonWriter.WritePropertyName("staminaName");
                    jsonWriter.Write(request.StaminaName.ToString());
                }
                if (request.Description != null)
                {
                    jsonWriter.WritePropertyName("description");
                    jsonWriter.Write(request.Description.ToString());
                }
                if (request.Metadata != null)
                {
                    jsonWriter.WritePropertyName("metadata");
                    jsonWriter.Write(request.Metadata.ToString());
                }
                if (request.RecoverIntervalMinutes != null)
                {
                    jsonWriter.WritePropertyName("recoverIntervalMinutes");
                    jsonWriter.Write(request.RecoverIntervalMinutes.ToString());
                }
                if (request.RecoverValue != null)
                {
                    jsonWriter.WritePropertyName("recoverValue");
                    jsonWriter.Write(request.RecoverValue.ToString());
                }
                if (request.InitialCapacity != null)
                {
                    jsonWriter.WritePropertyName("initialCapacity");
                    jsonWriter.Write(request.InitialCapacity.ToString());
                }
                if (request.IsOverflow != null)
                {
                    jsonWriter.WritePropertyName("isOverflow");
                    jsonWriter.Write(request.IsOverflow.ToString());
                }
                if (request.MaxCapacity != null)
                {
                    jsonWriter.WritePropertyName("maxCapacity");
                    jsonWriter.Write(request.MaxCapacity.ToString());
                }
                if (request.MaxStaminaTableName != null)
                {
                    jsonWriter.WritePropertyName("maxStaminaTableName");
                    jsonWriter.Write(request.MaxStaminaTableName.ToString());
                }
                if (request.RecoverIntervalTableName != null)
                {
                    jsonWriter.WritePropertyName("recoverIntervalTableName");
                    jsonWriter.Write(request.RecoverIntervalTableName.ToString());
                }
                if (request.RecoverValueTableName != null)
                {
                    jsonWriter.WritePropertyName("recoverValueTableName");
                    jsonWriter.Write(request.RecoverValueTableName.ToString());
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
                    "stamina",
                    "staminaModelMaster",
                    "updateStaminaModelMaster",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
        public IEnumerator UpdateStaminaModelMaster(
                Request.UpdateStaminaModelMasterRequest request,
                UnityAction<AsyncResult<Result.UpdateStaminaModelMasterResult>> callback
        ) =>
            new UpdateStaminaModelMasterTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToCoroutine(callback);

        public IFuture<Result.UpdateStaminaModelMasterResult> UpdateStaminaModelMasterFuture(
                Request.UpdateStaminaModelMasterRequest request
        ) =>
            new UpdateStaminaModelMasterTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
        public UniTask<Result.UpdateStaminaModelMasterResult> UpdateStaminaModelMasterAsync(
            Request.UpdateStaminaModelMasterRequest request
        ) =>
            new UpdateStaminaModelMasterTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsUniTask<Result.UpdateStaminaModelMasterResult>();
    #else
        public UpdateStaminaModelMasterTask UpdateStaminaModelMasterAsync(
                Request.UpdateStaminaModelMasterRequest request
        )
        {
            return new UpdateStaminaModelMasterTask(
                Gs2WebSocketSession,
                request
            );
        }
    #endif
#else
        public Task<Result.UpdateStaminaModelMasterResult> UpdateStaminaModelMasterAsync(
            Request.UpdateStaminaModelMasterRequest request
        ) =>
            new UpdateStaminaModelMasterTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsTask();
#endif


        public class DeleteStaminaModelMasterTask : Gs2WebSocketSessionTask<Request.DeleteStaminaModelMasterRequest, Result.DeleteStaminaModelMasterResult>
        {
            public DeleteStaminaModelMasterTask(IGs2Session session, Request.DeleteStaminaModelMasterRequest request) : base(session, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(Request.DeleteStaminaModelMasterRequest request)
            {
                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);

                jsonWriter.WriteObjectStart();

                if (request.NamespaceName != null)
                {
                    jsonWriter.WritePropertyName("namespaceName");
                    jsonWriter.Write(request.NamespaceName.ToString());
                }
                if (request.StaminaName != null)
                {
                    jsonWriter.WritePropertyName("staminaName");
                    jsonWriter.Write(request.StaminaName.ToString());
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
                    "stamina",
                    "staminaModelMaster",
                    "deleteStaminaModelMaster",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
        public IEnumerator DeleteStaminaModelMaster(
                Request.DeleteStaminaModelMasterRequest request,
                UnityAction<AsyncResult<Result.DeleteStaminaModelMasterResult>> callback
        ) =>
            new DeleteStaminaModelMasterTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToCoroutine(callback);

        public IFuture<Result.DeleteStaminaModelMasterResult> DeleteStaminaModelMasterFuture(
                Request.DeleteStaminaModelMasterRequest request
        ) =>
            new DeleteStaminaModelMasterTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
        public UniTask<Result.DeleteStaminaModelMasterResult> DeleteStaminaModelMasterAsync(
            Request.DeleteStaminaModelMasterRequest request
        ) =>
            new DeleteStaminaModelMasterTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsUniTask<Result.DeleteStaminaModelMasterResult>();
    #else
        public DeleteStaminaModelMasterTask DeleteStaminaModelMasterAsync(
                Request.DeleteStaminaModelMasterRequest request
        )
        {
            return new DeleteStaminaModelMasterTask(
                Gs2WebSocketSession,
                request
            );
        }
    #endif
#else
        public Task<Result.DeleteStaminaModelMasterResult> DeleteStaminaModelMasterAsync(
            Request.DeleteStaminaModelMasterRequest request
        ) =>
            new DeleteStaminaModelMasterTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsTask();
#endif


        public class CreateMaxStaminaTableMasterTask : Gs2WebSocketSessionTask<Request.CreateMaxStaminaTableMasterRequest, Result.CreateMaxStaminaTableMasterResult>
        {
            public CreateMaxStaminaTableMasterTask(IGs2Session session, Request.CreateMaxStaminaTableMasterRequest request) : base(session, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(Request.CreateMaxStaminaTableMasterRequest request)
            {
                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);

                jsonWriter.WriteObjectStart();

                if (request.NamespaceName != null)
                {
                    jsonWriter.WritePropertyName("namespaceName");
                    jsonWriter.Write(request.NamespaceName.ToString());
                }
                if (request.Name != null)
                {
                    jsonWriter.WritePropertyName("name");
                    jsonWriter.Write(request.Name.ToString());
                }
                if (request.Description != null)
                {
                    jsonWriter.WritePropertyName("description");
                    jsonWriter.Write(request.Description.ToString());
                }
                if (request.Metadata != null)
                {
                    jsonWriter.WritePropertyName("metadata");
                    jsonWriter.Write(request.Metadata.ToString());
                }
                if (request.ExperienceModelId != null)
                {
                    jsonWriter.WritePropertyName("experienceModelId");
                    jsonWriter.Write(request.ExperienceModelId.ToString());
                }
                if (request.Values != null)
                {
                    jsonWriter.WritePropertyName("values");
                    jsonWriter.WriteArrayStart();
                    foreach(var item in request.Values)
                    {
                        jsonWriter.Write(item);
                    }
                    jsonWriter.WriteArrayEnd();
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
                    "stamina",
                    "maxStaminaTableMaster",
                    "createMaxStaminaTableMaster",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
        public IEnumerator CreateMaxStaminaTableMaster(
                Request.CreateMaxStaminaTableMasterRequest request,
                UnityAction<AsyncResult<Result.CreateMaxStaminaTableMasterResult>> callback
        ) =>
            new CreateMaxStaminaTableMasterTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToCoroutine(callback);

        public IFuture<Result.CreateMaxStaminaTableMasterResult> CreateMaxStaminaTableMasterFuture(
                Request.CreateMaxStaminaTableMasterRequest request
        ) =>
            new CreateMaxStaminaTableMasterTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
        public UniTask<Result.CreateMaxStaminaTableMasterResult> CreateMaxStaminaTableMasterAsync(
            Request.CreateMaxStaminaTableMasterRequest request
        ) =>
            new CreateMaxStaminaTableMasterTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsUniTask<Result.CreateMaxStaminaTableMasterResult>();
    #else
        public CreateMaxStaminaTableMasterTask CreateMaxStaminaTableMasterAsync(
                Request.CreateMaxStaminaTableMasterRequest request
        )
        {
            return new CreateMaxStaminaTableMasterTask(
                Gs2WebSocketSession,
                request
            );
        }
    #endif
#else
        public Task<Result.CreateMaxStaminaTableMasterResult> CreateMaxStaminaTableMasterAsync(
            Request.CreateMaxStaminaTableMasterRequest request
        ) =>
            new CreateMaxStaminaTableMasterTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsTask();
#endif


        public class GetMaxStaminaTableMasterTask : Gs2WebSocketSessionTask<Request.GetMaxStaminaTableMasterRequest, Result.GetMaxStaminaTableMasterResult>
        {
            public GetMaxStaminaTableMasterTask(IGs2Session session, Request.GetMaxStaminaTableMasterRequest request) : base(session, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(Request.GetMaxStaminaTableMasterRequest request)
            {
                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);

                jsonWriter.WriteObjectStart();

                if (request.NamespaceName != null)
                {
                    jsonWriter.WritePropertyName("namespaceName");
                    jsonWriter.Write(request.NamespaceName.ToString());
                }
                if (request.MaxStaminaTableName != null)
                {
                    jsonWriter.WritePropertyName("maxStaminaTableName");
                    jsonWriter.Write(request.MaxStaminaTableName.ToString());
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
                    "stamina",
                    "maxStaminaTableMaster",
                    "getMaxStaminaTableMaster",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
        public IEnumerator GetMaxStaminaTableMaster(
                Request.GetMaxStaminaTableMasterRequest request,
                UnityAction<AsyncResult<Result.GetMaxStaminaTableMasterResult>> callback
        ) =>
            new GetMaxStaminaTableMasterTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToCoroutine(callback);

        public IFuture<Result.GetMaxStaminaTableMasterResult> GetMaxStaminaTableMasterFuture(
                Request.GetMaxStaminaTableMasterRequest request
        ) =>
            new GetMaxStaminaTableMasterTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
        public UniTask<Result.GetMaxStaminaTableMasterResult> GetMaxStaminaTableMasterAsync(
            Request.GetMaxStaminaTableMasterRequest request
        ) =>
            new GetMaxStaminaTableMasterTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsUniTask<Result.GetMaxStaminaTableMasterResult>();
    #else
        public GetMaxStaminaTableMasterTask GetMaxStaminaTableMasterAsync(
                Request.GetMaxStaminaTableMasterRequest request
        )
        {
            return new GetMaxStaminaTableMasterTask(
                Gs2WebSocketSession,
                request
            );
        }
    #endif
#else
        public Task<Result.GetMaxStaminaTableMasterResult> GetMaxStaminaTableMasterAsync(
            Request.GetMaxStaminaTableMasterRequest request
        ) =>
            new GetMaxStaminaTableMasterTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsTask();
#endif


        public class UpdateMaxStaminaTableMasterTask : Gs2WebSocketSessionTask<Request.UpdateMaxStaminaTableMasterRequest, Result.UpdateMaxStaminaTableMasterResult>
        {
            public UpdateMaxStaminaTableMasterTask(IGs2Session session, Request.UpdateMaxStaminaTableMasterRequest request) : base(session, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(Request.UpdateMaxStaminaTableMasterRequest request)
            {
                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);

                jsonWriter.WriteObjectStart();

                if (request.NamespaceName != null)
                {
                    jsonWriter.WritePropertyName("namespaceName");
                    jsonWriter.Write(request.NamespaceName.ToString());
                }
                if (request.MaxStaminaTableName != null)
                {
                    jsonWriter.WritePropertyName("maxStaminaTableName");
                    jsonWriter.Write(request.MaxStaminaTableName.ToString());
                }
                if (request.Description != null)
                {
                    jsonWriter.WritePropertyName("description");
                    jsonWriter.Write(request.Description.ToString());
                }
                if (request.Metadata != null)
                {
                    jsonWriter.WritePropertyName("metadata");
                    jsonWriter.Write(request.Metadata.ToString());
                }
                if (request.ExperienceModelId != null)
                {
                    jsonWriter.WritePropertyName("experienceModelId");
                    jsonWriter.Write(request.ExperienceModelId.ToString());
                }
                if (request.Values != null)
                {
                    jsonWriter.WritePropertyName("values");
                    jsonWriter.WriteArrayStart();
                    foreach(var item in request.Values)
                    {
                        jsonWriter.Write(item);
                    }
                    jsonWriter.WriteArrayEnd();
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
                    "stamina",
                    "maxStaminaTableMaster",
                    "updateMaxStaminaTableMaster",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
        public IEnumerator UpdateMaxStaminaTableMaster(
                Request.UpdateMaxStaminaTableMasterRequest request,
                UnityAction<AsyncResult<Result.UpdateMaxStaminaTableMasterResult>> callback
        ) =>
            new UpdateMaxStaminaTableMasterTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToCoroutine(callback);

        public IFuture<Result.UpdateMaxStaminaTableMasterResult> UpdateMaxStaminaTableMasterFuture(
                Request.UpdateMaxStaminaTableMasterRequest request
        ) =>
            new UpdateMaxStaminaTableMasterTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
        public UniTask<Result.UpdateMaxStaminaTableMasterResult> UpdateMaxStaminaTableMasterAsync(
            Request.UpdateMaxStaminaTableMasterRequest request
        ) =>
            new UpdateMaxStaminaTableMasterTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsUniTask<Result.UpdateMaxStaminaTableMasterResult>();
    #else
        public UpdateMaxStaminaTableMasterTask UpdateMaxStaminaTableMasterAsync(
                Request.UpdateMaxStaminaTableMasterRequest request
        )
        {
            return new UpdateMaxStaminaTableMasterTask(
                Gs2WebSocketSession,
                request
            );
        }
    #endif
#else
        public Task<Result.UpdateMaxStaminaTableMasterResult> UpdateMaxStaminaTableMasterAsync(
            Request.UpdateMaxStaminaTableMasterRequest request
        ) =>
            new UpdateMaxStaminaTableMasterTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsTask();
#endif


        public class DeleteMaxStaminaTableMasterTask : Gs2WebSocketSessionTask<Request.DeleteMaxStaminaTableMasterRequest, Result.DeleteMaxStaminaTableMasterResult>
        {
            public DeleteMaxStaminaTableMasterTask(IGs2Session session, Request.DeleteMaxStaminaTableMasterRequest request) : base(session, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(Request.DeleteMaxStaminaTableMasterRequest request)
            {
                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);

                jsonWriter.WriteObjectStart();

                if (request.NamespaceName != null)
                {
                    jsonWriter.WritePropertyName("namespaceName");
                    jsonWriter.Write(request.NamespaceName.ToString());
                }
                if (request.MaxStaminaTableName != null)
                {
                    jsonWriter.WritePropertyName("maxStaminaTableName");
                    jsonWriter.Write(request.MaxStaminaTableName.ToString());
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
                    "stamina",
                    "maxStaminaTableMaster",
                    "deleteMaxStaminaTableMaster",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
        public IEnumerator DeleteMaxStaminaTableMaster(
                Request.DeleteMaxStaminaTableMasterRequest request,
                UnityAction<AsyncResult<Result.DeleteMaxStaminaTableMasterResult>> callback
        ) =>
            new DeleteMaxStaminaTableMasterTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToCoroutine(callback);

        public IFuture<Result.DeleteMaxStaminaTableMasterResult> DeleteMaxStaminaTableMasterFuture(
                Request.DeleteMaxStaminaTableMasterRequest request
        ) =>
            new DeleteMaxStaminaTableMasterTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
        public UniTask<Result.DeleteMaxStaminaTableMasterResult> DeleteMaxStaminaTableMasterAsync(
            Request.DeleteMaxStaminaTableMasterRequest request
        ) =>
            new DeleteMaxStaminaTableMasterTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsUniTask<Result.DeleteMaxStaminaTableMasterResult>();
    #else
        public DeleteMaxStaminaTableMasterTask DeleteMaxStaminaTableMasterAsync(
                Request.DeleteMaxStaminaTableMasterRequest request
        )
        {
            return new DeleteMaxStaminaTableMasterTask(
                Gs2WebSocketSession,
                request
            );
        }
    #endif
#else
        public Task<Result.DeleteMaxStaminaTableMasterResult> DeleteMaxStaminaTableMasterAsync(
            Request.DeleteMaxStaminaTableMasterRequest request
        ) =>
            new DeleteMaxStaminaTableMasterTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsTask();
#endif


        public class CreateRecoverIntervalTableMasterTask : Gs2WebSocketSessionTask<Request.CreateRecoverIntervalTableMasterRequest, Result.CreateRecoverIntervalTableMasterResult>
        {
            public CreateRecoverIntervalTableMasterTask(IGs2Session session, Request.CreateRecoverIntervalTableMasterRequest request) : base(session, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(Request.CreateRecoverIntervalTableMasterRequest request)
            {
                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);

                jsonWriter.WriteObjectStart();

                if (request.NamespaceName != null)
                {
                    jsonWriter.WritePropertyName("namespaceName");
                    jsonWriter.Write(request.NamespaceName.ToString());
                }
                if (request.Name != null)
                {
                    jsonWriter.WritePropertyName("name");
                    jsonWriter.Write(request.Name.ToString());
                }
                if (request.Description != null)
                {
                    jsonWriter.WritePropertyName("description");
                    jsonWriter.Write(request.Description.ToString());
                }
                if (request.Metadata != null)
                {
                    jsonWriter.WritePropertyName("metadata");
                    jsonWriter.Write(request.Metadata.ToString());
                }
                if (request.ExperienceModelId != null)
                {
                    jsonWriter.WritePropertyName("experienceModelId");
                    jsonWriter.Write(request.ExperienceModelId.ToString());
                }
                if (request.Values != null)
                {
                    jsonWriter.WritePropertyName("values");
                    jsonWriter.WriteArrayStart();
                    foreach(var item in request.Values)
                    {
                        jsonWriter.Write(item);
                    }
                    jsonWriter.WriteArrayEnd();
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
                    "stamina",
                    "recoverIntervalTableMaster",
                    "createRecoverIntervalTableMaster",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
        public IEnumerator CreateRecoverIntervalTableMaster(
                Request.CreateRecoverIntervalTableMasterRequest request,
                UnityAction<AsyncResult<Result.CreateRecoverIntervalTableMasterResult>> callback
        ) =>
            new CreateRecoverIntervalTableMasterTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToCoroutine(callback);

        public IFuture<Result.CreateRecoverIntervalTableMasterResult> CreateRecoverIntervalTableMasterFuture(
                Request.CreateRecoverIntervalTableMasterRequest request
        ) =>
            new CreateRecoverIntervalTableMasterTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
        public UniTask<Result.CreateRecoverIntervalTableMasterResult> CreateRecoverIntervalTableMasterAsync(
            Request.CreateRecoverIntervalTableMasterRequest request
        ) =>
            new CreateRecoverIntervalTableMasterTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsUniTask<Result.CreateRecoverIntervalTableMasterResult>();
    #else
        public CreateRecoverIntervalTableMasterTask CreateRecoverIntervalTableMasterAsync(
                Request.CreateRecoverIntervalTableMasterRequest request
        )
        {
            return new CreateRecoverIntervalTableMasterTask(
                Gs2WebSocketSession,
                request
            );
        }
    #endif
#else
        public Task<Result.CreateRecoverIntervalTableMasterResult> CreateRecoverIntervalTableMasterAsync(
            Request.CreateRecoverIntervalTableMasterRequest request
        ) =>
            new CreateRecoverIntervalTableMasterTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsTask();
#endif


        public class GetRecoverIntervalTableMasterTask : Gs2WebSocketSessionTask<Request.GetRecoverIntervalTableMasterRequest, Result.GetRecoverIntervalTableMasterResult>
        {
            public GetRecoverIntervalTableMasterTask(IGs2Session session, Request.GetRecoverIntervalTableMasterRequest request) : base(session, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(Request.GetRecoverIntervalTableMasterRequest request)
            {
                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);

                jsonWriter.WriteObjectStart();

                if (request.NamespaceName != null)
                {
                    jsonWriter.WritePropertyName("namespaceName");
                    jsonWriter.Write(request.NamespaceName.ToString());
                }
                if (request.RecoverIntervalTableName != null)
                {
                    jsonWriter.WritePropertyName("recoverIntervalTableName");
                    jsonWriter.Write(request.RecoverIntervalTableName.ToString());
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
                    "stamina",
                    "recoverIntervalTableMaster",
                    "getRecoverIntervalTableMaster",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
        public IEnumerator GetRecoverIntervalTableMaster(
                Request.GetRecoverIntervalTableMasterRequest request,
                UnityAction<AsyncResult<Result.GetRecoverIntervalTableMasterResult>> callback
        ) =>
            new GetRecoverIntervalTableMasterTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToCoroutine(callback);

        public IFuture<Result.GetRecoverIntervalTableMasterResult> GetRecoverIntervalTableMasterFuture(
                Request.GetRecoverIntervalTableMasterRequest request
        ) =>
            new GetRecoverIntervalTableMasterTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
        public UniTask<Result.GetRecoverIntervalTableMasterResult> GetRecoverIntervalTableMasterAsync(
            Request.GetRecoverIntervalTableMasterRequest request
        ) =>
            new GetRecoverIntervalTableMasterTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsUniTask<Result.GetRecoverIntervalTableMasterResult>();
    #else
        public GetRecoverIntervalTableMasterTask GetRecoverIntervalTableMasterAsync(
                Request.GetRecoverIntervalTableMasterRequest request
        )
        {
            return new GetRecoverIntervalTableMasterTask(
                Gs2WebSocketSession,
                request
            );
        }
    #endif
#else
        public Task<Result.GetRecoverIntervalTableMasterResult> GetRecoverIntervalTableMasterAsync(
            Request.GetRecoverIntervalTableMasterRequest request
        ) =>
            new GetRecoverIntervalTableMasterTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsTask();
#endif


        public class UpdateRecoverIntervalTableMasterTask : Gs2WebSocketSessionTask<Request.UpdateRecoverIntervalTableMasterRequest, Result.UpdateRecoverIntervalTableMasterResult>
        {
            public UpdateRecoverIntervalTableMasterTask(IGs2Session session, Request.UpdateRecoverIntervalTableMasterRequest request) : base(session, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(Request.UpdateRecoverIntervalTableMasterRequest request)
            {
                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);

                jsonWriter.WriteObjectStart();

                if (request.NamespaceName != null)
                {
                    jsonWriter.WritePropertyName("namespaceName");
                    jsonWriter.Write(request.NamespaceName.ToString());
                }
                if (request.RecoverIntervalTableName != null)
                {
                    jsonWriter.WritePropertyName("recoverIntervalTableName");
                    jsonWriter.Write(request.RecoverIntervalTableName.ToString());
                }
                if (request.Description != null)
                {
                    jsonWriter.WritePropertyName("description");
                    jsonWriter.Write(request.Description.ToString());
                }
                if (request.Metadata != null)
                {
                    jsonWriter.WritePropertyName("metadata");
                    jsonWriter.Write(request.Metadata.ToString());
                }
                if (request.ExperienceModelId != null)
                {
                    jsonWriter.WritePropertyName("experienceModelId");
                    jsonWriter.Write(request.ExperienceModelId.ToString());
                }
                if (request.Values != null)
                {
                    jsonWriter.WritePropertyName("values");
                    jsonWriter.WriteArrayStart();
                    foreach(var item in request.Values)
                    {
                        jsonWriter.Write(item);
                    }
                    jsonWriter.WriteArrayEnd();
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
                    "stamina",
                    "recoverIntervalTableMaster",
                    "updateRecoverIntervalTableMaster",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
        public IEnumerator UpdateRecoverIntervalTableMaster(
                Request.UpdateRecoverIntervalTableMasterRequest request,
                UnityAction<AsyncResult<Result.UpdateRecoverIntervalTableMasterResult>> callback
        ) =>
            new UpdateRecoverIntervalTableMasterTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToCoroutine(callback);

        public IFuture<Result.UpdateRecoverIntervalTableMasterResult> UpdateRecoverIntervalTableMasterFuture(
                Request.UpdateRecoverIntervalTableMasterRequest request
        ) =>
            new UpdateRecoverIntervalTableMasterTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
        public UniTask<Result.UpdateRecoverIntervalTableMasterResult> UpdateRecoverIntervalTableMasterAsync(
            Request.UpdateRecoverIntervalTableMasterRequest request
        ) =>
            new UpdateRecoverIntervalTableMasterTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsUniTask<Result.UpdateRecoverIntervalTableMasterResult>();
    #else
        public UpdateRecoverIntervalTableMasterTask UpdateRecoverIntervalTableMasterAsync(
                Request.UpdateRecoverIntervalTableMasterRequest request
        )
        {
            return new UpdateRecoverIntervalTableMasterTask(
                Gs2WebSocketSession,
                request
            );
        }
    #endif
#else
        public Task<Result.UpdateRecoverIntervalTableMasterResult> UpdateRecoverIntervalTableMasterAsync(
            Request.UpdateRecoverIntervalTableMasterRequest request
        ) =>
            new UpdateRecoverIntervalTableMasterTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsTask();
#endif


        public class DeleteRecoverIntervalTableMasterTask : Gs2WebSocketSessionTask<Request.DeleteRecoverIntervalTableMasterRequest, Result.DeleteRecoverIntervalTableMasterResult>
        {
            public DeleteRecoverIntervalTableMasterTask(IGs2Session session, Request.DeleteRecoverIntervalTableMasterRequest request) : base(session, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(Request.DeleteRecoverIntervalTableMasterRequest request)
            {
                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);

                jsonWriter.WriteObjectStart();

                if (request.NamespaceName != null)
                {
                    jsonWriter.WritePropertyName("namespaceName");
                    jsonWriter.Write(request.NamespaceName.ToString());
                }
                if (request.RecoverIntervalTableName != null)
                {
                    jsonWriter.WritePropertyName("recoverIntervalTableName");
                    jsonWriter.Write(request.RecoverIntervalTableName.ToString());
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
                    "stamina",
                    "recoverIntervalTableMaster",
                    "deleteRecoverIntervalTableMaster",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
        public IEnumerator DeleteRecoverIntervalTableMaster(
                Request.DeleteRecoverIntervalTableMasterRequest request,
                UnityAction<AsyncResult<Result.DeleteRecoverIntervalTableMasterResult>> callback
        ) =>
            new DeleteRecoverIntervalTableMasterTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToCoroutine(callback);

        public IFuture<Result.DeleteRecoverIntervalTableMasterResult> DeleteRecoverIntervalTableMasterFuture(
                Request.DeleteRecoverIntervalTableMasterRequest request
        ) =>
            new DeleteRecoverIntervalTableMasterTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
        public UniTask<Result.DeleteRecoverIntervalTableMasterResult> DeleteRecoverIntervalTableMasterAsync(
            Request.DeleteRecoverIntervalTableMasterRequest request
        ) =>
            new DeleteRecoverIntervalTableMasterTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsUniTask<Result.DeleteRecoverIntervalTableMasterResult>();
    #else
        public DeleteRecoverIntervalTableMasterTask DeleteRecoverIntervalTableMasterAsync(
                Request.DeleteRecoverIntervalTableMasterRequest request
        )
        {
            return new DeleteRecoverIntervalTableMasterTask(
                Gs2WebSocketSession,
                request
            );
        }
    #endif
#else
        public Task<Result.DeleteRecoverIntervalTableMasterResult> DeleteRecoverIntervalTableMasterAsync(
            Request.DeleteRecoverIntervalTableMasterRequest request
        ) =>
            new DeleteRecoverIntervalTableMasterTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsTask();
#endif


        public class CreateRecoverValueTableMasterTask : Gs2WebSocketSessionTask<Request.CreateRecoverValueTableMasterRequest, Result.CreateRecoverValueTableMasterResult>
        {
            public CreateRecoverValueTableMasterTask(IGs2Session session, Request.CreateRecoverValueTableMasterRequest request) : base(session, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(Request.CreateRecoverValueTableMasterRequest request)
            {
                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);

                jsonWriter.WriteObjectStart();

                if (request.NamespaceName != null)
                {
                    jsonWriter.WritePropertyName("namespaceName");
                    jsonWriter.Write(request.NamespaceName.ToString());
                }
                if (request.Name != null)
                {
                    jsonWriter.WritePropertyName("name");
                    jsonWriter.Write(request.Name.ToString());
                }
                if (request.Description != null)
                {
                    jsonWriter.WritePropertyName("description");
                    jsonWriter.Write(request.Description.ToString());
                }
                if (request.Metadata != null)
                {
                    jsonWriter.WritePropertyName("metadata");
                    jsonWriter.Write(request.Metadata.ToString());
                }
                if (request.ExperienceModelId != null)
                {
                    jsonWriter.WritePropertyName("experienceModelId");
                    jsonWriter.Write(request.ExperienceModelId.ToString());
                }
                if (request.Values != null)
                {
                    jsonWriter.WritePropertyName("values");
                    jsonWriter.WriteArrayStart();
                    foreach(var item in request.Values)
                    {
                        jsonWriter.Write(item);
                    }
                    jsonWriter.WriteArrayEnd();
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
                    "stamina",
                    "recoverValueTableMaster",
                    "createRecoverValueTableMaster",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
        public IEnumerator CreateRecoverValueTableMaster(
                Request.CreateRecoverValueTableMasterRequest request,
                UnityAction<AsyncResult<Result.CreateRecoverValueTableMasterResult>> callback
        ) =>
            new CreateRecoverValueTableMasterTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToCoroutine(callback);

        public IFuture<Result.CreateRecoverValueTableMasterResult> CreateRecoverValueTableMasterFuture(
                Request.CreateRecoverValueTableMasterRequest request
        ) =>
            new CreateRecoverValueTableMasterTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
        public UniTask<Result.CreateRecoverValueTableMasterResult> CreateRecoverValueTableMasterAsync(
            Request.CreateRecoverValueTableMasterRequest request
        ) =>
            new CreateRecoverValueTableMasterTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsUniTask<Result.CreateRecoverValueTableMasterResult>();
    #else
        public CreateRecoverValueTableMasterTask CreateRecoverValueTableMasterAsync(
                Request.CreateRecoverValueTableMasterRequest request
        )
        {
            return new CreateRecoverValueTableMasterTask(
                Gs2WebSocketSession,
                request
            );
        }
    #endif
#else
        public Task<Result.CreateRecoverValueTableMasterResult> CreateRecoverValueTableMasterAsync(
            Request.CreateRecoverValueTableMasterRequest request
        ) =>
            new CreateRecoverValueTableMasterTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsTask();
#endif


        public class GetRecoverValueTableMasterTask : Gs2WebSocketSessionTask<Request.GetRecoverValueTableMasterRequest, Result.GetRecoverValueTableMasterResult>
        {
            public GetRecoverValueTableMasterTask(IGs2Session session, Request.GetRecoverValueTableMasterRequest request) : base(session, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(Request.GetRecoverValueTableMasterRequest request)
            {
                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);

                jsonWriter.WriteObjectStart();

                if (request.NamespaceName != null)
                {
                    jsonWriter.WritePropertyName("namespaceName");
                    jsonWriter.Write(request.NamespaceName.ToString());
                }
                if (request.RecoverValueTableName != null)
                {
                    jsonWriter.WritePropertyName("recoverValueTableName");
                    jsonWriter.Write(request.RecoverValueTableName.ToString());
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
                    "stamina",
                    "recoverValueTableMaster",
                    "getRecoverValueTableMaster",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
        public IEnumerator GetRecoverValueTableMaster(
                Request.GetRecoverValueTableMasterRequest request,
                UnityAction<AsyncResult<Result.GetRecoverValueTableMasterResult>> callback
        ) =>
            new GetRecoverValueTableMasterTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToCoroutine(callback);

        public IFuture<Result.GetRecoverValueTableMasterResult> GetRecoverValueTableMasterFuture(
                Request.GetRecoverValueTableMasterRequest request
        ) =>
            new GetRecoverValueTableMasterTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
        public UniTask<Result.GetRecoverValueTableMasterResult> GetRecoverValueTableMasterAsync(
            Request.GetRecoverValueTableMasterRequest request
        ) =>
            new GetRecoverValueTableMasterTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsUniTask<Result.GetRecoverValueTableMasterResult>();
    #else
        public GetRecoverValueTableMasterTask GetRecoverValueTableMasterAsync(
                Request.GetRecoverValueTableMasterRequest request
        )
        {
            return new GetRecoverValueTableMasterTask(
                Gs2WebSocketSession,
                request
            );
        }
    #endif
#else
        public Task<Result.GetRecoverValueTableMasterResult> GetRecoverValueTableMasterAsync(
            Request.GetRecoverValueTableMasterRequest request
        ) =>
            new GetRecoverValueTableMasterTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsTask();
#endif


        public class UpdateRecoverValueTableMasterTask : Gs2WebSocketSessionTask<Request.UpdateRecoverValueTableMasterRequest, Result.UpdateRecoverValueTableMasterResult>
        {
            public UpdateRecoverValueTableMasterTask(IGs2Session session, Request.UpdateRecoverValueTableMasterRequest request) : base(session, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(Request.UpdateRecoverValueTableMasterRequest request)
            {
                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);

                jsonWriter.WriteObjectStart();

                if (request.NamespaceName != null)
                {
                    jsonWriter.WritePropertyName("namespaceName");
                    jsonWriter.Write(request.NamespaceName.ToString());
                }
                if (request.RecoverValueTableName != null)
                {
                    jsonWriter.WritePropertyName("recoverValueTableName");
                    jsonWriter.Write(request.RecoverValueTableName.ToString());
                }
                if (request.Description != null)
                {
                    jsonWriter.WritePropertyName("description");
                    jsonWriter.Write(request.Description.ToString());
                }
                if (request.Metadata != null)
                {
                    jsonWriter.WritePropertyName("metadata");
                    jsonWriter.Write(request.Metadata.ToString());
                }
                if (request.ExperienceModelId != null)
                {
                    jsonWriter.WritePropertyName("experienceModelId");
                    jsonWriter.Write(request.ExperienceModelId.ToString());
                }
                if (request.Values != null)
                {
                    jsonWriter.WritePropertyName("values");
                    jsonWriter.WriteArrayStart();
                    foreach(var item in request.Values)
                    {
                        jsonWriter.Write(item);
                    }
                    jsonWriter.WriteArrayEnd();
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
                    "stamina",
                    "recoverValueTableMaster",
                    "updateRecoverValueTableMaster",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
        public IEnumerator UpdateRecoverValueTableMaster(
                Request.UpdateRecoverValueTableMasterRequest request,
                UnityAction<AsyncResult<Result.UpdateRecoverValueTableMasterResult>> callback
        ) =>
            new UpdateRecoverValueTableMasterTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToCoroutine(callback);

        public IFuture<Result.UpdateRecoverValueTableMasterResult> UpdateRecoverValueTableMasterFuture(
                Request.UpdateRecoverValueTableMasterRequest request
        ) =>
            new UpdateRecoverValueTableMasterTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
        public UniTask<Result.UpdateRecoverValueTableMasterResult> UpdateRecoverValueTableMasterAsync(
            Request.UpdateRecoverValueTableMasterRequest request
        ) =>
            new UpdateRecoverValueTableMasterTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsUniTask<Result.UpdateRecoverValueTableMasterResult>();
    #else
        public UpdateRecoverValueTableMasterTask UpdateRecoverValueTableMasterAsync(
                Request.UpdateRecoverValueTableMasterRequest request
        )
        {
            return new UpdateRecoverValueTableMasterTask(
                Gs2WebSocketSession,
                request
            );
        }
    #endif
#else
        public Task<Result.UpdateRecoverValueTableMasterResult> UpdateRecoverValueTableMasterAsync(
            Request.UpdateRecoverValueTableMasterRequest request
        ) =>
            new UpdateRecoverValueTableMasterTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsTask();
#endif


        public class DeleteRecoverValueTableMasterTask : Gs2WebSocketSessionTask<Request.DeleteRecoverValueTableMasterRequest, Result.DeleteRecoverValueTableMasterResult>
        {
            public DeleteRecoverValueTableMasterTask(IGs2Session session, Request.DeleteRecoverValueTableMasterRequest request) : base(session, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(Request.DeleteRecoverValueTableMasterRequest request)
            {
                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);

                jsonWriter.WriteObjectStart();

                if (request.NamespaceName != null)
                {
                    jsonWriter.WritePropertyName("namespaceName");
                    jsonWriter.Write(request.NamespaceName.ToString());
                }
                if (request.RecoverValueTableName != null)
                {
                    jsonWriter.WritePropertyName("recoverValueTableName");
                    jsonWriter.Write(request.RecoverValueTableName.ToString());
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
                    "stamina",
                    "recoverValueTableMaster",
                    "deleteRecoverValueTableMaster",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
        public IEnumerator DeleteRecoverValueTableMaster(
                Request.DeleteRecoverValueTableMasterRequest request,
                UnityAction<AsyncResult<Result.DeleteRecoverValueTableMasterResult>> callback
        ) =>
            new DeleteRecoverValueTableMasterTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToCoroutine(callback);

        public IFuture<Result.DeleteRecoverValueTableMasterResult> DeleteRecoverValueTableMasterFuture(
                Request.DeleteRecoverValueTableMasterRequest request
        ) =>
            new DeleteRecoverValueTableMasterTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
        public UniTask<Result.DeleteRecoverValueTableMasterResult> DeleteRecoverValueTableMasterAsync(
            Request.DeleteRecoverValueTableMasterRequest request
        ) =>
            new DeleteRecoverValueTableMasterTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsUniTask<Result.DeleteRecoverValueTableMasterResult>();
    #else
        public DeleteRecoverValueTableMasterTask DeleteRecoverValueTableMasterAsync(
                Request.DeleteRecoverValueTableMasterRequest request
        )
        {
            return new DeleteRecoverValueTableMasterTask(
                Gs2WebSocketSession,
                request
            );
        }
    #endif
#else
        public Task<Result.DeleteRecoverValueTableMasterResult> DeleteRecoverValueTableMasterAsync(
            Request.DeleteRecoverValueTableMasterRequest request
        ) =>
            new DeleteRecoverValueTableMasterTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsTask();
#endif


        public class PreUpdateCurrentStaminaMasterTask : Gs2WebSocketSessionTask<Request.PreUpdateCurrentStaminaMasterRequest, Result.PreUpdateCurrentStaminaMasterResult>
        {
            public PreUpdateCurrentStaminaMasterTask(IGs2Session session, Request.PreUpdateCurrentStaminaMasterRequest request) : base(session, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(Request.PreUpdateCurrentStaminaMasterRequest request)
            {
                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);

                jsonWriter.WriteObjectStart();

                if (request.NamespaceName != null)
                {
                    jsonWriter.WritePropertyName("namespaceName");
                    jsonWriter.Write(request.NamespaceName.ToString());
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
                    "stamina",
                    "currentStaminaMaster",
                    "preUpdateCurrentStaminaMaster",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
        public IEnumerator PreUpdateCurrentStaminaMaster(
                Request.PreUpdateCurrentStaminaMasterRequest request,
                UnityAction<AsyncResult<Result.PreUpdateCurrentStaminaMasterResult>> callback
        ) =>
            new PreUpdateCurrentStaminaMasterTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToCoroutine(callback);

        public IFuture<Result.PreUpdateCurrentStaminaMasterResult> PreUpdateCurrentStaminaMasterFuture(
                Request.PreUpdateCurrentStaminaMasterRequest request
        ) =>
            new PreUpdateCurrentStaminaMasterTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
        public UniTask<Result.PreUpdateCurrentStaminaMasterResult> PreUpdateCurrentStaminaMasterAsync(
            Request.PreUpdateCurrentStaminaMasterRequest request
        ) =>
            new PreUpdateCurrentStaminaMasterTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsUniTask<Result.PreUpdateCurrentStaminaMasterResult>();
    #else
        public PreUpdateCurrentStaminaMasterTask PreUpdateCurrentStaminaMasterAsync(
                Request.PreUpdateCurrentStaminaMasterRequest request
        )
        {
            return new PreUpdateCurrentStaminaMasterTask(
                Gs2WebSocketSession,
                request
            );
        }
    #endif
#else
        public Task<Result.PreUpdateCurrentStaminaMasterResult> PreUpdateCurrentStaminaMasterAsync(
            Request.PreUpdateCurrentStaminaMasterRequest request
        ) =>
            new PreUpdateCurrentStaminaMasterTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsTask();
#endif


        public class GetStaminaModelTask : Gs2WebSocketSessionTask<Request.GetStaminaModelRequest, Result.GetStaminaModelResult>
        {
            public GetStaminaModelTask(IGs2Session session, Request.GetStaminaModelRequest request) : base(session, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(Request.GetStaminaModelRequest request)
            {
                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);

                jsonWriter.WriteObjectStart();

                if (request.NamespaceName != null)
                {
                    jsonWriter.WritePropertyName("namespaceName");
                    jsonWriter.Write(request.NamespaceName.ToString());
                }
                if (request.StaminaName != null)
                {
                    jsonWriter.WritePropertyName("staminaName");
                    jsonWriter.Write(request.StaminaName.ToString());
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
                    "stamina",
                    "staminaModel",
                    "getStaminaModel",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
        public IEnumerator GetStaminaModel(
                Request.GetStaminaModelRequest request,
                UnityAction<AsyncResult<Result.GetStaminaModelResult>> callback
        ) =>
            new GetStaminaModelTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToCoroutine(callback);

        public IFuture<Result.GetStaminaModelResult> GetStaminaModelFuture(
                Request.GetStaminaModelRequest request
        ) =>
            new GetStaminaModelTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
        public UniTask<Result.GetStaminaModelResult> GetStaminaModelAsync(
            Request.GetStaminaModelRequest request
        ) =>
            new GetStaminaModelTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsUniTask<Result.GetStaminaModelResult>();
    #else
        public GetStaminaModelTask GetStaminaModelAsync(
                Request.GetStaminaModelRequest request
        )
        {
            return new GetStaminaModelTask(
                Gs2WebSocketSession,
                request
            );
        }
    #endif
#else
        public Task<Result.GetStaminaModelResult> GetStaminaModelAsync(
            Request.GetStaminaModelRequest request
        ) =>
            new GetStaminaModelTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsTask();
#endif


        public class GetStaminaTask : Gs2WebSocketSessionTask<Request.GetStaminaRequest, Result.GetStaminaResult>
        {
            public GetStaminaTask(IGs2Session session, Request.GetStaminaRequest request) : base(session, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(Request.GetStaminaRequest request)
            {
                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);

                jsonWriter.WriteObjectStart();

                if (request.NamespaceName != null)
                {
                    jsonWriter.WritePropertyName("namespaceName");
                    jsonWriter.Write(request.NamespaceName.ToString());
                }
                if (request.StaminaName != null)
                {
                    jsonWriter.WritePropertyName("staminaName");
                    jsonWriter.Write(request.StaminaName.ToString());
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
                if (request.AccessToken != null)
                {
                    jsonWriter.WritePropertyName("xGs2AccessToken");
                    jsonWriter.Write(request.AccessToken);
                }
                if (request.DryRun)
                {
                    jsonWriter.WritePropertyName("xGs2DryRun");
                    jsonWriter.Write("true");
                }

                AddHeader(
                    Session.Credential,
                    "stamina",
                    "stamina",
                    "getStamina",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
        public IEnumerator GetStamina(
                Request.GetStaminaRequest request,
                UnityAction<AsyncResult<Result.GetStaminaResult>> callback
        ) =>
            new GetStaminaTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToCoroutine(callback);

        public IFuture<Result.GetStaminaResult> GetStaminaFuture(
                Request.GetStaminaRequest request
        ) =>
            new GetStaminaTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
        public UniTask<Result.GetStaminaResult> GetStaminaAsync(
            Request.GetStaminaRequest request
        ) =>
            new GetStaminaTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsUniTask<Result.GetStaminaResult>();
    #else
        public GetStaminaTask GetStaminaAsync(
                Request.GetStaminaRequest request
        )
        {
            return new GetStaminaTask(
                Gs2WebSocketSession,
                request
            );
        }
    #endif
#else
        public Task<Result.GetStaminaResult> GetStaminaAsync(
            Request.GetStaminaRequest request
        ) =>
            new GetStaminaTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsTask();
#endif


        public class GetStaminaByUserIdTask : Gs2WebSocketSessionTask<Request.GetStaminaByUserIdRequest, Result.GetStaminaByUserIdResult>
        {
            public GetStaminaByUserIdTask(IGs2Session session, Request.GetStaminaByUserIdRequest request) : base(session, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(Request.GetStaminaByUserIdRequest request)
            {
                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);

                jsonWriter.WriteObjectStart();

                if (request.NamespaceName != null)
                {
                    jsonWriter.WritePropertyName("namespaceName");
                    jsonWriter.Write(request.NamespaceName.ToString());
                }
                if (request.StaminaName != null)
                {
                    jsonWriter.WritePropertyName("staminaName");
                    jsonWriter.Write(request.StaminaName.ToString());
                }
                if (request.UserId != null)
                {
                    jsonWriter.WritePropertyName("userId");
                    jsonWriter.Write(request.UserId.ToString());
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
                    "stamina",
                    "stamina",
                    "getStaminaByUserId",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
        public IEnumerator GetStaminaByUserId(
                Request.GetStaminaByUserIdRequest request,
                UnityAction<AsyncResult<Result.GetStaminaByUserIdResult>> callback
        ) =>
            new GetStaminaByUserIdTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToCoroutine(callback);

        public IFuture<Result.GetStaminaByUserIdResult> GetStaminaByUserIdFuture(
                Request.GetStaminaByUserIdRequest request
        ) =>
            new GetStaminaByUserIdTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
        public UniTask<Result.GetStaminaByUserIdResult> GetStaminaByUserIdAsync(
            Request.GetStaminaByUserIdRequest request
        ) =>
            new GetStaminaByUserIdTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsUniTask<Result.GetStaminaByUserIdResult>();
    #else
        public GetStaminaByUserIdTask GetStaminaByUserIdAsync(
                Request.GetStaminaByUserIdRequest request
        )
        {
            return new GetStaminaByUserIdTask(
                Gs2WebSocketSession,
                request
            );
        }
    #endif
#else
        public Task<Result.GetStaminaByUserIdResult> GetStaminaByUserIdAsync(
            Request.GetStaminaByUserIdRequest request
        ) =>
            new GetStaminaByUserIdTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsTask();
#endif


        public class UpdateStaminaByUserIdTask : Gs2WebSocketSessionTask<Request.UpdateStaminaByUserIdRequest, Result.UpdateStaminaByUserIdResult>
        {
            public UpdateStaminaByUserIdTask(IGs2Session session, Request.UpdateStaminaByUserIdRequest request) : base(session, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(Request.UpdateStaminaByUserIdRequest request)
            {
                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);

                jsonWriter.WriteObjectStart();

                if (request.NamespaceName != null)
                {
                    jsonWriter.WritePropertyName("namespaceName");
                    jsonWriter.Write(request.NamespaceName.ToString());
                }
                if (request.StaminaName != null)
                {
                    jsonWriter.WritePropertyName("staminaName");
                    jsonWriter.Write(request.StaminaName.ToString());
                }
                if (request.UserId != null)
                {
                    jsonWriter.WritePropertyName("userId");
                    jsonWriter.Write(request.UserId.ToString());
                }
                if (request.Value != null)
                {
                    jsonWriter.WritePropertyName("value");
                    jsonWriter.Write(request.Value.ToString());
                }
                if (request.MaxValue != null)
                {
                    jsonWriter.WritePropertyName("maxValue");
                    jsonWriter.Write(request.MaxValue.ToString());
                }
                if (request.RecoverIntervalMinutes != null)
                {
                    jsonWriter.WritePropertyName("recoverIntervalMinutes");
                    jsonWriter.Write(request.RecoverIntervalMinutes.ToString());
                }
                if (request.RecoverValue != null)
                {
                    jsonWriter.WritePropertyName("recoverValue");
                    jsonWriter.Write(request.RecoverValue.ToString());
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
                if (request.DuplicationAvoider != null)
                {
                    jsonWriter.WritePropertyName("xGs2DuplicationAvoider");
                    jsonWriter.Write(request.DuplicationAvoider);
                }
                if (request.DryRun)
                {
                    jsonWriter.WritePropertyName("xGs2DryRun");
                    jsonWriter.Write("true");
                }

                AddHeader(
                    Session.Credential,
                    "stamina",
                    "stamina",
                    "updateStaminaByUserId",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
        public IEnumerator UpdateStaminaByUserId(
                Request.UpdateStaminaByUserIdRequest request,
                UnityAction<AsyncResult<Result.UpdateStaminaByUserIdResult>> callback
        ) =>
            new UpdateStaminaByUserIdTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToCoroutine(callback);

        public IFuture<Result.UpdateStaminaByUserIdResult> UpdateStaminaByUserIdFuture(
                Request.UpdateStaminaByUserIdRequest request
        ) =>
            new UpdateStaminaByUserIdTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
        public UniTask<Result.UpdateStaminaByUserIdResult> UpdateStaminaByUserIdAsync(
            Request.UpdateStaminaByUserIdRequest request
        ) =>
            new UpdateStaminaByUserIdTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsUniTask<Result.UpdateStaminaByUserIdResult>();
    #else
        public UpdateStaminaByUserIdTask UpdateStaminaByUserIdAsync(
                Request.UpdateStaminaByUserIdRequest request
        )
        {
            return new UpdateStaminaByUserIdTask(
                Gs2WebSocketSession,
                request
            );
        }
    #endif
#else
        public Task<Result.UpdateStaminaByUserIdResult> UpdateStaminaByUserIdAsync(
            Request.UpdateStaminaByUserIdRequest request
        ) =>
            new UpdateStaminaByUserIdTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsTask();
#endif


        public class ConsumeStaminaTask : Gs2WebSocketSessionTask<Request.ConsumeStaminaRequest, Result.ConsumeStaminaResult>
        {
            public ConsumeStaminaTask(IGs2Session session, Request.ConsumeStaminaRequest request) : base(session, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(Request.ConsumeStaminaRequest request)
            {
                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);

                jsonWriter.WriteObjectStart();

                if (request.NamespaceName != null)
                {
                    jsonWriter.WritePropertyName("namespaceName");
                    jsonWriter.Write(request.NamespaceName.ToString());
                }
                if (request.StaminaName != null)
                {
                    jsonWriter.WritePropertyName("staminaName");
                    jsonWriter.Write(request.StaminaName.ToString());
                }
                if (request.AccessToken != null)
                {
                    jsonWriter.WritePropertyName("accessToken");
                    jsonWriter.Write(request.AccessToken.ToString());
                }
                if (request.ConsumeValue != null)
                {
                    jsonWriter.WritePropertyName("consumeValue");
                    jsonWriter.Write(request.ConsumeValue.ToString());
                }
                if (request.ContextStack != null)
                {
                    jsonWriter.WritePropertyName("contextStack");
                    jsonWriter.Write(request.ContextStack.ToString());
                }
                if (request.AccessToken != null)
                {
                    jsonWriter.WritePropertyName("xGs2AccessToken");
                    jsonWriter.Write(request.AccessToken);
                }
                if (request.DuplicationAvoider != null)
                {
                    jsonWriter.WritePropertyName("xGs2DuplicationAvoider");
                    jsonWriter.Write(request.DuplicationAvoider);
                }
                if (request.DryRun)
                {
                    jsonWriter.WritePropertyName("xGs2DryRun");
                    jsonWriter.Write("true");
                }

                AddHeader(
                    Session.Credential,
                    "stamina",
                    "stamina",
                    "consumeStamina",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }

            public override void OnError(Gs2.Core.Exception.Gs2Exception error)
            {
                if (error.Errors.Count(v => v.code == "stamina.stamina.insufficient") > 0) {
                    base.OnError(new Exception.InsufficientException(error));
                }
                else {
                    base.OnError(error);
                }
            }
        }

#if UNITY_2017_1_OR_NEWER
        public IEnumerator ConsumeStamina(
                Request.ConsumeStaminaRequest request,
                UnityAction<AsyncResult<Result.ConsumeStaminaResult>> callback
        ) =>
            new ConsumeStaminaTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToCoroutine(callback);

        public IFuture<Result.ConsumeStaminaResult> ConsumeStaminaFuture(
                Request.ConsumeStaminaRequest request
        ) =>
            new ConsumeStaminaTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
        public UniTask<Result.ConsumeStaminaResult> ConsumeStaminaAsync(
            Request.ConsumeStaminaRequest request
        ) =>
            new ConsumeStaminaTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsUniTask<Result.ConsumeStaminaResult>();
    #else
        public ConsumeStaminaTask ConsumeStaminaAsync(
                Request.ConsumeStaminaRequest request
        )
        {
            return new ConsumeStaminaTask(
                Gs2WebSocketSession,
                request
            );
        }
    #endif
#else
        public Task<Result.ConsumeStaminaResult> ConsumeStaminaAsync(
            Request.ConsumeStaminaRequest request
        ) =>
            new ConsumeStaminaTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsTask();
#endif


        public class ConsumeStaminaByUserIdTask : Gs2WebSocketSessionTask<Request.ConsumeStaminaByUserIdRequest, Result.ConsumeStaminaByUserIdResult>
        {
            public ConsumeStaminaByUserIdTask(IGs2Session session, Request.ConsumeStaminaByUserIdRequest request) : base(session, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(Request.ConsumeStaminaByUserIdRequest request)
            {
                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);

                jsonWriter.WriteObjectStart();

                if (request.NamespaceName != null)
                {
                    jsonWriter.WritePropertyName("namespaceName");
                    jsonWriter.Write(request.NamespaceName.ToString());
                }
                if (request.StaminaName != null)
                {
                    jsonWriter.WritePropertyName("staminaName");
                    jsonWriter.Write(request.StaminaName.ToString());
                }
                if (request.UserId != null)
                {
                    jsonWriter.WritePropertyName("userId");
                    jsonWriter.Write(request.UserId.ToString());
                }
                if (request.ConsumeValue != null)
                {
                    jsonWriter.WritePropertyName("consumeValue");
                    jsonWriter.Write(request.ConsumeValue.ToString());
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
                if (request.DuplicationAvoider != null)
                {
                    jsonWriter.WritePropertyName("xGs2DuplicationAvoider");
                    jsonWriter.Write(request.DuplicationAvoider);
                }
                if (request.DryRun)
                {
                    jsonWriter.WritePropertyName("xGs2DryRun");
                    jsonWriter.Write("true");
                }

                AddHeader(
                    Session.Credential,
                    "stamina",
                    "stamina",
                    "consumeStaminaByUserId",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }

            public override void OnError(Gs2.Core.Exception.Gs2Exception error)
            {
                if (error.Errors.Count(v => v.code == "stamina.stamina.insufficient") > 0) {
                    base.OnError(new Exception.InsufficientException(error));
                }
                else {
                    base.OnError(error);
                }
            }
        }

#if UNITY_2017_1_OR_NEWER
        public IEnumerator ConsumeStaminaByUserId(
                Request.ConsumeStaminaByUserIdRequest request,
                UnityAction<AsyncResult<Result.ConsumeStaminaByUserIdResult>> callback
        ) =>
            new ConsumeStaminaByUserIdTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToCoroutine(callback);

        public IFuture<Result.ConsumeStaminaByUserIdResult> ConsumeStaminaByUserIdFuture(
                Request.ConsumeStaminaByUserIdRequest request
        ) =>
            new ConsumeStaminaByUserIdTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
        public UniTask<Result.ConsumeStaminaByUserIdResult> ConsumeStaminaByUserIdAsync(
            Request.ConsumeStaminaByUserIdRequest request
        ) =>
            new ConsumeStaminaByUserIdTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsUniTask<Result.ConsumeStaminaByUserIdResult>();
    #else
        public ConsumeStaminaByUserIdTask ConsumeStaminaByUserIdAsync(
                Request.ConsumeStaminaByUserIdRequest request
        )
        {
            return new ConsumeStaminaByUserIdTask(
                Gs2WebSocketSession,
                request
            );
        }
    #endif
#else
        public Task<Result.ConsumeStaminaByUserIdResult> ConsumeStaminaByUserIdAsync(
            Request.ConsumeStaminaByUserIdRequest request
        ) =>
            new ConsumeStaminaByUserIdTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsTask();
#endif


        public class ApplyStaminaTask : Gs2WebSocketSessionTask<Request.ApplyStaminaRequest, Result.ApplyStaminaResult>
        {
            public ApplyStaminaTask(IGs2Session session, Request.ApplyStaminaRequest request) : base(session, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(Request.ApplyStaminaRequest request)
            {
                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);

                jsonWriter.WriteObjectStart();

                if (request.NamespaceName != null)
                {
                    jsonWriter.WritePropertyName("namespaceName");
                    jsonWriter.Write(request.NamespaceName.ToString());
                }
                if (request.StaminaName != null)
                {
                    jsonWriter.WritePropertyName("staminaName");
                    jsonWriter.Write(request.StaminaName.ToString());
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
                if (request.AccessToken != null)
                {
                    jsonWriter.WritePropertyName("xGs2AccessToken");
                    jsonWriter.Write(request.AccessToken);
                }
                if (request.DuplicationAvoider != null)
                {
                    jsonWriter.WritePropertyName("xGs2DuplicationAvoider");
                    jsonWriter.Write(request.DuplicationAvoider);
                }
                if (request.DryRun)
                {
                    jsonWriter.WritePropertyName("xGs2DryRun");
                    jsonWriter.Write("true");
                }

                AddHeader(
                    Session.Credential,
                    "stamina",
                    "stamina",
                    "applyStamina",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
        public IEnumerator ApplyStamina(
                Request.ApplyStaminaRequest request,
                UnityAction<AsyncResult<Result.ApplyStaminaResult>> callback
        ) =>
            new ApplyStaminaTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToCoroutine(callback);

        public IFuture<Result.ApplyStaminaResult> ApplyStaminaFuture(
                Request.ApplyStaminaRequest request
        ) =>
            new ApplyStaminaTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
        public UniTask<Result.ApplyStaminaResult> ApplyStaminaAsync(
            Request.ApplyStaminaRequest request
        ) =>
            new ApplyStaminaTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsUniTask<Result.ApplyStaminaResult>();
    #else
        public ApplyStaminaTask ApplyStaminaAsync(
                Request.ApplyStaminaRequest request
        )
        {
            return new ApplyStaminaTask(
                Gs2WebSocketSession,
                request
            );
        }
    #endif
#else
        public Task<Result.ApplyStaminaResult> ApplyStaminaAsync(
            Request.ApplyStaminaRequest request
        ) =>
            new ApplyStaminaTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsTask();
#endif


        public class ApplyStaminaByUserIdTask : Gs2WebSocketSessionTask<Request.ApplyStaminaByUserIdRequest, Result.ApplyStaminaByUserIdResult>
        {
            public ApplyStaminaByUserIdTask(IGs2Session session, Request.ApplyStaminaByUserIdRequest request) : base(session, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(Request.ApplyStaminaByUserIdRequest request)
            {
                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);

                jsonWriter.WriteObjectStart();

                if (request.NamespaceName != null)
                {
                    jsonWriter.WritePropertyName("namespaceName");
                    jsonWriter.Write(request.NamespaceName.ToString());
                }
                if (request.StaminaName != null)
                {
                    jsonWriter.WritePropertyName("staminaName");
                    jsonWriter.Write(request.StaminaName.ToString());
                }
                if (request.UserId != null)
                {
                    jsonWriter.WritePropertyName("userId");
                    jsonWriter.Write(request.UserId.ToString());
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
                if (request.DuplicationAvoider != null)
                {
                    jsonWriter.WritePropertyName("xGs2DuplicationAvoider");
                    jsonWriter.Write(request.DuplicationAvoider);
                }
                if (request.DryRun)
                {
                    jsonWriter.WritePropertyName("xGs2DryRun");
                    jsonWriter.Write("true");
                }

                AddHeader(
                    Session.Credential,
                    "stamina",
                    "stamina",
                    "applyStaminaByUserId",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
        public IEnumerator ApplyStaminaByUserId(
                Request.ApplyStaminaByUserIdRequest request,
                UnityAction<AsyncResult<Result.ApplyStaminaByUserIdResult>> callback
        ) =>
            new ApplyStaminaByUserIdTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToCoroutine(callback);

        public IFuture<Result.ApplyStaminaByUserIdResult> ApplyStaminaByUserIdFuture(
                Request.ApplyStaminaByUserIdRequest request
        ) =>
            new ApplyStaminaByUserIdTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
        public UniTask<Result.ApplyStaminaByUserIdResult> ApplyStaminaByUserIdAsync(
            Request.ApplyStaminaByUserIdRequest request
        ) =>
            new ApplyStaminaByUserIdTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsUniTask<Result.ApplyStaminaByUserIdResult>();
    #else
        public ApplyStaminaByUserIdTask ApplyStaminaByUserIdAsync(
                Request.ApplyStaminaByUserIdRequest request
        )
        {
            return new ApplyStaminaByUserIdTask(
                Gs2WebSocketSession,
                request
            );
        }
    #endif
#else
        public Task<Result.ApplyStaminaByUserIdResult> ApplyStaminaByUserIdAsync(
            Request.ApplyStaminaByUserIdRequest request
        ) =>
            new ApplyStaminaByUserIdTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsTask();
#endif


        public class RecoverStaminaByUserIdTask : Gs2WebSocketSessionTask<Request.RecoverStaminaByUserIdRequest, Result.RecoverStaminaByUserIdResult>
        {
            public RecoverStaminaByUserIdTask(IGs2Session session, Request.RecoverStaminaByUserIdRequest request) : base(session, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(Request.RecoverStaminaByUserIdRequest request)
            {
                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);

                jsonWriter.WriteObjectStart();

                if (request.NamespaceName != null)
                {
                    jsonWriter.WritePropertyName("namespaceName");
                    jsonWriter.Write(request.NamespaceName.ToString());
                }
                if (request.StaminaName != null)
                {
                    jsonWriter.WritePropertyName("staminaName");
                    jsonWriter.Write(request.StaminaName.ToString());
                }
                if (request.UserId != null)
                {
                    jsonWriter.WritePropertyName("userId");
                    jsonWriter.Write(request.UserId.ToString());
                }
                if (request.RecoverValue != null)
                {
                    jsonWriter.WritePropertyName("recoverValue");
                    jsonWriter.Write(request.RecoverValue.ToString());
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
                if (request.DuplicationAvoider != null)
                {
                    jsonWriter.WritePropertyName("xGs2DuplicationAvoider");
                    jsonWriter.Write(request.DuplicationAvoider);
                }
                if (request.DryRun)
                {
                    jsonWriter.WritePropertyName("xGs2DryRun");
                    jsonWriter.Write("true");
                }

                AddHeader(
                    Session.Credential,
                    "stamina",
                    "stamina",
                    "recoverStaminaByUserId",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
        public IEnumerator RecoverStaminaByUserId(
                Request.RecoverStaminaByUserIdRequest request,
                UnityAction<AsyncResult<Result.RecoverStaminaByUserIdResult>> callback
        ) =>
            new RecoverStaminaByUserIdTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToCoroutine(callback);

        public IFuture<Result.RecoverStaminaByUserIdResult> RecoverStaminaByUserIdFuture(
                Request.RecoverStaminaByUserIdRequest request
        ) =>
            new RecoverStaminaByUserIdTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
        public UniTask<Result.RecoverStaminaByUserIdResult> RecoverStaminaByUserIdAsync(
            Request.RecoverStaminaByUserIdRequest request
        ) =>
            new RecoverStaminaByUserIdTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsUniTask<Result.RecoverStaminaByUserIdResult>();
    #else
        public RecoverStaminaByUserIdTask RecoverStaminaByUserIdAsync(
                Request.RecoverStaminaByUserIdRequest request
        )
        {
            return new RecoverStaminaByUserIdTask(
                Gs2WebSocketSession,
                request
            );
        }
    #endif
#else
        public Task<Result.RecoverStaminaByUserIdResult> RecoverStaminaByUserIdAsync(
            Request.RecoverStaminaByUserIdRequest request
        ) =>
            new RecoverStaminaByUserIdTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsTask();
#endif


        public class RaiseMaxValueByUserIdTask : Gs2WebSocketSessionTask<Request.RaiseMaxValueByUserIdRequest, Result.RaiseMaxValueByUserIdResult>
        {
            public RaiseMaxValueByUserIdTask(IGs2Session session, Request.RaiseMaxValueByUserIdRequest request) : base(session, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(Request.RaiseMaxValueByUserIdRequest request)
            {
                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);

                jsonWriter.WriteObjectStart();

                if (request.NamespaceName != null)
                {
                    jsonWriter.WritePropertyName("namespaceName");
                    jsonWriter.Write(request.NamespaceName.ToString());
                }
                if (request.StaminaName != null)
                {
                    jsonWriter.WritePropertyName("staminaName");
                    jsonWriter.Write(request.StaminaName.ToString());
                }
                if (request.UserId != null)
                {
                    jsonWriter.WritePropertyName("userId");
                    jsonWriter.Write(request.UserId.ToString());
                }
                if (request.RaiseValue != null)
                {
                    jsonWriter.WritePropertyName("raiseValue");
                    jsonWriter.Write(request.RaiseValue.ToString());
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
                if (request.DuplicationAvoider != null)
                {
                    jsonWriter.WritePropertyName("xGs2DuplicationAvoider");
                    jsonWriter.Write(request.DuplicationAvoider);
                }
                if (request.DryRun)
                {
                    jsonWriter.WritePropertyName("xGs2DryRun");
                    jsonWriter.Write("true");
                }

                AddHeader(
                    Session.Credential,
                    "stamina",
                    "stamina",
                    "raiseMaxValueByUserId",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
        public IEnumerator RaiseMaxValueByUserId(
                Request.RaiseMaxValueByUserIdRequest request,
                UnityAction<AsyncResult<Result.RaiseMaxValueByUserIdResult>> callback
        ) =>
            new RaiseMaxValueByUserIdTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToCoroutine(callback);

        public IFuture<Result.RaiseMaxValueByUserIdResult> RaiseMaxValueByUserIdFuture(
                Request.RaiseMaxValueByUserIdRequest request
        ) =>
            new RaiseMaxValueByUserIdTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
        public UniTask<Result.RaiseMaxValueByUserIdResult> RaiseMaxValueByUserIdAsync(
            Request.RaiseMaxValueByUserIdRequest request
        ) =>
            new RaiseMaxValueByUserIdTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsUniTask<Result.RaiseMaxValueByUserIdResult>();
    #else
        public RaiseMaxValueByUserIdTask RaiseMaxValueByUserIdAsync(
                Request.RaiseMaxValueByUserIdRequest request
        )
        {
            return new RaiseMaxValueByUserIdTask(
                Gs2WebSocketSession,
                request
            );
        }
    #endif
#else
        public Task<Result.RaiseMaxValueByUserIdResult> RaiseMaxValueByUserIdAsync(
            Request.RaiseMaxValueByUserIdRequest request
        ) =>
            new RaiseMaxValueByUserIdTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsTask();
#endif


        public class DecreaseMaxValueTask : Gs2WebSocketSessionTask<Request.DecreaseMaxValueRequest, Result.DecreaseMaxValueResult>
        {
            public DecreaseMaxValueTask(IGs2Session session, Request.DecreaseMaxValueRequest request) : base(session, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(Request.DecreaseMaxValueRequest request)
            {
                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);

                jsonWriter.WriteObjectStart();

                if (request.NamespaceName != null)
                {
                    jsonWriter.WritePropertyName("namespaceName");
                    jsonWriter.Write(request.NamespaceName.ToString());
                }
                if (request.StaminaName != null)
                {
                    jsonWriter.WritePropertyName("staminaName");
                    jsonWriter.Write(request.StaminaName.ToString());
                }
                if (request.AccessToken != null)
                {
                    jsonWriter.WritePropertyName("accessToken");
                    jsonWriter.Write(request.AccessToken.ToString());
                }
                if (request.DecreaseValue != null)
                {
                    jsonWriter.WritePropertyName("decreaseValue");
                    jsonWriter.Write(request.DecreaseValue.ToString());
                }
                if (request.ContextStack != null)
                {
                    jsonWriter.WritePropertyName("contextStack");
                    jsonWriter.Write(request.ContextStack.ToString());
                }
                if (request.AccessToken != null)
                {
                    jsonWriter.WritePropertyName("xGs2AccessToken");
                    jsonWriter.Write(request.AccessToken);
                }
                if (request.DuplicationAvoider != null)
                {
                    jsonWriter.WritePropertyName("xGs2DuplicationAvoider");
                    jsonWriter.Write(request.DuplicationAvoider);
                }
                if (request.DryRun)
                {
                    jsonWriter.WritePropertyName("xGs2DryRun");
                    jsonWriter.Write("true");
                }

                AddHeader(
                    Session.Credential,
                    "stamina",
                    "stamina",
                    "decreaseMaxValue",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
        public IEnumerator DecreaseMaxValue(
                Request.DecreaseMaxValueRequest request,
                UnityAction<AsyncResult<Result.DecreaseMaxValueResult>> callback
        ) =>
            new DecreaseMaxValueTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToCoroutine(callback);

        public IFuture<Result.DecreaseMaxValueResult> DecreaseMaxValueFuture(
                Request.DecreaseMaxValueRequest request
        ) =>
            new DecreaseMaxValueTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
        public UniTask<Result.DecreaseMaxValueResult> DecreaseMaxValueAsync(
            Request.DecreaseMaxValueRequest request
        ) =>
            new DecreaseMaxValueTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsUniTask<Result.DecreaseMaxValueResult>();
    #else
        public DecreaseMaxValueTask DecreaseMaxValueAsync(
                Request.DecreaseMaxValueRequest request
        )
        {
            return new DecreaseMaxValueTask(
                Gs2WebSocketSession,
                request
            );
        }
    #endif
#else
        public Task<Result.DecreaseMaxValueResult> DecreaseMaxValueAsync(
            Request.DecreaseMaxValueRequest request
        ) =>
            new DecreaseMaxValueTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsTask();
#endif


        public class DecreaseMaxValueByUserIdTask : Gs2WebSocketSessionTask<Request.DecreaseMaxValueByUserIdRequest, Result.DecreaseMaxValueByUserIdResult>
        {
            public DecreaseMaxValueByUserIdTask(IGs2Session session, Request.DecreaseMaxValueByUserIdRequest request) : base(session, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(Request.DecreaseMaxValueByUserIdRequest request)
            {
                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);

                jsonWriter.WriteObjectStart();

                if (request.NamespaceName != null)
                {
                    jsonWriter.WritePropertyName("namespaceName");
                    jsonWriter.Write(request.NamespaceName.ToString());
                }
                if (request.StaminaName != null)
                {
                    jsonWriter.WritePropertyName("staminaName");
                    jsonWriter.Write(request.StaminaName.ToString());
                }
                if (request.UserId != null)
                {
                    jsonWriter.WritePropertyName("userId");
                    jsonWriter.Write(request.UserId.ToString());
                }
                if (request.DecreaseValue != null)
                {
                    jsonWriter.WritePropertyName("decreaseValue");
                    jsonWriter.Write(request.DecreaseValue.ToString());
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
                if (request.DuplicationAvoider != null)
                {
                    jsonWriter.WritePropertyName("xGs2DuplicationAvoider");
                    jsonWriter.Write(request.DuplicationAvoider);
                }
                if (request.DryRun)
                {
                    jsonWriter.WritePropertyName("xGs2DryRun");
                    jsonWriter.Write("true");
                }

                AddHeader(
                    Session.Credential,
                    "stamina",
                    "stamina",
                    "decreaseMaxValueByUserId",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
        public IEnumerator DecreaseMaxValueByUserId(
                Request.DecreaseMaxValueByUserIdRequest request,
                UnityAction<AsyncResult<Result.DecreaseMaxValueByUserIdResult>> callback
        ) =>
            new DecreaseMaxValueByUserIdTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToCoroutine(callback);

        public IFuture<Result.DecreaseMaxValueByUserIdResult> DecreaseMaxValueByUserIdFuture(
                Request.DecreaseMaxValueByUserIdRequest request
        ) =>
            new DecreaseMaxValueByUserIdTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
        public UniTask<Result.DecreaseMaxValueByUserIdResult> DecreaseMaxValueByUserIdAsync(
            Request.DecreaseMaxValueByUserIdRequest request
        ) =>
            new DecreaseMaxValueByUserIdTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsUniTask<Result.DecreaseMaxValueByUserIdResult>();
    #else
        public DecreaseMaxValueByUserIdTask DecreaseMaxValueByUserIdAsync(
                Request.DecreaseMaxValueByUserIdRequest request
        )
        {
            return new DecreaseMaxValueByUserIdTask(
                Gs2WebSocketSession,
                request
            );
        }
    #endif
#else
        public Task<Result.DecreaseMaxValueByUserIdResult> DecreaseMaxValueByUserIdAsync(
            Request.DecreaseMaxValueByUserIdRequest request
        ) =>
            new DecreaseMaxValueByUserIdTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsTask();
#endif


        public class SetMaxValueByUserIdTask : Gs2WebSocketSessionTask<Request.SetMaxValueByUserIdRequest, Result.SetMaxValueByUserIdResult>
        {
            public SetMaxValueByUserIdTask(IGs2Session session, Request.SetMaxValueByUserIdRequest request) : base(session, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(Request.SetMaxValueByUserIdRequest request)
            {
                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);

                jsonWriter.WriteObjectStart();

                if (request.NamespaceName != null)
                {
                    jsonWriter.WritePropertyName("namespaceName");
                    jsonWriter.Write(request.NamespaceName.ToString());
                }
                if (request.StaminaName != null)
                {
                    jsonWriter.WritePropertyName("staminaName");
                    jsonWriter.Write(request.StaminaName.ToString());
                }
                if (request.UserId != null)
                {
                    jsonWriter.WritePropertyName("userId");
                    jsonWriter.Write(request.UserId.ToString());
                }
                if (request.MaxValue != null)
                {
                    jsonWriter.WritePropertyName("maxValue");
                    jsonWriter.Write(request.MaxValue.ToString());
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
                if (request.DuplicationAvoider != null)
                {
                    jsonWriter.WritePropertyName("xGs2DuplicationAvoider");
                    jsonWriter.Write(request.DuplicationAvoider);
                }
                if (request.DryRun)
                {
                    jsonWriter.WritePropertyName("xGs2DryRun");
                    jsonWriter.Write("true");
                }

                AddHeader(
                    Session.Credential,
                    "stamina",
                    "stamina",
                    "setMaxValueByUserId",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
        public IEnumerator SetMaxValueByUserId(
                Request.SetMaxValueByUserIdRequest request,
                UnityAction<AsyncResult<Result.SetMaxValueByUserIdResult>> callback
        ) =>
            new SetMaxValueByUserIdTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToCoroutine(callback);

        public IFuture<Result.SetMaxValueByUserIdResult> SetMaxValueByUserIdFuture(
                Request.SetMaxValueByUserIdRequest request
        ) =>
            new SetMaxValueByUserIdTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
        public UniTask<Result.SetMaxValueByUserIdResult> SetMaxValueByUserIdAsync(
            Request.SetMaxValueByUserIdRequest request
        ) =>
            new SetMaxValueByUserIdTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsUniTask<Result.SetMaxValueByUserIdResult>();
    #else
        public SetMaxValueByUserIdTask SetMaxValueByUserIdAsync(
                Request.SetMaxValueByUserIdRequest request
        )
        {
            return new SetMaxValueByUserIdTask(
                Gs2WebSocketSession,
                request
            );
        }
    #endif
#else
        public Task<Result.SetMaxValueByUserIdResult> SetMaxValueByUserIdAsync(
            Request.SetMaxValueByUserIdRequest request
        ) =>
            new SetMaxValueByUserIdTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsTask();
#endif


        public class SetRecoverIntervalByUserIdTask : Gs2WebSocketSessionTask<Request.SetRecoverIntervalByUserIdRequest, Result.SetRecoverIntervalByUserIdResult>
        {
            public SetRecoverIntervalByUserIdTask(IGs2Session session, Request.SetRecoverIntervalByUserIdRequest request) : base(session, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(Request.SetRecoverIntervalByUserIdRequest request)
            {
                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);

                jsonWriter.WriteObjectStart();

                if (request.NamespaceName != null)
                {
                    jsonWriter.WritePropertyName("namespaceName");
                    jsonWriter.Write(request.NamespaceName.ToString());
                }
                if (request.StaminaName != null)
                {
                    jsonWriter.WritePropertyName("staminaName");
                    jsonWriter.Write(request.StaminaName.ToString());
                }
                if (request.UserId != null)
                {
                    jsonWriter.WritePropertyName("userId");
                    jsonWriter.Write(request.UserId.ToString());
                }
                if (request.RecoverIntervalMinutes != null)
                {
                    jsonWriter.WritePropertyName("recoverIntervalMinutes");
                    jsonWriter.Write(request.RecoverIntervalMinutes.ToString());
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
                if (request.DuplicationAvoider != null)
                {
                    jsonWriter.WritePropertyName("xGs2DuplicationAvoider");
                    jsonWriter.Write(request.DuplicationAvoider);
                }
                if (request.DryRun)
                {
                    jsonWriter.WritePropertyName("xGs2DryRun");
                    jsonWriter.Write("true");
                }

                AddHeader(
                    Session.Credential,
                    "stamina",
                    "stamina",
                    "setRecoverIntervalByUserId",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
        public IEnumerator SetRecoverIntervalByUserId(
                Request.SetRecoverIntervalByUserIdRequest request,
                UnityAction<AsyncResult<Result.SetRecoverIntervalByUserIdResult>> callback
        ) =>
            new SetRecoverIntervalByUserIdTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToCoroutine(callback);

        public IFuture<Result.SetRecoverIntervalByUserIdResult> SetRecoverIntervalByUserIdFuture(
                Request.SetRecoverIntervalByUserIdRequest request
        ) =>
            new SetRecoverIntervalByUserIdTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
        public UniTask<Result.SetRecoverIntervalByUserIdResult> SetRecoverIntervalByUserIdAsync(
            Request.SetRecoverIntervalByUserIdRequest request
        ) =>
            new SetRecoverIntervalByUserIdTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsUniTask<Result.SetRecoverIntervalByUserIdResult>();
    #else
        public SetRecoverIntervalByUserIdTask SetRecoverIntervalByUserIdAsync(
                Request.SetRecoverIntervalByUserIdRequest request
        )
        {
            return new SetRecoverIntervalByUserIdTask(
                Gs2WebSocketSession,
                request
            );
        }
    #endif
#else
        public Task<Result.SetRecoverIntervalByUserIdResult> SetRecoverIntervalByUserIdAsync(
            Request.SetRecoverIntervalByUserIdRequest request
        ) =>
            new SetRecoverIntervalByUserIdTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsTask();
#endif


        public class SetRecoverValueByUserIdTask : Gs2WebSocketSessionTask<Request.SetRecoverValueByUserIdRequest, Result.SetRecoverValueByUserIdResult>
        {
            public SetRecoverValueByUserIdTask(IGs2Session session, Request.SetRecoverValueByUserIdRequest request) : base(session, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(Request.SetRecoverValueByUserIdRequest request)
            {
                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);

                jsonWriter.WriteObjectStart();

                if (request.NamespaceName != null)
                {
                    jsonWriter.WritePropertyName("namespaceName");
                    jsonWriter.Write(request.NamespaceName.ToString());
                }
                if (request.StaminaName != null)
                {
                    jsonWriter.WritePropertyName("staminaName");
                    jsonWriter.Write(request.StaminaName.ToString());
                }
                if (request.UserId != null)
                {
                    jsonWriter.WritePropertyName("userId");
                    jsonWriter.Write(request.UserId.ToString());
                }
                if (request.RecoverValue != null)
                {
                    jsonWriter.WritePropertyName("recoverValue");
                    jsonWriter.Write(request.RecoverValue.ToString());
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
                if (request.DuplicationAvoider != null)
                {
                    jsonWriter.WritePropertyName("xGs2DuplicationAvoider");
                    jsonWriter.Write(request.DuplicationAvoider);
                }
                if (request.DryRun)
                {
                    jsonWriter.WritePropertyName("xGs2DryRun");
                    jsonWriter.Write("true");
                }

                AddHeader(
                    Session.Credential,
                    "stamina",
                    "stamina",
                    "setRecoverValueByUserId",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
        public IEnumerator SetRecoverValueByUserId(
                Request.SetRecoverValueByUserIdRequest request,
                UnityAction<AsyncResult<Result.SetRecoverValueByUserIdResult>> callback
        ) =>
            new SetRecoverValueByUserIdTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToCoroutine(callback);

        public IFuture<Result.SetRecoverValueByUserIdResult> SetRecoverValueByUserIdFuture(
                Request.SetRecoverValueByUserIdRequest request
        ) =>
            new SetRecoverValueByUserIdTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
        public UniTask<Result.SetRecoverValueByUserIdResult> SetRecoverValueByUserIdAsync(
            Request.SetRecoverValueByUserIdRequest request
        ) =>
            new SetRecoverValueByUserIdTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsUniTask<Result.SetRecoverValueByUserIdResult>();
    #else
        public SetRecoverValueByUserIdTask SetRecoverValueByUserIdAsync(
                Request.SetRecoverValueByUserIdRequest request
        )
        {
            return new SetRecoverValueByUserIdTask(
                Gs2WebSocketSession,
                request
            );
        }
    #endif
#else
        public Task<Result.SetRecoverValueByUserIdResult> SetRecoverValueByUserIdAsync(
            Request.SetRecoverValueByUserIdRequest request
        ) =>
            new SetRecoverValueByUserIdTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsTask();
#endif


        public class SetMaxValueByStatusTask : Gs2WebSocketSessionTask<Request.SetMaxValueByStatusRequest, Result.SetMaxValueByStatusResult>
        {
            public SetMaxValueByStatusTask(IGs2Session session, Request.SetMaxValueByStatusRequest request) : base(session, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(Request.SetMaxValueByStatusRequest request)
            {
                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);

                jsonWriter.WriteObjectStart();

                if (request.NamespaceName != null)
                {
                    jsonWriter.WritePropertyName("namespaceName");
                    jsonWriter.Write(request.NamespaceName.ToString());
                }
                if (request.StaminaName != null)
                {
                    jsonWriter.WritePropertyName("staminaName");
                    jsonWriter.Write(request.StaminaName.ToString());
                }
                if (request.AccessToken != null)
                {
                    jsonWriter.WritePropertyName("accessToken");
                    jsonWriter.Write(request.AccessToken.ToString());
                }
                if (request.KeyId != null)
                {
                    jsonWriter.WritePropertyName("keyId");
                    jsonWriter.Write(request.KeyId.ToString());
                }
                if (request.SignedStatusBody != null)
                {
                    jsonWriter.WritePropertyName("signedStatusBody");
                    jsonWriter.Write(request.SignedStatusBody.ToString());
                }
                if (request.SignedStatusSignature != null)
                {
                    jsonWriter.WritePropertyName("signedStatusSignature");
                    jsonWriter.Write(request.SignedStatusSignature.ToString());
                }
                if (request.ContextStack != null)
                {
                    jsonWriter.WritePropertyName("contextStack");
                    jsonWriter.Write(request.ContextStack.ToString());
                }
                if (request.AccessToken != null)
                {
                    jsonWriter.WritePropertyName("xGs2AccessToken");
                    jsonWriter.Write(request.AccessToken);
                }
                if (request.DuplicationAvoider != null)
                {
                    jsonWriter.WritePropertyName("xGs2DuplicationAvoider");
                    jsonWriter.Write(request.DuplicationAvoider);
                }
                if (request.DryRun)
                {
                    jsonWriter.WritePropertyName("xGs2DryRun");
                    jsonWriter.Write("true");
                }

                AddHeader(
                    Session.Credential,
                    "stamina",
                    "stamina",
                    "setMaxValueByStatus",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
        public IEnumerator SetMaxValueByStatus(
                Request.SetMaxValueByStatusRequest request,
                UnityAction<AsyncResult<Result.SetMaxValueByStatusResult>> callback
        ) =>
            new SetMaxValueByStatusTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToCoroutine(callback);

        public IFuture<Result.SetMaxValueByStatusResult> SetMaxValueByStatusFuture(
                Request.SetMaxValueByStatusRequest request
        ) =>
            new SetMaxValueByStatusTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
        public UniTask<Result.SetMaxValueByStatusResult> SetMaxValueByStatusAsync(
            Request.SetMaxValueByStatusRequest request
        ) =>
            new SetMaxValueByStatusTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsUniTask<Result.SetMaxValueByStatusResult>();
    #else
        public SetMaxValueByStatusTask SetMaxValueByStatusAsync(
                Request.SetMaxValueByStatusRequest request
        )
        {
            return new SetMaxValueByStatusTask(
                Gs2WebSocketSession,
                request
            );
        }
    #endif
#else
        public Task<Result.SetMaxValueByStatusResult> SetMaxValueByStatusAsync(
            Request.SetMaxValueByStatusRequest request
        ) =>
            new SetMaxValueByStatusTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsTask();
#endif


        public class SetRecoverIntervalByStatusTask : Gs2WebSocketSessionTask<Request.SetRecoverIntervalByStatusRequest, Result.SetRecoverIntervalByStatusResult>
        {
            public SetRecoverIntervalByStatusTask(IGs2Session session, Request.SetRecoverIntervalByStatusRequest request) : base(session, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(Request.SetRecoverIntervalByStatusRequest request)
            {
                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);

                jsonWriter.WriteObjectStart();

                if (request.NamespaceName != null)
                {
                    jsonWriter.WritePropertyName("namespaceName");
                    jsonWriter.Write(request.NamespaceName.ToString());
                }
                if (request.StaminaName != null)
                {
                    jsonWriter.WritePropertyName("staminaName");
                    jsonWriter.Write(request.StaminaName.ToString());
                }
                if (request.AccessToken != null)
                {
                    jsonWriter.WritePropertyName("accessToken");
                    jsonWriter.Write(request.AccessToken.ToString());
                }
                if (request.KeyId != null)
                {
                    jsonWriter.WritePropertyName("keyId");
                    jsonWriter.Write(request.KeyId.ToString());
                }
                if (request.SignedStatusBody != null)
                {
                    jsonWriter.WritePropertyName("signedStatusBody");
                    jsonWriter.Write(request.SignedStatusBody.ToString());
                }
                if (request.SignedStatusSignature != null)
                {
                    jsonWriter.WritePropertyName("signedStatusSignature");
                    jsonWriter.Write(request.SignedStatusSignature.ToString());
                }
                if (request.ContextStack != null)
                {
                    jsonWriter.WritePropertyName("contextStack");
                    jsonWriter.Write(request.ContextStack.ToString());
                }
                if (request.AccessToken != null)
                {
                    jsonWriter.WritePropertyName("xGs2AccessToken");
                    jsonWriter.Write(request.AccessToken);
                }
                if (request.DuplicationAvoider != null)
                {
                    jsonWriter.WritePropertyName("xGs2DuplicationAvoider");
                    jsonWriter.Write(request.DuplicationAvoider);
                }
                if (request.DryRun)
                {
                    jsonWriter.WritePropertyName("xGs2DryRun");
                    jsonWriter.Write("true");
                }

                AddHeader(
                    Session.Credential,
                    "stamina",
                    "stamina",
                    "setRecoverIntervalByStatus",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
        public IEnumerator SetRecoverIntervalByStatus(
                Request.SetRecoverIntervalByStatusRequest request,
                UnityAction<AsyncResult<Result.SetRecoverIntervalByStatusResult>> callback
        ) =>
            new SetRecoverIntervalByStatusTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToCoroutine(callback);

        public IFuture<Result.SetRecoverIntervalByStatusResult> SetRecoverIntervalByStatusFuture(
                Request.SetRecoverIntervalByStatusRequest request
        ) =>
            new SetRecoverIntervalByStatusTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
        public UniTask<Result.SetRecoverIntervalByStatusResult> SetRecoverIntervalByStatusAsync(
            Request.SetRecoverIntervalByStatusRequest request
        ) =>
            new SetRecoverIntervalByStatusTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsUniTask<Result.SetRecoverIntervalByStatusResult>();
    #else
        public SetRecoverIntervalByStatusTask SetRecoverIntervalByStatusAsync(
                Request.SetRecoverIntervalByStatusRequest request
        )
        {
            return new SetRecoverIntervalByStatusTask(
                Gs2WebSocketSession,
                request
            );
        }
    #endif
#else
        public Task<Result.SetRecoverIntervalByStatusResult> SetRecoverIntervalByStatusAsync(
            Request.SetRecoverIntervalByStatusRequest request
        ) =>
            new SetRecoverIntervalByStatusTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsTask();
#endif


        public class SetRecoverValueByStatusTask : Gs2WebSocketSessionTask<Request.SetRecoverValueByStatusRequest, Result.SetRecoverValueByStatusResult>
        {
            public SetRecoverValueByStatusTask(IGs2Session session, Request.SetRecoverValueByStatusRequest request) : base(session, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(Request.SetRecoverValueByStatusRequest request)
            {
                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);

                jsonWriter.WriteObjectStart();

                if (request.NamespaceName != null)
                {
                    jsonWriter.WritePropertyName("namespaceName");
                    jsonWriter.Write(request.NamespaceName.ToString());
                }
                if (request.StaminaName != null)
                {
                    jsonWriter.WritePropertyName("staminaName");
                    jsonWriter.Write(request.StaminaName.ToString());
                }
                if (request.AccessToken != null)
                {
                    jsonWriter.WritePropertyName("accessToken");
                    jsonWriter.Write(request.AccessToken.ToString());
                }
                if (request.KeyId != null)
                {
                    jsonWriter.WritePropertyName("keyId");
                    jsonWriter.Write(request.KeyId.ToString());
                }
                if (request.SignedStatusBody != null)
                {
                    jsonWriter.WritePropertyName("signedStatusBody");
                    jsonWriter.Write(request.SignedStatusBody.ToString());
                }
                if (request.SignedStatusSignature != null)
                {
                    jsonWriter.WritePropertyName("signedStatusSignature");
                    jsonWriter.Write(request.SignedStatusSignature.ToString());
                }
                if (request.ContextStack != null)
                {
                    jsonWriter.WritePropertyName("contextStack");
                    jsonWriter.Write(request.ContextStack.ToString());
                }
                if (request.AccessToken != null)
                {
                    jsonWriter.WritePropertyName("xGs2AccessToken");
                    jsonWriter.Write(request.AccessToken);
                }
                if (request.DuplicationAvoider != null)
                {
                    jsonWriter.WritePropertyName("xGs2DuplicationAvoider");
                    jsonWriter.Write(request.DuplicationAvoider);
                }
                if (request.DryRun)
                {
                    jsonWriter.WritePropertyName("xGs2DryRun");
                    jsonWriter.Write("true");
                }

                AddHeader(
                    Session.Credential,
                    "stamina",
                    "stamina",
                    "setRecoverValueByStatus",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
        public IEnumerator SetRecoverValueByStatus(
                Request.SetRecoverValueByStatusRequest request,
                UnityAction<AsyncResult<Result.SetRecoverValueByStatusResult>> callback
        ) =>
            new SetRecoverValueByStatusTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToCoroutine(callback);

        public IFuture<Result.SetRecoverValueByStatusResult> SetRecoverValueByStatusFuture(
                Request.SetRecoverValueByStatusRequest request
        ) =>
            new SetRecoverValueByStatusTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
        public UniTask<Result.SetRecoverValueByStatusResult> SetRecoverValueByStatusAsync(
            Request.SetRecoverValueByStatusRequest request
        ) =>
            new SetRecoverValueByStatusTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsUniTask<Result.SetRecoverValueByStatusResult>();
    #else
        public SetRecoverValueByStatusTask SetRecoverValueByStatusAsync(
                Request.SetRecoverValueByStatusRequest request
        )
        {
            return new SetRecoverValueByStatusTask(
                Gs2WebSocketSession,
                request
            );
        }
    #endif
#else
        public Task<Result.SetRecoverValueByStatusResult> SetRecoverValueByStatusAsync(
            Request.SetRecoverValueByStatusRequest request
        ) =>
            new SetRecoverValueByStatusTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsTask();
#endif


        public class DeleteStaminaByUserIdTask : Gs2WebSocketSessionTask<Request.DeleteStaminaByUserIdRequest, Result.DeleteStaminaByUserIdResult>
        {
            public DeleteStaminaByUserIdTask(IGs2Session session, Request.DeleteStaminaByUserIdRequest request) : base(session, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(Request.DeleteStaminaByUserIdRequest request)
            {
                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);

                jsonWriter.WriteObjectStart();

                if (request.NamespaceName != null)
                {
                    jsonWriter.WritePropertyName("namespaceName");
                    jsonWriter.Write(request.NamespaceName.ToString());
                }
                if (request.StaminaName != null)
                {
                    jsonWriter.WritePropertyName("staminaName");
                    jsonWriter.Write(request.StaminaName.ToString());
                }
                if (request.UserId != null)
                {
                    jsonWriter.WritePropertyName("userId");
                    jsonWriter.Write(request.UserId.ToString());
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
                if (request.DuplicationAvoider != null)
                {
                    jsonWriter.WritePropertyName("xGs2DuplicationAvoider");
                    jsonWriter.Write(request.DuplicationAvoider);
                }
                if (request.DryRun)
                {
                    jsonWriter.WritePropertyName("xGs2DryRun");
                    jsonWriter.Write("true");
                }

                AddHeader(
                    Session.Credential,
                    "stamina",
                    "stamina",
                    "deleteStaminaByUserId",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
        public IEnumerator DeleteStaminaByUserId(
                Request.DeleteStaminaByUserIdRequest request,
                UnityAction<AsyncResult<Result.DeleteStaminaByUserIdResult>> callback
        ) =>
            new DeleteStaminaByUserIdTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToCoroutine(callback);

        public IFuture<Result.DeleteStaminaByUserIdResult> DeleteStaminaByUserIdFuture(
                Request.DeleteStaminaByUserIdRequest request
        ) =>
            new DeleteStaminaByUserIdTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
        public UniTask<Result.DeleteStaminaByUserIdResult> DeleteStaminaByUserIdAsync(
            Request.DeleteStaminaByUserIdRequest request
        ) =>
            new DeleteStaminaByUserIdTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsUniTask<Result.DeleteStaminaByUserIdResult>();
    #else
        public DeleteStaminaByUserIdTask DeleteStaminaByUserIdAsync(
                Request.DeleteStaminaByUserIdRequest request
        )
        {
            return new DeleteStaminaByUserIdTask(
                Gs2WebSocketSession,
                request
            );
        }
    #endif
#else
        public Task<Result.DeleteStaminaByUserIdResult> DeleteStaminaByUserIdAsync(
            Request.DeleteStaminaByUserIdRequest request
        ) =>
            new DeleteStaminaByUserIdTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsTask();
#endif


        public class VerifyStaminaValueTask : Gs2WebSocketSessionTask<Request.VerifyStaminaValueRequest, Result.VerifyStaminaValueResult>
        {
            public VerifyStaminaValueTask(IGs2Session session, Request.VerifyStaminaValueRequest request) : base(session, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(Request.VerifyStaminaValueRequest request)
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
                if (request.StaminaName != null)
                {
                    jsonWriter.WritePropertyName("staminaName");
                    jsonWriter.Write(request.StaminaName.ToString());
                }
                if (request.VerifyType != null)
                {
                    jsonWriter.WritePropertyName("verifyType");
                    jsonWriter.Write(request.VerifyType.ToString());
                }
                if (request.Value != null)
                {
                    jsonWriter.WritePropertyName("value");
                    jsonWriter.Write(request.Value.ToString());
                }
                if (request.MultiplyValueSpecifyingQuantity != null)
                {
                    jsonWriter.WritePropertyName("multiplyValueSpecifyingQuantity");
                    jsonWriter.Write(request.MultiplyValueSpecifyingQuantity.ToString());
                }
                if (request.ContextStack != null)
                {
                    jsonWriter.WritePropertyName("contextStack");
                    jsonWriter.Write(request.ContextStack.ToString());
                }
                if (request.AccessToken != null)
                {
                    jsonWriter.WritePropertyName("xGs2AccessToken");
                    jsonWriter.Write(request.AccessToken);
                }
                if (request.DuplicationAvoider != null)
                {
                    jsonWriter.WritePropertyName("xGs2DuplicationAvoider");
                    jsonWriter.Write(request.DuplicationAvoider);
                }
                if (request.DryRun)
                {
                    jsonWriter.WritePropertyName("xGs2DryRun");
                    jsonWriter.Write("true");
                }

                AddHeader(
                    Session.Credential,
                    "stamina",
                    "stamina",
                    "verifyStaminaValue",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
        public IEnumerator VerifyStaminaValue(
                Request.VerifyStaminaValueRequest request,
                UnityAction<AsyncResult<Result.VerifyStaminaValueResult>> callback
        ) =>
            new VerifyStaminaValueTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToCoroutine(callback);

        public IFuture<Result.VerifyStaminaValueResult> VerifyStaminaValueFuture(
                Request.VerifyStaminaValueRequest request
        ) =>
            new VerifyStaminaValueTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
        public UniTask<Result.VerifyStaminaValueResult> VerifyStaminaValueAsync(
            Request.VerifyStaminaValueRequest request
        ) =>
            new VerifyStaminaValueTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsUniTask<Result.VerifyStaminaValueResult>();
    #else
        public VerifyStaminaValueTask VerifyStaminaValueAsync(
                Request.VerifyStaminaValueRequest request
        )
        {
            return new VerifyStaminaValueTask(
                Gs2WebSocketSession,
                request
            );
        }
    #endif
#else
        public Task<Result.VerifyStaminaValueResult> VerifyStaminaValueAsync(
            Request.VerifyStaminaValueRequest request
        ) =>
            new VerifyStaminaValueTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsTask();
#endif


        public class VerifyStaminaValueByUserIdTask : Gs2WebSocketSessionTask<Request.VerifyStaminaValueByUserIdRequest, Result.VerifyStaminaValueByUserIdResult>
        {
            public VerifyStaminaValueByUserIdTask(IGs2Session session, Request.VerifyStaminaValueByUserIdRequest request) : base(session, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(Request.VerifyStaminaValueByUserIdRequest request)
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
                if (request.StaminaName != null)
                {
                    jsonWriter.WritePropertyName("staminaName");
                    jsonWriter.Write(request.StaminaName.ToString());
                }
                if (request.VerifyType != null)
                {
                    jsonWriter.WritePropertyName("verifyType");
                    jsonWriter.Write(request.VerifyType.ToString());
                }
                if (request.Value != null)
                {
                    jsonWriter.WritePropertyName("value");
                    jsonWriter.Write(request.Value.ToString());
                }
                if (request.MultiplyValueSpecifyingQuantity != null)
                {
                    jsonWriter.WritePropertyName("multiplyValueSpecifyingQuantity");
                    jsonWriter.Write(request.MultiplyValueSpecifyingQuantity.ToString());
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
                if (request.DuplicationAvoider != null)
                {
                    jsonWriter.WritePropertyName("xGs2DuplicationAvoider");
                    jsonWriter.Write(request.DuplicationAvoider);
                }
                if (request.DryRun)
                {
                    jsonWriter.WritePropertyName("xGs2DryRun");
                    jsonWriter.Write("true");
                }

                AddHeader(
                    Session.Credential,
                    "stamina",
                    "stamina",
                    "verifyStaminaValueByUserId",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
        public IEnumerator VerifyStaminaValueByUserId(
                Request.VerifyStaminaValueByUserIdRequest request,
                UnityAction<AsyncResult<Result.VerifyStaminaValueByUserIdResult>> callback
        ) =>
            new VerifyStaminaValueByUserIdTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToCoroutine(callback);

        public IFuture<Result.VerifyStaminaValueByUserIdResult> VerifyStaminaValueByUserIdFuture(
                Request.VerifyStaminaValueByUserIdRequest request
        ) =>
            new VerifyStaminaValueByUserIdTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
        public UniTask<Result.VerifyStaminaValueByUserIdResult> VerifyStaminaValueByUserIdAsync(
            Request.VerifyStaminaValueByUserIdRequest request
        ) =>
            new VerifyStaminaValueByUserIdTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsUniTask<Result.VerifyStaminaValueByUserIdResult>();
    #else
        public VerifyStaminaValueByUserIdTask VerifyStaminaValueByUserIdAsync(
                Request.VerifyStaminaValueByUserIdRequest request
        )
        {
            return new VerifyStaminaValueByUserIdTask(
                Gs2WebSocketSession,
                request
            );
        }
    #endif
#else
        public Task<Result.VerifyStaminaValueByUserIdResult> VerifyStaminaValueByUserIdAsync(
            Request.VerifyStaminaValueByUserIdRequest request
        ) =>
            new VerifyStaminaValueByUserIdTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsTask();
#endif


        public class VerifyStaminaMaxValueTask : Gs2WebSocketSessionTask<Request.VerifyStaminaMaxValueRequest, Result.VerifyStaminaMaxValueResult>
        {
            public VerifyStaminaMaxValueTask(IGs2Session session, Request.VerifyStaminaMaxValueRequest request) : base(session, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(Request.VerifyStaminaMaxValueRequest request)
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
                if (request.StaminaName != null)
                {
                    jsonWriter.WritePropertyName("staminaName");
                    jsonWriter.Write(request.StaminaName.ToString());
                }
                if (request.VerifyType != null)
                {
                    jsonWriter.WritePropertyName("verifyType");
                    jsonWriter.Write(request.VerifyType.ToString());
                }
                if (request.Value != null)
                {
                    jsonWriter.WritePropertyName("value");
                    jsonWriter.Write(request.Value.ToString());
                }
                if (request.MultiplyValueSpecifyingQuantity != null)
                {
                    jsonWriter.WritePropertyName("multiplyValueSpecifyingQuantity");
                    jsonWriter.Write(request.MultiplyValueSpecifyingQuantity.ToString());
                }
                if (request.ContextStack != null)
                {
                    jsonWriter.WritePropertyName("contextStack");
                    jsonWriter.Write(request.ContextStack.ToString());
                }
                if (request.AccessToken != null)
                {
                    jsonWriter.WritePropertyName("xGs2AccessToken");
                    jsonWriter.Write(request.AccessToken);
                }
                if (request.DuplicationAvoider != null)
                {
                    jsonWriter.WritePropertyName("xGs2DuplicationAvoider");
                    jsonWriter.Write(request.DuplicationAvoider);
                }
                if (request.DryRun)
                {
                    jsonWriter.WritePropertyName("xGs2DryRun");
                    jsonWriter.Write("true");
                }

                AddHeader(
                    Session.Credential,
                    "stamina",
                    "stamina",
                    "verifyStaminaMaxValue",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
        public IEnumerator VerifyStaminaMaxValue(
                Request.VerifyStaminaMaxValueRequest request,
                UnityAction<AsyncResult<Result.VerifyStaminaMaxValueResult>> callback
        ) =>
            new VerifyStaminaMaxValueTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToCoroutine(callback);

        public IFuture<Result.VerifyStaminaMaxValueResult> VerifyStaminaMaxValueFuture(
                Request.VerifyStaminaMaxValueRequest request
        ) =>
            new VerifyStaminaMaxValueTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
        public UniTask<Result.VerifyStaminaMaxValueResult> VerifyStaminaMaxValueAsync(
            Request.VerifyStaminaMaxValueRequest request
        ) =>
            new VerifyStaminaMaxValueTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsUniTask<Result.VerifyStaminaMaxValueResult>();
    #else
        public VerifyStaminaMaxValueTask VerifyStaminaMaxValueAsync(
                Request.VerifyStaminaMaxValueRequest request
        )
        {
            return new VerifyStaminaMaxValueTask(
                Gs2WebSocketSession,
                request
            );
        }
    #endif
#else
        public Task<Result.VerifyStaminaMaxValueResult> VerifyStaminaMaxValueAsync(
            Request.VerifyStaminaMaxValueRequest request
        ) =>
            new VerifyStaminaMaxValueTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsTask();
#endif


        public class VerifyStaminaMaxValueByUserIdTask : Gs2WebSocketSessionTask<Request.VerifyStaminaMaxValueByUserIdRequest, Result.VerifyStaminaMaxValueByUserIdResult>
        {
            public VerifyStaminaMaxValueByUserIdTask(IGs2Session session, Request.VerifyStaminaMaxValueByUserIdRequest request) : base(session, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(Request.VerifyStaminaMaxValueByUserIdRequest request)
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
                if (request.StaminaName != null)
                {
                    jsonWriter.WritePropertyName("staminaName");
                    jsonWriter.Write(request.StaminaName.ToString());
                }
                if (request.VerifyType != null)
                {
                    jsonWriter.WritePropertyName("verifyType");
                    jsonWriter.Write(request.VerifyType.ToString());
                }
                if (request.Value != null)
                {
                    jsonWriter.WritePropertyName("value");
                    jsonWriter.Write(request.Value.ToString());
                }
                if (request.MultiplyValueSpecifyingQuantity != null)
                {
                    jsonWriter.WritePropertyName("multiplyValueSpecifyingQuantity");
                    jsonWriter.Write(request.MultiplyValueSpecifyingQuantity.ToString());
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
                if (request.DuplicationAvoider != null)
                {
                    jsonWriter.WritePropertyName("xGs2DuplicationAvoider");
                    jsonWriter.Write(request.DuplicationAvoider);
                }
                if (request.DryRun)
                {
                    jsonWriter.WritePropertyName("xGs2DryRun");
                    jsonWriter.Write("true");
                }

                AddHeader(
                    Session.Credential,
                    "stamina",
                    "stamina",
                    "verifyStaminaMaxValueByUserId",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
        public IEnumerator VerifyStaminaMaxValueByUserId(
                Request.VerifyStaminaMaxValueByUserIdRequest request,
                UnityAction<AsyncResult<Result.VerifyStaminaMaxValueByUserIdResult>> callback
        ) =>
            new VerifyStaminaMaxValueByUserIdTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToCoroutine(callback);

        public IFuture<Result.VerifyStaminaMaxValueByUserIdResult> VerifyStaminaMaxValueByUserIdFuture(
                Request.VerifyStaminaMaxValueByUserIdRequest request
        ) =>
            new VerifyStaminaMaxValueByUserIdTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
        public UniTask<Result.VerifyStaminaMaxValueByUserIdResult> VerifyStaminaMaxValueByUserIdAsync(
            Request.VerifyStaminaMaxValueByUserIdRequest request
        ) =>
            new VerifyStaminaMaxValueByUserIdTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsUniTask<Result.VerifyStaminaMaxValueByUserIdResult>();
    #else
        public VerifyStaminaMaxValueByUserIdTask VerifyStaminaMaxValueByUserIdAsync(
                Request.VerifyStaminaMaxValueByUserIdRequest request
        )
        {
            return new VerifyStaminaMaxValueByUserIdTask(
                Gs2WebSocketSession,
                request
            );
        }
    #endif
#else
        public Task<Result.VerifyStaminaMaxValueByUserIdResult> VerifyStaminaMaxValueByUserIdAsync(
            Request.VerifyStaminaMaxValueByUserIdRequest request
        ) =>
            new VerifyStaminaMaxValueByUserIdTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsTask();
#endif


        public class VerifyStaminaRecoverIntervalMinutesTask : Gs2WebSocketSessionTask<Request.VerifyStaminaRecoverIntervalMinutesRequest, Result.VerifyStaminaRecoverIntervalMinutesResult>
        {
            public VerifyStaminaRecoverIntervalMinutesTask(IGs2Session session, Request.VerifyStaminaRecoverIntervalMinutesRequest request) : base(session, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(Request.VerifyStaminaRecoverIntervalMinutesRequest request)
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
                if (request.StaminaName != null)
                {
                    jsonWriter.WritePropertyName("staminaName");
                    jsonWriter.Write(request.StaminaName.ToString());
                }
                if (request.VerifyType != null)
                {
                    jsonWriter.WritePropertyName("verifyType");
                    jsonWriter.Write(request.VerifyType.ToString());
                }
                if (request.Value != null)
                {
                    jsonWriter.WritePropertyName("value");
                    jsonWriter.Write(request.Value.ToString());
                }
                if (request.MultiplyValueSpecifyingQuantity != null)
                {
                    jsonWriter.WritePropertyName("multiplyValueSpecifyingQuantity");
                    jsonWriter.Write(request.MultiplyValueSpecifyingQuantity.ToString());
                }
                if (request.ContextStack != null)
                {
                    jsonWriter.WritePropertyName("contextStack");
                    jsonWriter.Write(request.ContextStack.ToString());
                }
                if (request.AccessToken != null)
                {
                    jsonWriter.WritePropertyName("xGs2AccessToken");
                    jsonWriter.Write(request.AccessToken);
                }
                if (request.DuplicationAvoider != null)
                {
                    jsonWriter.WritePropertyName("xGs2DuplicationAvoider");
                    jsonWriter.Write(request.DuplicationAvoider);
                }
                if (request.DryRun)
                {
                    jsonWriter.WritePropertyName("xGs2DryRun");
                    jsonWriter.Write("true");
                }

                AddHeader(
                    Session.Credential,
                    "stamina",
                    "stamina",
                    "verifyStaminaRecoverIntervalMinutes",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
        public IEnumerator VerifyStaminaRecoverIntervalMinutes(
                Request.VerifyStaminaRecoverIntervalMinutesRequest request,
                UnityAction<AsyncResult<Result.VerifyStaminaRecoverIntervalMinutesResult>> callback
        ) =>
            new VerifyStaminaRecoverIntervalMinutesTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToCoroutine(callback);

        public IFuture<Result.VerifyStaminaRecoverIntervalMinutesResult> VerifyStaminaRecoverIntervalMinutesFuture(
                Request.VerifyStaminaRecoverIntervalMinutesRequest request
        ) =>
            new VerifyStaminaRecoverIntervalMinutesTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
        public UniTask<Result.VerifyStaminaRecoverIntervalMinutesResult> VerifyStaminaRecoverIntervalMinutesAsync(
            Request.VerifyStaminaRecoverIntervalMinutesRequest request
        ) =>
            new VerifyStaminaRecoverIntervalMinutesTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsUniTask<Result.VerifyStaminaRecoverIntervalMinutesResult>();
    #else
        public VerifyStaminaRecoverIntervalMinutesTask VerifyStaminaRecoverIntervalMinutesAsync(
                Request.VerifyStaminaRecoverIntervalMinutesRequest request
        )
        {
            return new VerifyStaminaRecoverIntervalMinutesTask(
                Gs2WebSocketSession,
                request
            );
        }
    #endif
#else
        public Task<Result.VerifyStaminaRecoverIntervalMinutesResult> VerifyStaminaRecoverIntervalMinutesAsync(
            Request.VerifyStaminaRecoverIntervalMinutesRequest request
        ) =>
            new VerifyStaminaRecoverIntervalMinutesTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsTask();
#endif


        public class VerifyStaminaRecoverIntervalMinutesByUserIdTask : Gs2WebSocketSessionTask<Request.VerifyStaminaRecoverIntervalMinutesByUserIdRequest, Result.VerifyStaminaRecoverIntervalMinutesByUserIdResult>
        {
            public VerifyStaminaRecoverIntervalMinutesByUserIdTask(IGs2Session session, Request.VerifyStaminaRecoverIntervalMinutesByUserIdRequest request) : base(session, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(Request.VerifyStaminaRecoverIntervalMinutesByUserIdRequest request)
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
                if (request.StaminaName != null)
                {
                    jsonWriter.WritePropertyName("staminaName");
                    jsonWriter.Write(request.StaminaName.ToString());
                }
                if (request.VerifyType != null)
                {
                    jsonWriter.WritePropertyName("verifyType");
                    jsonWriter.Write(request.VerifyType.ToString());
                }
                if (request.Value != null)
                {
                    jsonWriter.WritePropertyName("value");
                    jsonWriter.Write(request.Value.ToString());
                }
                if (request.MultiplyValueSpecifyingQuantity != null)
                {
                    jsonWriter.WritePropertyName("multiplyValueSpecifyingQuantity");
                    jsonWriter.Write(request.MultiplyValueSpecifyingQuantity.ToString());
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
                if (request.DuplicationAvoider != null)
                {
                    jsonWriter.WritePropertyName("xGs2DuplicationAvoider");
                    jsonWriter.Write(request.DuplicationAvoider);
                }
                if (request.DryRun)
                {
                    jsonWriter.WritePropertyName("xGs2DryRun");
                    jsonWriter.Write("true");
                }

                AddHeader(
                    Session.Credential,
                    "stamina",
                    "stamina",
                    "verifyStaminaRecoverIntervalMinutesByUserId",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
        public IEnumerator VerifyStaminaRecoverIntervalMinutesByUserId(
                Request.VerifyStaminaRecoverIntervalMinutesByUserIdRequest request,
                UnityAction<AsyncResult<Result.VerifyStaminaRecoverIntervalMinutesByUserIdResult>> callback
        ) =>
            new VerifyStaminaRecoverIntervalMinutesByUserIdTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToCoroutine(callback);

        public IFuture<Result.VerifyStaminaRecoverIntervalMinutesByUserIdResult> VerifyStaminaRecoverIntervalMinutesByUserIdFuture(
                Request.VerifyStaminaRecoverIntervalMinutesByUserIdRequest request
        ) =>
            new VerifyStaminaRecoverIntervalMinutesByUserIdTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
        public UniTask<Result.VerifyStaminaRecoverIntervalMinutesByUserIdResult> VerifyStaminaRecoverIntervalMinutesByUserIdAsync(
            Request.VerifyStaminaRecoverIntervalMinutesByUserIdRequest request
        ) =>
            new VerifyStaminaRecoverIntervalMinutesByUserIdTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsUniTask<Result.VerifyStaminaRecoverIntervalMinutesByUserIdResult>();
    #else
        public VerifyStaminaRecoverIntervalMinutesByUserIdTask VerifyStaminaRecoverIntervalMinutesByUserIdAsync(
                Request.VerifyStaminaRecoverIntervalMinutesByUserIdRequest request
        )
        {
            return new VerifyStaminaRecoverIntervalMinutesByUserIdTask(
                Gs2WebSocketSession,
                request
            );
        }
    #endif
#else
        public Task<Result.VerifyStaminaRecoverIntervalMinutesByUserIdResult> VerifyStaminaRecoverIntervalMinutesByUserIdAsync(
            Request.VerifyStaminaRecoverIntervalMinutesByUserIdRequest request
        ) =>
            new VerifyStaminaRecoverIntervalMinutesByUserIdTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsTask();
#endif


        public class VerifyStaminaRecoverValueTask : Gs2WebSocketSessionTask<Request.VerifyStaminaRecoverValueRequest, Result.VerifyStaminaRecoverValueResult>
        {
            public VerifyStaminaRecoverValueTask(IGs2Session session, Request.VerifyStaminaRecoverValueRequest request) : base(session, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(Request.VerifyStaminaRecoverValueRequest request)
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
                if (request.StaminaName != null)
                {
                    jsonWriter.WritePropertyName("staminaName");
                    jsonWriter.Write(request.StaminaName.ToString());
                }
                if (request.VerifyType != null)
                {
                    jsonWriter.WritePropertyName("verifyType");
                    jsonWriter.Write(request.VerifyType.ToString());
                }
                if (request.Value != null)
                {
                    jsonWriter.WritePropertyName("value");
                    jsonWriter.Write(request.Value.ToString());
                }
                if (request.MultiplyValueSpecifyingQuantity != null)
                {
                    jsonWriter.WritePropertyName("multiplyValueSpecifyingQuantity");
                    jsonWriter.Write(request.MultiplyValueSpecifyingQuantity.ToString());
                }
                if (request.ContextStack != null)
                {
                    jsonWriter.WritePropertyName("contextStack");
                    jsonWriter.Write(request.ContextStack.ToString());
                }
                if (request.AccessToken != null)
                {
                    jsonWriter.WritePropertyName("xGs2AccessToken");
                    jsonWriter.Write(request.AccessToken);
                }
                if (request.DuplicationAvoider != null)
                {
                    jsonWriter.WritePropertyName("xGs2DuplicationAvoider");
                    jsonWriter.Write(request.DuplicationAvoider);
                }
                if (request.DryRun)
                {
                    jsonWriter.WritePropertyName("xGs2DryRun");
                    jsonWriter.Write("true");
                }

                AddHeader(
                    Session.Credential,
                    "stamina",
                    "stamina",
                    "verifyStaminaRecoverValue",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
        public IEnumerator VerifyStaminaRecoverValue(
                Request.VerifyStaminaRecoverValueRequest request,
                UnityAction<AsyncResult<Result.VerifyStaminaRecoverValueResult>> callback
        ) =>
            new VerifyStaminaRecoverValueTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToCoroutine(callback);

        public IFuture<Result.VerifyStaminaRecoverValueResult> VerifyStaminaRecoverValueFuture(
                Request.VerifyStaminaRecoverValueRequest request
        ) =>
            new VerifyStaminaRecoverValueTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
        public UniTask<Result.VerifyStaminaRecoverValueResult> VerifyStaminaRecoverValueAsync(
            Request.VerifyStaminaRecoverValueRequest request
        ) =>
            new VerifyStaminaRecoverValueTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsUniTask<Result.VerifyStaminaRecoverValueResult>();
    #else
        public VerifyStaminaRecoverValueTask VerifyStaminaRecoverValueAsync(
                Request.VerifyStaminaRecoverValueRequest request
        )
        {
            return new VerifyStaminaRecoverValueTask(
                Gs2WebSocketSession,
                request
            );
        }
    #endif
#else
        public Task<Result.VerifyStaminaRecoverValueResult> VerifyStaminaRecoverValueAsync(
            Request.VerifyStaminaRecoverValueRequest request
        ) =>
            new VerifyStaminaRecoverValueTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsTask();
#endif


        public class VerifyStaminaRecoverValueByUserIdTask : Gs2WebSocketSessionTask<Request.VerifyStaminaRecoverValueByUserIdRequest, Result.VerifyStaminaRecoverValueByUserIdResult>
        {
            public VerifyStaminaRecoverValueByUserIdTask(IGs2Session session, Request.VerifyStaminaRecoverValueByUserIdRequest request) : base(session, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(Request.VerifyStaminaRecoverValueByUserIdRequest request)
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
                if (request.StaminaName != null)
                {
                    jsonWriter.WritePropertyName("staminaName");
                    jsonWriter.Write(request.StaminaName.ToString());
                }
                if (request.VerifyType != null)
                {
                    jsonWriter.WritePropertyName("verifyType");
                    jsonWriter.Write(request.VerifyType.ToString());
                }
                if (request.Value != null)
                {
                    jsonWriter.WritePropertyName("value");
                    jsonWriter.Write(request.Value.ToString());
                }
                if (request.MultiplyValueSpecifyingQuantity != null)
                {
                    jsonWriter.WritePropertyName("multiplyValueSpecifyingQuantity");
                    jsonWriter.Write(request.MultiplyValueSpecifyingQuantity.ToString());
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
                if (request.DuplicationAvoider != null)
                {
                    jsonWriter.WritePropertyName("xGs2DuplicationAvoider");
                    jsonWriter.Write(request.DuplicationAvoider);
                }
                if (request.DryRun)
                {
                    jsonWriter.WritePropertyName("xGs2DryRun");
                    jsonWriter.Write("true");
                }

                AddHeader(
                    Session.Credential,
                    "stamina",
                    "stamina",
                    "verifyStaminaRecoverValueByUserId",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
        public IEnumerator VerifyStaminaRecoverValueByUserId(
                Request.VerifyStaminaRecoverValueByUserIdRequest request,
                UnityAction<AsyncResult<Result.VerifyStaminaRecoverValueByUserIdResult>> callback
        ) =>
            new VerifyStaminaRecoverValueByUserIdTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToCoroutine(callback);

        public IFuture<Result.VerifyStaminaRecoverValueByUserIdResult> VerifyStaminaRecoverValueByUserIdFuture(
                Request.VerifyStaminaRecoverValueByUserIdRequest request
        ) =>
            new VerifyStaminaRecoverValueByUserIdTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
        public UniTask<Result.VerifyStaminaRecoverValueByUserIdResult> VerifyStaminaRecoverValueByUserIdAsync(
            Request.VerifyStaminaRecoverValueByUserIdRequest request
        ) =>
            new VerifyStaminaRecoverValueByUserIdTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsUniTask<Result.VerifyStaminaRecoverValueByUserIdResult>();
    #else
        public VerifyStaminaRecoverValueByUserIdTask VerifyStaminaRecoverValueByUserIdAsync(
                Request.VerifyStaminaRecoverValueByUserIdRequest request
        )
        {
            return new VerifyStaminaRecoverValueByUserIdTask(
                Gs2WebSocketSession,
                request
            );
        }
    #endif
#else
        public Task<Result.VerifyStaminaRecoverValueByUserIdResult> VerifyStaminaRecoverValueByUserIdAsync(
            Request.VerifyStaminaRecoverValueByUserIdRequest request
        ) =>
            new VerifyStaminaRecoverValueByUserIdTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsTask();
#endif


        public class VerifyStaminaOverflowValueTask : Gs2WebSocketSessionTask<Request.VerifyStaminaOverflowValueRequest, Result.VerifyStaminaOverflowValueResult>
        {
            public VerifyStaminaOverflowValueTask(IGs2Session session, Request.VerifyStaminaOverflowValueRequest request) : base(session, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(Request.VerifyStaminaOverflowValueRequest request)
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
                if (request.StaminaName != null)
                {
                    jsonWriter.WritePropertyName("staminaName");
                    jsonWriter.Write(request.StaminaName.ToString());
                }
                if (request.VerifyType != null)
                {
                    jsonWriter.WritePropertyName("verifyType");
                    jsonWriter.Write(request.VerifyType.ToString());
                }
                if (request.Value != null)
                {
                    jsonWriter.WritePropertyName("value");
                    jsonWriter.Write(request.Value.ToString());
                }
                if (request.MultiplyValueSpecifyingQuantity != null)
                {
                    jsonWriter.WritePropertyName("multiplyValueSpecifyingQuantity");
                    jsonWriter.Write(request.MultiplyValueSpecifyingQuantity.ToString());
                }
                if (request.ContextStack != null)
                {
                    jsonWriter.WritePropertyName("contextStack");
                    jsonWriter.Write(request.ContextStack.ToString());
                }
                if (request.AccessToken != null)
                {
                    jsonWriter.WritePropertyName("xGs2AccessToken");
                    jsonWriter.Write(request.AccessToken);
                }
                if (request.DuplicationAvoider != null)
                {
                    jsonWriter.WritePropertyName("xGs2DuplicationAvoider");
                    jsonWriter.Write(request.DuplicationAvoider);
                }
                if (request.DryRun)
                {
                    jsonWriter.WritePropertyName("xGs2DryRun");
                    jsonWriter.Write("true");
                }

                AddHeader(
                    Session.Credential,
                    "stamina",
                    "stamina",
                    "verifyStaminaOverflowValue",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
        public IEnumerator VerifyStaminaOverflowValue(
                Request.VerifyStaminaOverflowValueRequest request,
                UnityAction<AsyncResult<Result.VerifyStaminaOverflowValueResult>> callback
        ) =>
            new VerifyStaminaOverflowValueTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToCoroutine(callback);

        public IFuture<Result.VerifyStaminaOverflowValueResult> VerifyStaminaOverflowValueFuture(
                Request.VerifyStaminaOverflowValueRequest request
        ) =>
            new VerifyStaminaOverflowValueTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
        public UniTask<Result.VerifyStaminaOverflowValueResult> VerifyStaminaOverflowValueAsync(
            Request.VerifyStaminaOverflowValueRequest request
        ) =>
            new VerifyStaminaOverflowValueTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsUniTask<Result.VerifyStaminaOverflowValueResult>();
    #else
        public VerifyStaminaOverflowValueTask VerifyStaminaOverflowValueAsync(
                Request.VerifyStaminaOverflowValueRequest request
        )
        {
            return new VerifyStaminaOverflowValueTask(
                Gs2WebSocketSession,
                request
            );
        }
    #endif
#else
        public Task<Result.VerifyStaminaOverflowValueResult> VerifyStaminaOverflowValueAsync(
            Request.VerifyStaminaOverflowValueRequest request
        ) =>
            new VerifyStaminaOverflowValueTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsTask();
#endif


        public class VerifyStaminaOverflowValueByUserIdTask : Gs2WebSocketSessionTask<Request.VerifyStaminaOverflowValueByUserIdRequest, Result.VerifyStaminaOverflowValueByUserIdResult>
        {
            public VerifyStaminaOverflowValueByUserIdTask(IGs2Session session, Request.VerifyStaminaOverflowValueByUserIdRequest request) : base(session, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(Request.VerifyStaminaOverflowValueByUserIdRequest request)
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
                if (request.StaminaName != null)
                {
                    jsonWriter.WritePropertyName("staminaName");
                    jsonWriter.Write(request.StaminaName.ToString());
                }
                if (request.VerifyType != null)
                {
                    jsonWriter.WritePropertyName("verifyType");
                    jsonWriter.Write(request.VerifyType.ToString());
                }
                if (request.Value != null)
                {
                    jsonWriter.WritePropertyName("value");
                    jsonWriter.Write(request.Value.ToString());
                }
                if (request.MultiplyValueSpecifyingQuantity != null)
                {
                    jsonWriter.WritePropertyName("multiplyValueSpecifyingQuantity");
                    jsonWriter.Write(request.MultiplyValueSpecifyingQuantity.ToString());
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
                if (request.DuplicationAvoider != null)
                {
                    jsonWriter.WritePropertyName("xGs2DuplicationAvoider");
                    jsonWriter.Write(request.DuplicationAvoider);
                }
                if (request.DryRun)
                {
                    jsonWriter.WritePropertyName("xGs2DryRun");
                    jsonWriter.Write("true");
                }

                AddHeader(
                    Session.Credential,
                    "stamina",
                    "stamina",
                    "verifyStaminaOverflowValueByUserId",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
        public IEnumerator VerifyStaminaOverflowValueByUserId(
                Request.VerifyStaminaOverflowValueByUserIdRequest request,
                UnityAction<AsyncResult<Result.VerifyStaminaOverflowValueByUserIdResult>> callback
        ) =>
            new VerifyStaminaOverflowValueByUserIdTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToCoroutine(callback);

        public IFuture<Result.VerifyStaminaOverflowValueByUserIdResult> VerifyStaminaOverflowValueByUserIdFuture(
                Request.VerifyStaminaOverflowValueByUserIdRequest request
        ) =>
            new VerifyStaminaOverflowValueByUserIdTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
        public UniTask<Result.VerifyStaminaOverflowValueByUserIdResult> VerifyStaminaOverflowValueByUserIdAsync(
            Request.VerifyStaminaOverflowValueByUserIdRequest request
        ) =>
            new VerifyStaminaOverflowValueByUserIdTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsUniTask<Result.VerifyStaminaOverflowValueByUserIdResult>();
    #else
        public VerifyStaminaOverflowValueByUserIdTask VerifyStaminaOverflowValueByUserIdAsync(
                Request.VerifyStaminaOverflowValueByUserIdRequest request
        )
        {
            return new VerifyStaminaOverflowValueByUserIdTask(
                Gs2WebSocketSession,
                request
            );
        }
    #endif
#else
        public Task<Result.VerifyStaminaOverflowValueByUserIdResult> VerifyStaminaOverflowValueByUserIdAsync(
            Request.VerifyStaminaOverflowValueByUserIdRequest request
        ) =>
            new VerifyStaminaOverflowValueByUserIdTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsTask();
#endif


        public class RecoverStaminaByStampSheetTask : Gs2WebSocketSessionTask<Request.RecoverStaminaByStampSheetRequest, Result.RecoverStaminaByStampSheetResult>
        {
            public RecoverStaminaByStampSheetTask(IGs2Session session, Request.RecoverStaminaByStampSheetRequest request) : base(session, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(Request.RecoverStaminaByStampSheetRequest request)
            {
                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);

                jsonWriter.WriteObjectStart();

                if (request.StampSheet != null)
                {
                    jsonWriter.WritePropertyName("stampSheet");
                    jsonWriter.Write(request.StampSheet.ToString());
                }
                if (request.KeyId != null)
                {
                    jsonWriter.WritePropertyName("keyId");
                    jsonWriter.Write(request.KeyId.ToString());
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
                    "stamina",
                    "stamina",
                    "recoverStaminaByStampSheet",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
        public IEnumerator RecoverStaminaByStampSheet(
                Request.RecoverStaminaByStampSheetRequest request,
                UnityAction<AsyncResult<Result.RecoverStaminaByStampSheetResult>> callback
        ) =>
            new RecoverStaminaByStampSheetTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToCoroutine(callback);

        public IFuture<Result.RecoverStaminaByStampSheetResult> RecoverStaminaByStampSheetFuture(
                Request.RecoverStaminaByStampSheetRequest request
        ) =>
            new RecoverStaminaByStampSheetTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
        public UniTask<Result.RecoverStaminaByStampSheetResult> RecoverStaminaByStampSheetAsync(
            Request.RecoverStaminaByStampSheetRequest request
        ) =>
            new RecoverStaminaByStampSheetTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsUniTask<Result.RecoverStaminaByStampSheetResult>();
    #else
        public RecoverStaminaByStampSheetTask RecoverStaminaByStampSheetAsync(
                Request.RecoverStaminaByStampSheetRequest request
        )
        {
            return new RecoverStaminaByStampSheetTask(
                Gs2WebSocketSession,
                request
            );
        }
    #endif
#else
        public Task<Result.RecoverStaminaByStampSheetResult> RecoverStaminaByStampSheetAsync(
            Request.RecoverStaminaByStampSheetRequest request
        ) =>
            new RecoverStaminaByStampSheetTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsTask();
#endif


        public class RaiseMaxValueByStampSheetTask : Gs2WebSocketSessionTask<Request.RaiseMaxValueByStampSheetRequest, Result.RaiseMaxValueByStampSheetResult>
        {
            public RaiseMaxValueByStampSheetTask(IGs2Session session, Request.RaiseMaxValueByStampSheetRequest request) : base(session, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(Request.RaiseMaxValueByStampSheetRequest request)
            {
                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);

                jsonWriter.WriteObjectStart();

                if (request.StampSheet != null)
                {
                    jsonWriter.WritePropertyName("stampSheet");
                    jsonWriter.Write(request.StampSheet.ToString());
                }
                if (request.KeyId != null)
                {
                    jsonWriter.WritePropertyName("keyId");
                    jsonWriter.Write(request.KeyId.ToString());
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
                    "stamina",
                    "stamina",
                    "raiseMaxValueByStampSheet",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
        public IEnumerator RaiseMaxValueByStampSheet(
                Request.RaiseMaxValueByStampSheetRequest request,
                UnityAction<AsyncResult<Result.RaiseMaxValueByStampSheetResult>> callback
        ) =>
            new RaiseMaxValueByStampSheetTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToCoroutine(callback);

        public IFuture<Result.RaiseMaxValueByStampSheetResult> RaiseMaxValueByStampSheetFuture(
                Request.RaiseMaxValueByStampSheetRequest request
        ) =>
            new RaiseMaxValueByStampSheetTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
        public UniTask<Result.RaiseMaxValueByStampSheetResult> RaiseMaxValueByStampSheetAsync(
            Request.RaiseMaxValueByStampSheetRequest request
        ) =>
            new RaiseMaxValueByStampSheetTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsUniTask<Result.RaiseMaxValueByStampSheetResult>();
    #else
        public RaiseMaxValueByStampSheetTask RaiseMaxValueByStampSheetAsync(
                Request.RaiseMaxValueByStampSheetRequest request
        )
        {
            return new RaiseMaxValueByStampSheetTask(
                Gs2WebSocketSession,
                request
            );
        }
    #endif
#else
        public Task<Result.RaiseMaxValueByStampSheetResult> RaiseMaxValueByStampSheetAsync(
            Request.RaiseMaxValueByStampSheetRequest request
        ) =>
            new RaiseMaxValueByStampSheetTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsTask();
#endif


        public class SetMaxValueByStampSheetTask : Gs2WebSocketSessionTask<Request.SetMaxValueByStampSheetRequest, Result.SetMaxValueByStampSheetResult>
        {
            public SetMaxValueByStampSheetTask(IGs2Session session, Request.SetMaxValueByStampSheetRequest request) : base(session, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(Request.SetMaxValueByStampSheetRequest request)
            {
                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);

                jsonWriter.WriteObjectStart();

                if (request.StampSheet != null)
                {
                    jsonWriter.WritePropertyName("stampSheet");
                    jsonWriter.Write(request.StampSheet.ToString());
                }
                if (request.KeyId != null)
                {
                    jsonWriter.WritePropertyName("keyId");
                    jsonWriter.Write(request.KeyId.ToString());
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
                    "stamina",
                    "stamina",
                    "setMaxValueByStampSheet",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
        public IEnumerator SetMaxValueByStampSheet(
                Request.SetMaxValueByStampSheetRequest request,
                UnityAction<AsyncResult<Result.SetMaxValueByStampSheetResult>> callback
        ) =>
            new SetMaxValueByStampSheetTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToCoroutine(callback);

        public IFuture<Result.SetMaxValueByStampSheetResult> SetMaxValueByStampSheetFuture(
                Request.SetMaxValueByStampSheetRequest request
        ) =>
            new SetMaxValueByStampSheetTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
        public UniTask<Result.SetMaxValueByStampSheetResult> SetMaxValueByStampSheetAsync(
            Request.SetMaxValueByStampSheetRequest request
        ) =>
            new SetMaxValueByStampSheetTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsUniTask<Result.SetMaxValueByStampSheetResult>();
    #else
        public SetMaxValueByStampSheetTask SetMaxValueByStampSheetAsync(
                Request.SetMaxValueByStampSheetRequest request
        )
        {
            return new SetMaxValueByStampSheetTask(
                Gs2WebSocketSession,
                request
            );
        }
    #endif
#else
        public Task<Result.SetMaxValueByStampSheetResult> SetMaxValueByStampSheetAsync(
            Request.SetMaxValueByStampSheetRequest request
        ) =>
            new SetMaxValueByStampSheetTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsTask();
#endif


        public class SetRecoverIntervalByStampSheetTask : Gs2WebSocketSessionTask<Request.SetRecoverIntervalByStampSheetRequest, Result.SetRecoverIntervalByStampSheetResult>
        {
            public SetRecoverIntervalByStampSheetTask(IGs2Session session, Request.SetRecoverIntervalByStampSheetRequest request) : base(session, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(Request.SetRecoverIntervalByStampSheetRequest request)
            {
                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);

                jsonWriter.WriteObjectStart();

                if (request.StampSheet != null)
                {
                    jsonWriter.WritePropertyName("stampSheet");
                    jsonWriter.Write(request.StampSheet.ToString());
                }
                if (request.KeyId != null)
                {
                    jsonWriter.WritePropertyName("keyId");
                    jsonWriter.Write(request.KeyId.ToString());
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
                    "stamina",
                    "stamina",
                    "setRecoverIntervalByStampSheet",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
        public IEnumerator SetRecoverIntervalByStampSheet(
                Request.SetRecoverIntervalByStampSheetRequest request,
                UnityAction<AsyncResult<Result.SetRecoverIntervalByStampSheetResult>> callback
        ) =>
            new SetRecoverIntervalByStampSheetTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToCoroutine(callback);

        public IFuture<Result.SetRecoverIntervalByStampSheetResult> SetRecoverIntervalByStampSheetFuture(
                Request.SetRecoverIntervalByStampSheetRequest request
        ) =>
            new SetRecoverIntervalByStampSheetTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
        public UniTask<Result.SetRecoverIntervalByStampSheetResult> SetRecoverIntervalByStampSheetAsync(
            Request.SetRecoverIntervalByStampSheetRequest request
        ) =>
            new SetRecoverIntervalByStampSheetTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsUniTask<Result.SetRecoverIntervalByStampSheetResult>();
    #else
        public SetRecoverIntervalByStampSheetTask SetRecoverIntervalByStampSheetAsync(
                Request.SetRecoverIntervalByStampSheetRequest request
        )
        {
            return new SetRecoverIntervalByStampSheetTask(
                Gs2WebSocketSession,
                request
            );
        }
    #endif
#else
        public Task<Result.SetRecoverIntervalByStampSheetResult> SetRecoverIntervalByStampSheetAsync(
            Request.SetRecoverIntervalByStampSheetRequest request
        ) =>
            new SetRecoverIntervalByStampSheetTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsTask();
#endif


        public class SetRecoverValueByStampSheetTask : Gs2WebSocketSessionTask<Request.SetRecoverValueByStampSheetRequest, Result.SetRecoverValueByStampSheetResult>
        {
            public SetRecoverValueByStampSheetTask(IGs2Session session, Request.SetRecoverValueByStampSheetRequest request) : base(session, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(Request.SetRecoverValueByStampSheetRequest request)
            {
                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);

                jsonWriter.WriteObjectStart();

                if (request.StampSheet != null)
                {
                    jsonWriter.WritePropertyName("stampSheet");
                    jsonWriter.Write(request.StampSheet.ToString());
                }
                if (request.KeyId != null)
                {
                    jsonWriter.WritePropertyName("keyId");
                    jsonWriter.Write(request.KeyId.ToString());
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
                    "stamina",
                    "stamina",
                    "setRecoverValueByStampSheet",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
        public IEnumerator SetRecoverValueByStampSheet(
                Request.SetRecoverValueByStampSheetRequest request,
                UnityAction<AsyncResult<Result.SetRecoverValueByStampSheetResult>> callback
        ) =>
            new SetRecoverValueByStampSheetTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToCoroutine(callback);

        public IFuture<Result.SetRecoverValueByStampSheetResult> SetRecoverValueByStampSheetFuture(
                Request.SetRecoverValueByStampSheetRequest request
        ) =>
            new SetRecoverValueByStampSheetTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
        public UniTask<Result.SetRecoverValueByStampSheetResult> SetRecoverValueByStampSheetAsync(
            Request.SetRecoverValueByStampSheetRequest request
        ) =>
            new SetRecoverValueByStampSheetTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsUniTask<Result.SetRecoverValueByStampSheetResult>();
    #else
        public SetRecoverValueByStampSheetTask SetRecoverValueByStampSheetAsync(
                Request.SetRecoverValueByStampSheetRequest request
        )
        {
            return new SetRecoverValueByStampSheetTask(
                Gs2WebSocketSession,
                request
            );
        }
    #endif
#else
        public Task<Result.SetRecoverValueByStampSheetResult> SetRecoverValueByStampSheetAsync(
            Request.SetRecoverValueByStampSheetRequest request
        ) =>
            new SetRecoverValueByStampSheetTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsTask();
#endif
	}
}