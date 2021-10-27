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

namespace Gs2.Gs2Experience
{
	public class Gs2ExperienceWebSocketClient : AbstractGs2Client
	{

		public static string Endpoint = "experience";

        protected Gs2WebSocketSession Gs2WebSocketSession => (Gs2WebSocketSession) Gs2Session;

		public Gs2ExperienceWebSocketClient(Gs2WebSocketSession Gs2WebSocketSession) : base(Gs2WebSocketSession)
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
                if (request.ExperienceCapScriptId != null)
                {
                    jsonWriter.WritePropertyName("experienceCapScriptId");
                    jsonWriter.Write(request.ExperienceCapScriptId.ToString());
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
                    request.OverflowExperienceScript.WriteJson(jsonWriter);
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
                if (request.ExperienceCapScriptId != null)
                {
                    jsonWriter.WritePropertyName("experienceCapScriptId");
                    jsonWriter.Write(request.ExperienceCapScriptId.ToString());
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
                    request.OverflowExperienceScript.WriteJson(jsonWriter);
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


        private class CreateExperienceModelMasterTask : Gs2WebSocketSessionTask<Request.CreateExperienceModelMasterRequest, Result.CreateExperienceModelMasterResult>
        {
	        public CreateExperienceModelMasterTask(IGs2Session session, Request.CreateExperienceModelMasterRequest request) : base(session, request)
	        {
	        }

            protected override IGs2SessionRequest CreateRequest(Request.CreateExperienceModelMasterRequest request)
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
                if (request.DefaultExperience != null)
                {
                    jsonWriter.WritePropertyName("defaultExperience");
                    jsonWriter.Write(request.DefaultExperience.ToString());
                }
                if (request.DefaultRankCap != null)
                {
                    jsonWriter.WritePropertyName("defaultRankCap");
                    jsonWriter.Write(request.DefaultRankCap.ToString());
                }
                if (request.MaxRankCap != null)
                {
                    jsonWriter.WritePropertyName("maxRankCap");
                    jsonWriter.Write(request.MaxRankCap.ToString());
                }
                if (request.RankThresholdName != null)
                {
                    jsonWriter.WritePropertyName("rankThresholdName");
                    jsonWriter.Write(request.RankThresholdName.ToString());
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
                    "experience",
                    "experienceModelMaster",
                    "createExperienceModelMaster",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator CreateExperienceModelMaster(
                Request.CreateExperienceModelMasterRequest request,
                UnityAction<AsyncResult<Result.CreateExperienceModelMasterResult>> callback
        )
		{
			var task = new CreateExperienceModelMasterTask(
			    Gs2WebSocketSession,
			    request
            );
            yield return task;
            callback.Invoke(new AsyncResult<Result.CreateExperienceModelMasterResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.CreateExperienceModelMasterResult> CreateExperienceModelMaster(
            Request.CreateExperienceModelMasterRequest request
        )
		{
		    var task = new CreateExperienceModelMasterTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
#endif


        private class GetExperienceModelMasterTask : Gs2WebSocketSessionTask<Request.GetExperienceModelMasterRequest, Result.GetExperienceModelMasterResult>
        {
	        public GetExperienceModelMasterTask(IGs2Session session, Request.GetExperienceModelMasterRequest request) : base(session, request)
	        {
	        }

            protected override IGs2SessionRequest CreateRequest(Request.GetExperienceModelMasterRequest request)
            {
                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);

                jsonWriter.WriteObjectStart();

                if (request.NamespaceName != null)
                {
                    jsonWriter.WritePropertyName("namespaceName");
                    jsonWriter.Write(request.NamespaceName.ToString());
                }
                if (request.ExperienceName != null)
                {
                    jsonWriter.WritePropertyName("experienceName");
                    jsonWriter.Write(request.ExperienceName.ToString());
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
                    "experience",
                    "experienceModelMaster",
                    "getExperienceModelMaster",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator GetExperienceModelMaster(
                Request.GetExperienceModelMasterRequest request,
                UnityAction<AsyncResult<Result.GetExperienceModelMasterResult>> callback
        )
		{
			var task = new GetExperienceModelMasterTask(
			    Gs2WebSocketSession,
			    request
            );
            yield return task;
            callback.Invoke(new AsyncResult<Result.GetExperienceModelMasterResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.GetExperienceModelMasterResult> GetExperienceModelMaster(
            Request.GetExperienceModelMasterRequest request
        )
		{
		    var task = new GetExperienceModelMasterTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
#endif


        private class UpdateExperienceModelMasterTask : Gs2WebSocketSessionTask<Request.UpdateExperienceModelMasterRequest, Result.UpdateExperienceModelMasterResult>
        {
	        public UpdateExperienceModelMasterTask(IGs2Session session, Request.UpdateExperienceModelMasterRequest request) : base(session, request)
	        {
	        }

            protected override IGs2SessionRequest CreateRequest(Request.UpdateExperienceModelMasterRequest request)
            {
                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);

                jsonWriter.WriteObjectStart();

                if (request.NamespaceName != null)
                {
                    jsonWriter.WritePropertyName("namespaceName");
                    jsonWriter.Write(request.NamespaceName.ToString());
                }
                if (request.ExperienceName != null)
                {
                    jsonWriter.WritePropertyName("experienceName");
                    jsonWriter.Write(request.ExperienceName.ToString());
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
                if (request.DefaultExperience != null)
                {
                    jsonWriter.WritePropertyName("defaultExperience");
                    jsonWriter.Write(request.DefaultExperience.ToString());
                }
                if (request.DefaultRankCap != null)
                {
                    jsonWriter.WritePropertyName("defaultRankCap");
                    jsonWriter.Write(request.DefaultRankCap.ToString());
                }
                if (request.MaxRankCap != null)
                {
                    jsonWriter.WritePropertyName("maxRankCap");
                    jsonWriter.Write(request.MaxRankCap.ToString());
                }
                if (request.RankThresholdName != null)
                {
                    jsonWriter.WritePropertyName("rankThresholdName");
                    jsonWriter.Write(request.RankThresholdName.ToString());
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
                    "experience",
                    "experienceModelMaster",
                    "updateExperienceModelMaster",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator UpdateExperienceModelMaster(
                Request.UpdateExperienceModelMasterRequest request,
                UnityAction<AsyncResult<Result.UpdateExperienceModelMasterResult>> callback
        )
		{
			var task = new UpdateExperienceModelMasterTask(
			    Gs2WebSocketSession,
			    request
            );
            yield return task;
            callback.Invoke(new AsyncResult<Result.UpdateExperienceModelMasterResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.UpdateExperienceModelMasterResult> UpdateExperienceModelMaster(
            Request.UpdateExperienceModelMasterRequest request
        )
		{
		    var task = new UpdateExperienceModelMasterTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
#endif


        private class DeleteExperienceModelMasterTask : Gs2WebSocketSessionTask<Request.DeleteExperienceModelMasterRequest, Result.DeleteExperienceModelMasterResult>
        {
	        public DeleteExperienceModelMasterTask(IGs2Session session, Request.DeleteExperienceModelMasterRequest request) : base(session, request)
	        {
	        }

            protected override IGs2SessionRequest CreateRequest(Request.DeleteExperienceModelMasterRequest request)
            {
                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);

                jsonWriter.WriteObjectStart();

                if (request.NamespaceName != null)
                {
                    jsonWriter.WritePropertyName("namespaceName");
                    jsonWriter.Write(request.NamespaceName.ToString());
                }
                if (request.ExperienceName != null)
                {
                    jsonWriter.WritePropertyName("experienceName");
                    jsonWriter.Write(request.ExperienceName.ToString());
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
                    "experience",
                    "experienceModelMaster",
                    "deleteExperienceModelMaster",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator DeleteExperienceModelMaster(
                Request.DeleteExperienceModelMasterRequest request,
                UnityAction<AsyncResult<Result.DeleteExperienceModelMasterResult>> callback
        )
		{
			var task = new DeleteExperienceModelMasterTask(
			    Gs2WebSocketSession,
			    request
            );
            yield return task;
            callback.Invoke(new AsyncResult<Result.DeleteExperienceModelMasterResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.DeleteExperienceModelMasterResult> DeleteExperienceModelMaster(
            Request.DeleteExperienceModelMasterRequest request
        )
		{
		    var task = new DeleteExperienceModelMasterTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
#endif


        private class GetExperienceModelTask : Gs2WebSocketSessionTask<Request.GetExperienceModelRequest, Result.GetExperienceModelResult>
        {
	        public GetExperienceModelTask(IGs2Session session, Request.GetExperienceModelRequest request) : base(session, request)
	        {
	        }

            protected override IGs2SessionRequest CreateRequest(Request.GetExperienceModelRequest request)
            {
                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);

                jsonWriter.WriteObjectStart();

                if (request.NamespaceName != null)
                {
                    jsonWriter.WritePropertyName("namespaceName");
                    jsonWriter.Write(request.NamespaceName.ToString());
                }
                if (request.ExperienceName != null)
                {
                    jsonWriter.WritePropertyName("experienceName");
                    jsonWriter.Write(request.ExperienceName.ToString());
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
                    "experience",
                    "experienceModel",
                    "getExperienceModel",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator GetExperienceModel(
                Request.GetExperienceModelRequest request,
                UnityAction<AsyncResult<Result.GetExperienceModelResult>> callback
        )
		{
			var task = new GetExperienceModelTask(
			    Gs2WebSocketSession,
			    request
            );
            yield return task;
            callback.Invoke(new AsyncResult<Result.GetExperienceModelResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.GetExperienceModelResult> GetExperienceModel(
            Request.GetExperienceModelRequest request
        )
		{
		    var task = new GetExperienceModelTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
#endif


        private class CreateThresholdMasterTask : Gs2WebSocketSessionTask<Request.CreateThresholdMasterRequest, Result.CreateThresholdMasterResult>
        {
	        public CreateThresholdMasterTask(IGs2Session session, Request.CreateThresholdMasterRequest request) : base(session, request)
	        {
	        }

            protected override IGs2SessionRequest CreateRequest(Request.CreateThresholdMasterRequest request)
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
                    "experience",
                    "thresholdMaster",
                    "createThresholdMaster",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator CreateThresholdMaster(
                Request.CreateThresholdMasterRequest request,
                UnityAction<AsyncResult<Result.CreateThresholdMasterResult>> callback
        )
		{
			var task = new CreateThresholdMasterTask(
			    Gs2WebSocketSession,
			    request
            );
            yield return task;
            callback.Invoke(new AsyncResult<Result.CreateThresholdMasterResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.CreateThresholdMasterResult> CreateThresholdMaster(
            Request.CreateThresholdMasterRequest request
        )
		{
		    var task = new CreateThresholdMasterTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
#endif


        private class GetThresholdMasterTask : Gs2WebSocketSessionTask<Request.GetThresholdMasterRequest, Result.GetThresholdMasterResult>
        {
	        public GetThresholdMasterTask(IGs2Session session, Request.GetThresholdMasterRequest request) : base(session, request)
	        {
	        }

            protected override IGs2SessionRequest CreateRequest(Request.GetThresholdMasterRequest request)
            {
                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);

                jsonWriter.WriteObjectStart();

                if (request.NamespaceName != null)
                {
                    jsonWriter.WritePropertyName("namespaceName");
                    jsonWriter.Write(request.NamespaceName.ToString());
                }
                if (request.ThresholdName != null)
                {
                    jsonWriter.WritePropertyName("thresholdName");
                    jsonWriter.Write(request.ThresholdName.ToString());
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
                    "experience",
                    "thresholdMaster",
                    "getThresholdMaster",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator GetThresholdMaster(
                Request.GetThresholdMasterRequest request,
                UnityAction<AsyncResult<Result.GetThresholdMasterResult>> callback
        )
		{
			var task = new GetThresholdMasterTask(
			    Gs2WebSocketSession,
			    request
            );
            yield return task;
            callback.Invoke(new AsyncResult<Result.GetThresholdMasterResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.GetThresholdMasterResult> GetThresholdMaster(
            Request.GetThresholdMasterRequest request
        )
		{
		    var task = new GetThresholdMasterTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
#endif


        private class UpdateThresholdMasterTask : Gs2WebSocketSessionTask<Request.UpdateThresholdMasterRequest, Result.UpdateThresholdMasterResult>
        {
	        public UpdateThresholdMasterTask(IGs2Session session, Request.UpdateThresholdMasterRequest request) : base(session, request)
	        {
	        }

            protected override IGs2SessionRequest CreateRequest(Request.UpdateThresholdMasterRequest request)
            {
                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);

                jsonWriter.WriteObjectStart();

                if (request.NamespaceName != null)
                {
                    jsonWriter.WritePropertyName("namespaceName");
                    jsonWriter.Write(request.NamespaceName.ToString());
                }
                if (request.ThresholdName != null)
                {
                    jsonWriter.WritePropertyName("thresholdName");
                    jsonWriter.Write(request.ThresholdName.ToString());
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
                    "experience",
                    "thresholdMaster",
                    "updateThresholdMaster",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator UpdateThresholdMaster(
                Request.UpdateThresholdMasterRequest request,
                UnityAction<AsyncResult<Result.UpdateThresholdMasterResult>> callback
        )
		{
			var task = new UpdateThresholdMasterTask(
			    Gs2WebSocketSession,
			    request
            );
            yield return task;
            callback.Invoke(new AsyncResult<Result.UpdateThresholdMasterResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.UpdateThresholdMasterResult> UpdateThresholdMaster(
            Request.UpdateThresholdMasterRequest request
        )
		{
		    var task = new UpdateThresholdMasterTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
#endif


        private class DeleteThresholdMasterTask : Gs2WebSocketSessionTask<Request.DeleteThresholdMasterRequest, Result.DeleteThresholdMasterResult>
        {
	        public DeleteThresholdMasterTask(IGs2Session session, Request.DeleteThresholdMasterRequest request) : base(session, request)
	        {
	        }

            protected override IGs2SessionRequest CreateRequest(Request.DeleteThresholdMasterRequest request)
            {
                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);

                jsonWriter.WriteObjectStart();

                if (request.NamespaceName != null)
                {
                    jsonWriter.WritePropertyName("namespaceName");
                    jsonWriter.Write(request.NamespaceName.ToString());
                }
                if (request.ThresholdName != null)
                {
                    jsonWriter.WritePropertyName("thresholdName");
                    jsonWriter.Write(request.ThresholdName.ToString());
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
                    "experience",
                    "thresholdMaster",
                    "deleteThresholdMaster",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator DeleteThresholdMaster(
                Request.DeleteThresholdMasterRequest request,
                UnityAction<AsyncResult<Result.DeleteThresholdMasterResult>> callback
        )
		{
			var task = new DeleteThresholdMasterTask(
			    Gs2WebSocketSession,
			    request
            );
            yield return task;
            callback.Invoke(new AsyncResult<Result.DeleteThresholdMasterResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.DeleteThresholdMasterResult> DeleteThresholdMaster(
            Request.DeleteThresholdMasterRequest request
        )
		{
		    var task = new DeleteThresholdMasterTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
#endif


        private class GetStatusTask : Gs2WebSocketSessionTask<Request.GetStatusRequest, Result.GetStatusResult>
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
        )
		{
			var task = new GetStatusTask(
			    Gs2WebSocketSession,
			    request
            );
            yield return task;
            callback.Invoke(new AsyncResult<Result.GetStatusResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.GetStatusResult> GetStatus(
            Request.GetStatusRequest request
        )
		{
		    var task = new GetStatusTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
#endif


        private class GetStatusByUserIdTask : Gs2WebSocketSessionTask<Request.GetStatusByUserIdRequest, Result.GetStatusByUserIdResult>
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
        )
		{
			var task = new GetStatusByUserIdTask(
			    Gs2WebSocketSession,
			    request
            );
            yield return task;
            callback.Invoke(new AsyncResult<Result.GetStatusByUserIdResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.GetStatusByUserIdResult> GetStatusByUserId(
            Request.GetStatusByUserIdRequest request
        )
		{
		    var task = new GetStatusByUserIdTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
#endif


        private class AddExperienceByUserIdTask : Gs2WebSocketSessionTask<Request.AddExperienceByUserIdRequest, Result.AddExperienceByUserIdResult>
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
        )
		{
			var task = new AddExperienceByUserIdTask(
			    Gs2WebSocketSession,
			    request
            );
            yield return task;
            callback.Invoke(new AsyncResult<Result.AddExperienceByUserIdResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.AddExperienceByUserIdResult> AddExperienceByUserId(
            Request.AddExperienceByUserIdRequest request
        )
		{
		    var task = new AddExperienceByUserIdTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
#endif


        private class SetExperienceByUserIdTask : Gs2WebSocketSessionTask<Request.SetExperienceByUserIdRequest, Result.SetExperienceByUserIdResult>
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
        )
		{
			var task = new SetExperienceByUserIdTask(
			    Gs2WebSocketSession,
			    request
            );
            yield return task;
            callback.Invoke(new AsyncResult<Result.SetExperienceByUserIdResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.SetExperienceByUserIdResult> SetExperienceByUserId(
            Request.SetExperienceByUserIdRequest request
        )
		{
		    var task = new SetExperienceByUserIdTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
#endif


        private class AddRankCapByUserIdTask : Gs2WebSocketSessionTask<Request.AddRankCapByUserIdRequest, Result.AddRankCapByUserIdResult>
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
        )
		{
			var task = new AddRankCapByUserIdTask(
			    Gs2WebSocketSession,
			    request
            );
            yield return task;
            callback.Invoke(new AsyncResult<Result.AddRankCapByUserIdResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.AddRankCapByUserIdResult> AddRankCapByUserId(
            Request.AddRankCapByUserIdRequest request
        )
		{
		    var task = new AddRankCapByUserIdTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
#endif


        private class SetRankCapByUserIdTask : Gs2WebSocketSessionTask<Request.SetRankCapByUserIdRequest, Result.SetRankCapByUserIdResult>
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
        )
		{
			var task = new SetRankCapByUserIdTask(
			    Gs2WebSocketSession,
			    request
            );
            yield return task;
            callback.Invoke(new AsyncResult<Result.SetRankCapByUserIdResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.SetRankCapByUserIdResult> SetRankCapByUserId(
            Request.SetRankCapByUserIdRequest request
        )
		{
		    var task = new SetRankCapByUserIdTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
#endif


        private class DeleteStatusByUserIdTask : Gs2WebSocketSessionTask<Request.DeleteStatusByUserIdRequest, Result.DeleteStatusByUserIdResult>
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
        )
		{
			var task = new DeleteStatusByUserIdTask(
			    Gs2WebSocketSession,
			    request
            );
            yield return task;
            callback.Invoke(new AsyncResult<Result.DeleteStatusByUserIdResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.DeleteStatusByUserIdResult> DeleteStatusByUserId(
            Request.DeleteStatusByUserIdRequest request
        )
		{
		    var task = new DeleteStatusByUserIdTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
#endif


        private class AddExperienceByStampSheetTask : Gs2WebSocketSessionTask<Request.AddExperienceByStampSheetRequest, Result.AddExperienceByStampSheetResult>
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
                if (request.RequestId != null)
                {
                    jsonWriter.WritePropertyName("xGs2RequestId");
                    jsonWriter.Write(request.RequestId);
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
        )
		{
			var task = new AddExperienceByStampSheetTask(
			    Gs2WebSocketSession,
			    request
            );
            yield return task;
            callback.Invoke(new AsyncResult<Result.AddExperienceByStampSheetResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.AddExperienceByStampSheetResult> AddExperienceByStampSheet(
            Request.AddExperienceByStampSheetRequest request
        )
		{
		    var task = new AddExperienceByStampSheetTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
#endif


        private class AddRankCapByStampSheetTask : Gs2WebSocketSessionTask<Request.AddRankCapByStampSheetRequest, Result.AddRankCapByStampSheetResult>
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
                if (request.RequestId != null)
                {
                    jsonWriter.WritePropertyName("xGs2RequestId");
                    jsonWriter.Write(request.RequestId);
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
        )
		{
			var task = new AddRankCapByStampSheetTask(
			    Gs2WebSocketSession,
			    request
            );
            yield return task;
            callback.Invoke(new AsyncResult<Result.AddRankCapByStampSheetResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.AddRankCapByStampSheetResult> AddRankCapByStampSheet(
            Request.AddRankCapByStampSheetRequest request
        )
		{
		    var task = new AddRankCapByStampSheetTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
#endif


        private class SetRankCapByStampSheetTask : Gs2WebSocketSessionTask<Request.SetRankCapByStampSheetRequest, Result.SetRankCapByStampSheetResult>
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
                if (request.RequestId != null)
                {
                    jsonWriter.WritePropertyName("xGs2RequestId");
                    jsonWriter.Write(request.RequestId);
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
        )
		{
			var task = new SetRankCapByStampSheetTask(
			    Gs2WebSocketSession,
			    request
            );
            yield return task;
            callback.Invoke(new AsyncResult<Result.SetRankCapByStampSheetResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.SetRankCapByStampSheetResult> SetRankCapByStampSheet(
            Request.SetRankCapByStampSheetRequest request
        )
		{
		    var task = new SetRankCapByStampSheetTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
#endif
	}
}