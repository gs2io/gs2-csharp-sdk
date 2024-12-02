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
using Gs2.Gs2Formation.Request;
using Gs2.Gs2Formation.Result;
using Gs2.Util.LitJson;

namespace Gs2.Gs2Formation
{
	public class Gs2FormationRestClient : AbstractGs2Client
	{
#if UNITY_2017_1_OR_NEWER
		private readonly CertificateHandler _certificateHandler;
#endif

		public static string Endpoint = "formation";

        protected Gs2RestSession Gs2RestSession => (Gs2RestSession) Gs2Session;

		public Gs2FormationRestClient(Gs2RestSession Gs2RestSession) : base(Gs2RestSession)
		{

		}

#if UNITY_2017_1_OR_NEWER
		public Gs2FormationRestClient(Gs2RestSession gs2RestSession, CertificateHandler certificateHandler) : base(gs2RestSession)
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
                    .Replace("{service}", "formation")
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
                    .Replace("{service}", "formation")
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
                if (request.UpdateMoldScript != null)
                {
                    jsonWriter.WritePropertyName("updateMoldScript");
                    request.UpdateMoldScript.WriteJson(jsonWriter);
                }
                if (request.UpdateFormScript != null)
                {
                    jsonWriter.WritePropertyName("updateFormScript");
                    request.UpdateFormScript.WriteJson(jsonWriter);
                }
                if (request.UpdatePropertyFormScript != null)
                {
                    jsonWriter.WritePropertyName("updatePropertyFormScript");
                    request.UpdatePropertyFormScript.WriteJson(jsonWriter);
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
                    .Replace("{service}", "formation")
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
                    .Replace("{service}", "formation")
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
                    .Replace("{service}", "formation")
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
                if (request.UpdateMoldScript != null)
                {
                    jsonWriter.WritePropertyName("updateMoldScript");
                    request.UpdateMoldScript.WriteJson(jsonWriter);
                }
                if (request.UpdateFormScript != null)
                {
                    jsonWriter.WritePropertyName("updateFormScript");
                    request.UpdateFormScript.WriteJson(jsonWriter);
                }
                if (request.UpdatePropertyFormScript != null)
                {
                    jsonWriter.WritePropertyName("updatePropertyFormScript");
                    request.UpdatePropertyFormScript.WriteJson(jsonWriter);
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
                    .Replace("{service}", "formation")
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
                    .Replace("{service}", "formation")
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
                    .Replace("{service}", "formation")
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
                    .Replace("{service}", "formation")
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
                    .Replace("{service}", "formation")
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
                    .Replace("{service}", "formation")
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
                    .Replace("{service}", "formation")
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
                    .Replace("{service}", "formation")
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


        public class GetFormModelTask : Gs2RestSessionTask<GetFormModelRequest, GetFormModelResult>
        {
            public GetFormModelTask(IGs2Session session, RestSessionRequestFactory factory, GetFormModelRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(GetFormModelRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "formation")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/model/{moldModelName}/form";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{moldModelName}", !string.IsNullOrEmpty(request.MoldModelName) ? request.MoldModelName.ToString() : "null");

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
		public IEnumerator GetFormModel(
                Request.GetFormModelRequest request,
                UnityAction<AsyncResult<Result.GetFormModelResult>> callback
        )
		{
			var task = new GetFormModelTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.GetFormModelResult>(task.Result, task.Error));
        }

		public IFuture<Result.GetFormModelResult> GetFormModelFuture(
                Request.GetFormModelRequest request
        )
		{
			return new GetFormModelTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.GetFormModelResult> GetFormModelAsync(
                Request.GetFormModelRequest request
        )
		{
            AsyncResult<Result.GetFormModelResult> result = null;
			await GetFormModel(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
    #else
		public GetFormModelTask GetFormModelAsync(
                Request.GetFormModelRequest request
        )
		{
			return new GetFormModelTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.GetFormModelResult> GetFormModelAsync(
                Request.GetFormModelRequest request
        )
		{
			var task = new GetFormModelTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class DescribeFormModelMastersTask : Gs2RestSessionTask<DescribeFormModelMastersRequest, DescribeFormModelMastersResult>
        {
            public DescribeFormModelMastersTask(IGs2Session session, RestSessionRequestFactory factory, DescribeFormModelMastersRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DescribeFormModelMastersRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "formation")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/master/model/form";

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
		public IEnumerator DescribeFormModelMasters(
                Request.DescribeFormModelMastersRequest request,
                UnityAction<AsyncResult<Result.DescribeFormModelMastersResult>> callback
        )
		{
			var task = new DescribeFormModelMastersTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.DescribeFormModelMastersResult>(task.Result, task.Error));
        }

		public IFuture<Result.DescribeFormModelMastersResult> DescribeFormModelMastersFuture(
                Request.DescribeFormModelMastersRequest request
        )
		{
			return new DescribeFormModelMastersTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.DescribeFormModelMastersResult> DescribeFormModelMastersAsync(
                Request.DescribeFormModelMastersRequest request
        )
		{
            AsyncResult<Result.DescribeFormModelMastersResult> result = null;
			await DescribeFormModelMasters(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
    #else
		public DescribeFormModelMastersTask DescribeFormModelMastersAsync(
                Request.DescribeFormModelMastersRequest request
        )
		{
			return new DescribeFormModelMastersTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.DescribeFormModelMastersResult> DescribeFormModelMastersAsync(
                Request.DescribeFormModelMastersRequest request
        )
		{
			var task = new DescribeFormModelMastersTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class CreateFormModelMasterTask : Gs2RestSessionTask<CreateFormModelMasterRequest, CreateFormModelMasterResult>
        {
            public CreateFormModelMasterTask(IGs2Session session, RestSessionRequestFactory factory, CreateFormModelMasterRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(CreateFormModelMasterRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "formation")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/master/model/form";

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
                if (request.Slots != null)
                {
                    jsonWriter.WritePropertyName("slots");
                    jsonWriter.WriteArrayStart();
                    foreach(var item in request.Slots)
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
		public IEnumerator CreateFormModelMaster(
                Request.CreateFormModelMasterRequest request,
                UnityAction<AsyncResult<Result.CreateFormModelMasterResult>> callback
        )
		{
			var task = new CreateFormModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.CreateFormModelMasterResult>(task.Result, task.Error));
        }

		public IFuture<Result.CreateFormModelMasterResult> CreateFormModelMasterFuture(
                Request.CreateFormModelMasterRequest request
        )
		{
			return new CreateFormModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.CreateFormModelMasterResult> CreateFormModelMasterAsync(
                Request.CreateFormModelMasterRequest request
        )
		{
            AsyncResult<Result.CreateFormModelMasterResult> result = null;
			await CreateFormModelMaster(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
    #else
		public CreateFormModelMasterTask CreateFormModelMasterAsync(
                Request.CreateFormModelMasterRequest request
        )
		{
			return new CreateFormModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.CreateFormModelMasterResult> CreateFormModelMasterAsync(
                Request.CreateFormModelMasterRequest request
        )
		{
			var task = new CreateFormModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class GetFormModelMasterTask : Gs2RestSessionTask<GetFormModelMasterRequest, GetFormModelMasterResult>
        {
            public GetFormModelMasterTask(IGs2Session session, RestSessionRequestFactory factory, GetFormModelMasterRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(GetFormModelMasterRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "formation")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/master/model/form/{formModelName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{formModelName}", !string.IsNullOrEmpty(request.FormModelName) ? request.FormModelName.ToString() : "null");

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
		public IEnumerator GetFormModelMaster(
                Request.GetFormModelMasterRequest request,
                UnityAction<AsyncResult<Result.GetFormModelMasterResult>> callback
        )
		{
			var task = new GetFormModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.GetFormModelMasterResult>(task.Result, task.Error));
        }

		public IFuture<Result.GetFormModelMasterResult> GetFormModelMasterFuture(
                Request.GetFormModelMasterRequest request
        )
		{
			return new GetFormModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.GetFormModelMasterResult> GetFormModelMasterAsync(
                Request.GetFormModelMasterRequest request
        )
		{
            AsyncResult<Result.GetFormModelMasterResult> result = null;
			await GetFormModelMaster(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
    #else
		public GetFormModelMasterTask GetFormModelMasterAsync(
                Request.GetFormModelMasterRequest request
        )
		{
			return new GetFormModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.GetFormModelMasterResult> GetFormModelMasterAsync(
                Request.GetFormModelMasterRequest request
        )
		{
			var task = new GetFormModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class UpdateFormModelMasterTask : Gs2RestSessionTask<UpdateFormModelMasterRequest, UpdateFormModelMasterResult>
        {
            public UpdateFormModelMasterTask(IGs2Session session, RestSessionRequestFactory factory, UpdateFormModelMasterRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(UpdateFormModelMasterRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "formation")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/master/model/form/{formModelName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{formModelName}", !string.IsNullOrEmpty(request.FormModelName) ? request.FormModelName.ToString() : "null");

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
                if (request.Slots != null)
                {
                    jsonWriter.WritePropertyName("slots");
                    jsonWriter.WriteArrayStart();
                    foreach(var item in request.Slots)
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
		public IEnumerator UpdateFormModelMaster(
                Request.UpdateFormModelMasterRequest request,
                UnityAction<AsyncResult<Result.UpdateFormModelMasterResult>> callback
        )
		{
			var task = new UpdateFormModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.UpdateFormModelMasterResult>(task.Result, task.Error));
        }

		public IFuture<Result.UpdateFormModelMasterResult> UpdateFormModelMasterFuture(
                Request.UpdateFormModelMasterRequest request
        )
		{
			return new UpdateFormModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.UpdateFormModelMasterResult> UpdateFormModelMasterAsync(
                Request.UpdateFormModelMasterRequest request
        )
		{
            AsyncResult<Result.UpdateFormModelMasterResult> result = null;
			await UpdateFormModelMaster(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
    #else
		public UpdateFormModelMasterTask UpdateFormModelMasterAsync(
                Request.UpdateFormModelMasterRequest request
        )
		{
			return new UpdateFormModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.UpdateFormModelMasterResult> UpdateFormModelMasterAsync(
                Request.UpdateFormModelMasterRequest request
        )
		{
			var task = new UpdateFormModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class DeleteFormModelMasterTask : Gs2RestSessionTask<DeleteFormModelMasterRequest, DeleteFormModelMasterResult>
        {
            public DeleteFormModelMasterTask(IGs2Session session, RestSessionRequestFactory factory, DeleteFormModelMasterRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DeleteFormModelMasterRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "formation")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/master/model/form/{formModelName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{formModelName}", !string.IsNullOrEmpty(request.FormModelName) ? request.FormModelName.ToString() : "null");

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
		public IEnumerator DeleteFormModelMaster(
                Request.DeleteFormModelMasterRequest request,
                UnityAction<AsyncResult<Result.DeleteFormModelMasterResult>> callback
        )
		{
			var task = new DeleteFormModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.DeleteFormModelMasterResult>(task.Result, task.Error));
        }

		public IFuture<Result.DeleteFormModelMasterResult> DeleteFormModelMasterFuture(
                Request.DeleteFormModelMasterRequest request
        )
		{
			return new DeleteFormModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.DeleteFormModelMasterResult> DeleteFormModelMasterAsync(
                Request.DeleteFormModelMasterRequest request
        )
		{
            AsyncResult<Result.DeleteFormModelMasterResult> result = null;
			await DeleteFormModelMaster(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
    #else
		public DeleteFormModelMasterTask DeleteFormModelMasterAsync(
                Request.DeleteFormModelMasterRequest request
        )
		{
			return new DeleteFormModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.DeleteFormModelMasterResult> DeleteFormModelMasterAsync(
                Request.DeleteFormModelMasterRequest request
        )
		{
			var task = new DeleteFormModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class DescribeMoldModelsTask : Gs2RestSessionTask<DescribeMoldModelsRequest, DescribeMoldModelsResult>
        {
            public DescribeMoldModelsTask(IGs2Session session, RestSessionRequestFactory factory, DescribeMoldModelsRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DescribeMoldModelsRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "formation")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/model/mold";

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
		public IEnumerator DescribeMoldModels(
                Request.DescribeMoldModelsRequest request,
                UnityAction<AsyncResult<Result.DescribeMoldModelsResult>> callback
        )
		{
			var task = new DescribeMoldModelsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.DescribeMoldModelsResult>(task.Result, task.Error));
        }

		public IFuture<Result.DescribeMoldModelsResult> DescribeMoldModelsFuture(
                Request.DescribeMoldModelsRequest request
        )
		{
			return new DescribeMoldModelsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.DescribeMoldModelsResult> DescribeMoldModelsAsync(
                Request.DescribeMoldModelsRequest request
        )
		{
            AsyncResult<Result.DescribeMoldModelsResult> result = null;
			await DescribeMoldModels(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
    #else
		public DescribeMoldModelsTask DescribeMoldModelsAsync(
                Request.DescribeMoldModelsRequest request
        )
		{
			return new DescribeMoldModelsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.DescribeMoldModelsResult> DescribeMoldModelsAsync(
                Request.DescribeMoldModelsRequest request
        )
		{
			var task = new DescribeMoldModelsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class GetMoldModelTask : Gs2RestSessionTask<GetMoldModelRequest, GetMoldModelResult>
        {
            public GetMoldModelTask(IGs2Session session, RestSessionRequestFactory factory, GetMoldModelRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(GetMoldModelRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "formation")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/model/mold/{moldModelName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{moldModelName}", !string.IsNullOrEmpty(request.MoldModelName) ? request.MoldModelName.ToString() : "null");

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
		public IEnumerator GetMoldModel(
                Request.GetMoldModelRequest request,
                UnityAction<AsyncResult<Result.GetMoldModelResult>> callback
        )
		{
			var task = new GetMoldModelTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.GetMoldModelResult>(task.Result, task.Error));
        }

		public IFuture<Result.GetMoldModelResult> GetMoldModelFuture(
                Request.GetMoldModelRequest request
        )
		{
			return new GetMoldModelTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.GetMoldModelResult> GetMoldModelAsync(
                Request.GetMoldModelRequest request
        )
		{
            AsyncResult<Result.GetMoldModelResult> result = null;
			await GetMoldModel(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
    #else
		public GetMoldModelTask GetMoldModelAsync(
                Request.GetMoldModelRequest request
        )
		{
			return new GetMoldModelTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.GetMoldModelResult> GetMoldModelAsync(
                Request.GetMoldModelRequest request
        )
		{
			var task = new GetMoldModelTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class DescribeMoldModelMastersTask : Gs2RestSessionTask<DescribeMoldModelMastersRequest, DescribeMoldModelMastersResult>
        {
            public DescribeMoldModelMastersTask(IGs2Session session, RestSessionRequestFactory factory, DescribeMoldModelMastersRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DescribeMoldModelMastersRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "formation")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/master/model/mold";

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
		public IEnumerator DescribeMoldModelMasters(
                Request.DescribeMoldModelMastersRequest request,
                UnityAction<AsyncResult<Result.DescribeMoldModelMastersResult>> callback
        )
		{
			var task = new DescribeMoldModelMastersTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.DescribeMoldModelMastersResult>(task.Result, task.Error));
        }

		public IFuture<Result.DescribeMoldModelMastersResult> DescribeMoldModelMastersFuture(
                Request.DescribeMoldModelMastersRequest request
        )
		{
			return new DescribeMoldModelMastersTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.DescribeMoldModelMastersResult> DescribeMoldModelMastersAsync(
                Request.DescribeMoldModelMastersRequest request
        )
		{
            AsyncResult<Result.DescribeMoldModelMastersResult> result = null;
			await DescribeMoldModelMasters(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
    #else
		public DescribeMoldModelMastersTask DescribeMoldModelMastersAsync(
                Request.DescribeMoldModelMastersRequest request
        )
		{
			return new DescribeMoldModelMastersTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.DescribeMoldModelMastersResult> DescribeMoldModelMastersAsync(
                Request.DescribeMoldModelMastersRequest request
        )
		{
			var task = new DescribeMoldModelMastersTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class CreateMoldModelMasterTask : Gs2RestSessionTask<CreateMoldModelMasterRequest, CreateMoldModelMasterResult>
        {
            public CreateMoldModelMasterTask(IGs2Session session, RestSessionRequestFactory factory, CreateMoldModelMasterRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(CreateMoldModelMasterRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "formation")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/master/model/mold";

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
                if (request.FormModelName != null)
                {
                    jsonWriter.WritePropertyName("formModelName");
                    jsonWriter.Write(request.FormModelName);
                }
                if (request.InitialMaxCapacity != null)
                {
                    jsonWriter.WritePropertyName("initialMaxCapacity");
                    jsonWriter.Write(request.InitialMaxCapacity.ToString());
                }
                if (request.MaxCapacity != null)
                {
                    jsonWriter.WritePropertyName("maxCapacity");
                    jsonWriter.Write(request.MaxCapacity.ToString());
                }
                if (request.ContextStack != null)
                {
                    jsonWriter.WritePropertyName("contextStack");
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
		public IEnumerator CreateMoldModelMaster(
                Request.CreateMoldModelMasterRequest request,
                UnityAction<AsyncResult<Result.CreateMoldModelMasterResult>> callback
        )
		{
			var task = new CreateMoldModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.CreateMoldModelMasterResult>(task.Result, task.Error));
        }

		public IFuture<Result.CreateMoldModelMasterResult> CreateMoldModelMasterFuture(
                Request.CreateMoldModelMasterRequest request
        )
		{
			return new CreateMoldModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.CreateMoldModelMasterResult> CreateMoldModelMasterAsync(
                Request.CreateMoldModelMasterRequest request
        )
		{
            AsyncResult<Result.CreateMoldModelMasterResult> result = null;
			await CreateMoldModelMaster(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
    #else
		public CreateMoldModelMasterTask CreateMoldModelMasterAsync(
                Request.CreateMoldModelMasterRequest request
        )
		{
			return new CreateMoldModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.CreateMoldModelMasterResult> CreateMoldModelMasterAsync(
                Request.CreateMoldModelMasterRequest request
        )
		{
			var task = new CreateMoldModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class GetMoldModelMasterTask : Gs2RestSessionTask<GetMoldModelMasterRequest, GetMoldModelMasterResult>
        {
            public GetMoldModelMasterTask(IGs2Session session, RestSessionRequestFactory factory, GetMoldModelMasterRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(GetMoldModelMasterRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "formation")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/master/model/mold/{moldModelName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{moldModelName}", !string.IsNullOrEmpty(request.MoldModelName) ? request.MoldModelName.ToString() : "null");

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
		public IEnumerator GetMoldModelMaster(
                Request.GetMoldModelMasterRequest request,
                UnityAction<AsyncResult<Result.GetMoldModelMasterResult>> callback
        )
		{
			var task = new GetMoldModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.GetMoldModelMasterResult>(task.Result, task.Error));
        }

		public IFuture<Result.GetMoldModelMasterResult> GetMoldModelMasterFuture(
                Request.GetMoldModelMasterRequest request
        )
		{
			return new GetMoldModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.GetMoldModelMasterResult> GetMoldModelMasterAsync(
                Request.GetMoldModelMasterRequest request
        )
		{
            AsyncResult<Result.GetMoldModelMasterResult> result = null;
			await GetMoldModelMaster(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
    #else
		public GetMoldModelMasterTask GetMoldModelMasterAsync(
                Request.GetMoldModelMasterRequest request
        )
		{
			return new GetMoldModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.GetMoldModelMasterResult> GetMoldModelMasterAsync(
                Request.GetMoldModelMasterRequest request
        )
		{
			var task = new GetMoldModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class UpdateMoldModelMasterTask : Gs2RestSessionTask<UpdateMoldModelMasterRequest, UpdateMoldModelMasterResult>
        {
            public UpdateMoldModelMasterTask(IGs2Session session, RestSessionRequestFactory factory, UpdateMoldModelMasterRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(UpdateMoldModelMasterRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "formation")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/master/model/mold/{moldModelName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{moldModelName}", !string.IsNullOrEmpty(request.MoldModelName) ? request.MoldModelName.ToString() : "null");

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
                if (request.FormModelName != null)
                {
                    jsonWriter.WritePropertyName("formModelName");
                    jsonWriter.Write(request.FormModelName);
                }
                if (request.InitialMaxCapacity != null)
                {
                    jsonWriter.WritePropertyName("initialMaxCapacity");
                    jsonWriter.Write(request.InitialMaxCapacity.ToString());
                }
                if (request.MaxCapacity != null)
                {
                    jsonWriter.WritePropertyName("maxCapacity");
                    jsonWriter.Write(request.MaxCapacity.ToString());
                }
                if (request.ContextStack != null)
                {
                    jsonWriter.WritePropertyName("contextStack");
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
		public IEnumerator UpdateMoldModelMaster(
                Request.UpdateMoldModelMasterRequest request,
                UnityAction<AsyncResult<Result.UpdateMoldModelMasterResult>> callback
        )
		{
			var task = new UpdateMoldModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.UpdateMoldModelMasterResult>(task.Result, task.Error));
        }

		public IFuture<Result.UpdateMoldModelMasterResult> UpdateMoldModelMasterFuture(
                Request.UpdateMoldModelMasterRequest request
        )
		{
			return new UpdateMoldModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.UpdateMoldModelMasterResult> UpdateMoldModelMasterAsync(
                Request.UpdateMoldModelMasterRequest request
        )
		{
            AsyncResult<Result.UpdateMoldModelMasterResult> result = null;
			await UpdateMoldModelMaster(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
    #else
		public UpdateMoldModelMasterTask UpdateMoldModelMasterAsync(
                Request.UpdateMoldModelMasterRequest request
        )
		{
			return new UpdateMoldModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.UpdateMoldModelMasterResult> UpdateMoldModelMasterAsync(
                Request.UpdateMoldModelMasterRequest request
        )
		{
			var task = new UpdateMoldModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class DeleteMoldModelMasterTask : Gs2RestSessionTask<DeleteMoldModelMasterRequest, DeleteMoldModelMasterResult>
        {
            public DeleteMoldModelMasterTask(IGs2Session session, RestSessionRequestFactory factory, DeleteMoldModelMasterRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DeleteMoldModelMasterRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "formation")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/master/model/mold/{moldModelName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{moldModelName}", !string.IsNullOrEmpty(request.MoldModelName) ? request.MoldModelName.ToString() : "null");

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
		public IEnumerator DeleteMoldModelMaster(
                Request.DeleteMoldModelMasterRequest request,
                UnityAction<AsyncResult<Result.DeleteMoldModelMasterResult>> callback
        )
		{
			var task = new DeleteMoldModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.DeleteMoldModelMasterResult>(task.Result, task.Error));
        }

		public IFuture<Result.DeleteMoldModelMasterResult> DeleteMoldModelMasterFuture(
                Request.DeleteMoldModelMasterRequest request
        )
		{
			return new DeleteMoldModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.DeleteMoldModelMasterResult> DeleteMoldModelMasterAsync(
                Request.DeleteMoldModelMasterRequest request
        )
		{
            AsyncResult<Result.DeleteMoldModelMasterResult> result = null;
			await DeleteMoldModelMaster(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
    #else
		public DeleteMoldModelMasterTask DeleteMoldModelMasterAsync(
                Request.DeleteMoldModelMasterRequest request
        )
		{
			return new DeleteMoldModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.DeleteMoldModelMasterResult> DeleteMoldModelMasterAsync(
                Request.DeleteMoldModelMasterRequest request
        )
		{
			var task = new DeleteMoldModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class DescribePropertyFormModelsTask : Gs2RestSessionTask<DescribePropertyFormModelsRequest, DescribePropertyFormModelsResult>
        {
            public DescribePropertyFormModelsTask(IGs2Session session, RestSessionRequestFactory factory, DescribePropertyFormModelsRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DescribePropertyFormModelsRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "formation")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/model/propertyForm";

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
		public IEnumerator DescribePropertyFormModels(
                Request.DescribePropertyFormModelsRequest request,
                UnityAction<AsyncResult<Result.DescribePropertyFormModelsResult>> callback
        )
		{
			var task = new DescribePropertyFormModelsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.DescribePropertyFormModelsResult>(task.Result, task.Error));
        }

		public IFuture<Result.DescribePropertyFormModelsResult> DescribePropertyFormModelsFuture(
                Request.DescribePropertyFormModelsRequest request
        )
		{
			return new DescribePropertyFormModelsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.DescribePropertyFormModelsResult> DescribePropertyFormModelsAsync(
                Request.DescribePropertyFormModelsRequest request
        )
		{
            AsyncResult<Result.DescribePropertyFormModelsResult> result = null;
			await DescribePropertyFormModels(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
    #else
		public DescribePropertyFormModelsTask DescribePropertyFormModelsAsync(
                Request.DescribePropertyFormModelsRequest request
        )
		{
			return new DescribePropertyFormModelsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.DescribePropertyFormModelsResult> DescribePropertyFormModelsAsync(
                Request.DescribePropertyFormModelsRequest request
        )
		{
			var task = new DescribePropertyFormModelsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class GetPropertyFormModelTask : Gs2RestSessionTask<GetPropertyFormModelRequest, GetPropertyFormModelResult>
        {
            public GetPropertyFormModelTask(IGs2Session session, RestSessionRequestFactory factory, GetPropertyFormModelRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(GetPropertyFormModelRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "formation")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/model/propertyForm/{propertyFormModelName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{propertyFormModelName}", !string.IsNullOrEmpty(request.PropertyFormModelName) ? request.PropertyFormModelName.ToString() : "null");

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
		public IEnumerator GetPropertyFormModel(
                Request.GetPropertyFormModelRequest request,
                UnityAction<AsyncResult<Result.GetPropertyFormModelResult>> callback
        )
		{
			var task = new GetPropertyFormModelTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.GetPropertyFormModelResult>(task.Result, task.Error));
        }

		public IFuture<Result.GetPropertyFormModelResult> GetPropertyFormModelFuture(
                Request.GetPropertyFormModelRequest request
        )
		{
			return new GetPropertyFormModelTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.GetPropertyFormModelResult> GetPropertyFormModelAsync(
                Request.GetPropertyFormModelRequest request
        )
		{
            AsyncResult<Result.GetPropertyFormModelResult> result = null;
			await GetPropertyFormModel(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
    #else
		public GetPropertyFormModelTask GetPropertyFormModelAsync(
                Request.GetPropertyFormModelRequest request
        )
		{
			return new GetPropertyFormModelTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.GetPropertyFormModelResult> GetPropertyFormModelAsync(
                Request.GetPropertyFormModelRequest request
        )
		{
			var task = new GetPropertyFormModelTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class DescribePropertyFormModelMastersTask : Gs2RestSessionTask<DescribePropertyFormModelMastersRequest, DescribePropertyFormModelMastersResult>
        {
            public DescribePropertyFormModelMastersTask(IGs2Session session, RestSessionRequestFactory factory, DescribePropertyFormModelMastersRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DescribePropertyFormModelMastersRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "formation")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/master/model/propertyForm";

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
		public IEnumerator DescribePropertyFormModelMasters(
                Request.DescribePropertyFormModelMastersRequest request,
                UnityAction<AsyncResult<Result.DescribePropertyFormModelMastersResult>> callback
        )
		{
			var task = new DescribePropertyFormModelMastersTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.DescribePropertyFormModelMastersResult>(task.Result, task.Error));
        }

		public IFuture<Result.DescribePropertyFormModelMastersResult> DescribePropertyFormModelMastersFuture(
                Request.DescribePropertyFormModelMastersRequest request
        )
		{
			return new DescribePropertyFormModelMastersTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.DescribePropertyFormModelMastersResult> DescribePropertyFormModelMastersAsync(
                Request.DescribePropertyFormModelMastersRequest request
        )
		{
            AsyncResult<Result.DescribePropertyFormModelMastersResult> result = null;
			await DescribePropertyFormModelMasters(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
    #else
		public DescribePropertyFormModelMastersTask DescribePropertyFormModelMastersAsync(
                Request.DescribePropertyFormModelMastersRequest request
        )
		{
			return new DescribePropertyFormModelMastersTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.DescribePropertyFormModelMastersResult> DescribePropertyFormModelMastersAsync(
                Request.DescribePropertyFormModelMastersRequest request
        )
		{
			var task = new DescribePropertyFormModelMastersTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class CreatePropertyFormModelMasterTask : Gs2RestSessionTask<CreatePropertyFormModelMasterRequest, CreatePropertyFormModelMasterResult>
        {
            public CreatePropertyFormModelMasterTask(IGs2Session session, RestSessionRequestFactory factory, CreatePropertyFormModelMasterRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(CreatePropertyFormModelMasterRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "formation")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/master/model/propertyForm";

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
                if (request.Slots != null)
                {
                    jsonWriter.WritePropertyName("slots");
                    jsonWriter.WriteArrayStart();
                    foreach(var item in request.Slots)
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
		public IEnumerator CreatePropertyFormModelMaster(
                Request.CreatePropertyFormModelMasterRequest request,
                UnityAction<AsyncResult<Result.CreatePropertyFormModelMasterResult>> callback
        )
		{
			var task = new CreatePropertyFormModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.CreatePropertyFormModelMasterResult>(task.Result, task.Error));
        }

		public IFuture<Result.CreatePropertyFormModelMasterResult> CreatePropertyFormModelMasterFuture(
                Request.CreatePropertyFormModelMasterRequest request
        )
		{
			return new CreatePropertyFormModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.CreatePropertyFormModelMasterResult> CreatePropertyFormModelMasterAsync(
                Request.CreatePropertyFormModelMasterRequest request
        )
		{
            AsyncResult<Result.CreatePropertyFormModelMasterResult> result = null;
			await CreatePropertyFormModelMaster(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
    #else
		public CreatePropertyFormModelMasterTask CreatePropertyFormModelMasterAsync(
                Request.CreatePropertyFormModelMasterRequest request
        )
		{
			return new CreatePropertyFormModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.CreatePropertyFormModelMasterResult> CreatePropertyFormModelMasterAsync(
                Request.CreatePropertyFormModelMasterRequest request
        )
		{
			var task = new CreatePropertyFormModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class GetPropertyFormModelMasterTask : Gs2RestSessionTask<GetPropertyFormModelMasterRequest, GetPropertyFormModelMasterResult>
        {
            public GetPropertyFormModelMasterTask(IGs2Session session, RestSessionRequestFactory factory, GetPropertyFormModelMasterRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(GetPropertyFormModelMasterRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "formation")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/master/model/propertyForm/{propertyFormModelName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{propertyFormModelName}", !string.IsNullOrEmpty(request.PropertyFormModelName) ? request.PropertyFormModelName.ToString() : "null");

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
		public IEnumerator GetPropertyFormModelMaster(
                Request.GetPropertyFormModelMasterRequest request,
                UnityAction<AsyncResult<Result.GetPropertyFormModelMasterResult>> callback
        )
		{
			var task = new GetPropertyFormModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.GetPropertyFormModelMasterResult>(task.Result, task.Error));
        }

		public IFuture<Result.GetPropertyFormModelMasterResult> GetPropertyFormModelMasterFuture(
                Request.GetPropertyFormModelMasterRequest request
        )
		{
			return new GetPropertyFormModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.GetPropertyFormModelMasterResult> GetPropertyFormModelMasterAsync(
                Request.GetPropertyFormModelMasterRequest request
        )
		{
            AsyncResult<Result.GetPropertyFormModelMasterResult> result = null;
			await GetPropertyFormModelMaster(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
    #else
		public GetPropertyFormModelMasterTask GetPropertyFormModelMasterAsync(
                Request.GetPropertyFormModelMasterRequest request
        )
		{
			return new GetPropertyFormModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.GetPropertyFormModelMasterResult> GetPropertyFormModelMasterAsync(
                Request.GetPropertyFormModelMasterRequest request
        )
		{
			var task = new GetPropertyFormModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class UpdatePropertyFormModelMasterTask : Gs2RestSessionTask<UpdatePropertyFormModelMasterRequest, UpdatePropertyFormModelMasterResult>
        {
            public UpdatePropertyFormModelMasterTask(IGs2Session session, RestSessionRequestFactory factory, UpdatePropertyFormModelMasterRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(UpdatePropertyFormModelMasterRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "formation")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/master/model/propertyForm/{propertyFormModelName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{propertyFormModelName}", !string.IsNullOrEmpty(request.PropertyFormModelName) ? request.PropertyFormModelName.ToString() : "null");

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
                if (request.Slots != null)
                {
                    jsonWriter.WritePropertyName("slots");
                    jsonWriter.WriteArrayStart();
                    foreach(var item in request.Slots)
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
		public IEnumerator UpdatePropertyFormModelMaster(
                Request.UpdatePropertyFormModelMasterRequest request,
                UnityAction<AsyncResult<Result.UpdatePropertyFormModelMasterResult>> callback
        )
		{
			var task = new UpdatePropertyFormModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.UpdatePropertyFormModelMasterResult>(task.Result, task.Error));
        }

		public IFuture<Result.UpdatePropertyFormModelMasterResult> UpdatePropertyFormModelMasterFuture(
                Request.UpdatePropertyFormModelMasterRequest request
        )
		{
			return new UpdatePropertyFormModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.UpdatePropertyFormModelMasterResult> UpdatePropertyFormModelMasterAsync(
                Request.UpdatePropertyFormModelMasterRequest request
        )
		{
            AsyncResult<Result.UpdatePropertyFormModelMasterResult> result = null;
			await UpdatePropertyFormModelMaster(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
    #else
		public UpdatePropertyFormModelMasterTask UpdatePropertyFormModelMasterAsync(
                Request.UpdatePropertyFormModelMasterRequest request
        )
		{
			return new UpdatePropertyFormModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.UpdatePropertyFormModelMasterResult> UpdatePropertyFormModelMasterAsync(
                Request.UpdatePropertyFormModelMasterRequest request
        )
		{
			var task = new UpdatePropertyFormModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class DeletePropertyFormModelMasterTask : Gs2RestSessionTask<DeletePropertyFormModelMasterRequest, DeletePropertyFormModelMasterResult>
        {
            public DeletePropertyFormModelMasterTask(IGs2Session session, RestSessionRequestFactory factory, DeletePropertyFormModelMasterRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DeletePropertyFormModelMasterRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "formation")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/master/model/propertyForm/{propertyFormModelName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{propertyFormModelName}", !string.IsNullOrEmpty(request.PropertyFormModelName) ? request.PropertyFormModelName.ToString() : "null");

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
		public IEnumerator DeletePropertyFormModelMaster(
                Request.DeletePropertyFormModelMasterRequest request,
                UnityAction<AsyncResult<Result.DeletePropertyFormModelMasterResult>> callback
        )
		{
			var task = new DeletePropertyFormModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.DeletePropertyFormModelMasterResult>(task.Result, task.Error));
        }

		public IFuture<Result.DeletePropertyFormModelMasterResult> DeletePropertyFormModelMasterFuture(
                Request.DeletePropertyFormModelMasterRequest request
        )
		{
			return new DeletePropertyFormModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.DeletePropertyFormModelMasterResult> DeletePropertyFormModelMasterAsync(
                Request.DeletePropertyFormModelMasterRequest request
        )
		{
            AsyncResult<Result.DeletePropertyFormModelMasterResult> result = null;
			await DeletePropertyFormModelMaster(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
    #else
		public DeletePropertyFormModelMasterTask DeletePropertyFormModelMasterAsync(
                Request.DeletePropertyFormModelMasterRequest request
        )
		{
			return new DeletePropertyFormModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.DeletePropertyFormModelMasterResult> DeletePropertyFormModelMasterAsync(
                Request.DeletePropertyFormModelMasterRequest request
        )
		{
			var task = new DeletePropertyFormModelMasterTask(
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
                    .Replace("{service}", "formation")
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


        public class GetCurrentFormMasterTask : Gs2RestSessionTask<GetCurrentFormMasterRequest, GetCurrentFormMasterResult>
        {
            public GetCurrentFormMasterTask(IGs2Session session, RestSessionRequestFactory factory, GetCurrentFormMasterRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(GetCurrentFormMasterRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "formation")
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
		public IEnumerator GetCurrentFormMaster(
                Request.GetCurrentFormMasterRequest request,
                UnityAction<AsyncResult<Result.GetCurrentFormMasterResult>> callback
        )
		{
			var task = new GetCurrentFormMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.GetCurrentFormMasterResult>(task.Result, task.Error));
        }

		public IFuture<Result.GetCurrentFormMasterResult> GetCurrentFormMasterFuture(
                Request.GetCurrentFormMasterRequest request
        )
		{
			return new GetCurrentFormMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.GetCurrentFormMasterResult> GetCurrentFormMasterAsync(
                Request.GetCurrentFormMasterRequest request
        )
		{
            AsyncResult<Result.GetCurrentFormMasterResult> result = null;
			await GetCurrentFormMaster(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
    #else
		public GetCurrentFormMasterTask GetCurrentFormMasterAsync(
                Request.GetCurrentFormMasterRequest request
        )
		{
			return new GetCurrentFormMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.GetCurrentFormMasterResult> GetCurrentFormMasterAsync(
                Request.GetCurrentFormMasterRequest request
        )
		{
			var task = new GetCurrentFormMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class UpdateCurrentFormMasterTask : Gs2RestSessionTask<UpdateCurrentFormMasterRequest, UpdateCurrentFormMasterResult>
        {
            public UpdateCurrentFormMasterTask(IGs2Session session, RestSessionRequestFactory factory, UpdateCurrentFormMasterRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(UpdateCurrentFormMasterRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "formation")
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
		public IEnumerator UpdateCurrentFormMaster(
                Request.UpdateCurrentFormMasterRequest request,
                UnityAction<AsyncResult<Result.UpdateCurrentFormMasterResult>> callback
        )
		{
			var task = new UpdateCurrentFormMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.UpdateCurrentFormMasterResult>(task.Result, task.Error));
        }

		public IFuture<Result.UpdateCurrentFormMasterResult> UpdateCurrentFormMasterFuture(
                Request.UpdateCurrentFormMasterRequest request
        )
		{
			return new UpdateCurrentFormMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.UpdateCurrentFormMasterResult> UpdateCurrentFormMasterAsync(
                Request.UpdateCurrentFormMasterRequest request
        )
		{
            AsyncResult<Result.UpdateCurrentFormMasterResult> result = null;
			await UpdateCurrentFormMaster(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
    #else
		public UpdateCurrentFormMasterTask UpdateCurrentFormMasterAsync(
                Request.UpdateCurrentFormMasterRequest request
        )
		{
			return new UpdateCurrentFormMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.UpdateCurrentFormMasterResult> UpdateCurrentFormMasterAsync(
                Request.UpdateCurrentFormMasterRequest request
        )
		{
			var task = new UpdateCurrentFormMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class UpdateCurrentFormMasterFromGitHubTask : Gs2RestSessionTask<UpdateCurrentFormMasterFromGitHubRequest, UpdateCurrentFormMasterFromGitHubResult>
        {
            public UpdateCurrentFormMasterFromGitHubTask(IGs2Session session, RestSessionRequestFactory factory, UpdateCurrentFormMasterFromGitHubRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(UpdateCurrentFormMasterFromGitHubRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "formation")
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
		public IEnumerator UpdateCurrentFormMasterFromGitHub(
                Request.UpdateCurrentFormMasterFromGitHubRequest request,
                UnityAction<AsyncResult<Result.UpdateCurrentFormMasterFromGitHubResult>> callback
        )
		{
			var task = new UpdateCurrentFormMasterFromGitHubTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.UpdateCurrentFormMasterFromGitHubResult>(task.Result, task.Error));
        }

		public IFuture<Result.UpdateCurrentFormMasterFromGitHubResult> UpdateCurrentFormMasterFromGitHubFuture(
                Request.UpdateCurrentFormMasterFromGitHubRequest request
        )
		{
			return new UpdateCurrentFormMasterFromGitHubTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.UpdateCurrentFormMasterFromGitHubResult> UpdateCurrentFormMasterFromGitHubAsync(
                Request.UpdateCurrentFormMasterFromGitHubRequest request
        )
		{
            AsyncResult<Result.UpdateCurrentFormMasterFromGitHubResult> result = null;
			await UpdateCurrentFormMasterFromGitHub(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
    #else
		public UpdateCurrentFormMasterFromGitHubTask UpdateCurrentFormMasterFromGitHubAsync(
                Request.UpdateCurrentFormMasterFromGitHubRequest request
        )
		{
			return new UpdateCurrentFormMasterFromGitHubTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.UpdateCurrentFormMasterFromGitHubResult> UpdateCurrentFormMasterFromGitHubAsync(
                Request.UpdateCurrentFormMasterFromGitHubRequest request
        )
		{
			var task = new UpdateCurrentFormMasterFromGitHubTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class DescribeMoldsTask : Gs2RestSessionTask<DescribeMoldsRequest, DescribeMoldsResult>
        {
            public DescribeMoldsTask(IGs2Session session, RestSessionRequestFactory factory, DescribeMoldsRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DescribeMoldsRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "formation")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/me/mold";

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
		public IEnumerator DescribeMolds(
                Request.DescribeMoldsRequest request,
                UnityAction<AsyncResult<Result.DescribeMoldsResult>> callback
        )
		{
			var task = new DescribeMoldsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.DescribeMoldsResult>(task.Result, task.Error));
        }

		public IFuture<Result.DescribeMoldsResult> DescribeMoldsFuture(
                Request.DescribeMoldsRequest request
        )
		{
			return new DescribeMoldsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.DescribeMoldsResult> DescribeMoldsAsync(
                Request.DescribeMoldsRequest request
        )
		{
            AsyncResult<Result.DescribeMoldsResult> result = null;
			await DescribeMolds(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
    #else
		public DescribeMoldsTask DescribeMoldsAsync(
                Request.DescribeMoldsRequest request
        )
		{
			return new DescribeMoldsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.DescribeMoldsResult> DescribeMoldsAsync(
                Request.DescribeMoldsRequest request
        )
		{
			var task = new DescribeMoldsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class DescribeMoldsByUserIdTask : Gs2RestSessionTask<DescribeMoldsByUserIdRequest, DescribeMoldsByUserIdResult>
        {
            public DescribeMoldsByUserIdTask(IGs2Session session, RestSessionRequestFactory factory, DescribeMoldsByUserIdRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DescribeMoldsByUserIdRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "formation")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/{userId}/mold";

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
		public IEnumerator DescribeMoldsByUserId(
                Request.DescribeMoldsByUserIdRequest request,
                UnityAction<AsyncResult<Result.DescribeMoldsByUserIdResult>> callback
        )
		{
			var task = new DescribeMoldsByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.DescribeMoldsByUserIdResult>(task.Result, task.Error));
        }

		public IFuture<Result.DescribeMoldsByUserIdResult> DescribeMoldsByUserIdFuture(
                Request.DescribeMoldsByUserIdRequest request
        )
		{
			return new DescribeMoldsByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.DescribeMoldsByUserIdResult> DescribeMoldsByUserIdAsync(
                Request.DescribeMoldsByUserIdRequest request
        )
		{
            AsyncResult<Result.DescribeMoldsByUserIdResult> result = null;
			await DescribeMoldsByUserId(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
    #else
		public DescribeMoldsByUserIdTask DescribeMoldsByUserIdAsync(
                Request.DescribeMoldsByUserIdRequest request
        )
		{
			return new DescribeMoldsByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.DescribeMoldsByUserIdResult> DescribeMoldsByUserIdAsync(
                Request.DescribeMoldsByUserIdRequest request
        )
		{
			var task = new DescribeMoldsByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class GetMoldTask : Gs2RestSessionTask<GetMoldRequest, GetMoldResult>
        {
            public GetMoldTask(IGs2Session session, RestSessionRequestFactory factory, GetMoldRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(GetMoldRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "formation")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/me/mold/{moldModelName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{moldModelName}", !string.IsNullOrEmpty(request.MoldModelName) ? request.MoldModelName.ToString() : "null");

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
		public IEnumerator GetMold(
                Request.GetMoldRequest request,
                UnityAction<AsyncResult<Result.GetMoldResult>> callback
        )
		{
			var task = new GetMoldTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.GetMoldResult>(task.Result, task.Error));
        }

		public IFuture<Result.GetMoldResult> GetMoldFuture(
                Request.GetMoldRequest request
        )
		{
			return new GetMoldTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.GetMoldResult> GetMoldAsync(
                Request.GetMoldRequest request
        )
		{
            AsyncResult<Result.GetMoldResult> result = null;
			await GetMold(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
    #else
		public GetMoldTask GetMoldAsync(
                Request.GetMoldRequest request
        )
		{
			return new GetMoldTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.GetMoldResult> GetMoldAsync(
                Request.GetMoldRequest request
        )
		{
			var task = new GetMoldTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class GetMoldByUserIdTask : Gs2RestSessionTask<GetMoldByUserIdRequest, GetMoldByUserIdResult>
        {
            public GetMoldByUserIdTask(IGs2Session session, RestSessionRequestFactory factory, GetMoldByUserIdRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(GetMoldByUserIdRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "formation")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/{userId}/mold/{moldModelName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{userId}", !string.IsNullOrEmpty(request.UserId) ? request.UserId.ToString() : "null");
                url = url.Replace("{moldModelName}", !string.IsNullOrEmpty(request.MoldModelName) ? request.MoldModelName.ToString() : "null");

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
		public IEnumerator GetMoldByUserId(
                Request.GetMoldByUserIdRequest request,
                UnityAction<AsyncResult<Result.GetMoldByUserIdResult>> callback
        )
		{
			var task = new GetMoldByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.GetMoldByUserIdResult>(task.Result, task.Error));
        }

		public IFuture<Result.GetMoldByUserIdResult> GetMoldByUserIdFuture(
                Request.GetMoldByUserIdRequest request
        )
		{
			return new GetMoldByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.GetMoldByUserIdResult> GetMoldByUserIdAsync(
                Request.GetMoldByUserIdRequest request
        )
		{
            AsyncResult<Result.GetMoldByUserIdResult> result = null;
			await GetMoldByUserId(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
    #else
		public GetMoldByUserIdTask GetMoldByUserIdAsync(
                Request.GetMoldByUserIdRequest request
        )
		{
			return new GetMoldByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.GetMoldByUserIdResult> GetMoldByUserIdAsync(
                Request.GetMoldByUserIdRequest request
        )
		{
			var task = new GetMoldByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class SetMoldCapacityByUserIdTask : Gs2RestSessionTask<SetMoldCapacityByUserIdRequest, SetMoldCapacityByUserIdResult>
        {
            public SetMoldCapacityByUserIdTask(IGs2Session session, RestSessionRequestFactory factory, SetMoldCapacityByUserIdRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(SetMoldCapacityByUserIdRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "formation")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/{userId}/mold/{moldModelName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{userId}", !string.IsNullOrEmpty(request.UserId) ? request.UserId.ToString() : "null");
                url = url.Replace("{moldModelName}", !string.IsNullOrEmpty(request.MoldModelName) ? request.MoldModelName.ToString() : "null");

                var sessionRequest = Factory.Put(url);

                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);
                jsonWriter.WriteObjectStart();
                if (request.Capacity != null)
                {
                    jsonWriter.WritePropertyName("capacity");
                    jsonWriter.Write(request.Capacity.ToString());
                }
                if (request.ContextStack != null)
                {
                    jsonWriter.WritePropertyName("contextStack");
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
		public IEnumerator SetMoldCapacityByUserId(
                Request.SetMoldCapacityByUserIdRequest request,
                UnityAction<AsyncResult<Result.SetMoldCapacityByUserIdResult>> callback
        )
		{
			var task = new SetMoldCapacityByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.SetMoldCapacityByUserIdResult>(task.Result, task.Error));
        }

		public IFuture<Result.SetMoldCapacityByUserIdResult> SetMoldCapacityByUserIdFuture(
                Request.SetMoldCapacityByUserIdRequest request
        )
		{
			return new SetMoldCapacityByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.SetMoldCapacityByUserIdResult> SetMoldCapacityByUserIdAsync(
                Request.SetMoldCapacityByUserIdRequest request
        )
		{
            AsyncResult<Result.SetMoldCapacityByUserIdResult> result = null;
			await SetMoldCapacityByUserId(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
    #else
		public SetMoldCapacityByUserIdTask SetMoldCapacityByUserIdAsync(
                Request.SetMoldCapacityByUserIdRequest request
        )
		{
			return new SetMoldCapacityByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.SetMoldCapacityByUserIdResult> SetMoldCapacityByUserIdAsync(
                Request.SetMoldCapacityByUserIdRequest request
        )
		{
			var task = new SetMoldCapacityByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class AddMoldCapacityByUserIdTask : Gs2RestSessionTask<AddMoldCapacityByUserIdRequest, AddMoldCapacityByUserIdResult>
        {
            public AddMoldCapacityByUserIdTask(IGs2Session session, RestSessionRequestFactory factory, AddMoldCapacityByUserIdRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(AddMoldCapacityByUserIdRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "formation")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/{userId}/mold/{moldModelName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{userId}", !string.IsNullOrEmpty(request.UserId) ? request.UserId.ToString() : "null");
                url = url.Replace("{moldModelName}", !string.IsNullOrEmpty(request.MoldModelName) ? request.MoldModelName.ToString() : "null");

                var sessionRequest = Factory.Post(url);

                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);
                jsonWriter.WriteObjectStart();
                if (request.Capacity != null)
                {
                    jsonWriter.WritePropertyName("capacity");
                    jsonWriter.Write(request.Capacity.ToString());
                }
                if (request.ContextStack != null)
                {
                    jsonWriter.WritePropertyName("contextStack");
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
		public IEnumerator AddMoldCapacityByUserId(
                Request.AddMoldCapacityByUserIdRequest request,
                UnityAction<AsyncResult<Result.AddMoldCapacityByUserIdResult>> callback
        )
		{
			var task = new AddMoldCapacityByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.AddMoldCapacityByUserIdResult>(task.Result, task.Error));
        }

		public IFuture<Result.AddMoldCapacityByUserIdResult> AddMoldCapacityByUserIdFuture(
                Request.AddMoldCapacityByUserIdRequest request
        )
		{
			return new AddMoldCapacityByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.AddMoldCapacityByUserIdResult> AddMoldCapacityByUserIdAsync(
                Request.AddMoldCapacityByUserIdRequest request
        )
		{
            AsyncResult<Result.AddMoldCapacityByUserIdResult> result = null;
			await AddMoldCapacityByUserId(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
    #else
		public AddMoldCapacityByUserIdTask AddMoldCapacityByUserIdAsync(
                Request.AddMoldCapacityByUserIdRequest request
        )
		{
			return new AddMoldCapacityByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.AddMoldCapacityByUserIdResult> AddMoldCapacityByUserIdAsync(
                Request.AddMoldCapacityByUserIdRequest request
        )
		{
			var task = new AddMoldCapacityByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class SubMoldCapacityTask : Gs2RestSessionTask<SubMoldCapacityRequest, SubMoldCapacityResult>
        {
            public SubMoldCapacityTask(IGs2Session session, RestSessionRequestFactory factory, SubMoldCapacityRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(SubMoldCapacityRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "formation")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/me/mold/{moldModelName}/sub";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{moldModelName}", !string.IsNullOrEmpty(request.MoldModelName) ? request.MoldModelName.ToString() : "null");

                var sessionRequest = Factory.Post(url);

                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);
                jsonWriter.WriteObjectStart();
                if (request.Capacity != null)
                {
                    jsonWriter.WritePropertyName("capacity");
                    jsonWriter.Write(request.Capacity.ToString());
                }
                if (request.ContextStack != null)
                {
                    jsonWriter.WritePropertyName("contextStack");
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
		public IEnumerator SubMoldCapacity(
                Request.SubMoldCapacityRequest request,
                UnityAction<AsyncResult<Result.SubMoldCapacityResult>> callback
        )
		{
			var task = new SubMoldCapacityTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.SubMoldCapacityResult>(task.Result, task.Error));
        }

		public IFuture<Result.SubMoldCapacityResult> SubMoldCapacityFuture(
                Request.SubMoldCapacityRequest request
        )
		{
			return new SubMoldCapacityTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.SubMoldCapacityResult> SubMoldCapacityAsync(
                Request.SubMoldCapacityRequest request
        )
		{
            AsyncResult<Result.SubMoldCapacityResult> result = null;
			await SubMoldCapacity(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
    #else
		public SubMoldCapacityTask SubMoldCapacityAsync(
                Request.SubMoldCapacityRequest request
        )
		{
			return new SubMoldCapacityTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.SubMoldCapacityResult> SubMoldCapacityAsync(
                Request.SubMoldCapacityRequest request
        )
		{
			var task = new SubMoldCapacityTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class SubMoldCapacityByUserIdTask : Gs2RestSessionTask<SubMoldCapacityByUserIdRequest, SubMoldCapacityByUserIdResult>
        {
            public SubMoldCapacityByUserIdTask(IGs2Session session, RestSessionRequestFactory factory, SubMoldCapacityByUserIdRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(SubMoldCapacityByUserIdRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "formation")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/{userId}/mold/{moldModelName}/sub";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{userId}", !string.IsNullOrEmpty(request.UserId) ? request.UserId.ToString() : "null");
                url = url.Replace("{moldModelName}", !string.IsNullOrEmpty(request.MoldModelName) ? request.MoldModelName.ToString() : "null");

                var sessionRequest = Factory.Post(url);

                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);
                jsonWriter.WriteObjectStart();
                if (request.Capacity != null)
                {
                    jsonWriter.WritePropertyName("capacity");
                    jsonWriter.Write(request.Capacity.ToString());
                }
                if (request.ContextStack != null)
                {
                    jsonWriter.WritePropertyName("contextStack");
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
		public IEnumerator SubMoldCapacityByUserId(
                Request.SubMoldCapacityByUserIdRequest request,
                UnityAction<AsyncResult<Result.SubMoldCapacityByUserIdResult>> callback
        )
		{
			var task = new SubMoldCapacityByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.SubMoldCapacityByUserIdResult>(task.Result, task.Error));
        }

		public IFuture<Result.SubMoldCapacityByUserIdResult> SubMoldCapacityByUserIdFuture(
                Request.SubMoldCapacityByUserIdRequest request
        )
		{
			return new SubMoldCapacityByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.SubMoldCapacityByUserIdResult> SubMoldCapacityByUserIdAsync(
                Request.SubMoldCapacityByUserIdRequest request
        )
		{
            AsyncResult<Result.SubMoldCapacityByUserIdResult> result = null;
			await SubMoldCapacityByUserId(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
    #else
		public SubMoldCapacityByUserIdTask SubMoldCapacityByUserIdAsync(
                Request.SubMoldCapacityByUserIdRequest request
        )
		{
			return new SubMoldCapacityByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.SubMoldCapacityByUserIdResult> SubMoldCapacityByUserIdAsync(
                Request.SubMoldCapacityByUserIdRequest request
        )
		{
			var task = new SubMoldCapacityByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class DeleteMoldTask : Gs2RestSessionTask<DeleteMoldRequest, DeleteMoldResult>
        {
            public DeleteMoldTask(IGs2Session session, RestSessionRequestFactory factory, DeleteMoldRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DeleteMoldRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "formation")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/me/mold/{moldModelName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{moldModelName}", !string.IsNullOrEmpty(request.MoldModelName) ? request.MoldModelName.ToString() : "null");

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
		public IEnumerator DeleteMold(
                Request.DeleteMoldRequest request,
                UnityAction<AsyncResult<Result.DeleteMoldResult>> callback
        )
		{
			var task = new DeleteMoldTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.DeleteMoldResult>(task.Result, task.Error));
        }

		public IFuture<Result.DeleteMoldResult> DeleteMoldFuture(
                Request.DeleteMoldRequest request
        )
		{
			return new DeleteMoldTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.DeleteMoldResult> DeleteMoldAsync(
                Request.DeleteMoldRequest request
        )
		{
            AsyncResult<Result.DeleteMoldResult> result = null;
			await DeleteMold(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
    #else
		public DeleteMoldTask DeleteMoldAsync(
                Request.DeleteMoldRequest request
        )
		{
			return new DeleteMoldTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.DeleteMoldResult> DeleteMoldAsync(
                Request.DeleteMoldRequest request
        )
		{
			var task = new DeleteMoldTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class DeleteMoldByUserIdTask : Gs2RestSessionTask<DeleteMoldByUserIdRequest, DeleteMoldByUserIdResult>
        {
            public DeleteMoldByUserIdTask(IGs2Session session, RestSessionRequestFactory factory, DeleteMoldByUserIdRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DeleteMoldByUserIdRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "formation")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/{userId}/mold/{moldModelName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{userId}", !string.IsNullOrEmpty(request.UserId) ? request.UserId.ToString() : "null");
                url = url.Replace("{moldModelName}", !string.IsNullOrEmpty(request.MoldModelName) ? request.MoldModelName.ToString() : "null");

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
		public IEnumerator DeleteMoldByUserId(
                Request.DeleteMoldByUserIdRequest request,
                UnityAction<AsyncResult<Result.DeleteMoldByUserIdResult>> callback
        )
		{
			var task = new DeleteMoldByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.DeleteMoldByUserIdResult>(task.Result, task.Error));
        }

		public IFuture<Result.DeleteMoldByUserIdResult> DeleteMoldByUserIdFuture(
                Request.DeleteMoldByUserIdRequest request
        )
		{
			return new DeleteMoldByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.DeleteMoldByUserIdResult> DeleteMoldByUserIdAsync(
                Request.DeleteMoldByUserIdRequest request
        )
		{
            AsyncResult<Result.DeleteMoldByUserIdResult> result = null;
			await DeleteMoldByUserId(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
    #else
		public DeleteMoldByUserIdTask DeleteMoldByUserIdAsync(
                Request.DeleteMoldByUserIdRequest request
        )
		{
			return new DeleteMoldByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.DeleteMoldByUserIdResult> DeleteMoldByUserIdAsync(
                Request.DeleteMoldByUserIdRequest request
        )
		{
			var task = new DeleteMoldByUserIdTask(
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
                    .Replace("{service}", "formation")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/stamp/mold/capacity/add";

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


        public class SubCapacityByStampTaskTask : Gs2RestSessionTask<SubCapacityByStampTaskRequest, SubCapacityByStampTaskResult>
        {
            public SubCapacityByStampTaskTask(IGs2Session session, RestSessionRequestFactory factory, SubCapacityByStampTaskRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(SubCapacityByStampTaskRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "formation")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/stamp/mold/capacity/sub";

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
		public IEnumerator SubCapacityByStampTask(
                Request.SubCapacityByStampTaskRequest request,
                UnityAction<AsyncResult<Result.SubCapacityByStampTaskResult>> callback
        )
		{
			var task = new SubCapacityByStampTaskTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.SubCapacityByStampTaskResult>(task.Result, task.Error));
        }

		public IFuture<Result.SubCapacityByStampTaskResult> SubCapacityByStampTaskFuture(
                Request.SubCapacityByStampTaskRequest request
        )
		{
			return new SubCapacityByStampTaskTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.SubCapacityByStampTaskResult> SubCapacityByStampTaskAsync(
                Request.SubCapacityByStampTaskRequest request
        )
		{
            AsyncResult<Result.SubCapacityByStampTaskResult> result = null;
			await SubCapacityByStampTask(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
    #else
		public SubCapacityByStampTaskTask SubCapacityByStampTaskAsync(
                Request.SubCapacityByStampTaskRequest request
        )
		{
			return new SubCapacityByStampTaskTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.SubCapacityByStampTaskResult> SubCapacityByStampTaskAsync(
                Request.SubCapacityByStampTaskRequest request
        )
		{
			var task = new SubCapacityByStampTaskTask(
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
                    .Replace("{service}", "formation")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/stamp/mold/capacity/set";

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


        public class DescribeFormsTask : Gs2RestSessionTask<DescribeFormsRequest, DescribeFormsResult>
        {
            public DescribeFormsTask(IGs2Session session, RestSessionRequestFactory factory, DescribeFormsRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DescribeFormsRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "formation")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/me/mold/{moldModelName}/form";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{moldModelName}", !string.IsNullOrEmpty(request.MoldModelName) ? request.MoldModelName.ToString() : "null");

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
		public IEnumerator DescribeForms(
                Request.DescribeFormsRequest request,
                UnityAction<AsyncResult<Result.DescribeFormsResult>> callback
        )
		{
			var task = new DescribeFormsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.DescribeFormsResult>(task.Result, task.Error));
        }

		public IFuture<Result.DescribeFormsResult> DescribeFormsFuture(
                Request.DescribeFormsRequest request
        )
		{
			return new DescribeFormsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.DescribeFormsResult> DescribeFormsAsync(
                Request.DescribeFormsRequest request
        )
		{
            AsyncResult<Result.DescribeFormsResult> result = null;
			await DescribeForms(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
    #else
		public DescribeFormsTask DescribeFormsAsync(
                Request.DescribeFormsRequest request
        )
		{
			return new DescribeFormsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.DescribeFormsResult> DescribeFormsAsync(
                Request.DescribeFormsRequest request
        )
		{
			var task = new DescribeFormsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class DescribeFormsByUserIdTask : Gs2RestSessionTask<DescribeFormsByUserIdRequest, DescribeFormsByUserIdResult>
        {
            public DescribeFormsByUserIdTask(IGs2Session session, RestSessionRequestFactory factory, DescribeFormsByUserIdRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DescribeFormsByUserIdRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "formation")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/{userId}/mold/{moldModelName}/form";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{moldModelName}", !string.IsNullOrEmpty(request.MoldModelName) ? request.MoldModelName.ToString() : "null");
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
		public IEnumerator DescribeFormsByUserId(
                Request.DescribeFormsByUserIdRequest request,
                UnityAction<AsyncResult<Result.DescribeFormsByUserIdResult>> callback
        )
		{
			var task = new DescribeFormsByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.DescribeFormsByUserIdResult>(task.Result, task.Error));
        }

		public IFuture<Result.DescribeFormsByUserIdResult> DescribeFormsByUserIdFuture(
                Request.DescribeFormsByUserIdRequest request
        )
		{
			return new DescribeFormsByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.DescribeFormsByUserIdResult> DescribeFormsByUserIdAsync(
                Request.DescribeFormsByUserIdRequest request
        )
		{
            AsyncResult<Result.DescribeFormsByUserIdResult> result = null;
			await DescribeFormsByUserId(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
    #else
		public DescribeFormsByUserIdTask DescribeFormsByUserIdAsync(
                Request.DescribeFormsByUserIdRequest request
        )
		{
			return new DescribeFormsByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.DescribeFormsByUserIdResult> DescribeFormsByUserIdAsync(
                Request.DescribeFormsByUserIdRequest request
        )
		{
			var task = new DescribeFormsByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class GetFormTask : Gs2RestSessionTask<GetFormRequest, GetFormResult>
        {
            public GetFormTask(IGs2Session session, RestSessionRequestFactory factory, GetFormRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(GetFormRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "formation")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/me/mold/{moldModelName}/form/{index}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{moldModelName}", !string.IsNullOrEmpty(request.MoldModelName) ? request.MoldModelName.ToString() : "null");
                url = url.Replace("{index}",request.Index != null ? request.Index.ToString() : "null");

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
		public IEnumerator GetForm(
                Request.GetFormRequest request,
                UnityAction<AsyncResult<Result.GetFormResult>> callback
        )
		{
			var task = new GetFormTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.GetFormResult>(task.Result, task.Error));
        }

		public IFuture<Result.GetFormResult> GetFormFuture(
                Request.GetFormRequest request
        )
		{
			return new GetFormTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.GetFormResult> GetFormAsync(
                Request.GetFormRequest request
        )
		{
            AsyncResult<Result.GetFormResult> result = null;
			await GetForm(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
    #else
		public GetFormTask GetFormAsync(
                Request.GetFormRequest request
        )
		{
			return new GetFormTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.GetFormResult> GetFormAsync(
                Request.GetFormRequest request
        )
		{
			var task = new GetFormTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class GetFormByUserIdTask : Gs2RestSessionTask<GetFormByUserIdRequest, GetFormByUserIdResult>
        {
            public GetFormByUserIdTask(IGs2Session session, RestSessionRequestFactory factory, GetFormByUserIdRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(GetFormByUserIdRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "formation")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/{userId}/mold/{moldModelName}/form/{index}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{userId}", !string.IsNullOrEmpty(request.UserId) ? request.UserId.ToString() : "null");
                url = url.Replace("{moldModelName}", !string.IsNullOrEmpty(request.MoldModelName) ? request.MoldModelName.ToString() : "null");
                url = url.Replace("{index}",request.Index != null ? request.Index.ToString() : "null");

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
		public IEnumerator GetFormByUserId(
                Request.GetFormByUserIdRequest request,
                UnityAction<AsyncResult<Result.GetFormByUserIdResult>> callback
        )
		{
			var task = new GetFormByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.GetFormByUserIdResult>(task.Result, task.Error));
        }

		public IFuture<Result.GetFormByUserIdResult> GetFormByUserIdFuture(
                Request.GetFormByUserIdRequest request
        )
		{
			return new GetFormByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.GetFormByUserIdResult> GetFormByUserIdAsync(
                Request.GetFormByUserIdRequest request
        )
		{
            AsyncResult<Result.GetFormByUserIdResult> result = null;
			await GetFormByUserId(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
    #else
		public GetFormByUserIdTask GetFormByUserIdAsync(
                Request.GetFormByUserIdRequest request
        )
		{
			return new GetFormByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.GetFormByUserIdResult> GetFormByUserIdAsync(
                Request.GetFormByUserIdRequest request
        )
		{
			var task = new GetFormByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class GetFormWithSignatureTask : Gs2RestSessionTask<GetFormWithSignatureRequest, GetFormWithSignatureResult>
        {
            public GetFormWithSignatureTask(IGs2Session session, RestSessionRequestFactory factory, GetFormWithSignatureRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(GetFormWithSignatureRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "formation")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/me/mold/{moldModelName}/form/{index}/signature";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{moldModelName}", !string.IsNullOrEmpty(request.MoldModelName) ? request.MoldModelName.ToString() : "null");
                url = url.Replace("{index}",request.Index != null ? request.Index.ToString() : "null");

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
		public IEnumerator GetFormWithSignature(
                Request.GetFormWithSignatureRequest request,
                UnityAction<AsyncResult<Result.GetFormWithSignatureResult>> callback
        )
		{
			var task = new GetFormWithSignatureTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.GetFormWithSignatureResult>(task.Result, task.Error));
        }

		public IFuture<Result.GetFormWithSignatureResult> GetFormWithSignatureFuture(
                Request.GetFormWithSignatureRequest request
        )
		{
			return new GetFormWithSignatureTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.GetFormWithSignatureResult> GetFormWithSignatureAsync(
                Request.GetFormWithSignatureRequest request
        )
		{
            AsyncResult<Result.GetFormWithSignatureResult> result = null;
			await GetFormWithSignature(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
    #else
		public GetFormWithSignatureTask GetFormWithSignatureAsync(
                Request.GetFormWithSignatureRequest request
        )
		{
			return new GetFormWithSignatureTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.GetFormWithSignatureResult> GetFormWithSignatureAsync(
                Request.GetFormWithSignatureRequest request
        )
		{
			var task = new GetFormWithSignatureTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class GetFormWithSignatureByUserIdTask : Gs2RestSessionTask<GetFormWithSignatureByUserIdRequest, GetFormWithSignatureByUserIdResult>
        {
            public GetFormWithSignatureByUserIdTask(IGs2Session session, RestSessionRequestFactory factory, GetFormWithSignatureByUserIdRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(GetFormWithSignatureByUserIdRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "formation")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/{userId}/mold/{moldModelName}/form/{index}/signature";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{userId}", !string.IsNullOrEmpty(request.UserId) ? request.UserId.ToString() : "null");
                url = url.Replace("{moldModelName}", !string.IsNullOrEmpty(request.MoldModelName) ? request.MoldModelName.ToString() : "null");
                url = url.Replace("{index}",request.Index != null ? request.Index.ToString() : "null");

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
		public IEnumerator GetFormWithSignatureByUserId(
                Request.GetFormWithSignatureByUserIdRequest request,
                UnityAction<AsyncResult<Result.GetFormWithSignatureByUserIdResult>> callback
        )
		{
			var task = new GetFormWithSignatureByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.GetFormWithSignatureByUserIdResult>(task.Result, task.Error));
        }

		public IFuture<Result.GetFormWithSignatureByUserIdResult> GetFormWithSignatureByUserIdFuture(
                Request.GetFormWithSignatureByUserIdRequest request
        )
		{
			return new GetFormWithSignatureByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.GetFormWithSignatureByUserIdResult> GetFormWithSignatureByUserIdAsync(
                Request.GetFormWithSignatureByUserIdRequest request
        )
		{
            AsyncResult<Result.GetFormWithSignatureByUserIdResult> result = null;
			await GetFormWithSignatureByUserId(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
    #else
		public GetFormWithSignatureByUserIdTask GetFormWithSignatureByUserIdAsync(
                Request.GetFormWithSignatureByUserIdRequest request
        )
		{
			return new GetFormWithSignatureByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.GetFormWithSignatureByUserIdResult> GetFormWithSignatureByUserIdAsync(
                Request.GetFormWithSignatureByUserIdRequest request
        )
		{
			var task = new GetFormWithSignatureByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class SetFormByUserIdTask : Gs2RestSessionTask<SetFormByUserIdRequest, SetFormByUserIdResult>
        {
            public SetFormByUserIdTask(IGs2Session session, RestSessionRequestFactory factory, SetFormByUserIdRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(SetFormByUserIdRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "formation")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/{userId}/mold/{moldModelName}/form/{index}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{userId}", !string.IsNullOrEmpty(request.UserId) ? request.UserId.ToString() : "null");
                url = url.Replace("{moldModelName}", !string.IsNullOrEmpty(request.MoldModelName) ? request.MoldModelName.ToString() : "null");
                url = url.Replace("{index}",request.Index != null ? request.Index.ToString() : "null");

                var sessionRequest = Factory.Put(url);

                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);
                jsonWriter.WriteObjectStart();
                if (request.Slots != null)
                {
                    jsonWriter.WritePropertyName("slots");
                    jsonWriter.WriteArrayStart();
                    foreach(var item in request.Slots)
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
		public IEnumerator SetFormByUserId(
                Request.SetFormByUserIdRequest request,
                UnityAction<AsyncResult<Result.SetFormByUserIdResult>> callback
        )
		{
			var task = new SetFormByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.SetFormByUserIdResult>(task.Result, task.Error));
        }

		public IFuture<Result.SetFormByUserIdResult> SetFormByUserIdFuture(
                Request.SetFormByUserIdRequest request
        )
		{
			return new SetFormByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.SetFormByUserIdResult> SetFormByUserIdAsync(
                Request.SetFormByUserIdRequest request
        )
		{
            AsyncResult<Result.SetFormByUserIdResult> result = null;
			await SetFormByUserId(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
    #else
		public SetFormByUserIdTask SetFormByUserIdAsync(
                Request.SetFormByUserIdRequest request
        )
		{
			return new SetFormByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.SetFormByUserIdResult> SetFormByUserIdAsync(
                Request.SetFormByUserIdRequest request
        )
		{
			var task = new SetFormByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class SetFormWithSignatureTask : Gs2RestSessionTask<SetFormWithSignatureRequest, SetFormWithSignatureResult>
        {
            public SetFormWithSignatureTask(IGs2Session session, RestSessionRequestFactory factory, SetFormWithSignatureRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(SetFormWithSignatureRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "formation")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/me/mold/{moldModelName}/form/{index}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{moldModelName}", !string.IsNullOrEmpty(request.MoldModelName) ? request.MoldModelName.ToString() : "null");
                url = url.Replace("{index}",request.Index != null ? request.Index.ToString() : "null");

                var sessionRequest = Factory.Put(url);

                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);
                jsonWriter.WriteObjectStart();
                if (request.Slots != null)
                {
                    jsonWriter.WritePropertyName("slots");
                    jsonWriter.WriteArrayStart();
                    foreach(var item in request.Slots)
                    {
                        item.WriteJson(jsonWriter);
                    }
                    jsonWriter.WriteArrayEnd();
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
		public IEnumerator SetFormWithSignature(
                Request.SetFormWithSignatureRequest request,
                UnityAction<AsyncResult<Result.SetFormWithSignatureResult>> callback
        )
		{
			var task = new SetFormWithSignatureTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.SetFormWithSignatureResult>(task.Result, task.Error));
        }

		public IFuture<Result.SetFormWithSignatureResult> SetFormWithSignatureFuture(
                Request.SetFormWithSignatureRequest request
        )
		{
			return new SetFormWithSignatureTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.SetFormWithSignatureResult> SetFormWithSignatureAsync(
                Request.SetFormWithSignatureRequest request
        )
		{
            AsyncResult<Result.SetFormWithSignatureResult> result = null;
			await SetFormWithSignature(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
    #else
		public SetFormWithSignatureTask SetFormWithSignatureAsync(
                Request.SetFormWithSignatureRequest request
        )
		{
			return new SetFormWithSignatureTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.SetFormWithSignatureResult> SetFormWithSignatureAsync(
                Request.SetFormWithSignatureRequest request
        )
		{
			var task = new SetFormWithSignatureTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class AcquireActionsToFormPropertiesTask : Gs2RestSessionTask<AcquireActionsToFormPropertiesRequest, AcquireActionsToFormPropertiesResult>
        {
            public AcquireActionsToFormPropertiesTask(IGs2Session session, RestSessionRequestFactory factory, AcquireActionsToFormPropertiesRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(AcquireActionsToFormPropertiesRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "formation")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/{userId}/mold/{moldModelName}/form/{index}/stamp/delegate";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{userId}", !string.IsNullOrEmpty(request.UserId) ? request.UserId.ToString() : "null");
                url = url.Replace("{moldModelName}", !string.IsNullOrEmpty(request.MoldModelName) ? request.MoldModelName.ToString() : "null");
                url = url.Replace("{index}",request.Index != null ? request.Index.ToString() : "null");

                var sessionRequest = Factory.Post(url);

                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);
                jsonWriter.WriteObjectStart();
                if (request.AcquireAction != null)
                {
                    jsonWriter.WritePropertyName("acquireAction");
                    request.AcquireAction.WriteJson(jsonWriter);
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
		public IEnumerator AcquireActionsToFormProperties(
                Request.AcquireActionsToFormPropertiesRequest request,
                UnityAction<AsyncResult<Result.AcquireActionsToFormPropertiesResult>> callback
        )
		{
			var task = new AcquireActionsToFormPropertiesTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.AcquireActionsToFormPropertiesResult>(task.Result, task.Error));
        }

		public IFuture<Result.AcquireActionsToFormPropertiesResult> AcquireActionsToFormPropertiesFuture(
                Request.AcquireActionsToFormPropertiesRequest request
        )
		{
			return new AcquireActionsToFormPropertiesTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.AcquireActionsToFormPropertiesResult> AcquireActionsToFormPropertiesAsync(
                Request.AcquireActionsToFormPropertiesRequest request
        )
		{
            AsyncResult<Result.AcquireActionsToFormPropertiesResult> result = null;
			await AcquireActionsToFormProperties(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
    #else
		public AcquireActionsToFormPropertiesTask AcquireActionsToFormPropertiesAsync(
                Request.AcquireActionsToFormPropertiesRequest request
        )
		{
			return new AcquireActionsToFormPropertiesTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.AcquireActionsToFormPropertiesResult> AcquireActionsToFormPropertiesAsync(
                Request.AcquireActionsToFormPropertiesRequest request
        )
		{
			var task = new AcquireActionsToFormPropertiesTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class DeleteFormTask : Gs2RestSessionTask<DeleteFormRequest, DeleteFormResult>
        {
            public DeleteFormTask(IGs2Session session, RestSessionRequestFactory factory, DeleteFormRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DeleteFormRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "formation")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/me/mold/{moldModelName}/form/{index}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{moldModelName}", !string.IsNullOrEmpty(request.MoldModelName) ? request.MoldModelName.ToString() : "null");
                url = url.Replace("{index}",request.Index != null ? request.Index.ToString() : "null");

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
		public IEnumerator DeleteForm(
                Request.DeleteFormRequest request,
                UnityAction<AsyncResult<Result.DeleteFormResult>> callback
        )
		{
			var task = new DeleteFormTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.DeleteFormResult>(task.Result, task.Error));
        }

		public IFuture<Result.DeleteFormResult> DeleteFormFuture(
                Request.DeleteFormRequest request
        )
		{
			return new DeleteFormTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.DeleteFormResult> DeleteFormAsync(
                Request.DeleteFormRequest request
        )
		{
            AsyncResult<Result.DeleteFormResult> result = null;
			await DeleteForm(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
    #else
		public DeleteFormTask DeleteFormAsync(
                Request.DeleteFormRequest request
        )
		{
			return new DeleteFormTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.DeleteFormResult> DeleteFormAsync(
                Request.DeleteFormRequest request
        )
		{
			var task = new DeleteFormTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class DeleteFormByUserIdTask : Gs2RestSessionTask<DeleteFormByUserIdRequest, DeleteFormByUserIdResult>
        {
            public DeleteFormByUserIdTask(IGs2Session session, RestSessionRequestFactory factory, DeleteFormByUserIdRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DeleteFormByUserIdRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "formation")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/{userId}/mold/{moldModelName}/form/{index}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{userId}", !string.IsNullOrEmpty(request.UserId) ? request.UserId.ToString() : "null");
                url = url.Replace("{moldModelName}", !string.IsNullOrEmpty(request.MoldModelName) ? request.MoldModelName.ToString() : "null");
                url = url.Replace("{index}",request.Index != null ? request.Index.ToString() : "null");

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
		public IEnumerator DeleteFormByUserId(
                Request.DeleteFormByUserIdRequest request,
                UnityAction<AsyncResult<Result.DeleteFormByUserIdResult>> callback
        )
		{
			var task = new DeleteFormByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.DeleteFormByUserIdResult>(task.Result, task.Error));
        }

		public IFuture<Result.DeleteFormByUserIdResult> DeleteFormByUserIdFuture(
                Request.DeleteFormByUserIdRequest request
        )
		{
			return new DeleteFormByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.DeleteFormByUserIdResult> DeleteFormByUserIdAsync(
                Request.DeleteFormByUserIdRequest request
        )
		{
            AsyncResult<Result.DeleteFormByUserIdResult> result = null;
			await DeleteFormByUserId(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
    #else
		public DeleteFormByUserIdTask DeleteFormByUserIdAsync(
                Request.DeleteFormByUserIdRequest request
        )
		{
			return new DeleteFormByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.DeleteFormByUserIdResult> DeleteFormByUserIdAsync(
                Request.DeleteFormByUserIdRequest request
        )
		{
			var task = new DeleteFormByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class AcquireActionToFormPropertiesByStampSheetTask : Gs2RestSessionTask<AcquireActionToFormPropertiesByStampSheetRequest, AcquireActionToFormPropertiesByStampSheetResult>
        {
            public AcquireActionToFormPropertiesByStampSheetTask(IGs2Session session, RestSessionRequestFactory factory, AcquireActionToFormPropertiesByStampSheetRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(AcquireActionToFormPropertiesByStampSheetRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "formation")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/stamp/form/acquire";

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
		public IEnumerator AcquireActionToFormPropertiesByStampSheet(
                Request.AcquireActionToFormPropertiesByStampSheetRequest request,
                UnityAction<AsyncResult<Result.AcquireActionToFormPropertiesByStampSheetResult>> callback
        )
		{
			var task = new AcquireActionToFormPropertiesByStampSheetTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.AcquireActionToFormPropertiesByStampSheetResult>(task.Result, task.Error));
        }

		public IFuture<Result.AcquireActionToFormPropertiesByStampSheetResult> AcquireActionToFormPropertiesByStampSheetFuture(
                Request.AcquireActionToFormPropertiesByStampSheetRequest request
        )
		{
			return new AcquireActionToFormPropertiesByStampSheetTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.AcquireActionToFormPropertiesByStampSheetResult> AcquireActionToFormPropertiesByStampSheetAsync(
                Request.AcquireActionToFormPropertiesByStampSheetRequest request
        )
		{
            AsyncResult<Result.AcquireActionToFormPropertiesByStampSheetResult> result = null;
			await AcquireActionToFormPropertiesByStampSheet(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
    #else
		public AcquireActionToFormPropertiesByStampSheetTask AcquireActionToFormPropertiesByStampSheetAsync(
                Request.AcquireActionToFormPropertiesByStampSheetRequest request
        )
		{
			return new AcquireActionToFormPropertiesByStampSheetTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.AcquireActionToFormPropertiesByStampSheetResult> AcquireActionToFormPropertiesByStampSheetAsync(
                Request.AcquireActionToFormPropertiesByStampSheetRequest request
        )
		{
			var task = new AcquireActionToFormPropertiesByStampSheetTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class SetFormByStampSheetTask : Gs2RestSessionTask<SetFormByStampSheetRequest, SetFormByStampSheetResult>
        {
            public SetFormByStampSheetTask(IGs2Session session, RestSessionRequestFactory factory, SetFormByStampSheetRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(SetFormByStampSheetRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "formation")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/stamp/form/set";

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
		public IEnumerator SetFormByStampSheet(
                Request.SetFormByStampSheetRequest request,
                UnityAction<AsyncResult<Result.SetFormByStampSheetResult>> callback
        )
		{
			var task = new SetFormByStampSheetTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.SetFormByStampSheetResult>(task.Result, task.Error));
        }

		public IFuture<Result.SetFormByStampSheetResult> SetFormByStampSheetFuture(
                Request.SetFormByStampSheetRequest request
        )
		{
			return new SetFormByStampSheetTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.SetFormByStampSheetResult> SetFormByStampSheetAsync(
                Request.SetFormByStampSheetRequest request
        )
		{
            AsyncResult<Result.SetFormByStampSheetResult> result = null;
			await SetFormByStampSheet(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
    #else
		public SetFormByStampSheetTask SetFormByStampSheetAsync(
                Request.SetFormByStampSheetRequest request
        )
		{
			return new SetFormByStampSheetTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.SetFormByStampSheetResult> SetFormByStampSheetAsync(
                Request.SetFormByStampSheetRequest request
        )
		{
			var task = new SetFormByStampSheetTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class DescribePropertyFormsTask : Gs2RestSessionTask<DescribePropertyFormsRequest, DescribePropertyFormsResult>
        {
            public DescribePropertyFormsTask(IGs2Session session, RestSessionRequestFactory factory, DescribePropertyFormsRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DescribePropertyFormsRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "formation")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/me/property/{propertyFormModelName}/form";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{propertyFormModelName}", !string.IsNullOrEmpty(request.PropertyFormModelName) ? request.PropertyFormModelName.ToString() : "null");

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
		public IEnumerator DescribePropertyForms(
                Request.DescribePropertyFormsRequest request,
                UnityAction<AsyncResult<Result.DescribePropertyFormsResult>> callback
        )
		{
			var task = new DescribePropertyFormsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.DescribePropertyFormsResult>(task.Result, task.Error));
        }

		public IFuture<Result.DescribePropertyFormsResult> DescribePropertyFormsFuture(
                Request.DescribePropertyFormsRequest request
        )
		{
			return new DescribePropertyFormsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.DescribePropertyFormsResult> DescribePropertyFormsAsync(
                Request.DescribePropertyFormsRequest request
        )
		{
            AsyncResult<Result.DescribePropertyFormsResult> result = null;
			await DescribePropertyForms(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
    #else
		public DescribePropertyFormsTask DescribePropertyFormsAsync(
                Request.DescribePropertyFormsRequest request
        )
		{
			return new DescribePropertyFormsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.DescribePropertyFormsResult> DescribePropertyFormsAsync(
                Request.DescribePropertyFormsRequest request
        )
		{
			var task = new DescribePropertyFormsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class DescribePropertyFormsByUserIdTask : Gs2RestSessionTask<DescribePropertyFormsByUserIdRequest, DescribePropertyFormsByUserIdResult>
        {
            public DescribePropertyFormsByUserIdTask(IGs2Session session, RestSessionRequestFactory factory, DescribePropertyFormsByUserIdRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DescribePropertyFormsByUserIdRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "formation")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/{userId}/property/{propertyFormModelName}/form";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{userId}", !string.IsNullOrEmpty(request.UserId) ? request.UserId.ToString() : "null");
                url = url.Replace("{propertyFormModelName}", !string.IsNullOrEmpty(request.PropertyFormModelName) ? request.PropertyFormModelName.ToString() : "null");

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
		public IEnumerator DescribePropertyFormsByUserId(
                Request.DescribePropertyFormsByUserIdRequest request,
                UnityAction<AsyncResult<Result.DescribePropertyFormsByUserIdResult>> callback
        )
		{
			var task = new DescribePropertyFormsByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.DescribePropertyFormsByUserIdResult>(task.Result, task.Error));
        }

		public IFuture<Result.DescribePropertyFormsByUserIdResult> DescribePropertyFormsByUserIdFuture(
                Request.DescribePropertyFormsByUserIdRequest request
        )
		{
			return new DescribePropertyFormsByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.DescribePropertyFormsByUserIdResult> DescribePropertyFormsByUserIdAsync(
                Request.DescribePropertyFormsByUserIdRequest request
        )
		{
            AsyncResult<Result.DescribePropertyFormsByUserIdResult> result = null;
			await DescribePropertyFormsByUserId(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
    #else
		public DescribePropertyFormsByUserIdTask DescribePropertyFormsByUserIdAsync(
                Request.DescribePropertyFormsByUserIdRequest request
        )
		{
			return new DescribePropertyFormsByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.DescribePropertyFormsByUserIdResult> DescribePropertyFormsByUserIdAsync(
                Request.DescribePropertyFormsByUserIdRequest request
        )
		{
			var task = new DescribePropertyFormsByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class GetPropertyFormTask : Gs2RestSessionTask<GetPropertyFormRequest, GetPropertyFormResult>
        {
            public GetPropertyFormTask(IGs2Session session, RestSessionRequestFactory factory, GetPropertyFormRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(GetPropertyFormRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "formation")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/me/property/{propertyFormModelName}/form/{propertyId}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{propertyFormModelName}", !string.IsNullOrEmpty(request.PropertyFormModelName) ? request.PropertyFormModelName.ToString() : "null");
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
		public IEnumerator GetPropertyForm(
                Request.GetPropertyFormRequest request,
                UnityAction<AsyncResult<Result.GetPropertyFormResult>> callback
        )
		{
			var task = new GetPropertyFormTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.GetPropertyFormResult>(task.Result, task.Error));
        }

		public IFuture<Result.GetPropertyFormResult> GetPropertyFormFuture(
                Request.GetPropertyFormRequest request
        )
		{
			return new GetPropertyFormTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.GetPropertyFormResult> GetPropertyFormAsync(
                Request.GetPropertyFormRequest request
        )
		{
            AsyncResult<Result.GetPropertyFormResult> result = null;
			await GetPropertyForm(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
    #else
		public GetPropertyFormTask GetPropertyFormAsync(
                Request.GetPropertyFormRequest request
        )
		{
			return new GetPropertyFormTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.GetPropertyFormResult> GetPropertyFormAsync(
                Request.GetPropertyFormRequest request
        )
		{
			var task = new GetPropertyFormTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class GetPropertyFormByUserIdTask : Gs2RestSessionTask<GetPropertyFormByUserIdRequest, GetPropertyFormByUserIdResult>
        {
            public GetPropertyFormByUserIdTask(IGs2Session session, RestSessionRequestFactory factory, GetPropertyFormByUserIdRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(GetPropertyFormByUserIdRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "formation")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/{userId}/property/{propertyFormModelName}/form/{propertyId}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{userId}", !string.IsNullOrEmpty(request.UserId) ? request.UserId.ToString() : "null");
                url = url.Replace("{propertyFormModelName}", !string.IsNullOrEmpty(request.PropertyFormModelName) ? request.PropertyFormModelName.ToString() : "null");
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
		public IEnumerator GetPropertyFormByUserId(
                Request.GetPropertyFormByUserIdRequest request,
                UnityAction<AsyncResult<Result.GetPropertyFormByUserIdResult>> callback
        )
		{
			var task = new GetPropertyFormByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.GetPropertyFormByUserIdResult>(task.Result, task.Error));
        }

		public IFuture<Result.GetPropertyFormByUserIdResult> GetPropertyFormByUserIdFuture(
                Request.GetPropertyFormByUserIdRequest request
        )
		{
			return new GetPropertyFormByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.GetPropertyFormByUserIdResult> GetPropertyFormByUserIdAsync(
                Request.GetPropertyFormByUserIdRequest request
        )
		{
            AsyncResult<Result.GetPropertyFormByUserIdResult> result = null;
			await GetPropertyFormByUserId(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
    #else
		public GetPropertyFormByUserIdTask GetPropertyFormByUserIdAsync(
                Request.GetPropertyFormByUserIdRequest request
        )
		{
			return new GetPropertyFormByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.GetPropertyFormByUserIdResult> GetPropertyFormByUserIdAsync(
                Request.GetPropertyFormByUserIdRequest request
        )
		{
			var task = new GetPropertyFormByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class GetPropertyFormWithSignatureTask : Gs2RestSessionTask<GetPropertyFormWithSignatureRequest, GetPropertyFormWithSignatureResult>
        {
            public GetPropertyFormWithSignatureTask(IGs2Session session, RestSessionRequestFactory factory, GetPropertyFormWithSignatureRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(GetPropertyFormWithSignatureRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "formation")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/me/property/{propertyFormModelName}/form/{propertyId}/signature";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{propertyFormModelName}", !string.IsNullOrEmpty(request.PropertyFormModelName) ? request.PropertyFormModelName.ToString() : "null");
                url = url.Replace("{propertyId}", !string.IsNullOrEmpty(request.PropertyId) ? request.PropertyId.ToString() : "null");

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
		public IEnumerator GetPropertyFormWithSignature(
                Request.GetPropertyFormWithSignatureRequest request,
                UnityAction<AsyncResult<Result.GetPropertyFormWithSignatureResult>> callback
        )
		{
			var task = new GetPropertyFormWithSignatureTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.GetPropertyFormWithSignatureResult>(task.Result, task.Error));
        }

		public IFuture<Result.GetPropertyFormWithSignatureResult> GetPropertyFormWithSignatureFuture(
                Request.GetPropertyFormWithSignatureRequest request
        )
		{
			return new GetPropertyFormWithSignatureTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.GetPropertyFormWithSignatureResult> GetPropertyFormWithSignatureAsync(
                Request.GetPropertyFormWithSignatureRequest request
        )
		{
            AsyncResult<Result.GetPropertyFormWithSignatureResult> result = null;
			await GetPropertyFormWithSignature(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
    #else
		public GetPropertyFormWithSignatureTask GetPropertyFormWithSignatureAsync(
                Request.GetPropertyFormWithSignatureRequest request
        )
		{
			return new GetPropertyFormWithSignatureTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.GetPropertyFormWithSignatureResult> GetPropertyFormWithSignatureAsync(
                Request.GetPropertyFormWithSignatureRequest request
        )
		{
			var task = new GetPropertyFormWithSignatureTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class GetPropertyFormWithSignatureByUserIdTask : Gs2RestSessionTask<GetPropertyFormWithSignatureByUserIdRequest, GetPropertyFormWithSignatureByUserIdResult>
        {
            public GetPropertyFormWithSignatureByUserIdTask(IGs2Session session, RestSessionRequestFactory factory, GetPropertyFormWithSignatureByUserIdRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(GetPropertyFormWithSignatureByUserIdRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "formation")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/{userId}/property/{propertyFormModelName}/form/{propertyId}/signature";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{userId}", !string.IsNullOrEmpty(request.UserId) ? request.UserId.ToString() : "null");
                url = url.Replace("{propertyFormModelName}", !string.IsNullOrEmpty(request.PropertyFormModelName) ? request.PropertyFormModelName.ToString() : "null");
                url = url.Replace("{propertyId}", !string.IsNullOrEmpty(request.PropertyId) ? request.PropertyId.ToString() : "null");

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
		public IEnumerator GetPropertyFormWithSignatureByUserId(
                Request.GetPropertyFormWithSignatureByUserIdRequest request,
                UnityAction<AsyncResult<Result.GetPropertyFormWithSignatureByUserIdResult>> callback
        )
		{
			var task = new GetPropertyFormWithSignatureByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.GetPropertyFormWithSignatureByUserIdResult>(task.Result, task.Error));
        }

		public IFuture<Result.GetPropertyFormWithSignatureByUserIdResult> GetPropertyFormWithSignatureByUserIdFuture(
                Request.GetPropertyFormWithSignatureByUserIdRequest request
        )
		{
			return new GetPropertyFormWithSignatureByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.GetPropertyFormWithSignatureByUserIdResult> GetPropertyFormWithSignatureByUserIdAsync(
                Request.GetPropertyFormWithSignatureByUserIdRequest request
        )
		{
            AsyncResult<Result.GetPropertyFormWithSignatureByUserIdResult> result = null;
			await GetPropertyFormWithSignatureByUserId(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
    #else
		public GetPropertyFormWithSignatureByUserIdTask GetPropertyFormWithSignatureByUserIdAsync(
                Request.GetPropertyFormWithSignatureByUserIdRequest request
        )
		{
			return new GetPropertyFormWithSignatureByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.GetPropertyFormWithSignatureByUserIdResult> GetPropertyFormWithSignatureByUserIdAsync(
                Request.GetPropertyFormWithSignatureByUserIdRequest request
        )
		{
			var task = new GetPropertyFormWithSignatureByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class SetPropertyFormByUserIdTask : Gs2RestSessionTask<SetPropertyFormByUserIdRequest, SetPropertyFormByUserIdResult>
        {
            public SetPropertyFormByUserIdTask(IGs2Session session, RestSessionRequestFactory factory, SetPropertyFormByUserIdRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(SetPropertyFormByUserIdRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "formation")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/{userId}/property/{propertyFormModelName}/form/{propertyId}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{userId}", !string.IsNullOrEmpty(request.UserId) ? request.UserId.ToString() : "null");
                url = url.Replace("{propertyFormModelName}", !string.IsNullOrEmpty(request.PropertyFormModelName) ? request.PropertyFormModelName.ToString() : "null");
                url = url.Replace("{propertyId}", !string.IsNullOrEmpty(request.PropertyId) ? request.PropertyId.ToString() : "null");

                var sessionRequest = Factory.Put(url);

                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);
                jsonWriter.WriteObjectStart();
                if (request.Slots != null)
                {
                    jsonWriter.WritePropertyName("slots");
                    jsonWriter.WriteArrayStart();
                    foreach(var item in request.Slots)
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
		public IEnumerator SetPropertyFormByUserId(
                Request.SetPropertyFormByUserIdRequest request,
                UnityAction<AsyncResult<Result.SetPropertyFormByUserIdResult>> callback
        )
		{
			var task = new SetPropertyFormByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.SetPropertyFormByUserIdResult>(task.Result, task.Error));
        }

		public IFuture<Result.SetPropertyFormByUserIdResult> SetPropertyFormByUserIdFuture(
                Request.SetPropertyFormByUserIdRequest request
        )
		{
			return new SetPropertyFormByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.SetPropertyFormByUserIdResult> SetPropertyFormByUserIdAsync(
                Request.SetPropertyFormByUserIdRequest request
        )
		{
            AsyncResult<Result.SetPropertyFormByUserIdResult> result = null;
			await SetPropertyFormByUserId(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
    #else
		public SetPropertyFormByUserIdTask SetPropertyFormByUserIdAsync(
                Request.SetPropertyFormByUserIdRequest request
        )
		{
			return new SetPropertyFormByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.SetPropertyFormByUserIdResult> SetPropertyFormByUserIdAsync(
                Request.SetPropertyFormByUserIdRequest request
        )
		{
			var task = new SetPropertyFormByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class SetPropertyFormWithSignatureTask : Gs2RestSessionTask<SetPropertyFormWithSignatureRequest, SetPropertyFormWithSignatureResult>
        {
            public SetPropertyFormWithSignatureTask(IGs2Session session, RestSessionRequestFactory factory, SetPropertyFormWithSignatureRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(SetPropertyFormWithSignatureRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "formation")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/me/property/{propertyFormModelName}/form/{propertyId}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{propertyFormModelName}", !string.IsNullOrEmpty(request.PropertyFormModelName) ? request.PropertyFormModelName.ToString() : "null");
                url = url.Replace("{propertyId}", !string.IsNullOrEmpty(request.PropertyId) ? request.PropertyId.ToString() : "null");

                var sessionRequest = Factory.Put(url);

                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);
                jsonWriter.WriteObjectStart();
                if (request.Slots != null)
                {
                    jsonWriter.WritePropertyName("slots");
                    jsonWriter.WriteArrayStart();
                    foreach(var item in request.Slots)
                    {
                        item.WriteJson(jsonWriter);
                    }
                    jsonWriter.WriteArrayEnd();
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
		public IEnumerator SetPropertyFormWithSignature(
                Request.SetPropertyFormWithSignatureRequest request,
                UnityAction<AsyncResult<Result.SetPropertyFormWithSignatureResult>> callback
        )
		{
			var task = new SetPropertyFormWithSignatureTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.SetPropertyFormWithSignatureResult>(task.Result, task.Error));
        }

		public IFuture<Result.SetPropertyFormWithSignatureResult> SetPropertyFormWithSignatureFuture(
                Request.SetPropertyFormWithSignatureRequest request
        )
		{
			return new SetPropertyFormWithSignatureTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.SetPropertyFormWithSignatureResult> SetPropertyFormWithSignatureAsync(
                Request.SetPropertyFormWithSignatureRequest request
        )
		{
            AsyncResult<Result.SetPropertyFormWithSignatureResult> result = null;
			await SetPropertyFormWithSignature(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
    #else
		public SetPropertyFormWithSignatureTask SetPropertyFormWithSignatureAsync(
                Request.SetPropertyFormWithSignatureRequest request
        )
		{
			return new SetPropertyFormWithSignatureTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.SetPropertyFormWithSignatureResult> SetPropertyFormWithSignatureAsync(
                Request.SetPropertyFormWithSignatureRequest request
        )
		{
			var task = new SetPropertyFormWithSignatureTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class AcquireActionsToPropertyFormPropertiesTask : Gs2RestSessionTask<AcquireActionsToPropertyFormPropertiesRequest, AcquireActionsToPropertyFormPropertiesResult>
        {
            public AcquireActionsToPropertyFormPropertiesTask(IGs2Session session, RestSessionRequestFactory factory, AcquireActionsToPropertyFormPropertiesRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(AcquireActionsToPropertyFormPropertiesRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "formation")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/{userId}/property/{propertyFormModelName}/form/{propertyId}/stamp/delegate";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{userId}", !string.IsNullOrEmpty(request.UserId) ? request.UserId.ToString() : "null");
                url = url.Replace("{propertyFormModelName}", !string.IsNullOrEmpty(request.PropertyFormModelName) ? request.PropertyFormModelName.ToString() : "null");
                url = url.Replace("{propertyId}", !string.IsNullOrEmpty(request.PropertyId) ? request.PropertyId.ToString() : "null");

                var sessionRequest = Factory.Post(url);

                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);
                jsonWriter.WriteObjectStart();
                if (request.AcquireAction != null)
                {
                    jsonWriter.WritePropertyName("acquireAction");
                    request.AcquireAction.WriteJson(jsonWriter);
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
		public IEnumerator AcquireActionsToPropertyFormProperties(
                Request.AcquireActionsToPropertyFormPropertiesRequest request,
                UnityAction<AsyncResult<Result.AcquireActionsToPropertyFormPropertiesResult>> callback
        )
		{
			var task = new AcquireActionsToPropertyFormPropertiesTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.AcquireActionsToPropertyFormPropertiesResult>(task.Result, task.Error));
        }

		public IFuture<Result.AcquireActionsToPropertyFormPropertiesResult> AcquireActionsToPropertyFormPropertiesFuture(
                Request.AcquireActionsToPropertyFormPropertiesRequest request
        )
		{
			return new AcquireActionsToPropertyFormPropertiesTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.AcquireActionsToPropertyFormPropertiesResult> AcquireActionsToPropertyFormPropertiesAsync(
                Request.AcquireActionsToPropertyFormPropertiesRequest request
        )
		{
            AsyncResult<Result.AcquireActionsToPropertyFormPropertiesResult> result = null;
			await AcquireActionsToPropertyFormProperties(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
    #else
		public AcquireActionsToPropertyFormPropertiesTask AcquireActionsToPropertyFormPropertiesAsync(
                Request.AcquireActionsToPropertyFormPropertiesRequest request
        )
		{
			return new AcquireActionsToPropertyFormPropertiesTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.AcquireActionsToPropertyFormPropertiesResult> AcquireActionsToPropertyFormPropertiesAsync(
                Request.AcquireActionsToPropertyFormPropertiesRequest request
        )
		{
			var task = new AcquireActionsToPropertyFormPropertiesTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class DeletePropertyFormTask : Gs2RestSessionTask<DeletePropertyFormRequest, DeletePropertyFormResult>
        {
            public DeletePropertyFormTask(IGs2Session session, RestSessionRequestFactory factory, DeletePropertyFormRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DeletePropertyFormRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "formation")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/me/property/{propertyFormModelName}/form/{propertyId}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{propertyFormModelName}", !string.IsNullOrEmpty(request.PropertyFormModelName) ? request.PropertyFormModelName.ToString() : "null");
                url = url.Replace("{propertyId}", !string.IsNullOrEmpty(request.PropertyId) ? request.PropertyId.ToString() : "null");

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
		public IEnumerator DeletePropertyForm(
                Request.DeletePropertyFormRequest request,
                UnityAction<AsyncResult<Result.DeletePropertyFormResult>> callback
        )
		{
			var task = new DeletePropertyFormTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.DeletePropertyFormResult>(task.Result, task.Error));
        }

		public IFuture<Result.DeletePropertyFormResult> DeletePropertyFormFuture(
                Request.DeletePropertyFormRequest request
        )
		{
			return new DeletePropertyFormTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.DeletePropertyFormResult> DeletePropertyFormAsync(
                Request.DeletePropertyFormRequest request
        )
		{
            AsyncResult<Result.DeletePropertyFormResult> result = null;
			await DeletePropertyForm(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
    #else
		public DeletePropertyFormTask DeletePropertyFormAsync(
                Request.DeletePropertyFormRequest request
        )
		{
			return new DeletePropertyFormTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.DeletePropertyFormResult> DeletePropertyFormAsync(
                Request.DeletePropertyFormRequest request
        )
		{
			var task = new DeletePropertyFormTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class DeletePropertyFormByUserIdTask : Gs2RestSessionTask<DeletePropertyFormByUserIdRequest, DeletePropertyFormByUserIdResult>
        {
            public DeletePropertyFormByUserIdTask(IGs2Session session, RestSessionRequestFactory factory, DeletePropertyFormByUserIdRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DeletePropertyFormByUserIdRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "formation")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/{userId}/property/{propertyFormModelName}/form/{propertyId}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{userId}", !string.IsNullOrEmpty(request.UserId) ? request.UserId.ToString() : "null");
                url = url.Replace("{propertyFormModelName}", !string.IsNullOrEmpty(request.PropertyFormModelName) ? request.PropertyFormModelName.ToString() : "null");
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
		public IEnumerator DeletePropertyFormByUserId(
                Request.DeletePropertyFormByUserIdRequest request,
                UnityAction<AsyncResult<Result.DeletePropertyFormByUserIdResult>> callback
        )
		{
			var task = new DeletePropertyFormByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.DeletePropertyFormByUserIdResult>(task.Result, task.Error));
        }

		public IFuture<Result.DeletePropertyFormByUserIdResult> DeletePropertyFormByUserIdFuture(
                Request.DeletePropertyFormByUserIdRequest request
        )
		{
			return new DeletePropertyFormByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.DeletePropertyFormByUserIdResult> DeletePropertyFormByUserIdAsync(
                Request.DeletePropertyFormByUserIdRequest request
        )
		{
            AsyncResult<Result.DeletePropertyFormByUserIdResult> result = null;
			await DeletePropertyFormByUserId(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
    #else
		public DeletePropertyFormByUserIdTask DeletePropertyFormByUserIdAsync(
                Request.DeletePropertyFormByUserIdRequest request
        )
		{
			return new DeletePropertyFormByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.DeletePropertyFormByUserIdResult> DeletePropertyFormByUserIdAsync(
                Request.DeletePropertyFormByUserIdRequest request
        )
		{
			var task = new DeletePropertyFormByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class AcquireActionToPropertyFormPropertiesByStampSheetTask : Gs2RestSessionTask<AcquireActionToPropertyFormPropertiesByStampSheetRequest, AcquireActionToPropertyFormPropertiesByStampSheetResult>
        {
            public AcquireActionToPropertyFormPropertiesByStampSheetTask(IGs2Session session, RestSessionRequestFactory factory, AcquireActionToPropertyFormPropertiesByStampSheetRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(AcquireActionToPropertyFormPropertiesByStampSheetRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "formation")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/stamp/property/form/acquire";

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
		public IEnumerator AcquireActionToPropertyFormPropertiesByStampSheet(
                Request.AcquireActionToPropertyFormPropertiesByStampSheetRequest request,
                UnityAction<AsyncResult<Result.AcquireActionToPropertyFormPropertiesByStampSheetResult>> callback
        )
		{
			var task = new AcquireActionToPropertyFormPropertiesByStampSheetTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.AcquireActionToPropertyFormPropertiesByStampSheetResult>(task.Result, task.Error));
        }

		public IFuture<Result.AcquireActionToPropertyFormPropertiesByStampSheetResult> AcquireActionToPropertyFormPropertiesByStampSheetFuture(
                Request.AcquireActionToPropertyFormPropertiesByStampSheetRequest request
        )
		{
			return new AcquireActionToPropertyFormPropertiesByStampSheetTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.AcquireActionToPropertyFormPropertiesByStampSheetResult> AcquireActionToPropertyFormPropertiesByStampSheetAsync(
                Request.AcquireActionToPropertyFormPropertiesByStampSheetRequest request
        )
		{
            AsyncResult<Result.AcquireActionToPropertyFormPropertiesByStampSheetResult> result = null;
			await AcquireActionToPropertyFormPropertiesByStampSheet(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
    #else
		public AcquireActionToPropertyFormPropertiesByStampSheetTask AcquireActionToPropertyFormPropertiesByStampSheetAsync(
                Request.AcquireActionToPropertyFormPropertiesByStampSheetRequest request
        )
		{
			return new AcquireActionToPropertyFormPropertiesByStampSheetTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.AcquireActionToPropertyFormPropertiesByStampSheetResult> AcquireActionToPropertyFormPropertiesByStampSheetAsync(
                Request.AcquireActionToPropertyFormPropertiesByStampSheetRequest request
        )
		{
			var task = new AcquireActionToPropertyFormPropertiesByStampSheetTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif
	}
}