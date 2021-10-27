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
#if UNITY_2017_1_OR_NEWER
using UnityEngine.Events;
using UnityEngine.Networking;
#else
using System.Web;
using System.Net.Http;
using System.Threading.Tasks;
#endif

using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Gs2.Core;
using Gs2.Core.Model;
using Gs2.Core.Net;
using Gs2.Gs2Ranking.Request;
using Gs2.Gs2Ranking.Result;
using Gs2.Util.LitJson;

namespace Gs2.Gs2Ranking
{
	public class Gs2RankingRestClient : AbstractGs2Client
	{
#if UNITY_2017_1_OR_NEWER
		private readonly CertificateHandler _certificateHandler;
#endif

		public static string Endpoint = "ranking";

        protected Gs2RestSession Gs2RestSession => (Gs2RestSession) Gs2Session;

		public Gs2RankingRestClient(Gs2RestSession Gs2RestSession) : base(Gs2RestSession)
		{

		}

#if UNITY_2017_1_OR_NEWER
		public Gs2RankingRestClient(Gs2RestSession gs2RestSession, CertificateHandler certificateHandler) : base(gs2RestSession)
		{
			_certificateHandler = certificateHandler;
		}
#endif


        private class DescribeNamespacesTask : Gs2RestSessionTask<DescribeNamespacesRequest, DescribeNamespacesResult>
        {
            public DescribeNamespacesTask(IGs2Session session, RestSessionRequestFactory factory, DescribeNamespacesRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DescribeNamespacesRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "ranking")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/";

                var sessionRequest = Factory.Get(url);
                if (request.ContextStack != null)
                {
                    sessionRequest.AddQueryString("contextStack", request.ContextStack);
                }
                if (request.PageToken != null) {
                    sessionRequest.AddQueryString("pageToken", $"{request.PageToken}");
                }
                if (request.Limit != null) {
                    sessionRequest.AddQueryString("limit", $"{request.Limit}");
                }

                if (request.RequestId != null)
                {
                    sessionRequest.AddHeader("X-GS2-REQUEST-ID", request.RequestId);
                }

                AddHeader(
                    Session.Credential,
                    sessionRequest
                );

                return sessionRequest;
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator DescribeNamespaces(
                Request.DescribeNamespacesRequest request,
                UnityAction<AsyncResult<Result.DescribeNamespacesResult>> callback
        )
		{
			var task = new DescribeNamespacesTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.DescribeNamespacesResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.DescribeNamespacesResult> DescribeNamespacesAsync(
                Request.DescribeNamespacesRequest request
        )
		{
			var task = new DescribeNamespacesTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        private class CreateNamespaceTask : Gs2RestSessionTask<CreateNamespaceRequest, CreateNamespaceResult>
        {
            public CreateNamespaceTask(IGs2Session session, RestSessionRequestFactory factory, CreateNamespaceRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(CreateNamespaceRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "ranking")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/";

                var sessionRequest = Factory.Post(url);

                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);
                jsonWriter.WriteObjectStart();
                if (request.Name != null)
                {
                    jsonWriter.WritePropertyName("name");
                    jsonWriter.Write(request.Name);
                }
                if (request.Description != null)
                {
                    jsonWriter.WritePropertyName("description");
                    jsonWriter.Write(request.Description);
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
                jsonWriter.WriteObjectEnd();

                var body = stringBuilder.ToString();
                if (!string.IsNullOrEmpty(body))
                {
                    sessionRequest.Body = body;
                }
                sessionRequest.AddHeader("Content-Type", "application/json");

                if (request.RequestId != null)
                {
                    sessionRequest.AddHeader("X-GS2-REQUEST-ID", request.RequestId);
                }

                AddHeader(
                    Session.Credential,
                    sessionRequest
                );

                return sessionRequest;
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator CreateNamespace(
                Request.CreateNamespaceRequest request,
                UnityAction<AsyncResult<Result.CreateNamespaceResult>> callback
        )
		{
			var task = new CreateNamespaceTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.CreateNamespaceResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.CreateNamespaceResult> CreateNamespaceAsync(
                Request.CreateNamespaceRequest request
        )
		{
			var task = new CreateNamespaceTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        private class GetNamespaceStatusTask : Gs2RestSessionTask<GetNamespaceStatusRequest, GetNamespaceStatusResult>
        {
            public GetNamespaceStatusTask(IGs2Session session, RestSessionRequestFactory factory, GetNamespaceStatusRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(GetNamespaceStatusRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "ranking")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/status";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");

                var sessionRequest = Factory.Get(url);
                if (request.ContextStack != null)
                {
                    sessionRequest.AddQueryString("contextStack", request.ContextStack);
                }

                if (request.RequestId != null)
                {
                    sessionRequest.AddHeader("X-GS2-REQUEST-ID", request.RequestId);
                }

                AddHeader(
                    Session.Credential,
                    sessionRequest
                );

                return sessionRequest;
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator GetNamespaceStatus(
                Request.GetNamespaceStatusRequest request,
                UnityAction<AsyncResult<Result.GetNamespaceStatusResult>> callback
        )
		{
			var task = new GetNamespaceStatusTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.GetNamespaceStatusResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.GetNamespaceStatusResult> GetNamespaceStatusAsync(
                Request.GetNamespaceStatusRequest request
        )
		{
			var task = new GetNamespaceStatusTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        private class GetNamespaceTask : Gs2RestSessionTask<GetNamespaceRequest, GetNamespaceResult>
        {
            public GetNamespaceTask(IGs2Session session, RestSessionRequestFactory factory, GetNamespaceRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(GetNamespaceRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "ranking")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");

                var sessionRequest = Factory.Get(url);
                if (request.ContextStack != null)
                {
                    sessionRequest.AddQueryString("contextStack", request.ContextStack);
                }

                if (request.RequestId != null)
                {
                    sessionRequest.AddHeader("X-GS2-REQUEST-ID", request.RequestId);
                }

                AddHeader(
                    Session.Credential,
                    sessionRequest
                );

                return sessionRequest;
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator GetNamespace(
                Request.GetNamespaceRequest request,
                UnityAction<AsyncResult<Result.GetNamespaceResult>> callback
        )
		{
			var task = new GetNamespaceTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.GetNamespaceResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.GetNamespaceResult> GetNamespaceAsync(
                Request.GetNamespaceRequest request
        )
		{
			var task = new GetNamespaceTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        private class UpdateNamespaceTask : Gs2RestSessionTask<UpdateNamespaceRequest, UpdateNamespaceResult>
        {
            public UpdateNamespaceTask(IGs2Session session, RestSessionRequestFactory factory, UpdateNamespaceRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(UpdateNamespaceRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "ranking")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");

                var sessionRequest = Factory.Put(url);

                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);
                jsonWriter.WriteObjectStart();
                if (request.Description != null)
                {
                    jsonWriter.WritePropertyName("description");
                    jsonWriter.Write(request.Description);
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
                jsonWriter.WriteObjectEnd();

                var body = stringBuilder.ToString();
                if (!string.IsNullOrEmpty(body))
                {
                    sessionRequest.Body = body;
                }
                sessionRequest.AddHeader("Content-Type", "application/json");

                if (request.RequestId != null)
                {
                    sessionRequest.AddHeader("X-GS2-REQUEST-ID", request.RequestId);
                }

                AddHeader(
                    Session.Credential,
                    sessionRequest
                );

                return sessionRequest;
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator UpdateNamespace(
                Request.UpdateNamespaceRequest request,
                UnityAction<AsyncResult<Result.UpdateNamespaceResult>> callback
        )
		{
			var task = new UpdateNamespaceTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.UpdateNamespaceResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.UpdateNamespaceResult> UpdateNamespaceAsync(
                Request.UpdateNamespaceRequest request
        )
		{
			var task = new UpdateNamespaceTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        private class DeleteNamespaceTask : Gs2RestSessionTask<DeleteNamespaceRequest, DeleteNamespaceResult>
        {
            public DeleteNamespaceTask(IGs2Session session, RestSessionRequestFactory factory, DeleteNamespaceRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DeleteNamespaceRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "ranking")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");

                var sessionRequest = Factory.Delete(url);
                if (request.ContextStack != null)
                {
                    sessionRequest.AddQueryString("contextStack", request.ContextStack);
                }

                if (request.RequestId != null)
                {
                    sessionRequest.AddHeader("X-GS2-REQUEST-ID", request.RequestId);
                }

                AddHeader(
                    Session.Credential,
                    sessionRequest
                );

                return sessionRequest;
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator DeleteNamespace(
                Request.DeleteNamespaceRequest request,
                UnityAction<AsyncResult<Result.DeleteNamespaceResult>> callback
        )
		{
			var task = new DeleteNamespaceTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.DeleteNamespaceResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.DeleteNamespaceResult> DeleteNamespaceAsync(
                Request.DeleteNamespaceRequest request
        )
		{
			var task = new DeleteNamespaceTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        private class DescribeCategoryModelsTask : Gs2RestSessionTask<DescribeCategoryModelsRequest, DescribeCategoryModelsResult>
        {
            public DescribeCategoryModelsTask(IGs2Session session, RestSessionRequestFactory factory, DescribeCategoryModelsRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DescribeCategoryModelsRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "ranking")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/category";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");

                var sessionRequest = Factory.Get(url);
                if (request.ContextStack != null)
                {
                    sessionRequest.AddQueryString("contextStack", request.ContextStack);
                }

                if (request.RequestId != null)
                {
                    sessionRequest.AddHeader("X-GS2-REQUEST-ID", request.RequestId);
                }

                AddHeader(
                    Session.Credential,
                    sessionRequest
                );

                return sessionRequest;
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator DescribeCategoryModels(
                Request.DescribeCategoryModelsRequest request,
                UnityAction<AsyncResult<Result.DescribeCategoryModelsResult>> callback
        )
		{
			var task = new DescribeCategoryModelsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.DescribeCategoryModelsResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.DescribeCategoryModelsResult> DescribeCategoryModelsAsync(
                Request.DescribeCategoryModelsRequest request
        )
		{
			var task = new DescribeCategoryModelsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        private class GetCategoryModelTask : Gs2RestSessionTask<GetCategoryModelRequest, GetCategoryModelResult>
        {
            public GetCategoryModelTask(IGs2Session session, RestSessionRequestFactory factory, GetCategoryModelRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(GetCategoryModelRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "ranking")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/category/{categoryName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{categoryName}", !string.IsNullOrEmpty(request.CategoryName) ? request.CategoryName.ToString() : "null");

                var sessionRequest = Factory.Get(url);
                if (request.ContextStack != null)
                {
                    sessionRequest.AddQueryString("contextStack", request.ContextStack);
                }

                if (request.RequestId != null)
                {
                    sessionRequest.AddHeader("X-GS2-REQUEST-ID", request.RequestId);
                }

                AddHeader(
                    Session.Credential,
                    sessionRequest
                );

                return sessionRequest;
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator GetCategoryModel(
                Request.GetCategoryModelRequest request,
                UnityAction<AsyncResult<Result.GetCategoryModelResult>> callback
        )
		{
			var task = new GetCategoryModelTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.GetCategoryModelResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.GetCategoryModelResult> GetCategoryModelAsync(
                Request.GetCategoryModelRequest request
        )
		{
			var task = new GetCategoryModelTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        private class DescribeCategoryModelMastersTask : Gs2RestSessionTask<DescribeCategoryModelMastersRequest, DescribeCategoryModelMastersResult>
        {
            public DescribeCategoryModelMastersTask(IGs2Session session, RestSessionRequestFactory factory, DescribeCategoryModelMastersRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DescribeCategoryModelMastersRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "ranking")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/master/category";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");

                var sessionRequest = Factory.Get(url);
                if (request.ContextStack != null)
                {
                    sessionRequest.AddQueryString("contextStack", request.ContextStack);
                }
                if (request.PageToken != null) {
                    sessionRequest.AddQueryString("pageToken", $"{request.PageToken}");
                }
                if (request.Limit != null) {
                    sessionRequest.AddQueryString("limit", $"{request.Limit}");
                }

                if (request.RequestId != null)
                {
                    sessionRequest.AddHeader("X-GS2-REQUEST-ID", request.RequestId);
                }

                AddHeader(
                    Session.Credential,
                    sessionRequest
                );

                return sessionRequest;
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator DescribeCategoryModelMasters(
                Request.DescribeCategoryModelMastersRequest request,
                UnityAction<AsyncResult<Result.DescribeCategoryModelMastersResult>> callback
        )
		{
			var task = new DescribeCategoryModelMastersTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.DescribeCategoryModelMastersResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.DescribeCategoryModelMastersResult> DescribeCategoryModelMastersAsync(
                Request.DescribeCategoryModelMastersRequest request
        )
		{
			var task = new DescribeCategoryModelMastersTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        private class CreateCategoryModelMasterTask : Gs2RestSessionTask<CreateCategoryModelMasterRequest, CreateCategoryModelMasterResult>
        {
            public CreateCategoryModelMasterTask(IGs2Session session, RestSessionRequestFactory factory, CreateCategoryModelMasterRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(CreateCategoryModelMasterRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "ranking")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/master/category";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");

                var sessionRequest = Factory.Post(url);

                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);
                jsonWriter.WriteObjectStart();
                if (request.Name != null)
                {
                    jsonWriter.WritePropertyName("name");
                    jsonWriter.Write(request.Name);
                }
                if (request.Description != null)
                {
                    jsonWriter.WritePropertyName("description");
                    jsonWriter.Write(request.Description);
                }
                if (request.Metadata != null)
                {
                    jsonWriter.WritePropertyName("metadata");
                    jsonWriter.Write(request.Metadata);
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
                    jsonWriter.Write(request.OrderDirection);
                }
                if (request.Scope != null)
                {
                    jsonWriter.WritePropertyName("scope");
                    jsonWriter.Write(request.Scope);
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
                    jsonWriter.Write(request.EntryPeriodEventId);
                }
                if (request.AccessPeriodEventId != null)
                {
                    jsonWriter.WritePropertyName("accessPeriodEventId");
                    jsonWriter.Write(request.AccessPeriodEventId);
                }
                if (request.Generation != null)
                {
                    jsonWriter.WritePropertyName("generation");
                    jsonWriter.Write(request.Generation);
                }
                if (request.ContextStack != null)
                {
                    jsonWriter.WritePropertyName("contextStack");
                    jsonWriter.Write(request.ContextStack.ToString());
                }
                jsonWriter.WriteObjectEnd();

                var body = stringBuilder.ToString();
                if (!string.IsNullOrEmpty(body))
                {
                    sessionRequest.Body = body;
                }
                sessionRequest.AddHeader("Content-Type", "application/json");

                if (request.RequestId != null)
                {
                    sessionRequest.AddHeader("X-GS2-REQUEST-ID", request.RequestId);
                }

                AddHeader(
                    Session.Credential,
                    sessionRequest
                );

                return sessionRequest;
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator CreateCategoryModelMaster(
                Request.CreateCategoryModelMasterRequest request,
                UnityAction<AsyncResult<Result.CreateCategoryModelMasterResult>> callback
        )
		{
			var task = new CreateCategoryModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.CreateCategoryModelMasterResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.CreateCategoryModelMasterResult> CreateCategoryModelMasterAsync(
                Request.CreateCategoryModelMasterRequest request
        )
		{
			var task = new CreateCategoryModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        private class GetCategoryModelMasterTask : Gs2RestSessionTask<GetCategoryModelMasterRequest, GetCategoryModelMasterResult>
        {
            public GetCategoryModelMasterTask(IGs2Session session, RestSessionRequestFactory factory, GetCategoryModelMasterRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(GetCategoryModelMasterRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "ranking")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/master/category/{categoryName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{categoryName}", !string.IsNullOrEmpty(request.CategoryName) ? request.CategoryName.ToString() : "null");

                var sessionRequest = Factory.Get(url);
                if (request.ContextStack != null)
                {
                    sessionRequest.AddQueryString("contextStack", request.ContextStack);
                }

                if (request.RequestId != null)
                {
                    sessionRequest.AddHeader("X-GS2-REQUEST-ID", request.RequestId);
                }

                AddHeader(
                    Session.Credential,
                    sessionRequest
                );

                return sessionRequest;
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator GetCategoryModelMaster(
                Request.GetCategoryModelMasterRequest request,
                UnityAction<AsyncResult<Result.GetCategoryModelMasterResult>> callback
        )
		{
			var task = new GetCategoryModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.GetCategoryModelMasterResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.GetCategoryModelMasterResult> GetCategoryModelMasterAsync(
                Request.GetCategoryModelMasterRequest request
        )
		{
			var task = new GetCategoryModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        private class UpdateCategoryModelMasterTask : Gs2RestSessionTask<UpdateCategoryModelMasterRequest, UpdateCategoryModelMasterResult>
        {
            public UpdateCategoryModelMasterTask(IGs2Session session, RestSessionRequestFactory factory, UpdateCategoryModelMasterRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(UpdateCategoryModelMasterRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "ranking")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/master/category/{categoryName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{categoryName}", !string.IsNullOrEmpty(request.CategoryName) ? request.CategoryName.ToString() : "null");

                var sessionRequest = Factory.Put(url);

                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);
                jsonWriter.WriteObjectStart();
                if (request.Description != null)
                {
                    jsonWriter.WritePropertyName("description");
                    jsonWriter.Write(request.Description);
                }
                if (request.Metadata != null)
                {
                    jsonWriter.WritePropertyName("metadata");
                    jsonWriter.Write(request.Metadata);
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
                    jsonWriter.Write(request.OrderDirection);
                }
                if (request.Scope != null)
                {
                    jsonWriter.WritePropertyName("scope");
                    jsonWriter.Write(request.Scope);
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
                    jsonWriter.Write(request.EntryPeriodEventId);
                }
                if (request.AccessPeriodEventId != null)
                {
                    jsonWriter.WritePropertyName("accessPeriodEventId");
                    jsonWriter.Write(request.AccessPeriodEventId);
                }
                if (request.Generation != null)
                {
                    jsonWriter.WritePropertyName("generation");
                    jsonWriter.Write(request.Generation);
                }
                if (request.ContextStack != null)
                {
                    jsonWriter.WritePropertyName("contextStack");
                    jsonWriter.Write(request.ContextStack.ToString());
                }
                jsonWriter.WriteObjectEnd();

                var body = stringBuilder.ToString();
                if (!string.IsNullOrEmpty(body))
                {
                    sessionRequest.Body = body;
                }
                sessionRequest.AddHeader("Content-Type", "application/json");

                if (request.RequestId != null)
                {
                    sessionRequest.AddHeader("X-GS2-REQUEST-ID", request.RequestId);
                }

                AddHeader(
                    Session.Credential,
                    sessionRequest
                );

                return sessionRequest;
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator UpdateCategoryModelMaster(
                Request.UpdateCategoryModelMasterRequest request,
                UnityAction<AsyncResult<Result.UpdateCategoryModelMasterResult>> callback
        )
		{
			var task = new UpdateCategoryModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.UpdateCategoryModelMasterResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.UpdateCategoryModelMasterResult> UpdateCategoryModelMasterAsync(
                Request.UpdateCategoryModelMasterRequest request
        )
		{
			var task = new UpdateCategoryModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        private class DeleteCategoryModelMasterTask : Gs2RestSessionTask<DeleteCategoryModelMasterRequest, DeleteCategoryModelMasterResult>
        {
            public DeleteCategoryModelMasterTask(IGs2Session session, RestSessionRequestFactory factory, DeleteCategoryModelMasterRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DeleteCategoryModelMasterRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "ranking")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/master/category/{categoryName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{categoryName}", !string.IsNullOrEmpty(request.CategoryName) ? request.CategoryName.ToString() : "null");

                var sessionRequest = Factory.Delete(url);
                if (request.ContextStack != null)
                {
                    sessionRequest.AddQueryString("contextStack", request.ContextStack);
                }

                if (request.RequestId != null)
                {
                    sessionRequest.AddHeader("X-GS2-REQUEST-ID", request.RequestId);
                }

                AddHeader(
                    Session.Credential,
                    sessionRequest
                );

                return sessionRequest;
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator DeleteCategoryModelMaster(
                Request.DeleteCategoryModelMasterRequest request,
                UnityAction<AsyncResult<Result.DeleteCategoryModelMasterResult>> callback
        )
		{
			var task = new DeleteCategoryModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.DeleteCategoryModelMasterResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.DeleteCategoryModelMasterResult> DeleteCategoryModelMasterAsync(
                Request.DeleteCategoryModelMasterRequest request
        )
		{
			var task = new DeleteCategoryModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        private class DescribeSubscribesByCategoryNameTask : Gs2RestSessionTask<DescribeSubscribesByCategoryNameRequest, DescribeSubscribesByCategoryNameResult>
        {
            public DescribeSubscribesByCategoryNameTask(IGs2Session session, RestSessionRequestFactory factory, DescribeSubscribesByCategoryNameRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DescribeSubscribesByCategoryNameRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "ranking")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/me/subscribe/category/{categoryName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{categoryName}", !string.IsNullOrEmpty(request.CategoryName) ? request.CategoryName.ToString() : "null");

                var sessionRequest = Factory.Get(url);
                if (request.ContextStack != null)
                {
                    sessionRequest.AddQueryString("contextStack", request.ContextStack);
                }

                if (request.RequestId != null)
                {
                    sessionRequest.AddHeader("X-GS2-REQUEST-ID", request.RequestId);
                }
                if (request.AccessToken != null)
                {
                    sessionRequest.AddHeader("X-GS2-ACCESS-TOKEN", request.AccessToken);
                }

                AddHeader(
                    Session.Credential,
                    sessionRequest
                );

                return sessionRequest;
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator DescribeSubscribesByCategoryName(
                Request.DescribeSubscribesByCategoryNameRequest request,
                UnityAction<AsyncResult<Result.DescribeSubscribesByCategoryNameResult>> callback
        )
		{
			var task = new DescribeSubscribesByCategoryNameTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.DescribeSubscribesByCategoryNameResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.DescribeSubscribesByCategoryNameResult> DescribeSubscribesByCategoryNameAsync(
                Request.DescribeSubscribesByCategoryNameRequest request
        )
		{
			var task = new DescribeSubscribesByCategoryNameTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        private class DescribeSubscribesByCategoryNameAndUserIdTask : Gs2RestSessionTask<DescribeSubscribesByCategoryNameAndUserIdRequest, DescribeSubscribesByCategoryNameAndUserIdResult>
        {
            public DescribeSubscribesByCategoryNameAndUserIdTask(IGs2Session session, RestSessionRequestFactory factory, DescribeSubscribesByCategoryNameAndUserIdRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DescribeSubscribesByCategoryNameAndUserIdRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "ranking")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/{userId}/subscribe/category/{categoryName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{categoryName}", !string.IsNullOrEmpty(request.CategoryName) ? request.CategoryName.ToString() : "null");
                url = url.Replace("{userId}", !string.IsNullOrEmpty(request.UserId) ? request.UserId.ToString() : "null");

                var sessionRequest = Factory.Get(url);
                if (request.ContextStack != null)
                {
                    sessionRequest.AddQueryString("contextStack", request.ContextStack);
                }

                if (request.RequestId != null)
                {
                    sessionRequest.AddHeader("X-GS2-REQUEST-ID", request.RequestId);
                }

                AddHeader(
                    Session.Credential,
                    sessionRequest
                );

                return sessionRequest;
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator DescribeSubscribesByCategoryNameAndUserId(
                Request.DescribeSubscribesByCategoryNameAndUserIdRequest request,
                UnityAction<AsyncResult<Result.DescribeSubscribesByCategoryNameAndUserIdResult>> callback
        )
		{
			var task = new DescribeSubscribesByCategoryNameAndUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.DescribeSubscribesByCategoryNameAndUserIdResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.DescribeSubscribesByCategoryNameAndUserIdResult> DescribeSubscribesByCategoryNameAndUserIdAsync(
                Request.DescribeSubscribesByCategoryNameAndUserIdRequest request
        )
		{
			var task = new DescribeSubscribesByCategoryNameAndUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        private class SubscribeTask : Gs2RestSessionTask<SubscribeRequest, SubscribeResult>
        {
            public SubscribeTask(IGs2Session session, RestSessionRequestFactory factory, SubscribeRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(SubscribeRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "ranking")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/me/subscribe/category/{categoryName}/target/{targetUserId}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{categoryName}", !string.IsNullOrEmpty(request.CategoryName) ? request.CategoryName.ToString() : "null");
                url = url.Replace("{targetUserId}", !string.IsNullOrEmpty(request.TargetUserId) ? request.TargetUserId.ToString() : "null");

                var sessionRequest = Factory.Post(url);

                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);
                jsonWriter.WriteObjectStart();
                if (request.ContextStack != null)
                {
                    jsonWriter.WritePropertyName("contextStack");
                    jsonWriter.Write(request.ContextStack.ToString());
                }
                jsonWriter.WriteObjectEnd();

                var body = stringBuilder.ToString();
                if (!string.IsNullOrEmpty(body))
                {
                    sessionRequest.Body = body;
                }
                sessionRequest.AddHeader("Content-Type", "application/json");

                if (request.RequestId != null)
                {
                    sessionRequest.AddHeader("X-GS2-REQUEST-ID", request.RequestId);
                }
                if (request.AccessToken != null)
                {
                    sessionRequest.AddHeader("X-GS2-ACCESS-TOKEN", request.AccessToken);
                }

                AddHeader(
                    Session.Credential,
                    sessionRequest
                );

                return sessionRequest;
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator Subscribe(
                Request.SubscribeRequest request,
                UnityAction<AsyncResult<Result.SubscribeResult>> callback
        )
		{
			var task = new SubscribeTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.SubscribeResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.SubscribeResult> SubscribeAsync(
                Request.SubscribeRequest request
        )
		{
			var task = new SubscribeTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        private class SubscribeByUserIdTask : Gs2RestSessionTask<SubscribeByUserIdRequest, SubscribeByUserIdResult>
        {
            public SubscribeByUserIdTask(IGs2Session session, RestSessionRequestFactory factory, SubscribeByUserIdRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(SubscribeByUserIdRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "ranking")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/{userId}/subscribe/category/{categoryName}/target/{targetUserId}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{categoryName}", !string.IsNullOrEmpty(request.CategoryName) ? request.CategoryName.ToString() : "null");
                url = url.Replace("{userId}", !string.IsNullOrEmpty(request.UserId) ? request.UserId.ToString() : "null");
                url = url.Replace("{targetUserId}", !string.IsNullOrEmpty(request.TargetUserId) ? request.TargetUserId.ToString() : "null");

                var sessionRequest = Factory.Post(url);

                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);
                jsonWriter.WriteObjectStart();
                if (request.ContextStack != null)
                {
                    jsonWriter.WritePropertyName("contextStack");
                    jsonWriter.Write(request.ContextStack.ToString());
                }
                jsonWriter.WriteObjectEnd();

                var body = stringBuilder.ToString();
                if (!string.IsNullOrEmpty(body))
                {
                    sessionRequest.Body = body;
                }
                sessionRequest.AddHeader("Content-Type", "application/json");

                if (request.RequestId != null)
                {
                    sessionRequest.AddHeader("X-GS2-REQUEST-ID", request.RequestId);
                }

                AddHeader(
                    Session.Credential,
                    sessionRequest
                );

                return sessionRequest;
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator SubscribeByUserId(
                Request.SubscribeByUserIdRequest request,
                UnityAction<AsyncResult<Result.SubscribeByUserIdResult>> callback
        )
		{
			var task = new SubscribeByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.SubscribeByUserIdResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.SubscribeByUserIdResult> SubscribeByUserIdAsync(
                Request.SubscribeByUserIdRequest request
        )
		{
			var task = new SubscribeByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        private class GetSubscribeTask : Gs2RestSessionTask<GetSubscribeRequest, GetSubscribeResult>
        {
            public GetSubscribeTask(IGs2Session session, RestSessionRequestFactory factory, GetSubscribeRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(GetSubscribeRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "ranking")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/me/subscribe/category/{categoryName}/target/{targetUserId}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{categoryName}", !string.IsNullOrEmpty(request.CategoryName) ? request.CategoryName.ToString() : "null");
                url = url.Replace("{targetUserId}", !string.IsNullOrEmpty(request.TargetUserId) ? request.TargetUserId.ToString() : "null");

                var sessionRequest = Factory.Get(url);
                if (request.ContextStack != null)
                {
                    sessionRequest.AddQueryString("contextStack", request.ContextStack);
                }

                if (request.RequestId != null)
                {
                    sessionRequest.AddHeader("X-GS2-REQUEST-ID", request.RequestId);
                }
                if (request.AccessToken != null)
                {
                    sessionRequest.AddHeader("X-GS2-ACCESS-TOKEN", request.AccessToken);
                }

                AddHeader(
                    Session.Credential,
                    sessionRequest
                );

                return sessionRequest;
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator GetSubscribe(
                Request.GetSubscribeRequest request,
                UnityAction<AsyncResult<Result.GetSubscribeResult>> callback
        )
		{
			var task = new GetSubscribeTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.GetSubscribeResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.GetSubscribeResult> GetSubscribeAsync(
                Request.GetSubscribeRequest request
        )
		{
			var task = new GetSubscribeTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        private class GetSubscribeByUserIdTask : Gs2RestSessionTask<GetSubscribeByUserIdRequest, GetSubscribeByUserIdResult>
        {
            public GetSubscribeByUserIdTask(IGs2Session session, RestSessionRequestFactory factory, GetSubscribeByUserIdRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(GetSubscribeByUserIdRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "ranking")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/{userId}/subscribe/category/{categoryName}/target/{targetUserId}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{categoryName}", !string.IsNullOrEmpty(request.CategoryName) ? request.CategoryName.ToString() : "null");
                url = url.Replace("{userId}", !string.IsNullOrEmpty(request.UserId) ? request.UserId.ToString() : "null");
                url = url.Replace("{targetUserId}", !string.IsNullOrEmpty(request.TargetUserId) ? request.TargetUserId.ToString() : "null");

                var sessionRequest = Factory.Get(url);
                if (request.ContextStack != null)
                {
                    sessionRequest.AddQueryString("contextStack", request.ContextStack);
                }

                if (request.RequestId != null)
                {
                    sessionRequest.AddHeader("X-GS2-REQUEST-ID", request.RequestId);
                }

                AddHeader(
                    Session.Credential,
                    sessionRequest
                );

                return sessionRequest;
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator GetSubscribeByUserId(
                Request.GetSubscribeByUserIdRequest request,
                UnityAction<AsyncResult<Result.GetSubscribeByUserIdResult>> callback
        )
		{
			var task = new GetSubscribeByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.GetSubscribeByUserIdResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.GetSubscribeByUserIdResult> GetSubscribeByUserIdAsync(
                Request.GetSubscribeByUserIdRequest request
        )
		{
			var task = new GetSubscribeByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        private class UnsubscribeTask : Gs2RestSessionTask<UnsubscribeRequest, UnsubscribeResult>
        {
            public UnsubscribeTask(IGs2Session session, RestSessionRequestFactory factory, UnsubscribeRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(UnsubscribeRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "ranking")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/me/subscribe/category/{categoryName}/target/{targetUserId}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{categoryName}", !string.IsNullOrEmpty(request.CategoryName) ? request.CategoryName.ToString() : "null");
                url = url.Replace("{targetUserId}", !string.IsNullOrEmpty(request.TargetUserId) ? request.TargetUserId.ToString() : "null");

                var sessionRequest = Factory.Delete(url);
                if (request.ContextStack != null)
                {
                    sessionRequest.AddQueryString("contextStack", request.ContextStack);
                }

                if (request.RequestId != null)
                {
                    sessionRequest.AddHeader("X-GS2-REQUEST-ID", request.RequestId);
                }
                if (request.AccessToken != null)
                {
                    sessionRequest.AddHeader("X-GS2-ACCESS-TOKEN", request.AccessToken);
                }

                AddHeader(
                    Session.Credential,
                    sessionRequest
                );

                return sessionRequest;
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator Unsubscribe(
                Request.UnsubscribeRequest request,
                UnityAction<AsyncResult<Result.UnsubscribeResult>> callback
        )
		{
			var task = new UnsubscribeTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.UnsubscribeResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.UnsubscribeResult> UnsubscribeAsync(
                Request.UnsubscribeRequest request
        )
		{
			var task = new UnsubscribeTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        private class UnsubscribeByUserIdTask : Gs2RestSessionTask<UnsubscribeByUserIdRequest, UnsubscribeByUserIdResult>
        {
            public UnsubscribeByUserIdTask(IGs2Session session, RestSessionRequestFactory factory, UnsubscribeByUserIdRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(UnsubscribeByUserIdRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "ranking")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/{userId}/subscribe/category/{categoryName}/target/{targetUserId}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{categoryName}", !string.IsNullOrEmpty(request.CategoryName) ? request.CategoryName.ToString() : "null");
                url = url.Replace("{userId}", !string.IsNullOrEmpty(request.UserId) ? request.UserId.ToString() : "null");
                url = url.Replace("{targetUserId}", !string.IsNullOrEmpty(request.TargetUserId) ? request.TargetUserId.ToString() : "null");

                var sessionRequest = Factory.Delete(url);
                if (request.ContextStack != null)
                {
                    sessionRequest.AddQueryString("contextStack", request.ContextStack);
                }

                if (request.RequestId != null)
                {
                    sessionRequest.AddHeader("X-GS2-REQUEST-ID", request.RequestId);
                }

                AddHeader(
                    Session.Credential,
                    sessionRequest
                );

                return sessionRequest;
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator UnsubscribeByUserId(
                Request.UnsubscribeByUserIdRequest request,
                UnityAction<AsyncResult<Result.UnsubscribeByUserIdResult>> callback
        )
		{
			var task = new UnsubscribeByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.UnsubscribeByUserIdResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.UnsubscribeByUserIdResult> UnsubscribeByUserIdAsync(
                Request.UnsubscribeByUserIdRequest request
        )
		{
			var task = new UnsubscribeByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        private class DescribeScoresTask : Gs2RestSessionTask<DescribeScoresRequest, DescribeScoresResult>
        {
            public DescribeScoresTask(IGs2Session session, RestSessionRequestFactory factory, DescribeScoresRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DescribeScoresRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "ranking")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/me/category/{categoryName}/scorer/{scorerUserId}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{categoryName}", !string.IsNullOrEmpty(request.CategoryName) ? request.CategoryName.ToString() : "null");
                url = url.Replace("{scorerUserId}", !string.IsNullOrEmpty(request.ScorerUserId) ? request.ScorerUserId.ToString() : "null");

                var sessionRequest = Factory.Get(url);
                if (request.ContextStack != null)
                {
                    sessionRequest.AddQueryString("contextStack", request.ContextStack);
                }
                if (request.PageToken != null) {
                    sessionRequest.AddQueryString("pageToken", $"{request.PageToken}");
                }
                if (request.Limit != null) {
                    sessionRequest.AddQueryString("limit", $"{request.Limit}");
                }

                if (request.RequestId != null)
                {
                    sessionRequest.AddHeader("X-GS2-REQUEST-ID", request.RequestId);
                }
                if (request.AccessToken != null)
                {
                    sessionRequest.AddHeader("X-GS2-ACCESS-TOKEN", request.AccessToken);
                }

                AddHeader(
                    Session.Credential,
                    sessionRequest
                );

                return sessionRequest;
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator DescribeScores(
                Request.DescribeScoresRequest request,
                UnityAction<AsyncResult<Result.DescribeScoresResult>> callback
        )
		{
			var task = new DescribeScoresTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.DescribeScoresResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.DescribeScoresResult> DescribeScoresAsync(
                Request.DescribeScoresRequest request
        )
		{
			var task = new DescribeScoresTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        private class DescribeScoresByUserIdTask : Gs2RestSessionTask<DescribeScoresByUserIdRequest, DescribeScoresByUserIdResult>
        {
            public DescribeScoresByUserIdTask(IGs2Session session, RestSessionRequestFactory factory, DescribeScoresByUserIdRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DescribeScoresByUserIdRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "ranking")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/{userId}/category/{categoryName}/scorer/{scorerUserId}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{categoryName}", !string.IsNullOrEmpty(request.CategoryName) ? request.CategoryName.ToString() : "null");
                url = url.Replace("{userId}", !string.IsNullOrEmpty(request.UserId) ? request.UserId.ToString() : "null");
                url = url.Replace("{scorerUserId}", !string.IsNullOrEmpty(request.ScorerUserId) ? request.ScorerUserId.ToString() : "null");

                var sessionRequest = Factory.Get(url);
                if (request.ContextStack != null)
                {
                    sessionRequest.AddQueryString("contextStack", request.ContextStack);
                }
                if (request.PageToken != null) {
                    sessionRequest.AddQueryString("pageToken", $"{request.PageToken}");
                }
                if (request.Limit != null) {
                    sessionRequest.AddQueryString("limit", $"{request.Limit}");
                }

                if (request.RequestId != null)
                {
                    sessionRequest.AddHeader("X-GS2-REQUEST-ID", request.RequestId);
                }

                AddHeader(
                    Session.Credential,
                    sessionRequest
                );

                return sessionRequest;
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator DescribeScoresByUserId(
                Request.DescribeScoresByUserIdRequest request,
                UnityAction<AsyncResult<Result.DescribeScoresByUserIdResult>> callback
        )
		{
			var task = new DescribeScoresByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.DescribeScoresByUserIdResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.DescribeScoresByUserIdResult> DescribeScoresByUserIdAsync(
                Request.DescribeScoresByUserIdRequest request
        )
		{
			var task = new DescribeScoresByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        private class GetScoreTask : Gs2RestSessionTask<GetScoreRequest, GetScoreResult>
        {
            public GetScoreTask(IGs2Session session, RestSessionRequestFactory factory, GetScoreRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(GetScoreRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "ranking")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/me/category/{categoryName}/scorer/{scorerUserId}/score/{uniqueId}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{categoryName}", !string.IsNullOrEmpty(request.CategoryName) ? request.CategoryName.ToString() : "null");
                url = url.Replace("{scorerUserId}", !string.IsNullOrEmpty(request.ScorerUserId) ? request.ScorerUserId.ToString() : "null");
                url = url.Replace("{uniqueId}", !string.IsNullOrEmpty(request.UniqueId) ? request.UniqueId.ToString() : "null");

                var sessionRequest = Factory.Get(url);
                if (request.ContextStack != null)
                {
                    sessionRequest.AddQueryString("contextStack", request.ContextStack);
                }

                if (request.RequestId != null)
                {
                    sessionRequest.AddHeader("X-GS2-REQUEST-ID", request.RequestId);
                }
                if (request.AccessToken != null)
                {
                    sessionRequest.AddHeader("X-GS2-ACCESS-TOKEN", request.AccessToken);
                }

                AddHeader(
                    Session.Credential,
                    sessionRequest
                );

                return sessionRequest;
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator GetScore(
                Request.GetScoreRequest request,
                UnityAction<AsyncResult<Result.GetScoreResult>> callback
        )
		{
			var task = new GetScoreTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.GetScoreResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.GetScoreResult> GetScoreAsync(
                Request.GetScoreRequest request
        )
		{
			var task = new GetScoreTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        private class GetScoreByUserIdTask : Gs2RestSessionTask<GetScoreByUserIdRequest, GetScoreByUserIdResult>
        {
            public GetScoreByUserIdTask(IGs2Session session, RestSessionRequestFactory factory, GetScoreByUserIdRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(GetScoreByUserIdRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "ranking")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/{userId}/category/{categoryName}/scorer/{scorerUserId}/score/{uniqueId}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{categoryName}", !string.IsNullOrEmpty(request.CategoryName) ? request.CategoryName.ToString() : "null");
                url = url.Replace("{userId}", !string.IsNullOrEmpty(request.UserId) ? request.UserId.ToString() : "null");
                url = url.Replace("{scorerUserId}", !string.IsNullOrEmpty(request.ScorerUserId) ? request.ScorerUserId.ToString() : "null");
                url = url.Replace("{uniqueId}", !string.IsNullOrEmpty(request.UniqueId) ? request.UniqueId.ToString() : "null");

                var sessionRequest = Factory.Get(url);
                if (request.ContextStack != null)
                {
                    sessionRequest.AddQueryString("contextStack", request.ContextStack);
                }

                if (request.RequestId != null)
                {
                    sessionRequest.AddHeader("X-GS2-REQUEST-ID", request.RequestId);
                }

                AddHeader(
                    Session.Credential,
                    sessionRequest
                );

                return sessionRequest;
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator GetScoreByUserId(
                Request.GetScoreByUserIdRequest request,
                UnityAction<AsyncResult<Result.GetScoreByUserIdResult>> callback
        )
		{
			var task = new GetScoreByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.GetScoreByUserIdResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.GetScoreByUserIdResult> GetScoreByUserIdAsync(
                Request.GetScoreByUserIdRequest request
        )
		{
			var task = new GetScoreByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        private class DescribeRankingsTask : Gs2RestSessionTask<DescribeRankingsRequest, DescribeRankingsResult>
        {
            public DescribeRankingsTask(IGs2Session session, RestSessionRequestFactory factory, DescribeRankingsRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DescribeRankingsRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "ranking")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/me/category/{categoryName}/ranking";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{categoryName}", !string.IsNullOrEmpty(request.CategoryName) ? request.CategoryName.ToString() : "null");

                var sessionRequest = Factory.Get(url);
                if (request.ContextStack != null)
                {
                    sessionRequest.AddQueryString("contextStack", request.ContextStack);
                }
                if (request.StartIndex != null) {
                    sessionRequest.AddQueryString("startIndex", $"{request.StartIndex}");
                }
                if (request.PageToken != null) {
                    sessionRequest.AddQueryString("pageToken", $"{request.PageToken}");
                }
                if (request.Limit != null) {
                    sessionRequest.AddQueryString("limit", $"{request.Limit}");
                }

                if (request.RequestId != null)
                {
                    sessionRequest.AddHeader("X-GS2-REQUEST-ID", request.RequestId);
                }
                if (request.AccessToken != null)
                {
                    sessionRequest.AddHeader("X-GS2-ACCESS-TOKEN", request.AccessToken);
                }

                AddHeader(
                    Session.Credential,
                    sessionRequest
                );

                return sessionRequest;
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator DescribeRankings(
                Request.DescribeRankingsRequest request,
                UnityAction<AsyncResult<Result.DescribeRankingsResult>> callback
        )
		{
			var task = new DescribeRankingsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.DescribeRankingsResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.DescribeRankingsResult> DescribeRankingsAsync(
                Request.DescribeRankingsRequest request
        )
		{
			var task = new DescribeRankingsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        private class DescribeRankingssByUserIdTask : Gs2RestSessionTask<DescribeRankingssByUserIdRequest, DescribeRankingssByUserIdResult>
        {
            public DescribeRankingssByUserIdTask(IGs2Session session, RestSessionRequestFactory factory, DescribeRankingssByUserIdRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DescribeRankingssByUserIdRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "ranking")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/{userId}/category/{categoryName}/ranking";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{categoryName}", !string.IsNullOrEmpty(request.CategoryName) ? request.CategoryName.ToString() : "null");
                url = url.Replace("{userId}", !string.IsNullOrEmpty(request.UserId) ? request.UserId.ToString() : "null");

                var sessionRequest = Factory.Get(url);
                if (request.ContextStack != null)
                {
                    sessionRequest.AddQueryString("contextStack", request.ContextStack);
                }
                if (request.StartIndex != null) {
                    sessionRequest.AddQueryString("startIndex", $"{request.StartIndex}");
                }
                if (request.PageToken != null) {
                    sessionRequest.AddQueryString("pageToken", $"{request.PageToken}");
                }
                if (request.Limit != null) {
                    sessionRequest.AddQueryString("limit", $"{request.Limit}");
                }

                if (request.RequestId != null)
                {
                    sessionRequest.AddHeader("X-GS2-REQUEST-ID", request.RequestId);
                }

                AddHeader(
                    Session.Credential,
                    sessionRequest
                );

                return sessionRequest;
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator DescribeRankingssByUserId(
                Request.DescribeRankingssByUserIdRequest request,
                UnityAction<AsyncResult<Result.DescribeRankingssByUserIdResult>> callback
        )
		{
			var task = new DescribeRankingssByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.DescribeRankingssByUserIdResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.DescribeRankingssByUserIdResult> DescribeRankingssByUserIdAsync(
                Request.DescribeRankingssByUserIdRequest request
        )
		{
			var task = new DescribeRankingssByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        private class DescribeNearRankingsTask : Gs2RestSessionTask<DescribeNearRankingsRequest, DescribeNearRankingsResult>
        {
            public DescribeNearRankingsTask(IGs2Session session, RestSessionRequestFactory factory, DescribeNearRankingsRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DescribeNearRankingsRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "ranking")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/me/category/{categoryName}/ranking/near";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{categoryName}", !string.IsNullOrEmpty(request.CategoryName) ? request.CategoryName.ToString() : "null");

                var sessionRequest = Factory.Get(url);
                if (request.ContextStack != null)
                {
                    sessionRequest.AddQueryString("contextStack", request.ContextStack);
                }
                if (request.Score != null) {
                    sessionRequest.AddQueryString("score", $"{request.Score}");
                }

                if (request.RequestId != null)
                {
                    sessionRequest.AddHeader("X-GS2-REQUEST-ID", request.RequestId);
                }

                AddHeader(
                    Session.Credential,
                    sessionRequest
                );

                return sessionRequest;
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator DescribeNearRankings(
                Request.DescribeNearRankingsRequest request,
                UnityAction<AsyncResult<Result.DescribeNearRankingsResult>> callback
        )
		{
			var task = new DescribeNearRankingsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.DescribeNearRankingsResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.DescribeNearRankingsResult> DescribeNearRankingsAsync(
                Request.DescribeNearRankingsRequest request
        )
		{
			var task = new DescribeNearRankingsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        private class GetRankingTask : Gs2RestSessionTask<GetRankingRequest, GetRankingResult>
        {
            public GetRankingTask(IGs2Session session, RestSessionRequestFactory factory, GetRankingRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(GetRankingRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "ranking")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/me/category/{categoryName}/ranking/scorer/{scorerUserId}/score/{uniqueId}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{categoryName}", !string.IsNullOrEmpty(request.CategoryName) ? request.CategoryName.ToString() : "null");
                url = url.Replace("{scorerUserId}", !string.IsNullOrEmpty(request.ScorerUserId) ? request.ScorerUserId.ToString() : "null");
                url = url.Replace("{uniqueId}", !string.IsNullOrEmpty(request.UniqueId) ? request.UniqueId.ToString() : "null");

                var sessionRequest = Factory.Get(url);
                if (request.ContextStack != null)
                {
                    sessionRequest.AddQueryString("contextStack", request.ContextStack);
                }

                if (request.RequestId != null)
                {
                    sessionRequest.AddHeader("X-GS2-REQUEST-ID", request.RequestId);
                }
                if (request.AccessToken != null)
                {
                    sessionRequest.AddHeader("X-GS2-ACCESS-TOKEN", request.AccessToken);
                }

                AddHeader(
                    Session.Credential,
                    sessionRequest
                );

                return sessionRequest;
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator GetRanking(
                Request.GetRankingRequest request,
                UnityAction<AsyncResult<Result.GetRankingResult>> callback
        )
		{
			var task = new GetRankingTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.GetRankingResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.GetRankingResult> GetRankingAsync(
                Request.GetRankingRequest request
        )
		{
			var task = new GetRankingTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        private class GetRankingByUserIdTask : Gs2RestSessionTask<GetRankingByUserIdRequest, GetRankingByUserIdResult>
        {
            public GetRankingByUserIdTask(IGs2Session session, RestSessionRequestFactory factory, GetRankingByUserIdRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(GetRankingByUserIdRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "ranking")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/{userId}/category/{categoryName}/ranking/scorer/{scorerUserId}/score/{uniqueId}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{categoryName}", !string.IsNullOrEmpty(request.CategoryName) ? request.CategoryName.ToString() : "null");
                url = url.Replace("{userId}", !string.IsNullOrEmpty(request.UserId) ? request.UserId.ToString() : "null");
                url = url.Replace("{scorerUserId}", !string.IsNullOrEmpty(request.ScorerUserId) ? request.ScorerUserId.ToString() : "null");
                url = url.Replace("{uniqueId}", !string.IsNullOrEmpty(request.UniqueId) ? request.UniqueId.ToString() : "null");

                var sessionRequest = Factory.Get(url);
                if (request.ContextStack != null)
                {
                    sessionRequest.AddQueryString("contextStack", request.ContextStack);
                }

                if (request.RequestId != null)
                {
                    sessionRequest.AddHeader("X-GS2-REQUEST-ID", request.RequestId);
                }

                AddHeader(
                    Session.Credential,
                    sessionRequest
                );

                return sessionRequest;
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator GetRankingByUserId(
                Request.GetRankingByUserIdRequest request,
                UnityAction<AsyncResult<Result.GetRankingByUserIdResult>> callback
        )
		{
			var task = new GetRankingByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.GetRankingByUserIdResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.GetRankingByUserIdResult> GetRankingByUserIdAsync(
                Request.GetRankingByUserIdRequest request
        )
		{
			var task = new GetRankingByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        private class PutScoreTask : Gs2RestSessionTask<PutScoreRequest, PutScoreResult>
        {
            public PutScoreTask(IGs2Session session, RestSessionRequestFactory factory, PutScoreRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(PutScoreRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "ranking")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/me/category/{categoryName}/ranking";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{categoryName}", !string.IsNullOrEmpty(request.CategoryName) ? request.CategoryName.ToString() : "null");

                var sessionRequest = Factory.Put(url);

                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);
                jsonWriter.WriteObjectStart();
                if (request.Score != null)
                {
                    jsonWriter.WritePropertyName("score");
                    jsonWriter.Write(request.Score.ToString());
                }
                if (request.Metadata != null)
                {
                    jsonWriter.WritePropertyName("metadata");
                    jsonWriter.Write(request.Metadata);
                }
                if (request.ContextStack != null)
                {
                    jsonWriter.WritePropertyName("contextStack");
                    jsonWriter.Write(request.ContextStack.ToString());
                }
                jsonWriter.WriteObjectEnd();

                var body = stringBuilder.ToString();
                if (!string.IsNullOrEmpty(body))
                {
                    sessionRequest.Body = body;
                }
                sessionRequest.AddHeader("Content-Type", "application/json");

                if (request.RequestId != null)
                {
                    sessionRequest.AddHeader("X-GS2-REQUEST-ID", request.RequestId);
                }
                if (request.AccessToken != null)
                {
                    sessionRequest.AddHeader("X-GS2-ACCESS-TOKEN", request.AccessToken);
                }

                AddHeader(
                    Session.Credential,
                    sessionRequest
                );

                return sessionRequest;
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator PutScore(
                Request.PutScoreRequest request,
                UnityAction<AsyncResult<Result.PutScoreResult>> callback
        )
		{
			var task = new PutScoreTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.PutScoreResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.PutScoreResult> PutScoreAsync(
                Request.PutScoreRequest request
        )
		{
			var task = new PutScoreTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        private class PutScoreByUserIdTask : Gs2RestSessionTask<PutScoreByUserIdRequest, PutScoreByUserIdResult>
        {
            public PutScoreByUserIdTask(IGs2Session session, RestSessionRequestFactory factory, PutScoreByUserIdRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(PutScoreByUserIdRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "ranking")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/{userId}/category/{categoryName}/ranking";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{categoryName}", !string.IsNullOrEmpty(request.CategoryName) ? request.CategoryName.ToString() : "null");
                url = url.Replace("{userId}", !string.IsNullOrEmpty(request.UserId) ? request.UserId.ToString() : "null");

                var sessionRequest = Factory.Put(url);

                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);
                jsonWriter.WriteObjectStart();
                if (request.Score != null)
                {
                    jsonWriter.WritePropertyName("score");
                    jsonWriter.Write(request.Score.ToString());
                }
                if (request.Metadata != null)
                {
                    jsonWriter.WritePropertyName("metadata");
                    jsonWriter.Write(request.Metadata);
                }
                if (request.ContextStack != null)
                {
                    jsonWriter.WritePropertyName("contextStack");
                    jsonWriter.Write(request.ContextStack.ToString());
                }
                jsonWriter.WriteObjectEnd();

                var body = stringBuilder.ToString();
                if (!string.IsNullOrEmpty(body))
                {
                    sessionRequest.Body = body;
                }
                sessionRequest.AddHeader("Content-Type", "application/json");

                if (request.RequestId != null)
                {
                    sessionRequest.AddHeader("X-GS2-REQUEST-ID", request.RequestId);
                }

                AddHeader(
                    Session.Credential,
                    sessionRequest
                );

                return sessionRequest;
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator PutScoreByUserId(
                Request.PutScoreByUserIdRequest request,
                UnityAction<AsyncResult<Result.PutScoreByUserIdResult>> callback
        )
		{
			var task = new PutScoreByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.PutScoreByUserIdResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.PutScoreByUserIdResult> PutScoreByUserIdAsync(
                Request.PutScoreByUserIdRequest request
        )
		{
			var task = new PutScoreByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        private class CalcRankingTask : Gs2RestSessionTask<CalcRankingRequest, CalcRankingResult>
        {
            public CalcRankingTask(IGs2Session session, RestSessionRequestFactory factory, CalcRankingRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(CalcRankingRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "ranking")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/category/{categoryName}/calc/ranking";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{categoryName}", !string.IsNullOrEmpty(request.CategoryName) ? request.CategoryName.ToString() : "null");

                var sessionRequest = Factory.Post(url);

                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);
                jsonWriter.WriteObjectStart();
                if (request.ContextStack != null)
                {
                    jsonWriter.WritePropertyName("contextStack");
                    jsonWriter.Write(request.ContextStack.ToString());
                }
                jsonWriter.WriteObjectEnd();

                var body = stringBuilder.ToString();
                if (!string.IsNullOrEmpty(body))
                {
                    sessionRequest.Body = body;
                }
                sessionRequest.AddHeader("Content-Type", "application/json");

                if (request.RequestId != null)
                {
                    sessionRequest.AddHeader("X-GS2-REQUEST-ID", request.RequestId);
                }

                AddHeader(
                    Session.Credential,
                    sessionRequest
                );

                return sessionRequest;
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator CalcRanking(
                Request.CalcRankingRequest request,
                UnityAction<AsyncResult<Result.CalcRankingResult>> callback
        )
		{
			var task = new CalcRankingTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.CalcRankingResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.CalcRankingResult> CalcRankingAsync(
                Request.CalcRankingRequest request
        )
		{
			var task = new CalcRankingTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        private class ExportMasterTask : Gs2RestSessionTask<ExportMasterRequest, ExportMasterResult>
        {
            public ExportMasterTask(IGs2Session session, RestSessionRequestFactory factory, ExportMasterRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(ExportMasterRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "ranking")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/master/export";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");

                var sessionRequest = Factory.Get(url);
                if (request.ContextStack != null)
                {
                    sessionRequest.AddQueryString("contextStack", request.ContextStack);
                }

                if (request.RequestId != null)
                {
                    sessionRequest.AddHeader("X-GS2-REQUEST-ID", request.RequestId);
                }

                AddHeader(
                    Session.Credential,
                    sessionRequest
                );

                return sessionRequest;
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator ExportMaster(
                Request.ExportMasterRequest request,
                UnityAction<AsyncResult<Result.ExportMasterResult>> callback
        )
		{
			var task = new ExportMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.ExportMasterResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.ExportMasterResult> ExportMasterAsync(
                Request.ExportMasterRequest request
        )
		{
			var task = new ExportMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        private class GetCurrentRankingMasterTask : Gs2RestSessionTask<GetCurrentRankingMasterRequest, GetCurrentRankingMasterResult>
        {
            public GetCurrentRankingMasterTask(IGs2Session session, RestSessionRequestFactory factory, GetCurrentRankingMasterRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(GetCurrentRankingMasterRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "ranking")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/master";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");

                var sessionRequest = Factory.Get(url);
                if (request.ContextStack != null)
                {
                    sessionRequest.AddQueryString("contextStack", request.ContextStack);
                }

                if (request.RequestId != null)
                {
                    sessionRequest.AddHeader("X-GS2-REQUEST-ID", request.RequestId);
                }

                AddHeader(
                    Session.Credential,
                    sessionRequest
                );

                return sessionRequest;
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator GetCurrentRankingMaster(
                Request.GetCurrentRankingMasterRequest request,
                UnityAction<AsyncResult<Result.GetCurrentRankingMasterResult>> callback
        )
		{
			var task = new GetCurrentRankingMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.GetCurrentRankingMasterResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.GetCurrentRankingMasterResult> GetCurrentRankingMasterAsync(
                Request.GetCurrentRankingMasterRequest request
        )
		{
			var task = new GetCurrentRankingMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        private class UpdateCurrentRankingMasterTask : Gs2RestSessionTask<UpdateCurrentRankingMasterRequest, UpdateCurrentRankingMasterResult>
        {
            public UpdateCurrentRankingMasterTask(IGs2Session session, RestSessionRequestFactory factory, UpdateCurrentRankingMasterRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(UpdateCurrentRankingMasterRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "ranking")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/master";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");

                var sessionRequest = Factory.Put(url);

                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);
                jsonWriter.WriteObjectStart();
                if (request.Settings != null)
                {
                    jsonWriter.WritePropertyName("settings");
                    jsonWriter.Write(request.Settings);
                }
                if (request.ContextStack != null)
                {
                    jsonWriter.WritePropertyName("contextStack");
                    jsonWriter.Write(request.ContextStack.ToString());
                }
                jsonWriter.WriteObjectEnd();

                var body = stringBuilder.ToString();
                if (!string.IsNullOrEmpty(body))
                {
                    sessionRequest.Body = body;
                }
                sessionRequest.AddHeader("Content-Type", "application/json");

                if (request.RequestId != null)
                {
                    sessionRequest.AddHeader("X-GS2-REQUEST-ID", request.RequestId);
                }

                AddHeader(
                    Session.Credential,
                    sessionRequest
                );

                return sessionRequest;
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator UpdateCurrentRankingMaster(
                Request.UpdateCurrentRankingMasterRequest request,
                UnityAction<AsyncResult<Result.UpdateCurrentRankingMasterResult>> callback
        )
		{
			var task = new UpdateCurrentRankingMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.UpdateCurrentRankingMasterResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.UpdateCurrentRankingMasterResult> UpdateCurrentRankingMasterAsync(
                Request.UpdateCurrentRankingMasterRequest request
        )
		{
			var task = new UpdateCurrentRankingMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        private class UpdateCurrentRankingMasterFromGitHubTask : Gs2RestSessionTask<UpdateCurrentRankingMasterFromGitHubRequest, UpdateCurrentRankingMasterFromGitHubResult>
        {
            public UpdateCurrentRankingMasterFromGitHubTask(IGs2Session session, RestSessionRequestFactory factory, UpdateCurrentRankingMasterFromGitHubRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(UpdateCurrentRankingMasterFromGitHubRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "ranking")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/master/from_git_hub";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");

                var sessionRequest = Factory.Put(url);

                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);
                jsonWriter.WriteObjectStart();
                if (request.CheckoutSetting != null)
                {
                    jsonWriter.WritePropertyName("checkoutSetting");
                    request.CheckoutSetting.WriteJson(jsonWriter);
                }
                if (request.ContextStack != null)
                {
                    jsonWriter.WritePropertyName("contextStack");
                    jsonWriter.Write(request.ContextStack.ToString());
                }
                jsonWriter.WriteObjectEnd();

                var body = stringBuilder.ToString();
                if (!string.IsNullOrEmpty(body))
                {
                    sessionRequest.Body = body;
                }
                sessionRequest.AddHeader("Content-Type", "application/json");

                if (request.RequestId != null)
                {
                    sessionRequest.AddHeader("X-GS2-REQUEST-ID", request.RequestId);
                }

                AddHeader(
                    Session.Credential,
                    sessionRequest
                );

                return sessionRequest;
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator UpdateCurrentRankingMasterFromGitHub(
                Request.UpdateCurrentRankingMasterFromGitHubRequest request,
                UnityAction<AsyncResult<Result.UpdateCurrentRankingMasterFromGitHubResult>> callback
        )
		{
			var task = new UpdateCurrentRankingMasterFromGitHubTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.UpdateCurrentRankingMasterFromGitHubResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.UpdateCurrentRankingMasterFromGitHubResult> UpdateCurrentRankingMasterFromGitHubAsync(
                Request.UpdateCurrentRankingMasterFromGitHubRequest request
        )
		{
			var task = new UpdateCurrentRankingMasterFromGitHubTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif
	}
}