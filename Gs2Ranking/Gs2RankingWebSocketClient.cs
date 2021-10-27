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

namespace Gs2.Gs2Ranking
{
	public class Gs2RankingWebSocketClient : AbstractGs2Client
	{

		public static string Endpoint = "ranking";

        protected Gs2WebSocketSession Gs2WebSocketSession => (Gs2WebSocketSession) Gs2Session;

		public Gs2RankingWebSocketClient(Gs2WebSocketSession Gs2WebSocketSession) : base(Gs2WebSocketSession)
		{

		}


        private class GetCategoryModelTask : Gs2WebSocketSessionTask<Request.GetCategoryModelRequest, Result.GetCategoryModelResult>
        {
	        public GetCategoryModelTask(IGs2Session session, Request.GetCategoryModelRequest request) : base(session, request)
	        {
	        }

            protected override IGs2SessionRequest CreateRequest(Request.GetCategoryModelRequest request)
            {
                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);

                jsonWriter.WriteObjectStart();

                if (request.NamespaceName != null)
                {
                    jsonWriter.WritePropertyName("namespaceName");
                    jsonWriter.Write(request.NamespaceName.ToString());
                }
                if (request.CategoryName != null)
                {
                    jsonWriter.WritePropertyName("categoryName");
                    jsonWriter.Write(request.CategoryName.ToString());
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
                    "ranking",
                    "categoryModel",
                    "getCategoryModel",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator GetCategoryModel(
                Request.GetCategoryModelRequest request,
                UnityAction<AsyncResult<Result.GetCategoryModelResult>> callback
        )
		{
			var task = new GetCategoryModelTask(
			    Gs2WebSocketSession,
			    request
            );
            yield return task;
            callback.Invoke(new AsyncResult<Result.GetCategoryModelResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.GetCategoryModelResult> GetCategoryModel(
            Request.GetCategoryModelRequest request
        )
		{
		    var task = new GetCategoryModelTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
#endif


        private class CreateCategoryModelMasterTask : Gs2WebSocketSessionTask<Request.CreateCategoryModelMasterRequest, Result.CreateCategoryModelMasterResult>
        {
	        public CreateCategoryModelMasterTask(IGs2Session session, Request.CreateCategoryModelMasterRequest request) : base(session, request)
	        {
	        }

            protected override IGs2SessionRequest CreateRequest(Request.CreateCategoryModelMasterRequest request)
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
                if (request.MinimumValue != null)
                {
                    jsonWriter.WritePropertyName("minimumValue");
                    jsonWriter.Write(request.MinimumValue.ToString());
                }
                if (request.MaximumValue != null)
                {
                    jsonWriter.WritePropertyName("maximumValue");
                    jsonWriter.Write(request.MaximumValue.ToString());
                }
                if (request.OrderDirection != null)
                {
                    jsonWriter.WritePropertyName("orderDirection");
                    jsonWriter.Write(request.OrderDirection.ToString());
                }
                if (request.Scope != null)
                {
                    jsonWriter.WritePropertyName("scope");
                    jsonWriter.Write(request.Scope.ToString());
                }
                if (request.UniqueByUserId != null)
                {
                    jsonWriter.WritePropertyName("uniqueByUserId");
                    jsonWriter.Write(request.UniqueByUserId.ToString());
                }
                if (request.CalculateFixedTimingHour != null)
                {
                    jsonWriter.WritePropertyName("calculateFixedTimingHour");
                    jsonWriter.Write(request.CalculateFixedTimingHour.ToString());
                }
                if (request.CalculateFixedTimingMinute != null)
                {
                    jsonWriter.WritePropertyName("calculateFixedTimingMinute");
                    jsonWriter.Write(request.CalculateFixedTimingMinute.ToString());
                }
                if (request.CalculateIntervalMinutes != null)
                {
                    jsonWriter.WritePropertyName("calculateIntervalMinutes");
                    jsonWriter.Write(request.CalculateIntervalMinutes.ToString());
                }
                if (request.EntryPeriodEventId != null)
                {
                    jsonWriter.WritePropertyName("entryPeriodEventId");
                    jsonWriter.Write(request.EntryPeriodEventId.ToString());
                }
                if (request.AccessPeriodEventId != null)
                {
                    jsonWriter.WritePropertyName("accessPeriodEventId");
                    jsonWriter.Write(request.AccessPeriodEventId.ToString());
                }
                if (request.Generation != null)
                {
                    jsonWriter.WritePropertyName("generation");
                    jsonWriter.Write(request.Generation.ToString());
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
                    "ranking",
                    "categoryModelMaster",
                    "createCategoryModelMaster",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator CreateCategoryModelMaster(
                Request.CreateCategoryModelMasterRequest request,
                UnityAction<AsyncResult<Result.CreateCategoryModelMasterResult>> callback
        )
		{
			var task = new CreateCategoryModelMasterTask(
			    Gs2WebSocketSession,
			    request
            );
            yield return task;
            callback.Invoke(new AsyncResult<Result.CreateCategoryModelMasterResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.CreateCategoryModelMasterResult> CreateCategoryModelMaster(
            Request.CreateCategoryModelMasterRequest request
        )
		{
		    var task = new CreateCategoryModelMasterTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
#endif


        private class GetCategoryModelMasterTask : Gs2WebSocketSessionTask<Request.GetCategoryModelMasterRequest, Result.GetCategoryModelMasterResult>
        {
	        public GetCategoryModelMasterTask(IGs2Session session, Request.GetCategoryModelMasterRequest request) : base(session, request)
	        {
	        }

            protected override IGs2SessionRequest CreateRequest(Request.GetCategoryModelMasterRequest request)
            {
                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);

                jsonWriter.WriteObjectStart();

                if (request.NamespaceName != null)
                {
                    jsonWriter.WritePropertyName("namespaceName");
                    jsonWriter.Write(request.NamespaceName.ToString());
                }
                if (request.CategoryName != null)
                {
                    jsonWriter.WritePropertyName("categoryName");
                    jsonWriter.Write(request.CategoryName.ToString());
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
                    "ranking",
                    "categoryModelMaster",
                    "getCategoryModelMaster",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator GetCategoryModelMaster(
                Request.GetCategoryModelMasterRequest request,
                UnityAction<AsyncResult<Result.GetCategoryModelMasterResult>> callback
        )
		{
			var task = new GetCategoryModelMasterTask(
			    Gs2WebSocketSession,
			    request
            );
            yield return task;
            callback.Invoke(new AsyncResult<Result.GetCategoryModelMasterResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.GetCategoryModelMasterResult> GetCategoryModelMaster(
            Request.GetCategoryModelMasterRequest request
        )
		{
		    var task = new GetCategoryModelMasterTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
#endif


        private class UpdateCategoryModelMasterTask : Gs2WebSocketSessionTask<Request.UpdateCategoryModelMasterRequest, Result.UpdateCategoryModelMasterResult>
        {
	        public UpdateCategoryModelMasterTask(IGs2Session session, Request.UpdateCategoryModelMasterRequest request) : base(session, request)
	        {
	        }

            protected override IGs2SessionRequest CreateRequest(Request.UpdateCategoryModelMasterRequest request)
            {
                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);

                jsonWriter.WriteObjectStart();

                if (request.NamespaceName != null)
                {
                    jsonWriter.WritePropertyName("namespaceName");
                    jsonWriter.Write(request.NamespaceName.ToString());
                }
                if (request.CategoryName != null)
                {
                    jsonWriter.WritePropertyName("categoryName");
                    jsonWriter.Write(request.CategoryName.ToString());
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
                if (request.MinimumValue != null)
                {
                    jsonWriter.WritePropertyName("minimumValue");
                    jsonWriter.Write(request.MinimumValue.ToString());
                }
                if (request.MaximumValue != null)
                {
                    jsonWriter.WritePropertyName("maximumValue");
                    jsonWriter.Write(request.MaximumValue.ToString());
                }
                if (request.OrderDirection != null)
                {
                    jsonWriter.WritePropertyName("orderDirection");
                    jsonWriter.Write(request.OrderDirection.ToString());
                }
                if (request.Scope != null)
                {
                    jsonWriter.WritePropertyName("scope");
                    jsonWriter.Write(request.Scope.ToString());
                }
                if (request.UniqueByUserId != null)
                {
                    jsonWriter.WritePropertyName("uniqueByUserId");
                    jsonWriter.Write(request.UniqueByUserId.ToString());
                }
                if (request.CalculateFixedTimingHour != null)
                {
                    jsonWriter.WritePropertyName("calculateFixedTimingHour");
                    jsonWriter.Write(request.CalculateFixedTimingHour.ToString());
                }
                if (request.CalculateFixedTimingMinute != null)
                {
                    jsonWriter.WritePropertyName("calculateFixedTimingMinute");
                    jsonWriter.Write(request.CalculateFixedTimingMinute.ToString());
                }
                if (request.CalculateIntervalMinutes != null)
                {
                    jsonWriter.WritePropertyName("calculateIntervalMinutes");
                    jsonWriter.Write(request.CalculateIntervalMinutes.ToString());
                }
                if (request.EntryPeriodEventId != null)
                {
                    jsonWriter.WritePropertyName("entryPeriodEventId");
                    jsonWriter.Write(request.EntryPeriodEventId.ToString());
                }
                if (request.AccessPeriodEventId != null)
                {
                    jsonWriter.WritePropertyName("accessPeriodEventId");
                    jsonWriter.Write(request.AccessPeriodEventId.ToString());
                }
                if (request.Generation != null)
                {
                    jsonWriter.WritePropertyName("generation");
                    jsonWriter.Write(request.Generation.ToString());
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
                    "ranking",
                    "categoryModelMaster",
                    "updateCategoryModelMaster",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator UpdateCategoryModelMaster(
                Request.UpdateCategoryModelMasterRequest request,
                UnityAction<AsyncResult<Result.UpdateCategoryModelMasterResult>> callback
        )
		{
			var task = new UpdateCategoryModelMasterTask(
			    Gs2WebSocketSession,
			    request
            );
            yield return task;
            callback.Invoke(new AsyncResult<Result.UpdateCategoryModelMasterResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.UpdateCategoryModelMasterResult> UpdateCategoryModelMaster(
            Request.UpdateCategoryModelMasterRequest request
        )
		{
		    var task = new UpdateCategoryModelMasterTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
#endif


        private class DeleteCategoryModelMasterTask : Gs2WebSocketSessionTask<Request.DeleteCategoryModelMasterRequest, Result.DeleteCategoryModelMasterResult>
        {
	        public DeleteCategoryModelMasterTask(IGs2Session session, Request.DeleteCategoryModelMasterRequest request) : base(session, request)
	        {
	        }

            protected override IGs2SessionRequest CreateRequest(Request.DeleteCategoryModelMasterRequest request)
            {
                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);

                jsonWriter.WriteObjectStart();

                if (request.NamespaceName != null)
                {
                    jsonWriter.WritePropertyName("namespaceName");
                    jsonWriter.Write(request.NamespaceName.ToString());
                }
                if (request.CategoryName != null)
                {
                    jsonWriter.WritePropertyName("categoryName");
                    jsonWriter.Write(request.CategoryName.ToString());
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
                    "ranking",
                    "categoryModelMaster",
                    "deleteCategoryModelMaster",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator DeleteCategoryModelMaster(
                Request.DeleteCategoryModelMasterRequest request,
                UnityAction<AsyncResult<Result.DeleteCategoryModelMasterResult>> callback
        )
		{
			var task = new DeleteCategoryModelMasterTask(
			    Gs2WebSocketSession,
			    request
            );
            yield return task;
            callback.Invoke(new AsyncResult<Result.DeleteCategoryModelMasterResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.DeleteCategoryModelMasterResult> DeleteCategoryModelMaster(
            Request.DeleteCategoryModelMasterRequest request
        )
		{
		    var task = new DeleteCategoryModelMasterTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
#endif


        private class SubscribeTask : Gs2WebSocketSessionTask<Request.SubscribeRequest, Result.SubscribeResult>
        {
	        public SubscribeTask(IGs2Session session, Request.SubscribeRequest request) : base(session, request)
	        {
	        }

            protected override IGs2SessionRequest CreateRequest(Request.SubscribeRequest request)
            {
                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);

                jsonWriter.WriteObjectStart();

                if (request.NamespaceName != null)
                {
                    jsonWriter.WritePropertyName("namespaceName");
                    jsonWriter.Write(request.NamespaceName.ToString());
                }
                if (request.CategoryName != null)
                {
                    jsonWriter.WritePropertyName("categoryName");
                    jsonWriter.Write(request.CategoryName.ToString());
                }
                if (request.AccessToken != null)
                {
                    jsonWriter.WritePropertyName("accessToken");
                    jsonWriter.Write(request.AccessToken.ToString());
                }
                if (request.TargetUserId != null)
                {
                    jsonWriter.WritePropertyName("targetUserId");
                    jsonWriter.Write(request.TargetUserId.ToString());
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
                    "ranking",
                    "subscribe",
                    "subscribe",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator Subscribe(
                Request.SubscribeRequest request,
                UnityAction<AsyncResult<Result.SubscribeResult>> callback
        )
		{
			var task = new SubscribeTask(
			    Gs2WebSocketSession,
			    request
            );
            yield return task;
            callback.Invoke(new AsyncResult<Result.SubscribeResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.SubscribeResult> Subscribe(
            Request.SubscribeRequest request
        )
		{
		    var task = new SubscribeTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
#endif


        private class SubscribeByUserIdTask : Gs2WebSocketSessionTask<Request.SubscribeByUserIdRequest, Result.SubscribeByUserIdResult>
        {
	        public SubscribeByUserIdTask(IGs2Session session, Request.SubscribeByUserIdRequest request) : base(session, request)
	        {
	        }

            protected override IGs2SessionRequest CreateRequest(Request.SubscribeByUserIdRequest request)
            {
                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);

                jsonWriter.WriteObjectStart();

                if (request.NamespaceName != null)
                {
                    jsonWriter.WritePropertyName("namespaceName");
                    jsonWriter.Write(request.NamespaceName.ToString());
                }
                if (request.CategoryName != null)
                {
                    jsonWriter.WritePropertyName("categoryName");
                    jsonWriter.Write(request.CategoryName.ToString());
                }
                if (request.UserId != null)
                {
                    jsonWriter.WritePropertyName("userId");
                    jsonWriter.Write(request.UserId.ToString());
                }
                if (request.TargetUserId != null)
                {
                    jsonWriter.WritePropertyName("targetUserId");
                    jsonWriter.Write(request.TargetUserId.ToString());
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
                    "ranking",
                    "subscribe",
                    "subscribeByUserId",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator SubscribeByUserId(
                Request.SubscribeByUserIdRequest request,
                UnityAction<AsyncResult<Result.SubscribeByUserIdResult>> callback
        )
		{
			var task = new SubscribeByUserIdTask(
			    Gs2WebSocketSession,
			    request
            );
            yield return task;
            callback.Invoke(new AsyncResult<Result.SubscribeByUserIdResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.SubscribeByUserIdResult> SubscribeByUserId(
            Request.SubscribeByUserIdRequest request
        )
		{
		    var task = new SubscribeByUserIdTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
#endif


        private class GetSubscribeTask : Gs2WebSocketSessionTask<Request.GetSubscribeRequest, Result.GetSubscribeResult>
        {
	        public GetSubscribeTask(IGs2Session session, Request.GetSubscribeRequest request) : base(session, request)
	        {
	        }

            protected override IGs2SessionRequest CreateRequest(Request.GetSubscribeRequest request)
            {
                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);

                jsonWriter.WriteObjectStart();

                if (request.NamespaceName != null)
                {
                    jsonWriter.WritePropertyName("namespaceName");
                    jsonWriter.Write(request.NamespaceName.ToString());
                }
                if (request.CategoryName != null)
                {
                    jsonWriter.WritePropertyName("categoryName");
                    jsonWriter.Write(request.CategoryName.ToString());
                }
                if (request.AccessToken != null)
                {
                    jsonWriter.WritePropertyName("accessToken");
                    jsonWriter.Write(request.AccessToken.ToString());
                }
                if (request.TargetUserId != null)
                {
                    jsonWriter.WritePropertyName("targetUserId");
                    jsonWriter.Write(request.TargetUserId.ToString());
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
                    "ranking",
                    "subscribe",
                    "getSubscribe",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator GetSubscribe(
                Request.GetSubscribeRequest request,
                UnityAction<AsyncResult<Result.GetSubscribeResult>> callback
        )
		{
			var task = new GetSubscribeTask(
			    Gs2WebSocketSession,
			    request
            );
            yield return task;
            callback.Invoke(new AsyncResult<Result.GetSubscribeResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.GetSubscribeResult> GetSubscribe(
            Request.GetSubscribeRequest request
        )
		{
		    var task = new GetSubscribeTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
#endif


        private class GetSubscribeByUserIdTask : Gs2WebSocketSessionTask<Request.GetSubscribeByUserIdRequest, Result.GetSubscribeByUserIdResult>
        {
	        public GetSubscribeByUserIdTask(IGs2Session session, Request.GetSubscribeByUserIdRequest request) : base(session, request)
	        {
	        }

            protected override IGs2SessionRequest CreateRequest(Request.GetSubscribeByUserIdRequest request)
            {
                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);

                jsonWriter.WriteObjectStart();

                if (request.NamespaceName != null)
                {
                    jsonWriter.WritePropertyName("namespaceName");
                    jsonWriter.Write(request.NamespaceName.ToString());
                }
                if (request.CategoryName != null)
                {
                    jsonWriter.WritePropertyName("categoryName");
                    jsonWriter.Write(request.CategoryName.ToString());
                }
                if (request.UserId != null)
                {
                    jsonWriter.WritePropertyName("userId");
                    jsonWriter.Write(request.UserId.ToString());
                }
                if (request.TargetUserId != null)
                {
                    jsonWriter.WritePropertyName("targetUserId");
                    jsonWriter.Write(request.TargetUserId.ToString());
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
                    "ranking",
                    "subscribe",
                    "getSubscribeByUserId",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator GetSubscribeByUserId(
                Request.GetSubscribeByUserIdRequest request,
                UnityAction<AsyncResult<Result.GetSubscribeByUserIdResult>> callback
        )
		{
			var task = new GetSubscribeByUserIdTask(
			    Gs2WebSocketSession,
			    request
            );
            yield return task;
            callback.Invoke(new AsyncResult<Result.GetSubscribeByUserIdResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.GetSubscribeByUserIdResult> GetSubscribeByUserId(
            Request.GetSubscribeByUserIdRequest request
        )
		{
		    var task = new GetSubscribeByUserIdTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
#endif


        private class UnsubscribeTask : Gs2WebSocketSessionTask<Request.UnsubscribeRequest, Result.UnsubscribeResult>
        {
	        public UnsubscribeTask(IGs2Session session, Request.UnsubscribeRequest request) : base(session, request)
	        {
	        }

            protected override IGs2SessionRequest CreateRequest(Request.UnsubscribeRequest request)
            {
                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);

                jsonWriter.WriteObjectStart();

                if (request.NamespaceName != null)
                {
                    jsonWriter.WritePropertyName("namespaceName");
                    jsonWriter.Write(request.NamespaceName.ToString());
                }
                if (request.CategoryName != null)
                {
                    jsonWriter.WritePropertyName("categoryName");
                    jsonWriter.Write(request.CategoryName.ToString());
                }
                if (request.AccessToken != null)
                {
                    jsonWriter.WritePropertyName("accessToken");
                    jsonWriter.Write(request.AccessToken.ToString());
                }
                if (request.TargetUserId != null)
                {
                    jsonWriter.WritePropertyName("targetUserId");
                    jsonWriter.Write(request.TargetUserId.ToString());
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
                    "ranking",
                    "subscribe",
                    "unsubscribe",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator Unsubscribe(
                Request.UnsubscribeRequest request,
                UnityAction<AsyncResult<Result.UnsubscribeResult>> callback
        )
		{
			var task = new UnsubscribeTask(
			    Gs2WebSocketSession,
			    request
            );
            yield return task;
            callback.Invoke(new AsyncResult<Result.UnsubscribeResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.UnsubscribeResult> Unsubscribe(
            Request.UnsubscribeRequest request
        )
		{
		    var task = new UnsubscribeTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
#endif


        private class UnsubscribeByUserIdTask : Gs2WebSocketSessionTask<Request.UnsubscribeByUserIdRequest, Result.UnsubscribeByUserIdResult>
        {
	        public UnsubscribeByUserIdTask(IGs2Session session, Request.UnsubscribeByUserIdRequest request) : base(session, request)
	        {
	        }

            protected override IGs2SessionRequest CreateRequest(Request.UnsubscribeByUserIdRequest request)
            {
                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);

                jsonWriter.WriteObjectStart();

                if (request.NamespaceName != null)
                {
                    jsonWriter.WritePropertyName("namespaceName");
                    jsonWriter.Write(request.NamespaceName.ToString());
                }
                if (request.CategoryName != null)
                {
                    jsonWriter.WritePropertyName("categoryName");
                    jsonWriter.Write(request.CategoryName.ToString());
                }
                if (request.UserId != null)
                {
                    jsonWriter.WritePropertyName("userId");
                    jsonWriter.Write(request.UserId.ToString());
                }
                if (request.TargetUserId != null)
                {
                    jsonWriter.WritePropertyName("targetUserId");
                    jsonWriter.Write(request.TargetUserId.ToString());
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
                    "ranking",
                    "subscribe",
                    "unsubscribeByUserId",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator UnsubscribeByUserId(
                Request.UnsubscribeByUserIdRequest request,
                UnityAction<AsyncResult<Result.UnsubscribeByUserIdResult>> callback
        )
		{
			var task = new UnsubscribeByUserIdTask(
			    Gs2WebSocketSession,
			    request
            );
            yield return task;
            callback.Invoke(new AsyncResult<Result.UnsubscribeByUserIdResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.UnsubscribeByUserIdResult> UnsubscribeByUserId(
            Request.UnsubscribeByUserIdRequest request
        )
		{
		    var task = new UnsubscribeByUserIdTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
#endif


        private class GetScoreTask : Gs2WebSocketSessionTask<Request.GetScoreRequest, Result.GetScoreResult>
        {
	        public GetScoreTask(IGs2Session session, Request.GetScoreRequest request) : base(session, request)
	        {
	        }

            protected override IGs2SessionRequest CreateRequest(Request.GetScoreRequest request)
            {
                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);

                jsonWriter.WriteObjectStart();

                if (request.NamespaceName != null)
                {
                    jsonWriter.WritePropertyName("namespaceName");
                    jsonWriter.Write(request.NamespaceName.ToString());
                }
                if (request.CategoryName != null)
                {
                    jsonWriter.WritePropertyName("categoryName");
                    jsonWriter.Write(request.CategoryName.ToString());
                }
                if (request.AccessToken != null)
                {
                    jsonWriter.WritePropertyName("accessToken");
                    jsonWriter.Write(request.AccessToken.ToString());
                }
                if (request.ScorerUserId != null)
                {
                    jsonWriter.WritePropertyName("scorerUserId");
                    jsonWriter.Write(request.ScorerUserId.ToString());
                }
                if (request.UniqueId != null)
                {
                    jsonWriter.WritePropertyName("uniqueId");
                    jsonWriter.Write(request.UniqueId.ToString());
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
                    "ranking",
                    "score",
                    "getScore",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator GetScore(
                Request.GetScoreRequest request,
                UnityAction<AsyncResult<Result.GetScoreResult>> callback
        )
		{
			var task = new GetScoreTask(
			    Gs2WebSocketSession,
			    request
            );
            yield return task;
            callback.Invoke(new AsyncResult<Result.GetScoreResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.GetScoreResult> GetScore(
            Request.GetScoreRequest request
        )
		{
		    var task = new GetScoreTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
#endif


        private class GetScoreByUserIdTask : Gs2WebSocketSessionTask<Request.GetScoreByUserIdRequest, Result.GetScoreByUserIdResult>
        {
	        public GetScoreByUserIdTask(IGs2Session session, Request.GetScoreByUserIdRequest request) : base(session, request)
	        {
	        }

            protected override IGs2SessionRequest CreateRequest(Request.GetScoreByUserIdRequest request)
            {
                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);

                jsonWriter.WriteObjectStart();

                if (request.NamespaceName != null)
                {
                    jsonWriter.WritePropertyName("namespaceName");
                    jsonWriter.Write(request.NamespaceName.ToString());
                }
                if (request.CategoryName != null)
                {
                    jsonWriter.WritePropertyName("categoryName");
                    jsonWriter.Write(request.CategoryName.ToString());
                }
                if (request.UserId != null)
                {
                    jsonWriter.WritePropertyName("userId");
                    jsonWriter.Write(request.UserId.ToString());
                }
                if (request.ScorerUserId != null)
                {
                    jsonWriter.WritePropertyName("scorerUserId");
                    jsonWriter.Write(request.ScorerUserId.ToString());
                }
                if (request.UniqueId != null)
                {
                    jsonWriter.WritePropertyName("uniqueId");
                    jsonWriter.Write(request.UniqueId.ToString());
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
                    "ranking",
                    "score",
                    "getScoreByUserId",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator GetScoreByUserId(
                Request.GetScoreByUserIdRequest request,
                UnityAction<AsyncResult<Result.GetScoreByUserIdResult>> callback
        )
		{
			var task = new GetScoreByUserIdTask(
			    Gs2WebSocketSession,
			    request
            );
            yield return task;
            callback.Invoke(new AsyncResult<Result.GetScoreByUserIdResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.GetScoreByUserIdResult> GetScoreByUserId(
            Request.GetScoreByUserIdRequest request
        )
		{
		    var task = new GetScoreByUserIdTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
#endif


        private class GetRankingTask : Gs2WebSocketSessionTask<Request.GetRankingRequest, Result.GetRankingResult>
        {
	        public GetRankingTask(IGs2Session session, Request.GetRankingRequest request) : base(session, request)
	        {
	        }

            protected override IGs2SessionRequest CreateRequest(Request.GetRankingRequest request)
            {
                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);

                jsonWriter.WriteObjectStart();

                if (request.NamespaceName != null)
                {
                    jsonWriter.WritePropertyName("namespaceName");
                    jsonWriter.Write(request.NamespaceName.ToString());
                }
                if (request.CategoryName != null)
                {
                    jsonWriter.WritePropertyName("categoryName");
                    jsonWriter.Write(request.CategoryName.ToString());
                }
                if (request.AccessToken != null)
                {
                    jsonWriter.WritePropertyName("accessToken");
                    jsonWriter.Write(request.AccessToken.ToString());
                }
                if (request.ScorerUserId != null)
                {
                    jsonWriter.WritePropertyName("scorerUserId");
                    jsonWriter.Write(request.ScorerUserId.ToString());
                }
                if (request.UniqueId != null)
                {
                    jsonWriter.WritePropertyName("uniqueId");
                    jsonWriter.Write(request.UniqueId.ToString());
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
                    "ranking",
                    "ranking",
                    "getRanking",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator GetRanking(
                Request.GetRankingRequest request,
                UnityAction<AsyncResult<Result.GetRankingResult>> callback
        )
		{
			var task = new GetRankingTask(
			    Gs2WebSocketSession,
			    request
            );
            yield return task;
            callback.Invoke(new AsyncResult<Result.GetRankingResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.GetRankingResult> GetRanking(
            Request.GetRankingRequest request
        )
		{
		    var task = new GetRankingTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
#endif


        private class GetRankingByUserIdTask : Gs2WebSocketSessionTask<Request.GetRankingByUserIdRequest, Result.GetRankingByUserIdResult>
        {
	        public GetRankingByUserIdTask(IGs2Session session, Request.GetRankingByUserIdRequest request) : base(session, request)
	        {
	        }

            protected override IGs2SessionRequest CreateRequest(Request.GetRankingByUserIdRequest request)
            {
                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);

                jsonWriter.WriteObjectStart();

                if (request.NamespaceName != null)
                {
                    jsonWriter.WritePropertyName("namespaceName");
                    jsonWriter.Write(request.NamespaceName.ToString());
                }
                if (request.CategoryName != null)
                {
                    jsonWriter.WritePropertyName("categoryName");
                    jsonWriter.Write(request.CategoryName.ToString());
                }
                if (request.UserId != null)
                {
                    jsonWriter.WritePropertyName("userId");
                    jsonWriter.Write(request.UserId.ToString());
                }
                if (request.ScorerUserId != null)
                {
                    jsonWriter.WritePropertyName("scorerUserId");
                    jsonWriter.Write(request.ScorerUserId.ToString());
                }
                if (request.UniqueId != null)
                {
                    jsonWriter.WritePropertyName("uniqueId");
                    jsonWriter.Write(request.UniqueId.ToString());
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
                    "ranking",
                    "ranking",
                    "getRankingByUserId",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator GetRankingByUserId(
                Request.GetRankingByUserIdRequest request,
                UnityAction<AsyncResult<Result.GetRankingByUserIdResult>> callback
        )
		{
			var task = new GetRankingByUserIdTask(
			    Gs2WebSocketSession,
			    request
            );
            yield return task;
            callback.Invoke(new AsyncResult<Result.GetRankingByUserIdResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.GetRankingByUserIdResult> GetRankingByUserId(
            Request.GetRankingByUserIdRequest request
        )
		{
		    var task = new GetRankingByUserIdTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
#endif


        private class PutScoreTask : Gs2WebSocketSessionTask<Request.PutScoreRequest, Result.PutScoreResult>
        {
	        public PutScoreTask(IGs2Session session, Request.PutScoreRequest request) : base(session, request)
	        {
	        }

            protected override IGs2SessionRequest CreateRequest(Request.PutScoreRequest request)
            {
                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);

                jsonWriter.WriteObjectStart();

                if (request.NamespaceName != null)
                {
                    jsonWriter.WritePropertyName("namespaceName");
                    jsonWriter.Write(request.NamespaceName.ToString());
                }
                if (request.CategoryName != null)
                {
                    jsonWriter.WritePropertyName("categoryName");
                    jsonWriter.Write(request.CategoryName.ToString());
                }
                if (request.AccessToken != null)
                {
                    jsonWriter.WritePropertyName("accessToken");
                    jsonWriter.Write(request.AccessToken.ToString());
                }
                if (request.Score != null)
                {
                    jsonWriter.WritePropertyName("score");
                    jsonWriter.Write(request.Score.ToString());
                }
                if (request.Metadata != null)
                {
                    jsonWriter.WritePropertyName("metadata");
                    jsonWriter.Write(request.Metadata.ToString());
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
                    "ranking",
                    "ranking",
                    "putScore",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator PutScore(
                Request.PutScoreRequest request,
                UnityAction<AsyncResult<Result.PutScoreResult>> callback
        )
		{
			var task = new PutScoreTask(
			    Gs2WebSocketSession,
			    request
            );
            yield return task;
            callback.Invoke(new AsyncResult<Result.PutScoreResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.PutScoreResult> PutScore(
            Request.PutScoreRequest request
        )
		{
		    var task = new PutScoreTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
#endif


        private class PutScoreByUserIdTask : Gs2WebSocketSessionTask<Request.PutScoreByUserIdRequest, Result.PutScoreByUserIdResult>
        {
	        public PutScoreByUserIdTask(IGs2Session session, Request.PutScoreByUserIdRequest request) : base(session, request)
	        {
	        }

            protected override IGs2SessionRequest CreateRequest(Request.PutScoreByUserIdRequest request)
            {
                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);

                jsonWriter.WriteObjectStart();

                if (request.NamespaceName != null)
                {
                    jsonWriter.WritePropertyName("namespaceName");
                    jsonWriter.Write(request.NamespaceName.ToString());
                }
                if (request.CategoryName != null)
                {
                    jsonWriter.WritePropertyName("categoryName");
                    jsonWriter.Write(request.CategoryName.ToString());
                }
                if (request.UserId != null)
                {
                    jsonWriter.WritePropertyName("userId");
                    jsonWriter.Write(request.UserId.ToString());
                }
                if (request.Score != null)
                {
                    jsonWriter.WritePropertyName("score");
                    jsonWriter.Write(request.Score.ToString());
                }
                if (request.Metadata != null)
                {
                    jsonWriter.WritePropertyName("metadata");
                    jsonWriter.Write(request.Metadata.ToString());
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
                    "ranking",
                    "ranking",
                    "putScoreByUserId",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator PutScoreByUserId(
                Request.PutScoreByUserIdRequest request,
                UnityAction<AsyncResult<Result.PutScoreByUserIdResult>> callback
        )
		{
			var task = new PutScoreByUserIdTask(
			    Gs2WebSocketSession,
			    request
            );
            yield return task;
            callback.Invoke(new AsyncResult<Result.PutScoreByUserIdResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.PutScoreByUserIdResult> PutScoreByUserId(
            Request.PutScoreByUserIdRequest request
        )
		{
		    var task = new PutScoreByUserIdTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
#endif


        private class CalcRankingTask : Gs2WebSocketSessionTask<Request.CalcRankingRequest, Result.CalcRankingResult>
        {
	        public CalcRankingTask(IGs2Session session, Request.CalcRankingRequest request) : base(session, request)
	        {
	        }

            protected override IGs2SessionRequest CreateRequest(Request.CalcRankingRequest request)
            {
                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);

                jsonWriter.WriteObjectStart();

                if (request.NamespaceName != null)
                {
                    jsonWriter.WritePropertyName("namespaceName");
                    jsonWriter.Write(request.NamespaceName.ToString());
                }
                if (request.CategoryName != null)
                {
                    jsonWriter.WritePropertyName("categoryName");
                    jsonWriter.Write(request.CategoryName.ToString());
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
                    "ranking",
                    "ranking",
                    "calcRanking",
                    jsonWriter
                );

                jsonWriter.WriteObjectEnd();

                return WebSocketSessionRequestFactory.New<WebSocketSessionRequest>(stringBuilder.ToString());
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator CalcRanking(
                Request.CalcRankingRequest request,
                UnityAction<AsyncResult<Result.CalcRankingResult>> callback
        )
		{
			var task = new CalcRankingTask(
			    Gs2WebSocketSession,
			    request
            );
            yield return task;
            callback.Invoke(new AsyncResult<Result.CalcRankingResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.CalcRankingResult> CalcRanking(
            Request.CalcRankingRequest request
        )
		{
		    var task = new CalcRankingTask(
		        Gs2WebSocketSession,
		        request
            );
			return await task.Invoke();
        }
#endif
	}
}