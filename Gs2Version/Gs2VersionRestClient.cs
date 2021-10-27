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
using Gs2.Gs2Version.Request;
using Gs2.Gs2Version.Result;
using Gs2.Util.LitJson;

namespace Gs2.Gs2Version
{
	public class Gs2VersionRestClient : AbstractGs2Client
	{
#if UNITY_2017_1_OR_NEWER
		private readonly CertificateHandler _certificateHandler;
#endif

		public static string Endpoint = "version";

        protected Gs2RestSession Gs2RestSession => (Gs2RestSession) Gs2Session;

		public Gs2VersionRestClient(Gs2RestSession Gs2RestSession) : base(Gs2RestSession)
		{

		}

#if UNITY_2017_1_OR_NEWER
		public Gs2VersionRestClient(Gs2RestSession gs2RestSession, CertificateHandler certificateHandler) : base(gs2RestSession)
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
                    .Replace("{service}", "version")
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
                    .Replace("{service}", "version")
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
                if (request.AssumeUserId != null)
                {
                    jsonWriter.WritePropertyName("assumeUserId");
                    jsonWriter.Write(request.AssumeUserId);
                }
                if (request.AcceptVersionScript != null)
                {
                    jsonWriter.WritePropertyName("acceptVersionScript");
                    request.AcceptVersionScript.WriteJson(jsonWriter);
                }
                if (request.CheckVersionTriggerScriptId != null)
                {
                    jsonWriter.WritePropertyName("checkVersionTriggerScriptId");
                    jsonWriter.Write(request.CheckVersionTriggerScriptId);
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
                    .Replace("{service}", "version")
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
                    .Replace("{service}", "version")
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
                    .Replace("{service}", "version")
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
                if (request.AssumeUserId != null)
                {
                    jsonWriter.WritePropertyName("assumeUserId");
                    jsonWriter.Write(request.AssumeUserId);
                }
                if (request.AcceptVersionScript != null)
                {
                    jsonWriter.WritePropertyName("acceptVersionScript");
                    request.AcceptVersionScript.WriteJson(jsonWriter);
                }
                if (request.CheckVersionTriggerScriptId != null)
                {
                    jsonWriter.WritePropertyName("checkVersionTriggerScriptId");
                    jsonWriter.Write(request.CheckVersionTriggerScriptId);
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
                    .Replace("{service}", "version")
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


        private class DescribeVersionModelMastersTask : Gs2RestSessionTask<DescribeVersionModelMastersRequest, DescribeVersionModelMastersResult>
        {
            public DescribeVersionModelMastersTask(IGs2Session session, RestSessionRequestFactory factory, DescribeVersionModelMastersRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DescribeVersionModelMastersRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "version")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/master/version";

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
		public IEnumerator DescribeVersionModelMasters(
                Request.DescribeVersionModelMastersRequest request,
                UnityAction<AsyncResult<Result.DescribeVersionModelMastersResult>> callback
        )
		{
			var task = new DescribeVersionModelMastersTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.DescribeVersionModelMastersResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.DescribeVersionModelMastersResult> DescribeVersionModelMastersAsync(
                Request.DescribeVersionModelMastersRequest request
        )
		{
			var task = new DescribeVersionModelMastersTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        private class CreateVersionModelMasterTask : Gs2RestSessionTask<CreateVersionModelMasterRequest, CreateVersionModelMasterResult>
        {
            public CreateVersionModelMasterTask(IGs2Session session, RestSessionRequestFactory factory, CreateVersionModelMasterRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(CreateVersionModelMasterRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "version")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/master/version";

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
                if (request.WarningVersion != null)
                {
                    jsonWriter.WritePropertyName("warningVersion");
                    request.WarningVersion.WriteJson(jsonWriter);
                }
                if (request.ErrorVersion != null)
                {
                    jsonWriter.WritePropertyName("errorVersion");
                    request.ErrorVersion.WriteJson(jsonWriter);
                }
                if (request.Scope != null)
                {
                    jsonWriter.WritePropertyName("scope");
                    jsonWriter.Write(request.Scope);
                }
                if (request.CurrentVersion != null)
                {
                    jsonWriter.WritePropertyName("currentVersion");
                    request.CurrentVersion.WriteJson(jsonWriter);
                }
                if (request.NeedSignature != null)
                {
                    jsonWriter.WritePropertyName("needSignature");
                    jsonWriter.Write(request.NeedSignature.ToString());
                }
                if (request.SignatureKeyId != null)
                {
                    jsonWriter.WritePropertyName("signatureKeyId");
                    jsonWriter.Write(request.SignatureKeyId);
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
		public IEnumerator CreateVersionModelMaster(
                Request.CreateVersionModelMasterRequest request,
                UnityAction<AsyncResult<Result.CreateVersionModelMasterResult>> callback
        )
		{
			var task = new CreateVersionModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.CreateVersionModelMasterResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.CreateVersionModelMasterResult> CreateVersionModelMasterAsync(
                Request.CreateVersionModelMasterRequest request
        )
		{
			var task = new CreateVersionModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        private class GetVersionModelMasterTask : Gs2RestSessionTask<GetVersionModelMasterRequest, GetVersionModelMasterResult>
        {
            public GetVersionModelMasterTask(IGs2Session session, RestSessionRequestFactory factory, GetVersionModelMasterRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(GetVersionModelMasterRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "version")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/master/version/{versionName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{versionName}", !string.IsNullOrEmpty(request.VersionName) ? request.VersionName.ToString() : "null");

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
		public IEnumerator GetVersionModelMaster(
                Request.GetVersionModelMasterRequest request,
                UnityAction<AsyncResult<Result.GetVersionModelMasterResult>> callback
        )
		{
			var task = new GetVersionModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.GetVersionModelMasterResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.GetVersionModelMasterResult> GetVersionModelMasterAsync(
                Request.GetVersionModelMasterRequest request
        )
		{
			var task = new GetVersionModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        private class UpdateVersionModelMasterTask : Gs2RestSessionTask<UpdateVersionModelMasterRequest, UpdateVersionModelMasterResult>
        {
            public UpdateVersionModelMasterTask(IGs2Session session, RestSessionRequestFactory factory, UpdateVersionModelMasterRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(UpdateVersionModelMasterRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "version")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/master/version/{versionName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{versionName}", !string.IsNullOrEmpty(request.VersionName) ? request.VersionName.ToString() : "null");

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
                if (request.WarningVersion != null)
                {
                    jsonWriter.WritePropertyName("warningVersion");
                    request.WarningVersion.WriteJson(jsonWriter);
                }
                if (request.ErrorVersion != null)
                {
                    jsonWriter.WritePropertyName("errorVersion");
                    request.ErrorVersion.WriteJson(jsonWriter);
                }
                if (request.Scope != null)
                {
                    jsonWriter.WritePropertyName("scope");
                    jsonWriter.Write(request.Scope);
                }
                if (request.CurrentVersion != null)
                {
                    jsonWriter.WritePropertyName("currentVersion");
                    request.CurrentVersion.WriteJson(jsonWriter);
                }
                if (request.NeedSignature != null)
                {
                    jsonWriter.WritePropertyName("needSignature");
                    jsonWriter.Write(request.NeedSignature.ToString());
                }
                if (request.SignatureKeyId != null)
                {
                    jsonWriter.WritePropertyName("signatureKeyId");
                    jsonWriter.Write(request.SignatureKeyId);
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
		public IEnumerator UpdateVersionModelMaster(
                Request.UpdateVersionModelMasterRequest request,
                UnityAction<AsyncResult<Result.UpdateVersionModelMasterResult>> callback
        )
		{
			var task = new UpdateVersionModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.UpdateVersionModelMasterResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.UpdateVersionModelMasterResult> UpdateVersionModelMasterAsync(
                Request.UpdateVersionModelMasterRequest request
        )
		{
			var task = new UpdateVersionModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        private class DeleteVersionModelMasterTask : Gs2RestSessionTask<DeleteVersionModelMasterRequest, DeleteVersionModelMasterResult>
        {
            public DeleteVersionModelMasterTask(IGs2Session session, RestSessionRequestFactory factory, DeleteVersionModelMasterRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DeleteVersionModelMasterRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "version")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/master/version/{versionName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{versionName}", !string.IsNullOrEmpty(request.VersionName) ? request.VersionName.ToString() : "null");

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
		public IEnumerator DeleteVersionModelMaster(
                Request.DeleteVersionModelMasterRequest request,
                UnityAction<AsyncResult<Result.DeleteVersionModelMasterResult>> callback
        )
		{
			var task = new DeleteVersionModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.DeleteVersionModelMasterResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.DeleteVersionModelMasterResult> DeleteVersionModelMasterAsync(
                Request.DeleteVersionModelMasterRequest request
        )
		{
			var task = new DeleteVersionModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        private class DescribeVersionModelsTask : Gs2RestSessionTask<DescribeVersionModelsRequest, DescribeVersionModelsResult>
        {
            public DescribeVersionModelsTask(IGs2Session session, RestSessionRequestFactory factory, DescribeVersionModelsRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DescribeVersionModelsRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "version")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/version";

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
		public IEnumerator DescribeVersionModels(
                Request.DescribeVersionModelsRequest request,
                UnityAction<AsyncResult<Result.DescribeVersionModelsResult>> callback
        )
		{
			var task = new DescribeVersionModelsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.DescribeVersionModelsResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.DescribeVersionModelsResult> DescribeVersionModelsAsync(
                Request.DescribeVersionModelsRequest request
        )
		{
			var task = new DescribeVersionModelsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        private class GetVersionModelTask : Gs2RestSessionTask<GetVersionModelRequest, GetVersionModelResult>
        {
            public GetVersionModelTask(IGs2Session session, RestSessionRequestFactory factory, GetVersionModelRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(GetVersionModelRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "version")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/version/{versionName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{versionName}", !string.IsNullOrEmpty(request.VersionName) ? request.VersionName.ToString() : "null");

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
		public IEnumerator GetVersionModel(
                Request.GetVersionModelRequest request,
                UnityAction<AsyncResult<Result.GetVersionModelResult>> callback
        )
		{
			var task = new GetVersionModelTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.GetVersionModelResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.GetVersionModelResult> GetVersionModelAsync(
                Request.GetVersionModelRequest request
        )
		{
			var task = new GetVersionModelTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        private class DescribeAcceptVersionsTask : Gs2RestSessionTask<DescribeAcceptVersionsRequest, DescribeAcceptVersionsResult>
        {
            public DescribeAcceptVersionsTask(IGs2Session session, RestSessionRequestFactory factory, DescribeAcceptVersionsRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DescribeAcceptVersionsRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "version")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/me/acceptVersion";

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
		public IEnumerator DescribeAcceptVersions(
                Request.DescribeAcceptVersionsRequest request,
                UnityAction<AsyncResult<Result.DescribeAcceptVersionsResult>> callback
        )
		{
			var task = new DescribeAcceptVersionsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.DescribeAcceptVersionsResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.DescribeAcceptVersionsResult> DescribeAcceptVersionsAsync(
                Request.DescribeAcceptVersionsRequest request
        )
		{
			var task = new DescribeAcceptVersionsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        private class DescribeAcceptVersionsByUserIdTask : Gs2RestSessionTask<DescribeAcceptVersionsByUserIdRequest, DescribeAcceptVersionsByUserIdResult>
        {
            public DescribeAcceptVersionsByUserIdTask(IGs2Session session, RestSessionRequestFactory factory, DescribeAcceptVersionsByUserIdRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DescribeAcceptVersionsByUserIdRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "version")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/acceptVersion";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");

                var sessionRequest = Factory.Get(url);
                if (request.ContextStack != null)
                {
                    sessionRequest.AddQueryString("contextStack", request.ContextStack);
                }
                if (request.UserId != null) {
                    sessionRequest.AddQueryString("userId", $"{request.UserId}");
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
		public IEnumerator DescribeAcceptVersionsByUserId(
                Request.DescribeAcceptVersionsByUserIdRequest request,
                UnityAction<AsyncResult<Result.DescribeAcceptVersionsByUserIdResult>> callback
        )
		{
			var task = new DescribeAcceptVersionsByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.DescribeAcceptVersionsByUserIdResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.DescribeAcceptVersionsByUserIdResult> DescribeAcceptVersionsByUserIdAsync(
                Request.DescribeAcceptVersionsByUserIdRequest request
        )
		{
			var task = new DescribeAcceptVersionsByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        private class AcceptTask : Gs2RestSessionTask<AcceptRequest, AcceptResult>
        {
            public AcceptTask(IGs2Session session, RestSessionRequestFactory factory, AcceptRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(AcceptRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "version")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/me/acceptVersion";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");

                var sessionRequest = Factory.Post(url);

                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);
                jsonWriter.WriteObjectStart();
                if (request.VersionName != null)
                {
                    jsonWriter.WritePropertyName("versionName");
                    jsonWriter.Write(request.VersionName);
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
		public IEnumerator Accept(
                Request.AcceptRequest request,
                UnityAction<AsyncResult<Result.AcceptResult>> callback
        )
		{
			var task = new AcceptTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.AcceptResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.AcceptResult> AcceptAsync(
                Request.AcceptRequest request
        )
		{
			var task = new AcceptTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        private class AcceptByUserIdTask : Gs2RestSessionTask<AcceptByUserIdRequest, AcceptByUserIdResult>
        {
            public AcceptByUserIdTask(IGs2Session session, RestSessionRequestFactory factory, AcceptByUserIdRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(AcceptByUserIdRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "version")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/{userId}/acceptVersion";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{userId}", !string.IsNullOrEmpty(request.UserId) ? request.UserId.ToString() : "null");

                var sessionRequest = Factory.Post(url);

                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);
                jsonWriter.WriteObjectStart();
                if (request.VersionName != null)
                {
                    jsonWriter.WritePropertyName("versionName");
                    jsonWriter.Write(request.VersionName);
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
		public IEnumerator AcceptByUserId(
                Request.AcceptByUserIdRequest request,
                UnityAction<AsyncResult<Result.AcceptByUserIdResult>> callback
        )
		{
			var task = new AcceptByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.AcceptByUserIdResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.AcceptByUserIdResult> AcceptByUserIdAsync(
                Request.AcceptByUserIdRequest request
        )
		{
			var task = new AcceptByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        private class GetAcceptVersionTask : Gs2RestSessionTask<GetAcceptVersionRequest, GetAcceptVersionResult>
        {
            public GetAcceptVersionTask(IGs2Session session, RestSessionRequestFactory factory, GetAcceptVersionRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(GetAcceptVersionRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "version")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/me/{versionName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{versionName}", !string.IsNullOrEmpty(request.VersionName) ? request.VersionName.ToString() : "null");

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
		public IEnumerator GetAcceptVersion(
                Request.GetAcceptVersionRequest request,
                UnityAction<AsyncResult<Result.GetAcceptVersionResult>> callback
        )
		{
			var task = new GetAcceptVersionTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.GetAcceptVersionResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.GetAcceptVersionResult> GetAcceptVersionAsync(
                Request.GetAcceptVersionRequest request
        )
		{
			var task = new GetAcceptVersionTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        private class GetAcceptVersionByUserIdTask : Gs2RestSessionTask<GetAcceptVersionByUserIdRequest, GetAcceptVersionByUserIdResult>
        {
            public GetAcceptVersionByUserIdTask(IGs2Session session, RestSessionRequestFactory factory, GetAcceptVersionByUserIdRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(GetAcceptVersionByUserIdRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "version")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/{userId}/{versionName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{userId}", !string.IsNullOrEmpty(request.UserId) ? request.UserId.ToString() : "null");
                url = url.Replace("{versionName}", !string.IsNullOrEmpty(request.VersionName) ? request.VersionName.ToString() : "null");

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
		public IEnumerator GetAcceptVersionByUserId(
                Request.GetAcceptVersionByUserIdRequest request,
                UnityAction<AsyncResult<Result.GetAcceptVersionByUserIdResult>> callback
        )
		{
			var task = new GetAcceptVersionByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.GetAcceptVersionByUserIdResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.GetAcceptVersionByUserIdResult> GetAcceptVersionByUserIdAsync(
                Request.GetAcceptVersionByUserIdRequest request
        )
		{
			var task = new GetAcceptVersionByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        private class DeleteAcceptVersionTask : Gs2RestSessionTask<DeleteAcceptVersionRequest, DeleteAcceptVersionResult>
        {
            public DeleteAcceptVersionTask(IGs2Session session, RestSessionRequestFactory factory, DeleteAcceptVersionRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DeleteAcceptVersionRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "version")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/me/{versionName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{versionName}", !string.IsNullOrEmpty(request.VersionName) ? request.VersionName.ToString() : "null");

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
		public IEnumerator DeleteAcceptVersion(
                Request.DeleteAcceptVersionRequest request,
                UnityAction<AsyncResult<Result.DeleteAcceptVersionResult>> callback
        )
		{
			var task = new DeleteAcceptVersionTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.DeleteAcceptVersionResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.DeleteAcceptVersionResult> DeleteAcceptVersionAsync(
                Request.DeleteAcceptVersionRequest request
        )
		{
			var task = new DeleteAcceptVersionTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        private class DeleteAcceptVersionByUserIdTask : Gs2RestSessionTask<DeleteAcceptVersionByUserIdRequest, DeleteAcceptVersionByUserIdResult>
        {
            public DeleteAcceptVersionByUserIdTask(IGs2Session session, RestSessionRequestFactory factory, DeleteAcceptVersionByUserIdRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DeleteAcceptVersionByUserIdRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "version")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/{userId}/{versionName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{userId}", !string.IsNullOrEmpty(request.UserId) ? request.UserId.ToString() : "null");
                url = url.Replace("{versionName}", !string.IsNullOrEmpty(request.VersionName) ? request.VersionName.ToString() : "null");

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
		public IEnumerator DeleteAcceptVersionByUserId(
                Request.DeleteAcceptVersionByUserIdRequest request,
                UnityAction<AsyncResult<Result.DeleteAcceptVersionByUserIdResult>> callback
        )
		{
			var task = new DeleteAcceptVersionByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.DeleteAcceptVersionByUserIdResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.DeleteAcceptVersionByUserIdResult> DeleteAcceptVersionByUserIdAsync(
                Request.DeleteAcceptVersionByUserIdRequest request
        )
		{
			var task = new DeleteAcceptVersionByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        private class CheckVersionTask : Gs2RestSessionTask<CheckVersionRequest, CheckVersionResult>
        {
            public CheckVersionTask(IGs2Session session, RestSessionRequestFactory factory, CheckVersionRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(CheckVersionRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "version")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/me/check";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");

                var sessionRequest = Factory.Post(url);

                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);
                jsonWriter.WriteObjectStart();
                if (request.TargetVersions != null)
                {
                    jsonWriter.WritePropertyName("targetVersions");
                    jsonWriter.WriteArrayStart();
                    foreach(var item in request.TargetVersions)
                    {
                        if (item == null) {
                            jsonWriter.Write(null);
                        } else {
                            item.WriteJson(jsonWriter);
                        }
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
		public IEnumerator CheckVersion(
                Request.CheckVersionRequest request,
                UnityAction<AsyncResult<Result.CheckVersionResult>> callback
        )
		{
			var task = new CheckVersionTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.CheckVersionResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.CheckVersionResult> CheckVersionAsync(
                Request.CheckVersionRequest request
        )
		{
			var task = new CheckVersionTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        private class CheckVersionByUserIdTask : Gs2RestSessionTask<CheckVersionByUserIdRequest, CheckVersionByUserIdResult>
        {
            public CheckVersionByUserIdTask(IGs2Session session, RestSessionRequestFactory factory, CheckVersionByUserIdRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(CheckVersionByUserIdRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "version")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/{userId}/check";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{userId}", !string.IsNullOrEmpty(request.UserId) ? request.UserId.ToString() : "null");

                var sessionRequest = Factory.Post(url);

                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);
                jsonWriter.WriteObjectStart();
                if (request.TargetVersions != null)
                {
                    jsonWriter.WritePropertyName("targetVersions");
                    jsonWriter.WriteArrayStart();
                    foreach(var item in request.TargetVersions)
                    {
                        if (item == null) {
                            jsonWriter.Write(null);
                        } else {
                            item.WriteJson(jsonWriter);
                        }
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
		public IEnumerator CheckVersionByUserId(
                Request.CheckVersionByUserIdRequest request,
                UnityAction<AsyncResult<Result.CheckVersionByUserIdResult>> callback
        )
		{
			var task = new CheckVersionByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.CheckVersionByUserIdResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.CheckVersionByUserIdResult> CheckVersionByUserIdAsync(
                Request.CheckVersionByUserIdRequest request
        )
		{
			var task = new CheckVersionByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        private class CalculateSignatureTask : Gs2RestSessionTask<CalculateSignatureRequest, CalculateSignatureResult>
        {
            public CalculateSignatureTask(IGs2Session session, RestSessionRequestFactory factory, CalculateSignatureRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(CalculateSignatureRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "version")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/version/{versionName}/calculate/signature";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{versionName}", !string.IsNullOrEmpty(request.VersionName) ? request.VersionName.ToString() : "null");

                var sessionRequest = Factory.Post(url);

                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);
                jsonWriter.WriteObjectStart();
                if (request.Version != null)
                {
                    jsonWriter.WritePropertyName("version");
                    request.Version.WriteJson(jsonWriter);
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
		public IEnumerator CalculateSignature(
                Request.CalculateSignatureRequest request,
                UnityAction<AsyncResult<Result.CalculateSignatureResult>> callback
        )
		{
			var task = new CalculateSignatureTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.CalculateSignatureResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.CalculateSignatureResult> CalculateSignatureAsync(
                Request.CalculateSignatureRequest request
        )
		{
			var task = new CalculateSignatureTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        private class ExportMasterTask : Gs2RestSessionTask<ExportMasterRequest, ExportMasterResult>
        {
            public ExportMasterTask(IGs2Session session, RestSessionRequestFactory factory, ExportMasterRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(ExportMasterRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "version")
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
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.ExportMasterResult>(task.Result, task.Error));
        }
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


        private class GetCurrentVersionMasterTask : Gs2RestSessionTask<GetCurrentVersionMasterRequest, GetCurrentVersionMasterResult>
        {
            public GetCurrentVersionMasterTask(IGs2Session session, RestSessionRequestFactory factory, GetCurrentVersionMasterRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(GetCurrentVersionMasterRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "version")
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
		public IEnumerator GetCurrentVersionMaster(
                Request.GetCurrentVersionMasterRequest request,
                UnityAction<AsyncResult<Result.GetCurrentVersionMasterResult>> callback
        )
		{
			var task = new GetCurrentVersionMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.GetCurrentVersionMasterResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.GetCurrentVersionMasterResult> GetCurrentVersionMasterAsync(
                Request.GetCurrentVersionMasterRequest request
        )
		{
			var task = new GetCurrentVersionMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        private class UpdateCurrentVersionMasterTask : Gs2RestSessionTask<UpdateCurrentVersionMasterRequest, UpdateCurrentVersionMasterResult>
        {
            public UpdateCurrentVersionMasterTask(IGs2Session session, RestSessionRequestFactory factory, UpdateCurrentVersionMasterRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(UpdateCurrentVersionMasterRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "version")
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
		public IEnumerator UpdateCurrentVersionMaster(
                Request.UpdateCurrentVersionMasterRequest request,
                UnityAction<AsyncResult<Result.UpdateCurrentVersionMasterResult>> callback
        )
		{
			var task = new UpdateCurrentVersionMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.UpdateCurrentVersionMasterResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.UpdateCurrentVersionMasterResult> UpdateCurrentVersionMasterAsync(
                Request.UpdateCurrentVersionMasterRequest request
        )
		{
			var task = new UpdateCurrentVersionMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        private class UpdateCurrentVersionMasterFromGitHubTask : Gs2RestSessionTask<UpdateCurrentVersionMasterFromGitHubRequest, UpdateCurrentVersionMasterFromGitHubResult>
        {
            public UpdateCurrentVersionMasterFromGitHubTask(IGs2Session session, RestSessionRequestFactory factory, UpdateCurrentVersionMasterFromGitHubRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(UpdateCurrentVersionMasterFromGitHubRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "version")
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
		public IEnumerator UpdateCurrentVersionMasterFromGitHub(
                Request.UpdateCurrentVersionMasterFromGitHubRequest request,
                UnityAction<AsyncResult<Result.UpdateCurrentVersionMasterFromGitHubResult>> callback
        )
		{
			var task = new UpdateCurrentVersionMasterFromGitHubTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.UpdateCurrentVersionMasterFromGitHubResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.UpdateCurrentVersionMasterFromGitHubResult> UpdateCurrentVersionMasterFromGitHubAsync(
                Request.UpdateCurrentVersionMasterFromGitHubRequest request
        )
		{
			var task = new UpdateCurrentVersionMasterFromGitHubTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif
	}
}