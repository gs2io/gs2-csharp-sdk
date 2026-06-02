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

namespace Gs2.Gs2SeasonRating
{
	public class Gs2SeasonRatingWebSocketClient : AbstractGs2Client
	{

		public static string Endpoint = "season-rating";

        protected Gs2WebSocketSession Gs2WebSocketSession => (Gs2WebSocketSession) Gs2Session;

		public Gs2SeasonRatingWebSocketClient(Gs2WebSocketSession Gs2WebSocketSession) : base(Gs2WebSocketSession)
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
                    "seasonRating",
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
                    "seasonRating",
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
                    "seasonRating",
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
                    "seasonRating",
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
                    "seasonRating",
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
                    "seasonRating",
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
                    "seasonRating",
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
                    "seasonRating",
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
                    "seasonRating",
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
                    "seasonRating",
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
                    "seasonRating",
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
                    "seasonRating",
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
                    "seasonRating",
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


        public class CreateMatchSessionTask : Gs2WebSocketSessionTask<Request.CreateMatchSessionRequest, Result.CreateMatchSessionResult>
        {
            public CreateMatchSessionTask(IGs2Session session, Request.CreateMatchSessionRequest request) : base(session, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(Request.CreateMatchSessionRequest request)
            {
                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);

                jsonWriter.WriteObjectStart();

                if (request.NamespaceName != null)
                {
                    jsonWriter.WritePropertyName("namespaceName");
                    jsonWriter.Write(request.NamespaceName.ToString());
                }
                if (request.SessionName != null)
                {
                    jsonWriter.WritePropertyName("sessionName");
                    jsonWriter.Write(request.SessionName.ToString());
                }
                if (request.TtlSeconds != null)
                {
                    jsonWriter.WritePropertyName("ttlSeconds");
                    jsonWriter.Write(request.TtlSeconds.ToString());
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
                    "seasonRating",
                    "matchSession",
                    "createMatchSession",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
        public IEnumerator CreateMatchSession(
                Request.CreateMatchSessionRequest request,
                UnityAction<AsyncResult<Result.CreateMatchSessionResult>> callback
        ) =>
            new CreateMatchSessionTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToCoroutine(callback);

        public IFuture<Result.CreateMatchSessionResult> CreateMatchSessionFuture(
                Request.CreateMatchSessionRequest request
        ) =>
            new CreateMatchSessionTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
        public UniTask<Result.CreateMatchSessionResult> CreateMatchSessionAsync(
            Request.CreateMatchSessionRequest request
        ) =>
            new CreateMatchSessionTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsUniTask<Result.CreateMatchSessionResult>();
    #else
        public CreateMatchSessionTask CreateMatchSessionAsync(
                Request.CreateMatchSessionRequest request
        )
        {
            return new CreateMatchSessionTask(
                Gs2WebSocketSession,
                request
            );
        }
    #endif
#else
        public Task<Result.CreateMatchSessionResult> CreateMatchSessionAsync(
            Request.CreateMatchSessionRequest request
        ) =>
            new CreateMatchSessionTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsTask();
#endif


        public class GetMatchSessionTask : Gs2WebSocketSessionTask<Request.GetMatchSessionRequest, Result.GetMatchSessionResult>
        {
            public GetMatchSessionTask(IGs2Session session, Request.GetMatchSessionRequest request) : base(session, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(Request.GetMatchSessionRequest request)
            {
                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);

                jsonWriter.WriteObjectStart();

                if (request.NamespaceName != null)
                {
                    jsonWriter.WritePropertyName("namespaceName");
                    jsonWriter.Write(request.NamespaceName.ToString());
                }
                if (request.SessionName != null)
                {
                    jsonWriter.WritePropertyName("sessionName");
                    jsonWriter.Write(request.SessionName.ToString());
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
                    "seasonRating",
                    "matchSession",
                    "getMatchSession",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
        public IEnumerator GetMatchSession(
                Request.GetMatchSessionRequest request,
                UnityAction<AsyncResult<Result.GetMatchSessionResult>> callback
        ) =>
            new GetMatchSessionTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToCoroutine(callback);

        public IFuture<Result.GetMatchSessionResult> GetMatchSessionFuture(
                Request.GetMatchSessionRequest request
        ) =>
            new GetMatchSessionTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
        public UniTask<Result.GetMatchSessionResult> GetMatchSessionAsync(
            Request.GetMatchSessionRequest request
        ) =>
            new GetMatchSessionTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsUniTask<Result.GetMatchSessionResult>();
    #else
        public GetMatchSessionTask GetMatchSessionAsync(
                Request.GetMatchSessionRequest request
        )
        {
            return new GetMatchSessionTask(
                Gs2WebSocketSession,
                request
            );
        }
    #endif
#else
        public Task<Result.GetMatchSessionResult> GetMatchSessionAsync(
            Request.GetMatchSessionRequest request
        ) =>
            new GetMatchSessionTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsTask();
#endif


        public class DeleteMatchSessionTask : Gs2WebSocketSessionTask<Request.DeleteMatchSessionRequest, Result.DeleteMatchSessionResult>
        {
            public DeleteMatchSessionTask(IGs2Session session, Request.DeleteMatchSessionRequest request) : base(session, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(Request.DeleteMatchSessionRequest request)
            {
                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);

                jsonWriter.WriteObjectStart();

                if (request.NamespaceName != null)
                {
                    jsonWriter.WritePropertyName("namespaceName");
                    jsonWriter.Write(request.NamespaceName.ToString());
                }
                if (request.SessionName != null)
                {
                    jsonWriter.WritePropertyName("sessionName");
                    jsonWriter.Write(request.SessionName.ToString());
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
                    "seasonRating",
                    "matchSession",
                    "deleteMatchSession",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
        public IEnumerator DeleteMatchSession(
                Request.DeleteMatchSessionRequest request,
                UnityAction<AsyncResult<Result.DeleteMatchSessionResult>> callback
        ) =>
            new DeleteMatchSessionTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToCoroutine(callback);

        public IFuture<Result.DeleteMatchSessionResult> DeleteMatchSessionFuture(
                Request.DeleteMatchSessionRequest request
        ) =>
            new DeleteMatchSessionTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
        public UniTask<Result.DeleteMatchSessionResult> DeleteMatchSessionAsync(
            Request.DeleteMatchSessionRequest request
        ) =>
            new DeleteMatchSessionTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsUniTask<Result.DeleteMatchSessionResult>();
    #else
        public DeleteMatchSessionTask DeleteMatchSessionAsync(
                Request.DeleteMatchSessionRequest request
        )
        {
            return new DeleteMatchSessionTask(
                Gs2WebSocketSession,
                request
            );
        }
    #endif
#else
        public Task<Result.DeleteMatchSessionResult> DeleteMatchSessionAsync(
            Request.DeleteMatchSessionRequest request
        ) =>
            new DeleteMatchSessionTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsTask();
#endif


        public class CreateSeasonModelMasterTask : Gs2WebSocketSessionTask<Request.CreateSeasonModelMasterRequest, Result.CreateSeasonModelMasterResult>
        {
            public CreateSeasonModelMasterTask(IGs2Session session, Request.CreateSeasonModelMasterRequest request) : base(session, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(Request.CreateSeasonModelMasterRequest request)
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
                if (request.Tiers != null)
                {
                    jsonWriter.WritePropertyName("tiers");
                    jsonWriter.WriteArrayStart();
                    foreach(var item in request.Tiers)
                    {
                        item.WriteJson(jsonWriter);
                    }
                    jsonWriter.WriteArrayEnd();
                }
                if (request.ExperienceModelId != null)
                {
                    jsonWriter.WritePropertyName("experienceModelId");
                    jsonWriter.Write(request.ExperienceModelId.ToString());
                }
                if (request.ChallengePeriodEventId != null)
                {
                    jsonWriter.WritePropertyName("challengePeriodEventId");
                    jsonWriter.Write(request.ChallengePeriodEventId.ToString());
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
                    "seasonRating",
                    "seasonModelMaster",
                    "createSeasonModelMaster",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
        public IEnumerator CreateSeasonModelMaster(
                Request.CreateSeasonModelMasterRequest request,
                UnityAction<AsyncResult<Result.CreateSeasonModelMasterResult>> callback
        ) =>
            new CreateSeasonModelMasterTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToCoroutine(callback);

        public IFuture<Result.CreateSeasonModelMasterResult> CreateSeasonModelMasterFuture(
                Request.CreateSeasonModelMasterRequest request
        ) =>
            new CreateSeasonModelMasterTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
        public UniTask<Result.CreateSeasonModelMasterResult> CreateSeasonModelMasterAsync(
            Request.CreateSeasonModelMasterRequest request
        ) =>
            new CreateSeasonModelMasterTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsUniTask<Result.CreateSeasonModelMasterResult>();
    #else
        public CreateSeasonModelMasterTask CreateSeasonModelMasterAsync(
                Request.CreateSeasonModelMasterRequest request
        )
        {
            return new CreateSeasonModelMasterTask(
                Gs2WebSocketSession,
                request
            );
        }
    #endif
#else
        public Task<Result.CreateSeasonModelMasterResult> CreateSeasonModelMasterAsync(
            Request.CreateSeasonModelMasterRequest request
        ) =>
            new CreateSeasonModelMasterTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsTask();
#endif


        public class GetSeasonModelMasterTask : Gs2WebSocketSessionTask<Request.GetSeasonModelMasterRequest, Result.GetSeasonModelMasterResult>
        {
            public GetSeasonModelMasterTask(IGs2Session session, Request.GetSeasonModelMasterRequest request) : base(session, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(Request.GetSeasonModelMasterRequest request)
            {
                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);

                jsonWriter.WriteObjectStart();

                if (request.NamespaceName != null)
                {
                    jsonWriter.WritePropertyName("namespaceName");
                    jsonWriter.Write(request.NamespaceName.ToString());
                }
                if (request.SeasonName != null)
                {
                    jsonWriter.WritePropertyName("seasonName");
                    jsonWriter.Write(request.SeasonName.ToString());
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
                    "seasonRating",
                    "seasonModelMaster",
                    "getSeasonModelMaster",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
        public IEnumerator GetSeasonModelMaster(
                Request.GetSeasonModelMasterRequest request,
                UnityAction<AsyncResult<Result.GetSeasonModelMasterResult>> callback
        ) =>
            new GetSeasonModelMasterTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToCoroutine(callback);

        public IFuture<Result.GetSeasonModelMasterResult> GetSeasonModelMasterFuture(
                Request.GetSeasonModelMasterRequest request
        ) =>
            new GetSeasonModelMasterTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
        public UniTask<Result.GetSeasonModelMasterResult> GetSeasonModelMasterAsync(
            Request.GetSeasonModelMasterRequest request
        ) =>
            new GetSeasonModelMasterTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsUniTask<Result.GetSeasonModelMasterResult>();
    #else
        public GetSeasonModelMasterTask GetSeasonModelMasterAsync(
                Request.GetSeasonModelMasterRequest request
        )
        {
            return new GetSeasonModelMasterTask(
                Gs2WebSocketSession,
                request
            );
        }
    #endif
#else
        public Task<Result.GetSeasonModelMasterResult> GetSeasonModelMasterAsync(
            Request.GetSeasonModelMasterRequest request
        ) =>
            new GetSeasonModelMasterTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsTask();
#endif


        public class UpdateSeasonModelMasterTask : Gs2WebSocketSessionTask<Request.UpdateSeasonModelMasterRequest, Result.UpdateSeasonModelMasterResult>
        {
            public UpdateSeasonModelMasterTask(IGs2Session session, Request.UpdateSeasonModelMasterRequest request) : base(session, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(Request.UpdateSeasonModelMasterRequest request)
            {
                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);

                jsonWriter.WriteObjectStart();

                if (request.NamespaceName != null)
                {
                    jsonWriter.WritePropertyName("namespaceName");
                    jsonWriter.Write(request.NamespaceName.ToString());
                }
                if (request.SeasonName != null)
                {
                    jsonWriter.WritePropertyName("seasonName");
                    jsonWriter.Write(request.SeasonName.ToString());
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
                if (request.Tiers != null)
                {
                    jsonWriter.WritePropertyName("tiers");
                    jsonWriter.WriteArrayStart();
                    foreach(var item in request.Tiers)
                    {
                        item.WriteJson(jsonWriter);
                    }
                    jsonWriter.WriteArrayEnd();
                }
                if (request.ExperienceModelId != null)
                {
                    jsonWriter.WritePropertyName("experienceModelId");
                    jsonWriter.Write(request.ExperienceModelId.ToString());
                }
                if (request.ChallengePeriodEventId != null)
                {
                    jsonWriter.WritePropertyName("challengePeriodEventId");
                    jsonWriter.Write(request.ChallengePeriodEventId.ToString());
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
                    "seasonRating",
                    "seasonModelMaster",
                    "updateSeasonModelMaster",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
        public IEnumerator UpdateSeasonModelMaster(
                Request.UpdateSeasonModelMasterRequest request,
                UnityAction<AsyncResult<Result.UpdateSeasonModelMasterResult>> callback
        ) =>
            new UpdateSeasonModelMasterTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToCoroutine(callback);

        public IFuture<Result.UpdateSeasonModelMasterResult> UpdateSeasonModelMasterFuture(
                Request.UpdateSeasonModelMasterRequest request
        ) =>
            new UpdateSeasonModelMasterTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
        public UniTask<Result.UpdateSeasonModelMasterResult> UpdateSeasonModelMasterAsync(
            Request.UpdateSeasonModelMasterRequest request
        ) =>
            new UpdateSeasonModelMasterTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsUniTask<Result.UpdateSeasonModelMasterResult>();
    #else
        public UpdateSeasonModelMasterTask UpdateSeasonModelMasterAsync(
                Request.UpdateSeasonModelMasterRequest request
        )
        {
            return new UpdateSeasonModelMasterTask(
                Gs2WebSocketSession,
                request
            );
        }
    #endif
#else
        public Task<Result.UpdateSeasonModelMasterResult> UpdateSeasonModelMasterAsync(
            Request.UpdateSeasonModelMasterRequest request
        ) =>
            new UpdateSeasonModelMasterTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsTask();
#endif


        public class DeleteSeasonModelMasterTask : Gs2WebSocketSessionTask<Request.DeleteSeasonModelMasterRequest, Result.DeleteSeasonModelMasterResult>
        {
            public DeleteSeasonModelMasterTask(IGs2Session session, Request.DeleteSeasonModelMasterRequest request) : base(session, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(Request.DeleteSeasonModelMasterRequest request)
            {
                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);

                jsonWriter.WriteObjectStart();

                if (request.NamespaceName != null)
                {
                    jsonWriter.WritePropertyName("namespaceName");
                    jsonWriter.Write(request.NamespaceName.ToString());
                }
                if (request.SeasonName != null)
                {
                    jsonWriter.WritePropertyName("seasonName");
                    jsonWriter.Write(request.SeasonName.ToString());
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
                    "seasonRating",
                    "seasonModelMaster",
                    "deleteSeasonModelMaster",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
        public IEnumerator DeleteSeasonModelMaster(
                Request.DeleteSeasonModelMasterRequest request,
                UnityAction<AsyncResult<Result.DeleteSeasonModelMasterResult>> callback
        ) =>
            new DeleteSeasonModelMasterTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToCoroutine(callback);

        public IFuture<Result.DeleteSeasonModelMasterResult> DeleteSeasonModelMasterFuture(
                Request.DeleteSeasonModelMasterRequest request
        ) =>
            new DeleteSeasonModelMasterTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
        public UniTask<Result.DeleteSeasonModelMasterResult> DeleteSeasonModelMasterAsync(
            Request.DeleteSeasonModelMasterRequest request
        ) =>
            new DeleteSeasonModelMasterTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsUniTask<Result.DeleteSeasonModelMasterResult>();
    #else
        public DeleteSeasonModelMasterTask DeleteSeasonModelMasterAsync(
                Request.DeleteSeasonModelMasterRequest request
        )
        {
            return new DeleteSeasonModelMasterTask(
                Gs2WebSocketSession,
                request
            );
        }
    #endif
#else
        public Task<Result.DeleteSeasonModelMasterResult> DeleteSeasonModelMasterAsync(
            Request.DeleteSeasonModelMasterRequest request
        ) =>
            new DeleteSeasonModelMasterTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsTask();
#endif


        public class GetSeasonModelTask : Gs2WebSocketSessionTask<Request.GetSeasonModelRequest, Result.GetSeasonModelResult>
        {
            public GetSeasonModelTask(IGs2Session session, Request.GetSeasonModelRequest request) : base(session, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(Request.GetSeasonModelRequest request)
            {
                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);

                jsonWriter.WriteObjectStart();

                if (request.NamespaceName != null)
                {
                    jsonWriter.WritePropertyName("namespaceName");
                    jsonWriter.Write(request.NamespaceName.ToString());
                }
                if (request.SeasonName != null)
                {
                    jsonWriter.WritePropertyName("seasonName");
                    jsonWriter.Write(request.SeasonName.ToString());
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
                    "seasonRating",
                    "seasonModel",
                    "getSeasonModel",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
        public IEnumerator GetSeasonModel(
                Request.GetSeasonModelRequest request,
                UnityAction<AsyncResult<Result.GetSeasonModelResult>> callback
        ) =>
            new GetSeasonModelTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToCoroutine(callback);

        public IFuture<Result.GetSeasonModelResult> GetSeasonModelFuture(
                Request.GetSeasonModelRequest request
        ) =>
            new GetSeasonModelTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
        public UniTask<Result.GetSeasonModelResult> GetSeasonModelAsync(
            Request.GetSeasonModelRequest request
        ) =>
            new GetSeasonModelTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsUniTask<Result.GetSeasonModelResult>();
    #else
        public GetSeasonModelTask GetSeasonModelAsync(
                Request.GetSeasonModelRequest request
        )
        {
            return new GetSeasonModelTask(
                Gs2WebSocketSession,
                request
            );
        }
    #endif
#else
        public Task<Result.GetSeasonModelResult> GetSeasonModelAsync(
            Request.GetSeasonModelRequest request
        ) =>
            new GetSeasonModelTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsTask();
#endif


        public class PreUpdateCurrentSeasonModelMasterTask : Gs2WebSocketSessionTask<Request.PreUpdateCurrentSeasonModelMasterRequest, Result.PreUpdateCurrentSeasonModelMasterResult>
        {
            public PreUpdateCurrentSeasonModelMasterTask(IGs2Session session, Request.PreUpdateCurrentSeasonModelMasterRequest request) : base(session, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(Request.PreUpdateCurrentSeasonModelMasterRequest request)
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
                    "seasonRating",
                    "currentSeasonModelMaster",
                    "preUpdateCurrentSeasonModelMaster",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
        public IEnumerator PreUpdateCurrentSeasonModelMaster(
                Request.PreUpdateCurrentSeasonModelMasterRequest request,
                UnityAction<AsyncResult<Result.PreUpdateCurrentSeasonModelMasterResult>> callback
        ) =>
            new PreUpdateCurrentSeasonModelMasterTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToCoroutine(callback);

        public IFuture<Result.PreUpdateCurrentSeasonModelMasterResult> PreUpdateCurrentSeasonModelMasterFuture(
                Request.PreUpdateCurrentSeasonModelMasterRequest request
        ) =>
            new PreUpdateCurrentSeasonModelMasterTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
        public UniTask<Result.PreUpdateCurrentSeasonModelMasterResult> PreUpdateCurrentSeasonModelMasterAsync(
            Request.PreUpdateCurrentSeasonModelMasterRequest request
        ) =>
            new PreUpdateCurrentSeasonModelMasterTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsUniTask<Result.PreUpdateCurrentSeasonModelMasterResult>();
    #else
        public PreUpdateCurrentSeasonModelMasterTask PreUpdateCurrentSeasonModelMasterAsync(
                Request.PreUpdateCurrentSeasonModelMasterRequest request
        )
        {
            return new PreUpdateCurrentSeasonModelMasterTask(
                Gs2WebSocketSession,
                request
            );
        }
    #endif
#else
        public Task<Result.PreUpdateCurrentSeasonModelMasterResult> PreUpdateCurrentSeasonModelMasterAsync(
            Request.PreUpdateCurrentSeasonModelMasterRequest request
        ) =>
            new PreUpdateCurrentSeasonModelMasterTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsTask();
#endif


        public class VoteTask : Gs2WebSocketSessionTask<Request.VoteRequest, Result.VoteResult>
        {
            public VoteTask(IGs2Session session, Request.VoteRequest request) : base(session, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(Request.VoteRequest request)
            {
                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);

                jsonWriter.WriteObjectStart();

                if (request.NamespaceName != null)
                {
                    jsonWriter.WritePropertyName("namespaceName");
                    jsonWriter.Write(request.NamespaceName.ToString());
                }
                if (request.BallotBody != null)
                {
                    jsonWriter.WritePropertyName("ballotBody");
                    jsonWriter.Write(request.BallotBody.ToString());
                }
                if (request.BallotSignature != null)
                {
                    jsonWriter.WritePropertyName("ballotSignature");
                    jsonWriter.Write(request.BallotSignature.ToString());
                }
                if (request.GameResults != null)
                {
                    jsonWriter.WritePropertyName("gameResults");
                    jsonWriter.WriteArrayStart();
                    foreach(var item in request.GameResults)
                    {
                        item.WriteJson(jsonWriter);
                    }
                    jsonWriter.WriteArrayEnd();
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
                    "seasonRating",
                    "vote",
                    "vote",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
        public IEnumerator Vote(
                Request.VoteRequest request,
                UnityAction<AsyncResult<Result.VoteResult>> callback
        ) =>
            new VoteTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToCoroutine(callback);

        public IFuture<Result.VoteResult> VoteFuture(
                Request.VoteRequest request
        ) =>
            new VoteTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
        public UniTask<Result.VoteResult> VoteAsync(
            Request.VoteRequest request
        ) =>
            new VoteTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsUniTask<Result.VoteResult>();
    #else
        public VoteTask VoteAsync(
                Request.VoteRequest request
        )
        {
            return new VoteTask(
                Gs2WebSocketSession,
                request
            );
        }
    #endif
#else
        public Task<Result.VoteResult> VoteAsync(
            Request.VoteRequest request
        ) =>
            new VoteTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsTask();
#endif


        public class VoteMultipleTask : Gs2WebSocketSessionTask<Request.VoteMultipleRequest, Result.VoteMultipleResult>
        {
            public VoteMultipleTask(IGs2Session session, Request.VoteMultipleRequest request) : base(session, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(Request.VoteMultipleRequest request)
            {
                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);

                jsonWriter.WriteObjectStart();

                if (request.NamespaceName != null)
                {
                    jsonWriter.WritePropertyName("namespaceName");
                    jsonWriter.Write(request.NamespaceName.ToString());
                }
                if (request.SignedBallots != null)
                {
                    jsonWriter.WritePropertyName("signedBallots");
                    jsonWriter.WriteArrayStart();
                    foreach(var item in request.SignedBallots)
                    {
                        item.WriteJson(jsonWriter);
                    }
                    jsonWriter.WriteArrayEnd();
                }
                if (request.GameResults != null)
                {
                    jsonWriter.WritePropertyName("gameResults");
                    jsonWriter.WriteArrayStart();
                    foreach(var item in request.GameResults)
                    {
                        item.WriteJson(jsonWriter);
                    }
                    jsonWriter.WriteArrayEnd();
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
                    "seasonRating",
                    "vote",
                    "voteMultiple",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
        public IEnumerator VoteMultiple(
                Request.VoteMultipleRequest request,
                UnityAction<AsyncResult<Result.VoteMultipleResult>> callback
        ) =>
            new VoteMultipleTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToCoroutine(callback);

        public IFuture<Result.VoteMultipleResult> VoteMultipleFuture(
                Request.VoteMultipleRequest request
        ) =>
            new VoteMultipleTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
        public UniTask<Result.VoteMultipleResult> VoteMultipleAsync(
            Request.VoteMultipleRequest request
        ) =>
            new VoteMultipleTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsUniTask<Result.VoteMultipleResult>();
    #else
        public VoteMultipleTask VoteMultipleAsync(
                Request.VoteMultipleRequest request
        )
        {
            return new VoteMultipleTask(
                Gs2WebSocketSession,
                request
            );
        }
    #endif
#else
        public Task<Result.VoteMultipleResult> VoteMultipleAsync(
            Request.VoteMultipleRequest request
        ) =>
            new VoteMultipleTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsTask();
#endif


        public class CommitVoteTask : Gs2WebSocketSessionTask<Request.CommitVoteRequest, Result.CommitVoteResult>
        {
            public CommitVoteTask(IGs2Session session, Request.CommitVoteRequest request) : base(session, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(Request.CommitVoteRequest request)
            {
                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);

                jsonWriter.WriteObjectStart();

                if (request.NamespaceName != null)
                {
                    jsonWriter.WritePropertyName("namespaceName");
                    jsonWriter.Write(request.NamespaceName.ToString());
                }
                if (request.SeasonName != null)
                {
                    jsonWriter.WritePropertyName("seasonName");
                    jsonWriter.Write(request.SeasonName.ToString());
                }
                if (request.SessionName != null)
                {
                    jsonWriter.WritePropertyName("sessionName");
                    jsonWriter.Write(request.SessionName.ToString());
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
                    "seasonRating",
                    "vote",
                    "commitVote",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
        public IEnumerator CommitVote(
                Request.CommitVoteRequest request,
                UnityAction<AsyncResult<Result.CommitVoteResult>> callback
        ) =>
            new CommitVoteTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToCoroutine(callback);

        public IFuture<Result.CommitVoteResult> CommitVoteFuture(
                Request.CommitVoteRequest request
        ) =>
            new CommitVoteTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
        public UniTask<Result.CommitVoteResult> CommitVoteAsync(
            Request.CommitVoteRequest request
        ) =>
            new CommitVoteTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsUniTask<Result.CommitVoteResult>();
    #else
        public CommitVoteTask CommitVoteAsync(
                Request.CommitVoteRequest request
        )
        {
            return new CommitVoteTask(
                Gs2WebSocketSession,
                request
            );
        }
    #endif
#else
        public Task<Result.CommitVoteResult> CommitVoteAsync(
            Request.CommitVoteRequest request
        ) =>
            new CommitVoteTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsTask();
#endif
	}
}