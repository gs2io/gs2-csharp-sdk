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
using Gs2.Gs2Deploy.Request;
using Gs2.Gs2Deploy.Result;
using Gs2.Util.LitJson;

namespace Gs2.Gs2Deploy
{
	public class Gs2DeployRestClient : AbstractGs2Client
	{
#if UNITY_2017_1_OR_NEWER
		private readonly CertificateHandler _certificateHandler;
#endif

		public static string Endpoint = "deploy";

        protected Gs2RestSession Gs2RestSession => (Gs2RestSession) Gs2Session;

		public Gs2DeployRestClient(Gs2RestSession Gs2RestSession) : base(Gs2RestSession)
		{

		}

#if UNITY_2017_1_OR_NEWER
		public Gs2DeployRestClient(Gs2RestSession gs2RestSession, CertificateHandler certificateHandler) : base(gs2RestSession)
		{
			_certificateHandler = certificateHandler;
		}
#endif


        private class DescribeStacksTask : Gs2RestSessionTask<DescribeStacksRequest, DescribeStacksResult>
        {
            public DescribeStacksTask(IGs2Session session, RestSessionRequestFactory factory, DescribeStacksRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DescribeStacksRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "deploy")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/stack";

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
		public IEnumerator DescribeStacks(
                Request.DescribeStacksRequest request,
                UnityAction<AsyncResult<Result.DescribeStacksResult>> callback
        )
		{
			var task = new DescribeStacksTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.DescribeStacksResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.DescribeStacksResult> DescribeStacksAsync(
                Request.DescribeStacksRequest request
        )
		{
			var task = new DescribeStacksTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        private class CreateStackTask : Gs2RestSessionTask<CreateStackRequest, CreateStackResult>
        {
            public CreateStackTask(IGs2Session session, RestSessionRequestFactory factory, CreateStackRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(CreateStackRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "deploy")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/stack";

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
                if (request.Template != null)
                {
                    jsonWriter.WritePropertyName("template");
                    jsonWriter.Write(request.Template);
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
		public IEnumerator CreateStack(
                Request.CreateStackRequest request,
                UnityAction<AsyncResult<Result.CreateStackResult>> callback
        )
		{
			var task = new CreateStackTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.CreateStackResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.CreateStackResult> CreateStackAsync(
                Request.CreateStackRequest request
        )
		{
			var task = new CreateStackTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        private class CreateStackFromGitHubTask : Gs2RestSessionTask<CreateStackFromGitHubRequest, CreateStackFromGitHubResult>
        {
            public CreateStackFromGitHubTask(IGs2Session session, RestSessionRequestFactory factory, CreateStackFromGitHubRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(CreateStackFromGitHubRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "deploy")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/stack/from_git_hub";

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
		public IEnumerator CreateStackFromGitHub(
                Request.CreateStackFromGitHubRequest request,
                UnityAction<AsyncResult<Result.CreateStackFromGitHubResult>> callback
        )
		{
			var task = new CreateStackFromGitHubTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.CreateStackFromGitHubResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.CreateStackFromGitHubResult> CreateStackFromGitHubAsync(
                Request.CreateStackFromGitHubRequest request
        )
		{
			var task = new CreateStackFromGitHubTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        private class ValidateTask : Gs2RestSessionTask<ValidateRequest, ValidateResult>
        {
            public ValidateTask(IGs2Session session, RestSessionRequestFactory factory, ValidateRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(ValidateRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "deploy")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/stack/validate";

                var sessionRequest = Factory.Post(url);

                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);
                jsonWriter.WriteObjectStart();
                if (request.Template != null)
                {
                    jsonWriter.WritePropertyName("template");
                    jsonWriter.Write(request.Template);
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
		public IEnumerator Validate(
                Request.ValidateRequest request,
                UnityAction<AsyncResult<Result.ValidateResult>> callback
        )
		{
			var task = new ValidateTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.ValidateResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.ValidateResult> ValidateAsync(
                Request.ValidateRequest request
        )
		{
			var task = new ValidateTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        private class GetStackStatusTask : Gs2RestSessionTask<GetStackStatusRequest, GetStackStatusResult>
        {
            public GetStackStatusTask(IGs2Session session, RestSessionRequestFactory factory, GetStackStatusRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(GetStackStatusRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "deploy")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/stack/{stackName}/status";

                url = url.Replace("{stackName}", !string.IsNullOrEmpty(request.StackName) ? request.StackName.ToString() : "null");

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
		public IEnumerator GetStackStatus(
                Request.GetStackStatusRequest request,
                UnityAction<AsyncResult<Result.GetStackStatusResult>> callback
        )
		{
			var task = new GetStackStatusTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.GetStackStatusResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.GetStackStatusResult> GetStackStatusAsync(
                Request.GetStackStatusRequest request
        )
		{
			var task = new GetStackStatusTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        private class GetStackTask : Gs2RestSessionTask<GetStackRequest, GetStackResult>
        {
            public GetStackTask(IGs2Session session, RestSessionRequestFactory factory, GetStackRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(GetStackRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "deploy")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/stack/{stackName}";

                url = url.Replace("{stackName}", !string.IsNullOrEmpty(request.StackName) ? request.StackName.ToString() : "null");

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
		public IEnumerator GetStack(
                Request.GetStackRequest request,
                UnityAction<AsyncResult<Result.GetStackResult>> callback
        )
		{
			var task = new GetStackTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.GetStackResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.GetStackResult> GetStackAsync(
                Request.GetStackRequest request
        )
		{
			var task = new GetStackTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        private class UpdateStackTask : Gs2RestSessionTask<UpdateStackRequest, UpdateStackResult>
        {
            public UpdateStackTask(IGs2Session session, RestSessionRequestFactory factory, UpdateStackRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(UpdateStackRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "deploy")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/stack/{stackName}";

                url = url.Replace("{stackName}", !string.IsNullOrEmpty(request.StackName) ? request.StackName.ToString() : "null");

                var sessionRequest = Factory.Put(url);

                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);
                jsonWriter.WriteObjectStart();
                if (request.Description != null)
                {
                    jsonWriter.WritePropertyName("description");
                    jsonWriter.Write(request.Description);
                }
                if (request.Template != null)
                {
                    jsonWriter.WritePropertyName("template");
                    jsonWriter.Write(request.Template);
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
		public IEnumerator UpdateStack(
                Request.UpdateStackRequest request,
                UnityAction<AsyncResult<Result.UpdateStackResult>> callback
        )
		{
			var task = new UpdateStackTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.UpdateStackResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.UpdateStackResult> UpdateStackAsync(
                Request.UpdateStackRequest request
        )
		{
			var task = new UpdateStackTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        private class UpdateStackFromGitHubTask : Gs2RestSessionTask<UpdateStackFromGitHubRequest, UpdateStackFromGitHubResult>
        {
            public UpdateStackFromGitHubTask(IGs2Session session, RestSessionRequestFactory factory, UpdateStackFromGitHubRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(UpdateStackFromGitHubRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "deploy")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/stack/{stackName}/from_git_hub";

                url = url.Replace("{stackName}", !string.IsNullOrEmpty(request.StackName) ? request.StackName.ToString() : "null");

                var sessionRequest = Factory.Put(url);

                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);
                jsonWriter.WriteObjectStart();
                if (request.Description != null)
                {
                    jsonWriter.WritePropertyName("description");
                    jsonWriter.Write(request.Description);
                }
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
		public IEnumerator UpdateStackFromGitHub(
                Request.UpdateStackFromGitHubRequest request,
                UnityAction<AsyncResult<Result.UpdateStackFromGitHubResult>> callback
        )
		{
			var task = new UpdateStackFromGitHubTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.UpdateStackFromGitHubResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.UpdateStackFromGitHubResult> UpdateStackFromGitHubAsync(
                Request.UpdateStackFromGitHubRequest request
        )
		{
			var task = new UpdateStackFromGitHubTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        private class DeleteStackTask : Gs2RestSessionTask<DeleteStackRequest, DeleteStackResult>
        {
            public DeleteStackTask(IGs2Session session, RestSessionRequestFactory factory, DeleteStackRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DeleteStackRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "deploy")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/stack/{stackName}";

                url = url.Replace("{stackName}", !string.IsNullOrEmpty(request.StackName) ? request.StackName.ToString() : "null");

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
		public IEnumerator DeleteStack(
                Request.DeleteStackRequest request,
                UnityAction<AsyncResult<Result.DeleteStackResult>> callback
        )
		{
			var task = new DeleteStackTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.DeleteStackResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.DeleteStackResult> DeleteStackAsync(
                Request.DeleteStackRequest request
        )
		{
			var task = new DeleteStackTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        private class ForceDeleteStackTask : Gs2RestSessionTask<ForceDeleteStackRequest, ForceDeleteStackResult>
        {
            public ForceDeleteStackTask(IGs2Session session, RestSessionRequestFactory factory, ForceDeleteStackRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(ForceDeleteStackRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "deploy")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/stack/{stackName}/force";

                url = url.Replace("{stackName}", !string.IsNullOrEmpty(request.StackName) ? request.StackName.ToString() : "null");

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
		public IEnumerator ForceDeleteStack(
                Request.ForceDeleteStackRequest request,
                UnityAction<AsyncResult<Result.ForceDeleteStackResult>> callback
        )
		{
			var task = new ForceDeleteStackTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.ForceDeleteStackResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.ForceDeleteStackResult> ForceDeleteStackAsync(
                Request.ForceDeleteStackRequest request
        )
		{
			var task = new ForceDeleteStackTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        private class DeleteStackResourcesTask : Gs2RestSessionTask<DeleteStackResourcesRequest, DeleteStackResourcesResult>
        {
            public DeleteStackResourcesTask(IGs2Session session, RestSessionRequestFactory factory, DeleteStackResourcesRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DeleteStackResourcesRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "deploy")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/stack/{stackName}/resources";

                url = url.Replace("{stackName}", !string.IsNullOrEmpty(request.StackName) ? request.StackName.ToString() : "null");

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
		public IEnumerator DeleteStackResources(
                Request.DeleteStackResourcesRequest request,
                UnityAction<AsyncResult<Result.DeleteStackResourcesResult>> callback
        )
		{
			var task = new DeleteStackResourcesTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.DeleteStackResourcesResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.DeleteStackResourcesResult> DeleteStackResourcesAsync(
                Request.DeleteStackResourcesRequest request
        )
		{
			var task = new DeleteStackResourcesTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        private class DeleteStackEntityTask : Gs2RestSessionTask<DeleteStackEntityRequest, DeleteStackEntityResult>
        {
            public DeleteStackEntityTask(IGs2Session session, RestSessionRequestFactory factory, DeleteStackEntityRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DeleteStackEntityRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "deploy")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/stack/{stackName}/entity";

                url = url.Replace("{stackName}", !string.IsNullOrEmpty(request.StackName) ? request.StackName.ToString() : "null");

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
		public IEnumerator DeleteStackEntity(
                Request.DeleteStackEntityRequest request,
                UnityAction<AsyncResult<Result.DeleteStackEntityResult>> callback
        )
		{
			var task = new DeleteStackEntityTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.DeleteStackEntityResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.DeleteStackEntityResult> DeleteStackEntityAsync(
                Request.DeleteStackEntityRequest request
        )
		{
			var task = new DeleteStackEntityTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        private class DescribeResourcesTask : Gs2RestSessionTask<DescribeResourcesRequest, DescribeResourcesResult>
        {
            public DescribeResourcesTask(IGs2Session session, RestSessionRequestFactory factory, DescribeResourcesRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DescribeResourcesRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "deploy")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/stack/{stackName}/resource";

                url = url.Replace("{stackName}", !string.IsNullOrEmpty(request.StackName) ? request.StackName.ToString() : "null");

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
		public IEnumerator DescribeResources(
                Request.DescribeResourcesRequest request,
                UnityAction<AsyncResult<Result.DescribeResourcesResult>> callback
        )
		{
			var task = new DescribeResourcesTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.DescribeResourcesResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.DescribeResourcesResult> DescribeResourcesAsync(
                Request.DescribeResourcesRequest request
        )
		{
			var task = new DescribeResourcesTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        private class GetResourceTask : Gs2RestSessionTask<GetResourceRequest, GetResourceResult>
        {
            public GetResourceTask(IGs2Session session, RestSessionRequestFactory factory, GetResourceRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(GetResourceRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "deploy")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/stack/{stackName}/resource/{resourceName}";

                url = url.Replace("{stackName}", !string.IsNullOrEmpty(request.StackName) ? request.StackName.ToString() : "null");
                url = url.Replace("{resourceName}", !string.IsNullOrEmpty(request.ResourceName) ? request.ResourceName.ToString() : "null");

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
		public IEnumerator GetResource(
                Request.GetResourceRequest request,
                UnityAction<AsyncResult<Result.GetResourceResult>> callback
        )
		{
			var task = new GetResourceTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.GetResourceResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.GetResourceResult> GetResourceAsync(
                Request.GetResourceRequest request
        )
		{
			var task = new GetResourceTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        private class DescribeEventsTask : Gs2RestSessionTask<DescribeEventsRequest, DescribeEventsResult>
        {
            public DescribeEventsTask(IGs2Session session, RestSessionRequestFactory factory, DescribeEventsRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DescribeEventsRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "deploy")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/stack/{stackName}/event";

                url = url.Replace("{stackName}", !string.IsNullOrEmpty(request.StackName) ? request.StackName.ToString() : "null");

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
		public IEnumerator DescribeEvents(
                Request.DescribeEventsRequest request,
                UnityAction<AsyncResult<Result.DescribeEventsResult>> callback
        )
		{
			var task = new DescribeEventsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.DescribeEventsResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.DescribeEventsResult> DescribeEventsAsync(
                Request.DescribeEventsRequest request
        )
		{
			var task = new DescribeEventsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        private class GetEventTask : Gs2RestSessionTask<GetEventRequest, GetEventResult>
        {
            public GetEventTask(IGs2Session session, RestSessionRequestFactory factory, GetEventRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(GetEventRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "deploy")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/stack/{stackName}/event/{eventName}";

                url = url.Replace("{stackName}", !string.IsNullOrEmpty(request.StackName) ? request.StackName.ToString() : "null");
                url = url.Replace("{eventName}", !string.IsNullOrEmpty(request.EventName) ? request.EventName.ToString() : "null");

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
		public IEnumerator GetEvent(
                Request.GetEventRequest request,
                UnityAction<AsyncResult<Result.GetEventResult>> callback
        )
		{
			var task = new GetEventTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.GetEventResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.GetEventResult> GetEventAsync(
                Request.GetEventRequest request
        )
		{
			var task = new GetEventTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        private class DescribeOutputsTask : Gs2RestSessionTask<DescribeOutputsRequest, DescribeOutputsResult>
        {
            public DescribeOutputsTask(IGs2Session session, RestSessionRequestFactory factory, DescribeOutputsRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DescribeOutputsRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "deploy")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/stack/{stackName}/output";

                url = url.Replace("{stackName}", !string.IsNullOrEmpty(request.StackName) ? request.StackName.ToString() : "null");

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
		public IEnumerator DescribeOutputs(
                Request.DescribeOutputsRequest request,
                UnityAction<AsyncResult<Result.DescribeOutputsResult>> callback
        )
		{
			var task = new DescribeOutputsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.DescribeOutputsResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.DescribeOutputsResult> DescribeOutputsAsync(
                Request.DescribeOutputsRequest request
        )
		{
			var task = new DescribeOutputsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        private class GetOutputTask : Gs2RestSessionTask<GetOutputRequest, GetOutputResult>
        {
            public GetOutputTask(IGs2Session session, RestSessionRequestFactory factory, GetOutputRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(GetOutputRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "deploy")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/stack/{stackName}/output/{outputName}";

                url = url.Replace("{stackName}", !string.IsNullOrEmpty(request.StackName) ? request.StackName.ToString() : "null");
                url = url.Replace("{outputName}", !string.IsNullOrEmpty(request.OutputName) ? request.OutputName.ToString() : "null");

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
		public IEnumerator GetOutput(
                Request.GetOutputRequest request,
                UnityAction<AsyncResult<Result.GetOutputResult>> callback
        )
		{
			var task = new GetOutputTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.GetOutputResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.GetOutputResult> GetOutputAsync(
                Request.GetOutputRequest request
        )
		{
			var task = new GetOutputTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif
	}
}