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
                if (request.SimpleItemAcquireScript != null)
                {
                    jsonWriter.WritePropertyName("simpleItemAcquireScript");
                    request.SimpleItemAcquireScript.WriteJson(jsonWriter);
                }
                if (request.SimpleItemConsumeScript != null)
                {
                    jsonWriter.WritePropertyName("simpleItemConsumeScript");
                    request.SimpleItemConsumeScript.WriteJson(jsonWriter);
                }
                if (request.BigItemAcquireScript != null)
                {
                    jsonWriter.WritePropertyName("bigItemAcquireScript");
                    request.BigItemAcquireScript.WriteJson(jsonWriter);
                }
                if (request.BigItemConsumeScript != null)
                {
                    jsonWriter.WritePropertyName("bigItemConsumeScript");
                    request.BigItemConsumeScript.WriteJson(jsonWriter);
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
                    .Replace("{service}", "inventory")
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
                    .Replace("{service}", "inventory")
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
                if (request.SimpleItemAcquireScript != null)
                {
                    jsonWriter.WritePropertyName("simpleItemAcquireScript");
                    request.SimpleItemAcquireScript.WriteJson(jsonWriter);
                }
                if (request.SimpleItemConsumeScript != null)
                {
                    jsonWriter.WritePropertyName("simpleItemConsumeScript");
                    request.SimpleItemConsumeScript.WriteJson(jsonWriter);
                }
                if (request.BigItemAcquireScript != null)
                {
                    jsonWriter.WritePropertyName("bigItemAcquireScript");
                    request.BigItemAcquireScript.WriteJson(jsonWriter);
                }
                if (request.BigItemConsumeScript != null)
                {
                    jsonWriter.WritePropertyName("bigItemConsumeScript");
                    request.BigItemConsumeScript.WriteJson(jsonWriter);
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
                    .Replace("{service}", "inventory")
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


        public class DumpUserDataByUserIdTask : Gs2RestSessionTask<DumpUserDataByUserIdRequest, DumpUserDataByUserIdResult>
        {
            public DumpUserDataByUserIdTask(IGs2Session session, RestSessionRequestFactory factory, DumpUserDataByUserIdRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DumpUserDataByUserIdRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "inventory")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/system/dump/user/{userId}";

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
		public IEnumerator DumpUserDataByUserId(
                Request.DumpUserDataByUserIdRequest request,
                UnityAction<AsyncResult<Result.DumpUserDataByUserIdResult>> callback
        )
		{
			var task = new DumpUserDataByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.DumpUserDataByUserIdResult>(task.Result, task.Error));
        }

		public IFuture<Result.DumpUserDataByUserIdResult> DumpUserDataByUserIdFuture(
                Request.DumpUserDataByUserIdRequest request
        )
		{
			return new DumpUserDataByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.DumpUserDataByUserIdResult> DumpUserDataByUserIdAsync(
                Request.DumpUserDataByUserIdRequest request
        )
		{
            AsyncResult<Result.DumpUserDataByUserIdResult> result = null;
			await DumpUserDataByUserId(
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
		public DumpUserDataByUserIdTask DumpUserDataByUserIdAsync(
                Request.DumpUserDataByUserIdRequest request
        )
		{
			return new DumpUserDataByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.DumpUserDataByUserIdResult> DumpUserDataByUserIdAsync(
                Request.DumpUserDataByUserIdRequest request
        )
		{
			var task = new DumpUserDataByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class CheckDumpUserDataByUserIdTask : Gs2RestSessionTask<CheckDumpUserDataByUserIdRequest, CheckDumpUserDataByUserIdResult>
        {
            public CheckDumpUserDataByUserIdTask(IGs2Session session, RestSessionRequestFactory factory, CheckDumpUserDataByUserIdRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(CheckDumpUserDataByUserIdRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "inventory")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/system/dump/user/{userId}";

                url = url.Replace("{userId}", !string.IsNullOrEmpty(request.UserId) ? request.UserId.ToString() : "null");

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
		public IEnumerator CheckDumpUserDataByUserId(
                Request.CheckDumpUserDataByUserIdRequest request,
                UnityAction<AsyncResult<Result.CheckDumpUserDataByUserIdResult>> callback
        )
		{
			var task = new CheckDumpUserDataByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.CheckDumpUserDataByUserIdResult>(task.Result, task.Error));
        }

		public IFuture<Result.CheckDumpUserDataByUserIdResult> CheckDumpUserDataByUserIdFuture(
                Request.CheckDumpUserDataByUserIdRequest request
        )
		{
			return new CheckDumpUserDataByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.CheckDumpUserDataByUserIdResult> CheckDumpUserDataByUserIdAsync(
                Request.CheckDumpUserDataByUserIdRequest request
        )
		{
            AsyncResult<Result.CheckDumpUserDataByUserIdResult> result = null;
			await CheckDumpUserDataByUserId(
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
		public CheckDumpUserDataByUserIdTask CheckDumpUserDataByUserIdAsync(
                Request.CheckDumpUserDataByUserIdRequest request
        )
		{
			return new CheckDumpUserDataByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.CheckDumpUserDataByUserIdResult> CheckDumpUserDataByUserIdAsync(
                Request.CheckDumpUserDataByUserIdRequest request
        )
		{
			var task = new CheckDumpUserDataByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class CleanUserDataByUserIdTask : Gs2RestSessionTask<CleanUserDataByUserIdRequest, CleanUserDataByUserIdResult>
        {
            public CleanUserDataByUserIdTask(IGs2Session session, RestSessionRequestFactory factory, CleanUserDataByUserIdRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(CleanUserDataByUserIdRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "inventory")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/system/clean/user/{userId}";

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
		public IEnumerator CleanUserDataByUserId(
                Request.CleanUserDataByUserIdRequest request,
                UnityAction<AsyncResult<Result.CleanUserDataByUserIdResult>> callback
        )
		{
			var task = new CleanUserDataByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.CleanUserDataByUserIdResult>(task.Result, task.Error));
        }

		public IFuture<Result.CleanUserDataByUserIdResult> CleanUserDataByUserIdFuture(
                Request.CleanUserDataByUserIdRequest request
        )
		{
			return new CleanUserDataByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.CleanUserDataByUserIdResult> CleanUserDataByUserIdAsync(
                Request.CleanUserDataByUserIdRequest request
        )
		{
            AsyncResult<Result.CleanUserDataByUserIdResult> result = null;
			await CleanUserDataByUserId(
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
		public CleanUserDataByUserIdTask CleanUserDataByUserIdAsync(
                Request.CleanUserDataByUserIdRequest request
        )
		{
			return new CleanUserDataByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.CleanUserDataByUserIdResult> CleanUserDataByUserIdAsync(
                Request.CleanUserDataByUserIdRequest request
        )
		{
			var task = new CleanUserDataByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class CheckCleanUserDataByUserIdTask : Gs2RestSessionTask<CheckCleanUserDataByUserIdRequest, CheckCleanUserDataByUserIdResult>
        {
            public CheckCleanUserDataByUserIdTask(IGs2Session session, RestSessionRequestFactory factory, CheckCleanUserDataByUserIdRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(CheckCleanUserDataByUserIdRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "inventory")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/system/clean/user/{userId}";

                url = url.Replace("{userId}", !string.IsNullOrEmpty(request.UserId) ? request.UserId.ToString() : "null");

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
		public IEnumerator CheckCleanUserDataByUserId(
                Request.CheckCleanUserDataByUserIdRequest request,
                UnityAction<AsyncResult<Result.CheckCleanUserDataByUserIdResult>> callback
        )
		{
			var task = new CheckCleanUserDataByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.CheckCleanUserDataByUserIdResult>(task.Result, task.Error));
        }

		public IFuture<Result.CheckCleanUserDataByUserIdResult> CheckCleanUserDataByUserIdFuture(
                Request.CheckCleanUserDataByUserIdRequest request
        )
		{
			return new CheckCleanUserDataByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.CheckCleanUserDataByUserIdResult> CheckCleanUserDataByUserIdAsync(
                Request.CheckCleanUserDataByUserIdRequest request
        )
		{
            AsyncResult<Result.CheckCleanUserDataByUserIdResult> result = null;
			await CheckCleanUserDataByUserId(
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
		public CheckCleanUserDataByUserIdTask CheckCleanUserDataByUserIdAsync(
                Request.CheckCleanUserDataByUserIdRequest request
        )
		{
			return new CheckCleanUserDataByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.CheckCleanUserDataByUserIdResult> CheckCleanUserDataByUserIdAsync(
                Request.CheckCleanUserDataByUserIdRequest request
        )
		{
			var task = new CheckCleanUserDataByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class PrepareImportUserDataByUserIdTask : Gs2RestSessionTask<PrepareImportUserDataByUserIdRequest, PrepareImportUserDataByUserIdResult>
        {
            public PrepareImportUserDataByUserIdTask(IGs2Session session, RestSessionRequestFactory factory, PrepareImportUserDataByUserIdRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(PrepareImportUserDataByUserIdRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "inventory")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/system/import/user/{userId}/prepare";

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
		public IEnumerator PrepareImportUserDataByUserId(
                Request.PrepareImportUserDataByUserIdRequest request,
                UnityAction<AsyncResult<Result.PrepareImportUserDataByUserIdResult>> callback
        )
		{
			var task = new PrepareImportUserDataByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.PrepareImportUserDataByUserIdResult>(task.Result, task.Error));
        }

		public IFuture<Result.PrepareImportUserDataByUserIdResult> PrepareImportUserDataByUserIdFuture(
                Request.PrepareImportUserDataByUserIdRequest request
        )
		{
			return new PrepareImportUserDataByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.PrepareImportUserDataByUserIdResult> PrepareImportUserDataByUserIdAsync(
                Request.PrepareImportUserDataByUserIdRequest request
        )
		{
            AsyncResult<Result.PrepareImportUserDataByUserIdResult> result = null;
			await PrepareImportUserDataByUserId(
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
		public PrepareImportUserDataByUserIdTask PrepareImportUserDataByUserIdAsync(
                Request.PrepareImportUserDataByUserIdRequest request
        )
		{
			return new PrepareImportUserDataByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.PrepareImportUserDataByUserIdResult> PrepareImportUserDataByUserIdAsync(
                Request.PrepareImportUserDataByUserIdRequest request
        )
		{
			var task = new PrepareImportUserDataByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class ImportUserDataByUserIdTask : Gs2RestSessionTask<ImportUserDataByUserIdRequest, ImportUserDataByUserIdResult>
        {
            public ImportUserDataByUserIdTask(IGs2Session session, RestSessionRequestFactory factory, ImportUserDataByUserIdRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(ImportUserDataByUserIdRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "inventory")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/system/import/user/{userId}";

                url = url.Replace("{userId}", !string.IsNullOrEmpty(request.UserId) ? request.UserId.ToString() : "null");

                var sessionRequest = Factory.Post(url);

                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);
                jsonWriter.WriteObjectStart();
                if (request.UploadToken != null)
                {
                    jsonWriter.WritePropertyName("uploadToken");
                    jsonWriter.Write(request.UploadToken);
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
		public IEnumerator ImportUserDataByUserId(
                Request.ImportUserDataByUserIdRequest request,
                UnityAction<AsyncResult<Result.ImportUserDataByUserIdResult>> callback
        )
		{
			var task = new ImportUserDataByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.ImportUserDataByUserIdResult>(task.Result, task.Error));
        }

		public IFuture<Result.ImportUserDataByUserIdResult> ImportUserDataByUserIdFuture(
                Request.ImportUserDataByUserIdRequest request
        )
		{
			return new ImportUserDataByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.ImportUserDataByUserIdResult> ImportUserDataByUserIdAsync(
                Request.ImportUserDataByUserIdRequest request
        )
		{
            AsyncResult<Result.ImportUserDataByUserIdResult> result = null;
			await ImportUserDataByUserId(
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
		public ImportUserDataByUserIdTask ImportUserDataByUserIdAsync(
                Request.ImportUserDataByUserIdRequest request
        )
		{
			return new ImportUserDataByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.ImportUserDataByUserIdResult> ImportUserDataByUserIdAsync(
                Request.ImportUserDataByUserIdRequest request
        )
		{
			var task = new ImportUserDataByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class CheckImportUserDataByUserIdTask : Gs2RestSessionTask<CheckImportUserDataByUserIdRequest, CheckImportUserDataByUserIdResult>
        {
            public CheckImportUserDataByUserIdTask(IGs2Session session, RestSessionRequestFactory factory, CheckImportUserDataByUserIdRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(CheckImportUserDataByUserIdRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "inventory")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/system/import/user/{userId}/{uploadToken}";

                url = url.Replace("{userId}", !string.IsNullOrEmpty(request.UserId) ? request.UserId.ToString() : "null");
                url = url.Replace("{uploadToken}", !string.IsNullOrEmpty(request.UploadToken) ? request.UploadToken.ToString() : "null");

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
		public IEnumerator CheckImportUserDataByUserId(
                Request.CheckImportUserDataByUserIdRequest request,
                UnityAction<AsyncResult<Result.CheckImportUserDataByUserIdResult>> callback
        )
		{
			var task = new CheckImportUserDataByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.CheckImportUserDataByUserIdResult>(task.Result, task.Error));
        }

		public IFuture<Result.CheckImportUserDataByUserIdResult> CheckImportUserDataByUserIdFuture(
                Request.CheckImportUserDataByUserIdRequest request
        )
		{
			return new CheckImportUserDataByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.CheckImportUserDataByUserIdResult> CheckImportUserDataByUserIdAsync(
                Request.CheckImportUserDataByUserIdRequest request
        )
		{
            AsyncResult<Result.CheckImportUserDataByUserIdResult> result = null;
			await CheckImportUserDataByUserId(
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
		public CheckImportUserDataByUserIdTask CheckImportUserDataByUserIdAsync(
                Request.CheckImportUserDataByUserIdRequest request
        )
		{
			return new CheckImportUserDataByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.CheckImportUserDataByUserIdResult> CheckImportUserDataByUserIdAsync(
                Request.CheckImportUserDataByUserIdRequest request
        )
		{
			var task = new CheckImportUserDataByUserIdTask(
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


        public class DescribeSimpleInventoryModelMastersTask : Gs2RestSessionTask<DescribeSimpleInventoryModelMastersRequest, DescribeSimpleInventoryModelMastersResult>
        {
            public DescribeSimpleInventoryModelMastersTask(IGs2Session session, RestSessionRequestFactory factory, DescribeSimpleInventoryModelMastersRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DescribeSimpleInventoryModelMastersRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "inventory")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/master/simple/inventory";

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
		public IEnumerator DescribeSimpleInventoryModelMasters(
                Request.DescribeSimpleInventoryModelMastersRequest request,
                UnityAction<AsyncResult<Result.DescribeSimpleInventoryModelMastersResult>> callback
        )
		{
			var task = new DescribeSimpleInventoryModelMastersTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.DescribeSimpleInventoryModelMastersResult>(task.Result, task.Error));
        }

		public IFuture<Result.DescribeSimpleInventoryModelMastersResult> DescribeSimpleInventoryModelMastersFuture(
                Request.DescribeSimpleInventoryModelMastersRequest request
        )
		{
			return new DescribeSimpleInventoryModelMastersTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.DescribeSimpleInventoryModelMastersResult> DescribeSimpleInventoryModelMastersAsync(
                Request.DescribeSimpleInventoryModelMastersRequest request
        )
		{
            AsyncResult<Result.DescribeSimpleInventoryModelMastersResult> result = null;
			await DescribeSimpleInventoryModelMasters(
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
		public DescribeSimpleInventoryModelMastersTask DescribeSimpleInventoryModelMastersAsync(
                Request.DescribeSimpleInventoryModelMastersRequest request
        )
		{
			return new DescribeSimpleInventoryModelMastersTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.DescribeSimpleInventoryModelMastersResult> DescribeSimpleInventoryModelMastersAsync(
                Request.DescribeSimpleInventoryModelMastersRequest request
        )
		{
			var task = new DescribeSimpleInventoryModelMastersTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class CreateSimpleInventoryModelMasterTask : Gs2RestSessionTask<CreateSimpleInventoryModelMasterRequest, CreateSimpleInventoryModelMasterResult>
        {
            public CreateSimpleInventoryModelMasterTask(IGs2Session session, RestSessionRequestFactory factory, CreateSimpleInventoryModelMasterRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(CreateSimpleInventoryModelMasterRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "inventory")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/master/simple/inventory";

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
		public IEnumerator CreateSimpleInventoryModelMaster(
                Request.CreateSimpleInventoryModelMasterRequest request,
                UnityAction<AsyncResult<Result.CreateSimpleInventoryModelMasterResult>> callback
        )
		{
			var task = new CreateSimpleInventoryModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.CreateSimpleInventoryModelMasterResult>(task.Result, task.Error));
        }

		public IFuture<Result.CreateSimpleInventoryModelMasterResult> CreateSimpleInventoryModelMasterFuture(
                Request.CreateSimpleInventoryModelMasterRequest request
        )
		{
			return new CreateSimpleInventoryModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.CreateSimpleInventoryModelMasterResult> CreateSimpleInventoryModelMasterAsync(
                Request.CreateSimpleInventoryModelMasterRequest request
        )
		{
            AsyncResult<Result.CreateSimpleInventoryModelMasterResult> result = null;
			await CreateSimpleInventoryModelMaster(
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
		public CreateSimpleInventoryModelMasterTask CreateSimpleInventoryModelMasterAsync(
                Request.CreateSimpleInventoryModelMasterRequest request
        )
		{
			return new CreateSimpleInventoryModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.CreateSimpleInventoryModelMasterResult> CreateSimpleInventoryModelMasterAsync(
                Request.CreateSimpleInventoryModelMasterRequest request
        )
		{
			var task = new CreateSimpleInventoryModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class GetSimpleInventoryModelMasterTask : Gs2RestSessionTask<GetSimpleInventoryModelMasterRequest, GetSimpleInventoryModelMasterResult>
        {
            public GetSimpleInventoryModelMasterTask(IGs2Session session, RestSessionRequestFactory factory, GetSimpleInventoryModelMasterRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(GetSimpleInventoryModelMasterRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "inventory")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/master/simple/inventory/{inventoryName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{inventoryName}", !string.IsNullOrEmpty(request.InventoryName) ? request.InventoryName.ToString() : "null");

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
		public IEnumerator GetSimpleInventoryModelMaster(
                Request.GetSimpleInventoryModelMasterRequest request,
                UnityAction<AsyncResult<Result.GetSimpleInventoryModelMasterResult>> callback
        )
		{
			var task = new GetSimpleInventoryModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.GetSimpleInventoryModelMasterResult>(task.Result, task.Error));
        }

		public IFuture<Result.GetSimpleInventoryModelMasterResult> GetSimpleInventoryModelMasterFuture(
                Request.GetSimpleInventoryModelMasterRequest request
        )
		{
			return new GetSimpleInventoryModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.GetSimpleInventoryModelMasterResult> GetSimpleInventoryModelMasterAsync(
                Request.GetSimpleInventoryModelMasterRequest request
        )
		{
            AsyncResult<Result.GetSimpleInventoryModelMasterResult> result = null;
			await GetSimpleInventoryModelMaster(
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
		public GetSimpleInventoryModelMasterTask GetSimpleInventoryModelMasterAsync(
                Request.GetSimpleInventoryModelMasterRequest request
        )
		{
			return new GetSimpleInventoryModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.GetSimpleInventoryModelMasterResult> GetSimpleInventoryModelMasterAsync(
                Request.GetSimpleInventoryModelMasterRequest request
        )
		{
			var task = new GetSimpleInventoryModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class UpdateSimpleInventoryModelMasterTask : Gs2RestSessionTask<UpdateSimpleInventoryModelMasterRequest, UpdateSimpleInventoryModelMasterResult>
        {
            public UpdateSimpleInventoryModelMasterTask(IGs2Session session, RestSessionRequestFactory factory, UpdateSimpleInventoryModelMasterRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(UpdateSimpleInventoryModelMasterRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "inventory")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/master/simple/inventory/{inventoryName}";

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
		public IEnumerator UpdateSimpleInventoryModelMaster(
                Request.UpdateSimpleInventoryModelMasterRequest request,
                UnityAction<AsyncResult<Result.UpdateSimpleInventoryModelMasterResult>> callback
        )
		{
			var task = new UpdateSimpleInventoryModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.UpdateSimpleInventoryModelMasterResult>(task.Result, task.Error));
        }

		public IFuture<Result.UpdateSimpleInventoryModelMasterResult> UpdateSimpleInventoryModelMasterFuture(
                Request.UpdateSimpleInventoryModelMasterRequest request
        )
		{
			return new UpdateSimpleInventoryModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.UpdateSimpleInventoryModelMasterResult> UpdateSimpleInventoryModelMasterAsync(
                Request.UpdateSimpleInventoryModelMasterRequest request
        )
		{
            AsyncResult<Result.UpdateSimpleInventoryModelMasterResult> result = null;
			await UpdateSimpleInventoryModelMaster(
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
		public UpdateSimpleInventoryModelMasterTask UpdateSimpleInventoryModelMasterAsync(
                Request.UpdateSimpleInventoryModelMasterRequest request
        )
		{
			return new UpdateSimpleInventoryModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.UpdateSimpleInventoryModelMasterResult> UpdateSimpleInventoryModelMasterAsync(
                Request.UpdateSimpleInventoryModelMasterRequest request
        )
		{
			var task = new UpdateSimpleInventoryModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class DeleteSimpleInventoryModelMasterTask : Gs2RestSessionTask<DeleteSimpleInventoryModelMasterRequest, DeleteSimpleInventoryModelMasterResult>
        {
            public DeleteSimpleInventoryModelMasterTask(IGs2Session session, RestSessionRequestFactory factory, DeleteSimpleInventoryModelMasterRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DeleteSimpleInventoryModelMasterRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "inventory")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/master/simple/inventory/{inventoryName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{inventoryName}", !string.IsNullOrEmpty(request.InventoryName) ? request.InventoryName.ToString() : "null");

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
		public IEnumerator DeleteSimpleInventoryModelMaster(
                Request.DeleteSimpleInventoryModelMasterRequest request,
                UnityAction<AsyncResult<Result.DeleteSimpleInventoryModelMasterResult>> callback
        )
		{
			var task = new DeleteSimpleInventoryModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.DeleteSimpleInventoryModelMasterResult>(task.Result, task.Error));
        }

		public IFuture<Result.DeleteSimpleInventoryModelMasterResult> DeleteSimpleInventoryModelMasterFuture(
                Request.DeleteSimpleInventoryModelMasterRequest request
        )
		{
			return new DeleteSimpleInventoryModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.DeleteSimpleInventoryModelMasterResult> DeleteSimpleInventoryModelMasterAsync(
                Request.DeleteSimpleInventoryModelMasterRequest request
        )
		{
            AsyncResult<Result.DeleteSimpleInventoryModelMasterResult> result = null;
			await DeleteSimpleInventoryModelMaster(
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
		public DeleteSimpleInventoryModelMasterTask DeleteSimpleInventoryModelMasterAsync(
                Request.DeleteSimpleInventoryModelMasterRequest request
        )
		{
			return new DeleteSimpleInventoryModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.DeleteSimpleInventoryModelMasterResult> DeleteSimpleInventoryModelMasterAsync(
                Request.DeleteSimpleInventoryModelMasterRequest request
        )
		{
			var task = new DeleteSimpleInventoryModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class DescribeSimpleInventoryModelsTask : Gs2RestSessionTask<DescribeSimpleInventoryModelsRequest, DescribeSimpleInventoryModelsResult>
        {
            public DescribeSimpleInventoryModelsTask(IGs2Session session, RestSessionRequestFactory factory, DescribeSimpleInventoryModelsRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DescribeSimpleInventoryModelsRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "inventory")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/simple/inventory";

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
		public IEnumerator DescribeSimpleInventoryModels(
                Request.DescribeSimpleInventoryModelsRequest request,
                UnityAction<AsyncResult<Result.DescribeSimpleInventoryModelsResult>> callback
        )
		{
			var task = new DescribeSimpleInventoryModelsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.DescribeSimpleInventoryModelsResult>(task.Result, task.Error));
        }

		public IFuture<Result.DescribeSimpleInventoryModelsResult> DescribeSimpleInventoryModelsFuture(
                Request.DescribeSimpleInventoryModelsRequest request
        )
		{
			return new DescribeSimpleInventoryModelsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.DescribeSimpleInventoryModelsResult> DescribeSimpleInventoryModelsAsync(
                Request.DescribeSimpleInventoryModelsRequest request
        )
		{
            AsyncResult<Result.DescribeSimpleInventoryModelsResult> result = null;
			await DescribeSimpleInventoryModels(
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
		public DescribeSimpleInventoryModelsTask DescribeSimpleInventoryModelsAsync(
                Request.DescribeSimpleInventoryModelsRequest request
        )
		{
			return new DescribeSimpleInventoryModelsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.DescribeSimpleInventoryModelsResult> DescribeSimpleInventoryModelsAsync(
                Request.DescribeSimpleInventoryModelsRequest request
        )
		{
			var task = new DescribeSimpleInventoryModelsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class GetSimpleInventoryModelTask : Gs2RestSessionTask<GetSimpleInventoryModelRequest, GetSimpleInventoryModelResult>
        {
            public GetSimpleInventoryModelTask(IGs2Session session, RestSessionRequestFactory factory, GetSimpleInventoryModelRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(GetSimpleInventoryModelRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "inventory")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/simple/inventory/{inventoryName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{inventoryName}", !string.IsNullOrEmpty(request.InventoryName) ? request.InventoryName.ToString() : "null");

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
		public IEnumerator GetSimpleInventoryModel(
                Request.GetSimpleInventoryModelRequest request,
                UnityAction<AsyncResult<Result.GetSimpleInventoryModelResult>> callback
        )
		{
			var task = new GetSimpleInventoryModelTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.GetSimpleInventoryModelResult>(task.Result, task.Error));
        }

		public IFuture<Result.GetSimpleInventoryModelResult> GetSimpleInventoryModelFuture(
                Request.GetSimpleInventoryModelRequest request
        )
		{
			return new GetSimpleInventoryModelTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.GetSimpleInventoryModelResult> GetSimpleInventoryModelAsync(
                Request.GetSimpleInventoryModelRequest request
        )
		{
            AsyncResult<Result.GetSimpleInventoryModelResult> result = null;
			await GetSimpleInventoryModel(
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
		public GetSimpleInventoryModelTask GetSimpleInventoryModelAsync(
                Request.GetSimpleInventoryModelRequest request
        )
		{
			return new GetSimpleInventoryModelTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.GetSimpleInventoryModelResult> GetSimpleInventoryModelAsync(
                Request.GetSimpleInventoryModelRequest request
        )
		{
			var task = new GetSimpleInventoryModelTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class DescribeSimpleItemModelMastersTask : Gs2RestSessionTask<DescribeSimpleItemModelMastersRequest, DescribeSimpleItemModelMastersResult>
        {
            public DescribeSimpleItemModelMastersTask(IGs2Session session, RestSessionRequestFactory factory, DescribeSimpleItemModelMastersRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DescribeSimpleItemModelMastersRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "inventory")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/master/simple/inventory/{inventoryName}/item";

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
		public IEnumerator DescribeSimpleItemModelMasters(
                Request.DescribeSimpleItemModelMastersRequest request,
                UnityAction<AsyncResult<Result.DescribeSimpleItemModelMastersResult>> callback
        )
		{
			var task = new DescribeSimpleItemModelMastersTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.DescribeSimpleItemModelMastersResult>(task.Result, task.Error));
        }

		public IFuture<Result.DescribeSimpleItemModelMastersResult> DescribeSimpleItemModelMastersFuture(
                Request.DescribeSimpleItemModelMastersRequest request
        )
		{
			return new DescribeSimpleItemModelMastersTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.DescribeSimpleItemModelMastersResult> DescribeSimpleItemModelMastersAsync(
                Request.DescribeSimpleItemModelMastersRequest request
        )
		{
            AsyncResult<Result.DescribeSimpleItemModelMastersResult> result = null;
			await DescribeSimpleItemModelMasters(
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
		public DescribeSimpleItemModelMastersTask DescribeSimpleItemModelMastersAsync(
                Request.DescribeSimpleItemModelMastersRequest request
        )
		{
			return new DescribeSimpleItemModelMastersTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.DescribeSimpleItemModelMastersResult> DescribeSimpleItemModelMastersAsync(
                Request.DescribeSimpleItemModelMastersRequest request
        )
		{
			var task = new DescribeSimpleItemModelMastersTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class CreateSimpleItemModelMasterTask : Gs2RestSessionTask<CreateSimpleItemModelMasterRequest, CreateSimpleItemModelMasterResult>
        {
            public CreateSimpleItemModelMasterTask(IGs2Session session, RestSessionRequestFactory factory, CreateSimpleItemModelMasterRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(CreateSimpleItemModelMasterRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "inventory")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/master/simple/inventory/{inventoryName}/item";

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
		public IEnumerator CreateSimpleItemModelMaster(
                Request.CreateSimpleItemModelMasterRequest request,
                UnityAction<AsyncResult<Result.CreateSimpleItemModelMasterResult>> callback
        )
		{
			var task = new CreateSimpleItemModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.CreateSimpleItemModelMasterResult>(task.Result, task.Error));
        }

		public IFuture<Result.CreateSimpleItemModelMasterResult> CreateSimpleItemModelMasterFuture(
                Request.CreateSimpleItemModelMasterRequest request
        )
		{
			return new CreateSimpleItemModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.CreateSimpleItemModelMasterResult> CreateSimpleItemModelMasterAsync(
                Request.CreateSimpleItemModelMasterRequest request
        )
		{
            AsyncResult<Result.CreateSimpleItemModelMasterResult> result = null;
			await CreateSimpleItemModelMaster(
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
		public CreateSimpleItemModelMasterTask CreateSimpleItemModelMasterAsync(
                Request.CreateSimpleItemModelMasterRequest request
        )
		{
			return new CreateSimpleItemModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.CreateSimpleItemModelMasterResult> CreateSimpleItemModelMasterAsync(
                Request.CreateSimpleItemModelMasterRequest request
        )
		{
			var task = new CreateSimpleItemModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class GetSimpleItemModelMasterTask : Gs2RestSessionTask<GetSimpleItemModelMasterRequest, GetSimpleItemModelMasterResult>
        {
            public GetSimpleItemModelMasterTask(IGs2Session session, RestSessionRequestFactory factory, GetSimpleItemModelMasterRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(GetSimpleItemModelMasterRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "inventory")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/master/simple/inventory/{inventoryName}/item/{itemName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{inventoryName}", !string.IsNullOrEmpty(request.InventoryName) ? request.InventoryName.ToString() : "null");
                url = url.Replace("{itemName}", !string.IsNullOrEmpty(request.ItemName) ? request.ItemName.ToString() : "null");

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
		public IEnumerator GetSimpleItemModelMaster(
                Request.GetSimpleItemModelMasterRequest request,
                UnityAction<AsyncResult<Result.GetSimpleItemModelMasterResult>> callback
        )
		{
			var task = new GetSimpleItemModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.GetSimpleItemModelMasterResult>(task.Result, task.Error));
        }

		public IFuture<Result.GetSimpleItemModelMasterResult> GetSimpleItemModelMasterFuture(
                Request.GetSimpleItemModelMasterRequest request
        )
		{
			return new GetSimpleItemModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.GetSimpleItemModelMasterResult> GetSimpleItemModelMasterAsync(
                Request.GetSimpleItemModelMasterRequest request
        )
		{
            AsyncResult<Result.GetSimpleItemModelMasterResult> result = null;
			await GetSimpleItemModelMaster(
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
		public GetSimpleItemModelMasterTask GetSimpleItemModelMasterAsync(
                Request.GetSimpleItemModelMasterRequest request
        )
		{
			return new GetSimpleItemModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.GetSimpleItemModelMasterResult> GetSimpleItemModelMasterAsync(
                Request.GetSimpleItemModelMasterRequest request
        )
		{
			var task = new GetSimpleItemModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class UpdateSimpleItemModelMasterTask : Gs2RestSessionTask<UpdateSimpleItemModelMasterRequest, UpdateSimpleItemModelMasterResult>
        {
            public UpdateSimpleItemModelMasterTask(IGs2Session session, RestSessionRequestFactory factory, UpdateSimpleItemModelMasterRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(UpdateSimpleItemModelMasterRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "inventory")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/master/simple/inventory/{inventoryName}/item/{itemName}";

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
		public IEnumerator UpdateSimpleItemModelMaster(
                Request.UpdateSimpleItemModelMasterRequest request,
                UnityAction<AsyncResult<Result.UpdateSimpleItemModelMasterResult>> callback
        )
		{
			var task = new UpdateSimpleItemModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.UpdateSimpleItemModelMasterResult>(task.Result, task.Error));
        }

		public IFuture<Result.UpdateSimpleItemModelMasterResult> UpdateSimpleItemModelMasterFuture(
                Request.UpdateSimpleItemModelMasterRequest request
        )
		{
			return new UpdateSimpleItemModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.UpdateSimpleItemModelMasterResult> UpdateSimpleItemModelMasterAsync(
                Request.UpdateSimpleItemModelMasterRequest request
        )
		{
            AsyncResult<Result.UpdateSimpleItemModelMasterResult> result = null;
			await UpdateSimpleItemModelMaster(
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
		public UpdateSimpleItemModelMasterTask UpdateSimpleItemModelMasterAsync(
                Request.UpdateSimpleItemModelMasterRequest request
        )
		{
			return new UpdateSimpleItemModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.UpdateSimpleItemModelMasterResult> UpdateSimpleItemModelMasterAsync(
                Request.UpdateSimpleItemModelMasterRequest request
        )
		{
			var task = new UpdateSimpleItemModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class DeleteSimpleItemModelMasterTask : Gs2RestSessionTask<DeleteSimpleItemModelMasterRequest, DeleteSimpleItemModelMasterResult>
        {
            public DeleteSimpleItemModelMasterTask(IGs2Session session, RestSessionRequestFactory factory, DeleteSimpleItemModelMasterRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DeleteSimpleItemModelMasterRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "inventory")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/master/simple/inventory/{inventoryName}/item/{itemName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{inventoryName}", !string.IsNullOrEmpty(request.InventoryName) ? request.InventoryName.ToString() : "null");
                url = url.Replace("{itemName}", !string.IsNullOrEmpty(request.ItemName) ? request.ItemName.ToString() : "null");

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
		public IEnumerator DeleteSimpleItemModelMaster(
                Request.DeleteSimpleItemModelMasterRequest request,
                UnityAction<AsyncResult<Result.DeleteSimpleItemModelMasterResult>> callback
        )
		{
			var task = new DeleteSimpleItemModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.DeleteSimpleItemModelMasterResult>(task.Result, task.Error));
        }

		public IFuture<Result.DeleteSimpleItemModelMasterResult> DeleteSimpleItemModelMasterFuture(
                Request.DeleteSimpleItemModelMasterRequest request
        )
		{
			return new DeleteSimpleItemModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.DeleteSimpleItemModelMasterResult> DeleteSimpleItemModelMasterAsync(
                Request.DeleteSimpleItemModelMasterRequest request
        )
		{
            AsyncResult<Result.DeleteSimpleItemModelMasterResult> result = null;
			await DeleteSimpleItemModelMaster(
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
		public DeleteSimpleItemModelMasterTask DeleteSimpleItemModelMasterAsync(
                Request.DeleteSimpleItemModelMasterRequest request
        )
		{
			return new DeleteSimpleItemModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.DeleteSimpleItemModelMasterResult> DeleteSimpleItemModelMasterAsync(
                Request.DeleteSimpleItemModelMasterRequest request
        )
		{
			var task = new DeleteSimpleItemModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class DescribeSimpleItemModelsTask : Gs2RestSessionTask<DescribeSimpleItemModelsRequest, DescribeSimpleItemModelsResult>
        {
            public DescribeSimpleItemModelsTask(IGs2Session session, RestSessionRequestFactory factory, DescribeSimpleItemModelsRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DescribeSimpleItemModelsRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "inventory")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/simple/inventory/{inventoryName}/item";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{inventoryName}", !string.IsNullOrEmpty(request.InventoryName) ? request.InventoryName.ToString() : "null");

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
		public IEnumerator DescribeSimpleItemModels(
                Request.DescribeSimpleItemModelsRequest request,
                UnityAction<AsyncResult<Result.DescribeSimpleItemModelsResult>> callback
        )
		{
			var task = new DescribeSimpleItemModelsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.DescribeSimpleItemModelsResult>(task.Result, task.Error));
        }

		public IFuture<Result.DescribeSimpleItemModelsResult> DescribeSimpleItemModelsFuture(
                Request.DescribeSimpleItemModelsRequest request
        )
		{
			return new DescribeSimpleItemModelsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.DescribeSimpleItemModelsResult> DescribeSimpleItemModelsAsync(
                Request.DescribeSimpleItemModelsRequest request
        )
		{
            AsyncResult<Result.DescribeSimpleItemModelsResult> result = null;
			await DescribeSimpleItemModels(
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
		public DescribeSimpleItemModelsTask DescribeSimpleItemModelsAsync(
                Request.DescribeSimpleItemModelsRequest request
        )
		{
			return new DescribeSimpleItemModelsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.DescribeSimpleItemModelsResult> DescribeSimpleItemModelsAsync(
                Request.DescribeSimpleItemModelsRequest request
        )
		{
			var task = new DescribeSimpleItemModelsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class GetSimpleItemModelTask : Gs2RestSessionTask<GetSimpleItemModelRequest, GetSimpleItemModelResult>
        {
            public GetSimpleItemModelTask(IGs2Session session, RestSessionRequestFactory factory, GetSimpleItemModelRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(GetSimpleItemModelRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "inventory")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/simple/inventory/{inventoryName}/item/{itemName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{inventoryName}", !string.IsNullOrEmpty(request.InventoryName) ? request.InventoryName.ToString() : "null");
                url = url.Replace("{itemName}", !string.IsNullOrEmpty(request.ItemName) ? request.ItemName.ToString() : "null");

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
		public IEnumerator GetSimpleItemModel(
                Request.GetSimpleItemModelRequest request,
                UnityAction<AsyncResult<Result.GetSimpleItemModelResult>> callback
        )
		{
			var task = new GetSimpleItemModelTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.GetSimpleItemModelResult>(task.Result, task.Error));
        }

		public IFuture<Result.GetSimpleItemModelResult> GetSimpleItemModelFuture(
                Request.GetSimpleItemModelRequest request
        )
		{
			return new GetSimpleItemModelTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.GetSimpleItemModelResult> GetSimpleItemModelAsync(
                Request.GetSimpleItemModelRequest request
        )
		{
            AsyncResult<Result.GetSimpleItemModelResult> result = null;
			await GetSimpleItemModel(
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
		public GetSimpleItemModelTask GetSimpleItemModelAsync(
                Request.GetSimpleItemModelRequest request
        )
		{
			return new GetSimpleItemModelTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.GetSimpleItemModelResult> GetSimpleItemModelAsync(
                Request.GetSimpleItemModelRequest request
        )
		{
			var task = new GetSimpleItemModelTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class DescribeBigInventoryModelMastersTask : Gs2RestSessionTask<DescribeBigInventoryModelMastersRequest, DescribeBigInventoryModelMastersResult>
        {
            public DescribeBigInventoryModelMastersTask(IGs2Session session, RestSessionRequestFactory factory, DescribeBigInventoryModelMastersRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DescribeBigInventoryModelMastersRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "inventory")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/master/big/inventory";

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
		public IEnumerator DescribeBigInventoryModelMasters(
                Request.DescribeBigInventoryModelMastersRequest request,
                UnityAction<AsyncResult<Result.DescribeBigInventoryModelMastersResult>> callback
        )
		{
			var task = new DescribeBigInventoryModelMastersTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.DescribeBigInventoryModelMastersResult>(task.Result, task.Error));
        }

		public IFuture<Result.DescribeBigInventoryModelMastersResult> DescribeBigInventoryModelMastersFuture(
                Request.DescribeBigInventoryModelMastersRequest request
        )
		{
			return new DescribeBigInventoryModelMastersTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.DescribeBigInventoryModelMastersResult> DescribeBigInventoryModelMastersAsync(
                Request.DescribeBigInventoryModelMastersRequest request
        )
		{
            AsyncResult<Result.DescribeBigInventoryModelMastersResult> result = null;
			await DescribeBigInventoryModelMasters(
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
		public DescribeBigInventoryModelMastersTask DescribeBigInventoryModelMastersAsync(
                Request.DescribeBigInventoryModelMastersRequest request
        )
		{
			return new DescribeBigInventoryModelMastersTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.DescribeBigInventoryModelMastersResult> DescribeBigInventoryModelMastersAsync(
                Request.DescribeBigInventoryModelMastersRequest request
        )
		{
			var task = new DescribeBigInventoryModelMastersTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class CreateBigInventoryModelMasterTask : Gs2RestSessionTask<CreateBigInventoryModelMasterRequest, CreateBigInventoryModelMasterResult>
        {
            public CreateBigInventoryModelMasterTask(IGs2Session session, RestSessionRequestFactory factory, CreateBigInventoryModelMasterRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(CreateBigInventoryModelMasterRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "inventory")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/master/big/inventory";

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
		public IEnumerator CreateBigInventoryModelMaster(
                Request.CreateBigInventoryModelMasterRequest request,
                UnityAction<AsyncResult<Result.CreateBigInventoryModelMasterResult>> callback
        )
		{
			var task = new CreateBigInventoryModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.CreateBigInventoryModelMasterResult>(task.Result, task.Error));
        }

		public IFuture<Result.CreateBigInventoryModelMasterResult> CreateBigInventoryModelMasterFuture(
                Request.CreateBigInventoryModelMasterRequest request
        )
		{
			return new CreateBigInventoryModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.CreateBigInventoryModelMasterResult> CreateBigInventoryModelMasterAsync(
                Request.CreateBigInventoryModelMasterRequest request
        )
		{
            AsyncResult<Result.CreateBigInventoryModelMasterResult> result = null;
			await CreateBigInventoryModelMaster(
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
		public CreateBigInventoryModelMasterTask CreateBigInventoryModelMasterAsync(
                Request.CreateBigInventoryModelMasterRequest request
        )
		{
			return new CreateBigInventoryModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.CreateBigInventoryModelMasterResult> CreateBigInventoryModelMasterAsync(
                Request.CreateBigInventoryModelMasterRequest request
        )
		{
			var task = new CreateBigInventoryModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class GetBigInventoryModelMasterTask : Gs2RestSessionTask<GetBigInventoryModelMasterRequest, GetBigInventoryModelMasterResult>
        {
            public GetBigInventoryModelMasterTask(IGs2Session session, RestSessionRequestFactory factory, GetBigInventoryModelMasterRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(GetBigInventoryModelMasterRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "inventory")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/master/big/inventory/{inventoryName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{inventoryName}", !string.IsNullOrEmpty(request.InventoryName) ? request.InventoryName.ToString() : "null");

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
		public IEnumerator GetBigInventoryModelMaster(
                Request.GetBigInventoryModelMasterRequest request,
                UnityAction<AsyncResult<Result.GetBigInventoryModelMasterResult>> callback
        )
		{
			var task = new GetBigInventoryModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.GetBigInventoryModelMasterResult>(task.Result, task.Error));
        }

		public IFuture<Result.GetBigInventoryModelMasterResult> GetBigInventoryModelMasterFuture(
                Request.GetBigInventoryModelMasterRequest request
        )
		{
			return new GetBigInventoryModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.GetBigInventoryModelMasterResult> GetBigInventoryModelMasterAsync(
                Request.GetBigInventoryModelMasterRequest request
        )
		{
            AsyncResult<Result.GetBigInventoryModelMasterResult> result = null;
			await GetBigInventoryModelMaster(
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
		public GetBigInventoryModelMasterTask GetBigInventoryModelMasterAsync(
                Request.GetBigInventoryModelMasterRequest request
        )
		{
			return new GetBigInventoryModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.GetBigInventoryModelMasterResult> GetBigInventoryModelMasterAsync(
                Request.GetBigInventoryModelMasterRequest request
        )
		{
			var task = new GetBigInventoryModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class UpdateBigInventoryModelMasterTask : Gs2RestSessionTask<UpdateBigInventoryModelMasterRequest, UpdateBigInventoryModelMasterResult>
        {
            public UpdateBigInventoryModelMasterTask(IGs2Session session, RestSessionRequestFactory factory, UpdateBigInventoryModelMasterRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(UpdateBigInventoryModelMasterRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "inventory")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/master/big/inventory/{inventoryName}";

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
		public IEnumerator UpdateBigInventoryModelMaster(
                Request.UpdateBigInventoryModelMasterRequest request,
                UnityAction<AsyncResult<Result.UpdateBigInventoryModelMasterResult>> callback
        )
		{
			var task = new UpdateBigInventoryModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.UpdateBigInventoryModelMasterResult>(task.Result, task.Error));
        }

		public IFuture<Result.UpdateBigInventoryModelMasterResult> UpdateBigInventoryModelMasterFuture(
                Request.UpdateBigInventoryModelMasterRequest request
        )
		{
			return new UpdateBigInventoryModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.UpdateBigInventoryModelMasterResult> UpdateBigInventoryModelMasterAsync(
                Request.UpdateBigInventoryModelMasterRequest request
        )
		{
            AsyncResult<Result.UpdateBigInventoryModelMasterResult> result = null;
			await UpdateBigInventoryModelMaster(
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
		public UpdateBigInventoryModelMasterTask UpdateBigInventoryModelMasterAsync(
                Request.UpdateBigInventoryModelMasterRequest request
        )
		{
			return new UpdateBigInventoryModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.UpdateBigInventoryModelMasterResult> UpdateBigInventoryModelMasterAsync(
                Request.UpdateBigInventoryModelMasterRequest request
        )
		{
			var task = new UpdateBigInventoryModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class DeleteBigInventoryModelMasterTask : Gs2RestSessionTask<DeleteBigInventoryModelMasterRequest, DeleteBigInventoryModelMasterResult>
        {
            public DeleteBigInventoryModelMasterTask(IGs2Session session, RestSessionRequestFactory factory, DeleteBigInventoryModelMasterRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DeleteBigInventoryModelMasterRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "inventory")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/master/big/inventory/{inventoryName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{inventoryName}", !string.IsNullOrEmpty(request.InventoryName) ? request.InventoryName.ToString() : "null");

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
		public IEnumerator DeleteBigInventoryModelMaster(
                Request.DeleteBigInventoryModelMasterRequest request,
                UnityAction<AsyncResult<Result.DeleteBigInventoryModelMasterResult>> callback
        )
		{
			var task = new DeleteBigInventoryModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.DeleteBigInventoryModelMasterResult>(task.Result, task.Error));
        }

		public IFuture<Result.DeleteBigInventoryModelMasterResult> DeleteBigInventoryModelMasterFuture(
                Request.DeleteBigInventoryModelMasterRequest request
        )
		{
			return new DeleteBigInventoryModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.DeleteBigInventoryModelMasterResult> DeleteBigInventoryModelMasterAsync(
                Request.DeleteBigInventoryModelMasterRequest request
        )
		{
            AsyncResult<Result.DeleteBigInventoryModelMasterResult> result = null;
			await DeleteBigInventoryModelMaster(
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
		public DeleteBigInventoryModelMasterTask DeleteBigInventoryModelMasterAsync(
                Request.DeleteBigInventoryModelMasterRequest request
        )
		{
			return new DeleteBigInventoryModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.DeleteBigInventoryModelMasterResult> DeleteBigInventoryModelMasterAsync(
                Request.DeleteBigInventoryModelMasterRequest request
        )
		{
			var task = new DeleteBigInventoryModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class DescribeBigInventoryModelsTask : Gs2RestSessionTask<DescribeBigInventoryModelsRequest, DescribeBigInventoryModelsResult>
        {
            public DescribeBigInventoryModelsTask(IGs2Session session, RestSessionRequestFactory factory, DescribeBigInventoryModelsRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DescribeBigInventoryModelsRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "inventory")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/big/inventory";

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
		public IEnumerator DescribeBigInventoryModels(
                Request.DescribeBigInventoryModelsRequest request,
                UnityAction<AsyncResult<Result.DescribeBigInventoryModelsResult>> callback
        )
		{
			var task = new DescribeBigInventoryModelsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.DescribeBigInventoryModelsResult>(task.Result, task.Error));
        }

		public IFuture<Result.DescribeBigInventoryModelsResult> DescribeBigInventoryModelsFuture(
                Request.DescribeBigInventoryModelsRequest request
        )
		{
			return new DescribeBigInventoryModelsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.DescribeBigInventoryModelsResult> DescribeBigInventoryModelsAsync(
                Request.DescribeBigInventoryModelsRequest request
        )
		{
            AsyncResult<Result.DescribeBigInventoryModelsResult> result = null;
			await DescribeBigInventoryModels(
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
		public DescribeBigInventoryModelsTask DescribeBigInventoryModelsAsync(
                Request.DescribeBigInventoryModelsRequest request
        )
		{
			return new DescribeBigInventoryModelsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.DescribeBigInventoryModelsResult> DescribeBigInventoryModelsAsync(
                Request.DescribeBigInventoryModelsRequest request
        )
		{
			var task = new DescribeBigInventoryModelsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class GetBigInventoryModelTask : Gs2RestSessionTask<GetBigInventoryModelRequest, GetBigInventoryModelResult>
        {
            public GetBigInventoryModelTask(IGs2Session session, RestSessionRequestFactory factory, GetBigInventoryModelRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(GetBigInventoryModelRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "inventory")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/big/inventory/{inventoryName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{inventoryName}", !string.IsNullOrEmpty(request.InventoryName) ? request.InventoryName.ToString() : "null");

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
		public IEnumerator GetBigInventoryModel(
                Request.GetBigInventoryModelRequest request,
                UnityAction<AsyncResult<Result.GetBigInventoryModelResult>> callback
        )
		{
			var task = new GetBigInventoryModelTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.GetBigInventoryModelResult>(task.Result, task.Error));
        }

		public IFuture<Result.GetBigInventoryModelResult> GetBigInventoryModelFuture(
                Request.GetBigInventoryModelRequest request
        )
		{
			return new GetBigInventoryModelTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.GetBigInventoryModelResult> GetBigInventoryModelAsync(
                Request.GetBigInventoryModelRequest request
        )
		{
            AsyncResult<Result.GetBigInventoryModelResult> result = null;
			await GetBigInventoryModel(
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
		public GetBigInventoryModelTask GetBigInventoryModelAsync(
                Request.GetBigInventoryModelRequest request
        )
		{
			return new GetBigInventoryModelTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.GetBigInventoryModelResult> GetBigInventoryModelAsync(
                Request.GetBigInventoryModelRequest request
        )
		{
			var task = new GetBigInventoryModelTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class DescribeBigItemModelMastersTask : Gs2RestSessionTask<DescribeBigItemModelMastersRequest, DescribeBigItemModelMastersResult>
        {
            public DescribeBigItemModelMastersTask(IGs2Session session, RestSessionRequestFactory factory, DescribeBigItemModelMastersRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DescribeBigItemModelMastersRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "inventory")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/master/big/inventory/{inventoryName}/item";

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
		public IEnumerator DescribeBigItemModelMasters(
                Request.DescribeBigItemModelMastersRequest request,
                UnityAction<AsyncResult<Result.DescribeBigItemModelMastersResult>> callback
        )
		{
			var task = new DescribeBigItemModelMastersTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.DescribeBigItemModelMastersResult>(task.Result, task.Error));
        }

		public IFuture<Result.DescribeBigItemModelMastersResult> DescribeBigItemModelMastersFuture(
                Request.DescribeBigItemModelMastersRequest request
        )
		{
			return new DescribeBigItemModelMastersTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.DescribeBigItemModelMastersResult> DescribeBigItemModelMastersAsync(
                Request.DescribeBigItemModelMastersRequest request
        )
		{
            AsyncResult<Result.DescribeBigItemModelMastersResult> result = null;
			await DescribeBigItemModelMasters(
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
		public DescribeBigItemModelMastersTask DescribeBigItemModelMastersAsync(
                Request.DescribeBigItemModelMastersRequest request
        )
		{
			return new DescribeBigItemModelMastersTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.DescribeBigItemModelMastersResult> DescribeBigItemModelMastersAsync(
                Request.DescribeBigItemModelMastersRequest request
        )
		{
			var task = new DescribeBigItemModelMastersTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class CreateBigItemModelMasterTask : Gs2RestSessionTask<CreateBigItemModelMasterRequest, CreateBigItemModelMasterResult>
        {
            public CreateBigItemModelMasterTask(IGs2Session session, RestSessionRequestFactory factory, CreateBigItemModelMasterRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(CreateBigItemModelMasterRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "inventory")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/master/big/inventory/{inventoryName}/item";

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
		public IEnumerator CreateBigItemModelMaster(
                Request.CreateBigItemModelMasterRequest request,
                UnityAction<AsyncResult<Result.CreateBigItemModelMasterResult>> callback
        )
		{
			var task = new CreateBigItemModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.CreateBigItemModelMasterResult>(task.Result, task.Error));
        }

		public IFuture<Result.CreateBigItemModelMasterResult> CreateBigItemModelMasterFuture(
                Request.CreateBigItemModelMasterRequest request
        )
		{
			return new CreateBigItemModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.CreateBigItemModelMasterResult> CreateBigItemModelMasterAsync(
                Request.CreateBigItemModelMasterRequest request
        )
		{
            AsyncResult<Result.CreateBigItemModelMasterResult> result = null;
			await CreateBigItemModelMaster(
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
		public CreateBigItemModelMasterTask CreateBigItemModelMasterAsync(
                Request.CreateBigItemModelMasterRequest request
        )
		{
			return new CreateBigItemModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.CreateBigItemModelMasterResult> CreateBigItemModelMasterAsync(
                Request.CreateBigItemModelMasterRequest request
        )
		{
			var task = new CreateBigItemModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class GetBigItemModelMasterTask : Gs2RestSessionTask<GetBigItemModelMasterRequest, GetBigItemModelMasterResult>
        {
            public GetBigItemModelMasterTask(IGs2Session session, RestSessionRequestFactory factory, GetBigItemModelMasterRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(GetBigItemModelMasterRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "inventory")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/master/big/inventory/{inventoryName}/item/{itemName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{inventoryName}", !string.IsNullOrEmpty(request.InventoryName) ? request.InventoryName.ToString() : "null");
                url = url.Replace("{itemName}", !string.IsNullOrEmpty(request.ItemName) ? request.ItemName.ToString() : "null");

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
		public IEnumerator GetBigItemModelMaster(
                Request.GetBigItemModelMasterRequest request,
                UnityAction<AsyncResult<Result.GetBigItemModelMasterResult>> callback
        )
		{
			var task = new GetBigItemModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.GetBigItemModelMasterResult>(task.Result, task.Error));
        }

		public IFuture<Result.GetBigItemModelMasterResult> GetBigItemModelMasterFuture(
                Request.GetBigItemModelMasterRequest request
        )
		{
			return new GetBigItemModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.GetBigItemModelMasterResult> GetBigItemModelMasterAsync(
                Request.GetBigItemModelMasterRequest request
        )
		{
            AsyncResult<Result.GetBigItemModelMasterResult> result = null;
			await GetBigItemModelMaster(
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
		public GetBigItemModelMasterTask GetBigItemModelMasterAsync(
                Request.GetBigItemModelMasterRequest request
        )
		{
			return new GetBigItemModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.GetBigItemModelMasterResult> GetBigItemModelMasterAsync(
                Request.GetBigItemModelMasterRequest request
        )
		{
			var task = new GetBigItemModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class UpdateBigItemModelMasterTask : Gs2RestSessionTask<UpdateBigItemModelMasterRequest, UpdateBigItemModelMasterResult>
        {
            public UpdateBigItemModelMasterTask(IGs2Session session, RestSessionRequestFactory factory, UpdateBigItemModelMasterRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(UpdateBigItemModelMasterRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "inventory")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/master/big/inventory/{inventoryName}/item/{itemName}";

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
		public IEnumerator UpdateBigItemModelMaster(
                Request.UpdateBigItemModelMasterRequest request,
                UnityAction<AsyncResult<Result.UpdateBigItemModelMasterResult>> callback
        )
		{
			var task = new UpdateBigItemModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.UpdateBigItemModelMasterResult>(task.Result, task.Error));
        }

		public IFuture<Result.UpdateBigItemModelMasterResult> UpdateBigItemModelMasterFuture(
                Request.UpdateBigItemModelMasterRequest request
        )
		{
			return new UpdateBigItemModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.UpdateBigItemModelMasterResult> UpdateBigItemModelMasterAsync(
                Request.UpdateBigItemModelMasterRequest request
        )
		{
            AsyncResult<Result.UpdateBigItemModelMasterResult> result = null;
			await UpdateBigItemModelMaster(
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
		public UpdateBigItemModelMasterTask UpdateBigItemModelMasterAsync(
                Request.UpdateBigItemModelMasterRequest request
        )
		{
			return new UpdateBigItemModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.UpdateBigItemModelMasterResult> UpdateBigItemModelMasterAsync(
                Request.UpdateBigItemModelMasterRequest request
        )
		{
			var task = new UpdateBigItemModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class DeleteBigItemModelMasterTask : Gs2RestSessionTask<DeleteBigItemModelMasterRequest, DeleteBigItemModelMasterResult>
        {
            public DeleteBigItemModelMasterTask(IGs2Session session, RestSessionRequestFactory factory, DeleteBigItemModelMasterRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DeleteBigItemModelMasterRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "inventory")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/master/big/inventory/{inventoryName}/item/{itemName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{inventoryName}", !string.IsNullOrEmpty(request.InventoryName) ? request.InventoryName.ToString() : "null");
                url = url.Replace("{itemName}", !string.IsNullOrEmpty(request.ItemName) ? request.ItemName.ToString() : "null");

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
		public IEnumerator DeleteBigItemModelMaster(
                Request.DeleteBigItemModelMasterRequest request,
                UnityAction<AsyncResult<Result.DeleteBigItemModelMasterResult>> callback
        )
		{
			var task = new DeleteBigItemModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.DeleteBigItemModelMasterResult>(task.Result, task.Error));
        }

		public IFuture<Result.DeleteBigItemModelMasterResult> DeleteBigItemModelMasterFuture(
                Request.DeleteBigItemModelMasterRequest request
        )
		{
			return new DeleteBigItemModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.DeleteBigItemModelMasterResult> DeleteBigItemModelMasterAsync(
                Request.DeleteBigItemModelMasterRequest request
        )
		{
            AsyncResult<Result.DeleteBigItemModelMasterResult> result = null;
			await DeleteBigItemModelMaster(
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
		public DeleteBigItemModelMasterTask DeleteBigItemModelMasterAsync(
                Request.DeleteBigItemModelMasterRequest request
        )
		{
			return new DeleteBigItemModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.DeleteBigItemModelMasterResult> DeleteBigItemModelMasterAsync(
                Request.DeleteBigItemModelMasterRequest request
        )
		{
			var task = new DeleteBigItemModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class DescribeBigItemModelsTask : Gs2RestSessionTask<DescribeBigItemModelsRequest, DescribeBigItemModelsResult>
        {
            public DescribeBigItemModelsTask(IGs2Session session, RestSessionRequestFactory factory, DescribeBigItemModelsRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DescribeBigItemModelsRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "inventory")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/big/inventory/{inventoryName}/item";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{inventoryName}", !string.IsNullOrEmpty(request.InventoryName) ? request.InventoryName.ToString() : "null");

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
		public IEnumerator DescribeBigItemModels(
                Request.DescribeBigItemModelsRequest request,
                UnityAction<AsyncResult<Result.DescribeBigItemModelsResult>> callback
        )
		{
			var task = new DescribeBigItemModelsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.DescribeBigItemModelsResult>(task.Result, task.Error));
        }

		public IFuture<Result.DescribeBigItemModelsResult> DescribeBigItemModelsFuture(
                Request.DescribeBigItemModelsRequest request
        )
		{
			return new DescribeBigItemModelsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.DescribeBigItemModelsResult> DescribeBigItemModelsAsync(
                Request.DescribeBigItemModelsRequest request
        )
		{
            AsyncResult<Result.DescribeBigItemModelsResult> result = null;
			await DescribeBigItemModels(
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
		public DescribeBigItemModelsTask DescribeBigItemModelsAsync(
                Request.DescribeBigItemModelsRequest request
        )
		{
			return new DescribeBigItemModelsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.DescribeBigItemModelsResult> DescribeBigItemModelsAsync(
                Request.DescribeBigItemModelsRequest request
        )
		{
			var task = new DescribeBigItemModelsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class GetBigItemModelTask : Gs2RestSessionTask<GetBigItemModelRequest, GetBigItemModelResult>
        {
            public GetBigItemModelTask(IGs2Session session, RestSessionRequestFactory factory, GetBigItemModelRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(GetBigItemModelRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "inventory")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/big/inventory/{inventoryName}/item/{itemName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{inventoryName}", !string.IsNullOrEmpty(request.InventoryName) ? request.InventoryName.ToString() : "null");
                url = url.Replace("{itemName}", !string.IsNullOrEmpty(request.ItemName) ? request.ItemName.ToString() : "null");

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
		public IEnumerator GetBigItemModel(
                Request.GetBigItemModelRequest request,
                UnityAction<AsyncResult<Result.GetBigItemModelResult>> callback
        )
		{
			var task = new GetBigItemModelTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.GetBigItemModelResult>(task.Result, task.Error));
        }

		public IFuture<Result.GetBigItemModelResult> GetBigItemModelFuture(
                Request.GetBigItemModelRequest request
        )
		{
			return new GetBigItemModelTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.GetBigItemModelResult> GetBigItemModelAsync(
                Request.GetBigItemModelRequest request
        )
		{
            AsyncResult<Result.GetBigItemModelResult> result = null;
			await GetBigItemModel(
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
		public GetBigItemModelTask GetBigItemModelAsync(
                Request.GetBigItemModelRequest request
        )
		{
			return new GetBigItemModelTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.GetBigItemModelResult> GetBigItemModelAsync(
                Request.GetBigItemModelRequest request
        )
		{
			var task = new GetBigItemModelTask(
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


        public class VerifyInventoryCurrentMaxCapacityTask : Gs2RestSessionTask<VerifyInventoryCurrentMaxCapacityRequest, VerifyInventoryCurrentMaxCapacityResult>
        {
            public VerifyInventoryCurrentMaxCapacityTask(IGs2Session session, RestSessionRequestFactory factory, VerifyInventoryCurrentMaxCapacityRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(VerifyInventoryCurrentMaxCapacityRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "inventory")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/me/inventory/{inventoryName}/verify/{verifyType}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{inventoryName}", !string.IsNullOrEmpty(request.InventoryName) ? request.InventoryName.ToString() : "null");
                url = url.Replace("{verifyType}", !string.IsNullOrEmpty(request.VerifyType) ? request.VerifyType.ToString() : "null");

                var sessionRequest = Factory.Post(url);

                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);
                jsonWriter.WriteObjectStart();
                if (request.CurrentInventoryMaxCapacity != null)
                {
                    jsonWriter.WritePropertyName("currentInventoryMaxCapacity");
                    jsonWriter.Write(request.CurrentInventoryMaxCapacity.ToString());
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
		public IEnumerator VerifyInventoryCurrentMaxCapacity(
                Request.VerifyInventoryCurrentMaxCapacityRequest request,
                UnityAction<AsyncResult<Result.VerifyInventoryCurrentMaxCapacityResult>> callback
        )
		{
			var task = new VerifyInventoryCurrentMaxCapacityTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.VerifyInventoryCurrentMaxCapacityResult>(task.Result, task.Error));
        }

		public IFuture<Result.VerifyInventoryCurrentMaxCapacityResult> VerifyInventoryCurrentMaxCapacityFuture(
                Request.VerifyInventoryCurrentMaxCapacityRequest request
        )
		{
			return new VerifyInventoryCurrentMaxCapacityTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.VerifyInventoryCurrentMaxCapacityResult> VerifyInventoryCurrentMaxCapacityAsync(
                Request.VerifyInventoryCurrentMaxCapacityRequest request
        )
		{
            AsyncResult<Result.VerifyInventoryCurrentMaxCapacityResult> result = null;
			await VerifyInventoryCurrentMaxCapacity(
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
		public VerifyInventoryCurrentMaxCapacityTask VerifyInventoryCurrentMaxCapacityAsync(
                Request.VerifyInventoryCurrentMaxCapacityRequest request
        )
		{
			return new VerifyInventoryCurrentMaxCapacityTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.VerifyInventoryCurrentMaxCapacityResult> VerifyInventoryCurrentMaxCapacityAsync(
                Request.VerifyInventoryCurrentMaxCapacityRequest request
        )
		{
			var task = new VerifyInventoryCurrentMaxCapacityTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class VerifyInventoryCurrentMaxCapacityByUserIdTask : Gs2RestSessionTask<VerifyInventoryCurrentMaxCapacityByUserIdRequest, VerifyInventoryCurrentMaxCapacityByUserIdResult>
        {
            public VerifyInventoryCurrentMaxCapacityByUserIdTask(IGs2Session session, RestSessionRequestFactory factory, VerifyInventoryCurrentMaxCapacityByUserIdRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(VerifyInventoryCurrentMaxCapacityByUserIdRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "inventory")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/{userId}/inventory/{inventoryName}/verify/{verifyType}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{userId}", !string.IsNullOrEmpty(request.UserId) ? request.UserId.ToString() : "null");
                url = url.Replace("{inventoryName}", !string.IsNullOrEmpty(request.InventoryName) ? request.InventoryName.ToString() : "null");
                url = url.Replace("{verifyType}", !string.IsNullOrEmpty(request.VerifyType) ? request.VerifyType.ToString() : "null");

                var sessionRequest = Factory.Post(url);

                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);
                jsonWriter.WriteObjectStart();
                if (request.CurrentInventoryMaxCapacity != null)
                {
                    jsonWriter.WritePropertyName("currentInventoryMaxCapacity");
                    jsonWriter.Write(request.CurrentInventoryMaxCapacity.ToString());
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
		public IEnumerator VerifyInventoryCurrentMaxCapacityByUserId(
                Request.VerifyInventoryCurrentMaxCapacityByUserIdRequest request,
                UnityAction<AsyncResult<Result.VerifyInventoryCurrentMaxCapacityByUserIdResult>> callback
        )
		{
			var task = new VerifyInventoryCurrentMaxCapacityByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.VerifyInventoryCurrentMaxCapacityByUserIdResult>(task.Result, task.Error));
        }

		public IFuture<Result.VerifyInventoryCurrentMaxCapacityByUserIdResult> VerifyInventoryCurrentMaxCapacityByUserIdFuture(
                Request.VerifyInventoryCurrentMaxCapacityByUserIdRequest request
        )
		{
			return new VerifyInventoryCurrentMaxCapacityByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.VerifyInventoryCurrentMaxCapacityByUserIdResult> VerifyInventoryCurrentMaxCapacityByUserIdAsync(
                Request.VerifyInventoryCurrentMaxCapacityByUserIdRequest request
        )
		{
            AsyncResult<Result.VerifyInventoryCurrentMaxCapacityByUserIdResult> result = null;
			await VerifyInventoryCurrentMaxCapacityByUserId(
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
		public VerifyInventoryCurrentMaxCapacityByUserIdTask VerifyInventoryCurrentMaxCapacityByUserIdAsync(
                Request.VerifyInventoryCurrentMaxCapacityByUserIdRequest request
        )
		{
			return new VerifyInventoryCurrentMaxCapacityByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.VerifyInventoryCurrentMaxCapacityByUserIdResult> VerifyInventoryCurrentMaxCapacityByUserIdAsync(
                Request.VerifyInventoryCurrentMaxCapacityByUserIdRequest request
        )
		{
			var task = new VerifyInventoryCurrentMaxCapacityByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class VerifyInventoryCurrentMaxCapacityByStampTaskTask : Gs2RestSessionTask<VerifyInventoryCurrentMaxCapacityByStampTaskRequest, VerifyInventoryCurrentMaxCapacityByStampTaskResult>
        {
            public VerifyInventoryCurrentMaxCapacityByStampTaskTask(IGs2Session session, RestSessionRequestFactory factory, VerifyInventoryCurrentMaxCapacityByStampTaskRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(VerifyInventoryCurrentMaxCapacityByStampTaskRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "inventory")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/stamp/inventory/verify";

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
		public IEnumerator VerifyInventoryCurrentMaxCapacityByStampTask(
                Request.VerifyInventoryCurrentMaxCapacityByStampTaskRequest request,
                UnityAction<AsyncResult<Result.VerifyInventoryCurrentMaxCapacityByStampTaskResult>> callback
        )
		{
			var task = new VerifyInventoryCurrentMaxCapacityByStampTaskTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.VerifyInventoryCurrentMaxCapacityByStampTaskResult>(task.Result, task.Error));
        }

		public IFuture<Result.VerifyInventoryCurrentMaxCapacityByStampTaskResult> VerifyInventoryCurrentMaxCapacityByStampTaskFuture(
                Request.VerifyInventoryCurrentMaxCapacityByStampTaskRequest request
        )
		{
			return new VerifyInventoryCurrentMaxCapacityByStampTaskTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.VerifyInventoryCurrentMaxCapacityByStampTaskResult> VerifyInventoryCurrentMaxCapacityByStampTaskAsync(
                Request.VerifyInventoryCurrentMaxCapacityByStampTaskRequest request
        )
		{
            AsyncResult<Result.VerifyInventoryCurrentMaxCapacityByStampTaskResult> result = null;
			await VerifyInventoryCurrentMaxCapacityByStampTask(
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
		public VerifyInventoryCurrentMaxCapacityByStampTaskTask VerifyInventoryCurrentMaxCapacityByStampTaskAsync(
                Request.VerifyInventoryCurrentMaxCapacityByStampTaskRequest request
        )
		{
			return new VerifyInventoryCurrentMaxCapacityByStampTaskTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.VerifyInventoryCurrentMaxCapacityByStampTaskResult> VerifyInventoryCurrentMaxCapacityByStampTaskAsync(
                Request.VerifyInventoryCurrentMaxCapacityByStampTaskRequest request
        )
		{
			var task = new VerifyInventoryCurrentMaxCapacityByStampTaskTask(
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

            public override void OnError(Gs2.Core.Exception.Gs2Exception error)
            {
                if (error.Errors.Count(v => v.code == "itemSet.operation.conflict") > 0) {
                    base.OnError(new Exception.ConflictException(error));
                }
                else {
                    base.OnError(error);
                }
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


        public class AcquireItemSetWithGradeByUserIdTask : Gs2RestSessionTask<AcquireItemSetWithGradeByUserIdRequest, AcquireItemSetWithGradeByUserIdResult>
        {
            public AcquireItemSetWithGradeByUserIdTask(IGs2Session session, RestSessionRequestFactory factory, AcquireItemSetWithGradeByUserIdRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(AcquireItemSetWithGradeByUserIdRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "inventory")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/{userId}/inventory/{inventoryName}/item/{itemName}/acquire/grade";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{inventoryName}", !string.IsNullOrEmpty(request.InventoryName) ? request.InventoryName.ToString() : "null");
                url = url.Replace("{itemName}", !string.IsNullOrEmpty(request.ItemName) ? request.ItemName.ToString() : "null");
                url = url.Replace("{userId}", !string.IsNullOrEmpty(request.UserId) ? request.UserId.ToString() : "null");

                var sessionRequest = Factory.Post(url);

                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);
                jsonWriter.WriteObjectStart();
                if (request.GradeModelId != null)
                {
                    jsonWriter.WritePropertyName("gradeModelId");
                    jsonWriter.Write(request.GradeModelId);
                }
                if (request.GradeValue != null)
                {
                    jsonWriter.WritePropertyName("gradeValue");
                    jsonWriter.Write(request.GradeValue.ToString());
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

            public override void OnError(Gs2.Core.Exception.Gs2Exception error)
            {
                if (error.Errors.Count(v => v.code == "itemSet.operation.conflict") > 0) {
                    base.OnError(new Exception.ConflictException(error));
                }
                else {
                    base.OnError(error);
                }
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator AcquireItemSetWithGradeByUserId(
                Request.AcquireItemSetWithGradeByUserIdRequest request,
                UnityAction<AsyncResult<Result.AcquireItemSetWithGradeByUserIdResult>> callback
        )
		{
			var task = new AcquireItemSetWithGradeByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.AcquireItemSetWithGradeByUserIdResult>(task.Result, task.Error));
        }

		public IFuture<Result.AcquireItemSetWithGradeByUserIdResult> AcquireItemSetWithGradeByUserIdFuture(
                Request.AcquireItemSetWithGradeByUserIdRequest request
        )
		{
			return new AcquireItemSetWithGradeByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.AcquireItemSetWithGradeByUserIdResult> AcquireItemSetWithGradeByUserIdAsync(
                Request.AcquireItemSetWithGradeByUserIdRequest request
        )
		{
            AsyncResult<Result.AcquireItemSetWithGradeByUserIdResult> result = null;
			await AcquireItemSetWithGradeByUserId(
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
		public AcquireItemSetWithGradeByUserIdTask AcquireItemSetWithGradeByUserIdAsync(
                Request.AcquireItemSetWithGradeByUserIdRequest request
        )
		{
			return new AcquireItemSetWithGradeByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.AcquireItemSetWithGradeByUserIdResult> AcquireItemSetWithGradeByUserIdAsync(
                Request.AcquireItemSetWithGradeByUserIdRequest request
        )
		{
			var task = new AcquireItemSetWithGradeByUserIdTask(
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

            public override void OnError(Gs2.Core.Exception.Gs2Exception error)
            {
                if (error.Errors.Count(v => v.code == "itemSet.operation.conflict") > 0) {
                    base.OnError(new Exception.ConflictException(error));
                }
                else if (error.Errors.Count(v => v.code == "itemSet.count.insufficient") > 0) {
                    base.OnError(new Exception.InsufficientException(error));
                }
                else {
                    base.OnError(error);
                }
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

            public override void OnError(Gs2.Core.Exception.Gs2Exception error)
            {
                if (error.Errors.Count(v => v.code == "itemSet.operation.conflict") > 0) {
                    base.OnError(new Exception.ConflictException(error));
                }
                else if (error.Errors.Count(v => v.code == "itemSet.count.insufficient") > 0) {
                    base.OnError(new Exception.InsufficientException(error));
                }
                else {
                    base.OnError(error);
                }
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


        public class VerifyItemSetTask : Gs2RestSessionTask<VerifyItemSetRequest, VerifyItemSetResult>
        {
            public VerifyItemSetTask(IGs2Session session, RestSessionRequestFactory factory, VerifyItemSetRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(VerifyItemSetRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "inventory")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/me/inventory/{inventoryName}/item/{itemName}/verify/{verifyType}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{inventoryName}", !string.IsNullOrEmpty(request.InventoryName) ? request.InventoryName.ToString() : "null");
                url = url.Replace("{itemName}", !string.IsNullOrEmpty(request.ItemName) ? request.ItemName.ToString() : "null");
                url = url.Replace("{verifyType}", !string.IsNullOrEmpty(request.VerifyType) ? request.VerifyType.ToString() : "null");

                var sessionRequest = Factory.Post(url);

                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);
                jsonWriter.WriteObjectStart();
                if (request.ItemSetName != null)
                {
                    jsonWriter.WritePropertyName("itemSetName");
                    jsonWriter.Write(request.ItemSetName);
                }
                if (request.Count != null)
                {
                    jsonWriter.WritePropertyName("count");
                    jsonWriter.Write(request.Count.ToString());
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
		public IEnumerator VerifyItemSet(
                Request.VerifyItemSetRequest request,
                UnityAction<AsyncResult<Result.VerifyItemSetResult>> callback
        )
		{
			var task = new VerifyItemSetTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.VerifyItemSetResult>(task.Result, task.Error));
        }

		public IFuture<Result.VerifyItemSetResult> VerifyItemSetFuture(
                Request.VerifyItemSetRequest request
        )
		{
			return new VerifyItemSetTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.VerifyItemSetResult> VerifyItemSetAsync(
                Request.VerifyItemSetRequest request
        )
		{
            AsyncResult<Result.VerifyItemSetResult> result = null;
			await VerifyItemSet(
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
		public VerifyItemSetTask VerifyItemSetAsync(
                Request.VerifyItemSetRequest request
        )
		{
			return new VerifyItemSetTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.VerifyItemSetResult> VerifyItemSetAsync(
                Request.VerifyItemSetRequest request
        )
		{
			var task = new VerifyItemSetTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class VerifyItemSetByUserIdTask : Gs2RestSessionTask<VerifyItemSetByUserIdRequest, VerifyItemSetByUserIdResult>
        {
            public VerifyItemSetByUserIdTask(IGs2Session session, RestSessionRequestFactory factory, VerifyItemSetByUserIdRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(VerifyItemSetByUserIdRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "inventory")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/{userId}/inventory/{inventoryName}/item/{itemName}/verify/{verifyType}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{userId}", !string.IsNullOrEmpty(request.UserId) ? request.UserId.ToString() : "null");
                url = url.Replace("{inventoryName}", !string.IsNullOrEmpty(request.InventoryName) ? request.InventoryName.ToString() : "null");
                url = url.Replace("{itemName}", !string.IsNullOrEmpty(request.ItemName) ? request.ItemName.ToString() : "null");
                url = url.Replace("{verifyType}", !string.IsNullOrEmpty(request.VerifyType) ? request.VerifyType.ToString() : "null");

                var sessionRequest = Factory.Post(url);

                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);
                jsonWriter.WriteObjectStart();
                if (request.ItemSetName != null)
                {
                    jsonWriter.WritePropertyName("itemSetName");
                    jsonWriter.Write(request.ItemSetName);
                }
                if (request.Count != null)
                {
                    jsonWriter.WritePropertyName("count");
                    jsonWriter.Write(request.Count.ToString());
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
		public IEnumerator VerifyItemSetByUserId(
                Request.VerifyItemSetByUserIdRequest request,
                UnityAction<AsyncResult<Result.VerifyItemSetByUserIdResult>> callback
        )
		{
			var task = new VerifyItemSetByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.VerifyItemSetByUserIdResult>(task.Result, task.Error));
        }

		public IFuture<Result.VerifyItemSetByUserIdResult> VerifyItemSetByUserIdFuture(
                Request.VerifyItemSetByUserIdRequest request
        )
		{
			return new VerifyItemSetByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.VerifyItemSetByUserIdResult> VerifyItemSetByUserIdAsync(
                Request.VerifyItemSetByUserIdRequest request
        )
		{
            AsyncResult<Result.VerifyItemSetByUserIdResult> result = null;
			await VerifyItemSetByUserId(
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
		public VerifyItemSetByUserIdTask VerifyItemSetByUserIdAsync(
                Request.VerifyItemSetByUserIdRequest request
        )
		{
			return new VerifyItemSetByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.VerifyItemSetByUserIdResult> VerifyItemSetByUserIdAsync(
                Request.VerifyItemSetByUserIdRequest request
        )
		{
			var task = new VerifyItemSetByUserIdTask(
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


        public class AcquireItemSetWithGradeByStampSheetTask : Gs2RestSessionTask<AcquireItemSetWithGradeByStampSheetRequest, AcquireItemSetWithGradeByStampSheetResult>
        {
            public AcquireItemSetWithGradeByStampSheetTask(IGs2Session session, RestSessionRequestFactory factory, AcquireItemSetWithGradeByStampSheetRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(AcquireItemSetWithGradeByStampSheetRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "inventory")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/stamp/item/acquire/grade";

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
		public IEnumerator AcquireItemSetWithGradeByStampSheet(
                Request.AcquireItemSetWithGradeByStampSheetRequest request,
                UnityAction<AsyncResult<Result.AcquireItemSetWithGradeByStampSheetResult>> callback
        )
		{
			var task = new AcquireItemSetWithGradeByStampSheetTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.AcquireItemSetWithGradeByStampSheetResult>(task.Result, task.Error));
        }

		public IFuture<Result.AcquireItemSetWithGradeByStampSheetResult> AcquireItemSetWithGradeByStampSheetFuture(
                Request.AcquireItemSetWithGradeByStampSheetRequest request
        )
		{
			return new AcquireItemSetWithGradeByStampSheetTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.AcquireItemSetWithGradeByStampSheetResult> AcquireItemSetWithGradeByStampSheetAsync(
                Request.AcquireItemSetWithGradeByStampSheetRequest request
        )
		{
            AsyncResult<Result.AcquireItemSetWithGradeByStampSheetResult> result = null;
			await AcquireItemSetWithGradeByStampSheet(
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
		public AcquireItemSetWithGradeByStampSheetTask AcquireItemSetWithGradeByStampSheetAsync(
                Request.AcquireItemSetWithGradeByStampSheetRequest request
        )
		{
			return new AcquireItemSetWithGradeByStampSheetTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.AcquireItemSetWithGradeByStampSheetResult> AcquireItemSetWithGradeByStampSheetAsync(
                Request.AcquireItemSetWithGradeByStampSheetRequest request
        )
		{
			var task = new AcquireItemSetWithGradeByStampSheetTask(
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


        public class VerifyItemSetByStampTaskTask : Gs2RestSessionTask<VerifyItemSetByStampTaskRequest, VerifyItemSetByStampTaskResult>
        {
            public VerifyItemSetByStampTaskTask(IGs2Session session, RestSessionRequestFactory factory, VerifyItemSetByStampTaskRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(VerifyItemSetByStampTaskRequest request)
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
		public IEnumerator VerifyItemSetByStampTask(
                Request.VerifyItemSetByStampTaskRequest request,
                UnityAction<AsyncResult<Result.VerifyItemSetByStampTaskResult>> callback
        )
		{
			var task = new VerifyItemSetByStampTaskTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.VerifyItemSetByStampTaskResult>(task.Result, task.Error));
        }

		public IFuture<Result.VerifyItemSetByStampTaskResult> VerifyItemSetByStampTaskFuture(
                Request.VerifyItemSetByStampTaskRequest request
        )
		{
			return new VerifyItemSetByStampTaskTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.VerifyItemSetByStampTaskResult> VerifyItemSetByStampTaskAsync(
                Request.VerifyItemSetByStampTaskRequest request
        )
		{
            AsyncResult<Result.VerifyItemSetByStampTaskResult> result = null;
			await VerifyItemSetByStampTask(
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
		public VerifyItemSetByStampTaskTask VerifyItemSetByStampTaskAsync(
                Request.VerifyItemSetByStampTaskRequest request
        )
		{
			return new VerifyItemSetByStampTaskTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.VerifyItemSetByStampTaskResult> VerifyItemSetByStampTaskAsync(
                Request.VerifyItemSetByStampTaskRequest request
        )
		{
			var task = new VerifyItemSetByStampTaskTask(
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
                    + "/stamp/item/reference/verify";

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


        public class DescribeSimpleItemsTask : Gs2RestSessionTask<DescribeSimpleItemsRequest, DescribeSimpleItemsResult>
        {
            public DescribeSimpleItemsTask(IGs2Session session, RestSessionRequestFactory factory, DescribeSimpleItemsRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DescribeSimpleItemsRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "inventory")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/me/simple/inventory/{inventoryName}/item";

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
		public IEnumerator DescribeSimpleItems(
                Request.DescribeSimpleItemsRequest request,
                UnityAction<AsyncResult<Result.DescribeSimpleItemsResult>> callback
        )
		{
			var task = new DescribeSimpleItemsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.DescribeSimpleItemsResult>(task.Result, task.Error));
        }

		public IFuture<Result.DescribeSimpleItemsResult> DescribeSimpleItemsFuture(
                Request.DescribeSimpleItemsRequest request
        )
		{
			return new DescribeSimpleItemsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.DescribeSimpleItemsResult> DescribeSimpleItemsAsync(
                Request.DescribeSimpleItemsRequest request
        )
		{
            AsyncResult<Result.DescribeSimpleItemsResult> result = null;
			await DescribeSimpleItems(
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
		public DescribeSimpleItemsTask DescribeSimpleItemsAsync(
                Request.DescribeSimpleItemsRequest request
        )
		{
			return new DescribeSimpleItemsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.DescribeSimpleItemsResult> DescribeSimpleItemsAsync(
                Request.DescribeSimpleItemsRequest request
        )
		{
			var task = new DescribeSimpleItemsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class DescribeSimpleItemsByUserIdTask : Gs2RestSessionTask<DescribeSimpleItemsByUserIdRequest, DescribeSimpleItemsByUserIdResult>
        {
            public DescribeSimpleItemsByUserIdTask(IGs2Session session, RestSessionRequestFactory factory, DescribeSimpleItemsByUserIdRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DescribeSimpleItemsByUserIdRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "inventory")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/{userId}/simple/inventory/{inventoryName}/item";

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
		public IEnumerator DescribeSimpleItemsByUserId(
                Request.DescribeSimpleItemsByUserIdRequest request,
                UnityAction<AsyncResult<Result.DescribeSimpleItemsByUserIdResult>> callback
        )
		{
			var task = new DescribeSimpleItemsByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.DescribeSimpleItemsByUserIdResult>(task.Result, task.Error));
        }

		public IFuture<Result.DescribeSimpleItemsByUserIdResult> DescribeSimpleItemsByUserIdFuture(
                Request.DescribeSimpleItemsByUserIdRequest request
        )
		{
			return new DescribeSimpleItemsByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.DescribeSimpleItemsByUserIdResult> DescribeSimpleItemsByUserIdAsync(
                Request.DescribeSimpleItemsByUserIdRequest request
        )
		{
            AsyncResult<Result.DescribeSimpleItemsByUserIdResult> result = null;
			await DescribeSimpleItemsByUserId(
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
		public DescribeSimpleItemsByUserIdTask DescribeSimpleItemsByUserIdAsync(
                Request.DescribeSimpleItemsByUserIdRequest request
        )
		{
			return new DescribeSimpleItemsByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.DescribeSimpleItemsByUserIdResult> DescribeSimpleItemsByUserIdAsync(
                Request.DescribeSimpleItemsByUserIdRequest request
        )
		{
			var task = new DescribeSimpleItemsByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class GetSimpleItemTask : Gs2RestSessionTask<GetSimpleItemRequest, GetSimpleItemResult>
        {
            public GetSimpleItemTask(IGs2Session session, RestSessionRequestFactory factory, GetSimpleItemRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(GetSimpleItemRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "inventory")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/me/simple/inventory/{inventoryName}/item/{itemName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{inventoryName}", !string.IsNullOrEmpty(request.InventoryName) ? request.InventoryName.ToString() : "null");
                url = url.Replace("{itemName}", !string.IsNullOrEmpty(request.ItemName) ? request.ItemName.ToString() : "null");

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
		public IEnumerator GetSimpleItem(
                Request.GetSimpleItemRequest request,
                UnityAction<AsyncResult<Result.GetSimpleItemResult>> callback
        )
		{
			var task = new GetSimpleItemTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.GetSimpleItemResult>(task.Result, task.Error));
        }

		public IFuture<Result.GetSimpleItemResult> GetSimpleItemFuture(
                Request.GetSimpleItemRequest request
        )
		{
			return new GetSimpleItemTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.GetSimpleItemResult> GetSimpleItemAsync(
                Request.GetSimpleItemRequest request
        )
		{
            AsyncResult<Result.GetSimpleItemResult> result = null;
			await GetSimpleItem(
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
		public GetSimpleItemTask GetSimpleItemAsync(
                Request.GetSimpleItemRequest request
        )
		{
			return new GetSimpleItemTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.GetSimpleItemResult> GetSimpleItemAsync(
                Request.GetSimpleItemRequest request
        )
		{
			var task = new GetSimpleItemTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class GetSimpleItemByUserIdTask : Gs2RestSessionTask<GetSimpleItemByUserIdRequest, GetSimpleItemByUserIdResult>
        {
            public GetSimpleItemByUserIdTask(IGs2Session session, RestSessionRequestFactory factory, GetSimpleItemByUserIdRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(GetSimpleItemByUserIdRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "inventory")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/{userId}/simple/inventory/{inventoryName}/item/{itemName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{inventoryName}", !string.IsNullOrEmpty(request.InventoryName) ? request.InventoryName.ToString() : "null");
                url = url.Replace("{userId}", !string.IsNullOrEmpty(request.UserId) ? request.UserId.ToString() : "null");
                url = url.Replace("{itemName}", !string.IsNullOrEmpty(request.ItemName) ? request.ItemName.ToString() : "null");

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
		public IEnumerator GetSimpleItemByUserId(
                Request.GetSimpleItemByUserIdRequest request,
                UnityAction<AsyncResult<Result.GetSimpleItemByUserIdResult>> callback
        )
		{
			var task = new GetSimpleItemByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.GetSimpleItemByUserIdResult>(task.Result, task.Error));
        }

		public IFuture<Result.GetSimpleItemByUserIdResult> GetSimpleItemByUserIdFuture(
                Request.GetSimpleItemByUserIdRequest request
        )
		{
			return new GetSimpleItemByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.GetSimpleItemByUserIdResult> GetSimpleItemByUserIdAsync(
                Request.GetSimpleItemByUserIdRequest request
        )
		{
            AsyncResult<Result.GetSimpleItemByUserIdResult> result = null;
			await GetSimpleItemByUserId(
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
		public GetSimpleItemByUserIdTask GetSimpleItemByUserIdAsync(
                Request.GetSimpleItemByUserIdRequest request
        )
		{
			return new GetSimpleItemByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.GetSimpleItemByUserIdResult> GetSimpleItemByUserIdAsync(
                Request.GetSimpleItemByUserIdRequest request
        )
		{
			var task = new GetSimpleItemByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class GetSimpleItemWithSignatureTask : Gs2RestSessionTask<GetSimpleItemWithSignatureRequest, GetSimpleItemWithSignatureResult>
        {
            public GetSimpleItemWithSignatureTask(IGs2Session session, RestSessionRequestFactory factory, GetSimpleItemWithSignatureRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(GetSimpleItemWithSignatureRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "inventory")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/me/simple/inventory/{inventoryName}/item/{itemName}/signature";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{inventoryName}", !string.IsNullOrEmpty(request.InventoryName) ? request.InventoryName.ToString() : "null");
                url = url.Replace("{itemName}", !string.IsNullOrEmpty(request.ItemName) ? request.ItemName.ToString() : "null");

                var sessionRequest = Factory.Get(url);
                if (request.ContextStack != null)
                {
                    sessionRequest.AddQueryString("contextStack", request.ContextStack);
                }
                if (request.KeyId != null) {
                    sessionRequest.AddQueryString("keyId", $"{request.KeyId}");
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
		public IEnumerator GetSimpleItemWithSignature(
                Request.GetSimpleItemWithSignatureRequest request,
                UnityAction<AsyncResult<Result.GetSimpleItemWithSignatureResult>> callback
        )
		{
			var task = new GetSimpleItemWithSignatureTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.GetSimpleItemWithSignatureResult>(task.Result, task.Error));
        }

		public IFuture<Result.GetSimpleItemWithSignatureResult> GetSimpleItemWithSignatureFuture(
                Request.GetSimpleItemWithSignatureRequest request
        )
		{
			return new GetSimpleItemWithSignatureTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.GetSimpleItemWithSignatureResult> GetSimpleItemWithSignatureAsync(
                Request.GetSimpleItemWithSignatureRequest request
        )
		{
            AsyncResult<Result.GetSimpleItemWithSignatureResult> result = null;
			await GetSimpleItemWithSignature(
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
		public GetSimpleItemWithSignatureTask GetSimpleItemWithSignatureAsync(
                Request.GetSimpleItemWithSignatureRequest request
        )
		{
			return new GetSimpleItemWithSignatureTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.GetSimpleItemWithSignatureResult> GetSimpleItemWithSignatureAsync(
                Request.GetSimpleItemWithSignatureRequest request
        )
		{
			var task = new GetSimpleItemWithSignatureTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class GetSimpleItemWithSignatureByUserIdTask : Gs2RestSessionTask<GetSimpleItemWithSignatureByUserIdRequest, GetSimpleItemWithSignatureByUserIdResult>
        {
            public GetSimpleItemWithSignatureByUserIdTask(IGs2Session session, RestSessionRequestFactory factory, GetSimpleItemWithSignatureByUserIdRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(GetSimpleItemWithSignatureByUserIdRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "inventory")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/{userId}/simple/inventory/{inventoryName}/item/{itemName}/signature";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{inventoryName}", !string.IsNullOrEmpty(request.InventoryName) ? request.InventoryName.ToString() : "null");
                url = url.Replace("{userId}", !string.IsNullOrEmpty(request.UserId) ? request.UserId.ToString() : "null");
                url = url.Replace("{itemName}", !string.IsNullOrEmpty(request.ItemName) ? request.ItemName.ToString() : "null");

                var sessionRequest = Factory.Get(url);
                if (request.ContextStack != null)
                {
                    sessionRequest.AddQueryString("contextStack", request.ContextStack);
                }
                if (request.KeyId != null) {
                    sessionRequest.AddQueryString("keyId", $"{request.KeyId}");
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
		public IEnumerator GetSimpleItemWithSignatureByUserId(
                Request.GetSimpleItemWithSignatureByUserIdRequest request,
                UnityAction<AsyncResult<Result.GetSimpleItemWithSignatureByUserIdResult>> callback
        )
		{
			var task = new GetSimpleItemWithSignatureByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.GetSimpleItemWithSignatureByUserIdResult>(task.Result, task.Error));
        }

		public IFuture<Result.GetSimpleItemWithSignatureByUserIdResult> GetSimpleItemWithSignatureByUserIdFuture(
                Request.GetSimpleItemWithSignatureByUserIdRequest request
        )
		{
			return new GetSimpleItemWithSignatureByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.GetSimpleItemWithSignatureByUserIdResult> GetSimpleItemWithSignatureByUserIdAsync(
                Request.GetSimpleItemWithSignatureByUserIdRequest request
        )
		{
            AsyncResult<Result.GetSimpleItemWithSignatureByUserIdResult> result = null;
			await GetSimpleItemWithSignatureByUserId(
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
		public GetSimpleItemWithSignatureByUserIdTask GetSimpleItemWithSignatureByUserIdAsync(
                Request.GetSimpleItemWithSignatureByUserIdRequest request
        )
		{
			return new GetSimpleItemWithSignatureByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.GetSimpleItemWithSignatureByUserIdResult> GetSimpleItemWithSignatureByUserIdAsync(
                Request.GetSimpleItemWithSignatureByUserIdRequest request
        )
		{
			var task = new GetSimpleItemWithSignatureByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class AcquireSimpleItemsByUserIdTask : Gs2RestSessionTask<AcquireSimpleItemsByUserIdRequest, AcquireSimpleItemsByUserIdResult>
        {
            public AcquireSimpleItemsByUserIdTask(IGs2Session session, RestSessionRequestFactory factory, AcquireSimpleItemsByUserIdRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(AcquireSimpleItemsByUserIdRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "inventory")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/{userId}/simple/inventory/{inventoryName}/acquire";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{inventoryName}", !string.IsNullOrEmpty(request.InventoryName) ? request.InventoryName.ToString() : "null");
                url = url.Replace("{userId}", !string.IsNullOrEmpty(request.UserId) ? request.UserId.ToString() : "null");

                var sessionRequest = Factory.Post(url);

                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);
                jsonWriter.WriteObjectStart();
                if (request.AcquireCounts != null)
                {
                    jsonWriter.WritePropertyName("acquireCounts");
                    jsonWriter.WriteArrayStart();
                    foreach(var item in request.AcquireCounts)
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

            public override void OnError(Gs2.Core.Exception.Gs2Exception error)
            {
                if (error.Errors.Count(v => v.code == "itemSet.operation.conflict") > 0) {
                    base.OnError(new Exception.ConflictException(error));
                }
                else {
                    base.OnError(error);
                }
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator AcquireSimpleItemsByUserId(
                Request.AcquireSimpleItemsByUserIdRequest request,
                UnityAction<AsyncResult<Result.AcquireSimpleItemsByUserIdResult>> callback
        )
		{
			var task = new AcquireSimpleItemsByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.AcquireSimpleItemsByUserIdResult>(task.Result, task.Error));
        }

		public IFuture<Result.AcquireSimpleItemsByUserIdResult> AcquireSimpleItemsByUserIdFuture(
                Request.AcquireSimpleItemsByUserIdRequest request
        )
		{
			return new AcquireSimpleItemsByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.AcquireSimpleItemsByUserIdResult> AcquireSimpleItemsByUserIdAsync(
                Request.AcquireSimpleItemsByUserIdRequest request
        )
		{
            AsyncResult<Result.AcquireSimpleItemsByUserIdResult> result = null;
			await AcquireSimpleItemsByUserId(
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
		public AcquireSimpleItemsByUserIdTask AcquireSimpleItemsByUserIdAsync(
                Request.AcquireSimpleItemsByUserIdRequest request
        )
		{
			return new AcquireSimpleItemsByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.AcquireSimpleItemsByUserIdResult> AcquireSimpleItemsByUserIdAsync(
                Request.AcquireSimpleItemsByUserIdRequest request
        )
		{
			var task = new AcquireSimpleItemsByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class ConsumeSimpleItemsTask : Gs2RestSessionTask<ConsumeSimpleItemsRequest, ConsumeSimpleItemsResult>
        {
            public ConsumeSimpleItemsTask(IGs2Session session, RestSessionRequestFactory factory, ConsumeSimpleItemsRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(ConsumeSimpleItemsRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "inventory")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/me/simple/inventory/{inventoryName}/consume";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{inventoryName}", !string.IsNullOrEmpty(request.InventoryName) ? request.InventoryName.ToString() : "null");

                var sessionRequest = Factory.Post(url);

                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);
                jsonWriter.WriteObjectStart();
                if (request.ConsumeCounts != null)
                {
                    jsonWriter.WritePropertyName("consumeCounts");
                    jsonWriter.WriteArrayStart();
                    foreach(var item in request.ConsumeCounts)
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

            public override void OnError(Gs2.Core.Exception.Gs2Exception error)
            {
                if (error.Errors.Count(v => v.code == "itemSet.operation.conflict") > 0) {
                    base.OnError(new Exception.ConflictException(error));
                }
                else if (error.Errors.Count(v => v.code == "itemSet.count.insufficient") > 0) {
                    base.OnError(new Exception.InsufficientException(error));
                }
                else {
                    base.OnError(error);
                }
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator ConsumeSimpleItems(
                Request.ConsumeSimpleItemsRequest request,
                UnityAction<AsyncResult<Result.ConsumeSimpleItemsResult>> callback
        )
		{
			var task = new ConsumeSimpleItemsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.ConsumeSimpleItemsResult>(task.Result, task.Error));
        }

		public IFuture<Result.ConsumeSimpleItemsResult> ConsumeSimpleItemsFuture(
                Request.ConsumeSimpleItemsRequest request
        )
		{
			return new ConsumeSimpleItemsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.ConsumeSimpleItemsResult> ConsumeSimpleItemsAsync(
                Request.ConsumeSimpleItemsRequest request
        )
		{
            AsyncResult<Result.ConsumeSimpleItemsResult> result = null;
			await ConsumeSimpleItems(
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
		public ConsumeSimpleItemsTask ConsumeSimpleItemsAsync(
                Request.ConsumeSimpleItemsRequest request
        )
		{
			return new ConsumeSimpleItemsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.ConsumeSimpleItemsResult> ConsumeSimpleItemsAsync(
                Request.ConsumeSimpleItemsRequest request
        )
		{
			var task = new ConsumeSimpleItemsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class ConsumeSimpleItemsByUserIdTask : Gs2RestSessionTask<ConsumeSimpleItemsByUserIdRequest, ConsumeSimpleItemsByUserIdResult>
        {
            public ConsumeSimpleItemsByUserIdTask(IGs2Session session, RestSessionRequestFactory factory, ConsumeSimpleItemsByUserIdRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(ConsumeSimpleItemsByUserIdRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "inventory")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/{userId}/simple/inventory/{inventoryName}/consume";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{inventoryName}", !string.IsNullOrEmpty(request.InventoryName) ? request.InventoryName.ToString() : "null");
                url = url.Replace("{userId}", !string.IsNullOrEmpty(request.UserId) ? request.UserId.ToString() : "null");

                var sessionRequest = Factory.Post(url);

                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);
                jsonWriter.WriteObjectStart();
                if (request.ConsumeCounts != null)
                {
                    jsonWriter.WritePropertyName("consumeCounts");
                    jsonWriter.WriteArrayStart();
                    foreach(var item in request.ConsumeCounts)
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

            public override void OnError(Gs2.Core.Exception.Gs2Exception error)
            {
                if (error.Errors.Count(v => v.code == "itemSet.operation.conflict") > 0) {
                    base.OnError(new Exception.ConflictException(error));
                }
                else if (error.Errors.Count(v => v.code == "itemSet.count.insufficient") > 0) {
                    base.OnError(new Exception.InsufficientException(error));
                }
                else {
                    base.OnError(error);
                }
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator ConsumeSimpleItemsByUserId(
                Request.ConsumeSimpleItemsByUserIdRequest request,
                UnityAction<AsyncResult<Result.ConsumeSimpleItemsByUserIdResult>> callback
        )
		{
			var task = new ConsumeSimpleItemsByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.ConsumeSimpleItemsByUserIdResult>(task.Result, task.Error));
        }

		public IFuture<Result.ConsumeSimpleItemsByUserIdResult> ConsumeSimpleItemsByUserIdFuture(
                Request.ConsumeSimpleItemsByUserIdRequest request
        )
		{
			return new ConsumeSimpleItemsByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.ConsumeSimpleItemsByUserIdResult> ConsumeSimpleItemsByUserIdAsync(
                Request.ConsumeSimpleItemsByUserIdRequest request
        )
		{
            AsyncResult<Result.ConsumeSimpleItemsByUserIdResult> result = null;
			await ConsumeSimpleItemsByUserId(
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
		public ConsumeSimpleItemsByUserIdTask ConsumeSimpleItemsByUserIdAsync(
                Request.ConsumeSimpleItemsByUserIdRequest request
        )
		{
			return new ConsumeSimpleItemsByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.ConsumeSimpleItemsByUserIdResult> ConsumeSimpleItemsByUserIdAsync(
                Request.ConsumeSimpleItemsByUserIdRequest request
        )
		{
			var task = new ConsumeSimpleItemsByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class SetSimpleItemsByUserIdTask : Gs2RestSessionTask<SetSimpleItemsByUserIdRequest, SetSimpleItemsByUserIdResult>
        {
            public SetSimpleItemsByUserIdTask(IGs2Session session, RestSessionRequestFactory factory, SetSimpleItemsByUserIdRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(SetSimpleItemsByUserIdRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "inventory")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/{userId}/simple/inventory/{inventoryName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{inventoryName}", !string.IsNullOrEmpty(request.InventoryName) ? request.InventoryName.ToString() : "null");
                url = url.Replace("{userId}", !string.IsNullOrEmpty(request.UserId) ? request.UserId.ToString() : "null");

                var sessionRequest = Factory.Put(url);

                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);
                jsonWriter.WriteObjectStart();
                if (request.Counts != null)
                {
                    jsonWriter.WritePropertyName("counts");
                    jsonWriter.WriteArrayStart();
                    foreach(var item in request.Counts)
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

            public override void OnError(Gs2.Core.Exception.Gs2Exception error)
            {
                if (error.Errors.Count(v => v.code == "itemSet.operation.conflict") > 0) {
                    base.OnError(new Exception.ConflictException(error));
                }
                else {
                    base.OnError(error);
                }
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator SetSimpleItemsByUserId(
                Request.SetSimpleItemsByUserIdRequest request,
                UnityAction<AsyncResult<Result.SetSimpleItemsByUserIdResult>> callback
        )
		{
			var task = new SetSimpleItemsByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.SetSimpleItemsByUserIdResult>(task.Result, task.Error));
        }

		public IFuture<Result.SetSimpleItemsByUserIdResult> SetSimpleItemsByUserIdFuture(
                Request.SetSimpleItemsByUserIdRequest request
        )
		{
			return new SetSimpleItemsByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.SetSimpleItemsByUserIdResult> SetSimpleItemsByUserIdAsync(
                Request.SetSimpleItemsByUserIdRequest request
        )
		{
            AsyncResult<Result.SetSimpleItemsByUserIdResult> result = null;
			await SetSimpleItemsByUserId(
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
		public SetSimpleItemsByUserIdTask SetSimpleItemsByUserIdAsync(
                Request.SetSimpleItemsByUserIdRequest request
        )
		{
			return new SetSimpleItemsByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.SetSimpleItemsByUserIdResult> SetSimpleItemsByUserIdAsync(
                Request.SetSimpleItemsByUserIdRequest request
        )
		{
			var task = new SetSimpleItemsByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class DeleteSimpleItemsByUserIdTask : Gs2RestSessionTask<DeleteSimpleItemsByUserIdRequest, DeleteSimpleItemsByUserIdResult>
        {
            public DeleteSimpleItemsByUserIdTask(IGs2Session session, RestSessionRequestFactory factory, DeleteSimpleItemsByUserIdRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DeleteSimpleItemsByUserIdRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "inventory")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/{userId}/simple/inventory/{inventoryName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{inventoryName}", !string.IsNullOrEmpty(request.InventoryName) ? request.InventoryName.ToString() : "null");
                url = url.Replace("{userId}", !string.IsNullOrEmpty(request.UserId) ? request.UserId.ToString() : "null");

                var sessionRequest = Factory.Delete(url);
                if (request.ContextStack != null)
                {
                    sessionRequest.AddQueryString("contextStack", request.ContextStack);
                }
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
		public IEnumerator DeleteSimpleItemsByUserId(
                Request.DeleteSimpleItemsByUserIdRequest request,
                UnityAction<AsyncResult<Result.DeleteSimpleItemsByUserIdResult>> callback
        )
		{
			var task = new DeleteSimpleItemsByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.DeleteSimpleItemsByUserIdResult>(task.Result, task.Error));
        }

		public IFuture<Result.DeleteSimpleItemsByUserIdResult> DeleteSimpleItemsByUserIdFuture(
                Request.DeleteSimpleItemsByUserIdRequest request
        )
		{
			return new DeleteSimpleItemsByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.DeleteSimpleItemsByUserIdResult> DeleteSimpleItemsByUserIdAsync(
                Request.DeleteSimpleItemsByUserIdRequest request
        )
		{
            AsyncResult<Result.DeleteSimpleItemsByUserIdResult> result = null;
			await DeleteSimpleItemsByUserId(
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
		public DeleteSimpleItemsByUserIdTask DeleteSimpleItemsByUserIdAsync(
                Request.DeleteSimpleItemsByUserIdRequest request
        )
		{
			return new DeleteSimpleItemsByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.DeleteSimpleItemsByUserIdResult> DeleteSimpleItemsByUserIdAsync(
                Request.DeleteSimpleItemsByUserIdRequest request
        )
		{
			var task = new DeleteSimpleItemsByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class VerifySimpleItemTask : Gs2RestSessionTask<VerifySimpleItemRequest, VerifySimpleItemResult>
        {
            public VerifySimpleItemTask(IGs2Session session, RestSessionRequestFactory factory, VerifySimpleItemRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(VerifySimpleItemRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "inventory")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/me/simple/inventory/{inventoryName}/item/{itemName}/verify/{verifyType}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{inventoryName}", !string.IsNullOrEmpty(request.InventoryName) ? request.InventoryName.ToString() : "null");
                url = url.Replace("{itemName}", !string.IsNullOrEmpty(request.ItemName) ? request.ItemName.ToString() : "null");
                url = url.Replace("{verifyType}", !string.IsNullOrEmpty(request.VerifyType) ? request.VerifyType.ToString() : "null");

                var sessionRequest = Factory.Post(url);

                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);
                jsonWriter.WriteObjectStart();
                if (request.Count != null)
                {
                    jsonWriter.WritePropertyName("count");
                    jsonWriter.Write(request.Count.ToString());
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
		public IEnumerator VerifySimpleItem(
                Request.VerifySimpleItemRequest request,
                UnityAction<AsyncResult<Result.VerifySimpleItemResult>> callback
        )
		{
			var task = new VerifySimpleItemTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.VerifySimpleItemResult>(task.Result, task.Error));
        }

		public IFuture<Result.VerifySimpleItemResult> VerifySimpleItemFuture(
                Request.VerifySimpleItemRequest request
        )
		{
			return new VerifySimpleItemTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.VerifySimpleItemResult> VerifySimpleItemAsync(
                Request.VerifySimpleItemRequest request
        )
		{
            AsyncResult<Result.VerifySimpleItemResult> result = null;
			await VerifySimpleItem(
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
		public VerifySimpleItemTask VerifySimpleItemAsync(
                Request.VerifySimpleItemRequest request
        )
		{
			return new VerifySimpleItemTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.VerifySimpleItemResult> VerifySimpleItemAsync(
                Request.VerifySimpleItemRequest request
        )
		{
			var task = new VerifySimpleItemTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class VerifySimpleItemByUserIdTask : Gs2RestSessionTask<VerifySimpleItemByUserIdRequest, VerifySimpleItemByUserIdResult>
        {
            public VerifySimpleItemByUserIdTask(IGs2Session session, RestSessionRequestFactory factory, VerifySimpleItemByUserIdRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(VerifySimpleItemByUserIdRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "inventory")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/{userId}/simple/inventory/{inventoryName}/item/{itemName}/verify/{verifyType}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{userId}", !string.IsNullOrEmpty(request.UserId) ? request.UserId.ToString() : "null");
                url = url.Replace("{inventoryName}", !string.IsNullOrEmpty(request.InventoryName) ? request.InventoryName.ToString() : "null");
                url = url.Replace("{itemName}", !string.IsNullOrEmpty(request.ItemName) ? request.ItemName.ToString() : "null");
                url = url.Replace("{verifyType}", !string.IsNullOrEmpty(request.VerifyType) ? request.VerifyType.ToString() : "null");

                var sessionRequest = Factory.Post(url);

                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);
                jsonWriter.WriteObjectStart();
                if (request.Count != null)
                {
                    jsonWriter.WritePropertyName("count");
                    jsonWriter.Write(request.Count.ToString());
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
		public IEnumerator VerifySimpleItemByUserId(
                Request.VerifySimpleItemByUserIdRequest request,
                UnityAction<AsyncResult<Result.VerifySimpleItemByUserIdResult>> callback
        )
		{
			var task = new VerifySimpleItemByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.VerifySimpleItemByUserIdResult>(task.Result, task.Error));
        }

		public IFuture<Result.VerifySimpleItemByUserIdResult> VerifySimpleItemByUserIdFuture(
                Request.VerifySimpleItemByUserIdRequest request
        )
		{
			return new VerifySimpleItemByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.VerifySimpleItemByUserIdResult> VerifySimpleItemByUserIdAsync(
                Request.VerifySimpleItemByUserIdRequest request
        )
		{
            AsyncResult<Result.VerifySimpleItemByUserIdResult> result = null;
			await VerifySimpleItemByUserId(
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
		public VerifySimpleItemByUserIdTask VerifySimpleItemByUserIdAsync(
                Request.VerifySimpleItemByUserIdRequest request
        )
		{
			return new VerifySimpleItemByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.VerifySimpleItemByUserIdResult> VerifySimpleItemByUserIdAsync(
                Request.VerifySimpleItemByUserIdRequest request
        )
		{
			var task = new VerifySimpleItemByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class AcquireSimpleItemsByStampSheetTask : Gs2RestSessionTask<AcquireSimpleItemsByStampSheetRequest, AcquireSimpleItemsByStampSheetResult>
        {
            public AcquireSimpleItemsByStampSheetTask(IGs2Session session, RestSessionRequestFactory factory, AcquireSimpleItemsByStampSheetRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(AcquireSimpleItemsByStampSheetRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "inventory")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/stamp/simple/item/acquire";

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
		public IEnumerator AcquireSimpleItemsByStampSheet(
                Request.AcquireSimpleItemsByStampSheetRequest request,
                UnityAction<AsyncResult<Result.AcquireSimpleItemsByStampSheetResult>> callback
        )
		{
			var task = new AcquireSimpleItemsByStampSheetTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.AcquireSimpleItemsByStampSheetResult>(task.Result, task.Error));
        }

		public IFuture<Result.AcquireSimpleItemsByStampSheetResult> AcquireSimpleItemsByStampSheetFuture(
                Request.AcquireSimpleItemsByStampSheetRequest request
        )
		{
			return new AcquireSimpleItemsByStampSheetTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.AcquireSimpleItemsByStampSheetResult> AcquireSimpleItemsByStampSheetAsync(
                Request.AcquireSimpleItemsByStampSheetRequest request
        )
		{
            AsyncResult<Result.AcquireSimpleItemsByStampSheetResult> result = null;
			await AcquireSimpleItemsByStampSheet(
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
		public AcquireSimpleItemsByStampSheetTask AcquireSimpleItemsByStampSheetAsync(
                Request.AcquireSimpleItemsByStampSheetRequest request
        )
		{
			return new AcquireSimpleItemsByStampSheetTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.AcquireSimpleItemsByStampSheetResult> AcquireSimpleItemsByStampSheetAsync(
                Request.AcquireSimpleItemsByStampSheetRequest request
        )
		{
			var task = new AcquireSimpleItemsByStampSheetTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class ConsumeSimpleItemsByStampTaskTask : Gs2RestSessionTask<ConsumeSimpleItemsByStampTaskRequest, ConsumeSimpleItemsByStampTaskResult>
        {
            public ConsumeSimpleItemsByStampTaskTask(IGs2Session session, RestSessionRequestFactory factory, ConsumeSimpleItemsByStampTaskRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(ConsumeSimpleItemsByStampTaskRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "inventory")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/stamp/simple/item/consume";

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
		public IEnumerator ConsumeSimpleItemsByStampTask(
                Request.ConsumeSimpleItemsByStampTaskRequest request,
                UnityAction<AsyncResult<Result.ConsumeSimpleItemsByStampTaskResult>> callback
        )
		{
			var task = new ConsumeSimpleItemsByStampTaskTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.ConsumeSimpleItemsByStampTaskResult>(task.Result, task.Error));
        }

		public IFuture<Result.ConsumeSimpleItemsByStampTaskResult> ConsumeSimpleItemsByStampTaskFuture(
                Request.ConsumeSimpleItemsByStampTaskRequest request
        )
		{
			return new ConsumeSimpleItemsByStampTaskTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.ConsumeSimpleItemsByStampTaskResult> ConsumeSimpleItemsByStampTaskAsync(
                Request.ConsumeSimpleItemsByStampTaskRequest request
        )
		{
            AsyncResult<Result.ConsumeSimpleItemsByStampTaskResult> result = null;
			await ConsumeSimpleItemsByStampTask(
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
		public ConsumeSimpleItemsByStampTaskTask ConsumeSimpleItemsByStampTaskAsync(
                Request.ConsumeSimpleItemsByStampTaskRequest request
        )
		{
			return new ConsumeSimpleItemsByStampTaskTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.ConsumeSimpleItemsByStampTaskResult> ConsumeSimpleItemsByStampTaskAsync(
                Request.ConsumeSimpleItemsByStampTaskRequest request
        )
		{
			var task = new ConsumeSimpleItemsByStampTaskTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class SetSimpleItemsByStampSheetTask : Gs2RestSessionTask<SetSimpleItemsByStampSheetRequest, SetSimpleItemsByStampSheetResult>
        {
            public SetSimpleItemsByStampSheetTask(IGs2Session session, RestSessionRequestFactory factory, SetSimpleItemsByStampSheetRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(SetSimpleItemsByStampSheetRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "inventory")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/stamp/simple/item/set";

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
		public IEnumerator SetSimpleItemsByStampSheet(
                Request.SetSimpleItemsByStampSheetRequest request,
                UnityAction<AsyncResult<Result.SetSimpleItemsByStampSheetResult>> callback
        )
		{
			var task = new SetSimpleItemsByStampSheetTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.SetSimpleItemsByStampSheetResult>(task.Result, task.Error));
        }

		public IFuture<Result.SetSimpleItemsByStampSheetResult> SetSimpleItemsByStampSheetFuture(
                Request.SetSimpleItemsByStampSheetRequest request
        )
		{
			return new SetSimpleItemsByStampSheetTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.SetSimpleItemsByStampSheetResult> SetSimpleItemsByStampSheetAsync(
                Request.SetSimpleItemsByStampSheetRequest request
        )
		{
            AsyncResult<Result.SetSimpleItemsByStampSheetResult> result = null;
			await SetSimpleItemsByStampSheet(
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
		public SetSimpleItemsByStampSheetTask SetSimpleItemsByStampSheetAsync(
                Request.SetSimpleItemsByStampSheetRequest request
        )
		{
			return new SetSimpleItemsByStampSheetTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.SetSimpleItemsByStampSheetResult> SetSimpleItemsByStampSheetAsync(
                Request.SetSimpleItemsByStampSheetRequest request
        )
		{
			var task = new SetSimpleItemsByStampSheetTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class VerifySimpleItemByStampTaskTask : Gs2RestSessionTask<VerifySimpleItemByStampTaskRequest, VerifySimpleItemByStampTaskResult>
        {
            public VerifySimpleItemByStampTaskTask(IGs2Session session, RestSessionRequestFactory factory, VerifySimpleItemByStampTaskRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(VerifySimpleItemByStampTaskRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "inventory")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/stamp/simple/item/verify";

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
		public IEnumerator VerifySimpleItemByStampTask(
                Request.VerifySimpleItemByStampTaskRequest request,
                UnityAction<AsyncResult<Result.VerifySimpleItemByStampTaskResult>> callback
        )
		{
			var task = new VerifySimpleItemByStampTaskTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.VerifySimpleItemByStampTaskResult>(task.Result, task.Error));
        }

		public IFuture<Result.VerifySimpleItemByStampTaskResult> VerifySimpleItemByStampTaskFuture(
                Request.VerifySimpleItemByStampTaskRequest request
        )
		{
			return new VerifySimpleItemByStampTaskTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.VerifySimpleItemByStampTaskResult> VerifySimpleItemByStampTaskAsync(
                Request.VerifySimpleItemByStampTaskRequest request
        )
		{
            AsyncResult<Result.VerifySimpleItemByStampTaskResult> result = null;
			await VerifySimpleItemByStampTask(
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
		public VerifySimpleItemByStampTaskTask VerifySimpleItemByStampTaskAsync(
                Request.VerifySimpleItemByStampTaskRequest request
        )
		{
			return new VerifySimpleItemByStampTaskTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.VerifySimpleItemByStampTaskResult> VerifySimpleItemByStampTaskAsync(
                Request.VerifySimpleItemByStampTaskRequest request
        )
		{
			var task = new VerifySimpleItemByStampTaskTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class DescribeBigItemsTask : Gs2RestSessionTask<DescribeBigItemsRequest, DescribeBigItemsResult>
        {
            public DescribeBigItemsTask(IGs2Session session, RestSessionRequestFactory factory, DescribeBigItemsRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DescribeBigItemsRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "inventory")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/me/big/inventory/{inventoryName}/item";

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
		public IEnumerator DescribeBigItems(
                Request.DescribeBigItemsRequest request,
                UnityAction<AsyncResult<Result.DescribeBigItemsResult>> callback
        )
		{
			var task = new DescribeBigItemsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.DescribeBigItemsResult>(task.Result, task.Error));
        }

		public IFuture<Result.DescribeBigItemsResult> DescribeBigItemsFuture(
                Request.DescribeBigItemsRequest request
        )
		{
			return new DescribeBigItemsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.DescribeBigItemsResult> DescribeBigItemsAsync(
                Request.DescribeBigItemsRequest request
        )
		{
            AsyncResult<Result.DescribeBigItemsResult> result = null;
			await DescribeBigItems(
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
		public DescribeBigItemsTask DescribeBigItemsAsync(
                Request.DescribeBigItemsRequest request
        )
		{
			return new DescribeBigItemsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.DescribeBigItemsResult> DescribeBigItemsAsync(
                Request.DescribeBigItemsRequest request
        )
		{
			var task = new DescribeBigItemsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class DescribeBigItemsByUserIdTask : Gs2RestSessionTask<DescribeBigItemsByUserIdRequest, DescribeBigItemsByUserIdResult>
        {
            public DescribeBigItemsByUserIdTask(IGs2Session session, RestSessionRequestFactory factory, DescribeBigItemsByUserIdRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DescribeBigItemsByUserIdRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "inventory")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/{userId}/big/inventory/{inventoryName}/item";

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
		public IEnumerator DescribeBigItemsByUserId(
                Request.DescribeBigItemsByUserIdRequest request,
                UnityAction<AsyncResult<Result.DescribeBigItemsByUserIdResult>> callback
        )
		{
			var task = new DescribeBigItemsByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.DescribeBigItemsByUserIdResult>(task.Result, task.Error));
        }

		public IFuture<Result.DescribeBigItemsByUserIdResult> DescribeBigItemsByUserIdFuture(
                Request.DescribeBigItemsByUserIdRequest request
        )
		{
			return new DescribeBigItemsByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.DescribeBigItemsByUserIdResult> DescribeBigItemsByUserIdAsync(
                Request.DescribeBigItemsByUserIdRequest request
        )
		{
            AsyncResult<Result.DescribeBigItemsByUserIdResult> result = null;
			await DescribeBigItemsByUserId(
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
		public DescribeBigItemsByUserIdTask DescribeBigItemsByUserIdAsync(
                Request.DescribeBigItemsByUserIdRequest request
        )
		{
			return new DescribeBigItemsByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.DescribeBigItemsByUserIdResult> DescribeBigItemsByUserIdAsync(
                Request.DescribeBigItemsByUserIdRequest request
        )
		{
			var task = new DescribeBigItemsByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class GetBigItemTask : Gs2RestSessionTask<GetBigItemRequest, GetBigItemResult>
        {
            public GetBigItemTask(IGs2Session session, RestSessionRequestFactory factory, GetBigItemRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(GetBigItemRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "inventory")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/me/big/inventory/{inventoryName}/item/{itemName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{inventoryName}", !string.IsNullOrEmpty(request.InventoryName) ? request.InventoryName.ToString() : "null");
                url = url.Replace("{itemName}", !string.IsNullOrEmpty(request.ItemName) ? request.ItemName.ToString() : "null");

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
		public IEnumerator GetBigItem(
                Request.GetBigItemRequest request,
                UnityAction<AsyncResult<Result.GetBigItemResult>> callback
        )
		{
			var task = new GetBigItemTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.GetBigItemResult>(task.Result, task.Error));
        }

		public IFuture<Result.GetBigItemResult> GetBigItemFuture(
                Request.GetBigItemRequest request
        )
		{
			return new GetBigItemTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.GetBigItemResult> GetBigItemAsync(
                Request.GetBigItemRequest request
        )
		{
            AsyncResult<Result.GetBigItemResult> result = null;
			await GetBigItem(
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
		public GetBigItemTask GetBigItemAsync(
                Request.GetBigItemRequest request
        )
		{
			return new GetBigItemTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.GetBigItemResult> GetBigItemAsync(
                Request.GetBigItemRequest request
        )
		{
			var task = new GetBigItemTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class GetBigItemByUserIdTask : Gs2RestSessionTask<GetBigItemByUserIdRequest, GetBigItemByUserIdResult>
        {
            public GetBigItemByUserIdTask(IGs2Session session, RestSessionRequestFactory factory, GetBigItemByUserIdRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(GetBigItemByUserIdRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "inventory")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/{userId}/big/inventory/{inventoryName}/item/{itemName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{inventoryName}", !string.IsNullOrEmpty(request.InventoryName) ? request.InventoryName.ToString() : "null");
                url = url.Replace("{userId}", !string.IsNullOrEmpty(request.UserId) ? request.UserId.ToString() : "null");
                url = url.Replace("{itemName}", !string.IsNullOrEmpty(request.ItemName) ? request.ItemName.ToString() : "null");

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
		public IEnumerator GetBigItemByUserId(
                Request.GetBigItemByUserIdRequest request,
                UnityAction<AsyncResult<Result.GetBigItemByUserIdResult>> callback
        )
		{
			var task = new GetBigItemByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.GetBigItemByUserIdResult>(task.Result, task.Error));
        }

		public IFuture<Result.GetBigItemByUserIdResult> GetBigItemByUserIdFuture(
                Request.GetBigItemByUserIdRequest request
        )
		{
			return new GetBigItemByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.GetBigItemByUserIdResult> GetBigItemByUserIdAsync(
                Request.GetBigItemByUserIdRequest request
        )
		{
            AsyncResult<Result.GetBigItemByUserIdResult> result = null;
			await GetBigItemByUserId(
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
		public GetBigItemByUserIdTask GetBigItemByUserIdAsync(
                Request.GetBigItemByUserIdRequest request
        )
		{
			return new GetBigItemByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.GetBigItemByUserIdResult> GetBigItemByUserIdAsync(
                Request.GetBigItemByUserIdRequest request
        )
		{
			var task = new GetBigItemByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class AcquireBigItemByUserIdTask : Gs2RestSessionTask<AcquireBigItemByUserIdRequest, AcquireBigItemByUserIdResult>
        {
            public AcquireBigItemByUserIdTask(IGs2Session session, RestSessionRequestFactory factory, AcquireBigItemByUserIdRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(AcquireBigItemByUserIdRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "inventory")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/{userId}/big/inventory/{inventoryName}/item/{itemName}/acquire";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{inventoryName}", !string.IsNullOrEmpty(request.InventoryName) ? request.InventoryName.ToString() : "null");
                url = url.Replace("{userId}", !string.IsNullOrEmpty(request.UserId) ? request.UserId.ToString() : "null");
                url = url.Replace("{itemName}", !string.IsNullOrEmpty(request.ItemName) ? request.ItemName.ToString() : "null");

                var sessionRequest = Factory.Post(url);

                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);
                jsonWriter.WriteObjectStart();
                if (request.AcquireCount != null)
                {
                    jsonWriter.WritePropertyName("acquireCount");
                    jsonWriter.Write(request.AcquireCount);
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

            public override void OnError(Gs2.Core.Exception.Gs2Exception error)
            {
                if (error.Errors.Count(v => v.code == "itemSet.operation.conflict") > 0) {
                    base.OnError(new Exception.ConflictException(error));
                }
                else {
                    base.OnError(error);
                }
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator AcquireBigItemByUserId(
                Request.AcquireBigItemByUserIdRequest request,
                UnityAction<AsyncResult<Result.AcquireBigItemByUserIdResult>> callback
        )
		{
			var task = new AcquireBigItemByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.AcquireBigItemByUserIdResult>(task.Result, task.Error));
        }

		public IFuture<Result.AcquireBigItemByUserIdResult> AcquireBigItemByUserIdFuture(
                Request.AcquireBigItemByUserIdRequest request
        )
		{
			return new AcquireBigItemByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.AcquireBigItemByUserIdResult> AcquireBigItemByUserIdAsync(
                Request.AcquireBigItemByUserIdRequest request
        )
		{
            AsyncResult<Result.AcquireBigItemByUserIdResult> result = null;
			await AcquireBigItemByUserId(
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
		public AcquireBigItemByUserIdTask AcquireBigItemByUserIdAsync(
                Request.AcquireBigItemByUserIdRequest request
        )
		{
			return new AcquireBigItemByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.AcquireBigItemByUserIdResult> AcquireBigItemByUserIdAsync(
                Request.AcquireBigItemByUserIdRequest request
        )
		{
			var task = new AcquireBigItemByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class ConsumeBigItemTask : Gs2RestSessionTask<ConsumeBigItemRequest, ConsumeBigItemResult>
        {
            public ConsumeBigItemTask(IGs2Session session, RestSessionRequestFactory factory, ConsumeBigItemRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(ConsumeBigItemRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "inventory")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/me/big/inventory/{inventoryName}/item/{itemName}/consume";

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
                    jsonWriter.Write(request.ConsumeCount);
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

            public override void OnError(Gs2.Core.Exception.Gs2Exception error)
            {
                if (error.Errors.Count(v => v.code == "itemSet.operation.conflict") > 0) {
                    base.OnError(new Exception.ConflictException(error));
                }
                else if (error.Errors.Count(v => v.code == "itemSet.count.insufficient") > 0) {
                    base.OnError(new Exception.InsufficientException(error));
                }
                else {
                    base.OnError(error);
                }
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator ConsumeBigItem(
                Request.ConsumeBigItemRequest request,
                UnityAction<AsyncResult<Result.ConsumeBigItemResult>> callback
        )
		{
			var task = new ConsumeBigItemTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.ConsumeBigItemResult>(task.Result, task.Error));
        }

		public IFuture<Result.ConsumeBigItemResult> ConsumeBigItemFuture(
                Request.ConsumeBigItemRequest request
        )
		{
			return new ConsumeBigItemTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.ConsumeBigItemResult> ConsumeBigItemAsync(
                Request.ConsumeBigItemRequest request
        )
		{
            AsyncResult<Result.ConsumeBigItemResult> result = null;
			await ConsumeBigItem(
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
		public ConsumeBigItemTask ConsumeBigItemAsync(
                Request.ConsumeBigItemRequest request
        )
		{
			return new ConsumeBigItemTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.ConsumeBigItemResult> ConsumeBigItemAsync(
                Request.ConsumeBigItemRequest request
        )
		{
			var task = new ConsumeBigItemTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class ConsumeBigItemByUserIdTask : Gs2RestSessionTask<ConsumeBigItemByUserIdRequest, ConsumeBigItemByUserIdResult>
        {
            public ConsumeBigItemByUserIdTask(IGs2Session session, RestSessionRequestFactory factory, ConsumeBigItemByUserIdRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(ConsumeBigItemByUserIdRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "inventory")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/{userId}/big/inventory/{inventoryName}/item/{itemName}/consume";

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
                    jsonWriter.Write(request.ConsumeCount);
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

            public override void OnError(Gs2.Core.Exception.Gs2Exception error)
            {
                if (error.Errors.Count(v => v.code == "itemSet.operation.conflict") > 0) {
                    base.OnError(new Exception.ConflictException(error));
                }
                else if (error.Errors.Count(v => v.code == "itemSet.count.insufficient") > 0) {
                    base.OnError(new Exception.InsufficientException(error));
                }
                else {
                    base.OnError(error);
                }
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator ConsumeBigItemByUserId(
                Request.ConsumeBigItemByUserIdRequest request,
                UnityAction<AsyncResult<Result.ConsumeBigItemByUserIdResult>> callback
        )
		{
			var task = new ConsumeBigItemByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.ConsumeBigItemByUserIdResult>(task.Result, task.Error));
        }

		public IFuture<Result.ConsumeBigItemByUserIdResult> ConsumeBigItemByUserIdFuture(
                Request.ConsumeBigItemByUserIdRequest request
        )
		{
			return new ConsumeBigItemByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.ConsumeBigItemByUserIdResult> ConsumeBigItemByUserIdAsync(
                Request.ConsumeBigItemByUserIdRequest request
        )
		{
            AsyncResult<Result.ConsumeBigItemByUserIdResult> result = null;
			await ConsumeBigItemByUserId(
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
		public ConsumeBigItemByUserIdTask ConsumeBigItemByUserIdAsync(
                Request.ConsumeBigItemByUserIdRequest request
        )
		{
			return new ConsumeBigItemByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.ConsumeBigItemByUserIdResult> ConsumeBigItemByUserIdAsync(
                Request.ConsumeBigItemByUserIdRequest request
        )
		{
			var task = new ConsumeBigItemByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class SetBigItemByUserIdTask : Gs2RestSessionTask<SetBigItemByUserIdRequest, SetBigItemByUserIdResult>
        {
            public SetBigItemByUserIdTask(IGs2Session session, RestSessionRequestFactory factory, SetBigItemByUserIdRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(SetBigItemByUserIdRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "inventory")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/{userId}/big/inventory/{inventoryName}/item/{itemName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{inventoryName}", !string.IsNullOrEmpty(request.InventoryName) ? request.InventoryName.ToString() : "null");
                url = url.Replace("{userId}", !string.IsNullOrEmpty(request.UserId) ? request.UserId.ToString() : "null");
                url = url.Replace("{itemName}", !string.IsNullOrEmpty(request.ItemName) ? request.ItemName.ToString() : "null");

                var sessionRequest = Factory.Put(url);

                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);
                jsonWriter.WriteObjectStart();
                if (request.Count != null)
                {
                    jsonWriter.WritePropertyName("count");
                    jsonWriter.Write(request.Count);
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

            public override void OnError(Gs2.Core.Exception.Gs2Exception error)
            {
                if (error.Errors.Count(v => v.code == "itemSet.operation.conflict") > 0) {
                    base.OnError(new Exception.ConflictException(error));
                }
                else {
                    base.OnError(error);
                }
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator SetBigItemByUserId(
                Request.SetBigItemByUserIdRequest request,
                UnityAction<AsyncResult<Result.SetBigItemByUserIdResult>> callback
        )
		{
			var task = new SetBigItemByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.SetBigItemByUserIdResult>(task.Result, task.Error));
        }

		public IFuture<Result.SetBigItemByUserIdResult> SetBigItemByUserIdFuture(
                Request.SetBigItemByUserIdRequest request
        )
		{
			return new SetBigItemByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.SetBigItemByUserIdResult> SetBigItemByUserIdAsync(
                Request.SetBigItemByUserIdRequest request
        )
		{
            AsyncResult<Result.SetBigItemByUserIdResult> result = null;
			await SetBigItemByUserId(
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
		public SetBigItemByUserIdTask SetBigItemByUserIdAsync(
                Request.SetBigItemByUserIdRequest request
        )
		{
			return new SetBigItemByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.SetBigItemByUserIdResult> SetBigItemByUserIdAsync(
                Request.SetBigItemByUserIdRequest request
        )
		{
			var task = new SetBigItemByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class DeleteBigItemByUserIdTask : Gs2RestSessionTask<DeleteBigItemByUserIdRequest, DeleteBigItemByUserIdResult>
        {
            public DeleteBigItemByUserIdTask(IGs2Session session, RestSessionRequestFactory factory, DeleteBigItemByUserIdRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DeleteBigItemByUserIdRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "inventory")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/{userId}/big/inventory/{inventoryName}/item/{itemName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{inventoryName}", !string.IsNullOrEmpty(request.InventoryName) ? request.InventoryName.ToString() : "null");
                url = url.Replace("{userId}", !string.IsNullOrEmpty(request.UserId) ? request.UserId.ToString() : "null");
                url = url.Replace("{itemName}", !string.IsNullOrEmpty(request.ItemName) ? request.ItemName.ToString() : "null");

                var sessionRequest = Factory.Delete(url);
                if (request.ContextStack != null)
                {
                    sessionRequest.AddQueryString("contextStack", request.ContextStack);
                }
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
		public IEnumerator DeleteBigItemByUserId(
                Request.DeleteBigItemByUserIdRequest request,
                UnityAction<AsyncResult<Result.DeleteBigItemByUserIdResult>> callback
        )
		{
			var task = new DeleteBigItemByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.DeleteBigItemByUserIdResult>(task.Result, task.Error));
        }

		public IFuture<Result.DeleteBigItemByUserIdResult> DeleteBigItemByUserIdFuture(
                Request.DeleteBigItemByUserIdRequest request
        )
		{
			return new DeleteBigItemByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.DeleteBigItemByUserIdResult> DeleteBigItemByUserIdAsync(
                Request.DeleteBigItemByUserIdRequest request
        )
		{
            AsyncResult<Result.DeleteBigItemByUserIdResult> result = null;
			await DeleteBigItemByUserId(
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
		public DeleteBigItemByUserIdTask DeleteBigItemByUserIdAsync(
                Request.DeleteBigItemByUserIdRequest request
        )
		{
			return new DeleteBigItemByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.DeleteBigItemByUserIdResult> DeleteBigItemByUserIdAsync(
                Request.DeleteBigItemByUserIdRequest request
        )
		{
			var task = new DeleteBigItemByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class VerifyBigItemTask : Gs2RestSessionTask<VerifyBigItemRequest, VerifyBigItemResult>
        {
            public VerifyBigItemTask(IGs2Session session, RestSessionRequestFactory factory, VerifyBigItemRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(VerifyBigItemRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "inventory")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/me/big/inventory/{inventoryName}/item/{itemName}/verify/{verifyType}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{inventoryName}", !string.IsNullOrEmpty(request.InventoryName) ? request.InventoryName.ToString() : "null");
                url = url.Replace("{itemName}", !string.IsNullOrEmpty(request.ItemName) ? request.ItemName.ToString() : "null");
                url = url.Replace("{verifyType}", !string.IsNullOrEmpty(request.VerifyType) ? request.VerifyType.ToString() : "null");

                var sessionRequest = Factory.Post(url);

                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);
                jsonWriter.WriteObjectStart();
                if (request.Count != null)
                {
                    jsonWriter.WritePropertyName("count");
                    jsonWriter.Write(request.Count);
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
		public IEnumerator VerifyBigItem(
                Request.VerifyBigItemRequest request,
                UnityAction<AsyncResult<Result.VerifyBigItemResult>> callback
        )
		{
			var task = new VerifyBigItemTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.VerifyBigItemResult>(task.Result, task.Error));
        }

		public IFuture<Result.VerifyBigItemResult> VerifyBigItemFuture(
                Request.VerifyBigItemRequest request
        )
		{
			return new VerifyBigItemTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.VerifyBigItemResult> VerifyBigItemAsync(
                Request.VerifyBigItemRequest request
        )
		{
            AsyncResult<Result.VerifyBigItemResult> result = null;
			await VerifyBigItem(
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
		public VerifyBigItemTask VerifyBigItemAsync(
                Request.VerifyBigItemRequest request
        )
		{
			return new VerifyBigItemTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.VerifyBigItemResult> VerifyBigItemAsync(
                Request.VerifyBigItemRequest request
        )
		{
			var task = new VerifyBigItemTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class VerifyBigItemByUserIdTask : Gs2RestSessionTask<VerifyBigItemByUserIdRequest, VerifyBigItemByUserIdResult>
        {
            public VerifyBigItemByUserIdTask(IGs2Session session, RestSessionRequestFactory factory, VerifyBigItemByUserIdRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(VerifyBigItemByUserIdRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "inventory")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/{userId}/big/inventory/{inventoryName}/item/{itemName}/verify/{verifyType}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{userId}", !string.IsNullOrEmpty(request.UserId) ? request.UserId.ToString() : "null");
                url = url.Replace("{inventoryName}", !string.IsNullOrEmpty(request.InventoryName) ? request.InventoryName.ToString() : "null");
                url = url.Replace("{itemName}", !string.IsNullOrEmpty(request.ItemName) ? request.ItemName.ToString() : "null");
                url = url.Replace("{verifyType}", !string.IsNullOrEmpty(request.VerifyType) ? request.VerifyType.ToString() : "null");

                var sessionRequest = Factory.Post(url);

                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);
                jsonWriter.WriteObjectStart();
                if (request.Count != null)
                {
                    jsonWriter.WritePropertyName("count");
                    jsonWriter.Write(request.Count);
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
		public IEnumerator VerifyBigItemByUserId(
                Request.VerifyBigItemByUserIdRequest request,
                UnityAction<AsyncResult<Result.VerifyBigItemByUserIdResult>> callback
        )
		{
			var task = new VerifyBigItemByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.VerifyBigItemByUserIdResult>(task.Result, task.Error));
        }

		public IFuture<Result.VerifyBigItemByUserIdResult> VerifyBigItemByUserIdFuture(
                Request.VerifyBigItemByUserIdRequest request
        )
		{
			return new VerifyBigItemByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.VerifyBigItemByUserIdResult> VerifyBigItemByUserIdAsync(
                Request.VerifyBigItemByUserIdRequest request
        )
		{
            AsyncResult<Result.VerifyBigItemByUserIdResult> result = null;
			await VerifyBigItemByUserId(
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
		public VerifyBigItemByUserIdTask VerifyBigItemByUserIdAsync(
                Request.VerifyBigItemByUserIdRequest request
        )
		{
			return new VerifyBigItemByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.VerifyBigItemByUserIdResult> VerifyBigItemByUserIdAsync(
                Request.VerifyBigItemByUserIdRequest request
        )
		{
			var task = new VerifyBigItemByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class AcquireBigItemByStampSheetTask : Gs2RestSessionTask<AcquireBigItemByStampSheetRequest, AcquireBigItemByStampSheetResult>
        {
            public AcquireBigItemByStampSheetTask(IGs2Session session, RestSessionRequestFactory factory, AcquireBigItemByStampSheetRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(AcquireBigItemByStampSheetRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "inventory")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/stamp/big/item/acquire";

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
		public IEnumerator AcquireBigItemByStampSheet(
                Request.AcquireBigItemByStampSheetRequest request,
                UnityAction<AsyncResult<Result.AcquireBigItemByStampSheetResult>> callback
        )
		{
			var task = new AcquireBigItemByStampSheetTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.AcquireBigItemByStampSheetResult>(task.Result, task.Error));
        }

		public IFuture<Result.AcquireBigItemByStampSheetResult> AcquireBigItemByStampSheetFuture(
                Request.AcquireBigItemByStampSheetRequest request
        )
		{
			return new AcquireBigItemByStampSheetTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.AcquireBigItemByStampSheetResult> AcquireBigItemByStampSheetAsync(
                Request.AcquireBigItemByStampSheetRequest request
        )
		{
            AsyncResult<Result.AcquireBigItemByStampSheetResult> result = null;
			await AcquireBigItemByStampSheet(
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
		public AcquireBigItemByStampSheetTask AcquireBigItemByStampSheetAsync(
                Request.AcquireBigItemByStampSheetRequest request
        )
		{
			return new AcquireBigItemByStampSheetTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.AcquireBigItemByStampSheetResult> AcquireBigItemByStampSheetAsync(
                Request.AcquireBigItemByStampSheetRequest request
        )
		{
			var task = new AcquireBigItemByStampSheetTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class ConsumeBigItemByStampTaskTask : Gs2RestSessionTask<ConsumeBigItemByStampTaskRequest, ConsumeBigItemByStampTaskResult>
        {
            public ConsumeBigItemByStampTaskTask(IGs2Session session, RestSessionRequestFactory factory, ConsumeBigItemByStampTaskRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(ConsumeBigItemByStampTaskRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "inventory")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/stamp/big/item/consume";

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
		public IEnumerator ConsumeBigItemByStampTask(
                Request.ConsumeBigItemByStampTaskRequest request,
                UnityAction<AsyncResult<Result.ConsumeBigItemByStampTaskResult>> callback
        )
		{
			var task = new ConsumeBigItemByStampTaskTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.ConsumeBigItemByStampTaskResult>(task.Result, task.Error));
        }

		public IFuture<Result.ConsumeBigItemByStampTaskResult> ConsumeBigItemByStampTaskFuture(
                Request.ConsumeBigItemByStampTaskRequest request
        )
		{
			return new ConsumeBigItemByStampTaskTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.ConsumeBigItemByStampTaskResult> ConsumeBigItemByStampTaskAsync(
                Request.ConsumeBigItemByStampTaskRequest request
        )
		{
            AsyncResult<Result.ConsumeBigItemByStampTaskResult> result = null;
			await ConsumeBigItemByStampTask(
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
		public ConsumeBigItemByStampTaskTask ConsumeBigItemByStampTaskAsync(
                Request.ConsumeBigItemByStampTaskRequest request
        )
		{
			return new ConsumeBigItemByStampTaskTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.ConsumeBigItemByStampTaskResult> ConsumeBigItemByStampTaskAsync(
                Request.ConsumeBigItemByStampTaskRequest request
        )
		{
			var task = new ConsumeBigItemByStampTaskTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class SetBigItemByStampSheetTask : Gs2RestSessionTask<SetBigItemByStampSheetRequest, SetBigItemByStampSheetResult>
        {
            public SetBigItemByStampSheetTask(IGs2Session session, RestSessionRequestFactory factory, SetBigItemByStampSheetRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(SetBigItemByStampSheetRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "inventory")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/stamp/big/item/set";

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
		public IEnumerator SetBigItemByStampSheet(
                Request.SetBigItemByStampSheetRequest request,
                UnityAction<AsyncResult<Result.SetBigItemByStampSheetResult>> callback
        )
		{
			var task = new SetBigItemByStampSheetTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.SetBigItemByStampSheetResult>(task.Result, task.Error));
        }

		public IFuture<Result.SetBigItemByStampSheetResult> SetBigItemByStampSheetFuture(
                Request.SetBigItemByStampSheetRequest request
        )
		{
			return new SetBigItemByStampSheetTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.SetBigItemByStampSheetResult> SetBigItemByStampSheetAsync(
                Request.SetBigItemByStampSheetRequest request
        )
		{
            AsyncResult<Result.SetBigItemByStampSheetResult> result = null;
			await SetBigItemByStampSheet(
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
		public SetBigItemByStampSheetTask SetBigItemByStampSheetAsync(
                Request.SetBigItemByStampSheetRequest request
        )
		{
			return new SetBigItemByStampSheetTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.SetBigItemByStampSheetResult> SetBigItemByStampSheetAsync(
                Request.SetBigItemByStampSheetRequest request
        )
		{
			var task = new SetBigItemByStampSheetTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class VerifyBigItemByStampTaskTask : Gs2RestSessionTask<VerifyBigItemByStampTaskRequest, VerifyBigItemByStampTaskResult>
        {
            public VerifyBigItemByStampTaskTask(IGs2Session session, RestSessionRequestFactory factory, VerifyBigItemByStampTaskRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(VerifyBigItemByStampTaskRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "inventory")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/stamp/big/item/verify";

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
		public IEnumerator VerifyBigItemByStampTask(
                Request.VerifyBigItemByStampTaskRequest request,
                UnityAction<AsyncResult<Result.VerifyBigItemByStampTaskResult>> callback
        )
		{
			var task = new VerifyBigItemByStampTaskTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.VerifyBigItemByStampTaskResult>(task.Result, task.Error));
        }

		public IFuture<Result.VerifyBigItemByStampTaskResult> VerifyBigItemByStampTaskFuture(
                Request.VerifyBigItemByStampTaskRequest request
        )
		{
			return new VerifyBigItemByStampTaskTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.VerifyBigItemByStampTaskResult> VerifyBigItemByStampTaskAsync(
                Request.VerifyBigItemByStampTaskRequest request
        )
		{
            AsyncResult<Result.VerifyBigItemByStampTaskResult> result = null;
			await VerifyBigItemByStampTask(
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
		public VerifyBigItemByStampTaskTask VerifyBigItemByStampTaskAsync(
                Request.VerifyBigItemByStampTaskRequest request
        )
		{
			return new VerifyBigItemByStampTaskTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.VerifyBigItemByStampTaskResult> VerifyBigItemByStampTaskAsync(
                Request.VerifyBigItemByStampTaskRequest request
        )
		{
			var task = new VerifyBigItemByStampTaskTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif
	}
}