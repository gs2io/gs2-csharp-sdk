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
using Gs2.Gs2Enchant.Request;
using Gs2.Gs2Enchant.Result;
using Gs2.Util.LitJson;

namespace Gs2.Gs2Enchant
{
	public class Gs2EnchantRestClient : AbstractGs2Client
	{
#if UNITY_2017_1_OR_NEWER
		private readonly CertificateHandler _certificateHandler;
#endif

		public static string Endpoint = "enchant";

        protected Gs2RestSession Gs2RestSession => (Gs2RestSession) Gs2Session;

		public Gs2EnchantRestClient(Gs2RestSession Gs2RestSession) : base(Gs2RestSession)
		{

		}

#if UNITY_2017_1_OR_NEWER
		public Gs2EnchantRestClient(Gs2RestSession gs2RestSession, CertificateHandler certificateHandler) : base(gs2RestSession)
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
                    .Replace("{service}", "enchant")
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
                    .Replace("{service}", "enchant")
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
                    .Replace("{service}", "enchant")
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
                    .Replace("{service}", "enchant")
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
                    .Replace("{service}", "enchant")
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
                    .Replace("{service}", "enchant")
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
                    .Replace("{service}", "enchant")
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
                    .Replace("{service}", "enchant")
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
                    .Replace("{service}", "enchant")
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
                    .Replace("{service}", "enchant")
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
                    .Replace("{service}", "enchant")
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
                    .Replace("{service}", "enchant")
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
                    .Replace("{service}", "enchant")
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


        public class DescribeBalanceParameterModelsTask : Gs2RestSessionTask<DescribeBalanceParameterModelsRequest, DescribeBalanceParameterModelsResult>
        {
            public DescribeBalanceParameterModelsTask(IGs2Session session, RestSessionRequestFactory factory, DescribeBalanceParameterModelsRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DescribeBalanceParameterModelsRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "enchant")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/model/balance";

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
		public IEnumerator DescribeBalanceParameterModels(
                Request.DescribeBalanceParameterModelsRequest request,
                UnityAction<AsyncResult<Result.DescribeBalanceParameterModelsResult>> callback
        )
		{
			var task = new DescribeBalanceParameterModelsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.DescribeBalanceParameterModelsResult>(task.Result, task.Error));
        }

		public IFuture<Result.DescribeBalanceParameterModelsResult> DescribeBalanceParameterModelsFuture(
                Request.DescribeBalanceParameterModelsRequest request
        )
		{
			return new DescribeBalanceParameterModelsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.DescribeBalanceParameterModelsResult> DescribeBalanceParameterModelsAsync(
                Request.DescribeBalanceParameterModelsRequest request
        )
		{
            AsyncResult<Result.DescribeBalanceParameterModelsResult> result = null;
			await DescribeBalanceParameterModels(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
    #else
		public DescribeBalanceParameterModelsTask DescribeBalanceParameterModelsAsync(
                Request.DescribeBalanceParameterModelsRequest request
        )
		{
			return new DescribeBalanceParameterModelsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.DescribeBalanceParameterModelsResult> DescribeBalanceParameterModelsAsync(
                Request.DescribeBalanceParameterModelsRequest request
        )
		{
			var task = new DescribeBalanceParameterModelsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class GetBalanceParameterModelTask : Gs2RestSessionTask<GetBalanceParameterModelRequest, GetBalanceParameterModelResult>
        {
            public GetBalanceParameterModelTask(IGs2Session session, RestSessionRequestFactory factory, GetBalanceParameterModelRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(GetBalanceParameterModelRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "enchant")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/model/balance/{parameterName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{parameterName}", !string.IsNullOrEmpty(request.ParameterName) ? request.ParameterName.ToString() : "null");

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
		public IEnumerator GetBalanceParameterModel(
                Request.GetBalanceParameterModelRequest request,
                UnityAction<AsyncResult<Result.GetBalanceParameterModelResult>> callback
        )
		{
			var task = new GetBalanceParameterModelTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.GetBalanceParameterModelResult>(task.Result, task.Error));
        }

		public IFuture<Result.GetBalanceParameterModelResult> GetBalanceParameterModelFuture(
                Request.GetBalanceParameterModelRequest request
        )
		{
			return new GetBalanceParameterModelTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.GetBalanceParameterModelResult> GetBalanceParameterModelAsync(
                Request.GetBalanceParameterModelRequest request
        )
		{
            AsyncResult<Result.GetBalanceParameterModelResult> result = null;
			await GetBalanceParameterModel(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
    #else
		public GetBalanceParameterModelTask GetBalanceParameterModelAsync(
                Request.GetBalanceParameterModelRequest request
        )
		{
			return new GetBalanceParameterModelTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.GetBalanceParameterModelResult> GetBalanceParameterModelAsync(
                Request.GetBalanceParameterModelRequest request
        )
		{
			var task = new GetBalanceParameterModelTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class DescribeBalanceParameterModelMastersTask : Gs2RestSessionTask<DescribeBalanceParameterModelMastersRequest, DescribeBalanceParameterModelMastersResult>
        {
            public DescribeBalanceParameterModelMastersTask(IGs2Session session, RestSessionRequestFactory factory, DescribeBalanceParameterModelMastersRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DescribeBalanceParameterModelMastersRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "enchant")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/master/model/balance";

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
		public IEnumerator DescribeBalanceParameterModelMasters(
                Request.DescribeBalanceParameterModelMastersRequest request,
                UnityAction<AsyncResult<Result.DescribeBalanceParameterModelMastersResult>> callback
        )
		{
			var task = new DescribeBalanceParameterModelMastersTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.DescribeBalanceParameterModelMastersResult>(task.Result, task.Error));
        }

		public IFuture<Result.DescribeBalanceParameterModelMastersResult> DescribeBalanceParameterModelMastersFuture(
                Request.DescribeBalanceParameterModelMastersRequest request
        )
		{
			return new DescribeBalanceParameterModelMastersTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.DescribeBalanceParameterModelMastersResult> DescribeBalanceParameterModelMastersAsync(
                Request.DescribeBalanceParameterModelMastersRequest request
        )
		{
            AsyncResult<Result.DescribeBalanceParameterModelMastersResult> result = null;
			await DescribeBalanceParameterModelMasters(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
    #else
		public DescribeBalanceParameterModelMastersTask DescribeBalanceParameterModelMastersAsync(
                Request.DescribeBalanceParameterModelMastersRequest request
        )
		{
			return new DescribeBalanceParameterModelMastersTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.DescribeBalanceParameterModelMastersResult> DescribeBalanceParameterModelMastersAsync(
                Request.DescribeBalanceParameterModelMastersRequest request
        )
		{
			var task = new DescribeBalanceParameterModelMastersTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class CreateBalanceParameterModelMasterTask : Gs2RestSessionTask<CreateBalanceParameterModelMasterRequest, CreateBalanceParameterModelMasterResult>
        {
            public CreateBalanceParameterModelMasterTask(IGs2Session session, RestSessionRequestFactory factory, CreateBalanceParameterModelMasterRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(CreateBalanceParameterModelMasterRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "enchant")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/master/model/balance";

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
                if (request.TotalValue != null)
                {
                    jsonWriter.WritePropertyName("totalValue");
                    jsonWriter.Write(request.TotalValue.ToString());
                }
                if (request.InitialValueStrategy != null)
                {
                    jsonWriter.WritePropertyName("initialValueStrategy");
                    jsonWriter.Write(request.InitialValueStrategy);
                }
                if (request.Parameters != null)
                {
                    jsonWriter.WritePropertyName("parameters");
                    jsonWriter.WriteArrayStart();
                    foreach(var item in request.Parameters)
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
		public IEnumerator CreateBalanceParameterModelMaster(
                Request.CreateBalanceParameterModelMasterRequest request,
                UnityAction<AsyncResult<Result.CreateBalanceParameterModelMasterResult>> callback
        )
		{
			var task = new CreateBalanceParameterModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.CreateBalanceParameterModelMasterResult>(task.Result, task.Error));
        }

		public IFuture<Result.CreateBalanceParameterModelMasterResult> CreateBalanceParameterModelMasterFuture(
                Request.CreateBalanceParameterModelMasterRequest request
        )
		{
			return new CreateBalanceParameterModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.CreateBalanceParameterModelMasterResult> CreateBalanceParameterModelMasterAsync(
                Request.CreateBalanceParameterModelMasterRequest request
        )
		{
            AsyncResult<Result.CreateBalanceParameterModelMasterResult> result = null;
			await CreateBalanceParameterModelMaster(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
    #else
		public CreateBalanceParameterModelMasterTask CreateBalanceParameterModelMasterAsync(
                Request.CreateBalanceParameterModelMasterRequest request
        )
		{
			return new CreateBalanceParameterModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.CreateBalanceParameterModelMasterResult> CreateBalanceParameterModelMasterAsync(
                Request.CreateBalanceParameterModelMasterRequest request
        )
		{
			var task = new CreateBalanceParameterModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class GetBalanceParameterModelMasterTask : Gs2RestSessionTask<GetBalanceParameterModelMasterRequest, GetBalanceParameterModelMasterResult>
        {
            public GetBalanceParameterModelMasterTask(IGs2Session session, RestSessionRequestFactory factory, GetBalanceParameterModelMasterRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(GetBalanceParameterModelMasterRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "enchant")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/master/model/balance/{parameterName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{parameterName}", !string.IsNullOrEmpty(request.ParameterName) ? request.ParameterName.ToString() : "null");

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
		public IEnumerator GetBalanceParameterModelMaster(
                Request.GetBalanceParameterModelMasterRequest request,
                UnityAction<AsyncResult<Result.GetBalanceParameterModelMasterResult>> callback
        )
		{
			var task = new GetBalanceParameterModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.GetBalanceParameterModelMasterResult>(task.Result, task.Error));
        }

		public IFuture<Result.GetBalanceParameterModelMasterResult> GetBalanceParameterModelMasterFuture(
                Request.GetBalanceParameterModelMasterRequest request
        )
		{
			return new GetBalanceParameterModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.GetBalanceParameterModelMasterResult> GetBalanceParameterModelMasterAsync(
                Request.GetBalanceParameterModelMasterRequest request
        )
		{
            AsyncResult<Result.GetBalanceParameterModelMasterResult> result = null;
			await GetBalanceParameterModelMaster(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
    #else
		public GetBalanceParameterModelMasterTask GetBalanceParameterModelMasterAsync(
                Request.GetBalanceParameterModelMasterRequest request
        )
		{
			return new GetBalanceParameterModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.GetBalanceParameterModelMasterResult> GetBalanceParameterModelMasterAsync(
                Request.GetBalanceParameterModelMasterRequest request
        )
		{
			var task = new GetBalanceParameterModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class UpdateBalanceParameterModelMasterTask : Gs2RestSessionTask<UpdateBalanceParameterModelMasterRequest, UpdateBalanceParameterModelMasterResult>
        {
            public UpdateBalanceParameterModelMasterTask(IGs2Session session, RestSessionRequestFactory factory, UpdateBalanceParameterModelMasterRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(UpdateBalanceParameterModelMasterRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "enchant")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/master/model/balance/{parameterName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{parameterName}", !string.IsNullOrEmpty(request.ParameterName) ? request.ParameterName.ToString() : "null");

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
                if (request.TotalValue != null)
                {
                    jsonWriter.WritePropertyName("totalValue");
                    jsonWriter.Write(request.TotalValue.ToString());
                }
                if (request.InitialValueStrategy != null)
                {
                    jsonWriter.WritePropertyName("initialValueStrategy");
                    jsonWriter.Write(request.InitialValueStrategy);
                }
                if (request.Parameters != null)
                {
                    jsonWriter.WritePropertyName("parameters");
                    jsonWriter.WriteArrayStart();
                    foreach(var item in request.Parameters)
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
		public IEnumerator UpdateBalanceParameterModelMaster(
                Request.UpdateBalanceParameterModelMasterRequest request,
                UnityAction<AsyncResult<Result.UpdateBalanceParameterModelMasterResult>> callback
        )
		{
			var task = new UpdateBalanceParameterModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.UpdateBalanceParameterModelMasterResult>(task.Result, task.Error));
        }

		public IFuture<Result.UpdateBalanceParameterModelMasterResult> UpdateBalanceParameterModelMasterFuture(
                Request.UpdateBalanceParameterModelMasterRequest request
        )
		{
			return new UpdateBalanceParameterModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.UpdateBalanceParameterModelMasterResult> UpdateBalanceParameterModelMasterAsync(
                Request.UpdateBalanceParameterModelMasterRequest request
        )
		{
            AsyncResult<Result.UpdateBalanceParameterModelMasterResult> result = null;
			await UpdateBalanceParameterModelMaster(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
    #else
		public UpdateBalanceParameterModelMasterTask UpdateBalanceParameterModelMasterAsync(
                Request.UpdateBalanceParameterModelMasterRequest request
        )
		{
			return new UpdateBalanceParameterModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.UpdateBalanceParameterModelMasterResult> UpdateBalanceParameterModelMasterAsync(
                Request.UpdateBalanceParameterModelMasterRequest request
        )
		{
			var task = new UpdateBalanceParameterModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class DeleteBalanceParameterModelMasterTask : Gs2RestSessionTask<DeleteBalanceParameterModelMasterRequest, DeleteBalanceParameterModelMasterResult>
        {
            public DeleteBalanceParameterModelMasterTask(IGs2Session session, RestSessionRequestFactory factory, DeleteBalanceParameterModelMasterRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DeleteBalanceParameterModelMasterRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "enchant")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/master/model/balance/{parameterName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{parameterName}", !string.IsNullOrEmpty(request.ParameterName) ? request.ParameterName.ToString() : "null");

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
		public IEnumerator DeleteBalanceParameterModelMaster(
                Request.DeleteBalanceParameterModelMasterRequest request,
                UnityAction<AsyncResult<Result.DeleteBalanceParameterModelMasterResult>> callback
        )
		{
			var task = new DeleteBalanceParameterModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.DeleteBalanceParameterModelMasterResult>(task.Result, task.Error));
        }

		public IFuture<Result.DeleteBalanceParameterModelMasterResult> DeleteBalanceParameterModelMasterFuture(
                Request.DeleteBalanceParameterModelMasterRequest request
        )
		{
			return new DeleteBalanceParameterModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.DeleteBalanceParameterModelMasterResult> DeleteBalanceParameterModelMasterAsync(
                Request.DeleteBalanceParameterModelMasterRequest request
        )
		{
            AsyncResult<Result.DeleteBalanceParameterModelMasterResult> result = null;
			await DeleteBalanceParameterModelMaster(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
    #else
		public DeleteBalanceParameterModelMasterTask DeleteBalanceParameterModelMasterAsync(
                Request.DeleteBalanceParameterModelMasterRequest request
        )
		{
			return new DeleteBalanceParameterModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.DeleteBalanceParameterModelMasterResult> DeleteBalanceParameterModelMasterAsync(
                Request.DeleteBalanceParameterModelMasterRequest request
        )
		{
			var task = new DeleteBalanceParameterModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class DescribeRarityParameterModelsTask : Gs2RestSessionTask<DescribeRarityParameterModelsRequest, DescribeRarityParameterModelsResult>
        {
            public DescribeRarityParameterModelsTask(IGs2Session session, RestSessionRequestFactory factory, DescribeRarityParameterModelsRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DescribeRarityParameterModelsRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "enchant")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/model/rarity";

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
		public IEnumerator DescribeRarityParameterModels(
                Request.DescribeRarityParameterModelsRequest request,
                UnityAction<AsyncResult<Result.DescribeRarityParameterModelsResult>> callback
        )
		{
			var task = new DescribeRarityParameterModelsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.DescribeRarityParameterModelsResult>(task.Result, task.Error));
        }

		public IFuture<Result.DescribeRarityParameterModelsResult> DescribeRarityParameterModelsFuture(
                Request.DescribeRarityParameterModelsRequest request
        )
		{
			return new DescribeRarityParameterModelsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.DescribeRarityParameterModelsResult> DescribeRarityParameterModelsAsync(
                Request.DescribeRarityParameterModelsRequest request
        )
		{
            AsyncResult<Result.DescribeRarityParameterModelsResult> result = null;
			await DescribeRarityParameterModels(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
    #else
		public DescribeRarityParameterModelsTask DescribeRarityParameterModelsAsync(
                Request.DescribeRarityParameterModelsRequest request
        )
		{
			return new DescribeRarityParameterModelsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.DescribeRarityParameterModelsResult> DescribeRarityParameterModelsAsync(
                Request.DescribeRarityParameterModelsRequest request
        )
		{
			var task = new DescribeRarityParameterModelsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class GetRarityParameterModelTask : Gs2RestSessionTask<GetRarityParameterModelRequest, GetRarityParameterModelResult>
        {
            public GetRarityParameterModelTask(IGs2Session session, RestSessionRequestFactory factory, GetRarityParameterModelRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(GetRarityParameterModelRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "enchant")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/model/rarity/{parameterName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{parameterName}", !string.IsNullOrEmpty(request.ParameterName) ? request.ParameterName.ToString() : "null");

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
		public IEnumerator GetRarityParameterModel(
                Request.GetRarityParameterModelRequest request,
                UnityAction<AsyncResult<Result.GetRarityParameterModelResult>> callback
        )
		{
			var task = new GetRarityParameterModelTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.GetRarityParameterModelResult>(task.Result, task.Error));
        }

		public IFuture<Result.GetRarityParameterModelResult> GetRarityParameterModelFuture(
                Request.GetRarityParameterModelRequest request
        )
		{
			return new GetRarityParameterModelTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.GetRarityParameterModelResult> GetRarityParameterModelAsync(
                Request.GetRarityParameterModelRequest request
        )
		{
            AsyncResult<Result.GetRarityParameterModelResult> result = null;
			await GetRarityParameterModel(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
    #else
		public GetRarityParameterModelTask GetRarityParameterModelAsync(
                Request.GetRarityParameterModelRequest request
        )
		{
			return new GetRarityParameterModelTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.GetRarityParameterModelResult> GetRarityParameterModelAsync(
                Request.GetRarityParameterModelRequest request
        )
		{
			var task = new GetRarityParameterModelTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class DescribeRarityParameterModelMastersTask : Gs2RestSessionTask<DescribeRarityParameterModelMastersRequest, DescribeRarityParameterModelMastersResult>
        {
            public DescribeRarityParameterModelMastersTask(IGs2Session session, RestSessionRequestFactory factory, DescribeRarityParameterModelMastersRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DescribeRarityParameterModelMastersRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "enchant")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/master/model/rarity";

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
		public IEnumerator DescribeRarityParameterModelMasters(
                Request.DescribeRarityParameterModelMastersRequest request,
                UnityAction<AsyncResult<Result.DescribeRarityParameterModelMastersResult>> callback
        )
		{
			var task = new DescribeRarityParameterModelMastersTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.DescribeRarityParameterModelMastersResult>(task.Result, task.Error));
        }

		public IFuture<Result.DescribeRarityParameterModelMastersResult> DescribeRarityParameterModelMastersFuture(
                Request.DescribeRarityParameterModelMastersRequest request
        )
		{
			return new DescribeRarityParameterModelMastersTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.DescribeRarityParameterModelMastersResult> DescribeRarityParameterModelMastersAsync(
                Request.DescribeRarityParameterModelMastersRequest request
        )
		{
            AsyncResult<Result.DescribeRarityParameterModelMastersResult> result = null;
			await DescribeRarityParameterModelMasters(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
    #else
		public DescribeRarityParameterModelMastersTask DescribeRarityParameterModelMastersAsync(
                Request.DescribeRarityParameterModelMastersRequest request
        )
		{
			return new DescribeRarityParameterModelMastersTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.DescribeRarityParameterModelMastersResult> DescribeRarityParameterModelMastersAsync(
                Request.DescribeRarityParameterModelMastersRequest request
        )
		{
			var task = new DescribeRarityParameterModelMastersTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class CreateRarityParameterModelMasterTask : Gs2RestSessionTask<CreateRarityParameterModelMasterRequest, CreateRarityParameterModelMasterResult>
        {
            public CreateRarityParameterModelMasterTask(IGs2Session session, RestSessionRequestFactory factory, CreateRarityParameterModelMasterRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(CreateRarityParameterModelMasterRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "enchant")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/master/model/rarity";

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
                if (request.MaximumParameterCount != null)
                {
                    jsonWriter.WritePropertyName("maximumParameterCount");
                    jsonWriter.Write(request.MaximumParameterCount.ToString());
                }
                if (request.ParameterCounts != null)
                {
                    jsonWriter.WritePropertyName("parameterCounts");
                    jsonWriter.WriteArrayStart();
                    foreach(var item in request.ParameterCounts)
                    {
                        item.WriteJson(jsonWriter);
                    }
                    jsonWriter.WriteArrayEnd();
                }
                if (request.Parameters != null)
                {
                    jsonWriter.WritePropertyName("parameters");
                    jsonWriter.WriteArrayStart();
                    foreach(var item in request.Parameters)
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
		public IEnumerator CreateRarityParameterModelMaster(
                Request.CreateRarityParameterModelMasterRequest request,
                UnityAction<AsyncResult<Result.CreateRarityParameterModelMasterResult>> callback
        )
		{
			var task = new CreateRarityParameterModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.CreateRarityParameterModelMasterResult>(task.Result, task.Error));
        }

		public IFuture<Result.CreateRarityParameterModelMasterResult> CreateRarityParameterModelMasterFuture(
                Request.CreateRarityParameterModelMasterRequest request
        )
		{
			return new CreateRarityParameterModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.CreateRarityParameterModelMasterResult> CreateRarityParameterModelMasterAsync(
                Request.CreateRarityParameterModelMasterRequest request
        )
		{
            AsyncResult<Result.CreateRarityParameterModelMasterResult> result = null;
			await CreateRarityParameterModelMaster(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
    #else
		public CreateRarityParameterModelMasterTask CreateRarityParameterModelMasterAsync(
                Request.CreateRarityParameterModelMasterRequest request
        )
		{
			return new CreateRarityParameterModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.CreateRarityParameterModelMasterResult> CreateRarityParameterModelMasterAsync(
                Request.CreateRarityParameterModelMasterRequest request
        )
		{
			var task = new CreateRarityParameterModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class GetRarityParameterModelMasterTask : Gs2RestSessionTask<GetRarityParameterModelMasterRequest, GetRarityParameterModelMasterResult>
        {
            public GetRarityParameterModelMasterTask(IGs2Session session, RestSessionRequestFactory factory, GetRarityParameterModelMasterRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(GetRarityParameterModelMasterRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "enchant")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/master/model/rarity/{parameterName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{parameterName}", !string.IsNullOrEmpty(request.ParameterName) ? request.ParameterName.ToString() : "null");

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
		public IEnumerator GetRarityParameterModelMaster(
                Request.GetRarityParameterModelMasterRequest request,
                UnityAction<AsyncResult<Result.GetRarityParameterModelMasterResult>> callback
        )
		{
			var task = new GetRarityParameterModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.GetRarityParameterModelMasterResult>(task.Result, task.Error));
        }

		public IFuture<Result.GetRarityParameterModelMasterResult> GetRarityParameterModelMasterFuture(
                Request.GetRarityParameterModelMasterRequest request
        )
		{
			return new GetRarityParameterModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.GetRarityParameterModelMasterResult> GetRarityParameterModelMasterAsync(
                Request.GetRarityParameterModelMasterRequest request
        )
		{
            AsyncResult<Result.GetRarityParameterModelMasterResult> result = null;
			await GetRarityParameterModelMaster(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
    #else
		public GetRarityParameterModelMasterTask GetRarityParameterModelMasterAsync(
                Request.GetRarityParameterModelMasterRequest request
        )
		{
			return new GetRarityParameterModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.GetRarityParameterModelMasterResult> GetRarityParameterModelMasterAsync(
                Request.GetRarityParameterModelMasterRequest request
        )
		{
			var task = new GetRarityParameterModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class UpdateRarityParameterModelMasterTask : Gs2RestSessionTask<UpdateRarityParameterModelMasterRequest, UpdateRarityParameterModelMasterResult>
        {
            public UpdateRarityParameterModelMasterTask(IGs2Session session, RestSessionRequestFactory factory, UpdateRarityParameterModelMasterRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(UpdateRarityParameterModelMasterRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "enchant")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/master/model/rarity/{parameterName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{parameterName}", !string.IsNullOrEmpty(request.ParameterName) ? request.ParameterName.ToString() : "null");

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
                if (request.MaximumParameterCount != null)
                {
                    jsonWriter.WritePropertyName("maximumParameterCount");
                    jsonWriter.Write(request.MaximumParameterCount.ToString());
                }
                if (request.ParameterCounts != null)
                {
                    jsonWriter.WritePropertyName("parameterCounts");
                    jsonWriter.WriteArrayStart();
                    foreach(var item in request.ParameterCounts)
                    {
                        item.WriteJson(jsonWriter);
                    }
                    jsonWriter.WriteArrayEnd();
                }
                if (request.Parameters != null)
                {
                    jsonWriter.WritePropertyName("parameters");
                    jsonWriter.WriteArrayStart();
                    foreach(var item in request.Parameters)
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
		public IEnumerator UpdateRarityParameterModelMaster(
                Request.UpdateRarityParameterModelMasterRequest request,
                UnityAction<AsyncResult<Result.UpdateRarityParameterModelMasterResult>> callback
        )
		{
			var task = new UpdateRarityParameterModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.UpdateRarityParameterModelMasterResult>(task.Result, task.Error));
        }

		public IFuture<Result.UpdateRarityParameterModelMasterResult> UpdateRarityParameterModelMasterFuture(
                Request.UpdateRarityParameterModelMasterRequest request
        )
		{
			return new UpdateRarityParameterModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.UpdateRarityParameterModelMasterResult> UpdateRarityParameterModelMasterAsync(
                Request.UpdateRarityParameterModelMasterRequest request
        )
		{
            AsyncResult<Result.UpdateRarityParameterModelMasterResult> result = null;
			await UpdateRarityParameterModelMaster(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
    #else
		public UpdateRarityParameterModelMasterTask UpdateRarityParameterModelMasterAsync(
                Request.UpdateRarityParameterModelMasterRequest request
        )
		{
			return new UpdateRarityParameterModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.UpdateRarityParameterModelMasterResult> UpdateRarityParameterModelMasterAsync(
                Request.UpdateRarityParameterModelMasterRequest request
        )
		{
			var task = new UpdateRarityParameterModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class DeleteRarityParameterModelMasterTask : Gs2RestSessionTask<DeleteRarityParameterModelMasterRequest, DeleteRarityParameterModelMasterResult>
        {
            public DeleteRarityParameterModelMasterTask(IGs2Session session, RestSessionRequestFactory factory, DeleteRarityParameterModelMasterRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DeleteRarityParameterModelMasterRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "enchant")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/master/model/rarity/{parameterName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{parameterName}", !string.IsNullOrEmpty(request.ParameterName) ? request.ParameterName.ToString() : "null");

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
		public IEnumerator DeleteRarityParameterModelMaster(
                Request.DeleteRarityParameterModelMasterRequest request,
                UnityAction<AsyncResult<Result.DeleteRarityParameterModelMasterResult>> callback
        )
		{
			var task = new DeleteRarityParameterModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.DeleteRarityParameterModelMasterResult>(task.Result, task.Error));
        }

		public IFuture<Result.DeleteRarityParameterModelMasterResult> DeleteRarityParameterModelMasterFuture(
                Request.DeleteRarityParameterModelMasterRequest request
        )
		{
			return new DeleteRarityParameterModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.DeleteRarityParameterModelMasterResult> DeleteRarityParameterModelMasterAsync(
                Request.DeleteRarityParameterModelMasterRequest request
        )
		{
            AsyncResult<Result.DeleteRarityParameterModelMasterResult> result = null;
			await DeleteRarityParameterModelMaster(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
    #else
		public DeleteRarityParameterModelMasterTask DeleteRarityParameterModelMasterAsync(
                Request.DeleteRarityParameterModelMasterRequest request
        )
		{
			return new DeleteRarityParameterModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.DeleteRarityParameterModelMasterResult> DeleteRarityParameterModelMasterAsync(
                Request.DeleteRarityParameterModelMasterRequest request
        )
		{
			var task = new DeleteRarityParameterModelMasterTask(
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
                    .Replace("{service}", "enchant")
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


        public class GetCurrentParameterMasterTask : Gs2RestSessionTask<GetCurrentParameterMasterRequest, GetCurrentParameterMasterResult>
        {
            public GetCurrentParameterMasterTask(IGs2Session session, RestSessionRequestFactory factory, GetCurrentParameterMasterRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(GetCurrentParameterMasterRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "enchant")
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
		public IEnumerator GetCurrentParameterMaster(
                Request.GetCurrentParameterMasterRequest request,
                UnityAction<AsyncResult<Result.GetCurrentParameterMasterResult>> callback
        )
		{
			var task = new GetCurrentParameterMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.GetCurrentParameterMasterResult>(task.Result, task.Error));
        }

		public IFuture<Result.GetCurrentParameterMasterResult> GetCurrentParameterMasterFuture(
                Request.GetCurrentParameterMasterRequest request
        )
		{
			return new GetCurrentParameterMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.GetCurrentParameterMasterResult> GetCurrentParameterMasterAsync(
                Request.GetCurrentParameterMasterRequest request
        )
		{
            AsyncResult<Result.GetCurrentParameterMasterResult> result = null;
			await GetCurrentParameterMaster(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
    #else
		public GetCurrentParameterMasterTask GetCurrentParameterMasterAsync(
                Request.GetCurrentParameterMasterRequest request
        )
		{
			return new GetCurrentParameterMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.GetCurrentParameterMasterResult> GetCurrentParameterMasterAsync(
                Request.GetCurrentParameterMasterRequest request
        )
		{
			var task = new GetCurrentParameterMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class UpdateCurrentParameterMasterTask : Gs2RestSessionTask<UpdateCurrentParameterMasterRequest, UpdateCurrentParameterMasterResult>
        {
            public UpdateCurrentParameterMasterTask(IGs2Session session, RestSessionRequestFactory factory, UpdateCurrentParameterMasterRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(UpdateCurrentParameterMasterRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "enchant")
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
		public IEnumerator UpdateCurrentParameterMaster(
                Request.UpdateCurrentParameterMasterRequest request,
                UnityAction<AsyncResult<Result.UpdateCurrentParameterMasterResult>> callback
        )
		{
			var task = new UpdateCurrentParameterMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.UpdateCurrentParameterMasterResult>(task.Result, task.Error));
        }

		public IFuture<Result.UpdateCurrentParameterMasterResult> UpdateCurrentParameterMasterFuture(
                Request.UpdateCurrentParameterMasterRequest request
        )
		{
			return new UpdateCurrentParameterMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.UpdateCurrentParameterMasterResult> UpdateCurrentParameterMasterAsync(
                Request.UpdateCurrentParameterMasterRequest request
        )
		{
            AsyncResult<Result.UpdateCurrentParameterMasterResult> result = null;
			await UpdateCurrentParameterMaster(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
    #else
		public UpdateCurrentParameterMasterTask UpdateCurrentParameterMasterAsync(
                Request.UpdateCurrentParameterMasterRequest request
        )
		{
			return new UpdateCurrentParameterMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.UpdateCurrentParameterMasterResult> UpdateCurrentParameterMasterAsync(
                Request.UpdateCurrentParameterMasterRequest request
        )
		{
			var task = new UpdateCurrentParameterMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class UpdateCurrentParameterMasterFromGitHubTask : Gs2RestSessionTask<UpdateCurrentParameterMasterFromGitHubRequest, UpdateCurrentParameterMasterFromGitHubResult>
        {
            public UpdateCurrentParameterMasterFromGitHubTask(IGs2Session session, RestSessionRequestFactory factory, UpdateCurrentParameterMasterFromGitHubRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(UpdateCurrentParameterMasterFromGitHubRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "enchant")
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
		public IEnumerator UpdateCurrentParameterMasterFromGitHub(
                Request.UpdateCurrentParameterMasterFromGitHubRequest request,
                UnityAction<AsyncResult<Result.UpdateCurrentParameterMasterFromGitHubResult>> callback
        )
		{
			var task = new UpdateCurrentParameterMasterFromGitHubTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.UpdateCurrentParameterMasterFromGitHubResult>(task.Result, task.Error));
        }

		public IFuture<Result.UpdateCurrentParameterMasterFromGitHubResult> UpdateCurrentParameterMasterFromGitHubFuture(
                Request.UpdateCurrentParameterMasterFromGitHubRequest request
        )
		{
			return new UpdateCurrentParameterMasterFromGitHubTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.UpdateCurrentParameterMasterFromGitHubResult> UpdateCurrentParameterMasterFromGitHubAsync(
                Request.UpdateCurrentParameterMasterFromGitHubRequest request
        )
		{
            AsyncResult<Result.UpdateCurrentParameterMasterFromGitHubResult> result = null;
			await UpdateCurrentParameterMasterFromGitHub(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
    #else
		public UpdateCurrentParameterMasterFromGitHubTask UpdateCurrentParameterMasterFromGitHubAsync(
                Request.UpdateCurrentParameterMasterFromGitHubRequest request
        )
		{
			return new UpdateCurrentParameterMasterFromGitHubTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.UpdateCurrentParameterMasterFromGitHubResult> UpdateCurrentParameterMasterFromGitHubAsync(
                Request.UpdateCurrentParameterMasterFromGitHubRequest request
        )
		{
			var task = new UpdateCurrentParameterMasterFromGitHubTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class DescribeBalanceParameterStatusesTask : Gs2RestSessionTask<DescribeBalanceParameterStatusesRequest, DescribeBalanceParameterStatusesResult>
        {
            public DescribeBalanceParameterStatusesTask(IGs2Session session, RestSessionRequestFactory factory, DescribeBalanceParameterStatusesRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DescribeBalanceParameterStatusesRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "enchant")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/me/status/balance";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");

                var sessionRequest = Factory.Get(url);
                if (request.ContextStack != null)
                {
                    sessionRequest.AddQueryString("contextStack", request.ContextStack);
                }
                if (request.ParameterName != null) {
                    sessionRequest.AddQueryString("parameterName", $"{request.ParameterName}");
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
		public IEnumerator DescribeBalanceParameterStatuses(
                Request.DescribeBalanceParameterStatusesRequest request,
                UnityAction<AsyncResult<Result.DescribeBalanceParameterStatusesResult>> callback
        )
		{
			var task = new DescribeBalanceParameterStatusesTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.DescribeBalanceParameterStatusesResult>(task.Result, task.Error));
        }

		public IFuture<Result.DescribeBalanceParameterStatusesResult> DescribeBalanceParameterStatusesFuture(
                Request.DescribeBalanceParameterStatusesRequest request
        )
		{
			return new DescribeBalanceParameterStatusesTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.DescribeBalanceParameterStatusesResult> DescribeBalanceParameterStatusesAsync(
                Request.DescribeBalanceParameterStatusesRequest request
        )
		{
            AsyncResult<Result.DescribeBalanceParameterStatusesResult> result = null;
			await DescribeBalanceParameterStatuses(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
    #else
		public DescribeBalanceParameterStatusesTask DescribeBalanceParameterStatusesAsync(
                Request.DescribeBalanceParameterStatusesRequest request
        )
		{
			return new DescribeBalanceParameterStatusesTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.DescribeBalanceParameterStatusesResult> DescribeBalanceParameterStatusesAsync(
                Request.DescribeBalanceParameterStatusesRequest request
        )
		{
			var task = new DescribeBalanceParameterStatusesTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class DescribeBalanceParameterStatusesByUserIdTask : Gs2RestSessionTask<DescribeBalanceParameterStatusesByUserIdRequest, DescribeBalanceParameterStatusesByUserIdResult>
        {
            public DescribeBalanceParameterStatusesByUserIdTask(IGs2Session session, RestSessionRequestFactory factory, DescribeBalanceParameterStatusesByUserIdRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DescribeBalanceParameterStatusesByUserIdRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "enchant")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/{userId}/status/balance";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{userId}", !string.IsNullOrEmpty(request.UserId) ? request.UserId.ToString() : "null");

                var sessionRequest = Factory.Get(url);
                if (request.ContextStack != null)
                {
                    sessionRequest.AddQueryString("contextStack", request.ContextStack);
                }
                if (request.ParameterName != null) {
                    sessionRequest.AddQueryString("parameterName", $"{request.ParameterName}");
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
		public IEnumerator DescribeBalanceParameterStatusesByUserId(
                Request.DescribeBalanceParameterStatusesByUserIdRequest request,
                UnityAction<AsyncResult<Result.DescribeBalanceParameterStatusesByUserIdResult>> callback
        )
		{
			var task = new DescribeBalanceParameterStatusesByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.DescribeBalanceParameterStatusesByUserIdResult>(task.Result, task.Error));
        }

		public IFuture<Result.DescribeBalanceParameterStatusesByUserIdResult> DescribeBalanceParameterStatusesByUserIdFuture(
                Request.DescribeBalanceParameterStatusesByUserIdRequest request
        )
		{
			return new DescribeBalanceParameterStatusesByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.DescribeBalanceParameterStatusesByUserIdResult> DescribeBalanceParameterStatusesByUserIdAsync(
                Request.DescribeBalanceParameterStatusesByUserIdRequest request
        )
		{
            AsyncResult<Result.DescribeBalanceParameterStatusesByUserIdResult> result = null;
			await DescribeBalanceParameterStatusesByUserId(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
    #else
		public DescribeBalanceParameterStatusesByUserIdTask DescribeBalanceParameterStatusesByUserIdAsync(
                Request.DescribeBalanceParameterStatusesByUserIdRequest request
        )
		{
			return new DescribeBalanceParameterStatusesByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.DescribeBalanceParameterStatusesByUserIdResult> DescribeBalanceParameterStatusesByUserIdAsync(
                Request.DescribeBalanceParameterStatusesByUserIdRequest request
        )
		{
			var task = new DescribeBalanceParameterStatusesByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class GetBalanceParameterStatusTask : Gs2RestSessionTask<GetBalanceParameterStatusRequest, GetBalanceParameterStatusResult>
        {
            public GetBalanceParameterStatusTask(IGs2Session session, RestSessionRequestFactory factory, GetBalanceParameterStatusRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(GetBalanceParameterStatusRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "enchant")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/me/status/balance/{parameterName}/{propertyId}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{parameterName}", !string.IsNullOrEmpty(request.ParameterName) ? request.ParameterName.ToString() : "null");
                url = url.Replace("{propertyId}", !string.IsNullOrEmpty(request.PropertyId) ? request.PropertyId.ToString() : "null");

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
		public IEnumerator GetBalanceParameterStatus(
                Request.GetBalanceParameterStatusRequest request,
                UnityAction<AsyncResult<Result.GetBalanceParameterStatusResult>> callback
        )
		{
			var task = new GetBalanceParameterStatusTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.GetBalanceParameterStatusResult>(task.Result, task.Error));
        }

		public IFuture<Result.GetBalanceParameterStatusResult> GetBalanceParameterStatusFuture(
                Request.GetBalanceParameterStatusRequest request
        )
		{
			return new GetBalanceParameterStatusTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.GetBalanceParameterStatusResult> GetBalanceParameterStatusAsync(
                Request.GetBalanceParameterStatusRequest request
        )
		{
            AsyncResult<Result.GetBalanceParameterStatusResult> result = null;
			await GetBalanceParameterStatus(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
    #else
		public GetBalanceParameterStatusTask GetBalanceParameterStatusAsync(
                Request.GetBalanceParameterStatusRequest request
        )
		{
			return new GetBalanceParameterStatusTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.GetBalanceParameterStatusResult> GetBalanceParameterStatusAsync(
                Request.GetBalanceParameterStatusRequest request
        )
		{
			var task = new GetBalanceParameterStatusTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class GetBalanceParameterStatusByUserIdTask : Gs2RestSessionTask<GetBalanceParameterStatusByUserIdRequest, GetBalanceParameterStatusByUserIdResult>
        {
            public GetBalanceParameterStatusByUserIdTask(IGs2Session session, RestSessionRequestFactory factory, GetBalanceParameterStatusByUserIdRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(GetBalanceParameterStatusByUserIdRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "enchant")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/{userId}/status/balance/{parameterName}/{propertyId}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{userId}", !string.IsNullOrEmpty(request.UserId) ? request.UserId.ToString() : "null");
                url = url.Replace("{parameterName}", !string.IsNullOrEmpty(request.ParameterName) ? request.ParameterName.ToString() : "null");
                url = url.Replace("{propertyId}", !string.IsNullOrEmpty(request.PropertyId) ? request.PropertyId.ToString() : "null");

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
		public IEnumerator GetBalanceParameterStatusByUserId(
                Request.GetBalanceParameterStatusByUserIdRequest request,
                UnityAction<AsyncResult<Result.GetBalanceParameterStatusByUserIdResult>> callback
        )
		{
			var task = new GetBalanceParameterStatusByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.GetBalanceParameterStatusByUserIdResult>(task.Result, task.Error));
        }

		public IFuture<Result.GetBalanceParameterStatusByUserIdResult> GetBalanceParameterStatusByUserIdFuture(
                Request.GetBalanceParameterStatusByUserIdRequest request
        )
		{
			return new GetBalanceParameterStatusByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.GetBalanceParameterStatusByUserIdResult> GetBalanceParameterStatusByUserIdAsync(
                Request.GetBalanceParameterStatusByUserIdRequest request
        )
		{
            AsyncResult<Result.GetBalanceParameterStatusByUserIdResult> result = null;
			await GetBalanceParameterStatusByUserId(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
    #else
		public GetBalanceParameterStatusByUserIdTask GetBalanceParameterStatusByUserIdAsync(
                Request.GetBalanceParameterStatusByUserIdRequest request
        )
		{
			return new GetBalanceParameterStatusByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.GetBalanceParameterStatusByUserIdResult> GetBalanceParameterStatusByUserIdAsync(
                Request.GetBalanceParameterStatusByUserIdRequest request
        )
		{
			var task = new GetBalanceParameterStatusByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class DeleteBalanceParameterStatusByUserIdTask : Gs2RestSessionTask<DeleteBalanceParameterStatusByUserIdRequest, DeleteBalanceParameterStatusByUserIdResult>
        {
            public DeleteBalanceParameterStatusByUserIdTask(IGs2Session session, RestSessionRequestFactory factory, DeleteBalanceParameterStatusByUserIdRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DeleteBalanceParameterStatusByUserIdRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "enchant")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/{userId}/status/balance/{parameterName}/{propertyId}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{userId}", !string.IsNullOrEmpty(request.UserId) ? request.UserId.ToString() : "null");
                url = url.Replace("{parameterName}", !string.IsNullOrEmpty(request.ParameterName) ? request.ParameterName.ToString() : "null");
                url = url.Replace("{propertyId}", !string.IsNullOrEmpty(request.PropertyId) ? request.PropertyId.ToString() : "null");

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
		public IEnumerator DeleteBalanceParameterStatusByUserId(
                Request.DeleteBalanceParameterStatusByUserIdRequest request,
                UnityAction<AsyncResult<Result.DeleteBalanceParameterStatusByUserIdResult>> callback
        )
		{
			var task = new DeleteBalanceParameterStatusByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.DeleteBalanceParameterStatusByUserIdResult>(task.Result, task.Error));
        }

		public IFuture<Result.DeleteBalanceParameterStatusByUserIdResult> DeleteBalanceParameterStatusByUserIdFuture(
                Request.DeleteBalanceParameterStatusByUserIdRequest request
        )
		{
			return new DeleteBalanceParameterStatusByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.DeleteBalanceParameterStatusByUserIdResult> DeleteBalanceParameterStatusByUserIdAsync(
                Request.DeleteBalanceParameterStatusByUserIdRequest request
        )
		{
            AsyncResult<Result.DeleteBalanceParameterStatusByUserIdResult> result = null;
			await DeleteBalanceParameterStatusByUserId(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
    #else
		public DeleteBalanceParameterStatusByUserIdTask DeleteBalanceParameterStatusByUserIdAsync(
                Request.DeleteBalanceParameterStatusByUserIdRequest request
        )
		{
			return new DeleteBalanceParameterStatusByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.DeleteBalanceParameterStatusByUserIdResult> DeleteBalanceParameterStatusByUserIdAsync(
                Request.DeleteBalanceParameterStatusByUserIdRequest request
        )
		{
			var task = new DeleteBalanceParameterStatusByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class ReDrawBalanceParameterStatusByUserIdTask : Gs2RestSessionTask<ReDrawBalanceParameterStatusByUserIdRequest, ReDrawBalanceParameterStatusByUserIdResult>
        {
            public ReDrawBalanceParameterStatusByUserIdTask(IGs2Session session, RestSessionRequestFactory factory, ReDrawBalanceParameterStatusByUserIdRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(ReDrawBalanceParameterStatusByUserIdRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "enchant")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/{userId}/status/balance/{parameterName}/{propertyId}/redraw";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{userId}", !string.IsNullOrEmpty(request.UserId) ? request.UserId.ToString() : "null");
                url = url.Replace("{parameterName}", !string.IsNullOrEmpty(request.ParameterName) ? request.ParameterName.ToString() : "null");
                url = url.Replace("{propertyId}", !string.IsNullOrEmpty(request.PropertyId) ? request.PropertyId.ToString() : "null");

                var sessionRequest = Factory.Post(url);

                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);
                jsonWriter.WriteObjectStart();
                if (request.FixedParameterNames != null)
                {
                    jsonWriter.WritePropertyName("fixedParameterNames");
                    jsonWriter.WriteArrayStart();
                    foreach(var item in request.FixedParameterNames)
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
		public IEnumerator ReDrawBalanceParameterStatusByUserId(
                Request.ReDrawBalanceParameterStatusByUserIdRequest request,
                UnityAction<AsyncResult<Result.ReDrawBalanceParameterStatusByUserIdResult>> callback
        )
		{
			var task = new ReDrawBalanceParameterStatusByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.ReDrawBalanceParameterStatusByUserIdResult>(task.Result, task.Error));
        }

		public IFuture<Result.ReDrawBalanceParameterStatusByUserIdResult> ReDrawBalanceParameterStatusByUserIdFuture(
                Request.ReDrawBalanceParameterStatusByUserIdRequest request
        )
		{
			return new ReDrawBalanceParameterStatusByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.ReDrawBalanceParameterStatusByUserIdResult> ReDrawBalanceParameterStatusByUserIdAsync(
                Request.ReDrawBalanceParameterStatusByUserIdRequest request
        )
		{
            AsyncResult<Result.ReDrawBalanceParameterStatusByUserIdResult> result = null;
			await ReDrawBalanceParameterStatusByUserId(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
    #else
		public ReDrawBalanceParameterStatusByUserIdTask ReDrawBalanceParameterStatusByUserIdAsync(
                Request.ReDrawBalanceParameterStatusByUserIdRequest request
        )
		{
			return new ReDrawBalanceParameterStatusByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.ReDrawBalanceParameterStatusByUserIdResult> ReDrawBalanceParameterStatusByUserIdAsync(
                Request.ReDrawBalanceParameterStatusByUserIdRequest request
        )
		{
			var task = new ReDrawBalanceParameterStatusByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class ReDrawBalanceParameterStatusByStampSheetTask : Gs2RestSessionTask<ReDrawBalanceParameterStatusByStampSheetRequest, ReDrawBalanceParameterStatusByStampSheetResult>
        {
            public ReDrawBalanceParameterStatusByStampSheetTask(IGs2Session session, RestSessionRequestFactory factory, ReDrawBalanceParameterStatusByStampSheetRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(ReDrawBalanceParameterStatusByStampSheetRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "enchant")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/stamp/balance/redraw";

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
		public IEnumerator ReDrawBalanceParameterStatusByStampSheet(
                Request.ReDrawBalanceParameterStatusByStampSheetRequest request,
                UnityAction<AsyncResult<Result.ReDrawBalanceParameterStatusByStampSheetResult>> callback
        )
		{
			var task = new ReDrawBalanceParameterStatusByStampSheetTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.ReDrawBalanceParameterStatusByStampSheetResult>(task.Result, task.Error));
        }

		public IFuture<Result.ReDrawBalanceParameterStatusByStampSheetResult> ReDrawBalanceParameterStatusByStampSheetFuture(
                Request.ReDrawBalanceParameterStatusByStampSheetRequest request
        )
		{
			return new ReDrawBalanceParameterStatusByStampSheetTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.ReDrawBalanceParameterStatusByStampSheetResult> ReDrawBalanceParameterStatusByStampSheetAsync(
                Request.ReDrawBalanceParameterStatusByStampSheetRequest request
        )
		{
            AsyncResult<Result.ReDrawBalanceParameterStatusByStampSheetResult> result = null;
			await ReDrawBalanceParameterStatusByStampSheet(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
    #else
		public ReDrawBalanceParameterStatusByStampSheetTask ReDrawBalanceParameterStatusByStampSheetAsync(
                Request.ReDrawBalanceParameterStatusByStampSheetRequest request
        )
		{
			return new ReDrawBalanceParameterStatusByStampSheetTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.ReDrawBalanceParameterStatusByStampSheetResult> ReDrawBalanceParameterStatusByStampSheetAsync(
                Request.ReDrawBalanceParameterStatusByStampSheetRequest request
        )
		{
			var task = new ReDrawBalanceParameterStatusByStampSheetTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class SetBalanceParameterStatusByUserIdTask : Gs2RestSessionTask<SetBalanceParameterStatusByUserIdRequest, SetBalanceParameterStatusByUserIdResult>
        {
            public SetBalanceParameterStatusByUserIdTask(IGs2Session session, RestSessionRequestFactory factory, SetBalanceParameterStatusByUserIdRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(SetBalanceParameterStatusByUserIdRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "enchant")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/{userId}/status/balance/{parameterName}/{propertyId}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{userId}", !string.IsNullOrEmpty(request.UserId) ? request.UserId.ToString() : "null");
                url = url.Replace("{parameterName}", !string.IsNullOrEmpty(request.ParameterName) ? request.ParameterName.ToString() : "null");
                url = url.Replace("{propertyId}", !string.IsNullOrEmpty(request.PropertyId) ? request.PropertyId.ToString() : "null");

                var sessionRequest = Factory.Put(url);

                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);
                jsonWriter.WriteObjectStart();
                if (request.ParameterValues != null)
                {
                    jsonWriter.WritePropertyName("parameterValues");
                    jsonWriter.WriteArrayStart();
                    foreach(var item in request.ParameterValues)
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
		public IEnumerator SetBalanceParameterStatusByUserId(
                Request.SetBalanceParameterStatusByUserIdRequest request,
                UnityAction<AsyncResult<Result.SetBalanceParameterStatusByUserIdResult>> callback
        )
		{
			var task = new SetBalanceParameterStatusByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.SetBalanceParameterStatusByUserIdResult>(task.Result, task.Error));
        }

		public IFuture<Result.SetBalanceParameterStatusByUserIdResult> SetBalanceParameterStatusByUserIdFuture(
                Request.SetBalanceParameterStatusByUserIdRequest request
        )
		{
			return new SetBalanceParameterStatusByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.SetBalanceParameterStatusByUserIdResult> SetBalanceParameterStatusByUserIdAsync(
                Request.SetBalanceParameterStatusByUserIdRequest request
        )
		{
            AsyncResult<Result.SetBalanceParameterStatusByUserIdResult> result = null;
			await SetBalanceParameterStatusByUserId(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
    #else
		public SetBalanceParameterStatusByUserIdTask SetBalanceParameterStatusByUserIdAsync(
                Request.SetBalanceParameterStatusByUserIdRequest request
        )
		{
			return new SetBalanceParameterStatusByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.SetBalanceParameterStatusByUserIdResult> SetBalanceParameterStatusByUserIdAsync(
                Request.SetBalanceParameterStatusByUserIdRequest request
        )
		{
			var task = new SetBalanceParameterStatusByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class SetBalanceParameterStatusByStampSheetTask : Gs2RestSessionTask<SetBalanceParameterStatusByStampSheetRequest, SetBalanceParameterStatusByStampSheetResult>
        {
            public SetBalanceParameterStatusByStampSheetTask(IGs2Session session, RestSessionRequestFactory factory, SetBalanceParameterStatusByStampSheetRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(SetBalanceParameterStatusByStampSheetRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "enchant")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/stamp/balance/set";

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
		public IEnumerator SetBalanceParameterStatusByStampSheet(
                Request.SetBalanceParameterStatusByStampSheetRequest request,
                UnityAction<AsyncResult<Result.SetBalanceParameterStatusByStampSheetResult>> callback
        )
		{
			var task = new SetBalanceParameterStatusByStampSheetTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.SetBalanceParameterStatusByStampSheetResult>(task.Result, task.Error));
        }

		public IFuture<Result.SetBalanceParameterStatusByStampSheetResult> SetBalanceParameterStatusByStampSheetFuture(
                Request.SetBalanceParameterStatusByStampSheetRequest request
        )
		{
			return new SetBalanceParameterStatusByStampSheetTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.SetBalanceParameterStatusByStampSheetResult> SetBalanceParameterStatusByStampSheetAsync(
                Request.SetBalanceParameterStatusByStampSheetRequest request
        )
		{
            AsyncResult<Result.SetBalanceParameterStatusByStampSheetResult> result = null;
			await SetBalanceParameterStatusByStampSheet(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
    #else
		public SetBalanceParameterStatusByStampSheetTask SetBalanceParameterStatusByStampSheetAsync(
                Request.SetBalanceParameterStatusByStampSheetRequest request
        )
		{
			return new SetBalanceParameterStatusByStampSheetTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.SetBalanceParameterStatusByStampSheetResult> SetBalanceParameterStatusByStampSheetAsync(
                Request.SetBalanceParameterStatusByStampSheetRequest request
        )
		{
			var task = new SetBalanceParameterStatusByStampSheetTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class DescribeRarityParameterStatusesTask : Gs2RestSessionTask<DescribeRarityParameterStatusesRequest, DescribeRarityParameterStatusesResult>
        {
            public DescribeRarityParameterStatusesTask(IGs2Session session, RestSessionRequestFactory factory, DescribeRarityParameterStatusesRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DescribeRarityParameterStatusesRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "enchant")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/me/status/rarity";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");

                var sessionRequest = Factory.Get(url);
                if (request.ContextStack != null)
                {
                    sessionRequest.AddQueryString("contextStack", request.ContextStack);
                }
                if (request.ParameterName != null) {
                    sessionRequest.AddQueryString("parameterName", $"{request.ParameterName}");
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
		public IEnumerator DescribeRarityParameterStatuses(
                Request.DescribeRarityParameterStatusesRequest request,
                UnityAction<AsyncResult<Result.DescribeRarityParameterStatusesResult>> callback
        )
		{
			var task = new DescribeRarityParameterStatusesTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.DescribeRarityParameterStatusesResult>(task.Result, task.Error));
        }

		public IFuture<Result.DescribeRarityParameterStatusesResult> DescribeRarityParameterStatusesFuture(
                Request.DescribeRarityParameterStatusesRequest request
        )
		{
			return new DescribeRarityParameterStatusesTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.DescribeRarityParameterStatusesResult> DescribeRarityParameterStatusesAsync(
                Request.DescribeRarityParameterStatusesRequest request
        )
		{
            AsyncResult<Result.DescribeRarityParameterStatusesResult> result = null;
			await DescribeRarityParameterStatuses(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
    #else
		public DescribeRarityParameterStatusesTask DescribeRarityParameterStatusesAsync(
                Request.DescribeRarityParameterStatusesRequest request
        )
		{
			return new DescribeRarityParameterStatusesTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.DescribeRarityParameterStatusesResult> DescribeRarityParameterStatusesAsync(
                Request.DescribeRarityParameterStatusesRequest request
        )
		{
			var task = new DescribeRarityParameterStatusesTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class DescribeRarityParameterStatusesByUserIdTask : Gs2RestSessionTask<DescribeRarityParameterStatusesByUserIdRequest, DescribeRarityParameterStatusesByUserIdResult>
        {
            public DescribeRarityParameterStatusesByUserIdTask(IGs2Session session, RestSessionRequestFactory factory, DescribeRarityParameterStatusesByUserIdRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DescribeRarityParameterStatusesByUserIdRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "enchant")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/{userId}/status/rarity";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{userId}", !string.IsNullOrEmpty(request.UserId) ? request.UserId.ToString() : "null");

                var sessionRequest = Factory.Get(url);
                if (request.ContextStack != null)
                {
                    sessionRequest.AddQueryString("contextStack", request.ContextStack);
                }
                if (request.ParameterName != null) {
                    sessionRequest.AddQueryString("parameterName", $"{request.ParameterName}");
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
		public IEnumerator DescribeRarityParameterStatusesByUserId(
                Request.DescribeRarityParameterStatusesByUserIdRequest request,
                UnityAction<AsyncResult<Result.DescribeRarityParameterStatusesByUserIdResult>> callback
        )
		{
			var task = new DescribeRarityParameterStatusesByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.DescribeRarityParameterStatusesByUserIdResult>(task.Result, task.Error));
        }

		public IFuture<Result.DescribeRarityParameterStatusesByUserIdResult> DescribeRarityParameterStatusesByUserIdFuture(
                Request.DescribeRarityParameterStatusesByUserIdRequest request
        )
		{
			return new DescribeRarityParameterStatusesByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.DescribeRarityParameterStatusesByUserIdResult> DescribeRarityParameterStatusesByUserIdAsync(
                Request.DescribeRarityParameterStatusesByUserIdRequest request
        )
		{
            AsyncResult<Result.DescribeRarityParameterStatusesByUserIdResult> result = null;
			await DescribeRarityParameterStatusesByUserId(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
    #else
		public DescribeRarityParameterStatusesByUserIdTask DescribeRarityParameterStatusesByUserIdAsync(
                Request.DescribeRarityParameterStatusesByUserIdRequest request
        )
		{
			return new DescribeRarityParameterStatusesByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.DescribeRarityParameterStatusesByUserIdResult> DescribeRarityParameterStatusesByUserIdAsync(
                Request.DescribeRarityParameterStatusesByUserIdRequest request
        )
		{
			var task = new DescribeRarityParameterStatusesByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class GetRarityParameterStatusTask : Gs2RestSessionTask<GetRarityParameterStatusRequest, GetRarityParameterStatusResult>
        {
            public GetRarityParameterStatusTask(IGs2Session session, RestSessionRequestFactory factory, GetRarityParameterStatusRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(GetRarityParameterStatusRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "enchant")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/me/status/rarity/{parameterName}/{propertyId}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{parameterName}", !string.IsNullOrEmpty(request.ParameterName) ? request.ParameterName.ToString() : "null");
                url = url.Replace("{propertyId}", !string.IsNullOrEmpty(request.PropertyId) ? request.PropertyId.ToString() : "null");

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
		public IEnumerator GetRarityParameterStatus(
                Request.GetRarityParameterStatusRequest request,
                UnityAction<AsyncResult<Result.GetRarityParameterStatusResult>> callback
        )
		{
			var task = new GetRarityParameterStatusTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.GetRarityParameterStatusResult>(task.Result, task.Error));
        }

		public IFuture<Result.GetRarityParameterStatusResult> GetRarityParameterStatusFuture(
                Request.GetRarityParameterStatusRequest request
        )
		{
			return new GetRarityParameterStatusTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.GetRarityParameterStatusResult> GetRarityParameterStatusAsync(
                Request.GetRarityParameterStatusRequest request
        )
		{
            AsyncResult<Result.GetRarityParameterStatusResult> result = null;
			await GetRarityParameterStatus(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
    #else
		public GetRarityParameterStatusTask GetRarityParameterStatusAsync(
                Request.GetRarityParameterStatusRequest request
        )
		{
			return new GetRarityParameterStatusTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.GetRarityParameterStatusResult> GetRarityParameterStatusAsync(
                Request.GetRarityParameterStatusRequest request
        )
		{
			var task = new GetRarityParameterStatusTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class GetRarityParameterStatusByUserIdTask : Gs2RestSessionTask<GetRarityParameterStatusByUserIdRequest, GetRarityParameterStatusByUserIdResult>
        {
            public GetRarityParameterStatusByUserIdTask(IGs2Session session, RestSessionRequestFactory factory, GetRarityParameterStatusByUserIdRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(GetRarityParameterStatusByUserIdRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "enchant")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/{userId}/status/rarity/{parameterName}/{propertyId}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{userId}", !string.IsNullOrEmpty(request.UserId) ? request.UserId.ToString() : "null");
                url = url.Replace("{parameterName}", !string.IsNullOrEmpty(request.ParameterName) ? request.ParameterName.ToString() : "null");
                url = url.Replace("{propertyId}", !string.IsNullOrEmpty(request.PropertyId) ? request.PropertyId.ToString() : "null");

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
		public IEnumerator GetRarityParameterStatusByUserId(
                Request.GetRarityParameterStatusByUserIdRequest request,
                UnityAction<AsyncResult<Result.GetRarityParameterStatusByUserIdResult>> callback
        )
		{
			var task = new GetRarityParameterStatusByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.GetRarityParameterStatusByUserIdResult>(task.Result, task.Error));
        }

		public IFuture<Result.GetRarityParameterStatusByUserIdResult> GetRarityParameterStatusByUserIdFuture(
                Request.GetRarityParameterStatusByUserIdRequest request
        )
		{
			return new GetRarityParameterStatusByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.GetRarityParameterStatusByUserIdResult> GetRarityParameterStatusByUserIdAsync(
                Request.GetRarityParameterStatusByUserIdRequest request
        )
		{
            AsyncResult<Result.GetRarityParameterStatusByUserIdResult> result = null;
			await GetRarityParameterStatusByUserId(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
    #else
		public GetRarityParameterStatusByUserIdTask GetRarityParameterStatusByUserIdAsync(
                Request.GetRarityParameterStatusByUserIdRequest request
        )
		{
			return new GetRarityParameterStatusByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.GetRarityParameterStatusByUserIdResult> GetRarityParameterStatusByUserIdAsync(
                Request.GetRarityParameterStatusByUserIdRequest request
        )
		{
			var task = new GetRarityParameterStatusByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class DeleteRarityParameterStatusByUserIdTask : Gs2RestSessionTask<DeleteRarityParameterStatusByUserIdRequest, DeleteRarityParameterStatusByUserIdResult>
        {
            public DeleteRarityParameterStatusByUserIdTask(IGs2Session session, RestSessionRequestFactory factory, DeleteRarityParameterStatusByUserIdRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DeleteRarityParameterStatusByUserIdRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "enchant")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/{userId}/status/rarity/{parameterName}/{propertyId}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{userId}", !string.IsNullOrEmpty(request.UserId) ? request.UserId.ToString() : "null");
                url = url.Replace("{parameterName}", !string.IsNullOrEmpty(request.ParameterName) ? request.ParameterName.ToString() : "null");
                url = url.Replace("{propertyId}", !string.IsNullOrEmpty(request.PropertyId) ? request.PropertyId.ToString() : "null");

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
		public IEnumerator DeleteRarityParameterStatusByUserId(
                Request.DeleteRarityParameterStatusByUserIdRequest request,
                UnityAction<AsyncResult<Result.DeleteRarityParameterStatusByUserIdResult>> callback
        )
		{
			var task = new DeleteRarityParameterStatusByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.DeleteRarityParameterStatusByUserIdResult>(task.Result, task.Error));
        }

		public IFuture<Result.DeleteRarityParameterStatusByUserIdResult> DeleteRarityParameterStatusByUserIdFuture(
                Request.DeleteRarityParameterStatusByUserIdRequest request
        )
		{
			return new DeleteRarityParameterStatusByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.DeleteRarityParameterStatusByUserIdResult> DeleteRarityParameterStatusByUserIdAsync(
                Request.DeleteRarityParameterStatusByUserIdRequest request
        )
		{
            AsyncResult<Result.DeleteRarityParameterStatusByUserIdResult> result = null;
			await DeleteRarityParameterStatusByUserId(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
    #else
		public DeleteRarityParameterStatusByUserIdTask DeleteRarityParameterStatusByUserIdAsync(
                Request.DeleteRarityParameterStatusByUserIdRequest request
        )
		{
			return new DeleteRarityParameterStatusByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.DeleteRarityParameterStatusByUserIdResult> DeleteRarityParameterStatusByUserIdAsync(
                Request.DeleteRarityParameterStatusByUserIdRequest request
        )
		{
			var task = new DeleteRarityParameterStatusByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class ReDrawRarityParameterStatusByUserIdTask : Gs2RestSessionTask<ReDrawRarityParameterStatusByUserIdRequest, ReDrawRarityParameterStatusByUserIdResult>
        {
            public ReDrawRarityParameterStatusByUserIdTask(IGs2Session session, RestSessionRequestFactory factory, ReDrawRarityParameterStatusByUserIdRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(ReDrawRarityParameterStatusByUserIdRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "enchant")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/{userId}/status/rarity/{parameterName}/{propertyId}/redraw";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{userId}", !string.IsNullOrEmpty(request.UserId) ? request.UserId.ToString() : "null");
                url = url.Replace("{parameterName}", !string.IsNullOrEmpty(request.ParameterName) ? request.ParameterName.ToString() : "null");
                url = url.Replace("{propertyId}", !string.IsNullOrEmpty(request.PropertyId) ? request.PropertyId.ToString() : "null");

                var sessionRequest = Factory.Post(url);

                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);
                jsonWriter.WriteObjectStart();
                if (request.FixedParameterNames != null)
                {
                    jsonWriter.WritePropertyName("fixedParameterNames");
                    jsonWriter.WriteArrayStart();
                    foreach(var item in request.FixedParameterNames)
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
		public IEnumerator ReDrawRarityParameterStatusByUserId(
                Request.ReDrawRarityParameterStatusByUserIdRequest request,
                UnityAction<AsyncResult<Result.ReDrawRarityParameterStatusByUserIdResult>> callback
        )
		{
			var task = new ReDrawRarityParameterStatusByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.ReDrawRarityParameterStatusByUserIdResult>(task.Result, task.Error));
        }

		public IFuture<Result.ReDrawRarityParameterStatusByUserIdResult> ReDrawRarityParameterStatusByUserIdFuture(
                Request.ReDrawRarityParameterStatusByUserIdRequest request
        )
		{
			return new ReDrawRarityParameterStatusByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.ReDrawRarityParameterStatusByUserIdResult> ReDrawRarityParameterStatusByUserIdAsync(
                Request.ReDrawRarityParameterStatusByUserIdRequest request
        )
		{
            AsyncResult<Result.ReDrawRarityParameterStatusByUserIdResult> result = null;
			await ReDrawRarityParameterStatusByUserId(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
    #else
		public ReDrawRarityParameterStatusByUserIdTask ReDrawRarityParameterStatusByUserIdAsync(
                Request.ReDrawRarityParameterStatusByUserIdRequest request
        )
		{
			return new ReDrawRarityParameterStatusByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.ReDrawRarityParameterStatusByUserIdResult> ReDrawRarityParameterStatusByUserIdAsync(
                Request.ReDrawRarityParameterStatusByUserIdRequest request
        )
		{
			var task = new ReDrawRarityParameterStatusByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class ReDrawRarityParameterStatusByStampSheetTask : Gs2RestSessionTask<ReDrawRarityParameterStatusByStampSheetRequest, ReDrawRarityParameterStatusByStampSheetResult>
        {
            public ReDrawRarityParameterStatusByStampSheetTask(IGs2Session session, RestSessionRequestFactory factory, ReDrawRarityParameterStatusByStampSheetRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(ReDrawRarityParameterStatusByStampSheetRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "enchant")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/stamp/rarity/parameter/redraw";

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
		public IEnumerator ReDrawRarityParameterStatusByStampSheet(
                Request.ReDrawRarityParameterStatusByStampSheetRequest request,
                UnityAction<AsyncResult<Result.ReDrawRarityParameterStatusByStampSheetResult>> callback
        )
		{
			var task = new ReDrawRarityParameterStatusByStampSheetTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.ReDrawRarityParameterStatusByStampSheetResult>(task.Result, task.Error));
        }

		public IFuture<Result.ReDrawRarityParameterStatusByStampSheetResult> ReDrawRarityParameterStatusByStampSheetFuture(
                Request.ReDrawRarityParameterStatusByStampSheetRequest request
        )
		{
			return new ReDrawRarityParameterStatusByStampSheetTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.ReDrawRarityParameterStatusByStampSheetResult> ReDrawRarityParameterStatusByStampSheetAsync(
                Request.ReDrawRarityParameterStatusByStampSheetRequest request
        )
		{
            AsyncResult<Result.ReDrawRarityParameterStatusByStampSheetResult> result = null;
			await ReDrawRarityParameterStatusByStampSheet(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
    #else
		public ReDrawRarityParameterStatusByStampSheetTask ReDrawRarityParameterStatusByStampSheetAsync(
                Request.ReDrawRarityParameterStatusByStampSheetRequest request
        )
		{
			return new ReDrawRarityParameterStatusByStampSheetTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.ReDrawRarityParameterStatusByStampSheetResult> ReDrawRarityParameterStatusByStampSheetAsync(
                Request.ReDrawRarityParameterStatusByStampSheetRequest request
        )
		{
			var task = new ReDrawRarityParameterStatusByStampSheetTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class AddRarityParameterStatusByUserIdTask : Gs2RestSessionTask<AddRarityParameterStatusByUserIdRequest, AddRarityParameterStatusByUserIdResult>
        {
            public AddRarityParameterStatusByUserIdTask(IGs2Session session, RestSessionRequestFactory factory, AddRarityParameterStatusByUserIdRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(AddRarityParameterStatusByUserIdRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "enchant")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/{userId}/status/rarity/{parameterName}/{propertyId}/add";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{userId}", !string.IsNullOrEmpty(request.UserId) ? request.UserId.ToString() : "null");
                url = url.Replace("{parameterName}", !string.IsNullOrEmpty(request.ParameterName) ? request.ParameterName.ToString() : "null");
                url = url.Replace("{propertyId}", !string.IsNullOrEmpty(request.PropertyId) ? request.PropertyId.ToString() : "null");

                var sessionRequest = Factory.Post(url);

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
		public IEnumerator AddRarityParameterStatusByUserId(
                Request.AddRarityParameterStatusByUserIdRequest request,
                UnityAction<AsyncResult<Result.AddRarityParameterStatusByUserIdResult>> callback
        )
		{
			var task = new AddRarityParameterStatusByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.AddRarityParameterStatusByUserIdResult>(task.Result, task.Error));
        }

		public IFuture<Result.AddRarityParameterStatusByUserIdResult> AddRarityParameterStatusByUserIdFuture(
                Request.AddRarityParameterStatusByUserIdRequest request
        )
		{
			return new AddRarityParameterStatusByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.AddRarityParameterStatusByUserIdResult> AddRarityParameterStatusByUserIdAsync(
                Request.AddRarityParameterStatusByUserIdRequest request
        )
		{
            AsyncResult<Result.AddRarityParameterStatusByUserIdResult> result = null;
			await AddRarityParameterStatusByUserId(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
    #else
		public AddRarityParameterStatusByUserIdTask AddRarityParameterStatusByUserIdAsync(
                Request.AddRarityParameterStatusByUserIdRequest request
        )
		{
			return new AddRarityParameterStatusByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.AddRarityParameterStatusByUserIdResult> AddRarityParameterStatusByUserIdAsync(
                Request.AddRarityParameterStatusByUserIdRequest request
        )
		{
			var task = new AddRarityParameterStatusByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class AddRarityParameterStatusByStampSheetTask : Gs2RestSessionTask<AddRarityParameterStatusByStampSheetRequest, AddRarityParameterStatusByStampSheetResult>
        {
            public AddRarityParameterStatusByStampSheetTask(IGs2Session session, RestSessionRequestFactory factory, AddRarityParameterStatusByStampSheetRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(AddRarityParameterStatusByStampSheetRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "enchant")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/stamp/rarity/parameter/add";

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
		public IEnumerator AddRarityParameterStatusByStampSheet(
                Request.AddRarityParameterStatusByStampSheetRequest request,
                UnityAction<AsyncResult<Result.AddRarityParameterStatusByStampSheetResult>> callback
        )
		{
			var task = new AddRarityParameterStatusByStampSheetTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.AddRarityParameterStatusByStampSheetResult>(task.Result, task.Error));
        }

		public IFuture<Result.AddRarityParameterStatusByStampSheetResult> AddRarityParameterStatusByStampSheetFuture(
                Request.AddRarityParameterStatusByStampSheetRequest request
        )
		{
			return new AddRarityParameterStatusByStampSheetTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.AddRarityParameterStatusByStampSheetResult> AddRarityParameterStatusByStampSheetAsync(
                Request.AddRarityParameterStatusByStampSheetRequest request
        )
		{
            AsyncResult<Result.AddRarityParameterStatusByStampSheetResult> result = null;
			await AddRarityParameterStatusByStampSheet(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
    #else
		public AddRarityParameterStatusByStampSheetTask AddRarityParameterStatusByStampSheetAsync(
                Request.AddRarityParameterStatusByStampSheetRequest request
        )
		{
			return new AddRarityParameterStatusByStampSheetTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.AddRarityParameterStatusByStampSheetResult> AddRarityParameterStatusByStampSheetAsync(
                Request.AddRarityParameterStatusByStampSheetRequest request
        )
		{
			var task = new AddRarityParameterStatusByStampSheetTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class VerifyRarityParameterStatusTask : Gs2RestSessionTask<VerifyRarityParameterStatusRequest, VerifyRarityParameterStatusResult>
        {
            public VerifyRarityParameterStatusTask(IGs2Session session, RestSessionRequestFactory factory, VerifyRarityParameterStatusRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(VerifyRarityParameterStatusRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "enchant")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/me/status/rarity/{parameterName}/{propertyId}/verify/{verifyType}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{parameterName}", !string.IsNullOrEmpty(request.ParameterName) ? request.ParameterName.ToString() : "null");
                url = url.Replace("{propertyId}", !string.IsNullOrEmpty(request.PropertyId) ? request.PropertyId.ToString() : "null");
                url = url.Replace("{verifyType}", !string.IsNullOrEmpty(request.VerifyType) ? request.VerifyType.ToString() : "null");

                var sessionRequest = Factory.Post(url);

                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);
                jsonWriter.WriteObjectStart();
                if (request.ParameterValueName != null)
                {
                    jsonWriter.WritePropertyName("parameterValueName");
                    jsonWriter.Write(request.ParameterValueName);
                }
                if (request.ParameterCount != null)
                {
                    jsonWriter.WritePropertyName("parameterCount");
                    jsonWriter.Write(request.ParameterCount.ToString());
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
		public IEnumerator VerifyRarityParameterStatus(
                Request.VerifyRarityParameterStatusRequest request,
                UnityAction<AsyncResult<Result.VerifyRarityParameterStatusResult>> callback
        )
		{
			var task = new VerifyRarityParameterStatusTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.VerifyRarityParameterStatusResult>(task.Result, task.Error));
        }

		public IFuture<Result.VerifyRarityParameterStatusResult> VerifyRarityParameterStatusFuture(
                Request.VerifyRarityParameterStatusRequest request
        )
		{
			return new VerifyRarityParameterStatusTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.VerifyRarityParameterStatusResult> VerifyRarityParameterStatusAsync(
                Request.VerifyRarityParameterStatusRequest request
        )
		{
            AsyncResult<Result.VerifyRarityParameterStatusResult> result = null;
			await VerifyRarityParameterStatus(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
    #else
		public VerifyRarityParameterStatusTask VerifyRarityParameterStatusAsync(
                Request.VerifyRarityParameterStatusRequest request
        )
		{
			return new VerifyRarityParameterStatusTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.VerifyRarityParameterStatusResult> VerifyRarityParameterStatusAsync(
                Request.VerifyRarityParameterStatusRequest request
        )
		{
			var task = new VerifyRarityParameterStatusTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class VerifyRarityParameterStatusByUserIdTask : Gs2RestSessionTask<VerifyRarityParameterStatusByUserIdRequest, VerifyRarityParameterStatusByUserIdResult>
        {
            public VerifyRarityParameterStatusByUserIdTask(IGs2Session session, RestSessionRequestFactory factory, VerifyRarityParameterStatusByUserIdRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(VerifyRarityParameterStatusByUserIdRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "enchant")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/{userId}/status/rarity/{parameterName}/{propertyId}/verify/{verifyType}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{parameterName}", !string.IsNullOrEmpty(request.ParameterName) ? request.ParameterName.ToString() : "null");
                url = url.Replace("{userId}", !string.IsNullOrEmpty(request.UserId) ? request.UserId.ToString() : "null");
                url = url.Replace("{propertyId}", !string.IsNullOrEmpty(request.PropertyId) ? request.PropertyId.ToString() : "null");
                url = url.Replace("{verifyType}", !string.IsNullOrEmpty(request.VerifyType) ? request.VerifyType.ToString() : "null");

                var sessionRequest = Factory.Post(url);

                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);
                jsonWriter.WriteObjectStart();
                if (request.ParameterValueName != null)
                {
                    jsonWriter.WritePropertyName("parameterValueName");
                    jsonWriter.Write(request.ParameterValueName);
                }
                if (request.ParameterCount != null)
                {
                    jsonWriter.WritePropertyName("parameterCount");
                    jsonWriter.Write(request.ParameterCount.ToString());
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
		public IEnumerator VerifyRarityParameterStatusByUserId(
                Request.VerifyRarityParameterStatusByUserIdRequest request,
                UnityAction<AsyncResult<Result.VerifyRarityParameterStatusByUserIdResult>> callback
        )
		{
			var task = new VerifyRarityParameterStatusByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.VerifyRarityParameterStatusByUserIdResult>(task.Result, task.Error));
        }

		public IFuture<Result.VerifyRarityParameterStatusByUserIdResult> VerifyRarityParameterStatusByUserIdFuture(
                Request.VerifyRarityParameterStatusByUserIdRequest request
        )
		{
			return new VerifyRarityParameterStatusByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.VerifyRarityParameterStatusByUserIdResult> VerifyRarityParameterStatusByUserIdAsync(
                Request.VerifyRarityParameterStatusByUserIdRequest request
        )
		{
            AsyncResult<Result.VerifyRarityParameterStatusByUserIdResult> result = null;
			await VerifyRarityParameterStatusByUserId(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
    #else
		public VerifyRarityParameterStatusByUserIdTask VerifyRarityParameterStatusByUserIdAsync(
                Request.VerifyRarityParameterStatusByUserIdRequest request
        )
		{
			return new VerifyRarityParameterStatusByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.VerifyRarityParameterStatusByUserIdResult> VerifyRarityParameterStatusByUserIdAsync(
                Request.VerifyRarityParameterStatusByUserIdRequest request
        )
		{
			var task = new VerifyRarityParameterStatusByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class VerifyRarityParameterStatusByStampTaskTask : Gs2RestSessionTask<VerifyRarityParameterStatusByStampTaskRequest, VerifyRarityParameterStatusByStampTaskResult>
        {
            public VerifyRarityParameterStatusByStampTaskTask(IGs2Session session, RestSessionRequestFactory factory, VerifyRarityParameterStatusByStampTaskRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(VerifyRarityParameterStatusByStampTaskRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "enchant")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/stamp/rarity/parameter/verify";

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
		public IEnumerator VerifyRarityParameterStatusByStampTask(
                Request.VerifyRarityParameterStatusByStampTaskRequest request,
                UnityAction<AsyncResult<Result.VerifyRarityParameterStatusByStampTaskResult>> callback
        )
		{
			var task = new VerifyRarityParameterStatusByStampTaskTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.VerifyRarityParameterStatusByStampTaskResult>(task.Result, task.Error));
        }

		public IFuture<Result.VerifyRarityParameterStatusByStampTaskResult> VerifyRarityParameterStatusByStampTaskFuture(
                Request.VerifyRarityParameterStatusByStampTaskRequest request
        )
		{
			return new VerifyRarityParameterStatusByStampTaskTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.VerifyRarityParameterStatusByStampTaskResult> VerifyRarityParameterStatusByStampTaskAsync(
                Request.VerifyRarityParameterStatusByStampTaskRequest request
        )
		{
            AsyncResult<Result.VerifyRarityParameterStatusByStampTaskResult> result = null;
			await VerifyRarityParameterStatusByStampTask(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
    #else
		public VerifyRarityParameterStatusByStampTaskTask VerifyRarityParameterStatusByStampTaskAsync(
                Request.VerifyRarityParameterStatusByStampTaskRequest request
        )
		{
			return new VerifyRarityParameterStatusByStampTaskTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.VerifyRarityParameterStatusByStampTaskResult> VerifyRarityParameterStatusByStampTaskAsync(
                Request.VerifyRarityParameterStatusByStampTaskRequest request
        )
		{
			var task = new VerifyRarityParameterStatusByStampTaskTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class SetRarityParameterStatusByUserIdTask : Gs2RestSessionTask<SetRarityParameterStatusByUserIdRequest, SetRarityParameterStatusByUserIdResult>
        {
            public SetRarityParameterStatusByUserIdTask(IGs2Session session, RestSessionRequestFactory factory, SetRarityParameterStatusByUserIdRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(SetRarityParameterStatusByUserIdRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "enchant")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/{userId}/status/rarity/{parameterName}/{propertyId}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{userId}", !string.IsNullOrEmpty(request.UserId) ? request.UserId.ToString() : "null");
                url = url.Replace("{parameterName}", !string.IsNullOrEmpty(request.ParameterName) ? request.ParameterName.ToString() : "null");
                url = url.Replace("{propertyId}", !string.IsNullOrEmpty(request.PropertyId) ? request.PropertyId.ToString() : "null");

                var sessionRequest = Factory.Put(url);

                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);
                jsonWriter.WriteObjectStart();
                if (request.ParameterValues != null)
                {
                    jsonWriter.WritePropertyName("parameterValues");
                    jsonWriter.WriteArrayStart();
                    foreach(var item in request.ParameterValues)
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
		public IEnumerator SetRarityParameterStatusByUserId(
                Request.SetRarityParameterStatusByUserIdRequest request,
                UnityAction<AsyncResult<Result.SetRarityParameterStatusByUserIdResult>> callback
        )
		{
			var task = new SetRarityParameterStatusByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.SetRarityParameterStatusByUserIdResult>(task.Result, task.Error));
        }

		public IFuture<Result.SetRarityParameterStatusByUserIdResult> SetRarityParameterStatusByUserIdFuture(
                Request.SetRarityParameterStatusByUserIdRequest request
        )
		{
			return new SetRarityParameterStatusByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.SetRarityParameterStatusByUserIdResult> SetRarityParameterStatusByUserIdAsync(
                Request.SetRarityParameterStatusByUserIdRequest request
        )
		{
            AsyncResult<Result.SetRarityParameterStatusByUserIdResult> result = null;
			await SetRarityParameterStatusByUserId(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
    #else
		public SetRarityParameterStatusByUserIdTask SetRarityParameterStatusByUserIdAsync(
                Request.SetRarityParameterStatusByUserIdRequest request
        )
		{
			return new SetRarityParameterStatusByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.SetRarityParameterStatusByUserIdResult> SetRarityParameterStatusByUserIdAsync(
                Request.SetRarityParameterStatusByUserIdRequest request
        )
		{
			var task = new SetRarityParameterStatusByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class SetRarityParameterStatusByStampSheetTask : Gs2RestSessionTask<SetRarityParameterStatusByStampSheetRequest, SetRarityParameterStatusByStampSheetResult>
        {
            public SetRarityParameterStatusByStampSheetTask(IGs2Session session, RestSessionRequestFactory factory, SetRarityParameterStatusByStampSheetRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(SetRarityParameterStatusByStampSheetRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "enchant")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/stamp/rarity/parameter/set";

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
		public IEnumerator SetRarityParameterStatusByStampSheet(
                Request.SetRarityParameterStatusByStampSheetRequest request,
                UnityAction<AsyncResult<Result.SetRarityParameterStatusByStampSheetResult>> callback
        )
		{
			var task = new SetRarityParameterStatusByStampSheetTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.SetRarityParameterStatusByStampSheetResult>(task.Result, task.Error));
        }

		public IFuture<Result.SetRarityParameterStatusByStampSheetResult> SetRarityParameterStatusByStampSheetFuture(
                Request.SetRarityParameterStatusByStampSheetRequest request
        )
		{
			return new SetRarityParameterStatusByStampSheetTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.SetRarityParameterStatusByStampSheetResult> SetRarityParameterStatusByStampSheetAsync(
                Request.SetRarityParameterStatusByStampSheetRequest request
        )
		{
            AsyncResult<Result.SetRarityParameterStatusByStampSheetResult> result = null;
			await SetRarityParameterStatusByStampSheet(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
    #else
		public SetRarityParameterStatusByStampSheetTask SetRarityParameterStatusByStampSheetAsync(
                Request.SetRarityParameterStatusByStampSheetRequest request
        )
		{
			return new SetRarityParameterStatusByStampSheetTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.SetRarityParameterStatusByStampSheetResult> SetRarityParameterStatusByStampSheetAsync(
                Request.SetRarityParameterStatusByStampSheetRequest request
        )
		{
			var task = new SetRarityParameterStatusByStampSheetTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif
	}
}