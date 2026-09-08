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

namespace Gs2.Gs2Enchant
{
	public class Gs2EnchantWebSocketClient : AbstractGs2Client
	{

		public static string Endpoint = "enchant";

        protected Gs2WebSocketSession Gs2WebSocketSession => (Gs2WebSocketSession) Gs2Session;

		public Gs2EnchantWebSocketClient(Gs2WebSocketSession Gs2WebSocketSession) : base(Gs2WebSocketSession)
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
                    "enchant",
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
                    "enchant",
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
                    "enchant",
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
                    "enchant",
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
                    "enchant",
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
                    "enchant",
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
                    "enchant",
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
                    "enchant",
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
                    "enchant",
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
                    "enchant",
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
                    "enchant",
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
                    "enchant",
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
                    "enchant",
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


        public class GetBalanceParameterModelTask : Gs2WebSocketSessionTask<Request.GetBalanceParameterModelRequest, Result.GetBalanceParameterModelResult>
        {
            public GetBalanceParameterModelTask(IGs2Session session, Request.GetBalanceParameterModelRequest request) : base(session, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(Request.GetBalanceParameterModelRequest request)
            {
                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);

                jsonWriter.WriteObjectStart();

                if (request.NamespaceName != null)
                {
                    jsonWriter.WritePropertyName("namespaceName");
                    jsonWriter.Write(request.NamespaceName.ToString());
                }
                if (request.ParameterName != null)
                {
                    jsonWriter.WritePropertyName("parameterName");
                    jsonWriter.Write(request.ParameterName.ToString());
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
                    "enchant",
                    "balanceParameterModel",
                    "getBalanceParameterModel",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
        public IEnumerator GetBalanceParameterModel(
                Request.GetBalanceParameterModelRequest request,
                UnityAction<AsyncResult<Result.GetBalanceParameterModelResult>> callback
        ) =>
            new GetBalanceParameterModelTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToCoroutine(callback);

        public IFuture<Result.GetBalanceParameterModelResult> GetBalanceParameterModelFuture(
                Request.GetBalanceParameterModelRequest request
        ) =>
            new GetBalanceParameterModelTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
        public UniTask<Result.GetBalanceParameterModelResult> GetBalanceParameterModelAsync(
            Request.GetBalanceParameterModelRequest request
        ) =>
            new GetBalanceParameterModelTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsUniTask<Result.GetBalanceParameterModelResult>();
    #else
        public GetBalanceParameterModelTask GetBalanceParameterModelAsync(
                Request.GetBalanceParameterModelRequest request
        )
        {
            return new GetBalanceParameterModelTask(
                Gs2WebSocketSession,
                request
            );
        }
    #endif
#else
        public Task<Result.GetBalanceParameterModelResult> GetBalanceParameterModelAsync(
            Request.GetBalanceParameterModelRequest request
        ) =>
            new GetBalanceParameterModelTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsTask();
#endif


        public class CreateBalanceParameterModelMasterTask : Gs2WebSocketSessionTask<Request.CreateBalanceParameterModelMasterRequest, Result.CreateBalanceParameterModelMasterResult>
        {
            public CreateBalanceParameterModelMasterTask(IGs2Session session, Request.CreateBalanceParameterModelMasterRequest request) : base(session, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(Request.CreateBalanceParameterModelMasterRequest request)
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
                if (request.TotalValue != null)
                {
                    jsonWriter.WritePropertyName("totalValue");
                    jsonWriter.Write(request.TotalValue.ToString());
                }
                if (request.InitialValueStrategy != null)
                {
                    jsonWriter.WritePropertyName("initialValueStrategy");
                    jsonWriter.Write(request.InitialValueStrategy.ToString());
                }
                if (request.Parameters != null)
                {
                    jsonWriter.WritePropertyName("parameters");
                    jsonWriter.WriteArrayStart();
                    foreach(var item in request.Parameters)
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
                    "enchant",
                    "balanceParameterModelMaster",
                    "createBalanceParameterModelMaster",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
        public IEnumerator CreateBalanceParameterModelMaster(
                Request.CreateBalanceParameterModelMasterRequest request,
                UnityAction<AsyncResult<Result.CreateBalanceParameterModelMasterResult>> callback
        ) =>
            new CreateBalanceParameterModelMasterTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToCoroutine(callback);

        public IFuture<Result.CreateBalanceParameterModelMasterResult> CreateBalanceParameterModelMasterFuture(
                Request.CreateBalanceParameterModelMasterRequest request
        ) =>
            new CreateBalanceParameterModelMasterTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
        public UniTask<Result.CreateBalanceParameterModelMasterResult> CreateBalanceParameterModelMasterAsync(
            Request.CreateBalanceParameterModelMasterRequest request
        ) =>
            new CreateBalanceParameterModelMasterTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsUniTask<Result.CreateBalanceParameterModelMasterResult>();
    #else
        public CreateBalanceParameterModelMasterTask CreateBalanceParameterModelMasterAsync(
                Request.CreateBalanceParameterModelMasterRequest request
        )
        {
            return new CreateBalanceParameterModelMasterTask(
                Gs2WebSocketSession,
                request
            );
        }
    #endif
#else
        public Task<Result.CreateBalanceParameterModelMasterResult> CreateBalanceParameterModelMasterAsync(
            Request.CreateBalanceParameterModelMasterRequest request
        ) =>
            new CreateBalanceParameterModelMasterTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsTask();
#endif


        public class GetBalanceParameterModelMasterTask : Gs2WebSocketSessionTask<Request.GetBalanceParameterModelMasterRequest, Result.GetBalanceParameterModelMasterResult>
        {
            public GetBalanceParameterModelMasterTask(IGs2Session session, Request.GetBalanceParameterModelMasterRequest request) : base(session, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(Request.GetBalanceParameterModelMasterRequest request)
            {
                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);

                jsonWriter.WriteObjectStart();

                if (request.NamespaceName != null)
                {
                    jsonWriter.WritePropertyName("namespaceName");
                    jsonWriter.Write(request.NamespaceName.ToString());
                }
                if (request.ParameterName != null)
                {
                    jsonWriter.WritePropertyName("parameterName");
                    jsonWriter.Write(request.ParameterName.ToString());
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
                    "enchant",
                    "balanceParameterModelMaster",
                    "getBalanceParameterModelMaster",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
        public IEnumerator GetBalanceParameterModelMaster(
                Request.GetBalanceParameterModelMasterRequest request,
                UnityAction<AsyncResult<Result.GetBalanceParameterModelMasterResult>> callback
        ) =>
            new GetBalanceParameterModelMasterTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToCoroutine(callback);

        public IFuture<Result.GetBalanceParameterModelMasterResult> GetBalanceParameterModelMasterFuture(
                Request.GetBalanceParameterModelMasterRequest request
        ) =>
            new GetBalanceParameterModelMasterTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
        public UniTask<Result.GetBalanceParameterModelMasterResult> GetBalanceParameterModelMasterAsync(
            Request.GetBalanceParameterModelMasterRequest request
        ) =>
            new GetBalanceParameterModelMasterTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsUniTask<Result.GetBalanceParameterModelMasterResult>();
    #else
        public GetBalanceParameterModelMasterTask GetBalanceParameterModelMasterAsync(
                Request.GetBalanceParameterModelMasterRequest request
        )
        {
            return new GetBalanceParameterModelMasterTask(
                Gs2WebSocketSession,
                request
            );
        }
    #endif
#else
        public Task<Result.GetBalanceParameterModelMasterResult> GetBalanceParameterModelMasterAsync(
            Request.GetBalanceParameterModelMasterRequest request
        ) =>
            new GetBalanceParameterModelMasterTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsTask();
#endif


        public class UpdateBalanceParameterModelMasterTask : Gs2WebSocketSessionTask<Request.UpdateBalanceParameterModelMasterRequest, Result.UpdateBalanceParameterModelMasterResult>
        {
            public UpdateBalanceParameterModelMasterTask(IGs2Session session, Request.UpdateBalanceParameterModelMasterRequest request) : base(session, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(Request.UpdateBalanceParameterModelMasterRequest request)
            {
                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);

                jsonWriter.WriteObjectStart();

                if (request.NamespaceName != null)
                {
                    jsonWriter.WritePropertyName("namespaceName");
                    jsonWriter.Write(request.NamespaceName.ToString());
                }
                if (request.ParameterName != null)
                {
                    jsonWriter.WritePropertyName("parameterName");
                    jsonWriter.Write(request.ParameterName.ToString());
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
                if (request.TotalValue != null)
                {
                    jsonWriter.WritePropertyName("totalValue");
                    jsonWriter.Write(request.TotalValue.ToString());
                }
                if (request.InitialValueStrategy != null)
                {
                    jsonWriter.WritePropertyName("initialValueStrategy");
                    jsonWriter.Write(request.InitialValueStrategy.ToString());
                }
                if (request.Parameters != null)
                {
                    jsonWriter.WritePropertyName("parameters");
                    jsonWriter.WriteArrayStart();
                    foreach(var item in request.Parameters)
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
                    "enchant",
                    "balanceParameterModelMaster",
                    "updateBalanceParameterModelMaster",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
        public IEnumerator UpdateBalanceParameterModelMaster(
                Request.UpdateBalanceParameterModelMasterRequest request,
                UnityAction<AsyncResult<Result.UpdateBalanceParameterModelMasterResult>> callback
        ) =>
            new UpdateBalanceParameterModelMasterTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToCoroutine(callback);

        public IFuture<Result.UpdateBalanceParameterModelMasterResult> UpdateBalanceParameterModelMasterFuture(
                Request.UpdateBalanceParameterModelMasterRequest request
        ) =>
            new UpdateBalanceParameterModelMasterTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
        public UniTask<Result.UpdateBalanceParameterModelMasterResult> UpdateBalanceParameterModelMasterAsync(
            Request.UpdateBalanceParameterModelMasterRequest request
        ) =>
            new UpdateBalanceParameterModelMasterTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsUniTask<Result.UpdateBalanceParameterModelMasterResult>();
    #else
        public UpdateBalanceParameterModelMasterTask UpdateBalanceParameterModelMasterAsync(
                Request.UpdateBalanceParameterModelMasterRequest request
        )
        {
            return new UpdateBalanceParameterModelMasterTask(
                Gs2WebSocketSession,
                request
            );
        }
    #endif
#else
        public Task<Result.UpdateBalanceParameterModelMasterResult> UpdateBalanceParameterModelMasterAsync(
            Request.UpdateBalanceParameterModelMasterRequest request
        ) =>
            new UpdateBalanceParameterModelMasterTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsTask();
#endif


        public class DeleteBalanceParameterModelMasterTask : Gs2WebSocketSessionTask<Request.DeleteBalanceParameterModelMasterRequest, Result.DeleteBalanceParameterModelMasterResult>
        {
            public DeleteBalanceParameterModelMasterTask(IGs2Session session, Request.DeleteBalanceParameterModelMasterRequest request) : base(session, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(Request.DeleteBalanceParameterModelMasterRequest request)
            {
                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);

                jsonWriter.WriteObjectStart();

                if (request.NamespaceName != null)
                {
                    jsonWriter.WritePropertyName("namespaceName");
                    jsonWriter.Write(request.NamespaceName.ToString());
                }
                if (request.ParameterName != null)
                {
                    jsonWriter.WritePropertyName("parameterName");
                    jsonWriter.Write(request.ParameterName.ToString());
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
                    "enchant",
                    "balanceParameterModelMaster",
                    "deleteBalanceParameterModelMaster",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
        public IEnumerator DeleteBalanceParameterModelMaster(
                Request.DeleteBalanceParameterModelMasterRequest request,
                UnityAction<AsyncResult<Result.DeleteBalanceParameterModelMasterResult>> callback
        ) =>
            new DeleteBalanceParameterModelMasterTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToCoroutine(callback);

        public IFuture<Result.DeleteBalanceParameterModelMasterResult> DeleteBalanceParameterModelMasterFuture(
                Request.DeleteBalanceParameterModelMasterRequest request
        ) =>
            new DeleteBalanceParameterModelMasterTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
        public UniTask<Result.DeleteBalanceParameterModelMasterResult> DeleteBalanceParameterModelMasterAsync(
            Request.DeleteBalanceParameterModelMasterRequest request
        ) =>
            new DeleteBalanceParameterModelMasterTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsUniTask<Result.DeleteBalanceParameterModelMasterResult>();
    #else
        public DeleteBalanceParameterModelMasterTask DeleteBalanceParameterModelMasterAsync(
                Request.DeleteBalanceParameterModelMasterRequest request
        )
        {
            return new DeleteBalanceParameterModelMasterTask(
                Gs2WebSocketSession,
                request
            );
        }
    #endif
#else
        public Task<Result.DeleteBalanceParameterModelMasterResult> DeleteBalanceParameterModelMasterAsync(
            Request.DeleteBalanceParameterModelMasterRequest request
        ) =>
            new DeleteBalanceParameterModelMasterTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsTask();
#endif


        public class PreUpdateCurrentParameterMasterTask : Gs2WebSocketSessionTask<Request.PreUpdateCurrentParameterMasterRequest, Result.PreUpdateCurrentParameterMasterResult>
        {
            public PreUpdateCurrentParameterMasterTask(IGs2Session session, Request.PreUpdateCurrentParameterMasterRequest request) : base(session, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(Request.PreUpdateCurrentParameterMasterRequest request)
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
                    "enchant",
                    "currentParameterMaster",
                    "preUpdateCurrentParameterMaster",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
        public IEnumerator PreUpdateCurrentParameterMaster(
                Request.PreUpdateCurrentParameterMasterRequest request,
                UnityAction<AsyncResult<Result.PreUpdateCurrentParameterMasterResult>> callback
        ) =>
            new PreUpdateCurrentParameterMasterTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToCoroutine(callback);

        public IFuture<Result.PreUpdateCurrentParameterMasterResult> PreUpdateCurrentParameterMasterFuture(
                Request.PreUpdateCurrentParameterMasterRequest request
        ) =>
            new PreUpdateCurrentParameterMasterTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
        public UniTask<Result.PreUpdateCurrentParameterMasterResult> PreUpdateCurrentParameterMasterAsync(
            Request.PreUpdateCurrentParameterMasterRequest request
        ) =>
            new PreUpdateCurrentParameterMasterTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsUniTask<Result.PreUpdateCurrentParameterMasterResult>();
    #else
        public PreUpdateCurrentParameterMasterTask PreUpdateCurrentParameterMasterAsync(
                Request.PreUpdateCurrentParameterMasterRequest request
        )
        {
            return new PreUpdateCurrentParameterMasterTask(
                Gs2WebSocketSession,
                request
            );
        }
    #endif
#else
        public Task<Result.PreUpdateCurrentParameterMasterResult> PreUpdateCurrentParameterMasterAsync(
            Request.PreUpdateCurrentParameterMasterRequest request
        ) =>
            new PreUpdateCurrentParameterMasterTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsTask();
#endif


        public class GetBalanceParameterStatusTask : Gs2WebSocketSessionTask<Request.GetBalanceParameterStatusRequest, Result.GetBalanceParameterStatusResult>
        {
            public GetBalanceParameterStatusTask(IGs2Session session, Request.GetBalanceParameterStatusRequest request) : base(session, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(Request.GetBalanceParameterStatusRequest request)
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
                if (request.ParameterName != null)
                {
                    jsonWriter.WritePropertyName("parameterName");
                    jsonWriter.Write(request.ParameterName.ToString());
                }
                if (request.PropertyId != null)
                {
                    jsonWriter.WritePropertyName("propertyId");
                    jsonWriter.Write(request.PropertyId.ToString());
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
                    "enchant",
                    "balanceParameterStatus",
                    "getBalanceParameterStatus",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
        public IEnumerator GetBalanceParameterStatus(
                Request.GetBalanceParameterStatusRequest request,
                UnityAction<AsyncResult<Result.GetBalanceParameterStatusResult>> callback
        ) =>
            new GetBalanceParameterStatusTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToCoroutine(callback);

        public IFuture<Result.GetBalanceParameterStatusResult> GetBalanceParameterStatusFuture(
                Request.GetBalanceParameterStatusRequest request
        ) =>
            new GetBalanceParameterStatusTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
        public UniTask<Result.GetBalanceParameterStatusResult> GetBalanceParameterStatusAsync(
            Request.GetBalanceParameterStatusRequest request
        ) =>
            new GetBalanceParameterStatusTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsUniTask<Result.GetBalanceParameterStatusResult>();
    #else
        public GetBalanceParameterStatusTask GetBalanceParameterStatusAsync(
                Request.GetBalanceParameterStatusRequest request
        )
        {
            return new GetBalanceParameterStatusTask(
                Gs2WebSocketSession,
                request
            );
        }
    #endif
#else
        public Task<Result.GetBalanceParameterStatusResult> GetBalanceParameterStatusAsync(
            Request.GetBalanceParameterStatusRequest request
        ) =>
            new GetBalanceParameterStatusTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsTask();
#endif


        public class GetBalanceParameterStatusByUserIdTask : Gs2WebSocketSessionTask<Request.GetBalanceParameterStatusByUserIdRequest, Result.GetBalanceParameterStatusByUserIdResult>
        {
            public GetBalanceParameterStatusByUserIdTask(IGs2Session session, Request.GetBalanceParameterStatusByUserIdRequest request) : base(session, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(Request.GetBalanceParameterStatusByUserIdRequest request)
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
                if (request.ParameterName != null)
                {
                    jsonWriter.WritePropertyName("parameterName");
                    jsonWriter.Write(request.ParameterName.ToString());
                }
                if (request.PropertyId != null)
                {
                    jsonWriter.WritePropertyName("propertyId");
                    jsonWriter.Write(request.PropertyId.ToString());
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
                    "enchant",
                    "balanceParameterStatus",
                    "getBalanceParameterStatusByUserId",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
        public IEnumerator GetBalanceParameterStatusByUserId(
                Request.GetBalanceParameterStatusByUserIdRequest request,
                UnityAction<AsyncResult<Result.GetBalanceParameterStatusByUserIdResult>> callback
        ) =>
            new GetBalanceParameterStatusByUserIdTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToCoroutine(callback);

        public IFuture<Result.GetBalanceParameterStatusByUserIdResult> GetBalanceParameterStatusByUserIdFuture(
                Request.GetBalanceParameterStatusByUserIdRequest request
        ) =>
            new GetBalanceParameterStatusByUserIdTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
        public UniTask<Result.GetBalanceParameterStatusByUserIdResult> GetBalanceParameterStatusByUserIdAsync(
            Request.GetBalanceParameterStatusByUserIdRequest request
        ) =>
            new GetBalanceParameterStatusByUserIdTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsUniTask<Result.GetBalanceParameterStatusByUserIdResult>();
    #else
        public GetBalanceParameterStatusByUserIdTask GetBalanceParameterStatusByUserIdAsync(
                Request.GetBalanceParameterStatusByUserIdRequest request
        )
        {
            return new GetBalanceParameterStatusByUserIdTask(
                Gs2WebSocketSession,
                request
            );
        }
    #endif
#else
        public Task<Result.GetBalanceParameterStatusByUserIdResult> GetBalanceParameterStatusByUserIdAsync(
            Request.GetBalanceParameterStatusByUserIdRequest request
        ) =>
            new GetBalanceParameterStatusByUserIdTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsTask();
#endif


        public class DeleteBalanceParameterStatusByUserIdTask : Gs2WebSocketSessionTask<Request.DeleteBalanceParameterStatusByUserIdRequest, Result.DeleteBalanceParameterStatusByUserIdResult>
        {
            public DeleteBalanceParameterStatusByUserIdTask(IGs2Session session, Request.DeleteBalanceParameterStatusByUserIdRequest request) : base(session, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(Request.DeleteBalanceParameterStatusByUserIdRequest request)
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
                if (request.ParameterName != null)
                {
                    jsonWriter.WritePropertyName("parameterName");
                    jsonWriter.Write(request.ParameterName.ToString());
                }
                if (request.PropertyId != null)
                {
                    jsonWriter.WritePropertyName("propertyId");
                    jsonWriter.Write(request.PropertyId.ToString());
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
                    "enchant",
                    "balanceParameterStatus",
                    "deleteBalanceParameterStatusByUserId",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
        public IEnumerator DeleteBalanceParameterStatusByUserId(
                Request.DeleteBalanceParameterStatusByUserIdRequest request,
                UnityAction<AsyncResult<Result.DeleteBalanceParameterStatusByUserIdResult>> callback
        ) =>
            new DeleteBalanceParameterStatusByUserIdTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToCoroutine(callback);

        public IFuture<Result.DeleteBalanceParameterStatusByUserIdResult> DeleteBalanceParameterStatusByUserIdFuture(
                Request.DeleteBalanceParameterStatusByUserIdRequest request
        ) =>
            new DeleteBalanceParameterStatusByUserIdTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
        public UniTask<Result.DeleteBalanceParameterStatusByUserIdResult> DeleteBalanceParameterStatusByUserIdAsync(
            Request.DeleteBalanceParameterStatusByUserIdRequest request
        ) =>
            new DeleteBalanceParameterStatusByUserIdTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsUniTask<Result.DeleteBalanceParameterStatusByUserIdResult>();
    #else
        public DeleteBalanceParameterStatusByUserIdTask DeleteBalanceParameterStatusByUserIdAsync(
                Request.DeleteBalanceParameterStatusByUserIdRequest request
        )
        {
            return new DeleteBalanceParameterStatusByUserIdTask(
                Gs2WebSocketSession,
                request
            );
        }
    #endif
#else
        public Task<Result.DeleteBalanceParameterStatusByUserIdResult> DeleteBalanceParameterStatusByUserIdAsync(
            Request.DeleteBalanceParameterStatusByUserIdRequest request
        ) =>
            new DeleteBalanceParameterStatusByUserIdTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsTask();
#endif


        public class ReDrawBalanceParameterStatusByUserIdTask : Gs2WebSocketSessionTask<Request.ReDrawBalanceParameterStatusByUserIdRequest, Result.ReDrawBalanceParameterStatusByUserIdResult>
        {
            public ReDrawBalanceParameterStatusByUserIdTask(IGs2Session session, Request.ReDrawBalanceParameterStatusByUserIdRequest request) : base(session, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(Request.ReDrawBalanceParameterStatusByUserIdRequest request)
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
                if (request.ParameterName != null)
                {
                    jsonWriter.WritePropertyName("parameterName");
                    jsonWriter.Write(request.ParameterName.ToString());
                }
                if (request.PropertyId != null)
                {
                    jsonWriter.WritePropertyName("propertyId");
                    jsonWriter.Write(request.PropertyId.ToString());
                }
                if (request.FixedParameterNames != null)
                {
                    jsonWriter.WritePropertyName("fixedParameterNames");
                    jsonWriter.WriteArrayStart();
                    foreach(var item in request.FixedParameterNames)
                    {
                        jsonWriter.Write(item);
                    }
                    jsonWriter.WriteArrayEnd();
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
                    "enchant",
                    "balanceParameterStatus",
                    "reDrawBalanceParameterStatusByUserId",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
        public IEnumerator ReDrawBalanceParameterStatusByUserId(
                Request.ReDrawBalanceParameterStatusByUserIdRequest request,
                UnityAction<AsyncResult<Result.ReDrawBalanceParameterStatusByUserIdResult>> callback
        ) =>
            new ReDrawBalanceParameterStatusByUserIdTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToCoroutine(callback);

        public IFuture<Result.ReDrawBalanceParameterStatusByUserIdResult> ReDrawBalanceParameterStatusByUserIdFuture(
                Request.ReDrawBalanceParameterStatusByUserIdRequest request
        ) =>
            new ReDrawBalanceParameterStatusByUserIdTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
        public UniTask<Result.ReDrawBalanceParameterStatusByUserIdResult> ReDrawBalanceParameterStatusByUserIdAsync(
            Request.ReDrawBalanceParameterStatusByUserIdRequest request
        ) =>
            new ReDrawBalanceParameterStatusByUserIdTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsUniTask<Result.ReDrawBalanceParameterStatusByUserIdResult>();
    #else
        public ReDrawBalanceParameterStatusByUserIdTask ReDrawBalanceParameterStatusByUserIdAsync(
                Request.ReDrawBalanceParameterStatusByUserIdRequest request
        )
        {
            return new ReDrawBalanceParameterStatusByUserIdTask(
                Gs2WebSocketSession,
                request
            );
        }
    #endif
#else
        public Task<Result.ReDrawBalanceParameterStatusByUserIdResult> ReDrawBalanceParameterStatusByUserIdAsync(
            Request.ReDrawBalanceParameterStatusByUserIdRequest request
        ) =>
            new ReDrawBalanceParameterStatusByUserIdTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsTask();
#endif


        public class ReDrawBalanceParameterStatusByStampSheetTask : Gs2WebSocketSessionTask<Request.ReDrawBalanceParameterStatusByStampSheetRequest, Result.ReDrawBalanceParameterStatusByStampSheetResult>
        {
            public ReDrawBalanceParameterStatusByStampSheetTask(IGs2Session session, Request.ReDrawBalanceParameterStatusByStampSheetRequest request) : base(session, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(Request.ReDrawBalanceParameterStatusByStampSheetRequest request)
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
                    "enchant",
                    "balanceParameterStatus",
                    "reDrawBalanceParameterStatusByStampSheet",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
        public IEnumerator ReDrawBalanceParameterStatusByStampSheet(
                Request.ReDrawBalanceParameterStatusByStampSheetRequest request,
                UnityAction<AsyncResult<Result.ReDrawBalanceParameterStatusByStampSheetResult>> callback
        ) =>
            new ReDrawBalanceParameterStatusByStampSheetTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToCoroutine(callback);

        public IFuture<Result.ReDrawBalanceParameterStatusByStampSheetResult> ReDrawBalanceParameterStatusByStampSheetFuture(
                Request.ReDrawBalanceParameterStatusByStampSheetRequest request
        ) =>
            new ReDrawBalanceParameterStatusByStampSheetTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
        public UniTask<Result.ReDrawBalanceParameterStatusByStampSheetResult> ReDrawBalanceParameterStatusByStampSheetAsync(
            Request.ReDrawBalanceParameterStatusByStampSheetRequest request
        ) =>
            new ReDrawBalanceParameterStatusByStampSheetTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsUniTask<Result.ReDrawBalanceParameterStatusByStampSheetResult>();
    #else
        public ReDrawBalanceParameterStatusByStampSheetTask ReDrawBalanceParameterStatusByStampSheetAsync(
                Request.ReDrawBalanceParameterStatusByStampSheetRequest request
        )
        {
            return new ReDrawBalanceParameterStatusByStampSheetTask(
                Gs2WebSocketSession,
                request
            );
        }
    #endif
#else
        public Task<Result.ReDrawBalanceParameterStatusByStampSheetResult> ReDrawBalanceParameterStatusByStampSheetAsync(
            Request.ReDrawBalanceParameterStatusByStampSheetRequest request
        ) =>
            new ReDrawBalanceParameterStatusByStampSheetTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsTask();
#endif


        public class SetBalanceParameterStatusByUserIdTask : Gs2WebSocketSessionTask<Request.SetBalanceParameterStatusByUserIdRequest, Result.SetBalanceParameterStatusByUserIdResult>
        {
            public SetBalanceParameterStatusByUserIdTask(IGs2Session session, Request.SetBalanceParameterStatusByUserIdRequest request) : base(session, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(Request.SetBalanceParameterStatusByUserIdRequest request)
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
                if (request.ParameterName != null)
                {
                    jsonWriter.WritePropertyName("parameterName");
                    jsonWriter.Write(request.ParameterName.ToString());
                }
                if (request.PropertyId != null)
                {
                    jsonWriter.WritePropertyName("propertyId");
                    jsonWriter.Write(request.PropertyId.ToString());
                }
                if (request.ParameterValues != null)
                {
                    jsonWriter.WritePropertyName("parameterValues");
                    jsonWriter.WriteArrayStart();
                    foreach(var item in request.ParameterValues)
                    {
                        item.WriteJson(jsonWriter);
                    }
                    jsonWriter.WriteArrayEnd();
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
                    "enchant",
                    "balanceParameterStatus",
                    "setBalanceParameterStatusByUserId",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
        public IEnumerator SetBalanceParameterStatusByUserId(
                Request.SetBalanceParameterStatusByUserIdRequest request,
                UnityAction<AsyncResult<Result.SetBalanceParameterStatusByUserIdResult>> callback
        ) =>
            new SetBalanceParameterStatusByUserIdTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToCoroutine(callback);

        public IFuture<Result.SetBalanceParameterStatusByUserIdResult> SetBalanceParameterStatusByUserIdFuture(
                Request.SetBalanceParameterStatusByUserIdRequest request
        ) =>
            new SetBalanceParameterStatusByUserIdTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
        public UniTask<Result.SetBalanceParameterStatusByUserIdResult> SetBalanceParameterStatusByUserIdAsync(
            Request.SetBalanceParameterStatusByUserIdRequest request
        ) =>
            new SetBalanceParameterStatusByUserIdTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsUniTask<Result.SetBalanceParameterStatusByUserIdResult>();
    #else
        public SetBalanceParameterStatusByUserIdTask SetBalanceParameterStatusByUserIdAsync(
                Request.SetBalanceParameterStatusByUserIdRequest request
        )
        {
            return new SetBalanceParameterStatusByUserIdTask(
                Gs2WebSocketSession,
                request
            );
        }
    #endif
#else
        public Task<Result.SetBalanceParameterStatusByUserIdResult> SetBalanceParameterStatusByUserIdAsync(
            Request.SetBalanceParameterStatusByUserIdRequest request
        ) =>
            new SetBalanceParameterStatusByUserIdTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsTask();
#endif


        public class SetBalanceParameterStatusByStampSheetTask : Gs2WebSocketSessionTask<Request.SetBalanceParameterStatusByStampSheetRequest, Result.SetBalanceParameterStatusByStampSheetResult>
        {
            public SetBalanceParameterStatusByStampSheetTask(IGs2Session session, Request.SetBalanceParameterStatusByStampSheetRequest request) : base(session, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(Request.SetBalanceParameterStatusByStampSheetRequest request)
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
                    "enchant",
                    "balanceParameterStatus",
                    "setBalanceParameterStatusByStampSheet",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
        public IEnumerator SetBalanceParameterStatusByStampSheet(
                Request.SetBalanceParameterStatusByStampSheetRequest request,
                UnityAction<AsyncResult<Result.SetBalanceParameterStatusByStampSheetResult>> callback
        ) =>
            new SetBalanceParameterStatusByStampSheetTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToCoroutine(callback);

        public IFuture<Result.SetBalanceParameterStatusByStampSheetResult> SetBalanceParameterStatusByStampSheetFuture(
                Request.SetBalanceParameterStatusByStampSheetRequest request
        ) =>
            new SetBalanceParameterStatusByStampSheetTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
        public UniTask<Result.SetBalanceParameterStatusByStampSheetResult> SetBalanceParameterStatusByStampSheetAsync(
            Request.SetBalanceParameterStatusByStampSheetRequest request
        ) =>
            new SetBalanceParameterStatusByStampSheetTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsUniTask<Result.SetBalanceParameterStatusByStampSheetResult>();
    #else
        public SetBalanceParameterStatusByStampSheetTask SetBalanceParameterStatusByStampSheetAsync(
                Request.SetBalanceParameterStatusByStampSheetRequest request
        )
        {
            return new SetBalanceParameterStatusByStampSheetTask(
                Gs2WebSocketSession,
                request
            );
        }
    #endif
#else
        public Task<Result.SetBalanceParameterStatusByStampSheetResult> SetBalanceParameterStatusByStampSheetAsync(
            Request.SetBalanceParameterStatusByStampSheetRequest request
        ) =>
            new SetBalanceParameterStatusByStampSheetTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsTask();
#endif


        public class GetRarityParameterStatusTask : Gs2WebSocketSessionTask<Request.GetRarityParameterStatusRequest, Result.GetRarityParameterStatusResult>
        {
            public GetRarityParameterStatusTask(IGs2Session session, Request.GetRarityParameterStatusRequest request) : base(session, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(Request.GetRarityParameterStatusRequest request)
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
                if (request.ParameterName != null)
                {
                    jsonWriter.WritePropertyName("parameterName");
                    jsonWriter.Write(request.ParameterName.ToString());
                }
                if (request.PropertyId != null)
                {
                    jsonWriter.WritePropertyName("propertyId");
                    jsonWriter.Write(request.PropertyId.ToString());
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
                    "enchant",
                    "rarityParameterStatus",
                    "getRarityParameterStatus",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
        public IEnumerator GetRarityParameterStatus(
                Request.GetRarityParameterStatusRequest request,
                UnityAction<AsyncResult<Result.GetRarityParameterStatusResult>> callback
        ) =>
            new GetRarityParameterStatusTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToCoroutine(callback);

        public IFuture<Result.GetRarityParameterStatusResult> GetRarityParameterStatusFuture(
                Request.GetRarityParameterStatusRequest request
        ) =>
            new GetRarityParameterStatusTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
        public UniTask<Result.GetRarityParameterStatusResult> GetRarityParameterStatusAsync(
            Request.GetRarityParameterStatusRequest request
        ) =>
            new GetRarityParameterStatusTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsUniTask<Result.GetRarityParameterStatusResult>();
    #else
        public GetRarityParameterStatusTask GetRarityParameterStatusAsync(
                Request.GetRarityParameterStatusRequest request
        )
        {
            return new GetRarityParameterStatusTask(
                Gs2WebSocketSession,
                request
            );
        }
    #endif
#else
        public Task<Result.GetRarityParameterStatusResult> GetRarityParameterStatusAsync(
            Request.GetRarityParameterStatusRequest request
        ) =>
            new GetRarityParameterStatusTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsTask();
#endif


        public class GetRarityParameterStatusByUserIdTask : Gs2WebSocketSessionTask<Request.GetRarityParameterStatusByUserIdRequest, Result.GetRarityParameterStatusByUserIdResult>
        {
            public GetRarityParameterStatusByUserIdTask(IGs2Session session, Request.GetRarityParameterStatusByUserIdRequest request) : base(session, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(Request.GetRarityParameterStatusByUserIdRequest request)
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
                if (request.ParameterName != null)
                {
                    jsonWriter.WritePropertyName("parameterName");
                    jsonWriter.Write(request.ParameterName.ToString());
                }
                if (request.PropertyId != null)
                {
                    jsonWriter.WritePropertyName("propertyId");
                    jsonWriter.Write(request.PropertyId.ToString());
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
                    "enchant",
                    "rarityParameterStatus",
                    "getRarityParameterStatusByUserId",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
        public IEnumerator GetRarityParameterStatusByUserId(
                Request.GetRarityParameterStatusByUserIdRequest request,
                UnityAction<AsyncResult<Result.GetRarityParameterStatusByUserIdResult>> callback
        ) =>
            new GetRarityParameterStatusByUserIdTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToCoroutine(callback);

        public IFuture<Result.GetRarityParameterStatusByUserIdResult> GetRarityParameterStatusByUserIdFuture(
                Request.GetRarityParameterStatusByUserIdRequest request
        ) =>
            new GetRarityParameterStatusByUserIdTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
        public UniTask<Result.GetRarityParameterStatusByUserIdResult> GetRarityParameterStatusByUserIdAsync(
            Request.GetRarityParameterStatusByUserIdRequest request
        ) =>
            new GetRarityParameterStatusByUserIdTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsUniTask<Result.GetRarityParameterStatusByUserIdResult>();
    #else
        public GetRarityParameterStatusByUserIdTask GetRarityParameterStatusByUserIdAsync(
                Request.GetRarityParameterStatusByUserIdRequest request
        )
        {
            return new GetRarityParameterStatusByUserIdTask(
                Gs2WebSocketSession,
                request
            );
        }
    #endif
#else
        public Task<Result.GetRarityParameterStatusByUserIdResult> GetRarityParameterStatusByUserIdAsync(
            Request.GetRarityParameterStatusByUserIdRequest request
        ) =>
            new GetRarityParameterStatusByUserIdTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsTask();
#endif


        public class DeleteRarityParameterStatusByUserIdTask : Gs2WebSocketSessionTask<Request.DeleteRarityParameterStatusByUserIdRequest, Result.DeleteRarityParameterStatusByUserIdResult>
        {
            public DeleteRarityParameterStatusByUserIdTask(IGs2Session session, Request.DeleteRarityParameterStatusByUserIdRequest request) : base(session, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(Request.DeleteRarityParameterStatusByUserIdRequest request)
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
                if (request.ParameterName != null)
                {
                    jsonWriter.WritePropertyName("parameterName");
                    jsonWriter.Write(request.ParameterName.ToString());
                }
                if (request.PropertyId != null)
                {
                    jsonWriter.WritePropertyName("propertyId");
                    jsonWriter.Write(request.PropertyId.ToString());
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
                    "enchant",
                    "rarityParameterStatus",
                    "deleteRarityParameterStatusByUserId",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
        public IEnumerator DeleteRarityParameterStatusByUserId(
                Request.DeleteRarityParameterStatusByUserIdRequest request,
                UnityAction<AsyncResult<Result.DeleteRarityParameterStatusByUserIdResult>> callback
        ) =>
            new DeleteRarityParameterStatusByUserIdTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToCoroutine(callback);

        public IFuture<Result.DeleteRarityParameterStatusByUserIdResult> DeleteRarityParameterStatusByUserIdFuture(
                Request.DeleteRarityParameterStatusByUserIdRequest request
        ) =>
            new DeleteRarityParameterStatusByUserIdTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
        public UniTask<Result.DeleteRarityParameterStatusByUserIdResult> DeleteRarityParameterStatusByUserIdAsync(
            Request.DeleteRarityParameterStatusByUserIdRequest request
        ) =>
            new DeleteRarityParameterStatusByUserIdTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsUniTask<Result.DeleteRarityParameterStatusByUserIdResult>();
    #else
        public DeleteRarityParameterStatusByUserIdTask DeleteRarityParameterStatusByUserIdAsync(
                Request.DeleteRarityParameterStatusByUserIdRequest request
        )
        {
            return new DeleteRarityParameterStatusByUserIdTask(
                Gs2WebSocketSession,
                request
            );
        }
    #endif
#else
        public Task<Result.DeleteRarityParameterStatusByUserIdResult> DeleteRarityParameterStatusByUserIdAsync(
            Request.DeleteRarityParameterStatusByUserIdRequest request
        ) =>
            new DeleteRarityParameterStatusByUserIdTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsTask();
#endif


        public class ReDrawRarityParameterStatusByUserIdTask : Gs2WebSocketSessionTask<Request.ReDrawRarityParameterStatusByUserIdRequest, Result.ReDrawRarityParameterStatusByUserIdResult>
        {
            public ReDrawRarityParameterStatusByUserIdTask(IGs2Session session, Request.ReDrawRarityParameterStatusByUserIdRequest request) : base(session, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(Request.ReDrawRarityParameterStatusByUserIdRequest request)
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
                if (request.ParameterName != null)
                {
                    jsonWriter.WritePropertyName("parameterName");
                    jsonWriter.Write(request.ParameterName.ToString());
                }
                if (request.PropertyId != null)
                {
                    jsonWriter.WritePropertyName("propertyId");
                    jsonWriter.Write(request.PropertyId.ToString());
                }
                if (request.FixedParameterNames != null)
                {
                    jsonWriter.WritePropertyName("fixedParameterNames");
                    jsonWriter.WriteArrayStart();
                    foreach(var item in request.FixedParameterNames)
                    {
                        jsonWriter.Write(item);
                    }
                    jsonWriter.WriteArrayEnd();
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
                    "enchant",
                    "rarityParameterStatus",
                    "reDrawRarityParameterStatusByUserId",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
        public IEnumerator ReDrawRarityParameterStatusByUserId(
                Request.ReDrawRarityParameterStatusByUserIdRequest request,
                UnityAction<AsyncResult<Result.ReDrawRarityParameterStatusByUserIdResult>> callback
        ) =>
            new ReDrawRarityParameterStatusByUserIdTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToCoroutine(callback);

        public IFuture<Result.ReDrawRarityParameterStatusByUserIdResult> ReDrawRarityParameterStatusByUserIdFuture(
                Request.ReDrawRarityParameterStatusByUserIdRequest request
        ) =>
            new ReDrawRarityParameterStatusByUserIdTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
        public UniTask<Result.ReDrawRarityParameterStatusByUserIdResult> ReDrawRarityParameterStatusByUserIdAsync(
            Request.ReDrawRarityParameterStatusByUserIdRequest request
        ) =>
            new ReDrawRarityParameterStatusByUserIdTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsUniTask<Result.ReDrawRarityParameterStatusByUserIdResult>();
    #else
        public ReDrawRarityParameterStatusByUserIdTask ReDrawRarityParameterStatusByUserIdAsync(
                Request.ReDrawRarityParameterStatusByUserIdRequest request
        )
        {
            return new ReDrawRarityParameterStatusByUserIdTask(
                Gs2WebSocketSession,
                request
            );
        }
    #endif
#else
        public Task<Result.ReDrawRarityParameterStatusByUserIdResult> ReDrawRarityParameterStatusByUserIdAsync(
            Request.ReDrawRarityParameterStatusByUserIdRequest request
        ) =>
            new ReDrawRarityParameterStatusByUserIdTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsTask();
#endif


        public class ReDrawRarityParameterStatusByStampSheetTask : Gs2WebSocketSessionTask<Request.ReDrawRarityParameterStatusByStampSheetRequest, Result.ReDrawRarityParameterStatusByStampSheetResult>
        {
            public ReDrawRarityParameterStatusByStampSheetTask(IGs2Session session, Request.ReDrawRarityParameterStatusByStampSheetRequest request) : base(session, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(Request.ReDrawRarityParameterStatusByStampSheetRequest request)
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
                    "enchant",
                    "rarityParameterStatus",
                    "reDrawRarityParameterStatusByStampSheet",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
        public IEnumerator ReDrawRarityParameterStatusByStampSheet(
                Request.ReDrawRarityParameterStatusByStampSheetRequest request,
                UnityAction<AsyncResult<Result.ReDrawRarityParameterStatusByStampSheetResult>> callback
        ) =>
            new ReDrawRarityParameterStatusByStampSheetTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToCoroutine(callback);

        public IFuture<Result.ReDrawRarityParameterStatusByStampSheetResult> ReDrawRarityParameterStatusByStampSheetFuture(
                Request.ReDrawRarityParameterStatusByStampSheetRequest request
        ) =>
            new ReDrawRarityParameterStatusByStampSheetTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
        public UniTask<Result.ReDrawRarityParameterStatusByStampSheetResult> ReDrawRarityParameterStatusByStampSheetAsync(
            Request.ReDrawRarityParameterStatusByStampSheetRequest request
        ) =>
            new ReDrawRarityParameterStatusByStampSheetTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsUniTask<Result.ReDrawRarityParameterStatusByStampSheetResult>();
    #else
        public ReDrawRarityParameterStatusByStampSheetTask ReDrawRarityParameterStatusByStampSheetAsync(
                Request.ReDrawRarityParameterStatusByStampSheetRequest request
        )
        {
            return new ReDrawRarityParameterStatusByStampSheetTask(
                Gs2WebSocketSession,
                request
            );
        }
    #endif
#else
        public Task<Result.ReDrawRarityParameterStatusByStampSheetResult> ReDrawRarityParameterStatusByStampSheetAsync(
            Request.ReDrawRarityParameterStatusByStampSheetRequest request
        ) =>
            new ReDrawRarityParameterStatusByStampSheetTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsTask();
#endif


        public class AddRarityParameterStatusByUserIdTask : Gs2WebSocketSessionTask<Request.AddRarityParameterStatusByUserIdRequest, Result.AddRarityParameterStatusByUserIdResult>
        {
            public AddRarityParameterStatusByUserIdTask(IGs2Session session, Request.AddRarityParameterStatusByUserIdRequest request) : base(session, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(Request.AddRarityParameterStatusByUserIdRequest request)
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
                if (request.ParameterName != null)
                {
                    jsonWriter.WritePropertyName("parameterName");
                    jsonWriter.Write(request.ParameterName.ToString());
                }
                if (request.PropertyId != null)
                {
                    jsonWriter.WritePropertyName("propertyId");
                    jsonWriter.Write(request.PropertyId.ToString());
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
                    "enchant",
                    "rarityParameterStatus",
                    "addRarityParameterStatusByUserId",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
        public IEnumerator AddRarityParameterStatusByUserId(
                Request.AddRarityParameterStatusByUserIdRequest request,
                UnityAction<AsyncResult<Result.AddRarityParameterStatusByUserIdResult>> callback
        ) =>
            new AddRarityParameterStatusByUserIdTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToCoroutine(callback);

        public IFuture<Result.AddRarityParameterStatusByUserIdResult> AddRarityParameterStatusByUserIdFuture(
                Request.AddRarityParameterStatusByUserIdRequest request
        ) =>
            new AddRarityParameterStatusByUserIdTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
        public UniTask<Result.AddRarityParameterStatusByUserIdResult> AddRarityParameterStatusByUserIdAsync(
            Request.AddRarityParameterStatusByUserIdRequest request
        ) =>
            new AddRarityParameterStatusByUserIdTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsUniTask<Result.AddRarityParameterStatusByUserIdResult>();
    #else
        public AddRarityParameterStatusByUserIdTask AddRarityParameterStatusByUserIdAsync(
                Request.AddRarityParameterStatusByUserIdRequest request
        )
        {
            return new AddRarityParameterStatusByUserIdTask(
                Gs2WebSocketSession,
                request
            );
        }
    #endif
#else
        public Task<Result.AddRarityParameterStatusByUserIdResult> AddRarityParameterStatusByUserIdAsync(
            Request.AddRarityParameterStatusByUserIdRequest request
        ) =>
            new AddRarityParameterStatusByUserIdTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsTask();
#endif


        public class AddRarityParameterStatusByStampSheetTask : Gs2WebSocketSessionTask<Request.AddRarityParameterStatusByStampSheetRequest, Result.AddRarityParameterStatusByStampSheetResult>
        {
            public AddRarityParameterStatusByStampSheetTask(IGs2Session session, Request.AddRarityParameterStatusByStampSheetRequest request) : base(session, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(Request.AddRarityParameterStatusByStampSheetRequest request)
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
                    "enchant",
                    "rarityParameterStatus",
                    "addRarityParameterStatusByStampSheet",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
        public IEnumerator AddRarityParameterStatusByStampSheet(
                Request.AddRarityParameterStatusByStampSheetRequest request,
                UnityAction<AsyncResult<Result.AddRarityParameterStatusByStampSheetResult>> callback
        ) =>
            new AddRarityParameterStatusByStampSheetTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToCoroutine(callback);

        public IFuture<Result.AddRarityParameterStatusByStampSheetResult> AddRarityParameterStatusByStampSheetFuture(
                Request.AddRarityParameterStatusByStampSheetRequest request
        ) =>
            new AddRarityParameterStatusByStampSheetTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
        public UniTask<Result.AddRarityParameterStatusByStampSheetResult> AddRarityParameterStatusByStampSheetAsync(
            Request.AddRarityParameterStatusByStampSheetRequest request
        ) =>
            new AddRarityParameterStatusByStampSheetTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsUniTask<Result.AddRarityParameterStatusByStampSheetResult>();
    #else
        public AddRarityParameterStatusByStampSheetTask AddRarityParameterStatusByStampSheetAsync(
                Request.AddRarityParameterStatusByStampSheetRequest request
        )
        {
            return new AddRarityParameterStatusByStampSheetTask(
                Gs2WebSocketSession,
                request
            );
        }
    #endif
#else
        public Task<Result.AddRarityParameterStatusByStampSheetResult> AddRarityParameterStatusByStampSheetAsync(
            Request.AddRarityParameterStatusByStampSheetRequest request
        ) =>
            new AddRarityParameterStatusByStampSheetTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsTask();
#endif


        public class VerifyRarityParameterStatusTask : Gs2WebSocketSessionTask<Request.VerifyRarityParameterStatusRequest, Result.VerifyRarityParameterStatusResult>
        {
            public VerifyRarityParameterStatusTask(IGs2Session session, Request.VerifyRarityParameterStatusRequest request) : base(session, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(Request.VerifyRarityParameterStatusRequest request)
            {
                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);

                jsonWriter.WriteObjectStart();

                if (request.NamespaceName != null)
                {
                    jsonWriter.WritePropertyName("namespaceName");
                    jsonWriter.Write(request.NamespaceName.ToString());
                }
                if (request.ParameterName != null)
                {
                    jsonWriter.WritePropertyName("parameterName");
                    jsonWriter.Write(request.ParameterName.ToString());
                }
                if (request.AccessToken != null)
                {
                    jsonWriter.WritePropertyName("accessToken");
                    jsonWriter.Write(request.AccessToken.ToString());
                }
                if (request.PropertyId != null)
                {
                    jsonWriter.WritePropertyName("propertyId");
                    jsonWriter.Write(request.PropertyId.ToString());
                }
                if (request.VerifyType != null)
                {
                    jsonWriter.WritePropertyName("verifyType");
                    jsonWriter.Write(request.VerifyType.ToString());
                }
                if (request.ParameterValueName != null)
                {
                    jsonWriter.WritePropertyName("parameterValueName");
                    jsonWriter.Write(request.ParameterValueName.ToString());
                }
                if (request.ParameterCount != null)
                {
                    jsonWriter.WritePropertyName("parameterCount");
                    jsonWriter.Write(request.ParameterCount.ToString());
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
                    "enchant",
                    "rarityParameterStatus",
                    "verifyRarityParameterStatus",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
        public IEnumerator VerifyRarityParameterStatus(
                Request.VerifyRarityParameterStatusRequest request,
                UnityAction<AsyncResult<Result.VerifyRarityParameterStatusResult>> callback
        ) =>
            new VerifyRarityParameterStatusTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToCoroutine(callback);

        public IFuture<Result.VerifyRarityParameterStatusResult> VerifyRarityParameterStatusFuture(
                Request.VerifyRarityParameterStatusRequest request
        ) =>
            new VerifyRarityParameterStatusTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
        public UniTask<Result.VerifyRarityParameterStatusResult> VerifyRarityParameterStatusAsync(
            Request.VerifyRarityParameterStatusRequest request
        ) =>
            new VerifyRarityParameterStatusTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsUniTask<Result.VerifyRarityParameterStatusResult>();
    #else
        public VerifyRarityParameterStatusTask VerifyRarityParameterStatusAsync(
                Request.VerifyRarityParameterStatusRequest request
        )
        {
            return new VerifyRarityParameterStatusTask(
                Gs2WebSocketSession,
                request
            );
        }
    #endif
#else
        public Task<Result.VerifyRarityParameterStatusResult> VerifyRarityParameterStatusAsync(
            Request.VerifyRarityParameterStatusRequest request
        ) =>
            new VerifyRarityParameterStatusTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsTask();
#endif


        public class VerifyRarityParameterStatusByUserIdTask : Gs2WebSocketSessionTask<Request.VerifyRarityParameterStatusByUserIdRequest, Result.VerifyRarityParameterStatusByUserIdResult>
        {
            public VerifyRarityParameterStatusByUserIdTask(IGs2Session session, Request.VerifyRarityParameterStatusByUserIdRequest request) : base(session, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(Request.VerifyRarityParameterStatusByUserIdRequest request)
            {
                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);

                jsonWriter.WriteObjectStart();

                if (request.NamespaceName != null)
                {
                    jsonWriter.WritePropertyName("namespaceName");
                    jsonWriter.Write(request.NamespaceName.ToString());
                }
                if (request.ParameterName != null)
                {
                    jsonWriter.WritePropertyName("parameterName");
                    jsonWriter.Write(request.ParameterName.ToString());
                }
                if (request.UserId != null)
                {
                    jsonWriter.WritePropertyName("userId");
                    jsonWriter.Write(request.UserId.ToString());
                }
                if (request.PropertyId != null)
                {
                    jsonWriter.WritePropertyName("propertyId");
                    jsonWriter.Write(request.PropertyId.ToString());
                }
                if (request.VerifyType != null)
                {
                    jsonWriter.WritePropertyName("verifyType");
                    jsonWriter.Write(request.VerifyType.ToString());
                }
                if (request.ParameterValueName != null)
                {
                    jsonWriter.WritePropertyName("parameterValueName");
                    jsonWriter.Write(request.ParameterValueName.ToString());
                }
                if (request.ParameterCount != null)
                {
                    jsonWriter.WritePropertyName("parameterCount");
                    jsonWriter.Write(request.ParameterCount.ToString());
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
                    "enchant",
                    "rarityParameterStatus",
                    "verifyRarityParameterStatusByUserId",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
        public IEnumerator VerifyRarityParameterStatusByUserId(
                Request.VerifyRarityParameterStatusByUserIdRequest request,
                UnityAction<AsyncResult<Result.VerifyRarityParameterStatusByUserIdResult>> callback
        ) =>
            new VerifyRarityParameterStatusByUserIdTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToCoroutine(callback);

        public IFuture<Result.VerifyRarityParameterStatusByUserIdResult> VerifyRarityParameterStatusByUserIdFuture(
                Request.VerifyRarityParameterStatusByUserIdRequest request
        ) =>
            new VerifyRarityParameterStatusByUserIdTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
        public UniTask<Result.VerifyRarityParameterStatusByUserIdResult> VerifyRarityParameterStatusByUserIdAsync(
            Request.VerifyRarityParameterStatusByUserIdRequest request
        ) =>
            new VerifyRarityParameterStatusByUserIdTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsUniTask<Result.VerifyRarityParameterStatusByUserIdResult>();
    #else
        public VerifyRarityParameterStatusByUserIdTask VerifyRarityParameterStatusByUserIdAsync(
                Request.VerifyRarityParameterStatusByUserIdRequest request
        )
        {
            return new VerifyRarityParameterStatusByUserIdTask(
                Gs2WebSocketSession,
                request
            );
        }
    #endif
#else
        public Task<Result.VerifyRarityParameterStatusByUserIdResult> VerifyRarityParameterStatusByUserIdAsync(
            Request.VerifyRarityParameterStatusByUserIdRequest request
        ) =>
            new VerifyRarityParameterStatusByUserIdTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsTask();
#endif


        public class SetRarityParameterStatusByUserIdTask : Gs2WebSocketSessionTask<Request.SetRarityParameterStatusByUserIdRequest, Result.SetRarityParameterStatusByUserIdResult>
        {
            public SetRarityParameterStatusByUserIdTask(IGs2Session session, Request.SetRarityParameterStatusByUserIdRequest request) : base(session, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(Request.SetRarityParameterStatusByUserIdRequest request)
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
                if (request.ParameterName != null)
                {
                    jsonWriter.WritePropertyName("parameterName");
                    jsonWriter.Write(request.ParameterName.ToString());
                }
                if (request.PropertyId != null)
                {
                    jsonWriter.WritePropertyName("propertyId");
                    jsonWriter.Write(request.PropertyId.ToString());
                }
                if (request.ParameterValues != null)
                {
                    jsonWriter.WritePropertyName("parameterValues");
                    jsonWriter.WriteArrayStart();
                    foreach(var item in request.ParameterValues)
                    {
                        item.WriteJson(jsonWriter);
                    }
                    jsonWriter.WriteArrayEnd();
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
                    "enchant",
                    "rarityParameterStatus",
                    "setRarityParameterStatusByUserId",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
        public IEnumerator SetRarityParameterStatusByUserId(
                Request.SetRarityParameterStatusByUserIdRequest request,
                UnityAction<AsyncResult<Result.SetRarityParameterStatusByUserIdResult>> callback
        ) =>
            new SetRarityParameterStatusByUserIdTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToCoroutine(callback);

        public IFuture<Result.SetRarityParameterStatusByUserIdResult> SetRarityParameterStatusByUserIdFuture(
                Request.SetRarityParameterStatusByUserIdRequest request
        ) =>
            new SetRarityParameterStatusByUserIdTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
        public UniTask<Result.SetRarityParameterStatusByUserIdResult> SetRarityParameterStatusByUserIdAsync(
            Request.SetRarityParameterStatusByUserIdRequest request
        ) =>
            new SetRarityParameterStatusByUserIdTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsUniTask<Result.SetRarityParameterStatusByUserIdResult>();
    #else
        public SetRarityParameterStatusByUserIdTask SetRarityParameterStatusByUserIdAsync(
                Request.SetRarityParameterStatusByUserIdRequest request
        )
        {
            return new SetRarityParameterStatusByUserIdTask(
                Gs2WebSocketSession,
                request
            );
        }
    #endif
#else
        public Task<Result.SetRarityParameterStatusByUserIdResult> SetRarityParameterStatusByUserIdAsync(
            Request.SetRarityParameterStatusByUserIdRequest request
        ) =>
            new SetRarityParameterStatusByUserIdTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsTask();
#endif


        public class SetRarityParameterStatusByStampSheetTask : Gs2WebSocketSessionTask<Request.SetRarityParameterStatusByStampSheetRequest, Result.SetRarityParameterStatusByStampSheetResult>
        {
            public SetRarityParameterStatusByStampSheetTask(IGs2Session session, Request.SetRarityParameterStatusByStampSheetRequest request) : base(session, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(Request.SetRarityParameterStatusByStampSheetRequest request)
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
                    "enchant",
                    "rarityParameterStatus",
                    "setRarityParameterStatusByStampSheet",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
        public IEnumerator SetRarityParameterStatusByStampSheet(
                Request.SetRarityParameterStatusByStampSheetRequest request,
                UnityAction<AsyncResult<Result.SetRarityParameterStatusByStampSheetResult>> callback
        ) =>
            new SetRarityParameterStatusByStampSheetTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToCoroutine(callback);

        public IFuture<Result.SetRarityParameterStatusByStampSheetResult> SetRarityParameterStatusByStampSheetFuture(
                Request.SetRarityParameterStatusByStampSheetRequest request
        ) =>
            new SetRarityParameterStatusByStampSheetTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
        public UniTask<Result.SetRarityParameterStatusByStampSheetResult> SetRarityParameterStatusByStampSheetAsync(
            Request.SetRarityParameterStatusByStampSheetRequest request
        ) =>
            new SetRarityParameterStatusByStampSheetTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsUniTask<Result.SetRarityParameterStatusByStampSheetResult>();
    #else
        public SetRarityParameterStatusByStampSheetTask SetRarityParameterStatusByStampSheetAsync(
                Request.SetRarityParameterStatusByStampSheetRequest request
        )
        {
            return new SetRarityParameterStatusByStampSheetTask(
                Gs2WebSocketSession,
                request
            );
        }
    #endif
#else
        public Task<Result.SetRarityParameterStatusByStampSheetResult> SetRarityParameterStatusByStampSheetAsync(
            Request.SetRarityParameterStatusByStampSheetRequest request
        ) =>
            new SetRarityParameterStatusByStampSheetTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsTask();
#endif
	}
}