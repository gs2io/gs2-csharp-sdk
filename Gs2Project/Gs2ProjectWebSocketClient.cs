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
using Gs2.Util.LitJson;namespace Gs2.Gs2Project
{
	public class Gs2ProjectWebSocketClient : AbstractGs2Client
	{

		public static string Endpoint = "project";

        protected Gs2WebSocketSession Gs2WebSocketSession => (Gs2WebSocketSession) Gs2Session;

		public Gs2ProjectWebSocketClient(Gs2WebSocketSession Gs2WebSocketSession) : base(Gs2WebSocketSession)
		{

		}

        private class CreateAccountTask : Gs2WebSocketSessionTask<Result.CreateAccountResult>
        {
			private readonly Request.CreateAccountRequest _request;

			public CreateAccountTask(Request.CreateAccountRequest request, UnityAction<AsyncResult<Result.CreateAccountResult>> userCallback) : base(userCallback)
			{
				_request = request;
			}

            protected override IEnumerator ExecuteImpl(Gs2Session gs2Session)
            {
                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);

                jsonWriter.WriteObjectStart();

                if (_request.Email != null)
                {
                    jsonWriter.WritePropertyName("email");
                    jsonWriter.Write(_request.Email.ToString());
                }
                if (_request.FullName != null)
                {
                    jsonWriter.WritePropertyName("fullName");
                    jsonWriter.Write(_request.FullName.ToString());
                }
                if (_request.CompanyName != null)
                {
                    jsonWriter.WritePropertyName("companyName");
                    jsonWriter.Write(_request.CompanyName.ToString());
                }
                if (_request.Password != null)
                {
                    jsonWriter.WritePropertyName("password");
                    jsonWriter.Write(_request.Password.ToString());
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
                jsonWriter.Write("project");
                jsonWriter.WritePropertyName("component");
                jsonWriter.Write("account");
                jsonWriter.WritePropertyName("function");
                jsonWriter.Write("createAccount");
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

		public IEnumerator CreateAccount(
                Request.CreateAccountRequest request,
                UnityAction<AsyncResult<Result.CreateAccountResult>> callback
        )
		{
			var task = new CreateAccountTask(request, callback);
			return Gs2WebSocketSession.Execute(task);
        }

        private class VerifyTask : Gs2WebSocketSessionTask<Result.VerifyResult>
        {
			private readonly Request.VerifyRequest _request;

			public VerifyTask(Request.VerifyRequest request, UnityAction<AsyncResult<Result.VerifyResult>> userCallback) : base(userCallback)
			{
				_request = request;
			}

            protected override IEnumerator ExecuteImpl(Gs2Session gs2Session)
            {
                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);

                jsonWriter.WriteObjectStart();

                if (_request.VerifyToken != null)
                {
                    jsonWriter.WritePropertyName("verifyToken");
                    jsonWriter.Write(_request.VerifyToken.ToString());
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
                jsonWriter.Write("project");
                jsonWriter.WritePropertyName("component");
                jsonWriter.Write("account");
                jsonWriter.WritePropertyName("function");
                jsonWriter.Write("verify");
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

		public IEnumerator Verify(
                Request.VerifyRequest request,
                UnityAction<AsyncResult<Result.VerifyResult>> callback
        )
		{
			var task = new VerifyTask(request, callback);
			return Gs2WebSocketSession.Execute(task);
        }

        private class UpdateAccountTask : Gs2WebSocketSessionTask<Result.UpdateAccountResult>
        {
			private readonly Request.UpdateAccountRequest _request;

			public UpdateAccountTask(Request.UpdateAccountRequest request, UnityAction<AsyncResult<Result.UpdateAccountResult>> userCallback) : base(userCallback)
			{
				_request = request;
			}

            protected override IEnumerator ExecuteImpl(Gs2Session gs2Session)
            {
                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);

                jsonWriter.WriteObjectStart();

                if (_request.Email != null)
                {
                    jsonWriter.WritePropertyName("email");
                    jsonWriter.Write(_request.Email.ToString());
                }
                if (_request.FullName != null)
                {
                    jsonWriter.WritePropertyName("fullName");
                    jsonWriter.Write(_request.FullName.ToString());
                }
                if (_request.CompanyName != null)
                {
                    jsonWriter.WritePropertyName("companyName");
                    jsonWriter.Write(_request.CompanyName.ToString());
                }
                if (_request.Password != null)
                {
                    jsonWriter.WritePropertyName("password");
                    jsonWriter.Write(_request.Password.ToString());
                }
                if (_request.AccountToken != null)
                {
                    jsonWriter.WritePropertyName("accountToken");
                    jsonWriter.Write(_request.AccountToken.ToString());
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
                jsonWriter.Write("project");
                jsonWriter.WritePropertyName("component");
                jsonWriter.Write("account");
                jsonWriter.WritePropertyName("function");
                jsonWriter.Write("updateAccount");
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

		public IEnumerator UpdateAccount(
                Request.UpdateAccountRequest request,
                UnityAction<AsyncResult<Result.UpdateAccountResult>> callback
        )
		{
			var task = new UpdateAccountTask(request, callback);
			return Gs2WebSocketSession.Execute(task);
        }

        private class DeleteAccountTask : Gs2WebSocketSessionTask<Result.DeleteAccountResult>
        {
			private readonly Request.DeleteAccountRequest _request;

			public DeleteAccountTask(Request.DeleteAccountRequest request, UnityAction<AsyncResult<Result.DeleteAccountResult>> userCallback) : base(userCallback)
			{
				_request = request;
			}

            protected override IEnumerator ExecuteImpl(Gs2Session gs2Session)
            {
                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);

                jsonWriter.WriteObjectStart();

                if (_request.AccountToken != null)
                {
                    jsonWriter.WritePropertyName("accountToken");
                    jsonWriter.Write(_request.AccountToken.ToString());
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
                jsonWriter.Write("project");
                jsonWriter.WritePropertyName("component");
                jsonWriter.Write("account");
                jsonWriter.WritePropertyName("function");
                jsonWriter.Write("deleteAccount");
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

		public IEnumerator DeleteAccount(
                Request.DeleteAccountRequest request,
                UnityAction<AsyncResult<Result.DeleteAccountResult>> callback
        )
		{
			var task = new DeleteAccountTask(request, callback);
			return Gs2WebSocketSession.Execute(task);
        }

        private class CreateProjectTask : Gs2WebSocketSessionTask<Result.CreateProjectResult>
        {
			private readonly Request.CreateProjectRequest _request;

			public CreateProjectTask(Request.CreateProjectRequest request, UnityAction<AsyncResult<Result.CreateProjectResult>> userCallback) : base(userCallback)
			{
				_request = request;
			}

            protected override IEnumerator ExecuteImpl(Gs2Session gs2Session)
            {
                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);

                jsonWriter.WriteObjectStart();

                if (_request.AccountToken != null)
                {
                    jsonWriter.WritePropertyName("accountToken");
                    jsonWriter.Write(_request.AccountToken.ToString());
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
                if (_request.Plan != null)
                {
                    jsonWriter.WritePropertyName("plan");
                    jsonWriter.Write(_request.Plan.ToString());
                }
                if (_request.BillingMethodName != null)
                {
                    jsonWriter.WritePropertyName("billingMethodName");
                    jsonWriter.Write(_request.BillingMethodName.ToString());
                }
                if (_request.EnableEventBridge != null)
                {
                    jsonWriter.WritePropertyName("enableEventBridge");
                    jsonWriter.Write(_request.EnableEventBridge.ToString());
                }
                if (_request.EventBridgeAwsAccountId != null)
                {
                    jsonWriter.WritePropertyName("eventBridgeAwsAccountId");
                    jsonWriter.Write(_request.EventBridgeAwsAccountId.ToString());
                }
                if (_request.EventBridgeAwsRegion != null)
                {
                    jsonWriter.WritePropertyName("eventBridgeAwsRegion");
                    jsonWriter.Write(_request.EventBridgeAwsRegion.ToString());
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
                jsonWriter.Write("project");
                jsonWriter.WritePropertyName("component");
                jsonWriter.Write("project");
                jsonWriter.WritePropertyName("function");
                jsonWriter.Write("createProject");
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

		public IEnumerator CreateProject(
                Request.CreateProjectRequest request,
                UnityAction<AsyncResult<Result.CreateProjectResult>> callback
        )
		{
			var task = new CreateProjectTask(request, callback);
			return Gs2WebSocketSession.Execute(task);
        }

        private class GetProjectTask : Gs2WebSocketSessionTask<Result.GetProjectResult>
        {
			private readonly Request.GetProjectRequest _request;

			public GetProjectTask(Request.GetProjectRequest request, UnityAction<AsyncResult<Result.GetProjectResult>> userCallback) : base(userCallback)
			{
				_request = request;
			}

            protected override IEnumerator ExecuteImpl(Gs2Session gs2Session)
            {
                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);

                jsonWriter.WriteObjectStart();

                if (_request.AccountToken != null)
                {
                    jsonWriter.WritePropertyName("accountToken");
                    jsonWriter.Write(_request.AccountToken.ToString());
                }
                if (_request.ProjectName != null)
                {
                    jsonWriter.WritePropertyName("projectName");
                    jsonWriter.Write(_request.ProjectName.ToString());
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
                jsonWriter.Write("project");
                jsonWriter.WritePropertyName("component");
                jsonWriter.Write("project");
                jsonWriter.WritePropertyName("function");
                jsonWriter.Write("getProject");
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

		public IEnumerator GetProject(
                Request.GetProjectRequest request,
                UnityAction<AsyncResult<Result.GetProjectResult>> callback
        )
		{
			var task = new GetProjectTask(request, callback);
			return Gs2WebSocketSession.Execute(task);
        }

        private class GetProjectTokenTask : Gs2WebSocketSessionTask<Result.GetProjectTokenResult>
        {
			private readonly Request.GetProjectTokenRequest _request;

			public GetProjectTokenTask(Request.GetProjectTokenRequest request, UnityAction<AsyncResult<Result.GetProjectTokenResult>> userCallback) : base(userCallback)
			{
				_request = request;
			}

            protected override IEnumerator ExecuteImpl(Gs2Session gs2Session)
            {
                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);

                jsonWriter.WriteObjectStart();

                if (_request.ProjectName != null)
                {
                    jsonWriter.WritePropertyName("projectName");
                    jsonWriter.Write(_request.ProjectName.ToString());
                }
                if (_request.AccountToken != null)
                {
                    jsonWriter.WritePropertyName("accountToken");
                    jsonWriter.Write(_request.AccountToken.ToString());
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
                jsonWriter.Write("project");
                jsonWriter.WritePropertyName("component");
                jsonWriter.Write("project");
                jsonWriter.WritePropertyName("function");
                jsonWriter.Write("getProjectToken");
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

		public IEnumerator GetProjectToken(
                Request.GetProjectTokenRequest request,
                UnityAction<AsyncResult<Result.GetProjectTokenResult>> callback
        )
		{
			var task = new GetProjectTokenTask(request, callback);
			return Gs2WebSocketSession.Execute(task);
        }

        private class GetProjectTokenByIdentifierTask : Gs2WebSocketSessionTask<Result.GetProjectTokenByIdentifierResult>
        {
			private readonly Request.GetProjectTokenByIdentifierRequest _request;

			public GetProjectTokenByIdentifierTask(Request.GetProjectTokenByIdentifierRequest request, UnityAction<AsyncResult<Result.GetProjectTokenByIdentifierResult>> userCallback) : base(userCallback)
			{
				_request = request;
			}

            protected override IEnumerator ExecuteImpl(Gs2Session gs2Session)
            {
                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);

                jsonWriter.WriteObjectStart();

                if (_request.AccountName != null)
                {
                    jsonWriter.WritePropertyName("accountName");
                    jsonWriter.Write(_request.AccountName.ToString());
                }
                if (_request.ProjectName != null)
                {
                    jsonWriter.WritePropertyName("projectName");
                    jsonWriter.Write(_request.ProjectName.ToString());
                }
                if (_request.UserName != null)
                {
                    jsonWriter.WritePropertyName("userName");
                    jsonWriter.Write(_request.UserName.ToString());
                }
                if (_request.Password != null)
                {
                    jsonWriter.WritePropertyName("password");
                    jsonWriter.Write(_request.Password.ToString());
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
                jsonWriter.Write("project");
                jsonWriter.WritePropertyName("component");
                jsonWriter.Write("project");
                jsonWriter.WritePropertyName("function");
                jsonWriter.Write("getProjectTokenByIdentifier");
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

		public IEnumerator GetProjectTokenByIdentifier(
                Request.GetProjectTokenByIdentifierRequest request,
                UnityAction<AsyncResult<Result.GetProjectTokenByIdentifierResult>> callback
        )
		{
			var task = new GetProjectTokenByIdentifierTask(request, callback);
			return Gs2WebSocketSession.Execute(task);
        }

        private class UpdateProjectTask : Gs2WebSocketSessionTask<Result.UpdateProjectResult>
        {
			private readonly Request.UpdateProjectRequest _request;

			public UpdateProjectTask(Request.UpdateProjectRequest request, UnityAction<AsyncResult<Result.UpdateProjectResult>> userCallback) : base(userCallback)
			{
				_request = request;
			}

            protected override IEnumerator ExecuteImpl(Gs2Session gs2Session)
            {
                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);

                jsonWriter.WriteObjectStart();

                if (_request.AccountToken != null)
                {
                    jsonWriter.WritePropertyName("accountToken");
                    jsonWriter.Write(_request.AccountToken.ToString());
                }
                if (_request.ProjectName != null)
                {
                    jsonWriter.WritePropertyName("projectName");
                    jsonWriter.Write(_request.ProjectName.ToString());
                }
                if (_request.Description != null)
                {
                    jsonWriter.WritePropertyName("description");
                    jsonWriter.Write(_request.Description.ToString());
                }
                if (_request.Plan != null)
                {
                    jsonWriter.WritePropertyName("plan");
                    jsonWriter.Write(_request.Plan.ToString());
                }
                if (_request.BillingMethodName != null)
                {
                    jsonWriter.WritePropertyName("billingMethodName");
                    jsonWriter.Write(_request.BillingMethodName.ToString());
                }
                if (_request.EnableEventBridge != null)
                {
                    jsonWriter.WritePropertyName("enableEventBridge");
                    jsonWriter.Write(_request.EnableEventBridge.ToString());
                }
                if (_request.EventBridgeAwsAccountId != null)
                {
                    jsonWriter.WritePropertyName("eventBridgeAwsAccountId");
                    jsonWriter.Write(_request.EventBridgeAwsAccountId.ToString());
                }
                if (_request.EventBridgeAwsRegion != null)
                {
                    jsonWriter.WritePropertyName("eventBridgeAwsRegion");
                    jsonWriter.Write(_request.EventBridgeAwsRegion.ToString());
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
                jsonWriter.Write("project");
                jsonWriter.WritePropertyName("component");
                jsonWriter.Write("project");
                jsonWriter.WritePropertyName("function");
                jsonWriter.Write("updateProject");
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

		public IEnumerator UpdateProject(
                Request.UpdateProjectRequest request,
                UnityAction<AsyncResult<Result.UpdateProjectResult>> callback
        )
		{
			var task = new UpdateProjectTask(request, callback);
			return Gs2WebSocketSession.Execute(task);
        }

        private class DeleteProjectTask : Gs2WebSocketSessionTask<Result.DeleteProjectResult>
        {
			private readonly Request.DeleteProjectRequest _request;

			public DeleteProjectTask(Request.DeleteProjectRequest request, UnityAction<AsyncResult<Result.DeleteProjectResult>> userCallback) : base(userCallback)
			{
				_request = request;
			}

            protected override IEnumerator ExecuteImpl(Gs2Session gs2Session)
            {
                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);

                jsonWriter.WriteObjectStart();

                if (_request.AccountToken != null)
                {
                    jsonWriter.WritePropertyName("accountToken");
                    jsonWriter.Write(_request.AccountToken.ToString());
                }
                if (_request.ProjectName != null)
                {
                    jsonWriter.WritePropertyName("projectName");
                    jsonWriter.Write(_request.ProjectName.ToString());
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
                jsonWriter.Write("project");
                jsonWriter.WritePropertyName("component");
                jsonWriter.Write("project");
                jsonWriter.WritePropertyName("function");
                jsonWriter.Write("deleteProject");
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

		public IEnumerator DeleteProject(
                Request.DeleteProjectRequest request,
                UnityAction<AsyncResult<Result.DeleteProjectResult>> callback
        )
		{
			var task = new DeleteProjectTask(request, callback);
			return Gs2WebSocketSession.Execute(task);
        }

        private class CreateBillingMethodTask : Gs2WebSocketSessionTask<Result.CreateBillingMethodResult>
        {
			private readonly Request.CreateBillingMethodRequest _request;

			public CreateBillingMethodTask(Request.CreateBillingMethodRequest request, UnityAction<AsyncResult<Result.CreateBillingMethodResult>> userCallback) : base(userCallback)
			{
				_request = request;
			}

            protected override IEnumerator ExecuteImpl(Gs2Session gs2Session)
            {
                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);

                jsonWriter.WriteObjectStart();

                if (_request.AccountToken != null)
                {
                    jsonWriter.WritePropertyName("accountToken");
                    jsonWriter.Write(_request.AccountToken.ToString());
                }
                if (_request.Description != null)
                {
                    jsonWriter.WritePropertyName("description");
                    jsonWriter.Write(_request.Description.ToString());
                }
                if (_request.MethodType != null)
                {
                    jsonWriter.WritePropertyName("methodType");
                    jsonWriter.Write(_request.MethodType.ToString());
                }
                if (_request.CardCustomerId != null)
                {
                    jsonWriter.WritePropertyName("cardCustomerId");
                    jsonWriter.Write(_request.CardCustomerId.ToString());
                }
                if (_request.PartnerId != null)
                {
                    jsonWriter.WritePropertyName("partnerId");
                    jsonWriter.Write(_request.PartnerId.ToString());
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
                jsonWriter.Write("project");
                jsonWriter.WritePropertyName("component");
                jsonWriter.Write("billingMethod");
                jsonWriter.WritePropertyName("function");
                jsonWriter.Write("createBillingMethod");
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

		public IEnumerator CreateBillingMethod(
                Request.CreateBillingMethodRequest request,
                UnityAction<AsyncResult<Result.CreateBillingMethodResult>> callback
        )
		{
			var task = new CreateBillingMethodTask(request, callback);
			return Gs2WebSocketSession.Execute(task);
        }

        private class GetBillingMethodTask : Gs2WebSocketSessionTask<Result.GetBillingMethodResult>
        {
			private readonly Request.GetBillingMethodRequest _request;

			public GetBillingMethodTask(Request.GetBillingMethodRequest request, UnityAction<AsyncResult<Result.GetBillingMethodResult>> userCallback) : base(userCallback)
			{
				_request = request;
			}

            protected override IEnumerator ExecuteImpl(Gs2Session gs2Session)
            {
                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);

                jsonWriter.WriteObjectStart();

                if (_request.AccountToken != null)
                {
                    jsonWriter.WritePropertyName("accountToken");
                    jsonWriter.Write(_request.AccountToken.ToString());
                }
                if (_request.BillingMethodName != null)
                {
                    jsonWriter.WritePropertyName("billingMethodName");
                    jsonWriter.Write(_request.BillingMethodName.ToString());
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
                jsonWriter.Write("project");
                jsonWriter.WritePropertyName("component");
                jsonWriter.Write("billingMethod");
                jsonWriter.WritePropertyName("function");
                jsonWriter.Write("getBillingMethod");
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

		public IEnumerator GetBillingMethod(
                Request.GetBillingMethodRequest request,
                UnityAction<AsyncResult<Result.GetBillingMethodResult>> callback
        )
		{
			var task = new GetBillingMethodTask(request, callback);
			return Gs2WebSocketSession.Execute(task);
        }

        private class UpdateBillingMethodTask : Gs2WebSocketSessionTask<Result.UpdateBillingMethodResult>
        {
			private readonly Request.UpdateBillingMethodRequest _request;

			public UpdateBillingMethodTask(Request.UpdateBillingMethodRequest request, UnityAction<AsyncResult<Result.UpdateBillingMethodResult>> userCallback) : base(userCallback)
			{
				_request = request;
			}

            protected override IEnumerator ExecuteImpl(Gs2Session gs2Session)
            {
                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);

                jsonWriter.WriteObjectStart();

                if (_request.AccountToken != null)
                {
                    jsonWriter.WritePropertyName("accountToken");
                    jsonWriter.Write(_request.AccountToken.ToString());
                }
                if (_request.BillingMethodName != null)
                {
                    jsonWriter.WritePropertyName("billingMethodName");
                    jsonWriter.Write(_request.BillingMethodName.ToString());
                }
                if (_request.Description != null)
                {
                    jsonWriter.WritePropertyName("description");
                    jsonWriter.Write(_request.Description.ToString());
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
                jsonWriter.Write("project");
                jsonWriter.WritePropertyName("component");
                jsonWriter.Write("billingMethod");
                jsonWriter.WritePropertyName("function");
                jsonWriter.Write("updateBillingMethod");
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

		public IEnumerator UpdateBillingMethod(
                Request.UpdateBillingMethodRequest request,
                UnityAction<AsyncResult<Result.UpdateBillingMethodResult>> callback
        )
		{
			var task = new UpdateBillingMethodTask(request, callback);
			return Gs2WebSocketSession.Execute(task);
        }

        private class DeleteBillingMethodTask : Gs2WebSocketSessionTask<Result.DeleteBillingMethodResult>
        {
			private readonly Request.DeleteBillingMethodRequest _request;

			public DeleteBillingMethodTask(Request.DeleteBillingMethodRequest request, UnityAction<AsyncResult<Result.DeleteBillingMethodResult>> userCallback) : base(userCallback)
			{
				_request = request;
			}

            protected override IEnumerator ExecuteImpl(Gs2Session gs2Session)
            {
                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);

                jsonWriter.WriteObjectStart();

                if (_request.AccountToken != null)
                {
                    jsonWriter.WritePropertyName("accountToken");
                    jsonWriter.Write(_request.AccountToken.ToString());
                }
                if (_request.BillingMethodName != null)
                {
                    jsonWriter.WritePropertyName("billingMethodName");
                    jsonWriter.Write(_request.BillingMethodName.ToString());
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
                jsonWriter.Write("project");
                jsonWriter.WritePropertyName("component");
                jsonWriter.Write("billingMethod");
                jsonWriter.WritePropertyName("function");
                jsonWriter.Write("deleteBillingMethod");
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

		public IEnumerator DeleteBillingMethod(
                Request.DeleteBillingMethodRequest request,
                UnityAction<AsyncResult<Result.DeleteBillingMethodResult>> callback
        )
		{
			var task = new DeleteBillingMethodTask(request, callback);
			return Gs2WebSocketSession.Execute(task);
        }
	}
}