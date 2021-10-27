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


        private class CreateNamespaceTask : Gs2WebSocketSessionTask<Request.CreateNamespaceRequest, Result.CreateNamespaceResult>
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
                if (request.RequestId != null)
                {
                    jsonWriter.WritePropertyName("xGs2RequestId");
                    jsonWriter.Write(request.RequestId);
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
        )
		{
			var task = new CreateNamespaceTask(
			    Gs2WebSocketSession,
			    request
            );
            yield return task;
            callback.Invoke(new AsyncResult<Result.CreateNamespaceResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.CreateNamespaceResult> CreateNamespace(
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


        private class GetNamespaceTask : Gs2WebSocketSessionTask<Request.GetNamespaceRequest, Result.GetNamespaceResult>
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
                if (request.RequestId != null)
                {
                    jsonWriter.WritePropertyName("xGs2RequestId");
                    jsonWriter.Write(request.RequestId);
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
        )
		{
			var task = new GetNamespaceTask(
			    Gs2WebSocketSession,
			    request
            );
            yield return task;
            callback.Invoke(new AsyncResult<Result.GetNamespaceResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.GetNamespaceResult> GetNamespace(
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


        private class UpdateNamespaceTask : Gs2WebSocketSessionTask<Request.UpdateNamespaceRequest, Result.UpdateNamespaceResult>
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
                if (request.RequestId != null)
                {
                    jsonWriter.WritePropertyName("xGs2RequestId");
                    jsonWriter.Write(request.RequestId);
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
        )
		{
			var task = new UpdateNamespaceTask(
			    Gs2WebSocketSession,
			    request
            );
            yield return task;
            callback.Invoke(new AsyncResult<Result.UpdateNamespaceResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.UpdateNamespaceResult> UpdateNamespace(
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


        private class DeleteNamespaceTask : Gs2WebSocketSessionTask<Request.DeleteNamespaceRequest, Result.DeleteNamespaceResult>
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
                if (request.RequestId != null)
                {
                    jsonWriter.WritePropertyName("xGs2RequestId");
                    jsonWriter.Write(request.RequestId);
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
        )
		{
			var task = new DeleteNamespaceTask(
			    Gs2WebSocketSession,
			    request
            );
            yield return task;
            callback.Invoke(new AsyncResult<Result.DeleteNamespaceResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.DeleteNamespaceResult> DeleteNamespace(
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


        private class CreateInventoryModelMasterTask : Gs2WebSocketSessionTask<Request.CreateInventoryModelMasterRequest, Result.CreateInventoryModelMasterResult>
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
                if (request.RequestId != null)
                {
                    jsonWriter.WritePropertyName("xGs2RequestId");
                    jsonWriter.Write(request.RequestId);
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
        )
		{
			var task = new CreateInventoryModelMasterTask(
			    Gs2WebSocketSession,
			    request
            );
            yield return task;
            callback.Invoke(new AsyncResult<Result.CreateInventoryModelMasterResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.CreateInventoryModelMasterResult> CreateInventoryModelMaster(
            Request.CreateInventoryModelMasterRequest request
        )
		{
		    var task = new CreateInventoryModelMasterTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
#endif


        private class GetInventoryModelMasterTask : Gs2WebSocketSessionTask<Request.GetInventoryModelMasterRequest, Result.GetInventoryModelMasterResult>
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
                if (request.RequestId != null)
                {
                    jsonWriter.WritePropertyName("xGs2RequestId");
                    jsonWriter.Write(request.RequestId);
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
        )
		{
			var task = new GetInventoryModelMasterTask(
			    Gs2WebSocketSession,
			    request
            );
            yield return task;
            callback.Invoke(new AsyncResult<Result.GetInventoryModelMasterResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.GetInventoryModelMasterResult> GetInventoryModelMaster(
            Request.GetInventoryModelMasterRequest request
        )
		{
		    var task = new GetInventoryModelMasterTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
#endif


        private class UpdateInventoryModelMasterTask : Gs2WebSocketSessionTask<Request.UpdateInventoryModelMasterRequest, Result.UpdateInventoryModelMasterResult>
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
                if (request.RequestId != null)
                {
                    jsonWriter.WritePropertyName("xGs2RequestId");
                    jsonWriter.Write(request.RequestId);
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
        )
		{
			var task = new UpdateInventoryModelMasterTask(
			    Gs2WebSocketSession,
			    request
            );
            yield return task;
            callback.Invoke(new AsyncResult<Result.UpdateInventoryModelMasterResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.UpdateInventoryModelMasterResult> UpdateInventoryModelMaster(
            Request.UpdateInventoryModelMasterRequest request
        )
		{
		    var task = new UpdateInventoryModelMasterTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
#endif


        private class DeleteInventoryModelMasterTask : Gs2WebSocketSessionTask<Request.DeleteInventoryModelMasterRequest, Result.DeleteInventoryModelMasterResult>
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
                if (request.RequestId != null)
                {
                    jsonWriter.WritePropertyName("xGs2RequestId");
                    jsonWriter.Write(request.RequestId);
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
        )
		{
			var task = new DeleteInventoryModelMasterTask(
			    Gs2WebSocketSession,
			    request
            );
            yield return task;
            callback.Invoke(new AsyncResult<Result.DeleteInventoryModelMasterResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.DeleteInventoryModelMasterResult> DeleteInventoryModelMaster(
            Request.DeleteInventoryModelMasterRequest request
        )
		{
		    var task = new DeleteInventoryModelMasterTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
#endif


        private class GetInventoryModelTask : Gs2WebSocketSessionTask<Request.GetInventoryModelRequest, Result.GetInventoryModelResult>
        {
	        public GetInventoryModelTask(IGs2Session session, Request.GetInventoryModelRequest request) : base(session, request)
	        {
	        }

            protected override IGs2SessionRequest CreateRequest(Request.GetInventoryModelRequest request)
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
                if (request.RequestId != null)
                {
                    jsonWriter.WritePropertyName("xGs2RequestId");
                    jsonWriter.Write(request.RequestId);
                }

                AddHeader(
                    Session.Credential,
                    "inventory",
                    "inventoryModel",
                    "getInventoryModel",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator GetInventoryModel(
                Request.GetInventoryModelRequest request,
                UnityAction<AsyncResult<Result.GetInventoryModelResult>> callback
        )
		{
			var task = new GetInventoryModelTask(
			    Gs2WebSocketSession,
			    request
            );
            yield return task;
            callback.Invoke(new AsyncResult<Result.GetInventoryModelResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.GetInventoryModelResult> GetInventoryModel(
            Request.GetInventoryModelRequest request
        )
		{
		    var task = new GetInventoryModelTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
#endif


        private class CreateItemModelMasterTask : Gs2WebSocketSessionTask<Request.CreateItemModelMasterRequest, Result.CreateItemModelMasterResult>
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
                if (request.RequestId != null)
                {
                    jsonWriter.WritePropertyName("xGs2RequestId");
                    jsonWriter.Write(request.RequestId);
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
        )
		{
			var task = new CreateItemModelMasterTask(
			    Gs2WebSocketSession,
			    request
            );
            yield return task;
            callback.Invoke(new AsyncResult<Result.CreateItemModelMasterResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.CreateItemModelMasterResult> CreateItemModelMaster(
            Request.CreateItemModelMasterRequest request
        )
		{
		    var task = new CreateItemModelMasterTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
#endif


        private class GetItemModelMasterTask : Gs2WebSocketSessionTask<Request.GetItemModelMasterRequest, Result.GetItemModelMasterResult>
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
                if (request.RequestId != null)
                {
                    jsonWriter.WritePropertyName("xGs2RequestId");
                    jsonWriter.Write(request.RequestId);
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
        )
		{
			var task = new GetItemModelMasterTask(
			    Gs2WebSocketSession,
			    request
            );
            yield return task;
            callback.Invoke(new AsyncResult<Result.GetItemModelMasterResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.GetItemModelMasterResult> GetItemModelMaster(
            Request.GetItemModelMasterRequest request
        )
		{
		    var task = new GetItemModelMasterTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
#endif


        private class UpdateItemModelMasterTask : Gs2WebSocketSessionTask<Request.UpdateItemModelMasterRequest, Result.UpdateItemModelMasterResult>
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
                if (request.RequestId != null)
                {
                    jsonWriter.WritePropertyName("xGs2RequestId");
                    jsonWriter.Write(request.RequestId);
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
        )
		{
			var task = new UpdateItemModelMasterTask(
			    Gs2WebSocketSession,
			    request
            );
            yield return task;
            callback.Invoke(new AsyncResult<Result.UpdateItemModelMasterResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.UpdateItemModelMasterResult> UpdateItemModelMaster(
            Request.UpdateItemModelMasterRequest request
        )
		{
		    var task = new UpdateItemModelMasterTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
#endif


        private class DeleteItemModelMasterTask : Gs2WebSocketSessionTask<Request.DeleteItemModelMasterRequest, Result.DeleteItemModelMasterResult>
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
                if (request.RequestId != null)
                {
                    jsonWriter.WritePropertyName("xGs2RequestId");
                    jsonWriter.Write(request.RequestId);
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
        )
		{
			var task = new DeleteItemModelMasterTask(
			    Gs2WebSocketSession,
			    request
            );
            yield return task;
            callback.Invoke(new AsyncResult<Result.DeleteItemModelMasterResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.DeleteItemModelMasterResult> DeleteItemModelMaster(
            Request.DeleteItemModelMasterRequest request
        )
		{
		    var task = new DeleteItemModelMasterTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
#endif


        private class GetItemModelTask : Gs2WebSocketSessionTask<Request.GetItemModelRequest, Result.GetItemModelResult>
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
                if (request.RequestId != null)
                {
                    jsonWriter.WritePropertyName("xGs2RequestId");
                    jsonWriter.Write(request.RequestId);
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
        )
		{
			var task = new GetItemModelTask(
			    Gs2WebSocketSession,
			    request
            );
            yield return task;
            callback.Invoke(new AsyncResult<Result.GetItemModelResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.GetItemModelResult> GetItemModel(
            Request.GetItemModelRequest request
        )
		{
		    var task = new GetItemModelTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
#endif


        private class GetInventoryTask : Gs2WebSocketSessionTask<Request.GetInventoryRequest, Result.GetInventoryResult>
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
                if (request.RequestId != null)
                {
                    jsonWriter.WritePropertyName("xGs2RequestId");
                    jsonWriter.Write(request.RequestId);
                }
                if (request.AccessToken != null)
                {
                    jsonWriter.WritePropertyName("xGs2AccessToken");
                    jsonWriter.Write(request.AccessToken);
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
        )
		{
			var task = new GetInventoryTask(
			    Gs2WebSocketSession,
			    request
            );
            yield return task;
            callback.Invoke(new AsyncResult<Result.GetInventoryResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.GetInventoryResult> GetInventory(
            Request.GetInventoryRequest request
        )
		{
		    var task = new GetInventoryTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
#endif


        private class GetInventoryByUserIdTask : Gs2WebSocketSessionTask<Request.GetInventoryByUserIdRequest, Result.GetInventoryByUserIdResult>
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
                if (request.ContextStack != null)
                {
                    jsonWriter.WritePropertyName("contextStack");
                    jsonWriter.Write(request.ContextStack.ToString());
                }
                if (request.RequestId != null)
                {
                    jsonWriter.WritePropertyName("xGs2RequestId");
                    jsonWriter.Write(request.RequestId);
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
        )
		{
			var task = new GetInventoryByUserIdTask(
			    Gs2WebSocketSession,
			    request
            );
            yield return task;
            callback.Invoke(new AsyncResult<Result.GetInventoryByUserIdResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.GetInventoryByUserIdResult> GetInventoryByUserId(
            Request.GetInventoryByUserIdRequest request
        )
		{
		    var task = new GetInventoryByUserIdTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
#endif


        private class AddCapacityByUserIdTask : Gs2WebSocketSessionTask<Request.AddCapacityByUserIdRequest, Result.AddCapacityByUserIdResult>
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
                if (request.ContextStack != null)
                {
                    jsonWriter.WritePropertyName("contextStack");
                    jsonWriter.Write(request.ContextStack.ToString());
                }
                if (request.RequestId != null)
                {
                    jsonWriter.WritePropertyName("xGs2RequestId");
                    jsonWriter.Write(request.RequestId);
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
        )
		{
			var task = new AddCapacityByUserIdTask(
			    Gs2WebSocketSession,
			    request
            );
            yield return task;
            callback.Invoke(new AsyncResult<Result.AddCapacityByUserIdResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.AddCapacityByUserIdResult> AddCapacityByUserId(
            Request.AddCapacityByUserIdRequest request
        )
		{
		    var task = new AddCapacityByUserIdTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
#endif


        private class SetCapacityByUserIdTask : Gs2WebSocketSessionTask<Request.SetCapacityByUserIdRequest, Result.SetCapacityByUserIdResult>
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
                if (request.ContextStack != null)
                {
                    jsonWriter.WritePropertyName("contextStack");
                    jsonWriter.Write(request.ContextStack.ToString());
                }
                if (request.RequestId != null)
                {
                    jsonWriter.WritePropertyName("xGs2RequestId");
                    jsonWriter.Write(request.RequestId);
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
        )
		{
			var task = new SetCapacityByUserIdTask(
			    Gs2WebSocketSession,
			    request
            );
            yield return task;
            callback.Invoke(new AsyncResult<Result.SetCapacityByUserIdResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.SetCapacityByUserIdResult> SetCapacityByUserId(
            Request.SetCapacityByUserIdRequest request
        )
		{
		    var task = new SetCapacityByUserIdTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
#endif


        private class DeleteInventoryByUserIdTask : Gs2WebSocketSessionTask<Request.DeleteInventoryByUserIdRequest, Result.DeleteInventoryByUserIdResult>
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
                if (request.ContextStack != null)
                {
                    jsonWriter.WritePropertyName("contextStack");
                    jsonWriter.Write(request.ContextStack.ToString());
                }
                if (request.RequestId != null)
                {
                    jsonWriter.WritePropertyName("xGs2RequestId");
                    jsonWriter.Write(request.RequestId);
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
        )
		{
			var task = new DeleteInventoryByUserIdTask(
			    Gs2WebSocketSession,
			    request
            );
            yield return task;
            callback.Invoke(new AsyncResult<Result.DeleteInventoryByUserIdResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.DeleteInventoryByUserIdResult> DeleteInventoryByUserId(
            Request.DeleteInventoryByUserIdRequest request
        )
		{
		    var task = new DeleteInventoryByUserIdTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
#endif


        private class AddCapacityByStampSheetTask : Gs2WebSocketSessionTask<Request.AddCapacityByStampSheetRequest, Result.AddCapacityByStampSheetResult>
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
                if (request.RequestId != null)
                {
                    jsonWriter.WritePropertyName("xGs2RequestId");
                    jsonWriter.Write(request.RequestId);
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
        )
		{
			var task = new AddCapacityByStampSheetTask(
			    Gs2WebSocketSession,
			    request
            );
            yield return task;
            callback.Invoke(new AsyncResult<Result.AddCapacityByStampSheetResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.AddCapacityByStampSheetResult> AddCapacityByStampSheet(
            Request.AddCapacityByStampSheetRequest request
        )
		{
		    var task = new AddCapacityByStampSheetTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
#endif


        private class SetCapacityByStampSheetTask : Gs2WebSocketSessionTask<Request.SetCapacityByStampSheetRequest, Result.SetCapacityByStampSheetResult>
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
                if (request.RequestId != null)
                {
                    jsonWriter.WritePropertyName("xGs2RequestId");
                    jsonWriter.Write(request.RequestId);
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
        )
		{
			var task = new SetCapacityByStampSheetTask(
			    Gs2WebSocketSession,
			    request
            );
            yield return task;
            callback.Invoke(new AsyncResult<Result.SetCapacityByStampSheetResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.SetCapacityByStampSheetResult> SetCapacityByStampSheet(
            Request.SetCapacityByStampSheetRequest request
        )
		{
		    var task = new SetCapacityByStampSheetTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
#endif
	}
}