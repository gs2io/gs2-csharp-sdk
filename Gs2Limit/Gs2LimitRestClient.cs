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
using Gs2.Gs2Limit.Request;
using Gs2.Gs2Limit.Result;
using Gs2.Util.LitJson;

namespace Gs2.Gs2Limit
{
	public class Gs2LimitRestClient : AbstractGs2Client
	{
#if UNITY_2017_1_OR_NEWER
		private readonly CertificateHandler _certificateHandler;
#endif

		public static string Endpoint = "limit";

        protected Gs2RestSession Gs2RestSession => (Gs2RestSession) Gs2Session;

		public Gs2LimitRestClient(Gs2RestSession Gs2RestSession) : base(Gs2RestSession)
		{

		}

#if UNITY_2017_1_OR_NEWER
		public Gs2LimitRestClient(Gs2RestSession gs2RestSession, CertificateHandler certificateHandler) : base(gs2RestSession)
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
                    .Replace("{service}", "limit")
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
                    .Replace("{service}", "limit")
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
                    .Replace("{service}", "limit")
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
                    .Replace("{service}", "limit")
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
                    .Replace("{service}", "limit")
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
                    .Replace("{service}", "limit")
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


        private class DescribeCountersTask : Gs2RestSessionTask<DescribeCountersRequest, DescribeCountersResult>
        {
            public DescribeCountersTask(IGs2Session session, RestSessionRequestFactory factory, DescribeCountersRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DescribeCountersRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "limit")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/me/counter";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");

                var sessionRequest = Factory.Get(url);
                if (request.ContextStack != null)
                {
                    sessionRequest.AddQueryString("contextStack", request.ContextStack);
                }
                if (request.LimitName != null) {
                    sessionRequest.AddQueryString("limitName", $"{request.LimitName}");
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
		public IEnumerator DescribeCounters(
                Request.DescribeCountersRequest request,
                UnityAction<AsyncResult<Result.DescribeCountersResult>> callback
        )
		{
			var task = new DescribeCountersTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.DescribeCountersResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.DescribeCountersResult> DescribeCountersAsync(
                Request.DescribeCountersRequest request
        )
		{
			var task = new DescribeCountersTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        private class DescribeCountersByUserIdTask : Gs2RestSessionTask<DescribeCountersByUserIdRequest, DescribeCountersByUserIdResult>
        {
            public DescribeCountersByUserIdTask(IGs2Session session, RestSessionRequestFactory factory, DescribeCountersByUserIdRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DescribeCountersByUserIdRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "limit")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/{userId}/counter";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{userId}", !string.IsNullOrEmpty(request.UserId) ? request.UserId.ToString() : "null");

                var sessionRequest = Factory.Get(url);
                if (request.ContextStack != null)
                {
                    sessionRequest.AddQueryString("contextStack", request.ContextStack);
                }
                if (request.LimitName != null) {
                    sessionRequest.AddQueryString("limitName", $"{request.LimitName}");
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
		public IEnumerator DescribeCountersByUserId(
                Request.DescribeCountersByUserIdRequest request,
                UnityAction<AsyncResult<Result.DescribeCountersByUserIdResult>> callback
        )
		{
			var task = new DescribeCountersByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.DescribeCountersByUserIdResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.DescribeCountersByUserIdResult> DescribeCountersByUserIdAsync(
                Request.DescribeCountersByUserIdRequest request
        )
		{
			var task = new DescribeCountersByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        private class GetCounterTask : Gs2RestSessionTask<GetCounterRequest, GetCounterResult>
        {
            public GetCounterTask(IGs2Session session, RestSessionRequestFactory factory, GetCounterRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(GetCounterRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "limit")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/me/counter/{limitName}/{counterName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{limitName}", !string.IsNullOrEmpty(request.LimitName) ? request.LimitName.ToString() : "null");
                url = url.Replace("{counterName}", !string.IsNullOrEmpty(request.CounterName) ? request.CounterName.ToString() : "null");

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
		public IEnumerator GetCounter(
                Request.GetCounterRequest request,
                UnityAction<AsyncResult<Result.GetCounterResult>> callback
        )
		{
			var task = new GetCounterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.GetCounterResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.GetCounterResult> GetCounterAsync(
                Request.GetCounterRequest request
        )
		{
			var task = new GetCounterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        private class GetCounterByUserIdTask : Gs2RestSessionTask<GetCounterByUserIdRequest, GetCounterByUserIdResult>
        {
            public GetCounterByUserIdTask(IGs2Session session, RestSessionRequestFactory factory, GetCounterByUserIdRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(GetCounterByUserIdRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "limit")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/{userId}/counter/{limitName}/{counterName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{limitName}", !string.IsNullOrEmpty(request.LimitName) ? request.LimitName.ToString() : "null");
                url = url.Replace("{userId}", !string.IsNullOrEmpty(request.UserId) ? request.UserId.ToString() : "null");
                url = url.Replace("{counterName}", !string.IsNullOrEmpty(request.CounterName) ? request.CounterName.ToString() : "null");

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
		public IEnumerator GetCounterByUserId(
                Request.GetCounterByUserIdRequest request,
                UnityAction<AsyncResult<Result.GetCounterByUserIdResult>> callback
        )
		{
			var task = new GetCounterByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.GetCounterByUserIdResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.GetCounterByUserIdResult> GetCounterByUserIdAsync(
                Request.GetCounterByUserIdRequest request
        )
		{
			var task = new GetCounterByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        private class CountUpTask : Gs2RestSessionTask<CountUpRequest, CountUpResult>
        {
            public CountUpTask(IGs2Session session, RestSessionRequestFactory factory, CountUpRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(CountUpRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "limit")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/me/counter/{limitName}/{counterName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{limitName}", !string.IsNullOrEmpty(request.LimitName) ? request.LimitName.ToString() : "null");
                url = url.Replace("{counterName}", !string.IsNullOrEmpty(request.CounterName) ? request.CounterName.ToString() : "null");

                var sessionRequest = Factory.Post(url);

                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);
                jsonWriter.WriteObjectStart();
                if (request.CountUpValue != null)
                {
                    jsonWriter.WritePropertyName("countUpValue");
                    jsonWriter.Write(request.CountUpValue.ToString());
                }
                if (request.MaxValue != null)
                {
                    jsonWriter.WritePropertyName("maxValue");
                    jsonWriter.Write(request.MaxValue.ToString());
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
		public IEnumerator CountUp(
                Request.CountUpRequest request,
                UnityAction<AsyncResult<Result.CountUpResult>> callback
        )
		{
			var task = new CountUpTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.CountUpResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.CountUpResult> CountUpAsync(
                Request.CountUpRequest request
        )
		{
			var task = new CountUpTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        private class CountUpByUserIdTask : Gs2RestSessionTask<CountUpByUserIdRequest, CountUpByUserIdResult>
        {
            public CountUpByUserIdTask(IGs2Session session, RestSessionRequestFactory factory, CountUpByUserIdRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(CountUpByUserIdRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "limit")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/{userId}/counter/{limitName}/{counterName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{limitName}", !string.IsNullOrEmpty(request.LimitName) ? request.LimitName.ToString() : "null");
                url = url.Replace("{counterName}", !string.IsNullOrEmpty(request.CounterName) ? request.CounterName.ToString() : "null");
                url = url.Replace("{userId}", !string.IsNullOrEmpty(request.UserId) ? request.UserId.ToString() : "null");

                var sessionRequest = Factory.Post(url);

                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);
                jsonWriter.WriteObjectStart();
                if (request.CountUpValue != null)
                {
                    jsonWriter.WritePropertyName("countUpValue");
                    jsonWriter.Write(request.CountUpValue.ToString());
                }
                if (request.MaxValue != null)
                {
                    jsonWriter.WritePropertyName("maxValue");
                    jsonWriter.Write(request.MaxValue.ToString());
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
		public IEnumerator CountUpByUserId(
                Request.CountUpByUserIdRequest request,
                UnityAction<AsyncResult<Result.CountUpByUserIdResult>> callback
        )
		{
			var task = new CountUpByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.CountUpByUserIdResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.CountUpByUserIdResult> CountUpByUserIdAsync(
                Request.CountUpByUserIdRequest request
        )
		{
			var task = new CountUpByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        private class DeleteCounterByUserIdTask : Gs2RestSessionTask<DeleteCounterByUserIdRequest, DeleteCounterByUserIdResult>
        {
            public DeleteCounterByUserIdTask(IGs2Session session, RestSessionRequestFactory factory, DeleteCounterByUserIdRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DeleteCounterByUserIdRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "limit")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/{userId}/counter/{limitName}/{counterName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{limitName}", !string.IsNullOrEmpty(request.LimitName) ? request.LimitName.ToString() : "null");
                url = url.Replace("{userId}", !string.IsNullOrEmpty(request.UserId) ? request.UserId.ToString() : "null");
                url = url.Replace("{counterName}", !string.IsNullOrEmpty(request.CounterName) ? request.CounterName.ToString() : "null");

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
		public IEnumerator DeleteCounterByUserId(
                Request.DeleteCounterByUserIdRequest request,
                UnityAction<AsyncResult<Result.DeleteCounterByUserIdResult>> callback
        )
		{
			var task = new DeleteCounterByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.DeleteCounterByUserIdResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.DeleteCounterByUserIdResult> DeleteCounterByUserIdAsync(
                Request.DeleteCounterByUserIdRequest request
        )
		{
			var task = new DeleteCounterByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        private class CountUpByStampTaskTask : Gs2RestSessionTask<CountUpByStampTaskRequest, CountUpByStampTaskResult>
        {
            public CountUpByStampTaskTask(IGs2Session session, RestSessionRequestFactory factory, CountUpByStampTaskRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(CountUpByStampTaskRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "limit")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/stamp/counter/increase";

                var sessionRequest = Factory.Post(url);

                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);
                jsonWriter.WriteObjectStart();
                if (request.StampTask != null)
                {
                    jsonWriter.WritePropertyName("stampTask");
                    jsonWriter.Write(request.StampTask);
                }
                if (request.KeyId != null)
                {
                    jsonWriter.WritePropertyName("keyId");
                    jsonWriter.Write(request.KeyId);
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
		public IEnumerator CountUpByStampTask(
                Request.CountUpByStampTaskRequest request,
                UnityAction<AsyncResult<Result.CountUpByStampTaskResult>> callback
        )
		{
			var task = new CountUpByStampTaskTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.CountUpByStampTaskResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.CountUpByStampTaskResult> CountUpByStampTaskAsync(
                Request.CountUpByStampTaskRequest request
        )
		{
			var task = new CountUpByStampTaskTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        private class DeleteByStampSheetTask : Gs2RestSessionTask<DeleteByStampSheetRequest, DeleteByStampSheetResult>
        {
            public DeleteByStampSheetTask(IGs2Session session, RestSessionRequestFactory factory, DeleteByStampSheetRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DeleteByStampSheetRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "limit")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/stamp/counter/delete";

                var sessionRequest = Factory.Post(url);

                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);
                jsonWriter.WriteObjectStart();
                if (request.StampSheet != null)
                {
                    jsonWriter.WritePropertyName("stampSheet");
                    jsonWriter.Write(request.StampSheet);
                }
                if (request.KeyId != null)
                {
                    jsonWriter.WritePropertyName("keyId");
                    jsonWriter.Write(request.KeyId);
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
		public IEnumerator DeleteByStampSheet(
                Request.DeleteByStampSheetRequest request,
                UnityAction<AsyncResult<Result.DeleteByStampSheetResult>> callback
        )
		{
			var task = new DeleteByStampSheetTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.DeleteByStampSheetResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.DeleteByStampSheetResult> DeleteByStampSheetAsync(
                Request.DeleteByStampSheetRequest request
        )
		{
			var task = new DeleteByStampSheetTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        private class DescribeLimitModelMastersTask : Gs2RestSessionTask<DescribeLimitModelMastersRequest, DescribeLimitModelMastersResult>
        {
            public DescribeLimitModelMastersTask(IGs2Session session, RestSessionRequestFactory factory, DescribeLimitModelMastersRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DescribeLimitModelMastersRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "limit")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/master/limit";

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
		public IEnumerator DescribeLimitModelMasters(
                Request.DescribeLimitModelMastersRequest request,
                UnityAction<AsyncResult<Result.DescribeLimitModelMastersResult>> callback
        )
		{
			var task = new DescribeLimitModelMastersTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.DescribeLimitModelMastersResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.DescribeLimitModelMastersResult> DescribeLimitModelMastersAsync(
                Request.DescribeLimitModelMastersRequest request
        )
		{
			var task = new DescribeLimitModelMastersTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        private class CreateLimitModelMasterTask : Gs2RestSessionTask<CreateLimitModelMasterRequest, CreateLimitModelMasterResult>
        {
            public CreateLimitModelMasterTask(IGs2Session session, RestSessionRequestFactory factory, CreateLimitModelMasterRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(CreateLimitModelMasterRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "limit")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/master/limit";

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
                if (request.ResetType != null)
                {
                    jsonWriter.WritePropertyName("resetType");
                    jsonWriter.Write(request.ResetType);
                }
                if (request.ResetDayOfMonth != null)
                {
                    jsonWriter.WritePropertyName("resetDayOfMonth");
                    jsonWriter.Write(request.ResetDayOfMonth.ToString());
                }
                if (request.ResetDayOfWeek != null)
                {
                    jsonWriter.WritePropertyName("resetDayOfWeek");
                    jsonWriter.Write(request.ResetDayOfWeek);
                }
                if (request.ResetHour != null)
                {
                    jsonWriter.WritePropertyName("resetHour");
                    jsonWriter.Write(request.ResetHour.ToString());
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
		public IEnumerator CreateLimitModelMaster(
                Request.CreateLimitModelMasterRequest request,
                UnityAction<AsyncResult<Result.CreateLimitModelMasterResult>> callback
        )
		{
			var task = new CreateLimitModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.CreateLimitModelMasterResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.CreateLimitModelMasterResult> CreateLimitModelMasterAsync(
                Request.CreateLimitModelMasterRequest request
        )
		{
			var task = new CreateLimitModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        private class GetLimitModelMasterTask : Gs2RestSessionTask<GetLimitModelMasterRequest, GetLimitModelMasterResult>
        {
            public GetLimitModelMasterTask(IGs2Session session, RestSessionRequestFactory factory, GetLimitModelMasterRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(GetLimitModelMasterRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "limit")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/master/limit/{limitName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{limitName}", !string.IsNullOrEmpty(request.LimitName) ? request.LimitName.ToString() : "null");

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
		public IEnumerator GetLimitModelMaster(
                Request.GetLimitModelMasterRequest request,
                UnityAction<AsyncResult<Result.GetLimitModelMasterResult>> callback
        )
		{
			var task = new GetLimitModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.GetLimitModelMasterResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.GetLimitModelMasterResult> GetLimitModelMasterAsync(
                Request.GetLimitModelMasterRequest request
        )
		{
			var task = new GetLimitModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        private class UpdateLimitModelMasterTask : Gs2RestSessionTask<UpdateLimitModelMasterRequest, UpdateLimitModelMasterResult>
        {
            public UpdateLimitModelMasterTask(IGs2Session session, RestSessionRequestFactory factory, UpdateLimitModelMasterRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(UpdateLimitModelMasterRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "limit")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/master/limit/{limitName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{limitName}", !string.IsNullOrEmpty(request.LimitName) ? request.LimitName.ToString() : "null");

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
                if (request.ResetType != null)
                {
                    jsonWriter.WritePropertyName("resetType");
                    jsonWriter.Write(request.ResetType);
                }
                if (request.ResetDayOfMonth != null)
                {
                    jsonWriter.WritePropertyName("resetDayOfMonth");
                    jsonWriter.Write(request.ResetDayOfMonth.ToString());
                }
                if (request.ResetDayOfWeek != null)
                {
                    jsonWriter.WritePropertyName("resetDayOfWeek");
                    jsonWriter.Write(request.ResetDayOfWeek);
                }
                if (request.ResetHour != null)
                {
                    jsonWriter.WritePropertyName("resetHour");
                    jsonWriter.Write(request.ResetHour.ToString());
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
		public IEnumerator UpdateLimitModelMaster(
                Request.UpdateLimitModelMasterRequest request,
                UnityAction<AsyncResult<Result.UpdateLimitModelMasterResult>> callback
        )
		{
			var task = new UpdateLimitModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.UpdateLimitModelMasterResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.UpdateLimitModelMasterResult> UpdateLimitModelMasterAsync(
                Request.UpdateLimitModelMasterRequest request
        )
		{
			var task = new UpdateLimitModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        private class DeleteLimitModelMasterTask : Gs2RestSessionTask<DeleteLimitModelMasterRequest, DeleteLimitModelMasterResult>
        {
            public DeleteLimitModelMasterTask(IGs2Session session, RestSessionRequestFactory factory, DeleteLimitModelMasterRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DeleteLimitModelMasterRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "limit")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/master/limit/{limitName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{limitName}", !string.IsNullOrEmpty(request.LimitName) ? request.LimitName.ToString() : "null");

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
		public IEnumerator DeleteLimitModelMaster(
                Request.DeleteLimitModelMasterRequest request,
                UnityAction<AsyncResult<Result.DeleteLimitModelMasterResult>> callback
        )
		{
			var task = new DeleteLimitModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.DeleteLimitModelMasterResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.DeleteLimitModelMasterResult> DeleteLimitModelMasterAsync(
                Request.DeleteLimitModelMasterRequest request
        )
		{
			var task = new DeleteLimitModelMasterTask(
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
                    .Replace("{service}", "limit")
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


        private class GetCurrentLimitMasterTask : Gs2RestSessionTask<GetCurrentLimitMasterRequest, GetCurrentLimitMasterResult>
        {
            public GetCurrentLimitMasterTask(IGs2Session session, RestSessionRequestFactory factory, GetCurrentLimitMasterRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(GetCurrentLimitMasterRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "limit")
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
		public IEnumerator GetCurrentLimitMaster(
                Request.GetCurrentLimitMasterRequest request,
                UnityAction<AsyncResult<Result.GetCurrentLimitMasterResult>> callback
        )
		{
			var task = new GetCurrentLimitMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.GetCurrentLimitMasterResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.GetCurrentLimitMasterResult> GetCurrentLimitMasterAsync(
                Request.GetCurrentLimitMasterRequest request
        )
		{
			var task = new GetCurrentLimitMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        private class UpdateCurrentLimitMasterTask : Gs2RestSessionTask<UpdateCurrentLimitMasterRequest, UpdateCurrentLimitMasterResult>
        {
            public UpdateCurrentLimitMasterTask(IGs2Session session, RestSessionRequestFactory factory, UpdateCurrentLimitMasterRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(UpdateCurrentLimitMasterRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "limit")
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
		public IEnumerator UpdateCurrentLimitMaster(
                Request.UpdateCurrentLimitMasterRequest request,
                UnityAction<AsyncResult<Result.UpdateCurrentLimitMasterResult>> callback
        )
		{
			var task = new UpdateCurrentLimitMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.UpdateCurrentLimitMasterResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.UpdateCurrentLimitMasterResult> UpdateCurrentLimitMasterAsync(
                Request.UpdateCurrentLimitMasterRequest request
        )
		{
			var task = new UpdateCurrentLimitMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        private class UpdateCurrentLimitMasterFromGitHubTask : Gs2RestSessionTask<UpdateCurrentLimitMasterFromGitHubRequest, UpdateCurrentLimitMasterFromGitHubResult>
        {
            public UpdateCurrentLimitMasterFromGitHubTask(IGs2Session session, RestSessionRequestFactory factory, UpdateCurrentLimitMasterFromGitHubRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(UpdateCurrentLimitMasterFromGitHubRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "limit")
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
		public IEnumerator UpdateCurrentLimitMasterFromGitHub(
                Request.UpdateCurrentLimitMasterFromGitHubRequest request,
                UnityAction<AsyncResult<Result.UpdateCurrentLimitMasterFromGitHubResult>> callback
        )
		{
			var task = new UpdateCurrentLimitMasterFromGitHubTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.UpdateCurrentLimitMasterFromGitHubResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.UpdateCurrentLimitMasterFromGitHubResult> UpdateCurrentLimitMasterFromGitHubAsync(
                Request.UpdateCurrentLimitMasterFromGitHubRequest request
        )
		{
			var task = new UpdateCurrentLimitMasterFromGitHubTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        private class DescribeLimitModelsTask : Gs2RestSessionTask<DescribeLimitModelsRequest, DescribeLimitModelsResult>
        {
            public DescribeLimitModelsTask(IGs2Session session, RestSessionRequestFactory factory, DescribeLimitModelsRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DescribeLimitModelsRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "limit")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/limit";

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
		public IEnumerator DescribeLimitModels(
                Request.DescribeLimitModelsRequest request,
                UnityAction<AsyncResult<Result.DescribeLimitModelsResult>> callback
        )
		{
			var task = new DescribeLimitModelsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.DescribeLimitModelsResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.DescribeLimitModelsResult> DescribeLimitModelsAsync(
                Request.DescribeLimitModelsRequest request
        )
		{
			var task = new DescribeLimitModelsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        private class GetLimitModelTask : Gs2RestSessionTask<GetLimitModelRequest, GetLimitModelResult>
        {
            public GetLimitModelTask(IGs2Session session, RestSessionRequestFactory factory, GetLimitModelRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(GetLimitModelRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "limit")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/limit/{limitName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{limitName}", !string.IsNullOrEmpty(request.LimitName) ? request.LimitName.ToString() : "null");

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
		public IEnumerator GetLimitModel(
                Request.GetLimitModelRequest request,
                UnityAction<AsyncResult<Result.GetLimitModelResult>> callback
        )
		{
			var task = new GetLimitModelTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.GetLimitModelResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.GetLimitModelResult> GetLimitModelAsync(
                Request.GetLimitModelRequest request
        )
		{
			var task = new GetLimitModelTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif
	}
}