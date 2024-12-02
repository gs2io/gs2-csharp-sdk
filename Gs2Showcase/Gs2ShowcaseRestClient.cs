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
using Gs2.Gs2Showcase.Request;
using Gs2.Gs2Showcase.Result;
using Gs2.Util.LitJson;

namespace Gs2.Gs2Showcase
{
	public class Gs2ShowcaseRestClient : AbstractGs2Client
	{
#if UNITY_2017_1_OR_NEWER
		private readonly CertificateHandler _certificateHandler;
#endif

		public static string Endpoint = "showcase";

        protected Gs2RestSession Gs2RestSession => (Gs2RestSession) Gs2Session;

		public Gs2ShowcaseRestClient(Gs2RestSession Gs2RestSession) : base(Gs2RestSession)
		{

		}

#if UNITY_2017_1_OR_NEWER
		public Gs2ShowcaseRestClient(Gs2RestSession gs2RestSession, CertificateHandler certificateHandler) : base(gs2RestSession)
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
                    .Replace("{service}", "showcase")
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
                    .Replace("{service}", "showcase")
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
                if (request.BuyScript != null)
                {
                    jsonWriter.WritePropertyName("buyScript");
                    request.BuyScript.WriteJson(jsonWriter);
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
                    .Replace("{service}", "showcase")
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
                    .Replace("{service}", "showcase")
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
                    .Replace("{service}", "showcase")
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
                if (request.BuyScript != null)
                {
                    jsonWriter.WritePropertyName("buyScript");
                    request.BuyScript.WriteJson(jsonWriter);
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
                if (request.DryRun)
                {
                    sessionRequest.AddHeader("X-GS2-DRY-RUN", "true");
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
                    .Replace("{service}", "showcase")
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
                    .Replace("{service}", "showcase")
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
                    .Replace("{service}", "showcase")
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
                    .Replace("{service}", "showcase")
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
                    .Replace("{service}", "showcase")
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
                    .Replace("{service}", "showcase")
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
                    .Replace("{service}", "showcase")
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
                    .Replace("{service}", "showcase")
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


        public class DescribeSalesItemMastersTask : Gs2RestSessionTask<DescribeSalesItemMastersRequest, DescribeSalesItemMastersResult>
        {
            public DescribeSalesItemMastersTask(IGs2Session session, RestSessionRequestFactory factory, DescribeSalesItemMastersRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DescribeSalesItemMastersRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "showcase")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/master/salesItem";

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
		public IEnumerator DescribeSalesItemMasters(
                Request.DescribeSalesItemMastersRequest request,
                UnityAction<AsyncResult<Result.DescribeSalesItemMastersResult>> callback
        )
		{
			var task = new DescribeSalesItemMastersTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.DescribeSalesItemMastersResult>(task.Result, task.Error));
        }

		public IFuture<Result.DescribeSalesItemMastersResult> DescribeSalesItemMastersFuture(
                Request.DescribeSalesItemMastersRequest request
        )
		{
			return new DescribeSalesItemMastersTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.DescribeSalesItemMastersResult> DescribeSalesItemMastersAsync(
                Request.DescribeSalesItemMastersRequest request
        )
		{
            AsyncResult<Result.DescribeSalesItemMastersResult> result = null;
			await DescribeSalesItemMasters(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
    #else
		public DescribeSalesItemMastersTask DescribeSalesItemMastersAsync(
                Request.DescribeSalesItemMastersRequest request
        )
		{
			return new DescribeSalesItemMastersTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.DescribeSalesItemMastersResult> DescribeSalesItemMastersAsync(
                Request.DescribeSalesItemMastersRequest request
        )
		{
			var task = new DescribeSalesItemMastersTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class CreateSalesItemMasterTask : Gs2RestSessionTask<CreateSalesItemMasterRequest, CreateSalesItemMasterResult>
        {
            public CreateSalesItemMasterTask(IGs2Session session, RestSessionRequestFactory factory, CreateSalesItemMasterRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(CreateSalesItemMasterRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "showcase")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/master/salesItem";

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
                if (request.VerifyActions != null)
                {
                    jsonWriter.WritePropertyName("verifyActions");
                    jsonWriter.WriteArrayStart();
                    foreach(var item in request.VerifyActions)
                    {
                        if (item == null) {
                            jsonWriter.Write(null);
                        } else {
                            item.WriteJson(jsonWriter);
                        }
                    }
                    jsonWriter.WriteArrayEnd();
                }
                if (request.ConsumeActions != null)
                {
                    jsonWriter.WritePropertyName("consumeActions");
                    jsonWriter.WriteArrayStart();
                    foreach(var item in request.ConsumeActions)
                    {
                        item.WriteJson(jsonWriter);
                    }
                    jsonWriter.WriteArrayEnd();
                }
                if (request.AcquireActions != null)
                {
                    jsonWriter.WritePropertyName("acquireActions");
                    jsonWriter.WriteArrayStart();
                    foreach(var item in request.AcquireActions)
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
		public IEnumerator CreateSalesItemMaster(
                Request.CreateSalesItemMasterRequest request,
                UnityAction<AsyncResult<Result.CreateSalesItemMasterResult>> callback
        )
		{
			var task = new CreateSalesItemMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.CreateSalesItemMasterResult>(task.Result, task.Error));
        }

		public IFuture<Result.CreateSalesItemMasterResult> CreateSalesItemMasterFuture(
                Request.CreateSalesItemMasterRequest request
        )
		{
			return new CreateSalesItemMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.CreateSalesItemMasterResult> CreateSalesItemMasterAsync(
                Request.CreateSalesItemMasterRequest request
        )
		{
            AsyncResult<Result.CreateSalesItemMasterResult> result = null;
			await CreateSalesItemMaster(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
    #else
		public CreateSalesItemMasterTask CreateSalesItemMasterAsync(
                Request.CreateSalesItemMasterRequest request
        )
		{
			return new CreateSalesItemMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.CreateSalesItemMasterResult> CreateSalesItemMasterAsync(
                Request.CreateSalesItemMasterRequest request
        )
		{
			var task = new CreateSalesItemMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class GetSalesItemMasterTask : Gs2RestSessionTask<GetSalesItemMasterRequest, GetSalesItemMasterResult>
        {
            public GetSalesItemMasterTask(IGs2Session session, RestSessionRequestFactory factory, GetSalesItemMasterRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(GetSalesItemMasterRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "showcase")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/master/salesItem/{salesItemName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{salesItemName}", !string.IsNullOrEmpty(request.SalesItemName) ? request.SalesItemName.ToString() : "null");

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
		public IEnumerator GetSalesItemMaster(
                Request.GetSalesItemMasterRequest request,
                UnityAction<AsyncResult<Result.GetSalesItemMasterResult>> callback
        )
		{
			var task = new GetSalesItemMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.GetSalesItemMasterResult>(task.Result, task.Error));
        }

		public IFuture<Result.GetSalesItemMasterResult> GetSalesItemMasterFuture(
                Request.GetSalesItemMasterRequest request
        )
		{
			return new GetSalesItemMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.GetSalesItemMasterResult> GetSalesItemMasterAsync(
                Request.GetSalesItemMasterRequest request
        )
		{
            AsyncResult<Result.GetSalesItemMasterResult> result = null;
			await GetSalesItemMaster(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
    #else
		public GetSalesItemMasterTask GetSalesItemMasterAsync(
                Request.GetSalesItemMasterRequest request
        )
		{
			return new GetSalesItemMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.GetSalesItemMasterResult> GetSalesItemMasterAsync(
                Request.GetSalesItemMasterRequest request
        )
		{
			var task = new GetSalesItemMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class UpdateSalesItemMasterTask : Gs2RestSessionTask<UpdateSalesItemMasterRequest, UpdateSalesItemMasterResult>
        {
            public UpdateSalesItemMasterTask(IGs2Session session, RestSessionRequestFactory factory, UpdateSalesItemMasterRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(UpdateSalesItemMasterRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "showcase")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/master/salesItem/{salesItemName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{salesItemName}", !string.IsNullOrEmpty(request.SalesItemName) ? request.SalesItemName.ToString() : "null");

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
                if (request.VerifyActions != null)
                {
                    jsonWriter.WritePropertyName("verifyActions");
                    jsonWriter.WriteArrayStart();
                    foreach(var item in request.VerifyActions)
                    {
                        if (item == null) {
                            jsonWriter.Write(null);
                        } else {
                            item.WriteJson(jsonWriter);
                        }
                    }
                    jsonWriter.WriteArrayEnd();
                }
                if (request.ConsumeActions != null)
                {
                    jsonWriter.WritePropertyName("consumeActions");
                    jsonWriter.WriteArrayStart();
                    foreach(var item in request.ConsumeActions)
                    {
                        item.WriteJson(jsonWriter);
                    }
                    jsonWriter.WriteArrayEnd();
                }
                if (request.AcquireActions != null)
                {
                    jsonWriter.WritePropertyName("acquireActions");
                    jsonWriter.WriteArrayStart();
                    foreach(var item in request.AcquireActions)
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
		public IEnumerator UpdateSalesItemMaster(
                Request.UpdateSalesItemMasterRequest request,
                UnityAction<AsyncResult<Result.UpdateSalesItemMasterResult>> callback
        )
		{
			var task = new UpdateSalesItemMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.UpdateSalesItemMasterResult>(task.Result, task.Error));
        }

		public IFuture<Result.UpdateSalesItemMasterResult> UpdateSalesItemMasterFuture(
                Request.UpdateSalesItemMasterRequest request
        )
		{
			return new UpdateSalesItemMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.UpdateSalesItemMasterResult> UpdateSalesItemMasterAsync(
                Request.UpdateSalesItemMasterRequest request
        )
		{
            AsyncResult<Result.UpdateSalesItemMasterResult> result = null;
			await UpdateSalesItemMaster(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
    #else
		public UpdateSalesItemMasterTask UpdateSalesItemMasterAsync(
                Request.UpdateSalesItemMasterRequest request
        )
		{
			return new UpdateSalesItemMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.UpdateSalesItemMasterResult> UpdateSalesItemMasterAsync(
                Request.UpdateSalesItemMasterRequest request
        )
		{
			var task = new UpdateSalesItemMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class DeleteSalesItemMasterTask : Gs2RestSessionTask<DeleteSalesItemMasterRequest, DeleteSalesItemMasterResult>
        {
            public DeleteSalesItemMasterTask(IGs2Session session, RestSessionRequestFactory factory, DeleteSalesItemMasterRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DeleteSalesItemMasterRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "showcase")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/master/salesItem/{salesItemName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{salesItemName}", !string.IsNullOrEmpty(request.SalesItemName) ? request.SalesItemName.ToString() : "null");

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
		public IEnumerator DeleteSalesItemMaster(
                Request.DeleteSalesItemMasterRequest request,
                UnityAction<AsyncResult<Result.DeleteSalesItemMasterResult>> callback
        )
		{
			var task = new DeleteSalesItemMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.DeleteSalesItemMasterResult>(task.Result, task.Error));
        }

		public IFuture<Result.DeleteSalesItemMasterResult> DeleteSalesItemMasterFuture(
                Request.DeleteSalesItemMasterRequest request
        )
		{
			return new DeleteSalesItemMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.DeleteSalesItemMasterResult> DeleteSalesItemMasterAsync(
                Request.DeleteSalesItemMasterRequest request
        )
		{
            AsyncResult<Result.DeleteSalesItemMasterResult> result = null;
			await DeleteSalesItemMaster(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
    #else
		public DeleteSalesItemMasterTask DeleteSalesItemMasterAsync(
                Request.DeleteSalesItemMasterRequest request
        )
		{
			return new DeleteSalesItemMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.DeleteSalesItemMasterResult> DeleteSalesItemMasterAsync(
                Request.DeleteSalesItemMasterRequest request
        )
		{
			var task = new DeleteSalesItemMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class DescribeSalesItemGroupMastersTask : Gs2RestSessionTask<DescribeSalesItemGroupMastersRequest, DescribeSalesItemGroupMastersResult>
        {
            public DescribeSalesItemGroupMastersTask(IGs2Session session, RestSessionRequestFactory factory, DescribeSalesItemGroupMastersRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DescribeSalesItemGroupMastersRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "showcase")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/master/group";

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
		public IEnumerator DescribeSalesItemGroupMasters(
                Request.DescribeSalesItemGroupMastersRequest request,
                UnityAction<AsyncResult<Result.DescribeSalesItemGroupMastersResult>> callback
        )
		{
			var task = new DescribeSalesItemGroupMastersTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.DescribeSalesItemGroupMastersResult>(task.Result, task.Error));
        }

		public IFuture<Result.DescribeSalesItemGroupMastersResult> DescribeSalesItemGroupMastersFuture(
                Request.DescribeSalesItemGroupMastersRequest request
        )
		{
			return new DescribeSalesItemGroupMastersTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.DescribeSalesItemGroupMastersResult> DescribeSalesItemGroupMastersAsync(
                Request.DescribeSalesItemGroupMastersRequest request
        )
		{
            AsyncResult<Result.DescribeSalesItemGroupMastersResult> result = null;
			await DescribeSalesItemGroupMasters(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
    #else
		public DescribeSalesItemGroupMastersTask DescribeSalesItemGroupMastersAsync(
                Request.DescribeSalesItemGroupMastersRequest request
        )
		{
			return new DescribeSalesItemGroupMastersTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.DescribeSalesItemGroupMastersResult> DescribeSalesItemGroupMastersAsync(
                Request.DescribeSalesItemGroupMastersRequest request
        )
		{
			var task = new DescribeSalesItemGroupMastersTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class CreateSalesItemGroupMasterTask : Gs2RestSessionTask<CreateSalesItemGroupMasterRequest, CreateSalesItemGroupMasterResult>
        {
            public CreateSalesItemGroupMasterTask(IGs2Session session, RestSessionRequestFactory factory, CreateSalesItemGroupMasterRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(CreateSalesItemGroupMasterRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "showcase")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/master/group";

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
                if (request.SalesItemNames != null)
                {
                    jsonWriter.WritePropertyName("salesItemNames");
                    jsonWriter.WriteArrayStart();
                    foreach(var item in request.SalesItemNames)
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
		public IEnumerator CreateSalesItemGroupMaster(
                Request.CreateSalesItemGroupMasterRequest request,
                UnityAction<AsyncResult<Result.CreateSalesItemGroupMasterResult>> callback
        )
		{
			var task = new CreateSalesItemGroupMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.CreateSalesItemGroupMasterResult>(task.Result, task.Error));
        }

		public IFuture<Result.CreateSalesItemGroupMasterResult> CreateSalesItemGroupMasterFuture(
                Request.CreateSalesItemGroupMasterRequest request
        )
		{
			return new CreateSalesItemGroupMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.CreateSalesItemGroupMasterResult> CreateSalesItemGroupMasterAsync(
                Request.CreateSalesItemGroupMasterRequest request
        )
		{
            AsyncResult<Result.CreateSalesItemGroupMasterResult> result = null;
			await CreateSalesItemGroupMaster(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
    #else
		public CreateSalesItemGroupMasterTask CreateSalesItemGroupMasterAsync(
                Request.CreateSalesItemGroupMasterRequest request
        )
		{
			return new CreateSalesItemGroupMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.CreateSalesItemGroupMasterResult> CreateSalesItemGroupMasterAsync(
                Request.CreateSalesItemGroupMasterRequest request
        )
		{
			var task = new CreateSalesItemGroupMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class GetSalesItemGroupMasterTask : Gs2RestSessionTask<GetSalesItemGroupMasterRequest, GetSalesItemGroupMasterResult>
        {
            public GetSalesItemGroupMasterTask(IGs2Session session, RestSessionRequestFactory factory, GetSalesItemGroupMasterRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(GetSalesItemGroupMasterRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "showcase")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/master/group/{salesItemGroupName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{salesItemGroupName}", !string.IsNullOrEmpty(request.SalesItemGroupName) ? request.SalesItemGroupName.ToString() : "null");

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
		public IEnumerator GetSalesItemGroupMaster(
                Request.GetSalesItemGroupMasterRequest request,
                UnityAction<AsyncResult<Result.GetSalesItemGroupMasterResult>> callback
        )
		{
			var task = new GetSalesItemGroupMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.GetSalesItemGroupMasterResult>(task.Result, task.Error));
        }

		public IFuture<Result.GetSalesItemGroupMasterResult> GetSalesItemGroupMasterFuture(
                Request.GetSalesItemGroupMasterRequest request
        )
		{
			return new GetSalesItemGroupMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.GetSalesItemGroupMasterResult> GetSalesItemGroupMasterAsync(
                Request.GetSalesItemGroupMasterRequest request
        )
		{
            AsyncResult<Result.GetSalesItemGroupMasterResult> result = null;
			await GetSalesItemGroupMaster(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
    #else
		public GetSalesItemGroupMasterTask GetSalesItemGroupMasterAsync(
                Request.GetSalesItemGroupMasterRequest request
        )
		{
			return new GetSalesItemGroupMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.GetSalesItemGroupMasterResult> GetSalesItemGroupMasterAsync(
                Request.GetSalesItemGroupMasterRequest request
        )
		{
			var task = new GetSalesItemGroupMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class UpdateSalesItemGroupMasterTask : Gs2RestSessionTask<UpdateSalesItemGroupMasterRequest, UpdateSalesItemGroupMasterResult>
        {
            public UpdateSalesItemGroupMasterTask(IGs2Session session, RestSessionRequestFactory factory, UpdateSalesItemGroupMasterRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(UpdateSalesItemGroupMasterRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "showcase")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/master/group/{salesItemGroupName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{salesItemGroupName}", !string.IsNullOrEmpty(request.SalesItemGroupName) ? request.SalesItemGroupName.ToString() : "null");

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
                if (request.SalesItemNames != null)
                {
                    jsonWriter.WritePropertyName("salesItemNames");
                    jsonWriter.WriteArrayStart();
                    foreach(var item in request.SalesItemNames)
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
		public IEnumerator UpdateSalesItemGroupMaster(
                Request.UpdateSalesItemGroupMasterRequest request,
                UnityAction<AsyncResult<Result.UpdateSalesItemGroupMasterResult>> callback
        )
		{
			var task = new UpdateSalesItemGroupMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.UpdateSalesItemGroupMasterResult>(task.Result, task.Error));
        }

		public IFuture<Result.UpdateSalesItemGroupMasterResult> UpdateSalesItemGroupMasterFuture(
                Request.UpdateSalesItemGroupMasterRequest request
        )
		{
			return new UpdateSalesItemGroupMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.UpdateSalesItemGroupMasterResult> UpdateSalesItemGroupMasterAsync(
                Request.UpdateSalesItemGroupMasterRequest request
        )
		{
            AsyncResult<Result.UpdateSalesItemGroupMasterResult> result = null;
			await UpdateSalesItemGroupMaster(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
    #else
		public UpdateSalesItemGroupMasterTask UpdateSalesItemGroupMasterAsync(
                Request.UpdateSalesItemGroupMasterRequest request
        )
		{
			return new UpdateSalesItemGroupMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.UpdateSalesItemGroupMasterResult> UpdateSalesItemGroupMasterAsync(
                Request.UpdateSalesItemGroupMasterRequest request
        )
		{
			var task = new UpdateSalesItemGroupMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class DeleteSalesItemGroupMasterTask : Gs2RestSessionTask<DeleteSalesItemGroupMasterRequest, DeleteSalesItemGroupMasterResult>
        {
            public DeleteSalesItemGroupMasterTask(IGs2Session session, RestSessionRequestFactory factory, DeleteSalesItemGroupMasterRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DeleteSalesItemGroupMasterRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "showcase")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/master/group/{salesItemGroupName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{salesItemGroupName}", !string.IsNullOrEmpty(request.SalesItemGroupName) ? request.SalesItemGroupName.ToString() : "null");

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
		public IEnumerator DeleteSalesItemGroupMaster(
                Request.DeleteSalesItemGroupMasterRequest request,
                UnityAction<AsyncResult<Result.DeleteSalesItemGroupMasterResult>> callback
        )
		{
			var task = new DeleteSalesItemGroupMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.DeleteSalesItemGroupMasterResult>(task.Result, task.Error));
        }

		public IFuture<Result.DeleteSalesItemGroupMasterResult> DeleteSalesItemGroupMasterFuture(
                Request.DeleteSalesItemGroupMasterRequest request
        )
		{
			return new DeleteSalesItemGroupMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.DeleteSalesItemGroupMasterResult> DeleteSalesItemGroupMasterAsync(
                Request.DeleteSalesItemGroupMasterRequest request
        )
		{
            AsyncResult<Result.DeleteSalesItemGroupMasterResult> result = null;
			await DeleteSalesItemGroupMaster(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
    #else
		public DeleteSalesItemGroupMasterTask DeleteSalesItemGroupMasterAsync(
                Request.DeleteSalesItemGroupMasterRequest request
        )
		{
			return new DeleteSalesItemGroupMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.DeleteSalesItemGroupMasterResult> DeleteSalesItemGroupMasterAsync(
                Request.DeleteSalesItemGroupMasterRequest request
        )
		{
			var task = new DeleteSalesItemGroupMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class DescribeShowcaseMastersTask : Gs2RestSessionTask<DescribeShowcaseMastersRequest, DescribeShowcaseMastersResult>
        {
            public DescribeShowcaseMastersTask(IGs2Session session, RestSessionRequestFactory factory, DescribeShowcaseMastersRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DescribeShowcaseMastersRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "showcase")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/master/showcase";

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
		public IEnumerator DescribeShowcaseMasters(
                Request.DescribeShowcaseMastersRequest request,
                UnityAction<AsyncResult<Result.DescribeShowcaseMastersResult>> callback
        )
		{
			var task = new DescribeShowcaseMastersTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.DescribeShowcaseMastersResult>(task.Result, task.Error));
        }

		public IFuture<Result.DescribeShowcaseMastersResult> DescribeShowcaseMastersFuture(
                Request.DescribeShowcaseMastersRequest request
        )
		{
			return new DescribeShowcaseMastersTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.DescribeShowcaseMastersResult> DescribeShowcaseMastersAsync(
                Request.DescribeShowcaseMastersRequest request
        )
		{
            AsyncResult<Result.DescribeShowcaseMastersResult> result = null;
			await DescribeShowcaseMasters(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
    #else
		public DescribeShowcaseMastersTask DescribeShowcaseMastersAsync(
                Request.DescribeShowcaseMastersRequest request
        )
		{
			return new DescribeShowcaseMastersTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.DescribeShowcaseMastersResult> DescribeShowcaseMastersAsync(
                Request.DescribeShowcaseMastersRequest request
        )
		{
			var task = new DescribeShowcaseMastersTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class CreateShowcaseMasterTask : Gs2RestSessionTask<CreateShowcaseMasterRequest, CreateShowcaseMasterResult>
        {
            public CreateShowcaseMasterTask(IGs2Session session, RestSessionRequestFactory factory, CreateShowcaseMasterRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(CreateShowcaseMasterRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "showcase")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/master/showcase";

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
                if (request.DisplayItems != null)
                {
                    jsonWriter.WritePropertyName("displayItems");
                    jsonWriter.WriteArrayStart();
                    foreach(var item in request.DisplayItems)
                    {
                        item.WriteJson(jsonWriter);
                    }
                    jsonWriter.WriteArrayEnd();
                }
                if (request.SalesPeriodEventId != null)
                {
                    jsonWriter.WritePropertyName("salesPeriodEventId");
                    jsonWriter.Write(request.SalesPeriodEventId);
                }
                if (request.ContextStack != null)
                {
                    jsonWriter.WritePropertyName("contextStack");
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
		public IEnumerator CreateShowcaseMaster(
                Request.CreateShowcaseMasterRequest request,
                UnityAction<AsyncResult<Result.CreateShowcaseMasterResult>> callback
        )
		{
			var task = new CreateShowcaseMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.CreateShowcaseMasterResult>(task.Result, task.Error));
        }

		public IFuture<Result.CreateShowcaseMasterResult> CreateShowcaseMasterFuture(
                Request.CreateShowcaseMasterRequest request
        )
		{
			return new CreateShowcaseMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.CreateShowcaseMasterResult> CreateShowcaseMasterAsync(
                Request.CreateShowcaseMasterRequest request
        )
		{
            AsyncResult<Result.CreateShowcaseMasterResult> result = null;
			await CreateShowcaseMaster(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
    #else
		public CreateShowcaseMasterTask CreateShowcaseMasterAsync(
                Request.CreateShowcaseMasterRequest request
        )
		{
			return new CreateShowcaseMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.CreateShowcaseMasterResult> CreateShowcaseMasterAsync(
                Request.CreateShowcaseMasterRequest request
        )
		{
			var task = new CreateShowcaseMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class GetShowcaseMasterTask : Gs2RestSessionTask<GetShowcaseMasterRequest, GetShowcaseMasterResult>
        {
            public GetShowcaseMasterTask(IGs2Session session, RestSessionRequestFactory factory, GetShowcaseMasterRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(GetShowcaseMasterRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "showcase")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/master/showcase/{showcaseName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{showcaseName}", !string.IsNullOrEmpty(request.ShowcaseName) ? request.ShowcaseName.ToString() : "null");

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
		public IEnumerator GetShowcaseMaster(
                Request.GetShowcaseMasterRequest request,
                UnityAction<AsyncResult<Result.GetShowcaseMasterResult>> callback
        )
		{
			var task = new GetShowcaseMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.GetShowcaseMasterResult>(task.Result, task.Error));
        }

		public IFuture<Result.GetShowcaseMasterResult> GetShowcaseMasterFuture(
                Request.GetShowcaseMasterRequest request
        )
		{
			return new GetShowcaseMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.GetShowcaseMasterResult> GetShowcaseMasterAsync(
                Request.GetShowcaseMasterRequest request
        )
		{
            AsyncResult<Result.GetShowcaseMasterResult> result = null;
			await GetShowcaseMaster(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
    #else
		public GetShowcaseMasterTask GetShowcaseMasterAsync(
                Request.GetShowcaseMasterRequest request
        )
		{
			return new GetShowcaseMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.GetShowcaseMasterResult> GetShowcaseMasterAsync(
                Request.GetShowcaseMasterRequest request
        )
		{
			var task = new GetShowcaseMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class UpdateShowcaseMasterTask : Gs2RestSessionTask<UpdateShowcaseMasterRequest, UpdateShowcaseMasterResult>
        {
            public UpdateShowcaseMasterTask(IGs2Session session, RestSessionRequestFactory factory, UpdateShowcaseMasterRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(UpdateShowcaseMasterRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "showcase")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/master/showcase/{showcaseName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{showcaseName}", !string.IsNullOrEmpty(request.ShowcaseName) ? request.ShowcaseName.ToString() : "null");

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
                if (request.DisplayItems != null)
                {
                    jsonWriter.WritePropertyName("displayItems");
                    jsonWriter.WriteArrayStart();
                    foreach(var item in request.DisplayItems)
                    {
                        item.WriteJson(jsonWriter);
                    }
                    jsonWriter.WriteArrayEnd();
                }
                if (request.SalesPeriodEventId != null)
                {
                    jsonWriter.WritePropertyName("salesPeriodEventId");
                    jsonWriter.Write(request.SalesPeriodEventId);
                }
                if (request.ContextStack != null)
                {
                    jsonWriter.WritePropertyName("contextStack");
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
		public IEnumerator UpdateShowcaseMaster(
                Request.UpdateShowcaseMasterRequest request,
                UnityAction<AsyncResult<Result.UpdateShowcaseMasterResult>> callback
        )
		{
			var task = new UpdateShowcaseMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.UpdateShowcaseMasterResult>(task.Result, task.Error));
        }

		public IFuture<Result.UpdateShowcaseMasterResult> UpdateShowcaseMasterFuture(
                Request.UpdateShowcaseMasterRequest request
        )
		{
			return new UpdateShowcaseMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.UpdateShowcaseMasterResult> UpdateShowcaseMasterAsync(
                Request.UpdateShowcaseMasterRequest request
        )
		{
            AsyncResult<Result.UpdateShowcaseMasterResult> result = null;
			await UpdateShowcaseMaster(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
    #else
		public UpdateShowcaseMasterTask UpdateShowcaseMasterAsync(
                Request.UpdateShowcaseMasterRequest request
        )
		{
			return new UpdateShowcaseMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.UpdateShowcaseMasterResult> UpdateShowcaseMasterAsync(
                Request.UpdateShowcaseMasterRequest request
        )
		{
			var task = new UpdateShowcaseMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class DeleteShowcaseMasterTask : Gs2RestSessionTask<DeleteShowcaseMasterRequest, DeleteShowcaseMasterResult>
        {
            public DeleteShowcaseMasterTask(IGs2Session session, RestSessionRequestFactory factory, DeleteShowcaseMasterRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DeleteShowcaseMasterRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "showcase")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/master/showcase/{showcaseName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{showcaseName}", !string.IsNullOrEmpty(request.ShowcaseName) ? request.ShowcaseName.ToString() : "null");

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
		public IEnumerator DeleteShowcaseMaster(
                Request.DeleteShowcaseMasterRequest request,
                UnityAction<AsyncResult<Result.DeleteShowcaseMasterResult>> callback
        )
		{
			var task = new DeleteShowcaseMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.DeleteShowcaseMasterResult>(task.Result, task.Error));
        }

		public IFuture<Result.DeleteShowcaseMasterResult> DeleteShowcaseMasterFuture(
                Request.DeleteShowcaseMasterRequest request
        )
		{
			return new DeleteShowcaseMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.DeleteShowcaseMasterResult> DeleteShowcaseMasterAsync(
                Request.DeleteShowcaseMasterRequest request
        )
		{
            AsyncResult<Result.DeleteShowcaseMasterResult> result = null;
			await DeleteShowcaseMaster(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
    #else
		public DeleteShowcaseMasterTask DeleteShowcaseMasterAsync(
                Request.DeleteShowcaseMasterRequest request
        )
		{
			return new DeleteShowcaseMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.DeleteShowcaseMasterResult> DeleteShowcaseMasterAsync(
                Request.DeleteShowcaseMasterRequest request
        )
		{
			var task = new DeleteShowcaseMasterTask(
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
                    .Replace("{service}", "showcase")
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


        public class GetCurrentShowcaseMasterTask : Gs2RestSessionTask<GetCurrentShowcaseMasterRequest, GetCurrentShowcaseMasterResult>
        {
            public GetCurrentShowcaseMasterTask(IGs2Session session, RestSessionRequestFactory factory, GetCurrentShowcaseMasterRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(GetCurrentShowcaseMasterRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "showcase")
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
		public IEnumerator GetCurrentShowcaseMaster(
                Request.GetCurrentShowcaseMasterRequest request,
                UnityAction<AsyncResult<Result.GetCurrentShowcaseMasterResult>> callback
        )
		{
			var task = new GetCurrentShowcaseMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.GetCurrentShowcaseMasterResult>(task.Result, task.Error));
        }

		public IFuture<Result.GetCurrentShowcaseMasterResult> GetCurrentShowcaseMasterFuture(
                Request.GetCurrentShowcaseMasterRequest request
        )
		{
			return new GetCurrentShowcaseMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.GetCurrentShowcaseMasterResult> GetCurrentShowcaseMasterAsync(
                Request.GetCurrentShowcaseMasterRequest request
        )
		{
            AsyncResult<Result.GetCurrentShowcaseMasterResult> result = null;
			await GetCurrentShowcaseMaster(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
    #else
		public GetCurrentShowcaseMasterTask GetCurrentShowcaseMasterAsync(
                Request.GetCurrentShowcaseMasterRequest request
        )
		{
			return new GetCurrentShowcaseMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.GetCurrentShowcaseMasterResult> GetCurrentShowcaseMasterAsync(
                Request.GetCurrentShowcaseMasterRequest request
        )
		{
			var task = new GetCurrentShowcaseMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class UpdateCurrentShowcaseMasterTask : Gs2RestSessionTask<UpdateCurrentShowcaseMasterRequest, UpdateCurrentShowcaseMasterResult>
        {
            public UpdateCurrentShowcaseMasterTask(IGs2Session session, RestSessionRequestFactory factory, UpdateCurrentShowcaseMasterRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(UpdateCurrentShowcaseMasterRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "showcase")
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
		public IEnumerator UpdateCurrentShowcaseMaster(
                Request.UpdateCurrentShowcaseMasterRequest request,
                UnityAction<AsyncResult<Result.UpdateCurrentShowcaseMasterResult>> callback
        )
		{
			var task = new UpdateCurrentShowcaseMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.UpdateCurrentShowcaseMasterResult>(task.Result, task.Error));
        }

		public IFuture<Result.UpdateCurrentShowcaseMasterResult> UpdateCurrentShowcaseMasterFuture(
                Request.UpdateCurrentShowcaseMasterRequest request
        )
		{
			return new UpdateCurrentShowcaseMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.UpdateCurrentShowcaseMasterResult> UpdateCurrentShowcaseMasterAsync(
                Request.UpdateCurrentShowcaseMasterRequest request
        )
		{
            AsyncResult<Result.UpdateCurrentShowcaseMasterResult> result = null;
			await UpdateCurrentShowcaseMaster(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
    #else
		public UpdateCurrentShowcaseMasterTask UpdateCurrentShowcaseMasterAsync(
                Request.UpdateCurrentShowcaseMasterRequest request
        )
		{
			return new UpdateCurrentShowcaseMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.UpdateCurrentShowcaseMasterResult> UpdateCurrentShowcaseMasterAsync(
                Request.UpdateCurrentShowcaseMasterRequest request
        )
		{
			var task = new UpdateCurrentShowcaseMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class UpdateCurrentShowcaseMasterFromGitHubTask : Gs2RestSessionTask<UpdateCurrentShowcaseMasterFromGitHubRequest, UpdateCurrentShowcaseMasterFromGitHubResult>
        {
            public UpdateCurrentShowcaseMasterFromGitHubTask(IGs2Session session, RestSessionRequestFactory factory, UpdateCurrentShowcaseMasterFromGitHubRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(UpdateCurrentShowcaseMasterFromGitHubRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "showcase")
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
		public IEnumerator UpdateCurrentShowcaseMasterFromGitHub(
                Request.UpdateCurrentShowcaseMasterFromGitHubRequest request,
                UnityAction<AsyncResult<Result.UpdateCurrentShowcaseMasterFromGitHubResult>> callback
        )
		{
			var task = new UpdateCurrentShowcaseMasterFromGitHubTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.UpdateCurrentShowcaseMasterFromGitHubResult>(task.Result, task.Error));
        }

		public IFuture<Result.UpdateCurrentShowcaseMasterFromGitHubResult> UpdateCurrentShowcaseMasterFromGitHubFuture(
                Request.UpdateCurrentShowcaseMasterFromGitHubRequest request
        )
		{
			return new UpdateCurrentShowcaseMasterFromGitHubTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.UpdateCurrentShowcaseMasterFromGitHubResult> UpdateCurrentShowcaseMasterFromGitHubAsync(
                Request.UpdateCurrentShowcaseMasterFromGitHubRequest request
        )
		{
            AsyncResult<Result.UpdateCurrentShowcaseMasterFromGitHubResult> result = null;
			await UpdateCurrentShowcaseMasterFromGitHub(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
    #else
		public UpdateCurrentShowcaseMasterFromGitHubTask UpdateCurrentShowcaseMasterFromGitHubAsync(
                Request.UpdateCurrentShowcaseMasterFromGitHubRequest request
        )
		{
			return new UpdateCurrentShowcaseMasterFromGitHubTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.UpdateCurrentShowcaseMasterFromGitHubResult> UpdateCurrentShowcaseMasterFromGitHubAsync(
                Request.UpdateCurrentShowcaseMasterFromGitHubRequest request
        )
		{
			var task = new UpdateCurrentShowcaseMasterFromGitHubTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class DescribeShowcasesTask : Gs2RestSessionTask<DescribeShowcasesRequest, DescribeShowcasesResult>
        {
            public DescribeShowcasesTask(IGs2Session session, RestSessionRequestFactory factory, DescribeShowcasesRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DescribeShowcasesRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "showcase")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/me/showcase";

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
		public IEnumerator DescribeShowcases(
                Request.DescribeShowcasesRequest request,
                UnityAction<AsyncResult<Result.DescribeShowcasesResult>> callback
        )
		{
			var task = new DescribeShowcasesTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.DescribeShowcasesResult>(task.Result, task.Error));
        }

		public IFuture<Result.DescribeShowcasesResult> DescribeShowcasesFuture(
                Request.DescribeShowcasesRequest request
        )
		{
			return new DescribeShowcasesTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.DescribeShowcasesResult> DescribeShowcasesAsync(
                Request.DescribeShowcasesRequest request
        )
		{
            AsyncResult<Result.DescribeShowcasesResult> result = null;
			await DescribeShowcases(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
    #else
		public DescribeShowcasesTask DescribeShowcasesAsync(
                Request.DescribeShowcasesRequest request
        )
		{
			return new DescribeShowcasesTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.DescribeShowcasesResult> DescribeShowcasesAsync(
                Request.DescribeShowcasesRequest request
        )
		{
			var task = new DescribeShowcasesTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class DescribeShowcasesByUserIdTask : Gs2RestSessionTask<DescribeShowcasesByUserIdRequest, DescribeShowcasesByUserIdResult>
        {
            public DescribeShowcasesByUserIdTask(IGs2Session session, RestSessionRequestFactory factory, DescribeShowcasesByUserIdRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DescribeShowcasesByUserIdRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "showcase")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/{userId}/showcase";

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
		public IEnumerator DescribeShowcasesByUserId(
                Request.DescribeShowcasesByUserIdRequest request,
                UnityAction<AsyncResult<Result.DescribeShowcasesByUserIdResult>> callback
        )
		{
			var task = new DescribeShowcasesByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.DescribeShowcasesByUserIdResult>(task.Result, task.Error));
        }

		public IFuture<Result.DescribeShowcasesByUserIdResult> DescribeShowcasesByUserIdFuture(
                Request.DescribeShowcasesByUserIdRequest request
        )
		{
			return new DescribeShowcasesByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.DescribeShowcasesByUserIdResult> DescribeShowcasesByUserIdAsync(
                Request.DescribeShowcasesByUserIdRequest request
        )
		{
            AsyncResult<Result.DescribeShowcasesByUserIdResult> result = null;
			await DescribeShowcasesByUserId(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
    #else
		public DescribeShowcasesByUserIdTask DescribeShowcasesByUserIdAsync(
                Request.DescribeShowcasesByUserIdRequest request
        )
		{
			return new DescribeShowcasesByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.DescribeShowcasesByUserIdResult> DescribeShowcasesByUserIdAsync(
                Request.DescribeShowcasesByUserIdRequest request
        )
		{
			var task = new DescribeShowcasesByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class GetShowcaseTask : Gs2RestSessionTask<GetShowcaseRequest, GetShowcaseResult>
        {
            public GetShowcaseTask(IGs2Session session, RestSessionRequestFactory factory, GetShowcaseRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(GetShowcaseRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "showcase")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/me/showcase/{showcaseName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{showcaseName}", !string.IsNullOrEmpty(request.ShowcaseName) ? request.ShowcaseName.ToString() : "null");

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
		public IEnumerator GetShowcase(
                Request.GetShowcaseRequest request,
                UnityAction<AsyncResult<Result.GetShowcaseResult>> callback
        )
		{
			var task = new GetShowcaseTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.GetShowcaseResult>(task.Result, task.Error));
        }

		public IFuture<Result.GetShowcaseResult> GetShowcaseFuture(
                Request.GetShowcaseRequest request
        )
		{
			return new GetShowcaseTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.GetShowcaseResult> GetShowcaseAsync(
                Request.GetShowcaseRequest request
        )
		{
            AsyncResult<Result.GetShowcaseResult> result = null;
			await GetShowcase(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
    #else
		public GetShowcaseTask GetShowcaseAsync(
                Request.GetShowcaseRequest request
        )
		{
			return new GetShowcaseTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.GetShowcaseResult> GetShowcaseAsync(
                Request.GetShowcaseRequest request
        )
		{
			var task = new GetShowcaseTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class GetShowcaseByUserIdTask : Gs2RestSessionTask<GetShowcaseByUserIdRequest, GetShowcaseByUserIdResult>
        {
            public GetShowcaseByUserIdTask(IGs2Session session, RestSessionRequestFactory factory, GetShowcaseByUserIdRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(GetShowcaseByUserIdRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "showcase")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/{userId}/showcase/{showcaseName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{showcaseName}", !string.IsNullOrEmpty(request.ShowcaseName) ? request.ShowcaseName.ToString() : "null");
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
		public IEnumerator GetShowcaseByUserId(
                Request.GetShowcaseByUserIdRequest request,
                UnityAction<AsyncResult<Result.GetShowcaseByUserIdResult>> callback
        )
		{
			var task = new GetShowcaseByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.GetShowcaseByUserIdResult>(task.Result, task.Error));
        }

		public IFuture<Result.GetShowcaseByUserIdResult> GetShowcaseByUserIdFuture(
                Request.GetShowcaseByUserIdRequest request
        )
		{
			return new GetShowcaseByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.GetShowcaseByUserIdResult> GetShowcaseByUserIdAsync(
                Request.GetShowcaseByUserIdRequest request
        )
		{
            AsyncResult<Result.GetShowcaseByUserIdResult> result = null;
			await GetShowcaseByUserId(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
    #else
		public GetShowcaseByUserIdTask GetShowcaseByUserIdAsync(
                Request.GetShowcaseByUserIdRequest request
        )
		{
			return new GetShowcaseByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.GetShowcaseByUserIdResult> GetShowcaseByUserIdAsync(
                Request.GetShowcaseByUserIdRequest request
        )
		{
			var task = new GetShowcaseByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class BuyTask : Gs2RestSessionTask<BuyRequest, BuyResult>
        {
            public BuyTask(IGs2Session session, RestSessionRequestFactory factory, BuyRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(BuyRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "showcase")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/me/showcase/{showcaseName}/{displayItemId}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{showcaseName}", !string.IsNullOrEmpty(request.ShowcaseName) ? request.ShowcaseName.ToString() : "null");
                url = url.Replace("{displayItemId}", !string.IsNullOrEmpty(request.DisplayItemId) ? request.DisplayItemId.ToString() : "null");

                var sessionRequest = Factory.Post(url);

                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);
                jsonWriter.WriteObjectStart();
                if (request.Quantity != null)
                {
                    jsonWriter.WritePropertyName("quantity");
                    jsonWriter.Write(request.Quantity.ToString());
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
		public IEnumerator Buy(
                Request.BuyRequest request,
                UnityAction<AsyncResult<Result.BuyResult>> callback
        )
		{
			var task = new BuyTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.BuyResult>(task.Result, task.Error));
        }

		public IFuture<Result.BuyResult> BuyFuture(
                Request.BuyRequest request
        )
		{
			return new BuyTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.BuyResult> BuyAsync(
                Request.BuyRequest request
        )
		{
            AsyncResult<Result.BuyResult> result = null;
			await Buy(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
    #else
		public BuyTask BuyAsync(
                Request.BuyRequest request
        )
		{
			return new BuyTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.BuyResult> BuyAsync(
                Request.BuyRequest request
        )
		{
			var task = new BuyTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class BuyByUserIdTask : Gs2RestSessionTask<BuyByUserIdRequest, BuyByUserIdResult>
        {
            public BuyByUserIdTask(IGs2Session session, RestSessionRequestFactory factory, BuyByUserIdRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(BuyByUserIdRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "showcase")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/{userId}/showcase/{showcaseName}/{displayItemId}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{showcaseName}", !string.IsNullOrEmpty(request.ShowcaseName) ? request.ShowcaseName.ToString() : "null");
                url = url.Replace("{displayItemId}", !string.IsNullOrEmpty(request.DisplayItemId) ? request.DisplayItemId.ToString() : "null");
                url = url.Replace("{userId}", !string.IsNullOrEmpty(request.UserId) ? request.UserId.ToString() : "null");

                var sessionRequest = Factory.Post(url);

                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);
                jsonWriter.WriteObjectStart();
                if (request.Quantity != null)
                {
                    jsonWriter.WritePropertyName("quantity");
                    jsonWriter.Write(request.Quantity.ToString());
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
		public IEnumerator BuyByUserId(
                Request.BuyByUserIdRequest request,
                UnityAction<AsyncResult<Result.BuyByUserIdResult>> callback
        )
		{
			var task = new BuyByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.BuyByUserIdResult>(task.Result, task.Error));
        }

		public IFuture<Result.BuyByUserIdResult> BuyByUserIdFuture(
                Request.BuyByUserIdRequest request
        )
		{
			return new BuyByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.BuyByUserIdResult> BuyByUserIdAsync(
                Request.BuyByUserIdRequest request
        )
		{
            AsyncResult<Result.BuyByUserIdResult> result = null;
			await BuyByUserId(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
    #else
		public BuyByUserIdTask BuyByUserIdAsync(
                Request.BuyByUserIdRequest request
        )
		{
			return new BuyByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.BuyByUserIdResult> BuyByUserIdAsync(
                Request.BuyByUserIdRequest request
        )
		{
			var task = new BuyByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class DescribeRandomShowcaseMastersTask : Gs2RestSessionTask<DescribeRandomShowcaseMastersRequest, DescribeRandomShowcaseMastersResult>
        {
            public DescribeRandomShowcaseMastersTask(IGs2Session session, RestSessionRequestFactory factory, DescribeRandomShowcaseMastersRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DescribeRandomShowcaseMastersRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "showcase")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/master/random/showcase";

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
		public IEnumerator DescribeRandomShowcaseMasters(
                Request.DescribeRandomShowcaseMastersRequest request,
                UnityAction<AsyncResult<Result.DescribeRandomShowcaseMastersResult>> callback
        )
		{
			var task = new DescribeRandomShowcaseMastersTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.DescribeRandomShowcaseMastersResult>(task.Result, task.Error));
        }

		public IFuture<Result.DescribeRandomShowcaseMastersResult> DescribeRandomShowcaseMastersFuture(
                Request.DescribeRandomShowcaseMastersRequest request
        )
		{
			return new DescribeRandomShowcaseMastersTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.DescribeRandomShowcaseMastersResult> DescribeRandomShowcaseMastersAsync(
                Request.DescribeRandomShowcaseMastersRequest request
        )
		{
            AsyncResult<Result.DescribeRandomShowcaseMastersResult> result = null;
			await DescribeRandomShowcaseMasters(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
    #else
		public DescribeRandomShowcaseMastersTask DescribeRandomShowcaseMastersAsync(
                Request.DescribeRandomShowcaseMastersRequest request
        )
		{
			return new DescribeRandomShowcaseMastersTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.DescribeRandomShowcaseMastersResult> DescribeRandomShowcaseMastersAsync(
                Request.DescribeRandomShowcaseMastersRequest request
        )
		{
			var task = new DescribeRandomShowcaseMastersTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class CreateRandomShowcaseMasterTask : Gs2RestSessionTask<CreateRandomShowcaseMasterRequest, CreateRandomShowcaseMasterResult>
        {
            public CreateRandomShowcaseMasterTask(IGs2Session session, RestSessionRequestFactory factory, CreateRandomShowcaseMasterRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(CreateRandomShowcaseMasterRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "showcase")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/master/random/showcase";

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
                if (request.MaximumNumberOfChoice != null)
                {
                    jsonWriter.WritePropertyName("maximumNumberOfChoice");
                    jsonWriter.Write(request.MaximumNumberOfChoice.ToString());
                }
                if (request.DisplayItems != null)
                {
                    jsonWriter.WritePropertyName("displayItems");
                    jsonWriter.WriteArrayStart();
                    foreach(var item in request.DisplayItems)
                    {
                        item.WriteJson(jsonWriter);
                    }
                    jsonWriter.WriteArrayEnd();
                }
                if (request.BaseTimestamp != null)
                {
                    jsonWriter.WritePropertyName("baseTimestamp");
                    jsonWriter.Write(request.BaseTimestamp.ToString());
                }
                if (request.ResetIntervalHours != null)
                {
                    jsonWriter.WritePropertyName("resetIntervalHours");
                    jsonWriter.Write(request.ResetIntervalHours.ToString());
                }
                if (request.SalesPeriodEventId != null)
                {
                    jsonWriter.WritePropertyName("salesPeriodEventId");
                    jsonWriter.Write(request.SalesPeriodEventId);
                }
                if (request.ContextStack != null)
                {
                    jsonWriter.WritePropertyName("contextStack");
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
		public IEnumerator CreateRandomShowcaseMaster(
                Request.CreateRandomShowcaseMasterRequest request,
                UnityAction<AsyncResult<Result.CreateRandomShowcaseMasterResult>> callback
        )
		{
			var task = new CreateRandomShowcaseMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.CreateRandomShowcaseMasterResult>(task.Result, task.Error));
        }

		public IFuture<Result.CreateRandomShowcaseMasterResult> CreateRandomShowcaseMasterFuture(
                Request.CreateRandomShowcaseMasterRequest request
        )
		{
			return new CreateRandomShowcaseMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.CreateRandomShowcaseMasterResult> CreateRandomShowcaseMasterAsync(
                Request.CreateRandomShowcaseMasterRequest request
        )
		{
            AsyncResult<Result.CreateRandomShowcaseMasterResult> result = null;
			await CreateRandomShowcaseMaster(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
    #else
		public CreateRandomShowcaseMasterTask CreateRandomShowcaseMasterAsync(
                Request.CreateRandomShowcaseMasterRequest request
        )
		{
			return new CreateRandomShowcaseMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.CreateRandomShowcaseMasterResult> CreateRandomShowcaseMasterAsync(
                Request.CreateRandomShowcaseMasterRequest request
        )
		{
			var task = new CreateRandomShowcaseMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class GetRandomShowcaseMasterTask : Gs2RestSessionTask<GetRandomShowcaseMasterRequest, GetRandomShowcaseMasterResult>
        {
            public GetRandomShowcaseMasterTask(IGs2Session session, RestSessionRequestFactory factory, GetRandomShowcaseMasterRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(GetRandomShowcaseMasterRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "showcase")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/master/random/showcase/{showcaseName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{showcaseName}", !string.IsNullOrEmpty(request.ShowcaseName) ? request.ShowcaseName.ToString() : "null");

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
		public IEnumerator GetRandomShowcaseMaster(
                Request.GetRandomShowcaseMasterRequest request,
                UnityAction<AsyncResult<Result.GetRandomShowcaseMasterResult>> callback
        )
		{
			var task = new GetRandomShowcaseMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.GetRandomShowcaseMasterResult>(task.Result, task.Error));
        }

		public IFuture<Result.GetRandomShowcaseMasterResult> GetRandomShowcaseMasterFuture(
                Request.GetRandomShowcaseMasterRequest request
        )
		{
			return new GetRandomShowcaseMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.GetRandomShowcaseMasterResult> GetRandomShowcaseMasterAsync(
                Request.GetRandomShowcaseMasterRequest request
        )
		{
            AsyncResult<Result.GetRandomShowcaseMasterResult> result = null;
			await GetRandomShowcaseMaster(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
    #else
		public GetRandomShowcaseMasterTask GetRandomShowcaseMasterAsync(
                Request.GetRandomShowcaseMasterRequest request
        )
		{
			return new GetRandomShowcaseMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.GetRandomShowcaseMasterResult> GetRandomShowcaseMasterAsync(
                Request.GetRandomShowcaseMasterRequest request
        )
		{
			var task = new GetRandomShowcaseMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class UpdateRandomShowcaseMasterTask : Gs2RestSessionTask<UpdateRandomShowcaseMasterRequest, UpdateRandomShowcaseMasterResult>
        {
            public UpdateRandomShowcaseMasterTask(IGs2Session session, RestSessionRequestFactory factory, UpdateRandomShowcaseMasterRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(UpdateRandomShowcaseMasterRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "showcase")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/master/random/showcase/{showcaseName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{showcaseName}", !string.IsNullOrEmpty(request.ShowcaseName) ? request.ShowcaseName.ToString() : "null");

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
                if (request.MaximumNumberOfChoice != null)
                {
                    jsonWriter.WritePropertyName("maximumNumberOfChoice");
                    jsonWriter.Write(request.MaximumNumberOfChoice.ToString());
                }
                if (request.DisplayItems != null)
                {
                    jsonWriter.WritePropertyName("displayItems");
                    jsonWriter.WriteArrayStart();
                    foreach(var item in request.DisplayItems)
                    {
                        item.WriteJson(jsonWriter);
                    }
                    jsonWriter.WriteArrayEnd();
                }
                if (request.BaseTimestamp != null)
                {
                    jsonWriter.WritePropertyName("baseTimestamp");
                    jsonWriter.Write(request.BaseTimestamp.ToString());
                }
                if (request.ResetIntervalHours != null)
                {
                    jsonWriter.WritePropertyName("resetIntervalHours");
                    jsonWriter.Write(request.ResetIntervalHours.ToString());
                }
                if (request.SalesPeriodEventId != null)
                {
                    jsonWriter.WritePropertyName("salesPeriodEventId");
                    jsonWriter.Write(request.SalesPeriodEventId);
                }
                if (request.ContextStack != null)
                {
                    jsonWriter.WritePropertyName("contextStack");
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
		public IEnumerator UpdateRandomShowcaseMaster(
                Request.UpdateRandomShowcaseMasterRequest request,
                UnityAction<AsyncResult<Result.UpdateRandomShowcaseMasterResult>> callback
        )
		{
			var task = new UpdateRandomShowcaseMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.UpdateRandomShowcaseMasterResult>(task.Result, task.Error));
        }

		public IFuture<Result.UpdateRandomShowcaseMasterResult> UpdateRandomShowcaseMasterFuture(
                Request.UpdateRandomShowcaseMasterRequest request
        )
		{
			return new UpdateRandomShowcaseMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.UpdateRandomShowcaseMasterResult> UpdateRandomShowcaseMasterAsync(
                Request.UpdateRandomShowcaseMasterRequest request
        )
		{
            AsyncResult<Result.UpdateRandomShowcaseMasterResult> result = null;
			await UpdateRandomShowcaseMaster(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
    #else
		public UpdateRandomShowcaseMasterTask UpdateRandomShowcaseMasterAsync(
                Request.UpdateRandomShowcaseMasterRequest request
        )
		{
			return new UpdateRandomShowcaseMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.UpdateRandomShowcaseMasterResult> UpdateRandomShowcaseMasterAsync(
                Request.UpdateRandomShowcaseMasterRequest request
        )
		{
			var task = new UpdateRandomShowcaseMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class DeleteRandomShowcaseMasterTask : Gs2RestSessionTask<DeleteRandomShowcaseMasterRequest, DeleteRandomShowcaseMasterResult>
        {
            public DeleteRandomShowcaseMasterTask(IGs2Session session, RestSessionRequestFactory factory, DeleteRandomShowcaseMasterRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DeleteRandomShowcaseMasterRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "showcase")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/master/random/showcase/{showcaseName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{showcaseName}", !string.IsNullOrEmpty(request.ShowcaseName) ? request.ShowcaseName.ToString() : "null");

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
		public IEnumerator DeleteRandomShowcaseMaster(
                Request.DeleteRandomShowcaseMasterRequest request,
                UnityAction<AsyncResult<Result.DeleteRandomShowcaseMasterResult>> callback
        )
		{
			var task = new DeleteRandomShowcaseMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.DeleteRandomShowcaseMasterResult>(task.Result, task.Error));
        }

		public IFuture<Result.DeleteRandomShowcaseMasterResult> DeleteRandomShowcaseMasterFuture(
                Request.DeleteRandomShowcaseMasterRequest request
        )
		{
			return new DeleteRandomShowcaseMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.DeleteRandomShowcaseMasterResult> DeleteRandomShowcaseMasterAsync(
                Request.DeleteRandomShowcaseMasterRequest request
        )
		{
            AsyncResult<Result.DeleteRandomShowcaseMasterResult> result = null;
			await DeleteRandomShowcaseMaster(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
    #else
		public DeleteRandomShowcaseMasterTask DeleteRandomShowcaseMasterAsync(
                Request.DeleteRandomShowcaseMasterRequest request
        )
		{
			return new DeleteRandomShowcaseMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.DeleteRandomShowcaseMasterResult> DeleteRandomShowcaseMasterAsync(
                Request.DeleteRandomShowcaseMasterRequest request
        )
		{
			var task = new DeleteRandomShowcaseMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class IncrementPurchaseCountTask : Gs2RestSessionTask<IncrementPurchaseCountRequest, IncrementPurchaseCountResult>
        {
            public IncrementPurchaseCountTask(IGs2Session session, RestSessionRequestFactory factory, IncrementPurchaseCountRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(IncrementPurchaseCountRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "showcase")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/random/showcase/user/me/status/{showcaseName}/{displayItemName}/purchase/count";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{showcaseName}", !string.IsNullOrEmpty(request.ShowcaseName) ? request.ShowcaseName.ToString() : "null");
                url = url.Replace("{displayItemName}", !string.IsNullOrEmpty(request.DisplayItemName) ? request.DisplayItemName.ToString() : "null");

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
		public IEnumerator IncrementPurchaseCount(
                Request.IncrementPurchaseCountRequest request,
                UnityAction<AsyncResult<Result.IncrementPurchaseCountResult>> callback
        )
		{
			var task = new IncrementPurchaseCountTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.IncrementPurchaseCountResult>(task.Result, task.Error));
        }

		public IFuture<Result.IncrementPurchaseCountResult> IncrementPurchaseCountFuture(
                Request.IncrementPurchaseCountRequest request
        )
		{
			return new IncrementPurchaseCountTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.IncrementPurchaseCountResult> IncrementPurchaseCountAsync(
                Request.IncrementPurchaseCountRequest request
        )
		{
            AsyncResult<Result.IncrementPurchaseCountResult> result = null;
			await IncrementPurchaseCount(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
    #else
		public IncrementPurchaseCountTask IncrementPurchaseCountAsync(
                Request.IncrementPurchaseCountRequest request
        )
		{
			return new IncrementPurchaseCountTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.IncrementPurchaseCountResult> IncrementPurchaseCountAsync(
                Request.IncrementPurchaseCountRequest request
        )
		{
			var task = new IncrementPurchaseCountTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class IncrementPurchaseCountByUserIdTask : Gs2RestSessionTask<IncrementPurchaseCountByUserIdRequest, IncrementPurchaseCountByUserIdResult>
        {
            public IncrementPurchaseCountByUserIdTask(IGs2Session session, RestSessionRequestFactory factory, IncrementPurchaseCountByUserIdRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(IncrementPurchaseCountByUserIdRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "showcase")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/random/showcase/user/{userId}/status/{showcaseName}/{displayItemName}/purchase/count";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{showcaseName}", !string.IsNullOrEmpty(request.ShowcaseName) ? request.ShowcaseName.ToString() : "null");
                url = url.Replace("{displayItemName}", !string.IsNullOrEmpty(request.DisplayItemName) ? request.DisplayItemName.ToString() : "null");
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
                if (request.ContextStack != null)
                {
                    jsonWriter.WritePropertyName("contextStack");
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
		public IEnumerator IncrementPurchaseCountByUserId(
                Request.IncrementPurchaseCountByUserIdRequest request,
                UnityAction<AsyncResult<Result.IncrementPurchaseCountByUserIdResult>> callback
        )
		{
			var task = new IncrementPurchaseCountByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.IncrementPurchaseCountByUserIdResult>(task.Result, task.Error));
        }

		public IFuture<Result.IncrementPurchaseCountByUserIdResult> IncrementPurchaseCountByUserIdFuture(
                Request.IncrementPurchaseCountByUserIdRequest request
        )
		{
			return new IncrementPurchaseCountByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.IncrementPurchaseCountByUserIdResult> IncrementPurchaseCountByUserIdAsync(
                Request.IncrementPurchaseCountByUserIdRequest request
        )
		{
            AsyncResult<Result.IncrementPurchaseCountByUserIdResult> result = null;
			await IncrementPurchaseCountByUserId(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
    #else
		public IncrementPurchaseCountByUserIdTask IncrementPurchaseCountByUserIdAsync(
                Request.IncrementPurchaseCountByUserIdRequest request
        )
		{
			return new IncrementPurchaseCountByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.IncrementPurchaseCountByUserIdResult> IncrementPurchaseCountByUserIdAsync(
                Request.IncrementPurchaseCountByUserIdRequest request
        )
		{
			var task = new IncrementPurchaseCountByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class DecrementPurchaseCountByUserIdTask : Gs2RestSessionTask<DecrementPurchaseCountByUserIdRequest, DecrementPurchaseCountByUserIdResult>
        {
            public DecrementPurchaseCountByUserIdTask(IGs2Session session, RestSessionRequestFactory factory, DecrementPurchaseCountByUserIdRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DecrementPurchaseCountByUserIdRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "showcase")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/random/showcase/user/{userId}/status/{showcaseName}/{displayItemName}/purchase/count/decrease";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{showcaseName}", !string.IsNullOrEmpty(request.ShowcaseName) ? request.ShowcaseName.ToString() : "null");
                url = url.Replace("{displayItemName}", !string.IsNullOrEmpty(request.DisplayItemName) ? request.DisplayItemName.ToString() : "null");
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
                if (request.ContextStack != null)
                {
                    jsonWriter.WritePropertyName("contextStack");
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
		public IEnumerator DecrementPurchaseCountByUserId(
                Request.DecrementPurchaseCountByUserIdRequest request,
                UnityAction<AsyncResult<Result.DecrementPurchaseCountByUserIdResult>> callback
        )
		{
			var task = new DecrementPurchaseCountByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.DecrementPurchaseCountByUserIdResult>(task.Result, task.Error));
        }

		public IFuture<Result.DecrementPurchaseCountByUserIdResult> DecrementPurchaseCountByUserIdFuture(
                Request.DecrementPurchaseCountByUserIdRequest request
        )
		{
			return new DecrementPurchaseCountByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.DecrementPurchaseCountByUserIdResult> DecrementPurchaseCountByUserIdAsync(
                Request.DecrementPurchaseCountByUserIdRequest request
        )
		{
            AsyncResult<Result.DecrementPurchaseCountByUserIdResult> result = null;
			await DecrementPurchaseCountByUserId(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
    #else
		public DecrementPurchaseCountByUserIdTask DecrementPurchaseCountByUserIdAsync(
                Request.DecrementPurchaseCountByUserIdRequest request
        )
		{
			return new DecrementPurchaseCountByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.DecrementPurchaseCountByUserIdResult> DecrementPurchaseCountByUserIdAsync(
                Request.DecrementPurchaseCountByUserIdRequest request
        )
		{
			var task = new DecrementPurchaseCountByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class IncrementPurchaseCountByStampTaskTask : Gs2RestSessionTask<IncrementPurchaseCountByStampTaskRequest, IncrementPurchaseCountByStampTaskResult>
        {
            public IncrementPurchaseCountByStampTaskTask(IGs2Session session, RestSessionRequestFactory factory, IncrementPurchaseCountByStampTaskRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(IncrementPurchaseCountByStampTaskRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "showcase")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/stamp/random/showcase/status/purchase/count";

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
		public IEnumerator IncrementPurchaseCountByStampTask(
                Request.IncrementPurchaseCountByStampTaskRequest request,
                UnityAction<AsyncResult<Result.IncrementPurchaseCountByStampTaskResult>> callback
        )
		{
			var task = new IncrementPurchaseCountByStampTaskTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.IncrementPurchaseCountByStampTaskResult>(task.Result, task.Error));
        }

		public IFuture<Result.IncrementPurchaseCountByStampTaskResult> IncrementPurchaseCountByStampTaskFuture(
                Request.IncrementPurchaseCountByStampTaskRequest request
        )
		{
			return new IncrementPurchaseCountByStampTaskTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.IncrementPurchaseCountByStampTaskResult> IncrementPurchaseCountByStampTaskAsync(
                Request.IncrementPurchaseCountByStampTaskRequest request
        )
		{
            AsyncResult<Result.IncrementPurchaseCountByStampTaskResult> result = null;
			await IncrementPurchaseCountByStampTask(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
    #else
		public IncrementPurchaseCountByStampTaskTask IncrementPurchaseCountByStampTaskAsync(
                Request.IncrementPurchaseCountByStampTaskRequest request
        )
		{
			return new IncrementPurchaseCountByStampTaskTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.IncrementPurchaseCountByStampTaskResult> IncrementPurchaseCountByStampTaskAsync(
                Request.IncrementPurchaseCountByStampTaskRequest request
        )
		{
			var task = new IncrementPurchaseCountByStampTaskTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class DecrementPurchaseCountByStampSheetTask : Gs2RestSessionTask<DecrementPurchaseCountByStampSheetRequest, DecrementPurchaseCountByStampSheetResult>
        {
            public DecrementPurchaseCountByStampSheetTask(IGs2Session session, RestSessionRequestFactory factory, DecrementPurchaseCountByStampSheetRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DecrementPurchaseCountByStampSheetRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "showcase")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/stamp/random/showcase/status/purchase/count/decrease";

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
		public IEnumerator DecrementPurchaseCountByStampSheet(
                Request.DecrementPurchaseCountByStampSheetRequest request,
                UnityAction<AsyncResult<Result.DecrementPurchaseCountByStampSheetResult>> callback
        )
		{
			var task = new DecrementPurchaseCountByStampSheetTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.DecrementPurchaseCountByStampSheetResult>(task.Result, task.Error));
        }

		public IFuture<Result.DecrementPurchaseCountByStampSheetResult> DecrementPurchaseCountByStampSheetFuture(
                Request.DecrementPurchaseCountByStampSheetRequest request
        )
		{
			return new DecrementPurchaseCountByStampSheetTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.DecrementPurchaseCountByStampSheetResult> DecrementPurchaseCountByStampSheetAsync(
                Request.DecrementPurchaseCountByStampSheetRequest request
        )
		{
            AsyncResult<Result.DecrementPurchaseCountByStampSheetResult> result = null;
			await DecrementPurchaseCountByStampSheet(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
    #else
		public DecrementPurchaseCountByStampSheetTask DecrementPurchaseCountByStampSheetAsync(
                Request.DecrementPurchaseCountByStampSheetRequest request
        )
		{
			return new DecrementPurchaseCountByStampSheetTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.DecrementPurchaseCountByStampSheetResult> DecrementPurchaseCountByStampSheetAsync(
                Request.DecrementPurchaseCountByStampSheetRequest request
        )
		{
			var task = new DecrementPurchaseCountByStampSheetTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class ForceReDrawByUserIdTask : Gs2RestSessionTask<ForceReDrawByUserIdRequest, ForceReDrawByUserIdResult>
        {
            public ForceReDrawByUserIdTask(IGs2Session session, RestSessionRequestFactory factory, ForceReDrawByUserIdRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(ForceReDrawByUserIdRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "showcase")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/random/showcase/{showcaseName}/user/{userId}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{showcaseName}", !string.IsNullOrEmpty(request.ShowcaseName) ? request.ShowcaseName.ToString() : "null");
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
		public IEnumerator ForceReDrawByUserId(
                Request.ForceReDrawByUserIdRequest request,
                UnityAction<AsyncResult<Result.ForceReDrawByUserIdResult>> callback
        )
		{
			var task = new ForceReDrawByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.ForceReDrawByUserIdResult>(task.Result, task.Error));
        }

		public IFuture<Result.ForceReDrawByUserIdResult> ForceReDrawByUserIdFuture(
                Request.ForceReDrawByUserIdRequest request
        )
		{
			return new ForceReDrawByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.ForceReDrawByUserIdResult> ForceReDrawByUserIdAsync(
                Request.ForceReDrawByUserIdRequest request
        )
		{
            AsyncResult<Result.ForceReDrawByUserIdResult> result = null;
			await ForceReDrawByUserId(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
    #else
		public ForceReDrawByUserIdTask ForceReDrawByUserIdAsync(
                Request.ForceReDrawByUserIdRequest request
        )
		{
			return new ForceReDrawByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.ForceReDrawByUserIdResult> ForceReDrawByUserIdAsync(
                Request.ForceReDrawByUserIdRequest request
        )
		{
			var task = new ForceReDrawByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class ForceReDrawByUserIdByStampSheetTask : Gs2RestSessionTask<ForceReDrawByUserIdByStampSheetRequest, ForceReDrawByUserIdByStampSheetResult>
        {
            public ForceReDrawByUserIdByStampSheetTask(IGs2Session session, RestSessionRequestFactory factory, ForceReDrawByUserIdByStampSheetRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(ForceReDrawByUserIdByStampSheetRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "showcase")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/stamp/random/showcase/status/redraw";

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
		public IEnumerator ForceReDrawByUserIdByStampSheet(
                Request.ForceReDrawByUserIdByStampSheetRequest request,
                UnityAction<AsyncResult<Result.ForceReDrawByUserIdByStampSheetResult>> callback
        )
		{
			var task = new ForceReDrawByUserIdByStampSheetTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.ForceReDrawByUserIdByStampSheetResult>(task.Result, task.Error));
        }

		public IFuture<Result.ForceReDrawByUserIdByStampSheetResult> ForceReDrawByUserIdByStampSheetFuture(
                Request.ForceReDrawByUserIdByStampSheetRequest request
        )
		{
			return new ForceReDrawByUserIdByStampSheetTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.ForceReDrawByUserIdByStampSheetResult> ForceReDrawByUserIdByStampSheetAsync(
                Request.ForceReDrawByUserIdByStampSheetRequest request
        )
		{
            AsyncResult<Result.ForceReDrawByUserIdByStampSheetResult> result = null;
			await ForceReDrawByUserIdByStampSheet(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
    #else
		public ForceReDrawByUserIdByStampSheetTask ForceReDrawByUserIdByStampSheetAsync(
                Request.ForceReDrawByUserIdByStampSheetRequest request
        )
		{
			return new ForceReDrawByUserIdByStampSheetTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.ForceReDrawByUserIdByStampSheetResult> ForceReDrawByUserIdByStampSheetAsync(
                Request.ForceReDrawByUserIdByStampSheetRequest request
        )
		{
			var task = new ForceReDrawByUserIdByStampSheetTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class DescribeRandomDisplayItemsTask : Gs2RestSessionTask<DescribeRandomDisplayItemsRequest, DescribeRandomDisplayItemsResult>
        {
            public DescribeRandomDisplayItemsTask(IGs2Session session, RestSessionRequestFactory factory, DescribeRandomDisplayItemsRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DescribeRandomDisplayItemsRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "showcase")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/me/random/showcase/{showcaseName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{showcaseName}", !string.IsNullOrEmpty(request.ShowcaseName) ? request.ShowcaseName.ToString() : "null");

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
		public IEnumerator DescribeRandomDisplayItems(
                Request.DescribeRandomDisplayItemsRequest request,
                UnityAction<AsyncResult<Result.DescribeRandomDisplayItemsResult>> callback
        )
		{
			var task = new DescribeRandomDisplayItemsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.DescribeRandomDisplayItemsResult>(task.Result, task.Error));
        }

		public IFuture<Result.DescribeRandomDisplayItemsResult> DescribeRandomDisplayItemsFuture(
                Request.DescribeRandomDisplayItemsRequest request
        )
		{
			return new DescribeRandomDisplayItemsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.DescribeRandomDisplayItemsResult> DescribeRandomDisplayItemsAsync(
                Request.DescribeRandomDisplayItemsRequest request
        )
		{
            AsyncResult<Result.DescribeRandomDisplayItemsResult> result = null;
			await DescribeRandomDisplayItems(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
    #else
		public DescribeRandomDisplayItemsTask DescribeRandomDisplayItemsAsync(
                Request.DescribeRandomDisplayItemsRequest request
        )
		{
			return new DescribeRandomDisplayItemsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.DescribeRandomDisplayItemsResult> DescribeRandomDisplayItemsAsync(
                Request.DescribeRandomDisplayItemsRequest request
        )
		{
			var task = new DescribeRandomDisplayItemsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class DescribeRandomDisplayItemsByUserIdTask : Gs2RestSessionTask<DescribeRandomDisplayItemsByUserIdRequest, DescribeRandomDisplayItemsByUserIdResult>
        {
            public DescribeRandomDisplayItemsByUserIdTask(IGs2Session session, RestSessionRequestFactory factory, DescribeRandomDisplayItemsByUserIdRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DescribeRandomDisplayItemsByUserIdRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "showcase")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/{userId}/random/showcase/{showcaseName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{showcaseName}", !string.IsNullOrEmpty(request.ShowcaseName) ? request.ShowcaseName.ToString() : "null");
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
		public IEnumerator DescribeRandomDisplayItemsByUserId(
                Request.DescribeRandomDisplayItemsByUserIdRequest request,
                UnityAction<AsyncResult<Result.DescribeRandomDisplayItemsByUserIdResult>> callback
        )
		{
			var task = new DescribeRandomDisplayItemsByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.DescribeRandomDisplayItemsByUserIdResult>(task.Result, task.Error));
        }

		public IFuture<Result.DescribeRandomDisplayItemsByUserIdResult> DescribeRandomDisplayItemsByUserIdFuture(
                Request.DescribeRandomDisplayItemsByUserIdRequest request
        )
		{
			return new DescribeRandomDisplayItemsByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.DescribeRandomDisplayItemsByUserIdResult> DescribeRandomDisplayItemsByUserIdAsync(
                Request.DescribeRandomDisplayItemsByUserIdRequest request
        )
		{
            AsyncResult<Result.DescribeRandomDisplayItemsByUserIdResult> result = null;
			await DescribeRandomDisplayItemsByUserId(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
    #else
		public DescribeRandomDisplayItemsByUserIdTask DescribeRandomDisplayItemsByUserIdAsync(
                Request.DescribeRandomDisplayItemsByUserIdRequest request
        )
		{
			return new DescribeRandomDisplayItemsByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.DescribeRandomDisplayItemsByUserIdResult> DescribeRandomDisplayItemsByUserIdAsync(
                Request.DescribeRandomDisplayItemsByUserIdRequest request
        )
		{
			var task = new DescribeRandomDisplayItemsByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class GetRandomDisplayItemTask : Gs2RestSessionTask<GetRandomDisplayItemRequest, GetRandomDisplayItemResult>
        {
            public GetRandomDisplayItemTask(IGs2Session session, RestSessionRequestFactory factory, GetRandomDisplayItemRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(GetRandomDisplayItemRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "showcase")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/me/random/showcase/{showcaseName}/displayItem/{displayItemName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{showcaseName}", !string.IsNullOrEmpty(request.ShowcaseName) ? request.ShowcaseName.ToString() : "null");
                url = url.Replace("{displayItemName}", !string.IsNullOrEmpty(request.DisplayItemName) ? request.DisplayItemName.ToString() : "null");

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
		public IEnumerator GetRandomDisplayItem(
                Request.GetRandomDisplayItemRequest request,
                UnityAction<AsyncResult<Result.GetRandomDisplayItemResult>> callback
        )
		{
			var task = new GetRandomDisplayItemTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.GetRandomDisplayItemResult>(task.Result, task.Error));
        }

		public IFuture<Result.GetRandomDisplayItemResult> GetRandomDisplayItemFuture(
                Request.GetRandomDisplayItemRequest request
        )
		{
			return new GetRandomDisplayItemTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.GetRandomDisplayItemResult> GetRandomDisplayItemAsync(
                Request.GetRandomDisplayItemRequest request
        )
		{
            AsyncResult<Result.GetRandomDisplayItemResult> result = null;
			await GetRandomDisplayItem(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
    #else
		public GetRandomDisplayItemTask GetRandomDisplayItemAsync(
                Request.GetRandomDisplayItemRequest request
        )
		{
			return new GetRandomDisplayItemTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.GetRandomDisplayItemResult> GetRandomDisplayItemAsync(
                Request.GetRandomDisplayItemRequest request
        )
		{
			var task = new GetRandomDisplayItemTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class GetRandomDisplayItemByUserIdTask : Gs2RestSessionTask<GetRandomDisplayItemByUserIdRequest, GetRandomDisplayItemByUserIdResult>
        {
            public GetRandomDisplayItemByUserIdTask(IGs2Session session, RestSessionRequestFactory factory, GetRandomDisplayItemByUserIdRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(GetRandomDisplayItemByUserIdRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "showcase")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/{userId}/random/showcase/{showcaseName}/displayItem/{displayItemName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{showcaseName}", !string.IsNullOrEmpty(request.ShowcaseName) ? request.ShowcaseName.ToString() : "null");
                url = url.Replace("{displayItemName}", !string.IsNullOrEmpty(request.DisplayItemName) ? request.DisplayItemName.ToString() : "null");
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
		public IEnumerator GetRandomDisplayItemByUserId(
                Request.GetRandomDisplayItemByUserIdRequest request,
                UnityAction<AsyncResult<Result.GetRandomDisplayItemByUserIdResult>> callback
        )
		{
			var task = new GetRandomDisplayItemByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.GetRandomDisplayItemByUserIdResult>(task.Result, task.Error));
        }

		public IFuture<Result.GetRandomDisplayItemByUserIdResult> GetRandomDisplayItemByUserIdFuture(
                Request.GetRandomDisplayItemByUserIdRequest request
        )
		{
			return new GetRandomDisplayItemByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.GetRandomDisplayItemByUserIdResult> GetRandomDisplayItemByUserIdAsync(
                Request.GetRandomDisplayItemByUserIdRequest request
        )
		{
            AsyncResult<Result.GetRandomDisplayItemByUserIdResult> result = null;
			await GetRandomDisplayItemByUserId(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
    #else
		public GetRandomDisplayItemByUserIdTask GetRandomDisplayItemByUserIdAsync(
                Request.GetRandomDisplayItemByUserIdRequest request
        )
		{
			return new GetRandomDisplayItemByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.GetRandomDisplayItemByUserIdResult> GetRandomDisplayItemByUserIdAsync(
                Request.GetRandomDisplayItemByUserIdRequest request
        )
		{
			var task = new GetRandomDisplayItemByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class RandomShowcaseBuyTask : Gs2RestSessionTask<RandomShowcaseBuyRequest, RandomShowcaseBuyResult>
        {
            public RandomShowcaseBuyTask(IGs2Session session, RestSessionRequestFactory factory, RandomShowcaseBuyRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(RandomShowcaseBuyRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "showcase")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/me/random/showcase/{showcaseName}/{displayItemName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{showcaseName}", !string.IsNullOrEmpty(request.ShowcaseName) ? request.ShowcaseName.ToString() : "null");
                url = url.Replace("{displayItemName}", !string.IsNullOrEmpty(request.DisplayItemName) ? request.DisplayItemName.ToString() : "null");

                var sessionRequest = Factory.Post(url);

                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);
                jsonWriter.WriteObjectStart();
                if (request.Quantity != null)
                {
                    jsonWriter.WritePropertyName("quantity");
                    jsonWriter.Write(request.Quantity.ToString());
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
		public IEnumerator RandomShowcaseBuy(
                Request.RandomShowcaseBuyRequest request,
                UnityAction<AsyncResult<Result.RandomShowcaseBuyResult>> callback
        )
		{
			var task = new RandomShowcaseBuyTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.RandomShowcaseBuyResult>(task.Result, task.Error));
        }

		public IFuture<Result.RandomShowcaseBuyResult> RandomShowcaseBuyFuture(
                Request.RandomShowcaseBuyRequest request
        )
		{
			return new RandomShowcaseBuyTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.RandomShowcaseBuyResult> RandomShowcaseBuyAsync(
                Request.RandomShowcaseBuyRequest request
        )
		{
            AsyncResult<Result.RandomShowcaseBuyResult> result = null;
			await RandomShowcaseBuy(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
    #else
		public RandomShowcaseBuyTask RandomShowcaseBuyAsync(
                Request.RandomShowcaseBuyRequest request
        )
		{
			return new RandomShowcaseBuyTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.RandomShowcaseBuyResult> RandomShowcaseBuyAsync(
                Request.RandomShowcaseBuyRequest request
        )
		{
			var task = new RandomShowcaseBuyTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class RandomShowcaseBuyByUserIdTask : Gs2RestSessionTask<RandomShowcaseBuyByUserIdRequest, RandomShowcaseBuyByUserIdResult>
        {
            public RandomShowcaseBuyByUserIdTask(IGs2Session session, RestSessionRequestFactory factory, RandomShowcaseBuyByUserIdRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(RandomShowcaseBuyByUserIdRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "showcase")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/{userId}/random/showcase/{showcaseName}/{displayItemName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{showcaseName}", !string.IsNullOrEmpty(request.ShowcaseName) ? request.ShowcaseName.ToString() : "null");
                url = url.Replace("{displayItemName}", !string.IsNullOrEmpty(request.DisplayItemName) ? request.DisplayItemName.ToString() : "null");
                url = url.Replace("{userId}", !string.IsNullOrEmpty(request.UserId) ? request.UserId.ToString() : "null");

                var sessionRequest = Factory.Post(url);

                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);
                jsonWriter.WriteObjectStart();
                if (request.Quantity != null)
                {
                    jsonWriter.WritePropertyName("quantity");
                    jsonWriter.Write(request.Quantity.ToString());
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
		public IEnumerator RandomShowcaseBuyByUserId(
                Request.RandomShowcaseBuyByUserIdRequest request,
                UnityAction<AsyncResult<Result.RandomShowcaseBuyByUserIdResult>> callback
        )
		{
			var task = new RandomShowcaseBuyByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.RandomShowcaseBuyByUserIdResult>(task.Result, task.Error));
        }

		public IFuture<Result.RandomShowcaseBuyByUserIdResult> RandomShowcaseBuyByUserIdFuture(
                Request.RandomShowcaseBuyByUserIdRequest request
        )
		{
			return new RandomShowcaseBuyByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.RandomShowcaseBuyByUserIdResult> RandomShowcaseBuyByUserIdAsync(
                Request.RandomShowcaseBuyByUserIdRequest request
        )
		{
            AsyncResult<Result.RandomShowcaseBuyByUserIdResult> result = null;
			await RandomShowcaseBuyByUserId(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
    #else
		public RandomShowcaseBuyByUserIdTask RandomShowcaseBuyByUserIdAsync(
                Request.RandomShowcaseBuyByUserIdRequest request
        )
		{
			return new RandomShowcaseBuyByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.RandomShowcaseBuyByUserIdResult> RandomShowcaseBuyByUserIdAsync(
                Request.RandomShowcaseBuyByUserIdRequest request
        )
		{
			var task = new RandomShowcaseBuyByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif
	}
}