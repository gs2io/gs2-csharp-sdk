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
using Gs2.Gs2Exchange.Request;
using Gs2.Gs2Exchange.Result;
using Gs2.Util.LitJson;

namespace Gs2.Gs2Exchange
{
	public class Gs2ExchangeRestClient : AbstractGs2Client
	{
#if UNITY_2017_1_OR_NEWER
		private readonly CertificateHandler _certificateHandler;
#endif

		public static string Endpoint = "exchange";

        protected Gs2RestSession Gs2RestSession => (Gs2RestSession) Gs2Session;

		public Gs2ExchangeRestClient(Gs2RestSession Gs2RestSession) : base(Gs2RestSession)
		{

		}

#if UNITY_2017_1_OR_NEWER
		public Gs2ExchangeRestClient(Gs2RestSession gs2RestSession, CertificateHandler certificateHandler) : base(gs2RestSession)
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
                    .Replace("{service}", "exchange")
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
                    .Replace("{service}", "exchange")
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
                if (request.EnableAwaitExchange != null)
                {
                    jsonWriter.WritePropertyName("enableAwaitExchange");
                    jsonWriter.Write(request.EnableAwaitExchange.ToString());
                }
                if (request.EnableDirectExchange != null)
                {
                    jsonWriter.WritePropertyName("enableDirectExchange");
                    jsonWriter.Write(request.EnableDirectExchange.ToString());
                }
                if (request.QueueNamespaceId != null)
                {
                    jsonWriter.WritePropertyName("queueNamespaceId");
                    jsonWriter.Write(request.QueueNamespaceId);
                }
                if (request.KeyId != null)
                {
                    jsonWriter.WritePropertyName("keyId");
                    jsonWriter.Write(request.KeyId);
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
                    .Replace("{service}", "exchange")
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
                    .Replace("{service}", "exchange")
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
                    .Replace("{service}", "exchange")
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
                if (request.EnableAwaitExchange != null)
                {
                    jsonWriter.WritePropertyName("enableAwaitExchange");
                    jsonWriter.Write(request.EnableAwaitExchange.ToString());
                }
                if (request.EnableDirectExchange != null)
                {
                    jsonWriter.WritePropertyName("enableDirectExchange");
                    jsonWriter.Write(request.EnableDirectExchange.ToString());
                }
                if (request.QueueNamespaceId != null)
                {
                    jsonWriter.WritePropertyName("queueNamespaceId");
                    jsonWriter.Write(request.QueueNamespaceId);
                }
                if (request.KeyId != null)
                {
                    jsonWriter.WritePropertyName("keyId");
                    jsonWriter.Write(request.KeyId);
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
                    .Replace("{service}", "exchange")
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


        private class DescribeRateModelsTask : Gs2RestSessionTask<DescribeRateModelsRequest, DescribeRateModelsResult>
        {
            public DescribeRateModelsTask(IGs2Session session, RestSessionRequestFactory factory, DescribeRateModelsRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DescribeRateModelsRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "exchange")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/model";

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
		public IEnumerator DescribeRateModels(
                Request.DescribeRateModelsRequest request,
                UnityAction<AsyncResult<Result.DescribeRateModelsResult>> callback
        )
		{
			var task = new DescribeRateModelsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.DescribeRateModelsResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.DescribeRateModelsResult> DescribeRateModelsAsync(
                Request.DescribeRateModelsRequest request
        )
		{
			var task = new DescribeRateModelsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        private class GetRateModelTask : Gs2RestSessionTask<GetRateModelRequest, GetRateModelResult>
        {
            public GetRateModelTask(IGs2Session session, RestSessionRequestFactory factory, GetRateModelRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(GetRateModelRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "exchange")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/model/{rateName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{rateName}", !string.IsNullOrEmpty(request.RateName) ? request.RateName.ToString() : "null");

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
		public IEnumerator GetRateModel(
                Request.GetRateModelRequest request,
                UnityAction<AsyncResult<Result.GetRateModelResult>> callback
        )
		{
			var task = new GetRateModelTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.GetRateModelResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.GetRateModelResult> GetRateModelAsync(
                Request.GetRateModelRequest request
        )
		{
			var task = new GetRateModelTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        private class DescribeRateModelMastersTask : Gs2RestSessionTask<DescribeRateModelMastersRequest, DescribeRateModelMastersResult>
        {
            public DescribeRateModelMastersTask(IGs2Session session, RestSessionRequestFactory factory, DescribeRateModelMastersRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DescribeRateModelMastersRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "exchange")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/master/model";

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
		public IEnumerator DescribeRateModelMasters(
                Request.DescribeRateModelMastersRequest request,
                UnityAction<AsyncResult<Result.DescribeRateModelMastersResult>> callback
        )
		{
			var task = new DescribeRateModelMastersTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.DescribeRateModelMastersResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.DescribeRateModelMastersResult> DescribeRateModelMastersAsync(
                Request.DescribeRateModelMastersRequest request
        )
		{
			var task = new DescribeRateModelMastersTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        private class CreateRateModelMasterTask : Gs2RestSessionTask<CreateRateModelMasterRequest, CreateRateModelMasterResult>
        {
            public CreateRateModelMasterTask(IGs2Session session, RestSessionRequestFactory factory, CreateRateModelMasterRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(CreateRateModelMasterRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "exchange")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/master/model";

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
                if (request.TimingType != null)
                {
                    jsonWriter.WritePropertyName("timingType");
                    jsonWriter.Write(request.TimingType);
                }
                if (request.LockTime != null)
                {
                    jsonWriter.WritePropertyName("lockTime");
                    jsonWriter.Write(request.LockTime.ToString());
                }
                if (request.EnableSkip != null)
                {
                    jsonWriter.WritePropertyName("enableSkip");
                    jsonWriter.Write(request.EnableSkip.ToString());
                }
                if (request.SkipConsumeActions != null)
                {
                    jsonWriter.WritePropertyName("skipConsumeActions");
                    jsonWriter.WriteArrayStart();
                    foreach(var item in request.SkipConsumeActions)
                    {
                        item.WriteJson(jsonWriter);
                    }
                    jsonWriter.WriteArrayEnd();
                }
                if (request.AcquireActions != null)
                {
                    jsonWriter.WritePropertyName("acquireActions");
                    jsonWriter.WriteArrayStart();
                    foreach(var item in request.AcquireActions)
                    {
                        item.WriteJson(jsonWriter);
                    }
                    jsonWriter.WriteArrayEnd();
                }
                if (request.ConsumeActions != null)
                {
                    jsonWriter.WritePropertyName("consumeActions");
                    jsonWriter.WriteArrayStart();
                    foreach(var item in request.ConsumeActions)
                    {
                        item.WriteJson(jsonWriter);
                    }
                    jsonWriter.WriteArrayEnd();
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
		public IEnumerator CreateRateModelMaster(
                Request.CreateRateModelMasterRequest request,
                UnityAction<AsyncResult<Result.CreateRateModelMasterResult>> callback
        )
		{
			var task = new CreateRateModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.CreateRateModelMasterResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.CreateRateModelMasterResult> CreateRateModelMasterAsync(
                Request.CreateRateModelMasterRequest request
        )
		{
			var task = new CreateRateModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        private class GetRateModelMasterTask : Gs2RestSessionTask<GetRateModelMasterRequest, GetRateModelMasterResult>
        {
            public GetRateModelMasterTask(IGs2Session session, RestSessionRequestFactory factory, GetRateModelMasterRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(GetRateModelMasterRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "exchange")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/master/model/{rateName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{rateName}", !string.IsNullOrEmpty(request.RateName) ? request.RateName.ToString() : "null");

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
		public IEnumerator GetRateModelMaster(
                Request.GetRateModelMasterRequest request,
                UnityAction<AsyncResult<Result.GetRateModelMasterResult>> callback
        )
		{
			var task = new GetRateModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.GetRateModelMasterResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.GetRateModelMasterResult> GetRateModelMasterAsync(
                Request.GetRateModelMasterRequest request
        )
		{
			var task = new GetRateModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        private class UpdateRateModelMasterTask : Gs2RestSessionTask<UpdateRateModelMasterRequest, UpdateRateModelMasterResult>
        {
            public UpdateRateModelMasterTask(IGs2Session session, RestSessionRequestFactory factory, UpdateRateModelMasterRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(UpdateRateModelMasterRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "exchange")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/master/model/{rateName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{rateName}", !string.IsNullOrEmpty(request.RateName) ? request.RateName.ToString() : "null");

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
                if (request.TimingType != null)
                {
                    jsonWriter.WritePropertyName("timingType");
                    jsonWriter.Write(request.TimingType);
                }
                if (request.LockTime != null)
                {
                    jsonWriter.WritePropertyName("lockTime");
                    jsonWriter.Write(request.LockTime.ToString());
                }
                if (request.EnableSkip != null)
                {
                    jsonWriter.WritePropertyName("enableSkip");
                    jsonWriter.Write(request.EnableSkip.ToString());
                }
                if (request.SkipConsumeActions != null)
                {
                    jsonWriter.WritePropertyName("skipConsumeActions");
                    jsonWriter.WriteArrayStart();
                    foreach(var item in request.SkipConsumeActions)
                    {
                        item.WriteJson(jsonWriter);
                    }
                    jsonWriter.WriteArrayEnd();
                }
                if (request.AcquireActions != null)
                {
                    jsonWriter.WritePropertyName("acquireActions");
                    jsonWriter.WriteArrayStart();
                    foreach(var item in request.AcquireActions)
                    {
                        item.WriteJson(jsonWriter);
                    }
                    jsonWriter.WriteArrayEnd();
                }
                if (request.ConsumeActions != null)
                {
                    jsonWriter.WritePropertyName("consumeActions");
                    jsonWriter.WriteArrayStart();
                    foreach(var item in request.ConsumeActions)
                    {
                        item.WriteJson(jsonWriter);
                    }
                    jsonWriter.WriteArrayEnd();
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
		public IEnumerator UpdateRateModelMaster(
                Request.UpdateRateModelMasterRequest request,
                UnityAction<AsyncResult<Result.UpdateRateModelMasterResult>> callback
        )
		{
			var task = new UpdateRateModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.UpdateRateModelMasterResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.UpdateRateModelMasterResult> UpdateRateModelMasterAsync(
                Request.UpdateRateModelMasterRequest request
        )
		{
			var task = new UpdateRateModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        private class DeleteRateModelMasterTask : Gs2RestSessionTask<DeleteRateModelMasterRequest, DeleteRateModelMasterResult>
        {
            public DeleteRateModelMasterTask(IGs2Session session, RestSessionRequestFactory factory, DeleteRateModelMasterRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DeleteRateModelMasterRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "exchange")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/master/model/{rateName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{rateName}", !string.IsNullOrEmpty(request.RateName) ? request.RateName.ToString() : "null");

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
		public IEnumerator DeleteRateModelMaster(
                Request.DeleteRateModelMasterRequest request,
                UnityAction<AsyncResult<Result.DeleteRateModelMasterResult>> callback
        )
		{
			var task = new DeleteRateModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.DeleteRateModelMasterResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.DeleteRateModelMasterResult> DeleteRateModelMasterAsync(
                Request.DeleteRateModelMasterRequest request
        )
		{
			var task = new DeleteRateModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        private class ExchangeTask : Gs2RestSessionTask<ExchangeRequest, ExchangeResult>
        {
            public ExchangeTask(IGs2Session session, RestSessionRequestFactory factory, ExchangeRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(ExchangeRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "exchange")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/me/exchange/{rateName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{rateName}", !string.IsNullOrEmpty(request.RateName) ? request.RateName.ToString() : "null");

                var sessionRequest = Factory.Post(url);

                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);
                jsonWriter.WriteObjectStart();
                if (request.Count != null)
                {
                    jsonWriter.WritePropertyName("count");
                    jsonWriter.Write(request.Count.ToString());
                }
                if (request.Config != null)
                {
                    jsonWriter.WritePropertyName("config");
                    jsonWriter.WriteArrayStart();
                    foreach(var item in request.Config)
                    {
                        item.WriteJson(jsonWriter);
                    }
                    jsonWriter.WriteArrayEnd();
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
		public IEnumerator Exchange(
                Request.ExchangeRequest request,
                UnityAction<AsyncResult<Result.ExchangeResult>> callback
        )
		{
			var task = new ExchangeTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.ExchangeResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.ExchangeResult> ExchangeAsync(
                Request.ExchangeRequest request
        )
		{
			var task = new ExchangeTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        private class ExchangeByUserIdTask : Gs2RestSessionTask<ExchangeByUserIdRequest, ExchangeByUserIdResult>
        {
            public ExchangeByUserIdTask(IGs2Session session, RestSessionRequestFactory factory, ExchangeByUserIdRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(ExchangeByUserIdRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "exchange")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/{userId}/exchange/{rateName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{rateName}", !string.IsNullOrEmpty(request.RateName) ? request.RateName.ToString() : "null");
                url = url.Replace("{userId}", !string.IsNullOrEmpty(request.UserId) ? request.UserId.ToString() : "null");

                var sessionRequest = Factory.Post(url);

                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);
                jsonWriter.WriteObjectStart();
                if (request.Count != null)
                {
                    jsonWriter.WritePropertyName("count");
                    jsonWriter.Write(request.Count.ToString());
                }
                if (request.Config != null)
                {
                    jsonWriter.WritePropertyName("config");
                    jsonWriter.WriteArrayStart();
                    foreach(var item in request.Config)
                    {
                        item.WriteJson(jsonWriter);
                    }
                    jsonWriter.WriteArrayEnd();
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
		public IEnumerator ExchangeByUserId(
                Request.ExchangeByUserIdRequest request,
                UnityAction<AsyncResult<Result.ExchangeByUserIdResult>> callback
        )
		{
			var task = new ExchangeByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.ExchangeByUserIdResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.ExchangeByUserIdResult> ExchangeByUserIdAsync(
                Request.ExchangeByUserIdRequest request
        )
		{
			var task = new ExchangeByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        private class ExchangeByStampSheetTask : Gs2RestSessionTask<ExchangeByStampSheetRequest, ExchangeByStampSheetResult>
        {
            public ExchangeByStampSheetTask(IGs2Session session, RestSessionRequestFactory factory, ExchangeByStampSheetRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(ExchangeByStampSheetRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "exchange")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/stamp/exchange";

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
		public IEnumerator ExchangeByStampSheet(
                Request.ExchangeByStampSheetRequest request,
                UnityAction<AsyncResult<Result.ExchangeByStampSheetResult>> callback
        )
		{
			var task = new ExchangeByStampSheetTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.ExchangeByStampSheetResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.ExchangeByStampSheetResult> ExchangeByStampSheetAsync(
                Request.ExchangeByStampSheetRequest request
        )
		{
			var task = new ExchangeByStampSheetTask(
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
                    .Replace("{service}", "exchange")
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


        private class GetCurrentRateMasterTask : Gs2RestSessionTask<GetCurrentRateMasterRequest, GetCurrentRateMasterResult>
        {
            public GetCurrentRateMasterTask(IGs2Session session, RestSessionRequestFactory factory, GetCurrentRateMasterRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(GetCurrentRateMasterRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "exchange")
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
		public IEnumerator GetCurrentRateMaster(
                Request.GetCurrentRateMasterRequest request,
                UnityAction<AsyncResult<Result.GetCurrentRateMasterResult>> callback
        )
		{
			var task = new GetCurrentRateMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.GetCurrentRateMasterResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.GetCurrentRateMasterResult> GetCurrentRateMasterAsync(
                Request.GetCurrentRateMasterRequest request
        )
		{
			var task = new GetCurrentRateMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        private class UpdateCurrentRateMasterTask : Gs2RestSessionTask<UpdateCurrentRateMasterRequest, UpdateCurrentRateMasterResult>
        {
            public UpdateCurrentRateMasterTask(IGs2Session session, RestSessionRequestFactory factory, UpdateCurrentRateMasterRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(UpdateCurrentRateMasterRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "exchange")
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
		public IEnumerator UpdateCurrentRateMaster(
                Request.UpdateCurrentRateMasterRequest request,
                UnityAction<AsyncResult<Result.UpdateCurrentRateMasterResult>> callback
        )
		{
			var task = new UpdateCurrentRateMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.UpdateCurrentRateMasterResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.UpdateCurrentRateMasterResult> UpdateCurrentRateMasterAsync(
                Request.UpdateCurrentRateMasterRequest request
        )
		{
			var task = new UpdateCurrentRateMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        private class UpdateCurrentRateMasterFromGitHubTask : Gs2RestSessionTask<UpdateCurrentRateMasterFromGitHubRequest, UpdateCurrentRateMasterFromGitHubResult>
        {
            public UpdateCurrentRateMasterFromGitHubTask(IGs2Session session, RestSessionRequestFactory factory, UpdateCurrentRateMasterFromGitHubRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(UpdateCurrentRateMasterFromGitHubRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "exchange")
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
		public IEnumerator UpdateCurrentRateMasterFromGitHub(
                Request.UpdateCurrentRateMasterFromGitHubRequest request,
                UnityAction<AsyncResult<Result.UpdateCurrentRateMasterFromGitHubResult>> callback
        )
		{
			var task = new UpdateCurrentRateMasterFromGitHubTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.UpdateCurrentRateMasterFromGitHubResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.UpdateCurrentRateMasterFromGitHubResult> UpdateCurrentRateMasterFromGitHubAsync(
                Request.UpdateCurrentRateMasterFromGitHubRequest request
        )
		{
			var task = new UpdateCurrentRateMasterFromGitHubTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        private class CreateAwaitByUserIdTask : Gs2RestSessionTask<CreateAwaitByUserIdRequest, CreateAwaitByUserIdResult>
        {
            public CreateAwaitByUserIdTask(IGs2Session session, RestSessionRequestFactory factory, CreateAwaitByUserIdRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(CreateAwaitByUserIdRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "exchange")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/{userId}/exchange/{rateName}/await";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{userId}", !string.IsNullOrEmpty(request.UserId) ? request.UserId.ToString() : "null");
                url = url.Replace("{rateName}", !string.IsNullOrEmpty(request.RateName) ? request.RateName.ToString() : "null");

                var sessionRequest = Factory.Put(url);

                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);
                jsonWriter.WriteObjectStart();
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
		public IEnumerator CreateAwaitByUserId(
                Request.CreateAwaitByUserIdRequest request,
                UnityAction<AsyncResult<Result.CreateAwaitByUserIdResult>> callback
        )
		{
			var task = new CreateAwaitByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.CreateAwaitByUserIdResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.CreateAwaitByUserIdResult> CreateAwaitByUserIdAsync(
                Request.CreateAwaitByUserIdRequest request
        )
		{
			var task = new CreateAwaitByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        private class DescribeAwaitsTask : Gs2RestSessionTask<DescribeAwaitsRequest, DescribeAwaitsResult>
        {
            public DescribeAwaitsTask(IGs2Session session, RestSessionRequestFactory factory, DescribeAwaitsRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DescribeAwaitsRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "exchange")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/me/exchange/await";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");

                var sessionRequest = Factory.Get(url);
                if (request.ContextStack != null)
                {
                    sessionRequest.AddQueryString("contextStack", request.ContextStack);
                }
                if (request.RateName != null) {
                    sessionRequest.AddQueryString("rateName", $"{request.RateName}");
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
		public IEnumerator DescribeAwaits(
                Request.DescribeAwaitsRequest request,
                UnityAction<AsyncResult<Result.DescribeAwaitsResult>> callback
        )
		{
			var task = new DescribeAwaitsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.DescribeAwaitsResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.DescribeAwaitsResult> DescribeAwaitsAsync(
                Request.DescribeAwaitsRequest request
        )
		{
			var task = new DescribeAwaitsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        private class DescribeAwaitsByUserIdTask : Gs2RestSessionTask<DescribeAwaitsByUserIdRequest, DescribeAwaitsByUserIdResult>
        {
            public DescribeAwaitsByUserIdTask(IGs2Session session, RestSessionRequestFactory factory, DescribeAwaitsByUserIdRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DescribeAwaitsByUserIdRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "exchange")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/{userId}/exchange/await";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{userId}", !string.IsNullOrEmpty(request.UserId) ? request.UserId.ToString() : "null");

                var sessionRequest = Factory.Get(url);
                if (request.ContextStack != null)
                {
                    sessionRequest.AddQueryString("contextStack", request.ContextStack);
                }
                if (request.RateName != null) {
                    sessionRequest.AddQueryString("rateName", $"{request.RateName}");
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
		public IEnumerator DescribeAwaitsByUserId(
                Request.DescribeAwaitsByUserIdRequest request,
                UnityAction<AsyncResult<Result.DescribeAwaitsByUserIdResult>> callback
        )
		{
			var task = new DescribeAwaitsByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.DescribeAwaitsByUserIdResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.DescribeAwaitsByUserIdResult> DescribeAwaitsByUserIdAsync(
                Request.DescribeAwaitsByUserIdRequest request
        )
		{
			var task = new DescribeAwaitsByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        private class GetAwaitTask : Gs2RestSessionTask<GetAwaitRequest, GetAwaitResult>
        {
            public GetAwaitTask(IGs2Session session, RestSessionRequestFactory factory, GetAwaitRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(GetAwaitRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "exchange")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/me/exchange/{rateName}/await/{awaitName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{rateName}", !string.IsNullOrEmpty(request.RateName) ? request.RateName.ToString() : "null");
                url = url.Replace("{awaitName}", !string.IsNullOrEmpty(request.AwaitName) ? request.AwaitName.ToString() : "null");

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
		public IEnumerator GetAwait(
                Request.GetAwaitRequest request,
                UnityAction<AsyncResult<Result.GetAwaitResult>> callback
        )
		{
			var task = new GetAwaitTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.GetAwaitResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.GetAwaitResult> GetAwaitAsync(
                Request.GetAwaitRequest request
        )
		{
			var task = new GetAwaitTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        private class GetAwaitByUserIdTask : Gs2RestSessionTask<GetAwaitByUserIdRequest, GetAwaitByUserIdResult>
        {
            public GetAwaitByUserIdTask(IGs2Session session, RestSessionRequestFactory factory, GetAwaitByUserIdRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(GetAwaitByUserIdRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "exchange")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/{userId}/exchange/{rateName}/await/{awaitName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{userId}", !string.IsNullOrEmpty(request.UserId) ? request.UserId.ToString() : "null");
                url = url.Replace("{rateName}", !string.IsNullOrEmpty(request.RateName) ? request.RateName.ToString() : "null");
                url = url.Replace("{awaitName}", !string.IsNullOrEmpty(request.AwaitName) ? request.AwaitName.ToString() : "null");

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
		public IEnumerator GetAwaitByUserId(
                Request.GetAwaitByUserIdRequest request,
                UnityAction<AsyncResult<Result.GetAwaitByUserIdResult>> callback
        )
		{
			var task = new GetAwaitByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.GetAwaitByUserIdResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.GetAwaitByUserIdResult> GetAwaitByUserIdAsync(
                Request.GetAwaitByUserIdRequest request
        )
		{
			var task = new GetAwaitByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        private class AcquireTask : Gs2RestSessionTask<AcquireRequest, AcquireResult>
        {
            public AcquireTask(IGs2Session session, RestSessionRequestFactory factory, AcquireRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(AcquireRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "exchange")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/me/exchange/{rateName}/await/{awaitName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{rateName}", !string.IsNullOrEmpty(request.RateName) ? request.RateName.ToString() : "null");
                url = url.Replace("{awaitName}", !string.IsNullOrEmpty(request.AwaitName) ? request.AwaitName.ToString() : "null");

                var sessionRequest = Factory.Post(url);

                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);
                jsonWriter.WriteObjectStart();
                if (request.Config != null)
                {
                    jsonWriter.WritePropertyName("config");
                    jsonWriter.WriteArrayStart();
                    foreach(var item in request.Config)
                    {
                        item.WriteJson(jsonWriter);
                    }
                    jsonWriter.WriteArrayEnd();
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
		public IEnumerator Acquire(
                Request.AcquireRequest request,
                UnityAction<AsyncResult<Result.AcquireResult>> callback
        )
		{
			var task = new AcquireTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.AcquireResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.AcquireResult> AcquireAsync(
                Request.AcquireRequest request
        )
		{
			var task = new AcquireTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        private class AcquireByUserIdTask : Gs2RestSessionTask<AcquireByUserIdRequest, AcquireByUserIdResult>
        {
            public AcquireByUserIdTask(IGs2Session session, RestSessionRequestFactory factory, AcquireByUserIdRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(AcquireByUserIdRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "exchange")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/{userId}/exchange/{rateName}/await/{awaitName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{userId}", !string.IsNullOrEmpty(request.UserId) ? request.UserId.ToString() : "null");
                url = url.Replace("{rateName}", !string.IsNullOrEmpty(request.RateName) ? request.RateName.ToString() : "null");
                url = url.Replace("{awaitName}", !string.IsNullOrEmpty(request.AwaitName) ? request.AwaitName.ToString() : "null");

                var sessionRequest = Factory.Post(url);

                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);
                jsonWriter.WriteObjectStart();
                if (request.Config != null)
                {
                    jsonWriter.WritePropertyName("config");
                    jsonWriter.WriteArrayStart();
                    foreach(var item in request.Config)
                    {
                        item.WriteJson(jsonWriter);
                    }
                    jsonWriter.WriteArrayEnd();
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
		public IEnumerator AcquireByUserId(
                Request.AcquireByUserIdRequest request,
                UnityAction<AsyncResult<Result.AcquireByUserIdResult>> callback
        )
		{
			var task = new AcquireByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.AcquireByUserIdResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.AcquireByUserIdResult> AcquireByUserIdAsync(
                Request.AcquireByUserIdRequest request
        )
		{
			var task = new AcquireByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        private class AcquireForceByUserIdTask : Gs2RestSessionTask<AcquireForceByUserIdRequest, AcquireForceByUserIdResult>
        {
            public AcquireForceByUserIdTask(IGs2Session session, RestSessionRequestFactory factory, AcquireForceByUserIdRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(AcquireForceByUserIdRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "exchange")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/{userId}/exchange/{rateName}/await/{awaitName}/force";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{userId}", !string.IsNullOrEmpty(request.UserId) ? request.UserId.ToString() : "null");
                url = url.Replace("{rateName}", !string.IsNullOrEmpty(request.RateName) ? request.RateName.ToString() : "null");
                url = url.Replace("{awaitName}", !string.IsNullOrEmpty(request.AwaitName) ? request.AwaitName.ToString() : "null");

                var sessionRequest = Factory.Post(url);

                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);
                jsonWriter.WriteObjectStart();
                if (request.Config != null)
                {
                    jsonWriter.WritePropertyName("config");
                    jsonWriter.WriteArrayStart();
                    foreach(var item in request.Config)
                    {
                        item.WriteJson(jsonWriter);
                    }
                    jsonWriter.WriteArrayEnd();
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
		public IEnumerator AcquireForceByUserId(
                Request.AcquireForceByUserIdRequest request,
                UnityAction<AsyncResult<Result.AcquireForceByUserIdResult>> callback
        )
		{
			var task = new AcquireForceByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.AcquireForceByUserIdResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.AcquireForceByUserIdResult> AcquireForceByUserIdAsync(
                Request.AcquireForceByUserIdRequest request
        )
		{
			var task = new AcquireForceByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        private class SkipTask : Gs2RestSessionTask<SkipRequest, SkipResult>
        {
            public SkipTask(IGs2Session session, RestSessionRequestFactory factory, SkipRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(SkipRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "exchange")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/me/exchange/{rateName}/await/{awaitName}/skip";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{rateName}", !string.IsNullOrEmpty(request.RateName) ? request.RateName.ToString() : "null");
                url = url.Replace("{awaitName}", !string.IsNullOrEmpty(request.AwaitName) ? request.AwaitName.ToString() : "null");

                var sessionRequest = Factory.Post(url);

                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);
                jsonWriter.WriteObjectStart();
                if (request.Config != null)
                {
                    jsonWriter.WritePropertyName("config");
                    jsonWriter.WriteArrayStart();
                    foreach(var item in request.Config)
                    {
                        item.WriteJson(jsonWriter);
                    }
                    jsonWriter.WriteArrayEnd();
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
		public IEnumerator Skip(
                Request.SkipRequest request,
                UnityAction<AsyncResult<Result.SkipResult>> callback
        )
		{
			var task = new SkipTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.SkipResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.SkipResult> SkipAsync(
                Request.SkipRequest request
        )
		{
			var task = new SkipTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        private class SkipByUserIdTask : Gs2RestSessionTask<SkipByUserIdRequest, SkipByUserIdResult>
        {
            public SkipByUserIdTask(IGs2Session session, RestSessionRequestFactory factory, SkipByUserIdRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(SkipByUserIdRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "exchange")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/{userId}/exchange/{rateName}/await/{awaitName}/skip";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{userId}", !string.IsNullOrEmpty(request.UserId) ? request.UserId.ToString() : "null");
                url = url.Replace("{rateName}", !string.IsNullOrEmpty(request.RateName) ? request.RateName.ToString() : "null");
                url = url.Replace("{awaitName}", !string.IsNullOrEmpty(request.AwaitName) ? request.AwaitName.ToString() : "null");

                var sessionRequest = Factory.Post(url);

                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);
                jsonWriter.WriteObjectStart();
                if (request.Config != null)
                {
                    jsonWriter.WritePropertyName("config");
                    jsonWriter.WriteArrayStart();
                    foreach(var item in request.Config)
                    {
                        item.WriteJson(jsonWriter);
                    }
                    jsonWriter.WriteArrayEnd();
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
		public IEnumerator SkipByUserId(
                Request.SkipByUserIdRequest request,
                UnityAction<AsyncResult<Result.SkipByUserIdResult>> callback
        )
		{
			var task = new SkipByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.SkipByUserIdResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.SkipByUserIdResult> SkipByUserIdAsync(
                Request.SkipByUserIdRequest request
        )
		{
			var task = new SkipByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        private class DeleteAwaitTask : Gs2RestSessionTask<DeleteAwaitRequest, DeleteAwaitResult>
        {
            public DeleteAwaitTask(IGs2Session session, RestSessionRequestFactory factory, DeleteAwaitRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DeleteAwaitRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "exchange")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/me/exchange/{rateName}/await/{awaitName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{rateName}", !string.IsNullOrEmpty(request.RateName) ? request.RateName.ToString() : "null");
                url = url.Replace("{awaitName}", !string.IsNullOrEmpty(request.AwaitName) ? request.AwaitName.ToString() : "null");

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
		public IEnumerator DeleteAwait(
                Request.DeleteAwaitRequest request,
                UnityAction<AsyncResult<Result.DeleteAwaitResult>> callback
        )
		{
			var task = new DeleteAwaitTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.DeleteAwaitResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.DeleteAwaitResult> DeleteAwaitAsync(
                Request.DeleteAwaitRequest request
        )
		{
			var task = new DeleteAwaitTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        private class DeleteAwaitByUserIdTask : Gs2RestSessionTask<DeleteAwaitByUserIdRequest, DeleteAwaitByUserIdResult>
        {
            public DeleteAwaitByUserIdTask(IGs2Session session, RestSessionRequestFactory factory, DeleteAwaitByUserIdRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DeleteAwaitByUserIdRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "exchange")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/{userId}/exchange/{rateName}/await/{awaitName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{userId}", !string.IsNullOrEmpty(request.UserId) ? request.UserId.ToString() : "null");
                url = url.Replace("{rateName}", !string.IsNullOrEmpty(request.RateName) ? request.RateName.ToString() : "null");
                url = url.Replace("{awaitName}", !string.IsNullOrEmpty(request.AwaitName) ? request.AwaitName.ToString() : "null");

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
		public IEnumerator DeleteAwaitByUserId(
                Request.DeleteAwaitByUserIdRequest request,
                UnityAction<AsyncResult<Result.DeleteAwaitByUserIdResult>> callback
        )
		{
			var task = new DeleteAwaitByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.DeleteAwaitByUserIdResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.DeleteAwaitByUserIdResult> DeleteAwaitByUserIdAsync(
                Request.DeleteAwaitByUserIdRequest request
        )
		{
			var task = new DeleteAwaitByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        private class CreateAwaitByStampSheetTask : Gs2RestSessionTask<CreateAwaitByStampSheetRequest, CreateAwaitByStampSheetResult>
        {
            public CreateAwaitByStampSheetTask(IGs2Session session, RestSessionRequestFactory factory, CreateAwaitByStampSheetRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(CreateAwaitByStampSheetRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "exchange")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/stamp/await/create";

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
		public IEnumerator CreateAwaitByStampSheet(
                Request.CreateAwaitByStampSheetRequest request,
                UnityAction<AsyncResult<Result.CreateAwaitByStampSheetResult>> callback
        )
		{
			var task = new CreateAwaitByStampSheetTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.CreateAwaitByStampSheetResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.CreateAwaitByStampSheetResult> CreateAwaitByStampSheetAsync(
                Request.CreateAwaitByStampSheetRequest request
        )
		{
			var task = new CreateAwaitByStampSheetTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        private class DeleteAwaitByStampTaskTask : Gs2RestSessionTask<DeleteAwaitByStampTaskRequest, DeleteAwaitByStampTaskResult>
        {
            public DeleteAwaitByStampTaskTask(IGs2Session session, RestSessionRequestFactory factory, DeleteAwaitByStampTaskRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DeleteAwaitByStampTaskRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "exchange")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/stamp/await/delete";

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
		public IEnumerator DeleteAwaitByStampTask(
                Request.DeleteAwaitByStampTaskRequest request,
                UnityAction<AsyncResult<Result.DeleteAwaitByStampTaskResult>> callback
        )
		{
			var task = new DeleteAwaitByStampTaskTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.DeleteAwaitByStampTaskResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.DeleteAwaitByStampTaskResult> DeleteAwaitByStampTaskAsync(
                Request.DeleteAwaitByStampTaskRequest request
        )
		{
			var task = new DeleteAwaitByStampTaskTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif
	}
}