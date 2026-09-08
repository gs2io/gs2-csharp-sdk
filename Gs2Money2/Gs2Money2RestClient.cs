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
using Gs2.Gs2Money2.Request;
using Gs2.Gs2Money2.Result;
using Gs2.Util.LitJson;

namespace Gs2.Gs2Money2
{
	public class Gs2Money2RestClient : AbstractGs2Client
	{
#if UNITY_2017_1_OR_NEWER
		private readonly CertificateHandler _certificateHandler;
#endif

		public static string Endpoint = "money2";

        protected Gs2RestSession Gs2RestSession => (Gs2RestSession) Gs2Session;

		public Gs2Money2RestClient(Gs2RestSession Gs2RestSession) : base(Gs2RestSession)
		{

		}

#if UNITY_2017_1_OR_NEWER
		public Gs2Money2RestClient(Gs2RestSession gs2RestSession, CertificateHandler certificateHandler) : base(gs2RestSession)
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
                    .Replace("{service}", "money2")
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
                    .Replace("{service}", "money2")
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
                if (request.CurrencyUsagePriority != null)
                {
                    jsonWriter.WritePropertyName("currencyUsagePriority");
                    jsonWriter.Write(request.CurrencyUsagePriority);
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
                if (request.SharedFreeCurrency != null)
                {
                    jsonWriter.WritePropertyName("sharedFreeCurrency");
                    jsonWriter.Write(request.SharedFreeCurrency.ToString());
                }
                if (request.PlatformSetting != null)
                {
                    jsonWriter.WritePropertyName("platformSetting");
                    request.PlatformSetting.WriteJson(jsonWriter);
                }
                if (request.DepositBalanceScript != null)
                {
                    jsonWriter.WritePropertyName("depositBalanceScript");
                    request.DepositBalanceScript.WriteJson(jsonWriter);
                }
                if (request.WithdrawBalanceScript != null)
                {
                    jsonWriter.WritePropertyName("withdrawBalanceScript");
                    request.WithdrawBalanceScript.WriteJson(jsonWriter);
                }
                if (request.VerifyReceiptScript != null)
                {
                    jsonWriter.WritePropertyName("verifyReceiptScript");
                    request.VerifyReceiptScript.WriteJson(jsonWriter);
                }
                if (request.SubscribeScript != null)
                {
                    jsonWriter.WritePropertyName("subscribeScript");
                    jsonWriter.Write(request.SubscribeScript);
                }
                if (request.RenewScript != null)
                {
                    jsonWriter.WritePropertyName("renewScript");
                    jsonWriter.Write(request.RenewScript);
                }
                if (request.UnsubscribeScript != null)
                {
                    jsonWriter.WritePropertyName("unsubscribeScript");
                    jsonWriter.Write(request.UnsubscribeScript);
                }
                if (request.TakeOverScript != null)
                {
                    jsonWriter.WritePropertyName("takeOverScript");
                    request.TakeOverScript.WriteJson(jsonWriter);
                }
                if (request.ChangeSubscriptionStatusNotification != null)
                {
                    jsonWriter.WritePropertyName("changeSubscriptionStatusNotification");
                    request.ChangeSubscriptionStatusNotification.WriteJson(jsonWriter);
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
                    .Replace("{service}", "money2")
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
                    .Replace("{service}", "money2")
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
                    .Replace("{service}", "money2")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");

                var sessionRequest = Factory.Put(url);

                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);
                jsonWriter.WriteObjectStart();
                if (request.CurrencyUsagePriority != null)
                {
                    jsonWriter.WritePropertyName("currencyUsagePriority");
                    jsonWriter.Write(request.CurrencyUsagePriority);
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
                if (request.PlatformSetting != null)
                {
                    jsonWriter.WritePropertyName("platformSetting");
                    request.PlatformSetting.WriteJson(jsonWriter);
                }
                if (request.DepositBalanceScript != null)
                {
                    jsonWriter.WritePropertyName("depositBalanceScript");
                    request.DepositBalanceScript.WriteJson(jsonWriter);
                }
                if (request.WithdrawBalanceScript != null)
                {
                    jsonWriter.WritePropertyName("withdrawBalanceScript");
                    request.WithdrawBalanceScript.WriteJson(jsonWriter);
                }
                if (request.VerifyReceiptScript != null)
                {
                    jsonWriter.WritePropertyName("verifyReceiptScript");
                    request.VerifyReceiptScript.WriteJson(jsonWriter);
                }
                if (request.SubscribeScript != null)
                {
                    jsonWriter.WritePropertyName("subscribeScript");
                    jsonWriter.Write(request.SubscribeScript);
                }
                if (request.RenewScript != null)
                {
                    jsonWriter.WritePropertyName("renewScript");
                    jsonWriter.Write(request.RenewScript);
                }
                if (request.UnsubscribeScript != null)
                {
                    jsonWriter.WritePropertyName("unsubscribeScript");
                    jsonWriter.Write(request.UnsubscribeScript);
                }
                if (request.TakeOverScript != null)
                {
                    jsonWriter.WritePropertyName("takeOverScript");
                    request.TakeOverScript.WriteJson(jsonWriter);
                }
                if (request.ChangeSubscriptionStatusNotification != null)
                {
                    jsonWriter.WritePropertyName("changeSubscriptionStatusNotification");
                    request.ChangeSubscriptionStatusNotification.WriteJson(jsonWriter);
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
                    .Replace("{service}", "money2")
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
                    .Replace("{service}", "money2")
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
                    .Replace("{service}", "money2")
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
                    .Replace("{service}", "money2")
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
                    .Replace("{service}", "money2")
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
                    .Replace("{service}", "money2")
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
                    .Replace("{service}", "money2")
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
                    .Replace("{service}", "money2")
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
                    .Replace("{service}", "money2")
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


        public class DescribeWalletsTask : Gs2RestSessionTask<DescribeWalletsRequest, DescribeWalletsResult>
        {
            public DescribeWalletsTask(IGs2Session session, RestSessionRequestFactory factory, DescribeWalletsRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DescribeWalletsRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "money2")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/me/wallet";

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
		public IEnumerator DescribeWallets(
                Request.DescribeWalletsRequest request,
                UnityAction<AsyncResult<Result.DescribeWalletsResult>> callback
        ) =>
            new DescribeWalletsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.DescribeWalletsResult> DescribeWalletsFuture(
                Request.DescribeWalletsRequest request
        ) =>
            new DescribeWalletsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.DescribeWalletsResult> DescribeWalletsAsync(
                Request.DescribeWalletsRequest request
        ) =>
            new DescribeWalletsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.DescribeWalletsResult>();
    #else
		public DescribeWalletsTask DescribeWalletsAsync(
                Request.DescribeWalletsRequest request
        )
		{
			return new DescribeWalletsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.DescribeWalletsResult> DescribeWalletsAsync(
                Request.DescribeWalletsRequest request
        ) =>
            new DescribeWalletsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class DescribeWalletsByUserIdTask : Gs2RestSessionTask<DescribeWalletsByUserIdRequest, DescribeWalletsByUserIdResult>
        {
            public DescribeWalletsByUserIdTask(IGs2Session session, RestSessionRequestFactory factory, DescribeWalletsByUserIdRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DescribeWalletsByUserIdRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "money2")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/{userId}/wallet";

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
		public IEnumerator DescribeWalletsByUserId(
                Request.DescribeWalletsByUserIdRequest request,
                UnityAction<AsyncResult<Result.DescribeWalletsByUserIdResult>> callback
        ) =>
            new DescribeWalletsByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.DescribeWalletsByUserIdResult> DescribeWalletsByUserIdFuture(
                Request.DescribeWalletsByUserIdRequest request
        ) =>
            new DescribeWalletsByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.DescribeWalletsByUserIdResult> DescribeWalletsByUserIdAsync(
                Request.DescribeWalletsByUserIdRequest request
        ) =>
            new DescribeWalletsByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.DescribeWalletsByUserIdResult>();
    #else
		public DescribeWalletsByUserIdTask DescribeWalletsByUserIdAsync(
                Request.DescribeWalletsByUserIdRequest request
        )
		{
			return new DescribeWalletsByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.DescribeWalletsByUserIdResult> DescribeWalletsByUserIdAsync(
                Request.DescribeWalletsByUserIdRequest request
        ) =>
            new DescribeWalletsByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class GetWalletTask : Gs2RestSessionTask<GetWalletRequest, GetWalletResult>
        {
            public GetWalletTask(IGs2Session session, RestSessionRequestFactory factory, GetWalletRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(GetWalletRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "money2")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/me/wallet/{slot}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{slot}",request.Slot != null ? request.Slot.ToString() : "null");

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
		public IEnumerator GetWallet(
                Request.GetWalletRequest request,
                UnityAction<AsyncResult<Result.GetWalletResult>> callback
        ) =>
            new GetWalletTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.GetWalletResult> GetWalletFuture(
                Request.GetWalletRequest request
        ) =>
            new GetWalletTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.GetWalletResult> GetWalletAsync(
                Request.GetWalletRequest request
        ) =>
            new GetWalletTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.GetWalletResult>();
    #else
		public GetWalletTask GetWalletAsync(
                Request.GetWalletRequest request
        )
		{
			return new GetWalletTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.GetWalletResult> GetWalletAsync(
                Request.GetWalletRequest request
        ) =>
            new GetWalletTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class GetWalletByUserIdTask : Gs2RestSessionTask<GetWalletByUserIdRequest, GetWalletByUserIdResult>
        {
            public GetWalletByUserIdTask(IGs2Session session, RestSessionRequestFactory factory, GetWalletByUserIdRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(GetWalletByUserIdRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "money2")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/{userId}/wallet/{slot}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{userId}", !string.IsNullOrEmpty(request.UserId) ? request.UserId.ToString() : "null");
                url = url.Replace("{slot}",request.Slot != null ? request.Slot.ToString() : "null");

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
		public IEnumerator GetWalletByUserId(
                Request.GetWalletByUserIdRequest request,
                UnityAction<AsyncResult<Result.GetWalletByUserIdResult>> callback
        ) =>
            new GetWalletByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.GetWalletByUserIdResult> GetWalletByUserIdFuture(
                Request.GetWalletByUserIdRequest request
        ) =>
            new GetWalletByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.GetWalletByUserIdResult> GetWalletByUserIdAsync(
                Request.GetWalletByUserIdRequest request
        ) =>
            new GetWalletByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.GetWalletByUserIdResult>();
    #else
		public GetWalletByUserIdTask GetWalletByUserIdAsync(
                Request.GetWalletByUserIdRequest request
        )
		{
			return new GetWalletByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.GetWalletByUserIdResult> GetWalletByUserIdAsync(
                Request.GetWalletByUserIdRequest request
        ) =>
            new GetWalletByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class DepositByUserIdTask : Gs2RestSessionTask<DepositByUserIdRequest, DepositByUserIdResult>
        {
            public DepositByUserIdTask(IGs2Session session, RestSessionRequestFactory factory, DepositByUserIdRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DepositByUserIdRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "money2")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/{userId}/wallet/{slot}/deposit";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{userId}", !string.IsNullOrEmpty(request.UserId) ? request.UserId.ToString() : "null");
                url = url.Replace("{slot}",request.Slot != null ? request.Slot.ToString() : "null");

                var sessionRequest = Factory.Post(url);

                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);
                jsonWriter.WriteObjectStart();
                if (request.DepositTransactions != null)
                {
                    jsonWriter.WritePropertyName("depositTransactions");
                    jsonWriter.WriteArrayStart();
                    foreach(var item in request.DepositTransactions)
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
                if (error.Errors.Count(v => v.code == "wallet.operation.conflict") > 0) {
                    base.OnError(new Exception.ConflictException(error));
                }
                else {
                    base.OnError(error);
                }
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator DepositByUserId(
                Request.DepositByUserIdRequest request,
                UnityAction<AsyncResult<Result.DepositByUserIdResult>> callback
        ) =>
            new DepositByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.DepositByUserIdResult> DepositByUserIdFuture(
                Request.DepositByUserIdRequest request
        ) =>
            new DepositByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.DepositByUserIdResult> DepositByUserIdAsync(
                Request.DepositByUserIdRequest request
        ) =>
            new DepositByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.DepositByUserIdResult>();
    #else
		public DepositByUserIdTask DepositByUserIdAsync(
                Request.DepositByUserIdRequest request
        )
		{
			return new DepositByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.DepositByUserIdResult> DepositByUserIdAsync(
                Request.DepositByUserIdRequest request
        ) =>
            new DepositByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class WithdrawTask : Gs2RestSessionTask<WithdrawRequest, WithdrawResult>
        {
            public WithdrawTask(IGs2Session session, RestSessionRequestFactory factory, WithdrawRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(WithdrawRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "money2")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/me/wallet/{slot}/withdraw";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{slot}",request.Slot != null ? request.Slot.ToString() : "null");

                var sessionRequest = Factory.Post(url);

                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);
                jsonWriter.WriteObjectStart();
                if (request.WithdrawCount != null)
                {
                    jsonWriter.WritePropertyName("withdrawCount");
                    jsonWriter.Write(request.WithdrawCount.ToString());
                }
                if (request.PaidOnly != null)
                {
                    jsonWriter.WritePropertyName("paidOnly");
                    jsonWriter.Write(request.PaidOnly.ToString());
                }
                if (request.ContextStack != null)
                {
                    jsonWriter.WritePropertyName("contextStack");
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
                if (error.Errors.Count(v => v.code == "wallet.operation.conflict") > 0) {
                    base.OnError(new Exception.ConflictException(error));
                }
                else if (error.Errors.Count(v => v.code == "wallet.balance.insufficient") > 0) {
                    base.OnError(new Exception.InsufficientException(error));
                }
                else {
                    base.OnError(error);
                }
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator Withdraw(
                Request.WithdrawRequest request,
                UnityAction<AsyncResult<Result.WithdrawResult>> callback
        ) =>
            new WithdrawTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.WithdrawResult> WithdrawFuture(
                Request.WithdrawRequest request
        ) =>
            new WithdrawTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.WithdrawResult> WithdrawAsync(
                Request.WithdrawRequest request
        ) =>
            new WithdrawTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.WithdrawResult>();
    #else
		public WithdrawTask WithdrawAsync(
                Request.WithdrawRequest request
        )
		{
			return new WithdrawTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.WithdrawResult> WithdrawAsync(
                Request.WithdrawRequest request
        ) =>
            new WithdrawTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class WithdrawByUserIdTask : Gs2RestSessionTask<WithdrawByUserIdRequest, WithdrawByUserIdResult>
        {
            public WithdrawByUserIdTask(IGs2Session session, RestSessionRequestFactory factory, WithdrawByUserIdRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(WithdrawByUserIdRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "money2")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/{userId}/wallet/{slot}/withdraw";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{userId}", !string.IsNullOrEmpty(request.UserId) ? request.UserId.ToString() : "null");
                url = url.Replace("{slot}",request.Slot != null ? request.Slot.ToString() : "null");

                var sessionRequest = Factory.Post(url);

                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);
                jsonWriter.WriteObjectStart();
                if (request.WithdrawCount != null)
                {
                    jsonWriter.WritePropertyName("withdrawCount");
                    jsonWriter.Write(request.WithdrawCount.ToString());
                }
                if (request.PaidOnly != null)
                {
                    jsonWriter.WritePropertyName("paidOnly");
                    jsonWriter.Write(request.PaidOnly.ToString());
                }
                if (request.ContextStack != null)
                {
                    jsonWriter.WritePropertyName("contextStack");
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
                if (error.Errors.Count(v => v.code == "wallet.operation.conflict") > 0) {
                    base.OnError(new Exception.ConflictException(error));
                }
                else if (error.Errors.Count(v => v.code == "wallet.balance.insufficient") > 0) {
                    base.OnError(new Exception.InsufficientException(error));
                }
                else {
                    base.OnError(error);
                }
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator WithdrawByUserId(
                Request.WithdrawByUserIdRequest request,
                UnityAction<AsyncResult<Result.WithdrawByUserIdResult>> callback
        ) =>
            new WithdrawByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.WithdrawByUserIdResult> WithdrawByUserIdFuture(
                Request.WithdrawByUserIdRequest request
        ) =>
            new WithdrawByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.WithdrawByUserIdResult> WithdrawByUserIdAsync(
                Request.WithdrawByUserIdRequest request
        ) =>
            new WithdrawByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.WithdrawByUserIdResult>();
    #else
		public WithdrawByUserIdTask WithdrawByUserIdAsync(
                Request.WithdrawByUserIdRequest request
        )
		{
			return new WithdrawByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.WithdrawByUserIdResult> WithdrawByUserIdAsync(
                Request.WithdrawByUserIdRequest request
        ) =>
            new WithdrawByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class DepositByStampSheetTask : Gs2RestSessionTask<DepositByStampSheetRequest, DepositByStampSheetResult>
        {
            public DepositByStampSheetTask(IGs2Session session, RestSessionRequestFactory factory, DepositByStampSheetRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DepositByStampSheetRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "money2")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/stamp/deposit";

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
		public IEnumerator DepositByStampSheet(
                Request.DepositByStampSheetRequest request,
                UnityAction<AsyncResult<Result.DepositByStampSheetResult>> callback
        ) =>
            new DepositByStampSheetTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.DepositByStampSheetResult> DepositByStampSheetFuture(
                Request.DepositByStampSheetRequest request
        ) =>
            new DepositByStampSheetTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.DepositByStampSheetResult> DepositByStampSheetAsync(
                Request.DepositByStampSheetRequest request
        ) =>
            new DepositByStampSheetTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.DepositByStampSheetResult>();
    #else
		public DepositByStampSheetTask DepositByStampSheetAsync(
                Request.DepositByStampSheetRequest request
        )
		{
			return new DepositByStampSheetTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.DepositByStampSheetResult> DepositByStampSheetAsync(
                Request.DepositByStampSheetRequest request
        ) =>
            new DepositByStampSheetTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class WithdrawByStampTaskTask : Gs2RestSessionTask<WithdrawByStampTaskRequest, WithdrawByStampTaskResult>
        {
            public WithdrawByStampTaskTask(IGs2Session session, RestSessionRequestFactory factory, WithdrawByStampTaskRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(WithdrawByStampTaskRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "money2")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/stamp/withdraw";

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
		public IEnumerator WithdrawByStampTask(
                Request.WithdrawByStampTaskRequest request,
                UnityAction<AsyncResult<Result.WithdrawByStampTaskResult>> callback
        ) =>
            new WithdrawByStampTaskTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.WithdrawByStampTaskResult> WithdrawByStampTaskFuture(
                Request.WithdrawByStampTaskRequest request
        ) =>
            new WithdrawByStampTaskTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.WithdrawByStampTaskResult> WithdrawByStampTaskAsync(
                Request.WithdrawByStampTaskRequest request
        ) =>
            new WithdrawByStampTaskTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.WithdrawByStampTaskResult>();
    #else
		public WithdrawByStampTaskTask WithdrawByStampTaskAsync(
                Request.WithdrawByStampTaskRequest request
        )
		{
			return new WithdrawByStampTaskTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.WithdrawByStampTaskResult> WithdrawByStampTaskAsync(
                Request.WithdrawByStampTaskRequest request
        ) =>
            new WithdrawByStampTaskTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class DescribeEventsByUserIdTask : Gs2RestSessionTask<DescribeEventsByUserIdRequest, DescribeEventsByUserIdResult>
        {
            public DescribeEventsByUserIdTask(IGs2Session session, RestSessionRequestFactory factory, DescribeEventsByUserIdRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DescribeEventsByUserIdRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "money2")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/event/user/{userId}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{userId}", !string.IsNullOrEmpty(request.UserId) ? request.UserId.ToString() : "null");

                var sessionRequest = Factory.Get(url);
                if (request.ContextStack != null)
                {
                    sessionRequest.AddQueryString("contextStack", request.ContextStack);
                }
                if (request.Begin != null) {
                    sessionRequest.AddQueryString("begin", $"{request.Begin}");
                }
                if (request.End != null) {
                    sessionRequest.AddQueryString("end", $"{request.End}");
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
		public IEnumerator DescribeEventsByUserId(
                Request.DescribeEventsByUserIdRequest request,
                UnityAction<AsyncResult<Result.DescribeEventsByUserIdResult>> callback
        ) =>
            new DescribeEventsByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.DescribeEventsByUserIdResult> DescribeEventsByUserIdFuture(
                Request.DescribeEventsByUserIdRequest request
        ) =>
            new DescribeEventsByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.DescribeEventsByUserIdResult> DescribeEventsByUserIdAsync(
                Request.DescribeEventsByUserIdRequest request
        ) =>
            new DescribeEventsByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.DescribeEventsByUserIdResult>();
    #else
		public DescribeEventsByUserIdTask DescribeEventsByUserIdAsync(
                Request.DescribeEventsByUserIdRequest request
        )
		{
			return new DescribeEventsByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.DescribeEventsByUserIdResult> DescribeEventsByUserIdAsync(
                Request.DescribeEventsByUserIdRequest request
        ) =>
            new DescribeEventsByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class GetEventByTransactionIdTask : Gs2RestSessionTask<GetEventByTransactionIdRequest, GetEventByTransactionIdResult>
        {
            public GetEventByTransactionIdTask(IGs2Session session, RestSessionRequestFactory factory, GetEventByTransactionIdRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(GetEventByTransactionIdRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "money2")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/event/{transactionId}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{transactionId}", !string.IsNullOrEmpty(request.TransactionId) ? request.TransactionId.ToString() : "null");

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
		public IEnumerator GetEventByTransactionId(
                Request.GetEventByTransactionIdRequest request,
                UnityAction<AsyncResult<Result.GetEventByTransactionIdResult>> callback
        ) =>
            new GetEventByTransactionIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.GetEventByTransactionIdResult> GetEventByTransactionIdFuture(
                Request.GetEventByTransactionIdRequest request
        ) =>
            new GetEventByTransactionIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.GetEventByTransactionIdResult> GetEventByTransactionIdAsync(
                Request.GetEventByTransactionIdRequest request
        ) =>
            new GetEventByTransactionIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.GetEventByTransactionIdResult>();
    #else
		public GetEventByTransactionIdTask GetEventByTransactionIdAsync(
                Request.GetEventByTransactionIdRequest request
        )
		{
			return new GetEventByTransactionIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.GetEventByTransactionIdResult> GetEventByTransactionIdAsync(
                Request.GetEventByTransactionIdRequest request
        ) =>
            new GetEventByTransactionIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class VerifyReceiptTask : Gs2RestSessionTask<VerifyReceiptRequest, VerifyReceiptResult>
        {
            public VerifyReceiptTask(IGs2Session session, RestSessionRequestFactory factory, VerifyReceiptRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(VerifyReceiptRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "money2")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/me/content/{contentName}/receipt/verify";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{contentName}", !string.IsNullOrEmpty(request.ContentName) ? request.ContentName.ToString() : "null");

                var sessionRequest = Factory.Post(url);

                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);
                jsonWriter.WriteObjectStart();
                if (request.Receipt != null)
                {
                    jsonWriter.WritePropertyName("receipt");
                    request.Receipt.WriteJson(jsonWriter);
                }
                if (request.ContextStack != null)
                {
                    jsonWriter.WritePropertyName("contextStack");
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
                if (error.Errors.Count(v => v.code == "receipt.payload.invalid") > 0) {
                    base.OnError(new Exception.ReceiptInvalidException(error));
                }
                else {
                    base.OnError(error);
                }
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator VerifyReceipt(
                Request.VerifyReceiptRequest request,
                UnityAction<AsyncResult<Result.VerifyReceiptResult>> callback
        ) =>
            new VerifyReceiptTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.VerifyReceiptResult> VerifyReceiptFuture(
                Request.VerifyReceiptRequest request
        ) =>
            new VerifyReceiptTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.VerifyReceiptResult> VerifyReceiptAsync(
                Request.VerifyReceiptRequest request
        ) =>
            new VerifyReceiptTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.VerifyReceiptResult>();
    #else
		public VerifyReceiptTask VerifyReceiptAsync(
                Request.VerifyReceiptRequest request
        )
		{
			return new VerifyReceiptTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.VerifyReceiptResult> VerifyReceiptAsync(
                Request.VerifyReceiptRequest request
        ) =>
            new VerifyReceiptTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class VerifyReceiptByUserIdTask : Gs2RestSessionTask<VerifyReceiptByUserIdRequest, VerifyReceiptByUserIdResult>
        {
            public VerifyReceiptByUserIdTask(IGs2Session session, RestSessionRequestFactory factory, VerifyReceiptByUserIdRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(VerifyReceiptByUserIdRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "money2")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/{userId}/content/{contentName}/receipt/verify";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{userId}", !string.IsNullOrEmpty(request.UserId) ? request.UserId.ToString() : "null");
                url = url.Replace("{contentName}", !string.IsNullOrEmpty(request.ContentName) ? request.ContentName.ToString() : "null");

                var sessionRequest = Factory.Post(url);

                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);
                jsonWriter.WriteObjectStart();
                if (request.Receipt != null)
                {
                    jsonWriter.WritePropertyName("receipt");
                    request.Receipt.WriteJson(jsonWriter);
                }
                if (request.ContextStack != null)
                {
                    jsonWriter.WritePropertyName("contextStack");
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
                if (error.Errors.Count(v => v.code == "receipt.payload.invalid") > 0) {
                    base.OnError(new Exception.ReceiptInvalidException(error));
                }
                else {
                    base.OnError(error);
                }
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator VerifyReceiptByUserId(
                Request.VerifyReceiptByUserIdRequest request,
                UnityAction<AsyncResult<Result.VerifyReceiptByUserIdResult>> callback
        ) =>
            new VerifyReceiptByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.VerifyReceiptByUserIdResult> VerifyReceiptByUserIdFuture(
                Request.VerifyReceiptByUserIdRequest request
        ) =>
            new VerifyReceiptByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.VerifyReceiptByUserIdResult> VerifyReceiptByUserIdAsync(
                Request.VerifyReceiptByUserIdRequest request
        ) =>
            new VerifyReceiptByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.VerifyReceiptByUserIdResult>();
    #else
		public VerifyReceiptByUserIdTask VerifyReceiptByUserIdAsync(
                Request.VerifyReceiptByUserIdRequest request
        )
		{
			return new VerifyReceiptByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.VerifyReceiptByUserIdResult> VerifyReceiptByUserIdAsync(
                Request.VerifyReceiptByUserIdRequest request
        ) =>
            new VerifyReceiptByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class VerifyReceiptByStampTaskTask : Gs2RestSessionTask<VerifyReceiptByStampTaskRequest, VerifyReceiptByStampTaskResult>
        {
            public VerifyReceiptByStampTaskTask(IGs2Session session, RestSessionRequestFactory factory, VerifyReceiptByStampTaskRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(VerifyReceiptByStampTaskRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "money2")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/stamp/receipt/verify";

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
		public IEnumerator VerifyReceiptByStampTask(
                Request.VerifyReceiptByStampTaskRequest request,
                UnityAction<AsyncResult<Result.VerifyReceiptByStampTaskResult>> callback
        ) =>
            new VerifyReceiptByStampTaskTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.VerifyReceiptByStampTaskResult> VerifyReceiptByStampTaskFuture(
                Request.VerifyReceiptByStampTaskRequest request
        ) =>
            new VerifyReceiptByStampTaskTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.VerifyReceiptByStampTaskResult> VerifyReceiptByStampTaskAsync(
                Request.VerifyReceiptByStampTaskRequest request
        ) =>
            new VerifyReceiptByStampTaskTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.VerifyReceiptByStampTaskResult>();
    #else
		public VerifyReceiptByStampTaskTask VerifyReceiptByStampTaskAsync(
                Request.VerifyReceiptByStampTaskRequest request
        )
		{
			return new VerifyReceiptByStampTaskTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.VerifyReceiptByStampTaskResult> VerifyReceiptByStampTaskAsync(
                Request.VerifyReceiptByStampTaskRequest request
        ) =>
            new VerifyReceiptByStampTaskTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class DescribeSubscriptionStatusesTask : Gs2RestSessionTask<DescribeSubscriptionStatusesRequest, DescribeSubscriptionStatusesResult>
        {
            public DescribeSubscriptionStatusesTask(IGs2Session session, RestSessionRequestFactory factory, DescribeSubscriptionStatusesRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DescribeSubscriptionStatusesRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "money2")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/me/subscription";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");

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
		public IEnumerator DescribeSubscriptionStatuses(
                Request.DescribeSubscriptionStatusesRequest request,
                UnityAction<AsyncResult<Result.DescribeSubscriptionStatusesResult>> callback
        ) =>
            new DescribeSubscriptionStatusesTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.DescribeSubscriptionStatusesResult> DescribeSubscriptionStatusesFuture(
                Request.DescribeSubscriptionStatusesRequest request
        ) =>
            new DescribeSubscriptionStatusesTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.DescribeSubscriptionStatusesResult> DescribeSubscriptionStatusesAsync(
                Request.DescribeSubscriptionStatusesRequest request
        ) =>
            new DescribeSubscriptionStatusesTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.DescribeSubscriptionStatusesResult>();
    #else
		public DescribeSubscriptionStatusesTask DescribeSubscriptionStatusesAsync(
                Request.DescribeSubscriptionStatusesRequest request
        )
		{
			return new DescribeSubscriptionStatusesTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.DescribeSubscriptionStatusesResult> DescribeSubscriptionStatusesAsync(
                Request.DescribeSubscriptionStatusesRequest request
        ) =>
            new DescribeSubscriptionStatusesTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class DescribeSubscriptionStatusesByUserIdTask : Gs2RestSessionTask<DescribeSubscriptionStatusesByUserIdRequest, DescribeSubscriptionStatusesByUserIdResult>
        {
            public DescribeSubscriptionStatusesByUserIdTask(IGs2Session session, RestSessionRequestFactory factory, DescribeSubscriptionStatusesByUserIdRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DescribeSubscriptionStatusesByUserIdRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "money2")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/{userId}/subscription";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
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
		public IEnumerator DescribeSubscriptionStatusesByUserId(
                Request.DescribeSubscriptionStatusesByUserIdRequest request,
                UnityAction<AsyncResult<Result.DescribeSubscriptionStatusesByUserIdResult>> callback
        ) =>
            new DescribeSubscriptionStatusesByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.DescribeSubscriptionStatusesByUserIdResult> DescribeSubscriptionStatusesByUserIdFuture(
                Request.DescribeSubscriptionStatusesByUserIdRequest request
        ) =>
            new DescribeSubscriptionStatusesByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.DescribeSubscriptionStatusesByUserIdResult> DescribeSubscriptionStatusesByUserIdAsync(
                Request.DescribeSubscriptionStatusesByUserIdRequest request
        ) =>
            new DescribeSubscriptionStatusesByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.DescribeSubscriptionStatusesByUserIdResult>();
    #else
		public DescribeSubscriptionStatusesByUserIdTask DescribeSubscriptionStatusesByUserIdAsync(
                Request.DescribeSubscriptionStatusesByUserIdRequest request
        )
		{
			return new DescribeSubscriptionStatusesByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.DescribeSubscriptionStatusesByUserIdResult> DescribeSubscriptionStatusesByUserIdAsync(
                Request.DescribeSubscriptionStatusesByUserIdRequest request
        ) =>
            new DescribeSubscriptionStatusesByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class GetSubscriptionStatusTask : Gs2RestSessionTask<GetSubscriptionStatusRequest, GetSubscriptionStatusResult>
        {
            public GetSubscriptionStatusTask(IGs2Session session, RestSessionRequestFactory factory, GetSubscriptionStatusRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(GetSubscriptionStatusRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "money2")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/me/subscription/{contentName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{contentName}", !string.IsNullOrEmpty(request.ContentName) ? request.ContentName.ToString() : "null");

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
		public IEnumerator GetSubscriptionStatus(
                Request.GetSubscriptionStatusRequest request,
                UnityAction<AsyncResult<Result.GetSubscriptionStatusResult>> callback
        ) =>
            new GetSubscriptionStatusTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.GetSubscriptionStatusResult> GetSubscriptionStatusFuture(
                Request.GetSubscriptionStatusRequest request
        ) =>
            new GetSubscriptionStatusTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.GetSubscriptionStatusResult> GetSubscriptionStatusAsync(
                Request.GetSubscriptionStatusRequest request
        ) =>
            new GetSubscriptionStatusTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.GetSubscriptionStatusResult>();
    #else
		public GetSubscriptionStatusTask GetSubscriptionStatusAsync(
                Request.GetSubscriptionStatusRequest request
        )
		{
			return new GetSubscriptionStatusTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.GetSubscriptionStatusResult> GetSubscriptionStatusAsync(
                Request.GetSubscriptionStatusRequest request
        ) =>
            new GetSubscriptionStatusTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class GetSubscriptionStatusByUserIdTask : Gs2RestSessionTask<GetSubscriptionStatusByUserIdRequest, GetSubscriptionStatusByUserIdResult>
        {
            public GetSubscriptionStatusByUserIdTask(IGs2Session session, RestSessionRequestFactory factory, GetSubscriptionStatusByUserIdRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(GetSubscriptionStatusByUserIdRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "money2")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/{userId}/subscription/{contentName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{userId}", !string.IsNullOrEmpty(request.UserId) ? request.UserId.ToString() : "null");
                url = url.Replace("{contentName}", !string.IsNullOrEmpty(request.ContentName) ? request.ContentName.ToString() : "null");

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
		public IEnumerator GetSubscriptionStatusByUserId(
                Request.GetSubscriptionStatusByUserIdRequest request,
                UnityAction<AsyncResult<Result.GetSubscriptionStatusByUserIdResult>> callback
        ) =>
            new GetSubscriptionStatusByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.GetSubscriptionStatusByUserIdResult> GetSubscriptionStatusByUserIdFuture(
                Request.GetSubscriptionStatusByUserIdRequest request
        ) =>
            new GetSubscriptionStatusByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.GetSubscriptionStatusByUserIdResult> GetSubscriptionStatusByUserIdAsync(
                Request.GetSubscriptionStatusByUserIdRequest request
        ) =>
            new GetSubscriptionStatusByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.GetSubscriptionStatusByUserIdResult>();
    #else
		public GetSubscriptionStatusByUserIdTask GetSubscriptionStatusByUserIdAsync(
                Request.GetSubscriptionStatusByUserIdRequest request
        )
		{
			return new GetSubscriptionStatusByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.GetSubscriptionStatusByUserIdResult> GetSubscriptionStatusByUserIdAsync(
                Request.GetSubscriptionStatusByUserIdRequest request
        ) =>
            new GetSubscriptionStatusByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class AllocateSubscriptionStatusTask : Gs2RestSessionTask<AllocateSubscriptionStatusRequest, AllocateSubscriptionStatusResult>
        {
            public AllocateSubscriptionStatusTask(IGs2Session session, RestSessionRequestFactory factory, AllocateSubscriptionStatusRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(AllocateSubscriptionStatusRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "money2")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/me/allocate/subscription";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");

                var sessionRequest = Factory.Post(url);

                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);
                jsonWriter.WriteObjectStart();
                if (request.Receipt != null)
                {
                    jsonWriter.WritePropertyName("receipt");
                    jsonWriter.Write(request.Receipt);
                }
                if (request.ContextStack != null)
                {
                    jsonWriter.WritePropertyName("contextStack");
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
                if (error.Errors.Count(v => v.code == "subscription.transaction.used") > 0) {
                    base.OnError(new Exception.AlreadyUsedException(error));
                }
                else {
                    base.OnError(error);
                }
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator AllocateSubscriptionStatus(
                Request.AllocateSubscriptionStatusRequest request,
                UnityAction<AsyncResult<Result.AllocateSubscriptionStatusResult>> callback
        ) =>
            new AllocateSubscriptionStatusTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.AllocateSubscriptionStatusResult> AllocateSubscriptionStatusFuture(
                Request.AllocateSubscriptionStatusRequest request
        ) =>
            new AllocateSubscriptionStatusTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.AllocateSubscriptionStatusResult> AllocateSubscriptionStatusAsync(
                Request.AllocateSubscriptionStatusRequest request
        ) =>
            new AllocateSubscriptionStatusTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.AllocateSubscriptionStatusResult>();
    #else
		public AllocateSubscriptionStatusTask AllocateSubscriptionStatusAsync(
                Request.AllocateSubscriptionStatusRequest request
        )
		{
			return new AllocateSubscriptionStatusTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.AllocateSubscriptionStatusResult> AllocateSubscriptionStatusAsync(
                Request.AllocateSubscriptionStatusRequest request
        ) =>
            new AllocateSubscriptionStatusTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class AllocateSubscriptionStatusByUserIdTask : Gs2RestSessionTask<AllocateSubscriptionStatusByUserIdRequest, AllocateSubscriptionStatusByUserIdResult>
        {
            public AllocateSubscriptionStatusByUserIdTask(IGs2Session session, RestSessionRequestFactory factory, AllocateSubscriptionStatusByUserIdRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(AllocateSubscriptionStatusByUserIdRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "money2")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/{userId}/allocate/subscription";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{userId}", !string.IsNullOrEmpty(request.UserId) ? request.UserId.ToString() : "null");

                var sessionRequest = Factory.Post(url);

                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);
                jsonWriter.WriteObjectStart();
                if (request.Receipt != null)
                {
                    jsonWriter.WritePropertyName("receipt");
                    jsonWriter.Write(request.Receipt);
                }
                if (request.ContextStack != null)
                {
                    jsonWriter.WritePropertyName("contextStack");
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
                if (error.Errors.Count(v => v.code == "subscription.transaction.used") > 0) {
                    base.OnError(new Exception.AlreadyUsedException(error));
                }
                else {
                    base.OnError(error);
                }
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator AllocateSubscriptionStatusByUserId(
                Request.AllocateSubscriptionStatusByUserIdRequest request,
                UnityAction<AsyncResult<Result.AllocateSubscriptionStatusByUserIdResult>> callback
        ) =>
            new AllocateSubscriptionStatusByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.AllocateSubscriptionStatusByUserIdResult> AllocateSubscriptionStatusByUserIdFuture(
                Request.AllocateSubscriptionStatusByUserIdRequest request
        ) =>
            new AllocateSubscriptionStatusByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.AllocateSubscriptionStatusByUserIdResult> AllocateSubscriptionStatusByUserIdAsync(
                Request.AllocateSubscriptionStatusByUserIdRequest request
        ) =>
            new AllocateSubscriptionStatusByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.AllocateSubscriptionStatusByUserIdResult>();
    #else
		public AllocateSubscriptionStatusByUserIdTask AllocateSubscriptionStatusByUserIdAsync(
                Request.AllocateSubscriptionStatusByUserIdRequest request
        )
		{
			return new AllocateSubscriptionStatusByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.AllocateSubscriptionStatusByUserIdResult> AllocateSubscriptionStatusByUserIdAsync(
                Request.AllocateSubscriptionStatusByUserIdRequest request
        ) =>
            new AllocateSubscriptionStatusByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class TakeoverSubscriptionStatusTask : Gs2RestSessionTask<TakeoverSubscriptionStatusRequest, TakeoverSubscriptionStatusResult>
        {
            public TakeoverSubscriptionStatusTask(IGs2Session session, RestSessionRequestFactory factory, TakeoverSubscriptionStatusRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(TakeoverSubscriptionStatusRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "money2")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/me/takeover/subscription";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");

                var sessionRequest = Factory.Post(url);

                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);
                jsonWriter.WriteObjectStart();
                if (request.Receipt != null)
                {
                    jsonWriter.WritePropertyName("receipt");
                    jsonWriter.Write(request.Receipt);
                }
                if (request.ContextStack != null)
                {
                    jsonWriter.WritePropertyName("contextStack");
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
                if (error.Errors.Count(v => v.code == "subscription.transaction.used") > 0) {
                    base.OnError(new Exception.LockPeriodNotElapsedException(error));
                }
                else {
                    base.OnError(error);
                }
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator TakeoverSubscriptionStatus(
                Request.TakeoverSubscriptionStatusRequest request,
                UnityAction<AsyncResult<Result.TakeoverSubscriptionStatusResult>> callback
        ) =>
            new TakeoverSubscriptionStatusTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.TakeoverSubscriptionStatusResult> TakeoverSubscriptionStatusFuture(
                Request.TakeoverSubscriptionStatusRequest request
        ) =>
            new TakeoverSubscriptionStatusTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.TakeoverSubscriptionStatusResult> TakeoverSubscriptionStatusAsync(
                Request.TakeoverSubscriptionStatusRequest request
        ) =>
            new TakeoverSubscriptionStatusTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.TakeoverSubscriptionStatusResult>();
    #else
		public TakeoverSubscriptionStatusTask TakeoverSubscriptionStatusAsync(
                Request.TakeoverSubscriptionStatusRequest request
        )
		{
			return new TakeoverSubscriptionStatusTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.TakeoverSubscriptionStatusResult> TakeoverSubscriptionStatusAsync(
                Request.TakeoverSubscriptionStatusRequest request
        ) =>
            new TakeoverSubscriptionStatusTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class TakeoverSubscriptionStatusByUserIdTask : Gs2RestSessionTask<TakeoverSubscriptionStatusByUserIdRequest, TakeoverSubscriptionStatusByUserIdResult>
        {
            public TakeoverSubscriptionStatusByUserIdTask(IGs2Session session, RestSessionRequestFactory factory, TakeoverSubscriptionStatusByUserIdRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(TakeoverSubscriptionStatusByUserIdRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "money2")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/{userId}/takeover/subscription";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{userId}", !string.IsNullOrEmpty(request.UserId) ? request.UserId.ToString() : "null");

                var sessionRequest = Factory.Post(url);

                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);
                jsonWriter.WriteObjectStart();
                if (request.Receipt != null)
                {
                    jsonWriter.WritePropertyName("receipt");
                    jsonWriter.Write(request.Receipt);
                }
                if (request.ContextStack != null)
                {
                    jsonWriter.WritePropertyName("contextStack");
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
                if (error.Errors.Count(v => v.code == "subscription.transaction.used") > 0) {
                    base.OnError(new Exception.LockPeriodNotElapsedException(error));
                }
                else {
                    base.OnError(error);
                }
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator TakeoverSubscriptionStatusByUserId(
                Request.TakeoverSubscriptionStatusByUserIdRequest request,
                UnityAction<AsyncResult<Result.TakeoverSubscriptionStatusByUserIdResult>> callback
        ) =>
            new TakeoverSubscriptionStatusByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.TakeoverSubscriptionStatusByUserIdResult> TakeoverSubscriptionStatusByUserIdFuture(
                Request.TakeoverSubscriptionStatusByUserIdRequest request
        ) =>
            new TakeoverSubscriptionStatusByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.TakeoverSubscriptionStatusByUserIdResult> TakeoverSubscriptionStatusByUserIdAsync(
                Request.TakeoverSubscriptionStatusByUserIdRequest request
        ) =>
            new TakeoverSubscriptionStatusByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.TakeoverSubscriptionStatusByUserIdResult>();
    #else
		public TakeoverSubscriptionStatusByUserIdTask TakeoverSubscriptionStatusByUserIdAsync(
                Request.TakeoverSubscriptionStatusByUserIdRequest request
        )
		{
			return new TakeoverSubscriptionStatusByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.TakeoverSubscriptionStatusByUserIdResult> TakeoverSubscriptionStatusByUserIdAsync(
                Request.TakeoverSubscriptionStatusByUserIdRequest request
        ) =>
            new TakeoverSubscriptionStatusByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class DescribeRefundHistoriesByUserIdTask : Gs2RestSessionTask<DescribeRefundHistoriesByUserIdRequest, DescribeRefundHistoriesByUserIdResult>
        {
            public DescribeRefundHistoriesByUserIdTask(IGs2Session session, RestSessionRequestFactory factory, DescribeRefundHistoriesByUserIdRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DescribeRefundHistoriesByUserIdRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "money2")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/refund/user/{userId}";

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
		public IEnumerator DescribeRefundHistoriesByUserId(
                Request.DescribeRefundHistoriesByUserIdRequest request,
                UnityAction<AsyncResult<Result.DescribeRefundHistoriesByUserIdResult>> callback
        ) =>
            new DescribeRefundHistoriesByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.DescribeRefundHistoriesByUserIdResult> DescribeRefundHistoriesByUserIdFuture(
                Request.DescribeRefundHistoriesByUserIdRequest request
        ) =>
            new DescribeRefundHistoriesByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.DescribeRefundHistoriesByUserIdResult> DescribeRefundHistoriesByUserIdAsync(
                Request.DescribeRefundHistoriesByUserIdRequest request
        ) =>
            new DescribeRefundHistoriesByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.DescribeRefundHistoriesByUserIdResult>();
    #else
		public DescribeRefundHistoriesByUserIdTask DescribeRefundHistoriesByUserIdAsync(
                Request.DescribeRefundHistoriesByUserIdRequest request
        )
		{
			return new DescribeRefundHistoriesByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.DescribeRefundHistoriesByUserIdResult> DescribeRefundHistoriesByUserIdAsync(
                Request.DescribeRefundHistoriesByUserIdRequest request
        ) =>
            new DescribeRefundHistoriesByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class DescribeRefundHistoriesByDateTask : Gs2RestSessionTask<DescribeRefundHistoriesByDateRequest, DescribeRefundHistoriesByDateResult>
        {
            public DescribeRefundHistoriesByDateTask(IGs2Session session, RestSessionRequestFactory factory, DescribeRefundHistoriesByDateRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DescribeRefundHistoriesByDateRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "money2")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/refund/date/{year}/{month}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{year}",request.Year != null ? request.Year.ToString() : "null");
                url = url.Replace("{month}",request.Month != null ? request.Month.ToString() : "null");

                var sessionRequest = Factory.Get(url);
                if (request.ContextStack != null)
                {
                    sessionRequest.AddQueryString("contextStack", request.ContextStack);
                }
                if (request.Day != null) {
                    sessionRequest.AddQueryString("day", $"{request.Day}");
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
		public IEnumerator DescribeRefundHistoriesByDate(
                Request.DescribeRefundHistoriesByDateRequest request,
                UnityAction<AsyncResult<Result.DescribeRefundHistoriesByDateResult>> callback
        ) =>
            new DescribeRefundHistoriesByDateTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.DescribeRefundHistoriesByDateResult> DescribeRefundHistoriesByDateFuture(
                Request.DescribeRefundHistoriesByDateRequest request
        ) =>
            new DescribeRefundHistoriesByDateTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.DescribeRefundHistoriesByDateResult> DescribeRefundHistoriesByDateAsync(
                Request.DescribeRefundHistoriesByDateRequest request
        ) =>
            new DescribeRefundHistoriesByDateTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.DescribeRefundHistoriesByDateResult>();
    #else
		public DescribeRefundHistoriesByDateTask DescribeRefundHistoriesByDateAsync(
                Request.DescribeRefundHistoriesByDateRequest request
        )
		{
			return new DescribeRefundHistoriesByDateTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.DescribeRefundHistoriesByDateResult> DescribeRefundHistoriesByDateAsync(
                Request.DescribeRefundHistoriesByDateRequest request
        ) =>
            new DescribeRefundHistoriesByDateTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class GetRefundHistoryTask : Gs2RestSessionTask<GetRefundHistoryRequest, GetRefundHistoryResult>
        {
            public GetRefundHistoryTask(IGs2Session session, RestSessionRequestFactory factory, GetRefundHistoryRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(GetRefundHistoryRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "money2")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/refund/{transactionId}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{transactionId}", !string.IsNullOrEmpty(request.TransactionId) ? request.TransactionId.ToString() : "null");

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
		public IEnumerator GetRefundHistory(
                Request.GetRefundHistoryRequest request,
                UnityAction<AsyncResult<Result.GetRefundHistoryResult>> callback
        ) =>
            new GetRefundHistoryTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.GetRefundHistoryResult> GetRefundHistoryFuture(
                Request.GetRefundHistoryRequest request
        ) =>
            new GetRefundHistoryTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.GetRefundHistoryResult> GetRefundHistoryAsync(
                Request.GetRefundHistoryRequest request
        ) =>
            new GetRefundHistoryTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.GetRefundHistoryResult>();
    #else
		public GetRefundHistoryTask GetRefundHistoryAsync(
                Request.GetRefundHistoryRequest request
        )
		{
			return new GetRefundHistoryTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.GetRefundHistoryResult> GetRefundHistoryAsync(
                Request.GetRefundHistoryRequest request
        ) =>
            new GetRefundHistoryTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class DescribeStoreContentModelsTask : Gs2RestSessionTask<DescribeStoreContentModelsRequest, DescribeStoreContentModelsResult>
        {
            public DescribeStoreContentModelsTask(IGs2Session session, RestSessionRequestFactory factory, DescribeStoreContentModelsRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DescribeStoreContentModelsRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "money2")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/model/content";

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
		public IEnumerator DescribeStoreContentModels(
                Request.DescribeStoreContentModelsRequest request,
                UnityAction<AsyncResult<Result.DescribeStoreContentModelsResult>> callback
        ) =>
            new DescribeStoreContentModelsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.DescribeStoreContentModelsResult> DescribeStoreContentModelsFuture(
                Request.DescribeStoreContentModelsRequest request
        ) =>
            new DescribeStoreContentModelsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.DescribeStoreContentModelsResult> DescribeStoreContentModelsAsync(
                Request.DescribeStoreContentModelsRequest request
        ) =>
            new DescribeStoreContentModelsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.DescribeStoreContentModelsResult>();
    #else
		public DescribeStoreContentModelsTask DescribeStoreContentModelsAsync(
                Request.DescribeStoreContentModelsRequest request
        )
		{
			return new DescribeStoreContentModelsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.DescribeStoreContentModelsResult> DescribeStoreContentModelsAsync(
                Request.DescribeStoreContentModelsRequest request
        ) =>
            new DescribeStoreContentModelsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class GetStoreContentModelTask : Gs2RestSessionTask<GetStoreContentModelRequest, GetStoreContentModelResult>
        {
            public GetStoreContentModelTask(IGs2Session session, RestSessionRequestFactory factory, GetStoreContentModelRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(GetStoreContentModelRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "money2")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/model/content/{contentName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{contentName}", !string.IsNullOrEmpty(request.ContentName) ? request.ContentName.ToString() : "null");

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
		public IEnumerator GetStoreContentModel(
                Request.GetStoreContentModelRequest request,
                UnityAction<AsyncResult<Result.GetStoreContentModelResult>> callback
        ) =>
            new GetStoreContentModelTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.GetStoreContentModelResult> GetStoreContentModelFuture(
                Request.GetStoreContentModelRequest request
        ) =>
            new GetStoreContentModelTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.GetStoreContentModelResult> GetStoreContentModelAsync(
                Request.GetStoreContentModelRequest request
        ) =>
            new GetStoreContentModelTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.GetStoreContentModelResult>();
    #else
		public GetStoreContentModelTask GetStoreContentModelAsync(
                Request.GetStoreContentModelRequest request
        )
		{
			return new GetStoreContentModelTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.GetStoreContentModelResult> GetStoreContentModelAsync(
                Request.GetStoreContentModelRequest request
        ) =>
            new GetStoreContentModelTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class DescribeStoreContentModelMastersTask : Gs2RestSessionTask<DescribeStoreContentModelMastersRequest, DescribeStoreContentModelMastersResult>
        {
            public DescribeStoreContentModelMastersTask(IGs2Session session, RestSessionRequestFactory factory, DescribeStoreContentModelMastersRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DescribeStoreContentModelMastersRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "money2")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/master/model";

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
		public IEnumerator DescribeStoreContentModelMasters(
                Request.DescribeStoreContentModelMastersRequest request,
                UnityAction<AsyncResult<Result.DescribeStoreContentModelMastersResult>> callback
        ) =>
            new DescribeStoreContentModelMastersTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.DescribeStoreContentModelMastersResult> DescribeStoreContentModelMastersFuture(
                Request.DescribeStoreContentModelMastersRequest request
        ) =>
            new DescribeStoreContentModelMastersTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.DescribeStoreContentModelMastersResult> DescribeStoreContentModelMastersAsync(
                Request.DescribeStoreContentModelMastersRequest request
        ) =>
            new DescribeStoreContentModelMastersTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.DescribeStoreContentModelMastersResult>();
    #else
		public DescribeStoreContentModelMastersTask DescribeStoreContentModelMastersAsync(
                Request.DescribeStoreContentModelMastersRequest request
        )
		{
			return new DescribeStoreContentModelMastersTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.DescribeStoreContentModelMastersResult> DescribeStoreContentModelMastersAsync(
                Request.DescribeStoreContentModelMastersRequest request
        ) =>
            new DescribeStoreContentModelMastersTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class CreateStoreContentModelMasterTask : Gs2RestSessionTask<CreateStoreContentModelMasterRequest, CreateStoreContentModelMasterResult>
        {
            public CreateStoreContentModelMasterTask(IGs2Session session, RestSessionRequestFactory factory, CreateStoreContentModelMasterRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(CreateStoreContentModelMasterRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "money2")
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
                if (request.AppleAppStore != null)
                {
                    jsonWriter.WritePropertyName("appleAppStore");
                    request.AppleAppStore.WriteJson(jsonWriter);
                }
                if (request.GooglePlay != null)
                {
                    jsonWriter.WritePropertyName("googlePlay");
                    request.GooglePlay.WriteJson(jsonWriter);
                }
                if (request.ContextStack != null)
                {
                    jsonWriter.WritePropertyName("contextStack");
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
		public IEnumerator CreateStoreContentModelMaster(
                Request.CreateStoreContentModelMasterRequest request,
                UnityAction<AsyncResult<Result.CreateStoreContentModelMasterResult>> callback
        ) =>
            new CreateStoreContentModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.CreateStoreContentModelMasterResult> CreateStoreContentModelMasterFuture(
                Request.CreateStoreContentModelMasterRequest request
        ) =>
            new CreateStoreContentModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.CreateStoreContentModelMasterResult> CreateStoreContentModelMasterAsync(
                Request.CreateStoreContentModelMasterRequest request
        ) =>
            new CreateStoreContentModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.CreateStoreContentModelMasterResult>();
    #else
		public CreateStoreContentModelMasterTask CreateStoreContentModelMasterAsync(
                Request.CreateStoreContentModelMasterRequest request
        )
		{
			return new CreateStoreContentModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.CreateStoreContentModelMasterResult> CreateStoreContentModelMasterAsync(
                Request.CreateStoreContentModelMasterRequest request
        ) =>
            new CreateStoreContentModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class GetStoreContentModelMasterTask : Gs2RestSessionTask<GetStoreContentModelMasterRequest, GetStoreContentModelMasterResult>
        {
            public GetStoreContentModelMasterTask(IGs2Session session, RestSessionRequestFactory factory, GetStoreContentModelMasterRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(GetStoreContentModelMasterRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "money2")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/master/model/{contentName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{contentName}", !string.IsNullOrEmpty(request.ContentName) ? request.ContentName.ToString() : "null");

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
		public IEnumerator GetStoreContentModelMaster(
                Request.GetStoreContentModelMasterRequest request,
                UnityAction<AsyncResult<Result.GetStoreContentModelMasterResult>> callback
        ) =>
            new GetStoreContentModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.GetStoreContentModelMasterResult> GetStoreContentModelMasterFuture(
                Request.GetStoreContentModelMasterRequest request
        ) =>
            new GetStoreContentModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.GetStoreContentModelMasterResult> GetStoreContentModelMasterAsync(
                Request.GetStoreContentModelMasterRequest request
        ) =>
            new GetStoreContentModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.GetStoreContentModelMasterResult>();
    #else
		public GetStoreContentModelMasterTask GetStoreContentModelMasterAsync(
                Request.GetStoreContentModelMasterRequest request
        )
		{
			return new GetStoreContentModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.GetStoreContentModelMasterResult> GetStoreContentModelMasterAsync(
                Request.GetStoreContentModelMasterRequest request
        ) =>
            new GetStoreContentModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class UpdateStoreContentModelMasterTask : Gs2RestSessionTask<UpdateStoreContentModelMasterRequest, UpdateStoreContentModelMasterResult>
        {
            public UpdateStoreContentModelMasterTask(IGs2Session session, RestSessionRequestFactory factory, UpdateStoreContentModelMasterRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(UpdateStoreContentModelMasterRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "money2")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/master/model/{contentName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{contentName}", !string.IsNullOrEmpty(request.ContentName) ? request.ContentName.ToString() : "null");

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
                if (request.AppleAppStore != null)
                {
                    jsonWriter.WritePropertyName("appleAppStore");
                    request.AppleAppStore.WriteJson(jsonWriter);
                }
                if (request.GooglePlay != null)
                {
                    jsonWriter.WritePropertyName("googlePlay");
                    request.GooglePlay.WriteJson(jsonWriter);
                }
                if (request.ContextStack != null)
                {
                    jsonWriter.WritePropertyName("contextStack");
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
		public IEnumerator UpdateStoreContentModelMaster(
                Request.UpdateStoreContentModelMasterRequest request,
                UnityAction<AsyncResult<Result.UpdateStoreContentModelMasterResult>> callback
        ) =>
            new UpdateStoreContentModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.UpdateStoreContentModelMasterResult> UpdateStoreContentModelMasterFuture(
                Request.UpdateStoreContentModelMasterRequest request
        ) =>
            new UpdateStoreContentModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.UpdateStoreContentModelMasterResult> UpdateStoreContentModelMasterAsync(
                Request.UpdateStoreContentModelMasterRequest request
        ) =>
            new UpdateStoreContentModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.UpdateStoreContentModelMasterResult>();
    #else
		public UpdateStoreContentModelMasterTask UpdateStoreContentModelMasterAsync(
                Request.UpdateStoreContentModelMasterRequest request
        )
		{
			return new UpdateStoreContentModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.UpdateStoreContentModelMasterResult> UpdateStoreContentModelMasterAsync(
                Request.UpdateStoreContentModelMasterRequest request
        ) =>
            new UpdateStoreContentModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class DeleteStoreContentModelMasterTask : Gs2RestSessionTask<DeleteStoreContentModelMasterRequest, DeleteStoreContentModelMasterResult>
        {
            public DeleteStoreContentModelMasterTask(IGs2Session session, RestSessionRequestFactory factory, DeleteStoreContentModelMasterRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DeleteStoreContentModelMasterRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "money2")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/master/model/{contentName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{contentName}", !string.IsNullOrEmpty(request.ContentName) ? request.ContentName.ToString() : "null");

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
		public IEnumerator DeleteStoreContentModelMaster(
                Request.DeleteStoreContentModelMasterRequest request,
                UnityAction<AsyncResult<Result.DeleteStoreContentModelMasterResult>> callback
        ) =>
            new DeleteStoreContentModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.DeleteStoreContentModelMasterResult> DeleteStoreContentModelMasterFuture(
                Request.DeleteStoreContentModelMasterRequest request
        ) =>
            new DeleteStoreContentModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.DeleteStoreContentModelMasterResult> DeleteStoreContentModelMasterAsync(
                Request.DeleteStoreContentModelMasterRequest request
        ) =>
            new DeleteStoreContentModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.DeleteStoreContentModelMasterResult>();
    #else
		public DeleteStoreContentModelMasterTask DeleteStoreContentModelMasterAsync(
                Request.DeleteStoreContentModelMasterRequest request
        )
		{
			return new DeleteStoreContentModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.DeleteStoreContentModelMasterResult> DeleteStoreContentModelMasterAsync(
                Request.DeleteStoreContentModelMasterRequest request
        ) =>
            new DeleteStoreContentModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class DescribeStoreSubscriptionContentModelsTask : Gs2RestSessionTask<DescribeStoreSubscriptionContentModelsRequest, DescribeStoreSubscriptionContentModelsResult>
        {
            public DescribeStoreSubscriptionContentModelsTask(IGs2Session session, RestSessionRequestFactory factory, DescribeStoreSubscriptionContentModelsRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DescribeStoreSubscriptionContentModelsRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "money2")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/model/subscription/content";

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
		public IEnumerator DescribeStoreSubscriptionContentModels(
                Request.DescribeStoreSubscriptionContentModelsRequest request,
                UnityAction<AsyncResult<Result.DescribeStoreSubscriptionContentModelsResult>> callback
        ) =>
            new DescribeStoreSubscriptionContentModelsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.DescribeStoreSubscriptionContentModelsResult> DescribeStoreSubscriptionContentModelsFuture(
                Request.DescribeStoreSubscriptionContentModelsRequest request
        ) =>
            new DescribeStoreSubscriptionContentModelsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.DescribeStoreSubscriptionContentModelsResult> DescribeStoreSubscriptionContentModelsAsync(
                Request.DescribeStoreSubscriptionContentModelsRequest request
        ) =>
            new DescribeStoreSubscriptionContentModelsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.DescribeStoreSubscriptionContentModelsResult>();
    #else
		public DescribeStoreSubscriptionContentModelsTask DescribeStoreSubscriptionContentModelsAsync(
                Request.DescribeStoreSubscriptionContentModelsRequest request
        )
		{
			return new DescribeStoreSubscriptionContentModelsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.DescribeStoreSubscriptionContentModelsResult> DescribeStoreSubscriptionContentModelsAsync(
                Request.DescribeStoreSubscriptionContentModelsRequest request
        ) =>
            new DescribeStoreSubscriptionContentModelsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class GetStoreSubscriptionContentModelTask : Gs2RestSessionTask<GetStoreSubscriptionContentModelRequest, GetStoreSubscriptionContentModelResult>
        {
            public GetStoreSubscriptionContentModelTask(IGs2Session session, RestSessionRequestFactory factory, GetStoreSubscriptionContentModelRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(GetStoreSubscriptionContentModelRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "money2")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/model/subscription/content/{contentName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{contentName}", !string.IsNullOrEmpty(request.ContentName) ? request.ContentName.ToString() : "null");

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
		public IEnumerator GetStoreSubscriptionContentModel(
                Request.GetStoreSubscriptionContentModelRequest request,
                UnityAction<AsyncResult<Result.GetStoreSubscriptionContentModelResult>> callback
        ) =>
            new GetStoreSubscriptionContentModelTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.GetStoreSubscriptionContentModelResult> GetStoreSubscriptionContentModelFuture(
                Request.GetStoreSubscriptionContentModelRequest request
        ) =>
            new GetStoreSubscriptionContentModelTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.GetStoreSubscriptionContentModelResult> GetStoreSubscriptionContentModelAsync(
                Request.GetStoreSubscriptionContentModelRequest request
        ) =>
            new GetStoreSubscriptionContentModelTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.GetStoreSubscriptionContentModelResult>();
    #else
		public GetStoreSubscriptionContentModelTask GetStoreSubscriptionContentModelAsync(
                Request.GetStoreSubscriptionContentModelRequest request
        )
		{
			return new GetStoreSubscriptionContentModelTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.GetStoreSubscriptionContentModelResult> GetStoreSubscriptionContentModelAsync(
                Request.GetStoreSubscriptionContentModelRequest request
        ) =>
            new GetStoreSubscriptionContentModelTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class DescribeStoreSubscriptionContentModelMastersTask : Gs2RestSessionTask<DescribeStoreSubscriptionContentModelMastersRequest, DescribeStoreSubscriptionContentModelMastersResult>
        {
            public DescribeStoreSubscriptionContentModelMastersTask(IGs2Session session, RestSessionRequestFactory factory, DescribeStoreSubscriptionContentModelMastersRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DescribeStoreSubscriptionContentModelMastersRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "money2")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/master/model/subscription";

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
		public IEnumerator DescribeStoreSubscriptionContentModelMasters(
                Request.DescribeStoreSubscriptionContentModelMastersRequest request,
                UnityAction<AsyncResult<Result.DescribeStoreSubscriptionContentModelMastersResult>> callback
        ) =>
            new DescribeStoreSubscriptionContentModelMastersTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.DescribeStoreSubscriptionContentModelMastersResult> DescribeStoreSubscriptionContentModelMastersFuture(
                Request.DescribeStoreSubscriptionContentModelMastersRequest request
        ) =>
            new DescribeStoreSubscriptionContentModelMastersTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.DescribeStoreSubscriptionContentModelMastersResult> DescribeStoreSubscriptionContentModelMastersAsync(
                Request.DescribeStoreSubscriptionContentModelMastersRequest request
        ) =>
            new DescribeStoreSubscriptionContentModelMastersTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.DescribeStoreSubscriptionContentModelMastersResult>();
    #else
		public DescribeStoreSubscriptionContentModelMastersTask DescribeStoreSubscriptionContentModelMastersAsync(
                Request.DescribeStoreSubscriptionContentModelMastersRequest request
        )
		{
			return new DescribeStoreSubscriptionContentModelMastersTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.DescribeStoreSubscriptionContentModelMastersResult> DescribeStoreSubscriptionContentModelMastersAsync(
                Request.DescribeStoreSubscriptionContentModelMastersRequest request
        ) =>
            new DescribeStoreSubscriptionContentModelMastersTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class CreateStoreSubscriptionContentModelMasterTask : Gs2RestSessionTask<CreateStoreSubscriptionContentModelMasterRequest, CreateStoreSubscriptionContentModelMasterResult>
        {
            public CreateStoreSubscriptionContentModelMasterTask(IGs2Session session, RestSessionRequestFactory factory, CreateStoreSubscriptionContentModelMasterRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(CreateStoreSubscriptionContentModelMasterRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "money2")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/master/model/subscription";

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
                if (request.ScheduleNamespaceId != null)
                {
                    jsonWriter.WritePropertyName("scheduleNamespaceId");
                    jsonWriter.Write(request.ScheduleNamespaceId);
                }
                if (request.TriggerName != null)
                {
                    jsonWriter.WritePropertyName("triggerName");
                    jsonWriter.Write(request.TriggerName);
                }
                if (request.TriggerExtendMode != null)
                {
                    jsonWriter.WritePropertyName("triggerExtendMode");
                    jsonWriter.Write(request.TriggerExtendMode);
                }
                if (request.RollupHour != null)
                {
                    jsonWriter.WritePropertyName("rollupHour");
                    jsonWriter.Write(request.RollupHour.ToString());
                }
                if (request.ReallocateSpanDays != null)
                {
                    jsonWriter.WritePropertyName("reallocateSpanDays");
                    jsonWriter.Write(request.ReallocateSpanDays.ToString());
                }
                if (request.AppleAppStore != null)
                {
                    jsonWriter.WritePropertyName("appleAppStore");
                    request.AppleAppStore.WriteJson(jsonWriter);
                }
                if (request.GooglePlay != null)
                {
                    jsonWriter.WritePropertyName("googlePlay");
                    request.GooglePlay.WriteJson(jsonWriter);
                }
                if (request.ContextStack != null)
                {
                    jsonWriter.WritePropertyName("contextStack");
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
		public IEnumerator CreateStoreSubscriptionContentModelMaster(
                Request.CreateStoreSubscriptionContentModelMasterRequest request,
                UnityAction<AsyncResult<Result.CreateStoreSubscriptionContentModelMasterResult>> callback
        ) =>
            new CreateStoreSubscriptionContentModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.CreateStoreSubscriptionContentModelMasterResult> CreateStoreSubscriptionContentModelMasterFuture(
                Request.CreateStoreSubscriptionContentModelMasterRequest request
        ) =>
            new CreateStoreSubscriptionContentModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.CreateStoreSubscriptionContentModelMasterResult> CreateStoreSubscriptionContentModelMasterAsync(
                Request.CreateStoreSubscriptionContentModelMasterRequest request
        ) =>
            new CreateStoreSubscriptionContentModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.CreateStoreSubscriptionContentModelMasterResult>();
    #else
		public CreateStoreSubscriptionContentModelMasterTask CreateStoreSubscriptionContentModelMasterAsync(
                Request.CreateStoreSubscriptionContentModelMasterRequest request
        )
		{
			return new CreateStoreSubscriptionContentModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.CreateStoreSubscriptionContentModelMasterResult> CreateStoreSubscriptionContentModelMasterAsync(
                Request.CreateStoreSubscriptionContentModelMasterRequest request
        ) =>
            new CreateStoreSubscriptionContentModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class GetStoreSubscriptionContentModelMasterTask : Gs2RestSessionTask<GetStoreSubscriptionContentModelMasterRequest, GetStoreSubscriptionContentModelMasterResult>
        {
            public GetStoreSubscriptionContentModelMasterTask(IGs2Session session, RestSessionRequestFactory factory, GetStoreSubscriptionContentModelMasterRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(GetStoreSubscriptionContentModelMasterRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "money2")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/master/model/subscription/{contentName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{contentName}", !string.IsNullOrEmpty(request.ContentName) ? request.ContentName.ToString() : "null");

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
		public IEnumerator GetStoreSubscriptionContentModelMaster(
                Request.GetStoreSubscriptionContentModelMasterRequest request,
                UnityAction<AsyncResult<Result.GetStoreSubscriptionContentModelMasterResult>> callback
        ) =>
            new GetStoreSubscriptionContentModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.GetStoreSubscriptionContentModelMasterResult> GetStoreSubscriptionContentModelMasterFuture(
                Request.GetStoreSubscriptionContentModelMasterRequest request
        ) =>
            new GetStoreSubscriptionContentModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.GetStoreSubscriptionContentModelMasterResult> GetStoreSubscriptionContentModelMasterAsync(
                Request.GetStoreSubscriptionContentModelMasterRequest request
        ) =>
            new GetStoreSubscriptionContentModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.GetStoreSubscriptionContentModelMasterResult>();
    #else
		public GetStoreSubscriptionContentModelMasterTask GetStoreSubscriptionContentModelMasterAsync(
                Request.GetStoreSubscriptionContentModelMasterRequest request
        )
		{
			return new GetStoreSubscriptionContentModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.GetStoreSubscriptionContentModelMasterResult> GetStoreSubscriptionContentModelMasterAsync(
                Request.GetStoreSubscriptionContentModelMasterRequest request
        ) =>
            new GetStoreSubscriptionContentModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class UpdateStoreSubscriptionContentModelMasterTask : Gs2RestSessionTask<UpdateStoreSubscriptionContentModelMasterRequest, UpdateStoreSubscriptionContentModelMasterResult>
        {
            public UpdateStoreSubscriptionContentModelMasterTask(IGs2Session session, RestSessionRequestFactory factory, UpdateStoreSubscriptionContentModelMasterRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(UpdateStoreSubscriptionContentModelMasterRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "money2")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/master/model/subscription/{contentName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{contentName}", !string.IsNullOrEmpty(request.ContentName) ? request.ContentName.ToString() : "null");

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
                if (request.ScheduleNamespaceId != null)
                {
                    jsonWriter.WritePropertyName("scheduleNamespaceId");
                    jsonWriter.Write(request.ScheduleNamespaceId);
                }
                if (request.TriggerName != null)
                {
                    jsonWriter.WritePropertyName("triggerName");
                    jsonWriter.Write(request.TriggerName);
                }
                if (request.TriggerExtendMode != null)
                {
                    jsonWriter.WritePropertyName("triggerExtendMode");
                    jsonWriter.Write(request.TriggerExtendMode);
                }
                if (request.RollupHour != null)
                {
                    jsonWriter.WritePropertyName("rollupHour");
                    jsonWriter.Write(request.RollupHour.ToString());
                }
                if (request.ReallocateSpanDays != null)
                {
                    jsonWriter.WritePropertyName("reallocateSpanDays");
                    jsonWriter.Write(request.ReallocateSpanDays.ToString());
                }
                if (request.AppleAppStore != null)
                {
                    jsonWriter.WritePropertyName("appleAppStore");
                    request.AppleAppStore.WriteJson(jsonWriter);
                }
                if (request.GooglePlay != null)
                {
                    jsonWriter.WritePropertyName("googlePlay");
                    request.GooglePlay.WriteJson(jsonWriter);
                }
                if (request.ContextStack != null)
                {
                    jsonWriter.WritePropertyName("contextStack");
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
		public IEnumerator UpdateStoreSubscriptionContentModelMaster(
                Request.UpdateStoreSubscriptionContentModelMasterRequest request,
                UnityAction<AsyncResult<Result.UpdateStoreSubscriptionContentModelMasterResult>> callback
        ) =>
            new UpdateStoreSubscriptionContentModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.UpdateStoreSubscriptionContentModelMasterResult> UpdateStoreSubscriptionContentModelMasterFuture(
                Request.UpdateStoreSubscriptionContentModelMasterRequest request
        ) =>
            new UpdateStoreSubscriptionContentModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.UpdateStoreSubscriptionContentModelMasterResult> UpdateStoreSubscriptionContentModelMasterAsync(
                Request.UpdateStoreSubscriptionContentModelMasterRequest request
        ) =>
            new UpdateStoreSubscriptionContentModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.UpdateStoreSubscriptionContentModelMasterResult>();
    #else
		public UpdateStoreSubscriptionContentModelMasterTask UpdateStoreSubscriptionContentModelMasterAsync(
                Request.UpdateStoreSubscriptionContentModelMasterRequest request
        )
		{
			return new UpdateStoreSubscriptionContentModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.UpdateStoreSubscriptionContentModelMasterResult> UpdateStoreSubscriptionContentModelMasterAsync(
                Request.UpdateStoreSubscriptionContentModelMasterRequest request
        ) =>
            new UpdateStoreSubscriptionContentModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class DeleteStoreSubscriptionContentModelMasterTask : Gs2RestSessionTask<DeleteStoreSubscriptionContentModelMasterRequest, DeleteStoreSubscriptionContentModelMasterResult>
        {
            public DeleteStoreSubscriptionContentModelMasterTask(IGs2Session session, RestSessionRequestFactory factory, DeleteStoreSubscriptionContentModelMasterRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DeleteStoreSubscriptionContentModelMasterRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "money2")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/master/model/subscription/{contentName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{contentName}", !string.IsNullOrEmpty(request.ContentName) ? request.ContentName.ToString() : "null");

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
		public IEnumerator DeleteStoreSubscriptionContentModelMaster(
                Request.DeleteStoreSubscriptionContentModelMasterRequest request,
                UnityAction<AsyncResult<Result.DeleteStoreSubscriptionContentModelMasterResult>> callback
        ) =>
            new DeleteStoreSubscriptionContentModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.DeleteStoreSubscriptionContentModelMasterResult> DeleteStoreSubscriptionContentModelMasterFuture(
                Request.DeleteStoreSubscriptionContentModelMasterRequest request
        ) =>
            new DeleteStoreSubscriptionContentModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.DeleteStoreSubscriptionContentModelMasterResult> DeleteStoreSubscriptionContentModelMasterAsync(
                Request.DeleteStoreSubscriptionContentModelMasterRequest request
        ) =>
            new DeleteStoreSubscriptionContentModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.DeleteStoreSubscriptionContentModelMasterResult>();
    #else
		public DeleteStoreSubscriptionContentModelMasterTask DeleteStoreSubscriptionContentModelMasterAsync(
                Request.DeleteStoreSubscriptionContentModelMasterRequest request
        )
		{
			return new DeleteStoreSubscriptionContentModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.DeleteStoreSubscriptionContentModelMasterResult> DeleteStoreSubscriptionContentModelMasterAsync(
                Request.DeleteStoreSubscriptionContentModelMasterRequest request
        ) =>
            new DeleteStoreSubscriptionContentModelMasterTask(
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
                    .Replace("{service}", "money2")
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


        public class GetCurrentModelMasterTask : Gs2RestSessionTask<GetCurrentModelMasterRequest, GetCurrentModelMasterResult>
        {
            public GetCurrentModelMasterTask(IGs2Session session, RestSessionRequestFactory factory, GetCurrentModelMasterRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(GetCurrentModelMasterRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "money2")
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
		public IEnumerator GetCurrentModelMaster(
                Request.GetCurrentModelMasterRequest request,
                UnityAction<AsyncResult<Result.GetCurrentModelMasterResult>> callback
        ) =>
            new GetCurrentModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.GetCurrentModelMasterResult> GetCurrentModelMasterFuture(
                Request.GetCurrentModelMasterRequest request
        ) =>
            new GetCurrentModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.GetCurrentModelMasterResult> GetCurrentModelMasterAsync(
                Request.GetCurrentModelMasterRequest request
        ) =>
            new GetCurrentModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.GetCurrentModelMasterResult>();
    #else
		public GetCurrentModelMasterTask GetCurrentModelMasterAsync(
                Request.GetCurrentModelMasterRequest request
        )
		{
			return new GetCurrentModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.GetCurrentModelMasterResult> GetCurrentModelMasterAsync(
                Request.GetCurrentModelMasterRequest request
        ) =>
            new GetCurrentModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class PreUpdateCurrentModelMasterTask : Gs2RestSessionTask<PreUpdateCurrentModelMasterRequest, PreUpdateCurrentModelMasterResult>
        {
            public PreUpdateCurrentModelMasterTask(IGs2Session session, RestSessionRequestFactory factory, PreUpdateCurrentModelMasterRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(PreUpdateCurrentModelMasterRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "money2")
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
		public IEnumerator PreUpdateCurrentModelMaster(
                Request.PreUpdateCurrentModelMasterRequest request,
                UnityAction<AsyncResult<Result.PreUpdateCurrentModelMasterResult>> callback
        ) =>
            new PreUpdateCurrentModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.PreUpdateCurrentModelMasterResult> PreUpdateCurrentModelMasterFuture(
                Request.PreUpdateCurrentModelMasterRequest request
        ) =>
            new PreUpdateCurrentModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.PreUpdateCurrentModelMasterResult> PreUpdateCurrentModelMasterAsync(
                Request.PreUpdateCurrentModelMasterRequest request
        ) =>
            new PreUpdateCurrentModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.PreUpdateCurrentModelMasterResult>();
    #else
		public PreUpdateCurrentModelMasterTask PreUpdateCurrentModelMasterAsync(
                Request.PreUpdateCurrentModelMasterRequest request
        )
		{
			return new PreUpdateCurrentModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.PreUpdateCurrentModelMasterResult> PreUpdateCurrentModelMasterAsync(
                Request.PreUpdateCurrentModelMasterRequest request
        ) =>
            new PreUpdateCurrentModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class UpdateCurrentModelMasterTask : Gs2RestSessionTask<UpdateCurrentModelMasterRequest, UpdateCurrentModelMasterResult>
        {
            public UpdateCurrentModelMasterTask(IGs2Session session, RestSessionRequestFactory factory, UpdateCurrentModelMasterRequest request) : base(session, factory, request)
            {
            }
#if GS2_ENABLE_UNITASK
            protected override async UniTask<UpdateCurrentModelMasterResult> InvokeImpl()
#else
            protected override async Task<UpdateCurrentModelMasterResult> InvokeImpl()
#endif
            {
                if (Request.Settings != null) {
                    var preTask = new PreUpdateCurrentModelMasterTask(
                        Session,
                        Factory,
                        new PreUpdateCurrentModelMasterRequest()
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

            protected override IGs2SessionRequest CreateRequest(UpdateCurrentModelMasterRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "money2")
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
		public IEnumerator UpdateCurrentModelMaster(
                Request.UpdateCurrentModelMasterRequest request,
                UnityAction<AsyncResult<Result.UpdateCurrentModelMasterResult>> callback
        ) =>
            new UpdateCurrentModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.UpdateCurrentModelMasterResult> UpdateCurrentModelMasterFuture(
                Request.UpdateCurrentModelMasterRequest request
        ) =>
            new UpdateCurrentModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.UpdateCurrentModelMasterResult> UpdateCurrentModelMasterAsync(
                Request.UpdateCurrentModelMasterRequest request
        ) =>
            new UpdateCurrentModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.UpdateCurrentModelMasterResult>();
    #else
		public UpdateCurrentModelMasterTask UpdateCurrentModelMasterAsync(
                Request.UpdateCurrentModelMasterRequest request
        )
		{
			return new UpdateCurrentModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.UpdateCurrentModelMasterResult> UpdateCurrentModelMasterAsync(
                Request.UpdateCurrentModelMasterRequest request
        ) =>
            new UpdateCurrentModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class UpdateCurrentModelMasterFromGitHubTask : Gs2RestSessionTask<UpdateCurrentModelMasterFromGitHubRequest, UpdateCurrentModelMasterFromGitHubResult>
        {
            public UpdateCurrentModelMasterFromGitHubTask(IGs2Session session, RestSessionRequestFactory factory, UpdateCurrentModelMasterFromGitHubRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(UpdateCurrentModelMasterFromGitHubRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "money2")
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
		public IEnumerator UpdateCurrentModelMasterFromGitHub(
                Request.UpdateCurrentModelMasterFromGitHubRequest request,
                UnityAction<AsyncResult<Result.UpdateCurrentModelMasterFromGitHubResult>> callback
        ) =>
            new UpdateCurrentModelMasterFromGitHubTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.UpdateCurrentModelMasterFromGitHubResult> UpdateCurrentModelMasterFromGitHubFuture(
                Request.UpdateCurrentModelMasterFromGitHubRequest request
        ) =>
            new UpdateCurrentModelMasterFromGitHubTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.UpdateCurrentModelMasterFromGitHubResult> UpdateCurrentModelMasterFromGitHubAsync(
                Request.UpdateCurrentModelMasterFromGitHubRequest request
        ) =>
            new UpdateCurrentModelMasterFromGitHubTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.UpdateCurrentModelMasterFromGitHubResult>();
    #else
		public UpdateCurrentModelMasterFromGitHubTask UpdateCurrentModelMasterFromGitHubAsync(
                Request.UpdateCurrentModelMasterFromGitHubRequest request
        )
		{
			return new UpdateCurrentModelMasterFromGitHubTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.UpdateCurrentModelMasterFromGitHubResult> UpdateCurrentModelMasterFromGitHubAsync(
                Request.UpdateCurrentModelMasterFromGitHubRequest request
        ) =>
            new UpdateCurrentModelMasterFromGitHubTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class DescribeDailyTransactionHistoriesByCurrencyTask : Gs2RestSessionTask<DescribeDailyTransactionHistoriesByCurrencyRequest, DescribeDailyTransactionHistoriesByCurrencyResult>
        {
            public DescribeDailyTransactionHistoriesByCurrencyTask(IGs2Session session, RestSessionRequestFactory factory, DescribeDailyTransactionHistoriesByCurrencyRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DescribeDailyTransactionHistoriesByCurrencyRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "money2")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/transaction/daily/currency/{currency}/date/{year}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{currency}", !string.IsNullOrEmpty(request.Currency) ? request.Currency.ToString() : "null");
                url = url.Replace("{year}",request.Year != null ? request.Year.ToString() : "null");

                var sessionRequest = Factory.Get(url);
                if (request.ContextStack != null)
                {
                    sessionRequest.AddQueryString("contextStack", request.ContextStack);
                }
                if (request.Month != null) {
                    sessionRequest.AddQueryString("month", $"{request.Month}");
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
		public IEnumerator DescribeDailyTransactionHistoriesByCurrency(
                Request.DescribeDailyTransactionHistoriesByCurrencyRequest request,
                UnityAction<AsyncResult<Result.DescribeDailyTransactionHistoriesByCurrencyResult>> callback
        ) =>
            new DescribeDailyTransactionHistoriesByCurrencyTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.DescribeDailyTransactionHistoriesByCurrencyResult> DescribeDailyTransactionHistoriesByCurrencyFuture(
                Request.DescribeDailyTransactionHistoriesByCurrencyRequest request
        ) =>
            new DescribeDailyTransactionHistoriesByCurrencyTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.DescribeDailyTransactionHistoriesByCurrencyResult> DescribeDailyTransactionHistoriesByCurrencyAsync(
                Request.DescribeDailyTransactionHistoriesByCurrencyRequest request
        ) =>
            new DescribeDailyTransactionHistoriesByCurrencyTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.DescribeDailyTransactionHistoriesByCurrencyResult>();
    #else
		public DescribeDailyTransactionHistoriesByCurrencyTask DescribeDailyTransactionHistoriesByCurrencyAsync(
                Request.DescribeDailyTransactionHistoriesByCurrencyRequest request
        )
		{
			return new DescribeDailyTransactionHistoriesByCurrencyTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.DescribeDailyTransactionHistoriesByCurrencyResult> DescribeDailyTransactionHistoriesByCurrencyAsync(
                Request.DescribeDailyTransactionHistoriesByCurrencyRequest request
        ) =>
            new DescribeDailyTransactionHistoriesByCurrencyTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class DescribeDailyTransactionHistoriesTask : Gs2RestSessionTask<DescribeDailyTransactionHistoriesRequest, DescribeDailyTransactionHistoriesResult>
        {
            public DescribeDailyTransactionHistoriesTask(IGs2Session session, RestSessionRequestFactory factory, DescribeDailyTransactionHistoriesRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DescribeDailyTransactionHistoriesRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "money2")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/transaction/daily/{year}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{year}",request.Year != null ? request.Year.ToString() : "null");

                var sessionRequest = Factory.Get(url);
                if (request.ContextStack != null)
                {
                    sessionRequest.AddQueryString("contextStack", request.ContextStack);
                }
                if (request.Month != null) {
                    sessionRequest.AddQueryString("month", $"{request.Month}");
                }
                if (request.Day != null) {
                    sessionRequest.AddQueryString("day", $"{request.Day}");
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
		public IEnumerator DescribeDailyTransactionHistories(
                Request.DescribeDailyTransactionHistoriesRequest request,
                UnityAction<AsyncResult<Result.DescribeDailyTransactionHistoriesResult>> callback
        ) =>
            new DescribeDailyTransactionHistoriesTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.DescribeDailyTransactionHistoriesResult> DescribeDailyTransactionHistoriesFuture(
                Request.DescribeDailyTransactionHistoriesRequest request
        ) =>
            new DescribeDailyTransactionHistoriesTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.DescribeDailyTransactionHistoriesResult> DescribeDailyTransactionHistoriesAsync(
                Request.DescribeDailyTransactionHistoriesRequest request
        ) =>
            new DescribeDailyTransactionHistoriesTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.DescribeDailyTransactionHistoriesResult>();
    #else
		public DescribeDailyTransactionHistoriesTask DescribeDailyTransactionHistoriesAsync(
                Request.DescribeDailyTransactionHistoriesRequest request
        )
		{
			return new DescribeDailyTransactionHistoriesTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.DescribeDailyTransactionHistoriesResult> DescribeDailyTransactionHistoriesAsync(
                Request.DescribeDailyTransactionHistoriesRequest request
        ) =>
            new DescribeDailyTransactionHistoriesTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class GetDailyTransactionHistoryTask : Gs2RestSessionTask<GetDailyTransactionHistoryRequest, GetDailyTransactionHistoryResult>
        {
            public GetDailyTransactionHistoryTask(IGs2Session session, RestSessionRequestFactory factory, GetDailyTransactionHistoryRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(GetDailyTransactionHistoryRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "money2")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/transaction/daily/{year}/{month}/{day}/currency/{currency}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{year}",request.Year != null ? request.Year.ToString() : "null");
                url = url.Replace("{month}",request.Month != null ? request.Month.ToString() : "null");
                url = url.Replace("{day}",request.Day != null ? request.Day.ToString() : "null");
                url = url.Replace("{currency}", !string.IsNullOrEmpty(request.Currency) ? request.Currency.ToString() : "null");

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
		public IEnumerator GetDailyTransactionHistory(
                Request.GetDailyTransactionHistoryRequest request,
                UnityAction<AsyncResult<Result.GetDailyTransactionHistoryResult>> callback
        ) =>
            new GetDailyTransactionHistoryTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.GetDailyTransactionHistoryResult> GetDailyTransactionHistoryFuture(
                Request.GetDailyTransactionHistoryRequest request
        ) =>
            new GetDailyTransactionHistoryTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.GetDailyTransactionHistoryResult> GetDailyTransactionHistoryAsync(
                Request.GetDailyTransactionHistoryRequest request
        ) =>
            new GetDailyTransactionHistoryTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.GetDailyTransactionHistoryResult>();
    #else
		public GetDailyTransactionHistoryTask GetDailyTransactionHistoryAsync(
                Request.GetDailyTransactionHistoryRequest request
        )
		{
			return new GetDailyTransactionHistoryTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.GetDailyTransactionHistoryResult> GetDailyTransactionHistoryAsync(
                Request.GetDailyTransactionHistoryRequest request
        ) =>
            new GetDailyTransactionHistoryTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class DescribeUnusedBalancesTask : Gs2RestSessionTask<DescribeUnusedBalancesRequest, DescribeUnusedBalancesResult>
        {
            public DescribeUnusedBalancesTask(IGs2Session session, RestSessionRequestFactory factory, DescribeUnusedBalancesRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DescribeUnusedBalancesRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "money2")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/balance/unused";

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
		public IEnumerator DescribeUnusedBalances(
                Request.DescribeUnusedBalancesRequest request,
                UnityAction<AsyncResult<Result.DescribeUnusedBalancesResult>> callback
        ) =>
            new DescribeUnusedBalancesTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.DescribeUnusedBalancesResult> DescribeUnusedBalancesFuture(
                Request.DescribeUnusedBalancesRequest request
        ) =>
            new DescribeUnusedBalancesTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.DescribeUnusedBalancesResult> DescribeUnusedBalancesAsync(
                Request.DescribeUnusedBalancesRequest request
        ) =>
            new DescribeUnusedBalancesTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.DescribeUnusedBalancesResult>();
    #else
		public DescribeUnusedBalancesTask DescribeUnusedBalancesAsync(
                Request.DescribeUnusedBalancesRequest request
        )
		{
			return new DescribeUnusedBalancesTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.DescribeUnusedBalancesResult> DescribeUnusedBalancesAsync(
                Request.DescribeUnusedBalancesRequest request
        ) =>
            new DescribeUnusedBalancesTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class GetUnusedBalanceTask : Gs2RestSessionTask<GetUnusedBalanceRequest, GetUnusedBalanceResult>
        {
            public GetUnusedBalanceTask(IGs2Session session, RestSessionRequestFactory factory, GetUnusedBalanceRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(GetUnusedBalanceRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "money2")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/balance/unused/{currency}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{currency}", !string.IsNullOrEmpty(request.Currency) ? request.Currency.ToString() : "null");

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
		public IEnumerator GetUnusedBalance(
                Request.GetUnusedBalanceRequest request,
                UnityAction<AsyncResult<Result.GetUnusedBalanceResult>> callback
        ) =>
            new GetUnusedBalanceTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.GetUnusedBalanceResult> GetUnusedBalanceFuture(
                Request.GetUnusedBalanceRequest request
        ) =>
            new GetUnusedBalanceTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.GetUnusedBalanceResult> GetUnusedBalanceAsync(
                Request.GetUnusedBalanceRequest request
        ) =>
            new GetUnusedBalanceTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.GetUnusedBalanceResult>();
    #else
		public GetUnusedBalanceTask GetUnusedBalanceAsync(
                Request.GetUnusedBalanceRequest request
        )
		{
			return new GetUnusedBalanceTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.GetUnusedBalanceResult> GetUnusedBalanceAsync(
                Request.GetUnusedBalanceRequest request
        ) =>
            new GetUnusedBalanceTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif
	}
}