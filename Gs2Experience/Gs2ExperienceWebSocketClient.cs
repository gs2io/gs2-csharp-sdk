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

namespace Gs2.Gs2Experience
{
	public class Gs2ExperienceWebSocketClient : AbstractGs2Client
	{

		public static string Endpoint = "experience";

        protected Gs2WebSocketSession Gs2WebSocketSession => (Gs2WebSocketSession) Gs2Session;

		public Gs2ExperienceWebSocketClient(Gs2WebSocketSession Gs2WebSocketSession) : base(Gs2WebSocketSession)
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
                if (request.RankCapScriptId != null)
                {
                    jsonWriter.WritePropertyName("rankCapScriptId");
                    jsonWriter.Write(request.RankCapScriptId.ToString());
                }
                if (request.ChangeExperienceScript != null)
                {
                    jsonWriter.WritePropertyName("changeExperienceScript");
                    request.ChangeExperienceScript.WriteJson(jsonWriter);
                }
                if (request.ChangeRankScript != null)
                {
                    jsonWriter.WritePropertyName("changeRankScript");
                    request.ChangeRankScript.WriteJson(jsonWriter);
                }
                if (request.ChangeRankCapScript != null)
                {
                    jsonWriter.WritePropertyName("changeRankCapScript");
                    request.ChangeRankCapScript.WriteJson(jsonWriter);
                }
                if (request.OverflowExperienceScript != null)
                {
                    jsonWriter.WritePropertyName("overflowExperienceScript");
                    jsonWriter.Write(request.OverflowExperienceScript.ToString());
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
                    "experience",
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
                    "experience",
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
                    "experience",
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
                if (request.RankCapScriptId != null)
                {
                    jsonWriter.WritePropertyName("rankCapScriptId");
                    jsonWriter.Write(request.RankCapScriptId.ToString());
                }
                if (request.ChangeExperienceScript != null)
                {
                    jsonWriter.WritePropertyName("changeExperienceScript");
                    request.ChangeExperienceScript.WriteJson(jsonWriter);
                }
                if (request.ChangeRankScript != null)
                {
                    jsonWriter.WritePropertyName("changeRankScript");
                    request.ChangeRankScript.WriteJson(jsonWriter);
                }
                if (request.ChangeRankCapScript != null)
                {
                    jsonWriter.WritePropertyName("changeRankCapScript");
                    request.ChangeRankCapScript.WriteJson(jsonWriter);
                }
                if (request.OverflowExperienceScript != null)
                {
                    jsonWriter.WritePropertyName("overflowExperienceScript");
                    jsonWriter.Write(request.OverflowExperienceScript.ToString());
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
                    "experience",
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
                    "experience",
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
                    "experience",
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
                    "experience",
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
                    "experience",
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
                    "experience",
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
                    "experience",
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
                    "experience",
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
                    "experience",
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
                    "experience",
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


        public class PreUpdateCurrentExperienceMasterTask : Gs2WebSocketSessionTask<Request.PreUpdateCurrentExperienceMasterRequest, Result.PreUpdateCurrentExperienceMasterResult>
        {
            public PreUpdateCurrentExperienceMasterTask(IGs2Session session, Request.PreUpdateCurrentExperienceMasterRequest request) : base(session, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(Request.PreUpdateCurrentExperienceMasterRequest request)
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
                    "experience",
                    "currentExperienceMaster",
                    "preUpdateCurrentExperienceMaster",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
        public IEnumerator PreUpdateCurrentExperienceMaster(
                Request.PreUpdateCurrentExperienceMasterRequest request,
                UnityAction<AsyncResult<Result.PreUpdateCurrentExperienceMasterResult>> callback
        ) =>
            new PreUpdateCurrentExperienceMasterTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToCoroutine(callback);

        public IFuture<Result.PreUpdateCurrentExperienceMasterResult> PreUpdateCurrentExperienceMasterFuture(
                Request.PreUpdateCurrentExperienceMasterRequest request
        ) =>
            new PreUpdateCurrentExperienceMasterTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
        public UniTask<Result.PreUpdateCurrentExperienceMasterResult> PreUpdateCurrentExperienceMasterAsync(
            Request.PreUpdateCurrentExperienceMasterRequest request
        ) =>
            new PreUpdateCurrentExperienceMasterTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsUniTask<Result.PreUpdateCurrentExperienceMasterResult>();
    #else
        public PreUpdateCurrentExperienceMasterTask PreUpdateCurrentExperienceMasterAsync(
                Request.PreUpdateCurrentExperienceMasterRequest request
        )
        {
            return new PreUpdateCurrentExperienceMasterTask(
                Gs2WebSocketSession,
                request
            );
        }
    #endif
#else
        public Task<Result.PreUpdateCurrentExperienceMasterResult> PreUpdateCurrentExperienceMasterAsync(
            Request.PreUpdateCurrentExperienceMasterRequest request
        ) =>
            new PreUpdateCurrentExperienceMasterTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsTask();
#endif


        public class GetStatusTask : Gs2WebSocketSessionTask<Request.GetStatusRequest, Result.GetStatusResult>
        {
            public GetStatusTask(IGs2Session session, Request.GetStatusRequest request) : base(session, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(Request.GetStatusRequest request)
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
                if (request.ExperienceName != null)
                {
                    jsonWriter.WritePropertyName("experienceName");
                    jsonWriter.Write(request.ExperienceName.ToString());
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
                    "experience",
                    "status",
                    "getStatus",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
        public IEnumerator GetStatus(
                Request.GetStatusRequest request,
                UnityAction<AsyncResult<Result.GetStatusResult>> callback
        ) =>
            new GetStatusTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToCoroutine(callback);

        public IFuture<Result.GetStatusResult> GetStatusFuture(
                Request.GetStatusRequest request
        ) =>
            new GetStatusTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
        public UniTask<Result.GetStatusResult> GetStatusAsync(
            Request.GetStatusRequest request
        ) =>
            new GetStatusTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsUniTask<Result.GetStatusResult>();
    #else
        public GetStatusTask GetStatusAsync(
                Request.GetStatusRequest request
        )
        {
            return new GetStatusTask(
                Gs2WebSocketSession,
                request
            );
        }
    #endif
#else
        public Task<Result.GetStatusResult> GetStatusAsync(
            Request.GetStatusRequest request
        ) =>
            new GetStatusTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsTask();
#endif


        public class GetStatusByUserIdTask : Gs2WebSocketSessionTask<Request.GetStatusByUserIdRequest, Result.GetStatusByUserIdResult>
        {
            public GetStatusByUserIdTask(IGs2Session session, Request.GetStatusByUserIdRequest request) : base(session, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(Request.GetStatusByUserIdRequest request)
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
                if (request.ExperienceName != null)
                {
                    jsonWriter.WritePropertyName("experienceName");
                    jsonWriter.Write(request.ExperienceName.ToString());
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
                    "experience",
                    "status",
                    "getStatusByUserId",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
        public IEnumerator GetStatusByUserId(
                Request.GetStatusByUserIdRequest request,
                UnityAction<AsyncResult<Result.GetStatusByUserIdResult>> callback
        ) =>
            new GetStatusByUserIdTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToCoroutine(callback);

        public IFuture<Result.GetStatusByUserIdResult> GetStatusByUserIdFuture(
                Request.GetStatusByUserIdRequest request
        ) =>
            new GetStatusByUserIdTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
        public UniTask<Result.GetStatusByUserIdResult> GetStatusByUserIdAsync(
            Request.GetStatusByUserIdRequest request
        ) =>
            new GetStatusByUserIdTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsUniTask<Result.GetStatusByUserIdResult>();
    #else
        public GetStatusByUserIdTask GetStatusByUserIdAsync(
                Request.GetStatusByUserIdRequest request
        )
        {
            return new GetStatusByUserIdTask(
                Gs2WebSocketSession,
                request
            );
        }
    #endif
#else
        public Task<Result.GetStatusByUserIdResult> GetStatusByUserIdAsync(
            Request.GetStatusByUserIdRequest request
        ) =>
            new GetStatusByUserIdTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsTask();
#endif


        public class AddExperienceByUserIdTask : Gs2WebSocketSessionTask<Request.AddExperienceByUserIdRequest, Result.AddExperienceByUserIdResult>
        {
            public AddExperienceByUserIdTask(IGs2Session session, Request.AddExperienceByUserIdRequest request) : base(session, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(Request.AddExperienceByUserIdRequest request)
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
                if (request.ExperienceName != null)
                {
                    jsonWriter.WritePropertyName("experienceName");
                    jsonWriter.Write(request.ExperienceName.ToString());
                }
                if (request.PropertyId != null)
                {
                    jsonWriter.WritePropertyName("propertyId");
                    jsonWriter.Write(request.PropertyId.ToString());
                }
                if (request.ExperienceValue != null)
                {
                    jsonWriter.WritePropertyName("experienceValue");
                    jsonWriter.Write(request.ExperienceValue.ToString());
                }
                if (request.TruncateExperienceWhenRankUp != null)
                {
                    jsonWriter.WritePropertyName("truncateExperienceWhenRankUp");
                    jsonWriter.Write(request.TruncateExperienceWhenRankUp.ToString());
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
                    "experience",
                    "status",
                    "addExperienceByUserId",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
        public IEnumerator AddExperienceByUserId(
                Request.AddExperienceByUserIdRequest request,
                UnityAction<AsyncResult<Result.AddExperienceByUserIdResult>> callback
        ) =>
            new AddExperienceByUserIdTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToCoroutine(callback);

        public IFuture<Result.AddExperienceByUserIdResult> AddExperienceByUserIdFuture(
                Request.AddExperienceByUserIdRequest request
        ) =>
            new AddExperienceByUserIdTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
        public UniTask<Result.AddExperienceByUserIdResult> AddExperienceByUserIdAsync(
            Request.AddExperienceByUserIdRequest request
        ) =>
            new AddExperienceByUserIdTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsUniTask<Result.AddExperienceByUserIdResult>();
    #else
        public AddExperienceByUserIdTask AddExperienceByUserIdAsync(
                Request.AddExperienceByUserIdRequest request
        )
        {
            return new AddExperienceByUserIdTask(
                Gs2WebSocketSession,
                request
            );
        }
    #endif
#else
        public Task<Result.AddExperienceByUserIdResult> AddExperienceByUserIdAsync(
            Request.AddExperienceByUserIdRequest request
        ) =>
            new AddExperienceByUserIdTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsTask();
#endif


        public class SubExperienceTask : Gs2WebSocketSessionTask<Request.SubExperienceRequest, Result.SubExperienceResult>
        {
            public SubExperienceTask(IGs2Session session, Request.SubExperienceRequest request) : base(session, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(Request.SubExperienceRequest request)
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
                if (request.ExperienceName != null)
                {
                    jsonWriter.WritePropertyName("experienceName");
                    jsonWriter.Write(request.ExperienceName.ToString());
                }
                if (request.PropertyId != null)
                {
                    jsonWriter.WritePropertyName("propertyId");
                    jsonWriter.Write(request.PropertyId.ToString());
                }
                if (request.ExperienceValue != null)
                {
                    jsonWriter.WritePropertyName("experienceValue");
                    jsonWriter.Write(request.ExperienceValue.ToString());
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
                    "experience",
                    "status",
                    "subExperience",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
        public IEnumerator SubExperience(
                Request.SubExperienceRequest request,
                UnityAction<AsyncResult<Result.SubExperienceResult>> callback
        ) =>
            new SubExperienceTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToCoroutine(callback);

        public IFuture<Result.SubExperienceResult> SubExperienceFuture(
                Request.SubExperienceRequest request
        ) =>
            new SubExperienceTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
        public UniTask<Result.SubExperienceResult> SubExperienceAsync(
            Request.SubExperienceRequest request
        ) =>
            new SubExperienceTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsUniTask<Result.SubExperienceResult>();
    #else
        public SubExperienceTask SubExperienceAsync(
                Request.SubExperienceRequest request
        )
        {
            return new SubExperienceTask(
                Gs2WebSocketSession,
                request
            );
        }
    #endif
#else
        public Task<Result.SubExperienceResult> SubExperienceAsync(
            Request.SubExperienceRequest request
        ) =>
            new SubExperienceTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsTask();
#endif


        public class SubExperienceByUserIdTask : Gs2WebSocketSessionTask<Request.SubExperienceByUserIdRequest, Result.SubExperienceByUserIdResult>
        {
            public SubExperienceByUserIdTask(IGs2Session session, Request.SubExperienceByUserIdRequest request) : base(session, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(Request.SubExperienceByUserIdRequest request)
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
                if (request.ExperienceName != null)
                {
                    jsonWriter.WritePropertyName("experienceName");
                    jsonWriter.Write(request.ExperienceName.ToString());
                }
                if (request.PropertyId != null)
                {
                    jsonWriter.WritePropertyName("propertyId");
                    jsonWriter.Write(request.PropertyId.ToString());
                }
                if (request.ExperienceValue != null)
                {
                    jsonWriter.WritePropertyName("experienceValue");
                    jsonWriter.Write(request.ExperienceValue.ToString());
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
                    "experience",
                    "status",
                    "subExperienceByUserId",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
        public IEnumerator SubExperienceByUserId(
                Request.SubExperienceByUserIdRequest request,
                UnityAction<AsyncResult<Result.SubExperienceByUserIdResult>> callback
        ) =>
            new SubExperienceByUserIdTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToCoroutine(callback);

        public IFuture<Result.SubExperienceByUserIdResult> SubExperienceByUserIdFuture(
                Request.SubExperienceByUserIdRequest request
        ) =>
            new SubExperienceByUserIdTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
        public UniTask<Result.SubExperienceByUserIdResult> SubExperienceByUserIdAsync(
            Request.SubExperienceByUserIdRequest request
        ) =>
            new SubExperienceByUserIdTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsUniTask<Result.SubExperienceByUserIdResult>();
    #else
        public SubExperienceByUserIdTask SubExperienceByUserIdAsync(
                Request.SubExperienceByUserIdRequest request
        )
        {
            return new SubExperienceByUserIdTask(
                Gs2WebSocketSession,
                request
            );
        }
    #endif
#else
        public Task<Result.SubExperienceByUserIdResult> SubExperienceByUserIdAsync(
            Request.SubExperienceByUserIdRequest request
        ) =>
            new SubExperienceByUserIdTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsTask();
#endif


        public class SetExperienceByUserIdTask : Gs2WebSocketSessionTask<Request.SetExperienceByUserIdRequest, Result.SetExperienceByUserIdResult>
        {
            public SetExperienceByUserIdTask(IGs2Session session, Request.SetExperienceByUserIdRequest request) : base(session, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(Request.SetExperienceByUserIdRequest request)
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
                if (request.ExperienceName != null)
                {
                    jsonWriter.WritePropertyName("experienceName");
                    jsonWriter.Write(request.ExperienceName.ToString());
                }
                if (request.PropertyId != null)
                {
                    jsonWriter.WritePropertyName("propertyId");
                    jsonWriter.Write(request.PropertyId.ToString());
                }
                if (request.ExperienceValue != null)
                {
                    jsonWriter.WritePropertyName("experienceValue");
                    jsonWriter.Write(request.ExperienceValue.ToString());
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
                    "experience",
                    "status",
                    "setExperienceByUserId",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
        public IEnumerator SetExperienceByUserId(
                Request.SetExperienceByUserIdRequest request,
                UnityAction<AsyncResult<Result.SetExperienceByUserIdResult>> callback
        ) =>
            new SetExperienceByUserIdTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToCoroutine(callback);

        public IFuture<Result.SetExperienceByUserIdResult> SetExperienceByUserIdFuture(
                Request.SetExperienceByUserIdRequest request
        ) =>
            new SetExperienceByUserIdTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
        public UniTask<Result.SetExperienceByUserIdResult> SetExperienceByUserIdAsync(
            Request.SetExperienceByUserIdRequest request
        ) =>
            new SetExperienceByUserIdTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsUniTask<Result.SetExperienceByUserIdResult>();
    #else
        public SetExperienceByUserIdTask SetExperienceByUserIdAsync(
                Request.SetExperienceByUserIdRequest request
        )
        {
            return new SetExperienceByUserIdTask(
                Gs2WebSocketSession,
                request
            );
        }
    #endif
#else
        public Task<Result.SetExperienceByUserIdResult> SetExperienceByUserIdAsync(
            Request.SetExperienceByUserIdRequest request
        ) =>
            new SetExperienceByUserIdTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsTask();
#endif


        public class AddRankCapByUserIdTask : Gs2WebSocketSessionTask<Request.AddRankCapByUserIdRequest, Result.AddRankCapByUserIdResult>
        {
            public AddRankCapByUserIdTask(IGs2Session session, Request.AddRankCapByUserIdRequest request) : base(session, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(Request.AddRankCapByUserIdRequest request)
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
                if (request.ExperienceName != null)
                {
                    jsonWriter.WritePropertyName("experienceName");
                    jsonWriter.Write(request.ExperienceName.ToString());
                }
                if (request.PropertyId != null)
                {
                    jsonWriter.WritePropertyName("propertyId");
                    jsonWriter.Write(request.PropertyId.ToString());
                }
                if (request.RankCapValue != null)
                {
                    jsonWriter.WritePropertyName("rankCapValue");
                    jsonWriter.Write(request.RankCapValue.ToString());
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
                    "experience",
                    "status",
                    "addRankCapByUserId",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
        public IEnumerator AddRankCapByUserId(
                Request.AddRankCapByUserIdRequest request,
                UnityAction<AsyncResult<Result.AddRankCapByUserIdResult>> callback
        ) =>
            new AddRankCapByUserIdTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToCoroutine(callback);

        public IFuture<Result.AddRankCapByUserIdResult> AddRankCapByUserIdFuture(
                Request.AddRankCapByUserIdRequest request
        ) =>
            new AddRankCapByUserIdTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
        public UniTask<Result.AddRankCapByUserIdResult> AddRankCapByUserIdAsync(
            Request.AddRankCapByUserIdRequest request
        ) =>
            new AddRankCapByUserIdTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsUniTask<Result.AddRankCapByUserIdResult>();
    #else
        public AddRankCapByUserIdTask AddRankCapByUserIdAsync(
                Request.AddRankCapByUserIdRequest request
        )
        {
            return new AddRankCapByUserIdTask(
                Gs2WebSocketSession,
                request
            );
        }
    #endif
#else
        public Task<Result.AddRankCapByUserIdResult> AddRankCapByUserIdAsync(
            Request.AddRankCapByUserIdRequest request
        ) =>
            new AddRankCapByUserIdTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsTask();
#endif


        public class SubRankCapTask : Gs2WebSocketSessionTask<Request.SubRankCapRequest, Result.SubRankCapResult>
        {
            public SubRankCapTask(IGs2Session session, Request.SubRankCapRequest request) : base(session, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(Request.SubRankCapRequest request)
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
                if (request.ExperienceName != null)
                {
                    jsonWriter.WritePropertyName("experienceName");
                    jsonWriter.Write(request.ExperienceName.ToString());
                }
                if (request.PropertyId != null)
                {
                    jsonWriter.WritePropertyName("propertyId");
                    jsonWriter.Write(request.PropertyId.ToString());
                }
                if (request.RankCapValue != null)
                {
                    jsonWriter.WritePropertyName("rankCapValue");
                    jsonWriter.Write(request.RankCapValue.ToString());
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
                    "experience",
                    "status",
                    "subRankCap",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
        public IEnumerator SubRankCap(
                Request.SubRankCapRequest request,
                UnityAction<AsyncResult<Result.SubRankCapResult>> callback
        ) =>
            new SubRankCapTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToCoroutine(callback);

        public IFuture<Result.SubRankCapResult> SubRankCapFuture(
                Request.SubRankCapRequest request
        ) =>
            new SubRankCapTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
        public UniTask<Result.SubRankCapResult> SubRankCapAsync(
            Request.SubRankCapRequest request
        ) =>
            new SubRankCapTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsUniTask<Result.SubRankCapResult>();
    #else
        public SubRankCapTask SubRankCapAsync(
                Request.SubRankCapRequest request
        )
        {
            return new SubRankCapTask(
                Gs2WebSocketSession,
                request
            );
        }
    #endif
#else
        public Task<Result.SubRankCapResult> SubRankCapAsync(
            Request.SubRankCapRequest request
        ) =>
            new SubRankCapTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsTask();
#endif


        public class SubRankCapByUserIdTask : Gs2WebSocketSessionTask<Request.SubRankCapByUserIdRequest, Result.SubRankCapByUserIdResult>
        {
            public SubRankCapByUserIdTask(IGs2Session session, Request.SubRankCapByUserIdRequest request) : base(session, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(Request.SubRankCapByUserIdRequest request)
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
                if (request.ExperienceName != null)
                {
                    jsonWriter.WritePropertyName("experienceName");
                    jsonWriter.Write(request.ExperienceName.ToString());
                }
                if (request.PropertyId != null)
                {
                    jsonWriter.WritePropertyName("propertyId");
                    jsonWriter.Write(request.PropertyId.ToString());
                }
                if (request.RankCapValue != null)
                {
                    jsonWriter.WritePropertyName("rankCapValue");
                    jsonWriter.Write(request.RankCapValue.ToString());
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
                    "experience",
                    "status",
                    "subRankCapByUserId",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
        public IEnumerator SubRankCapByUserId(
                Request.SubRankCapByUserIdRequest request,
                UnityAction<AsyncResult<Result.SubRankCapByUserIdResult>> callback
        ) =>
            new SubRankCapByUserIdTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToCoroutine(callback);

        public IFuture<Result.SubRankCapByUserIdResult> SubRankCapByUserIdFuture(
                Request.SubRankCapByUserIdRequest request
        ) =>
            new SubRankCapByUserIdTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
        public UniTask<Result.SubRankCapByUserIdResult> SubRankCapByUserIdAsync(
            Request.SubRankCapByUserIdRequest request
        ) =>
            new SubRankCapByUserIdTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsUniTask<Result.SubRankCapByUserIdResult>();
    #else
        public SubRankCapByUserIdTask SubRankCapByUserIdAsync(
                Request.SubRankCapByUserIdRequest request
        )
        {
            return new SubRankCapByUserIdTask(
                Gs2WebSocketSession,
                request
            );
        }
    #endif
#else
        public Task<Result.SubRankCapByUserIdResult> SubRankCapByUserIdAsync(
            Request.SubRankCapByUserIdRequest request
        ) =>
            new SubRankCapByUserIdTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsTask();
#endif


        public class SetRankCapByUserIdTask : Gs2WebSocketSessionTask<Request.SetRankCapByUserIdRequest, Result.SetRankCapByUserIdResult>
        {
            public SetRankCapByUserIdTask(IGs2Session session, Request.SetRankCapByUserIdRequest request) : base(session, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(Request.SetRankCapByUserIdRequest request)
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
                if (request.ExperienceName != null)
                {
                    jsonWriter.WritePropertyName("experienceName");
                    jsonWriter.Write(request.ExperienceName.ToString());
                }
                if (request.PropertyId != null)
                {
                    jsonWriter.WritePropertyName("propertyId");
                    jsonWriter.Write(request.PropertyId.ToString());
                }
                if (request.RankCapValue != null)
                {
                    jsonWriter.WritePropertyName("rankCapValue");
                    jsonWriter.Write(request.RankCapValue.ToString());
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
                    "experience",
                    "status",
                    "setRankCapByUserId",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
        public IEnumerator SetRankCapByUserId(
                Request.SetRankCapByUserIdRequest request,
                UnityAction<AsyncResult<Result.SetRankCapByUserIdResult>> callback
        ) =>
            new SetRankCapByUserIdTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToCoroutine(callback);

        public IFuture<Result.SetRankCapByUserIdResult> SetRankCapByUserIdFuture(
                Request.SetRankCapByUserIdRequest request
        ) =>
            new SetRankCapByUserIdTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
        public UniTask<Result.SetRankCapByUserIdResult> SetRankCapByUserIdAsync(
            Request.SetRankCapByUserIdRequest request
        ) =>
            new SetRankCapByUserIdTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsUniTask<Result.SetRankCapByUserIdResult>();
    #else
        public SetRankCapByUserIdTask SetRankCapByUserIdAsync(
                Request.SetRankCapByUserIdRequest request
        )
        {
            return new SetRankCapByUserIdTask(
                Gs2WebSocketSession,
                request
            );
        }
    #endif
#else
        public Task<Result.SetRankCapByUserIdResult> SetRankCapByUserIdAsync(
            Request.SetRankCapByUserIdRequest request
        ) =>
            new SetRankCapByUserIdTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsTask();
#endif


        public class DeleteStatusByUserIdTask : Gs2WebSocketSessionTask<Request.DeleteStatusByUserIdRequest, Result.DeleteStatusByUserIdResult>
        {
            public DeleteStatusByUserIdTask(IGs2Session session, Request.DeleteStatusByUserIdRequest request) : base(session, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(Request.DeleteStatusByUserIdRequest request)
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
                if (request.ExperienceName != null)
                {
                    jsonWriter.WritePropertyName("experienceName");
                    jsonWriter.Write(request.ExperienceName.ToString());
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
                    "experience",
                    "status",
                    "deleteStatusByUserId",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
        public IEnumerator DeleteStatusByUserId(
                Request.DeleteStatusByUserIdRequest request,
                UnityAction<AsyncResult<Result.DeleteStatusByUserIdResult>> callback
        ) =>
            new DeleteStatusByUserIdTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToCoroutine(callback);

        public IFuture<Result.DeleteStatusByUserIdResult> DeleteStatusByUserIdFuture(
                Request.DeleteStatusByUserIdRequest request
        ) =>
            new DeleteStatusByUserIdTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
        public UniTask<Result.DeleteStatusByUserIdResult> DeleteStatusByUserIdAsync(
            Request.DeleteStatusByUserIdRequest request
        ) =>
            new DeleteStatusByUserIdTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsUniTask<Result.DeleteStatusByUserIdResult>();
    #else
        public DeleteStatusByUserIdTask DeleteStatusByUserIdAsync(
                Request.DeleteStatusByUserIdRequest request
        )
        {
            return new DeleteStatusByUserIdTask(
                Gs2WebSocketSession,
                request
            );
        }
    #endif
#else
        public Task<Result.DeleteStatusByUserIdResult> DeleteStatusByUserIdAsync(
            Request.DeleteStatusByUserIdRequest request
        ) =>
            new DeleteStatusByUserIdTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsTask();
#endif


        public class VerifyRankTask : Gs2WebSocketSessionTask<Request.VerifyRankRequest, Result.VerifyRankResult>
        {
            public VerifyRankTask(IGs2Session session, Request.VerifyRankRequest request) : base(session, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(Request.VerifyRankRequest request)
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
                if (request.ExperienceName != null)
                {
                    jsonWriter.WritePropertyName("experienceName");
                    jsonWriter.Write(request.ExperienceName.ToString());
                }
                if (request.VerifyType != null)
                {
                    jsonWriter.WritePropertyName("verifyType");
                    jsonWriter.Write(request.VerifyType.ToString());
                }
                if (request.PropertyId != null)
                {
                    jsonWriter.WritePropertyName("propertyId");
                    jsonWriter.Write(request.PropertyId.ToString());
                }
                if (request.RankValue != null)
                {
                    jsonWriter.WritePropertyName("rankValue");
                    jsonWriter.Write(request.RankValue.ToString());
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
                    "experience",
                    "status",
                    "verifyRank",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
        public IEnumerator VerifyRank(
                Request.VerifyRankRequest request,
                UnityAction<AsyncResult<Result.VerifyRankResult>> callback
        ) =>
            new VerifyRankTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToCoroutine(callback);

        public IFuture<Result.VerifyRankResult> VerifyRankFuture(
                Request.VerifyRankRequest request
        ) =>
            new VerifyRankTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
        public UniTask<Result.VerifyRankResult> VerifyRankAsync(
            Request.VerifyRankRequest request
        ) =>
            new VerifyRankTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsUniTask<Result.VerifyRankResult>();
    #else
        public VerifyRankTask VerifyRankAsync(
                Request.VerifyRankRequest request
        )
        {
            return new VerifyRankTask(
                Gs2WebSocketSession,
                request
            );
        }
    #endif
#else
        public Task<Result.VerifyRankResult> VerifyRankAsync(
            Request.VerifyRankRequest request
        ) =>
            new VerifyRankTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsTask();
#endif


        public class VerifyRankByUserIdTask : Gs2WebSocketSessionTask<Request.VerifyRankByUserIdRequest, Result.VerifyRankByUserIdResult>
        {
            public VerifyRankByUserIdTask(IGs2Session session, Request.VerifyRankByUserIdRequest request) : base(session, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(Request.VerifyRankByUserIdRequest request)
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
                if (request.ExperienceName != null)
                {
                    jsonWriter.WritePropertyName("experienceName");
                    jsonWriter.Write(request.ExperienceName.ToString());
                }
                if (request.VerifyType != null)
                {
                    jsonWriter.WritePropertyName("verifyType");
                    jsonWriter.Write(request.VerifyType.ToString());
                }
                if (request.PropertyId != null)
                {
                    jsonWriter.WritePropertyName("propertyId");
                    jsonWriter.Write(request.PropertyId.ToString());
                }
                if (request.RankValue != null)
                {
                    jsonWriter.WritePropertyName("rankValue");
                    jsonWriter.Write(request.RankValue.ToString());
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
                    "experience",
                    "status",
                    "verifyRankByUserId",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
        public IEnumerator VerifyRankByUserId(
                Request.VerifyRankByUserIdRequest request,
                UnityAction<AsyncResult<Result.VerifyRankByUserIdResult>> callback
        ) =>
            new VerifyRankByUserIdTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToCoroutine(callback);

        public IFuture<Result.VerifyRankByUserIdResult> VerifyRankByUserIdFuture(
                Request.VerifyRankByUserIdRequest request
        ) =>
            new VerifyRankByUserIdTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
        public UniTask<Result.VerifyRankByUserIdResult> VerifyRankByUserIdAsync(
            Request.VerifyRankByUserIdRequest request
        ) =>
            new VerifyRankByUserIdTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsUniTask<Result.VerifyRankByUserIdResult>();
    #else
        public VerifyRankByUserIdTask VerifyRankByUserIdAsync(
                Request.VerifyRankByUserIdRequest request
        )
        {
            return new VerifyRankByUserIdTask(
                Gs2WebSocketSession,
                request
            );
        }
    #endif
#else
        public Task<Result.VerifyRankByUserIdResult> VerifyRankByUserIdAsync(
            Request.VerifyRankByUserIdRequest request
        ) =>
            new VerifyRankByUserIdTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsTask();
#endif


        public class VerifyRankCapTask : Gs2WebSocketSessionTask<Request.VerifyRankCapRequest, Result.VerifyRankCapResult>
        {
            public VerifyRankCapTask(IGs2Session session, Request.VerifyRankCapRequest request) : base(session, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(Request.VerifyRankCapRequest request)
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
                if (request.ExperienceName != null)
                {
                    jsonWriter.WritePropertyName("experienceName");
                    jsonWriter.Write(request.ExperienceName.ToString());
                }
                if (request.VerifyType != null)
                {
                    jsonWriter.WritePropertyName("verifyType");
                    jsonWriter.Write(request.VerifyType.ToString());
                }
                if (request.PropertyId != null)
                {
                    jsonWriter.WritePropertyName("propertyId");
                    jsonWriter.Write(request.PropertyId.ToString());
                }
                if (request.RankCapValue != null)
                {
                    jsonWriter.WritePropertyName("rankCapValue");
                    jsonWriter.Write(request.RankCapValue.ToString());
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
                    "experience",
                    "status",
                    "verifyRankCap",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
        public IEnumerator VerifyRankCap(
                Request.VerifyRankCapRequest request,
                UnityAction<AsyncResult<Result.VerifyRankCapResult>> callback
        ) =>
            new VerifyRankCapTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToCoroutine(callback);

        public IFuture<Result.VerifyRankCapResult> VerifyRankCapFuture(
                Request.VerifyRankCapRequest request
        ) =>
            new VerifyRankCapTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
        public UniTask<Result.VerifyRankCapResult> VerifyRankCapAsync(
            Request.VerifyRankCapRequest request
        ) =>
            new VerifyRankCapTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsUniTask<Result.VerifyRankCapResult>();
    #else
        public VerifyRankCapTask VerifyRankCapAsync(
                Request.VerifyRankCapRequest request
        )
        {
            return new VerifyRankCapTask(
                Gs2WebSocketSession,
                request
            );
        }
    #endif
#else
        public Task<Result.VerifyRankCapResult> VerifyRankCapAsync(
            Request.VerifyRankCapRequest request
        ) =>
            new VerifyRankCapTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsTask();
#endif


        public class VerifyRankCapByUserIdTask : Gs2WebSocketSessionTask<Request.VerifyRankCapByUserIdRequest, Result.VerifyRankCapByUserIdResult>
        {
            public VerifyRankCapByUserIdTask(IGs2Session session, Request.VerifyRankCapByUserIdRequest request) : base(session, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(Request.VerifyRankCapByUserIdRequest request)
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
                if (request.ExperienceName != null)
                {
                    jsonWriter.WritePropertyName("experienceName");
                    jsonWriter.Write(request.ExperienceName.ToString());
                }
                if (request.VerifyType != null)
                {
                    jsonWriter.WritePropertyName("verifyType");
                    jsonWriter.Write(request.VerifyType.ToString());
                }
                if (request.PropertyId != null)
                {
                    jsonWriter.WritePropertyName("propertyId");
                    jsonWriter.Write(request.PropertyId.ToString());
                }
                if (request.RankCapValue != null)
                {
                    jsonWriter.WritePropertyName("rankCapValue");
                    jsonWriter.Write(request.RankCapValue.ToString());
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
                    "experience",
                    "status",
                    "verifyRankCapByUserId",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
        public IEnumerator VerifyRankCapByUserId(
                Request.VerifyRankCapByUserIdRequest request,
                UnityAction<AsyncResult<Result.VerifyRankCapByUserIdResult>> callback
        ) =>
            new VerifyRankCapByUserIdTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToCoroutine(callback);

        public IFuture<Result.VerifyRankCapByUserIdResult> VerifyRankCapByUserIdFuture(
                Request.VerifyRankCapByUserIdRequest request
        ) =>
            new VerifyRankCapByUserIdTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
        public UniTask<Result.VerifyRankCapByUserIdResult> VerifyRankCapByUserIdAsync(
            Request.VerifyRankCapByUserIdRequest request
        ) =>
            new VerifyRankCapByUserIdTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsUniTask<Result.VerifyRankCapByUserIdResult>();
    #else
        public VerifyRankCapByUserIdTask VerifyRankCapByUserIdAsync(
                Request.VerifyRankCapByUserIdRequest request
        )
        {
            return new VerifyRankCapByUserIdTask(
                Gs2WebSocketSession,
                request
            );
        }
    #endif
#else
        public Task<Result.VerifyRankCapByUserIdResult> VerifyRankCapByUserIdAsync(
            Request.VerifyRankCapByUserIdRequest request
        ) =>
            new VerifyRankCapByUserIdTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsTask();
#endif


        public class AddExperienceByStampSheetTask : Gs2WebSocketSessionTask<Request.AddExperienceByStampSheetRequest, Result.AddExperienceByStampSheetResult>
        {
            public AddExperienceByStampSheetTask(IGs2Session session, Request.AddExperienceByStampSheetRequest request) : base(session, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(Request.AddExperienceByStampSheetRequest request)
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
                    "experience",
                    "status",
                    "addExperienceByStampSheet",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
        public IEnumerator AddExperienceByStampSheet(
                Request.AddExperienceByStampSheetRequest request,
                UnityAction<AsyncResult<Result.AddExperienceByStampSheetResult>> callback
        ) =>
            new AddExperienceByStampSheetTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToCoroutine(callback);

        public IFuture<Result.AddExperienceByStampSheetResult> AddExperienceByStampSheetFuture(
                Request.AddExperienceByStampSheetRequest request
        ) =>
            new AddExperienceByStampSheetTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
        public UniTask<Result.AddExperienceByStampSheetResult> AddExperienceByStampSheetAsync(
            Request.AddExperienceByStampSheetRequest request
        ) =>
            new AddExperienceByStampSheetTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsUniTask<Result.AddExperienceByStampSheetResult>();
    #else
        public AddExperienceByStampSheetTask AddExperienceByStampSheetAsync(
                Request.AddExperienceByStampSheetRequest request
        )
        {
            return new AddExperienceByStampSheetTask(
                Gs2WebSocketSession,
                request
            );
        }
    #endif
#else
        public Task<Result.AddExperienceByStampSheetResult> AddExperienceByStampSheetAsync(
            Request.AddExperienceByStampSheetRequest request
        ) =>
            new AddExperienceByStampSheetTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsTask();
#endif


        public class SetExperienceByStampSheetTask : Gs2WebSocketSessionTask<Request.SetExperienceByStampSheetRequest, Result.SetExperienceByStampSheetResult>
        {
            public SetExperienceByStampSheetTask(IGs2Session session, Request.SetExperienceByStampSheetRequest request) : base(session, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(Request.SetExperienceByStampSheetRequest request)
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
                    "experience",
                    "status",
                    "setExperienceByStampSheet",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
        public IEnumerator SetExperienceByStampSheet(
                Request.SetExperienceByStampSheetRequest request,
                UnityAction<AsyncResult<Result.SetExperienceByStampSheetResult>> callback
        ) =>
            new SetExperienceByStampSheetTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToCoroutine(callback);

        public IFuture<Result.SetExperienceByStampSheetResult> SetExperienceByStampSheetFuture(
                Request.SetExperienceByStampSheetRequest request
        ) =>
            new SetExperienceByStampSheetTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
        public UniTask<Result.SetExperienceByStampSheetResult> SetExperienceByStampSheetAsync(
            Request.SetExperienceByStampSheetRequest request
        ) =>
            new SetExperienceByStampSheetTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsUniTask<Result.SetExperienceByStampSheetResult>();
    #else
        public SetExperienceByStampSheetTask SetExperienceByStampSheetAsync(
                Request.SetExperienceByStampSheetRequest request
        )
        {
            return new SetExperienceByStampSheetTask(
                Gs2WebSocketSession,
                request
            );
        }
    #endif
#else
        public Task<Result.SetExperienceByStampSheetResult> SetExperienceByStampSheetAsync(
            Request.SetExperienceByStampSheetRequest request
        ) =>
            new SetExperienceByStampSheetTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsTask();
#endif


        public class AddRankCapByStampSheetTask : Gs2WebSocketSessionTask<Request.AddRankCapByStampSheetRequest, Result.AddRankCapByStampSheetResult>
        {
            public AddRankCapByStampSheetTask(IGs2Session session, Request.AddRankCapByStampSheetRequest request) : base(session, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(Request.AddRankCapByStampSheetRequest request)
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
                    "experience",
                    "status",
                    "addRankCapByStampSheet",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
        public IEnumerator AddRankCapByStampSheet(
                Request.AddRankCapByStampSheetRequest request,
                UnityAction<AsyncResult<Result.AddRankCapByStampSheetResult>> callback
        ) =>
            new AddRankCapByStampSheetTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToCoroutine(callback);

        public IFuture<Result.AddRankCapByStampSheetResult> AddRankCapByStampSheetFuture(
                Request.AddRankCapByStampSheetRequest request
        ) =>
            new AddRankCapByStampSheetTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
        public UniTask<Result.AddRankCapByStampSheetResult> AddRankCapByStampSheetAsync(
            Request.AddRankCapByStampSheetRequest request
        ) =>
            new AddRankCapByStampSheetTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsUniTask<Result.AddRankCapByStampSheetResult>();
    #else
        public AddRankCapByStampSheetTask AddRankCapByStampSheetAsync(
                Request.AddRankCapByStampSheetRequest request
        )
        {
            return new AddRankCapByStampSheetTask(
                Gs2WebSocketSession,
                request
            );
        }
    #endif
#else
        public Task<Result.AddRankCapByStampSheetResult> AddRankCapByStampSheetAsync(
            Request.AddRankCapByStampSheetRequest request
        ) =>
            new AddRankCapByStampSheetTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsTask();
#endif


        public class SetRankCapByStampSheetTask : Gs2WebSocketSessionTask<Request.SetRankCapByStampSheetRequest, Result.SetRankCapByStampSheetResult>
        {
            public SetRankCapByStampSheetTask(IGs2Session session, Request.SetRankCapByStampSheetRequest request) : base(session, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(Request.SetRankCapByStampSheetRequest request)
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
                    "experience",
                    "status",
                    "setRankCapByStampSheet",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
        public IEnumerator SetRankCapByStampSheet(
                Request.SetRankCapByStampSheetRequest request,
                UnityAction<AsyncResult<Result.SetRankCapByStampSheetResult>> callback
        ) =>
            new SetRankCapByStampSheetTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToCoroutine(callback);

        public IFuture<Result.SetRankCapByStampSheetResult> SetRankCapByStampSheetFuture(
                Request.SetRankCapByStampSheetRequest request
        ) =>
            new SetRankCapByStampSheetTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
        public UniTask<Result.SetRankCapByStampSheetResult> SetRankCapByStampSheetAsync(
            Request.SetRankCapByStampSheetRequest request
        ) =>
            new SetRankCapByStampSheetTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsUniTask<Result.SetRankCapByStampSheetResult>();
    #else
        public SetRankCapByStampSheetTask SetRankCapByStampSheetAsync(
                Request.SetRankCapByStampSheetRequest request
        )
        {
            return new SetRankCapByStampSheetTask(
                Gs2WebSocketSession,
                request
            );
        }
    #endif
#else
        public Task<Result.SetRankCapByStampSheetResult> SetRankCapByStampSheetAsync(
            Request.SetRankCapByStampSheetRequest request
        ) =>
            new SetRankCapByStampSheetTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsTask();
#endif
	}
}