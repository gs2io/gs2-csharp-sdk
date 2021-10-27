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

namespace Gs2.Gs2Project
{
	public class Gs2ProjectWebSocketClient : AbstractGs2Client
	{

		public static string Endpoint = "project";

        protected Gs2WebSocketSession Gs2WebSocketSession => (Gs2WebSocketSession) Gs2Session;

		public Gs2ProjectWebSocketClient(Gs2WebSocketSession Gs2WebSocketSession) : base(Gs2WebSocketSession)
		{

		}


        private class CreateAccountTask : Gs2WebSocketSessionTask<Request.CreateAccountRequest, Result.CreateAccountResult>
        {
	        public CreateAccountTask(IGs2Session session, Request.CreateAccountRequest request) : base(session, request)
	        {
	        }

            protected override IGs2SessionRequest CreateRequest(Request.CreateAccountRequest request)
            {
                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);

                jsonWriter.WriteObjectStart();

                if (request.Email != null)
                {
                    jsonWriter.WritePropertyName("email");
                    jsonWriter.Write(request.Email.ToString());
                }
                if (request.FullName != null)
                {
                    jsonWriter.WritePropertyName("fullName");
                    jsonWriter.Write(request.FullName.ToString());
                }
                if (request.CompanyName != null)
                {
                    jsonWriter.WritePropertyName("companyName");
                    jsonWriter.Write(request.CompanyName.ToString());
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
                if (request.RequestId != null)
                {
                    jsonWriter.WritePropertyName("xGs2RequestId");
                    jsonWriter.Write(request.RequestId);
                }

                AddHeader(
                    Session.Credential,
                    "project",
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
        )
		{
			var task = new CreateAccountTask(
			    Gs2WebSocketSession,
			    request
            );
            yield return task;
            callback.Invoke(new AsyncResult<Result.CreateAccountResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.CreateAccountResult> CreateAccount(
            Request.CreateAccountRequest request
        )
		{
		    var task = new CreateAccountTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
#endif


        private class VerifyTask : Gs2WebSocketSessionTask<Request.VerifyRequest, Result.VerifyResult>
        {
	        public VerifyTask(IGs2Session session, Request.VerifyRequest request) : base(session, request)
	        {
	        }

            protected override IGs2SessionRequest CreateRequest(Request.VerifyRequest request)
            {
                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);

                jsonWriter.WriteObjectStart();

                if (request.VerifyToken != null)
                {
                    jsonWriter.WritePropertyName("verifyToken");
                    jsonWriter.Write(request.VerifyToken.ToString());
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
                    "project",
                    "account",
                    "verify",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator Verify(
                Request.VerifyRequest request,
                UnityAction<AsyncResult<Result.VerifyResult>> callback
        )
		{
			var task = new VerifyTask(
			    Gs2WebSocketSession,
			    request
            );
            yield return task;
            callback.Invoke(new AsyncResult<Result.VerifyResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.VerifyResult> Verify(
            Request.VerifyRequest request
        )
		{
		    var task = new VerifyTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
#endif


        private class UpdateAccountTask : Gs2WebSocketSessionTask<Request.UpdateAccountRequest, Result.UpdateAccountResult>
        {
	        public UpdateAccountTask(IGs2Session session, Request.UpdateAccountRequest request) : base(session, request)
	        {
	        }

            protected override IGs2SessionRequest CreateRequest(Request.UpdateAccountRequest request)
            {
                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);

                jsonWriter.WriteObjectStart();

                if (request.Email != null)
                {
                    jsonWriter.WritePropertyName("email");
                    jsonWriter.Write(request.Email.ToString());
                }
                if (request.FullName != null)
                {
                    jsonWriter.WritePropertyName("fullName");
                    jsonWriter.Write(request.FullName.ToString());
                }
                if (request.CompanyName != null)
                {
                    jsonWriter.WritePropertyName("companyName");
                    jsonWriter.Write(request.CompanyName.ToString());
                }
                if (request.Password != null)
                {
                    jsonWriter.WritePropertyName("password");
                    jsonWriter.Write(request.Password.ToString());
                }
                if (request.AccountToken != null)
                {
                    jsonWriter.WritePropertyName("accountToken");
                    jsonWriter.Write(request.AccountToken.ToString());
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
                    "project",
                    "account",
                    "updateAccount",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator UpdateAccount(
                Request.UpdateAccountRequest request,
                UnityAction<AsyncResult<Result.UpdateAccountResult>> callback
        )
		{
			var task = new UpdateAccountTask(
			    Gs2WebSocketSession,
			    request
            );
            yield return task;
            callback.Invoke(new AsyncResult<Result.UpdateAccountResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.UpdateAccountResult> UpdateAccount(
            Request.UpdateAccountRequest request
        )
		{
		    var task = new UpdateAccountTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
#endif


        private class DeleteAccountTask : Gs2WebSocketSessionTask<Request.DeleteAccountRequest, Result.DeleteAccountResult>
        {
	        public DeleteAccountTask(IGs2Session session, Request.DeleteAccountRequest request) : base(session, request)
	        {
	        }

            protected override IGs2SessionRequest CreateRequest(Request.DeleteAccountRequest request)
            {
                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);

                jsonWriter.WriteObjectStart();

                if (request.AccountToken != null)
                {
                    jsonWriter.WritePropertyName("accountToken");
                    jsonWriter.Write(request.AccountToken.ToString());
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
                    "project",
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
        )
		{
			var task = new DeleteAccountTask(
			    Gs2WebSocketSession,
			    request
            );
            yield return task;
            callback.Invoke(new AsyncResult<Result.DeleteAccountResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.DeleteAccountResult> DeleteAccount(
            Request.DeleteAccountRequest request
        )
		{
		    var task = new DeleteAccountTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
#endif


        private class CreateProjectTask : Gs2WebSocketSessionTask<Request.CreateProjectRequest, Result.CreateProjectResult>
        {
	        public CreateProjectTask(IGs2Session session, Request.CreateProjectRequest request) : base(session, request)
	        {
	        }

            protected override IGs2SessionRequest CreateRequest(Request.CreateProjectRequest request)
            {
                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);

                jsonWriter.WriteObjectStart();

                if (request.AccountToken != null)
                {
                    jsonWriter.WritePropertyName("accountToken");
                    jsonWriter.Write(request.AccountToken.ToString());
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
                if (request.Plan != null)
                {
                    jsonWriter.WritePropertyName("plan");
                    jsonWriter.Write(request.Plan.ToString());
                }
                if (request.BillingMethodName != null)
                {
                    jsonWriter.WritePropertyName("billingMethodName");
                    jsonWriter.Write(request.BillingMethodName.ToString());
                }
                if (request.EnableEventBridge != null)
                {
                    jsonWriter.WritePropertyName("enableEventBridge");
                    jsonWriter.Write(request.EnableEventBridge.ToString());
                }
                if (request.EventBridgeAwsAccountId != null)
                {
                    jsonWriter.WritePropertyName("eventBridgeAwsAccountId");
                    jsonWriter.Write(request.EventBridgeAwsAccountId.ToString());
                }
                if (request.EventBridgeAwsRegion != null)
                {
                    jsonWriter.WritePropertyName("eventBridgeAwsRegion");
                    jsonWriter.Write(request.EventBridgeAwsRegion.ToString());
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
                    "project",
                    "project",
                    "createProject",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator CreateProject(
                Request.CreateProjectRequest request,
                UnityAction<AsyncResult<Result.CreateProjectResult>> callback
        )
		{
			var task = new CreateProjectTask(
			    Gs2WebSocketSession,
			    request
            );
            yield return task;
            callback.Invoke(new AsyncResult<Result.CreateProjectResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.CreateProjectResult> CreateProject(
            Request.CreateProjectRequest request
        )
		{
		    var task = new CreateProjectTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
#endif


        private class GetProjectTask : Gs2WebSocketSessionTask<Request.GetProjectRequest, Result.GetProjectResult>
        {
	        public GetProjectTask(IGs2Session session, Request.GetProjectRequest request) : base(session, request)
	        {
	        }

            protected override IGs2SessionRequest CreateRequest(Request.GetProjectRequest request)
            {
                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);

                jsonWriter.WriteObjectStart();

                if (request.AccountToken != null)
                {
                    jsonWriter.WritePropertyName("accountToken");
                    jsonWriter.Write(request.AccountToken.ToString());
                }
                if (request.ProjectName != null)
                {
                    jsonWriter.WritePropertyName("projectName");
                    jsonWriter.Write(request.ProjectName.ToString());
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
                    "project",
                    "project",
                    "getProject",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator GetProject(
                Request.GetProjectRequest request,
                UnityAction<AsyncResult<Result.GetProjectResult>> callback
        )
		{
			var task = new GetProjectTask(
			    Gs2WebSocketSession,
			    request
            );
            yield return task;
            callback.Invoke(new AsyncResult<Result.GetProjectResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.GetProjectResult> GetProject(
            Request.GetProjectRequest request
        )
		{
		    var task = new GetProjectTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
#endif


        private class GetProjectTokenTask : Gs2WebSocketSessionTask<Request.GetProjectTokenRequest, Result.GetProjectTokenResult>
        {
	        public GetProjectTokenTask(IGs2Session session, Request.GetProjectTokenRequest request) : base(session, request)
	        {
	        }

            protected override IGs2SessionRequest CreateRequest(Request.GetProjectTokenRequest request)
            {
                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);

                jsonWriter.WriteObjectStart();

                if (request.ProjectName != null)
                {
                    jsonWriter.WritePropertyName("projectName");
                    jsonWriter.Write(request.ProjectName.ToString());
                }
                if (request.AccountToken != null)
                {
                    jsonWriter.WritePropertyName("accountToken");
                    jsonWriter.Write(request.AccountToken.ToString());
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
                    "project",
                    "project",
                    "getProjectToken",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator GetProjectToken(
                Request.GetProjectTokenRequest request,
                UnityAction<AsyncResult<Result.GetProjectTokenResult>> callback
        )
		{
			var task = new GetProjectTokenTask(
			    Gs2WebSocketSession,
			    request
            );
            yield return task;
            callback.Invoke(new AsyncResult<Result.GetProjectTokenResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.GetProjectTokenResult> GetProjectToken(
            Request.GetProjectTokenRequest request
        )
		{
		    var task = new GetProjectTokenTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
#endif


        private class GetProjectTokenByIdentifierTask : Gs2WebSocketSessionTask<Request.GetProjectTokenByIdentifierRequest, Result.GetProjectTokenByIdentifierResult>
        {
	        public GetProjectTokenByIdentifierTask(IGs2Session session, Request.GetProjectTokenByIdentifierRequest request) : base(session, request)
	        {
	        }

            protected override IGs2SessionRequest CreateRequest(Request.GetProjectTokenByIdentifierRequest request)
            {
                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);

                jsonWriter.WriteObjectStart();

                if (request.AccountName != null)
                {
                    jsonWriter.WritePropertyName("accountName");
                    jsonWriter.Write(request.AccountName.ToString());
                }
                if (request.ProjectName != null)
                {
                    jsonWriter.WritePropertyName("projectName");
                    jsonWriter.Write(request.ProjectName.ToString());
                }
                if (request.UserName != null)
                {
                    jsonWriter.WritePropertyName("userName");
                    jsonWriter.Write(request.UserName.ToString());
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
                if (request.RequestId != null)
                {
                    jsonWriter.WritePropertyName("xGs2RequestId");
                    jsonWriter.Write(request.RequestId);
                }

                AddHeader(
                    Session.Credential,
                    "project",
                    "project",
                    "getProjectTokenByIdentifier",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator GetProjectTokenByIdentifier(
                Request.GetProjectTokenByIdentifierRequest request,
                UnityAction<AsyncResult<Result.GetProjectTokenByIdentifierResult>> callback
        )
		{
			var task = new GetProjectTokenByIdentifierTask(
			    Gs2WebSocketSession,
			    request
            );
            yield return task;
            callback.Invoke(new AsyncResult<Result.GetProjectTokenByIdentifierResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.GetProjectTokenByIdentifierResult> GetProjectTokenByIdentifier(
            Request.GetProjectTokenByIdentifierRequest request
        )
		{
		    var task = new GetProjectTokenByIdentifierTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
#endif


        private class UpdateProjectTask : Gs2WebSocketSessionTask<Request.UpdateProjectRequest, Result.UpdateProjectResult>
        {
	        public UpdateProjectTask(IGs2Session session, Request.UpdateProjectRequest request) : base(session, request)
	        {
	        }

            protected override IGs2SessionRequest CreateRequest(Request.UpdateProjectRequest request)
            {
                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);

                jsonWriter.WriteObjectStart();

                if (request.AccountToken != null)
                {
                    jsonWriter.WritePropertyName("accountToken");
                    jsonWriter.Write(request.AccountToken.ToString());
                }
                if (request.ProjectName != null)
                {
                    jsonWriter.WritePropertyName("projectName");
                    jsonWriter.Write(request.ProjectName.ToString());
                }
                if (request.Description != null)
                {
                    jsonWriter.WritePropertyName("description");
                    jsonWriter.Write(request.Description.ToString());
                }
                if (request.Plan != null)
                {
                    jsonWriter.WritePropertyName("plan");
                    jsonWriter.Write(request.Plan.ToString());
                }
                if (request.BillingMethodName != null)
                {
                    jsonWriter.WritePropertyName("billingMethodName");
                    jsonWriter.Write(request.BillingMethodName.ToString());
                }
                if (request.EnableEventBridge != null)
                {
                    jsonWriter.WritePropertyName("enableEventBridge");
                    jsonWriter.Write(request.EnableEventBridge.ToString());
                }
                if (request.EventBridgeAwsAccountId != null)
                {
                    jsonWriter.WritePropertyName("eventBridgeAwsAccountId");
                    jsonWriter.Write(request.EventBridgeAwsAccountId.ToString());
                }
                if (request.EventBridgeAwsRegion != null)
                {
                    jsonWriter.WritePropertyName("eventBridgeAwsRegion");
                    jsonWriter.Write(request.EventBridgeAwsRegion.ToString());
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
                    "project",
                    "project",
                    "updateProject",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator UpdateProject(
                Request.UpdateProjectRequest request,
                UnityAction<AsyncResult<Result.UpdateProjectResult>> callback
        )
		{
			var task = new UpdateProjectTask(
			    Gs2WebSocketSession,
			    request
            );
            yield return task;
            callback.Invoke(new AsyncResult<Result.UpdateProjectResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.UpdateProjectResult> UpdateProject(
            Request.UpdateProjectRequest request
        )
		{
		    var task = new UpdateProjectTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
#endif


        private class DeleteProjectTask : Gs2WebSocketSessionTask<Request.DeleteProjectRequest, Result.DeleteProjectResult>
        {
	        public DeleteProjectTask(IGs2Session session, Request.DeleteProjectRequest request) : base(session, request)
	        {
	        }

            protected override IGs2SessionRequest CreateRequest(Request.DeleteProjectRequest request)
            {
                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);

                jsonWriter.WriteObjectStart();

                if (request.AccountToken != null)
                {
                    jsonWriter.WritePropertyName("accountToken");
                    jsonWriter.Write(request.AccountToken.ToString());
                }
                if (request.ProjectName != null)
                {
                    jsonWriter.WritePropertyName("projectName");
                    jsonWriter.Write(request.ProjectName.ToString());
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
                    "project",
                    "project",
                    "deleteProject",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator DeleteProject(
                Request.DeleteProjectRequest request,
                UnityAction<AsyncResult<Result.DeleteProjectResult>> callback
        )
		{
			var task = new DeleteProjectTask(
			    Gs2WebSocketSession,
			    request
            );
            yield return task;
            callback.Invoke(new AsyncResult<Result.DeleteProjectResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.DeleteProjectResult> DeleteProject(
            Request.DeleteProjectRequest request
        )
		{
		    var task = new DeleteProjectTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
#endif


        private class CreateBillingMethodTask : Gs2WebSocketSessionTask<Request.CreateBillingMethodRequest, Result.CreateBillingMethodResult>
        {
	        public CreateBillingMethodTask(IGs2Session session, Request.CreateBillingMethodRequest request) : base(session, request)
	        {
	        }

            protected override IGs2SessionRequest CreateRequest(Request.CreateBillingMethodRequest request)
            {
                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);

                jsonWriter.WriteObjectStart();

                if (request.AccountToken != null)
                {
                    jsonWriter.WritePropertyName("accountToken");
                    jsonWriter.Write(request.AccountToken.ToString());
                }
                if (request.Description != null)
                {
                    jsonWriter.WritePropertyName("description");
                    jsonWriter.Write(request.Description.ToString());
                }
                if (request.MethodType != null)
                {
                    jsonWriter.WritePropertyName("methodType");
                    jsonWriter.Write(request.MethodType.ToString());
                }
                if (request.CardCustomerId != null)
                {
                    jsonWriter.WritePropertyName("cardCustomerId");
                    jsonWriter.Write(request.CardCustomerId.ToString());
                }
                if (request.PartnerId != null)
                {
                    jsonWriter.WritePropertyName("partnerId");
                    jsonWriter.Write(request.PartnerId.ToString());
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
                    "project",
                    "billingMethod",
                    "createBillingMethod",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator CreateBillingMethod(
                Request.CreateBillingMethodRequest request,
                UnityAction<AsyncResult<Result.CreateBillingMethodResult>> callback
        )
		{
			var task = new CreateBillingMethodTask(
			    Gs2WebSocketSession,
			    request
            );
            yield return task;
            callback.Invoke(new AsyncResult<Result.CreateBillingMethodResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.CreateBillingMethodResult> CreateBillingMethod(
            Request.CreateBillingMethodRequest request
        )
		{
		    var task = new CreateBillingMethodTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
#endif


        private class GetBillingMethodTask : Gs2WebSocketSessionTask<Request.GetBillingMethodRequest, Result.GetBillingMethodResult>
        {
	        public GetBillingMethodTask(IGs2Session session, Request.GetBillingMethodRequest request) : base(session, request)
	        {
	        }

            protected override IGs2SessionRequest CreateRequest(Request.GetBillingMethodRequest request)
            {
                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);

                jsonWriter.WriteObjectStart();

                if (request.AccountToken != null)
                {
                    jsonWriter.WritePropertyName("accountToken");
                    jsonWriter.Write(request.AccountToken.ToString());
                }
                if (request.BillingMethodName != null)
                {
                    jsonWriter.WritePropertyName("billingMethodName");
                    jsonWriter.Write(request.BillingMethodName.ToString());
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
                    "project",
                    "billingMethod",
                    "getBillingMethod",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator GetBillingMethod(
                Request.GetBillingMethodRequest request,
                UnityAction<AsyncResult<Result.GetBillingMethodResult>> callback
        )
		{
			var task = new GetBillingMethodTask(
			    Gs2WebSocketSession,
			    request
            );
            yield return task;
            callback.Invoke(new AsyncResult<Result.GetBillingMethodResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.GetBillingMethodResult> GetBillingMethod(
            Request.GetBillingMethodRequest request
        )
		{
		    var task = new GetBillingMethodTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
#endif


        private class UpdateBillingMethodTask : Gs2WebSocketSessionTask<Request.UpdateBillingMethodRequest, Result.UpdateBillingMethodResult>
        {
	        public UpdateBillingMethodTask(IGs2Session session, Request.UpdateBillingMethodRequest request) : base(session, request)
	        {
	        }

            protected override IGs2SessionRequest CreateRequest(Request.UpdateBillingMethodRequest request)
            {
                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);

                jsonWriter.WriteObjectStart();

                if (request.AccountToken != null)
                {
                    jsonWriter.WritePropertyName("accountToken");
                    jsonWriter.Write(request.AccountToken.ToString());
                }
                if (request.BillingMethodName != null)
                {
                    jsonWriter.WritePropertyName("billingMethodName");
                    jsonWriter.Write(request.BillingMethodName.ToString());
                }
                if (request.Description != null)
                {
                    jsonWriter.WritePropertyName("description");
                    jsonWriter.Write(request.Description.ToString());
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
                    "project",
                    "billingMethod",
                    "updateBillingMethod",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator UpdateBillingMethod(
                Request.UpdateBillingMethodRequest request,
                UnityAction<AsyncResult<Result.UpdateBillingMethodResult>> callback
        )
		{
			var task = new UpdateBillingMethodTask(
			    Gs2WebSocketSession,
			    request
            );
            yield return task;
            callback.Invoke(new AsyncResult<Result.UpdateBillingMethodResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.UpdateBillingMethodResult> UpdateBillingMethod(
            Request.UpdateBillingMethodRequest request
        )
		{
		    var task = new UpdateBillingMethodTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
#endif


        private class DeleteBillingMethodTask : Gs2WebSocketSessionTask<Request.DeleteBillingMethodRequest, Result.DeleteBillingMethodResult>
        {
	        public DeleteBillingMethodTask(IGs2Session session, Request.DeleteBillingMethodRequest request) : base(session, request)
	        {
	        }

            protected override IGs2SessionRequest CreateRequest(Request.DeleteBillingMethodRequest request)
            {
                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);

                jsonWriter.WriteObjectStart();

                if (request.AccountToken != null)
                {
                    jsonWriter.WritePropertyName("accountToken");
                    jsonWriter.Write(request.AccountToken.ToString());
                }
                if (request.BillingMethodName != null)
                {
                    jsonWriter.WritePropertyName("billingMethodName");
                    jsonWriter.Write(request.BillingMethodName.ToString());
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
                    "project",
                    "billingMethod",
                    "deleteBillingMethod",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator DeleteBillingMethod(
                Request.DeleteBillingMethodRequest request,
                UnityAction<AsyncResult<Result.DeleteBillingMethodResult>> callback
        )
		{
			var task = new DeleteBillingMethodTask(
			    Gs2WebSocketSession,
			    request
            );
            yield return task;
            callback.Invoke(new AsyncResult<Result.DeleteBillingMethodResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.DeleteBillingMethodResult> DeleteBillingMethod(
            Request.DeleteBillingMethodRequest request
        )
		{
		    var task = new DeleteBillingMethodTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
#endif
	}
}