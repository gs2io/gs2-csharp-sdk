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
using Gs2.Gs2Showcase.Request;
using Gs2.Gs2Showcase.Result;
using Gs2.Util.LitJson;

namespace Gs2.Gs2Showcase
{
	public class Gs2ShowcaseRestClient : AbstractGs2Client
	{
#if UNITY_2017_1_OR_NEWER
		private readonly CertificateHandler _certificateHandler;
#endif

		public static string Endpoint = "showcase";

        protected Gs2RestSession Gs2RestSession => (Gs2RestSession) Gs2Session;

		public Gs2ShowcaseRestClient(Gs2RestSession Gs2RestSession) : base(Gs2RestSession)
		{

		}

#if UNITY_2017_1_OR_NEWER
		public Gs2ShowcaseRestClient(Gs2RestSession gs2RestSession, CertificateHandler certificateHandler) : base(gs2RestSession)
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
                    .Replace("{service}", "showcase")
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
                    .Replace("{service}", "showcase")
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
                    .Replace("{service}", "showcase")
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
                    .Replace("{service}", "showcase")
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
                    .Replace("{service}", "showcase")
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
                    .Replace("{service}", "showcase")
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


        private class DescribeSalesItemMastersTask : Gs2RestSessionTask<DescribeSalesItemMastersRequest, DescribeSalesItemMastersResult>
        {
            public DescribeSalesItemMastersTask(IGs2Session session, RestSessionRequestFactory factory, DescribeSalesItemMastersRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DescribeSalesItemMastersRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "showcase")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/master/salesItem";

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
		public IEnumerator DescribeSalesItemMasters(
                Request.DescribeSalesItemMastersRequest request,
                UnityAction<AsyncResult<Result.DescribeSalesItemMastersResult>> callback
        )
		{
			var task = new DescribeSalesItemMastersTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.DescribeSalesItemMastersResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.DescribeSalesItemMastersResult> DescribeSalesItemMastersAsync(
                Request.DescribeSalesItemMastersRequest request
        )
		{
			var task = new DescribeSalesItemMastersTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        private class CreateSalesItemMasterTask : Gs2RestSessionTask<CreateSalesItemMasterRequest, CreateSalesItemMasterResult>
        {
            public CreateSalesItemMasterTask(IGs2Session session, RestSessionRequestFactory factory, CreateSalesItemMasterRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(CreateSalesItemMasterRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "showcase")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/master/salesItem";

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
		public IEnumerator CreateSalesItemMaster(
                Request.CreateSalesItemMasterRequest request,
                UnityAction<AsyncResult<Result.CreateSalesItemMasterResult>> callback
        )
		{
			var task = new CreateSalesItemMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.CreateSalesItemMasterResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.CreateSalesItemMasterResult> CreateSalesItemMasterAsync(
                Request.CreateSalesItemMasterRequest request
        )
		{
			var task = new CreateSalesItemMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        private class GetSalesItemMasterTask : Gs2RestSessionTask<GetSalesItemMasterRequest, GetSalesItemMasterResult>
        {
            public GetSalesItemMasterTask(IGs2Session session, RestSessionRequestFactory factory, GetSalesItemMasterRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(GetSalesItemMasterRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "showcase")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/master/salesItem/{salesItemName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{salesItemName}", !string.IsNullOrEmpty(request.SalesItemName) ? request.SalesItemName.ToString() : "null");

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
		public IEnumerator GetSalesItemMaster(
                Request.GetSalesItemMasterRequest request,
                UnityAction<AsyncResult<Result.GetSalesItemMasterResult>> callback
        )
		{
			var task = new GetSalesItemMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.GetSalesItemMasterResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.GetSalesItemMasterResult> GetSalesItemMasterAsync(
                Request.GetSalesItemMasterRequest request
        )
		{
			var task = new GetSalesItemMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        private class UpdateSalesItemMasterTask : Gs2RestSessionTask<UpdateSalesItemMasterRequest, UpdateSalesItemMasterResult>
        {
            public UpdateSalesItemMasterTask(IGs2Session session, RestSessionRequestFactory factory, UpdateSalesItemMasterRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(UpdateSalesItemMasterRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "showcase")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/master/salesItem/{salesItemName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{salesItemName}", !string.IsNullOrEmpty(request.SalesItemName) ? request.SalesItemName.ToString() : "null");

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
		public IEnumerator UpdateSalesItemMaster(
                Request.UpdateSalesItemMasterRequest request,
                UnityAction<AsyncResult<Result.UpdateSalesItemMasterResult>> callback
        )
		{
			var task = new UpdateSalesItemMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.UpdateSalesItemMasterResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.UpdateSalesItemMasterResult> UpdateSalesItemMasterAsync(
                Request.UpdateSalesItemMasterRequest request
        )
		{
			var task = new UpdateSalesItemMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        private class DeleteSalesItemMasterTask : Gs2RestSessionTask<DeleteSalesItemMasterRequest, DeleteSalesItemMasterResult>
        {
            public DeleteSalesItemMasterTask(IGs2Session session, RestSessionRequestFactory factory, DeleteSalesItemMasterRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DeleteSalesItemMasterRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "showcase")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/master/salesItem/{salesItemName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{salesItemName}", !string.IsNullOrEmpty(request.SalesItemName) ? request.SalesItemName.ToString() : "null");

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
		public IEnumerator DeleteSalesItemMaster(
                Request.DeleteSalesItemMasterRequest request,
                UnityAction<AsyncResult<Result.DeleteSalesItemMasterResult>> callback
        )
		{
			var task = new DeleteSalesItemMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.DeleteSalesItemMasterResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.DeleteSalesItemMasterResult> DeleteSalesItemMasterAsync(
                Request.DeleteSalesItemMasterRequest request
        )
		{
			var task = new DeleteSalesItemMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        private class DescribeSalesItemGroupMastersTask : Gs2RestSessionTask<DescribeSalesItemGroupMastersRequest, DescribeSalesItemGroupMastersResult>
        {
            public DescribeSalesItemGroupMastersTask(IGs2Session session, RestSessionRequestFactory factory, DescribeSalesItemGroupMastersRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DescribeSalesItemGroupMastersRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "showcase")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/master/group";

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
		public IEnumerator DescribeSalesItemGroupMasters(
                Request.DescribeSalesItemGroupMastersRequest request,
                UnityAction<AsyncResult<Result.DescribeSalesItemGroupMastersResult>> callback
        )
		{
			var task = new DescribeSalesItemGroupMastersTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.DescribeSalesItemGroupMastersResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.DescribeSalesItemGroupMastersResult> DescribeSalesItemGroupMastersAsync(
                Request.DescribeSalesItemGroupMastersRequest request
        )
		{
			var task = new DescribeSalesItemGroupMastersTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        private class CreateSalesItemGroupMasterTask : Gs2RestSessionTask<CreateSalesItemGroupMasterRequest, CreateSalesItemGroupMasterResult>
        {
            public CreateSalesItemGroupMasterTask(IGs2Session session, RestSessionRequestFactory factory, CreateSalesItemGroupMasterRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(CreateSalesItemGroupMasterRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "showcase")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/master/group";

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
                if (request.SalesItemNames != null)
                {
                    jsonWriter.WritePropertyName("salesItemNames");
                    jsonWriter.WriteArrayStart();
                    foreach(var item in request.SalesItemNames)
                    {
                        jsonWriter.Write(item);
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
		public IEnumerator CreateSalesItemGroupMaster(
                Request.CreateSalesItemGroupMasterRequest request,
                UnityAction<AsyncResult<Result.CreateSalesItemGroupMasterResult>> callback
        )
		{
			var task = new CreateSalesItemGroupMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.CreateSalesItemGroupMasterResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.CreateSalesItemGroupMasterResult> CreateSalesItemGroupMasterAsync(
                Request.CreateSalesItemGroupMasterRequest request
        )
		{
			var task = new CreateSalesItemGroupMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        private class GetSalesItemGroupMasterTask : Gs2RestSessionTask<GetSalesItemGroupMasterRequest, GetSalesItemGroupMasterResult>
        {
            public GetSalesItemGroupMasterTask(IGs2Session session, RestSessionRequestFactory factory, GetSalesItemGroupMasterRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(GetSalesItemGroupMasterRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "showcase")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/master/group/{salesItemGroupName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{salesItemGroupName}", !string.IsNullOrEmpty(request.SalesItemGroupName) ? request.SalesItemGroupName.ToString() : "null");

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
		public IEnumerator GetSalesItemGroupMaster(
                Request.GetSalesItemGroupMasterRequest request,
                UnityAction<AsyncResult<Result.GetSalesItemGroupMasterResult>> callback
        )
		{
			var task = new GetSalesItemGroupMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.GetSalesItemGroupMasterResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.GetSalesItemGroupMasterResult> GetSalesItemGroupMasterAsync(
                Request.GetSalesItemGroupMasterRequest request
        )
		{
			var task = new GetSalesItemGroupMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        private class UpdateSalesItemGroupMasterTask : Gs2RestSessionTask<UpdateSalesItemGroupMasterRequest, UpdateSalesItemGroupMasterResult>
        {
            public UpdateSalesItemGroupMasterTask(IGs2Session session, RestSessionRequestFactory factory, UpdateSalesItemGroupMasterRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(UpdateSalesItemGroupMasterRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "showcase")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/master/group/{salesItemGroupName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{salesItemGroupName}", !string.IsNullOrEmpty(request.SalesItemGroupName) ? request.SalesItemGroupName.ToString() : "null");

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
                if (request.SalesItemNames != null)
                {
                    jsonWriter.WritePropertyName("salesItemNames");
                    jsonWriter.WriteArrayStart();
                    foreach(var item in request.SalesItemNames)
                    {
                        jsonWriter.Write(item);
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
		public IEnumerator UpdateSalesItemGroupMaster(
                Request.UpdateSalesItemGroupMasterRequest request,
                UnityAction<AsyncResult<Result.UpdateSalesItemGroupMasterResult>> callback
        )
		{
			var task = new UpdateSalesItemGroupMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.UpdateSalesItemGroupMasterResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.UpdateSalesItemGroupMasterResult> UpdateSalesItemGroupMasterAsync(
                Request.UpdateSalesItemGroupMasterRequest request
        )
		{
			var task = new UpdateSalesItemGroupMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        private class DeleteSalesItemGroupMasterTask : Gs2RestSessionTask<DeleteSalesItemGroupMasterRequest, DeleteSalesItemGroupMasterResult>
        {
            public DeleteSalesItemGroupMasterTask(IGs2Session session, RestSessionRequestFactory factory, DeleteSalesItemGroupMasterRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DeleteSalesItemGroupMasterRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "showcase")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/master/group/{salesItemGroupName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{salesItemGroupName}", !string.IsNullOrEmpty(request.SalesItemGroupName) ? request.SalesItemGroupName.ToString() : "null");

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
		public IEnumerator DeleteSalesItemGroupMaster(
                Request.DeleteSalesItemGroupMasterRequest request,
                UnityAction<AsyncResult<Result.DeleteSalesItemGroupMasterResult>> callback
        )
		{
			var task = new DeleteSalesItemGroupMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.DeleteSalesItemGroupMasterResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.DeleteSalesItemGroupMasterResult> DeleteSalesItemGroupMasterAsync(
                Request.DeleteSalesItemGroupMasterRequest request
        )
		{
			var task = new DeleteSalesItemGroupMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        private class DescribeShowcaseMastersTask : Gs2RestSessionTask<DescribeShowcaseMastersRequest, DescribeShowcaseMastersResult>
        {
            public DescribeShowcaseMastersTask(IGs2Session session, RestSessionRequestFactory factory, DescribeShowcaseMastersRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DescribeShowcaseMastersRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "showcase")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/master/showcase";

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
		public IEnumerator DescribeShowcaseMasters(
                Request.DescribeShowcaseMastersRequest request,
                UnityAction<AsyncResult<Result.DescribeShowcaseMastersResult>> callback
        )
		{
			var task = new DescribeShowcaseMastersTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.DescribeShowcaseMastersResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.DescribeShowcaseMastersResult> DescribeShowcaseMastersAsync(
                Request.DescribeShowcaseMastersRequest request
        )
		{
			var task = new DescribeShowcaseMastersTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        private class CreateShowcaseMasterTask : Gs2RestSessionTask<CreateShowcaseMasterRequest, CreateShowcaseMasterResult>
        {
            public CreateShowcaseMasterTask(IGs2Session session, RestSessionRequestFactory factory, CreateShowcaseMasterRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(CreateShowcaseMasterRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "showcase")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/master/showcase";

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
                if (request.DisplayItems != null)
                {
                    jsonWriter.WritePropertyName("displayItems");
                    jsonWriter.WriteArrayStart();
                    foreach(var item in request.DisplayItems)
                    {
                        item.WriteJson(jsonWriter);
                    }
                    jsonWriter.WriteArrayEnd();
                }
                if (request.SalesPeriodEventId != null)
                {
                    jsonWriter.WritePropertyName("salesPeriodEventId");
                    jsonWriter.Write(request.SalesPeriodEventId);
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
		public IEnumerator CreateShowcaseMaster(
                Request.CreateShowcaseMasterRequest request,
                UnityAction<AsyncResult<Result.CreateShowcaseMasterResult>> callback
        )
		{
			var task = new CreateShowcaseMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.CreateShowcaseMasterResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.CreateShowcaseMasterResult> CreateShowcaseMasterAsync(
                Request.CreateShowcaseMasterRequest request
        )
		{
			var task = new CreateShowcaseMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        private class GetShowcaseMasterTask : Gs2RestSessionTask<GetShowcaseMasterRequest, GetShowcaseMasterResult>
        {
            public GetShowcaseMasterTask(IGs2Session session, RestSessionRequestFactory factory, GetShowcaseMasterRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(GetShowcaseMasterRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "showcase")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/master/showcase/{showcaseName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{showcaseName}", !string.IsNullOrEmpty(request.ShowcaseName) ? request.ShowcaseName.ToString() : "null");

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
		public IEnumerator GetShowcaseMaster(
                Request.GetShowcaseMasterRequest request,
                UnityAction<AsyncResult<Result.GetShowcaseMasterResult>> callback
        )
		{
			var task = new GetShowcaseMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.GetShowcaseMasterResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.GetShowcaseMasterResult> GetShowcaseMasterAsync(
                Request.GetShowcaseMasterRequest request
        )
		{
			var task = new GetShowcaseMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        private class UpdateShowcaseMasterTask : Gs2RestSessionTask<UpdateShowcaseMasterRequest, UpdateShowcaseMasterResult>
        {
            public UpdateShowcaseMasterTask(IGs2Session session, RestSessionRequestFactory factory, UpdateShowcaseMasterRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(UpdateShowcaseMasterRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "showcase")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/master/showcase/{showcaseName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{showcaseName}", !string.IsNullOrEmpty(request.ShowcaseName) ? request.ShowcaseName.ToString() : "null");

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
                if (request.DisplayItems != null)
                {
                    jsonWriter.WritePropertyName("displayItems");
                    jsonWriter.WriteArrayStart();
                    foreach(var item in request.DisplayItems)
                    {
                        item.WriteJson(jsonWriter);
                    }
                    jsonWriter.WriteArrayEnd();
                }
                if (request.SalesPeriodEventId != null)
                {
                    jsonWriter.WritePropertyName("salesPeriodEventId");
                    jsonWriter.Write(request.SalesPeriodEventId);
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
		public IEnumerator UpdateShowcaseMaster(
                Request.UpdateShowcaseMasterRequest request,
                UnityAction<AsyncResult<Result.UpdateShowcaseMasterResult>> callback
        )
		{
			var task = new UpdateShowcaseMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.UpdateShowcaseMasterResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.UpdateShowcaseMasterResult> UpdateShowcaseMasterAsync(
                Request.UpdateShowcaseMasterRequest request
        )
		{
			var task = new UpdateShowcaseMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        private class DeleteShowcaseMasterTask : Gs2RestSessionTask<DeleteShowcaseMasterRequest, DeleteShowcaseMasterResult>
        {
            public DeleteShowcaseMasterTask(IGs2Session session, RestSessionRequestFactory factory, DeleteShowcaseMasterRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DeleteShowcaseMasterRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "showcase")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/master/showcase/{showcaseName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{showcaseName}", !string.IsNullOrEmpty(request.ShowcaseName) ? request.ShowcaseName.ToString() : "null");

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
		public IEnumerator DeleteShowcaseMaster(
                Request.DeleteShowcaseMasterRequest request,
                UnityAction<AsyncResult<Result.DeleteShowcaseMasterResult>> callback
        )
		{
			var task = new DeleteShowcaseMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.DeleteShowcaseMasterResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.DeleteShowcaseMasterResult> DeleteShowcaseMasterAsync(
                Request.DeleteShowcaseMasterRequest request
        )
		{
			var task = new DeleteShowcaseMasterTask(
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
                    .Replace("{service}", "showcase")
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


        private class GetCurrentShowcaseMasterTask : Gs2RestSessionTask<GetCurrentShowcaseMasterRequest, GetCurrentShowcaseMasterResult>
        {
            public GetCurrentShowcaseMasterTask(IGs2Session session, RestSessionRequestFactory factory, GetCurrentShowcaseMasterRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(GetCurrentShowcaseMasterRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "showcase")
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
		public IEnumerator GetCurrentShowcaseMaster(
                Request.GetCurrentShowcaseMasterRequest request,
                UnityAction<AsyncResult<Result.GetCurrentShowcaseMasterResult>> callback
        )
		{
			var task = new GetCurrentShowcaseMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.GetCurrentShowcaseMasterResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.GetCurrentShowcaseMasterResult> GetCurrentShowcaseMasterAsync(
                Request.GetCurrentShowcaseMasterRequest request
        )
		{
			var task = new GetCurrentShowcaseMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        private class UpdateCurrentShowcaseMasterTask : Gs2RestSessionTask<UpdateCurrentShowcaseMasterRequest, UpdateCurrentShowcaseMasterResult>
        {
            public UpdateCurrentShowcaseMasterTask(IGs2Session session, RestSessionRequestFactory factory, UpdateCurrentShowcaseMasterRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(UpdateCurrentShowcaseMasterRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "showcase")
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
		public IEnumerator UpdateCurrentShowcaseMaster(
                Request.UpdateCurrentShowcaseMasterRequest request,
                UnityAction<AsyncResult<Result.UpdateCurrentShowcaseMasterResult>> callback
        )
		{
			var task = new UpdateCurrentShowcaseMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.UpdateCurrentShowcaseMasterResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.UpdateCurrentShowcaseMasterResult> UpdateCurrentShowcaseMasterAsync(
                Request.UpdateCurrentShowcaseMasterRequest request
        )
		{
			var task = new UpdateCurrentShowcaseMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        private class UpdateCurrentShowcaseMasterFromGitHubTask : Gs2RestSessionTask<UpdateCurrentShowcaseMasterFromGitHubRequest, UpdateCurrentShowcaseMasterFromGitHubResult>
        {
            public UpdateCurrentShowcaseMasterFromGitHubTask(IGs2Session session, RestSessionRequestFactory factory, UpdateCurrentShowcaseMasterFromGitHubRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(UpdateCurrentShowcaseMasterFromGitHubRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "showcase")
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
		public IEnumerator UpdateCurrentShowcaseMasterFromGitHub(
                Request.UpdateCurrentShowcaseMasterFromGitHubRequest request,
                UnityAction<AsyncResult<Result.UpdateCurrentShowcaseMasterFromGitHubResult>> callback
        )
		{
			var task = new UpdateCurrentShowcaseMasterFromGitHubTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.UpdateCurrentShowcaseMasterFromGitHubResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.UpdateCurrentShowcaseMasterFromGitHubResult> UpdateCurrentShowcaseMasterFromGitHubAsync(
                Request.UpdateCurrentShowcaseMasterFromGitHubRequest request
        )
		{
			var task = new UpdateCurrentShowcaseMasterFromGitHubTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        private class DescribeShowcasesTask : Gs2RestSessionTask<DescribeShowcasesRequest, DescribeShowcasesResult>
        {
            public DescribeShowcasesTask(IGs2Session session, RestSessionRequestFactory factory, DescribeShowcasesRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DescribeShowcasesRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "showcase")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/me/showcase";

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
		public IEnumerator DescribeShowcases(
                Request.DescribeShowcasesRequest request,
                UnityAction<AsyncResult<Result.DescribeShowcasesResult>> callback
        )
		{
			var task = new DescribeShowcasesTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.DescribeShowcasesResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.DescribeShowcasesResult> DescribeShowcasesAsync(
                Request.DescribeShowcasesRequest request
        )
		{
			var task = new DescribeShowcasesTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        private class DescribeShowcasesByUserIdTask : Gs2RestSessionTask<DescribeShowcasesByUserIdRequest, DescribeShowcasesByUserIdResult>
        {
            public DescribeShowcasesByUserIdTask(IGs2Session session, RestSessionRequestFactory factory, DescribeShowcasesByUserIdRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DescribeShowcasesByUserIdRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "showcase")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/{userId}/showcase";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
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
		public IEnumerator DescribeShowcasesByUserId(
                Request.DescribeShowcasesByUserIdRequest request,
                UnityAction<AsyncResult<Result.DescribeShowcasesByUserIdResult>> callback
        )
		{
			var task = new DescribeShowcasesByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.DescribeShowcasesByUserIdResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.DescribeShowcasesByUserIdResult> DescribeShowcasesByUserIdAsync(
                Request.DescribeShowcasesByUserIdRequest request
        )
		{
			var task = new DescribeShowcasesByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        private class GetShowcaseTask : Gs2RestSessionTask<GetShowcaseRequest, GetShowcaseResult>
        {
            public GetShowcaseTask(IGs2Session session, RestSessionRequestFactory factory, GetShowcaseRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(GetShowcaseRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "showcase")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/me/showcase/{showcaseName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{showcaseName}", !string.IsNullOrEmpty(request.ShowcaseName) ? request.ShowcaseName.ToString() : "null");

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
		public IEnumerator GetShowcase(
                Request.GetShowcaseRequest request,
                UnityAction<AsyncResult<Result.GetShowcaseResult>> callback
        )
		{
			var task = new GetShowcaseTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.GetShowcaseResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.GetShowcaseResult> GetShowcaseAsync(
                Request.GetShowcaseRequest request
        )
		{
			var task = new GetShowcaseTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        private class GetShowcaseByUserIdTask : Gs2RestSessionTask<GetShowcaseByUserIdRequest, GetShowcaseByUserIdResult>
        {
            public GetShowcaseByUserIdTask(IGs2Session session, RestSessionRequestFactory factory, GetShowcaseByUserIdRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(GetShowcaseByUserIdRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "showcase")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/{userId}/showcase/{showcaseName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{showcaseName}", !string.IsNullOrEmpty(request.ShowcaseName) ? request.ShowcaseName.ToString() : "null");
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
		public IEnumerator GetShowcaseByUserId(
                Request.GetShowcaseByUserIdRequest request,
                UnityAction<AsyncResult<Result.GetShowcaseByUserIdResult>> callback
        )
		{
			var task = new GetShowcaseByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.GetShowcaseByUserIdResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.GetShowcaseByUserIdResult> GetShowcaseByUserIdAsync(
                Request.GetShowcaseByUserIdRequest request
        )
		{
			var task = new GetShowcaseByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        private class BuyTask : Gs2RestSessionTask<BuyRequest, BuyResult>
        {
            public BuyTask(IGs2Session session, RestSessionRequestFactory factory, BuyRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(BuyRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "showcase")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/me/showcase/{showcaseName}/{displayItemId}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{showcaseName}", !string.IsNullOrEmpty(request.ShowcaseName) ? request.ShowcaseName.ToString() : "null");
                url = url.Replace("{displayItemId}", !string.IsNullOrEmpty(request.DisplayItemId) ? request.DisplayItemId.ToString() : "null");

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
		public IEnumerator Buy(
                Request.BuyRequest request,
                UnityAction<AsyncResult<Result.BuyResult>> callback
        )
		{
			var task = new BuyTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.BuyResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.BuyResult> BuyAsync(
                Request.BuyRequest request
        )
		{
			var task = new BuyTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        private class BuyByUserIdTask : Gs2RestSessionTask<BuyByUserIdRequest, BuyByUserIdResult>
        {
            public BuyByUserIdTask(IGs2Session session, RestSessionRequestFactory factory, BuyByUserIdRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(BuyByUserIdRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "showcase")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/{userId}/showcase/{showcaseName}/{displayItemId}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{showcaseName}", !string.IsNullOrEmpty(request.ShowcaseName) ? request.ShowcaseName.ToString() : "null");
                url = url.Replace("{displayItemId}", !string.IsNullOrEmpty(request.DisplayItemId) ? request.DisplayItemId.ToString() : "null");
                url = url.Replace("{userId}", !string.IsNullOrEmpty(request.UserId) ? request.UserId.ToString() : "null");

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
		public IEnumerator BuyByUserId(
                Request.BuyByUserIdRequest request,
                UnityAction<AsyncResult<Result.BuyByUserIdResult>> callback
        )
		{
			var task = new BuyByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.BuyByUserIdResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.BuyByUserIdResult> BuyByUserIdAsync(
                Request.BuyByUserIdRequest request
        )
		{
			var task = new BuyByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif
	}
}