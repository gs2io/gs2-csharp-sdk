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
using Gs2.Gs2Quest.Request;
using Gs2.Gs2Quest.Result;
using Gs2.Util.LitJson;

namespace Gs2.Gs2Quest
{
	public class Gs2QuestRestClient : AbstractGs2Client
	{
#if UNITY_2017_1_OR_NEWER
		private readonly CertificateHandler _certificateHandler;
#endif

		public static string Endpoint = "quest";

        protected Gs2RestSession Gs2RestSession => (Gs2RestSession) Gs2Session;

		public Gs2QuestRestClient(Gs2RestSession Gs2RestSession) : base(Gs2RestSession)
		{

		}

#if UNITY_2017_1_OR_NEWER
		public Gs2QuestRestClient(Gs2RestSession gs2RestSession, CertificateHandler certificateHandler) : base(gs2RestSession)
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
                    .Replace("{service}", "quest")
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
                    .Replace("{service}", "quest")
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
                if (request.StartQuestScript != null)
                {
                    jsonWriter.WritePropertyName("startQuestScript");
                    request.StartQuestScript.WriteJson(jsonWriter);
                }
                if (request.CompleteQuestScript != null)
                {
                    jsonWriter.WritePropertyName("completeQuestScript");
                    request.CompleteQuestScript.WriteJson(jsonWriter);
                }
                if (request.FailedQuestScript != null)
                {
                    jsonWriter.WritePropertyName("failedQuestScript");
                    request.FailedQuestScript.WriteJson(jsonWriter);
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
                    .Replace("{service}", "quest")
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
                    .Replace("{service}", "quest")
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
                    .Replace("{service}", "quest")
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
                if (request.StartQuestScript != null)
                {
                    jsonWriter.WritePropertyName("startQuestScript");
                    request.StartQuestScript.WriteJson(jsonWriter);
                }
                if (request.CompleteQuestScript != null)
                {
                    jsonWriter.WritePropertyName("completeQuestScript");
                    request.CompleteQuestScript.WriteJson(jsonWriter);
                }
                if (request.FailedQuestScript != null)
                {
                    jsonWriter.WritePropertyName("failedQuestScript");
                    request.FailedQuestScript.WriteJson(jsonWriter);
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
                    .Replace("{service}", "quest")
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


        private class DescribeQuestGroupModelMastersTask : Gs2RestSessionTask<DescribeQuestGroupModelMastersRequest, DescribeQuestGroupModelMastersResult>
        {
            public DescribeQuestGroupModelMastersTask(IGs2Session session, RestSessionRequestFactory factory, DescribeQuestGroupModelMastersRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DescribeQuestGroupModelMastersRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "quest")
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
		public IEnumerator DescribeQuestGroupModelMasters(
                Request.DescribeQuestGroupModelMastersRequest request,
                UnityAction<AsyncResult<Result.DescribeQuestGroupModelMastersResult>> callback
        )
		{
			var task = new DescribeQuestGroupModelMastersTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.DescribeQuestGroupModelMastersResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.DescribeQuestGroupModelMastersResult> DescribeQuestGroupModelMastersAsync(
                Request.DescribeQuestGroupModelMastersRequest request
        )
		{
			var task = new DescribeQuestGroupModelMastersTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        private class CreateQuestGroupModelMasterTask : Gs2RestSessionTask<CreateQuestGroupModelMasterRequest, CreateQuestGroupModelMasterResult>
        {
            public CreateQuestGroupModelMasterTask(IGs2Session session, RestSessionRequestFactory factory, CreateQuestGroupModelMasterRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(CreateQuestGroupModelMasterRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "quest")
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
                if (request.ChallengePeriodEventId != null)
                {
                    jsonWriter.WritePropertyName("challengePeriodEventId");
                    jsonWriter.Write(request.ChallengePeriodEventId);
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
		public IEnumerator CreateQuestGroupModelMaster(
                Request.CreateQuestGroupModelMasterRequest request,
                UnityAction<AsyncResult<Result.CreateQuestGroupModelMasterResult>> callback
        )
		{
			var task = new CreateQuestGroupModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.CreateQuestGroupModelMasterResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.CreateQuestGroupModelMasterResult> CreateQuestGroupModelMasterAsync(
                Request.CreateQuestGroupModelMasterRequest request
        )
		{
			var task = new CreateQuestGroupModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        private class GetQuestGroupModelMasterTask : Gs2RestSessionTask<GetQuestGroupModelMasterRequest, GetQuestGroupModelMasterResult>
        {
            public GetQuestGroupModelMasterTask(IGs2Session session, RestSessionRequestFactory factory, GetQuestGroupModelMasterRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(GetQuestGroupModelMasterRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "quest")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/master/group/{questGroupName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{questGroupName}", !string.IsNullOrEmpty(request.QuestGroupName) ? request.QuestGroupName.ToString() : "null");

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
		public IEnumerator GetQuestGroupModelMaster(
                Request.GetQuestGroupModelMasterRequest request,
                UnityAction<AsyncResult<Result.GetQuestGroupModelMasterResult>> callback
        )
		{
			var task = new GetQuestGroupModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.GetQuestGroupModelMasterResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.GetQuestGroupModelMasterResult> GetQuestGroupModelMasterAsync(
                Request.GetQuestGroupModelMasterRequest request
        )
		{
			var task = new GetQuestGroupModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        private class UpdateQuestGroupModelMasterTask : Gs2RestSessionTask<UpdateQuestGroupModelMasterRequest, UpdateQuestGroupModelMasterResult>
        {
            public UpdateQuestGroupModelMasterTask(IGs2Session session, RestSessionRequestFactory factory, UpdateQuestGroupModelMasterRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(UpdateQuestGroupModelMasterRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "quest")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/master/group/{questGroupName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{questGroupName}", !string.IsNullOrEmpty(request.QuestGroupName) ? request.QuestGroupName.ToString() : "null");

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
                if (request.ChallengePeriodEventId != null)
                {
                    jsonWriter.WritePropertyName("challengePeriodEventId");
                    jsonWriter.Write(request.ChallengePeriodEventId);
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
		public IEnumerator UpdateQuestGroupModelMaster(
                Request.UpdateQuestGroupModelMasterRequest request,
                UnityAction<AsyncResult<Result.UpdateQuestGroupModelMasterResult>> callback
        )
		{
			var task = new UpdateQuestGroupModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.UpdateQuestGroupModelMasterResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.UpdateQuestGroupModelMasterResult> UpdateQuestGroupModelMasterAsync(
                Request.UpdateQuestGroupModelMasterRequest request
        )
		{
			var task = new UpdateQuestGroupModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        private class DeleteQuestGroupModelMasterTask : Gs2RestSessionTask<DeleteQuestGroupModelMasterRequest, DeleteQuestGroupModelMasterResult>
        {
            public DeleteQuestGroupModelMasterTask(IGs2Session session, RestSessionRequestFactory factory, DeleteQuestGroupModelMasterRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DeleteQuestGroupModelMasterRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "quest")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/master/group/{questGroupName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{questGroupName}", !string.IsNullOrEmpty(request.QuestGroupName) ? request.QuestGroupName.ToString() : "null");

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
		public IEnumerator DeleteQuestGroupModelMaster(
                Request.DeleteQuestGroupModelMasterRequest request,
                UnityAction<AsyncResult<Result.DeleteQuestGroupModelMasterResult>> callback
        )
		{
			var task = new DeleteQuestGroupModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.DeleteQuestGroupModelMasterResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.DeleteQuestGroupModelMasterResult> DeleteQuestGroupModelMasterAsync(
                Request.DeleteQuestGroupModelMasterRequest request
        )
		{
			var task = new DeleteQuestGroupModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        private class DescribeQuestModelMastersTask : Gs2RestSessionTask<DescribeQuestModelMastersRequest, DescribeQuestModelMastersResult>
        {
            public DescribeQuestModelMastersTask(IGs2Session session, RestSessionRequestFactory factory, DescribeQuestModelMastersRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DescribeQuestModelMastersRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "quest")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/master/group/{questGroupName}/quest";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{questGroupName}", !string.IsNullOrEmpty(request.QuestGroupName) ? request.QuestGroupName.ToString() : "null");

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
		public IEnumerator DescribeQuestModelMasters(
                Request.DescribeQuestModelMastersRequest request,
                UnityAction<AsyncResult<Result.DescribeQuestModelMastersResult>> callback
        )
		{
			var task = new DescribeQuestModelMastersTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.DescribeQuestModelMastersResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.DescribeQuestModelMastersResult> DescribeQuestModelMastersAsync(
                Request.DescribeQuestModelMastersRequest request
        )
		{
			var task = new DescribeQuestModelMastersTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        private class CreateQuestModelMasterTask : Gs2RestSessionTask<CreateQuestModelMasterRequest, CreateQuestModelMasterResult>
        {
            public CreateQuestModelMasterTask(IGs2Session session, RestSessionRequestFactory factory, CreateQuestModelMasterRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(CreateQuestModelMasterRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "quest")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/master/group/{questGroupName}/quest";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{questGroupName}", !string.IsNullOrEmpty(request.QuestGroupName) ? request.QuestGroupName.ToString() : "null");

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
                if (request.Contents != null)
                {
                    jsonWriter.WritePropertyName("contents");
                    jsonWriter.WriteArrayStart();
                    foreach(var item in request.Contents)
                    {
                        item.WriteJson(jsonWriter);
                    }
                    jsonWriter.WriteArrayEnd();
                }
                if (request.ChallengePeriodEventId != null)
                {
                    jsonWriter.WritePropertyName("challengePeriodEventId");
                    jsonWriter.Write(request.ChallengePeriodEventId);
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
                if (request.FailedAcquireActions != null)
                {
                    jsonWriter.WritePropertyName("failedAcquireActions");
                    jsonWriter.WriteArrayStart();
                    foreach(var item in request.FailedAcquireActions)
                    {
                        item.WriteJson(jsonWriter);
                    }
                    jsonWriter.WriteArrayEnd();
                }
                if (request.PremiseQuestNames != null)
                {
                    jsonWriter.WritePropertyName("premiseQuestNames");
                    jsonWriter.WriteArrayStart();
                    foreach(var item in request.PremiseQuestNames)
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
		public IEnumerator CreateQuestModelMaster(
                Request.CreateQuestModelMasterRequest request,
                UnityAction<AsyncResult<Result.CreateQuestModelMasterResult>> callback
        )
		{
			var task = new CreateQuestModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.CreateQuestModelMasterResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.CreateQuestModelMasterResult> CreateQuestModelMasterAsync(
                Request.CreateQuestModelMasterRequest request
        )
		{
			var task = new CreateQuestModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        private class GetQuestModelMasterTask : Gs2RestSessionTask<GetQuestModelMasterRequest, GetQuestModelMasterResult>
        {
            public GetQuestModelMasterTask(IGs2Session session, RestSessionRequestFactory factory, GetQuestModelMasterRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(GetQuestModelMasterRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "quest")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/master/group/{questGroupName}/quest/{questName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{questGroupName}", !string.IsNullOrEmpty(request.QuestGroupName) ? request.QuestGroupName.ToString() : "null");
                url = url.Replace("{questName}", !string.IsNullOrEmpty(request.QuestName) ? request.QuestName.ToString() : "null");

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
		public IEnumerator GetQuestModelMaster(
                Request.GetQuestModelMasterRequest request,
                UnityAction<AsyncResult<Result.GetQuestModelMasterResult>> callback
        )
		{
			var task = new GetQuestModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.GetQuestModelMasterResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.GetQuestModelMasterResult> GetQuestModelMasterAsync(
                Request.GetQuestModelMasterRequest request
        )
		{
			var task = new GetQuestModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        private class UpdateQuestModelMasterTask : Gs2RestSessionTask<UpdateQuestModelMasterRequest, UpdateQuestModelMasterResult>
        {
            public UpdateQuestModelMasterTask(IGs2Session session, RestSessionRequestFactory factory, UpdateQuestModelMasterRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(UpdateQuestModelMasterRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "quest")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/master/group/{questGroupName}/quest/{questName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{questGroupName}", !string.IsNullOrEmpty(request.QuestGroupName) ? request.QuestGroupName.ToString() : "null");
                url = url.Replace("{questName}", !string.IsNullOrEmpty(request.QuestName) ? request.QuestName.ToString() : "null");

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
                if (request.Contents != null)
                {
                    jsonWriter.WritePropertyName("contents");
                    jsonWriter.WriteArrayStart();
                    foreach(var item in request.Contents)
                    {
                        item.WriteJson(jsonWriter);
                    }
                    jsonWriter.WriteArrayEnd();
                }
                if (request.ChallengePeriodEventId != null)
                {
                    jsonWriter.WritePropertyName("challengePeriodEventId");
                    jsonWriter.Write(request.ChallengePeriodEventId);
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
                if (request.FailedAcquireActions != null)
                {
                    jsonWriter.WritePropertyName("failedAcquireActions");
                    jsonWriter.WriteArrayStart();
                    foreach(var item in request.FailedAcquireActions)
                    {
                        item.WriteJson(jsonWriter);
                    }
                    jsonWriter.WriteArrayEnd();
                }
                if (request.PremiseQuestNames != null)
                {
                    jsonWriter.WritePropertyName("premiseQuestNames");
                    jsonWriter.WriteArrayStart();
                    foreach(var item in request.PremiseQuestNames)
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
		public IEnumerator UpdateQuestModelMaster(
                Request.UpdateQuestModelMasterRequest request,
                UnityAction<AsyncResult<Result.UpdateQuestModelMasterResult>> callback
        )
		{
			var task = new UpdateQuestModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.UpdateQuestModelMasterResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.UpdateQuestModelMasterResult> UpdateQuestModelMasterAsync(
                Request.UpdateQuestModelMasterRequest request
        )
		{
			var task = new UpdateQuestModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        private class DeleteQuestModelMasterTask : Gs2RestSessionTask<DeleteQuestModelMasterRequest, DeleteQuestModelMasterResult>
        {
            public DeleteQuestModelMasterTask(IGs2Session session, RestSessionRequestFactory factory, DeleteQuestModelMasterRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DeleteQuestModelMasterRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "quest")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/master/group/{questGroupName}/quest/{questName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{questGroupName}", !string.IsNullOrEmpty(request.QuestGroupName) ? request.QuestGroupName.ToString() : "null");
                url = url.Replace("{questName}", !string.IsNullOrEmpty(request.QuestName) ? request.QuestName.ToString() : "null");

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
		public IEnumerator DeleteQuestModelMaster(
                Request.DeleteQuestModelMasterRequest request,
                UnityAction<AsyncResult<Result.DeleteQuestModelMasterResult>> callback
        )
		{
			var task = new DeleteQuestModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.DeleteQuestModelMasterResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.DeleteQuestModelMasterResult> DeleteQuestModelMasterAsync(
                Request.DeleteQuestModelMasterRequest request
        )
		{
			var task = new DeleteQuestModelMasterTask(
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
                    .Replace("{service}", "quest")
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


        private class GetCurrentQuestMasterTask : Gs2RestSessionTask<GetCurrentQuestMasterRequest, GetCurrentQuestMasterResult>
        {
            public GetCurrentQuestMasterTask(IGs2Session session, RestSessionRequestFactory factory, GetCurrentQuestMasterRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(GetCurrentQuestMasterRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "quest")
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
		public IEnumerator GetCurrentQuestMaster(
                Request.GetCurrentQuestMasterRequest request,
                UnityAction<AsyncResult<Result.GetCurrentQuestMasterResult>> callback
        )
		{
			var task = new GetCurrentQuestMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.GetCurrentQuestMasterResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.GetCurrentQuestMasterResult> GetCurrentQuestMasterAsync(
                Request.GetCurrentQuestMasterRequest request
        )
		{
			var task = new GetCurrentQuestMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        private class UpdateCurrentQuestMasterTask : Gs2RestSessionTask<UpdateCurrentQuestMasterRequest, UpdateCurrentQuestMasterResult>
        {
            public UpdateCurrentQuestMasterTask(IGs2Session session, RestSessionRequestFactory factory, UpdateCurrentQuestMasterRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(UpdateCurrentQuestMasterRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "quest")
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
		public IEnumerator UpdateCurrentQuestMaster(
                Request.UpdateCurrentQuestMasterRequest request,
                UnityAction<AsyncResult<Result.UpdateCurrentQuestMasterResult>> callback
        )
		{
			var task = new UpdateCurrentQuestMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.UpdateCurrentQuestMasterResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.UpdateCurrentQuestMasterResult> UpdateCurrentQuestMasterAsync(
                Request.UpdateCurrentQuestMasterRequest request
        )
		{
			var task = new UpdateCurrentQuestMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        private class UpdateCurrentQuestMasterFromGitHubTask : Gs2RestSessionTask<UpdateCurrentQuestMasterFromGitHubRequest, UpdateCurrentQuestMasterFromGitHubResult>
        {
            public UpdateCurrentQuestMasterFromGitHubTask(IGs2Session session, RestSessionRequestFactory factory, UpdateCurrentQuestMasterFromGitHubRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(UpdateCurrentQuestMasterFromGitHubRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "quest")
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
		public IEnumerator UpdateCurrentQuestMasterFromGitHub(
                Request.UpdateCurrentQuestMasterFromGitHubRequest request,
                UnityAction<AsyncResult<Result.UpdateCurrentQuestMasterFromGitHubResult>> callback
        )
		{
			var task = new UpdateCurrentQuestMasterFromGitHubTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.UpdateCurrentQuestMasterFromGitHubResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.UpdateCurrentQuestMasterFromGitHubResult> UpdateCurrentQuestMasterFromGitHubAsync(
                Request.UpdateCurrentQuestMasterFromGitHubRequest request
        )
		{
			var task = new UpdateCurrentQuestMasterFromGitHubTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        private class DescribeProgressesByUserIdTask : Gs2RestSessionTask<DescribeProgressesByUserIdRequest, DescribeProgressesByUserIdResult>
        {
            public DescribeProgressesByUserIdTask(IGs2Session session, RestSessionRequestFactory factory, DescribeProgressesByUserIdRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DescribeProgressesByUserIdRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "quest")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/progress";

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
		public IEnumerator DescribeProgressesByUserId(
                Request.DescribeProgressesByUserIdRequest request,
                UnityAction<AsyncResult<Result.DescribeProgressesByUserIdResult>> callback
        )
		{
			var task = new DescribeProgressesByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.DescribeProgressesByUserIdResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.DescribeProgressesByUserIdResult> DescribeProgressesByUserIdAsync(
                Request.DescribeProgressesByUserIdRequest request
        )
		{
			var task = new DescribeProgressesByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        private class CreateProgressByUserIdTask : Gs2RestSessionTask<CreateProgressByUserIdRequest, CreateProgressByUserIdResult>
        {
            public CreateProgressByUserIdTask(IGs2Session session, RestSessionRequestFactory factory, CreateProgressByUserIdRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(CreateProgressByUserIdRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "quest")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/{userId}/progress";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{userId}", !string.IsNullOrEmpty(request.UserId) ? request.UserId.ToString() : "null");

                var sessionRequest = Factory.Post(url);

                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);
                jsonWriter.WriteObjectStart();
                if (request.QuestModelId != null)
                {
                    jsonWriter.WritePropertyName("questModelId");
                    jsonWriter.Write(request.QuestModelId);
                }
                if (request.Force != null)
                {
                    jsonWriter.WritePropertyName("force");
                    jsonWriter.Write(request.Force.ToString());
                }
                if (request.Config != null)
                {
                    jsonWriter.WritePropertyName("config");
                    jsonWriter.WriteArrayStart();
                    foreach(var item in request.Config)
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
		public IEnumerator CreateProgressByUserId(
                Request.CreateProgressByUserIdRequest request,
                UnityAction<AsyncResult<Result.CreateProgressByUserIdResult>> callback
        )
		{
			var task = new CreateProgressByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.CreateProgressByUserIdResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.CreateProgressByUserIdResult> CreateProgressByUserIdAsync(
                Request.CreateProgressByUserIdRequest request
        )
		{
			var task = new CreateProgressByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        private class GetProgressTask : Gs2RestSessionTask<GetProgressRequest, GetProgressResult>
        {
            public GetProgressTask(IGs2Session session, RestSessionRequestFactory factory, GetProgressRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(GetProgressRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "quest")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/me/progress";

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
		public IEnumerator GetProgress(
                Request.GetProgressRequest request,
                UnityAction<AsyncResult<Result.GetProgressResult>> callback
        )
		{
			var task = new GetProgressTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.GetProgressResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.GetProgressResult> GetProgressAsync(
                Request.GetProgressRequest request
        )
		{
			var task = new GetProgressTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        private class GetProgressByUserIdTask : Gs2RestSessionTask<GetProgressByUserIdRequest, GetProgressByUserIdResult>
        {
            public GetProgressByUserIdTask(IGs2Session session, RestSessionRequestFactory factory, GetProgressByUserIdRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(GetProgressByUserIdRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "quest")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/{userId}/progress";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{userId}", !string.IsNullOrEmpty(request.UserId) ? request.UserId.ToString() : "null");

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
		public IEnumerator GetProgressByUserId(
                Request.GetProgressByUserIdRequest request,
                UnityAction<AsyncResult<Result.GetProgressByUserIdResult>> callback
        )
		{
			var task = new GetProgressByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.GetProgressByUserIdResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.GetProgressByUserIdResult> GetProgressByUserIdAsync(
                Request.GetProgressByUserIdRequest request
        )
		{
			var task = new GetProgressByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        private class StartTask : Gs2RestSessionTask<StartRequest, StartResult>
        {
            public StartTask(IGs2Session session, RestSessionRequestFactory factory, StartRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(StartRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "quest")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/me/progress/group/{questGroupName}/quest/{questName}/start";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{questGroupName}", !string.IsNullOrEmpty(request.QuestGroupName) ? request.QuestGroupName.ToString() : "null");
                url = url.Replace("{questName}", !string.IsNullOrEmpty(request.QuestName) ? request.QuestName.ToString() : "null");

                var sessionRequest = Factory.Post(url);

                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);
                jsonWriter.WriteObjectStart();
                if (request.Force != null)
                {
                    jsonWriter.WritePropertyName("force");
                    jsonWriter.Write(request.Force.ToString());
                }
                if (request.Config != null)
                {
                    jsonWriter.WritePropertyName("config");
                    jsonWriter.WriteArrayStart();
                    foreach(var item in request.Config)
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
		public IEnumerator Start(
                Request.StartRequest request,
                UnityAction<AsyncResult<Result.StartResult>> callback
        )
		{
			var task = new StartTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.StartResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.StartResult> StartAsync(
                Request.StartRequest request
        )
		{
			var task = new StartTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        private class StartByUserIdTask : Gs2RestSessionTask<StartByUserIdRequest, StartByUserIdResult>
        {
            public StartByUserIdTask(IGs2Session session, RestSessionRequestFactory factory, StartByUserIdRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(StartByUserIdRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "quest")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/{userId}/progress/group/{questGroupName}/quest/{questName}/start";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{questGroupName}", !string.IsNullOrEmpty(request.QuestGroupName) ? request.QuestGroupName.ToString() : "null");
                url = url.Replace("{questName}", !string.IsNullOrEmpty(request.QuestName) ? request.QuestName.ToString() : "null");
                url = url.Replace("{userId}", !string.IsNullOrEmpty(request.UserId) ? request.UserId.ToString() : "null");

                var sessionRequest = Factory.Post(url);

                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);
                jsonWriter.WriteObjectStart();
                if (request.Force != null)
                {
                    jsonWriter.WritePropertyName("force");
                    jsonWriter.Write(request.Force.ToString());
                }
                if (request.Config != null)
                {
                    jsonWriter.WritePropertyName("config");
                    jsonWriter.WriteArrayStart();
                    foreach(var item in request.Config)
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
		public IEnumerator StartByUserId(
                Request.StartByUserIdRequest request,
                UnityAction<AsyncResult<Result.StartByUserIdResult>> callback
        )
		{
			var task = new StartByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.StartByUserIdResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.StartByUserIdResult> StartByUserIdAsync(
                Request.StartByUserIdRequest request
        )
		{
			var task = new StartByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        private class EndTask : Gs2RestSessionTask<EndRequest, EndResult>
        {
            public EndTask(IGs2Session session, RestSessionRequestFactory factory, EndRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(EndRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "quest")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/me/progress/end";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");

                var sessionRequest = Factory.Post(url);

                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);
                jsonWriter.WriteObjectStart();
                if (request.TransactionId != null)
                {
                    jsonWriter.WritePropertyName("transactionId");
                    jsonWriter.Write(request.TransactionId);
                }
                if (request.Rewards != null)
                {
                    jsonWriter.WritePropertyName("rewards");
                    jsonWriter.WriteArrayStart();
                    foreach(var item in request.Rewards)
                    {
                        item.WriteJson(jsonWriter);
                    }
                    jsonWriter.WriteArrayEnd();
                }
                if (request.IsComplete != null)
                {
                    jsonWriter.WritePropertyName("isComplete");
                    jsonWriter.Write(request.IsComplete.ToString());
                }
                if (request.Config != null)
                {
                    jsonWriter.WritePropertyName("config");
                    jsonWriter.WriteArrayStart();
                    foreach(var item in request.Config)
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
		public IEnumerator End(
                Request.EndRequest request,
                UnityAction<AsyncResult<Result.EndResult>> callback
        )
		{
			var task = new EndTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.EndResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.EndResult> EndAsync(
                Request.EndRequest request
        )
		{
			var task = new EndTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        private class EndByUserIdTask : Gs2RestSessionTask<EndByUserIdRequest, EndByUserIdResult>
        {
            public EndByUserIdTask(IGs2Session session, RestSessionRequestFactory factory, EndByUserIdRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(EndByUserIdRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "quest")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/{userId}/progress/end";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{userId}", !string.IsNullOrEmpty(request.UserId) ? request.UserId.ToString() : "null");

                var sessionRequest = Factory.Post(url);

                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);
                jsonWriter.WriteObjectStart();
                if (request.TransactionId != null)
                {
                    jsonWriter.WritePropertyName("transactionId");
                    jsonWriter.Write(request.TransactionId);
                }
                if (request.Rewards != null)
                {
                    jsonWriter.WritePropertyName("rewards");
                    jsonWriter.WriteArrayStart();
                    foreach(var item in request.Rewards)
                    {
                        item.WriteJson(jsonWriter);
                    }
                    jsonWriter.WriteArrayEnd();
                }
                if (request.IsComplete != null)
                {
                    jsonWriter.WritePropertyName("isComplete");
                    jsonWriter.Write(request.IsComplete.ToString());
                }
                if (request.Config != null)
                {
                    jsonWriter.WritePropertyName("config");
                    jsonWriter.WriteArrayStart();
                    foreach(var item in request.Config)
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
		public IEnumerator EndByUserId(
                Request.EndByUserIdRequest request,
                UnityAction<AsyncResult<Result.EndByUserIdResult>> callback
        )
		{
			var task = new EndByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.EndByUserIdResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.EndByUserIdResult> EndByUserIdAsync(
                Request.EndByUserIdRequest request
        )
		{
			var task = new EndByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        private class DeleteProgressTask : Gs2RestSessionTask<DeleteProgressRequest, DeleteProgressResult>
        {
            public DeleteProgressTask(IGs2Session session, RestSessionRequestFactory factory, DeleteProgressRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DeleteProgressRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "quest")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/me/progress";

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
		public IEnumerator DeleteProgress(
                Request.DeleteProgressRequest request,
                UnityAction<AsyncResult<Result.DeleteProgressResult>> callback
        )
		{
			var task = new DeleteProgressTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.DeleteProgressResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.DeleteProgressResult> DeleteProgressAsync(
                Request.DeleteProgressRequest request
        )
		{
			var task = new DeleteProgressTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        private class DeleteProgressByUserIdTask : Gs2RestSessionTask<DeleteProgressByUserIdRequest, DeleteProgressByUserIdResult>
        {
            public DeleteProgressByUserIdTask(IGs2Session session, RestSessionRequestFactory factory, DeleteProgressByUserIdRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DeleteProgressByUserIdRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "quest")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/{userId}/progress";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{userId}", !string.IsNullOrEmpty(request.UserId) ? request.UserId.ToString() : "null");

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
		public IEnumerator DeleteProgressByUserId(
                Request.DeleteProgressByUserIdRequest request,
                UnityAction<AsyncResult<Result.DeleteProgressByUserIdResult>> callback
        )
		{
			var task = new DeleteProgressByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.DeleteProgressByUserIdResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.DeleteProgressByUserIdResult> DeleteProgressByUserIdAsync(
                Request.DeleteProgressByUserIdRequest request
        )
		{
			var task = new DeleteProgressByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        private class CreateProgressByStampSheetTask : Gs2RestSessionTask<CreateProgressByStampSheetRequest, CreateProgressByStampSheetResult>
        {
            public CreateProgressByStampSheetTask(IGs2Session session, RestSessionRequestFactory factory, CreateProgressByStampSheetRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(CreateProgressByStampSheetRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "quest")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/stamp/progress/create";

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
		public IEnumerator CreateProgressByStampSheet(
                Request.CreateProgressByStampSheetRequest request,
                UnityAction<AsyncResult<Result.CreateProgressByStampSheetResult>> callback
        )
		{
			var task = new CreateProgressByStampSheetTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.CreateProgressByStampSheetResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.CreateProgressByStampSheetResult> CreateProgressByStampSheetAsync(
                Request.CreateProgressByStampSheetRequest request
        )
		{
			var task = new CreateProgressByStampSheetTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        private class DeleteProgressByStampTaskTask : Gs2RestSessionTask<DeleteProgressByStampTaskRequest, DeleteProgressByStampTaskResult>
        {
            public DeleteProgressByStampTaskTask(IGs2Session session, RestSessionRequestFactory factory, DeleteProgressByStampTaskRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DeleteProgressByStampTaskRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "quest")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/stamp/progress/delete";

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
		public IEnumerator DeleteProgressByStampTask(
                Request.DeleteProgressByStampTaskRequest request,
                UnityAction<AsyncResult<Result.DeleteProgressByStampTaskResult>> callback
        )
		{
			var task = new DeleteProgressByStampTaskTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.DeleteProgressByStampTaskResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.DeleteProgressByStampTaskResult> DeleteProgressByStampTaskAsync(
                Request.DeleteProgressByStampTaskRequest request
        )
		{
			var task = new DeleteProgressByStampTaskTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        private class DescribeCompletedQuestListsTask : Gs2RestSessionTask<DescribeCompletedQuestListsRequest, DescribeCompletedQuestListsResult>
        {
            public DescribeCompletedQuestListsTask(IGs2Session session, RestSessionRequestFactory factory, DescribeCompletedQuestListsRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DescribeCompletedQuestListsRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "quest")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/me/completed";

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
		public IEnumerator DescribeCompletedQuestLists(
                Request.DescribeCompletedQuestListsRequest request,
                UnityAction<AsyncResult<Result.DescribeCompletedQuestListsResult>> callback
        )
		{
			var task = new DescribeCompletedQuestListsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.DescribeCompletedQuestListsResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.DescribeCompletedQuestListsResult> DescribeCompletedQuestListsAsync(
                Request.DescribeCompletedQuestListsRequest request
        )
		{
			var task = new DescribeCompletedQuestListsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        private class DescribeCompletedQuestListsByUserIdTask : Gs2RestSessionTask<DescribeCompletedQuestListsByUserIdRequest, DescribeCompletedQuestListsByUserIdResult>
        {
            public DescribeCompletedQuestListsByUserIdTask(IGs2Session session, RestSessionRequestFactory factory, DescribeCompletedQuestListsByUserIdRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DescribeCompletedQuestListsByUserIdRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "quest")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/{userId}/completed";

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
		public IEnumerator DescribeCompletedQuestListsByUserId(
                Request.DescribeCompletedQuestListsByUserIdRequest request,
                UnityAction<AsyncResult<Result.DescribeCompletedQuestListsByUserIdResult>> callback
        )
		{
			var task = new DescribeCompletedQuestListsByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.DescribeCompletedQuestListsByUserIdResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.DescribeCompletedQuestListsByUserIdResult> DescribeCompletedQuestListsByUserIdAsync(
                Request.DescribeCompletedQuestListsByUserIdRequest request
        )
		{
			var task = new DescribeCompletedQuestListsByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        private class GetCompletedQuestListTask : Gs2RestSessionTask<GetCompletedQuestListRequest, GetCompletedQuestListResult>
        {
            public GetCompletedQuestListTask(IGs2Session session, RestSessionRequestFactory factory, GetCompletedQuestListRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(GetCompletedQuestListRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "quest")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/me/completed/group/{questGroupName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{questGroupName}", !string.IsNullOrEmpty(request.QuestGroupName) ? request.QuestGroupName.ToString() : "null");

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
		public IEnumerator GetCompletedQuestList(
                Request.GetCompletedQuestListRequest request,
                UnityAction<AsyncResult<Result.GetCompletedQuestListResult>> callback
        )
		{
			var task = new GetCompletedQuestListTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.GetCompletedQuestListResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.GetCompletedQuestListResult> GetCompletedQuestListAsync(
                Request.GetCompletedQuestListRequest request
        )
		{
			var task = new GetCompletedQuestListTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        private class GetCompletedQuestListByUserIdTask : Gs2RestSessionTask<GetCompletedQuestListByUserIdRequest, GetCompletedQuestListByUserIdResult>
        {
            public GetCompletedQuestListByUserIdTask(IGs2Session session, RestSessionRequestFactory factory, GetCompletedQuestListByUserIdRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(GetCompletedQuestListByUserIdRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "quest")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/{userId}/completed/group/{questGroupName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{questGroupName}", !string.IsNullOrEmpty(request.QuestGroupName) ? request.QuestGroupName.ToString() : "null");
                url = url.Replace("{userId}", !string.IsNullOrEmpty(request.UserId) ? request.UserId.ToString() : "null");

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
		public IEnumerator GetCompletedQuestListByUserId(
                Request.GetCompletedQuestListByUserIdRequest request,
                UnityAction<AsyncResult<Result.GetCompletedQuestListByUserIdResult>> callback
        )
		{
			var task = new GetCompletedQuestListByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.GetCompletedQuestListByUserIdResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.GetCompletedQuestListByUserIdResult> GetCompletedQuestListByUserIdAsync(
                Request.GetCompletedQuestListByUserIdRequest request
        )
		{
			var task = new GetCompletedQuestListByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        private class DeleteCompletedQuestListByUserIdTask : Gs2RestSessionTask<DeleteCompletedQuestListByUserIdRequest, DeleteCompletedQuestListByUserIdResult>
        {
            public DeleteCompletedQuestListByUserIdTask(IGs2Session session, RestSessionRequestFactory factory, DeleteCompletedQuestListByUserIdRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DeleteCompletedQuestListByUserIdRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "quest")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/{userId}/completed/group/{questGroupName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{questGroupName}", !string.IsNullOrEmpty(request.QuestGroupName) ? request.QuestGroupName.ToString() : "null");
                url = url.Replace("{userId}", !string.IsNullOrEmpty(request.UserId) ? request.UserId.ToString() : "null");

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
		public IEnumerator DeleteCompletedQuestListByUserId(
                Request.DeleteCompletedQuestListByUserIdRequest request,
                UnityAction<AsyncResult<Result.DeleteCompletedQuestListByUserIdResult>> callback
        )
		{
			var task = new DeleteCompletedQuestListByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.DeleteCompletedQuestListByUserIdResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.DeleteCompletedQuestListByUserIdResult> DeleteCompletedQuestListByUserIdAsync(
                Request.DeleteCompletedQuestListByUserIdRequest request
        )
		{
			var task = new DeleteCompletedQuestListByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        private class DescribeQuestGroupModelsTask : Gs2RestSessionTask<DescribeQuestGroupModelsRequest, DescribeQuestGroupModelsResult>
        {
            public DescribeQuestGroupModelsTask(IGs2Session session, RestSessionRequestFactory factory, DescribeQuestGroupModelsRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DescribeQuestGroupModelsRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "quest")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/group";

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
		public IEnumerator DescribeQuestGroupModels(
                Request.DescribeQuestGroupModelsRequest request,
                UnityAction<AsyncResult<Result.DescribeQuestGroupModelsResult>> callback
        )
		{
			var task = new DescribeQuestGroupModelsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.DescribeQuestGroupModelsResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.DescribeQuestGroupModelsResult> DescribeQuestGroupModelsAsync(
                Request.DescribeQuestGroupModelsRequest request
        )
		{
			var task = new DescribeQuestGroupModelsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        private class GetQuestGroupModelTask : Gs2RestSessionTask<GetQuestGroupModelRequest, GetQuestGroupModelResult>
        {
            public GetQuestGroupModelTask(IGs2Session session, RestSessionRequestFactory factory, GetQuestGroupModelRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(GetQuestGroupModelRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "quest")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/group/{questGroupName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{questGroupName}", !string.IsNullOrEmpty(request.QuestGroupName) ? request.QuestGroupName.ToString() : "null");

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
		public IEnumerator GetQuestGroupModel(
                Request.GetQuestGroupModelRequest request,
                UnityAction<AsyncResult<Result.GetQuestGroupModelResult>> callback
        )
		{
			var task = new GetQuestGroupModelTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.GetQuestGroupModelResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.GetQuestGroupModelResult> GetQuestGroupModelAsync(
                Request.GetQuestGroupModelRequest request
        )
		{
			var task = new GetQuestGroupModelTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        private class DescribeQuestModelsTask : Gs2RestSessionTask<DescribeQuestModelsRequest, DescribeQuestModelsResult>
        {
            public DescribeQuestModelsTask(IGs2Session session, RestSessionRequestFactory factory, DescribeQuestModelsRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DescribeQuestModelsRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "quest")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/group/{questGroupName}/quest";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{questGroupName}", !string.IsNullOrEmpty(request.QuestGroupName) ? request.QuestGroupName.ToString() : "null");

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
		public IEnumerator DescribeQuestModels(
                Request.DescribeQuestModelsRequest request,
                UnityAction<AsyncResult<Result.DescribeQuestModelsResult>> callback
        )
		{
			var task = new DescribeQuestModelsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.DescribeQuestModelsResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.DescribeQuestModelsResult> DescribeQuestModelsAsync(
                Request.DescribeQuestModelsRequest request
        )
		{
			var task = new DescribeQuestModelsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        private class GetQuestModelTask : Gs2RestSessionTask<GetQuestModelRequest, GetQuestModelResult>
        {
            public GetQuestModelTask(IGs2Session session, RestSessionRequestFactory factory, GetQuestModelRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(GetQuestModelRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "quest")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/group/{questGroupName}/quest/{questName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{questGroupName}", !string.IsNullOrEmpty(request.QuestGroupName) ? request.QuestGroupName.ToString() : "null");
                url = url.Replace("{questName}", !string.IsNullOrEmpty(request.QuestName) ? request.QuestName.ToString() : "null");

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
		public IEnumerator GetQuestModel(
                Request.GetQuestModelRequest request,
                UnityAction<AsyncResult<Result.GetQuestModelResult>> callback
        )
		{
			var task = new GetQuestModelTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.GetQuestModelResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.GetQuestModelResult> GetQuestModelAsync(
                Request.GetQuestModelRequest request
        )
		{
			var task = new GetQuestModelTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif
	}
}