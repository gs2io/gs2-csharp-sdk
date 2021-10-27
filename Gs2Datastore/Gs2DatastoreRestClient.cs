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
using Gs2.Gs2Datastore.Request;
using Gs2.Gs2Datastore.Result;
using Gs2.Util.LitJson;

namespace Gs2.Gs2Datastore
{
	public class Gs2DatastoreRestClient : AbstractGs2Client
	{
#if UNITY_2017_1_OR_NEWER
		private readonly CertificateHandler _certificateHandler;
#endif

		public static string Endpoint = "datastore";

        protected Gs2RestSession Gs2RestSession => (Gs2RestSession) Gs2Session;

		public Gs2DatastoreRestClient(Gs2RestSession Gs2RestSession) : base(Gs2RestSession)
		{

		}

#if UNITY_2017_1_OR_NEWER
		public Gs2DatastoreRestClient(Gs2RestSession gs2RestSession, CertificateHandler certificateHandler) : base(gs2RestSession)
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
                    .Replace("{service}", "datastore")
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
                    .Replace("{service}", "datastore")
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
                if (request.LogSetting != null)
                {
                    jsonWriter.WritePropertyName("logSetting");
                    request.LogSetting.WriteJson(jsonWriter);
                }
                if (request.DoneUploadScript != null)
                {
                    jsonWriter.WritePropertyName("doneUploadScript");
                    request.DoneUploadScript.WriteJson(jsonWriter);
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
                    .Replace("{service}", "datastore")
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
                    .Replace("{service}", "datastore")
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
                    .Replace("{service}", "datastore")
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
                if (request.LogSetting != null)
                {
                    jsonWriter.WritePropertyName("logSetting");
                    request.LogSetting.WriteJson(jsonWriter);
                }
                if (request.DoneUploadScript != null)
                {
                    jsonWriter.WritePropertyName("doneUploadScript");
                    request.DoneUploadScript.WriteJson(jsonWriter);
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
                    .Replace("{service}", "datastore")
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


        private class DescribeDataObjectsTask : Gs2RestSessionTask<DescribeDataObjectsRequest, DescribeDataObjectsResult>
        {
            public DescribeDataObjectsTask(IGs2Session session, RestSessionRequestFactory factory, DescribeDataObjectsRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DescribeDataObjectsRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "datastore")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/me/data";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");

                var sessionRequest = Factory.Get(url);
                if (request.ContextStack != null)
                {
                    sessionRequest.AddQueryString("contextStack", request.ContextStack);
                }
                if (request.Status != null) {
                    sessionRequest.AddQueryString("status", $"{request.Status}");
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
		public IEnumerator DescribeDataObjects(
                Request.DescribeDataObjectsRequest request,
                UnityAction<AsyncResult<Result.DescribeDataObjectsResult>> callback
        )
		{
			var task = new DescribeDataObjectsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.DescribeDataObjectsResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.DescribeDataObjectsResult> DescribeDataObjectsAsync(
                Request.DescribeDataObjectsRequest request
        )
		{
			var task = new DescribeDataObjectsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        private class DescribeDataObjectsByUserIdTask : Gs2RestSessionTask<DescribeDataObjectsByUserIdRequest, DescribeDataObjectsByUserIdResult>
        {
            public DescribeDataObjectsByUserIdTask(IGs2Session session, RestSessionRequestFactory factory, DescribeDataObjectsByUserIdRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DescribeDataObjectsByUserIdRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "datastore")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/{userId}/data";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{userId}", !string.IsNullOrEmpty(request.UserId) ? request.UserId.ToString() : "null");

                var sessionRequest = Factory.Get(url);
                if (request.ContextStack != null)
                {
                    sessionRequest.AddQueryString("contextStack", request.ContextStack);
                }
                if (request.Status != null) {
                    sessionRequest.AddQueryString("status", $"{request.Status}");
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
		public IEnumerator DescribeDataObjectsByUserId(
                Request.DescribeDataObjectsByUserIdRequest request,
                UnityAction<AsyncResult<Result.DescribeDataObjectsByUserIdResult>> callback
        )
		{
			var task = new DescribeDataObjectsByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.DescribeDataObjectsByUserIdResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.DescribeDataObjectsByUserIdResult> DescribeDataObjectsByUserIdAsync(
                Request.DescribeDataObjectsByUserIdRequest request
        )
		{
			var task = new DescribeDataObjectsByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        private class PrepareUploadTask : Gs2RestSessionTask<PrepareUploadRequest, PrepareUploadResult>
        {
            public PrepareUploadTask(IGs2Session session, RestSessionRequestFactory factory, PrepareUploadRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(PrepareUploadRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "datastore")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/me/data/file";

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
                if (request.ContentType != null)
                {
                    jsonWriter.WritePropertyName("contentType");
                    jsonWriter.Write(request.ContentType);
                }
                if (request.Scope != null)
                {
                    jsonWriter.WritePropertyName("scope");
                    jsonWriter.Write(request.Scope);
                }
                if (request.AllowUserIds != null)
                {
                    jsonWriter.WritePropertyName("allowUserIds");
                    jsonWriter.WriteArrayStart();
                    foreach(var item in request.AllowUserIds)
                    {
                        jsonWriter.Write(item);
                    }
                    jsonWriter.WriteArrayEnd();
                }
                if (request.UpdateIfExists != null)
                {
                    jsonWriter.WritePropertyName("updateIfExists");
                    jsonWriter.Write(request.UpdateIfExists.ToString());
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
		public IEnumerator PrepareUpload(
                Request.PrepareUploadRequest request,
                UnityAction<AsyncResult<Result.PrepareUploadResult>> callback
        )
		{
			var task = new PrepareUploadTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.PrepareUploadResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.PrepareUploadResult> PrepareUploadAsync(
                Request.PrepareUploadRequest request
        )
		{
			var task = new PrepareUploadTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        private class PrepareUploadByUserIdTask : Gs2RestSessionTask<PrepareUploadByUserIdRequest, PrepareUploadByUserIdResult>
        {
            public PrepareUploadByUserIdTask(IGs2Session session, RestSessionRequestFactory factory, PrepareUploadByUserIdRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(PrepareUploadByUserIdRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "datastore")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/{userId}/data/file";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{userId}", !string.IsNullOrEmpty(request.UserId) ? request.UserId.ToString() : "null");

                var sessionRequest = Factory.Post(url);

                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);
                jsonWriter.WriteObjectStart();
                if (request.Name != null)
                {
                    jsonWriter.WritePropertyName("name");
                    jsonWriter.Write(request.Name);
                }
                if (request.ContentType != null)
                {
                    jsonWriter.WritePropertyName("contentType");
                    jsonWriter.Write(request.ContentType);
                }
                if (request.Scope != null)
                {
                    jsonWriter.WritePropertyName("scope");
                    jsonWriter.Write(request.Scope);
                }
                if (request.AllowUserIds != null)
                {
                    jsonWriter.WritePropertyName("allowUserIds");
                    jsonWriter.WriteArrayStart();
                    foreach(var item in request.AllowUserIds)
                    {
                        jsonWriter.Write(item);
                    }
                    jsonWriter.WriteArrayEnd();
                }
                if (request.UpdateIfExists != null)
                {
                    jsonWriter.WritePropertyName("updateIfExists");
                    jsonWriter.Write(request.UpdateIfExists.ToString());
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
		public IEnumerator PrepareUploadByUserId(
                Request.PrepareUploadByUserIdRequest request,
                UnityAction<AsyncResult<Result.PrepareUploadByUserIdResult>> callback
        )
		{
			var task = new PrepareUploadByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.PrepareUploadByUserIdResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.PrepareUploadByUserIdResult> PrepareUploadByUserIdAsync(
                Request.PrepareUploadByUserIdRequest request
        )
		{
			var task = new PrepareUploadByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        private class UpdateDataObjectTask : Gs2RestSessionTask<UpdateDataObjectRequest, UpdateDataObjectResult>
        {
            public UpdateDataObjectTask(IGs2Session session, RestSessionRequestFactory factory, UpdateDataObjectRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(UpdateDataObjectRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "datastore")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/me/data/{dataObjectName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{dataObjectName}", !string.IsNullOrEmpty(request.DataObjectName) ? request.DataObjectName.ToString() : "null");

                var sessionRequest = Factory.Post(url);

                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);
                jsonWriter.WriteObjectStart();
                if (request.Scope != null)
                {
                    jsonWriter.WritePropertyName("scope");
                    jsonWriter.Write(request.Scope);
                }
                if (request.AllowUserIds != null)
                {
                    jsonWriter.WritePropertyName("allowUserIds");
                    jsonWriter.WriteArrayStart();
                    foreach(var item in request.AllowUserIds)
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
		public IEnumerator UpdateDataObject(
                Request.UpdateDataObjectRequest request,
                UnityAction<AsyncResult<Result.UpdateDataObjectResult>> callback
        )
		{
			var task = new UpdateDataObjectTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.UpdateDataObjectResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.UpdateDataObjectResult> UpdateDataObjectAsync(
                Request.UpdateDataObjectRequest request
        )
		{
			var task = new UpdateDataObjectTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        private class UpdateDataObjectByUserIdTask : Gs2RestSessionTask<UpdateDataObjectByUserIdRequest, UpdateDataObjectByUserIdResult>
        {
            public UpdateDataObjectByUserIdTask(IGs2Session session, RestSessionRequestFactory factory, UpdateDataObjectByUserIdRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(UpdateDataObjectByUserIdRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "datastore")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/{userId}/data/{dataObjectName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{dataObjectName}", !string.IsNullOrEmpty(request.DataObjectName) ? request.DataObjectName.ToString() : "null");
                url = url.Replace("{userId}", !string.IsNullOrEmpty(request.UserId) ? request.UserId.ToString() : "null");

                var sessionRequest = Factory.Post(url);

                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);
                jsonWriter.WriteObjectStart();
                if (request.Scope != null)
                {
                    jsonWriter.WritePropertyName("scope");
                    jsonWriter.Write(request.Scope);
                }
                if (request.AllowUserIds != null)
                {
                    jsonWriter.WritePropertyName("allowUserIds");
                    jsonWriter.WriteArrayStart();
                    foreach(var item in request.AllowUserIds)
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
		public IEnumerator UpdateDataObjectByUserId(
                Request.UpdateDataObjectByUserIdRequest request,
                UnityAction<AsyncResult<Result.UpdateDataObjectByUserIdResult>> callback
        )
		{
			var task = new UpdateDataObjectByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.UpdateDataObjectByUserIdResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.UpdateDataObjectByUserIdResult> UpdateDataObjectByUserIdAsync(
                Request.UpdateDataObjectByUserIdRequest request
        )
		{
			var task = new UpdateDataObjectByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        private class PrepareReUploadTask : Gs2RestSessionTask<PrepareReUploadRequest, PrepareReUploadResult>
        {
            public PrepareReUploadTask(IGs2Session session, RestSessionRequestFactory factory, PrepareReUploadRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(PrepareReUploadRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "datastore")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/me/data/{dataObjectName}/file/reUpload";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{dataObjectName}", !string.IsNullOrEmpty(request.DataObjectName) ? request.DataObjectName.ToString() : "null");

                var sessionRequest = Factory.Post(url);

                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);
                jsonWriter.WriteObjectStart();
                if (request.ContentType != null)
                {
                    jsonWriter.WritePropertyName("contentType");
                    jsonWriter.Write(request.ContentType);
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
		public IEnumerator PrepareReUpload(
                Request.PrepareReUploadRequest request,
                UnityAction<AsyncResult<Result.PrepareReUploadResult>> callback
        )
		{
			var task = new PrepareReUploadTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.PrepareReUploadResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.PrepareReUploadResult> PrepareReUploadAsync(
                Request.PrepareReUploadRequest request
        )
		{
			var task = new PrepareReUploadTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        private class PrepareReUploadByUserIdTask : Gs2RestSessionTask<PrepareReUploadByUserIdRequest, PrepareReUploadByUserIdResult>
        {
            public PrepareReUploadByUserIdTask(IGs2Session session, RestSessionRequestFactory factory, PrepareReUploadByUserIdRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(PrepareReUploadByUserIdRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "datastore")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/{userId}/data/{dataObjectName}/file/reUpload";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{dataObjectName}", !string.IsNullOrEmpty(request.DataObjectName) ? request.DataObjectName.ToString() : "null");
                url = url.Replace("{userId}", !string.IsNullOrEmpty(request.UserId) ? request.UserId.ToString() : "null");

                var sessionRequest = Factory.Post(url);

                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);
                jsonWriter.WriteObjectStart();
                if (request.ContentType != null)
                {
                    jsonWriter.WritePropertyName("contentType");
                    jsonWriter.Write(request.ContentType);
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
		public IEnumerator PrepareReUploadByUserId(
                Request.PrepareReUploadByUserIdRequest request,
                UnityAction<AsyncResult<Result.PrepareReUploadByUserIdResult>> callback
        )
		{
			var task = new PrepareReUploadByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.PrepareReUploadByUserIdResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.PrepareReUploadByUserIdResult> PrepareReUploadByUserIdAsync(
                Request.PrepareReUploadByUserIdRequest request
        )
		{
			var task = new PrepareReUploadByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        private class DoneUploadTask : Gs2RestSessionTask<DoneUploadRequest, DoneUploadResult>
        {
            public DoneUploadTask(IGs2Session session, RestSessionRequestFactory factory, DoneUploadRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DoneUploadRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "datastore")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/me/data/{dataObjectName}/done";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{dataObjectName}", !string.IsNullOrEmpty(request.DataObjectName) ? request.DataObjectName.ToString() : "null");

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
		public IEnumerator DoneUpload(
                Request.DoneUploadRequest request,
                UnityAction<AsyncResult<Result.DoneUploadResult>> callback
        )
		{
			var task = new DoneUploadTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.DoneUploadResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.DoneUploadResult> DoneUploadAsync(
                Request.DoneUploadRequest request
        )
		{
			var task = new DoneUploadTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        private class DoneUploadByUserIdTask : Gs2RestSessionTask<DoneUploadByUserIdRequest, DoneUploadByUserIdResult>
        {
            public DoneUploadByUserIdTask(IGs2Session session, RestSessionRequestFactory factory, DoneUploadByUserIdRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DoneUploadByUserIdRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "datastore")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/{userId}/data/{dataObjectName}/done";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{dataObjectName}", !string.IsNullOrEmpty(request.DataObjectName) ? request.DataObjectName.ToString() : "null");
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
		public IEnumerator DoneUploadByUserId(
                Request.DoneUploadByUserIdRequest request,
                UnityAction<AsyncResult<Result.DoneUploadByUserIdResult>> callback
        )
		{
			var task = new DoneUploadByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.DoneUploadByUserIdResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.DoneUploadByUserIdResult> DoneUploadByUserIdAsync(
                Request.DoneUploadByUserIdRequest request
        )
		{
			var task = new DoneUploadByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        private class DeleteDataObjectTask : Gs2RestSessionTask<DeleteDataObjectRequest, DeleteDataObjectResult>
        {
            public DeleteDataObjectTask(IGs2Session session, RestSessionRequestFactory factory, DeleteDataObjectRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DeleteDataObjectRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "datastore")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/me/data/{dataObjectName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{dataObjectName}", !string.IsNullOrEmpty(request.DataObjectName) ? request.DataObjectName.ToString() : "null");

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
		public IEnumerator DeleteDataObject(
                Request.DeleteDataObjectRequest request,
                UnityAction<AsyncResult<Result.DeleteDataObjectResult>> callback
        )
		{
			var task = new DeleteDataObjectTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.DeleteDataObjectResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.DeleteDataObjectResult> DeleteDataObjectAsync(
                Request.DeleteDataObjectRequest request
        )
		{
			var task = new DeleteDataObjectTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        private class DeleteDataObjectByUserIdTask : Gs2RestSessionTask<DeleteDataObjectByUserIdRequest, DeleteDataObjectByUserIdResult>
        {
            public DeleteDataObjectByUserIdTask(IGs2Session session, RestSessionRequestFactory factory, DeleteDataObjectByUserIdRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DeleteDataObjectByUserIdRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "datastore")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/{userId}/data/{dataObjectName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{userId}", !string.IsNullOrEmpty(request.UserId) ? request.UserId.ToString() : "null");
                url = url.Replace("{dataObjectName}", !string.IsNullOrEmpty(request.DataObjectName) ? request.DataObjectName.ToString() : "null");

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
		public IEnumerator DeleteDataObjectByUserId(
                Request.DeleteDataObjectByUserIdRequest request,
                UnityAction<AsyncResult<Result.DeleteDataObjectByUserIdResult>> callback
        )
		{
			var task = new DeleteDataObjectByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.DeleteDataObjectByUserIdResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.DeleteDataObjectByUserIdResult> DeleteDataObjectByUserIdAsync(
                Request.DeleteDataObjectByUserIdRequest request
        )
		{
			var task = new DeleteDataObjectByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        private class PrepareDownloadTask : Gs2RestSessionTask<PrepareDownloadRequest, PrepareDownloadResult>
        {
            public PrepareDownloadTask(IGs2Session session, RestSessionRequestFactory factory, PrepareDownloadRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(PrepareDownloadRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "datastore")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/file";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");

                var sessionRequest = Factory.Post(url);

                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);
                jsonWriter.WriteObjectStart();
                if (request.DataObjectId != null)
                {
                    jsonWriter.WritePropertyName("dataObjectId");
                    jsonWriter.Write(request.DataObjectId);
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
		public IEnumerator PrepareDownload(
                Request.PrepareDownloadRequest request,
                UnityAction<AsyncResult<Result.PrepareDownloadResult>> callback
        )
		{
			var task = new PrepareDownloadTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.PrepareDownloadResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.PrepareDownloadResult> PrepareDownloadAsync(
                Request.PrepareDownloadRequest request
        )
		{
			var task = new PrepareDownloadTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        private class PrepareDownloadByUserIdTask : Gs2RestSessionTask<PrepareDownloadByUserIdRequest, PrepareDownloadByUserIdResult>
        {
            public PrepareDownloadByUserIdTask(IGs2Session session, RestSessionRequestFactory factory, PrepareDownloadByUserIdRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(PrepareDownloadByUserIdRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "datastore")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/{userId}/file";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{userId}", !string.IsNullOrEmpty(request.UserId) ? request.UserId.ToString() : "null");

                var sessionRequest = Factory.Post(url);

                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);
                jsonWriter.WriteObjectStart();
                if (request.DataObjectId != null)
                {
                    jsonWriter.WritePropertyName("dataObjectId");
                    jsonWriter.Write(request.DataObjectId);
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
		public IEnumerator PrepareDownloadByUserId(
                Request.PrepareDownloadByUserIdRequest request,
                UnityAction<AsyncResult<Result.PrepareDownloadByUserIdResult>> callback
        )
		{
			var task = new PrepareDownloadByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.PrepareDownloadByUserIdResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.PrepareDownloadByUserIdResult> PrepareDownloadByUserIdAsync(
                Request.PrepareDownloadByUserIdRequest request
        )
		{
			var task = new PrepareDownloadByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        private class PrepareDownloadByGenerationTask : Gs2RestSessionTask<PrepareDownloadByGenerationRequest, PrepareDownloadByGenerationResult>
        {
            public PrepareDownloadByGenerationTask(IGs2Session session, RestSessionRequestFactory factory, PrepareDownloadByGenerationRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(PrepareDownloadByGenerationRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "datastore")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/file/generation/{generation}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{generation}", !string.IsNullOrEmpty(request.Generation) ? request.Generation.ToString() : "null");

                var sessionRequest = Factory.Post(url);

                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);
                jsonWriter.WriteObjectStart();
                if (request.DataObjectId != null)
                {
                    jsonWriter.WritePropertyName("dataObjectId");
                    jsonWriter.Write(request.DataObjectId);
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
		public IEnumerator PrepareDownloadByGeneration(
                Request.PrepareDownloadByGenerationRequest request,
                UnityAction<AsyncResult<Result.PrepareDownloadByGenerationResult>> callback
        )
		{
			var task = new PrepareDownloadByGenerationTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.PrepareDownloadByGenerationResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.PrepareDownloadByGenerationResult> PrepareDownloadByGenerationAsync(
                Request.PrepareDownloadByGenerationRequest request
        )
		{
			var task = new PrepareDownloadByGenerationTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        private class PrepareDownloadByGenerationAndUserIdTask : Gs2RestSessionTask<PrepareDownloadByGenerationAndUserIdRequest, PrepareDownloadByGenerationAndUserIdResult>
        {
            public PrepareDownloadByGenerationAndUserIdTask(IGs2Session session, RestSessionRequestFactory factory, PrepareDownloadByGenerationAndUserIdRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(PrepareDownloadByGenerationAndUserIdRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "datastore")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/{userId}/file/generation/{generation}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{userId}", !string.IsNullOrEmpty(request.UserId) ? request.UserId.ToString() : "null");
                url = url.Replace("{generation}", !string.IsNullOrEmpty(request.Generation) ? request.Generation.ToString() : "null");

                var sessionRequest = Factory.Post(url);

                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);
                jsonWriter.WriteObjectStart();
                if (request.DataObjectId != null)
                {
                    jsonWriter.WritePropertyName("dataObjectId");
                    jsonWriter.Write(request.DataObjectId);
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
		public IEnumerator PrepareDownloadByGenerationAndUserId(
                Request.PrepareDownloadByGenerationAndUserIdRequest request,
                UnityAction<AsyncResult<Result.PrepareDownloadByGenerationAndUserIdResult>> callback
        )
		{
			var task = new PrepareDownloadByGenerationAndUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.PrepareDownloadByGenerationAndUserIdResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.PrepareDownloadByGenerationAndUserIdResult> PrepareDownloadByGenerationAndUserIdAsync(
                Request.PrepareDownloadByGenerationAndUserIdRequest request
        )
		{
			var task = new PrepareDownloadByGenerationAndUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        private class PrepareDownloadOwnDataTask : Gs2RestSessionTask<PrepareDownloadOwnDataRequest, PrepareDownloadOwnDataResult>
        {
            public PrepareDownloadOwnDataTask(IGs2Session session, RestSessionRequestFactory factory, PrepareDownloadOwnDataRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(PrepareDownloadOwnDataRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "datastore")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/me/data/{dataObjectName}/file";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{dataObjectName}", !string.IsNullOrEmpty(request.DataObjectName) ? request.DataObjectName.ToString() : "null");

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
		public IEnumerator PrepareDownloadOwnData(
                Request.PrepareDownloadOwnDataRequest request,
                UnityAction<AsyncResult<Result.PrepareDownloadOwnDataResult>> callback
        )
		{
			var task = new PrepareDownloadOwnDataTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.PrepareDownloadOwnDataResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.PrepareDownloadOwnDataResult> PrepareDownloadOwnDataAsync(
                Request.PrepareDownloadOwnDataRequest request
        )
		{
			var task = new PrepareDownloadOwnDataTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        private class PrepareDownloadByUserIdAndDataObjectNameTask : Gs2RestSessionTask<PrepareDownloadByUserIdAndDataObjectNameRequest, PrepareDownloadByUserIdAndDataObjectNameResult>
        {
            public PrepareDownloadByUserIdAndDataObjectNameTask(IGs2Session session, RestSessionRequestFactory factory, PrepareDownloadByUserIdAndDataObjectNameRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(PrepareDownloadByUserIdAndDataObjectNameRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "datastore")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/{userId}/data/{dataObjectName}/file";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{userId}", !string.IsNullOrEmpty(request.UserId) ? request.UserId.ToString() : "null");
                url = url.Replace("{dataObjectName}", !string.IsNullOrEmpty(request.DataObjectName) ? request.DataObjectName.ToString() : "null");

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
		public IEnumerator PrepareDownloadByUserIdAndDataObjectName(
                Request.PrepareDownloadByUserIdAndDataObjectNameRequest request,
                UnityAction<AsyncResult<Result.PrepareDownloadByUserIdAndDataObjectNameResult>> callback
        )
		{
			var task = new PrepareDownloadByUserIdAndDataObjectNameTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.PrepareDownloadByUserIdAndDataObjectNameResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.PrepareDownloadByUserIdAndDataObjectNameResult> PrepareDownloadByUserIdAndDataObjectNameAsync(
                Request.PrepareDownloadByUserIdAndDataObjectNameRequest request
        )
		{
			var task = new PrepareDownloadByUserIdAndDataObjectNameTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        private class PrepareDownloadOwnDataByGenerationTask : Gs2RestSessionTask<PrepareDownloadOwnDataByGenerationRequest, PrepareDownloadOwnDataByGenerationResult>
        {
            public PrepareDownloadOwnDataByGenerationTask(IGs2Session session, RestSessionRequestFactory factory, PrepareDownloadOwnDataByGenerationRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(PrepareDownloadOwnDataByGenerationRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "datastore")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/me/data/{dataObjectName}/generation/{generation}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{dataObjectName}", !string.IsNullOrEmpty(request.DataObjectName) ? request.DataObjectName.ToString() : "null");
                url = url.Replace("{generation}", !string.IsNullOrEmpty(request.Generation) ? request.Generation.ToString() : "null");

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
		public IEnumerator PrepareDownloadOwnDataByGeneration(
                Request.PrepareDownloadOwnDataByGenerationRequest request,
                UnityAction<AsyncResult<Result.PrepareDownloadOwnDataByGenerationResult>> callback
        )
		{
			var task = new PrepareDownloadOwnDataByGenerationTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.PrepareDownloadOwnDataByGenerationResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.PrepareDownloadOwnDataByGenerationResult> PrepareDownloadOwnDataByGenerationAsync(
                Request.PrepareDownloadOwnDataByGenerationRequest request
        )
		{
			var task = new PrepareDownloadOwnDataByGenerationTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        private class PrepareDownloadByUserIdAndDataObjectNameAndGenerationTask : Gs2RestSessionTask<PrepareDownloadByUserIdAndDataObjectNameAndGenerationRequest, PrepareDownloadByUserIdAndDataObjectNameAndGenerationResult>
        {
            public PrepareDownloadByUserIdAndDataObjectNameAndGenerationTask(IGs2Session session, RestSessionRequestFactory factory, PrepareDownloadByUserIdAndDataObjectNameAndGenerationRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(PrepareDownloadByUserIdAndDataObjectNameAndGenerationRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "datastore")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/{userId}/data/{dataObjectName}/generation/{generation}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{userId}", !string.IsNullOrEmpty(request.UserId) ? request.UserId.ToString() : "null");
                url = url.Replace("{dataObjectName}", !string.IsNullOrEmpty(request.DataObjectName) ? request.DataObjectName.ToString() : "null");
                url = url.Replace("{generation}", !string.IsNullOrEmpty(request.Generation) ? request.Generation.ToString() : "null");

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
		public IEnumerator PrepareDownloadByUserIdAndDataObjectNameAndGeneration(
                Request.PrepareDownloadByUserIdAndDataObjectNameAndGenerationRequest request,
                UnityAction<AsyncResult<Result.PrepareDownloadByUserIdAndDataObjectNameAndGenerationResult>> callback
        )
		{
			var task = new PrepareDownloadByUserIdAndDataObjectNameAndGenerationTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.PrepareDownloadByUserIdAndDataObjectNameAndGenerationResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.PrepareDownloadByUserIdAndDataObjectNameAndGenerationResult> PrepareDownloadByUserIdAndDataObjectNameAndGenerationAsync(
                Request.PrepareDownloadByUserIdAndDataObjectNameAndGenerationRequest request
        )
		{
			var task = new PrepareDownloadByUserIdAndDataObjectNameAndGenerationTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        private class RestoreDataObjectTask : Gs2RestSessionTask<RestoreDataObjectRequest, RestoreDataObjectResult>
        {
            public RestoreDataObjectTask(IGs2Session session, RestSessionRequestFactory factory, RestoreDataObjectRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(RestoreDataObjectRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "datastore")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/file/restore";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");

                var sessionRequest = Factory.Post(url);

                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);
                jsonWriter.WriteObjectStart();
                if (request.DataObjectId != null)
                {
                    jsonWriter.WritePropertyName("dataObjectId");
                    jsonWriter.Write(request.DataObjectId);
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
		public IEnumerator RestoreDataObject(
                Request.RestoreDataObjectRequest request,
                UnityAction<AsyncResult<Result.RestoreDataObjectResult>> callback
        )
		{
			var task = new RestoreDataObjectTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.RestoreDataObjectResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.RestoreDataObjectResult> RestoreDataObjectAsync(
                Request.RestoreDataObjectRequest request
        )
		{
			var task = new RestoreDataObjectTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        private class DescribeDataObjectHistoriesTask : Gs2RestSessionTask<DescribeDataObjectHistoriesRequest, DescribeDataObjectHistoriesResult>
        {
            public DescribeDataObjectHistoriesTask(IGs2Session session, RestSessionRequestFactory factory, DescribeDataObjectHistoriesRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DescribeDataObjectHistoriesRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "datastore")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/me/data/{dataObjectName}/history";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{dataObjectName}", !string.IsNullOrEmpty(request.DataObjectName) ? request.DataObjectName.ToString() : "null");

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
		public IEnumerator DescribeDataObjectHistories(
                Request.DescribeDataObjectHistoriesRequest request,
                UnityAction<AsyncResult<Result.DescribeDataObjectHistoriesResult>> callback
        )
		{
			var task = new DescribeDataObjectHistoriesTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.DescribeDataObjectHistoriesResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.DescribeDataObjectHistoriesResult> DescribeDataObjectHistoriesAsync(
                Request.DescribeDataObjectHistoriesRequest request
        )
		{
			var task = new DescribeDataObjectHistoriesTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        private class DescribeDataObjectHistoriesByUserIdTask : Gs2RestSessionTask<DescribeDataObjectHistoriesByUserIdRequest, DescribeDataObjectHistoriesByUserIdResult>
        {
            public DescribeDataObjectHistoriesByUserIdTask(IGs2Session session, RestSessionRequestFactory factory, DescribeDataObjectHistoriesByUserIdRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DescribeDataObjectHistoriesByUserIdRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "datastore")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/{userId}/data/{dataObjectName}/history";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{userId}", !string.IsNullOrEmpty(request.UserId) ? request.UserId.ToString() : "null");
                url = url.Replace("{dataObjectName}", !string.IsNullOrEmpty(request.DataObjectName) ? request.DataObjectName.ToString() : "null");

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
		public IEnumerator DescribeDataObjectHistoriesByUserId(
                Request.DescribeDataObjectHistoriesByUserIdRequest request,
                UnityAction<AsyncResult<Result.DescribeDataObjectHistoriesByUserIdResult>> callback
        )
		{
			var task = new DescribeDataObjectHistoriesByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.DescribeDataObjectHistoriesByUserIdResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.DescribeDataObjectHistoriesByUserIdResult> DescribeDataObjectHistoriesByUserIdAsync(
                Request.DescribeDataObjectHistoriesByUserIdRequest request
        )
		{
			var task = new DescribeDataObjectHistoriesByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        private class GetDataObjectHistoryTask : Gs2RestSessionTask<GetDataObjectHistoryRequest, GetDataObjectHistoryResult>
        {
            public GetDataObjectHistoryTask(IGs2Session session, RestSessionRequestFactory factory, GetDataObjectHistoryRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(GetDataObjectHistoryRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "datastore")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/me/data/{dataObjectName}/history/{generation}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{dataObjectName}", !string.IsNullOrEmpty(request.DataObjectName) ? request.DataObjectName.ToString() : "null");
                url = url.Replace("{generation}", !string.IsNullOrEmpty(request.Generation) ? request.Generation.ToString() : "null");

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
		public IEnumerator GetDataObjectHistory(
                Request.GetDataObjectHistoryRequest request,
                UnityAction<AsyncResult<Result.GetDataObjectHistoryResult>> callback
        )
		{
			var task = new GetDataObjectHistoryTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.GetDataObjectHistoryResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.GetDataObjectHistoryResult> GetDataObjectHistoryAsync(
                Request.GetDataObjectHistoryRequest request
        )
		{
			var task = new GetDataObjectHistoryTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        private class GetDataObjectHistoryByUserIdTask : Gs2RestSessionTask<GetDataObjectHistoryByUserIdRequest, GetDataObjectHistoryByUserIdResult>
        {
            public GetDataObjectHistoryByUserIdTask(IGs2Session session, RestSessionRequestFactory factory, GetDataObjectHistoryByUserIdRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(GetDataObjectHistoryByUserIdRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "datastore")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/{userId}/data/{dataObjectName}/history/{generation}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{userId}", !string.IsNullOrEmpty(request.UserId) ? request.UserId.ToString() : "null");
                url = url.Replace("{dataObjectName}", !string.IsNullOrEmpty(request.DataObjectName) ? request.DataObjectName.ToString() : "null");
                url = url.Replace("{generation}", !string.IsNullOrEmpty(request.Generation) ? request.Generation.ToString() : "null");

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
		public IEnumerator GetDataObjectHistoryByUserId(
                Request.GetDataObjectHistoryByUserIdRequest request,
                UnityAction<AsyncResult<Result.GetDataObjectHistoryByUserIdResult>> callback
        )
		{
			var task = new GetDataObjectHistoryByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.GetDataObjectHistoryByUserIdResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.GetDataObjectHistoryByUserIdResult> GetDataObjectHistoryByUserIdAsync(
                Request.GetDataObjectHistoryByUserIdRequest request
        )
		{
			var task = new GetDataObjectHistoryByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif
	}
}