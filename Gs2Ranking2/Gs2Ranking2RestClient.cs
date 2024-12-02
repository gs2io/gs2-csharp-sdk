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
using Gs2.Gs2Ranking2.Request;
using Gs2.Gs2Ranking2.Result;
using Gs2.Util.LitJson;

namespace Gs2.Gs2Ranking2
{
	public class Gs2Ranking2RestClient : AbstractGs2Client
	{
#if UNITY_2017_1_OR_NEWER
		private readonly CertificateHandler _certificateHandler;
#endif

		public static string Endpoint = "ranking2";

        protected Gs2RestSession Gs2RestSession => (Gs2RestSession) Gs2Session;

		public Gs2Ranking2RestClient(Gs2RestSession Gs2RestSession) : base(Gs2RestSession)
		{

		}

#if UNITY_2017_1_OR_NEWER
		public Gs2Ranking2RestClient(Gs2RestSession gs2RestSession, CertificateHandler certificateHandler) : base(gs2RestSession)
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
                    .Replace("{service}", "ranking2")
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
                    .Replace("{service}", "ranking2")
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
                    .Replace("{service}", "ranking2")
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
                    .Replace("{service}", "ranking2")
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
                    .Replace("{service}", "ranking2")
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
                    .Replace("{service}", "ranking2")
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
                    .Replace("{service}", "ranking2")
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
                    .Replace("{service}", "ranking2")
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
                    .Replace("{service}", "ranking2")
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
                    .Replace("{service}", "ranking2")
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
                    .Replace("{service}", "ranking2")
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
                    .Replace("{service}", "ranking2")
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
                    .Replace("{service}", "ranking2")
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


        public class DescribeGlobalRankingModelsTask : Gs2RestSessionTask<DescribeGlobalRankingModelsRequest, DescribeGlobalRankingModelsResult>
        {
            public DescribeGlobalRankingModelsTask(IGs2Session session, RestSessionRequestFactory factory, DescribeGlobalRankingModelsRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DescribeGlobalRankingModelsRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "ranking2")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/model/global";

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
		public IEnumerator DescribeGlobalRankingModels(
                Request.DescribeGlobalRankingModelsRequest request,
                UnityAction<AsyncResult<Result.DescribeGlobalRankingModelsResult>> callback
        )
		{
			var task = new DescribeGlobalRankingModelsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.DescribeGlobalRankingModelsResult>(task.Result, task.Error));
        }

		public IFuture<Result.DescribeGlobalRankingModelsResult> DescribeGlobalRankingModelsFuture(
                Request.DescribeGlobalRankingModelsRequest request
        )
		{
			return new DescribeGlobalRankingModelsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.DescribeGlobalRankingModelsResult> DescribeGlobalRankingModelsAsync(
                Request.DescribeGlobalRankingModelsRequest request
        )
		{
            AsyncResult<Result.DescribeGlobalRankingModelsResult> result = null;
			await DescribeGlobalRankingModels(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
    #else
		public DescribeGlobalRankingModelsTask DescribeGlobalRankingModelsAsync(
                Request.DescribeGlobalRankingModelsRequest request
        )
		{
			return new DescribeGlobalRankingModelsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.DescribeGlobalRankingModelsResult> DescribeGlobalRankingModelsAsync(
                Request.DescribeGlobalRankingModelsRequest request
        )
		{
			var task = new DescribeGlobalRankingModelsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class GetGlobalRankingModelTask : Gs2RestSessionTask<GetGlobalRankingModelRequest, GetGlobalRankingModelResult>
        {
            public GetGlobalRankingModelTask(IGs2Session session, RestSessionRequestFactory factory, GetGlobalRankingModelRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(GetGlobalRankingModelRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "ranking2")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/model/global/{rankingName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{rankingName}", !string.IsNullOrEmpty(request.RankingName) ? request.RankingName.ToString() : "null");

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
		public IEnumerator GetGlobalRankingModel(
                Request.GetGlobalRankingModelRequest request,
                UnityAction<AsyncResult<Result.GetGlobalRankingModelResult>> callback
        )
		{
			var task = new GetGlobalRankingModelTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.GetGlobalRankingModelResult>(task.Result, task.Error));
        }

		public IFuture<Result.GetGlobalRankingModelResult> GetGlobalRankingModelFuture(
                Request.GetGlobalRankingModelRequest request
        )
		{
			return new GetGlobalRankingModelTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.GetGlobalRankingModelResult> GetGlobalRankingModelAsync(
                Request.GetGlobalRankingModelRequest request
        )
		{
            AsyncResult<Result.GetGlobalRankingModelResult> result = null;
			await GetGlobalRankingModel(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
    #else
		public GetGlobalRankingModelTask GetGlobalRankingModelAsync(
                Request.GetGlobalRankingModelRequest request
        )
		{
			return new GetGlobalRankingModelTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.GetGlobalRankingModelResult> GetGlobalRankingModelAsync(
                Request.GetGlobalRankingModelRequest request
        )
		{
			var task = new GetGlobalRankingModelTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class DescribeGlobalRankingModelMastersTask : Gs2RestSessionTask<DescribeGlobalRankingModelMastersRequest, DescribeGlobalRankingModelMastersResult>
        {
            public DescribeGlobalRankingModelMastersTask(IGs2Session session, RestSessionRequestFactory factory, DescribeGlobalRankingModelMastersRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DescribeGlobalRankingModelMastersRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "ranking2")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/master/global";

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
		public IEnumerator DescribeGlobalRankingModelMasters(
                Request.DescribeGlobalRankingModelMastersRequest request,
                UnityAction<AsyncResult<Result.DescribeGlobalRankingModelMastersResult>> callback
        )
		{
			var task = new DescribeGlobalRankingModelMastersTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.DescribeGlobalRankingModelMastersResult>(task.Result, task.Error));
        }

		public IFuture<Result.DescribeGlobalRankingModelMastersResult> DescribeGlobalRankingModelMastersFuture(
                Request.DescribeGlobalRankingModelMastersRequest request
        )
		{
			return new DescribeGlobalRankingModelMastersTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.DescribeGlobalRankingModelMastersResult> DescribeGlobalRankingModelMastersAsync(
                Request.DescribeGlobalRankingModelMastersRequest request
        )
		{
            AsyncResult<Result.DescribeGlobalRankingModelMastersResult> result = null;
			await DescribeGlobalRankingModelMasters(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
    #else
		public DescribeGlobalRankingModelMastersTask DescribeGlobalRankingModelMastersAsync(
                Request.DescribeGlobalRankingModelMastersRequest request
        )
		{
			return new DescribeGlobalRankingModelMastersTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.DescribeGlobalRankingModelMastersResult> DescribeGlobalRankingModelMastersAsync(
                Request.DescribeGlobalRankingModelMastersRequest request
        )
		{
			var task = new DescribeGlobalRankingModelMastersTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class CreateGlobalRankingModelMasterTask : Gs2RestSessionTask<CreateGlobalRankingModelMasterRequest, CreateGlobalRankingModelMasterResult>
        {
            public CreateGlobalRankingModelMasterTask(IGs2Session session, RestSessionRequestFactory factory, CreateGlobalRankingModelMasterRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(CreateGlobalRankingModelMasterRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "ranking2")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/master/global";

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
                if (request.MinimumValue != null)
                {
                    jsonWriter.WritePropertyName("minimumValue");
                    jsonWriter.Write(request.MinimumValue.ToString());
                }
                if (request.MaximumValue != null)
                {
                    jsonWriter.WritePropertyName("maximumValue");
                    jsonWriter.Write(request.MaximumValue.ToString());
                }
                if (request.Sum != null)
                {
                    jsonWriter.WritePropertyName("sum");
                    jsonWriter.Write(request.Sum.ToString());
                }
                if (request.OrderDirection != null)
                {
                    jsonWriter.WritePropertyName("orderDirection");
                    jsonWriter.Write(request.OrderDirection);
                }
                if (request.RankingRewards != null)
                {
                    jsonWriter.WritePropertyName("rankingRewards");
                    jsonWriter.WriteArrayStart();
                    foreach(var item in request.RankingRewards)
                    {
                        item.WriteJson(jsonWriter);
                    }
                    jsonWriter.WriteArrayEnd();
                }
                if (request.EntryPeriodEventId != null)
                {
                    jsonWriter.WritePropertyName("entryPeriodEventId");
                    jsonWriter.Write(request.EntryPeriodEventId);
                }
                if (request.AccessPeriodEventId != null)
                {
                    jsonWriter.WritePropertyName("accessPeriodEventId");
                    jsonWriter.Write(request.AccessPeriodEventId);
                }
                if (request.ContextStack != null)
                {
                    jsonWriter.WritePropertyName("contextStack");
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
		public IEnumerator CreateGlobalRankingModelMaster(
                Request.CreateGlobalRankingModelMasterRequest request,
                UnityAction<AsyncResult<Result.CreateGlobalRankingModelMasterResult>> callback
        )
		{
			var task = new CreateGlobalRankingModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.CreateGlobalRankingModelMasterResult>(task.Result, task.Error));
        }

		public IFuture<Result.CreateGlobalRankingModelMasterResult> CreateGlobalRankingModelMasterFuture(
                Request.CreateGlobalRankingModelMasterRequest request
        )
		{
			return new CreateGlobalRankingModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.CreateGlobalRankingModelMasterResult> CreateGlobalRankingModelMasterAsync(
                Request.CreateGlobalRankingModelMasterRequest request
        )
		{
            AsyncResult<Result.CreateGlobalRankingModelMasterResult> result = null;
			await CreateGlobalRankingModelMaster(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
    #else
		public CreateGlobalRankingModelMasterTask CreateGlobalRankingModelMasterAsync(
                Request.CreateGlobalRankingModelMasterRequest request
        )
		{
			return new CreateGlobalRankingModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.CreateGlobalRankingModelMasterResult> CreateGlobalRankingModelMasterAsync(
                Request.CreateGlobalRankingModelMasterRequest request
        )
		{
			var task = new CreateGlobalRankingModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class GetGlobalRankingModelMasterTask : Gs2RestSessionTask<GetGlobalRankingModelMasterRequest, GetGlobalRankingModelMasterResult>
        {
            public GetGlobalRankingModelMasterTask(IGs2Session session, RestSessionRequestFactory factory, GetGlobalRankingModelMasterRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(GetGlobalRankingModelMasterRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "ranking2")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/master/global/{rankingName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{rankingName}", !string.IsNullOrEmpty(request.RankingName) ? request.RankingName.ToString() : "null");

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
		public IEnumerator GetGlobalRankingModelMaster(
                Request.GetGlobalRankingModelMasterRequest request,
                UnityAction<AsyncResult<Result.GetGlobalRankingModelMasterResult>> callback
        )
		{
			var task = new GetGlobalRankingModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.GetGlobalRankingModelMasterResult>(task.Result, task.Error));
        }

		public IFuture<Result.GetGlobalRankingModelMasterResult> GetGlobalRankingModelMasterFuture(
                Request.GetGlobalRankingModelMasterRequest request
        )
		{
			return new GetGlobalRankingModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.GetGlobalRankingModelMasterResult> GetGlobalRankingModelMasterAsync(
                Request.GetGlobalRankingModelMasterRequest request
        )
		{
            AsyncResult<Result.GetGlobalRankingModelMasterResult> result = null;
			await GetGlobalRankingModelMaster(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
    #else
		public GetGlobalRankingModelMasterTask GetGlobalRankingModelMasterAsync(
                Request.GetGlobalRankingModelMasterRequest request
        )
		{
			return new GetGlobalRankingModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.GetGlobalRankingModelMasterResult> GetGlobalRankingModelMasterAsync(
                Request.GetGlobalRankingModelMasterRequest request
        )
		{
			var task = new GetGlobalRankingModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class UpdateGlobalRankingModelMasterTask : Gs2RestSessionTask<UpdateGlobalRankingModelMasterRequest, UpdateGlobalRankingModelMasterResult>
        {
            public UpdateGlobalRankingModelMasterTask(IGs2Session session, RestSessionRequestFactory factory, UpdateGlobalRankingModelMasterRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(UpdateGlobalRankingModelMasterRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "ranking2")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/master/global/{rankingName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{rankingName}", !string.IsNullOrEmpty(request.RankingName) ? request.RankingName.ToString() : "null");

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
                if (request.MinimumValue != null)
                {
                    jsonWriter.WritePropertyName("minimumValue");
                    jsonWriter.Write(request.MinimumValue.ToString());
                }
                if (request.MaximumValue != null)
                {
                    jsonWriter.WritePropertyName("maximumValue");
                    jsonWriter.Write(request.MaximumValue.ToString());
                }
                if (request.Sum != null)
                {
                    jsonWriter.WritePropertyName("sum");
                    jsonWriter.Write(request.Sum.ToString());
                }
                if (request.OrderDirection != null)
                {
                    jsonWriter.WritePropertyName("orderDirection");
                    jsonWriter.Write(request.OrderDirection);
                }
                if (request.RankingRewards != null)
                {
                    jsonWriter.WritePropertyName("rankingRewards");
                    jsonWriter.WriteArrayStart();
                    foreach(var item in request.RankingRewards)
                    {
                        item.WriteJson(jsonWriter);
                    }
                    jsonWriter.WriteArrayEnd();
                }
                if (request.EntryPeriodEventId != null)
                {
                    jsonWriter.WritePropertyName("entryPeriodEventId");
                    jsonWriter.Write(request.EntryPeriodEventId);
                }
                if (request.AccessPeriodEventId != null)
                {
                    jsonWriter.WritePropertyName("accessPeriodEventId");
                    jsonWriter.Write(request.AccessPeriodEventId);
                }
                if (request.ContextStack != null)
                {
                    jsonWriter.WritePropertyName("contextStack");
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
		public IEnumerator UpdateGlobalRankingModelMaster(
                Request.UpdateGlobalRankingModelMasterRequest request,
                UnityAction<AsyncResult<Result.UpdateGlobalRankingModelMasterResult>> callback
        )
		{
			var task = new UpdateGlobalRankingModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.UpdateGlobalRankingModelMasterResult>(task.Result, task.Error));
        }

		public IFuture<Result.UpdateGlobalRankingModelMasterResult> UpdateGlobalRankingModelMasterFuture(
                Request.UpdateGlobalRankingModelMasterRequest request
        )
		{
			return new UpdateGlobalRankingModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.UpdateGlobalRankingModelMasterResult> UpdateGlobalRankingModelMasterAsync(
                Request.UpdateGlobalRankingModelMasterRequest request
        )
		{
            AsyncResult<Result.UpdateGlobalRankingModelMasterResult> result = null;
			await UpdateGlobalRankingModelMaster(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
    #else
		public UpdateGlobalRankingModelMasterTask UpdateGlobalRankingModelMasterAsync(
                Request.UpdateGlobalRankingModelMasterRequest request
        )
		{
			return new UpdateGlobalRankingModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.UpdateGlobalRankingModelMasterResult> UpdateGlobalRankingModelMasterAsync(
                Request.UpdateGlobalRankingModelMasterRequest request
        )
		{
			var task = new UpdateGlobalRankingModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class DeleteGlobalRankingModelMasterTask : Gs2RestSessionTask<DeleteGlobalRankingModelMasterRequest, DeleteGlobalRankingModelMasterResult>
        {
            public DeleteGlobalRankingModelMasterTask(IGs2Session session, RestSessionRequestFactory factory, DeleteGlobalRankingModelMasterRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DeleteGlobalRankingModelMasterRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "ranking2")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/master/global/{rankingName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{rankingName}", !string.IsNullOrEmpty(request.RankingName) ? request.RankingName.ToString() : "null");

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
		public IEnumerator DeleteGlobalRankingModelMaster(
                Request.DeleteGlobalRankingModelMasterRequest request,
                UnityAction<AsyncResult<Result.DeleteGlobalRankingModelMasterResult>> callback
        )
		{
			var task = new DeleteGlobalRankingModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.DeleteGlobalRankingModelMasterResult>(task.Result, task.Error));
        }

		public IFuture<Result.DeleteGlobalRankingModelMasterResult> DeleteGlobalRankingModelMasterFuture(
                Request.DeleteGlobalRankingModelMasterRequest request
        )
		{
			return new DeleteGlobalRankingModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.DeleteGlobalRankingModelMasterResult> DeleteGlobalRankingModelMasterAsync(
                Request.DeleteGlobalRankingModelMasterRequest request
        )
		{
            AsyncResult<Result.DeleteGlobalRankingModelMasterResult> result = null;
			await DeleteGlobalRankingModelMaster(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
    #else
		public DeleteGlobalRankingModelMasterTask DeleteGlobalRankingModelMasterAsync(
                Request.DeleteGlobalRankingModelMasterRequest request
        )
		{
			return new DeleteGlobalRankingModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.DeleteGlobalRankingModelMasterResult> DeleteGlobalRankingModelMasterAsync(
                Request.DeleteGlobalRankingModelMasterRequest request
        )
		{
			var task = new DeleteGlobalRankingModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class DescribeGlobalRankingScoresTask : Gs2RestSessionTask<DescribeGlobalRankingScoresRequest, DescribeGlobalRankingScoresResult>
        {
            public DescribeGlobalRankingScoresTask(IGs2Session session, RestSessionRequestFactory factory, DescribeGlobalRankingScoresRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DescribeGlobalRankingScoresRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "ranking2")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/me/score/global";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");

                var sessionRequest = Factory.Get(url);
                if (request.ContextStack != null)
                {
                    sessionRequest.AddQueryString("contextStack", request.ContextStack);
                }
                if (request.RankingName != null) {
                    sessionRequest.AddQueryString("rankingName", $"{request.RankingName}");
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
		public IEnumerator DescribeGlobalRankingScores(
                Request.DescribeGlobalRankingScoresRequest request,
                UnityAction<AsyncResult<Result.DescribeGlobalRankingScoresResult>> callback
        )
		{
			var task = new DescribeGlobalRankingScoresTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.DescribeGlobalRankingScoresResult>(task.Result, task.Error));
        }

		public IFuture<Result.DescribeGlobalRankingScoresResult> DescribeGlobalRankingScoresFuture(
                Request.DescribeGlobalRankingScoresRequest request
        )
		{
			return new DescribeGlobalRankingScoresTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.DescribeGlobalRankingScoresResult> DescribeGlobalRankingScoresAsync(
                Request.DescribeGlobalRankingScoresRequest request
        )
		{
            AsyncResult<Result.DescribeGlobalRankingScoresResult> result = null;
			await DescribeGlobalRankingScores(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
    #else
		public DescribeGlobalRankingScoresTask DescribeGlobalRankingScoresAsync(
                Request.DescribeGlobalRankingScoresRequest request
        )
		{
			return new DescribeGlobalRankingScoresTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.DescribeGlobalRankingScoresResult> DescribeGlobalRankingScoresAsync(
                Request.DescribeGlobalRankingScoresRequest request
        )
		{
			var task = new DescribeGlobalRankingScoresTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class DescribeGlobalRankingScoresByUserIdTask : Gs2RestSessionTask<DescribeGlobalRankingScoresByUserIdRequest, DescribeGlobalRankingScoresByUserIdResult>
        {
            public DescribeGlobalRankingScoresByUserIdTask(IGs2Session session, RestSessionRequestFactory factory, DescribeGlobalRankingScoresByUserIdRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DescribeGlobalRankingScoresByUserIdRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "ranking2")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/{userId}/score/global";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{userId}", !string.IsNullOrEmpty(request.UserId) ? request.UserId.ToString() : "null");

                var sessionRequest = Factory.Get(url);
                if (request.ContextStack != null)
                {
                    sessionRequest.AddQueryString("contextStack", request.ContextStack);
                }
                if (request.RankingName != null) {
                    sessionRequest.AddQueryString("rankingName", $"{request.RankingName}");
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
		public IEnumerator DescribeGlobalRankingScoresByUserId(
                Request.DescribeGlobalRankingScoresByUserIdRequest request,
                UnityAction<AsyncResult<Result.DescribeGlobalRankingScoresByUserIdResult>> callback
        )
		{
			var task = new DescribeGlobalRankingScoresByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.DescribeGlobalRankingScoresByUserIdResult>(task.Result, task.Error));
        }

		public IFuture<Result.DescribeGlobalRankingScoresByUserIdResult> DescribeGlobalRankingScoresByUserIdFuture(
                Request.DescribeGlobalRankingScoresByUserIdRequest request
        )
		{
			return new DescribeGlobalRankingScoresByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.DescribeGlobalRankingScoresByUserIdResult> DescribeGlobalRankingScoresByUserIdAsync(
                Request.DescribeGlobalRankingScoresByUserIdRequest request
        )
		{
            AsyncResult<Result.DescribeGlobalRankingScoresByUserIdResult> result = null;
			await DescribeGlobalRankingScoresByUserId(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
    #else
		public DescribeGlobalRankingScoresByUserIdTask DescribeGlobalRankingScoresByUserIdAsync(
                Request.DescribeGlobalRankingScoresByUserIdRequest request
        )
		{
			return new DescribeGlobalRankingScoresByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.DescribeGlobalRankingScoresByUserIdResult> DescribeGlobalRankingScoresByUserIdAsync(
                Request.DescribeGlobalRankingScoresByUserIdRequest request
        )
		{
			var task = new DescribeGlobalRankingScoresByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class PutGlobalRankingScoreTask : Gs2RestSessionTask<PutGlobalRankingScoreRequest, PutGlobalRankingScoreResult>
        {
            public PutGlobalRankingScoreTask(IGs2Session session, RestSessionRequestFactory factory, PutGlobalRankingScoreRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(PutGlobalRankingScoreRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "ranking2")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/me/score/global/{rankingName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{rankingName}", !string.IsNullOrEmpty(request.RankingName) ? request.RankingName.ToString() : "null");

                var sessionRequest = Factory.Put(url);

                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);
                jsonWriter.WriteObjectStart();
                if (request.Score != null)
                {
                    jsonWriter.WritePropertyName("score");
                    jsonWriter.Write(request.Score.ToString());
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
		public IEnumerator PutGlobalRankingScore(
                Request.PutGlobalRankingScoreRequest request,
                UnityAction<AsyncResult<Result.PutGlobalRankingScoreResult>> callback
        )
		{
			var task = new PutGlobalRankingScoreTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.PutGlobalRankingScoreResult>(task.Result, task.Error));
        }

		public IFuture<Result.PutGlobalRankingScoreResult> PutGlobalRankingScoreFuture(
                Request.PutGlobalRankingScoreRequest request
        )
		{
			return new PutGlobalRankingScoreTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.PutGlobalRankingScoreResult> PutGlobalRankingScoreAsync(
                Request.PutGlobalRankingScoreRequest request
        )
		{
            AsyncResult<Result.PutGlobalRankingScoreResult> result = null;
			await PutGlobalRankingScore(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
    #else
		public PutGlobalRankingScoreTask PutGlobalRankingScoreAsync(
                Request.PutGlobalRankingScoreRequest request
        )
		{
			return new PutGlobalRankingScoreTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.PutGlobalRankingScoreResult> PutGlobalRankingScoreAsync(
                Request.PutGlobalRankingScoreRequest request
        )
		{
			var task = new PutGlobalRankingScoreTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class PutGlobalRankingScoreByUserIdTask : Gs2RestSessionTask<PutGlobalRankingScoreByUserIdRequest, PutGlobalRankingScoreByUserIdResult>
        {
            public PutGlobalRankingScoreByUserIdTask(IGs2Session session, RestSessionRequestFactory factory, PutGlobalRankingScoreByUserIdRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(PutGlobalRankingScoreByUserIdRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "ranking2")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/{userId}/score/global/{rankingName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{rankingName}", !string.IsNullOrEmpty(request.RankingName) ? request.RankingName.ToString() : "null");
                url = url.Replace("{userId}", !string.IsNullOrEmpty(request.UserId) ? request.UserId.ToString() : "null");

                var sessionRequest = Factory.Put(url);

                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);
                jsonWriter.WriteObjectStart();
                if (request.Score != null)
                {
                    jsonWriter.WritePropertyName("score");
                    jsonWriter.Write(request.Score.ToString());
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
		public IEnumerator PutGlobalRankingScoreByUserId(
                Request.PutGlobalRankingScoreByUserIdRequest request,
                UnityAction<AsyncResult<Result.PutGlobalRankingScoreByUserIdResult>> callback
        )
		{
			var task = new PutGlobalRankingScoreByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.PutGlobalRankingScoreByUserIdResult>(task.Result, task.Error));
        }

		public IFuture<Result.PutGlobalRankingScoreByUserIdResult> PutGlobalRankingScoreByUserIdFuture(
                Request.PutGlobalRankingScoreByUserIdRequest request
        )
		{
			return new PutGlobalRankingScoreByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.PutGlobalRankingScoreByUserIdResult> PutGlobalRankingScoreByUserIdAsync(
                Request.PutGlobalRankingScoreByUserIdRequest request
        )
		{
            AsyncResult<Result.PutGlobalRankingScoreByUserIdResult> result = null;
			await PutGlobalRankingScoreByUserId(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
    #else
		public PutGlobalRankingScoreByUserIdTask PutGlobalRankingScoreByUserIdAsync(
                Request.PutGlobalRankingScoreByUserIdRequest request
        )
		{
			return new PutGlobalRankingScoreByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.PutGlobalRankingScoreByUserIdResult> PutGlobalRankingScoreByUserIdAsync(
                Request.PutGlobalRankingScoreByUserIdRequest request
        )
		{
			var task = new PutGlobalRankingScoreByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class GetGlobalRankingScoreTask : Gs2RestSessionTask<GetGlobalRankingScoreRequest, GetGlobalRankingScoreResult>
        {
            public GetGlobalRankingScoreTask(IGs2Session session, RestSessionRequestFactory factory, GetGlobalRankingScoreRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(GetGlobalRankingScoreRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "ranking2")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/me/score/global/{rankingName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{rankingName}", !string.IsNullOrEmpty(request.RankingName) ? request.RankingName.ToString() : "null");

                var sessionRequest = Factory.Get(url);
                if (request.ContextStack != null)
                {
                    sessionRequest.AddQueryString("contextStack", request.ContextStack);
                }
                if (request.Season != null) {
                    sessionRequest.AddQueryString("season", $"{request.Season}");
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
		public IEnumerator GetGlobalRankingScore(
                Request.GetGlobalRankingScoreRequest request,
                UnityAction<AsyncResult<Result.GetGlobalRankingScoreResult>> callback
        )
		{
			var task = new GetGlobalRankingScoreTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.GetGlobalRankingScoreResult>(task.Result, task.Error));
        }

		public IFuture<Result.GetGlobalRankingScoreResult> GetGlobalRankingScoreFuture(
                Request.GetGlobalRankingScoreRequest request
        )
		{
			return new GetGlobalRankingScoreTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.GetGlobalRankingScoreResult> GetGlobalRankingScoreAsync(
                Request.GetGlobalRankingScoreRequest request
        )
		{
            AsyncResult<Result.GetGlobalRankingScoreResult> result = null;
			await GetGlobalRankingScore(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
    #else
		public GetGlobalRankingScoreTask GetGlobalRankingScoreAsync(
                Request.GetGlobalRankingScoreRequest request
        )
		{
			return new GetGlobalRankingScoreTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.GetGlobalRankingScoreResult> GetGlobalRankingScoreAsync(
                Request.GetGlobalRankingScoreRequest request
        )
		{
			var task = new GetGlobalRankingScoreTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class GetGlobalRankingScoreByUserIdTask : Gs2RestSessionTask<GetGlobalRankingScoreByUserIdRequest, GetGlobalRankingScoreByUserIdResult>
        {
            public GetGlobalRankingScoreByUserIdTask(IGs2Session session, RestSessionRequestFactory factory, GetGlobalRankingScoreByUserIdRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(GetGlobalRankingScoreByUserIdRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "ranking2")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/{userId}/score/global/{rankingName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{rankingName}", !string.IsNullOrEmpty(request.RankingName) ? request.RankingName.ToString() : "null");
                url = url.Replace("{userId}", !string.IsNullOrEmpty(request.UserId) ? request.UserId.ToString() : "null");

                var sessionRequest = Factory.Get(url);
                if (request.ContextStack != null)
                {
                    sessionRequest.AddQueryString("contextStack", request.ContextStack);
                }
                if (request.Season != null) {
                    sessionRequest.AddQueryString("season", $"{request.Season}");
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
		public IEnumerator GetGlobalRankingScoreByUserId(
                Request.GetGlobalRankingScoreByUserIdRequest request,
                UnityAction<AsyncResult<Result.GetGlobalRankingScoreByUserIdResult>> callback
        )
		{
			var task = new GetGlobalRankingScoreByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.GetGlobalRankingScoreByUserIdResult>(task.Result, task.Error));
        }

		public IFuture<Result.GetGlobalRankingScoreByUserIdResult> GetGlobalRankingScoreByUserIdFuture(
                Request.GetGlobalRankingScoreByUserIdRequest request
        )
		{
			return new GetGlobalRankingScoreByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.GetGlobalRankingScoreByUserIdResult> GetGlobalRankingScoreByUserIdAsync(
                Request.GetGlobalRankingScoreByUserIdRequest request
        )
		{
            AsyncResult<Result.GetGlobalRankingScoreByUserIdResult> result = null;
			await GetGlobalRankingScoreByUserId(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
    #else
		public GetGlobalRankingScoreByUserIdTask GetGlobalRankingScoreByUserIdAsync(
                Request.GetGlobalRankingScoreByUserIdRequest request
        )
		{
			return new GetGlobalRankingScoreByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.GetGlobalRankingScoreByUserIdResult> GetGlobalRankingScoreByUserIdAsync(
                Request.GetGlobalRankingScoreByUserIdRequest request
        )
		{
			var task = new GetGlobalRankingScoreByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class DeleteGlobalRankingScoreByUserIdTask : Gs2RestSessionTask<DeleteGlobalRankingScoreByUserIdRequest, DeleteGlobalRankingScoreByUserIdResult>
        {
            public DeleteGlobalRankingScoreByUserIdTask(IGs2Session session, RestSessionRequestFactory factory, DeleteGlobalRankingScoreByUserIdRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DeleteGlobalRankingScoreByUserIdRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "ranking2")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/{userId}/score/global/{rankingName}/{season}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{rankingName}", !string.IsNullOrEmpty(request.RankingName) ? request.RankingName.ToString() : "null");
                url = url.Replace("{userId}", !string.IsNullOrEmpty(request.UserId) ? request.UserId.ToString() : "null");

                var sessionRequest = Factory.Delete(url);
                if (request.ContextStack != null)
                {
                    sessionRequest.AddQueryString("contextStack", request.ContextStack);
                }
                if (request.Season != null) {
                    sessionRequest.AddQueryString("season", $"{request.Season}");
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
		public IEnumerator DeleteGlobalRankingScoreByUserId(
                Request.DeleteGlobalRankingScoreByUserIdRequest request,
                UnityAction<AsyncResult<Result.DeleteGlobalRankingScoreByUserIdResult>> callback
        )
		{
			var task = new DeleteGlobalRankingScoreByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.DeleteGlobalRankingScoreByUserIdResult>(task.Result, task.Error));
        }

		public IFuture<Result.DeleteGlobalRankingScoreByUserIdResult> DeleteGlobalRankingScoreByUserIdFuture(
                Request.DeleteGlobalRankingScoreByUserIdRequest request
        )
		{
			return new DeleteGlobalRankingScoreByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.DeleteGlobalRankingScoreByUserIdResult> DeleteGlobalRankingScoreByUserIdAsync(
                Request.DeleteGlobalRankingScoreByUserIdRequest request
        )
		{
            AsyncResult<Result.DeleteGlobalRankingScoreByUserIdResult> result = null;
			await DeleteGlobalRankingScoreByUserId(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
    #else
		public DeleteGlobalRankingScoreByUserIdTask DeleteGlobalRankingScoreByUserIdAsync(
                Request.DeleteGlobalRankingScoreByUserIdRequest request
        )
		{
			return new DeleteGlobalRankingScoreByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.DeleteGlobalRankingScoreByUserIdResult> DeleteGlobalRankingScoreByUserIdAsync(
                Request.DeleteGlobalRankingScoreByUserIdRequest request
        )
		{
			var task = new DeleteGlobalRankingScoreByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class VerifyGlobalRankingScoreTask : Gs2RestSessionTask<VerifyGlobalRankingScoreRequest, VerifyGlobalRankingScoreResult>
        {
            public VerifyGlobalRankingScoreTask(IGs2Session session, RestSessionRequestFactory factory, VerifyGlobalRankingScoreRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(VerifyGlobalRankingScoreRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "ranking2")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/{userId}/score/global/{rankingName}/verify/{verifyType}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{rankingName}", !string.IsNullOrEmpty(request.RankingName) ? request.RankingName.ToString() : "null");
                url = url.Replace("{verifyType}", !string.IsNullOrEmpty(request.VerifyType) ? request.VerifyType.ToString() : "null");

                var sessionRequest = Factory.Post(url);

                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);
                jsonWriter.WriteObjectStart();
                if (request.Season != null)
                {
                    jsonWriter.WritePropertyName("season");
                    jsonWriter.Write(request.Season.ToString());
                }
                if (request.Score != null)
                {
                    jsonWriter.WritePropertyName("score");
                    jsonWriter.Write(request.Score.ToString());
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
		public IEnumerator VerifyGlobalRankingScore(
                Request.VerifyGlobalRankingScoreRequest request,
                UnityAction<AsyncResult<Result.VerifyGlobalRankingScoreResult>> callback
        )
		{
			var task = new VerifyGlobalRankingScoreTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.VerifyGlobalRankingScoreResult>(task.Result, task.Error));
        }

		public IFuture<Result.VerifyGlobalRankingScoreResult> VerifyGlobalRankingScoreFuture(
                Request.VerifyGlobalRankingScoreRequest request
        )
		{
			return new VerifyGlobalRankingScoreTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.VerifyGlobalRankingScoreResult> VerifyGlobalRankingScoreAsync(
                Request.VerifyGlobalRankingScoreRequest request
        )
		{
            AsyncResult<Result.VerifyGlobalRankingScoreResult> result = null;
			await VerifyGlobalRankingScore(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
    #else
		public VerifyGlobalRankingScoreTask VerifyGlobalRankingScoreAsync(
                Request.VerifyGlobalRankingScoreRequest request
        )
		{
			return new VerifyGlobalRankingScoreTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.VerifyGlobalRankingScoreResult> VerifyGlobalRankingScoreAsync(
                Request.VerifyGlobalRankingScoreRequest request
        )
		{
			var task = new VerifyGlobalRankingScoreTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class VerifyGlobalRankingScoreByUserIdTask : Gs2RestSessionTask<VerifyGlobalRankingScoreByUserIdRequest, VerifyGlobalRankingScoreByUserIdResult>
        {
            public VerifyGlobalRankingScoreByUserIdTask(IGs2Session session, RestSessionRequestFactory factory, VerifyGlobalRankingScoreByUserIdRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(VerifyGlobalRankingScoreByUserIdRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "ranking2")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/{userId}/score/global/{rankingName}/verify/{verifyType}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{userId}", !string.IsNullOrEmpty(request.UserId) ? request.UserId.ToString() : "null");
                url = url.Replace("{rankingName}", !string.IsNullOrEmpty(request.RankingName) ? request.RankingName.ToString() : "null");
                url = url.Replace("{verifyType}", !string.IsNullOrEmpty(request.VerifyType) ? request.VerifyType.ToString() : "null");

                var sessionRequest = Factory.Post(url);

                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);
                jsonWriter.WriteObjectStart();
                if (request.Season != null)
                {
                    jsonWriter.WritePropertyName("season");
                    jsonWriter.Write(request.Season.ToString());
                }
                if (request.Score != null)
                {
                    jsonWriter.WritePropertyName("score");
                    jsonWriter.Write(request.Score.ToString());
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
		public IEnumerator VerifyGlobalRankingScoreByUserId(
                Request.VerifyGlobalRankingScoreByUserIdRequest request,
                UnityAction<AsyncResult<Result.VerifyGlobalRankingScoreByUserIdResult>> callback
        )
		{
			var task = new VerifyGlobalRankingScoreByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.VerifyGlobalRankingScoreByUserIdResult>(task.Result, task.Error));
        }

		public IFuture<Result.VerifyGlobalRankingScoreByUserIdResult> VerifyGlobalRankingScoreByUserIdFuture(
                Request.VerifyGlobalRankingScoreByUserIdRequest request
        )
		{
			return new VerifyGlobalRankingScoreByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.VerifyGlobalRankingScoreByUserIdResult> VerifyGlobalRankingScoreByUserIdAsync(
                Request.VerifyGlobalRankingScoreByUserIdRequest request
        )
		{
            AsyncResult<Result.VerifyGlobalRankingScoreByUserIdResult> result = null;
			await VerifyGlobalRankingScoreByUserId(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
    #else
		public VerifyGlobalRankingScoreByUserIdTask VerifyGlobalRankingScoreByUserIdAsync(
                Request.VerifyGlobalRankingScoreByUserIdRequest request
        )
		{
			return new VerifyGlobalRankingScoreByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.VerifyGlobalRankingScoreByUserIdResult> VerifyGlobalRankingScoreByUserIdAsync(
                Request.VerifyGlobalRankingScoreByUserIdRequest request
        )
		{
			var task = new VerifyGlobalRankingScoreByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class VerifyGlobalRankingScoreByStampTaskTask : Gs2RestSessionTask<VerifyGlobalRankingScoreByStampTaskRequest, VerifyGlobalRankingScoreByStampTaskResult>
        {
            public VerifyGlobalRankingScoreByStampTaskTask(IGs2Session session, RestSessionRequestFactory factory, VerifyGlobalRankingScoreByStampTaskRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(VerifyGlobalRankingScoreByStampTaskRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "ranking2")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/stamp/global/score/verify";

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
		public IEnumerator VerifyGlobalRankingScoreByStampTask(
                Request.VerifyGlobalRankingScoreByStampTaskRequest request,
                UnityAction<AsyncResult<Result.VerifyGlobalRankingScoreByStampTaskResult>> callback
        )
		{
			var task = new VerifyGlobalRankingScoreByStampTaskTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.VerifyGlobalRankingScoreByStampTaskResult>(task.Result, task.Error));
        }

		public IFuture<Result.VerifyGlobalRankingScoreByStampTaskResult> VerifyGlobalRankingScoreByStampTaskFuture(
                Request.VerifyGlobalRankingScoreByStampTaskRequest request
        )
		{
			return new VerifyGlobalRankingScoreByStampTaskTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.VerifyGlobalRankingScoreByStampTaskResult> VerifyGlobalRankingScoreByStampTaskAsync(
                Request.VerifyGlobalRankingScoreByStampTaskRequest request
        )
		{
            AsyncResult<Result.VerifyGlobalRankingScoreByStampTaskResult> result = null;
			await VerifyGlobalRankingScoreByStampTask(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
    #else
		public VerifyGlobalRankingScoreByStampTaskTask VerifyGlobalRankingScoreByStampTaskAsync(
                Request.VerifyGlobalRankingScoreByStampTaskRequest request
        )
		{
			return new VerifyGlobalRankingScoreByStampTaskTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.VerifyGlobalRankingScoreByStampTaskResult> VerifyGlobalRankingScoreByStampTaskAsync(
                Request.VerifyGlobalRankingScoreByStampTaskRequest request
        )
		{
			var task = new VerifyGlobalRankingScoreByStampTaskTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class DescribeGlobalRankingReceivedRewardsTask : Gs2RestSessionTask<DescribeGlobalRankingReceivedRewardsRequest, DescribeGlobalRankingReceivedRewardsResult>
        {
            public DescribeGlobalRankingReceivedRewardsTask(IGs2Session session, RestSessionRequestFactory factory, DescribeGlobalRankingReceivedRewardsRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DescribeGlobalRankingReceivedRewardsRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "ranking2")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/me/global/reward/received";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");

                var sessionRequest = Factory.Get(url);
                if (request.ContextStack != null)
                {
                    sessionRequest.AddQueryString("contextStack", request.ContextStack);
                }
                if (request.RankingName != null) {
                    sessionRequest.AddQueryString("rankingName", $"{request.RankingName}");
                }
                if (request.Season != null) {
                    sessionRequest.AddQueryString("season", $"{request.Season}");
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
		public IEnumerator DescribeGlobalRankingReceivedRewards(
                Request.DescribeGlobalRankingReceivedRewardsRequest request,
                UnityAction<AsyncResult<Result.DescribeGlobalRankingReceivedRewardsResult>> callback
        )
		{
			var task = new DescribeGlobalRankingReceivedRewardsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.DescribeGlobalRankingReceivedRewardsResult>(task.Result, task.Error));
        }

		public IFuture<Result.DescribeGlobalRankingReceivedRewardsResult> DescribeGlobalRankingReceivedRewardsFuture(
                Request.DescribeGlobalRankingReceivedRewardsRequest request
        )
		{
			return new DescribeGlobalRankingReceivedRewardsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.DescribeGlobalRankingReceivedRewardsResult> DescribeGlobalRankingReceivedRewardsAsync(
                Request.DescribeGlobalRankingReceivedRewardsRequest request
        )
		{
            AsyncResult<Result.DescribeGlobalRankingReceivedRewardsResult> result = null;
			await DescribeGlobalRankingReceivedRewards(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
    #else
		public DescribeGlobalRankingReceivedRewardsTask DescribeGlobalRankingReceivedRewardsAsync(
                Request.DescribeGlobalRankingReceivedRewardsRequest request
        )
		{
			return new DescribeGlobalRankingReceivedRewardsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.DescribeGlobalRankingReceivedRewardsResult> DescribeGlobalRankingReceivedRewardsAsync(
                Request.DescribeGlobalRankingReceivedRewardsRequest request
        )
		{
			var task = new DescribeGlobalRankingReceivedRewardsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class DescribeGlobalRankingReceivedRewardsByUserIdTask : Gs2RestSessionTask<DescribeGlobalRankingReceivedRewardsByUserIdRequest, DescribeGlobalRankingReceivedRewardsByUserIdResult>
        {
            public DescribeGlobalRankingReceivedRewardsByUserIdTask(IGs2Session session, RestSessionRequestFactory factory, DescribeGlobalRankingReceivedRewardsByUserIdRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DescribeGlobalRankingReceivedRewardsByUserIdRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "ranking2")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/{userId}/global/reward/received";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{userId}", !string.IsNullOrEmpty(request.UserId) ? request.UserId.ToString() : "null");

                var sessionRequest = Factory.Get(url);
                if (request.ContextStack != null)
                {
                    sessionRequest.AddQueryString("contextStack", request.ContextStack);
                }
                if (request.RankingName != null) {
                    sessionRequest.AddQueryString("rankingName", $"{request.RankingName}");
                }
                if (request.Season != null) {
                    sessionRequest.AddQueryString("season", $"{request.Season}");
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
		public IEnumerator DescribeGlobalRankingReceivedRewardsByUserId(
                Request.DescribeGlobalRankingReceivedRewardsByUserIdRequest request,
                UnityAction<AsyncResult<Result.DescribeGlobalRankingReceivedRewardsByUserIdResult>> callback
        )
		{
			var task = new DescribeGlobalRankingReceivedRewardsByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.DescribeGlobalRankingReceivedRewardsByUserIdResult>(task.Result, task.Error));
        }

		public IFuture<Result.DescribeGlobalRankingReceivedRewardsByUserIdResult> DescribeGlobalRankingReceivedRewardsByUserIdFuture(
                Request.DescribeGlobalRankingReceivedRewardsByUserIdRequest request
        )
		{
			return new DescribeGlobalRankingReceivedRewardsByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.DescribeGlobalRankingReceivedRewardsByUserIdResult> DescribeGlobalRankingReceivedRewardsByUserIdAsync(
                Request.DescribeGlobalRankingReceivedRewardsByUserIdRequest request
        )
		{
            AsyncResult<Result.DescribeGlobalRankingReceivedRewardsByUserIdResult> result = null;
			await DescribeGlobalRankingReceivedRewardsByUserId(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
    #else
		public DescribeGlobalRankingReceivedRewardsByUserIdTask DescribeGlobalRankingReceivedRewardsByUserIdAsync(
                Request.DescribeGlobalRankingReceivedRewardsByUserIdRequest request
        )
		{
			return new DescribeGlobalRankingReceivedRewardsByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.DescribeGlobalRankingReceivedRewardsByUserIdResult> DescribeGlobalRankingReceivedRewardsByUserIdAsync(
                Request.DescribeGlobalRankingReceivedRewardsByUserIdRequest request
        )
		{
			var task = new DescribeGlobalRankingReceivedRewardsByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class CreateGlobalRankingReceivedRewardTask : Gs2RestSessionTask<CreateGlobalRankingReceivedRewardRequest, CreateGlobalRankingReceivedRewardResult>
        {
            public CreateGlobalRankingReceivedRewardTask(IGs2Session session, RestSessionRequestFactory factory, CreateGlobalRankingReceivedRewardRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(CreateGlobalRankingReceivedRewardRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "ranking2")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/me/global/reward/received/{rankingName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{rankingName}", !string.IsNullOrEmpty(request.RankingName) ? request.RankingName.ToString() : "null");

                var sessionRequest = Factory.Post(url);

                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);
                jsonWriter.WriteObjectStart();
                if (request.Season != null)
                {
                    jsonWriter.WritePropertyName("season");
                    jsonWriter.Write(request.Season.ToString());
                }
                if (request.ContextStack != null)
                {
                    jsonWriter.WritePropertyName("contextStack");
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
		public IEnumerator CreateGlobalRankingReceivedReward(
                Request.CreateGlobalRankingReceivedRewardRequest request,
                UnityAction<AsyncResult<Result.CreateGlobalRankingReceivedRewardResult>> callback
        )
		{
			var task = new CreateGlobalRankingReceivedRewardTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.CreateGlobalRankingReceivedRewardResult>(task.Result, task.Error));
        }

		public IFuture<Result.CreateGlobalRankingReceivedRewardResult> CreateGlobalRankingReceivedRewardFuture(
                Request.CreateGlobalRankingReceivedRewardRequest request
        )
		{
			return new CreateGlobalRankingReceivedRewardTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.CreateGlobalRankingReceivedRewardResult> CreateGlobalRankingReceivedRewardAsync(
                Request.CreateGlobalRankingReceivedRewardRequest request
        )
		{
            AsyncResult<Result.CreateGlobalRankingReceivedRewardResult> result = null;
			await CreateGlobalRankingReceivedReward(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
    #else
		public CreateGlobalRankingReceivedRewardTask CreateGlobalRankingReceivedRewardAsync(
                Request.CreateGlobalRankingReceivedRewardRequest request
        )
		{
			return new CreateGlobalRankingReceivedRewardTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.CreateGlobalRankingReceivedRewardResult> CreateGlobalRankingReceivedRewardAsync(
                Request.CreateGlobalRankingReceivedRewardRequest request
        )
		{
			var task = new CreateGlobalRankingReceivedRewardTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class CreateGlobalRankingReceivedRewardByUserIdTask : Gs2RestSessionTask<CreateGlobalRankingReceivedRewardByUserIdRequest, CreateGlobalRankingReceivedRewardByUserIdResult>
        {
            public CreateGlobalRankingReceivedRewardByUserIdTask(IGs2Session session, RestSessionRequestFactory factory, CreateGlobalRankingReceivedRewardByUserIdRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(CreateGlobalRankingReceivedRewardByUserIdRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "ranking2")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/{userId}/global/reward/received/{rankingName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{rankingName}", !string.IsNullOrEmpty(request.RankingName) ? request.RankingName.ToString() : "null");
                url = url.Replace("{userId}", !string.IsNullOrEmpty(request.UserId) ? request.UserId.ToString() : "null");

                var sessionRequest = Factory.Post(url);

                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);
                jsonWriter.WriteObjectStart();
                if (request.Season != null)
                {
                    jsonWriter.WritePropertyName("season");
                    jsonWriter.Write(request.Season.ToString());
                }
                if (request.ContextStack != null)
                {
                    jsonWriter.WritePropertyName("contextStack");
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
		public IEnumerator CreateGlobalRankingReceivedRewardByUserId(
                Request.CreateGlobalRankingReceivedRewardByUserIdRequest request,
                UnityAction<AsyncResult<Result.CreateGlobalRankingReceivedRewardByUserIdResult>> callback
        )
		{
			var task = new CreateGlobalRankingReceivedRewardByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.CreateGlobalRankingReceivedRewardByUserIdResult>(task.Result, task.Error));
        }

		public IFuture<Result.CreateGlobalRankingReceivedRewardByUserIdResult> CreateGlobalRankingReceivedRewardByUserIdFuture(
                Request.CreateGlobalRankingReceivedRewardByUserIdRequest request
        )
		{
			return new CreateGlobalRankingReceivedRewardByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.CreateGlobalRankingReceivedRewardByUserIdResult> CreateGlobalRankingReceivedRewardByUserIdAsync(
                Request.CreateGlobalRankingReceivedRewardByUserIdRequest request
        )
		{
            AsyncResult<Result.CreateGlobalRankingReceivedRewardByUserIdResult> result = null;
			await CreateGlobalRankingReceivedRewardByUserId(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
    #else
		public CreateGlobalRankingReceivedRewardByUserIdTask CreateGlobalRankingReceivedRewardByUserIdAsync(
                Request.CreateGlobalRankingReceivedRewardByUserIdRequest request
        )
		{
			return new CreateGlobalRankingReceivedRewardByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.CreateGlobalRankingReceivedRewardByUserIdResult> CreateGlobalRankingReceivedRewardByUserIdAsync(
                Request.CreateGlobalRankingReceivedRewardByUserIdRequest request
        )
		{
			var task = new CreateGlobalRankingReceivedRewardByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class ReceiveGlobalRankingReceivedRewardTask : Gs2RestSessionTask<ReceiveGlobalRankingReceivedRewardRequest, ReceiveGlobalRankingReceivedRewardResult>
        {
            public ReceiveGlobalRankingReceivedRewardTask(IGs2Session session, RestSessionRequestFactory factory, ReceiveGlobalRankingReceivedRewardRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(ReceiveGlobalRankingReceivedRewardRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "ranking2")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/me/global/reward/received/{rankingName}/{season}/reward/receive";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{rankingName}", !string.IsNullOrEmpty(request.RankingName) ? request.RankingName.ToString() : "null");
                url = url.Replace("{season}",request.Season != null ? request.Season.ToString() : "null");

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
		public IEnumerator ReceiveGlobalRankingReceivedReward(
                Request.ReceiveGlobalRankingReceivedRewardRequest request,
                UnityAction<AsyncResult<Result.ReceiveGlobalRankingReceivedRewardResult>> callback
        )
		{
			var task = new ReceiveGlobalRankingReceivedRewardTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.ReceiveGlobalRankingReceivedRewardResult>(task.Result, task.Error));
        }

		public IFuture<Result.ReceiveGlobalRankingReceivedRewardResult> ReceiveGlobalRankingReceivedRewardFuture(
                Request.ReceiveGlobalRankingReceivedRewardRequest request
        )
		{
			return new ReceiveGlobalRankingReceivedRewardTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.ReceiveGlobalRankingReceivedRewardResult> ReceiveGlobalRankingReceivedRewardAsync(
                Request.ReceiveGlobalRankingReceivedRewardRequest request
        )
		{
            AsyncResult<Result.ReceiveGlobalRankingReceivedRewardResult> result = null;
			await ReceiveGlobalRankingReceivedReward(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
    #else
		public ReceiveGlobalRankingReceivedRewardTask ReceiveGlobalRankingReceivedRewardAsync(
                Request.ReceiveGlobalRankingReceivedRewardRequest request
        )
		{
			return new ReceiveGlobalRankingReceivedRewardTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.ReceiveGlobalRankingReceivedRewardResult> ReceiveGlobalRankingReceivedRewardAsync(
                Request.ReceiveGlobalRankingReceivedRewardRequest request
        )
		{
			var task = new ReceiveGlobalRankingReceivedRewardTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class ReceiveGlobalRankingReceivedRewardByUserIdTask : Gs2RestSessionTask<ReceiveGlobalRankingReceivedRewardByUserIdRequest, ReceiveGlobalRankingReceivedRewardByUserIdResult>
        {
            public ReceiveGlobalRankingReceivedRewardByUserIdTask(IGs2Session session, RestSessionRequestFactory factory, ReceiveGlobalRankingReceivedRewardByUserIdRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(ReceiveGlobalRankingReceivedRewardByUserIdRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "ranking2")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/{userId}/global/reward/received/{rankingName}/{season}/reward/receive";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{userId}", !string.IsNullOrEmpty(request.UserId) ? request.UserId.ToString() : "null");
                url = url.Replace("{rankingName}", !string.IsNullOrEmpty(request.RankingName) ? request.RankingName.ToString() : "null");
                url = url.Replace("{season}",request.Season != null ? request.Season.ToString() : "null");

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
		public IEnumerator ReceiveGlobalRankingReceivedRewardByUserId(
                Request.ReceiveGlobalRankingReceivedRewardByUserIdRequest request,
                UnityAction<AsyncResult<Result.ReceiveGlobalRankingReceivedRewardByUserIdResult>> callback
        )
		{
			var task = new ReceiveGlobalRankingReceivedRewardByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.ReceiveGlobalRankingReceivedRewardByUserIdResult>(task.Result, task.Error));
        }

		public IFuture<Result.ReceiveGlobalRankingReceivedRewardByUserIdResult> ReceiveGlobalRankingReceivedRewardByUserIdFuture(
                Request.ReceiveGlobalRankingReceivedRewardByUserIdRequest request
        )
		{
			return new ReceiveGlobalRankingReceivedRewardByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.ReceiveGlobalRankingReceivedRewardByUserIdResult> ReceiveGlobalRankingReceivedRewardByUserIdAsync(
                Request.ReceiveGlobalRankingReceivedRewardByUserIdRequest request
        )
		{
            AsyncResult<Result.ReceiveGlobalRankingReceivedRewardByUserIdResult> result = null;
			await ReceiveGlobalRankingReceivedRewardByUserId(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
    #else
		public ReceiveGlobalRankingReceivedRewardByUserIdTask ReceiveGlobalRankingReceivedRewardByUserIdAsync(
                Request.ReceiveGlobalRankingReceivedRewardByUserIdRequest request
        )
		{
			return new ReceiveGlobalRankingReceivedRewardByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.ReceiveGlobalRankingReceivedRewardByUserIdResult> ReceiveGlobalRankingReceivedRewardByUserIdAsync(
                Request.ReceiveGlobalRankingReceivedRewardByUserIdRequest request
        )
		{
			var task = new ReceiveGlobalRankingReceivedRewardByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class GetGlobalRankingReceivedRewardTask : Gs2RestSessionTask<GetGlobalRankingReceivedRewardRequest, GetGlobalRankingReceivedRewardResult>
        {
            public GetGlobalRankingReceivedRewardTask(IGs2Session session, RestSessionRequestFactory factory, GetGlobalRankingReceivedRewardRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(GetGlobalRankingReceivedRewardRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "ranking2")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/me/global/reward/received/{rankingName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{rankingName}", !string.IsNullOrEmpty(request.RankingName) ? request.RankingName.ToString() : "null");

                var sessionRequest = Factory.Get(url);
                if (request.ContextStack != null)
                {
                    sessionRequest.AddQueryString("contextStack", request.ContextStack);
                }
                if (request.Season != null) {
                    sessionRequest.AddQueryString("season", $"{request.Season}");
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
		public IEnumerator GetGlobalRankingReceivedReward(
                Request.GetGlobalRankingReceivedRewardRequest request,
                UnityAction<AsyncResult<Result.GetGlobalRankingReceivedRewardResult>> callback
        )
		{
			var task = new GetGlobalRankingReceivedRewardTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.GetGlobalRankingReceivedRewardResult>(task.Result, task.Error));
        }

		public IFuture<Result.GetGlobalRankingReceivedRewardResult> GetGlobalRankingReceivedRewardFuture(
                Request.GetGlobalRankingReceivedRewardRequest request
        )
		{
			return new GetGlobalRankingReceivedRewardTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.GetGlobalRankingReceivedRewardResult> GetGlobalRankingReceivedRewardAsync(
                Request.GetGlobalRankingReceivedRewardRequest request
        )
		{
            AsyncResult<Result.GetGlobalRankingReceivedRewardResult> result = null;
			await GetGlobalRankingReceivedReward(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
    #else
		public GetGlobalRankingReceivedRewardTask GetGlobalRankingReceivedRewardAsync(
                Request.GetGlobalRankingReceivedRewardRequest request
        )
		{
			return new GetGlobalRankingReceivedRewardTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.GetGlobalRankingReceivedRewardResult> GetGlobalRankingReceivedRewardAsync(
                Request.GetGlobalRankingReceivedRewardRequest request
        )
		{
			var task = new GetGlobalRankingReceivedRewardTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class GetGlobalRankingReceivedRewardByUserIdTask : Gs2RestSessionTask<GetGlobalRankingReceivedRewardByUserIdRequest, GetGlobalRankingReceivedRewardByUserIdResult>
        {
            public GetGlobalRankingReceivedRewardByUserIdTask(IGs2Session session, RestSessionRequestFactory factory, GetGlobalRankingReceivedRewardByUserIdRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(GetGlobalRankingReceivedRewardByUserIdRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "ranking2")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/{userId}/global/reward/received/{rankingName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{rankingName}", !string.IsNullOrEmpty(request.RankingName) ? request.RankingName.ToString() : "null");
                url = url.Replace("{userId}", !string.IsNullOrEmpty(request.UserId) ? request.UserId.ToString() : "null");

                var sessionRequest = Factory.Get(url);
                if (request.ContextStack != null)
                {
                    sessionRequest.AddQueryString("contextStack", request.ContextStack);
                }
                if (request.Season != null) {
                    sessionRequest.AddQueryString("season", $"{request.Season}");
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
		public IEnumerator GetGlobalRankingReceivedRewardByUserId(
                Request.GetGlobalRankingReceivedRewardByUserIdRequest request,
                UnityAction<AsyncResult<Result.GetGlobalRankingReceivedRewardByUserIdResult>> callback
        )
		{
			var task = new GetGlobalRankingReceivedRewardByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.GetGlobalRankingReceivedRewardByUserIdResult>(task.Result, task.Error));
        }

		public IFuture<Result.GetGlobalRankingReceivedRewardByUserIdResult> GetGlobalRankingReceivedRewardByUserIdFuture(
                Request.GetGlobalRankingReceivedRewardByUserIdRequest request
        )
		{
			return new GetGlobalRankingReceivedRewardByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.GetGlobalRankingReceivedRewardByUserIdResult> GetGlobalRankingReceivedRewardByUserIdAsync(
                Request.GetGlobalRankingReceivedRewardByUserIdRequest request
        )
		{
            AsyncResult<Result.GetGlobalRankingReceivedRewardByUserIdResult> result = null;
			await GetGlobalRankingReceivedRewardByUserId(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
    #else
		public GetGlobalRankingReceivedRewardByUserIdTask GetGlobalRankingReceivedRewardByUserIdAsync(
                Request.GetGlobalRankingReceivedRewardByUserIdRequest request
        )
		{
			return new GetGlobalRankingReceivedRewardByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.GetGlobalRankingReceivedRewardByUserIdResult> GetGlobalRankingReceivedRewardByUserIdAsync(
                Request.GetGlobalRankingReceivedRewardByUserIdRequest request
        )
		{
			var task = new GetGlobalRankingReceivedRewardByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class DeleteGlobalRankingReceivedRewardByUserIdTask : Gs2RestSessionTask<DeleteGlobalRankingReceivedRewardByUserIdRequest, DeleteGlobalRankingReceivedRewardByUserIdResult>
        {
            public DeleteGlobalRankingReceivedRewardByUserIdTask(IGs2Session session, RestSessionRequestFactory factory, DeleteGlobalRankingReceivedRewardByUserIdRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DeleteGlobalRankingReceivedRewardByUserIdRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "ranking2")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/{userId}/global/reward/received/{rankingName}/{season}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{rankingName}", !string.IsNullOrEmpty(request.RankingName) ? request.RankingName.ToString() : "null");
                url = url.Replace("{userId}", !string.IsNullOrEmpty(request.UserId) ? request.UserId.ToString() : "null");

                var sessionRequest = Factory.Delete(url);
                if (request.ContextStack != null)
                {
                    sessionRequest.AddQueryString("contextStack", request.ContextStack);
                }
                if (request.Season != null) {
                    sessionRequest.AddQueryString("season", $"{request.Season}");
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
		public IEnumerator DeleteGlobalRankingReceivedRewardByUserId(
                Request.DeleteGlobalRankingReceivedRewardByUserIdRequest request,
                UnityAction<AsyncResult<Result.DeleteGlobalRankingReceivedRewardByUserIdResult>> callback
        )
		{
			var task = new DeleteGlobalRankingReceivedRewardByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.DeleteGlobalRankingReceivedRewardByUserIdResult>(task.Result, task.Error));
        }

		public IFuture<Result.DeleteGlobalRankingReceivedRewardByUserIdResult> DeleteGlobalRankingReceivedRewardByUserIdFuture(
                Request.DeleteGlobalRankingReceivedRewardByUserIdRequest request
        )
		{
			return new DeleteGlobalRankingReceivedRewardByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.DeleteGlobalRankingReceivedRewardByUserIdResult> DeleteGlobalRankingReceivedRewardByUserIdAsync(
                Request.DeleteGlobalRankingReceivedRewardByUserIdRequest request
        )
		{
            AsyncResult<Result.DeleteGlobalRankingReceivedRewardByUserIdResult> result = null;
			await DeleteGlobalRankingReceivedRewardByUserId(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
    #else
		public DeleteGlobalRankingReceivedRewardByUserIdTask DeleteGlobalRankingReceivedRewardByUserIdAsync(
                Request.DeleteGlobalRankingReceivedRewardByUserIdRequest request
        )
		{
			return new DeleteGlobalRankingReceivedRewardByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.DeleteGlobalRankingReceivedRewardByUserIdResult> DeleteGlobalRankingReceivedRewardByUserIdAsync(
                Request.DeleteGlobalRankingReceivedRewardByUserIdRequest request
        )
		{
			var task = new DeleteGlobalRankingReceivedRewardByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class CreateGlobalRankingReceivedRewardByStampTaskTask : Gs2RestSessionTask<CreateGlobalRankingReceivedRewardByStampTaskRequest, CreateGlobalRankingReceivedRewardByStampTaskResult>
        {
            public CreateGlobalRankingReceivedRewardByStampTaskTask(IGs2Session session, RestSessionRequestFactory factory, CreateGlobalRankingReceivedRewardByStampTaskRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(CreateGlobalRankingReceivedRewardByStampTaskRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "ranking2")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/stamp/ranking/global/reward/receive";

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
		public IEnumerator CreateGlobalRankingReceivedRewardByStampTask(
                Request.CreateGlobalRankingReceivedRewardByStampTaskRequest request,
                UnityAction<AsyncResult<Result.CreateGlobalRankingReceivedRewardByStampTaskResult>> callback
        )
		{
			var task = new CreateGlobalRankingReceivedRewardByStampTaskTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.CreateGlobalRankingReceivedRewardByStampTaskResult>(task.Result, task.Error));
        }

		public IFuture<Result.CreateGlobalRankingReceivedRewardByStampTaskResult> CreateGlobalRankingReceivedRewardByStampTaskFuture(
                Request.CreateGlobalRankingReceivedRewardByStampTaskRequest request
        )
		{
			return new CreateGlobalRankingReceivedRewardByStampTaskTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.CreateGlobalRankingReceivedRewardByStampTaskResult> CreateGlobalRankingReceivedRewardByStampTaskAsync(
                Request.CreateGlobalRankingReceivedRewardByStampTaskRequest request
        )
		{
            AsyncResult<Result.CreateGlobalRankingReceivedRewardByStampTaskResult> result = null;
			await CreateGlobalRankingReceivedRewardByStampTask(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
    #else
		public CreateGlobalRankingReceivedRewardByStampTaskTask CreateGlobalRankingReceivedRewardByStampTaskAsync(
                Request.CreateGlobalRankingReceivedRewardByStampTaskRequest request
        )
		{
			return new CreateGlobalRankingReceivedRewardByStampTaskTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.CreateGlobalRankingReceivedRewardByStampTaskResult> CreateGlobalRankingReceivedRewardByStampTaskAsync(
                Request.CreateGlobalRankingReceivedRewardByStampTaskRequest request
        )
		{
			var task = new CreateGlobalRankingReceivedRewardByStampTaskTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class DescribeGlobalRankingsTask : Gs2RestSessionTask<DescribeGlobalRankingsRequest, DescribeGlobalRankingsResult>
        {
            public DescribeGlobalRankingsTask(IGs2Session session, RestSessionRequestFactory factory, DescribeGlobalRankingsRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DescribeGlobalRankingsRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "ranking2")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/ranking/global/{rankingName}/user/me";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{rankingName}", !string.IsNullOrEmpty(request.RankingName) ? request.RankingName.ToString() : "null");

                var sessionRequest = Factory.Get(url);
                if (request.ContextStack != null)
                {
                    sessionRequest.AddQueryString("contextStack", request.ContextStack);
                }
                if (request.Season != null) {
                    sessionRequest.AddQueryString("season", $"{request.Season}");
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
		public IEnumerator DescribeGlobalRankings(
                Request.DescribeGlobalRankingsRequest request,
                UnityAction<AsyncResult<Result.DescribeGlobalRankingsResult>> callback
        )
		{
			var task = new DescribeGlobalRankingsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.DescribeGlobalRankingsResult>(task.Result, task.Error));
        }

		public IFuture<Result.DescribeGlobalRankingsResult> DescribeGlobalRankingsFuture(
                Request.DescribeGlobalRankingsRequest request
        )
		{
			return new DescribeGlobalRankingsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.DescribeGlobalRankingsResult> DescribeGlobalRankingsAsync(
                Request.DescribeGlobalRankingsRequest request
        )
		{
            AsyncResult<Result.DescribeGlobalRankingsResult> result = null;
			await DescribeGlobalRankings(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
    #else
		public DescribeGlobalRankingsTask DescribeGlobalRankingsAsync(
                Request.DescribeGlobalRankingsRequest request
        )
		{
			return new DescribeGlobalRankingsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.DescribeGlobalRankingsResult> DescribeGlobalRankingsAsync(
                Request.DescribeGlobalRankingsRequest request
        )
		{
			var task = new DescribeGlobalRankingsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class DescribeGlobalRankingsByUserIdTask : Gs2RestSessionTask<DescribeGlobalRankingsByUserIdRequest, DescribeGlobalRankingsByUserIdResult>
        {
            public DescribeGlobalRankingsByUserIdTask(IGs2Session session, RestSessionRequestFactory factory, DescribeGlobalRankingsByUserIdRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DescribeGlobalRankingsByUserIdRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "ranking2")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/ranking/global/{rankingName}/user/{userId}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{userId}", !string.IsNullOrEmpty(request.UserId) ? request.UserId.ToString() : "null");
                url = url.Replace("{rankingName}", !string.IsNullOrEmpty(request.RankingName) ? request.RankingName.ToString() : "null");

                var sessionRequest = Factory.Get(url);
                if (request.ContextStack != null)
                {
                    sessionRequest.AddQueryString("contextStack", request.ContextStack);
                }
                if (request.Season != null) {
                    sessionRequest.AddQueryString("season", $"{request.Season}");
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
		public IEnumerator DescribeGlobalRankingsByUserId(
                Request.DescribeGlobalRankingsByUserIdRequest request,
                UnityAction<AsyncResult<Result.DescribeGlobalRankingsByUserIdResult>> callback
        )
		{
			var task = new DescribeGlobalRankingsByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.DescribeGlobalRankingsByUserIdResult>(task.Result, task.Error));
        }

		public IFuture<Result.DescribeGlobalRankingsByUserIdResult> DescribeGlobalRankingsByUserIdFuture(
                Request.DescribeGlobalRankingsByUserIdRequest request
        )
		{
			return new DescribeGlobalRankingsByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.DescribeGlobalRankingsByUserIdResult> DescribeGlobalRankingsByUserIdAsync(
                Request.DescribeGlobalRankingsByUserIdRequest request
        )
		{
            AsyncResult<Result.DescribeGlobalRankingsByUserIdResult> result = null;
			await DescribeGlobalRankingsByUserId(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
    #else
		public DescribeGlobalRankingsByUserIdTask DescribeGlobalRankingsByUserIdAsync(
                Request.DescribeGlobalRankingsByUserIdRequest request
        )
		{
			return new DescribeGlobalRankingsByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.DescribeGlobalRankingsByUserIdResult> DescribeGlobalRankingsByUserIdAsync(
                Request.DescribeGlobalRankingsByUserIdRequest request
        )
		{
			var task = new DescribeGlobalRankingsByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class GetGlobalRankingTask : Gs2RestSessionTask<GetGlobalRankingRequest, GetGlobalRankingResult>
        {
            public GetGlobalRankingTask(IGs2Session session, RestSessionRequestFactory factory, GetGlobalRankingRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(GetGlobalRankingRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "ranking2")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/ranking/global/{rankingName}/user/me/rank";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{rankingName}", !string.IsNullOrEmpty(request.RankingName) ? request.RankingName.ToString() : "null");

                var sessionRequest = Factory.Get(url);
                if (request.ContextStack != null)
                {
                    sessionRequest.AddQueryString("contextStack", request.ContextStack);
                }
                if (request.Season != null) {
                    sessionRequest.AddQueryString("season", $"{request.Season}");
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
		public IEnumerator GetGlobalRanking(
                Request.GetGlobalRankingRequest request,
                UnityAction<AsyncResult<Result.GetGlobalRankingResult>> callback
        )
		{
			var task = new GetGlobalRankingTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.GetGlobalRankingResult>(task.Result, task.Error));
        }

		public IFuture<Result.GetGlobalRankingResult> GetGlobalRankingFuture(
                Request.GetGlobalRankingRequest request
        )
		{
			return new GetGlobalRankingTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.GetGlobalRankingResult> GetGlobalRankingAsync(
                Request.GetGlobalRankingRequest request
        )
		{
            AsyncResult<Result.GetGlobalRankingResult> result = null;
			await GetGlobalRanking(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
    #else
		public GetGlobalRankingTask GetGlobalRankingAsync(
                Request.GetGlobalRankingRequest request
        )
		{
			return new GetGlobalRankingTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.GetGlobalRankingResult> GetGlobalRankingAsync(
                Request.GetGlobalRankingRequest request
        )
		{
			var task = new GetGlobalRankingTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class GetGlobalRankingByUserIdTask : Gs2RestSessionTask<GetGlobalRankingByUserIdRequest, GetGlobalRankingByUserIdResult>
        {
            public GetGlobalRankingByUserIdTask(IGs2Session session, RestSessionRequestFactory factory, GetGlobalRankingByUserIdRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(GetGlobalRankingByUserIdRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "ranking2")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/ranking/global/{rankingName}/user/{userId}/rank";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{rankingName}", !string.IsNullOrEmpty(request.RankingName) ? request.RankingName.ToString() : "null");
                url = url.Replace("{userId}", !string.IsNullOrEmpty(request.UserId) ? request.UserId.ToString() : "null");

                var sessionRequest = Factory.Get(url);
                if (request.ContextStack != null)
                {
                    sessionRequest.AddQueryString("contextStack", request.ContextStack);
                }
                if (request.Season != null) {
                    sessionRequest.AddQueryString("season", $"{request.Season}");
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
		public IEnumerator GetGlobalRankingByUserId(
                Request.GetGlobalRankingByUserIdRequest request,
                UnityAction<AsyncResult<Result.GetGlobalRankingByUserIdResult>> callback
        )
		{
			var task = new GetGlobalRankingByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.GetGlobalRankingByUserIdResult>(task.Result, task.Error));
        }

		public IFuture<Result.GetGlobalRankingByUserIdResult> GetGlobalRankingByUserIdFuture(
                Request.GetGlobalRankingByUserIdRequest request
        )
		{
			return new GetGlobalRankingByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.GetGlobalRankingByUserIdResult> GetGlobalRankingByUserIdAsync(
                Request.GetGlobalRankingByUserIdRequest request
        )
		{
            AsyncResult<Result.GetGlobalRankingByUserIdResult> result = null;
			await GetGlobalRankingByUserId(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
    #else
		public GetGlobalRankingByUserIdTask GetGlobalRankingByUserIdAsync(
                Request.GetGlobalRankingByUserIdRequest request
        )
		{
			return new GetGlobalRankingByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.GetGlobalRankingByUserIdResult> GetGlobalRankingByUserIdAsync(
                Request.GetGlobalRankingByUserIdRequest request
        )
		{
			var task = new GetGlobalRankingByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class DescribeClusterRankingModelsTask : Gs2RestSessionTask<DescribeClusterRankingModelsRequest, DescribeClusterRankingModelsResult>
        {
            public DescribeClusterRankingModelsTask(IGs2Session session, RestSessionRequestFactory factory, DescribeClusterRankingModelsRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DescribeClusterRankingModelsRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "ranking2")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/model/cluster";

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
		public IEnumerator DescribeClusterRankingModels(
                Request.DescribeClusterRankingModelsRequest request,
                UnityAction<AsyncResult<Result.DescribeClusterRankingModelsResult>> callback
        )
		{
			var task = new DescribeClusterRankingModelsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.DescribeClusterRankingModelsResult>(task.Result, task.Error));
        }

		public IFuture<Result.DescribeClusterRankingModelsResult> DescribeClusterRankingModelsFuture(
                Request.DescribeClusterRankingModelsRequest request
        )
		{
			return new DescribeClusterRankingModelsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.DescribeClusterRankingModelsResult> DescribeClusterRankingModelsAsync(
                Request.DescribeClusterRankingModelsRequest request
        )
		{
            AsyncResult<Result.DescribeClusterRankingModelsResult> result = null;
			await DescribeClusterRankingModels(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
    #else
		public DescribeClusterRankingModelsTask DescribeClusterRankingModelsAsync(
                Request.DescribeClusterRankingModelsRequest request
        )
		{
			return new DescribeClusterRankingModelsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.DescribeClusterRankingModelsResult> DescribeClusterRankingModelsAsync(
                Request.DescribeClusterRankingModelsRequest request
        )
		{
			var task = new DescribeClusterRankingModelsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class GetClusterRankingModelTask : Gs2RestSessionTask<GetClusterRankingModelRequest, GetClusterRankingModelResult>
        {
            public GetClusterRankingModelTask(IGs2Session session, RestSessionRequestFactory factory, GetClusterRankingModelRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(GetClusterRankingModelRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "ranking2")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/model/cluster/{rankingName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{rankingName}", !string.IsNullOrEmpty(request.RankingName) ? request.RankingName.ToString() : "null");

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
		public IEnumerator GetClusterRankingModel(
                Request.GetClusterRankingModelRequest request,
                UnityAction<AsyncResult<Result.GetClusterRankingModelResult>> callback
        )
		{
			var task = new GetClusterRankingModelTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.GetClusterRankingModelResult>(task.Result, task.Error));
        }

		public IFuture<Result.GetClusterRankingModelResult> GetClusterRankingModelFuture(
                Request.GetClusterRankingModelRequest request
        )
		{
			return new GetClusterRankingModelTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.GetClusterRankingModelResult> GetClusterRankingModelAsync(
                Request.GetClusterRankingModelRequest request
        )
		{
            AsyncResult<Result.GetClusterRankingModelResult> result = null;
			await GetClusterRankingModel(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
    #else
		public GetClusterRankingModelTask GetClusterRankingModelAsync(
                Request.GetClusterRankingModelRequest request
        )
		{
			return new GetClusterRankingModelTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.GetClusterRankingModelResult> GetClusterRankingModelAsync(
                Request.GetClusterRankingModelRequest request
        )
		{
			var task = new GetClusterRankingModelTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class DescribeClusterRankingModelMastersTask : Gs2RestSessionTask<DescribeClusterRankingModelMastersRequest, DescribeClusterRankingModelMastersResult>
        {
            public DescribeClusterRankingModelMastersTask(IGs2Session session, RestSessionRequestFactory factory, DescribeClusterRankingModelMastersRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DescribeClusterRankingModelMastersRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "ranking2")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/master/cluster";

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
		public IEnumerator DescribeClusterRankingModelMasters(
                Request.DescribeClusterRankingModelMastersRequest request,
                UnityAction<AsyncResult<Result.DescribeClusterRankingModelMastersResult>> callback
        )
		{
			var task = new DescribeClusterRankingModelMastersTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.DescribeClusterRankingModelMastersResult>(task.Result, task.Error));
        }

		public IFuture<Result.DescribeClusterRankingModelMastersResult> DescribeClusterRankingModelMastersFuture(
                Request.DescribeClusterRankingModelMastersRequest request
        )
		{
			return new DescribeClusterRankingModelMastersTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.DescribeClusterRankingModelMastersResult> DescribeClusterRankingModelMastersAsync(
                Request.DescribeClusterRankingModelMastersRequest request
        )
		{
            AsyncResult<Result.DescribeClusterRankingModelMastersResult> result = null;
			await DescribeClusterRankingModelMasters(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
    #else
		public DescribeClusterRankingModelMastersTask DescribeClusterRankingModelMastersAsync(
                Request.DescribeClusterRankingModelMastersRequest request
        )
		{
			return new DescribeClusterRankingModelMastersTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.DescribeClusterRankingModelMastersResult> DescribeClusterRankingModelMastersAsync(
                Request.DescribeClusterRankingModelMastersRequest request
        )
		{
			var task = new DescribeClusterRankingModelMastersTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class CreateClusterRankingModelMasterTask : Gs2RestSessionTask<CreateClusterRankingModelMasterRequest, CreateClusterRankingModelMasterResult>
        {
            public CreateClusterRankingModelMasterTask(IGs2Session session, RestSessionRequestFactory factory, CreateClusterRankingModelMasterRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(CreateClusterRankingModelMasterRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "ranking2")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/master/cluster";

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
                if (request.ClusterType != null)
                {
                    jsonWriter.WritePropertyName("clusterType");
                    jsonWriter.Write(request.ClusterType);
                }
                if (request.MinimumValue != null)
                {
                    jsonWriter.WritePropertyName("minimumValue");
                    jsonWriter.Write(request.MinimumValue.ToString());
                }
                if (request.MaximumValue != null)
                {
                    jsonWriter.WritePropertyName("maximumValue");
                    jsonWriter.Write(request.MaximumValue.ToString());
                }
                if (request.Sum != null)
                {
                    jsonWriter.WritePropertyName("sum");
                    jsonWriter.Write(request.Sum.ToString());
                }
                if (request.ScoreTtlDays != null)
                {
                    jsonWriter.WritePropertyName("scoreTtlDays");
                    jsonWriter.Write(request.ScoreTtlDays.ToString());
                }
                if (request.OrderDirection != null)
                {
                    jsonWriter.WritePropertyName("orderDirection");
                    jsonWriter.Write(request.OrderDirection);
                }
                if (request.RankingRewards != null)
                {
                    jsonWriter.WritePropertyName("rankingRewards");
                    jsonWriter.WriteArrayStart();
                    foreach(var item in request.RankingRewards)
                    {
                        item.WriteJson(jsonWriter);
                    }
                    jsonWriter.WriteArrayEnd();
                }
                if (request.EntryPeriodEventId != null)
                {
                    jsonWriter.WritePropertyName("entryPeriodEventId");
                    jsonWriter.Write(request.EntryPeriodEventId);
                }
                if (request.AccessPeriodEventId != null)
                {
                    jsonWriter.WritePropertyName("accessPeriodEventId");
                    jsonWriter.Write(request.AccessPeriodEventId);
                }
                if (request.ContextStack != null)
                {
                    jsonWriter.WritePropertyName("contextStack");
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
		public IEnumerator CreateClusterRankingModelMaster(
                Request.CreateClusterRankingModelMasterRequest request,
                UnityAction<AsyncResult<Result.CreateClusterRankingModelMasterResult>> callback
        )
		{
			var task = new CreateClusterRankingModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.CreateClusterRankingModelMasterResult>(task.Result, task.Error));
        }

		public IFuture<Result.CreateClusterRankingModelMasterResult> CreateClusterRankingModelMasterFuture(
                Request.CreateClusterRankingModelMasterRequest request
        )
		{
			return new CreateClusterRankingModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.CreateClusterRankingModelMasterResult> CreateClusterRankingModelMasterAsync(
                Request.CreateClusterRankingModelMasterRequest request
        )
		{
            AsyncResult<Result.CreateClusterRankingModelMasterResult> result = null;
			await CreateClusterRankingModelMaster(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
    #else
		public CreateClusterRankingModelMasterTask CreateClusterRankingModelMasterAsync(
                Request.CreateClusterRankingModelMasterRequest request
        )
		{
			return new CreateClusterRankingModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.CreateClusterRankingModelMasterResult> CreateClusterRankingModelMasterAsync(
                Request.CreateClusterRankingModelMasterRequest request
        )
		{
			var task = new CreateClusterRankingModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class GetClusterRankingModelMasterTask : Gs2RestSessionTask<GetClusterRankingModelMasterRequest, GetClusterRankingModelMasterResult>
        {
            public GetClusterRankingModelMasterTask(IGs2Session session, RestSessionRequestFactory factory, GetClusterRankingModelMasterRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(GetClusterRankingModelMasterRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "ranking2")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/master/cluster/{rankingName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{rankingName}", !string.IsNullOrEmpty(request.RankingName) ? request.RankingName.ToString() : "null");

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
		public IEnumerator GetClusterRankingModelMaster(
                Request.GetClusterRankingModelMasterRequest request,
                UnityAction<AsyncResult<Result.GetClusterRankingModelMasterResult>> callback
        )
		{
			var task = new GetClusterRankingModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.GetClusterRankingModelMasterResult>(task.Result, task.Error));
        }

		public IFuture<Result.GetClusterRankingModelMasterResult> GetClusterRankingModelMasterFuture(
                Request.GetClusterRankingModelMasterRequest request
        )
		{
			return new GetClusterRankingModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.GetClusterRankingModelMasterResult> GetClusterRankingModelMasterAsync(
                Request.GetClusterRankingModelMasterRequest request
        )
		{
            AsyncResult<Result.GetClusterRankingModelMasterResult> result = null;
			await GetClusterRankingModelMaster(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
    #else
		public GetClusterRankingModelMasterTask GetClusterRankingModelMasterAsync(
                Request.GetClusterRankingModelMasterRequest request
        )
		{
			return new GetClusterRankingModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.GetClusterRankingModelMasterResult> GetClusterRankingModelMasterAsync(
                Request.GetClusterRankingModelMasterRequest request
        )
		{
			var task = new GetClusterRankingModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class UpdateClusterRankingModelMasterTask : Gs2RestSessionTask<UpdateClusterRankingModelMasterRequest, UpdateClusterRankingModelMasterResult>
        {
            public UpdateClusterRankingModelMasterTask(IGs2Session session, RestSessionRequestFactory factory, UpdateClusterRankingModelMasterRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(UpdateClusterRankingModelMasterRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "ranking2")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/master/cluster/{rankingName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{rankingName}", !string.IsNullOrEmpty(request.RankingName) ? request.RankingName.ToString() : "null");

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
                if (request.ClusterType != null)
                {
                    jsonWriter.WritePropertyName("clusterType");
                    jsonWriter.Write(request.ClusterType);
                }
                if (request.MinimumValue != null)
                {
                    jsonWriter.WritePropertyName("minimumValue");
                    jsonWriter.Write(request.MinimumValue.ToString());
                }
                if (request.MaximumValue != null)
                {
                    jsonWriter.WritePropertyName("maximumValue");
                    jsonWriter.Write(request.MaximumValue.ToString());
                }
                if (request.Sum != null)
                {
                    jsonWriter.WritePropertyName("sum");
                    jsonWriter.Write(request.Sum.ToString());
                }
                if (request.ScoreTtlDays != null)
                {
                    jsonWriter.WritePropertyName("scoreTtlDays");
                    jsonWriter.Write(request.ScoreTtlDays.ToString());
                }
                if (request.OrderDirection != null)
                {
                    jsonWriter.WritePropertyName("orderDirection");
                    jsonWriter.Write(request.OrderDirection);
                }
                if (request.RankingRewards != null)
                {
                    jsonWriter.WritePropertyName("rankingRewards");
                    jsonWriter.WriteArrayStart();
                    foreach(var item in request.RankingRewards)
                    {
                        item.WriteJson(jsonWriter);
                    }
                    jsonWriter.WriteArrayEnd();
                }
                if (request.EntryPeriodEventId != null)
                {
                    jsonWriter.WritePropertyName("entryPeriodEventId");
                    jsonWriter.Write(request.EntryPeriodEventId);
                }
                if (request.AccessPeriodEventId != null)
                {
                    jsonWriter.WritePropertyName("accessPeriodEventId");
                    jsonWriter.Write(request.AccessPeriodEventId);
                }
                if (request.ContextStack != null)
                {
                    jsonWriter.WritePropertyName("contextStack");
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
		public IEnumerator UpdateClusterRankingModelMaster(
                Request.UpdateClusterRankingModelMasterRequest request,
                UnityAction<AsyncResult<Result.UpdateClusterRankingModelMasterResult>> callback
        )
		{
			var task = new UpdateClusterRankingModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.UpdateClusterRankingModelMasterResult>(task.Result, task.Error));
        }

		public IFuture<Result.UpdateClusterRankingModelMasterResult> UpdateClusterRankingModelMasterFuture(
                Request.UpdateClusterRankingModelMasterRequest request
        )
		{
			return new UpdateClusterRankingModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.UpdateClusterRankingModelMasterResult> UpdateClusterRankingModelMasterAsync(
                Request.UpdateClusterRankingModelMasterRequest request
        )
		{
            AsyncResult<Result.UpdateClusterRankingModelMasterResult> result = null;
			await UpdateClusterRankingModelMaster(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
    #else
		public UpdateClusterRankingModelMasterTask UpdateClusterRankingModelMasterAsync(
                Request.UpdateClusterRankingModelMasterRequest request
        )
		{
			return new UpdateClusterRankingModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.UpdateClusterRankingModelMasterResult> UpdateClusterRankingModelMasterAsync(
                Request.UpdateClusterRankingModelMasterRequest request
        )
		{
			var task = new UpdateClusterRankingModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class DeleteClusterRankingModelMasterTask : Gs2RestSessionTask<DeleteClusterRankingModelMasterRequest, DeleteClusterRankingModelMasterResult>
        {
            public DeleteClusterRankingModelMasterTask(IGs2Session session, RestSessionRequestFactory factory, DeleteClusterRankingModelMasterRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DeleteClusterRankingModelMasterRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "ranking2")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/master/cluster/{rankingName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{rankingName}", !string.IsNullOrEmpty(request.RankingName) ? request.RankingName.ToString() : "null");

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
		public IEnumerator DeleteClusterRankingModelMaster(
                Request.DeleteClusterRankingModelMasterRequest request,
                UnityAction<AsyncResult<Result.DeleteClusterRankingModelMasterResult>> callback
        )
		{
			var task = new DeleteClusterRankingModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.DeleteClusterRankingModelMasterResult>(task.Result, task.Error));
        }

		public IFuture<Result.DeleteClusterRankingModelMasterResult> DeleteClusterRankingModelMasterFuture(
                Request.DeleteClusterRankingModelMasterRequest request
        )
		{
			return new DeleteClusterRankingModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.DeleteClusterRankingModelMasterResult> DeleteClusterRankingModelMasterAsync(
                Request.DeleteClusterRankingModelMasterRequest request
        )
		{
            AsyncResult<Result.DeleteClusterRankingModelMasterResult> result = null;
			await DeleteClusterRankingModelMaster(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
    #else
		public DeleteClusterRankingModelMasterTask DeleteClusterRankingModelMasterAsync(
                Request.DeleteClusterRankingModelMasterRequest request
        )
		{
			return new DeleteClusterRankingModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.DeleteClusterRankingModelMasterResult> DeleteClusterRankingModelMasterAsync(
                Request.DeleteClusterRankingModelMasterRequest request
        )
		{
			var task = new DeleteClusterRankingModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class DescribeClusterRankingScoresTask : Gs2RestSessionTask<DescribeClusterRankingScoresRequest, DescribeClusterRankingScoresResult>
        {
            public DescribeClusterRankingScoresTask(IGs2Session session, RestSessionRequestFactory factory, DescribeClusterRankingScoresRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DescribeClusterRankingScoresRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "ranking2")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/me/score/cluster";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");

                var sessionRequest = Factory.Get(url);
                if (request.ContextStack != null)
                {
                    sessionRequest.AddQueryString("contextStack", request.ContextStack);
                }
                if (request.RankingName != null) {
                    sessionRequest.AddQueryString("rankingName", $"{request.RankingName}");
                }
                if (request.ClusterName != null) {
                    sessionRequest.AddQueryString("clusterName", $"{request.ClusterName}");
                }
                if (request.Season != null) {
                    sessionRequest.AddQueryString("season", $"{request.Season}");
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
		public IEnumerator DescribeClusterRankingScores(
                Request.DescribeClusterRankingScoresRequest request,
                UnityAction<AsyncResult<Result.DescribeClusterRankingScoresResult>> callback
        )
		{
			var task = new DescribeClusterRankingScoresTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.DescribeClusterRankingScoresResult>(task.Result, task.Error));
        }

		public IFuture<Result.DescribeClusterRankingScoresResult> DescribeClusterRankingScoresFuture(
                Request.DescribeClusterRankingScoresRequest request
        )
		{
			return new DescribeClusterRankingScoresTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.DescribeClusterRankingScoresResult> DescribeClusterRankingScoresAsync(
                Request.DescribeClusterRankingScoresRequest request
        )
		{
            AsyncResult<Result.DescribeClusterRankingScoresResult> result = null;
			await DescribeClusterRankingScores(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
    #else
		public DescribeClusterRankingScoresTask DescribeClusterRankingScoresAsync(
                Request.DescribeClusterRankingScoresRequest request
        )
		{
			return new DescribeClusterRankingScoresTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.DescribeClusterRankingScoresResult> DescribeClusterRankingScoresAsync(
                Request.DescribeClusterRankingScoresRequest request
        )
		{
			var task = new DescribeClusterRankingScoresTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class DescribeClusterRankingScoresByUserIdTask : Gs2RestSessionTask<DescribeClusterRankingScoresByUserIdRequest, DescribeClusterRankingScoresByUserIdResult>
        {
            public DescribeClusterRankingScoresByUserIdTask(IGs2Session session, RestSessionRequestFactory factory, DescribeClusterRankingScoresByUserIdRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DescribeClusterRankingScoresByUserIdRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "ranking2")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/{userId}/score/cluster";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{userId}", !string.IsNullOrEmpty(request.UserId) ? request.UserId.ToString() : "null");

                var sessionRequest = Factory.Get(url);
                if (request.ContextStack != null)
                {
                    sessionRequest.AddQueryString("contextStack", request.ContextStack);
                }
                if (request.RankingName != null) {
                    sessionRequest.AddQueryString("rankingName", $"{request.RankingName}");
                }
                if (request.ClusterName != null) {
                    sessionRequest.AddQueryString("clusterName", $"{request.ClusterName}");
                }
                if (request.Season != null) {
                    sessionRequest.AddQueryString("season", $"{request.Season}");
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
		public IEnumerator DescribeClusterRankingScoresByUserId(
                Request.DescribeClusterRankingScoresByUserIdRequest request,
                UnityAction<AsyncResult<Result.DescribeClusterRankingScoresByUserIdResult>> callback
        )
		{
			var task = new DescribeClusterRankingScoresByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.DescribeClusterRankingScoresByUserIdResult>(task.Result, task.Error));
        }

		public IFuture<Result.DescribeClusterRankingScoresByUserIdResult> DescribeClusterRankingScoresByUserIdFuture(
                Request.DescribeClusterRankingScoresByUserIdRequest request
        )
		{
			return new DescribeClusterRankingScoresByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.DescribeClusterRankingScoresByUserIdResult> DescribeClusterRankingScoresByUserIdAsync(
                Request.DescribeClusterRankingScoresByUserIdRequest request
        )
		{
            AsyncResult<Result.DescribeClusterRankingScoresByUserIdResult> result = null;
			await DescribeClusterRankingScoresByUserId(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
    #else
		public DescribeClusterRankingScoresByUserIdTask DescribeClusterRankingScoresByUserIdAsync(
                Request.DescribeClusterRankingScoresByUserIdRequest request
        )
		{
			return new DescribeClusterRankingScoresByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.DescribeClusterRankingScoresByUserIdResult> DescribeClusterRankingScoresByUserIdAsync(
                Request.DescribeClusterRankingScoresByUserIdRequest request
        )
		{
			var task = new DescribeClusterRankingScoresByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class PutClusterRankingScoreTask : Gs2RestSessionTask<PutClusterRankingScoreRequest, PutClusterRankingScoreResult>
        {
            public PutClusterRankingScoreTask(IGs2Session session, RestSessionRequestFactory factory, PutClusterRankingScoreRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(PutClusterRankingScoreRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "ranking2")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/me/score/cluster/{rankingName}/{clusterName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{rankingName}", !string.IsNullOrEmpty(request.RankingName) ? request.RankingName.ToString() : "null");
                url = url.Replace("{clusterName}", !string.IsNullOrEmpty(request.ClusterName) ? request.ClusterName.ToString() : "null");

                var sessionRequest = Factory.Put(url);

                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);
                jsonWriter.WriteObjectStart();
                if (request.Score != null)
                {
                    jsonWriter.WritePropertyName("score");
                    jsonWriter.Write(request.Score.ToString());
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
		public IEnumerator PutClusterRankingScore(
                Request.PutClusterRankingScoreRequest request,
                UnityAction<AsyncResult<Result.PutClusterRankingScoreResult>> callback
        )
		{
			var task = new PutClusterRankingScoreTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.PutClusterRankingScoreResult>(task.Result, task.Error));
        }

		public IFuture<Result.PutClusterRankingScoreResult> PutClusterRankingScoreFuture(
                Request.PutClusterRankingScoreRequest request
        )
		{
			return new PutClusterRankingScoreTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.PutClusterRankingScoreResult> PutClusterRankingScoreAsync(
                Request.PutClusterRankingScoreRequest request
        )
		{
            AsyncResult<Result.PutClusterRankingScoreResult> result = null;
			await PutClusterRankingScore(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
    #else
		public PutClusterRankingScoreTask PutClusterRankingScoreAsync(
                Request.PutClusterRankingScoreRequest request
        )
		{
			return new PutClusterRankingScoreTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.PutClusterRankingScoreResult> PutClusterRankingScoreAsync(
                Request.PutClusterRankingScoreRequest request
        )
		{
			var task = new PutClusterRankingScoreTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class PutClusterRankingScoreByUserIdTask : Gs2RestSessionTask<PutClusterRankingScoreByUserIdRequest, PutClusterRankingScoreByUserIdResult>
        {
            public PutClusterRankingScoreByUserIdTask(IGs2Session session, RestSessionRequestFactory factory, PutClusterRankingScoreByUserIdRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(PutClusterRankingScoreByUserIdRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "ranking2")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/{userId}/score/cluster/{rankingName}/{clusterName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{rankingName}", !string.IsNullOrEmpty(request.RankingName) ? request.RankingName.ToString() : "null");
                url = url.Replace("{clusterName}", !string.IsNullOrEmpty(request.ClusterName) ? request.ClusterName.ToString() : "null");
                url = url.Replace("{userId}", !string.IsNullOrEmpty(request.UserId) ? request.UserId.ToString() : "null");

                var sessionRequest = Factory.Put(url);

                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);
                jsonWriter.WriteObjectStart();
                if (request.Score != null)
                {
                    jsonWriter.WritePropertyName("score");
                    jsonWriter.Write(request.Score.ToString());
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
		public IEnumerator PutClusterRankingScoreByUserId(
                Request.PutClusterRankingScoreByUserIdRequest request,
                UnityAction<AsyncResult<Result.PutClusterRankingScoreByUserIdResult>> callback
        )
		{
			var task = new PutClusterRankingScoreByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.PutClusterRankingScoreByUserIdResult>(task.Result, task.Error));
        }

		public IFuture<Result.PutClusterRankingScoreByUserIdResult> PutClusterRankingScoreByUserIdFuture(
                Request.PutClusterRankingScoreByUserIdRequest request
        )
		{
			return new PutClusterRankingScoreByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.PutClusterRankingScoreByUserIdResult> PutClusterRankingScoreByUserIdAsync(
                Request.PutClusterRankingScoreByUserIdRequest request
        )
		{
            AsyncResult<Result.PutClusterRankingScoreByUserIdResult> result = null;
			await PutClusterRankingScoreByUserId(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
    #else
		public PutClusterRankingScoreByUserIdTask PutClusterRankingScoreByUserIdAsync(
                Request.PutClusterRankingScoreByUserIdRequest request
        )
		{
			return new PutClusterRankingScoreByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.PutClusterRankingScoreByUserIdResult> PutClusterRankingScoreByUserIdAsync(
                Request.PutClusterRankingScoreByUserIdRequest request
        )
		{
			var task = new PutClusterRankingScoreByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class GetClusterRankingScoreTask : Gs2RestSessionTask<GetClusterRankingScoreRequest, GetClusterRankingScoreResult>
        {
            public GetClusterRankingScoreTask(IGs2Session session, RestSessionRequestFactory factory, GetClusterRankingScoreRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(GetClusterRankingScoreRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "ranking2")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/me/score/cluster/{rankingName}/{clusterName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{rankingName}", !string.IsNullOrEmpty(request.RankingName) ? request.RankingName.ToString() : "null");
                url = url.Replace("{clusterName}", !string.IsNullOrEmpty(request.ClusterName) ? request.ClusterName.ToString() : "null");

                var sessionRequest = Factory.Get(url);
                if (request.ContextStack != null)
                {
                    sessionRequest.AddQueryString("contextStack", request.ContextStack);
                }
                if (request.Season != null) {
                    sessionRequest.AddQueryString("season", $"{request.Season}");
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
		public IEnumerator GetClusterRankingScore(
                Request.GetClusterRankingScoreRequest request,
                UnityAction<AsyncResult<Result.GetClusterRankingScoreResult>> callback
        )
		{
			var task = new GetClusterRankingScoreTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.GetClusterRankingScoreResult>(task.Result, task.Error));
        }

		public IFuture<Result.GetClusterRankingScoreResult> GetClusterRankingScoreFuture(
                Request.GetClusterRankingScoreRequest request
        )
		{
			return new GetClusterRankingScoreTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.GetClusterRankingScoreResult> GetClusterRankingScoreAsync(
                Request.GetClusterRankingScoreRequest request
        )
		{
            AsyncResult<Result.GetClusterRankingScoreResult> result = null;
			await GetClusterRankingScore(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
    #else
		public GetClusterRankingScoreTask GetClusterRankingScoreAsync(
                Request.GetClusterRankingScoreRequest request
        )
		{
			return new GetClusterRankingScoreTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.GetClusterRankingScoreResult> GetClusterRankingScoreAsync(
                Request.GetClusterRankingScoreRequest request
        )
		{
			var task = new GetClusterRankingScoreTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class GetClusterRankingScoreByUserIdTask : Gs2RestSessionTask<GetClusterRankingScoreByUserIdRequest, GetClusterRankingScoreByUserIdResult>
        {
            public GetClusterRankingScoreByUserIdTask(IGs2Session session, RestSessionRequestFactory factory, GetClusterRankingScoreByUserIdRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(GetClusterRankingScoreByUserIdRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "ranking2")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/{userId}/score/cluster/{rankingName}/{clusterName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{rankingName}", !string.IsNullOrEmpty(request.RankingName) ? request.RankingName.ToString() : "null");
                url = url.Replace("{clusterName}", !string.IsNullOrEmpty(request.ClusterName) ? request.ClusterName.ToString() : "null");
                url = url.Replace("{userId}", !string.IsNullOrEmpty(request.UserId) ? request.UserId.ToString() : "null");

                var sessionRequest = Factory.Get(url);
                if (request.ContextStack != null)
                {
                    sessionRequest.AddQueryString("contextStack", request.ContextStack);
                }
                if (request.Season != null) {
                    sessionRequest.AddQueryString("season", $"{request.Season}");
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
		public IEnumerator GetClusterRankingScoreByUserId(
                Request.GetClusterRankingScoreByUserIdRequest request,
                UnityAction<AsyncResult<Result.GetClusterRankingScoreByUserIdResult>> callback
        )
		{
			var task = new GetClusterRankingScoreByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.GetClusterRankingScoreByUserIdResult>(task.Result, task.Error));
        }

		public IFuture<Result.GetClusterRankingScoreByUserIdResult> GetClusterRankingScoreByUserIdFuture(
                Request.GetClusterRankingScoreByUserIdRequest request
        )
		{
			return new GetClusterRankingScoreByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.GetClusterRankingScoreByUserIdResult> GetClusterRankingScoreByUserIdAsync(
                Request.GetClusterRankingScoreByUserIdRequest request
        )
		{
            AsyncResult<Result.GetClusterRankingScoreByUserIdResult> result = null;
			await GetClusterRankingScoreByUserId(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
    #else
		public GetClusterRankingScoreByUserIdTask GetClusterRankingScoreByUserIdAsync(
                Request.GetClusterRankingScoreByUserIdRequest request
        )
		{
			return new GetClusterRankingScoreByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.GetClusterRankingScoreByUserIdResult> GetClusterRankingScoreByUserIdAsync(
                Request.GetClusterRankingScoreByUserIdRequest request
        )
		{
			var task = new GetClusterRankingScoreByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class DeleteClusterRankingScoreByUserIdTask : Gs2RestSessionTask<DeleteClusterRankingScoreByUserIdRequest, DeleteClusterRankingScoreByUserIdResult>
        {
            public DeleteClusterRankingScoreByUserIdTask(IGs2Session session, RestSessionRequestFactory factory, DeleteClusterRankingScoreByUserIdRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DeleteClusterRankingScoreByUserIdRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "ranking2")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/{userId}/score/cluster/{rankingName}/{clusterName}/{season}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{rankingName}", !string.IsNullOrEmpty(request.RankingName) ? request.RankingName.ToString() : "null");
                url = url.Replace("{clusterName}", !string.IsNullOrEmpty(request.ClusterName) ? request.ClusterName.ToString() : "null");
                url = url.Replace("{userId}", !string.IsNullOrEmpty(request.UserId) ? request.UserId.ToString() : "null");

                var sessionRequest = Factory.Delete(url);
                if (request.ContextStack != null)
                {
                    sessionRequest.AddQueryString("contextStack", request.ContextStack);
                }
                if (request.Season != null) {
                    sessionRequest.AddQueryString("season", $"{request.Season}");
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
		public IEnumerator DeleteClusterRankingScoreByUserId(
                Request.DeleteClusterRankingScoreByUserIdRequest request,
                UnityAction<AsyncResult<Result.DeleteClusterRankingScoreByUserIdResult>> callback
        )
		{
			var task = new DeleteClusterRankingScoreByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.DeleteClusterRankingScoreByUserIdResult>(task.Result, task.Error));
        }

		public IFuture<Result.DeleteClusterRankingScoreByUserIdResult> DeleteClusterRankingScoreByUserIdFuture(
                Request.DeleteClusterRankingScoreByUserIdRequest request
        )
		{
			return new DeleteClusterRankingScoreByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.DeleteClusterRankingScoreByUserIdResult> DeleteClusterRankingScoreByUserIdAsync(
                Request.DeleteClusterRankingScoreByUserIdRequest request
        )
		{
            AsyncResult<Result.DeleteClusterRankingScoreByUserIdResult> result = null;
			await DeleteClusterRankingScoreByUserId(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
    #else
		public DeleteClusterRankingScoreByUserIdTask DeleteClusterRankingScoreByUserIdAsync(
                Request.DeleteClusterRankingScoreByUserIdRequest request
        )
		{
			return new DeleteClusterRankingScoreByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.DeleteClusterRankingScoreByUserIdResult> DeleteClusterRankingScoreByUserIdAsync(
                Request.DeleteClusterRankingScoreByUserIdRequest request
        )
		{
			var task = new DeleteClusterRankingScoreByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class VerifyClusterRankingScoreTask : Gs2RestSessionTask<VerifyClusterRankingScoreRequest, VerifyClusterRankingScoreResult>
        {
            public VerifyClusterRankingScoreTask(IGs2Session session, RestSessionRequestFactory factory, VerifyClusterRankingScoreRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(VerifyClusterRankingScoreRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "ranking2")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/{userId}/score/cluster/{rankingName}/{clusterName}/verify/{verifyType}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{rankingName}", !string.IsNullOrEmpty(request.RankingName) ? request.RankingName.ToString() : "null");
                url = url.Replace("{clusterName}", !string.IsNullOrEmpty(request.ClusterName) ? request.ClusterName.ToString() : "null");
                url = url.Replace("{verifyType}", !string.IsNullOrEmpty(request.VerifyType) ? request.VerifyType.ToString() : "null");

                var sessionRequest = Factory.Post(url);

                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);
                jsonWriter.WriteObjectStart();
                if (request.Season != null)
                {
                    jsonWriter.WritePropertyName("season");
                    jsonWriter.Write(request.Season.ToString());
                }
                if (request.Score != null)
                {
                    jsonWriter.WritePropertyName("score");
                    jsonWriter.Write(request.Score.ToString());
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
		public IEnumerator VerifyClusterRankingScore(
                Request.VerifyClusterRankingScoreRequest request,
                UnityAction<AsyncResult<Result.VerifyClusterRankingScoreResult>> callback
        )
		{
			var task = new VerifyClusterRankingScoreTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.VerifyClusterRankingScoreResult>(task.Result, task.Error));
        }

		public IFuture<Result.VerifyClusterRankingScoreResult> VerifyClusterRankingScoreFuture(
                Request.VerifyClusterRankingScoreRequest request
        )
		{
			return new VerifyClusterRankingScoreTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.VerifyClusterRankingScoreResult> VerifyClusterRankingScoreAsync(
                Request.VerifyClusterRankingScoreRequest request
        )
		{
            AsyncResult<Result.VerifyClusterRankingScoreResult> result = null;
			await VerifyClusterRankingScore(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
    #else
		public VerifyClusterRankingScoreTask VerifyClusterRankingScoreAsync(
                Request.VerifyClusterRankingScoreRequest request
        )
		{
			return new VerifyClusterRankingScoreTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.VerifyClusterRankingScoreResult> VerifyClusterRankingScoreAsync(
                Request.VerifyClusterRankingScoreRequest request
        )
		{
			var task = new VerifyClusterRankingScoreTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class VerifyClusterRankingScoreByUserIdTask : Gs2RestSessionTask<VerifyClusterRankingScoreByUserIdRequest, VerifyClusterRankingScoreByUserIdResult>
        {
            public VerifyClusterRankingScoreByUserIdTask(IGs2Session session, RestSessionRequestFactory factory, VerifyClusterRankingScoreByUserIdRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(VerifyClusterRankingScoreByUserIdRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "ranking2")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/{userId}/score/cluster/{rankingName}/{clusterName}/verify/{verifyType}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{userId}", !string.IsNullOrEmpty(request.UserId) ? request.UserId.ToString() : "null");
                url = url.Replace("{rankingName}", !string.IsNullOrEmpty(request.RankingName) ? request.RankingName.ToString() : "null");
                url = url.Replace("{clusterName}", !string.IsNullOrEmpty(request.ClusterName) ? request.ClusterName.ToString() : "null");
                url = url.Replace("{verifyType}", !string.IsNullOrEmpty(request.VerifyType) ? request.VerifyType.ToString() : "null");

                var sessionRequest = Factory.Post(url);

                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);
                jsonWriter.WriteObjectStart();
                if (request.Season != null)
                {
                    jsonWriter.WritePropertyName("season");
                    jsonWriter.Write(request.Season.ToString());
                }
                if (request.Score != null)
                {
                    jsonWriter.WritePropertyName("score");
                    jsonWriter.Write(request.Score.ToString());
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
		public IEnumerator VerifyClusterRankingScoreByUserId(
                Request.VerifyClusterRankingScoreByUserIdRequest request,
                UnityAction<AsyncResult<Result.VerifyClusterRankingScoreByUserIdResult>> callback
        )
		{
			var task = new VerifyClusterRankingScoreByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.VerifyClusterRankingScoreByUserIdResult>(task.Result, task.Error));
        }

		public IFuture<Result.VerifyClusterRankingScoreByUserIdResult> VerifyClusterRankingScoreByUserIdFuture(
                Request.VerifyClusterRankingScoreByUserIdRequest request
        )
		{
			return new VerifyClusterRankingScoreByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.VerifyClusterRankingScoreByUserIdResult> VerifyClusterRankingScoreByUserIdAsync(
                Request.VerifyClusterRankingScoreByUserIdRequest request
        )
		{
            AsyncResult<Result.VerifyClusterRankingScoreByUserIdResult> result = null;
			await VerifyClusterRankingScoreByUserId(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
    #else
		public VerifyClusterRankingScoreByUserIdTask VerifyClusterRankingScoreByUserIdAsync(
                Request.VerifyClusterRankingScoreByUserIdRequest request
        )
		{
			return new VerifyClusterRankingScoreByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.VerifyClusterRankingScoreByUserIdResult> VerifyClusterRankingScoreByUserIdAsync(
                Request.VerifyClusterRankingScoreByUserIdRequest request
        )
		{
			var task = new VerifyClusterRankingScoreByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class VerifyClusterRankingScoreByStampTaskTask : Gs2RestSessionTask<VerifyClusterRankingScoreByStampTaskRequest, VerifyClusterRankingScoreByStampTaskResult>
        {
            public VerifyClusterRankingScoreByStampTaskTask(IGs2Session session, RestSessionRequestFactory factory, VerifyClusterRankingScoreByStampTaskRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(VerifyClusterRankingScoreByStampTaskRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "ranking2")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/stamp/cluster/score/verify";

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
		public IEnumerator VerifyClusterRankingScoreByStampTask(
                Request.VerifyClusterRankingScoreByStampTaskRequest request,
                UnityAction<AsyncResult<Result.VerifyClusterRankingScoreByStampTaskResult>> callback
        )
		{
			var task = new VerifyClusterRankingScoreByStampTaskTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.VerifyClusterRankingScoreByStampTaskResult>(task.Result, task.Error));
        }

		public IFuture<Result.VerifyClusterRankingScoreByStampTaskResult> VerifyClusterRankingScoreByStampTaskFuture(
                Request.VerifyClusterRankingScoreByStampTaskRequest request
        )
		{
			return new VerifyClusterRankingScoreByStampTaskTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.VerifyClusterRankingScoreByStampTaskResult> VerifyClusterRankingScoreByStampTaskAsync(
                Request.VerifyClusterRankingScoreByStampTaskRequest request
        )
		{
            AsyncResult<Result.VerifyClusterRankingScoreByStampTaskResult> result = null;
			await VerifyClusterRankingScoreByStampTask(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
    #else
		public VerifyClusterRankingScoreByStampTaskTask VerifyClusterRankingScoreByStampTaskAsync(
                Request.VerifyClusterRankingScoreByStampTaskRequest request
        )
		{
			return new VerifyClusterRankingScoreByStampTaskTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.VerifyClusterRankingScoreByStampTaskResult> VerifyClusterRankingScoreByStampTaskAsync(
                Request.VerifyClusterRankingScoreByStampTaskRequest request
        )
		{
			var task = new VerifyClusterRankingScoreByStampTaskTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class DescribeClusterRankingReceivedRewardsTask : Gs2RestSessionTask<DescribeClusterRankingReceivedRewardsRequest, DescribeClusterRankingReceivedRewardsResult>
        {
            public DescribeClusterRankingReceivedRewardsTask(IGs2Session session, RestSessionRequestFactory factory, DescribeClusterRankingReceivedRewardsRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DescribeClusterRankingReceivedRewardsRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "ranking2")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/me/cluster/reward/received";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");

                var sessionRequest = Factory.Get(url);
                if (request.ContextStack != null)
                {
                    sessionRequest.AddQueryString("contextStack", request.ContextStack);
                }
                if (request.RankingName != null) {
                    sessionRequest.AddQueryString("rankingName", $"{request.RankingName}");
                }
                if (request.ClusterName != null) {
                    sessionRequest.AddQueryString("clusterName", $"{request.ClusterName}");
                }
                if (request.Season != null) {
                    sessionRequest.AddQueryString("season", $"{request.Season}");
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
		public IEnumerator DescribeClusterRankingReceivedRewards(
                Request.DescribeClusterRankingReceivedRewardsRequest request,
                UnityAction<AsyncResult<Result.DescribeClusterRankingReceivedRewardsResult>> callback
        )
		{
			var task = new DescribeClusterRankingReceivedRewardsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.DescribeClusterRankingReceivedRewardsResult>(task.Result, task.Error));
        }

		public IFuture<Result.DescribeClusterRankingReceivedRewardsResult> DescribeClusterRankingReceivedRewardsFuture(
                Request.DescribeClusterRankingReceivedRewardsRequest request
        )
		{
			return new DescribeClusterRankingReceivedRewardsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.DescribeClusterRankingReceivedRewardsResult> DescribeClusterRankingReceivedRewardsAsync(
                Request.DescribeClusterRankingReceivedRewardsRequest request
        )
		{
            AsyncResult<Result.DescribeClusterRankingReceivedRewardsResult> result = null;
			await DescribeClusterRankingReceivedRewards(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
    #else
		public DescribeClusterRankingReceivedRewardsTask DescribeClusterRankingReceivedRewardsAsync(
                Request.DescribeClusterRankingReceivedRewardsRequest request
        )
		{
			return new DescribeClusterRankingReceivedRewardsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.DescribeClusterRankingReceivedRewardsResult> DescribeClusterRankingReceivedRewardsAsync(
                Request.DescribeClusterRankingReceivedRewardsRequest request
        )
		{
			var task = new DescribeClusterRankingReceivedRewardsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class DescribeClusterRankingReceivedRewardsByUserIdTask : Gs2RestSessionTask<DescribeClusterRankingReceivedRewardsByUserIdRequest, DescribeClusterRankingReceivedRewardsByUserIdResult>
        {
            public DescribeClusterRankingReceivedRewardsByUserIdTask(IGs2Session session, RestSessionRequestFactory factory, DescribeClusterRankingReceivedRewardsByUserIdRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DescribeClusterRankingReceivedRewardsByUserIdRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "ranking2")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/{userId}/cluster/reward/received";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{userId}", !string.IsNullOrEmpty(request.UserId) ? request.UserId.ToString() : "null");

                var sessionRequest = Factory.Get(url);
                if (request.ContextStack != null)
                {
                    sessionRequest.AddQueryString("contextStack", request.ContextStack);
                }
                if (request.RankingName != null) {
                    sessionRequest.AddQueryString("rankingName", $"{request.RankingName}");
                }
                if (request.ClusterName != null) {
                    sessionRequest.AddQueryString("clusterName", $"{request.ClusterName}");
                }
                if (request.Season != null) {
                    sessionRequest.AddQueryString("season", $"{request.Season}");
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
		public IEnumerator DescribeClusterRankingReceivedRewardsByUserId(
                Request.DescribeClusterRankingReceivedRewardsByUserIdRequest request,
                UnityAction<AsyncResult<Result.DescribeClusterRankingReceivedRewardsByUserIdResult>> callback
        )
		{
			var task = new DescribeClusterRankingReceivedRewardsByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.DescribeClusterRankingReceivedRewardsByUserIdResult>(task.Result, task.Error));
        }

		public IFuture<Result.DescribeClusterRankingReceivedRewardsByUserIdResult> DescribeClusterRankingReceivedRewardsByUserIdFuture(
                Request.DescribeClusterRankingReceivedRewardsByUserIdRequest request
        )
		{
			return new DescribeClusterRankingReceivedRewardsByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.DescribeClusterRankingReceivedRewardsByUserIdResult> DescribeClusterRankingReceivedRewardsByUserIdAsync(
                Request.DescribeClusterRankingReceivedRewardsByUserIdRequest request
        )
		{
            AsyncResult<Result.DescribeClusterRankingReceivedRewardsByUserIdResult> result = null;
			await DescribeClusterRankingReceivedRewardsByUserId(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
    #else
		public DescribeClusterRankingReceivedRewardsByUserIdTask DescribeClusterRankingReceivedRewardsByUserIdAsync(
                Request.DescribeClusterRankingReceivedRewardsByUserIdRequest request
        )
		{
			return new DescribeClusterRankingReceivedRewardsByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.DescribeClusterRankingReceivedRewardsByUserIdResult> DescribeClusterRankingReceivedRewardsByUserIdAsync(
                Request.DescribeClusterRankingReceivedRewardsByUserIdRequest request
        )
		{
			var task = new DescribeClusterRankingReceivedRewardsByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class CreateClusterRankingReceivedRewardTask : Gs2RestSessionTask<CreateClusterRankingReceivedRewardRequest, CreateClusterRankingReceivedRewardResult>
        {
            public CreateClusterRankingReceivedRewardTask(IGs2Session session, RestSessionRequestFactory factory, CreateClusterRankingReceivedRewardRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(CreateClusterRankingReceivedRewardRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "ranking2")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/me/cluster/reward/received/{rankingName}/{clusterName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{rankingName}", !string.IsNullOrEmpty(request.RankingName) ? request.RankingName.ToString() : "null");
                url = url.Replace("{clusterName}", !string.IsNullOrEmpty(request.ClusterName) ? request.ClusterName.ToString() : "null");

                var sessionRequest = Factory.Post(url);

                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);
                jsonWriter.WriteObjectStart();
                if (request.Season != null)
                {
                    jsonWriter.WritePropertyName("season");
                    jsonWriter.Write(request.Season.ToString());
                }
                if (request.ContextStack != null)
                {
                    jsonWriter.WritePropertyName("contextStack");
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
		public IEnumerator CreateClusterRankingReceivedReward(
                Request.CreateClusterRankingReceivedRewardRequest request,
                UnityAction<AsyncResult<Result.CreateClusterRankingReceivedRewardResult>> callback
        )
		{
			var task = new CreateClusterRankingReceivedRewardTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.CreateClusterRankingReceivedRewardResult>(task.Result, task.Error));
        }

		public IFuture<Result.CreateClusterRankingReceivedRewardResult> CreateClusterRankingReceivedRewardFuture(
                Request.CreateClusterRankingReceivedRewardRequest request
        )
		{
			return new CreateClusterRankingReceivedRewardTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.CreateClusterRankingReceivedRewardResult> CreateClusterRankingReceivedRewardAsync(
                Request.CreateClusterRankingReceivedRewardRequest request
        )
		{
            AsyncResult<Result.CreateClusterRankingReceivedRewardResult> result = null;
			await CreateClusterRankingReceivedReward(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
    #else
		public CreateClusterRankingReceivedRewardTask CreateClusterRankingReceivedRewardAsync(
                Request.CreateClusterRankingReceivedRewardRequest request
        )
		{
			return new CreateClusterRankingReceivedRewardTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.CreateClusterRankingReceivedRewardResult> CreateClusterRankingReceivedRewardAsync(
                Request.CreateClusterRankingReceivedRewardRequest request
        )
		{
			var task = new CreateClusterRankingReceivedRewardTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class CreateClusterRankingReceivedRewardByUserIdTask : Gs2RestSessionTask<CreateClusterRankingReceivedRewardByUserIdRequest, CreateClusterRankingReceivedRewardByUserIdResult>
        {
            public CreateClusterRankingReceivedRewardByUserIdTask(IGs2Session session, RestSessionRequestFactory factory, CreateClusterRankingReceivedRewardByUserIdRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(CreateClusterRankingReceivedRewardByUserIdRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "ranking2")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/{userId}/cluster/reward/received/{rankingName}/{clusterName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{rankingName}", !string.IsNullOrEmpty(request.RankingName) ? request.RankingName.ToString() : "null");
                url = url.Replace("{clusterName}", !string.IsNullOrEmpty(request.ClusterName) ? request.ClusterName.ToString() : "null");
                url = url.Replace("{userId}", !string.IsNullOrEmpty(request.UserId) ? request.UserId.ToString() : "null");

                var sessionRequest = Factory.Post(url);

                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);
                jsonWriter.WriteObjectStart();
                if (request.Season != null)
                {
                    jsonWriter.WritePropertyName("season");
                    jsonWriter.Write(request.Season.ToString());
                }
                if (request.ContextStack != null)
                {
                    jsonWriter.WritePropertyName("contextStack");
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
		public IEnumerator CreateClusterRankingReceivedRewardByUserId(
                Request.CreateClusterRankingReceivedRewardByUserIdRequest request,
                UnityAction<AsyncResult<Result.CreateClusterRankingReceivedRewardByUserIdResult>> callback
        )
		{
			var task = new CreateClusterRankingReceivedRewardByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.CreateClusterRankingReceivedRewardByUserIdResult>(task.Result, task.Error));
        }

		public IFuture<Result.CreateClusterRankingReceivedRewardByUserIdResult> CreateClusterRankingReceivedRewardByUserIdFuture(
                Request.CreateClusterRankingReceivedRewardByUserIdRequest request
        )
		{
			return new CreateClusterRankingReceivedRewardByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.CreateClusterRankingReceivedRewardByUserIdResult> CreateClusterRankingReceivedRewardByUserIdAsync(
                Request.CreateClusterRankingReceivedRewardByUserIdRequest request
        )
		{
            AsyncResult<Result.CreateClusterRankingReceivedRewardByUserIdResult> result = null;
			await CreateClusterRankingReceivedRewardByUserId(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
    #else
		public CreateClusterRankingReceivedRewardByUserIdTask CreateClusterRankingReceivedRewardByUserIdAsync(
                Request.CreateClusterRankingReceivedRewardByUserIdRequest request
        )
		{
			return new CreateClusterRankingReceivedRewardByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.CreateClusterRankingReceivedRewardByUserIdResult> CreateClusterRankingReceivedRewardByUserIdAsync(
                Request.CreateClusterRankingReceivedRewardByUserIdRequest request
        )
		{
			var task = new CreateClusterRankingReceivedRewardByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class ReceiveClusterRankingReceivedRewardTask : Gs2RestSessionTask<ReceiveClusterRankingReceivedRewardRequest, ReceiveClusterRankingReceivedRewardResult>
        {
            public ReceiveClusterRankingReceivedRewardTask(IGs2Session session, RestSessionRequestFactory factory, ReceiveClusterRankingReceivedRewardRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(ReceiveClusterRankingReceivedRewardRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "ranking2")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/me/cluster/reward/received/{rankingName}/{clusterName}/{season}/reward/receive";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{rankingName}", !string.IsNullOrEmpty(request.RankingName) ? request.RankingName.ToString() : "null");
                url = url.Replace("{clusterName}", !string.IsNullOrEmpty(request.ClusterName) ? request.ClusterName.ToString() : "null");
                url = url.Replace("{season}",request.Season != null ? request.Season.ToString() : "null");

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
		public IEnumerator ReceiveClusterRankingReceivedReward(
                Request.ReceiveClusterRankingReceivedRewardRequest request,
                UnityAction<AsyncResult<Result.ReceiveClusterRankingReceivedRewardResult>> callback
        )
		{
			var task = new ReceiveClusterRankingReceivedRewardTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.ReceiveClusterRankingReceivedRewardResult>(task.Result, task.Error));
        }

		public IFuture<Result.ReceiveClusterRankingReceivedRewardResult> ReceiveClusterRankingReceivedRewardFuture(
                Request.ReceiveClusterRankingReceivedRewardRequest request
        )
		{
			return new ReceiveClusterRankingReceivedRewardTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.ReceiveClusterRankingReceivedRewardResult> ReceiveClusterRankingReceivedRewardAsync(
                Request.ReceiveClusterRankingReceivedRewardRequest request
        )
		{
            AsyncResult<Result.ReceiveClusterRankingReceivedRewardResult> result = null;
			await ReceiveClusterRankingReceivedReward(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
    #else
		public ReceiveClusterRankingReceivedRewardTask ReceiveClusterRankingReceivedRewardAsync(
                Request.ReceiveClusterRankingReceivedRewardRequest request
        )
		{
			return new ReceiveClusterRankingReceivedRewardTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.ReceiveClusterRankingReceivedRewardResult> ReceiveClusterRankingReceivedRewardAsync(
                Request.ReceiveClusterRankingReceivedRewardRequest request
        )
		{
			var task = new ReceiveClusterRankingReceivedRewardTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class ReceiveClusterRankingReceivedRewardByUserIdTask : Gs2RestSessionTask<ReceiveClusterRankingReceivedRewardByUserIdRequest, ReceiveClusterRankingReceivedRewardByUserIdResult>
        {
            public ReceiveClusterRankingReceivedRewardByUserIdTask(IGs2Session session, RestSessionRequestFactory factory, ReceiveClusterRankingReceivedRewardByUserIdRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(ReceiveClusterRankingReceivedRewardByUserIdRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "ranking2")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/{userId}/cluster/reward/received/{rankingName}/{clusterName}/{season}/reward/receive";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{userId}", !string.IsNullOrEmpty(request.UserId) ? request.UserId.ToString() : "null");
                url = url.Replace("{rankingName}", !string.IsNullOrEmpty(request.RankingName) ? request.RankingName.ToString() : "null");
                url = url.Replace("{clusterName}", !string.IsNullOrEmpty(request.ClusterName) ? request.ClusterName.ToString() : "null");
                url = url.Replace("{season}",request.Season != null ? request.Season.ToString() : "null");

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
		public IEnumerator ReceiveClusterRankingReceivedRewardByUserId(
                Request.ReceiveClusterRankingReceivedRewardByUserIdRequest request,
                UnityAction<AsyncResult<Result.ReceiveClusterRankingReceivedRewardByUserIdResult>> callback
        )
		{
			var task = new ReceiveClusterRankingReceivedRewardByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.ReceiveClusterRankingReceivedRewardByUserIdResult>(task.Result, task.Error));
        }

		public IFuture<Result.ReceiveClusterRankingReceivedRewardByUserIdResult> ReceiveClusterRankingReceivedRewardByUserIdFuture(
                Request.ReceiveClusterRankingReceivedRewardByUserIdRequest request
        )
		{
			return new ReceiveClusterRankingReceivedRewardByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.ReceiveClusterRankingReceivedRewardByUserIdResult> ReceiveClusterRankingReceivedRewardByUserIdAsync(
                Request.ReceiveClusterRankingReceivedRewardByUserIdRequest request
        )
		{
            AsyncResult<Result.ReceiveClusterRankingReceivedRewardByUserIdResult> result = null;
			await ReceiveClusterRankingReceivedRewardByUserId(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
    #else
		public ReceiveClusterRankingReceivedRewardByUserIdTask ReceiveClusterRankingReceivedRewardByUserIdAsync(
                Request.ReceiveClusterRankingReceivedRewardByUserIdRequest request
        )
		{
			return new ReceiveClusterRankingReceivedRewardByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.ReceiveClusterRankingReceivedRewardByUserIdResult> ReceiveClusterRankingReceivedRewardByUserIdAsync(
                Request.ReceiveClusterRankingReceivedRewardByUserIdRequest request
        )
		{
			var task = new ReceiveClusterRankingReceivedRewardByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class GetClusterRankingReceivedRewardTask : Gs2RestSessionTask<GetClusterRankingReceivedRewardRequest, GetClusterRankingReceivedRewardResult>
        {
            public GetClusterRankingReceivedRewardTask(IGs2Session session, RestSessionRequestFactory factory, GetClusterRankingReceivedRewardRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(GetClusterRankingReceivedRewardRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "ranking2")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/me/cluster/reward/received/{rankingName}/{clusterName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{rankingName}", !string.IsNullOrEmpty(request.RankingName) ? request.RankingName.ToString() : "null");
                url = url.Replace("{clusterName}", !string.IsNullOrEmpty(request.ClusterName) ? request.ClusterName.ToString() : "null");

                var sessionRequest = Factory.Get(url);
                if (request.ContextStack != null)
                {
                    sessionRequest.AddQueryString("contextStack", request.ContextStack);
                }
                if (request.Season != null) {
                    sessionRequest.AddQueryString("season", $"{request.Season}");
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
		public IEnumerator GetClusterRankingReceivedReward(
                Request.GetClusterRankingReceivedRewardRequest request,
                UnityAction<AsyncResult<Result.GetClusterRankingReceivedRewardResult>> callback
        )
		{
			var task = new GetClusterRankingReceivedRewardTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.GetClusterRankingReceivedRewardResult>(task.Result, task.Error));
        }

		public IFuture<Result.GetClusterRankingReceivedRewardResult> GetClusterRankingReceivedRewardFuture(
                Request.GetClusterRankingReceivedRewardRequest request
        )
		{
			return new GetClusterRankingReceivedRewardTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.GetClusterRankingReceivedRewardResult> GetClusterRankingReceivedRewardAsync(
                Request.GetClusterRankingReceivedRewardRequest request
        )
		{
            AsyncResult<Result.GetClusterRankingReceivedRewardResult> result = null;
			await GetClusterRankingReceivedReward(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
    #else
		public GetClusterRankingReceivedRewardTask GetClusterRankingReceivedRewardAsync(
                Request.GetClusterRankingReceivedRewardRequest request
        )
		{
			return new GetClusterRankingReceivedRewardTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.GetClusterRankingReceivedRewardResult> GetClusterRankingReceivedRewardAsync(
                Request.GetClusterRankingReceivedRewardRequest request
        )
		{
			var task = new GetClusterRankingReceivedRewardTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class GetClusterRankingReceivedRewardByUserIdTask : Gs2RestSessionTask<GetClusterRankingReceivedRewardByUserIdRequest, GetClusterRankingReceivedRewardByUserIdResult>
        {
            public GetClusterRankingReceivedRewardByUserIdTask(IGs2Session session, RestSessionRequestFactory factory, GetClusterRankingReceivedRewardByUserIdRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(GetClusterRankingReceivedRewardByUserIdRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "ranking2")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/{userId}/cluster/reward/received/{rankingName}/{clusterName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{rankingName}", !string.IsNullOrEmpty(request.RankingName) ? request.RankingName.ToString() : "null");
                url = url.Replace("{clusterName}", !string.IsNullOrEmpty(request.ClusterName) ? request.ClusterName.ToString() : "null");
                url = url.Replace("{userId}", !string.IsNullOrEmpty(request.UserId) ? request.UserId.ToString() : "null");

                var sessionRequest = Factory.Get(url);
                if (request.ContextStack != null)
                {
                    sessionRequest.AddQueryString("contextStack", request.ContextStack);
                }
                if (request.Season != null) {
                    sessionRequest.AddQueryString("season", $"{request.Season}");
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
		public IEnumerator GetClusterRankingReceivedRewardByUserId(
                Request.GetClusterRankingReceivedRewardByUserIdRequest request,
                UnityAction<AsyncResult<Result.GetClusterRankingReceivedRewardByUserIdResult>> callback
        )
		{
			var task = new GetClusterRankingReceivedRewardByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.GetClusterRankingReceivedRewardByUserIdResult>(task.Result, task.Error));
        }

		public IFuture<Result.GetClusterRankingReceivedRewardByUserIdResult> GetClusterRankingReceivedRewardByUserIdFuture(
                Request.GetClusterRankingReceivedRewardByUserIdRequest request
        )
		{
			return new GetClusterRankingReceivedRewardByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.GetClusterRankingReceivedRewardByUserIdResult> GetClusterRankingReceivedRewardByUserIdAsync(
                Request.GetClusterRankingReceivedRewardByUserIdRequest request
        )
		{
            AsyncResult<Result.GetClusterRankingReceivedRewardByUserIdResult> result = null;
			await GetClusterRankingReceivedRewardByUserId(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
    #else
		public GetClusterRankingReceivedRewardByUserIdTask GetClusterRankingReceivedRewardByUserIdAsync(
                Request.GetClusterRankingReceivedRewardByUserIdRequest request
        )
		{
			return new GetClusterRankingReceivedRewardByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.GetClusterRankingReceivedRewardByUserIdResult> GetClusterRankingReceivedRewardByUserIdAsync(
                Request.GetClusterRankingReceivedRewardByUserIdRequest request
        )
		{
			var task = new GetClusterRankingReceivedRewardByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class DeleteClusterRankingReceivedRewardByUserIdTask : Gs2RestSessionTask<DeleteClusterRankingReceivedRewardByUserIdRequest, DeleteClusterRankingReceivedRewardByUserIdResult>
        {
            public DeleteClusterRankingReceivedRewardByUserIdTask(IGs2Session session, RestSessionRequestFactory factory, DeleteClusterRankingReceivedRewardByUserIdRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DeleteClusterRankingReceivedRewardByUserIdRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "ranking2")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/{userId}/cluster/reward/received/{rankingName}/{clusterName}/{season}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{rankingName}", !string.IsNullOrEmpty(request.RankingName) ? request.RankingName.ToString() : "null");
                url = url.Replace("{clusterName}", !string.IsNullOrEmpty(request.ClusterName) ? request.ClusterName.ToString() : "null");
                url = url.Replace("{userId}", !string.IsNullOrEmpty(request.UserId) ? request.UserId.ToString() : "null");

                var sessionRequest = Factory.Delete(url);
                if (request.ContextStack != null)
                {
                    sessionRequest.AddQueryString("contextStack", request.ContextStack);
                }
                if (request.Season != null) {
                    sessionRequest.AddQueryString("season", $"{request.Season}");
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
		public IEnumerator DeleteClusterRankingReceivedRewardByUserId(
                Request.DeleteClusterRankingReceivedRewardByUserIdRequest request,
                UnityAction<AsyncResult<Result.DeleteClusterRankingReceivedRewardByUserIdResult>> callback
        )
		{
			var task = new DeleteClusterRankingReceivedRewardByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.DeleteClusterRankingReceivedRewardByUserIdResult>(task.Result, task.Error));
        }

		public IFuture<Result.DeleteClusterRankingReceivedRewardByUserIdResult> DeleteClusterRankingReceivedRewardByUserIdFuture(
                Request.DeleteClusterRankingReceivedRewardByUserIdRequest request
        )
		{
			return new DeleteClusterRankingReceivedRewardByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.DeleteClusterRankingReceivedRewardByUserIdResult> DeleteClusterRankingReceivedRewardByUserIdAsync(
                Request.DeleteClusterRankingReceivedRewardByUserIdRequest request
        )
		{
            AsyncResult<Result.DeleteClusterRankingReceivedRewardByUserIdResult> result = null;
			await DeleteClusterRankingReceivedRewardByUserId(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
    #else
		public DeleteClusterRankingReceivedRewardByUserIdTask DeleteClusterRankingReceivedRewardByUserIdAsync(
                Request.DeleteClusterRankingReceivedRewardByUserIdRequest request
        )
		{
			return new DeleteClusterRankingReceivedRewardByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.DeleteClusterRankingReceivedRewardByUserIdResult> DeleteClusterRankingReceivedRewardByUserIdAsync(
                Request.DeleteClusterRankingReceivedRewardByUserIdRequest request
        )
		{
			var task = new DeleteClusterRankingReceivedRewardByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class CreateClusterRankingReceivedRewardByStampTaskTask : Gs2RestSessionTask<CreateClusterRankingReceivedRewardByStampTaskRequest, CreateClusterRankingReceivedRewardByStampTaskResult>
        {
            public CreateClusterRankingReceivedRewardByStampTaskTask(IGs2Session session, RestSessionRequestFactory factory, CreateClusterRankingReceivedRewardByStampTaskRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(CreateClusterRankingReceivedRewardByStampTaskRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "ranking2")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/stamp/ranking/cluster/reward/receive";

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
		public IEnumerator CreateClusterRankingReceivedRewardByStampTask(
                Request.CreateClusterRankingReceivedRewardByStampTaskRequest request,
                UnityAction<AsyncResult<Result.CreateClusterRankingReceivedRewardByStampTaskResult>> callback
        )
		{
			var task = new CreateClusterRankingReceivedRewardByStampTaskTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.CreateClusterRankingReceivedRewardByStampTaskResult>(task.Result, task.Error));
        }

		public IFuture<Result.CreateClusterRankingReceivedRewardByStampTaskResult> CreateClusterRankingReceivedRewardByStampTaskFuture(
                Request.CreateClusterRankingReceivedRewardByStampTaskRequest request
        )
		{
			return new CreateClusterRankingReceivedRewardByStampTaskTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.CreateClusterRankingReceivedRewardByStampTaskResult> CreateClusterRankingReceivedRewardByStampTaskAsync(
                Request.CreateClusterRankingReceivedRewardByStampTaskRequest request
        )
		{
            AsyncResult<Result.CreateClusterRankingReceivedRewardByStampTaskResult> result = null;
			await CreateClusterRankingReceivedRewardByStampTask(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
    #else
		public CreateClusterRankingReceivedRewardByStampTaskTask CreateClusterRankingReceivedRewardByStampTaskAsync(
                Request.CreateClusterRankingReceivedRewardByStampTaskRequest request
        )
		{
			return new CreateClusterRankingReceivedRewardByStampTaskTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.CreateClusterRankingReceivedRewardByStampTaskResult> CreateClusterRankingReceivedRewardByStampTaskAsync(
                Request.CreateClusterRankingReceivedRewardByStampTaskRequest request
        )
		{
			var task = new CreateClusterRankingReceivedRewardByStampTaskTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class DescribeClusterRankingsTask : Gs2RestSessionTask<DescribeClusterRankingsRequest, DescribeClusterRankingsResult>
        {
            public DescribeClusterRankingsTask(IGs2Session session, RestSessionRequestFactory factory, DescribeClusterRankingsRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DescribeClusterRankingsRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "ranking2")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/ranking/cluster/{rankingName}/{clusterName}/user/me";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{rankingName}", !string.IsNullOrEmpty(request.RankingName) ? request.RankingName.ToString() : "null");
                url = url.Replace("{clusterName}", !string.IsNullOrEmpty(request.ClusterName) ? request.ClusterName.ToString() : "null");

                var sessionRequest = Factory.Get(url);
                if (request.ContextStack != null)
                {
                    sessionRequest.AddQueryString("contextStack", request.ContextStack);
                }
                if (request.Season != null) {
                    sessionRequest.AddQueryString("season", $"{request.Season}");
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
		public IEnumerator DescribeClusterRankings(
                Request.DescribeClusterRankingsRequest request,
                UnityAction<AsyncResult<Result.DescribeClusterRankingsResult>> callback
        )
		{
			var task = new DescribeClusterRankingsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.DescribeClusterRankingsResult>(task.Result, task.Error));
        }

		public IFuture<Result.DescribeClusterRankingsResult> DescribeClusterRankingsFuture(
                Request.DescribeClusterRankingsRequest request
        )
		{
			return new DescribeClusterRankingsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.DescribeClusterRankingsResult> DescribeClusterRankingsAsync(
                Request.DescribeClusterRankingsRequest request
        )
		{
            AsyncResult<Result.DescribeClusterRankingsResult> result = null;
			await DescribeClusterRankings(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
    #else
		public DescribeClusterRankingsTask DescribeClusterRankingsAsync(
                Request.DescribeClusterRankingsRequest request
        )
		{
			return new DescribeClusterRankingsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.DescribeClusterRankingsResult> DescribeClusterRankingsAsync(
                Request.DescribeClusterRankingsRequest request
        )
		{
			var task = new DescribeClusterRankingsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class DescribeClusterRankingsByUserIdTask : Gs2RestSessionTask<DescribeClusterRankingsByUserIdRequest, DescribeClusterRankingsByUserIdResult>
        {
            public DescribeClusterRankingsByUserIdTask(IGs2Session session, RestSessionRequestFactory factory, DescribeClusterRankingsByUserIdRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DescribeClusterRankingsByUserIdRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "ranking2")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/ranking/cluster/{rankingName}/{clusterName}/user/{userId}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{userId}", !string.IsNullOrEmpty(request.UserId) ? request.UserId.ToString() : "null");
                url = url.Replace("{rankingName}", !string.IsNullOrEmpty(request.RankingName) ? request.RankingName.ToString() : "null");
                url = url.Replace("{clusterName}", !string.IsNullOrEmpty(request.ClusterName) ? request.ClusterName.ToString() : "null");

                var sessionRequest = Factory.Get(url);
                if (request.ContextStack != null)
                {
                    sessionRequest.AddQueryString("contextStack", request.ContextStack);
                }
                if (request.Season != null) {
                    sessionRequest.AddQueryString("season", $"{request.Season}");
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
		public IEnumerator DescribeClusterRankingsByUserId(
                Request.DescribeClusterRankingsByUserIdRequest request,
                UnityAction<AsyncResult<Result.DescribeClusterRankingsByUserIdResult>> callback
        )
		{
			var task = new DescribeClusterRankingsByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.DescribeClusterRankingsByUserIdResult>(task.Result, task.Error));
        }

		public IFuture<Result.DescribeClusterRankingsByUserIdResult> DescribeClusterRankingsByUserIdFuture(
                Request.DescribeClusterRankingsByUserIdRequest request
        )
		{
			return new DescribeClusterRankingsByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.DescribeClusterRankingsByUserIdResult> DescribeClusterRankingsByUserIdAsync(
                Request.DescribeClusterRankingsByUserIdRequest request
        )
		{
            AsyncResult<Result.DescribeClusterRankingsByUserIdResult> result = null;
			await DescribeClusterRankingsByUserId(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
    #else
		public DescribeClusterRankingsByUserIdTask DescribeClusterRankingsByUserIdAsync(
                Request.DescribeClusterRankingsByUserIdRequest request
        )
		{
			return new DescribeClusterRankingsByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.DescribeClusterRankingsByUserIdResult> DescribeClusterRankingsByUserIdAsync(
                Request.DescribeClusterRankingsByUserIdRequest request
        )
		{
			var task = new DescribeClusterRankingsByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class GetClusterRankingTask : Gs2RestSessionTask<GetClusterRankingRequest, GetClusterRankingResult>
        {
            public GetClusterRankingTask(IGs2Session session, RestSessionRequestFactory factory, GetClusterRankingRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(GetClusterRankingRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "ranking2")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/ranking/cluster/{rankingName}/{clusterName}/user/me/rank";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{rankingName}", !string.IsNullOrEmpty(request.RankingName) ? request.RankingName.ToString() : "null");
                url = url.Replace("{clusterName}", !string.IsNullOrEmpty(request.ClusterName) ? request.ClusterName.ToString() : "null");

                var sessionRequest = Factory.Get(url);
                if (request.ContextStack != null)
                {
                    sessionRequest.AddQueryString("contextStack", request.ContextStack);
                }
                if (request.Season != null) {
                    sessionRequest.AddQueryString("season", $"{request.Season}");
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
		public IEnumerator GetClusterRanking(
                Request.GetClusterRankingRequest request,
                UnityAction<AsyncResult<Result.GetClusterRankingResult>> callback
        )
		{
			var task = new GetClusterRankingTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.GetClusterRankingResult>(task.Result, task.Error));
        }

		public IFuture<Result.GetClusterRankingResult> GetClusterRankingFuture(
                Request.GetClusterRankingRequest request
        )
		{
			return new GetClusterRankingTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.GetClusterRankingResult> GetClusterRankingAsync(
                Request.GetClusterRankingRequest request
        )
		{
            AsyncResult<Result.GetClusterRankingResult> result = null;
			await GetClusterRanking(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
    #else
		public GetClusterRankingTask GetClusterRankingAsync(
                Request.GetClusterRankingRequest request
        )
		{
			return new GetClusterRankingTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.GetClusterRankingResult> GetClusterRankingAsync(
                Request.GetClusterRankingRequest request
        )
		{
			var task = new GetClusterRankingTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class GetClusterRankingByUserIdTask : Gs2RestSessionTask<GetClusterRankingByUserIdRequest, GetClusterRankingByUserIdResult>
        {
            public GetClusterRankingByUserIdTask(IGs2Session session, RestSessionRequestFactory factory, GetClusterRankingByUserIdRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(GetClusterRankingByUserIdRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "ranking2")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/ranking/cluster/{rankingName}/{clusterName}/user/{userId}/rank";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{rankingName}", !string.IsNullOrEmpty(request.RankingName) ? request.RankingName.ToString() : "null");
                url = url.Replace("{clusterName}", !string.IsNullOrEmpty(request.ClusterName) ? request.ClusterName.ToString() : "null");
                url = url.Replace("{userId}", !string.IsNullOrEmpty(request.UserId) ? request.UserId.ToString() : "null");

                var sessionRequest = Factory.Get(url);
                if (request.ContextStack != null)
                {
                    sessionRequest.AddQueryString("contextStack", request.ContextStack);
                }
                if (request.Season != null) {
                    sessionRequest.AddQueryString("season", $"{request.Season}");
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
		public IEnumerator GetClusterRankingByUserId(
                Request.GetClusterRankingByUserIdRequest request,
                UnityAction<AsyncResult<Result.GetClusterRankingByUserIdResult>> callback
        )
		{
			var task = new GetClusterRankingByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.GetClusterRankingByUserIdResult>(task.Result, task.Error));
        }

		public IFuture<Result.GetClusterRankingByUserIdResult> GetClusterRankingByUserIdFuture(
                Request.GetClusterRankingByUserIdRequest request
        )
		{
			return new GetClusterRankingByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.GetClusterRankingByUserIdResult> GetClusterRankingByUserIdAsync(
                Request.GetClusterRankingByUserIdRequest request
        )
		{
            AsyncResult<Result.GetClusterRankingByUserIdResult> result = null;
			await GetClusterRankingByUserId(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
    #else
		public GetClusterRankingByUserIdTask GetClusterRankingByUserIdAsync(
                Request.GetClusterRankingByUserIdRequest request
        )
		{
			return new GetClusterRankingByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.GetClusterRankingByUserIdResult> GetClusterRankingByUserIdAsync(
                Request.GetClusterRankingByUserIdRequest request
        )
		{
			var task = new GetClusterRankingByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class DescribeSubscribeRankingModelsTask : Gs2RestSessionTask<DescribeSubscribeRankingModelsRequest, DescribeSubscribeRankingModelsResult>
        {
            public DescribeSubscribeRankingModelsTask(IGs2Session session, RestSessionRequestFactory factory, DescribeSubscribeRankingModelsRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DescribeSubscribeRankingModelsRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "ranking2")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/model/subscribe";

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
		public IEnumerator DescribeSubscribeRankingModels(
                Request.DescribeSubscribeRankingModelsRequest request,
                UnityAction<AsyncResult<Result.DescribeSubscribeRankingModelsResult>> callback
        )
		{
			var task = new DescribeSubscribeRankingModelsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.DescribeSubscribeRankingModelsResult>(task.Result, task.Error));
        }

		public IFuture<Result.DescribeSubscribeRankingModelsResult> DescribeSubscribeRankingModelsFuture(
                Request.DescribeSubscribeRankingModelsRequest request
        )
		{
			return new DescribeSubscribeRankingModelsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.DescribeSubscribeRankingModelsResult> DescribeSubscribeRankingModelsAsync(
                Request.DescribeSubscribeRankingModelsRequest request
        )
		{
            AsyncResult<Result.DescribeSubscribeRankingModelsResult> result = null;
			await DescribeSubscribeRankingModels(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
    #else
		public DescribeSubscribeRankingModelsTask DescribeSubscribeRankingModelsAsync(
                Request.DescribeSubscribeRankingModelsRequest request
        )
		{
			return new DescribeSubscribeRankingModelsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.DescribeSubscribeRankingModelsResult> DescribeSubscribeRankingModelsAsync(
                Request.DescribeSubscribeRankingModelsRequest request
        )
		{
			var task = new DescribeSubscribeRankingModelsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class GetSubscribeRankingModelTask : Gs2RestSessionTask<GetSubscribeRankingModelRequest, GetSubscribeRankingModelResult>
        {
            public GetSubscribeRankingModelTask(IGs2Session session, RestSessionRequestFactory factory, GetSubscribeRankingModelRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(GetSubscribeRankingModelRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "ranking2")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/model/subscribe/{rankingName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{rankingName}", !string.IsNullOrEmpty(request.RankingName) ? request.RankingName.ToString() : "null");

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
		public IEnumerator GetSubscribeRankingModel(
                Request.GetSubscribeRankingModelRequest request,
                UnityAction<AsyncResult<Result.GetSubscribeRankingModelResult>> callback
        )
		{
			var task = new GetSubscribeRankingModelTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.GetSubscribeRankingModelResult>(task.Result, task.Error));
        }

		public IFuture<Result.GetSubscribeRankingModelResult> GetSubscribeRankingModelFuture(
                Request.GetSubscribeRankingModelRequest request
        )
		{
			return new GetSubscribeRankingModelTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.GetSubscribeRankingModelResult> GetSubscribeRankingModelAsync(
                Request.GetSubscribeRankingModelRequest request
        )
		{
            AsyncResult<Result.GetSubscribeRankingModelResult> result = null;
			await GetSubscribeRankingModel(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
    #else
		public GetSubscribeRankingModelTask GetSubscribeRankingModelAsync(
                Request.GetSubscribeRankingModelRequest request
        )
		{
			return new GetSubscribeRankingModelTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.GetSubscribeRankingModelResult> GetSubscribeRankingModelAsync(
                Request.GetSubscribeRankingModelRequest request
        )
		{
			var task = new GetSubscribeRankingModelTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class DescribeSubscribeRankingModelMastersTask : Gs2RestSessionTask<DescribeSubscribeRankingModelMastersRequest, DescribeSubscribeRankingModelMastersResult>
        {
            public DescribeSubscribeRankingModelMastersTask(IGs2Session session, RestSessionRequestFactory factory, DescribeSubscribeRankingModelMastersRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DescribeSubscribeRankingModelMastersRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "ranking2")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/master/subscribe";

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
		public IEnumerator DescribeSubscribeRankingModelMasters(
                Request.DescribeSubscribeRankingModelMastersRequest request,
                UnityAction<AsyncResult<Result.DescribeSubscribeRankingModelMastersResult>> callback
        )
		{
			var task = new DescribeSubscribeRankingModelMastersTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.DescribeSubscribeRankingModelMastersResult>(task.Result, task.Error));
        }

		public IFuture<Result.DescribeSubscribeRankingModelMastersResult> DescribeSubscribeRankingModelMastersFuture(
                Request.DescribeSubscribeRankingModelMastersRequest request
        )
		{
			return new DescribeSubscribeRankingModelMastersTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.DescribeSubscribeRankingModelMastersResult> DescribeSubscribeRankingModelMastersAsync(
                Request.DescribeSubscribeRankingModelMastersRequest request
        )
		{
            AsyncResult<Result.DescribeSubscribeRankingModelMastersResult> result = null;
			await DescribeSubscribeRankingModelMasters(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
    #else
		public DescribeSubscribeRankingModelMastersTask DescribeSubscribeRankingModelMastersAsync(
                Request.DescribeSubscribeRankingModelMastersRequest request
        )
		{
			return new DescribeSubscribeRankingModelMastersTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.DescribeSubscribeRankingModelMastersResult> DescribeSubscribeRankingModelMastersAsync(
                Request.DescribeSubscribeRankingModelMastersRequest request
        )
		{
			var task = new DescribeSubscribeRankingModelMastersTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class CreateSubscribeRankingModelMasterTask : Gs2RestSessionTask<CreateSubscribeRankingModelMasterRequest, CreateSubscribeRankingModelMasterResult>
        {
            public CreateSubscribeRankingModelMasterTask(IGs2Session session, RestSessionRequestFactory factory, CreateSubscribeRankingModelMasterRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(CreateSubscribeRankingModelMasterRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "ranking2")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/master/subscribe";

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
                if (request.MinimumValue != null)
                {
                    jsonWriter.WritePropertyName("minimumValue");
                    jsonWriter.Write(request.MinimumValue.ToString());
                }
                if (request.MaximumValue != null)
                {
                    jsonWriter.WritePropertyName("maximumValue");
                    jsonWriter.Write(request.MaximumValue.ToString());
                }
                if (request.Sum != null)
                {
                    jsonWriter.WritePropertyName("sum");
                    jsonWriter.Write(request.Sum.ToString());
                }
                if (request.ScoreTtlDays != null)
                {
                    jsonWriter.WritePropertyName("scoreTtlDays");
                    jsonWriter.Write(request.ScoreTtlDays.ToString());
                }
                if (request.OrderDirection != null)
                {
                    jsonWriter.WritePropertyName("orderDirection");
                    jsonWriter.Write(request.OrderDirection);
                }
                if (request.EntryPeriodEventId != null)
                {
                    jsonWriter.WritePropertyName("entryPeriodEventId");
                    jsonWriter.Write(request.EntryPeriodEventId);
                }
                if (request.AccessPeriodEventId != null)
                {
                    jsonWriter.WritePropertyName("accessPeriodEventId");
                    jsonWriter.Write(request.AccessPeriodEventId);
                }
                if (request.ContextStack != null)
                {
                    jsonWriter.WritePropertyName("contextStack");
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
		public IEnumerator CreateSubscribeRankingModelMaster(
                Request.CreateSubscribeRankingModelMasterRequest request,
                UnityAction<AsyncResult<Result.CreateSubscribeRankingModelMasterResult>> callback
        )
		{
			var task = new CreateSubscribeRankingModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.CreateSubscribeRankingModelMasterResult>(task.Result, task.Error));
        }

		public IFuture<Result.CreateSubscribeRankingModelMasterResult> CreateSubscribeRankingModelMasterFuture(
                Request.CreateSubscribeRankingModelMasterRequest request
        )
		{
			return new CreateSubscribeRankingModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.CreateSubscribeRankingModelMasterResult> CreateSubscribeRankingModelMasterAsync(
                Request.CreateSubscribeRankingModelMasterRequest request
        )
		{
            AsyncResult<Result.CreateSubscribeRankingModelMasterResult> result = null;
			await CreateSubscribeRankingModelMaster(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
    #else
		public CreateSubscribeRankingModelMasterTask CreateSubscribeRankingModelMasterAsync(
                Request.CreateSubscribeRankingModelMasterRequest request
        )
		{
			return new CreateSubscribeRankingModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.CreateSubscribeRankingModelMasterResult> CreateSubscribeRankingModelMasterAsync(
                Request.CreateSubscribeRankingModelMasterRequest request
        )
		{
			var task = new CreateSubscribeRankingModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class GetSubscribeRankingModelMasterTask : Gs2RestSessionTask<GetSubscribeRankingModelMasterRequest, GetSubscribeRankingModelMasterResult>
        {
            public GetSubscribeRankingModelMasterTask(IGs2Session session, RestSessionRequestFactory factory, GetSubscribeRankingModelMasterRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(GetSubscribeRankingModelMasterRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "ranking2")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/master/subscribe/{rankingName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{rankingName}", !string.IsNullOrEmpty(request.RankingName) ? request.RankingName.ToString() : "null");

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
		public IEnumerator GetSubscribeRankingModelMaster(
                Request.GetSubscribeRankingModelMasterRequest request,
                UnityAction<AsyncResult<Result.GetSubscribeRankingModelMasterResult>> callback
        )
		{
			var task = new GetSubscribeRankingModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.GetSubscribeRankingModelMasterResult>(task.Result, task.Error));
        }

		public IFuture<Result.GetSubscribeRankingModelMasterResult> GetSubscribeRankingModelMasterFuture(
                Request.GetSubscribeRankingModelMasterRequest request
        )
		{
			return new GetSubscribeRankingModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.GetSubscribeRankingModelMasterResult> GetSubscribeRankingModelMasterAsync(
                Request.GetSubscribeRankingModelMasterRequest request
        )
		{
            AsyncResult<Result.GetSubscribeRankingModelMasterResult> result = null;
			await GetSubscribeRankingModelMaster(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
    #else
		public GetSubscribeRankingModelMasterTask GetSubscribeRankingModelMasterAsync(
                Request.GetSubscribeRankingModelMasterRequest request
        )
		{
			return new GetSubscribeRankingModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.GetSubscribeRankingModelMasterResult> GetSubscribeRankingModelMasterAsync(
                Request.GetSubscribeRankingModelMasterRequest request
        )
		{
			var task = new GetSubscribeRankingModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class UpdateSubscribeRankingModelMasterTask : Gs2RestSessionTask<UpdateSubscribeRankingModelMasterRequest, UpdateSubscribeRankingModelMasterResult>
        {
            public UpdateSubscribeRankingModelMasterTask(IGs2Session session, RestSessionRequestFactory factory, UpdateSubscribeRankingModelMasterRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(UpdateSubscribeRankingModelMasterRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "ranking2")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/master/subscribe/{rankingName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{rankingName}", !string.IsNullOrEmpty(request.RankingName) ? request.RankingName.ToString() : "null");

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
                if (request.MinimumValue != null)
                {
                    jsonWriter.WritePropertyName("minimumValue");
                    jsonWriter.Write(request.MinimumValue.ToString());
                }
                if (request.MaximumValue != null)
                {
                    jsonWriter.WritePropertyName("maximumValue");
                    jsonWriter.Write(request.MaximumValue.ToString());
                }
                if (request.Sum != null)
                {
                    jsonWriter.WritePropertyName("sum");
                    jsonWriter.Write(request.Sum.ToString());
                }
                if (request.ScoreTtlDays != null)
                {
                    jsonWriter.WritePropertyName("scoreTtlDays");
                    jsonWriter.Write(request.ScoreTtlDays.ToString());
                }
                if (request.OrderDirection != null)
                {
                    jsonWriter.WritePropertyName("orderDirection");
                    jsonWriter.Write(request.OrderDirection);
                }
                if (request.EntryPeriodEventId != null)
                {
                    jsonWriter.WritePropertyName("entryPeriodEventId");
                    jsonWriter.Write(request.EntryPeriodEventId);
                }
                if (request.AccessPeriodEventId != null)
                {
                    jsonWriter.WritePropertyName("accessPeriodEventId");
                    jsonWriter.Write(request.AccessPeriodEventId);
                }
                if (request.ContextStack != null)
                {
                    jsonWriter.WritePropertyName("contextStack");
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
		public IEnumerator UpdateSubscribeRankingModelMaster(
                Request.UpdateSubscribeRankingModelMasterRequest request,
                UnityAction<AsyncResult<Result.UpdateSubscribeRankingModelMasterResult>> callback
        )
		{
			var task = new UpdateSubscribeRankingModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.UpdateSubscribeRankingModelMasterResult>(task.Result, task.Error));
        }

		public IFuture<Result.UpdateSubscribeRankingModelMasterResult> UpdateSubscribeRankingModelMasterFuture(
                Request.UpdateSubscribeRankingModelMasterRequest request
        )
		{
			return new UpdateSubscribeRankingModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.UpdateSubscribeRankingModelMasterResult> UpdateSubscribeRankingModelMasterAsync(
                Request.UpdateSubscribeRankingModelMasterRequest request
        )
		{
            AsyncResult<Result.UpdateSubscribeRankingModelMasterResult> result = null;
			await UpdateSubscribeRankingModelMaster(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
    #else
		public UpdateSubscribeRankingModelMasterTask UpdateSubscribeRankingModelMasterAsync(
                Request.UpdateSubscribeRankingModelMasterRequest request
        )
		{
			return new UpdateSubscribeRankingModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.UpdateSubscribeRankingModelMasterResult> UpdateSubscribeRankingModelMasterAsync(
                Request.UpdateSubscribeRankingModelMasterRequest request
        )
		{
			var task = new UpdateSubscribeRankingModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class DeleteSubscribeRankingModelMasterTask : Gs2RestSessionTask<DeleteSubscribeRankingModelMasterRequest, DeleteSubscribeRankingModelMasterResult>
        {
            public DeleteSubscribeRankingModelMasterTask(IGs2Session session, RestSessionRequestFactory factory, DeleteSubscribeRankingModelMasterRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DeleteSubscribeRankingModelMasterRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "ranking2")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/master/subscribe/{rankingName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{rankingName}", !string.IsNullOrEmpty(request.RankingName) ? request.RankingName.ToString() : "null");

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
		public IEnumerator DeleteSubscribeRankingModelMaster(
                Request.DeleteSubscribeRankingModelMasterRequest request,
                UnityAction<AsyncResult<Result.DeleteSubscribeRankingModelMasterResult>> callback
        )
		{
			var task = new DeleteSubscribeRankingModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.DeleteSubscribeRankingModelMasterResult>(task.Result, task.Error));
        }

		public IFuture<Result.DeleteSubscribeRankingModelMasterResult> DeleteSubscribeRankingModelMasterFuture(
                Request.DeleteSubscribeRankingModelMasterRequest request
        )
		{
			return new DeleteSubscribeRankingModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.DeleteSubscribeRankingModelMasterResult> DeleteSubscribeRankingModelMasterAsync(
                Request.DeleteSubscribeRankingModelMasterRequest request
        )
		{
            AsyncResult<Result.DeleteSubscribeRankingModelMasterResult> result = null;
			await DeleteSubscribeRankingModelMaster(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
    #else
		public DeleteSubscribeRankingModelMasterTask DeleteSubscribeRankingModelMasterAsync(
                Request.DeleteSubscribeRankingModelMasterRequest request
        )
		{
			return new DeleteSubscribeRankingModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.DeleteSubscribeRankingModelMasterResult> DeleteSubscribeRankingModelMasterAsync(
                Request.DeleteSubscribeRankingModelMasterRequest request
        )
		{
			var task = new DeleteSubscribeRankingModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class DescribeSubscribesTask : Gs2RestSessionTask<DescribeSubscribesRequest, DescribeSubscribesResult>
        {
            public DescribeSubscribesTask(IGs2Session session, RestSessionRequestFactory factory, DescribeSubscribesRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DescribeSubscribesRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "ranking2")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/me/subscribe/score";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");

                var sessionRequest = Factory.Get(url);
                if (request.ContextStack != null)
                {
                    sessionRequest.AddQueryString("contextStack", request.ContextStack);
                }
                if (request.RankingName != null) {
                    sessionRequest.AddQueryString("rankingName", $"{request.RankingName}");
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
		public IEnumerator DescribeSubscribes(
                Request.DescribeSubscribesRequest request,
                UnityAction<AsyncResult<Result.DescribeSubscribesResult>> callback
        )
		{
			var task = new DescribeSubscribesTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.DescribeSubscribesResult>(task.Result, task.Error));
        }

		public IFuture<Result.DescribeSubscribesResult> DescribeSubscribesFuture(
                Request.DescribeSubscribesRequest request
        )
		{
			return new DescribeSubscribesTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.DescribeSubscribesResult> DescribeSubscribesAsync(
                Request.DescribeSubscribesRequest request
        )
		{
            AsyncResult<Result.DescribeSubscribesResult> result = null;
			await DescribeSubscribes(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
    #else
		public DescribeSubscribesTask DescribeSubscribesAsync(
                Request.DescribeSubscribesRequest request
        )
		{
			return new DescribeSubscribesTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.DescribeSubscribesResult> DescribeSubscribesAsync(
                Request.DescribeSubscribesRequest request
        )
		{
			var task = new DescribeSubscribesTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class DescribeSubscribesByUserIdTask : Gs2RestSessionTask<DescribeSubscribesByUserIdRequest, DescribeSubscribesByUserIdResult>
        {
            public DescribeSubscribesByUserIdTask(IGs2Session session, RestSessionRequestFactory factory, DescribeSubscribesByUserIdRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DescribeSubscribesByUserIdRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "ranking2")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/{userId}/subscribe/score";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{userId}", !string.IsNullOrEmpty(request.UserId) ? request.UserId.ToString() : "null");

                var sessionRequest = Factory.Get(url);
                if (request.ContextStack != null)
                {
                    sessionRequest.AddQueryString("contextStack", request.ContextStack);
                }
                if (request.RankingName != null) {
                    sessionRequest.AddQueryString("rankingName", $"{request.RankingName}");
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
		public IEnumerator DescribeSubscribesByUserId(
                Request.DescribeSubscribesByUserIdRequest request,
                UnityAction<AsyncResult<Result.DescribeSubscribesByUserIdResult>> callback
        )
		{
			var task = new DescribeSubscribesByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.DescribeSubscribesByUserIdResult>(task.Result, task.Error));
        }

		public IFuture<Result.DescribeSubscribesByUserIdResult> DescribeSubscribesByUserIdFuture(
                Request.DescribeSubscribesByUserIdRequest request
        )
		{
			return new DescribeSubscribesByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.DescribeSubscribesByUserIdResult> DescribeSubscribesByUserIdAsync(
                Request.DescribeSubscribesByUserIdRequest request
        )
		{
            AsyncResult<Result.DescribeSubscribesByUserIdResult> result = null;
			await DescribeSubscribesByUserId(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
    #else
		public DescribeSubscribesByUserIdTask DescribeSubscribesByUserIdAsync(
                Request.DescribeSubscribesByUserIdRequest request
        )
		{
			return new DescribeSubscribesByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.DescribeSubscribesByUserIdResult> DescribeSubscribesByUserIdAsync(
                Request.DescribeSubscribesByUserIdRequest request
        )
		{
			var task = new DescribeSubscribesByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class AddSubscribeTask : Gs2RestSessionTask<AddSubscribeRequest, AddSubscribeResult>
        {
            public AddSubscribeTask(IGs2Session session, RestSessionRequestFactory factory, AddSubscribeRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(AddSubscribeRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "ranking2")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/me/subscribe/{rankingName}/target/{targetUserId}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{rankingName}", !string.IsNullOrEmpty(request.RankingName) ? request.RankingName.ToString() : "null");
                url = url.Replace("{targetUserId}", !string.IsNullOrEmpty(request.TargetUserId) ? request.TargetUserId.ToString() : "null");

                var sessionRequest = Factory.Put(url);

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
		public IEnumerator AddSubscribe(
                Request.AddSubscribeRequest request,
                UnityAction<AsyncResult<Result.AddSubscribeResult>> callback
        )
		{
			var task = new AddSubscribeTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.AddSubscribeResult>(task.Result, task.Error));
        }

		public IFuture<Result.AddSubscribeResult> AddSubscribeFuture(
                Request.AddSubscribeRequest request
        )
		{
			return new AddSubscribeTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.AddSubscribeResult> AddSubscribeAsync(
                Request.AddSubscribeRequest request
        )
		{
            AsyncResult<Result.AddSubscribeResult> result = null;
			await AddSubscribe(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
    #else
		public AddSubscribeTask AddSubscribeAsync(
                Request.AddSubscribeRequest request
        )
		{
			return new AddSubscribeTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.AddSubscribeResult> AddSubscribeAsync(
                Request.AddSubscribeRequest request
        )
		{
			var task = new AddSubscribeTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class AddSubscribeByUserIdTask : Gs2RestSessionTask<AddSubscribeByUserIdRequest, AddSubscribeByUserIdResult>
        {
            public AddSubscribeByUserIdTask(IGs2Session session, RestSessionRequestFactory factory, AddSubscribeByUserIdRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(AddSubscribeByUserIdRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "ranking2")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/{userId}/subscribe/{rankingName}/target/{targetUserId}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{rankingName}", !string.IsNullOrEmpty(request.RankingName) ? request.RankingName.ToString() : "null");
                url = url.Replace("{userId}", !string.IsNullOrEmpty(request.UserId) ? request.UserId.ToString() : "null");
                url = url.Replace("{targetUserId}", !string.IsNullOrEmpty(request.TargetUserId) ? request.TargetUserId.ToString() : "null");

                var sessionRequest = Factory.Put(url);

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
		public IEnumerator AddSubscribeByUserId(
                Request.AddSubscribeByUserIdRequest request,
                UnityAction<AsyncResult<Result.AddSubscribeByUserIdResult>> callback
        )
		{
			var task = new AddSubscribeByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.AddSubscribeByUserIdResult>(task.Result, task.Error));
        }

		public IFuture<Result.AddSubscribeByUserIdResult> AddSubscribeByUserIdFuture(
                Request.AddSubscribeByUserIdRequest request
        )
		{
			return new AddSubscribeByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.AddSubscribeByUserIdResult> AddSubscribeByUserIdAsync(
                Request.AddSubscribeByUserIdRequest request
        )
		{
            AsyncResult<Result.AddSubscribeByUserIdResult> result = null;
			await AddSubscribeByUserId(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
    #else
		public AddSubscribeByUserIdTask AddSubscribeByUserIdAsync(
                Request.AddSubscribeByUserIdRequest request
        )
		{
			return new AddSubscribeByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.AddSubscribeByUserIdResult> AddSubscribeByUserIdAsync(
                Request.AddSubscribeByUserIdRequest request
        )
		{
			var task = new AddSubscribeByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class DescribeSubscribeRankingScoresTask : Gs2RestSessionTask<DescribeSubscribeRankingScoresRequest, DescribeSubscribeRankingScoresResult>
        {
            public DescribeSubscribeRankingScoresTask(IGs2Session session, RestSessionRequestFactory factory, DescribeSubscribeRankingScoresRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DescribeSubscribeRankingScoresRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "ranking2")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/me/score/subscribe";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");

                var sessionRequest = Factory.Get(url);
                if (request.ContextStack != null)
                {
                    sessionRequest.AddQueryString("contextStack", request.ContextStack);
                }
                if (request.RankingName != null) {
                    sessionRequest.AddQueryString("rankingName", $"{request.RankingName}");
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
		public IEnumerator DescribeSubscribeRankingScores(
                Request.DescribeSubscribeRankingScoresRequest request,
                UnityAction<AsyncResult<Result.DescribeSubscribeRankingScoresResult>> callback
        )
		{
			var task = new DescribeSubscribeRankingScoresTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.DescribeSubscribeRankingScoresResult>(task.Result, task.Error));
        }

		public IFuture<Result.DescribeSubscribeRankingScoresResult> DescribeSubscribeRankingScoresFuture(
                Request.DescribeSubscribeRankingScoresRequest request
        )
		{
			return new DescribeSubscribeRankingScoresTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.DescribeSubscribeRankingScoresResult> DescribeSubscribeRankingScoresAsync(
                Request.DescribeSubscribeRankingScoresRequest request
        )
		{
            AsyncResult<Result.DescribeSubscribeRankingScoresResult> result = null;
			await DescribeSubscribeRankingScores(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
    #else
		public DescribeSubscribeRankingScoresTask DescribeSubscribeRankingScoresAsync(
                Request.DescribeSubscribeRankingScoresRequest request
        )
		{
			return new DescribeSubscribeRankingScoresTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.DescribeSubscribeRankingScoresResult> DescribeSubscribeRankingScoresAsync(
                Request.DescribeSubscribeRankingScoresRequest request
        )
		{
			var task = new DescribeSubscribeRankingScoresTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class DescribeSubscribeRankingScoresByUserIdTask : Gs2RestSessionTask<DescribeSubscribeRankingScoresByUserIdRequest, DescribeSubscribeRankingScoresByUserIdResult>
        {
            public DescribeSubscribeRankingScoresByUserIdTask(IGs2Session session, RestSessionRequestFactory factory, DescribeSubscribeRankingScoresByUserIdRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DescribeSubscribeRankingScoresByUserIdRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "ranking2")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/{userId}/score/subscribe";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{userId}", !string.IsNullOrEmpty(request.UserId) ? request.UserId.ToString() : "null");

                var sessionRequest = Factory.Get(url);
                if (request.ContextStack != null)
                {
                    sessionRequest.AddQueryString("contextStack", request.ContextStack);
                }
                if (request.RankingName != null) {
                    sessionRequest.AddQueryString("rankingName", $"{request.RankingName}");
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
		public IEnumerator DescribeSubscribeRankingScoresByUserId(
                Request.DescribeSubscribeRankingScoresByUserIdRequest request,
                UnityAction<AsyncResult<Result.DescribeSubscribeRankingScoresByUserIdResult>> callback
        )
		{
			var task = new DescribeSubscribeRankingScoresByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.DescribeSubscribeRankingScoresByUserIdResult>(task.Result, task.Error));
        }

		public IFuture<Result.DescribeSubscribeRankingScoresByUserIdResult> DescribeSubscribeRankingScoresByUserIdFuture(
                Request.DescribeSubscribeRankingScoresByUserIdRequest request
        )
		{
			return new DescribeSubscribeRankingScoresByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.DescribeSubscribeRankingScoresByUserIdResult> DescribeSubscribeRankingScoresByUserIdAsync(
                Request.DescribeSubscribeRankingScoresByUserIdRequest request
        )
		{
            AsyncResult<Result.DescribeSubscribeRankingScoresByUserIdResult> result = null;
			await DescribeSubscribeRankingScoresByUserId(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
    #else
		public DescribeSubscribeRankingScoresByUserIdTask DescribeSubscribeRankingScoresByUserIdAsync(
                Request.DescribeSubscribeRankingScoresByUserIdRequest request
        )
		{
			return new DescribeSubscribeRankingScoresByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.DescribeSubscribeRankingScoresByUserIdResult> DescribeSubscribeRankingScoresByUserIdAsync(
                Request.DescribeSubscribeRankingScoresByUserIdRequest request
        )
		{
			var task = new DescribeSubscribeRankingScoresByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class PutSubscribeRankingScoreTask : Gs2RestSessionTask<PutSubscribeRankingScoreRequest, PutSubscribeRankingScoreResult>
        {
            public PutSubscribeRankingScoreTask(IGs2Session session, RestSessionRequestFactory factory, PutSubscribeRankingScoreRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(PutSubscribeRankingScoreRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "ranking2")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/me/score/subscribe/{rankingName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{rankingName}", !string.IsNullOrEmpty(request.RankingName) ? request.RankingName.ToString() : "null");

                var sessionRequest = Factory.Put(url);

                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);
                jsonWriter.WriteObjectStart();
                if (request.Score != null)
                {
                    jsonWriter.WritePropertyName("score");
                    jsonWriter.Write(request.Score.ToString());
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
		public IEnumerator PutSubscribeRankingScore(
                Request.PutSubscribeRankingScoreRequest request,
                UnityAction<AsyncResult<Result.PutSubscribeRankingScoreResult>> callback
        )
		{
			var task = new PutSubscribeRankingScoreTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.PutSubscribeRankingScoreResult>(task.Result, task.Error));
        }

		public IFuture<Result.PutSubscribeRankingScoreResult> PutSubscribeRankingScoreFuture(
                Request.PutSubscribeRankingScoreRequest request
        )
		{
			return new PutSubscribeRankingScoreTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.PutSubscribeRankingScoreResult> PutSubscribeRankingScoreAsync(
                Request.PutSubscribeRankingScoreRequest request
        )
		{
            AsyncResult<Result.PutSubscribeRankingScoreResult> result = null;
			await PutSubscribeRankingScore(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
    #else
		public PutSubscribeRankingScoreTask PutSubscribeRankingScoreAsync(
                Request.PutSubscribeRankingScoreRequest request
        )
		{
			return new PutSubscribeRankingScoreTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.PutSubscribeRankingScoreResult> PutSubscribeRankingScoreAsync(
                Request.PutSubscribeRankingScoreRequest request
        )
		{
			var task = new PutSubscribeRankingScoreTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class PutSubscribeRankingScoreByUserIdTask : Gs2RestSessionTask<PutSubscribeRankingScoreByUserIdRequest, PutSubscribeRankingScoreByUserIdResult>
        {
            public PutSubscribeRankingScoreByUserIdTask(IGs2Session session, RestSessionRequestFactory factory, PutSubscribeRankingScoreByUserIdRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(PutSubscribeRankingScoreByUserIdRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "ranking2")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/{userId}/score/subscribe/{rankingName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{rankingName}", !string.IsNullOrEmpty(request.RankingName) ? request.RankingName.ToString() : "null");
                url = url.Replace("{userId}", !string.IsNullOrEmpty(request.UserId) ? request.UserId.ToString() : "null");

                var sessionRequest = Factory.Put(url);

                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);
                jsonWriter.WriteObjectStart();
                if (request.Score != null)
                {
                    jsonWriter.WritePropertyName("score");
                    jsonWriter.Write(request.Score.ToString());
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
		public IEnumerator PutSubscribeRankingScoreByUserId(
                Request.PutSubscribeRankingScoreByUserIdRequest request,
                UnityAction<AsyncResult<Result.PutSubscribeRankingScoreByUserIdResult>> callback
        )
		{
			var task = new PutSubscribeRankingScoreByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.PutSubscribeRankingScoreByUserIdResult>(task.Result, task.Error));
        }

		public IFuture<Result.PutSubscribeRankingScoreByUserIdResult> PutSubscribeRankingScoreByUserIdFuture(
                Request.PutSubscribeRankingScoreByUserIdRequest request
        )
		{
			return new PutSubscribeRankingScoreByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.PutSubscribeRankingScoreByUserIdResult> PutSubscribeRankingScoreByUserIdAsync(
                Request.PutSubscribeRankingScoreByUserIdRequest request
        )
		{
            AsyncResult<Result.PutSubscribeRankingScoreByUserIdResult> result = null;
			await PutSubscribeRankingScoreByUserId(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
    #else
		public PutSubscribeRankingScoreByUserIdTask PutSubscribeRankingScoreByUserIdAsync(
                Request.PutSubscribeRankingScoreByUserIdRequest request
        )
		{
			return new PutSubscribeRankingScoreByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.PutSubscribeRankingScoreByUserIdResult> PutSubscribeRankingScoreByUserIdAsync(
                Request.PutSubscribeRankingScoreByUserIdRequest request
        )
		{
			var task = new PutSubscribeRankingScoreByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class GetSubscribeRankingScoreTask : Gs2RestSessionTask<GetSubscribeRankingScoreRequest, GetSubscribeRankingScoreResult>
        {
            public GetSubscribeRankingScoreTask(IGs2Session session, RestSessionRequestFactory factory, GetSubscribeRankingScoreRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(GetSubscribeRankingScoreRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "ranking2")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/me/score/subscribe/{rankingName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{rankingName}", !string.IsNullOrEmpty(request.RankingName) ? request.RankingName.ToString() : "null");

                var sessionRequest = Factory.Get(url);
                if (request.ContextStack != null)
                {
                    sessionRequest.AddQueryString("contextStack", request.ContextStack);
                }
                if (request.Season != null) {
                    sessionRequest.AddQueryString("season", $"{request.Season}");
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
		public IEnumerator GetSubscribeRankingScore(
                Request.GetSubscribeRankingScoreRequest request,
                UnityAction<AsyncResult<Result.GetSubscribeRankingScoreResult>> callback
        )
		{
			var task = new GetSubscribeRankingScoreTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.GetSubscribeRankingScoreResult>(task.Result, task.Error));
        }

		public IFuture<Result.GetSubscribeRankingScoreResult> GetSubscribeRankingScoreFuture(
                Request.GetSubscribeRankingScoreRequest request
        )
		{
			return new GetSubscribeRankingScoreTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.GetSubscribeRankingScoreResult> GetSubscribeRankingScoreAsync(
                Request.GetSubscribeRankingScoreRequest request
        )
		{
            AsyncResult<Result.GetSubscribeRankingScoreResult> result = null;
			await GetSubscribeRankingScore(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
    #else
		public GetSubscribeRankingScoreTask GetSubscribeRankingScoreAsync(
                Request.GetSubscribeRankingScoreRequest request
        )
		{
			return new GetSubscribeRankingScoreTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.GetSubscribeRankingScoreResult> GetSubscribeRankingScoreAsync(
                Request.GetSubscribeRankingScoreRequest request
        )
		{
			var task = new GetSubscribeRankingScoreTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class GetSubscribeRankingScoreByUserIdTask : Gs2RestSessionTask<GetSubscribeRankingScoreByUserIdRequest, GetSubscribeRankingScoreByUserIdResult>
        {
            public GetSubscribeRankingScoreByUserIdTask(IGs2Session session, RestSessionRequestFactory factory, GetSubscribeRankingScoreByUserIdRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(GetSubscribeRankingScoreByUserIdRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "ranking2")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/{userId}/score/subscribe/{rankingName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{rankingName}", !string.IsNullOrEmpty(request.RankingName) ? request.RankingName.ToString() : "null");
                url = url.Replace("{userId}", !string.IsNullOrEmpty(request.UserId) ? request.UserId.ToString() : "null");

                var sessionRequest = Factory.Get(url);
                if (request.ContextStack != null)
                {
                    sessionRequest.AddQueryString("contextStack", request.ContextStack);
                }
                if (request.Season != null) {
                    sessionRequest.AddQueryString("season", $"{request.Season}");
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
		public IEnumerator GetSubscribeRankingScoreByUserId(
                Request.GetSubscribeRankingScoreByUserIdRequest request,
                UnityAction<AsyncResult<Result.GetSubscribeRankingScoreByUserIdResult>> callback
        )
		{
			var task = new GetSubscribeRankingScoreByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.GetSubscribeRankingScoreByUserIdResult>(task.Result, task.Error));
        }

		public IFuture<Result.GetSubscribeRankingScoreByUserIdResult> GetSubscribeRankingScoreByUserIdFuture(
                Request.GetSubscribeRankingScoreByUserIdRequest request
        )
		{
			return new GetSubscribeRankingScoreByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.GetSubscribeRankingScoreByUserIdResult> GetSubscribeRankingScoreByUserIdAsync(
                Request.GetSubscribeRankingScoreByUserIdRequest request
        )
		{
            AsyncResult<Result.GetSubscribeRankingScoreByUserIdResult> result = null;
			await GetSubscribeRankingScoreByUserId(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
    #else
		public GetSubscribeRankingScoreByUserIdTask GetSubscribeRankingScoreByUserIdAsync(
                Request.GetSubscribeRankingScoreByUserIdRequest request
        )
		{
			return new GetSubscribeRankingScoreByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.GetSubscribeRankingScoreByUserIdResult> GetSubscribeRankingScoreByUserIdAsync(
                Request.GetSubscribeRankingScoreByUserIdRequest request
        )
		{
			var task = new GetSubscribeRankingScoreByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class DeleteSubscribeRankingScoreByUserIdTask : Gs2RestSessionTask<DeleteSubscribeRankingScoreByUserIdRequest, DeleteSubscribeRankingScoreByUserIdResult>
        {
            public DeleteSubscribeRankingScoreByUserIdTask(IGs2Session session, RestSessionRequestFactory factory, DeleteSubscribeRankingScoreByUserIdRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DeleteSubscribeRankingScoreByUserIdRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "ranking2")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/{userId}/score/subscribe/{rankingName}/{season}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{rankingName}", !string.IsNullOrEmpty(request.RankingName) ? request.RankingName.ToString() : "null");
                url = url.Replace("{userId}", !string.IsNullOrEmpty(request.UserId) ? request.UserId.ToString() : "null");

                var sessionRequest = Factory.Delete(url);
                if (request.ContextStack != null)
                {
                    sessionRequest.AddQueryString("contextStack", request.ContextStack);
                }
                if (request.Season != null) {
                    sessionRequest.AddQueryString("season", $"{request.Season}");
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
		public IEnumerator DeleteSubscribeRankingScoreByUserId(
                Request.DeleteSubscribeRankingScoreByUserIdRequest request,
                UnityAction<AsyncResult<Result.DeleteSubscribeRankingScoreByUserIdResult>> callback
        )
		{
			var task = new DeleteSubscribeRankingScoreByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.DeleteSubscribeRankingScoreByUserIdResult>(task.Result, task.Error));
        }

		public IFuture<Result.DeleteSubscribeRankingScoreByUserIdResult> DeleteSubscribeRankingScoreByUserIdFuture(
                Request.DeleteSubscribeRankingScoreByUserIdRequest request
        )
		{
			return new DeleteSubscribeRankingScoreByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.DeleteSubscribeRankingScoreByUserIdResult> DeleteSubscribeRankingScoreByUserIdAsync(
                Request.DeleteSubscribeRankingScoreByUserIdRequest request
        )
		{
            AsyncResult<Result.DeleteSubscribeRankingScoreByUserIdResult> result = null;
			await DeleteSubscribeRankingScoreByUserId(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
    #else
		public DeleteSubscribeRankingScoreByUserIdTask DeleteSubscribeRankingScoreByUserIdAsync(
                Request.DeleteSubscribeRankingScoreByUserIdRequest request
        )
		{
			return new DeleteSubscribeRankingScoreByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.DeleteSubscribeRankingScoreByUserIdResult> DeleteSubscribeRankingScoreByUserIdAsync(
                Request.DeleteSubscribeRankingScoreByUserIdRequest request
        )
		{
			var task = new DeleteSubscribeRankingScoreByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class VerifySubscribeRankingScoreTask : Gs2RestSessionTask<VerifySubscribeRankingScoreRequest, VerifySubscribeRankingScoreResult>
        {
            public VerifySubscribeRankingScoreTask(IGs2Session session, RestSessionRequestFactory factory, VerifySubscribeRankingScoreRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(VerifySubscribeRankingScoreRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "ranking2")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/{userId}/score/subscribe/{rankingName}/verify/{verifyType}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{rankingName}", !string.IsNullOrEmpty(request.RankingName) ? request.RankingName.ToString() : "null");
                url = url.Replace("{verifyType}", !string.IsNullOrEmpty(request.VerifyType) ? request.VerifyType.ToString() : "null");

                var sessionRequest = Factory.Post(url);

                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);
                jsonWriter.WriteObjectStart();
                if (request.Season != null)
                {
                    jsonWriter.WritePropertyName("season");
                    jsonWriter.Write(request.Season.ToString());
                }
                if (request.Score != null)
                {
                    jsonWriter.WritePropertyName("score");
                    jsonWriter.Write(request.Score.ToString());
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
		public IEnumerator VerifySubscribeRankingScore(
                Request.VerifySubscribeRankingScoreRequest request,
                UnityAction<AsyncResult<Result.VerifySubscribeRankingScoreResult>> callback
        )
		{
			var task = new VerifySubscribeRankingScoreTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.VerifySubscribeRankingScoreResult>(task.Result, task.Error));
        }

		public IFuture<Result.VerifySubscribeRankingScoreResult> VerifySubscribeRankingScoreFuture(
                Request.VerifySubscribeRankingScoreRequest request
        )
		{
			return new VerifySubscribeRankingScoreTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.VerifySubscribeRankingScoreResult> VerifySubscribeRankingScoreAsync(
                Request.VerifySubscribeRankingScoreRequest request
        )
		{
            AsyncResult<Result.VerifySubscribeRankingScoreResult> result = null;
			await VerifySubscribeRankingScore(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
    #else
		public VerifySubscribeRankingScoreTask VerifySubscribeRankingScoreAsync(
                Request.VerifySubscribeRankingScoreRequest request
        )
		{
			return new VerifySubscribeRankingScoreTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.VerifySubscribeRankingScoreResult> VerifySubscribeRankingScoreAsync(
                Request.VerifySubscribeRankingScoreRequest request
        )
		{
			var task = new VerifySubscribeRankingScoreTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class VerifySubscribeRankingScoreByUserIdTask : Gs2RestSessionTask<VerifySubscribeRankingScoreByUserIdRequest, VerifySubscribeRankingScoreByUserIdResult>
        {
            public VerifySubscribeRankingScoreByUserIdTask(IGs2Session session, RestSessionRequestFactory factory, VerifySubscribeRankingScoreByUserIdRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(VerifySubscribeRankingScoreByUserIdRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "ranking2")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/{userId}/score/subscribe/{rankingName}/verify/{verifyType}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{userId}", !string.IsNullOrEmpty(request.UserId) ? request.UserId.ToString() : "null");
                url = url.Replace("{rankingName}", !string.IsNullOrEmpty(request.RankingName) ? request.RankingName.ToString() : "null");
                url = url.Replace("{verifyType}", !string.IsNullOrEmpty(request.VerifyType) ? request.VerifyType.ToString() : "null");

                var sessionRequest = Factory.Post(url);

                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);
                jsonWriter.WriteObjectStart();
                if (request.Season != null)
                {
                    jsonWriter.WritePropertyName("season");
                    jsonWriter.Write(request.Season.ToString());
                }
                if (request.Score != null)
                {
                    jsonWriter.WritePropertyName("score");
                    jsonWriter.Write(request.Score.ToString());
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
		public IEnumerator VerifySubscribeRankingScoreByUserId(
                Request.VerifySubscribeRankingScoreByUserIdRequest request,
                UnityAction<AsyncResult<Result.VerifySubscribeRankingScoreByUserIdResult>> callback
        )
		{
			var task = new VerifySubscribeRankingScoreByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.VerifySubscribeRankingScoreByUserIdResult>(task.Result, task.Error));
        }

		public IFuture<Result.VerifySubscribeRankingScoreByUserIdResult> VerifySubscribeRankingScoreByUserIdFuture(
                Request.VerifySubscribeRankingScoreByUserIdRequest request
        )
		{
			return new VerifySubscribeRankingScoreByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.VerifySubscribeRankingScoreByUserIdResult> VerifySubscribeRankingScoreByUserIdAsync(
                Request.VerifySubscribeRankingScoreByUserIdRequest request
        )
		{
            AsyncResult<Result.VerifySubscribeRankingScoreByUserIdResult> result = null;
			await VerifySubscribeRankingScoreByUserId(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
    #else
		public VerifySubscribeRankingScoreByUserIdTask VerifySubscribeRankingScoreByUserIdAsync(
                Request.VerifySubscribeRankingScoreByUserIdRequest request
        )
		{
			return new VerifySubscribeRankingScoreByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.VerifySubscribeRankingScoreByUserIdResult> VerifySubscribeRankingScoreByUserIdAsync(
                Request.VerifySubscribeRankingScoreByUserIdRequest request
        )
		{
			var task = new VerifySubscribeRankingScoreByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class VerifySubscribeRankingScoreByStampTaskTask : Gs2RestSessionTask<VerifySubscribeRankingScoreByStampTaskRequest, VerifySubscribeRankingScoreByStampTaskResult>
        {
            public VerifySubscribeRankingScoreByStampTaskTask(IGs2Session session, RestSessionRequestFactory factory, VerifySubscribeRankingScoreByStampTaskRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(VerifySubscribeRankingScoreByStampTaskRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "ranking2")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/stamp/subscribe/score/verify";

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
		public IEnumerator VerifySubscribeRankingScoreByStampTask(
                Request.VerifySubscribeRankingScoreByStampTaskRequest request,
                UnityAction<AsyncResult<Result.VerifySubscribeRankingScoreByStampTaskResult>> callback
        )
		{
			var task = new VerifySubscribeRankingScoreByStampTaskTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.VerifySubscribeRankingScoreByStampTaskResult>(task.Result, task.Error));
        }

		public IFuture<Result.VerifySubscribeRankingScoreByStampTaskResult> VerifySubscribeRankingScoreByStampTaskFuture(
                Request.VerifySubscribeRankingScoreByStampTaskRequest request
        )
		{
			return new VerifySubscribeRankingScoreByStampTaskTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.VerifySubscribeRankingScoreByStampTaskResult> VerifySubscribeRankingScoreByStampTaskAsync(
                Request.VerifySubscribeRankingScoreByStampTaskRequest request
        )
		{
            AsyncResult<Result.VerifySubscribeRankingScoreByStampTaskResult> result = null;
			await VerifySubscribeRankingScoreByStampTask(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
    #else
		public VerifySubscribeRankingScoreByStampTaskTask VerifySubscribeRankingScoreByStampTaskAsync(
                Request.VerifySubscribeRankingScoreByStampTaskRequest request
        )
		{
			return new VerifySubscribeRankingScoreByStampTaskTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.VerifySubscribeRankingScoreByStampTaskResult> VerifySubscribeRankingScoreByStampTaskAsync(
                Request.VerifySubscribeRankingScoreByStampTaskRequest request
        )
		{
			var task = new VerifySubscribeRankingScoreByStampTaskTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class DescribeSubscribeRankingsTask : Gs2RestSessionTask<DescribeSubscribeRankingsRequest, DescribeSubscribeRankingsResult>
        {
            public DescribeSubscribeRankingsTask(IGs2Session session, RestSessionRequestFactory factory, DescribeSubscribeRankingsRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DescribeSubscribeRankingsRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "ranking2")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/ranking/subscribe/{rankingName}/user/me";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{rankingName}", !string.IsNullOrEmpty(request.RankingName) ? request.RankingName.ToString() : "null");

                var sessionRequest = Factory.Get(url);
                if (request.ContextStack != null)
                {
                    sessionRequest.AddQueryString("contextStack", request.ContextStack);
                }
                if (request.Season != null) {
                    sessionRequest.AddQueryString("season", $"{request.Season}");
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
		public IEnumerator DescribeSubscribeRankings(
                Request.DescribeSubscribeRankingsRequest request,
                UnityAction<AsyncResult<Result.DescribeSubscribeRankingsResult>> callback
        )
		{
			var task = new DescribeSubscribeRankingsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.DescribeSubscribeRankingsResult>(task.Result, task.Error));
        }

		public IFuture<Result.DescribeSubscribeRankingsResult> DescribeSubscribeRankingsFuture(
                Request.DescribeSubscribeRankingsRequest request
        )
		{
			return new DescribeSubscribeRankingsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.DescribeSubscribeRankingsResult> DescribeSubscribeRankingsAsync(
                Request.DescribeSubscribeRankingsRequest request
        )
		{
            AsyncResult<Result.DescribeSubscribeRankingsResult> result = null;
			await DescribeSubscribeRankings(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
    #else
		public DescribeSubscribeRankingsTask DescribeSubscribeRankingsAsync(
                Request.DescribeSubscribeRankingsRequest request
        )
		{
			return new DescribeSubscribeRankingsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.DescribeSubscribeRankingsResult> DescribeSubscribeRankingsAsync(
                Request.DescribeSubscribeRankingsRequest request
        )
		{
			var task = new DescribeSubscribeRankingsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class DescribeSubscribeRankingsByUserIdTask : Gs2RestSessionTask<DescribeSubscribeRankingsByUserIdRequest, DescribeSubscribeRankingsByUserIdResult>
        {
            public DescribeSubscribeRankingsByUserIdTask(IGs2Session session, RestSessionRequestFactory factory, DescribeSubscribeRankingsByUserIdRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DescribeSubscribeRankingsByUserIdRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "ranking2")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/ranking/subscribe/{rankingName}/user/{userId}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{userId}", !string.IsNullOrEmpty(request.UserId) ? request.UserId.ToString() : "null");
                url = url.Replace("{rankingName}", !string.IsNullOrEmpty(request.RankingName) ? request.RankingName.ToString() : "null");

                var sessionRequest = Factory.Get(url);
                if (request.ContextStack != null)
                {
                    sessionRequest.AddQueryString("contextStack", request.ContextStack);
                }
                if (request.Season != null) {
                    sessionRequest.AddQueryString("season", $"{request.Season}");
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
		public IEnumerator DescribeSubscribeRankingsByUserId(
                Request.DescribeSubscribeRankingsByUserIdRequest request,
                UnityAction<AsyncResult<Result.DescribeSubscribeRankingsByUserIdResult>> callback
        )
		{
			var task = new DescribeSubscribeRankingsByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.DescribeSubscribeRankingsByUserIdResult>(task.Result, task.Error));
        }

		public IFuture<Result.DescribeSubscribeRankingsByUserIdResult> DescribeSubscribeRankingsByUserIdFuture(
                Request.DescribeSubscribeRankingsByUserIdRequest request
        )
		{
			return new DescribeSubscribeRankingsByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.DescribeSubscribeRankingsByUserIdResult> DescribeSubscribeRankingsByUserIdAsync(
                Request.DescribeSubscribeRankingsByUserIdRequest request
        )
		{
            AsyncResult<Result.DescribeSubscribeRankingsByUserIdResult> result = null;
			await DescribeSubscribeRankingsByUserId(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
    #else
		public DescribeSubscribeRankingsByUserIdTask DescribeSubscribeRankingsByUserIdAsync(
                Request.DescribeSubscribeRankingsByUserIdRequest request
        )
		{
			return new DescribeSubscribeRankingsByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.DescribeSubscribeRankingsByUserIdResult> DescribeSubscribeRankingsByUserIdAsync(
                Request.DescribeSubscribeRankingsByUserIdRequest request
        )
		{
			var task = new DescribeSubscribeRankingsByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class GetSubscribeRankingTask : Gs2RestSessionTask<GetSubscribeRankingRequest, GetSubscribeRankingResult>
        {
            public GetSubscribeRankingTask(IGs2Session session, RestSessionRequestFactory factory, GetSubscribeRankingRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(GetSubscribeRankingRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "ranking2")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/ranking/subscribe/{rankingName}/user/me/rank";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{rankingName}", !string.IsNullOrEmpty(request.RankingName) ? request.RankingName.ToString() : "null");

                var sessionRequest = Factory.Get(url);
                if (request.ContextStack != null)
                {
                    sessionRequest.AddQueryString("contextStack", request.ContextStack);
                }
                if (request.Season != null) {
                    sessionRequest.AddQueryString("season", $"{request.Season}");
                }
                if (request.ScorerUserId != null) {
                    sessionRequest.AddQueryString("scorerUserId", $"{request.ScorerUserId}");
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
		public IEnumerator GetSubscribeRanking(
                Request.GetSubscribeRankingRequest request,
                UnityAction<AsyncResult<Result.GetSubscribeRankingResult>> callback
        )
		{
			var task = new GetSubscribeRankingTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.GetSubscribeRankingResult>(task.Result, task.Error));
        }

		public IFuture<Result.GetSubscribeRankingResult> GetSubscribeRankingFuture(
                Request.GetSubscribeRankingRequest request
        )
		{
			return new GetSubscribeRankingTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.GetSubscribeRankingResult> GetSubscribeRankingAsync(
                Request.GetSubscribeRankingRequest request
        )
		{
            AsyncResult<Result.GetSubscribeRankingResult> result = null;
			await GetSubscribeRanking(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
    #else
		public GetSubscribeRankingTask GetSubscribeRankingAsync(
                Request.GetSubscribeRankingRequest request
        )
		{
			return new GetSubscribeRankingTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.GetSubscribeRankingResult> GetSubscribeRankingAsync(
                Request.GetSubscribeRankingRequest request
        )
		{
			var task = new GetSubscribeRankingTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class GetSubscribeRankingByUserIdTask : Gs2RestSessionTask<GetSubscribeRankingByUserIdRequest, GetSubscribeRankingByUserIdResult>
        {
            public GetSubscribeRankingByUserIdTask(IGs2Session session, RestSessionRequestFactory factory, GetSubscribeRankingByUserIdRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(GetSubscribeRankingByUserIdRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "ranking2")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/ranking/subscribe/{rankingName}/user/{userId}/rank";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{rankingName}", !string.IsNullOrEmpty(request.RankingName) ? request.RankingName.ToString() : "null");
                url = url.Replace("{userId}", !string.IsNullOrEmpty(request.UserId) ? request.UserId.ToString() : "null");

                var sessionRequest = Factory.Get(url);
                if (request.ContextStack != null)
                {
                    sessionRequest.AddQueryString("contextStack", request.ContextStack);
                }
                if (request.Season != null) {
                    sessionRequest.AddQueryString("season", $"{request.Season}");
                }
                if (request.ScorerUserId != null) {
                    sessionRequest.AddQueryString("scorerUserId", $"{request.ScorerUserId}");
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
		public IEnumerator GetSubscribeRankingByUserId(
                Request.GetSubscribeRankingByUserIdRequest request,
                UnityAction<AsyncResult<Result.GetSubscribeRankingByUserIdResult>> callback
        )
		{
			var task = new GetSubscribeRankingByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.GetSubscribeRankingByUserIdResult>(task.Result, task.Error));
        }

		public IFuture<Result.GetSubscribeRankingByUserIdResult> GetSubscribeRankingByUserIdFuture(
                Request.GetSubscribeRankingByUserIdRequest request
        )
		{
			return new GetSubscribeRankingByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.GetSubscribeRankingByUserIdResult> GetSubscribeRankingByUserIdAsync(
                Request.GetSubscribeRankingByUserIdRequest request
        )
		{
            AsyncResult<Result.GetSubscribeRankingByUserIdResult> result = null;
			await GetSubscribeRankingByUserId(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
    #else
		public GetSubscribeRankingByUserIdTask GetSubscribeRankingByUserIdAsync(
                Request.GetSubscribeRankingByUserIdRequest request
        )
		{
			return new GetSubscribeRankingByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.GetSubscribeRankingByUserIdResult> GetSubscribeRankingByUserIdAsync(
                Request.GetSubscribeRankingByUserIdRequest request
        )
		{
			var task = new GetSubscribeRankingByUserIdTask(
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
                    .Replace("{service}", "ranking2")
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


        public class GetCurrentRankingMasterTask : Gs2RestSessionTask<GetCurrentRankingMasterRequest, GetCurrentRankingMasterResult>
        {
            public GetCurrentRankingMasterTask(IGs2Session session, RestSessionRequestFactory factory, GetCurrentRankingMasterRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(GetCurrentRankingMasterRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "ranking2")
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
		public IEnumerator GetCurrentRankingMaster(
                Request.GetCurrentRankingMasterRequest request,
                UnityAction<AsyncResult<Result.GetCurrentRankingMasterResult>> callback
        )
		{
			var task = new GetCurrentRankingMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.GetCurrentRankingMasterResult>(task.Result, task.Error));
        }

		public IFuture<Result.GetCurrentRankingMasterResult> GetCurrentRankingMasterFuture(
                Request.GetCurrentRankingMasterRequest request
        )
		{
			return new GetCurrentRankingMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.GetCurrentRankingMasterResult> GetCurrentRankingMasterAsync(
                Request.GetCurrentRankingMasterRequest request
        )
		{
            AsyncResult<Result.GetCurrentRankingMasterResult> result = null;
			await GetCurrentRankingMaster(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
    #else
		public GetCurrentRankingMasterTask GetCurrentRankingMasterAsync(
                Request.GetCurrentRankingMasterRequest request
        )
		{
			return new GetCurrentRankingMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.GetCurrentRankingMasterResult> GetCurrentRankingMasterAsync(
                Request.GetCurrentRankingMasterRequest request
        )
		{
			var task = new GetCurrentRankingMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class UpdateCurrentRankingMasterTask : Gs2RestSessionTask<UpdateCurrentRankingMasterRequest, UpdateCurrentRankingMasterResult>
        {
            public UpdateCurrentRankingMasterTask(IGs2Session session, RestSessionRequestFactory factory, UpdateCurrentRankingMasterRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(UpdateCurrentRankingMasterRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "ranking2")
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
		public IEnumerator UpdateCurrentRankingMaster(
                Request.UpdateCurrentRankingMasterRequest request,
                UnityAction<AsyncResult<Result.UpdateCurrentRankingMasterResult>> callback
        )
		{
			var task = new UpdateCurrentRankingMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.UpdateCurrentRankingMasterResult>(task.Result, task.Error));
        }

		public IFuture<Result.UpdateCurrentRankingMasterResult> UpdateCurrentRankingMasterFuture(
                Request.UpdateCurrentRankingMasterRequest request
        )
		{
			return new UpdateCurrentRankingMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.UpdateCurrentRankingMasterResult> UpdateCurrentRankingMasterAsync(
                Request.UpdateCurrentRankingMasterRequest request
        )
		{
            AsyncResult<Result.UpdateCurrentRankingMasterResult> result = null;
			await UpdateCurrentRankingMaster(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
    #else
		public UpdateCurrentRankingMasterTask UpdateCurrentRankingMasterAsync(
                Request.UpdateCurrentRankingMasterRequest request
        )
		{
			return new UpdateCurrentRankingMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.UpdateCurrentRankingMasterResult> UpdateCurrentRankingMasterAsync(
                Request.UpdateCurrentRankingMasterRequest request
        )
		{
			var task = new UpdateCurrentRankingMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class UpdateCurrentRankingMasterFromGitHubTask : Gs2RestSessionTask<UpdateCurrentRankingMasterFromGitHubRequest, UpdateCurrentRankingMasterFromGitHubResult>
        {
            public UpdateCurrentRankingMasterFromGitHubTask(IGs2Session session, RestSessionRequestFactory factory, UpdateCurrentRankingMasterFromGitHubRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(UpdateCurrentRankingMasterFromGitHubRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "ranking2")
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
		public IEnumerator UpdateCurrentRankingMasterFromGitHub(
                Request.UpdateCurrentRankingMasterFromGitHubRequest request,
                UnityAction<AsyncResult<Result.UpdateCurrentRankingMasterFromGitHubResult>> callback
        )
		{
			var task = new UpdateCurrentRankingMasterFromGitHubTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.UpdateCurrentRankingMasterFromGitHubResult>(task.Result, task.Error));
        }

		public IFuture<Result.UpdateCurrentRankingMasterFromGitHubResult> UpdateCurrentRankingMasterFromGitHubFuture(
                Request.UpdateCurrentRankingMasterFromGitHubRequest request
        )
		{
			return new UpdateCurrentRankingMasterFromGitHubTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.UpdateCurrentRankingMasterFromGitHubResult> UpdateCurrentRankingMasterFromGitHubAsync(
                Request.UpdateCurrentRankingMasterFromGitHubRequest request
        )
		{
            AsyncResult<Result.UpdateCurrentRankingMasterFromGitHubResult> result = null;
			await UpdateCurrentRankingMasterFromGitHub(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
    #else
		public UpdateCurrentRankingMasterFromGitHubTask UpdateCurrentRankingMasterFromGitHubAsync(
                Request.UpdateCurrentRankingMasterFromGitHubRequest request
        )
		{
			return new UpdateCurrentRankingMasterFromGitHubTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.UpdateCurrentRankingMasterFromGitHubResult> UpdateCurrentRankingMasterFromGitHubAsync(
                Request.UpdateCurrentRankingMasterFromGitHubRequest request
        )
		{
			var task = new UpdateCurrentRankingMasterFromGitHubTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class GetSubscribeTask : Gs2RestSessionTask<GetSubscribeRequest, GetSubscribeResult>
        {
            public GetSubscribeTask(IGs2Session session, RestSessionRequestFactory factory, GetSubscribeRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(GetSubscribeRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "ranking2")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/me/subscribe/{rankingName}/target/{targetUserId}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{rankingName}", !string.IsNullOrEmpty(request.RankingName) ? request.RankingName.ToString() : "null");
                url = url.Replace("{targetUserId}", !string.IsNullOrEmpty(request.TargetUserId) ? request.TargetUserId.ToString() : "null");

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
		public IEnumerator GetSubscribe(
                Request.GetSubscribeRequest request,
                UnityAction<AsyncResult<Result.GetSubscribeResult>> callback
        )
		{
			var task = new GetSubscribeTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.GetSubscribeResult>(task.Result, task.Error));
        }

		public IFuture<Result.GetSubscribeResult> GetSubscribeFuture(
                Request.GetSubscribeRequest request
        )
		{
			return new GetSubscribeTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.GetSubscribeResult> GetSubscribeAsync(
                Request.GetSubscribeRequest request
        )
		{
            AsyncResult<Result.GetSubscribeResult> result = null;
			await GetSubscribe(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
    #else
		public GetSubscribeTask GetSubscribeAsync(
                Request.GetSubscribeRequest request
        )
		{
			return new GetSubscribeTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.GetSubscribeResult> GetSubscribeAsync(
                Request.GetSubscribeRequest request
        )
		{
			var task = new GetSubscribeTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class GetSubscribeByUserIdTask : Gs2RestSessionTask<GetSubscribeByUserIdRequest, GetSubscribeByUserIdResult>
        {
            public GetSubscribeByUserIdTask(IGs2Session session, RestSessionRequestFactory factory, GetSubscribeByUserIdRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(GetSubscribeByUserIdRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "ranking2")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/{userId}/subscribe/{rankingName}/target/{targetUserId}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{rankingName}", !string.IsNullOrEmpty(request.RankingName) ? request.RankingName.ToString() : "null");
                url = url.Replace("{userId}", !string.IsNullOrEmpty(request.UserId) ? request.UserId.ToString() : "null");
                url = url.Replace("{targetUserId}", !string.IsNullOrEmpty(request.TargetUserId) ? request.TargetUserId.ToString() : "null");

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
		public IEnumerator GetSubscribeByUserId(
                Request.GetSubscribeByUserIdRequest request,
                UnityAction<AsyncResult<Result.GetSubscribeByUserIdResult>> callback
        )
		{
			var task = new GetSubscribeByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.GetSubscribeByUserIdResult>(task.Result, task.Error));
        }

		public IFuture<Result.GetSubscribeByUserIdResult> GetSubscribeByUserIdFuture(
                Request.GetSubscribeByUserIdRequest request
        )
		{
			return new GetSubscribeByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.GetSubscribeByUserIdResult> GetSubscribeByUserIdAsync(
                Request.GetSubscribeByUserIdRequest request
        )
		{
            AsyncResult<Result.GetSubscribeByUserIdResult> result = null;
			await GetSubscribeByUserId(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
    #else
		public GetSubscribeByUserIdTask GetSubscribeByUserIdAsync(
                Request.GetSubscribeByUserIdRequest request
        )
		{
			return new GetSubscribeByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.GetSubscribeByUserIdResult> GetSubscribeByUserIdAsync(
                Request.GetSubscribeByUserIdRequest request
        )
		{
			var task = new GetSubscribeByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class DeleteSubscribeTask : Gs2RestSessionTask<DeleteSubscribeRequest, DeleteSubscribeResult>
        {
            public DeleteSubscribeTask(IGs2Session session, RestSessionRequestFactory factory, DeleteSubscribeRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DeleteSubscribeRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "ranking2")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/me/subscribe/{rankingName}/target/{targetUserId}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{rankingName}", !string.IsNullOrEmpty(request.RankingName) ? request.RankingName.ToString() : "null");
                url = url.Replace("{targetUserId}", !string.IsNullOrEmpty(request.TargetUserId) ? request.TargetUserId.ToString() : "null");

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
		public IEnumerator DeleteSubscribe(
                Request.DeleteSubscribeRequest request,
                UnityAction<AsyncResult<Result.DeleteSubscribeResult>> callback
        )
		{
			var task = new DeleteSubscribeTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.DeleteSubscribeResult>(task.Result, task.Error));
        }

		public IFuture<Result.DeleteSubscribeResult> DeleteSubscribeFuture(
                Request.DeleteSubscribeRequest request
        )
		{
			return new DeleteSubscribeTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.DeleteSubscribeResult> DeleteSubscribeAsync(
                Request.DeleteSubscribeRequest request
        )
		{
            AsyncResult<Result.DeleteSubscribeResult> result = null;
			await DeleteSubscribe(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
    #else
		public DeleteSubscribeTask DeleteSubscribeAsync(
                Request.DeleteSubscribeRequest request
        )
		{
			return new DeleteSubscribeTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.DeleteSubscribeResult> DeleteSubscribeAsync(
                Request.DeleteSubscribeRequest request
        )
		{
			var task = new DeleteSubscribeTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class DeleteSubscribeByUserIdTask : Gs2RestSessionTask<DeleteSubscribeByUserIdRequest, DeleteSubscribeByUserIdResult>
        {
            public DeleteSubscribeByUserIdTask(IGs2Session session, RestSessionRequestFactory factory, DeleteSubscribeByUserIdRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DeleteSubscribeByUserIdRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "ranking2")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/{userId}/subscribe/{rankingName}/target/{targetUserId}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{rankingName}", !string.IsNullOrEmpty(request.RankingName) ? request.RankingName.ToString() : "null");
                url = url.Replace("{userId}", !string.IsNullOrEmpty(request.UserId) ? request.UserId.ToString() : "null");
                url = url.Replace("{targetUserId}", !string.IsNullOrEmpty(request.TargetUserId) ? request.TargetUserId.ToString() : "null");

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
		public IEnumerator DeleteSubscribeByUserId(
                Request.DeleteSubscribeByUserIdRequest request,
                UnityAction<AsyncResult<Result.DeleteSubscribeByUserIdResult>> callback
        )
		{
			var task = new DeleteSubscribeByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.DeleteSubscribeByUserIdResult>(task.Result, task.Error));
        }

		public IFuture<Result.DeleteSubscribeByUserIdResult> DeleteSubscribeByUserIdFuture(
                Request.DeleteSubscribeByUserIdRequest request
        )
		{
			return new DeleteSubscribeByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.DeleteSubscribeByUserIdResult> DeleteSubscribeByUserIdAsync(
                Request.DeleteSubscribeByUserIdRequest request
        )
		{
            AsyncResult<Result.DeleteSubscribeByUserIdResult> result = null;
			await DeleteSubscribeByUserId(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
    #else
		public DeleteSubscribeByUserIdTask DeleteSubscribeByUserIdAsync(
                Request.DeleteSubscribeByUserIdRequest request
        )
		{
			return new DeleteSubscribeByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.DeleteSubscribeByUserIdResult> DeleteSubscribeByUserIdAsync(
                Request.DeleteSubscribeByUserIdRequest request
        )
		{
			var task = new DeleteSubscribeByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif
	}
}