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

#pragma warning disable CS0618 // Obsolete with a message

#if UNITY_2017_1_OR_NEWER
using UnityEngine.Events;
using UnityEngine.Networking;
    #if GS2_ENABLE_UNITASK
using Cysharp.Threading.Tasks;
    #endif
#else
using System.Web;
using System.Net.Http;
#endif

using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using Gs2.Core;
using Gs2.Core.Model;
using Gs2.Core.Net;
using Gs2.Core.Util;
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
                if (request.NamePrefix != null) {
                    sessionRequest.AddQueryString("namePrefix", $"{request.NamePrefix}");
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
        ) =>
            new DescribeNamespacesTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.DescribeNamespacesResult> DescribeNamespacesFuture(
                Request.DescribeNamespacesRequest request
        ) =>
            new DescribeNamespacesTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.DescribeNamespacesResult> DescribeNamespacesAsync(
                Request.DescribeNamespacesRequest request
        ) =>
            new DescribeNamespacesTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.DescribeNamespacesResult>();
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
		public Task<Result.DescribeNamespacesResult> DescribeNamespacesAsync(
                Request.DescribeNamespacesRequest request
        ) =>
            new DescribeNamespacesTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
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
                if (request.TransactionSetting != null)
                {
                    jsonWriter.WritePropertyName("transactionSetting");
                    request.TransactionSetting.WriteJson(jsonWriter);
                }
                if (request.TransactionSettingV2 != null)
                {
                    jsonWriter.WritePropertyName("transactionSettingV2");
                    request.TransactionSettingV2.WriteJson(jsonWriter);
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
        ) =>
            new CreateNamespaceTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.CreateNamespaceResult> CreateNamespaceFuture(
                Request.CreateNamespaceRequest request
        ) =>
            new CreateNamespaceTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.CreateNamespaceResult> CreateNamespaceAsync(
                Request.CreateNamespaceRequest request
        ) =>
            new CreateNamespaceTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.CreateNamespaceResult>();
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
		public Task<Result.CreateNamespaceResult> CreateNamespaceAsync(
                Request.CreateNamespaceRequest request
        ) =>
            new CreateNamespaceTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
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
        ) =>
            new GetNamespaceStatusTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.GetNamespaceStatusResult> GetNamespaceStatusFuture(
                Request.GetNamespaceStatusRequest request
        ) =>
            new GetNamespaceStatusTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.GetNamespaceStatusResult> GetNamespaceStatusAsync(
                Request.GetNamespaceStatusRequest request
        ) =>
            new GetNamespaceStatusTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.GetNamespaceStatusResult>();
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
		public Task<Result.GetNamespaceStatusResult> GetNamespaceStatusAsync(
                Request.GetNamespaceStatusRequest request
        ) =>
            new GetNamespaceStatusTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
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
        ) =>
            new GetNamespaceTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.GetNamespaceResult> GetNamespaceFuture(
                Request.GetNamespaceRequest request
        ) =>
            new GetNamespaceTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.GetNamespaceResult> GetNamespaceAsync(
                Request.GetNamespaceRequest request
        ) =>
            new GetNamespaceTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.GetNamespaceResult>();
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
		public Task<Result.GetNamespaceResult> GetNamespaceAsync(
                Request.GetNamespaceRequest request
        ) =>
            new GetNamespaceTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
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
                if (request.TransactionSetting != null)
                {
                    jsonWriter.WritePropertyName("transactionSetting");
                    request.TransactionSetting.WriteJson(jsonWriter);
                }
                if (request.TransactionSettingV2 != null)
                {
                    jsonWriter.WritePropertyName("transactionSettingV2");
                    request.TransactionSettingV2.WriteJson(jsonWriter);
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
        ) =>
            new UpdateNamespaceTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.UpdateNamespaceResult> UpdateNamespaceFuture(
                Request.UpdateNamespaceRequest request
        ) =>
            new UpdateNamespaceTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.UpdateNamespaceResult> UpdateNamespaceAsync(
                Request.UpdateNamespaceRequest request
        ) =>
            new UpdateNamespaceTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.UpdateNamespaceResult>();
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
		public Task<Result.UpdateNamespaceResult> UpdateNamespaceAsync(
                Request.UpdateNamespaceRequest request
        ) =>
            new UpdateNamespaceTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
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
        ) =>
            new DeleteNamespaceTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.DeleteNamespaceResult> DeleteNamespaceFuture(
                Request.DeleteNamespaceRequest request
        ) =>
            new DeleteNamespaceTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.DeleteNamespaceResult> DeleteNamespaceAsync(
                Request.DeleteNamespaceRequest request
        ) =>
            new DeleteNamespaceTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.DeleteNamespaceResult>();
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
		public Task<Result.DeleteNamespaceResult> DeleteNamespaceAsync(
                Request.DeleteNamespaceRequest request
        ) =>
            new DeleteNamespaceTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class GetServiceVersionTask : Gs2RestSessionTask<GetServiceVersionRequest, GetServiceVersionResult>
        {
            public GetServiceVersionTask(IGs2Session session, RestSessionRequestFactory factory, GetServiceVersionRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(GetServiceVersionRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "inventory")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/system/version";

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
		public IEnumerator GetServiceVersion(
                Request.GetServiceVersionRequest request,
                UnityAction<AsyncResult<Result.GetServiceVersionResult>> callback
        ) =>
            new GetServiceVersionTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.GetServiceVersionResult> GetServiceVersionFuture(
                Request.GetServiceVersionRequest request
        ) =>
            new GetServiceVersionTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.GetServiceVersionResult> GetServiceVersionAsync(
                Request.GetServiceVersionRequest request
        ) =>
            new GetServiceVersionTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.GetServiceVersionResult>();
    #else
		public GetServiceVersionTask GetServiceVersionAsync(
                Request.GetServiceVersionRequest request
        )
		{
			return new GetServiceVersionTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.GetServiceVersionResult> GetServiceVersionAsync(
                Request.GetServiceVersionRequest request
        ) =>
            new GetServiceVersionTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
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
        ) =>
            new DumpUserDataByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.DumpUserDataByUserIdResult> DumpUserDataByUserIdFuture(
                Request.DumpUserDataByUserIdRequest request
        ) =>
            new DumpUserDataByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.DumpUserDataByUserIdResult> DumpUserDataByUserIdAsync(
                Request.DumpUserDataByUserIdRequest request
        ) =>
            new DumpUserDataByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.DumpUserDataByUserIdResult>();
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
		public Task<Result.DumpUserDataByUserIdResult> DumpUserDataByUserIdAsync(
                Request.DumpUserDataByUserIdRequest request
        ) =>
            new DumpUserDataByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
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
        ) =>
            new CheckDumpUserDataByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.CheckDumpUserDataByUserIdResult> CheckDumpUserDataByUserIdFuture(
                Request.CheckDumpUserDataByUserIdRequest request
        ) =>
            new CheckDumpUserDataByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.CheckDumpUserDataByUserIdResult> CheckDumpUserDataByUserIdAsync(
                Request.CheckDumpUserDataByUserIdRequest request
        ) =>
            new CheckDumpUserDataByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.CheckDumpUserDataByUserIdResult>();
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
		public Task<Result.CheckDumpUserDataByUserIdResult> CheckDumpUserDataByUserIdAsync(
                Request.CheckDumpUserDataByUserIdRequest request
        ) =>
            new CheckDumpUserDataByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
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
        ) =>
            new CleanUserDataByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.CleanUserDataByUserIdResult> CleanUserDataByUserIdFuture(
                Request.CleanUserDataByUserIdRequest request
        ) =>
            new CleanUserDataByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.CleanUserDataByUserIdResult> CleanUserDataByUserIdAsync(
                Request.CleanUserDataByUserIdRequest request
        ) =>
            new CleanUserDataByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.CleanUserDataByUserIdResult>();
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
		public Task<Result.CleanUserDataByUserIdResult> CleanUserDataByUserIdAsync(
                Request.CleanUserDataByUserIdRequest request
        ) =>
            new CleanUserDataByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
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
        ) =>
            new CheckCleanUserDataByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.CheckCleanUserDataByUserIdResult> CheckCleanUserDataByUserIdFuture(
                Request.CheckCleanUserDataByUserIdRequest request
        ) =>
            new CheckCleanUserDataByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.CheckCleanUserDataByUserIdResult> CheckCleanUserDataByUserIdAsync(
                Request.CheckCleanUserDataByUserIdRequest request
        ) =>
            new CheckCleanUserDataByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.CheckCleanUserDataByUserIdResult>();
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
		public Task<Result.CheckCleanUserDataByUserIdResult> CheckCleanUserDataByUserIdAsync(
                Request.CheckCleanUserDataByUserIdRequest request
        ) =>
            new CheckCleanUserDataByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
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
        ) =>
            new PrepareImportUserDataByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.PrepareImportUserDataByUserIdResult> PrepareImportUserDataByUserIdFuture(
                Request.PrepareImportUserDataByUserIdRequest request
        ) =>
            new PrepareImportUserDataByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.PrepareImportUserDataByUserIdResult> PrepareImportUserDataByUserIdAsync(
                Request.PrepareImportUserDataByUserIdRequest request
        ) =>
            new PrepareImportUserDataByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.PrepareImportUserDataByUserIdResult>();
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
		public Task<Result.PrepareImportUserDataByUserIdResult> PrepareImportUserDataByUserIdAsync(
                Request.PrepareImportUserDataByUserIdRequest request
        ) =>
            new PrepareImportUserDataByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
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
        ) =>
            new ImportUserDataByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.ImportUserDataByUserIdResult> ImportUserDataByUserIdFuture(
                Request.ImportUserDataByUserIdRequest request
        ) =>
            new ImportUserDataByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.ImportUserDataByUserIdResult> ImportUserDataByUserIdAsync(
                Request.ImportUserDataByUserIdRequest request
        ) =>
            new ImportUserDataByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.ImportUserDataByUserIdResult>();
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
		public Task<Result.ImportUserDataByUserIdResult> ImportUserDataByUserIdAsync(
                Request.ImportUserDataByUserIdRequest request
        ) =>
            new ImportUserDataByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
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
        ) =>
            new CheckImportUserDataByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.CheckImportUserDataByUserIdResult> CheckImportUserDataByUserIdFuture(
                Request.CheckImportUserDataByUserIdRequest request
        ) =>
            new CheckImportUserDataByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.CheckImportUserDataByUserIdResult> CheckImportUserDataByUserIdAsync(
                Request.CheckImportUserDataByUserIdRequest request
        ) =>
            new CheckImportUserDataByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.CheckImportUserDataByUserIdResult>();
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
		public Task<Result.CheckImportUserDataByUserIdResult> CheckImportUserDataByUserIdAsync(
                Request.CheckImportUserDataByUserIdRequest request
        ) =>
            new CheckImportUserDataByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
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
                if (request.NamePrefix != null) {
                    sessionRequest.AddQueryString("namePrefix", $"{request.NamePrefix}");
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
        ) =>
            new DescribeInventoryModelMastersTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.DescribeInventoryModelMastersResult> DescribeInventoryModelMastersFuture(
                Request.DescribeInventoryModelMastersRequest request
        ) =>
            new DescribeInventoryModelMastersTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.DescribeInventoryModelMastersResult> DescribeInventoryModelMastersAsync(
                Request.DescribeInventoryModelMastersRequest request
        ) =>
            new DescribeInventoryModelMastersTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.DescribeInventoryModelMastersResult>();
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
		public Task<Result.DescribeInventoryModelMastersResult> DescribeInventoryModelMastersAsync(
                Request.DescribeInventoryModelMastersRequest request
        ) =>
            new DescribeInventoryModelMastersTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
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
        ) =>
            new CreateInventoryModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.CreateInventoryModelMasterResult> CreateInventoryModelMasterFuture(
                Request.CreateInventoryModelMasterRequest request
        ) =>
            new CreateInventoryModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.CreateInventoryModelMasterResult> CreateInventoryModelMasterAsync(
                Request.CreateInventoryModelMasterRequest request
        ) =>
            new CreateInventoryModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.CreateInventoryModelMasterResult>();
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
		public Task<Result.CreateInventoryModelMasterResult> CreateInventoryModelMasterAsync(
                Request.CreateInventoryModelMasterRequest request
        ) =>
            new CreateInventoryModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
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
        ) =>
            new GetInventoryModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.GetInventoryModelMasterResult> GetInventoryModelMasterFuture(
                Request.GetInventoryModelMasterRequest request
        ) =>
            new GetInventoryModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.GetInventoryModelMasterResult> GetInventoryModelMasterAsync(
                Request.GetInventoryModelMasterRequest request
        ) =>
            new GetInventoryModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.GetInventoryModelMasterResult>();
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
		public Task<Result.GetInventoryModelMasterResult> GetInventoryModelMasterAsync(
                Request.GetInventoryModelMasterRequest request
        ) =>
            new GetInventoryModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
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
        ) =>
            new UpdateInventoryModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.UpdateInventoryModelMasterResult> UpdateInventoryModelMasterFuture(
                Request.UpdateInventoryModelMasterRequest request
        ) =>
            new UpdateInventoryModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.UpdateInventoryModelMasterResult> UpdateInventoryModelMasterAsync(
                Request.UpdateInventoryModelMasterRequest request
        ) =>
            new UpdateInventoryModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.UpdateInventoryModelMasterResult>();
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
		public Task<Result.UpdateInventoryModelMasterResult> UpdateInventoryModelMasterAsync(
                Request.UpdateInventoryModelMasterRequest request
        ) =>
            new UpdateInventoryModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
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
        ) =>
            new DeleteInventoryModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.DeleteInventoryModelMasterResult> DeleteInventoryModelMasterFuture(
                Request.DeleteInventoryModelMasterRequest request
        ) =>
            new DeleteInventoryModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.DeleteInventoryModelMasterResult> DeleteInventoryModelMasterAsync(
                Request.DeleteInventoryModelMasterRequest request
        ) =>
            new DeleteInventoryModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.DeleteInventoryModelMasterResult>();
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
		public Task<Result.DeleteInventoryModelMasterResult> DeleteInventoryModelMasterAsync(
                Request.DeleteInventoryModelMasterRequest request
        ) =>
            new DeleteInventoryModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
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
        ) =>
            new DescribeInventoryModelsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.DescribeInventoryModelsResult> DescribeInventoryModelsFuture(
                Request.DescribeInventoryModelsRequest request
        ) =>
            new DescribeInventoryModelsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.DescribeInventoryModelsResult> DescribeInventoryModelsAsync(
                Request.DescribeInventoryModelsRequest request
        ) =>
            new DescribeInventoryModelsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.DescribeInventoryModelsResult>();
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
		public Task<Result.DescribeInventoryModelsResult> DescribeInventoryModelsAsync(
                Request.DescribeInventoryModelsRequest request
        ) =>
            new DescribeInventoryModelsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
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
        ) =>
            new GetInventoryModelTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.GetInventoryModelResult> GetInventoryModelFuture(
                Request.GetInventoryModelRequest request
        ) =>
            new GetInventoryModelTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.GetInventoryModelResult> GetInventoryModelAsync(
                Request.GetInventoryModelRequest request
        ) =>
            new GetInventoryModelTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.GetInventoryModelResult>();
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
		public Task<Result.GetInventoryModelResult> GetInventoryModelAsync(
                Request.GetInventoryModelRequest request
        ) =>
            new GetInventoryModelTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
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
        ) =>
            new DescribeItemModelMastersTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.DescribeItemModelMastersResult> DescribeItemModelMastersFuture(
                Request.DescribeItemModelMastersRequest request
        ) =>
            new DescribeItemModelMastersTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.DescribeItemModelMastersResult> DescribeItemModelMastersAsync(
                Request.DescribeItemModelMastersRequest request
        ) =>
            new DescribeItemModelMastersTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.DescribeItemModelMastersResult>();
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
		public Task<Result.DescribeItemModelMastersResult> DescribeItemModelMastersAsync(
                Request.DescribeItemModelMastersRequest request
        ) =>
            new DescribeItemModelMastersTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
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
        ) =>
            new CreateItemModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.CreateItemModelMasterResult> CreateItemModelMasterFuture(
                Request.CreateItemModelMasterRequest request
        ) =>
            new CreateItemModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.CreateItemModelMasterResult> CreateItemModelMasterAsync(
                Request.CreateItemModelMasterRequest request
        ) =>
            new CreateItemModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.CreateItemModelMasterResult>();
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
		public Task<Result.CreateItemModelMasterResult> CreateItemModelMasterAsync(
                Request.CreateItemModelMasterRequest request
        ) =>
            new CreateItemModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
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
        ) =>
            new GetItemModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.GetItemModelMasterResult> GetItemModelMasterFuture(
                Request.GetItemModelMasterRequest request
        ) =>
            new GetItemModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.GetItemModelMasterResult> GetItemModelMasterAsync(
                Request.GetItemModelMasterRequest request
        ) =>
            new GetItemModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.GetItemModelMasterResult>();
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
		public Task<Result.GetItemModelMasterResult> GetItemModelMasterAsync(
                Request.GetItemModelMasterRequest request
        ) =>
            new GetItemModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
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
        ) =>
            new UpdateItemModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.UpdateItemModelMasterResult> UpdateItemModelMasterFuture(
                Request.UpdateItemModelMasterRequest request
        ) =>
            new UpdateItemModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.UpdateItemModelMasterResult> UpdateItemModelMasterAsync(
                Request.UpdateItemModelMasterRequest request
        ) =>
            new UpdateItemModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.UpdateItemModelMasterResult>();
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
		public Task<Result.UpdateItemModelMasterResult> UpdateItemModelMasterAsync(
                Request.UpdateItemModelMasterRequest request
        ) =>
            new UpdateItemModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
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
        ) =>
            new DeleteItemModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.DeleteItemModelMasterResult> DeleteItemModelMasterFuture(
                Request.DeleteItemModelMasterRequest request
        ) =>
            new DeleteItemModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.DeleteItemModelMasterResult> DeleteItemModelMasterAsync(
                Request.DeleteItemModelMasterRequest request
        ) =>
            new DeleteItemModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.DeleteItemModelMasterResult>();
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
		public Task<Result.DeleteItemModelMasterResult> DeleteItemModelMasterAsync(
                Request.DeleteItemModelMasterRequest request
        ) =>
            new DeleteItemModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
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
        ) =>
            new DescribeItemModelsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.DescribeItemModelsResult> DescribeItemModelsFuture(
                Request.DescribeItemModelsRequest request
        ) =>
            new DescribeItemModelsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.DescribeItemModelsResult> DescribeItemModelsAsync(
                Request.DescribeItemModelsRequest request
        ) =>
            new DescribeItemModelsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.DescribeItemModelsResult>();
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
		public Task<Result.DescribeItemModelsResult> DescribeItemModelsAsync(
                Request.DescribeItemModelsRequest request
        ) =>
            new DescribeItemModelsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
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
        ) =>
            new GetItemModelTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.GetItemModelResult> GetItemModelFuture(
                Request.GetItemModelRequest request
        ) =>
            new GetItemModelTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.GetItemModelResult> GetItemModelAsync(
                Request.GetItemModelRequest request
        ) =>
            new GetItemModelTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.GetItemModelResult>();
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
		public Task<Result.GetItemModelResult> GetItemModelAsync(
                Request.GetItemModelRequest request
        ) =>
            new GetItemModelTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
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
                if (request.NamePrefix != null) {
                    sessionRequest.AddQueryString("namePrefix", $"{request.NamePrefix}");
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
        ) =>
            new DescribeSimpleInventoryModelMastersTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.DescribeSimpleInventoryModelMastersResult> DescribeSimpleInventoryModelMastersFuture(
                Request.DescribeSimpleInventoryModelMastersRequest request
        ) =>
            new DescribeSimpleInventoryModelMastersTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.DescribeSimpleInventoryModelMastersResult> DescribeSimpleInventoryModelMastersAsync(
                Request.DescribeSimpleInventoryModelMastersRequest request
        ) =>
            new DescribeSimpleInventoryModelMastersTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.DescribeSimpleInventoryModelMastersResult>();
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
		public Task<Result.DescribeSimpleInventoryModelMastersResult> DescribeSimpleInventoryModelMastersAsync(
                Request.DescribeSimpleInventoryModelMastersRequest request
        ) =>
            new DescribeSimpleInventoryModelMastersTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
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
        ) =>
            new CreateSimpleInventoryModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.CreateSimpleInventoryModelMasterResult> CreateSimpleInventoryModelMasterFuture(
                Request.CreateSimpleInventoryModelMasterRequest request
        ) =>
            new CreateSimpleInventoryModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.CreateSimpleInventoryModelMasterResult> CreateSimpleInventoryModelMasterAsync(
                Request.CreateSimpleInventoryModelMasterRequest request
        ) =>
            new CreateSimpleInventoryModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.CreateSimpleInventoryModelMasterResult>();
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
		public Task<Result.CreateSimpleInventoryModelMasterResult> CreateSimpleInventoryModelMasterAsync(
                Request.CreateSimpleInventoryModelMasterRequest request
        ) =>
            new CreateSimpleInventoryModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
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
        ) =>
            new GetSimpleInventoryModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.GetSimpleInventoryModelMasterResult> GetSimpleInventoryModelMasterFuture(
                Request.GetSimpleInventoryModelMasterRequest request
        ) =>
            new GetSimpleInventoryModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.GetSimpleInventoryModelMasterResult> GetSimpleInventoryModelMasterAsync(
                Request.GetSimpleInventoryModelMasterRequest request
        ) =>
            new GetSimpleInventoryModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.GetSimpleInventoryModelMasterResult>();
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
		public Task<Result.GetSimpleInventoryModelMasterResult> GetSimpleInventoryModelMasterAsync(
                Request.GetSimpleInventoryModelMasterRequest request
        ) =>
            new GetSimpleInventoryModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
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
        ) =>
            new UpdateSimpleInventoryModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.UpdateSimpleInventoryModelMasterResult> UpdateSimpleInventoryModelMasterFuture(
                Request.UpdateSimpleInventoryModelMasterRequest request
        ) =>
            new UpdateSimpleInventoryModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.UpdateSimpleInventoryModelMasterResult> UpdateSimpleInventoryModelMasterAsync(
                Request.UpdateSimpleInventoryModelMasterRequest request
        ) =>
            new UpdateSimpleInventoryModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.UpdateSimpleInventoryModelMasterResult>();
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
		public Task<Result.UpdateSimpleInventoryModelMasterResult> UpdateSimpleInventoryModelMasterAsync(
                Request.UpdateSimpleInventoryModelMasterRequest request
        ) =>
            new UpdateSimpleInventoryModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
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
        ) =>
            new DeleteSimpleInventoryModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.DeleteSimpleInventoryModelMasterResult> DeleteSimpleInventoryModelMasterFuture(
                Request.DeleteSimpleInventoryModelMasterRequest request
        ) =>
            new DeleteSimpleInventoryModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.DeleteSimpleInventoryModelMasterResult> DeleteSimpleInventoryModelMasterAsync(
                Request.DeleteSimpleInventoryModelMasterRequest request
        ) =>
            new DeleteSimpleInventoryModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.DeleteSimpleInventoryModelMasterResult>();
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
		public Task<Result.DeleteSimpleInventoryModelMasterResult> DeleteSimpleInventoryModelMasterAsync(
                Request.DeleteSimpleInventoryModelMasterRequest request
        ) =>
            new DeleteSimpleInventoryModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
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
        ) =>
            new DescribeSimpleInventoryModelsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.DescribeSimpleInventoryModelsResult> DescribeSimpleInventoryModelsFuture(
                Request.DescribeSimpleInventoryModelsRequest request
        ) =>
            new DescribeSimpleInventoryModelsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.DescribeSimpleInventoryModelsResult> DescribeSimpleInventoryModelsAsync(
                Request.DescribeSimpleInventoryModelsRequest request
        ) =>
            new DescribeSimpleInventoryModelsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.DescribeSimpleInventoryModelsResult>();
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
		public Task<Result.DescribeSimpleInventoryModelsResult> DescribeSimpleInventoryModelsAsync(
                Request.DescribeSimpleInventoryModelsRequest request
        ) =>
            new DescribeSimpleInventoryModelsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
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
        ) =>
            new GetSimpleInventoryModelTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.GetSimpleInventoryModelResult> GetSimpleInventoryModelFuture(
                Request.GetSimpleInventoryModelRequest request
        ) =>
            new GetSimpleInventoryModelTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.GetSimpleInventoryModelResult> GetSimpleInventoryModelAsync(
                Request.GetSimpleInventoryModelRequest request
        ) =>
            new GetSimpleInventoryModelTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.GetSimpleInventoryModelResult>();
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
		public Task<Result.GetSimpleInventoryModelResult> GetSimpleInventoryModelAsync(
                Request.GetSimpleInventoryModelRequest request
        ) =>
            new GetSimpleInventoryModelTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
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
                if (request.NamePrefix != null) {
                    sessionRequest.AddQueryString("namePrefix", $"{request.NamePrefix}");
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
        ) =>
            new DescribeSimpleItemModelMastersTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.DescribeSimpleItemModelMastersResult> DescribeSimpleItemModelMastersFuture(
                Request.DescribeSimpleItemModelMastersRequest request
        ) =>
            new DescribeSimpleItemModelMastersTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.DescribeSimpleItemModelMastersResult> DescribeSimpleItemModelMastersAsync(
                Request.DescribeSimpleItemModelMastersRequest request
        ) =>
            new DescribeSimpleItemModelMastersTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.DescribeSimpleItemModelMastersResult>();
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
		public Task<Result.DescribeSimpleItemModelMastersResult> DescribeSimpleItemModelMastersAsync(
                Request.DescribeSimpleItemModelMastersRequest request
        ) =>
            new DescribeSimpleItemModelMastersTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
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
        ) =>
            new CreateSimpleItemModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.CreateSimpleItemModelMasterResult> CreateSimpleItemModelMasterFuture(
                Request.CreateSimpleItemModelMasterRequest request
        ) =>
            new CreateSimpleItemModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.CreateSimpleItemModelMasterResult> CreateSimpleItemModelMasterAsync(
                Request.CreateSimpleItemModelMasterRequest request
        ) =>
            new CreateSimpleItemModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.CreateSimpleItemModelMasterResult>();
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
		public Task<Result.CreateSimpleItemModelMasterResult> CreateSimpleItemModelMasterAsync(
                Request.CreateSimpleItemModelMasterRequest request
        ) =>
            new CreateSimpleItemModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
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
        ) =>
            new GetSimpleItemModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.GetSimpleItemModelMasterResult> GetSimpleItemModelMasterFuture(
                Request.GetSimpleItemModelMasterRequest request
        ) =>
            new GetSimpleItemModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.GetSimpleItemModelMasterResult> GetSimpleItemModelMasterAsync(
                Request.GetSimpleItemModelMasterRequest request
        ) =>
            new GetSimpleItemModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.GetSimpleItemModelMasterResult>();
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
		public Task<Result.GetSimpleItemModelMasterResult> GetSimpleItemModelMasterAsync(
                Request.GetSimpleItemModelMasterRequest request
        ) =>
            new GetSimpleItemModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
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
        ) =>
            new UpdateSimpleItemModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.UpdateSimpleItemModelMasterResult> UpdateSimpleItemModelMasterFuture(
                Request.UpdateSimpleItemModelMasterRequest request
        ) =>
            new UpdateSimpleItemModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.UpdateSimpleItemModelMasterResult> UpdateSimpleItemModelMasterAsync(
                Request.UpdateSimpleItemModelMasterRequest request
        ) =>
            new UpdateSimpleItemModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.UpdateSimpleItemModelMasterResult>();
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
		public Task<Result.UpdateSimpleItemModelMasterResult> UpdateSimpleItemModelMasterAsync(
                Request.UpdateSimpleItemModelMasterRequest request
        ) =>
            new UpdateSimpleItemModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
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
        ) =>
            new DeleteSimpleItemModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.DeleteSimpleItemModelMasterResult> DeleteSimpleItemModelMasterFuture(
                Request.DeleteSimpleItemModelMasterRequest request
        ) =>
            new DeleteSimpleItemModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.DeleteSimpleItemModelMasterResult> DeleteSimpleItemModelMasterAsync(
                Request.DeleteSimpleItemModelMasterRequest request
        ) =>
            new DeleteSimpleItemModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.DeleteSimpleItemModelMasterResult>();
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
		public Task<Result.DeleteSimpleItemModelMasterResult> DeleteSimpleItemModelMasterAsync(
                Request.DeleteSimpleItemModelMasterRequest request
        ) =>
            new DeleteSimpleItemModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
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
        ) =>
            new DescribeSimpleItemModelsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.DescribeSimpleItemModelsResult> DescribeSimpleItemModelsFuture(
                Request.DescribeSimpleItemModelsRequest request
        ) =>
            new DescribeSimpleItemModelsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.DescribeSimpleItemModelsResult> DescribeSimpleItemModelsAsync(
                Request.DescribeSimpleItemModelsRequest request
        ) =>
            new DescribeSimpleItemModelsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.DescribeSimpleItemModelsResult>();
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
		public Task<Result.DescribeSimpleItemModelsResult> DescribeSimpleItemModelsAsync(
                Request.DescribeSimpleItemModelsRequest request
        ) =>
            new DescribeSimpleItemModelsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
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
        ) =>
            new GetSimpleItemModelTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.GetSimpleItemModelResult> GetSimpleItemModelFuture(
                Request.GetSimpleItemModelRequest request
        ) =>
            new GetSimpleItemModelTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.GetSimpleItemModelResult> GetSimpleItemModelAsync(
                Request.GetSimpleItemModelRequest request
        ) =>
            new GetSimpleItemModelTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.GetSimpleItemModelResult>();
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
		public Task<Result.GetSimpleItemModelResult> GetSimpleItemModelAsync(
                Request.GetSimpleItemModelRequest request
        ) =>
            new GetSimpleItemModelTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
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
                if (request.NamePrefix != null) {
                    sessionRequest.AddQueryString("namePrefix", $"{request.NamePrefix}");
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
        ) =>
            new DescribeBigInventoryModelMastersTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.DescribeBigInventoryModelMastersResult> DescribeBigInventoryModelMastersFuture(
                Request.DescribeBigInventoryModelMastersRequest request
        ) =>
            new DescribeBigInventoryModelMastersTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.DescribeBigInventoryModelMastersResult> DescribeBigInventoryModelMastersAsync(
                Request.DescribeBigInventoryModelMastersRequest request
        ) =>
            new DescribeBigInventoryModelMastersTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.DescribeBigInventoryModelMastersResult>();
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
		public Task<Result.DescribeBigInventoryModelMastersResult> DescribeBigInventoryModelMastersAsync(
                Request.DescribeBigInventoryModelMastersRequest request
        ) =>
            new DescribeBigInventoryModelMastersTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
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
        ) =>
            new CreateBigInventoryModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.CreateBigInventoryModelMasterResult> CreateBigInventoryModelMasterFuture(
                Request.CreateBigInventoryModelMasterRequest request
        ) =>
            new CreateBigInventoryModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.CreateBigInventoryModelMasterResult> CreateBigInventoryModelMasterAsync(
                Request.CreateBigInventoryModelMasterRequest request
        ) =>
            new CreateBigInventoryModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.CreateBigInventoryModelMasterResult>();
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
		public Task<Result.CreateBigInventoryModelMasterResult> CreateBigInventoryModelMasterAsync(
                Request.CreateBigInventoryModelMasterRequest request
        ) =>
            new CreateBigInventoryModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
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
        ) =>
            new GetBigInventoryModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.GetBigInventoryModelMasterResult> GetBigInventoryModelMasterFuture(
                Request.GetBigInventoryModelMasterRequest request
        ) =>
            new GetBigInventoryModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.GetBigInventoryModelMasterResult> GetBigInventoryModelMasterAsync(
                Request.GetBigInventoryModelMasterRequest request
        ) =>
            new GetBigInventoryModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.GetBigInventoryModelMasterResult>();
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
		public Task<Result.GetBigInventoryModelMasterResult> GetBigInventoryModelMasterAsync(
                Request.GetBigInventoryModelMasterRequest request
        ) =>
            new GetBigInventoryModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
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
        ) =>
            new UpdateBigInventoryModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.UpdateBigInventoryModelMasterResult> UpdateBigInventoryModelMasterFuture(
                Request.UpdateBigInventoryModelMasterRequest request
        ) =>
            new UpdateBigInventoryModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.UpdateBigInventoryModelMasterResult> UpdateBigInventoryModelMasterAsync(
                Request.UpdateBigInventoryModelMasterRequest request
        ) =>
            new UpdateBigInventoryModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.UpdateBigInventoryModelMasterResult>();
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
		public Task<Result.UpdateBigInventoryModelMasterResult> UpdateBigInventoryModelMasterAsync(
                Request.UpdateBigInventoryModelMasterRequest request
        ) =>
            new UpdateBigInventoryModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
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
        ) =>
            new DeleteBigInventoryModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.DeleteBigInventoryModelMasterResult> DeleteBigInventoryModelMasterFuture(
                Request.DeleteBigInventoryModelMasterRequest request
        ) =>
            new DeleteBigInventoryModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.DeleteBigInventoryModelMasterResult> DeleteBigInventoryModelMasterAsync(
                Request.DeleteBigInventoryModelMasterRequest request
        ) =>
            new DeleteBigInventoryModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.DeleteBigInventoryModelMasterResult>();
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
		public Task<Result.DeleteBigInventoryModelMasterResult> DeleteBigInventoryModelMasterAsync(
                Request.DeleteBigInventoryModelMasterRequest request
        ) =>
            new DeleteBigInventoryModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
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
        ) =>
            new DescribeBigInventoryModelsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.DescribeBigInventoryModelsResult> DescribeBigInventoryModelsFuture(
                Request.DescribeBigInventoryModelsRequest request
        ) =>
            new DescribeBigInventoryModelsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.DescribeBigInventoryModelsResult> DescribeBigInventoryModelsAsync(
                Request.DescribeBigInventoryModelsRequest request
        ) =>
            new DescribeBigInventoryModelsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.DescribeBigInventoryModelsResult>();
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
		public Task<Result.DescribeBigInventoryModelsResult> DescribeBigInventoryModelsAsync(
                Request.DescribeBigInventoryModelsRequest request
        ) =>
            new DescribeBigInventoryModelsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
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
        ) =>
            new GetBigInventoryModelTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.GetBigInventoryModelResult> GetBigInventoryModelFuture(
                Request.GetBigInventoryModelRequest request
        ) =>
            new GetBigInventoryModelTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.GetBigInventoryModelResult> GetBigInventoryModelAsync(
                Request.GetBigInventoryModelRequest request
        ) =>
            new GetBigInventoryModelTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.GetBigInventoryModelResult>();
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
		public Task<Result.GetBigInventoryModelResult> GetBigInventoryModelAsync(
                Request.GetBigInventoryModelRequest request
        ) =>
            new GetBigInventoryModelTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
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
                if (request.NamePrefix != null) {
                    sessionRequest.AddQueryString("namePrefix", $"{request.NamePrefix}");
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
        ) =>
            new DescribeBigItemModelMastersTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.DescribeBigItemModelMastersResult> DescribeBigItemModelMastersFuture(
                Request.DescribeBigItemModelMastersRequest request
        ) =>
            new DescribeBigItemModelMastersTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.DescribeBigItemModelMastersResult> DescribeBigItemModelMastersAsync(
                Request.DescribeBigItemModelMastersRequest request
        ) =>
            new DescribeBigItemModelMastersTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.DescribeBigItemModelMastersResult>();
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
		public Task<Result.DescribeBigItemModelMastersResult> DescribeBigItemModelMastersAsync(
                Request.DescribeBigItemModelMastersRequest request
        ) =>
            new DescribeBigItemModelMastersTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
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
        ) =>
            new CreateBigItemModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.CreateBigItemModelMasterResult> CreateBigItemModelMasterFuture(
                Request.CreateBigItemModelMasterRequest request
        ) =>
            new CreateBigItemModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.CreateBigItemModelMasterResult> CreateBigItemModelMasterAsync(
                Request.CreateBigItemModelMasterRequest request
        ) =>
            new CreateBigItemModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.CreateBigItemModelMasterResult>();
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
		public Task<Result.CreateBigItemModelMasterResult> CreateBigItemModelMasterAsync(
                Request.CreateBigItemModelMasterRequest request
        ) =>
            new CreateBigItemModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
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
        ) =>
            new GetBigItemModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.GetBigItemModelMasterResult> GetBigItemModelMasterFuture(
                Request.GetBigItemModelMasterRequest request
        ) =>
            new GetBigItemModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.GetBigItemModelMasterResult> GetBigItemModelMasterAsync(
                Request.GetBigItemModelMasterRequest request
        ) =>
            new GetBigItemModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.GetBigItemModelMasterResult>();
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
		public Task<Result.GetBigItemModelMasterResult> GetBigItemModelMasterAsync(
                Request.GetBigItemModelMasterRequest request
        ) =>
            new GetBigItemModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
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
        ) =>
            new UpdateBigItemModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.UpdateBigItemModelMasterResult> UpdateBigItemModelMasterFuture(
                Request.UpdateBigItemModelMasterRequest request
        ) =>
            new UpdateBigItemModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.UpdateBigItemModelMasterResult> UpdateBigItemModelMasterAsync(
                Request.UpdateBigItemModelMasterRequest request
        ) =>
            new UpdateBigItemModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.UpdateBigItemModelMasterResult>();
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
		public Task<Result.UpdateBigItemModelMasterResult> UpdateBigItemModelMasterAsync(
                Request.UpdateBigItemModelMasterRequest request
        ) =>
            new UpdateBigItemModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
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
        ) =>
            new DeleteBigItemModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.DeleteBigItemModelMasterResult> DeleteBigItemModelMasterFuture(
                Request.DeleteBigItemModelMasterRequest request
        ) =>
            new DeleteBigItemModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.DeleteBigItemModelMasterResult> DeleteBigItemModelMasterAsync(
                Request.DeleteBigItemModelMasterRequest request
        ) =>
            new DeleteBigItemModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.DeleteBigItemModelMasterResult>();
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
		public Task<Result.DeleteBigItemModelMasterResult> DeleteBigItemModelMasterAsync(
                Request.DeleteBigItemModelMasterRequest request
        ) =>
            new DeleteBigItemModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
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
        ) =>
            new DescribeBigItemModelsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.DescribeBigItemModelsResult> DescribeBigItemModelsFuture(
                Request.DescribeBigItemModelsRequest request
        ) =>
            new DescribeBigItemModelsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.DescribeBigItemModelsResult> DescribeBigItemModelsAsync(
                Request.DescribeBigItemModelsRequest request
        ) =>
            new DescribeBigItemModelsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.DescribeBigItemModelsResult>();
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
		public Task<Result.DescribeBigItemModelsResult> DescribeBigItemModelsAsync(
                Request.DescribeBigItemModelsRequest request
        ) =>
            new DescribeBigItemModelsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
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
        ) =>
            new GetBigItemModelTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.GetBigItemModelResult> GetBigItemModelFuture(
                Request.GetBigItemModelRequest request
        ) =>
            new GetBigItemModelTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.GetBigItemModelResult> GetBigItemModelAsync(
                Request.GetBigItemModelRequest request
        ) =>
            new GetBigItemModelTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.GetBigItemModelResult>();
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
		public Task<Result.GetBigItemModelResult> GetBigItemModelAsync(
                Request.GetBigItemModelRequest request
        ) =>
            new GetBigItemModelTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
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
        ) =>
            new ExportMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.ExportMasterResult> ExportMasterFuture(
                Request.ExportMasterRequest request
        ) =>
            new ExportMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.ExportMasterResult> ExportMasterAsync(
                Request.ExportMasterRequest request
        ) =>
            new ExportMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.ExportMasterResult>();
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
		public Task<Result.ExportMasterResult> ExportMasterAsync(
                Request.ExportMasterRequest request
        ) =>
            new ExportMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
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
        ) =>
            new GetCurrentItemModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.GetCurrentItemModelMasterResult> GetCurrentItemModelMasterFuture(
                Request.GetCurrentItemModelMasterRequest request
        ) =>
            new GetCurrentItemModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.GetCurrentItemModelMasterResult> GetCurrentItemModelMasterAsync(
                Request.GetCurrentItemModelMasterRequest request
        ) =>
            new GetCurrentItemModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.GetCurrentItemModelMasterResult>();
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
		public Task<Result.GetCurrentItemModelMasterResult> GetCurrentItemModelMasterAsync(
                Request.GetCurrentItemModelMasterRequest request
        ) =>
            new GetCurrentItemModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class PreUpdateCurrentItemModelMasterTask : Gs2RestSessionTask<PreUpdateCurrentItemModelMasterRequest, PreUpdateCurrentItemModelMasterResult>
        {
            public PreUpdateCurrentItemModelMasterTask(IGs2Session session, RestSessionRequestFactory factory, PreUpdateCurrentItemModelMasterRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(PreUpdateCurrentItemModelMasterRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "inventory")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/master";

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
		public IEnumerator PreUpdateCurrentItemModelMaster(
                Request.PreUpdateCurrentItemModelMasterRequest request,
                UnityAction<AsyncResult<Result.PreUpdateCurrentItemModelMasterResult>> callback
        ) =>
            new PreUpdateCurrentItemModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.PreUpdateCurrentItemModelMasterResult> PreUpdateCurrentItemModelMasterFuture(
                Request.PreUpdateCurrentItemModelMasterRequest request
        ) =>
            new PreUpdateCurrentItemModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.PreUpdateCurrentItemModelMasterResult> PreUpdateCurrentItemModelMasterAsync(
                Request.PreUpdateCurrentItemModelMasterRequest request
        ) =>
            new PreUpdateCurrentItemModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.PreUpdateCurrentItemModelMasterResult>();
    #else
		public PreUpdateCurrentItemModelMasterTask PreUpdateCurrentItemModelMasterAsync(
                Request.PreUpdateCurrentItemModelMasterRequest request
        )
		{
			return new PreUpdateCurrentItemModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.PreUpdateCurrentItemModelMasterResult> PreUpdateCurrentItemModelMasterAsync(
                Request.PreUpdateCurrentItemModelMasterRequest request
        ) =>
            new PreUpdateCurrentItemModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class UpdateCurrentItemModelMasterTask : Gs2RestSessionTask<UpdateCurrentItemModelMasterRequest, UpdateCurrentItemModelMasterResult>
        {
            public UpdateCurrentItemModelMasterTask(IGs2Session session, RestSessionRequestFactory factory, UpdateCurrentItemModelMasterRequest request) : base(session, factory, request)
            {
            }
#if GS2_ENABLE_UNITASK
            protected override async UniTask<UpdateCurrentItemModelMasterResult> InvokeImpl()
#else
            protected override async Task<UpdateCurrentItemModelMasterResult> InvokeImpl()
#endif
            {
                if (Request.Settings != null) {
                    var preTask = new PreUpdateCurrentItemModelMasterTask(
                        Session,
                        Factory,
                        new PreUpdateCurrentItemModelMasterRequest()
                            .WithContextStack(Request.ContextStack)
                            .WithNamespaceName(Request.NamespaceName)
                    );
                    var preTaskResult = await preTask.Invoke();
#if UNITY_2017_1_OR_NEWER
                    using (var request = UnityWebRequest.Put(preTaskResult.UploadUrl, Request.Settings))
                    {
                        request.SetRequestHeader("Content-Type", "application/json");
                        try {
                            await request.SendWebRequest();
                        }
#if GS2_ENABLE_UNITASK
                        catch (UnityWebRequestException) {}
#endif
                        finally {}

                        var restResult = request.result switch
                        {
                            UnityWebRequest.Result.Success or UnityWebRequest.Result.ProtocolError =>
                                new RestResult(
                                    (int) request.responseCode,
                                    request.downloadHandler?.text
                                ),
                            UnityWebRequest.Result.ConnectionError or UnityWebRequest.Result.DataProcessingError =>
                                new RestResult(
                                    (int) request.responseCode,
                                    null,
                                    (int) request.result,
                                    request.error
                                ),
                            _ =>
                                throw new InvalidOperationException(),
                        };

                        if (restResult.Error != null) throw restResult.Error;
                    }
#else
                    {
                        using var httpRequestMessage = new HttpRequestMessage(System.Net.Http.HttpMethod.Put, preTaskResult.UploadUrl);
                        httpRequestMessage.Content = new ByteArrayContent(System.Text.Encoding.UTF8.GetBytes(Request.Settings));
                        httpRequestMessage.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
    
                        RestResult restResult;
    
                        try
                        {
                            using var httpClient = new HttpClient();
                            using var httpResponseMessage = await httpClient.SendAsync(httpRequestMessage);
    
                            restResult = new RestResult((int) httpResponseMessage.StatusCode, "");
                        }
                        catch (OperationCanceledException e)
                        {
                            restResult = new RestResult(
                                0, // NoInternetConnectionException
                                "",
                                0,
                                e.Message
                            );
                        }
                        catch (System.Net.Http.HttpRequestException e)
                        {
                            restResult = new RestResult(
                                0, // NoInternetConnectionException
                                "",
                                0,
                                e.Message
                            );
                        }
    
                        if (restResult.Error != null) throw restResult.Error;
                    }
#endif
                    Request.Mode = "preUpload";
                    Request.UploadToken = preTaskResult.UploadToken;
                    Request.Settings = null;
                }
                return await base.InvokeImpl();
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
                if (request.Mode != null)
                {
                    jsonWriter.WritePropertyName("mode");
                    jsonWriter.Write(request.Mode);
                }
                if (request.Settings != null)
                {
                    jsonWriter.WritePropertyName("settings");
                    jsonWriter.Write(request.Settings);
                }
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
        ) =>
            new UpdateCurrentItemModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.UpdateCurrentItemModelMasterResult> UpdateCurrentItemModelMasterFuture(
                Request.UpdateCurrentItemModelMasterRequest request
        ) =>
            new UpdateCurrentItemModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.UpdateCurrentItemModelMasterResult> UpdateCurrentItemModelMasterAsync(
                Request.UpdateCurrentItemModelMasterRequest request
        ) =>
            new UpdateCurrentItemModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.UpdateCurrentItemModelMasterResult>();
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
		public Task<Result.UpdateCurrentItemModelMasterResult> UpdateCurrentItemModelMasterAsync(
                Request.UpdateCurrentItemModelMasterRequest request
        ) =>
            new UpdateCurrentItemModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
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
        ) =>
            new UpdateCurrentItemModelMasterFromGitHubTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.UpdateCurrentItemModelMasterFromGitHubResult> UpdateCurrentItemModelMasterFromGitHubFuture(
                Request.UpdateCurrentItemModelMasterFromGitHubRequest request
        ) =>
            new UpdateCurrentItemModelMasterFromGitHubTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.UpdateCurrentItemModelMasterFromGitHubResult> UpdateCurrentItemModelMasterFromGitHubAsync(
                Request.UpdateCurrentItemModelMasterFromGitHubRequest request
        ) =>
            new UpdateCurrentItemModelMasterFromGitHubTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.UpdateCurrentItemModelMasterFromGitHubResult>();
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
		public Task<Result.UpdateCurrentItemModelMasterFromGitHubResult> UpdateCurrentItemModelMasterFromGitHubAsync(
                Request.UpdateCurrentItemModelMasterFromGitHubRequest request
        ) =>
            new UpdateCurrentItemModelMasterFromGitHubTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
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
        ) =>
            new DescribeInventoriesTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.DescribeInventoriesResult> DescribeInventoriesFuture(
                Request.DescribeInventoriesRequest request
        ) =>
            new DescribeInventoriesTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.DescribeInventoriesResult> DescribeInventoriesAsync(
                Request.DescribeInventoriesRequest request
        ) =>
            new DescribeInventoriesTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.DescribeInventoriesResult>();
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
		public Task<Result.DescribeInventoriesResult> DescribeInventoriesAsync(
                Request.DescribeInventoriesRequest request
        ) =>
            new DescribeInventoriesTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
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
        ) =>
            new DescribeInventoriesByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.DescribeInventoriesByUserIdResult> DescribeInventoriesByUserIdFuture(
                Request.DescribeInventoriesByUserIdRequest request
        ) =>
            new DescribeInventoriesByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.DescribeInventoriesByUserIdResult> DescribeInventoriesByUserIdAsync(
                Request.DescribeInventoriesByUserIdRequest request
        ) =>
            new DescribeInventoriesByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.DescribeInventoriesByUserIdResult>();
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
		public Task<Result.DescribeInventoriesByUserIdResult> DescribeInventoriesByUserIdAsync(
                Request.DescribeInventoriesByUserIdRequest request
        ) =>
            new DescribeInventoriesByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
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
        ) =>
            new GetInventoryTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.GetInventoryResult> GetInventoryFuture(
                Request.GetInventoryRequest request
        ) =>
            new GetInventoryTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.GetInventoryResult> GetInventoryAsync(
                Request.GetInventoryRequest request
        ) =>
            new GetInventoryTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.GetInventoryResult>();
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
		public Task<Result.GetInventoryResult> GetInventoryAsync(
                Request.GetInventoryRequest request
        ) =>
            new GetInventoryTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
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
        ) =>
            new GetInventoryByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.GetInventoryByUserIdResult> GetInventoryByUserIdFuture(
                Request.GetInventoryByUserIdRequest request
        ) =>
            new GetInventoryByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.GetInventoryByUserIdResult> GetInventoryByUserIdAsync(
                Request.GetInventoryByUserIdRequest request
        ) =>
            new GetInventoryByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.GetInventoryByUserIdResult>();
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
		public Task<Result.GetInventoryByUserIdResult> GetInventoryByUserIdAsync(
                Request.GetInventoryByUserIdRequest request
        ) =>
            new GetInventoryByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
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
        ) =>
            new AddCapacityByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.AddCapacityByUserIdResult> AddCapacityByUserIdFuture(
                Request.AddCapacityByUserIdRequest request
        ) =>
            new AddCapacityByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.AddCapacityByUserIdResult> AddCapacityByUserIdAsync(
                Request.AddCapacityByUserIdRequest request
        ) =>
            new AddCapacityByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.AddCapacityByUserIdResult>();
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
		public Task<Result.AddCapacityByUserIdResult> AddCapacityByUserIdAsync(
                Request.AddCapacityByUserIdRequest request
        ) =>
            new AddCapacityByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
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
        ) =>
            new SetCapacityByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.SetCapacityByUserIdResult> SetCapacityByUserIdFuture(
                Request.SetCapacityByUserIdRequest request
        ) =>
            new SetCapacityByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.SetCapacityByUserIdResult> SetCapacityByUserIdAsync(
                Request.SetCapacityByUserIdRequest request
        ) =>
            new SetCapacityByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.SetCapacityByUserIdResult>();
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
		public Task<Result.SetCapacityByUserIdResult> SetCapacityByUserIdAsync(
                Request.SetCapacityByUserIdRequest request
        ) =>
            new SetCapacityByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
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
        ) =>
            new DeleteInventoryByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.DeleteInventoryByUserIdResult> DeleteInventoryByUserIdFuture(
                Request.DeleteInventoryByUserIdRequest request
        ) =>
            new DeleteInventoryByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.DeleteInventoryByUserIdResult> DeleteInventoryByUserIdAsync(
                Request.DeleteInventoryByUserIdRequest request
        ) =>
            new DeleteInventoryByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.DeleteInventoryByUserIdResult>();
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
		public Task<Result.DeleteInventoryByUserIdResult> DeleteInventoryByUserIdAsync(
                Request.DeleteInventoryByUserIdRequest request
        ) =>
            new DeleteInventoryByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
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
        ) =>
            new VerifyInventoryCurrentMaxCapacityTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.VerifyInventoryCurrentMaxCapacityResult> VerifyInventoryCurrentMaxCapacityFuture(
                Request.VerifyInventoryCurrentMaxCapacityRequest request
        ) =>
            new VerifyInventoryCurrentMaxCapacityTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.VerifyInventoryCurrentMaxCapacityResult> VerifyInventoryCurrentMaxCapacityAsync(
                Request.VerifyInventoryCurrentMaxCapacityRequest request
        ) =>
            new VerifyInventoryCurrentMaxCapacityTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.VerifyInventoryCurrentMaxCapacityResult>();
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
		public Task<Result.VerifyInventoryCurrentMaxCapacityResult> VerifyInventoryCurrentMaxCapacityAsync(
                Request.VerifyInventoryCurrentMaxCapacityRequest request
        ) =>
            new VerifyInventoryCurrentMaxCapacityTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
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
        ) =>
            new VerifyInventoryCurrentMaxCapacityByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.VerifyInventoryCurrentMaxCapacityByUserIdResult> VerifyInventoryCurrentMaxCapacityByUserIdFuture(
                Request.VerifyInventoryCurrentMaxCapacityByUserIdRequest request
        ) =>
            new VerifyInventoryCurrentMaxCapacityByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.VerifyInventoryCurrentMaxCapacityByUserIdResult> VerifyInventoryCurrentMaxCapacityByUserIdAsync(
                Request.VerifyInventoryCurrentMaxCapacityByUserIdRequest request
        ) =>
            new VerifyInventoryCurrentMaxCapacityByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.VerifyInventoryCurrentMaxCapacityByUserIdResult>();
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
		public Task<Result.VerifyInventoryCurrentMaxCapacityByUserIdResult> VerifyInventoryCurrentMaxCapacityByUserIdAsync(
                Request.VerifyInventoryCurrentMaxCapacityByUserIdRequest request
        ) =>
            new VerifyInventoryCurrentMaxCapacityByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
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
        ) =>
            new VerifyInventoryCurrentMaxCapacityByStampTaskTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.VerifyInventoryCurrentMaxCapacityByStampTaskResult> VerifyInventoryCurrentMaxCapacityByStampTaskFuture(
                Request.VerifyInventoryCurrentMaxCapacityByStampTaskRequest request
        ) =>
            new VerifyInventoryCurrentMaxCapacityByStampTaskTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.VerifyInventoryCurrentMaxCapacityByStampTaskResult> VerifyInventoryCurrentMaxCapacityByStampTaskAsync(
                Request.VerifyInventoryCurrentMaxCapacityByStampTaskRequest request
        ) =>
            new VerifyInventoryCurrentMaxCapacityByStampTaskTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.VerifyInventoryCurrentMaxCapacityByStampTaskResult>();
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
		public Task<Result.VerifyInventoryCurrentMaxCapacityByStampTaskResult> VerifyInventoryCurrentMaxCapacityByStampTaskAsync(
                Request.VerifyInventoryCurrentMaxCapacityByStampTaskRequest request
        ) =>
            new VerifyInventoryCurrentMaxCapacityByStampTaskTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
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
        ) =>
            new AddCapacityByStampSheetTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.AddCapacityByStampSheetResult> AddCapacityByStampSheetFuture(
                Request.AddCapacityByStampSheetRequest request
        ) =>
            new AddCapacityByStampSheetTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.AddCapacityByStampSheetResult> AddCapacityByStampSheetAsync(
                Request.AddCapacityByStampSheetRequest request
        ) =>
            new AddCapacityByStampSheetTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.AddCapacityByStampSheetResult>();
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
		public Task<Result.AddCapacityByStampSheetResult> AddCapacityByStampSheetAsync(
                Request.AddCapacityByStampSheetRequest request
        ) =>
            new AddCapacityByStampSheetTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
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
        ) =>
            new SetCapacityByStampSheetTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.SetCapacityByStampSheetResult> SetCapacityByStampSheetFuture(
                Request.SetCapacityByStampSheetRequest request
        ) =>
            new SetCapacityByStampSheetTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.SetCapacityByStampSheetResult> SetCapacityByStampSheetAsync(
                Request.SetCapacityByStampSheetRequest request
        ) =>
            new SetCapacityByStampSheetTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.SetCapacityByStampSheetResult>();
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
		public Task<Result.SetCapacityByStampSheetResult> SetCapacityByStampSheetAsync(
                Request.SetCapacityByStampSheetRequest request
        ) =>
            new SetCapacityByStampSheetTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
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
        ) =>
            new DescribeItemSetsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.DescribeItemSetsResult> DescribeItemSetsFuture(
                Request.DescribeItemSetsRequest request
        ) =>
            new DescribeItemSetsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.DescribeItemSetsResult> DescribeItemSetsAsync(
                Request.DescribeItemSetsRequest request
        ) =>
            new DescribeItemSetsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.DescribeItemSetsResult>();
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
		public Task<Result.DescribeItemSetsResult> DescribeItemSetsAsync(
                Request.DescribeItemSetsRequest request
        ) =>
            new DescribeItemSetsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
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
        ) =>
            new DescribeItemSetsByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.DescribeItemSetsByUserIdResult> DescribeItemSetsByUserIdFuture(
                Request.DescribeItemSetsByUserIdRequest request
        ) =>
            new DescribeItemSetsByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.DescribeItemSetsByUserIdResult> DescribeItemSetsByUserIdAsync(
                Request.DescribeItemSetsByUserIdRequest request
        ) =>
            new DescribeItemSetsByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.DescribeItemSetsByUserIdResult>();
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
		public Task<Result.DescribeItemSetsByUserIdResult> DescribeItemSetsByUserIdAsync(
                Request.DescribeItemSetsByUserIdRequest request
        ) =>
            new DescribeItemSetsByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
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
        ) =>
            new GetItemSetTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.GetItemSetResult> GetItemSetFuture(
                Request.GetItemSetRequest request
        ) =>
            new GetItemSetTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.GetItemSetResult> GetItemSetAsync(
                Request.GetItemSetRequest request
        ) =>
            new GetItemSetTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.GetItemSetResult>();
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
		public Task<Result.GetItemSetResult> GetItemSetAsync(
                Request.GetItemSetRequest request
        ) =>
            new GetItemSetTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
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
        ) =>
            new GetItemSetByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.GetItemSetByUserIdResult> GetItemSetByUserIdFuture(
                Request.GetItemSetByUserIdRequest request
        ) =>
            new GetItemSetByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.GetItemSetByUserIdResult> GetItemSetByUserIdAsync(
                Request.GetItemSetByUserIdRequest request
        ) =>
            new GetItemSetByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.GetItemSetByUserIdResult>();
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
		public Task<Result.GetItemSetByUserIdResult> GetItemSetByUserIdAsync(
                Request.GetItemSetByUserIdRequest request
        ) =>
            new GetItemSetByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
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
        ) =>
            new GetItemWithSignatureTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.GetItemWithSignatureResult> GetItemWithSignatureFuture(
                Request.GetItemWithSignatureRequest request
        ) =>
            new GetItemWithSignatureTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.GetItemWithSignatureResult> GetItemWithSignatureAsync(
                Request.GetItemWithSignatureRequest request
        ) =>
            new GetItemWithSignatureTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.GetItemWithSignatureResult>();
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
		public Task<Result.GetItemWithSignatureResult> GetItemWithSignatureAsync(
                Request.GetItemWithSignatureRequest request
        ) =>
            new GetItemWithSignatureTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
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
        ) =>
            new GetItemWithSignatureByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.GetItemWithSignatureByUserIdResult> GetItemWithSignatureByUserIdFuture(
                Request.GetItemWithSignatureByUserIdRequest request
        ) =>
            new GetItemWithSignatureByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.GetItemWithSignatureByUserIdResult> GetItemWithSignatureByUserIdAsync(
                Request.GetItemWithSignatureByUserIdRequest request
        ) =>
            new GetItemWithSignatureByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.GetItemWithSignatureByUserIdResult>();
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
		public Task<Result.GetItemWithSignatureByUserIdResult> GetItemWithSignatureByUserIdAsync(
                Request.GetItemWithSignatureByUserIdRequest request
        ) =>
            new GetItemWithSignatureByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
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
        ) =>
            new AcquireItemSetByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.AcquireItemSetByUserIdResult> AcquireItemSetByUserIdFuture(
                Request.AcquireItemSetByUserIdRequest request
        ) =>
            new AcquireItemSetByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.AcquireItemSetByUserIdResult> AcquireItemSetByUserIdAsync(
                Request.AcquireItemSetByUserIdRequest request
        ) =>
            new AcquireItemSetByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.AcquireItemSetByUserIdResult>();
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
		public Task<Result.AcquireItemSetByUserIdResult> AcquireItemSetByUserIdAsync(
                Request.AcquireItemSetByUserIdRequest request
        ) =>
            new AcquireItemSetByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
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
        ) =>
            new AcquireItemSetWithGradeByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.AcquireItemSetWithGradeByUserIdResult> AcquireItemSetWithGradeByUserIdFuture(
                Request.AcquireItemSetWithGradeByUserIdRequest request
        ) =>
            new AcquireItemSetWithGradeByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.AcquireItemSetWithGradeByUserIdResult> AcquireItemSetWithGradeByUserIdAsync(
                Request.AcquireItemSetWithGradeByUserIdRequest request
        ) =>
            new AcquireItemSetWithGradeByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.AcquireItemSetWithGradeByUserIdResult>();
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
		public Task<Result.AcquireItemSetWithGradeByUserIdResult> AcquireItemSetWithGradeByUserIdAsync(
                Request.AcquireItemSetWithGradeByUserIdRequest request
        ) =>
            new AcquireItemSetWithGradeByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
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
        ) =>
            new ConsumeItemSetTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.ConsumeItemSetResult> ConsumeItemSetFuture(
                Request.ConsumeItemSetRequest request
        ) =>
            new ConsumeItemSetTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.ConsumeItemSetResult> ConsumeItemSetAsync(
                Request.ConsumeItemSetRequest request
        ) =>
            new ConsumeItemSetTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.ConsumeItemSetResult>();
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
		public Task<Result.ConsumeItemSetResult> ConsumeItemSetAsync(
                Request.ConsumeItemSetRequest request
        ) =>
            new ConsumeItemSetTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
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
        ) =>
            new ConsumeItemSetByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.ConsumeItemSetByUserIdResult> ConsumeItemSetByUserIdFuture(
                Request.ConsumeItemSetByUserIdRequest request
        ) =>
            new ConsumeItemSetByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.ConsumeItemSetByUserIdResult> ConsumeItemSetByUserIdAsync(
                Request.ConsumeItemSetByUserIdRequest request
        ) =>
            new ConsumeItemSetByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.ConsumeItemSetByUserIdResult>();
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
		public Task<Result.ConsumeItemSetByUserIdResult> ConsumeItemSetByUserIdAsync(
                Request.ConsumeItemSetByUserIdRequest request
        ) =>
            new ConsumeItemSetByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
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
        ) =>
            new DeleteItemSetByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.DeleteItemSetByUserIdResult> DeleteItemSetByUserIdFuture(
                Request.DeleteItemSetByUserIdRequest request
        ) =>
            new DeleteItemSetByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.DeleteItemSetByUserIdResult> DeleteItemSetByUserIdAsync(
                Request.DeleteItemSetByUserIdRequest request
        ) =>
            new DeleteItemSetByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.DeleteItemSetByUserIdResult>();
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
		public Task<Result.DeleteItemSetByUserIdResult> DeleteItemSetByUserIdAsync(
                Request.DeleteItemSetByUserIdRequest request
        ) =>
            new DeleteItemSetByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
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
        ) =>
            new VerifyItemSetTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.VerifyItemSetResult> VerifyItemSetFuture(
                Request.VerifyItemSetRequest request
        ) =>
            new VerifyItemSetTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.VerifyItemSetResult> VerifyItemSetAsync(
                Request.VerifyItemSetRequest request
        ) =>
            new VerifyItemSetTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.VerifyItemSetResult>();
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
		public Task<Result.VerifyItemSetResult> VerifyItemSetAsync(
                Request.VerifyItemSetRequest request
        ) =>
            new VerifyItemSetTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
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
        ) =>
            new VerifyItemSetByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.VerifyItemSetByUserIdResult> VerifyItemSetByUserIdFuture(
                Request.VerifyItemSetByUserIdRequest request
        ) =>
            new VerifyItemSetByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.VerifyItemSetByUserIdResult> VerifyItemSetByUserIdAsync(
                Request.VerifyItemSetByUserIdRequest request
        ) =>
            new VerifyItemSetByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.VerifyItemSetByUserIdResult>();
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
		public Task<Result.VerifyItemSetByUserIdResult> VerifyItemSetByUserIdAsync(
                Request.VerifyItemSetByUserIdRequest request
        ) =>
            new VerifyItemSetByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
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
        ) =>
            new AcquireItemSetByStampSheetTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.AcquireItemSetByStampSheetResult> AcquireItemSetByStampSheetFuture(
                Request.AcquireItemSetByStampSheetRequest request
        ) =>
            new AcquireItemSetByStampSheetTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.AcquireItemSetByStampSheetResult> AcquireItemSetByStampSheetAsync(
                Request.AcquireItemSetByStampSheetRequest request
        ) =>
            new AcquireItemSetByStampSheetTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.AcquireItemSetByStampSheetResult>();
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
		public Task<Result.AcquireItemSetByStampSheetResult> AcquireItemSetByStampSheetAsync(
                Request.AcquireItemSetByStampSheetRequest request
        ) =>
            new AcquireItemSetByStampSheetTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
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
        ) =>
            new AcquireItemSetWithGradeByStampSheetTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.AcquireItemSetWithGradeByStampSheetResult> AcquireItemSetWithGradeByStampSheetFuture(
                Request.AcquireItemSetWithGradeByStampSheetRequest request
        ) =>
            new AcquireItemSetWithGradeByStampSheetTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.AcquireItemSetWithGradeByStampSheetResult> AcquireItemSetWithGradeByStampSheetAsync(
                Request.AcquireItemSetWithGradeByStampSheetRequest request
        ) =>
            new AcquireItemSetWithGradeByStampSheetTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.AcquireItemSetWithGradeByStampSheetResult>();
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
		public Task<Result.AcquireItemSetWithGradeByStampSheetResult> AcquireItemSetWithGradeByStampSheetAsync(
                Request.AcquireItemSetWithGradeByStampSheetRequest request
        ) =>
            new AcquireItemSetWithGradeByStampSheetTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
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
        ) =>
            new ConsumeItemSetByStampTaskTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.ConsumeItemSetByStampTaskResult> ConsumeItemSetByStampTaskFuture(
                Request.ConsumeItemSetByStampTaskRequest request
        ) =>
            new ConsumeItemSetByStampTaskTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.ConsumeItemSetByStampTaskResult> ConsumeItemSetByStampTaskAsync(
                Request.ConsumeItemSetByStampTaskRequest request
        ) =>
            new ConsumeItemSetByStampTaskTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.ConsumeItemSetByStampTaskResult>();
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
		public Task<Result.ConsumeItemSetByStampTaskResult> ConsumeItemSetByStampTaskAsync(
                Request.ConsumeItemSetByStampTaskRequest request
        ) =>
            new ConsumeItemSetByStampTaskTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
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
        ) =>
            new VerifyItemSetByStampTaskTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.VerifyItemSetByStampTaskResult> VerifyItemSetByStampTaskFuture(
                Request.VerifyItemSetByStampTaskRequest request
        ) =>
            new VerifyItemSetByStampTaskTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.VerifyItemSetByStampTaskResult> VerifyItemSetByStampTaskAsync(
                Request.VerifyItemSetByStampTaskRequest request
        ) =>
            new VerifyItemSetByStampTaskTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.VerifyItemSetByStampTaskResult>();
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
		public Task<Result.VerifyItemSetByStampTaskResult> VerifyItemSetByStampTaskAsync(
                Request.VerifyItemSetByStampTaskRequest request
        ) =>
            new VerifyItemSetByStampTaskTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
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
        ) =>
            new DescribeReferenceOfTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.DescribeReferenceOfResult> DescribeReferenceOfFuture(
                Request.DescribeReferenceOfRequest request
        ) =>
            new DescribeReferenceOfTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.DescribeReferenceOfResult> DescribeReferenceOfAsync(
                Request.DescribeReferenceOfRequest request
        ) =>
            new DescribeReferenceOfTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.DescribeReferenceOfResult>();
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
		public Task<Result.DescribeReferenceOfResult> DescribeReferenceOfAsync(
                Request.DescribeReferenceOfRequest request
        ) =>
            new DescribeReferenceOfTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
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
        ) =>
            new DescribeReferenceOfByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.DescribeReferenceOfByUserIdResult> DescribeReferenceOfByUserIdFuture(
                Request.DescribeReferenceOfByUserIdRequest request
        ) =>
            new DescribeReferenceOfByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.DescribeReferenceOfByUserIdResult> DescribeReferenceOfByUserIdAsync(
                Request.DescribeReferenceOfByUserIdRequest request
        ) =>
            new DescribeReferenceOfByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.DescribeReferenceOfByUserIdResult>();
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
		public Task<Result.DescribeReferenceOfByUserIdResult> DescribeReferenceOfByUserIdAsync(
                Request.DescribeReferenceOfByUserIdRequest request
        ) =>
            new DescribeReferenceOfByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
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
        ) =>
            new GetReferenceOfTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.GetReferenceOfResult> GetReferenceOfFuture(
                Request.GetReferenceOfRequest request
        ) =>
            new GetReferenceOfTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.GetReferenceOfResult> GetReferenceOfAsync(
                Request.GetReferenceOfRequest request
        ) =>
            new GetReferenceOfTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.GetReferenceOfResult>();
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
		public Task<Result.GetReferenceOfResult> GetReferenceOfAsync(
                Request.GetReferenceOfRequest request
        ) =>
            new GetReferenceOfTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
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
        ) =>
            new GetReferenceOfByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.GetReferenceOfByUserIdResult> GetReferenceOfByUserIdFuture(
                Request.GetReferenceOfByUserIdRequest request
        ) =>
            new GetReferenceOfByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.GetReferenceOfByUserIdResult> GetReferenceOfByUserIdAsync(
                Request.GetReferenceOfByUserIdRequest request
        ) =>
            new GetReferenceOfByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.GetReferenceOfByUserIdResult>();
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
		public Task<Result.GetReferenceOfByUserIdResult> GetReferenceOfByUserIdAsync(
                Request.GetReferenceOfByUserIdRequest request
        ) =>
            new GetReferenceOfByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
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
        ) =>
            new VerifyReferenceOfTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.VerifyReferenceOfResult> VerifyReferenceOfFuture(
                Request.VerifyReferenceOfRequest request
        ) =>
            new VerifyReferenceOfTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.VerifyReferenceOfResult> VerifyReferenceOfAsync(
                Request.VerifyReferenceOfRequest request
        ) =>
            new VerifyReferenceOfTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.VerifyReferenceOfResult>();
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
		public Task<Result.VerifyReferenceOfResult> VerifyReferenceOfAsync(
                Request.VerifyReferenceOfRequest request
        ) =>
            new VerifyReferenceOfTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
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
        ) =>
            new VerifyReferenceOfByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.VerifyReferenceOfByUserIdResult> VerifyReferenceOfByUserIdFuture(
                Request.VerifyReferenceOfByUserIdRequest request
        ) =>
            new VerifyReferenceOfByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.VerifyReferenceOfByUserIdResult> VerifyReferenceOfByUserIdAsync(
                Request.VerifyReferenceOfByUserIdRequest request
        ) =>
            new VerifyReferenceOfByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.VerifyReferenceOfByUserIdResult>();
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
		public Task<Result.VerifyReferenceOfByUserIdResult> VerifyReferenceOfByUserIdAsync(
                Request.VerifyReferenceOfByUserIdRequest request
        ) =>
            new VerifyReferenceOfByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
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
        ) =>
            new AddReferenceOfTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.AddReferenceOfResult> AddReferenceOfFuture(
                Request.AddReferenceOfRequest request
        ) =>
            new AddReferenceOfTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.AddReferenceOfResult> AddReferenceOfAsync(
                Request.AddReferenceOfRequest request
        ) =>
            new AddReferenceOfTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.AddReferenceOfResult>();
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
		public Task<Result.AddReferenceOfResult> AddReferenceOfAsync(
                Request.AddReferenceOfRequest request
        ) =>
            new AddReferenceOfTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
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
        ) =>
            new AddReferenceOfByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.AddReferenceOfByUserIdResult> AddReferenceOfByUserIdFuture(
                Request.AddReferenceOfByUserIdRequest request
        ) =>
            new AddReferenceOfByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.AddReferenceOfByUserIdResult> AddReferenceOfByUserIdAsync(
                Request.AddReferenceOfByUserIdRequest request
        ) =>
            new AddReferenceOfByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.AddReferenceOfByUserIdResult>();
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
		public Task<Result.AddReferenceOfByUserIdResult> AddReferenceOfByUserIdAsync(
                Request.AddReferenceOfByUserIdRequest request
        ) =>
            new AddReferenceOfByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
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
        ) =>
            new DeleteReferenceOfTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.DeleteReferenceOfResult> DeleteReferenceOfFuture(
                Request.DeleteReferenceOfRequest request
        ) =>
            new DeleteReferenceOfTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.DeleteReferenceOfResult> DeleteReferenceOfAsync(
                Request.DeleteReferenceOfRequest request
        ) =>
            new DeleteReferenceOfTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.DeleteReferenceOfResult>();
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
		public Task<Result.DeleteReferenceOfResult> DeleteReferenceOfAsync(
                Request.DeleteReferenceOfRequest request
        ) =>
            new DeleteReferenceOfTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
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
        ) =>
            new DeleteReferenceOfByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.DeleteReferenceOfByUserIdResult> DeleteReferenceOfByUserIdFuture(
                Request.DeleteReferenceOfByUserIdRequest request
        ) =>
            new DeleteReferenceOfByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.DeleteReferenceOfByUserIdResult> DeleteReferenceOfByUserIdAsync(
                Request.DeleteReferenceOfByUserIdRequest request
        ) =>
            new DeleteReferenceOfByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.DeleteReferenceOfByUserIdResult>();
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
		public Task<Result.DeleteReferenceOfByUserIdResult> DeleteReferenceOfByUserIdAsync(
                Request.DeleteReferenceOfByUserIdRequest request
        ) =>
            new DeleteReferenceOfByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
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
        ) =>
            new AddReferenceOfItemSetByStampSheetTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.AddReferenceOfItemSetByStampSheetResult> AddReferenceOfItemSetByStampSheetFuture(
                Request.AddReferenceOfItemSetByStampSheetRequest request
        ) =>
            new AddReferenceOfItemSetByStampSheetTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.AddReferenceOfItemSetByStampSheetResult> AddReferenceOfItemSetByStampSheetAsync(
                Request.AddReferenceOfItemSetByStampSheetRequest request
        ) =>
            new AddReferenceOfItemSetByStampSheetTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.AddReferenceOfItemSetByStampSheetResult>();
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
		public Task<Result.AddReferenceOfItemSetByStampSheetResult> AddReferenceOfItemSetByStampSheetAsync(
                Request.AddReferenceOfItemSetByStampSheetRequest request
        ) =>
            new AddReferenceOfItemSetByStampSheetTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
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
        ) =>
            new DeleteReferenceOfItemSetByStampSheetTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.DeleteReferenceOfItemSetByStampSheetResult> DeleteReferenceOfItemSetByStampSheetFuture(
                Request.DeleteReferenceOfItemSetByStampSheetRequest request
        ) =>
            new DeleteReferenceOfItemSetByStampSheetTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.DeleteReferenceOfItemSetByStampSheetResult> DeleteReferenceOfItemSetByStampSheetAsync(
                Request.DeleteReferenceOfItemSetByStampSheetRequest request
        ) =>
            new DeleteReferenceOfItemSetByStampSheetTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.DeleteReferenceOfItemSetByStampSheetResult>();
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
		public Task<Result.DeleteReferenceOfItemSetByStampSheetResult> DeleteReferenceOfItemSetByStampSheetAsync(
                Request.DeleteReferenceOfItemSetByStampSheetRequest request
        ) =>
            new DeleteReferenceOfItemSetByStampSheetTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
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
        ) =>
            new VerifyReferenceOfByStampTaskTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.VerifyReferenceOfByStampTaskResult> VerifyReferenceOfByStampTaskFuture(
                Request.VerifyReferenceOfByStampTaskRequest request
        ) =>
            new VerifyReferenceOfByStampTaskTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.VerifyReferenceOfByStampTaskResult> VerifyReferenceOfByStampTaskAsync(
                Request.VerifyReferenceOfByStampTaskRequest request
        ) =>
            new VerifyReferenceOfByStampTaskTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.VerifyReferenceOfByStampTaskResult>();
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
		public Task<Result.VerifyReferenceOfByStampTaskResult> VerifyReferenceOfByStampTaskAsync(
                Request.VerifyReferenceOfByStampTaskRequest request
        ) =>
            new VerifyReferenceOfByStampTaskTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
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
        ) =>
            new DescribeSimpleItemsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.DescribeSimpleItemsResult> DescribeSimpleItemsFuture(
                Request.DescribeSimpleItemsRequest request
        ) =>
            new DescribeSimpleItemsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.DescribeSimpleItemsResult> DescribeSimpleItemsAsync(
                Request.DescribeSimpleItemsRequest request
        ) =>
            new DescribeSimpleItemsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.DescribeSimpleItemsResult>();
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
		public Task<Result.DescribeSimpleItemsResult> DescribeSimpleItemsAsync(
                Request.DescribeSimpleItemsRequest request
        ) =>
            new DescribeSimpleItemsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
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
        ) =>
            new DescribeSimpleItemsByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.DescribeSimpleItemsByUserIdResult> DescribeSimpleItemsByUserIdFuture(
                Request.DescribeSimpleItemsByUserIdRequest request
        ) =>
            new DescribeSimpleItemsByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.DescribeSimpleItemsByUserIdResult> DescribeSimpleItemsByUserIdAsync(
                Request.DescribeSimpleItemsByUserIdRequest request
        ) =>
            new DescribeSimpleItemsByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.DescribeSimpleItemsByUserIdResult>();
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
		public Task<Result.DescribeSimpleItemsByUserIdResult> DescribeSimpleItemsByUserIdAsync(
                Request.DescribeSimpleItemsByUserIdRequest request
        ) =>
            new DescribeSimpleItemsByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
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
        ) =>
            new GetSimpleItemTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.GetSimpleItemResult> GetSimpleItemFuture(
                Request.GetSimpleItemRequest request
        ) =>
            new GetSimpleItemTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.GetSimpleItemResult> GetSimpleItemAsync(
                Request.GetSimpleItemRequest request
        ) =>
            new GetSimpleItemTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.GetSimpleItemResult>();
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
		public Task<Result.GetSimpleItemResult> GetSimpleItemAsync(
                Request.GetSimpleItemRequest request
        ) =>
            new GetSimpleItemTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
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
        ) =>
            new GetSimpleItemByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.GetSimpleItemByUserIdResult> GetSimpleItemByUserIdFuture(
                Request.GetSimpleItemByUserIdRequest request
        ) =>
            new GetSimpleItemByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.GetSimpleItemByUserIdResult> GetSimpleItemByUserIdAsync(
                Request.GetSimpleItemByUserIdRequest request
        ) =>
            new GetSimpleItemByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.GetSimpleItemByUserIdResult>();
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
		public Task<Result.GetSimpleItemByUserIdResult> GetSimpleItemByUserIdAsync(
                Request.GetSimpleItemByUserIdRequest request
        ) =>
            new GetSimpleItemByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
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
        ) =>
            new GetSimpleItemWithSignatureTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.GetSimpleItemWithSignatureResult> GetSimpleItemWithSignatureFuture(
                Request.GetSimpleItemWithSignatureRequest request
        ) =>
            new GetSimpleItemWithSignatureTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.GetSimpleItemWithSignatureResult> GetSimpleItemWithSignatureAsync(
                Request.GetSimpleItemWithSignatureRequest request
        ) =>
            new GetSimpleItemWithSignatureTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.GetSimpleItemWithSignatureResult>();
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
		public Task<Result.GetSimpleItemWithSignatureResult> GetSimpleItemWithSignatureAsync(
                Request.GetSimpleItemWithSignatureRequest request
        ) =>
            new GetSimpleItemWithSignatureTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
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
        ) =>
            new GetSimpleItemWithSignatureByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.GetSimpleItemWithSignatureByUserIdResult> GetSimpleItemWithSignatureByUserIdFuture(
                Request.GetSimpleItemWithSignatureByUserIdRequest request
        ) =>
            new GetSimpleItemWithSignatureByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.GetSimpleItemWithSignatureByUserIdResult> GetSimpleItemWithSignatureByUserIdAsync(
                Request.GetSimpleItemWithSignatureByUserIdRequest request
        ) =>
            new GetSimpleItemWithSignatureByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.GetSimpleItemWithSignatureByUserIdResult>();
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
		public Task<Result.GetSimpleItemWithSignatureByUserIdResult> GetSimpleItemWithSignatureByUserIdAsync(
                Request.GetSimpleItemWithSignatureByUserIdRequest request
        ) =>
            new GetSimpleItemWithSignatureByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
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
        ) =>
            new AcquireSimpleItemsByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.AcquireSimpleItemsByUserIdResult> AcquireSimpleItemsByUserIdFuture(
                Request.AcquireSimpleItemsByUserIdRequest request
        ) =>
            new AcquireSimpleItemsByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.AcquireSimpleItemsByUserIdResult> AcquireSimpleItemsByUserIdAsync(
                Request.AcquireSimpleItemsByUserIdRequest request
        ) =>
            new AcquireSimpleItemsByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.AcquireSimpleItemsByUserIdResult>();
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
		public Task<Result.AcquireSimpleItemsByUserIdResult> AcquireSimpleItemsByUserIdAsync(
                Request.AcquireSimpleItemsByUserIdRequest request
        ) =>
            new AcquireSimpleItemsByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
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
        ) =>
            new ConsumeSimpleItemsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.ConsumeSimpleItemsResult> ConsumeSimpleItemsFuture(
                Request.ConsumeSimpleItemsRequest request
        ) =>
            new ConsumeSimpleItemsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.ConsumeSimpleItemsResult> ConsumeSimpleItemsAsync(
                Request.ConsumeSimpleItemsRequest request
        ) =>
            new ConsumeSimpleItemsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.ConsumeSimpleItemsResult>();
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
		public Task<Result.ConsumeSimpleItemsResult> ConsumeSimpleItemsAsync(
                Request.ConsumeSimpleItemsRequest request
        ) =>
            new ConsumeSimpleItemsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
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
        ) =>
            new ConsumeSimpleItemsByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.ConsumeSimpleItemsByUserIdResult> ConsumeSimpleItemsByUserIdFuture(
                Request.ConsumeSimpleItemsByUserIdRequest request
        ) =>
            new ConsumeSimpleItemsByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.ConsumeSimpleItemsByUserIdResult> ConsumeSimpleItemsByUserIdAsync(
                Request.ConsumeSimpleItemsByUserIdRequest request
        ) =>
            new ConsumeSimpleItemsByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.ConsumeSimpleItemsByUserIdResult>();
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
		public Task<Result.ConsumeSimpleItemsByUserIdResult> ConsumeSimpleItemsByUserIdAsync(
                Request.ConsumeSimpleItemsByUserIdRequest request
        ) =>
            new ConsumeSimpleItemsByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
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
        ) =>
            new SetSimpleItemsByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.SetSimpleItemsByUserIdResult> SetSimpleItemsByUserIdFuture(
                Request.SetSimpleItemsByUserIdRequest request
        ) =>
            new SetSimpleItemsByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.SetSimpleItemsByUserIdResult> SetSimpleItemsByUserIdAsync(
                Request.SetSimpleItemsByUserIdRequest request
        ) =>
            new SetSimpleItemsByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.SetSimpleItemsByUserIdResult>();
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
		public Task<Result.SetSimpleItemsByUserIdResult> SetSimpleItemsByUserIdAsync(
                Request.SetSimpleItemsByUserIdRequest request
        ) =>
            new SetSimpleItemsByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
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
        ) =>
            new DeleteSimpleItemsByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.DeleteSimpleItemsByUserIdResult> DeleteSimpleItemsByUserIdFuture(
                Request.DeleteSimpleItemsByUserIdRequest request
        ) =>
            new DeleteSimpleItemsByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.DeleteSimpleItemsByUserIdResult> DeleteSimpleItemsByUserIdAsync(
                Request.DeleteSimpleItemsByUserIdRequest request
        ) =>
            new DeleteSimpleItemsByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.DeleteSimpleItemsByUserIdResult>();
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
		public Task<Result.DeleteSimpleItemsByUserIdResult> DeleteSimpleItemsByUserIdAsync(
                Request.DeleteSimpleItemsByUserIdRequest request
        ) =>
            new DeleteSimpleItemsByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
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
        ) =>
            new VerifySimpleItemTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.VerifySimpleItemResult> VerifySimpleItemFuture(
                Request.VerifySimpleItemRequest request
        ) =>
            new VerifySimpleItemTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.VerifySimpleItemResult> VerifySimpleItemAsync(
                Request.VerifySimpleItemRequest request
        ) =>
            new VerifySimpleItemTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.VerifySimpleItemResult>();
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
		public Task<Result.VerifySimpleItemResult> VerifySimpleItemAsync(
                Request.VerifySimpleItemRequest request
        ) =>
            new VerifySimpleItemTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
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
        ) =>
            new VerifySimpleItemByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.VerifySimpleItemByUserIdResult> VerifySimpleItemByUserIdFuture(
                Request.VerifySimpleItemByUserIdRequest request
        ) =>
            new VerifySimpleItemByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.VerifySimpleItemByUserIdResult> VerifySimpleItemByUserIdAsync(
                Request.VerifySimpleItemByUserIdRequest request
        ) =>
            new VerifySimpleItemByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.VerifySimpleItemByUserIdResult>();
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
		public Task<Result.VerifySimpleItemByUserIdResult> VerifySimpleItemByUserIdAsync(
                Request.VerifySimpleItemByUserIdRequest request
        ) =>
            new VerifySimpleItemByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
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
        ) =>
            new AcquireSimpleItemsByStampSheetTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.AcquireSimpleItemsByStampSheetResult> AcquireSimpleItemsByStampSheetFuture(
                Request.AcquireSimpleItemsByStampSheetRequest request
        ) =>
            new AcquireSimpleItemsByStampSheetTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.AcquireSimpleItemsByStampSheetResult> AcquireSimpleItemsByStampSheetAsync(
                Request.AcquireSimpleItemsByStampSheetRequest request
        ) =>
            new AcquireSimpleItemsByStampSheetTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.AcquireSimpleItemsByStampSheetResult>();
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
		public Task<Result.AcquireSimpleItemsByStampSheetResult> AcquireSimpleItemsByStampSheetAsync(
                Request.AcquireSimpleItemsByStampSheetRequest request
        ) =>
            new AcquireSimpleItemsByStampSheetTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
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
        ) =>
            new ConsumeSimpleItemsByStampTaskTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.ConsumeSimpleItemsByStampTaskResult> ConsumeSimpleItemsByStampTaskFuture(
                Request.ConsumeSimpleItemsByStampTaskRequest request
        ) =>
            new ConsumeSimpleItemsByStampTaskTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.ConsumeSimpleItemsByStampTaskResult> ConsumeSimpleItemsByStampTaskAsync(
                Request.ConsumeSimpleItemsByStampTaskRequest request
        ) =>
            new ConsumeSimpleItemsByStampTaskTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.ConsumeSimpleItemsByStampTaskResult>();
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
		public Task<Result.ConsumeSimpleItemsByStampTaskResult> ConsumeSimpleItemsByStampTaskAsync(
                Request.ConsumeSimpleItemsByStampTaskRequest request
        ) =>
            new ConsumeSimpleItemsByStampTaskTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
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
        ) =>
            new SetSimpleItemsByStampSheetTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.SetSimpleItemsByStampSheetResult> SetSimpleItemsByStampSheetFuture(
                Request.SetSimpleItemsByStampSheetRequest request
        ) =>
            new SetSimpleItemsByStampSheetTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.SetSimpleItemsByStampSheetResult> SetSimpleItemsByStampSheetAsync(
                Request.SetSimpleItemsByStampSheetRequest request
        ) =>
            new SetSimpleItemsByStampSheetTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.SetSimpleItemsByStampSheetResult>();
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
		public Task<Result.SetSimpleItemsByStampSheetResult> SetSimpleItemsByStampSheetAsync(
                Request.SetSimpleItemsByStampSheetRequest request
        ) =>
            new SetSimpleItemsByStampSheetTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
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
        ) =>
            new VerifySimpleItemByStampTaskTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.VerifySimpleItemByStampTaskResult> VerifySimpleItemByStampTaskFuture(
                Request.VerifySimpleItemByStampTaskRequest request
        ) =>
            new VerifySimpleItemByStampTaskTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.VerifySimpleItemByStampTaskResult> VerifySimpleItemByStampTaskAsync(
                Request.VerifySimpleItemByStampTaskRequest request
        ) =>
            new VerifySimpleItemByStampTaskTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.VerifySimpleItemByStampTaskResult>();
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
		public Task<Result.VerifySimpleItemByStampTaskResult> VerifySimpleItemByStampTaskAsync(
                Request.VerifySimpleItemByStampTaskRequest request
        ) =>
            new VerifySimpleItemByStampTaskTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
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
        ) =>
            new DescribeBigItemsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.DescribeBigItemsResult> DescribeBigItemsFuture(
                Request.DescribeBigItemsRequest request
        ) =>
            new DescribeBigItemsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.DescribeBigItemsResult> DescribeBigItemsAsync(
                Request.DescribeBigItemsRequest request
        ) =>
            new DescribeBigItemsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.DescribeBigItemsResult>();
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
		public Task<Result.DescribeBigItemsResult> DescribeBigItemsAsync(
                Request.DescribeBigItemsRequest request
        ) =>
            new DescribeBigItemsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
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
        ) =>
            new DescribeBigItemsByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.DescribeBigItemsByUserIdResult> DescribeBigItemsByUserIdFuture(
                Request.DescribeBigItemsByUserIdRequest request
        ) =>
            new DescribeBigItemsByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.DescribeBigItemsByUserIdResult> DescribeBigItemsByUserIdAsync(
                Request.DescribeBigItemsByUserIdRequest request
        ) =>
            new DescribeBigItemsByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.DescribeBigItemsByUserIdResult>();
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
		public Task<Result.DescribeBigItemsByUserIdResult> DescribeBigItemsByUserIdAsync(
                Request.DescribeBigItemsByUserIdRequest request
        ) =>
            new DescribeBigItemsByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
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
        ) =>
            new GetBigItemTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.GetBigItemResult> GetBigItemFuture(
                Request.GetBigItemRequest request
        ) =>
            new GetBigItemTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.GetBigItemResult> GetBigItemAsync(
                Request.GetBigItemRequest request
        ) =>
            new GetBigItemTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.GetBigItemResult>();
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
		public Task<Result.GetBigItemResult> GetBigItemAsync(
                Request.GetBigItemRequest request
        ) =>
            new GetBigItemTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
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
        ) =>
            new GetBigItemByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.GetBigItemByUserIdResult> GetBigItemByUserIdFuture(
                Request.GetBigItemByUserIdRequest request
        ) =>
            new GetBigItemByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.GetBigItemByUserIdResult> GetBigItemByUserIdAsync(
                Request.GetBigItemByUserIdRequest request
        ) =>
            new GetBigItemByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.GetBigItemByUserIdResult>();
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
		public Task<Result.GetBigItemByUserIdResult> GetBigItemByUserIdAsync(
                Request.GetBigItemByUserIdRequest request
        ) =>
            new GetBigItemByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
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
        ) =>
            new AcquireBigItemByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.AcquireBigItemByUserIdResult> AcquireBigItemByUserIdFuture(
                Request.AcquireBigItemByUserIdRequest request
        ) =>
            new AcquireBigItemByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.AcquireBigItemByUserIdResult> AcquireBigItemByUserIdAsync(
                Request.AcquireBigItemByUserIdRequest request
        ) =>
            new AcquireBigItemByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.AcquireBigItemByUserIdResult>();
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
		public Task<Result.AcquireBigItemByUserIdResult> AcquireBigItemByUserIdAsync(
                Request.AcquireBigItemByUserIdRequest request
        ) =>
            new AcquireBigItemByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
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
        ) =>
            new ConsumeBigItemTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.ConsumeBigItemResult> ConsumeBigItemFuture(
                Request.ConsumeBigItemRequest request
        ) =>
            new ConsumeBigItemTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.ConsumeBigItemResult> ConsumeBigItemAsync(
                Request.ConsumeBigItemRequest request
        ) =>
            new ConsumeBigItemTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.ConsumeBigItemResult>();
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
		public Task<Result.ConsumeBigItemResult> ConsumeBigItemAsync(
                Request.ConsumeBigItemRequest request
        ) =>
            new ConsumeBigItemTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
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
        ) =>
            new ConsumeBigItemByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.ConsumeBigItemByUserIdResult> ConsumeBigItemByUserIdFuture(
                Request.ConsumeBigItemByUserIdRequest request
        ) =>
            new ConsumeBigItemByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.ConsumeBigItemByUserIdResult> ConsumeBigItemByUserIdAsync(
                Request.ConsumeBigItemByUserIdRequest request
        ) =>
            new ConsumeBigItemByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.ConsumeBigItemByUserIdResult>();
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
		public Task<Result.ConsumeBigItemByUserIdResult> ConsumeBigItemByUserIdAsync(
                Request.ConsumeBigItemByUserIdRequest request
        ) =>
            new ConsumeBigItemByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
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
        ) =>
            new SetBigItemByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.SetBigItemByUserIdResult> SetBigItemByUserIdFuture(
                Request.SetBigItemByUserIdRequest request
        ) =>
            new SetBigItemByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.SetBigItemByUserIdResult> SetBigItemByUserIdAsync(
                Request.SetBigItemByUserIdRequest request
        ) =>
            new SetBigItemByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.SetBigItemByUserIdResult>();
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
		public Task<Result.SetBigItemByUserIdResult> SetBigItemByUserIdAsync(
                Request.SetBigItemByUserIdRequest request
        ) =>
            new SetBigItemByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
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
        ) =>
            new DeleteBigItemByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.DeleteBigItemByUserIdResult> DeleteBigItemByUserIdFuture(
                Request.DeleteBigItemByUserIdRequest request
        ) =>
            new DeleteBigItemByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.DeleteBigItemByUserIdResult> DeleteBigItemByUserIdAsync(
                Request.DeleteBigItemByUserIdRequest request
        ) =>
            new DeleteBigItemByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.DeleteBigItemByUserIdResult>();
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
		public Task<Result.DeleteBigItemByUserIdResult> DeleteBigItemByUserIdAsync(
                Request.DeleteBigItemByUserIdRequest request
        ) =>
            new DeleteBigItemByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
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
        ) =>
            new VerifyBigItemTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.VerifyBigItemResult> VerifyBigItemFuture(
                Request.VerifyBigItemRequest request
        ) =>
            new VerifyBigItemTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.VerifyBigItemResult> VerifyBigItemAsync(
                Request.VerifyBigItemRequest request
        ) =>
            new VerifyBigItemTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.VerifyBigItemResult>();
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
		public Task<Result.VerifyBigItemResult> VerifyBigItemAsync(
                Request.VerifyBigItemRequest request
        ) =>
            new VerifyBigItemTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
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
        ) =>
            new VerifyBigItemByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.VerifyBigItemByUserIdResult> VerifyBigItemByUserIdFuture(
                Request.VerifyBigItemByUserIdRequest request
        ) =>
            new VerifyBigItemByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.VerifyBigItemByUserIdResult> VerifyBigItemByUserIdAsync(
                Request.VerifyBigItemByUserIdRequest request
        ) =>
            new VerifyBigItemByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.VerifyBigItemByUserIdResult>();
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
		public Task<Result.VerifyBigItemByUserIdResult> VerifyBigItemByUserIdAsync(
                Request.VerifyBigItemByUserIdRequest request
        ) =>
            new VerifyBigItemByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
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
        ) =>
            new AcquireBigItemByStampSheetTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.AcquireBigItemByStampSheetResult> AcquireBigItemByStampSheetFuture(
                Request.AcquireBigItemByStampSheetRequest request
        ) =>
            new AcquireBigItemByStampSheetTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.AcquireBigItemByStampSheetResult> AcquireBigItemByStampSheetAsync(
                Request.AcquireBigItemByStampSheetRequest request
        ) =>
            new AcquireBigItemByStampSheetTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.AcquireBigItemByStampSheetResult>();
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
		public Task<Result.AcquireBigItemByStampSheetResult> AcquireBigItemByStampSheetAsync(
                Request.AcquireBigItemByStampSheetRequest request
        ) =>
            new AcquireBigItemByStampSheetTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
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
        ) =>
            new ConsumeBigItemByStampTaskTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.ConsumeBigItemByStampTaskResult> ConsumeBigItemByStampTaskFuture(
                Request.ConsumeBigItemByStampTaskRequest request
        ) =>
            new ConsumeBigItemByStampTaskTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.ConsumeBigItemByStampTaskResult> ConsumeBigItemByStampTaskAsync(
                Request.ConsumeBigItemByStampTaskRequest request
        ) =>
            new ConsumeBigItemByStampTaskTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.ConsumeBigItemByStampTaskResult>();
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
		public Task<Result.ConsumeBigItemByStampTaskResult> ConsumeBigItemByStampTaskAsync(
                Request.ConsumeBigItemByStampTaskRequest request
        ) =>
            new ConsumeBigItemByStampTaskTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
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
        ) =>
            new SetBigItemByStampSheetTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.SetBigItemByStampSheetResult> SetBigItemByStampSheetFuture(
                Request.SetBigItemByStampSheetRequest request
        ) =>
            new SetBigItemByStampSheetTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.SetBigItemByStampSheetResult> SetBigItemByStampSheetAsync(
                Request.SetBigItemByStampSheetRequest request
        ) =>
            new SetBigItemByStampSheetTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.SetBigItemByStampSheetResult>();
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
		public Task<Result.SetBigItemByStampSheetResult> SetBigItemByStampSheetAsync(
                Request.SetBigItemByStampSheetRequest request
        ) =>
            new SetBigItemByStampSheetTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
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
        ) =>
            new VerifyBigItemByStampTaskTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.VerifyBigItemByStampTaskResult> VerifyBigItemByStampTaskFuture(
                Request.VerifyBigItemByStampTaskRequest request
        ) =>
            new VerifyBigItemByStampTaskTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.VerifyBigItemByStampTaskResult> VerifyBigItemByStampTaskAsync(
                Request.VerifyBigItemByStampTaskRequest request
        ) =>
            new VerifyBigItemByStampTaskTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.VerifyBigItemByStampTaskResult>();
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
		public Task<Result.VerifyBigItemByStampTaskResult> VerifyBigItemByStampTaskAsync(
                Request.VerifyBigItemByStampTaskRequest request
        ) =>
            new VerifyBigItemByStampTaskTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif
	}
}