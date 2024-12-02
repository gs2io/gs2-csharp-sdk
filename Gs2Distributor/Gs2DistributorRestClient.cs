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
using Gs2.Gs2Distributor.Request;
using Gs2.Gs2Distributor.Result;
using Gs2.Util.LitJson;

namespace Gs2.Gs2Distributor
{
	public class Gs2DistributorRestClient : AbstractGs2Client
	{
#if UNITY_2017_1_OR_NEWER
		private readonly CertificateHandler _certificateHandler;
#endif

		public static string Endpoint = "distributor";

        protected Gs2RestSession Gs2RestSession => (Gs2RestSession) Gs2Session;

		public Gs2DistributorRestClient(Gs2RestSession Gs2RestSession) : base(Gs2RestSession)
		{

		}

#if UNITY_2017_1_OR_NEWER
		public Gs2DistributorRestClient(Gs2RestSession gs2RestSession, CertificateHandler certificateHandler) : base(gs2RestSession)
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
                    .Replace("{service}", "distributor")
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
                    .Replace("{service}", "distributor")
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
                if (request.AssumeUserId != null)
                {
                    jsonWriter.WritePropertyName("assumeUserId");
                    jsonWriter.Write(request.AssumeUserId);
                }
                if (request.AutoRunStampSheetNotification != null)
                {
                    jsonWriter.WritePropertyName("autoRunStampSheetNotification");
                    request.AutoRunStampSheetNotification.WriteJson(jsonWriter);
                }
                if (request.AutoRunTransactionNotification != null)
                {
                    jsonWriter.WritePropertyName("autoRunTransactionNotification");
                    request.AutoRunTransactionNotification.WriteJson(jsonWriter);
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
                    .Replace("{service}", "distributor")
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
                    .Replace("{service}", "distributor")
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
                    .Replace("{service}", "distributor")
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
                if (request.AssumeUserId != null)
                {
                    jsonWriter.WritePropertyName("assumeUserId");
                    jsonWriter.Write(request.AssumeUserId);
                }
                if (request.AutoRunStampSheetNotification != null)
                {
                    jsonWriter.WritePropertyName("autoRunStampSheetNotification");
                    request.AutoRunStampSheetNotification.WriteJson(jsonWriter);
                }
                if (request.AutoRunTransactionNotification != null)
                {
                    jsonWriter.WritePropertyName("autoRunTransactionNotification");
                    request.AutoRunTransactionNotification.WriteJson(jsonWriter);
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
                    .Replace("{service}", "distributor")
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


        public class DescribeDistributorModelMastersTask : Gs2RestSessionTask<DescribeDistributorModelMastersRequest, DescribeDistributorModelMastersResult>
        {
            public DescribeDistributorModelMastersTask(IGs2Session session, RestSessionRequestFactory factory, DescribeDistributorModelMastersRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DescribeDistributorModelMastersRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "distributor")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/master/distributor";

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
		public IEnumerator DescribeDistributorModelMasters(
                Request.DescribeDistributorModelMastersRequest request,
                UnityAction<AsyncResult<Result.DescribeDistributorModelMastersResult>> callback
        )
		{
			var task = new DescribeDistributorModelMastersTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.DescribeDistributorModelMastersResult>(task.Result, task.Error));
        }

		public IFuture<Result.DescribeDistributorModelMastersResult> DescribeDistributorModelMastersFuture(
                Request.DescribeDistributorModelMastersRequest request
        )
		{
			return new DescribeDistributorModelMastersTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.DescribeDistributorModelMastersResult> DescribeDistributorModelMastersAsync(
                Request.DescribeDistributorModelMastersRequest request
        )
		{
            AsyncResult<Result.DescribeDistributorModelMastersResult> result = null;
			await DescribeDistributorModelMasters(
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
		public DescribeDistributorModelMastersTask DescribeDistributorModelMastersAsync(
                Request.DescribeDistributorModelMastersRequest request
        )
		{
			return new DescribeDistributorModelMastersTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.DescribeDistributorModelMastersResult> DescribeDistributorModelMastersAsync(
                Request.DescribeDistributorModelMastersRequest request
        )
		{
			var task = new DescribeDistributorModelMastersTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class CreateDistributorModelMasterTask : Gs2RestSessionTask<CreateDistributorModelMasterRequest, CreateDistributorModelMasterResult>
        {
            public CreateDistributorModelMasterTask(IGs2Session session, RestSessionRequestFactory factory, CreateDistributorModelMasterRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(CreateDistributorModelMasterRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "distributor")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/master/distributor";

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
                if (request.InboxNamespaceId != null)
                {
                    jsonWriter.WritePropertyName("inboxNamespaceId");
                    jsonWriter.Write(request.InboxNamespaceId);
                }
                if (request.WhiteListTargetIds != null)
                {
                    jsonWriter.WritePropertyName("whiteListTargetIds");
                    jsonWriter.WriteArrayStart();
                    foreach(var item in request.WhiteListTargetIds)
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
		public IEnumerator CreateDistributorModelMaster(
                Request.CreateDistributorModelMasterRequest request,
                UnityAction<AsyncResult<Result.CreateDistributorModelMasterResult>> callback
        )
		{
			var task = new CreateDistributorModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.CreateDistributorModelMasterResult>(task.Result, task.Error));
        }

		public IFuture<Result.CreateDistributorModelMasterResult> CreateDistributorModelMasterFuture(
                Request.CreateDistributorModelMasterRequest request
        )
		{
			return new CreateDistributorModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.CreateDistributorModelMasterResult> CreateDistributorModelMasterAsync(
                Request.CreateDistributorModelMasterRequest request
        )
		{
            AsyncResult<Result.CreateDistributorModelMasterResult> result = null;
			await CreateDistributorModelMaster(
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
		public CreateDistributorModelMasterTask CreateDistributorModelMasterAsync(
                Request.CreateDistributorModelMasterRequest request
        )
		{
			return new CreateDistributorModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.CreateDistributorModelMasterResult> CreateDistributorModelMasterAsync(
                Request.CreateDistributorModelMasterRequest request
        )
		{
			var task = new CreateDistributorModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class GetDistributorModelMasterTask : Gs2RestSessionTask<GetDistributorModelMasterRequest, GetDistributorModelMasterResult>
        {
            public GetDistributorModelMasterTask(IGs2Session session, RestSessionRequestFactory factory, GetDistributorModelMasterRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(GetDistributorModelMasterRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "distributor")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/master/distributor/{distributorName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{distributorName}", !string.IsNullOrEmpty(request.DistributorName) ? request.DistributorName.ToString() : "null");

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
		public IEnumerator GetDistributorModelMaster(
                Request.GetDistributorModelMasterRequest request,
                UnityAction<AsyncResult<Result.GetDistributorModelMasterResult>> callback
        )
		{
			var task = new GetDistributorModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.GetDistributorModelMasterResult>(task.Result, task.Error));
        }

		public IFuture<Result.GetDistributorModelMasterResult> GetDistributorModelMasterFuture(
                Request.GetDistributorModelMasterRequest request
        )
		{
			return new GetDistributorModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.GetDistributorModelMasterResult> GetDistributorModelMasterAsync(
                Request.GetDistributorModelMasterRequest request
        )
		{
            AsyncResult<Result.GetDistributorModelMasterResult> result = null;
			await GetDistributorModelMaster(
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
		public GetDistributorModelMasterTask GetDistributorModelMasterAsync(
                Request.GetDistributorModelMasterRequest request
        )
		{
			return new GetDistributorModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.GetDistributorModelMasterResult> GetDistributorModelMasterAsync(
                Request.GetDistributorModelMasterRequest request
        )
		{
			var task = new GetDistributorModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class UpdateDistributorModelMasterTask : Gs2RestSessionTask<UpdateDistributorModelMasterRequest, UpdateDistributorModelMasterResult>
        {
            public UpdateDistributorModelMasterTask(IGs2Session session, RestSessionRequestFactory factory, UpdateDistributorModelMasterRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(UpdateDistributorModelMasterRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "distributor")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/master/distributor/{distributorName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{distributorName}", !string.IsNullOrEmpty(request.DistributorName) ? request.DistributorName.ToString() : "null");

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
                if (request.InboxNamespaceId != null)
                {
                    jsonWriter.WritePropertyName("inboxNamespaceId");
                    jsonWriter.Write(request.InboxNamespaceId);
                }
                if (request.WhiteListTargetIds != null)
                {
                    jsonWriter.WritePropertyName("whiteListTargetIds");
                    jsonWriter.WriteArrayStart();
                    foreach(var item in request.WhiteListTargetIds)
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
		public IEnumerator UpdateDistributorModelMaster(
                Request.UpdateDistributorModelMasterRequest request,
                UnityAction<AsyncResult<Result.UpdateDistributorModelMasterResult>> callback
        )
		{
			var task = new UpdateDistributorModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.UpdateDistributorModelMasterResult>(task.Result, task.Error));
        }

		public IFuture<Result.UpdateDistributorModelMasterResult> UpdateDistributorModelMasterFuture(
                Request.UpdateDistributorModelMasterRequest request
        )
		{
			return new UpdateDistributorModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.UpdateDistributorModelMasterResult> UpdateDistributorModelMasterAsync(
                Request.UpdateDistributorModelMasterRequest request
        )
		{
            AsyncResult<Result.UpdateDistributorModelMasterResult> result = null;
			await UpdateDistributorModelMaster(
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
		public UpdateDistributorModelMasterTask UpdateDistributorModelMasterAsync(
                Request.UpdateDistributorModelMasterRequest request
        )
		{
			return new UpdateDistributorModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.UpdateDistributorModelMasterResult> UpdateDistributorModelMasterAsync(
                Request.UpdateDistributorModelMasterRequest request
        )
		{
			var task = new UpdateDistributorModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class DeleteDistributorModelMasterTask : Gs2RestSessionTask<DeleteDistributorModelMasterRequest, DeleteDistributorModelMasterResult>
        {
            public DeleteDistributorModelMasterTask(IGs2Session session, RestSessionRequestFactory factory, DeleteDistributorModelMasterRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DeleteDistributorModelMasterRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "distributor")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/master/distributor/{distributorName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{distributorName}", !string.IsNullOrEmpty(request.DistributorName) ? request.DistributorName.ToString() : "null");

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
		public IEnumerator DeleteDistributorModelMaster(
                Request.DeleteDistributorModelMasterRequest request,
                UnityAction<AsyncResult<Result.DeleteDistributorModelMasterResult>> callback
        )
		{
			var task = new DeleteDistributorModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.DeleteDistributorModelMasterResult>(task.Result, task.Error));
        }

		public IFuture<Result.DeleteDistributorModelMasterResult> DeleteDistributorModelMasterFuture(
                Request.DeleteDistributorModelMasterRequest request
        )
		{
			return new DeleteDistributorModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.DeleteDistributorModelMasterResult> DeleteDistributorModelMasterAsync(
                Request.DeleteDistributorModelMasterRequest request
        )
		{
            AsyncResult<Result.DeleteDistributorModelMasterResult> result = null;
			await DeleteDistributorModelMaster(
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
		public DeleteDistributorModelMasterTask DeleteDistributorModelMasterAsync(
                Request.DeleteDistributorModelMasterRequest request
        )
		{
			return new DeleteDistributorModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.DeleteDistributorModelMasterResult> DeleteDistributorModelMasterAsync(
                Request.DeleteDistributorModelMasterRequest request
        )
		{
			var task = new DeleteDistributorModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class DescribeDistributorModelsTask : Gs2RestSessionTask<DescribeDistributorModelsRequest, DescribeDistributorModelsResult>
        {
            public DescribeDistributorModelsTask(IGs2Session session, RestSessionRequestFactory factory, DescribeDistributorModelsRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DescribeDistributorModelsRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "distributor")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/distributor";

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
		public IEnumerator DescribeDistributorModels(
                Request.DescribeDistributorModelsRequest request,
                UnityAction<AsyncResult<Result.DescribeDistributorModelsResult>> callback
        )
		{
			var task = new DescribeDistributorModelsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.DescribeDistributorModelsResult>(task.Result, task.Error));
        }

		public IFuture<Result.DescribeDistributorModelsResult> DescribeDistributorModelsFuture(
                Request.DescribeDistributorModelsRequest request
        )
		{
			return new DescribeDistributorModelsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.DescribeDistributorModelsResult> DescribeDistributorModelsAsync(
                Request.DescribeDistributorModelsRequest request
        )
		{
            AsyncResult<Result.DescribeDistributorModelsResult> result = null;
			await DescribeDistributorModels(
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
		public DescribeDistributorModelsTask DescribeDistributorModelsAsync(
                Request.DescribeDistributorModelsRequest request
        )
		{
			return new DescribeDistributorModelsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.DescribeDistributorModelsResult> DescribeDistributorModelsAsync(
                Request.DescribeDistributorModelsRequest request
        )
		{
			var task = new DescribeDistributorModelsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class GetDistributorModelTask : Gs2RestSessionTask<GetDistributorModelRequest, GetDistributorModelResult>
        {
            public GetDistributorModelTask(IGs2Session session, RestSessionRequestFactory factory, GetDistributorModelRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(GetDistributorModelRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "distributor")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/distributor/{distributorName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{distributorName}", !string.IsNullOrEmpty(request.DistributorName) ? request.DistributorName.ToString() : "null");

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
		public IEnumerator GetDistributorModel(
                Request.GetDistributorModelRequest request,
                UnityAction<AsyncResult<Result.GetDistributorModelResult>> callback
        )
		{
			var task = new GetDistributorModelTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.GetDistributorModelResult>(task.Result, task.Error));
        }

		public IFuture<Result.GetDistributorModelResult> GetDistributorModelFuture(
                Request.GetDistributorModelRequest request
        )
		{
			return new GetDistributorModelTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.GetDistributorModelResult> GetDistributorModelAsync(
                Request.GetDistributorModelRequest request
        )
		{
            AsyncResult<Result.GetDistributorModelResult> result = null;
			await GetDistributorModel(
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
		public GetDistributorModelTask GetDistributorModelAsync(
                Request.GetDistributorModelRequest request
        )
		{
			return new GetDistributorModelTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.GetDistributorModelResult> GetDistributorModelAsync(
                Request.GetDistributorModelRequest request
        )
		{
			var task = new GetDistributorModelTask(
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
                    .Replace("{service}", "distributor")
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


        public class GetCurrentDistributorMasterTask : Gs2RestSessionTask<GetCurrentDistributorMasterRequest, GetCurrentDistributorMasterResult>
        {
            public GetCurrentDistributorMasterTask(IGs2Session session, RestSessionRequestFactory factory, GetCurrentDistributorMasterRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(GetCurrentDistributorMasterRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "distributor")
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
		public IEnumerator GetCurrentDistributorMaster(
                Request.GetCurrentDistributorMasterRequest request,
                UnityAction<AsyncResult<Result.GetCurrentDistributorMasterResult>> callback
        )
		{
			var task = new GetCurrentDistributorMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.GetCurrentDistributorMasterResult>(task.Result, task.Error));
        }

		public IFuture<Result.GetCurrentDistributorMasterResult> GetCurrentDistributorMasterFuture(
                Request.GetCurrentDistributorMasterRequest request
        )
		{
			return new GetCurrentDistributorMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.GetCurrentDistributorMasterResult> GetCurrentDistributorMasterAsync(
                Request.GetCurrentDistributorMasterRequest request
        )
		{
            AsyncResult<Result.GetCurrentDistributorMasterResult> result = null;
			await GetCurrentDistributorMaster(
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
		public GetCurrentDistributorMasterTask GetCurrentDistributorMasterAsync(
                Request.GetCurrentDistributorMasterRequest request
        )
		{
			return new GetCurrentDistributorMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.GetCurrentDistributorMasterResult> GetCurrentDistributorMasterAsync(
                Request.GetCurrentDistributorMasterRequest request
        )
		{
			var task = new GetCurrentDistributorMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class UpdateCurrentDistributorMasterTask : Gs2RestSessionTask<UpdateCurrentDistributorMasterRequest, UpdateCurrentDistributorMasterResult>
        {
            public UpdateCurrentDistributorMasterTask(IGs2Session session, RestSessionRequestFactory factory, UpdateCurrentDistributorMasterRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(UpdateCurrentDistributorMasterRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "distributor")
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
		public IEnumerator UpdateCurrentDistributorMaster(
                Request.UpdateCurrentDistributorMasterRequest request,
                UnityAction<AsyncResult<Result.UpdateCurrentDistributorMasterResult>> callback
        )
		{
			var task = new UpdateCurrentDistributorMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.UpdateCurrentDistributorMasterResult>(task.Result, task.Error));
        }

		public IFuture<Result.UpdateCurrentDistributorMasterResult> UpdateCurrentDistributorMasterFuture(
                Request.UpdateCurrentDistributorMasterRequest request
        )
		{
			return new UpdateCurrentDistributorMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.UpdateCurrentDistributorMasterResult> UpdateCurrentDistributorMasterAsync(
                Request.UpdateCurrentDistributorMasterRequest request
        )
		{
            AsyncResult<Result.UpdateCurrentDistributorMasterResult> result = null;
			await UpdateCurrentDistributorMaster(
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
		public UpdateCurrentDistributorMasterTask UpdateCurrentDistributorMasterAsync(
                Request.UpdateCurrentDistributorMasterRequest request
        )
		{
			return new UpdateCurrentDistributorMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.UpdateCurrentDistributorMasterResult> UpdateCurrentDistributorMasterAsync(
                Request.UpdateCurrentDistributorMasterRequest request
        )
		{
			var task = new UpdateCurrentDistributorMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class UpdateCurrentDistributorMasterFromGitHubTask : Gs2RestSessionTask<UpdateCurrentDistributorMasterFromGitHubRequest, UpdateCurrentDistributorMasterFromGitHubResult>
        {
            public UpdateCurrentDistributorMasterFromGitHubTask(IGs2Session session, RestSessionRequestFactory factory, UpdateCurrentDistributorMasterFromGitHubRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(UpdateCurrentDistributorMasterFromGitHubRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "distributor")
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
		public IEnumerator UpdateCurrentDistributorMasterFromGitHub(
                Request.UpdateCurrentDistributorMasterFromGitHubRequest request,
                UnityAction<AsyncResult<Result.UpdateCurrentDistributorMasterFromGitHubResult>> callback
        )
		{
			var task = new UpdateCurrentDistributorMasterFromGitHubTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.UpdateCurrentDistributorMasterFromGitHubResult>(task.Result, task.Error));
        }

		public IFuture<Result.UpdateCurrentDistributorMasterFromGitHubResult> UpdateCurrentDistributorMasterFromGitHubFuture(
                Request.UpdateCurrentDistributorMasterFromGitHubRequest request
        )
		{
			return new UpdateCurrentDistributorMasterFromGitHubTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.UpdateCurrentDistributorMasterFromGitHubResult> UpdateCurrentDistributorMasterFromGitHubAsync(
                Request.UpdateCurrentDistributorMasterFromGitHubRequest request
        )
		{
            AsyncResult<Result.UpdateCurrentDistributorMasterFromGitHubResult> result = null;
			await UpdateCurrentDistributorMasterFromGitHub(
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
		public UpdateCurrentDistributorMasterFromGitHubTask UpdateCurrentDistributorMasterFromGitHubAsync(
                Request.UpdateCurrentDistributorMasterFromGitHubRequest request
        )
		{
			return new UpdateCurrentDistributorMasterFromGitHubTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.UpdateCurrentDistributorMasterFromGitHubResult> UpdateCurrentDistributorMasterFromGitHubAsync(
                Request.UpdateCurrentDistributorMasterFromGitHubRequest request
        )
		{
			var task = new UpdateCurrentDistributorMasterFromGitHubTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class DistributeTask : Gs2RestSessionTask<DistributeRequest, DistributeResult>
        {
            public DistributeTask(IGs2Session session, RestSessionRequestFactory factory, DistributeRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DistributeRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "distributor")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/distribute/{distributorName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{distributorName}", !string.IsNullOrEmpty(request.DistributorName) ? request.DistributorName.ToString() : "null");

                var sessionRequest = Factory.Post(url);

                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);
                jsonWriter.WriteObjectStart();
                if (request.UserId != null)
                {
                    jsonWriter.WritePropertyName("userId");
                    jsonWriter.Write(request.UserId);
                }
                if (request.DistributeResource != null)
                {
                    jsonWriter.WritePropertyName("distributeResource");
                    request.DistributeResource.WriteJson(jsonWriter);
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
		public IEnumerator Distribute(
                Request.DistributeRequest request,
                UnityAction<AsyncResult<Result.DistributeResult>> callback
        )
		{
			var task = new DistributeTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.DistributeResult>(task.Result, task.Error));
        }

		public IFuture<Result.DistributeResult> DistributeFuture(
                Request.DistributeRequest request
        )
		{
			return new DistributeTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.DistributeResult> DistributeAsync(
                Request.DistributeRequest request
        )
		{
            AsyncResult<Result.DistributeResult> result = null;
			await Distribute(
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
		public DistributeTask DistributeAsync(
                Request.DistributeRequest request
        )
		{
			return new DistributeTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.DistributeResult> DistributeAsync(
                Request.DistributeRequest request
        )
		{
			var task = new DistributeTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class DistributeWithoutOverflowProcessTask : Gs2RestSessionTask<DistributeWithoutOverflowProcessRequest, DistributeWithoutOverflowProcessResult>
        {
            public DistributeWithoutOverflowProcessTask(IGs2Session session, RestSessionRequestFactory factory, DistributeWithoutOverflowProcessRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DistributeWithoutOverflowProcessRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "distributor")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/distribute";

                var sessionRequest = Factory.Post(url);

                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);
                jsonWriter.WriteObjectStart();
                if (request.UserId != null)
                {
                    jsonWriter.WritePropertyName("userId");
                    jsonWriter.Write(request.UserId);
                }
                if (request.DistributeResource != null)
                {
                    jsonWriter.WritePropertyName("distributeResource");
                    request.DistributeResource.WriteJson(jsonWriter);
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
		public IEnumerator DistributeWithoutOverflowProcess(
                Request.DistributeWithoutOverflowProcessRequest request,
                UnityAction<AsyncResult<Result.DistributeWithoutOverflowProcessResult>> callback
        )
		{
			var task = new DistributeWithoutOverflowProcessTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.DistributeWithoutOverflowProcessResult>(task.Result, task.Error));
        }

		public IFuture<Result.DistributeWithoutOverflowProcessResult> DistributeWithoutOverflowProcessFuture(
                Request.DistributeWithoutOverflowProcessRequest request
        )
		{
			return new DistributeWithoutOverflowProcessTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.DistributeWithoutOverflowProcessResult> DistributeWithoutOverflowProcessAsync(
                Request.DistributeWithoutOverflowProcessRequest request
        )
		{
            AsyncResult<Result.DistributeWithoutOverflowProcessResult> result = null;
			await DistributeWithoutOverflowProcess(
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
		public DistributeWithoutOverflowProcessTask DistributeWithoutOverflowProcessAsync(
                Request.DistributeWithoutOverflowProcessRequest request
        )
		{
			return new DistributeWithoutOverflowProcessTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.DistributeWithoutOverflowProcessResult> DistributeWithoutOverflowProcessAsync(
                Request.DistributeWithoutOverflowProcessRequest request
        )
		{
			var task = new DistributeWithoutOverflowProcessTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class RunVerifyTaskTask : Gs2RestSessionTask<RunVerifyTaskRequest, RunVerifyTaskResult>
        {
            public RunVerifyTaskTask(IGs2Session session, RestSessionRequestFactory factory, RunVerifyTaskRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(RunVerifyTaskRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "distributor")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/distribute/stamp/verifyTask/run";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");

                var sessionRequest = Factory.Post(url);

                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);
                jsonWriter.WriteObjectStart();
                if (request.VerifyTask != null)
                {
                    jsonWriter.WritePropertyName("verifyTask");
                    jsonWriter.Write(request.VerifyTask);
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
		public IEnumerator RunVerifyTask(
                Request.RunVerifyTaskRequest request,
                UnityAction<AsyncResult<Result.RunVerifyTaskResult>> callback
        )
		{
			var task = new RunVerifyTaskTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.RunVerifyTaskResult>(task.Result, task.Error));
        }

		public IFuture<Result.RunVerifyTaskResult> RunVerifyTaskFuture(
                Request.RunVerifyTaskRequest request
        )
		{
			return new RunVerifyTaskTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.RunVerifyTaskResult> RunVerifyTaskAsync(
                Request.RunVerifyTaskRequest request
        )
		{
            AsyncResult<Result.RunVerifyTaskResult> result = null;
			await RunVerifyTask(
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
		public RunVerifyTaskTask RunVerifyTaskAsync(
                Request.RunVerifyTaskRequest request
        )
		{
			return new RunVerifyTaskTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.RunVerifyTaskResult> RunVerifyTaskAsync(
                Request.RunVerifyTaskRequest request
        )
		{
			var task = new RunVerifyTaskTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class RunStampTaskTask : Gs2RestSessionTask<RunStampTaskRequest, RunStampTaskResult>
        {
            public RunStampTaskTask(IGs2Session session, RestSessionRequestFactory factory, RunStampTaskRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(RunStampTaskRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "distributor")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/distribute/stamp/task/run";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");

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
		public IEnumerator RunStampTask(
                Request.RunStampTaskRequest request,
                UnityAction<AsyncResult<Result.RunStampTaskResult>> callback
        )
		{
			var task = new RunStampTaskTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.RunStampTaskResult>(task.Result, task.Error));
        }

		public IFuture<Result.RunStampTaskResult> RunStampTaskFuture(
                Request.RunStampTaskRequest request
        )
		{
			return new RunStampTaskTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.RunStampTaskResult> RunStampTaskAsync(
                Request.RunStampTaskRequest request
        )
		{
            AsyncResult<Result.RunStampTaskResult> result = null;
			await RunStampTask(
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
		public RunStampTaskTask RunStampTaskAsync(
                Request.RunStampTaskRequest request
        )
		{
			return new RunStampTaskTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.RunStampTaskResult> RunStampTaskAsync(
                Request.RunStampTaskRequest request
        )
		{
			var task = new RunStampTaskTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class RunStampSheetTask : Gs2RestSessionTask<RunStampSheetRequest, RunStampSheetResult>
        {
            public RunStampSheetTask(IGs2Session session, RestSessionRequestFactory factory, RunStampSheetRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(RunStampSheetRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "distributor")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/distribute/stamp/sheet/run";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");

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
		public IEnumerator RunStampSheet(
                Request.RunStampSheetRequest request,
                UnityAction<AsyncResult<Result.RunStampSheetResult>> callback
        )
		{
			var task = new RunStampSheetTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.RunStampSheetResult>(task.Result, task.Error));
        }

		public IFuture<Result.RunStampSheetResult> RunStampSheetFuture(
                Request.RunStampSheetRequest request
        )
		{
			return new RunStampSheetTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.RunStampSheetResult> RunStampSheetAsync(
                Request.RunStampSheetRequest request
        )
		{
            AsyncResult<Result.RunStampSheetResult> result = null;
			await RunStampSheet(
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
		public RunStampSheetTask RunStampSheetAsync(
                Request.RunStampSheetRequest request
        )
		{
			return new RunStampSheetTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.RunStampSheetResult> RunStampSheetAsync(
                Request.RunStampSheetRequest request
        )
		{
			var task = new RunStampSheetTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class RunStampSheetExpressTask : Gs2RestSessionTask<RunStampSheetExpressRequest, RunStampSheetExpressResult>
        {
            public RunStampSheetExpressTask(IGs2Session session, RestSessionRequestFactory factory, RunStampSheetExpressRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(RunStampSheetExpressRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "distributor")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/distribute/stamp/run";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");

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
		public IEnumerator RunStampSheetExpress(
                Request.RunStampSheetExpressRequest request,
                UnityAction<AsyncResult<Result.RunStampSheetExpressResult>> callback
        )
		{
			var task = new RunStampSheetExpressTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.RunStampSheetExpressResult>(task.Result, task.Error));
        }

		public IFuture<Result.RunStampSheetExpressResult> RunStampSheetExpressFuture(
                Request.RunStampSheetExpressRequest request
        )
		{
			return new RunStampSheetExpressTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.RunStampSheetExpressResult> RunStampSheetExpressAsync(
                Request.RunStampSheetExpressRequest request
        )
		{
            AsyncResult<Result.RunStampSheetExpressResult> result = null;
			await RunStampSheetExpress(
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
		public RunStampSheetExpressTask RunStampSheetExpressAsync(
                Request.RunStampSheetExpressRequest request
        )
		{
			return new RunStampSheetExpressTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.RunStampSheetExpressResult> RunStampSheetExpressAsync(
                Request.RunStampSheetExpressRequest request
        )
		{
			var task = new RunStampSheetExpressTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class RunVerifyTaskWithoutNamespaceTask : Gs2RestSessionTask<RunVerifyTaskWithoutNamespaceRequest, RunVerifyTaskWithoutNamespaceResult>
        {
            public RunVerifyTaskWithoutNamespaceTask(IGs2Session session, RestSessionRequestFactory factory, RunVerifyTaskWithoutNamespaceRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(RunVerifyTaskWithoutNamespaceRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "distributor")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/stamp/verifyTask/run";

                var sessionRequest = Factory.Post(url);

                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);
                jsonWriter.WriteObjectStart();
                if (request.VerifyTask != null)
                {
                    jsonWriter.WritePropertyName("verifyTask");
                    jsonWriter.Write(request.VerifyTask);
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
		public IEnumerator RunVerifyTaskWithoutNamespace(
                Request.RunVerifyTaskWithoutNamespaceRequest request,
                UnityAction<AsyncResult<Result.RunVerifyTaskWithoutNamespaceResult>> callback
        )
		{
			var task = new RunVerifyTaskWithoutNamespaceTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.RunVerifyTaskWithoutNamespaceResult>(task.Result, task.Error));
        }

		public IFuture<Result.RunVerifyTaskWithoutNamespaceResult> RunVerifyTaskWithoutNamespaceFuture(
                Request.RunVerifyTaskWithoutNamespaceRequest request
        )
		{
			return new RunVerifyTaskWithoutNamespaceTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.RunVerifyTaskWithoutNamespaceResult> RunVerifyTaskWithoutNamespaceAsync(
                Request.RunVerifyTaskWithoutNamespaceRequest request
        )
		{
            AsyncResult<Result.RunVerifyTaskWithoutNamespaceResult> result = null;
			await RunVerifyTaskWithoutNamespace(
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
		public RunVerifyTaskWithoutNamespaceTask RunVerifyTaskWithoutNamespaceAsync(
                Request.RunVerifyTaskWithoutNamespaceRequest request
        )
		{
			return new RunVerifyTaskWithoutNamespaceTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.RunVerifyTaskWithoutNamespaceResult> RunVerifyTaskWithoutNamespaceAsync(
                Request.RunVerifyTaskWithoutNamespaceRequest request
        )
		{
			var task = new RunVerifyTaskWithoutNamespaceTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class RunStampTaskWithoutNamespaceTask : Gs2RestSessionTask<RunStampTaskWithoutNamespaceRequest, RunStampTaskWithoutNamespaceResult>
        {
            public RunStampTaskWithoutNamespaceTask(IGs2Session session, RestSessionRequestFactory factory, RunStampTaskWithoutNamespaceRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(RunStampTaskWithoutNamespaceRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "distributor")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/stamp/task/run";

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
		public IEnumerator RunStampTaskWithoutNamespace(
                Request.RunStampTaskWithoutNamespaceRequest request,
                UnityAction<AsyncResult<Result.RunStampTaskWithoutNamespaceResult>> callback
        )
		{
			var task = new RunStampTaskWithoutNamespaceTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.RunStampTaskWithoutNamespaceResult>(task.Result, task.Error));
        }

		public IFuture<Result.RunStampTaskWithoutNamespaceResult> RunStampTaskWithoutNamespaceFuture(
                Request.RunStampTaskWithoutNamespaceRequest request
        )
		{
			return new RunStampTaskWithoutNamespaceTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.RunStampTaskWithoutNamespaceResult> RunStampTaskWithoutNamespaceAsync(
                Request.RunStampTaskWithoutNamespaceRequest request
        )
		{
            AsyncResult<Result.RunStampTaskWithoutNamespaceResult> result = null;
			await RunStampTaskWithoutNamespace(
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
		public RunStampTaskWithoutNamespaceTask RunStampTaskWithoutNamespaceAsync(
                Request.RunStampTaskWithoutNamespaceRequest request
        )
		{
			return new RunStampTaskWithoutNamespaceTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.RunStampTaskWithoutNamespaceResult> RunStampTaskWithoutNamespaceAsync(
                Request.RunStampTaskWithoutNamespaceRequest request
        )
		{
			var task = new RunStampTaskWithoutNamespaceTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class RunStampSheetWithoutNamespaceTask : Gs2RestSessionTask<RunStampSheetWithoutNamespaceRequest, RunStampSheetWithoutNamespaceResult>
        {
            public RunStampSheetWithoutNamespaceTask(IGs2Session session, RestSessionRequestFactory factory, RunStampSheetWithoutNamespaceRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(RunStampSheetWithoutNamespaceRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "distributor")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/stamp/sheet/run";

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
		public IEnumerator RunStampSheetWithoutNamespace(
                Request.RunStampSheetWithoutNamespaceRequest request,
                UnityAction<AsyncResult<Result.RunStampSheetWithoutNamespaceResult>> callback
        )
		{
			var task = new RunStampSheetWithoutNamespaceTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.RunStampSheetWithoutNamespaceResult>(task.Result, task.Error));
        }

		public IFuture<Result.RunStampSheetWithoutNamespaceResult> RunStampSheetWithoutNamespaceFuture(
                Request.RunStampSheetWithoutNamespaceRequest request
        )
		{
			return new RunStampSheetWithoutNamespaceTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.RunStampSheetWithoutNamespaceResult> RunStampSheetWithoutNamespaceAsync(
                Request.RunStampSheetWithoutNamespaceRequest request
        )
		{
            AsyncResult<Result.RunStampSheetWithoutNamespaceResult> result = null;
			await RunStampSheetWithoutNamespace(
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
		public RunStampSheetWithoutNamespaceTask RunStampSheetWithoutNamespaceAsync(
                Request.RunStampSheetWithoutNamespaceRequest request
        )
		{
			return new RunStampSheetWithoutNamespaceTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.RunStampSheetWithoutNamespaceResult> RunStampSheetWithoutNamespaceAsync(
                Request.RunStampSheetWithoutNamespaceRequest request
        )
		{
			var task = new RunStampSheetWithoutNamespaceTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class RunStampSheetExpressWithoutNamespaceTask : Gs2RestSessionTask<RunStampSheetExpressWithoutNamespaceRequest, RunStampSheetExpressWithoutNamespaceResult>
        {
            public RunStampSheetExpressWithoutNamespaceTask(IGs2Session session, RestSessionRequestFactory factory, RunStampSheetExpressWithoutNamespaceRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(RunStampSheetExpressWithoutNamespaceRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "distributor")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/stamp/run";

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
		public IEnumerator RunStampSheetExpressWithoutNamespace(
                Request.RunStampSheetExpressWithoutNamespaceRequest request,
                UnityAction<AsyncResult<Result.RunStampSheetExpressWithoutNamespaceResult>> callback
        )
		{
			var task = new RunStampSheetExpressWithoutNamespaceTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.RunStampSheetExpressWithoutNamespaceResult>(task.Result, task.Error));
        }

		public IFuture<Result.RunStampSheetExpressWithoutNamespaceResult> RunStampSheetExpressWithoutNamespaceFuture(
                Request.RunStampSheetExpressWithoutNamespaceRequest request
        )
		{
			return new RunStampSheetExpressWithoutNamespaceTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.RunStampSheetExpressWithoutNamespaceResult> RunStampSheetExpressWithoutNamespaceAsync(
                Request.RunStampSheetExpressWithoutNamespaceRequest request
        )
		{
            AsyncResult<Result.RunStampSheetExpressWithoutNamespaceResult> result = null;
			await RunStampSheetExpressWithoutNamespace(
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
		public RunStampSheetExpressWithoutNamespaceTask RunStampSheetExpressWithoutNamespaceAsync(
                Request.RunStampSheetExpressWithoutNamespaceRequest request
        )
		{
			return new RunStampSheetExpressWithoutNamespaceTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.RunStampSheetExpressWithoutNamespaceResult> RunStampSheetExpressWithoutNamespaceAsync(
                Request.RunStampSheetExpressWithoutNamespaceRequest request
        )
		{
			var task = new RunStampSheetExpressWithoutNamespaceTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class SetTransactionDefaultConfigTask : Gs2RestSessionTask<SetTransactionDefaultConfigRequest, SetTransactionDefaultConfigResult>
        {
            public SetTransactionDefaultConfigTask(IGs2Session session, RestSessionRequestFactory factory, SetTransactionDefaultConfigRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(SetTransactionDefaultConfigRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "distributor")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/transaction/user/me/config";

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
		public IEnumerator SetTransactionDefaultConfig(
                Request.SetTransactionDefaultConfigRequest request,
                UnityAction<AsyncResult<Result.SetTransactionDefaultConfigResult>> callback
        )
		{
			var task = new SetTransactionDefaultConfigTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.SetTransactionDefaultConfigResult>(task.Result, task.Error));
        }

		public IFuture<Result.SetTransactionDefaultConfigResult> SetTransactionDefaultConfigFuture(
                Request.SetTransactionDefaultConfigRequest request
        )
		{
			return new SetTransactionDefaultConfigTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.SetTransactionDefaultConfigResult> SetTransactionDefaultConfigAsync(
                Request.SetTransactionDefaultConfigRequest request
        )
		{
            AsyncResult<Result.SetTransactionDefaultConfigResult> result = null;
			await SetTransactionDefaultConfig(
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
		public SetTransactionDefaultConfigTask SetTransactionDefaultConfigAsync(
                Request.SetTransactionDefaultConfigRequest request
        )
		{
			return new SetTransactionDefaultConfigTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.SetTransactionDefaultConfigResult> SetTransactionDefaultConfigAsync(
                Request.SetTransactionDefaultConfigRequest request
        )
		{
			var task = new SetTransactionDefaultConfigTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class SetTransactionDefaultConfigByUserIdTask : Gs2RestSessionTask<SetTransactionDefaultConfigByUserIdRequest, SetTransactionDefaultConfigByUserIdResult>
        {
            public SetTransactionDefaultConfigByUserIdTask(IGs2Session session, RestSessionRequestFactory factory, SetTransactionDefaultConfigByUserIdRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(SetTransactionDefaultConfigByUserIdRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "distributor")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/transaction/user/{userId}/config";

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
		public IEnumerator SetTransactionDefaultConfigByUserId(
                Request.SetTransactionDefaultConfigByUserIdRequest request,
                UnityAction<AsyncResult<Result.SetTransactionDefaultConfigByUserIdResult>> callback
        )
		{
			var task = new SetTransactionDefaultConfigByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.SetTransactionDefaultConfigByUserIdResult>(task.Result, task.Error));
        }

		public IFuture<Result.SetTransactionDefaultConfigByUserIdResult> SetTransactionDefaultConfigByUserIdFuture(
                Request.SetTransactionDefaultConfigByUserIdRequest request
        )
		{
			return new SetTransactionDefaultConfigByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.SetTransactionDefaultConfigByUserIdResult> SetTransactionDefaultConfigByUserIdAsync(
                Request.SetTransactionDefaultConfigByUserIdRequest request
        )
		{
            AsyncResult<Result.SetTransactionDefaultConfigByUserIdResult> result = null;
			await SetTransactionDefaultConfigByUserId(
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
		public SetTransactionDefaultConfigByUserIdTask SetTransactionDefaultConfigByUserIdAsync(
                Request.SetTransactionDefaultConfigByUserIdRequest request
        )
		{
			return new SetTransactionDefaultConfigByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.SetTransactionDefaultConfigByUserIdResult> SetTransactionDefaultConfigByUserIdAsync(
                Request.SetTransactionDefaultConfigByUserIdRequest request
        )
		{
			var task = new SetTransactionDefaultConfigByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class FreezeMasterDataTask : Gs2RestSessionTask<FreezeMasterDataRequest, FreezeMasterDataResult>
        {
            public FreezeMasterDataTask(IGs2Session session, RestSessionRequestFactory factory, FreezeMasterDataRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(FreezeMasterDataRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "distributor")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/me/masterdata/freeze";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");

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
		public IEnumerator FreezeMasterData(
                Request.FreezeMasterDataRequest request,
                UnityAction<AsyncResult<Result.FreezeMasterDataResult>> callback
        )
		{
			var task = new FreezeMasterDataTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.FreezeMasterDataResult>(task.Result, task.Error));
        }

		public IFuture<Result.FreezeMasterDataResult> FreezeMasterDataFuture(
                Request.FreezeMasterDataRequest request
        )
		{
			return new FreezeMasterDataTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.FreezeMasterDataResult> FreezeMasterDataAsync(
                Request.FreezeMasterDataRequest request
        )
		{
            AsyncResult<Result.FreezeMasterDataResult> result = null;
			await FreezeMasterData(
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
		public FreezeMasterDataTask FreezeMasterDataAsync(
                Request.FreezeMasterDataRequest request
        )
		{
			return new FreezeMasterDataTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.FreezeMasterDataResult> FreezeMasterDataAsync(
                Request.FreezeMasterDataRequest request
        )
		{
			var task = new FreezeMasterDataTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class FreezeMasterDataByUserIdTask : Gs2RestSessionTask<FreezeMasterDataByUserIdRequest, FreezeMasterDataByUserIdResult>
        {
            public FreezeMasterDataByUserIdTask(IGs2Session session, RestSessionRequestFactory factory, FreezeMasterDataByUserIdRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(FreezeMasterDataByUserIdRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "distributor")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/{userId}/masterdata/freeze";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{userId}", !string.IsNullOrEmpty(request.UserId) ? request.UserId.ToString() : "null");

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
		public IEnumerator FreezeMasterDataByUserId(
                Request.FreezeMasterDataByUserIdRequest request,
                UnityAction<AsyncResult<Result.FreezeMasterDataByUserIdResult>> callback
        )
		{
			var task = new FreezeMasterDataByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.FreezeMasterDataByUserIdResult>(task.Result, task.Error));
        }

		public IFuture<Result.FreezeMasterDataByUserIdResult> FreezeMasterDataByUserIdFuture(
                Request.FreezeMasterDataByUserIdRequest request
        )
		{
			return new FreezeMasterDataByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.FreezeMasterDataByUserIdResult> FreezeMasterDataByUserIdAsync(
                Request.FreezeMasterDataByUserIdRequest request
        )
		{
            AsyncResult<Result.FreezeMasterDataByUserIdResult> result = null;
			await FreezeMasterDataByUserId(
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
		public FreezeMasterDataByUserIdTask FreezeMasterDataByUserIdAsync(
                Request.FreezeMasterDataByUserIdRequest request
        )
		{
			return new FreezeMasterDataByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.FreezeMasterDataByUserIdResult> FreezeMasterDataByUserIdAsync(
                Request.FreezeMasterDataByUserIdRequest request
        )
		{
			var task = new FreezeMasterDataByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class IfExpressionByUserIdTask : Gs2RestSessionTask<IfExpressionByUserIdRequest, IfExpressionByUserIdResult>
        {
            public IfExpressionByUserIdTask(IGs2Session session, RestSessionRequestFactory factory, IfExpressionByUserIdRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(IfExpressionByUserIdRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "distributor")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/expression/if";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");

                var sessionRequest = Factory.Post(url);

                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);
                jsonWriter.WriteObjectStart();
                if (request.UserId != null)
                {
                    jsonWriter.WritePropertyName("userId");
                    jsonWriter.Write(request.UserId);
                }
                if (request.Condition != null)
                {
                    jsonWriter.WritePropertyName("condition");
                    request.Condition.WriteJson(jsonWriter);
                }
                if (request.TrueActions != null)
                {
                    jsonWriter.WritePropertyName("trueActions");
                    jsonWriter.WriteArrayStart();
                    foreach(var item in request.TrueActions)
                    {
                        item.WriteJson(jsonWriter);
                    }
                    jsonWriter.WriteArrayEnd();
                }
                if (request.FalseActions != null)
                {
                    jsonWriter.WritePropertyName("falseActions");
                    jsonWriter.WriteArrayStart();
                    foreach(var item in request.FalseActions)
                    {
                        item.WriteJson(jsonWriter);
                    }
                    jsonWriter.WriteArrayEnd();
                }
                if (request.MultiplyValueSpecifyingQuantity != null)
                {
                    jsonWriter.WritePropertyName("multiplyValueSpecifyingQuantity");
                    jsonWriter.Write(request.MultiplyValueSpecifyingQuantity.ToString());
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
		public IEnumerator IfExpressionByUserId(
                Request.IfExpressionByUserIdRequest request,
                UnityAction<AsyncResult<Result.IfExpressionByUserIdResult>> callback
        )
		{
			var task = new IfExpressionByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.IfExpressionByUserIdResult>(task.Result, task.Error));
        }

		public IFuture<Result.IfExpressionByUserIdResult> IfExpressionByUserIdFuture(
                Request.IfExpressionByUserIdRequest request
        )
		{
			return new IfExpressionByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.IfExpressionByUserIdResult> IfExpressionByUserIdAsync(
                Request.IfExpressionByUserIdRequest request
        )
		{
            AsyncResult<Result.IfExpressionByUserIdResult> result = null;
			await IfExpressionByUserId(
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
		public IfExpressionByUserIdTask IfExpressionByUserIdAsync(
                Request.IfExpressionByUserIdRequest request
        )
		{
			return new IfExpressionByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.IfExpressionByUserIdResult> IfExpressionByUserIdAsync(
                Request.IfExpressionByUserIdRequest request
        )
		{
			var task = new IfExpressionByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class AndExpressionByUserIdTask : Gs2RestSessionTask<AndExpressionByUserIdRequest, AndExpressionByUserIdResult>
        {
            public AndExpressionByUserIdTask(IGs2Session session, RestSessionRequestFactory factory, AndExpressionByUserIdRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(AndExpressionByUserIdRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "distributor")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/expression/and";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");

                var sessionRequest = Factory.Post(url);

                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);
                jsonWriter.WriteObjectStart();
                if (request.UserId != null)
                {
                    jsonWriter.WritePropertyName("userId");
                    jsonWriter.Write(request.UserId);
                }
                if (request.Actions != null)
                {
                    jsonWriter.WritePropertyName("actions");
                    jsonWriter.WriteArrayStart();
                    foreach(var item in request.Actions)
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
		public IEnumerator AndExpressionByUserId(
                Request.AndExpressionByUserIdRequest request,
                UnityAction<AsyncResult<Result.AndExpressionByUserIdResult>> callback
        )
		{
			var task = new AndExpressionByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.AndExpressionByUserIdResult>(task.Result, task.Error));
        }

		public IFuture<Result.AndExpressionByUserIdResult> AndExpressionByUserIdFuture(
                Request.AndExpressionByUserIdRequest request
        )
		{
			return new AndExpressionByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.AndExpressionByUserIdResult> AndExpressionByUserIdAsync(
                Request.AndExpressionByUserIdRequest request
        )
		{
            AsyncResult<Result.AndExpressionByUserIdResult> result = null;
			await AndExpressionByUserId(
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
		public AndExpressionByUserIdTask AndExpressionByUserIdAsync(
                Request.AndExpressionByUserIdRequest request
        )
		{
			return new AndExpressionByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.AndExpressionByUserIdResult> AndExpressionByUserIdAsync(
                Request.AndExpressionByUserIdRequest request
        )
		{
			var task = new AndExpressionByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class OrExpressionByUserIdTask : Gs2RestSessionTask<OrExpressionByUserIdRequest, OrExpressionByUserIdResult>
        {
            public OrExpressionByUserIdTask(IGs2Session session, RestSessionRequestFactory factory, OrExpressionByUserIdRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(OrExpressionByUserIdRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "distributor")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/expression/or";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");

                var sessionRequest = Factory.Post(url);

                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);
                jsonWriter.WriteObjectStart();
                if (request.UserId != null)
                {
                    jsonWriter.WritePropertyName("userId");
                    jsonWriter.Write(request.UserId);
                }
                if (request.Actions != null)
                {
                    jsonWriter.WritePropertyName("actions");
                    jsonWriter.WriteArrayStart();
                    foreach(var item in request.Actions)
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
		public IEnumerator OrExpressionByUserId(
                Request.OrExpressionByUserIdRequest request,
                UnityAction<AsyncResult<Result.OrExpressionByUserIdResult>> callback
        )
		{
			var task = new OrExpressionByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.OrExpressionByUserIdResult>(task.Result, task.Error));
        }

		public IFuture<Result.OrExpressionByUserIdResult> OrExpressionByUserIdFuture(
                Request.OrExpressionByUserIdRequest request
        )
		{
			return new OrExpressionByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.OrExpressionByUserIdResult> OrExpressionByUserIdAsync(
                Request.OrExpressionByUserIdRequest request
        )
		{
            AsyncResult<Result.OrExpressionByUserIdResult> result = null;
			await OrExpressionByUserId(
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
		public OrExpressionByUserIdTask OrExpressionByUserIdAsync(
                Request.OrExpressionByUserIdRequest request
        )
		{
			return new OrExpressionByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.OrExpressionByUserIdResult> OrExpressionByUserIdAsync(
                Request.OrExpressionByUserIdRequest request
        )
		{
			var task = new OrExpressionByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class IfExpressionByStampTaskTask : Gs2RestSessionTask<IfExpressionByStampTaskRequest, IfExpressionByStampTaskResult>
        {
            public IfExpressionByStampTaskTask(IGs2Session session, RestSessionRequestFactory factory, IfExpressionByStampTaskRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(IfExpressionByStampTaskRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "distributor")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/stamp/expression/if";

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
		public IEnumerator IfExpressionByStampTask(
                Request.IfExpressionByStampTaskRequest request,
                UnityAction<AsyncResult<Result.IfExpressionByStampTaskResult>> callback
        )
		{
			var task = new IfExpressionByStampTaskTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.IfExpressionByStampTaskResult>(task.Result, task.Error));
        }

		public IFuture<Result.IfExpressionByStampTaskResult> IfExpressionByStampTaskFuture(
                Request.IfExpressionByStampTaskRequest request
        )
		{
			return new IfExpressionByStampTaskTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.IfExpressionByStampTaskResult> IfExpressionByStampTaskAsync(
                Request.IfExpressionByStampTaskRequest request
        )
		{
            AsyncResult<Result.IfExpressionByStampTaskResult> result = null;
			await IfExpressionByStampTask(
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
		public IfExpressionByStampTaskTask IfExpressionByStampTaskAsync(
                Request.IfExpressionByStampTaskRequest request
        )
		{
			return new IfExpressionByStampTaskTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.IfExpressionByStampTaskResult> IfExpressionByStampTaskAsync(
                Request.IfExpressionByStampTaskRequest request
        )
		{
			var task = new IfExpressionByStampTaskTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class AndExpressionByStampTaskTask : Gs2RestSessionTask<AndExpressionByStampTaskRequest, AndExpressionByStampTaskResult>
        {
            public AndExpressionByStampTaskTask(IGs2Session session, RestSessionRequestFactory factory, AndExpressionByStampTaskRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(AndExpressionByStampTaskRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "distributor")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/stamp/expression/and";

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
		public IEnumerator AndExpressionByStampTask(
                Request.AndExpressionByStampTaskRequest request,
                UnityAction<AsyncResult<Result.AndExpressionByStampTaskResult>> callback
        )
		{
			var task = new AndExpressionByStampTaskTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.AndExpressionByStampTaskResult>(task.Result, task.Error));
        }

		public IFuture<Result.AndExpressionByStampTaskResult> AndExpressionByStampTaskFuture(
                Request.AndExpressionByStampTaskRequest request
        )
		{
			return new AndExpressionByStampTaskTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.AndExpressionByStampTaskResult> AndExpressionByStampTaskAsync(
                Request.AndExpressionByStampTaskRequest request
        )
		{
            AsyncResult<Result.AndExpressionByStampTaskResult> result = null;
			await AndExpressionByStampTask(
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
		public AndExpressionByStampTaskTask AndExpressionByStampTaskAsync(
                Request.AndExpressionByStampTaskRequest request
        )
		{
			return new AndExpressionByStampTaskTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.AndExpressionByStampTaskResult> AndExpressionByStampTaskAsync(
                Request.AndExpressionByStampTaskRequest request
        )
		{
			var task = new AndExpressionByStampTaskTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class OrExpressionByStampTaskTask : Gs2RestSessionTask<OrExpressionByStampTaskRequest, OrExpressionByStampTaskResult>
        {
            public OrExpressionByStampTaskTask(IGs2Session session, RestSessionRequestFactory factory, OrExpressionByStampTaskRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(OrExpressionByStampTaskRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "distributor")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/stamp/expression/or";

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
		public IEnumerator OrExpressionByStampTask(
                Request.OrExpressionByStampTaskRequest request,
                UnityAction<AsyncResult<Result.OrExpressionByStampTaskResult>> callback
        )
		{
			var task = new OrExpressionByStampTaskTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.OrExpressionByStampTaskResult>(task.Result, task.Error));
        }

		public IFuture<Result.OrExpressionByStampTaskResult> OrExpressionByStampTaskFuture(
                Request.OrExpressionByStampTaskRequest request
        )
		{
			return new OrExpressionByStampTaskTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.OrExpressionByStampTaskResult> OrExpressionByStampTaskAsync(
                Request.OrExpressionByStampTaskRequest request
        )
		{
            AsyncResult<Result.OrExpressionByStampTaskResult> result = null;
			await OrExpressionByStampTask(
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
		public OrExpressionByStampTaskTask OrExpressionByStampTaskAsync(
                Request.OrExpressionByStampTaskRequest request
        )
		{
			return new OrExpressionByStampTaskTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.OrExpressionByStampTaskResult> OrExpressionByStampTaskAsync(
                Request.OrExpressionByStampTaskRequest request
        )
		{
			var task = new OrExpressionByStampTaskTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class GetStampSheetResultTask : Gs2RestSessionTask<GetStampSheetResultRequest, GetStampSheetResultResult>
        {
            public GetStampSheetResultTask(IGs2Session session, RestSessionRequestFactory factory, GetStampSheetResultRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(GetStampSheetResultRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "distributor")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/me/stampSheet/{transactionId}/result";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{transactionId}", !string.IsNullOrEmpty(request.TransactionId) ? request.TransactionId.ToString() : "null");

                var sessionRequest = Factory.Get(url);
                if (request.ContextStack != null)
                {
                    sessionRequest.AddQueryString("contextStack", request.ContextStack);
                }
                if (request.AccessToken != null)
                {
                    sessionRequest.AddHeader("X-GS2-ACCESS-TOKEN", request.AccessToken);
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
		public IEnumerator GetStampSheetResult(
                Request.GetStampSheetResultRequest request,
                UnityAction<AsyncResult<Result.GetStampSheetResultResult>> callback
        )
		{
			var task = new GetStampSheetResultTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.GetStampSheetResultResult>(task.Result, task.Error));
        }

		public IFuture<Result.GetStampSheetResultResult> GetStampSheetResultFuture(
                Request.GetStampSheetResultRequest request
        )
		{
			return new GetStampSheetResultTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.GetStampSheetResultResult> GetStampSheetResultAsync(
                Request.GetStampSheetResultRequest request
        )
		{
            AsyncResult<Result.GetStampSheetResultResult> result = null;
			await GetStampSheetResult(
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
		public GetStampSheetResultTask GetStampSheetResultAsync(
                Request.GetStampSheetResultRequest request
        )
		{
			return new GetStampSheetResultTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.GetStampSheetResultResult> GetStampSheetResultAsync(
                Request.GetStampSheetResultRequest request
        )
		{
			var task = new GetStampSheetResultTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class GetStampSheetResultByUserIdTask : Gs2RestSessionTask<GetStampSheetResultByUserIdRequest, GetStampSheetResultByUserIdResult>
        {
            public GetStampSheetResultByUserIdTask(IGs2Session session, RestSessionRequestFactory factory, GetStampSheetResultByUserIdRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(GetStampSheetResultByUserIdRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "distributor")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/{userId}/stampSheet/{transactionId}/result";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{userId}", !string.IsNullOrEmpty(request.UserId) ? request.UserId.ToString() : "null");
                url = url.Replace("{transactionId}", !string.IsNullOrEmpty(request.TransactionId) ? request.TransactionId.ToString() : "null");

                var sessionRequest = Factory.Get(url);
                if (request.ContextStack != null)
                {
                    sessionRequest.AddQueryString("contextStack", request.ContextStack);
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
		public IEnumerator GetStampSheetResultByUserId(
                Request.GetStampSheetResultByUserIdRequest request,
                UnityAction<AsyncResult<Result.GetStampSheetResultByUserIdResult>> callback
        )
		{
			var task = new GetStampSheetResultByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.GetStampSheetResultByUserIdResult>(task.Result, task.Error));
        }

		public IFuture<Result.GetStampSheetResultByUserIdResult> GetStampSheetResultByUserIdFuture(
                Request.GetStampSheetResultByUserIdRequest request
        )
		{
			return new GetStampSheetResultByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.GetStampSheetResultByUserIdResult> GetStampSheetResultByUserIdAsync(
                Request.GetStampSheetResultByUserIdRequest request
        )
		{
            AsyncResult<Result.GetStampSheetResultByUserIdResult> result = null;
			await GetStampSheetResultByUserId(
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
		public GetStampSheetResultByUserIdTask GetStampSheetResultByUserIdAsync(
                Request.GetStampSheetResultByUserIdRequest request
        )
		{
			return new GetStampSheetResultByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.GetStampSheetResultByUserIdResult> GetStampSheetResultByUserIdAsync(
                Request.GetStampSheetResultByUserIdRequest request
        )
		{
			var task = new GetStampSheetResultByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class RunTransactionTask : Gs2RestSessionTask<RunTransactionRequest, RunTransactionResult>
        {
            public RunTransactionTask(IGs2Session session, RestSessionRequestFactory factory, RunTransactionRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(RunTransactionRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "distributor")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/system/{ownerId}/{namespaceName}/user/{userId}/transaction/run";

                url = url.Replace("{ownerId}", !string.IsNullOrEmpty(request.OwnerId) ? request.OwnerId.ToString() : "null");
                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{userId}", !string.IsNullOrEmpty(request.UserId) ? request.UserId.ToString() : "null");

                var sessionRequest = Factory.Post(url);

                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);
                jsonWriter.WriteObjectStart();
                if (request.Transaction != null)
                {
                    jsonWriter.WritePropertyName("transaction");
                    jsonWriter.Write(request.Transaction);
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
		public IEnumerator RunTransaction(
                Request.RunTransactionRequest request,
                UnityAction<AsyncResult<Result.RunTransactionResult>> callback
        )
		{
			var task = new RunTransactionTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.RunTransactionResult>(task.Result, task.Error));
        }

		public IFuture<Result.RunTransactionResult> RunTransactionFuture(
                Request.RunTransactionRequest request
        )
		{
			return new RunTransactionTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.RunTransactionResult> RunTransactionAsync(
                Request.RunTransactionRequest request
        )
		{
            AsyncResult<Result.RunTransactionResult> result = null;
			await RunTransaction(
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
		public RunTransactionTask RunTransactionAsync(
                Request.RunTransactionRequest request
        )
		{
			return new RunTransactionTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.RunTransactionResult> RunTransactionAsync(
                Request.RunTransactionRequest request
        )
		{
			var task = new RunTransactionTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class GetTransactionResultTask : Gs2RestSessionTask<GetTransactionResultRequest, GetTransactionResultResult>
        {
            public GetTransactionResultTask(IGs2Session session, RestSessionRequestFactory factory, GetTransactionResultRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(GetTransactionResultRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "distributor")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/me/transaction/{transactionId}/result";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{transactionId}", !string.IsNullOrEmpty(request.TransactionId) ? request.TransactionId.ToString() : "null");

                var sessionRequest = Factory.Get(url);
                if (request.ContextStack != null)
                {
                    sessionRequest.AddQueryString("contextStack", request.ContextStack);
                }
                if (request.AccessToken != null)
                {
                    sessionRequest.AddHeader("X-GS2-ACCESS-TOKEN", request.AccessToken);
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
		public IEnumerator GetTransactionResult(
                Request.GetTransactionResultRequest request,
                UnityAction<AsyncResult<Result.GetTransactionResultResult>> callback
        )
		{
			var task = new GetTransactionResultTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.GetTransactionResultResult>(task.Result, task.Error));
        }

		public IFuture<Result.GetTransactionResultResult> GetTransactionResultFuture(
                Request.GetTransactionResultRequest request
        )
		{
			return new GetTransactionResultTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.GetTransactionResultResult> GetTransactionResultAsync(
                Request.GetTransactionResultRequest request
        )
		{
            AsyncResult<Result.GetTransactionResultResult> result = null;
			await GetTransactionResult(
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
		public GetTransactionResultTask GetTransactionResultAsync(
                Request.GetTransactionResultRequest request
        )
		{
			return new GetTransactionResultTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.GetTransactionResultResult> GetTransactionResultAsync(
                Request.GetTransactionResultRequest request
        )
		{
			var task = new GetTransactionResultTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class GetTransactionResultByUserIdTask : Gs2RestSessionTask<GetTransactionResultByUserIdRequest, GetTransactionResultByUserIdResult>
        {
            public GetTransactionResultByUserIdTask(IGs2Session session, RestSessionRequestFactory factory, GetTransactionResultByUserIdRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(GetTransactionResultByUserIdRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "distributor")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/{userId}/transaction/{transactionId}/result";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{userId}", !string.IsNullOrEmpty(request.UserId) ? request.UserId.ToString() : "null");
                url = url.Replace("{transactionId}", !string.IsNullOrEmpty(request.TransactionId) ? request.TransactionId.ToString() : "null");

                var sessionRequest = Factory.Get(url);
                if (request.ContextStack != null)
                {
                    sessionRequest.AddQueryString("contextStack", request.ContextStack);
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
		public IEnumerator GetTransactionResultByUserId(
                Request.GetTransactionResultByUserIdRequest request,
                UnityAction<AsyncResult<Result.GetTransactionResultByUserIdResult>> callback
        )
		{
			var task = new GetTransactionResultByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.GetTransactionResultByUserIdResult>(task.Result, task.Error));
        }

		public IFuture<Result.GetTransactionResultByUserIdResult> GetTransactionResultByUserIdFuture(
                Request.GetTransactionResultByUserIdRequest request
        )
		{
			return new GetTransactionResultByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.GetTransactionResultByUserIdResult> GetTransactionResultByUserIdAsync(
                Request.GetTransactionResultByUserIdRequest request
        )
		{
            AsyncResult<Result.GetTransactionResultByUserIdResult> result = null;
			await GetTransactionResultByUserId(
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
		public GetTransactionResultByUserIdTask GetTransactionResultByUserIdAsync(
                Request.GetTransactionResultByUserIdRequest request
        )
		{
			return new GetTransactionResultByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.GetTransactionResultByUserIdResult> GetTransactionResultByUserIdAsync(
                Request.GetTransactionResultByUserIdRequest request
        )
		{
			var task = new GetTransactionResultByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif
	}
}