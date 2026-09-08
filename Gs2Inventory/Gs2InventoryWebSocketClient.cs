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

namespace Gs2.Gs2Inventory
{
	public class Gs2InventoryWebSocketClient : AbstractGs2Client
	{

		public static string Endpoint = "inventory";

        protected Gs2WebSocketSession Gs2WebSocketSession => (Gs2WebSocketSession) Gs2Session;

		public Gs2InventoryWebSocketClient(Gs2WebSocketSession Gs2WebSocketSession) : base(Gs2WebSocketSession)
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
                if (request.TransactionSettingV2 != null)
                {
                    jsonWriter.WritePropertyName("transactionSettingV2");
                    request.TransactionSettingV2.WriteJson(jsonWriter);
                }
                if (request.AcquireScript != null)
                {
                    jsonWriter.WritePropertyName("acquireScript");
                    request.AcquireScript.WriteJson(jsonWriter);
                }
                if (request.OverflowScript != null)
                {
                    jsonWriter.WritePropertyName("overflowScript");
                    request.OverflowScript.WriteJson(jsonWriter);
                }
                if (request.ConsumeScript != null)
                {
                    jsonWriter.WritePropertyName("consumeScript");
                    request.ConsumeScript.WriteJson(jsonWriter);
                }
                if (request.SimpleItemAcquireScript != null)
                {
                    jsonWriter.WritePropertyName("simpleItemAcquireScript");
                    request.SimpleItemAcquireScript.WriteJson(jsonWriter);
                }
                if (request.SimpleItemConsumeScript != null)
                {
                    jsonWriter.WritePropertyName("simpleItemConsumeScript");
                    request.SimpleItemConsumeScript.WriteJson(jsonWriter);
                }
                if (request.BigItemAcquireScript != null)
                {
                    jsonWriter.WritePropertyName("bigItemAcquireScript");
                    request.BigItemAcquireScript.WriteJson(jsonWriter);
                }
                if (request.BigItemConsumeScript != null)
                {
                    jsonWriter.WritePropertyName("bigItemConsumeScript");
                    request.BigItemConsumeScript.WriteJson(jsonWriter);
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
                    "inventory",
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
                    "inventory",
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
                    "inventory",
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
                if (request.TransactionSettingV2 != null)
                {
                    jsonWriter.WritePropertyName("transactionSettingV2");
                    request.TransactionSettingV2.WriteJson(jsonWriter);
                }
                if (request.AcquireScript != null)
                {
                    jsonWriter.WritePropertyName("acquireScript");
                    request.AcquireScript.WriteJson(jsonWriter);
                }
                if (request.OverflowScript != null)
                {
                    jsonWriter.WritePropertyName("overflowScript");
                    request.OverflowScript.WriteJson(jsonWriter);
                }
                if (request.ConsumeScript != null)
                {
                    jsonWriter.WritePropertyName("consumeScript");
                    request.ConsumeScript.WriteJson(jsonWriter);
                }
                if (request.SimpleItemAcquireScript != null)
                {
                    jsonWriter.WritePropertyName("simpleItemAcquireScript");
                    request.SimpleItemAcquireScript.WriteJson(jsonWriter);
                }
                if (request.SimpleItemConsumeScript != null)
                {
                    jsonWriter.WritePropertyName("simpleItemConsumeScript");
                    request.SimpleItemConsumeScript.WriteJson(jsonWriter);
                }
                if (request.BigItemAcquireScript != null)
                {
                    jsonWriter.WritePropertyName("bigItemAcquireScript");
                    request.BigItemAcquireScript.WriteJson(jsonWriter);
                }
                if (request.BigItemConsumeScript != null)
                {
                    jsonWriter.WritePropertyName("bigItemConsumeScript");
                    request.BigItemConsumeScript.WriteJson(jsonWriter);
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
                    "inventory",
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
                    "inventory",
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
                    "inventory",
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
                    "inventory",
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
                    "inventory",
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
                    "inventory",
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
                    "inventory",
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
                    "inventory",
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
                    "inventory",
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
                    "inventory",
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


        public class CreateInventoryModelMasterTask : Gs2WebSocketSessionTask<Request.CreateInventoryModelMasterRequest, Result.CreateInventoryModelMasterResult>
        {
            public CreateInventoryModelMasterTask(IGs2Session session, Request.CreateInventoryModelMasterRequest request) : base(session, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(Request.CreateInventoryModelMasterRequest request)
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
                if (request.InitialCapacity != null)
                {
                    jsonWriter.WritePropertyName("initialCapacity");
                    jsonWriter.Write(request.InitialCapacity.ToString());
                }
                if (request.MaxCapacity != null)
                {
                    jsonWriter.WritePropertyName("maxCapacity");
                    jsonWriter.Write(request.MaxCapacity.ToString());
                }
                if (request.ProtectReferencedItem != null)
                {
                    jsonWriter.WritePropertyName("protectReferencedItem");
                    jsonWriter.Write(request.ProtectReferencedItem.ToString());
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
                    "inventory",
                    "inventoryModelMaster",
                    "createInventoryModelMaster",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
        public IEnumerator CreateInventoryModelMaster(
                Request.CreateInventoryModelMasterRequest request,
                UnityAction<AsyncResult<Result.CreateInventoryModelMasterResult>> callback
        ) =>
            new CreateInventoryModelMasterTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToCoroutine(callback);

        public IFuture<Result.CreateInventoryModelMasterResult> CreateInventoryModelMasterFuture(
                Request.CreateInventoryModelMasterRequest request
        ) =>
            new CreateInventoryModelMasterTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
        public UniTask<Result.CreateInventoryModelMasterResult> CreateInventoryModelMasterAsync(
            Request.CreateInventoryModelMasterRequest request
        ) =>
            new CreateInventoryModelMasterTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsUniTask<Result.CreateInventoryModelMasterResult>();
    #else
        public CreateInventoryModelMasterTask CreateInventoryModelMasterAsync(
                Request.CreateInventoryModelMasterRequest request
        )
        {
            return new CreateInventoryModelMasterTask(
                Gs2WebSocketSession,
                request
            );
        }
    #endif
#else
        public Task<Result.CreateInventoryModelMasterResult> CreateInventoryModelMasterAsync(
            Request.CreateInventoryModelMasterRequest request
        ) =>
            new CreateInventoryModelMasterTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsTask();
#endif


        public class GetInventoryModelMasterTask : Gs2WebSocketSessionTask<Request.GetInventoryModelMasterRequest, Result.GetInventoryModelMasterResult>
        {
            public GetInventoryModelMasterTask(IGs2Session session, Request.GetInventoryModelMasterRequest request) : base(session, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(Request.GetInventoryModelMasterRequest request)
            {
                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);

                jsonWriter.WriteObjectStart();

                if (request.NamespaceName != null)
                {
                    jsonWriter.WritePropertyName("namespaceName");
                    jsonWriter.Write(request.NamespaceName.ToString());
                }
                if (request.InventoryName != null)
                {
                    jsonWriter.WritePropertyName("inventoryName");
                    jsonWriter.Write(request.InventoryName.ToString());
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
                    "inventory",
                    "inventoryModelMaster",
                    "getInventoryModelMaster",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
        public IEnumerator GetInventoryModelMaster(
                Request.GetInventoryModelMasterRequest request,
                UnityAction<AsyncResult<Result.GetInventoryModelMasterResult>> callback
        ) =>
            new GetInventoryModelMasterTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToCoroutine(callback);

        public IFuture<Result.GetInventoryModelMasterResult> GetInventoryModelMasterFuture(
                Request.GetInventoryModelMasterRequest request
        ) =>
            new GetInventoryModelMasterTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
        public UniTask<Result.GetInventoryModelMasterResult> GetInventoryModelMasterAsync(
            Request.GetInventoryModelMasterRequest request
        ) =>
            new GetInventoryModelMasterTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsUniTask<Result.GetInventoryModelMasterResult>();
    #else
        public GetInventoryModelMasterTask GetInventoryModelMasterAsync(
                Request.GetInventoryModelMasterRequest request
        )
        {
            return new GetInventoryModelMasterTask(
                Gs2WebSocketSession,
                request
            );
        }
    #endif
#else
        public Task<Result.GetInventoryModelMasterResult> GetInventoryModelMasterAsync(
            Request.GetInventoryModelMasterRequest request
        ) =>
            new GetInventoryModelMasterTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsTask();
#endif


        public class UpdateInventoryModelMasterTask : Gs2WebSocketSessionTask<Request.UpdateInventoryModelMasterRequest, Result.UpdateInventoryModelMasterResult>
        {
            public UpdateInventoryModelMasterTask(IGs2Session session, Request.UpdateInventoryModelMasterRequest request) : base(session, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(Request.UpdateInventoryModelMasterRequest request)
            {
                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);

                jsonWriter.WriteObjectStart();

                if (request.NamespaceName != null)
                {
                    jsonWriter.WritePropertyName("namespaceName");
                    jsonWriter.Write(request.NamespaceName.ToString());
                }
                if (request.InventoryName != null)
                {
                    jsonWriter.WritePropertyName("inventoryName");
                    jsonWriter.Write(request.InventoryName.ToString());
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
                if (request.InitialCapacity != null)
                {
                    jsonWriter.WritePropertyName("initialCapacity");
                    jsonWriter.Write(request.InitialCapacity.ToString());
                }
                if (request.MaxCapacity != null)
                {
                    jsonWriter.WritePropertyName("maxCapacity");
                    jsonWriter.Write(request.MaxCapacity.ToString());
                }
                if (request.ProtectReferencedItem != null)
                {
                    jsonWriter.WritePropertyName("protectReferencedItem");
                    jsonWriter.Write(request.ProtectReferencedItem.ToString());
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
                    "inventory",
                    "inventoryModelMaster",
                    "updateInventoryModelMaster",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
        public IEnumerator UpdateInventoryModelMaster(
                Request.UpdateInventoryModelMasterRequest request,
                UnityAction<AsyncResult<Result.UpdateInventoryModelMasterResult>> callback
        ) =>
            new UpdateInventoryModelMasterTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToCoroutine(callback);

        public IFuture<Result.UpdateInventoryModelMasterResult> UpdateInventoryModelMasterFuture(
                Request.UpdateInventoryModelMasterRequest request
        ) =>
            new UpdateInventoryModelMasterTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
        public UniTask<Result.UpdateInventoryModelMasterResult> UpdateInventoryModelMasterAsync(
            Request.UpdateInventoryModelMasterRequest request
        ) =>
            new UpdateInventoryModelMasterTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsUniTask<Result.UpdateInventoryModelMasterResult>();
    #else
        public UpdateInventoryModelMasterTask UpdateInventoryModelMasterAsync(
                Request.UpdateInventoryModelMasterRequest request
        )
        {
            return new UpdateInventoryModelMasterTask(
                Gs2WebSocketSession,
                request
            );
        }
    #endif
#else
        public Task<Result.UpdateInventoryModelMasterResult> UpdateInventoryModelMasterAsync(
            Request.UpdateInventoryModelMasterRequest request
        ) =>
            new UpdateInventoryModelMasterTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsTask();
#endif


        public class DeleteInventoryModelMasterTask : Gs2WebSocketSessionTask<Request.DeleteInventoryModelMasterRequest, Result.DeleteInventoryModelMasterResult>
        {
            public DeleteInventoryModelMasterTask(IGs2Session session, Request.DeleteInventoryModelMasterRequest request) : base(session, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(Request.DeleteInventoryModelMasterRequest request)
            {
                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);

                jsonWriter.WriteObjectStart();

                if (request.NamespaceName != null)
                {
                    jsonWriter.WritePropertyName("namespaceName");
                    jsonWriter.Write(request.NamespaceName.ToString());
                }
                if (request.InventoryName != null)
                {
                    jsonWriter.WritePropertyName("inventoryName");
                    jsonWriter.Write(request.InventoryName.ToString());
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
                    "inventory",
                    "inventoryModelMaster",
                    "deleteInventoryModelMaster",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
        public IEnumerator DeleteInventoryModelMaster(
                Request.DeleteInventoryModelMasterRequest request,
                UnityAction<AsyncResult<Result.DeleteInventoryModelMasterResult>> callback
        ) =>
            new DeleteInventoryModelMasterTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToCoroutine(callback);

        public IFuture<Result.DeleteInventoryModelMasterResult> DeleteInventoryModelMasterFuture(
                Request.DeleteInventoryModelMasterRequest request
        ) =>
            new DeleteInventoryModelMasterTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
        public UniTask<Result.DeleteInventoryModelMasterResult> DeleteInventoryModelMasterAsync(
            Request.DeleteInventoryModelMasterRequest request
        ) =>
            new DeleteInventoryModelMasterTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsUniTask<Result.DeleteInventoryModelMasterResult>();
    #else
        public DeleteInventoryModelMasterTask DeleteInventoryModelMasterAsync(
                Request.DeleteInventoryModelMasterRequest request
        )
        {
            return new DeleteInventoryModelMasterTask(
                Gs2WebSocketSession,
                request
            );
        }
    #endif
#else
        public Task<Result.DeleteInventoryModelMasterResult> DeleteInventoryModelMasterAsync(
            Request.DeleteInventoryModelMasterRequest request
        ) =>
            new DeleteInventoryModelMasterTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsTask();
#endif


        public class CreateItemModelMasterTask : Gs2WebSocketSessionTask<Request.CreateItemModelMasterRequest, Result.CreateItemModelMasterResult>
        {
            public CreateItemModelMasterTask(IGs2Session session, Request.CreateItemModelMasterRequest request) : base(session, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(Request.CreateItemModelMasterRequest request)
            {
                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);

                jsonWriter.WriteObjectStart();

                if (request.NamespaceName != null)
                {
                    jsonWriter.WritePropertyName("namespaceName");
                    jsonWriter.Write(request.NamespaceName.ToString());
                }
                if (request.InventoryName != null)
                {
                    jsonWriter.WritePropertyName("inventoryName");
                    jsonWriter.Write(request.InventoryName.ToString());
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
                if (request.StackingLimit != null)
                {
                    jsonWriter.WritePropertyName("stackingLimit");
                    jsonWriter.Write(request.StackingLimit.ToString());
                }
                if (request.AllowMultipleStacks != null)
                {
                    jsonWriter.WritePropertyName("allowMultipleStacks");
                    jsonWriter.Write(request.AllowMultipleStacks.ToString());
                }
                if (request.SortValue != null)
                {
                    jsonWriter.WritePropertyName("sortValue");
                    jsonWriter.Write(request.SortValue.ToString());
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
                    "inventory",
                    "itemModelMaster",
                    "createItemModelMaster",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
        public IEnumerator CreateItemModelMaster(
                Request.CreateItemModelMasterRequest request,
                UnityAction<AsyncResult<Result.CreateItemModelMasterResult>> callback
        ) =>
            new CreateItemModelMasterTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToCoroutine(callback);

        public IFuture<Result.CreateItemModelMasterResult> CreateItemModelMasterFuture(
                Request.CreateItemModelMasterRequest request
        ) =>
            new CreateItemModelMasterTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
        public UniTask<Result.CreateItemModelMasterResult> CreateItemModelMasterAsync(
            Request.CreateItemModelMasterRequest request
        ) =>
            new CreateItemModelMasterTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsUniTask<Result.CreateItemModelMasterResult>();
    #else
        public CreateItemModelMasterTask CreateItemModelMasterAsync(
                Request.CreateItemModelMasterRequest request
        )
        {
            return new CreateItemModelMasterTask(
                Gs2WebSocketSession,
                request
            );
        }
    #endif
#else
        public Task<Result.CreateItemModelMasterResult> CreateItemModelMasterAsync(
            Request.CreateItemModelMasterRequest request
        ) =>
            new CreateItemModelMasterTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsTask();
#endif


        public class GetItemModelMasterTask : Gs2WebSocketSessionTask<Request.GetItemModelMasterRequest, Result.GetItemModelMasterResult>
        {
            public GetItemModelMasterTask(IGs2Session session, Request.GetItemModelMasterRequest request) : base(session, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(Request.GetItemModelMasterRequest request)
            {
                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);

                jsonWriter.WriteObjectStart();

                if (request.NamespaceName != null)
                {
                    jsonWriter.WritePropertyName("namespaceName");
                    jsonWriter.Write(request.NamespaceName.ToString());
                }
                if (request.InventoryName != null)
                {
                    jsonWriter.WritePropertyName("inventoryName");
                    jsonWriter.Write(request.InventoryName.ToString());
                }
                if (request.ItemName != null)
                {
                    jsonWriter.WritePropertyName("itemName");
                    jsonWriter.Write(request.ItemName.ToString());
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
                    "inventory",
                    "itemModelMaster",
                    "getItemModelMaster",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
        public IEnumerator GetItemModelMaster(
                Request.GetItemModelMasterRequest request,
                UnityAction<AsyncResult<Result.GetItemModelMasterResult>> callback
        ) =>
            new GetItemModelMasterTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToCoroutine(callback);

        public IFuture<Result.GetItemModelMasterResult> GetItemModelMasterFuture(
                Request.GetItemModelMasterRequest request
        ) =>
            new GetItemModelMasterTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
        public UniTask<Result.GetItemModelMasterResult> GetItemModelMasterAsync(
            Request.GetItemModelMasterRequest request
        ) =>
            new GetItemModelMasterTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsUniTask<Result.GetItemModelMasterResult>();
    #else
        public GetItemModelMasterTask GetItemModelMasterAsync(
                Request.GetItemModelMasterRequest request
        )
        {
            return new GetItemModelMasterTask(
                Gs2WebSocketSession,
                request
            );
        }
    #endif
#else
        public Task<Result.GetItemModelMasterResult> GetItemModelMasterAsync(
            Request.GetItemModelMasterRequest request
        ) =>
            new GetItemModelMasterTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsTask();
#endif


        public class UpdateItemModelMasterTask : Gs2WebSocketSessionTask<Request.UpdateItemModelMasterRequest, Result.UpdateItemModelMasterResult>
        {
            public UpdateItemModelMasterTask(IGs2Session session, Request.UpdateItemModelMasterRequest request) : base(session, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(Request.UpdateItemModelMasterRequest request)
            {
                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);

                jsonWriter.WriteObjectStart();

                if (request.NamespaceName != null)
                {
                    jsonWriter.WritePropertyName("namespaceName");
                    jsonWriter.Write(request.NamespaceName.ToString());
                }
                if (request.InventoryName != null)
                {
                    jsonWriter.WritePropertyName("inventoryName");
                    jsonWriter.Write(request.InventoryName.ToString());
                }
                if (request.ItemName != null)
                {
                    jsonWriter.WritePropertyName("itemName");
                    jsonWriter.Write(request.ItemName.ToString());
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
                if (request.StackingLimit != null)
                {
                    jsonWriter.WritePropertyName("stackingLimit");
                    jsonWriter.Write(request.StackingLimit.ToString());
                }
                if (request.AllowMultipleStacks != null)
                {
                    jsonWriter.WritePropertyName("allowMultipleStacks");
                    jsonWriter.Write(request.AllowMultipleStacks.ToString());
                }
                if (request.SortValue != null)
                {
                    jsonWriter.WritePropertyName("sortValue");
                    jsonWriter.Write(request.SortValue.ToString());
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
                    "inventory",
                    "itemModelMaster",
                    "updateItemModelMaster",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
        public IEnumerator UpdateItemModelMaster(
                Request.UpdateItemModelMasterRequest request,
                UnityAction<AsyncResult<Result.UpdateItemModelMasterResult>> callback
        ) =>
            new UpdateItemModelMasterTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToCoroutine(callback);

        public IFuture<Result.UpdateItemModelMasterResult> UpdateItemModelMasterFuture(
                Request.UpdateItemModelMasterRequest request
        ) =>
            new UpdateItemModelMasterTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
        public UniTask<Result.UpdateItemModelMasterResult> UpdateItemModelMasterAsync(
            Request.UpdateItemModelMasterRequest request
        ) =>
            new UpdateItemModelMasterTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsUniTask<Result.UpdateItemModelMasterResult>();
    #else
        public UpdateItemModelMasterTask UpdateItemModelMasterAsync(
                Request.UpdateItemModelMasterRequest request
        )
        {
            return new UpdateItemModelMasterTask(
                Gs2WebSocketSession,
                request
            );
        }
    #endif
#else
        public Task<Result.UpdateItemModelMasterResult> UpdateItemModelMasterAsync(
            Request.UpdateItemModelMasterRequest request
        ) =>
            new UpdateItemModelMasterTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsTask();
#endif


        public class DeleteItemModelMasterTask : Gs2WebSocketSessionTask<Request.DeleteItemModelMasterRequest, Result.DeleteItemModelMasterResult>
        {
            public DeleteItemModelMasterTask(IGs2Session session, Request.DeleteItemModelMasterRequest request) : base(session, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(Request.DeleteItemModelMasterRequest request)
            {
                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);

                jsonWriter.WriteObjectStart();

                if (request.NamespaceName != null)
                {
                    jsonWriter.WritePropertyName("namespaceName");
                    jsonWriter.Write(request.NamespaceName.ToString());
                }
                if (request.InventoryName != null)
                {
                    jsonWriter.WritePropertyName("inventoryName");
                    jsonWriter.Write(request.InventoryName.ToString());
                }
                if (request.ItemName != null)
                {
                    jsonWriter.WritePropertyName("itemName");
                    jsonWriter.Write(request.ItemName.ToString());
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
                    "inventory",
                    "itemModelMaster",
                    "deleteItemModelMaster",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
        public IEnumerator DeleteItemModelMaster(
                Request.DeleteItemModelMasterRequest request,
                UnityAction<AsyncResult<Result.DeleteItemModelMasterResult>> callback
        ) =>
            new DeleteItemModelMasterTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToCoroutine(callback);

        public IFuture<Result.DeleteItemModelMasterResult> DeleteItemModelMasterFuture(
                Request.DeleteItemModelMasterRequest request
        ) =>
            new DeleteItemModelMasterTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
        public UniTask<Result.DeleteItemModelMasterResult> DeleteItemModelMasterAsync(
            Request.DeleteItemModelMasterRequest request
        ) =>
            new DeleteItemModelMasterTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsUniTask<Result.DeleteItemModelMasterResult>();
    #else
        public DeleteItemModelMasterTask DeleteItemModelMasterAsync(
                Request.DeleteItemModelMasterRequest request
        )
        {
            return new DeleteItemModelMasterTask(
                Gs2WebSocketSession,
                request
            );
        }
    #endif
#else
        public Task<Result.DeleteItemModelMasterResult> DeleteItemModelMasterAsync(
            Request.DeleteItemModelMasterRequest request
        ) =>
            new DeleteItemModelMasterTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsTask();
#endif


        public class GetItemModelTask : Gs2WebSocketSessionTask<Request.GetItemModelRequest, Result.GetItemModelResult>
        {
            public GetItemModelTask(IGs2Session session, Request.GetItemModelRequest request) : base(session, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(Request.GetItemModelRequest request)
            {
                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);

                jsonWriter.WriteObjectStart();

                if (request.NamespaceName != null)
                {
                    jsonWriter.WritePropertyName("namespaceName");
                    jsonWriter.Write(request.NamespaceName.ToString());
                }
                if (request.InventoryName != null)
                {
                    jsonWriter.WritePropertyName("inventoryName");
                    jsonWriter.Write(request.InventoryName.ToString());
                }
                if (request.ItemName != null)
                {
                    jsonWriter.WritePropertyName("itemName");
                    jsonWriter.Write(request.ItemName.ToString());
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
                    "inventory",
                    "itemModel",
                    "getItemModel",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
        public IEnumerator GetItemModel(
                Request.GetItemModelRequest request,
                UnityAction<AsyncResult<Result.GetItemModelResult>> callback
        ) =>
            new GetItemModelTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToCoroutine(callback);

        public IFuture<Result.GetItemModelResult> GetItemModelFuture(
                Request.GetItemModelRequest request
        ) =>
            new GetItemModelTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
        public UniTask<Result.GetItemModelResult> GetItemModelAsync(
            Request.GetItemModelRequest request
        ) =>
            new GetItemModelTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsUniTask<Result.GetItemModelResult>();
    #else
        public GetItemModelTask GetItemModelAsync(
                Request.GetItemModelRequest request
        )
        {
            return new GetItemModelTask(
                Gs2WebSocketSession,
                request
            );
        }
    #endif
#else
        public Task<Result.GetItemModelResult> GetItemModelAsync(
            Request.GetItemModelRequest request
        ) =>
            new GetItemModelTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsTask();
#endif


        public class CreateSimpleInventoryModelMasterTask : Gs2WebSocketSessionTask<Request.CreateSimpleInventoryModelMasterRequest, Result.CreateSimpleInventoryModelMasterResult>
        {
            public CreateSimpleInventoryModelMasterTask(IGs2Session session, Request.CreateSimpleInventoryModelMasterRequest request) : base(session, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(Request.CreateSimpleInventoryModelMasterRequest request)
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
                    "inventory",
                    "simpleInventoryModelMaster",
                    "createSimpleInventoryModelMaster",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
        public IEnumerator CreateSimpleInventoryModelMaster(
                Request.CreateSimpleInventoryModelMasterRequest request,
                UnityAction<AsyncResult<Result.CreateSimpleInventoryModelMasterResult>> callback
        ) =>
            new CreateSimpleInventoryModelMasterTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToCoroutine(callback);

        public IFuture<Result.CreateSimpleInventoryModelMasterResult> CreateSimpleInventoryModelMasterFuture(
                Request.CreateSimpleInventoryModelMasterRequest request
        ) =>
            new CreateSimpleInventoryModelMasterTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
        public UniTask<Result.CreateSimpleInventoryModelMasterResult> CreateSimpleInventoryModelMasterAsync(
            Request.CreateSimpleInventoryModelMasterRequest request
        ) =>
            new CreateSimpleInventoryModelMasterTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsUniTask<Result.CreateSimpleInventoryModelMasterResult>();
    #else
        public CreateSimpleInventoryModelMasterTask CreateSimpleInventoryModelMasterAsync(
                Request.CreateSimpleInventoryModelMasterRequest request
        )
        {
            return new CreateSimpleInventoryModelMasterTask(
                Gs2WebSocketSession,
                request
            );
        }
    #endif
#else
        public Task<Result.CreateSimpleInventoryModelMasterResult> CreateSimpleInventoryModelMasterAsync(
            Request.CreateSimpleInventoryModelMasterRequest request
        ) =>
            new CreateSimpleInventoryModelMasterTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsTask();
#endif


        public class GetSimpleInventoryModelMasterTask : Gs2WebSocketSessionTask<Request.GetSimpleInventoryModelMasterRequest, Result.GetSimpleInventoryModelMasterResult>
        {
            public GetSimpleInventoryModelMasterTask(IGs2Session session, Request.GetSimpleInventoryModelMasterRequest request) : base(session, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(Request.GetSimpleInventoryModelMasterRequest request)
            {
                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);

                jsonWriter.WriteObjectStart();

                if (request.NamespaceName != null)
                {
                    jsonWriter.WritePropertyName("namespaceName");
                    jsonWriter.Write(request.NamespaceName.ToString());
                }
                if (request.InventoryName != null)
                {
                    jsonWriter.WritePropertyName("inventoryName");
                    jsonWriter.Write(request.InventoryName.ToString());
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
                    "inventory",
                    "simpleInventoryModelMaster",
                    "getSimpleInventoryModelMaster",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
        public IEnumerator GetSimpleInventoryModelMaster(
                Request.GetSimpleInventoryModelMasterRequest request,
                UnityAction<AsyncResult<Result.GetSimpleInventoryModelMasterResult>> callback
        ) =>
            new GetSimpleInventoryModelMasterTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToCoroutine(callback);

        public IFuture<Result.GetSimpleInventoryModelMasterResult> GetSimpleInventoryModelMasterFuture(
                Request.GetSimpleInventoryModelMasterRequest request
        ) =>
            new GetSimpleInventoryModelMasterTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
        public UniTask<Result.GetSimpleInventoryModelMasterResult> GetSimpleInventoryModelMasterAsync(
            Request.GetSimpleInventoryModelMasterRequest request
        ) =>
            new GetSimpleInventoryModelMasterTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsUniTask<Result.GetSimpleInventoryModelMasterResult>();
    #else
        public GetSimpleInventoryModelMasterTask GetSimpleInventoryModelMasterAsync(
                Request.GetSimpleInventoryModelMasterRequest request
        )
        {
            return new GetSimpleInventoryModelMasterTask(
                Gs2WebSocketSession,
                request
            );
        }
    #endif
#else
        public Task<Result.GetSimpleInventoryModelMasterResult> GetSimpleInventoryModelMasterAsync(
            Request.GetSimpleInventoryModelMasterRequest request
        ) =>
            new GetSimpleInventoryModelMasterTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsTask();
#endif


        public class UpdateSimpleInventoryModelMasterTask : Gs2WebSocketSessionTask<Request.UpdateSimpleInventoryModelMasterRequest, Result.UpdateSimpleInventoryModelMasterResult>
        {
            public UpdateSimpleInventoryModelMasterTask(IGs2Session session, Request.UpdateSimpleInventoryModelMasterRequest request) : base(session, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(Request.UpdateSimpleInventoryModelMasterRequest request)
            {
                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);

                jsonWriter.WriteObjectStart();

                if (request.NamespaceName != null)
                {
                    jsonWriter.WritePropertyName("namespaceName");
                    jsonWriter.Write(request.NamespaceName.ToString());
                }
                if (request.InventoryName != null)
                {
                    jsonWriter.WritePropertyName("inventoryName");
                    jsonWriter.Write(request.InventoryName.ToString());
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
                    "inventory",
                    "simpleInventoryModelMaster",
                    "updateSimpleInventoryModelMaster",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
        public IEnumerator UpdateSimpleInventoryModelMaster(
                Request.UpdateSimpleInventoryModelMasterRequest request,
                UnityAction<AsyncResult<Result.UpdateSimpleInventoryModelMasterResult>> callback
        ) =>
            new UpdateSimpleInventoryModelMasterTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToCoroutine(callback);

        public IFuture<Result.UpdateSimpleInventoryModelMasterResult> UpdateSimpleInventoryModelMasterFuture(
                Request.UpdateSimpleInventoryModelMasterRequest request
        ) =>
            new UpdateSimpleInventoryModelMasterTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
        public UniTask<Result.UpdateSimpleInventoryModelMasterResult> UpdateSimpleInventoryModelMasterAsync(
            Request.UpdateSimpleInventoryModelMasterRequest request
        ) =>
            new UpdateSimpleInventoryModelMasterTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsUniTask<Result.UpdateSimpleInventoryModelMasterResult>();
    #else
        public UpdateSimpleInventoryModelMasterTask UpdateSimpleInventoryModelMasterAsync(
                Request.UpdateSimpleInventoryModelMasterRequest request
        )
        {
            return new UpdateSimpleInventoryModelMasterTask(
                Gs2WebSocketSession,
                request
            );
        }
    #endif
#else
        public Task<Result.UpdateSimpleInventoryModelMasterResult> UpdateSimpleInventoryModelMasterAsync(
            Request.UpdateSimpleInventoryModelMasterRequest request
        ) =>
            new UpdateSimpleInventoryModelMasterTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsTask();
#endif


        public class DeleteSimpleInventoryModelMasterTask : Gs2WebSocketSessionTask<Request.DeleteSimpleInventoryModelMasterRequest, Result.DeleteSimpleInventoryModelMasterResult>
        {
            public DeleteSimpleInventoryModelMasterTask(IGs2Session session, Request.DeleteSimpleInventoryModelMasterRequest request) : base(session, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(Request.DeleteSimpleInventoryModelMasterRequest request)
            {
                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);

                jsonWriter.WriteObjectStart();

                if (request.NamespaceName != null)
                {
                    jsonWriter.WritePropertyName("namespaceName");
                    jsonWriter.Write(request.NamespaceName.ToString());
                }
                if (request.InventoryName != null)
                {
                    jsonWriter.WritePropertyName("inventoryName");
                    jsonWriter.Write(request.InventoryName.ToString());
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
                    "inventory",
                    "simpleInventoryModelMaster",
                    "deleteSimpleInventoryModelMaster",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
        public IEnumerator DeleteSimpleInventoryModelMaster(
                Request.DeleteSimpleInventoryModelMasterRequest request,
                UnityAction<AsyncResult<Result.DeleteSimpleInventoryModelMasterResult>> callback
        ) =>
            new DeleteSimpleInventoryModelMasterTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToCoroutine(callback);

        public IFuture<Result.DeleteSimpleInventoryModelMasterResult> DeleteSimpleInventoryModelMasterFuture(
                Request.DeleteSimpleInventoryModelMasterRequest request
        ) =>
            new DeleteSimpleInventoryModelMasterTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
        public UniTask<Result.DeleteSimpleInventoryModelMasterResult> DeleteSimpleInventoryModelMasterAsync(
            Request.DeleteSimpleInventoryModelMasterRequest request
        ) =>
            new DeleteSimpleInventoryModelMasterTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsUniTask<Result.DeleteSimpleInventoryModelMasterResult>();
    #else
        public DeleteSimpleInventoryModelMasterTask DeleteSimpleInventoryModelMasterAsync(
                Request.DeleteSimpleInventoryModelMasterRequest request
        )
        {
            return new DeleteSimpleInventoryModelMasterTask(
                Gs2WebSocketSession,
                request
            );
        }
    #endif
#else
        public Task<Result.DeleteSimpleInventoryModelMasterResult> DeleteSimpleInventoryModelMasterAsync(
            Request.DeleteSimpleInventoryModelMasterRequest request
        ) =>
            new DeleteSimpleInventoryModelMasterTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsTask();
#endif


        public class CreateSimpleItemModelMasterTask : Gs2WebSocketSessionTask<Request.CreateSimpleItemModelMasterRequest, Result.CreateSimpleItemModelMasterResult>
        {
            public CreateSimpleItemModelMasterTask(IGs2Session session, Request.CreateSimpleItemModelMasterRequest request) : base(session, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(Request.CreateSimpleItemModelMasterRequest request)
            {
                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);

                jsonWriter.WriteObjectStart();

                if (request.NamespaceName != null)
                {
                    jsonWriter.WritePropertyName("namespaceName");
                    jsonWriter.Write(request.NamespaceName.ToString());
                }
                if (request.InventoryName != null)
                {
                    jsonWriter.WritePropertyName("inventoryName");
                    jsonWriter.Write(request.InventoryName.ToString());
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
                    "inventory",
                    "simpleItemModelMaster",
                    "createSimpleItemModelMaster",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
        public IEnumerator CreateSimpleItemModelMaster(
                Request.CreateSimpleItemModelMasterRequest request,
                UnityAction<AsyncResult<Result.CreateSimpleItemModelMasterResult>> callback
        ) =>
            new CreateSimpleItemModelMasterTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToCoroutine(callback);

        public IFuture<Result.CreateSimpleItemModelMasterResult> CreateSimpleItemModelMasterFuture(
                Request.CreateSimpleItemModelMasterRequest request
        ) =>
            new CreateSimpleItemModelMasterTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
        public UniTask<Result.CreateSimpleItemModelMasterResult> CreateSimpleItemModelMasterAsync(
            Request.CreateSimpleItemModelMasterRequest request
        ) =>
            new CreateSimpleItemModelMasterTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsUniTask<Result.CreateSimpleItemModelMasterResult>();
    #else
        public CreateSimpleItemModelMasterTask CreateSimpleItemModelMasterAsync(
                Request.CreateSimpleItemModelMasterRequest request
        )
        {
            return new CreateSimpleItemModelMasterTask(
                Gs2WebSocketSession,
                request
            );
        }
    #endif
#else
        public Task<Result.CreateSimpleItemModelMasterResult> CreateSimpleItemModelMasterAsync(
            Request.CreateSimpleItemModelMasterRequest request
        ) =>
            new CreateSimpleItemModelMasterTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsTask();
#endif


        public class GetSimpleItemModelMasterTask : Gs2WebSocketSessionTask<Request.GetSimpleItemModelMasterRequest, Result.GetSimpleItemModelMasterResult>
        {
            public GetSimpleItemModelMasterTask(IGs2Session session, Request.GetSimpleItemModelMasterRequest request) : base(session, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(Request.GetSimpleItemModelMasterRequest request)
            {
                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);

                jsonWriter.WriteObjectStart();

                if (request.NamespaceName != null)
                {
                    jsonWriter.WritePropertyName("namespaceName");
                    jsonWriter.Write(request.NamespaceName.ToString());
                }
                if (request.InventoryName != null)
                {
                    jsonWriter.WritePropertyName("inventoryName");
                    jsonWriter.Write(request.InventoryName.ToString());
                }
                if (request.ItemName != null)
                {
                    jsonWriter.WritePropertyName("itemName");
                    jsonWriter.Write(request.ItemName.ToString());
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
                    "inventory",
                    "simpleItemModelMaster",
                    "getSimpleItemModelMaster",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
        public IEnumerator GetSimpleItemModelMaster(
                Request.GetSimpleItemModelMasterRequest request,
                UnityAction<AsyncResult<Result.GetSimpleItemModelMasterResult>> callback
        ) =>
            new GetSimpleItemModelMasterTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToCoroutine(callback);

        public IFuture<Result.GetSimpleItemModelMasterResult> GetSimpleItemModelMasterFuture(
                Request.GetSimpleItemModelMasterRequest request
        ) =>
            new GetSimpleItemModelMasterTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
        public UniTask<Result.GetSimpleItemModelMasterResult> GetSimpleItemModelMasterAsync(
            Request.GetSimpleItemModelMasterRequest request
        ) =>
            new GetSimpleItemModelMasterTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsUniTask<Result.GetSimpleItemModelMasterResult>();
    #else
        public GetSimpleItemModelMasterTask GetSimpleItemModelMasterAsync(
                Request.GetSimpleItemModelMasterRequest request
        )
        {
            return new GetSimpleItemModelMasterTask(
                Gs2WebSocketSession,
                request
            );
        }
    #endif
#else
        public Task<Result.GetSimpleItemModelMasterResult> GetSimpleItemModelMasterAsync(
            Request.GetSimpleItemModelMasterRequest request
        ) =>
            new GetSimpleItemModelMasterTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsTask();
#endif


        public class UpdateSimpleItemModelMasterTask : Gs2WebSocketSessionTask<Request.UpdateSimpleItemModelMasterRequest, Result.UpdateSimpleItemModelMasterResult>
        {
            public UpdateSimpleItemModelMasterTask(IGs2Session session, Request.UpdateSimpleItemModelMasterRequest request) : base(session, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(Request.UpdateSimpleItemModelMasterRequest request)
            {
                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);

                jsonWriter.WriteObjectStart();

                if (request.NamespaceName != null)
                {
                    jsonWriter.WritePropertyName("namespaceName");
                    jsonWriter.Write(request.NamespaceName.ToString());
                }
                if (request.InventoryName != null)
                {
                    jsonWriter.WritePropertyName("inventoryName");
                    jsonWriter.Write(request.InventoryName.ToString());
                }
                if (request.ItemName != null)
                {
                    jsonWriter.WritePropertyName("itemName");
                    jsonWriter.Write(request.ItemName.ToString());
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
                    "inventory",
                    "simpleItemModelMaster",
                    "updateSimpleItemModelMaster",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
        public IEnumerator UpdateSimpleItemModelMaster(
                Request.UpdateSimpleItemModelMasterRequest request,
                UnityAction<AsyncResult<Result.UpdateSimpleItemModelMasterResult>> callback
        ) =>
            new UpdateSimpleItemModelMasterTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToCoroutine(callback);

        public IFuture<Result.UpdateSimpleItemModelMasterResult> UpdateSimpleItemModelMasterFuture(
                Request.UpdateSimpleItemModelMasterRequest request
        ) =>
            new UpdateSimpleItemModelMasterTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
        public UniTask<Result.UpdateSimpleItemModelMasterResult> UpdateSimpleItemModelMasterAsync(
            Request.UpdateSimpleItemModelMasterRequest request
        ) =>
            new UpdateSimpleItemModelMasterTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsUniTask<Result.UpdateSimpleItemModelMasterResult>();
    #else
        public UpdateSimpleItemModelMasterTask UpdateSimpleItemModelMasterAsync(
                Request.UpdateSimpleItemModelMasterRequest request
        )
        {
            return new UpdateSimpleItemModelMasterTask(
                Gs2WebSocketSession,
                request
            );
        }
    #endif
#else
        public Task<Result.UpdateSimpleItemModelMasterResult> UpdateSimpleItemModelMasterAsync(
            Request.UpdateSimpleItemModelMasterRequest request
        ) =>
            new UpdateSimpleItemModelMasterTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsTask();
#endif


        public class DeleteSimpleItemModelMasterTask : Gs2WebSocketSessionTask<Request.DeleteSimpleItemModelMasterRequest, Result.DeleteSimpleItemModelMasterResult>
        {
            public DeleteSimpleItemModelMasterTask(IGs2Session session, Request.DeleteSimpleItemModelMasterRequest request) : base(session, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(Request.DeleteSimpleItemModelMasterRequest request)
            {
                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);

                jsonWriter.WriteObjectStart();

                if (request.NamespaceName != null)
                {
                    jsonWriter.WritePropertyName("namespaceName");
                    jsonWriter.Write(request.NamespaceName.ToString());
                }
                if (request.InventoryName != null)
                {
                    jsonWriter.WritePropertyName("inventoryName");
                    jsonWriter.Write(request.InventoryName.ToString());
                }
                if (request.ItemName != null)
                {
                    jsonWriter.WritePropertyName("itemName");
                    jsonWriter.Write(request.ItemName.ToString());
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
                    "inventory",
                    "simpleItemModelMaster",
                    "deleteSimpleItemModelMaster",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
        public IEnumerator DeleteSimpleItemModelMaster(
                Request.DeleteSimpleItemModelMasterRequest request,
                UnityAction<AsyncResult<Result.DeleteSimpleItemModelMasterResult>> callback
        ) =>
            new DeleteSimpleItemModelMasterTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToCoroutine(callback);

        public IFuture<Result.DeleteSimpleItemModelMasterResult> DeleteSimpleItemModelMasterFuture(
                Request.DeleteSimpleItemModelMasterRequest request
        ) =>
            new DeleteSimpleItemModelMasterTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
        public UniTask<Result.DeleteSimpleItemModelMasterResult> DeleteSimpleItemModelMasterAsync(
            Request.DeleteSimpleItemModelMasterRequest request
        ) =>
            new DeleteSimpleItemModelMasterTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsUniTask<Result.DeleteSimpleItemModelMasterResult>();
    #else
        public DeleteSimpleItemModelMasterTask DeleteSimpleItemModelMasterAsync(
                Request.DeleteSimpleItemModelMasterRequest request
        )
        {
            return new DeleteSimpleItemModelMasterTask(
                Gs2WebSocketSession,
                request
            );
        }
    #endif
#else
        public Task<Result.DeleteSimpleItemModelMasterResult> DeleteSimpleItemModelMasterAsync(
            Request.DeleteSimpleItemModelMasterRequest request
        ) =>
            new DeleteSimpleItemModelMasterTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsTask();
#endif


        public class GetSimpleItemModelTask : Gs2WebSocketSessionTask<Request.GetSimpleItemModelRequest, Result.GetSimpleItemModelResult>
        {
            public GetSimpleItemModelTask(IGs2Session session, Request.GetSimpleItemModelRequest request) : base(session, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(Request.GetSimpleItemModelRequest request)
            {
                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);

                jsonWriter.WriteObjectStart();

                if (request.NamespaceName != null)
                {
                    jsonWriter.WritePropertyName("namespaceName");
                    jsonWriter.Write(request.NamespaceName.ToString());
                }
                if (request.InventoryName != null)
                {
                    jsonWriter.WritePropertyName("inventoryName");
                    jsonWriter.Write(request.InventoryName.ToString());
                }
                if (request.ItemName != null)
                {
                    jsonWriter.WritePropertyName("itemName");
                    jsonWriter.Write(request.ItemName.ToString());
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
                    "inventory",
                    "simpleItemModel",
                    "getSimpleItemModel",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
        public IEnumerator GetSimpleItemModel(
                Request.GetSimpleItemModelRequest request,
                UnityAction<AsyncResult<Result.GetSimpleItemModelResult>> callback
        ) =>
            new GetSimpleItemModelTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToCoroutine(callback);

        public IFuture<Result.GetSimpleItemModelResult> GetSimpleItemModelFuture(
                Request.GetSimpleItemModelRequest request
        ) =>
            new GetSimpleItemModelTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
        public UniTask<Result.GetSimpleItemModelResult> GetSimpleItemModelAsync(
            Request.GetSimpleItemModelRequest request
        ) =>
            new GetSimpleItemModelTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsUniTask<Result.GetSimpleItemModelResult>();
    #else
        public GetSimpleItemModelTask GetSimpleItemModelAsync(
                Request.GetSimpleItemModelRequest request
        )
        {
            return new GetSimpleItemModelTask(
                Gs2WebSocketSession,
                request
            );
        }
    #endif
#else
        public Task<Result.GetSimpleItemModelResult> GetSimpleItemModelAsync(
            Request.GetSimpleItemModelRequest request
        ) =>
            new GetSimpleItemModelTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsTask();
#endif


        public class CreateBigInventoryModelMasterTask : Gs2WebSocketSessionTask<Request.CreateBigInventoryModelMasterRequest, Result.CreateBigInventoryModelMasterResult>
        {
            public CreateBigInventoryModelMasterTask(IGs2Session session, Request.CreateBigInventoryModelMasterRequest request) : base(session, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(Request.CreateBigInventoryModelMasterRequest request)
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
                    "inventory",
                    "bigInventoryModelMaster",
                    "createBigInventoryModelMaster",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
        public IEnumerator CreateBigInventoryModelMaster(
                Request.CreateBigInventoryModelMasterRequest request,
                UnityAction<AsyncResult<Result.CreateBigInventoryModelMasterResult>> callback
        ) =>
            new CreateBigInventoryModelMasterTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToCoroutine(callback);

        public IFuture<Result.CreateBigInventoryModelMasterResult> CreateBigInventoryModelMasterFuture(
                Request.CreateBigInventoryModelMasterRequest request
        ) =>
            new CreateBigInventoryModelMasterTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
        public UniTask<Result.CreateBigInventoryModelMasterResult> CreateBigInventoryModelMasterAsync(
            Request.CreateBigInventoryModelMasterRequest request
        ) =>
            new CreateBigInventoryModelMasterTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsUniTask<Result.CreateBigInventoryModelMasterResult>();
    #else
        public CreateBigInventoryModelMasterTask CreateBigInventoryModelMasterAsync(
                Request.CreateBigInventoryModelMasterRequest request
        )
        {
            return new CreateBigInventoryModelMasterTask(
                Gs2WebSocketSession,
                request
            );
        }
    #endif
#else
        public Task<Result.CreateBigInventoryModelMasterResult> CreateBigInventoryModelMasterAsync(
            Request.CreateBigInventoryModelMasterRequest request
        ) =>
            new CreateBigInventoryModelMasterTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsTask();
#endif


        public class GetBigInventoryModelMasterTask : Gs2WebSocketSessionTask<Request.GetBigInventoryModelMasterRequest, Result.GetBigInventoryModelMasterResult>
        {
            public GetBigInventoryModelMasterTask(IGs2Session session, Request.GetBigInventoryModelMasterRequest request) : base(session, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(Request.GetBigInventoryModelMasterRequest request)
            {
                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);

                jsonWriter.WriteObjectStart();

                if (request.NamespaceName != null)
                {
                    jsonWriter.WritePropertyName("namespaceName");
                    jsonWriter.Write(request.NamespaceName.ToString());
                }
                if (request.InventoryName != null)
                {
                    jsonWriter.WritePropertyName("inventoryName");
                    jsonWriter.Write(request.InventoryName.ToString());
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
                    "inventory",
                    "bigInventoryModelMaster",
                    "getBigInventoryModelMaster",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
        public IEnumerator GetBigInventoryModelMaster(
                Request.GetBigInventoryModelMasterRequest request,
                UnityAction<AsyncResult<Result.GetBigInventoryModelMasterResult>> callback
        ) =>
            new GetBigInventoryModelMasterTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToCoroutine(callback);

        public IFuture<Result.GetBigInventoryModelMasterResult> GetBigInventoryModelMasterFuture(
                Request.GetBigInventoryModelMasterRequest request
        ) =>
            new GetBigInventoryModelMasterTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
        public UniTask<Result.GetBigInventoryModelMasterResult> GetBigInventoryModelMasterAsync(
            Request.GetBigInventoryModelMasterRequest request
        ) =>
            new GetBigInventoryModelMasterTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsUniTask<Result.GetBigInventoryModelMasterResult>();
    #else
        public GetBigInventoryModelMasterTask GetBigInventoryModelMasterAsync(
                Request.GetBigInventoryModelMasterRequest request
        )
        {
            return new GetBigInventoryModelMasterTask(
                Gs2WebSocketSession,
                request
            );
        }
    #endif
#else
        public Task<Result.GetBigInventoryModelMasterResult> GetBigInventoryModelMasterAsync(
            Request.GetBigInventoryModelMasterRequest request
        ) =>
            new GetBigInventoryModelMasterTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsTask();
#endif


        public class UpdateBigInventoryModelMasterTask : Gs2WebSocketSessionTask<Request.UpdateBigInventoryModelMasterRequest, Result.UpdateBigInventoryModelMasterResult>
        {
            public UpdateBigInventoryModelMasterTask(IGs2Session session, Request.UpdateBigInventoryModelMasterRequest request) : base(session, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(Request.UpdateBigInventoryModelMasterRequest request)
            {
                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);

                jsonWriter.WriteObjectStart();

                if (request.NamespaceName != null)
                {
                    jsonWriter.WritePropertyName("namespaceName");
                    jsonWriter.Write(request.NamespaceName.ToString());
                }
                if (request.InventoryName != null)
                {
                    jsonWriter.WritePropertyName("inventoryName");
                    jsonWriter.Write(request.InventoryName.ToString());
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
                    "inventory",
                    "bigInventoryModelMaster",
                    "updateBigInventoryModelMaster",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
        public IEnumerator UpdateBigInventoryModelMaster(
                Request.UpdateBigInventoryModelMasterRequest request,
                UnityAction<AsyncResult<Result.UpdateBigInventoryModelMasterResult>> callback
        ) =>
            new UpdateBigInventoryModelMasterTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToCoroutine(callback);

        public IFuture<Result.UpdateBigInventoryModelMasterResult> UpdateBigInventoryModelMasterFuture(
                Request.UpdateBigInventoryModelMasterRequest request
        ) =>
            new UpdateBigInventoryModelMasterTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
        public UniTask<Result.UpdateBigInventoryModelMasterResult> UpdateBigInventoryModelMasterAsync(
            Request.UpdateBigInventoryModelMasterRequest request
        ) =>
            new UpdateBigInventoryModelMasterTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsUniTask<Result.UpdateBigInventoryModelMasterResult>();
    #else
        public UpdateBigInventoryModelMasterTask UpdateBigInventoryModelMasterAsync(
                Request.UpdateBigInventoryModelMasterRequest request
        )
        {
            return new UpdateBigInventoryModelMasterTask(
                Gs2WebSocketSession,
                request
            );
        }
    #endif
#else
        public Task<Result.UpdateBigInventoryModelMasterResult> UpdateBigInventoryModelMasterAsync(
            Request.UpdateBigInventoryModelMasterRequest request
        ) =>
            new UpdateBigInventoryModelMasterTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsTask();
#endif


        public class DeleteBigInventoryModelMasterTask : Gs2WebSocketSessionTask<Request.DeleteBigInventoryModelMasterRequest, Result.DeleteBigInventoryModelMasterResult>
        {
            public DeleteBigInventoryModelMasterTask(IGs2Session session, Request.DeleteBigInventoryModelMasterRequest request) : base(session, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(Request.DeleteBigInventoryModelMasterRequest request)
            {
                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);

                jsonWriter.WriteObjectStart();

                if (request.NamespaceName != null)
                {
                    jsonWriter.WritePropertyName("namespaceName");
                    jsonWriter.Write(request.NamespaceName.ToString());
                }
                if (request.InventoryName != null)
                {
                    jsonWriter.WritePropertyName("inventoryName");
                    jsonWriter.Write(request.InventoryName.ToString());
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
                    "inventory",
                    "bigInventoryModelMaster",
                    "deleteBigInventoryModelMaster",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
        public IEnumerator DeleteBigInventoryModelMaster(
                Request.DeleteBigInventoryModelMasterRequest request,
                UnityAction<AsyncResult<Result.DeleteBigInventoryModelMasterResult>> callback
        ) =>
            new DeleteBigInventoryModelMasterTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToCoroutine(callback);

        public IFuture<Result.DeleteBigInventoryModelMasterResult> DeleteBigInventoryModelMasterFuture(
                Request.DeleteBigInventoryModelMasterRequest request
        ) =>
            new DeleteBigInventoryModelMasterTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
        public UniTask<Result.DeleteBigInventoryModelMasterResult> DeleteBigInventoryModelMasterAsync(
            Request.DeleteBigInventoryModelMasterRequest request
        ) =>
            new DeleteBigInventoryModelMasterTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsUniTask<Result.DeleteBigInventoryModelMasterResult>();
    #else
        public DeleteBigInventoryModelMasterTask DeleteBigInventoryModelMasterAsync(
                Request.DeleteBigInventoryModelMasterRequest request
        )
        {
            return new DeleteBigInventoryModelMasterTask(
                Gs2WebSocketSession,
                request
            );
        }
    #endif
#else
        public Task<Result.DeleteBigInventoryModelMasterResult> DeleteBigInventoryModelMasterAsync(
            Request.DeleteBigInventoryModelMasterRequest request
        ) =>
            new DeleteBigInventoryModelMasterTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsTask();
#endif


        public class CreateBigItemModelMasterTask : Gs2WebSocketSessionTask<Request.CreateBigItemModelMasterRequest, Result.CreateBigItemModelMasterResult>
        {
            public CreateBigItemModelMasterTask(IGs2Session session, Request.CreateBigItemModelMasterRequest request) : base(session, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(Request.CreateBigItemModelMasterRequest request)
            {
                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);

                jsonWriter.WriteObjectStart();

                if (request.NamespaceName != null)
                {
                    jsonWriter.WritePropertyName("namespaceName");
                    jsonWriter.Write(request.NamespaceName.ToString());
                }
                if (request.InventoryName != null)
                {
                    jsonWriter.WritePropertyName("inventoryName");
                    jsonWriter.Write(request.InventoryName.ToString());
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
                    "inventory",
                    "bigItemModelMaster",
                    "createBigItemModelMaster",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
        public IEnumerator CreateBigItemModelMaster(
                Request.CreateBigItemModelMasterRequest request,
                UnityAction<AsyncResult<Result.CreateBigItemModelMasterResult>> callback
        ) =>
            new CreateBigItemModelMasterTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToCoroutine(callback);

        public IFuture<Result.CreateBigItemModelMasterResult> CreateBigItemModelMasterFuture(
                Request.CreateBigItemModelMasterRequest request
        ) =>
            new CreateBigItemModelMasterTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
        public UniTask<Result.CreateBigItemModelMasterResult> CreateBigItemModelMasterAsync(
            Request.CreateBigItemModelMasterRequest request
        ) =>
            new CreateBigItemModelMasterTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsUniTask<Result.CreateBigItemModelMasterResult>();
    #else
        public CreateBigItemModelMasterTask CreateBigItemModelMasterAsync(
                Request.CreateBigItemModelMasterRequest request
        )
        {
            return new CreateBigItemModelMasterTask(
                Gs2WebSocketSession,
                request
            );
        }
    #endif
#else
        public Task<Result.CreateBigItemModelMasterResult> CreateBigItemModelMasterAsync(
            Request.CreateBigItemModelMasterRequest request
        ) =>
            new CreateBigItemModelMasterTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsTask();
#endif


        public class GetBigItemModelMasterTask : Gs2WebSocketSessionTask<Request.GetBigItemModelMasterRequest, Result.GetBigItemModelMasterResult>
        {
            public GetBigItemModelMasterTask(IGs2Session session, Request.GetBigItemModelMasterRequest request) : base(session, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(Request.GetBigItemModelMasterRequest request)
            {
                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);

                jsonWriter.WriteObjectStart();

                if (request.NamespaceName != null)
                {
                    jsonWriter.WritePropertyName("namespaceName");
                    jsonWriter.Write(request.NamespaceName.ToString());
                }
                if (request.InventoryName != null)
                {
                    jsonWriter.WritePropertyName("inventoryName");
                    jsonWriter.Write(request.InventoryName.ToString());
                }
                if (request.ItemName != null)
                {
                    jsonWriter.WritePropertyName("itemName");
                    jsonWriter.Write(request.ItemName.ToString());
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
                    "inventory",
                    "bigItemModelMaster",
                    "getBigItemModelMaster",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
        public IEnumerator GetBigItemModelMaster(
                Request.GetBigItemModelMasterRequest request,
                UnityAction<AsyncResult<Result.GetBigItemModelMasterResult>> callback
        ) =>
            new GetBigItemModelMasterTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToCoroutine(callback);

        public IFuture<Result.GetBigItemModelMasterResult> GetBigItemModelMasterFuture(
                Request.GetBigItemModelMasterRequest request
        ) =>
            new GetBigItemModelMasterTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
        public UniTask<Result.GetBigItemModelMasterResult> GetBigItemModelMasterAsync(
            Request.GetBigItemModelMasterRequest request
        ) =>
            new GetBigItemModelMasterTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsUniTask<Result.GetBigItemModelMasterResult>();
    #else
        public GetBigItemModelMasterTask GetBigItemModelMasterAsync(
                Request.GetBigItemModelMasterRequest request
        )
        {
            return new GetBigItemModelMasterTask(
                Gs2WebSocketSession,
                request
            );
        }
    #endif
#else
        public Task<Result.GetBigItemModelMasterResult> GetBigItemModelMasterAsync(
            Request.GetBigItemModelMasterRequest request
        ) =>
            new GetBigItemModelMasterTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsTask();
#endif


        public class UpdateBigItemModelMasterTask : Gs2WebSocketSessionTask<Request.UpdateBigItemModelMasterRequest, Result.UpdateBigItemModelMasterResult>
        {
            public UpdateBigItemModelMasterTask(IGs2Session session, Request.UpdateBigItemModelMasterRequest request) : base(session, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(Request.UpdateBigItemModelMasterRequest request)
            {
                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);

                jsonWriter.WriteObjectStart();

                if (request.NamespaceName != null)
                {
                    jsonWriter.WritePropertyName("namespaceName");
                    jsonWriter.Write(request.NamespaceName.ToString());
                }
                if (request.InventoryName != null)
                {
                    jsonWriter.WritePropertyName("inventoryName");
                    jsonWriter.Write(request.InventoryName.ToString());
                }
                if (request.ItemName != null)
                {
                    jsonWriter.WritePropertyName("itemName");
                    jsonWriter.Write(request.ItemName.ToString());
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
                    "inventory",
                    "bigItemModelMaster",
                    "updateBigItemModelMaster",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
        public IEnumerator UpdateBigItemModelMaster(
                Request.UpdateBigItemModelMasterRequest request,
                UnityAction<AsyncResult<Result.UpdateBigItemModelMasterResult>> callback
        ) =>
            new UpdateBigItemModelMasterTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToCoroutine(callback);

        public IFuture<Result.UpdateBigItemModelMasterResult> UpdateBigItemModelMasterFuture(
                Request.UpdateBigItemModelMasterRequest request
        ) =>
            new UpdateBigItemModelMasterTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
        public UniTask<Result.UpdateBigItemModelMasterResult> UpdateBigItemModelMasterAsync(
            Request.UpdateBigItemModelMasterRequest request
        ) =>
            new UpdateBigItemModelMasterTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsUniTask<Result.UpdateBigItemModelMasterResult>();
    #else
        public UpdateBigItemModelMasterTask UpdateBigItemModelMasterAsync(
                Request.UpdateBigItemModelMasterRequest request
        )
        {
            return new UpdateBigItemModelMasterTask(
                Gs2WebSocketSession,
                request
            );
        }
    #endif
#else
        public Task<Result.UpdateBigItemModelMasterResult> UpdateBigItemModelMasterAsync(
            Request.UpdateBigItemModelMasterRequest request
        ) =>
            new UpdateBigItemModelMasterTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsTask();
#endif


        public class DeleteBigItemModelMasterTask : Gs2WebSocketSessionTask<Request.DeleteBigItemModelMasterRequest, Result.DeleteBigItemModelMasterResult>
        {
            public DeleteBigItemModelMasterTask(IGs2Session session, Request.DeleteBigItemModelMasterRequest request) : base(session, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(Request.DeleteBigItemModelMasterRequest request)
            {
                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);

                jsonWriter.WriteObjectStart();

                if (request.NamespaceName != null)
                {
                    jsonWriter.WritePropertyName("namespaceName");
                    jsonWriter.Write(request.NamespaceName.ToString());
                }
                if (request.InventoryName != null)
                {
                    jsonWriter.WritePropertyName("inventoryName");
                    jsonWriter.Write(request.InventoryName.ToString());
                }
                if (request.ItemName != null)
                {
                    jsonWriter.WritePropertyName("itemName");
                    jsonWriter.Write(request.ItemName.ToString());
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
                    "inventory",
                    "bigItemModelMaster",
                    "deleteBigItemModelMaster",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
        public IEnumerator DeleteBigItemModelMaster(
                Request.DeleteBigItemModelMasterRequest request,
                UnityAction<AsyncResult<Result.DeleteBigItemModelMasterResult>> callback
        ) =>
            new DeleteBigItemModelMasterTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToCoroutine(callback);

        public IFuture<Result.DeleteBigItemModelMasterResult> DeleteBigItemModelMasterFuture(
                Request.DeleteBigItemModelMasterRequest request
        ) =>
            new DeleteBigItemModelMasterTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
        public UniTask<Result.DeleteBigItemModelMasterResult> DeleteBigItemModelMasterAsync(
            Request.DeleteBigItemModelMasterRequest request
        ) =>
            new DeleteBigItemModelMasterTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsUniTask<Result.DeleteBigItemModelMasterResult>();
    #else
        public DeleteBigItemModelMasterTask DeleteBigItemModelMasterAsync(
                Request.DeleteBigItemModelMasterRequest request
        )
        {
            return new DeleteBigItemModelMasterTask(
                Gs2WebSocketSession,
                request
            );
        }
    #endif
#else
        public Task<Result.DeleteBigItemModelMasterResult> DeleteBigItemModelMasterAsync(
            Request.DeleteBigItemModelMasterRequest request
        ) =>
            new DeleteBigItemModelMasterTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsTask();
#endif


        public class GetBigItemModelTask : Gs2WebSocketSessionTask<Request.GetBigItemModelRequest, Result.GetBigItemModelResult>
        {
            public GetBigItemModelTask(IGs2Session session, Request.GetBigItemModelRequest request) : base(session, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(Request.GetBigItemModelRequest request)
            {
                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);

                jsonWriter.WriteObjectStart();

                if (request.NamespaceName != null)
                {
                    jsonWriter.WritePropertyName("namespaceName");
                    jsonWriter.Write(request.NamespaceName.ToString());
                }
                if (request.InventoryName != null)
                {
                    jsonWriter.WritePropertyName("inventoryName");
                    jsonWriter.Write(request.InventoryName.ToString());
                }
                if (request.ItemName != null)
                {
                    jsonWriter.WritePropertyName("itemName");
                    jsonWriter.Write(request.ItemName.ToString());
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
                    "inventory",
                    "bigItemModel",
                    "getBigItemModel",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
        public IEnumerator GetBigItemModel(
                Request.GetBigItemModelRequest request,
                UnityAction<AsyncResult<Result.GetBigItemModelResult>> callback
        ) =>
            new GetBigItemModelTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToCoroutine(callback);

        public IFuture<Result.GetBigItemModelResult> GetBigItemModelFuture(
                Request.GetBigItemModelRequest request
        ) =>
            new GetBigItemModelTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
        public UniTask<Result.GetBigItemModelResult> GetBigItemModelAsync(
            Request.GetBigItemModelRequest request
        ) =>
            new GetBigItemModelTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsUniTask<Result.GetBigItemModelResult>();
    #else
        public GetBigItemModelTask GetBigItemModelAsync(
                Request.GetBigItemModelRequest request
        )
        {
            return new GetBigItemModelTask(
                Gs2WebSocketSession,
                request
            );
        }
    #endif
#else
        public Task<Result.GetBigItemModelResult> GetBigItemModelAsync(
            Request.GetBigItemModelRequest request
        ) =>
            new GetBigItemModelTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsTask();
#endif


        public class PreUpdateCurrentItemModelMasterTask : Gs2WebSocketSessionTask<Request.PreUpdateCurrentItemModelMasterRequest, Result.PreUpdateCurrentItemModelMasterResult>
        {
            public PreUpdateCurrentItemModelMasterTask(IGs2Session session, Request.PreUpdateCurrentItemModelMasterRequest request) : base(session, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(Request.PreUpdateCurrentItemModelMasterRequest request)
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
                    "inventory",
                    "currentItemModelMaster",
                    "preUpdateCurrentItemModelMaster",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
        public IEnumerator PreUpdateCurrentItemModelMaster(
                Request.PreUpdateCurrentItemModelMasterRequest request,
                UnityAction<AsyncResult<Result.PreUpdateCurrentItemModelMasterResult>> callback
        ) =>
            new PreUpdateCurrentItemModelMasterTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToCoroutine(callback);

        public IFuture<Result.PreUpdateCurrentItemModelMasterResult> PreUpdateCurrentItemModelMasterFuture(
                Request.PreUpdateCurrentItemModelMasterRequest request
        ) =>
            new PreUpdateCurrentItemModelMasterTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
        public UniTask<Result.PreUpdateCurrentItemModelMasterResult> PreUpdateCurrentItemModelMasterAsync(
            Request.PreUpdateCurrentItemModelMasterRequest request
        ) =>
            new PreUpdateCurrentItemModelMasterTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsUniTask<Result.PreUpdateCurrentItemModelMasterResult>();
    #else
        public PreUpdateCurrentItemModelMasterTask PreUpdateCurrentItemModelMasterAsync(
                Request.PreUpdateCurrentItemModelMasterRequest request
        )
        {
            return new PreUpdateCurrentItemModelMasterTask(
                Gs2WebSocketSession,
                request
            );
        }
    #endif
#else
        public Task<Result.PreUpdateCurrentItemModelMasterResult> PreUpdateCurrentItemModelMasterAsync(
            Request.PreUpdateCurrentItemModelMasterRequest request
        ) =>
            new PreUpdateCurrentItemModelMasterTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsTask();
#endif


        public class GetInventoryTask : Gs2WebSocketSessionTask<Request.GetInventoryRequest, Result.GetInventoryResult>
        {
            public GetInventoryTask(IGs2Session session, Request.GetInventoryRequest request) : base(session, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(Request.GetInventoryRequest request)
            {
                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);

                jsonWriter.WriteObjectStart();

                if (request.NamespaceName != null)
                {
                    jsonWriter.WritePropertyName("namespaceName");
                    jsonWriter.Write(request.NamespaceName.ToString());
                }
                if (request.InventoryName != null)
                {
                    jsonWriter.WritePropertyName("inventoryName");
                    jsonWriter.Write(request.InventoryName.ToString());
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
                    "inventory",
                    "inventory",
                    "getInventory",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
        public IEnumerator GetInventory(
                Request.GetInventoryRequest request,
                UnityAction<AsyncResult<Result.GetInventoryResult>> callback
        ) =>
            new GetInventoryTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToCoroutine(callback);

        public IFuture<Result.GetInventoryResult> GetInventoryFuture(
                Request.GetInventoryRequest request
        ) =>
            new GetInventoryTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
        public UniTask<Result.GetInventoryResult> GetInventoryAsync(
            Request.GetInventoryRequest request
        ) =>
            new GetInventoryTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsUniTask<Result.GetInventoryResult>();
    #else
        public GetInventoryTask GetInventoryAsync(
                Request.GetInventoryRequest request
        )
        {
            return new GetInventoryTask(
                Gs2WebSocketSession,
                request
            );
        }
    #endif
#else
        public Task<Result.GetInventoryResult> GetInventoryAsync(
            Request.GetInventoryRequest request
        ) =>
            new GetInventoryTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsTask();
#endif


        public class GetInventoryByUserIdTask : Gs2WebSocketSessionTask<Request.GetInventoryByUserIdRequest, Result.GetInventoryByUserIdResult>
        {
            public GetInventoryByUserIdTask(IGs2Session session, Request.GetInventoryByUserIdRequest request) : base(session, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(Request.GetInventoryByUserIdRequest request)
            {
                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);

                jsonWriter.WriteObjectStart();

                if (request.NamespaceName != null)
                {
                    jsonWriter.WritePropertyName("namespaceName");
                    jsonWriter.Write(request.NamespaceName.ToString());
                }
                if (request.InventoryName != null)
                {
                    jsonWriter.WritePropertyName("inventoryName");
                    jsonWriter.Write(request.InventoryName.ToString());
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
                    "inventory",
                    "inventory",
                    "getInventoryByUserId",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
        public IEnumerator GetInventoryByUserId(
                Request.GetInventoryByUserIdRequest request,
                UnityAction<AsyncResult<Result.GetInventoryByUserIdResult>> callback
        ) =>
            new GetInventoryByUserIdTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToCoroutine(callback);

        public IFuture<Result.GetInventoryByUserIdResult> GetInventoryByUserIdFuture(
                Request.GetInventoryByUserIdRequest request
        ) =>
            new GetInventoryByUserIdTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
        public UniTask<Result.GetInventoryByUserIdResult> GetInventoryByUserIdAsync(
            Request.GetInventoryByUserIdRequest request
        ) =>
            new GetInventoryByUserIdTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsUniTask<Result.GetInventoryByUserIdResult>();
    #else
        public GetInventoryByUserIdTask GetInventoryByUserIdAsync(
                Request.GetInventoryByUserIdRequest request
        )
        {
            return new GetInventoryByUserIdTask(
                Gs2WebSocketSession,
                request
            );
        }
    #endif
#else
        public Task<Result.GetInventoryByUserIdResult> GetInventoryByUserIdAsync(
            Request.GetInventoryByUserIdRequest request
        ) =>
            new GetInventoryByUserIdTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsTask();
#endif


        public class AddCapacityByUserIdTask : Gs2WebSocketSessionTask<Request.AddCapacityByUserIdRequest, Result.AddCapacityByUserIdResult>
        {
            public AddCapacityByUserIdTask(IGs2Session session, Request.AddCapacityByUserIdRequest request) : base(session, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(Request.AddCapacityByUserIdRequest request)
            {
                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);

                jsonWriter.WriteObjectStart();

                if (request.NamespaceName != null)
                {
                    jsonWriter.WritePropertyName("namespaceName");
                    jsonWriter.Write(request.NamespaceName.ToString());
                }
                if (request.InventoryName != null)
                {
                    jsonWriter.WritePropertyName("inventoryName");
                    jsonWriter.Write(request.InventoryName.ToString());
                }
                if (request.UserId != null)
                {
                    jsonWriter.WritePropertyName("userId");
                    jsonWriter.Write(request.UserId.ToString());
                }
                if (request.AddCapacityValue != null)
                {
                    jsonWriter.WritePropertyName("addCapacityValue");
                    jsonWriter.Write(request.AddCapacityValue.ToString());
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
                    "inventory",
                    "inventory",
                    "addCapacityByUserId",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
        public IEnumerator AddCapacityByUserId(
                Request.AddCapacityByUserIdRequest request,
                UnityAction<AsyncResult<Result.AddCapacityByUserIdResult>> callback
        ) =>
            new AddCapacityByUserIdTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToCoroutine(callback);

        public IFuture<Result.AddCapacityByUserIdResult> AddCapacityByUserIdFuture(
                Request.AddCapacityByUserIdRequest request
        ) =>
            new AddCapacityByUserIdTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
        public UniTask<Result.AddCapacityByUserIdResult> AddCapacityByUserIdAsync(
            Request.AddCapacityByUserIdRequest request
        ) =>
            new AddCapacityByUserIdTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsUniTask<Result.AddCapacityByUserIdResult>();
    #else
        public AddCapacityByUserIdTask AddCapacityByUserIdAsync(
                Request.AddCapacityByUserIdRequest request
        )
        {
            return new AddCapacityByUserIdTask(
                Gs2WebSocketSession,
                request
            );
        }
    #endif
#else
        public Task<Result.AddCapacityByUserIdResult> AddCapacityByUserIdAsync(
            Request.AddCapacityByUserIdRequest request
        ) =>
            new AddCapacityByUserIdTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsTask();
#endif


        public class SetCapacityByUserIdTask : Gs2WebSocketSessionTask<Request.SetCapacityByUserIdRequest, Result.SetCapacityByUserIdResult>
        {
            public SetCapacityByUserIdTask(IGs2Session session, Request.SetCapacityByUserIdRequest request) : base(session, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(Request.SetCapacityByUserIdRequest request)
            {
                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);

                jsonWriter.WriteObjectStart();

                if (request.NamespaceName != null)
                {
                    jsonWriter.WritePropertyName("namespaceName");
                    jsonWriter.Write(request.NamespaceName.ToString());
                }
                if (request.InventoryName != null)
                {
                    jsonWriter.WritePropertyName("inventoryName");
                    jsonWriter.Write(request.InventoryName.ToString());
                }
                if (request.UserId != null)
                {
                    jsonWriter.WritePropertyName("userId");
                    jsonWriter.Write(request.UserId.ToString());
                }
                if (request.NewCapacityValue != null)
                {
                    jsonWriter.WritePropertyName("newCapacityValue");
                    jsonWriter.Write(request.NewCapacityValue.ToString());
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
                    "inventory",
                    "inventory",
                    "setCapacityByUserId",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
        public IEnumerator SetCapacityByUserId(
                Request.SetCapacityByUserIdRequest request,
                UnityAction<AsyncResult<Result.SetCapacityByUserIdResult>> callback
        ) =>
            new SetCapacityByUserIdTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToCoroutine(callback);

        public IFuture<Result.SetCapacityByUserIdResult> SetCapacityByUserIdFuture(
                Request.SetCapacityByUserIdRequest request
        ) =>
            new SetCapacityByUserIdTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
        public UniTask<Result.SetCapacityByUserIdResult> SetCapacityByUserIdAsync(
            Request.SetCapacityByUserIdRequest request
        ) =>
            new SetCapacityByUserIdTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsUniTask<Result.SetCapacityByUserIdResult>();
    #else
        public SetCapacityByUserIdTask SetCapacityByUserIdAsync(
                Request.SetCapacityByUserIdRequest request
        )
        {
            return new SetCapacityByUserIdTask(
                Gs2WebSocketSession,
                request
            );
        }
    #endif
#else
        public Task<Result.SetCapacityByUserIdResult> SetCapacityByUserIdAsync(
            Request.SetCapacityByUserIdRequest request
        ) =>
            new SetCapacityByUserIdTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsTask();
#endif


        public class DeleteInventoryByUserIdTask : Gs2WebSocketSessionTask<Request.DeleteInventoryByUserIdRequest, Result.DeleteInventoryByUserIdResult>
        {
            public DeleteInventoryByUserIdTask(IGs2Session session, Request.DeleteInventoryByUserIdRequest request) : base(session, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(Request.DeleteInventoryByUserIdRequest request)
            {
                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);

                jsonWriter.WriteObjectStart();

                if (request.NamespaceName != null)
                {
                    jsonWriter.WritePropertyName("namespaceName");
                    jsonWriter.Write(request.NamespaceName.ToString());
                }
                if (request.InventoryName != null)
                {
                    jsonWriter.WritePropertyName("inventoryName");
                    jsonWriter.Write(request.InventoryName.ToString());
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
                    "inventory",
                    "inventory",
                    "deleteInventoryByUserId",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
        public IEnumerator DeleteInventoryByUserId(
                Request.DeleteInventoryByUserIdRequest request,
                UnityAction<AsyncResult<Result.DeleteInventoryByUserIdResult>> callback
        ) =>
            new DeleteInventoryByUserIdTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToCoroutine(callback);

        public IFuture<Result.DeleteInventoryByUserIdResult> DeleteInventoryByUserIdFuture(
                Request.DeleteInventoryByUserIdRequest request
        ) =>
            new DeleteInventoryByUserIdTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
        public UniTask<Result.DeleteInventoryByUserIdResult> DeleteInventoryByUserIdAsync(
            Request.DeleteInventoryByUserIdRequest request
        ) =>
            new DeleteInventoryByUserIdTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsUniTask<Result.DeleteInventoryByUserIdResult>();
    #else
        public DeleteInventoryByUserIdTask DeleteInventoryByUserIdAsync(
                Request.DeleteInventoryByUserIdRequest request
        )
        {
            return new DeleteInventoryByUserIdTask(
                Gs2WebSocketSession,
                request
            );
        }
    #endif
#else
        public Task<Result.DeleteInventoryByUserIdResult> DeleteInventoryByUserIdAsync(
            Request.DeleteInventoryByUserIdRequest request
        ) =>
            new DeleteInventoryByUserIdTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsTask();
#endif


        public class VerifyInventoryCurrentMaxCapacityTask : Gs2WebSocketSessionTask<Request.VerifyInventoryCurrentMaxCapacityRequest, Result.VerifyInventoryCurrentMaxCapacityResult>
        {
            public VerifyInventoryCurrentMaxCapacityTask(IGs2Session session, Request.VerifyInventoryCurrentMaxCapacityRequest request) : base(session, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(Request.VerifyInventoryCurrentMaxCapacityRequest request)
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
                if (request.InventoryName != null)
                {
                    jsonWriter.WritePropertyName("inventoryName");
                    jsonWriter.Write(request.InventoryName.ToString());
                }
                if (request.VerifyType != null)
                {
                    jsonWriter.WritePropertyName("verifyType");
                    jsonWriter.Write(request.VerifyType.ToString());
                }
                if (request.CurrentInventoryMaxCapacity != null)
                {
                    jsonWriter.WritePropertyName("currentInventoryMaxCapacity");
                    jsonWriter.Write(request.CurrentInventoryMaxCapacity.ToString());
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
                    "inventory",
                    "inventory",
                    "verifyInventoryCurrentMaxCapacity",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
        public IEnumerator VerifyInventoryCurrentMaxCapacity(
                Request.VerifyInventoryCurrentMaxCapacityRequest request,
                UnityAction<AsyncResult<Result.VerifyInventoryCurrentMaxCapacityResult>> callback
        ) =>
            new VerifyInventoryCurrentMaxCapacityTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToCoroutine(callback);

        public IFuture<Result.VerifyInventoryCurrentMaxCapacityResult> VerifyInventoryCurrentMaxCapacityFuture(
                Request.VerifyInventoryCurrentMaxCapacityRequest request
        ) =>
            new VerifyInventoryCurrentMaxCapacityTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
        public UniTask<Result.VerifyInventoryCurrentMaxCapacityResult> VerifyInventoryCurrentMaxCapacityAsync(
            Request.VerifyInventoryCurrentMaxCapacityRequest request
        ) =>
            new VerifyInventoryCurrentMaxCapacityTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsUniTask<Result.VerifyInventoryCurrentMaxCapacityResult>();
    #else
        public VerifyInventoryCurrentMaxCapacityTask VerifyInventoryCurrentMaxCapacityAsync(
                Request.VerifyInventoryCurrentMaxCapacityRequest request
        )
        {
            return new VerifyInventoryCurrentMaxCapacityTask(
                Gs2WebSocketSession,
                request
            );
        }
    #endif
#else
        public Task<Result.VerifyInventoryCurrentMaxCapacityResult> VerifyInventoryCurrentMaxCapacityAsync(
            Request.VerifyInventoryCurrentMaxCapacityRequest request
        ) =>
            new VerifyInventoryCurrentMaxCapacityTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsTask();
#endif


        public class VerifyInventoryCurrentMaxCapacityByUserIdTask : Gs2WebSocketSessionTask<Request.VerifyInventoryCurrentMaxCapacityByUserIdRequest, Result.VerifyInventoryCurrentMaxCapacityByUserIdResult>
        {
            public VerifyInventoryCurrentMaxCapacityByUserIdTask(IGs2Session session, Request.VerifyInventoryCurrentMaxCapacityByUserIdRequest request) : base(session, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(Request.VerifyInventoryCurrentMaxCapacityByUserIdRequest request)
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
                if (request.InventoryName != null)
                {
                    jsonWriter.WritePropertyName("inventoryName");
                    jsonWriter.Write(request.InventoryName.ToString());
                }
                if (request.VerifyType != null)
                {
                    jsonWriter.WritePropertyName("verifyType");
                    jsonWriter.Write(request.VerifyType.ToString());
                }
                if (request.CurrentInventoryMaxCapacity != null)
                {
                    jsonWriter.WritePropertyName("currentInventoryMaxCapacity");
                    jsonWriter.Write(request.CurrentInventoryMaxCapacity.ToString());
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
                    "inventory",
                    "inventory",
                    "verifyInventoryCurrentMaxCapacityByUserId",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
        public IEnumerator VerifyInventoryCurrentMaxCapacityByUserId(
                Request.VerifyInventoryCurrentMaxCapacityByUserIdRequest request,
                UnityAction<AsyncResult<Result.VerifyInventoryCurrentMaxCapacityByUserIdResult>> callback
        ) =>
            new VerifyInventoryCurrentMaxCapacityByUserIdTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToCoroutine(callback);

        public IFuture<Result.VerifyInventoryCurrentMaxCapacityByUserIdResult> VerifyInventoryCurrentMaxCapacityByUserIdFuture(
                Request.VerifyInventoryCurrentMaxCapacityByUserIdRequest request
        ) =>
            new VerifyInventoryCurrentMaxCapacityByUserIdTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
        public UniTask<Result.VerifyInventoryCurrentMaxCapacityByUserIdResult> VerifyInventoryCurrentMaxCapacityByUserIdAsync(
            Request.VerifyInventoryCurrentMaxCapacityByUserIdRequest request
        ) =>
            new VerifyInventoryCurrentMaxCapacityByUserIdTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsUniTask<Result.VerifyInventoryCurrentMaxCapacityByUserIdResult>();
    #else
        public VerifyInventoryCurrentMaxCapacityByUserIdTask VerifyInventoryCurrentMaxCapacityByUserIdAsync(
                Request.VerifyInventoryCurrentMaxCapacityByUserIdRequest request
        )
        {
            return new VerifyInventoryCurrentMaxCapacityByUserIdTask(
                Gs2WebSocketSession,
                request
            );
        }
    #endif
#else
        public Task<Result.VerifyInventoryCurrentMaxCapacityByUserIdResult> VerifyInventoryCurrentMaxCapacityByUserIdAsync(
            Request.VerifyInventoryCurrentMaxCapacityByUserIdRequest request
        ) =>
            new VerifyInventoryCurrentMaxCapacityByUserIdTask(
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
                    "inventory",
                    "inventory",
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
                    "inventory",
                    "inventory",
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


        public class GetReferenceOfTask : Gs2WebSocketSessionTask<Request.GetReferenceOfRequest, Result.GetReferenceOfResult>
        {
            public GetReferenceOfTask(IGs2Session session, Request.GetReferenceOfRequest request) : base(session, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(Request.GetReferenceOfRequest request)
            {
                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);

                jsonWriter.WriteObjectStart();

                if (request.NamespaceName != null)
                {
                    jsonWriter.WritePropertyName("namespaceName");
                    jsonWriter.Write(request.NamespaceName.ToString());
                }
                if (request.InventoryName != null)
                {
                    jsonWriter.WritePropertyName("inventoryName");
                    jsonWriter.Write(request.InventoryName.ToString());
                }
                if (request.AccessToken != null)
                {
                    jsonWriter.WritePropertyName("accessToken");
                    jsonWriter.Write(request.AccessToken.ToString());
                }
                if (request.ItemName != null)
                {
                    jsonWriter.WritePropertyName("itemName");
                    jsonWriter.Write(request.ItemName.ToString());
                }
                if (request.ItemSetName != null)
                {
                    jsonWriter.WritePropertyName("itemSetName");
                    jsonWriter.Write(request.ItemSetName.ToString());
                }
                if (request.ReferenceOf != null)
                {
                    jsonWriter.WritePropertyName("referenceOf");
                    jsonWriter.Write(request.ReferenceOf.ToString());
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
                    "inventory",
                    "referenceOf",
                    "getReferenceOf",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
        public IEnumerator GetReferenceOf(
                Request.GetReferenceOfRequest request,
                UnityAction<AsyncResult<Result.GetReferenceOfResult>> callback
        ) =>
            new GetReferenceOfTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToCoroutine(callback);

        public IFuture<Result.GetReferenceOfResult> GetReferenceOfFuture(
                Request.GetReferenceOfRequest request
        ) =>
            new GetReferenceOfTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
        public UniTask<Result.GetReferenceOfResult> GetReferenceOfAsync(
            Request.GetReferenceOfRequest request
        ) =>
            new GetReferenceOfTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsUniTask<Result.GetReferenceOfResult>();
    #else
        public GetReferenceOfTask GetReferenceOfAsync(
                Request.GetReferenceOfRequest request
        )
        {
            return new GetReferenceOfTask(
                Gs2WebSocketSession,
                request
            );
        }
    #endif
#else
        public Task<Result.GetReferenceOfResult> GetReferenceOfAsync(
            Request.GetReferenceOfRequest request
        ) =>
            new GetReferenceOfTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsTask();
#endif


        public class GetReferenceOfByUserIdTask : Gs2WebSocketSessionTask<Request.GetReferenceOfByUserIdRequest, Result.GetReferenceOfByUserIdResult>
        {
            public GetReferenceOfByUserIdTask(IGs2Session session, Request.GetReferenceOfByUserIdRequest request) : base(session, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(Request.GetReferenceOfByUserIdRequest request)
            {
                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);

                jsonWriter.WriteObjectStart();

                if (request.NamespaceName != null)
                {
                    jsonWriter.WritePropertyName("namespaceName");
                    jsonWriter.Write(request.NamespaceName.ToString());
                }
                if (request.InventoryName != null)
                {
                    jsonWriter.WritePropertyName("inventoryName");
                    jsonWriter.Write(request.InventoryName.ToString());
                }
                if (request.UserId != null)
                {
                    jsonWriter.WritePropertyName("userId");
                    jsonWriter.Write(request.UserId.ToString());
                }
                if (request.ItemName != null)
                {
                    jsonWriter.WritePropertyName("itemName");
                    jsonWriter.Write(request.ItemName.ToString());
                }
                if (request.ItemSetName != null)
                {
                    jsonWriter.WritePropertyName("itemSetName");
                    jsonWriter.Write(request.ItemSetName.ToString());
                }
                if (request.ReferenceOf != null)
                {
                    jsonWriter.WritePropertyName("referenceOf");
                    jsonWriter.Write(request.ReferenceOf.ToString());
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
                    "inventory",
                    "referenceOf",
                    "getReferenceOfByUserId",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
        public IEnumerator GetReferenceOfByUserId(
                Request.GetReferenceOfByUserIdRequest request,
                UnityAction<AsyncResult<Result.GetReferenceOfByUserIdResult>> callback
        ) =>
            new GetReferenceOfByUserIdTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToCoroutine(callback);

        public IFuture<Result.GetReferenceOfByUserIdResult> GetReferenceOfByUserIdFuture(
                Request.GetReferenceOfByUserIdRequest request
        ) =>
            new GetReferenceOfByUserIdTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
        public UniTask<Result.GetReferenceOfByUserIdResult> GetReferenceOfByUserIdAsync(
            Request.GetReferenceOfByUserIdRequest request
        ) =>
            new GetReferenceOfByUserIdTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsUniTask<Result.GetReferenceOfByUserIdResult>();
    #else
        public GetReferenceOfByUserIdTask GetReferenceOfByUserIdAsync(
                Request.GetReferenceOfByUserIdRequest request
        )
        {
            return new GetReferenceOfByUserIdTask(
                Gs2WebSocketSession,
                request
            );
        }
    #endif
#else
        public Task<Result.GetReferenceOfByUserIdResult> GetReferenceOfByUserIdAsync(
            Request.GetReferenceOfByUserIdRequest request
        ) =>
            new GetReferenceOfByUserIdTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsTask();
#endif


        public class VerifyReferenceOfTask : Gs2WebSocketSessionTask<Request.VerifyReferenceOfRequest, Result.VerifyReferenceOfResult>
        {
            public VerifyReferenceOfTask(IGs2Session session, Request.VerifyReferenceOfRequest request) : base(session, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(Request.VerifyReferenceOfRequest request)
            {
                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);

                jsonWriter.WriteObjectStart();

                if (request.NamespaceName != null)
                {
                    jsonWriter.WritePropertyName("namespaceName");
                    jsonWriter.Write(request.NamespaceName.ToString());
                }
                if (request.InventoryName != null)
                {
                    jsonWriter.WritePropertyName("inventoryName");
                    jsonWriter.Write(request.InventoryName.ToString());
                }
                if (request.AccessToken != null)
                {
                    jsonWriter.WritePropertyName("accessToken");
                    jsonWriter.Write(request.AccessToken.ToString());
                }
                if (request.ItemName != null)
                {
                    jsonWriter.WritePropertyName("itemName");
                    jsonWriter.Write(request.ItemName.ToString());
                }
                if (request.ItemSetName != null)
                {
                    jsonWriter.WritePropertyName("itemSetName");
                    jsonWriter.Write(request.ItemSetName.ToString());
                }
                if (request.ReferenceOf != null)
                {
                    jsonWriter.WritePropertyName("referenceOf");
                    jsonWriter.Write(request.ReferenceOf.ToString());
                }
                if (request.VerifyType != null)
                {
                    jsonWriter.WritePropertyName("verifyType");
                    jsonWriter.Write(request.VerifyType.ToString());
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
                    "inventory",
                    "referenceOf",
                    "verifyReferenceOf",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
        public IEnumerator VerifyReferenceOf(
                Request.VerifyReferenceOfRequest request,
                UnityAction<AsyncResult<Result.VerifyReferenceOfResult>> callback
        ) =>
            new VerifyReferenceOfTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToCoroutine(callback);

        public IFuture<Result.VerifyReferenceOfResult> VerifyReferenceOfFuture(
                Request.VerifyReferenceOfRequest request
        ) =>
            new VerifyReferenceOfTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
        public UniTask<Result.VerifyReferenceOfResult> VerifyReferenceOfAsync(
            Request.VerifyReferenceOfRequest request
        ) =>
            new VerifyReferenceOfTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsUniTask<Result.VerifyReferenceOfResult>();
    #else
        public VerifyReferenceOfTask VerifyReferenceOfAsync(
                Request.VerifyReferenceOfRequest request
        )
        {
            return new VerifyReferenceOfTask(
                Gs2WebSocketSession,
                request
            );
        }
    #endif
#else
        public Task<Result.VerifyReferenceOfResult> VerifyReferenceOfAsync(
            Request.VerifyReferenceOfRequest request
        ) =>
            new VerifyReferenceOfTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsTask();
#endif


        public class VerifyReferenceOfByUserIdTask : Gs2WebSocketSessionTask<Request.VerifyReferenceOfByUserIdRequest, Result.VerifyReferenceOfByUserIdResult>
        {
            public VerifyReferenceOfByUserIdTask(IGs2Session session, Request.VerifyReferenceOfByUserIdRequest request) : base(session, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(Request.VerifyReferenceOfByUserIdRequest request)
            {
                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);

                jsonWriter.WriteObjectStart();

                if (request.NamespaceName != null)
                {
                    jsonWriter.WritePropertyName("namespaceName");
                    jsonWriter.Write(request.NamespaceName.ToString());
                }
                if (request.InventoryName != null)
                {
                    jsonWriter.WritePropertyName("inventoryName");
                    jsonWriter.Write(request.InventoryName.ToString());
                }
                if (request.UserId != null)
                {
                    jsonWriter.WritePropertyName("userId");
                    jsonWriter.Write(request.UserId.ToString());
                }
                if (request.ItemName != null)
                {
                    jsonWriter.WritePropertyName("itemName");
                    jsonWriter.Write(request.ItemName.ToString());
                }
                if (request.ItemSetName != null)
                {
                    jsonWriter.WritePropertyName("itemSetName");
                    jsonWriter.Write(request.ItemSetName.ToString());
                }
                if (request.ReferenceOf != null)
                {
                    jsonWriter.WritePropertyName("referenceOf");
                    jsonWriter.Write(request.ReferenceOf.ToString());
                }
                if (request.VerifyType != null)
                {
                    jsonWriter.WritePropertyName("verifyType");
                    jsonWriter.Write(request.VerifyType.ToString());
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
                    "inventory",
                    "referenceOf",
                    "verifyReferenceOfByUserId",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
        public IEnumerator VerifyReferenceOfByUserId(
                Request.VerifyReferenceOfByUserIdRequest request,
                UnityAction<AsyncResult<Result.VerifyReferenceOfByUserIdResult>> callback
        ) =>
            new VerifyReferenceOfByUserIdTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToCoroutine(callback);

        public IFuture<Result.VerifyReferenceOfByUserIdResult> VerifyReferenceOfByUserIdFuture(
                Request.VerifyReferenceOfByUserIdRequest request
        ) =>
            new VerifyReferenceOfByUserIdTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
        public UniTask<Result.VerifyReferenceOfByUserIdResult> VerifyReferenceOfByUserIdAsync(
            Request.VerifyReferenceOfByUserIdRequest request
        ) =>
            new VerifyReferenceOfByUserIdTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsUniTask<Result.VerifyReferenceOfByUserIdResult>();
    #else
        public VerifyReferenceOfByUserIdTask VerifyReferenceOfByUserIdAsync(
                Request.VerifyReferenceOfByUserIdRequest request
        )
        {
            return new VerifyReferenceOfByUserIdTask(
                Gs2WebSocketSession,
                request
            );
        }
    #endif
#else
        public Task<Result.VerifyReferenceOfByUserIdResult> VerifyReferenceOfByUserIdAsync(
            Request.VerifyReferenceOfByUserIdRequest request
        ) =>
            new VerifyReferenceOfByUserIdTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsTask();
#endif


        public class AddReferenceOfTask : Gs2WebSocketSessionTask<Request.AddReferenceOfRequest, Result.AddReferenceOfResult>
        {
            public AddReferenceOfTask(IGs2Session session, Request.AddReferenceOfRequest request) : base(session, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(Request.AddReferenceOfRequest request)
            {
                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);

                jsonWriter.WriteObjectStart();

                if (request.NamespaceName != null)
                {
                    jsonWriter.WritePropertyName("namespaceName");
                    jsonWriter.Write(request.NamespaceName.ToString());
                }
                if (request.InventoryName != null)
                {
                    jsonWriter.WritePropertyName("inventoryName");
                    jsonWriter.Write(request.InventoryName.ToString());
                }
                if (request.AccessToken != null)
                {
                    jsonWriter.WritePropertyName("accessToken");
                    jsonWriter.Write(request.AccessToken.ToString());
                }
                if (request.ItemName != null)
                {
                    jsonWriter.WritePropertyName("itemName");
                    jsonWriter.Write(request.ItemName.ToString());
                }
                if (request.ItemSetName != null)
                {
                    jsonWriter.WritePropertyName("itemSetName");
                    jsonWriter.Write(request.ItemSetName.ToString());
                }
                if (request.ReferenceOf != null)
                {
                    jsonWriter.WritePropertyName("referenceOf");
                    jsonWriter.Write(request.ReferenceOf.ToString());
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
                    "inventory",
                    "referenceOf",
                    "addReferenceOf",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
        public IEnumerator AddReferenceOf(
                Request.AddReferenceOfRequest request,
                UnityAction<AsyncResult<Result.AddReferenceOfResult>> callback
        ) =>
            new AddReferenceOfTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToCoroutine(callback);

        public IFuture<Result.AddReferenceOfResult> AddReferenceOfFuture(
                Request.AddReferenceOfRequest request
        ) =>
            new AddReferenceOfTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
        public UniTask<Result.AddReferenceOfResult> AddReferenceOfAsync(
            Request.AddReferenceOfRequest request
        ) =>
            new AddReferenceOfTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsUniTask<Result.AddReferenceOfResult>();
    #else
        public AddReferenceOfTask AddReferenceOfAsync(
                Request.AddReferenceOfRequest request
        )
        {
            return new AddReferenceOfTask(
                Gs2WebSocketSession,
                request
            );
        }
    #endif
#else
        public Task<Result.AddReferenceOfResult> AddReferenceOfAsync(
            Request.AddReferenceOfRequest request
        ) =>
            new AddReferenceOfTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsTask();
#endif


        public class AddReferenceOfByUserIdTask : Gs2WebSocketSessionTask<Request.AddReferenceOfByUserIdRequest, Result.AddReferenceOfByUserIdResult>
        {
            public AddReferenceOfByUserIdTask(IGs2Session session, Request.AddReferenceOfByUserIdRequest request) : base(session, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(Request.AddReferenceOfByUserIdRequest request)
            {
                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);

                jsonWriter.WriteObjectStart();

                if (request.NamespaceName != null)
                {
                    jsonWriter.WritePropertyName("namespaceName");
                    jsonWriter.Write(request.NamespaceName.ToString());
                }
                if (request.InventoryName != null)
                {
                    jsonWriter.WritePropertyName("inventoryName");
                    jsonWriter.Write(request.InventoryName.ToString());
                }
                if (request.UserId != null)
                {
                    jsonWriter.WritePropertyName("userId");
                    jsonWriter.Write(request.UserId.ToString());
                }
                if (request.ItemName != null)
                {
                    jsonWriter.WritePropertyName("itemName");
                    jsonWriter.Write(request.ItemName.ToString());
                }
                if (request.ItemSetName != null)
                {
                    jsonWriter.WritePropertyName("itemSetName");
                    jsonWriter.Write(request.ItemSetName.ToString());
                }
                if (request.ReferenceOf != null)
                {
                    jsonWriter.WritePropertyName("referenceOf");
                    jsonWriter.Write(request.ReferenceOf.ToString());
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
                    "inventory",
                    "referenceOf",
                    "addReferenceOfByUserId",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
        public IEnumerator AddReferenceOfByUserId(
                Request.AddReferenceOfByUserIdRequest request,
                UnityAction<AsyncResult<Result.AddReferenceOfByUserIdResult>> callback
        ) =>
            new AddReferenceOfByUserIdTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToCoroutine(callback);

        public IFuture<Result.AddReferenceOfByUserIdResult> AddReferenceOfByUserIdFuture(
                Request.AddReferenceOfByUserIdRequest request
        ) =>
            new AddReferenceOfByUserIdTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
        public UniTask<Result.AddReferenceOfByUserIdResult> AddReferenceOfByUserIdAsync(
            Request.AddReferenceOfByUserIdRequest request
        ) =>
            new AddReferenceOfByUserIdTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsUniTask<Result.AddReferenceOfByUserIdResult>();
    #else
        public AddReferenceOfByUserIdTask AddReferenceOfByUserIdAsync(
                Request.AddReferenceOfByUserIdRequest request
        )
        {
            return new AddReferenceOfByUserIdTask(
                Gs2WebSocketSession,
                request
            );
        }
    #endif
#else
        public Task<Result.AddReferenceOfByUserIdResult> AddReferenceOfByUserIdAsync(
            Request.AddReferenceOfByUserIdRequest request
        ) =>
            new AddReferenceOfByUserIdTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsTask();
#endif


        public class DeleteReferenceOfTask : Gs2WebSocketSessionTask<Request.DeleteReferenceOfRequest, Result.DeleteReferenceOfResult>
        {
            public DeleteReferenceOfTask(IGs2Session session, Request.DeleteReferenceOfRequest request) : base(session, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(Request.DeleteReferenceOfRequest request)
            {
                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);

                jsonWriter.WriteObjectStart();

                if (request.NamespaceName != null)
                {
                    jsonWriter.WritePropertyName("namespaceName");
                    jsonWriter.Write(request.NamespaceName.ToString());
                }
                if (request.InventoryName != null)
                {
                    jsonWriter.WritePropertyName("inventoryName");
                    jsonWriter.Write(request.InventoryName.ToString());
                }
                if (request.AccessToken != null)
                {
                    jsonWriter.WritePropertyName("accessToken");
                    jsonWriter.Write(request.AccessToken.ToString());
                }
                if (request.ItemName != null)
                {
                    jsonWriter.WritePropertyName("itemName");
                    jsonWriter.Write(request.ItemName.ToString());
                }
                if (request.ItemSetName != null)
                {
                    jsonWriter.WritePropertyName("itemSetName");
                    jsonWriter.Write(request.ItemSetName.ToString());
                }
                if (request.ReferenceOf != null)
                {
                    jsonWriter.WritePropertyName("referenceOf");
                    jsonWriter.Write(request.ReferenceOf.ToString());
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
                    "inventory",
                    "referenceOf",
                    "deleteReferenceOf",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
        public IEnumerator DeleteReferenceOf(
                Request.DeleteReferenceOfRequest request,
                UnityAction<AsyncResult<Result.DeleteReferenceOfResult>> callback
        ) =>
            new DeleteReferenceOfTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToCoroutine(callback);

        public IFuture<Result.DeleteReferenceOfResult> DeleteReferenceOfFuture(
                Request.DeleteReferenceOfRequest request
        ) =>
            new DeleteReferenceOfTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
        public UniTask<Result.DeleteReferenceOfResult> DeleteReferenceOfAsync(
            Request.DeleteReferenceOfRequest request
        ) =>
            new DeleteReferenceOfTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsUniTask<Result.DeleteReferenceOfResult>();
    #else
        public DeleteReferenceOfTask DeleteReferenceOfAsync(
                Request.DeleteReferenceOfRequest request
        )
        {
            return new DeleteReferenceOfTask(
                Gs2WebSocketSession,
                request
            );
        }
    #endif
#else
        public Task<Result.DeleteReferenceOfResult> DeleteReferenceOfAsync(
            Request.DeleteReferenceOfRequest request
        ) =>
            new DeleteReferenceOfTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsTask();
#endif


        public class DeleteReferenceOfByUserIdTask : Gs2WebSocketSessionTask<Request.DeleteReferenceOfByUserIdRequest, Result.DeleteReferenceOfByUserIdResult>
        {
            public DeleteReferenceOfByUserIdTask(IGs2Session session, Request.DeleteReferenceOfByUserIdRequest request) : base(session, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(Request.DeleteReferenceOfByUserIdRequest request)
            {
                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);

                jsonWriter.WriteObjectStart();

                if (request.NamespaceName != null)
                {
                    jsonWriter.WritePropertyName("namespaceName");
                    jsonWriter.Write(request.NamespaceName.ToString());
                }
                if (request.InventoryName != null)
                {
                    jsonWriter.WritePropertyName("inventoryName");
                    jsonWriter.Write(request.InventoryName.ToString());
                }
                if (request.UserId != null)
                {
                    jsonWriter.WritePropertyName("userId");
                    jsonWriter.Write(request.UserId.ToString());
                }
                if (request.ItemName != null)
                {
                    jsonWriter.WritePropertyName("itemName");
                    jsonWriter.Write(request.ItemName.ToString());
                }
                if (request.ItemSetName != null)
                {
                    jsonWriter.WritePropertyName("itemSetName");
                    jsonWriter.Write(request.ItemSetName.ToString());
                }
                if (request.ReferenceOf != null)
                {
                    jsonWriter.WritePropertyName("referenceOf");
                    jsonWriter.Write(request.ReferenceOf.ToString());
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
                    "inventory",
                    "referenceOf",
                    "deleteReferenceOfByUserId",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
        public IEnumerator DeleteReferenceOfByUserId(
                Request.DeleteReferenceOfByUserIdRequest request,
                UnityAction<AsyncResult<Result.DeleteReferenceOfByUserIdResult>> callback
        ) =>
            new DeleteReferenceOfByUserIdTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToCoroutine(callback);

        public IFuture<Result.DeleteReferenceOfByUserIdResult> DeleteReferenceOfByUserIdFuture(
                Request.DeleteReferenceOfByUserIdRequest request
        ) =>
            new DeleteReferenceOfByUserIdTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
        public UniTask<Result.DeleteReferenceOfByUserIdResult> DeleteReferenceOfByUserIdAsync(
            Request.DeleteReferenceOfByUserIdRequest request
        ) =>
            new DeleteReferenceOfByUserIdTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsUniTask<Result.DeleteReferenceOfByUserIdResult>();
    #else
        public DeleteReferenceOfByUserIdTask DeleteReferenceOfByUserIdAsync(
                Request.DeleteReferenceOfByUserIdRequest request
        )
        {
            return new DeleteReferenceOfByUserIdTask(
                Gs2WebSocketSession,
                request
            );
        }
    #endif
#else
        public Task<Result.DeleteReferenceOfByUserIdResult> DeleteReferenceOfByUserIdAsync(
            Request.DeleteReferenceOfByUserIdRequest request
        ) =>
            new DeleteReferenceOfByUserIdTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsTask();
#endif


        public class AddReferenceOfItemSetByStampSheetTask : Gs2WebSocketSessionTask<Request.AddReferenceOfItemSetByStampSheetRequest, Result.AddReferenceOfItemSetByStampSheetResult>
        {
            public AddReferenceOfItemSetByStampSheetTask(IGs2Session session, Request.AddReferenceOfItemSetByStampSheetRequest request) : base(session, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(Request.AddReferenceOfItemSetByStampSheetRequest request)
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
                    "inventory",
                    "referenceOf",
                    "addReferenceOfItemSetByStampSheet",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
        public IEnumerator AddReferenceOfItemSetByStampSheet(
                Request.AddReferenceOfItemSetByStampSheetRequest request,
                UnityAction<AsyncResult<Result.AddReferenceOfItemSetByStampSheetResult>> callback
        ) =>
            new AddReferenceOfItemSetByStampSheetTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToCoroutine(callback);

        public IFuture<Result.AddReferenceOfItemSetByStampSheetResult> AddReferenceOfItemSetByStampSheetFuture(
                Request.AddReferenceOfItemSetByStampSheetRequest request
        ) =>
            new AddReferenceOfItemSetByStampSheetTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
        public UniTask<Result.AddReferenceOfItemSetByStampSheetResult> AddReferenceOfItemSetByStampSheetAsync(
            Request.AddReferenceOfItemSetByStampSheetRequest request
        ) =>
            new AddReferenceOfItemSetByStampSheetTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsUniTask<Result.AddReferenceOfItemSetByStampSheetResult>();
    #else
        public AddReferenceOfItemSetByStampSheetTask AddReferenceOfItemSetByStampSheetAsync(
                Request.AddReferenceOfItemSetByStampSheetRequest request
        )
        {
            return new AddReferenceOfItemSetByStampSheetTask(
                Gs2WebSocketSession,
                request
            );
        }
    #endif
#else
        public Task<Result.AddReferenceOfItemSetByStampSheetResult> AddReferenceOfItemSetByStampSheetAsync(
            Request.AddReferenceOfItemSetByStampSheetRequest request
        ) =>
            new AddReferenceOfItemSetByStampSheetTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsTask();
#endif


        public class DeleteReferenceOfItemSetByStampSheetTask : Gs2WebSocketSessionTask<Request.DeleteReferenceOfItemSetByStampSheetRequest, Result.DeleteReferenceOfItemSetByStampSheetResult>
        {
            public DeleteReferenceOfItemSetByStampSheetTask(IGs2Session session, Request.DeleteReferenceOfItemSetByStampSheetRequest request) : base(session, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(Request.DeleteReferenceOfItemSetByStampSheetRequest request)
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
                    "inventory",
                    "referenceOf",
                    "deleteReferenceOfItemSetByStampSheet",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
        public IEnumerator DeleteReferenceOfItemSetByStampSheet(
                Request.DeleteReferenceOfItemSetByStampSheetRequest request,
                UnityAction<AsyncResult<Result.DeleteReferenceOfItemSetByStampSheetResult>> callback
        ) =>
            new DeleteReferenceOfItemSetByStampSheetTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToCoroutine(callback);

        public IFuture<Result.DeleteReferenceOfItemSetByStampSheetResult> DeleteReferenceOfItemSetByStampSheetFuture(
                Request.DeleteReferenceOfItemSetByStampSheetRequest request
        ) =>
            new DeleteReferenceOfItemSetByStampSheetTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
        public UniTask<Result.DeleteReferenceOfItemSetByStampSheetResult> DeleteReferenceOfItemSetByStampSheetAsync(
            Request.DeleteReferenceOfItemSetByStampSheetRequest request
        ) =>
            new DeleteReferenceOfItemSetByStampSheetTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsUniTask<Result.DeleteReferenceOfItemSetByStampSheetResult>();
    #else
        public DeleteReferenceOfItemSetByStampSheetTask DeleteReferenceOfItemSetByStampSheetAsync(
                Request.DeleteReferenceOfItemSetByStampSheetRequest request
        )
        {
            return new DeleteReferenceOfItemSetByStampSheetTask(
                Gs2WebSocketSession,
                request
            );
        }
    #endif
#else
        public Task<Result.DeleteReferenceOfItemSetByStampSheetResult> DeleteReferenceOfItemSetByStampSheetAsync(
            Request.DeleteReferenceOfItemSetByStampSheetRequest request
        ) =>
            new DeleteReferenceOfItemSetByStampSheetTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsTask();
#endif


        public class GetSimpleItemTask : Gs2WebSocketSessionTask<Request.GetSimpleItemRequest, Result.GetSimpleItemResult>
        {
            public GetSimpleItemTask(IGs2Session session, Request.GetSimpleItemRequest request) : base(session, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(Request.GetSimpleItemRequest request)
            {
                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);

                jsonWriter.WriteObjectStart();

                if (request.NamespaceName != null)
                {
                    jsonWriter.WritePropertyName("namespaceName");
                    jsonWriter.Write(request.NamespaceName.ToString());
                }
                if (request.InventoryName != null)
                {
                    jsonWriter.WritePropertyName("inventoryName");
                    jsonWriter.Write(request.InventoryName.ToString());
                }
                if (request.AccessToken != null)
                {
                    jsonWriter.WritePropertyName("accessToken");
                    jsonWriter.Write(request.AccessToken.ToString());
                }
                if (request.ItemName != null)
                {
                    jsonWriter.WritePropertyName("itemName");
                    jsonWriter.Write(request.ItemName.ToString());
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
                    "inventory",
                    "simpleItem",
                    "getSimpleItem",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
        public IEnumerator GetSimpleItem(
                Request.GetSimpleItemRequest request,
                UnityAction<AsyncResult<Result.GetSimpleItemResult>> callback
        ) =>
            new GetSimpleItemTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToCoroutine(callback);

        public IFuture<Result.GetSimpleItemResult> GetSimpleItemFuture(
                Request.GetSimpleItemRequest request
        ) =>
            new GetSimpleItemTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
        public UniTask<Result.GetSimpleItemResult> GetSimpleItemAsync(
            Request.GetSimpleItemRequest request
        ) =>
            new GetSimpleItemTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsUniTask<Result.GetSimpleItemResult>();
    #else
        public GetSimpleItemTask GetSimpleItemAsync(
                Request.GetSimpleItemRequest request
        )
        {
            return new GetSimpleItemTask(
                Gs2WebSocketSession,
                request
            );
        }
    #endif
#else
        public Task<Result.GetSimpleItemResult> GetSimpleItemAsync(
            Request.GetSimpleItemRequest request
        ) =>
            new GetSimpleItemTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsTask();
#endif


        public class GetSimpleItemByUserIdTask : Gs2WebSocketSessionTask<Request.GetSimpleItemByUserIdRequest, Result.GetSimpleItemByUserIdResult>
        {
            public GetSimpleItemByUserIdTask(IGs2Session session, Request.GetSimpleItemByUserIdRequest request) : base(session, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(Request.GetSimpleItemByUserIdRequest request)
            {
                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);

                jsonWriter.WriteObjectStart();

                if (request.NamespaceName != null)
                {
                    jsonWriter.WritePropertyName("namespaceName");
                    jsonWriter.Write(request.NamespaceName.ToString());
                }
                if (request.InventoryName != null)
                {
                    jsonWriter.WritePropertyName("inventoryName");
                    jsonWriter.Write(request.InventoryName.ToString());
                }
                if (request.UserId != null)
                {
                    jsonWriter.WritePropertyName("userId");
                    jsonWriter.Write(request.UserId.ToString());
                }
                if (request.ItemName != null)
                {
                    jsonWriter.WritePropertyName("itemName");
                    jsonWriter.Write(request.ItemName.ToString());
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
                    "inventory",
                    "simpleItem",
                    "getSimpleItemByUserId",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
        public IEnumerator GetSimpleItemByUserId(
                Request.GetSimpleItemByUserIdRequest request,
                UnityAction<AsyncResult<Result.GetSimpleItemByUserIdResult>> callback
        ) =>
            new GetSimpleItemByUserIdTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToCoroutine(callback);

        public IFuture<Result.GetSimpleItemByUserIdResult> GetSimpleItemByUserIdFuture(
                Request.GetSimpleItemByUserIdRequest request
        ) =>
            new GetSimpleItemByUserIdTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
        public UniTask<Result.GetSimpleItemByUserIdResult> GetSimpleItemByUserIdAsync(
            Request.GetSimpleItemByUserIdRequest request
        ) =>
            new GetSimpleItemByUserIdTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsUniTask<Result.GetSimpleItemByUserIdResult>();
    #else
        public GetSimpleItemByUserIdTask GetSimpleItemByUserIdAsync(
                Request.GetSimpleItemByUserIdRequest request
        )
        {
            return new GetSimpleItemByUserIdTask(
                Gs2WebSocketSession,
                request
            );
        }
    #endif
#else
        public Task<Result.GetSimpleItemByUserIdResult> GetSimpleItemByUserIdAsync(
            Request.GetSimpleItemByUserIdRequest request
        ) =>
            new GetSimpleItemByUserIdTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsTask();
#endif


        public class DeleteSimpleItemsByUserIdTask : Gs2WebSocketSessionTask<Request.DeleteSimpleItemsByUserIdRequest, Result.DeleteSimpleItemsByUserIdResult>
        {
            public DeleteSimpleItemsByUserIdTask(IGs2Session session, Request.DeleteSimpleItemsByUserIdRequest request) : base(session, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(Request.DeleteSimpleItemsByUserIdRequest request)
            {
                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);

                jsonWriter.WriteObjectStart();

                if (request.NamespaceName != null)
                {
                    jsonWriter.WritePropertyName("namespaceName");
                    jsonWriter.Write(request.NamespaceName.ToString());
                }
                if (request.InventoryName != null)
                {
                    jsonWriter.WritePropertyName("inventoryName");
                    jsonWriter.Write(request.InventoryName.ToString());
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
                    "inventory",
                    "simpleItem",
                    "deleteSimpleItemsByUserId",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
        public IEnumerator DeleteSimpleItemsByUserId(
                Request.DeleteSimpleItemsByUserIdRequest request,
                UnityAction<AsyncResult<Result.DeleteSimpleItemsByUserIdResult>> callback
        ) =>
            new DeleteSimpleItemsByUserIdTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToCoroutine(callback);

        public IFuture<Result.DeleteSimpleItemsByUserIdResult> DeleteSimpleItemsByUserIdFuture(
                Request.DeleteSimpleItemsByUserIdRequest request
        ) =>
            new DeleteSimpleItemsByUserIdTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
        public UniTask<Result.DeleteSimpleItemsByUserIdResult> DeleteSimpleItemsByUserIdAsync(
            Request.DeleteSimpleItemsByUserIdRequest request
        ) =>
            new DeleteSimpleItemsByUserIdTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsUniTask<Result.DeleteSimpleItemsByUserIdResult>();
    #else
        public DeleteSimpleItemsByUserIdTask DeleteSimpleItemsByUserIdAsync(
                Request.DeleteSimpleItemsByUserIdRequest request
        )
        {
            return new DeleteSimpleItemsByUserIdTask(
                Gs2WebSocketSession,
                request
            );
        }
    #endif
#else
        public Task<Result.DeleteSimpleItemsByUserIdResult> DeleteSimpleItemsByUserIdAsync(
            Request.DeleteSimpleItemsByUserIdRequest request
        ) =>
            new DeleteSimpleItemsByUserIdTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsTask();
#endif


        public class VerifySimpleItemTask : Gs2WebSocketSessionTask<Request.VerifySimpleItemRequest, Result.VerifySimpleItemResult>
        {
            public VerifySimpleItemTask(IGs2Session session, Request.VerifySimpleItemRequest request) : base(session, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(Request.VerifySimpleItemRequest request)
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
                if (request.InventoryName != null)
                {
                    jsonWriter.WritePropertyName("inventoryName");
                    jsonWriter.Write(request.InventoryName.ToString());
                }
                if (request.ItemName != null)
                {
                    jsonWriter.WritePropertyName("itemName");
                    jsonWriter.Write(request.ItemName.ToString());
                }
                if (request.VerifyType != null)
                {
                    jsonWriter.WritePropertyName("verifyType");
                    jsonWriter.Write(request.VerifyType.ToString());
                }
                if (request.Count != null)
                {
                    jsonWriter.WritePropertyName("count");
                    jsonWriter.Write(request.Count.ToString());
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
                    "inventory",
                    "simpleItem",
                    "verifySimpleItem",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
        public IEnumerator VerifySimpleItem(
                Request.VerifySimpleItemRequest request,
                UnityAction<AsyncResult<Result.VerifySimpleItemResult>> callback
        ) =>
            new VerifySimpleItemTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToCoroutine(callback);

        public IFuture<Result.VerifySimpleItemResult> VerifySimpleItemFuture(
                Request.VerifySimpleItemRequest request
        ) =>
            new VerifySimpleItemTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
        public UniTask<Result.VerifySimpleItemResult> VerifySimpleItemAsync(
            Request.VerifySimpleItemRequest request
        ) =>
            new VerifySimpleItemTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsUniTask<Result.VerifySimpleItemResult>();
    #else
        public VerifySimpleItemTask VerifySimpleItemAsync(
                Request.VerifySimpleItemRequest request
        )
        {
            return new VerifySimpleItemTask(
                Gs2WebSocketSession,
                request
            );
        }
    #endif
#else
        public Task<Result.VerifySimpleItemResult> VerifySimpleItemAsync(
            Request.VerifySimpleItemRequest request
        ) =>
            new VerifySimpleItemTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsTask();
#endif


        public class VerifySimpleItemByUserIdTask : Gs2WebSocketSessionTask<Request.VerifySimpleItemByUserIdRequest, Result.VerifySimpleItemByUserIdResult>
        {
            public VerifySimpleItemByUserIdTask(IGs2Session session, Request.VerifySimpleItemByUserIdRequest request) : base(session, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(Request.VerifySimpleItemByUserIdRequest request)
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
                if (request.InventoryName != null)
                {
                    jsonWriter.WritePropertyName("inventoryName");
                    jsonWriter.Write(request.InventoryName.ToString());
                }
                if (request.ItemName != null)
                {
                    jsonWriter.WritePropertyName("itemName");
                    jsonWriter.Write(request.ItemName.ToString());
                }
                if (request.VerifyType != null)
                {
                    jsonWriter.WritePropertyName("verifyType");
                    jsonWriter.Write(request.VerifyType.ToString());
                }
                if (request.Count != null)
                {
                    jsonWriter.WritePropertyName("count");
                    jsonWriter.Write(request.Count.ToString());
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
                    "inventory",
                    "simpleItem",
                    "verifySimpleItemByUserId",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
        public IEnumerator VerifySimpleItemByUserId(
                Request.VerifySimpleItemByUserIdRequest request,
                UnityAction<AsyncResult<Result.VerifySimpleItemByUserIdResult>> callback
        ) =>
            new VerifySimpleItemByUserIdTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToCoroutine(callback);

        public IFuture<Result.VerifySimpleItemByUserIdResult> VerifySimpleItemByUserIdFuture(
                Request.VerifySimpleItemByUserIdRequest request
        ) =>
            new VerifySimpleItemByUserIdTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
        public UniTask<Result.VerifySimpleItemByUserIdResult> VerifySimpleItemByUserIdAsync(
            Request.VerifySimpleItemByUserIdRequest request
        ) =>
            new VerifySimpleItemByUserIdTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsUniTask<Result.VerifySimpleItemByUserIdResult>();
    #else
        public VerifySimpleItemByUserIdTask VerifySimpleItemByUserIdAsync(
                Request.VerifySimpleItemByUserIdRequest request
        )
        {
            return new VerifySimpleItemByUserIdTask(
                Gs2WebSocketSession,
                request
            );
        }
    #endif
#else
        public Task<Result.VerifySimpleItemByUserIdResult> VerifySimpleItemByUserIdAsync(
            Request.VerifySimpleItemByUserIdRequest request
        ) =>
            new VerifySimpleItemByUserIdTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsTask();
#endif


        public class GetBigItemTask : Gs2WebSocketSessionTask<Request.GetBigItemRequest, Result.GetBigItemResult>
        {
            public GetBigItemTask(IGs2Session session, Request.GetBigItemRequest request) : base(session, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(Request.GetBigItemRequest request)
            {
                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);

                jsonWriter.WriteObjectStart();

                if (request.NamespaceName != null)
                {
                    jsonWriter.WritePropertyName("namespaceName");
                    jsonWriter.Write(request.NamespaceName.ToString());
                }
                if (request.InventoryName != null)
                {
                    jsonWriter.WritePropertyName("inventoryName");
                    jsonWriter.Write(request.InventoryName.ToString());
                }
                if (request.AccessToken != null)
                {
                    jsonWriter.WritePropertyName("accessToken");
                    jsonWriter.Write(request.AccessToken.ToString());
                }
                if (request.ItemName != null)
                {
                    jsonWriter.WritePropertyName("itemName");
                    jsonWriter.Write(request.ItemName.ToString());
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
                    "inventory",
                    "bigItem",
                    "getBigItem",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
        public IEnumerator GetBigItem(
                Request.GetBigItemRequest request,
                UnityAction<AsyncResult<Result.GetBigItemResult>> callback
        ) =>
            new GetBigItemTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToCoroutine(callback);

        public IFuture<Result.GetBigItemResult> GetBigItemFuture(
                Request.GetBigItemRequest request
        ) =>
            new GetBigItemTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
        public UniTask<Result.GetBigItemResult> GetBigItemAsync(
            Request.GetBigItemRequest request
        ) =>
            new GetBigItemTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsUniTask<Result.GetBigItemResult>();
    #else
        public GetBigItemTask GetBigItemAsync(
                Request.GetBigItemRequest request
        )
        {
            return new GetBigItemTask(
                Gs2WebSocketSession,
                request
            );
        }
    #endif
#else
        public Task<Result.GetBigItemResult> GetBigItemAsync(
            Request.GetBigItemRequest request
        ) =>
            new GetBigItemTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsTask();
#endif


        public class GetBigItemByUserIdTask : Gs2WebSocketSessionTask<Request.GetBigItemByUserIdRequest, Result.GetBigItemByUserIdResult>
        {
            public GetBigItemByUserIdTask(IGs2Session session, Request.GetBigItemByUserIdRequest request) : base(session, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(Request.GetBigItemByUserIdRequest request)
            {
                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);

                jsonWriter.WriteObjectStart();

                if (request.NamespaceName != null)
                {
                    jsonWriter.WritePropertyName("namespaceName");
                    jsonWriter.Write(request.NamespaceName.ToString());
                }
                if (request.InventoryName != null)
                {
                    jsonWriter.WritePropertyName("inventoryName");
                    jsonWriter.Write(request.InventoryName.ToString());
                }
                if (request.UserId != null)
                {
                    jsonWriter.WritePropertyName("userId");
                    jsonWriter.Write(request.UserId.ToString());
                }
                if (request.ItemName != null)
                {
                    jsonWriter.WritePropertyName("itemName");
                    jsonWriter.Write(request.ItemName.ToString());
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
                    "inventory",
                    "bigItem",
                    "getBigItemByUserId",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
        public IEnumerator GetBigItemByUserId(
                Request.GetBigItemByUserIdRequest request,
                UnityAction<AsyncResult<Result.GetBigItemByUserIdResult>> callback
        ) =>
            new GetBigItemByUserIdTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToCoroutine(callback);

        public IFuture<Result.GetBigItemByUserIdResult> GetBigItemByUserIdFuture(
                Request.GetBigItemByUserIdRequest request
        ) =>
            new GetBigItemByUserIdTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
        public UniTask<Result.GetBigItemByUserIdResult> GetBigItemByUserIdAsync(
            Request.GetBigItemByUserIdRequest request
        ) =>
            new GetBigItemByUserIdTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsUniTask<Result.GetBigItemByUserIdResult>();
    #else
        public GetBigItemByUserIdTask GetBigItemByUserIdAsync(
                Request.GetBigItemByUserIdRequest request
        )
        {
            return new GetBigItemByUserIdTask(
                Gs2WebSocketSession,
                request
            );
        }
    #endif
#else
        public Task<Result.GetBigItemByUserIdResult> GetBigItemByUserIdAsync(
            Request.GetBigItemByUserIdRequest request
        ) =>
            new GetBigItemByUserIdTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsTask();
#endif


        public class AcquireBigItemByUserIdTask : Gs2WebSocketSessionTask<Request.AcquireBigItemByUserIdRequest, Result.AcquireBigItemByUserIdResult>
        {
            public AcquireBigItemByUserIdTask(IGs2Session session, Request.AcquireBigItemByUserIdRequest request) : base(session, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(Request.AcquireBigItemByUserIdRequest request)
            {
                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);

                jsonWriter.WriteObjectStart();

                if (request.NamespaceName != null)
                {
                    jsonWriter.WritePropertyName("namespaceName");
                    jsonWriter.Write(request.NamespaceName.ToString());
                }
                if (request.InventoryName != null)
                {
                    jsonWriter.WritePropertyName("inventoryName");
                    jsonWriter.Write(request.InventoryName.ToString());
                }
                if (request.UserId != null)
                {
                    jsonWriter.WritePropertyName("userId");
                    jsonWriter.Write(request.UserId.ToString());
                }
                if (request.ItemName != null)
                {
                    jsonWriter.WritePropertyName("itemName");
                    jsonWriter.Write(request.ItemName.ToString());
                }
                if (request.AcquireCount != null)
                {
                    jsonWriter.WritePropertyName("acquireCount");
                    jsonWriter.Write(request.AcquireCount.ToString());
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
                    "inventory",
                    "bigItem",
                    "acquireBigItemByUserId",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }

            public override void OnError(Gs2.Core.Exception.Gs2Exception error)
            {
                if (error.Errors.Count(v => v.code == "itemSet.operation.conflict") > 0) {
                    base.OnError(new Exception.ConflictException(error));
                }
                else {
                    base.OnError(error);
                }
            }
        }

#if UNITY_2017_1_OR_NEWER
        public IEnumerator AcquireBigItemByUserId(
                Request.AcquireBigItemByUserIdRequest request,
                UnityAction<AsyncResult<Result.AcquireBigItemByUserIdResult>> callback
        ) =>
            new AcquireBigItemByUserIdTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToCoroutine(callback);

        public IFuture<Result.AcquireBigItemByUserIdResult> AcquireBigItemByUserIdFuture(
                Request.AcquireBigItemByUserIdRequest request
        ) =>
            new AcquireBigItemByUserIdTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
        public UniTask<Result.AcquireBigItemByUserIdResult> AcquireBigItemByUserIdAsync(
            Request.AcquireBigItemByUserIdRequest request
        ) =>
            new AcquireBigItemByUserIdTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsUniTask<Result.AcquireBigItemByUserIdResult>();
    #else
        public AcquireBigItemByUserIdTask AcquireBigItemByUserIdAsync(
                Request.AcquireBigItemByUserIdRequest request
        )
        {
            return new AcquireBigItemByUserIdTask(
                Gs2WebSocketSession,
                request
            );
        }
    #endif
#else
        public Task<Result.AcquireBigItemByUserIdResult> AcquireBigItemByUserIdAsync(
            Request.AcquireBigItemByUserIdRequest request
        ) =>
            new AcquireBigItemByUserIdTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsTask();
#endif


        public class ConsumeBigItemTask : Gs2WebSocketSessionTask<Request.ConsumeBigItemRequest, Result.ConsumeBigItemResult>
        {
            public ConsumeBigItemTask(IGs2Session session, Request.ConsumeBigItemRequest request) : base(session, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(Request.ConsumeBigItemRequest request)
            {
                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);

                jsonWriter.WriteObjectStart();

                if (request.NamespaceName != null)
                {
                    jsonWriter.WritePropertyName("namespaceName");
                    jsonWriter.Write(request.NamespaceName.ToString());
                }
                if (request.InventoryName != null)
                {
                    jsonWriter.WritePropertyName("inventoryName");
                    jsonWriter.Write(request.InventoryName.ToString());
                }
                if (request.AccessToken != null)
                {
                    jsonWriter.WritePropertyName("accessToken");
                    jsonWriter.Write(request.AccessToken.ToString());
                }
                if (request.ItemName != null)
                {
                    jsonWriter.WritePropertyName("itemName");
                    jsonWriter.Write(request.ItemName.ToString());
                }
                if (request.ConsumeCount != null)
                {
                    jsonWriter.WritePropertyName("consumeCount");
                    jsonWriter.Write(request.ConsumeCount.ToString());
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
                    "inventory",
                    "bigItem",
                    "consumeBigItem",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }

            public override void OnError(Gs2.Core.Exception.Gs2Exception error)
            {
                if (error.Errors.Count(v => v.code == "itemSet.operation.conflict") > 0) {
                    base.OnError(new Exception.ConflictException(error));
                }
                else if (error.Errors.Count(v => v.code == "itemSet.count.insufficient") > 0) {
                    base.OnError(new Exception.InsufficientException(error));
                }
                else {
                    base.OnError(error);
                }
            }
        }

#if UNITY_2017_1_OR_NEWER
        public IEnumerator ConsumeBigItem(
                Request.ConsumeBigItemRequest request,
                UnityAction<AsyncResult<Result.ConsumeBigItemResult>> callback
        ) =>
            new ConsumeBigItemTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToCoroutine(callback);

        public IFuture<Result.ConsumeBigItemResult> ConsumeBigItemFuture(
                Request.ConsumeBigItemRequest request
        ) =>
            new ConsumeBigItemTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
        public UniTask<Result.ConsumeBigItemResult> ConsumeBigItemAsync(
            Request.ConsumeBigItemRequest request
        ) =>
            new ConsumeBigItemTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsUniTask<Result.ConsumeBigItemResult>();
    #else
        public ConsumeBigItemTask ConsumeBigItemAsync(
                Request.ConsumeBigItemRequest request
        )
        {
            return new ConsumeBigItemTask(
                Gs2WebSocketSession,
                request
            );
        }
    #endif
#else
        public Task<Result.ConsumeBigItemResult> ConsumeBigItemAsync(
            Request.ConsumeBigItemRequest request
        ) =>
            new ConsumeBigItemTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsTask();
#endif


        public class ConsumeBigItemByUserIdTask : Gs2WebSocketSessionTask<Request.ConsumeBigItemByUserIdRequest, Result.ConsumeBigItemByUserIdResult>
        {
            public ConsumeBigItemByUserIdTask(IGs2Session session, Request.ConsumeBigItemByUserIdRequest request) : base(session, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(Request.ConsumeBigItemByUserIdRequest request)
            {
                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);

                jsonWriter.WriteObjectStart();

                if (request.NamespaceName != null)
                {
                    jsonWriter.WritePropertyName("namespaceName");
                    jsonWriter.Write(request.NamespaceName.ToString());
                }
                if (request.InventoryName != null)
                {
                    jsonWriter.WritePropertyName("inventoryName");
                    jsonWriter.Write(request.InventoryName.ToString());
                }
                if (request.UserId != null)
                {
                    jsonWriter.WritePropertyName("userId");
                    jsonWriter.Write(request.UserId.ToString());
                }
                if (request.ItemName != null)
                {
                    jsonWriter.WritePropertyName("itemName");
                    jsonWriter.Write(request.ItemName.ToString());
                }
                if (request.ConsumeCount != null)
                {
                    jsonWriter.WritePropertyName("consumeCount");
                    jsonWriter.Write(request.ConsumeCount.ToString());
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
                    "inventory",
                    "bigItem",
                    "consumeBigItemByUserId",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }

            public override void OnError(Gs2.Core.Exception.Gs2Exception error)
            {
                if (error.Errors.Count(v => v.code == "itemSet.operation.conflict") > 0) {
                    base.OnError(new Exception.ConflictException(error));
                }
                else if (error.Errors.Count(v => v.code == "itemSet.count.insufficient") > 0) {
                    base.OnError(new Exception.InsufficientException(error));
                }
                else {
                    base.OnError(error);
                }
            }
        }

#if UNITY_2017_1_OR_NEWER
        public IEnumerator ConsumeBigItemByUserId(
                Request.ConsumeBigItemByUserIdRequest request,
                UnityAction<AsyncResult<Result.ConsumeBigItemByUserIdResult>> callback
        ) =>
            new ConsumeBigItemByUserIdTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToCoroutine(callback);

        public IFuture<Result.ConsumeBigItemByUserIdResult> ConsumeBigItemByUserIdFuture(
                Request.ConsumeBigItemByUserIdRequest request
        ) =>
            new ConsumeBigItemByUserIdTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
        public UniTask<Result.ConsumeBigItemByUserIdResult> ConsumeBigItemByUserIdAsync(
            Request.ConsumeBigItemByUserIdRequest request
        ) =>
            new ConsumeBigItemByUserIdTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsUniTask<Result.ConsumeBigItemByUserIdResult>();
    #else
        public ConsumeBigItemByUserIdTask ConsumeBigItemByUserIdAsync(
                Request.ConsumeBigItemByUserIdRequest request
        )
        {
            return new ConsumeBigItemByUserIdTask(
                Gs2WebSocketSession,
                request
            );
        }
    #endif
#else
        public Task<Result.ConsumeBigItemByUserIdResult> ConsumeBigItemByUserIdAsync(
            Request.ConsumeBigItemByUserIdRequest request
        ) =>
            new ConsumeBigItemByUserIdTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsTask();
#endif


        public class SetBigItemByUserIdTask : Gs2WebSocketSessionTask<Request.SetBigItemByUserIdRequest, Result.SetBigItemByUserIdResult>
        {
            public SetBigItemByUserIdTask(IGs2Session session, Request.SetBigItemByUserIdRequest request) : base(session, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(Request.SetBigItemByUserIdRequest request)
            {
                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);

                jsonWriter.WriteObjectStart();

                if (request.NamespaceName != null)
                {
                    jsonWriter.WritePropertyName("namespaceName");
                    jsonWriter.Write(request.NamespaceName.ToString());
                }
                if (request.InventoryName != null)
                {
                    jsonWriter.WritePropertyName("inventoryName");
                    jsonWriter.Write(request.InventoryName.ToString());
                }
                if (request.UserId != null)
                {
                    jsonWriter.WritePropertyName("userId");
                    jsonWriter.Write(request.UserId.ToString());
                }
                if (request.ItemName != null)
                {
                    jsonWriter.WritePropertyName("itemName");
                    jsonWriter.Write(request.ItemName.ToString());
                }
                if (request.Count != null)
                {
                    jsonWriter.WritePropertyName("count");
                    jsonWriter.Write(request.Count.ToString());
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
                    "inventory",
                    "bigItem",
                    "setBigItemByUserId",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }

            public override void OnError(Gs2.Core.Exception.Gs2Exception error)
            {
                if (error.Errors.Count(v => v.code == "itemSet.operation.conflict") > 0) {
                    base.OnError(new Exception.ConflictException(error));
                }
                else {
                    base.OnError(error);
                }
            }
        }

#if UNITY_2017_1_OR_NEWER
        public IEnumerator SetBigItemByUserId(
                Request.SetBigItemByUserIdRequest request,
                UnityAction<AsyncResult<Result.SetBigItemByUserIdResult>> callback
        ) =>
            new SetBigItemByUserIdTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToCoroutine(callback);

        public IFuture<Result.SetBigItemByUserIdResult> SetBigItemByUserIdFuture(
                Request.SetBigItemByUserIdRequest request
        ) =>
            new SetBigItemByUserIdTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
        public UniTask<Result.SetBigItemByUserIdResult> SetBigItemByUserIdAsync(
            Request.SetBigItemByUserIdRequest request
        ) =>
            new SetBigItemByUserIdTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsUniTask<Result.SetBigItemByUserIdResult>();
    #else
        public SetBigItemByUserIdTask SetBigItemByUserIdAsync(
                Request.SetBigItemByUserIdRequest request
        )
        {
            return new SetBigItemByUserIdTask(
                Gs2WebSocketSession,
                request
            );
        }
    #endif
#else
        public Task<Result.SetBigItemByUserIdResult> SetBigItemByUserIdAsync(
            Request.SetBigItemByUserIdRequest request
        ) =>
            new SetBigItemByUserIdTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsTask();
#endif


        public class DeleteBigItemByUserIdTask : Gs2WebSocketSessionTask<Request.DeleteBigItemByUserIdRequest, Result.DeleteBigItemByUserIdResult>
        {
            public DeleteBigItemByUserIdTask(IGs2Session session, Request.DeleteBigItemByUserIdRequest request) : base(session, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(Request.DeleteBigItemByUserIdRequest request)
            {
                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);

                jsonWriter.WriteObjectStart();

                if (request.NamespaceName != null)
                {
                    jsonWriter.WritePropertyName("namespaceName");
                    jsonWriter.Write(request.NamespaceName.ToString());
                }
                if (request.InventoryName != null)
                {
                    jsonWriter.WritePropertyName("inventoryName");
                    jsonWriter.Write(request.InventoryName.ToString());
                }
                if (request.UserId != null)
                {
                    jsonWriter.WritePropertyName("userId");
                    jsonWriter.Write(request.UserId.ToString());
                }
                if (request.ItemName != null)
                {
                    jsonWriter.WritePropertyName("itemName");
                    jsonWriter.Write(request.ItemName.ToString());
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
                    "inventory",
                    "bigItem",
                    "deleteBigItemByUserId",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
        public IEnumerator DeleteBigItemByUserId(
                Request.DeleteBigItemByUserIdRequest request,
                UnityAction<AsyncResult<Result.DeleteBigItemByUserIdResult>> callback
        ) =>
            new DeleteBigItemByUserIdTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToCoroutine(callback);

        public IFuture<Result.DeleteBigItemByUserIdResult> DeleteBigItemByUserIdFuture(
                Request.DeleteBigItemByUserIdRequest request
        ) =>
            new DeleteBigItemByUserIdTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
        public UniTask<Result.DeleteBigItemByUserIdResult> DeleteBigItemByUserIdAsync(
            Request.DeleteBigItemByUserIdRequest request
        ) =>
            new DeleteBigItemByUserIdTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsUniTask<Result.DeleteBigItemByUserIdResult>();
    #else
        public DeleteBigItemByUserIdTask DeleteBigItemByUserIdAsync(
                Request.DeleteBigItemByUserIdRequest request
        )
        {
            return new DeleteBigItemByUserIdTask(
                Gs2WebSocketSession,
                request
            );
        }
    #endif
#else
        public Task<Result.DeleteBigItemByUserIdResult> DeleteBigItemByUserIdAsync(
            Request.DeleteBigItemByUserIdRequest request
        ) =>
            new DeleteBigItemByUserIdTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsTask();
#endif


        public class VerifyBigItemTask : Gs2WebSocketSessionTask<Request.VerifyBigItemRequest, Result.VerifyBigItemResult>
        {
            public VerifyBigItemTask(IGs2Session session, Request.VerifyBigItemRequest request) : base(session, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(Request.VerifyBigItemRequest request)
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
                if (request.InventoryName != null)
                {
                    jsonWriter.WritePropertyName("inventoryName");
                    jsonWriter.Write(request.InventoryName.ToString());
                }
                if (request.ItemName != null)
                {
                    jsonWriter.WritePropertyName("itemName");
                    jsonWriter.Write(request.ItemName.ToString());
                }
                if (request.VerifyType != null)
                {
                    jsonWriter.WritePropertyName("verifyType");
                    jsonWriter.Write(request.VerifyType.ToString());
                }
                if (request.Count != null)
                {
                    jsonWriter.WritePropertyName("count");
                    jsonWriter.Write(request.Count.ToString());
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
                    "inventory",
                    "bigItem",
                    "verifyBigItem",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
        public IEnumerator VerifyBigItem(
                Request.VerifyBigItemRequest request,
                UnityAction<AsyncResult<Result.VerifyBigItemResult>> callback
        ) =>
            new VerifyBigItemTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToCoroutine(callback);

        public IFuture<Result.VerifyBigItemResult> VerifyBigItemFuture(
                Request.VerifyBigItemRequest request
        ) =>
            new VerifyBigItemTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
        public UniTask<Result.VerifyBigItemResult> VerifyBigItemAsync(
            Request.VerifyBigItemRequest request
        ) =>
            new VerifyBigItemTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsUniTask<Result.VerifyBigItemResult>();
    #else
        public VerifyBigItemTask VerifyBigItemAsync(
                Request.VerifyBigItemRequest request
        )
        {
            return new VerifyBigItemTask(
                Gs2WebSocketSession,
                request
            );
        }
    #endif
#else
        public Task<Result.VerifyBigItemResult> VerifyBigItemAsync(
            Request.VerifyBigItemRequest request
        ) =>
            new VerifyBigItemTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsTask();
#endif


        public class VerifyBigItemByUserIdTask : Gs2WebSocketSessionTask<Request.VerifyBigItemByUserIdRequest, Result.VerifyBigItemByUserIdResult>
        {
            public VerifyBigItemByUserIdTask(IGs2Session session, Request.VerifyBigItemByUserIdRequest request) : base(session, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(Request.VerifyBigItemByUserIdRequest request)
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
                if (request.InventoryName != null)
                {
                    jsonWriter.WritePropertyName("inventoryName");
                    jsonWriter.Write(request.InventoryName.ToString());
                }
                if (request.ItemName != null)
                {
                    jsonWriter.WritePropertyName("itemName");
                    jsonWriter.Write(request.ItemName.ToString());
                }
                if (request.VerifyType != null)
                {
                    jsonWriter.WritePropertyName("verifyType");
                    jsonWriter.Write(request.VerifyType.ToString());
                }
                if (request.Count != null)
                {
                    jsonWriter.WritePropertyName("count");
                    jsonWriter.Write(request.Count.ToString());
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
                    "inventory",
                    "bigItem",
                    "verifyBigItemByUserId",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
        public IEnumerator VerifyBigItemByUserId(
                Request.VerifyBigItemByUserIdRequest request,
                UnityAction<AsyncResult<Result.VerifyBigItemByUserIdResult>> callback
        ) =>
            new VerifyBigItemByUserIdTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToCoroutine(callback);

        public IFuture<Result.VerifyBigItemByUserIdResult> VerifyBigItemByUserIdFuture(
                Request.VerifyBigItemByUserIdRequest request
        ) =>
            new VerifyBigItemByUserIdTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
        public UniTask<Result.VerifyBigItemByUserIdResult> VerifyBigItemByUserIdAsync(
            Request.VerifyBigItemByUserIdRequest request
        ) =>
            new VerifyBigItemByUserIdTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsUniTask<Result.VerifyBigItemByUserIdResult>();
    #else
        public VerifyBigItemByUserIdTask VerifyBigItemByUserIdAsync(
                Request.VerifyBigItemByUserIdRequest request
        )
        {
            return new VerifyBigItemByUserIdTask(
                Gs2WebSocketSession,
                request
            );
        }
    #endif
#else
        public Task<Result.VerifyBigItemByUserIdResult> VerifyBigItemByUserIdAsync(
            Request.VerifyBigItemByUserIdRequest request
        ) =>
            new VerifyBigItemByUserIdTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsTask();
#endif


        public class AcquireBigItemByStampSheetTask : Gs2WebSocketSessionTask<Request.AcquireBigItemByStampSheetRequest, Result.AcquireBigItemByStampSheetResult>
        {
            public AcquireBigItemByStampSheetTask(IGs2Session session, Request.AcquireBigItemByStampSheetRequest request) : base(session, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(Request.AcquireBigItemByStampSheetRequest request)
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
                    "inventory",
                    "bigItem",
                    "acquireBigItemByStampSheet",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
        public IEnumerator AcquireBigItemByStampSheet(
                Request.AcquireBigItemByStampSheetRequest request,
                UnityAction<AsyncResult<Result.AcquireBigItemByStampSheetResult>> callback
        ) =>
            new AcquireBigItemByStampSheetTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToCoroutine(callback);

        public IFuture<Result.AcquireBigItemByStampSheetResult> AcquireBigItemByStampSheetFuture(
                Request.AcquireBigItemByStampSheetRequest request
        ) =>
            new AcquireBigItemByStampSheetTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
        public UniTask<Result.AcquireBigItemByStampSheetResult> AcquireBigItemByStampSheetAsync(
            Request.AcquireBigItemByStampSheetRequest request
        ) =>
            new AcquireBigItemByStampSheetTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsUniTask<Result.AcquireBigItemByStampSheetResult>();
    #else
        public AcquireBigItemByStampSheetTask AcquireBigItemByStampSheetAsync(
                Request.AcquireBigItemByStampSheetRequest request
        )
        {
            return new AcquireBigItemByStampSheetTask(
                Gs2WebSocketSession,
                request
            );
        }
    #endif
#else
        public Task<Result.AcquireBigItemByStampSheetResult> AcquireBigItemByStampSheetAsync(
            Request.AcquireBigItemByStampSheetRequest request
        ) =>
            new AcquireBigItemByStampSheetTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsTask();
#endif


        public class SetBigItemByStampSheetTask : Gs2WebSocketSessionTask<Request.SetBigItemByStampSheetRequest, Result.SetBigItemByStampSheetResult>
        {
            public SetBigItemByStampSheetTask(IGs2Session session, Request.SetBigItemByStampSheetRequest request) : base(session, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(Request.SetBigItemByStampSheetRequest request)
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
                    "inventory",
                    "bigItem",
                    "setBigItemByStampSheet",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
        public IEnumerator SetBigItemByStampSheet(
                Request.SetBigItemByStampSheetRequest request,
                UnityAction<AsyncResult<Result.SetBigItemByStampSheetResult>> callback
        ) =>
            new SetBigItemByStampSheetTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToCoroutine(callback);

        public IFuture<Result.SetBigItemByStampSheetResult> SetBigItemByStampSheetFuture(
                Request.SetBigItemByStampSheetRequest request
        ) =>
            new SetBigItemByStampSheetTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
        public UniTask<Result.SetBigItemByStampSheetResult> SetBigItemByStampSheetAsync(
            Request.SetBigItemByStampSheetRequest request
        ) =>
            new SetBigItemByStampSheetTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsUniTask<Result.SetBigItemByStampSheetResult>();
    #else
        public SetBigItemByStampSheetTask SetBigItemByStampSheetAsync(
                Request.SetBigItemByStampSheetRequest request
        )
        {
            return new SetBigItemByStampSheetTask(
                Gs2WebSocketSession,
                request
            );
        }
    #endif
#else
        public Task<Result.SetBigItemByStampSheetResult> SetBigItemByStampSheetAsync(
            Request.SetBigItemByStampSheetRequest request
        ) =>
            new SetBigItemByStampSheetTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsTask();
#endif
	}
}