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


        private class DescribeNamespacesTask : Gs2RestSessionTask<DescribeNamespacesRequest, DescribeNamespacesResult>
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
                    .Replace("{service}", "formation")
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
                    .Replace("{service}", "formation")
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
                    .Replace("{service}", "formation")
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


        private class DescribeFormModelMastersTask : Gs2RestSessionTask<DescribeFormModelMastersRequest, DescribeFormModelMastersResult>
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
		public IEnumerator DescribeFormModelMasters(
                Request.DescribeFormModelMastersRequest request,
                UnityAction<AsyncResult<Result.DescribeFormModelMastersResult>> callback
        )
		{
			var task = new DescribeFormModelMastersTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.DescribeFormModelMastersResult>(task.Result, task.Error));
        }
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


        private class CreateFormModelMasterTask : Gs2RestSessionTask<CreateFormModelMasterRequest, CreateFormModelMasterResult>
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
		public IEnumerator CreateFormModelMaster(
                Request.CreateFormModelMasterRequest request,
                UnityAction<AsyncResult<Result.CreateFormModelMasterResult>> callback
        )
		{
			var task = new CreateFormModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.CreateFormModelMasterResult>(task.Result, task.Error));
        }
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


        private class GetFormModelMasterTask : Gs2RestSessionTask<GetFormModelMasterRequest, GetFormModelMasterResult>
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
		public IEnumerator GetFormModelMaster(
                Request.GetFormModelMasterRequest request,
                UnityAction<AsyncResult<Result.GetFormModelMasterResult>> callback
        )
		{
			var task = new GetFormModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.GetFormModelMasterResult>(task.Result, task.Error));
        }
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


        private class UpdateFormModelMasterTask : Gs2RestSessionTask<UpdateFormModelMasterRequest, UpdateFormModelMasterResult>
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
		public IEnumerator UpdateFormModelMaster(
                Request.UpdateFormModelMasterRequest request,
                UnityAction<AsyncResult<Result.UpdateFormModelMasterResult>> callback
        )
		{
			var task = new UpdateFormModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.UpdateFormModelMasterResult>(task.Result, task.Error));
        }
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


        private class DeleteFormModelMasterTask : Gs2RestSessionTask<DeleteFormModelMasterRequest, DeleteFormModelMasterResult>
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
		public IEnumerator DeleteFormModelMaster(
                Request.DeleteFormModelMasterRequest request,
                UnityAction<AsyncResult<Result.DeleteFormModelMasterResult>> callback
        )
		{
			var task = new DeleteFormModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.DeleteFormModelMasterResult>(task.Result, task.Error));
        }
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


        private class DescribeMoldModelsTask : Gs2RestSessionTask<DescribeMoldModelsRequest, DescribeMoldModelsResult>
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
		public IEnumerator DescribeMoldModels(
                Request.DescribeMoldModelsRequest request,
                UnityAction<AsyncResult<Result.DescribeMoldModelsResult>> callback
        )
		{
			var task = new DescribeMoldModelsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.DescribeMoldModelsResult>(task.Result, task.Error));
        }
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


        private class GetMoldModelTask : Gs2RestSessionTask<GetMoldModelRequest, GetMoldModelResult>
        {
            public GetMoldModelTask(IGs2Session session, RestSessionRequestFactory factory, GetMoldModelRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(GetMoldModelRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "formation")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/model/mold/{moldName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{moldName}", !string.IsNullOrEmpty(request.MoldName) ? request.MoldName.ToString() : "null");

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
		public IEnumerator GetMoldModel(
                Request.GetMoldModelRequest request,
                UnityAction<AsyncResult<Result.GetMoldModelResult>> callback
        )
		{
			var task = new GetMoldModelTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.GetMoldModelResult>(task.Result, task.Error));
        }
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


        private class DescribeMoldModelMastersTask : Gs2RestSessionTask<DescribeMoldModelMastersRequest, DescribeMoldModelMastersResult>
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
		public IEnumerator DescribeMoldModelMasters(
                Request.DescribeMoldModelMastersRequest request,
                UnityAction<AsyncResult<Result.DescribeMoldModelMastersResult>> callback
        )
		{
			var task = new DescribeMoldModelMastersTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.DescribeMoldModelMastersResult>(task.Result, task.Error));
        }
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


        private class CreateMoldModelMasterTask : Gs2RestSessionTask<CreateMoldModelMasterRequest, CreateMoldModelMasterResult>
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
		public IEnumerator CreateMoldModelMaster(
                Request.CreateMoldModelMasterRequest request,
                UnityAction<AsyncResult<Result.CreateMoldModelMasterResult>> callback
        )
		{
			var task = new CreateMoldModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.CreateMoldModelMasterResult>(task.Result, task.Error));
        }
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


        private class GetMoldModelMasterTask : Gs2RestSessionTask<GetMoldModelMasterRequest, GetMoldModelMasterResult>
        {
            public GetMoldModelMasterTask(IGs2Session session, RestSessionRequestFactory factory, GetMoldModelMasterRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(GetMoldModelMasterRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "formation")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/master/model/mold/{moldName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{moldName}", !string.IsNullOrEmpty(request.MoldName) ? request.MoldName.ToString() : "null");

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
		public IEnumerator GetMoldModelMaster(
                Request.GetMoldModelMasterRequest request,
                UnityAction<AsyncResult<Result.GetMoldModelMasterResult>> callback
        )
		{
			var task = new GetMoldModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.GetMoldModelMasterResult>(task.Result, task.Error));
        }
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


        private class UpdateMoldModelMasterTask : Gs2RestSessionTask<UpdateMoldModelMasterRequest, UpdateMoldModelMasterResult>
        {
            public UpdateMoldModelMasterTask(IGs2Session session, RestSessionRequestFactory factory, UpdateMoldModelMasterRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(UpdateMoldModelMasterRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "formation")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/master/model/mold/{moldName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{moldName}", !string.IsNullOrEmpty(request.MoldName) ? request.MoldName.ToString() : "null");

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
		public IEnumerator UpdateMoldModelMaster(
                Request.UpdateMoldModelMasterRequest request,
                UnityAction<AsyncResult<Result.UpdateMoldModelMasterResult>> callback
        )
		{
			var task = new UpdateMoldModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.UpdateMoldModelMasterResult>(task.Result, task.Error));
        }
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


        private class DeleteMoldModelMasterTask : Gs2RestSessionTask<DeleteMoldModelMasterRequest, DeleteMoldModelMasterResult>
        {
            public DeleteMoldModelMasterTask(IGs2Session session, RestSessionRequestFactory factory, DeleteMoldModelMasterRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DeleteMoldModelMasterRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "formation")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/master/model/mold/{moldName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{moldName}", !string.IsNullOrEmpty(request.MoldName) ? request.MoldName.ToString() : "null");

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
		public IEnumerator DeleteMoldModelMaster(
                Request.DeleteMoldModelMasterRequest request,
                UnityAction<AsyncResult<Result.DeleteMoldModelMasterResult>> callback
        )
		{
			var task = new DeleteMoldModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.DeleteMoldModelMasterResult>(task.Result, task.Error));
        }
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


        private class ExportMasterTask : Gs2RestSessionTask<ExportMasterRequest, ExportMasterResult>
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


        private class GetCurrentFormMasterTask : Gs2RestSessionTask<GetCurrentFormMasterRequest, GetCurrentFormMasterResult>
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
		public IEnumerator GetCurrentFormMaster(
                Request.GetCurrentFormMasterRequest request,
                UnityAction<AsyncResult<Result.GetCurrentFormMasterResult>> callback
        )
		{
			var task = new GetCurrentFormMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.GetCurrentFormMasterResult>(task.Result, task.Error));
        }
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


        private class UpdateCurrentFormMasterTask : Gs2RestSessionTask<UpdateCurrentFormMasterRequest, UpdateCurrentFormMasterResult>
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
		public IEnumerator UpdateCurrentFormMaster(
                Request.UpdateCurrentFormMasterRequest request,
                UnityAction<AsyncResult<Result.UpdateCurrentFormMasterResult>> callback
        )
		{
			var task = new UpdateCurrentFormMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.UpdateCurrentFormMasterResult>(task.Result, task.Error));
        }
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


        private class UpdateCurrentFormMasterFromGitHubTask : Gs2RestSessionTask<UpdateCurrentFormMasterFromGitHubRequest, UpdateCurrentFormMasterFromGitHubResult>
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
		public IEnumerator UpdateCurrentFormMasterFromGitHub(
                Request.UpdateCurrentFormMasterFromGitHubRequest request,
                UnityAction<AsyncResult<Result.UpdateCurrentFormMasterFromGitHubResult>> callback
        )
		{
			var task = new UpdateCurrentFormMasterFromGitHubTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.UpdateCurrentFormMasterFromGitHubResult>(task.Result, task.Error));
        }
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


        private class DescribeMoldsTask : Gs2RestSessionTask<DescribeMoldsRequest, DescribeMoldsResult>
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
		public IEnumerator DescribeMolds(
                Request.DescribeMoldsRequest request,
                UnityAction<AsyncResult<Result.DescribeMoldsResult>> callback
        )
		{
			var task = new DescribeMoldsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.DescribeMoldsResult>(task.Result, task.Error));
        }
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


        private class DescribeMoldsByUserIdTask : Gs2RestSessionTask<DescribeMoldsByUserIdRequest, DescribeMoldsByUserIdResult>
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
		public IEnumerator DescribeMoldsByUserId(
                Request.DescribeMoldsByUserIdRequest request,
                UnityAction<AsyncResult<Result.DescribeMoldsByUserIdResult>> callback
        )
		{
			var task = new DescribeMoldsByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.DescribeMoldsByUserIdResult>(task.Result, task.Error));
        }
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


        private class GetMoldTask : Gs2RestSessionTask<GetMoldRequest, GetMoldResult>
        {
            public GetMoldTask(IGs2Session session, RestSessionRequestFactory factory, GetMoldRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(GetMoldRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "formation")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/me/mold/{moldName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{moldName}", !string.IsNullOrEmpty(request.MoldName) ? request.MoldName.ToString() : "null");

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
		public IEnumerator GetMold(
                Request.GetMoldRequest request,
                UnityAction<AsyncResult<Result.GetMoldResult>> callback
        )
		{
			var task = new GetMoldTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.GetMoldResult>(task.Result, task.Error));
        }
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


        private class GetMoldByUserIdTask : Gs2RestSessionTask<GetMoldByUserIdRequest, GetMoldByUserIdResult>
        {
            public GetMoldByUserIdTask(IGs2Session session, RestSessionRequestFactory factory, GetMoldByUserIdRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(GetMoldByUserIdRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "formation")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/{userId}/mold/{moldName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{userId}", !string.IsNullOrEmpty(request.UserId) ? request.UserId.ToString() : "null");
                url = url.Replace("{moldName}", !string.IsNullOrEmpty(request.MoldName) ? request.MoldName.ToString() : "null");

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
		public IEnumerator GetMoldByUserId(
                Request.GetMoldByUserIdRequest request,
                UnityAction<AsyncResult<Result.GetMoldByUserIdResult>> callback
        )
		{
			var task = new GetMoldByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.GetMoldByUserIdResult>(task.Result, task.Error));
        }
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


        private class SetMoldCapacityByUserIdTask : Gs2RestSessionTask<SetMoldCapacityByUserIdRequest, SetMoldCapacityByUserIdResult>
        {
            public SetMoldCapacityByUserIdTask(IGs2Session session, RestSessionRequestFactory factory, SetMoldCapacityByUserIdRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(SetMoldCapacityByUserIdRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "formation")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/{userId}/mold/{moldName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{userId}", !string.IsNullOrEmpty(request.UserId) ? request.UserId.ToString() : "null");
                url = url.Replace("{moldName}", !string.IsNullOrEmpty(request.MoldName) ? request.MoldName.ToString() : "null");

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
		public IEnumerator SetMoldCapacityByUserId(
                Request.SetMoldCapacityByUserIdRequest request,
                UnityAction<AsyncResult<Result.SetMoldCapacityByUserIdResult>> callback
        )
		{
			var task = new SetMoldCapacityByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.SetMoldCapacityByUserIdResult>(task.Result, task.Error));
        }
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


        private class AddMoldCapacityByUserIdTask : Gs2RestSessionTask<AddMoldCapacityByUserIdRequest, AddMoldCapacityByUserIdResult>
        {
            public AddMoldCapacityByUserIdTask(IGs2Session session, RestSessionRequestFactory factory, AddMoldCapacityByUserIdRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(AddMoldCapacityByUserIdRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "formation")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/{userId}/mold/{moldName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{userId}", !string.IsNullOrEmpty(request.UserId) ? request.UserId.ToString() : "null");
                url = url.Replace("{moldName}", !string.IsNullOrEmpty(request.MoldName) ? request.MoldName.ToString() : "null");

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
		public IEnumerator AddMoldCapacityByUserId(
                Request.AddMoldCapacityByUserIdRequest request,
                UnityAction<AsyncResult<Result.AddMoldCapacityByUserIdResult>> callback
        )
		{
			var task = new AddMoldCapacityByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.AddMoldCapacityByUserIdResult>(task.Result, task.Error));
        }
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


        private class DeleteMoldTask : Gs2RestSessionTask<DeleteMoldRequest, DeleteMoldResult>
        {
            public DeleteMoldTask(IGs2Session session, RestSessionRequestFactory factory, DeleteMoldRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DeleteMoldRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "formation")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/me/mold/{moldName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{moldName}", !string.IsNullOrEmpty(request.MoldName) ? request.MoldName.ToString() : "null");

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
		public IEnumerator DeleteMold(
                Request.DeleteMoldRequest request,
                UnityAction<AsyncResult<Result.DeleteMoldResult>> callback
        )
		{
			var task = new DeleteMoldTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.DeleteMoldResult>(task.Result, task.Error));
        }
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


        private class DeleteMoldByUserIdTask : Gs2RestSessionTask<DeleteMoldByUserIdRequest, DeleteMoldByUserIdResult>
        {
            public DeleteMoldByUserIdTask(IGs2Session session, RestSessionRequestFactory factory, DeleteMoldByUserIdRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DeleteMoldByUserIdRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "formation")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/{userId}/mold/{moldName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{userId}", !string.IsNullOrEmpty(request.UserId) ? request.UserId.ToString() : "null");
                url = url.Replace("{moldName}", !string.IsNullOrEmpty(request.MoldName) ? request.MoldName.ToString() : "null");

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
		public IEnumerator DeleteMoldByUserId(
                Request.DeleteMoldByUserIdRequest request,
                UnityAction<AsyncResult<Result.DeleteMoldByUserIdResult>> callback
        )
		{
			var task = new DeleteMoldByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.DeleteMoldByUserIdResult>(task.Result, task.Error));
        }
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


        private class AddCapacityByStampSheetTask : Gs2RestSessionTask<AddCapacityByStampSheetRequest, AddCapacityByStampSheetResult>
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
		public IEnumerator AddCapacityByStampSheet(
                Request.AddCapacityByStampSheetRequest request,
                UnityAction<AsyncResult<Result.AddCapacityByStampSheetResult>> callback
        )
		{
			var task = new AddCapacityByStampSheetTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.AddCapacityByStampSheetResult>(task.Result, task.Error));
        }
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


        private class SetCapacityByStampSheetTask : Gs2RestSessionTask<SetCapacityByStampSheetRequest, SetCapacityByStampSheetResult>
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
		public IEnumerator SetCapacityByStampSheet(
                Request.SetCapacityByStampSheetRequest request,
                UnityAction<AsyncResult<Result.SetCapacityByStampSheetResult>> callback
        )
		{
			var task = new SetCapacityByStampSheetTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.SetCapacityByStampSheetResult>(task.Result, task.Error));
        }
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


        private class DescribeFormsTask : Gs2RestSessionTask<DescribeFormsRequest, DescribeFormsResult>
        {
            public DescribeFormsTask(IGs2Session session, RestSessionRequestFactory factory, DescribeFormsRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DescribeFormsRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "formation")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/me/mold/{moldName}/form";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{moldName}", !string.IsNullOrEmpty(request.MoldName) ? request.MoldName.ToString() : "null");

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
		public IEnumerator DescribeForms(
                Request.DescribeFormsRequest request,
                UnityAction<AsyncResult<Result.DescribeFormsResult>> callback
        )
		{
			var task = new DescribeFormsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.DescribeFormsResult>(task.Result, task.Error));
        }
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


        private class DescribeFormsByUserIdTask : Gs2RestSessionTask<DescribeFormsByUserIdRequest, DescribeFormsByUserIdResult>
        {
            public DescribeFormsByUserIdTask(IGs2Session session, RestSessionRequestFactory factory, DescribeFormsByUserIdRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DescribeFormsByUserIdRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "formation")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/{userId}/mold/{moldName}/form";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{moldName}", !string.IsNullOrEmpty(request.MoldName) ? request.MoldName.ToString() : "null");
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
		public IEnumerator DescribeFormsByUserId(
                Request.DescribeFormsByUserIdRequest request,
                UnityAction<AsyncResult<Result.DescribeFormsByUserIdResult>> callback
        )
		{
			var task = new DescribeFormsByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.DescribeFormsByUserIdResult>(task.Result, task.Error));
        }
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


        private class GetFormTask : Gs2RestSessionTask<GetFormRequest, GetFormResult>
        {
            public GetFormTask(IGs2Session session, RestSessionRequestFactory factory, GetFormRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(GetFormRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "formation")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/me/mold/{moldName}/form/{index}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{moldName}", !string.IsNullOrEmpty(request.MoldName) ? request.MoldName.ToString() : "null");
                url = url.Replace("{index}",request.Index != null ? request.Index.ToString() : "null");

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
		public IEnumerator GetForm(
                Request.GetFormRequest request,
                UnityAction<AsyncResult<Result.GetFormResult>> callback
        )
		{
			var task = new GetFormTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.GetFormResult>(task.Result, task.Error));
        }
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


        private class GetFormByUserIdTask : Gs2RestSessionTask<GetFormByUserIdRequest, GetFormByUserIdResult>
        {
            public GetFormByUserIdTask(IGs2Session session, RestSessionRequestFactory factory, GetFormByUserIdRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(GetFormByUserIdRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "formation")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/{userId}/mold/{moldName}/form/{index}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{userId}", !string.IsNullOrEmpty(request.UserId) ? request.UserId.ToString() : "null");
                url = url.Replace("{moldName}", !string.IsNullOrEmpty(request.MoldName) ? request.MoldName.ToString() : "null");
                url = url.Replace("{index}",request.Index != null ? request.Index.ToString() : "null");

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
		public IEnumerator GetFormByUserId(
                Request.GetFormByUserIdRequest request,
                UnityAction<AsyncResult<Result.GetFormByUserIdResult>> callback
        )
		{
			var task = new GetFormByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.GetFormByUserIdResult>(task.Result, task.Error));
        }
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


        private class GetFormWithSignatureTask : Gs2RestSessionTask<GetFormWithSignatureRequest, GetFormWithSignatureResult>
        {
            public GetFormWithSignatureTask(IGs2Session session, RestSessionRequestFactory factory, GetFormWithSignatureRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(GetFormWithSignatureRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "formation")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/me/mold/{moldName}/form/{index}/signature";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{moldName}", !string.IsNullOrEmpty(request.MoldName) ? request.MoldName.ToString() : "null");
                url = url.Replace("{index}",request.Index != null ? request.Index.ToString() : "null");

                var sessionRequest = Factory.Get(url);
                if (request.ContextStack != null)
                {
                    sessionRequest.AddQueryString("contextStack", request.ContextStack);
                }
                if (request.KeyId != null) {
                    sessionRequest.AddQueryString("keyId", $"{request.KeyId}");
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
		public IEnumerator GetFormWithSignature(
                Request.GetFormWithSignatureRequest request,
                UnityAction<AsyncResult<Result.GetFormWithSignatureResult>> callback
        )
		{
			var task = new GetFormWithSignatureTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.GetFormWithSignatureResult>(task.Result, task.Error));
        }
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


        private class GetFormWithSignatureByUserIdTask : Gs2RestSessionTask<GetFormWithSignatureByUserIdRequest, GetFormWithSignatureByUserIdResult>
        {
            public GetFormWithSignatureByUserIdTask(IGs2Session session, RestSessionRequestFactory factory, GetFormWithSignatureByUserIdRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(GetFormWithSignatureByUserIdRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "formation")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/{userId}/mold/{moldName}/form/{index}/signature";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{userId}", !string.IsNullOrEmpty(request.UserId) ? request.UserId.ToString() : "null");
                url = url.Replace("{moldName}", !string.IsNullOrEmpty(request.MoldName) ? request.MoldName.ToString() : "null");
                url = url.Replace("{index}",request.Index != null ? request.Index.ToString() : "null");

                var sessionRequest = Factory.Get(url);
                if (request.ContextStack != null)
                {
                    sessionRequest.AddQueryString("contextStack", request.ContextStack);
                }
                if (request.KeyId != null) {
                    sessionRequest.AddQueryString("keyId", $"{request.KeyId}");
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
		public IEnumerator GetFormWithSignatureByUserId(
                Request.GetFormWithSignatureByUserIdRequest request,
                UnityAction<AsyncResult<Result.GetFormWithSignatureByUserIdResult>> callback
        )
		{
			var task = new GetFormWithSignatureByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.GetFormWithSignatureByUserIdResult>(task.Result, task.Error));
        }
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


        private class SetFormByUserIdTask : Gs2RestSessionTask<SetFormByUserIdRequest, SetFormByUserIdResult>
        {
            public SetFormByUserIdTask(IGs2Session session, RestSessionRequestFactory factory, SetFormByUserIdRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(SetFormByUserIdRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "formation")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/{userId}/mold/{moldName}/form/{index}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{userId}", !string.IsNullOrEmpty(request.UserId) ? request.UserId.ToString() : "null");
                url = url.Replace("{moldName}", !string.IsNullOrEmpty(request.MoldName) ? request.MoldName.ToString() : "null");
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
		public IEnumerator SetFormByUserId(
                Request.SetFormByUserIdRequest request,
                UnityAction<AsyncResult<Result.SetFormByUserIdResult>> callback
        )
		{
			var task = new SetFormByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.SetFormByUserIdResult>(task.Result, task.Error));
        }
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


        private class SetFormWithSignatureTask : Gs2RestSessionTask<SetFormWithSignatureRequest, SetFormWithSignatureResult>
        {
            public SetFormWithSignatureTask(IGs2Session session, RestSessionRequestFactory factory, SetFormWithSignatureRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(SetFormWithSignatureRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "formation")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/me/mold/{moldName}/form/{index}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{moldName}", !string.IsNullOrEmpty(request.MoldName) ? request.MoldName.ToString() : "null");
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
		public IEnumerator SetFormWithSignature(
                Request.SetFormWithSignatureRequest request,
                UnityAction<AsyncResult<Result.SetFormWithSignatureResult>> callback
        )
		{
			var task = new SetFormWithSignatureTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.SetFormWithSignatureResult>(task.Result, task.Error));
        }
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


        private class AcquireActionsToFormPropertiesTask : Gs2RestSessionTask<AcquireActionsToFormPropertiesRequest, AcquireActionsToFormPropertiesResult>
        {
            public AcquireActionsToFormPropertiesTask(IGs2Session session, RestSessionRequestFactory factory, AcquireActionsToFormPropertiesRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(AcquireActionsToFormPropertiesRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "formation")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/{userId}/mold/{moldName}/form/{index}/stamp/delegate";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{userId}", !string.IsNullOrEmpty(request.UserId) ? request.UserId.ToString() : "null");
                url = url.Replace("{moldName}", !string.IsNullOrEmpty(request.MoldName) ? request.MoldName.ToString() : "null");
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
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.AcquireActionsToFormPropertiesResult>(task.Result, task.Error));
        }
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


        private class DeleteFormTask : Gs2RestSessionTask<DeleteFormRequest, DeleteFormResult>
        {
            public DeleteFormTask(IGs2Session session, RestSessionRequestFactory factory, DeleteFormRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DeleteFormRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "formation")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/me/mold/{moldName}/form/{index}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{moldName}", !string.IsNullOrEmpty(request.MoldName) ? request.MoldName.ToString() : "null");
                url = url.Replace("{index}",request.Index != null ? request.Index.ToString() : "null");

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
		public IEnumerator DeleteForm(
                Request.DeleteFormRequest request,
                UnityAction<AsyncResult<Result.DeleteFormResult>> callback
        )
		{
			var task = new DeleteFormTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.DeleteFormResult>(task.Result, task.Error));
        }
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


        private class DeleteFormByUserIdTask : Gs2RestSessionTask<DeleteFormByUserIdRequest, DeleteFormByUserIdResult>
        {
            public DeleteFormByUserIdTask(IGs2Session session, RestSessionRequestFactory factory, DeleteFormByUserIdRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DeleteFormByUserIdRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "formation")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/{userId}/mold/{moldName}/form/{index}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{userId}", !string.IsNullOrEmpty(request.UserId) ? request.UserId.ToString() : "null");
                url = url.Replace("{moldName}", !string.IsNullOrEmpty(request.MoldName) ? request.MoldName.ToString() : "null");
                url = url.Replace("{index}",request.Index != null ? request.Index.ToString() : "null");

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
		public IEnumerator DeleteFormByUserId(
                Request.DeleteFormByUserIdRequest request,
                UnityAction<AsyncResult<Result.DeleteFormByUserIdResult>> callback
        )
		{
			var task = new DeleteFormByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.DeleteFormByUserIdResult>(task.Result, task.Error));
        }
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


        private class AcquireActionToFormPropertiesByStampSheetTask : Gs2RestSessionTask<AcquireActionToFormPropertiesByStampSheetRequest, AcquireActionToFormPropertiesByStampSheetResult>
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
		public IEnumerator AcquireActionToFormPropertiesByStampSheet(
                Request.AcquireActionToFormPropertiesByStampSheetRequest request,
                UnityAction<AsyncResult<Result.AcquireActionToFormPropertiesByStampSheetResult>> callback
        )
		{
			var task = new AcquireActionToFormPropertiesByStampSheetTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.AcquireActionToFormPropertiesByStampSheetResult>(task.Result, task.Error));
        }
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
	}
}