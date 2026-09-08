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
                if (request.TransactionSetting != null)
                {
                    jsonWriter.WritePropertyName("transactionSetting");
                    request.TransactionSetting.WriteJson(jsonWriter);
                }
                if (request.TransactionSettingV2 != null)
                {
                    jsonWriter.WritePropertyName("transactionSettingV2");
                    request.TransactionSettingV2.WriteJson(jsonWriter);
                }
                if (request.EnableRating != null)
                {
                    jsonWriter.WritePropertyName("enableRating");
                    jsonWriter.Write(request.EnableRating.ToString());
                }
                if (request.EnableDisconnectDetection != null)
                {
                    jsonWriter.WritePropertyName("enableDisconnectDetection");
                    jsonWriter.Write(request.EnableDisconnectDetection);
                }
                if (request.DisconnectDetectionTimeoutSeconds != null)
                {
                    jsonWriter.WritePropertyName("disconnectDetectionTimeoutSeconds");
                    jsonWriter.Write(request.DisconnectDetectionTimeoutSeconds.ToString());
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
                if (request.EnableCollaborateSeasonRating != null)
                {
                    jsonWriter.WritePropertyName("enableCollaborateSeasonRating");
                    jsonWriter.Write(request.EnableCollaborateSeasonRating);
                }
                if (request.CollaborateSeasonRatingNamespaceId != null)
                {
                    jsonWriter.WritePropertyName("collaborateSeasonRatingNamespaceId");
                    jsonWriter.Write(request.CollaborateSeasonRatingNamespaceId);
                }
                if (request.CollaborateSeasonRatingTtl != null)
                {
                    jsonWriter.WritePropertyName("collaborateSeasonRatingTtl");
                    jsonWriter.Write(request.CollaborateSeasonRatingTtl.ToString());
                }
                if (request.ChangeRatingScript != null)
                {
                    jsonWriter.WritePropertyName("changeRatingScript");
                    request.ChangeRatingScript.WriteJson(jsonWriter);
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
                if (request.ChangeRatingNotification != null)
                {
                    jsonWriter.WritePropertyName("changeRatingNotification");
                    request.ChangeRatingNotification.WriteJson(jsonWriter);
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
                    .Replace("{service}", "matchmaking")
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
                    .Replace("{service}", "matchmaking")
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
                if (request.TransactionSetting != null)
                {
                    jsonWriter.WritePropertyName("transactionSetting");
                    request.TransactionSetting.WriteJson(jsonWriter);
                }
                if (request.TransactionSettingV2 != null)
                {
                    jsonWriter.WritePropertyName("transactionSettingV2");
                    request.TransactionSettingV2.WriteJson(jsonWriter);
                }
                if (request.EnableRating != null)
                {
                    jsonWriter.WritePropertyName("enableRating");
                    jsonWriter.Write(request.EnableRating.ToString());
                }
                if (request.EnableDisconnectDetection != null)
                {
                    jsonWriter.WritePropertyName("enableDisconnectDetection");
                    jsonWriter.Write(request.EnableDisconnectDetection);
                }
                if (request.DisconnectDetectionTimeoutSeconds != null)
                {
                    jsonWriter.WritePropertyName("disconnectDetectionTimeoutSeconds");
                    jsonWriter.Write(request.DisconnectDetectionTimeoutSeconds.ToString());
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
                if (request.EnableCollaborateSeasonRating != null)
                {
                    jsonWriter.WritePropertyName("enableCollaborateSeasonRating");
                    jsonWriter.Write(request.EnableCollaborateSeasonRating);
                }
                if (request.CollaborateSeasonRatingNamespaceId != null)
                {
                    jsonWriter.WritePropertyName("collaborateSeasonRatingNamespaceId");
                    jsonWriter.Write(request.CollaborateSeasonRatingNamespaceId);
                }
                if (request.CollaborateSeasonRatingTtl != null)
                {
                    jsonWriter.WritePropertyName("collaborateSeasonRatingTtl");
                    jsonWriter.Write(request.CollaborateSeasonRatingTtl.ToString());
                }
                if (request.ChangeRatingScript != null)
                {
                    jsonWriter.WritePropertyName("changeRatingScript");
                    request.ChangeRatingScript.WriteJson(jsonWriter);
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
                if (request.ChangeRatingNotification != null)
                {
                    jsonWriter.WritePropertyName("changeRatingNotification");
                    request.ChangeRatingNotification.WriteJson(jsonWriter);
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
                    .Replace("{service}", "matchmaking")
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
                    .Replace("{service}", "matchmaking")
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


        public class DumpUserDataByUserIdTask : Gs2RestSessionTask<DumpUserDataByUserIdRequest, DumpUserDataByUserIdResult>
        {
            public DumpUserDataByUserIdTask(IGs2Session session, RestSessionRequestFactory factory, DumpUserDataByUserIdRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DumpUserDataByUserIdRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "matchmaking")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/system/dump/user/{userId}";

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
		public IEnumerator DumpUserDataByUserId(
                Request.DumpUserDataByUserIdRequest request,
                UnityAction<AsyncResult<Result.DumpUserDataByUserIdResult>> callback
        ) =>
            new DumpUserDataByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.DumpUserDataByUserIdResult> DumpUserDataByUserIdFuture(
                Request.DumpUserDataByUserIdRequest request
        ) =>
            new DumpUserDataByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.DumpUserDataByUserIdResult> DumpUserDataByUserIdAsync(
                Request.DumpUserDataByUserIdRequest request
        ) =>
            new DumpUserDataByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.DumpUserDataByUserIdResult>();
    #else
		public DumpUserDataByUserIdTask DumpUserDataByUserIdAsync(
                Request.DumpUserDataByUserIdRequest request
        )
		{
			return new DumpUserDataByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.DumpUserDataByUserIdResult> DumpUserDataByUserIdAsync(
                Request.DumpUserDataByUserIdRequest request
        ) =>
            new DumpUserDataByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class CheckDumpUserDataByUserIdTask : Gs2RestSessionTask<CheckDumpUserDataByUserIdRequest, CheckDumpUserDataByUserIdResult>
        {
            public CheckDumpUserDataByUserIdTask(IGs2Session session, RestSessionRequestFactory factory, CheckDumpUserDataByUserIdRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(CheckDumpUserDataByUserIdRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "matchmaking")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/system/dump/user/{userId}";

                url = url.Replace("{userId}", !string.IsNullOrEmpty(request.UserId) ? request.UserId.ToString() : "null");

                var sessionRequest = Factory.Get(url);
                if (request.ContextStack != null)
                {
                    sessionRequest.AddQueryString("contextStack", request.ContextStack);
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
		public IEnumerator CheckDumpUserDataByUserId(
                Request.CheckDumpUserDataByUserIdRequest request,
                UnityAction<AsyncResult<Result.CheckDumpUserDataByUserIdResult>> callback
        ) =>
            new CheckDumpUserDataByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.CheckDumpUserDataByUserIdResult> CheckDumpUserDataByUserIdFuture(
                Request.CheckDumpUserDataByUserIdRequest request
        ) =>
            new CheckDumpUserDataByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.CheckDumpUserDataByUserIdResult> CheckDumpUserDataByUserIdAsync(
                Request.CheckDumpUserDataByUserIdRequest request
        ) =>
            new CheckDumpUserDataByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.CheckDumpUserDataByUserIdResult>();
    #else
		public CheckDumpUserDataByUserIdTask CheckDumpUserDataByUserIdAsync(
                Request.CheckDumpUserDataByUserIdRequest request
        )
		{
			return new CheckDumpUserDataByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.CheckDumpUserDataByUserIdResult> CheckDumpUserDataByUserIdAsync(
                Request.CheckDumpUserDataByUserIdRequest request
        ) =>
            new CheckDumpUserDataByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class CleanUserDataByUserIdTask : Gs2RestSessionTask<CleanUserDataByUserIdRequest, CleanUserDataByUserIdResult>
        {
            public CleanUserDataByUserIdTask(IGs2Session session, RestSessionRequestFactory factory, CleanUserDataByUserIdRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(CleanUserDataByUserIdRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "matchmaking")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/system/clean/user/{userId}";

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
		public IEnumerator CleanUserDataByUserId(
                Request.CleanUserDataByUserIdRequest request,
                UnityAction<AsyncResult<Result.CleanUserDataByUserIdResult>> callback
        ) =>
            new CleanUserDataByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.CleanUserDataByUserIdResult> CleanUserDataByUserIdFuture(
                Request.CleanUserDataByUserIdRequest request
        ) =>
            new CleanUserDataByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.CleanUserDataByUserIdResult> CleanUserDataByUserIdAsync(
                Request.CleanUserDataByUserIdRequest request
        ) =>
            new CleanUserDataByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.CleanUserDataByUserIdResult>();
    #else
		public CleanUserDataByUserIdTask CleanUserDataByUserIdAsync(
                Request.CleanUserDataByUserIdRequest request
        )
		{
			return new CleanUserDataByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.CleanUserDataByUserIdResult> CleanUserDataByUserIdAsync(
                Request.CleanUserDataByUserIdRequest request
        ) =>
            new CleanUserDataByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class CheckCleanUserDataByUserIdTask : Gs2RestSessionTask<CheckCleanUserDataByUserIdRequest, CheckCleanUserDataByUserIdResult>
        {
            public CheckCleanUserDataByUserIdTask(IGs2Session session, RestSessionRequestFactory factory, CheckCleanUserDataByUserIdRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(CheckCleanUserDataByUserIdRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "matchmaking")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/system/clean/user/{userId}";

                url = url.Replace("{userId}", !string.IsNullOrEmpty(request.UserId) ? request.UserId.ToString() : "null");

                var sessionRequest = Factory.Get(url);
                if (request.ContextStack != null)
                {
                    sessionRequest.AddQueryString("contextStack", request.ContextStack);
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
		public IEnumerator CheckCleanUserDataByUserId(
                Request.CheckCleanUserDataByUserIdRequest request,
                UnityAction<AsyncResult<Result.CheckCleanUserDataByUserIdResult>> callback
        ) =>
            new CheckCleanUserDataByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.CheckCleanUserDataByUserIdResult> CheckCleanUserDataByUserIdFuture(
                Request.CheckCleanUserDataByUserIdRequest request
        ) =>
            new CheckCleanUserDataByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.CheckCleanUserDataByUserIdResult> CheckCleanUserDataByUserIdAsync(
                Request.CheckCleanUserDataByUserIdRequest request
        ) =>
            new CheckCleanUserDataByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.CheckCleanUserDataByUserIdResult>();
    #else
		public CheckCleanUserDataByUserIdTask CheckCleanUserDataByUserIdAsync(
                Request.CheckCleanUserDataByUserIdRequest request
        )
		{
			return new CheckCleanUserDataByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.CheckCleanUserDataByUserIdResult> CheckCleanUserDataByUserIdAsync(
                Request.CheckCleanUserDataByUserIdRequest request
        ) =>
            new CheckCleanUserDataByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class PrepareImportUserDataByUserIdTask : Gs2RestSessionTask<PrepareImportUserDataByUserIdRequest, PrepareImportUserDataByUserIdResult>
        {
            public PrepareImportUserDataByUserIdTask(IGs2Session session, RestSessionRequestFactory factory, PrepareImportUserDataByUserIdRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(PrepareImportUserDataByUserIdRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "matchmaking")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/system/import/user/{userId}/prepare";

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
		public IEnumerator PrepareImportUserDataByUserId(
                Request.PrepareImportUserDataByUserIdRequest request,
                UnityAction<AsyncResult<Result.PrepareImportUserDataByUserIdResult>> callback
        ) =>
            new PrepareImportUserDataByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.PrepareImportUserDataByUserIdResult> PrepareImportUserDataByUserIdFuture(
                Request.PrepareImportUserDataByUserIdRequest request
        ) =>
            new PrepareImportUserDataByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.PrepareImportUserDataByUserIdResult> PrepareImportUserDataByUserIdAsync(
                Request.PrepareImportUserDataByUserIdRequest request
        ) =>
            new PrepareImportUserDataByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.PrepareImportUserDataByUserIdResult>();
    #else
		public PrepareImportUserDataByUserIdTask PrepareImportUserDataByUserIdAsync(
                Request.PrepareImportUserDataByUserIdRequest request
        )
		{
			return new PrepareImportUserDataByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.PrepareImportUserDataByUserIdResult> PrepareImportUserDataByUserIdAsync(
                Request.PrepareImportUserDataByUserIdRequest request
        ) =>
            new PrepareImportUserDataByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class ImportUserDataByUserIdTask : Gs2RestSessionTask<ImportUserDataByUserIdRequest, ImportUserDataByUserIdResult>
        {
            public ImportUserDataByUserIdTask(IGs2Session session, RestSessionRequestFactory factory, ImportUserDataByUserIdRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(ImportUserDataByUserIdRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "matchmaking")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/system/import/user/{userId}";

                url = url.Replace("{userId}", !string.IsNullOrEmpty(request.UserId) ? request.UserId.ToString() : "null");

                var sessionRequest = Factory.Post(url);

                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);
                jsonWriter.WriteObjectStart();
                if (request.UploadToken != null)
                {
                    jsonWriter.WritePropertyName("uploadToken");
                    jsonWriter.Write(request.UploadToken);
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
		public IEnumerator ImportUserDataByUserId(
                Request.ImportUserDataByUserIdRequest request,
                UnityAction<AsyncResult<Result.ImportUserDataByUserIdResult>> callback
        ) =>
            new ImportUserDataByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.ImportUserDataByUserIdResult> ImportUserDataByUserIdFuture(
                Request.ImportUserDataByUserIdRequest request
        ) =>
            new ImportUserDataByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.ImportUserDataByUserIdResult> ImportUserDataByUserIdAsync(
                Request.ImportUserDataByUserIdRequest request
        ) =>
            new ImportUserDataByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.ImportUserDataByUserIdResult>();
    #else
		public ImportUserDataByUserIdTask ImportUserDataByUserIdAsync(
                Request.ImportUserDataByUserIdRequest request
        )
		{
			return new ImportUserDataByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.ImportUserDataByUserIdResult> ImportUserDataByUserIdAsync(
                Request.ImportUserDataByUserIdRequest request
        ) =>
            new ImportUserDataByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class CheckImportUserDataByUserIdTask : Gs2RestSessionTask<CheckImportUserDataByUserIdRequest, CheckImportUserDataByUserIdResult>
        {
            public CheckImportUserDataByUserIdTask(IGs2Session session, RestSessionRequestFactory factory, CheckImportUserDataByUserIdRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(CheckImportUserDataByUserIdRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "matchmaking")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/system/import/user/{userId}/{uploadToken}";

                url = url.Replace("{userId}", !string.IsNullOrEmpty(request.UserId) ? request.UserId.ToString() : "null");
                url = url.Replace("{uploadToken}", !string.IsNullOrEmpty(request.UploadToken) ? request.UploadToken.ToString() : "null");

                var sessionRequest = Factory.Get(url);
                if (request.ContextStack != null)
                {
                    sessionRequest.AddQueryString("contextStack", request.ContextStack);
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
		public IEnumerator CheckImportUserDataByUserId(
                Request.CheckImportUserDataByUserIdRequest request,
                UnityAction<AsyncResult<Result.CheckImportUserDataByUserIdResult>> callback
        ) =>
            new CheckImportUserDataByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.CheckImportUserDataByUserIdResult> CheckImportUserDataByUserIdFuture(
                Request.CheckImportUserDataByUserIdRequest request
        ) =>
            new CheckImportUserDataByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.CheckImportUserDataByUserIdResult> CheckImportUserDataByUserIdAsync(
                Request.CheckImportUserDataByUserIdRequest request
        ) =>
            new CheckImportUserDataByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.CheckImportUserDataByUserIdResult>();
    #else
		public CheckImportUserDataByUserIdTask CheckImportUserDataByUserIdAsync(
                Request.CheckImportUserDataByUserIdRequest request
        )
		{
			return new CheckImportUserDataByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.CheckImportUserDataByUserIdResult> CheckImportUserDataByUserIdAsync(
                Request.CheckImportUserDataByUserIdRequest request
        ) =>
            new CheckImportUserDataByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
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
		public IEnumerator DescribeGatherings(
                Request.DescribeGatheringsRequest request,
                UnityAction<AsyncResult<Result.DescribeGatheringsResult>> callback
        ) =>
            new DescribeGatheringsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.DescribeGatheringsResult> DescribeGatheringsFuture(
                Request.DescribeGatheringsRequest request
        ) =>
            new DescribeGatheringsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.DescribeGatheringsResult> DescribeGatheringsAsync(
                Request.DescribeGatheringsRequest request
        ) =>
            new DescribeGatheringsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.DescribeGatheringsResult>();
    #else
		public DescribeGatheringsTask DescribeGatheringsAsync(
                Request.DescribeGatheringsRequest request
        )
		{
			return new DescribeGatheringsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.DescribeGatheringsResult> DescribeGatheringsAsync(
                Request.DescribeGatheringsRequest request
        ) =>
            new DescribeGatheringsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
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
		public IEnumerator CreateGathering(
                Request.CreateGatheringRequest request,
                UnityAction<AsyncResult<Result.CreateGatheringResult>> callback
        ) =>
            new CreateGatheringTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.CreateGatheringResult> CreateGatheringFuture(
                Request.CreateGatheringRequest request
        ) =>
            new CreateGatheringTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.CreateGatheringResult> CreateGatheringAsync(
                Request.CreateGatheringRequest request
        ) =>
            new CreateGatheringTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.CreateGatheringResult>();
    #else
		public CreateGatheringTask CreateGatheringAsync(
                Request.CreateGatheringRequest request
        )
		{
			return new CreateGatheringTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.CreateGatheringResult> CreateGatheringAsync(
                Request.CreateGatheringRequest request
        ) =>
            new CreateGatheringTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
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
		public IEnumerator CreateGatheringByUserId(
                Request.CreateGatheringByUserIdRequest request,
                UnityAction<AsyncResult<Result.CreateGatheringByUserIdResult>> callback
        ) =>
            new CreateGatheringByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.CreateGatheringByUserIdResult> CreateGatheringByUserIdFuture(
                Request.CreateGatheringByUserIdRequest request
        ) =>
            new CreateGatheringByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.CreateGatheringByUserIdResult> CreateGatheringByUserIdAsync(
                Request.CreateGatheringByUserIdRequest request
        ) =>
            new CreateGatheringByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.CreateGatheringByUserIdResult>();
    #else
		public CreateGatheringByUserIdTask CreateGatheringByUserIdAsync(
                Request.CreateGatheringByUserIdRequest request
        )
		{
			return new CreateGatheringByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.CreateGatheringByUserIdResult> CreateGatheringByUserIdAsync(
                Request.CreateGatheringByUserIdRequest request
        ) =>
            new CreateGatheringByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
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
		public IEnumerator UpdateGathering(
                Request.UpdateGatheringRequest request,
                UnityAction<AsyncResult<Result.UpdateGatheringResult>> callback
        ) =>
            new UpdateGatheringTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.UpdateGatheringResult> UpdateGatheringFuture(
                Request.UpdateGatheringRequest request
        ) =>
            new UpdateGatheringTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.UpdateGatheringResult> UpdateGatheringAsync(
                Request.UpdateGatheringRequest request
        ) =>
            new UpdateGatheringTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.UpdateGatheringResult>();
    #else
		public UpdateGatheringTask UpdateGatheringAsync(
                Request.UpdateGatheringRequest request
        )
		{
			return new UpdateGatheringTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.UpdateGatheringResult> UpdateGatheringAsync(
                Request.UpdateGatheringRequest request
        ) =>
            new UpdateGatheringTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
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
		public IEnumerator UpdateGatheringByUserId(
                Request.UpdateGatheringByUserIdRequest request,
                UnityAction<AsyncResult<Result.UpdateGatheringByUserIdResult>> callback
        ) =>
            new UpdateGatheringByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.UpdateGatheringByUserIdResult> UpdateGatheringByUserIdFuture(
                Request.UpdateGatheringByUserIdRequest request
        ) =>
            new UpdateGatheringByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.UpdateGatheringByUserIdResult> UpdateGatheringByUserIdAsync(
                Request.UpdateGatheringByUserIdRequest request
        ) =>
            new UpdateGatheringByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.UpdateGatheringByUserIdResult>();
    #else
		public UpdateGatheringByUserIdTask UpdateGatheringByUserIdAsync(
                Request.UpdateGatheringByUserIdRequest request
        )
		{
			return new UpdateGatheringByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.UpdateGatheringByUserIdResult> UpdateGatheringByUserIdAsync(
                Request.UpdateGatheringByUserIdRequest request
        ) =>
            new UpdateGatheringByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
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
		public IEnumerator DoMatchmakingByPlayer(
                Request.DoMatchmakingByPlayerRequest request,
                UnityAction<AsyncResult<Result.DoMatchmakingByPlayerResult>> callback
        ) =>
            new DoMatchmakingByPlayerTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.DoMatchmakingByPlayerResult> DoMatchmakingByPlayerFuture(
                Request.DoMatchmakingByPlayerRequest request
        ) =>
            new DoMatchmakingByPlayerTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.DoMatchmakingByPlayerResult> DoMatchmakingByPlayerAsync(
                Request.DoMatchmakingByPlayerRequest request
        ) =>
            new DoMatchmakingByPlayerTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.DoMatchmakingByPlayerResult>();
    #else
		public DoMatchmakingByPlayerTask DoMatchmakingByPlayerAsync(
                Request.DoMatchmakingByPlayerRequest request
        )
		{
			return new DoMatchmakingByPlayerTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.DoMatchmakingByPlayerResult> DoMatchmakingByPlayerAsync(
                Request.DoMatchmakingByPlayerRequest request
        ) =>
            new DoMatchmakingByPlayerTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
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
		public IEnumerator DoMatchmaking(
                Request.DoMatchmakingRequest request,
                UnityAction<AsyncResult<Result.DoMatchmakingResult>> callback
        ) =>
            new DoMatchmakingTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.DoMatchmakingResult> DoMatchmakingFuture(
                Request.DoMatchmakingRequest request
        ) =>
            new DoMatchmakingTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.DoMatchmakingResult> DoMatchmakingAsync(
                Request.DoMatchmakingRequest request
        ) =>
            new DoMatchmakingTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.DoMatchmakingResult>();
    #else
		public DoMatchmakingTask DoMatchmakingAsync(
                Request.DoMatchmakingRequest request
        )
		{
			return new DoMatchmakingTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.DoMatchmakingResult> DoMatchmakingAsync(
                Request.DoMatchmakingRequest request
        ) =>
            new DoMatchmakingTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
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
		public IEnumerator DoMatchmakingByUserId(
                Request.DoMatchmakingByUserIdRequest request,
                UnityAction<AsyncResult<Result.DoMatchmakingByUserIdResult>> callback
        ) =>
            new DoMatchmakingByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.DoMatchmakingByUserIdResult> DoMatchmakingByUserIdFuture(
                Request.DoMatchmakingByUserIdRequest request
        ) =>
            new DoMatchmakingByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.DoMatchmakingByUserIdResult> DoMatchmakingByUserIdAsync(
                Request.DoMatchmakingByUserIdRequest request
        ) =>
            new DoMatchmakingByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.DoMatchmakingByUserIdResult>();
    #else
		public DoMatchmakingByUserIdTask DoMatchmakingByUserIdAsync(
                Request.DoMatchmakingByUserIdRequest request
        )
		{
			return new DoMatchmakingByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.DoMatchmakingByUserIdResult> DoMatchmakingByUserIdAsync(
                Request.DoMatchmakingByUserIdRequest request
        ) =>
            new DoMatchmakingByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class PingTask : Gs2RestSessionTask<PingRequest, PingResult>
        {
            public PingTask(IGs2Session session, RestSessionRequestFactory factory, PingRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(PingRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "matchmaking")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/gathering/{gatheringName}/ping";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
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
		public IEnumerator Ping(
                Request.PingRequest request,
                UnityAction<AsyncResult<Result.PingResult>> callback
        ) =>
            new PingTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.PingResult> PingFuture(
                Request.PingRequest request
        ) =>
            new PingTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.PingResult> PingAsync(
                Request.PingRequest request
        ) =>
            new PingTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.PingResult>();
    #else
		public PingTask PingAsync(
                Request.PingRequest request
        )
		{
			return new PingTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.PingResult> PingAsync(
                Request.PingRequest request
        ) =>
            new PingTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class PingByUserIdTask : Gs2RestSessionTask<PingByUserIdRequest, PingByUserIdResult>
        {
            public PingByUserIdTask(IGs2Session session, RestSessionRequestFactory factory, PingByUserIdRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(PingByUserIdRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "matchmaking")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/gathering/{gatheringName}/user/{userId}/ping";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{gatheringName}", !string.IsNullOrEmpty(request.GatheringName) ? request.GatheringName.ToString() : "null");
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
		public IEnumerator PingByUserId(
                Request.PingByUserIdRequest request,
                UnityAction<AsyncResult<Result.PingByUserIdResult>> callback
        ) =>
            new PingByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.PingByUserIdResult> PingByUserIdFuture(
                Request.PingByUserIdRequest request
        ) =>
            new PingByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.PingByUserIdResult> PingByUserIdAsync(
                Request.PingByUserIdRequest request
        ) =>
            new PingByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.PingByUserIdResult>();
    #else
		public PingByUserIdTask PingByUserIdAsync(
                Request.PingByUserIdRequest request
        )
		{
			return new PingByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.PingByUserIdResult> PingByUserIdAsync(
                Request.PingByUserIdRequest request
        ) =>
            new PingByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
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
		public IEnumerator GetGathering(
                Request.GetGatheringRequest request,
                UnityAction<AsyncResult<Result.GetGatheringResult>> callback
        ) =>
            new GetGatheringTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.GetGatheringResult> GetGatheringFuture(
                Request.GetGatheringRequest request
        ) =>
            new GetGatheringTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.GetGatheringResult> GetGatheringAsync(
                Request.GetGatheringRequest request
        ) =>
            new GetGatheringTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.GetGatheringResult>();
    #else
		public GetGatheringTask GetGatheringAsync(
                Request.GetGatheringRequest request
        )
		{
			return new GetGatheringTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.GetGatheringResult> GetGatheringAsync(
                Request.GetGatheringRequest request
        ) =>
            new GetGatheringTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
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
		public IEnumerator CancelMatchmaking(
                Request.CancelMatchmakingRequest request,
                UnityAction<AsyncResult<Result.CancelMatchmakingResult>> callback
        ) =>
            new CancelMatchmakingTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.CancelMatchmakingResult> CancelMatchmakingFuture(
                Request.CancelMatchmakingRequest request
        ) =>
            new CancelMatchmakingTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.CancelMatchmakingResult> CancelMatchmakingAsync(
                Request.CancelMatchmakingRequest request
        ) =>
            new CancelMatchmakingTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.CancelMatchmakingResult>();
    #else
		public CancelMatchmakingTask CancelMatchmakingAsync(
                Request.CancelMatchmakingRequest request
        )
		{
			return new CancelMatchmakingTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.CancelMatchmakingResult> CancelMatchmakingAsync(
                Request.CancelMatchmakingRequest request
        ) =>
            new CancelMatchmakingTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
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
		public IEnumerator CancelMatchmakingByUserId(
                Request.CancelMatchmakingByUserIdRequest request,
                UnityAction<AsyncResult<Result.CancelMatchmakingByUserIdResult>> callback
        ) =>
            new CancelMatchmakingByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.CancelMatchmakingByUserIdResult> CancelMatchmakingByUserIdFuture(
                Request.CancelMatchmakingByUserIdRequest request
        ) =>
            new CancelMatchmakingByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.CancelMatchmakingByUserIdResult> CancelMatchmakingByUserIdAsync(
                Request.CancelMatchmakingByUserIdRequest request
        ) =>
            new CancelMatchmakingByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.CancelMatchmakingByUserIdResult>();
    #else
		public CancelMatchmakingByUserIdTask CancelMatchmakingByUserIdAsync(
                Request.CancelMatchmakingByUserIdRequest request
        )
		{
			return new CancelMatchmakingByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.CancelMatchmakingByUserIdResult> CancelMatchmakingByUserIdAsync(
                Request.CancelMatchmakingByUserIdRequest request
        ) =>
            new CancelMatchmakingByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class EarlyCompleteTask : Gs2RestSessionTask<EarlyCompleteRequest, EarlyCompleteResult>
        {
            public EarlyCompleteTask(IGs2Session session, RestSessionRequestFactory factory, EarlyCompleteRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(EarlyCompleteRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "matchmaking")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/gathering/{gatheringName}/user/me/early";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{gatheringName}", !string.IsNullOrEmpty(request.GatheringName) ? request.GatheringName.ToString() : "null");

                var sessionRequest = Factory.Delete(url);
                if (request.ContextStack != null)
                {
                    sessionRequest.AddQueryString("contextStack", request.ContextStack);
                }
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
		public IEnumerator EarlyComplete(
                Request.EarlyCompleteRequest request,
                UnityAction<AsyncResult<Result.EarlyCompleteResult>> callback
        ) =>
            new EarlyCompleteTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.EarlyCompleteResult> EarlyCompleteFuture(
                Request.EarlyCompleteRequest request
        ) =>
            new EarlyCompleteTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.EarlyCompleteResult> EarlyCompleteAsync(
                Request.EarlyCompleteRequest request
        ) =>
            new EarlyCompleteTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.EarlyCompleteResult>();
    #else
		public EarlyCompleteTask EarlyCompleteAsync(
                Request.EarlyCompleteRequest request
        )
		{
			return new EarlyCompleteTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.EarlyCompleteResult> EarlyCompleteAsync(
                Request.EarlyCompleteRequest request
        ) =>
            new EarlyCompleteTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class EarlyCompleteByUserIdTask : Gs2RestSessionTask<EarlyCompleteByUserIdRequest, EarlyCompleteByUserIdResult>
        {
            public EarlyCompleteByUserIdTask(IGs2Session session, RestSessionRequestFactory factory, EarlyCompleteByUserIdRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(EarlyCompleteByUserIdRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "matchmaking")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/gathering/{gatheringName}/user/{userId}/early";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{gatheringName}", !string.IsNullOrEmpty(request.GatheringName) ? request.GatheringName.ToString() : "null");
                url = url.Replace("{userId}", !string.IsNullOrEmpty(request.UserId) ? request.UserId.ToString() : "null");

                var sessionRequest = Factory.Delete(url);
                if (request.ContextStack != null)
                {
                    sessionRequest.AddQueryString("contextStack", request.ContextStack);
                }
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
		public IEnumerator EarlyCompleteByUserId(
                Request.EarlyCompleteByUserIdRequest request,
                UnityAction<AsyncResult<Result.EarlyCompleteByUserIdResult>> callback
        ) =>
            new EarlyCompleteByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.EarlyCompleteByUserIdResult> EarlyCompleteByUserIdFuture(
                Request.EarlyCompleteByUserIdRequest request
        ) =>
            new EarlyCompleteByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.EarlyCompleteByUserIdResult> EarlyCompleteByUserIdAsync(
                Request.EarlyCompleteByUserIdRequest request
        ) =>
            new EarlyCompleteByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.EarlyCompleteByUserIdResult>();
    #else
		public EarlyCompleteByUserIdTask EarlyCompleteByUserIdAsync(
                Request.EarlyCompleteByUserIdRequest request
        )
		{
			return new EarlyCompleteByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.EarlyCompleteByUserIdResult> EarlyCompleteByUserIdAsync(
                Request.EarlyCompleteByUserIdRequest request
        ) =>
            new EarlyCompleteByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
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
		public IEnumerator DeleteGathering(
                Request.DeleteGatheringRequest request,
                UnityAction<AsyncResult<Result.DeleteGatheringResult>> callback
        ) =>
            new DeleteGatheringTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.DeleteGatheringResult> DeleteGatheringFuture(
                Request.DeleteGatheringRequest request
        ) =>
            new DeleteGatheringTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.DeleteGatheringResult> DeleteGatheringAsync(
                Request.DeleteGatheringRequest request
        ) =>
            new DeleteGatheringTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.DeleteGatheringResult>();
    #else
		public DeleteGatheringTask DeleteGatheringAsync(
                Request.DeleteGatheringRequest request
        )
		{
			return new DeleteGatheringTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.DeleteGatheringResult> DeleteGatheringAsync(
                Request.DeleteGatheringRequest request
        ) =>
            new DeleteGatheringTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
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
		public IEnumerator DescribeRatingModelMasters(
                Request.DescribeRatingModelMastersRequest request,
                UnityAction<AsyncResult<Result.DescribeRatingModelMastersResult>> callback
        ) =>
            new DescribeRatingModelMastersTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.DescribeRatingModelMastersResult> DescribeRatingModelMastersFuture(
                Request.DescribeRatingModelMastersRequest request
        ) =>
            new DescribeRatingModelMastersTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.DescribeRatingModelMastersResult> DescribeRatingModelMastersAsync(
                Request.DescribeRatingModelMastersRequest request
        ) =>
            new DescribeRatingModelMastersTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.DescribeRatingModelMastersResult>();
    #else
		public DescribeRatingModelMastersTask DescribeRatingModelMastersAsync(
                Request.DescribeRatingModelMastersRequest request
        )
		{
			return new DescribeRatingModelMastersTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.DescribeRatingModelMastersResult> DescribeRatingModelMastersAsync(
                Request.DescribeRatingModelMastersRequest request
        ) =>
            new DescribeRatingModelMastersTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
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
                if (request.InitialValue != null)
                {
                    jsonWriter.WritePropertyName("initialValue");
                    jsonWriter.Write(request.InitialValue.ToString());
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
		public IEnumerator CreateRatingModelMaster(
                Request.CreateRatingModelMasterRequest request,
                UnityAction<AsyncResult<Result.CreateRatingModelMasterResult>> callback
        ) =>
            new CreateRatingModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.CreateRatingModelMasterResult> CreateRatingModelMasterFuture(
                Request.CreateRatingModelMasterRequest request
        ) =>
            new CreateRatingModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.CreateRatingModelMasterResult> CreateRatingModelMasterAsync(
                Request.CreateRatingModelMasterRequest request
        ) =>
            new CreateRatingModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.CreateRatingModelMasterResult>();
    #else
		public CreateRatingModelMasterTask CreateRatingModelMasterAsync(
                Request.CreateRatingModelMasterRequest request
        )
		{
			return new CreateRatingModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.CreateRatingModelMasterResult> CreateRatingModelMasterAsync(
                Request.CreateRatingModelMasterRequest request
        ) =>
            new CreateRatingModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
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
		public IEnumerator GetRatingModelMaster(
                Request.GetRatingModelMasterRequest request,
                UnityAction<AsyncResult<Result.GetRatingModelMasterResult>> callback
        ) =>
            new GetRatingModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.GetRatingModelMasterResult> GetRatingModelMasterFuture(
                Request.GetRatingModelMasterRequest request
        ) =>
            new GetRatingModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.GetRatingModelMasterResult> GetRatingModelMasterAsync(
                Request.GetRatingModelMasterRequest request
        ) =>
            new GetRatingModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.GetRatingModelMasterResult>();
    #else
		public GetRatingModelMasterTask GetRatingModelMasterAsync(
                Request.GetRatingModelMasterRequest request
        )
		{
			return new GetRatingModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.GetRatingModelMasterResult> GetRatingModelMasterAsync(
                Request.GetRatingModelMasterRequest request
        ) =>
            new GetRatingModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
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
                if (request.InitialValue != null)
                {
                    jsonWriter.WritePropertyName("initialValue");
                    jsonWriter.Write(request.InitialValue.ToString());
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
		public IEnumerator UpdateRatingModelMaster(
                Request.UpdateRatingModelMasterRequest request,
                UnityAction<AsyncResult<Result.UpdateRatingModelMasterResult>> callback
        ) =>
            new UpdateRatingModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.UpdateRatingModelMasterResult> UpdateRatingModelMasterFuture(
                Request.UpdateRatingModelMasterRequest request
        ) =>
            new UpdateRatingModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.UpdateRatingModelMasterResult> UpdateRatingModelMasterAsync(
                Request.UpdateRatingModelMasterRequest request
        ) =>
            new UpdateRatingModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.UpdateRatingModelMasterResult>();
    #else
		public UpdateRatingModelMasterTask UpdateRatingModelMasterAsync(
                Request.UpdateRatingModelMasterRequest request
        )
		{
			return new UpdateRatingModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.UpdateRatingModelMasterResult> UpdateRatingModelMasterAsync(
                Request.UpdateRatingModelMasterRequest request
        ) =>
            new UpdateRatingModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
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
		public IEnumerator DeleteRatingModelMaster(
                Request.DeleteRatingModelMasterRequest request,
                UnityAction<AsyncResult<Result.DeleteRatingModelMasterResult>> callback
        ) =>
            new DeleteRatingModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.DeleteRatingModelMasterResult> DeleteRatingModelMasterFuture(
                Request.DeleteRatingModelMasterRequest request
        ) =>
            new DeleteRatingModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.DeleteRatingModelMasterResult> DeleteRatingModelMasterAsync(
                Request.DeleteRatingModelMasterRequest request
        ) =>
            new DeleteRatingModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.DeleteRatingModelMasterResult>();
    #else
		public DeleteRatingModelMasterTask DeleteRatingModelMasterAsync(
                Request.DeleteRatingModelMasterRequest request
        )
		{
			return new DeleteRatingModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.DeleteRatingModelMasterResult> DeleteRatingModelMasterAsync(
                Request.DeleteRatingModelMasterRequest request
        ) =>
            new DeleteRatingModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
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
		public IEnumerator DescribeRatingModels(
                Request.DescribeRatingModelsRequest request,
                UnityAction<AsyncResult<Result.DescribeRatingModelsResult>> callback
        ) =>
            new DescribeRatingModelsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.DescribeRatingModelsResult> DescribeRatingModelsFuture(
                Request.DescribeRatingModelsRequest request
        ) =>
            new DescribeRatingModelsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.DescribeRatingModelsResult> DescribeRatingModelsAsync(
                Request.DescribeRatingModelsRequest request
        ) =>
            new DescribeRatingModelsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.DescribeRatingModelsResult>();
    #else
		public DescribeRatingModelsTask DescribeRatingModelsAsync(
                Request.DescribeRatingModelsRequest request
        )
		{
			return new DescribeRatingModelsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.DescribeRatingModelsResult> DescribeRatingModelsAsync(
                Request.DescribeRatingModelsRequest request
        ) =>
            new DescribeRatingModelsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
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
		public IEnumerator GetRatingModel(
                Request.GetRatingModelRequest request,
                UnityAction<AsyncResult<Result.GetRatingModelResult>> callback
        ) =>
            new GetRatingModelTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.GetRatingModelResult> GetRatingModelFuture(
                Request.GetRatingModelRequest request
        ) =>
            new GetRatingModelTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.GetRatingModelResult> GetRatingModelAsync(
                Request.GetRatingModelRequest request
        ) =>
            new GetRatingModelTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.GetRatingModelResult>();
    #else
		public GetRatingModelTask GetRatingModelAsync(
                Request.GetRatingModelRequest request
        )
		{
			return new GetRatingModelTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.GetRatingModelResult> GetRatingModelAsync(
                Request.GetRatingModelRequest request
        ) =>
            new GetRatingModelTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
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
		public IEnumerator ExportMaster(
                Request.ExportMasterRequest request,
                UnityAction<AsyncResult<Result.ExportMasterResult>> callback
        ) =>
            new ExportMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.ExportMasterResult> ExportMasterFuture(
                Request.ExportMasterRequest request
        ) =>
            new ExportMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.ExportMasterResult> ExportMasterAsync(
                Request.ExportMasterRequest request
        ) =>
            new ExportMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.ExportMasterResult>();
    #else
		public ExportMasterTask ExportMasterAsync(
                Request.ExportMasterRequest request
        )
		{
			return new ExportMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.ExportMasterResult> ExportMasterAsync(
                Request.ExportMasterRequest request
        ) =>
            new ExportMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class GetCurrentModelMasterTask : Gs2RestSessionTask<GetCurrentModelMasterRequest, GetCurrentModelMasterResult>
        {
            public GetCurrentModelMasterTask(IGs2Session session, RestSessionRequestFactory factory, GetCurrentModelMasterRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(GetCurrentModelMasterRequest request)
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
		public IEnumerator GetCurrentModelMaster(
                Request.GetCurrentModelMasterRequest request,
                UnityAction<AsyncResult<Result.GetCurrentModelMasterResult>> callback
        ) =>
            new GetCurrentModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.GetCurrentModelMasterResult> GetCurrentModelMasterFuture(
                Request.GetCurrentModelMasterRequest request
        ) =>
            new GetCurrentModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.GetCurrentModelMasterResult> GetCurrentModelMasterAsync(
                Request.GetCurrentModelMasterRequest request
        ) =>
            new GetCurrentModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.GetCurrentModelMasterResult>();
    #else
		public GetCurrentModelMasterTask GetCurrentModelMasterAsync(
                Request.GetCurrentModelMasterRequest request
        )
		{
			return new GetCurrentModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.GetCurrentModelMasterResult> GetCurrentModelMasterAsync(
                Request.GetCurrentModelMasterRequest request
        ) =>
            new GetCurrentModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class PreUpdateCurrentModelMasterTask : Gs2RestSessionTask<PreUpdateCurrentModelMasterRequest, PreUpdateCurrentModelMasterResult>
        {
            public PreUpdateCurrentModelMasterTask(IGs2Session session, RestSessionRequestFactory factory, PreUpdateCurrentModelMasterRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(PreUpdateCurrentModelMasterRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "matchmaking")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/master";

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
		public IEnumerator PreUpdateCurrentModelMaster(
                Request.PreUpdateCurrentModelMasterRequest request,
                UnityAction<AsyncResult<Result.PreUpdateCurrentModelMasterResult>> callback
        ) =>
            new PreUpdateCurrentModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.PreUpdateCurrentModelMasterResult> PreUpdateCurrentModelMasterFuture(
                Request.PreUpdateCurrentModelMasterRequest request
        ) =>
            new PreUpdateCurrentModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.PreUpdateCurrentModelMasterResult> PreUpdateCurrentModelMasterAsync(
                Request.PreUpdateCurrentModelMasterRequest request
        ) =>
            new PreUpdateCurrentModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.PreUpdateCurrentModelMasterResult>();
    #else
		public PreUpdateCurrentModelMasterTask PreUpdateCurrentModelMasterAsync(
                Request.PreUpdateCurrentModelMasterRequest request
        )
		{
			return new PreUpdateCurrentModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.PreUpdateCurrentModelMasterResult> PreUpdateCurrentModelMasterAsync(
                Request.PreUpdateCurrentModelMasterRequest request
        ) =>
            new PreUpdateCurrentModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class UpdateCurrentModelMasterTask : Gs2RestSessionTask<UpdateCurrentModelMasterRequest, UpdateCurrentModelMasterResult>
        {
            public UpdateCurrentModelMasterTask(IGs2Session session, RestSessionRequestFactory factory, UpdateCurrentModelMasterRequest request) : base(session, factory, request)
            {
            }
#if GS2_ENABLE_UNITASK
            protected override async UniTask<UpdateCurrentModelMasterResult> InvokeImpl()
#else
            protected override async Task<UpdateCurrentModelMasterResult> InvokeImpl()
#endif
            {
                if (Request.Settings != null) {
                    var preTask = new PreUpdateCurrentModelMasterTask(
                        Session,
                        Factory,
                        new PreUpdateCurrentModelMasterRequest()
                            .WithContextStack(Request.ContextStack)
                            .WithNamespaceName(Request.NamespaceName)
                    );
                    var preTaskResult = await preTask.Invoke();
#if UNITY_2017_1_OR_NEWER
                    using (var request = UnityWebRequest.Put(preTaskResult.UploadUrl, Request.Settings))
                    {
                        request.SetRequestHeader("Content-Type", "application/json");
                        try {
                            await request.SendWebRequest();
                        }
#if GS2_ENABLE_UNITASK
                        catch (UnityWebRequestException) {}
#endif
                        finally {}

                        var restResult = request.result switch
                        {
                            UnityWebRequest.Result.Success or UnityWebRequest.Result.ProtocolError =>
                                new RestResult(
                                    (int) request.responseCode,
                                    request.downloadHandler?.text
                                ),
                            UnityWebRequest.Result.ConnectionError or UnityWebRequest.Result.DataProcessingError =>
                                new RestResult(
                                    (int) request.responseCode,
                                    null,
                                    (int) request.result,
                                    request.error
                                ),
                            _ =>
                                throw new InvalidOperationException(),
                        };

                        if (restResult.Error != null) throw restResult.Error;
                    }
#else
                    {
                        using var httpRequestMessage = new HttpRequestMessage(System.Net.Http.HttpMethod.Put, preTaskResult.UploadUrl);
                        httpRequestMessage.Content = new ByteArrayContent(System.Text.Encoding.UTF8.GetBytes(Request.Settings));
                        httpRequestMessage.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
    
                        RestResult restResult;
    
                        try
                        {
                            using var httpClient = new HttpClient();
                            using var httpResponseMessage = await httpClient.SendAsync(httpRequestMessage);
    
                            restResult = new RestResult((int) httpResponseMessage.StatusCode, "");
                        }
                        catch (OperationCanceledException e)
                        {
                            restResult = new RestResult(
                                0, // NoInternetConnectionException
                                "",
                                0,
                                e.Message
                            );
                        }
                        catch (System.Net.Http.HttpRequestException e)
                        {
                            restResult = new RestResult(
                                0, // NoInternetConnectionException
                                "",
                                0,
                                e.Message
                            );
                        }
    
                        if (restResult.Error != null) throw restResult.Error;
                    }
#endif
                    Request.Mode = "preUpload";
                    Request.UploadToken = preTaskResult.UploadToken;
                    Request.Settings = null;
                }
                return await base.InvokeImpl();
            }

            protected override IGs2SessionRequest CreateRequest(UpdateCurrentModelMasterRequest request)
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
                if (request.Mode != null)
                {
                    jsonWriter.WritePropertyName("mode");
                    jsonWriter.Write(request.Mode);
                }
                if (request.Settings != null)
                {
                    jsonWriter.WritePropertyName("settings");
                    jsonWriter.Write(request.Settings);
                }
                if (request.UploadToken != null)
                {
                    jsonWriter.WritePropertyName("uploadToken");
                    jsonWriter.Write(request.UploadToken);
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
		public IEnumerator UpdateCurrentModelMaster(
                Request.UpdateCurrentModelMasterRequest request,
                UnityAction<AsyncResult<Result.UpdateCurrentModelMasterResult>> callback
        ) =>
            new UpdateCurrentModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.UpdateCurrentModelMasterResult> UpdateCurrentModelMasterFuture(
                Request.UpdateCurrentModelMasterRequest request
        ) =>
            new UpdateCurrentModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.UpdateCurrentModelMasterResult> UpdateCurrentModelMasterAsync(
                Request.UpdateCurrentModelMasterRequest request
        ) =>
            new UpdateCurrentModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.UpdateCurrentModelMasterResult>();
    #else
		public UpdateCurrentModelMasterTask UpdateCurrentModelMasterAsync(
                Request.UpdateCurrentModelMasterRequest request
        )
		{
			return new UpdateCurrentModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.UpdateCurrentModelMasterResult> UpdateCurrentModelMasterAsync(
                Request.UpdateCurrentModelMasterRequest request
        ) =>
            new UpdateCurrentModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class UpdateCurrentModelMasterFromGitHubTask : Gs2RestSessionTask<UpdateCurrentModelMasterFromGitHubRequest, UpdateCurrentModelMasterFromGitHubResult>
        {
            public UpdateCurrentModelMasterFromGitHubTask(IGs2Session session, RestSessionRequestFactory factory, UpdateCurrentModelMasterFromGitHubRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(UpdateCurrentModelMasterFromGitHubRequest request)
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
		public IEnumerator UpdateCurrentModelMasterFromGitHub(
                Request.UpdateCurrentModelMasterFromGitHubRequest request,
                UnityAction<AsyncResult<Result.UpdateCurrentModelMasterFromGitHubResult>> callback
        ) =>
            new UpdateCurrentModelMasterFromGitHubTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.UpdateCurrentModelMasterFromGitHubResult> UpdateCurrentModelMasterFromGitHubFuture(
                Request.UpdateCurrentModelMasterFromGitHubRequest request
        ) =>
            new UpdateCurrentModelMasterFromGitHubTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.UpdateCurrentModelMasterFromGitHubResult> UpdateCurrentModelMasterFromGitHubAsync(
                Request.UpdateCurrentModelMasterFromGitHubRequest request
        ) =>
            new UpdateCurrentModelMasterFromGitHubTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.UpdateCurrentModelMasterFromGitHubResult>();
    #else
		public UpdateCurrentModelMasterFromGitHubTask UpdateCurrentModelMasterFromGitHubAsync(
                Request.UpdateCurrentModelMasterFromGitHubRequest request
        )
		{
			return new UpdateCurrentModelMasterFromGitHubTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.UpdateCurrentModelMasterFromGitHubResult> UpdateCurrentModelMasterFromGitHubAsync(
                Request.UpdateCurrentModelMasterFromGitHubRequest request
        ) =>
            new UpdateCurrentModelMasterFromGitHubTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class DescribeSeasonModelsTask : Gs2RestSessionTask<DescribeSeasonModelsRequest, DescribeSeasonModelsResult>
        {
            public DescribeSeasonModelsTask(IGs2Session session, RestSessionRequestFactory factory, DescribeSeasonModelsRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DescribeSeasonModelsRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "matchmaking")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/season";

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
		public IEnumerator DescribeSeasonModels(
                Request.DescribeSeasonModelsRequest request,
                UnityAction<AsyncResult<Result.DescribeSeasonModelsResult>> callback
        ) =>
            new DescribeSeasonModelsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.DescribeSeasonModelsResult> DescribeSeasonModelsFuture(
                Request.DescribeSeasonModelsRequest request
        ) =>
            new DescribeSeasonModelsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.DescribeSeasonModelsResult> DescribeSeasonModelsAsync(
                Request.DescribeSeasonModelsRequest request
        ) =>
            new DescribeSeasonModelsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.DescribeSeasonModelsResult>();
    #else
		public DescribeSeasonModelsTask DescribeSeasonModelsAsync(
                Request.DescribeSeasonModelsRequest request
        )
		{
			return new DescribeSeasonModelsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.DescribeSeasonModelsResult> DescribeSeasonModelsAsync(
                Request.DescribeSeasonModelsRequest request
        ) =>
            new DescribeSeasonModelsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class GetSeasonModelTask : Gs2RestSessionTask<GetSeasonModelRequest, GetSeasonModelResult>
        {
            public GetSeasonModelTask(IGs2Session session, RestSessionRequestFactory factory, GetSeasonModelRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(GetSeasonModelRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "matchmaking")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/season/{seasonName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{seasonName}", !string.IsNullOrEmpty(request.SeasonName) ? request.SeasonName.ToString() : "null");

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
		public IEnumerator GetSeasonModel(
                Request.GetSeasonModelRequest request,
                UnityAction<AsyncResult<Result.GetSeasonModelResult>> callback
        ) =>
            new GetSeasonModelTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.GetSeasonModelResult> GetSeasonModelFuture(
                Request.GetSeasonModelRequest request
        ) =>
            new GetSeasonModelTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.GetSeasonModelResult> GetSeasonModelAsync(
                Request.GetSeasonModelRequest request
        ) =>
            new GetSeasonModelTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.GetSeasonModelResult>();
    #else
		public GetSeasonModelTask GetSeasonModelAsync(
                Request.GetSeasonModelRequest request
        )
		{
			return new GetSeasonModelTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.GetSeasonModelResult> GetSeasonModelAsync(
                Request.GetSeasonModelRequest request
        ) =>
            new GetSeasonModelTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class DescribeSeasonModelMastersTask : Gs2RestSessionTask<DescribeSeasonModelMastersRequest, DescribeSeasonModelMastersResult>
        {
            public DescribeSeasonModelMastersTask(IGs2Session session, RestSessionRequestFactory factory, DescribeSeasonModelMastersRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DescribeSeasonModelMastersRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "matchmaking")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/master/season";

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
		public IEnumerator DescribeSeasonModelMasters(
                Request.DescribeSeasonModelMastersRequest request,
                UnityAction<AsyncResult<Result.DescribeSeasonModelMastersResult>> callback
        ) =>
            new DescribeSeasonModelMastersTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.DescribeSeasonModelMastersResult> DescribeSeasonModelMastersFuture(
                Request.DescribeSeasonModelMastersRequest request
        ) =>
            new DescribeSeasonModelMastersTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.DescribeSeasonModelMastersResult> DescribeSeasonModelMastersAsync(
                Request.DescribeSeasonModelMastersRequest request
        ) =>
            new DescribeSeasonModelMastersTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.DescribeSeasonModelMastersResult>();
    #else
		public DescribeSeasonModelMastersTask DescribeSeasonModelMastersAsync(
                Request.DescribeSeasonModelMastersRequest request
        )
		{
			return new DescribeSeasonModelMastersTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.DescribeSeasonModelMastersResult> DescribeSeasonModelMastersAsync(
                Request.DescribeSeasonModelMastersRequest request
        ) =>
            new DescribeSeasonModelMastersTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class CreateSeasonModelMasterTask : Gs2RestSessionTask<CreateSeasonModelMasterRequest, CreateSeasonModelMasterResult>
        {
            public CreateSeasonModelMasterTask(IGs2Session session, RestSessionRequestFactory factory, CreateSeasonModelMasterRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(CreateSeasonModelMasterRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "matchmaking")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/master/season";

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
                if (request.MaximumParticipants != null)
                {
                    jsonWriter.WritePropertyName("maximumParticipants");
                    jsonWriter.Write(request.MaximumParticipants.ToString());
                }
                if (request.ExperienceModelId != null)
                {
                    jsonWriter.WritePropertyName("experienceModelId");
                    jsonWriter.Write(request.ExperienceModelId);
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
		public IEnumerator CreateSeasonModelMaster(
                Request.CreateSeasonModelMasterRequest request,
                UnityAction<AsyncResult<Result.CreateSeasonModelMasterResult>> callback
        ) =>
            new CreateSeasonModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.CreateSeasonModelMasterResult> CreateSeasonModelMasterFuture(
                Request.CreateSeasonModelMasterRequest request
        ) =>
            new CreateSeasonModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.CreateSeasonModelMasterResult> CreateSeasonModelMasterAsync(
                Request.CreateSeasonModelMasterRequest request
        ) =>
            new CreateSeasonModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.CreateSeasonModelMasterResult>();
    #else
		public CreateSeasonModelMasterTask CreateSeasonModelMasterAsync(
                Request.CreateSeasonModelMasterRequest request
        )
		{
			return new CreateSeasonModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.CreateSeasonModelMasterResult> CreateSeasonModelMasterAsync(
                Request.CreateSeasonModelMasterRequest request
        ) =>
            new CreateSeasonModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class GetSeasonModelMasterTask : Gs2RestSessionTask<GetSeasonModelMasterRequest, GetSeasonModelMasterResult>
        {
            public GetSeasonModelMasterTask(IGs2Session session, RestSessionRequestFactory factory, GetSeasonModelMasterRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(GetSeasonModelMasterRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "matchmaking")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/master/season/{seasonName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{seasonName}", !string.IsNullOrEmpty(request.SeasonName) ? request.SeasonName.ToString() : "null");

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
		public IEnumerator GetSeasonModelMaster(
                Request.GetSeasonModelMasterRequest request,
                UnityAction<AsyncResult<Result.GetSeasonModelMasterResult>> callback
        ) =>
            new GetSeasonModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.GetSeasonModelMasterResult> GetSeasonModelMasterFuture(
                Request.GetSeasonModelMasterRequest request
        ) =>
            new GetSeasonModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.GetSeasonModelMasterResult> GetSeasonModelMasterAsync(
                Request.GetSeasonModelMasterRequest request
        ) =>
            new GetSeasonModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.GetSeasonModelMasterResult>();
    #else
		public GetSeasonModelMasterTask GetSeasonModelMasterAsync(
                Request.GetSeasonModelMasterRequest request
        )
		{
			return new GetSeasonModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.GetSeasonModelMasterResult> GetSeasonModelMasterAsync(
                Request.GetSeasonModelMasterRequest request
        ) =>
            new GetSeasonModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class UpdateSeasonModelMasterTask : Gs2RestSessionTask<UpdateSeasonModelMasterRequest, UpdateSeasonModelMasterResult>
        {
            public UpdateSeasonModelMasterTask(IGs2Session session, RestSessionRequestFactory factory, UpdateSeasonModelMasterRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(UpdateSeasonModelMasterRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "matchmaking")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/master/season/{seasonName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{seasonName}", !string.IsNullOrEmpty(request.SeasonName) ? request.SeasonName.ToString() : "null");

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
                if (request.MaximumParticipants != null)
                {
                    jsonWriter.WritePropertyName("maximumParticipants");
                    jsonWriter.Write(request.MaximumParticipants.ToString());
                }
                if (request.ExperienceModelId != null)
                {
                    jsonWriter.WritePropertyName("experienceModelId");
                    jsonWriter.Write(request.ExperienceModelId);
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
		public IEnumerator UpdateSeasonModelMaster(
                Request.UpdateSeasonModelMasterRequest request,
                UnityAction<AsyncResult<Result.UpdateSeasonModelMasterResult>> callback
        ) =>
            new UpdateSeasonModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.UpdateSeasonModelMasterResult> UpdateSeasonModelMasterFuture(
                Request.UpdateSeasonModelMasterRequest request
        ) =>
            new UpdateSeasonModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.UpdateSeasonModelMasterResult> UpdateSeasonModelMasterAsync(
                Request.UpdateSeasonModelMasterRequest request
        ) =>
            new UpdateSeasonModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.UpdateSeasonModelMasterResult>();
    #else
		public UpdateSeasonModelMasterTask UpdateSeasonModelMasterAsync(
                Request.UpdateSeasonModelMasterRequest request
        )
		{
			return new UpdateSeasonModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.UpdateSeasonModelMasterResult> UpdateSeasonModelMasterAsync(
                Request.UpdateSeasonModelMasterRequest request
        ) =>
            new UpdateSeasonModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class DeleteSeasonModelMasterTask : Gs2RestSessionTask<DeleteSeasonModelMasterRequest, DeleteSeasonModelMasterResult>
        {
            public DeleteSeasonModelMasterTask(IGs2Session session, RestSessionRequestFactory factory, DeleteSeasonModelMasterRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DeleteSeasonModelMasterRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "matchmaking")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/master/season/{seasonName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{seasonName}", !string.IsNullOrEmpty(request.SeasonName) ? request.SeasonName.ToString() : "null");

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
		public IEnumerator DeleteSeasonModelMaster(
                Request.DeleteSeasonModelMasterRequest request,
                UnityAction<AsyncResult<Result.DeleteSeasonModelMasterResult>> callback
        ) =>
            new DeleteSeasonModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.DeleteSeasonModelMasterResult> DeleteSeasonModelMasterFuture(
                Request.DeleteSeasonModelMasterRequest request
        ) =>
            new DeleteSeasonModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.DeleteSeasonModelMasterResult> DeleteSeasonModelMasterAsync(
                Request.DeleteSeasonModelMasterRequest request
        ) =>
            new DeleteSeasonModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.DeleteSeasonModelMasterResult>();
    #else
		public DeleteSeasonModelMasterTask DeleteSeasonModelMasterAsync(
                Request.DeleteSeasonModelMasterRequest request
        )
		{
			return new DeleteSeasonModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.DeleteSeasonModelMasterResult> DeleteSeasonModelMasterAsync(
                Request.DeleteSeasonModelMasterRequest request
        ) =>
            new DeleteSeasonModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class DescribeSeasonGatheringsTask : Gs2RestSessionTask<DescribeSeasonGatheringsRequest, DescribeSeasonGatheringsResult>
        {
            public DescribeSeasonGatheringsTask(IGs2Session session, RestSessionRequestFactory factory, DescribeSeasonGatheringsRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DescribeSeasonGatheringsRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "matchmaking")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/season/{seasonName}/{season}/gathering";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{seasonName}", !string.IsNullOrEmpty(request.SeasonName) ? request.SeasonName.ToString() : "null");
                url = url.Replace("{season}",request.Season != null ? request.Season.ToString() : "null");

                var sessionRequest = Factory.Get(url);
                if (request.ContextStack != null)
                {
                    sessionRequest.AddQueryString("contextStack", request.ContextStack);
                }
                if (request.Tier != null) {
                    sessionRequest.AddQueryString("tier", $"{request.Tier}");
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
		public IEnumerator DescribeSeasonGatherings(
                Request.DescribeSeasonGatheringsRequest request,
                UnityAction<AsyncResult<Result.DescribeSeasonGatheringsResult>> callback
        ) =>
            new DescribeSeasonGatheringsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.DescribeSeasonGatheringsResult> DescribeSeasonGatheringsFuture(
                Request.DescribeSeasonGatheringsRequest request
        ) =>
            new DescribeSeasonGatheringsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.DescribeSeasonGatheringsResult> DescribeSeasonGatheringsAsync(
                Request.DescribeSeasonGatheringsRequest request
        ) =>
            new DescribeSeasonGatheringsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.DescribeSeasonGatheringsResult>();
    #else
		public DescribeSeasonGatheringsTask DescribeSeasonGatheringsAsync(
                Request.DescribeSeasonGatheringsRequest request
        )
		{
			return new DescribeSeasonGatheringsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.DescribeSeasonGatheringsResult> DescribeSeasonGatheringsAsync(
                Request.DescribeSeasonGatheringsRequest request
        ) =>
            new DescribeSeasonGatheringsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class DescribeMatchmakingSeasonGatheringsTask : Gs2RestSessionTask<DescribeMatchmakingSeasonGatheringsRequest, DescribeMatchmakingSeasonGatheringsResult>
        {
            public DescribeMatchmakingSeasonGatheringsTask(IGs2Session session, RestSessionRequestFactory factory, DescribeMatchmakingSeasonGatheringsRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DescribeMatchmakingSeasonGatheringsRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "matchmaking")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/season/{seasonName}/{season}/gathering/matchmaking";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{seasonName}", !string.IsNullOrEmpty(request.SeasonName) ? request.SeasonName.ToString() : "null");
                url = url.Replace("{season}",request.Season != null ? request.Season.ToString() : "null");

                var sessionRequest = Factory.Get(url);
                if (request.ContextStack != null)
                {
                    sessionRequest.AddQueryString("contextStack", request.ContextStack);
                }
                if (request.Tier != null) {
                    sessionRequest.AddQueryString("tier", $"{request.Tier}");
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
		public IEnumerator DescribeMatchmakingSeasonGatherings(
                Request.DescribeMatchmakingSeasonGatheringsRequest request,
                UnityAction<AsyncResult<Result.DescribeMatchmakingSeasonGatheringsResult>> callback
        ) =>
            new DescribeMatchmakingSeasonGatheringsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.DescribeMatchmakingSeasonGatheringsResult> DescribeMatchmakingSeasonGatheringsFuture(
                Request.DescribeMatchmakingSeasonGatheringsRequest request
        ) =>
            new DescribeMatchmakingSeasonGatheringsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.DescribeMatchmakingSeasonGatheringsResult> DescribeMatchmakingSeasonGatheringsAsync(
                Request.DescribeMatchmakingSeasonGatheringsRequest request
        ) =>
            new DescribeMatchmakingSeasonGatheringsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.DescribeMatchmakingSeasonGatheringsResult>();
    #else
		public DescribeMatchmakingSeasonGatheringsTask DescribeMatchmakingSeasonGatheringsAsync(
                Request.DescribeMatchmakingSeasonGatheringsRequest request
        )
		{
			return new DescribeMatchmakingSeasonGatheringsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.DescribeMatchmakingSeasonGatheringsResult> DescribeMatchmakingSeasonGatheringsAsync(
                Request.DescribeMatchmakingSeasonGatheringsRequest request
        ) =>
            new DescribeMatchmakingSeasonGatheringsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class DoSeasonMatchmakingTask : Gs2RestSessionTask<DoSeasonMatchmakingRequest, DoSeasonMatchmakingResult>
        {
            public DoSeasonMatchmakingTask(IGs2Session session, RestSessionRequestFactory factory, DoSeasonMatchmakingRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DoSeasonMatchmakingRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "matchmaking")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/me/season/{seasonName}/gathering/do";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{seasonName}", !string.IsNullOrEmpty(request.SeasonName) ? request.SeasonName.ToString() : "null");

                var sessionRequest = Factory.Post(url);

                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);
                jsonWriter.WriteObjectStart();
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
		public IEnumerator DoSeasonMatchmaking(
                Request.DoSeasonMatchmakingRequest request,
                UnityAction<AsyncResult<Result.DoSeasonMatchmakingResult>> callback
        ) =>
            new DoSeasonMatchmakingTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.DoSeasonMatchmakingResult> DoSeasonMatchmakingFuture(
                Request.DoSeasonMatchmakingRequest request
        ) =>
            new DoSeasonMatchmakingTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.DoSeasonMatchmakingResult> DoSeasonMatchmakingAsync(
                Request.DoSeasonMatchmakingRequest request
        ) =>
            new DoSeasonMatchmakingTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.DoSeasonMatchmakingResult>();
    #else
		public DoSeasonMatchmakingTask DoSeasonMatchmakingAsync(
                Request.DoSeasonMatchmakingRequest request
        )
		{
			return new DoSeasonMatchmakingTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.DoSeasonMatchmakingResult> DoSeasonMatchmakingAsync(
                Request.DoSeasonMatchmakingRequest request
        ) =>
            new DoSeasonMatchmakingTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class DoSeasonMatchmakingByUserIdTask : Gs2RestSessionTask<DoSeasonMatchmakingByUserIdRequest, DoSeasonMatchmakingByUserIdResult>
        {
            public DoSeasonMatchmakingByUserIdTask(IGs2Session session, RestSessionRequestFactory factory, DoSeasonMatchmakingByUserIdRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DoSeasonMatchmakingByUserIdRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "matchmaking")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/{userId}/season/{seasonName}/gathering/do";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{seasonName}", !string.IsNullOrEmpty(request.SeasonName) ? request.SeasonName.ToString() : "null");
                url = url.Replace("{userId}", !string.IsNullOrEmpty(request.UserId) ? request.UserId.ToString() : "null");

                var sessionRequest = Factory.Post(url);

                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);
                jsonWriter.WriteObjectStart();
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
		public IEnumerator DoSeasonMatchmakingByUserId(
                Request.DoSeasonMatchmakingByUserIdRequest request,
                UnityAction<AsyncResult<Result.DoSeasonMatchmakingByUserIdResult>> callback
        ) =>
            new DoSeasonMatchmakingByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.DoSeasonMatchmakingByUserIdResult> DoSeasonMatchmakingByUserIdFuture(
                Request.DoSeasonMatchmakingByUserIdRequest request
        ) =>
            new DoSeasonMatchmakingByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.DoSeasonMatchmakingByUserIdResult> DoSeasonMatchmakingByUserIdAsync(
                Request.DoSeasonMatchmakingByUserIdRequest request
        ) =>
            new DoSeasonMatchmakingByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.DoSeasonMatchmakingByUserIdResult>();
    #else
		public DoSeasonMatchmakingByUserIdTask DoSeasonMatchmakingByUserIdAsync(
                Request.DoSeasonMatchmakingByUserIdRequest request
        )
		{
			return new DoSeasonMatchmakingByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.DoSeasonMatchmakingByUserIdResult> DoSeasonMatchmakingByUserIdAsync(
                Request.DoSeasonMatchmakingByUserIdRequest request
        ) =>
            new DoSeasonMatchmakingByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class GetSeasonGatheringTask : Gs2RestSessionTask<GetSeasonGatheringRequest, GetSeasonGatheringResult>
        {
            public GetSeasonGatheringTask(IGs2Session session, RestSessionRequestFactory factory, GetSeasonGatheringRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(GetSeasonGatheringRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "matchmaking")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/season/{seasonName}/{season}/{tier}/gathering/{seasonGatheringName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{seasonName}", !string.IsNullOrEmpty(request.SeasonName) ? request.SeasonName.ToString() : "null");
                url = url.Replace("{season}",request.Season != null ? request.Season.ToString() : "null");
                url = url.Replace("{tier}",request.Tier != null ? request.Tier.ToString() : "null");
                url = url.Replace("{seasonGatheringName}", !string.IsNullOrEmpty(request.SeasonGatheringName) ? request.SeasonGatheringName.ToString() : "null");

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
		public IEnumerator GetSeasonGathering(
                Request.GetSeasonGatheringRequest request,
                UnityAction<AsyncResult<Result.GetSeasonGatheringResult>> callback
        ) =>
            new GetSeasonGatheringTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.GetSeasonGatheringResult> GetSeasonGatheringFuture(
                Request.GetSeasonGatheringRequest request
        ) =>
            new GetSeasonGatheringTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.GetSeasonGatheringResult> GetSeasonGatheringAsync(
                Request.GetSeasonGatheringRequest request
        ) =>
            new GetSeasonGatheringTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.GetSeasonGatheringResult>();
    #else
		public GetSeasonGatheringTask GetSeasonGatheringAsync(
                Request.GetSeasonGatheringRequest request
        )
		{
			return new GetSeasonGatheringTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.GetSeasonGatheringResult> GetSeasonGatheringAsync(
                Request.GetSeasonGatheringRequest request
        ) =>
            new GetSeasonGatheringTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class VerifyIncludeParticipantTask : Gs2RestSessionTask<VerifyIncludeParticipantRequest, VerifyIncludeParticipantResult>
        {
            public VerifyIncludeParticipantTask(IGs2Session session, RestSessionRequestFactory factory, VerifyIncludeParticipantRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(VerifyIncludeParticipantRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "matchmaking")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/season/{seasonName}/{season}/{tier}/gathering/{seasonGatheringName}/participant/me/verify";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{seasonName}", !string.IsNullOrEmpty(request.SeasonName) ? request.SeasonName.ToString() : "null");
                url = url.Replace("{season}",request.Season != null ? request.Season.ToString() : "null");
                url = url.Replace("{tier}",request.Tier != null ? request.Tier.ToString() : "null");
                url = url.Replace("{seasonGatheringName}", !string.IsNullOrEmpty(request.SeasonGatheringName) ? request.SeasonGatheringName.ToString() : "null");

                var sessionRequest = Factory.Post(url);

                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);
                jsonWriter.WriteObjectStart();
                if (request.VerifyType != null)
                {
                    jsonWriter.WritePropertyName("verifyType");
                    jsonWriter.Write(request.VerifyType);
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
		public IEnumerator VerifyIncludeParticipant(
                Request.VerifyIncludeParticipantRequest request,
                UnityAction<AsyncResult<Result.VerifyIncludeParticipantResult>> callback
        ) =>
            new VerifyIncludeParticipantTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.VerifyIncludeParticipantResult> VerifyIncludeParticipantFuture(
                Request.VerifyIncludeParticipantRequest request
        ) =>
            new VerifyIncludeParticipantTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.VerifyIncludeParticipantResult> VerifyIncludeParticipantAsync(
                Request.VerifyIncludeParticipantRequest request
        ) =>
            new VerifyIncludeParticipantTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.VerifyIncludeParticipantResult>();
    #else
		public VerifyIncludeParticipantTask VerifyIncludeParticipantAsync(
                Request.VerifyIncludeParticipantRequest request
        )
		{
			return new VerifyIncludeParticipantTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.VerifyIncludeParticipantResult> VerifyIncludeParticipantAsync(
                Request.VerifyIncludeParticipantRequest request
        ) =>
            new VerifyIncludeParticipantTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class VerifyIncludeParticipantByUserIdTask : Gs2RestSessionTask<VerifyIncludeParticipantByUserIdRequest, VerifyIncludeParticipantByUserIdResult>
        {
            public VerifyIncludeParticipantByUserIdTask(IGs2Session session, RestSessionRequestFactory factory, VerifyIncludeParticipantByUserIdRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(VerifyIncludeParticipantByUserIdRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "matchmaking")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/season/{seasonName}/{season}/{tier}/gathering/{seasonGatheringName}/participant/{userId}/verify";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{seasonName}", !string.IsNullOrEmpty(request.SeasonName) ? request.SeasonName.ToString() : "null");
                url = url.Replace("{season}",request.Season != null ? request.Season.ToString() : "null");
                url = url.Replace("{tier}",request.Tier != null ? request.Tier.ToString() : "null");
                url = url.Replace("{seasonGatheringName}", !string.IsNullOrEmpty(request.SeasonGatheringName) ? request.SeasonGatheringName.ToString() : "null");
                url = url.Replace("{userId}", !string.IsNullOrEmpty(request.UserId) ? request.UserId.ToString() : "null");

                var sessionRequest = Factory.Post(url);

                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);
                jsonWriter.WriteObjectStart();
                if (request.VerifyType != null)
                {
                    jsonWriter.WritePropertyName("verifyType");
                    jsonWriter.Write(request.VerifyType);
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
		public IEnumerator VerifyIncludeParticipantByUserId(
                Request.VerifyIncludeParticipantByUserIdRequest request,
                UnityAction<AsyncResult<Result.VerifyIncludeParticipantByUserIdResult>> callback
        ) =>
            new VerifyIncludeParticipantByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.VerifyIncludeParticipantByUserIdResult> VerifyIncludeParticipantByUserIdFuture(
                Request.VerifyIncludeParticipantByUserIdRequest request
        ) =>
            new VerifyIncludeParticipantByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.VerifyIncludeParticipantByUserIdResult> VerifyIncludeParticipantByUserIdAsync(
                Request.VerifyIncludeParticipantByUserIdRequest request
        ) =>
            new VerifyIncludeParticipantByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.VerifyIncludeParticipantByUserIdResult>();
    #else
		public VerifyIncludeParticipantByUserIdTask VerifyIncludeParticipantByUserIdAsync(
                Request.VerifyIncludeParticipantByUserIdRequest request
        )
		{
			return new VerifyIncludeParticipantByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.VerifyIncludeParticipantByUserIdResult> VerifyIncludeParticipantByUserIdAsync(
                Request.VerifyIncludeParticipantByUserIdRequest request
        ) =>
            new VerifyIncludeParticipantByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class DeleteSeasonGatheringTask : Gs2RestSessionTask<DeleteSeasonGatheringRequest, DeleteSeasonGatheringResult>
        {
            public DeleteSeasonGatheringTask(IGs2Session session, RestSessionRequestFactory factory, DeleteSeasonGatheringRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DeleteSeasonGatheringRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "matchmaking")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/season/{seasonName}/{season}/{tier}/gathering/{seasonGatheringName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{seasonName}", !string.IsNullOrEmpty(request.SeasonName) ? request.SeasonName.ToString() : "null");
                url = url.Replace("{season}",request.Season != null ? request.Season.ToString() : "null");
                url = url.Replace("{tier}",request.Tier != null ? request.Tier.ToString() : "null");
                url = url.Replace("{seasonGatheringName}", !string.IsNullOrEmpty(request.SeasonGatheringName) ? request.SeasonGatheringName.ToString() : "null");

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
		public IEnumerator DeleteSeasonGathering(
                Request.DeleteSeasonGatheringRequest request,
                UnityAction<AsyncResult<Result.DeleteSeasonGatheringResult>> callback
        ) =>
            new DeleteSeasonGatheringTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.DeleteSeasonGatheringResult> DeleteSeasonGatheringFuture(
                Request.DeleteSeasonGatheringRequest request
        ) =>
            new DeleteSeasonGatheringTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.DeleteSeasonGatheringResult> DeleteSeasonGatheringAsync(
                Request.DeleteSeasonGatheringRequest request
        ) =>
            new DeleteSeasonGatheringTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.DeleteSeasonGatheringResult>();
    #else
		public DeleteSeasonGatheringTask DeleteSeasonGatheringAsync(
                Request.DeleteSeasonGatheringRequest request
        )
		{
			return new DeleteSeasonGatheringTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.DeleteSeasonGatheringResult> DeleteSeasonGatheringAsync(
                Request.DeleteSeasonGatheringRequest request
        ) =>
            new DeleteSeasonGatheringTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class VerifyIncludeParticipantByStampTaskTask : Gs2RestSessionTask<VerifyIncludeParticipantByStampTaskRequest, VerifyIncludeParticipantByStampTaskResult>
        {
            public VerifyIncludeParticipantByStampTaskTask(IGs2Session session, RestSessionRequestFactory factory, VerifyIncludeParticipantByStampTaskRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(VerifyIncludeParticipantByStampTaskRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "matchmaking")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/stamp/season/gathering/participant/verify";

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
		public IEnumerator VerifyIncludeParticipantByStampTask(
                Request.VerifyIncludeParticipantByStampTaskRequest request,
                UnityAction<AsyncResult<Result.VerifyIncludeParticipantByStampTaskResult>> callback
        ) =>
            new VerifyIncludeParticipantByStampTaskTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.VerifyIncludeParticipantByStampTaskResult> VerifyIncludeParticipantByStampTaskFuture(
                Request.VerifyIncludeParticipantByStampTaskRequest request
        ) =>
            new VerifyIncludeParticipantByStampTaskTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.VerifyIncludeParticipantByStampTaskResult> VerifyIncludeParticipantByStampTaskAsync(
                Request.VerifyIncludeParticipantByStampTaskRequest request
        ) =>
            new VerifyIncludeParticipantByStampTaskTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.VerifyIncludeParticipantByStampTaskResult>();
    #else
		public VerifyIncludeParticipantByStampTaskTask VerifyIncludeParticipantByStampTaskAsync(
                Request.VerifyIncludeParticipantByStampTaskRequest request
        )
		{
			return new VerifyIncludeParticipantByStampTaskTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.VerifyIncludeParticipantByStampTaskResult> VerifyIncludeParticipantByStampTaskAsync(
                Request.VerifyIncludeParticipantByStampTaskRequest request
        ) =>
            new VerifyIncludeParticipantByStampTaskTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class DescribeJoinedSeasonGatheringsTask : Gs2RestSessionTask<DescribeJoinedSeasonGatheringsRequest, DescribeJoinedSeasonGatheringsResult>
        {
            public DescribeJoinedSeasonGatheringsTask(IGs2Session session, RestSessionRequestFactory factory, DescribeJoinedSeasonGatheringsRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DescribeJoinedSeasonGatheringsRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "matchmaking")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/me/season/{seasonName}/gathering/join";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{seasonName}", !string.IsNullOrEmpty(request.SeasonName) ? request.SeasonName.ToString() : "null");

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
                if (request.AccessToken != null)
                {
                    sessionRequest.AddHeader("X-GS2-ACCESS-TOKEN", request.AccessToken);
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
		public IEnumerator DescribeJoinedSeasonGatherings(
                Request.DescribeJoinedSeasonGatheringsRequest request,
                UnityAction<AsyncResult<Result.DescribeJoinedSeasonGatheringsResult>> callback
        ) =>
            new DescribeJoinedSeasonGatheringsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.DescribeJoinedSeasonGatheringsResult> DescribeJoinedSeasonGatheringsFuture(
                Request.DescribeJoinedSeasonGatheringsRequest request
        ) =>
            new DescribeJoinedSeasonGatheringsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.DescribeJoinedSeasonGatheringsResult> DescribeJoinedSeasonGatheringsAsync(
                Request.DescribeJoinedSeasonGatheringsRequest request
        ) =>
            new DescribeJoinedSeasonGatheringsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.DescribeJoinedSeasonGatheringsResult>();
    #else
		public DescribeJoinedSeasonGatheringsTask DescribeJoinedSeasonGatheringsAsync(
                Request.DescribeJoinedSeasonGatheringsRequest request
        )
		{
			return new DescribeJoinedSeasonGatheringsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.DescribeJoinedSeasonGatheringsResult> DescribeJoinedSeasonGatheringsAsync(
                Request.DescribeJoinedSeasonGatheringsRequest request
        ) =>
            new DescribeJoinedSeasonGatheringsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class DescribeJoinedSeasonGatheringsByUserIdTask : Gs2RestSessionTask<DescribeJoinedSeasonGatheringsByUserIdRequest, DescribeJoinedSeasonGatheringsByUserIdResult>
        {
            public DescribeJoinedSeasonGatheringsByUserIdTask(IGs2Session session, RestSessionRequestFactory factory, DescribeJoinedSeasonGatheringsByUserIdRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DescribeJoinedSeasonGatheringsByUserIdRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "matchmaking")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/{userId}/season/{seasonName}/gathering/join";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{userId}", !string.IsNullOrEmpty(request.UserId) ? request.UserId.ToString() : "null");
                url = url.Replace("{seasonName}", !string.IsNullOrEmpty(request.SeasonName) ? request.SeasonName.ToString() : "null");

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
		public IEnumerator DescribeJoinedSeasonGatheringsByUserId(
                Request.DescribeJoinedSeasonGatheringsByUserIdRequest request,
                UnityAction<AsyncResult<Result.DescribeJoinedSeasonGatheringsByUserIdResult>> callback
        ) =>
            new DescribeJoinedSeasonGatheringsByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.DescribeJoinedSeasonGatheringsByUserIdResult> DescribeJoinedSeasonGatheringsByUserIdFuture(
                Request.DescribeJoinedSeasonGatheringsByUserIdRequest request
        ) =>
            new DescribeJoinedSeasonGatheringsByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.DescribeJoinedSeasonGatheringsByUserIdResult> DescribeJoinedSeasonGatheringsByUserIdAsync(
                Request.DescribeJoinedSeasonGatheringsByUserIdRequest request
        ) =>
            new DescribeJoinedSeasonGatheringsByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.DescribeJoinedSeasonGatheringsByUserIdResult>();
    #else
		public DescribeJoinedSeasonGatheringsByUserIdTask DescribeJoinedSeasonGatheringsByUserIdAsync(
                Request.DescribeJoinedSeasonGatheringsByUserIdRequest request
        )
		{
			return new DescribeJoinedSeasonGatheringsByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.DescribeJoinedSeasonGatheringsByUserIdResult> DescribeJoinedSeasonGatheringsByUserIdAsync(
                Request.DescribeJoinedSeasonGatheringsByUserIdRequest request
        ) =>
            new DescribeJoinedSeasonGatheringsByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class GetJoinedSeasonGatheringTask : Gs2RestSessionTask<GetJoinedSeasonGatheringRequest, GetJoinedSeasonGatheringResult>
        {
            public GetJoinedSeasonGatheringTask(IGs2Session session, RestSessionRequestFactory factory, GetJoinedSeasonGatheringRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(GetJoinedSeasonGatheringRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "matchmaking")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/me/season/{seasonName}/gathering/join/{season}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{seasonName}", !string.IsNullOrEmpty(request.SeasonName) ? request.SeasonName.ToString() : "null");
                url = url.Replace("{season}",request.Season != null ? request.Season.ToString() : "null");

                var sessionRequest = Factory.Get(url);
                if (request.ContextStack != null)
                {
                    sessionRequest.AddQueryString("contextStack", request.ContextStack);
                }
                if (request.AccessToken != null)
                {
                    sessionRequest.AddHeader("X-GS2-ACCESS-TOKEN", request.AccessToken);
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
		public IEnumerator GetJoinedSeasonGathering(
                Request.GetJoinedSeasonGatheringRequest request,
                UnityAction<AsyncResult<Result.GetJoinedSeasonGatheringResult>> callback
        ) =>
            new GetJoinedSeasonGatheringTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.GetJoinedSeasonGatheringResult> GetJoinedSeasonGatheringFuture(
                Request.GetJoinedSeasonGatheringRequest request
        ) =>
            new GetJoinedSeasonGatheringTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.GetJoinedSeasonGatheringResult> GetJoinedSeasonGatheringAsync(
                Request.GetJoinedSeasonGatheringRequest request
        ) =>
            new GetJoinedSeasonGatheringTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.GetJoinedSeasonGatheringResult>();
    #else
		public GetJoinedSeasonGatheringTask GetJoinedSeasonGatheringAsync(
                Request.GetJoinedSeasonGatheringRequest request
        )
		{
			return new GetJoinedSeasonGatheringTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.GetJoinedSeasonGatheringResult> GetJoinedSeasonGatheringAsync(
                Request.GetJoinedSeasonGatheringRequest request
        ) =>
            new GetJoinedSeasonGatheringTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class GetJoinedSeasonGatheringByUserIdTask : Gs2RestSessionTask<GetJoinedSeasonGatheringByUserIdRequest, GetJoinedSeasonGatheringByUserIdResult>
        {
            public GetJoinedSeasonGatheringByUserIdTask(IGs2Session session, RestSessionRequestFactory factory, GetJoinedSeasonGatheringByUserIdRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(GetJoinedSeasonGatheringByUserIdRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "matchmaking")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/{userId}/season/{seasonName}/gathering/join/{season}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{userId}", !string.IsNullOrEmpty(request.UserId) ? request.UserId.ToString() : "null");
                url = url.Replace("{seasonName}", !string.IsNullOrEmpty(request.SeasonName) ? request.SeasonName.ToString() : "null");
                url = url.Replace("{season}",request.Season != null ? request.Season.ToString() : "null");

                var sessionRequest = Factory.Get(url);
                if (request.ContextStack != null)
                {
                    sessionRequest.AddQueryString("contextStack", request.ContextStack);
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
		public IEnumerator GetJoinedSeasonGatheringByUserId(
                Request.GetJoinedSeasonGatheringByUserIdRequest request,
                UnityAction<AsyncResult<Result.GetJoinedSeasonGatheringByUserIdResult>> callback
        ) =>
            new GetJoinedSeasonGatheringByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.GetJoinedSeasonGatheringByUserIdResult> GetJoinedSeasonGatheringByUserIdFuture(
                Request.GetJoinedSeasonGatheringByUserIdRequest request
        ) =>
            new GetJoinedSeasonGatheringByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.GetJoinedSeasonGatheringByUserIdResult> GetJoinedSeasonGatheringByUserIdAsync(
                Request.GetJoinedSeasonGatheringByUserIdRequest request
        ) =>
            new GetJoinedSeasonGatheringByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.GetJoinedSeasonGatheringByUserIdResult>();
    #else
		public GetJoinedSeasonGatheringByUserIdTask GetJoinedSeasonGatheringByUserIdAsync(
                Request.GetJoinedSeasonGatheringByUserIdRequest request
        )
		{
			return new GetJoinedSeasonGatheringByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.GetJoinedSeasonGatheringByUserIdResult> GetJoinedSeasonGatheringByUserIdAsync(
                Request.GetJoinedSeasonGatheringByUserIdRequest request
        ) =>
            new GetJoinedSeasonGatheringByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
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
                if (request.AccessToken != null)
                {
                    sessionRequest.AddHeader("X-GS2-ACCESS-TOKEN", request.AccessToken);
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
		public IEnumerator DescribeRatings(
                Request.DescribeRatingsRequest request,
                UnityAction<AsyncResult<Result.DescribeRatingsResult>> callback
        ) =>
            new DescribeRatingsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.DescribeRatingsResult> DescribeRatingsFuture(
                Request.DescribeRatingsRequest request
        ) =>
            new DescribeRatingsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.DescribeRatingsResult> DescribeRatingsAsync(
                Request.DescribeRatingsRequest request
        ) =>
            new DescribeRatingsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.DescribeRatingsResult>();
    #else
		public DescribeRatingsTask DescribeRatingsAsync(
                Request.DescribeRatingsRequest request
        )
		{
			return new DescribeRatingsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.DescribeRatingsResult> DescribeRatingsAsync(
                Request.DescribeRatingsRequest request
        ) =>
            new DescribeRatingsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
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
		public IEnumerator DescribeRatingsByUserId(
                Request.DescribeRatingsByUserIdRequest request,
                UnityAction<AsyncResult<Result.DescribeRatingsByUserIdResult>> callback
        ) =>
            new DescribeRatingsByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.DescribeRatingsByUserIdResult> DescribeRatingsByUserIdFuture(
                Request.DescribeRatingsByUserIdRequest request
        ) =>
            new DescribeRatingsByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.DescribeRatingsByUserIdResult> DescribeRatingsByUserIdAsync(
                Request.DescribeRatingsByUserIdRequest request
        ) =>
            new DescribeRatingsByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.DescribeRatingsByUserIdResult>();
    #else
		public DescribeRatingsByUserIdTask DescribeRatingsByUserIdAsync(
                Request.DescribeRatingsByUserIdRequest request
        )
		{
			return new DescribeRatingsByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.DescribeRatingsByUserIdResult> DescribeRatingsByUserIdAsync(
                Request.DescribeRatingsByUserIdRequest request
        ) =>
            new DescribeRatingsByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
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
                if (request.AccessToken != null)
                {
                    sessionRequest.AddHeader("X-GS2-ACCESS-TOKEN", request.AccessToken);
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
		public IEnumerator GetRating(
                Request.GetRatingRequest request,
                UnityAction<AsyncResult<Result.GetRatingResult>> callback
        ) =>
            new GetRatingTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.GetRatingResult> GetRatingFuture(
                Request.GetRatingRequest request
        ) =>
            new GetRatingTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.GetRatingResult> GetRatingAsync(
                Request.GetRatingRequest request
        ) =>
            new GetRatingTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.GetRatingResult>();
    #else
		public GetRatingTask GetRatingAsync(
                Request.GetRatingRequest request
        )
		{
			return new GetRatingTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.GetRatingResult> GetRatingAsync(
                Request.GetRatingRequest request
        ) =>
            new GetRatingTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
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
		public IEnumerator GetRatingByUserId(
                Request.GetRatingByUserIdRequest request,
                UnityAction<AsyncResult<Result.GetRatingByUserIdResult>> callback
        ) =>
            new GetRatingByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.GetRatingByUserIdResult> GetRatingByUserIdFuture(
                Request.GetRatingByUserIdRequest request
        ) =>
            new GetRatingByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.GetRatingByUserIdResult> GetRatingByUserIdAsync(
                Request.GetRatingByUserIdRequest request
        ) =>
            new GetRatingByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.GetRatingByUserIdResult>();
    #else
		public GetRatingByUserIdTask GetRatingByUserIdAsync(
                Request.GetRatingByUserIdRequest request
        )
		{
			return new GetRatingByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.GetRatingByUserIdResult> GetRatingByUserIdAsync(
                Request.GetRatingByUserIdRequest request
        ) =>
            new GetRatingByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
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
		public IEnumerator PutResult(
                Request.PutResultRequest request,
                UnityAction<AsyncResult<Result.PutResultResult>> callback
        ) =>
            new PutResultTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.PutResultResult> PutResultFuture(
                Request.PutResultRequest request
        ) =>
            new PutResultTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.PutResultResult> PutResultAsync(
                Request.PutResultRequest request
        ) =>
            new PutResultTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.PutResultResult>();
    #else
		public PutResultTask PutResultAsync(
                Request.PutResultRequest request
        )
		{
			return new PutResultTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.PutResultResult> PutResultAsync(
                Request.PutResultRequest request
        ) =>
            new PutResultTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
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
		public IEnumerator DeleteRating(
                Request.DeleteRatingRequest request,
                UnityAction<AsyncResult<Result.DeleteRatingResult>> callback
        ) =>
            new DeleteRatingTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.DeleteRatingResult> DeleteRatingFuture(
                Request.DeleteRatingRequest request
        ) =>
            new DeleteRatingTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.DeleteRatingResult> DeleteRatingAsync(
                Request.DeleteRatingRequest request
        ) =>
            new DeleteRatingTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.DeleteRatingResult>();
    #else
		public DeleteRatingTask DeleteRatingAsync(
                Request.DeleteRatingRequest request
        )
		{
			return new DeleteRatingTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.DeleteRatingResult> DeleteRatingAsync(
                Request.DeleteRatingRequest request
        ) =>
            new DeleteRatingTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
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
                if (request.AccessToken != null)
                {
                    sessionRequest.AddHeader("X-GS2-ACCESS-TOKEN", request.AccessToken);
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
		public IEnumerator GetBallot(
                Request.GetBallotRequest request,
                UnityAction<AsyncResult<Result.GetBallotResult>> callback
        ) =>
            new GetBallotTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.GetBallotResult> GetBallotFuture(
                Request.GetBallotRequest request
        ) =>
            new GetBallotTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.GetBallotResult> GetBallotAsync(
                Request.GetBallotRequest request
        ) =>
            new GetBallotTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.GetBallotResult>();
    #else
		public GetBallotTask GetBallotAsync(
                Request.GetBallotRequest request
        )
		{
			return new GetBallotTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.GetBallotResult> GetBallotAsync(
                Request.GetBallotRequest request
        ) =>
            new GetBallotTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
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
		public IEnumerator GetBallotByUserId(
                Request.GetBallotByUserIdRequest request,
                UnityAction<AsyncResult<Result.GetBallotByUserIdResult>> callback
        ) =>
            new GetBallotByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.GetBallotByUserIdResult> GetBallotByUserIdFuture(
                Request.GetBallotByUserIdRequest request
        ) =>
            new GetBallotByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.GetBallotByUserIdResult> GetBallotByUserIdAsync(
                Request.GetBallotByUserIdRequest request
        ) =>
            new GetBallotByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.GetBallotByUserIdResult>();
    #else
		public GetBallotByUserIdTask GetBallotByUserIdAsync(
                Request.GetBallotByUserIdRequest request
        )
		{
			return new GetBallotByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.GetBallotByUserIdResult> GetBallotByUserIdAsync(
                Request.GetBallotByUserIdRequest request
        ) =>
            new GetBallotByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
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
		public IEnumerator Vote(
                Request.VoteRequest request,
                UnityAction<AsyncResult<Result.VoteResult>> callback
        ) =>
            new VoteTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.VoteResult> VoteFuture(
                Request.VoteRequest request
        ) =>
            new VoteTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.VoteResult> VoteAsync(
                Request.VoteRequest request
        ) =>
            new VoteTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.VoteResult>();
    #else
		public VoteTask VoteAsync(
                Request.VoteRequest request
        )
		{
			return new VoteTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.VoteResult> VoteAsync(
                Request.VoteRequest request
        ) =>
            new VoteTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
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
		public IEnumerator VoteMultiple(
                Request.VoteMultipleRequest request,
                UnityAction<AsyncResult<Result.VoteMultipleResult>> callback
        ) =>
            new VoteMultipleTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.VoteMultipleResult> VoteMultipleFuture(
                Request.VoteMultipleRequest request
        ) =>
            new VoteMultipleTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.VoteMultipleResult> VoteMultipleAsync(
                Request.VoteMultipleRequest request
        ) =>
            new VoteMultipleTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.VoteMultipleResult>();
    #else
		public VoteMultipleTask VoteMultipleAsync(
                Request.VoteMultipleRequest request
        )
		{
			return new VoteMultipleTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.VoteMultipleResult> VoteMultipleAsync(
                Request.VoteMultipleRequest request
        ) =>
            new VoteMultipleTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
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
		public IEnumerator CommitVote(
                Request.CommitVoteRequest request,
                UnityAction<AsyncResult<Result.CommitVoteResult>> callback
        ) =>
            new CommitVoteTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.CommitVoteResult> CommitVoteFuture(
                Request.CommitVoteRequest request
        ) =>
            new CommitVoteTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.CommitVoteResult> CommitVoteAsync(
                Request.CommitVoteRequest request
        ) =>
            new CommitVoteTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.CommitVoteResult>();
    #else
		public CommitVoteTask CommitVoteAsync(
                Request.CommitVoteRequest request
        )
		{
			return new CommitVoteTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.CommitVoteResult> CommitVoteAsync(
                Request.CommitVoteRequest request
        ) =>
            new CommitVoteTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif
	}
}