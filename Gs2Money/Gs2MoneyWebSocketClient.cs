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

namespace Gs2.Gs2Money
{
	public class Gs2MoneyWebSocketClient : AbstractGs2Client
	{

		public static string Endpoint = "money";

        protected Gs2WebSocketSession Gs2WebSocketSession => (Gs2WebSocketSession) Gs2Session;

		public Gs2MoneyWebSocketClient(Gs2WebSocketSession Gs2WebSocketSession) : base(Gs2WebSocketSession)
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
                if (request.Priority != null)
                {
                    jsonWriter.WritePropertyName("priority");
                    jsonWriter.Write(request.Priority.ToString());
                }
                if (request.ShareFree != null)
                {
                    jsonWriter.WritePropertyName("shareFree");
                    jsonWriter.Write(request.ShareFree.ToString());
                }
                if (request.Currency != null)
                {
                    jsonWriter.WritePropertyName("currency");
                    jsonWriter.Write(request.Currency.ToString());
                }
                if (request.AppleKey != null)
                {
                    jsonWriter.WritePropertyName("appleKey");
                    jsonWriter.Write(request.AppleKey.ToString());
                }
                if (request.GoogleKey != null)
                {
                    jsonWriter.WritePropertyName("googleKey");
                    jsonWriter.Write(request.GoogleKey.ToString());
                }
                if (request.EnableFakeReceipt != null)
                {
                    jsonWriter.WritePropertyName("enableFakeReceipt");
                    jsonWriter.Write(request.EnableFakeReceipt.ToString());
                }
                if (request.CreateWalletScript != null)
                {
                    jsonWriter.WritePropertyName("createWalletScript");
                    request.CreateWalletScript.WriteJson(jsonWriter);
                }
                if (request.DepositScript != null)
                {
                    jsonWriter.WritePropertyName("depositScript");
                    request.DepositScript.WriteJson(jsonWriter);
                }
                if (request.WithdrawScript != null)
                {
                    jsonWriter.WritePropertyName("withdrawScript");
                    request.WithdrawScript.WriteJson(jsonWriter);
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
                    "money",
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
                if (request.RequestId != null)
                {
                    jsonWriter.WritePropertyName("xGs2RequestId");
                    jsonWriter.Write(request.RequestId);
                }

                AddHeader(
                    Session.Credential,
                    "money",
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
                if (request.Priority != null)
                {
                    jsonWriter.WritePropertyName("priority");
                    jsonWriter.Write(request.Priority.ToString());
                }
                if (request.AppleKey != null)
                {
                    jsonWriter.WritePropertyName("appleKey");
                    jsonWriter.Write(request.AppleKey.ToString());
                }
                if (request.GoogleKey != null)
                {
                    jsonWriter.WritePropertyName("googleKey");
                    jsonWriter.Write(request.GoogleKey.ToString());
                }
                if (request.EnableFakeReceipt != null)
                {
                    jsonWriter.WritePropertyName("enableFakeReceipt");
                    jsonWriter.Write(request.EnableFakeReceipt.ToString());
                }
                if (request.CreateWalletScript != null)
                {
                    jsonWriter.WritePropertyName("createWalletScript");
                    request.CreateWalletScript.WriteJson(jsonWriter);
                }
                if (request.DepositScript != null)
                {
                    jsonWriter.WritePropertyName("depositScript");
                    request.DepositScript.WriteJson(jsonWriter);
                }
                if (request.WithdrawScript != null)
                {
                    jsonWriter.WritePropertyName("withdrawScript");
                    request.WithdrawScript.WriteJson(jsonWriter);
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
                    "money",
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
                if (request.RequestId != null)
                {
                    jsonWriter.WritePropertyName("xGs2RequestId");
                    jsonWriter.Write(request.RequestId);
                }

                AddHeader(
                    Session.Credential,
                    "money",
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


        public class GetWalletTask : Gs2WebSocketSessionTask<Request.GetWalletRequest, Result.GetWalletResult>
        {
	        public GetWalletTask(IGs2Session session, Request.GetWalletRequest request) : base(session, request)
	        {
	        }

            protected override IGs2SessionRequest CreateRequest(Request.GetWalletRequest request)
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
                if (request.Slot != null)
                {
                    jsonWriter.WritePropertyName("slot");
                    jsonWriter.Write(request.Slot.ToString());
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
                    "money",
                    "wallet",
                    "getWallet",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator GetWallet(
                Request.GetWalletRequest request,
                UnityAction<AsyncResult<Result.GetWalletResult>> callback
        )
		{
			var task = new GetWalletTask(
			    Gs2WebSocketSession,
			    request
            );
            yield return task;
            callback.Invoke(new AsyncResult<Result.GetWalletResult>(task.Result, task.Error));
        }

		public IFuture<Result.GetWalletResult> GetWalletFuture(
                Request.GetWalletRequest request
        )
		{
			return new GetWalletTask(
			    Gs2WebSocketSession,
			    request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.GetWalletResult> GetWalletAsync(
            Request.GetWalletRequest request
        )
		{
		    var task = new GetWalletTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
    #else
		public GetWalletTask GetWalletAsync(
                Request.GetWalletRequest request
        )
		{
			return new GetWalletTask(
                Gs2WebSocketSession,
			    request
            );
        }
    #endif
#else
		public async Task<Result.GetWalletResult> GetWalletAsync(
            Request.GetWalletRequest request
        )
		{
		    var task = new GetWalletTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
#endif


        public class GetWalletByUserIdTask : Gs2WebSocketSessionTask<Request.GetWalletByUserIdRequest, Result.GetWalletByUserIdResult>
        {
	        public GetWalletByUserIdTask(IGs2Session session, Request.GetWalletByUserIdRequest request) : base(session, request)
	        {
	        }

            protected override IGs2SessionRequest CreateRequest(Request.GetWalletByUserIdRequest request)
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
                if (request.Slot != null)
                {
                    jsonWriter.WritePropertyName("slot");
                    jsonWriter.Write(request.Slot.ToString());
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
                    "money",
                    "wallet",
                    "getWalletByUserId",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator GetWalletByUserId(
                Request.GetWalletByUserIdRequest request,
                UnityAction<AsyncResult<Result.GetWalletByUserIdResult>> callback
        )
		{
			var task = new GetWalletByUserIdTask(
			    Gs2WebSocketSession,
			    request
            );
            yield return task;
            callback.Invoke(new AsyncResult<Result.GetWalletByUserIdResult>(task.Result, task.Error));
        }

		public IFuture<Result.GetWalletByUserIdResult> GetWalletByUserIdFuture(
                Request.GetWalletByUserIdRequest request
        )
		{
			return new GetWalletByUserIdTask(
			    Gs2WebSocketSession,
			    request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.GetWalletByUserIdResult> GetWalletByUserIdAsync(
            Request.GetWalletByUserIdRequest request
        )
		{
		    var task = new GetWalletByUserIdTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
    #else
		public GetWalletByUserIdTask GetWalletByUserIdAsync(
                Request.GetWalletByUserIdRequest request
        )
		{
			return new GetWalletByUserIdTask(
                Gs2WebSocketSession,
			    request
            );
        }
    #endif
#else
		public async Task<Result.GetWalletByUserIdResult> GetWalletByUserIdAsync(
            Request.GetWalletByUserIdRequest request
        )
		{
		    var task = new GetWalletByUserIdTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
#endif


        public class DepositByUserIdTask : Gs2WebSocketSessionTask<Request.DepositByUserIdRequest, Result.DepositByUserIdResult>
        {
	        public DepositByUserIdTask(IGs2Session session, Request.DepositByUserIdRequest request) : base(session, request)
	        {
	        }

            protected override IGs2SessionRequest CreateRequest(Request.DepositByUserIdRequest request)
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
                if (request.Slot != null)
                {
                    jsonWriter.WritePropertyName("slot");
                    jsonWriter.Write(request.Slot.ToString());
                }
                if (request.Price != null)
                {
                    jsonWriter.WritePropertyName("price");
                    jsonWriter.Write(request.Price.ToString());
                }
                if (request.Count != null)
                {
                    jsonWriter.WritePropertyName("count");
                    jsonWriter.Write(request.Count.ToString());
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
                if (request.DuplicationAvoider != null)
                {
                    jsonWriter.WritePropertyName("xGs2DuplicationAvoider");
                    jsonWriter.Write(request.DuplicationAvoider);
                }

                AddHeader(
                    Session.Credential,
                    "money",
                    "wallet",
                    "depositByUserId",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }

            public override void OnError(Gs2.Core.Exception.Gs2Exception error)
            {
                if (error.Errors.Count(v => v.code == "wallet.operation.conflict") > 0) {
                    base.OnError(new Exception.ConflictException(error));
                }
                else {
                    base.OnError(error);
                }
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator DepositByUserId(
                Request.DepositByUserIdRequest request,
                UnityAction<AsyncResult<Result.DepositByUserIdResult>> callback
        )
		{
			var task = new DepositByUserIdTask(
			    Gs2WebSocketSession,
			    request
            );
            yield return task;
            callback.Invoke(new AsyncResult<Result.DepositByUserIdResult>(task.Result, task.Error));
        }

		public IFuture<Result.DepositByUserIdResult> DepositByUserIdFuture(
                Request.DepositByUserIdRequest request
        )
		{
			return new DepositByUserIdTask(
			    Gs2WebSocketSession,
			    request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.DepositByUserIdResult> DepositByUserIdAsync(
            Request.DepositByUserIdRequest request
        )
		{
		    var task = new DepositByUserIdTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
    #else
		public DepositByUserIdTask DepositByUserIdAsync(
                Request.DepositByUserIdRequest request
        )
		{
			return new DepositByUserIdTask(
                Gs2WebSocketSession,
			    request
            );
        }
    #endif
#else
		public async Task<Result.DepositByUserIdResult> DepositByUserIdAsync(
            Request.DepositByUserIdRequest request
        )
		{
		    var task = new DepositByUserIdTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
#endif


        public class WithdrawTask : Gs2WebSocketSessionTask<Request.WithdrawRequest, Result.WithdrawResult>
        {
	        public WithdrawTask(IGs2Session session, Request.WithdrawRequest request) : base(session, request)
	        {
	        }

            protected override IGs2SessionRequest CreateRequest(Request.WithdrawRequest request)
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
                if (request.Slot != null)
                {
                    jsonWriter.WritePropertyName("slot");
                    jsonWriter.Write(request.Slot.ToString());
                }
                if (request.Count != null)
                {
                    jsonWriter.WritePropertyName("count");
                    jsonWriter.Write(request.Count.ToString());
                }
                if (request.PaidOnly != null)
                {
                    jsonWriter.WritePropertyName("paidOnly");
                    jsonWriter.Write(request.PaidOnly.ToString());
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
                if (request.DuplicationAvoider != null)
                {
                    jsonWriter.WritePropertyName("xGs2DuplicationAvoider");
                    jsonWriter.Write(request.DuplicationAvoider);
                }

                AddHeader(
                    Session.Credential,
                    "money",
                    "wallet",
                    "withdraw",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }

            public override void OnError(Gs2.Core.Exception.Gs2Exception error)
            {
                if (error.Errors.Count(v => v.code == "wallet.operation.conflict") > 0) {
                    base.OnError(new Exception.ConflictException(error));
                }
                else if (error.Errors.Count(v => v.code == "wallet.balance.insufficient") > 0) {
                    base.OnError(new Exception.InsufficientException(error));
                }
                else {
                    base.OnError(error);
                }
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator Withdraw(
                Request.WithdrawRequest request,
                UnityAction<AsyncResult<Result.WithdrawResult>> callback
        )
		{
			var task = new WithdrawTask(
			    Gs2WebSocketSession,
			    request
            );
            yield return task;
            callback.Invoke(new AsyncResult<Result.WithdrawResult>(task.Result, task.Error));
        }

		public IFuture<Result.WithdrawResult> WithdrawFuture(
                Request.WithdrawRequest request
        )
		{
			return new WithdrawTask(
			    Gs2WebSocketSession,
			    request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.WithdrawResult> WithdrawAsync(
            Request.WithdrawRequest request
        )
		{
		    var task = new WithdrawTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
    #else
		public WithdrawTask WithdrawAsync(
                Request.WithdrawRequest request
        )
		{
			return new WithdrawTask(
                Gs2WebSocketSession,
			    request
            );
        }
    #endif
#else
		public async Task<Result.WithdrawResult> WithdrawAsync(
            Request.WithdrawRequest request
        )
		{
		    var task = new WithdrawTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
#endif


        public class WithdrawByUserIdTask : Gs2WebSocketSessionTask<Request.WithdrawByUserIdRequest, Result.WithdrawByUserIdResult>
        {
	        public WithdrawByUserIdTask(IGs2Session session, Request.WithdrawByUserIdRequest request) : base(session, request)
	        {
	        }

            protected override IGs2SessionRequest CreateRequest(Request.WithdrawByUserIdRequest request)
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
                if (request.Slot != null)
                {
                    jsonWriter.WritePropertyName("slot");
                    jsonWriter.Write(request.Slot.ToString());
                }
                if (request.Count != null)
                {
                    jsonWriter.WritePropertyName("count");
                    jsonWriter.Write(request.Count.ToString());
                }
                if (request.PaidOnly != null)
                {
                    jsonWriter.WritePropertyName("paidOnly");
                    jsonWriter.Write(request.PaidOnly.ToString());
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
                if (request.DuplicationAvoider != null)
                {
                    jsonWriter.WritePropertyName("xGs2DuplicationAvoider");
                    jsonWriter.Write(request.DuplicationAvoider);
                }

                AddHeader(
                    Session.Credential,
                    "money",
                    "wallet",
                    "withdrawByUserId",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }

            public override void OnError(Gs2.Core.Exception.Gs2Exception error)
            {
                if (error.Errors.Count(v => v.code == "wallet.operation.conflict") > 0) {
                    base.OnError(new Exception.ConflictException(error));
                }
                else if (error.Errors.Count(v => v.code == "wallet.balance.insufficient") > 0) {
                    base.OnError(new Exception.InsufficientException(error));
                }
                else {
                    base.OnError(error);
                }
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator WithdrawByUserId(
                Request.WithdrawByUserIdRequest request,
                UnityAction<AsyncResult<Result.WithdrawByUserIdResult>> callback
        )
		{
			var task = new WithdrawByUserIdTask(
			    Gs2WebSocketSession,
			    request
            );
            yield return task;
            callback.Invoke(new AsyncResult<Result.WithdrawByUserIdResult>(task.Result, task.Error));
        }

		public IFuture<Result.WithdrawByUserIdResult> WithdrawByUserIdFuture(
                Request.WithdrawByUserIdRequest request
        )
		{
			return new WithdrawByUserIdTask(
			    Gs2WebSocketSession,
			    request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.WithdrawByUserIdResult> WithdrawByUserIdAsync(
            Request.WithdrawByUserIdRequest request
        )
		{
		    var task = new WithdrawByUserIdTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
    #else
		public WithdrawByUserIdTask WithdrawByUserIdAsync(
                Request.WithdrawByUserIdRequest request
        )
		{
			return new WithdrawByUserIdTask(
                Gs2WebSocketSession,
			    request
            );
        }
    #endif
#else
		public async Task<Result.WithdrawByUserIdResult> WithdrawByUserIdAsync(
            Request.WithdrawByUserIdRequest request
        )
		{
		    var task = new WithdrawByUserIdTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
#endif


        public class DepositByStampSheetTask : Gs2WebSocketSessionTask<Request.DepositByStampSheetRequest, Result.DepositByStampSheetResult>
        {
	        public DepositByStampSheetTask(IGs2Session session, Request.DepositByStampSheetRequest request) : base(session, request)
	        {
	        }

            protected override IGs2SessionRequest CreateRequest(Request.DepositByStampSheetRequest request)
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
                    "money",
                    "wallet",
                    "depositByStampSheet",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator DepositByStampSheet(
                Request.DepositByStampSheetRequest request,
                UnityAction<AsyncResult<Result.DepositByStampSheetResult>> callback
        )
		{
			var task = new DepositByStampSheetTask(
			    Gs2WebSocketSession,
			    request
            );
            yield return task;
            callback.Invoke(new AsyncResult<Result.DepositByStampSheetResult>(task.Result, task.Error));
        }

		public IFuture<Result.DepositByStampSheetResult> DepositByStampSheetFuture(
                Request.DepositByStampSheetRequest request
        )
		{
			return new DepositByStampSheetTask(
			    Gs2WebSocketSession,
			    request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.DepositByStampSheetResult> DepositByStampSheetAsync(
            Request.DepositByStampSheetRequest request
        )
		{
		    var task = new DepositByStampSheetTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
    #else
		public DepositByStampSheetTask DepositByStampSheetAsync(
                Request.DepositByStampSheetRequest request
        )
		{
			return new DepositByStampSheetTask(
                Gs2WebSocketSession,
			    request
            );
        }
    #endif
#else
		public async Task<Result.DepositByStampSheetResult> DepositByStampSheetAsync(
            Request.DepositByStampSheetRequest request
        )
		{
		    var task = new DepositByStampSheetTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
#endif


        public class GetByUserIdAndTransactionIdTask : Gs2WebSocketSessionTask<Request.GetByUserIdAndTransactionIdRequest, Result.GetByUserIdAndTransactionIdResult>
        {
	        public GetByUserIdAndTransactionIdTask(IGs2Session session, Request.GetByUserIdAndTransactionIdRequest request) : base(session, request)
	        {
	        }

            protected override IGs2SessionRequest CreateRequest(Request.GetByUserIdAndTransactionIdRequest request)
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
                if (request.TransactionId != null)
                {
                    jsonWriter.WritePropertyName("transactionId");
                    jsonWriter.Write(request.TransactionId.ToString());
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
                    "money",
                    "receipt",
                    "getByUserIdAndTransactionId",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator GetByUserIdAndTransactionId(
                Request.GetByUserIdAndTransactionIdRequest request,
                UnityAction<AsyncResult<Result.GetByUserIdAndTransactionIdResult>> callback
        )
		{
			var task = new GetByUserIdAndTransactionIdTask(
			    Gs2WebSocketSession,
			    request
            );
            yield return task;
            callback.Invoke(new AsyncResult<Result.GetByUserIdAndTransactionIdResult>(task.Result, task.Error));
        }

		public IFuture<Result.GetByUserIdAndTransactionIdResult> GetByUserIdAndTransactionIdFuture(
                Request.GetByUserIdAndTransactionIdRequest request
        )
		{
			return new GetByUserIdAndTransactionIdTask(
			    Gs2WebSocketSession,
			    request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.GetByUserIdAndTransactionIdResult> GetByUserIdAndTransactionIdAsync(
            Request.GetByUserIdAndTransactionIdRequest request
        )
		{
		    var task = new GetByUserIdAndTransactionIdTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
    #else
		public GetByUserIdAndTransactionIdTask GetByUserIdAndTransactionIdAsync(
                Request.GetByUserIdAndTransactionIdRequest request
        )
		{
			return new GetByUserIdAndTransactionIdTask(
                Gs2WebSocketSession,
			    request
            );
        }
    #endif
#else
		public async Task<Result.GetByUserIdAndTransactionIdResult> GetByUserIdAndTransactionIdAsync(
            Request.GetByUserIdAndTransactionIdRequest request
        )
		{
		    var task = new GetByUserIdAndTransactionIdTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
#endif


        public class RecordReceiptTask : Gs2WebSocketSessionTask<Request.RecordReceiptRequest, Result.RecordReceiptResult>
        {
	        public RecordReceiptTask(IGs2Session session, Request.RecordReceiptRequest request) : base(session, request)
	        {
	        }

            protected override IGs2SessionRequest CreateRequest(Request.RecordReceiptRequest request)
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
                if (request.ContentsId != null)
                {
                    jsonWriter.WritePropertyName("contentsId");
                    jsonWriter.Write(request.ContentsId.ToString());
                }
                if (request.Receipt != null)
                {
                    jsonWriter.WritePropertyName("receipt");
                    jsonWriter.Write(request.Receipt.ToString());
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
                if (request.DuplicationAvoider != null)
                {
                    jsonWriter.WritePropertyName("xGs2DuplicationAvoider");
                    jsonWriter.Write(request.DuplicationAvoider);
                }

                AddHeader(
                    Session.Credential,
                    "money",
                    "receipt",
                    "recordReceipt",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }

            public override void OnError(Gs2.Core.Exception.Gs2Exception error)
            {
                if (error.Errors.Count(v => v.code == "receipt.payload.invalid") > 0) {
                    base.OnError(new Exception.ReceiptInvalidException(error));
                }
                else {
                    base.OnError(error);
                }
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator RecordReceipt(
                Request.RecordReceiptRequest request,
                UnityAction<AsyncResult<Result.RecordReceiptResult>> callback
        )
		{
			var task = new RecordReceiptTask(
			    Gs2WebSocketSession,
			    request
            );
            yield return task;
            callback.Invoke(new AsyncResult<Result.RecordReceiptResult>(task.Result, task.Error));
        }

		public IFuture<Result.RecordReceiptResult> RecordReceiptFuture(
                Request.RecordReceiptRequest request
        )
		{
			return new RecordReceiptTask(
			    Gs2WebSocketSession,
			    request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.RecordReceiptResult> RecordReceiptAsync(
            Request.RecordReceiptRequest request
        )
		{
		    var task = new RecordReceiptTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
    #else
		public RecordReceiptTask RecordReceiptAsync(
                Request.RecordReceiptRequest request
        )
		{
			return new RecordReceiptTask(
                Gs2WebSocketSession,
			    request
            );
        }
    #endif
#else
		public async Task<Result.RecordReceiptResult> RecordReceiptAsync(
            Request.RecordReceiptRequest request
        )
		{
		    var task = new RecordReceiptTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
#endif
	}
}