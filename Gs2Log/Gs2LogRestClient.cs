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


        private class DescribeNamespacesTask : Gs2RestSessionTask<DescribeNamespacesRequest, DescribeNamespacesResult>
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
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.DescribeNamespacesResult>(task.Result, task.Error));
        }
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


        private class CreateNamespaceTask : Gs2RestSessionTask<CreateNamespaceRequest, CreateNamespaceResult>
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
                if (request.ContextStack != null)
                {
                    jsonWriter.WritePropertyName("contextStack");
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
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.CreateNamespaceResult>(task.Result, task.Error));
        }
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


        private class GetNamespaceStatusTask : Gs2RestSessionTask<GetNamespaceStatusRequest, GetNamespaceStatusResult>
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
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.GetNamespaceStatusResult>(task.Result, task.Error));
        }
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


        private class GetNamespaceTask : Gs2RestSessionTask<GetNamespaceRequest, GetNamespaceResult>
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
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.GetNamespaceResult>(task.Result, task.Error));
        }
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


        private class UpdateNamespaceTask : Gs2RestSessionTask<UpdateNamespaceRequest, UpdateNamespaceResult>
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
                if (request.ContextStack != null)
                {
                    jsonWriter.WritePropertyName("contextStack");
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
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.UpdateNamespaceResult>(task.Result, task.Error));
        }
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


        private class DeleteNamespaceTask : Gs2RestSessionTask<DeleteNamespaceRequest, DeleteNamespaceResult>
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
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.DeleteNamespaceResult>(task.Result, task.Error));
        }
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


        private class QueryAccessLogTask : Gs2RestSessionTask<QueryAccessLogRequest, QueryAccessLogResult>
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
		public IEnumerator QueryAccessLog(
                Request.QueryAccessLogRequest request,
                UnityAction<AsyncResult<Result.QueryAccessLogResult>> callback
        )
		{
			var task = new QueryAccessLogTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.QueryAccessLogResult>(task.Result, task.Error));
        }
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


        private class CountAccessLogTask : Gs2RestSessionTask<CountAccessLogRequest, CountAccessLogResult>
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
		public IEnumerator CountAccessLog(
                Request.CountAccessLogRequest request,
                UnityAction<AsyncResult<Result.CountAccessLogResult>> callback
        )
		{
			var task = new CountAccessLogTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.CountAccessLogResult>(task.Result, task.Error));
        }
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


        private class QueryIssueStampSheetLogTask : Gs2RestSessionTask<QueryIssueStampSheetLogRequest, QueryIssueStampSheetLogResult>
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
		public IEnumerator QueryIssueStampSheetLog(
                Request.QueryIssueStampSheetLogRequest request,
                UnityAction<AsyncResult<Result.QueryIssueStampSheetLogResult>> callback
        )
		{
			var task = new QueryIssueStampSheetLogTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.QueryIssueStampSheetLogResult>(task.Result, task.Error));
        }
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


        private class CountIssueStampSheetLogTask : Gs2RestSessionTask<CountIssueStampSheetLogRequest, CountIssueStampSheetLogResult>
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
		public IEnumerator CountIssueStampSheetLog(
                Request.CountIssueStampSheetLogRequest request,
                UnityAction<AsyncResult<Result.CountIssueStampSheetLogResult>> callback
        )
		{
			var task = new CountIssueStampSheetLogTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.CountIssueStampSheetLogResult>(task.Result, task.Error));
        }
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


        private class QueryExecuteStampSheetLogTask : Gs2RestSessionTask<QueryExecuteStampSheetLogRequest, QueryExecuteStampSheetLogResult>
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
		public IEnumerator QueryExecuteStampSheetLog(
                Request.QueryExecuteStampSheetLogRequest request,
                UnityAction<AsyncResult<Result.QueryExecuteStampSheetLogResult>> callback
        )
		{
			var task = new QueryExecuteStampSheetLogTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.QueryExecuteStampSheetLogResult>(task.Result, task.Error));
        }
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


        private class CountExecuteStampSheetLogTask : Gs2RestSessionTask<CountExecuteStampSheetLogRequest, CountExecuteStampSheetLogResult>
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
		public IEnumerator CountExecuteStampSheetLog(
                Request.CountExecuteStampSheetLogRequest request,
                UnityAction<AsyncResult<Result.CountExecuteStampSheetLogResult>> callback
        )
		{
			var task = new CountExecuteStampSheetLogTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.CountExecuteStampSheetLogResult>(task.Result, task.Error));
        }
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


        private class QueryExecuteStampTaskLogTask : Gs2RestSessionTask<QueryExecuteStampTaskLogRequest, QueryExecuteStampTaskLogResult>
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
		public IEnumerator QueryExecuteStampTaskLog(
                Request.QueryExecuteStampTaskLogRequest request,
                UnityAction<AsyncResult<Result.QueryExecuteStampTaskLogResult>> callback
        )
		{
			var task = new QueryExecuteStampTaskLogTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.QueryExecuteStampTaskLogResult>(task.Result, task.Error));
        }
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


        private class CountExecuteStampTaskLogTask : Gs2RestSessionTask<CountExecuteStampTaskLogRequest, CountExecuteStampTaskLogResult>
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
		public IEnumerator CountExecuteStampTaskLog(
                Request.CountExecuteStampTaskLogRequest request,
                UnityAction<AsyncResult<Result.CountExecuteStampTaskLogResult>> callback
        )
		{
			var task = new CountExecuteStampTaskLogTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.CountExecuteStampTaskLogResult>(task.Result, task.Error));
        }
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


        private class PutLogTask : Gs2RestSessionTask<PutLogRequest, PutLogResult>
        {
            public PutLogTask(IGs2Session session, RestSessionRequestFactory factory, PutLogRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(PutLogRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "log")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/log/put";

                var sessionRequest = Factory.Post(url);

                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);
                jsonWriter.WriteObjectStart();
                if (request.LoggingNamespaceId != null)
                {
                    jsonWriter.WritePropertyName("loggingNamespaceId");
                    jsonWriter.Write(request.LoggingNamespaceId);
                }
                if (request.LogCategory != null)
                {
                    jsonWriter.WritePropertyName("logCategory");
                    jsonWriter.Write(request.LogCategory);
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
		public IEnumerator PutLog(
                Request.PutLogRequest request,
                UnityAction<AsyncResult<Result.PutLogResult>> callback
        )
		{
			var task = new PutLogTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.PutLogResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.PutLogResult> PutLogAsync(
                Request.PutLogRequest request
        )
		{
			var task = new PutLogTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif
	}
}