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
using Gs2.Gs2Mission.Request;
using Gs2.Gs2Mission.Result;
using Gs2.Util.LitJson;

namespace Gs2.Gs2Mission
{
	public class Gs2MissionRestClient : AbstractGs2Client
	{
#if UNITY_2017_1_OR_NEWER
		private readonly CertificateHandler _certificateHandler;
#endif

		public static string Endpoint = "mission";

        protected Gs2RestSession Gs2RestSession => (Gs2RestSession) Gs2Session;

		public Gs2MissionRestClient(Gs2RestSession Gs2RestSession) : base(Gs2RestSession)
		{

		}

#if UNITY_2017_1_OR_NEWER
		public Gs2MissionRestClient(Gs2RestSession gs2RestSession, CertificateHandler certificateHandler) : base(gs2RestSession)
		{
			_certificateHandler = certificateHandler;
		}
#endif


        public class DescribeCompletesTask : Gs2RestSessionTask<DescribeCompletesRequest, DescribeCompletesResult>
        {
            public DescribeCompletesTask(IGs2Session session, RestSessionRequestFactory factory, DescribeCompletesRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DescribeCompletesRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "mission")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/me/complete";

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
		public IEnumerator DescribeCompletes(
                Request.DescribeCompletesRequest request,
                UnityAction<AsyncResult<Result.DescribeCompletesResult>> callback
        ) =>
            new DescribeCompletesTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.DescribeCompletesResult> DescribeCompletesFuture(
                Request.DescribeCompletesRequest request
        ) =>
            new DescribeCompletesTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.DescribeCompletesResult> DescribeCompletesAsync(
                Request.DescribeCompletesRequest request
        ) =>
            new DescribeCompletesTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.DescribeCompletesResult>();
    #else
		public DescribeCompletesTask DescribeCompletesAsync(
                Request.DescribeCompletesRequest request
        )
		{
			return new DescribeCompletesTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.DescribeCompletesResult> DescribeCompletesAsync(
                Request.DescribeCompletesRequest request
        ) =>
            new DescribeCompletesTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class DescribeCompletesByUserIdTask : Gs2RestSessionTask<DescribeCompletesByUserIdRequest, DescribeCompletesByUserIdResult>
        {
            public DescribeCompletesByUserIdTask(IGs2Session session, RestSessionRequestFactory factory, DescribeCompletesByUserIdRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DescribeCompletesByUserIdRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "mission")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/{userId}/complete";

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
		public IEnumerator DescribeCompletesByUserId(
                Request.DescribeCompletesByUserIdRequest request,
                UnityAction<AsyncResult<Result.DescribeCompletesByUserIdResult>> callback
        ) =>
            new DescribeCompletesByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.DescribeCompletesByUserIdResult> DescribeCompletesByUserIdFuture(
                Request.DescribeCompletesByUserIdRequest request
        ) =>
            new DescribeCompletesByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.DescribeCompletesByUserIdResult> DescribeCompletesByUserIdAsync(
                Request.DescribeCompletesByUserIdRequest request
        ) =>
            new DescribeCompletesByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.DescribeCompletesByUserIdResult>();
    #else
		public DescribeCompletesByUserIdTask DescribeCompletesByUserIdAsync(
                Request.DescribeCompletesByUserIdRequest request
        )
		{
			return new DescribeCompletesByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.DescribeCompletesByUserIdResult> DescribeCompletesByUserIdAsync(
                Request.DescribeCompletesByUserIdRequest request
        ) =>
            new DescribeCompletesByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class CompleteTask : Gs2RestSessionTask<CompleteRequest, CompleteResult>
        {
            public CompleteTask(IGs2Session session, RestSessionRequestFactory factory, CompleteRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(CompleteRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "mission")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/me/complete/group/{missionGroupName}/task/{missionTaskName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{missionGroupName}", !string.IsNullOrEmpty(request.MissionGroupName) ? request.MissionGroupName.ToString() : "null");
                url = url.Replace("{missionTaskName}", !string.IsNullOrEmpty(request.MissionTaskName) ? request.MissionTaskName.ToString() : "null");

                var sessionRequest = Factory.Post(url);

                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);
                jsonWriter.WriteObjectStart();
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
		public IEnumerator Complete(
                Request.CompleteRequest request,
                UnityAction<AsyncResult<Result.CompleteResult>> callback
        ) =>
            new CompleteTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.CompleteResult> CompleteFuture(
                Request.CompleteRequest request
        ) =>
            new CompleteTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.CompleteResult> CompleteAsync(
                Request.CompleteRequest request
        ) =>
            new CompleteTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.CompleteResult>();
    #else
		public CompleteTask CompleteAsync(
                Request.CompleteRequest request
        )
		{
			return new CompleteTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.CompleteResult> CompleteAsync(
                Request.CompleteRequest request
        ) =>
            new CompleteTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class CompleteByUserIdTask : Gs2RestSessionTask<CompleteByUserIdRequest, CompleteByUserIdResult>
        {
            public CompleteByUserIdTask(IGs2Session session, RestSessionRequestFactory factory, CompleteByUserIdRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(CompleteByUserIdRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "mission")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/{userId}/complete/group/{missionGroupName}/task/{missionTaskName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{missionGroupName}", !string.IsNullOrEmpty(request.MissionGroupName) ? request.MissionGroupName.ToString() : "null");
                url = url.Replace("{missionTaskName}", !string.IsNullOrEmpty(request.MissionTaskName) ? request.MissionTaskName.ToString() : "null");
                url = url.Replace("{userId}", !string.IsNullOrEmpty(request.UserId) ? request.UserId.ToString() : "null");

                var sessionRequest = Factory.Post(url);

                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);
                jsonWriter.WriteObjectStart();
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
		public IEnumerator CompleteByUserId(
                Request.CompleteByUserIdRequest request,
                UnityAction<AsyncResult<Result.CompleteByUserIdResult>> callback
        ) =>
            new CompleteByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.CompleteByUserIdResult> CompleteByUserIdFuture(
                Request.CompleteByUserIdRequest request
        ) =>
            new CompleteByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.CompleteByUserIdResult> CompleteByUserIdAsync(
                Request.CompleteByUserIdRequest request
        ) =>
            new CompleteByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.CompleteByUserIdResult>();
    #else
		public CompleteByUserIdTask CompleteByUserIdAsync(
                Request.CompleteByUserIdRequest request
        )
		{
			return new CompleteByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.CompleteByUserIdResult> CompleteByUserIdAsync(
                Request.CompleteByUserIdRequest request
        ) =>
            new CompleteByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class BatchCompleteTask : Gs2RestSessionTask<BatchCompleteRequest, BatchCompleteResult>
        {
            public BatchCompleteTask(IGs2Session session, RestSessionRequestFactory factory, BatchCompleteRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(BatchCompleteRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "mission")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/me/complete/group/{missionGroupName}/task/any/batch";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{missionGroupName}", !string.IsNullOrEmpty(request.MissionGroupName) ? request.MissionGroupName.ToString() : "null");

                var sessionRequest = Factory.Post(url);

                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);
                jsonWriter.WriteObjectStart();
                if (request.MissionTaskNames != null)
                {
                    jsonWriter.WritePropertyName("missionTaskNames");
                    jsonWriter.WriteArrayStart();
                    foreach(var item in request.MissionTaskNames)
                    {
                        jsonWriter.Write(item);
                    }
                    jsonWriter.WriteArrayEnd();
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
		public IEnumerator BatchComplete(
                Request.BatchCompleteRequest request,
                UnityAction<AsyncResult<Result.BatchCompleteResult>> callback
        ) =>
            new BatchCompleteTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.BatchCompleteResult> BatchCompleteFuture(
                Request.BatchCompleteRequest request
        ) =>
            new BatchCompleteTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.BatchCompleteResult> BatchCompleteAsync(
                Request.BatchCompleteRequest request
        ) =>
            new BatchCompleteTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.BatchCompleteResult>();
    #else
		public BatchCompleteTask BatchCompleteAsync(
                Request.BatchCompleteRequest request
        )
		{
			return new BatchCompleteTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.BatchCompleteResult> BatchCompleteAsync(
                Request.BatchCompleteRequest request
        ) =>
            new BatchCompleteTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class BatchCompleteByUserIdTask : Gs2RestSessionTask<BatchCompleteByUserIdRequest, BatchCompleteByUserIdResult>
        {
            public BatchCompleteByUserIdTask(IGs2Session session, RestSessionRequestFactory factory, BatchCompleteByUserIdRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(BatchCompleteByUserIdRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "mission")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/{userId}/complete/group/{missionGroupName}/task/any/batch";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{missionGroupName}", !string.IsNullOrEmpty(request.MissionGroupName) ? request.MissionGroupName.ToString() : "null");
                url = url.Replace("{userId}", !string.IsNullOrEmpty(request.UserId) ? request.UserId.ToString() : "null");

                var sessionRequest = Factory.Post(url);

                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);
                jsonWriter.WriteObjectStart();
                if (request.MissionTaskNames != null)
                {
                    jsonWriter.WritePropertyName("missionTaskNames");
                    jsonWriter.WriteArrayStart();
                    foreach(var item in request.MissionTaskNames)
                    {
                        jsonWriter.Write(item);
                    }
                    jsonWriter.WriteArrayEnd();
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
		public IEnumerator BatchCompleteByUserId(
                Request.BatchCompleteByUserIdRequest request,
                UnityAction<AsyncResult<Result.BatchCompleteByUserIdResult>> callback
        ) =>
            new BatchCompleteByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.BatchCompleteByUserIdResult> BatchCompleteByUserIdFuture(
                Request.BatchCompleteByUserIdRequest request
        ) =>
            new BatchCompleteByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.BatchCompleteByUserIdResult> BatchCompleteByUserIdAsync(
                Request.BatchCompleteByUserIdRequest request
        ) =>
            new BatchCompleteByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.BatchCompleteByUserIdResult>();
    #else
		public BatchCompleteByUserIdTask BatchCompleteByUserIdAsync(
                Request.BatchCompleteByUserIdRequest request
        )
		{
			return new BatchCompleteByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.BatchCompleteByUserIdResult> BatchCompleteByUserIdAsync(
                Request.BatchCompleteByUserIdRequest request
        ) =>
            new BatchCompleteByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class ReceiveByUserIdTask : Gs2RestSessionTask<ReceiveByUserIdRequest, ReceiveByUserIdResult>
        {
            public ReceiveByUserIdTask(IGs2Session session, RestSessionRequestFactory factory, ReceiveByUserIdRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(ReceiveByUserIdRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "mission")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/{userId}/complete/group/{missionGroupName}/task/{missionTaskName}/receive";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{missionGroupName}", !string.IsNullOrEmpty(request.MissionGroupName) ? request.MissionGroupName.ToString() : "null");
                url = url.Replace("{missionTaskName}", !string.IsNullOrEmpty(request.MissionTaskName) ? request.MissionTaskName.ToString() : "null");
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
		public IEnumerator ReceiveByUserId(
                Request.ReceiveByUserIdRequest request,
                UnityAction<AsyncResult<Result.ReceiveByUserIdResult>> callback
        ) =>
            new ReceiveByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.ReceiveByUserIdResult> ReceiveByUserIdFuture(
                Request.ReceiveByUserIdRequest request
        ) =>
            new ReceiveByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.ReceiveByUserIdResult> ReceiveByUserIdAsync(
                Request.ReceiveByUserIdRequest request
        ) =>
            new ReceiveByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.ReceiveByUserIdResult>();
    #else
		public ReceiveByUserIdTask ReceiveByUserIdAsync(
                Request.ReceiveByUserIdRequest request
        )
		{
			return new ReceiveByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.ReceiveByUserIdResult> ReceiveByUserIdAsync(
                Request.ReceiveByUserIdRequest request
        ) =>
            new ReceiveByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class BatchReceiveByUserIdTask : Gs2RestSessionTask<BatchReceiveByUserIdRequest, BatchReceiveByUserIdResult>
        {
            public BatchReceiveByUserIdTask(IGs2Session session, RestSessionRequestFactory factory, BatchReceiveByUserIdRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(BatchReceiveByUserIdRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "mission")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/{userId}/complete/group/{missionGroupName}/task/any/receive/batch";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{missionGroupName}", !string.IsNullOrEmpty(request.MissionGroupName) ? request.MissionGroupName.ToString() : "null");
                url = url.Replace("{userId}", !string.IsNullOrEmpty(request.UserId) ? request.UserId.ToString() : "null");

                var sessionRequest = Factory.Post(url);

                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);
                jsonWriter.WriteObjectStart();
                if (request.MissionTaskNames != null)
                {
                    jsonWriter.WritePropertyName("missionTaskNames");
                    jsonWriter.WriteArrayStart();
                    foreach(var item in request.MissionTaskNames)
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
		public IEnumerator BatchReceiveByUserId(
                Request.BatchReceiveByUserIdRequest request,
                UnityAction<AsyncResult<Result.BatchReceiveByUserIdResult>> callback
        ) =>
            new BatchReceiveByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.BatchReceiveByUserIdResult> BatchReceiveByUserIdFuture(
                Request.BatchReceiveByUserIdRequest request
        ) =>
            new BatchReceiveByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.BatchReceiveByUserIdResult> BatchReceiveByUserIdAsync(
                Request.BatchReceiveByUserIdRequest request
        ) =>
            new BatchReceiveByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.BatchReceiveByUserIdResult>();
    #else
		public BatchReceiveByUserIdTask BatchReceiveByUserIdAsync(
                Request.BatchReceiveByUserIdRequest request
        )
		{
			return new BatchReceiveByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.BatchReceiveByUserIdResult> BatchReceiveByUserIdAsync(
                Request.BatchReceiveByUserIdRequest request
        ) =>
            new BatchReceiveByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class RevertReceiveByUserIdTask : Gs2RestSessionTask<RevertReceiveByUserIdRequest, RevertReceiveByUserIdResult>
        {
            public RevertReceiveByUserIdTask(IGs2Session session, RestSessionRequestFactory factory, RevertReceiveByUserIdRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(RevertReceiveByUserIdRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "mission")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/{userId}/complete/group/{missionGroupName}/task/{missionTaskName}/revert";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{missionGroupName}", !string.IsNullOrEmpty(request.MissionGroupName) ? request.MissionGroupName.ToString() : "null");
                url = url.Replace("{missionTaskName}", !string.IsNullOrEmpty(request.MissionTaskName) ? request.MissionTaskName.ToString() : "null");
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
		public IEnumerator RevertReceiveByUserId(
                Request.RevertReceiveByUserIdRequest request,
                UnityAction<AsyncResult<Result.RevertReceiveByUserIdResult>> callback
        ) =>
            new RevertReceiveByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.RevertReceiveByUserIdResult> RevertReceiveByUserIdFuture(
                Request.RevertReceiveByUserIdRequest request
        ) =>
            new RevertReceiveByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.RevertReceiveByUserIdResult> RevertReceiveByUserIdAsync(
                Request.RevertReceiveByUserIdRequest request
        ) =>
            new RevertReceiveByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.RevertReceiveByUserIdResult>();
    #else
		public RevertReceiveByUserIdTask RevertReceiveByUserIdAsync(
                Request.RevertReceiveByUserIdRequest request
        )
		{
			return new RevertReceiveByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.RevertReceiveByUserIdResult> RevertReceiveByUserIdAsync(
                Request.RevertReceiveByUserIdRequest request
        ) =>
            new RevertReceiveByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class GetCompleteTask : Gs2RestSessionTask<GetCompleteRequest, GetCompleteResult>
        {
            public GetCompleteTask(IGs2Session session, RestSessionRequestFactory factory, GetCompleteRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(GetCompleteRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "mission")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/me/complete/group/{missionGroupName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{missionGroupName}", !string.IsNullOrEmpty(request.MissionGroupName) ? request.MissionGroupName.ToString() : "null");

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
		public IEnumerator GetComplete(
                Request.GetCompleteRequest request,
                UnityAction<AsyncResult<Result.GetCompleteResult>> callback
        ) =>
            new GetCompleteTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.GetCompleteResult> GetCompleteFuture(
                Request.GetCompleteRequest request
        ) =>
            new GetCompleteTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.GetCompleteResult> GetCompleteAsync(
                Request.GetCompleteRequest request
        ) =>
            new GetCompleteTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.GetCompleteResult>();
    #else
		public GetCompleteTask GetCompleteAsync(
                Request.GetCompleteRequest request
        )
		{
			return new GetCompleteTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.GetCompleteResult> GetCompleteAsync(
                Request.GetCompleteRequest request
        ) =>
            new GetCompleteTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class GetCompleteByUserIdTask : Gs2RestSessionTask<GetCompleteByUserIdRequest, GetCompleteByUserIdResult>
        {
            public GetCompleteByUserIdTask(IGs2Session session, RestSessionRequestFactory factory, GetCompleteByUserIdRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(GetCompleteByUserIdRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "mission")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/{userId}/complete/group/{missionGroupName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{missionGroupName}", !string.IsNullOrEmpty(request.MissionGroupName) ? request.MissionGroupName.ToString() : "null");
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
		public IEnumerator GetCompleteByUserId(
                Request.GetCompleteByUserIdRequest request,
                UnityAction<AsyncResult<Result.GetCompleteByUserIdResult>> callback
        ) =>
            new GetCompleteByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.GetCompleteByUserIdResult> GetCompleteByUserIdFuture(
                Request.GetCompleteByUserIdRequest request
        ) =>
            new GetCompleteByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.GetCompleteByUserIdResult> GetCompleteByUserIdAsync(
                Request.GetCompleteByUserIdRequest request
        ) =>
            new GetCompleteByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.GetCompleteByUserIdResult>();
    #else
		public GetCompleteByUserIdTask GetCompleteByUserIdAsync(
                Request.GetCompleteByUserIdRequest request
        )
		{
			return new GetCompleteByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.GetCompleteByUserIdResult> GetCompleteByUserIdAsync(
                Request.GetCompleteByUserIdRequest request
        ) =>
            new GetCompleteByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class EvaluateCompleteTask : Gs2RestSessionTask<EvaluateCompleteRequest, EvaluateCompleteResult>
        {
            public EvaluateCompleteTask(IGs2Session session, RestSessionRequestFactory factory, EvaluateCompleteRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(EvaluateCompleteRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "mission")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/me/complete/group/{missionGroupName}/eval";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{missionGroupName}", !string.IsNullOrEmpty(request.MissionGroupName) ? request.MissionGroupName.ToString() : "null");

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
		public IEnumerator EvaluateComplete(
                Request.EvaluateCompleteRequest request,
                UnityAction<AsyncResult<Result.EvaluateCompleteResult>> callback
        ) =>
            new EvaluateCompleteTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.EvaluateCompleteResult> EvaluateCompleteFuture(
                Request.EvaluateCompleteRequest request
        ) =>
            new EvaluateCompleteTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.EvaluateCompleteResult> EvaluateCompleteAsync(
                Request.EvaluateCompleteRequest request
        ) =>
            new EvaluateCompleteTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.EvaluateCompleteResult>();
    #else
		public EvaluateCompleteTask EvaluateCompleteAsync(
                Request.EvaluateCompleteRequest request
        )
		{
			return new EvaluateCompleteTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.EvaluateCompleteResult> EvaluateCompleteAsync(
                Request.EvaluateCompleteRequest request
        ) =>
            new EvaluateCompleteTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class EvaluateCompleteByUserIdTask : Gs2RestSessionTask<EvaluateCompleteByUserIdRequest, EvaluateCompleteByUserIdResult>
        {
            public EvaluateCompleteByUserIdTask(IGs2Session session, RestSessionRequestFactory factory, EvaluateCompleteByUserIdRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(EvaluateCompleteByUserIdRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "mission")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/{userId}/complete/group/{missionGroupName}/eval";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{userId}", !string.IsNullOrEmpty(request.UserId) ? request.UserId.ToString() : "null");
                url = url.Replace("{missionGroupName}", !string.IsNullOrEmpty(request.MissionGroupName) ? request.MissionGroupName.ToString() : "null");

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
		public IEnumerator EvaluateCompleteByUserId(
                Request.EvaluateCompleteByUserIdRequest request,
                UnityAction<AsyncResult<Result.EvaluateCompleteByUserIdResult>> callback
        ) =>
            new EvaluateCompleteByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.EvaluateCompleteByUserIdResult> EvaluateCompleteByUserIdFuture(
                Request.EvaluateCompleteByUserIdRequest request
        ) =>
            new EvaluateCompleteByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.EvaluateCompleteByUserIdResult> EvaluateCompleteByUserIdAsync(
                Request.EvaluateCompleteByUserIdRequest request
        ) =>
            new EvaluateCompleteByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.EvaluateCompleteByUserIdResult>();
    #else
		public EvaluateCompleteByUserIdTask EvaluateCompleteByUserIdAsync(
                Request.EvaluateCompleteByUserIdRequest request
        )
		{
			return new EvaluateCompleteByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.EvaluateCompleteByUserIdResult> EvaluateCompleteByUserIdAsync(
                Request.EvaluateCompleteByUserIdRequest request
        ) =>
            new EvaluateCompleteByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class DeleteCompleteByUserIdTask : Gs2RestSessionTask<DeleteCompleteByUserIdRequest, DeleteCompleteByUserIdResult>
        {
            public DeleteCompleteByUserIdTask(IGs2Session session, RestSessionRequestFactory factory, DeleteCompleteByUserIdRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DeleteCompleteByUserIdRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "mission")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/{userId}/complete/group/{missionGroupName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{userId}", !string.IsNullOrEmpty(request.UserId) ? request.UserId.ToString() : "null");
                url = url.Replace("{missionGroupName}", !string.IsNullOrEmpty(request.MissionGroupName) ? request.MissionGroupName.ToString() : "null");

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
		public IEnumerator DeleteCompleteByUserId(
                Request.DeleteCompleteByUserIdRequest request,
                UnityAction<AsyncResult<Result.DeleteCompleteByUserIdResult>> callback
        ) =>
            new DeleteCompleteByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.DeleteCompleteByUserIdResult> DeleteCompleteByUserIdFuture(
                Request.DeleteCompleteByUserIdRequest request
        ) =>
            new DeleteCompleteByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.DeleteCompleteByUserIdResult> DeleteCompleteByUserIdAsync(
                Request.DeleteCompleteByUserIdRequest request
        ) =>
            new DeleteCompleteByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.DeleteCompleteByUserIdResult>();
    #else
		public DeleteCompleteByUserIdTask DeleteCompleteByUserIdAsync(
                Request.DeleteCompleteByUserIdRequest request
        )
		{
			return new DeleteCompleteByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.DeleteCompleteByUserIdResult> DeleteCompleteByUserIdAsync(
                Request.DeleteCompleteByUserIdRequest request
        ) =>
            new DeleteCompleteByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class VerifyCompleteTask : Gs2RestSessionTask<VerifyCompleteRequest, VerifyCompleteResult>
        {
            public VerifyCompleteTask(IGs2Session session, RestSessionRequestFactory factory, VerifyCompleteRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(VerifyCompleteRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "mission")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/me/complete/group/{missionGroupName}/task/{missionTaskName}/verify/{verifyType}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{missionGroupName}", !string.IsNullOrEmpty(request.MissionGroupName) ? request.MissionGroupName.ToString() : "null");
                url = url.Replace("{verifyType}", !string.IsNullOrEmpty(request.VerifyType) ? request.VerifyType.ToString() : "null");
                url = url.Replace("{missionTaskName}", !string.IsNullOrEmpty(request.MissionTaskName) ? request.MissionTaskName.ToString() : "null");
                url = url.Replace("{multiplyValueSpecifyingQuantity}",request.MultiplyValueSpecifyingQuantity != null ? request.MultiplyValueSpecifyingQuantity.ToString() : "null");

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
		public IEnumerator VerifyComplete(
                Request.VerifyCompleteRequest request,
                UnityAction<AsyncResult<Result.VerifyCompleteResult>> callback
        ) =>
            new VerifyCompleteTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.VerifyCompleteResult> VerifyCompleteFuture(
                Request.VerifyCompleteRequest request
        ) =>
            new VerifyCompleteTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.VerifyCompleteResult> VerifyCompleteAsync(
                Request.VerifyCompleteRequest request
        ) =>
            new VerifyCompleteTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.VerifyCompleteResult>();
    #else
		public VerifyCompleteTask VerifyCompleteAsync(
                Request.VerifyCompleteRequest request
        )
		{
			return new VerifyCompleteTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.VerifyCompleteResult> VerifyCompleteAsync(
                Request.VerifyCompleteRequest request
        ) =>
            new VerifyCompleteTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class VerifyCompleteByUserIdTask : Gs2RestSessionTask<VerifyCompleteByUserIdRequest, VerifyCompleteByUserIdResult>
        {
            public VerifyCompleteByUserIdTask(IGs2Session session, RestSessionRequestFactory factory, VerifyCompleteByUserIdRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(VerifyCompleteByUserIdRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "mission")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/{userId}/complete/group/{missionGroupName}/task/{missionTaskName}/verify/{verifyType}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{missionGroupName}", !string.IsNullOrEmpty(request.MissionGroupName) ? request.MissionGroupName.ToString() : "null");
                url = url.Replace("{userId}", !string.IsNullOrEmpty(request.UserId) ? request.UserId.ToString() : "null");
                url = url.Replace("{verifyType}", !string.IsNullOrEmpty(request.VerifyType) ? request.VerifyType.ToString() : "null");
                url = url.Replace("{missionTaskName}", !string.IsNullOrEmpty(request.MissionTaskName) ? request.MissionTaskName.ToString() : "null");
                url = url.Replace("{multiplyValueSpecifyingQuantity}",request.MultiplyValueSpecifyingQuantity != null ? request.MultiplyValueSpecifyingQuantity.ToString() : "null");

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
		public IEnumerator VerifyCompleteByUserId(
                Request.VerifyCompleteByUserIdRequest request,
                UnityAction<AsyncResult<Result.VerifyCompleteByUserIdResult>> callback
        ) =>
            new VerifyCompleteByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.VerifyCompleteByUserIdResult> VerifyCompleteByUserIdFuture(
                Request.VerifyCompleteByUserIdRequest request
        ) =>
            new VerifyCompleteByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.VerifyCompleteByUserIdResult> VerifyCompleteByUserIdAsync(
                Request.VerifyCompleteByUserIdRequest request
        ) =>
            new VerifyCompleteByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.VerifyCompleteByUserIdResult>();
    #else
		public VerifyCompleteByUserIdTask VerifyCompleteByUserIdAsync(
                Request.VerifyCompleteByUserIdRequest request
        )
		{
			return new VerifyCompleteByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.VerifyCompleteByUserIdResult> VerifyCompleteByUserIdAsync(
                Request.VerifyCompleteByUserIdRequest request
        ) =>
            new VerifyCompleteByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class ReceiveByStampTaskTask : Gs2RestSessionTask<ReceiveByStampTaskRequest, ReceiveByStampTaskResult>
        {
            public ReceiveByStampTaskTask(IGs2Session session, RestSessionRequestFactory factory, ReceiveByStampTaskRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(ReceiveByStampTaskRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "mission")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/stamp/receive";

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
		public IEnumerator ReceiveByStampTask(
                Request.ReceiveByStampTaskRequest request,
                UnityAction<AsyncResult<Result.ReceiveByStampTaskResult>> callback
        ) =>
            new ReceiveByStampTaskTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.ReceiveByStampTaskResult> ReceiveByStampTaskFuture(
                Request.ReceiveByStampTaskRequest request
        ) =>
            new ReceiveByStampTaskTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.ReceiveByStampTaskResult> ReceiveByStampTaskAsync(
                Request.ReceiveByStampTaskRequest request
        ) =>
            new ReceiveByStampTaskTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.ReceiveByStampTaskResult>();
    #else
		public ReceiveByStampTaskTask ReceiveByStampTaskAsync(
                Request.ReceiveByStampTaskRequest request
        )
		{
			return new ReceiveByStampTaskTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.ReceiveByStampTaskResult> ReceiveByStampTaskAsync(
                Request.ReceiveByStampTaskRequest request
        ) =>
            new ReceiveByStampTaskTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class BatchReceiveByStampTaskTask : Gs2RestSessionTask<BatchReceiveByStampTaskRequest, BatchReceiveByStampTaskResult>
        {
            public BatchReceiveByStampTaskTask(IGs2Session session, RestSessionRequestFactory factory, BatchReceiveByStampTaskRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(BatchReceiveByStampTaskRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "mission")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/stamp/receive/batch";

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
		public IEnumerator BatchReceiveByStampTask(
                Request.BatchReceiveByStampTaskRequest request,
                UnityAction<AsyncResult<Result.BatchReceiveByStampTaskResult>> callback
        ) =>
            new BatchReceiveByStampTaskTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.BatchReceiveByStampTaskResult> BatchReceiveByStampTaskFuture(
                Request.BatchReceiveByStampTaskRequest request
        ) =>
            new BatchReceiveByStampTaskTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.BatchReceiveByStampTaskResult> BatchReceiveByStampTaskAsync(
                Request.BatchReceiveByStampTaskRequest request
        ) =>
            new BatchReceiveByStampTaskTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.BatchReceiveByStampTaskResult>();
    #else
		public BatchReceiveByStampTaskTask BatchReceiveByStampTaskAsync(
                Request.BatchReceiveByStampTaskRequest request
        )
		{
			return new BatchReceiveByStampTaskTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.BatchReceiveByStampTaskResult> BatchReceiveByStampTaskAsync(
                Request.BatchReceiveByStampTaskRequest request
        ) =>
            new BatchReceiveByStampTaskTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class RevertReceiveByStampSheetTask : Gs2RestSessionTask<RevertReceiveByStampSheetRequest, RevertReceiveByStampSheetResult>
        {
            public RevertReceiveByStampSheetTask(IGs2Session session, RestSessionRequestFactory factory, RevertReceiveByStampSheetRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(RevertReceiveByStampSheetRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "mission")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/stamp/revert";

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
		public IEnumerator RevertReceiveByStampSheet(
                Request.RevertReceiveByStampSheetRequest request,
                UnityAction<AsyncResult<Result.RevertReceiveByStampSheetResult>> callback
        ) =>
            new RevertReceiveByStampSheetTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.RevertReceiveByStampSheetResult> RevertReceiveByStampSheetFuture(
                Request.RevertReceiveByStampSheetRequest request
        ) =>
            new RevertReceiveByStampSheetTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.RevertReceiveByStampSheetResult> RevertReceiveByStampSheetAsync(
                Request.RevertReceiveByStampSheetRequest request
        ) =>
            new RevertReceiveByStampSheetTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.RevertReceiveByStampSheetResult>();
    #else
		public RevertReceiveByStampSheetTask RevertReceiveByStampSheetAsync(
                Request.RevertReceiveByStampSheetRequest request
        )
		{
			return new RevertReceiveByStampSheetTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.RevertReceiveByStampSheetResult> RevertReceiveByStampSheetAsync(
                Request.RevertReceiveByStampSheetRequest request
        ) =>
            new RevertReceiveByStampSheetTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class VerifyCompleteByStampTaskTask : Gs2RestSessionTask<VerifyCompleteByStampTaskRequest, VerifyCompleteByStampTaskResult>
        {
            public VerifyCompleteByStampTaskTask(IGs2Session session, RestSessionRequestFactory factory, VerifyCompleteByStampTaskRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(VerifyCompleteByStampTaskRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "mission")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/stamp/complete/verify";

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
		public IEnumerator VerifyCompleteByStampTask(
                Request.VerifyCompleteByStampTaskRequest request,
                UnityAction<AsyncResult<Result.VerifyCompleteByStampTaskResult>> callback
        ) =>
            new VerifyCompleteByStampTaskTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.VerifyCompleteByStampTaskResult> VerifyCompleteByStampTaskFuture(
                Request.VerifyCompleteByStampTaskRequest request
        ) =>
            new VerifyCompleteByStampTaskTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.VerifyCompleteByStampTaskResult> VerifyCompleteByStampTaskAsync(
                Request.VerifyCompleteByStampTaskRequest request
        ) =>
            new VerifyCompleteByStampTaskTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.VerifyCompleteByStampTaskResult>();
    #else
		public VerifyCompleteByStampTaskTask VerifyCompleteByStampTaskAsync(
                Request.VerifyCompleteByStampTaskRequest request
        )
		{
			return new VerifyCompleteByStampTaskTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.VerifyCompleteByStampTaskResult> VerifyCompleteByStampTaskAsync(
                Request.VerifyCompleteByStampTaskRequest request
        ) =>
            new VerifyCompleteByStampTaskTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class DescribeCounterModelMastersTask : Gs2RestSessionTask<DescribeCounterModelMastersRequest, DescribeCounterModelMastersResult>
        {
            public DescribeCounterModelMastersTask(IGs2Session session, RestSessionRequestFactory factory, DescribeCounterModelMastersRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DescribeCounterModelMastersRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "mission")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/master/counter";

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
		public IEnumerator DescribeCounterModelMasters(
                Request.DescribeCounterModelMastersRequest request,
                UnityAction<AsyncResult<Result.DescribeCounterModelMastersResult>> callback
        ) =>
            new DescribeCounterModelMastersTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.DescribeCounterModelMastersResult> DescribeCounterModelMastersFuture(
                Request.DescribeCounterModelMastersRequest request
        ) =>
            new DescribeCounterModelMastersTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.DescribeCounterModelMastersResult> DescribeCounterModelMastersAsync(
                Request.DescribeCounterModelMastersRequest request
        ) =>
            new DescribeCounterModelMastersTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.DescribeCounterModelMastersResult>();
    #else
		public DescribeCounterModelMastersTask DescribeCounterModelMastersAsync(
                Request.DescribeCounterModelMastersRequest request
        )
		{
			return new DescribeCounterModelMastersTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.DescribeCounterModelMastersResult> DescribeCounterModelMastersAsync(
                Request.DescribeCounterModelMastersRequest request
        ) =>
            new DescribeCounterModelMastersTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class CreateCounterModelMasterTask : Gs2RestSessionTask<CreateCounterModelMasterRequest, CreateCounterModelMasterResult>
        {
            public CreateCounterModelMasterTask(IGs2Session session, RestSessionRequestFactory factory, CreateCounterModelMasterRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(CreateCounterModelMasterRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "mission")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/master/counter";

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
                if (request.Metadata != null)
                {
                    jsonWriter.WritePropertyName("metadata");
                    jsonWriter.Write(request.Metadata);
                }
                if (request.Description != null)
                {
                    jsonWriter.WritePropertyName("description");
                    jsonWriter.Write(request.Description);
                }
                if (request.Scopes != null)
                {
                    jsonWriter.WritePropertyName("scopes");
                    jsonWriter.WriteArrayStart();
                    foreach(var item in request.Scopes)
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
		public IEnumerator CreateCounterModelMaster(
                Request.CreateCounterModelMasterRequest request,
                UnityAction<AsyncResult<Result.CreateCounterModelMasterResult>> callback
        ) =>
            new CreateCounterModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.CreateCounterModelMasterResult> CreateCounterModelMasterFuture(
                Request.CreateCounterModelMasterRequest request
        ) =>
            new CreateCounterModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.CreateCounterModelMasterResult> CreateCounterModelMasterAsync(
                Request.CreateCounterModelMasterRequest request
        ) =>
            new CreateCounterModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.CreateCounterModelMasterResult>();
    #else
		public CreateCounterModelMasterTask CreateCounterModelMasterAsync(
                Request.CreateCounterModelMasterRequest request
        )
		{
			return new CreateCounterModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.CreateCounterModelMasterResult> CreateCounterModelMasterAsync(
                Request.CreateCounterModelMasterRequest request
        ) =>
            new CreateCounterModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class GetCounterModelMasterTask : Gs2RestSessionTask<GetCounterModelMasterRequest, GetCounterModelMasterResult>
        {
            public GetCounterModelMasterTask(IGs2Session session, RestSessionRequestFactory factory, GetCounterModelMasterRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(GetCounterModelMasterRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "mission")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/master/counter/{counterName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{counterName}", !string.IsNullOrEmpty(request.CounterName) ? request.CounterName.ToString() : "null");

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
		public IEnumerator GetCounterModelMaster(
                Request.GetCounterModelMasterRequest request,
                UnityAction<AsyncResult<Result.GetCounterModelMasterResult>> callback
        ) =>
            new GetCounterModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.GetCounterModelMasterResult> GetCounterModelMasterFuture(
                Request.GetCounterModelMasterRequest request
        ) =>
            new GetCounterModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.GetCounterModelMasterResult> GetCounterModelMasterAsync(
                Request.GetCounterModelMasterRequest request
        ) =>
            new GetCounterModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.GetCounterModelMasterResult>();
    #else
		public GetCounterModelMasterTask GetCounterModelMasterAsync(
                Request.GetCounterModelMasterRequest request
        )
		{
			return new GetCounterModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.GetCounterModelMasterResult> GetCounterModelMasterAsync(
                Request.GetCounterModelMasterRequest request
        ) =>
            new GetCounterModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class UpdateCounterModelMasterTask : Gs2RestSessionTask<UpdateCounterModelMasterRequest, UpdateCounterModelMasterResult>
        {
            public UpdateCounterModelMasterTask(IGs2Session session, RestSessionRequestFactory factory, UpdateCounterModelMasterRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(UpdateCounterModelMasterRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "mission")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/master/counter/{counterName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{counterName}", !string.IsNullOrEmpty(request.CounterName) ? request.CounterName.ToString() : "null");

                var sessionRequest = Factory.Put(url);

                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);
                jsonWriter.WriteObjectStart();
                if (request.Metadata != null)
                {
                    jsonWriter.WritePropertyName("metadata");
                    jsonWriter.Write(request.Metadata);
                }
                if (request.Description != null)
                {
                    jsonWriter.WritePropertyName("description");
                    jsonWriter.Write(request.Description);
                }
                if (request.Scopes != null)
                {
                    jsonWriter.WritePropertyName("scopes");
                    jsonWriter.WriteArrayStart();
                    foreach(var item in request.Scopes)
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
		public IEnumerator UpdateCounterModelMaster(
                Request.UpdateCounterModelMasterRequest request,
                UnityAction<AsyncResult<Result.UpdateCounterModelMasterResult>> callback
        ) =>
            new UpdateCounterModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.UpdateCounterModelMasterResult> UpdateCounterModelMasterFuture(
                Request.UpdateCounterModelMasterRequest request
        ) =>
            new UpdateCounterModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.UpdateCounterModelMasterResult> UpdateCounterModelMasterAsync(
                Request.UpdateCounterModelMasterRequest request
        ) =>
            new UpdateCounterModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.UpdateCounterModelMasterResult>();
    #else
		public UpdateCounterModelMasterTask UpdateCounterModelMasterAsync(
                Request.UpdateCounterModelMasterRequest request
        )
		{
			return new UpdateCounterModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.UpdateCounterModelMasterResult> UpdateCounterModelMasterAsync(
                Request.UpdateCounterModelMasterRequest request
        ) =>
            new UpdateCounterModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class DeleteCounterModelMasterTask : Gs2RestSessionTask<DeleteCounterModelMasterRequest, DeleteCounterModelMasterResult>
        {
            public DeleteCounterModelMasterTask(IGs2Session session, RestSessionRequestFactory factory, DeleteCounterModelMasterRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DeleteCounterModelMasterRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "mission")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/master/counter/{counterName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{counterName}", !string.IsNullOrEmpty(request.CounterName) ? request.CounterName.ToString() : "null");

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
		public IEnumerator DeleteCounterModelMaster(
                Request.DeleteCounterModelMasterRequest request,
                UnityAction<AsyncResult<Result.DeleteCounterModelMasterResult>> callback
        ) =>
            new DeleteCounterModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.DeleteCounterModelMasterResult> DeleteCounterModelMasterFuture(
                Request.DeleteCounterModelMasterRequest request
        ) =>
            new DeleteCounterModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.DeleteCounterModelMasterResult> DeleteCounterModelMasterAsync(
                Request.DeleteCounterModelMasterRequest request
        ) =>
            new DeleteCounterModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.DeleteCounterModelMasterResult>();
    #else
		public DeleteCounterModelMasterTask DeleteCounterModelMasterAsync(
                Request.DeleteCounterModelMasterRequest request
        )
		{
			return new DeleteCounterModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.DeleteCounterModelMasterResult> DeleteCounterModelMasterAsync(
                Request.DeleteCounterModelMasterRequest request
        ) =>
            new DeleteCounterModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class DescribeMissionGroupModelMastersTask : Gs2RestSessionTask<DescribeMissionGroupModelMastersRequest, DescribeMissionGroupModelMastersResult>
        {
            public DescribeMissionGroupModelMastersTask(IGs2Session session, RestSessionRequestFactory factory, DescribeMissionGroupModelMastersRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DescribeMissionGroupModelMastersRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "mission")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/master/group";

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
		public IEnumerator DescribeMissionGroupModelMasters(
                Request.DescribeMissionGroupModelMastersRequest request,
                UnityAction<AsyncResult<Result.DescribeMissionGroupModelMastersResult>> callback
        ) =>
            new DescribeMissionGroupModelMastersTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.DescribeMissionGroupModelMastersResult> DescribeMissionGroupModelMastersFuture(
                Request.DescribeMissionGroupModelMastersRequest request
        ) =>
            new DescribeMissionGroupModelMastersTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.DescribeMissionGroupModelMastersResult> DescribeMissionGroupModelMastersAsync(
                Request.DescribeMissionGroupModelMastersRequest request
        ) =>
            new DescribeMissionGroupModelMastersTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.DescribeMissionGroupModelMastersResult>();
    #else
		public DescribeMissionGroupModelMastersTask DescribeMissionGroupModelMastersAsync(
                Request.DescribeMissionGroupModelMastersRequest request
        )
		{
			return new DescribeMissionGroupModelMastersTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.DescribeMissionGroupModelMastersResult> DescribeMissionGroupModelMastersAsync(
                Request.DescribeMissionGroupModelMastersRequest request
        ) =>
            new DescribeMissionGroupModelMastersTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class CreateMissionGroupModelMasterTask : Gs2RestSessionTask<CreateMissionGroupModelMasterRequest, CreateMissionGroupModelMasterResult>
        {
            public CreateMissionGroupModelMasterTask(IGs2Session session, RestSessionRequestFactory factory, CreateMissionGroupModelMasterRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(CreateMissionGroupModelMasterRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "mission")
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
                if (request.Metadata != null)
                {
                    jsonWriter.WritePropertyName("metadata");
                    jsonWriter.Write(request.Metadata);
                }
                if (request.Description != null)
                {
                    jsonWriter.WritePropertyName("description");
                    jsonWriter.Write(request.Description);
                }
                if (request.ResetType != null)
                {
                    jsonWriter.WritePropertyName("resetType");
                    jsonWriter.Write(request.ResetType);
                }
                if (request.ResetDayOfMonth != null)
                {
                    jsonWriter.WritePropertyName("resetDayOfMonth");
                    jsonWriter.Write(request.ResetDayOfMonth.ToString());
                }
                if (request.ResetDayOfWeek != null)
                {
                    jsonWriter.WritePropertyName("resetDayOfWeek");
                    jsonWriter.Write(request.ResetDayOfWeek);
                }
                if (request.ResetHour != null)
                {
                    jsonWriter.WritePropertyName("resetHour");
                    jsonWriter.Write(request.ResetHour.ToString());
                }
                if (request.AnchorTimestamp != null)
                {
                    jsonWriter.WritePropertyName("anchorTimestamp");
                    jsonWriter.Write(request.AnchorTimestamp.ToString());
                }
                if (request.Days != null)
                {
                    jsonWriter.WritePropertyName("days");
                    jsonWriter.Write(request.Days.ToString());
                }
                if (request.CompleteNotificationNamespaceId != null)
                {
                    jsonWriter.WritePropertyName("completeNotificationNamespaceId");
                    jsonWriter.Write(request.CompleteNotificationNamespaceId);
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
		public IEnumerator CreateMissionGroupModelMaster(
                Request.CreateMissionGroupModelMasterRequest request,
                UnityAction<AsyncResult<Result.CreateMissionGroupModelMasterResult>> callback
        ) =>
            new CreateMissionGroupModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.CreateMissionGroupModelMasterResult> CreateMissionGroupModelMasterFuture(
                Request.CreateMissionGroupModelMasterRequest request
        ) =>
            new CreateMissionGroupModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.CreateMissionGroupModelMasterResult> CreateMissionGroupModelMasterAsync(
                Request.CreateMissionGroupModelMasterRequest request
        ) =>
            new CreateMissionGroupModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.CreateMissionGroupModelMasterResult>();
    #else
		public CreateMissionGroupModelMasterTask CreateMissionGroupModelMasterAsync(
                Request.CreateMissionGroupModelMasterRequest request
        )
		{
			return new CreateMissionGroupModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.CreateMissionGroupModelMasterResult> CreateMissionGroupModelMasterAsync(
                Request.CreateMissionGroupModelMasterRequest request
        ) =>
            new CreateMissionGroupModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class GetMissionGroupModelMasterTask : Gs2RestSessionTask<GetMissionGroupModelMasterRequest, GetMissionGroupModelMasterResult>
        {
            public GetMissionGroupModelMasterTask(IGs2Session session, RestSessionRequestFactory factory, GetMissionGroupModelMasterRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(GetMissionGroupModelMasterRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "mission")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/master/group/{missionGroupName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{missionGroupName}", !string.IsNullOrEmpty(request.MissionGroupName) ? request.MissionGroupName.ToString() : "null");

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
		public IEnumerator GetMissionGroupModelMaster(
                Request.GetMissionGroupModelMasterRequest request,
                UnityAction<AsyncResult<Result.GetMissionGroupModelMasterResult>> callback
        ) =>
            new GetMissionGroupModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.GetMissionGroupModelMasterResult> GetMissionGroupModelMasterFuture(
                Request.GetMissionGroupModelMasterRequest request
        ) =>
            new GetMissionGroupModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.GetMissionGroupModelMasterResult> GetMissionGroupModelMasterAsync(
                Request.GetMissionGroupModelMasterRequest request
        ) =>
            new GetMissionGroupModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.GetMissionGroupModelMasterResult>();
    #else
		public GetMissionGroupModelMasterTask GetMissionGroupModelMasterAsync(
                Request.GetMissionGroupModelMasterRequest request
        )
		{
			return new GetMissionGroupModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.GetMissionGroupModelMasterResult> GetMissionGroupModelMasterAsync(
                Request.GetMissionGroupModelMasterRequest request
        ) =>
            new GetMissionGroupModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class UpdateMissionGroupModelMasterTask : Gs2RestSessionTask<UpdateMissionGroupModelMasterRequest, UpdateMissionGroupModelMasterResult>
        {
            public UpdateMissionGroupModelMasterTask(IGs2Session session, RestSessionRequestFactory factory, UpdateMissionGroupModelMasterRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(UpdateMissionGroupModelMasterRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "mission")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/master/group/{missionGroupName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{missionGroupName}", !string.IsNullOrEmpty(request.MissionGroupName) ? request.MissionGroupName.ToString() : "null");

                var sessionRequest = Factory.Put(url);

                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);
                jsonWriter.WriteObjectStart();
                if (request.Metadata != null)
                {
                    jsonWriter.WritePropertyName("metadata");
                    jsonWriter.Write(request.Metadata);
                }
                if (request.Description != null)
                {
                    jsonWriter.WritePropertyName("description");
                    jsonWriter.Write(request.Description);
                }
                if (request.ResetType != null)
                {
                    jsonWriter.WritePropertyName("resetType");
                    jsonWriter.Write(request.ResetType);
                }
                if (request.ResetDayOfMonth != null)
                {
                    jsonWriter.WritePropertyName("resetDayOfMonth");
                    jsonWriter.Write(request.ResetDayOfMonth.ToString());
                }
                if (request.ResetDayOfWeek != null)
                {
                    jsonWriter.WritePropertyName("resetDayOfWeek");
                    jsonWriter.Write(request.ResetDayOfWeek);
                }
                if (request.ResetHour != null)
                {
                    jsonWriter.WritePropertyName("resetHour");
                    jsonWriter.Write(request.ResetHour.ToString());
                }
                if (request.AnchorTimestamp != null)
                {
                    jsonWriter.WritePropertyName("anchorTimestamp");
                    jsonWriter.Write(request.AnchorTimestamp.ToString());
                }
                if (request.Days != null)
                {
                    jsonWriter.WritePropertyName("days");
                    jsonWriter.Write(request.Days.ToString());
                }
                if (request.CompleteNotificationNamespaceId != null)
                {
                    jsonWriter.WritePropertyName("completeNotificationNamespaceId");
                    jsonWriter.Write(request.CompleteNotificationNamespaceId);
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
		public IEnumerator UpdateMissionGroupModelMaster(
                Request.UpdateMissionGroupModelMasterRequest request,
                UnityAction<AsyncResult<Result.UpdateMissionGroupModelMasterResult>> callback
        ) =>
            new UpdateMissionGroupModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.UpdateMissionGroupModelMasterResult> UpdateMissionGroupModelMasterFuture(
                Request.UpdateMissionGroupModelMasterRequest request
        ) =>
            new UpdateMissionGroupModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.UpdateMissionGroupModelMasterResult> UpdateMissionGroupModelMasterAsync(
                Request.UpdateMissionGroupModelMasterRequest request
        ) =>
            new UpdateMissionGroupModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.UpdateMissionGroupModelMasterResult>();
    #else
		public UpdateMissionGroupModelMasterTask UpdateMissionGroupModelMasterAsync(
                Request.UpdateMissionGroupModelMasterRequest request
        )
		{
			return new UpdateMissionGroupModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.UpdateMissionGroupModelMasterResult> UpdateMissionGroupModelMasterAsync(
                Request.UpdateMissionGroupModelMasterRequest request
        ) =>
            new UpdateMissionGroupModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class DeleteMissionGroupModelMasterTask : Gs2RestSessionTask<DeleteMissionGroupModelMasterRequest, DeleteMissionGroupModelMasterResult>
        {
            public DeleteMissionGroupModelMasterTask(IGs2Session session, RestSessionRequestFactory factory, DeleteMissionGroupModelMasterRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DeleteMissionGroupModelMasterRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "mission")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/master/group/{missionGroupName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{missionGroupName}", !string.IsNullOrEmpty(request.MissionGroupName) ? request.MissionGroupName.ToString() : "null");

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
		public IEnumerator DeleteMissionGroupModelMaster(
                Request.DeleteMissionGroupModelMasterRequest request,
                UnityAction<AsyncResult<Result.DeleteMissionGroupModelMasterResult>> callback
        ) =>
            new DeleteMissionGroupModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.DeleteMissionGroupModelMasterResult> DeleteMissionGroupModelMasterFuture(
                Request.DeleteMissionGroupModelMasterRequest request
        ) =>
            new DeleteMissionGroupModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.DeleteMissionGroupModelMasterResult> DeleteMissionGroupModelMasterAsync(
                Request.DeleteMissionGroupModelMasterRequest request
        ) =>
            new DeleteMissionGroupModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.DeleteMissionGroupModelMasterResult>();
    #else
		public DeleteMissionGroupModelMasterTask DeleteMissionGroupModelMasterAsync(
                Request.DeleteMissionGroupModelMasterRequest request
        )
		{
			return new DeleteMissionGroupModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.DeleteMissionGroupModelMasterResult> DeleteMissionGroupModelMasterAsync(
                Request.DeleteMissionGroupModelMasterRequest request
        ) =>
            new DeleteMissionGroupModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class DescribeNamespacesTask : Gs2RestSessionTask<DescribeNamespacesRequest, DescribeNamespacesResult>
        {
            public DescribeNamespacesTask(IGs2Session session, RestSessionRequestFactory factory, DescribeNamespacesRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DescribeNamespacesRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "mission")
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
                    .Replace("{service}", "mission")
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
                if (request.MissionCompleteScript != null)
                {
                    jsonWriter.WritePropertyName("missionCompleteScript");
                    request.MissionCompleteScript.WriteJson(jsonWriter);
                }
                if (request.CounterIncrementScript != null)
                {
                    jsonWriter.WritePropertyName("counterIncrementScript");
                    request.CounterIncrementScript.WriteJson(jsonWriter);
                }
                if (request.ReceiveRewardsScript != null)
                {
                    jsonWriter.WritePropertyName("receiveRewardsScript");
                    request.ReceiveRewardsScript.WriteJson(jsonWriter);
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
                    .Replace("{service}", "mission")
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
                    .Replace("{service}", "mission")
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
                    .Replace("{service}", "mission")
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
                if (request.MissionCompleteScript != null)
                {
                    jsonWriter.WritePropertyName("missionCompleteScript");
                    request.MissionCompleteScript.WriteJson(jsonWriter);
                }
                if (request.CounterIncrementScript != null)
                {
                    jsonWriter.WritePropertyName("counterIncrementScript");
                    request.CounterIncrementScript.WriteJson(jsonWriter);
                }
                if (request.ReceiveRewardsScript != null)
                {
                    jsonWriter.WritePropertyName("receiveRewardsScript");
                    request.ReceiveRewardsScript.WriteJson(jsonWriter);
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
                    .Replace("{service}", "mission")
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
                    .Replace("{service}", "mission")
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
                    .Replace("{service}", "mission")
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
                    .Replace("{service}", "mission")
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
                    .Replace("{service}", "mission")
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
                    .Replace("{service}", "mission")
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
                    .Replace("{service}", "mission")
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
                    .Replace("{service}", "mission")
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
                    .Replace("{service}", "mission")
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


        public class DescribeCountersTask : Gs2RestSessionTask<DescribeCountersRequest, DescribeCountersResult>
        {
            public DescribeCountersTask(IGs2Session session, RestSessionRequestFactory factory, DescribeCountersRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DescribeCountersRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "mission")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/me/counter";

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
		public IEnumerator DescribeCounters(
                Request.DescribeCountersRequest request,
                UnityAction<AsyncResult<Result.DescribeCountersResult>> callback
        ) =>
            new DescribeCountersTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.DescribeCountersResult> DescribeCountersFuture(
                Request.DescribeCountersRequest request
        ) =>
            new DescribeCountersTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.DescribeCountersResult> DescribeCountersAsync(
                Request.DescribeCountersRequest request
        ) =>
            new DescribeCountersTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.DescribeCountersResult>();
    #else
		public DescribeCountersTask DescribeCountersAsync(
                Request.DescribeCountersRequest request
        )
		{
			return new DescribeCountersTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.DescribeCountersResult> DescribeCountersAsync(
                Request.DescribeCountersRequest request
        ) =>
            new DescribeCountersTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class DescribeCountersByUserIdTask : Gs2RestSessionTask<DescribeCountersByUserIdRequest, DescribeCountersByUserIdResult>
        {
            public DescribeCountersByUserIdTask(IGs2Session session, RestSessionRequestFactory factory, DescribeCountersByUserIdRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DescribeCountersByUserIdRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "mission")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/{userId}/counter";

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
		public IEnumerator DescribeCountersByUserId(
                Request.DescribeCountersByUserIdRequest request,
                UnityAction<AsyncResult<Result.DescribeCountersByUserIdResult>> callback
        ) =>
            new DescribeCountersByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.DescribeCountersByUserIdResult> DescribeCountersByUserIdFuture(
                Request.DescribeCountersByUserIdRequest request
        ) =>
            new DescribeCountersByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.DescribeCountersByUserIdResult> DescribeCountersByUserIdAsync(
                Request.DescribeCountersByUserIdRequest request
        ) =>
            new DescribeCountersByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.DescribeCountersByUserIdResult>();
    #else
		public DescribeCountersByUserIdTask DescribeCountersByUserIdAsync(
                Request.DescribeCountersByUserIdRequest request
        )
		{
			return new DescribeCountersByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.DescribeCountersByUserIdResult> DescribeCountersByUserIdAsync(
                Request.DescribeCountersByUserIdRequest request
        ) =>
            new DescribeCountersByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class IncreaseCounterByUserIdTask : Gs2RestSessionTask<IncreaseCounterByUserIdRequest, IncreaseCounterByUserIdResult>
        {
            public IncreaseCounterByUserIdTask(IGs2Session session, RestSessionRequestFactory factory, IncreaseCounterByUserIdRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(IncreaseCounterByUserIdRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "mission")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/{userId}/counter/{counterName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{counterName}", !string.IsNullOrEmpty(request.CounterName) ? request.CounterName.ToString() : "null");
                url = url.Replace("{userId}", !string.IsNullOrEmpty(request.UserId) ? request.UserId.ToString() : "null");

                var sessionRequest = Factory.Post(url);

                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);
                jsonWriter.WriteObjectStart();
                if (request.Value != null)
                {
                    jsonWriter.WritePropertyName("value");
                    jsonWriter.Write(request.Value.ToString());
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

            public override void OnError(Gs2.Core.Exception.Gs2Exception error)
            {
                if (error.Errors.Count(v => v.code == "counter.increase.conflict") > 0) {
                    base.OnError(new Exception.ConflictException(error));
                }
                else {
                    base.OnError(error);
                }
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator IncreaseCounterByUserId(
                Request.IncreaseCounterByUserIdRequest request,
                UnityAction<AsyncResult<Result.IncreaseCounterByUserIdResult>> callback
        ) =>
            new IncreaseCounterByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.IncreaseCounterByUserIdResult> IncreaseCounterByUserIdFuture(
                Request.IncreaseCounterByUserIdRequest request
        ) =>
            new IncreaseCounterByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.IncreaseCounterByUserIdResult> IncreaseCounterByUserIdAsync(
                Request.IncreaseCounterByUserIdRequest request
        ) =>
            new IncreaseCounterByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.IncreaseCounterByUserIdResult>();
    #else
		public IncreaseCounterByUserIdTask IncreaseCounterByUserIdAsync(
                Request.IncreaseCounterByUserIdRequest request
        )
		{
			return new IncreaseCounterByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.IncreaseCounterByUserIdResult> IncreaseCounterByUserIdAsync(
                Request.IncreaseCounterByUserIdRequest request
        ) =>
            new IncreaseCounterByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class SetCounterByUserIdTask : Gs2RestSessionTask<SetCounterByUserIdRequest, SetCounterByUserIdResult>
        {
            public SetCounterByUserIdTask(IGs2Session session, RestSessionRequestFactory factory, SetCounterByUserIdRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(SetCounterByUserIdRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "mission")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/{userId}/counter/{counterName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{counterName}", !string.IsNullOrEmpty(request.CounterName) ? request.CounterName.ToString() : "null");
                url = url.Replace("{userId}", !string.IsNullOrEmpty(request.UserId) ? request.UserId.ToString() : "null");

                var sessionRequest = Factory.Put(url);

                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);
                jsonWriter.WriteObjectStart();
                if (request.Values != null)
                {
                    jsonWriter.WritePropertyName("values");
                    jsonWriter.WriteArrayStart();
                    foreach(var item in request.Values)
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

            public override void OnError(Gs2.Core.Exception.Gs2Exception error)
            {
                if (error.Errors.Count(v => v.code == "counter.increase.conflict") > 0) {
                    base.OnError(new Exception.ConflictException(error));
                }
                else {
                    base.OnError(error);
                }
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator SetCounterByUserId(
                Request.SetCounterByUserIdRequest request,
                UnityAction<AsyncResult<Result.SetCounterByUserIdResult>> callback
        ) =>
            new SetCounterByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.SetCounterByUserIdResult> SetCounterByUserIdFuture(
                Request.SetCounterByUserIdRequest request
        ) =>
            new SetCounterByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.SetCounterByUserIdResult> SetCounterByUserIdAsync(
                Request.SetCounterByUserIdRequest request
        ) =>
            new SetCounterByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.SetCounterByUserIdResult>();
    #else
		public SetCounterByUserIdTask SetCounterByUserIdAsync(
                Request.SetCounterByUserIdRequest request
        )
		{
			return new SetCounterByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.SetCounterByUserIdResult> SetCounterByUserIdAsync(
                Request.SetCounterByUserIdRequest request
        ) =>
            new SetCounterByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class DecreaseCounterTask : Gs2RestSessionTask<DecreaseCounterRequest, DecreaseCounterResult>
        {
            public DecreaseCounterTask(IGs2Session session, RestSessionRequestFactory factory, DecreaseCounterRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DecreaseCounterRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "mission")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/me/counter/{counterName}/decrease";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{counterName}", !string.IsNullOrEmpty(request.CounterName) ? request.CounterName.ToString() : "null");

                var sessionRequest = Factory.Post(url);

                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);
                jsonWriter.WriteObjectStart();
                if (request.Value != null)
                {
                    jsonWriter.WritePropertyName("value");
                    jsonWriter.Write(request.Value.ToString());
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

            public override void OnError(Gs2.Core.Exception.Gs2Exception error)
            {
                if (error.Errors.Count(v => v.code == "counter.increase.conflict") > 0) {
                    base.OnError(new Exception.ConflictException(error));
                }
                else {
                    base.OnError(error);
                }
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator DecreaseCounter(
                Request.DecreaseCounterRequest request,
                UnityAction<AsyncResult<Result.DecreaseCounterResult>> callback
        ) =>
            new DecreaseCounterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.DecreaseCounterResult> DecreaseCounterFuture(
                Request.DecreaseCounterRequest request
        ) =>
            new DecreaseCounterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.DecreaseCounterResult> DecreaseCounterAsync(
                Request.DecreaseCounterRequest request
        ) =>
            new DecreaseCounterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.DecreaseCounterResult>();
    #else
		public DecreaseCounterTask DecreaseCounterAsync(
                Request.DecreaseCounterRequest request
        )
		{
			return new DecreaseCounterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.DecreaseCounterResult> DecreaseCounterAsync(
                Request.DecreaseCounterRequest request
        ) =>
            new DecreaseCounterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class DecreaseCounterByUserIdTask : Gs2RestSessionTask<DecreaseCounterByUserIdRequest, DecreaseCounterByUserIdResult>
        {
            public DecreaseCounterByUserIdTask(IGs2Session session, RestSessionRequestFactory factory, DecreaseCounterByUserIdRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DecreaseCounterByUserIdRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "mission")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/{userId}/counter/{counterName}/decrease";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{counterName}", !string.IsNullOrEmpty(request.CounterName) ? request.CounterName.ToString() : "null");
                url = url.Replace("{userId}", !string.IsNullOrEmpty(request.UserId) ? request.UserId.ToString() : "null");

                var sessionRequest = Factory.Post(url);

                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);
                jsonWriter.WriteObjectStart();
                if (request.Value != null)
                {
                    jsonWriter.WritePropertyName("value");
                    jsonWriter.Write(request.Value.ToString());
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

            public override void OnError(Gs2.Core.Exception.Gs2Exception error)
            {
                if (error.Errors.Count(v => v.code == "counter.increase.conflict") > 0) {
                    base.OnError(new Exception.ConflictException(error));
                }
                else {
                    base.OnError(error);
                }
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator DecreaseCounterByUserId(
                Request.DecreaseCounterByUserIdRequest request,
                UnityAction<AsyncResult<Result.DecreaseCounterByUserIdResult>> callback
        ) =>
            new DecreaseCounterByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.DecreaseCounterByUserIdResult> DecreaseCounterByUserIdFuture(
                Request.DecreaseCounterByUserIdRequest request
        ) =>
            new DecreaseCounterByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.DecreaseCounterByUserIdResult> DecreaseCounterByUserIdAsync(
                Request.DecreaseCounterByUserIdRequest request
        ) =>
            new DecreaseCounterByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.DecreaseCounterByUserIdResult>();
    #else
		public DecreaseCounterByUserIdTask DecreaseCounterByUserIdAsync(
                Request.DecreaseCounterByUserIdRequest request
        )
		{
			return new DecreaseCounterByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.DecreaseCounterByUserIdResult> DecreaseCounterByUserIdAsync(
                Request.DecreaseCounterByUserIdRequest request
        ) =>
            new DecreaseCounterByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class GetCounterTask : Gs2RestSessionTask<GetCounterRequest, GetCounterResult>
        {
            public GetCounterTask(IGs2Session session, RestSessionRequestFactory factory, GetCounterRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(GetCounterRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "mission")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/me/counter/{counterName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{counterName}", !string.IsNullOrEmpty(request.CounterName) ? request.CounterName.ToString() : "null");

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
		public IEnumerator GetCounter(
                Request.GetCounterRequest request,
                UnityAction<AsyncResult<Result.GetCounterResult>> callback
        ) =>
            new GetCounterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.GetCounterResult> GetCounterFuture(
                Request.GetCounterRequest request
        ) =>
            new GetCounterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.GetCounterResult> GetCounterAsync(
                Request.GetCounterRequest request
        ) =>
            new GetCounterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.GetCounterResult>();
    #else
		public GetCounterTask GetCounterAsync(
                Request.GetCounterRequest request
        )
		{
			return new GetCounterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.GetCounterResult> GetCounterAsync(
                Request.GetCounterRequest request
        ) =>
            new GetCounterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class GetCounterByUserIdTask : Gs2RestSessionTask<GetCounterByUserIdRequest, GetCounterByUserIdResult>
        {
            public GetCounterByUserIdTask(IGs2Session session, RestSessionRequestFactory factory, GetCounterByUserIdRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(GetCounterByUserIdRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "mission")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/{userId}/counter/{counterName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{counterName}", !string.IsNullOrEmpty(request.CounterName) ? request.CounterName.ToString() : "null");
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
		public IEnumerator GetCounterByUserId(
                Request.GetCounterByUserIdRequest request,
                UnityAction<AsyncResult<Result.GetCounterByUserIdResult>> callback
        ) =>
            new GetCounterByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.GetCounterByUserIdResult> GetCounterByUserIdFuture(
                Request.GetCounterByUserIdRequest request
        ) =>
            new GetCounterByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.GetCounterByUserIdResult> GetCounterByUserIdAsync(
                Request.GetCounterByUserIdRequest request
        ) =>
            new GetCounterByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.GetCounterByUserIdResult>();
    #else
		public GetCounterByUserIdTask GetCounterByUserIdAsync(
                Request.GetCounterByUserIdRequest request
        )
		{
			return new GetCounterByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.GetCounterByUserIdResult> GetCounterByUserIdAsync(
                Request.GetCounterByUserIdRequest request
        ) =>
            new GetCounterByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class VerifyCounterValueTask : Gs2RestSessionTask<VerifyCounterValueRequest, VerifyCounterValueResult>
        {
            public VerifyCounterValueTask(IGs2Session session, RestSessionRequestFactory factory, VerifyCounterValueRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(VerifyCounterValueRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "mission")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/me/counter/{counterName}/verify/counter/{verifyType}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{counterName}", !string.IsNullOrEmpty(request.CounterName) ? request.CounterName.ToString() : "null");
                url = url.Replace("{verifyType}", !string.IsNullOrEmpty(request.VerifyType) ? request.VerifyType.ToString() : "null");

                var sessionRequest = Factory.Post(url);

                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);
                jsonWriter.WriteObjectStart();
                if (request.ScopeType != null)
                {
                    jsonWriter.WritePropertyName("scopeType");
                    jsonWriter.Write(request.ScopeType);
                }
                if (request.ResetType != null)
                {
                    jsonWriter.WritePropertyName("resetType");
                    jsonWriter.Write(request.ResetType);
                }
                if (request.ConditionName != null)
                {
                    jsonWriter.WritePropertyName("conditionName");
                    jsonWriter.Write(request.ConditionName);
                }
                if (request.Value != null)
                {
                    jsonWriter.WritePropertyName("value");
                    jsonWriter.Write(request.Value.ToString());
                }
                if (request.MultiplyValueSpecifyingQuantity != null)
                {
                    jsonWriter.WritePropertyName("multiplyValueSpecifyingQuantity");
                    jsonWriter.Write(request.MultiplyValueSpecifyingQuantity.ToString());
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
		public IEnumerator VerifyCounterValue(
                Request.VerifyCounterValueRequest request,
                UnityAction<AsyncResult<Result.VerifyCounterValueResult>> callback
        ) =>
            new VerifyCounterValueTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.VerifyCounterValueResult> VerifyCounterValueFuture(
                Request.VerifyCounterValueRequest request
        ) =>
            new VerifyCounterValueTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.VerifyCounterValueResult> VerifyCounterValueAsync(
                Request.VerifyCounterValueRequest request
        ) =>
            new VerifyCounterValueTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.VerifyCounterValueResult>();
    #else
		public VerifyCounterValueTask VerifyCounterValueAsync(
                Request.VerifyCounterValueRequest request
        )
		{
			return new VerifyCounterValueTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.VerifyCounterValueResult> VerifyCounterValueAsync(
                Request.VerifyCounterValueRequest request
        ) =>
            new VerifyCounterValueTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class VerifyCounterValueByUserIdTask : Gs2RestSessionTask<VerifyCounterValueByUserIdRequest, VerifyCounterValueByUserIdResult>
        {
            public VerifyCounterValueByUserIdTask(IGs2Session session, RestSessionRequestFactory factory, VerifyCounterValueByUserIdRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(VerifyCounterValueByUserIdRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "mission")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/{userId}/counter/{counterName}/verify/counter/{verifyType}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{userId}", !string.IsNullOrEmpty(request.UserId) ? request.UserId.ToString() : "null");
                url = url.Replace("{counterName}", !string.IsNullOrEmpty(request.CounterName) ? request.CounterName.ToString() : "null");
                url = url.Replace("{verifyType}", !string.IsNullOrEmpty(request.VerifyType) ? request.VerifyType.ToString() : "null");

                var sessionRequest = Factory.Post(url);

                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);
                jsonWriter.WriteObjectStart();
                if (request.ScopeType != null)
                {
                    jsonWriter.WritePropertyName("scopeType");
                    jsonWriter.Write(request.ScopeType);
                }
                if (request.ResetType != null)
                {
                    jsonWriter.WritePropertyName("resetType");
                    jsonWriter.Write(request.ResetType);
                }
                if (request.ConditionName != null)
                {
                    jsonWriter.WritePropertyName("conditionName");
                    jsonWriter.Write(request.ConditionName);
                }
                if (request.Value != null)
                {
                    jsonWriter.WritePropertyName("value");
                    jsonWriter.Write(request.Value.ToString());
                }
                if (request.MultiplyValueSpecifyingQuantity != null)
                {
                    jsonWriter.WritePropertyName("multiplyValueSpecifyingQuantity");
                    jsonWriter.Write(request.MultiplyValueSpecifyingQuantity.ToString());
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
		public IEnumerator VerifyCounterValueByUserId(
                Request.VerifyCounterValueByUserIdRequest request,
                UnityAction<AsyncResult<Result.VerifyCounterValueByUserIdResult>> callback
        ) =>
            new VerifyCounterValueByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.VerifyCounterValueByUserIdResult> VerifyCounterValueByUserIdFuture(
                Request.VerifyCounterValueByUserIdRequest request
        ) =>
            new VerifyCounterValueByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.VerifyCounterValueByUserIdResult> VerifyCounterValueByUserIdAsync(
                Request.VerifyCounterValueByUserIdRequest request
        ) =>
            new VerifyCounterValueByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.VerifyCounterValueByUserIdResult>();
    #else
		public VerifyCounterValueByUserIdTask VerifyCounterValueByUserIdAsync(
                Request.VerifyCounterValueByUserIdRequest request
        )
		{
			return new VerifyCounterValueByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.VerifyCounterValueByUserIdResult> VerifyCounterValueByUserIdAsync(
                Request.VerifyCounterValueByUserIdRequest request
        ) =>
            new VerifyCounterValueByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class ResetCounterTask : Gs2RestSessionTask<ResetCounterRequest, ResetCounterResult>
        {
            public ResetCounterTask(IGs2Session session, RestSessionRequestFactory factory, ResetCounterRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(ResetCounterRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "mission")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/me/counter/{counterName}/reset";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{counterName}", !string.IsNullOrEmpty(request.CounterName) ? request.CounterName.ToString() : "null");

                var sessionRequest = Factory.Post(url);

                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);
                jsonWriter.WriteObjectStart();
                if (request.Scopes != null)
                {
                    jsonWriter.WritePropertyName("scopes");
                    jsonWriter.WriteArrayStart();
                    foreach(var item in request.Scopes)
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
		public IEnumerator ResetCounter(
                Request.ResetCounterRequest request,
                UnityAction<AsyncResult<Result.ResetCounterResult>> callback
        ) =>
            new ResetCounterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.ResetCounterResult> ResetCounterFuture(
                Request.ResetCounterRequest request
        ) =>
            new ResetCounterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.ResetCounterResult> ResetCounterAsync(
                Request.ResetCounterRequest request
        ) =>
            new ResetCounterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.ResetCounterResult>();
    #else
		public ResetCounterTask ResetCounterAsync(
                Request.ResetCounterRequest request
        )
		{
			return new ResetCounterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.ResetCounterResult> ResetCounterAsync(
                Request.ResetCounterRequest request
        ) =>
            new ResetCounterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class ResetCounterByUserIdTask : Gs2RestSessionTask<ResetCounterByUserIdRequest, ResetCounterByUserIdResult>
        {
            public ResetCounterByUserIdTask(IGs2Session session, RestSessionRequestFactory factory, ResetCounterByUserIdRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(ResetCounterByUserIdRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "mission")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/{userId}/counter/{counterName}/reset";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{userId}", !string.IsNullOrEmpty(request.UserId) ? request.UserId.ToString() : "null");
                url = url.Replace("{counterName}", !string.IsNullOrEmpty(request.CounterName) ? request.CounterName.ToString() : "null");

                var sessionRequest = Factory.Post(url);

                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);
                jsonWriter.WriteObjectStart();
                if (request.Scopes != null)
                {
                    jsonWriter.WritePropertyName("scopes");
                    jsonWriter.WriteArrayStart();
                    foreach(var item in request.Scopes)
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
		public IEnumerator ResetCounterByUserId(
                Request.ResetCounterByUserIdRequest request,
                UnityAction<AsyncResult<Result.ResetCounterByUserIdResult>> callback
        ) =>
            new ResetCounterByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.ResetCounterByUserIdResult> ResetCounterByUserIdFuture(
                Request.ResetCounterByUserIdRequest request
        ) =>
            new ResetCounterByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.ResetCounterByUserIdResult> ResetCounterByUserIdAsync(
                Request.ResetCounterByUserIdRequest request
        ) =>
            new ResetCounterByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.ResetCounterByUserIdResult>();
    #else
		public ResetCounterByUserIdTask ResetCounterByUserIdAsync(
                Request.ResetCounterByUserIdRequest request
        )
		{
			return new ResetCounterByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.ResetCounterByUserIdResult> ResetCounterByUserIdAsync(
                Request.ResetCounterByUserIdRequest request
        ) =>
            new ResetCounterByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class DeleteCounterTask : Gs2RestSessionTask<DeleteCounterRequest, DeleteCounterResult>
        {
            public DeleteCounterTask(IGs2Session session, RestSessionRequestFactory factory, DeleteCounterRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DeleteCounterRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "mission")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/me/counter/{counterName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{counterName}", !string.IsNullOrEmpty(request.CounterName) ? request.CounterName.ToString() : "null");

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
		public IEnumerator DeleteCounter(
                Request.DeleteCounterRequest request,
                UnityAction<AsyncResult<Result.DeleteCounterResult>> callback
        ) =>
            new DeleteCounterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.DeleteCounterResult> DeleteCounterFuture(
                Request.DeleteCounterRequest request
        ) =>
            new DeleteCounterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.DeleteCounterResult> DeleteCounterAsync(
                Request.DeleteCounterRequest request
        ) =>
            new DeleteCounterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.DeleteCounterResult>();
    #else
		public DeleteCounterTask DeleteCounterAsync(
                Request.DeleteCounterRequest request
        )
		{
			return new DeleteCounterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.DeleteCounterResult> DeleteCounterAsync(
                Request.DeleteCounterRequest request
        ) =>
            new DeleteCounterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class DeleteCounterByUserIdTask : Gs2RestSessionTask<DeleteCounterByUserIdRequest, DeleteCounterByUserIdResult>
        {
            public DeleteCounterByUserIdTask(IGs2Session session, RestSessionRequestFactory factory, DeleteCounterByUserIdRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DeleteCounterByUserIdRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "mission")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/{userId}/counter/{counterName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{userId}", !string.IsNullOrEmpty(request.UserId) ? request.UserId.ToString() : "null");
                url = url.Replace("{counterName}", !string.IsNullOrEmpty(request.CounterName) ? request.CounterName.ToString() : "null");

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
		public IEnumerator DeleteCounterByUserId(
                Request.DeleteCounterByUserIdRequest request,
                UnityAction<AsyncResult<Result.DeleteCounterByUserIdResult>> callback
        ) =>
            new DeleteCounterByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.DeleteCounterByUserIdResult> DeleteCounterByUserIdFuture(
                Request.DeleteCounterByUserIdRequest request
        ) =>
            new DeleteCounterByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.DeleteCounterByUserIdResult> DeleteCounterByUserIdAsync(
                Request.DeleteCounterByUserIdRequest request
        ) =>
            new DeleteCounterByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.DeleteCounterByUserIdResult>();
    #else
		public DeleteCounterByUserIdTask DeleteCounterByUserIdAsync(
                Request.DeleteCounterByUserIdRequest request
        )
		{
			return new DeleteCounterByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.DeleteCounterByUserIdResult> DeleteCounterByUserIdAsync(
                Request.DeleteCounterByUserIdRequest request
        ) =>
            new DeleteCounterByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class IncreaseByStampSheetTask : Gs2RestSessionTask<IncreaseByStampSheetRequest, IncreaseByStampSheetResult>
        {
            public IncreaseByStampSheetTask(IGs2Session session, RestSessionRequestFactory factory, IncreaseByStampSheetRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(IncreaseByStampSheetRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "mission")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/stamp/increase";

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
		public IEnumerator IncreaseByStampSheet(
                Request.IncreaseByStampSheetRequest request,
                UnityAction<AsyncResult<Result.IncreaseByStampSheetResult>> callback
        ) =>
            new IncreaseByStampSheetTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.IncreaseByStampSheetResult> IncreaseByStampSheetFuture(
                Request.IncreaseByStampSheetRequest request
        ) =>
            new IncreaseByStampSheetTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.IncreaseByStampSheetResult> IncreaseByStampSheetAsync(
                Request.IncreaseByStampSheetRequest request
        ) =>
            new IncreaseByStampSheetTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.IncreaseByStampSheetResult>();
    #else
		public IncreaseByStampSheetTask IncreaseByStampSheetAsync(
                Request.IncreaseByStampSheetRequest request
        )
		{
			return new IncreaseByStampSheetTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.IncreaseByStampSheetResult> IncreaseByStampSheetAsync(
                Request.IncreaseByStampSheetRequest request
        ) =>
            new IncreaseByStampSheetTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class SetByStampSheetTask : Gs2RestSessionTask<SetByStampSheetRequest, SetByStampSheetResult>
        {
            public SetByStampSheetTask(IGs2Session session, RestSessionRequestFactory factory, SetByStampSheetRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(SetByStampSheetRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "mission")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/stamp/set";

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
		public IEnumerator SetByStampSheet(
                Request.SetByStampSheetRequest request,
                UnityAction<AsyncResult<Result.SetByStampSheetResult>> callback
        ) =>
            new SetByStampSheetTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.SetByStampSheetResult> SetByStampSheetFuture(
                Request.SetByStampSheetRequest request
        ) =>
            new SetByStampSheetTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.SetByStampSheetResult> SetByStampSheetAsync(
                Request.SetByStampSheetRequest request
        ) =>
            new SetByStampSheetTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.SetByStampSheetResult>();
    #else
		public SetByStampSheetTask SetByStampSheetAsync(
                Request.SetByStampSheetRequest request
        )
		{
			return new SetByStampSheetTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.SetByStampSheetResult> SetByStampSheetAsync(
                Request.SetByStampSheetRequest request
        ) =>
            new SetByStampSheetTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class DecreaseByStampTaskTask : Gs2RestSessionTask<DecreaseByStampTaskRequest, DecreaseByStampTaskResult>
        {
            public DecreaseByStampTaskTask(IGs2Session session, RestSessionRequestFactory factory, DecreaseByStampTaskRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DecreaseByStampTaskRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "mission")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/stamp/decrease";

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
		public IEnumerator DecreaseByStampTask(
                Request.DecreaseByStampTaskRequest request,
                UnityAction<AsyncResult<Result.DecreaseByStampTaskResult>> callback
        ) =>
            new DecreaseByStampTaskTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.DecreaseByStampTaskResult> DecreaseByStampTaskFuture(
                Request.DecreaseByStampTaskRequest request
        ) =>
            new DecreaseByStampTaskTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.DecreaseByStampTaskResult> DecreaseByStampTaskAsync(
                Request.DecreaseByStampTaskRequest request
        ) =>
            new DecreaseByStampTaskTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.DecreaseByStampTaskResult>();
    #else
		public DecreaseByStampTaskTask DecreaseByStampTaskAsync(
                Request.DecreaseByStampTaskRequest request
        )
		{
			return new DecreaseByStampTaskTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.DecreaseByStampTaskResult> DecreaseByStampTaskAsync(
                Request.DecreaseByStampTaskRequest request
        ) =>
            new DecreaseByStampTaskTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class ResetByStampTaskTask : Gs2RestSessionTask<ResetByStampTaskRequest, ResetByStampTaskResult>
        {
            public ResetByStampTaskTask(IGs2Session session, RestSessionRequestFactory factory, ResetByStampTaskRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(ResetByStampTaskRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "mission")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/stamp/reset";

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
		public IEnumerator ResetByStampTask(
                Request.ResetByStampTaskRequest request,
                UnityAction<AsyncResult<Result.ResetByStampTaskResult>> callback
        ) =>
            new ResetByStampTaskTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.ResetByStampTaskResult> ResetByStampTaskFuture(
                Request.ResetByStampTaskRequest request
        ) =>
            new ResetByStampTaskTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.ResetByStampTaskResult> ResetByStampTaskAsync(
                Request.ResetByStampTaskRequest request
        ) =>
            new ResetByStampTaskTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.ResetByStampTaskResult>();
    #else
		public ResetByStampTaskTask ResetByStampTaskAsync(
                Request.ResetByStampTaskRequest request
        )
		{
			return new ResetByStampTaskTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.ResetByStampTaskResult> ResetByStampTaskAsync(
                Request.ResetByStampTaskRequest request
        ) =>
            new ResetByStampTaskTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class VerifyCounterValueByStampTaskTask : Gs2RestSessionTask<VerifyCounterValueByStampTaskRequest, VerifyCounterValueByStampTaskResult>
        {
            public VerifyCounterValueByStampTaskTask(IGs2Session session, RestSessionRequestFactory factory, VerifyCounterValueByStampTaskRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(VerifyCounterValueByStampTaskRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "mission")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/stamp/counter/verify";

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
		public IEnumerator VerifyCounterValueByStampTask(
                Request.VerifyCounterValueByStampTaskRequest request,
                UnityAction<AsyncResult<Result.VerifyCounterValueByStampTaskResult>> callback
        ) =>
            new VerifyCounterValueByStampTaskTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.VerifyCounterValueByStampTaskResult> VerifyCounterValueByStampTaskFuture(
                Request.VerifyCounterValueByStampTaskRequest request
        ) =>
            new VerifyCounterValueByStampTaskTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.VerifyCounterValueByStampTaskResult> VerifyCounterValueByStampTaskAsync(
                Request.VerifyCounterValueByStampTaskRequest request
        ) =>
            new VerifyCounterValueByStampTaskTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.VerifyCounterValueByStampTaskResult>();
    #else
		public VerifyCounterValueByStampTaskTask VerifyCounterValueByStampTaskAsync(
                Request.VerifyCounterValueByStampTaskRequest request
        )
		{
			return new VerifyCounterValueByStampTaskTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.VerifyCounterValueByStampTaskResult> VerifyCounterValueByStampTaskAsync(
                Request.VerifyCounterValueByStampTaskRequest request
        ) =>
            new VerifyCounterValueByStampTaskTask(
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
                    .Replace("{service}", "mission")
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


        public class GetCurrentMissionMasterTask : Gs2RestSessionTask<GetCurrentMissionMasterRequest, GetCurrentMissionMasterResult>
        {
            public GetCurrentMissionMasterTask(IGs2Session session, RestSessionRequestFactory factory, GetCurrentMissionMasterRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(GetCurrentMissionMasterRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "mission")
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
		public IEnumerator GetCurrentMissionMaster(
                Request.GetCurrentMissionMasterRequest request,
                UnityAction<AsyncResult<Result.GetCurrentMissionMasterResult>> callback
        ) =>
            new GetCurrentMissionMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.GetCurrentMissionMasterResult> GetCurrentMissionMasterFuture(
                Request.GetCurrentMissionMasterRequest request
        ) =>
            new GetCurrentMissionMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.GetCurrentMissionMasterResult> GetCurrentMissionMasterAsync(
                Request.GetCurrentMissionMasterRequest request
        ) =>
            new GetCurrentMissionMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.GetCurrentMissionMasterResult>();
    #else
		public GetCurrentMissionMasterTask GetCurrentMissionMasterAsync(
                Request.GetCurrentMissionMasterRequest request
        )
		{
			return new GetCurrentMissionMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.GetCurrentMissionMasterResult> GetCurrentMissionMasterAsync(
                Request.GetCurrentMissionMasterRequest request
        ) =>
            new GetCurrentMissionMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class PreUpdateCurrentMissionMasterTask : Gs2RestSessionTask<PreUpdateCurrentMissionMasterRequest, PreUpdateCurrentMissionMasterResult>
        {
            public PreUpdateCurrentMissionMasterTask(IGs2Session session, RestSessionRequestFactory factory, PreUpdateCurrentMissionMasterRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(PreUpdateCurrentMissionMasterRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "mission")
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
		public IEnumerator PreUpdateCurrentMissionMaster(
                Request.PreUpdateCurrentMissionMasterRequest request,
                UnityAction<AsyncResult<Result.PreUpdateCurrentMissionMasterResult>> callback
        ) =>
            new PreUpdateCurrentMissionMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.PreUpdateCurrentMissionMasterResult> PreUpdateCurrentMissionMasterFuture(
                Request.PreUpdateCurrentMissionMasterRequest request
        ) =>
            new PreUpdateCurrentMissionMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.PreUpdateCurrentMissionMasterResult> PreUpdateCurrentMissionMasterAsync(
                Request.PreUpdateCurrentMissionMasterRequest request
        ) =>
            new PreUpdateCurrentMissionMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.PreUpdateCurrentMissionMasterResult>();
    #else
		public PreUpdateCurrentMissionMasterTask PreUpdateCurrentMissionMasterAsync(
                Request.PreUpdateCurrentMissionMasterRequest request
        )
		{
			return new PreUpdateCurrentMissionMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.PreUpdateCurrentMissionMasterResult> PreUpdateCurrentMissionMasterAsync(
                Request.PreUpdateCurrentMissionMasterRequest request
        ) =>
            new PreUpdateCurrentMissionMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class UpdateCurrentMissionMasterTask : Gs2RestSessionTask<UpdateCurrentMissionMasterRequest, UpdateCurrentMissionMasterResult>
        {
            public UpdateCurrentMissionMasterTask(IGs2Session session, RestSessionRequestFactory factory, UpdateCurrentMissionMasterRequest request) : base(session, factory, request)
            {
            }
#if GS2_ENABLE_UNITASK
            protected override async UniTask<UpdateCurrentMissionMasterResult> InvokeImpl()
#else
            protected override async Task<UpdateCurrentMissionMasterResult> InvokeImpl()
#endif
            {
                if (Request.Settings != null) {
                    var preTask = new PreUpdateCurrentMissionMasterTask(
                        Session,
                        Factory,
                        new PreUpdateCurrentMissionMasterRequest()
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

            protected override IGs2SessionRequest CreateRequest(UpdateCurrentMissionMasterRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "mission")
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
		public IEnumerator UpdateCurrentMissionMaster(
                Request.UpdateCurrentMissionMasterRequest request,
                UnityAction<AsyncResult<Result.UpdateCurrentMissionMasterResult>> callback
        ) =>
            new UpdateCurrentMissionMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.UpdateCurrentMissionMasterResult> UpdateCurrentMissionMasterFuture(
                Request.UpdateCurrentMissionMasterRequest request
        ) =>
            new UpdateCurrentMissionMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.UpdateCurrentMissionMasterResult> UpdateCurrentMissionMasterAsync(
                Request.UpdateCurrentMissionMasterRequest request
        ) =>
            new UpdateCurrentMissionMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.UpdateCurrentMissionMasterResult>();
    #else
		public UpdateCurrentMissionMasterTask UpdateCurrentMissionMasterAsync(
                Request.UpdateCurrentMissionMasterRequest request
        )
		{
			return new UpdateCurrentMissionMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.UpdateCurrentMissionMasterResult> UpdateCurrentMissionMasterAsync(
                Request.UpdateCurrentMissionMasterRequest request
        ) =>
            new UpdateCurrentMissionMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class UpdateCurrentMissionMasterFromGitHubTask : Gs2RestSessionTask<UpdateCurrentMissionMasterFromGitHubRequest, UpdateCurrentMissionMasterFromGitHubResult>
        {
            public UpdateCurrentMissionMasterFromGitHubTask(IGs2Session session, RestSessionRequestFactory factory, UpdateCurrentMissionMasterFromGitHubRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(UpdateCurrentMissionMasterFromGitHubRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "mission")
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
		public IEnumerator UpdateCurrentMissionMasterFromGitHub(
                Request.UpdateCurrentMissionMasterFromGitHubRequest request,
                UnityAction<AsyncResult<Result.UpdateCurrentMissionMasterFromGitHubResult>> callback
        ) =>
            new UpdateCurrentMissionMasterFromGitHubTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.UpdateCurrentMissionMasterFromGitHubResult> UpdateCurrentMissionMasterFromGitHubFuture(
                Request.UpdateCurrentMissionMasterFromGitHubRequest request
        ) =>
            new UpdateCurrentMissionMasterFromGitHubTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.UpdateCurrentMissionMasterFromGitHubResult> UpdateCurrentMissionMasterFromGitHubAsync(
                Request.UpdateCurrentMissionMasterFromGitHubRequest request
        ) =>
            new UpdateCurrentMissionMasterFromGitHubTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.UpdateCurrentMissionMasterFromGitHubResult>();
    #else
		public UpdateCurrentMissionMasterFromGitHubTask UpdateCurrentMissionMasterFromGitHubAsync(
                Request.UpdateCurrentMissionMasterFromGitHubRequest request
        )
		{
			return new UpdateCurrentMissionMasterFromGitHubTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.UpdateCurrentMissionMasterFromGitHubResult> UpdateCurrentMissionMasterFromGitHubAsync(
                Request.UpdateCurrentMissionMasterFromGitHubRequest request
        ) =>
            new UpdateCurrentMissionMasterFromGitHubTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class DescribeCounterModelsTask : Gs2RestSessionTask<DescribeCounterModelsRequest, DescribeCounterModelsResult>
        {
            public DescribeCounterModelsTask(IGs2Session session, RestSessionRequestFactory factory, DescribeCounterModelsRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DescribeCounterModelsRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "mission")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/counter";

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
		public IEnumerator DescribeCounterModels(
                Request.DescribeCounterModelsRequest request,
                UnityAction<AsyncResult<Result.DescribeCounterModelsResult>> callback
        ) =>
            new DescribeCounterModelsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.DescribeCounterModelsResult> DescribeCounterModelsFuture(
                Request.DescribeCounterModelsRequest request
        ) =>
            new DescribeCounterModelsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.DescribeCounterModelsResult> DescribeCounterModelsAsync(
                Request.DescribeCounterModelsRequest request
        ) =>
            new DescribeCounterModelsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.DescribeCounterModelsResult>();
    #else
		public DescribeCounterModelsTask DescribeCounterModelsAsync(
                Request.DescribeCounterModelsRequest request
        )
		{
			return new DescribeCounterModelsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.DescribeCounterModelsResult> DescribeCounterModelsAsync(
                Request.DescribeCounterModelsRequest request
        ) =>
            new DescribeCounterModelsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class GetCounterModelTask : Gs2RestSessionTask<GetCounterModelRequest, GetCounterModelResult>
        {
            public GetCounterModelTask(IGs2Session session, RestSessionRequestFactory factory, GetCounterModelRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(GetCounterModelRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "mission")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/counter/{counterName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{counterName}", !string.IsNullOrEmpty(request.CounterName) ? request.CounterName.ToString() : "null");

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
		public IEnumerator GetCounterModel(
                Request.GetCounterModelRequest request,
                UnityAction<AsyncResult<Result.GetCounterModelResult>> callback
        ) =>
            new GetCounterModelTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.GetCounterModelResult> GetCounterModelFuture(
                Request.GetCounterModelRequest request
        ) =>
            new GetCounterModelTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.GetCounterModelResult> GetCounterModelAsync(
                Request.GetCounterModelRequest request
        ) =>
            new GetCounterModelTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.GetCounterModelResult>();
    #else
		public GetCounterModelTask GetCounterModelAsync(
                Request.GetCounterModelRequest request
        )
		{
			return new GetCounterModelTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.GetCounterModelResult> GetCounterModelAsync(
                Request.GetCounterModelRequest request
        ) =>
            new GetCounterModelTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class DescribeMissionGroupModelsTask : Gs2RestSessionTask<DescribeMissionGroupModelsRequest, DescribeMissionGroupModelsResult>
        {
            public DescribeMissionGroupModelsTask(IGs2Session session, RestSessionRequestFactory factory, DescribeMissionGroupModelsRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DescribeMissionGroupModelsRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "mission")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/group";

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
		public IEnumerator DescribeMissionGroupModels(
                Request.DescribeMissionGroupModelsRequest request,
                UnityAction<AsyncResult<Result.DescribeMissionGroupModelsResult>> callback
        ) =>
            new DescribeMissionGroupModelsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.DescribeMissionGroupModelsResult> DescribeMissionGroupModelsFuture(
                Request.DescribeMissionGroupModelsRequest request
        ) =>
            new DescribeMissionGroupModelsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.DescribeMissionGroupModelsResult> DescribeMissionGroupModelsAsync(
                Request.DescribeMissionGroupModelsRequest request
        ) =>
            new DescribeMissionGroupModelsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.DescribeMissionGroupModelsResult>();
    #else
		public DescribeMissionGroupModelsTask DescribeMissionGroupModelsAsync(
                Request.DescribeMissionGroupModelsRequest request
        )
		{
			return new DescribeMissionGroupModelsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.DescribeMissionGroupModelsResult> DescribeMissionGroupModelsAsync(
                Request.DescribeMissionGroupModelsRequest request
        ) =>
            new DescribeMissionGroupModelsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class GetMissionGroupModelTask : Gs2RestSessionTask<GetMissionGroupModelRequest, GetMissionGroupModelResult>
        {
            public GetMissionGroupModelTask(IGs2Session session, RestSessionRequestFactory factory, GetMissionGroupModelRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(GetMissionGroupModelRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "mission")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/group/{missionGroupName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{missionGroupName}", !string.IsNullOrEmpty(request.MissionGroupName) ? request.MissionGroupName.ToString() : "null");

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
		public IEnumerator GetMissionGroupModel(
                Request.GetMissionGroupModelRequest request,
                UnityAction<AsyncResult<Result.GetMissionGroupModelResult>> callback
        ) =>
            new GetMissionGroupModelTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.GetMissionGroupModelResult> GetMissionGroupModelFuture(
                Request.GetMissionGroupModelRequest request
        ) =>
            new GetMissionGroupModelTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.GetMissionGroupModelResult> GetMissionGroupModelAsync(
                Request.GetMissionGroupModelRequest request
        ) =>
            new GetMissionGroupModelTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.GetMissionGroupModelResult>();
    #else
		public GetMissionGroupModelTask GetMissionGroupModelAsync(
                Request.GetMissionGroupModelRequest request
        )
		{
			return new GetMissionGroupModelTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.GetMissionGroupModelResult> GetMissionGroupModelAsync(
                Request.GetMissionGroupModelRequest request
        ) =>
            new GetMissionGroupModelTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class DescribeMissionTaskModelsTask : Gs2RestSessionTask<DescribeMissionTaskModelsRequest, DescribeMissionTaskModelsResult>
        {
            public DescribeMissionTaskModelsTask(IGs2Session session, RestSessionRequestFactory factory, DescribeMissionTaskModelsRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DescribeMissionTaskModelsRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "mission")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/group/{missionGroupName}/task";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{missionGroupName}", !string.IsNullOrEmpty(request.MissionGroupName) ? request.MissionGroupName.ToString() : "null");

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
		public IEnumerator DescribeMissionTaskModels(
                Request.DescribeMissionTaskModelsRequest request,
                UnityAction<AsyncResult<Result.DescribeMissionTaskModelsResult>> callback
        ) =>
            new DescribeMissionTaskModelsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.DescribeMissionTaskModelsResult> DescribeMissionTaskModelsFuture(
                Request.DescribeMissionTaskModelsRequest request
        ) =>
            new DescribeMissionTaskModelsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.DescribeMissionTaskModelsResult> DescribeMissionTaskModelsAsync(
                Request.DescribeMissionTaskModelsRequest request
        ) =>
            new DescribeMissionTaskModelsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.DescribeMissionTaskModelsResult>();
    #else
		public DescribeMissionTaskModelsTask DescribeMissionTaskModelsAsync(
                Request.DescribeMissionTaskModelsRequest request
        )
		{
			return new DescribeMissionTaskModelsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.DescribeMissionTaskModelsResult> DescribeMissionTaskModelsAsync(
                Request.DescribeMissionTaskModelsRequest request
        ) =>
            new DescribeMissionTaskModelsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class GetMissionTaskModelTask : Gs2RestSessionTask<GetMissionTaskModelRequest, GetMissionTaskModelResult>
        {
            public GetMissionTaskModelTask(IGs2Session session, RestSessionRequestFactory factory, GetMissionTaskModelRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(GetMissionTaskModelRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "mission")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/group/{missionGroupName}/task/{missionTaskName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{missionGroupName}", !string.IsNullOrEmpty(request.MissionGroupName) ? request.MissionGroupName.ToString() : "null");
                url = url.Replace("{missionTaskName}", !string.IsNullOrEmpty(request.MissionTaskName) ? request.MissionTaskName.ToString() : "null");

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
		public IEnumerator GetMissionTaskModel(
                Request.GetMissionTaskModelRequest request,
                UnityAction<AsyncResult<Result.GetMissionTaskModelResult>> callback
        ) =>
            new GetMissionTaskModelTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.GetMissionTaskModelResult> GetMissionTaskModelFuture(
                Request.GetMissionTaskModelRequest request
        ) =>
            new GetMissionTaskModelTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.GetMissionTaskModelResult> GetMissionTaskModelAsync(
                Request.GetMissionTaskModelRequest request
        ) =>
            new GetMissionTaskModelTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.GetMissionTaskModelResult>();
    #else
		public GetMissionTaskModelTask GetMissionTaskModelAsync(
                Request.GetMissionTaskModelRequest request
        )
		{
			return new GetMissionTaskModelTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.GetMissionTaskModelResult> GetMissionTaskModelAsync(
                Request.GetMissionTaskModelRequest request
        ) =>
            new GetMissionTaskModelTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class DescribeMissionTaskModelMastersTask : Gs2RestSessionTask<DescribeMissionTaskModelMastersRequest, DescribeMissionTaskModelMastersResult>
        {
            public DescribeMissionTaskModelMastersTask(IGs2Session session, RestSessionRequestFactory factory, DescribeMissionTaskModelMastersRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DescribeMissionTaskModelMastersRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "mission")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/master/group/{missionGroupName}/task";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{missionGroupName}", !string.IsNullOrEmpty(request.MissionGroupName) ? request.MissionGroupName.ToString() : "null");

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
		public IEnumerator DescribeMissionTaskModelMasters(
                Request.DescribeMissionTaskModelMastersRequest request,
                UnityAction<AsyncResult<Result.DescribeMissionTaskModelMastersResult>> callback
        ) =>
            new DescribeMissionTaskModelMastersTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.DescribeMissionTaskModelMastersResult> DescribeMissionTaskModelMastersFuture(
                Request.DescribeMissionTaskModelMastersRequest request
        ) =>
            new DescribeMissionTaskModelMastersTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.DescribeMissionTaskModelMastersResult> DescribeMissionTaskModelMastersAsync(
                Request.DescribeMissionTaskModelMastersRequest request
        ) =>
            new DescribeMissionTaskModelMastersTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.DescribeMissionTaskModelMastersResult>();
    #else
		public DescribeMissionTaskModelMastersTask DescribeMissionTaskModelMastersAsync(
                Request.DescribeMissionTaskModelMastersRequest request
        )
		{
			return new DescribeMissionTaskModelMastersTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.DescribeMissionTaskModelMastersResult> DescribeMissionTaskModelMastersAsync(
                Request.DescribeMissionTaskModelMastersRequest request
        ) =>
            new DescribeMissionTaskModelMastersTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class CreateMissionTaskModelMasterTask : Gs2RestSessionTask<CreateMissionTaskModelMasterRequest, CreateMissionTaskModelMasterResult>
        {
            public CreateMissionTaskModelMasterTask(IGs2Session session, RestSessionRequestFactory factory, CreateMissionTaskModelMasterRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(CreateMissionTaskModelMasterRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "mission")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/master/group/{missionGroupName}/task";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{missionGroupName}", !string.IsNullOrEmpty(request.MissionGroupName) ? request.MissionGroupName.ToString() : "null");

                var sessionRequest = Factory.Post(url);

                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);
                jsonWriter.WriteObjectStart();
                if (request.Name != null)
                {
                    jsonWriter.WritePropertyName("name");
                    jsonWriter.Write(request.Name);
                }
                if (request.Metadata != null)
                {
                    jsonWriter.WritePropertyName("metadata");
                    jsonWriter.Write(request.Metadata);
                }
                if (request.Description != null)
                {
                    jsonWriter.WritePropertyName("description");
                    jsonWriter.Write(request.Description);
                }
                if (request.VerifyCompleteType != null)
                {
                    jsonWriter.WritePropertyName("verifyCompleteType");
                    jsonWriter.Write(request.VerifyCompleteType);
                }
                if (request.TargetCounter != null)
                {
                    jsonWriter.WritePropertyName("targetCounter");
                    request.TargetCounter.WriteJson(jsonWriter);
                }
                if (request.VerifyCompleteConsumeActions != null)
                {
                    jsonWriter.WritePropertyName("verifyCompleteConsumeActions");
                    jsonWriter.WriteArrayStart();
                    foreach(var item in request.VerifyCompleteConsumeActions)
                    {
                        item.WriteJson(jsonWriter);
                    }
                    jsonWriter.WriteArrayEnd();
                }
                if (request.CompleteAcquireActions != null)
                {
                    jsonWriter.WritePropertyName("completeAcquireActions");
                    jsonWriter.WriteArrayStart();
                    foreach(var item in request.CompleteAcquireActions)
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
                if (request.PremiseMissionTaskName != null)
                {
                    jsonWriter.WritePropertyName("premiseMissionTaskName");
                    jsonWriter.Write(request.PremiseMissionTaskName);
                }
                if (request.CounterName != null)
                {
                    jsonWriter.WritePropertyName("counterName");
                    jsonWriter.Write(request.CounterName);
                }
                if (request.TargetResetType != null)
                {
                    jsonWriter.WritePropertyName("targetResetType");
                    jsonWriter.Write(request.TargetResetType);
                }
                if (request.TargetValue != null)
                {
                    jsonWriter.WritePropertyName("targetValue");
                    jsonWriter.Write(request.TargetValue.ToString());
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
		public IEnumerator CreateMissionTaskModelMaster(
                Request.CreateMissionTaskModelMasterRequest request,
                UnityAction<AsyncResult<Result.CreateMissionTaskModelMasterResult>> callback
        ) =>
            new CreateMissionTaskModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.CreateMissionTaskModelMasterResult> CreateMissionTaskModelMasterFuture(
                Request.CreateMissionTaskModelMasterRequest request
        ) =>
            new CreateMissionTaskModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.CreateMissionTaskModelMasterResult> CreateMissionTaskModelMasterAsync(
                Request.CreateMissionTaskModelMasterRequest request
        ) =>
            new CreateMissionTaskModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.CreateMissionTaskModelMasterResult>();
    #else
		public CreateMissionTaskModelMasterTask CreateMissionTaskModelMasterAsync(
                Request.CreateMissionTaskModelMasterRequest request
        )
		{
			return new CreateMissionTaskModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.CreateMissionTaskModelMasterResult> CreateMissionTaskModelMasterAsync(
                Request.CreateMissionTaskModelMasterRequest request
        ) =>
            new CreateMissionTaskModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class GetMissionTaskModelMasterTask : Gs2RestSessionTask<GetMissionTaskModelMasterRequest, GetMissionTaskModelMasterResult>
        {
            public GetMissionTaskModelMasterTask(IGs2Session session, RestSessionRequestFactory factory, GetMissionTaskModelMasterRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(GetMissionTaskModelMasterRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "mission")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/master/group/{missionGroupName}/task/{missionTaskName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{missionGroupName}", !string.IsNullOrEmpty(request.MissionGroupName) ? request.MissionGroupName.ToString() : "null");
                url = url.Replace("{missionTaskName}", !string.IsNullOrEmpty(request.MissionTaskName) ? request.MissionTaskName.ToString() : "null");

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
		public IEnumerator GetMissionTaskModelMaster(
                Request.GetMissionTaskModelMasterRequest request,
                UnityAction<AsyncResult<Result.GetMissionTaskModelMasterResult>> callback
        ) =>
            new GetMissionTaskModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.GetMissionTaskModelMasterResult> GetMissionTaskModelMasterFuture(
                Request.GetMissionTaskModelMasterRequest request
        ) =>
            new GetMissionTaskModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.GetMissionTaskModelMasterResult> GetMissionTaskModelMasterAsync(
                Request.GetMissionTaskModelMasterRequest request
        ) =>
            new GetMissionTaskModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.GetMissionTaskModelMasterResult>();
    #else
		public GetMissionTaskModelMasterTask GetMissionTaskModelMasterAsync(
                Request.GetMissionTaskModelMasterRequest request
        )
		{
			return new GetMissionTaskModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.GetMissionTaskModelMasterResult> GetMissionTaskModelMasterAsync(
                Request.GetMissionTaskModelMasterRequest request
        ) =>
            new GetMissionTaskModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class UpdateMissionTaskModelMasterTask : Gs2RestSessionTask<UpdateMissionTaskModelMasterRequest, UpdateMissionTaskModelMasterResult>
        {
            public UpdateMissionTaskModelMasterTask(IGs2Session session, RestSessionRequestFactory factory, UpdateMissionTaskModelMasterRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(UpdateMissionTaskModelMasterRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "mission")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/master/group/{missionGroupName}/task/{missionTaskName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{missionGroupName}", !string.IsNullOrEmpty(request.MissionGroupName) ? request.MissionGroupName.ToString() : "null");
                url = url.Replace("{missionTaskName}", !string.IsNullOrEmpty(request.MissionTaskName) ? request.MissionTaskName.ToString() : "null");

                var sessionRequest = Factory.Put(url);

                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);
                jsonWriter.WriteObjectStart();
                if (request.Metadata != null)
                {
                    jsonWriter.WritePropertyName("metadata");
                    jsonWriter.Write(request.Metadata);
                }
                if (request.Description != null)
                {
                    jsonWriter.WritePropertyName("description");
                    jsonWriter.Write(request.Description);
                }
                if (request.VerifyCompleteType != null)
                {
                    jsonWriter.WritePropertyName("verifyCompleteType");
                    jsonWriter.Write(request.VerifyCompleteType);
                }
                if (request.TargetCounter != null)
                {
                    jsonWriter.WritePropertyName("targetCounter");
                    request.TargetCounter.WriteJson(jsonWriter);
                }
                if (request.VerifyCompleteConsumeActions != null)
                {
                    jsonWriter.WritePropertyName("verifyCompleteConsumeActions");
                    jsonWriter.WriteArrayStart();
                    foreach(var item in request.VerifyCompleteConsumeActions)
                    {
                        item.WriteJson(jsonWriter);
                    }
                    jsonWriter.WriteArrayEnd();
                }
                if (request.CompleteAcquireActions != null)
                {
                    jsonWriter.WritePropertyName("completeAcquireActions");
                    jsonWriter.WriteArrayStart();
                    foreach(var item in request.CompleteAcquireActions)
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
                if (request.PremiseMissionTaskName != null)
                {
                    jsonWriter.WritePropertyName("premiseMissionTaskName");
                    jsonWriter.Write(request.PremiseMissionTaskName);
                }
                if (request.CounterName != null)
                {
                    jsonWriter.WritePropertyName("counterName");
                    jsonWriter.Write(request.CounterName);
                }
                if (request.TargetResetType != null)
                {
                    jsonWriter.WritePropertyName("targetResetType");
                    jsonWriter.Write(request.TargetResetType);
                }
                if (request.TargetValue != null)
                {
                    jsonWriter.WritePropertyName("targetValue");
                    jsonWriter.Write(request.TargetValue.ToString());
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
		public IEnumerator UpdateMissionTaskModelMaster(
                Request.UpdateMissionTaskModelMasterRequest request,
                UnityAction<AsyncResult<Result.UpdateMissionTaskModelMasterResult>> callback
        ) =>
            new UpdateMissionTaskModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.UpdateMissionTaskModelMasterResult> UpdateMissionTaskModelMasterFuture(
                Request.UpdateMissionTaskModelMasterRequest request
        ) =>
            new UpdateMissionTaskModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.UpdateMissionTaskModelMasterResult> UpdateMissionTaskModelMasterAsync(
                Request.UpdateMissionTaskModelMasterRequest request
        ) =>
            new UpdateMissionTaskModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.UpdateMissionTaskModelMasterResult>();
    #else
		public UpdateMissionTaskModelMasterTask UpdateMissionTaskModelMasterAsync(
                Request.UpdateMissionTaskModelMasterRequest request
        )
		{
			return new UpdateMissionTaskModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.UpdateMissionTaskModelMasterResult> UpdateMissionTaskModelMasterAsync(
                Request.UpdateMissionTaskModelMasterRequest request
        ) =>
            new UpdateMissionTaskModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class DeleteMissionTaskModelMasterTask : Gs2RestSessionTask<DeleteMissionTaskModelMasterRequest, DeleteMissionTaskModelMasterResult>
        {
            public DeleteMissionTaskModelMasterTask(IGs2Session session, RestSessionRequestFactory factory, DeleteMissionTaskModelMasterRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DeleteMissionTaskModelMasterRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "mission")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/master/group/{missionGroupName}/task/{missionTaskName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{missionGroupName}", !string.IsNullOrEmpty(request.MissionGroupName) ? request.MissionGroupName.ToString() : "null");
                url = url.Replace("{missionTaskName}", !string.IsNullOrEmpty(request.MissionTaskName) ? request.MissionTaskName.ToString() : "null");

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
		public IEnumerator DeleteMissionTaskModelMaster(
                Request.DeleteMissionTaskModelMasterRequest request,
                UnityAction<AsyncResult<Result.DeleteMissionTaskModelMasterResult>> callback
        ) =>
            new DeleteMissionTaskModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.DeleteMissionTaskModelMasterResult> DeleteMissionTaskModelMasterFuture(
                Request.DeleteMissionTaskModelMasterRequest request
        ) =>
            new DeleteMissionTaskModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.DeleteMissionTaskModelMasterResult> DeleteMissionTaskModelMasterAsync(
                Request.DeleteMissionTaskModelMasterRequest request
        ) =>
            new DeleteMissionTaskModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.DeleteMissionTaskModelMasterResult>();
    #else
		public DeleteMissionTaskModelMasterTask DeleteMissionTaskModelMasterAsync(
                Request.DeleteMissionTaskModelMasterRequest request
        )
		{
			return new DeleteMissionTaskModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.DeleteMissionTaskModelMasterResult> DeleteMissionTaskModelMasterAsync(
                Request.DeleteMissionTaskModelMasterRequest request
        ) =>
            new DeleteMissionTaskModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif
	}
}