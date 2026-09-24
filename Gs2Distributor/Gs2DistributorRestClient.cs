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
                    .Replace("{service}", "distributor")
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
		public IEnumerator DescribeDistributorModelMasters(
                Request.DescribeDistributorModelMastersRequest request,
                UnityAction<AsyncResult<Result.DescribeDistributorModelMastersResult>> callback
        ) =>
            new DescribeDistributorModelMastersTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.DescribeDistributorModelMastersResult> DescribeDistributorModelMastersFuture(
                Request.DescribeDistributorModelMastersRequest request
        ) =>
            new DescribeDistributorModelMastersTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.DescribeDistributorModelMastersResult> DescribeDistributorModelMastersAsync(
                Request.DescribeDistributorModelMastersRequest request
        ) =>
            new DescribeDistributorModelMastersTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.DescribeDistributorModelMastersResult>();
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
		public Task<Result.DescribeDistributorModelMastersResult> DescribeDistributorModelMastersAsync(
                Request.DescribeDistributorModelMastersRequest request
        ) =>
            new DescribeDistributorModelMastersTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
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
        ) =>
            new CreateDistributorModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.CreateDistributorModelMasterResult> CreateDistributorModelMasterFuture(
                Request.CreateDistributorModelMasterRequest request
        ) =>
            new CreateDistributorModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.CreateDistributorModelMasterResult> CreateDistributorModelMasterAsync(
                Request.CreateDistributorModelMasterRequest request
        ) =>
            new CreateDistributorModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.CreateDistributorModelMasterResult>();
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
		public Task<Result.CreateDistributorModelMasterResult> CreateDistributorModelMasterAsync(
                Request.CreateDistributorModelMasterRequest request
        ) =>
            new CreateDistributorModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
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
        ) =>
            new GetDistributorModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.GetDistributorModelMasterResult> GetDistributorModelMasterFuture(
                Request.GetDistributorModelMasterRequest request
        ) =>
            new GetDistributorModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.GetDistributorModelMasterResult> GetDistributorModelMasterAsync(
                Request.GetDistributorModelMasterRequest request
        ) =>
            new GetDistributorModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.GetDistributorModelMasterResult>();
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
		public Task<Result.GetDistributorModelMasterResult> GetDistributorModelMasterAsync(
                Request.GetDistributorModelMasterRequest request
        ) =>
            new GetDistributorModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
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
        ) =>
            new UpdateDistributorModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.UpdateDistributorModelMasterResult> UpdateDistributorModelMasterFuture(
                Request.UpdateDistributorModelMasterRequest request
        ) =>
            new UpdateDistributorModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.UpdateDistributorModelMasterResult> UpdateDistributorModelMasterAsync(
                Request.UpdateDistributorModelMasterRequest request
        ) =>
            new UpdateDistributorModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.UpdateDistributorModelMasterResult>();
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
		public Task<Result.UpdateDistributorModelMasterResult> UpdateDistributorModelMasterAsync(
                Request.UpdateDistributorModelMasterRequest request
        ) =>
            new UpdateDistributorModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
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
        ) =>
            new DeleteDistributorModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.DeleteDistributorModelMasterResult> DeleteDistributorModelMasterFuture(
                Request.DeleteDistributorModelMasterRequest request
        ) =>
            new DeleteDistributorModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.DeleteDistributorModelMasterResult> DeleteDistributorModelMasterAsync(
                Request.DeleteDistributorModelMasterRequest request
        ) =>
            new DeleteDistributorModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.DeleteDistributorModelMasterResult>();
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
		public Task<Result.DeleteDistributorModelMasterResult> DeleteDistributorModelMasterAsync(
                Request.DeleteDistributorModelMasterRequest request
        ) =>
            new DeleteDistributorModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
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
        ) =>
            new DescribeDistributorModelsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.DescribeDistributorModelsResult> DescribeDistributorModelsFuture(
                Request.DescribeDistributorModelsRequest request
        ) =>
            new DescribeDistributorModelsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.DescribeDistributorModelsResult> DescribeDistributorModelsAsync(
                Request.DescribeDistributorModelsRequest request
        ) =>
            new DescribeDistributorModelsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.DescribeDistributorModelsResult>();
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
		public Task<Result.DescribeDistributorModelsResult> DescribeDistributorModelsAsync(
                Request.DescribeDistributorModelsRequest request
        ) =>
            new DescribeDistributorModelsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
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
        ) =>
            new GetDistributorModelTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.GetDistributorModelResult> GetDistributorModelFuture(
                Request.GetDistributorModelRequest request
        ) =>
            new GetDistributorModelTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.GetDistributorModelResult> GetDistributorModelAsync(
                Request.GetDistributorModelRequest request
        ) =>
            new GetDistributorModelTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.GetDistributorModelResult>();
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
		public Task<Result.GetDistributorModelResult> GetDistributorModelAsync(
                Request.GetDistributorModelRequest request
        ) =>
            new GetDistributorModelTask(
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
        ) =>
            new GetCurrentDistributorMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.GetCurrentDistributorMasterResult> GetCurrentDistributorMasterFuture(
                Request.GetCurrentDistributorMasterRequest request
        ) =>
            new GetCurrentDistributorMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.GetCurrentDistributorMasterResult> GetCurrentDistributorMasterAsync(
                Request.GetCurrentDistributorMasterRequest request
        ) =>
            new GetCurrentDistributorMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.GetCurrentDistributorMasterResult>();
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
		public Task<Result.GetCurrentDistributorMasterResult> GetCurrentDistributorMasterAsync(
                Request.GetCurrentDistributorMasterRequest request
        ) =>
            new GetCurrentDistributorMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class PreUpdateCurrentDistributorMasterTask : Gs2RestSessionTask<PreUpdateCurrentDistributorMasterRequest, PreUpdateCurrentDistributorMasterResult>
        {
            public PreUpdateCurrentDistributorMasterTask(IGs2Session session, RestSessionRequestFactory factory, PreUpdateCurrentDistributorMasterRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(PreUpdateCurrentDistributorMasterRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "distributor")
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
		public IEnumerator PreUpdateCurrentDistributorMaster(
                Request.PreUpdateCurrentDistributorMasterRequest request,
                UnityAction<AsyncResult<Result.PreUpdateCurrentDistributorMasterResult>> callback
        ) =>
            new PreUpdateCurrentDistributorMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.PreUpdateCurrentDistributorMasterResult> PreUpdateCurrentDistributorMasterFuture(
                Request.PreUpdateCurrentDistributorMasterRequest request
        ) =>
            new PreUpdateCurrentDistributorMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.PreUpdateCurrentDistributorMasterResult> PreUpdateCurrentDistributorMasterAsync(
                Request.PreUpdateCurrentDistributorMasterRequest request
        ) =>
            new PreUpdateCurrentDistributorMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.PreUpdateCurrentDistributorMasterResult>();
    #else
		public PreUpdateCurrentDistributorMasterTask PreUpdateCurrentDistributorMasterAsync(
                Request.PreUpdateCurrentDistributorMasterRequest request
        )
		{
			return new PreUpdateCurrentDistributorMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.PreUpdateCurrentDistributorMasterResult> PreUpdateCurrentDistributorMasterAsync(
                Request.PreUpdateCurrentDistributorMasterRequest request
        ) =>
            new PreUpdateCurrentDistributorMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class UpdateCurrentDistributorMasterTask : Gs2RestSessionTask<UpdateCurrentDistributorMasterRequest, UpdateCurrentDistributorMasterResult>
        {
            public UpdateCurrentDistributorMasterTask(IGs2Session session, RestSessionRequestFactory factory, UpdateCurrentDistributorMasterRequest request) : base(session, factory, request)
            {
            }
#if GS2_ENABLE_UNITASK
            protected override async UniTask<UpdateCurrentDistributorMasterResult> InvokeImpl()
#else
            protected override async Task<UpdateCurrentDistributorMasterResult> InvokeImpl()
#endif
            {
                if (Request.Settings != null) {
                    var preTask = new PreUpdateCurrentDistributorMasterTask(
                        Session,
                        Factory,
                        new PreUpdateCurrentDistributorMasterRequest()
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
		public IEnumerator UpdateCurrentDistributorMaster(
                Request.UpdateCurrentDistributorMasterRequest request,
                UnityAction<AsyncResult<Result.UpdateCurrentDistributorMasterResult>> callback
        ) =>
            new UpdateCurrentDistributorMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.UpdateCurrentDistributorMasterResult> UpdateCurrentDistributorMasterFuture(
                Request.UpdateCurrentDistributorMasterRequest request
        ) =>
            new UpdateCurrentDistributorMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.UpdateCurrentDistributorMasterResult> UpdateCurrentDistributorMasterAsync(
                Request.UpdateCurrentDistributorMasterRequest request
        ) =>
            new UpdateCurrentDistributorMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.UpdateCurrentDistributorMasterResult>();
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
		public Task<Result.UpdateCurrentDistributorMasterResult> UpdateCurrentDistributorMasterAsync(
                Request.UpdateCurrentDistributorMasterRequest request
        ) =>
            new UpdateCurrentDistributorMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
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
        ) =>
            new UpdateCurrentDistributorMasterFromGitHubTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.UpdateCurrentDistributorMasterFromGitHubResult> UpdateCurrentDistributorMasterFromGitHubFuture(
                Request.UpdateCurrentDistributorMasterFromGitHubRequest request
        ) =>
            new UpdateCurrentDistributorMasterFromGitHubTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.UpdateCurrentDistributorMasterFromGitHubResult> UpdateCurrentDistributorMasterFromGitHubAsync(
                Request.UpdateCurrentDistributorMasterFromGitHubRequest request
        ) =>
            new UpdateCurrentDistributorMasterFromGitHubTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.UpdateCurrentDistributorMasterFromGitHubResult>();
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
		public Task<Result.UpdateCurrentDistributorMasterFromGitHubResult> UpdateCurrentDistributorMasterFromGitHubAsync(
                Request.UpdateCurrentDistributorMasterFromGitHubRequest request
        ) =>
            new UpdateCurrentDistributorMasterFromGitHubTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
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
        ) =>
            new DistributeTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.DistributeResult> DistributeFuture(
                Request.DistributeRequest request
        ) =>
            new DistributeTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.DistributeResult> DistributeAsync(
                Request.DistributeRequest request
        ) =>
            new DistributeTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.DistributeResult>();
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
		public Task<Result.DistributeResult> DistributeAsync(
                Request.DistributeRequest request
        ) =>
            new DistributeTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
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
        ) =>
            new DistributeWithoutOverflowProcessTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.DistributeWithoutOverflowProcessResult> DistributeWithoutOverflowProcessFuture(
                Request.DistributeWithoutOverflowProcessRequest request
        ) =>
            new DistributeWithoutOverflowProcessTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.DistributeWithoutOverflowProcessResult> DistributeWithoutOverflowProcessAsync(
                Request.DistributeWithoutOverflowProcessRequest request
        ) =>
            new DistributeWithoutOverflowProcessTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.DistributeWithoutOverflowProcessResult>();
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
		public Task<Result.DistributeWithoutOverflowProcessResult> DistributeWithoutOverflowProcessAsync(
                Request.DistributeWithoutOverflowProcessRequest request
        ) =>
            new DistributeWithoutOverflowProcessTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
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
        ) =>
            new RunVerifyTaskTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.RunVerifyTaskResult> RunVerifyTaskFuture(
                Request.RunVerifyTaskRequest request
        ) =>
            new RunVerifyTaskTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.RunVerifyTaskResult> RunVerifyTaskAsync(
                Request.RunVerifyTaskRequest request
        ) =>
            new RunVerifyTaskTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.RunVerifyTaskResult>();
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
		public Task<Result.RunVerifyTaskResult> RunVerifyTaskAsync(
                Request.RunVerifyTaskRequest request
        ) =>
            new RunVerifyTaskTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
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
        ) =>
            new RunStampTaskTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.RunStampTaskResult> RunStampTaskFuture(
                Request.RunStampTaskRequest request
        ) =>
            new RunStampTaskTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.RunStampTaskResult> RunStampTaskAsync(
                Request.RunStampTaskRequest request
        ) =>
            new RunStampTaskTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.RunStampTaskResult>();
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
		public Task<Result.RunStampTaskResult> RunStampTaskAsync(
                Request.RunStampTaskRequest request
        ) =>
            new RunStampTaskTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
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
        ) =>
            new RunStampSheetTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.RunStampSheetResult> RunStampSheetFuture(
                Request.RunStampSheetRequest request
        ) =>
            new RunStampSheetTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.RunStampSheetResult> RunStampSheetAsync(
                Request.RunStampSheetRequest request
        ) =>
            new RunStampSheetTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.RunStampSheetResult>();
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
		public Task<Result.RunStampSheetResult> RunStampSheetAsync(
                Request.RunStampSheetRequest request
        ) =>
            new RunStampSheetTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
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
        ) =>
            new RunStampSheetExpressTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.RunStampSheetExpressResult> RunStampSheetExpressFuture(
                Request.RunStampSheetExpressRequest request
        ) =>
            new RunStampSheetExpressTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.RunStampSheetExpressResult> RunStampSheetExpressAsync(
                Request.RunStampSheetExpressRequest request
        ) =>
            new RunStampSheetExpressTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.RunStampSheetExpressResult>();
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
		public Task<Result.RunStampSheetExpressResult> RunStampSheetExpressAsync(
                Request.RunStampSheetExpressRequest request
        ) =>
            new RunStampSheetExpressTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
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
        ) =>
            new RunVerifyTaskWithoutNamespaceTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.RunVerifyTaskWithoutNamespaceResult> RunVerifyTaskWithoutNamespaceFuture(
                Request.RunVerifyTaskWithoutNamespaceRequest request
        ) =>
            new RunVerifyTaskWithoutNamespaceTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.RunVerifyTaskWithoutNamespaceResult> RunVerifyTaskWithoutNamespaceAsync(
                Request.RunVerifyTaskWithoutNamespaceRequest request
        ) =>
            new RunVerifyTaskWithoutNamespaceTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.RunVerifyTaskWithoutNamespaceResult>();
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
		public Task<Result.RunVerifyTaskWithoutNamespaceResult> RunVerifyTaskWithoutNamespaceAsync(
                Request.RunVerifyTaskWithoutNamespaceRequest request
        ) =>
            new RunVerifyTaskWithoutNamespaceTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
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
        ) =>
            new RunStampTaskWithoutNamespaceTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.RunStampTaskWithoutNamespaceResult> RunStampTaskWithoutNamespaceFuture(
                Request.RunStampTaskWithoutNamespaceRequest request
        ) =>
            new RunStampTaskWithoutNamespaceTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.RunStampTaskWithoutNamespaceResult> RunStampTaskWithoutNamespaceAsync(
                Request.RunStampTaskWithoutNamespaceRequest request
        ) =>
            new RunStampTaskWithoutNamespaceTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.RunStampTaskWithoutNamespaceResult>();
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
		public Task<Result.RunStampTaskWithoutNamespaceResult> RunStampTaskWithoutNamespaceAsync(
                Request.RunStampTaskWithoutNamespaceRequest request
        ) =>
            new RunStampTaskWithoutNamespaceTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
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
        ) =>
            new RunStampSheetWithoutNamespaceTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.RunStampSheetWithoutNamespaceResult> RunStampSheetWithoutNamespaceFuture(
                Request.RunStampSheetWithoutNamespaceRequest request
        ) =>
            new RunStampSheetWithoutNamespaceTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.RunStampSheetWithoutNamespaceResult> RunStampSheetWithoutNamespaceAsync(
                Request.RunStampSheetWithoutNamespaceRequest request
        ) =>
            new RunStampSheetWithoutNamespaceTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.RunStampSheetWithoutNamespaceResult>();
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
		public Task<Result.RunStampSheetWithoutNamespaceResult> RunStampSheetWithoutNamespaceAsync(
                Request.RunStampSheetWithoutNamespaceRequest request
        ) =>
            new RunStampSheetWithoutNamespaceTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
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
        ) =>
            new RunStampSheetExpressWithoutNamespaceTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.RunStampSheetExpressWithoutNamespaceResult> RunStampSheetExpressWithoutNamespaceFuture(
                Request.RunStampSheetExpressWithoutNamespaceRequest request
        ) =>
            new RunStampSheetExpressWithoutNamespaceTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.RunStampSheetExpressWithoutNamespaceResult> RunStampSheetExpressWithoutNamespaceAsync(
                Request.RunStampSheetExpressWithoutNamespaceRequest request
        ) =>
            new RunStampSheetExpressWithoutNamespaceTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.RunStampSheetExpressWithoutNamespaceResult>();
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
		public Task<Result.RunStampSheetExpressWithoutNamespaceResult> RunStampSheetExpressWithoutNamespaceAsync(
                Request.RunStampSheetExpressWithoutNamespaceRequest request
        ) =>
            new RunStampSheetExpressWithoutNamespaceTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
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
        ) =>
            new SetTransactionDefaultConfigTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.SetTransactionDefaultConfigResult> SetTransactionDefaultConfigFuture(
                Request.SetTransactionDefaultConfigRequest request
        ) =>
            new SetTransactionDefaultConfigTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.SetTransactionDefaultConfigResult> SetTransactionDefaultConfigAsync(
                Request.SetTransactionDefaultConfigRequest request
        ) =>
            new SetTransactionDefaultConfigTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.SetTransactionDefaultConfigResult>();
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
		public Task<Result.SetTransactionDefaultConfigResult> SetTransactionDefaultConfigAsync(
                Request.SetTransactionDefaultConfigRequest request
        ) =>
            new SetTransactionDefaultConfigTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
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
        ) =>
            new SetTransactionDefaultConfigByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.SetTransactionDefaultConfigByUserIdResult> SetTransactionDefaultConfigByUserIdFuture(
                Request.SetTransactionDefaultConfigByUserIdRequest request
        ) =>
            new SetTransactionDefaultConfigByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.SetTransactionDefaultConfigByUserIdResult> SetTransactionDefaultConfigByUserIdAsync(
                Request.SetTransactionDefaultConfigByUserIdRequest request
        ) =>
            new SetTransactionDefaultConfigByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.SetTransactionDefaultConfigByUserIdResult>();
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
		public Task<Result.SetTransactionDefaultConfigByUserIdResult> SetTransactionDefaultConfigByUserIdAsync(
                Request.SetTransactionDefaultConfigByUserIdRequest request
        ) =>
            new SetTransactionDefaultConfigByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
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
                if (request.DryRun)
                {
                    sessionRequest.AddHeader("X-GS2-DRY-RUN", "true");
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
        ) =>
            new FreezeMasterDataTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.FreezeMasterDataResult> FreezeMasterDataFuture(
                Request.FreezeMasterDataRequest request
        ) =>
            new FreezeMasterDataTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.FreezeMasterDataResult> FreezeMasterDataAsync(
                Request.FreezeMasterDataRequest request
        ) =>
            new FreezeMasterDataTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.FreezeMasterDataResult>();
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
		public Task<Result.FreezeMasterDataResult> FreezeMasterDataAsync(
                Request.FreezeMasterDataRequest request
        ) =>
            new FreezeMasterDataTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
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
        ) =>
            new FreezeMasterDataByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.FreezeMasterDataByUserIdResult> FreezeMasterDataByUserIdFuture(
                Request.FreezeMasterDataByUserIdRequest request
        ) =>
            new FreezeMasterDataByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.FreezeMasterDataByUserIdResult> FreezeMasterDataByUserIdAsync(
                Request.FreezeMasterDataByUserIdRequest request
        ) =>
            new FreezeMasterDataByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.FreezeMasterDataByUserIdResult>();
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
		public Task<Result.FreezeMasterDataByUserIdResult> FreezeMasterDataByUserIdAsync(
                Request.FreezeMasterDataByUserIdRequest request
        ) =>
            new FreezeMasterDataByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class SignFreezeMasterDataTimestampTask : Gs2RestSessionTask<SignFreezeMasterDataTimestampRequest, SignFreezeMasterDataTimestampResult>
        {
            public SignFreezeMasterDataTimestampTask(IGs2Session session, RestSessionRequestFactory factory, SignFreezeMasterDataTimestampRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(SignFreezeMasterDataTimestampRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "distributor")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/masterdata/freeze/timestamp";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");

                var sessionRequest = Factory.Post(url);

                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);
                jsonWriter.WriteObjectStart();
                if (request.Timestamp != null)
                {
                    jsonWriter.WritePropertyName("timestamp");
                    jsonWriter.Write(request.Timestamp.ToString());
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
		public IEnumerator SignFreezeMasterDataTimestamp(
                Request.SignFreezeMasterDataTimestampRequest request,
                UnityAction<AsyncResult<Result.SignFreezeMasterDataTimestampResult>> callback
        ) =>
            new SignFreezeMasterDataTimestampTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.SignFreezeMasterDataTimestampResult> SignFreezeMasterDataTimestampFuture(
                Request.SignFreezeMasterDataTimestampRequest request
        ) =>
            new SignFreezeMasterDataTimestampTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.SignFreezeMasterDataTimestampResult> SignFreezeMasterDataTimestampAsync(
                Request.SignFreezeMasterDataTimestampRequest request
        ) =>
            new SignFreezeMasterDataTimestampTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.SignFreezeMasterDataTimestampResult>();
    #else
		public SignFreezeMasterDataTimestampTask SignFreezeMasterDataTimestampAsync(
                Request.SignFreezeMasterDataTimestampRequest request
        )
		{
			return new SignFreezeMasterDataTimestampTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.SignFreezeMasterDataTimestampResult> SignFreezeMasterDataTimestampAsync(
                Request.SignFreezeMasterDataTimestampRequest request
        ) =>
            new SignFreezeMasterDataTimestampTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class FreezeMasterDataBySignedTimestampTask : Gs2RestSessionTask<FreezeMasterDataBySignedTimestampRequest, FreezeMasterDataBySignedTimestampResult>
        {
            public FreezeMasterDataBySignedTimestampTask(IGs2Session session, RestSessionRequestFactory factory, FreezeMasterDataBySignedTimestampRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(FreezeMasterDataBySignedTimestampRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "distributor")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/me/masterdata/freeze/timestamp";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");

                var sessionRequest = Factory.Post(url);

                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);
                jsonWriter.WriteObjectStart();
                if (request.Body != null)
                {
                    jsonWriter.WritePropertyName("body");
                    jsonWriter.Write(request.Body);
                }
                if (request.Signature != null)
                {
                    jsonWriter.WritePropertyName("signature");
                    jsonWriter.Write(request.Signature);
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
		public IEnumerator FreezeMasterDataBySignedTimestamp(
                Request.FreezeMasterDataBySignedTimestampRequest request,
                UnityAction<AsyncResult<Result.FreezeMasterDataBySignedTimestampResult>> callback
        ) =>
            new FreezeMasterDataBySignedTimestampTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.FreezeMasterDataBySignedTimestampResult> FreezeMasterDataBySignedTimestampFuture(
                Request.FreezeMasterDataBySignedTimestampRequest request
        ) =>
            new FreezeMasterDataBySignedTimestampTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.FreezeMasterDataBySignedTimestampResult> FreezeMasterDataBySignedTimestampAsync(
                Request.FreezeMasterDataBySignedTimestampRequest request
        ) =>
            new FreezeMasterDataBySignedTimestampTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.FreezeMasterDataBySignedTimestampResult>();
    #else
		public FreezeMasterDataBySignedTimestampTask FreezeMasterDataBySignedTimestampAsync(
                Request.FreezeMasterDataBySignedTimestampRequest request
        )
		{
			return new FreezeMasterDataBySignedTimestampTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.FreezeMasterDataBySignedTimestampResult> FreezeMasterDataBySignedTimestampAsync(
                Request.FreezeMasterDataBySignedTimestampRequest request
        ) =>
            new FreezeMasterDataBySignedTimestampTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class FreezeMasterDataByTimestampTask : Gs2RestSessionTask<FreezeMasterDataByTimestampRequest, FreezeMasterDataByTimestampResult>
        {
            public FreezeMasterDataByTimestampTask(IGs2Session session, RestSessionRequestFactory factory, FreezeMasterDataByTimestampRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(FreezeMasterDataByTimestampRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "distributor")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/me/masterdata/freeze/timestamp/raw";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");

                var sessionRequest = Factory.Post(url);

                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);
                jsonWriter.WriteObjectStart();
                if (request.Timestamp != null)
                {
                    jsonWriter.WritePropertyName("timestamp");
                    jsonWriter.Write(request.Timestamp.ToString());
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
                if (request.DryRun)
                {
                    sessionRequest.AddHeader("X-GS2-DRY-RUN", "true");
                }

                AddHeader(
                    Session.Credential,
                    sessionRequest
                );

                return sessionRequest;
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator FreezeMasterDataByTimestamp(
                Request.FreezeMasterDataByTimestampRequest request,
                UnityAction<AsyncResult<Result.FreezeMasterDataByTimestampResult>> callback
        ) =>
            new FreezeMasterDataByTimestampTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.FreezeMasterDataByTimestampResult> FreezeMasterDataByTimestampFuture(
                Request.FreezeMasterDataByTimestampRequest request
        ) =>
            new FreezeMasterDataByTimestampTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.FreezeMasterDataByTimestampResult> FreezeMasterDataByTimestampAsync(
                Request.FreezeMasterDataByTimestampRequest request
        ) =>
            new FreezeMasterDataByTimestampTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.FreezeMasterDataByTimestampResult>();
    #else
		public FreezeMasterDataByTimestampTask FreezeMasterDataByTimestampAsync(
                Request.FreezeMasterDataByTimestampRequest request
        )
		{
			return new FreezeMasterDataByTimestampTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.FreezeMasterDataByTimestampResult> FreezeMasterDataByTimestampAsync(
                Request.FreezeMasterDataByTimestampRequest request
        ) =>
            new FreezeMasterDataByTimestampTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class BatchExecuteApiTask : Gs2RestSessionTask<BatchExecuteApiRequest, BatchExecuteApiResult>
        {
            public BatchExecuteApiTask(IGs2Session session, RestSessionRequestFactory factory, BatchExecuteApiRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(BatchExecuteApiRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "distributor")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/batch/execute";

                var sessionRequest = Factory.Post(url);

                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);
                jsonWriter.WriteObjectStart();
                if (request.RequestPayloads != null)
                {
                    jsonWriter.WritePropertyName("requestPayloads");
                    jsonWriter.WriteArrayStart();
                    foreach(var item in request.RequestPayloads)
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
                if (request.DryRun)
                {
                    sessionRequest.AddHeader("X-GS2-DRY-RUN", "true");
                }

                AddHeader(
                    Session.Credential,
                    sessionRequest
                );

                return sessionRequest;
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator BatchExecuteApi(
                Request.BatchExecuteApiRequest request,
                UnityAction<AsyncResult<Result.BatchExecuteApiResult>> callback
        ) =>
            new BatchExecuteApiTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.BatchExecuteApiResult> BatchExecuteApiFuture(
                Request.BatchExecuteApiRequest request
        ) =>
            new BatchExecuteApiTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.BatchExecuteApiResult> BatchExecuteApiAsync(
                Request.BatchExecuteApiRequest request
        ) =>
            new BatchExecuteApiTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.BatchExecuteApiResult>();
    #else
		public BatchExecuteApiTask BatchExecuteApiAsync(
                Request.BatchExecuteApiRequest request
        )
		{
			return new BatchExecuteApiTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.BatchExecuteApiResult> BatchExecuteApiAsync(
                Request.BatchExecuteApiRequest request
        ) =>
            new BatchExecuteApiTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
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
        ) =>
            new IfExpressionByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.IfExpressionByUserIdResult> IfExpressionByUserIdFuture(
                Request.IfExpressionByUserIdRequest request
        ) =>
            new IfExpressionByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.IfExpressionByUserIdResult> IfExpressionByUserIdAsync(
                Request.IfExpressionByUserIdRequest request
        ) =>
            new IfExpressionByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.IfExpressionByUserIdResult>();
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
		public Task<Result.IfExpressionByUserIdResult> IfExpressionByUserIdAsync(
                Request.IfExpressionByUserIdRequest request
        ) =>
            new IfExpressionByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
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
        ) =>
            new AndExpressionByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.AndExpressionByUserIdResult> AndExpressionByUserIdFuture(
                Request.AndExpressionByUserIdRequest request
        ) =>
            new AndExpressionByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.AndExpressionByUserIdResult> AndExpressionByUserIdAsync(
                Request.AndExpressionByUserIdRequest request
        ) =>
            new AndExpressionByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.AndExpressionByUserIdResult>();
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
		public Task<Result.AndExpressionByUserIdResult> AndExpressionByUserIdAsync(
                Request.AndExpressionByUserIdRequest request
        ) =>
            new AndExpressionByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
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
        ) =>
            new OrExpressionByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.OrExpressionByUserIdResult> OrExpressionByUserIdFuture(
                Request.OrExpressionByUserIdRequest request
        ) =>
            new OrExpressionByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.OrExpressionByUserIdResult> OrExpressionByUserIdAsync(
                Request.OrExpressionByUserIdRequest request
        ) =>
            new OrExpressionByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.OrExpressionByUserIdResult>();
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
		public Task<Result.OrExpressionByUserIdResult> OrExpressionByUserIdAsync(
                Request.OrExpressionByUserIdRequest request
        ) =>
            new OrExpressionByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
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
        ) =>
            new IfExpressionByStampTaskTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.IfExpressionByStampTaskResult> IfExpressionByStampTaskFuture(
                Request.IfExpressionByStampTaskRequest request
        ) =>
            new IfExpressionByStampTaskTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.IfExpressionByStampTaskResult> IfExpressionByStampTaskAsync(
                Request.IfExpressionByStampTaskRequest request
        ) =>
            new IfExpressionByStampTaskTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.IfExpressionByStampTaskResult>();
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
		public Task<Result.IfExpressionByStampTaskResult> IfExpressionByStampTaskAsync(
                Request.IfExpressionByStampTaskRequest request
        ) =>
            new IfExpressionByStampTaskTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
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
        ) =>
            new AndExpressionByStampTaskTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.AndExpressionByStampTaskResult> AndExpressionByStampTaskFuture(
                Request.AndExpressionByStampTaskRequest request
        ) =>
            new AndExpressionByStampTaskTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.AndExpressionByStampTaskResult> AndExpressionByStampTaskAsync(
                Request.AndExpressionByStampTaskRequest request
        ) =>
            new AndExpressionByStampTaskTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.AndExpressionByStampTaskResult>();
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
		public Task<Result.AndExpressionByStampTaskResult> AndExpressionByStampTaskAsync(
                Request.AndExpressionByStampTaskRequest request
        ) =>
            new AndExpressionByStampTaskTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
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
        ) =>
            new OrExpressionByStampTaskTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.OrExpressionByStampTaskResult> OrExpressionByStampTaskFuture(
                Request.OrExpressionByStampTaskRequest request
        ) =>
            new OrExpressionByStampTaskTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.OrExpressionByStampTaskResult> OrExpressionByStampTaskAsync(
                Request.OrExpressionByStampTaskRequest request
        ) =>
            new OrExpressionByStampTaskTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.OrExpressionByStampTaskResult>();
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
		public Task<Result.OrExpressionByStampTaskResult> OrExpressionByStampTaskAsync(
                Request.OrExpressionByStampTaskRequest request
        ) =>
            new OrExpressionByStampTaskTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
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
        ) =>
            new GetStampSheetResultTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.GetStampSheetResultResult> GetStampSheetResultFuture(
                Request.GetStampSheetResultRequest request
        ) =>
            new GetStampSheetResultTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.GetStampSheetResultResult> GetStampSheetResultAsync(
                Request.GetStampSheetResultRequest request
        ) =>
            new GetStampSheetResultTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.GetStampSheetResultResult>();
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
		public Task<Result.GetStampSheetResultResult> GetStampSheetResultAsync(
                Request.GetStampSheetResultRequest request
        ) =>
            new GetStampSheetResultTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
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
        ) =>
            new GetStampSheetResultByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.GetStampSheetResultByUserIdResult> GetStampSheetResultByUserIdFuture(
                Request.GetStampSheetResultByUserIdRequest request
        ) =>
            new GetStampSheetResultByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.GetStampSheetResultByUserIdResult> GetStampSheetResultByUserIdAsync(
                Request.GetStampSheetResultByUserIdRequest request
        ) =>
            new GetStampSheetResultByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.GetStampSheetResultByUserIdResult>();
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
		public Task<Result.GetStampSheetResultByUserIdResult> GetStampSheetResultByUserIdAsync(
                Request.GetStampSheetResultByUserIdRequest request
        ) =>
            new GetStampSheetResultByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class DescribeUserDataTask : Gs2RestSessionTask<DescribeUserDataRequest, DescribeUserDataResult>
        {
            public DescribeUserDataTask(IGs2Session session, RestSessionRequestFactory factory, DescribeUserDataRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DescribeUserDataRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "distributor")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/user/me/data";

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
		public IEnumerator DescribeUserData(
                Request.DescribeUserDataRequest request,
                UnityAction<AsyncResult<Result.DescribeUserDataResult>> callback
        ) =>
            new DescribeUserDataTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.DescribeUserDataResult> DescribeUserDataFuture(
                Request.DescribeUserDataRequest request
        ) =>
            new DescribeUserDataTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.DescribeUserDataResult> DescribeUserDataAsync(
                Request.DescribeUserDataRequest request
        ) =>
            new DescribeUserDataTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.DescribeUserDataResult>();
    #else
		public DescribeUserDataTask DescribeUserDataAsync(
                Request.DescribeUserDataRequest request
        )
		{
			return new DescribeUserDataTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.DescribeUserDataResult> DescribeUserDataAsync(
                Request.DescribeUserDataRequest request
        ) =>
            new DescribeUserDataTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class DescribeUserDataByUserIdTask : Gs2RestSessionTask<DescribeUserDataByUserIdRequest, DescribeUserDataByUserIdResult>
        {
            public DescribeUserDataByUserIdTask(IGs2Session session, RestSessionRequestFactory factory, DescribeUserDataByUserIdRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DescribeUserDataByUserIdRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "distributor")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/user/{userId}/data";

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
		public IEnumerator DescribeUserDataByUserId(
                Request.DescribeUserDataByUserIdRequest request,
                UnityAction<AsyncResult<Result.DescribeUserDataByUserIdResult>> callback
        ) =>
            new DescribeUserDataByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.DescribeUserDataByUserIdResult> DescribeUserDataByUserIdFuture(
                Request.DescribeUserDataByUserIdRequest request
        ) =>
            new DescribeUserDataByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.DescribeUserDataByUserIdResult> DescribeUserDataByUserIdAsync(
                Request.DescribeUserDataByUserIdRequest request
        ) =>
            new DescribeUserDataByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.DescribeUserDataByUserIdResult>();
    #else
		public DescribeUserDataByUserIdTask DescribeUserDataByUserIdAsync(
                Request.DescribeUserDataByUserIdRequest request
        )
		{
			return new DescribeUserDataByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.DescribeUserDataByUserIdResult> DescribeUserDataByUserIdAsync(
                Request.DescribeUserDataByUserIdRequest request
        ) =>
            new DescribeUserDataByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
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
        ) =>
            new RunTransactionTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.RunTransactionResult> RunTransactionFuture(
                Request.RunTransactionRequest request
        ) =>
            new RunTransactionTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.RunTransactionResult> RunTransactionAsync(
                Request.RunTransactionRequest request
        ) =>
            new RunTransactionTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.RunTransactionResult>();
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
		public Task<Result.RunTransactionResult> RunTransactionAsync(
                Request.RunTransactionRequest request
        ) =>
            new RunTransactionTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
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
        ) =>
            new GetTransactionResultTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.GetTransactionResultResult> GetTransactionResultFuture(
                Request.GetTransactionResultRequest request
        ) =>
            new GetTransactionResultTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.GetTransactionResultResult> GetTransactionResultAsync(
                Request.GetTransactionResultRequest request
        ) =>
            new GetTransactionResultTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.GetTransactionResultResult>();
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
		public Task<Result.GetTransactionResultResult> GetTransactionResultAsync(
                Request.GetTransactionResultRequest request
        ) =>
            new GetTransactionResultTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
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
        ) =>
            new GetTransactionResultByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.GetTransactionResultByUserIdResult> GetTransactionResultByUserIdFuture(
                Request.GetTransactionResultByUserIdRequest request
        ) =>
            new GetTransactionResultByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.GetTransactionResultByUserIdResult> GetTransactionResultByUserIdAsync(
                Request.GetTransactionResultByUserIdRequest request
        ) =>
            new GetTransactionResultByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.GetTransactionResultByUserIdResult>();
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
		public Task<Result.GetTransactionResultByUserIdResult> GetTransactionResultByUserIdAsync(
                Request.GetTransactionResultByUserIdRequest request
        ) =>
            new GetTransactionResultByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif
	}
}