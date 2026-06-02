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

namespace Gs2.Gs2Account
{
	public class Gs2AccountWebSocketClient : AbstractGs2Client
	{

		public static string Endpoint = "account";

        protected Gs2WebSocketSession Gs2WebSocketSession => (Gs2WebSocketSession) Gs2Session;

		public Gs2AccountWebSocketClient(Gs2WebSocketSession Gs2WebSocketSession) : base(Gs2WebSocketSession)
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
                if (request.ChangePasswordIfTakeOver != null)
                {
                    jsonWriter.WritePropertyName("changePasswordIfTakeOver");
                    jsonWriter.Write(request.ChangePasswordIfTakeOver.ToString());
                }
                if (request.DifferentUserIdForLoginAndDataRetention != null)
                {
                    jsonWriter.WritePropertyName("differentUserIdForLoginAndDataRetention");
                    jsonWriter.Write(request.DifferentUserIdForLoginAndDataRetention.ToString());
                }
                if (request.CreateAccountScript != null)
                {
                    jsonWriter.WritePropertyName("createAccountScript");
                    request.CreateAccountScript.WriteJson(jsonWriter);
                }
                if (request.AuthenticationScript != null)
                {
                    jsonWriter.WritePropertyName("authenticationScript");
                    request.AuthenticationScript.WriteJson(jsonWriter);
                }
                if (request.CreateTakeOverScript != null)
                {
                    jsonWriter.WritePropertyName("createTakeOverScript");
                    request.CreateTakeOverScript.WriteJson(jsonWriter);
                }
                if (request.DoTakeOverScript != null)
                {
                    jsonWriter.WritePropertyName("doTakeOverScript");
                    request.DoTakeOverScript.WriteJson(jsonWriter);
                }
                if (request.BanScript != null)
                {
                    jsonWriter.WritePropertyName("banScript");
                    request.BanScript.WriteJson(jsonWriter);
                }
                if (request.UnBanScript != null)
                {
                    jsonWriter.WritePropertyName("unBanScript");
                    request.UnBanScript.WriteJson(jsonWriter);
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
                    "account",
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
                    "account",
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
                    "account",
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
                if (request.ChangePasswordIfTakeOver != null)
                {
                    jsonWriter.WritePropertyName("changePasswordIfTakeOver");
                    jsonWriter.Write(request.ChangePasswordIfTakeOver.ToString());
                }
                if (request.CreateAccountScript != null)
                {
                    jsonWriter.WritePropertyName("createAccountScript");
                    request.CreateAccountScript.WriteJson(jsonWriter);
                }
                if (request.AuthenticationScript != null)
                {
                    jsonWriter.WritePropertyName("authenticationScript");
                    request.AuthenticationScript.WriteJson(jsonWriter);
                }
                if (request.CreateTakeOverScript != null)
                {
                    jsonWriter.WritePropertyName("createTakeOverScript");
                    request.CreateTakeOverScript.WriteJson(jsonWriter);
                }
                if (request.DoTakeOverScript != null)
                {
                    jsonWriter.WritePropertyName("doTakeOverScript");
                    request.DoTakeOverScript.WriteJson(jsonWriter);
                }
                if (request.BanScript != null)
                {
                    jsonWriter.WritePropertyName("banScript");
                    request.BanScript.WriteJson(jsonWriter);
                }
                if (request.UnBanScript != null)
                {
                    jsonWriter.WritePropertyName("unBanScript");
                    request.UnBanScript.WriteJson(jsonWriter);
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
                    "account",
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
                    "account",
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
                    "account",
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
                    "account",
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
                    "account",
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
                    "account",
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
                    "account",
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
                    "account",
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
                    "account",
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
                    "account",
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


        public class CreateAccountTask : Gs2WebSocketSessionTask<Request.CreateAccountRequest, Result.CreateAccountResult>
        {
            public CreateAccountTask(IGs2Session session, Request.CreateAccountRequest request) : base(session, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(Request.CreateAccountRequest request)
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
                    "account",
                    "account",
                    "createAccount",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
        public IEnumerator CreateAccount(
                Request.CreateAccountRequest request,
                UnityAction<AsyncResult<Result.CreateAccountResult>> callback
        ) =>
            new CreateAccountTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToCoroutine(callback);

        public IFuture<Result.CreateAccountResult> CreateAccountFuture(
                Request.CreateAccountRequest request
        ) =>
            new CreateAccountTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
        public UniTask<Result.CreateAccountResult> CreateAccountAsync(
            Request.CreateAccountRequest request
        ) =>
            new CreateAccountTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsUniTask<Result.CreateAccountResult>();
    #else
        public CreateAccountTask CreateAccountAsync(
                Request.CreateAccountRequest request
        )
        {
            return new CreateAccountTask(
                Gs2WebSocketSession,
                request
            );
        }
    #endif
#else
        public Task<Result.CreateAccountResult> CreateAccountAsync(
            Request.CreateAccountRequest request
        ) =>
            new CreateAccountTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsTask();
#endif


        public class UpdateTimeOffsetTask : Gs2WebSocketSessionTask<Request.UpdateTimeOffsetRequest, Result.UpdateTimeOffsetResult>
        {
            public UpdateTimeOffsetTask(IGs2Session session, Request.UpdateTimeOffsetRequest request) : base(session, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(Request.UpdateTimeOffsetRequest request)
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
                    "account",
                    "account",
                    "updateTimeOffset",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
        public IEnumerator UpdateTimeOffset(
                Request.UpdateTimeOffsetRequest request,
                UnityAction<AsyncResult<Result.UpdateTimeOffsetResult>> callback
        ) =>
            new UpdateTimeOffsetTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToCoroutine(callback);

        public IFuture<Result.UpdateTimeOffsetResult> UpdateTimeOffsetFuture(
                Request.UpdateTimeOffsetRequest request
        ) =>
            new UpdateTimeOffsetTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
        public UniTask<Result.UpdateTimeOffsetResult> UpdateTimeOffsetAsync(
            Request.UpdateTimeOffsetRequest request
        ) =>
            new UpdateTimeOffsetTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsUniTask<Result.UpdateTimeOffsetResult>();
    #else
        public UpdateTimeOffsetTask UpdateTimeOffsetAsync(
                Request.UpdateTimeOffsetRequest request
        )
        {
            return new UpdateTimeOffsetTask(
                Gs2WebSocketSession,
                request
            );
        }
    #endif
#else
        public Task<Result.UpdateTimeOffsetResult> UpdateTimeOffsetAsync(
            Request.UpdateTimeOffsetRequest request
        ) =>
            new UpdateTimeOffsetTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsTask();
#endif


        public class UpdateBannedTask : Gs2WebSocketSessionTask<Request.UpdateBannedRequest, Result.UpdateBannedResult>
        {
            public UpdateBannedTask(IGs2Session session, Request.UpdateBannedRequest request) : base(session, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(Request.UpdateBannedRequest request)
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
                if (request.Banned != null)
                {
                    jsonWriter.WritePropertyName("banned");
                    jsonWriter.Write(request.Banned.ToString());
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
                    "account",
                    "account",
                    "updateBanned",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
        public IEnumerator UpdateBanned(
                Request.UpdateBannedRequest request,
                UnityAction<AsyncResult<Result.UpdateBannedResult>> callback
        ) =>
            new UpdateBannedTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToCoroutine(callback);

        public IFuture<Result.UpdateBannedResult> UpdateBannedFuture(
                Request.UpdateBannedRequest request
        ) =>
            new UpdateBannedTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
        public UniTask<Result.UpdateBannedResult> UpdateBannedAsync(
            Request.UpdateBannedRequest request
        ) =>
            new UpdateBannedTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsUniTask<Result.UpdateBannedResult>();
    #else
        public UpdateBannedTask UpdateBannedAsync(
                Request.UpdateBannedRequest request
        )
        {
            return new UpdateBannedTask(
                Gs2WebSocketSession,
                request
            );
        }
    #endif
#else
        public Task<Result.UpdateBannedResult> UpdateBannedAsync(
            Request.UpdateBannedRequest request
        ) =>
            new UpdateBannedTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsTask();
#endif


        public class AddBanTask : Gs2WebSocketSessionTask<Request.AddBanRequest, Result.AddBanResult>
        {
            public AddBanTask(IGs2Session session, Request.AddBanRequest request) : base(session, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(Request.AddBanRequest request)
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
                if (request.BanStatus != null)
                {
                    jsonWriter.WritePropertyName("banStatus");
                    request.BanStatus.WriteJson(jsonWriter);
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
                    "account",
                    "account",
                    "addBan",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
        public IEnumerator AddBan(
                Request.AddBanRequest request,
                UnityAction<AsyncResult<Result.AddBanResult>> callback
        ) =>
            new AddBanTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToCoroutine(callback);

        public IFuture<Result.AddBanResult> AddBanFuture(
                Request.AddBanRequest request
        ) =>
            new AddBanTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
        public UniTask<Result.AddBanResult> AddBanAsync(
            Request.AddBanRequest request
        ) =>
            new AddBanTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsUniTask<Result.AddBanResult>();
    #else
        public AddBanTask AddBanAsync(
                Request.AddBanRequest request
        )
        {
            return new AddBanTask(
                Gs2WebSocketSession,
                request
            );
        }
    #endif
#else
        public Task<Result.AddBanResult> AddBanAsync(
            Request.AddBanRequest request
        ) =>
            new AddBanTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsTask();
#endif


        public class RemoveBanTask : Gs2WebSocketSessionTask<Request.RemoveBanRequest, Result.RemoveBanResult>
        {
            public RemoveBanTask(IGs2Session session, Request.RemoveBanRequest request) : base(session, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(Request.RemoveBanRequest request)
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
                if (request.BanStatusName != null)
                {
                    jsonWriter.WritePropertyName("banStatusName");
                    jsonWriter.Write(request.BanStatusName.ToString());
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
                    "account",
                    "account",
                    "removeBan",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
        public IEnumerator RemoveBan(
                Request.RemoveBanRequest request,
                UnityAction<AsyncResult<Result.RemoveBanResult>> callback
        ) =>
            new RemoveBanTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToCoroutine(callback);

        public IFuture<Result.RemoveBanResult> RemoveBanFuture(
                Request.RemoveBanRequest request
        ) =>
            new RemoveBanTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
        public UniTask<Result.RemoveBanResult> RemoveBanAsync(
            Request.RemoveBanRequest request
        ) =>
            new RemoveBanTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsUniTask<Result.RemoveBanResult>();
    #else
        public RemoveBanTask RemoveBanAsync(
                Request.RemoveBanRequest request
        )
        {
            return new RemoveBanTask(
                Gs2WebSocketSession,
                request
            );
        }
    #endif
#else
        public Task<Result.RemoveBanResult> RemoveBanAsync(
            Request.RemoveBanRequest request
        ) =>
            new RemoveBanTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsTask();
#endif


        public class GetAccountTask : Gs2WebSocketSessionTask<Request.GetAccountRequest, Result.GetAccountResult>
        {
            public GetAccountTask(IGs2Session session, Request.GetAccountRequest request) : base(session, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(Request.GetAccountRequest request)
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
                if (request.IncludeLastAuthenticatedAt != null)
                {
                    jsonWriter.WritePropertyName("includeLastAuthenticatedAt");
                    jsonWriter.Write(request.IncludeLastAuthenticatedAt.ToString());
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
                    "account",
                    "account",
                    "getAccount",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
        public IEnumerator GetAccount(
                Request.GetAccountRequest request,
                UnityAction<AsyncResult<Result.GetAccountResult>> callback
        ) =>
            new GetAccountTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToCoroutine(callback);

        public IFuture<Result.GetAccountResult> GetAccountFuture(
                Request.GetAccountRequest request
        ) =>
            new GetAccountTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
        public UniTask<Result.GetAccountResult> GetAccountAsync(
            Request.GetAccountRequest request
        ) =>
            new GetAccountTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsUniTask<Result.GetAccountResult>();
    #else
        public GetAccountTask GetAccountAsync(
                Request.GetAccountRequest request
        )
        {
            return new GetAccountTask(
                Gs2WebSocketSession,
                request
            );
        }
    #endif
#else
        public Task<Result.GetAccountResult> GetAccountAsync(
            Request.GetAccountRequest request
        ) =>
            new GetAccountTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsTask();
#endif


        public class DeleteAccountTask : Gs2WebSocketSessionTask<Request.DeleteAccountRequest, Result.DeleteAccountResult>
        {
            public DeleteAccountTask(IGs2Session session, Request.DeleteAccountRequest request) : base(session, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(Request.DeleteAccountRequest request)
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
                    "account",
                    "account",
                    "deleteAccount",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
        public IEnumerator DeleteAccount(
                Request.DeleteAccountRequest request,
                UnityAction<AsyncResult<Result.DeleteAccountResult>> callback
        ) =>
            new DeleteAccountTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToCoroutine(callback);

        public IFuture<Result.DeleteAccountResult> DeleteAccountFuture(
                Request.DeleteAccountRequest request
        ) =>
            new DeleteAccountTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
        public UniTask<Result.DeleteAccountResult> DeleteAccountAsync(
            Request.DeleteAccountRequest request
        ) =>
            new DeleteAccountTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsUniTask<Result.DeleteAccountResult>();
    #else
        public DeleteAccountTask DeleteAccountAsync(
                Request.DeleteAccountRequest request
        )
        {
            return new DeleteAccountTask(
                Gs2WebSocketSession,
                request
            );
        }
    #endif
#else
        public Task<Result.DeleteAccountResult> DeleteAccountAsync(
            Request.DeleteAccountRequest request
        ) =>
            new DeleteAccountTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsTask();
#endif


        public class CreateTakeOverTask : Gs2WebSocketSessionTask<Request.CreateTakeOverRequest, Result.CreateTakeOverResult>
        {
            public CreateTakeOverTask(IGs2Session session, Request.CreateTakeOverRequest request) : base(session, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(Request.CreateTakeOverRequest request)
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
                if (request.Type != null)
                {
                    jsonWriter.WritePropertyName("type");
                    jsonWriter.Write(request.Type.ToString());
                }
                if (request.UserIdentifier != null)
                {
                    jsonWriter.WritePropertyName("userIdentifier");
                    jsonWriter.Write(request.UserIdentifier.ToString());
                }
                if (request.Password != null)
                {
                    jsonWriter.WritePropertyName("password");
                    jsonWriter.Write(request.Password.ToString());
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
                    "account",
                    "takeOver",
                    "createTakeOver",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
        public IEnumerator CreateTakeOver(
                Request.CreateTakeOverRequest request,
                UnityAction<AsyncResult<Result.CreateTakeOverResult>> callback
        ) =>
            new CreateTakeOverTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToCoroutine(callback);

        public IFuture<Result.CreateTakeOverResult> CreateTakeOverFuture(
                Request.CreateTakeOverRequest request
        ) =>
            new CreateTakeOverTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
        public UniTask<Result.CreateTakeOverResult> CreateTakeOverAsync(
            Request.CreateTakeOverRequest request
        ) =>
            new CreateTakeOverTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsUniTask<Result.CreateTakeOverResult>();
    #else
        public CreateTakeOverTask CreateTakeOverAsync(
                Request.CreateTakeOverRequest request
        )
        {
            return new CreateTakeOverTask(
                Gs2WebSocketSession,
                request
            );
        }
    #endif
#else
        public Task<Result.CreateTakeOverResult> CreateTakeOverAsync(
            Request.CreateTakeOverRequest request
        ) =>
            new CreateTakeOverTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsTask();
#endif


        public class CreateTakeOverByUserIdTask : Gs2WebSocketSessionTask<Request.CreateTakeOverByUserIdRequest, Result.CreateTakeOverByUserIdResult>
        {
            public CreateTakeOverByUserIdTask(IGs2Session session, Request.CreateTakeOverByUserIdRequest request) : base(session, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(Request.CreateTakeOverByUserIdRequest request)
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
                if (request.Type != null)
                {
                    jsonWriter.WritePropertyName("type");
                    jsonWriter.Write(request.Type.ToString());
                }
                if (request.UserIdentifier != null)
                {
                    jsonWriter.WritePropertyName("userIdentifier");
                    jsonWriter.Write(request.UserIdentifier.ToString());
                }
                if (request.Password != null)
                {
                    jsonWriter.WritePropertyName("password");
                    jsonWriter.Write(request.Password.ToString());
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
                    "account",
                    "takeOver",
                    "createTakeOverByUserId",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
        public IEnumerator CreateTakeOverByUserId(
                Request.CreateTakeOverByUserIdRequest request,
                UnityAction<AsyncResult<Result.CreateTakeOverByUserIdResult>> callback
        ) =>
            new CreateTakeOverByUserIdTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToCoroutine(callback);

        public IFuture<Result.CreateTakeOverByUserIdResult> CreateTakeOverByUserIdFuture(
                Request.CreateTakeOverByUserIdRequest request
        ) =>
            new CreateTakeOverByUserIdTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
        public UniTask<Result.CreateTakeOverByUserIdResult> CreateTakeOverByUserIdAsync(
            Request.CreateTakeOverByUserIdRequest request
        ) =>
            new CreateTakeOverByUserIdTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsUniTask<Result.CreateTakeOverByUserIdResult>();
    #else
        public CreateTakeOverByUserIdTask CreateTakeOverByUserIdAsync(
                Request.CreateTakeOverByUserIdRequest request
        )
        {
            return new CreateTakeOverByUserIdTask(
                Gs2WebSocketSession,
                request
            );
        }
    #endif
#else
        public Task<Result.CreateTakeOverByUserIdResult> CreateTakeOverByUserIdAsync(
            Request.CreateTakeOverByUserIdRequest request
        ) =>
            new CreateTakeOverByUserIdTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsTask();
#endif


        public class CreateTakeOverOpenIdConnectTask : Gs2WebSocketSessionTask<Request.CreateTakeOverOpenIdConnectRequest, Result.CreateTakeOverOpenIdConnectResult>
        {
            public CreateTakeOverOpenIdConnectTask(IGs2Session session, Request.CreateTakeOverOpenIdConnectRequest request) : base(session, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(Request.CreateTakeOverOpenIdConnectRequest request)
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
                if (request.Type != null)
                {
                    jsonWriter.WritePropertyName("type");
                    jsonWriter.Write(request.Type.ToString());
                }
                if (request.IdToken != null)
                {
                    jsonWriter.WritePropertyName("idToken");
                    jsonWriter.Write(request.IdToken.ToString());
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
                    "account",
                    "takeOver",
                    "createTakeOverOpenIdConnect",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
        public IEnumerator CreateTakeOverOpenIdConnect(
                Request.CreateTakeOverOpenIdConnectRequest request,
                UnityAction<AsyncResult<Result.CreateTakeOverOpenIdConnectResult>> callback
        ) =>
            new CreateTakeOverOpenIdConnectTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToCoroutine(callback);

        public IFuture<Result.CreateTakeOverOpenIdConnectResult> CreateTakeOverOpenIdConnectFuture(
                Request.CreateTakeOverOpenIdConnectRequest request
        ) =>
            new CreateTakeOverOpenIdConnectTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
        public UniTask<Result.CreateTakeOverOpenIdConnectResult> CreateTakeOverOpenIdConnectAsync(
            Request.CreateTakeOverOpenIdConnectRequest request
        ) =>
            new CreateTakeOverOpenIdConnectTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsUniTask<Result.CreateTakeOverOpenIdConnectResult>();
    #else
        public CreateTakeOverOpenIdConnectTask CreateTakeOverOpenIdConnectAsync(
                Request.CreateTakeOverOpenIdConnectRequest request
        )
        {
            return new CreateTakeOverOpenIdConnectTask(
                Gs2WebSocketSession,
                request
            );
        }
    #endif
#else
        public Task<Result.CreateTakeOverOpenIdConnectResult> CreateTakeOverOpenIdConnectAsync(
            Request.CreateTakeOverOpenIdConnectRequest request
        ) =>
            new CreateTakeOverOpenIdConnectTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsTask();
#endif


        public class CreateTakeOverOpenIdConnectAndByUserIdTask : Gs2WebSocketSessionTask<Request.CreateTakeOverOpenIdConnectAndByUserIdRequest, Result.CreateTakeOverOpenIdConnectAndByUserIdResult>
        {
            public CreateTakeOverOpenIdConnectAndByUserIdTask(IGs2Session session, Request.CreateTakeOverOpenIdConnectAndByUserIdRequest request) : base(session, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(Request.CreateTakeOverOpenIdConnectAndByUserIdRequest request)
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
                if (request.Type != null)
                {
                    jsonWriter.WritePropertyName("type");
                    jsonWriter.Write(request.Type.ToString());
                }
                if (request.IdToken != null)
                {
                    jsonWriter.WritePropertyName("idToken");
                    jsonWriter.Write(request.IdToken.ToString());
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
                    "account",
                    "takeOver",
                    "createTakeOverOpenIdConnectAndByUserId",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
        public IEnumerator CreateTakeOverOpenIdConnectAndByUserId(
                Request.CreateTakeOverOpenIdConnectAndByUserIdRequest request,
                UnityAction<AsyncResult<Result.CreateTakeOverOpenIdConnectAndByUserIdResult>> callback
        ) =>
            new CreateTakeOverOpenIdConnectAndByUserIdTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToCoroutine(callback);

        public IFuture<Result.CreateTakeOverOpenIdConnectAndByUserIdResult> CreateTakeOverOpenIdConnectAndByUserIdFuture(
                Request.CreateTakeOverOpenIdConnectAndByUserIdRequest request
        ) =>
            new CreateTakeOverOpenIdConnectAndByUserIdTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
        public UniTask<Result.CreateTakeOverOpenIdConnectAndByUserIdResult> CreateTakeOverOpenIdConnectAndByUserIdAsync(
            Request.CreateTakeOverOpenIdConnectAndByUserIdRequest request
        ) =>
            new CreateTakeOverOpenIdConnectAndByUserIdTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsUniTask<Result.CreateTakeOverOpenIdConnectAndByUserIdResult>();
    #else
        public CreateTakeOverOpenIdConnectAndByUserIdTask CreateTakeOverOpenIdConnectAndByUserIdAsync(
                Request.CreateTakeOverOpenIdConnectAndByUserIdRequest request
        )
        {
            return new CreateTakeOverOpenIdConnectAndByUserIdTask(
                Gs2WebSocketSession,
                request
            );
        }
    #endif
#else
        public Task<Result.CreateTakeOverOpenIdConnectAndByUserIdResult> CreateTakeOverOpenIdConnectAndByUserIdAsync(
            Request.CreateTakeOverOpenIdConnectAndByUserIdRequest request
        ) =>
            new CreateTakeOverOpenIdConnectAndByUserIdTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsTask();
#endif


        public class GetTakeOverTask : Gs2WebSocketSessionTask<Request.GetTakeOverRequest, Result.GetTakeOverResult>
        {
            public GetTakeOverTask(IGs2Session session, Request.GetTakeOverRequest request) : base(session, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(Request.GetTakeOverRequest request)
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
                if (request.Type != null)
                {
                    jsonWriter.WritePropertyName("type");
                    jsonWriter.Write(request.Type.ToString());
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
                    "account",
                    "takeOver",
                    "getTakeOver",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
        public IEnumerator GetTakeOver(
                Request.GetTakeOverRequest request,
                UnityAction<AsyncResult<Result.GetTakeOverResult>> callback
        ) =>
            new GetTakeOverTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToCoroutine(callback);

        public IFuture<Result.GetTakeOverResult> GetTakeOverFuture(
                Request.GetTakeOverRequest request
        ) =>
            new GetTakeOverTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
        public UniTask<Result.GetTakeOverResult> GetTakeOverAsync(
            Request.GetTakeOverRequest request
        ) =>
            new GetTakeOverTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsUniTask<Result.GetTakeOverResult>();
    #else
        public GetTakeOverTask GetTakeOverAsync(
                Request.GetTakeOverRequest request
        )
        {
            return new GetTakeOverTask(
                Gs2WebSocketSession,
                request
            );
        }
    #endif
#else
        public Task<Result.GetTakeOverResult> GetTakeOverAsync(
            Request.GetTakeOverRequest request
        ) =>
            new GetTakeOverTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsTask();
#endif


        public class GetTakeOverByUserIdTask : Gs2WebSocketSessionTask<Request.GetTakeOverByUserIdRequest, Result.GetTakeOverByUserIdResult>
        {
            public GetTakeOverByUserIdTask(IGs2Session session, Request.GetTakeOverByUserIdRequest request) : base(session, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(Request.GetTakeOverByUserIdRequest request)
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
                if (request.Type != null)
                {
                    jsonWriter.WritePropertyName("type");
                    jsonWriter.Write(request.Type.ToString());
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
                    "account",
                    "takeOver",
                    "getTakeOverByUserId",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
        public IEnumerator GetTakeOverByUserId(
                Request.GetTakeOverByUserIdRequest request,
                UnityAction<AsyncResult<Result.GetTakeOverByUserIdResult>> callback
        ) =>
            new GetTakeOverByUserIdTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToCoroutine(callback);

        public IFuture<Result.GetTakeOverByUserIdResult> GetTakeOverByUserIdFuture(
                Request.GetTakeOverByUserIdRequest request
        ) =>
            new GetTakeOverByUserIdTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
        public UniTask<Result.GetTakeOverByUserIdResult> GetTakeOverByUserIdAsync(
            Request.GetTakeOverByUserIdRequest request
        ) =>
            new GetTakeOverByUserIdTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsUniTask<Result.GetTakeOverByUserIdResult>();
    #else
        public GetTakeOverByUserIdTask GetTakeOverByUserIdAsync(
                Request.GetTakeOverByUserIdRequest request
        )
        {
            return new GetTakeOverByUserIdTask(
                Gs2WebSocketSession,
                request
            );
        }
    #endif
#else
        public Task<Result.GetTakeOverByUserIdResult> GetTakeOverByUserIdAsync(
            Request.GetTakeOverByUserIdRequest request
        ) =>
            new GetTakeOverByUserIdTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsTask();
#endif


        public class UpdateTakeOverTask : Gs2WebSocketSessionTask<Request.UpdateTakeOverRequest, Result.UpdateTakeOverResult>
        {
            public UpdateTakeOverTask(IGs2Session session, Request.UpdateTakeOverRequest request) : base(session, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(Request.UpdateTakeOverRequest request)
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
                if (request.Type != null)
                {
                    jsonWriter.WritePropertyName("type");
                    jsonWriter.Write(request.Type.ToString());
                }
                if (request.OldPassword != null)
                {
                    jsonWriter.WritePropertyName("oldPassword");
                    jsonWriter.Write(request.OldPassword.ToString());
                }
                if (request.Password != null)
                {
                    jsonWriter.WritePropertyName("password");
                    jsonWriter.Write(request.Password.ToString());
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
                    "account",
                    "takeOver",
                    "updateTakeOver",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }

            public override void OnError(Gs2.Core.Exception.Gs2Exception error)
            {
                if (error.Errors.Count(v => v.code == "account.password.invalid") > 0) {
                    base.OnError(new Exception.PasswordIncorrectException(error));
                }
                else {
                    base.OnError(error);
                }
            }
        }

#if UNITY_2017_1_OR_NEWER
        public IEnumerator UpdateTakeOver(
                Request.UpdateTakeOverRequest request,
                UnityAction<AsyncResult<Result.UpdateTakeOverResult>> callback
        ) =>
            new UpdateTakeOverTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToCoroutine(callback);

        public IFuture<Result.UpdateTakeOverResult> UpdateTakeOverFuture(
                Request.UpdateTakeOverRequest request
        ) =>
            new UpdateTakeOverTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
        public UniTask<Result.UpdateTakeOverResult> UpdateTakeOverAsync(
            Request.UpdateTakeOverRequest request
        ) =>
            new UpdateTakeOverTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsUniTask<Result.UpdateTakeOverResult>();
    #else
        public UpdateTakeOverTask UpdateTakeOverAsync(
                Request.UpdateTakeOverRequest request
        )
        {
            return new UpdateTakeOverTask(
                Gs2WebSocketSession,
                request
            );
        }
    #endif
#else
        public Task<Result.UpdateTakeOverResult> UpdateTakeOverAsync(
            Request.UpdateTakeOverRequest request
        ) =>
            new UpdateTakeOverTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsTask();
#endif


        public class UpdateTakeOverByUserIdTask : Gs2WebSocketSessionTask<Request.UpdateTakeOverByUserIdRequest, Result.UpdateTakeOverByUserIdResult>
        {
            public UpdateTakeOverByUserIdTask(IGs2Session session, Request.UpdateTakeOverByUserIdRequest request) : base(session, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(Request.UpdateTakeOverByUserIdRequest request)
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
                if (request.Type != null)
                {
                    jsonWriter.WritePropertyName("type");
                    jsonWriter.Write(request.Type.ToString());
                }
                if (request.OldPassword != null)
                {
                    jsonWriter.WritePropertyName("oldPassword");
                    jsonWriter.Write(request.OldPassword.ToString());
                }
                if (request.Password != null)
                {
                    jsonWriter.WritePropertyName("password");
                    jsonWriter.Write(request.Password.ToString());
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
                    "account",
                    "takeOver",
                    "updateTakeOverByUserId",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }

            public override void OnError(Gs2.Core.Exception.Gs2Exception error)
            {
                if (error.Errors.Count(v => v.code == "account.password.invalid") > 0) {
                    base.OnError(new Exception.PasswordIncorrectException(error));
                }
                else {
                    base.OnError(error);
                }
            }
        }

#if UNITY_2017_1_OR_NEWER
        public IEnumerator UpdateTakeOverByUserId(
                Request.UpdateTakeOverByUserIdRequest request,
                UnityAction<AsyncResult<Result.UpdateTakeOverByUserIdResult>> callback
        ) =>
            new UpdateTakeOverByUserIdTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToCoroutine(callback);

        public IFuture<Result.UpdateTakeOverByUserIdResult> UpdateTakeOverByUserIdFuture(
                Request.UpdateTakeOverByUserIdRequest request
        ) =>
            new UpdateTakeOverByUserIdTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
        public UniTask<Result.UpdateTakeOverByUserIdResult> UpdateTakeOverByUserIdAsync(
            Request.UpdateTakeOverByUserIdRequest request
        ) =>
            new UpdateTakeOverByUserIdTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsUniTask<Result.UpdateTakeOverByUserIdResult>();
    #else
        public UpdateTakeOverByUserIdTask UpdateTakeOverByUserIdAsync(
                Request.UpdateTakeOverByUserIdRequest request
        )
        {
            return new UpdateTakeOverByUserIdTask(
                Gs2WebSocketSession,
                request
            );
        }
    #endif
#else
        public Task<Result.UpdateTakeOverByUserIdResult> UpdateTakeOverByUserIdAsync(
            Request.UpdateTakeOverByUserIdRequest request
        ) =>
            new UpdateTakeOverByUserIdTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsTask();
#endif


        public class DeleteTakeOverTask : Gs2WebSocketSessionTask<Request.DeleteTakeOverRequest, Result.DeleteTakeOverResult>
        {
            public DeleteTakeOverTask(IGs2Session session, Request.DeleteTakeOverRequest request) : base(session, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(Request.DeleteTakeOverRequest request)
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
                if (request.Type != null)
                {
                    jsonWriter.WritePropertyName("type");
                    jsonWriter.Write(request.Type.ToString());
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
                    "account",
                    "takeOver",
                    "deleteTakeOver",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
        public IEnumerator DeleteTakeOver(
                Request.DeleteTakeOverRequest request,
                UnityAction<AsyncResult<Result.DeleteTakeOverResult>> callback
        ) =>
            new DeleteTakeOverTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToCoroutine(callback);

        public IFuture<Result.DeleteTakeOverResult> DeleteTakeOverFuture(
                Request.DeleteTakeOverRequest request
        ) =>
            new DeleteTakeOverTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
        public UniTask<Result.DeleteTakeOverResult> DeleteTakeOverAsync(
            Request.DeleteTakeOverRequest request
        ) =>
            new DeleteTakeOverTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsUniTask<Result.DeleteTakeOverResult>();
    #else
        public DeleteTakeOverTask DeleteTakeOverAsync(
                Request.DeleteTakeOverRequest request
        )
        {
            return new DeleteTakeOverTask(
                Gs2WebSocketSession,
                request
            );
        }
    #endif
#else
        public Task<Result.DeleteTakeOverResult> DeleteTakeOverAsync(
            Request.DeleteTakeOverRequest request
        ) =>
            new DeleteTakeOverTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsTask();
#endif


        public class DeleteTakeOverByUserIdentifierTask : Gs2WebSocketSessionTask<Request.DeleteTakeOverByUserIdentifierRequest, Result.DeleteTakeOverByUserIdentifierResult>
        {
            public DeleteTakeOverByUserIdentifierTask(IGs2Session session, Request.DeleteTakeOverByUserIdentifierRequest request) : base(session, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(Request.DeleteTakeOverByUserIdentifierRequest request)
            {
                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);

                jsonWriter.WriteObjectStart();

                if (request.NamespaceName != null)
                {
                    jsonWriter.WritePropertyName("namespaceName");
                    jsonWriter.Write(request.NamespaceName.ToString());
                }
                if (request.Type != null)
                {
                    jsonWriter.WritePropertyName("type");
                    jsonWriter.Write(request.Type.ToString());
                }
                if (request.UserIdentifier != null)
                {
                    jsonWriter.WritePropertyName("userIdentifier");
                    jsonWriter.Write(request.UserIdentifier.ToString());
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
                    "account",
                    "takeOver",
                    "deleteTakeOverByUserIdentifier",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
        public IEnumerator DeleteTakeOverByUserIdentifier(
                Request.DeleteTakeOverByUserIdentifierRequest request,
                UnityAction<AsyncResult<Result.DeleteTakeOverByUserIdentifierResult>> callback
        ) =>
            new DeleteTakeOverByUserIdentifierTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToCoroutine(callback);

        public IFuture<Result.DeleteTakeOverByUserIdentifierResult> DeleteTakeOverByUserIdentifierFuture(
                Request.DeleteTakeOverByUserIdentifierRequest request
        ) =>
            new DeleteTakeOverByUserIdentifierTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
        public UniTask<Result.DeleteTakeOverByUserIdentifierResult> DeleteTakeOverByUserIdentifierAsync(
            Request.DeleteTakeOverByUserIdentifierRequest request
        ) =>
            new DeleteTakeOverByUserIdentifierTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsUniTask<Result.DeleteTakeOverByUserIdentifierResult>();
    #else
        public DeleteTakeOverByUserIdentifierTask DeleteTakeOverByUserIdentifierAsync(
                Request.DeleteTakeOverByUserIdentifierRequest request
        )
        {
            return new DeleteTakeOverByUserIdentifierTask(
                Gs2WebSocketSession,
                request
            );
        }
    #endif
#else
        public Task<Result.DeleteTakeOverByUserIdentifierResult> DeleteTakeOverByUserIdentifierAsync(
            Request.DeleteTakeOverByUserIdentifierRequest request
        ) =>
            new DeleteTakeOverByUserIdentifierTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsTask();
#endif


        public class DeleteTakeOverByUserIdTask : Gs2WebSocketSessionTask<Request.DeleteTakeOverByUserIdRequest, Result.DeleteTakeOverByUserIdResult>
        {
            public DeleteTakeOverByUserIdTask(IGs2Session session, Request.DeleteTakeOverByUserIdRequest request) : base(session, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(Request.DeleteTakeOverByUserIdRequest request)
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
                if (request.Type != null)
                {
                    jsonWriter.WritePropertyName("type");
                    jsonWriter.Write(request.Type.ToString());
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
                    "account",
                    "takeOver",
                    "deleteTakeOverByUserId",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
        public IEnumerator DeleteTakeOverByUserId(
                Request.DeleteTakeOverByUserIdRequest request,
                UnityAction<AsyncResult<Result.DeleteTakeOverByUserIdResult>> callback
        ) =>
            new DeleteTakeOverByUserIdTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToCoroutine(callback);

        public IFuture<Result.DeleteTakeOverByUserIdResult> DeleteTakeOverByUserIdFuture(
                Request.DeleteTakeOverByUserIdRequest request
        ) =>
            new DeleteTakeOverByUserIdTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
        public UniTask<Result.DeleteTakeOverByUserIdResult> DeleteTakeOverByUserIdAsync(
            Request.DeleteTakeOverByUserIdRequest request
        ) =>
            new DeleteTakeOverByUserIdTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsUniTask<Result.DeleteTakeOverByUserIdResult>();
    #else
        public DeleteTakeOverByUserIdTask DeleteTakeOverByUserIdAsync(
                Request.DeleteTakeOverByUserIdRequest request
        )
        {
            return new DeleteTakeOverByUserIdTask(
                Gs2WebSocketSession,
                request
            );
        }
    #endif
#else
        public Task<Result.DeleteTakeOverByUserIdResult> DeleteTakeOverByUserIdAsync(
            Request.DeleteTakeOverByUserIdRequest request
        ) =>
            new DeleteTakeOverByUserIdTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsTask();
#endif


        public class DoTakeOverTask : Gs2WebSocketSessionTask<Request.DoTakeOverRequest, Result.DoTakeOverResult>
        {
            public DoTakeOverTask(IGs2Session session, Request.DoTakeOverRequest request) : base(session, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(Request.DoTakeOverRequest request)
            {
                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);

                jsonWriter.WriteObjectStart();

                if (request.NamespaceName != null)
                {
                    jsonWriter.WritePropertyName("namespaceName");
                    jsonWriter.Write(request.NamespaceName.ToString());
                }
                if (request.Type != null)
                {
                    jsonWriter.WritePropertyName("type");
                    jsonWriter.Write(request.Type.ToString());
                }
                if (request.UserIdentifier != null)
                {
                    jsonWriter.WritePropertyName("userIdentifier");
                    jsonWriter.Write(request.UserIdentifier.ToString());
                }
                if (request.Password != null)
                {
                    jsonWriter.WritePropertyName("password");
                    jsonWriter.Write(request.Password.ToString());
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
                    "account",
                    "takeOver",
                    "doTakeOver",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }

            public override void OnError(Gs2.Core.Exception.Gs2Exception error)
            {
                if (error.Errors.Count(v => v.code == "account.password.invalid") > 0) {
                    base.OnError(new Exception.PasswordIncorrectException(error));
                }
                else {
                    base.OnError(error);
                }
            }
        }

#if UNITY_2017_1_OR_NEWER
        public IEnumerator DoTakeOver(
                Request.DoTakeOverRequest request,
                UnityAction<AsyncResult<Result.DoTakeOverResult>> callback
        ) =>
            new DoTakeOverTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToCoroutine(callback);

        public IFuture<Result.DoTakeOverResult> DoTakeOverFuture(
                Request.DoTakeOverRequest request
        ) =>
            new DoTakeOverTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
        public UniTask<Result.DoTakeOverResult> DoTakeOverAsync(
            Request.DoTakeOverRequest request
        ) =>
            new DoTakeOverTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsUniTask<Result.DoTakeOverResult>();
    #else
        public DoTakeOverTask DoTakeOverAsync(
                Request.DoTakeOverRequest request
        )
        {
            return new DoTakeOverTask(
                Gs2WebSocketSession,
                request
            );
        }
    #endif
#else
        public Task<Result.DoTakeOverResult> DoTakeOverAsync(
            Request.DoTakeOverRequest request
        ) =>
            new DoTakeOverTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsTask();
#endif


        public class DoTakeOverOpenIdConnectTask : Gs2WebSocketSessionTask<Request.DoTakeOverOpenIdConnectRequest, Result.DoTakeOverOpenIdConnectResult>
        {
            public DoTakeOverOpenIdConnectTask(IGs2Session session, Request.DoTakeOverOpenIdConnectRequest request) : base(session, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(Request.DoTakeOverOpenIdConnectRequest request)
            {
                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);

                jsonWriter.WriteObjectStart();

                if (request.NamespaceName != null)
                {
                    jsonWriter.WritePropertyName("namespaceName");
                    jsonWriter.Write(request.NamespaceName.ToString());
                }
                if (request.Type != null)
                {
                    jsonWriter.WritePropertyName("type");
                    jsonWriter.Write(request.Type.ToString());
                }
                if (request.IdToken != null)
                {
                    jsonWriter.WritePropertyName("idToken");
                    jsonWriter.Write(request.IdToken.ToString());
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
                    "account",
                    "takeOver",
                    "doTakeOverOpenIdConnect",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
        public IEnumerator DoTakeOverOpenIdConnect(
                Request.DoTakeOverOpenIdConnectRequest request,
                UnityAction<AsyncResult<Result.DoTakeOverOpenIdConnectResult>> callback
        ) =>
            new DoTakeOverOpenIdConnectTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToCoroutine(callback);

        public IFuture<Result.DoTakeOverOpenIdConnectResult> DoTakeOverOpenIdConnectFuture(
                Request.DoTakeOverOpenIdConnectRequest request
        ) =>
            new DoTakeOverOpenIdConnectTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
        public UniTask<Result.DoTakeOverOpenIdConnectResult> DoTakeOverOpenIdConnectAsync(
            Request.DoTakeOverOpenIdConnectRequest request
        ) =>
            new DoTakeOverOpenIdConnectTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsUniTask<Result.DoTakeOverOpenIdConnectResult>();
    #else
        public DoTakeOverOpenIdConnectTask DoTakeOverOpenIdConnectAsync(
                Request.DoTakeOverOpenIdConnectRequest request
        )
        {
            return new DoTakeOverOpenIdConnectTask(
                Gs2WebSocketSession,
                request
            );
        }
    #endif
#else
        public Task<Result.DoTakeOverOpenIdConnectResult> DoTakeOverOpenIdConnectAsync(
            Request.DoTakeOverOpenIdConnectRequest request
        ) =>
            new DoTakeOverOpenIdConnectTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsTask();
#endif


        public class GetAuthorizationUrlTask : Gs2WebSocketSessionTask<Request.GetAuthorizationUrlRequest, Result.GetAuthorizationUrlResult>
        {
            public GetAuthorizationUrlTask(IGs2Session session, Request.GetAuthorizationUrlRequest request) : base(session, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(Request.GetAuthorizationUrlRequest request)
            {
                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);

                jsonWriter.WriteObjectStart();

                if (request.NamespaceName != null)
                {
                    jsonWriter.WritePropertyName("namespaceName");
                    jsonWriter.Write(request.NamespaceName.ToString());
                }
                if (request.Type != null)
                {
                    jsonWriter.WritePropertyName("type");
                    jsonWriter.Write(request.Type.ToString());
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
                    "account",
                    "takeOver",
                    "getAuthorizationUrl",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
        public IEnumerator GetAuthorizationUrl(
                Request.GetAuthorizationUrlRequest request,
                UnityAction<AsyncResult<Result.GetAuthorizationUrlResult>> callback
        ) =>
            new GetAuthorizationUrlTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToCoroutine(callback);

        public IFuture<Result.GetAuthorizationUrlResult> GetAuthorizationUrlFuture(
                Request.GetAuthorizationUrlRequest request
        ) =>
            new GetAuthorizationUrlTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
        public UniTask<Result.GetAuthorizationUrlResult> GetAuthorizationUrlAsync(
            Request.GetAuthorizationUrlRequest request
        ) =>
            new GetAuthorizationUrlTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsUniTask<Result.GetAuthorizationUrlResult>();
    #else
        public GetAuthorizationUrlTask GetAuthorizationUrlAsync(
                Request.GetAuthorizationUrlRequest request
        )
        {
            return new GetAuthorizationUrlTask(
                Gs2WebSocketSession,
                request
            );
        }
    #endif
#else
        public Task<Result.GetAuthorizationUrlResult> GetAuthorizationUrlAsync(
            Request.GetAuthorizationUrlRequest request
        ) =>
            new GetAuthorizationUrlTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsTask();
#endif


        public class CreatePlatformIdTask : Gs2WebSocketSessionTask<Request.CreatePlatformIdRequest, Result.CreatePlatformIdResult>
        {
            public CreatePlatformIdTask(IGs2Session session, Request.CreatePlatformIdRequest request) : base(session, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(Request.CreatePlatformIdRequest request)
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
                if (request.Type != null)
                {
                    jsonWriter.WritePropertyName("type");
                    jsonWriter.Write(request.Type.ToString());
                }
                if (request.UserIdentifier != null)
                {
                    jsonWriter.WritePropertyName("userIdentifier");
                    jsonWriter.Write(request.UserIdentifier.ToString());
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
                    "account",
                    "platformId",
                    "createPlatformId",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
        public IEnumerator CreatePlatformId(
                Request.CreatePlatformIdRequest request,
                UnityAction<AsyncResult<Result.CreatePlatformIdResult>> callback
        ) =>
            new CreatePlatformIdTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToCoroutine(callback);

        public IFuture<Result.CreatePlatformIdResult> CreatePlatformIdFuture(
                Request.CreatePlatformIdRequest request
        ) =>
            new CreatePlatformIdTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
        public UniTask<Result.CreatePlatformIdResult> CreatePlatformIdAsync(
            Request.CreatePlatformIdRequest request
        ) =>
            new CreatePlatformIdTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsUniTask<Result.CreatePlatformIdResult>();
    #else
        public CreatePlatformIdTask CreatePlatformIdAsync(
                Request.CreatePlatformIdRequest request
        )
        {
            return new CreatePlatformIdTask(
                Gs2WebSocketSession,
                request
            );
        }
    #endif
#else
        public Task<Result.CreatePlatformIdResult> CreatePlatformIdAsync(
            Request.CreatePlatformIdRequest request
        ) =>
            new CreatePlatformIdTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsTask();
#endif


        public class CreatePlatformIdByUserIdTask : Gs2WebSocketSessionTask<Request.CreatePlatformIdByUserIdRequest, Result.CreatePlatformIdByUserIdResult>
        {
            public CreatePlatformIdByUserIdTask(IGs2Session session, Request.CreatePlatformIdByUserIdRequest request) : base(session, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(Request.CreatePlatformIdByUserIdRequest request)
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
                if (request.Type != null)
                {
                    jsonWriter.WritePropertyName("type");
                    jsonWriter.Write(request.Type.ToString());
                }
                if (request.UserIdentifier != null)
                {
                    jsonWriter.WritePropertyName("userIdentifier");
                    jsonWriter.Write(request.UserIdentifier.ToString());
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
                    "account",
                    "platformId",
                    "createPlatformIdByUserId",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
        public IEnumerator CreatePlatformIdByUserId(
                Request.CreatePlatformIdByUserIdRequest request,
                UnityAction<AsyncResult<Result.CreatePlatformIdByUserIdResult>> callback
        ) =>
            new CreatePlatformIdByUserIdTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToCoroutine(callback);

        public IFuture<Result.CreatePlatformIdByUserIdResult> CreatePlatformIdByUserIdFuture(
                Request.CreatePlatformIdByUserIdRequest request
        ) =>
            new CreatePlatformIdByUserIdTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
        public UniTask<Result.CreatePlatformIdByUserIdResult> CreatePlatformIdByUserIdAsync(
            Request.CreatePlatformIdByUserIdRequest request
        ) =>
            new CreatePlatformIdByUserIdTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsUniTask<Result.CreatePlatformIdByUserIdResult>();
    #else
        public CreatePlatformIdByUserIdTask CreatePlatformIdByUserIdAsync(
                Request.CreatePlatformIdByUserIdRequest request
        )
        {
            return new CreatePlatformIdByUserIdTask(
                Gs2WebSocketSession,
                request
            );
        }
    #endif
#else
        public Task<Result.CreatePlatformIdByUserIdResult> CreatePlatformIdByUserIdAsync(
            Request.CreatePlatformIdByUserIdRequest request
        ) =>
            new CreatePlatformIdByUserIdTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsTask();
#endif


        public class GetPlatformIdTask : Gs2WebSocketSessionTask<Request.GetPlatformIdRequest, Result.GetPlatformIdResult>
        {
            public GetPlatformIdTask(IGs2Session session, Request.GetPlatformIdRequest request) : base(session, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(Request.GetPlatformIdRequest request)
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
                if (request.Type != null)
                {
                    jsonWriter.WritePropertyName("type");
                    jsonWriter.Write(request.Type.ToString());
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
                    "account",
                    "platformId",
                    "getPlatformId",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
        public IEnumerator GetPlatformId(
                Request.GetPlatformIdRequest request,
                UnityAction<AsyncResult<Result.GetPlatformIdResult>> callback
        ) =>
            new GetPlatformIdTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToCoroutine(callback);

        public IFuture<Result.GetPlatformIdResult> GetPlatformIdFuture(
                Request.GetPlatformIdRequest request
        ) =>
            new GetPlatformIdTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
        public UniTask<Result.GetPlatformIdResult> GetPlatformIdAsync(
            Request.GetPlatformIdRequest request
        ) =>
            new GetPlatformIdTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsUniTask<Result.GetPlatformIdResult>();
    #else
        public GetPlatformIdTask GetPlatformIdAsync(
                Request.GetPlatformIdRequest request
        )
        {
            return new GetPlatformIdTask(
                Gs2WebSocketSession,
                request
            );
        }
    #endif
#else
        public Task<Result.GetPlatformIdResult> GetPlatformIdAsync(
            Request.GetPlatformIdRequest request
        ) =>
            new GetPlatformIdTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsTask();
#endif


        public class GetPlatformIdByUserIdTask : Gs2WebSocketSessionTask<Request.GetPlatformIdByUserIdRequest, Result.GetPlatformIdByUserIdResult>
        {
            public GetPlatformIdByUserIdTask(IGs2Session session, Request.GetPlatformIdByUserIdRequest request) : base(session, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(Request.GetPlatformIdByUserIdRequest request)
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
                if (request.Type != null)
                {
                    jsonWriter.WritePropertyName("type");
                    jsonWriter.Write(request.Type.ToString());
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
                    "account",
                    "platformId",
                    "getPlatformIdByUserId",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
        public IEnumerator GetPlatformIdByUserId(
                Request.GetPlatformIdByUserIdRequest request,
                UnityAction<AsyncResult<Result.GetPlatformIdByUserIdResult>> callback
        ) =>
            new GetPlatformIdByUserIdTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToCoroutine(callback);

        public IFuture<Result.GetPlatformIdByUserIdResult> GetPlatformIdByUserIdFuture(
                Request.GetPlatformIdByUserIdRequest request
        ) =>
            new GetPlatformIdByUserIdTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
        public UniTask<Result.GetPlatformIdByUserIdResult> GetPlatformIdByUserIdAsync(
            Request.GetPlatformIdByUserIdRequest request
        ) =>
            new GetPlatformIdByUserIdTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsUniTask<Result.GetPlatformIdByUserIdResult>();
    #else
        public GetPlatformIdByUserIdTask GetPlatformIdByUserIdAsync(
                Request.GetPlatformIdByUserIdRequest request
        )
        {
            return new GetPlatformIdByUserIdTask(
                Gs2WebSocketSession,
                request
            );
        }
    #endif
#else
        public Task<Result.GetPlatformIdByUserIdResult> GetPlatformIdByUserIdAsync(
            Request.GetPlatformIdByUserIdRequest request
        ) =>
            new GetPlatformIdByUserIdTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsTask();
#endif


        public class FindPlatformIdTask : Gs2WebSocketSessionTask<Request.FindPlatformIdRequest, Result.FindPlatformIdResult>
        {
            public FindPlatformIdTask(IGs2Session session, Request.FindPlatformIdRequest request) : base(session, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(Request.FindPlatformIdRequest request)
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
                if (request.Type != null)
                {
                    jsonWriter.WritePropertyName("type");
                    jsonWriter.Write(request.Type.ToString());
                }
                if (request.UserIdentifier != null)
                {
                    jsonWriter.WritePropertyName("userIdentifier");
                    jsonWriter.Write(request.UserIdentifier.ToString());
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
                    "account",
                    "platformId",
                    "findPlatformId",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
        public IEnumerator FindPlatformId(
                Request.FindPlatformIdRequest request,
                UnityAction<AsyncResult<Result.FindPlatformIdResult>> callback
        ) =>
            new FindPlatformIdTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToCoroutine(callback);

        public IFuture<Result.FindPlatformIdResult> FindPlatformIdFuture(
                Request.FindPlatformIdRequest request
        ) =>
            new FindPlatformIdTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
        public UniTask<Result.FindPlatformIdResult> FindPlatformIdAsync(
            Request.FindPlatformIdRequest request
        ) =>
            new FindPlatformIdTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsUniTask<Result.FindPlatformIdResult>();
    #else
        public FindPlatformIdTask FindPlatformIdAsync(
                Request.FindPlatformIdRequest request
        )
        {
            return new FindPlatformIdTask(
                Gs2WebSocketSession,
                request
            );
        }
    #endif
#else
        public Task<Result.FindPlatformIdResult> FindPlatformIdAsync(
            Request.FindPlatformIdRequest request
        ) =>
            new FindPlatformIdTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsTask();
#endif


        public class FindPlatformIdByUserIdTask : Gs2WebSocketSessionTask<Request.FindPlatformIdByUserIdRequest, Result.FindPlatformIdByUserIdResult>
        {
            public FindPlatformIdByUserIdTask(IGs2Session session, Request.FindPlatformIdByUserIdRequest request) : base(session, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(Request.FindPlatformIdByUserIdRequest request)
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
                if (request.Type != null)
                {
                    jsonWriter.WritePropertyName("type");
                    jsonWriter.Write(request.Type.ToString());
                }
                if (request.UserIdentifier != null)
                {
                    jsonWriter.WritePropertyName("userIdentifier");
                    jsonWriter.Write(request.UserIdentifier.ToString());
                }
                if (request.DontResolveDataOwner != null)
                {
                    jsonWriter.WritePropertyName("dontResolveDataOwner");
                    jsonWriter.Write(request.DontResolveDataOwner.ToString());
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
                    "account",
                    "platformId",
                    "findPlatformIdByUserId",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
        public IEnumerator FindPlatformIdByUserId(
                Request.FindPlatformIdByUserIdRequest request,
                UnityAction<AsyncResult<Result.FindPlatformIdByUserIdResult>> callback
        ) =>
            new FindPlatformIdByUserIdTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToCoroutine(callback);

        public IFuture<Result.FindPlatformIdByUserIdResult> FindPlatformIdByUserIdFuture(
                Request.FindPlatformIdByUserIdRequest request
        ) =>
            new FindPlatformIdByUserIdTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
        public UniTask<Result.FindPlatformIdByUserIdResult> FindPlatformIdByUserIdAsync(
            Request.FindPlatformIdByUserIdRequest request
        ) =>
            new FindPlatformIdByUserIdTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsUniTask<Result.FindPlatformIdByUserIdResult>();
    #else
        public FindPlatformIdByUserIdTask FindPlatformIdByUserIdAsync(
                Request.FindPlatformIdByUserIdRequest request
        )
        {
            return new FindPlatformIdByUserIdTask(
                Gs2WebSocketSession,
                request
            );
        }
    #endif
#else
        public Task<Result.FindPlatformIdByUserIdResult> FindPlatformIdByUserIdAsync(
            Request.FindPlatformIdByUserIdRequest request
        ) =>
            new FindPlatformIdByUserIdTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsTask();
#endif


        public class DeletePlatformIdTask : Gs2WebSocketSessionTask<Request.DeletePlatformIdRequest, Result.DeletePlatformIdResult>
        {
            public DeletePlatformIdTask(IGs2Session session, Request.DeletePlatformIdRequest request) : base(session, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(Request.DeletePlatformIdRequest request)
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
                if (request.Type != null)
                {
                    jsonWriter.WritePropertyName("type");
                    jsonWriter.Write(request.Type.ToString());
                }
                if (request.UserIdentifier != null)
                {
                    jsonWriter.WritePropertyName("userIdentifier");
                    jsonWriter.Write(request.UserIdentifier.ToString());
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
                    "account",
                    "platformId",
                    "deletePlatformId",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
        public IEnumerator DeletePlatformId(
                Request.DeletePlatformIdRequest request,
                UnityAction<AsyncResult<Result.DeletePlatformIdResult>> callback
        ) =>
            new DeletePlatformIdTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToCoroutine(callback);

        public IFuture<Result.DeletePlatformIdResult> DeletePlatformIdFuture(
                Request.DeletePlatformIdRequest request
        ) =>
            new DeletePlatformIdTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
        public UniTask<Result.DeletePlatformIdResult> DeletePlatformIdAsync(
            Request.DeletePlatformIdRequest request
        ) =>
            new DeletePlatformIdTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsUniTask<Result.DeletePlatformIdResult>();
    #else
        public DeletePlatformIdTask DeletePlatformIdAsync(
                Request.DeletePlatformIdRequest request
        )
        {
            return new DeletePlatformIdTask(
                Gs2WebSocketSession,
                request
            );
        }
    #endif
#else
        public Task<Result.DeletePlatformIdResult> DeletePlatformIdAsync(
            Request.DeletePlatformIdRequest request
        ) =>
            new DeletePlatformIdTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsTask();
#endif


        public class DeletePlatformIdByUserIdentifierTask : Gs2WebSocketSessionTask<Request.DeletePlatformIdByUserIdentifierRequest, Result.DeletePlatformIdByUserIdentifierResult>
        {
            public DeletePlatformIdByUserIdentifierTask(IGs2Session session, Request.DeletePlatformIdByUserIdentifierRequest request) : base(session, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(Request.DeletePlatformIdByUserIdentifierRequest request)
            {
                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);

                jsonWriter.WriteObjectStart();

                if (request.NamespaceName != null)
                {
                    jsonWriter.WritePropertyName("namespaceName");
                    jsonWriter.Write(request.NamespaceName.ToString());
                }
                if (request.Type != null)
                {
                    jsonWriter.WritePropertyName("type");
                    jsonWriter.Write(request.Type.ToString());
                }
                if (request.UserIdentifier != null)
                {
                    jsonWriter.WritePropertyName("userIdentifier");
                    jsonWriter.Write(request.UserIdentifier.ToString());
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
                    "account",
                    "platformId",
                    "deletePlatformIdByUserIdentifier",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
        public IEnumerator DeletePlatformIdByUserIdentifier(
                Request.DeletePlatformIdByUserIdentifierRequest request,
                UnityAction<AsyncResult<Result.DeletePlatformIdByUserIdentifierResult>> callback
        ) =>
            new DeletePlatformIdByUserIdentifierTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToCoroutine(callback);

        public IFuture<Result.DeletePlatformIdByUserIdentifierResult> DeletePlatformIdByUserIdentifierFuture(
                Request.DeletePlatformIdByUserIdentifierRequest request
        ) =>
            new DeletePlatformIdByUserIdentifierTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
        public UniTask<Result.DeletePlatformIdByUserIdentifierResult> DeletePlatformIdByUserIdentifierAsync(
            Request.DeletePlatformIdByUserIdentifierRequest request
        ) =>
            new DeletePlatformIdByUserIdentifierTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsUniTask<Result.DeletePlatformIdByUserIdentifierResult>();
    #else
        public DeletePlatformIdByUserIdentifierTask DeletePlatformIdByUserIdentifierAsync(
                Request.DeletePlatformIdByUserIdentifierRequest request
        )
        {
            return new DeletePlatformIdByUserIdentifierTask(
                Gs2WebSocketSession,
                request
            );
        }
    #endif
#else
        public Task<Result.DeletePlatformIdByUserIdentifierResult> DeletePlatformIdByUserIdentifierAsync(
            Request.DeletePlatformIdByUserIdentifierRequest request
        ) =>
            new DeletePlatformIdByUserIdentifierTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsTask();
#endif


        public class DeletePlatformIdByUserIdTask : Gs2WebSocketSessionTask<Request.DeletePlatformIdByUserIdRequest, Result.DeletePlatformIdByUserIdResult>
        {
            public DeletePlatformIdByUserIdTask(IGs2Session session, Request.DeletePlatformIdByUserIdRequest request) : base(session, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(Request.DeletePlatformIdByUserIdRequest request)
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
                if (request.Type != null)
                {
                    jsonWriter.WritePropertyName("type");
                    jsonWriter.Write(request.Type.ToString());
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
                    "account",
                    "platformId",
                    "deletePlatformIdByUserId",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
        public IEnumerator DeletePlatformIdByUserId(
                Request.DeletePlatformIdByUserIdRequest request,
                UnityAction<AsyncResult<Result.DeletePlatformIdByUserIdResult>> callback
        ) =>
            new DeletePlatformIdByUserIdTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToCoroutine(callback);

        public IFuture<Result.DeletePlatformIdByUserIdResult> DeletePlatformIdByUserIdFuture(
                Request.DeletePlatformIdByUserIdRequest request
        ) =>
            new DeletePlatformIdByUserIdTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
        public UniTask<Result.DeletePlatformIdByUserIdResult> DeletePlatformIdByUserIdAsync(
            Request.DeletePlatformIdByUserIdRequest request
        ) =>
            new DeletePlatformIdByUserIdTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsUniTask<Result.DeletePlatformIdByUserIdResult>();
    #else
        public DeletePlatformIdByUserIdTask DeletePlatformIdByUserIdAsync(
                Request.DeletePlatformIdByUserIdRequest request
        )
        {
            return new DeletePlatformIdByUserIdTask(
                Gs2WebSocketSession,
                request
            );
        }
    #endif
#else
        public Task<Result.DeletePlatformIdByUserIdResult> DeletePlatformIdByUserIdAsync(
            Request.DeletePlatformIdByUserIdRequest request
        ) =>
            new DeletePlatformIdByUserIdTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsTask();
#endif


        public class GetDataOwnerByUserIdTask : Gs2WebSocketSessionTask<Request.GetDataOwnerByUserIdRequest, Result.GetDataOwnerByUserIdResult>
        {
            public GetDataOwnerByUserIdTask(IGs2Session session, Request.GetDataOwnerByUserIdRequest request) : base(session, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(Request.GetDataOwnerByUserIdRequest request)
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
                    "account",
                    "dataOwner",
                    "getDataOwnerByUserId",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
        public IEnumerator GetDataOwnerByUserId(
                Request.GetDataOwnerByUserIdRequest request,
                UnityAction<AsyncResult<Result.GetDataOwnerByUserIdResult>> callback
        ) =>
            new GetDataOwnerByUserIdTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToCoroutine(callback);

        public IFuture<Result.GetDataOwnerByUserIdResult> GetDataOwnerByUserIdFuture(
                Request.GetDataOwnerByUserIdRequest request
        ) =>
            new GetDataOwnerByUserIdTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
        public UniTask<Result.GetDataOwnerByUserIdResult> GetDataOwnerByUserIdAsync(
            Request.GetDataOwnerByUserIdRequest request
        ) =>
            new GetDataOwnerByUserIdTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsUniTask<Result.GetDataOwnerByUserIdResult>();
    #else
        public GetDataOwnerByUserIdTask GetDataOwnerByUserIdAsync(
                Request.GetDataOwnerByUserIdRequest request
        )
        {
            return new GetDataOwnerByUserIdTask(
                Gs2WebSocketSession,
                request
            );
        }
    #endif
#else
        public Task<Result.GetDataOwnerByUserIdResult> GetDataOwnerByUserIdAsync(
            Request.GetDataOwnerByUserIdRequest request
        ) =>
            new GetDataOwnerByUserIdTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsTask();
#endif


        public class UpdateDataOwnerByUserIdTask : Gs2WebSocketSessionTask<Request.UpdateDataOwnerByUserIdRequest, Result.UpdateDataOwnerByUserIdResult>
        {
            public UpdateDataOwnerByUserIdTask(IGs2Session session, Request.UpdateDataOwnerByUserIdRequest request) : base(session, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(Request.UpdateDataOwnerByUserIdRequest request)
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
                if (request.DataOwnerName != null)
                {
                    jsonWriter.WritePropertyName("dataOwnerName");
                    jsonWriter.Write(request.DataOwnerName.ToString());
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
                    "account",
                    "dataOwner",
                    "updateDataOwnerByUserId",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
        public IEnumerator UpdateDataOwnerByUserId(
                Request.UpdateDataOwnerByUserIdRequest request,
                UnityAction<AsyncResult<Result.UpdateDataOwnerByUserIdResult>> callback
        ) =>
            new UpdateDataOwnerByUserIdTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToCoroutine(callback);

        public IFuture<Result.UpdateDataOwnerByUserIdResult> UpdateDataOwnerByUserIdFuture(
                Request.UpdateDataOwnerByUserIdRequest request
        ) =>
            new UpdateDataOwnerByUserIdTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
        public UniTask<Result.UpdateDataOwnerByUserIdResult> UpdateDataOwnerByUserIdAsync(
            Request.UpdateDataOwnerByUserIdRequest request
        ) =>
            new UpdateDataOwnerByUserIdTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsUniTask<Result.UpdateDataOwnerByUserIdResult>();
    #else
        public UpdateDataOwnerByUserIdTask UpdateDataOwnerByUserIdAsync(
                Request.UpdateDataOwnerByUserIdRequest request
        )
        {
            return new UpdateDataOwnerByUserIdTask(
                Gs2WebSocketSession,
                request
            );
        }
    #endif
#else
        public Task<Result.UpdateDataOwnerByUserIdResult> UpdateDataOwnerByUserIdAsync(
            Request.UpdateDataOwnerByUserIdRequest request
        ) =>
            new UpdateDataOwnerByUserIdTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsTask();
#endif


        public class DeleteDataOwnerByUserIdTask : Gs2WebSocketSessionTask<Request.DeleteDataOwnerByUserIdRequest, Result.DeleteDataOwnerByUserIdResult>
        {
            public DeleteDataOwnerByUserIdTask(IGs2Session session, Request.DeleteDataOwnerByUserIdRequest request) : base(session, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(Request.DeleteDataOwnerByUserIdRequest request)
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
                    "account",
                    "dataOwner",
                    "deleteDataOwnerByUserId",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
        public IEnumerator DeleteDataOwnerByUserId(
                Request.DeleteDataOwnerByUserIdRequest request,
                UnityAction<AsyncResult<Result.DeleteDataOwnerByUserIdResult>> callback
        ) =>
            new DeleteDataOwnerByUserIdTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToCoroutine(callback);

        public IFuture<Result.DeleteDataOwnerByUserIdResult> DeleteDataOwnerByUserIdFuture(
                Request.DeleteDataOwnerByUserIdRequest request
        ) =>
            new DeleteDataOwnerByUserIdTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
        public UniTask<Result.DeleteDataOwnerByUserIdResult> DeleteDataOwnerByUserIdAsync(
            Request.DeleteDataOwnerByUserIdRequest request
        ) =>
            new DeleteDataOwnerByUserIdTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsUniTask<Result.DeleteDataOwnerByUserIdResult>();
    #else
        public DeleteDataOwnerByUserIdTask DeleteDataOwnerByUserIdAsync(
                Request.DeleteDataOwnerByUserIdRequest request
        )
        {
            return new DeleteDataOwnerByUserIdTask(
                Gs2WebSocketSession,
                request
            );
        }
    #endif
#else
        public Task<Result.DeleteDataOwnerByUserIdResult> DeleteDataOwnerByUserIdAsync(
            Request.DeleteDataOwnerByUserIdRequest request
        ) =>
            new DeleteDataOwnerByUserIdTask(
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
                    "account",
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
	}
}