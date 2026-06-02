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
using Gs2.Gs2Schedule.Request;
using Gs2.Gs2Schedule.Result;
using Gs2.Util.LitJson;

namespace Gs2.Gs2Schedule
{
	public class Gs2ScheduleRestClient : AbstractGs2Client
	{
#if UNITY_2017_1_OR_NEWER
		private readonly CertificateHandler _certificateHandler;
#endif

		public static string Endpoint = "schedule";

        protected Gs2RestSession Gs2RestSession => (Gs2RestSession) Gs2Session;

		public Gs2ScheduleRestClient(Gs2RestSession Gs2RestSession) : base(Gs2RestSession)
		{

		}

#if UNITY_2017_1_OR_NEWER
		public Gs2ScheduleRestClient(Gs2RestSession gs2RestSession, CertificateHandler certificateHandler) : base(gs2RestSession)
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
                    .Replace("{service}", "schedule")
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
                    .Replace("{service}", "schedule")
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
                    .Replace("{service}", "schedule")
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
                    .Replace("{service}", "schedule")
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
                    .Replace("{service}", "schedule")
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
                    .Replace("{service}", "schedule")
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
                    .Replace("{service}", "schedule")
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
                    .Replace("{service}", "schedule")
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
                    .Replace("{service}", "schedule")
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
                    .Replace("{service}", "schedule")
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
                    .Replace("{service}", "schedule")
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
                    .Replace("{service}", "schedule")
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
                    .Replace("{service}", "schedule")
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
                    .Replace("{service}", "schedule")
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


        public class DescribeEventMastersTask : Gs2RestSessionTask<DescribeEventMastersRequest, DescribeEventMastersResult>
        {
            public DescribeEventMastersTask(IGs2Session session, RestSessionRequestFactory factory, DescribeEventMastersRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DescribeEventMastersRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "schedule")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/master/event";

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
		public IEnumerator DescribeEventMasters(
                Request.DescribeEventMastersRequest request,
                UnityAction<AsyncResult<Result.DescribeEventMastersResult>> callback
        ) =>
            new DescribeEventMastersTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.DescribeEventMastersResult> DescribeEventMastersFuture(
                Request.DescribeEventMastersRequest request
        ) =>
            new DescribeEventMastersTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.DescribeEventMastersResult> DescribeEventMastersAsync(
                Request.DescribeEventMastersRequest request
        ) =>
            new DescribeEventMastersTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.DescribeEventMastersResult>();
    #else
		public DescribeEventMastersTask DescribeEventMastersAsync(
                Request.DescribeEventMastersRequest request
        )
		{
			return new DescribeEventMastersTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.DescribeEventMastersResult> DescribeEventMastersAsync(
                Request.DescribeEventMastersRequest request
        ) =>
            new DescribeEventMastersTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class CreateEventMasterTask : Gs2RestSessionTask<CreateEventMasterRequest, CreateEventMasterResult>
        {
            public CreateEventMasterTask(IGs2Session session, RestSessionRequestFactory factory, CreateEventMasterRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(CreateEventMasterRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "schedule")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/master/event";

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
                if (request.ScheduleType != null)
                {
                    jsonWriter.WritePropertyName("scheduleType");
                    jsonWriter.Write(request.ScheduleType);
                }
                if (request.AbsoluteBegin != null)
                {
                    jsonWriter.WritePropertyName("absoluteBegin");
                    jsonWriter.Write(request.AbsoluteBegin.ToString());
                }
                if (request.AbsoluteEnd != null)
                {
                    jsonWriter.WritePropertyName("absoluteEnd");
                    jsonWriter.Write(request.AbsoluteEnd.ToString());
                }
                if (request.RepeatType != null)
                {
                    jsonWriter.WritePropertyName("repeatType");
                    jsonWriter.Write(request.RepeatType);
                }
                if (request.RepeatBeginDayOfMonth != null)
                {
                    jsonWriter.WritePropertyName("repeatBeginDayOfMonth");
                    jsonWriter.Write(request.RepeatBeginDayOfMonth.ToString());
                }
                if (request.RepeatEndDayOfMonth != null)
                {
                    jsonWriter.WritePropertyName("repeatEndDayOfMonth");
                    jsonWriter.Write(request.RepeatEndDayOfMonth.ToString());
                }
                if (request.RepeatBeginDayOfWeek != null)
                {
                    jsonWriter.WritePropertyName("repeatBeginDayOfWeek");
                    jsonWriter.Write(request.RepeatBeginDayOfWeek);
                }
                if (request.RepeatEndDayOfWeek != null)
                {
                    jsonWriter.WritePropertyName("repeatEndDayOfWeek");
                    jsonWriter.Write(request.RepeatEndDayOfWeek);
                }
                if (request.RepeatBeginHour != null)
                {
                    jsonWriter.WritePropertyName("repeatBeginHour");
                    jsonWriter.Write(request.RepeatBeginHour.ToString());
                }
                if (request.RepeatEndHour != null)
                {
                    jsonWriter.WritePropertyName("repeatEndHour");
                    jsonWriter.Write(request.RepeatEndHour.ToString());
                }
                if (request.RelativeTriggerName != null)
                {
                    jsonWriter.WritePropertyName("relativeTriggerName");
                    jsonWriter.Write(request.RelativeTriggerName);
                }
                if (request.RepeatSetting != null)
                {
                    jsonWriter.WritePropertyName("repeatSetting");
                    request.RepeatSetting.WriteJson(jsonWriter);
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
		public IEnumerator CreateEventMaster(
                Request.CreateEventMasterRequest request,
                UnityAction<AsyncResult<Result.CreateEventMasterResult>> callback
        ) =>
            new CreateEventMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.CreateEventMasterResult> CreateEventMasterFuture(
                Request.CreateEventMasterRequest request
        ) =>
            new CreateEventMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.CreateEventMasterResult> CreateEventMasterAsync(
                Request.CreateEventMasterRequest request
        ) =>
            new CreateEventMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.CreateEventMasterResult>();
    #else
		public CreateEventMasterTask CreateEventMasterAsync(
                Request.CreateEventMasterRequest request
        )
		{
			return new CreateEventMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.CreateEventMasterResult> CreateEventMasterAsync(
                Request.CreateEventMasterRequest request
        ) =>
            new CreateEventMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class GetEventMasterTask : Gs2RestSessionTask<GetEventMasterRequest, GetEventMasterResult>
        {
            public GetEventMasterTask(IGs2Session session, RestSessionRequestFactory factory, GetEventMasterRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(GetEventMasterRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "schedule")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/master/event/{eventName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{eventName}", !string.IsNullOrEmpty(request.EventName) ? request.EventName.ToString() : "null");

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
		public IEnumerator GetEventMaster(
                Request.GetEventMasterRequest request,
                UnityAction<AsyncResult<Result.GetEventMasterResult>> callback
        ) =>
            new GetEventMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.GetEventMasterResult> GetEventMasterFuture(
                Request.GetEventMasterRequest request
        ) =>
            new GetEventMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.GetEventMasterResult> GetEventMasterAsync(
                Request.GetEventMasterRequest request
        ) =>
            new GetEventMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.GetEventMasterResult>();
    #else
		public GetEventMasterTask GetEventMasterAsync(
                Request.GetEventMasterRequest request
        )
		{
			return new GetEventMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.GetEventMasterResult> GetEventMasterAsync(
                Request.GetEventMasterRequest request
        ) =>
            new GetEventMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class UpdateEventMasterTask : Gs2RestSessionTask<UpdateEventMasterRequest, UpdateEventMasterResult>
        {
            public UpdateEventMasterTask(IGs2Session session, RestSessionRequestFactory factory, UpdateEventMasterRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(UpdateEventMasterRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "schedule")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/master/event/{eventName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{eventName}", !string.IsNullOrEmpty(request.EventName) ? request.EventName.ToString() : "null");

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
                if (request.ScheduleType != null)
                {
                    jsonWriter.WritePropertyName("scheduleType");
                    jsonWriter.Write(request.ScheduleType);
                }
                if (request.AbsoluteBegin != null)
                {
                    jsonWriter.WritePropertyName("absoluteBegin");
                    jsonWriter.Write(request.AbsoluteBegin.ToString());
                }
                if (request.AbsoluteEnd != null)
                {
                    jsonWriter.WritePropertyName("absoluteEnd");
                    jsonWriter.Write(request.AbsoluteEnd.ToString());
                }
                if (request.RepeatType != null)
                {
                    jsonWriter.WritePropertyName("repeatType");
                    jsonWriter.Write(request.RepeatType);
                }
                if (request.RepeatBeginDayOfMonth != null)
                {
                    jsonWriter.WritePropertyName("repeatBeginDayOfMonth");
                    jsonWriter.Write(request.RepeatBeginDayOfMonth.ToString());
                }
                if (request.RepeatEndDayOfMonth != null)
                {
                    jsonWriter.WritePropertyName("repeatEndDayOfMonth");
                    jsonWriter.Write(request.RepeatEndDayOfMonth.ToString());
                }
                if (request.RepeatBeginDayOfWeek != null)
                {
                    jsonWriter.WritePropertyName("repeatBeginDayOfWeek");
                    jsonWriter.Write(request.RepeatBeginDayOfWeek);
                }
                if (request.RepeatEndDayOfWeek != null)
                {
                    jsonWriter.WritePropertyName("repeatEndDayOfWeek");
                    jsonWriter.Write(request.RepeatEndDayOfWeek);
                }
                if (request.RepeatBeginHour != null)
                {
                    jsonWriter.WritePropertyName("repeatBeginHour");
                    jsonWriter.Write(request.RepeatBeginHour.ToString());
                }
                if (request.RepeatEndHour != null)
                {
                    jsonWriter.WritePropertyName("repeatEndHour");
                    jsonWriter.Write(request.RepeatEndHour.ToString());
                }
                if (request.RelativeTriggerName != null)
                {
                    jsonWriter.WritePropertyName("relativeTriggerName");
                    jsonWriter.Write(request.RelativeTriggerName);
                }
                if (request.RepeatSetting != null)
                {
                    jsonWriter.WritePropertyName("repeatSetting");
                    request.RepeatSetting.WriteJson(jsonWriter);
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
		public IEnumerator UpdateEventMaster(
                Request.UpdateEventMasterRequest request,
                UnityAction<AsyncResult<Result.UpdateEventMasterResult>> callback
        ) =>
            new UpdateEventMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.UpdateEventMasterResult> UpdateEventMasterFuture(
                Request.UpdateEventMasterRequest request
        ) =>
            new UpdateEventMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.UpdateEventMasterResult> UpdateEventMasterAsync(
                Request.UpdateEventMasterRequest request
        ) =>
            new UpdateEventMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.UpdateEventMasterResult>();
    #else
		public UpdateEventMasterTask UpdateEventMasterAsync(
                Request.UpdateEventMasterRequest request
        )
		{
			return new UpdateEventMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.UpdateEventMasterResult> UpdateEventMasterAsync(
                Request.UpdateEventMasterRequest request
        ) =>
            new UpdateEventMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class DeleteEventMasterTask : Gs2RestSessionTask<DeleteEventMasterRequest, DeleteEventMasterResult>
        {
            public DeleteEventMasterTask(IGs2Session session, RestSessionRequestFactory factory, DeleteEventMasterRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DeleteEventMasterRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "schedule")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/master/event/{eventName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{eventName}", !string.IsNullOrEmpty(request.EventName) ? request.EventName.ToString() : "null");

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
		public IEnumerator DeleteEventMaster(
                Request.DeleteEventMasterRequest request,
                UnityAction<AsyncResult<Result.DeleteEventMasterResult>> callback
        ) =>
            new DeleteEventMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.DeleteEventMasterResult> DeleteEventMasterFuture(
                Request.DeleteEventMasterRequest request
        ) =>
            new DeleteEventMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.DeleteEventMasterResult> DeleteEventMasterAsync(
                Request.DeleteEventMasterRequest request
        ) =>
            new DeleteEventMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.DeleteEventMasterResult>();
    #else
		public DeleteEventMasterTask DeleteEventMasterAsync(
                Request.DeleteEventMasterRequest request
        )
		{
			return new DeleteEventMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.DeleteEventMasterResult> DeleteEventMasterAsync(
                Request.DeleteEventMasterRequest request
        ) =>
            new DeleteEventMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class DescribeTriggersTask : Gs2RestSessionTask<DescribeTriggersRequest, DescribeTriggersResult>
        {
            public DescribeTriggersTask(IGs2Session session, RestSessionRequestFactory factory, DescribeTriggersRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DescribeTriggersRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "schedule")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/me/trigger";

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
		public IEnumerator DescribeTriggers(
                Request.DescribeTriggersRequest request,
                UnityAction<AsyncResult<Result.DescribeTriggersResult>> callback
        ) =>
            new DescribeTriggersTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.DescribeTriggersResult> DescribeTriggersFuture(
                Request.DescribeTriggersRequest request
        ) =>
            new DescribeTriggersTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.DescribeTriggersResult> DescribeTriggersAsync(
                Request.DescribeTriggersRequest request
        ) =>
            new DescribeTriggersTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.DescribeTriggersResult>();
    #else
		public DescribeTriggersTask DescribeTriggersAsync(
                Request.DescribeTriggersRequest request
        )
		{
			return new DescribeTriggersTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.DescribeTriggersResult> DescribeTriggersAsync(
                Request.DescribeTriggersRequest request
        ) =>
            new DescribeTriggersTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class DescribeTriggersByUserIdTask : Gs2RestSessionTask<DescribeTriggersByUserIdRequest, DescribeTriggersByUserIdResult>
        {
            public DescribeTriggersByUserIdTask(IGs2Session session, RestSessionRequestFactory factory, DescribeTriggersByUserIdRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DescribeTriggersByUserIdRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "schedule")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/{userId}/trigger";

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
		public IEnumerator DescribeTriggersByUserId(
                Request.DescribeTriggersByUserIdRequest request,
                UnityAction<AsyncResult<Result.DescribeTriggersByUserIdResult>> callback
        ) =>
            new DescribeTriggersByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.DescribeTriggersByUserIdResult> DescribeTriggersByUserIdFuture(
                Request.DescribeTriggersByUserIdRequest request
        ) =>
            new DescribeTriggersByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.DescribeTriggersByUserIdResult> DescribeTriggersByUserIdAsync(
                Request.DescribeTriggersByUserIdRequest request
        ) =>
            new DescribeTriggersByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.DescribeTriggersByUserIdResult>();
    #else
		public DescribeTriggersByUserIdTask DescribeTriggersByUserIdAsync(
                Request.DescribeTriggersByUserIdRequest request
        )
		{
			return new DescribeTriggersByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.DescribeTriggersByUserIdResult> DescribeTriggersByUserIdAsync(
                Request.DescribeTriggersByUserIdRequest request
        ) =>
            new DescribeTriggersByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class GetTriggerTask : Gs2RestSessionTask<GetTriggerRequest, GetTriggerResult>
        {
            public GetTriggerTask(IGs2Session session, RestSessionRequestFactory factory, GetTriggerRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(GetTriggerRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "schedule")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/me/trigger/{triggerName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{triggerName}", !string.IsNullOrEmpty(request.TriggerName) ? request.TriggerName.ToString() : "null");

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
		public IEnumerator GetTrigger(
                Request.GetTriggerRequest request,
                UnityAction<AsyncResult<Result.GetTriggerResult>> callback
        ) =>
            new GetTriggerTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.GetTriggerResult> GetTriggerFuture(
                Request.GetTriggerRequest request
        ) =>
            new GetTriggerTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.GetTriggerResult> GetTriggerAsync(
                Request.GetTriggerRequest request
        ) =>
            new GetTriggerTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.GetTriggerResult>();
    #else
		public GetTriggerTask GetTriggerAsync(
                Request.GetTriggerRequest request
        )
		{
			return new GetTriggerTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.GetTriggerResult> GetTriggerAsync(
                Request.GetTriggerRequest request
        ) =>
            new GetTriggerTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class GetTriggerByUserIdTask : Gs2RestSessionTask<GetTriggerByUserIdRequest, GetTriggerByUserIdResult>
        {
            public GetTriggerByUserIdTask(IGs2Session session, RestSessionRequestFactory factory, GetTriggerByUserIdRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(GetTriggerByUserIdRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "schedule")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/{userId}/trigger/{triggerName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{userId}", !string.IsNullOrEmpty(request.UserId) ? request.UserId.ToString() : "null");
                url = url.Replace("{triggerName}", !string.IsNullOrEmpty(request.TriggerName) ? request.TriggerName.ToString() : "null");

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
		public IEnumerator GetTriggerByUserId(
                Request.GetTriggerByUserIdRequest request,
                UnityAction<AsyncResult<Result.GetTriggerByUserIdResult>> callback
        ) =>
            new GetTriggerByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.GetTriggerByUserIdResult> GetTriggerByUserIdFuture(
                Request.GetTriggerByUserIdRequest request
        ) =>
            new GetTriggerByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.GetTriggerByUserIdResult> GetTriggerByUserIdAsync(
                Request.GetTriggerByUserIdRequest request
        ) =>
            new GetTriggerByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.GetTriggerByUserIdResult>();
    #else
		public GetTriggerByUserIdTask GetTriggerByUserIdAsync(
                Request.GetTriggerByUserIdRequest request
        )
		{
			return new GetTriggerByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.GetTriggerByUserIdResult> GetTriggerByUserIdAsync(
                Request.GetTriggerByUserIdRequest request
        ) =>
            new GetTriggerByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class TriggerByUserIdTask : Gs2RestSessionTask<TriggerByUserIdRequest, TriggerByUserIdResult>
        {
            public TriggerByUserIdTask(IGs2Session session, RestSessionRequestFactory factory, TriggerByUserIdRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(TriggerByUserIdRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "schedule")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/{userId}/trigger/{triggerName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{triggerName}", !string.IsNullOrEmpty(request.TriggerName) ? request.TriggerName.ToString() : "null");
                url = url.Replace("{userId}", !string.IsNullOrEmpty(request.UserId) ? request.UserId.ToString() : "null");

                var sessionRequest = Factory.Post(url);

                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);
                jsonWriter.WriteObjectStart();
                if (request.TriggerStrategy != null)
                {
                    jsonWriter.WritePropertyName("triggerStrategy");
                    jsonWriter.Write(request.TriggerStrategy);
                }
                if (request.Ttl != null)
                {
                    jsonWriter.WritePropertyName("ttl");
                    jsonWriter.Write(request.Ttl.ToString());
                }
                if (request.EventId != null)
                {
                    jsonWriter.WritePropertyName("eventId");
                    jsonWriter.Write(request.EventId);
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
		public IEnumerator TriggerByUserId(
                Request.TriggerByUserIdRequest request,
                UnityAction<AsyncResult<Result.TriggerByUserIdResult>> callback
        ) =>
            new TriggerByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.TriggerByUserIdResult> TriggerByUserIdFuture(
                Request.TriggerByUserIdRequest request
        ) =>
            new TriggerByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.TriggerByUserIdResult> TriggerByUserIdAsync(
                Request.TriggerByUserIdRequest request
        ) =>
            new TriggerByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.TriggerByUserIdResult>();
    #else
		public TriggerByUserIdTask TriggerByUserIdAsync(
                Request.TriggerByUserIdRequest request
        )
		{
			return new TriggerByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.TriggerByUserIdResult> TriggerByUserIdAsync(
                Request.TriggerByUserIdRequest request
        ) =>
            new TriggerByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class ExtendTriggerByUserIdTask : Gs2RestSessionTask<ExtendTriggerByUserIdRequest, ExtendTriggerByUserIdResult>
        {
            public ExtendTriggerByUserIdTask(IGs2Session session, RestSessionRequestFactory factory, ExtendTriggerByUserIdRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(ExtendTriggerByUserIdRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "schedule")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/{userId}/trigger/{triggerName}/extend";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{triggerName}", !string.IsNullOrEmpty(request.TriggerName) ? request.TriggerName.ToString() : "null");
                url = url.Replace("{userId}", !string.IsNullOrEmpty(request.UserId) ? request.UserId.ToString() : "null");

                var sessionRequest = Factory.Post(url);

                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);
                jsonWriter.WriteObjectStart();
                if (request.ExtendSeconds != null)
                {
                    jsonWriter.WritePropertyName("extendSeconds");
                    jsonWriter.Write(request.ExtendSeconds.ToString());
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
		public IEnumerator ExtendTriggerByUserId(
                Request.ExtendTriggerByUserIdRequest request,
                UnityAction<AsyncResult<Result.ExtendTriggerByUserIdResult>> callback
        ) =>
            new ExtendTriggerByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.ExtendTriggerByUserIdResult> ExtendTriggerByUserIdFuture(
                Request.ExtendTriggerByUserIdRequest request
        ) =>
            new ExtendTriggerByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.ExtendTriggerByUserIdResult> ExtendTriggerByUserIdAsync(
                Request.ExtendTriggerByUserIdRequest request
        ) =>
            new ExtendTriggerByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.ExtendTriggerByUserIdResult>();
    #else
		public ExtendTriggerByUserIdTask ExtendTriggerByUserIdAsync(
                Request.ExtendTriggerByUserIdRequest request
        )
		{
			return new ExtendTriggerByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.ExtendTriggerByUserIdResult> ExtendTriggerByUserIdAsync(
                Request.ExtendTriggerByUserIdRequest request
        ) =>
            new ExtendTriggerByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class TriggerByStampSheetTask : Gs2RestSessionTask<TriggerByStampSheetRequest, TriggerByStampSheetResult>
        {
            public TriggerByStampSheetTask(IGs2Session session, RestSessionRequestFactory factory, TriggerByStampSheetRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(TriggerByStampSheetRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "schedule")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/stamp/trigger";

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
		public IEnumerator TriggerByStampSheet(
                Request.TriggerByStampSheetRequest request,
                UnityAction<AsyncResult<Result.TriggerByStampSheetResult>> callback
        ) =>
            new TriggerByStampSheetTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.TriggerByStampSheetResult> TriggerByStampSheetFuture(
                Request.TriggerByStampSheetRequest request
        ) =>
            new TriggerByStampSheetTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.TriggerByStampSheetResult> TriggerByStampSheetAsync(
                Request.TriggerByStampSheetRequest request
        ) =>
            new TriggerByStampSheetTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.TriggerByStampSheetResult>();
    #else
		public TriggerByStampSheetTask TriggerByStampSheetAsync(
                Request.TriggerByStampSheetRequest request
        )
		{
			return new TriggerByStampSheetTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.TriggerByStampSheetResult> TriggerByStampSheetAsync(
                Request.TriggerByStampSheetRequest request
        ) =>
            new TriggerByStampSheetTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class ExtendTriggerByStampSheetTask : Gs2RestSessionTask<ExtendTriggerByStampSheetRequest, ExtendTriggerByStampSheetResult>
        {
            public ExtendTriggerByStampSheetTask(IGs2Session session, RestSessionRequestFactory factory, ExtendTriggerByStampSheetRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(ExtendTriggerByStampSheetRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "schedule")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/stamp/trigger/extend";

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
		public IEnumerator ExtendTriggerByStampSheet(
                Request.ExtendTriggerByStampSheetRequest request,
                UnityAction<AsyncResult<Result.ExtendTriggerByStampSheetResult>> callback
        ) =>
            new ExtendTriggerByStampSheetTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.ExtendTriggerByStampSheetResult> ExtendTriggerByStampSheetFuture(
                Request.ExtendTriggerByStampSheetRequest request
        ) =>
            new ExtendTriggerByStampSheetTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.ExtendTriggerByStampSheetResult> ExtendTriggerByStampSheetAsync(
                Request.ExtendTriggerByStampSheetRequest request
        ) =>
            new ExtendTriggerByStampSheetTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.ExtendTriggerByStampSheetResult>();
    #else
		public ExtendTriggerByStampSheetTask ExtendTriggerByStampSheetAsync(
                Request.ExtendTriggerByStampSheetRequest request
        )
		{
			return new ExtendTriggerByStampSheetTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.ExtendTriggerByStampSheetResult> ExtendTriggerByStampSheetAsync(
                Request.ExtendTriggerByStampSheetRequest request
        ) =>
            new ExtendTriggerByStampSheetTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class DeleteTriggerTask : Gs2RestSessionTask<DeleteTriggerRequest, DeleteTriggerResult>
        {
            public DeleteTriggerTask(IGs2Session session, RestSessionRequestFactory factory, DeleteTriggerRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DeleteTriggerRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "schedule")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/me/trigger/{triggerName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{triggerName}", !string.IsNullOrEmpty(request.TriggerName) ? request.TriggerName.ToString() : "null");

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
		public IEnumerator DeleteTrigger(
                Request.DeleteTriggerRequest request,
                UnityAction<AsyncResult<Result.DeleteTriggerResult>> callback
        ) =>
            new DeleteTriggerTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.DeleteTriggerResult> DeleteTriggerFuture(
                Request.DeleteTriggerRequest request
        ) =>
            new DeleteTriggerTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.DeleteTriggerResult> DeleteTriggerAsync(
                Request.DeleteTriggerRequest request
        ) =>
            new DeleteTriggerTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.DeleteTriggerResult>();
    #else
		public DeleteTriggerTask DeleteTriggerAsync(
                Request.DeleteTriggerRequest request
        )
		{
			return new DeleteTriggerTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.DeleteTriggerResult> DeleteTriggerAsync(
                Request.DeleteTriggerRequest request
        ) =>
            new DeleteTriggerTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class DeleteTriggerByUserIdTask : Gs2RestSessionTask<DeleteTriggerByUserIdRequest, DeleteTriggerByUserIdResult>
        {
            public DeleteTriggerByUserIdTask(IGs2Session session, RestSessionRequestFactory factory, DeleteTriggerByUserIdRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DeleteTriggerByUserIdRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "schedule")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/{userId}/trigger/{triggerName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{userId}", !string.IsNullOrEmpty(request.UserId) ? request.UserId.ToString() : "null");
                url = url.Replace("{triggerName}", !string.IsNullOrEmpty(request.TriggerName) ? request.TriggerName.ToString() : "null");

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
		public IEnumerator DeleteTriggerByUserId(
                Request.DeleteTriggerByUserIdRequest request,
                UnityAction<AsyncResult<Result.DeleteTriggerByUserIdResult>> callback
        ) =>
            new DeleteTriggerByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.DeleteTriggerByUserIdResult> DeleteTriggerByUserIdFuture(
                Request.DeleteTriggerByUserIdRequest request
        ) =>
            new DeleteTriggerByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.DeleteTriggerByUserIdResult> DeleteTriggerByUserIdAsync(
                Request.DeleteTriggerByUserIdRequest request
        ) =>
            new DeleteTriggerByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.DeleteTriggerByUserIdResult>();
    #else
		public DeleteTriggerByUserIdTask DeleteTriggerByUserIdAsync(
                Request.DeleteTriggerByUserIdRequest request
        )
		{
			return new DeleteTriggerByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.DeleteTriggerByUserIdResult> DeleteTriggerByUserIdAsync(
                Request.DeleteTriggerByUserIdRequest request
        ) =>
            new DeleteTriggerByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class VerifyTriggerTask : Gs2RestSessionTask<VerifyTriggerRequest, VerifyTriggerResult>
        {
            public VerifyTriggerTask(IGs2Session session, RestSessionRequestFactory factory, VerifyTriggerRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(VerifyTriggerRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "schedule")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/me/trigger/{triggerName}/verify/{verifyType}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{triggerName}", !string.IsNullOrEmpty(request.TriggerName) ? request.TriggerName.ToString() : "null");
                url = url.Replace("{verifyType}", !string.IsNullOrEmpty(request.VerifyType) ? request.VerifyType.ToString() : "null");

                var sessionRequest = Factory.Post(url);

                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);
                jsonWriter.WriteObjectStart();
                if (request.ElapsedMinutes != null)
                {
                    jsonWriter.WritePropertyName("elapsedMinutes");
                    jsonWriter.Write(request.ElapsedMinutes.ToString());
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
		public IEnumerator VerifyTrigger(
                Request.VerifyTriggerRequest request,
                UnityAction<AsyncResult<Result.VerifyTriggerResult>> callback
        ) =>
            new VerifyTriggerTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.VerifyTriggerResult> VerifyTriggerFuture(
                Request.VerifyTriggerRequest request
        ) =>
            new VerifyTriggerTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.VerifyTriggerResult> VerifyTriggerAsync(
                Request.VerifyTriggerRequest request
        ) =>
            new VerifyTriggerTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.VerifyTriggerResult>();
    #else
		public VerifyTriggerTask VerifyTriggerAsync(
                Request.VerifyTriggerRequest request
        )
		{
			return new VerifyTriggerTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.VerifyTriggerResult> VerifyTriggerAsync(
                Request.VerifyTriggerRequest request
        ) =>
            new VerifyTriggerTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class VerifyTriggerByUserIdTask : Gs2RestSessionTask<VerifyTriggerByUserIdRequest, VerifyTriggerByUserIdResult>
        {
            public VerifyTriggerByUserIdTask(IGs2Session session, RestSessionRequestFactory factory, VerifyTriggerByUserIdRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(VerifyTriggerByUserIdRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "schedule")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/{userId}/trigger/{triggerName}/verify/{verifyType}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{userId}", !string.IsNullOrEmpty(request.UserId) ? request.UserId.ToString() : "null");
                url = url.Replace("{triggerName}", !string.IsNullOrEmpty(request.TriggerName) ? request.TriggerName.ToString() : "null");
                url = url.Replace("{verifyType}", !string.IsNullOrEmpty(request.VerifyType) ? request.VerifyType.ToString() : "null");

                var sessionRequest = Factory.Post(url);

                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);
                jsonWriter.WriteObjectStart();
                if (request.ElapsedMinutes != null)
                {
                    jsonWriter.WritePropertyName("elapsedMinutes");
                    jsonWriter.Write(request.ElapsedMinutes.ToString());
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
		public IEnumerator VerifyTriggerByUserId(
                Request.VerifyTriggerByUserIdRequest request,
                UnityAction<AsyncResult<Result.VerifyTriggerByUserIdResult>> callback
        ) =>
            new VerifyTriggerByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.VerifyTriggerByUserIdResult> VerifyTriggerByUserIdFuture(
                Request.VerifyTriggerByUserIdRequest request
        ) =>
            new VerifyTriggerByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.VerifyTriggerByUserIdResult> VerifyTriggerByUserIdAsync(
                Request.VerifyTriggerByUserIdRequest request
        ) =>
            new VerifyTriggerByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.VerifyTriggerByUserIdResult>();
    #else
		public VerifyTriggerByUserIdTask VerifyTriggerByUserIdAsync(
                Request.VerifyTriggerByUserIdRequest request
        )
		{
			return new VerifyTriggerByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.VerifyTriggerByUserIdResult> VerifyTriggerByUserIdAsync(
                Request.VerifyTriggerByUserIdRequest request
        ) =>
            new VerifyTriggerByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class DeleteTriggerByStampTaskTask : Gs2RestSessionTask<DeleteTriggerByStampTaskRequest, DeleteTriggerByStampTaskResult>
        {
            public DeleteTriggerByStampTaskTask(IGs2Session session, RestSessionRequestFactory factory, DeleteTriggerByStampTaskRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DeleteTriggerByStampTaskRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "schedule")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/stamp/trigger/delete";

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
		public IEnumerator DeleteTriggerByStampTask(
                Request.DeleteTriggerByStampTaskRequest request,
                UnityAction<AsyncResult<Result.DeleteTriggerByStampTaskResult>> callback
        ) =>
            new DeleteTriggerByStampTaskTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.DeleteTriggerByStampTaskResult> DeleteTriggerByStampTaskFuture(
                Request.DeleteTriggerByStampTaskRequest request
        ) =>
            new DeleteTriggerByStampTaskTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.DeleteTriggerByStampTaskResult> DeleteTriggerByStampTaskAsync(
                Request.DeleteTriggerByStampTaskRequest request
        ) =>
            new DeleteTriggerByStampTaskTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.DeleteTriggerByStampTaskResult>();
    #else
		public DeleteTriggerByStampTaskTask DeleteTriggerByStampTaskAsync(
                Request.DeleteTriggerByStampTaskRequest request
        )
		{
			return new DeleteTriggerByStampTaskTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.DeleteTriggerByStampTaskResult> DeleteTriggerByStampTaskAsync(
                Request.DeleteTriggerByStampTaskRequest request
        ) =>
            new DeleteTriggerByStampTaskTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class VerifyTriggerByStampTaskTask : Gs2RestSessionTask<VerifyTriggerByStampTaskRequest, VerifyTriggerByStampTaskResult>
        {
            public VerifyTriggerByStampTaskTask(IGs2Session session, RestSessionRequestFactory factory, VerifyTriggerByStampTaskRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(VerifyTriggerByStampTaskRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "schedule")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/stamp/trigger/verify";

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
		public IEnumerator VerifyTriggerByStampTask(
                Request.VerifyTriggerByStampTaskRequest request,
                UnityAction<AsyncResult<Result.VerifyTriggerByStampTaskResult>> callback
        ) =>
            new VerifyTriggerByStampTaskTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.VerifyTriggerByStampTaskResult> VerifyTriggerByStampTaskFuture(
                Request.VerifyTriggerByStampTaskRequest request
        ) =>
            new VerifyTriggerByStampTaskTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.VerifyTriggerByStampTaskResult> VerifyTriggerByStampTaskAsync(
                Request.VerifyTriggerByStampTaskRequest request
        ) =>
            new VerifyTriggerByStampTaskTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.VerifyTriggerByStampTaskResult>();
    #else
		public VerifyTriggerByStampTaskTask VerifyTriggerByStampTaskAsync(
                Request.VerifyTriggerByStampTaskRequest request
        )
		{
			return new VerifyTriggerByStampTaskTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.VerifyTriggerByStampTaskResult> VerifyTriggerByStampTaskAsync(
                Request.VerifyTriggerByStampTaskRequest request
        ) =>
            new VerifyTriggerByStampTaskTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class DescribeEventsTask : Gs2RestSessionTask<DescribeEventsRequest, DescribeEventsResult>
        {
            public DescribeEventsTask(IGs2Session session, RestSessionRequestFactory factory, DescribeEventsRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DescribeEventsRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "schedule")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/me/event";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");

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
		public IEnumerator DescribeEvents(
                Request.DescribeEventsRequest request,
                UnityAction<AsyncResult<Result.DescribeEventsResult>> callback
        ) =>
            new DescribeEventsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.DescribeEventsResult> DescribeEventsFuture(
                Request.DescribeEventsRequest request
        ) =>
            new DescribeEventsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.DescribeEventsResult> DescribeEventsAsync(
                Request.DescribeEventsRequest request
        ) =>
            new DescribeEventsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.DescribeEventsResult>();
    #else
		public DescribeEventsTask DescribeEventsAsync(
                Request.DescribeEventsRequest request
        )
		{
			return new DescribeEventsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.DescribeEventsResult> DescribeEventsAsync(
                Request.DescribeEventsRequest request
        ) =>
            new DescribeEventsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class DescribeEventsByUserIdTask : Gs2RestSessionTask<DescribeEventsByUserIdRequest, DescribeEventsByUserIdResult>
        {
            public DescribeEventsByUserIdTask(IGs2Session session, RestSessionRequestFactory factory, DescribeEventsByUserIdRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DescribeEventsByUserIdRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "schedule")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/{userId}/event";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
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
		public IEnumerator DescribeEventsByUserId(
                Request.DescribeEventsByUserIdRequest request,
                UnityAction<AsyncResult<Result.DescribeEventsByUserIdResult>> callback
        ) =>
            new DescribeEventsByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.DescribeEventsByUserIdResult> DescribeEventsByUserIdFuture(
                Request.DescribeEventsByUserIdRequest request
        ) =>
            new DescribeEventsByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.DescribeEventsByUserIdResult> DescribeEventsByUserIdAsync(
                Request.DescribeEventsByUserIdRequest request
        ) =>
            new DescribeEventsByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.DescribeEventsByUserIdResult>();
    #else
		public DescribeEventsByUserIdTask DescribeEventsByUserIdAsync(
                Request.DescribeEventsByUserIdRequest request
        )
		{
			return new DescribeEventsByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.DescribeEventsByUserIdResult> DescribeEventsByUserIdAsync(
                Request.DescribeEventsByUserIdRequest request
        ) =>
            new DescribeEventsByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class DescribeRawEventsTask : Gs2RestSessionTask<DescribeRawEventsRequest, DescribeRawEventsResult>
        {
            public DescribeRawEventsTask(IGs2Session session, RestSessionRequestFactory factory, DescribeRawEventsRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DescribeRawEventsRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "schedule")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/event";

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
		public IEnumerator DescribeRawEvents(
                Request.DescribeRawEventsRequest request,
                UnityAction<AsyncResult<Result.DescribeRawEventsResult>> callback
        ) =>
            new DescribeRawEventsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.DescribeRawEventsResult> DescribeRawEventsFuture(
                Request.DescribeRawEventsRequest request
        ) =>
            new DescribeRawEventsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.DescribeRawEventsResult> DescribeRawEventsAsync(
                Request.DescribeRawEventsRequest request
        ) =>
            new DescribeRawEventsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.DescribeRawEventsResult>();
    #else
		public DescribeRawEventsTask DescribeRawEventsAsync(
                Request.DescribeRawEventsRequest request
        )
		{
			return new DescribeRawEventsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.DescribeRawEventsResult> DescribeRawEventsAsync(
                Request.DescribeRawEventsRequest request
        ) =>
            new DescribeRawEventsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class GetEventTask : Gs2RestSessionTask<GetEventRequest, GetEventResult>
        {
            public GetEventTask(IGs2Session session, RestSessionRequestFactory factory, GetEventRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(GetEventRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "schedule")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/me/event/{eventName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{eventName}", !string.IsNullOrEmpty(request.EventName) ? request.EventName.ToString() : "null");

                var sessionRequest = Factory.Get(url);
                if (request.ContextStack != null)
                {
                    sessionRequest.AddQueryString("contextStack", request.ContextStack);
                }
                if (request.IsInSchedule != null) {
                    sessionRequest.AddQueryString("isInSchedule", $"{request.IsInSchedule}");
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
		public IEnumerator GetEvent(
                Request.GetEventRequest request,
                UnityAction<AsyncResult<Result.GetEventResult>> callback
        ) =>
            new GetEventTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.GetEventResult> GetEventFuture(
                Request.GetEventRequest request
        ) =>
            new GetEventTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.GetEventResult> GetEventAsync(
                Request.GetEventRequest request
        ) =>
            new GetEventTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.GetEventResult>();
    #else
		public GetEventTask GetEventAsync(
                Request.GetEventRequest request
        )
		{
			return new GetEventTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.GetEventResult> GetEventAsync(
                Request.GetEventRequest request
        ) =>
            new GetEventTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class GetEventByUserIdTask : Gs2RestSessionTask<GetEventByUserIdRequest, GetEventByUserIdResult>
        {
            public GetEventByUserIdTask(IGs2Session session, RestSessionRequestFactory factory, GetEventByUserIdRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(GetEventByUserIdRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "schedule")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/{userId}/event/{eventName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{eventName}", !string.IsNullOrEmpty(request.EventName) ? request.EventName.ToString() : "null");
                url = url.Replace("{userId}", !string.IsNullOrEmpty(request.UserId) ? request.UserId.ToString() : "null");

                var sessionRequest = Factory.Get(url);
                if (request.ContextStack != null)
                {
                    sessionRequest.AddQueryString("contextStack", request.ContextStack);
                }
                if (request.IsInSchedule != null) {
                    sessionRequest.AddQueryString("isInSchedule", $"{request.IsInSchedule}");
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
		public IEnumerator GetEventByUserId(
                Request.GetEventByUserIdRequest request,
                UnityAction<AsyncResult<Result.GetEventByUserIdResult>> callback
        ) =>
            new GetEventByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.GetEventByUserIdResult> GetEventByUserIdFuture(
                Request.GetEventByUserIdRequest request
        ) =>
            new GetEventByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.GetEventByUserIdResult> GetEventByUserIdAsync(
                Request.GetEventByUserIdRequest request
        ) =>
            new GetEventByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.GetEventByUserIdResult>();
    #else
		public GetEventByUserIdTask GetEventByUserIdAsync(
                Request.GetEventByUserIdRequest request
        )
		{
			return new GetEventByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.GetEventByUserIdResult> GetEventByUserIdAsync(
                Request.GetEventByUserIdRequest request
        ) =>
            new GetEventByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class GetRawEventTask : Gs2RestSessionTask<GetRawEventRequest, GetRawEventResult>
        {
            public GetRawEventTask(IGs2Session session, RestSessionRequestFactory factory, GetRawEventRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(GetRawEventRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "schedule")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/event/{eventName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{eventName}", !string.IsNullOrEmpty(request.EventName) ? request.EventName.ToString() : "null");

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
		public IEnumerator GetRawEvent(
                Request.GetRawEventRequest request,
                UnityAction<AsyncResult<Result.GetRawEventResult>> callback
        ) =>
            new GetRawEventTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.GetRawEventResult> GetRawEventFuture(
                Request.GetRawEventRequest request
        ) =>
            new GetRawEventTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.GetRawEventResult> GetRawEventAsync(
                Request.GetRawEventRequest request
        ) =>
            new GetRawEventTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.GetRawEventResult>();
    #else
		public GetRawEventTask GetRawEventAsync(
                Request.GetRawEventRequest request
        )
		{
			return new GetRawEventTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.GetRawEventResult> GetRawEventAsync(
                Request.GetRawEventRequest request
        ) =>
            new GetRawEventTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class VerifyEventTask : Gs2RestSessionTask<VerifyEventRequest, VerifyEventResult>
        {
            public VerifyEventTask(IGs2Session session, RestSessionRequestFactory factory, VerifyEventRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(VerifyEventRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "schedule")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/me/event/{eventName}/verify/{verifyType}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{eventName}", !string.IsNullOrEmpty(request.EventName) ? request.EventName.ToString() : "null");
                url = url.Replace("{verifyType}", !string.IsNullOrEmpty(request.VerifyType) ? request.VerifyType.ToString() : "null");

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
		public IEnumerator VerifyEvent(
                Request.VerifyEventRequest request,
                UnityAction<AsyncResult<Result.VerifyEventResult>> callback
        ) =>
            new VerifyEventTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.VerifyEventResult> VerifyEventFuture(
                Request.VerifyEventRequest request
        ) =>
            new VerifyEventTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.VerifyEventResult> VerifyEventAsync(
                Request.VerifyEventRequest request
        ) =>
            new VerifyEventTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.VerifyEventResult>();
    #else
		public VerifyEventTask VerifyEventAsync(
                Request.VerifyEventRequest request
        )
		{
			return new VerifyEventTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.VerifyEventResult> VerifyEventAsync(
                Request.VerifyEventRequest request
        ) =>
            new VerifyEventTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class VerifyEventByUserIdTask : Gs2RestSessionTask<VerifyEventByUserIdRequest, VerifyEventByUserIdResult>
        {
            public VerifyEventByUserIdTask(IGs2Session session, RestSessionRequestFactory factory, VerifyEventByUserIdRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(VerifyEventByUserIdRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "schedule")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/{userId}/event/{eventName}/verify/{verifyType}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{userId}", !string.IsNullOrEmpty(request.UserId) ? request.UserId.ToString() : "null");
                url = url.Replace("{eventName}", !string.IsNullOrEmpty(request.EventName) ? request.EventName.ToString() : "null");
                url = url.Replace("{verifyType}", !string.IsNullOrEmpty(request.VerifyType) ? request.VerifyType.ToString() : "null");

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
		public IEnumerator VerifyEventByUserId(
                Request.VerifyEventByUserIdRequest request,
                UnityAction<AsyncResult<Result.VerifyEventByUserIdResult>> callback
        ) =>
            new VerifyEventByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.VerifyEventByUserIdResult> VerifyEventByUserIdFuture(
                Request.VerifyEventByUserIdRequest request
        ) =>
            new VerifyEventByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.VerifyEventByUserIdResult> VerifyEventByUserIdAsync(
                Request.VerifyEventByUserIdRequest request
        ) =>
            new VerifyEventByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.VerifyEventByUserIdResult>();
    #else
		public VerifyEventByUserIdTask VerifyEventByUserIdAsync(
                Request.VerifyEventByUserIdRequest request
        )
		{
			return new VerifyEventByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.VerifyEventByUserIdResult> VerifyEventByUserIdAsync(
                Request.VerifyEventByUserIdRequest request
        ) =>
            new VerifyEventByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class VerifyEventByStampTaskTask : Gs2RestSessionTask<VerifyEventByStampTaskRequest, VerifyEventByStampTaskResult>
        {
            public VerifyEventByStampTaskTask(IGs2Session session, RestSessionRequestFactory factory, VerifyEventByStampTaskRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(VerifyEventByStampTaskRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "schedule")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/stamp/event/verify";

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
		public IEnumerator VerifyEventByStampTask(
                Request.VerifyEventByStampTaskRequest request,
                UnityAction<AsyncResult<Result.VerifyEventByStampTaskResult>> callback
        ) =>
            new VerifyEventByStampTaskTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.VerifyEventByStampTaskResult> VerifyEventByStampTaskFuture(
                Request.VerifyEventByStampTaskRequest request
        ) =>
            new VerifyEventByStampTaskTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.VerifyEventByStampTaskResult> VerifyEventByStampTaskAsync(
                Request.VerifyEventByStampTaskRequest request
        ) =>
            new VerifyEventByStampTaskTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.VerifyEventByStampTaskResult>();
    #else
		public VerifyEventByStampTaskTask VerifyEventByStampTaskAsync(
                Request.VerifyEventByStampTaskRequest request
        )
		{
			return new VerifyEventByStampTaskTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.VerifyEventByStampTaskResult> VerifyEventByStampTaskAsync(
                Request.VerifyEventByStampTaskRequest request
        ) =>
            new VerifyEventByStampTaskTask(
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
                    .Replace("{service}", "schedule")
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


        public class GetCurrentEventMasterTask : Gs2RestSessionTask<GetCurrentEventMasterRequest, GetCurrentEventMasterResult>
        {
            public GetCurrentEventMasterTask(IGs2Session session, RestSessionRequestFactory factory, GetCurrentEventMasterRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(GetCurrentEventMasterRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "schedule")
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
		public IEnumerator GetCurrentEventMaster(
                Request.GetCurrentEventMasterRequest request,
                UnityAction<AsyncResult<Result.GetCurrentEventMasterResult>> callback
        ) =>
            new GetCurrentEventMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.GetCurrentEventMasterResult> GetCurrentEventMasterFuture(
                Request.GetCurrentEventMasterRequest request
        ) =>
            new GetCurrentEventMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.GetCurrentEventMasterResult> GetCurrentEventMasterAsync(
                Request.GetCurrentEventMasterRequest request
        ) =>
            new GetCurrentEventMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.GetCurrentEventMasterResult>();
    #else
		public GetCurrentEventMasterTask GetCurrentEventMasterAsync(
                Request.GetCurrentEventMasterRequest request
        )
		{
			return new GetCurrentEventMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.GetCurrentEventMasterResult> GetCurrentEventMasterAsync(
                Request.GetCurrentEventMasterRequest request
        ) =>
            new GetCurrentEventMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class PreUpdateCurrentEventMasterTask : Gs2RestSessionTask<PreUpdateCurrentEventMasterRequest, PreUpdateCurrentEventMasterResult>
        {
            public PreUpdateCurrentEventMasterTask(IGs2Session session, RestSessionRequestFactory factory, PreUpdateCurrentEventMasterRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(PreUpdateCurrentEventMasterRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "schedule")
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
		public IEnumerator PreUpdateCurrentEventMaster(
                Request.PreUpdateCurrentEventMasterRequest request,
                UnityAction<AsyncResult<Result.PreUpdateCurrentEventMasterResult>> callback
        ) =>
            new PreUpdateCurrentEventMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.PreUpdateCurrentEventMasterResult> PreUpdateCurrentEventMasterFuture(
                Request.PreUpdateCurrentEventMasterRequest request
        ) =>
            new PreUpdateCurrentEventMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.PreUpdateCurrentEventMasterResult> PreUpdateCurrentEventMasterAsync(
                Request.PreUpdateCurrentEventMasterRequest request
        ) =>
            new PreUpdateCurrentEventMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.PreUpdateCurrentEventMasterResult>();
    #else
		public PreUpdateCurrentEventMasterTask PreUpdateCurrentEventMasterAsync(
                Request.PreUpdateCurrentEventMasterRequest request
        )
		{
			return new PreUpdateCurrentEventMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.PreUpdateCurrentEventMasterResult> PreUpdateCurrentEventMasterAsync(
                Request.PreUpdateCurrentEventMasterRequest request
        ) =>
            new PreUpdateCurrentEventMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class UpdateCurrentEventMasterTask : Gs2RestSessionTask<UpdateCurrentEventMasterRequest, UpdateCurrentEventMasterResult>
        {
            public UpdateCurrentEventMasterTask(IGs2Session session, RestSessionRequestFactory factory, UpdateCurrentEventMasterRequest request) : base(session, factory, request)
            {
            }
#if GS2_ENABLE_UNITASK
            protected override async UniTask<UpdateCurrentEventMasterResult> InvokeImpl()
#else
            protected override async Task<UpdateCurrentEventMasterResult> InvokeImpl()
#endif
            {
                if (Request.Settings != null) {
                    var preTask = new PreUpdateCurrentEventMasterTask(
                        Session,
                        Factory,
                        new PreUpdateCurrentEventMasterRequest()
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

            protected override IGs2SessionRequest CreateRequest(UpdateCurrentEventMasterRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "schedule")
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
		public IEnumerator UpdateCurrentEventMaster(
                Request.UpdateCurrentEventMasterRequest request,
                UnityAction<AsyncResult<Result.UpdateCurrentEventMasterResult>> callback
        ) =>
            new UpdateCurrentEventMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.UpdateCurrentEventMasterResult> UpdateCurrentEventMasterFuture(
                Request.UpdateCurrentEventMasterRequest request
        ) =>
            new UpdateCurrentEventMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.UpdateCurrentEventMasterResult> UpdateCurrentEventMasterAsync(
                Request.UpdateCurrentEventMasterRequest request
        ) =>
            new UpdateCurrentEventMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.UpdateCurrentEventMasterResult>();
    #else
		public UpdateCurrentEventMasterTask UpdateCurrentEventMasterAsync(
                Request.UpdateCurrentEventMasterRequest request
        )
		{
			return new UpdateCurrentEventMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.UpdateCurrentEventMasterResult> UpdateCurrentEventMasterAsync(
                Request.UpdateCurrentEventMasterRequest request
        ) =>
            new UpdateCurrentEventMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class UpdateCurrentEventMasterFromGitHubTask : Gs2RestSessionTask<UpdateCurrentEventMasterFromGitHubRequest, UpdateCurrentEventMasterFromGitHubResult>
        {
            public UpdateCurrentEventMasterFromGitHubTask(IGs2Session session, RestSessionRequestFactory factory, UpdateCurrentEventMasterFromGitHubRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(UpdateCurrentEventMasterFromGitHubRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "schedule")
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
		public IEnumerator UpdateCurrentEventMasterFromGitHub(
                Request.UpdateCurrentEventMasterFromGitHubRequest request,
                UnityAction<AsyncResult<Result.UpdateCurrentEventMasterFromGitHubResult>> callback
        ) =>
            new UpdateCurrentEventMasterFromGitHubTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.UpdateCurrentEventMasterFromGitHubResult> UpdateCurrentEventMasterFromGitHubFuture(
                Request.UpdateCurrentEventMasterFromGitHubRequest request
        ) =>
            new UpdateCurrentEventMasterFromGitHubTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.UpdateCurrentEventMasterFromGitHubResult> UpdateCurrentEventMasterFromGitHubAsync(
                Request.UpdateCurrentEventMasterFromGitHubRequest request
        ) =>
            new UpdateCurrentEventMasterFromGitHubTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.UpdateCurrentEventMasterFromGitHubResult>();
    #else
		public UpdateCurrentEventMasterFromGitHubTask UpdateCurrentEventMasterFromGitHubAsync(
                Request.UpdateCurrentEventMasterFromGitHubRequest request
        )
		{
			return new UpdateCurrentEventMasterFromGitHubTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.UpdateCurrentEventMasterFromGitHubResult> UpdateCurrentEventMasterFromGitHubAsync(
                Request.UpdateCurrentEventMasterFromGitHubRequest request
        ) =>
            new UpdateCurrentEventMasterFromGitHubTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif
	}
}