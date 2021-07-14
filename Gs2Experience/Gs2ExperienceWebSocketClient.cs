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
using UnityEngine.Events;
using UnityEngine.Networking;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Gs2.Core;
using Gs2.Core.Model;
using Gs2.Core.Net;
using Gs2.Util.LitJson;namespace Gs2.Gs2Experience
{
	public class Gs2ExperienceWebSocketClient : AbstractGs2Client
	{

		public static string Endpoint = "experience";

        protected Gs2WebSocketSession Gs2WebSocketSession => (Gs2WebSocketSession) Gs2Session;

		public Gs2ExperienceWebSocketClient(Gs2WebSocketSession Gs2WebSocketSession) : base(Gs2WebSocketSession)
		{

		}

        private class CreateNamespaceTask : Gs2WebSocketSessionTask<Result.CreateNamespaceResult>
        {
			private readonly Request.CreateNamespaceRequest _request;

			public CreateNamespaceTask(Request.CreateNamespaceRequest request, UnityAction<AsyncResult<Result.CreateNamespaceResult>> userCallback) : base(userCallback)
			{
				_request = request;
			}

            protected override IEnumerator ExecuteImpl(Gs2Session gs2Session)
            {
                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);

                jsonWriter.WriteObjectStart();

                if (_request.Name != null)
                {
                    jsonWriter.WritePropertyName("name");
                    jsonWriter.Write(_request.Name.ToString());
                }
                if (_request.Description != null)
                {
                    jsonWriter.WritePropertyName("description");
                    jsonWriter.Write(_request.Description.ToString());
                }
                if (_request.ExperienceCapScriptId != null)
                {
                    jsonWriter.WritePropertyName("experienceCapScriptId");
                    jsonWriter.Write(_request.ExperienceCapScriptId.ToString());
                }
                if (_request.ChangeExperienceScript != null)
                {
                    jsonWriter.WritePropertyName("changeExperienceScript");
                    _request.ChangeExperienceScript.WriteJson(jsonWriter);
                }
                if (_request.ChangeRankScript != null)
                {
                    jsonWriter.WritePropertyName("changeRankScript");
                    _request.ChangeRankScript.WriteJson(jsonWriter);
                }
                if (_request.ChangeRankCapScript != null)
                {
                    jsonWriter.WritePropertyName("changeRankCapScript");
                    _request.ChangeRankCapScript.WriteJson(jsonWriter);
                }
                if (_request.OverflowExperienceScript != null)
                {
                    jsonWriter.WritePropertyName("overflowExperienceScript");
                    _request.OverflowExperienceScript.WriteJson(jsonWriter);
                }
                if (_request.LogSetting != null)
                {
                    jsonWriter.WritePropertyName("logSetting");
                    _request.LogSetting.WriteJson(jsonWriter);
                }
                if (_request.ContextStack != null)
                {
                    jsonWriter.WritePropertyName("contextStack");
                    jsonWriter.Write(_request.ContextStack.ToString());
                }
                if (_request.RequestId != null)
                {
                    jsonWriter.WritePropertyName("xGs2RequestId");
                    jsonWriter.Write(_request.RequestId);
                }

                jsonWriter.WritePropertyName("xGs2ClientId");
                jsonWriter.Write(gs2Session.Credential.ClientId);
                jsonWriter.WritePropertyName("xGs2ProjectToken");
                jsonWriter.Write(gs2Session.ProjectToken);

                jsonWriter.WritePropertyName("x_gs2");
                jsonWriter.WriteObjectStart();
                jsonWriter.WritePropertyName("service");
                jsonWriter.Write("experience");
                jsonWriter.WritePropertyName("component");
                jsonWriter.Write("namespace");
                jsonWriter.WritePropertyName("function");
                jsonWriter.Write("createNamespace");
                jsonWriter.WritePropertyName("contentType");
                jsonWriter.Write("application/json");
                jsonWriter.WritePropertyName("requestId");
                jsonWriter.Write(Gs2SessionTaskId.ToString());
                jsonWriter.WriteObjectEnd();

                jsonWriter.WriteObjectEnd();

                ((Gs2WebSocketSession)gs2Session).Send(stringBuilder.ToString());

                return new EmptyCoroutine();
            }
        }

		public IEnumerator CreateNamespace(
                Request.CreateNamespaceRequest request,
                UnityAction<AsyncResult<Result.CreateNamespaceResult>> callback
        )
		{
			var task = new CreateNamespaceTask(request, callback);
			return Gs2WebSocketSession.Execute(task);
        }

        private class GetNamespaceTask : Gs2WebSocketSessionTask<Result.GetNamespaceResult>
        {
			private readonly Request.GetNamespaceRequest _request;

			public GetNamespaceTask(Request.GetNamespaceRequest request, UnityAction<AsyncResult<Result.GetNamespaceResult>> userCallback) : base(userCallback)
			{
				_request = request;
			}

            protected override IEnumerator ExecuteImpl(Gs2Session gs2Session)
            {
                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);

                jsonWriter.WriteObjectStart();

                if (_request.NamespaceName != null)
                {
                    jsonWriter.WritePropertyName("namespaceName");
                    jsonWriter.Write(_request.NamespaceName.ToString());
                }
                if (_request.ContextStack != null)
                {
                    jsonWriter.WritePropertyName("contextStack");
                    jsonWriter.Write(_request.ContextStack.ToString());
                }
                if (_request.RequestId != null)
                {
                    jsonWriter.WritePropertyName("xGs2RequestId");
                    jsonWriter.Write(_request.RequestId);
                }

                jsonWriter.WritePropertyName("xGs2ClientId");
                jsonWriter.Write(gs2Session.Credential.ClientId);
                jsonWriter.WritePropertyName("xGs2ProjectToken");
                jsonWriter.Write(gs2Session.ProjectToken);

                jsonWriter.WritePropertyName("x_gs2");
                jsonWriter.WriteObjectStart();
                jsonWriter.WritePropertyName("service");
                jsonWriter.Write("experience");
                jsonWriter.WritePropertyName("component");
                jsonWriter.Write("namespace");
                jsonWriter.WritePropertyName("function");
                jsonWriter.Write("getNamespace");
                jsonWriter.WritePropertyName("contentType");
                jsonWriter.Write("application/json");
                jsonWriter.WritePropertyName("requestId");
                jsonWriter.Write(Gs2SessionTaskId.ToString());
                jsonWriter.WriteObjectEnd();

                jsonWriter.WriteObjectEnd();

                ((Gs2WebSocketSession)gs2Session).Send(stringBuilder.ToString());

                return new EmptyCoroutine();
            }
        }

		public IEnumerator GetNamespace(
                Request.GetNamespaceRequest request,
                UnityAction<AsyncResult<Result.GetNamespaceResult>> callback
        )
		{
			var task = new GetNamespaceTask(request, callback);
			return Gs2WebSocketSession.Execute(task);
        }

        private class UpdateNamespaceTask : Gs2WebSocketSessionTask<Result.UpdateNamespaceResult>
        {
			private readonly Request.UpdateNamespaceRequest _request;

			public UpdateNamespaceTask(Request.UpdateNamespaceRequest request, UnityAction<AsyncResult<Result.UpdateNamespaceResult>> userCallback) : base(userCallback)
			{
				_request = request;
			}

            protected override IEnumerator ExecuteImpl(Gs2Session gs2Session)
            {
                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);

                jsonWriter.WriteObjectStart();

                if (_request.NamespaceName != null)
                {
                    jsonWriter.WritePropertyName("namespaceName");
                    jsonWriter.Write(_request.NamespaceName.ToString());
                }
                if (_request.Description != null)
                {
                    jsonWriter.WritePropertyName("description");
                    jsonWriter.Write(_request.Description.ToString());
                }
                if (_request.ExperienceCapScriptId != null)
                {
                    jsonWriter.WritePropertyName("experienceCapScriptId");
                    jsonWriter.Write(_request.ExperienceCapScriptId.ToString());
                }
                if (_request.ChangeExperienceScript != null)
                {
                    jsonWriter.WritePropertyName("changeExperienceScript");
                    _request.ChangeExperienceScript.WriteJson(jsonWriter);
                }
                if (_request.ChangeRankScript != null)
                {
                    jsonWriter.WritePropertyName("changeRankScript");
                    _request.ChangeRankScript.WriteJson(jsonWriter);
                }
                if (_request.ChangeRankCapScript != null)
                {
                    jsonWriter.WritePropertyName("changeRankCapScript");
                    _request.ChangeRankCapScript.WriteJson(jsonWriter);
                }
                if (_request.OverflowExperienceScript != null)
                {
                    jsonWriter.WritePropertyName("overflowExperienceScript");
                    _request.OverflowExperienceScript.WriteJson(jsonWriter);
                }
                if (_request.LogSetting != null)
                {
                    jsonWriter.WritePropertyName("logSetting");
                    _request.LogSetting.WriteJson(jsonWriter);
                }
                if (_request.ContextStack != null)
                {
                    jsonWriter.WritePropertyName("contextStack");
                    jsonWriter.Write(_request.ContextStack.ToString());
                }
                if (_request.RequestId != null)
                {
                    jsonWriter.WritePropertyName("xGs2RequestId");
                    jsonWriter.Write(_request.RequestId);
                }

                jsonWriter.WritePropertyName("xGs2ClientId");
                jsonWriter.Write(gs2Session.Credential.ClientId);
                jsonWriter.WritePropertyName("xGs2ProjectToken");
                jsonWriter.Write(gs2Session.ProjectToken);

                jsonWriter.WritePropertyName("x_gs2");
                jsonWriter.WriteObjectStart();
                jsonWriter.WritePropertyName("service");
                jsonWriter.Write("experience");
                jsonWriter.WritePropertyName("component");
                jsonWriter.Write("namespace");
                jsonWriter.WritePropertyName("function");
                jsonWriter.Write("updateNamespace");
                jsonWriter.WritePropertyName("contentType");
                jsonWriter.Write("application/json");
                jsonWriter.WritePropertyName("requestId");
                jsonWriter.Write(Gs2SessionTaskId.ToString());
                jsonWriter.WriteObjectEnd();

                jsonWriter.WriteObjectEnd();

                ((Gs2WebSocketSession)gs2Session).Send(stringBuilder.ToString());

                return new EmptyCoroutine();
            }
        }

		public IEnumerator UpdateNamespace(
                Request.UpdateNamespaceRequest request,
                UnityAction<AsyncResult<Result.UpdateNamespaceResult>> callback
        )
		{
			var task = new UpdateNamespaceTask(request, callback);
			return Gs2WebSocketSession.Execute(task);
        }

        private class DeleteNamespaceTask : Gs2WebSocketSessionTask<Result.DeleteNamespaceResult>
        {
			private readonly Request.DeleteNamespaceRequest _request;

			public DeleteNamespaceTask(Request.DeleteNamespaceRequest request, UnityAction<AsyncResult<Result.DeleteNamespaceResult>> userCallback) : base(userCallback)
			{
				_request = request;
			}

            protected override IEnumerator ExecuteImpl(Gs2Session gs2Session)
            {
                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);

                jsonWriter.WriteObjectStart();

                if (_request.NamespaceName != null)
                {
                    jsonWriter.WritePropertyName("namespaceName");
                    jsonWriter.Write(_request.NamespaceName.ToString());
                }
                if (_request.ContextStack != null)
                {
                    jsonWriter.WritePropertyName("contextStack");
                    jsonWriter.Write(_request.ContextStack.ToString());
                }
                if (_request.RequestId != null)
                {
                    jsonWriter.WritePropertyName("xGs2RequestId");
                    jsonWriter.Write(_request.RequestId);
                }

                jsonWriter.WritePropertyName("xGs2ClientId");
                jsonWriter.Write(gs2Session.Credential.ClientId);
                jsonWriter.WritePropertyName("xGs2ProjectToken");
                jsonWriter.Write(gs2Session.ProjectToken);

                jsonWriter.WritePropertyName("x_gs2");
                jsonWriter.WriteObjectStart();
                jsonWriter.WritePropertyName("service");
                jsonWriter.Write("experience");
                jsonWriter.WritePropertyName("component");
                jsonWriter.Write("namespace");
                jsonWriter.WritePropertyName("function");
                jsonWriter.Write("deleteNamespace");
                jsonWriter.WritePropertyName("contentType");
                jsonWriter.Write("application/json");
                jsonWriter.WritePropertyName("requestId");
                jsonWriter.Write(Gs2SessionTaskId.ToString());
                jsonWriter.WriteObjectEnd();

                jsonWriter.WriteObjectEnd();

                ((Gs2WebSocketSession)gs2Session).Send(stringBuilder.ToString());

                return new EmptyCoroutine();
            }
        }

		public IEnumerator DeleteNamespace(
                Request.DeleteNamespaceRequest request,
                UnityAction<AsyncResult<Result.DeleteNamespaceResult>> callback
        )
		{
			var task = new DeleteNamespaceTask(request, callback);
			return Gs2WebSocketSession.Execute(task);
        }

        private class CreateExperienceModelMasterTask : Gs2WebSocketSessionTask<Result.CreateExperienceModelMasterResult>
        {
			private readonly Request.CreateExperienceModelMasterRequest _request;

			public CreateExperienceModelMasterTask(Request.CreateExperienceModelMasterRequest request, UnityAction<AsyncResult<Result.CreateExperienceModelMasterResult>> userCallback) : base(userCallback)
			{
				_request = request;
			}

            protected override IEnumerator ExecuteImpl(Gs2Session gs2Session)
            {
                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);

                jsonWriter.WriteObjectStart();

                if (_request.NamespaceName != null)
                {
                    jsonWriter.WritePropertyName("namespaceName");
                    jsonWriter.Write(_request.NamespaceName.ToString());
                }
                if (_request.Name != null)
                {
                    jsonWriter.WritePropertyName("name");
                    jsonWriter.Write(_request.Name.ToString());
                }
                if (_request.Description != null)
                {
                    jsonWriter.WritePropertyName("description");
                    jsonWriter.Write(_request.Description.ToString());
                }
                if (_request.Metadata != null)
                {
                    jsonWriter.WritePropertyName("metadata");
                    jsonWriter.Write(_request.Metadata.ToString());
                }
                if (_request.DefaultExperience != null)
                {
                    jsonWriter.WritePropertyName("defaultExperience");
                    jsonWriter.Write(_request.DefaultExperience.ToString());
                }
                if (_request.DefaultRankCap != null)
                {
                    jsonWriter.WritePropertyName("defaultRankCap");
                    jsonWriter.Write(_request.DefaultRankCap.ToString());
                }
                if (_request.MaxRankCap != null)
                {
                    jsonWriter.WritePropertyName("maxRankCap");
                    jsonWriter.Write(_request.MaxRankCap.ToString());
                }
                if (_request.RankThresholdName != null)
                {
                    jsonWriter.WritePropertyName("rankThresholdName");
                    jsonWriter.Write(_request.RankThresholdName.ToString());
                }
                if (_request.ContextStack != null)
                {
                    jsonWriter.WritePropertyName("contextStack");
                    jsonWriter.Write(_request.ContextStack.ToString());
                }
                if (_request.RequestId != null)
                {
                    jsonWriter.WritePropertyName("xGs2RequestId");
                    jsonWriter.Write(_request.RequestId);
                }

                jsonWriter.WritePropertyName("xGs2ClientId");
                jsonWriter.Write(gs2Session.Credential.ClientId);
                jsonWriter.WritePropertyName("xGs2ProjectToken");
                jsonWriter.Write(gs2Session.ProjectToken);

                jsonWriter.WritePropertyName("x_gs2");
                jsonWriter.WriteObjectStart();
                jsonWriter.WritePropertyName("service");
                jsonWriter.Write("experience");
                jsonWriter.WritePropertyName("component");
                jsonWriter.Write("experienceModelMaster");
                jsonWriter.WritePropertyName("function");
                jsonWriter.Write("createExperienceModelMaster");
                jsonWriter.WritePropertyName("contentType");
                jsonWriter.Write("application/json");
                jsonWriter.WritePropertyName("requestId");
                jsonWriter.Write(Gs2SessionTaskId.ToString());
                jsonWriter.WriteObjectEnd();

                jsonWriter.WriteObjectEnd();

                ((Gs2WebSocketSession)gs2Session).Send(stringBuilder.ToString());

                return new EmptyCoroutine();
            }
        }

		public IEnumerator CreateExperienceModelMaster(
                Request.CreateExperienceModelMasterRequest request,
                UnityAction<AsyncResult<Result.CreateExperienceModelMasterResult>> callback
        )
		{
			var task = new CreateExperienceModelMasterTask(request, callback);
			return Gs2WebSocketSession.Execute(task);
        }

        private class GetExperienceModelMasterTask : Gs2WebSocketSessionTask<Result.GetExperienceModelMasterResult>
        {
			private readonly Request.GetExperienceModelMasterRequest _request;

			public GetExperienceModelMasterTask(Request.GetExperienceModelMasterRequest request, UnityAction<AsyncResult<Result.GetExperienceModelMasterResult>> userCallback) : base(userCallback)
			{
				_request = request;
			}

            protected override IEnumerator ExecuteImpl(Gs2Session gs2Session)
            {
                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);

                jsonWriter.WriteObjectStart();

                if (_request.NamespaceName != null)
                {
                    jsonWriter.WritePropertyName("namespaceName");
                    jsonWriter.Write(_request.NamespaceName.ToString());
                }
                if (_request.ExperienceName != null)
                {
                    jsonWriter.WritePropertyName("experienceName");
                    jsonWriter.Write(_request.ExperienceName.ToString());
                }
                if (_request.ContextStack != null)
                {
                    jsonWriter.WritePropertyName("contextStack");
                    jsonWriter.Write(_request.ContextStack.ToString());
                }
                if (_request.RequestId != null)
                {
                    jsonWriter.WritePropertyName("xGs2RequestId");
                    jsonWriter.Write(_request.RequestId);
                }

                jsonWriter.WritePropertyName("xGs2ClientId");
                jsonWriter.Write(gs2Session.Credential.ClientId);
                jsonWriter.WritePropertyName("xGs2ProjectToken");
                jsonWriter.Write(gs2Session.ProjectToken);

                jsonWriter.WritePropertyName("x_gs2");
                jsonWriter.WriteObjectStart();
                jsonWriter.WritePropertyName("service");
                jsonWriter.Write("experience");
                jsonWriter.WritePropertyName("component");
                jsonWriter.Write("experienceModelMaster");
                jsonWriter.WritePropertyName("function");
                jsonWriter.Write("getExperienceModelMaster");
                jsonWriter.WritePropertyName("contentType");
                jsonWriter.Write("application/json");
                jsonWriter.WritePropertyName("requestId");
                jsonWriter.Write(Gs2SessionTaskId.ToString());
                jsonWriter.WriteObjectEnd();

                jsonWriter.WriteObjectEnd();

                ((Gs2WebSocketSession)gs2Session).Send(stringBuilder.ToString());

                return new EmptyCoroutine();
            }
        }

		public IEnumerator GetExperienceModelMaster(
                Request.GetExperienceModelMasterRequest request,
                UnityAction<AsyncResult<Result.GetExperienceModelMasterResult>> callback
        )
		{
			var task = new GetExperienceModelMasterTask(request, callback);
			return Gs2WebSocketSession.Execute(task);
        }

        private class UpdateExperienceModelMasterTask : Gs2WebSocketSessionTask<Result.UpdateExperienceModelMasterResult>
        {
			private readonly Request.UpdateExperienceModelMasterRequest _request;

			public UpdateExperienceModelMasterTask(Request.UpdateExperienceModelMasterRequest request, UnityAction<AsyncResult<Result.UpdateExperienceModelMasterResult>> userCallback) : base(userCallback)
			{
				_request = request;
			}

            protected override IEnumerator ExecuteImpl(Gs2Session gs2Session)
            {
                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);

                jsonWriter.WriteObjectStart();

                if (_request.NamespaceName != null)
                {
                    jsonWriter.WritePropertyName("namespaceName");
                    jsonWriter.Write(_request.NamespaceName.ToString());
                }
                if (_request.ExperienceName != null)
                {
                    jsonWriter.WritePropertyName("experienceName");
                    jsonWriter.Write(_request.ExperienceName.ToString());
                }
                if (_request.Description != null)
                {
                    jsonWriter.WritePropertyName("description");
                    jsonWriter.Write(_request.Description.ToString());
                }
                if (_request.Metadata != null)
                {
                    jsonWriter.WritePropertyName("metadata");
                    jsonWriter.Write(_request.Metadata.ToString());
                }
                if (_request.DefaultExperience != null)
                {
                    jsonWriter.WritePropertyName("defaultExperience");
                    jsonWriter.Write(_request.DefaultExperience.ToString());
                }
                if (_request.DefaultRankCap != null)
                {
                    jsonWriter.WritePropertyName("defaultRankCap");
                    jsonWriter.Write(_request.DefaultRankCap.ToString());
                }
                if (_request.MaxRankCap != null)
                {
                    jsonWriter.WritePropertyName("maxRankCap");
                    jsonWriter.Write(_request.MaxRankCap.ToString());
                }
                if (_request.RankThresholdName != null)
                {
                    jsonWriter.WritePropertyName("rankThresholdName");
                    jsonWriter.Write(_request.RankThresholdName.ToString());
                }
                if (_request.ContextStack != null)
                {
                    jsonWriter.WritePropertyName("contextStack");
                    jsonWriter.Write(_request.ContextStack.ToString());
                }
                if (_request.RequestId != null)
                {
                    jsonWriter.WritePropertyName("xGs2RequestId");
                    jsonWriter.Write(_request.RequestId);
                }

                jsonWriter.WritePropertyName("xGs2ClientId");
                jsonWriter.Write(gs2Session.Credential.ClientId);
                jsonWriter.WritePropertyName("xGs2ProjectToken");
                jsonWriter.Write(gs2Session.ProjectToken);

                jsonWriter.WritePropertyName("x_gs2");
                jsonWriter.WriteObjectStart();
                jsonWriter.WritePropertyName("service");
                jsonWriter.Write("experience");
                jsonWriter.WritePropertyName("component");
                jsonWriter.Write("experienceModelMaster");
                jsonWriter.WritePropertyName("function");
                jsonWriter.Write("updateExperienceModelMaster");
                jsonWriter.WritePropertyName("contentType");
                jsonWriter.Write("application/json");
                jsonWriter.WritePropertyName("requestId");
                jsonWriter.Write(Gs2SessionTaskId.ToString());
                jsonWriter.WriteObjectEnd();

                jsonWriter.WriteObjectEnd();

                ((Gs2WebSocketSession)gs2Session).Send(stringBuilder.ToString());

                return new EmptyCoroutine();
            }
        }

		public IEnumerator UpdateExperienceModelMaster(
                Request.UpdateExperienceModelMasterRequest request,
                UnityAction<AsyncResult<Result.UpdateExperienceModelMasterResult>> callback
        )
		{
			var task = new UpdateExperienceModelMasterTask(request, callback);
			return Gs2WebSocketSession.Execute(task);
        }

        private class DeleteExperienceModelMasterTask : Gs2WebSocketSessionTask<Result.DeleteExperienceModelMasterResult>
        {
			private readonly Request.DeleteExperienceModelMasterRequest _request;

			public DeleteExperienceModelMasterTask(Request.DeleteExperienceModelMasterRequest request, UnityAction<AsyncResult<Result.DeleteExperienceModelMasterResult>> userCallback) : base(userCallback)
			{
				_request = request;
			}

            protected override IEnumerator ExecuteImpl(Gs2Session gs2Session)
            {
                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);

                jsonWriter.WriteObjectStart();

                if (_request.NamespaceName != null)
                {
                    jsonWriter.WritePropertyName("namespaceName");
                    jsonWriter.Write(_request.NamespaceName.ToString());
                }
                if (_request.ExperienceName != null)
                {
                    jsonWriter.WritePropertyName("experienceName");
                    jsonWriter.Write(_request.ExperienceName.ToString());
                }
                if (_request.ContextStack != null)
                {
                    jsonWriter.WritePropertyName("contextStack");
                    jsonWriter.Write(_request.ContextStack.ToString());
                }
                if (_request.RequestId != null)
                {
                    jsonWriter.WritePropertyName("xGs2RequestId");
                    jsonWriter.Write(_request.RequestId);
                }

                jsonWriter.WritePropertyName("xGs2ClientId");
                jsonWriter.Write(gs2Session.Credential.ClientId);
                jsonWriter.WritePropertyName("xGs2ProjectToken");
                jsonWriter.Write(gs2Session.ProjectToken);

                jsonWriter.WritePropertyName("x_gs2");
                jsonWriter.WriteObjectStart();
                jsonWriter.WritePropertyName("service");
                jsonWriter.Write("experience");
                jsonWriter.WritePropertyName("component");
                jsonWriter.Write("experienceModelMaster");
                jsonWriter.WritePropertyName("function");
                jsonWriter.Write("deleteExperienceModelMaster");
                jsonWriter.WritePropertyName("contentType");
                jsonWriter.Write("application/json");
                jsonWriter.WritePropertyName("requestId");
                jsonWriter.Write(Gs2SessionTaskId.ToString());
                jsonWriter.WriteObjectEnd();

                jsonWriter.WriteObjectEnd();

                ((Gs2WebSocketSession)gs2Session).Send(stringBuilder.ToString());

                return new EmptyCoroutine();
            }
        }

		public IEnumerator DeleteExperienceModelMaster(
                Request.DeleteExperienceModelMasterRequest request,
                UnityAction<AsyncResult<Result.DeleteExperienceModelMasterResult>> callback
        )
		{
			var task = new DeleteExperienceModelMasterTask(request, callback);
			return Gs2WebSocketSession.Execute(task);
        }

        private class GetExperienceModelTask : Gs2WebSocketSessionTask<Result.GetExperienceModelResult>
        {
			private readonly Request.GetExperienceModelRequest _request;

			public GetExperienceModelTask(Request.GetExperienceModelRequest request, UnityAction<AsyncResult<Result.GetExperienceModelResult>> userCallback) : base(userCallback)
			{
				_request = request;
			}

            protected override IEnumerator ExecuteImpl(Gs2Session gs2Session)
            {
                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);

                jsonWriter.WriteObjectStart();

                if (_request.NamespaceName != null)
                {
                    jsonWriter.WritePropertyName("namespaceName");
                    jsonWriter.Write(_request.NamespaceName.ToString());
                }
                if (_request.ExperienceName != null)
                {
                    jsonWriter.WritePropertyName("experienceName");
                    jsonWriter.Write(_request.ExperienceName.ToString());
                }
                if (_request.ContextStack != null)
                {
                    jsonWriter.WritePropertyName("contextStack");
                    jsonWriter.Write(_request.ContextStack.ToString());
                }
                if (_request.RequestId != null)
                {
                    jsonWriter.WritePropertyName("xGs2RequestId");
                    jsonWriter.Write(_request.RequestId);
                }

                jsonWriter.WritePropertyName("xGs2ClientId");
                jsonWriter.Write(gs2Session.Credential.ClientId);
                jsonWriter.WritePropertyName("xGs2ProjectToken");
                jsonWriter.Write(gs2Session.ProjectToken);

                jsonWriter.WritePropertyName("x_gs2");
                jsonWriter.WriteObjectStart();
                jsonWriter.WritePropertyName("service");
                jsonWriter.Write("experience");
                jsonWriter.WritePropertyName("component");
                jsonWriter.Write("experienceModel");
                jsonWriter.WritePropertyName("function");
                jsonWriter.Write("getExperienceModel");
                jsonWriter.WritePropertyName("contentType");
                jsonWriter.Write("application/json");
                jsonWriter.WritePropertyName("requestId");
                jsonWriter.Write(Gs2SessionTaskId.ToString());
                jsonWriter.WriteObjectEnd();

                jsonWriter.WriteObjectEnd();

                ((Gs2WebSocketSession)gs2Session).Send(stringBuilder.ToString());

                return new EmptyCoroutine();
            }
        }

		public IEnumerator GetExperienceModel(
                Request.GetExperienceModelRequest request,
                UnityAction<AsyncResult<Result.GetExperienceModelResult>> callback
        )
		{
			var task = new GetExperienceModelTask(request, callback);
			return Gs2WebSocketSession.Execute(task);
        }

        private class CreateThresholdMasterTask : Gs2WebSocketSessionTask<Result.CreateThresholdMasterResult>
        {
			private readonly Request.CreateThresholdMasterRequest _request;

			public CreateThresholdMasterTask(Request.CreateThresholdMasterRequest request, UnityAction<AsyncResult<Result.CreateThresholdMasterResult>> userCallback) : base(userCallback)
			{
				_request = request;
			}

            protected override IEnumerator ExecuteImpl(Gs2Session gs2Session)
            {
                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);

                jsonWriter.WriteObjectStart();

                if (_request.NamespaceName != null)
                {
                    jsonWriter.WritePropertyName("namespaceName");
                    jsonWriter.Write(_request.NamespaceName.ToString());
                }
                if (_request.Name != null)
                {
                    jsonWriter.WritePropertyName("name");
                    jsonWriter.Write(_request.Name.ToString());
                }
                if (_request.Description != null)
                {
                    jsonWriter.WritePropertyName("description");
                    jsonWriter.Write(_request.Description.ToString());
                }
                if (_request.Metadata != null)
                {
                    jsonWriter.WritePropertyName("metadata");
                    jsonWriter.Write(_request.Metadata.ToString());
                }
                if (_request.Values != null)
                {
                    jsonWriter.WritePropertyName("values");
                    jsonWriter.WriteArrayStart();
                    foreach(var item in _request.Values)
                    {
                        jsonWriter.Write(item);
                    }
                    jsonWriter.WriteArrayEnd();
                }
                if (_request.ContextStack != null)
                {
                    jsonWriter.WritePropertyName("contextStack");
                    jsonWriter.Write(_request.ContextStack.ToString());
                }
                if (_request.RequestId != null)
                {
                    jsonWriter.WritePropertyName("xGs2RequestId");
                    jsonWriter.Write(_request.RequestId);
                }

                jsonWriter.WritePropertyName("xGs2ClientId");
                jsonWriter.Write(gs2Session.Credential.ClientId);
                jsonWriter.WritePropertyName("xGs2ProjectToken");
                jsonWriter.Write(gs2Session.ProjectToken);

                jsonWriter.WritePropertyName("x_gs2");
                jsonWriter.WriteObjectStart();
                jsonWriter.WritePropertyName("service");
                jsonWriter.Write("experience");
                jsonWriter.WritePropertyName("component");
                jsonWriter.Write("thresholdMaster");
                jsonWriter.WritePropertyName("function");
                jsonWriter.Write("createThresholdMaster");
                jsonWriter.WritePropertyName("contentType");
                jsonWriter.Write("application/json");
                jsonWriter.WritePropertyName("requestId");
                jsonWriter.Write(Gs2SessionTaskId.ToString());
                jsonWriter.WriteObjectEnd();

                jsonWriter.WriteObjectEnd();

                ((Gs2WebSocketSession)gs2Session).Send(stringBuilder.ToString());

                return new EmptyCoroutine();
            }
        }

		public IEnumerator CreateThresholdMaster(
                Request.CreateThresholdMasterRequest request,
                UnityAction<AsyncResult<Result.CreateThresholdMasterResult>> callback
        )
		{
			var task = new CreateThresholdMasterTask(request, callback);
			return Gs2WebSocketSession.Execute(task);
        }

        private class GetThresholdMasterTask : Gs2WebSocketSessionTask<Result.GetThresholdMasterResult>
        {
			private readonly Request.GetThresholdMasterRequest _request;

			public GetThresholdMasterTask(Request.GetThresholdMasterRequest request, UnityAction<AsyncResult<Result.GetThresholdMasterResult>> userCallback) : base(userCallback)
			{
				_request = request;
			}

            protected override IEnumerator ExecuteImpl(Gs2Session gs2Session)
            {
                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);

                jsonWriter.WriteObjectStart();

                if (_request.NamespaceName != null)
                {
                    jsonWriter.WritePropertyName("namespaceName");
                    jsonWriter.Write(_request.NamespaceName.ToString());
                }
                if (_request.ThresholdName != null)
                {
                    jsonWriter.WritePropertyName("thresholdName");
                    jsonWriter.Write(_request.ThresholdName.ToString());
                }
                if (_request.ContextStack != null)
                {
                    jsonWriter.WritePropertyName("contextStack");
                    jsonWriter.Write(_request.ContextStack.ToString());
                }
                if (_request.RequestId != null)
                {
                    jsonWriter.WritePropertyName("xGs2RequestId");
                    jsonWriter.Write(_request.RequestId);
                }

                jsonWriter.WritePropertyName("xGs2ClientId");
                jsonWriter.Write(gs2Session.Credential.ClientId);
                jsonWriter.WritePropertyName("xGs2ProjectToken");
                jsonWriter.Write(gs2Session.ProjectToken);

                jsonWriter.WritePropertyName("x_gs2");
                jsonWriter.WriteObjectStart();
                jsonWriter.WritePropertyName("service");
                jsonWriter.Write("experience");
                jsonWriter.WritePropertyName("component");
                jsonWriter.Write("thresholdMaster");
                jsonWriter.WritePropertyName("function");
                jsonWriter.Write("getThresholdMaster");
                jsonWriter.WritePropertyName("contentType");
                jsonWriter.Write("application/json");
                jsonWriter.WritePropertyName("requestId");
                jsonWriter.Write(Gs2SessionTaskId.ToString());
                jsonWriter.WriteObjectEnd();

                jsonWriter.WriteObjectEnd();

                ((Gs2WebSocketSession)gs2Session).Send(stringBuilder.ToString());

                return new EmptyCoroutine();
            }
        }

		public IEnumerator GetThresholdMaster(
                Request.GetThresholdMasterRequest request,
                UnityAction<AsyncResult<Result.GetThresholdMasterResult>> callback
        )
		{
			var task = new GetThresholdMasterTask(request, callback);
			return Gs2WebSocketSession.Execute(task);
        }

        private class UpdateThresholdMasterTask : Gs2WebSocketSessionTask<Result.UpdateThresholdMasterResult>
        {
			private readonly Request.UpdateThresholdMasterRequest _request;

			public UpdateThresholdMasterTask(Request.UpdateThresholdMasterRequest request, UnityAction<AsyncResult<Result.UpdateThresholdMasterResult>> userCallback) : base(userCallback)
			{
				_request = request;
			}

            protected override IEnumerator ExecuteImpl(Gs2Session gs2Session)
            {
                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);

                jsonWriter.WriteObjectStart();

                if (_request.NamespaceName != null)
                {
                    jsonWriter.WritePropertyName("namespaceName");
                    jsonWriter.Write(_request.NamespaceName.ToString());
                }
                if (_request.ThresholdName != null)
                {
                    jsonWriter.WritePropertyName("thresholdName");
                    jsonWriter.Write(_request.ThresholdName.ToString());
                }
                if (_request.Description != null)
                {
                    jsonWriter.WritePropertyName("description");
                    jsonWriter.Write(_request.Description.ToString());
                }
                if (_request.Metadata != null)
                {
                    jsonWriter.WritePropertyName("metadata");
                    jsonWriter.Write(_request.Metadata.ToString());
                }
                if (_request.Values != null)
                {
                    jsonWriter.WritePropertyName("values");
                    jsonWriter.WriteArrayStart();
                    foreach(var item in _request.Values)
                    {
                        jsonWriter.Write(item);
                    }
                    jsonWriter.WriteArrayEnd();
                }
                if (_request.ContextStack != null)
                {
                    jsonWriter.WritePropertyName("contextStack");
                    jsonWriter.Write(_request.ContextStack.ToString());
                }
                if (_request.RequestId != null)
                {
                    jsonWriter.WritePropertyName("xGs2RequestId");
                    jsonWriter.Write(_request.RequestId);
                }

                jsonWriter.WritePropertyName("xGs2ClientId");
                jsonWriter.Write(gs2Session.Credential.ClientId);
                jsonWriter.WritePropertyName("xGs2ProjectToken");
                jsonWriter.Write(gs2Session.ProjectToken);

                jsonWriter.WritePropertyName("x_gs2");
                jsonWriter.WriteObjectStart();
                jsonWriter.WritePropertyName("service");
                jsonWriter.Write("experience");
                jsonWriter.WritePropertyName("component");
                jsonWriter.Write("thresholdMaster");
                jsonWriter.WritePropertyName("function");
                jsonWriter.Write("updateThresholdMaster");
                jsonWriter.WritePropertyName("contentType");
                jsonWriter.Write("application/json");
                jsonWriter.WritePropertyName("requestId");
                jsonWriter.Write(Gs2SessionTaskId.ToString());
                jsonWriter.WriteObjectEnd();

                jsonWriter.WriteObjectEnd();

                ((Gs2WebSocketSession)gs2Session).Send(stringBuilder.ToString());

                return new EmptyCoroutine();
            }
        }

		public IEnumerator UpdateThresholdMaster(
                Request.UpdateThresholdMasterRequest request,
                UnityAction<AsyncResult<Result.UpdateThresholdMasterResult>> callback
        )
		{
			var task = new UpdateThresholdMasterTask(request, callback);
			return Gs2WebSocketSession.Execute(task);
        }

        private class DeleteThresholdMasterTask : Gs2WebSocketSessionTask<Result.DeleteThresholdMasterResult>
        {
			private readonly Request.DeleteThresholdMasterRequest _request;

			public DeleteThresholdMasterTask(Request.DeleteThresholdMasterRequest request, UnityAction<AsyncResult<Result.DeleteThresholdMasterResult>> userCallback) : base(userCallback)
			{
				_request = request;
			}

            protected override IEnumerator ExecuteImpl(Gs2Session gs2Session)
            {
                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);

                jsonWriter.WriteObjectStart();

                if (_request.NamespaceName != null)
                {
                    jsonWriter.WritePropertyName("namespaceName");
                    jsonWriter.Write(_request.NamespaceName.ToString());
                }
                if (_request.ThresholdName != null)
                {
                    jsonWriter.WritePropertyName("thresholdName");
                    jsonWriter.Write(_request.ThresholdName.ToString());
                }
                if (_request.ContextStack != null)
                {
                    jsonWriter.WritePropertyName("contextStack");
                    jsonWriter.Write(_request.ContextStack.ToString());
                }
                if (_request.RequestId != null)
                {
                    jsonWriter.WritePropertyName("xGs2RequestId");
                    jsonWriter.Write(_request.RequestId);
                }

                jsonWriter.WritePropertyName("xGs2ClientId");
                jsonWriter.Write(gs2Session.Credential.ClientId);
                jsonWriter.WritePropertyName("xGs2ProjectToken");
                jsonWriter.Write(gs2Session.ProjectToken);

                jsonWriter.WritePropertyName("x_gs2");
                jsonWriter.WriteObjectStart();
                jsonWriter.WritePropertyName("service");
                jsonWriter.Write("experience");
                jsonWriter.WritePropertyName("component");
                jsonWriter.Write("thresholdMaster");
                jsonWriter.WritePropertyName("function");
                jsonWriter.Write("deleteThresholdMaster");
                jsonWriter.WritePropertyName("contentType");
                jsonWriter.Write("application/json");
                jsonWriter.WritePropertyName("requestId");
                jsonWriter.Write(Gs2SessionTaskId.ToString());
                jsonWriter.WriteObjectEnd();

                jsonWriter.WriteObjectEnd();

                ((Gs2WebSocketSession)gs2Session).Send(stringBuilder.ToString());

                return new EmptyCoroutine();
            }
        }

		public IEnumerator DeleteThresholdMaster(
                Request.DeleteThresholdMasterRequest request,
                UnityAction<AsyncResult<Result.DeleteThresholdMasterResult>> callback
        )
		{
			var task = new DeleteThresholdMasterTask(request, callback);
			return Gs2WebSocketSession.Execute(task);
        }

        private class GetStatusTask : Gs2WebSocketSessionTask<Result.GetStatusResult>
        {
			private readonly Request.GetStatusRequest _request;

			public GetStatusTask(Request.GetStatusRequest request, UnityAction<AsyncResult<Result.GetStatusResult>> userCallback) : base(userCallback)
			{
				_request = request;
			}

            protected override IEnumerator ExecuteImpl(Gs2Session gs2Session)
            {
                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);

                jsonWriter.WriteObjectStart();

                if (_request.NamespaceName != null)
                {
                    jsonWriter.WritePropertyName("namespaceName");
                    jsonWriter.Write(_request.NamespaceName.ToString());
                }
                if (_request.AccessToken != null)
                {
                    jsonWriter.WritePropertyName("accessToken");
                    jsonWriter.Write(_request.AccessToken.ToString());
                }
                if (_request.ExperienceName != null)
                {
                    jsonWriter.WritePropertyName("experienceName");
                    jsonWriter.Write(_request.ExperienceName.ToString());
                }
                if (_request.PropertyId != null)
                {
                    jsonWriter.WritePropertyName("propertyId");
                    jsonWriter.Write(_request.PropertyId.ToString());
                }
                if (_request.ContextStack != null)
                {
                    jsonWriter.WritePropertyName("contextStack");
                    jsonWriter.Write(_request.ContextStack.ToString());
                }
                if (_request.RequestId != null)
                {
                    jsonWriter.WritePropertyName("xGs2RequestId");
                    jsonWriter.Write(_request.RequestId);
                }
                if (_request.AccessToken != null)
                {
                    jsonWriter.WritePropertyName("xGs2AccessToken");
                    jsonWriter.Write(_request.AccessToken);
                }

                jsonWriter.WritePropertyName("xGs2ClientId");
                jsonWriter.Write(gs2Session.Credential.ClientId);
                jsonWriter.WritePropertyName("xGs2ProjectToken");
                jsonWriter.Write(gs2Session.ProjectToken);

                jsonWriter.WritePropertyName("x_gs2");
                jsonWriter.WriteObjectStart();
                jsonWriter.WritePropertyName("service");
                jsonWriter.Write("experience");
                jsonWriter.WritePropertyName("component");
                jsonWriter.Write("status");
                jsonWriter.WritePropertyName("function");
                jsonWriter.Write("getStatus");
                jsonWriter.WritePropertyName("contentType");
                jsonWriter.Write("application/json");
                jsonWriter.WritePropertyName("requestId");
                jsonWriter.Write(Gs2SessionTaskId.ToString());
                jsonWriter.WriteObjectEnd();

                jsonWriter.WriteObjectEnd();

                ((Gs2WebSocketSession)gs2Session).Send(stringBuilder.ToString());

                return new EmptyCoroutine();
            }
        }

		public IEnumerator GetStatus(
                Request.GetStatusRequest request,
                UnityAction<AsyncResult<Result.GetStatusResult>> callback
        )
		{
			var task = new GetStatusTask(request, callback);
			return Gs2WebSocketSession.Execute(task);
        }

        private class GetStatusByUserIdTask : Gs2WebSocketSessionTask<Result.GetStatusByUserIdResult>
        {
			private readonly Request.GetStatusByUserIdRequest _request;

			public GetStatusByUserIdTask(Request.GetStatusByUserIdRequest request, UnityAction<AsyncResult<Result.GetStatusByUserIdResult>> userCallback) : base(userCallback)
			{
				_request = request;
			}

            protected override IEnumerator ExecuteImpl(Gs2Session gs2Session)
            {
                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);

                jsonWriter.WriteObjectStart();

                if (_request.NamespaceName != null)
                {
                    jsonWriter.WritePropertyName("namespaceName");
                    jsonWriter.Write(_request.NamespaceName.ToString());
                }
                if (_request.UserId != null)
                {
                    jsonWriter.WritePropertyName("userId");
                    jsonWriter.Write(_request.UserId.ToString());
                }
                if (_request.ExperienceName != null)
                {
                    jsonWriter.WritePropertyName("experienceName");
                    jsonWriter.Write(_request.ExperienceName.ToString());
                }
                if (_request.PropertyId != null)
                {
                    jsonWriter.WritePropertyName("propertyId");
                    jsonWriter.Write(_request.PropertyId.ToString());
                }
                if (_request.ContextStack != null)
                {
                    jsonWriter.WritePropertyName("contextStack");
                    jsonWriter.Write(_request.ContextStack.ToString());
                }
                if (_request.RequestId != null)
                {
                    jsonWriter.WritePropertyName("xGs2RequestId");
                    jsonWriter.Write(_request.RequestId);
                }

                jsonWriter.WritePropertyName("xGs2ClientId");
                jsonWriter.Write(gs2Session.Credential.ClientId);
                jsonWriter.WritePropertyName("xGs2ProjectToken");
                jsonWriter.Write(gs2Session.ProjectToken);

                jsonWriter.WritePropertyName("x_gs2");
                jsonWriter.WriteObjectStart();
                jsonWriter.WritePropertyName("service");
                jsonWriter.Write("experience");
                jsonWriter.WritePropertyName("component");
                jsonWriter.Write("status");
                jsonWriter.WritePropertyName("function");
                jsonWriter.Write("getStatusByUserId");
                jsonWriter.WritePropertyName("contentType");
                jsonWriter.Write("application/json");
                jsonWriter.WritePropertyName("requestId");
                jsonWriter.Write(Gs2SessionTaskId.ToString());
                jsonWriter.WriteObjectEnd();

                jsonWriter.WriteObjectEnd();

                ((Gs2WebSocketSession)gs2Session).Send(stringBuilder.ToString());

                return new EmptyCoroutine();
            }
        }

		public IEnumerator GetStatusByUserId(
                Request.GetStatusByUserIdRequest request,
                UnityAction<AsyncResult<Result.GetStatusByUserIdResult>> callback
        )
		{
			var task = new GetStatusByUserIdTask(request, callback);
			return Gs2WebSocketSession.Execute(task);
        }

        private class AddExperienceByUserIdTask : Gs2WebSocketSessionTask<Result.AddExperienceByUserIdResult>
        {
			private readonly Request.AddExperienceByUserIdRequest _request;

			public AddExperienceByUserIdTask(Request.AddExperienceByUserIdRequest request, UnityAction<AsyncResult<Result.AddExperienceByUserIdResult>> userCallback) : base(userCallback)
			{
				_request = request;
			}

            protected override IEnumerator ExecuteImpl(Gs2Session gs2Session)
            {
                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);

                jsonWriter.WriteObjectStart();

                if (_request.NamespaceName != null)
                {
                    jsonWriter.WritePropertyName("namespaceName");
                    jsonWriter.Write(_request.NamespaceName.ToString());
                }
                if (_request.UserId != null)
                {
                    jsonWriter.WritePropertyName("userId");
                    jsonWriter.Write(_request.UserId.ToString());
                }
                if (_request.ExperienceName != null)
                {
                    jsonWriter.WritePropertyName("experienceName");
                    jsonWriter.Write(_request.ExperienceName.ToString());
                }
                if (_request.PropertyId != null)
                {
                    jsonWriter.WritePropertyName("propertyId");
                    jsonWriter.Write(_request.PropertyId.ToString());
                }
                if (_request.ExperienceValue != null)
                {
                    jsonWriter.WritePropertyName("experienceValue");
                    jsonWriter.Write(_request.ExperienceValue.ToString());
                }
                if (_request.ContextStack != null)
                {
                    jsonWriter.WritePropertyName("contextStack");
                    jsonWriter.Write(_request.ContextStack.ToString());
                }
                if (_request.RequestId != null)
                {
                    jsonWriter.WritePropertyName("xGs2RequestId");
                    jsonWriter.Write(_request.RequestId);
                }

                jsonWriter.WritePropertyName("xGs2ClientId");
                jsonWriter.Write(gs2Session.Credential.ClientId);
                jsonWriter.WritePropertyName("xGs2ProjectToken");
                jsonWriter.Write(gs2Session.ProjectToken);

                jsonWriter.WritePropertyName("x_gs2");
                jsonWriter.WriteObjectStart();
                jsonWriter.WritePropertyName("service");
                jsonWriter.Write("experience");
                jsonWriter.WritePropertyName("component");
                jsonWriter.Write("status");
                jsonWriter.WritePropertyName("function");
                jsonWriter.Write("addExperienceByUserId");
                jsonWriter.WritePropertyName("contentType");
                jsonWriter.Write("application/json");
                jsonWriter.WritePropertyName("requestId");
                jsonWriter.Write(Gs2SessionTaskId.ToString());
                jsonWriter.WriteObjectEnd();

                jsonWriter.WriteObjectEnd();

                ((Gs2WebSocketSession)gs2Session).Send(stringBuilder.ToString());

                return new EmptyCoroutine();
            }
        }

		public IEnumerator AddExperienceByUserId(
                Request.AddExperienceByUserIdRequest request,
                UnityAction<AsyncResult<Result.AddExperienceByUserIdResult>> callback
        )
		{
			var task = new AddExperienceByUserIdTask(request, callback);
			return Gs2WebSocketSession.Execute(task);
        }

        private class SetExperienceByUserIdTask : Gs2WebSocketSessionTask<Result.SetExperienceByUserIdResult>
        {
			private readonly Request.SetExperienceByUserIdRequest _request;

			public SetExperienceByUserIdTask(Request.SetExperienceByUserIdRequest request, UnityAction<AsyncResult<Result.SetExperienceByUserIdResult>> userCallback) : base(userCallback)
			{
				_request = request;
			}

            protected override IEnumerator ExecuteImpl(Gs2Session gs2Session)
            {
                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);

                jsonWriter.WriteObjectStart();

                if (_request.NamespaceName != null)
                {
                    jsonWriter.WritePropertyName("namespaceName");
                    jsonWriter.Write(_request.NamespaceName.ToString());
                }
                if (_request.UserId != null)
                {
                    jsonWriter.WritePropertyName("userId");
                    jsonWriter.Write(_request.UserId.ToString());
                }
                if (_request.ExperienceName != null)
                {
                    jsonWriter.WritePropertyName("experienceName");
                    jsonWriter.Write(_request.ExperienceName.ToString());
                }
                if (_request.PropertyId != null)
                {
                    jsonWriter.WritePropertyName("propertyId");
                    jsonWriter.Write(_request.PropertyId.ToString());
                }
                if (_request.ExperienceValue != null)
                {
                    jsonWriter.WritePropertyName("experienceValue");
                    jsonWriter.Write(_request.ExperienceValue.ToString());
                }
                if (_request.ContextStack != null)
                {
                    jsonWriter.WritePropertyName("contextStack");
                    jsonWriter.Write(_request.ContextStack.ToString());
                }
                if (_request.RequestId != null)
                {
                    jsonWriter.WritePropertyName("xGs2RequestId");
                    jsonWriter.Write(_request.RequestId);
                }

                jsonWriter.WritePropertyName("xGs2ClientId");
                jsonWriter.Write(gs2Session.Credential.ClientId);
                jsonWriter.WritePropertyName("xGs2ProjectToken");
                jsonWriter.Write(gs2Session.ProjectToken);

                jsonWriter.WritePropertyName("x_gs2");
                jsonWriter.WriteObjectStart();
                jsonWriter.WritePropertyName("service");
                jsonWriter.Write("experience");
                jsonWriter.WritePropertyName("component");
                jsonWriter.Write("status");
                jsonWriter.WritePropertyName("function");
                jsonWriter.Write("setExperienceByUserId");
                jsonWriter.WritePropertyName("contentType");
                jsonWriter.Write("application/json");
                jsonWriter.WritePropertyName("requestId");
                jsonWriter.Write(Gs2SessionTaskId.ToString());
                jsonWriter.WriteObjectEnd();

                jsonWriter.WriteObjectEnd();

                ((Gs2WebSocketSession)gs2Session).Send(stringBuilder.ToString());

                return new EmptyCoroutine();
            }
        }

		public IEnumerator SetExperienceByUserId(
                Request.SetExperienceByUserIdRequest request,
                UnityAction<AsyncResult<Result.SetExperienceByUserIdResult>> callback
        )
		{
			var task = new SetExperienceByUserIdTask(request, callback);
			return Gs2WebSocketSession.Execute(task);
        }

        private class AddRankCapByUserIdTask : Gs2WebSocketSessionTask<Result.AddRankCapByUserIdResult>
        {
			private readonly Request.AddRankCapByUserIdRequest _request;

			public AddRankCapByUserIdTask(Request.AddRankCapByUserIdRequest request, UnityAction<AsyncResult<Result.AddRankCapByUserIdResult>> userCallback) : base(userCallback)
			{
				_request = request;
			}

            protected override IEnumerator ExecuteImpl(Gs2Session gs2Session)
            {
                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);

                jsonWriter.WriteObjectStart();

                if (_request.NamespaceName != null)
                {
                    jsonWriter.WritePropertyName("namespaceName");
                    jsonWriter.Write(_request.NamespaceName.ToString());
                }
                if (_request.UserId != null)
                {
                    jsonWriter.WritePropertyName("userId");
                    jsonWriter.Write(_request.UserId.ToString());
                }
                if (_request.ExperienceName != null)
                {
                    jsonWriter.WritePropertyName("experienceName");
                    jsonWriter.Write(_request.ExperienceName.ToString());
                }
                if (_request.PropertyId != null)
                {
                    jsonWriter.WritePropertyName("propertyId");
                    jsonWriter.Write(_request.PropertyId.ToString());
                }
                if (_request.RankCapValue != null)
                {
                    jsonWriter.WritePropertyName("rankCapValue");
                    jsonWriter.Write(_request.RankCapValue.ToString());
                }
                if (_request.ContextStack != null)
                {
                    jsonWriter.WritePropertyName("contextStack");
                    jsonWriter.Write(_request.ContextStack.ToString());
                }
                if (_request.RequestId != null)
                {
                    jsonWriter.WritePropertyName("xGs2RequestId");
                    jsonWriter.Write(_request.RequestId);
                }

                jsonWriter.WritePropertyName("xGs2ClientId");
                jsonWriter.Write(gs2Session.Credential.ClientId);
                jsonWriter.WritePropertyName("xGs2ProjectToken");
                jsonWriter.Write(gs2Session.ProjectToken);

                jsonWriter.WritePropertyName("x_gs2");
                jsonWriter.WriteObjectStart();
                jsonWriter.WritePropertyName("service");
                jsonWriter.Write("experience");
                jsonWriter.WritePropertyName("component");
                jsonWriter.Write("status");
                jsonWriter.WritePropertyName("function");
                jsonWriter.Write("addRankCapByUserId");
                jsonWriter.WritePropertyName("contentType");
                jsonWriter.Write("application/json");
                jsonWriter.WritePropertyName("requestId");
                jsonWriter.Write(Gs2SessionTaskId.ToString());
                jsonWriter.WriteObjectEnd();

                jsonWriter.WriteObjectEnd();

                ((Gs2WebSocketSession)gs2Session).Send(stringBuilder.ToString());

                return new EmptyCoroutine();
            }
        }

		public IEnumerator AddRankCapByUserId(
                Request.AddRankCapByUserIdRequest request,
                UnityAction<AsyncResult<Result.AddRankCapByUserIdResult>> callback
        )
		{
			var task = new AddRankCapByUserIdTask(request, callback);
			return Gs2WebSocketSession.Execute(task);
        }

        private class SetRankCapByUserIdTask : Gs2WebSocketSessionTask<Result.SetRankCapByUserIdResult>
        {
			private readonly Request.SetRankCapByUserIdRequest _request;

			public SetRankCapByUserIdTask(Request.SetRankCapByUserIdRequest request, UnityAction<AsyncResult<Result.SetRankCapByUserIdResult>> userCallback) : base(userCallback)
			{
				_request = request;
			}

            protected override IEnumerator ExecuteImpl(Gs2Session gs2Session)
            {
                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);

                jsonWriter.WriteObjectStart();

                if (_request.NamespaceName != null)
                {
                    jsonWriter.WritePropertyName("namespaceName");
                    jsonWriter.Write(_request.NamespaceName.ToString());
                }
                if (_request.UserId != null)
                {
                    jsonWriter.WritePropertyName("userId");
                    jsonWriter.Write(_request.UserId.ToString());
                }
                if (_request.ExperienceName != null)
                {
                    jsonWriter.WritePropertyName("experienceName");
                    jsonWriter.Write(_request.ExperienceName.ToString());
                }
                if (_request.PropertyId != null)
                {
                    jsonWriter.WritePropertyName("propertyId");
                    jsonWriter.Write(_request.PropertyId.ToString());
                }
                if (_request.RankCapValue != null)
                {
                    jsonWriter.WritePropertyName("rankCapValue");
                    jsonWriter.Write(_request.RankCapValue.ToString());
                }
                if (_request.ContextStack != null)
                {
                    jsonWriter.WritePropertyName("contextStack");
                    jsonWriter.Write(_request.ContextStack.ToString());
                }
                if (_request.RequestId != null)
                {
                    jsonWriter.WritePropertyName("xGs2RequestId");
                    jsonWriter.Write(_request.RequestId);
                }

                jsonWriter.WritePropertyName("xGs2ClientId");
                jsonWriter.Write(gs2Session.Credential.ClientId);
                jsonWriter.WritePropertyName("xGs2ProjectToken");
                jsonWriter.Write(gs2Session.ProjectToken);

                jsonWriter.WritePropertyName("x_gs2");
                jsonWriter.WriteObjectStart();
                jsonWriter.WritePropertyName("service");
                jsonWriter.Write("experience");
                jsonWriter.WritePropertyName("component");
                jsonWriter.Write("status");
                jsonWriter.WritePropertyName("function");
                jsonWriter.Write("setRankCapByUserId");
                jsonWriter.WritePropertyName("contentType");
                jsonWriter.Write("application/json");
                jsonWriter.WritePropertyName("requestId");
                jsonWriter.Write(Gs2SessionTaskId.ToString());
                jsonWriter.WriteObjectEnd();

                jsonWriter.WriteObjectEnd();

                ((Gs2WebSocketSession)gs2Session).Send(stringBuilder.ToString());

                return new EmptyCoroutine();
            }
        }

		public IEnumerator SetRankCapByUserId(
                Request.SetRankCapByUserIdRequest request,
                UnityAction<AsyncResult<Result.SetRankCapByUserIdResult>> callback
        )
		{
			var task = new SetRankCapByUserIdTask(request, callback);
			return Gs2WebSocketSession.Execute(task);
        }

        private class DeleteStatusByUserIdTask : Gs2WebSocketSessionTask<Result.DeleteStatusByUserIdResult>
        {
			private readonly Request.DeleteStatusByUserIdRequest _request;

			public DeleteStatusByUserIdTask(Request.DeleteStatusByUserIdRequest request, UnityAction<AsyncResult<Result.DeleteStatusByUserIdResult>> userCallback) : base(userCallback)
			{
				_request = request;
			}

            protected override IEnumerator ExecuteImpl(Gs2Session gs2Session)
            {
                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);

                jsonWriter.WriteObjectStart();

                if (_request.NamespaceName != null)
                {
                    jsonWriter.WritePropertyName("namespaceName");
                    jsonWriter.Write(_request.NamespaceName.ToString());
                }
                if (_request.UserId != null)
                {
                    jsonWriter.WritePropertyName("userId");
                    jsonWriter.Write(_request.UserId.ToString());
                }
                if (_request.ExperienceName != null)
                {
                    jsonWriter.WritePropertyName("experienceName");
                    jsonWriter.Write(_request.ExperienceName.ToString());
                }
                if (_request.PropertyId != null)
                {
                    jsonWriter.WritePropertyName("propertyId");
                    jsonWriter.Write(_request.PropertyId.ToString());
                }
                if (_request.ContextStack != null)
                {
                    jsonWriter.WritePropertyName("contextStack");
                    jsonWriter.Write(_request.ContextStack.ToString());
                }
                if (_request.RequestId != null)
                {
                    jsonWriter.WritePropertyName("xGs2RequestId");
                    jsonWriter.Write(_request.RequestId);
                }

                jsonWriter.WritePropertyName("xGs2ClientId");
                jsonWriter.Write(gs2Session.Credential.ClientId);
                jsonWriter.WritePropertyName("xGs2ProjectToken");
                jsonWriter.Write(gs2Session.ProjectToken);

                jsonWriter.WritePropertyName("x_gs2");
                jsonWriter.WriteObjectStart();
                jsonWriter.WritePropertyName("service");
                jsonWriter.Write("experience");
                jsonWriter.WritePropertyName("component");
                jsonWriter.Write("status");
                jsonWriter.WritePropertyName("function");
                jsonWriter.Write("deleteStatusByUserId");
                jsonWriter.WritePropertyName("contentType");
                jsonWriter.Write("application/json");
                jsonWriter.WritePropertyName("requestId");
                jsonWriter.Write(Gs2SessionTaskId.ToString());
                jsonWriter.WriteObjectEnd();

                jsonWriter.WriteObjectEnd();

                ((Gs2WebSocketSession)gs2Session).Send(stringBuilder.ToString());

                return new EmptyCoroutine();
            }
        }

		public IEnumerator DeleteStatusByUserId(
                Request.DeleteStatusByUserIdRequest request,
                UnityAction<AsyncResult<Result.DeleteStatusByUserIdResult>> callback
        )
		{
			var task = new DeleteStatusByUserIdTask(request, callback);
			return Gs2WebSocketSession.Execute(task);
        }

        private class AddExperienceByStampSheetTask : Gs2WebSocketSessionTask<Result.AddExperienceByStampSheetResult>
        {
			private readonly Request.AddExperienceByStampSheetRequest _request;

			public AddExperienceByStampSheetTask(Request.AddExperienceByStampSheetRequest request, UnityAction<AsyncResult<Result.AddExperienceByStampSheetResult>> userCallback) : base(userCallback)
			{
				_request = request;
			}

            protected override IEnumerator ExecuteImpl(Gs2Session gs2Session)
            {
                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);

                jsonWriter.WriteObjectStart();

                if (_request.StampSheet != null)
                {
                    jsonWriter.WritePropertyName("stampSheet");
                    jsonWriter.Write(_request.StampSheet.ToString());
                }
                if (_request.KeyId != null)
                {
                    jsonWriter.WritePropertyName("keyId");
                    jsonWriter.Write(_request.KeyId.ToString());
                }
                if (_request.ContextStack != null)
                {
                    jsonWriter.WritePropertyName("contextStack");
                    jsonWriter.Write(_request.ContextStack.ToString());
                }
                if (_request.RequestId != null)
                {
                    jsonWriter.WritePropertyName("xGs2RequestId");
                    jsonWriter.Write(_request.RequestId);
                }

                jsonWriter.WritePropertyName("xGs2ClientId");
                jsonWriter.Write(gs2Session.Credential.ClientId);
                jsonWriter.WritePropertyName("xGs2ProjectToken");
                jsonWriter.Write(gs2Session.ProjectToken);

                jsonWriter.WritePropertyName("x_gs2");
                jsonWriter.WriteObjectStart();
                jsonWriter.WritePropertyName("service");
                jsonWriter.Write("experience");
                jsonWriter.WritePropertyName("component");
                jsonWriter.Write("status");
                jsonWriter.WritePropertyName("function");
                jsonWriter.Write("addExperienceByStampSheet");
                jsonWriter.WritePropertyName("contentType");
                jsonWriter.Write("application/json");
                jsonWriter.WritePropertyName("requestId");
                jsonWriter.Write(Gs2SessionTaskId.ToString());
                jsonWriter.WriteObjectEnd();

                jsonWriter.WriteObjectEnd();

                ((Gs2WebSocketSession)gs2Session).Send(stringBuilder.ToString());

                return new EmptyCoroutine();
            }
        }

		public IEnumerator AddExperienceByStampSheet(
                Request.AddExperienceByStampSheetRequest request,
                UnityAction<AsyncResult<Result.AddExperienceByStampSheetResult>> callback
        )
		{
			var task = new AddExperienceByStampSheetTask(request, callback);
			return Gs2WebSocketSession.Execute(task);
        }

        private class AddRankCapByStampSheetTask : Gs2WebSocketSessionTask<Result.AddRankCapByStampSheetResult>
        {
			private readonly Request.AddRankCapByStampSheetRequest _request;

			public AddRankCapByStampSheetTask(Request.AddRankCapByStampSheetRequest request, UnityAction<AsyncResult<Result.AddRankCapByStampSheetResult>> userCallback) : base(userCallback)
			{
				_request = request;
			}

            protected override IEnumerator ExecuteImpl(Gs2Session gs2Session)
            {
                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);

                jsonWriter.WriteObjectStart();

                if (_request.StampSheet != null)
                {
                    jsonWriter.WritePropertyName("stampSheet");
                    jsonWriter.Write(_request.StampSheet.ToString());
                }
                if (_request.KeyId != null)
                {
                    jsonWriter.WritePropertyName("keyId");
                    jsonWriter.Write(_request.KeyId.ToString());
                }
                if (_request.ContextStack != null)
                {
                    jsonWriter.WritePropertyName("contextStack");
                    jsonWriter.Write(_request.ContextStack.ToString());
                }
                if (_request.RequestId != null)
                {
                    jsonWriter.WritePropertyName("xGs2RequestId");
                    jsonWriter.Write(_request.RequestId);
                }

                jsonWriter.WritePropertyName("xGs2ClientId");
                jsonWriter.Write(gs2Session.Credential.ClientId);
                jsonWriter.WritePropertyName("xGs2ProjectToken");
                jsonWriter.Write(gs2Session.ProjectToken);

                jsonWriter.WritePropertyName("x_gs2");
                jsonWriter.WriteObjectStart();
                jsonWriter.WritePropertyName("service");
                jsonWriter.Write("experience");
                jsonWriter.WritePropertyName("component");
                jsonWriter.Write("status");
                jsonWriter.WritePropertyName("function");
                jsonWriter.Write("addRankCapByStampSheet");
                jsonWriter.WritePropertyName("contentType");
                jsonWriter.Write("application/json");
                jsonWriter.WritePropertyName("requestId");
                jsonWriter.Write(Gs2SessionTaskId.ToString());
                jsonWriter.WriteObjectEnd();

                jsonWriter.WriteObjectEnd();

                ((Gs2WebSocketSession)gs2Session).Send(stringBuilder.ToString());

                return new EmptyCoroutine();
            }
        }

		public IEnumerator AddRankCapByStampSheet(
                Request.AddRankCapByStampSheetRequest request,
                UnityAction<AsyncResult<Result.AddRankCapByStampSheetResult>> callback
        )
		{
			var task = new AddRankCapByStampSheetTask(request, callback);
			return Gs2WebSocketSession.Execute(task);
        }

        private class SetRankCapByStampSheetTask : Gs2WebSocketSessionTask<Result.SetRankCapByStampSheetResult>
        {
			private readonly Request.SetRankCapByStampSheetRequest _request;

			public SetRankCapByStampSheetTask(Request.SetRankCapByStampSheetRequest request, UnityAction<AsyncResult<Result.SetRankCapByStampSheetResult>> userCallback) : base(userCallback)
			{
				_request = request;
			}

            protected override IEnumerator ExecuteImpl(Gs2Session gs2Session)
            {
                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);

                jsonWriter.WriteObjectStart();

                if (_request.StampSheet != null)
                {
                    jsonWriter.WritePropertyName("stampSheet");
                    jsonWriter.Write(_request.StampSheet.ToString());
                }
                if (_request.KeyId != null)
                {
                    jsonWriter.WritePropertyName("keyId");
                    jsonWriter.Write(_request.KeyId.ToString());
                }
                if (_request.ContextStack != null)
                {
                    jsonWriter.WritePropertyName("contextStack");
                    jsonWriter.Write(_request.ContextStack.ToString());
                }
                if (_request.RequestId != null)
                {
                    jsonWriter.WritePropertyName("xGs2RequestId");
                    jsonWriter.Write(_request.RequestId);
                }

                jsonWriter.WritePropertyName("xGs2ClientId");
                jsonWriter.Write(gs2Session.Credential.ClientId);
                jsonWriter.WritePropertyName("xGs2ProjectToken");
                jsonWriter.Write(gs2Session.ProjectToken);

                jsonWriter.WritePropertyName("x_gs2");
                jsonWriter.WriteObjectStart();
                jsonWriter.WritePropertyName("service");
                jsonWriter.Write("experience");
                jsonWriter.WritePropertyName("component");
                jsonWriter.Write("status");
                jsonWriter.WritePropertyName("function");
                jsonWriter.Write("setRankCapByStampSheet");
                jsonWriter.WritePropertyName("contentType");
                jsonWriter.Write("application/json");
                jsonWriter.WritePropertyName("requestId");
                jsonWriter.Write(Gs2SessionTaskId.ToString());
                jsonWriter.WriteObjectEnd();

                jsonWriter.WriteObjectEnd();

                ((Gs2WebSocketSession)gs2Session).Send(stringBuilder.ToString());

                return new EmptyCoroutine();
            }
        }

		public IEnumerator SetRankCapByStampSheet(
                Request.SetRankCapByStampSheetRequest request,
                UnityAction<AsyncResult<Result.SetRankCapByStampSheetResult>> callback
        )
		{
			var task = new SetRankCapByStampSheetTask(request, callback);
			return Gs2WebSocketSession.Execute(task);
        }
	}
}