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
    #if GS2_ENABLE_UNITASK
using Cysharp.Threading.Tasks;
    #endif
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
using Gs2.Gs2Inventory.Request;
using Gs2.Gs2Inventory.Result;
using Gs2.Util.LitJson;

namespace Gs2.Gs2Inventory
{
	public class Gs2InventoryRestClient : AbstractGs2Client
	{
#if UNITY_2017_1_OR_NEWER
		private readonly CertificateHandler _certificateHandler;
#endif

		public static string Endpoint = "inventory";

        protected Gs2RestSession Gs2RestSession => (Gs2RestSession) Gs2Session;

		public Gs2InventoryRestClient(Gs2RestSession Gs2RestSession) : base(Gs2RestSession)
		{

		}

#if UNITY_2017_1_OR_NEWER
		public Gs2InventoryRestClient(Gs2RestSession gs2RestSession, CertificateHandler certificateHandler) : base(gs2RestSession)
		{
			_certificateHandler = certificateHandler;
		}
#endif


        public class DescribeNamespacesTask : Gs2RestSessionTask<DescribeNamespacesRequest, DescribeNamespacesResult>
        {
            public DescribeNamespacesTask(IGs2Session session, RestSessionRequestFactory factory, DescribeNamespacesRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DescribeNamespacesRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "inventory")
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
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.DescribeNamespacesResult>(task.Result, task.Error));
        }

		public IFuture<Result.DescribeNamespacesResult> DescribeNamespacesFuture(
                Request.DescribeNamespacesRequest request
        )
		{
			return new DescribeNamespacesTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.DescribeNamespacesResult> DescribeNamespacesAsync(
                Request.DescribeNamespacesRequest request
        )
		{
            AsyncResult<Result.DescribeNamespacesResult> result = null;
			await DescribeNamespaces(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
    #else
		public DescribeNamespacesTask DescribeNamespacesAsync(
                Request.DescribeNamespacesRequest request
        )
		{
			return new DescribeNamespacesTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
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


        public class CreateNamespaceTask : Gs2RestSessionTask<CreateNamespaceRequest, CreateNamespaceResult>
        {
            public CreateNamespaceTask(IGs2Session session, RestSessionRequestFactory factory, CreateNamespaceRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(CreateNamespaceRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "inventory")
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
                if (request.AcquireScript != null)
                {
                    jsonWriter.WritePropertyName("acquireScript");
                    request.AcquireScript.WriteJson(jsonWriter);
                }
                if (request.OverflowScript != null)
                {
                    jsonWriter.WritePropertyName("overflowScript");
                    request.OverflowScript.WriteJson(jsonWriter);
                }
                if (request.ConsumeScript != null)
                {
                    jsonWriter.WritePropertyName("consumeScript");
                    request.ConsumeScript.WriteJson(jsonWriter);
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
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
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
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.CreateNamespaceResult> CreateNamespaceAsync(
                Request.CreateNamespaceRequest request
        )
		{
            AsyncResult<Result.CreateNamespaceResult> result = null;
			await CreateNamespace(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
    #else
		public CreateNamespaceTask CreateNamespaceAsync(
                Request.CreateNamespaceRequest request
        )
		{
			return new CreateNamespaceTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
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
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class GetNamespaceStatusTask : Gs2RestSessionTask<GetNamespaceStatusRequest, GetNamespaceStatusResult>
        {
            public GetNamespaceStatusTask(IGs2Session session, RestSessionRequestFactory factory, GetNamespaceStatusRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(GetNamespaceStatusRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "inventory")
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
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.GetNamespaceStatusResult>(task.Result, task.Error));
        }

		public IFuture<Result.GetNamespaceStatusResult> GetNamespaceStatusFuture(
                Request.GetNamespaceStatusRequest request
        )
		{
			return new GetNamespaceStatusTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.GetNamespaceStatusResult> GetNamespaceStatusAsync(
                Request.GetNamespaceStatusRequest request
        )
		{
            AsyncResult<Result.GetNamespaceStatusResult> result = null;
			await GetNamespaceStatus(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
    #else
		public GetNamespaceStatusTask GetNamespaceStatusAsync(
                Request.GetNamespaceStatusRequest request
        )
		{
			return new GetNamespaceStatusTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
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


        public class GetNamespaceTask : Gs2RestSessionTask<GetNamespaceRequest, GetNamespaceResult>
        {
            public GetNamespaceTask(IGs2Session session, RestSessionRequestFactory factory, GetNamespaceRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(GetNamespaceRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "inventory")
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
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
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
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.GetNamespaceResult> GetNamespaceAsync(
                Request.GetNamespaceRequest request
        )
		{
            AsyncResult<Result.GetNamespaceResult> result = null;
			await GetNamespace(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
    #else
		public GetNamespaceTask GetNamespaceAsync(
                Request.GetNamespaceRequest request
        )
		{
			return new GetNamespaceTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
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
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class UpdateNamespaceTask : Gs2RestSessionTask<UpdateNamespaceRequest, UpdateNamespaceResult>
        {
            public UpdateNamespaceTask(IGs2Session session, RestSessionRequestFactory factory, UpdateNamespaceRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(UpdateNamespaceRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "inventory")
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
                if (request.AcquireScript != null)
                {
                    jsonWriter.WritePropertyName("acquireScript");
                    request.AcquireScript.WriteJson(jsonWriter);
                }
                if (request.OverflowScript != null)
                {
                    jsonWriter.WritePropertyName("overflowScript");
                    request.OverflowScript.WriteJson(jsonWriter);
                }
                if (request.ConsumeScript != null)
                {
                    jsonWriter.WritePropertyName("consumeScript");
                    request.ConsumeScript.WriteJson(jsonWriter);
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
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
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
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.UpdateNamespaceResult> UpdateNamespaceAsync(
                Request.UpdateNamespaceRequest request
        )
		{
            AsyncResult<Result.UpdateNamespaceResult> result = null;
			await UpdateNamespace(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
    #else
		public UpdateNamespaceTask UpdateNamespaceAsync(
                Request.UpdateNamespaceRequest request
        )
		{
			return new UpdateNamespaceTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
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
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class DeleteNamespaceTask : Gs2RestSessionTask<DeleteNamespaceRequest, DeleteNamespaceResult>
        {
            public DeleteNamespaceTask(IGs2Session session, RestSessionRequestFactory factory, DeleteNamespaceRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DeleteNamespaceRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "inventory")
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
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
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
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.DeleteNamespaceResult> DeleteNamespaceAsync(
                Request.DeleteNamespaceRequest request
        )
		{
            AsyncResult<Result.DeleteNamespaceResult> result = null;
			await DeleteNamespace(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
    #else
		public DeleteNamespaceTask DeleteNamespaceAsync(
                Request.DeleteNamespaceRequest request
        )
		{
			return new DeleteNamespaceTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
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
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class DescribeInventoryModelMastersTask : Gs2RestSessionTask<DescribeInventoryModelMastersRequest, DescribeInventoryModelMastersResult>
        {
            public DescribeInventoryModelMastersTask(IGs2Session session, RestSessionRequestFactory factory, DescribeInventoryModelMastersRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DescribeInventoryModelMastersRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "inventory")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/master/inventory";

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
		public IEnumerator DescribeInventoryModelMasters(
                Request.DescribeInventoryModelMastersRequest request,
                UnityAction<AsyncResult<Result.DescribeInventoryModelMastersResult>> callback
        )
		{
			var task = new DescribeInventoryModelMastersTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.DescribeInventoryModelMastersResult>(task.Result, task.Error));
        }

		public IFuture<Result.DescribeInventoryModelMastersResult> DescribeInventoryModelMastersFuture(
                Request.DescribeInventoryModelMastersRequest request
        )
		{
			return new DescribeInventoryModelMastersTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.DescribeInventoryModelMastersResult> DescribeInventoryModelMastersAsync(
                Request.DescribeInventoryModelMastersRequest request
        )
		{
            AsyncResult<Result.DescribeInventoryModelMastersResult> result = null;
			await DescribeInventoryModelMasters(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
    #else
		public DescribeInventoryModelMastersTask DescribeInventoryModelMastersAsync(
                Request.DescribeInventoryModelMastersRequest request
        )
		{
			return new DescribeInventoryModelMastersTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.DescribeInventoryModelMastersResult> DescribeInventoryModelMastersAsync(
                Request.DescribeInventoryModelMastersRequest request
        )
		{
			var task = new DescribeInventoryModelMastersTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class CreateInventoryModelMasterTask : Gs2RestSessionTask<CreateInventoryModelMasterRequest, CreateInventoryModelMasterResult>
        {
            public CreateInventoryModelMasterTask(IGs2Session session, RestSessionRequestFactory factory, CreateInventoryModelMasterRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(CreateInventoryModelMasterRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "inventory")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/master/inventory";

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
                if (request.InitialCapacity != null)
                {
                    jsonWriter.WritePropertyName("initialCapacity");
                    jsonWriter.Write(request.InitialCapacity.ToString());
                }
                if (request.MaxCapacity != null)
                {
                    jsonWriter.WritePropertyName("maxCapacity");
                    jsonWriter.Write(request.MaxCapacity.ToString());
                }
                if (request.ProtectReferencedItem != null)
                {
                    jsonWriter.WritePropertyName("protectReferencedItem");
                    jsonWriter.Write(request.ProtectReferencedItem.ToString());
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
		public IEnumerator CreateInventoryModelMaster(
                Request.CreateInventoryModelMasterRequest request,
                UnityAction<AsyncResult<Result.CreateInventoryModelMasterResult>> callback
        )
		{
			var task = new CreateInventoryModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.CreateInventoryModelMasterResult>(task.Result, task.Error));
        }

		public IFuture<Result.CreateInventoryModelMasterResult> CreateInventoryModelMasterFuture(
                Request.CreateInventoryModelMasterRequest request
        )
		{
			return new CreateInventoryModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.CreateInventoryModelMasterResult> CreateInventoryModelMasterAsync(
                Request.CreateInventoryModelMasterRequest request
        )
		{
            AsyncResult<Result.CreateInventoryModelMasterResult> result = null;
			await CreateInventoryModelMaster(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
    #else
		public CreateInventoryModelMasterTask CreateInventoryModelMasterAsync(
                Request.CreateInventoryModelMasterRequest request
        )
		{
			return new CreateInventoryModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.CreateInventoryModelMasterResult> CreateInventoryModelMasterAsync(
                Request.CreateInventoryModelMasterRequest request
        )
		{
			var task = new CreateInventoryModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class GetInventoryModelMasterTask : Gs2RestSessionTask<GetInventoryModelMasterRequest, GetInventoryModelMasterResult>
        {
            public GetInventoryModelMasterTask(IGs2Session session, RestSessionRequestFactory factory, GetInventoryModelMasterRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(GetInventoryModelMasterRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "inventory")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/master/inventory/{inventoryName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{inventoryName}", !string.IsNullOrEmpty(request.InventoryName) ? request.InventoryName.ToString() : "null");

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
		public IEnumerator GetInventoryModelMaster(
                Request.GetInventoryModelMasterRequest request,
                UnityAction<AsyncResult<Result.GetInventoryModelMasterResult>> callback
        )
		{
			var task = new GetInventoryModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.GetInventoryModelMasterResult>(task.Result, task.Error));
        }

		public IFuture<Result.GetInventoryModelMasterResult> GetInventoryModelMasterFuture(
                Request.GetInventoryModelMasterRequest request
        )
		{
			return new GetInventoryModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.GetInventoryModelMasterResult> GetInventoryModelMasterAsync(
                Request.GetInventoryModelMasterRequest request
        )
		{
            AsyncResult<Result.GetInventoryModelMasterResult> result = null;
			await GetInventoryModelMaster(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
    #else
		public GetInventoryModelMasterTask GetInventoryModelMasterAsync(
                Request.GetInventoryModelMasterRequest request
        )
		{
			return new GetInventoryModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.GetInventoryModelMasterResult> GetInventoryModelMasterAsync(
                Request.GetInventoryModelMasterRequest request
        )
		{
			var task = new GetInventoryModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class UpdateInventoryModelMasterTask : Gs2RestSessionTask<UpdateInventoryModelMasterRequest, UpdateInventoryModelMasterResult>
        {
            public UpdateInventoryModelMasterTask(IGs2Session session, RestSessionRequestFactory factory, UpdateInventoryModelMasterRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(UpdateInventoryModelMasterRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "inventory")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/master/inventory/{inventoryName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{inventoryName}", !string.IsNullOrEmpty(request.InventoryName) ? request.InventoryName.ToString() : "null");

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
                if (request.InitialCapacity != null)
                {
                    jsonWriter.WritePropertyName("initialCapacity");
                    jsonWriter.Write(request.InitialCapacity.ToString());
                }
                if (request.MaxCapacity != null)
                {
                    jsonWriter.WritePropertyName("maxCapacity");
                    jsonWriter.Write(request.MaxCapacity.ToString());
                }
                if (request.ProtectReferencedItem != null)
                {
                    jsonWriter.WritePropertyName("protectReferencedItem");
                    jsonWriter.Write(request.ProtectReferencedItem.ToString());
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
		public IEnumerator UpdateInventoryModelMaster(
                Request.UpdateInventoryModelMasterRequest request,
                UnityAction<AsyncResult<Result.UpdateInventoryModelMasterResult>> callback
        )
		{
			var task = new UpdateInventoryModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.UpdateInventoryModelMasterResult>(task.Result, task.Error));
        }

		public IFuture<Result.UpdateInventoryModelMasterResult> UpdateInventoryModelMasterFuture(
                Request.UpdateInventoryModelMasterRequest request
        )
		{
			return new UpdateInventoryModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.UpdateInventoryModelMasterResult> UpdateInventoryModelMasterAsync(
                Request.UpdateInventoryModelMasterRequest request
        )
		{
            AsyncResult<Result.UpdateInventoryModelMasterResult> result = null;
			await UpdateInventoryModelMaster(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
    #else
		public UpdateInventoryModelMasterTask UpdateInventoryModelMasterAsync(
                Request.UpdateInventoryModelMasterRequest request
        )
		{
			return new UpdateInventoryModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.UpdateInventoryModelMasterResult> UpdateInventoryModelMasterAsync(
                Request.UpdateInventoryModelMasterRequest request
        )
		{
			var task = new UpdateInventoryModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class DeleteInventoryModelMasterTask : Gs2RestSessionTask<DeleteInventoryModelMasterRequest, DeleteInventoryModelMasterResult>
        {
            public DeleteInventoryModelMasterTask(IGs2Session session, RestSessionRequestFactory factory, DeleteInventoryModelMasterRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DeleteInventoryModelMasterRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "inventory")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/master/inventory/{inventoryName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{inventoryName}", !string.IsNullOrEmpty(request.InventoryName) ? request.InventoryName.ToString() : "null");

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
		public IEnumerator DeleteInventoryModelMaster(
                Request.DeleteInventoryModelMasterRequest request,
                UnityAction<AsyncResult<Result.DeleteInventoryModelMasterResult>> callback
        )
		{
			var task = new DeleteInventoryModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.DeleteInventoryModelMasterResult>(task.Result, task.Error));
        }

		public IFuture<Result.DeleteInventoryModelMasterResult> DeleteInventoryModelMasterFuture(
                Request.DeleteInventoryModelMasterRequest request
        )
		{
			return new DeleteInventoryModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.DeleteInventoryModelMasterResult> DeleteInventoryModelMasterAsync(
                Request.DeleteInventoryModelMasterRequest request
        )
		{
            AsyncResult<Result.DeleteInventoryModelMasterResult> result = null;
			await DeleteInventoryModelMaster(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
    #else
		public DeleteInventoryModelMasterTask DeleteInventoryModelMasterAsync(
                Request.DeleteInventoryModelMasterRequest request
        )
		{
			return new DeleteInventoryModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.DeleteInventoryModelMasterResult> DeleteInventoryModelMasterAsync(
                Request.DeleteInventoryModelMasterRequest request
        )
		{
			var task = new DeleteInventoryModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class DescribeInventoryModelsTask : Gs2RestSessionTask<DescribeInventoryModelsRequest, DescribeInventoryModelsResult>
        {
            public DescribeInventoryModelsTask(IGs2Session session, RestSessionRequestFactory factory, DescribeInventoryModelsRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DescribeInventoryModelsRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "inventory")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/inventory";

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
		public IEnumerator DescribeInventoryModels(
                Request.DescribeInventoryModelsRequest request,
                UnityAction<AsyncResult<Result.DescribeInventoryModelsResult>> callback
        )
		{
			var task = new DescribeInventoryModelsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.DescribeInventoryModelsResult>(task.Result, task.Error));
        }

		public IFuture<Result.DescribeInventoryModelsResult> DescribeInventoryModelsFuture(
                Request.DescribeInventoryModelsRequest request
        )
		{
			return new DescribeInventoryModelsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.DescribeInventoryModelsResult> DescribeInventoryModelsAsync(
                Request.DescribeInventoryModelsRequest request
        )
		{
            AsyncResult<Result.DescribeInventoryModelsResult> result = null;
			await DescribeInventoryModels(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
    #else
		public DescribeInventoryModelsTask DescribeInventoryModelsAsync(
                Request.DescribeInventoryModelsRequest request
        )
		{
			return new DescribeInventoryModelsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.DescribeInventoryModelsResult> DescribeInventoryModelsAsync(
                Request.DescribeInventoryModelsRequest request
        )
		{
			var task = new DescribeInventoryModelsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class GetInventoryModelTask : Gs2RestSessionTask<GetInventoryModelRequest, GetInventoryModelResult>
        {
            public GetInventoryModelTask(IGs2Session session, RestSessionRequestFactory factory, GetInventoryModelRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(GetInventoryModelRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "inventory")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/inventory/{inventoryName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{inventoryName}", !string.IsNullOrEmpty(request.InventoryName) ? request.InventoryName.ToString() : "null");

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
		public IEnumerator GetInventoryModel(
                Request.GetInventoryModelRequest request,
                UnityAction<AsyncResult<Result.GetInventoryModelResult>> callback
        )
		{
			var task = new GetInventoryModelTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.GetInventoryModelResult>(task.Result, task.Error));
        }

		public IFuture<Result.GetInventoryModelResult> GetInventoryModelFuture(
                Request.GetInventoryModelRequest request
        )
		{
			return new GetInventoryModelTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.GetInventoryModelResult> GetInventoryModelAsync(
                Request.GetInventoryModelRequest request
        )
		{
            AsyncResult<Result.GetInventoryModelResult> result = null;
			await GetInventoryModel(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
    #else
		public GetInventoryModelTask GetInventoryModelAsync(
                Request.GetInventoryModelRequest request
        )
		{
			return new GetInventoryModelTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.GetInventoryModelResult> GetInventoryModelAsync(
                Request.GetInventoryModelRequest request
        )
		{
			var task = new GetInventoryModelTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class DescribeItemModelMastersTask : Gs2RestSessionTask<DescribeItemModelMastersRequest, DescribeItemModelMastersResult>
        {
            public DescribeItemModelMastersTask(IGs2Session session, RestSessionRequestFactory factory, DescribeItemModelMastersRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DescribeItemModelMastersRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "inventory")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/master/inventory/{inventoryName}/item";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{inventoryName}", !string.IsNullOrEmpty(request.InventoryName) ? request.InventoryName.ToString() : "null");

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
		public IEnumerator DescribeItemModelMasters(
                Request.DescribeItemModelMastersRequest request,
                UnityAction<AsyncResult<Result.DescribeItemModelMastersResult>> callback
        )
		{
			var task = new DescribeItemModelMastersTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.DescribeItemModelMastersResult>(task.Result, task.Error));
        }

		public IFuture<Result.DescribeItemModelMastersResult> DescribeItemModelMastersFuture(
                Request.DescribeItemModelMastersRequest request
        )
		{
			return new DescribeItemModelMastersTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.DescribeItemModelMastersResult> DescribeItemModelMastersAsync(
                Request.DescribeItemModelMastersRequest request
        )
		{
            AsyncResult<Result.DescribeItemModelMastersResult> result = null;
			await DescribeItemModelMasters(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
    #else
		public DescribeItemModelMastersTask DescribeItemModelMastersAsync(
                Request.DescribeItemModelMastersRequest request
        )
		{
			return new DescribeItemModelMastersTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.DescribeItemModelMastersResult> DescribeItemModelMastersAsync(
                Request.DescribeItemModelMastersRequest request
        )
		{
			var task = new DescribeItemModelMastersTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class CreateItemModelMasterTask : Gs2RestSessionTask<CreateItemModelMasterRequest, CreateItemModelMasterResult>
        {
            public CreateItemModelMasterTask(IGs2Session session, RestSessionRequestFactory factory, CreateItemModelMasterRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(CreateItemModelMasterRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "inventory")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/master/inventory/{inventoryName}/item";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{inventoryName}", !string.IsNullOrEmpty(request.InventoryName) ? request.InventoryName.ToString() : "null");

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
                if (request.StackingLimit != null)
                {
                    jsonWriter.WritePropertyName("stackingLimit");
                    jsonWriter.Write(request.StackingLimit.ToString());
                }
                if (request.AllowMultipleStacks != null)
                {
                    jsonWriter.WritePropertyName("allowMultipleStacks");
                    jsonWriter.Write(request.AllowMultipleStacks.ToString());
                }
                if (request.SortValue != null)
                {
                    jsonWriter.WritePropertyName("sortValue");
                    jsonWriter.Write(request.SortValue.ToString());
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
		public IEnumerator CreateItemModelMaster(
                Request.CreateItemModelMasterRequest request,
                UnityAction<AsyncResult<Result.CreateItemModelMasterResult>> callback
        )
		{
			var task = new CreateItemModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.CreateItemModelMasterResult>(task.Result, task.Error));
        }

		public IFuture<Result.CreateItemModelMasterResult> CreateItemModelMasterFuture(
                Request.CreateItemModelMasterRequest request
        )
		{
			return new CreateItemModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.CreateItemModelMasterResult> CreateItemModelMasterAsync(
                Request.CreateItemModelMasterRequest request
        )
		{
            AsyncResult<Result.CreateItemModelMasterResult> result = null;
			await CreateItemModelMaster(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
    #else
		public CreateItemModelMasterTask CreateItemModelMasterAsync(
                Request.CreateItemModelMasterRequest request
        )
		{
			return new CreateItemModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.CreateItemModelMasterResult> CreateItemModelMasterAsync(
                Request.CreateItemModelMasterRequest request
        )
		{
			var task = new CreateItemModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class GetItemModelMasterTask : Gs2RestSessionTask<GetItemModelMasterRequest, GetItemModelMasterResult>
        {
            public GetItemModelMasterTask(IGs2Session session, RestSessionRequestFactory factory, GetItemModelMasterRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(GetItemModelMasterRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "inventory")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/master/inventory/{inventoryName}/item/{itemName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{inventoryName}", !string.IsNullOrEmpty(request.InventoryName) ? request.InventoryName.ToString() : "null");
                url = url.Replace("{itemName}", !string.IsNullOrEmpty(request.ItemName) ? request.ItemName.ToString() : "null");

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
		public IEnumerator GetItemModelMaster(
                Request.GetItemModelMasterRequest request,
                UnityAction<AsyncResult<Result.GetItemModelMasterResult>> callback
        )
		{
			var task = new GetItemModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.GetItemModelMasterResult>(task.Result, task.Error));
        }

		public IFuture<Result.GetItemModelMasterResult> GetItemModelMasterFuture(
                Request.GetItemModelMasterRequest request
        )
		{
			return new GetItemModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.GetItemModelMasterResult> GetItemModelMasterAsync(
                Request.GetItemModelMasterRequest request
        )
		{
            AsyncResult<Result.GetItemModelMasterResult> result = null;
			await GetItemModelMaster(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
    #else
		public GetItemModelMasterTask GetItemModelMasterAsync(
                Request.GetItemModelMasterRequest request
        )
		{
			return new GetItemModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.GetItemModelMasterResult> GetItemModelMasterAsync(
                Request.GetItemModelMasterRequest request
        )
		{
			var task = new GetItemModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class UpdateItemModelMasterTask : Gs2RestSessionTask<UpdateItemModelMasterRequest, UpdateItemModelMasterResult>
        {
            public UpdateItemModelMasterTask(IGs2Session session, RestSessionRequestFactory factory, UpdateItemModelMasterRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(UpdateItemModelMasterRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "inventory")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/master/inventory/{inventoryName}/item/{itemName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{inventoryName}", !string.IsNullOrEmpty(request.InventoryName) ? request.InventoryName.ToString() : "null");
                url = url.Replace("{itemName}", !string.IsNullOrEmpty(request.ItemName) ? request.ItemName.ToString() : "null");

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
                if (request.StackingLimit != null)
                {
                    jsonWriter.WritePropertyName("stackingLimit");
                    jsonWriter.Write(request.StackingLimit.ToString());
                }
                if (request.AllowMultipleStacks != null)
                {
                    jsonWriter.WritePropertyName("allowMultipleStacks");
                    jsonWriter.Write(request.AllowMultipleStacks.ToString());
                }
                if (request.SortValue != null)
                {
                    jsonWriter.WritePropertyName("sortValue");
                    jsonWriter.Write(request.SortValue.ToString());
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
		public IEnumerator UpdateItemModelMaster(
                Request.UpdateItemModelMasterRequest request,
                UnityAction<AsyncResult<Result.UpdateItemModelMasterResult>> callback
        )
		{
			var task = new UpdateItemModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.UpdateItemModelMasterResult>(task.Result, task.Error));
        }

		public IFuture<Result.UpdateItemModelMasterResult> UpdateItemModelMasterFuture(
                Request.UpdateItemModelMasterRequest request
        )
		{
			return new UpdateItemModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.UpdateItemModelMasterResult> UpdateItemModelMasterAsync(
                Request.UpdateItemModelMasterRequest request
        )
		{
            AsyncResult<Result.UpdateItemModelMasterResult> result = null;
			await UpdateItemModelMaster(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
    #else
		public UpdateItemModelMasterTask UpdateItemModelMasterAsync(
                Request.UpdateItemModelMasterRequest request
        )
		{
			return new UpdateItemModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.UpdateItemModelMasterResult> UpdateItemModelMasterAsync(
                Request.UpdateItemModelMasterRequest request
        )
		{
			var task = new UpdateItemModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class DeleteItemModelMasterTask : Gs2RestSessionTask<DeleteItemModelMasterRequest, DeleteItemModelMasterResult>
        {
            public DeleteItemModelMasterTask(IGs2Session session, RestSessionRequestFactory factory, DeleteItemModelMasterRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DeleteItemModelMasterRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "inventory")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/master/inventory/{inventoryName}/item/{itemName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{inventoryName}", !string.IsNullOrEmpty(request.InventoryName) ? request.InventoryName.ToString() : "null");
                url = url.Replace("{itemName}", !string.IsNullOrEmpty(request.ItemName) ? request.ItemName.ToString() : "null");

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
		public IEnumerator DeleteItemModelMaster(
                Request.DeleteItemModelMasterRequest request,
                UnityAction<AsyncResult<Result.DeleteItemModelMasterResult>> callback
        )
		{
			var task = new DeleteItemModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.DeleteItemModelMasterResult>(task.Result, task.Error));
        }

		public IFuture<Result.DeleteItemModelMasterResult> DeleteItemModelMasterFuture(
                Request.DeleteItemModelMasterRequest request
        )
		{
			return new DeleteItemModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.DeleteItemModelMasterResult> DeleteItemModelMasterAsync(
                Request.DeleteItemModelMasterRequest request
        )
		{
            AsyncResult<Result.DeleteItemModelMasterResult> result = null;
			await DeleteItemModelMaster(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
    #else
		public DeleteItemModelMasterTask DeleteItemModelMasterAsync(
                Request.DeleteItemModelMasterRequest request
        )
		{
			return new DeleteItemModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.DeleteItemModelMasterResult> DeleteItemModelMasterAsync(
                Request.DeleteItemModelMasterRequest request
        )
		{
			var task = new DeleteItemModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class DescribeItemModelsTask : Gs2RestSessionTask<DescribeItemModelsRequest, DescribeItemModelsResult>
        {
            public DescribeItemModelsTask(IGs2Session session, RestSessionRequestFactory factory, DescribeItemModelsRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DescribeItemModelsRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "inventory")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/inventory/{inventoryName}/item";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{inventoryName}", !string.IsNullOrEmpty(request.InventoryName) ? request.InventoryName.ToString() : "null");

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
		public IEnumerator DescribeItemModels(
                Request.DescribeItemModelsRequest request,
                UnityAction<AsyncResult<Result.DescribeItemModelsResult>> callback
        )
		{
			var task = new DescribeItemModelsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.DescribeItemModelsResult>(task.Result, task.Error));
        }

		public IFuture<Result.DescribeItemModelsResult> DescribeItemModelsFuture(
                Request.DescribeItemModelsRequest request
        )
		{
			return new DescribeItemModelsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.DescribeItemModelsResult> DescribeItemModelsAsync(
                Request.DescribeItemModelsRequest request
        )
		{
            AsyncResult<Result.DescribeItemModelsResult> result = null;
			await DescribeItemModels(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
    #else
		public DescribeItemModelsTask DescribeItemModelsAsync(
                Request.DescribeItemModelsRequest request
        )
		{
			return new DescribeItemModelsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.DescribeItemModelsResult> DescribeItemModelsAsync(
                Request.DescribeItemModelsRequest request
        )
		{
			var task = new DescribeItemModelsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class GetItemModelTask : Gs2RestSessionTask<GetItemModelRequest, GetItemModelResult>
        {
            public GetItemModelTask(IGs2Session session, RestSessionRequestFactory factory, GetItemModelRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(GetItemModelRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "inventory")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/inventory/{inventoryName}/item/{itemName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{inventoryName}", !string.IsNullOrEmpty(request.InventoryName) ? request.InventoryName.ToString() : "null");
                url = url.Replace("{itemName}", !string.IsNullOrEmpty(request.ItemName) ? request.ItemName.ToString() : "null");

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
		public IEnumerator GetItemModel(
                Request.GetItemModelRequest request,
                UnityAction<AsyncResult<Result.GetItemModelResult>> callback
        )
		{
			var task = new GetItemModelTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.GetItemModelResult>(task.Result, task.Error));
        }

		public IFuture<Result.GetItemModelResult> GetItemModelFuture(
                Request.GetItemModelRequest request
        )
		{
			return new GetItemModelTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.GetItemModelResult> GetItemModelAsync(
                Request.GetItemModelRequest request
        )
		{
            AsyncResult<Result.GetItemModelResult> result = null;
			await GetItemModel(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
    #else
		public GetItemModelTask GetItemModelAsync(
                Request.GetItemModelRequest request
        )
		{
			return new GetItemModelTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.GetItemModelResult> GetItemModelAsync(
                Request.GetItemModelRequest request
        )
		{
			var task = new GetItemModelTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class ExportMasterTask : Gs2RestSessionTask<ExportMasterRequest, ExportMasterResult>
        {
            public ExportMasterTask(IGs2Session session, RestSessionRequestFactory factory, ExportMasterRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(ExportMasterRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "inventory")
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
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.ExportMasterResult>(task.Result, task.Error));
        }

		public IFuture<Result.ExportMasterResult> ExportMasterFuture(
                Request.ExportMasterRequest request
        )
		{
			return new ExportMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.ExportMasterResult> ExportMasterAsync(
                Request.ExportMasterRequest request
        )
		{
            AsyncResult<Result.ExportMasterResult> result = null;
			await ExportMaster(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
    #else
		public ExportMasterTask ExportMasterAsync(
                Request.ExportMasterRequest request
        )
		{
			return new ExportMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
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


        public class GetCurrentItemModelMasterTask : Gs2RestSessionTask<GetCurrentItemModelMasterRequest, GetCurrentItemModelMasterResult>
        {
            public GetCurrentItemModelMasterTask(IGs2Session session, RestSessionRequestFactory factory, GetCurrentItemModelMasterRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(GetCurrentItemModelMasterRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "inventory")
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
		public IEnumerator GetCurrentItemModelMaster(
                Request.GetCurrentItemModelMasterRequest request,
                UnityAction<AsyncResult<Result.GetCurrentItemModelMasterResult>> callback
        )
		{
			var task = new GetCurrentItemModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.GetCurrentItemModelMasterResult>(task.Result, task.Error));
        }

		public IFuture<Result.GetCurrentItemModelMasterResult> GetCurrentItemModelMasterFuture(
                Request.GetCurrentItemModelMasterRequest request
        )
		{
			return new GetCurrentItemModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.GetCurrentItemModelMasterResult> GetCurrentItemModelMasterAsync(
                Request.GetCurrentItemModelMasterRequest request
        )
		{
            AsyncResult<Result.GetCurrentItemModelMasterResult> result = null;
			await GetCurrentItemModelMaster(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
    #else
		public GetCurrentItemModelMasterTask GetCurrentItemModelMasterAsync(
                Request.GetCurrentItemModelMasterRequest request
        )
		{
			return new GetCurrentItemModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.GetCurrentItemModelMasterResult> GetCurrentItemModelMasterAsync(
                Request.GetCurrentItemModelMasterRequest request
        )
		{
			var task = new GetCurrentItemModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class UpdateCurrentItemModelMasterTask : Gs2RestSessionTask<UpdateCurrentItemModelMasterRequest, UpdateCurrentItemModelMasterResult>
        {
            public UpdateCurrentItemModelMasterTask(IGs2Session session, RestSessionRequestFactory factory, UpdateCurrentItemModelMasterRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(UpdateCurrentItemModelMasterRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "inventory")
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
		public IEnumerator UpdateCurrentItemModelMaster(
                Request.UpdateCurrentItemModelMasterRequest request,
                UnityAction<AsyncResult<Result.UpdateCurrentItemModelMasterResult>> callback
        )
		{
			var task = new UpdateCurrentItemModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.UpdateCurrentItemModelMasterResult>(task.Result, task.Error));
        }

		public IFuture<Result.UpdateCurrentItemModelMasterResult> UpdateCurrentItemModelMasterFuture(
                Request.UpdateCurrentItemModelMasterRequest request
        )
		{
			return new UpdateCurrentItemModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.UpdateCurrentItemModelMasterResult> UpdateCurrentItemModelMasterAsync(
                Request.UpdateCurrentItemModelMasterRequest request
        )
		{
            AsyncResult<Result.UpdateCurrentItemModelMasterResult> result = null;
			await UpdateCurrentItemModelMaster(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
    #else
		public UpdateCurrentItemModelMasterTask UpdateCurrentItemModelMasterAsync(
                Request.UpdateCurrentItemModelMasterRequest request
        )
		{
			return new UpdateCurrentItemModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.UpdateCurrentItemModelMasterResult> UpdateCurrentItemModelMasterAsync(
                Request.UpdateCurrentItemModelMasterRequest request
        )
		{
			var task = new UpdateCurrentItemModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class UpdateCurrentItemModelMasterFromGitHubTask : Gs2RestSessionTask<UpdateCurrentItemModelMasterFromGitHubRequest, UpdateCurrentItemModelMasterFromGitHubResult>
        {
            public UpdateCurrentItemModelMasterFromGitHubTask(IGs2Session session, RestSessionRequestFactory factory, UpdateCurrentItemModelMasterFromGitHubRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(UpdateCurrentItemModelMasterFromGitHubRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "inventory")
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
		public IEnumerator UpdateCurrentItemModelMasterFromGitHub(
                Request.UpdateCurrentItemModelMasterFromGitHubRequest request,
                UnityAction<AsyncResult<Result.UpdateCurrentItemModelMasterFromGitHubResult>> callback
        )
		{
			var task = new UpdateCurrentItemModelMasterFromGitHubTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.UpdateCurrentItemModelMasterFromGitHubResult>(task.Result, task.Error));
        }

		public IFuture<Result.UpdateCurrentItemModelMasterFromGitHubResult> UpdateCurrentItemModelMasterFromGitHubFuture(
                Request.UpdateCurrentItemModelMasterFromGitHubRequest request
        )
		{
			return new UpdateCurrentItemModelMasterFromGitHubTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.UpdateCurrentItemModelMasterFromGitHubResult> UpdateCurrentItemModelMasterFromGitHubAsync(
                Request.UpdateCurrentItemModelMasterFromGitHubRequest request
        )
		{
            AsyncResult<Result.UpdateCurrentItemModelMasterFromGitHubResult> result = null;
			await UpdateCurrentItemModelMasterFromGitHub(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
    #else
		public UpdateCurrentItemModelMasterFromGitHubTask UpdateCurrentItemModelMasterFromGitHubAsync(
                Request.UpdateCurrentItemModelMasterFromGitHubRequest request
        )
		{
			return new UpdateCurrentItemModelMasterFromGitHubTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.UpdateCurrentItemModelMasterFromGitHubResult> UpdateCurrentItemModelMasterFromGitHubAsync(
                Request.UpdateCurrentItemModelMasterFromGitHubRequest request
        )
		{
			var task = new UpdateCurrentItemModelMasterFromGitHubTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class DescribeInventoriesTask : Gs2RestSessionTask<DescribeInventoriesRequest, DescribeInventoriesResult>
        {
            public DescribeInventoriesTask(IGs2Session session, RestSessionRequestFactory factory, DescribeInventoriesRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DescribeInventoriesRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "inventory")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/me/inventory";

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
		public IEnumerator DescribeInventories(
                Request.DescribeInventoriesRequest request,
                UnityAction<AsyncResult<Result.DescribeInventoriesResult>> callback
        )
		{
			var task = new DescribeInventoriesTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.DescribeInventoriesResult>(task.Result, task.Error));
        }

		public IFuture<Result.DescribeInventoriesResult> DescribeInventoriesFuture(
                Request.DescribeInventoriesRequest request
        )
		{
			return new DescribeInventoriesTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.DescribeInventoriesResult> DescribeInventoriesAsync(
                Request.DescribeInventoriesRequest request
        )
		{
            AsyncResult<Result.DescribeInventoriesResult> result = null;
			await DescribeInventories(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
    #else
		public DescribeInventoriesTask DescribeInventoriesAsync(
                Request.DescribeInventoriesRequest request
        )
		{
			return new DescribeInventoriesTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.DescribeInventoriesResult> DescribeInventoriesAsync(
                Request.DescribeInventoriesRequest request
        )
		{
			var task = new DescribeInventoriesTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class DescribeInventoriesByUserIdTask : Gs2RestSessionTask<DescribeInventoriesByUserIdRequest, DescribeInventoriesByUserIdResult>
        {
            public DescribeInventoriesByUserIdTask(IGs2Session session, RestSessionRequestFactory factory, DescribeInventoriesByUserIdRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DescribeInventoriesByUserIdRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "inventory")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/{userId}/inventory";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{userId}", !string.IsNullOrEmpty(request.UserId) ? request.UserId.ToString() : "null");

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
		public IEnumerator DescribeInventoriesByUserId(
                Request.DescribeInventoriesByUserIdRequest request,
                UnityAction<AsyncResult<Result.DescribeInventoriesByUserIdResult>> callback
        )
		{
			var task = new DescribeInventoriesByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.DescribeInventoriesByUserIdResult>(task.Result, task.Error));
        }

		public IFuture<Result.DescribeInventoriesByUserIdResult> DescribeInventoriesByUserIdFuture(
                Request.DescribeInventoriesByUserIdRequest request
        )
		{
			return new DescribeInventoriesByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.DescribeInventoriesByUserIdResult> DescribeInventoriesByUserIdAsync(
                Request.DescribeInventoriesByUserIdRequest request
        )
		{
            AsyncResult<Result.DescribeInventoriesByUserIdResult> result = null;
			await DescribeInventoriesByUserId(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
    #else
		public DescribeInventoriesByUserIdTask DescribeInventoriesByUserIdAsync(
                Request.DescribeInventoriesByUserIdRequest request
        )
		{
			return new DescribeInventoriesByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.DescribeInventoriesByUserIdResult> DescribeInventoriesByUserIdAsync(
                Request.DescribeInventoriesByUserIdRequest request
        )
		{
			var task = new DescribeInventoriesByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class GetInventoryTask : Gs2RestSessionTask<GetInventoryRequest, GetInventoryResult>
        {
            public GetInventoryTask(IGs2Session session, RestSessionRequestFactory factory, GetInventoryRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(GetInventoryRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "inventory")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/me/inventory/{inventoryName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{inventoryName}", !string.IsNullOrEmpty(request.InventoryName) ? request.InventoryName.ToString() : "null");

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
		public IEnumerator GetInventory(
                Request.GetInventoryRequest request,
                UnityAction<AsyncResult<Result.GetInventoryResult>> callback
        )
		{
			var task = new GetInventoryTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.GetInventoryResult>(task.Result, task.Error));
        }

		public IFuture<Result.GetInventoryResult> GetInventoryFuture(
                Request.GetInventoryRequest request
        )
		{
			return new GetInventoryTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.GetInventoryResult> GetInventoryAsync(
                Request.GetInventoryRequest request
        )
		{
            AsyncResult<Result.GetInventoryResult> result = null;
			await GetInventory(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
    #else
		public GetInventoryTask GetInventoryAsync(
                Request.GetInventoryRequest request
        )
		{
			return new GetInventoryTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.GetInventoryResult> GetInventoryAsync(
                Request.GetInventoryRequest request
        )
		{
			var task = new GetInventoryTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class GetInventoryByUserIdTask : Gs2RestSessionTask<GetInventoryByUserIdRequest, GetInventoryByUserIdResult>
        {
            public GetInventoryByUserIdTask(IGs2Session session, RestSessionRequestFactory factory, GetInventoryByUserIdRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(GetInventoryByUserIdRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "inventory")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/{userId}/inventory/{inventoryName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{inventoryName}", !string.IsNullOrEmpty(request.InventoryName) ? request.InventoryName.ToString() : "null");
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
		public IEnumerator GetInventoryByUserId(
                Request.GetInventoryByUserIdRequest request,
                UnityAction<AsyncResult<Result.GetInventoryByUserIdResult>> callback
        )
		{
			var task = new GetInventoryByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.GetInventoryByUserIdResult>(task.Result, task.Error));
        }

		public IFuture<Result.GetInventoryByUserIdResult> GetInventoryByUserIdFuture(
                Request.GetInventoryByUserIdRequest request
        )
		{
			return new GetInventoryByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.GetInventoryByUserIdResult> GetInventoryByUserIdAsync(
                Request.GetInventoryByUserIdRequest request
        )
		{
            AsyncResult<Result.GetInventoryByUserIdResult> result = null;
			await GetInventoryByUserId(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
    #else
		public GetInventoryByUserIdTask GetInventoryByUserIdAsync(
                Request.GetInventoryByUserIdRequest request
        )
		{
			return new GetInventoryByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.GetInventoryByUserIdResult> GetInventoryByUserIdAsync(
                Request.GetInventoryByUserIdRequest request
        )
		{
			var task = new GetInventoryByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class AddCapacityByUserIdTask : Gs2RestSessionTask<AddCapacityByUserIdRequest, AddCapacityByUserIdResult>
        {
            public AddCapacityByUserIdTask(IGs2Session session, RestSessionRequestFactory factory, AddCapacityByUserIdRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(AddCapacityByUserIdRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "inventory")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/{userId}/inventory/{inventoryName}/capacity";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{inventoryName}", !string.IsNullOrEmpty(request.InventoryName) ? request.InventoryName.ToString() : "null");
                url = url.Replace("{userId}", !string.IsNullOrEmpty(request.UserId) ? request.UserId.ToString() : "null");

                var sessionRequest = Factory.Post(url);

                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);
                jsonWriter.WriteObjectStart();
                if (request.AddCapacityValue != null)
                {
                    jsonWriter.WritePropertyName("addCapacityValue");
                    jsonWriter.Write(request.AddCapacityValue.ToString());
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
                if (request.DuplicationAvoider != null)
                {
                    sessionRequest.AddHeader("X-GS2-DUPLICATION-AVOIDER", request.DuplicationAvoider);
                }

                AddHeader(
                    Session.Credential,
                    sessionRequest
                );

                return sessionRequest;
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator AddCapacityByUserId(
                Request.AddCapacityByUserIdRequest request,
                UnityAction<AsyncResult<Result.AddCapacityByUserIdResult>> callback
        )
		{
			var task = new AddCapacityByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.AddCapacityByUserIdResult>(task.Result, task.Error));
        }

		public IFuture<Result.AddCapacityByUserIdResult> AddCapacityByUserIdFuture(
                Request.AddCapacityByUserIdRequest request
        )
		{
			return new AddCapacityByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.AddCapacityByUserIdResult> AddCapacityByUserIdAsync(
                Request.AddCapacityByUserIdRequest request
        )
		{
            AsyncResult<Result.AddCapacityByUserIdResult> result = null;
			await AddCapacityByUserId(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
    #else
		public AddCapacityByUserIdTask AddCapacityByUserIdAsync(
                Request.AddCapacityByUserIdRequest request
        )
		{
			return new AddCapacityByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.AddCapacityByUserIdResult> AddCapacityByUserIdAsync(
                Request.AddCapacityByUserIdRequest request
        )
		{
			var task = new AddCapacityByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class SetCapacityByUserIdTask : Gs2RestSessionTask<SetCapacityByUserIdRequest, SetCapacityByUserIdResult>
        {
            public SetCapacityByUserIdTask(IGs2Session session, RestSessionRequestFactory factory, SetCapacityByUserIdRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(SetCapacityByUserIdRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "inventory")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/{userId}/inventory/{inventoryName}/capacity";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{inventoryName}", !string.IsNullOrEmpty(request.InventoryName) ? request.InventoryName.ToString() : "null");
                url = url.Replace("{userId}", !string.IsNullOrEmpty(request.UserId) ? request.UserId.ToString() : "null");

                var sessionRequest = Factory.Put(url);

                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);
                jsonWriter.WriteObjectStart();
                if (request.NewCapacityValue != null)
                {
                    jsonWriter.WritePropertyName("newCapacityValue");
                    jsonWriter.Write(request.NewCapacityValue.ToString());
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
                if (request.DuplicationAvoider != null)
                {
                    sessionRequest.AddHeader("X-GS2-DUPLICATION-AVOIDER", request.DuplicationAvoider);
                }

                AddHeader(
                    Session.Credential,
                    sessionRequest
                );

                return sessionRequest;
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator SetCapacityByUserId(
                Request.SetCapacityByUserIdRequest request,
                UnityAction<AsyncResult<Result.SetCapacityByUserIdResult>> callback
        )
		{
			var task = new SetCapacityByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.SetCapacityByUserIdResult>(task.Result, task.Error));
        }

		public IFuture<Result.SetCapacityByUserIdResult> SetCapacityByUserIdFuture(
                Request.SetCapacityByUserIdRequest request
        )
		{
			return new SetCapacityByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.SetCapacityByUserIdResult> SetCapacityByUserIdAsync(
                Request.SetCapacityByUserIdRequest request
        )
		{
            AsyncResult<Result.SetCapacityByUserIdResult> result = null;
			await SetCapacityByUserId(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
    #else
		public SetCapacityByUserIdTask SetCapacityByUserIdAsync(
                Request.SetCapacityByUserIdRequest request
        )
		{
			return new SetCapacityByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.SetCapacityByUserIdResult> SetCapacityByUserIdAsync(
                Request.SetCapacityByUserIdRequest request
        )
		{
			var task = new SetCapacityByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class DeleteInventoryByUserIdTask : Gs2RestSessionTask<DeleteInventoryByUserIdRequest, DeleteInventoryByUserIdResult>
        {
            public DeleteInventoryByUserIdTask(IGs2Session session, RestSessionRequestFactory factory, DeleteInventoryByUserIdRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DeleteInventoryByUserIdRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "inventory")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/{userId}/inventory/{inventoryName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{inventoryName}", !string.IsNullOrEmpty(request.InventoryName) ? request.InventoryName.ToString() : "null");
                url = url.Replace("{userId}", !string.IsNullOrEmpty(request.UserId) ? request.UserId.ToString() : "null");

                var sessionRequest = Factory.Delete(url);
                if (request.ContextStack != null)
                {
                    sessionRequest.AddQueryString("contextStack", request.ContextStack);
                }

                if (request.RequestId != null)
                {
                    sessionRequest.AddHeader("X-GS2-REQUEST-ID", request.RequestId);
                }
                if (request.DuplicationAvoider != null)
                {
                    sessionRequest.AddHeader("X-GS2-DUPLICATION-AVOIDER", request.DuplicationAvoider);
                }

                AddHeader(
                    Session.Credential,
                    sessionRequest
                );

                return sessionRequest;
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator DeleteInventoryByUserId(
                Request.DeleteInventoryByUserIdRequest request,
                UnityAction<AsyncResult<Result.DeleteInventoryByUserIdResult>> callback
        )
		{
			var task = new DeleteInventoryByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.DeleteInventoryByUserIdResult>(task.Result, task.Error));
        }

		public IFuture<Result.DeleteInventoryByUserIdResult> DeleteInventoryByUserIdFuture(
                Request.DeleteInventoryByUserIdRequest request
        )
		{
			return new DeleteInventoryByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.DeleteInventoryByUserIdResult> DeleteInventoryByUserIdAsync(
                Request.DeleteInventoryByUserIdRequest request
        )
		{
            AsyncResult<Result.DeleteInventoryByUserIdResult> result = null;
			await DeleteInventoryByUserId(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
    #else
		public DeleteInventoryByUserIdTask DeleteInventoryByUserIdAsync(
                Request.DeleteInventoryByUserIdRequest request
        )
		{
			return new DeleteInventoryByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.DeleteInventoryByUserIdResult> DeleteInventoryByUserIdAsync(
                Request.DeleteInventoryByUserIdRequest request
        )
		{
			var task = new DeleteInventoryByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class AddCapacityByStampSheetTask : Gs2RestSessionTask<AddCapacityByStampSheetRequest, AddCapacityByStampSheetResult>
        {
            public AddCapacityByStampSheetTask(IGs2Session session, RestSessionRequestFactory factory, AddCapacityByStampSheetRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(AddCapacityByStampSheetRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "inventory")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/stamp/inventory/capacity/add";

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
		public IEnumerator AddCapacityByStampSheet(
                Request.AddCapacityByStampSheetRequest request,
                UnityAction<AsyncResult<Result.AddCapacityByStampSheetResult>> callback
        )
		{
			var task = new AddCapacityByStampSheetTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.AddCapacityByStampSheetResult>(task.Result, task.Error));
        }

		public IFuture<Result.AddCapacityByStampSheetResult> AddCapacityByStampSheetFuture(
                Request.AddCapacityByStampSheetRequest request
        )
		{
			return new AddCapacityByStampSheetTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.AddCapacityByStampSheetResult> AddCapacityByStampSheetAsync(
                Request.AddCapacityByStampSheetRequest request
        )
		{
            AsyncResult<Result.AddCapacityByStampSheetResult> result = null;
			await AddCapacityByStampSheet(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
    #else
		public AddCapacityByStampSheetTask AddCapacityByStampSheetAsync(
                Request.AddCapacityByStampSheetRequest request
        )
		{
			return new AddCapacityByStampSheetTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.AddCapacityByStampSheetResult> AddCapacityByStampSheetAsync(
                Request.AddCapacityByStampSheetRequest request
        )
		{
			var task = new AddCapacityByStampSheetTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class SetCapacityByStampSheetTask : Gs2RestSessionTask<SetCapacityByStampSheetRequest, SetCapacityByStampSheetResult>
        {
            public SetCapacityByStampSheetTask(IGs2Session session, RestSessionRequestFactory factory, SetCapacityByStampSheetRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(SetCapacityByStampSheetRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "inventory")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/stamp/inventory/capacity/set";

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
		public IEnumerator SetCapacityByStampSheet(
                Request.SetCapacityByStampSheetRequest request,
                UnityAction<AsyncResult<Result.SetCapacityByStampSheetResult>> callback
        )
		{
			var task = new SetCapacityByStampSheetTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.SetCapacityByStampSheetResult>(task.Result, task.Error));
        }

		public IFuture<Result.SetCapacityByStampSheetResult> SetCapacityByStampSheetFuture(
                Request.SetCapacityByStampSheetRequest request
        )
		{
			return new SetCapacityByStampSheetTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.SetCapacityByStampSheetResult> SetCapacityByStampSheetAsync(
                Request.SetCapacityByStampSheetRequest request
        )
		{
            AsyncResult<Result.SetCapacityByStampSheetResult> result = null;
			await SetCapacityByStampSheet(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
    #else
		public SetCapacityByStampSheetTask SetCapacityByStampSheetAsync(
                Request.SetCapacityByStampSheetRequest request
        )
		{
			return new SetCapacityByStampSheetTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.SetCapacityByStampSheetResult> SetCapacityByStampSheetAsync(
                Request.SetCapacityByStampSheetRequest request
        )
		{
			var task = new SetCapacityByStampSheetTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class DescribeItemSetsTask : Gs2RestSessionTask<DescribeItemSetsRequest, DescribeItemSetsResult>
        {
            public DescribeItemSetsTask(IGs2Session session, RestSessionRequestFactory factory, DescribeItemSetsRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DescribeItemSetsRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "inventory")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/me/inventory/{inventoryName}/item";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{inventoryName}", !string.IsNullOrEmpty(request.InventoryName) ? request.InventoryName.ToString() : "null");

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
		public IEnumerator DescribeItemSets(
                Request.DescribeItemSetsRequest request,
                UnityAction<AsyncResult<Result.DescribeItemSetsResult>> callback
        )
		{
			var task = new DescribeItemSetsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.DescribeItemSetsResult>(task.Result, task.Error));
        }

		public IFuture<Result.DescribeItemSetsResult> DescribeItemSetsFuture(
                Request.DescribeItemSetsRequest request
        )
		{
			return new DescribeItemSetsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.DescribeItemSetsResult> DescribeItemSetsAsync(
                Request.DescribeItemSetsRequest request
        )
		{
            AsyncResult<Result.DescribeItemSetsResult> result = null;
			await DescribeItemSets(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
    #else
		public DescribeItemSetsTask DescribeItemSetsAsync(
                Request.DescribeItemSetsRequest request
        )
		{
			return new DescribeItemSetsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.DescribeItemSetsResult> DescribeItemSetsAsync(
                Request.DescribeItemSetsRequest request
        )
		{
			var task = new DescribeItemSetsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class DescribeItemSetsByUserIdTask : Gs2RestSessionTask<DescribeItemSetsByUserIdRequest, DescribeItemSetsByUserIdResult>
        {
            public DescribeItemSetsByUserIdTask(IGs2Session session, RestSessionRequestFactory factory, DescribeItemSetsByUserIdRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DescribeItemSetsByUserIdRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "inventory")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/{userId}/inventory/{inventoryName}/item";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{inventoryName}", !string.IsNullOrEmpty(request.InventoryName) ? request.InventoryName.ToString() : "null");
                url = url.Replace("{userId}", !string.IsNullOrEmpty(request.UserId) ? request.UserId.ToString() : "null");

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
		public IEnumerator DescribeItemSetsByUserId(
                Request.DescribeItemSetsByUserIdRequest request,
                UnityAction<AsyncResult<Result.DescribeItemSetsByUserIdResult>> callback
        )
		{
			var task = new DescribeItemSetsByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.DescribeItemSetsByUserIdResult>(task.Result, task.Error));
        }

		public IFuture<Result.DescribeItemSetsByUserIdResult> DescribeItemSetsByUserIdFuture(
                Request.DescribeItemSetsByUserIdRequest request
        )
		{
			return new DescribeItemSetsByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.DescribeItemSetsByUserIdResult> DescribeItemSetsByUserIdAsync(
                Request.DescribeItemSetsByUserIdRequest request
        )
		{
            AsyncResult<Result.DescribeItemSetsByUserIdResult> result = null;
			await DescribeItemSetsByUserId(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
    #else
		public DescribeItemSetsByUserIdTask DescribeItemSetsByUserIdAsync(
                Request.DescribeItemSetsByUserIdRequest request
        )
		{
			return new DescribeItemSetsByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.DescribeItemSetsByUserIdResult> DescribeItemSetsByUserIdAsync(
                Request.DescribeItemSetsByUserIdRequest request
        )
		{
			var task = new DescribeItemSetsByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class GetItemSetTask : Gs2RestSessionTask<GetItemSetRequest, GetItemSetResult>
        {
            public GetItemSetTask(IGs2Session session, RestSessionRequestFactory factory, GetItemSetRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(GetItemSetRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "inventory")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/me/inventory/{inventoryName}/item/{itemName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{inventoryName}", !string.IsNullOrEmpty(request.InventoryName) ? request.InventoryName.ToString() : "null");
                url = url.Replace("{itemName}", !string.IsNullOrEmpty(request.ItemName) ? request.ItemName.ToString() : "null");

                var sessionRequest = Factory.Get(url);
                if (request.ContextStack != null)
                {
                    sessionRequest.AddQueryString("contextStack", request.ContextStack);
                }
                if (request.ItemSetName != null) {
                    sessionRequest.AddQueryString("itemSetName", $"{request.ItemSetName}");
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
		public IEnumerator GetItemSet(
                Request.GetItemSetRequest request,
                UnityAction<AsyncResult<Result.GetItemSetResult>> callback
        )
		{
			var task = new GetItemSetTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.GetItemSetResult>(task.Result, task.Error));
        }

		public IFuture<Result.GetItemSetResult> GetItemSetFuture(
                Request.GetItemSetRequest request
        )
		{
			return new GetItemSetTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.GetItemSetResult> GetItemSetAsync(
                Request.GetItemSetRequest request
        )
		{
            AsyncResult<Result.GetItemSetResult> result = null;
			await GetItemSet(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
    #else
		public GetItemSetTask GetItemSetAsync(
                Request.GetItemSetRequest request
        )
		{
			return new GetItemSetTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.GetItemSetResult> GetItemSetAsync(
                Request.GetItemSetRequest request
        )
		{
			var task = new GetItemSetTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class GetItemSetByUserIdTask : Gs2RestSessionTask<GetItemSetByUserIdRequest, GetItemSetByUserIdResult>
        {
            public GetItemSetByUserIdTask(IGs2Session session, RestSessionRequestFactory factory, GetItemSetByUserIdRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(GetItemSetByUserIdRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "inventory")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/{userId}/inventory/{inventoryName}/item/{itemName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{inventoryName}", !string.IsNullOrEmpty(request.InventoryName) ? request.InventoryName.ToString() : "null");
                url = url.Replace("{userId}", !string.IsNullOrEmpty(request.UserId) ? request.UserId.ToString() : "null");
                url = url.Replace("{itemName}", !string.IsNullOrEmpty(request.ItemName) ? request.ItemName.ToString() : "null");

                var sessionRequest = Factory.Get(url);
                if (request.ContextStack != null)
                {
                    sessionRequest.AddQueryString("contextStack", request.ContextStack);
                }
                if (request.ItemSetName != null) {
                    sessionRequest.AddQueryString("itemSetName", $"{request.ItemSetName}");
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
		public IEnumerator GetItemSetByUserId(
                Request.GetItemSetByUserIdRequest request,
                UnityAction<AsyncResult<Result.GetItemSetByUserIdResult>> callback
        )
		{
			var task = new GetItemSetByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.GetItemSetByUserIdResult>(task.Result, task.Error));
        }

		public IFuture<Result.GetItemSetByUserIdResult> GetItemSetByUserIdFuture(
                Request.GetItemSetByUserIdRequest request
        )
		{
			return new GetItemSetByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.GetItemSetByUserIdResult> GetItemSetByUserIdAsync(
                Request.GetItemSetByUserIdRequest request
        )
		{
            AsyncResult<Result.GetItemSetByUserIdResult> result = null;
			await GetItemSetByUserId(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
    #else
		public GetItemSetByUserIdTask GetItemSetByUserIdAsync(
                Request.GetItemSetByUserIdRequest request
        )
		{
			return new GetItemSetByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.GetItemSetByUserIdResult> GetItemSetByUserIdAsync(
                Request.GetItemSetByUserIdRequest request
        )
		{
			var task = new GetItemSetByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class GetItemWithSignatureTask : Gs2RestSessionTask<GetItemWithSignatureRequest, GetItemWithSignatureResult>
        {
            public GetItemWithSignatureTask(IGs2Session session, RestSessionRequestFactory factory, GetItemWithSignatureRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(GetItemWithSignatureRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "inventory")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/me/inventory/{inventoryName}/item/{itemName}/signature";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{inventoryName}", !string.IsNullOrEmpty(request.InventoryName) ? request.InventoryName.ToString() : "null");
                url = url.Replace("{itemName}", !string.IsNullOrEmpty(request.ItemName) ? request.ItemName.ToString() : "null");

                var sessionRequest = Factory.Get(url);
                if (request.ContextStack != null)
                {
                    sessionRequest.AddQueryString("contextStack", request.ContextStack);
                }
                if (request.ItemSetName != null) {
                    sessionRequest.AddQueryString("itemSetName", $"{request.ItemSetName}");
                }
                if (request.KeyId != null) {
                    sessionRequest.AddQueryString("keyId", $"{request.KeyId}");
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
		public IEnumerator GetItemWithSignature(
                Request.GetItemWithSignatureRequest request,
                UnityAction<AsyncResult<Result.GetItemWithSignatureResult>> callback
        )
		{
			var task = new GetItemWithSignatureTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.GetItemWithSignatureResult>(task.Result, task.Error));
        }

		public IFuture<Result.GetItemWithSignatureResult> GetItemWithSignatureFuture(
                Request.GetItemWithSignatureRequest request
        )
		{
			return new GetItemWithSignatureTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.GetItemWithSignatureResult> GetItemWithSignatureAsync(
                Request.GetItemWithSignatureRequest request
        )
		{
            AsyncResult<Result.GetItemWithSignatureResult> result = null;
			await GetItemWithSignature(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
    #else
		public GetItemWithSignatureTask GetItemWithSignatureAsync(
                Request.GetItemWithSignatureRequest request
        )
		{
			return new GetItemWithSignatureTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.GetItemWithSignatureResult> GetItemWithSignatureAsync(
                Request.GetItemWithSignatureRequest request
        )
		{
			var task = new GetItemWithSignatureTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class GetItemWithSignatureByUserIdTask : Gs2RestSessionTask<GetItemWithSignatureByUserIdRequest, GetItemWithSignatureByUserIdResult>
        {
            public GetItemWithSignatureByUserIdTask(IGs2Session session, RestSessionRequestFactory factory, GetItemWithSignatureByUserIdRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(GetItemWithSignatureByUserIdRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "inventory")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/{userId}/inventory/{inventoryName}/item/{itemName}/signature";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{inventoryName}", !string.IsNullOrEmpty(request.InventoryName) ? request.InventoryName.ToString() : "null");
                url = url.Replace("{userId}", !string.IsNullOrEmpty(request.UserId) ? request.UserId.ToString() : "null");
                url = url.Replace("{itemName}", !string.IsNullOrEmpty(request.ItemName) ? request.ItemName.ToString() : "null");

                var sessionRequest = Factory.Get(url);
                if (request.ContextStack != null)
                {
                    sessionRequest.AddQueryString("contextStack", request.ContextStack);
                }
                if (request.ItemSetName != null) {
                    sessionRequest.AddQueryString("itemSetName", $"{request.ItemSetName}");
                }
                if (request.KeyId != null) {
                    sessionRequest.AddQueryString("keyId", $"{request.KeyId}");
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
		public IEnumerator GetItemWithSignatureByUserId(
                Request.GetItemWithSignatureByUserIdRequest request,
                UnityAction<AsyncResult<Result.GetItemWithSignatureByUserIdResult>> callback
        )
		{
			var task = new GetItemWithSignatureByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.GetItemWithSignatureByUserIdResult>(task.Result, task.Error));
        }

		public IFuture<Result.GetItemWithSignatureByUserIdResult> GetItemWithSignatureByUserIdFuture(
                Request.GetItemWithSignatureByUserIdRequest request
        )
		{
			return new GetItemWithSignatureByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.GetItemWithSignatureByUserIdResult> GetItemWithSignatureByUserIdAsync(
                Request.GetItemWithSignatureByUserIdRequest request
        )
		{
            AsyncResult<Result.GetItemWithSignatureByUserIdResult> result = null;
			await GetItemWithSignatureByUserId(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
    #else
		public GetItemWithSignatureByUserIdTask GetItemWithSignatureByUserIdAsync(
                Request.GetItemWithSignatureByUserIdRequest request
        )
		{
			return new GetItemWithSignatureByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.GetItemWithSignatureByUserIdResult> GetItemWithSignatureByUserIdAsync(
                Request.GetItemWithSignatureByUserIdRequest request
        )
		{
			var task = new GetItemWithSignatureByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class AcquireItemSetByUserIdTask : Gs2RestSessionTask<AcquireItemSetByUserIdRequest, AcquireItemSetByUserIdResult>
        {
            public AcquireItemSetByUserIdTask(IGs2Session session, RestSessionRequestFactory factory, AcquireItemSetByUserIdRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(AcquireItemSetByUserIdRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "inventory")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/{userId}/inventory/{inventoryName}/item/{itemName}/acquire";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{inventoryName}", !string.IsNullOrEmpty(request.InventoryName) ? request.InventoryName.ToString() : "null");
                url = url.Replace("{itemName}", !string.IsNullOrEmpty(request.ItemName) ? request.ItemName.ToString() : "null");
                url = url.Replace("{userId}", !string.IsNullOrEmpty(request.UserId) ? request.UserId.ToString() : "null");

                var sessionRequest = Factory.Post(url);

                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);
                jsonWriter.WriteObjectStart();
                if (request.AcquireCount != null)
                {
                    jsonWriter.WritePropertyName("acquireCount");
                    jsonWriter.Write(request.AcquireCount.ToString());
                }
                if (request.ExpiresAt != null)
                {
                    jsonWriter.WritePropertyName("expiresAt");
                    jsonWriter.Write(request.ExpiresAt.ToString());
                }
                if (request.CreateNewItemSet != null)
                {
                    jsonWriter.WritePropertyName("createNewItemSet");
                    jsonWriter.Write(request.CreateNewItemSet.ToString());
                }
                if (request.ItemSetName != null)
                {
                    jsonWriter.WritePropertyName("itemSetName");
                    jsonWriter.Write(request.ItemSetName);
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
                if (request.DuplicationAvoider != null)
                {
                    sessionRequest.AddHeader("X-GS2-DUPLICATION-AVOIDER", request.DuplicationAvoider);
                }

                AddHeader(
                    Session.Credential,
                    sessionRequest
                );

                return sessionRequest;
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator AcquireItemSetByUserId(
                Request.AcquireItemSetByUserIdRequest request,
                UnityAction<AsyncResult<Result.AcquireItemSetByUserIdResult>> callback
        )
		{
			var task = new AcquireItemSetByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.AcquireItemSetByUserIdResult>(task.Result, task.Error));
        }

		public IFuture<Result.AcquireItemSetByUserIdResult> AcquireItemSetByUserIdFuture(
                Request.AcquireItemSetByUserIdRequest request
        )
		{
			return new AcquireItemSetByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.AcquireItemSetByUserIdResult> AcquireItemSetByUserIdAsync(
                Request.AcquireItemSetByUserIdRequest request
        )
		{
            AsyncResult<Result.AcquireItemSetByUserIdResult> result = null;
			await AcquireItemSetByUserId(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
    #else
		public AcquireItemSetByUserIdTask AcquireItemSetByUserIdAsync(
                Request.AcquireItemSetByUserIdRequest request
        )
		{
			return new AcquireItemSetByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.AcquireItemSetByUserIdResult> AcquireItemSetByUserIdAsync(
                Request.AcquireItemSetByUserIdRequest request
        )
		{
			var task = new AcquireItemSetByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class ConsumeItemSetTask : Gs2RestSessionTask<ConsumeItemSetRequest, ConsumeItemSetResult>
        {
            public ConsumeItemSetTask(IGs2Session session, RestSessionRequestFactory factory, ConsumeItemSetRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(ConsumeItemSetRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "inventory")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/me/inventory/{inventoryName}/item/{itemName}/consume";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{inventoryName}", !string.IsNullOrEmpty(request.InventoryName) ? request.InventoryName.ToString() : "null");
                url = url.Replace("{itemName}", !string.IsNullOrEmpty(request.ItemName) ? request.ItemName.ToString() : "null");

                var sessionRequest = Factory.Post(url);

                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);
                jsonWriter.WriteObjectStart();
                if (request.ConsumeCount != null)
                {
                    jsonWriter.WritePropertyName("consumeCount");
                    jsonWriter.Write(request.ConsumeCount.ToString());
                }
                if (request.ItemSetName != null)
                {
                    jsonWriter.WritePropertyName("itemSetName");
                    jsonWriter.Write(request.ItemSetName);
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
		public IEnumerator ConsumeItemSet(
                Request.ConsumeItemSetRequest request,
                UnityAction<AsyncResult<Result.ConsumeItemSetResult>> callback
        )
		{
			var task = new ConsumeItemSetTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.ConsumeItemSetResult>(task.Result, task.Error));
        }

		public IFuture<Result.ConsumeItemSetResult> ConsumeItemSetFuture(
                Request.ConsumeItemSetRequest request
        )
		{
			return new ConsumeItemSetTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.ConsumeItemSetResult> ConsumeItemSetAsync(
                Request.ConsumeItemSetRequest request
        )
		{
            AsyncResult<Result.ConsumeItemSetResult> result = null;
			await ConsumeItemSet(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
    #else
		public ConsumeItemSetTask ConsumeItemSetAsync(
                Request.ConsumeItemSetRequest request
        )
		{
			return new ConsumeItemSetTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.ConsumeItemSetResult> ConsumeItemSetAsync(
                Request.ConsumeItemSetRequest request
        )
		{
			var task = new ConsumeItemSetTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class ConsumeItemSetByUserIdTask : Gs2RestSessionTask<ConsumeItemSetByUserIdRequest, ConsumeItemSetByUserIdResult>
        {
            public ConsumeItemSetByUserIdTask(IGs2Session session, RestSessionRequestFactory factory, ConsumeItemSetByUserIdRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(ConsumeItemSetByUserIdRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "inventory")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/{userId}/inventory/{inventoryName}/item/{itemName}/consume";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{inventoryName}", !string.IsNullOrEmpty(request.InventoryName) ? request.InventoryName.ToString() : "null");
                url = url.Replace("{userId}", !string.IsNullOrEmpty(request.UserId) ? request.UserId.ToString() : "null");
                url = url.Replace("{itemName}", !string.IsNullOrEmpty(request.ItemName) ? request.ItemName.ToString() : "null");

                var sessionRequest = Factory.Post(url);

                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);
                jsonWriter.WriteObjectStart();
                if (request.ConsumeCount != null)
                {
                    jsonWriter.WritePropertyName("consumeCount");
                    jsonWriter.Write(request.ConsumeCount.ToString());
                }
                if (request.ItemSetName != null)
                {
                    jsonWriter.WritePropertyName("itemSetName");
                    jsonWriter.Write(request.ItemSetName);
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
                if (request.DuplicationAvoider != null)
                {
                    sessionRequest.AddHeader("X-GS2-DUPLICATION-AVOIDER", request.DuplicationAvoider);
                }

                AddHeader(
                    Session.Credential,
                    sessionRequest
                );

                return sessionRequest;
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator ConsumeItemSetByUserId(
                Request.ConsumeItemSetByUserIdRequest request,
                UnityAction<AsyncResult<Result.ConsumeItemSetByUserIdResult>> callback
        )
		{
			var task = new ConsumeItemSetByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.ConsumeItemSetByUserIdResult>(task.Result, task.Error));
        }

		public IFuture<Result.ConsumeItemSetByUserIdResult> ConsumeItemSetByUserIdFuture(
                Request.ConsumeItemSetByUserIdRequest request
        )
		{
			return new ConsumeItemSetByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.ConsumeItemSetByUserIdResult> ConsumeItemSetByUserIdAsync(
                Request.ConsumeItemSetByUserIdRequest request
        )
		{
            AsyncResult<Result.ConsumeItemSetByUserIdResult> result = null;
			await ConsumeItemSetByUserId(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
    #else
		public ConsumeItemSetByUserIdTask ConsumeItemSetByUserIdAsync(
                Request.ConsumeItemSetByUserIdRequest request
        )
		{
			return new ConsumeItemSetByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.ConsumeItemSetByUserIdResult> ConsumeItemSetByUserIdAsync(
                Request.ConsumeItemSetByUserIdRequest request
        )
		{
			var task = new ConsumeItemSetByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class DeleteItemSetByUserIdTask : Gs2RestSessionTask<DeleteItemSetByUserIdRequest, DeleteItemSetByUserIdResult>
        {
            public DeleteItemSetByUserIdTask(IGs2Session session, RestSessionRequestFactory factory, DeleteItemSetByUserIdRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DeleteItemSetByUserIdRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "inventory")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/{userId}/inventory/{inventoryName}/item/{itemName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{inventoryName}", !string.IsNullOrEmpty(request.InventoryName) ? request.InventoryName.ToString() : "null");
                url = url.Replace("{userId}", !string.IsNullOrEmpty(request.UserId) ? request.UserId.ToString() : "null");
                url = url.Replace("{itemName}", !string.IsNullOrEmpty(request.ItemName) ? request.ItemName.ToString() : "null");

                var sessionRequest = Factory.Delete(url);
                if (request.ContextStack != null)
                {
                    sessionRequest.AddQueryString("contextStack", request.ContextStack);
                }
                if (request.ItemSetName != null) {
                    sessionRequest.AddQueryString("itemSetName", $"{request.ItemSetName}");
                }

                if (request.RequestId != null)
                {
                    sessionRequest.AddHeader("X-GS2-REQUEST-ID", request.RequestId);
                }
                if (request.DuplicationAvoider != null)
                {
                    sessionRequest.AddHeader("X-GS2-DUPLICATION-AVOIDER", request.DuplicationAvoider);
                }

                AddHeader(
                    Session.Credential,
                    sessionRequest
                );

                return sessionRequest;
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator DeleteItemSetByUserId(
                Request.DeleteItemSetByUserIdRequest request,
                UnityAction<AsyncResult<Result.DeleteItemSetByUserIdResult>> callback
        )
		{
			var task = new DeleteItemSetByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.DeleteItemSetByUserIdResult>(task.Result, task.Error));
        }

		public IFuture<Result.DeleteItemSetByUserIdResult> DeleteItemSetByUserIdFuture(
                Request.DeleteItemSetByUserIdRequest request
        )
		{
			return new DeleteItemSetByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.DeleteItemSetByUserIdResult> DeleteItemSetByUserIdAsync(
                Request.DeleteItemSetByUserIdRequest request
        )
		{
            AsyncResult<Result.DeleteItemSetByUserIdResult> result = null;
			await DeleteItemSetByUserId(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
    #else
		public DeleteItemSetByUserIdTask DeleteItemSetByUserIdAsync(
                Request.DeleteItemSetByUserIdRequest request
        )
		{
			return new DeleteItemSetByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.DeleteItemSetByUserIdResult> DeleteItemSetByUserIdAsync(
                Request.DeleteItemSetByUserIdRequest request
        )
		{
			var task = new DeleteItemSetByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class AcquireItemSetByStampSheetTask : Gs2RestSessionTask<AcquireItemSetByStampSheetRequest, AcquireItemSetByStampSheetResult>
        {
            public AcquireItemSetByStampSheetTask(IGs2Session session, RestSessionRequestFactory factory, AcquireItemSetByStampSheetRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(AcquireItemSetByStampSheetRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "inventory")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/stamp/item/acquire";

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
		public IEnumerator AcquireItemSetByStampSheet(
                Request.AcquireItemSetByStampSheetRequest request,
                UnityAction<AsyncResult<Result.AcquireItemSetByStampSheetResult>> callback
        )
		{
			var task = new AcquireItemSetByStampSheetTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.AcquireItemSetByStampSheetResult>(task.Result, task.Error));
        }

		public IFuture<Result.AcquireItemSetByStampSheetResult> AcquireItemSetByStampSheetFuture(
                Request.AcquireItemSetByStampSheetRequest request
        )
		{
			return new AcquireItemSetByStampSheetTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.AcquireItemSetByStampSheetResult> AcquireItemSetByStampSheetAsync(
                Request.AcquireItemSetByStampSheetRequest request
        )
		{
            AsyncResult<Result.AcquireItemSetByStampSheetResult> result = null;
			await AcquireItemSetByStampSheet(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
    #else
		public AcquireItemSetByStampSheetTask AcquireItemSetByStampSheetAsync(
                Request.AcquireItemSetByStampSheetRequest request
        )
		{
			return new AcquireItemSetByStampSheetTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.AcquireItemSetByStampSheetResult> AcquireItemSetByStampSheetAsync(
                Request.AcquireItemSetByStampSheetRequest request
        )
		{
			var task = new AcquireItemSetByStampSheetTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class ConsumeItemSetByStampTaskTask : Gs2RestSessionTask<ConsumeItemSetByStampTaskRequest, ConsumeItemSetByStampTaskResult>
        {
            public ConsumeItemSetByStampTaskTask(IGs2Session session, RestSessionRequestFactory factory, ConsumeItemSetByStampTaskRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(ConsumeItemSetByStampTaskRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "inventory")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/stamp/item/consume";

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
		public IEnumerator ConsumeItemSetByStampTask(
                Request.ConsumeItemSetByStampTaskRequest request,
                UnityAction<AsyncResult<Result.ConsumeItemSetByStampTaskResult>> callback
        )
		{
			var task = new ConsumeItemSetByStampTaskTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.ConsumeItemSetByStampTaskResult>(task.Result, task.Error));
        }

		public IFuture<Result.ConsumeItemSetByStampTaskResult> ConsumeItemSetByStampTaskFuture(
                Request.ConsumeItemSetByStampTaskRequest request
        )
		{
			return new ConsumeItemSetByStampTaskTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.ConsumeItemSetByStampTaskResult> ConsumeItemSetByStampTaskAsync(
                Request.ConsumeItemSetByStampTaskRequest request
        )
		{
            AsyncResult<Result.ConsumeItemSetByStampTaskResult> result = null;
			await ConsumeItemSetByStampTask(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
    #else
		public ConsumeItemSetByStampTaskTask ConsumeItemSetByStampTaskAsync(
                Request.ConsumeItemSetByStampTaskRequest request
        )
		{
			return new ConsumeItemSetByStampTaskTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.ConsumeItemSetByStampTaskResult> ConsumeItemSetByStampTaskAsync(
                Request.ConsumeItemSetByStampTaskRequest request
        )
		{
			var task = new ConsumeItemSetByStampTaskTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class DescribeReferenceOfTask : Gs2RestSessionTask<DescribeReferenceOfRequest, DescribeReferenceOfResult>
        {
            public DescribeReferenceOfTask(IGs2Session session, RestSessionRequestFactory factory, DescribeReferenceOfRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DescribeReferenceOfRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "inventory")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/me/inventory/{inventoryName}/item/{itemName}/{itemSetName}/reference";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{inventoryName}", !string.IsNullOrEmpty(request.InventoryName) ? request.InventoryName.ToString() : "null");
                url = url.Replace("{itemName}", !string.IsNullOrEmpty(request.ItemName) ? request.ItemName.ToString() : "null");
                url = url.Replace("{itemSetName}", !string.IsNullOrEmpty(request.ItemSetName) ? request.ItemSetName.ToString() : "null");

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
		public IEnumerator DescribeReferenceOf(
                Request.DescribeReferenceOfRequest request,
                UnityAction<AsyncResult<Result.DescribeReferenceOfResult>> callback
        )
		{
			var task = new DescribeReferenceOfTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.DescribeReferenceOfResult>(task.Result, task.Error));
        }

		public IFuture<Result.DescribeReferenceOfResult> DescribeReferenceOfFuture(
                Request.DescribeReferenceOfRequest request
        )
		{
			return new DescribeReferenceOfTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.DescribeReferenceOfResult> DescribeReferenceOfAsync(
                Request.DescribeReferenceOfRequest request
        )
		{
            AsyncResult<Result.DescribeReferenceOfResult> result = null;
			await DescribeReferenceOf(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
    #else
		public DescribeReferenceOfTask DescribeReferenceOfAsync(
                Request.DescribeReferenceOfRequest request
        )
		{
			return new DescribeReferenceOfTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.DescribeReferenceOfResult> DescribeReferenceOfAsync(
                Request.DescribeReferenceOfRequest request
        )
		{
			var task = new DescribeReferenceOfTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class DescribeReferenceOfByUserIdTask : Gs2RestSessionTask<DescribeReferenceOfByUserIdRequest, DescribeReferenceOfByUserIdResult>
        {
            public DescribeReferenceOfByUserIdTask(IGs2Session session, RestSessionRequestFactory factory, DescribeReferenceOfByUserIdRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DescribeReferenceOfByUserIdRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "inventory")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/{userId}/inventory/{inventoryName}/item/{itemName}/{itemSetName}/reference";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{inventoryName}", !string.IsNullOrEmpty(request.InventoryName) ? request.InventoryName.ToString() : "null");
                url = url.Replace("{userId}", !string.IsNullOrEmpty(request.UserId) ? request.UserId.ToString() : "null");
                url = url.Replace("{itemName}", !string.IsNullOrEmpty(request.ItemName) ? request.ItemName.ToString() : "null");
                url = url.Replace("{itemSetName}", !string.IsNullOrEmpty(request.ItemSetName) ? request.ItemSetName.ToString() : "null");

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
		public IEnumerator DescribeReferenceOfByUserId(
                Request.DescribeReferenceOfByUserIdRequest request,
                UnityAction<AsyncResult<Result.DescribeReferenceOfByUserIdResult>> callback
        )
		{
			var task = new DescribeReferenceOfByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.DescribeReferenceOfByUserIdResult>(task.Result, task.Error));
        }

		public IFuture<Result.DescribeReferenceOfByUserIdResult> DescribeReferenceOfByUserIdFuture(
                Request.DescribeReferenceOfByUserIdRequest request
        )
		{
			return new DescribeReferenceOfByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.DescribeReferenceOfByUserIdResult> DescribeReferenceOfByUserIdAsync(
                Request.DescribeReferenceOfByUserIdRequest request
        )
		{
            AsyncResult<Result.DescribeReferenceOfByUserIdResult> result = null;
			await DescribeReferenceOfByUserId(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
    #else
		public DescribeReferenceOfByUserIdTask DescribeReferenceOfByUserIdAsync(
                Request.DescribeReferenceOfByUserIdRequest request
        )
		{
			return new DescribeReferenceOfByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.DescribeReferenceOfByUserIdResult> DescribeReferenceOfByUserIdAsync(
                Request.DescribeReferenceOfByUserIdRequest request
        )
		{
			var task = new DescribeReferenceOfByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class GetReferenceOfTask : Gs2RestSessionTask<GetReferenceOfRequest, GetReferenceOfResult>
        {
            public GetReferenceOfTask(IGs2Session session, RestSessionRequestFactory factory, GetReferenceOfRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(GetReferenceOfRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "inventory")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/me/inventory/{inventoryName}/item/{itemName}/{itemSetName}/reference/{referenceOf}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{inventoryName}", !string.IsNullOrEmpty(request.InventoryName) ? request.InventoryName.ToString() : "null");
                url = url.Replace("{itemName}", !string.IsNullOrEmpty(request.ItemName) ? request.ItemName.ToString() : "null");
                url = url.Replace("{itemSetName}", !string.IsNullOrEmpty(request.ItemSetName) ? request.ItemSetName.ToString() : "null");
                url = url.Replace("{referenceOf}", !string.IsNullOrEmpty(request.ReferenceOf) ? request.ReferenceOf.ToString() : "null");

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
		public IEnumerator GetReferenceOf(
                Request.GetReferenceOfRequest request,
                UnityAction<AsyncResult<Result.GetReferenceOfResult>> callback
        )
		{
			var task = new GetReferenceOfTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.GetReferenceOfResult>(task.Result, task.Error));
        }

		public IFuture<Result.GetReferenceOfResult> GetReferenceOfFuture(
                Request.GetReferenceOfRequest request
        )
		{
			return new GetReferenceOfTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.GetReferenceOfResult> GetReferenceOfAsync(
                Request.GetReferenceOfRequest request
        )
		{
            AsyncResult<Result.GetReferenceOfResult> result = null;
			await GetReferenceOf(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
    #else
		public GetReferenceOfTask GetReferenceOfAsync(
                Request.GetReferenceOfRequest request
        )
		{
			return new GetReferenceOfTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.GetReferenceOfResult> GetReferenceOfAsync(
                Request.GetReferenceOfRequest request
        )
		{
			var task = new GetReferenceOfTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class GetReferenceOfByUserIdTask : Gs2RestSessionTask<GetReferenceOfByUserIdRequest, GetReferenceOfByUserIdResult>
        {
            public GetReferenceOfByUserIdTask(IGs2Session session, RestSessionRequestFactory factory, GetReferenceOfByUserIdRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(GetReferenceOfByUserIdRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "inventory")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/{userId}/inventory/{inventoryName}/item/{itemName}/{itemSetName}/reference/{referenceOf}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{inventoryName}", !string.IsNullOrEmpty(request.InventoryName) ? request.InventoryName.ToString() : "null");
                url = url.Replace("{userId}", !string.IsNullOrEmpty(request.UserId) ? request.UserId.ToString() : "null");
                url = url.Replace("{itemName}", !string.IsNullOrEmpty(request.ItemName) ? request.ItemName.ToString() : "null");
                url = url.Replace("{itemSetName}", !string.IsNullOrEmpty(request.ItemSetName) ? request.ItemSetName.ToString() : "null");
                url = url.Replace("{referenceOf}", !string.IsNullOrEmpty(request.ReferenceOf) ? request.ReferenceOf.ToString() : "null");

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
		public IEnumerator GetReferenceOfByUserId(
                Request.GetReferenceOfByUserIdRequest request,
                UnityAction<AsyncResult<Result.GetReferenceOfByUserIdResult>> callback
        )
		{
			var task = new GetReferenceOfByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.GetReferenceOfByUserIdResult>(task.Result, task.Error));
        }

		public IFuture<Result.GetReferenceOfByUserIdResult> GetReferenceOfByUserIdFuture(
                Request.GetReferenceOfByUserIdRequest request
        )
		{
			return new GetReferenceOfByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.GetReferenceOfByUserIdResult> GetReferenceOfByUserIdAsync(
                Request.GetReferenceOfByUserIdRequest request
        )
		{
            AsyncResult<Result.GetReferenceOfByUserIdResult> result = null;
			await GetReferenceOfByUserId(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
    #else
		public GetReferenceOfByUserIdTask GetReferenceOfByUserIdAsync(
                Request.GetReferenceOfByUserIdRequest request
        )
		{
			return new GetReferenceOfByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.GetReferenceOfByUserIdResult> GetReferenceOfByUserIdAsync(
                Request.GetReferenceOfByUserIdRequest request
        )
		{
			var task = new GetReferenceOfByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class VerifyReferenceOfTask : Gs2RestSessionTask<VerifyReferenceOfRequest, VerifyReferenceOfResult>
        {
            public VerifyReferenceOfTask(IGs2Session session, RestSessionRequestFactory factory, VerifyReferenceOfRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(VerifyReferenceOfRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "inventory")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/me/inventory/{inventoryName}/item/{itemName}/{itemSetName}/reference/{referenceOf}/verify/{verifyType}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{inventoryName}", !string.IsNullOrEmpty(request.InventoryName) ? request.InventoryName.ToString() : "null");
                url = url.Replace("{itemName}", !string.IsNullOrEmpty(request.ItemName) ? request.ItemName.ToString() : "null");
                url = url.Replace("{itemSetName}", !string.IsNullOrEmpty(request.ItemSetName) ? request.ItemSetName.ToString() : "null");
                url = url.Replace("{referenceOf}", !string.IsNullOrEmpty(request.ReferenceOf) ? request.ReferenceOf.ToString() : "null");
                url = url.Replace("{verifyType}", !string.IsNullOrEmpty(request.VerifyType) ? request.VerifyType.ToString() : "null");

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
		public IEnumerator VerifyReferenceOf(
                Request.VerifyReferenceOfRequest request,
                UnityAction<AsyncResult<Result.VerifyReferenceOfResult>> callback
        )
		{
			var task = new VerifyReferenceOfTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.VerifyReferenceOfResult>(task.Result, task.Error));
        }

		public IFuture<Result.VerifyReferenceOfResult> VerifyReferenceOfFuture(
                Request.VerifyReferenceOfRequest request
        )
		{
			return new VerifyReferenceOfTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.VerifyReferenceOfResult> VerifyReferenceOfAsync(
                Request.VerifyReferenceOfRequest request
        )
		{
            AsyncResult<Result.VerifyReferenceOfResult> result = null;
			await VerifyReferenceOf(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
    #else
		public VerifyReferenceOfTask VerifyReferenceOfAsync(
                Request.VerifyReferenceOfRequest request
        )
		{
			return new VerifyReferenceOfTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.VerifyReferenceOfResult> VerifyReferenceOfAsync(
                Request.VerifyReferenceOfRequest request
        )
		{
			var task = new VerifyReferenceOfTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class VerifyReferenceOfByUserIdTask : Gs2RestSessionTask<VerifyReferenceOfByUserIdRequest, VerifyReferenceOfByUserIdResult>
        {
            public VerifyReferenceOfByUserIdTask(IGs2Session session, RestSessionRequestFactory factory, VerifyReferenceOfByUserIdRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(VerifyReferenceOfByUserIdRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "inventory")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/{userId}/inventory/{inventoryName}/item/{itemName}/{itemSetName}/reference/{referenceOf}/verify/{verifyType}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{inventoryName}", !string.IsNullOrEmpty(request.InventoryName) ? request.InventoryName.ToString() : "null");
                url = url.Replace("{userId}", !string.IsNullOrEmpty(request.UserId) ? request.UserId.ToString() : "null");
                url = url.Replace("{itemName}", !string.IsNullOrEmpty(request.ItemName) ? request.ItemName.ToString() : "null");
                url = url.Replace("{itemSetName}", !string.IsNullOrEmpty(request.ItemSetName) ? request.ItemSetName.ToString() : "null");
                url = url.Replace("{referenceOf}", !string.IsNullOrEmpty(request.ReferenceOf) ? request.ReferenceOf.ToString() : "null");
                url = url.Replace("{verifyType}", !string.IsNullOrEmpty(request.VerifyType) ? request.VerifyType.ToString() : "null");

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
                if (request.DuplicationAvoider != null)
                {
                    sessionRequest.AddHeader("X-GS2-DUPLICATION-AVOIDER", request.DuplicationAvoider);
                }

                AddHeader(
                    Session.Credential,
                    sessionRequest
                );

                return sessionRequest;
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator VerifyReferenceOfByUserId(
                Request.VerifyReferenceOfByUserIdRequest request,
                UnityAction<AsyncResult<Result.VerifyReferenceOfByUserIdResult>> callback
        )
		{
			var task = new VerifyReferenceOfByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.VerifyReferenceOfByUserIdResult>(task.Result, task.Error));
        }

		public IFuture<Result.VerifyReferenceOfByUserIdResult> VerifyReferenceOfByUserIdFuture(
                Request.VerifyReferenceOfByUserIdRequest request
        )
		{
			return new VerifyReferenceOfByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.VerifyReferenceOfByUserIdResult> VerifyReferenceOfByUserIdAsync(
                Request.VerifyReferenceOfByUserIdRequest request
        )
		{
            AsyncResult<Result.VerifyReferenceOfByUserIdResult> result = null;
			await VerifyReferenceOfByUserId(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
    #else
		public VerifyReferenceOfByUserIdTask VerifyReferenceOfByUserIdAsync(
                Request.VerifyReferenceOfByUserIdRequest request
        )
		{
			return new VerifyReferenceOfByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.VerifyReferenceOfByUserIdResult> VerifyReferenceOfByUserIdAsync(
                Request.VerifyReferenceOfByUserIdRequest request
        )
		{
			var task = new VerifyReferenceOfByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class AddReferenceOfTask : Gs2RestSessionTask<AddReferenceOfRequest, AddReferenceOfResult>
        {
            public AddReferenceOfTask(IGs2Session session, RestSessionRequestFactory factory, AddReferenceOfRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(AddReferenceOfRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "inventory")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/me/inventory/{inventoryName}/item/{itemName}/{itemSetName}/reference";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{inventoryName}", !string.IsNullOrEmpty(request.InventoryName) ? request.InventoryName.ToString() : "null");
                url = url.Replace("{itemName}", !string.IsNullOrEmpty(request.ItemName) ? request.ItemName.ToString() : "null");
                url = url.Replace("{itemSetName}", !string.IsNullOrEmpty(request.ItemSetName) ? request.ItemSetName.ToString() : "null");

                var sessionRequest = Factory.Post(url);

                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);
                jsonWriter.WriteObjectStart();
                if (request.ReferenceOf != null)
                {
                    jsonWriter.WritePropertyName("referenceOf");
                    jsonWriter.Write(request.ReferenceOf);
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
		public IEnumerator AddReferenceOf(
                Request.AddReferenceOfRequest request,
                UnityAction<AsyncResult<Result.AddReferenceOfResult>> callback
        )
		{
			var task = new AddReferenceOfTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.AddReferenceOfResult>(task.Result, task.Error));
        }

		public IFuture<Result.AddReferenceOfResult> AddReferenceOfFuture(
                Request.AddReferenceOfRequest request
        )
		{
			return new AddReferenceOfTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.AddReferenceOfResult> AddReferenceOfAsync(
                Request.AddReferenceOfRequest request
        )
		{
            AsyncResult<Result.AddReferenceOfResult> result = null;
			await AddReferenceOf(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
    #else
		public AddReferenceOfTask AddReferenceOfAsync(
                Request.AddReferenceOfRequest request
        )
		{
			return new AddReferenceOfTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.AddReferenceOfResult> AddReferenceOfAsync(
                Request.AddReferenceOfRequest request
        )
		{
			var task = new AddReferenceOfTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class AddReferenceOfByUserIdTask : Gs2RestSessionTask<AddReferenceOfByUserIdRequest, AddReferenceOfByUserIdResult>
        {
            public AddReferenceOfByUserIdTask(IGs2Session session, RestSessionRequestFactory factory, AddReferenceOfByUserIdRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(AddReferenceOfByUserIdRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "inventory")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/{userId}/inventory/{inventoryName}/item/{itemName}/{itemSetName}/reference";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{inventoryName}", !string.IsNullOrEmpty(request.InventoryName) ? request.InventoryName.ToString() : "null");
                url = url.Replace("{userId}", !string.IsNullOrEmpty(request.UserId) ? request.UserId.ToString() : "null");
                url = url.Replace("{itemName}", !string.IsNullOrEmpty(request.ItemName) ? request.ItemName.ToString() : "null");
                url = url.Replace("{itemSetName}", !string.IsNullOrEmpty(request.ItemSetName) ? request.ItemSetName.ToString() : "null");

                var sessionRequest = Factory.Post(url);

                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);
                jsonWriter.WriteObjectStart();
                if (request.ReferenceOf != null)
                {
                    jsonWriter.WritePropertyName("referenceOf");
                    jsonWriter.Write(request.ReferenceOf);
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
                if (request.DuplicationAvoider != null)
                {
                    sessionRequest.AddHeader("X-GS2-DUPLICATION-AVOIDER", request.DuplicationAvoider);
                }

                AddHeader(
                    Session.Credential,
                    sessionRequest
                );

                return sessionRequest;
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator AddReferenceOfByUserId(
                Request.AddReferenceOfByUserIdRequest request,
                UnityAction<AsyncResult<Result.AddReferenceOfByUserIdResult>> callback
        )
		{
			var task = new AddReferenceOfByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.AddReferenceOfByUserIdResult>(task.Result, task.Error));
        }

		public IFuture<Result.AddReferenceOfByUserIdResult> AddReferenceOfByUserIdFuture(
                Request.AddReferenceOfByUserIdRequest request
        )
		{
			return new AddReferenceOfByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.AddReferenceOfByUserIdResult> AddReferenceOfByUserIdAsync(
                Request.AddReferenceOfByUserIdRequest request
        )
		{
            AsyncResult<Result.AddReferenceOfByUserIdResult> result = null;
			await AddReferenceOfByUserId(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
    #else
		public AddReferenceOfByUserIdTask AddReferenceOfByUserIdAsync(
                Request.AddReferenceOfByUserIdRequest request
        )
		{
			return new AddReferenceOfByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.AddReferenceOfByUserIdResult> AddReferenceOfByUserIdAsync(
                Request.AddReferenceOfByUserIdRequest request
        )
		{
			var task = new AddReferenceOfByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class DeleteReferenceOfTask : Gs2RestSessionTask<DeleteReferenceOfRequest, DeleteReferenceOfResult>
        {
            public DeleteReferenceOfTask(IGs2Session session, RestSessionRequestFactory factory, DeleteReferenceOfRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DeleteReferenceOfRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "inventory")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/me/inventory/{inventoryName}/item/{itemName}/{itemSetName}/reference/{referenceOf}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{inventoryName}", !string.IsNullOrEmpty(request.InventoryName) ? request.InventoryName.ToString() : "null");
                url = url.Replace("{itemName}", !string.IsNullOrEmpty(request.ItemName) ? request.ItemName.ToString() : "null");
                url = url.Replace("{itemSetName}", !string.IsNullOrEmpty(request.ItemSetName) ? request.ItemSetName.ToString() : "null");
                url = url.Replace("{referenceOf}", !string.IsNullOrEmpty(request.ReferenceOf) ? request.ReferenceOf.ToString() : "null");

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
		public IEnumerator DeleteReferenceOf(
                Request.DeleteReferenceOfRequest request,
                UnityAction<AsyncResult<Result.DeleteReferenceOfResult>> callback
        )
		{
			var task = new DeleteReferenceOfTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.DeleteReferenceOfResult>(task.Result, task.Error));
        }

		public IFuture<Result.DeleteReferenceOfResult> DeleteReferenceOfFuture(
                Request.DeleteReferenceOfRequest request
        )
		{
			return new DeleteReferenceOfTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.DeleteReferenceOfResult> DeleteReferenceOfAsync(
                Request.DeleteReferenceOfRequest request
        )
		{
            AsyncResult<Result.DeleteReferenceOfResult> result = null;
			await DeleteReferenceOf(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
    #else
		public DeleteReferenceOfTask DeleteReferenceOfAsync(
                Request.DeleteReferenceOfRequest request
        )
		{
			return new DeleteReferenceOfTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.DeleteReferenceOfResult> DeleteReferenceOfAsync(
                Request.DeleteReferenceOfRequest request
        )
		{
			var task = new DeleteReferenceOfTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class DeleteReferenceOfByUserIdTask : Gs2RestSessionTask<DeleteReferenceOfByUserIdRequest, DeleteReferenceOfByUserIdResult>
        {
            public DeleteReferenceOfByUserIdTask(IGs2Session session, RestSessionRequestFactory factory, DeleteReferenceOfByUserIdRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DeleteReferenceOfByUserIdRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "inventory")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/{userId}/inventory/{inventoryName}/item/{itemName}/{itemSetName}/reference/{referenceOf}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{inventoryName}", !string.IsNullOrEmpty(request.InventoryName) ? request.InventoryName.ToString() : "null");
                url = url.Replace("{userId}", !string.IsNullOrEmpty(request.UserId) ? request.UserId.ToString() : "null");
                url = url.Replace("{itemName}", !string.IsNullOrEmpty(request.ItemName) ? request.ItemName.ToString() : "null");
                url = url.Replace("{itemSetName}", !string.IsNullOrEmpty(request.ItemSetName) ? request.ItemSetName.ToString() : "null");
                url = url.Replace("{referenceOf}", !string.IsNullOrEmpty(request.ReferenceOf) ? request.ReferenceOf.ToString() : "null");

                var sessionRequest = Factory.Delete(url);
                if (request.ContextStack != null)
                {
                    sessionRequest.AddQueryString("contextStack", request.ContextStack);
                }

                if (request.RequestId != null)
                {
                    sessionRequest.AddHeader("X-GS2-REQUEST-ID", request.RequestId);
                }
                if (request.DuplicationAvoider != null)
                {
                    sessionRequest.AddHeader("X-GS2-DUPLICATION-AVOIDER", request.DuplicationAvoider);
                }

                AddHeader(
                    Session.Credential,
                    sessionRequest
                );

                return sessionRequest;
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator DeleteReferenceOfByUserId(
                Request.DeleteReferenceOfByUserIdRequest request,
                UnityAction<AsyncResult<Result.DeleteReferenceOfByUserIdResult>> callback
        )
		{
			var task = new DeleteReferenceOfByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.DeleteReferenceOfByUserIdResult>(task.Result, task.Error));
        }

		public IFuture<Result.DeleteReferenceOfByUserIdResult> DeleteReferenceOfByUserIdFuture(
                Request.DeleteReferenceOfByUserIdRequest request
        )
		{
			return new DeleteReferenceOfByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.DeleteReferenceOfByUserIdResult> DeleteReferenceOfByUserIdAsync(
                Request.DeleteReferenceOfByUserIdRequest request
        )
		{
            AsyncResult<Result.DeleteReferenceOfByUserIdResult> result = null;
			await DeleteReferenceOfByUserId(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
    #else
		public DeleteReferenceOfByUserIdTask DeleteReferenceOfByUserIdAsync(
                Request.DeleteReferenceOfByUserIdRequest request
        )
		{
			return new DeleteReferenceOfByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.DeleteReferenceOfByUserIdResult> DeleteReferenceOfByUserIdAsync(
                Request.DeleteReferenceOfByUserIdRequest request
        )
		{
			var task = new DeleteReferenceOfByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class AddReferenceOfItemSetByStampSheetTask : Gs2RestSessionTask<AddReferenceOfItemSetByStampSheetRequest, AddReferenceOfItemSetByStampSheetResult>
        {
            public AddReferenceOfItemSetByStampSheetTask(IGs2Session session, RestSessionRequestFactory factory, AddReferenceOfItemSetByStampSheetRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(AddReferenceOfItemSetByStampSheetRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "inventory")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/stamp/item/reference/add";

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
		public IEnumerator AddReferenceOfItemSetByStampSheet(
                Request.AddReferenceOfItemSetByStampSheetRequest request,
                UnityAction<AsyncResult<Result.AddReferenceOfItemSetByStampSheetResult>> callback
        )
		{
			var task = new AddReferenceOfItemSetByStampSheetTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.AddReferenceOfItemSetByStampSheetResult>(task.Result, task.Error));
        }

		public IFuture<Result.AddReferenceOfItemSetByStampSheetResult> AddReferenceOfItemSetByStampSheetFuture(
                Request.AddReferenceOfItemSetByStampSheetRequest request
        )
		{
			return new AddReferenceOfItemSetByStampSheetTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.AddReferenceOfItemSetByStampSheetResult> AddReferenceOfItemSetByStampSheetAsync(
                Request.AddReferenceOfItemSetByStampSheetRequest request
        )
		{
            AsyncResult<Result.AddReferenceOfItemSetByStampSheetResult> result = null;
			await AddReferenceOfItemSetByStampSheet(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
    #else
		public AddReferenceOfItemSetByStampSheetTask AddReferenceOfItemSetByStampSheetAsync(
                Request.AddReferenceOfItemSetByStampSheetRequest request
        )
		{
			return new AddReferenceOfItemSetByStampSheetTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.AddReferenceOfItemSetByStampSheetResult> AddReferenceOfItemSetByStampSheetAsync(
                Request.AddReferenceOfItemSetByStampSheetRequest request
        )
		{
			var task = new AddReferenceOfItemSetByStampSheetTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class DeleteReferenceOfItemSetByStampSheetTask : Gs2RestSessionTask<DeleteReferenceOfItemSetByStampSheetRequest, DeleteReferenceOfItemSetByStampSheetResult>
        {
            public DeleteReferenceOfItemSetByStampSheetTask(IGs2Session session, RestSessionRequestFactory factory, DeleteReferenceOfItemSetByStampSheetRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DeleteReferenceOfItemSetByStampSheetRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "inventory")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/stamp/item/reference/delete";

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
		public IEnumerator DeleteReferenceOfItemSetByStampSheet(
                Request.DeleteReferenceOfItemSetByStampSheetRequest request,
                UnityAction<AsyncResult<Result.DeleteReferenceOfItemSetByStampSheetResult>> callback
        )
		{
			var task = new DeleteReferenceOfItemSetByStampSheetTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.DeleteReferenceOfItemSetByStampSheetResult>(task.Result, task.Error));
        }

		public IFuture<Result.DeleteReferenceOfItemSetByStampSheetResult> DeleteReferenceOfItemSetByStampSheetFuture(
                Request.DeleteReferenceOfItemSetByStampSheetRequest request
        )
		{
			return new DeleteReferenceOfItemSetByStampSheetTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.DeleteReferenceOfItemSetByStampSheetResult> DeleteReferenceOfItemSetByStampSheetAsync(
                Request.DeleteReferenceOfItemSetByStampSheetRequest request
        )
		{
            AsyncResult<Result.DeleteReferenceOfItemSetByStampSheetResult> result = null;
			await DeleteReferenceOfItemSetByStampSheet(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
    #else
		public DeleteReferenceOfItemSetByStampSheetTask DeleteReferenceOfItemSetByStampSheetAsync(
                Request.DeleteReferenceOfItemSetByStampSheetRequest request
        )
		{
			return new DeleteReferenceOfItemSetByStampSheetTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.DeleteReferenceOfItemSetByStampSheetResult> DeleteReferenceOfItemSetByStampSheetAsync(
                Request.DeleteReferenceOfItemSetByStampSheetRequest request
        )
		{
			var task = new DeleteReferenceOfItemSetByStampSheetTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class VerifyReferenceOfByStampTaskTask : Gs2RestSessionTask<VerifyReferenceOfByStampTaskRequest, VerifyReferenceOfByStampTaskResult>
        {
            public VerifyReferenceOfByStampTaskTask(IGs2Session session, RestSessionRequestFactory factory, VerifyReferenceOfByStampTaskRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(VerifyReferenceOfByStampTaskRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "inventory")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/stamp/item/verify";

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
		public IEnumerator VerifyReferenceOfByStampTask(
                Request.VerifyReferenceOfByStampTaskRequest request,
                UnityAction<AsyncResult<Result.VerifyReferenceOfByStampTaskResult>> callback
        )
		{
			var task = new VerifyReferenceOfByStampTaskTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.VerifyReferenceOfByStampTaskResult>(task.Result, task.Error));
        }

		public IFuture<Result.VerifyReferenceOfByStampTaskResult> VerifyReferenceOfByStampTaskFuture(
                Request.VerifyReferenceOfByStampTaskRequest request
        )
		{
			return new VerifyReferenceOfByStampTaskTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.VerifyReferenceOfByStampTaskResult> VerifyReferenceOfByStampTaskAsync(
                Request.VerifyReferenceOfByStampTaskRequest request
        )
		{
            AsyncResult<Result.VerifyReferenceOfByStampTaskResult> result = null;
			await VerifyReferenceOfByStampTask(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
    #else
		public VerifyReferenceOfByStampTaskTask VerifyReferenceOfByStampTaskAsync(
                Request.VerifyReferenceOfByStampTaskRequest request
        )
		{
			return new VerifyReferenceOfByStampTaskTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.VerifyReferenceOfByStampTaskResult> VerifyReferenceOfByStampTaskAsync(
                Request.VerifyReferenceOfByStampTaskRequest request
        )
		{
			var task = new VerifyReferenceOfByStampTaskTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif
	}
}