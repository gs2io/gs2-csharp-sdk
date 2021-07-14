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
using Gs2.Util.LitJson;namespace Gs2.Gs2Ranking
{
	public class Gs2RankingWebSocketClient : AbstractGs2Client
	{

		public static string Endpoint = "ranking";

        protected Gs2WebSocketSession Gs2WebSocketSession => (Gs2WebSocketSession) Gs2Session;

		public Gs2RankingWebSocketClient(Gs2WebSocketSession Gs2WebSocketSession) : base(Gs2WebSocketSession)
		{

		}

        private class GetCategoryModelTask : Gs2WebSocketSessionTask<Result.GetCategoryModelResult>
        {
			private readonly Request.GetCategoryModelRequest _request;

			public GetCategoryModelTask(Request.GetCategoryModelRequest request, UnityAction<AsyncResult<Result.GetCategoryModelResult>> userCallback) : base(userCallback)
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
                if (_request.CategoryName != null)
                {
                    jsonWriter.WritePropertyName("categoryName");
                    jsonWriter.Write(_request.CategoryName.ToString());
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
                jsonWriter.Write("ranking");
                jsonWriter.WritePropertyName("component");
                jsonWriter.Write("categoryModel");
                jsonWriter.WritePropertyName("function");
                jsonWriter.Write("getCategoryModel");
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

		public IEnumerator GetCategoryModel(
                Request.GetCategoryModelRequest request,
                UnityAction<AsyncResult<Result.GetCategoryModelResult>> callback
        )
		{
			var task = new GetCategoryModelTask(request, callback);
			return Gs2WebSocketSession.Execute(task);
        }

        private class CreateCategoryModelMasterTask : Gs2WebSocketSessionTask<Result.CreateCategoryModelMasterResult>
        {
			private readonly Request.CreateCategoryModelMasterRequest _request;

			public CreateCategoryModelMasterTask(Request.CreateCategoryModelMasterRequest request, UnityAction<AsyncResult<Result.CreateCategoryModelMasterResult>> userCallback) : base(userCallback)
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
                if (_request.MinimumValue != null)
                {
                    jsonWriter.WritePropertyName("minimumValue");
                    jsonWriter.Write(_request.MinimumValue.ToString());
                }
                if (_request.MaximumValue != null)
                {
                    jsonWriter.WritePropertyName("maximumValue");
                    jsonWriter.Write(_request.MaximumValue.ToString());
                }
                if (_request.OrderDirection != null)
                {
                    jsonWriter.WritePropertyName("orderDirection");
                    jsonWriter.Write(_request.OrderDirection.ToString());
                }
                if (_request.Scope != null)
                {
                    jsonWriter.WritePropertyName("scope");
                    jsonWriter.Write(_request.Scope.ToString());
                }
                if (_request.UniqueByUserId != null)
                {
                    jsonWriter.WritePropertyName("uniqueByUserId");
                    jsonWriter.Write(_request.UniqueByUserId.ToString());
                }
                if (_request.CalculateFixedTimingHour != null)
                {
                    jsonWriter.WritePropertyName("calculateFixedTimingHour");
                    jsonWriter.Write(_request.CalculateFixedTimingHour.ToString());
                }
                if (_request.CalculateFixedTimingMinute != null)
                {
                    jsonWriter.WritePropertyName("calculateFixedTimingMinute");
                    jsonWriter.Write(_request.CalculateFixedTimingMinute.ToString());
                }
                if (_request.CalculateIntervalMinutes != null)
                {
                    jsonWriter.WritePropertyName("calculateIntervalMinutes");
                    jsonWriter.Write(_request.CalculateIntervalMinutes.ToString());
                }
                if (_request.EntryPeriodEventId != null)
                {
                    jsonWriter.WritePropertyName("entryPeriodEventId");
                    jsonWriter.Write(_request.EntryPeriodEventId.ToString());
                }
                if (_request.AccessPeriodEventId != null)
                {
                    jsonWriter.WritePropertyName("accessPeriodEventId");
                    jsonWriter.Write(_request.AccessPeriodEventId.ToString());
                }
                if (_request.Generation != null)
                {
                    jsonWriter.WritePropertyName("generation");
                    jsonWriter.Write(_request.Generation.ToString());
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
                jsonWriter.Write("ranking");
                jsonWriter.WritePropertyName("component");
                jsonWriter.Write("categoryModelMaster");
                jsonWriter.WritePropertyName("function");
                jsonWriter.Write("createCategoryModelMaster");
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

		public IEnumerator CreateCategoryModelMaster(
                Request.CreateCategoryModelMasterRequest request,
                UnityAction<AsyncResult<Result.CreateCategoryModelMasterResult>> callback
        )
		{
			var task = new CreateCategoryModelMasterTask(request, callback);
			return Gs2WebSocketSession.Execute(task);
        }

        private class GetCategoryModelMasterTask : Gs2WebSocketSessionTask<Result.GetCategoryModelMasterResult>
        {
			private readonly Request.GetCategoryModelMasterRequest _request;

			public GetCategoryModelMasterTask(Request.GetCategoryModelMasterRequest request, UnityAction<AsyncResult<Result.GetCategoryModelMasterResult>> userCallback) : base(userCallback)
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
                if (_request.CategoryName != null)
                {
                    jsonWriter.WritePropertyName("categoryName");
                    jsonWriter.Write(_request.CategoryName.ToString());
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
                jsonWriter.Write("ranking");
                jsonWriter.WritePropertyName("component");
                jsonWriter.Write("categoryModelMaster");
                jsonWriter.WritePropertyName("function");
                jsonWriter.Write("getCategoryModelMaster");
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

		public IEnumerator GetCategoryModelMaster(
                Request.GetCategoryModelMasterRequest request,
                UnityAction<AsyncResult<Result.GetCategoryModelMasterResult>> callback
        )
		{
			var task = new GetCategoryModelMasterTask(request, callback);
			return Gs2WebSocketSession.Execute(task);
        }

        private class UpdateCategoryModelMasterTask : Gs2WebSocketSessionTask<Result.UpdateCategoryModelMasterResult>
        {
			private readonly Request.UpdateCategoryModelMasterRequest _request;

			public UpdateCategoryModelMasterTask(Request.UpdateCategoryModelMasterRequest request, UnityAction<AsyncResult<Result.UpdateCategoryModelMasterResult>> userCallback) : base(userCallback)
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
                if (_request.CategoryName != null)
                {
                    jsonWriter.WritePropertyName("categoryName");
                    jsonWriter.Write(_request.CategoryName.ToString());
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
                if (_request.MinimumValue != null)
                {
                    jsonWriter.WritePropertyName("minimumValue");
                    jsonWriter.Write(_request.MinimumValue.ToString());
                }
                if (_request.MaximumValue != null)
                {
                    jsonWriter.WritePropertyName("maximumValue");
                    jsonWriter.Write(_request.MaximumValue.ToString());
                }
                if (_request.OrderDirection != null)
                {
                    jsonWriter.WritePropertyName("orderDirection");
                    jsonWriter.Write(_request.OrderDirection.ToString());
                }
                if (_request.Scope != null)
                {
                    jsonWriter.WritePropertyName("scope");
                    jsonWriter.Write(_request.Scope.ToString());
                }
                if (_request.UniqueByUserId != null)
                {
                    jsonWriter.WritePropertyName("uniqueByUserId");
                    jsonWriter.Write(_request.UniqueByUserId.ToString());
                }
                if (_request.CalculateFixedTimingHour != null)
                {
                    jsonWriter.WritePropertyName("calculateFixedTimingHour");
                    jsonWriter.Write(_request.CalculateFixedTimingHour.ToString());
                }
                if (_request.CalculateFixedTimingMinute != null)
                {
                    jsonWriter.WritePropertyName("calculateFixedTimingMinute");
                    jsonWriter.Write(_request.CalculateFixedTimingMinute.ToString());
                }
                if (_request.CalculateIntervalMinutes != null)
                {
                    jsonWriter.WritePropertyName("calculateIntervalMinutes");
                    jsonWriter.Write(_request.CalculateIntervalMinutes.ToString());
                }
                if (_request.EntryPeriodEventId != null)
                {
                    jsonWriter.WritePropertyName("entryPeriodEventId");
                    jsonWriter.Write(_request.EntryPeriodEventId.ToString());
                }
                if (_request.AccessPeriodEventId != null)
                {
                    jsonWriter.WritePropertyName("accessPeriodEventId");
                    jsonWriter.Write(_request.AccessPeriodEventId.ToString());
                }
                if (_request.Generation != null)
                {
                    jsonWriter.WritePropertyName("generation");
                    jsonWriter.Write(_request.Generation.ToString());
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
                jsonWriter.Write("ranking");
                jsonWriter.WritePropertyName("component");
                jsonWriter.Write("categoryModelMaster");
                jsonWriter.WritePropertyName("function");
                jsonWriter.Write("updateCategoryModelMaster");
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

		public IEnumerator UpdateCategoryModelMaster(
                Request.UpdateCategoryModelMasterRequest request,
                UnityAction<AsyncResult<Result.UpdateCategoryModelMasterResult>> callback
        )
		{
			var task = new UpdateCategoryModelMasterTask(request, callback);
			return Gs2WebSocketSession.Execute(task);
        }

        private class DeleteCategoryModelMasterTask : Gs2WebSocketSessionTask<Result.DeleteCategoryModelMasterResult>
        {
			private readonly Request.DeleteCategoryModelMasterRequest _request;

			public DeleteCategoryModelMasterTask(Request.DeleteCategoryModelMasterRequest request, UnityAction<AsyncResult<Result.DeleteCategoryModelMasterResult>> userCallback) : base(userCallback)
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
                if (_request.CategoryName != null)
                {
                    jsonWriter.WritePropertyName("categoryName");
                    jsonWriter.Write(_request.CategoryName.ToString());
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
                jsonWriter.Write("ranking");
                jsonWriter.WritePropertyName("component");
                jsonWriter.Write("categoryModelMaster");
                jsonWriter.WritePropertyName("function");
                jsonWriter.Write("deleteCategoryModelMaster");
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

		public IEnumerator DeleteCategoryModelMaster(
                Request.DeleteCategoryModelMasterRequest request,
                UnityAction<AsyncResult<Result.DeleteCategoryModelMasterResult>> callback
        )
		{
			var task = new DeleteCategoryModelMasterTask(request, callback);
			return Gs2WebSocketSession.Execute(task);
        }

        private class SubscribeTask : Gs2WebSocketSessionTask<Result.SubscribeResult>
        {
			private readonly Request.SubscribeRequest _request;

			public SubscribeTask(Request.SubscribeRequest request, UnityAction<AsyncResult<Result.SubscribeResult>> userCallback) : base(userCallback)
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
                if (_request.CategoryName != null)
                {
                    jsonWriter.WritePropertyName("categoryName");
                    jsonWriter.Write(_request.CategoryName.ToString());
                }
                if (_request.AccessToken != null)
                {
                    jsonWriter.WritePropertyName("accessToken");
                    jsonWriter.Write(_request.AccessToken.ToString());
                }
                if (_request.TargetUserId != null)
                {
                    jsonWriter.WritePropertyName("targetUserId");
                    jsonWriter.Write(_request.TargetUserId.ToString());
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
                jsonWriter.Write("ranking");
                jsonWriter.WritePropertyName("component");
                jsonWriter.Write("subscribe");
                jsonWriter.WritePropertyName("function");
                jsonWriter.Write("subscribe");
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

		public IEnumerator Subscribe(
                Request.SubscribeRequest request,
                UnityAction<AsyncResult<Result.SubscribeResult>> callback
        )
		{
			var task = new SubscribeTask(request, callback);
			return Gs2WebSocketSession.Execute(task);
        }

        private class SubscribeByUserIdTask : Gs2WebSocketSessionTask<Result.SubscribeByUserIdResult>
        {
			private readonly Request.SubscribeByUserIdRequest _request;

			public SubscribeByUserIdTask(Request.SubscribeByUserIdRequest request, UnityAction<AsyncResult<Result.SubscribeByUserIdResult>> userCallback) : base(userCallback)
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
                if (_request.CategoryName != null)
                {
                    jsonWriter.WritePropertyName("categoryName");
                    jsonWriter.Write(_request.CategoryName.ToString());
                }
                if (_request.UserId != null)
                {
                    jsonWriter.WritePropertyName("userId");
                    jsonWriter.Write(_request.UserId.ToString());
                }
                if (_request.TargetUserId != null)
                {
                    jsonWriter.WritePropertyName("targetUserId");
                    jsonWriter.Write(_request.TargetUserId.ToString());
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
                jsonWriter.Write("ranking");
                jsonWriter.WritePropertyName("component");
                jsonWriter.Write("subscribe");
                jsonWriter.WritePropertyName("function");
                jsonWriter.Write("subscribeByUserId");
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

		public IEnumerator SubscribeByUserId(
                Request.SubscribeByUserIdRequest request,
                UnityAction<AsyncResult<Result.SubscribeByUserIdResult>> callback
        )
		{
			var task = new SubscribeByUserIdTask(request, callback);
			return Gs2WebSocketSession.Execute(task);
        }

        private class GetSubscribeTask : Gs2WebSocketSessionTask<Result.GetSubscribeResult>
        {
			private readonly Request.GetSubscribeRequest _request;

			public GetSubscribeTask(Request.GetSubscribeRequest request, UnityAction<AsyncResult<Result.GetSubscribeResult>> userCallback) : base(userCallback)
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
                if (_request.CategoryName != null)
                {
                    jsonWriter.WritePropertyName("categoryName");
                    jsonWriter.Write(_request.CategoryName.ToString());
                }
                if (_request.AccessToken != null)
                {
                    jsonWriter.WritePropertyName("accessToken");
                    jsonWriter.Write(_request.AccessToken.ToString());
                }
                if (_request.TargetUserId != null)
                {
                    jsonWriter.WritePropertyName("targetUserId");
                    jsonWriter.Write(_request.TargetUserId.ToString());
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
                jsonWriter.Write("ranking");
                jsonWriter.WritePropertyName("component");
                jsonWriter.Write("subscribe");
                jsonWriter.WritePropertyName("function");
                jsonWriter.Write("getSubscribe");
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

		public IEnumerator GetSubscribe(
                Request.GetSubscribeRequest request,
                UnityAction<AsyncResult<Result.GetSubscribeResult>> callback
        )
		{
			var task = new GetSubscribeTask(request, callback);
			return Gs2WebSocketSession.Execute(task);
        }

        private class GetSubscribeByUserIdTask : Gs2WebSocketSessionTask<Result.GetSubscribeByUserIdResult>
        {
			private readonly Request.GetSubscribeByUserIdRequest _request;

			public GetSubscribeByUserIdTask(Request.GetSubscribeByUserIdRequest request, UnityAction<AsyncResult<Result.GetSubscribeByUserIdResult>> userCallback) : base(userCallback)
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
                if (_request.CategoryName != null)
                {
                    jsonWriter.WritePropertyName("categoryName");
                    jsonWriter.Write(_request.CategoryName.ToString());
                }
                if (_request.UserId != null)
                {
                    jsonWriter.WritePropertyName("userId");
                    jsonWriter.Write(_request.UserId.ToString());
                }
                if (_request.TargetUserId != null)
                {
                    jsonWriter.WritePropertyName("targetUserId");
                    jsonWriter.Write(_request.TargetUserId.ToString());
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
                jsonWriter.Write("ranking");
                jsonWriter.WritePropertyName("component");
                jsonWriter.Write("subscribe");
                jsonWriter.WritePropertyName("function");
                jsonWriter.Write("getSubscribeByUserId");
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

		public IEnumerator GetSubscribeByUserId(
                Request.GetSubscribeByUserIdRequest request,
                UnityAction<AsyncResult<Result.GetSubscribeByUserIdResult>> callback
        )
		{
			var task = new GetSubscribeByUserIdTask(request, callback);
			return Gs2WebSocketSession.Execute(task);
        }

        private class UnsubscribeTask : Gs2WebSocketSessionTask<Result.UnsubscribeResult>
        {
			private readonly Request.UnsubscribeRequest _request;

			public UnsubscribeTask(Request.UnsubscribeRequest request, UnityAction<AsyncResult<Result.UnsubscribeResult>> userCallback) : base(userCallback)
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
                if (_request.CategoryName != null)
                {
                    jsonWriter.WritePropertyName("categoryName");
                    jsonWriter.Write(_request.CategoryName.ToString());
                }
                if (_request.AccessToken != null)
                {
                    jsonWriter.WritePropertyName("accessToken");
                    jsonWriter.Write(_request.AccessToken.ToString());
                }
                if (_request.TargetUserId != null)
                {
                    jsonWriter.WritePropertyName("targetUserId");
                    jsonWriter.Write(_request.TargetUserId.ToString());
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
                jsonWriter.Write("ranking");
                jsonWriter.WritePropertyName("component");
                jsonWriter.Write("subscribe");
                jsonWriter.WritePropertyName("function");
                jsonWriter.Write("unsubscribe");
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

		public IEnumerator Unsubscribe(
                Request.UnsubscribeRequest request,
                UnityAction<AsyncResult<Result.UnsubscribeResult>> callback
        )
		{
			var task = new UnsubscribeTask(request, callback);
			return Gs2WebSocketSession.Execute(task);
        }

        private class UnsubscribeByUserIdTask : Gs2WebSocketSessionTask<Result.UnsubscribeByUserIdResult>
        {
			private readonly Request.UnsubscribeByUserIdRequest _request;

			public UnsubscribeByUserIdTask(Request.UnsubscribeByUserIdRequest request, UnityAction<AsyncResult<Result.UnsubscribeByUserIdResult>> userCallback) : base(userCallback)
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
                if (_request.CategoryName != null)
                {
                    jsonWriter.WritePropertyName("categoryName");
                    jsonWriter.Write(_request.CategoryName.ToString());
                }
                if (_request.UserId != null)
                {
                    jsonWriter.WritePropertyName("userId");
                    jsonWriter.Write(_request.UserId.ToString());
                }
                if (_request.TargetUserId != null)
                {
                    jsonWriter.WritePropertyName("targetUserId");
                    jsonWriter.Write(_request.TargetUserId.ToString());
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
                jsonWriter.Write("ranking");
                jsonWriter.WritePropertyName("component");
                jsonWriter.Write("subscribe");
                jsonWriter.WritePropertyName("function");
                jsonWriter.Write("unsubscribeByUserId");
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

		public IEnumerator UnsubscribeByUserId(
                Request.UnsubscribeByUserIdRequest request,
                UnityAction<AsyncResult<Result.UnsubscribeByUserIdResult>> callback
        )
		{
			var task = new UnsubscribeByUserIdTask(request, callback);
			return Gs2WebSocketSession.Execute(task);
        }

        private class GetScoreTask : Gs2WebSocketSessionTask<Result.GetScoreResult>
        {
			private readonly Request.GetScoreRequest _request;

			public GetScoreTask(Request.GetScoreRequest request, UnityAction<AsyncResult<Result.GetScoreResult>> userCallback) : base(userCallback)
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
                if (_request.CategoryName != null)
                {
                    jsonWriter.WritePropertyName("categoryName");
                    jsonWriter.Write(_request.CategoryName.ToString());
                }
                if (_request.AccessToken != null)
                {
                    jsonWriter.WritePropertyName("accessToken");
                    jsonWriter.Write(_request.AccessToken.ToString());
                }
                if (_request.ScorerUserId != null)
                {
                    jsonWriter.WritePropertyName("scorerUserId");
                    jsonWriter.Write(_request.ScorerUserId.ToString());
                }
                if (_request.UniqueId != null)
                {
                    jsonWriter.WritePropertyName("uniqueId");
                    jsonWriter.Write(_request.UniqueId.ToString());
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
                jsonWriter.Write("ranking");
                jsonWriter.WritePropertyName("component");
                jsonWriter.Write("score");
                jsonWriter.WritePropertyName("function");
                jsonWriter.Write("getScore");
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

		public IEnumerator GetScore(
                Request.GetScoreRequest request,
                UnityAction<AsyncResult<Result.GetScoreResult>> callback
        )
		{
			var task = new GetScoreTask(request, callback);
			return Gs2WebSocketSession.Execute(task);
        }

        private class GetScoreByUserIdTask : Gs2WebSocketSessionTask<Result.GetScoreByUserIdResult>
        {
			private readonly Request.GetScoreByUserIdRequest _request;

			public GetScoreByUserIdTask(Request.GetScoreByUserIdRequest request, UnityAction<AsyncResult<Result.GetScoreByUserIdResult>> userCallback) : base(userCallback)
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
                if (_request.CategoryName != null)
                {
                    jsonWriter.WritePropertyName("categoryName");
                    jsonWriter.Write(_request.CategoryName.ToString());
                }
                if (_request.UserId != null)
                {
                    jsonWriter.WritePropertyName("userId");
                    jsonWriter.Write(_request.UserId.ToString());
                }
                if (_request.ScorerUserId != null)
                {
                    jsonWriter.WritePropertyName("scorerUserId");
                    jsonWriter.Write(_request.ScorerUserId.ToString());
                }
                if (_request.UniqueId != null)
                {
                    jsonWriter.WritePropertyName("uniqueId");
                    jsonWriter.Write(_request.UniqueId.ToString());
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
                jsonWriter.Write("ranking");
                jsonWriter.WritePropertyName("component");
                jsonWriter.Write("score");
                jsonWriter.WritePropertyName("function");
                jsonWriter.Write("getScoreByUserId");
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

		public IEnumerator GetScoreByUserId(
                Request.GetScoreByUserIdRequest request,
                UnityAction<AsyncResult<Result.GetScoreByUserIdResult>> callback
        )
		{
			var task = new GetScoreByUserIdTask(request, callback);
			return Gs2WebSocketSession.Execute(task);
        }

        private class GetRankingTask : Gs2WebSocketSessionTask<Result.GetRankingResult>
        {
			private readonly Request.GetRankingRequest _request;

			public GetRankingTask(Request.GetRankingRequest request, UnityAction<AsyncResult<Result.GetRankingResult>> userCallback) : base(userCallback)
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
                if (_request.CategoryName != null)
                {
                    jsonWriter.WritePropertyName("categoryName");
                    jsonWriter.Write(_request.CategoryName.ToString());
                }
                if (_request.AccessToken != null)
                {
                    jsonWriter.WritePropertyName("accessToken");
                    jsonWriter.Write(_request.AccessToken.ToString());
                }
                if (_request.ScorerUserId != null)
                {
                    jsonWriter.WritePropertyName("scorerUserId");
                    jsonWriter.Write(_request.ScorerUserId.ToString());
                }
                if (_request.UniqueId != null)
                {
                    jsonWriter.WritePropertyName("uniqueId");
                    jsonWriter.Write(_request.UniqueId.ToString());
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
                jsonWriter.Write("ranking");
                jsonWriter.WritePropertyName("component");
                jsonWriter.Write("ranking");
                jsonWriter.WritePropertyName("function");
                jsonWriter.Write("getRanking");
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

		public IEnumerator GetRanking(
                Request.GetRankingRequest request,
                UnityAction<AsyncResult<Result.GetRankingResult>> callback
        )
		{
			var task = new GetRankingTask(request, callback);
			return Gs2WebSocketSession.Execute(task);
        }

        private class GetRankingByUserIdTask : Gs2WebSocketSessionTask<Result.GetRankingByUserIdResult>
        {
			private readonly Request.GetRankingByUserIdRequest _request;

			public GetRankingByUserIdTask(Request.GetRankingByUserIdRequest request, UnityAction<AsyncResult<Result.GetRankingByUserIdResult>> userCallback) : base(userCallback)
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
                if (_request.CategoryName != null)
                {
                    jsonWriter.WritePropertyName("categoryName");
                    jsonWriter.Write(_request.CategoryName.ToString());
                }
                if (_request.UserId != null)
                {
                    jsonWriter.WritePropertyName("userId");
                    jsonWriter.Write(_request.UserId.ToString());
                }
                if (_request.ScorerUserId != null)
                {
                    jsonWriter.WritePropertyName("scorerUserId");
                    jsonWriter.Write(_request.ScorerUserId.ToString());
                }
                if (_request.UniqueId != null)
                {
                    jsonWriter.WritePropertyName("uniqueId");
                    jsonWriter.Write(_request.UniqueId.ToString());
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
                jsonWriter.Write("ranking");
                jsonWriter.WritePropertyName("component");
                jsonWriter.Write("ranking");
                jsonWriter.WritePropertyName("function");
                jsonWriter.Write("getRankingByUserId");
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

		public IEnumerator GetRankingByUserId(
                Request.GetRankingByUserIdRequest request,
                UnityAction<AsyncResult<Result.GetRankingByUserIdResult>> callback
        )
		{
			var task = new GetRankingByUserIdTask(request, callback);
			return Gs2WebSocketSession.Execute(task);
        }

        private class PutScoreTask : Gs2WebSocketSessionTask<Result.PutScoreResult>
        {
			private readonly Request.PutScoreRequest _request;

			public PutScoreTask(Request.PutScoreRequest request, UnityAction<AsyncResult<Result.PutScoreResult>> userCallback) : base(userCallback)
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
                if (_request.CategoryName != null)
                {
                    jsonWriter.WritePropertyName("categoryName");
                    jsonWriter.Write(_request.CategoryName.ToString());
                }
                if (_request.AccessToken != null)
                {
                    jsonWriter.WritePropertyName("accessToken");
                    jsonWriter.Write(_request.AccessToken.ToString());
                }
                if (_request.Score != null)
                {
                    jsonWriter.WritePropertyName("score");
                    jsonWriter.Write(_request.Score.ToString());
                }
                if (_request.Metadata != null)
                {
                    jsonWriter.WritePropertyName("metadata");
                    jsonWriter.Write(_request.Metadata.ToString());
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
                jsonWriter.Write("ranking");
                jsonWriter.WritePropertyName("component");
                jsonWriter.Write("ranking");
                jsonWriter.WritePropertyName("function");
                jsonWriter.Write("putScore");
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

		public IEnumerator PutScore(
                Request.PutScoreRequest request,
                UnityAction<AsyncResult<Result.PutScoreResult>> callback
        )
		{
			var task = new PutScoreTask(request, callback);
			return Gs2WebSocketSession.Execute(task);
        }

        private class PutScoreByUserIdTask : Gs2WebSocketSessionTask<Result.PutScoreByUserIdResult>
        {
			private readonly Request.PutScoreByUserIdRequest _request;

			public PutScoreByUserIdTask(Request.PutScoreByUserIdRequest request, UnityAction<AsyncResult<Result.PutScoreByUserIdResult>> userCallback) : base(userCallback)
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
                if (_request.CategoryName != null)
                {
                    jsonWriter.WritePropertyName("categoryName");
                    jsonWriter.Write(_request.CategoryName.ToString());
                }
                if (_request.UserId != null)
                {
                    jsonWriter.WritePropertyName("userId");
                    jsonWriter.Write(_request.UserId.ToString());
                }
                if (_request.Score != null)
                {
                    jsonWriter.WritePropertyName("score");
                    jsonWriter.Write(_request.Score.ToString());
                }
                if (_request.Metadata != null)
                {
                    jsonWriter.WritePropertyName("metadata");
                    jsonWriter.Write(_request.Metadata.ToString());
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
                jsonWriter.Write("ranking");
                jsonWriter.WritePropertyName("component");
                jsonWriter.Write("ranking");
                jsonWriter.WritePropertyName("function");
                jsonWriter.Write("putScoreByUserId");
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

		public IEnumerator PutScoreByUserId(
                Request.PutScoreByUserIdRequest request,
                UnityAction<AsyncResult<Result.PutScoreByUserIdResult>> callback
        )
		{
			var task = new PutScoreByUserIdTask(request, callback);
			return Gs2WebSocketSession.Execute(task);
        }
	}
}