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
using Gs2.Gs2Experience.Request;
using Gs2.Gs2Experience.Result;
using Gs2.Util.LitJson;

namespace Gs2.Gs2Experience
{
	public class Gs2ExperienceRestClient : AbstractGs2Client
	{
#if UNITY_2017_1_OR_NEWER
		private readonly CertificateHandler _certificateHandler;
#endif

		public static string Endpoint = "experience";

        protected Gs2RestSession Gs2RestSession => (Gs2RestSession) Gs2Session;

		public Gs2ExperienceRestClient(Gs2RestSession Gs2RestSession) : base(Gs2RestSession)
		{

		}

#if UNITY_2017_1_OR_NEWER
		public Gs2ExperienceRestClient(Gs2RestSession gs2RestSession, CertificateHandler certificateHandler) : base(gs2RestSession)
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
                    .Replace("{service}", "experience")
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
                    .Replace("{service}", "experience")
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
                if (request.ExperienceCapScriptId != null)
                {
                    jsonWriter.WritePropertyName("experienceCapScriptId");
                    jsonWriter.Write(request.ExperienceCapScriptId);
                }
                if (request.ChangeExperienceScript != null)
                {
                    jsonWriter.WritePropertyName("changeExperienceScript");
                    request.ChangeExperienceScript.WriteJson(jsonWriter);
                }
                if (request.ChangeRankScript != null)
                {
                    jsonWriter.WritePropertyName("changeRankScript");
                    request.ChangeRankScript.WriteJson(jsonWriter);
                }
                if (request.ChangeRankCapScript != null)
                {
                    jsonWriter.WritePropertyName("changeRankCapScript");
                    request.ChangeRankCapScript.WriteJson(jsonWriter);
                }
                if (request.OverflowExperienceScript != null)
                {
                    jsonWriter.WritePropertyName("overflowExperienceScript");
                    request.OverflowExperienceScript.WriteJson(jsonWriter);
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
                    .Replace("{service}", "experience")
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
                    .Replace("{service}", "experience")
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
                    .Replace("{service}", "experience")
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
                if (request.ExperienceCapScriptId != null)
                {
                    jsonWriter.WritePropertyName("experienceCapScriptId");
                    jsonWriter.Write(request.ExperienceCapScriptId);
                }
                if (request.ChangeExperienceScript != null)
                {
                    jsonWriter.WritePropertyName("changeExperienceScript");
                    request.ChangeExperienceScript.WriteJson(jsonWriter);
                }
                if (request.ChangeRankScript != null)
                {
                    jsonWriter.WritePropertyName("changeRankScript");
                    request.ChangeRankScript.WriteJson(jsonWriter);
                }
                if (request.ChangeRankCapScript != null)
                {
                    jsonWriter.WritePropertyName("changeRankCapScript");
                    request.ChangeRankCapScript.WriteJson(jsonWriter);
                }
                if (request.OverflowExperienceScript != null)
                {
                    jsonWriter.WritePropertyName("overflowExperienceScript");
                    request.OverflowExperienceScript.WriteJson(jsonWriter);
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
                    .Replace("{service}", "experience")
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


        private class DescribeExperienceModelMastersTask : Gs2RestSessionTask<DescribeExperienceModelMastersRequest, DescribeExperienceModelMastersResult>
        {
            public DescribeExperienceModelMastersTask(IGs2Session session, RestSessionRequestFactory factory, DescribeExperienceModelMastersRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DescribeExperienceModelMastersRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "experience")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/master/model";

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
		public IEnumerator DescribeExperienceModelMasters(
                Request.DescribeExperienceModelMastersRequest request,
                UnityAction<AsyncResult<Result.DescribeExperienceModelMastersResult>> callback
        )
		{
			var task = new DescribeExperienceModelMastersTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.DescribeExperienceModelMastersResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.DescribeExperienceModelMastersResult> DescribeExperienceModelMastersAsync(
                Request.DescribeExperienceModelMastersRequest request
        )
		{
			var task = new DescribeExperienceModelMastersTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        private class CreateExperienceModelMasterTask : Gs2RestSessionTask<CreateExperienceModelMasterRequest, CreateExperienceModelMasterResult>
        {
            public CreateExperienceModelMasterTask(IGs2Session session, RestSessionRequestFactory factory, CreateExperienceModelMasterRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(CreateExperienceModelMasterRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "experience")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/master/model";

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
                if (request.DefaultExperience != null)
                {
                    jsonWriter.WritePropertyName("defaultExperience");
                    jsonWriter.Write(request.DefaultExperience.ToString());
                }
                if (request.DefaultRankCap != null)
                {
                    jsonWriter.WritePropertyName("defaultRankCap");
                    jsonWriter.Write(request.DefaultRankCap.ToString());
                }
                if (request.MaxRankCap != null)
                {
                    jsonWriter.WritePropertyName("maxRankCap");
                    jsonWriter.Write(request.MaxRankCap.ToString());
                }
                if (request.RankThresholdName != null)
                {
                    jsonWriter.WritePropertyName("rankThresholdName");
                    jsonWriter.Write(request.RankThresholdName);
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
		public IEnumerator CreateExperienceModelMaster(
                Request.CreateExperienceModelMasterRequest request,
                UnityAction<AsyncResult<Result.CreateExperienceModelMasterResult>> callback
        )
		{
			var task = new CreateExperienceModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.CreateExperienceModelMasterResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.CreateExperienceModelMasterResult> CreateExperienceModelMasterAsync(
                Request.CreateExperienceModelMasterRequest request
        )
		{
			var task = new CreateExperienceModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        private class GetExperienceModelMasterTask : Gs2RestSessionTask<GetExperienceModelMasterRequest, GetExperienceModelMasterResult>
        {
            public GetExperienceModelMasterTask(IGs2Session session, RestSessionRequestFactory factory, GetExperienceModelMasterRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(GetExperienceModelMasterRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "experience")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/master/model/{experienceName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{experienceName}", !string.IsNullOrEmpty(request.ExperienceName) ? request.ExperienceName.ToString() : "null");

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
		public IEnumerator GetExperienceModelMaster(
                Request.GetExperienceModelMasterRequest request,
                UnityAction<AsyncResult<Result.GetExperienceModelMasterResult>> callback
        )
		{
			var task = new GetExperienceModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.GetExperienceModelMasterResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.GetExperienceModelMasterResult> GetExperienceModelMasterAsync(
                Request.GetExperienceModelMasterRequest request
        )
		{
			var task = new GetExperienceModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        private class UpdateExperienceModelMasterTask : Gs2RestSessionTask<UpdateExperienceModelMasterRequest, UpdateExperienceModelMasterResult>
        {
            public UpdateExperienceModelMasterTask(IGs2Session session, RestSessionRequestFactory factory, UpdateExperienceModelMasterRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(UpdateExperienceModelMasterRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "experience")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/master/model/{experienceName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{experienceName}", !string.IsNullOrEmpty(request.ExperienceName) ? request.ExperienceName.ToString() : "null");

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
                if (request.DefaultExperience != null)
                {
                    jsonWriter.WritePropertyName("defaultExperience");
                    jsonWriter.Write(request.DefaultExperience.ToString());
                }
                if (request.DefaultRankCap != null)
                {
                    jsonWriter.WritePropertyName("defaultRankCap");
                    jsonWriter.Write(request.DefaultRankCap.ToString());
                }
                if (request.MaxRankCap != null)
                {
                    jsonWriter.WritePropertyName("maxRankCap");
                    jsonWriter.Write(request.MaxRankCap.ToString());
                }
                if (request.RankThresholdName != null)
                {
                    jsonWriter.WritePropertyName("rankThresholdName");
                    jsonWriter.Write(request.RankThresholdName);
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
		public IEnumerator UpdateExperienceModelMaster(
                Request.UpdateExperienceModelMasterRequest request,
                UnityAction<AsyncResult<Result.UpdateExperienceModelMasterResult>> callback
        )
		{
			var task = new UpdateExperienceModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.UpdateExperienceModelMasterResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.UpdateExperienceModelMasterResult> UpdateExperienceModelMasterAsync(
                Request.UpdateExperienceModelMasterRequest request
        )
		{
			var task = new UpdateExperienceModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        private class DeleteExperienceModelMasterTask : Gs2RestSessionTask<DeleteExperienceModelMasterRequest, DeleteExperienceModelMasterResult>
        {
            public DeleteExperienceModelMasterTask(IGs2Session session, RestSessionRequestFactory factory, DeleteExperienceModelMasterRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DeleteExperienceModelMasterRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "experience")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/master/model/{experienceName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{experienceName}", !string.IsNullOrEmpty(request.ExperienceName) ? request.ExperienceName.ToString() : "null");

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
		public IEnumerator DeleteExperienceModelMaster(
                Request.DeleteExperienceModelMasterRequest request,
                UnityAction<AsyncResult<Result.DeleteExperienceModelMasterResult>> callback
        )
		{
			var task = new DeleteExperienceModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.DeleteExperienceModelMasterResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.DeleteExperienceModelMasterResult> DeleteExperienceModelMasterAsync(
                Request.DeleteExperienceModelMasterRequest request
        )
		{
			var task = new DeleteExperienceModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        private class DescribeExperienceModelsTask : Gs2RestSessionTask<DescribeExperienceModelsRequest, DescribeExperienceModelsResult>
        {
            public DescribeExperienceModelsTask(IGs2Session session, RestSessionRequestFactory factory, DescribeExperienceModelsRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DescribeExperienceModelsRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "experience")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/model";

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
		public IEnumerator DescribeExperienceModels(
                Request.DescribeExperienceModelsRequest request,
                UnityAction<AsyncResult<Result.DescribeExperienceModelsResult>> callback
        )
		{
			var task = new DescribeExperienceModelsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.DescribeExperienceModelsResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.DescribeExperienceModelsResult> DescribeExperienceModelsAsync(
                Request.DescribeExperienceModelsRequest request
        )
		{
			var task = new DescribeExperienceModelsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        private class GetExperienceModelTask : Gs2RestSessionTask<GetExperienceModelRequest, GetExperienceModelResult>
        {
            public GetExperienceModelTask(IGs2Session session, RestSessionRequestFactory factory, GetExperienceModelRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(GetExperienceModelRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "experience")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/model/{experienceName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{experienceName}", !string.IsNullOrEmpty(request.ExperienceName) ? request.ExperienceName.ToString() : "null");

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
		public IEnumerator GetExperienceModel(
                Request.GetExperienceModelRequest request,
                UnityAction<AsyncResult<Result.GetExperienceModelResult>> callback
        )
		{
			var task = new GetExperienceModelTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.GetExperienceModelResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.GetExperienceModelResult> GetExperienceModelAsync(
                Request.GetExperienceModelRequest request
        )
		{
			var task = new GetExperienceModelTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        private class DescribeThresholdMastersTask : Gs2RestSessionTask<DescribeThresholdMastersRequest, DescribeThresholdMastersResult>
        {
            public DescribeThresholdMastersTask(IGs2Session session, RestSessionRequestFactory factory, DescribeThresholdMastersRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DescribeThresholdMastersRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "experience")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/master/threshold";

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
		public IEnumerator DescribeThresholdMasters(
                Request.DescribeThresholdMastersRequest request,
                UnityAction<AsyncResult<Result.DescribeThresholdMastersResult>> callback
        )
		{
			var task = new DescribeThresholdMastersTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.DescribeThresholdMastersResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.DescribeThresholdMastersResult> DescribeThresholdMastersAsync(
                Request.DescribeThresholdMastersRequest request
        )
		{
			var task = new DescribeThresholdMastersTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        private class CreateThresholdMasterTask : Gs2RestSessionTask<CreateThresholdMasterRequest, CreateThresholdMasterResult>
        {
            public CreateThresholdMasterTask(IGs2Session session, RestSessionRequestFactory factory, CreateThresholdMasterRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(CreateThresholdMasterRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "experience")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/master/threshold";

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
                if (request.Values != null)
                {
                    jsonWriter.WritePropertyName("values");
                    jsonWriter.WriteArrayStart();
                    foreach(var item in request.Values)
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
		public IEnumerator CreateThresholdMaster(
                Request.CreateThresholdMasterRequest request,
                UnityAction<AsyncResult<Result.CreateThresholdMasterResult>> callback
        )
		{
			var task = new CreateThresholdMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.CreateThresholdMasterResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.CreateThresholdMasterResult> CreateThresholdMasterAsync(
                Request.CreateThresholdMasterRequest request
        )
		{
			var task = new CreateThresholdMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        private class GetThresholdMasterTask : Gs2RestSessionTask<GetThresholdMasterRequest, GetThresholdMasterResult>
        {
            public GetThresholdMasterTask(IGs2Session session, RestSessionRequestFactory factory, GetThresholdMasterRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(GetThresholdMasterRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "experience")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/master/threshold/{thresholdName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{thresholdName}", !string.IsNullOrEmpty(request.ThresholdName) ? request.ThresholdName.ToString() : "null");

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
		public IEnumerator GetThresholdMaster(
                Request.GetThresholdMasterRequest request,
                UnityAction<AsyncResult<Result.GetThresholdMasterResult>> callback
        )
		{
			var task = new GetThresholdMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.GetThresholdMasterResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.GetThresholdMasterResult> GetThresholdMasterAsync(
                Request.GetThresholdMasterRequest request
        )
		{
			var task = new GetThresholdMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        private class UpdateThresholdMasterTask : Gs2RestSessionTask<UpdateThresholdMasterRequest, UpdateThresholdMasterResult>
        {
            public UpdateThresholdMasterTask(IGs2Session session, RestSessionRequestFactory factory, UpdateThresholdMasterRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(UpdateThresholdMasterRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "experience")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/master/threshold/{thresholdName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{thresholdName}", !string.IsNullOrEmpty(request.ThresholdName) ? request.ThresholdName.ToString() : "null");

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
                if (request.Values != null)
                {
                    jsonWriter.WritePropertyName("values");
                    jsonWriter.WriteArrayStart();
                    foreach(var item in request.Values)
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
		public IEnumerator UpdateThresholdMaster(
                Request.UpdateThresholdMasterRequest request,
                UnityAction<AsyncResult<Result.UpdateThresholdMasterResult>> callback
        )
		{
			var task = new UpdateThresholdMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.UpdateThresholdMasterResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.UpdateThresholdMasterResult> UpdateThresholdMasterAsync(
                Request.UpdateThresholdMasterRequest request
        )
		{
			var task = new UpdateThresholdMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        private class DeleteThresholdMasterTask : Gs2RestSessionTask<DeleteThresholdMasterRequest, DeleteThresholdMasterResult>
        {
            public DeleteThresholdMasterTask(IGs2Session session, RestSessionRequestFactory factory, DeleteThresholdMasterRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DeleteThresholdMasterRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "experience")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/master/threshold/{thresholdName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{thresholdName}", !string.IsNullOrEmpty(request.ThresholdName) ? request.ThresholdName.ToString() : "null");

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
		public IEnumerator DeleteThresholdMaster(
                Request.DeleteThresholdMasterRequest request,
                UnityAction<AsyncResult<Result.DeleteThresholdMasterResult>> callback
        )
		{
			var task = new DeleteThresholdMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.DeleteThresholdMasterResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.DeleteThresholdMasterResult> DeleteThresholdMasterAsync(
                Request.DeleteThresholdMasterRequest request
        )
		{
			var task = new DeleteThresholdMasterTask(
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
                    .Replace("{service}", "experience")
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


        private class GetCurrentExperienceMasterTask : Gs2RestSessionTask<GetCurrentExperienceMasterRequest, GetCurrentExperienceMasterResult>
        {
            public GetCurrentExperienceMasterTask(IGs2Session session, RestSessionRequestFactory factory, GetCurrentExperienceMasterRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(GetCurrentExperienceMasterRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "experience")
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
		public IEnumerator GetCurrentExperienceMaster(
                Request.GetCurrentExperienceMasterRequest request,
                UnityAction<AsyncResult<Result.GetCurrentExperienceMasterResult>> callback
        )
		{
			var task = new GetCurrentExperienceMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.GetCurrentExperienceMasterResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.GetCurrentExperienceMasterResult> GetCurrentExperienceMasterAsync(
                Request.GetCurrentExperienceMasterRequest request
        )
		{
			var task = new GetCurrentExperienceMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        private class UpdateCurrentExperienceMasterTask : Gs2RestSessionTask<UpdateCurrentExperienceMasterRequest, UpdateCurrentExperienceMasterResult>
        {
            public UpdateCurrentExperienceMasterTask(IGs2Session session, RestSessionRequestFactory factory, UpdateCurrentExperienceMasterRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(UpdateCurrentExperienceMasterRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "experience")
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
		public IEnumerator UpdateCurrentExperienceMaster(
                Request.UpdateCurrentExperienceMasterRequest request,
                UnityAction<AsyncResult<Result.UpdateCurrentExperienceMasterResult>> callback
        )
		{
			var task = new UpdateCurrentExperienceMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.UpdateCurrentExperienceMasterResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.UpdateCurrentExperienceMasterResult> UpdateCurrentExperienceMasterAsync(
                Request.UpdateCurrentExperienceMasterRequest request
        )
		{
			var task = new UpdateCurrentExperienceMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        private class UpdateCurrentExperienceMasterFromGitHubTask : Gs2RestSessionTask<UpdateCurrentExperienceMasterFromGitHubRequest, UpdateCurrentExperienceMasterFromGitHubResult>
        {
            public UpdateCurrentExperienceMasterFromGitHubTask(IGs2Session session, RestSessionRequestFactory factory, UpdateCurrentExperienceMasterFromGitHubRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(UpdateCurrentExperienceMasterFromGitHubRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "experience")
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
		public IEnumerator UpdateCurrentExperienceMasterFromGitHub(
                Request.UpdateCurrentExperienceMasterFromGitHubRequest request,
                UnityAction<AsyncResult<Result.UpdateCurrentExperienceMasterFromGitHubResult>> callback
        )
		{
			var task = new UpdateCurrentExperienceMasterFromGitHubTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.UpdateCurrentExperienceMasterFromGitHubResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.UpdateCurrentExperienceMasterFromGitHubResult> UpdateCurrentExperienceMasterFromGitHubAsync(
                Request.UpdateCurrentExperienceMasterFromGitHubRequest request
        )
		{
			var task = new UpdateCurrentExperienceMasterFromGitHubTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        private class DescribeStatusesTask : Gs2RestSessionTask<DescribeStatusesRequest, DescribeStatusesResult>
        {
            public DescribeStatusesTask(IGs2Session session, RestSessionRequestFactory factory, DescribeStatusesRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DescribeStatusesRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "experience")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/me/status";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");

                var sessionRequest = Factory.Get(url);
                if (request.ContextStack != null)
                {
                    sessionRequest.AddQueryString("contextStack", request.ContextStack);
                }
                if (request.ExperienceName != null) {
                    sessionRequest.AddQueryString("experienceName", $"{request.ExperienceName}");
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
		public IEnumerator DescribeStatuses(
                Request.DescribeStatusesRequest request,
                UnityAction<AsyncResult<Result.DescribeStatusesResult>> callback
        )
		{
			var task = new DescribeStatusesTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.DescribeStatusesResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.DescribeStatusesResult> DescribeStatusesAsync(
                Request.DescribeStatusesRequest request
        )
		{
			var task = new DescribeStatusesTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        private class DescribeStatusesByUserIdTask : Gs2RestSessionTask<DescribeStatusesByUserIdRequest, DescribeStatusesByUserIdResult>
        {
            public DescribeStatusesByUserIdTask(IGs2Session session, RestSessionRequestFactory factory, DescribeStatusesByUserIdRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DescribeStatusesByUserIdRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "experience")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/{userId}/status";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{userId}", !string.IsNullOrEmpty(request.UserId) ? request.UserId.ToString() : "null");

                var sessionRequest = Factory.Get(url);
                if (request.ContextStack != null)
                {
                    sessionRequest.AddQueryString("contextStack", request.ContextStack);
                }
                if (request.ExperienceName != null) {
                    sessionRequest.AddQueryString("experienceName", $"{request.ExperienceName}");
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
		public IEnumerator DescribeStatusesByUserId(
                Request.DescribeStatusesByUserIdRequest request,
                UnityAction<AsyncResult<Result.DescribeStatusesByUserIdResult>> callback
        )
		{
			var task = new DescribeStatusesByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.DescribeStatusesByUserIdResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.DescribeStatusesByUserIdResult> DescribeStatusesByUserIdAsync(
                Request.DescribeStatusesByUserIdRequest request
        )
		{
			var task = new DescribeStatusesByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        private class GetStatusTask : Gs2RestSessionTask<GetStatusRequest, GetStatusResult>
        {
            public GetStatusTask(IGs2Session session, RestSessionRequestFactory factory, GetStatusRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(GetStatusRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "experience")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/me/status/model/{experienceName}/property/{propertyId}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{experienceName}", !string.IsNullOrEmpty(request.ExperienceName) ? request.ExperienceName.ToString() : "null");
                url = url.Replace("{propertyId}", !string.IsNullOrEmpty(request.PropertyId) ? request.PropertyId.ToString() : "null");

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
		public IEnumerator GetStatus(
                Request.GetStatusRequest request,
                UnityAction<AsyncResult<Result.GetStatusResult>> callback
        )
		{
			var task = new GetStatusTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.GetStatusResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.GetStatusResult> GetStatusAsync(
                Request.GetStatusRequest request
        )
		{
			var task = new GetStatusTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        private class GetStatusByUserIdTask : Gs2RestSessionTask<GetStatusByUserIdRequest, GetStatusByUserIdResult>
        {
            public GetStatusByUserIdTask(IGs2Session session, RestSessionRequestFactory factory, GetStatusByUserIdRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(GetStatusByUserIdRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "experience")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/{userId}/status/model/{experienceName}/property/{propertyId}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{userId}", !string.IsNullOrEmpty(request.UserId) ? request.UserId.ToString() : "null");
                url = url.Replace("{experienceName}", !string.IsNullOrEmpty(request.ExperienceName) ? request.ExperienceName.ToString() : "null");
                url = url.Replace("{propertyId}", !string.IsNullOrEmpty(request.PropertyId) ? request.PropertyId.ToString() : "null");

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
		public IEnumerator GetStatusByUserId(
                Request.GetStatusByUserIdRequest request,
                UnityAction<AsyncResult<Result.GetStatusByUserIdResult>> callback
        )
		{
			var task = new GetStatusByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.GetStatusByUserIdResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.GetStatusByUserIdResult> GetStatusByUserIdAsync(
                Request.GetStatusByUserIdRequest request
        )
		{
			var task = new GetStatusByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        private class GetStatusWithSignatureTask : Gs2RestSessionTask<GetStatusWithSignatureRequest, GetStatusWithSignatureResult>
        {
            public GetStatusWithSignatureTask(IGs2Session session, RestSessionRequestFactory factory, GetStatusWithSignatureRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(GetStatusWithSignatureRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "experience")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/me/status/model/{experienceName}/property/{propertyId}/signature";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{experienceName}", !string.IsNullOrEmpty(request.ExperienceName) ? request.ExperienceName.ToString() : "null");
                url = url.Replace("{propertyId}", !string.IsNullOrEmpty(request.PropertyId) ? request.PropertyId.ToString() : "null");

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
		public IEnumerator GetStatusWithSignature(
                Request.GetStatusWithSignatureRequest request,
                UnityAction<AsyncResult<Result.GetStatusWithSignatureResult>> callback
        )
		{
			var task = new GetStatusWithSignatureTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.GetStatusWithSignatureResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.GetStatusWithSignatureResult> GetStatusWithSignatureAsync(
                Request.GetStatusWithSignatureRequest request
        )
		{
			var task = new GetStatusWithSignatureTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        private class GetStatusWithSignatureByUserIdTask : Gs2RestSessionTask<GetStatusWithSignatureByUserIdRequest, GetStatusWithSignatureByUserIdResult>
        {
            public GetStatusWithSignatureByUserIdTask(IGs2Session session, RestSessionRequestFactory factory, GetStatusWithSignatureByUserIdRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(GetStatusWithSignatureByUserIdRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "experience")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/{userId}/status/model/{experienceName}/property/{propertyId}/signature";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{userId}", !string.IsNullOrEmpty(request.UserId) ? request.UserId.ToString() : "null");
                url = url.Replace("{experienceName}", !string.IsNullOrEmpty(request.ExperienceName) ? request.ExperienceName.ToString() : "null");
                url = url.Replace("{propertyId}", !string.IsNullOrEmpty(request.PropertyId) ? request.PropertyId.ToString() : "null");

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
		public IEnumerator GetStatusWithSignatureByUserId(
                Request.GetStatusWithSignatureByUserIdRequest request,
                UnityAction<AsyncResult<Result.GetStatusWithSignatureByUserIdResult>> callback
        )
		{
			var task = new GetStatusWithSignatureByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.GetStatusWithSignatureByUserIdResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.GetStatusWithSignatureByUserIdResult> GetStatusWithSignatureByUserIdAsync(
                Request.GetStatusWithSignatureByUserIdRequest request
        )
		{
			var task = new GetStatusWithSignatureByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        private class AddExperienceByUserIdTask : Gs2RestSessionTask<AddExperienceByUserIdRequest, AddExperienceByUserIdResult>
        {
            public AddExperienceByUserIdTask(IGs2Session session, RestSessionRequestFactory factory, AddExperienceByUserIdRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(AddExperienceByUserIdRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "experience")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/{userId}/status/model/{experienceName}/property/{propertyId}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{userId}", !string.IsNullOrEmpty(request.UserId) ? request.UserId.ToString() : "null");
                url = url.Replace("{experienceName}", !string.IsNullOrEmpty(request.ExperienceName) ? request.ExperienceName.ToString() : "null");
                url = url.Replace("{propertyId}", !string.IsNullOrEmpty(request.PropertyId) ? request.PropertyId.ToString() : "null");

                var sessionRequest = Factory.Post(url);

                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);
                jsonWriter.WriteObjectStart();
                if (request.ExperienceValue != null)
                {
                    jsonWriter.WritePropertyName("experienceValue");
                    jsonWriter.Write(request.ExperienceValue.ToString());
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
		public IEnumerator AddExperienceByUserId(
                Request.AddExperienceByUserIdRequest request,
                UnityAction<AsyncResult<Result.AddExperienceByUserIdResult>> callback
        )
		{
			var task = new AddExperienceByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.AddExperienceByUserIdResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.AddExperienceByUserIdResult> AddExperienceByUserIdAsync(
                Request.AddExperienceByUserIdRequest request
        )
		{
			var task = new AddExperienceByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        private class SetExperienceByUserIdTask : Gs2RestSessionTask<SetExperienceByUserIdRequest, SetExperienceByUserIdResult>
        {
            public SetExperienceByUserIdTask(IGs2Session session, RestSessionRequestFactory factory, SetExperienceByUserIdRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(SetExperienceByUserIdRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "experience")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/{userId}/status/model/{experienceName}/property/{propertyId}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{userId}", !string.IsNullOrEmpty(request.UserId) ? request.UserId.ToString() : "null");
                url = url.Replace("{experienceName}", !string.IsNullOrEmpty(request.ExperienceName) ? request.ExperienceName.ToString() : "null");
                url = url.Replace("{propertyId}", !string.IsNullOrEmpty(request.PropertyId) ? request.PropertyId.ToString() : "null");

                var sessionRequest = Factory.Put(url);

                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);
                jsonWriter.WriteObjectStart();
                if (request.ExperienceValue != null)
                {
                    jsonWriter.WritePropertyName("experienceValue");
                    jsonWriter.Write(request.ExperienceValue.ToString());
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
		public IEnumerator SetExperienceByUserId(
                Request.SetExperienceByUserIdRequest request,
                UnityAction<AsyncResult<Result.SetExperienceByUserIdResult>> callback
        )
		{
			var task = new SetExperienceByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.SetExperienceByUserIdResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.SetExperienceByUserIdResult> SetExperienceByUserIdAsync(
                Request.SetExperienceByUserIdRequest request
        )
		{
			var task = new SetExperienceByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        private class AddRankCapByUserIdTask : Gs2RestSessionTask<AddRankCapByUserIdRequest, AddRankCapByUserIdResult>
        {
            public AddRankCapByUserIdTask(IGs2Session session, RestSessionRequestFactory factory, AddRankCapByUserIdRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(AddRankCapByUserIdRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "experience")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/{userId}/status/model/{experienceName}/property/{propertyId}/cap";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{userId}", !string.IsNullOrEmpty(request.UserId) ? request.UserId.ToString() : "null");
                url = url.Replace("{experienceName}", !string.IsNullOrEmpty(request.ExperienceName) ? request.ExperienceName.ToString() : "null");
                url = url.Replace("{propertyId}", !string.IsNullOrEmpty(request.PropertyId) ? request.PropertyId.ToString() : "null");

                var sessionRequest = Factory.Post(url);

                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);
                jsonWriter.WriteObjectStart();
                if (request.RankCapValue != null)
                {
                    jsonWriter.WritePropertyName("rankCapValue");
                    jsonWriter.Write(request.RankCapValue.ToString());
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
		public IEnumerator AddRankCapByUserId(
                Request.AddRankCapByUserIdRequest request,
                UnityAction<AsyncResult<Result.AddRankCapByUserIdResult>> callback
        )
		{
			var task = new AddRankCapByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.AddRankCapByUserIdResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.AddRankCapByUserIdResult> AddRankCapByUserIdAsync(
                Request.AddRankCapByUserIdRequest request
        )
		{
			var task = new AddRankCapByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        private class SetRankCapByUserIdTask : Gs2RestSessionTask<SetRankCapByUserIdRequest, SetRankCapByUserIdResult>
        {
            public SetRankCapByUserIdTask(IGs2Session session, RestSessionRequestFactory factory, SetRankCapByUserIdRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(SetRankCapByUserIdRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "experience")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/{userId}/status/model/{experienceName}/property/{propertyId}/cap";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{userId}", !string.IsNullOrEmpty(request.UserId) ? request.UserId.ToString() : "null");
                url = url.Replace("{experienceName}", !string.IsNullOrEmpty(request.ExperienceName) ? request.ExperienceName.ToString() : "null");
                url = url.Replace("{propertyId}", !string.IsNullOrEmpty(request.PropertyId) ? request.PropertyId.ToString() : "null");

                var sessionRequest = Factory.Put(url);

                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);
                jsonWriter.WriteObjectStart();
                if (request.RankCapValue != null)
                {
                    jsonWriter.WritePropertyName("rankCapValue");
                    jsonWriter.Write(request.RankCapValue.ToString());
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
		public IEnumerator SetRankCapByUserId(
                Request.SetRankCapByUserIdRequest request,
                UnityAction<AsyncResult<Result.SetRankCapByUserIdResult>> callback
        )
		{
			var task = new SetRankCapByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.SetRankCapByUserIdResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.SetRankCapByUserIdResult> SetRankCapByUserIdAsync(
                Request.SetRankCapByUserIdRequest request
        )
		{
			var task = new SetRankCapByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        private class DeleteStatusByUserIdTask : Gs2RestSessionTask<DeleteStatusByUserIdRequest, DeleteStatusByUserIdResult>
        {
            public DeleteStatusByUserIdTask(IGs2Session session, RestSessionRequestFactory factory, DeleteStatusByUserIdRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DeleteStatusByUserIdRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "experience")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/{userId}/status/model/{experienceName}/property/{propertyId}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{userId}", !string.IsNullOrEmpty(request.UserId) ? request.UserId.ToString() : "null");
                url = url.Replace("{experienceName}", !string.IsNullOrEmpty(request.ExperienceName) ? request.ExperienceName.ToString() : "null");
                url = url.Replace("{propertyId}", !string.IsNullOrEmpty(request.PropertyId) ? request.PropertyId.ToString() : "null");

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
		public IEnumerator DeleteStatusByUserId(
                Request.DeleteStatusByUserIdRequest request,
                UnityAction<AsyncResult<Result.DeleteStatusByUserIdResult>> callback
        )
		{
			var task = new DeleteStatusByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.DeleteStatusByUserIdResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.DeleteStatusByUserIdResult> DeleteStatusByUserIdAsync(
                Request.DeleteStatusByUserIdRequest request
        )
		{
			var task = new DeleteStatusByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        private class AddExperienceByStampSheetTask : Gs2RestSessionTask<AddExperienceByStampSheetRequest, AddExperienceByStampSheetResult>
        {
            public AddExperienceByStampSheetTask(IGs2Session session, RestSessionRequestFactory factory, AddExperienceByStampSheetRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(AddExperienceByStampSheetRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "experience")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/stamp/experience/add";

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
		public IEnumerator AddExperienceByStampSheet(
                Request.AddExperienceByStampSheetRequest request,
                UnityAction<AsyncResult<Result.AddExperienceByStampSheetResult>> callback
        )
		{
			var task = new AddExperienceByStampSheetTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.AddExperienceByStampSheetResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.AddExperienceByStampSheetResult> AddExperienceByStampSheetAsync(
                Request.AddExperienceByStampSheetRequest request
        )
		{
			var task = new AddExperienceByStampSheetTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        private class AddRankCapByStampSheetTask : Gs2RestSessionTask<AddRankCapByStampSheetRequest, AddRankCapByStampSheetResult>
        {
            public AddRankCapByStampSheetTask(IGs2Session session, RestSessionRequestFactory factory, AddRankCapByStampSheetRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(AddRankCapByStampSheetRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "experience")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/stamp/rankCap/add";

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
		public IEnumerator AddRankCapByStampSheet(
                Request.AddRankCapByStampSheetRequest request,
                UnityAction<AsyncResult<Result.AddRankCapByStampSheetResult>> callback
        )
		{
			var task = new AddRankCapByStampSheetTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.AddRankCapByStampSheetResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.AddRankCapByStampSheetResult> AddRankCapByStampSheetAsync(
                Request.AddRankCapByStampSheetRequest request
        )
		{
			var task = new AddRankCapByStampSheetTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        private class SetRankCapByStampSheetTask : Gs2RestSessionTask<SetRankCapByStampSheetRequest, SetRankCapByStampSheetResult>
        {
            public SetRankCapByStampSheetTask(IGs2Session session, RestSessionRequestFactory factory, SetRankCapByStampSheetRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(SetRankCapByStampSheetRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "experience")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/stamp/rankCap/set";

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
		public IEnumerator SetRankCapByStampSheet(
                Request.SetRankCapByStampSheetRequest request,
                UnityAction<AsyncResult<Result.SetRankCapByStampSheetResult>> callback
        )
		{
			var task = new SetRankCapByStampSheetTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.SetRankCapByStampSheetResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.SetRankCapByStampSheetResult> SetRankCapByStampSheetAsync(
                Request.SetRankCapByStampSheetRequest request
        )
		{
			var task = new SetRankCapByStampSheetTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif
	}
}