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

namespace Gs2.Gs2Guild
{
	public class Gs2GuildWebSocketClient : AbstractGs2Client
	{

		public static string Endpoint = "guild";

        protected Gs2WebSocketSession Gs2WebSocketSession => (Gs2WebSocketSession) Gs2Session;

		public Gs2GuildWebSocketClient(Gs2WebSocketSession Gs2WebSocketSession) : base(Gs2WebSocketSession)
		{

		}


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
                    "guild",
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
                    "guild",
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
                    "guild",
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
                    "guild",
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
                    "guild",
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
                    "guild",
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
                    "guild",
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
                    "guild",
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
                    "guild",
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


        public class GetJoinedGuildTask : Gs2WebSocketSessionTask<Request.GetJoinedGuildRequest, Result.GetJoinedGuildResult>
        {
            public GetJoinedGuildTask(IGs2Session session, Request.GetJoinedGuildRequest request) : base(session, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(Request.GetJoinedGuildRequest request)
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
                if (request.GuildModelName != null)
                {
                    jsonWriter.WritePropertyName("guildModelName");
                    jsonWriter.Write(request.GuildModelName.ToString());
                }
                if (request.GuildName != null)
                {
                    jsonWriter.WritePropertyName("guildName");
                    jsonWriter.Write(request.GuildName.ToString());
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
                    "guild",
                    "joinedGuild",
                    "getJoinedGuild",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
        public IEnumerator GetJoinedGuild(
                Request.GetJoinedGuildRequest request,
                UnityAction<AsyncResult<Result.GetJoinedGuildResult>> callback
        ) =>
            new GetJoinedGuildTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToCoroutine(callback);

        public IFuture<Result.GetJoinedGuildResult> GetJoinedGuildFuture(
                Request.GetJoinedGuildRequest request
        ) =>
            new GetJoinedGuildTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
        public UniTask<Result.GetJoinedGuildResult> GetJoinedGuildAsync(
            Request.GetJoinedGuildRequest request
        ) =>
            new GetJoinedGuildTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsUniTask<Result.GetJoinedGuildResult>();
    #else
        public GetJoinedGuildTask GetJoinedGuildAsync(
                Request.GetJoinedGuildRequest request
        )
        {
            return new GetJoinedGuildTask(
                Gs2WebSocketSession,
                request
            );
        }
    #endif
#else
        public Task<Result.GetJoinedGuildResult> GetJoinedGuildAsync(
            Request.GetJoinedGuildRequest request
        ) =>
            new GetJoinedGuildTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsTask();
#endif


        public class GetJoinedGuildByUserIdTask : Gs2WebSocketSessionTask<Request.GetJoinedGuildByUserIdRequest, Result.GetJoinedGuildByUserIdResult>
        {
            public GetJoinedGuildByUserIdTask(IGs2Session session, Request.GetJoinedGuildByUserIdRequest request) : base(session, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(Request.GetJoinedGuildByUserIdRequest request)
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
                if (request.GuildModelName != null)
                {
                    jsonWriter.WritePropertyName("guildModelName");
                    jsonWriter.Write(request.GuildModelName.ToString());
                }
                if (request.GuildName != null)
                {
                    jsonWriter.WritePropertyName("guildName");
                    jsonWriter.Write(request.GuildName.ToString());
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
                    "guild",
                    "joinedGuild",
                    "getJoinedGuildByUserId",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
        public IEnumerator GetJoinedGuildByUserId(
                Request.GetJoinedGuildByUserIdRequest request,
                UnityAction<AsyncResult<Result.GetJoinedGuildByUserIdResult>> callback
        ) =>
            new GetJoinedGuildByUserIdTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToCoroutine(callback);

        public IFuture<Result.GetJoinedGuildByUserIdResult> GetJoinedGuildByUserIdFuture(
                Request.GetJoinedGuildByUserIdRequest request
        ) =>
            new GetJoinedGuildByUserIdTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
        public UniTask<Result.GetJoinedGuildByUserIdResult> GetJoinedGuildByUserIdAsync(
            Request.GetJoinedGuildByUserIdRequest request
        ) =>
            new GetJoinedGuildByUserIdTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsUniTask<Result.GetJoinedGuildByUserIdResult>();
    #else
        public GetJoinedGuildByUserIdTask GetJoinedGuildByUserIdAsync(
                Request.GetJoinedGuildByUserIdRequest request
        )
        {
            return new GetJoinedGuildByUserIdTask(
                Gs2WebSocketSession,
                request
            );
        }
    #endif
#else
        public Task<Result.GetJoinedGuildByUserIdResult> GetJoinedGuildByUserIdAsync(
            Request.GetJoinedGuildByUserIdRequest request
        ) =>
            new GetJoinedGuildByUserIdTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsTask();
#endif


        public class PreUpdateCurrentGuildMasterTask : Gs2WebSocketSessionTask<Request.PreUpdateCurrentGuildMasterRequest, Result.PreUpdateCurrentGuildMasterResult>
        {
            public PreUpdateCurrentGuildMasterTask(IGs2Session session, Request.PreUpdateCurrentGuildMasterRequest request) : base(session, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(Request.PreUpdateCurrentGuildMasterRequest request)
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
                    "guild",
                    "currentGuildMaster",
                    "preUpdateCurrentGuildMaster",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
        public IEnumerator PreUpdateCurrentGuildMaster(
                Request.PreUpdateCurrentGuildMasterRequest request,
                UnityAction<AsyncResult<Result.PreUpdateCurrentGuildMasterResult>> callback
        ) =>
            new PreUpdateCurrentGuildMasterTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToCoroutine(callback);

        public IFuture<Result.PreUpdateCurrentGuildMasterResult> PreUpdateCurrentGuildMasterFuture(
                Request.PreUpdateCurrentGuildMasterRequest request
        ) =>
            new PreUpdateCurrentGuildMasterTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
        public UniTask<Result.PreUpdateCurrentGuildMasterResult> PreUpdateCurrentGuildMasterAsync(
            Request.PreUpdateCurrentGuildMasterRequest request
        ) =>
            new PreUpdateCurrentGuildMasterTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsUniTask<Result.PreUpdateCurrentGuildMasterResult>();
    #else
        public PreUpdateCurrentGuildMasterTask PreUpdateCurrentGuildMasterAsync(
                Request.PreUpdateCurrentGuildMasterRequest request
        )
        {
            return new PreUpdateCurrentGuildMasterTask(
                Gs2WebSocketSession,
                request
            );
        }
    #endif
#else
        public Task<Result.PreUpdateCurrentGuildMasterResult> PreUpdateCurrentGuildMasterAsync(
            Request.PreUpdateCurrentGuildMasterRequest request
        ) =>
            new PreUpdateCurrentGuildMasterTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsTask();
#endif


        public class GetReceiveRequestTask : Gs2WebSocketSessionTask<Request.GetReceiveRequestRequest, Result.GetReceiveRequestResult>
        {
            public GetReceiveRequestTask(IGs2Session session, Request.GetReceiveRequestRequest request) : base(session, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(Request.GetReceiveRequestRequest request)
            {
                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);

                jsonWriter.WriteObjectStart();

                if (request.NamespaceName != null)
                {
                    jsonWriter.WritePropertyName("namespaceName");
                    jsonWriter.Write(request.NamespaceName.ToString());
                }
                if (request.GuildModelName != null)
                {
                    jsonWriter.WritePropertyName("guildModelName");
                    jsonWriter.Write(request.GuildModelName.ToString());
                }
                if (request.AccessToken != null)
                {
                    jsonWriter.WritePropertyName("accessToken");
                    jsonWriter.Write(request.AccessToken.ToString());
                }
                if (request.FromUserId != null)
                {
                    jsonWriter.WritePropertyName("fromUserId");
                    jsonWriter.Write(request.FromUserId.ToString());
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
                    "guild",
                    "receiveMemberRequest",
                    "getReceiveRequest",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
        public IEnumerator GetReceiveRequest(
                Request.GetReceiveRequestRequest request,
                UnityAction<AsyncResult<Result.GetReceiveRequestResult>> callback
        ) =>
            new GetReceiveRequestTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToCoroutine(callback);

        public IFuture<Result.GetReceiveRequestResult> GetReceiveRequestFuture(
                Request.GetReceiveRequestRequest request
        ) =>
            new GetReceiveRequestTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
        public UniTask<Result.GetReceiveRequestResult> GetReceiveRequestAsync(
            Request.GetReceiveRequestRequest request
        ) =>
            new GetReceiveRequestTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsUniTask<Result.GetReceiveRequestResult>();
    #else
        public GetReceiveRequestTask GetReceiveRequestAsync(
                Request.GetReceiveRequestRequest request
        )
        {
            return new GetReceiveRequestTask(
                Gs2WebSocketSession,
                request
            );
        }
    #endif
#else
        public Task<Result.GetReceiveRequestResult> GetReceiveRequestAsync(
            Request.GetReceiveRequestRequest request
        ) =>
            new GetReceiveRequestTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsTask();
#endif


        public class GetReceiveRequestByGuildNameTask : Gs2WebSocketSessionTask<Request.GetReceiveRequestByGuildNameRequest, Result.GetReceiveRequestByGuildNameResult>
        {
            public GetReceiveRequestByGuildNameTask(IGs2Session session, Request.GetReceiveRequestByGuildNameRequest request) : base(session, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(Request.GetReceiveRequestByGuildNameRequest request)
            {
                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);

                jsonWriter.WriteObjectStart();

                if (request.NamespaceName != null)
                {
                    jsonWriter.WritePropertyName("namespaceName");
                    jsonWriter.Write(request.NamespaceName.ToString());
                }
                if (request.GuildModelName != null)
                {
                    jsonWriter.WritePropertyName("guildModelName");
                    jsonWriter.Write(request.GuildModelName.ToString());
                }
                if (request.GuildName != null)
                {
                    jsonWriter.WritePropertyName("guildName");
                    jsonWriter.Write(request.GuildName.ToString());
                }
                if (request.FromUserId != null)
                {
                    jsonWriter.WritePropertyName("fromUserId");
                    jsonWriter.Write(request.FromUserId.ToString());
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
                    "guild",
                    "receiveMemberRequest",
                    "getReceiveRequestByGuildName",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
        public IEnumerator GetReceiveRequestByGuildName(
                Request.GetReceiveRequestByGuildNameRequest request,
                UnityAction<AsyncResult<Result.GetReceiveRequestByGuildNameResult>> callback
        ) =>
            new GetReceiveRequestByGuildNameTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToCoroutine(callback);

        public IFuture<Result.GetReceiveRequestByGuildNameResult> GetReceiveRequestByGuildNameFuture(
                Request.GetReceiveRequestByGuildNameRequest request
        ) =>
            new GetReceiveRequestByGuildNameTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
        public UniTask<Result.GetReceiveRequestByGuildNameResult> GetReceiveRequestByGuildNameAsync(
            Request.GetReceiveRequestByGuildNameRequest request
        ) =>
            new GetReceiveRequestByGuildNameTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsUniTask<Result.GetReceiveRequestByGuildNameResult>();
    #else
        public GetReceiveRequestByGuildNameTask GetReceiveRequestByGuildNameAsync(
                Request.GetReceiveRequestByGuildNameRequest request
        )
        {
            return new GetReceiveRequestByGuildNameTask(
                Gs2WebSocketSession,
                request
            );
        }
    #endif
#else
        public Task<Result.GetReceiveRequestByGuildNameResult> GetReceiveRequestByGuildNameAsync(
            Request.GetReceiveRequestByGuildNameRequest request
        ) =>
            new GetReceiveRequestByGuildNameTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsTask();
#endif


        public class RejectRequestTask : Gs2WebSocketSessionTask<Request.RejectRequestRequest, Result.RejectRequestResult>
        {
            public RejectRequestTask(IGs2Session session, Request.RejectRequestRequest request) : base(session, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(Request.RejectRequestRequest request)
            {
                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);

                jsonWriter.WriteObjectStart();

                if (request.NamespaceName != null)
                {
                    jsonWriter.WritePropertyName("namespaceName");
                    jsonWriter.Write(request.NamespaceName.ToString());
                }
                if (request.GuildModelName != null)
                {
                    jsonWriter.WritePropertyName("guildModelName");
                    jsonWriter.Write(request.GuildModelName.ToString());
                }
                if (request.AccessToken != null)
                {
                    jsonWriter.WritePropertyName("accessToken");
                    jsonWriter.Write(request.AccessToken.ToString());
                }
                if (request.FromUserId != null)
                {
                    jsonWriter.WritePropertyName("fromUserId");
                    jsonWriter.Write(request.FromUserId.ToString());
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
                    "guild",
                    "receiveMemberRequest",
                    "rejectRequest",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
        public IEnumerator RejectRequest(
                Request.RejectRequestRequest request,
                UnityAction<AsyncResult<Result.RejectRequestResult>> callback
        ) =>
            new RejectRequestTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToCoroutine(callback);

        public IFuture<Result.RejectRequestResult> RejectRequestFuture(
                Request.RejectRequestRequest request
        ) =>
            new RejectRequestTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
        public UniTask<Result.RejectRequestResult> RejectRequestAsync(
            Request.RejectRequestRequest request
        ) =>
            new RejectRequestTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsUniTask<Result.RejectRequestResult>();
    #else
        public RejectRequestTask RejectRequestAsync(
                Request.RejectRequestRequest request
        )
        {
            return new RejectRequestTask(
                Gs2WebSocketSession,
                request
            );
        }
    #endif
#else
        public Task<Result.RejectRequestResult> RejectRequestAsync(
            Request.RejectRequestRequest request
        ) =>
            new RejectRequestTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsTask();
#endif


        public class RejectRequestByGuildNameTask : Gs2WebSocketSessionTask<Request.RejectRequestByGuildNameRequest, Result.RejectRequestByGuildNameResult>
        {
            public RejectRequestByGuildNameTask(IGs2Session session, Request.RejectRequestByGuildNameRequest request) : base(session, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(Request.RejectRequestByGuildNameRequest request)
            {
                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);

                jsonWriter.WriteObjectStart();

                if (request.NamespaceName != null)
                {
                    jsonWriter.WritePropertyName("namespaceName");
                    jsonWriter.Write(request.NamespaceName.ToString());
                }
                if (request.GuildModelName != null)
                {
                    jsonWriter.WritePropertyName("guildModelName");
                    jsonWriter.Write(request.GuildModelName.ToString());
                }
                if (request.GuildName != null)
                {
                    jsonWriter.WritePropertyName("guildName");
                    jsonWriter.Write(request.GuildName.ToString());
                }
                if (request.FromUserId != null)
                {
                    jsonWriter.WritePropertyName("fromUserId");
                    jsonWriter.Write(request.FromUserId.ToString());
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
                    "guild",
                    "receiveMemberRequest",
                    "rejectRequestByGuildName",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
        public IEnumerator RejectRequestByGuildName(
                Request.RejectRequestByGuildNameRequest request,
                UnityAction<AsyncResult<Result.RejectRequestByGuildNameResult>> callback
        ) =>
            new RejectRequestByGuildNameTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToCoroutine(callback);

        public IFuture<Result.RejectRequestByGuildNameResult> RejectRequestByGuildNameFuture(
                Request.RejectRequestByGuildNameRequest request
        ) =>
            new RejectRequestByGuildNameTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
        public UniTask<Result.RejectRequestByGuildNameResult> RejectRequestByGuildNameAsync(
            Request.RejectRequestByGuildNameRequest request
        ) =>
            new RejectRequestByGuildNameTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsUniTask<Result.RejectRequestByGuildNameResult>();
    #else
        public RejectRequestByGuildNameTask RejectRequestByGuildNameAsync(
                Request.RejectRequestByGuildNameRequest request
        )
        {
            return new RejectRequestByGuildNameTask(
                Gs2WebSocketSession,
                request
            );
        }
    #endif
#else
        public Task<Result.RejectRequestByGuildNameResult> RejectRequestByGuildNameAsync(
            Request.RejectRequestByGuildNameRequest request
        ) =>
            new RejectRequestByGuildNameTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsTask();
#endif


        public class GetSendRequestTask : Gs2WebSocketSessionTask<Request.GetSendRequestRequest, Result.GetSendRequestResult>
        {
            public GetSendRequestTask(IGs2Session session, Request.GetSendRequestRequest request) : base(session, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(Request.GetSendRequestRequest request)
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
                if (request.GuildModelName != null)
                {
                    jsonWriter.WritePropertyName("guildModelName");
                    jsonWriter.Write(request.GuildModelName.ToString());
                }
                if (request.TargetGuildName != null)
                {
                    jsonWriter.WritePropertyName("targetGuildName");
                    jsonWriter.Write(request.TargetGuildName.ToString());
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
                    "guild",
                    "sendMemberRequest",
                    "getSendRequest",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
        public IEnumerator GetSendRequest(
                Request.GetSendRequestRequest request,
                UnityAction<AsyncResult<Result.GetSendRequestResult>> callback
        ) =>
            new GetSendRequestTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToCoroutine(callback);

        public IFuture<Result.GetSendRequestResult> GetSendRequestFuture(
                Request.GetSendRequestRequest request
        ) =>
            new GetSendRequestTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
        public UniTask<Result.GetSendRequestResult> GetSendRequestAsync(
            Request.GetSendRequestRequest request
        ) =>
            new GetSendRequestTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsUniTask<Result.GetSendRequestResult>();
    #else
        public GetSendRequestTask GetSendRequestAsync(
                Request.GetSendRequestRequest request
        )
        {
            return new GetSendRequestTask(
                Gs2WebSocketSession,
                request
            );
        }
    #endif
#else
        public Task<Result.GetSendRequestResult> GetSendRequestAsync(
            Request.GetSendRequestRequest request
        ) =>
            new GetSendRequestTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsTask();
#endif


        public class GetSendRequestByUserIdTask : Gs2WebSocketSessionTask<Request.GetSendRequestByUserIdRequest, Result.GetSendRequestByUserIdResult>
        {
            public GetSendRequestByUserIdTask(IGs2Session session, Request.GetSendRequestByUserIdRequest request) : base(session, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(Request.GetSendRequestByUserIdRequest request)
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
                if (request.GuildModelName != null)
                {
                    jsonWriter.WritePropertyName("guildModelName");
                    jsonWriter.Write(request.GuildModelName.ToString());
                }
                if (request.TargetGuildName != null)
                {
                    jsonWriter.WritePropertyName("targetGuildName");
                    jsonWriter.Write(request.TargetGuildName.ToString());
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
                    "guild",
                    "sendMemberRequest",
                    "getSendRequestByUserId",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
        public IEnumerator GetSendRequestByUserId(
                Request.GetSendRequestByUserIdRequest request,
                UnityAction<AsyncResult<Result.GetSendRequestByUserIdResult>> callback
        ) =>
            new GetSendRequestByUserIdTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToCoroutine(callback);

        public IFuture<Result.GetSendRequestByUserIdResult> GetSendRequestByUserIdFuture(
                Request.GetSendRequestByUserIdRequest request
        ) =>
            new GetSendRequestByUserIdTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
        public UniTask<Result.GetSendRequestByUserIdResult> GetSendRequestByUserIdAsync(
            Request.GetSendRequestByUserIdRequest request
        ) =>
            new GetSendRequestByUserIdTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsUniTask<Result.GetSendRequestByUserIdResult>();
    #else
        public GetSendRequestByUserIdTask GetSendRequestByUserIdAsync(
                Request.GetSendRequestByUserIdRequest request
        )
        {
            return new GetSendRequestByUserIdTask(
                Gs2WebSocketSession,
                request
            );
        }
    #endif
#else
        public Task<Result.GetSendRequestByUserIdResult> GetSendRequestByUserIdAsync(
            Request.GetSendRequestByUserIdRequest request
        ) =>
            new GetSendRequestByUserIdTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsTask();
#endif


        public class DeleteRequestTask : Gs2WebSocketSessionTask<Request.DeleteRequestRequest, Result.DeleteRequestResult>
        {
            public DeleteRequestTask(IGs2Session session, Request.DeleteRequestRequest request) : base(session, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(Request.DeleteRequestRequest request)
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
                if (request.GuildModelName != null)
                {
                    jsonWriter.WritePropertyName("guildModelName");
                    jsonWriter.Write(request.GuildModelName.ToString());
                }
                if (request.TargetGuildName != null)
                {
                    jsonWriter.WritePropertyName("targetGuildName");
                    jsonWriter.Write(request.TargetGuildName.ToString());
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
                    "guild",
                    "sendMemberRequest",
                    "deleteRequest",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
        public IEnumerator DeleteRequest(
                Request.DeleteRequestRequest request,
                UnityAction<AsyncResult<Result.DeleteRequestResult>> callback
        ) =>
            new DeleteRequestTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToCoroutine(callback);

        public IFuture<Result.DeleteRequestResult> DeleteRequestFuture(
                Request.DeleteRequestRequest request
        ) =>
            new DeleteRequestTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
        public UniTask<Result.DeleteRequestResult> DeleteRequestAsync(
            Request.DeleteRequestRequest request
        ) =>
            new DeleteRequestTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsUniTask<Result.DeleteRequestResult>();
    #else
        public DeleteRequestTask DeleteRequestAsync(
                Request.DeleteRequestRequest request
        )
        {
            return new DeleteRequestTask(
                Gs2WebSocketSession,
                request
            );
        }
    #endif
#else
        public Task<Result.DeleteRequestResult> DeleteRequestAsync(
            Request.DeleteRequestRequest request
        ) =>
            new DeleteRequestTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsTask();
#endif


        public class DeleteRequestByUserIdTask : Gs2WebSocketSessionTask<Request.DeleteRequestByUserIdRequest, Result.DeleteRequestByUserIdResult>
        {
            public DeleteRequestByUserIdTask(IGs2Session session, Request.DeleteRequestByUserIdRequest request) : base(session, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(Request.DeleteRequestByUserIdRequest request)
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
                if (request.GuildModelName != null)
                {
                    jsonWriter.WritePropertyName("guildModelName");
                    jsonWriter.Write(request.GuildModelName.ToString());
                }
                if (request.TargetGuildName != null)
                {
                    jsonWriter.WritePropertyName("targetGuildName");
                    jsonWriter.Write(request.TargetGuildName.ToString());
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
                    "guild",
                    "sendMemberRequest",
                    "deleteRequestByUserId",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
        public IEnumerator DeleteRequestByUserId(
                Request.DeleteRequestByUserIdRequest request,
                UnityAction<AsyncResult<Result.DeleteRequestByUserIdResult>> callback
        ) =>
            new DeleteRequestByUserIdTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToCoroutine(callback);

        public IFuture<Result.DeleteRequestByUserIdResult> DeleteRequestByUserIdFuture(
                Request.DeleteRequestByUserIdRequest request
        ) =>
            new DeleteRequestByUserIdTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
        public UniTask<Result.DeleteRequestByUserIdResult> DeleteRequestByUserIdAsync(
            Request.DeleteRequestByUserIdRequest request
        ) =>
            new DeleteRequestByUserIdTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsUniTask<Result.DeleteRequestByUserIdResult>();
    #else
        public DeleteRequestByUserIdTask DeleteRequestByUserIdAsync(
                Request.DeleteRequestByUserIdRequest request
        )
        {
            return new DeleteRequestByUserIdTask(
                Gs2WebSocketSession,
                request
            );
        }
    #endif
#else
        public Task<Result.DeleteRequestByUserIdResult> DeleteRequestByUserIdAsync(
            Request.DeleteRequestByUserIdRequest request
        ) =>
            new DeleteRequestByUserIdTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsTask();
#endif


        public class GetIgnoreUserTask : Gs2WebSocketSessionTask<Request.GetIgnoreUserRequest, Result.GetIgnoreUserResult>
        {
            public GetIgnoreUserTask(IGs2Session session, Request.GetIgnoreUserRequest request) : base(session, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(Request.GetIgnoreUserRequest request)
            {
                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);

                jsonWriter.WriteObjectStart();

                if (request.NamespaceName != null)
                {
                    jsonWriter.WritePropertyName("namespaceName");
                    jsonWriter.Write(request.NamespaceName.ToString());
                }
                if (request.GuildModelName != null)
                {
                    jsonWriter.WritePropertyName("guildModelName");
                    jsonWriter.Write(request.GuildModelName.ToString());
                }
                if (request.AccessToken != null)
                {
                    jsonWriter.WritePropertyName("accessToken");
                    jsonWriter.Write(request.AccessToken.ToString());
                }
                if (request.UserId != null)
                {
                    jsonWriter.WritePropertyName("userId");
                    jsonWriter.Write(request.UserId.ToString());
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
                    "guild",
                    "ignoreUser",
                    "getIgnoreUser",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
        public IEnumerator GetIgnoreUser(
                Request.GetIgnoreUserRequest request,
                UnityAction<AsyncResult<Result.GetIgnoreUserResult>> callback
        ) =>
            new GetIgnoreUserTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToCoroutine(callback);

        public IFuture<Result.GetIgnoreUserResult> GetIgnoreUserFuture(
                Request.GetIgnoreUserRequest request
        ) =>
            new GetIgnoreUserTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
        public UniTask<Result.GetIgnoreUserResult> GetIgnoreUserAsync(
            Request.GetIgnoreUserRequest request
        ) =>
            new GetIgnoreUserTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsUniTask<Result.GetIgnoreUserResult>();
    #else
        public GetIgnoreUserTask GetIgnoreUserAsync(
                Request.GetIgnoreUserRequest request
        )
        {
            return new GetIgnoreUserTask(
                Gs2WebSocketSession,
                request
            );
        }
    #endif
#else
        public Task<Result.GetIgnoreUserResult> GetIgnoreUserAsync(
            Request.GetIgnoreUserRequest request
        ) =>
            new GetIgnoreUserTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsTask();
#endif


        public class GetIgnoreUserByGuildNameTask : Gs2WebSocketSessionTask<Request.GetIgnoreUserByGuildNameRequest, Result.GetIgnoreUserByGuildNameResult>
        {
            public GetIgnoreUserByGuildNameTask(IGs2Session session, Request.GetIgnoreUserByGuildNameRequest request) : base(session, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(Request.GetIgnoreUserByGuildNameRequest request)
            {
                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);

                jsonWriter.WriteObjectStart();

                if (request.NamespaceName != null)
                {
                    jsonWriter.WritePropertyName("namespaceName");
                    jsonWriter.Write(request.NamespaceName.ToString());
                }
                if (request.GuildModelName != null)
                {
                    jsonWriter.WritePropertyName("guildModelName");
                    jsonWriter.Write(request.GuildModelName.ToString());
                }
                if (request.GuildName != null)
                {
                    jsonWriter.WritePropertyName("guildName");
                    jsonWriter.Write(request.GuildName.ToString());
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
                    "guild",
                    "ignoreUser",
                    "getIgnoreUserByGuildName",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
        public IEnumerator GetIgnoreUserByGuildName(
                Request.GetIgnoreUserByGuildNameRequest request,
                UnityAction<AsyncResult<Result.GetIgnoreUserByGuildNameResult>> callback
        ) =>
            new GetIgnoreUserByGuildNameTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToCoroutine(callback);

        public IFuture<Result.GetIgnoreUserByGuildNameResult> GetIgnoreUserByGuildNameFuture(
                Request.GetIgnoreUserByGuildNameRequest request
        ) =>
            new GetIgnoreUserByGuildNameTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
        public UniTask<Result.GetIgnoreUserByGuildNameResult> GetIgnoreUserByGuildNameAsync(
            Request.GetIgnoreUserByGuildNameRequest request
        ) =>
            new GetIgnoreUserByGuildNameTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsUniTask<Result.GetIgnoreUserByGuildNameResult>();
    #else
        public GetIgnoreUserByGuildNameTask GetIgnoreUserByGuildNameAsync(
                Request.GetIgnoreUserByGuildNameRequest request
        )
        {
            return new GetIgnoreUserByGuildNameTask(
                Gs2WebSocketSession,
                request
            );
        }
    #endif
#else
        public Task<Result.GetIgnoreUserByGuildNameResult> GetIgnoreUserByGuildNameAsync(
            Request.GetIgnoreUserByGuildNameRequest request
        ) =>
            new GetIgnoreUserByGuildNameTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsTask();
#endif


        public class DeleteIgnoreUserTask : Gs2WebSocketSessionTask<Request.DeleteIgnoreUserRequest, Result.DeleteIgnoreUserResult>
        {
            public DeleteIgnoreUserTask(IGs2Session session, Request.DeleteIgnoreUserRequest request) : base(session, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(Request.DeleteIgnoreUserRequest request)
            {
                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);

                jsonWriter.WriteObjectStart();

                if (request.NamespaceName != null)
                {
                    jsonWriter.WritePropertyName("namespaceName");
                    jsonWriter.Write(request.NamespaceName.ToString());
                }
                if (request.GuildModelName != null)
                {
                    jsonWriter.WritePropertyName("guildModelName");
                    jsonWriter.Write(request.GuildModelName.ToString());
                }
                if (request.AccessToken != null)
                {
                    jsonWriter.WritePropertyName("accessToken");
                    jsonWriter.Write(request.AccessToken.ToString());
                }
                if (request.UserId != null)
                {
                    jsonWriter.WritePropertyName("userId");
                    jsonWriter.Write(request.UserId.ToString());
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
                    "guild",
                    "ignoreUser",
                    "deleteIgnoreUser",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
        public IEnumerator DeleteIgnoreUser(
                Request.DeleteIgnoreUserRequest request,
                UnityAction<AsyncResult<Result.DeleteIgnoreUserResult>> callback
        ) =>
            new DeleteIgnoreUserTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToCoroutine(callback);

        public IFuture<Result.DeleteIgnoreUserResult> DeleteIgnoreUserFuture(
                Request.DeleteIgnoreUserRequest request
        ) =>
            new DeleteIgnoreUserTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
        public UniTask<Result.DeleteIgnoreUserResult> DeleteIgnoreUserAsync(
            Request.DeleteIgnoreUserRequest request
        ) =>
            new DeleteIgnoreUserTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsUniTask<Result.DeleteIgnoreUserResult>();
    #else
        public DeleteIgnoreUserTask DeleteIgnoreUserAsync(
                Request.DeleteIgnoreUserRequest request
        )
        {
            return new DeleteIgnoreUserTask(
                Gs2WebSocketSession,
                request
            );
        }
    #endif
#else
        public Task<Result.DeleteIgnoreUserResult> DeleteIgnoreUserAsync(
            Request.DeleteIgnoreUserRequest request
        ) =>
            new DeleteIgnoreUserTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsTask();
#endif


        public class DeleteIgnoreUserByGuildNameTask : Gs2WebSocketSessionTask<Request.DeleteIgnoreUserByGuildNameRequest, Result.DeleteIgnoreUserByGuildNameResult>
        {
            public DeleteIgnoreUserByGuildNameTask(IGs2Session session, Request.DeleteIgnoreUserByGuildNameRequest request) : base(session, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(Request.DeleteIgnoreUserByGuildNameRequest request)
            {
                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);

                jsonWriter.WriteObjectStart();

                if (request.NamespaceName != null)
                {
                    jsonWriter.WritePropertyName("namespaceName");
                    jsonWriter.Write(request.NamespaceName.ToString());
                }
                if (request.GuildModelName != null)
                {
                    jsonWriter.WritePropertyName("guildModelName");
                    jsonWriter.Write(request.GuildModelName.ToString());
                }
                if (request.GuildName != null)
                {
                    jsonWriter.WritePropertyName("guildName");
                    jsonWriter.Write(request.GuildName.ToString());
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
                    "guild",
                    "ignoreUser",
                    "deleteIgnoreUserByGuildName",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
        public IEnumerator DeleteIgnoreUserByGuildName(
                Request.DeleteIgnoreUserByGuildNameRequest request,
                UnityAction<AsyncResult<Result.DeleteIgnoreUserByGuildNameResult>> callback
        ) =>
            new DeleteIgnoreUserByGuildNameTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToCoroutine(callback);

        public IFuture<Result.DeleteIgnoreUserByGuildNameResult> DeleteIgnoreUserByGuildNameFuture(
                Request.DeleteIgnoreUserByGuildNameRequest request
        ) =>
            new DeleteIgnoreUserByGuildNameTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
        public UniTask<Result.DeleteIgnoreUserByGuildNameResult> DeleteIgnoreUserByGuildNameAsync(
            Request.DeleteIgnoreUserByGuildNameRequest request
        ) =>
            new DeleteIgnoreUserByGuildNameTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsUniTask<Result.DeleteIgnoreUserByGuildNameResult>();
    #else
        public DeleteIgnoreUserByGuildNameTask DeleteIgnoreUserByGuildNameAsync(
                Request.DeleteIgnoreUserByGuildNameRequest request
        )
        {
            return new DeleteIgnoreUserByGuildNameTask(
                Gs2WebSocketSession,
                request
            );
        }
    #endif
#else
        public Task<Result.DeleteIgnoreUserByGuildNameResult> DeleteIgnoreUserByGuildNameAsync(
            Request.DeleteIgnoreUserByGuildNameRequest request
        ) =>
            new DeleteIgnoreUserByGuildNameTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsTask();
#endif
	}
}