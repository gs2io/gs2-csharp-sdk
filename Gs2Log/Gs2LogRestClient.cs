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
using Gs2.Gs2Log.Request;
using Gs2.Gs2Log.Result;
using Gs2.Util.LitJson;

namespace Gs2.Gs2Log
{
	public class Gs2LogRestClient : AbstractGs2Client
	{
#if UNITY_2017_1_OR_NEWER
		private readonly CertificateHandler _certificateHandler;
#endif

		public static string Endpoint = "log";

        protected Gs2RestSession Gs2RestSession => (Gs2RestSession) Gs2Session;

		public Gs2LogRestClient(Gs2RestSession Gs2RestSession) : base(Gs2RestSession)
		{

		}

#if UNITY_2017_1_OR_NEWER
		public Gs2LogRestClient(Gs2RestSession gs2RestSession, CertificateHandler certificateHandler) : base(gs2RestSession)
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
                    .Replace("{service}", "log")
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
                    .Replace("{service}", "log")
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
                if (request.Type != null)
                {
                    jsonWriter.WritePropertyName("type");
                    jsonWriter.Write(request.Type);
                }
                if (request.GcpCredentialJson != null)
                {
                    jsonWriter.WritePropertyName("gcpCredentialJson");
                    jsonWriter.Write(request.GcpCredentialJson);
                }
                if (request.BigQueryDatasetName != null)
                {
                    jsonWriter.WritePropertyName("bigQueryDatasetName");
                    jsonWriter.Write(request.BigQueryDatasetName);
                }
                if (request.LogExpireDays != null)
                {
                    jsonWriter.WritePropertyName("logExpireDays");
                    jsonWriter.Write(request.LogExpireDays.ToString());
                }
                if (request.AwsRegion != null)
                {
                    jsonWriter.WritePropertyName("awsRegion");
                    jsonWriter.Write(request.AwsRegion);
                }
                if (request.AwsAccessKeyId != null)
                {
                    jsonWriter.WritePropertyName("awsAccessKeyId");
                    jsonWriter.Write(request.AwsAccessKeyId);
                }
                if (request.AwsSecretAccessKey != null)
                {
                    jsonWriter.WritePropertyName("awsSecretAccessKey");
                    jsonWriter.Write(request.AwsSecretAccessKey);
                }
                if (request.FirehoseStreamName != null)
                {
                    jsonWriter.WritePropertyName("firehoseStreamName");
                    jsonWriter.Write(request.FirehoseStreamName);
                }
                if (request.FirehoseCompressData != null)
                {
                    jsonWriter.WritePropertyName("firehoseCompressData");
                    jsonWriter.Write(request.FirehoseCompressData);
                }
                if (request.ContextStack != null)
                {
                    jsonWriter.WritePropertyName("contextStack");
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
                    .Replace("{service}", "log")
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
                    .Replace("{service}", "log")
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
                    .Replace("{service}", "log")
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
                if (request.Type != null)
                {
                    jsonWriter.WritePropertyName("type");
                    jsonWriter.Write(request.Type);
                }
                if (request.GcpCredentialJson != null)
                {
                    jsonWriter.WritePropertyName("gcpCredentialJson");
                    jsonWriter.Write(request.GcpCredentialJson);
                }
                if (request.BigQueryDatasetName != null)
                {
                    jsonWriter.WritePropertyName("bigQueryDatasetName");
                    jsonWriter.Write(request.BigQueryDatasetName);
                }
                if (request.LogExpireDays != null)
                {
                    jsonWriter.WritePropertyName("logExpireDays");
                    jsonWriter.Write(request.LogExpireDays.ToString());
                }
                if (request.AwsRegion != null)
                {
                    jsonWriter.WritePropertyName("awsRegion");
                    jsonWriter.Write(request.AwsRegion);
                }
                if (request.AwsAccessKeyId != null)
                {
                    jsonWriter.WritePropertyName("awsAccessKeyId");
                    jsonWriter.Write(request.AwsAccessKeyId);
                }
                if (request.AwsSecretAccessKey != null)
                {
                    jsonWriter.WritePropertyName("awsSecretAccessKey");
                    jsonWriter.Write(request.AwsSecretAccessKey);
                }
                if (request.FirehoseStreamName != null)
                {
                    jsonWriter.WritePropertyName("firehoseStreamName");
                    jsonWriter.Write(request.FirehoseStreamName);
                }
                if (request.FirehoseCompressData != null)
                {
                    jsonWriter.WritePropertyName("firehoseCompressData");
                    jsonWriter.Write(request.FirehoseCompressData);
                }
                if (request.ContextStack != null)
                {
                    jsonWriter.WritePropertyName("contextStack");
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
                    .Replace("{service}", "log")
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


        public class GetServiceVersionTask : Gs2RestSessionTask<GetServiceVersionRequest, GetServiceVersionResult>
        {
            public GetServiceVersionTask(IGs2Session session, RestSessionRequestFactory factory, GetServiceVersionRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(GetServiceVersionRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "log")
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
        )
		{
			var task = new GetServiceVersionTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.GetServiceVersionResult>(task.Result, task.Error));
        }

		public IFuture<Result.GetServiceVersionResult> GetServiceVersionFuture(
                Request.GetServiceVersionRequest request
        )
		{
			return new GetServiceVersionTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.GetServiceVersionResult> GetServiceVersionAsync(
                Request.GetServiceVersionRequest request
        )
		{
            AsyncResult<Result.GetServiceVersionResult> result = null;
			await GetServiceVersion(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
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
		public async Task<Result.GetServiceVersionResult> GetServiceVersionAsync(
                Request.GetServiceVersionRequest request
        )
		{
			var task = new GetServiceVersionTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class QueryAccessLogTask : Gs2RestSessionTask<QueryAccessLogRequest, QueryAccessLogResult>
        {
            public QueryAccessLogTask(IGs2Session session, RestSessionRequestFactory factory, QueryAccessLogRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(QueryAccessLogRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "log")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/log/access";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");

                var sessionRequest = Factory.Get(url);
                if (request.ContextStack != null)
                {
                    sessionRequest.AddQueryString("contextStack", request.ContextStack);
                }
                if (request.Service != null) {
                    sessionRequest.AddQueryString("service", $"{request.Service}");
                }
                if (request.Method != null) {
                    sessionRequest.AddQueryString("method", $"{request.Method}");
                }
                if (request.UserId != null) {
                    sessionRequest.AddQueryString("userId", $"{request.UserId}");
                }
                if (request.Begin != null) {
                    sessionRequest.AddQueryString("begin", $"{request.Begin}");
                }
                if (request.End != null) {
                    sessionRequest.AddQueryString("end", $"{request.End}");
                }
                if (request.LongTerm != null) {
                    sessionRequest.AddQueryString("longTerm", $"{request.LongTerm}");
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
		public IEnumerator QueryAccessLog(
                Request.QueryAccessLogRequest request,
                UnityAction<AsyncResult<Result.QueryAccessLogResult>> callback
        )
		{
			var task = new QueryAccessLogTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.QueryAccessLogResult>(task.Result, task.Error));
        }

		public IFuture<Result.QueryAccessLogResult> QueryAccessLogFuture(
                Request.QueryAccessLogRequest request
        )
		{
			return new QueryAccessLogTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.QueryAccessLogResult> QueryAccessLogAsync(
                Request.QueryAccessLogRequest request
        )
		{
            AsyncResult<Result.QueryAccessLogResult> result = null;
			await QueryAccessLog(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
    #else
		public QueryAccessLogTask QueryAccessLogAsync(
                Request.QueryAccessLogRequest request
        )
		{
			return new QueryAccessLogTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.QueryAccessLogResult> QueryAccessLogAsync(
                Request.QueryAccessLogRequest request
        )
		{
			var task = new QueryAccessLogTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class CountAccessLogTask : Gs2RestSessionTask<CountAccessLogRequest, CountAccessLogResult>
        {
            public CountAccessLogTask(IGs2Session session, RestSessionRequestFactory factory, CountAccessLogRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(CountAccessLogRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "log")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/log/access/count";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");

                var sessionRequest = Factory.Get(url);
                if (request.ContextStack != null)
                {
                    sessionRequest.AddQueryString("contextStack", request.ContextStack);
                }
                if (request.Service != null) {
                    sessionRequest.AddQueryString("service", $"{request.Service}");
                }
                if (request.Method != null) {
                    sessionRequest.AddQueryString("method", $"{request.Method}");
                }
                if (request.UserId != null) {
                    sessionRequest.AddQueryString("userId", $"{request.UserId}");
                }
                if (request.Begin != null) {
                    sessionRequest.AddQueryString("begin", $"{request.Begin}");
                }
                if (request.End != null) {
                    sessionRequest.AddQueryString("end", $"{request.End}");
                }
                if (request.LongTerm != null) {
                    sessionRequest.AddQueryString("longTerm", $"{request.LongTerm}");
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
		public IEnumerator CountAccessLog(
                Request.CountAccessLogRequest request,
                UnityAction<AsyncResult<Result.CountAccessLogResult>> callback
        )
		{
			var task = new CountAccessLogTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.CountAccessLogResult>(task.Result, task.Error));
        }

		public IFuture<Result.CountAccessLogResult> CountAccessLogFuture(
                Request.CountAccessLogRequest request
        )
		{
			return new CountAccessLogTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.CountAccessLogResult> CountAccessLogAsync(
                Request.CountAccessLogRequest request
        )
		{
            AsyncResult<Result.CountAccessLogResult> result = null;
			await CountAccessLog(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
    #else
		public CountAccessLogTask CountAccessLogAsync(
                Request.CountAccessLogRequest request
        )
		{
			return new CountAccessLogTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.CountAccessLogResult> CountAccessLogAsync(
                Request.CountAccessLogRequest request
        )
		{
			var task = new CountAccessLogTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class QueryIssueStampSheetLogTask : Gs2RestSessionTask<QueryIssueStampSheetLogRequest, QueryIssueStampSheetLogResult>
        {
            public QueryIssueStampSheetLogTask(IGs2Session session, RestSessionRequestFactory factory, QueryIssueStampSheetLogRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(QueryIssueStampSheetLogRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "log")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/log/issue/stamp/sheet";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");

                var sessionRequest = Factory.Get(url);
                if (request.ContextStack != null)
                {
                    sessionRequest.AddQueryString("contextStack", request.ContextStack);
                }
                if (request.Service != null) {
                    sessionRequest.AddQueryString("service", $"{request.Service}");
                }
                if (request.Method != null) {
                    sessionRequest.AddQueryString("method", $"{request.Method}");
                }
                if (request.UserId != null) {
                    sessionRequest.AddQueryString("userId", $"{request.UserId}");
                }
                if (request.Action != null) {
                    sessionRequest.AddQueryString("action", $"{request.Action}");
                }
                if (request.Begin != null) {
                    sessionRequest.AddQueryString("begin", $"{request.Begin}");
                }
                if (request.End != null) {
                    sessionRequest.AddQueryString("end", $"{request.End}");
                }
                if (request.LongTerm != null) {
                    sessionRequest.AddQueryString("longTerm", $"{request.LongTerm}");
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
		public IEnumerator QueryIssueStampSheetLog(
                Request.QueryIssueStampSheetLogRequest request,
                UnityAction<AsyncResult<Result.QueryIssueStampSheetLogResult>> callback
        )
		{
			var task = new QueryIssueStampSheetLogTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.QueryIssueStampSheetLogResult>(task.Result, task.Error));
        }

		public IFuture<Result.QueryIssueStampSheetLogResult> QueryIssueStampSheetLogFuture(
                Request.QueryIssueStampSheetLogRequest request
        )
		{
			return new QueryIssueStampSheetLogTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.QueryIssueStampSheetLogResult> QueryIssueStampSheetLogAsync(
                Request.QueryIssueStampSheetLogRequest request
        )
		{
            AsyncResult<Result.QueryIssueStampSheetLogResult> result = null;
			await QueryIssueStampSheetLog(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
    #else
		public QueryIssueStampSheetLogTask QueryIssueStampSheetLogAsync(
                Request.QueryIssueStampSheetLogRequest request
        )
		{
			return new QueryIssueStampSheetLogTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.QueryIssueStampSheetLogResult> QueryIssueStampSheetLogAsync(
                Request.QueryIssueStampSheetLogRequest request
        )
		{
			var task = new QueryIssueStampSheetLogTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class CountIssueStampSheetLogTask : Gs2RestSessionTask<CountIssueStampSheetLogRequest, CountIssueStampSheetLogResult>
        {
            public CountIssueStampSheetLogTask(IGs2Session session, RestSessionRequestFactory factory, CountIssueStampSheetLogRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(CountIssueStampSheetLogRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "log")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/log/issue/stamp/sheet/count";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");

                var sessionRequest = Factory.Get(url);
                if (request.ContextStack != null)
                {
                    sessionRequest.AddQueryString("contextStack", request.ContextStack);
                }
                if (request.Service != null) {
                    sessionRequest.AddQueryString("service", $"{request.Service}");
                }
                if (request.Method != null) {
                    sessionRequest.AddQueryString("method", $"{request.Method}");
                }
                if (request.UserId != null) {
                    sessionRequest.AddQueryString("userId", $"{request.UserId}");
                }
                if (request.Action != null) {
                    sessionRequest.AddQueryString("action", $"{request.Action}");
                }
                if (request.Begin != null) {
                    sessionRequest.AddQueryString("begin", $"{request.Begin}");
                }
                if (request.End != null) {
                    sessionRequest.AddQueryString("end", $"{request.End}");
                }
                if (request.LongTerm != null) {
                    sessionRequest.AddQueryString("longTerm", $"{request.LongTerm}");
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
		public IEnumerator CountIssueStampSheetLog(
                Request.CountIssueStampSheetLogRequest request,
                UnityAction<AsyncResult<Result.CountIssueStampSheetLogResult>> callback
        )
		{
			var task = new CountIssueStampSheetLogTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.CountIssueStampSheetLogResult>(task.Result, task.Error));
        }

		public IFuture<Result.CountIssueStampSheetLogResult> CountIssueStampSheetLogFuture(
                Request.CountIssueStampSheetLogRequest request
        )
		{
			return new CountIssueStampSheetLogTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.CountIssueStampSheetLogResult> CountIssueStampSheetLogAsync(
                Request.CountIssueStampSheetLogRequest request
        )
		{
            AsyncResult<Result.CountIssueStampSheetLogResult> result = null;
			await CountIssueStampSheetLog(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
    #else
		public CountIssueStampSheetLogTask CountIssueStampSheetLogAsync(
                Request.CountIssueStampSheetLogRequest request
        )
		{
			return new CountIssueStampSheetLogTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.CountIssueStampSheetLogResult> CountIssueStampSheetLogAsync(
                Request.CountIssueStampSheetLogRequest request
        )
		{
			var task = new CountIssueStampSheetLogTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class QueryExecuteStampSheetLogTask : Gs2RestSessionTask<QueryExecuteStampSheetLogRequest, QueryExecuteStampSheetLogResult>
        {
            public QueryExecuteStampSheetLogTask(IGs2Session session, RestSessionRequestFactory factory, QueryExecuteStampSheetLogRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(QueryExecuteStampSheetLogRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "log")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/log/execute/stamp/sheet";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");

                var sessionRequest = Factory.Get(url);
                if (request.ContextStack != null)
                {
                    sessionRequest.AddQueryString("contextStack", request.ContextStack);
                }
                if (request.Service != null) {
                    sessionRequest.AddQueryString("service", $"{request.Service}");
                }
                if (request.Method != null) {
                    sessionRequest.AddQueryString("method", $"{request.Method}");
                }
                if (request.UserId != null) {
                    sessionRequest.AddQueryString("userId", $"{request.UserId}");
                }
                if (request.Action != null) {
                    sessionRequest.AddQueryString("action", $"{request.Action}");
                }
                if (request.Begin != null) {
                    sessionRequest.AddQueryString("begin", $"{request.Begin}");
                }
                if (request.End != null) {
                    sessionRequest.AddQueryString("end", $"{request.End}");
                }
                if (request.LongTerm != null) {
                    sessionRequest.AddQueryString("longTerm", $"{request.LongTerm}");
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
		public IEnumerator QueryExecuteStampSheetLog(
                Request.QueryExecuteStampSheetLogRequest request,
                UnityAction<AsyncResult<Result.QueryExecuteStampSheetLogResult>> callback
        )
		{
			var task = new QueryExecuteStampSheetLogTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.QueryExecuteStampSheetLogResult>(task.Result, task.Error));
        }

		public IFuture<Result.QueryExecuteStampSheetLogResult> QueryExecuteStampSheetLogFuture(
                Request.QueryExecuteStampSheetLogRequest request
        )
		{
			return new QueryExecuteStampSheetLogTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.QueryExecuteStampSheetLogResult> QueryExecuteStampSheetLogAsync(
                Request.QueryExecuteStampSheetLogRequest request
        )
		{
            AsyncResult<Result.QueryExecuteStampSheetLogResult> result = null;
			await QueryExecuteStampSheetLog(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
    #else
		public QueryExecuteStampSheetLogTask QueryExecuteStampSheetLogAsync(
                Request.QueryExecuteStampSheetLogRequest request
        )
		{
			return new QueryExecuteStampSheetLogTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.QueryExecuteStampSheetLogResult> QueryExecuteStampSheetLogAsync(
                Request.QueryExecuteStampSheetLogRequest request
        )
		{
			var task = new QueryExecuteStampSheetLogTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class CountExecuteStampSheetLogTask : Gs2RestSessionTask<CountExecuteStampSheetLogRequest, CountExecuteStampSheetLogResult>
        {
            public CountExecuteStampSheetLogTask(IGs2Session session, RestSessionRequestFactory factory, CountExecuteStampSheetLogRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(CountExecuteStampSheetLogRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "log")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/log/execute/stamp/sheet/count";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");

                var sessionRequest = Factory.Get(url);
                if (request.ContextStack != null)
                {
                    sessionRequest.AddQueryString("contextStack", request.ContextStack);
                }
                if (request.Service != null) {
                    sessionRequest.AddQueryString("service", $"{request.Service}");
                }
                if (request.Method != null) {
                    sessionRequest.AddQueryString("method", $"{request.Method}");
                }
                if (request.UserId != null) {
                    sessionRequest.AddQueryString("userId", $"{request.UserId}");
                }
                if (request.Action != null) {
                    sessionRequest.AddQueryString("action", $"{request.Action}");
                }
                if (request.Begin != null) {
                    sessionRequest.AddQueryString("begin", $"{request.Begin}");
                }
                if (request.End != null) {
                    sessionRequest.AddQueryString("end", $"{request.End}");
                }
                if (request.LongTerm != null) {
                    sessionRequest.AddQueryString("longTerm", $"{request.LongTerm}");
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
		public IEnumerator CountExecuteStampSheetLog(
                Request.CountExecuteStampSheetLogRequest request,
                UnityAction<AsyncResult<Result.CountExecuteStampSheetLogResult>> callback
        )
		{
			var task = new CountExecuteStampSheetLogTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.CountExecuteStampSheetLogResult>(task.Result, task.Error));
        }

		public IFuture<Result.CountExecuteStampSheetLogResult> CountExecuteStampSheetLogFuture(
                Request.CountExecuteStampSheetLogRequest request
        )
		{
			return new CountExecuteStampSheetLogTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.CountExecuteStampSheetLogResult> CountExecuteStampSheetLogAsync(
                Request.CountExecuteStampSheetLogRequest request
        )
		{
            AsyncResult<Result.CountExecuteStampSheetLogResult> result = null;
			await CountExecuteStampSheetLog(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
    #else
		public CountExecuteStampSheetLogTask CountExecuteStampSheetLogAsync(
                Request.CountExecuteStampSheetLogRequest request
        )
		{
			return new CountExecuteStampSheetLogTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.CountExecuteStampSheetLogResult> CountExecuteStampSheetLogAsync(
                Request.CountExecuteStampSheetLogRequest request
        )
		{
			var task = new CountExecuteStampSheetLogTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class QueryExecuteStampTaskLogTask : Gs2RestSessionTask<QueryExecuteStampTaskLogRequest, QueryExecuteStampTaskLogResult>
        {
            public QueryExecuteStampTaskLogTask(IGs2Session session, RestSessionRequestFactory factory, QueryExecuteStampTaskLogRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(QueryExecuteStampTaskLogRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "log")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/log/execute/stamp/task";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");

                var sessionRequest = Factory.Get(url);
                if (request.ContextStack != null)
                {
                    sessionRequest.AddQueryString("contextStack", request.ContextStack);
                }
                if (request.Service != null) {
                    sessionRequest.AddQueryString("service", $"{request.Service}");
                }
                if (request.Method != null) {
                    sessionRequest.AddQueryString("method", $"{request.Method}");
                }
                if (request.UserId != null) {
                    sessionRequest.AddQueryString("userId", $"{request.UserId}");
                }
                if (request.Action != null) {
                    sessionRequest.AddQueryString("action", $"{request.Action}");
                }
                if (request.Begin != null) {
                    sessionRequest.AddQueryString("begin", $"{request.Begin}");
                }
                if (request.End != null) {
                    sessionRequest.AddQueryString("end", $"{request.End}");
                }
                if (request.LongTerm != null) {
                    sessionRequest.AddQueryString("longTerm", $"{request.LongTerm}");
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
		public IEnumerator QueryExecuteStampTaskLog(
                Request.QueryExecuteStampTaskLogRequest request,
                UnityAction<AsyncResult<Result.QueryExecuteStampTaskLogResult>> callback
        )
		{
			var task = new QueryExecuteStampTaskLogTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.QueryExecuteStampTaskLogResult>(task.Result, task.Error));
        }

		public IFuture<Result.QueryExecuteStampTaskLogResult> QueryExecuteStampTaskLogFuture(
                Request.QueryExecuteStampTaskLogRequest request
        )
		{
			return new QueryExecuteStampTaskLogTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.QueryExecuteStampTaskLogResult> QueryExecuteStampTaskLogAsync(
                Request.QueryExecuteStampTaskLogRequest request
        )
		{
            AsyncResult<Result.QueryExecuteStampTaskLogResult> result = null;
			await QueryExecuteStampTaskLog(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
    #else
		public QueryExecuteStampTaskLogTask QueryExecuteStampTaskLogAsync(
                Request.QueryExecuteStampTaskLogRequest request
        )
		{
			return new QueryExecuteStampTaskLogTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.QueryExecuteStampTaskLogResult> QueryExecuteStampTaskLogAsync(
                Request.QueryExecuteStampTaskLogRequest request
        )
		{
			var task = new QueryExecuteStampTaskLogTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class CountExecuteStampTaskLogTask : Gs2RestSessionTask<CountExecuteStampTaskLogRequest, CountExecuteStampTaskLogResult>
        {
            public CountExecuteStampTaskLogTask(IGs2Session session, RestSessionRequestFactory factory, CountExecuteStampTaskLogRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(CountExecuteStampTaskLogRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "log")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/log/execute/stamp/task/count";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");

                var sessionRequest = Factory.Get(url);
                if (request.ContextStack != null)
                {
                    sessionRequest.AddQueryString("contextStack", request.ContextStack);
                }
                if (request.Service != null) {
                    sessionRequest.AddQueryString("service", $"{request.Service}");
                }
                if (request.Method != null) {
                    sessionRequest.AddQueryString("method", $"{request.Method}");
                }
                if (request.UserId != null) {
                    sessionRequest.AddQueryString("userId", $"{request.UserId}");
                }
                if (request.Action != null) {
                    sessionRequest.AddQueryString("action", $"{request.Action}");
                }
                if (request.Begin != null) {
                    sessionRequest.AddQueryString("begin", $"{request.Begin}");
                }
                if (request.End != null) {
                    sessionRequest.AddQueryString("end", $"{request.End}");
                }
                if (request.LongTerm != null) {
                    sessionRequest.AddQueryString("longTerm", $"{request.LongTerm}");
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
		public IEnumerator CountExecuteStampTaskLog(
                Request.CountExecuteStampTaskLogRequest request,
                UnityAction<AsyncResult<Result.CountExecuteStampTaskLogResult>> callback
        )
		{
			var task = new CountExecuteStampTaskLogTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.CountExecuteStampTaskLogResult>(task.Result, task.Error));
        }

		public IFuture<Result.CountExecuteStampTaskLogResult> CountExecuteStampTaskLogFuture(
                Request.CountExecuteStampTaskLogRequest request
        )
		{
			return new CountExecuteStampTaskLogTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.CountExecuteStampTaskLogResult> CountExecuteStampTaskLogAsync(
                Request.CountExecuteStampTaskLogRequest request
        )
		{
            AsyncResult<Result.CountExecuteStampTaskLogResult> result = null;
			await CountExecuteStampTaskLog(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
    #else
		public CountExecuteStampTaskLogTask CountExecuteStampTaskLogAsync(
                Request.CountExecuteStampTaskLogRequest request
        )
		{
			return new CountExecuteStampTaskLogTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.CountExecuteStampTaskLogResult> CountExecuteStampTaskLogAsync(
                Request.CountExecuteStampTaskLogRequest request
        )
		{
			var task = new CountExecuteStampTaskLogTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class QueryInGameLogTask : Gs2RestSessionTask<QueryInGameLogRequest, QueryInGameLogResult>
        {
            public QueryInGameLogTask(IGs2Session session, RestSessionRequestFactory factory, QueryInGameLogRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(QueryInGameLogRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "log")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/ingame/log";

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
                if (request.Tags != null)
                {
                    jsonWriter.WritePropertyName("tags");
                    jsonWriter.WriteArrayStart();
                    foreach(var item in request.Tags)
                    {
                        item.WriteJson(jsonWriter);
                    }
                    jsonWriter.WriteArrayEnd();
                }
                if (request.Begin != null)
                {
                    jsonWriter.WritePropertyName("begin");
                    jsonWriter.Write(request.Begin.ToString());
                }
                if (request.End != null)
                {
                    jsonWriter.WritePropertyName("end");
                    jsonWriter.Write(request.End.ToString());
                }
                if (request.LongTerm != null)
                {
                    jsonWriter.WritePropertyName("longTerm");
                    jsonWriter.Write(request.LongTerm.ToString());
                }
                if (request.PageToken != null)
                {
                    jsonWriter.WritePropertyName("pageToken");
                    jsonWriter.Write(request.PageToken);
                }
                if (request.Limit != null)
                {
                    jsonWriter.WritePropertyName("limit");
                    jsonWriter.Write(request.Limit.ToString());
                }
                if (request.ContextStack != null)
                {
                    jsonWriter.WritePropertyName("contextStack");
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
		public IEnumerator QueryInGameLog(
                Request.QueryInGameLogRequest request,
                UnityAction<AsyncResult<Result.QueryInGameLogResult>> callback
        )
		{
			var task = new QueryInGameLogTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.QueryInGameLogResult>(task.Result, task.Error));
        }

		public IFuture<Result.QueryInGameLogResult> QueryInGameLogFuture(
                Request.QueryInGameLogRequest request
        )
		{
			return new QueryInGameLogTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.QueryInGameLogResult> QueryInGameLogAsync(
                Request.QueryInGameLogRequest request
        )
		{
            AsyncResult<Result.QueryInGameLogResult> result = null;
			await QueryInGameLog(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
    #else
		public QueryInGameLogTask QueryInGameLogAsync(
                Request.QueryInGameLogRequest request
        )
		{
			return new QueryInGameLogTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.QueryInGameLogResult> QueryInGameLogAsync(
                Request.QueryInGameLogRequest request
        )
		{
			var task = new QueryInGameLogTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class SendInGameLogTask : Gs2RestSessionTask<SendInGameLogRequest, SendInGameLogResult>
        {
            public SendInGameLogTask(IGs2Session session, RestSessionRequestFactory factory, SendInGameLogRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(SendInGameLogRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "log")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/ingame/log/user/me/send";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");

                var sessionRequest = Factory.Post(url);

                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);
                jsonWriter.WriteObjectStart();
                if (request.Tags != null)
                {
                    jsonWriter.WritePropertyName("tags");
                    jsonWriter.WriteArrayStart();
                    foreach(var item in request.Tags)
                    {
                        item.WriteJson(jsonWriter);
                    }
                    jsonWriter.WriteArrayEnd();
                }
                if (request.Payload != null)
                {
                    jsonWriter.WritePropertyName("payload");
                    jsonWriter.Write(request.Payload);
                }
                if (request.ContextStack != null)
                {
                    jsonWriter.WritePropertyName("contextStack");
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
		public IEnumerator SendInGameLog(
                Request.SendInGameLogRequest request,
                UnityAction<AsyncResult<Result.SendInGameLogResult>> callback
        )
		{
			var task = new SendInGameLogTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.SendInGameLogResult>(task.Result, task.Error));
        }

		public IFuture<Result.SendInGameLogResult> SendInGameLogFuture(
                Request.SendInGameLogRequest request
        )
		{
			return new SendInGameLogTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.SendInGameLogResult> SendInGameLogAsync(
                Request.SendInGameLogRequest request
        )
		{
            AsyncResult<Result.SendInGameLogResult> result = null;
			await SendInGameLog(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
    #else
		public SendInGameLogTask SendInGameLogAsync(
                Request.SendInGameLogRequest request
        )
		{
			return new SendInGameLogTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.SendInGameLogResult> SendInGameLogAsync(
                Request.SendInGameLogRequest request
        )
		{
			var task = new SendInGameLogTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class SendInGameLogByUserIdTask : Gs2RestSessionTask<SendInGameLogByUserIdRequest, SendInGameLogByUserIdResult>
        {
            public SendInGameLogByUserIdTask(IGs2Session session, RestSessionRequestFactory factory, SendInGameLogByUserIdRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(SendInGameLogByUserIdRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "log")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/ingame/log/user/{userId}/send";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{userId}", !string.IsNullOrEmpty(request.UserId) ? request.UserId.ToString() : "null");

                var sessionRequest = Factory.Post(url);

                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);
                jsonWriter.WriteObjectStart();
                if (request.Tags != null)
                {
                    jsonWriter.WritePropertyName("tags");
                    jsonWriter.WriteArrayStart();
                    foreach(var item in request.Tags)
                    {
                        item.WriteJson(jsonWriter);
                    }
                    jsonWriter.WriteArrayEnd();
                }
                if (request.Payload != null)
                {
                    jsonWriter.WritePropertyName("payload");
                    jsonWriter.Write(request.Payload);
                }
                if (request.ContextStack != null)
                {
                    jsonWriter.WritePropertyName("contextStack");
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
		public IEnumerator SendInGameLogByUserId(
                Request.SendInGameLogByUserIdRequest request,
                UnityAction<AsyncResult<Result.SendInGameLogByUserIdResult>> callback
        )
		{
			var task = new SendInGameLogByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.SendInGameLogByUserIdResult>(task.Result, task.Error));
        }

		public IFuture<Result.SendInGameLogByUserIdResult> SendInGameLogByUserIdFuture(
                Request.SendInGameLogByUserIdRequest request
        )
		{
			return new SendInGameLogByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.SendInGameLogByUserIdResult> SendInGameLogByUserIdAsync(
                Request.SendInGameLogByUserIdRequest request
        )
		{
            AsyncResult<Result.SendInGameLogByUserIdResult> result = null;
			await SendInGameLogByUserId(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
    #else
		public SendInGameLogByUserIdTask SendInGameLogByUserIdAsync(
                Request.SendInGameLogByUserIdRequest request
        )
		{
			return new SendInGameLogByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.SendInGameLogByUserIdResult> SendInGameLogByUserIdAsync(
                Request.SendInGameLogByUserIdRequest request
        )
		{
			var task = new SendInGameLogByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class QueryAccessLogWithTelemetryTask : Gs2RestSessionTask<QueryAccessLogWithTelemetryRequest, QueryAccessLogWithTelemetryResult>
        {
            public QueryAccessLogWithTelemetryTask(IGs2Session session, RestSessionRequestFactory factory, QueryAccessLogWithTelemetryRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(QueryAccessLogWithTelemetryRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "log")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/log/access/telemetry";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");

                var sessionRequest = Factory.Get(url);
                if (request.ContextStack != null)
                {
                    sessionRequest.AddQueryString("contextStack", request.ContextStack);
                }
                if (request.UserId != null) {
                    sessionRequest.AddQueryString("userId", $"{request.UserId}");
                }
                if (request.Begin != null) {
                    sessionRequest.AddQueryString("begin", $"{request.Begin}");
                }
                if (request.End != null) {
                    sessionRequest.AddQueryString("end", $"{request.End}");
                }
                if (request.LongTerm != null) {
                    sessionRequest.AddQueryString("longTerm", $"{request.LongTerm}");
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
		public IEnumerator QueryAccessLogWithTelemetry(
                Request.QueryAccessLogWithTelemetryRequest request,
                UnityAction<AsyncResult<Result.QueryAccessLogWithTelemetryResult>> callback
        )
		{
			var task = new QueryAccessLogWithTelemetryTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.QueryAccessLogWithTelemetryResult>(task.Result, task.Error));
        }

		public IFuture<Result.QueryAccessLogWithTelemetryResult> QueryAccessLogWithTelemetryFuture(
                Request.QueryAccessLogWithTelemetryRequest request
        )
		{
			return new QueryAccessLogWithTelemetryTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.QueryAccessLogWithTelemetryResult> QueryAccessLogWithTelemetryAsync(
                Request.QueryAccessLogWithTelemetryRequest request
        )
		{
            AsyncResult<Result.QueryAccessLogWithTelemetryResult> result = null;
			await QueryAccessLogWithTelemetry(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
    #else
		public QueryAccessLogWithTelemetryTask QueryAccessLogWithTelemetryAsync(
                Request.QueryAccessLogWithTelemetryRequest request
        )
		{
			return new QueryAccessLogWithTelemetryTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.QueryAccessLogWithTelemetryResult> QueryAccessLogWithTelemetryAsync(
                Request.QueryAccessLogWithTelemetryRequest request
        )
		{
			var task = new QueryAccessLogWithTelemetryTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class DescribeInsightsTask : Gs2RestSessionTask<DescribeInsightsRequest, DescribeInsightsResult>
        {
            public DescribeInsightsTask(IGs2Session session, RestSessionRequestFactory factory, DescribeInsightsRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DescribeInsightsRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "log")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/insight";

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
		public IEnumerator DescribeInsights(
                Request.DescribeInsightsRequest request,
                UnityAction<AsyncResult<Result.DescribeInsightsResult>> callback
        )
		{
			var task = new DescribeInsightsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.DescribeInsightsResult>(task.Result, task.Error));
        }

		public IFuture<Result.DescribeInsightsResult> DescribeInsightsFuture(
                Request.DescribeInsightsRequest request
        )
		{
			return new DescribeInsightsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.DescribeInsightsResult> DescribeInsightsAsync(
                Request.DescribeInsightsRequest request
        )
		{
            AsyncResult<Result.DescribeInsightsResult> result = null;
			await DescribeInsights(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
    #else
		public DescribeInsightsTask DescribeInsightsAsync(
                Request.DescribeInsightsRequest request
        )
		{
			return new DescribeInsightsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.DescribeInsightsResult> DescribeInsightsAsync(
                Request.DescribeInsightsRequest request
        )
		{
			var task = new DescribeInsightsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class CreateInsightTask : Gs2RestSessionTask<CreateInsightRequest, CreateInsightResult>
        {
            public CreateInsightTask(IGs2Session session, RestSessionRequestFactory factory, CreateInsightRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(CreateInsightRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "log")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/insight";

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
		public IEnumerator CreateInsight(
                Request.CreateInsightRequest request,
                UnityAction<AsyncResult<Result.CreateInsightResult>> callback
        )
		{
			var task = new CreateInsightTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.CreateInsightResult>(task.Result, task.Error));
        }

		public IFuture<Result.CreateInsightResult> CreateInsightFuture(
                Request.CreateInsightRequest request
        )
		{
			return new CreateInsightTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.CreateInsightResult> CreateInsightAsync(
                Request.CreateInsightRequest request
        )
		{
            AsyncResult<Result.CreateInsightResult> result = null;
			await CreateInsight(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
    #else
		public CreateInsightTask CreateInsightAsync(
                Request.CreateInsightRequest request
        )
		{
			return new CreateInsightTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.CreateInsightResult> CreateInsightAsync(
                Request.CreateInsightRequest request
        )
		{
			var task = new CreateInsightTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class GetInsightTask : Gs2RestSessionTask<GetInsightRequest, GetInsightResult>
        {
            public GetInsightTask(IGs2Session session, RestSessionRequestFactory factory, GetInsightRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(GetInsightRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "log")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/insight/{insightName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{insightName}", !string.IsNullOrEmpty(request.InsightName) ? request.InsightName.ToString() : "null");

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
		public IEnumerator GetInsight(
                Request.GetInsightRequest request,
                UnityAction<AsyncResult<Result.GetInsightResult>> callback
        )
		{
			var task = new GetInsightTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.GetInsightResult>(task.Result, task.Error));
        }

		public IFuture<Result.GetInsightResult> GetInsightFuture(
                Request.GetInsightRequest request
        )
		{
			return new GetInsightTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.GetInsightResult> GetInsightAsync(
                Request.GetInsightRequest request
        )
		{
            AsyncResult<Result.GetInsightResult> result = null;
			await GetInsight(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
    #else
		public GetInsightTask GetInsightAsync(
                Request.GetInsightRequest request
        )
		{
			return new GetInsightTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.GetInsightResult> GetInsightAsync(
                Request.GetInsightRequest request
        )
		{
			var task = new GetInsightTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class DeleteInsightTask : Gs2RestSessionTask<DeleteInsightRequest, DeleteInsightResult>
        {
            public DeleteInsightTask(IGs2Session session, RestSessionRequestFactory factory, DeleteInsightRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DeleteInsightRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "log")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/insight/{insightName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{insightName}", !string.IsNullOrEmpty(request.InsightName) ? request.InsightName.ToString() : "null");

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
		public IEnumerator DeleteInsight(
                Request.DeleteInsightRequest request,
                UnityAction<AsyncResult<Result.DeleteInsightResult>> callback
        )
		{
			var task = new DeleteInsightTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.DeleteInsightResult>(task.Result, task.Error));
        }

		public IFuture<Result.DeleteInsightResult> DeleteInsightFuture(
                Request.DeleteInsightRequest request
        )
		{
			return new DeleteInsightTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.DeleteInsightResult> DeleteInsightAsync(
                Request.DeleteInsightRequest request
        )
		{
            AsyncResult<Result.DeleteInsightResult> result = null;
			await DeleteInsight(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
    #else
		public DeleteInsightTask DeleteInsightAsync(
                Request.DeleteInsightRequest request
        )
		{
			return new DeleteInsightTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.DeleteInsightResult> DeleteInsightAsync(
                Request.DeleteInsightRequest request
        )
		{
			var task = new DeleteInsightTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class DescribeFacetModelsTask : Gs2RestSessionTask<DescribeFacetModelsRequest, DescribeFacetModelsResult>
        {
            public DescribeFacetModelsTask(IGs2Session session, RestSessionRequestFactory factory, DescribeFacetModelsRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DescribeFacetModelsRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "log")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/model/facet";

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
		public IEnumerator DescribeFacetModels(
                Request.DescribeFacetModelsRequest request,
                UnityAction<AsyncResult<Result.DescribeFacetModelsResult>> callback
        )
		{
			var task = new DescribeFacetModelsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.DescribeFacetModelsResult>(task.Result, task.Error));
        }

		public IFuture<Result.DescribeFacetModelsResult> DescribeFacetModelsFuture(
                Request.DescribeFacetModelsRequest request
        )
		{
			return new DescribeFacetModelsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.DescribeFacetModelsResult> DescribeFacetModelsAsync(
                Request.DescribeFacetModelsRequest request
        )
		{
            AsyncResult<Result.DescribeFacetModelsResult> result = null;
			await DescribeFacetModels(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
    #else
		public DescribeFacetModelsTask DescribeFacetModelsAsync(
                Request.DescribeFacetModelsRequest request
        )
		{
			return new DescribeFacetModelsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.DescribeFacetModelsResult> DescribeFacetModelsAsync(
                Request.DescribeFacetModelsRequest request
        )
		{
			var task = new DescribeFacetModelsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class CreateFacetModelTask : Gs2RestSessionTask<CreateFacetModelRequest, CreateFacetModelResult>
        {
            public CreateFacetModelTask(IGs2Session session, RestSessionRequestFactory factory, CreateFacetModelRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(CreateFacetModelRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "log")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/model/facet";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");

                var sessionRequest = Factory.Post(url);

                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);
                jsonWriter.WriteObjectStart();
                if (request.Field != null)
                {
                    jsonWriter.WritePropertyName("field");
                    jsonWriter.Write(request.Field);
                }
                if (request.Type != null)
                {
                    jsonWriter.WritePropertyName("type");
                    jsonWriter.Write(request.Type);
                }
                if (request.DisplayName != null)
                {
                    jsonWriter.WritePropertyName("displayName");
                    jsonWriter.Write(request.DisplayName);
                }
                if (request.Order != null)
                {
                    jsonWriter.WritePropertyName("order");
                    jsonWriter.Write(request.Order.ToString());
                }
                if (request.ContextStack != null)
                {
                    jsonWriter.WritePropertyName("contextStack");
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
		public IEnumerator CreateFacetModel(
                Request.CreateFacetModelRequest request,
                UnityAction<AsyncResult<Result.CreateFacetModelResult>> callback
        )
		{
			var task = new CreateFacetModelTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.CreateFacetModelResult>(task.Result, task.Error));
        }

		public IFuture<Result.CreateFacetModelResult> CreateFacetModelFuture(
                Request.CreateFacetModelRequest request
        )
		{
			return new CreateFacetModelTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.CreateFacetModelResult> CreateFacetModelAsync(
                Request.CreateFacetModelRequest request
        )
		{
            AsyncResult<Result.CreateFacetModelResult> result = null;
			await CreateFacetModel(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
    #else
		public CreateFacetModelTask CreateFacetModelAsync(
                Request.CreateFacetModelRequest request
        )
		{
			return new CreateFacetModelTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.CreateFacetModelResult> CreateFacetModelAsync(
                Request.CreateFacetModelRequest request
        )
		{
			var task = new CreateFacetModelTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class GetFacetModelTask : Gs2RestSessionTask<GetFacetModelRequest, GetFacetModelResult>
        {
            public GetFacetModelTask(IGs2Session session, RestSessionRequestFactory factory, GetFacetModelRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(GetFacetModelRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "log")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/model/facet/{field}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{field}", !string.IsNullOrEmpty(request.Field) ? request.Field.ToString() : "null");

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
		public IEnumerator GetFacetModel(
                Request.GetFacetModelRequest request,
                UnityAction<AsyncResult<Result.GetFacetModelResult>> callback
        )
		{
			var task = new GetFacetModelTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.GetFacetModelResult>(task.Result, task.Error));
        }

		public IFuture<Result.GetFacetModelResult> GetFacetModelFuture(
                Request.GetFacetModelRequest request
        )
		{
			return new GetFacetModelTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.GetFacetModelResult> GetFacetModelAsync(
                Request.GetFacetModelRequest request
        )
		{
            AsyncResult<Result.GetFacetModelResult> result = null;
			await GetFacetModel(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
    #else
		public GetFacetModelTask GetFacetModelAsync(
                Request.GetFacetModelRequest request
        )
		{
			return new GetFacetModelTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.GetFacetModelResult> GetFacetModelAsync(
                Request.GetFacetModelRequest request
        )
		{
			var task = new GetFacetModelTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class UpdateFacetModelTask : Gs2RestSessionTask<UpdateFacetModelRequest, UpdateFacetModelResult>
        {
            public UpdateFacetModelTask(IGs2Session session, RestSessionRequestFactory factory, UpdateFacetModelRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(UpdateFacetModelRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "log")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/model/facet/{field}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{field}", !string.IsNullOrEmpty(request.Field) ? request.Field.ToString() : "null");

                var sessionRequest = Factory.Put(url);

                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);
                jsonWriter.WriteObjectStart();
                if (request.Type != null)
                {
                    jsonWriter.WritePropertyName("type");
                    jsonWriter.Write(request.Type);
                }
                if (request.DisplayName != null)
                {
                    jsonWriter.WritePropertyName("displayName");
                    jsonWriter.Write(request.DisplayName);
                }
                if (request.Order != null)
                {
                    jsonWriter.WritePropertyName("order");
                    jsonWriter.Write(request.Order.ToString());
                }
                if (request.ContextStack != null)
                {
                    jsonWriter.WritePropertyName("contextStack");
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
		public IEnumerator UpdateFacetModel(
                Request.UpdateFacetModelRequest request,
                UnityAction<AsyncResult<Result.UpdateFacetModelResult>> callback
        )
		{
			var task = new UpdateFacetModelTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.UpdateFacetModelResult>(task.Result, task.Error));
        }

		public IFuture<Result.UpdateFacetModelResult> UpdateFacetModelFuture(
                Request.UpdateFacetModelRequest request
        )
		{
			return new UpdateFacetModelTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.UpdateFacetModelResult> UpdateFacetModelAsync(
                Request.UpdateFacetModelRequest request
        )
		{
            AsyncResult<Result.UpdateFacetModelResult> result = null;
			await UpdateFacetModel(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
    #else
		public UpdateFacetModelTask UpdateFacetModelAsync(
                Request.UpdateFacetModelRequest request
        )
		{
			return new UpdateFacetModelTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.UpdateFacetModelResult> UpdateFacetModelAsync(
                Request.UpdateFacetModelRequest request
        )
		{
			var task = new UpdateFacetModelTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class DeleteFacetModelTask : Gs2RestSessionTask<DeleteFacetModelRequest, DeleteFacetModelResult>
        {
            public DeleteFacetModelTask(IGs2Session session, RestSessionRequestFactory factory, DeleteFacetModelRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DeleteFacetModelRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "log")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/model/facet/{field}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{field}", !string.IsNullOrEmpty(request.Field) ? request.Field.ToString() : "null");

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
		public IEnumerator DeleteFacetModel(
                Request.DeleteFacetModelRequest request,
                UnityAction<AsyncResult<Result.DeleteFacetModelResult>> callback
        )
		{
			var task = new DeleteFacetModelTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.DeleteFacetModelResult>(task.Result, task.Error));
        }

		public IFuture<Result.DeleteFacetModelResult> DeleteFacetModelFuture(
                Request.DeleteFacetModelRequest request
        )
		{
			return new DeleteFacetModelTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.DeleteFacetModelResult> DeleteFacetModelAsync(
                Request.DeleteFacetModelRequest request
        )
		{
            AsyncResult<Result.DeleteFacetModelResult> result = null;
			await DeleteFacetModel(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
    #else
		public DeleteFacetModelTask DeleteFacetModelAsync(
                Request.DeleteFacetModelRequest request
        )
		{
			return new DeleteFacetModelTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.DeleteFacetModelResult> DeleteFacetModelAsync(
                Request.DeleteFacetModelRequest request
        )
		{
			var task = new DeleteFacetModelTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class DescribeDashboardsTask : Gs2RestSessionTask<DescribeDashboardsRequest, DescribeDashboardsResult>
        {
            public DescribeDashboardsTask(IGs2Session session, RestSessionRequestFactory factory, DescribeDashboardsRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DescribeDashboardsRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "log")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/dashboard";

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
		public IEnumerator DescribeDashboards(
                Request.DescribeDashboardsRequest request,
                UnityAction<AsyncResult<Result.DescribeDashboardsResult>> callback
        )
		{
			var task = new DescribeDashboardsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.DescribeDashboardsResult>(task.Result, task.Error));
        }

		public IFuture<Result.DescribeDashboardsResult> DescribeDashboardsFuture(
                Request.DescribeDashboardsRequest request
        )
		{
			return new DescribeDashboardsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.DescribeDashboardsResult> DescribeDashboardsAsync(
                Request.DescribeDashboardsRequest request
        )
		{
            AsyncResult<Result.DescribeDashboardsResult> result = null;
			await DescribeDashboards(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
    #else
		public DescribeDashboardsTask DescribeDashboardsAsync(
                Request.DescribeDashboardsRequest request
        )
		{
			return new DescribeDashboardsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.DescribeDashboardsResult> DescribeDashboardsAsync(
                Request.DescribeDashboardsRequest request
        )
		{
			var task = new DescribeDashboardsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class CreateDashboardTask : Gs2RestSessionTask<CreateDashboardRequest, CreateDashboardResult>
        {
            public CreateDashboardTask(IGs2Session session, RestSessionRequestFactory factory, CreateDashboardRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(CreateDashboardRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "log")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/dashboard";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");

                var sessionRequest = Factory.Post(url);

                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);
                jsonWriter.WriteObjectStart();
                if (request.DisplayName != null)
                {
                    jsonWriter.WritePropertyName("displayName");
                    jsonWriter.Write(request.DisplayName);
                }
                if (request.Description != null)
                {
                    jsonWriter.WritePropertyName("description");
                    jsonWriter.Write(request.Description);
                }
                if (request.ContextStack != null)
                {
                    jsonWriter.WritePropertyName("contextStack");
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
		public IEnumerator CreateDashboard(
                Request.CreateDashboardRequest request,
                UnityAction<AsyncResult<Result.CreateDashboardResult>> callback
        )
		{
			var task = new CreateDashboardTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.CreateDashboardResult>(task.Result, task.Error));
        }

		public IFuture<Result.CreateDashboardResult> CreateDashboardFuture(
                Request.CreateDashboardRequest request
        )
		{
			return new CreateDashboardTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.CreateDashboardResult> CreateDashboardAsync(
                Request.CreateDashboardRequest request
        )
		{
            AsyncResult<Result.CreateDashboardResult> result = null;
			await CreateDashboard(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
    #else
		public CreateDashboardTask CreateDashboardAsync(
                Request.CreateDashboardRequest request
        )
		{
			return new CreateDashboardTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.CreateDashboardResult> CreateDashboardAsync(
                Request.CreateDashboardRequest request
        )
		{
			var task = new CreateDashboardTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class GetDashboardTask : Gs2RestSessionTask<GetDashboardRequest, GetDashboardResult>
        {
            public GetDashboardTask(IGs2Session session, RestSessionRequestFactory factory, GetDashboardRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(GetDashboardRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "log")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/dashboard/{dashboardName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{dashboardName}", !string.IsNullOrEmpty(request.DashboardName) ? request.DashboardName.ToString() : "null");

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
		public IEnumerator GetDashboard(
                Request.GetDashboardRequest request,
                UnityAction<AsyncResult<Result.GetDashboardResult>> callback
        )
		{
			var task = new GetDashboardTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.GetDashboardResult>(task.Result, task.Error));
        }

		public IFuture<Result.GetDashboardResult> GetDashboardFuture(
                Request.GetDashboardRequest request
        )
		{
			return new GetDashboardTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.GetDashboardResult> GetDashboardAsync(
                Request.GetDashboardRequest request
        )
		{
            AsyncResult<Result.GetDashboardResult> result = null;
			await GetDashboard(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
    #else
		public GetDashboardTask GetDashboardAsync(
                Request.GetDashboardRequest request
        )
		{
			return new GetDashboardTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.GetDashboardResult> GetDashboardAsync(
                Request.GetDashboardRequest request
        )
		{
			var task = new GetDashboardTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class UpdateDashboardTask : Gs2RestSessionTask<UpdateDashboardRequest, UpdateDashboardResult>
        {
            public UpdateDashboardTask(IGs2Session session, RestSessionRequestFactory factory, UpdateDashboardRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(UpdateDashboardRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "log")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/dashboard/{dashboardName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{dashboardName}", !string.IsNullOrEmpty(request.DashboardName) ? request.DashboardName.ToString() : "null");

                var sessionRequest = Factory.Put(url);

                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);
                jsonWriter.WriteObjectStart();
                if (request.DisplayName != null)
                {
                    jsonWriter.WritePropertyName("displayName");
                    jsonWriter.Write(request.DisplayName);
                }
                if (request.Description != null)
                {
                    jsonWriter.WritePropertyName("description");
                    jsonWriter.Write(request.Description);
                }
                if (request.Payload != null)
                {
                    jsonWriter.WritePropertyName("payload");
                    jsonWriter.Write(request.Payload);
                }
                if (request.ContextStack != null)
                {
                    jsonWriter.WritePropertyName("contextStack");
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
		public IEnumerator UpdateDashboard(
                Request.UpdateDashboardRequest request,
                UnityAction<AsyncResult<Result.UpdateDashboardResult>> callback
        )
		{
			var task = new UpdateDashboardTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.UpdateDashboardResult>(task.Result, task.Error));
        }

		public IFuture<Result.UpdateDashboardResult> UpdateDashboardFuture(
                Request.UpdateDashboardRequest request
        )
		{
			return new UpdateDashboardTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.UpdateDashboardResult> UpdateDashboardAsync(
                Request.UpdateDashboardRequest request
        )
		{
            AsyncResult<Result.UpdateDashboardResult> result = null;
			await UpdateDashboard(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
    #else
		public UpdateDashboardTask UpdateDashboardAsync(
                Request.UpdateDashboardRequest request
        )
		{
			return new UpdateDashboardTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.UpdateDashboardResult> UpdateDashboardAsync(
                Request.UpdateDashboardRequest request
        )
		{
			var task = new UpdateDashboardTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class DuplicateDashboardTask : Gs2RestSessionTask<DuplicateDashboardRequest, DuplicateDashboardResult>
        {
            public DuplicateDashboardTask(IGs2Session session, RestSessionRequestFactory factory, DuplicateDashboardRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DuplicateDashboardRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "log")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/dashboard/{dashboardName}/copy";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{dashboardName}", !string.IsNullOrEmpty(request.DashboardName) ? request.DashboardName.ToString() : "null");

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
		public IEnumerator DuplicateDashboard(
                Request.DuplicateDashboardRequest request,
                UnityAction<AsyncResult<Result.DuplicateDashboardResult>> callback
        )
		{
			var task = new DuplicateDashboardTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.DuplicateDashboardResult>(task.Result, task.Error));
        }

		public IFuture<Result.DuplicateDashboardResult> DuplicateDashboardFuture(
                Request.DuplicateDashboardRequest request
        )
		{
			return new DuplicateDashboardTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.DuplicateDashboardResult> DuplicateDashboardAsync(
                Request.DuplicateDashboardRequest request
        )
		{
            AsyncResult<Result.DuplicateDashboardResult> result = null;
			await DuplicateDashboard(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
    #else
		public DuplicateDashboardTask DuplicateDashboardAsync(
                Request.DuplicateDashboardRequest request
        )
		{
			return new DuplicateDashboardTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.DuplicateDashboardResult> DuplicateDashboardAsync(
                Request.DuplicateDashboardRequest request
        )
		{
			var task = new DuplicateDashboardTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class DeleteDashboardTask : Gs2RestSessionTask<DeleteDashboardRequest, DeleteDashboardResult>
        {
            public DeleteDashboardTask(IGs2Session session, RestSessionRequestFactory factory, DeleteDashboardRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DeleteDashboardRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "log")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/dashboard/{dashboardName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{dashboardName}", !string.IsNullOrEmpty(request.DashboardName) ? request.DashboardName.ToString() : "null");

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
		public IEnumerator DeleteDashboard(
                Request.DeleteDashboardRequest request,
                UnityAction<AsyncResult<Result.DeleteDashboardResult>> callback
        )
		{
			var task = new DeleteDashboardTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.DeleteDashboardResult>(task.Result, task.Error));
        }

		public IFuture<Result.DeleteDashboardResult> DeleteDashboardFuture(
                Request.DeleteDashboardRequest request
        )
		{
			return new DeleteDashboardTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.DeleteDashboardResult> DeleteDashboardAsync(
                Request.DeleteDashboardRequest request
        )
		{
            AsyncResult<Result.DeleteDashboardResult> result = null;
			await DeleteDashboard(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
    #else
		public DeleteDashboardTask DeleteDashboardAsync(
                Request.DeleteDashboardRequest request
        )
		{
			return new DeleteDashboardTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.DeleteDashboardResult> DeleteDashboardAsync(
                Request.DeleteDashboardRequest request
        )
		{
			var task = new DeleteDashboardTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class QueryLogTask : Gs2RestSessionTask<QueryLogRequest, QueryLogResult>
        {
            public QueryLogTask(IGs2Session session, RestSessionRequestFactory factory, QueryLogRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(QueryLogRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "log")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/log/v2/query";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");

                var sessionRequest = Factory.Post(url);

                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);
                jsonWriter.WriteObjectStart();
                if (request.Begin != null)
                {
                    jsonWriter.WritePropertyName("begin");
                    jsonWriter.Write(request.Begin.ToString());
                }
                if (request.End != null)
                {
                    jsonWriter.WritePropertyName("end");
                    jsonWriter.Write(request.End.ToString());
                }
                if (request.Query != null)
                {
                    jsonWriter.WritePropertyName("query");
                    jsonWriter.Write(request.Query);
                }
                if (request.PageToken != null)
                {
                    jsonWriter.WritePropertyName("pageToken");
                    jsonWriter.Write(request.PageToken);
                }
                if (request.Limit != null)
                {
                    jsonWriter.WritePropertyName("limit");
                    jsonWriter.Write(request.Limit.ToString());
                }
                if (request.ContextStack != null)
                {
                    jsonWriter.WritePropertyName("contextStack");
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
		public IEnumerator QueryLog(
                Request.QueryLogRequest request,
                UnityAction<AsyncResult<Result.QueryLogResult>> callback
        )
		{
			var task = new QueryLogTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.QueryLogResult>(task.Result, task.Error));
        }

		public IFuture<Result.QueryLogResult> QueryLogFuture(
                Request.QueryLogRequest request
        )
		{
			return new QueryLogTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.QueryLogResult> QueryLogAsync(
                Request.QueryLogRequest request
        )
		{
            AsyncResult<Result.QueryLogResult> result = null;
			await QueryLog(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
    #else
		public QueryLogTask QueryLogAsync(
                Request.QueryLogRequest request
        )
		{
			return new QueryLogTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.QueryLogResult> QueryLogAsync(
                Request.QueryLogRequest request
        )
		{
			var task = new QueryLogTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class GetLogTask : Gs2RestSessionTask<GetLogRequest, GetLogResult>
        {
            public GetLogTask(IGs2Session session, RestSessionRequestFactory factory, GetLogRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(GetLogRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "log")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/log/v2/query/{logRequestId}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{logRequestId}", !string.IsNullOrEmpty(request.LogRequestId) ? request.LogRequestId.ToString() : "null");

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
                if (request.DryRun)
                {
                    sessionRequest.AddHeader("X-GS2-DRY-RUN", "true");
                }

                AddHeader(
                    Session.Credential,
                    sessionRequest
                );

                return sessionRequest;
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator GetLog(
                Request.GetLogRequest request,
                UnityAction<AsyncResult<Result.GetLogResult>> callback
        )
		{
			var task = new GetLogTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.GetLogResult>(task.Result, task.Error));
        }

		public IFuture<Result.GetLogResult> GetLogFuture(
                Request.GetLogRequest request
        )
		{
			return new GetLogTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.GetLogResult> GetLogAsync(
                Request.GetLogRequest request
        )
		{
            AsyncResult<Result.GetLogResult> result = null;
			await GetLog(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
    #else
		public GetLogTask GetLogAsync(
                Request.GetLogRequest request
        )
		{
			return new GetLogTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.GetLogResult> GetLogAsync(
                Request.GetLogRequest request
        )
		{
			var task = new GetLogTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class QueryFacetsTask : Gs2RestSessionTask<QueryFacetsRequest, QueryFacetsResult>
        {
            public QueryFacetsTask(IGs2Session session, RestSessionRequestFactory factory, QueryFacetsRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(QueryFacetsRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "log")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/log/v2/query/facet";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");

                var sessionRequest = Factory.Post(url);

                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);
                jsonWriter.WriteObjectStart();
                if (request.Begin != null)
                {
                    jsonWriter.WritePropertyName("begin");
                    jsonWriter.Write(request.Begin.ToString());
                }
                if (request.End != null)
                {
                    jsonWriter.WritePropertyName("end");
                    jsonWriter.Write(request.End.ToString());
                }
                if (request.Query != null)
                {
                    jsonWriter.WritePropertyName("query");
                    jsonWriter.Write(request.Query);
                }
                if (request.ContextStack != null)
                {
                    jsonWriter.WritePropertyName("contextStack");
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
		public IEnumerator QueryFacets(
                Request.QueryFacetsRequest request,
                UnityAction<AsyncResult<Result.QueryFacetsResult>> callback
        )
		{
			var task = new QueryFacetsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.QueryFacetsResult>(task.Result, task.Error));
        }

		public IFuture<Result.QueryFacetsResult> QueryFacetsFuture(
                Request.QueryFacetsRequest request
        )
		{
			return new QueryFacetsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.QueryFacetsResult> QueryFacetsAsync(
                Request.QueryFacetsRequest request
        )
		{
            AsyncResult<Result.QueryFacetsResult> result = null;
			await QueryFacets(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
    #else
		public QueryFacetsTask QueryFacetsAsync(
                Request.QueryFacetsRequest request
        )
		{
			return new QueryFacetsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.QueryFacetsResult> QueryFacetsAsync(
                Request.QueryFacetsRequest request
        )
		{
			var task = new QueryFacetsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class QueryTimeseriesTask : Gs2RestSessionTask<QueryTimeseriesRequest, QueryTimeseriesResult>
        {
            public QueryTimeseriesTask(IGs2Session session, RestSessionRequestFactory factory, QueryTimeseriesRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(QueryTimeseriesRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "log")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/log/v2/timeseries";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");

                var sessionRequest = Factory.Post(url);

                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);
                jsonWriter.WriteObjectStart();
                if (request.Begin != null)
                {
                    jsonWriter.WritePropertyName("begin");
                    jsonWriter.Write(request.Begin.ToString());
                }
                if (request.End != null)
                {
                    jsonWriter.WritePropertyName("end");
                    jsonWriter.Write(request.End.ToString());
                }
                if (request.Query != null)
                {
                    jsonWriter.WritePropertyName("query");
                    jsonWriter.Write(request.Query);
                }
                if (request.GroupBy != null)
                {
                    jsonWriter.WritePropertyName("groupBy");
                    jsonWriter.WriteArrayStart();
                    foreach(var item in request.GroupBy)
                    {
                        jsonWriter.Write(item);
                    }
                    jsonWriter.WriteArrayEnd();
                }
                if (request.Aggregation != null)
                {
                    jsonWriter.WritePropertyName("aggregation");
                    request.Aggregation.WriteJson(jsonWriter);
                }
                if (request.Interval != null)
                {
                    jsonWriter.WritePropertyName("interval");
                    jsonWriter.Write(request.Interval.ToString());
                }
                if (request.SeriesLimit != null)
                {
                    jsonWriter.WritePropertyName("seriesLimit");
                    jsonWriter.Write(request.SeriesLimit.ToString());
                }
                if (request.PageToken != null)
                {
                    jsonWriter.WritePropertyName("pageToken");
                    jsonWriter.Write(request.PageToken);
                }
                if (request.Limit != null)
                {
                    jsonWriter.WritePropertyName("limit");
                    jsonWriter.Write(request.Limit.ToString());
                }
                if (request.ContextStack != null)
                {
                    jsonWriter.WritePropertyName("contextStack");
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
		public IEnumerator QueryTimeseries(
                Request.QueryTimeseriesRequest request,
                UnityAction<AsyncResult<Result.QueryTimeseriesResult>> callback
        )
		{
			var task = new QueryTimeseriesTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.QueryTimeseriesResult>(task.Result, task.Error));
        }

		public IFuture<Result.QueryTimeseriesResult> QueryTimeseriesFuture(
                Request.QueryTimeseriesRequest request
        )
		{
			return new QueryTimeseriesTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.QueryTimeseriesResult> QueryTimeseriesAsync(
                Request.QueryTimeseriesRequest request
        )
		{
            AsyncResult<Result.QueryTimeseriesResult> result = null;
			await QueryTimeseries(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
    #else
		public QueryTimeseriesTask QueryTimeseriesAsync(
                Request.QueryTimeseriesRequest request
        )
		{
			return new QueryTimeseriesTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.QueryTimeseriesResult> QueryTimeseriesAsync(
                Request.QueryTimeseriesRequest request
        )
		{
			var task = new QueryTimeseriesTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class GetTraceTask : Gs2RestSessionTask<GetTraceRequest, GetTraceResult>
        {
            public GetTraceTask(IGs2Session session, RestSessionRequestFactory factory, GetTraceRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(GetTraceRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "log")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/log/v2/trace/{traceId}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{traceId}", !string.IsNullOrEmpty(request.TraceId) ? request.TraceId.ToString() : "null");

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
                if (request.DryRun)
                {
                    sessionRequest.AddHeader("X-GS2-DRY-RUN", "true");
                }

                AddHeader(
                    Session.Credential,
                    sessionRequest
                );

                return sessionRequest;
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator GetTrace(
                Request.GetTraceRequest request,
                UnityAction<AsyncResult<Result.GetTraceResult>> callback
        )
		{
			var task = new GetTraceTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.GetTraceResult>(task.Result, task.Error));
        }

		public IFuture<Result.GetTraceResult> GetTraceFuture(
                Request.GetTraceRequest request
        )
		{
			return new GetTraceTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.GetTraceResult> GetTraceAsync(
                Request.GetTraceRequest request
        )
		{
            AsyncResult<Result.GetTraceResult> result = null;
			await GetTrace(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
    #else
		public GetTraceTask GetTraceAsync(
                Request.GetTraceRequest request
        )
		{
			return new GetTraceTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.GetTraceResult> GetTraceAsync(
                Request.GetTraceRequest request
        )
		{
			var task = new GetTraceTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class QueryMetricsTimeseriesTask : Gs2RestSessionTask<QueryMetricsTimeseriesRequest, QueryMetricsTimeseriesResult>
        {
            public QueryMetricsTimeseriesTask(IGs2Session session, RestSessionRequestFactory factory, QueryMetricsTimeseriesRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(QueryMetricsTimeseriesRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "log")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/metrics/timeseries";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");

                var sessionRequest = Factory.Post(url);

                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);
                jsonWriter.WriteObjectStart();
                if (request.Begin != null)
                {
                    jsonWriter.WritePropertyName("begin");
                    jsonWriter.Write(request.Begin.ToString());
                }
                if (request.End != null)
                {
                    jsonWriter.WritePropertyName("end");
                    jsonWriter.Write(request.End.ToString());
                }
                if (request.Query != null)
                {
                    jsonWriter.WritePropertyName("query");
                    jsonWriter.Write(request.Query);
                }
                if (request.GroupBy != null)
                {
                    jsonWriter.WritePropertyName("groupBy");
                    jsonWriter.WriteArrayStart();
                    foreach(var item in request.GroupBy)
                    {
                        jsonWriter.Write(item);
                    }
                    jsonWriter.WriteArrayEnd();
                }
                if (request.Aggregations != null)
                {
                    jsonWriter.WritePropertyName("aggregations");
                    jsonWriter.WriteArrayStart();
                    foreach(var item in request.Aggregations)
                    {
                        item.WriteJson(jsonWriter);
                    }
                    jsonWriter.WriteArrayEnd();
                }
                if (request.Interval != null)
                {
                    jsonWriter.WritePropertyName("interval");
                    jsonWriter.Write(request.Interval.ToString());
                }
                if (request.SeriesLimit != null)
                {
                    jsonWriter.WritePropertyName("seriesLimit");
                    jsonWriter.Write(request.SeriesLimit.ToString());
                }
                if (request.OrderKey != null)
                {
                    jsonWriter.WritePropertyName("orderKey");
                    jsonWriter.Write(request.OrderKey);
                }
                if (request.OrderBy != null)
                {
                    jsonWriter.WritePropertyName("orderBy");
                    jsonWriter.Write(request.OrderBy);
                }
                if (request.ContextStack != null)
                {
                    jsonWriter.WritePropertyName("contextStack");
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
		public IEnumerator QueryMetricsTimeseries(
                Request.QueryMetricsTimeseriesRequest request,
                UnityAction<AsyncResult<Result.QueryMetricsTimeseriesResult>> callback
        )
		{
			var task = new QueryMetricsTimeseriesTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.QueryMetricsTimeseriesResult>(task.Result, task.Error));
        }

		public IFuture<Result.QueryMetricsTimeseriesResult> QueryMetricsTimeseriesFuture(
                Request.QueryMetricsTimeseriesRequest request
        )
		{
			return new QueryMetricsTimeseriesTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.QueryMetricsTimeseriesResult> QueryMetricsTimeseriesAsync(
                Request.QueryMetricsTimeseriesRequest request
        )
		{
            AsyncResult<Result.QueryMetricsTimeseriesResult> result = null;
			await QueryMetricsTimeseries(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
    #else
		public QueryMetricsTimeseriesTask QueryMetricsTimeseriesAsync(
                Request.QueryMetricsTimeseriesRequest request
        )
		{
			return new QueryMetricsTimeseriesTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.QueryMetricsTimeseriesResult> QueryMetricsTimeseriesAsync(
                Request.QueryMetricsTimeseriesRequest request
        )
		{
			var task = new QueryMetricsTimeseriesTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class DescribeMetricsTask : Gs2RestSessionTask<DescribeMetricsRequest, DescribeMetricsResult>
        {
            public DescribeMetricsTask(IGs2Session session, RestSessionRequestFactory factory, DescribeMetricsRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DescribeMetricsRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "log")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/model/metrics";

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
		public IEnumerator DescribeMetrics(
                Request.DescribeMetricsRequest request,
                UnityAction<AsyncResult<Result.DescribeMetricsResult>> callback
        )
		{
			var task = new DescribeMetricsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.DescribeMetricsResult>(task.Result, task.Error));
        }

		public IFuture<Result.DescribeMetricsResult> DescribeMetricsFuture(
                Request.DescribeMetricsRequest request
        )
		{
			return new DescribeMetricsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.DescribeMetricsResult> DescribeMetricsAsync(
                Request.DescribeMetricsRequest request
        )
		{
            AsyncResult<Result.DescribeMetricsResult> result = null;
			await DescribeMetrics(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
    #else
		public DescribeMetricsTask DescribeMetricsAsync(
                Request.DescribeMetricsRequest request
        )
		{
			return new DescribeMetricsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.DescribeMetricsResult> DescribeMetricsAsync(
                Request.DescribeMetricsRequest request
        )
		{
			var task = new DescribeMetricsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class DescribeLabelValuesTask : Gs2RestSessionTask<DescribeLabelValuesRequest, DescribeLabelValuesResult>
        {
            public DescribeLabelValuesTask(IGs2Session session, RestSessionRequestFactory factory, DescribeLabelValuesRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DescribeLabelValuesRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "log")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/model/metrics/{metricName}/label";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{metricName}", !string.IsNullOrEmpty(request.MetricName) ? request.MetricName.ToString() : "null");

                var sessionRequest = Factory.Get(url);
                if (request.ContextStack != null)
                {
                    sessionRequest.AddQueryString("contextStack", request.ContextStack);
                }
                if (request.LabelNamePrefix != null) {
                    sessionRequest.AddQueryString("labelNamePrefix", $"{request.LabelNamePrefix}");
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
		public IEnumerator DescribeLabelValues(
                Request.DescribeLabelValuesRequest request,
                UnityAction<AsyncResult<Result.DescribeLabelValuesResult>> callback
        )
		{
			var task = new DescribeLabelValuesTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.DescribeLabelValuesResult>(task.Result, task.Error));
        }

		public IFuture<Result.DescribeLabelValuesResult> DescribeLabelValuesFuture(
                Request.DescribeLabelValuesRequest request
        )
		{
			return new DescribeLabelValuesTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.DescribeLabelValuesResult> DescribeLabelValuesAsync(
                Request.DescribeLabelValuesRequest request
        )
		{
            AsyncResult<Result.DescribeLabelValuesResult> result = null;
			await DescribeLabelValues(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
    #else
		public DescribeLabelValuesTask DescribeLabelValuesAsync(
                Request.DescribeLabelValuesRequest request
        )
		{
			return new DescribeLabelValuesTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.DescribeLabelValuesResult> DescribeLabelValuesAsync(
                Request.DescribeLabelValuesRequest request
        )
		{
			var task = new DescribeLabelValuesTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif
	}
}