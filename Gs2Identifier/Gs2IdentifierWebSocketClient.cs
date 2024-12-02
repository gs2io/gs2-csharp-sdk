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

namespace Gs2.Gs2Identifier
{
	public class Gs2IdentifierWebSocketClient : AbstractGs2Client
	{

		public static string Endpoint = "identifier";

        protected Gs2WebSocketSession Gs2WebSocketSession => (Gs2WebSocketSession) Gs2Session;

		public Gs2IdentifierWebSocketClient(Gs2WebSocketSession Gs2WebSocketSession) : base(Gs2WebSocketSession)
		{

		}


        public class CreateUserTask : Gs2WebSocketSessionTask<Request.CreateUserRequest, Result.CreateUserResult>
        {
	        public CreateUserTask(IGs2Session session, Request.CreateUserRequest request) : base(session, request)
	        {
	        }

            protected override IGs2SessionRequest CreateRequest(Request.CreateUserRequest request)
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
                    "identifier",
                    "user",
                    "createUser",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator CreateUser(
                Request.CreateUserRequest request,
                UnityAction<AsyncResult<Result.CreateUserResult>> callback
        )
		{
			var task = new CreateUserTask(
			    Gs2WebSocketSession,
			    request
            );
            yield return task;
            callback.Invoke(new AsyncResult<Result.CreateUserResult>(task.Result, task.Error));
        }

		public IFuture<Result.CreateUserResult> CreateUserFuture(
                Request.CreateUserRequest request
        )
		{
			return new CreateUserTask(
			    Gs2WebSocketSession,
			    request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.CreateUserResult> CreateUserAsync(
            Request.CreateUserRequest request
        )
		{
		    var task = new CreateUserTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
    #else
		public CreateUserTask CreateUserAsync(
                Request.CreateUserRequest request
        )
		{
			return new CreateUserTask(
                Gs2WebSocketSession,
			    request
            );
        }
    #endif
#else
		public async Task<Result.CreateUserResult> CreateUserAsync(
            Request.CreateUserRequest request
        )
		{
		    var task = new CreateUserTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
#endif


        public class UpdateUserTask : Gs2WebSocketSessionTask<Request.UpdateUserRequest, Result.UpdateUserResult>
        {
	        public UpdateUserTask(IGs2Session session, Request.UpdateUserRequest request) : base(session, request)
	        {
	        }

            protected override IGs2SessionRequest CreateRequest(Request.UpdateUserRequest request)
            {
                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);

                jsonWriter.WriteObjectStart();

                if (request.UserName != null)
                {
                    jsonWriter.WritePropertyName("userName");
                    jsonWriter.Write(request.UserName.ToString());
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
                if (request.DryRun)
                {
                    jsonWriter.WritePropertyName("xGs2DryRun");
                    jsonWriter.Write("true");
                }

                AddHeader(
                    Session.Credential,
                    "identifier",
                    "user",
                    "updateUser",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator UpdateUser(
                Request.UpdateUserRequest request,
                UnityAction<AsyncResult<Result.UpdateUserResult>> callback
        )
		{
			var task = new UpdateUserTask(
			    Gs2WebSocketSession,
			    request
            );
            yield return task;
            callback.Invoke(new AsyncResult<Result.UpdateUserResult>(task.Result, task.Error));
        }

		public IFuture<Result.UpdateUserResult> UpdateUserFuture(
                Request.UpdateUserRequest request
        )
		{
			return new UpdateUserTask(
			    Gs2WebSocketSession,
			    request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.UpdateUserResult> UpdateUserAsync(
            Request.UpdateUserRequest request
        )
		{
		    var task = new UpdateUserTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
    #else
		public UpdateUserTask UpdateUserAsync(
                Request.UpdateUserRequest request
        )
		{
			return new UpdateUserTask(
                Gs2WebSocketSession,
			    request
            );
        }
    #endif
#else
		public async Task<Result.UpdateUserResult> UpdateUserAsync(
            Request.UpdateUserRequest request
        )
		{
		    var task = new UpdateUserTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
#endif


        public class GetUserTask : Gs2WebSocketSessionTask<Request.GetUserRequest, Result.GetUserResult>
        {
	        public GetUserTask(IGs2Session session, Request.GetUserRequest request) : base(session, request)
	        {
	        }

            protected override IGs2SessionRequest CreateRequest(Request.GetUserRequest request)
            {
                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);

                jsonWriter.WriteObjectStart();

                if (request.UserName != null)
                {
                    jsonWriter.WritePropertyName("userName");
                    jsonWriter.Write(request.UserName.ToString());
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
                    "identifier",
                    "user",
                    "getUser",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator GetUser(
                Request.GetUserRequest request,
                UnityAction<AsyncResult<Result.GetUserResult>> callback
        )
		{
			var task = new GetUserTask(
			    Gs2WebSocketSession,
			    request
            );
            yield return task;
            callback.Invoke(new AsyncResult<Result.GetUserResult>(task.Result, task.Error));
        }

		public IFuture<Result.GetUserResult> GetUserFuture(
                Request.GetUserRequest request
        )
		{
			return new GetUserTask(
			    Gs2WebSocketSession,
			    request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.GetUserResult> GetUserAsync(
            Request.GetUserRequest request
        )
		{
		    var task = new GetUserTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
    #else
		public GetUserTask GetUserAsync(
                Request.GetUserRequest request
        )
		{
			return new GetUserTask(
                Gs2WebSocketSession,
			    request
            );
        }
    #endif
#else
		public async Task<Result.GetUserResult> GetUserAsync(
            Request.GetUserRequest request
        )
		{
		    var task = new GetUserTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
#endif


        public class DeleteUserTask : Gs2WebSocketSessionTask<Request.DeleteUserRequest, Result.DeleteUserResult>
        {
	        public DeleteUserTask(IGs2Session session, Request.DeleteUserRequest request) : base(session, request)
	        {
	        }

            protected override IGs2SessionRequest CreateRequest(Request.DeleteUserRequest request)
            {
                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);

                jsonWriter.WriteObjectStart();

                if (request.UserName != null)
                {
                    jsonWriter.WritePropertyName("userName");
                    jsonWriter.Write(request.UserName.ToString());
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
                    "identifier",
                    "user",
                    "deleteUser",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator DeleteUser(
                Request.DeleteUserRequest request,
                UnityAction<AsyncResult<Result.DeleteUserResult>> callback
        )
		{
			var task = new DeleteUserTask(
			    Gs2WebSocketSession,
			    request
            );
            yield return task;
            callback.Invoke(new AsyncResult<Result.DeleteUserResult>(task.Result, task.Error));
        }

		public IFuture<Result.DeleteUserResult> DeleteUserFuture(
                Request.DeleteUserRequest request
        )
		{
			return new DeleteUserTask(
			    Gs2WebSocketSession,
			    request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.DeleteUserResult> DeleteUserAsync(
            Request.DeleteUserRequest request
        )
		{
		    var task = new DeleteUserTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
    #else
		public DeleteUserTask DeleteUserAsync(
                Request.DeleteUserRequest request
        )
		{
			return new DeleteUserTask(
                Gs2WebSocketSession,
			    request
            );
        }
    #endif
#else
		public async Task<Result.DeleteUserResult> DeleteUserAsync(
            Request.DeleteUserRequest request
        )
		{
		    var task = new DeleteUserTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
#endif


        public class CreateIdentifierTask : Gs2WebSocketSessionTask<Request.CreateIdentifierRequest, Result.CreateIdentifierResult>
        {
	        public CreateIdentifierTask(IGs2Session session, Request.CreateIdentifierRequest request) : base(session, request)
	        {
	        }

            protected override IGs2SessionRequest CreateRequest(Request.CreateIdentifierRequest request)
            {
                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);

                jsonWriter.WriteObjectStart();

                if (request.UserName != null)
                {
                    jsonWriter.WritePropertyName("userName");
                    jsonWriter.Write(request.UserName.ToString());
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
                    "identifier",
                    "identifier",
                    "createIdentifier",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator CreateIdentifier(
                Request.CreateIdentifierRequest request,
                UnityAction<AsyncResult<Result.CreateIdentifierResult>> callback
        )
		{
			var task = new CreateIdentifierTask(
			    Gs2WebSocketSession,
			    request
            );
            yield return task;
            callback.Invoke(new AsyncResult<Result.CreateIdentifierResult>(task.Result, task.Error));
        }

		public IFuture<Result.CreateIdentifierResult> CreateIdentifierFuture(
                Request.CreateIdentifierRequest request
        )
		{
			return new CreateIdentifierTask(
			    Gs2WebSocketSession,
			    request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.CreateIdentifierResult> CreateIdentifierAsync(
            Request.CreateIdentifierRequest request
        )
		{
		    var task = new CreateIdentifierTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
    #else
		public CreateIdentifierTask CreateIdentifierAsync(
                Request.CreateIdentifierRequest request
        )
		{
			return new CreateIdentifierTask(
                Gs2WebSocketSession,
			    request
            );
        }
    #endif
#else
		public async Task<Result.CreateIdentifierResult> CreateIdentifierAsync(
            Request.CreateIdentifierRequest request
        )
		{
		    var task = new CreateIdentifierTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
#endif


        public class GetIdentifierTask : Gs2WebSocketSessionTask<Request.GetIdentifierRequest, Result.GetIdentifierResult>
        {
	        public GetIdentifierTask(IGs2Session session, Request.GetIdentifierRequest request) : base(session, request)
	        {
	        }

            protected override IGs2SessionRequest CreateRequest(Request.GetIdentifierRequest request)
            {
                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);

                jsonWriter.WriteObjectStart();

                if (request.UserName != null)
                {
                    jsonWriter.WritePropertyName("userName");
                    jsonWriter.Write(request.UserName.ToString());
                }
                if (request.ClientId != null)
                {
                    jsonWriter.WritePropertyName("clientId");
                    jsonWriter.Write(request.ClientId.ToString());
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
                    "identifier",
                    "identifier",
                    "getIdentifier",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator GetIdentifier(
                Request.GetIdentifierRequest request,
                UnityAction<AsyncResult<Result.GetIdentifierResult>> callback
        )
		{
			var task = new GetIdentifierTask(
			    Gs2WebSocketSession,
			    request
            );
            yield return task;
            callback.Invoke(new AsyncResult<Result.GetIdentifierResult>(task.Result, task.Error));
        }

		public IFuture<Result.GetIdentifierResult> GetIdentifierFuture(
                Request.GetIdentifierRequest request
        )
		{
			return new GetIdentifierTask(
			    Gs2WebSocketSession,
			    request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.GetIdentifierResult> GetIdentifierAsync(
            Request.GetIdentifierRequest request
        )
		{
		    var task = new GetIdentifierTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
    #else
		public GetIdentifierTask GetIdentifierAsync(
                Request.GetIdentifierRequest request
        )
		{
			return new GetIdentifierTask(
                Gs2WebSocketSession,
			    request
            );
        }
    #endif
#else
		public async Task<Result.GetIdentifierResult> GetIdentifierAsync(
            Request.GetIdentifierRequest request
        )
		{
		    var task = new GetIdentifierTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
#endif


        public class DeleteIdentifierTask : Gs2WebSocketSessionTask<Request.DeleteIdentifierRequest, Result.DeleteIdentifierResult>
        {
	        public DeleteIdentifierTask(IGs2Session session, Request.DeleteIdentifierRequest request) : base(session, request)
	        {
	        }

            protected override IGs2SessionRequest CreateRequest(Request.DeleteIdentifierRequest request)
            {
                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);

                jsonWriter.WriteObjectStart();

                if (request.UserName != null)
                {
                    jsonWriter.WritePropertyName("userName");
                    jsonWriter.Write(request.UserName.ToString());
                }
                if (request.ClientId != null)
                {
                    jsonWriter.WritePropertyName("clientId");
                    jsonWriter.Write(request.ClientId.ToString());
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
                    "identifier",
                    "identifier",
                    "deleteIdentifier",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator DeleteIdentifier(
                Request.DeleteIdentifierRequest request,
                UnityAction<AsyncResult<Result.DeleteIdentifierResult>> callback
        )
		{
			var task = new DeleteIdentifierTask(
			    Gs2WebSocketSession,
			    request
            );
            yield return task;
            callback.Invoke(new AsyncResult<Result.DeleteIdentifierResult>(task.Result, task.Error));
        }

		public IFuture<Result.DeleteIdentifierResult> DeleteIdentifierFuture(
                Request.DeleteIdentifierRequest request
        )
		{
			return new DeleteIdentifierTask(
			    Gs2WebSocketSession,
			    request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.DeleteIdentifierResult> DeleteIdentifierAsync(
            Request.DeleteIdentifierRequest request
        )
		{
		    var task = new DeleteIdentifierTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
    #else
		public DeleteIdentifierTask DeleteIdentifierAsync(
                Request.DeleteIdentifierRequest request
        )
		{
			return new DeleteIdentifierTask(
                Gs2WebSocketSession,
			    request
            );
        }
    #endif
#else
		public async Task<Result.DeleteIdentifierResult> DeleteIdentifierAsync(
            Request.DeleteIdentifierRequest request
        )
		{
		    var task = new DeleteIdentifierTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
#endif


        public class CreatePasswordTask : Gs2WebSocketSessionTask<Request.CreatePasswordRequest, Result.CreatePasswordResult>
        {
	        public CreatePasswordTask(IGs2Session session, Request.CreatePasswordRequest request) : base(session, request)
	        {
	        }

            protected override IGs2SessionRequest CreateRequest(Request.CreatePasswordRequest request)
            {
                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);

                jsonWriter.WriteObjectStart();

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
                if (request.DryRun)
                {
                    jsonWriter.WritePropertyName("xGs2DryRun");
                    jsonWriter.Write("true");
                }

                AddHeader(
                    Session.Credential,
                    "identifier",
                    "password",
                    "createPassword",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator CreatePassword(
                Request.CreatePasswordRequest request,
                UnityAction<AsyncResult<Result.CreatePasswordResult>> callback
        )
		{
			var task = new CreatePasswordTask(
			    Gs2WebSocketSession,
			    request
            );
            yield return task;
            callback.Invoke(new AsyncResult<Result.CreatePasswordResult>(task.Result, task.Error));
        }

		public IFuture<Result.CreatePasswordResult> CreatePasswordFuture(
                Request.CreatePasswordRequest request
        )
		{
			return new CreatePasswordTask(
			    Gs2WebSocketSession,
			    request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.CreatePasswordResult> CreatePasswordAsync(
            Request.CreatePasswordRequest request
        )
		{
		    var task = new CreatePasswordTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
    #else
		public CreatePasswordTask CreatePasswordAsync(
                Request.CreatePasswordRequest request
        )
		{
			return new CreatePasswordTask(
                Gs2WebSocketSession,
			    request
            );
        }
    #endif
#else
		public async Task<Result.CreatePasswordResult> CreatePasswordAsync(
            Request.CreatePasswordRequest request
        )
		{
		    var task = new CreatePasswordTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
#endif


        public class GetPasswordTask : Gs2WebSocketSessionTask<Request.GetPasswordRequest, Result.GetPasswordResult>
        {
	        public GetPasswordTask(IGs2Session session, Request.GetPasswordRequest request) : base(session, request)
	        {
	        }

            protected override IGs2SessionRequest CreateRequest(Request.GetPasswordRequest request)
            {
                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);

                jsonWriter.WriteObjectStart();

                if (request.UserName != null)
                {
                    jsonWriter.WritePropertyName("userName");
                    jsonWriter.Write(request.UserName.ToString());
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
                    "identifier",
                    "password",
                    "getPassword",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator GetPassword(
                Request.GetPasswordRequest request,
                UnityAction<AsyncResult<Result.GetPasswordResult>> callback
        )
		{
			var task = new GetPasswordTask(
			    Gs2WebSocketSession,
			    request
            );
            yield return task;
            callback.Invoke(new AsyncResult<Result.GetPasswordResult>(task.Result, task.Error));
        }

		public IFuture<Result.GetPasswordResult> GetPasswordFuture(
                Request.GetPasswordRequest request
        )
		{
			return new GetPasswordTask(
			    Gs2WebSocketSession,
			    request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.GetPasswordResult> GetPasswordAsync(
            Request.GetPasswordRequest request
        )
		{
		    var task = new GetPasswordTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
    #else
		public GetPasswordTask GetPasswordAsync(
                Request.GetPasswordRequest request
        )
		{
			return new GetPasswordTask(
                Gs2WebSocketSession,
			    request
            );
        }
    #endif
#else
		public async Task<Result.GetPasswordResult> GetPasswordAsync(
            Request.GetPasswordRequest request
        )
		{
		    var task = new GetPasswordTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
#endif


        public class EnableMfaTask : Gs2WebSocketSessionTask<Request.EnableMfaRequest, Result.EnableMfaResult>
        {
	        public EnableMfaTask(IGs2Session session, Request.EnableMfaRequest request) : base(session, request)
	        {
	        }

            protected override IGs2SessionRequest CreateRequest(Request.EnableMfaRequest request)
            {
                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);

                jsonWriter.WriteObjectStart();

                if (request.UserName != null)
                {
                    jsonWriter.WritePropertyName("userName");
                    jsonWriter.Write(request.UserName.ToString());
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
                    "identifier",
                    "password",
                    "enableMfa",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator EnableMfa(
                Request.EnableMfaRequest request,
                UnityAction<AsyncResult<Result.EnableMfaResult>> callback
        )
		{
			var task = new EnableMfaTask(
			    Gs2WebSocketSession,
			    request
            );
            yield return task;
            callback.Invoke(new AsyncResult<Result.EnableMfaResult>(task.Result, task.Error));
        }

		public IFuture<Result.EnableMfaResult> EnableMfaFuture(
                Request.EnableMfaRequest request
        )
		{
			return new EnableMfaTask(
			    Gs2WebSocketSession,
			    request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.EnableMfaResult> EnableMfaAsync(
            Request.EnableMfaRequest request
        )
		{
		    var task = new EnableMfaTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
    #else
		public EnableMfaTask EnableMfaAsync(
                Request.EnableMfaRequest request
        )
		{
			return new EnableMfaTask(
                Gs2WebSocketSession,
			    request
            );
        }
    #endif
#else
		public async Task<Result.EnableMfaResult> EnableMfaAsync(
            Request.EnableMfaRequest request
        )
		{
		    var task = new EnableMfaTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
#endif


        public class ChallengeMfaTask : Gs2WebSocketSessionTask<Request.ChallengeMfaRequest, Result.ChallengeMfaResult>
        {
	        public ChallengeMfaTask(IGs2Session session, Request.ChallengeMfaRequest request) : base(session, request)
	        {
	        }

            protected override IGs2SessionRequest CreateRequest(Request.ChallengeMfaRequest request)
            {
                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);

                jsonWriter.WriteObjectStart();

                if (request.UserName != null)
                {
                    jsonWriter.WritePropertyName("userName");
                    jsonWriter.Write(request.UserName.ToString());
                }
                if (request.Passcode != null)
                {
                    jsonWriter.WritePropertyName("passcode");
                    jsonWriter.Write(request.Passcode.ToString());
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
                    "identifier",
                    "password",
                    "challengeMfa",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator ChallengeMfa(
                Request.ChallengeMfaRequest request,
                UnityAction<AsyncResult<Result.ChallengeMfaResult>> callback
        )
		{
			var task = new ChallengeMfaTask(
			    Gs2WebSocketSession,
			    request
            );
            yield return task;
            callback.Invoke(new AsyncResult<Result.ChallengeMfaResult>(task.Result, task.Error));
        }

		public IFuture<Result.ChallengeMfaResult> ChallengeMfaFuture(
                Request.ChallengeMfaRequest request
        )
		{
			return new ChallengeMfaTask(
			    Gs2WebSocketSession,
			    request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.ChallengeMfaResult> ChallengeMfaAsync(
            Request.ChallengeMfaRequest request
        )
		{
		    var task = new ChallengeMfaTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
    #else
		public ChallengeMfaTask ChallengeMfaAsync(
                Request.ChallengeMfaRequest request
        )
		{
			return new ChallengeMfaTask(
                Gs2WebSocketSession,
			    request
            );
        }
    #endif
#else
		public async Task<Result.ChallengeMfaResult> ChallengeMfaAsync(
            Request.ChallengeMfaRequest request
        )
		{
		    var task = new ChallengeMfaTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
#endif


        public class DisableMfaTask : Gs2WebSocketSessionTask<Request.DisableMfaRequest, Result.DisableMfaResult>
        {
	        public DisableMfaTask(IGs2Session session, Request.DisableMfaRequest request) : base(session, request)
	        {
	        }

            protected override IGs2SessionRequest CreateRequest(Request.DisableMfaRequest request)
            {
                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);

                jsonWriter.WriteObjectStart();

                if (request.UserName != null)
                {
                    jsonWriter.WritePropertyName("userName");
                    jsonWriter.Write(request.UserName.ToString());
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
                    "identifier",
                    "password",
                    "disableMfa",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator DisableMfa(
                Request.DisableMfaRequest request,
                UnityAction<AsyncResult<Result.DisableMfaResult>> callback
        )
		{
			var task = new DisableMfaTask(
			    Gs2WebSocketSession,
			    request
            );
            yield return task;
            callback.Invoke(new AsyncResult<Result.DisableMfaResult>(task.Result, task.Error));
        }

		public IFuture<Result.DisableMfaResult> DisableMfaFuture(
                Request.DisableMfaRequest request
        )
		{
			return new DisableMfaTask(
			    Gs2WebSocketSession,
			    request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.DisableMfaResult> DisableMfaAsync(
            Request.DisableMfaRequest request
        )
		{
		    var task = new DisableMfaTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
    #else
		public DisableMfaTask DisableMfaAsync(
                Request.DisableMfaRequest request
        )
		{
			return new DisableMfaTask(
                Gs2WebSocketSession,
			    request
            );
        }
    #endif
#else
		public async Task<Result.DisableMfaResult> DisableMfaAsync(
            Request.DisableMfaRequest request
        )
		{
		    var task = new DisableMfaTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
#endif


        public class DeletePasswordTask : Gs2WebSocketSessionTask<Request.DeletePasswordRequest, Result.DeletePasswordResult>
        {
	        public DeletePasswordTask(IGs2Session session, Request.DeletePasswordRequest request) : base(session, request)
	        {
	        }

            protected override IGs2SessionRequest CreateRequest(Request.DeletePasswordRequest request)
            {
                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);

                jsonWriter.WriteObjectStart();

                if (request.UserName != null)
                {
                    jsonWriter.WritePropertyName("userName");
                    jsonWriter.Write(request.UserName.ToString());
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
                    "identifier",
                    "password",
                    "deletePassword",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator DeletePassword(
                Request.DeletePasswordRequest request,
                UnityAction<AsyncResult<Result.DeletePasswordResult>> callback
        )
		{
			var task = new DeletePasswordTask(
			    Gs2WebSocketSession,
			    request
            );
            yield return task;
            callback.Invoke(new AsyncResult<Result.DeletePasswordResult>(task.Result, task.Error));
        }

		public IFuture<Result.DeletePasswordResult> DeletePasswordFuture(
                Request.DeletePasswordRequest request
        )
		{
			return new DeletePasswordTask(
			    Gs2WebSocketSession,
			    request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.DeletePasswordResult> DeletePasswordAsync(
            Request.DeletePasswordRequest request
        )
		{
		    var task = new DeletePasswordTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
    #else
		public DeletePasswordTask DeletePasswordAsync(
                Request.DeletePasswordRequest request
        )
		{
			return new DeletePasswordTask(
                Gs2WebSocketSession,
			    request
            );
        }
    #endif
#else
		public async Task<Result.DeletePasswordResult> DeletePasswordAsync(
            Request.DeletePasswordRequest request
        )
		{
		    var task = new DeletePasswordTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
#endif


        public class LoginByUserTask : Gs2WebSocketSessionTask<Request.LoginByUserRequest, Result.LoginByUserResult>
        {
	        public LoginByUserTask(IGs2Session session, Request.LoginByUserRequest request) : base(session, request)
	        {
	        }

            protected override IGs2SessionRequest CreateRequest(Request.LoginByUserRequest request)
            {
                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);

                jsonWriter.WriteObjectStart();

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
                if (request.Otp != null)
                {
                    jsonWriter.WritePropertyName("otp");
                    jsonWriter.Write(request.Otp.ToString());
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
                    "identifier",
                    "projectToken",
                    "loginByUser",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator LoginByUser(
                Request.LoginByUserRequest request,
                UnityAction<AsyncResult<Result.LoginByUserResult>> callback
        )
		{
			var task = new LoginByUserTask(
			    Gs2WebSocketSession,
			    request
            );
            yield return task;
            callback.Invoke(new AsyncResult<Result.LoginByUserResult>(task.Result, task.Error));
        }

		public IFuture<Result.LoginByUserResult> LoginByUserFuture(
                Request.LoginByUserRequest request
        )
		{
			return new LoginByUserTask(
			    Gs2WebSocketSession,
			    request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.LoginByUserResult> LoginByUserAsync(
            Request.LoginByUserRequest request
        )
		{
		    var task = new LoginByUserTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
    #else
		public LoginByUserTask LoginByUserAsync(
                Request.LoginByUserRequest request
        )
		{
			return new LoginByUserTask(
                Gs2WebSocketSession,
			    request
            );
        }
    #endif
#else
		public async Task<Result.LoginByUserResult> LoginByUserAsync(
            Request.LoginByUserRequest request
        )
		{
		    var task = new LoginByUserTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
#endif
	}
}