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

namespace Gs2.Gs2Money2
{
	public class Gs2Money2WebSocketClient : AbstractGs2Client
	{

		public static string Endpoint = "money2";

        protected Gs2WebSocketSession Gs2WebSocketSession => (Gs2WebSocketSession) Gs2Session;

		public Gs2Money2WebSocketClient(Gs2WebSocketSession Gs2WebSocketSession) : base(Gs2WebSocketSession)
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
                    "money2",
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
                    "money2",
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
                    "money2",
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
                    "money2",
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
                    "money2",
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
                    "money2",
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
                    "money2",
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
                    "money2",
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
                    "money2",
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


        public class GetWalletTask : Gs2WebSocketSessionTask<Request.GetWalletRequest, Result.GetWalletResult>
        {
            public GetWalletTask(IGs2Session session, Request.GetWalletRequest request) : base(session, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(Request.GetWalletRequest request)
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
                if (request.Slot != null)
                {
                    jsonWriter.WritePropertyName("slot");
                    jsonWriter.Write(request.Slot.ToString());
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
                    "money2",
                    "wallet",
                    "getWallet",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
        public IEnumerator GetWallet(
                Request.GetWalletRequest request,
                UnityAction<AsyncResult<Result.GetWalletResult>> callback
        ) =>
            new GetWalletTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToCoroutine(callback);

        public IFuture<Result.GetWalletResult> GetWalletFuture(
                Request.GetWalletRequest request
        ) =>
            new GetWalletTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
        public UniTask<Result.GetWalletResult> GetWalletAsync(
            Request.GetWalletRequest request
        ) =>
            new GetWalletTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsUniTask<Result.GetWalletResult>();
    #else
        public GetWalletTask GetWalletAsync(
                Request.GetWalletRequest request
        )
        {
            return new GetWalletTask(
                Gs2WebSocketSession,
                request
            );
        }
    #endif
#else
        public Task<Result.GetWalletResult> GetWalletAsync(
            Request.GetWalletRequest request
        ) =>
            new GetWalletTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsTask();
#endif


        public class GetWalletByUserIdTask : Gs2WebSocketSessionTask<Request.GetWalletByUserIdRequest, Result.GetWalletByUserIdResult>
        {
            public GetWalletByUserIdTask(IGs2Session session, Request.GetWalletByUserIdRequest request) : base(session, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(Request.GetWalletByUserIdRequest request)
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
                if (request.Slot != null)
                {
                    jsonWriter.WritePropertyName("slot");
                    jsonWriter.Write(request.Slot.ToString());
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
                    "money2",
                    "wallet",
                    "getWalletByUserId",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
        public IEnumerator GetWalletByUserId(
                Request.GetWalletByUserIdRequest request,
                UnityAction<AsyncResult<Result.GetWalletByUserIdResult>> callback
        ) =>
            new GetWalletByUserIdTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToCoroutine(callback);

        public IFuture<Result.GetWalletByUserIdResult> GetWalletByUserIdFuture(
                Request.GetWalletByUserIdRequest request
        ) =>
            new GetWalletByUserIdTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
        public UniTask<Result.GetWalletByUserIdResult> GetWalletByUserIdAsync(
            Request.GetWalletByUserIdRequest request
        ) =>
            new GetWalletByUserIdTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsUniTask<Result.GetWalletByUserIdResult>();
    #else
        public GetWalletByUserIdTask GetWalletByUserIdAsync(
                Request.GetWalletByUserIdRequest request
        )
        {
            return new GetWalletByUserIdTask(
                Gs2WebSocketSession,
                request
            );
        }
    #endif
#else
        public Task<Result.GetWalletByUserIdResult> GetWalletByUserIdAsync(
            Request.GetWalletByUserIdRequest request
        ) =>
            new GetWalletByUserIdTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsTask();
#endif


        public class DepositByUserIdTask : Gs2WebSocketSessionTask<Request.DepositByUserIdRequest, Result.DepositByUserIdResult>
        {
            public DepositByUserIdTask(IGs2Session session, Request.DepositByUserIdRequest request) : base(session, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(Request.DepositByUserIdRequest request)
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
                if (request.Slot != null)
                {
                    jsonWriter.WritePropertyName("slot");
                    jsonWriter.Write(request.Slot.ToString());
                }
                if (request.DepositTransactions != null)
                {
                    jsonWriter.WritePropertyName("depositTransactions");
                    jsonWriter.WriteArrayStart();
                    foreach(var item in request.DepositTransactions)
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
                    "money2",
                    "wallet",
                    "depositByUserId",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }

            public override void OnError(Gs2.Core.Exception.Gs2Exception error)
            {
                if (error.Errors.Count(v => v.code == "wallet.operation.conflict") > 0) {
                    base.OnError(new Exception.ConflictException(error));
                }
                else {
                    base.OnError(error);
                }
            }
        }

#if UNITY_2017_1_OR_NEWER
        public IEnumerator DepositByUserId(
                Request.DepositByUserIdRequest request,
                UnityAction<AsyncResult<Result.DepositByUserIdResult>> callback
        ) =>
            new DepositByUserIdTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToCoroutine(callback);

        public IFuture<Result.DepositByUserIdResult> DepositByUserIdFuture(
                Request.DepositByUserIdRequest request
        ) =>
            new DepositByUserIdTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
        public UniTask<Result.DepositByUserIdResult> DepositByUserIdAsync(
            Request.DepositByUserIdRequest request
        ) =>
            new DepositByUserIdTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsUniTask<Result.DepositByUserIdResult>();
    #else
        public DepositByUserIdTask DepositByUserIdAsync(
                Request.DepositByUserIdRequest request
        )
        {
            return new DepositByUserIdTask(
                Gs2WebSocketSession,
                request
            );
        }
    #endif
#else
        public Task<Result.DepositByUserIdResult> DepositByUserIdAsync(
            Request.DepositByUserIdRequest request
        ) =>
            new DepositByUserIdTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsTask();
#endif


        public class DepositByStampSheetTask : Gs2WebSocketSessionTask<Request.DepositByStampSheetRequest, Result.DepositByStampSheetResult>
        {
            public DepositByStampSheetTask(IGs2Session session, Request.DepositByStampSheetRequest request) : base(session, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(Request.DepositByStampSheetRequest request)
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
                    "money2",
                    "wallet",
                    "depositByStampSheet",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
        public IEnumerator DepositByStampSheet(
                Request.DepositByStampSheetRequest request,
                UnityAction<AsyncResult<Result.DepositByStampSheetResult>> callback
        ) =>
            new DepositByStampSheetTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToCoroutine(callback);

        public IFuture<Result.DepositByStampSheetResult> DepositByStampSheetFuture(
                Request.DepositByStampSheetRequest request
        ) =>
            new DepositByStampSheetTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
        public UniTask<Result.DepositByStampSheetResult> DepositByStampSheetAsync(
            Request.DepositByStampSheetRequest request
        ) =>
            new DepositByStampSheetTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsUniTask<Result.DepositByStampSheetResult>();
    #else
        public DepositByStampSheetTask DepositByStampSheetAsync(
                Request.DepositByStampSheetRequest request
        )
        {
            return new DepositByStampSheetTask(
                Gs2WebSocketSession,
                request
            );
        }
    #endif
#else
        public Task<Result.DepositByStampSheetResult> DepositByStampSheetAsync(
            Request.DepositByStampSheetRequest request
        ) =>
            new DepositByStampSheetTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsTask();
#endif


        public class GetRefundHistoryTask : Gs2WebSocketSessionTask<Request.GetRefundHistoryRequest, Result.GetRefundHistoryResult>
        {
            public GetRefundHistoryTask(IGs2Session session, Request.GetRefundHistoryRequest request) : base(session, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(Request.GetRefundHistoryRequest request)
            {
                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);

                jsonWriter.WriteObjectStart();

                if (request.NamespaceName != null)
                {
                    jsonWriter.WritePropertyName("namespaceName");
                    jsonWriter.Write(request.NamespaceName.ToString());
                }
                if (request.TransactionId != null)
                {
                    jsonWriter.WritePropertyName("transactionId");
                    jsonWriter.Write(request.TransactionId.ToString());
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
                    "money2",
                    "refundHistory",
                    "getRefundHistory",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
        public IEnumerator GetRefundHistory(
                Request.GetRefundHistoryRequest request,
                UnityAction<AsyncResult<Result.GetRefundHistoryResult>> callback
        ) =>
            new GetRefundHistoryTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToCoroutine(callback);

        public IFuture<Result.GetRefundHistoryResult> GetRefundHistoryFuture(
                Request.GetRefundHistoryRequest request
        ) =>
            new GetRefundHistoryTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
        public UniTask<Result.GetRefundHistoryResult> GetRefundHistoryAsync(
            Request.GetRefundHistoryRequest request
        ) =>
            new GetRefundHistoryTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsUniTask<Result.GetRefundHistoryResult>();
    #else
        public GetRefundHistoryTask GetRefundHistoryAsync(
                Request.GetRefundHistoryRequest request
        )
        {
            return new GetRefundHistoryTask(
                Gs2WebSocketSession,
                request
            );
        }
    #endif
#else
        public Task<Result.GetRefundHistoryResult> GetRefundHistoryAsync(
            Request.GetRefundHistoryRequest request
        ) =>
            new GetRefundHistoryTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsTask();
#endif


        public class GetStoreContentModelTask : Gs2WebSocketSessionTask<Request.GetStoreContentModelRequest, Result.GetStoreContentModelResult>
        {
            public GetStoreContentModelTask(IGs2Session session, Request.GetStoreContentModelRequest request) : base(session, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(Request.GetStoreContentModelRequest request)
            {
                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);

                jsonWriter.WriteObjectStart();

                if (request.NamespaceName != null)
                {
                    jsonWriter.WritePropertyName("namespaceName");
                    jsonWriter.Write(request.NamespaceName.ToString());
                }
                if (request.ContentName != null)
                {
                    jsonWriter.WritePropertyName("contentName");
                    jsonWriter.Write(request.ContentName.ToString());
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
                    "money2",
                    "storeContentModel",
                    "getStoreContentModel",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
        public IEnumerator GetStoreContentModel(
                Request.GetStoreContentModelRequest request,
                UnityAction<AsyncResult<Result.GetStoreContentModelResult>> callback
        ) =>
            new GetStoreContentModelTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToCoroutine(callback);

        public IFuture<Result.GetStoreContentModelResult> GetStoreContentModelFuture(
                Request.GetStoreContentModelRequest request
        ) =>
            new GetStoreContentModelTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
        public UniTask<Result.GetStoreContentModelResult> GetStoreContentModelAsync(
            Request.GetStoreContentModelRequest request
        ) =>
            new GetStoreContentModelTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsUniTask<Result.GetStoreContentModelResult>();
    #else
        public GetStoreContentModelTask GetStoreContentModelAsync(
                Request.GetStoreContentModelRequest request
        )
        {
            return new GetStoreContentModelTask(
                Gs2WebSocketSession,
                request
            );
        }
    #endif
#else
        public Task<Result.GetStoreContentModelResult> GetStoreContentModelAsync(
            Request.GetStoreContentModelRequest request
        ) =>
            new GetStoreContentModelTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsTask();
#endif


        public class CreateStoreContentModelMasterTask : Gs2WebSocketSessionTask<Request.CreateStoreContentModelMasterRequest, Result.CreateStoreContentModelMasterResult>
        {
            public CreateStoreContentModelMasterTask(IGs2Session session, Request.CreateStoreContentModelMasterRequest request) : base(session, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(Request.CreateStoreContentModelMasterRequest request)
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
                if (request.AppleAppStore != null)
                {
                    jsonWriter.WritePropertyName("appleAppStore");
                    request.AppleAppStore.WriteJson(jsonWriter);
                }
                if (request.GooglePlay != null)
                {
                    jsonWriter.WritePropertyName("googlePlay");
                    request.GooglePlay.WriteJson(jsonWriter);
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
                    "money2",
                    "storeContentModelMaster",
                    "createStoreContentModelMaster",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
        public IEnumerator CreateStoreContentModelMaster(
                Request.CreateStoreContentModelMasterRequest request,
                UnityAction<AsyncResult<Result.CreateStoreContentModelMasterResult>> callback
        ) =>
            new CreateStoreContentModelMasterTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToCoroutine(callback);

        public IFuture<Result.CreateStoreContentModelMasterResult> CreateStoreContentModelMasterFuture(
                Request.CreateStoreContentModelMasterRequest request
        ) =>
            new CreateStoreContentModelMasterTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
        public UniTask<Result.CreateStoreContentModelMasterResult> CreateStoreContentModelMasterAsync(
            Request.CreateStoreContentModelMasterRequest request
        ) =>
            new CreateStoreContentModelMasterTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsUniTask<Result.CreateStoreContentModelMasterResult>();
    #else
        public CreateStoreContentModelMasterTask CreateStoreContentModelMasterAsync(
                Request.CreateStoreContentModelMasterRequest request
        )
        {
            return new CreateStoreContentModelMasterTask(
                Gs2WebSocketSession,
                request
            );
        }
    #endif
#else
        public Task<Result.CreateStoreContentModelMasterResult> CreateStoreContentModelMasterAsync(
            Request.CreateStoreContentModelMasterRequest request
        ) =>
            new CreateStoreContentModelMasterTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsTask();
#endif


        public class GetStoreContentModelMasterTask : Gs2WebSocketSessionTask<Request.GetStoreContentModelMasterRequest, Result.GetStoreContentModelMasterResult>
        {
            public GetStoreContentModelMasterTask(IGs2Session session, Request.GetStoreContentModelMasterRequest request) : base(session, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(Request.GetStoreContentModelMasterRequest request)
            {
                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);

                jsonWriter.WriteObjectStart();

                if (request.NamespaceName != null)
                {
                    jsonWriter.WritePropertyName("namespaceName");
                    jsonWriter.Write(request.NamespaceName.ToString());
                }
                if (request.ContentName != null)
                {
                    jsonWriter.WritePropertyName("contentName");
                    jsonWriter.Write(request.ContentName.ToString());
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
                    "money2",
                    "storeContentModelMaster",
                    "getStoreContentModelMaster",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
        public IEnumerator GetStoreContentModelMaster(
                Request.GetStoreContentModelMasterRequest request,
                UnityAction<AsyncResult<Result.GetStoreContentModelMasterResult>> callback
        ) =>
            new GetStoreContentModelMasterTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToCoroutine(callback);

        public IFuture<Result.GetStoreContentModelMasterResult> GetStoreContentModelMasterFuture(
                Request.GetStoreContentModelMasterRequest request
        ) =>
            new GetStoreContentModelMasterTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
        public UniTask<Result.GetStoreContentModelMasterResult> GetStoreContentModelMasterAsync(
            Request.GetStoreContentModelMasterRequest request
        ) =>
            new GetStoreContentModelMasterTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsUniTask<Result.GetStoreContentModelMasterResult>();
    #else
        public GetStoreContentModelMasterTask GetStoreContentModelMasterAsync(
                Request.GetStoreContentModelMasterRequest request
        )
        {
            return new GetStoreContentModelMasterTask(
                Gs2WebSocketSession,
                request
            );
        }
    #endif
#else
        public Task<Result.GetStoreContentModelMasterResult> GetStoreContentModelMasterAsync(
            Request.GetStoreContentModelMasterRequest request
        ) =>
            new GetStoreContentModelMasterTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsTask();
#endif


        public class UpdateStoreContentModelMasterTask : Gs2WebSocketSessionTask<Request.UpdateStoreContentModelMasterRequest, Result.UpdateStoreContentModelMasterResult>
        {
            public UpdateStoreContentModelMasterTask(IGs2Session session, Request.UpdateStoreContentModelMasterRequest request) : base(session, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(Request.UpdateStoreContentModelMasterRequest request)
            {
                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);

                jsonWriter.WriteObjectStart();

                if (request.NamespaceName != null)
                {
                    jsonWriter.WritePropertyName("namespaceName");
                    jsonWriter.Write(request.NamespaceName.ToString());
                }
                if (request.ContentName != null)
                {
                    jsonWriter.WritePropertyName("contentName");
                    jsonWriter.Write(request.ContentName.ToString());
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
                if (request.AppleAppStore != null)
                {
                    jsonWriter.WritePropertyName("appleAppStore");
                    request.AppleAppStore.WriteJson(jsonWriter);
                }
                if (request.GooglePlay != null)
                {
                    jsonWriter.WritePropertyName("googlePlay");
                    request.GooglePlay.WriteJson(jsonWriter);
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
                    "money2",
                    "storeContentModelMaster",
                    "updateStoreContentModelMaster",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
        public IEnumerator UpdateStoreContentModelMaster(
                Request.UpdateStoreContentModelMasterRequest request,
                UnityAction<AsyncResult<Result.UpdateStoreContentModelMasterResult>> callback
        ) =>
            new UpdateStoreContentModelMasterTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToCoroutine(callback);

        public IFuture<Result.UpdateStoreContentModelMasterResult> UpdateStoreContentModelMasterFuture(
                Request.UpdateStoreContentModelMasterRequest request
        ) =>
            new UpdateStoreContentModelMasterTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
        public UniTask<Result.UpdateStoreContentModelMasterResult> UpdateStoreContentModelMasterAsync(
            Request.UpdateStoreContentModelMasterRequest request
        ) =>
            new UpdateStoreContentModelMasterTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsUniTask<Result.UpdateStoreContentModelMasterResult>();
    #else
        public UpdateStoreContentModelMasterTask UpdateStoreContentModelMasterAsync(
                Request.UpdateStoreContentModelMasterRequest request
        )
        {
            return new UpdateStoreContentModelMasterTask(
                Gs2WebSocketSession,
                request
            );
        }
    #endif
#else
        public Task<Result.UpdateStoreContentModelMasterResult> UpdateStoreContentModelMasterAsync(
            Request.UpdateStoreContentModelMasterRequest request
        ) =>
            new UpdateStoreContentModelMasterTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsTask();
#endif


        public class DeleteStoreContentModelMasterTask : Gs2WebSocketSessionTask<Request.DeleteStoreContentModelMasterRequest, Result.DeleteStoreContentModelMasterResult>
        {
            public DeleteStoreContentModelMasterTask(IGs2Session session, Request.DeleteStoreContentModelMasterRequest request) : base(session, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(Request.DeleteStoreContentModelMasterRequest request)
            {
                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);

                jsonWriter.WriteObjectStart();

                if (request.NamespaceName != null)
                {
                    jsonWriter.WritePropertyName("namespaceName");
                    jsonWriter.Write(request.NamespaceName.ToString());
                }
                if (request.ContentName != null)
                {
                    jsonWriter.WritePropertyName("contentName");
                    jsonWriter.Write(request.ContentName.ToString());
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
                    "money2",
                    "storeContentModelMaster",
                    "deleteStoreContentModelMaster",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
        public IEnumerator DeleteStoreContentModelMaster(
                Request.DeleteStoreContentModelMasterRequest request,
                UnityAction<AsyncResult<Result.DeleteStoreContentModelMasterResult>> callback
        ) =>
            new DeleteStoreContentModelMasterTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToCoroutine(callback);

        public IFuture<Result.DeleteStoreContentModelMasterResult> DeleteStoreContentModelMasterFuture(
                Request.DeleteStoreContentModelMasterRequest request
        ) =>
            new DeleteStoreContentModelMasterTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
        public UniTask<Result.DeleteStoreContentModelMasterResult> DeleteStoreContentModelMasterAsync(
            Request.DeleteStoreContentModelMasterRequest request
        ) =>
            new DeleteStoreContentModelMasterTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsUniTask<Result.DeleteStoreContentModelMasterResult>();
    #else
        public DeleteStoreContentModelMasterTask DeleteStoreContentModelMasterAsync(
                Request.DeleteStoreContentModelMasterRequest request
        )
        {
            return new DeleteStoreContentModelMasterTask(
                Gs2WebSocketSession,
                request
            );
        }
    #endif
#else
        public Task<Result.DeleteStoreContentModelMasterResult> DeleteStoreContentModelMasterAsync(
            Request.DeleteStoreContentModelMasterRequest request
        ) =>
            new DeleteStoreContentModelMasterTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsTask();
#endif


        public class GetStoreSubscriptionContentModelTask : Gs2WebSocketSessionTask<Request.GetStoreSubscriptionContentModelRequest, Result.GetStoreSubscriptionContentModelResult>
        {
            public GetStoreSubscriptionContentModelTask(IGs2Session session, Request.GetStoreSubscriptionContentModelRequest request) : base(session, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(Request.GetStoreSubscriptionContentModelRequest request)
            {
                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);

                jsonWriter.WriteObjectStart();

                if (request.NamespaceName != null)
                {
                    jsonWriter.WritePropertyName("namespaceName");
                    jsonWriter.Write(request.NamespaceName.ToString());
                }
                if (request.ContentName != null)
                {
                    jsonWriter.WritePropertyName("contentName");
                    jsonWriter.Write(request.ContentName.ToString());
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
                    "money2",
                    "storeSubscriptionContentModel",
                    "getStoreSubscriptionContentModel",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
        public IEnumerator GetStoreSubscriptionContentModel(
                Request.GetStoreSubscriptionContentModelRequest request,
                UnityAction<AsyncResult<Result.GetStoreSubscriptionContentModelResult>> callback
        ) =>
            new GetStoreSubscriptionContentModelTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToCoroutine(callback);

        public IFuture<Result.GetStoreSubscriptionContentModelResult> GetStoreSubscriptionContentModelFuture(
                Request.GetStoreSubscriptionContentModelRequest request
        ) =>
            new GetStoreSubscriptionContentModelTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
        public UniTask<Result.GetStoreSubscriptionContentModelResult> GetStoreSubscriptionContentModelAsync(
            Request.GetStoreSubscriptionContentModelRequest request
        ) =>
            new GetStoreSubscriptionContentModelTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsUniTask<Result.GetStoreSubscriptionContentModelResult>();
    #else
        public GetStoreSubscriptionContentModelTask GetStoreSubscriptionContentModelAsync(
                Request.GetStoreSubscriptionContentModelRequest request
        )
        {
            return new GetStoreSubscriptionContentModelTask(
                Gs2WebSocketSession,
                request
            );
        }
    #endif
#else
        public Task<Result.GetStoreSubscriptionContentModelResult> GetStoreSubscriptionContentModelAsync(
            Request.GetStoreSubscriptionContentModelRequest request
        ) =>
            new GetStoreSubscriptionContentModelTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsTask();
#endif


        public class CreateStoreSubscriptionContentModelMasterTask : Gs2WebSocketSessionTask<Request.CreateStoreSubscriptionContentModelMasterRequest, Result.CreateStoreSubscriptionContentModelMasterResult>
        {
            public CreateStoreSubscriptionContentModelMasterTask(IGs2Session session, Request.CreateStoreSubscriptionContentModelMasterRequest request) : base(session, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(Request.CreateStoreSubscriptionContentModelMasterRequest request)
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
                if (request.ScheduleNamespaceId != null)
                {
                    jsonWriter.WritePropertyName("scheduleNamespaceId");
                    jsonWriter.Write(request.ScheduleNamespaceId.ToString());
                }
                if (request.TriggerName != null)
                {
                    jsonWriter.WritePropertyName("triggerName");
                    jsonWriter.Write(request.TriggerName.ToString());
                }
                if (request.TriggerExtendMode != null)
                {
                    jsonWriter.WritePropertyName("triggerExtendMode");
                    jsonWriter.Write(request.TriggerExtendMode.ToString());
                }
                if (request.RollupHour != null)
                {
                    jsonWriter.WritePropertyName("rollupHour");
                    jsonWriter.Write(request.RollupHour.ToString());
                }
                if (request.ReallocateSpanDays != null)
                {
                    jsonWriter.WritePropertyName("reallocateSpanDays");
                    jsonWriter.Write(request.ReallocateSpanDays.ToString());
                }
                if (request.AppleAppStore != null)
                {
                    jsonWriter.WritePropertyName("appleAppStore");
                    request.AppleAppStore.WriteJson(jsonWriter);
                }
                if (request.GooglePlay != null)
                {
                    jsonWriter.WritePropertyName("googlePlay");
                    request.GooglePlay.WriteJson(jsonWriter);
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
                    "money2",
                    "storeSubscriptionContentModelMaster",
                    "createStoreSubscriptionContentModelMaster",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
        public IEnumerator CreateStoreSubscriptionContentModelMaster(
                Request.CreateStoreSubscriptionContentModelMasterRequest request,
                UnityAction<AsyncResult<Result.CreateStoreSubscriptionContentModelMasterResult>> callback
        ) =>
            new CreateStoreSubscriptionContentModelMasterTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToCoroutine(callback);

        public IFuture<Result.CreateStoreSubscriptionContentModelMasterResult> CreateStoreSubscriptionContentModelMasterFuture(
                Request.CreateStoreSubscriptionContentModelMasterRequest request
        ) =>
            new CreateStoreSubscriptionContentModelMasterTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
        public UniTask<Result.CreateStoreSubscriptionContentModelMasterResult> CreateStoreSubscriptionContentModelMasterAsync(
            Request.CreateStoreSubscriptionContentModelMasterRequest request
        ) =>
            new CreateStoreSubscriptionContentModelMasterTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsUniTask<Result.CreateStoreSubscriptionContentModelMasterResult>();
    #else
        public CreateStoreSubscriptionContentModelMasterTask CreateStoreSubscriptionContentModelMasterAsync(
                Request.CreateStoreSubscriptionContentModelMasterRequest request
        )
        {
            return new CreateStoreSubscriptionContentModelMasterTask(
                Gs2WebSocketSession,
                request
            );
        }
    #endif
#else
        public Task<Result.CreateStoreSubscriptionContentModelMasterResult> CreateStoreSubscriptionContentModelMasterAsync(
            Request.CreateStoreSubscriptionContentModelMasterRequest request
        ) =>
            new CreateStoreSubscriptionContentModelMasterTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsTask();
#endif


        public class GetStoreSubscriptionContentModelMasterTask : Gs2WebSocketSessionTask<Request.GetStoreSubscriptionContentModelMasterRequest, Result.GetStoreSubscriptionContentModelMasterResult>
        {
            public GetStoreSubscriptionContentModelMasterTask(IGs2Session session, Request.GetStoreSubscriptionContentModelMasterRequest request) : base(session, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(Request.GetStoreSubscriptionContentModelMasterRequest request)
            {
                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);

                jsonWriter.WriteObjectStart();

                if (request.NamespaceName != null)
                {
                    jsonWriter.WritePropertyName("namespaceName");
                    jsonWriter.Write(request.NamespaceName.ToString());
                }
                if (request.ContentName != null)
                {
                    jsonWriter.WritePropertyName("contentName");
                    jsonWriter.Write(request.ContentName.ToString());
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
                    "money2",
                    "storeSubscriptionContentModelMaster",
                    "getStoreSubscriptionContentModelMaster",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
        public IEnumerator GetStoreSubscriptionContentModelMaster(
                Request.GetStoreSubscriptionContentModelMasterRequest request,
                UnityAction<AsyncResult<Result.GetStoreSubscriptionContentModelMasterResult>> callback
        ) =>
            new GetStoreSubscriptionContentModelMasterTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToCoroutine(callback);

        public IFuture<Result.GetStoreSubscriptionContentModelMasterResult> GetStoreSubscriptionContentModelMasterFuture(
                Request.GetStoreSubscriptionContentModelMasterRequest request
        ) =>
            new GetStoreSubscriptionContentModelMasterTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
        public UniTask<Result.GetStoreSubscriptionContentModelMasterResult> GetStoreSubscriptionContentModelMasterAsync(
            Request.GetStoreSubscriptionContentModelMasterRequest request
        ) =>
            new GetStoreSubscriptionContentModelMasterTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsUniTask<Result.GetStoreSubscriptionContentModelMasterResult>();
    #else
        public GetStoreSubscriptionContentModelMasterTask GetStoreSubscriptionContentModelMasterAsync(
                Request.GetStoreSubscriptionContentModelMasterRequest request
        )
        {
            return new GetStoreSubscriptionContentModelMasterTask(
                Gs2WebSocketSession,
                request
            );
        }
    #endif
#else
        public Task<Result.GetStoreSubscriptionContentModelMasterResult> GetStoreSubscriptionContentModelMasterAsync(
            Request.GetStoreSubscriptionContentModelMasterRequest request
        ) =>
            new GetStoreSubscriptionContentModelMasterTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsTask();
#endif


        public class UpdateStoreSubscriptionContentModelMasterTask : Gs2WebSocketSessionTask<Request.UpdateStoreSubscriptionContentModelMasterRequest, Result.UpdateStoreSubscriptionContentModelMasterResult>
        {
            public UpdateStoreSubscriptionContentModelMasterTask(IGs2Session session, Request.UpdateStoreSubscriptionContentModelMasterRequest request) : base(session, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(Request.UpdateStoreSubscriptionContentModelMasterRequest request)
            {
                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);

                jsonWriter.WriteObjectStart();

                if (request.NamespaceName != null)
                {
                    jsonWriter.WritePropertyName("namespaceName");
                    jsonWriter.Write(request.NamespaceName.ToString());
                }
                if (request.ContentName != null)
                {
                    jsonWriter.WritePropertyName("contentName");
                    jsonWriter.Write(request.ContentName.ToString());
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
                if (request.ScheduleNamespaceId != null)
                {
                    jsonWriter.WritePropertyName("scheduleNamespaceId");
                    jsonWriter.Write(request.ScheduleNamespaceId.ToString());
                }
                if (request.TriggerName != null)
                {
                    jsonWriter.WritePropertyName("triggerName");
                    jsonWriter.Write(request.TriggerName.ToString());
                }
                if (request.TriggerExtendMode != null)
                {
                    jsonWriter.WritePropertyName("triggerExtendMode");
                    jsonWriter.Write(request.TriggerExtendMode.ToString());
                }
                if (request.RollupHour != null)
                {
                    jsonWriter.WritePropertyName("rollupHour");
                    jsonWriter.Write(request.RollupHour.ToString());
                }
                if (request.ReallocateSpanDays != null)
                {
                    jsonWriter.WritePropertyName("reallocateSpanDays");
                    jsonWriter.Write(request.ReallocateSpanDays.ToString());
                }
                if (request.AppleAppStore != null)
                {
                    jsonWriter.WritePropertyName("appleAppStore");
                    request.AppleAppStore.WriteJson(jsonWriter);
                }
                if (request.GooglePlay != null)
                {
                    jsonWriter.WritePropertyName("googlePlay");
                    request.GooglePlay.WriteJson(jsonWriter);
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
                    "money2",
                    "storeSubscriptionContentModelMaster",
                    "updateStoreSubscriptionContentModelMaster",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
        public IEnumerator UpdateStoreSubscriptionContentModelMaster(
                Request.UpdateStoreSubscriptionContentModelMasterRequest request,
                UnityAction<AsyncResult<Result.UpdateStoreSubscriptionContentModelMasterResult>> callback
        ) =>
            new UpdateStoreSubscriptionContentModelMasterTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToCoroutine(callback);

        public IFuture<Result.UpdateStoreSubscriptionContentModelMasterResult> UpdateStoreSubscriptionContentModelMasterFuture(
                Request.UpdateStoreSubscriptionContentModelMasterRequest request
        ) =>
            new UpdateStoreSubscriptionContentModelMasterTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
        public UniTask<Result.UpdateStoreSubscriptionContentModelMasterResult> UpdateStoreSubscriptionContentModelMasterAsync(
            Request.UpdateStoreSubscriptionContentModelMasterRequest request
        ) =>
            new UpdateStoreSubscriptionContentModelMasterTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsUniTask<Result.UpdateStoreSubscriptionContentModelMasterResult>();
    #else
        public UpdateStoreSubscriptionContentModelMasterTask UpdateStoreSubscriptionContentModelMasterAsync(
                Request.UpdateStoreSubscriptionContentModelMasterRequest request
        )
        {
            return new UpdateStoreSubscriptionContentModelMasterTask(
                Gs2WebSocketSession,
                request
            );
        }
    #endif
#else
        public Task<Result.UpdateStoreSubscriptionContentModelMasterResult> UpdateStoreSubscriptionContentModelMasterAsync(
            Request.UpdateStoreSubscriptionContentModelMasterRequest request
        ) =>
            new UpdateStoreSubscriptionContentModelMasterTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsTask();
#endif


        public class DeleteStoreSubscriptionContentModelMasterTask : Gs2WebSocketSessionTask<Request.DeleteStoreSubscriptionContentModelMasterRequest, Result.DeleteStoreSubscriptionContentModelMasterResult>
        {
            public DeleteStoreSubscriptionContentModelMasterTask(IGs2Session session, Request.DeleteStoreSubscriptionContentModelMasterRequest request) : base(session, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(Request.DeleteStoreSubscriptionContentModelMasterRequest request)
            {
                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);

                jsonWriter.WriteObjectStart();

                if (request.NamespaceName != null)
                {
                    jsonWriter.WritePropertyName("namespaceName");
                    jsonWriter.Write(request.NamespaceName.ToString());
                }
                if (request.ContentName != null)
                {
                    jsonWriter.WritePropertyName("contentName");
                    jsonWriter.Write(request.ContentName.ToString());
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
                    "money2",
                    "storeSubscriptionContentModelMaster",
                    "deleteStoreSubscriptionContentModelMaster",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
        public IEnumerator DeleteStoreSubscriptionContentModelMaster(
                Request.DeleteStoreSubscriptionContentModelMasterRequest request,
                UnityAction<AsyncResult<Result.DeleteStoreSubscriptionContentModelMasterResult>> callback
        ) =>
            new DeleteStoreSubscriptionContentModelMasterTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToCoroutine(callback);

        public IFuture<Result.DeleteStoreSubscriptionContentModelMasterResult> DeleteStoreSubscriptionContentModelMasterFuture(
                Request.DeleteStoreSubscriptionContentModelMasterRequest request
        ) =>
            new DeleteStoreSubscriptionContentModelMasterTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
        public UniTask<Result.DeleteStoreSubscriptionContentModelMasterResult> DeleteStoreSubscriptionContentModelMasterAsync(
            Request.DeleteStoreSubscriptionContentModelMasterRequest request
        ) =>
            new DeleteStoreSubscriptionContentModelMasterTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsUniTask<Result.DeleteStoreSubscriptionContentModelMasterResult>();
    #else
        public DeleteStoreSubscriptionContentModelMasterTask DeleteStoreSubscriptionContentModelMasterAsync(
                Request.DeleteStoreSubscriptionContentModelMasterRequest request
        )
        {
            return new DeleteStoreSubscriptionContentModelMasterTask(
                Gs2WebSocketSession,
                request
            );
        }
    #endif
#else
        public Task<Result.DeleteStoreSubscriptionContentModelMasterResult> DeleteStoreSubscriptionContentModelMasterAsync(
            Request.DeleteStoreSubscriptionContentModelMasterRequest request
        ) =>
            new DeleteStoreSubscriptionContentModelMasterTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsTask();
#endif


        public class PreUpdateCurrentModelMasterTask : Gs2WebSocketSessionTask<Request.PreUpdateCurrentModelMasterRequest, Result.PreUpdateCurrentModelMasterResult>
        {
            public PreUpdateCurrentModelMasterTask(IGs2Session session, Request.PreUpdateCurrentModelMasterRequest request) : base(session, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(Request.PreUpdateCurrentModelMasterRequest request)
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
                    "money2",
                    "currentModelMaster",
                    "preUpdateCurrentModelMaster",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
        public IEnumerator PreUpdateCurrentModelMaster(
                Request.PreUpdateCurrentModelMasterRequest request,
                UnityAction<AsyncResult<Result.PreUpdateCurrentModelMasterResult>> callback
        ) =>
            new PreUpdateCurrentModelMasterTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToCoroutine(callback);

        public IFuture<Result.PreUpdateCurrentModelMasterResult> PreUpdateCurrentModelMasterFuture(
                Request.PreUpdateCurrentModelMasterRequest request
        ) =>
            new PreUpdateCurrentModelMasterTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
        public UniTask<Result.PreUpdateCurrentModelMasterResult> PreUpdateCurrentModelMasterAsync(
            Request.PreUpdateCurrentModelMasterRequest request
        ) =>
            new PreUpdateCurrentModelMasterTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsUniTask<Result.PreUpdateCurrentModelMasterResult>();
    #else
        public PreUpdateCurrentModelMasterTask PreUpdateCurrentModelMasterAsync(
                Request.PreUpdateCurrentModelMasterRequest request
        )
        {
            return new PreUpdateCurrentModelMasterTask(
                Gs2WebSocketSession,
                request
            );
        }
    #endif
#else
        public Task<Result.PreUpdateCurrentModelMasterResult> PreUpdateCurrentModelMasterAsync(
            Request.PreUpdateCurrentModelMasterRequest request
        ) =>
            new PreUpdateCurrentModelMasterTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsTask();
#endif


        public class GetDailyTransactionHistoryTask : Gs2WebSocketSessionTask<Request.GetDailyTransactionHistoryRequest, Result.GetDailyTransactionHistoryResult>
        {
            public GetDailyTransactionHistoryTask(IGs2Session session, Request.GetDailyTransactionHistoryRequest request) : base(session, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(Request.GetDailyTransactionHistoryRequest request)
            {
                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);

                jsonWriter.WriteObjectStart();

                if (request.NamespaceName != null)
                {
                    jsonWriter.WritePropertyName("namespaceName");
                    jsonWriter.Write(request.NamespaceName.ToString());
                }
                if (request.Year != null)
                {
                    jsonWriter.WritePropertyName("year");
                    jsonWriter.Write(request.Year.ToString());
                }
                if (request.Month != null)
                {
                    jsonWriter.WritePropertyName("month");
                    jsonWriter.Write(request.Month.ToString());
                }
                if (request.Day != null)
                {
                    jsonWriter.WritePropertyName("day");
                    jsonWriter.Write(request.Day.ToString());
                }
                if (request.Currency != null)
                {
                    jsonWriter.WritePropertyName("currency");
                    jsonWriter.Write(request.Currency.ToString());
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
                    "money2",
                    "dailyTransactionHistory",
                    "getDailyTransactionHistory",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
        public IEnumerator GetDailyTransactionHistory(
                Request.GetDailyTransactionHistoryRequest request,
                UnityAction<AsyncResult<Result.GetDailyTransactionHistoryResult>> callback
        ) =>
            new GetDailyTransactionHistoryTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToCoroutine(callback);

        public IFuture<Result.GetDailyTransactionHistoryResult> GetDailyTransactionHistoryFuture(
                Request.GetDailyTransactionHistoryRequest request
        ) =>
            new GetDailyTransactionHistoryTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
        public UniTask<Result.GetDailyTransactionHistoryResult> GetDailyTransactionHistoryAsync(
            Request.GetDailyTransactionHistoryRequest request
        ) =>
            new GetDailyTransactionHistoryTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsUniTask<Result.GetDailyTransactionHistoryResult>();
    #else
        public GetDailyTransactionHistoryTask GetDailyTransactionHistoryAsync(
                Request.GetDailyTransactionHistoryRequest request
        )
        {
            return new GetDailyTransactionHistoryTask(
                Gs2WebSocketSession,
                request
            );
        }
    #endif
#else
        public Task<Result.GetDailyTransactionHistoryResult> GetDailyTransactionHistoryAsync(
            Request.GetDailyTransactionHistoryRequest request
        ) =>
            new GetDailyTransactionHistoryTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsTask();
#endif


        public class GetUnusedBalanceTask : Gs2WebSocketSessionTask<Request.GetUnusedBalanceRequest, Result.GetUnusedBalanceResult>
        {
            public GetUnusedBalanceTask(IGs2Session session, Request.GetUnusedBalanceRequest request) : base(session, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(Request.GetUnusedBalanceRequest request)
            {
                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);

                jsonWriter.WriteObjectStart();

                if (request.NamespaceName != null)
                {
                    jsonWriter.WritePropertyName("namespaceName");
                    jsonWriter.Write(request.NamespaceName.ToString());
                }
                if (request.Currency != null)
                {
                    jsonWriter.WritePropertyName("currency");
                    jsonWriter.Write(request.Currency.ToString());
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
                    "money2",
                    "unusedBalance",
                    "getUnusedBalance",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
        public IEnumerator GetUnusedBalance(
                Request.GetUnusedBalanceRequest request,
                UnityAction<AsyncResult<Result.GetUnusedBalanceResult>> callback
        ) =>
            new GetUnusedBalanceTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToCoroutine(callback);

        public IFuture<Result.GetUnusedBalanceResult> GetUnusedBalanceFuture(
                Request.GetUnusedBalanceRequest request
        ) =>
            new GetUnusedBalanceTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
        public UniTask<Result.GetUnusedBalanceResult> GetUnusedBalanceAsync(
            Request.GetUnusedBalanceRequest request
        ) =>
            new GetUnusedBalanceTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsUniTask<Result.GetUnusedBalanceResult>();
    #else
        public GetUnusedBalanceTask GetUnusedBalanceAsync(
                Request.GetUnusedBalanceRequest request
        )
        {
            return new GetUnusedBalanceTask(
                Gs2WebSocketSession,
                request
            );
        }
    #endif
#else
        public Task<Result.GetUnusedBalanceResult> GetUnusedBalanceAsync(
            Request.GetUnusedBalanceRequest request
        ) =>
            new GetUnusedBalanceTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsTask();
#endif
	}
}