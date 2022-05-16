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

namespace Gs2.Gs2Project
{
	public class Gs2ProjectWebSocketClient : AbstractGs2Client
	{

		public static string Endpoint = "project";

        protected Gs2WebSocketSession Gs2WebSocketSession => (Gs2WebSocketSession) Gs2Session;

		public Gs2ProjectWebSocketClient(Gs2WebSocketSession Gs2WebSocketSession) : base(Gs2WebSocketSession)
		{

		}


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
                if (request.Lang != null)
                {
                    jsonWriter.WritePropertyName("lang");
                    jsonWriter.Write(request.Lang.ToString());
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

		public IFuture<Result.CreateAccountResult> CreateAccountFuture(
                Request.CreateAccountRequest request
        )
		{
			return new CreateAccountTask(
			    Gs2WebSocketSession,
			    request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.CreateAccountResult> CreateAccountAsync(
            Request.CreateAccountRequest request
        )
		{
		    var task = new CreateAccountTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
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
		public async Task<Result.CreateAccountResult> CreateAccountAsync(
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


        public class VerifyTask : Gs2WebSocketSessionTask<Request.VerifyRequest, Result.VerifyResult>
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

		public IFuture<Result.VerifyResult> VerifyFuture(
                Request.VerifyRequest request
        )
		{
			return new VerifyTask(
			    Gs2WebSocketSession,
			    request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.VerifyResult> VerifyAsync(
            Request.VerifyRequest request
        )
		{
		    var task = new VerifyTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
    #else
		public VerifyTask VerifyAsync(
                Request.VerifyRequest request
        )
		{
			return new VerifyTask(
                Gs2WebSocketSession,
			    request
            );
        }
    #endif
#else
		public async Task<Result.VerifyResult> VerifyAsync(
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


        public class ForgetTask : Gs2WebSocketSessionTask<Request.ForgetRequest, Result.ForgetResult>
        {
	        public ForgetTask(IGs2Session session, Request.ForgetRequest request) : base(session, request)
	        {
	        }

            protected override IGs2SessionRequest CreateRequest(Request.ForgetRequest request)
            {
                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);

                jsonWriter.WriteObjectStart();

                if (request.Email != null)
                {
                    jsonWriter.WritePropertyName("email");
                    jsonWriter.Write(request.Email.ToString());
                }
                if (request.Lang != null)
                {
                    jsonWriter.WritePropertyName("lang");
                    jsonWriter.Write(request.Lang.ToString());
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
                    "forget",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator Forget(
                Request.ForgetRequest request,
                UnityAction<AsyncResult<Result.ForgetResult>> callback
        )
		{
			var task = new ForgetTask(
			    Gs2WebSocketSession,
			    request
            );
            yield return task;
            callback.Invoke(new AsyncResult<Result.ForgetResult>(task.Result, task.Error));
        }

		public IFuture<Result.ForgetResult> ForgetFuture(
                Request.ForgetRequest request
        )
		{
			return new ForgetTask(
			    Gs2WebSocketSession,
			    request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.ForgetResult> ForgetAsync(
            Request.ForgetRequest request
        )
		{
		    var task = new ForgetTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
    #else
		public ForgetTask ForgetAsync(
                Request.ForgetRequest request
        )
		{
			return new ForgetTask(
                Gs2WebSocketSession,
			    request
            );
        }
    #endif
#else
		public async Task<Result.ForgetResult> ForgetAsync(
            Request.ForgetRequest request
        )
		{
		    var task = new ForgetTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
#endif


        public class UpdateAccountTask : Gs2WebSocketSessionTask<Request.UpdateAccountRequest, Result.UpdateAccountResult>
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

		public IFuture<Result.UpdateAccountResult> UpdateAccountFuture(
                Request.UpdateAccountRequest request
        )
		{
			return new UpdateAccountTask(
			    Gs2WebSocketSession,
			    request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.UpdateAccountResult> UpdateAccountAsync(
            Request.UpdateAccountRequest request
        )
		{
		    var task = new UpdateAccountTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
    #else
		public UpdateAccountTask UpdateAccountAsync(
                Request.UpdateAccountRequest request
        )
		{
			return new UpdateAccountTask(
                Gs2WebSocketSession,
			    request
            );
        }
    #endif
#else
		public async Task<Result.UpdateAccountResult> UpdateAccountAsync(
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

		public IFuture<Result.DeleteAccountResult> DeleteAccountFuture(
                Request.DeleteAccountRequest request
        )
		{
			return new DeleteAccountTask(
			    Gs2WebSocketSession,
			    request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.DeleteAccountResult> DeleteAccountAsync(
            Request.DeleteAccountRequest request
        )
		{
		    var task = new DeleteAccountTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
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
		public async Task<Result.DeleteAccountResult> DeleteAccountAsync(
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


        public class CreateProjectTask : Gs2WebSocketSessionTask<Request.CreateProjectRequest, Result.CreateProjectResult>
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

		public IFuture<Result.CreateProjectResult> CreateProjectFuture(
                Request.CreateProjectRequest request
        )
		{
			return new CreateProjectTask(
			    Gs2WebSocketSession,
			    request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.CreateProjectResult> CreateProjectAsync(
            Request.CreateProjectRequest request
        )
		{
		    var task = new CreateProjectTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
    #else
		public CreateProjectTask CreateProjectAsync(
                Request.CreateProjectRequest request
        )
		{
			return new CreateProjectTask(
                Gs2WebSocketSession,
			    request
            );
        }
    #endif
#else
		public async Task<Result.CreateProjectResult> CreateProjectAsync(
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


        public class GetProjectTask : Gs2WebSocketSessionTask<Request.GetProjectRequest, Result.GetProjectResult>
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

		public IFuture<Result.GetProjectResult> GetProjectFuture(
                Request.GetProjectRequest request
        )
		{
			return new GetProjectTask(
			    Gs2WebSocketSession,
			    request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.GetProjectResult> GetProjectAsync(
            Request.GetProjectRequest request
        )
		{
		    var task = new GetProjectTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
    #else
		public GetProjectTask GetProjectAsync(
                Request.GetProjectRequest request
        )
		{
			return new GetProjectTask(
                Gs2WebSocketSession,
			    request
            );
        }
    #endif
#else
		public async Task<Result.GetProjectResult> GetProjectAsync(
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


        public class GetProjectTokenTask : Gs2WebSocketSessionTask<Request.GetProjectTokenRequest, Result.GetProjectTokenResult>
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

		public IFuture<Result.GetProjectTokenResult> GetProjectTokenFuture(
                Request.GetProjectTokenRequest request
        )
		{
			return new GetProjectTokenTask(
			    Gs2WebSocketSession,
			    request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.GetProjectTokenResult> GetProjectTokenAsync(
            Request.GetProjectTokenRequest request
        )
		{
		    var task = new GetProjectTokenTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
    #else
		public GetProjectTokenTask GetProjectTokenAsync(
                Request.GetProjectTokenRequest request
        )
		{
			return new GetProjectTokenTask(
                Gs2WebSocketSession,
			    request
            );
        }
    #endif
#else
		public async Task<Result.GetProjectTokenResult> GetProjectTokenAsync(
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


        public class GetProjectTokenByIdentifierTask : Gs2WebSocketSessionTask<Request.GetProjectTokenByIdentifierRequest, Result.GetProjectTokenByIdentifierResult>
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

		public IFuture<Result.GetProjectTokenByIdentifierResult> GetProjectTokenByIdentifierFuture(
                Request.GetProjectTokenByIdentifierRequest request
        )
		{
			return new GetProjectTokenByIdentifierTask(
			    Gs2WebSocketSession,
			    request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.GetProjectTokenByIdentifierResult> GetProjectTokenByIdentifierAsync(
            Request.GetProjectTokenByIdentifierRequest request
        )
		{
		    var task = new GetProjectTokenByIdentifierTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
    #else
		public GetProjectTokenByIdentifierTask GetProjectTokenByIdentifierAsync(
                Request.GetProjectTokenByIdentifierRequest request
        )
		{
			return new GetProjectTokenByIdentifierTask(
                Gs2WebSocketSession,
			    request
            );
        }
    #endif
#else
		public async Task<Result.GetProjectTokenByIdentifierResult> GetProjectTokenByIdentifierAsync(
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


        public class UpdateProjectTask : Gs2WebSocketSessionTask<Request.UpdateProjectRequest, Result.UpdateProjectResult>
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

		public IFuture<Result.UpdateProjectResult> UpdateProjectFuture(
                Request.UpdateProjectRequest request
        )
		{
			return new UpdateProjectTask(
			    Gs2WebSocketSession,
			    request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.UpdateProjectResult> UpdateProjectAsync(
            Request.UpdateProjectRequest request
        )
		{
		    var task = new UpdateProjectTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
    #else
		public UpdateProjectTask UpdateProjectAsync(
                Request.UpdateProjectRequest request
        )
		{
			return new UpdateProjectTask(
                Gs2WebSocketSession,
			    request
            );
        }
    #endif
#else
		public async Task<Result.UpdateProjectResult> UpdateProjectAsync(
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


        public class DeleteProjectTask : Gs2WebSocketSessionTask<Request.DeleteProjectRequest, Result.DeleteProjectResult>
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

		public IFuture<Result.DeleteProjectResult> DeleteProjectFuture(
                Request.DeleteProjectRequest request
        )
		{
			return new DeleteProjectTask(
			    Gs2WebSocketSession,
			    request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.DeleteProjectResult> DeleteProjectAsync(
            Request.DeleteProjectRequest request
        )
		{
		    var task = new DeleteProjectTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
    #else
		public DeleteProjectTask DeleteProjectAsync(
                Request.DeleteProjectRequest request
        )
		{
			return new DeleteProjectTask(
                Gs2WebSocketSession,
			    request
            );
        }
    #endif
#else
		public async Task<Result.DeleteProjectResult> DeleteProjectAsync(
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


        public class CreateBillingMethodTask : Gs2WebSocketSessionTask<Request.CreateBillingMethodRequest, Result.CreateBillingMethodResult>
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

		public IFuture<Result.CreateBillingMethodResult> CreateBillingMethodFuture(
                Request.CreateBillingMethodRequest request
        )
		{
			return new CreateBillingMethodTask(
			    Gs2WebSocketSession,
			    request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.CreateBillingMethodResult> CreateBillingMethodAsync(
            Request.CreateBillingMethodRequest request
        )
		{
		    var task = new CreateBillingMethodTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
    #else
		public CreateBillingMethodTask CreateBillingMethodAsync(
                Request.CreateBillingMethodRequest request
        )
		{
			return new CreateBillingMethodTask(
                Gs2WebSocketSession,
			    request
            );
        }
    #endif
#else
		public async Task<Result.CreateBillingMethodResult> CreateBillingMethodAsync(
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


        public class GetBillingMethodTask : Gs2WebSocketSessionTask<Request.GetBillingMethodRequest, Result.GetBillingMethodResult>
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

		public IFuture<Result.GetBillingMethodResult> GetBillingMethodFuture(
                Request.GetBillingMethodRequest request
        )
		{
			return new GetBillingMethodTask(
			    Gs2WebSocketSession,
			    request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.GetBillingMethodResult> GetBillingMethodAsync(
            Request.GetBillingMethodRequest request
        )
		{
		    var task = new GetBillingMethodTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
    #else
		public GetBillingMethodTask GetBillingMethodAsync(
                Request.GetBillingMethodRequest request
        )
		{
			return new GetBillingMethodTask(
                Gs2WebSocketSession,
			    request
            );
        }
    #endif
#else
		public async Task<Result.GetBillingMethodResult> GetBillingMethodAsync(
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


        public class UpdateBillingMethodTask : Gs2WebSocketSessionTask<Request.UpdateBillingMethodRequest, Result.UpdateBillingMethodResult>
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

		public IFuture<Result.UpdateBillingMethodResult> UpdateBillingMethodFuture(
                Request.UpdateBillingMethodRequest request
        )
		{
			return new UpdateBillingMethodTask(
			    Gs2WebSocketSession,
			    request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.UpdateBillingMethodResult> UpdateBillingMethodAsync(
            Request.UpdateBillingMethodRequest request
        )
		{
		    var task = new UpdateBillingMethodTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
    #else
		public UpdateBillingMethodTask UpdateBillingMethodAsync(
                Request.UpdateBillingMethodRequest request
        )
		{
			return new UpdateBillingMethodTask(
                Gs2WebSocketSession,
			    request
            );
        }
    #endif
#else
		public async Task<Result.UpdateBillingMethodResult> UpdateBillingMethodAsync(
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


        public class DeleteBillingMethodTask : Gs2WebSocketSessionTask<Request.DeleteBillingMethodRequest, Result.DeleteBillingMethodResult>
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

		public IFuture<Result.DeleteBillingMethodResult> DeleteBillingMethodFuture(
                Request.DeleteBillingMethodRequest request
        )
		{
			return new DeleteBillingMethodTask(
			    Gs2WebSocketSession,
			    request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.DeleteBillingMethodResult> DeleteBillingMethodAsync(
            Request.DeleteBillingMethodRequest request
        )
		{
		    var task = new DeleteBillingMethodTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
    #else
		public DeleteBillingMethodTask DeleteBillingMethodAsync(
                Request.DeleteBillingMethodRequest request
        )
		{
			return new DeleteBillingMethodTask(
                Gs2WebSocketSession,
			    request
            );
        }
    #endif
#else
		public async Task<Result.DeleteBillingMethodResult> DeleteBillingMethodAsync(
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