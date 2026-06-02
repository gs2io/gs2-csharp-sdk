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

namespace Gs2.Gs2Formation
{
	public class Gs2FormationWebSocketClient : AbstractGs2Client
	{

		public static string Endpoint = "formation";

        protected Gs2WebSocketSession Gs2WebSocketSession => (Gs2WebSocketSession) Gs2Session;

		public Gs2FormationWebSocketClient(Gs2WebSocketSession Gs2WebSocketSession) : base(Gs2WebSocketSession)
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
                if (request.UpdateMoldScript != null)
                {
                    jsonWriter.WritePropertyName("updateMoldScript");
                    request.UpdateMoldScript.WriteJson(jsonWriter);
                }
                if (request.UpdateFormScript != null)
                {
                    jsonWriter.WritePropertyName("updateFormScript");
                    request.UpdateFormScript.WriteJson(jsonWriter);
                }
                if (request.UpdatePropertyFormScript != null)
                {
                    jsonWriter.WritePropertyName("updatePropertyFormScript");
                    request.UpdatePropertyFormScript.WriteJson(jsonWriter);
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
                    "formation",
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
                    "formation",
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
                    "formation",
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
                if (request.UpdateMoldScript != null)
                {
                    jsonWriter.WritePropertyName("updateMoldScript");
                    request.UpdateMoldScript.WriteJson(jsonWriter);
                }
                if (request.UpdateFormScript != null)
                {
                    jsonWriter.WritePropertyName("updateFormScript");
                    request.UpdateFormScript.WriteJson(jsonWriter);
                }
                if (request.UpdatePropertyFormScript != null)
                {
                    jsonWriter.WritePropertyName("updatePropertyFormScript");
                    request.UpdatePropertyFormScript.WriteJson(jsonWriter);
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
                    "formation",
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
                    "formation",
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
                    "formation",
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
                    "formation",
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
                    "formation",
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
                    "formation",
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
                    "formation",
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
                    "formation",
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
                    "formation",
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
                    "formation",
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


        public class GetFormModelTask : Gs2WebSocketSessionTask<Request.GetFormModelRequest, Result.GetFormModelResult>
        {
            public GetFormModelTask(IGs2Session session, Request.GetFormModelRequest request) : base(session, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(Request.GetFormModelRequest request)
            {
                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);

                jsonWriter.WriteObjectStart();

                if (request.NamespaceName != null)
                {
                    jsonWriter.WritePropertyName("namespaceName");
                    jsonWriter.Write(request.NamespaceName.ToString());
                }
                if (request.MoldModelName != null)
                {
                    jsonWriter.WritePropertyName("moldModelName");
                    jsonWriter.Write(request.MoldModelName.ToString());
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
                    "formation",
                    "formModel",
                    "getFormModel",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
        public IEnumerator GetFormModel(
                Request.GetFormModelRequest request,
                UnityAction<AsyncResult<Result.GetFormModelResult>> callback
        ) =>
            new GetFormModelTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToCoroutine(callback);

        public IFuture<Result.GetFormModelResult> GetFormModelFuture(
                Request.GetFormModelRequest request
        ) =>
            new GetFormModelTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
        public UniTask<Result.GetFormModelResult> GetFormModelAsync(
            Request.GetFormModelRequest request
        ) =>
            new GetFormModelTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsUniTask<Result.GetFormModelResult>();
    #else
        public GetFormModelTask GetFormModelAsync(
                Request.GetFormModelRequest request
        )
        {
            return new GetFormModelTask(
                Gs2WebSocketSession,
                request
            );
        }
    #endif
#else
        public Task<Result.GetFormModelResult> GetFormModelAsync(
            Request.GetFormModelRequest request
        ) =>
            new GetFormModelTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsTask();
#endif


        public class CreateFormModelMasterTask : Gs2WebSocketSessionTask<Request.CreateFormModelMasterRequest, Result.CreateFormModelMasterResult>
        {
            public CreateFormModelMasterTask(IGs2Session session, Request.CreateFormModelMasterRequest request) : base(session, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(Request.CreateFormModelMasterRequest request)
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
                if (request.Slots != null)
                {
                    jsonWriter.WritePropertyName("slots");
                    jsonWriter.WriteArrayStart();
                    foreach(var item in request.Slots)
                    {
                        item.WriteJson(jsonWriter);
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
                    "formation",
                    "formModelMaster",
                    "createFormModelMaster",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
        public IEnumerator CreateFormModelMaster(
                Request.CreateFormModelMasterRequest request,
                UnityAction<AsyncResult<Result.CreateFormModelMasterResult>> callback
        ) =>
            new CreateFormModelMasterTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToCoroutine(callback);

        public IFuture<Result.CreateFormModelMasterResult> CreateFormModelMasterFuture(
                Request.CreateFormModelMasterRequest request
        ) =>
            new CreateFormModelMasterTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
        public UniTask<Result.CreateFormModelMasterResult> CreateFormModelMasterAsync(
            Request.CreateFormModelMasterRequest request
        ) =>
            new CreateFormModelMasterTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsUniTask<Result.CreateFormModelMasterResult>();
    #else
        public CreateFormModelMasterTask CreateFormModelMasterAsync(
                Request.CreateFormModelMasterRequest request
        )
        {
            return new CreateFormModelMasterTask(
                Gs2WebSocketSession,
                request
            );
        }
    #endif
#else
        public Task<Result.CreateFormModelMasterResult> CreateFormModelMasterAsync(
            Request.CreateFormModelMasterRequest request
        ) =>
            new CreateFormModelMasterTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsTask();
#endif


        public class GetFormModelMasterTask : Gs2WebSocketSessionTask<Request.GetFormModelMasterRequest, Result.GetFormModelMasterResult>
        {
            public GetFormModelMasterTask(IGs2Session session, Request.GetFormModelMasterRequest request) : base(session, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(Request.GetFormModelMasterRequest request)
            {
                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);

                jsonWriter.WriteObjectStart();

                if (request.NamespaceName != null)
                {
                    jsonWriter.WritePropertyName("namespaceName");
                    jsonWriter.Write(request.NamespaceName.ToString());
                }
                if (request.FormModelName != null)
                {
                    jsonWriter.WritePropertyName("formModelName");
                    jsonWriter.Write(request.FormModelName.ToString());
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
                    "formation",
                    "formModelMaster",
                    "getFormModelMaster",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
        public IEnumerator GetFormModelMaster(
                Request.GetFormModelMasterRequest request,
                UnityAction<AsyncResult<Result.GetFormModelMasterResult>> callback
        ) =>
            new GetFormModelMasterTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToCoroutine(callback);

        public IFuture<Result.GetFormModelMasterResult> GetFormModelMasterFuture(
                Request.GetFormModelMasterRequest request
        ) =>
            new GetFormModelMasterTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
        public UniTask<Result.GetFormModelMasterResult> GetFormModelMasterAsync(
            Request.GetFormModelMasterRequest request
        ) =>
            new GetFormModelMasterTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsUniTask<Result.GetFormModelMasterResult>();
    #else
        public GetFormModelMasterTask GetFormModelMasterAsync(
                Request.GetFormModelMasterRequest request
        )
        {
            return new GetFormModelMasterTask(
                Gs2WebSocketSession,
                request
            );
        }
    #endif
#else
        public Task<Result.GetFormModelMasterResult> GetFormModelMasterAsync(
            Request.GetFormModelMasterRequest request
        ) =>
            new GetFormModelMasterTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsTask();
#endif


        public class UpdateFormModelMasterTask : Gs2WebSocketSessionTask<Request.UpdateFormModelMasterRequest, Result.UpdateFormModelMasterResult>
        {
            public UpdateFormModelMasterTask(IGs2Session session, Request.UpdateFormModelMasterRequest request) : base(session, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(Request.UpdateFormModelMasterRequest request)
            {
                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);

                jsonWriter.WriteObjectStart();

                if (request.NamespaceName != null)
                {
                    jsonWriter.WritePropertyName("namespaceName");
                    jsonWriter.Write(request.NamespaceName.ToString());
                }
                if (request.FormModelName != null)
                {
                    jsonWriter.WritePropertyName("formModelName");
                    jsonWriter.Write(request.FormModelName.ToString());
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
                if (request.Slots != null)
                {
                    jsonWriter.WritePropertyName("slots");
                    jsonWriter.WriteArrayStart();
                    foreach(var item in request.Slots)
                    {
                        item.WriteJson(jsonWriter);
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
                    "formation",
                    "formModelMaster",
                    "updateFormModelMaster",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
        public IEnumerator UpdateFormModelMaster(
                Request.UpdateFormModelMasterRequest request,
                UnityAction<AsyncResult<Result.UpdateFormModelMasterResult>> callback
        ) =>
            new UpdateFormModelMasterTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToCoroutine(callback);

        public IFuture<Result.UpdateFormModelMasterResult> UpdateFormModelMasterFuture(
                Request.UpdateFormModelMasterRequest request
        ) =>
            new UpdateFormModelMasterTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
        public UniTask<Result.UpdateFormModelMasterResult> UpdateFormModelMasterAsync(
            Request.UpdateFormModelMasterRequest request
        ) =>
            new UpdateFormModelMasterTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsUniTask<Result.UpdateFormModelMasterResult>();
    #else
        public UpdateFormModelMasterTask UpdateFormModelMasterAsync(
                Request.UpdateFormModelMasterRequest request
        )
        {
            return new UpdateFormModelMasterTask(
                Gs2WebSocketSession,
                request
            );
        }
    #endif
#else
        public Task<Result.UpdateFormModelMasterResult> UpdateFormModelMasterAsync(
            Request.UpdateFormModelMasterRequest request
        ) =>
            new UpdateFormModelMasterTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsTask();
#endif


        public class DeleteFormModelMasterTask : Gs2WebSocketSessionTask<Request.DeleteFormModelMasterRequest, Result.DeleteFormModelMasterResult>
        {
            public DeleteFormModelMasterTask(IGs2Session session, Request.DeleteFormModelMasterRequest request) : base(session, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(Request.DeleteFormModelMasterRequest request)
            {
                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);

                jsonWriter.WriteObjectStart();

                if (request.NamespaceName != null)
                {
                    jsonWriter.WritePropertyName("namespaceName");
                    jsonWriter.Write(request.NamespaceName.ToString());
                }
                if (request.FormModelName != null)
                {
                    jsonWriter.WritePropertyName("formModelName");
                    jsonWriter.Write(request.FormModelName.ToString());
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
                    "formation",
                    "formModelMaster",
                    "deleteFormModelMaster",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
        public IEnumerator DeleteFormModelMaster(
                Request.DeleteFormModelMasterRequest request,
                UnityAction<AsyncResult<Result.DeleteFormModelMasterResult>> callback
        ) =>
            new DeleteFormModelMasterTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToCoroutine(callback);

        public IFuture<Result.DeleteFormModelMasterResult> DeleteFormModelMasterFuture(
                Request.DeleteFormModelMasterRequest request
        ) =>
            new DeleteFormModelMasterTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
        public UniTask<Result.DeleteFormModelMasterResult> DeleteFormModelMasterAsync(
            Request.DeleteFormModelMasterRequest request
        ) =>
            new DeleteFormModelMasterTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsUniTask<Result.DeleteFormModelMasterResult>();
    #else
        public DeleteFormModelMasterTask DeleteFormModelMasterAsync(
                Request.DeleteFormModelMasterRequest request
        )
        {
            return new DeleteFormModelMasterTask(
                Gs2WebSocketSession,
                request
            );
        }
    #endif
#else
        public Task<Result.DeleteFormModelMasterResult> DeleteFormModelMasterAsync(
            Request.DeleteFormModelMasterRequest request
        ) =>
            new DeleteFormModelMasterTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsTask();
#endif


        public class GetMoldModelTask : Gs2WebSocketSessionTask<Request.GetMoldModelRequest, Result.GetMoldModelResult>
        {
            public GetMoldModelTask(IGs2Session session, Request.GetMoldModelRequest request) : base(session, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(Request.GetMoldModelRequest request)
            {
                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);

                jsonWriter.WriteObjectStart();

                if (request.NamespaceName != null)
                {
                    jsonWriter.WritePropertyName("namespaceName");
                    jsonWriter.Write(request.NamespaceName.ToString());
                }
                if (request.MoldModelName != null)
                {
                    jsonWriter.WritePropertyName("moldModelName");
                    jsonWriter.Write(request.MoldModelName.ToString());
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
                    "formation",
                    "moldModel",
                    "getMoldModel",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
        public IEnumerator GetMoldModel(
                Request.GetMoldModelRequest request,
                UnityAction<AsyncResult<Result.GetMoldModelResult>> callback
        ) =>
            new GetMoldModelTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToCoroutine(callback);

        public IFuture<Result.GetMoldModelResult> GetMoldModelFuture(
                Request.GetMoldModelRequest request
        ) =>
            new GetMoldModelTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
        public UniTask<Result.GetMoldModelResult> GetMoldModelAsync(
            Request.GetMoldModelRequest request
        ) =>
            new GetMoldModelTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsUniTask<Result.GetMoldModelResult>();
    #else
        public GetMoldModelTask GetMoldModelAsync(
                Request.GetMoldModelRequest request
        )
        {
            return new GetMoldModelTask(
                Gs2WebSocketSession,
                request
            );
        }
    #endif
#else
        public Task<Result.GetMoldModelResult> GetMoldModelAsync(
            Request.GetMoldModelRequest request
        ) =>
            new GetMoldModelTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsTask();
#endif


        public class CreateMoldModelMasterTask : Gs2WebSocketSessionTask<Request.CreateMoldModelMasterRequest, Result.CreateMoldModelMasterResult>
        {
            public CreateMoldModelMasterTask(IGs2Session session, Request.CreateMoldModelMasterRequest request) : base(session, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(Request.CreateMoldModelMasterRequest request)
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
                if (request.FormModelName != null)
                {
                    jsonWriter.WritePropertyName("formModelName");
                    jsonWriter.Write(request.FormModelName.ToString());
                }
                if (request.InitialMaxCapacity != null)
                {
                    jsonWriter.WritePropertyName("initialMaxCapacity");
                    jsonWriter.Write(request.InitialMaxCapacity.ToString());
                }
                if (request.MaxCapacity != null)
                {
                    jsonWriter.WritePropertyName("maxCapacity");
                    jsonWriter.Write(request.MaxCapacity.ToString());
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
                    "formation",
                    "moldModelMaster",
                    "createMoldModelMaster",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
        public IEnumerator CreateMoldModelMaster(
                Request.CreateMoldModelMasterRequest request,
                UnityAction<AsyncResult<Result.CreateMoldModelMasterResult>> callback
        ) =>
            new CreateMoldModelMasterTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToCoroutine(callback);

        public IFuture<Result.CreateMoldModelMasterResult> CreateMoldModelMasterFuture(
                Request.CreateMoldModelMasterRequest request
        ) =>
            new CreateMoldModelMasterTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
        public UniTask<Result.CreateMoldModelMasterResult> CreateMoldModelMasterAsync(
            Request.CreateMoldModelMasterRequest request
        ) =>
            new CreateMoldModelMasterTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsUniTask<Result.CreateMoldModelMasterResult>();
    #else
        public CreateMoldModelMasterTask CreateMoldModelMasterAsync(
                Request.CreateMoldModelMasterRequest request
        )
        {
            return new CreateMoldModelMasterTask(
                Gs2WebSocketSession,
                request
            );
        }
    #endif
#else
        public Task<Result.CreateMoldModelMasterResult> CreateMoldModelMasterAsync(
            Request.CreateMoldModelMasterRequest request
        ) =>
            new CreateMoldModelMasterTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsTask();
#endif


        public class GetMoldModelMasterTask : Gs2WebSocketSessionTask<Request.GetMoldModelMasterRequest, Result.GetMoldModelMasterResult>
        {
            public GetMoldModelMasterTask(IGs2Session session, Request.GetMoldModelMasterRequest request) : base(session, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(Request.GetMoldModelMasterRequest request)
            {
                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);

                jsonWriter.WriteObjectStart();

                if (request.NamespaceName != null)
                {
                    jsonWriter.WritePropertyName("namespaceName");
                    jsonWriter.Write(request.NamespaceName.ToString());
                }
                if (request.MoldModelName != null)
                {
                    jsonWriter.WritePropertyName("moldModelName");
                    jsonWriter.Write(request.MoldModelName.ToString());
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
                    "formation",
                    "moldModelMaster",
                    "getMoldModelMaster",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
        public IEnumerator GetMoldModelMaster(
                Request.GetMoldModelMasterRequest request,
                UnityAction<AsyncResult<Result.GetMoldModelMasterResult>> callback
        ) =>
            new GetMoldModelMasterTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToCoroutine(callback);

        public IFuture<Result.GetMoldModelMasterResult> GetMoldModelMasterFuture(
                Request.GetMoldModelMasterRequest request
        ) =>
            new GetMoldModelMasterTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
        public UniTask<Result.GetMoldModelMasterResult> GetMoldModelMasterAsync(
            Request.GetMoldModelMasterRequest request
        ) =>
            new GetMoldModelMasterTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsUniTask<Result.GetMoldModelMasterResult>();
    #else
        public GetMoldModelMasterTask GetMoldModelMasterAsync(
                Request.GetMoldModelMasterRequest request
        )
        {
            return new GetMoldModelMasterTask(
                Gs2WebSocketSession,
                request
            );
        }
    #endif
#else
        public Task<Result.GetMoldModelMasterResult> GetMoldModelMasterAsync(
            Request.GetMoldModelMasterRequest request
        ) =>
            new GetMoldModelMasterTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsTask();
#endif


        public class UpdateMoldModelMasterTask : Gs2WebSocketSessionTask<Request.UpdateMoldModelMasterRequest, Result.UpdateMoldModelMasterResult>
        {
            public UpdateMoldModelMasterTask(IGs2Session session, Request.UpdateMoldModelMasterRequest request) : base(session, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(Request.UpdateMoldModelMasterRequest request)
            {
                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);

                jsonWriter.WriteObjectStart();

                if (request.NamespaceName != null)
                {
                    jsonWriter.WritePropertyName("namespaceName");
                    jsonWriter.Write(request.NamespaceName.ToString());
                }
                if (request.MoldModelName != null)
                {
                    jsonWriter.WritePropertyName("moldModelName");
                    jsonWriter.Write(request.MoldModelName.ToString());
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
                if (request.FormModelName != null)
                {
                    jsonWriter.WritePropertyName("formModelName");
                    jsonWriter.Write(request.FormModelName.ToString());
                }
                if (request.InitialMaxCapacity != null)
                {
                    jsonWriter.WritePropertyName("initialMaxCapacity");
                    jsonWriter.Write(request.InitialMaxCapacity.ToString());
                }
                if (request.MaxCapacity != null)
                {
                    jsonWriter.WritePropertyName("maxCapacity");
                    jsonWriter.Write(request.MaxCapacity.ToString());
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
                    "formation",
                    "moldModelMaster",
                    "updateMoldModelMaster",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
        public IEnumerator UpdateMoldModelMaster(
                Request.UpdateMoldModelMasterRequest request,
                UnityAction<AsyncResult<Result.UpdateMoldModelMasterResult>> callback
        ) =>
            new UpdateMoldModelMasterTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToCoroutine(callback);

        public IFuture<Result.UpdateMoldModelMasterResult> UpdateMoldModelMasterFuture(
                Request.UpdateMoldModelMasterRequest request
        ) =>
            new UpdateMoldModelMasterTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
        public UniTask<Result.UpdateMoldModelMasterResult> UpdateMoldModelMasterAsync(
            Request.UpdateMoldModelMasterRequest request
        ) =>
            new UpdateMoldModelMasterTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsUniTask<Result.UpdateMoldModelMasterResult>();
    #else
        public UpdateMoldModelMasterTask UpdateMoldModelMasterAsync(
                Request.UpdateMoldModelMasterRequest request
        )
        {
            return new UpdateMoldModelMasterTask(
                Gs2WebSocketSession,
                request
            );
        }
    #endif
#else
        public Task<Result.UpdateMoldModelMasterResult> UpdateMoldModelMasterAsync(
            Request.UpdateMoldModelMasterRequest request
        ) =>
            new UpdateMoldModelMasterTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsTask();
#endif


        public class DeleteMoldModelMasterTask : Gs2WebSocketSessionTask<Request.DeleteMoldModelMasterRequest, Result.DeleteMoldModelMasterResult>
        {
            public DeleteMoldModelMasterTask(IGs2Session session, Request.DeleteMoldModelMasterRequest request) : base(session, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(Request.DeleteMoldModelMasterRequest request)
            {
                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);

                jsonWriter.WriteObjectStart();

                if (request.NamespaceName != null)
                {
                    jsonWriter.WritePropertyName("namespaceName");
                    jsonWriter.Write(request.NamespaceName.ToString());
                }
                if (request.MoldModelName != null)
                {
                    jsonWriter.WritePropertyName("moldModelName");
                    jsonWriter.Write(request.MoldModelName.ToString());
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
                    "formation",
                    "moldModelMaster",
                    "deleteMoldModelMaster",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
        public IEnumerator DeleteMoldModelMaster(
                Request.DeleteMoldModelMasterRequest request,
                UnityAction<AsyncResult<Result.DeleteMoldModelMasterResult>> callback
        ) =>
            new DeleteMoldModelMasterTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToCoroutine(callback);

        public IFuture<Result.DeleteMoldModelMasterResult> DeleteMoldModelMasterFuture(
                Request.DeleteMoldModelMasterRequest request
        ) =>
            new DeleteMoldModelMasterTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
        public UniTask<Result.DeleteMoldModelMasterResult> DeleteMoldModelMasterAsync(
            Request.DeleteMoldModelMasterRequest request
        ) =>
            new DeleteMoldModelMasterTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsUniTask<Result.DeleteMoldModelMasterResult>();
    #else
        public DeleteMoldModelMasterTask DeleteMoldModelMasterAsync(
                Request.DeleteMoldModelMasterRequest request
        )
        {
            return new DeleteMoldModelMasterTask(
                Gs2WebSocketSession,
                request
            );
        }
    #endif
#else
        public Task<Result.DeleteMoldModelMasterResult> DeleteMoldModelMasterAsync(
            Request.DeleteMoldModelMasterRequest request
        ) =>
            new DeleteMoldModelMasterTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsTask();
#endif


        public class GetPropertyFormModelTask : Gs2WebSocketSessionTask<Request.GetPropertyFormModelRequest, Result.GetPropertyFormModelResult>
        {
            public GetPropertyFormModelTask(IGs2Session session, Request.GetPropertyFormModelRequest request) : base(session, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(Request.GetPropertyFormModelRequest request)
            {
                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);

                jsonWriter.WriteObjectStart();

                if (request.NamespaceName != null)
                {
                    jsonWriter.WritePropertyName("namespaceName");
                    jsonWriter.Write(request.NamespaceName.ToString());
                }
                if (request.PropertyFormModelName != null)
                {
                    jsonWriter.WritePropertyName("propertyFormModelName");
                    jsonWriter.Write(request.PropertyFormModelName.ToString());
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
                    "formation",
                    "propertyFormModel",
                    "getPropertyFormModel",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
        public IEnumerator GetPropertyFormModel(
                Request.GetPropertyFormModelRequest request,
                UnityAction<AsyncResult<Result.GetPropertyFormModelResult>> callback
        ) =>
            new GetPropertyFormModelTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToCoroutine(callback);

        public IFuture<Result.GetPropertyFormModelResult> GetPropertyFormModelFuture(
                Request.GetPropertyFormModelRequest request
        ) =>
            new GetPropertyFormModelTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
        public UniTask<Result.GetPropertyFormModelResult> GetPropertyFormModelAsync(
            Request.GetPropertyFormModelRequest request
        ) =>
            new GetPropertyFormModelTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsUniTask<Result.GetPropertyFormModelResult>();
    #else
        public GetPropertyFormModelTask GetPropertyFormModelAsync(
                Request.GetPropertyFormModelRequest request
        )
        {
            return new GetPropertyFormModelTask(
                Gs2WebSocketSession,
                request
            );
        }
    #endif
#else
        public Task<Result.GetPropertyFormModelResult> GetPropertyFormModelAsync(
            Request.GetPropertyFormModelRequest request
        ) =>
            new GetPropertyFormModelTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsTask();
#endif


        public class CreatePropertyFormModelMasterTask : Gs2WebSocketSessionTask<Request.CreatePropertyFormModelMasterRequest, Result.CreatePropertyFormModelMasterResult>
        {
            public CreatePropertyFormModelMasterTask(IGs2Session session, Request.CreatePropertyFormModelMasterRequest request) : base(session, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(Request.CreatePropertyFormModelMasterRequest request)
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
                if (request.Slots != null)
                {
                    jsonWriter.WritePropertyName("slots");
                    jsonWriter.WriteArrayStart();
                    foreach(var item in request.Slots)
                    {
                        item.WriteJson(jsonWriter);
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
                    "formation",
                    "propertyFormModelMaster",
                    "createPropertyFormModelMaster",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
        public IEnumerator CreatePropertyFormModelMaster(
                Request.CreatePropertyFormModelMasterRequest request,
                UnityAction<AsyncResult<Result.CreatePropertyFormModelMasterResult>> callback
        ) =>
            new CreatePropertyFormModelMasterTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToCoroutine(callback);

        public IFuture<Result.CreatePropertyFormModelMasterResult> CreatePropertyFormModelMasterFuture(
                Request.CreatePropertyFormModelMasterRequest request
        ) =>
            new CreatePropertyFormModelMasterTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
        public UniTask<Result.CreatePropertyFormModelMasterResult> CreatePropertyFormModelMasterAsync(
            Request.CreatePropertyFormModelMasterRequest request
        ) =>
            new CreatePropertyFormModelMasterTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsUniTask<Result.CreatePropertyFormModelMasterResult>();
    #else
        public CreatePropertyFormModelMasterTask CreatePropertyFormModelMasterAsync(
                Request.CreatePropertyFormModelMasterRequest request
        )
        {
            return new CreatePropertyFormModelMasterTask(
                Gs2WebSocketSession,
                request
            );
        }
    #endif
#else
        public Task<Result.CreatePropertyFormModelMasterResult> CreatePropertyFormModelMasterAsync(
            Request.CreatePropertyFormModelMasterRequest request
        ) =>
            new CreatePropertyFormModelMasterTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsTask();
#endif


        public class GetPropertyFormModelMasterTask : Gs2WebSocketSessionTask<Request.GetPropertyFormModelMasterRequest, Result.GetPropertyFormModelMasterResult>
        {
            public GetPropertyFormModelMasterTask(IGs2Session session, Request.GetPropertyFormModelMasterRequest request) : base(session, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(Request.GetPropertyFormModelMasterRequest request)
            {
                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);

                jsonWriter.WriteObjectStart();

                if (request.NamespaceName != null)
                {
                    jsonWriter.WritePropertyName("namespaceName");
                    jsonWriter.Write(request.NamespaceName.ToString());
                }
                if (request.PropertyFormModelName != null)
                {
                    jsonWriter.WritePropertyName("propertyFormModelName");
                    jsonWriter.Write(request.PropertyFormModelName.ToString());
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
                    "formation",
                    "propertyFormModelMaster",
                    "getPropertyFormModelMaster",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
        public IEnumerator GetPropertyFormModelMaster(
                Request.GetPropertyFormModelMasterRequest request,
                UnityAction<AsyncResult<Result.GetPropertyFormModelMasterResult>> callback
        ) =>
            new GetPropertyFormModelMasterTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToCoroutine(callback);

        public IFuture<Result.GetPropertyFormModelMasterResult> GetPropertyFormModelMasterFuture(
                Request.GetPropertyFormModelMasterRequest request
        ) =>
            new GetPropertyFormModelMasterTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
        public UniTask<Result.GetPropertyFormModelMasterResult> GetPropertyFormModelMasterAsync(
            Request.GetPropertyFormModelMasterRequest request
        ) =>
            new GetPropertyFormModelMasterTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsUniTask<Result.GetPropertyFormModelMasterResult>();
    #else
        public GetPropertyFormModelMasterTask GetPropertyFormModelMasterAsync(
                Request.GetPropertyFormModelMasterRequest request
        )
        {
            return new GetPropertyFormModelMasterTask(
                Gs2WebSocketSession,
                request
            );
        }
    #endif
#else
        public Task<Result.GetPropertyFormModelMasterResult> GetPropertyFormModelMasterAsync(
            Request.GetPropertyFormModelMasterRequest request
        ) =>
            new GetPropertyFormModelMasterTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsTask();
#endif


        public class UpdatePropertyFormModelMasterTask : Gs2WebSocketSessionTask<Request.UpdatePropertyFormModelMasterRequest, Result.UpdatePropertyFormModelMasterResult>
        {
            public UpdatePropertyFormModelMasterTask(IGs2Session session, Request.UpdatePropertyFormModelMasterRequest request) : base(session, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(Request.UpdatePropertyFormModelMasterRequest request)
            {
                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);

                jsonWriter.WriteObjectStart();

                if (request.NamespaceName != null)
                {
                    jsonWriter.WritePropertyName("namespaceName");
                    jsonWriter.Write(request.NamespaceName.ToString());
                }
                if (request.PropertyFormModelName != null)
                {
                    jsonWriter.WritePropertyName("propertyFormModelName");
                    jsonWriter.Write(request.PropertyFormModelName.ToString());
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
                if (request.Slots != null)
                {
                    jsonWriter.WritePropertyName("slots");
                    jsonWriter.WriteArrayStart();
                    foreach(var item in request.Slots)
                    {
                        item.WriteJson(jsonWriter);
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
                    "formation",
                    "propertyFormModelMaster",
                    "updatePropertyFormModelMaster",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
        public IEnumerator UpdatePropertyFormModelMaster(
                Request.UpdatePropertyFormModelMasterRequest request,
                UnityAction<AsyncResult<Result.UpdatePropertyFormModelMasterResult>> callback
        ) =>
            new UpdatePropertyFormModelMasterTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToCoroutine(callback);

        public IFuture<Result.UpdatePropertyFormModelMasterResult> UpdatePropertyFormModelMasterFuture(
                Request.UpdatePropertyFormModelMasterRequest request
        ) =>
            new UpdatePropertyFormModelMasterTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
        public UniTask<Result.UpdatePropertyFormModelMasterResult> UpdatePropertyFormModelMasterAsync(
            Request.UpdatePropertyFormModelMasterRequest request
        ) =>
            new UpdatePropertyFormModelMasterTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsUniTask<Result.UpdatePropertyFormModelMasterResult>();
    #else
        public UpdatePropertyFormModelMasterTask UpdatePropertyFormModelMasterAsync(
                Request.UpdatePropertyFormModelMasterRequest request
        )
        {
            return new UpdatePropertyFormModelMasterTask(
                Gs2WebSocketSession,
                request
            );
        }
    #endif
#else
        public Task<Result.UpdatePropertyFormModelMasterResult> UpdatePropertyFormModelMasterAsync(
            Request.UpdatePropertyFormModelMasterRequest request
        ) =>
            new UpdatePropertyFormModelMasterTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsTask();
#endif


        public class DeletePropertyFormModelMasterTask : Gs2WebSocketSessionTask<Request.DeletePropertyFormModelMasterRequest, Result.DeletePropertyFormModelMasterResult>
        {
            public DeletePropertyFormModelMasterTask(IGs2Session session, Request.DeletePropertyFormModelMasterRequest request) : base(session, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(Request.DeletePropertyFormModelMasterRequest request)
            {
                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);

                jsonWriter.WriteObjectStart();

                if (request.NamespaceName != null)
                {
                    jsonWriter.WritePropertyName("namespaceName");
                    jsonWriter.Write(request.NamespaceName.ToString());
                }
                if (request.PropertyFormModelName != null)
                {
                    jsonWriter.WritePropertyName("propertyFormModelName");
                    jsonWriter.Write(request.PropertyFormModelName.ToString());
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
                    "formation",
                    "propertyFormModelMaster",
                    "deletePropertyFormModelMaster",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
        public IEnumerator DeletePropertyFormModelMaster(
                Request.DeletePropertyFormModelMasterRequest request,
                UnityAction<AsyncResult<Result.DeletePropertyFormModelMasterResult>> callback
        ) =>
            new DeletePropertyFormModelMasterTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToCoroutine(callback);

        public IFuture<Result.DeletePropertyFormModelMasterResult> DeletePropertyFormModelMasterFuture(
                Request.DeletePropertyFormModelMasterRequest request
        ) =>
            new DeletePropertyFormModelMasterTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
        public UniTask<Result.DeletePropertyFormModelMasterResult> DeletePropertyFormModelMasterAsync(
            Request.DeletePropertyFormModelMasterRequest request
        ) =>
            new DeletePropertyFormModelMasterTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsUniTask<Result.DeletePropertyFormModelMasterResult>();
    #else
        public DeletePropertyFormModelMasterTask DeletePropertyFormModelMasterAsync(
                Request.DeletePropertyFormModelMasterRequest request
        )
        {
            return new DeletePropertyFormModelMasterTask(
                Gs2WebSocketSession,
                request
            );
        }
    #endif
#else
        public Task<Result.DeletePropertyFormModelMasterResult> DeletePropertyFormModelMasterAsync(
            Request.DeletePropertyFormModelMasterRequest request
        ) =>
            new DeletePropertyFormModelMasterTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsTask();
#endif


        public class PreUpdateCurrentFormMasterTask : Gs2WebSocketSessionTask<Request.PreUpdateCurrentFormMasterRequest, Result.PreUpdateCurrentFormMasterResult>
        {
            public PreUpdateCurrentFormMasterTask(IGs2Session session, Request.PreUpdateCurrentFormMasterRequest request) : base(session, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(Request.PreUpdateCurrentFormMasterRequest request)
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
                    "formation",
                    "currentFormMaster",
                    "preUpdateCurrentFormMaster",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
        public IEnumerator PreUpdateCurrentFormMaster(
                Request.PreUpdateCurrentFormMasterRequest request,
                UnityAction<AsyncResult<Result.PreUpdateCurrentFormMasterResult>> callback
        ) =>
            new PreUpdateCurrentFormMasterTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToCoroutine(callback);

        public IFuture<Result.PreUpdateCurrentFormMasterResult> PreUpdateCurrentFormMasterFuture(
                Request.PreUpdateCurrentFormMasterRequest request
        ) =>
            new PreUpdateCurrentFormMasterTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
        public UniTask<Result.PreUpdateCurrentFormMasterResult> PreUpdateCurrentFormMasterAsync(
            Request.PreUpdateCurrentFormMasterRequest request
        ) =>
            new PreUpdateCurrentFormMasterTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsUniTask<Result.PreUpdateCurrentFormMasterResult>();
    #else
        public PreUpdateCurrentFormMasterTask PreUpdateCurrentFormMasterAsync(
                Request.PreUpdateCurrentFormMasterRequest request
        )
        {
            return new PreUpdateCurrentFormMasterTask(
                Gs2WebSocketSession,
                request
            );
        }
    #endif
#else
        public Task<Result.PreUpdateCurrentFormMasterResult> PreUpdateCurrentFormMasterAsync(
            Request.PreUpdateCurrentFormMasterRequest request
        ) =>
            new PreUpdateCurrentFormMasterTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsTask();
#endif


        public class GetMoldTask : Gs2WebSocketSessionTask<Request.GetMoldRequest, Result.GetMoldResult>
        {
            public GetMoldTask(IGs2Session session, Request.GetMoldRequest request) : base(session, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(Request.GetMoldRequest request)
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
                if (request.MoldModelName != null)
                {
                    jsonWriter.WritePropertyName("moldModelName");
                    jsonWriter.Write(request.MoldModelName.ToString());
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
                    "formation",
                    "mold",
                    "getMold",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
        public IEnumerator GetMold(
                Request.GetMoldRequest request,
                UnityAction<AsyncResult<Result.GetMoldResult>> callback
        ) =>
            new GetMoldTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToCoroutine(callback);

        public IFuture<Result.GetMoldResult> GetMoldFuture(
                Request.GetMoldRequest request
        ) =>
            new GetMoldTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
        public UniTask<Result.GetMoldResult> GetMoldAsync(
            Request.GetMoldRequest request
        ) =>
            new GetMoldTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsUniTask<Result.GetMoldResult>();
    #else
        public GetMoldTask GetMoldAsync(
                Request.GetMoldRequest request
        )
        {
            return new GetMoldTask(
                Gs2WebSocketSession,
                request
            );
        }
    #endif
#else
        public Task<Result.GetMoldResult> GetMoldAsync(
            Request.GetMoldRequest request
        ) =>
            new GetMoldTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsTask();
#endif


        public class GetMoldByUserIdTask : Gs2WebSocketSessionTask<Request.GetMoldByUserIdRequest, Result.GetMoldByUserIdResult>
        {
            public GetMoldByUserIdTask(IGs2Session session, Request.GetMoldByUserIdRequest request) : base(session, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(Request.GetMoldByUserIdRequest request)
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
                if (request.MoldModelName != null)
                {
                    jsonWriter.WritePropertyName("moldModelName");
                    jsonWriter.Write(request.MoldModelName.ToString());
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
                    "formation",
                    "mold",
                    "getMoldByUserId",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
        public IEnumerator GetMoldByUserId(
                Request.GetMoldByUserIdRequest request,
                UnityAction<AsyncResult<Result.GetMoldByUserIdResult>> callback
        ) =>
            new GetMoldByUserIdTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToCoroutine(callback);

        public IFuture<Result.GetMoldByUserIdResult> GetMoldByUserIdFuture(
                Request.GetMoldByUserIdRequest request
        ) =>
            new GetMoldByUserIdTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
        public UniTask<Result.GetMoldByUserIdResult> GetMoldByUserIdAsync(
            Request.GetMoldByUserIdRequest request
        ) =>
            new GetMoldByUserIdTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsUniTask<Result.GetMoldByUserIdResult>();
    #else
        public GetMoldByUserIdTask GetMoldByUserIdAsync(
                Request.GetMoldByUserIdRequest request
        )
        {
            return new GetMoldByUserIdTask(
                Gs2WebSocketSession,
                request
            );
        }
    #endif
#else
        public Task<Result.GetMoldByUserIdResult> GetMoldByUserIdAsync(
            Request.GetMoldByUserIdRequest request
        ) =>
            new GetMoldByUserIdTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsTask();
#endif


        public class SetMoldCapacityByUserIdTask : Gs2WebSocketSessionTask<Request.SetMoldCapacityByUserIdRequest, Result.SetMoldCapacityByUserIdResult>
        {
            public SetMoldCapacityByUserIdTask(IGs2Session session, Request.SetMoldCapacityByUserIdRequest request) : base(session, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(Request.SetMoldCapacityByUserIdRequest request)
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
                if (request.MoldModelName != null)
                {
                    jsonWriter.WritePropertyName("moldModelName");
                    jsonWriter.Write(request.MoldModelName.ToString());
                }
                if (request.Capacity != null)
                {
                    jsonWriter.WritePropertyName("capacity");
                    jsonWriter.Write(request.Capacity.ToString());
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
                    "formation",
                    "mold",
                    "setMoldCapacityByUserId",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
        public IEnumerator SetMoldCapacityByUserId(
                Request.SetMoldCapacityByUserIdRequest request,
                UnityAction<AsyncResult<Result.SetMoldCapacityByUserIdResult>> callback
        ) =>
            new SetMoldCapacityByUserIdTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToCoroutine(callback);

        public IFuture<Result.SetMoldCapacityByUserIdResult> SetMoldCapacityByUserIdFuture(
                Request.SetMoldCapacityByUserIdRequest request
        ) =>
            new SetMoldCapacityByUserIdTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
        public UniTask<Result.SetMoldCapacityByUserIdResult> SetMoldCapacityByUserIdAsync(
            Request.SetMoldCapacityByUserIdRequest request
        ) =>
            new SetMoldCapacityByUserIdTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsUniTask<Result.SetMoldCapacityByUserIdResult>();
    #else
        public SetMoldCapacityByUserIdTask SetMoldCapacityByUserIdAsync(
                Request.SetMoldCapacityByUserIdRequest request
        )
        {
            return new SetMoldCapacityByUserIdTask(
                Gs2WebSocketSession,
                request
            );
        }
    #endif
#else
        public Task<Result.SetMoldCapacityByUserIdResult> SetMoldCapacityByUserIdAsync(
            Request.SetMoldCapacityByUserIdRequest request
        ) =>
            new SetMoldCapacityByUserIdTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsTask();
#endif


        public class AddMoldCapacityByUserIdTask : Gs2WebSocketSessionTask<Request.AddMoldCapacityByUserIdRequest, Result.AddMoldCapacityByUserIdResult>
        {
            public AddMoldCapacityByUserIdTask(IGs2Session session, Request.AddMoldCapacityByUserIdRequest request) : base(session, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(Request.AddMoldCapacityByUserIdRequest request)
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
                if (request.MoldModelName != null)
                {
                    jsonWriter.WritePropertyName("moldModelName");
                    jsonWriter.Write(request.MoldModelName.ToString());
                }
                if (request.Capacity != null)
                {
                    jsonWriter.WritePropertyName("capacity");
                    jsonWriter.Write(request.Capacity.ToString());
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
                    "formation",
                    "mold",
                    "addMoldCapacityByUserId",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
        public IEnumerator AddMoldCapacityByUserId(
                Request.AddMoldCapacityByUserIdRequest request,
                UnityAction<AsyncResult<Result.AddMoldCapacityByUserIdResult>> callback
        ) =>
            new AddMoldCapacityByUserIdTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToCoroutine(callback);

        public IFuture<Result.AddMoldCapacityByUserIdResult> AddMoldCapacityByUserIdFuture(
                Request.AddMoldCapacityByUserIdRequest request
        ) =>
            new AddMoldCapacityByUserIdTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
        public UniTask<Result.AddMoldCapacityByUserIdResult> AddMoldCapacityByUserIdAsync(
            Request.AddMoldCapacityByUserIdRequest request
        ) =>
            new AddMoldCapacityByUserIdTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsUniTask<Result.AddMoldCapacityByUserIdResult>();
    #else
        public AddMoldCapacityByUserIdTask AddMoldCapacityByUserIdAsync(
                Request.AddMoldCapacityByUserIdRequest request
        )
        {
            return new AddMoldCapacityByUserIdTask(
                Gs2WebSocketSession,
                request
            );
        }
    #endif
#else
        public Task<Result.AddMoldCapacityByUserIdResult> AddMoldCapacityByUserIdAsync(
            Request.AddMoldCapacityByUserIdRequest request
        ) =>
            new AddMoldCapacityByUserIdTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsTask();
#endif


        public class SubMoldCapacityTask : Gs2WebSocketSessionTask<Request.SubMoldCapacityRequest, Result.SubMoldCapacityResult>
        {
            public SubMoldCapacityTask(IGs2Session session, Request.SubMoldCapacityRequest request) : base(session, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(Request.SubMoldCapacityRequest request)
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
                if (request.MoldModelName != null)
                {
                    jsonWriter.WritePropertyName("moldModelName");
                    jsonWriter.Write(request.MoldModelName.ToString());
                }
                if (request.Capacity != null)
                {
                    jsonWriter.WritePropertyName("capacity");
                    jsonWriter.Write(request.Capacity.ToString());
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
                    "formation",
                    "mold",
                    "subMoldCapacity",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
        public IEnumerator SubMoldCapacity(
                Request.SubMoldCapacityRequest request,
                UnityAction<AsyncResult<Result.SubMoldCapacityResult>> callback
        ) =>
            new SubMoldCapacityTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToCoroutine(callback);

        public IFuture<Result.SubMoldCapacityResult> SubMoldCapacityFuture(
                Request.SubMoldCapacityRequest request
        ) =>
            new SubMoldCapacityTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
        public UniTask<Result.SubMoldCapacityResult> SubMoldCapacityAsync(
            Request.SubMoldCapacityRequest request
        ) =>
            new SubMoldCapacityTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsUniTask<Result.SubMoldCapacityResult>();
    #else
        public SubMoldCapacityTask SubMoldCapacityAsync(
                Request.SubMoldCapacityRequest request
        )
        {
            return new SubMoldCapacityTask(
                Gs2WebSocketSession,
                request
            );
        }
    #endif
#else
        public Task<Result.SubMoldCapacityResult> SubMoldCapacityAsync(
            Request.SubMoldCapacityRequest request
        ) =>
            new SubMoldCapacityTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsTask();
#endif


        public class SubMoldCapacityByUserIdTask : Gs2WebSocketSessionTask<Request.SubMoldCapacityByUserIdRequest, Result.SubMoldCapacityByUserIdResult>
        {
            public SubMoldCapacityByUserIdTask(IGs2Session session, Request.SubMoldCapacityByUserIdRequest request) : base(session, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(Request.SubMoldCapacityByUserIdRequest request)
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
                if (request.MoldModelName != null)
                {
                    jsonWriter.WritePropertyName("moldModelName");
                    jsonWriter.Write(request.MoldModelName.ToString());
                }
                if (request.Capacity != null)
                {
                    jsonWriter.WritePropertyName("capacity");
                    jsonWriter.Write(request.Capacity.ToString());
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
                    "formation",
                    "mold",
                    "subMoldCapacityByUserId",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
        public IEnumerator SubMoldCapacityByUserId(
                Request.SubMoldCapacityByUserIdRequest request,
                UnityAction<AsyncResult<Result.SubMoldCapacityByUserIdResult>> callback
        ) =>
            new SubMoldCapacityByUserIdTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToCoroutine(callback);

        public IFuture<Result.SubMoldCapacityByUserIdResult> SubMoldCapacityByUserIdFuture(
                Request.SubMoldCapacityByUserIdRequest request
        ) =>
            new SubMoldCapacityByUserIdTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
        public UniTask<Result.SubMoldCapacityByUserIdResult> SubMoldCapacityByUserIdAsync(
            Request.SubMoldCapacityByUserIdRequest request
        ) =>
            new SubMoldCapacityByUserIdTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsUniTask<Result.SubMoldCapacityByUserIdResult>();
    #else
        public SubMoldCapacityByUserIdTask SubMoldCapacityByUserIdAsync(
                Request.SubMoldCapacityByUserIdRequest request
        )
        {
            return new SubMoldCapacityByUserIdTask(
                Gs2WebSocketSession,
                request
            );
        }
    #endif
#else
        public Task<Result.SubMoldCapacityByUserIdResult> SubMoldCapacityByUserIdAsync(
            Request.SubMoldCapacityByUserIdRequest request
        ) =>
            new SubMoldCapacityByUserIdTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsTask();
#endif


        public class DeleteMoldTask : Gs2WebSocketSessionTask<Request.DeleteMoldRequest, Result.DeleteMoldResult>
        {
            public DeleteMoldTask(IGs2Session session, Request.DeleteMoldRequest request) : base(session, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(Request.DeleteMoldRequest request)
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
                if (request.MoldModelName != null)
                {
                    jsonWriter.WritePropertyName("moldModelName");
                    jsonWriter.Write(request.MoldModelName.ToString());
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
                    "formation",
                    "mold",
                    "deleteMold",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
        public IEnumerator DeleteMold(
                Request.DeleteMoldRequest request,
                UnityAction<AsyncResult<Result.DeleteMoldResult>> callback
        ) =>
            new DeleteMoldTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToCoroutine(callback);

        public IFuture<Result.DeleteMoldResult> DeleteMoldFuture(
                Request.DeleteMoldRequest request
        ) =>
            new DeleteMoldTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
        public UniTask<Result.DeleteMoldResult> DeleteMoldAsync(
            Request.DeleteMoldRequest request
        ) =>
            new DeleteMoldTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsUniTask<Result.DeleteMoldResult>();
    #else
        public DeleteMoldTask DeleteMoldAsync(
                Request.DeleteMoldRequest request
        )
        {
            return new DeleteMoldTask(
                Gs2WebSocketSession,
                request
            );
        }
    #endif
#else
        public Task<Result.DeleteMoldResult> DeleteMoldAsync(
            Request.DeleteMoldRequest request
        ) =>
            new DeleteMoldTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsTask();
#endif


        public class DeleteMoldByUserIdTask : Gs2WebSocketSessionTask<Request.DeleteMoldByUserIdRequest, Result.DeleteMoldByUserIdResult>
        {
            public DeleteMoldByUserIdTask(IGs2Session session, Request.DeleteMoldByUserIdRequest request) : base(session, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(Request.DeleteMoldByUserIdRequest request)
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
                if (request.MoldModelName != null)
                {
                    jsonWriter.WritePropertyName("moldModelName");
                    jsonWriter.Write(request.MoldModelName.ToString());
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
                    "formation",
                    "mold",
                    "deleteMoldByUserId",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
        public IEnumerator DeleteMoldByUserId(
                Request.DeleteMoldByUserIdRequest request,
                UnityAction<AsyncResult<Result.DeleteMoldByUserIdResult>> callback
        ) =>
            new DeleteMoldByUserIdTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToCoroutine(callback);

        public IFuture<Result.DeleteMoldByUserIdResult> DeleteMoldByUserIdFuture(
                Request.DeleteMoldByUserIdRequest request
        ) =>
            new DeleteMoldByUserIdTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
        public UniTask<Result.DeleteMoldByUserIdResult> DeleteMoldByUserIdAsync(
            Request.DeleteMoldByUserIdRequest request
        ) =>
            new DeleteMoldByUserIdTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsUniTask<Result.DeleteMoldByUserIdResult>();
    #else
        public DeleteMoldByUserIdTask DeleteMoldByUserIdAsync(
                Request.DeleteMoldByUserIdRequest request
        )
        {
            return new DeleteMoldByUserIdTask(
                Gs2WebSocketSession,
                request
            );
        }
    #endif
#else
        public Task<Result.DeleteMoldByUserIdResult> DeleteMoldByUserIdAsync(
            Request.DeleteMoldByUserIdRequest request
        ) =>
            new DeleteMoldByUserIdTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsTask();
#endif


        public class AddCapacityByStampSheetTask : Gs2WebSocketSessionTask<Request.AddCapacityByStampSheetRequest, Result.AddCapacityByStampSheetResult>
        {
            public AddCapacityByStampSheetTask(IGs2Session session, Request.AddCapacityByStampSheetRequest request) : base(session, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(Request.AddCapacityByStampSheetRequest request)
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
                    "formation",
                    "mold",
                    "addCapacityByStampSheet",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
        public IEnumerator AddCapacityByStampSheet(
                Request.AddCapacityByStampSheetRequest request,
                UnityAction<AsyncResult<Result.AddCapacityByStampSheetResult>> callback
        ) =>
            new AddCapacityByStampSheetTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToCoroutine(callback);

        public IFuture<Result.AddCapacityByStampSheetResult> AddCapacityByStampSheetFuture(
                Request.AddCapacityByStampSheetRequest request
        ) =>
            new AddCapacityByStampSheetTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
        public UniTask<Result.AddCapacityByStampSheetResult> AddCapacityByStampSheetAsync(
            Request.AddCapacityByStampSheetRequest request
        ) =>
            new AddCapacityByStampSheetTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsUniTask<Result.AddCapacityByStampSheetResult>();
    #else
        public AddCapacityByStampSheetTask AddCapacityByStampSheetAsync(
                Request.AddCapacityByStampSheetRequest request
        )
        {
            return new AddCapacityByStampSheetTask(
                Gs2WebSocketSession,
                request
            );
        }
    #endif
#else
        public Task<Result.AddCapacityByStampSheetResult> AddCapacityByStampSheetAsync(
            Request.AddCapacityByStampSheetRequest request
        ) =>
            new AddCapacityByStampSheetTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsTask();
#endif


        public class SetCapacityByStampSheetTask : Gs2WebSocketSessionTask<Request.SetCapacityByStampSheetRequest, Result.SetCapacityByStampSheetResult>
        {
            public SetCapacityByStampSheetTask(IGs2Session session, Request.SetCapacityByStampSheetRequest request) : base(session, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(Request.SetCapacityByStampSheetRequest request)
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
                    "formation",
                    "mold",
                    "setCapacityByStampSheet",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
        public IEnumerator SetCapacityByStampSheet(
                Request.SetCapacityByStampSheetRequest request,
                UnityAction<AsyncResult<Result.SetCapacityByStampSheetResult>> callback
        ) =>
            new SetCapacityByStampSheetTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToCoroutine(callback);

        public IFuture<Result.SetCapacityByStampSheetResult> SetCapacityByStampSheetFuture(
                Request.SetCapacityByStampSheetRequest request
        ) =>
            new SetCapacityByStampSheetTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
        public UniTask<Result.SetCapacityByStampSheetResult> SetCapacityByStampSheetAsync(
            Request.SetCapacityByStampSheetRequest request
        ) =>
            new SetCapacityByStampSheetTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsUniTask<Result.SetCapacityByStampSheetResult>();
    #else
        public SetCapacityByStampSheetTask SetCapacityByStampSheetAsync(
                Request.SetCapacityByStampSheetRequest request
        )
        {
            return new SetCapacityByStampSheetTask(
                Gs2WebSocketSession,
                request
            );
        }
    #endif
#else
        public Task<Result.SetCapacityByStampSheetResult> SetCapacityByStampSheetAsync(
            Request.SetCapacityByStampSheetRequest request
        ) =>
            new SetCapacityByStampSheetTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsTask();
#endif
	}
}