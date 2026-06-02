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
        ) =>
            new QueryAccessLogTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.QueryAccessLogResult> QueryAccessLogFuture(
                Request.QueryAccessLogRequest request
        ) =>
            new QueryAccessLogTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.QueryAccessLogResult> QueryAccessLogAsync(
                Request.QueryAccessLogRequest request
        ) =>
            new QueryAccessLogTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.QueryAccessLogResult>();
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
		public Task<Result.QueryAccessLogResult> QueryAccessLogAsync(
                Request.QueryAccessLogRequest request
        ) =>
            new QueryAccessLogTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
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
        ) =>
            new CountAccessLogTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.CountAccessLogResult> CountAccessLogFuture(
                Request.CountAccessLogRequest request
        ) =>
            new CountAccessLogTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.CountAccessLogResult> CountAccessLogAsync(
                Request.CountAccessLogRequest request
        ) =>
            new CountAccessLogTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.CountAccessLogResult>();
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
		public Task<Result.CountAccessLogResult> CountAccessLogAsync(
                Request.CountAccessLogRequest request
        ) =>
            new CountAccessLogTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
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
        ) =>
            new QueryIssueStampSheetLogTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.QueryIssueStampSheetLogResult> QueryIssueStampSheetLogFuture(
                Request.QueryIssueStampSheetLogRequest request
        ) =>
            new QueryIssueStampSheetLogTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.QueryIssueStampSheetLogResult> QueryIssueStampSheetLogAsync(
                Request.QueryIssueStampSheetLogRequest request
        ) =>
            new QueryIssueStampSheetLogTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.QueryIssueStampSheetLogResult>();
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
		public Task<Result.QueryIssueStampSheetLogResult> QueryIssueStampSheetLogAsync(
                Request.QueryIssueStampSheetLogRequest request
        ) =>
            new QueryIssueStampSheetLogTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
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
        ) =>
            new CountIssueStampSheetLogTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.CountIssueStampSheetLogResult> CountIssueStampSheetLogFuture(
                Request.CountIssueStampSheetLogRequest request
        ) =>
            new CountIssueStampSheetLogTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.CountIssueStampSheetLogResult> CountIssueStampSheetLogAsync(
                Request.CountIssueStampSheetLogRequest request
        ) =>
            new CountIssueStampSheetLogTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.CountIssueStampSheetLogResult>();
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
		public Task<Result.CountIssueStampSheetLogResult> CountIssueStampSheetLogAsync(
                Request.CountIssueStampSheetLogRequest request
        ) =>
            new CountIssueStampSheetLogTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
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
        ) =>
            new QueryExecuteStampSheetLogTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.QueryExecuteStampSheetLogResult> QueryExecuteStampSheetLogFuture(
                Request.QueryExecuteStampSheetLogRequest request
        ) =>
            new QueryExecuteStampSheetLogTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.QueryExecuteStampSheetLogResult> QueryExecuteStampSheetLogAsync(
                Request.QueryExecuteStampSheetLogRequest request
        ) =>
            new QueryExecuteStampSheetLogTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.QueryExecuteStampSheetLogResult>();
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
		public Task<Result.QueryExecuteStampSheetLogResult> QueryExecuteStampSheetLogAsync(
                Request.QueryExecuteStampSheetLogRequest request
        ) =>
            new QueryExecuteStampSheetLogTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
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
        ) =>
            new CountExecuteStampSheetLogTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.CountExecuteStampSheetLogResult> CountExecuteStampSheetLogFuture(
                Request.CountExecuteStampSheetLogRequest request
        ) =>
            new CountExecuteStampSheetLogTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.CountExecuteStampSheetLogResult> CountExecuteStampSheetLogAsync(
                Request.CountExecuteStampSheetLogRequest request
        ) =>
            new CountExecuteStampSheetLogTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.CountExecuteStampSheetLogResult>();
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
		public Task<Result.CountExecuteStampSheetLogResult> CountExecuteStampSheetLogAsync(
                Request.CountExecuteStampSheetLogRequest request
        ) =>
            new CountExecuteStampSheetLogTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
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
        ) =>
            new QueryExecuteStampTaskLogTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.QueryExecuteStampTaskLogResult> QueryExecuteStampTaskLogFuture(
                Request.QueryExecuteStampTaskLogRequest request
        ) =>
            new QueryExecuteStampTaskLogTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.QueryExecuteStampTaskLogResult> QueryExecuteStampTaskLogAsync(
                Request.QueryExecuteStampTaskLogRequest request
        ) =>
            new QueryExecuteStampTaskLogTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.QueryExecuteStampTaskLogResult>();
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
		public Task<Result.QueryExecuteStampTaskLogResult> QueryExecuteStampTaskLogAsync(
                Request.QueryExecuteStampTaskLogRequest request
        ) =>
            new QueryExecuteStampTaskLogTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
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
        ) =>
            new CountExecuteStampTaskLogTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.CountExecuteStampTaskLogResult> CountExecuteStampTaskLogFuture(
                Request.CountExecuteStampTaskLogRequest request
        ) =>
            new CountExecuteStampTaskLogTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.CountExecuteStampTaskLogResult> CountExecuteStampTaskLogAsync(
                Request.CountExecuteStampTaskLogRequest request
        ) =>
            new CountExecuteStampTaskLogTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.CountExecuteStampTaskLogResult>();
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
		public Task<Result.CountExecuteStampTaskLogResult> CountExecuteStampTaskLogAsync(
                Request.CountExecuteStampTaskLogRequest request
        ) =>
            new CountExecuteStampTaskLogTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
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
        ) =>
            new QueryInGameLogTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.QueryInGameLogResult> QueryInGameLogFuture(
                Request.QueryInGameLogRequest request
        ) =>
            new QueryInGameLogTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.QueryInGameLogResult> QueryInGameLogAsync(
                Request.QueryInGameLogRequest request
        ) =>
            new QueryInGameLogTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.QueryInGameLogResult>();
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
		public Task<Result.QueryInGameLogResult> QueryInGameLogAsync(
                Request.QueryInGameLogRequest request
        ) =>
            new QueryInGameLogTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
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
        ) =>
            new SendInGameLogTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.SendInGameLogResult> SendInGameLogFuture(
                Request.SendInGameLogRequest request
        ) =>
            new SendInGameLogTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.SendInGameLogResult> SendInGameLogAsync(
                Request.SendInGameLogRequest request
        ) =>
            new SendInGameLogTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.SendInGameLogResult>();
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
		public Task<Result.SendInGameLogResult> SendInGameLogAsync(
                Request.SendInGameLogRequest request
        ) =>
            new SendInGameLogTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
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
        ) =>
            new SendInGameLogByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.SendInGameLogByUserIdResult> SendInGameLogByUserIdFuture(
                Request.SendInGameLogByUserIdRequest request
        ) =>
            new SendInGameLogByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.SendInGameLogByUserIdResult> SendInGameLogByUserIdAsync(
                Request.SendInGameLogByUserIdRequest request
        ) =>
            new SendInGameLogByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.SendInGameLogByUserIdResult>();
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
		public Task<Result.SendInGameLogByUserIdResult> SendInGameLogByUserIdAsync(
                Request.SendInGameLogByUserIdRequest request
        ) =>
            new SendInGameLogByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
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
        ) =>
            new QueryAccessLogWithTelemetryTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.QueryAccessLogWithTelemetryResult> QueryAccessLogWithTelemetryFuture(
                Request.QueryAccessLogWithTelemetryRequest request
        ) =>
            new QueryAccessLogWithTelemetryTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.QueryAccessLogWithTelemetryResult> QueryAccessLogWithTelemetryAsync(
                Request.QueryAccessLogWithTelemetryRequest request
        ) =>
            new QueryAccessLogWithTelemetryTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.QueryAccessLogWithTelemetryResult>();
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
		public Task<Result.QueryAccessLogWithTelemetryResult> QueryAccessLogWithTelemetryAsync(
                Request.QueryAccessLogWithTelemetryRequest request
        ) =>
            new QueryAccessLogWithTelemetryTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
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
        ) =>
            new DescribeInsightsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.DescribeInsightsResult> DescribeInsightsFuture(
                Request.DescribeInsightsRequest request
        ) =>
            new DescribeInsightsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.DescribeInsightsResult> DescribeInsightsAsync(
                Request.DescribeInsightsRequest request
        ) =>
            new DescribeInsightsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.DescribeInsightsResult>();
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
		public Task<Result.DescribeInsightsResult> DescribeInsightsAsync(
                Request.DescribeInsightsRequest request
        ) =>
            new DescribeInsightsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
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
        ) =>
            new CreateInsightTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.CreateInsightResult> CreateInsightFuture(
                Request.CreateInsightRequest request
        ) =>
            new CreateInsightTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.CreateInsightResult> CreateInsightAsync(
                Request.CreateInsightRequest request
        ) =>
            new CreateInsightTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.CreateInsightResult>();
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
		public Task<Result.CreateInsightResult> CreateInsightAsync(
                Request.CreateInsightRequest request
        ) =>
            new CreateInsightTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
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
        ) =>
            new GetInsightTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.GetInsightResult> GetInsightFuture(
                Request.GetInsightRequest request
        ) =>
            new GetInsightTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.GetInsightResult> GetInsightAsync(
                Request.GetInsightRequest request
        ) =>
            new GetInsightTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.GetInsightResult>();
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
		public Task<Result.GetInsightResult> GetInsightAsync(
                Request.GetInsightRequest request
        ) =>
            new GetInsightTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
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
        ) =>
            new DeleteInsightTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.DeleteInsightResult> DeleteInsightFuture(
                Request.DeleteInsightRequest request
        ) =>
            new DeleteInsightTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.DeleteInsightResult> DeleteInsightAsync(
                Request.DeleteInsightRequest request
        ) =>
            new DeleteInsightTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.DeleteInsightResult>();
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
		public Task<Result.DeleteInsightResult> DeleteInsightAsync(
                Request.DeleteInsightRequest request
        ) =>
            new DeleteInsightTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
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
        ) =>
            new DescribeFacetModelsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.DescribeFacetModelsResult> DescribeFacetModelsFuture(
                Request.DescribeFacetModelsRequest request
        ) =>
            new DescribeFacetModelsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.DescribeFacetModelsResult> DescribeFacetModelsAsync(
                Request.DescribeFacetModelsRequest request
        ) =>
            new DescribeFacetModelsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.DescribeFacetModelsResult>();
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
		public Task<Result.DescribeFacetModelsResult> DescribeFacetModelsAsync(
                Request.DescribeFacetModelsRequest request
        ) =>
            new DescribeFacetModelsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
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
        ) =>
            new CreateFacetModelTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.CreateFacetModelResult> CreateFacetModelFuture(
                Request.CreateFacetModelRequest request
        ) =>
            new CreateFacetModelTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.CreateFacetModelResult> CreateFacetModelAsync(
                Request.CreateFacetModelRequest request
        ) =>
            new CreateFacetModelTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.CreateFacetModelResult>();
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
		public Task<Result.CreateFacetModelResult> CreateFacetModelAsync(
                Request.CreateFacetModelRequest request
        ) =>
            new CreateFacetModelTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
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
        ) =>
            new GetFacetModelTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.GetFacetModelResult> GetFacetModelFuture(
                Request.GetFacetModelRequest request
        ) =>
            new GetFacetModelTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.GetFacetModelResult> GetFacetModelAsync(
                Request.GetFacetModelRequest request
        ) =>
            new GetFacetModelTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.GetFacetModelResult>();
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
		public Task<Result.GetFacetModelResult> GetFacetModelAsync(
                Request.GetFacetModelRequest request
        ) =>
            new GetFacetModelTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
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
        ) =>
            new UpdateFacetModelTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.UpdateFacetModelResult> UpdateFacetModelFuture(
                Request.UpdateFacetModelRequest request
        ) =>
            new UpdateFacetModelTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.UpdateFacetModelResult> UpdateFacetModelAsync(
                Request.UpdateFacetModelRequest request
        ) =>
            new UpdateFacetModelTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.UpdateFacetModelResult>();
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
		public Task<Result.UpdateFacetModelResult> UpdateFacetModelAsync(
                Request.UpdateFacetModelRequest request
        ) =>
            new UpdateFacetModelTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
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
        ) =>
            new DeleteFacetModelTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.DeleteFacetModelResult> DeleteFacetModelFuture(
                Request.DeleteFacetModelRequest request
        ) =>
            new DeleteFacetModelTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.DeleteFacetModelResult> DeleteFacetModelAsync(
                Request.DeleteFacetModelRequest request
        ) =>
            new DeleteFacetModelTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.DeleteFacetModelResult>();
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
		public Task<Result.DeleteFacetModelResult> DeleteFacetModelAsync(
                Request.DeleteFacetModelRequest request
        ) =>
            new DeleteFacetModelTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
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
        ) =>
            new DescribeDashboardsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.DescribeDashboardsResult> DescribeDashboardsFuture(
                Request.DescribeDashboardsRequest request
        ) =>
            new DescribeDashboardsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.DescribeDashboardsResult> DescribeDashboardsAsync(
                Request.DescribeDashboardsRequest request
        ) =>
            new DescribeDashboardsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.DescribeDashboardsResult>();
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
		public Task<Result.DescribeDashboardsResult> DescribeDashboardsAsync(
                Request.DescribeDashboardsRequest request
        ) =>
            new DescribeDashboardsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
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
        ) =>
            new CreateDashboardTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.CreateDashboardResult> CreateDashboardFuture(
                Request.CreateDashboardRequest request
        ) =>
            new CreateDashboardTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.CreateDashboardResult> CreateDashboardAsync(
                Request.CreateDashboardRequest request
        ) =>
            new CreateDashboardTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.CreateDashboardResult>();
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
		public Task<Result.CreateDashboardResult> CreateDashboardAsync(
                Request.CreateDashboardRequest request
        ) =>
            new CreateDashboardTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
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
        ) =>
            new GetDashboardTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.GetDashboardResult> GetDashboardFuture(
                Request.GetDashboardRequest request
        ) =>
            new GetDashboardTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.GetDashboardResult> GetDashboardAsync(
                Request.GetDashboardRequest request
        ) =>
            new GetDashboardTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.GetDashboardResult>();
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
		public Task<Result.GetDashboardResult> GetDashboardAsync(
                Request.GetDashboardRequest request
        ) =>
            new GetDashboardTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
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
        ) =>
            new UpdateDashboardTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.UpdateDashboardResult> UpdateDashboardFuture(
                Request.UpdateDashboardRequest request
        ) =>
            new UpdateDashboardTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.UpdateDashboardResult> UpdateDashboardAsync(
                Request.UpdateDashboardRequest request
        ) =>
            new UpdateDashboardTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.UpdateDashboardResult>();
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
		public Task<Result.UpdateDashboardResult> UpdateDashboardAsync(
                Request.UpdateDashboardRequest request
        ) =>
            new UpdateDashboardTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
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
        ) =>
            new DuplicateDashboardTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.DuplicateDashboardResult> DuplicateDashboardFuture(
                Request.DuplicateDashboardRequest request
        ) =>
            new DuplicateDashboardTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.DuplicateDashboardResult> DuplicateDashboardAsync(
                Request.DuplicateDashboardRequest request
        ) =>
            new DuplicateDashboardTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.DuplicateDashboardResult>();
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
		public Task<Result.DuplicateDashboardResult> DuplicateDashboardAsync(
                Request.DuplicateDashboardRequest request
        ) =>
            new DuplicateDashboardTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
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
        ) =>
            new DeleteDashboardTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.DeleteDashboardResult> DeleteDashboardFuture(
                Request.DeleteDashboardRequest request
        ) =>
            new DeleteDashboardTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.DeleteDashboardResult> DeleteDashboardAsync(
                Request.DeleteDashboardRequest request
        ) =>
            new DeleteDashboardTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.DeleteDashboardResult>();
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
		public Task<Result.DeleteDashboardResult> DeleteDashboardAsync(
                Request.DeleteDashboardRequest request
        ) =>
            new DeleteDashboardTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
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
        ) =>
            new QueryLogTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.QueryLogResult> QueryLogFuture(
                Request.QueryLogRequest request
        ) =>
            new QueryLogTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.QueryLogResult> QueryLogAsync(
                Request.QueryLogRequest request
        ) =>
            new QueryLogTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.QueryLogResult>();
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
		public Task<Result.QueryLogResult> QueryLogAsync(
                Request.QueryLogRequest request
        ) =>
            new QueryLogTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
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
        ) =>
            new GetLogTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.GetLogResult> GetLogFuture(
                Request.GetLogRequest request
        ) =>
            new GetLogTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.GetLogResult> GetLogAsync(
                Request.GetLogRequest request
        ) =>
            new GetLogTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.GetLogResult>();
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
		public Task<Result.GetLogResult> GetLogAsync(
                Request.GetLogRequest request
        ) =>
            new GetLogTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
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
        ) =>
            new QueryFacetsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.QueryFacetsResult> QueryFacetsFuture(
                Request.QueryFacetsRequest request
        ) =>
            new QueryFacetsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.QueryFacetsResult> QueryFacetsAsync(
                Request.QueryFacetsRequest request
        ) =>
            new QueryFacetsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.QueryFacetsResult>();
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
		public Task<Result.QueryFacetsResult> QueryFacetsAsync(
                Request.QueryFacetsRequest request
        ) =>
            new QueryFacetsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
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
        ) =>
            new QueryTimeseriesTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.QueryTimeseriesResult> QueryTimeseriesFuture(
                Request.QueryTimeseriesRequest request
        ) =>
            new QueryTimeseriesTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.QueryTimeseriesResult> QueryTimeseriesAsync(
                Request.QueryTimeseriesRequest request
        ) =>
            new QueryTimeseriesTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.QueryTimeseriesResult>();
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
		public Task<Result.QueryTimeseriesResult> QueryTimeseriesAsync(
                Request.QueryTimeseriesRequest request
        ) =>
            new QueryTimeseriesTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
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
        ) =>
            new GetTraceTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.GetTraceResult> GetTraceFuture(
                Request.GetTraceRequest request
        ) =>
            new GetTraceTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.GetTraceResult> GetTraceAsync(
                Request.GetTraceRequest request
        ) =>
            new GetTraceTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.GetTraceResult>();
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
		public Task<Result.GetTraceResult> GetTraceAsync(
                Request.GetTraceRequest request
        ) =>
            new GetTraceTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
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
        ) =>
            new QueryMetricsTimeseriesTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.QueryMetricsTimeseriesResult> QueryMetricsTimeseriesFuture(
                Request.QueryMetricsTimeseriesRequest request
        ) =>
            new QueryMetricsTimeseriesTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.QueryMetricsTimeseriesResult> QueryMetricsTimeseriesAsync(
                Request.QueryMetricsTimeseriesRequest request
        ) =>
            new QueryMetricsTimeseriesTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.QueryMetricsTimeseriesResult>();
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
		public Task<Result.QueryMetricsTimeseriesResult> QueryMetricsTimeseriesAsync(
                Request.QueryMetricsTimeseriesRequest request
        ) =>
            new QueryMetricsTimeseriesTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
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
        ) =>
            new DescribeMetricsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.DescribeMetricsResult> DescribeMetricsFuture(
                Request.DescribeMetricsRequest request
        ) =>
            new DescribeMetricsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.DescribeMetricsResult> DescribeMetricsAsync(
                Request.DescribeMetricsRequest request
        ) =>
            new DescribeMetricsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.DescribeMetricsResult>();
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
		public Task<Result.DescribeMetricsResult> DescribeMetricsAsync(
                Request.DescribeMetricsRequest request
        ) =>
            new DescribeMetricsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
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
        ) =>
            new DescribeLabelValuesTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.DescribeLabelValuesResult> DescribeLabelValuesFuture(
                Request.DescribeLabelValuesRequest request
        ) =>
            new DescribeLabelValuesTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.DescribeLabelValuesResult> DescribeLabelValuesAsync(
                Request.DescribeLabelValuesRequest request
        ) =>
            new DescribeLabelValuesTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.DescribeLabelValuesResult>();
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
		public Task<Result.DescribeLabelValuesResult> DescribeLabelValuesAsync(
                Request.DescribeLabelValuesRequest request
        ) =>
            new DescribeLabelValuesTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif
	}
}