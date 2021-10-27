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

namespace Gs2.Gs2Stamina
{
	public class Gs2StaminaWebSocketClient : AbstractGs2Client
	{

		public static string Endpoint = "stamina";

        protected Gs2WebSocketSession Gs2WebSocketSession => (Gs2WebSocketSession) Gs2Session;

		public Gs2StaminaWebSocketClient(Gs2WebSocketSession Gs2WebSocketSession) : base(Gs2WebSocketSession)
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
                if (request.OverflowTriggerScript != null)
                {
                    jsonWriter.WritePropertyName("overflowTriggerScript");
                    request.OverflowTriggerScript.WriteJson(jsonWriter);
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
                    "stamina",
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
                    "stamina",
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
                if (request.OverflowTriggerScript != null)
                {
                    jsonWriter.WritePropertyName("overflowTriggerScript");
                    request.OverflowTriggerScript.WriteJson(jsonWriter);
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
                    "stamina",
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
                    "stamina",
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


        private class CreateStaminaModelMasterTask : Gs2WebSocketSessionTask<Request.CreateStaminaModelMasterRequest, Result.CreateStaminaModelMasterResult>
        {
	        public CreateStaminaModelMasterTask(IGs2Session session, Request.CreateStaminaModelMasterRequest request) : base(session, request)
	        {
	        }

            protected override IGs2SessionRequest CreateRequest(Request.CreateStaminaModelMasterRequest request)
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
                if (request.RecoverIntervalMinutes != null)
                {
                    jsonWriter.WritePropertyName("recoverIntervalMinutes");
                    jsonWriter.Write(request.RecoverIntervalMinutes.ToString());
                }
                if (request.RecoverValue != null)
                {
                    jsonWriter.WritePropertyName("recoverValue");
                    jsonWriter.Write(request.RecoverValue.ToString());
                }
                if (request.InitialCapacity != null)
                {
                    jsonWriter.WritePropertyName("initialCapacity");
                    jsonWriter.Write(request.InitialCapacity.ToString());
                }
                if (request.IsOverflow != null)
                {
                    jsonWriter.WritePropertyName("isOverflow");
                    jsonWriter.Write(request.IsOverflow.ToString());
                }
                if (request.MaxCapacity != null)
                {
                    jsonWriter.WritePropertyName("maxCapacity");
                    jsonWriter.Write(request.MaxCapacity.ToString());
                }
                if (request.MaxStaminaTableName != null)
                {
                    jsonWriter.WritePropertyName("maxStaminaTableName");
                    jsonWriter.Write(request.MaxStaminaTableName.ToString());
                }
                if (request.RecoverIntervalTableName != null)
                {
                    jsonWriter.WritePropertyName("recoverIntervalTableName");
                    jsonWriter.Write(request.RecoverIntervalTableName.ToString());
                }
                if (request.RecoverValueTableName != null)
                {
                    jsonWriter.WritePropertyName("recoverValueTableName");
                    jsonWriter.Write(request.RecoverValueTableName.ToString());
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
                    "stamina",
                    "staminaModelMaster",
                    "createStaminaModelMaster",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator CreateStaminaModelMaster(
                Request.CreateStaminaModelMasterRequest request,
                UnityAction<AsyncResult<Result.CreateStaminaModelMasterResult>> callback
        )
		{
			var task = new CreateStaminaModelMasterTask(
			    Gs2WebSocketSession,
			    request
            );
            yield return task;
            callback.Invoke(new AsyncResult<Result.CreateStaminaModelMasterResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.CreateStaminaModelMasterResult> CreateStaminaModelMaster(
            Request.CreateStaminaModelMasterRequest request
        )
		{
		    var task = new CreateStaminaModelMasterTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
#endif


        private class GetStaminaModelMasterTask : Gs2WebSocketSessionTask<Request.GetStaminaModelMasterRequest, Result.GetStaminaModelMasterResult>
        {
	        public GetStaminaModelMasterTask(IGs2Session session, Request.GetStaminaModelMasterRequest request) : base(session, request)
	        {
	        }

            protected override IGs2SessionRequest CreateRequest(Request.GetStaminaModelMasterRequest request)
            {
                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);

                jsonWriter.WriteObjectStart();

                if (request.NamespaceName != null)
                {
                    jsonWriter.WritePropertyName("namespaceName");
                    jsonWriter.Write(request.NamespaceName.ToString());
                }
                if (request.StaminaName != null)
                {
                    jsonWriter.WritePropertyName("staminaName");
                    jsonWriter.Write(request.StaminaName.ToString());
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
                    "stamina",
                    "staminaModelMaster",
                    "getStaminaModelMaster",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator GetStaminaModelMaster(
                Request.GetStaminaModelMasterRequest request,
                UnityAction<AsyncResult<Result.GetStaminaModelMasterResult>> callback
        )
		{
			var task = new GetStaminaModelMasterTask(
			    Gs2WebSocketSession,
			    request
            );
            yield return task;
            callback.Invoke(new AsyncResult<Result.GetStaminaModelMasterResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.GetStaminaModelMasterResult> GetStaminaModelMaster(
            Request.GetStaminaModelMasterRequest request
        )
		{
		    var task = new GetStaminaModelMasterTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
#endif


        private class UpdateStaminaModelMasterTask : Gs2WebSocketSessionTask<Request.UpdateStaminaModelMasterRequest, Result.UpdateStaminaModelMasterResult>
        {
	        public UpdateStaminaModelMasterTask(IGs2Session session, Request.UpdateStaminaModelMasterRequest request) : base(session, request)
	        {
	        }

            protected override IGs2SessionRequest CreateRequest(Request.UpdateStaminaModelMasterRequest request)
            {
                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);

                jsonWriter.WriteObjectStart();

                if (request.NamespaceName != null)
                {
                    jsonWriter.WritePropertyName("namespaceName");
                    jsonWriter.Write(request.NamespaceName.ToString());
                }
                if (request.StaminaName != null)
                {
                    jsonWriter.WritePropertyName("staminaName");
                    jsonWriter.Write(request.StaminaName.ToString());
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
                if (request.RecoverIntervalMinutes != null)
                {
                    jsonWriter.WritePropertyName("recoverIntervalMinutes");
                    jsonWriter.Write(request.RecoverIntervalMinutes.ToString());
                }
                if (request.RecoverValue != null)
                {
                    jsonWriter.WritePropertyName("recoverValue");
                    jsonWriter.Write(request.RecoverValue.ToString());
                }
                if (request.InitialCapacity != null)
                {
                    jsonWriter.WritePropertyName("initialCapacity");
                    jsonWriter.Write(request.InitialCapacity.ToString());
                }
                if (request.IsOverflow != null)
                {
                    jsonWriter.WritePropertyName("isOverflow");
                    jsonWriter.Write(request.IsOverflow.ToString());
                }
                if (request.MaxCapacity != null)
                {
                    jsonWriter.WritePropertyName("maxCapacity");
                    jsonWriter.Write(request.MaxCapacity.ToString());
                }
                if (request.MaxStaminaTableName != null)
                {
                    jsonWriter.WritePropertyName("maxStaminaTableName");
                    jsonWriter.Write(request.MaxStaminaTableName.ToString());
                }
                if (request.RecoverIntervalTableName != null)
                {
                    jsonWriter.WritePropertyName("recoverIntervalTableName");
                    jsonWriter.Write(request.RecoverIntervalTableName.ToString());
                }
                if (request.RecoverValueTableName != null)
                {
                    jsonWriter.WritePropertyName("recoverValueTableName");
                    jsonWriter.Write(request.RecoverValueTableName.ToString());
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
                    "stamina",
                    "staminaModelMaster",
                    "updateStaminaModelMaster",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator UpdateStaminaModelMaster(
                Request.UpdateStaminaModelMasterRequest request,
                UnityAction<AsyncResult<Result.UpdateStaminaModelMasterResult>> callback
        )
		{
			var task = new UpdateStaminaModelMasterTask(
			    Gs2WebSocketSession,
			    request
            );
            yield return task;
            callback.Invoke(new AsyncResult<Result.UpdateStaminaModelMasterResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.UpdateStaminaModelMasterResult> UpdateStaminaModelMaster(
            Request.UpdateStaminaModelMasterRequest request
        )
		{
		    var task = new UpdateStaminaModelMasterTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
#endif


        private class DeleteStaminaModelMasterTask : Gs2WebSocketSessionTask<Request.DeleteStaminaModelMasterRequest, Result.DeleteStaminaModelMasterResult>
        {
	        public DeleteStaminaModelMasterTask(IGs2Session session, Request.DeleteStaminaModelMasterRequest request) : base(session, request)
	        {
	        }

            protected override IGs2SessionRequest CreateRequest(Request.DeleteStaminaModelMasterRequest request)
            {
                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);

                jsonWriter.WriteObjectStart();

                if (request.NamespaceName != null)
                {
                    jsonWriter.WritePropertyName("namespaceName");
                    jsonWriter.Write(request.NamespaceName.ToString());
                }
                if (request.StaminaName != null)
                {
                    jsonWriter.WritePropertyName("staminaName");
                    jsonWriter.Write(request.StaminaName.ToString());
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
                    "stamina",
                    "staminaModelMaster",
                    "deleteStaminaModelMaster",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator DeleteStaminaModelMaster(
                Request.DeleteStaminaModelMasterRequest request,
                UnityAction<AsyncResult<Result.DeleteStaminaModelMasterResult>> callback
        )
		{
			var task = new DeleteStaminaModelMasterTask(
			    Gs2WebSocketSession,
			    request
            );
            yield return task;
            callback.Invoke(new AsyncResult<Result.DeleteStaminaModelMasterResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.DeleteStaminaModelMasterResult> DeleteStaminaModelMaster(
            Request.DeleteStaminaModelMasterRequest request
        )
		{
		    var task = new DeleteStaminaModelMasterTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
#endif


        private class CreateMaxStaminaTableMasterTask : Gs2WebSocketSessionTask<Request.CreateMaxStaminaTableMasterRequest, Result.CreateMaxStaminaTableMasterResult>
        {
	        public CreateMaxStaminaTableMasterTask(IGs2Session session, Request.CreateMaxStaminaTableMasterRequest request) : base(session, request)
	        {
	        }

            protected override IGs2SessionRequest CreateRequest(Request.CreateMaxStaminaTableMasterRequest request)
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
                if (request.ExperienceModelId != null)
                {
                    jsonWriter.WritePropertyName("experienceModelId");
                    jsonWriter.Write(request.ExperienceModelId.ToString());
                }
                if (request.Values != null)
                {
                    jsonWriter.WritePropertyName("values");
                    jsonWriter.WriteArrayStart();
                    foreach(var item in request.Values)
                    {
                        jsonWriter.Write(item);
                    }
                    jsonWriter.WriteArrayEnd();
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
                    "stamina",
                    "maxStaminaTableMaster",
                    "createMaxStaminaTableMaster",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator CreateMaxStaminaTableMaster(
                Request.CreateMaxStaminaTableMasterRequest request,
                UnityAction<AsyncResult<Result.CreateMaxStaminaTableMasterResult>> callback
        )
		{
			var task = new CreateMaxStaminaTableMasterTask(
			    Gs2WebSocketSession,
			    request
            );
            yield return task;
            callback.Invoke(new AsyncResult<Result.CreateMaxStaminaTableMasterResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.CreateMaxStaminaTableMasterResult> CreateMaxStaminaTableMaster(
            Request.CreateMaxStaminaTableMasterRequest request
        )
		{
		    var task = new CreateMaxStaminaTableMasterTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
#endif


        private class GetMaxStaminaTableMasterTask : Gs2WebSocketSessionTask<Request.GetMaxStaminaTableMasterRequest, Result.GetMaxStaminaTableMasterResult>
        {
	        public GetMaxStaminaTableMasterTask(IGs2Session session, Request.GetMaxStaminaTableMasterRequest request) : base(session, request)
	        {
	        }

            protected override IGs2SessionRequest CreateRequest(Request.GetMaxStaminaTableMasterRequest request)
            {
                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);

                jsonWriter.WriteObjectStart();

                if (request.NamespaceName != null)
                {
                    jsonWriter.WritePropertyName("namespaceName");
                    jsonWriter.Write(request.NamespaceName.ToString());
                }
                if (request.MaxStaminaTableName != null)
                {
                    jsonWriter.WritePropertyName("maxStaminaTableName");
                    jsonWriter.Write(request.MaxStaminaTableName.ToString());
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
                    "stamina",
                    "maxStaminaTableMaster",
                    "getMaxStaminaTableMaster",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator GetMaxStaminaTableMaster(
                Request.GetMaxStaminaTableMasterRequest request,
                UnityAction<AsyncResult<Result.GetMaxStaminaTableMasterResult>> callback
        )
		{
			var task = new GetMaxStaminaTableMasterTask(
			    Gs2WebSocketSession,
			    request
            );
            yield return task;
            callback.Invoke(new AsyncResult<Result.GetMaxStaminaTableMasterResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.GetMaxStaminaTableMasterResult> GetMaxStaminaTableMaster(
            Request.GetMaxStaminaTableMasterRequest request
        )
		{
		    var task = new GetMaxStaminaTableMasterTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
#endif


        private class UpdateMaxStaminaTableMasterTask : Gs2WebSocketSessionTask<Request.UpdateMaxStaminaTableMasterRequest, Result.UpdateMaxStaminaTableMasterResult>
        {
	        public UpdateMaxStaminaTableMasterTask(IGs2Session session, Request.UpdateMaxStaminaTableMasterRequest request) : base(session, request)
	        {
	        }

            protected override IGs2SessionRequest CreateRequest(Request.UpdateMaxStaminaTableMasterRequest request)
            {
                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);

                jsonWriter.WriteObjectStart();

                if (request.NamespaceName != null)
                {
                    jsonWriter.WritePropertyName("namespaceName");
                    jsonWriter.Write(request.NamespaceName.ToString());
                }
                if (request.MaxStaminaTableName != null)
                {
                    jsonWriter.WritePropertyName("maxStaminaTableName");
                    jsonWriter.Write(request.MaxStaminaTableName.ToString());
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
                if (request.ExperienceModelId != null)
                {
                    jsonWriter.WritePropertyName("experienceModelId");
                    jsonWriter.Write(request.ExperienceModelId.ToString());
                }
                if (request.Values != null)
                {
                    jsonWriter.WritePropertyName("values");
                    jsonWriter.WriteArrayStart();
                    foreach(var item in request.Values)
                    {
                        jsonWriter.Write(item);
                    }
                    jsonWriter.WriteArrayEnd();
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
                    "stamina",
                    "maxStaminaTableMaster",
                    "updateMaxStaminaTableMaster",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator UpdateMaxStaminaTableMaster(
                Request.UpdateMaxStaminaTableMasterRequest request,
                UnityAction<AsyncResult<Result.UpdateMaxStaminaTableMasterResult>> callback
        )
		{
			var task = new UpdateMaxStaminaTableMasterTask(
			    Gs2WebSocketSession,
			    request
            );
            yield return task;
            callback.Invoke(new AsyncResult<Result.UpdateMaxStaminaTableMasterResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.UpdateMaxStaminaTableMasterResult> UpdateMaxStaminaTableMaster(
            Request.UpdateMaxStaminaTableMasterRequest request
        )
		{
		    var task = new UpdateMaxStaminaTableMasterTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
#endif


        private class DeleteMaxStaminaTableMasterTask : Gs2WebSocketSessionTask<Request.DeleteMaxStaminaTableMasterRequest, Result.DeleteMaxStaminaTableMasterResult>
        {
	        public DeleteMaxStaminaTableMasterTask(IGs2Session session, Request.DeleteMaxStaminaTableMasterRequest request) : base(session, request)
	        {
	        }

            protected override IGs2SessionRequest CreateRequest(Request.DeleteMaxStaminaTableMasterRequest request)
            {
                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);

                jsonWriter.WriteObjectStart();

                if (request.NamespaceName != null)
                {
                    jsonWriter.WritePropertyName("namespaceName");
                    jsonWriter.Write(request.NamespaceName.ToString());
                }
                if (request.MaxStaminaTableName != null)
                {
                    jsonWriter.WritePropertyName("maxStaminaTableName");
                    jsonWriter.Write(request.MaxStaminaTableName.ToString());
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
                    "stamina",
                    "maxStaminaTableMaster",
                    "deleteMaxStaminaTableMaster",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator DeleteMaxStaminaTableMaster(
                Request.DeleteMaxStaminaTableMasterRequest request,
                UnityAction<AsyncResult<Result.DeleteMaxStaminaTableMasterResult>> callback
        )
		{
			var task = new DeleteMaxStaminaTableMasterTask(
			    Gs2WebSocketSession,
			    request
            );
            yield return task;
            callback.Invoke(new AsyncResult<Result.DeleteMaxStaminaTableMasterResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.DeleteMaxStaminaTableMasterResult> DeleteMaxStaminaTableMaster(
            Request.DeleteMaxStaminaTableMasterRequest request
        )
		{
		    var task = new DeleteMaxStaminaTableMasterTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
#endif


        private class CreateRecoverIntervalTableMasterTask : Gs2WebSocketSessionTask<Request.CreateRecoverIntervalTableMasterRequest, Result.CreateRecoverIntervalTableMasterResult>
        {
	        public CreateRecoverIntervalTableMasterTask(IGs2Session session, Request.CreateRecoverIntervalTableMasterRequest request) : base(session, request)
	        {
	        }

            protected override IGs2SessionRequest CreateRequest(Request.CreateRecoverIntervalTableMasterRequest request)
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
                if (request.ExperienceModelId != null)
                {
                    jsonWriter.WritePropertyName("experienceModelId");
                    jsonWriter.Write(request.ExperienceModelId.ToString());
                }
                if (request.Values != null)
                {
                    jsonWriter.WritePropertyName("values");
                    jsonWriter.WriteArrayStart();
                    foreach(var item in request.Values)
                    {
                        jsonWriter.Write(item);
                    }
                    jsonWriter.WriteArrayEnd();
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
                    "stamina",
                    "recoverIntervalTableMaster",
                    "createRecoverIntervalTableMaster",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator CreateRecoverIntervalTableMaster(
                Request.CreateRecoverIntervalTableMasterRequest request,
                UnityAction<AsyncResult<Result.CreateRecoverIntervalTableMasterResult>> callback
        )
		{
			var task = new CreateRecoverIntervalTableMasterTask(
			    Gs2WebSocketSession,
			    request
            );
            yield return task;
            callback.Invoke(new AsyncResult<Result.CreateRecoverIntervalTableMasterResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.CreateRecoverIntervalTableMasterResult> CreateRecoverIntervalTableMaster(
            Request.CreateRecoverIntervalTableMasterRequest request
        )
		{
		    var task = new CreateRecoverIntervalTableMasterTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
#endif


        private class GetRecoverIntervalTableMasterTask : Gs2WebSocketSessionTask<Request.GetRecoverIntervalTableMasterRequest, Result.GetRecoverIntervalTableMasterResult>
        {
	        public GetRecoverIntervalTableMasterTask(IGs2Session session, Request.GetRecoverIntervalTableMasterRequest request) : base(session, request)
	        {
	        }

            protected override IGs2SessionRequest CreateRequest(Request.GetRecoverIntervalTableMasterRequest request)
            {
                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);

                jsonWriter.WriteObjectStart();

                if (request.NamespaceName != null)
                {
                    jsonWriter.WritePropertyName("namespaceName");
                    jsonWriter.Write(request.NamespaceName.ToString());
                }
                if (request.RecoverIntervalTableName != null)
                {
                    jsonWriter.WritePropertyName("recoverIntervalTableName");
                    jsonWriter.Write(request.RecoverIntervalTableName.ToString());
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
                    "stamina",
                    "recoverIntervalTableMaster",
                    "getRecoverIntervalTableMaster",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator GetRecoverIntervalTableMaster(
                Request.GetRecoverIntervalTableMasterRequest request,
                UnityAction<AsyncResult<Result.GetRecoverIntervalTableMasterResult>> callback
        )
		{
			var task = new GetRecoverIntervalTableMasterTask(
			    Gs2WebSocketSession,
			    request
            );
            yield return task;
            callback.Invoke(new AsyncResult<Result.GetRecoverIntervalTableMasterResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.GetRecoverIntervalTableMasterResult> GetRecoverIntervalTableMaster(
            Request.GetRecoverIntervalTableMasterRequest request
        )
		{
		    var task = new GetRecoverIntervalTableMasterTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
#endif


        private class UpdateRecoverIntervalTableMasterTask : Gs2WebSocketSessionTask<Request.UpdateRecoverIntervalTableMasterRequest, Result.UpdateRecoverIntervalTableMasterResult>
        {
	        public UpdateRecoverIntervalTableMasterTask(IGs2Session session, Request.UpdateRecoverIntervalTableMasterRequest request) : base(session, request)
	        {
	        }

            protected override IGs2SessionRequest CreateRequest(Request.UpdateRecoverIntervalTableMasterRequest request)
            {
                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);

                jsonWriter.WriteObjectStart();

                if (request.NamespaceName != null)
                {
                    jsonWriter.WritePropertyName("namespaceName");
                    jsonWriter.Write(request.NamespaceName.ToString());
                }
                if (request.RecoverIntervalTableName != null)
                {
                    jsonWriter.WritePropertyName("recoverIntervalTableName");
                    jsonWriter.Write(request.RecoverIntervalTableName.ToString());
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
                if (request.ExperienceModelId != null)
                {
                    jsonWriter.WritePropertyName("experienceModelId");
                    jsonWriter.Write(request.ExperienceModelId.ToString());
                }
                if (request.Values != null)
                {
                    jsonWriter.WritePropertyName("values");
                    jsonWriter.WriteArrayStart();
                    foreach(var item in request.Values)
                    {
                        jsonWriter.Write(item);
                    }
                    jsonWriter.WriteArrayEnd();
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
                    "stamina",
                    "recoverIntervalTableMaster",
                    "updateRecoverIntervalTableMaster",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator UpdateRecoverIntervalTableMaster(
                Request.UpdateRecoverIntervalTableMasterRequest request,
                UnityAction<AsyncResult<Result.UpdateRecoverIntervalTableMasterResult>> callback
        )
		{
			var task = new UpdateRecoverIntervalTableMasterTask(
			    Gs2WebSocketSession,
			    request
            );
            yield return task;
            callback.Invoke(new AsyncResult<Result.UpdateRecoverIntervalTableMasterResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.UpdateRecoverIntervalTableMasterResult> UpdateRecoverIntervalTableMaster(
            Request.UpdateRecoverIntervalTableMasterRequest request
        )
		{
		    var task = new UpdateRecoverIntervalTableMasterTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
#endif


        private class DeleteRecoverIntervalTableMasterTask : Gs2WebSocketSessionTask<Request.DeleteRecoverIntervalTableMasterRequest, Result.DeleteRecoverIntervalTableMasterResult>
        {
	        public DeleteRecoverIntervalTableMasterTask(IGs2Session session, Request.DeleteRecoverIntervalTableMasterRequest request) : base(session, request)
	        {
	        }

            protected override IGs2SessionRequest CreateRequest(Request.DeleteRecoverIntervalTableMasterRequest request)
            {
                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);

                jsonWriter.WriteObjectStart();

                if (request.NamespaceName != null)
                {
                    jsonWriter.WritePropertyName("namespaceName");
                    jsonWriter.Write(request.NamespaceName.ToString());
                }
                if (request.RecoverIntervalTableName != null)
                {
                    jsonWriter.WritePropertyName("recoverIntervalTableName");
                    jsonWriter.Write(request.RecoverIntervalTableName.ToString());
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
                    "stamina",
                    "recoverIntervalTableMaster",
                    "deleteRecoverIntervalTableMaster",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator DeleteRecoverIntervalTableMaster(
                Request.DeleteRecoverIntervalTableMasterRequest request,
                UnityAction<AsyncResult<Result.DeleteRecoverIntervalTableMasterResult>> callback
        )
		{
			var task = new DeleteRecoverIntervalTableMasterTask(
			    Gs2WebSocketSession,
			    request
            );
            yield return task;
            callback.Invoke(new AsyncResult<Result.DeleteRecoverIntervalTableMasterResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.DeleteRecoverIntervalTableMasterResult> DeleteRecoverIntervalTableMaster(
            Request.DeleteRecoverIntervalTableMasterRequest request
        )
		{
		    var task = new DeleteRecoverIntervalTableMasterTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
#endif


        private class CreateRecoverValueTableMasterTask : Gs2WebSocketSessionTask<Request.CreateRecoverValueTableMasterRequest, Result.CreateRecoverValueTableMasterResult>
        {
	        public CreateRecoverValueTableMasterTask(IGs2Session session, Request.CreateRecoverValueTableMasterRequest request) : base(session, request)
	        {
	        }

            protected override IGs2SessionRequest CreateRequest(Request.CreateRecoverValueTableMasterRequest request)
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
                if (request.ExperienceModelId != null)
                {
                    jsonWriter.WritePropertyName("experienceModelId");
                    jsonWriter.Write(request.ExperienceModelId.ToString());
                }
                if (request.Values != null)
                {
                    jsonWriter.WritePropertyName("values");
                    jsonWriter.WriteArrayStart();
                    foreach(var item in request.Values)
                    {
                        jsonWriter.Write(item);
                    }
                    jsonWriter.WriteArrayEnd();
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
                    "stamina",
                    "recoverValueTableMaster",
                    "createRecoverValueTableMaster",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator CreateRecoverValueTableMaster(
                Request.CreateRecoverValueTableMasterRequest request,
                UnityAction<AsyncResult<Result.CreateRecoverValueTableMasterResult>> callback
        )
		{
			var task = new CreateRecoverValueTableMasterTask(
			    Gs2WebSocketSession,
			    request
            );
            yield return task;
            callback.Invoke(new AsyncResult<Result.CreateRecoverValueTableMasterResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.CreateRecoverValueTableMasterResult> CreateRecoverValueTableMaster(
            Request.CreateRecoverValueTableMasterRequest request
        )
		{
		    var task = new CreateRecoverValueTableMasterTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
#endif


        private class GetRecoverValueTableMasterTask : Gs2WebSocketSessionTask<Request.GetRecoverValueTableMasterRequest, Result.GetRecoverValueTableMasterResult>
        {
	        public GetRecoverValueTableMasterTask(IGs2Session session, Request.GetRecoverValueTableMasterRequest request) : base(session, request)
	        {
	        }

            protected override IGs2SessionRequest CreateRequest(Request.GetRecoverValueTableMasterRequest request)
            {
                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);

                jsonWriter.WriteObjectStart();

                if (request.NamespaceName != null)
                {
                    jsonWriter.WritePropertyName("namespaceName");
                    jsonWriter.Write(request.NamespaceName.ToString());
                }
                if (request.RecoverValueTableName != null)
                {
                    jsonWriter.WritePropertyName("recoverValueTableName");
                    jsonWriter.Write(request.RecoverValueTableName.ToString());
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
                    "stamina",
                    "recoverValueTableMaster",
                    "getRecoverValueTableMaster",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator GetRecoverValueTableMaster(
                Request.GetRecoverValueTableMasterRequest request,
                UnityAction<AsyncResult<Result.GetRecoverValueTableMasterResult>> callback
        )
		{
			var task = new GetRecoverValueTableMasterTask(
			    Gs2WebSocketSession,
			    request
            );
            yield return task;
            callback.Invoke(new AsyncResult<Result.GetRecoverValueTableMasterResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.GetRecoverValueTableMasterResult> GetRecoverValueTableMaster(
            Request.GetRecoverValueTableMasterRequest request
        )
		{
		    var task = new GetRecoverValueTableMasterTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
#endif


        private class UpdateRecoverValueTableMasterTask : Gs2WebSocketSessionTask<Request.UpdateRecoverValueTableMasterRequest, Result.UpdateRecoverValueTableMasterResult>
        {
	        public UpdateRecoverValueTableMasterTask(IGs2Session session, Request.UpdateRecoverValueTableMasterRequest request) : base(session, request)
	        {
	        }

            protected override IGs2SessionRequest CreateRequest(Request.UpdateRecoverValueTableMasterRequest request)
            {
                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);

                jsonWriter.WriteObjectStart();

                if (request.NamespaceName != null)
                {
                    jsonWriter.WritePropertyName("namespaceName");
                    jsonWriter.Write(request.NamespaceName.ToString());
                }
                if (request.RecoverValueTableName != null)
                {
                    jsonWriter.WritePropertyName("recoverValueTableName");
                    jsonWriter.Write(request.RecoverValueTableName.ToString());
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
                if (request.ExperienceModelId != null)
                {
                    jsonWriter.WritePropertyName("experienceModelId");
                    jsonWriter.Write(request.ExperienceModelId.ToString());
                }
                if (request.Values != null)
                {
                    jsonWriter.WritePropertyName("values");
                    jsonWriter.WriteArrayStart();
                    foreach(var item in request.Values)
                    {
                        jsonWriter.Write(item);
                    }
                    jsonWriter.WriteArrayEnd();
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
                    "stamina",
                    "recoverValueTableMaster",
                    "updateRecoverValueTableMaster",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator UpdateRecoverValueTableMaster(
                Request.UpdateRecoverValueTableMasterRequest request,
                UnityAction<AsyncResult<Result.UpdateRecoverValueTableMasterResult>> callback
        )
		{
			var task = new UpdateRecoverValueTableMasterTask(
			    Gs2WebSocketSession,
			    request
            );
            yield return task;
            callback.Invoke(new AsyncResult<Result.UpdateRecoverValueTableMasterResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.UpdateRecoverValueTableMasterResult> UpdateRecoverValueTableMaster(
            Request.UpdateRecoverValueTableMasterRequest request
        )
		{
		    var task = new UpdateRecoverValueTableMasterTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
#endif


        private class DeleteRecoverValueTableMasterTask : Gs2WebSocketSessionTask<Request.DeleteRecoverValueTableMasterRequest, Result.DeleteRecoverValueTableMasterResult>
        {
	        public DeleteRecoverValueTableMasterTask(IGs2Session session, Request.DeleteRecoverValueTableMasterRequest request) : base(session, request)
	        {
	        }

            protected override IGs2SessionRequest CreateRequest(Request.DeleteRecoverValueTableMasterRequest request)
            {
                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);

                jsonWriter.WriteObjectStart();

                if (request.NamespaceName != null)
                {
                    jsonWriter.WritePropertyName("namespaceName");
                    jsonWriter.Write(request.NamespaceName.ToString());
                }
                if (request.RecoverValueTableName != null)
                {
                    jsonWriter.WritePropertyName("recoverValueTableName");
                    jsonWriter.Write(request.RecoverValueTableName.ToString());
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
                    "stamina",
                    "recoverValueTableMaster",
                    "deleteRecoverValueTableMaster",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator DeleteRecoverValueTableMaster(
                Request.DeleteRecoverValueTableMasterRequest request,
                UnityAction<AsyncResult<Result.DeleteRecoverValueTableMasterResult>> callback
        )
		{
			var task = new DeleteRecoverValueTableMasterTask(
			    Gs2WebSocketSession,
			    request
            );
            yield return task;
            callback.Invoke(new AsyncResult<Result.DeleteRecoverValueTableMasterResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.DeleteRecoverValueTableMasterResult> DeleteRecoverValueTableMaster(
            Request.DeleteRecoverValueTableMasterRequest request
        )
		{
		    var task = new DeleteRecoverValueTableMasterTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
#endif


        private class GetStaminaModelTask : Gs2WebSocketSessionTask<Request.GetStaminaModelRequest, Result.GetStaminaModelResult>
        {
	        public GetStaminaModelTask(IGs2Session session, Request.GetStaminaModelRequest request) : base(session, request)
	        {
	        }

            protected override IGs2SessionRequest CreateRequest(Request.GetStaminaModelRequest request)
            {
                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);

                jsonWriter.WriteObjectStart();

                if (request.NamespaceName != null)
                {
                    jsonWriter.WritePropertyName("namespaceName");
                    jsonWriter.Write(request.NamespaceName.ToString());
                }
                if (request.StaminaName != null)
                {
                    jsonWriter.WritePropertyName("staminaName");
                    jsonWriter.Write(request.StaminaName.ToString());
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
                    "stamina",
                    "staminaModel",
                    "getStaminaModel",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator GetStaminaModel(
                Request.GetStaminaModelRequest request,
                UnityAction<AsyncResult<Result.GetStaminaModelResult>> callback
        )
		{
			var task = new GetStaminaModelTask(
			    Gs2WebSocketSession,
			    request
            );
            yield return task;
            callback.Invoke(new AsyncResult<Result.GetStaminaModelResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.GetStaminaModelResult> GetStaminaModel(
            Request.GetStaminaModelRequest request
        )
		{
		    var task = new GetStaminaModelTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
#endif


        private class GetStaminaTask : Gs2WebSocketSessionTask<Request.GetStaminaRequest, Result.GetStaminaResult>
        {
	        public GetStaminaTask(IGs2Session session, Request.GetStaminaRequest request) : base(session, request)
	        {
	        }

            protected override IGs2SessionRequest CreateRequest(Request.GetStaminaRequest request)
            {
                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);

                jsonWriter.WriteObjectStart();

                if (request.NamespaceName != null)
                {
                    jsonWriter.WritePropertyName("namespaceName");
                    jsonWriter.Write(request.NamespaceName.ToString());
                }
                if (request.StaminaName != null)
                {
                    jsonWriter.WritePropertyName("staminaName");
                    jsonWriter.Write(request.StaminaName.ToString());
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
                    "stamina",
                    "stamina",
                    "getStamina",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator GetStamina(
                Request.GetStaminaRequest request,
                UnityAction<AsyncResult<Result.GetStaminaResult>> callback
        )
		{
			var task = new GetStaminaTask(
			    Gs2WebSocketSession,
			    request
            );
            yield return task;
            callback.Invoke(new AsyncResult<Result.GetStaminaResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.GetStaminaResult> GetStamina(
            Request.GetStaminaRequest request
        )
		{
		    var task = new GetStaminaTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
#endif


        private class GetStaminaByUserIdTask : Gs2WebSocketSessionTask<Request.GetStaminaByUserIdRequest, Result.GetStaminaByUserIdResult>
        {
	        public GetStaminaByUserIdTask(IGs2Session session, Request.GetStaminaByUserIdRequest request) : base(session, request)
	        {
	        }

            protected override IGs2SessionRequest CreateRequest(Request.GetStaminaByUserIdRequest request)
            {
                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);

                jsonWriter.WriteObjectStart();

                if (request.NamespaceName != null)
                {
                    jsonWriter.WritePropertyName("namespaceName");
                    jsonWriter.Write(request.NamespaceName.ToString());
                }
                if (request.StaminaName != null)
                {
                    jsonWriter.WritePropertyName("staminaName");
                    jsonWriter.Write(request.StaminaName.ToString());
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
                    "stamina",
                    "stamina",
                    "getStaminaByUserId",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator GetStaminaByUserId(
                Request.GetStaminaByUserIdRequest request,
                UnityAction<AsyncResult<Result.GetStaminaByUserIdResult>> callback
        )
		{
			var task = new GetStaminaByUserIdTask(
			    Gs2WebSocketSession,
			    request
            );
            yield return task;
            callback.Invoke(new AsyncResult<Result.GetStaminaByUserIdResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.GetStaminaByUserIdResult> GetStaminaByUserId(
            Request.GetStaminaByUserIdRequest request
        )
		{
		    var task = new GetStaminaByUserIdTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
#endif


        private class UpdateStaminaByUserIdTask : Gs2WebSocketSessionTask<Request.UpdateStaminaByUserIdRequest, Result.UpdateStaminaByUserIdResult>
        {
	        public UpdateStaminaByUserIdTask(IGs2Session session, Request.UpdateStaminaByUserIdRequest request) : base(session, request)
	        {
	        }

            protected override IGs2SessionRequest CreateRequest(Request.UpdateStaminaByUserIdRequest request)
            {
                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);

                jsonWriter.WriteObjectStart();

                if (request.NamespaceName != null)
                {
                    jsonWriter.WritePropertyName("namespaceName");
                    jsonWriter.Write(request.NamespaceName.ToString());
                }
                if (request.StaminaName != null)
                {
                    jsonWriter.WritePropertyName("staminaName");
                    jsonWriter.Write(request.StaminaName.ToString());
                }
                if (request.UserId != null)
                {
                    jsonWriter.WritePropertyName("userId");
                    jsonWriter.Write(request.UserId.ToString());
                }
                if (request.Value != null)
                {
                    jsonWriter.WritePropertyName("value");
                    jsonWriter.Write(request.Value.ToString());
                }
                if (request.MaxValue != null)
                {
                    jsonWriter.WritePropertyName("maxValue");
                    jsonWriter.Write(request.MaxValue.ToString());
                }
                if (request.RecoverIntervalMinutes != null)
                {
                    jsonWriter.WritePropertyName("recoverIntervalMinutes");
                    jsonWriter.Write(request.RecoverIntervalMinutes.ToString());
                }
                if (request.RecoverValue != null)
                {
                    jsonWriter.WritePropertyName("recoverValue");
                    jsonWriter.Write(request.RecoverValue.ToString());
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
                    "stamina",
                    "stamina",
                    "updateStaminaByUserId",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator UpdateStaminaByUserId(
                Request.UpdateStaminaByUserIdRequest request,
                UnityAction<AsyncResult<Result.UpdateStaminaByUserIdResult>> callback
        )
		{
			var task = new UpdateStaminaByUserIdTask(
			    Gs2WebSocketSession,
			    request
            );
            yield return task;
            callback.Invoke(new AsyncResult<Result.UpdateStaminaByUserIdResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.UpdateStaminaByUserIdResult> UpdateStaminaByUserId(
            Request.UpdateStaminaByUserIdRequest request
        )
		{
		    var task = new UpdateStaminaByUserIdTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
#endif


        private class ConsumeStaminaTask : Gs2WebSocketSessionTask<Request.ConsumeStaminaRequest, Result.ConsumeStaminaResult>
        {
	        public ConsumeStaminaTask(IGs2Session session, Request.ConsumeStaminaRequest request) : base(session, request)
	        {
	        }

            protected override IGs2SessionRequest CreateRequest(Request.ConsumeStaminaRequest request)
            {
                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);

                jsonWriter.WriteObjectStart();

                if (request.NamespaceName != null)
                {
                    jsonWriter.WritePropertyName("namespaceName");
                    jsonWriter.Write(request.NamespaceName.ToString());
                }
                if (request.StaminaName != null)
                {
                    jsonWriter.WritePropertyName("staminaName");
                    jsonWriter.Write(request.StaminaName.ToString());
                }
                if (request.AccessToken != null)
                {
                    jsonWriter.WritePropertyName("accessToken");
                    jsonWriter.Write(request.AccessToken.ToString());
                }
                if (request.ConsumeValue != null)
                {
                    jsonWriter.WritePropertyName("consumeValue");
                    jsonWriter.Write(request.ConsumeValue.ToString());
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
                    "stamina",
                    "stamina",
                    "consumeStamina",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator ConsumeStamina(
                Request.ConsumeStaminaRequest request,
                UnityAction<AsyncResult<Result.ConsumeStaminaResult>> callback
        )
		{
			var task = new ConsumeStaminaTask(
			    Gs2WebSocketSession,
			    request
            );
            yield return task;
            callback.Invoke(new AsyncResult<Result.ConsumeStaminaResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.ConsumeStaminaResult> ConsumeStamina(
            Request.ConsumeStaminaRequest request
        )
		{
		    var task = new ConsumeStaminaTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
#endif


        private class ConsumeStaminaByUserIdTask : Gs2WebSocketSessionTask<Request.ConsumeStaminaByUserIdRequest, Result.ConsumeStaminaByUserIdResult>
        {
	        public ConsumeStaminaByUserIdTask(IGs2Session session, Request.ConsumeStaminaByUserIdRequest request) : base(session, request)
	        {
	        }

            protected override IGs2SessionRequest CreateRequest(Request.ConsumeStaminaByUserIdRequest request)
            {
                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);

                jsonWriter.WriteObjectStart();

                if (request.NamespaceName != null)
                {
                    jsonWriter.WritePropertyName("namespaceName");
                    jsonWriter.Write(request.NamespaceName.ToString());
                }
                if (request.StaminaName != null)
                {
                    jsonWriter.WritePropertyName("staminaName");
                    jsonWriter.Write(request.StaminaName.ToString());
                }
                if (request.UserId != null)
                {
                    jsonWriter.WritePropertyName("userId");
                    jsonWriter.Write(request.UserId.ToString());
                }
                if (request.ConsumeValue != null)
                {
                    jsonWriter.WritePropertyName("consumeValue");
                    jsonWriter.Write(request.ConsumeValue.ToString());
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
                    "stamina",
                    "stamina",
                    "consumeStaminaByUserId",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator ConsumeStaminaByUserId(
                Request.ConsumeStaminaByUserIdRequest request,
                UnityAction<AsyncResult<Result.ConsumeStaminaByUserIdResult>> callback
        )
		{
			var task = new ConsumeStaminaByUserIdTask(
			    Gs2WebSocketSession,
			    request
            );
            yield return task;
            callback.Invoke(new AsyncResult<Result.ConsumeStaminaByUserIdResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.ConsumeStaminaByUserIdResult> ConsumeStaminaByUserId(
            Request.ConsumeStaminaByUserIdRequest request
        )
		{
		    var task = new ConsumeStaminaByUserIdTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
#endif


        private class RecoverStaminaByUserIdTask : Gs2WebSocketSessionTask<Request.RecoverStaminaByUserIdRequest, Result.RecoverStaminaByUserIdResult>
        {
	        public RecoverStaminaByUserIdTask(IGs2Session session, Request.RecoverStaminaByUserIdRequest request) : base(session, request)
	        {
	        }

            protected override IGs2SessionRequest CreateRequest(Request.RecoverStaminaByUserIdRequest request)
            {
                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);

                jsonWriter.WriteObjectStart();

                if (request.NamespaceName != null)
                {
                    jsonWriter.WritePropertyName("namespaceName");
                    jsonWriter.Write(request.NamespaceName.ToString());
                }
                if (request.StaminaName != null)
                {
                    jsonWriter.WritePropertyName("staminaName");
                    jsonWriter.Write(request.StaminaName.ToString());
                }
                if (request.UserId != null)
                {
                    jsonWriter.WritePropertyName("userId");
                    jsonWriter.Write(request.UserId.ToString());
                }
                if (request.RecoverValue != null)
                {
                    jsonWriter.WritePropertyName("recoverValue");
                    jsonWriter.Write(request.RecoverValue.ToString());
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
                    "stamina",
                    "stamina",
                    "recoverStaminaByUserId",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator RecoverStaminaByUserId(
                Request.RecoverStaminaByUserIdRequest request,
                UnityAction<AsyncResult<Result.RecoverStaminaByUserIdResult>> callback
        )
		{
			var task = new RecoverStaminaByUserIdTask(
			    Gs2WebSocketSession,
			    request
            );
            yield return task;
            callback.Invoke(new AsyncResult<Result.RecoverStaminaByUserIdResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.RecoverStaminaByUserIdResult> RecoverStaminaByUserId(
            Request.RecoverStaminaByUserIdRequest request
        )
		{
		    var task = new RecoverStaminaByUserIdTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
#endif


        private class RaiseMaxValueByUserIdTask : Gs2WebSocketSessionTask<Request.RaiseMaxValueByUserIdRequest, Result.RaiseMaxValueByUserIdResult>
        {
	        public RaiseMaxValueByUserIdTask(IGs2Session session, Request.RaiseMaxValueByUserIdRequest request) : base(session, request)
	        {
	        }

            protected override IGs2SessionRequest CreateRequest(Request.RaiseMaxValueByUserIdRequest request)
            {
                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);

                jsonWriter.WriteObjectStart();

                if (request.NamespaceName != null)
                {
                    jsonWriter.WritePropertyName("namespaceName");
                    jsonWriter.Write(request.NamespaceName.ToString());
                }
                if (request.StaminaName != null)
                {
                    jsonWriter.WritePropertyName("staminaName");
                    jsonWriter.Write(request.StaminaName.ToString());
                }
                if (request.UserId != null)
                {
                    jsonWriter.WritePropertyName("userId");
                    jsonWriter.Write(request.UserId.ToString());
                }
                if (request.RaiseValue != null)
                {
                    jsonWriter.WritePropertyName("raiseValue");
                    jsonWriter.Write(request.RaiseValue.ToString());
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
                    "stamina",
                    "stamina",
                    "raiseMaxValueByUserId",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator RaiseMaxValueByUserId(
                Request.RaiseMaxValueByUserIdRequest request,
                UnityAction<AsyncResult<Result.RaiseMaxValueByUserIdResult>> callback
        )
		{
			var task = new RaiseMaxValueByUserIdTask(
			    Gs2WebSocketSession,
			    request
            );
            yield return task;
            callback.Invoke(new AsyncResult<Result.RaiseMaxValueByUserIdResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.RaiseMaxValueByUserIdResult> RaiseMaxValueByUserId(
            Request.RaiseMaxValueByUserIdRequest request
        )
		{
		    var task = new RaiseMaxValueByUserIdTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
#endif


        private class SetMaxValueByUserIdTask : Gs2WebSocketSessionTask<Request.SetMaxValueByUserIdRequest, Result.SetMaxValueByUserIdResult>
        {
	        public SetMaxValueByUserIdTask(IGs2Session session, Request.SetMaxValueByUserIdRequest request) : base(session, request)
	        {
	        }

            protected override IGs2SessionRequest CreateRequest(Request.SetMaxValueByUserIdRequest request)
            {
                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);

                jsonWriter.WriteObjectStart();

                if (request.NamespaceName != null)
                {
                    jsonWriter.WritePropertyName("namespaceName");
                    jsonWriter.Write(request.NamespaceName.ToString());
                }
                if (request.StaminaName != null)
                {
                    jsonWriter.WritePropertyName("staminaName");
                    jsonWriter.Write(request.StaminaName.ToString());
                }
                if (request.UserId != null)
                {
                    jsonWriter.WritePropertyName("userId");
                    jsonWriter.Write(request.UserId.ToString());
                }
                if (request.MaxValue != null)
                {
                    jsonWriter.WritePropertyName("maxValue");
                    jsonWriter.Write(request.MaxValue.ToString());
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
                    "stamina",
                    "stamina",
                    "setMaxValueByUserId",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator SetMaxValueByUserId(
                Request.SetMaxValueByUserIdRequest request,
                UnityAction<AsyncResult<Result.SetMaxValueByUserIdResult>> callback
        )
		{
			var task = new SetMaxValueByUserIdTask(
			    Gs2WebSocketSession,
			    request
            );
            yield return task;
            callback.Invoke(new AsyncResult<Result.SetMaxValueByUserIdResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.SetMaxValueByUserIdResult> SetMaxValueByUserId(
            Request.SetMaxValueByUserIdRequest request
        )
		{
		    var task = new SetMaxValueByUserIdTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
#endif


        private class SetRecoverIntervalByUserIdTask : Gs2WebSocketSessionTask<Request.SetRecoverIntervalByUserIdRequest, Result.SetRecoverIntervalByUserIdResult>
        {
	        public SetRecoverIntervalByUserIdTask(IGs2Session session, Request.SetRecoverIntervalByUserIdRequest request) : base(session, request)
	        {
	        }

            protected override IGs2SessionRequest CreateRequest(Request.SetRecoverIntervalByUserIdRequest request)
            {
                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);

                jsonWriter.WriteObjectStart();

                if (request.NamespaceName != null)
                {
                    jsonWriter.WritePropertyName("namespaceName");
                    jsonWriter.Write(request.NamespaceName.ToString());
                }
                if (request.StaminaName != null)
                {
                    jsonWriter.WritePropertyName("staminaName");
                    jsonWriter.Write(request.StaminaName.ToString());
                }
                if (request.UserId != null)
                {
                    jsonWriter.WritePropertyName("userId");
                    jsonWriter.Write(request.UserId.ToString());
                }
                if (request.RecoverIntervalMinutes != null)
                {
                    jsonWriter.WritePropertyName("recoverIntervalMinutes");
                    jsonWriter.Write(request.RecoverIntervalMinutes.ToString());
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
                    "stamina",
                    "stamina",
                    "setRecoverIntervalByUserId",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator SetRecoverIntervalByUserId(
                Request.SetRecoverIntervalByUserIdRequest request,
                UnityAction<AsyncResult<Result.SetRecoverIntervalByUserIdResult>> callback
        )
		{
			var task = new SetRecoverIntervalByUserIdTask(
			    Gs2WebSocketSession,
			    request
            );
            yield return task;
            callback.Invoke(new AsyncResult<Result.SetRecoverIntervalByUserIdResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.SetRecoverIntervalByUserIdResult> SetRecoverIntervalByUserId(
            Request.SetRecoverIntervalByUserIdRequest request
        )
		{
		    var task = new SetRecoverIntervalByUserIdTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
#endif


        private class SetRecoverValueByUserIdTask : Gs2WebSocketSessionTask<Request.SetRecoverValueByUserIdRequest, Result.SetRecoverValueByUserIdResult>
        {
	        public SetRecoverValueByUserIdTask(IGs2Session session, Request.SetRecoverValueByUserIdRequest request) : base(session, request)
	        {
	        }

            protected override IGs2SessionRequest CreateRequest(Request.SetRecoverValueByUserIdRequest request)
            {
                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);

                jsonWriter.WriteObjectStart();

                if (request.NamespaceName != null)
                {
                    jsonWriter.WritePropertyName("namespaceName");
                    jsonWriter.Write(request.NamespaceName.ToString());
                }
                if (request.StaminaName != null)
                {
                    jsonWriter.WritePropertyName("staminaName");
                    jsonWriter.Write(request.StaminaName.ToString());
                }
                if (request.UserId != null)
                {
                    jsonWriter.WritePropertyName("userId");
                    jsonWriter.Write(request.UserId.ToString());
                }
                if (request.RecoverValue != null)
                {
                    jsonWriter.WritePropertyName("recoverValue");
                    jsonWriter.Write(request.RecoverValue.ToString());
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
                    "stamina",
                    "stamina",
                    "setRecoverValueByUserId",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator SetRecoverValueByUserId(
                Request.SetRecoverValueByUserIdRequest request,
                UnityAction<AsyncResult<Result.SetRecoverValueByUserIdResult>> callback
        )
		{
			var task = new SetRecoverValueByUserIdTask(
			    Gs2WebSocketSession,
			    request
            );
            yield return task;
            callback.Invoke(new AsyncResult<Result.SetRecoverValueByUserIdResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.SetRecoverValueByUserIdResult> SetRecoverValueByUserId(
            Request.SetRecoverValueByUserIdRequest request
        )
		{
		    var task = new SetRecoverValueByUserIdTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
#endif


        private class SetMaxValueByStatusTask : Gs2WebSocketSessionTask<Request.SetMaxValueByStatusRequest, Result.SetMaxValueByStatusResult>
        {
	        public SetMaxValueByStatusTask(IGs2Session session, Request.SetMaxValueByStatusRequest request) : base(session, request)
	        {
	        }

            protected override IGs2SessionRequest CreateRequest(Request.SetMaxValueByStatusRequest request)
            {
                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);

                jsonWriter.WriteObjectStart();

                if (request.NamespaceName != null)
                {
                    jsonWriter.WritePropertyName("namespaceName");
                    jsonWriter.Write(request.NamespaceName.ToString());
                }
                if (request.StaminaName != null)
                {
                    jsonWriter.WritePropertyName("staminaName");
                    jsonWriter.Write(request.StaminaName.ToString());
                }
                if (request.AccessToken != null)
                {
                    jsonWriter.WritePropertyName("accessToken");
                    jsonWriter.Write(request.AccessToken.ToString());
                }
                if (request.KeyId != null)
                {
                    jsonWriter.WritePropertyName("keyId");
                    jsonWriter.Write(request.KeyId.ToString());
                }
                if (request.SignedStatusBody != null)
                {
                    jsonWriter.WritePropertyName("signedStatusBody");
                    jsonWriter.Write(request.SignedStatusBody.ToString());
                }
                if (request.SignedStatusSignature != null)
                {
                    jsonWriter.WritePropertyName("signedStatusSignature");
                    jsonWriter.Write(request.SignedStatusSignature.ToString());
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
                    "stamina",
                    "stamina",
                    "setMaxValueByStatus",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator SetMaxValueByStatus(
                Request.SetMaxValueByStatusRequest request,
                UnityAction<AsyncResult<Result.SetMaxValueByStatusResult>> callback
        )
		{
			var task = new SetMaxValueByStatusTask(
			    Gs2WebSocketSession,
			    request
            );
            yield return task;
            callback.Invoke(new AsyncResult<Result.SetMaxValueByStatusResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.SetMaxValueByStatusResult> SetMaxValueByStatus(
            Request.SetMaxValueByStatusRequest request
        )
		{
		    var task = new SetMaxValueByStatusTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
#endif


        private class SetRecoverIntervalByStatusTask : Gs2WebSocketSessionTask<Request.SetRecoverIntervalByStatusRequest, Result.SetRecoverIntervalByStatusResult>
        {
	        public SetRecoverIntervalByStatusTask(IGs2Session session, Request.SetRecoverIntervalByStatusRequest request) : base(session, request)
	        {
	        }

            protected override IGs2SessionRequest CreateRequest(Request.SetRecoverIntervalByStatusRequest request)
            {
                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);

                jsonWriter.WriteObjectStart();

                if (request.NamespaceName != null)
                {
                    jsonWriter.WritePropertyName("namespaceName");
                    jsonWriter.Write(request.NamespaceName.ToString());
                }
                if (request.StaminaName != null)
                {
                    jsonWriter.WritePropertyName("staminaName");
                    jsonWriter.Write(request.StaminaName.ToString());
                }
                if (request.AccessToken != null)
                {
                    jsonWriter.WritePropertyName("accessToken");
                    jsonWriter.Write(request.AccessToken.ToString());
                }
                if (request.KeyId != null)
                {
                    jsonWriter.WritePropertyName("keyId");
                    jsonWriter.Write(request.KeyId.ToString());
                }
                if (request.SignedStatusBody != null)
                {
                    jsonWriter.WritePropertyName("signedStatusBody");
                    jsonWriter.Write(request.SignedStatusBody.ToString());
                }
                if (request.SignedStatusSignature != null)
                {
                    jsonWriter.WritePropertyName("signedStatusSignature");
                    jsonWriter.Write(request.SignedStatusSignature.ToString());
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
                    "stamina",
                    "stamina",
                    "setRecoverIntervalByStatus",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator SetRecoverIntervalByStatus(
                Request.SetRecoverIntervalByStatusRequest request,
                UnityAction<AsyncResult<Result.SetRecoverIntervalByStatusResult>> callback
        )
		{
			var task = new SetRecoverIntervalByStatusTask(
			    Gs2WebSocketSession,
			    request
            );
            yield return task;
            callback.Invoke(new AsyncResult<Result.SetRecoverIntervalByStatusResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.SetRecoverIntervalByStatusResult> SetRecoverIntervalByStatus(
            Request.SetRecoverIntervalByStatusRequest request
        )
		{
		    var task = new SetRecoverIntervalByStatusTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
#endif


        private class SetRecoverValueByStatusTask : Gs2WebSocketSessionTask<Request.SetRecoverValueByStatusRequest, Result.SetRecoverValueByStatusResult>
        {
	        public SetRecoverValueByStatusTask(IGs2Session session, Request.SetRecoverValueByStatusRequest request) : base(session, request)
	        {
	        }

            protected override IGs2SessionRequest CreateRequest(Request.SetRecoverValueByStatusRequest request)
            {
                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);

                jsonWriter.WriteObjectStart();

                if (request.NamespaceName != null)
                {
                    jsonWriter.WritePropertyName("namespaceName");
                    jsonWriter.Write(request.NamespaceName.ToString());
                }
                if (request.StaminaName != null)
                {
                    jsonWriter.WritePropertyName("staminaName");
                    jsonWriter.Write(request.StaminaName.ToString());
                }
                if (request.AccessToken != null)
                {
                    jsonWriter.WritePropertyName("accessToken");
                    jsonWriter.Write(request.AccessToken.ToString());
                }
                if (request.KeyId != null)
                {
                    jsonWriter.WritePropertyName("keyId");
                    jsonWriter.Write(request.KeyId.ToString());
                }
                if (request.SignedStatusBody != null)
                {
                    jsonWriter.WritePropertyName("signedStatusBody");
                    jsonWriter.Write(request.SignedStatusBody.ToString());
                }
                if (request.SignedStatusSignature != null)
                {
                    jsonWriter.WritePropertyName("signedStatusSignature");
                    jsonWriter.Write(request.SignedStatusSignature.ToString());
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
                    "stamina",
                    "stamina",
                    "setRecoverValueByStatus",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator SetRecoverValueByStatus(
                Request.SetRecoverValueByStatusRequest request,
                UnityAction<AsyncResult<Result.SetRecoverValueByStatusResult>> callback
        )
		{
			var task = new SetRecoverValueByStatusTask(
			    Gs2WebSocketSession,
			    request
            );
            yield return task;
            callback.Invoke(new AsyncResult<Result.SetRecoverValueByStatusResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.SetRecoverValueByStatusResult> SetRecoverValueByStatus(
            Request.SetRecoverValueByStatusRequest request
        )
		{
		    var task = new SetRecoverValueByStatusTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
#endif


        private class DeleteStaminaByUserIdTask : Gs2WebSocketSessionTask<Request.DeleteStaminaByUserIdRequest, Result.DeleteStaminaByUserIdResult>
        {
	        public DeleteStaminaByUserIdTask(IGs2Session session, Request.DeleteStaminaByUserIdRequest request) : base(session, request)
	        {
	        }

            protected override IGs2SessionRequest CreateRequest(Request.DeleteStaminaByUserIdRequest request)
            {
                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);

                jsonWriter.WriteObjectStart();

                if (request.NamespaceName != null)
                {
                    jsonWriter.WritePropertyName("namespaceName");
                    jsonWriter.Write(request.NamespaceName.ToString());
                }
                if (request.StaminaName != null)
                {
                    jsonWriter.WritePropertyName("staminaName");
                    jsonWriter.Write(request.StaminaName.ToString());
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
                    "stamina",
                    "stamina",
                    "deleteStaminaByUserId",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator DeleteStaminaByUserId(
                Request.DeleteStaminaByUserIdRequest request,
                UnityAction<AsyncResult<Result.DeleteStaminaByUserIdResult>> callback
        )
		{
			var task = new DeleteStaminaByUserIdTask(
			    Gs2WebSocketSession,
			    request
            );
            yield return task;
            callback.Invoke(new AsyncResult<Result.DeleteStaminaByUserIdResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.DeleteStaminaByUserIdResult> DeleteStaminaByUserId(
            Request.DeleteStaminaByUserIdRequest request
        )
		{
		    var task = new DeleteStaminaByUserIdTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
#endif


        private class RecoverStaminaByStampSheetTask : Gs2WebSocketSessionTask<Request.RecoverStaminaByStampSheetRequest, Result.RecoverStaminaByStampSheetResult>
        {
	        public RecoverStaminaByStampSheetTask(IGs2Session session, Request.RecoverStaminaByStampSheetRequest request) : base(session, request)
	        {
	        }

            protected override IGs2SessionRequest CreateRequest(Request.RecoverStaminaByStampSheetRequest request)
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
                    "stamina",
                    "stamina",
                    "recoverStaminaByStampSheet",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator RecoverStaminaByStampSheet(
                Request.RecoverStaminaByStampSheetRequest request,
                UnityAction<AsyncResult<Result.RecoverStaminaByStampSheetResult>> callback
        )
		{
			var task = new RecoverStaminaByStampSheetTask(
			    Gs2WebSocketSession,
			    request
            );
            yield return task;
            callback.Invoke(new AsyncResult<Result.RecoverStaminaByStampSheetResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.RecoverStaminaByStampSheetResult> RecoverStaminaByStampSheet(
            Request.RecoverStaminaByStampSheetRequest request
        )
		{
		    var task = new RecoverStaminaByStampSheetTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
#endif


        private class RaiseMaxValueByStampSheetTask : Gs2WebSocketSessionTask<Request.RaiseMaxValueByStampSheetRequest, Result.RaiseMaxValueByStampSheetResult>
        {
	        public RaiseMaxValueByStampSheetTask(IGs2Session session, Request.RaiseMaxValueByStampSheetRequest request) : base(session, request)
	        {
	        }

            protected override IGs2SessionRequest CreateRequest(Request.RaiseMaxValueByStampSheetRequest request)
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
                    "stamina",
                    "stamina",
                    "raiseMaxValueByStampSheet",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator RaiseMaxValueByStampSheet(
                Request.RaiseMaxValueByStampSheetRequest request,
                UnityAction<AsyncResult<Result.RaiseMaxValueByStampSheetResult>> callback
        )
		{
			var task = new RaiseMaxValueByStampSheetTask(
			    Gs2WebSocketSession,
			    request
            );
            yield return task;
            callback.Invoke(new AsyncResult<Result.RaiseMaxValueByStampSheetResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.RaiseMaxValueByStampSheetResult> RaiseMaxValueByStampSheet(
            Request.RaiseMaxValueByStampSheetRequest request
        )
		{
		    var task = new RaiseMaxValueByStampSheetTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
#endif


        private class SetMaxValueByStampSheetTask : Gs2WebSocketSessionTask<Request.SetMaxValueByStampSheetRequest, Result.SetMaxValueByStampSheetResult>
        {
	        public SetMaxValueByStampSheetTask(IGs2Session session, Request.SetMaxValueByStampSheetRequest request) : base(session, request)
	        {
	        }

            protected override IGs2SessionRequest CreateRequest(Request.SetMaxValueByStampSheetRequest request)
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
                    "stamina",
                    "stamina",
                    "setMaxValueByStampSheet",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator SetMaxValueByStampSheet(
                Request.SetMaxValueByStampSheetRequest request,
                UnityAction<AsyncResult<Result.SetMaxValueByStampSheetResult>> callback
        )
		{
			var task = new SetMaxValueByStampSheetTask(
			    Gs2WebSocketSession,
			    request
            );
            yield return task;
            callback.Invoke(new AsyncResult<Result.SetMaxValueByStampSheetResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.SetMaxValueByStampSheetResult> SetMaxValueByStampSheet(
            Request.SetMaxValueByStampSheetRequest request
        )
		{
		    var task = new SetMaxValueByStampSheetTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
#endif


        private class SetRecoverIntervalByStampSheetTask : Gs2WebSocketSessionTask<Request.SetRecoverIntervalByStampSheetRequest, Result.SetRecoverIntervalByStampSheetResult>
        {
	        public SetRecoverIntervalByStampSheetTask(IGs2Session session, Request.SetRecoverIntervalByStampSheetRequest request) : base(session, request)
	        {
	        }

            protected override IGs2SessionRequest CreateRequest(Request.SetRecoverIntervalByStampSheetRequest request)
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
                    "stamina",
                    "stamina",
                    "setRecoverIntervalByStampSheet",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator SetRecoverIntervalByStampSheet(
                Request.SetRecoverIntervalByStampSheetRequest request,
                UnityAction<AsyncResult<Result.SetRecoverIntervalByStampSheetResult>> callback
        )
		{
			var task = new SetRecoverIntervalByStampSheetTask(
			    Gs2WebSocketSession,
			    request
            );
            yield return task;
            callback.Invoke(new AsyncResult<Result.SetRecoverIntervalByStampSheetResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.SetRecoverIntervalByStampSheetResult> SetRecoverIntervalByStampSheet(
            Request.SetRecoverIntervalByStampSheetRequest request
        )
		{
		    var task = new SetRecoverIntervalByStampSheetTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
#endif


        private class SetRecoverValueByStampSheetTask : Gs2WebSocketSessionTask<Request.SetRecoverValueByStampSheetRequest, Result.SetRecoverValueByStampSheetResult>
        {
	        public SetRecoverValueByStampSheetTask(IGs2Session session, Request.SetRecoverValueByStampSheetRequest request) : base(session, request)
	        {
	        }

            protected override IGs2SessionRequest CreateRequest(Request.SetRecoverValueByStampSheetRequest request)
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
                    "stamina",
                    "stamina",
                    "setRecoverValueByStampSheet",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator SetRecoverValueByStampSheet(
                Request.SetRecoverValueByStampSheetRequest request,
                UnityAction<AsyncResult<Result.SetRecoverValueByStampSheetResult>> callback
        )
		{
			var task = new SetRecoverValueByStampSheetTask(
			    Gs2WebSocketSession,
			    request
            );
            yield return task;
            callback.Invoke(new AsyncResult<Result.SetRecoverValueByStampSheetResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.SetRecoverValueByStampSheetResult> SetRecoverValueByStampSheet(
            Request.SetRecoverValueByStampSheetRequest request
        )
		{
		    var task = new SetRecoverValueByStampSheetTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
#endif
	}
}