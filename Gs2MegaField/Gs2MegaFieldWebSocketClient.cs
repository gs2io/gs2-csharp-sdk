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

namespace Gs2.Gs2MegaField
{
	public class Gs2MegaFieldWebSocketClient : AbstractGs2Client
	{

		public static string Endpoint = "mega-field";

        protected Gs2WebSocketSession Gs2WebSocketSession => (Gs2WebSocketSession) Gs2Session;

		public Gs2MegaFieldWebSocketClient(Gs2WebSocketSession Gs2WebSocketSession) : base(Gs2WebSocketSession)
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
                    "megaField",
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
                    "megaField",
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
                    "megaField",
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
                    "megaField",
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
                    "megaField",
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
                    "megaField",
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


        public class CreateAreaModelMasterTask : Gs2WebSocketSessionTask<Request.CreateAreaModelMasterRequest, Result.CreateAreaModelMasterResult>
        {
            public CreateAreaModelMasterTask(IGs2Session session, Request.CreateAreaModelMasterRequest request) : base(session, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(Request.CreateAreaModelMasterRequest request)
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
                    "megaField",
                    "areaModelMaster",
                    "createAreaModelMaster",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
        public IEnumerator CreateAreaModelMaster(
                Request.CreateAreaModelMasterRequest request,
                UnityAction<AsyncResult<Result.CreateAreaModelMasterResult>> callback
        ) =>
            new CreateAreaModelMasterTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToCoroutine(callback);

        public IFuture<Result.CreateAreaModelMasterResult> CreateAreaModelMasterFuture(
                Request.CreateAreaModelMasterRequest request
        ) =>
            new CreateAreaModelMasterTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
        public UniTask<Result.CreateAreaModelMasterResult> CreateAreaModelMasterAsync(
            Request.CreateAreaModelMasterRequest request
        ) =>
            new CreateAreaModelMasterTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsUniTask<Result.CreateAreaModelMasterResult>();
    #else
        public CreateAreaModelMasterTask CreateAreaModelMasterAsync(
                Request.CreateAreaModelMasterRequest request
        )
        {
            return new CreateAreaModelMasterTask(
                Gs2WebSocketSession,
                request
            );
        }
    #endif
#else
        public Task<Result.CreateAreaModelMasterResult> CreateAreaModelMasterAsync(
            Request.CreateAreaModelMasterRequest request
        ) =>
            new CreateAreaModelMasterTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsTask();
#endif


        public class GetAreaModelMasterTask : Gs2WebSocketSessionTask<Request.GetAreaModelMasterRequest, Result.GetAreaModelMasterResult>
        {
            public GetAreaModelMasterTask(IGs2Session session, Request.GetAreaModelMasterRequest request) : base(session, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(Request.GetAreaModelMasterRequest request)
            {
                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);

                jsonWriter.WriteObjectStart();

                if (request.NamespaceName != null)
                {
                    jsonWriter.WritePropertyName("namespaceName");
                    jsonWriter.Write(request.NamespaceName.ToString());
                }
                if (request.AreaModelName != null)
                {
                    jsonWriter.WritePropertyName("areaModelName");
                    jsonWriter.Write(request.AreaModelName.ToString());
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
                    "megaField",
                    "areaModelMaster",
                    "getAreaModelMaster",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
        public IEnumerator GetAreaModelMaster(
                Request.GetAreaModelMasterRequest request,
                UnityAction<AsyncResult<Result.GetAreaModelMasterResult>> callback
        ) =>
            new GetAreaModelMasterTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToCoroutine(callback);

        public IFuture<Result.GetAreaModelMasterResult> GetAreaModelMasterFuture(
                Request.GetAreaModelMasterRequest request
        ) =>
            new GetAreaModelMasterTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
        public UniTask<Result.GetAreaModelMasterResult> GetAreaModelMasterAsync(
            Request.GetAreaModelMasterRequest request
        ) =>
            new GetAreaModelMasterTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsUniTask<Result.GetAreaModelMasterResult>();
    #else
        public GetAreaModelMasterTask GetAreaModelMasterAsync(
                Request.GetAreaModelMasterRequest request
        )
        {
            return new GetAreaModelMasterTask(
                Gs2WebSocketSession,
                request
            );
        }
    #endif
#else
        public Task<Result.GetAreaModelMasterResult> GetAreaModelMasterAsync(
            Request.GetAreaModelMasterRequest request
        ) =>
            new GetAreaModelMasterTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsTask();
#endif


        public class UpdateAreaModelMasterTask : Gs2WebSocketSessionTask<Request.UpdateAreaModelMasterRequest, Result.UpdateAreaModelMasterResult>
        {
            public UpdateAreaModelMasterTask(IGs2Session session, Request.UpdateAreaModelMasterRequest request) : base(session, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(Request.UpdateAreaModelMasterRequest request)
            {
                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);

                jsonWriter.WriteObjectStart();

                if (request.NamespaceName != null)
                {
                    jsonWriter.WritePropertyName("namespaceName");
                    jsonWriter.Write(request.NamespaceName.ToString());
                }
                if (request.AreaModelName != null)
                {
                    jsonWriter.WritePropertyName("areaModelName");
                    jsonWriter.Write(request.AreaModelName.ToString());
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
                    "megaField",
                    "areaModelMaster",
                    "updateAreaModelMaster",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
        public IEnumerator UpdateAreaModelMaster(
                Request.UpdateAreaModelMasterRequest request,
                UnityAction<AsyncResult<Result.UpdateAreaModelMasterResult>> callback
        ) =>
            new UpdateAreaModelMasterTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToCoroutine(callback);

        public IFuture<Result.UpdateAreaModelMasterResult> UpdateAreaModelMasterFuture(
                Request.UpdateAreaModelMasterRequest request
        ) =>
            new UpdateAreaModelMasterTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
        public UniTask<Result.UpdateAreaModelMasterResult> UpdateAreaModelMasterAsync(
            Request.UpdateAreaModelMasterRequest request
        ) =>
            new UpdateAreaModelMasterTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsUniTask<Result.UpdateAreaModelMasterResult>();
    #else
        public UpdateAreaModelMasterTask UpdateAreaModelMasterAsync(
                Request.UpdateAreaModelMasterRequest request
        )
        {
            return new UpdateAreaModelMasterTask(
                Gs2WebSocketSession,
                request
            );
        }
    #endif
#else
        public Task<Result.UpdateAreaModelMasterResult> UpdateAreaModelMasterAsync(
            Request.UpdateAreaModelMasterRequest request
        ) =>
            new UpdateAreaModelMasterTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsTask();
#endif


        public class DeleteAreaModelMasterTask : Gs2WebSocketSessionTask<Request.DeleteAreaModelMasterRequest, Result.DeleteAreaModelMasterResult>
        {
            public DeleteAreaModelMasterTask(IGs2Session session, Request.DeleteAreaModelMasterRequest request) : base(session, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(Request.DeleteAreaModelMasterRequest request)
            {
                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);

                jsonWriter.WriteObjectStart();

                if (request.NamespaceName != null)
                {
                    jsonWriter.WritePropertyName("namespaceName");
                    jsonWriter.Write(request.NamespaceName.ToString());
                }
                if (request.AreaModelName != null)
                {
                    jsonWriter.WritePropertyName("areaModelName");
                    jsonWriter.Write(request.AreaModelName.ToString());
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
                    "megaField",
                    "areaModelMaster",
                    "deleteAreaModelMaster",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
        public IEnumerator DeleteAreaModelMaster(
                Request.DeleteAreaModelMasterRequest request,
                UnityAction<AsyncResult<Result.DeleteAreaModelMasterResult>> callback
        ) =>
            new DeleteAreaModelMasterTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToCoroutine(callback);

        public IFuture<Result.DeleteAreaModelMasterResult> DeleteAreaModelMasterFuture(
                Request.DeleteAreaModelMasterRequest request
        ) =>
            new DeleteAreaModelMasterTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
        public UniTask<Result.DeleteAreaModelMasterResult> DeleteAreaModelMasterAsync(
            Request.DeleteAreaModelMasterRequest request
        ) =>
            new DeleteAreaModelMasterTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsUniTask<Result.DeleteAreaModelMasterResult>();
    #else
        public DeleteAreaModelMasterTask DeleteAreaModelMasterAsync(
                Request.DeleteAreaModelMasterRequest request
        )
        {
            return new DeleteAreaModelMasterTask(
                Gs2WebSocketSession,
                request
            );
        }
    #endif
#else
        public Task<Result.DeleteAreaModelMasterResult> DeleteAreaModelMasterAsync(
            Request.DeleteAreaModelMasterRequest request
        ) =>
            new DeleteAreaModelMasterTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsTask();
#endif


        public class GetLayerModelTask : Gs2WebSocketSessionTask<Request.GetLayerModelRequest, Result.GetLayerModelResult>
        {
            public GetLayerModelTask(IGs2Session session, Request.GetLayerModelRequest request) : base(session, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(Request.GetLayerModelRequest request)
            {
                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);

                jsonWriter.WriteObjectStart();

                if (request.NamespaceName != null)
                {
                    jsonWriter.WritePropertyName("namespaceName");
                    jsonWriter.Write(request.NamespaceName.ToString());
                }
                if (request.AreaModelName != null)
                {
                    jsonWriter.WritePropertyName("areaModelName");
                    jsonWriter.Write(request.AreaModelName.ToString());
                }
                if (request.LayerModelName != null)
                {
                    jsonWriter.WritePropertyName("layerModelName");
                    jsonWriter.Write(request.LayerModelName.ToString());
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
                    "megaField",
                    "layerModel",
                    "getLayerModel",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
        public IEnumerator GetLayerModel(
                Request.GetLayerModelRequest request,
                UnityAction<AsyncResult<Result.GetLayerModelResult>> callback
        ) =>
            new GetLayerModelTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToCoroutine(callback);

        public IFuture<Result.GetLayerModelResult> GetLayerModelFuture(
                Request.GetLayerModelRequest request
        ) =>
            new GetLayerModelTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
        public UniTask<Result.GetLayerModelResult> GetLayerModelAsync(
            Request.GetLayerModelRequest request
        ) =>
            new GetLayerModelTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsUniTask<Result.GetLayerModelResult>();
    #else
        public GetLayerModelTask GetLayerModelAsync(
                Request.GetLayerModelRequest request
        )
        {
            return new GetLayerModelTask(
                Gs2WebSocketSession,
                request
            );
        }
    #endif
#else
        public Task<Result.GetLayerModelResult> GetLayerModelAsync(
            Request.GetLayerModelRequest request
        ) =>
            new GetLayerModelTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsTask();
#endif


        public class CreateLayerModelMasterTask : Gs2WebSocketSessionTask<Request.CreateLayerModelMasterRequest, Result.CreateLayerModelMasterResult>
        {
            public CreateLayerModelMasterTask(IGs2Session session, Request.CreateLayerModelMasterRequest request) : base(session, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(Request.CreateLayerModelMasterRequest request)
            {
                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);

                jsonWriter.WriteObjectStart();

                if (request.NamespaceName != null)
                {
                    jsonWriter.WritePropertyName("namespaceName");
                    jsonWriter.Write(request.NamespaceName.ToString());
                }
                if (request.AreaModelName != null)
                {
                    jsonWriter.WritePropertyName("areaModelName");
                    jsonWriter.Write(request.AreaModelName.ToString());
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
                    "megaField",
                    "layerModelMaster",
                    "createLayerModelMaster",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
        public IEnumerator CreateLayerModelMaster(
                Request.CreateLayerModelMasterRequest request,
                UnityAction<AsyncResult<Result.CreateLayerModelMasterResult>> callback
        ) =>
            new CreateLayerModelMasterTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToCoroutine(callback);

        public IFuture<Result.CreateLayerModelMasterResult> CreateLayerModelMasterFuture(
                Request.CreateLayerModelMasterRequest request
        ) =>
            new CreateLayerModelMasterTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
        public UniTask<Result.CreateLayerModelMasterResult> CreateLayerModelMasterAsync(
            Request.CreateLayerModelMasterRequest request
        ) =>
            new CreateLayerModelMasterTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsUniTask<Result.CreateLayerModelMasterResult>();
    #else
        public CreateLayerModelMasterTask CreateLayerModelMasterAsync(
                Request.CreateLayerModelMasterRequest request
        )
        {
            return new CreateLayerModelMasterTask(
                Gs2WebSocketSession,
                request
            );
        }
    #endif
#else
        public Task<Result.CreateLayerModelMasterResult> CreateLayerModelMasterAsync(
            Request.CreateLayerModelMasterRequest request
        ) =>
            new CreateLayerModelMasterTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsTask();
#endif


        public class GetLayerModelMasterTask : Gs2WebSocketSessionTask<Request.GetLayerModelMasterRequest, Result.GetLayerModelMasterResult>
        {
            public GetLayerModelMasterTask(IGs2Session session, Request.GetLayerModelMasterRequest request) : base(session, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(Request.GetLayerModelMasterRequest request)
            {
                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);

                jsonWriter.WriteObjectStart();

                if (request.NamespaceName != null)
                {
                    jsonWriter.WritePropertyName("namespaceName");
                    jsonWriter.Write(request.NamespaceName.ToString());
                }
                if (request.AreaModelName != null)
                {
                    jsonWriter.WritePropertyName("areaModelName");
                    jsonWriter.Write(request.AreaModelName.ToString());
                }
                if (request.LayerModelName != null)
                {
                    jsonWriter.WritePropertyName("layerModelName");
                    jsonWriter.Write(request.LayerModelName.ToString());
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
                    "megaField",
                    "layerModelMaster",
                    "getLayerModelMaster",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
        public IEnumerator GetLayerModelMaster(
                Request.GetLayerModelMasterRequest request,
                UnityAction<AsyncResult<Result.GetLayerModelMasterResult>> callback
        ) =>
            new GetLayerModelMasterTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToCoroutine(callback);

        public IFuture<Result.GetLayerModelMasterResult> GetLayerModelMasterFuture(
                Request.GetLayerModelMasterRequest request
        ) =>
            new GetLayerModelMasterTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
        public UniTask<Result.GetLayerModelMasterResult> GetLayerModelMasterAsync(
            Request.GetLayerModelMasterRequest request
        ) =>
            new GetLayerModelMasterTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsUniTask<Result.GetLayerModelMasterResult>();
    #else
        public GetLayerModelMasterTask GetLayerModelMasterAsync(
                Request.GetLayerModelMasterRequest request
        )
        {
            return new GetLayerModelMasterTask(
                Gs2WebSocketSession,
                request
            );
        }
    #endif
#else
        public Task<Result.GetLayerModelMasterResult> GetLayerModelMasterAsync(
            Request.GetLayerModelMasterRequest request
        ) =>
            new GetLayerModelMasterTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsTask();
#endif


        public class UpdateLayerModelMasterTask : Gs2WebSocketSessionTask<Request.UpdateLayerModelMasterRequest, Result.UpdateLayerModelMasterResult>
        {
            public UpdateLayerModelMasterTask(IGs2Session session, Request.UpdateLayerModelMasterRequest request) : base(session, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(Request.UpdateLayerModelMasterRequest request)
            {
                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);

                jsonWriter.WriteObjectStart();

                if (request.NamespaceName != null)
                {
                    jsonWriter.WritePropertyName("namespaceName");
                    jsonWriter.Write(request.NamespaceName.ToString());
                }
                if (request.AreaModelName != null)
                {
                    jsonWriter.WritePropertyName("areaModelName");
                    jsonWriter.Write(request.AreaModelName.ToString());
                }
                if (request.LayerModelName != null)
                {
                    jsonWriter.WritePropertyName("layerModelName");
                    jsonWriter.Write(request.LayerModelName.ToString());
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
                    "megaField",
                    "layerModelMaster",
                    "updateLayerModelMaster",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
        public IEnumerator UpdateLayerModelMaster(
                Request.UpdateLayerModelMasterRequest request,
                UnityAction<AsyncResult<Result.UpdateLayerModelMasterResult>> callback
        ) =>
            new UpdateLayerModelMasterTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToCoroutine(callback);

        public IFuture<Result.UpdateLayerModelMasterResult> UpdateLayerModelMasterFuture(
                Request.UpdateLayerModelMasterRequest request
        ) =>
            new UpdateLayerModelMasterTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
        public UniTask<Result.UpdateLayerModelMasterResult> UpdateLayerModelMasterAsync(
            Request.UpdateLayerModelMasterRequest request
        ) =>
            new UpdateLayerModelMasterTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsUniTask<Result.UpdateLayerModelMasterResult>();
    #else
        public UpdateLayerModelMasterTask UpdateLayerModelMasterAsync(
                Request.UpdateLayerModelMasterRequest request
        )
        {
            return new UpdateLayerModelMasterTask(
                Gs2WebSocketSession,
                request
            );
        }
    #endif
#else
        public Task<Result.UpdateLayerModelMasterResult> UpdateLayerModelMasterAsync(
            Request.UpdateLayerModelMasterRequest request
        ) =>
            new UpdateLayerModelMasterTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsTask();
#endif


        public class DeleteLayerModelMasterTask : Gs2WebSocketSessionTask<Request.DeleteLayerModelMasterRequest, Result.DeleteLayerModelMasterResult>
        {
            public DeleteLayerModelMasterTask(IGs2Session session, Request.DeleteLayerModelMasterRequest request) : base(session, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(Request.DeleteLayerModelMasterRequest request)
            {
                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);

                jsonWriter.WriteObjectStart();

                if (request.NamespaceName != null)
                {
                    jsonWriter.WritePropertyName("namespaceName");
                    jsonWriter.Write(request.NamespaceName.ToString());
                }
                if (request.AreaModelName != null)
                {
                    jsonWriter.WritePropertyName("areaModelName");
                    jsonWriter.Write(request.AreaModelName.ToString());
                }
                if (request.LayerModelName != null)
                {
                    jsonWriter.WritePropertyName("layerModelName");
                    jsonWriter.Write(request.LayerModelName.ToString());
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
                    "megaField",
                    "layerModelMaster",
                    "deleteLayerModelMaster",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
        public IEnumerator DeleteLayerModelMaster(
                Request.DeleteLayerModelMasterRequest request,
                UnityAction<AsyncResult<Result.DeleteLayerModelMasterResult>> callback
        ) =>
            new DeleteLayerModelMasterTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToCoroutine(callback);

        public IFuture<Result.DeleteLayerModelMasterResult> DeleteLayerModelMasterFuture(
                Request.DeleteLayerModelMasterRequest request
        ) =>
            new DeleteLayerModelMasterTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
        public UniTask<Result.DeleteLayerModelMasterResult> DeleteLayerModelMasterAsync(
            Request.DeleteLayerModelMasterRequest request
        ) =>
            new DeleteLayerModelMasterTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsUniTask<Result.DeleteLayerModelMasterResult>();
    #else
        public DeleteLayerModelMasterTask DeleteLayerModelMasterAsync(
                Request.DeleteLayerModelMasterRequest request
        )
        {
            return new DeleteLayerModelMasterTask(
                Gs2WebSocketSession,
                request
            );
        }
    #endif
#else
        public Task<Result.DeleteLayerModelMasterResult> DeleteLayerModelMasterAsync(
            Request.DeleteLayerModelMasterRequest request
        ) =>
            new DeleteLayerModelMasterTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsTask();
#endif


        public class PreUpdateCurrentFieldMasterTask : Gs2WebSocketSessionTask<Request.PreUpdateCurrentFieldMasterRequest, Result.PreUpdateCurrentFieldMasterResult>
        {
            public PreUpdateCurrentFieldMasterTask(IGs2Session session, Request.PreUpdateCurrentFieldMasterRequest request) : base(session, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(Request.PreUpdateCurrentFieldMasterRequest request)
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
                    "megaField",
                    "currentFieldMaster",
                    "preUpdateCurrentFieldMaster",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
        public IEnumerator PreUpdateCurrentFieldMaster(
                Request.PreUpdateCurrentFieldMasterRequest request,
                UnityAction<AsyncResult<Result.PreUpdateCurrentFieldMasterResult>> callback
        ) =>
            new PreUpdateCurrentFieldMasterTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToCoroutine(callback);

        public IFuture<Result.PreUpdateCurrentFieldMasterResult> PreUpdateCurrentFieldMasterFuture(
                Request.PreUpdateCurrentFieldMasterRequest request
        ) =>
            new PreUpdateCurrentFieldMasterTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
        public UniTask<Result.PreUpdateCurrentFieldMasterResult> PreUpdateCurrentFieldMasterAsync(
            Request.PreUpdateCurrentFieldMasterRequest request
        ) =>
            new PreUpdateCurrentFieldMasterTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsUniTask<Result.PreUpdateCurrentFieldMasterResult>();
    #else
        public PreUpdateCurrentFieldMasterTask PreUpdateCurrentFieldMasterAsync(
                Request.PreUpdateCurrentFieldMasterRequest request
        )
        {
            return new PreUpdateCurrentFieldMasterTask(
                Gs2WebSocketSession,
                request
            );
        }
    #endif
#else
        public Task<Result.PreUpdateCurrentFieldMasterResult> PreUpdateCurrentFieldMasterAsync(
            Request.PreUpdateCurrentFieldMasterRequest request
        ) =>
            new PreUpdateCurrentFieldMasterTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsTask();
#endif


        public class PutPositionTask : Gs2WebSocketSessionTask<Request.PutPositionRequest, Result.PutPositionResult>
        {
            public PutPositionTask(IGs2Session session, Request.PutPositionRequest request) : base(session, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(Request.PutPositionRequest request)
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
                if (request.AreaModelName != null)
                {
                    jsonWriter.WritePropertyName("areaModelName");
                    jsonWriter.Write(request.AreaModelName.ToString());
                }
                if (request.LayerModelName != null)
                {
                    jsonWriter.WritePropertyName("layerModelName");
                    jsonWriter.Write(request.LayerModelName.ToString());
                }
                if (request.Position != null)
                {
                    jsonWriter.WritePropertyName("position");
                    request.Position.WriteJson(jsonWriter);
                }
                if (request.Vector != null)
                {
                    jsonWriter.WritePropertyName("vector");
                    request.Vector.WriteJson(jsonWriter);
                }
                if (request.R != null)
                {
                    jsonWriter.WritePropertyName("r");
                    jsonWriter.Write(request.R.ToString());
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
                    "megaField",
                    "spatial",
                    "putPosition",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
        public IEnumerator PutPosition(
                Request.PutPositionRequest request,
                UnityAction<AsyncResult<Result.PutPositionResult>> callback
        ) =>
            new PutPositionTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToCoroutine(callback);

        public IFuture<Result.PutPositionResult> PutPositionFuture(
                Request.PutPositionRequest request
        ) =>
            new PutPositionTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
        public UniTask<Result.PutPositionResult> PutPositionAsync(
            Request.PutPositionRequest request
        ) =>
            new PutPositionTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsUniTask<Result.PutPositionResult>();
    #else
        public PutPositionTask PutPositionAsync(
                Request.PutPositionRequest request
        )
        {
            return new PutPositionTask(
                Gs2WebSocketSession,
                request
            );
        }
    #endif
#else
        public Task<Result.PutPositionResult> PutPositionAsync(
            Request.PutPositionRequest request
        ) =>
            new PutPositionTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsTask();
#endif


        public class PutPositionByUserIdTask : Gs2WebSocketSessionTask<Request.PutPositionByUserIdRequest, Result.PutPositionByUserIdResult>
        {
            public PutPositionByUserIdTask(IGs2Session session, Request.PutPositionByUserIdRequest request) : base(session, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(Request.PutPositionByUserIdRequest request)
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
                if (request.AreaModelName != null)
                {
                    jsonWriter.WritePropertyName("areaModelName");
                    jsonWriter.Write(request.AreaModelName.ToString());
                }
                if (request.LayerModelName != null)
                {
                    jsonWriter.WritePropertyName("layerModelName");
                    jsonWriter.Write(request.LayerModelName.ToString());
                }
                if (request.Position != null)
                {
                    jsonWriter.WritePropertyName("position");
                    request.Position.WriteJson(jsonWriter);
                }
                if (request.Vector != null)
                {
                    jsonWriter.WritePropertyName("vector");
                    request.Vector.WriteJson(jsonWriter);
                }
                if (request.R != null)
                {
                    jsonWriter.WritePropertyName("r");
                    jsonWriter.Write(request.R.ToString());
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
                    "megaField",
                    "spatial",
                    "putPositionByUserId",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
        public IEnumerator PutPositionByUserId(
                Request.PutPositionByUserIdRequest request,
                UnityAction<AsyncResult<Result.PutPositionByUserIdResult>> callback
        ) =>
            new PutPositionByUserIdTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToCoroutine(callback);

        public IFuture<Result.PutPositionByUserIdResult> PutPositionByUserIdFuture(
                Request.PutPositionByUserIdRequest request
        ) =>
            new PutPositionByUserIdTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
        public UniTask<Result.PutPositionByUserIdResult> PutPositionByUserIdAsync(
            Request.PutPositionByUserIdRequest request
        ) =>
            new PutPositionByUserIdTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsUniTask<Result.PutPositionByUserIdResult>();
    #else
        public PutPositionByUserIdTask PutPositionByUserIdAsync(
                Request.PutPositionByUserIdRequest request
        )
        {
            return new PutPositionByUserIdTask(
                Gs2WebSocketSession,
                request
            );
        }
    #endif
#else
        public Task<Result.PutPositionByUserIdResult> PutPositionByUserIdAsync(
            Request.PutPositionByUserIdRequest request
        ) =>
            new PutPositionByUserIdTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsTask();
#endif


        public class NearUserIdsTask : Gs2WebSocketSessionTask<Request.NearUserIdsRequest, Result.NearUserIdsResult>
        {
            public NearUserIdsTask(IGs2Session session, Request.NearUserIdsRequest request) : base(session, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(Request.NearUserIdsRequest request)
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
                if (request.AreaModelName != null)
                {
                    jsonWriter.WritePropertyName("areaModelName");
                    jsonWriter.Write(request.AreaModelName.ToString());
                }
                if (request.LayerModelName != null)
                {
                    jsonWriter.WritePropertyName("layerModelName");
                    jsonWriter.Write(request.LayerModelName.ToString());
                }
                if (request.Point != null)
                {
                    jsonWriter.WritePropertyName("point");
                    request.Point.WriteJson(jsonWriter);
                }
                if (request.R != null)
                {
                    jsonWriter.WritePropertyName("r");
                    jsonWriter.Write(request.R.ToString());
                }
                if (request.Limit != null)
                {
                    jsonWriter.WritePropertyName("limit");
                    jsonWriter.Write(request.Limit.ToString());
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
                    "megaField",
                    "spatial",
                    "nearUserIds",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
        public IEnumerator NearUserIds(
                Request.NearUserIdsRequest request,
                UnityAction<AsyncResult<Result.NearUserIdsResult>> callback
        ) =>
            new NearUserIdsTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToCoroutine(callback);

        public IFuture<Result.NearUserIdsResult> NearUserIdsFuture(
                Request.NearUserIdsRequest request
        ) =>
            new NearUserIdsTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
        public UniTask<Result.NearUserIdsResult> NearUserIdsAsync(
            Request.NearUserIdsRequest request
        ) =>
            new NearUserIdsTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsUniTask<Result.NearUserIdsResult>();
    #else
        public NearUserIdsTask NearUserIdsAsync(
                Request.NearUserIdsRequest request
        )
        {
            return new NearUserIdsTask(
                Gs2WebSocketSession,
                request
            );
        }
    #endif
#else
        public Task<Result.NearUserIdsResult> NearUserIdsAsync(
            Request.NearUserIdsRequest request
        ) =>
            new NearUserIdsTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsTask();
#endif


        public class NearUserIdsFromSystemTask : Gs2WebSocketSessionTask<Request.NearUserIdsFromSystemRequest, Result.NearUserIdsFromSystemResult>
        {
            public NearUserIdsFromSystemTask(IGs2Session session, Request.NearUserIdsFromSystemRequest request) : base(session, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(Request.NearUserIdsFromSystemRequest request)
            {
                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);

                jsonWriter.WriteObjectStart();

                if (request.NamespaceName != null)
                {
                    jsonWriter.WritePropertyName("namespaceName");
                    jsonWriter.Write(request.NamespaceName.ToString());
                }
                if (request.AreaModelName != null)
                {
                    jsonWriter.WritePropertyName("areaModelName");
                    jsonWriter.Write(request.AreaModelName.ToString());
                }
                if (request.LayerModelName != null)
                {
                    jsonWriter.WritePropertyName("layerModelName");
                    jsonWriter.Write(request.LayerModelName.ToString());
                }
                if (request.Point != null)
                {
                    jsonWriter.WritePropertyName("point");
                    request.Point.WriteJson(jsonWriter);
                }
                if (request.R != null)
                {
                    jsonWriter.WritePropertyName("r");
                    jsonWriter.Write(request.R.ToString());
                }
                if (request.Limit != null)
                {
                    jsonWriter.WritePropertyName("limit");
                    jsonWriter.Write(request.Limit.ToString());
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
                    "megaField",
                    "spatial",
                    "nearUserIdsFromSystem",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
        public IEnumerator NearUserIdsFromSystem(
                Request.NearUserIdsFromSystemRequest request,
                UnityAction<AsyncResult<Result.NearUserIdsFromSystemResult>> callback
        ) =>
            new NearUserIdsFromSystemTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToCoroutine(callback);

        public IFuture<Result.NearUserIdsFromSystemResult> NearUserIdsFromSystemFuture(
                Request.NearUserIdsFromSystemRequest request
        ) =>
            new NearUserIdsFromSystemTask(
                Gs2WebSocketSession,
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
        public UniTask<Result.NearUserIdsFromSystemResult> NearUserIdsFromSystemAsync(
            Request.NearUserIdsFromSystemRequest request
        ) =>
            new NearUserIdsFromSystemTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsUniTask<Result.NearUserIdsFromSystemResult>();
    #else
        public NearUserIdsFromSystemTask NearUserIdsFromSystemAsync(
                Request.NearUserIdsFromSystemRequest request
        )
        {
            return new NearUserIdsFromSystemTask(
                Gs2WebSocketSession,
                request
            );
        }
    #endif
#else
        public Task<Result.NearUserIdsFromSystemResult> NearUserIdsFromSystemAsync(
            Request.NearUserIdsFromSystemRequest request
        ) =>
            new NearUserIdsFromSystemTask(
                Gs2WebSocketSession,
                request
            ).Invoke().AsTask();
#endif
	}
}