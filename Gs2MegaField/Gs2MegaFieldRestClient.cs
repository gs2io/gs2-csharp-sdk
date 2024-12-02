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
using Gs2.Gs2MegaField.Request;
using Gs2.Gs2MegaField.Result;
using Gs2.Util.LitJson;

namespace Gs2.Gs2MegaField
{
	public class Gs2MegaFieldRestClient : AbstractGs2Client
	{
#if UNITY_2017_1_OR_NEWER
		private readonly CertificateHandler _certificateHandler;
#endif

		public static string Endpoint = "mega-field";

        protected Gs2RestSession Gs2RestSession => (Gs2RestSession) Gs2Session;

		public Gs2MegaFieldRestClient(Gs2RestSession Gs2RestSession) : base(Gs2RestSession)
		{

		}

#if UNITY_2017_1_OR_NEWER
		public Gs2MegaFieldRestClient(Gs2RestSession gs2RestSession, CertificateHandler certificateHandler) : base(gs2RestSession)
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
                    .Replace("{service}", "mega-field")
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
                if (request.DryRun)
                {
                    sessionRequest.AddHeader("X-GS2-DRY-RUN", "true");
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
                    .Replace("{service}", "mega-field")
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
                if (request.DryRun)
                {
                    sessionRequest.AddHeader("X-GS2-DRY-RUN", "true");
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
                    .Replace("{service}", "mega-field")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/status";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");

                var sessionRequest = Factory.Get(url);
                if (request.ContextStack != null)
                {
                    sessionRequest.AddQueryString("contextStack", request.ContextStack);
                }
                if (request.DryRun)
                {
                    sessionRequest.AddHeader("X-GS2-DRY-RUN", "true");
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
                    .Replace("{service}", "mega-field")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");

                var sessionRequest = Factory.Get(url);
                if (request.ContextStack != null)
                {
                    sessionRequest.AddQueryString("contextStack", request.ContextStack);
                }
                if (request.DryRun)
                {
                    sessionRequest.AddHeader("X-GS2-DRY-RUN", "true");
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
                    .Replace("{service}", "mega-field")
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
                if (request.DryRun)
                {
                    sessionRequest.AddHeader("X-GS2-DRY-RUN", "true");
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
                    .Replace("{service}", "mega-field")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");

                var sessionRequest = Factory.Delete(url);
                if (request.ContextStack != null)
                {
                    sessionRequest.AddQueryString("contextStack", request.ContextStack);
                }
                if (request.DryRun)
                {
                    sessionRequest.AddHeader("X-GS2-DRY-RUN", "true");
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


        public class DescribeAreaModelsTask : Gs2RestSessionTask<DescribeAreaModelsRequest, DescribeAreaModelsResult>
        {
            public DescribeAreaModelsTask(IGs2Session session, RestSessionRequestFactory factory, DescribeAreaModelsRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DescribeAreaModelsRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "mega-field")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/area";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");

                var sessionRequest = Factory.Get(url);
                if (request.ContextStack != null)
                {
                    sessionRequest.AddQueryString("contextStack", request.ContextStack);
                }
                if (request.DryRun)
                {
                    sessionRequest.AddHeader("X-GS2-DRY-RUN", "true");
                }

                AddHeader(
                    Session.Credential,
                    sessionRequest
                );

                return sessionRequest;
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator DescribeAreaModels(
                Request.DescribeAreaModelsRequest request,
                UnityAction<AsyncResult<Result.DescribeAreaModelsResult>> callback
        )
		{
			var task = new DescribeAreaModelsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.DescribeAreaModelsResult>(task.Result, task.Error));
        }

		public IFuture<Result.DescribeAreaModelsResult> DescribeAreaModelsFuture(
                Request.DescribeAreaModelsRequest request
        )
		{
			return new DescribeAreaModelsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.DescribeAreaModelsResult> DescribeAreaModelsAsync(
                Request.DescribeAreaModelsRequest request
        )
		{
            AsyncResult<Result.DescribeAreaModelsResult> result = null;
			await DescribeAreaModels(
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
		public DescribeAreaModelsTask DescribeAreaModelsAsync(
                Request.DescribeAreaModelsRequest request
        )
		{
			return new DescribeAreaModelsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.DescribeAreaModelsResult> DescribeAreaModelsAsync(
                Request.DescribeAreaModelsRequest request
        )
		{
			var task = new DescribeAreaModelsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class GetAreaModelTask : Gs2RestSessionTask<GetAreaModelRequest, GetAreaModelResult>
        {
            public GetAreaModelTask(IGs2Session session, RestSessionRequestFactory factory, GetAreaModelRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(GetAreaModelRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "mega-field")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/area/{areaModelName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{areaModelName}", !string.IsNullOrEmpty(request.AreaModelName) ? request.AreaModelName.ToString() : "null");

                var sessionRequest = Factory.Get(url);
                if (request.ContextStack != null)
                {
                    sessionRequest.AddQueryString("contextStack", request.ContextStack);
                }
                if (request.DryRun)
                {
                    sessionRequest.AddHeader("X-GS2-DRY-RUN", "true");
                }

                AddHeader(
                    Session.Credential,
                    sessionRequest
                );

                return sessionRequest;
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator GetAreaModel(
                Request.GetAreaModelRequest request,
                UnityAction<AsyncResult<Result.GetAreaModelResult>> callback
        )
		{
			var task = new GetAreaModelTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.GetAreaModelResult>(task.Result, task.Error));
        }

		public IFuture<Result.GetAreaModelResult> GetAreaModelFuture(
                Request.GetAreaModelRequest request
        )
		{
			return new GetAreaModelTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.GetAreaModelResult> GetAreaModelAsync(
                Request.GetAreaModelRequest request
        )
		{
            AsyncResult<Result.GetAreaModelResult> result = null;
			await GetAreaModel(
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
		public GetAreaModelTask GetAreaModelAsync(
                Request.GetAreaModelRequest request
        )
		{
			return new GetAreaModelTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.GetAreaModelResult> GetAreaModelAsync(
                Request.GetAreaModelRequest request
        )
		{
			var task = new GetAreaModelTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class DescribeAreaModelMastersTask : Gs2RestSessionTask<DescribeAreaModelMastersRequest, DescribeAreaModelMastersResult>
        {
            public DescribeAreaModelMastersTask(IGs2Session session, RestSessionRequestFactory factory, DescribeAreaModelMastersRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DescribeAreaModelMastersRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "mega-field")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/master/area";

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
                if (request.DryRun)
                {
                    sessionRequest.AddHeader("X-GS2-DRY-RUN", "true");
                }

                AddHeader(
                    Session.Credential,
                    sessionRequest
                );

                return sessionRequest;
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator DescribeAreaModelMasters(
                Request.DescribeAreaModelMastersRequest request,
                UnityAction<AsyncResult<Result.DescribeAreaModelMastersResult>> callback
        )
		{
			var task = new DescribeAreaModelMastersTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.DescribeAreaModelMastersResult>(task.Result, task.Error));
        }

		public IFuture<Result.DescribeAreaModelMastersResult> DescribeAreaModelMastersFuture(
                Request.DescribeAreaModelMastersRequest request
        )
		{
			return new DescribeAreaModelMastersTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.DescribeAreaModelMastersResult> DescribeAreaModelMastersAsync(
                Request.DescribeAreaModelMastersRequest request
        )
		{
            AsyncResult<Result.DescribeAreaModelMastersResult> result = null;
			await DescribeAreaModelMasters(
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
		public DescribeAreaModelMastersTask DescribeAreaModelMastersAsync(
                Request.DescribeAreaModelMastersRequest request
        )
		{
			return new DescribeAreaModelMastersTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.DescribeAreaModelMastersResult> DescribeAreaModelMastersAsync(
                Request.DescribeAreaModelMastersRequest request
        )
		{
			var task = new DescribeAreaModelMastersTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class CreateAreaModelMasterTask : Gs2RestSessionTask<CreateAreaModelMasterRequest, CreateAreaModelMasterResult>
        {
            public CreateAreaModelMasterTask(IGs2Session session, RestSessionRequestFactory factory, CreateAreaModelMasterRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(CreateAreaModelMasterRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "mega-field")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/master/area";

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
                if (request.DryRun)
                {
                    sessionRequest.AddHeader("X-GS2-DRY-RUN", "true");
                }

                AddHeader(
                    Session.Credential,
                    sessionRequest
                );

                return sessionRequest;
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator CreateAreaModelMaster(
                Request.CreateAreaModelMasterRequest request,
                UnityAction<AsyncResult<Result.CreateAreaModelMasterResult>> callback
        )
		{
			var task = new CreateAreaModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.CreateAreaModelMasterResult>(task.Result, task.Error));
        }

		public IFuture<Result.CreateAreaModelMasterResult> CreateAreaModelMasterFuture(
                Request.CreateAreaModelMasterRequest request
        )
		{
			return new CreateAreaModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.CreateAreaModelMasterResult> CreateAreaModelMasterAsync(
                Request.CreateAreaModelMasterRequest request
        )
		{
            AsyncResult<Result.CreateAreaModelMasterResult> result = null;
			await CreateAreaModelMaster(
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
		public CreateAreaModelMasterTask CreateAreaModelMasterAsync(
                Request.CreateAreaModelMasterRequest request
        )
		{
			return new CreateAreaModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.CreateAreaModelMasterResult> CreateAreaModelMasterAsync(
                Request.CreateAreaModelMasterRequest request
        )
		{
			var task = new CreateAreaModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class GetAreaModelMasterTask : Gs2RestSessionTask<GetAreaModelMasterRequest, GetAreaModelMasterResult>
        {
            public GetAreaModelMasterTask(IGs2Session session, RestSessionRequestFactory factory, GetAreaModelMasterRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(GetAreaModelMasterRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "mega-field")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/master/area/{areaModelName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{areaModelName}", !string.IsNullOrEmpty(request.AreaModelName) ? request.AreaModelName.ToString() : "null");

                var sessionRequest = Factory.Get(url);
                if (request.ContextStack != null)
                {
                    sessionRequest.AddQueryString("contextStack", request.ContextStack);
                }
                if (request.DryRun)
                {
                    sessionRequest.AddHeader("X-GS2-DRY-RUN", "true");
                }

                AddHeader(
                    Session.Credential,
                    sessionRequest
                );

                return sessionRequest;
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator GetAreaModelMaster(
                Request.GetAreaModelMasterRequest request,
                UnityAction<AsyncResult<Result.GetAreaModelMasterResult>> callback
        )
		{
			var task = new GetAreaModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.GetAreaModelMasterResult>(task.Result, task.Error));
        }

		public IFuture<Result.GetAreaModelMasterResult> GetAreaModelMasterFuture(
                Request.GetAreaModelMasterRequest request
        )
		{
			return new GetAreaModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.GetAreaModelMasterResult> GetAreaModelMasterAsync(
                Request.GetAreaModelMasterRequest request
        )
		{
            AsyncResult<Result.GetAreaModelMasterResult> result = null;
			await GetAreaModelMaster(
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
		public GetAreaModelMasterTask GetAreaModelMasterAsync(
                Request.GetAreaModelMasterRequest request
        )
		{
			return new GetAreaModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.GetAreaModelMasterResult> GetAreaModelMasterAsync(
                Request.GetAreaModelMasterRequest request
        )
		{
			var task = new GetAreaModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class UpdateAreaModelMasterTask : Gs2RestSessionTask<UpdateAreaModelMasterRequest, UpdateAreaModelMasterResult>
        {
            public UpdateAreaModelMasterTask(IGs2Session session, RestSessionRequestFactory factory, UpdateAreaModelMasterRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(UpdateAreaModelMasterRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "mega-field")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/master/area/{areaModelName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{areaModelName}", !string.IsNullOrEmpty(request.AreaModelName) ? request.AreaModelName.ToString() : "null");

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
                if (request.DryRun)
                {
                    sessionRequest.AddHeader("X-GS2-DRY-RUN", "true");
                }

                AddHeader(
                    Session.Credential,
                    sessionRequest
                );

                return sessionRequest;
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator UpdateAreaModelMaster(
                Request.UpdateAreaModelMasterRequest request,
                UnityAction<AsyncResult<Result.UpdateAreaModelMasterResult>> callback
        )
		{
			var task = new UpdateAreaModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.UpdateAreaModelMasterResult>(task.Result, task.Error));
        }

		public IFuture<Result.UpdateAreaModelMasterResult> UpdateAreaModelMasterFuture(
                Request.UpdateAreaModelMasterRequest request
        )
		{
			return new UpdateAreaModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.UpdateAreaModelMasterResult> UpdateAreaModelMasterAsync(
                Request.UpdateAreaModelMasterRequest request
        )
		{
            AsyncResult<Result.UpdateAreaModelMasterResult> result = null;
			await UpdateAreaModelMaster(
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
		public UpdateAreaModelMasterTask UpdateAreaModelMasterAsync(
                Request.UpdateAreaModelMasterRequest request
        )
		{
			return new UpdateAreaModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.UpdateAreaModelMasterResult> UpdateAreaModelMasterAsync(
                Request.UpdateAreaModelMasterRequest request
        )
		{
			var task = new UpdateAreaModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class DeleteAreaModelMasterTask : Gs2RestSessionTask<DeleteAreaModelMasterRequest, DeleteAreaModelMasterResult>
        {
            public DeleteAreaModelMasterTask(IGs2Session session, RestSessionRequestFactory factory, DeleteAreaModelMasterRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DeleteAreaModelMasterRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "mega-field")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/master/area/{areaModelName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{areaModelName}", !string.IsNullOrEmpty(request.AreaModelName) ? request.AreaModelName.ToString() : "null");

                var sessionRequest = Factory.Delete(url);
                if (request.ContextStack != null)
                {
                    sessionRequest.AddQueryString("contextStack", request.ContextStack);
                }
                if (request.DryRun)
                {
                    sessionRequest.AddHeader("X-GS2-DRY-RUN", "true");
                }

                AddHeader(
                    Session.Credential,
                    sessionRequest
                );

                return sessionRequest;
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator DeleteAreaModelMaster(
                Request.DeleteAreaModelMasterRequest request,
                UnityAction<AsyncResult<Result.DeleteAreaModelMasterResult>> callback
        )
		{
			var task = new DeleteAreaModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.DeleteAreaModelMasterResult>(task.Result, task.Error));
        }

		public IFuture<Result.DeleteAreaModelMasterResult> DeleteAreaModelMasterFuture(
                Request.DeleteAreaModelMasterRequest request
        )
		{
			return new DeleteAreaModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.DeleteAreaModelMasterResult> DeleteAreaModelMasterAsync(
                Request.DeleteAreaModelMasterRequest request
        )
		{
            AsyncResult<Result.DeleteAreaModelMasterResult> result = null;
			await DeleteAreaModelMaster(
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
		public DeleteAreaModelMasterTask DeleteAreaModelMasterAsync(
                Request.DeleteAreaModelMasterRequest request
        )
		{
			return new DeleteAreaModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.DeleteAreaModelMasterResult> DeleteAreaModelMasterAsync(
                Request.DeleteAreaModelMasterRequest request
        )
		{
			var task = new DeleteAreaModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class DescribeLayerModelsTask : Gs2RestSessionTask<DescribeLayerModelsRequest, DescribeLayerModelsResult>
        {
            public DescribeLayerModelsTask(IGs2Session session, RestSessionRequestFactory factory, DescribeLayerModelsRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DescribeLayerModelsRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "mega-field")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/area/{areaModelName}/layer";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{areaModelName}", !string.IsNullOrEmpty(request.AreaModelName) ? request.AreaModelName.ToString() : "null");

                var sessionRequest = Factory.Get(url);
                if (request.ContextStack != null)
                {
                    sessionRequest.AddQueryString("contextStack", request.ContextStack);
                }
                if (request.DryRun)
                {
                    sessionRequest.AddHeader("X-GS2-DRY-RUN", "true");
                }

                AddHeader(
                    Session.Credential,
                    sessionRequest
                );

                return sessionRequest;
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator DescribeLayerModels(
                Request.DescribeLayerModelsRequest request,
                UnityAction<AsyncResult<Result.DescribeLayerModelsResult>> callback
        )
		{
			var task = new DescribeLayerModelsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.DescribeLayerModelsResult>(task.Result, task.Error));
        }

		public IFuture<Result.DescribeLayerModelsResult> DescribeLayerModelsFuture(
                Request.DescribeLayerModelsRequest request
        )
		{
			return new DescribeLayerModelsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.DescribeLayerModelsResult> DescribeLayerModelsAsync(
                Request.DescribeLayerModelsRequest request
        )
		{
            AsyncResult<Result.DescribeLayerModelsResult> result = null;
			await DescribeLayerModels(
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
		public DescribeLayerModelsTask DescribeLayerModelsAsync(
                Request.DescribeLayerModelsRequest request
        )
		{
			return new DescribeLayerModelsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.DescribeLayerModelsResult> DescribeLayerModelsAsync(
                Request.DescribeLayerModelsRequest request
        )
		{
			var task = new DescribeLayerModelsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class GetLayerModelTask : Gs2RestSessionTask<GetLayerModelRequest, GetLayerModelResult>
        {
            public GetLayerModelTask(IGs2Session session, RestSessionRequestFactory factory, GetLayerModelRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(GetLayerModelRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "mega-field")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/area/{areaModelName}/layer/{layerModelName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{areaModelName}", !string.IsNullOrEmpty(request.AreaModelName) ? request.AreaModelName.ToString() : "null");
                url = url.Replace("{layerModelName}", !string.IsNullOrEmpty(request.LayerModelName) ? request.LayerModelName.ToString() : "null");

                var sessionRequest = Factory.Get(url);
                if (request.ContextStack != null)
                {
                    sessionRequest.AddQueryString("contextStack", request.ContextStack);
                }
                if (request.DryRun)
                {
                    sessionRequest.AddHeader("X-GS2-DRY-RUN", "true");
                }

                AddHeader(
                    Session.Credential,
                    sessionRequest
                );

                return sessionRequest;
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator GetLayerModel(
                Request.GetLayerModelRequest request,
                UnityAction<AsyncResult<Result.GetLayerModelResult>> callback
        )
		{
			var task = new GetLayerModelTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.GetLayerModelResult>(task.Result, task.Error));
        }

		public IFuture<Result.GetLayerModelResult> GetLayerModelFuture(
                Request.GetLayerModelRequest request
        )
		{
			return new GetLayerModelTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.GetLayerModelResult> GetLayerModelAsync(
                Request.GetLayerModelRequest request
        )
		{
            AsyncResult<Result.GetLayerModelResult> result = null;
			await GetLayerModel(
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
		public GetLayerModelTask GetLayerModelAsync(
                Request.GetLayerModelRequest request
        )
		{
			return new GetLayerModelTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.GetLayerModelResult> GetLayerModelAsync(
                Request.GetLayerModelRequest request
        )
		{
			var task = new GetLayerModelTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class DescribeLayerModelMastersTask : Gs2RestSessionTask<DescribeLayerModelMastersRequest, DescribeLayerModelMastersResult>
        {
            public DescribeLayerModelMastersTask(IGs2Session session, RestSessionRequestFactory factory, DescribeLayerModelMastersRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DescribeLayerModelMastersRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "mega-field")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/master/area/{areaModelName}/layer";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{areaModelName}", !string.IsNullOrEmpty(request.AreaModelName) ? request.AreaModelName.ToString() : "null");

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
                if (request.DryRun)
                {
                    sessionRequest.AddHeader("X-GS2-DRY-RUN", "true");
                }

                AddHeader(
                    Session.Credential,
                    sessionRequest
                );

                return sessionRequest;
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator DescribeLayerModelMasters(
                Request.DescribeLayerModelMastersRequest request,
                UnityAction<AsyncResult<Result.DescribeLayerModelMastersResult>> callback
        )
		{
			var task = new DescribeLayerModelMastersTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.DescribeLayerModelMastersResult>(task.Result, task.Error));
        }

		public IFuture<Result.DescribeLayerModelMastersResult> DescribeLayerModelMastersFuture(
                Request.DescribeLayerModelMastersRequest request
        )
		{
			return new DescribeLayerModelMastersTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.DescribeLayerModelMastersResult> DescribeLayerModelMastersAsync(
                Request.DescribeLayerModelMastersRequest request
        )
		{
            AsyncResult<Result.DescribeLayerModelMastersResult> result = null;
			await DescribeLayerModelMasters(
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
		public DescribeLayerModelMastersTask DescribeLayerModelMastersAsync(
                Request.DescribeLayerModelMastersRequest request
        )
		{
			return new DescribeLayerModelMastersTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.DescribeLayerModelMastersResult> DescribeLayerModelMastersAsync(
                Request.DescribeLayerModelMastersRequest request
        )
		{
			var task = new DescribeLayerModelMastersTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class CreateLayerModelMasterTask : Gs2RestSessionTask<CreateLayerModelMasterRequest, CreateLayerModelMasterResult>
        {
            public CreateLayerModelMasterTask(IGs2Session session, RestSessionRequestFactory factory, CreateLayerModelMasterRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(CreateLayerModelMasterRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "mega-field")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/master/area/{areaModelName}/layer";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{areaModelName}", !string.IsNullOrEmpty(request.AreaModelName) ? request.AreaModelName.ToString() : "null");

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
                if (request.DryRun)
                {
                    sessionRequest.AddHeader("X-GS2-DRY-RUN", "true");
                }

                AddHeader(
                    Session.Credential,
                    sessionRequest
                );

                return sessionRequest;
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator CreateLayerModelMaster(
                Request.CreateLayerModelMasterRequest request,
                UnityAction<AsyncResult<Result.CreateLayerModelMasterResult>> callback
        )
		{
			var task = new CreateLayerModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.CreateLayerModelMasterResult>(task.Result, task.Error));
        }

		public IFuture<Result.CreateLayerModelMasterResult> CreateLayerModelMasterFuture(
                Request.CreateLayerModelMasterRequest request
        )
		{
			return new CreateLayerModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.CreateLayerModelMasterResult> CreateLayerModelMasterAsync(
                Request.CreateLayerModelMasterRequest request
        )
		{
            AsyncResult<Result.CreateLayerModelMasterResult> result = null;
			await CreateLayerModelMaster(
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
		public CreateLayerModelMasterTask CreateLayerModelMasterAsync(
                Request.CreateLayerModelMasterRequest request
        )
		{
			return new CreateLayerModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.CreateLayerModelMasterResult> CreateLayerModelMasterAsync(
                Request.CreateLayerModelMasterRequest request
        )
		{
			var task = new CreateLayerModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class GetLayerModelMasterTask : Gs2RestSessionTask<GetLayerModelMasterRequest, GetLayerModelMasterResult>
        {
            public GetLayerModelMasterTask(IGs2Session session, RestSessionRequestFactory factory, GetLayerModelMasterRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(GetLayerModelMasterRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "mega-field")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/master/area/{areaModelName}/layer/{layerModelName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{areaModelName}", !string.IsNullOrEmpty(request.AreaModelName) ? request.AreaModelName.ToString() : "null");
                url = url.Replace("{layerModelName}", !string.IsNullOrEmpty(request.LayerModelName) ? request.LayerModelName.ToString() : "null");

                var sessionRequest = Factory.Get(url);
                if (request.ContextStack != null)
                {
                    sessionRequest.AddQueryString("contextStack", request.ContextStack);
                }
                if (request.DryRun)
                {
                    sessionRequest.AddHeader("X-GS2-DRY-RUN", "true");
                }

                AddHeader(
                    Session.Credential,
                    sessionRequest
                );

                return sessionRequest;
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator GetLayerModelMaster(
                Request.GetLayerModelMasterRequest request,
                UnityAction<AsyncResult<Result.GetLayerModelMasterResult>> callback
        )
		{
			var task = new GetLayerModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.GetLayerModelMasterResult>(task.Result, task.Error));
        }

		public IFuture<Result.GetLayerModelMasterResult> GetLayerModelMasterFuture(
                Request.GetLayerModelMasterRequest request
        )
		{
			return new GetLayerModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.GetLayerModelMasterResult> GetLayerModelMasterAsync(
                Request.GetLayerModelMasterRequest request
        )
		{
            AsyncResult<Result.GetLayerModelMasterResult> result = null;
			await GetLayerModelMaster(
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
		public GetLayerModelMasterTask GetLayerModelMasterAsync(
                Request.GetLayerModelMasterRequest request
        )
		{
			return new GetLayerModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.GetLayerModelMasterResult> GetLayerModelMasterAsync(
                Request.GetLayerModelMasterRequest request
        )
		{
			var task = new GetLayerModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class UpdateLayerModelMasterTask : Gs2RestSessionTask<UpdateLayerModelMasterRequest, UpdateLayerModelMasterResult>
        {
            public UpdateLayerModelMasterTask(IGs2Session session, RestSessionRequestFactory factory, UpdateLayerModelMasterRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(UpdateLayerModelMasterRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "mega-field")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/master/area/{areaModelName}/layer/{layerModelName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{areaModelName}", !string.IsNullOrEmpty(request.AreaModelName) ? request.AreaModelName.ToString() : "null");
                url = url.Replace("{layerModelName}", !string.IsNullOrEmpty(request.LayerModelName) ? request.LayerModelName.ToString() : "null");

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
                if (request.DryRun)
                {
                    sessionRequest.AddHeader("X-GS2-DRY-RUN", "true");
                }

                AddHeader(
                    Session.Credential,
                    sessionRequest
                );

                return sessionRequest;
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator UpdateLayerModelMaster(
                Request.UpdateLayerModelMasterRequest request,
                UnityAction<AsyncResult<Result.UpdateLayerModelMasterResult>> callback
        )
		{
			var task = new UpdateLayerModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.UpdateLayerModelMasterResult>(task.Result, task.Error));
        }

		public IFuture<Result.UpdateLayerModelMasterResult> UpdateLayerModelMasterFuture(
                Request.UpdateLayerModelMasterRequest request
        )
		{
			return new UpdateLayerModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.UpdateLayerModelMasterResult> UpdateLayerModelMasterAsync(
                Request.UpdateLayerModelMasterRequest request
        )
		{
            AsyncResult<Result.UpdateLayerModelMasterResult> result = null;
			await UpdateLayerModelMaster(
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
		public UpdateLayerModelMasterTask UpdateLayerModelMasterAsync(
                Request.UpdateLayerModelMasterRequest request
        )
		{
			return new UpdateLayerModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.UpdateLayerModelMasterResult> UpdateLayerModelMasterAsync(
                Request.UpdateLayerModelMasterRequest request
        )
		{
			var task = new UpdateLayerModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class DeleteLayerModelMasterTask : Gs2RestSessionTask<DeleteLayerModelMasterRequest, DeleteLayerModelMasterResult>
        {
            public DeleteLayerModelMasterTask(IGs2Session session, RestSessionRequestFactory factory, DeleteLayerModelMasterRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DeleteLayerModelMasterRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "mega-field")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/master/area/{areaModelName}/layer/{layerModelName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{areaModelName}", !string.IsNullOrEmpty(request.AreaModelName) ? request.AreaModelName.ToString() : "null");
                url = url.Replace("{layerModelName}", !string.IsNullOrEmpty(request.LayerModelName) ? request.LayerModelName.ToString() : "null");

                var sessionRequest = Factory.Delete(url);
                if (request.ContextStack != null)
                {
                    sessionRequest.AddQueryString("contextStack", request.ContextStack);
                }
                if (request.DryRun)
                {
                    sessionRequest.AddHeader("X-GS2-DRY-RUN", "true");
                }

                AddHeader(
                    Session.Credential,
                    sessionRequest
                );

                return sessionRequest;
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator DeleteLayerModelMaster(
                Request.DeleteLayerModelMasterRequest request,
                UnityAction<AsyncResult<Result.DeleteLayerModelMasterResult>> callback
        )
		{
			var task = new DeleteLayerModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.DeleteLayerModelMasterResult>(task.Result, task.Error));
        }

		public IFuture<Result.DeleteLayerModelMasterResult> DeleteLayerModelMasterFuture(
                Request.DeleteLayerModelMasterRequest request
        )
		{
			return new DeleteLayerModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.DeleteLayerModelMasterResult> DeleteLayerModelMasterAsync(
                Request.DeleteLayerModelMasterRequest request
        )
		{
            AsyncResult<Result.DeleteLayerModelMasterResult> result = null;
			await DeleteLayerModelMaster(
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
		public DeleteLayerModelMasterTask DeleteLayerModelMasterAsync(
                Request.DeleteLayerModelMasterRequest request
        )
		{
			return new DeleteLayerModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.DeleteLayerModelMasterResult> DeleteLayerModelMasterAsync(
                Request.DeleteLayerModelMasterRequest request
        )
		{
			var task = new DeleteLayerModelMasterTask(
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
                    .Replace("{service}", "mega-field")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/master/export";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");

                var sessionRequest = Factory.Get(url);
                if (request.ContextStack != null)
                {
                    sessionRequest.AddQueryString("contextStack", request.ContextStack);
                }
                if (request.DryRun)
                {
                    sessionRequest.AddHeader("X-GS2-DRY-RUN", "true");
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


        public class GetCurrentFieldMasterTask : Gs2RestSessionTask<GetCurrentFieldMasterRequest, GetCurrentFieldMasterResult>
        {
            public GetCurrentFieldMasterTask(IGs2Session session, RestSessionRequestFactory factory, GetCurrentFieldMasterRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(GetCurrentFieldMasterRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "mega-field")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/master";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");

                var sessionRequest = Factory.Get(url);
                if (request.ContextStack != null)
                {
                    sessionRequest.AddQueryString("contextStack", request.ContextStack);
                }
                if (request.DryRun)
                {
                    sessionRequest.AddHeader("X-GS2-DRY-RUN", "true");
                }

                AddHeader(
                    Session.Credential,
                    sessionRequest
                );

                return sessionRequest;
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator GetCurrentFieldMaster(
                Request.GetCurrentFieldMasterRequest request,
                UnityAction<AsyncResult<Result.GetCurrentFieldMasterResult>> callback
        )
		{
			var task = new GetCurrentFieldMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.GetCurrentFieldMasterResult>(task.Result, task.Error));
        }

		public IFuture<Result.GetCurrentFieldMasterResult> GetCurrentFieldMasterFuture(
                Request.GetCurrentFieldMasterRequest request
        )
		{
			return new GetCurrentFieldMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.GetCurrentFieldMasterResult> GetCurrentFieldMasterAsync(
                Request.GetCurrentFieldMasterRequest request
        )
		{
            AsyncResult<Result.GetCurrentFieldMasterResult> result = null;
			await GetCurrentFieldMaster(
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
		public GetCurrentFieldMasterTask GetCurrentFieldMasterAsync(
                Request.GetCurrentFieldMasterRequest request
        )
		{
			return new GetCurrentFieldMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.GetCurrentFieldMasterResult> GetCurrentFieldMasterAsync(
                Request.GetCurrentFieldMasterRequest request
        )
		{
			var task = new GetCurrentFieldMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class UpdateCurrentFieldMasterTask : Gs2RestSessionTask<UpdateCurrentFieldMasterRequest, UpdateCurrentFieldMasterResult>
        {
            public UpdateCurrentFieldMasterTask(IGs2Session session, RestSessionRequestFactory factory, UpdateCurrentFieldMasterRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(UpdateCurrentFieldMasterRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "mega-field")
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
                if (request.DryRun)
                {
                    sessionRequest.AddHeader("X-GS2-DRY-RUN", "true");
                }

                AddHeader(
                    Session.Credential,
                    sessionRequest
                );

                return sessionRequest;
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator UpdateCurrentFieldMaster(
                Request.UpdateCurrentFieldMasterRequest request,
                UnityAction<AsyncResult<Result.UpdateCurrentFieldMasterResult>> callback
        )
		{
			var task = new UpdateCurrentFieldMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.UpdateCurrentFieldMasterResult>(task.Result, task.Error));
        }

		public IFuture<Result.UpdateCurrentFieldMasterResult> UpdateCurrentFieldMasterFuture(
                Request.UpdateCurrentFieldMasterRequest request
        )
		{
			return new UpdateCurrentFieldMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.UpdateCurrentFieldMasterResult> UpdateCurrentFieldMasterAsync(
                Request.UpdateCurrentFieldMasterRequest request
        )
		{
            AsyncResult<Result.UpdateCurrentFieldMasterResult> result = null;
			await UpdateCurrentFieldMaster(
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
		public UpdateCurrentFieldMasterTask UpdateCurrentFieldMasterAsync(
                Request.UpdateCurrentFieldMasterRequest request
        )
		{
			return new UpdateCurrentFieldMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.UpdateCurrentFieldMasterResult> UpdateCurrentFieldMasterAsync(
                Request.UpdateCurrentFieldMasterRequest request
        )
		{
			var task = new UpdateCurrentFieldMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class UpdateCurrentFieldMasterFromGitHubTask : Gs2RestSessionTask<UpdateCurrentFieldMasterFromGitHubRequest, UpdateCurrentFieldMasterFromGitHubResult>
        {
            public UpdateCurrentFieldMasterFromGitHubTask(IGs2Session session, RestSessionRequestFactory factory, UpdateCurrentFieldMasterFromGitHubRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(UpdateCurrentFieldMasterFromGitHubRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "mega-field")
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
                if (request.DryRun)
                {
                    sessionRequest.AddHeader("X-GS2-DRY-RUN", "true");
                }

                AddHeader(
                    Session.Credential,
                    sessionRequest
                );

                return sessionRequest;
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator UpdateCurrentFieldMasterFromGitHub(
                Request.UpdateCurrentFieldMasterFromGitHubRequest request,
                UnityAction<AsyncResult<Result.UpdateCurrentFieldMasterFromGitHubResult>> callback
        )
		{
			var task = new UpdateCurrentFieldMasterFromGitHubTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.UpdateCurrentFieldMasterFromGitHubResult>(task.Result, task.Error));
        }

		public IFuture<Result.UpdateCurrentFieldMasterFromGitHubResult> UpdateCurrentFieldMasterFromGitHubFuture(
                Request.UpdateCurrentFieldMasterFromGitHubRequest request
        )
		{
			return new UpdateCurrentFieldMasterFromGitHubTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.UpdateCurrentFieldMasterFromGitHubResult> UpdateCurrentFieldMasterFromGitHubAsync(
                Request.UpdateCurrentFieldMasterFromGitHubRequest request
        )
		{
            AsyncResult<Result.UpdateCurrentFieldMasterFromGitHubResult> result = null;
			await UpdateCurrentFieldMasterFromGitHub(
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
		public UpdateCurrentFieldMasterFromGitHubTask UpdateCurrentFieldMasterFromGitHubAsync(
                Request.UpdateCurrentFieldMasterFromGitHubRequest request
        )
		{
			return new UpdateCurrentFieldMasterFromGitHubTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.UpdateCurrentFieldMasterFromGitHubResult> UpdateCurrentFieldMasterFromGitHubAsync(
                Request.UpdateCurrentFieldMasterFromGitHubRequest request
        )
		{
			var task = new UpdateCurrentFieldMasterFromGitHubTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class PutPositionTask : Gs2RestSessionTask<PutPositionRequest, PutPositionResult>
        {
            public PutPositionTask(IGs2Session session, RestSessionRequestFactory factory, PutPositionRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(PutPositionRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "mega-field")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/me/spatial/{areaModelName}/{layerModelName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{areaModelName}", !string.IsNullOrEmpty(request.AreaModelName) ? request.AreaModelName.ToString() : "null");
                url = url.Replace("{layerModelName}", !string.IsNullOrEmpty(request.LayerModelName) ? request.LayerModelName.ToString() : "null");

                var sessionRequest = Factory.Post(url);

                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);
                jsonWriter.WriteObjectStart();
                if (request.Position != null)
                {
                    jsonWriter.WritePropertyName("position");
                    request.Position.WriteJson(jsonWriter);
                }
                if (request.Vector != null)
                {
                    jsonWriter.WritePropertyName("vector");
                    request.Vector.WriteJson(jsonWriter);
                }
                if (request.R != null)
                {
                    jsonWriter.WritePropertyName("r");
                    jsonWriter.Write(request.R.ToString());
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
                if (request.AccessToken != null)
                {
                    sessionRequest.AddHeader("X-GS2-ACCESS-TOKEN", request.AccessToken);
                }
                if (request.DuplicationAvoider != null)
                {
                    sessionRequest.AddHeader("X-GS2-DUPLICATION-AVOIDER", request.DuplicationAvoider);
                }
                if (request.DryRun)
                {
                    sessionRequest.AddHeader("X-GS2-DRY-RUN", "true");
                }

                AddHeader(
                    Session.Credential,
                    sessionRequest
                );

                return sessionRequest;
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator PutPosition(
                Request.PutPositionRequest request,
                UnityAction<AsyncResult<Result.PutPositionResult>> callback
        )
		{
			var task = new PutPositionTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.PutPositionResult>(task.Result, task.Error));
        }

		public IFuture<Result.PutPositionResult> PutPositionFuture(
                Request.PutPositionRequest request
        )
		{
			return new PutPositionTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.PutPositionResult> PutPositionAsync(
                Request.PutPositionRequest request
        )
		{
            AsyncResult<Result.PutPositionResult> result = null;
			await PutPosition(
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
		public PutPositionTask PutPositionAsync(
                Request.PutPositionRequest request
        )
		{
			return new PutPositionTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.PutPositionResult> PutPositionAsync(
                Request.PutPositionRequest request
        )
		{
			var task = new PutPositionTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class PutPositionByUserIdTask : Gs2RestSessionTask<PutPositionByUserIdRequest, PutPositionByUserIdResult>
        {
            public PutPositionByUserIdTask(IGs2Session session, RestSessionRequestFactory factory, PutPositionByUserIdRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(PutPositionByUserIdRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "mega-field")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/{userId}/spatial/{areaModelName}/{layerModelName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{userId}", !string.IsNullOrEmpty(request.UserId) ? request.UserId.ToString() : "null");
                url = url.Replace("{areaModelName}", !string.IsNullOrEmpty(request.AreaModelName) ? request.AreaModelName.ToString() : "null");
                url = url.Replace("{layerModelName}", !string.IsNullOrEmpty(request.LayerModelName) ? request.LayerModelName.ToString() : "null");

                var sessionRequest = Factory.Post(url);

                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);
                jsonWriter.WriteObjectStart();
                if (request.Position != null)
                {
                    jsonWriter.WritePropertyName("position");
                    request.Position.WriteJson(jsonWriter);
                }
                if (request.Vector != null)
                {
                    jsonWriter.WritePropertyName("vector");
                    request.Vector.WriteJson(jsonWriter);
                }
                if (request.R != null)
                {
                    jsonWriter.WritePropertyName("r");
                    jsonWriter.Write(request.R.ToString());
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
                if (request.DuplicationAvoider != null)
                {
                    sessionRequest.AddHeader("X-GS2-DUPLICATION-AVOIDER", request.DuplicationAvoider);
                }
                if (request.TimeOffsetToken != null)
                {
                    sessionRequest.AddHeader("X-GS2-TIME-OFFSET-TOKEN", request.TimeOffsetToken);
                }
                if (request.DryRun)
                {
                    sessionRequest.AddHeader("X-GS2-DRY-RUN", "true");
                }

                AddHeader(
                    Session.Credential,
                    sessionRequest
                );

                return sessionRequest;
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator PutPositionByUserId(
                Request.PutPositionByUserIdRequest request,
                UnityAction<AsyncResult<Result.PutPositionByUserIdResult>> callback
        )
		{
			var task = new PutPositionByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.PutPositionByUserIdResult>(task.Result, task.Error));
        }

		public IFuture<Result.PutPositionByUserIdResult> PutPositionByUserIdFuture(
                Request.PutPositionByUserIdRequest request
        )
		{
			return new PutPositionByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.PutPositionByUserIdResult> PutPositionByUserIdAsync(
                Request.PutPositionByUserIdRequest request
        )
		{
            AsyncResult<Result.PutPositionByUserIdResult> result = null;
			await PutPositionByUserId(
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
		public PutPositionByUserIdTask PutPositionByUserIdAsync(
                Request.PutPositionByUserIdRequest request
        )
		{
			return new PutPositionByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.PutPositionByUserIdResult> PutPositionByUserIdAsync(
                Request.PutPositionByUserIdRequest request
        )
		{
			var task = new PutPositionByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class FetchPositionTask : Gs2RestSessionTask<FetchPositionRequest, FetchPositionResult>
        {
            public FetchPositionTask(IGs2Session session, RestSessionRequestFactory factory, FetchPositionRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(FetchPositionRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "mega-field")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/area/{areaModelName}/layer/{layerModelName}/spatial/fetch";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{areaModelName}", !string.IsNullOrEmpty(request.AreaModelName) ? request.AreaModelName.ToString() : "null");
                url = url.Replace("{layerModelName}", !string.IsNullOrEmpty(request.LayerModelName) ? request.LayerModelName.ToString() : "null");

                var sessionRequest = Factory.Post(url);

                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);
                jsonWriter.WriteObjectStart();
                if (request.UserIds != null)
                {
                    jsonWriter.WritePropertyName("userIds");
                    jsonWriter.WriteArrayStart();
                    foreach(var item in request.UserIds)
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
                if (request.AccessToken != null)
                {
                    sessionRequest.AddHeader("X-GS2-ACCESS-TOKEN", request.AccessToken);
                }
                if (request.DuplicationAvoider != null)
                {
                    sessionRequest.AddHeader("X-GS2-DUPLICATION-AVOIDER", request.DuplicationAvoider);
                }
                if (request.DryRun)
                {
                    sessionRequest.AddHeader("X-GS2-DRY-RUN", "true");
                }

                AddHeader(
                    Session.Credential,
                    sessionRequest
                );

                return sessionRequest;
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator FetchPosition(
                Request.FetchPositionRequest request,
                UnityAction<AsyncResult<Result.FetchPositionResult>> callback
        )
		{
			var task = new FetchPositionTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.FetchPositionResult>(task.Result, task.Error));
        }

		public IFuture<Result.FetchPositionResult> FetchPositionFuture(
                Request.FetchPositionRequest request
        )
		{
			return new FetchPositionTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.FetchPositionResult> FetchPositionAsync(
                Request.FetchPositionRequest request
        )
		{
            AsyncResult<Result.FetchPositionResult> result = null;
			await FetchPosition(
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
		public FetchPositionTask FetchPositionAsync(
                Request.FetchPositionRequest request
        )
		{
			return new FetchPositionTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.FetchPositionResult> FetchPositionAsync(
                Request.FetchPositionRequest request
        )
		{
			var task = new FetchPositionTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class FetchPositionFromSystemTask : Gs2RestSessionTask<FetchPositionFromSystemRequest, FetchPositionFromSystemResult>
        {
            public FetchPositionFromSystemTask(IGs2Session session, RestSessionRequestFactory factory, FetchPositionFromSystemRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(FetchPositionFromSystemRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "mega-field")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/system/area/{areaModelName}/layer/{layerModelName}/spatial/fetch";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{areaModelName}", !string.IsNullOrEmpty(request.AreaModelName) ? request.AreaModelName.ToString() : "null");
                url = url.Replace("{layerModelName}", !string.IsNullOrEmpty(request.LayerModelName) ? request.LayerModelName.ToString() : "null");

                var sessionRequest = Factory.Post(url);

                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);
                jsonWriter.WriteObjectStart();
                if (request.UserIds != null)
                {
                    jsonWriter.WritePropertyName("userIds");
                    jsonWriter.WriteArrayStart();
                    foreach(var item in request.UserIds)
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
                if (request.DuplicationAvoider != null)
                {
                    sessionRequest.AddHeader("X-GS2-DUPLICATION-AVOIDER", request.DuplicationAvoider);
                }
                if (request.DryRun)
                {
                    sessionRequest.AddHeader("X-GS2-DRY-RUN", "true");
                }

                AddHeader(
                    Session.Credential,
                    sessionRequest
                );

                return sessionRequest;
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator FetchPositionFromSystem(
                Request.FetchPositionFromSystemRequest request,
                UnityAction<AsyncResult<Result.FetchPositionFromSystemResult>> callback
        )
		{
			var task = new FetchPositionFromSystemTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.FetchPositionFromSystemResult>(task.Result, task.Error));
        }

		public IFuture<Result.FetchPositionFromSystemResult> FetchPositionFromSystemFuture(
                Request.FetchPositionFromSystemRequest request
        )
		{
			return new FetchPositionFromSystemTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.FetchPositionFromSystemResult> FetchPositionFromSystemAsync(
                Request.FetchPositionFromSystemRequest request
        )
		{
            AsyncResult<Result.FetchPositionFromSystemResult> result = null;
			await FetchPositionFromSystem(
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
		public FetchPositionFromSystemTask FetchPositionFromSystemAsync(
                Request.FetchPositionFromSystemRequest request
        )
		{
			return new FetchPositionFromSystemTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.FetchPositionFromSystemResult> FetchPositionFromSystemAsync(
                Request.FetchPositionFromSystemRequest request
        )
		{
			var task = new FetchPositionFromSystemTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class NearUserIdsTask : Gs2RestSessionTask<NearUserIdsRequest, NearUserIdsResult>
        {
            public NearUserIdsTask(IGs2Session session, RestSessionRequestFactory factory, NearUserIdsRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(NearUserIdsRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "mega-field")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/area/{areaModelName}/layer/{layerModelName}/spatial/near";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{areaModelName}", !string.IsNullOrEmpty(request.AreaModelName) ? request.AreaModelName.ToString() : "null");
                url = url.Replace("{layerModelName}", !string.IsNullOrEmpty(request.LayerModelName) ? request.LayerModelName.ToString() : "null");

                var sessionRequest = Factory.Post(url);

                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);
                jsonWriter.WriteObjectStart();
                if (request.Point != null)
                {
                    jsonWriter.WritePropertyName("point");
                    request.Point.WriteJson(jsonWriter);
                }
                if (request.R != null)
                {
                    jsonWriter.WritePropertyName("r");
                    jsonWriter.Write(request.R.ToString());
                }
                if (request.Limit != null)
                {
                    jsonWriter.WritePropertyName("limit");
                    jsonWriter.Write(request.Limit.ToString());
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
                if (request.AccessToken != null)
                {
                    sessionRequest.AddHeader("X-GS2-ACCESS-TOKEN", request.AccessToken);
                }
                if (request.DuplicationAvoider != null)
                {
                    sessionRequest.AddHeader("X-GS2-DUPLICATION-AVOIDER", request.DuplicationAvoider);
                }
                if (request.DryRun)
                {
                    sessionRequest.AddHeader("X-GS2-DRY-RUN", "true");
                }

                AddHeader(
                    Session.Credential,
                    sessionRequest
                );

                return sessionRequest;
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator NearUserIds(
                Request.NearUserIdsRequest request,
                UnityAction<AsyncResult<Result.NearUserIdsResult>> callback
        )
		{
			var task = new NearUserIdsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.NearUserIdsResult>(task.Result, task.Error));
        }

		public IFuture<Result.NearUserIdsResult> NearUserIdsFuture(
                Request.NearUserIdsRequest request
        )
		{
			return new NearUserIdsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.NearUserIdsResult> NearUserIdsAsync(
                Request.NearUserIdsRequest request
        )
		{
            AsyncResult<Result.NearUserIdsResult> result = null;
			await NearUserIds(
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
		public NearUserIdsTask NearUserIdsAsync(
                Request.NearUserIdsRequest request
        )
		{
			return new NearUserIdsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.NearUserIdsResult> NearUserIdsAsync(
                Request.NearUserIdsRequest request
        )
		{
			var task = new NearUserIdsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class NearUserIdsFromSystemTask : Gs2RestSessionTask<NearUserIdsFromSystemRequest, NearUserIdsFromSystemResult>
        {
            public NearUserIdsFromSystemTask(IGs2Session session, RestSessionRequestFactory factory, NearUserIdsFromSystemRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(NearUserIdsFromSystemRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "mega-field")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/system/area/{areaModelName}/layer/{layerModelName}/spatial/near";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{areaModelName}", !string.IsNullOrEmpty(request.AreaModelName) ? request.AreaModelName.ToString() : "null");
                url = url.Replace("{layerModelName}", !string.IsNullOrEmpty(request.LayerModelName) ? request.LayerModelName.ToString() : "null");

                var sessionRequest = Factory.Post(url);

                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);
                jsonWriter.WriteObjectStart();
                if (request.Point != null)
                {
                    jsonWriter.WritePropertyName("point");
                    request.Point.WriteJson(jsonWriter);
                }
                if (request.R != null)
                {
                    jsonWriter.WritePropertyName("r");
                    jsonWriter.Write(request.R.ToString());
                }
                if (request.Limit != null)
                {
                    jsonWriter.WritePropertyName("limit");
                    jsonWriter.Write(request.Limit.ToString());
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
                if (request.DuplicationAvoider != null)
                {
                    sessionRequest.AddHeader("X-GS2-DUPLICATION-AVOIDER", request.DuplicationAvoider);
                }
                if (request.DryRun)
                {
                    sessionRequest.AddHeader("X-GS2-DRY-RUN", "true");
                }

                AddHeader(
                    Session.Credential,
                    sessionRequest
                );

                return sessionRequest;
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator NearUserIdsFromSystem(
                Request.NearUserIdsFromSystemRequest request,
                UnityAction<AsyncResult<Result.NearUserIdsFromSystemResult>> callback
        )
		{
			var task = new NearUserIdsFromSystemTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.NearUserIdsFromSystemResult>(task.Result, task.Error));
        }

		public IFuture<Result.NearUserIdsFromSystemResult> NearUserIdsFromSystemFuture(
                Request.NearUserIdsFromSystemRequest request
        )
		{
			return new NearUserIdsFromSystemTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.NearUserIdsFromSystemResult> NearUserIdsFromSystemAsync(
                Request.NearUserIdsFromSystemRequest request
        )
		{
            AsyncResult<Result.NearUserIdsFromSystemResult> result = null;
			await NearUserIdsFromSystem(
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
		public NearUserIdsFromSystemTask NearUserIdsFromSystemAsync(
                Request.NearUserIdsFromSystemRequest request
        )
		{
			return new NearUserIdsFromSystemTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.NearUserIdsFromSystemResult> NearUserIdsFromSystemAsync(
                Request.NearUserIdsFromSystemRequest request
        )
		{
			var task = new NearUserIdsFromSystemTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class ActionTask : Gs2RestSessionTask<ActionRequest, ActionResult>
        {
            public ActionTask(IGs2Session session, RestSessionRequestFactory factory, ActionRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(ActionRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "mega-field")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/me/spatial/{areaModelName}/{layerModelName}/action";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{areaModelName}", !string.IsNullOrEmpty(request.AreaModelName) ? request.AreaModelName.ToString() : "null");
                url = url.Replace("{layerModelName}", !string.IsNullOrEmpty(request.LayerModelName) ? request.LayerModelName.ToString() : "null");

                var sessionRequest = Factory.Post(url);

                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);
                jsonWriter.WriteObjectStart();
                if (request.Position != null)
                {
                    jsonWriter.WritePropertyName("position");
                    request.Position.WriteJson(jsonWriter);
                }
                if (request.Scopes != null)
                {
                    jsonWriter.WritePropertyName("scopes");
                    jsonWriter.WriteArrayStart();
                    foreach(var item in request.Scopes)
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
                if (request.AccessToken != null)
                {
                    sessionRequest.AddHeader("X-GS2-ACCESS-TOKEN", request.AccessToken);
                }
                if (request.DuplicationAvoider != null)
                {
                    sessionRequest.AddHeader("X-GS2-DUPLICATION-AVOIDER", request.DuplicationAvoider);
                }
                if (request.DryRun)
                {
                    sessionRequest.AddHeader("X-GS2-DRY-RUN", "true");
                }

                AddHeader(
                    Session.Credential,
                    sessionRequest
                );

                return sessionRequest;
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator Action(
                Request.ActionRequest request,
                UnityAction<AsyncResult<Result.ActionResult>> callback
        )
		{
			var task = new ActionTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.ActionResult>(task.Result, task.Error));
        }

		public IFuture<Result.ActionResult> ActionFuture(
                Request.ActionRequest request
        )
		{
			return new ActionTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.ActionResult> ActionAsync(
                Request.ActionRequest request
        )
		{
            AsyncResult<Result.ActionResult> result = null;
			await Action(
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
		public ActionTask ActionAsync(
                Request.ActionRequest request
        )
		{
			return new ActionTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.ActionResult> ActionAsync(
                Request.ActionRequest request
        )
		{
			var task = new ActionTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class ActionByUserIdTask : Gs2RestSessionTask<ActionByUserIdRequest, ActionByUserIdResult>
        {
            public ActionByUserIdTask(IGs2Session session, RestSessionRequestFactory factory, ActionByUserIdRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(ActionByUserIdRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "mega-field")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/{userId}/spatial/{areaModelName}/{layerModelName}/action";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{userId}", !string.IsNullOrEmpty(request.UserId) ? request.UserId.ToString() : "null");
                url = url.Replace("{areaModelName}", !string.IsNullOrEmpty(request.AreaModelName) ? request.AreaModelName.ToString() : "null");
                url = url.Replace("{layerModelName}", !string.IsNullOrEmpty(request.LayerModelName) ? request.LayerModelName.ToString() : "null");

                var sessionRequest = Factory.Post(url);

                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);
                jsonWriter.WriteObjectStart();
                if (request.Position != null)
                {
                    jsonWriter.WritePropertyName("position");
                    request.Position.WriteJson(jsonWriter);
                }
                if (request.Scopes != null)
                {
                    jsonWriter.WritePropertyName("scopes");
                    jsonWriter.WriteArrayStart();
                    foreach(var item in request.Scopes)
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
                if (request.DuplicationAvoider != null)
                {
                    sessionRequest.AddHeader("X-GS2-DUPLICATION-AVOIDER", request.DuplicationAvoider);
                }
                if (request.TimeOffsetToken != null)
                {
                    sessionRequest.AddHeader("X-GS2-TIME-OFFSET-TOKEN", request.TimeOffsetToken);
                }
                if (request.DryRun)
                {
                    sessionRequest.AddHeader("X-GS2-DRY-RUN", "true");
                }

                AddHeader(
                    Session.Credential,
                    sessionRequest
                );

                return sessionRequest;
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator ActionByUserId(
                Request.ActionByUserIdRequest request,
                UnityAction<AsyncResult<Result.ActionByUserIdResult>> callback
        )
		{
			var task = new ActionByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.ActionByUserIdResult>(task.Result, task.Error));
        }

		public IFuture<Result.ActionByUserIdResult> ActionByUserIdFuture(
                Request.ActionByUserIdRequest request
        )
		{
			return new ActionByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.ActionByUserIdResult> ActionByUserIdAsync(
                Request.ActionByUserIdRequest request
        )
		{
            AsyncResult<Result.ActionByUserIdResult> result = null;
			await ActionByUserId(
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
		public ActionByUserIdTask ActionByUserIdAsync(
                Request.ActionByUserIdRequest request
        )
		{
			return new ActionByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.ActionByUserIdResult> ActionByUserIdAsync(
                Request.ActionByUserIdRequest request
        )
		{
			var task = new ActionByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif
	}
}