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
using Gs2.Gs2Lottery.Request;
using Gs2.Gs2Lottery.Result;
using Gs2.Util.LitJson;

namespace Gs2.Gs2Lottery
{
	public class Gs2LotteryRestClient : AbstractGs2Client
	{
#if UNITY_2017_1_OR_NEWER
		private readonly CertificateHandler _certificateHandler;
#endif

		public static string Endpoint = "lottery";

        protected Gs2RestSession Gs2RestSession => (Gs2RestSession) Gs2Session;

		public Gs2LotteryRestClient(Gs2RestSession Gs2RestSession) : base(Gs2RestSession)
		{

		}

#if UNITY_2017_1_OR_NEWER
		public Gs2LotteryRestClient(Gs2RestSession gs2RestSession, CertificateHandler certificateHandler) : base(gs2RestSession)
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
                    .Replace("{service}", "lottery")
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
                    .Replace("{service}", "lottery")
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
                if (request.LotteryTriggerScriptId != null)
                {
                    jsonWriter.WritePropertyName("lotteryTriggerScriptId");
                    jsonWriter.Write(request.LotteryTriggerScriptId);
                }
                if (request.ChoicePrizeTableScriptId != null)
                {
                    jsonWriter.WritePropertyName("choicePrizeTableScriptId");
                    jsonWriter.Write(request.ChoicePrizeTableScriptId);
                }
                if (request.LogSetting != null)
                {
                    jsonWriter.WritePropertyName("logSetting");
                    request.LogSetting.WriteJson(jsonWriter);
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
                if (request.ContextStack != null)
                {
                    jsonWriter.WritePropertyName("contextStack");
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
                    .Replace("{service}", "lottery")
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
                    .Replace("{service}", "lottery")
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
                    .Replace("{service}", "lottery")
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
                if (request.LotteryTriggerScriptId != null)
                {
                    jsonWriter.WritePropertyName("lotteryTriggerScriptId");
                    jsonWriter.Write(request.LotteryTriggerScriptId);
                }
                if (request.ChoicePrizeTableScriptId != null)
                {
                    jsonWriter.WritePropertyName("choicePrizeTableScriptId");
                    jsonWriter.Write(request.ChoicePrizeTableScriptId);
                }
                if (request.LogSetting != null)
                {
                    jsonWriter.WritePropertyName("logSetting");
                    request.LogSetting.WriteJson(jsonWriter);
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
                if (request.ContextStack != null)
                {
                    jsonWriter.WritePropertyName("contextStack");
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
                    .Replace("{service}", "lottery")
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


        public class DescribeLotteryModelMastersTask : Gs2RestSessionTask<DescribeLotteryModelMastersRequest, DescribeLotteryModelMastersResult>
        {
            public DescribeLotteryModelMastersTask(IGs2Session session, RestSessionRequestFactory factory, DescribeLotteryModelMastersRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DescribeLotteryModelMastersRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "lottery")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/master/lottery";

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
		public IEnumerator DescribeLotteryModelMasters(
                Request.DescribeLotteryModelMastersRequest request,
                UnityAction<AsyncResult<Result.DescribeLotteryModelMastersResult>> callback
        )
		{
			var task = new DescribeLotteryModelMastersTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.DescribeLotteryModelMastersResult>(task.Result, task.Error));
        }

		public IFuture<Result.DescribeLotteryModelMastersResult> DescribeLotteryModelMastersFuture(
                Request.DescribeLotteryModelMastersRequest request
        )
		{
			return new DescribeLotteryModelMastersTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.DescribeLotteryModelMastersResult> DescribeLotteryModelMastersAsync(
                Request.DescribeLotteryModelMastersRequest request
        )
		{
            AsyncResult<Result.DescribeLotteryModelMastersResult> result = null;
			await DescribeLotteryModelMasters(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
    #else
		public DescribeLotteryModelMastersTask DescribeLotteryModelMastersAsync(
                Request.DescribeLotteryModelMastersRequest request
        )
		{
			return new DescribeLotteryModelMastersTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.DescribeLotteryModelMastersResult> DescribeLotteryModelMastersAsync(
                Request.DescribeLotteryModelMastersRequest request
        )
		{
			var task = new DescribeLotteryModelMastersTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class CreateLotteryModelMasterTask : Gs2RestSessionTask<CreateLotteryModelMasterRequest, CreateLotteryModelMasterResult>
        {
            public CreateLotteryModelMasterTask(IGs2Session session, RestSessionRequestFactory factory, CreateLotteryModelMasterRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(CreateLotteryModelMasterRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "lottery")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/master/lottery";

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
                if (request.Mode != null)
                {
                    jsonWriter.WritePropertyName("mode");
                    jsonWriter.Write(request.Mode);
                }
                if (request.Method != null)
                {
                    jsonWriter.WritePropertyName("method");
                    jsonWriter.Write(request.Method);
                }
                if (request.PrizeTableName != null)
                {
                    jsonWriter.WritePropertyName("prizeTableName");
                    jsonWriter.Write(request.PrizeTableName);
                }
                if (request.ChoicePrizeTableScriptId != null)
                {
                    jsonWriter.WritePropertyName("choicePrizeTableScriptId");
                    jsonWriter.Write(request.ChoicePrizeTableScriptId);
                }
                if (request.ContextStack != null)
                {
                    jsonWriter.WritePropertyName("contextStack");
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
		public IEnumerator CreateLotteryModelMaster(
                Request.CreateLotteryModelMasterRequest request,
                UnityAction<AsyncResult<Result.CreateLotteryModelMasterResult>> callback
        )
		{
			var task = new CreateLotteryModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.CreateLotteryModelMasterResult>(task.Result, task.Error));
        }

		public IFuture<Result.CreateLotteryModelMasterResult> CreateLotteryModelMasterFuture(
                Request.CreateLotteryModelMasterRequest request
        )
		{
			return new CreateLotteryModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.CreateLotteryModelMasterResult> CreateLotteryModelMasterAsync(
                Request.CreateLotteryModelMasterRequest request
        )
		{
            AsyncResult<Result.CreateLotteryModelMasterResult> result = null;
			await CreateLotteryModelMaster(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
    #else
		public CreateLotteryModelMasterTask CreateLotteryModelMasterAsync(
                Request.CreateLotteryModelMasterRequest request
        )
		{
			return new CreateLotteryModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.CreateLotteryModelMasterResult> CreateLotteryModelMasterAsync(
                Request.CreateLotteryModelMasterRequest request
        )
		{
			var task = new CreateLotteryModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class GetLotteryModelMasterTask : Gs2RestSessionTask<GetLotteryModelMasterRequest, GetLotteryModelMasterResult>
        {
            public GetLotteryModelMasterTask(IGs2Session session, RestSessionRequestFactory factory, GetLotteryModelMasterRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(GetLotteryModelMasterRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "lottery")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/master/lottery/{lotteryName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{lotteryName}", !string.IsNullOrEmpty(request.LotteryName) ? request.LotteryName.ToString() : "null");

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
		public IEnumerator GetLotteryModelMaster(
                Request.GetLotteryModelMasterRequest request,
                UnityAction<AsyncResult<Result.GetLotteryModelMasterResult>> callback
        )
		{
			var task = new GetLotteryModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.GetLotteryModelMasterResult>(task.Result, task.Error));
        }

		public IFuture<Result.GetLotteryModelMasterResult> GetLotteryModelMasterFuture(
                Request.GetLotteryModelMasterRequest request
        )
		{
			return new GetLotteryModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.GetLotteryModelMasterResult> GetLotteryModelMasterAsync(
                Request.GetLotteryModelMasterRequest request
        )
		{
            AsyncResult<Result.GetLotteryModelMasterResult> result = null;
			await GetLotteryModelMaster(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
    #else
		public GetLotteryModelMasterTask GetLotteryModelMasterAsync(
                Request.GetLotteryModelMasterRequest request
        )
		{
			return new GetLotteryModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.GetLotteryModelMasterResult> GetLotteryModelMasterAsync(
                Request.GetLotteryModelMasterRequest request
        )
		{
			var task = new GetLotteryModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class UpdateLotteryModelMasterTask : Gs2RestSessionTask<UpdateLotteryModelMasterRequest, UpdateLotteryModelMasterResult>
        {
            public UpdateLotteryModelMasterTask(IGs2Session session, RestSessionRequestFactory factory, UpdateLotteryModelMasterRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(UpdateLotteryModelMasterRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "lottery")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/master/lottery/{lotteryName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{lotteryName}", !string.IsNullOrEmpty(request.LotteryName) ? request.LotteryName.ToString() : "null");

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
                if (request.Mode != null)
                {
                    jsonWriter.WritePropertyName("mode");
                    jsonWriter.Write(request.Mode);
                }
                if (request.Method != null)
                {
                    jsonWriter.WritePropertyName("method");
                    jsonWriter.Write(request.Method);
                }
                if (request.PrizeTableName != null)
                {
                    jsonWriter.WritePropertyName("prizeTableName");
                    jsonWriter.Write(request.PrizeTableName);
                }
                if (request.ChoicePrizeTableScriptId != null)
                {
                    jsonWriter.WritePropertyName("choicePrizeTableScriptId");
                    jsonWriter.Write(request.ChoicePrizeTableScriptId);
                }
                if (request.ContextStack != null)
                {
                    jsonWriter.WritePropertyName("contextStack");
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
		public IEnumerator UpdateLotteryModelMaster(
                Request.UpdateLotteryModelMasterRequest request,
                UnityAction<AsyncResult<Result.UpdateLotteryModelMasterResult>> callback
        )
		{
			var task = new UpdateLotteryModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.UpdateLotteryModelMasterResult>(task.Result, task.Error));
        }

		public IFuture<Result.UpdateLotteryModelMasterResult> UpdateLotteryModelMasterFuture(
                Request.UpdateLotteryModelMasterRequest request
        )
		{
			return new UpdateLotteryModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.UpdateLotteryModelMasterResult> UpdateLotteryModelMasterAsync(
                Request.UpdateLotteryModelMasterRequest request
        )
		{
            AsyncResult<Result.UpdateLotteryModelMasterResult> result = null;
			await UpdateLotteryModelMaster(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
    #else
		public UpdateLotteryModelMasterTask UpdateLotteryModelMasterAsync(
                Request.UpdateLotteryModelMasterRequest request
        )
		{
			return new UpdateLotteryModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.UpdateLotteryModelMasterResult> UpdateLotteryModelMasterAsync(
                Request.UpdateLotteryModelMasterRequest request
        )
		{
			var task = new UpdateLotteryModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class DeleteLotteryModelMasterTask : Gs2RestSessionTask<DeleteLotteryModelMasterRequest, DeleteLotteryModelMasterResult>
        {
            public DeleteLotteryModelMasterTask(IGs2Session session, RestSessionRequestFactory factory, DeleteLotteryModelMasterRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DeleteLotteryModelMasterRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "lottery")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/master/lottery/{lotteryName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{lotteryName}", !string.IsNullOrEmpty(request.LotteryName) ? request.LotteryName.ToString() : "null");

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
		public IEnumerator DeleteLotteryModelMaster(
                Request.DeleteLotteryModelMasterRequest request,
                UnityAction<AsyncResult<Result.DeleteLotteryModelMasterResult>> callback
        )
		{
			var task = new DeleteLotteryModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.DeleteLotteryModelMasterResult>(task.Result, task.Error));
        }

		public IFuture<Result.DeleteLotteryModelMasterResult> DeleteLotteryModelMasterFuture(
                Request.DeleteLotteryModelMasterRequest request
        )
		{
			return new DeleteLotteryModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.DeleteLotteryModelMasterResult> DeleteLotteryModelMasterAsync(
                Request.DeleteLotteryModelMasterRequest request
        )
		{
            AsyncResult<Result.DeleteLotteryModelMasterResult> result = null;
			await DeleteLotteryModelMaster(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
    #else
		public DeleteLotteryModelMasterTask DeleteLotteryModelMasterAsync(
                Request.DeleteLotteryModelMasterRequest request
        )
		{
			return new DeleteLotteryModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.DeleteLotteryModelMasterResult> DeleteLotteryModelMasterAsync(
                Request.DeleteLotteryModelMasterRequest request
        )
		{
			var task = new DeleteLotteryModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class DescribePrizeTableMastersTask : Gs2RestSessionTask<DescribePrizeTableMastersRequest, DescribePrizeTableMastersResult>
        {
            public DescribePrizeTableMastersTask(IGs2Session session, RestSessionRequestFactory factory, DescribePrizeTableMastersRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DescribePrizeTableMastersRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "lottery")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/master/table";

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
		public IEnumerator DescribePrizeTableMasters(
                Request.DescribePrizeTableMastersRequest request,
                UnityAction<AsyncResult<Result.DescribePrizeTableMastersResult>> callback
        )
		{
			var task = new DescribePrizeTableMastersTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.DescribePrizeTableMastersResult>(task.Result, task.Error));
        }

		public IFuture<Result.DescribePrizeTableMastersResult> DescribePrizeTableMastersFuture(
                Request.DescribePrizeTableMastersRequest request
        )
		{
			return new DescribePrizeTableMastersTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.DescribePrizeTableMastersResult> DescribePrizeTableMastersAsync(
                Request.DescribePrizeTableMastersRequest request
        )
		{
            AsyncResult<Result.DescribePrizeTableMastersResult> result = null;
			await DescribePrizeTableMasters(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
    #else
		public DescribePrizeTableMastersTask DescribePrizeTableMastersAsync(
                Request.DescribePrizeTableMastersRequest request
        )
		{
			return new DescribePrizeTableMastersTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.DescribePrizeTableMastersResult> DescribePrizeTableMastersAsync(
                Request.DescribePrizeTableMastersRequest request
        )
		{
			var task = new DescribePrizeTableMastersTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class CreatePrizeTableMasterTask : Gs2RestSessionTask<CreatePrizeTableMasterRequest, CreatePrizeTableMasterResult>
        {
            public CreatePrizeTableMasterTask(IGs2Session session, RestSessionRequestFactory factory, CreatePrizeTableMasterRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(CreatePrizeTableMasterRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "lottery")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/master/table";

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
                if (request.Prizes != null)
                {
                    jsonWriter.WritePropertyName("prizes");
                    jsonWriter.WriteArrayStart();
                    foreach(var item in request.Prizes)
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
		public IEnumerator CreatePrizeTableMaster(
                Request.CreatePrizeTableMasterRequest request,
                UnityAction<AsyncResult<Result.CreatePrizeTableMasterResult>> callback
        )
		{
			var task = new CreatePrizeTableMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.CreatePrizeTableMasterResult>(task.Result, task.Error));
        }

		public IFuture<Result.CreatePrizeTableMasterResult> CreatePrizeTableMasterFuture(
                Request.CreatePrizeTableMasterRequest request
        )
		{
			return new CreatePrizeTableMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.CreatePrizeTableMasterResult> CreatePrizeTableMasterAsync(
                Request.CreatePrizeTableMasterRequest request
        )
		{
            AsyncResult<Result.CreatePrizeTableMasterResult> result = null;
			await CreatePrizeTableMaster(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
    #else
		public CreatePrizeTableMasterTask CreatePrizeTableMasterAsync(
                Request.CreatePrizeTableMasterRequest request
        )
		{
			return new CreatePrizeTableMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.CreatePrizeTableMasterResult> CreatePrizeTableMasterAsync(
                Request.CreatePrizeTableMasterRequest request
        )
		{
			var task = new CreatePrizeTableMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class GetPrizeTableMasterTask : Gs2RestSessionTask<GetPrizeTableMasterRequest, GetPrizeTableMasterResult>
        {
            public GetPrizeTableMasterTask(IGs2Session session, RestSessionRequestFactory factory, GetPrizeTableMasterRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(GetPrizeTableMasterRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "lottery")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/master/table/{prizeTableName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{prizeTableName}", !string.IsNullOrEmpty(request.PrizeTableName) ? request.PrizeTableName.ToString() : "null");

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
		public IEnumerator GetPrizeTableMaster(
                Request.GetPrizeTableMasterRequest request,
                UnityAction<AsyncResult<Result.GetPrizeTableMasterResult>> callback
        )
		{
			var task = new GetPrizeTableMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.GetPrizeTableMasterResult>(task.Result, task.Error));
        }

		public IFuture<Result.GetPrizeTableMasterResult> GetPrizeTableMasterFuture(
                Request.GetPrizeTableMasterRequest request
        )
		{
			return new GetPrizeTableMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.GetPrizeTableMasterResult> GetPrizeTableMasterAsync(
                Request.GetPrizeTableMasterRequest request
        )
		{
            AsyncResult<Result.GetPrizeTableMasterResult> result = null;
			await GetPrizeTableMaster(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
    #else
		public GetPrizeTableMasterTask GetPrizeTableMasterAsync(
                Request.GetPrizeTableMasterRequest request
        )
		{
			return new GetPrizeTableMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.GetPrizeTableMasterResult> GetPrizeTableMasterAsync(
                Request.GetPrizeTableMasterRequest request
        )
		{
			var task = new GetPrizeTableMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class UpdatePrizeTableMasterTask : Gs2RestSessionTask<UpdatePrizeTableMasterRequest, UpdatePrizeTableMasterResult>
        {
            public UpdatePrizeTableMasterTask(IGs2Session session, RestSessionRequestFactory factory, UpdatePrizeTableMasterRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(UpdatePrizeTableMasterRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "lottery")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/master/table/{prizeTableName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{prizeTableName}", !string.IsNullOrEmpty(request.PrizeTableName) ? request.PrizeTableName.ToString() : "null");

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
                if (request.Prizes != null)
                {
                    jsonWriter.WritePropertyName("prizes");
                    jsonWriter.WriteArrayStart();
                    foreach(var item in request.Prizes)
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
		public IEnumerator UpdatePrizeTableMaster(
                Request.UpdatePrizeTableMasterRequest request,
                UnityAction<AsyncResult<Result.UpdatePrizeTableMasterResult>> callback
        )
		{
			var task = new UpdatePrizeTableMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.UpdatePrizeTableMasterResult>(task.Result, task.Error));
        }

		public IFuture<Result.UpdatePrizeTableMasterResult> UpdatePrizeTableMasterFuture(
                Request.UpdatePrizeTableMasterRequest request
        )
		{
			return new UpdatePrizeTableMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.UpdatePrizeTableMasterResult> UpdatePrizeTableMasterAsync(
                Request.UpdatePrizeTableMasterRequest request
        )
		{
            AsyncResult<Result.UpdatePrizeTableMasterResult> result = null;
			await UpdatePrizeTableMaster(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
    #else
		public UpdatePrizeTableMasterTask UpdatePrizeTableMasterAsync(
                Request.UpdatePrizeTableMasterRequest request
        )
		{
			return new UpdatePrizeTableMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.UpdatePrizeTableMasterResult> UpdatePrizeTableMasterAsync(
                Request.UpdatePrizeTableMasterRequest request
        )
		{
			var task = new UpdatePrizeTableMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class DeletePrizeTableMasterTask : Gs2RestSessionTask<DeletePrizeTableMasterRequest, DeletePrizeTableMasterResult>
        {
            public DeletePrizeTableMasterTask(IGs2Session session, RestSessionRequestFactory factory, DeletePrizeTableMasterRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DeletePrizeTableMasterRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "lottery")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/master/table/{prizeTableName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{prizeTableName}", !string.IsNullOrEmpty(request.PrizeTableName) ? request.PrizeTableName.ToString() : "null");

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
		public IEnumerator DeletePrizeTableMaster(
                Request.DeletePrizeTableMasterRequest request,
                UnityAction<AsyncResult<Result.DeletePrizeTableMasterResult>> callback
        )
		{
			var task = new DeletePrizeTableMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.DeletePrizeTableMasterResult>(task.Result, task.Error));
        }

		public IFuture<Result.DeletePrizeTableMasterResult> DeletePrizeTableMasterFuture(
                Request.DeletePrizeTableMasterRequest request
        )
		{
			return new DeletePrizeTableMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.DeletePrizeTableMasterResult> DeletePrizeTableMasterAsync(
                Request.DeletePrizeTableMasterRequest request
        )
		{
            AsyncResult<Result.DeletePrizeTableMasterResult> result = null;
			await DeletePrizeTableMaster(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
    #else
		public DeletePrizeTableMasterTask DeletePrizeTableMasterAsync(
                Request.DeletePrizeTableMasterRequest request
        )
		{
			return new DeletePrizeTableMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.DeletePrizeTableMasterResult> DeletePrizeTableMasterAsync(
                Request.DeletePrizeTableMasterRequest request
        )
		{
			var task = new DeletePrizeTableMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class DescribeBoxesTask : Gs2RestSessionTask<DescribeBoxesRequest, DescribeBoxesResult>
        {
            public DescribeBoxesTask(IGs2Session session, RestSessionRequestFactory factory, DescribeBoxesRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DescribeBoxesRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "lottery")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/me/box";

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
		public IEnumerator DescribeBoxes(
                Request.DescribeBoxesRequest request,
                UnityAction<AsyncResult<Result.DescribeBoxesResult>> callback
        )
		{
			var task = new DescribeBoxesTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.DescribeBoxesResult>(task.Result, task.Error));
        }

		public IFuture<Result.DescribeBoxesResult> DescribeBoxesFuture(
                Request.DescribeBoxesRequest request
        )
		{
			return new DescribeBoxesTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.DescribeBoxesResult> DescribeBoxesAsync(
                Request.DescribeBoxesRequest request
        )
		{
            AsyncResult<Result.DescribeBoxesResult> result = null;
			await DescribeBoxes(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
    #else
		public DescribeBoxesTask DescribeBoxesAsync(
                Request.DescribeBoxesRequest request
        )
		{
			return new DescribeBoxesTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.DescribeBoxesResult> DescribeBoxesAsync(
                Request.DescribeBoxesRequest request
        )
		{
			var task = new DescribeBoxesTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class DescribeBoxesByUserIdTask : Gs2RestSessionTask<DescribeBoxesByUserIdRequest, DescribeBoxesByUserIdResult>
        {
            public DescribeBoxesByUserIdTask(IGs2Session session, RestSessionRequestFactory factory, DescribeBoxesByUserIdRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DescribeBoxesByUserIdRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "lottery")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/{userId}/box";

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
		public IEnumerator DescribeBoxesByUserId(
                Request.DescribeBoxesByUserIdRequest request,
                UnityAction<AsyncResult<Result.DescribeBoxesByUserIdResult>> callback
        )
		{
			var task = new DescribeBoxesByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.DescribeBoxesByUserIdResult>(task.Result, task.Error));
        }

		public IFuture<Result.DescribeBoxesByUserIdResult> DescribeBoxesByUserIdFuture(
                Request.DescribeBoxesByUserIdRequest request
        )
		{
			return new DescribeBoxesByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.DescribeBoxesByUserIdResult> DescribeBoxesByUserIdAsync(
                Request.DescribeBoxesByUserIdRequest request
        )
		{
            AsyncResult<Result.DescribeBoxesByUserIdResult> result = null;
			await DescribeBoxesByUserId(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
    #else
		public DescribeBoxesByUserIdTask DescribeBoxesByUserIdAsync(
                Request.DescribeBoxesByUserIdRequest request
        )
		{
			return new DescribeBoxesByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.DescribeBoxesByUserIdResult> DescribeBoxesByUserIdAsync(
                Request.DescribeBoxesByUserIdRequest request
        )
		{
			var task = new DescribeBoxesByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class GetBoxTask : Gs2RestSessionTask<GetBoxRequest, GetBoxResult>
        {
            public GetBoxTask(IGs2Session session, RestSessionRequestFactory factory, GetBoxRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(GetBoxRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "lottery")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/me/box/{prizeTableName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{prizeTableName}", !string.IsNullOrEmpty(request.PrizeTableName) ? request.PrizeTableName.ToString() : "null");

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
		public IEnumerator GetBox(
                Request.GetBoxRequest request,
                UnityAction<AsyncResult<Result.GetBoxResult>> callback
        )
		{
			var task = new GetBoxTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.GetBoxResult>(task.Result, task.Error));
        }

		public IFuture<Result.GetBoxResult> GetBoxFuture(
                Request.GetBoxRequest request
        )
		{
			return new GetBoxTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.GetBoxResult> GetBoxAsync(
                Request.GetBoxRequest request
        )
		{
            AsyncResult<Result.GetBoxResult> result = null;
			await GetBox(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
    #else
		public GetBoxTask GetBoxAsync(
                Request.GetBoxRequest request
        )
		{
			return new GetBoxTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.GetBoxResult> GetBoxAsync(
                Request.GetBoxRequest request
        )
		{
			var task = new GetBoxTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class GetBoxByUserIdTask : Gs2RestSessionTask<GetBoxByUserIdRequest, GetBoxByUserIdResult>
        {
            public GetBoxByUserIdTask(IGs2Session session, RestSessionRequestFactory factory, GetBoxByUserIdRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(GetBoxByUserIdRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "lottery")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/{userId}/box/{prizeTableName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{prizeTableName}", !string.IsNullOrEmpty(request.PrizeTableName) ? request.PrizeTableName.ToString() : "null");
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
		public IEnumerator GetBoxByUserId(
                Request.GetBoxByUserIdRequest request,
                UnityAction<AsyncResult<Result.GetBoxByUserIdResult>> callback
        )
		{
			var task = new GetBoxByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.GetBoxByUserIdResult>(task.Result, task.Error));
        }

		public IFuture<Result.GetBoxByUserIdResult> GetBoxByUserIdFuture(
                Request.GetBoxByUserIdRequest request
        )
		{
			return new GetBoxByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.GetBoxByUserIdResult> GetBoxByUserIdAsync(
                Request.GetBoxByUserIdRequest request
        )
		{
            AsyncResult<Result.GetBoxByUserIdResult> result = null;
			await GetBoxByUserId(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
    #else
		public GetBoxByUserIdTask GetBoxByUserIdAsync(
                Request.GetBoxByUserIdRequest request
        )
		{
			return new GetBoxByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.GetBoxByUserIdResult> GetBoxByUserIdAsync(
                Request.GetBoxByUserIdRequest request
        )
		{
			var task = new GetBoxByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class ResetBoxTask : Gs2RestSessionTask<ResetBoxRequest, ResetBoxResult>
        {
            public ResetBoxTask(IGs2Session session, RestSessionRequestFactory factory, ResetBoxRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(ResetBoxRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "lottery")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/me/box/{prizeTableName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{prizeTableName}", !string.IsNullOrEmpty(request.PrizeTableName) ? request.PrizeTableName.ToString() : "null");

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
		public IEnumerator ResetBox(
                Request.ResetBoxRequest request,
                UnityAction<AsyncResult<Result.ResetBoxResult>> callback
        )
		{
			var task = new ResetBoxTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.ResetBoxResult>(task.Result, task.Error));
        }

		public IFuture<Result.ResetBoxResult> ResetBoxFuture(
                Request.ResetBoxRequest request
        )
		{
			return new ResetBoxTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.ResetBoxResult> ResetBoxAsync(
                Request.ResetBoxRequest request
        )
		{
            AsyncResult<Result.ResetBoxResult> result = null;
			await ResetBox(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
    #else
		public ResetBoxTask ResetBoxAsync(
                Request.ResetBoxRequest request
        )
		{
			return new ResetBoxTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.ResetBoxResult> ResetBoxAsync(
                Request.ResetBoxRequest request
        )
		{
			var task = new ResetBoxTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class ResetBoxByUserIdTask : Gs2RestSessionTask<ResetBoxByUserIdRequest, ResetBoxByUserIdResult>
        {
            public ResetBoxByUserIdTask(IGs2Session session, RestSessionRequestFactory factory, ResetBoxByUserIdRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(ResetBoxByUserIdRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "lottery")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/{userId}/box/{prizeTableName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{prizeTableName}", !string.IsNullOrEmpty(request.PrizeTableName) ? request.PrizeTableName.ToString() : "null");
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
		public IEnumerator ResetBoxByUserId(
                Request.ResetBoxByUserIdRequest request,
                UnityAction<AsyncResult<Result.ResetBoxByUserIdResult>> callback
        )
		{
			var task = new ResetBoxByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.ResetBoxByUserIdResult>(task.Result, task.Error));
        }

		public IFuture<Result.ResetBoxByUserIdResult> ResetBoxByUserIdFuture(
                Request.ResetBoxByUserIdRequest request
        )
		{
			return new ResetBoxByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.ResetBoxByUserIdResult> ResetBoxByUserIdAsync(
                Request.ResetBoxByUserIdRequest request
        )
		{
            AsyncResult<Result.ResetBoxByUserIdResult> result = null;
			await ResetBoxByUserId(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
    #else
		public ResetBoxByUserIdTask ResetBoxByUserIdAsync(
                Request.ResetBoxByUserIdRequest request
        )
		{
			return new ResetBoxByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.ResetBoxByUserIdResult> ResetBoxByUserIdAsync(
                Request.ResetBoxByUserIdRequest request
        )
		{
			var task = new ResetBoxByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class DescribeLotteryModelsTask : Gs2RestSessionTask<DescribeLotteryModelsRequest, DescribeLotteryModelsResult>
        {
            public DescribeLotteryModelsTask(IGs2Session session, RestSessionRequestFactory factory, DescribeLotteryModelsRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DescribeLotteryModelsRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "lottery")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/lottery";

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
		public IEnumerator DescribeLotteryModels(
                Request.DescribeLotteryModelsRequest request,
                UnityAction<AsyncResult<Result.DescribeLotteryModelsResult>> callback
        )
		{
			var task = new DescribeLotteryModelsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.DescribeLotteryModelsResult>(task.Result, task.Error));
        }

		public IFuture<Result.DescribeLotteryModelsResult> DescribeLotteryModelsFuture(
                Request.DescribeLotteryModelsRequest request
        )
		{
			return new DescribeLotteryModelsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.DescribeLotteryModelsResult> DescribeLotteryModelsAsync(
                Request.DescribeLotteryModelsRequest request
        )
		{
            AsyncResult<Result.DescribeLotteryModelsResult> result = null;
			await DescribeLotteryModels(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
    #else
		public DescribeLotteryModelsTask DescribeLotteryModelsAsync(
                Request.DescribeLotteryModelsRequest request
        )
		{
			return new DescribeLotteryModelsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.DescribeLotteryModelsResult> DescribeLotteryModelsAsync(
                Request.DescribeLotteryModelsRequest request
        )
		{
			var task = new DescribeLotteryModelsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class GetLotteryModelTask : Gs2RestSessionTask<GetLotteryModelRequest, GetLotteryModelResult>
        {
            public GetLotteryModelTask(IGs2Session session, RestSessionRequestFactory factory, GetLotteryModelRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(GetLotteryModelRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "lottery")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/lottery/{lotteryName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{lotteryName}", !string.IsNullOrEmpty(request.LotteryName) ? request.LotteryName.ToString() : "null");

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
		public IEnumerator GetLotteryModel(
                Request.GetLotteryModelRequest request,
                UnityAction<AsyncResult<Result.GetLotteryModelResult>> callback
        )
		{
			var task = new GetLotteryModelTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.GetLotteryModelResult>(task.Result, task.Error));
        }

		public IFuture<Result.GetLotteryModelResult> GetLotteryModelFuture(
                Request.GetLotteryModelRequest request
        )
		{
			return new GetLotteryModelTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.GetLotteryModelResult> GetLotteryModelAsync(
                Request.GetLotteryModelRequest request
        )
		{
            AsyncResult<Result.GetLotteryModelResult> result = null;
			await GetLotteryModel(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
    #else
		public GetLotteryModelTask GetLotteryModelAsync(
                Request.GetLotteryModelRequest request
        )
		{
			return new GetLotteryModelTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.GetLotteryModelResult> GetLotteryModelAsync(
                Request.GetLotteryModelRequest request
        )
		{
			var task = new GetLotteryModelTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class DescribePrizeTablesTask : Gs2RestSessionTask<DescribePrizeTablesRequest, DescribePrizeTablesResult>
        {
            public DescribePrizeTablesTask(IGs2Session session, RestSessionRequestFactory factory, DescribePrizeTablesRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DescribePrizeTablesRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "lottery")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/table";

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
		public IEnumerator DescribePrizeTables(
                Request.DescribePrizeTablesRequest request,
                UnityAction<AsyncResult<Result.DescribePrizeTablesResult>> callback
        )
		{
			var task = new DescribePrizeTablesTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.DescribePrizeTablesResult>(task.Result, task.Error));
        }

		public IFuture<Result.DescribePrizeTablesResult> DescribePrizeTablesFuture(
                Request.DescribePrizeTablesRequest request
        )
		{
			return new DescribePrizeTablesTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.DescribePrizeTablesResult> DescribePrizeTablesAsync(
                Request.DescribePrizeTablesRequest request
        )
		{
            AsyncResult<Result.DescribePrizeTablesResult> result = null;
			await DescribePrizeTables(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
    #else
		public DescribePrizeTablesTask DescribePrizeTablesAsync(
                Request.DescribePrizeTablesRequest request
        )
		{
			return new DescribePrizeTablesTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.DescribePrizeTablesResult> DescribePrizeTablesAsync(
                Request.DescribePrizeTablesRequest request
        )
		{
			var task = new DescribePrizeTablesTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class GetPrizeTableTask : Gs2RestSessionTask<GetPrizeTableRequest, GetPrizeTableResult>
        {
            public GetPrizeTableTask(IGs2Session session, RestSessionRequestFactory factory, GetPrizeTableRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(GetPrizeTableRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "lottery")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/table/{prizeTableName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{prizeTableName}", !string.IsNullOrEmpty(request.PrizeTableName) ? request.PrizeTableName.ToString() : "null");

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
		public IEnumerator GetPrizeTable(
                Request.GetPrizeTableRequest request,
                UnityAction<AsyncResult<Result.GetPrizeTableResult>> callback
        )
		{
			var task = new GetPrizeTableTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.GetPrizeTableResult>(task.Result, task.Error));
        }

		public IFuture<Result.GetPrizeTableResult> GetPrizeTableFuture(
                Request.GetPrizeTableRequest request
        )
		{
			return new GetPrizeTableTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.GetPrizeTableResult> GetPrizeTableAsync(
                Request.GetPrizeTableRequest request
        )
		{
            AsyncResult<Result.GetPrizeTableResult> result = null;
			await GetPrizeTable(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
    #else
		public GetPrizeTableTask GetPrizeTableAsync(
                Request.GetPrizeTableRequest request
        )
		{
			return new GetPrizeTableTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.GetPrizeTableResult> GetPrizeTableAsync(
                Request.GetPrizeTableRequest request
        )
		{
			var task = new GetPrizeTableTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class DrawByUserIdTask : Gs2RestSessionTask<DrawByUserIdRequest, DrawByUserIdResult>
        {
            public DrawByUserIdTask(IGs2Session session, RestSessionRequestFactory factory, DrawByUserIdRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DrawByUserIdRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "lottery")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/{userId}/lottery/{lotteryName}/draw";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{lotteryName}", !string.IsNullOrEmpty(request.LotteryName) ? request.LotteryName.ToString() : "null");
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
		public IEnumerator DrawByUserId(
                Request.DrawByUserIdRequest request,
                UnityAction<AsyncResult<Result.DrawByUserIdResult>> callback
        )
		{
			var task = new DrawByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.DrawByUserIdResult>(task.Result, task.Error));
        }

		public IFuture<Result.DrawByUserIdResult> DrawByUserIdFuture(
                Request.DrawByUserIdRequest request
        )
		{
			return new DrawByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.DrawByUserIdResult> DrawByUserIdAsync(
                Request.DrawByUserIdRequest request
        )
		{
            AsyncResult<Result.DrawByUserIdResult> result = null;
			await DrawByUserId(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
    #else
		public DrawByUserIdTask DrawByUserIdAsync(
                Request.DrawByUserIdRequest request
        )
		{
			return new DrawByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.DrawByUserIdResult> DrawByUserIdAsync(
                Request.DrawByUserIdRequest request
        )
		{
			var task = new DrawByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class DrawByStampSheetTask : Gs2RestSessionTask<DrawByStampSheetRequest, DrawByStampSheetResult>
        {
            public DrawByStampSheetTask(IGs2Session session, RestSessionRequestFactory factory, DrawByStampSheetRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DrawByStampSheetRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "lottery")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/stamp/draw";

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
		public IEnumerator DrawByStampSheet(
                Request.DrawByStampSheetRequest request,
                UnityAction<AsyncResult<Result.DrawByStampSheetResult>> callback
        )
		{
			var task = new DrawByStampSheetTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.DrawByStampSheetResult>(task.Result, task.Error));
        }

		public IFuture<Result.DrawByStampSheetResult> DrawByStampSheetFuture(
                Request.DrawByStampSheetRequest request
        )
		{
			return new DrawByStampSheetTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.DrawByStampSheetResult> DrawByStampSheetAsync(
                Request.DrawByStampSheetRequest request
        )
		{
            AsyncResult<Result.DrawByStampSheetResult> result = null;
			await DrawByStampSheet(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
    #else
		public DrawByStampSheetTask DrawByStampSheetAsync(
                Request.DrawByStampSheetRequest request
        )
		{
			return new DrawByStampSheetTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.DrawByStampSheetResult> DrawByStampSheetAsync(
                Request.DrawByStampSheetRequest request
        )
		{
			var task = new DrawByStampSheetTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class DescribeProbabilitiesTask : Gs2RestSessionTask<DescribeProbabilitiesRequest, DescribeProbabilitiesResult>
        {
            public DescribeProbabilitiesTask(IGs2Session session, RestSessionRequestFactory factory, DescribeProbabilitiesRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DescribeProbabilitiesRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "lottery")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/me/lottery/{lotteryName}/probability";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{lotteryName}", !string.IsNullOrEmpty(request.LotteryName) ? request.LotteryName.ToString() : "null");

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
		public IEnumerator DescribeProbabilities(
                Request.DescribeProbabilitiesRequest request,
                UnityAction<AsyncResult<Result.DescribeProbabilitiesResult>> callback
        )
		{
			var task = new DescribeProbabilitiesTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.DescribeProbabilitiesResult>(task.Result, task.Error));
        }

		public IFuture<Result.DescribeProbabilitiesResult> DescribeProbabilitiesFuture(
                Request.DescribeProbabilitiesRequest request
        )
		{
			return new DescribeProbabilitiesTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.DescribeProbabilitiesResult> DescribeProbabilitiesAsync(
                Request.DescribeProbabilitiesRequest request
        )
		{
            AsyncResult<Result.DescribeProbabilitiesResult> result = null;
			await DescribeProbabilities(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
    #else
		public DescribeProbabilitiesTask DescribeProbabilitiesAsync(
                Request.DescribeProbabilitiesRequest request
        )
		{
			return new DescribeProbabilitiesTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.DescribeProbabilitiesResult> DescribeProbabilitiesAsync(
                Request.DescribeProbabilitiesRequest request
        )
		{
			var task = new DescribeProbabilitiesTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class DescribeProbabilitiesByUserIdTask : Gs2RestSessionTask<DescribeProbabilitiesByUserIdRequest, DescribeProbabilitiesByUserIdResult>
        {
            public DescribeProbabilitiesByUserIdTask(IGs2Session session, RestSessionRequestFactory factory, DescribeProbabilitiesByUserIdRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DescribeProbabilitiesByUserIdRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "lottery")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/{userId}/lottery/{lotteryName}/probability";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{lotteryName}", !string.IsNullOrEmpty(request.LotteryName) ? request.LotteryName.ToString() : "null");
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
		public IEnumerator DescribeProbabilitiesByUserId(
                Request.DescribeProbabilitiesByUserIdRequest request,
                UnityAction<AsyncResult<Result.DescribeProbabilitiesByUserIdResult>> callback
        )
		{
			var task = new DescribeProbabilitiesByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.DescribeProbabilitiesByUserIdResult>(task.Result, task.Error));
        }

		public IFuture<Result.DescribeProbabilitiesByUserIdResult> DescribeProbabilitiesByUserIdFuture(
                Request.DescribeProbabilitiesByUserIdRequest request
        )
		{
			return new DescribeProbabilitiesByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.DescribeProbabilitiesByUserIdResult> DescribeProbabilitiesByUserIdAsync(
                Request.DescribeProbabilitiesByUserIdRequest request
        )
		{
            AsyncResult<Result.DescribeProbabilitiesByUserIdResult> result = null;
			await DescribeProbabilitiesByUserId(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
    #else
		public DescribeProbabilitiesByUserIdTask DescribeProbabilitiesByUserIdAsync(
                Request.DescribeProbabilitiesByUserIdRequest request
        )
		{
			return new DescribeProbabilitiesByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.DescribeProbabilitiesByUserIdResult> DescribeProbabilitiesByUserIdAsync(
                Request.DescribeProbabilitiesByUserIdRequest request
        )
		{
			var task = new DescribeProbabilitiesByUserIdTask(
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
                    .Replace("{service}", "lottery")
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


        public class GetCurrentLotteryMasterTask : Gs2RestSessionTask<GetCurrentLotteryMasterRequest, GetCurrentLotteryMasterResult>
        {
            public GetCurrentLotteryMasterTask(IGs2Session session, RestSessionRequestFactory factory, GetCurrentLotteryMasterRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(GetCurrentLotteryMasterRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "lottery")
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
		public IEnumerator GetCurrentLotteryMaster(
                Request.GetCurrentLotteryMasterRequest request,
                UnityAction<AsyncResult<Result.GetCurrentLotteryMasterResult>> callback
        )
		{
			var task = new GetCurrentLotteryMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.GetCurrentLotteryMasterResult>(task.Result, task.Error));
        }

		public IFuture<Result.GetCurrentLotteryMasterResult> GetCurrentLotteryMasterFuture(
                Request.GetCurrentLotteryMasterRequest request
        )
		{
			return new GetCurrentLotteryMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.GetCurrentLotteryMasterResult> GetCurrentLotteryMasterAsync(
                Request.GetCurrentLotteryMasterRequest request
        )
		{
            AsyncResult<Result.GetCurrentLotteryMasterResult> result = null;
			await GetCurrentLotteryMaster(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
    #else
		public GetCurrentLotteryMasterTask GetCurrentLotteryMasterAsync(
                Request.GetCurrentLotteryMasterRequest request
        )
		{
			return new GetCurrentLotteryMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.GetCurrentLotteryMasterResult> GetCurrentLotteryMasterAsync(
                Request.GetCurrentLotteryMasterRequest request
        )
		{
			var task = new GetCurrentLotteryMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class UpdateCurrentLotteryMasterTask : Gs2RestSessionTask<UpdateCurrentLotteryMasterRequest, UpdateCurrentLotteryMasterResult>
        {
            public UpdateCurrentLotteryMasterTask(IGs2Session session, RestSessionRequestFactory factory, UpdateCurrentLotteryMasterRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(UpdateCurrentLotteryMasterRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "lottery")
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
		public IEnumerator UpdateCurrentLotteryMaster(
                Request.UpdateCurrentLotteryMasterRequest request,
                UnityAction<AsyncResult<Result.UpdateCurrentLotteryMasterResult>> callback
        )
		{
			var task = new UpdateCurrentLotteryMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.UpdateCurrentLotteryMasterResult>(task.Result, task.Error));
        }

		public IFuture<Result.UpdateCurrentLotteryMasterResult> UpdateCurrentLotteryMasterFuture(
                Request.UpdateCurrentLotteryMasterRequest request
        )
		{
			return new UpdateCurrentLotteryMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.UpdateCurrentLotteryMasterResult> UpdateCurrentLotteryMasterAsync(
                Request.UpdateCurrentLotteryMasterRequest request
        )
		{
            AsyncResult<Result.UpdateCurrentLotteryMasterResult> result = null;
			await UpdateCurrentLotteryMaster(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
    #else
		public UpdateCurrentLotteryMasterTask UpdateCurrentLotteryMasterAsync(
                Request.UpdateCurrentLotteryMasterRequest request
        )
		{
			return new UpdateCurrentLotteryMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.UpdateCurrentLotteryMasterResult> UpdateCurrentLotteryMasterAsync(
                Request.UpdateCurrentLotteryMasterRequest request
        )
		{
			var task = new UpdateCurrentLotteryMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class UpdateCurrentLotteryMasterFromGitHubTask : Gs2RestSessionTask<UpdateCurrentLotteryMasterFromGitHubRequest, UpdateCurrentLotteryMasterFromGitHubResult>
        {
            public UpdateCurrentLotteryMasterFromGitHubTask(IGs2Session session, RestSessionRequestFactory factory, UpdateCurrentLotteryMasterFromGitHubRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(UpdateCurrentLotteryMasterFromGitHubRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "lottery")
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
		public IEnumerator UpdateCurrentLotteryMasterFromGitHub(
                Request.UpdateCurrentLotteryMasterFromGitHubRequest request,
                UnityAction<AsyncResult<Result.UpdateCurrentLotteryMasterFromGitHubResult>> callback
        )
		{
			var task = new UpdateCurrentLotteryMasterFromGitHubTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.UpdateCurrentLotteryMasterFromGitHubResult>(task.Result, task.Error));
        }

		public IFuture<Result.UpdateCurrentLotteryMasterFromGitHubResult> UpdateCurrentLotteryMasterFromGitHubFuture(
                Request.UpdateCurrentLotteryMasterFromGitHubRequest request
        )
		{
			return new UpdateCurrentLotteryMasterFromGitHubTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.UpdateCurrentLotteryMasterFromGitHubResult> UpdateCurrentLotteryMasterFromGitHubAsync(
                Request.UpdateCurrentLotteryMasterFromGitHubRequest request
        )
		{
            AsyncResult<Result.UpdateCurrentLotteryMasterFromGitHubResult> result = null;
			await UpdateCurrentLotteryMasterFromGitHub(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
    #else
		public UpdateCurrentLotteryMasterFromGitHubTask UpdateCurrentLotteryMasterFromGitHubAsync(
                Request.UpdateCurrentLotteryMasterFromGitHubRequest request
        )
		{
			return new UpdateCurrentLotteryMasterFromGitHubTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.UpdateCurrentLotteryMasterFromGitHubResult> UpdateCurrentLotteryMasterFromGitHubAsync(
                Request.UpdateCurrentLotteryMasterFromGitHubRequest request
        )
		{
			var task = new UpdateCurrentLotteryMasterFromGitHubTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif
	}
}