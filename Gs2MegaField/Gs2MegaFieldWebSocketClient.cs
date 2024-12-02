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
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Gs2.Core;
using Gs2.Core.Model;
using Gs2.Core.Net;
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
        )
		{
			var task = new CreateNamespaceTask(
			    Gs2WebSocketSession,
			    request
            );
            yield return task;
            callback.Invoke(new AsyncResult<Result.CreateNamespaceResult>(task.Result, task.Error));
        }

		public IFuture<Result.CreateNamespaceResult> CreateNamespaceFuture(
                Request.CreateNamespaceRequest request
        )
		{
			return new CreateNamespaceTask(
			    Gs2WebSocketSession,
			    request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.CreateNamespaceResult> CreateNamespaceAsync(
            Request.CreateNamespaceRequest request
        )
		{
		    var task = new CreateNamespaceTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
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
		public async Task<Result.CreateNamespaceResult> CreateNamespaceAsync(
            Request.CreateNamespaceRequest request
        )
		{
		    var task = new CreateNamespaceTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
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
        )
		{
			var task = new GetNamespaceTask(
			    Gs2WebSocketSession,
			    request
            );
            yield return task;
            callback.Invoke(new AsyncResult<Result.GetNamespaceResult>(task.Result, task.Error));
        }

		public IFuture<Result.GetNamespaceResult> GetNamespaceFuture(
                Request.GetNamespaceRequest request
        )
		{
			return new GetNamespaceTask(
			    Gs2WebSocketSession,
			    request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.GetNamespaceResult> GetNamespaceAsync(
            Request.GetNamespaceRequest request
        )
		{
		    var task = new GetNamespaceTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
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
		public async Task<Result.GetNamespaceResult> GetNamespaceAsync(
            Request.GetNamespaceRequest request
        )
		{
		    var task = new GetNamespaceTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
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
        )
		{
			var task = new UpdateNamespaceTask(
			    Gs2WebSocketSession,
			    request
            );
            yield return task;
            callback.Invoke(new AsyncResult<Result.UpdateNamespaceResult>(task.Result, task.Error));
        }

		public IFuture<Result.UpdateNamespaceResult> UpdateNamespaceFuture(
                Request.UpdateNamespaceRequest request
        )
		{
			return new UpdateNamespaceTask(
			    Gs2WebSocketSession,
			    request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.UpdateNamespaceResult> UpdateNamespaceAsync(
            Request.UpdateNamespaceRequest request
        )
		{
		    var task = new UpdateNamespaceTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
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
		public async Task<Result.UpdateNamespaceResult> UpdateNamespaceAsync(
            Request.UpdateNamespaceRequest request
        )
		{
		    var task = new UpdateNamespaceTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
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
        )
		{
			var task = new DeleteNamespaceTask(
			    Gs2WebSocketSession,
			    request
            );
            yield return task;
            callback.Invoke(new AsyncResult<Result.DeleteNamespaceResult>(task.Result, task.Error));
        }

		public IFuture<Result.DeleteNamespaceResult> DeleteNamespaceFuture(
                Request.DeleteNamespaceRequest request
        )
		{
			return new DeleteNamespaceTask(
			    Gs2WebSocketSession,
			    request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.DeleteNamespaceResult> DeleteNamespaceAsync(
            Request.DeleteNamespaceRequest request
        )
		{
		    var task = new DeleteNamespaceTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
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
		public async Task<Result.DeleteNamespaceResult> DeleteNamespaceAsync(
            Request.DeleteNamespaceRequest request
        )
		{
		    var task = new DeleteNamespaceTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
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
        )
		{
			var task = new CreateAreaModelMasterTask(
			    Gs2WebSocketSession,
			    request
            );
            yield return task;
            callback.Invoke(new AsyncResult<Result.CreateAreaModelMasterResult>(task.Result, task.Error));
        }

		public IFuture<Result.CreateAreaModelMasterResult> CreateAreaModelMasterFuture(
                Request.CreateAreaModelMasterRequest request
        )
		{
			return new CreateAreaModelMasterTask(
			    Gs2WebSocketSession,
			    request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.CreateAreaModelMasterResult> CreateAreaModelMasterAsync(
            Request.CreateAreaModelMasterRequest request
        )
		{
		    var task = new CreateAreaModelMasterTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
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
		public async Task<Result.CreateAreaModelMasterResult> CreateAreaModelMasterAsync(
            Request.CreateAreaModelMasterRequest request
        )
		{
		    var task = new CreateAreaModelMasterTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
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
        )
		{
			var task = new GetAreaModelMasterTask(
			    Gs2WebSocketSession,
			    request
            );
            yield return task;
            callback.Invoke(new AsyncResult<Result.GetAreaModelMasterResult>(task.Result, task.Error));
        }

		public IFuture<Result.GetAreaModelMasterResult> GetAreaModelMasterFuture(
                Request.GetAreaModelMasterRequest request
        )
		{
			return new GetAreaModelMasterTask(
			    Gs2WebSocketSession,
			    request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.GetAreaModelMasterResult> GetAreaModelMasterAsync(
            Request.GetAreaModelMasterRequest request
        )
		{
		    var task = new GetAreaModelMasterTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
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
		public async Task<Result.GetAreaModelMasterResult> GetAreaModelMasterAsync(
            Request.GetAreaModelMasterRequest request
        )
		{
		    var task = new GetAreaModelMasterTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
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
        )
		{
			var task = new UpdateAreaModelMasterTask(
			    Gs2WebSocketSession,
			    request
            );
            yield return task;
            callback.Invoke(new AsyncResult<Result.UpdateAreaModelMasterResult>(task.Result, task.Error));
        }

		public IFuture<Result.UpdateAreaModelMasterResult> UpdateAreaModelMasterFuture(
                Request.UpdateAreaModelMasterRequest request
        )
		{
			return new UpdateAreaModelMasterTask(
			    Gs2WebSocketSession,
			    request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.UpdateAreaModelMasterResult> UpdateAreaModelMasterAsync(
            Request.UpdateAreaModelMasterRequest request
        )
		{
		    var task = new UpdateAreaModelMasterTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
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
		public async Task<Result.UpdateAreaModelMasterResult> UpdateAreaModelMasterAsync(
            Request.UpdateAreaModelMasterRequest request
        )
		{
		    var task = new UpdateAreaModelMasterTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
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
        )
		{
			var task = new DeleteAreaModelMasterTask(
			    Gs2WebSocketSession,
			    request
            );
            yield return task;
            callback.Invoke(new AsyncResult<Result.DeleteAreaModelMasterResult>(task.Result, task.Error));
        }

		public IFuture<Result.DeleteAreaModelMasterResult> DeleteAreaModelMasterFuture(
                Request.DeleteAreaModelMasterRequest request
        )
		{
			return new DeleteAreaModelMasterTask(
			    Gs2WebSocketSession,
			    request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.DeleteAreaModelMasterResult> DeleteAreaModelMasterAsync(
            Request.DeleteAreaModelMasterRequest request
        )
		{
		    var task = new DeleteAreaModelMasterTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
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
		public async Task<Result.DeleteAreaModelMasterResult> DeleteAreaModelMasterAsync(
            Request.DeleteAreaModelMasterRequest request
        )
		{
		    var task = new DeleteAreaModelMasterTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
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
        )
		{
			var task = new GetLayerModelTask(
			    Gs2WebSocketSession,
			    request
            );
            yield return task;
            callback.Invoke(new AsyncResult<Result.GetLayerModelResult>(task.Result, task.Error));
        }

		public IFuture<Result.GetLayerModelResult> GetLayerModelFuture(
                Request.GetLayerModelRequest request
        )
		{
			return new GetLayerModelTask(
			    Gs2WebSocketSession,
			    request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.GetLayerModelResult> GetLayerModelAsync(
            Request.GetLayerModelRequest request
        )
		{
		    var task = new GetLayerModelTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
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
		public async Task<Result.GetLayerModelResult> GetLayerModelAsync(
            Request.GetLayerModelRequest request
        )
		{
		    var task = new GetLayerModelTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
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
        )
		{
			var task = new CreateLayerModelMasterTask(
			    Gs2WebSocketSession,
			    request
            );
            yield return task;
            callback.Invoke(new AsyncResult<Result.CreateLayerModelMasterResult>(task.Result, task.Error));
        }

		public IFuture<Result.CreateLayerModelMasterResult> CreateLayerModelMasterFuture(
                Request.CreateLayerModelMasterRequest request
        )
		{
			return new CreateLayerModelMasterTask(
			    Gs2WebSocketSession,
			    request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.CreateLayerModelMasterResult> CreateLayerModelMasterAsync(
            Request.CreateLayerModelMasterRequest request
        )
		{
		    var task = new CreateLayerModelMasterTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
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
		public async Task<Result.CreateLayerModelMasterResult> CreateLayerModelMasterAsync(
            Request.CreateLayerModelMasterRequest request
        )
		{
		    var task = new CreateLayerModelMasterTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
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
        )
		{
			var task = new GetLayerModelMasterTask(
			    Gs2WebSocketSession,
			    request
            );
            yield return task;
            callback.Invoke(new AsyncResult<Result.GetLayerModelMasterResult>(task.Result, task.Error));
        }

		public IFuture<Result.GetLayerModelMasterResult> GetLayerModelMasterFuture(
                Request.GetLayerModelMasterRequest request
        )
		{
			return new GetLayerModelMasterTask(
			    Gs2WebSocketSession,
			    request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.GetLayerModelMasterResult> GetLayerModelMasterAsync(
            Request.GetLayerModelMasterRequest request
        )
		{
		    var task = new GetLayerModelMasterTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
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
		public async Task<Result.GetLayerModelMasterResult> GetLayerModelMasterAsync(
            Request.GetLayerModelMasterRequest request
        )
		{
		    var task = new GetLayerModelMasterTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
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
        )
		{
			var task = new UpdateLayerModelMasterTask(
			    Gs2WebSocketSession,
			    request
            );
            yield return task;
            callback.Invoke(new AsyncResult<Result.UpdateLayerModelMasterResult>(task.Result, task.Error));
        }

		public IFuture<Result.UpdateLayerModelMasterResult> UpdateLayerModelMasterFuture(
                Request.UpdateLayerModelMasterRequest request
        )
		{
			return new UpdateLayerModelMasterTask(
			    Gs2WebSocketSession,
			    request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.UpdateLayerModelMasterResult> UpdateLayerModelMasterAsync(
            Request.UpdateLayerModelMasterRequest request
        )
		{
		    var task = new UpdateLayerModelMasterTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
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
		public async Task<Result.UpdateLayerModelMasterResult> UpdateLayerModelMasterAsync(
            Request.UpdateLayerModelMasterRequest request
        )
		{
		    var task = new UpdateLayerModelMasterTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
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
        )
		{
			var task = new DeleteLayerModelMasterTask(
			    Gs2WebSocketSession,
			    request
            );
            yield return task;
            callback.Invoke(new AsyncResult<Result.DeleteLayerModelMasterResult>(task.Result, task.Error));
        }

		public IFuture<Result.DeleteLayerModelMasterResult> DeleteLayerModelMasterFuture(
                Request.DeleteLayerModelMasterRequest request
        )
		{
			return new DeleteLayerModelMasterTask(
			    Gs2WebSocketSession,
			    request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.DeleteLayerModelMasterResult> DeleteLayerModelMasterAsync(
            Request.DeleteLayerModelMasterRequest request
        )
		{
		    var task = new DeleteLayerModelMasterTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
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
		public async Task<Result.DeleteLayerModelMasterResult> DeleteLayerModelMasterAsync(
            Request.DeleteLayerModelMasterRequest request
        )
		{
		    var task = new DeleteLayerModelMasterTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
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
        )
		{
			var task = new PutPositionTask(
			    Gs2WebSocketSession,
			    request
            );
            yield return task;
            callback.Invoke(new AsyncResult<Result.PutPositionResult>(task.Result, task.Error));
        }

		public IFuture<Result.PutPositionResult> PutPositionFuture(
                Request.PutPositionRequest request
        )
		{
			return new PutPositionTask(
			    Gs2WebSocketSession,
			    request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.PutPositionResult> PutPositionAsync(
            Request.PutPositionRequest request
        )
		{
		    var task = new PutPositionTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
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
		public async Task<Result.PutPositionResult> PutPositionAsync(
            Request.PutPositionRequest request
        )
		{
		    var task = new PutPositionTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
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
        )
		{
			var task = new PutPositionByUserIdTask(
			    Gs2WebSocketSession,
			    request
            );
            yield return task;
            callback.Invoke(new AsyncResult<Result.PutPositionByUserIdResult>(task.Result, task.Error));
        }

		public IFuture<Result.PutPositionByUserIdResult> PutPositionByUserIdFuture(
                Request.PutPositionByUserIdRequest request
        )
		{
			return new PutPositionByUserIdTask(
			    Gs2WebSocketSession,
			    request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.PutPositionByUserIdResult> PutPositionByUserIdAsync(
            Request.PutPositionByUserIdRequest request
        )
		{
		    var task = new PutPositionByUserIdTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
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
		public async Task<Result.PutPositionByUserIdResult> PutPositionByUserIdAsync(
            Request.PutPositionByUserIdRequest request
        )
		{
		    var task = new PutPositionByUserIdTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
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
        )
		{
			var task = new NearUserIdsTask(
			    Gs2WebSocketSession,
			    request
            );
            yield return task;
            callback.Invoke(new AsyncResult<Result.NearUserIdsResult>(task.Result, task.Error));
        }

		public IFuture<Result.NearUserIdsResult> NearUserIdsFuture(
                Request.NearUserIdsRequest request
        )
		{
			return new NearUserIdsTask(
			    Gs2WebSocketSession,
			    request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.NearUserIdsResult> NearUserIdsAsync(
            Request.NearUserIdsRequest request
        )
		{
		    var task = new NearUserIdsTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
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
		public async Task<Result.NearUserIdsResult> NearUserIdsAsync(
            Request.NearUserIdsRequest request
        )
		{
		    var task = new NearUserIdsTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
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
        )
		{
			var task = new NearUserIdsFromSystemTask(
			    Gs2WebSocketSession,
			    request
            );
            yield return task;
            callback.Invoke(new AsyncResult<Result.NearUserIdsFromSystemResult>(task.Result, task.Error));
        }

		public IFuture<Result.NearUserIdsFromSystemResult> NearUserIdsFromSystemFuture(
                Request.NearUserIdsFromSystemRequest request
        )
		{
			return new NearUserIdsFromSystemTask(
			    Gs2WebSocketSession,
			    request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.NearUserIdsFromSystemResult> NearUserIdsFromSystemAsync(
            Request.NearUserIdsFromSystemRequest request
        )
		{
		    var task = new NearUserIdsFromSystemTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
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
		public async Task<Result.NearUserIdsFromSystemResult> NearUserIdsFromSystemAsync(
            Request.NearUserIdsFromSystemRequest request
        )
		{
		    var task = new NearUserIdsFromSystemTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
#endif
	}
}