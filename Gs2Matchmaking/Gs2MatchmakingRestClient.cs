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
using Gs2.Gs2Matchmaking.Request;
using Gs2.Gs2Matchmaking.Result;
using Gs2.Util.LitJson;

namespace Gs2.Gs2Matchmaking
{
	public class Gs2MatchmakingRestClient : AbstractGs2Client
	{
#if UNITY_2017_1_OR_NEWER
		private readonly CertificateHandler _certificateHandler;
#endif

		public static string Endpoint = "matchmaking";

        protected Gs2RestSession Gs2RestSession => (Gs2RestSession) Gs2Session;

		public Gs2MatchmakingRestClient(Gs2RestSession Gs2RestSession) : base(Gs2RestSession)
		{

		}

#if UNITY_2017_1_OR_NEWER
		public Gs2MatchmakingRestClient(Gs2RestSession gs2RestSession, CertificateHandler certificateHandler) : base(gs2RestSession)
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
                    .Replace("{service}", "matchmaking")
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

		public IFuture<Result.DescribeNamespacesResult> DescribeNamespacesFuture(
                Request.DescribeNamespacesRequest request
        )
		{
			return new DescribeNamespacesTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
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
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
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
                    .Replace("{service}", "matchmaking")
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
                if (request.EnableRating != null)
                {
                    jsonWriter.WritePropertyName("enableRating");
                    jsonWriter.Write(request.EnableRating.ToString());
                }
                if (request.CreateGatheringTriggerType != null)
                {
                    jsonWriter.WritePropertyName("createGatheringTriggerType");
                    jsonWriter.Write(request.CreateGatheringTriggerType);
                }
                if (request.CreateGatheringTriggerRealtimeNamespaceId != null)
                {
                    jsonWriter.WritePropertyName("createGatheringTriggerRealtimeNamespaceId");
                    jsonWriter.Write(request.CreateGatheringTriggerRealtimeNamespaceId);
                }
                if (request.CreateGatheringTriggerScriptId != null)
                {
                    jsonWriter.WritePropertyName("createGatheringTriggerScriptId");
                    jsonWriter.Write(request.CreateGatheringTriggerScriptId);
                }
                if (request.CompleteMatchmakingTriggerType != null)
                {
                    jsonWriter.WritePropertyName("completeMatchmakingTriggerType");
                    jsonWriter.Write(request.CompleteMatchmakingTriggerType);
                }
                if (request.CompleteMatchmakingTriggerRealtimeNamespaceId != null)
                {
                    jsonWriter.WritePropertyName("completeMatchmakingTriggerRealtimeNamespaceId");
                    jsonWriter.Write(request.CompleteMatchmakingTriggerRealtimeNamespaceId);
                }
                if (request.CompleteMatchmakingTriggerScriptId != null)
                {
                    jsonWriter.WritePropertyName("completeMatchmakingTriggerScriptId");
                    jsonWriter.Write(request.CompleteMatchmakingTriggerScriptId);
                }
                if (request.JoinNotification != null)
                {
                    jsonWriter.WritePropertyName("joinNotification");
                    request.JoinNotification.WriteJson(jsonWriter);
                }
                if (request.LeaveNotification != null)
                {
                    jsonWriter.WritePropertyName("leaveNotification");
                    request.LeaveNotification.WriteJson(jsonWriter);
                }
                if (request.CompleteNotification != null)
                {
                    jsonWriter.WritePropertyName("completeNotification");
                    request.CompleteNotification.WriteJson(jsonWriter);
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

		public IFuture<Result.CreateNamespaceResult> CreateNamespaceFuture(
                Request.CreateNamespaceRequest request
        )
		{
			return new CreateNamespaceTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
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
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
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
                    .Replace("{service}", "matchmaking")
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

		public IFuture<Result.GetNamespaceStatusResult> GetNamespaceStatusFuture(
                Request.GetNamespaceStatusRequest request
        )
		{
			return new GetNamespaceStatusTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
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
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
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
                    .Replace("{service}", "matchmaking")
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

		public IFuture<Result.GetNamespaceResult> GetNamespaceFuture(
                Request.GetNamespaceRequest request
        )
		{
			return new GetNamespaceTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
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
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
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
                    .Replace("{service}", "matchmaking")
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
                if (request.EnableRating != null)
                {
                    jsonWriter.WritePropertyName("enableRating");
                    jsonWriter.Write(request.EnableRating.ToString());
                }
                if (request.CreateGatheringTriggerType != null)
                {
                    jsonWriter.WritePropertyName("createGatheringTriggerType");
                    jsonWriter.Write(request.CreateGatheringTriggerType);
                }
                if (request.CreateGatheringTriggerRealtimeNamespaceId != null)
                {
                    jsonWriter.WritePropertyName("createGatheringTriggerRealtimeNamespaceId");
                    jsonWriter.Write(request.CreateGatheringTriggerRealtimeNamespaceId);
                }
                if (request.CreateGatheringTriggerScriptId != null)
                {
                    jsonWriter.WritePropertyName("createGatheringTriggerScriptId");
                    jsonWriter.Write(request.CreateGatheringTriggerScriptId);
                }
                if (request.CompleteMatchmakingTriggerType != null)
                {
                    jsonWriter.WritePropertyName("completeMatchmakingTriggerType");
                    jsonWriter.Write(request.CompleteMatchmakingTriggerType);
                }
                if (request.CompleteMatchmakingTriggerRealtimeNamespaceId != null)
                {
                    jsonWriter.WritePropertyName("completeMatchmakingTriggerRealtimeNamespaceId");
                    jsonWriter.Write(request.CompleteMatchmakingTriggerRealtimeNamespaceId);
                }
                if (request.CompleteMatchmakingTriggerScriptId != null)
                {
                    jsonWriter.WritePropertyName("completeMatchmakingTriggerScriptId");
                    jsonWriter.Write(request.CompleteMatchmakingTriggerScriptId);
                }
                if (request.JoinNotification != null)
                {
                    jsonWriter.WritePropertyName("joinNotification");
                    request.JoinNotification.WriteJson(jsonWriter);
                }
                if (request.LeaveNotification != null)
                {
                    jsonWriter.WritePropertyName("leaveNotification");
                    request.LeaveNotification.WriteJson(jsonWriter);
                }
                if (request.CompleteNotification != null)
                {
                    jsonWriter.WritePropertyName("completeNotification");
                    request.CompleteNotification.WriteJson(jsonWriter);
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

		public IFuture<Result.UpdateNamespaceResult> UpdateNamespaceFuture(
                Request.UpdateNamespaceRequest request
        )
		{
			return new UpdateNamespaceTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
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
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
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
                    .Replace("{service}", "matchmaking")
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

		public IFuture<Result.DeleteNamespaceResult> DeleteNamespaceFuture(
                Request.DeleteNamespaceRequest request
        )
		{
			return new DeleteNamespaceTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
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
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
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


        public class DescribeGatheringsTask : Gs2RestSessionTask<DescribeGatheringsRequest, DescribeGatheringsResult>
        {
            public DescribeGatheringsTask(IGs2Session session, RestSessionRequestFactory factory, DescribeGatheringsRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DescribeGatheringsRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "matchmaking")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/gathering";

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
		public IEnumerator DescribeGatherings(
                Request.DescribeGatheringsRequest request,
                UnityAction<AsyncResult<Result.DescribeGatheringsResult>> callback
        )
		{
			var task = new DescribeGatheringsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.DescribeGatheringsResult>(task.Result, task.Error));
        }

		public IFuture<Result.DescribeGatheringsResult> DescribeGatheringsFuture(
                Request.DescribeGatheringsRequest request
        )
		{
			return new DescribeGatheringsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.DescribeGatheringsResult> DescribeGatheringsAsync(
                Request.DescribeGatheringsRequest request
        )
		{
            AsyncResult<Result.DescribeGatheringsResult> result = null;
			await DescribeGatherings(
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
		public DescribeGatheringsTask DescribeGatheringsAsync(
                Request.DescribeGatheringsRequest request
        )
		{
			return new DescribeGatheringsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
			    request
            );
        }
    #endif
#else
		public async Task<Result.DescribeGatheringsResult> DescribeGatheringsAsync(
                Request.DescribeGatheringsRequest request
        )
		{
			var task = new DescribeGatheringsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class CreateGatheringTask : Gs2RestSessionTask<CreateGatheringRequest, CreateGatheringResult>
        {
            public CreateGatheringTask(IGs2Session session, RestSessionRequestFactory factory, CreateGatheringRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(CreateGatheringRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "matchmaking")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/gathering";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");

                var sessionRequest = Factory.Post(url);

                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);
                jsonWriter.WriteObjectStart();
                if (request.Player != null)
                {
                    jsonWriter.WritePropertyName("player");
                    request.Player.WriteJson(jsonWriter);
                }
                if (request.AttributeRanges != null)
                {
                    jsonWriter.WritePropertyName("attributeRanges");
                    jsonWriter.WriteArrayStart();
                    foreach(var item in request.AttributeRanges)
                    {
                        item.WriteJson(jsonWriter);
                    }
                    jsonWriter.WriteArrayEnd();
                }
                if (request.CapacityOfRoles != null)
                {
                    jsonWriter.WritePropertyName("capacityOfRoles");
                    jsonWriter.WriteArrayStart();
                    foreach(var item in request.CapacityOfRoles)
                    {
                        item.WriteJson(jsonWriter);
                    }
                    jsonWriter.WriteArrayEnd();
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
                if (request.ExpiresAt != null)
                {
                    jsonWriter.WritePropertyName("expiresAt");
                    jsonWriter.Write(request.ExpiresAt.ToString());
                }
                if (request.ExpiresAtTimeSpan != null)
                {
                    jsonWriter.WritePropertyName("expiresAtTimeSpan");
                    request.ExpiresAtTimeSpan.WriteJson(jsonWriter);
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
		public IEnumerator CreateGathering(
                Request.CreateGatheringRequest request,
                UnityAction<AsyncResult<Result.CreateGatheringResult>> callback
        )
		{
			var task = new CreateGatheringTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.CreateGatheringResult>(task.Result, task.Error));
        }

		public IFuture<Result.CreateGatheringResult> CreateGatheringFuture(
                Request.CreateGatheringRequest request
        )
		{
			return new CreateGatheringTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.CreateGatheringResult> CreateGatheringAsync(
                Request.CreateGatheringRequest request
        )
		{
            AsyncResult<Result.CreateGatheringResult> result = null;
			await CreateGathering(
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
		public CreateGatheringTask CreateGatheringAsync(
                Request.CreateGatheringRequest request
        )
		{
			return new CreateGatheringTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
			    request
            );
        }
    #endif
#else
		public async Task<Result.CreateGatheringResult> CreateGatheringAsync(
                Request.CreateGatheringRequest request
        )
		{
			var task = new CreateGatheringTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class CreateGatheringByUserIdTask : Gs2RestSessionTask<CreateGatheringByUserIdRequest, CreateGatheringByUserIdResult>
        {
            public CreateGatheringByUserIdTask(IGs2Session session, RestSessionRequestFactory factory, CreateGatheringByUserIdRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(CreateGatheringByUserIdRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "matchmaking")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/gathering/user/{userId}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{userId}", !string.IsNullOrEmpty(request.UserId) ? request.UserId.ToString() : "null");

                var sessionRequest = Factory.Post(url);

                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);
                jsonWriter.WriteObjectStart();
                if (request.Player != null)
                {
                    jsonWriter.WritePropertyName("player");
                    request.Player.WriteJson(jsonWriter);
                }
                if (request.AttributeRanges != null)
                {
                    jsonWriter.WritePropertyName("attributeRanges");
                    jsonWriter.WriteArrayStart();
                    foreach(var item in request.AttributeRanges)
                    {
                        item.WriteJson(jsonWriter);
                    }
                    jsonWriter.WriteArrayEnd();
                }
                if (request.CapacityOfRoles != null)
                {
                    jsonWriter.WritePropertyName("capacityOfRoles");
                    jsonWriter.WriteArrayStart();
                    foreach(var item in request.CapacityOfRoles)
                    {
                        item.WriteJson(jsonWriter);
                    }
                    jsonWriter.WriteArrayEnd();
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
                if (request.ExpiresAt != null)
                {
                    jsonWriter.WritePropertyName("expiresAt");
                    jsonWriter.Write(request.ExpiresAt.ToString());
                }
                if (request.ExpiresAtTimeSpan != null)
                {
                    jsonWriter.WritePropertyName("expiresAtTimeSpan");
                    request.ExpiresAtTimeSpan.WriteJson(jsonWriter);
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
		public IEnumerator CreateGatheringByUserId(
                Request.CreateGatheringByUserIdRequest request,
                UnityAction<AsyncResult<Result.CreateGatheringByUserIdResult>> callback
        )
		{
			var task = new CreateGatheringByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.CreateGatheringByUserIdResult>(task.Result, task.Error));
        }

		public IFuture<Result.CreateGatheringByUserIdResult> CreateGatheringByUserIdFuture(
                Request.CreateGatheringByUserIdRequest request
        )
		{
			return new CreateGatheringByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.CreateGatheringByUserIdResult> CreateGatheringByUserIdAsync(
                Request.CreateGatheringByUserIdRequest request
        )
		{
            AsyncResult<Result.CreateGatheringByUserIdResult> result = null;
			await CreateGatheringByUserId(
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
		public CreateGatheringByUserIdTask CreateGatheringByUserIdAsync(
                Request.CreateGatheringByUserIdRequest request
        )
		{
			return new CreateGatheringByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
			    request
            );
        }
    #endif
#else
		public async Task<Result.CreateGatheringByUserIdResult> CreateGatheringByUserIdAsync(
                Request.CreateGatheringByUserIdRequest request
        )
		{
			var task = new CreateGatheringByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class UpdateGatheringTask : Gs2RestSessionTask<UpdateGatheringRequest, UpdateGatheringResult>
        {
            public UpdateGatheringTask(IGs2Session session, RestSessionRequestFactory factory, UpdateGatheringRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(UpdateGatheringRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "matchmaking")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/gathering/{gatheringName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{gatheringName}", !string.IsNullOrEmpty(request.GatheringName) ? request.GatheringName.ToString() : "null");

                var sessionRequest = Factory.Post(url);

                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);
                jsonWriter.WriteObjectStart();
                if (request.AttributeRanges != null)
                {
                    jsonWriter.WritePropertyName("attributeRanges");
                    jsonWriter.WriteArrayStart();
                    foreach(var item in request.AttributeRanges)
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
		public IEnumerator UpdateGathering(
                Request.UpdateGatheringRequest request,
                UnityAction<AsyncResult<Result.UpdateGatheringResult>> callback
        )
		{
			var task = new UpdateGatheringTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.UpdateGatheringResult>(task.Result, task.Error));
        }

		public IFuture<Result.UpdateGatheringResult> UpdateGatheringFuture(
                Request.UpdateGatheringRequest request
        )
		{
			return new UpdateGatheringTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.UpdateGatheringResult> UpdateGatheringAsync(
                Request.UpdateGatheringRequest request
        )
		{
            AsyncResult<Result.UpdateGatheringResult> result = null;
			await UpdateGathering(
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
		public UpdateGatheringTask UpdateGatheringAsync(
                Request.UpdateGatheringRequest request
        )
		{
			return new UpdateGatheringTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
			    request
            );
        }
    #endif
#else
		public async Task<Result.UpdateGatheringResult> UpdateGatheringAsync(
                Request.UpdateGatheringRequest request
        )
		{
			var task = new UpdateGatheringTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class UpdateGatheringByUserIdTask : Gs2RestSessionTask<UpdateGatheringByUserIdRequest, UpdateGatheringByUserIdResult>
        {
            public UpdateGatheringByUserIdTask(IGs2Session session, RestSessionRequestFactory factory, UpdateGatheringByUserIdRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(UpdateGatheringByUserIdRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "matchmaking")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/gathering/{gatheringName}/user/{userId}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{gatheringName}", !string.IsNullOrEmpty(request.GatheringName) ? request.GatheringName.ToString() : "null");
                url = url.Replace("{userId}", !string.IsNullOrEmpty(request.UserId) ? request.UserId.ToString() : "null");

                var sessionRequest = Factory.Post(url);

                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);
                jsonWriter.WriteObjectStart();
                if (request.AttributeRanges != null)
                {
                    jsonWriter.WritePropertyName("attributeRanges");
                    jsonWriter.WriteArrayStart();
                    foreach(var item in request.AttributeRanges)
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
		public IEnumerator UpdateGatheringByUserId(
                Request.UpdateGatheringByUserIdRequest request,
                UnityAction<AsyncResult<Result.UpdateGatheringByUserIdResult>> callback
        )
		{
			var task = new UpdateGatheringByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.UpdateGatheringByUserIdResult>(task.Result, task.Error));
        }

		public IFuture<Result.UpdateGatheringByUserIdResult> UpdateGatheringByUserIdFuture(
                Request.UpdateGatheringByUserIdRequest request
        )
		{
			return new UpdateGatheringByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.UpdateGatheringByUserIdResult> UpdateGatheringByUserIdAsync(
                Request.UpdateGatheringByUserIdRequest request
        )
		{
            AsyncResult<Result.UpdateGatheringByUserIdResult> result = null;
			await UpdateGatheringByUserId(
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
		public UpdateGatheringByUserIdTask UpdateGatheringByUserIdAsync(
                Request.UpdateGatheringByUserIdRequest request
        )
		{
			return new UpdateGatheringByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
			    request
            );
        }
    #endif
#else
		public async Task<Result.UpdateGatheringByUserIdResult> UpdateGatheringByUserIdAsync(
                Request.UpdateGatheringByUserIdRequest request
        )
		{
			var task = new UpdateGatheringByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class DoMatchmakingByPlayerTask : Gs2RestSessionTask<DoMatchmakingByPlayerRequest, DoMatchmakingByPlayerResult>
        {
            public DoMatchmakingByPlayerTask(IGs2Session session, RestSessionRequestFactory factory, DoMatchmakingByPlayerRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DoMatchmakingByPlayerRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "matchmaking")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/gathering/player/do";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");

                var sessionRequest = Factory.Post(url);

                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);
                jsonWriter.WriteObjectStart();
                if (request.Player != null)
                {
                    jsonWriter.WritePropertyName("player");
                    request.Player.WriteJson(jsonWriter);
                }
                if (request.MatchmakingContextToken != null)
                {
                    jsonWriter.WritePropertyName("matchmakingContextToken");
                    jsonWriter.Write(request.MatchmakingContextToken);
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
		public IEnumerator DoMatchmakingByPlayer(
                Request.DoMatchmakingByPlayerRequest request,
                UnityAction<AsyncResult<Result.DoMatchmakingByPlayerResult>> callback
        )
		{
			var task = new DoMatchmakingByPlayerTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.DoMatchmakingByPlayerResult>(task.Result, task.Error));
        }

		public IFuture<Result.DoMatchmakingByPlayerResult> DoMatchmakingByPlayerFuture(
                Request.DoMatchmakingByPlayerRequest request
        )
		{
			return new DoMatchmakingByPlayerTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.DoMatchmakingByPlayerResult> DoMatchmakingByPlayerAsync(
                Request.DoMatchmakingByPlayerRequest request
        )
		{
            AsyncResult<Result.DoMatchmakingByPlayerResult> result = null;
			await DoMatchmakingByPlayer(
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
		public DoMatchmakingByPlayerTask DoMatchmakingByPlayerAsync(
                Request.DoMatchmakingByPlayerRequest request
        )
		{
			return new DoMatchmakingByPlayerTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
			    request
            );
        }
    #endif
#else
		public async Task<Result.DoMatchmakingByPlayerResult> DoMatchmakingByPlayerAsync(
                Request.DoMatchmakingByPlayerRequest request
        )
		{
			var task = new DoMatchmakingByPlayerTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class DoMatchmakingTask : Gs2RestSessionTask<DoMatchmakingRequest, DoMatchmakingResult>
        {
            public DoMatchmakingTask(IGs2Session session, RestSessionRequestFactory factory, DoMatchmakingRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DoMatchmakingRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "matchmaking")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/gathering/do";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");

                var sessionRequest = Factory.Post(url);

                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);
                jsonWriter.WriteObjectStart();
                if (request.Player != null)
                {
                    jsonWriter.WritePropertyName("player");
                    request.Player.WriteJson(jsonWriter);
                }
                if (request.MatchmakingContextToken != null)
                {
                    jsonWriter.WritePropertyName("matchmakingContextToken");
                    jsonWriter.Write(request.MatchmakingContextToken);
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
		public IEnumerator DoMatchmaking(
                Request.DoMatchmakingRequest request,
                UnityAction<AsyncResult<Result.DoMatchmakingResult>> callback
        )
		{
			var task = new DoMatchmakingTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.DoMatchmakingResult>(task.Result, task.Error));
        }

		public IFuture<Result.DoMatchmakingResult> DoMatchmakingFuture(
                Request.DoMatchmakingRequest request
        )
		{
			return new DoMatchmakingTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.DoMatchmakingResult> DoMatchmakingAsync(
                Request.DoMatchmakingRequest request
        )
		{
            AsyncResult<Result.DoMatchmakingResult> result = null;
			await DoMatchmaking(
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
		public DoMatchmakingTask DoMatchmakingAsync(
                Request.DoMatchmakingRequest request
        )
		{
			return new DoMatchmakingTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
			    request
            );
        }
    #endif
#else
		public async Task<Result.DoMatchmakingResult> DoMatchmakingAsync(
                Request.DoMatchmakingRequest request
        )
		{
			var task = new DoMatchmakingTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class DoMatchmakingByUserIdTask : Gs2RestSessionTask<DoMatchmakingByUserIdRequest, DoMatchmakingByUserIdResult>
        {
            public DoMatchmakingByUserIdTask(IGs2Session session, RestSessionRequestFactory factory, DoMatchmakingByUserIdRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DoMatchmakingByUserIdRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "matchmaking")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/{userId}/gathering/do";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{userId}", !string.IsNullOrEmpty(request.UserId) ? request.UserId.ToString() : "null");

                var sessionRequest = Factory.Post(url);

                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);
                jsonWriter.WriteObjectStart();
                if (request.Player != null)
                {
                    jsonWriter.WritePropertyName("player");
                    request.Player.WriteJson(jsonWriter);
                }
                if (request.MatchmakingContextToken != null)
                {
                    jsonWriter.WritePropertyName("matchmakingContextToken");
                    jsonWriter.Write(request.MatchmakingContextToken);
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
		public IEnumerator DoMatchmakingByUserId(
                Request.DoMatchmakingByUserIdRequest request,
                UnityAction<AsyncResult<Result.DoMatchmakingByUserIdResult>> callback
        )
		{
			var task = new DoMatchmakingByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.DoMatchmakingByUserIdResult>(task.Result, task.Error));
        }

		public IFuture<Result.DoMatchmakingByUserIdResult> DoMatchmakingByUserIdFuture(
                Request.DoMatchmakingByUserIdRequest request
        )
		{
			return new DoMatchmakingByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.DoMatchmakingByUserIdResult> DoMatchmakingByUserIdAsync(
                Request.DoMatchmakingByUserIdRequest request
        )
		{
            AsyncResult<Result.DoMatchmakingByUserIdResult> result = null;
			await DoMatchmakingByUserId(
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
		public DoMatchmakingByUserIdTask DoMatchmakingByUserIdAsync(
                Request.DoMatchmakingByUserIdRequest request
        )
		{
			return new DoMatchmakingByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
			    request
            );
        }
    #endif
#else
		public async Task<Result.DoMatchmakingByUserIdResult> DoMatchmakingByUserIdAsync(
                Request.DoMatchmakingByUserIdRequest request
        )
		{
			var task = new DoMatchmakingByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class GetGatheringTask : Gs2RestSessionTask<GetGatheringRequest, GetGatheringResult>
        {
            public GetGatheringTask(IGs2Session session, RestSessionRequestFactory factory, GetGatheringRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(GetGatheringRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "matchmaking")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/gathering/{gatheringName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{gatheringName}", !string.IsNullOrEmpty(request.GatheringName) ? request.GatheringName.ToString() : "null");

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
		public IEnumerator GetGathering(
                Request.GetGatheringRequest request,
                UnityAction<AsyncResult<Result.GetGatheringResult>> callback
        )
		{
			var task = new GetGatheringTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.GetGatheringResult>(task.Result, task.Error));
        }

		public IFuture<Result.GetGatheringResult> GetGatheringFuture(
                Request.GetGatheringRequest request
        )
		{
			return new GetGatheringTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.GetGatheringResult> GetGatheringAsync(
                Request.GetGatheringRequest request
        )
		{
            AsyncResult<Result.GetGatheringResult> result = null;
			await GetGathering(
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
		public GetGatheringTask GetGatheringAsync(
                Request.GetGatheringRequest request
        )
		{
			return new GetGatheringTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
			    request
            );
        }
    #endif
#else
		public async Task<Result.GetGatheringResult> GetGatheringAsync(
                Request.GetGatheringRequest request
        )
		{
			var task = new GetGatheringTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class CancelMatchmakingTask : Gs2RestSessionTask<CancelMatchmakingRequest, CancelMatchmakingResult>
        {
            public CancelMatchmakingTask(IGs2Session session, RestSessionRequestFactory factory, CancelMatchmakingRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(CancelMatchmakingRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "matchmaking")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/gathering/{gatheringName}/user/me";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{gatheringName}", !string.IsNullOrEmpty(request.GatheringName) ? request.GatheringName.ToString() : "null");

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
		public IEnumerator CancelMatchmaking(
                Request.CancelMatchmakingRequest request,
                UnityAction<AsyncResult<Result.CancelMatchmakingResult>> callback
        )
		{
			var task = new CancelMatchmakingTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.CancelMatchmakingResult>(task.Result, task.Error));
        }

		public IFuture<Result.CancelMatchmakingResult> CancelMatchmakingFuture(
                Request.CancelMatchmakingRequest request
        )
		{
			return new CancelMatchmakingTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.CancelMatchmakingResult> CancelMatchmakingAsync(
                Request.CancelMatchmakingRequest request
        )
		{
            AsyncResult<Result.CancelMatchmakingResult> result = null;
			await CancelMatchmaking(
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
		public CancelMatchmakingTask CancelMatchmakingAsync(
                Request.CancelMatchmakingRequest request
        )
		{
			return new CancelMatchmakingTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
			    request
            );
        }
    #endif
#else
		public async Task<Result.CancelMatchmakingResult> CancelMatchmakingAsync(
                Request.CancelMatchmakingRequest request
        )
		{
			var task = new CancelMatchmakingTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class CancelMatchmakingByUserIdTask : Gs2RestSessionTask<CancelMatchmakingByUserIdRequest, CancelMatchmakingByUserIdResult>
        {
            public CancelMatchmakingByUserIdTask(IGs2Session session, RestSessionRequestFactory factory, CancelMatchmakingByUserIdRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(CancelMatchmakingByUserIdRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "matchmaking")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/gathering/{gatheringName}/user/{userId}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{gatheringName}", !string.IsNullOrEmpty(request.GatheringName) ? request.GatheringName.ToString() : "null");
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
		public IEnumerator CancelMatchmakingByUserId(
                Request.CancelMatchmakingByUserIdRequest request,
                UnityAction<AsyncResult<Result.CancelMatchmakingByUserIdResult>> callback
        )
		{
			var task = new CancelMatchmakingByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.CancelMatchmakingByUserIdResult>(task.Result, task.Error));
        }

		public IFuture<Result.CancelMatchmakingByUserIdResult> CancelMatchmakingByUserIdFuture(
                Request.CancelMatchmakingByUserIdRequest request
        )
		{
			return new CancelMatchmakingByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.CancelMatchmakingByUserIdResult> CancelMatchmakingByUserIdAsync(
                Request.CancelMatchmakingByUserIdRequest request
        )
		{
            AsyncResult<Result.CancelMatchmakingByUserIdResult> result = null;
			await CancelMatchmakingByUserId(
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
		public CancelMatchmakingByUserIdTask CancelMatchmakingByUserIdAsync(
                Request.CancelMatchmakingByUserIdRequest request
        )
		{
			return new CancelMatchmakingByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
			    request
            );
        }
    #endif
#else
		public async Task<Result.CancelMatchmakingByUserIdResult> CancelMatchmakingByUserIdAsync(
                Request.CancelMatchmakingByUserIdRequest request
        )
		{
			var task = new CancelMatchmakingByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class DeleteGatheringTask : Gs2RestSessionTask<DeleteGatheringRequest, DeleteGatheringResult>
        {
            public DeleteGatheringTask(IGs2Session session, RestSessionRequestFactory factory, DeleteGatheringRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DeleteGatheringRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "matchmaking")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/gathering/{gatheringName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{gatheringName}", !string.IsNullOrEmpty(request.GatheringName) ? request.GatheringName.ToString() : "null");

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
		public IEnumerator DeleteGathering(
                Request.DeleteGatheringRequest request,
                UnityAction<AsyncResult<Result.DeleteGatheringResult>> callback
        )
		{
			var task = new DeleteGatheringTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.DeleteGatheringResult>(task.Result, task.Error));
        }

		public IFuture<Result.DeleteGatheringResult> DeleteGatheringFuture(
                Request.DeleteGatheringRequest request
        )
		{
			return new DeleteGatheringTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.DeleteGatheringResult> DeleteGatheringAsync(
                Request.DeleteGatheringRequest request
        )
		{
            AsyncResult<Result.DeleteGatheringResult> result = null;
			await DeleteGathering(
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
		public DeleteGatheringTask DeleteGatheringAsync(
                Request.DeleteGatheringRequest request
        )
		{
			return new DeleteGatheringTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
			    request
            );
        }
    #endif
#else
		public async Task<Result.DeleteGatheringResult> DeleteGatheringAsync(
                Request.DeleteGatheringRequest request
        )
		{
			var task = new DeleteGatheringTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class DescribeRatingModelMastersTask : Gs2RestSessionTask<DescribeRatingModelMastersRequest, DescribeRatingModelMastersResult>
        {
            public DescribeRatingModelMastersTask(IGs2Session session, RestSessionRequestFactory factory, DescribeRatingModelMastersRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DescribeRatingModelMastersRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "matchmaking")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/master/rating";

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
		public IEnumerator DescribeRatingModelMasters(
                Request.DescribeRatingModelMastersRequest request,
                UnityAction<AsyncResult<Result.DescribeRatingModelMastersResult>> callback
        )
		{
			var task = new DescribeRatingModelMastersTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.DescribeRatingModelMastersResult>(task.Result, task.Error));
        }

		public IFuture<Result.DescribeRatingModelMastersResult> DescribeRatingModelMastersFuture(
                Request.DescribeRatingModelMastersRequest request
        )
		{
			return new DescribeRatingModelMastersTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.DescribeRatingModelMastersResult> DescribeRatingModelMastersAsync(
                Request.DescribeRatingModelMastersRequest request
        )
		{
            AsyncResult<Result.DescribeRatingModelMastersResult> result = null;
			await DescribeRatingModelMasters(
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
		public DescribeRatingModelMastersTask DescribeRatingModelMastersAsync(
                Request.DescribeRatingModelMastersRequest request
        )
		{
			return new DescribeRatingModelMastersTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
			    request
            );
        }
    #endif
#else
		public async Task<Result.DescribeRatingModelMastersResult> DescribeRatingModelMastersAsync(
                Request.DescribeRatingModelMastersRequest request
        )
		{
			var task = new DescribeRatingModelMastersTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class CreateRatingModelMasterTask : Gs2RestSessionTask<CreateRatingModelMasterRequest, CreateRatingModelMasterResult>
        {
            public CreateRatingModelMasterTask(IGs2Session session, RestSessionRequestFactory factory, CreateRatingModelMasterRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(CreateRatingModelMasterRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "matchmaking")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/master/rating";

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
                if (request.Volatility != null)
                {
                    jsonWriter.WritePropertyName("volatility");
                    jsonWriter.Write(request.Volatility.ToString());
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
		public IEnumerator CreateRatingModelMaster(
                Request.CreateRatingModelMasterRequest request,
                UnityAction<AsyncResult<Result.CreateRatingModelMasterResult>> callback
        )
		{
			var task = new CreateRatingModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.CreateRatingModelMasterResult>(task.Result, task.Error));
        }

		public IFuture<Result.CreateRatingModelMasterResult> CreateRatingModelMasterFuture(
                Request.CreateRatingModelMasterRequest request
        )
		{
			return new CreateRatingModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.CreateRatingModelMasterResult> CreateRatingModelMasterAsync(
                Request.CreateRatingModelMasterRequest request
        )
		{
            AsyncResult<Result.CreateRatingModelMasterResult> result = null;
			await CreateRatingModelMaster(
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
		public CreateRatingModelMasterTask CreateRatingModelMasterAsync(
                Request.CreateRatingModelMasterRequest request
        )
		{
			return new CreateRatingModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
			    request
            );
        }
    #endif
#else
		public async Task<Result.CreateRatingModelMasterResult> CreateRatingModelMasterAsync(
                Request.CreateRatingModelMasterRequest request
        )
		{
			var task = new CreateRatingModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class GetRatingModelMasterTask : Gs2RestSessionTask<GetRatingModelMasterRequest, GetRatingModelMasterResult>
        {
            public GetRatingModelMasterTask(IGs2Session session, RestSessionRequestFactory factory, GetRatingModelMasterRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(GetRatingModelMasterRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "matchmaking")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/master/rating/{ratingName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{ratingName}", !string.IsNullOrEmpty(request.RatingName) ? request.RatingName.ToString() : "null");

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
		public IEnumerator GetRatingModelMaster(
                Request.GetRatingModelMasterRequest request,
                UnityAction<AsyncResult<Result.GetRatingModelMasterResult>> callback
        )
		{
			var task = new GetRatingModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.GetRatingModelMasterResult>(task.Result, task.Error));
        }

		public IFuture<Result.GetRatingModelMasterResult> GetRatingModelMasterFuture(
                Request.GetRatingModelMasterRequest request
        )
		{
			return new GetRatingModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.GetRatingModelMasterResult> GetRatingModelMasterAsync(
                Request.GetRatingModelMasterRequest request
        )
		{
            AsyncResult<Result.GetRatingModelMasterResult> result = null;
			await GetRatingModelMaster(
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
		public GetRatingModelMasterTask GetRatingModelMasterAsync(
                Request.GetRatingModelMasterRequest request
        )
		{
			return new GetRatingModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
			    request
            );
        }
    #endif
#else
		public async Task<Result.GetRatingModelMasterResult> GetRatingModelMasterAsync(
                Request.GetRatingModelMasterRequest request
        )
		{
			var task = new GetRatingModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class UpdateRatingModelMasterTask : Gs2RestSessionTask<UpdateRatingModelMasterRequest, UpdateRatingModelMasterResult>
        {
            public UpdateRatingModelMasterTask(IGs2Session session, RestSessionRequestFactory factory, UpdateRatingModelMasterRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(UpdateRatingModelMasterRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "matchmaking")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/master/rating/{ratingName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{ratingName}", !string.IsNullOrEmpty(request.RatingName) ? request.RatingName.ToString() : "null");

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
                if (request.Volatility != null)
                {
                    jsonWriter.WritePropertyName("volatility");
                    jsonWriter.Write(request.Volatility.ToString());
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
		public IEnumerator UpdateRatingModelMaster(
                Request.UpdateRatingModelMasterRequest request,
                UnityAction<AsyncResult<Result.UpdateRatingModelMasterResult>> callback
        )
		{
			var task = new UpdateRatingModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.UpdateRatingModelMasterResult>(task.Result, task.Error));
        }

		public IFuture<Result.UpdateRatingModelMasterResult> UpdateRatingModelMasterFuture(
                Request.UpdateRatingModelMasterRequest request
        )
		{
			return new UpdateRatingModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.UpdateRatingModelMasterResult> UpdateRatingModelMasterAsync(
                Request.UpdateRatingModelMasterRequest request
        )
		{
            AsyncResult<Result.UpdateRatingModelMasterResult> result = null;
			await UpdateRatingModelMaster(
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
		public UpdateRatingModelMasterTask UpdateRatingModelMasterAsync(
                Request.UpdateRatingModelMasterRequest request
        )
		{
			return new UpdateRatingModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
			    request
            );
        }
    #endif
#else
		public async Task<Result.UpdateRatingModelMasterResult> UpdateRatingModelMasterAsync(
                Request.UpdateRatingModelMasterRequest request
        )
		{
			var task = new UpdateRatingModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class DeleteRatingModelMasterTask : Gs2RestSessionTask<DeleteRatingModelMasterRequest, DeleteRatingModelMasterResult>
        {
            public DeleteRatingModelMasterTask(IGs2Session session, RestSessionRequestFactory factory, DeleteRatingModelMasterRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DeleteRatingModelMasterRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "matchmaking")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/master/rating/{ratingName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{ratingName}", !string.IsNullOrEmpty(request.RatingName) ? request.RatingName.ToString() : "null");

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
		public IEnumerator DeleteRatingModelMaster(
                Request.DeleteRatingModelMasterRequest request,
                UnityAction<AsyncResult<Result.DeleteRatingModelMasterResult>> callback
        )
		{
			var task = new DeleteRatingModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.DeleteRatingModelMasterResult>(task.Result, task.Error));
        }

		public IFuture<Result.DeleteRatingModelMasterResult> DeleteRatingModelMasterFuture(
                Request.DeleteRatingModelMasterRequest request
        )
		{
			return new DeleteRatingModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.DeleteRatingModelMasterResult> DeleteRatingModelMasterAsync(
                Request.DeleteRatingModelMasterRequest request
        )
		{
            AsyncResult<Result.DeleteRatingModelMasterResult> result = null;
			await DeleteRatingModelMaster(
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
		public DeleteRatingModelMasterTask DeleteRatingModelMasterAsync(
                Request.DeleteRatingModelMasterRequest request
        )
		{
			return new DeleteRatingModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
			    request
            );
        }
    #endif
#else
		public async Task<Result.DeleteRatingModelMasterResult> DeleteRatingModelMasterAsync(
                Request.DeleteRatingModelMasterRequest request
        )
		{
			var task = new DeleteRatingModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class DescribeRatingModelsTask : Gs2RestSessionTask<DescribeRatingModelsRequest, DescribeRatingModelsResult>
        {
            public DescribeRatingModelsTask(IGs2Session session, RestSessionRequestFactory factory, DescribeRatingModelsRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DescribeRatingModelsRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "matchmaking")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/rating";

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
		public IEnumerator DescribeRatingModels(
                Request.DescribeRatingModelsRequest request,
                UnityAction<AsyncResult<Result.DescribeRatingModelsResult>> callback
        )
		{
			var task = new DescribeRatingModelsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.DescribeRatingModelsResult>(task.Result, task.Error));
        }

		public IFuture<Result.DescribeRatingModelsResult> DescribeRatingModelsFuture(
                Request.DescribeRatingModelsRequest request
        )
		{
			return new DescribeRatingModelsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.DescribeRatingModelsResult> DescribeRatingModelsAsync(
                Request.DescribeRatingModelsRequest request
        )
		{
            AsyncResult<Result.DescribeRatingModelsResult> result = null;
			await DescribeRatingModels(
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
		public DescribeRatingModelsTask DescribeRatingModelsAsync(
                Request.DescribeRatingModelsRequest request
        )
		{
			return new DescribeRatingModelsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
			    request
            );
        }
    #endif
#else
		public async Task<Result.DescribeRatingModelsResult> DescribeRatingModelsAsync(
                Request.DescribeRatingModelsRequest request
        )
		{
			var task = new DescribeRatingModelsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class GetRatingModelTask : Gs2RestSessionTask<GetRatingModelRequest, GetRatingModelResult>
        {
            public GetRatingModelTask(IGs2Session session, RestSessionRequestFactory factory, GetRatingModelRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(GetRatingModelRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "matchmaking")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/rating/{ratingName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{ratingName}", !string.IsNullOrEmpty(request.RatingName) ? request.RatingName.ToString() : "null");

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
		public IEnumerator GetRatingModel(
                Request.GetRatingModelRequest request,
                UnityAction<AsyncResult<Result.GetRatingModelResult>> callback
        )
		{
			var task = new GetRatingModelTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.GetRatingModelResult>(task.Result, task.Error));
        }

		public IFuture<Result.GetRatingModelResult> GetRatingModelFuture(
                Request.GetRatingModelRequest request
        )
		{
			return new GetRatingModelTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.GetRatingModelResult> GetRatingModelAsync(
                Request.GetRatingModelRequest request
        )
		{
            AsyncResult<Result.GetRatingModelResult> result = null;
			await GetRatingModel(
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
		public GetRatingModelTask GetRatingModelAsync(
                Request.GetRatingModelRequest request
        )
		{
			return new GetRatingModelTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
			    request
            );
        }
    #endif
#else
		public async Task<Result.GetRatingModelResult> GetRatingModelAsync(
                Request.GetRatingModelRequest request
        )
		{
			var task = new GetRatingModelTask(
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
                    .Replace("{service}", "matchmaking")
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

		public IFuture<Result.ExportMasterResult> ExportMasterFuture(
                Request.ExportMasterRequest request
        )
		{
			return new ExportMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
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
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
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


        public class GetCurrentRatingModelMasterTask : Gs2RestSessionTask<GetCurrentRatingModelMasterRequest, GetCurrentRatingModelMasterResult>
        {
            public GetCurrentRatingModelMasterTask(IGs2Session session, RestSessionRequestFactory factory, GetCurrentRatingModelMasterRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(GetCurrentRatingModelMasterRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "matchmaking")
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
		public IEnumerator GetCurrentRatingModelMaster(
                Request.GetCurrentRatingModelMasterRequest request,
                UnityAction<AsyncResult<Result.GetCurrentRatingModelMasterResult>> callback
        )
		{
			var task = new GetCurrentRatingModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.GetCurrentRatingModelMasterResult>(task.Result, task.Error));
        }

		public IFuture<Result.GetCurrentRatingModelMasterResult> GetCurrentRatingModelMasterFuture(
                Request.GetCurrentRatingModelMasterRequest request
        )
		{
			return new GetCurrentRatingModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.GetCurrentRatingModelMasterResult> GetCurrentRatingModelMasterAsync(
                Request.GetCurrentRatingModelMasterRequest request
        )
		{
            AsyncResult<Result.GetCurrentRatingModelMasterResult> result = null;
			await GetCurrentRatingModelMaster(
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
		public GetCurrentRatingModelMasterTask GetCurrentRatingModelMasterAsync(
                Request.GetCurrentRatingModelMasterRequest request
        )
		{
			return new GetCurrentRatingModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
			    request
            );
        }
    #endif
#else
		public async Task<Result.GetCurrentRatingModelMasterResult> GetCurrentRatingModelMasterAsync(
                Request.GetCurrentRatingModelMasterRequest request
        )
		{
			var task = new GetCurrentRatingModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class UpdateCurrentRatingModelMasterTask : Gs2RestSessionTask<UpdateCurrentRatingModelMasterRequest, UpdateCurrentRatingModelMasterResult>
        {
            public UpdateCurrentRatingModelMasterTask(IGs2Session session, RestSessionRequestFactory factory, UpdateCurrentRatingModelMasterRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(UpdateCurrentRatingModelMasterRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "matchmaking")
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
		public IEnumerator UpdateCurrentRatingModelMaster(
                Request.UpdateCurrentRatingModelMasterRequest request,
                UnityAction<AsyncResult<Result.UpdateCurrentRatingModelMasterResult>> callback
        )
		{
			var task = new UpdateCurrentRatingModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.UpdateCurrentRatingModelMasterResult>(task.Result, task.Error));
        }

		public IFuture<Result.UpdateCurrentRatingModelMasterResult> UpdateCurrentRatingModelMasterFuture(
                Request.UpdateCurrentRatingModelMasterRequest request
        )
		{
			return new UpdateCurrentRatingModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.UpdateCurrentRatingModelMasterResult> UpdateCurrentRatingModelMasterAsync(
                Request.UpdateCurrentRatingModelMasterRequest request
        )
		{
            AsyncResult<Result.UpdateCurrentRatingModelMasterResult> result = null;
			await UpdateCurrentRatingModelMaster(
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
		public UpdateCurrentRatingModelMasterTask UpdateCurrentRatingModelMasterAsync(
                Request.UpdateCurrentRatingModelMasterRequest request
        )
		{
			return new UpdateCurrentRatingModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
			    request
            );
        }
    #endif
#else
		public async Task<Result.UpdateCurrentRatingModelMasterResult> UpdateCurrentRatingModelMasterAsync(
                Request.UpdateCurrentRatingModelMasterRequest request
        )
		{
			var task = new UpdateCurrentRatingModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class UpdateCurrentRatingModelMasterFromGitHubTask : Gs2RestSessionTask<UpdateCurrentRatingModelMasterFromGitHubRequest, UpdateCurrentRatingModelMasterFromGitHubResult>
        {
            public UpdateCurrentRatingModelMasterFromGitHubTask(IGs2Session session, RestSessionRequestFactory factory, UpdateCurrentRatingModelMasterFromGitHubRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(UpdateCurrentRatingModelMasterFromGitHubRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "matchmaking")
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
		public IEnumerator UpdateCurrentRatingModelMasterFromGitHub(
                Request.UpdateCurrentRatingModelMasterFromGitHubRequest request,
                UnityAction<AsyncResult<Result.UpdateCurrentRatingModelMasterFromGitHubResult>> callback
        )
		{
			var task = new UpdateCurrentRatingModelMasterFromGitHubTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.UpdateCurrentRatingModelMasterFromGitHubResult>(task.Result, task.Error));
        }

		public IFuture<Result.UpdateCurrentRatingModelMasterFromGitHubResult> UpdateCurrentRatingModelMasterFromGitHubFuture(
                Request.UpdateCurrentRatingModelMasterFromGitHubRequest request
        )
		{
			return new UpdateCurrentRatingModelMasterFromGitHubTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.UpdateCurrentRatingModelMasterFromGitHubResult> UpdateCurrentRatingModelMasterFromGitHubAsync(
                Request.UpdateCurrentRatingModelMasterFromGitHubRequest request
        )
		{
            AsyncResult<Result.UpdateCurrentRatingModelMasterFromGitHubResult> result = null;
			await UpdateCurrentRatingModelMasterFromGitHub(
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
		public UpdateCurrentRatingModelMasterFromGitHubTask UpdateCurrentRatingModelMasterFromGitHubAsync(
                Request.UpdateCurrentRatingModelMasterFromGitHubRequest request
        )
		{
			return new UpdateCurrentRatingModelMasterFromGitHubTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
			    request
            );
        }
    #endif
#else
		public async Task<Result.UpdateCurrentRatingModelMasterFromGitHubResult> UpdateCurrentRatingModelMasterFromGitHubAsync(
                Request.UpdateCurrentRatingModelMasterFromGitHubRequest request
        )
		{
			var task = new UpdateCurrentRatingModelMasterFromGitHubTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class DescribeRatingsTask : Gs2RestSessionTask<DescribeRatingsRequest, DescribeRatingsResult>
        {
            public DescribeRatingsTask(IGs2Session session, RestSessionRequestFactory factory, DescribeRatingsRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DescribeRatingsRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "matchmaking")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/me/rating";

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
		public IEnumerator DescribeRatings(
                Request.DescribeRatingsRequest request,
                UnityAction<AsyncResult<Result.DescribeRatingsResult>> callback
        )
		{
			var task = new DescribeRatingsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.DescribeRatingsResult>(task.Result, task.Error));
        }

		public IFuture<Result.DescribeRatingsResult> DescribeRatingsFuture(
                Request.DescribeRatingsRequest request
        )
		{
			return new DescribeRatingsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.DescribeRatingsResult> DescribeRatingsAsync(
                Request.DescribeRatingsRequest request
        )
		{
            AsyncResult<Result.DescribeRatingsResult> result = null;
			await DescribeRatings(
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
		public DescribeRatingsTask DescribeRatingsAsync(
                Request.DescribeRatingsRequest request
        )
		{
			return new DescribeRatingsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
			    request
            );
        }
    #endif
#else
		public async Task<Result.DescribeRatingsResult> DescribeRatingsAsync(
                Request.DescribeRatingsRequest request
        )
		{
			var task = new DescribeRatingsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class DescribeRatingsByUserIdTask : Gs2RestSessionTask<DescribeRatingsByUserIdRequest, DescribeRatingsByUserIdResult>
        {
            public DescribeRatingsByUserIdTask(IGs2Session session, RestSessionRequestFactory factory, DescribeRatingsByUserIdRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DescribeRatingsByUserIdRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "matchmaking")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/{userId}/rating";

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
		public IEnumerator DescribeRatingsByUserId(
                Request.DescribeRatingsByUserIdRequest request,
                UnityAction<AsyncResult<Result.DescribeRatingsByUserIdResult>> callback
        )
		{
			var task = new DescribeRatingsByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.DescribeRatingsByUserIdResult>(task.Result, task.Error));
        }

		public IFuture<Result.DescribeRatingsByUserIdResult> DescribeRatingsByUserIdFuture(
                Request.DescribeRatingsByUserIdRequest request
        )
		{
			return new DescribeRatingsByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.DescribeRatingsByUserIdResult> DescribeRatingsByUserIdAsync(
                Request.DescribeRatingsByUserIdRequest request
        )
		{
            AsyncResult<Result.DescribeRatingsByUserIdResult> result = null;
			await DescribeRatingsByUserId(
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
		public DescribeRatingsByUserIdTask DescribeRatingsByUserIdAsync(
                Request.DescribeRatingsByUserIdRequest request
        )
		{
			return new DescribeRatingsByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
			    request
            );
        }
    #endif
#else
		public async Task<Result.DescribeRatingsByUserIdResult> DescribeRatingsByUserIdAsync(
                Request.DescribeRatingsByUserIdRequest request
        )
		{
			var task = new DescribeRatingsByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class GetRatingTask : Gs2RestSessionTask<GetRatingRequest, GetRatingResult>
        {
            public GetRatingTask(IGs2Session session, RestSessionRequestFactory factory, GetRatingRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(GetRatingRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "matchmaking")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/me/rating/{ratingName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{ratingName}", !string.IsNullOrEmpty(request.RatingName) ? request.RatingName.ToString() : "null");

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
		public IEnumerator GetRating(
                Request.GetRatingRequest request,
                UnityAction<AsyncResult<Result.GetRatingResult>> callback
        )
		{
			var task = new GetRatingTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.GetRatingResult>(task.Result, task.Error));
        }

		public IFuture<Result.GetRatingResult> GetRatingFuture(
                Request.GetRatingRequest request
        )
		{
			return new GetRatingTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.GetRatingResult> GetRatingAsync(
                Request.GetRatingRequest request
        )
		{
            AsyncResult<Result.GetRatingResult> result = null;
			await GetRating(
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
		public GetRatingTask GetRatingAsync(
                Request.GetRatingRequest request
        )
		{
			return new GetRatingTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
			    request
            );
        }
    #endif
#else
		public async Task<Result.GetRatingResult> GetRatingAsync(
                Request.GetRatingRequest request
        )
		{
			var task = new GetRatingTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class GetRatingByUserIdTask : Gs2RestSessionTask<GetRatingByUserIdRequest, GetRatingByUserIdResult>
        {
            public GetRatingByUserIdTask(IGs2Session session, RestSessionRequestFactory factory, GetRatingByUserIdRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(GetRatingByUserIdRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "matchmaking")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/{userId}/rating/{ratingName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{userId}", !string.IsNullOrEmpty(request.UserId) ? request.UserId.ToString() : "null");
                url = url.Replace("{ratingName}", !string.IsNullOrEmpty(request.RatingName) ? request.RatingName.ToString() : "null");

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
		public IEnumerator GetRatingByUserId(
                Request.GetRatingByUserIdRequest request,
                UnityAction<AsyncResult<Result.GetRatingByUserIdResult>> callback
        )
		{
			var task = new GetRatingByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.GetRatingByUserIdResult>(task.Result, task.Error));
        }

		public IFuture<Result.GetRatingByUserIdResult> GetRatingByUserIdFuture(
                Request.GetRatingByUserIdRequest request
        )
		{
			return new GetRatingByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.GetRatingByUserIdResult> GetRatingByUserIdAsync(
                Request.GetRatingByUserIdRequest request
        )
		{
            AsyncResult<Result.GetRatingByUserIdResult> result = null;
			await GetRatingByUserId(
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
		public GetRatingByUserIdTask GetRatingByUserIdAsync(
                Request.GetRatingByUserIdRequest request
        )
		{
			return new GetRatingByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
			    request
            );
        }
    #endif
#else
		public async Task<Result.GetRatingByUserIdResult> GetRatingByUserIdAsync(
                Request.GetRatingByUserIdRequest request
        )
		{
			var task = new GetRatingByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class PutResultTask : Gs2RestSessionTask<PutResultRequest, PutResultResult>
        {
            public PutResultTask(IGs2Session session, RestSessionRequestFactory factory, PutResultRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(PutResultRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "matchmaking")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/rating/{ratingName}/vote";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{ratingName}", !string.IsNullOrEmpty(request.RatingName) ? request.RatingName.ToString() : "null");

                var sessionRequest = Factory.Post(url);

                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);
                jsonWriter.WriteObjectStart();
                if (request.GameResults != null)
                {
                    jsonWriter.WritePropertyName("gameResults");
                    jsonWriter.WriteArrayStart();
                    foreach(var item in request.GameResults)
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
		public IEnumerator PutResult(
                Request.PutResultRequest request,
                UnityAction<AsyncResult<Result.PutResultResult>> callback
        )
		{
			var task = new PutResultTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.PutResultResult>(task.Result, task.Error));
        }

		public IFuture<Result.PutResultResult> PutResultFuture(
                Request.PutResultRequest request
        )
		{
			return new PutResultTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.PutResultResult> PutResultAsync(
                Request.PutResultRequest request
        )
		{
            AsyncResult<Result.PutResultResult> result = null;
			await PutResult(
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
		public PutResultTask PutResultAsync(
                Request.PutResultRequest request
        )
		{
			return new PutResultTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
			    request
            );
        }
    #endif
#else
		public async Task<Result.PutResultResult> PutResultAsync(
                Request.PutResultRequest request
        )
		{
			var task = new PutResultTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class DeleteRatingTask : Gs2RestSessionTask<DeleteRatingRequest, DeleteRatingResult>
        {
            public DeleteRatingTask(IGs2Session session, RestSessionRequestFactory factory, DeleteRatingRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DeleteRatingRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "matchmaking")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/{userId}/rating/{ratingName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{userId}", !string.IsNullOrEmpty(request.UserId) ? request.UserId.ToString() : "null");
                url = url.Replace("{ratingName}", !string.IsNullOrEmpty(request.RatingName) ? request.RatingName.ToString() : "null");

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
		public IEnumerator DeleteRating(
                Request.DeleteRatingRequest request,
                UnityAction<AsyncResult<Result.DeleteRatingResult>> callback
        )
		{
			var task = new DeleteRatingTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.DeleteRatingResult>(task.Result, task.Error));
        }

		public IFuture<Result.DeleteRatingResult> DeleteRatingFuture(
                Request.DeleteRatingRequest request
        )
		{
			return new DeleteRatingTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.DeleteRatingResult> DeleteRatingAsync(
                Request.DeleteRatingRequest request
        )
		{
            AsyncResult<Result.DeleteRatingResult> result = null;
			await DeleteRating(
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
		public DeleteRatingTask DeleteRatingAsync(
                Request.DeleteRatingRequest request
        )
		{
			return new DeleteRatingTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
			    request
            );
        }
    #endif
#else
		public async Task<Result.DeleteRatingResult> DeleteRatingAsync(
                Request.DeleteRatingRequest request
        )
		{
			var task = new DeleteRatingTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class GetBallotTask : Gs2RestSessionTask<GetBallotRequest, GetBallotResult>
        {
            public GetBallotTask(IGs2Session session, RestSessionRequestFactory factory, GetBallotRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(GetBallotRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "matchmaking")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/me/vote/{ratingName}/{gatheringName}/ballot";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{ratingName}", !string.IsNullOrEmpty(request.RatingName) ? request.RatingName.ToString() : "null");
                url = url.Replace("{gatheringName}", !string.IsNullOrEmpty(request.GatheringName) ? request.GatheringName.ToString() : "null");

                var sessionRequest = Factory.Post(url);

                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);
                jsonWriter.WriteObjectStart();
                if (request.NumberOfPlayer != null)
                {
                    jsonWriter.WritePropertyName("numberOfPlayer");
                    jsonWriter.Write(request.NumberOfPlayer.ToString());
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
		public IEnumerator GetBallot(
                Request.GetBallotRequest request,
                UnityAction<AsyncResult<Result.GetBallotResult>> callback
        )
		{
			var task = new GetBallotTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.GetBallotResult>(task.Result, task.Error));
        }

		public IFuture<Result.GetBallotResult> GetBallotFuture(
                Request.GetBallotRequest request
        )
		{
			return new GetBallotTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.GetBallotResult> GetBallotAsync(
                Request.GetBallotRequest request
        )
		{
            AsyncResult<Result.GetBallotResult> result = null;
			await GetBallot(
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
		public GetBallotTask GetBallotAsync(
                Request.GetBallotRequest request
        )
		{
			return new GetBallotTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
			    request
            );
        }
    #endif
#else
		public async Task<Result.GetBallotResult> GetBallotAsync(
                Request.GetBallotRequest request
        )
		{
			var task = new GetBallotTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class GetBallotByUserIdTask : Gs2RestSessionTask<GetBallotByUserIdRequest, GetBallotByUserIdResult>
        {
            public GetBallotByUserIdTask(IGs2Session session, RestSessionRequestFactory factory, GetBallotByUserIdRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(GetBallotByUserIdRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "matchmaking")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/{userId}/vote/{ratingName}/{gatheringName}/ballot";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{ratingName}", !string.IsNullOrEmpty(request.RatingName) ? request.RatingName.ToString() : "null");
                url = url.Replace("{gatheringName}", !string.IsNullOrEmpty(request.GatheringName) ? request.GatheringName.ToString() : "null");
                url = url.Replace("{userId}", !string.IsNullOrEmpty(request.UserId) ? request.UserId.ToString() : "null");

                var sessionRequest = Factory.Post(url);

                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);
                jsonWriter.WriteObjectStart();
                if (request.NumberOfPlayer != null)
                {
                    jsonWriter.WritePropertyName("numberOfPlayer");
                    jsonWriter.Write(request.NumberOfPlayer.ToString());
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
		public IEnumerator GetBallotByUserId(
                Request.GetBallotByUserIdRequest request,
                UnityAction<AsyncResult<Result.GetBallotByUserIdResult>> callback
        )
		{
			var task = new GetBallotByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.GetBallotByUserIdResult>(task.Result, task.Error));
        }

		public IFuture<Result.GetBallotByUserIdResult> GetBallotByUserIdFuture(
                Request.GetBallotByUserIdRequest request
        )
		{
			return new GetBallotByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.GetBallotByUserIdResult> GetBallotByUserIdAsync(
                Request.GetBallotByUserIdRequest request
        )
		{
            AsyncResult<Result.GetBallotByUserIdResult> result = null;
			await GetBallotByUserId(
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
		public GetBallotByUserIdTask GetBallotByUserIdAsync(
                Request.GetBallotByUserIdRequest request
        )
		{
			return new GetBallotByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
			    request
            );
        }
    #endif
#else
		public async Task<Result.GetBallotByUserIdResult> GetBallotByUserIdAsync(
                Request.GetBallotByUserIdRequest request
        )
		{
			var task = new GetBallotByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class VoteTask : Gs2RestSessionTask<VoteRequest, VoteResult>
        {
            public VoteTask(IGs2Session session, RestSessionRequestFactory factory, VoteRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(VoteRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "matchmaking")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/action/vote";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");

                var sessionRequest = Factory.Post(url);

                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);
                jsonWriter.WriteObjectStart();
                if (request.BallotBody != null)
                {
                    jsonWriter.WritePropertyName("ballotBody");
                    jsonWriter.Write(request.BallotBody);
                }
                if (request.BallotSignature != null)
                {
                    jsonWriter.WritePropertyName("ballotSignature");
                    jsonWriter.Write(request.BallotSignature);
                }
                if (request.GameResults != null)
                {
                    jsonWriter.WritePropertyName("gameResults");
                    jsonWriter.WriteArrayStart();
                    foreach(var item in request.GameResults)
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

                AddHeader(
                    Session.Credential,
                    sessionRequest
                );

                return sessionRequest;
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator Vote(
                Request.VoteRequest request,
                UnityAction<AsyncResult<Result.VoteResult>> callback
        )
		{
			var task = new VoteTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.VoteResult>(task.Result, task.Error));
        }

		public IFuture<Result.VoteResult> VoteFuture(
                Request.VoteRequest request
        )
		{
			return new VoteTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.VoteResult> VoteAsync(
                Request.VoteRequest request
        )
		{
            AsyncResult<Result.VoteResult> result = null;
			await Vote(
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
		public VoteTask VoteAsync(
                Request.VoteRequest request
        )
		{
			return new VoteTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
			    request
            );
        }
    #endif
#else
		public async Task<Result.VoteResult> VoteAsync(
                Request.VoteRequest request
        )
		{
			var task = new VoteTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class VoteMultipleTask : Gs2RestSessionTask<VoteMultipleRequest, VoteMultipleResult>
        {
            public VoteMultipleTask(IGs2Session session, RestSessionRequestFactory factory, VoteMultipleRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(VoteMultipleRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "matchmaking")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/action/vote/multiple";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");

                var sessionRequest = Factory.Post(url);

                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);
                jsonWriter.WriteObjectStart();
                if (request.SignedBallots != null)
                {
                    jsonWriter.WritePropertyName("signedBallots");
                    jsonWriter.WriteArrayStart();
                    foreach(var item in request.SignedBallots)
                    {
                        item.WriteJson(jsonWriter);
                    }
                    jsonWriter.WriteArrayEnd();
                }
                if (request.GameResults != null)
                {
                    jsonWriter.WritePropertyName("gameResults");
                    jsonWriter.WriteArrayStart();
                    foreach(var item in request.GameResults)
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

                AddHeader(
                    Session.Credential,
                    sessionRequest
                );

                return sessionRequest;
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator VoteMultiple(
                Request.VoteMultipleRequest request,
                UnityAction<AsyncResult<Result.VoteMultipleResult>> callback
        )
		{
			var task = new VoteMultipleTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.VoteMultipleResult>(task.Result, task.Error));
        }

		public IFuture<Result.VoteMultipleResult> VoteMultipleFuture(
                Request.VoteMultipleRequest request
        )
		{
			return new VoteMultipleTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.VoteMultipleResult> VoteMultipleAsync(
                Request.VoteMultipleRequest request
        )
		{
            AsyncResult<Result.VoteMultipleResult> result = null;
			await VoteMultiple(
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
		public VoteMultipleTask VoteMultipleAsync(
                Request.VoteMultipleRequest request
        )
		{
			return new VoteMultipleTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
			    request
            );
        }
    #endif
#else
		public async Task<Result.VoteMultipleResult> VoteMultipleAsync(
                Request.VoteMultipleRequest request
        )
		{
			var task = new VoteMultipleTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class CommitVoteTask : Gs2RestSessionTask<CommitVoteRequest, CommitVoteResult>
        {
            public CommitVoteTask(IGs2Session session, RestSessionRequestFactory factory, CommitVoteRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(CommitVoteRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "matchmaking")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/vote/{ratingName}/{gatheringName}/action/vote/commit";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{ratingName}", !string.IsNullOrEmpty(request.RatingName) ? request.RatingName.ToString() : "null");
                url = url.Replace("{gatheringName}", !string.IsNullOrEmpty(request.GatheringName) ? request.GatheringName.ToString() : "null");

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
		public IEnumerator CommitVote(
                Request.CommitVoteRequest request,
                UnityAction<AsyncResult<Result.CommitVoteResult>> callback
        )
		{
			var task = new CommitVoteTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.CommitVoteResult>(task.Result, task.Error));
        }

		public IFuture<Result.CommitVoteResult> CommitVoteFuture(
                Request.CommitVoteRequest request
        )
		{
			return new CommitVoteTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.CommitVoteResult> CommitVoteAsync(
                Request.CommitVoteRequest request
        )
		{
            AsyncResult<Result.CommitVoteResult> result = null;
			await CommitVote(
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
		public CommitVoteTask CommitVoteAsync(
                Request.CommitVoteRequest request
        )
		{
			return new CommitVoteTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
			    request
            );
        }
    #endif
#else
		public async Task<Result.CommitVoteResult> CommitVoteAsync(
                Request.CommitVoteRequest request
        )
		{
			var task = new CommitVoteTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif
	}
}