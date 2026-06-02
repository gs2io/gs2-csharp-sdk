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
using Gs2.Gs2Ranking2.Request;
using Gs2.Gs2Ranking2.Result;
using Gs2.Util.LitJson;

namespace Gs2.Gs2Ranking2
{
	public class Gs2Ranking2RestClient : AbstractGs2Client
	{
#if UNITY_2017_1_OR_NEWER
		private readonly CertificateHandler _certificateHandler;
#endif

		public static string Endpoint = "ranking2";

        protected Gs2RestSession Gs2RestSession => (Gs2RestSession) Gs2Session;

		public Gs2Ranking2RestClient(Gs2RestSession Gs2RestSession) : base(Gs2RestSession)
		{

		}

#if UNITY_2017_1_OR_NEWER
		public Gs2Ranking2RestClient(Gs2RestSession gs2RestSession, CertificateHandler certificateHandler) : base(gs2RestSession)
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
                    .Replace("{service}", "ranking2")
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
                    .Replace("{service}", "ranking2")
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
                    .Replace("{service}", "ranking2")
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
                    .Replace("{service}", "ranking2")
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
                    .Replace("{service}", "ranking2")
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
                    .Replace("{service}", "ranking2")
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
                    .Replace("{service}", "ranking2")
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
                    .Replace("{service}", "ranking2")
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
                    .Replace("{service}", "ranking2")
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
                    .Replace("{service}", "ranking2")
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
                    .Replace("{service}", "ranking2")
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
                    .Replace("{service}", "ranking2")
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
                    .Replace("{service}", "ranking2")
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
                    .Replace("{service}", "ranking2")
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


        public class DescribeGlobalRankingModelsTask : Gs2RestSessionTask<DescribeGlobalRankingModelsRequest, DescribeGlobalRankingModelsResult>
        {
            public DescribeGlobalRankingModelsTask(IGs2Session session, RestSessionRequestFactory factory, DescribeGlobalRankingModelsRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DescribeGlobalRankingModelsRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "ranking2")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/model/global";

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
		public IEnumerator DescribeGlobalRankingModels(
                Request.DescribeGlobalRankingModelsRequest request,
                UnityAction<AsyncResult<Result.DescribeGlobalRankingModelsResult>> callback
        ) =>
            new DescribeGlobalRankingModelsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.DescribeGlobalRankingModelsResult> DescribeGlobalRankingModelsFuture(
                Request.DescribeGlobalRankingModelsRequest request
        ) =>
            new DescribeGlobalRankingModelsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.DescribeGlobalRankingModelsResult> DescribeGlobalRankingModelsAsync(
                Request.DescribeGlobalRankingModelsRequest request
        ) =>
            new DescribeGlobalRankingModelsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.DescribeGlobalRankingModelsResult>();
    #else
		public DescribeGlobalRankingModelsTask DescribeGlobalRankingModelsAsync(
                Request.DescribeGlobalRankingModelsRequest request
        )
		{
			return new DescribeGlobalRankingModelsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.DescribeGlobalRankingModelsResult> DescribeGlobalRankingModelsAsync(
                Request.DescribeGlobalRankingModelsRequest request
        ) =>
            new DescribeGlobalRankingModelsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class GetGlobalRankingModelTask : Gs2RestSessionTask<GetGlobalRankingModelRequest, GetGlobalRankingModelResult>
        {
            public GetGlobalRankingModelTask(IGs2Session session, RestSessionRequestFactory factory, GetGlobalRankingModelRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(GetGlobalRankingModelRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "ranking2")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/model/global/{rankingName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{rankingName}", !string.IsNullOrEmpty(request.RankingName) ? request.RankingName.ToString() : "null");

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
		public IEnumerator GetGlobalRankingModel(
                Request.GetGlobalRankingModelRequest request,
                UnityAction<AsyncResult<Result.GetGlobalRankingModelResult>> callback
        ) =>
            new GetGlobalRankingModelTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.GetGlobalRankingModelResult> GetGlobalRankingModelFuture(
                Request.GetGlobalRankingModelRequest request
        ) =>
            new GetGlobalRankingModelTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.GetGlobalRankingModelResult> GetGlobalRankingModelAsync(
                Request.GetGlobalRankingModelRequest request
        ) =>
            new GetGlobalRankingModelTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.GetGlobalRankingModelResult>();
    #else
		public GetGlobalRankingModelTask GetGlobalRankingModelAsync(
                Request.GetGlobalRankingModelRequest request
        )
		{
			return new GetGlobalRankingModelTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.GetGlobalRankingModelResult> GetGlobalRankingModelAsync(
                Request.GetGlobalRankingModelRequest request
        ) =>
            new GetGlobalRankingModelTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class DescribeGlobalRankingModelMastersTask : Gs2RestSessionTask<DescribeGlobalRankingModelMastersRequest, DescribeGlobalRankingModelMastersResult>
        {
            public DescribeGlobalRankingModelMastersTask(IGs2Session session, RestSessionRequestFactory factory, DescribeGlobalRankingModelMastersRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DescribeGlobalRankingModelMastersRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "ranking2")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/master/global";

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
		public IEnumerator DescribeGlobalRankingModelMasters(
                Request.DescribeGlobalRankingModelMastersRequest request,
                UnityAction<AsyncResult<Result.DescribeGlobalRankingModelMastersResult>> callback
        ) =>
            new DescribeGlobalRankingModelMastersTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.DescribeGlobalRankingModelMastersResult> DescribeGlobalRankingModelMastersFuture(
                Request.DescribeGlobalRankingModelMastersRequest request
        ) =>
            new DescribeGlobalRankingModelMastersTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.DescribeGlobalRankingModelMastersResult> DescribeGlobalRankingModelMastersAsync(
                Request.DescribeGlobalRankingModelMastersRequest request
        ) =>
            new DescribeGlobalRankingModelMastersTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.DescribeGlobalRankingModelMastersResult>();
    #else
		public DescribeGlobalRankingModelMastersTask DescribeGlobalRankingModelMastersAsync(
                Request.DescribeGlobalRankingModelMastersRequest request
        )
		{
			return new DescribeGlobalRankingModelMastersTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.DescribeGlobalRankingModelMastersResult> DescribeGlobalRankingModelMastersAsync(
                Request.DescribeGlobalRankingModelMastersRequest request
        ) =>
            new DescribeGlobalRankingModelMastersTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class CreateGlobalRankingModelMasterTask : Gs2RestSessionTask<CreateGlobalRankingModelMasterRequest, CreateGlobalRankingModelMasterResult>
        {
            public CreateGlobalRankingModelMasterTask(IGs2Session session, RestSessionRequestFactory factory, CreateGlobalRankingModelMasterRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(CreateGlobalRankingModelMasterRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "ranking2")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/master/global";

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
                if (request.MinimumValue != null)
                {
                    jsonWriter.WritePropertyName("minimumValue");
                    jsonWriter.Write(request.MinimumValue.ToString());
                }
                if (request.MaximumValue != null)
                {
                    jsonWriter.WritePropertyName("maximumValue");
                    jsonWriter.Write(request.MaximumValue.ToString());
                }
                if (request.Sum != null)
                {
                    jsonWriter.WritePropertyName("sum");
                    jsonWriter.Write(request.Sum.ToString());
                }
                if (request.OrderDirection != null)
                {
                    jsonWriter.WritePropertyName("orderDirection");
                    jsonWriter.Write(request.OrderDirection);
                }
                if (request.RankingRewards != null)
                {
                    jsonWriter.WritePropertyName("rankingRewards");
                    jsonWriter.WriteArrayStart();
                    foreach(var item in request.RankingRewards)
                    {
                        item.WriteJson(jsonWriter);
                    }
                    jsonWriter.WriteArrayEnd();
                }
                if (request.RewardCalculationIndex != null)
                {
                    jsonWriter.WritePropertyName("rewardCalculationIndex");
                    jsonWriter.Write(request.RewardCalculationIndex);
                }
                if (request.EntryPeriodEventId != null)
                {
                    jsonWriter.WritePropertyName("entryPeriodEventId");
                    jsonWriter.Write(request.EntryPeriodEventId);
                }
                if (request.AccessPeriodEventId != null)
                {
                    jsonWriter.WritePropertyName("accessPeriodEventId");
                    jsonWriter.Write(request.AccessPeriodEventId);
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
		public IEnumerator CreateGlobalRankingModelMaster(
                Request.CreateGlobalRankingModelMasterRequest request,
                UnityAction<AsyncResult<Result.CreateGlobalRankingModelMasterResult>> callback
        ) =>
            new CreateGlobalRankingModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.CreateGlobalRankingModelMasterResult> CreateGlobalRankingModelMasterFuture(
                Request.CreateGlobalRankingModelMasterRequest request
        ) =>
            new CreateGlobalRankingModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.CreateGlobalRankingModelMasterResult> CreateGlobalRankingModelMasterAsync(
                Request.CreateGlobalRankingModelMasterRequest request
        ) =>
            new CreateGlobalRankingModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.CreateGlobalRankingModelMasterResult>();
    #else
		public CreateGlobalRankingModelMasterTask CreateGlobalRankingModelMasterAsync(
                Request.CreateGlobalRankingModelMasterRequest request
        )
		{
			return new CreateGlobalRankingModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.CreateGlobalRankingModelMasterResult> CreateGlobalRankingModelMasterAsync(
                Request.CreateGlobalRankingModelMasterRequest request
        ) =>
            new CreateGlobalRankingModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class GetGlobalRankingModelMasterTask : Gs2RestSessionTask<GetGlobalRankingModelMasterRequest, GetGlobalRankingModelMasterResult>
        {
            public GetGlobalRankingModelMasterTask(IGs2Session session, RestSessionRequestFactory factory, GetGlobalRankingModelMasterRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(GetGlobalRankingModelMasterRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "ranking2")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/master/global/{rankingName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{rankingName}", !string.IsNullOrEmpty(request.RankingName) ? request.RankingName.ToString() : "null");

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
		public IEnumerator GetGlobalRankingModelMaster(
                Request.GetGlobalRankingModelMasterRequest request,
                UnityAction<AsyncResult<Result.GetGlobalRankingModelMasterResult>> callback
        ) =>
            new GetGlobalRankingModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.GetGlobalRankingModelMasterResult> GetGlobalRankingModelMasterFuture(
                Request.GetGlobalRankingModelMasterRequest request
        ) =>
            new GetGlobalRankingModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.GetGlobalRankingModelMasterResult> GetGlobalRankingModelMasterAsync(
                Request.GetGlobalRankingModelMasterRequest request
        ) =>
            new GetGlobalRankingModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.GetGlobalRankingModelMasterResult>();
    #else
		public GetGlobalRankingModelMasterTask GetGlobalRankingModelMasterAsync(
                Request.GetGlobalRankingModelMasterRequest request
        )
		{
			return new GetGlobalRankingModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.GetGlobalRankingModelMasterResult> GetGlobalRankingModelMasterAsync(
                Request.GetGlobalRankingModelMasterRequest request
        ) =>
            new GetGlobalRankingModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class UpdateGlobalRankingModelMasterTask : Gs2RestSessionTask<UpdateGlobalRankingModelMasterRequest, UpdateGlobalRankingModelMasterResult>
        {
            public UpdateGlobalRankingModelMasterTask(IGs2Session session, RestSessionRequestFactory factory, UpdateGlobalRankingModelMasterRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(UpdateGlobalRankingModelMasterRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "ranking2")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/master/global/{rankingName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{rankingName}", !string.IsNullOrEmpty(request.RankingName) ? request.RankingName.ToString() : "null");

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
                if (request.MinimumValue != null)
                {
                    jsonWriter.WritePropertyName("minimumValue");
                    jsonWriter.Write(request.MinimumValue.ToString());
                }
                if (request.MaximumValue != null)
                {
                    jsonWriter.WritePropertyName("maximumValue");
                    jsonWriter.Write(request.MaximumValue.ToString());
                }
                if (request.Sum != null)
                {
                    jsonWriter.WritePropertyName("sum");
                    jsonWriter.Write(request.Sum.ToString());
                }
                if (request.OrderDirection != null)
                {
                    jsonWriter.WritePropertyName("orderDirection");
                    jsonWriter.Write(request.OrderDirection);
                }
                if (request.RankingRewards != null)
                {
                    jsonWriter.WritePropertyName("rankingRewards");
                    jsonWriter.WriteArrayStart();
                    foreach(var item in request.RankingRewards)
                    {
                        item.WriteJson(jsonWriter);
                    }
                    jsonWriter.WriteArrayEnd();
                }
                if (request.RewardCalculationIndex != null)
                {
                    jsonWriter.WritePropertyName("rewardCalculationIndex");
                    jsonWriter.Write(request.RewardCalculationIndex);
                }
                if (request.EntryPeriodEventId != null)
                {
                    jsonWriter.WritePropertyName("entryPeriodEventId");
                    jsonWriter.Write(request.EntryPeriodEventId);
                }
                if (request.AccessPeriodEventId != null)
                {
                    jsonWriter.WritePropertyName("accessPeriodEventId");
                    jsonWriter.Write(request.AccessPeriodEventId);
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
		public IEnumerator UpdateGlobalRankingModelMaster(
                Request.UpdateGlobalRankingModelMasterRequest request,
                UnityAction<AsyncResult<Result.UpdateGlobalRankingModelMasterResult>> callback
        ) =>
            new UpdateGlobalRankingModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.UpdateGlobalRankingModelMasterResult> UpdateGlobalRankingModelMasterFuture(
                Request.UpdateGlobalRankingModelMasterRequest request
        ) =>
            new UpdateGlobalRankingModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.UpdateGlobalRankingModelMasterResult> UpdateGlobalRankingModelMasterAsync(
                Request.UpdateGlobalRankingModelMasterRequest request
        ) =>
            new UpdateGlobalRankingModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.UpdateGlobalRankingModelMasterResult>();
    #else
		public UpdateGlobalRankingModelMasterTask UpdateGlobalRankingModelMasterAsync(
                Request.UpdateGlobalRankingModelMasterRequest request
        )
		{
			return new UpdateGlobalRankingModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.UpdateGlobalRankingModelMasterResult> UpdateGlobalRankingModelMasterAsync(
                Request.UpdateGlobalRankingModelMasterRequest request
        ) =>
            new UpdateGlobalRankingModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class DeleteGlobalRankingModelMasterTask : Gs2RestSessionTask<DeleteGlobalRankingModelMasterRequest, DeleteGlobalRankingModelMasterResult>
        {
            public DeleteGlobalRankingModelMasterTask(IGs2Session session, RestSessionRequestFactory factory, DeleteGlobalRankingModelMasterRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DeleteGlobalRankingModelMasterRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "ranking2")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/master/global/{rankingName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{rankingName}", !string.IsNullOrEmpty(request.RankingName) ? request.RankingName.ToString() : "null");

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
		public IEnumerator DeleteGlobalRankingModelMaster(
                Request.DeleteGlobalRankingModelMasterRequest request,
                UnityAction<AsyncResult<Result.DeleteGlobalRankingModelMasterResult>> callback
        ) =>
            new DeleteGlobalRankingModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.DeleteGlobalRankingModelMasterResult> DeleteGlobalRankingModelMasterFuture(
                Request.DeleteGlobalRankingModelMasterRequest request
        ) =>
            new DeleteGlobalRankingModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.DeleteGlobalRankingModelMasterResult> DeleteGlobalRankingModelMasterAsync(
                Request.DeleteGlobalRankingModelMasterRequest request
        ) =>
            new DeleteGlobalRankingModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.DeleteGlobalRankingModelMasterResult>();
    #else
		public DeleteGlobalRankingModelMasterTask DeleteGlobalRankingModelMasterAsync(
                Request.DeleteGlobalRankingModelMasterRequest request
        )
		{
			return new DeleteGlobalRankingModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.DeleteGlobalRankingModelMasterResult> DeleteGlobalRankingModelMasterAsync(
                Request.DeleteGlobalRankingModelMasterRequest request
        ) =>
            new DeleteGlobalRankingModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class DescribeGlobalRankingScoresTask : Gs2RestSessionTask<DescribeGlobalRankingScoresRequest, DescribeGlobalRankingScoresResult>
        {
            public DescribeGlobalRankingScoresTask(IGs2Session session, RestSessionRequestFactory factory, DescribeGlobalRankingScoresRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DescribeGlobalRankingScoresRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "ranking2")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/me/score/global";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");

                var sessionRequest = Factory.Get(url);
                if (request.ContextStack != null)
                {
                    sessionRequest.AddQueryString("contextStack", request.ContextStack);
                }
                if (request.RankingName != null) {
                    sessionRequest.AddQueryString("rankingName", $"{request.RankingName}");
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
		public IEnumerator DescribeGlobalRankingScores(
                Request.DescribeGlobalRankingScoresRequest request,
                UnityAction<AsyncResult<Result.DescribeGlobalRankingScoresResult>> callback
        ) =>
            new DescribeGlobalRankingScoresTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.DescribeGlobalRankingScoresResult> DescribeGlobalRankingScoresFuture(
                Request.DescribeGlobalRankingScoresRequest request
        ) =>
            new DescribeGlobalRankingScoresTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.DescribeGlobalRankingScoresResult> DescribeGlobalRankingScoresAsync(
                Request.DescribeGlobalRankingScoresRequest request
        ) =>
            new DescribeGlobalRankingScoresTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.DescribeGlobalRankingScoresResult>();
    #else
		public DescribeGlobalRankingScoresTask DescribeGlobalRankingScoresAsync(
                Request.DescribeGlobalRankingScoresRequest request
        )
		{
			return new DescribeGlobalRankingScoresTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.DescribeGlobalRankingScoresResult> DescribeGlobalRankingScoresAsync(
                Request.DescribeGlobalRankingScoresRequest request
        ) =>
            new DescribeGlobalRankingScoresTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class DescribeGlobalRankingScoresByUserIdTask : Gs2RestSessionTask<DescribeGlobalRankingScoresByUserIdRequest, DescribeGlobalRankingScoresByUserIdResult>
        {
            public DescribeGlobalRankingScoresByUserIdTask(IGs2Session session, RestSessionRequestFactory factory, DescribeGlobalRankingScoresByUserIdRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DescribeGlobalRankingScoresByUserIdRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "ranking2")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/{userId}/score/global";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{userId}", !string.IsNullOrEmpty(request.UserId) ? request.UserId.ToString() : "null");

                var sessionRequest = Factory.Get(url);
                if (request.ContextStack != null)
                {
                    sessionRequest.AddQueryString("contextStack", request.ContextStack);
                }
                if (request.RankingName != null) {
                    sessionRequest.AddQueryString("rankingName", $"{request.RankingName}");
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
		public IEnumerator DescribeGlobalRankingScoresByUserId(
                Request.DescribeGlobalRankingScoresByUserIdRequest request,
                UnityAction<AsyncResult<Result.DescribeGlobalRankingScoresByUserIdResult>> callback
        ) =>
            new DescribeGlobalRankingScoresByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.DescribeGlobalRankingScoresByUserIdResult> DescribeGlobalRankingScoresByUserIdFuture(
                Request.DescribeGlobalRankingScoresByUserIdRequest request
        ) =>
            new DescribeGlobalRankingScoresByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.DescribeGlobalRankingScoresByUserIdResult> DescribeGlobalRankingScoresByUserIdAsync(
                Request.DescribeGlobalRankingScoresByUserIdRequest request
        ) =>
            new DescribeGlobalRankingScoresByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.DescribeGlobalRankingScoresByUserIdResult>();
    #else
		public DescribeGlobalRankingScoresByUserIdTask DescribeGlobalRankingScoresByUserIdAsync(
                Request.DescribeGlobalRankingScoresByUserIdRequest request
        )
		{
			return new DescribeGlobalRankingScoresByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.DescribeGlobalRankingScoresByUserIdResult> DescribeGlobalRankingScoresByUserIdAsync(
                Request.DescribeGlobalRankingScoresByUserIdRequest request
        ) =>
            new DescribeGlobalRankingScoresByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class PutGlobalRankingScoreTask : Gs2RestSessionTask<PutGlobalRankingScoreRequest, PutGlobalRankingScoreResult>
        {
            public PutGlobalRankingScoreTask(IGs2Session session, RestSessionRequestFactory factory, PutGlobalRankingScoreRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(PutGlobalRankingScoreRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "ranking2")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/me/score/global/{rankingName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{rankingName}", !string.IsNullOrEmpty(request.RankingName) ? request.RankingName.ToString() : "null");

                var sessionRequest = Factory.Put(url);

                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);
                jsonWriter.WriteObjectStart();
                if (request.Score != null)
                {
                    jsonWriter.WritePropertyName("score");
                    jsonWriter.Write(request.Score.ToString());
                }
                if (request.Metadata != null)
                {
                    jsonWriter.WritePropertyName("metadata");
                    jsonWriter.Write(request.Metadata);
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
		public IEnumerator PutGlobalRankingScore(
                Request.PutGlobalRankingScoreRequest request,
                UnityAction<AsyncResult<Result.PutGlobalRankingScoreResult>> callback
        ) =>
            new PutGlobalRankingScoreTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.PutGlobalRankingScoreResult> PutGlobalRankingScoreFuture(
                Request.PutGlobalRankingScoreRequest request
        ) =>
            new PutGlobalRankingScoreTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.PutGlobalRankingScoreResult> PutGlobalRankingScoreAsync(
                Request.PutGlobalRankingScoreRequest request
        ) =>
            new PutGlobalRankingScoreTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.PutGlobalRankingScoreResult>();
    #else
		public PutGlobalRankingScoreTask PutGlobalRankingScoreAsync(
                Request.PutGlobalRankingScoreRequest request
        )
		{
			return new PutGlobalRankingScoreTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.PutGlobalRankingScoreResult> PutGlobalRankingScoreAsync(
                Request.PutGlobalRankingScoreRequest request
        ) =>
            new PutGlobalRankingScoreTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class PutGlobalRankingScoreByUserIdTask : Gs2RestSessionTask<PutGlobalRankingScoreByUserIdRequest, PutGlobalRankingScoreByUserIdResult>
        {
            public PutGlobalRankingScoreByUserIdTask(IGs2Session session, RestSessionRequestFactory factory, PutGlobalRankingScoreByUserIdRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(PutGlobalRankingScoreByUserIdRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "ranking2")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/{userId}/score/global/{rankingName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{rankingName}", !string.IsNullOrEmpty(request.RankingName) ? request.RankingName.ToString() : "null");
                url = url.Replace("{userId}", !string.IsNullOrEmpty(request.UserId) ? request.UserId.ToString() : "null");

                var sessionRequest = Factory.Put(url);

                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);
                jsonWriter.WriteObjectStart();
                if (request.Score != null)
                {
                    jsonWriter.WritePropertyName("score");
                    jsonWriter.Write(request.Score.ToString());
                }
                if (request.Metadata != null)
                {
                    jsonWriter.WritePropertyName("metadata");
                    jsonWriter.Write(request.Metadata);
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
		public IEnumerator PutGlobalRankingScoreByUserId(
                Request.PutGlobalRankingScoreByUserIdRequest request,
                UnityAction<AsyncResult<Result.PutGlobalRankingScoreByUserIdResult>> callback
        ) =>
            new PutGlobalRankingScoreByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.PutGlobalRankingScoreByUserIdResult> PutGlobalRankingScoreByUserIdFuture(
                Request.PutGlobalRankingScoreByUserIdRequest request
        ) =>
            new PutGlobalRankingScoreByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.PutGlobalRankingScoreByUserIdResult> PutGlobalRankingScoreByUserIdAsync(
                Request.PutGlobalRankingScoreByUserIdRequest request
        ) =>
            new PutGlobalRankingScoreByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.PutGlobalRankingScoreByUserIdResult>();
    #else
		public PutGlobalRankingScoreByUserIdTask PutGlobalRankingScoreByUserIdAsync(
                Request.PutGlobalRankingScoreByUserIdRequest request
        )
		{
			return new PutGlobalRankingScoreByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.PutGlobalRankingScoreByUserIdResult> PutGlobalRankingScoreByUserIdAsync(
                Request.PutGlobalRankingScoreByUserIdRequest request
        ) =>
            new PutGlobalRankingScoreByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class GetGlobalRankingScoreTask : Gs2RestSessionTask<GetGlobalRankingScoreRequest, GetGlobalRankingScoreResult>
        {
            public GetGlobalRankingScoreTask(IGs2Session session, RestSessionRequestFactory factory, GetGlobalRankingScoreRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(GetGlobalRankingScoreRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "ranking2")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/me/score/global/{rankingName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{rankingName}", !string.IsNullOrEmpty(request.RankingName) ? request.RankingName.ToString() : "null");

                var sessionRequest = Factory.Get(url);
                if (request.ContextStack != null)
                {
                    sessionRequest.AddQueryString("contextStack", request.ContextStack);
                }
                if (request.Season != null) {
                    sessionRequest.AddQueryString("season", $"{request.Season}");
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
		public IEnumerator GetGlobalRankingScore(
                Request.GetGlobalRankingScoreRequest request,
                UnityAction<AsyncResult<Result.GetGlobalRankingScoreResult>> callback
        ) =>
            new GetGlobalRankingScoreTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.GetGlobalRankingScoreResult> GetGlobalRankingScoreFuture(
                Request.GetGlobalRankingScoreRequest request
        ) =>
            new GetGlobalRankingScoreTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.GetGlobalRankingScoreResult> GetGlobalRankingScoreAsync(
                Request.GetGlobalRankingScoreRequest request
        ) =>
            new GetGlobalRankingScoreTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.GetGlobalRankingScoreResult>();
    #else
		public GetGlobalRankingScoreTask GetGlobalRankingScoreAsync(
                Request.GetGlobalRankingScoreRequest request
        )
		{
			return new GetGlobalRankingScoreTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.GetGlobalRankingScoreResult> GetGlobalRankingScoreAsync(
                Request.GetGlobalRankingScoreRequest request
        ) =>
            new GetGlobalRankingScoreTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class GetGlobalRankingScoreByUserIdTask : Gs2RestSessionTask<GetGlobalRankingScoreByUserIdRequest, GetGlobalRankingScoreByUserIdResult>
        {
            public GetGlobalRankingScoreByUserIdTask(IGs2Session session, RestSessionRequestFactory factory, GetGlobalRankingScoreByUserIdRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(GetGlobalRankingScoreByUserIdRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "ranking2")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/{userId}/score/global/{rankingName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{rankingName}", !string.IsNullOrEmpty(request.RankingName) ? request.RankingName.ToString() : "null");
                url = url.Replace("{userId}", !string.IsNullOrEmpty(request.UserId) ? request.UserId.ToString() : "null");

                var sessionRequest = Factory.Get(url);
                if (request.ContextStack != null)
                {
                    sessionRequest.AddQueryString("contextStack", request.ContextStack);
                }
                if (request.Season != null) {
                    sessionRequest.AddQueryString("season", $"{request.Season}");
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
		public IEnumerator GetGlobalRankingScoreByUserId(
                Request.GetGlobalRankingScoreByUserIdRequest request,
                UnityAction<AsyncResult<Result.GetGlobalRankingScoreByUserIdResult>> callback
        ) =>
            new GetGlobalRankingScoreByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.GetGlobalRankingScoreByUserIdResult> GetGlobalRankingScoreByUserIdFuture(
                Request.GetGlobalRankingScoreByUserIdRequest request
        ) =>
            new GetGlobalRankingScoreByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.GetGlobalRankingScoreByUserIdResult> GetGlobalRankingScoreByUserIdAsync(
                Request.GetGlobalRankingScoreByUserIdRequest request
        ) =>
            new GetGlobalRankingScoreByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.GetGlobalRankingScoreByUserIdResult>();
    #else
		public GetGlobalRankingScoreByUserIdTask GetGlobalRankingScoreByUserIdAsync(
                Request.GetGlobalRankingScoreByUserIdRequest request
        )
		{
			return new GetGlobalRankingScoreByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.GetGlobalRankingScoreByUserIdResult> GetGlobalRankingScoreByUserIdAsync(
                Request.GetGlobalRankingScoreByUserIdRequest request
        ) =>
            new GetGlobalRankingScoreByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class DeleteGlobalRankingScoreByUserIdTask : Gs2RestSessionTask<DeleteGlobalRankingScoreByUserIdRequest, DeleteGlobalRankingScoreByUserIdResult>
        {
            public DeleteGlobalRankingScoreByUserIdTask(IGs2Session session, RestSessionRequestFactory factory, DeleteGlobalRankingScoreByUserIdRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DeleteGlobalRankingScoreByUserIdRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "ranking2")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/{userId}/score/global/{rankingName}/{season}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{rankingName}", !string.IsNullOrEmpty(request.RankingName) ? request.RankingName.ToString() : "null");
                url = url.Replace("{userId}", !string.IsNullOrEmpty(request.UserId) ? request.UserId.ToString() : "null");

                var sessionRequest = Factory.Delete(url);
                if (request.ContextStack != null)
                {
                    sessionRequest.AddQueryString("contextStack", request.ContextStack);
                }
                if (request.Season != null) {
                    sessionRequest.AddQueryString("season", $"{request.Season}");
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
		public IEnumerator DeleteGlobalRankingScoreByUserId(
                Request.DeleteGlobalRankingScoreByUserIdRequest request,
                UnityAction<AsyncResult<Result.DeleteGlobalRankingScoreByUserIdResult>> callback
        ) =>
            new DeleteGlobalRankingScoreByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.DeleteGlobalRankingScoreByUserIdResult> DeleteGlobalRankingScoreByUserIdFuture(
                Request.DeleteGlobalRankingScoreByUserIdRequest request
        ) =>
            new DeleteGlobalRankingScoreByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.DeleteGlobalRankingScoreByUserIdResult> DeleteGlobalRankingScoreByUserIdAsync(
                Request.DeleteGlobalRankingScoreByUserIdRequest request
        ) =>
            new DeleteGlobalRankingScoreByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.DeleteGlobalRankingScoreByUserIdResult>();
    #else
		public DeleteGlobalRankingScoreByUserIdTask DeleteGlobalRankingScoreByUserIdAsync(
                Request.DeleteGlobalRankingScoreByUserIdRequest request
        )
		{
			return new DeleteGlobalRankingScoreByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.DeleteGlobalRankingScoreByUserIdResult> DeleteGlobalRankingScoreByUserIdAsync(
                Request.DeleteGlobalRankingScoreByUserIdRequest request
        ) =>
            new DeleteGlobalRankingScoreByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class VerifyGlobalRankingScoreTask : Gs2RestSessionTask<VerifyGlobalRankingScoreRequest, VerifyGlobalRankingScoreResult>
        {
            public VerifyGlobalRankingScoreTask(IGs2Session session, RestSessionRequestFactory factory, VerifyGlobalRankingScoreRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(VerifyGlobalRankingScoreRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "ranking2")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/{userId}/score/global/{rankingName}/verify/{verifyType}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{rankingName}", !string.IsNullOrEmpty(request.RankingName) ? request.RankingName.ToString() : "null");
                url = url.Replace("{verifyType}", !string.IsNullOrEmpty(request.VerifyType) ? request.VerifyType.ToString() : "null");

                var sessionRequest = Factory.Post(url);

                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);
                jsonWriter.WriteObjectStart();
                if (request.Season != null)
                {
                    jsonWriter.WritePropertyName("season");
                    jsonWriter.Write(request.Season.ToString());
                }
                if (request.Score != null)
                {
                    jsonWriter.WritePropertyName("score");
                    jsonWriter.Write(request.Score.ToString());
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
		public IEnumerator VerifyGlobalRankingScore(
                Request.VerifyGlobalRankingScoreRequest request,
                UnityAction<AsyncResult<Result.VerifyGlobalRankingScoreResult>> callback
        ) =>
            new VerifyGlobalRankingScoreTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.VerifyGlobalRankingScoreResult> VerifyGlobalRankingScoreFuture(
                Request.VerifyGlobalRankingScoreRequest request
        ) =>
            new VerifyGlobalRankingScoreTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.VerifyGlobalRankingScoreResult> VerifyGlobalRankingScoreAsync(
                Request.VerifyGlobalRankingScoreRequest request
        ) =>
            new VerifyGlobalRankingScoreTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.VerifyGlobalRankingScoreResult>();
    #else
		public VerifyGlobalRankingScoreTask VerifyGlobalRankingScoreAsync(
                Request.VerifyGlobalRankingScoreRequest request
        )
		{
			return new VerifyGlobalRankingScoreTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.VerifyGlobalRankingScoreResult> VerifyGlobalRankingScoreAsync(
                Request.VerifyGlobalRankingScoreRequest request
        ) =>
            new VerifyGlobalRankingScoreTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class VerifyGlobalRankingScoreByUserIdTask : Gs2RestSessionTask<VerifyGlobalRankingScoreByUserIdRequest, VerifyGlobalRankingScoreByUserIdResult>
        {
            public VerifyGlobalRankingScoreByUserIdTask(IGs2Session session, RestSessionRequestFactory factory, VerifyGlobalRankingScoreByUserIdRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(VerifyGlobalRankingScoreByUserIdRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "ranking2")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/{userId}/score/global/{rankingName}/verify/{verifyType}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{userId}", !string.IsNullOrEmpty(request.UserId) ? request.UserId.ToString() : "null");
                url = url.Replace("{rankingName}", !string.IsNullOrEmpty(request.RankingName) ? request.RankingName.ToString() : "null");
                url = url.Replace("{verifyType}", !string.IsNullOrEmpty(request.VerifyType) ? request.VerifyType.ToString() : "null");

                var sessionRequest = Factory.Post(url);

                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);
                jsonWriter.WriteObjectStart();
                if (request.Season != null)
                {
                    jsonWriter.WritePropertyName("season");
                    jsonWriter.Write(request.Season.ToString());
                }
                if (request.Score != null)
                {
                    jsonWriter.WritePropertyName("score");
                    jsonWriter.Write(request.Score.ToString());
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
		public IEnumerator VerifyGlobalRankingScoreByUserId(
                Request.VerifyGlobalRankingScoreByUserIdRequest request,
                UnityAction<AsyncResult<Result.VerifyGlobalRankingScoreByUserIdResult>> callback
        ) =>
            new VerifyGlobalRankingScoreByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.VerifyGlobalRankingScoreByUserIdResult> VerifyGlobalRankingScoreByUserIdFuture(
                Request.VerifyGlobalRankingScoreByUserIdRequest request
        ) =>
            new VerifyGlobalRankingScoreByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.VerifyGlobalRankingScoreByUserIdResult> VerifyGlobalRankingScoreByUserIdAsync(
                Request.VerifyGlobalRankingScoreByUserIdRequest request
        ) =>
            new VerifyGlobalRankingScoreByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.VerifyGlobalRankingScoreByUserIdResult>();
    #else
		public VerifyGlobalRankingScoreByUserIdTask VerifyGlobalRankingScoreByUserIdAsync(
                Request.VerifyGlobalRankingScoreByUserIdRequest request
        )
		{
			return new VerifyGlobalRankingScoreByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.VerifyGlobalRankingScoreByUserIdResult> VerifyGlobalRankingScoreByUserIdAsync(
                Request.VerifyGlobalRankingScoreByUserIdRequest request
        ) =>
            new VerifyGlobalRankingScoreByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class VerifyGlobalRankingScoreByStampTaskTask : Gs2RestSessionTask<VerifyGlobalRankingScoreByStampTaskRequest, VerifyGlobalRankingScoreByStampTaskResult>
        {
            public VerifyGlobalRankingScoreByStampTaskTask(IGs2Session session, RestSessionRequestFactory factory, VerifyGlobalRankingScoreByStampTaskRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(VerifyGlobalRankingScoreByStampTaskRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "ranking2")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/stamp/global/score/verify";

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
		public IEnumerator VerifyGlobalRankingScoreByStampTask(
                Request.VerifyGlobalRankingScoreByStampTaskRequest request,
                UnityAction<AsyncResult<Result.VerifyGlobalRankingScoreByStampTaskResult>> callback
        ) =>
            new VerifyGlobalRankingScoreByStampTaskTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.VerifyGlobalRankingScoreByStampTaskResult> VerifyGlobalRankingScoreByStampTaskFuture(
                Request.VerifyGlobalRankingScoreByStampTaskRequest request
        ) =>
            new VerifyGlobalRankingScoreByStampTaskTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.VerifyGlobalRankingScoreByStampTaskResult> VerifyGlobalRankingScoreByStampTaskAsync(
                Request.VerifyGlobalRankingScoreByStampTaskRequest request
        ) =>
            new VerifyGlobalRankingScoreByStampTaskTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.VerifyGlobalRankingScoreByStampTaskResult>();
    #else
		public VerifyGlobalRankingScoreByStampTaskTask VerifyGlobalRankingScoreByStampTaskAsync(
                Request.VerifyGlobalRankingScoreByStampTaskRequest request
        )
		{
			return new VerifyGlobalRankingScoreByStampTaskTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.VerifyGlobalRankingScoreByStampTaskResult> VerifyGlobalRankingScoreByStampTaskAsync(
                Request.VerifyGlobalRankingScoreByStampTaskRequest request
        ) =>
            new VerifyGlobalRankingScoreByStampTaskTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class DescribeGlobalRankingReceivedRewardsTask : Gs2RestSessionTask<DescribeGlobalRankingReceivedRewardsRequest, DescribeGlobalRankingReceivedRewardsResult>
        {
            public DescribeGlobalRankingReceivedRewardsTask(IGs2Session session, RestSessionRequestFactory factory, DescribeGlobalRankingReceivedRewardsRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DescribeGlobalRankingReceivedRewardsRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "ranking2")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/me/global/reward/received";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");

                var sessionRequest = Factory.Get(url);
                if (request.ContextStack != null)
                {
                    sessionRequest.AddQueryString("contextStack", request.ContextStack);
                }
                if (request.RankingName != null) {
                    sessionRequest.AddQueryString("rankingName", $"{request.RankingName}");
                }
                if (request.Season != null) {
                    sessionRequest.AddQueryString("season", $"{request.Season}");
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
		public IEnumerator DescribeGlobalRankingReceivedRewards(
                Request.DescribeGlobalRankingReceivedRewardsRequest request,
                UnityAction<AsyncResult<Result.DescribeGlobalRankingReceivedRewardsResult>> callback
        ) =>
            new DescribeGlobalRankingReceivedRewardsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.DescribeGlobalRankingReceivedRewardsResult> DescribeGlobalRankingReceivedRewardsFuture(
                Request.DescribeGlobalRankingReceivedRewardsRequest request
        ) =>
            new DescribeGlobalRankingReceivedRewardsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.DescribeGlobalRankingReceivedRewardsResult> DescribeGlobalRankingReceivedRewardsAsync(
                Request.DescribeGlobalRankingReceivedRewardsRequest request
        ) =>
            new DescribeGlobalRankingReceivedRewardsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.DescribeGlobalRankingReceivedRewardsResult>();
    #else
		public DescribeGlobalRankingReceivedRewardsTask DescribeGlobalRankingReceivedRewardsAsync(
                Request.DescribeGlobalRankingReceivedRewardsRequest request
        )
		{
			return new DescribeGlobalRankingReceivedRewardsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.DescribeGlobalRankingReceivedRewardsResult> DescribeGlobalRankingReceivedRewardsAsync(
                Request.DescribeGlobalRankingReceivedRewardsRequest request
        ) =>
            new DescribeGlobalRankingReceivedRewardsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class DescribeGlobalRankingReceivedRewardsByUserIdTask : Gs2RestSessionTask<DescribeGlobalRankingReceivedRewardsByUserIdRequest, DescribeGlobalRankingReceivedRewardsByUserIdResult>
        {
            public DescribeGlobalRankingReceivedRewardsByUserIdTask(IGs2Session session, RestSessionRequestFactory factory, DescribeGlobalRankingReceivedRewardsByUserIdRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DescribeGlobalRankingReceivedRewardsByUserIdRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "ranking2")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/{userId}/global/reward/received";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{userId}", !string.IsNullOrEmpty(request.UserId) ? request.UserId.ToString() : "null");

                var sessionRequest = Factory.Get(url);
                if (request.ContextStack != null)
                {
                    sessionRequest.AddQueryString("contextStack", request.ContextStack);
                }
                if (request.RankingName != null) {
                    sessionRequest.AddQueryString("rankingName", $"{request.RankingName}");
                }
                if (request.Season != null) {
                    sessionRequest.AddQueryString("season", $"{request.Season}");
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
		public IEnumerator DescribeGlobalRankingReceivedRewardsByUserId(
                Request.DescribeGlobalRankingReceivedRewardsByUserIdRequest request,
                UnityAction<AsyncResult<Result.DescribeGlobalRankingReceivedRewardsByUserIdResult>> callback
        ) =>
            new DescribeGlobalRankingReceivedRewardsByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.DescribeGlobalRankingReceivedRewardsByUserIdResult> DescribeGlobalRankingReceivedRewardsByUserIdFuture(
                Request.DescribeGlobalRankingReceivedRewardsByUserIdRequest request
        ) =>
            new DescribeGlobalRankingReceivedRewardsByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.DescribeGlobalRankingReceivedRewardsByUserIdResult> DescribeGlobalRankingReceivedRewardsByUserIdAsync(
                Request.DescribeGlobalRankingReceivedRewardsByUserIdRequest request
        ) =>
            new DescribeGlobalRankingReceivedRewardsByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.DescribeGlobalRankingReceivedRewardsByUserIdResult>();
    #else
		public DescribeGlobalRankingReceivedRewardsByUserIdTask DescribeGlobalRankingReceivedRewardsByUserIdAsync(
                Request.DescribeGlobalRankingReceivedRewardsByUserIdRequest request
        )
		{
			return new DescribeGlobalRankingReceivedRewardsByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.DescribeGlobalRankingReceivedRewardsByUserIdResult> DescribeGlobalRankingReceivedRewardsByUserIdAsync(
                Request.DescribeGlobalRankingReceivedRewardsByUserIdRequest request
        ) =>
            new DescribeGlobalRankingReceivedRewardsByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class CreateGlobalRankingReceivedRewardTask : Gs2RestSessionTask<CreateGlobalRankingReceivedRewardRequest, CreateGlobalRankingReceivedRewardResult>
        {
            public CreateGlobalRankingReceivedRewardTask(IGs2Session session, RestSessionRequestFactory factory, CreateGlobalRankingReceivedRewardRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(CreateGlobalRankingReceivedRewardRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "ranking2")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/me/global/reward/received/{rankingName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{rankingName}", !string.IsNullOrEmpty(request.RankingName) ? request.RankingName.ToString() : "null");

                var sessionRequest = Factory.Post(url);

                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);
                jsonWriter.WriteObjectStart();
                if (request.Season != null)
                {
                    jsonWriter.WritePropertyName("season");
                    jsonWriter.Write(request.Season.ToString());
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
		public IEnumerator CreateGlobalRankingReceivedReward(
                Request.CreateGlobalRankingReceivedRewardRequest request,
                UnityAction<AsyncResult<Result.CreateGlobalRankingReceivedRewardResult>> callback
        ) =>
            new CreateGlobalRankingReceivedRewardTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.CreateGlobalRankingReceivedRewardResult> CreateGlobalRankingReceivedRewardFuture(
                Request.CreateGlobalRankingReceivedRewardRequest request
        ) =>
            new CreateGlobalRankingReceivedRewardTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.CreateGlobalRankingReceivedRewardResult> CreateGlobalRankingReceivedRewardAsync(
                Request.CreateGlobalRankingReceivedRewardRequest request
        ) =>
            new CreateGlobalRankingReceivedRewardTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.CreateGlobalRankingReceivedRewardResult>();
    #else
		public CreateGlobalRankingReceivedRewardTask CreateGlobalRankingReceivedRewardAsync(
                Request.CreateGlobalRankingReceivedRewardRequest request
        )
		{
			return new CreateGlobalRankingReceivedRewardTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.CreateGlobalRankingReceivedRewardResult> CreateGlobalRankingReceivedRewardAsync(
                Request.CreateGlobalRankingReceivedRewardRequest request
        ) =>
            new CreateGlobalRankingReceivedRewardTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class CreateGlobalRankingReceivedRewardByUserIdTask : Gs2RestSessionTask<CreateGlobalRankingReceivedRewardByUserIdRequest, CreateGlobalRankingReceivedRewardByUserIdResult>
        {
            public CreateGlobalRankingReceivedRewardByUserIdTask(IGs2Session session, RestSessionRequestFactory factory, CreateGlobalRankingReceivedRewardByUserIdRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(CreateGlobalRankingReceivedRewardByUserIdRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "ranking2")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/{userId}/global/reward/received/{rankingName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{rankingName}", !string.IsNullOrEmpty(request.RankingName) ? request.RankingName.ToString() : "null");
                url = url.Replace("{userId}", !string.IsNullOrEmpty(request.UserId) ? request.UserId.ToString() : "null");

                var sessionRequest = Factory.Post(url);

                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);
                jsonWriter.WriteObjectStart();
                if (request.Season != null)
                {
                    jsonWriter.WritePropertyName("season");
                    jsonWriter.Write(request.Season.ToString());
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
		public IEnumerator CreateGlobalRankingReceivedRewardByUserId(
                Request.CreateGlobalRankingReceivedRewardByUserIdRequest request,
                UnityAction<AsyncResult<Result.CreateGlobalRankingReceivedRewardByUserIdResult>> callback
        ) =>
            new CreateGlobalRankingReceivedRewardByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.CreateGlobalRankingReceivedRewardByUserIdResult> CreateGlobalRankingReceivedRewardByUserIdFuture(
                Request.CreateGlobalRankingReceivedRewardByUserIdRequest request
        ) =>
            new CreateGlobalRankingReceivedRewardByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.CreateGlobalRankingReceivedRewardByUserIdResult> CreateGlobalRankingReceivedRewardByUserIdAsync(
                Request.CreateGlobalRankingReceivedRewardByUserIdRequest request
        ) =>
            new CreateGlobalRankingReceivedRewardByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.CreateGlobalRankingReceivedRewardByUserIdResult>();
    #else
		public CreateGlobalRankingReceivedRewardByUserIdTask CreateGlobalRankingReceivedRewardByUserIdAsync(
                Request.CreateGlobalRankingReceivedRewardByUserIdRequest request
        )
		{
			return new CreateGlobalRankingReceivedRewardByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.CreateGlobalRankingReceivedRewardByUserIdResult> CreateGlobalRankingReceivedRewardByUserIdAsync(
                Request.CreateGlobalRankingReceivedRewardByUserIdRequest request
        ) =>
            new CreateGlobalRankingReceivedRewardByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class ReceiveGlobalRankingReceivedRewardTask : Gs2RestSessionTask<ReceiveGlobalRankingReceivedRewardRequest, ReceiveGlobalRankingReceivedRewardResult>
        {
            public ReceiveGlobalRankingReceivedRewardTask(IGs2Session session, RestSessionRequestFactory factory, ReceiveGlobalRankingReceivedRewardRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(ReceiveGlobalRankingReceivedRewardRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "ranking2")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/me/global/reward/received/{rankingName}/{season}/reward/receive";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{rankingName}", !string.IsNullOrEmpty(request.RankingName) ? request.RankingName.ToString() : "null");
                url = url.Replace("{season}",request.Season != null ? request.Season.ToString() : "null");

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
		public IEnumerator ReceiveGlobalRankingReceivedReward(
                Request.ReceiveGlobalRankingReceivedRewardRequest request,
                UnityAction<AsyncResult<Result.ReceiveGlobalRankingReceivedRewardResult>> callback
        ) =>
            new ReceiveGlobalRankingReceivedRewardTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.ReceiveGlobalRankingReceivedRewardResult> ReceiveGlobalRankingReceivedRewardFuture(
                Request.ReceiveGlobalRankingReceivedRewardRequest request
        ) =>
            new ReceiveGlobalRankingReceivedRewardTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.ReceiveGlobalRankingReceivedRewardResult> ReceiveGlobalRankingReceivedRewardAsync(
                Request.ReceiveGlobalRankingReceivedRewardRequest request
        ) =>
            new ReceiveGlobalRankingReceivedRewardTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.ReceiveGlobalRankingReceivedRewardResult>();
    #else
		public ReceiveGlobalRankingReceivedRewardTask ReceiveGlobalRankingReceivedRewardAsync(
                Request.ReceiveGlobalRankingReceivedRewardRequest request
        )
		{
			return new ReceiveGlobalRankingReceivedRewardTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.ReceiveGlobalRankingReceivedRewardResult> ReceiveGlobalRankingReceivedRewardAsync(
                Request.ReceiveGlobalRankingReceivedRewardRequest request
        ) =>
            new ReceiveGlobalRankingReceivedRewardTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class ReceiveGlobalRankingReceivedRewardByUserIdTask : Gs2RestSessionTask<ReceiveGlobalRankingReceivedRewardByUserIdRequest, ReceiveGlobalRankingReceivedRewardByUserIdResult>
        {
            public ReceiveGlobalRankingReceivedRewardByUserIdTask(IGs2Session session, RestSessionRequestFactory factory, ReceiveGlobalRankingReceivedRewardByUserIdRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(ReceiveGlobalRankingReceivedRewardByUserIdRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "ranking2")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/{userId}/global/reward/received/{rankingName}/{season}/reward/receive";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{userId}", !string.IsNullOrEmpty(request.UserId) ? request.UserId.ToString() : "null");
                url = url.Replace("{rankingName}", !string.IsNullOrEmpty(request.RankingName) ? request.RankingName.ToString() : "null");
                url = url.Replace("{season}",request.Season != null ? request.Season.ToString() : "null");

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
		public IEnumerator ReceiveGlobalRankingReceivedRewardByUserId(
                Request.ReceiveGlobalRankingReceivedRewardByUserIdRequest request,
                UnityAction<AsyncResult<Result.ReceiveGlobalRankingReceivedRewardByUserIdResult>> callback
        ) =>
            new ReceiveGlobalRankingReceivedRewardByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.ReceiveGlobalRankingReceivedRewardByUserIdResult> ReceiveGlobalRankingReceivedRewardByUserIdFuture(
                Request.ReceiveGlobalRankingReceivedRewardByUserIdRequest request
        ) =>
            new ReceiveGlobalRankingReceivedRewardByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.ReceiveGlobalRankingReceivedRewardByUserIdResult> ReceiveGlobalRankingReceivedRewardByUserIdAsync(
                Request.ReceiveGlobalRankingReceivedRewardByUserIdRequest request
        ) =>
            new ReceiveGlobalRankingReceivedRewardByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.ReceiveGlobalRankingReceivedRewardByUserIdResult>();
    #else
		public ReceiveGlobalRankingReceivedRewardByUserIdTask ReceiveGlobalRankingReceivedRewardByUserIdAsync(
                Request.ReceiveGlobalRankingReceivedRewardByUserIdRequest request
        )
		{
			return new ReceiveGlobalRankingReceivedRewardByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.ReceiveGlobalRankingReceivedRewardByUserIdResult> ReceiveGlobalRankingReceivedRewardByUserIdAsync(
                Request.ReceiveGlobalRankingReceivedRewardByUserIdRequest request
        ) =>
            new ReceiveGlobalRankingReceivedRewardByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class GetGlobalRankingReceivedRewardTask : Gs2RestSessionTask<GetGlobalRankingReceivedRewardRequest, GetGlobalRankingReceivedRewardResult>
        {
            public GetGlobalRankingReceivedRewardTask(IGs2Session session, RestSessionRequestFactory factory, GetGlobalRankingReceivedRewardRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(GetGlobalRankingReceivedRewardRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "ranking2")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/me/global/reward/received/{rankingName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{rankingName}", !string.IsNullOrEmpty(request.RankingName) ? request.RankingName.ToString() : "null");

                var sessionRequest = Factory.Get(url);
                if (request.ContextStack != null)
                {
                    sessionRequest.AddQueryString("contextStack", request.ContextStack);
                }
                if (request.Season != null) {
                    sessionRequest.AddQueryString("season", $"{request.Season}");
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
		public IEnumerator GetGlobalRankingReceivedReward(
                Request.GetGlobalRankingReceivedRewardRequest request,
                UnityAction<AsyncResult<Result.GetGlobalRankingReceivedRewardResult>> callback
        ) =>
            new GetGlobalRankingReceivedRewardTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.GetGlobalRankingReceivedRewardResult> GetGlobalRankingReceivedRewardFuture(
                Request.GetGlobalRankingReceivedRewardRequest request
        ) =>
            new GetGlobalRankingReceivedRewardTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.GetGlobalRankingReceivedRewardResult> GetGlobalRankingReceivedRewardAsync(
                Request.GetGlobalRankingReceivedRewardRequest request
        ) =>
            new GetGlobalRankingReceivedRewardTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.GetGlobalRankingReceivedRewardResult>();
    #else
		public GetGlobalRankingReceivedRewardTask GetGlobalRankingReceivedRewardAsync(
                Request.GetGlobalRankingReceivedRewardRequest request
        )
		{
			return new GetGlobalRankingReceivedRewardTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.GetGlobalRankingReceivedRewardResult> GetGlobalRankingReceivedRewardAsync(
                Request.GetGlobalRankingReceivedRewardRequest request
        ) =>
            new GetGlobalRankingReceivedRewardTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class GetGlobalRankingReceivedRewardByUserIdTask : Gs2RestSessionTask<GetGlobalRankingReceivedRewardByUserIdRequest, GetGlobalRankingReceivedRewardByUserIdResult>
        {
            public GetGlobalRankingReceivedRewardByUserIdTask(IGs2Session session, RestSessionRequestFactory factory, GetGlobalRankingReceivedRewardByUserIdRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(GetGlobalRankingReceivedRewardByUserIdRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "ranking2")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/{userId}/global/reward/received/{rankingName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{rankingName}", !string.IsNullOrEmpty(request.RankingName) ? request.RankingName.ToString() : "null");
                url = url.Replace("{userId}", !string.IsNullOrEmpty(request.UserId) ? request.UserId.ToString() : "null");

                var sessionRequest = Factory.Get(url);
                if (request.ContextStack != null)
                {
                    sessionRequest.AddQueryString("contextStack", request.ContextStack);
                }
                if (request.Season != null) {
                    sessionRequest.AddQueryString("season", $"{request.Season}");
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
		public IEnumerator GetGlobalRankingReceivedRewardByUserId(
                Request.GetGlobalRankingReceivedRewardByUserIdRequest request,
                UnityAction<AsyncResult<Result.GetGlobalRankingReceivedRewardByUserIdResult>> callback
        ) =>
            new GetGlobalRankingReceivedRewardByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.GetGlobalRankingReceivedRewardByUserIdResult> GetGlobalRankingReceivedRewardByUserIdFuture(
                Request.GetGlobalRankingReceivedRewardByUserIdRequest request
        ) =>
            new GetGlobalRankingReceivedRewardByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.GetGlobalRankingReceivedRewardByUserIdResult> GetGlobalRankingReceivedRewardByUserIdAsync(
                Request.GetGlobalRankingReceivedRewardByUserIdRequest request
        ) =>
            new GetGlobalRankingReceivedRewardByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.GetGlobalRankingReceivedRewardByUserIdResult>();
    #else
		public GetGlobalRankingReceivedRewardByUserIdTask GetGlobalRankingReceivedRewardByUserIdAsync(
                Request.GetGlobalRankingReceivedRewardByUserIdRequest request
        )
		{
			return new GetGlobalRankingReceivedRewardByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.GetGlobalRankingReceivedRewardByUserIdResult> GetGlobalRankingReceivedRewardByUserIdAsync(
                Request.GetGlobalRankingReceivedRewardByUserIdRequest request
        ) =>
            new GetGlobalRankingReceivedRewardByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class DeleteGlobalRankingReceivedRewardByUserIdTask : Gs2RestSessionTask<DeleteGlobalRankingReceivedRewardByUserIdRequest, DeleteGlobalRankingReceivedRewardByUserIdResult>
        {
            public DeleteGlobalRankingReceivedRewardByUserIdTask(IGs2Session session, RestSessionRequestFactory factory, DeleteGlobalRankingReceivedRewardByUserIdRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DeleteGlobalRankingReceivedRewardByUserIdRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "ranking2")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/{userId}/global/reward/received/{rankingName}/{season}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{rankingName}", !string.IsNullOrEmpty(request.RankingName) ? request.RankingName.ToString() : "null");
                url = url.Replace("{userId}", !string.IsNullOrEmpty(request.UserId) ? request.UserId.ToString() : "null");

                var sessionRequest = Factory.Delete(url);
                if (request.ContextStack != null)
                {
                    sessionRequest.AddQueryString("contextStack", request.ContextStack);
                }
                if (request.Season != null) {
                    sessionRequest.AddQueryString("season", $"{request.Season}");
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
		public IEnumerator DeleteGlobalRankingReceivedRewardByUserId(
                Request.DeleteGlobalRankingReceivedRewardByUserIdRequest request,
                UnityAction<AsyncResult<Result.DeleteGlobalRankingReceivedRewardByUserIdResult>> callback
        ) =>
            new DeleteGlobalRankingReceivedRewardByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.DeleteGlobalRankingReceivedRewardByUserIdResult> DeleteGlobalRankingReceivedRewardByUserIdFuture(
                Request.DeleteGlobalRankingReceivedRewardByUserIdRequest request
        ) =>
            new DeleteGlobalRankingReceivedRewardByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.DeleteGlobalRankingReceivedRewardByUserIdResult> DeleteGlobalRankingReceivedRewardByUserIdAsync(
                Request.DeleteGlobalRankingReceivedRewardByUserIdRequest request
        ) =>
            new DeleteGlobalRankingReceivedRewardByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.DeleteGlobalRankingReceivedRewardByUserIdResult>();
    #else
		public DeleteGlobalRankingReceivedRewardByUserIdTask DeleteGlobalRankingReceivedRewardByUserIdAsync(
                Request.DeleteGlobalRankingReceivedRewardByUserIdRequest request
        )
		{
			return new DeleteGlobalRankingReceivedRewardByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.DeleteGlobalRankingReceivedRewardByUserIdResult> DeleteGlobalRankingReceivedRewardByUserIdAsync(
                Request.DeleteGlobalRankingReceivedRewardByUserIdRequest request
        ) =>
            new DeleteGlobalRankingReceivedRewardByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class CreateGlobalRankingReceivedRewardByStampTaskTask : Gs2RestSessionTask<CreateGlobalRankingReceivedRewardByStampTaskRequest, CreateGlobalRankingReceivedRewardByStampTaskResult>
        {
            public CreateGlobalRankingReceivedRewardByStampTaskTask(IGs2Session session, RestSessionRequestFactory factory, CreateGlobalRankingReceivedRewardByStampTaskRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(CreateGlobalRankingReceivedRewardByStampTaskRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "ranking2")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/stamp/ranking/global/reward/receive";

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
		public IEnumerator CreateGlobalRankingReceivedRewardByStampTask(
                Request.CreateGlobalRankingReceivedRewardByStampTaskRequest request,
                UnityAction<AsyncResult<Result.CreateGlobalRankingReceivedRewardByStampTaskResult>> callback
        ) =>
            new CreateGlobalRankingReceivedRewardByStampTaskTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.CreateGlobalRankingReceivedRewardByStampTaskResult> CreateGlobalRankingReceivedRewardByStampTaskFuture(
                Request.CreateGlobalRankingReceivedRewardByStampTaskRequest request
        ) =>
            new CreateGlobalRankingReceivedRewardByStampTaskTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.CreateGlobalRankingReceivedRewardByStampTaskResult> CreateGlobalRankingReceivedRewardByStampTaskAsync(
                Request.CreateGlobalRankingReceivedRewardByStampTaskRequest request
        ) =>
            new CreateGlobalRankingReceivedRewardByStampTaskTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.CreateGlobalRankingReceivedRewardByStampTaskResult>();
    #else
		public CreateGlobalRankingReceivedRewardByStampTaskTask CreateGlobalRankingReceivedRewardByStampTaskAsync(
                Request.CreateGlobalRankingReceivedRewardByStampTaskRequest request
        )
		{
			return new CreateGlobalRankingReceivedRewardByStampTaskTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.CreateGlobalRankingReceivedRewardByStampTaskResult> CreateGlobalRankingReceivedRewardByStampTaskAsync(
                Request.CreateGlobalRankingReceivedRewardByStampTaskRequest request
        ) =>
            new CreateGlobalRankingReceivedRewardByStampTaskTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class DescribeGlobalRankingsTask : Gs2RestSessionTask<DescribeGlobalRankingsRequest, DescribeGlobalRankingsResult>
        {
            public DescribeGlobalRankingsTask(IGs2Session session, RestSessionRequestFactory factory, DescribeGlobalRankingsRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DescribeGlobalRankingsRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "ranking2")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/ranking/global/{rankingName}/user/me";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{rankingName}", !string.IsNullOrEmpty(request.RankingName) ? request.RankingName.ToString() : "null");

                var sessionRequest = Factory.Get(url);
                if (request.ContextStack != null)
                {
                    sessionRequest.AddQueryString("contextStack", request.ContextStack);
                }
                if (request.Season != null) {
                    sessionRequest.AddQueryString("season", $"{request.Season}");
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
		public IEnumerator DescribeGlobalRankings(
                Request.DescribeGlobalRankingsRequest request,
                UnityAction<AsyncResult<Result.DescribeGlobalRankingsResult>> callback
        ) =>
            new DescribeGlobalRankingsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.DescribeGlobalRankingsResult> DescribeGlobalRankingsFuture(
                Request.DescribeGlobalRankingsRequest request
        ) =>
            new DescribeGlobalRankingsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.DescribeGlobalRankingsResult> DescribeGlobalRankingsAsync(
                Request.DescribeGlobalRankingsRequest request
        ) =>
            new DescribeGlobalRankingsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.DescribeGlobalRankingsResult>();
    #else
		public DescribeGlobalRankingsTask DescribeGlobalRankingsAsync(
                Request.DescribeGlobalRankingsRequest request
        )
		{
			return new DescribeGlobalRankingsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.DescribeGlobalRankingsResult> DescribeGlobalRankingsAsync(
                Request.DescribeGlobalRankingsRequest request
        ) =>
            new DescribeGlobalRankingsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class DescribeGlobalRankingsByUserIdTask : Gs2RestSessionTask<DescribeGlobalRankingsByUserIdRequest, DescribeGlobalRankingsByUserIdResult>
        {
            public DescribeGlobalRankingsByUserIdTask(IGs2Session session, RestSessionRequestFactory factory, DescribeGlobalRankingsByUserIdRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DescribeGlobalRankingsByUserIdRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "ranking2")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/ranking/global/{rankingName}/user/{userId}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{userId}", !string.IsNullOrEmpty(request.UserId) ? request.UserId.ToString() : "null");
                url = url.Replace("{rankingName}", !string.IsNullOrEmpty(request.RankingName) ? request.RankingName.ToString() : "null");

                var sessionRequest = Factory.Get(url);
                if (request.ContextStack != null)
                {
                    sessionRequest.AddQueryString("contextStack", request.ContextStack);
                }
                if (request.Season != null) {
                    sessionRequest.AddQueryString("season", $"{request.Season}");
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
		public IEnumerator DescribeGlobalRankingsByUserId(
                Request.DescribeGlobalRankingsByUserIdRequest request,
                UnityAction<AsyncResult<Result.DescribeGlobalRankingsByUserIdResult>> callback
        ) =>
            new DescribeGlobalRankingsByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.DescribeGlobalRankingsByUserIdResult> DescribeGlobalRankingsByUserIdFuture(
                Request.DescribeGlobalRankingsByUserIdRequest request
        ) =>
            new DescribeGlobalRankingsByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.DescribeGlobalRankingsByUserIdResult> DescribeGlobalRankingsByUserIdAsync(
                Request.DescribeGlobalRankingsByUserIdRequest request
        ) =>
            new DescribeGlobalRankingsByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.DescribeGlobalRankingsByUserIdResult>();
    #else
		public DescribeGlobalRankingsByUserIdTask DescribeGlobalRankingsByUserIdAsync(
                Request.DescribeGlobalRankingsByUserIdRequest request
        )
		{
			return new DescribeGlobalRankingsByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.DescribeGlobalRankingsByUserIdResult> DescribeGlobalRankingsByUserIdAsync(
                Request.DescribeGlobalRankingsByUserIdRequest request
        ) =>
            new DescribeGlobalRankingsByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class GetGlobalRankingTask : Gs2RestSessionTask<GetGlobalRankingRequest, GetGlobalRankingResult>
        {
            public GetGlobalRankingTask(IGs2Session session, RestSessionRequestFactory factory, GetGlobalRankingRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(GetGlobalRankingRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "ranking2")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/ranking/global/{rankingName}/user/me/rank";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{rankingName}", !string.IsNullOrEmpty(request.RankingName) ? request.RankingName.ToString() : "null");

                var sessionRequest = Factory.Get(url);
                if (request.ContextStack != null)
                {
                    sessionRequest.AddQueryString("contextStack", request.ContextStack);
                }
                if (request.Season != null) {
                    sessionRequest.AddQueryString("season", $"{request.Season}");
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
		public IEnumerator GetGlobalRanking(
                Request.GetGlobalRankingRequest request,
                UnityAction<AsyncResult<Result.GetGlobalRankingResult>> callback
        ) =>
            new GetGlobalRankingTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.GetGlobalRankingResult> GetGlobalRankingFuture(
                Request.GetGlobalRankingRequest request
        ) =>
            new GetGlobalRankingTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.GetGlobalRankingResult> GetGlobalRankingAsync(
                Request.GetGlobalRankingRequest request
        ) =>
            new GetGlobalRankingTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.GetGlobalRankingResult>();
    #else
		public GetGlobalRankingTask GetGlobalRankingAsync(
                Request.GetGlobalRankingRequest request
        )
		{
			return new GetGlobalRankingTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.GetGlobalRankingResult> GetGlobalRankingAsync(
                Request.GetGlobalRankingRequest request
        ) =>
            new GetGlobalRankingTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class GetGlobalRankingByUserIdTask : Gs2RestSessionTask<GetGlobalRankingByUserIdRequest, GetGlobalRankingByUserIdResult>
        {
            public GetGlobalRankingByUserIdTask(IGs2Session session, RestSessionRequestFactory factory, GetGlobalRankingByUserIdRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(GetGlobalRankingByUserIdRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "ranking2")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/ranking/global/{rankingName}/user/{userId}/rank";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{rankingName}", !string.IsNullOrEmpty(request.RankingName) ? request.RankingName.ToString() : "null");
                url = url.Replace("{userId}", !string.IsNullOrEmpty(request.UserId) ? request.UserId.ToString() : "null");

                var sessionRequest = Factory.Get(url);
                if (request.ContextStack != null)
                {
                    sessionRequest.AddQueryString("contextStack", request.ContextStack);
                }
                if (request.Season != null) {
                    sessionRequest.AddQueryString("season", $"{request.Season}");
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
		public IEnumerator GetGlobalRankingByUserId(
                Request.GetGlobalRankingByUserIdRequest request,
                UnityAction<AsyncResult<Result.GetGlobalRankingByUserIdResult>> callback
        ) =>
            new GetGlobalRankingByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.GetGlobalRankingByUserIdResult> GetGlobalRankingByUserIdFuture(
                Request.GetGlobalRankingByUserIdRequest request
        ) =>
            new GetGlobalRankingByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.GetGlobalRankingByUserIdResult> GetGlobalRankingByUserIdAsync(
                Request.GetGlobalRankingByUserIdRequest request
        ) =>
            new GetGlobalRankingByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.GetGlobalRankingByUserIdResult>();
    #else
		public GetGlobalRankingByUserIdTask GetGlobalRankingByUserIdAsync(
                Request.GetGlobalRankingByUserIdRequest request
        )
		{
			return new GetGlobalRankingByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.GetGlobalRankingByUserIdResult> GetGlobalRankingByUserIdAsync(
                Request.GetGlobalRankingByUserIdRequest request
        ) =>
            new GetGlobalRankingByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class DescribeClusterRankingModelsTask : Gs2RestSessionTask<DescribeClusterRankingModelsRequest, DescribeClusterRankingModelsResult>
        {
            public DescribeClusterRankingModelsTask(IGs2Session session, RestSessionRequestFactory factory, DescribeClusterRankingModelsRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DescribeClusterRankingModelsRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "ranking2")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/model/cluster";

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
		public IEnumerator DescribeClusterRankingModels(
                Request.DescribeClusterRankingModelsRequest request,
                UnityAction<AsyncResult<Result.DescribeClusterRankingModelsResult>> callback
        ) =>
            new DescribeClusterRankingModelsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.DescribeClusterRankingModelsResult> DescribeClusterRankingModelsFuture(
                Request.DescribeClusterRankingModelsRequest request
        ) =>
            new DescribeClusterRankingModelsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.DescribeClusterRankingModelsResult> DescribeClusterRankingModelsAsync(
                Request.DescribeClusterRankingModelsRequest request
        ) =>
            new DescribeClusterRankingModelsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.DescribeClusterRankingModelsResult>();
    #else
		public DescribeClusterRankingModelsTask DescribeClusterRankingModelsAsync(
                Request.DescribeClusterRankingModelsRequest request
        )
		{
			return new DescribeClusterRankingModelsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.DescribeClusterRankingModelsResult> DescribeClusterRankingModelsAsync(
                Request.DescribeClusterRankingModelsRequest request
        ) =>
            new DescribeClusterRankingModelsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class GetClusterRankingModelTask : Gs2RestSessionTask<GetClusterRankingModelRequest, GetClusterRankingModelResult>
        {
            public GetClusterRankingModelTask(IGs2Session session, RestSessionRequestFactory factory, GetClusterRankingModelRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(GetClusterRankingModelRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "ranking2")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/model/cluster/{rankingName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{rankingName}", !string.IsNullOrEmpty(request.RankingName) ? request.RankingName.ToString() : "null");

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
		public IEnumerator GetClusterRankingModel(
                Request.GetClusterRankingModelRequest request,
                UnityAction<AsyncResult<Result.GetClusterRankingModelResult>> callback
        ) =>
            new GetClusterRankingModelTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.GetClusterRankingModelResult> GetClusterRankingModelFuture(
                Request.GetClusterRankingModelRequest request
        ) =>
            new GetClusterRankingModelTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.GetClusterRankingModelResult> GetClusterRankingModelAsync(
                Request.GetClusterRankingModelRequest request
        ) =>
            new GetClusterRankingModelTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.GetClusterRankingModelResult>();
    #else
		public GetClusterRankingModelTask GetClusterRankingModelAsync(
                Request.GetClusterRankingModelRequest request
        )
		{
			return new GetClusterRankingModelTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.GetClusterRankingModelResult> GetClusterRankingModelAsync(
                Request.GetClusterRankingModelRequest request
        ) =>
            new GetClusterRankingModelTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class DescribeClusterRankingModelMastersTask : Gs2RestSessionTask<DescribeClusterRankingModelMastersRequest, DescribeClusterRankingModelMastersResult>
        {
            public DescribeClusterRankingModelMastersTask(IGs2Session session, RestSessionRequestFactory factory, DescribeClusterRankingModelMastersRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DescribeClusterRankingModelMastersRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "ranking2")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/master/cluster";

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
		public IEnumerator DescribeClusterRankingModelMasters(
                Request.DescribeClusterRankingModelMastersRequest request,
                UnityAction<AsyncResult<Result.DescribeClusterRankingModelMastersResult>> callback
        ) =>
            new DescribeClusterRankingModelMastersTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.DescribeClusterRankingModelMastersResult> DescribeClusterRankingModelMastersFuture(
                Request.DescribeClusterRankingModelMastersRequest request
        ) =>
            new DescribeClusterRankingModelMastersTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.DescribeClusterRankingModelMastersResult> DescribeClusterRankingModelMastersAsync(
                Request.DescribeClusterRankingModelMastersRequest request
        ) =>
            new DescribeClusterRankingModelMastersTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.DescribeClusterRankingModelMastersResult>();
    #else
		public DescribeClusterRankingModelMastersTask DescribeClusterRankingModelMastersAsync(
                Request.DescribeClusterRankingModelMastersRequest request
        )
		{
			return new DescribeClusterRankingModelMastersTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.DescribeClusterRankingModelMastersResult> DescribeClusterRankingModelMastersAsync(
                Request.DescribeClusterRankingModelMastersRequest request
        ) =>
            new DescribeClusterRankingModelMastersTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class CreateClusterRankingModelMasterTask : Gs2RestSessionTask<CreateClusterRankingModelMasterRequest, CreateClusterRankingModelMasterResult>
        {
            public CreateClusterRankingModelMasterTask(IGs2Session session, RestSessionRequestFactory factory, CreateClusterRankingModelMasterRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(CreateClusterRankingModelMasterRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "ranking2")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/master/cluster";

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
                if (request.ClusterType != null)
                {
                    jsonWriter.WritePropertyName("clusterType");
                    jsonWriter.Write(request.ClusterType);
                }
                if (request.MinimumValue != null)
                {
                    jsonWriter.WritePropertyName("minimumValue");
                    jsonWriter.Write(request.MinimumValue.ToString());
                }
                if (request.MaximumValue != null)
                {
                    jsonWriter.WritePropertyName("maximumValue");
                    jsonWriter.Write(request.MaximumValue.ToString());
                }
                if (request.Sum != null)
                {
                    jsonWriter.WritePropertyName("sum");
                    jsonWriter.Write(request.Sum.ToString());
                }
                if (request.OrderDirection != null)
                {
                    jsonWriter.WritePropertyName("orderDirection");
                    jsonWriter.Write(request.OrderDirection);
                }
                if (request.RankingRewards != null)
                {
                    jsonWriter.WritePropertyName("rankingRewards");
                    jsonWriter.WriteArrayStart();
                    foreach(var item in request.RankingRewards)
                    {
                        item.WriteJson(jsonWriter);
                    }
                    jsonWriter.WriteArrayEnd();
                }
                if (request.RewardCalculationIndex != null)
                {
                    jsonWriter.WritePropertyName("rewardCalculationIndex");
                    jsonWriter.Write(request.RewardCalculationIndex);
                }
                if (request.EntryPeriodEventId != null)
                {
                    jsonWriter.WritePropertyName("entryPeriodEventId");
                    jsonWriter.Write(request.EntryPeriodEventId);
                }
                if (request.AccessPeriodEventId != null)
                {
                    jsonWriter.WritePropertyName("accessPeriodEventId");
                    jsonWriter.Write(request.AccessPeriodEventId);
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
		public IEnumerator CreateClusterRankingModelMaster(
                Request.CreateClusterRankingModelMasterRequest request,
                UnityAction<AsyncResult<Result.CreateClusterRankingModelMasterResult>> callback
        ) =>
            new CreateClusterRankingModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.CreateClusterRankingModelMasterResult> CreateClusterRankingModelMasterFuture(
                Request.CreateClusterRankingModelMasterRequest request
        ) =>
            new CreateClusterRankingModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.CreateClusterRankingModelMasterResult> CreateClusterRankingModelMasterAsync(
                Request.CreateClusterRankingModelMasterRequest request
        ) =>
            new CreateClusterRankingModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.CreateClusterRankingModelMasterResult>();
    #else
		public CreateClusterRankingModelMasterTask CreateClusterRankingModelMasterAsync(
                Request.CreateClusterRankingModelMasterRequest request
        )
		{
			return new CreateClusterRankingModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.CreateClusterRankingModelMasterResult> CreateClusterRankingModelMasterAsync(
                Request.CreateClusterRankingModelMasterRequest request
        ) =>
            new CreateClusterRankingModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class GetClusterRankingModelMasterTask : Gs2RestSessionTask<GetClusterRankingModelMasterRequest, GetClusterRankingModelMasterResult>
        {
            public GetClusterRankingModelMasterTask(IGs2Session session, RestSessionRequestFactory factory, GetClusterRankingModelMasterRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(GetClusterRankingModelMasterRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "ranking2")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/master/cluster/{rankingName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{rankingName}", !string.IsNullOrEmpty(request.RankingName) ? request.RankingName.ToString() : "null");

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
		public IEnumerator GetClusterRankingModelMaster(
                Request.GetClusterRankingModelMasterRequest request,
                UnityAction<AsyncResult<Result.GetClusterRankingModelMasterResult>> callback
        ) =>
            new GetClusterRankingModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.GetClusterRankingModelMasterResult> GetClusterRankingModelMasterFuture(
                Request.GetClusterRankingModelMasterRequest request
        ) =>
            new GetClusterRankingModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.GetClusterRankingModelMasterResult> GetClusterRankingModelMasterAsync(
                Request.GetClusterRankingModelMasterRequest request
        ) =>
            new GetClusterRankingModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.GetClusterRankingModelMasterResult>();
    #else
		public GetClusterRankingModelMasterTask GetClusterRankingModelMasterAsync(
                Request.GetClusterRankingModelMasterRequest request
        )
		{
			return new GetClusterRankingModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.GetClusterRankingModelMasterResult> GetClusterRankingModelMasterAsync(
                Request.GetClusterRankingModelMasterRequest request
        ) =>
            new GetClusterRankingModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class UpdateClusterRankingModelMasterTask : Gs2RestSessionTask<UpdateClusterRankingModelMasterRequest, UpdateClusterRankingModelMasterResult>
        {
            public UpdateClusterRankingModelMasterTask(IGs2Session session, RestSessionRequestFactory factory, UpdateClusterRankingModelMasterRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(UpdateClusterRankingModelMasterRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "ranking2")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/master/cluster/{rankingName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{rankingName}", !string.IsNullOrEmpty(request.RankingName) ? request.RankingName.ToString() : "null");

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
                if (request.ClusterType != null)
                {
                    jsonWriter.WritePropertyName("clusterType");
                    jsonWriter.Write(request.ClusterType);
                }
                if (request.MinimumValue != null)
                {
                    jsonWriter.WritePropertyName("minimumValue");
                    jsonWriter.Write(request.MinimumValue.ToString());
                }
                if (request.MaximumValue != null)
                {
                    jsonWriter.WritePropertyName("maximumValue");
                    jsonWriter.Write(request.MaximumValue.ToString());
                }
                if (request.Sum != null)
                {
                    jsonWriter.WritePropertyName("sum");
                    jsonWriter.Write(request.Sum.ToString());
                }
                if (request.OrderDirection != null)
                {
                    jsonWriter.WritePropertyName("orderDirection");
                    jsonWriter.Write(request.OrderDirection);
                }
                if (request.RankingRewards != null)
                {
                    jsonWriter.WritePropertyName("rankingRewards");
                    jsonWriter.WriteArrayStart();
                    foreach(var item in request.RankingRewards)
                    {
                        item.WriteJson(jsonWriter);
                    }
                    jsonWriter.WriteArrayEnd();
                }
                if (request.RewardCalculationIndex != null)
                {
                    jsonWriter.WritePropertyName("rewardCalculationIndex");
                    jsonWriter.Write(request.RewardCalculationIndex);
                }
                if (request.EntryPeriodEventId != null)
                {
                    jsonWriter.WritePropertyName("entryPeriodEventId");
                    jsonWriter.Write(request.EntryPeriodEventId);
                }
                if (request.AccessPeriodEventId != null)
                {
                    jsonWriter.WritePropertyName("accessPeriodEventId");
                    jsonWriter.Write(request.AccessPeriodEventId);
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
		public IEnumerator UpdateClusterRankingModelMaster(
                Request.UpdateClusterRankingModelMasterRequest request,
                UnityAction<AsyncResult<Result.UpdateClusterRankingModelMasterResult>> callback
        ) =>
            new UpdateClusterRankingModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.UpdateClusterRankingModelMasterResult> UpdateClusterRankingModelMasterFuture(
                Request.UpdateClusterRankingModelMasterRequest request
        ) =>
            new UpdateClusterRankingModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.UpdateClusterRankingModelMasterResult> UpdateClusterRankingModelMasterAsync(
                Request.UpdateClusterRankingModelMasterRequest request
        ) =>
            new UpdateClusterRankingModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.UpdateClusterRankingModelMasterResult>();
    #else
		public UpdateClusterRankingModelMasterTask UpdateClusterRankingModelMasterAsync(
                Request.UpdateClusterRankingModelMasterRequest request
        )
		{
			return new UpdateClusterRankingModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.UpdateClusterRankingModelMasterResult> UpdateClusterRankingModelMasterAsync(
                Request.UpdateClusterRankingModelMasterRequest request
        ) =>
            new UpdateClusterRankingModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class DeleteClusterRankingModelMasterTask : Gs2RestSessionTask<DeleteClusterRankingModelMasterRequest, DeleteClusterRankingModelMasterResult>
        {
            public DeleteClusterRankingModelMasterTask(IGs2Session session, RestSessionRequestFactory factory, DeleteClusterRankingModelMasterRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DeleteClusterRankingModelMasterRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "ranking2")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/master/cluster/{rankingName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{rankingName}", !string.IsNullOrEmpty(request.RankingName) ? request.RankingName.ToString() : "null");

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
		public IEnumerator DeleteClusterRankingModelMaster(
                Request.DeleteClusterRankingModelMasterRequest request,
                UnityAction<AsyncResult<Result.DeleteClusterRankingModelMasterResult>> callback
        ) =>
            new DeleteClusterRankingModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.DeleteClusterRankingModelMasterResult> DeleteClusterRankingModelMasterFuture(
                Request.DeleteClusterRankingModelMasterRequest request
        ) =>
            new DeleteClusterRankingModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.DeleteClusterRankingModelMasterResult> DeleteClusterRankingModelMasterAsync(
                Request.DeleteClusterRankingModelMasterRequest request
        ) =>
            new DeleteClusterRankingModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.DeleteClusterRankingModelMasterResult>();
    #else
		public DeleteClusterRankingModelMasterTask DeleteClusterRankingModelMasterAsync(
                Request.DeleteClusterRankingModelMasterRequest request
        )
		{
			return new DeleteClusterRankingModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.DeleteClusterRankingModelMasterResult> DeleteClusterRankingModelMasterAsync(
                Request.DeleteClusterRankingModelMasterRequest request
        ) =>
            new DeleteClusterRankingModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class DescribeClusterRankingScoresTask : Gs2RestSessionTask<DescribeClusterRankingScoresRequest, DescribeClusterRankingScoresResult>
        {
            public DescribeClusterRankingScoresTask(IGs2Session session, RestSessionRequestFactory factory, DescribeClusterRankingScoresRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DescribeClusterRankingScoresRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "ranking2")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/me/score/cluster";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");

                var sessionRequest = Factory.Get(url);
                if (request.ContextStack != null)
                {
                    sessionRequest.AddQueryString("contextStack", request.ContextStack);
                }
                if (request.RankingName != null) {
                    sessionRequest.AddQueryString("rankingName", $"{request.RankingName}");
                }
                if (request.ClusterName != null) {
                    sessionRequest.AddQueryString("clusterName", $"{request.ClusterName}");
                }
                if (request.Season != null) {
                    sessionRequest.AddQueryString("season", $"{request.Season}");
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
		public IEnumerator DescribeClusterRankingScores(
                Request.DescribeClusterRankingScoresRequest request,
                UnityAction<AsyncResult<Result.DescribeClusterRankingScoresResult>> callback
        ) =>
            new DescribeClusterRankingScoresTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.DescribeClusterRankingScoresResult> DescribeClusterRankingScoresFuture(
                Request.DescribeClusterRankingScoresRequest request
        ) =>
            new DescribeClusterRankingScoresTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.DescribeClusterRankingScoresResult> DescribeClusterRankingScoresAsync(
                Request.DescribeClusterRankingScoresRequest request
        ) =>
            new DescribeClusterRankingScoresTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.DescribeClusterRankingScoresResult>();
    #else
		public DescribeClusterRankingScoresTask DescribeClusterRankingScoresAsync(
                Request.DescribeClusterRankingScoresRequest request
        )
		{
			return new DescribeClusterRankingScoresTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.DescribeClusterRankingScoresResult> DescribeClusterRankingScoresAsync(
                Request.DescribeClusterRankingScoresRequest request
        ) =>
            new DescribeClusterRankingScoresTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class DescribeClusterRankingScoresByUserIdTask : Gs2RestSessionTask<DescribeClusterRankingScoresByUserIdRequest, DescribeClusterRankingScoresByUserIdResult>
        {
            public DescribeClusterRankingScoresByUserIdTask(IGs2Session session, RestSessionRequestFactory factory, DescribeClusterRankingScoresByUserIdRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DescribeClusterRankingScoresByUserIdRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "ranking2")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/{userId}/score/cluster";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{userId}", !string.IsNullOrEmpty(request.UserId) ? request.UserId.ToString() : "null");

                var sessionRequest = Factory.Get(url);
                if (request.ContextStack != null)
                {
                    sessionRequest.AddQueryString("contextStack", request.ContextStack);
                }
                if (request.RankingName != null) {
                    sessionRequest.AddQueryString("rankingName", $"{request.RankingName}");
                }
                if (request.ClusterName != null) {
                    sessionRequest.AddQueryString("clusterName", $"{request.ClusterName}");
                }
                if (request.Season != null) {
                    sessionRequest.AddQueryString("season", $"{request.Season}");
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
		public IEnumerator DescribeClusterRankingScoresByUserId(
                Request.DescribeClusterRankingScoresByUserIdRequest request,
                UnityAction<AsyncResult<Result.DescribeClusterRankingScoresByUserIdResult>> callback
        ) =>
            new DescribeClusterRankingScoresByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.DescribeClusterRankingScoresByUserIdResult> DescribeClusterRankingScoresByUserIdFuture(
                Request.DescribeClusterRankingScoresByUserIdRequest request
        ) =>
            new DescribeClusterRankingScoresByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.DescribeClusterRankingScoresByUserIdResult> DescribeClusterRankingScoresByUserIdAsync(
                Request.DescribeClusterRankingScoresByUserIdRequest request
        ) =>
            new DescribeClusterRankingScoresByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.DescribeClusterRankingScoresByUserIdResult>();
    #else
		public DescribeClusterRankingScoresByUserIdTask DescribeClusterRankingScoresByUserIdAsync(
                Request.DescribeClusterRankingScoresByUserIdRequest request
        )
		{
			return new DescribeClusterRankingScoresByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.DescribeClusterRankingScoresByUserIdResult> DescribeClusterRankingScoresByUserIdAsync(
                Request.DescribeClusterRankingScoresByUserIdRequest request
        ) =>
            new DescribeClusterRankingScoresByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class PutClusterRankingScoreTask : Gs2RestSessionTask<PutClusterRankingScoreRequest, PutClusterRankingScoreResult>
        {
            public PutClusterRankingScoreTask(IGs2Session session, RestSessionRequestFactory factory, PutClusterRankingScoreRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(PutClusterRankingScoreRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "ranking2")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/me/score/cluster/{rankingName}/{clusterName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{rankingName}", !string.IsNullOrEmpty(request.RankingName) ? request.RankingName.ToString() : "null");
                url = url.Replace("{clusterName}", !string.IsNullOrEmpty(request.ClusterName) ? request.ClusterName.ToString() : "null");

                var sessionRequest = Factory.Put(url);

                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);
                jsonWriter.WriteObjectStart();
                if (request.Score != null)
                {
                    jsonWriter.WritePropertyName("score");
                    jsonWriter.Write(request.Score.ToString());
                }
                if (request.Metadata != null)
                {
                    jsonWriter.WritePropertyName("metadata");
                    jsonWriter.Write(request.Metadata);
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
		public IEnumerator PutClusterRankingScore(
                Request.PutClusterRankingScoreRequest request,
                UnityAction<AsyncResult<Result.PutClusterRankingScoreResult>> callback
        ) =>
            new PutClusterRankingScoreTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.PutClusterRankingScoreResult> PutClusterRankingScoreFuture(
                Request.PutClusterRankingScoreRequest request
        ) =>
            new PutClusterRankingScoreTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.PutClusterRankingScoreResult> PutClusterRankingScoreAsync(
                Request.PutClusterRankingScoreRequest request
        ) =>
            new PutClusterRankingScoreTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.PutClusterRankingScoreResult>();
    #else
		public PutClusterRankingScoreTask PutClusterRankingScoreAsync(
                Request.PutClusterRankingScoreRequest request
        )
		{
			return new PutClusterRankingScoreTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.PutClusterRankingScoreResult> PutClusterRankingScoreAsync(
                Request.PutClusterRankingScoreRequest request
        ) =>
            new PutClusterRankingScoreTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class PutClusterRankingScoreByUserIdTask : Gs2RestSessionTask<PutClusterRankingScoreByUserIdRequest, PutClusterRankingScoreByUserIdResult>
        {
            public PutClusterRankingScoreByUserIdTask(IGs2Session session, RestSessionRequestFactory factory, PutClusterRankingScoreByUserIdRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(PutClusterRankingScoreByUserIdRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "ranking2")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/{userId}/score/cluster/{rankingName}/{clusterName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{rankingName}", !string.IsNullOrEmpty(request.RankingName) ? request.RankingName.ToString() : "null");
                url = url.Replace("{clusterName}", !string.IsNullOrEmpty(request.ClusterName) ? request.ClusterName.ToString() : "null");
                url = url.Replace("{userId}", !string.IsNullOrEmpty(request.UserId) ? request.UserId.ToString() : "null");

                var sessionRequest = Factory.Put(url);

                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);
                jsonWriter.WriteObjectStart();
                if (request.Score != null)
                {
                    jsonWriter.WritePropertyName("score");
                    jsonWriter.Write(request.Score.ToString());
                }
                if (request.Metadata != null)
                {
                    jsonWriter.WritePropertyName("metadata");
                    jsonWriter.Write(request.Metadata);
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
		public IEnumerator PutClusterRankingScoreByUserId(
                Request.PutClusterRankingScoreByUserIdRequest request,
                UnityAction<AsyncResult<Result.PutClusterRankingScoreByUserIdResult>> callback
        ) =>
            new PutClusterRankingScoreByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.PutClusterRankingScoreByUserIdResult> PutClusterRankingScoreByUserIdFuture(
                Request.PutClusterRankingScoreByUserIdRequest request
        ) =>
            new PutClusterRankingScoreByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.PutClusterRankingScoreByUserIdResult> PutClusterRankingScoreByUserIdAsync(
                Request.PutClusterRankingScoreByUserIdRequest request
        ) =>
            new PutClusterRankingScoreByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.PutClusterRankingScoreByUserIdResult>();
    #else
		public PutClusterRankingScoreByUserIdTask PutClusterRankingScoreByUserIdAsync(
                Request.PutClusterRankingScoreByUserIdRequest request
        )
		{
			return new PutClusterRankingScoreByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.PutClusterRankingScoreByUserIdResult> PutClusterRankingScoreByUserIdAsync(
                Request.PutClusterRankingScoreByUserIdRequest request
        ) =>
            new PutClusterRankingScoreByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class GetClusterRankingScoreTask : Gs2RestSessionTask<GetClusterRankingScoreRequest, GetClusterRankingScoreResult>
        {
            public GetClusterRankingScoreTask(IGs2Session session, RestSessionRequestFactory factory, GetClusterRankingScoreRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(GetClusterRankingScoreRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "ranking2")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/me/score/cluster/{rankingName}/{clusterName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{rankingName}", !string.IsNullOrEmpty(request.RankingName) ? request.RankingName.ToString() : "null");
                url = url.Replace("{clusterName}", !string.IsNullOrEmpty(request.ClusterName) ? request.ClusterName.ToString() : "null");

                var sessionRequest = Factory.Get(url);
                if (request.ContextStack != null)
                {
                    sessionRequest.AddQueryString("contextStack", request.ContextStack);
                }
                if (request.Season != null) {
                    sessionRequest.AddQueryString("season", $"{request.Season}");
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
		public IEnumerator GetClusterRankingScore(
                Request.GetClusterRankingScoreRequest request,
                UnityAction<AsyncResult<Result.GetClusterRankingScoreResult>> callback
        ) =>
            new GetClusterRankingScoreTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.GetClusterRankingScoreResult> GetClusterRankingScoreFuture(
                Request.GetClusterRankingScoreRequest request
        ) =>
            new GetClusterRankingScoreTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.GetClusterRankingScoreResult> GetClusterRankingScoreAsync(
                Request.GetClusterRankingScoreRequest request
        ) =>
            new GetClusterRankingScoreTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.GetClusterRankingScoreResult>();
    #else
		public GetClusterRankingScoreTask GetClusterRankingScoreAsync(
                Request.GetClusterRankingScoreRequest request
        )
		{
			return new GetClusterRankingScoreTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.GetClusterRankingScoreResult> GetClusterRankingScoreAsync(
                Request.GetClusterRankingScoreRequest request
        ) =>
            new GetClusterRankingScoreTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class GetClusterRankingScoreByUserIdTask : Gs2RestSessionTask<GetClusterRankingScoreByUserIdRequest, GetClusterRankingScoreByUserIdResult>
        {
            public GetClusterRankingScoreByUserIdTask(IGs2Session session, RestSessionRequestFactory factory, GetClusterRankingScoreByUserIdRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(GetClusterRankingScoreByUserIdRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "ranking2")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/{userId}/score/cluster/{rankingName}/{clusterName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{rankingName}", !string.IsNullOrEmpty(request.RankingName) ? request.RankingName.ToString() : "null");
                url = url.Replace("{clusterName}", !string.IsNullOrEmpty(request.ClusterName) ? request.ClusterName.ToString() : "null");
                url = url.Replace("{userId}", !string.IsNullOrEmpty(request.UserId) ? request.UserId.ToString() : "null");

                var sessionRequest = Factory.Get(url);
                if (request.ContextStack != null)
                {
                    sessionRequest.AddQueryString("contextStack", request.ContextStack);
                }
                if (request.Season != null) {
                    sessionRequest.AddQueryString("season", $"{request.Season}");
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
		public IEnumerator GetClusterRankingScoreByUserId(
                Request.GetClusterRankingScoreByUserIdRequest request,
                UnityAction<AsyncResult<Result.GetClusterRankingScoreByUserIdResult>> callback
        ) =>
            new GetClusterRankingScoreByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.GetClusterRankingScoreByUserIdResult> GetClusterRankingScoreByUserIdFuture(
                Request.GetClusterRankingScoreByUserIdRequest request
        ) =>
            new GetClusterRankingScoreByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.GetClusterRankingScoreByUserIdResult> GetClusterRankingScoreByUserIdAsync(
                Request.GetClusterRankingScoreByUserIdRequest request
        ) =>
            new GetClusterRankingScoreByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.GetClusterRankingScoreByUserIdResult>();
    #else
		public GetClusterRankingScoreByUserIdTask GetClusterRankingScoreByUserIdAsync(
                Request.GetClusterRankingScoreByUserIdRequest request
        )
		{
			return new GetClusterRankingScoreByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.GetClusterRankingScoreByUserIdResult> GetClusterRankingScoreByUserIdAsync(
                Request.GetClusterRankingScoreByUserIdRequest request
        ) =>
            new GetClusterRankingScoreByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class DeleteClusterRankingScoreByUserIdTask : Gs2RestSessionTask<DeleteClusterRankingScoreByUserIdRequest, DeleteClusterRankingScoreByUserIdResult>
        {
            public DeleteClusterRankingScoreByUserIdTask(IGs2Session session, RestSessionRequestFactory factory, DeleteClusterRankingScoreByUserIdRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DeleteClusterRankingScoreByUserIdRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "ranking2")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/{userId}/score/cluster/{rankingName}/{clusterName}/{season}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{rankingName}", !string.IsNullOrEmpty(request.RankingName) ? request.RankingName.ToString() : "null");
                url = url.Replace("{clusterName}", !string.IsNullOrEmpty(request.ClusterName) ? request.ClusterName.ToString() : "null");
                url = url.Replace("{userId}", !string.IsNullOrEmpty(request.UserId) ? request.UserId.ToString() : "null");

                var sessionRequest = Factory.Delete(url);
                if (request.ContextStack != null)
                {
                    sessionRequest.AddQueryString("contextStack", request.ContextStack);
                }
                if (request.Season != null) {
                    sessionRequest.AddQueryString("season", $"{request.Season}");
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
		public IEnumerator DeleteClusterRankingScoreByUserId(
                Request.DeleteClusterRankingScoreByUserIdRequest request,
                UnityAction<AsyncResult<Result.DeleteClusterRankingScoreByUserIdResult>> callback
        ) =>
            new DeleteClusterRankingScoreByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.DeleteClusterRankingScoreByUserIdResult> DeleteClusterRankingScoreByUserIdFuture(
                Request.DeleteClusterRankingScoreByUserIdRequest request
        ) =>
            new DeleteClusterRankingScoreByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.DeleteClusterRankingScoreByUserIdResult> DeleteClusterRankingScoreByUserIdAsync(
                Request.DeleteClusterRankingScoreByUserIdRequest request
        ) =>
            new DeleteClusterRankingScoreByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.DeleteClusterRankingScoreByUserIdResult>();
    #else
		public DeleteClusterRankingScoreByUserIdTask DeleteClusterRankingScoreByUserIdAsync(
                Request.DeleteClusterRankingScoreByUserIdRequest request
        )
		{
			return new DeleteClusterRankingScoreByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.DeleteClusterRankingScoreByUserIdResult> DeleteClusterRankingScoreByUserIdAsync(
                Request.DeleteClusterRankingScoreByUserIdRequest request
        ) =>
            new DeleteClusterRankingScoreByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class VerifyClusterRankingScoreTask : Gs2RestSessionTask<VerifyClusterRankingScoreRequest, VerifyClusterRankingScoreResult>
        {
            public VerifyClusterRankingScoreTask(IGs2Session session, RestSessionRequestFactory factory, VerifyClusterRankingScoreRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(VerifyClusterRankingScoreRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "ranking2")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/{userId}/score/cluster/{rankingName}/{clusterName}/verify/{verifyType}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{rankingName}", !string.IsNullOrEmpty(request.RankingName) ? request.RankingName.ToString() : "null");
                url = url.Replace("{clusterName}", !string.IsNullOrEmpty(request.ClusterName) ? request.ClusterName.ToString() : "null");
                url = url.Replace("{verifyType}", !string.IsNullOrEmpty(request.VerifyType) ? request.VerifyType.ToString() : "null");

                var sessionRequest = Factory.Post(url);

                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);
                jsonWriter.WriteObjectStart();
                if (request.Season != null)
                {
                    jsonWriter.WritePropertyName("season");
                    jsonWriter.Write(request.Season.ToString());
                }
                if (request.Score != null)
                {
                    jsonWriter.WritePropertyName("score");
                    jsonWriter.Write(request.Score.ToString());
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
		public IEnumerator VerifyClusterRankingScore(
                Request.VerifyClusterRankingScoreRequest request,
                UnityAction<AsyncResult<Result.VerifyClusterRankingScoreResult>> callback
        ) =>
            new VerifyClusterRankingScoreTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.VerifyClusterRankingScoreResult> VerifyClusterRankingScoreFuture(
                Request.VerifyClusterRankingScoreRequest request
        ) =>
            new VerifyClusterRankingScoreTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.VerifyClusterRankingScoreResult> VerifyClusterRankingScoreAsync(
                Request.VerifyClusterRankingScoreRequest request
        ) =>
            new VerifyClusterRankingScoreTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.VerifyClusterRankingScoreResult>();
    #else
		public VerifyClusterRankingScoreTask VerifyClusterRankingScoreAsync(
                Request.VerifyClusterRankingScoreRequest request
        )
		{
			return new VerifyClusterRankingScoreTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.VerifyClusterRankingScoreResult> VerifyClusterRankingScoreAsync(
                Request.VerifyClusterRankingScoreRequest request
        ) =>
            new VerifyClusterRankingScoreTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class VerifyClusterRankingScoreByUserIdTask : Gs2RestSessionTask<VerifyClusterRankingScoreByUserIdRequest, VerifyClusterRankingScoreByUserIdResult>
        {
            public VerifyClusterRankingScoreByUserIdTask(IGs2Session session, RestSessionRequestFactory factory, VerifyClusterRankingScoreByUserIdRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(VerifyClusterRankingScoreByUserIdRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "ranking2")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/{userId}/score/cluster/{rankingName}/{clusterName}/verify/{verifyType}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{userId}", !string.IsNullOrEmpty(request.UserId) ? request.UserId.ToString() : "null");
                url = url.Replace("{rankingName}", !string.IsNullOrEmpty(request.RankingName) ? request.RankingName.ToString() : "null");
                url = url.Replace("{clusterName}", !string.IsNullOrEmpty(request.ClusterName) ? request.ClusterName.ToString() : "null");
                url = url.Replace("{verifyType}", !string.IsNullOrEmpty(request.VerifyType) ? request.VerifyType.ToString() : "null");

                var sessionRequest = Factory.Post(url);

                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);
                jsonWriter.WriteObjectStart();
                if (request.Season != null)
                {
                    jsonWriter.WritePropertyName("season");
                    jsonWriter.Write(request.Season.ToString());
                }
                if (request.Score != null)
                {
                    jsonWriter.WritePropertyName("score");
                    jsonWriter.Write(request.Score.ToString());
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
		public IEnumerator VerifyClusterRankingScoreByUserId(
                Request.VerifyClusterRankingScoreByUserIdRequest request,
                UnityAction<AsyncResult<Result.VerifyClusterRankingScoreByUserIdResult>> callback
        ) =>
            new VerifyClusterRankingScoreByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.VerifyClusterRankingScoreByUserIdResult> VerifyClusterRankingScoreByUserIdFuture(
                Request.VerifyClusterRankingScoreByUserIdRequest request
        ) =>
            new VerifyClusterRankingScoreByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.VerifyClusterRankingScoreByUserIdResult> VerifyClusterRankingScoreByUserIdAsync(
                Request.VerifyClusterRankingScoreByUserIdRequest request
        ) =>
            new VerifyClusterRankingScoreByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.VerifyClusterRankingScoreByUserIdResult>();
    #else
		public VerifyClusterRankingScoreByUserIdTask VerifyClusterRankingScoreByUserIdAsync(
                Request.VerifyClusterRankingScoreByUserIdRequest request
        )
		{
			return new VerifyClusterRankingScoreByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.VerifyClusterRankingScoreByUserIdResult> VerifyClusterRankingScoreByUserIdAsync(
                Request.VerifyClusterRankingScoreByUserIdRequest request
        ) =>
            new VerifyClusterRankingScoreByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class VerifyClusterRankingScoreByStampTaskTask : Gs2RestSessionTask<VerifyClusterRankingScoreByStampTaskRequest, VerifyClusterRankingScoreByStampTaskResult>
        {
            public VerifyClusterRankingScoreByStampTaskTask(IGs2Session session, RestSessionRequestFactory factory, VerifyClusterRankingScoreByStampTaskRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(VerifyClusterRankingScoreByStampTaskRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "ranking2")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/stamp/cluster/score/verify";

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
		public IEnumerator VerifyClusterRankingScoreByStampTask(
                Request.VerifyClusterRankingScoreByStampTaskRequest request,
                UnityAction<AsyncResult<Result.VerifyClusterRankingScoreByStampTaskResult>> callback
        ) =>
            new VerifyClusterRankingScoreByStampTaskTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.VerifyClusterRankingScoreByStampTaskResult> VerifyClusterRankingScoreByStampTaskFuture(
                Request.VerifyClusterRankingScoreByStampTaskRequest request
        ) =>
            new VerifyClusterRankingScoreByStampTaskTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.VerifyClusterRankingScoreByStampTaskResult> VerifyClusterRankingScoreByStampTaskAsync(
                Request.VerifyClusterRankingScoreByStampTaskRequest request
        ) =>
            new VerifyClusterRankingScoreByStampTaskTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.VerifyClusterRankingScoreByStampTaskResult>();
    #else
		public VerifyClusterRankingScoreByStampTaskTask VerifyClusterRankingScoreByStampTaskAsync(
                Request.VerifyClusterRankingScoreByStampTaskRequest request
        )
		{
			return new VerifyClusterRankingScoreByStampTaskTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.VerifyClusterRankingScoreByStampTaskResult> VerifyClusterRankingScoreByStampTaskAsync(
                Request.VerifyClusterRankingScoreByStampTaskRequest request
        ) =>
            new VerifyClusterRankingScoreByStampTaskTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class DescribeClusterRankingReceivedRewardsTask : Gs2RestSessionTask<DescribeClusterRankingReceivedRewardsRequest, DescribeClusterRankingReceivedRewardsResult>
        {
            public DescribeClusterRankingReceivedRewardsTask(IGs2Session session, RestSessionRequestFactory factory, DescribeClusterRankingReceivedRewardsRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DescribeClusterRankingReceivedRewardsRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "ranking2")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/me/cluster/reward/received";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");

                var sessionRequest = Factory.Get(url);
                if (request.ContextStack != null)
                {
                    sessionRequest.AddQueryString("contextStack", request.ContextStack);
                }
                if (request.RankingName != null) {
                    sessionRequest.AddQueryString("rankingName", $"{request.RankingName}");
                }
                if (request.ClusterName != null) {
                    sessionRequest.AddQueryString("clusterName", $"{request.ClusterName}");
                }
                if (request.Season != null) {
                    sessionRequest.AddQueryString("season", $"{request.Season}");
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
		public IEnumerator DescribeClusterRankingReceivedRewards(
                Request.DescribeClusterRankingReceivedRewardsRequest request,
                UnityAction<AsyncResult<Result.DescribeClusterRankingReceivedRewardsResult>> callback
        ) =>
            new DescribeClusterRankingReceivedRewardsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.DescribeClusterRankingReceivedRewardsResult> DescribeClusterRankingReceivedRewardsFuture(
                Request.DescribeClusterRankingReceivedRewardsRequest request
        ) =>
            new DescribeClusterRankingReceivedRewardsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.DescribeClusterRankingReceivedRewardsResult> DescribeClusterRankingReceivedRewardsAsync(
                Request.DescribeClusterRankingReceivedRewardsRequest request
        ) =>
            new DescribeClusterRankingReceivedRewardsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.DescribeClusterRankingReceivedRewardsResult>();
    #else
		public DescribeClusterRankingReceivedRewardsTask DescribeClusterRankingReceivedRewardsAsync(
                Request.DescribeClusterRankingReceivedRewardsRequest request
        )
		{
			return new DescribeClusterRankingReceivedRewardsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.DescribeClusterRankingReceivedRewardsResult> DescribeClusterRankingReceivedRewardsAsync(
                Request.DescribeClusterRankingReceivedRewardsRequest request
        ) =>
            new DescribeClusterRankingReceivedRewardsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class DescribeClusterRankingReceivedRewardsByUserIdTask : Gs2RestSessionTask<DescribeClusterRankingReceivedRewardsByUserIdRequest, DescribeClusterRankingReceivedRewardsByUserIdResult>
        {
            public DescribeClusterRankingReceivedRewardsByUserIdTask(IGs2Session session, RestSessionRequestFactory factory, DescribeClusterRankingReceivedRewardsByUserIdRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DescribeClusterRankingReceivedRewardsByUserIdRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "ranking2")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/{userId}/cluster/reward/received";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{userId}", !string.IsNullOrEmpty(request.UserId) ? request.UserId.ToString() : "null");

                var sessionRequest = Factory.Get(url);
                if (request.ContextStack != null)
                {
                    sessionRequest.AddQueryString("contextStack", request.ContextStack);
                }
                if (request.RankingName != null) {
                    sessionRequest.AddQueryString("rankingName", $"{request.RankingName}");
                }
                if (request.ClusterName != null) {
                    sessionRequest.AddQueryString("clusterName", $"{request.ClusterName}");
                }
                if (request.Season != null) {
                    sessionRequest.AddQueryString("season", $"{request.Season}");
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
		public IEnumerator DescribeClusterRankingReceivedRewardsByUserId(
                Request.DescribeClusterRankingReceivedRewardsByUserIdRequest request,
                UnityAction<AsyncResult<Result.DescribeClusterRankingReceivedRewardsByUserIdResult>> callback
        ) =>
            new DescribeClusterRankingReceivedRewardsByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.DescribeClusterRankingReceivedRewardsByUserIdResult> DescribeClusterRankingReceivedRewardsByUserIdFuture(
                Request.DescribeClusterRankingReceivedRewardsByUserIdRequest request
        ) =>
            new DescribeClusterRankingReceivedRewardsByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.DescribeClusterRankingReceivedRewardsByUserIdResult> DescribeClusterRankingReceivedRewardsByUserIdAsync(
                Request.DescribeClusterRankingReceivedRewardsByUserIdRequest request
        ) =>
            new DescribeClusterRankingReceivedRewardsByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.DescribeClusterRankingReceivedRewardsByUserIdResult>();
    #else
		public DescribeClusterRankingReceivedRewardsByUserIdTask DescribeClusterRankingReceivedRewardsByUserIdAsync(
                Request.DescribeClusterRankingReceivedRewardsByUserIdRequest request
        )
		{
			return new DescribeClusterRankingReceivedRewardsByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.DescribeClusterRankingReceivedRewardsByUserIdResult> DescribeClusterRankingReceivedRewardsByUserIdAsync(
                Request.DescribeClusterRankingReceivedRewardsByUserIdRequest request
        ) =>
            new DescribeClusterRankingReceivedRewardsByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class CreateClusterRankingReceivedRewardTask : Gs2RestSessionTask<CreateClusterRankingReceivedRewardRequest, CreateClusterRankingReceivedRewardResult>
        {
            public CreateClusterRankingReceivedRewardTask(IGs2Session session, RestSessionRequestFactory factory, CreateClusterRankingReceivedRewardRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(CreateClusterRankingReceivedRewardRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "ranking2")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/me/cluster/reward/received/{rankingName}/{clusterName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{rankingName}", !string.IsNullOrEmpty(request.RankingName) ? request.RankingName.ToString() : "null");
                url = url.Replace("{clusterName}", !string.IsNullOrEmpty(request.ClusterName) ? request.ClusterName.ToString() : "null");

                var sessionRequest = Factory.Post(url);

                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);
                jsonWriter.WriteObjectStart();
                if (request.Season != null)
                {
                    jsonWriter.WritePropertyName("season");
                    jsonWriter.Write(request.Season.ToString());
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
		public IEnumerator CreateClusterRankingReceivedReward(
                Request.CreateClusterRankingReceivedRewardRequest request,
                UnityAction<AsyncResult<Result.CreateClusterRankingReceivedRewardResult>> callback
        ) =>
            new CreateClusterRankingReceivedRewardTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.CreateClusterRankingReceivedRewardResult> CreateClusterRankingReceivedRewardFuture(
                Request.CreateClusterRankingReceivedRewardRequest request
        ) =>
            new CreateClusterRankingReceivedRewardTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.CreateClusterRankingReceivedRewardResult> CreateClusterRankingReceivedRewardAsync(
                Request.CreateClusterRankingReceivedRewardRequest request
        ) =>
            new CreateClusterRankingReceivedRewardTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.CreateClusterRankingReceivedRewardResult>();
    #else
		public CreateClusterRankingReceivedRewardTask CreateClusterRankingReceivedRewardAsync(
                Request.CreateClusterRankingReceivedRewardRequest request
        )
		{
			return new CreateClusterRankingReceivedRewardTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.CreateClusterRankingReceivedRewardResult> CreateClusterRankingReceivedRewardAsync(
                Request.CreateClusterRankingReceivedRewardRequest request
        ) =>
            new CreateClusterRankingReceivedRewardTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class CreateClusterRankingReceivedRewardByUserIdTask : Gs2RestSessionTask<CreateClusterRankingReceivedRewardByUserIdRequest, CreateClusterRankingReceivedRewardByUserIdResult>
        {
            public CreateClusterRankingReceivedRewardByUserIdTask(IGs2Session session, RestSessionRequestFactory factory, CreateClusterRankingReceivedRewardByUserIdRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(CreateClusterRankingReceivedRewardByUserIdRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "ranking2")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/{userId}/cluster/reward/received/{rankingName}/{clusterName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{rankingName}", !string.IsNullOrEmpty(request.RankingName) ? request.RankingName.ToString() : "null");
                url = url.Replace("{clusterName}", !string.IsNullOrEmpty(request.ClusterName) ? request.ClusterName.ToString() : "null");
                url = url.Replace("{userId}", !string.IsNullOrEmpty(request.UserId) ? request.UserId.ToString() : "null");

                var sessionRequest = Factory.Post(url);

                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);
                jsonWriter.WriteObjectStart();
                if (request.Season != null)
                {
                    jsonWriter.WritePropertyName("season");
                    jsonWriter.Write(request.Season.ToString());
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
		public IEnumerator CreateClusterRankingReceivedRewardByUserId(
                Request.CreateClusterRankingReceivedRewardByUserIdRequest request,
                UnityAction<AsyncResult<Result.CreateClusterRankingReceivedRewardByUserIdResult>> callback
        ) =>
            new CreateClusterRankingReceivedRewardByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.CreateClusterRankingReceivedRewardByUserIdResult> CreateClusterRankingReceivedRewardByUserIdFuture(
                Request.CreateClusterRankingReceivedRewardByUserIdRequest request
        ) =>
            new CreateClusterRankingReceivedRewardByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.CreateClusterRankingReceivedRewardByUserIdResult> CreateClusterRankingReceivedRewardByUserIdAsync(
                Request.CreateClusterRankingReceivedRewardByUserIdRequest request
        ) =>
            new CreateClusterRankingReceivedRewardByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.CreateClusterRankingReceivedRewardByUserIdResult>();
    #else
		public CreateClusterRankingReceivedRewardByUserIdTask CreateClusterRankingReceivedRewardByUserIdAsync(
                Request.CreateClusterRankingReceivedRewardByUserIdRequest request
        )
		{
			return new CreateClusterRankingReceivedRewardByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.CreateClusterRankingReceivedRewardByUserIdResult> CreateClusterRankingReceivedRewardByUserIdAsync(
                Request.CreateClusterRankingReceivedRewardByUserIdRequest request
        ) =>
            new CreateClusterRankingReceivedRewardByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class ReceiveClusterRankingReceivedRewardTask : Gs2RestSessionTask<ReceiveClusterRankingReceivedRewardRequest, ReceiveClusterRankingReceivedRewardResult>
        {
            public ReceiveClusterRankingReceivedRewardTask(IGs2Session session, RestSessionRequestFactory factory, ReceiveClusterRankingReceivedRewardRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(ReceiveClusterRankingReceivedRewardRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "ranking2")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/me/cluster/reward/received/{rankingName}/{clusterName}/{season}/reward/receive";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{rankingName}", !string.IsNullOrEmpty(request.RankingName) ? request.RankingName.ToString() : "null");
                url = url.Replace("{clusterName}", !string.IsNullOrEmpty(request.ClusterName) ? request.ClusterName.ToString() : "null");
                url = url.Replace("{season}",request.Season != null ? request.Season.ToString() : "null");

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
		public IEnumerator ReceiveClusterRankingReceivedReward(
                Request.ReceiveClusterRankingReceivedRewardRequest request,
                UnityAction<AsyncResult<Result.ReceiveClusterRankingReceivedRewardResult>> callback
        ) =>
            new ReceiveClusterRankingReceivedRewardTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.ReceiveClusterRankingReceivedRewardResult> ReceiveClusterRankingReceivedRewardFuture(
                Request.ReceiveClusterRankingReceivedRewardRequest request
        ) =>
            new ReceiveClusterRankingReceivedRewardTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.ReceiveClusterRankingReceivedRewardResult> ReceiveClusterRankingReceivedRewardAsync(
                Request.ReceiveClusterRankingReceivedRewardRequest request
        ) =>
            new ReceiveClusterRankingReceivedRewardTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.ReceiveClusterRankingReceivedRewardResult>();
    #else
		public ReceiveClusterRankingReceivedRewardTask ReceiveClusterRankingReceivedRewardAsync(
                Request.ReceiveClusterRankingReceivedRewardRequest request
        )
		{
			return new ReceiveClusterRankingReceivedRewardTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.ReceiveClusterRankingReceivedRewardResult> ReceiveClusterRankingReceivedRewardAsync(
                Request.ReceiveClusterRankingReceivedRewardRequest request
        ) =>
            new ReceiveClusterRankingReceivedRewardTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class ReceiveClusterRankingReceivedRewardByUserIdTask : Gs2RestSessionTask<ReceiveClusterRankingReceivedRewardByUserIdRequest, ReceiveClusterRankingReceivedRewardByUserIdResult>
        {
            public ReceiveClusterRankingReceivedRewardByUserIdTask(IGs2Session session, RestSessionRequestFactory factory, ReceiveClusterRankingReceivedRewardByUserIdRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(ReceiveClusterRankingReceivedRewardByUserIdRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "ranking2")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/{userId}/cluster/reward/received/{rankingName}/{clusterName}/{season}/reward/receive";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{userId}", !string.IsNullOrEmpty(request.UserId) ? request.UserId.ToString() : "null");
                url = url.Replace("{rankingName}", !string.IsNullOrEmpty(request.RankingName) ? request.RankingName.ToString() : "null");
                url = url.Replace("{clusterName}", !string.IsNullOrEmpty(request.ClusterName) ? request.ClusterName.ToString() : "null");
                url = url.Replace("{season}",request.Season != null ? request.Season.ToString() : "null");

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
		public IEnumerator ReceiveClusterRankingReceivedRewardByUserId(
                Request.ReceiveClusterRankingReceivedRewardByUserIdRequest request,
                UnityAction<AsyncResult<Result.ReceiveClusterRankingReceivedRewardByUserIdResult>> callback
        ) =>
            new ReceiveClusterRankingReceivedRewardByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.ReceiveClusterRankingReceivedRewardByUserIdResult> ReceiveClusterRankingReceivedRewardByUserIdFuture(
                Request.ReceiveClusterRankingReceivedRewardByUserIdRequest request
        ) =>
            new ReceiveClusterRankingReceivedRewardByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.ReceiveClusterRankingReceivedRewardByUserIdResult> ReceiveClusterRankingReceivedRewardByUserIdAsync(
                Request.ReceiveClusterRankingReceivedRewardByUserIdRequest request
        ) =>
            new ReceiveClusterRankingReceivedRewardByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.ReceiveClusterRankingReceivedRewardByUserIdResult>();
    #else
		public ReceiveClusterRankingReceivedRewardByUserIdTask ReceiveClusterRankingReceivedRewardByUserIdAsync(
                Request.ReceiveClusterRankingReceivedRewardByUserIdRequest request
        )
		{
			return new ReceiveClusterRankingReceivedRewardByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.ReceiveClusterRankingReceivedRewardByUserIdResult> ReceiveClusterRankingReceivedRewardByUserIdAsync(
                Request.ReceiveClusterRankingReceivedRewardByUserIdRequest request
        ) =>
            new ReceiveClusterRankingReceivedRewardByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class GetClusterRankingReceivedRewardTask : Gs2RestSessionTask<GetClusterRankingReceivedRewardRequest, GetClusterRankingReceivedRewardResult>
        {
            public GetClusterRankingReceivedRewardTask(IGs2Session session, RestSessionRequestFactory factory, GetClusterRankingReceivedRewardRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(GetClusterRankingReceivedRewardRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "ranking2")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/me/cluster/reward/received/{rankingName}/{clusterName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{rankingName}", !string.IsNullOrEmpty(request.RankingName) ? request.RankingName.ToString() : "null");
                url = url.Replace("{clusterName}", !string.IsNullOrEmpty(request.ClusterName) ? request.ClusterName.ToString() : "null");

                var sessionRequest = Factory.Get(url);
                if (request.ContextStack != null)
                {
                    sessionRequest.AddQueryString("contextStack", request.ContextStack);
                }
                if (request.Season != null) {
                    sessionRequest.AddQueryString("season", $"{request.Season}");
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
		public IEnumerator GetClusterRankingReceivedReward(
                Request.GetClusterRankingReceivedRewardRequest request,
                UnityAction<AsyncResult<Result.GetClusterRankingReceivedRewardResult>> callback
        ) =>
            new GetClusterRankingReceivedRewardTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.GetClusterRankingReceivedRewardResult> GetClusterRankingReceivedRewardFuture(
                Request.GetClusterRankingReceivedRewardRequest request
        ) =>
            new GetClusterRankingReceivedRewardTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.GetClusterRankingReceivedRewardResult> GetClusterRankingReceivedRewardAsync(
                Request.GetClusterRankingReceivedRewardRequest request
        ) =>
            new GetClusterRankingReceivedRewardTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.GetClusterRankingReceivedRewardResult>();
    #else
		public GetClusterRankingReceivedRewardTask GetClusterRankingReceivedRewardAsync(
                Request.GetClusterRankingReceivedRewardRequest request
        )
		{
			return new GetClusterRankingReceivedRewardTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.GetClusterRankingReceivedRewardResult> GetClusterRankingReceivedRewardAsync(
                Request.GetClusterRankingReceivedRewardRequest request
        ) =>
            new GetClusterRankingReceivedRewardTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class GetClusterRankingReceivedRewardByUserIdTask : Gs2RestSessionTask<GetClusterRankingReceivedRewardByUserIdRequest, GetClusterRankingReceivedRewardByUserIdResult>
        {
            public GetClusterRankingReceivedRewardByUserIdTask(IGs2Session session, RestSessionRequestFactory factory, GetClusterRankingReceivedRewardByUserIdRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(GetClusterRankingReceivedRewardByUserIdRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "ranking2")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/{userId}/cluster/reward/received/{rankingName}/{clusterName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{rankingName}", !string.IsNullOrEmpty(request.RankingName) ? request.RankingName.ToString() : "null");
                url = url.Replace("{clusterName}", !string.IsNullOrEmpty(request.ClusterName) ? request.ClusterName.ToString() : "null");
                url = url.Replace("{userId}", !string.IsNullOrEmpty(request.UserId) ? request.UserId.ToString() : "null");

                var sessionRequest = Factory.Get(url);
                if (request.ContextStack != null)
                {
                    sessionRequest.AddQueryString("contextStack", request.ContextStack);
                }
                if (request.Season != null) {
                    sessionRequest.AddQueryString("season", $"{request.Season}");
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
		public IEnumerator GetClusterRankingReceivedRewardByUserId(
                Request.GetClusterRankingReceivedRewardByUserIdRequest request,
                UnityAction<AsyncResult<Result.GetClusterRankingReceivedRewardByUserIdResult>> callback
        ) =>
            new GetClusterRankingReceivedRewardByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.GetClusterRankingReceivedRewardByUserIdResult> GetClusterRankingReceivedRewardByUserIdFuture(
                Request.GetClusterRankingReceivedRewardByUserIdRequest request
        ) =>
            new GetClusterRankingReceivedRewardByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.GetClusterRankingReceivedRewardByUserIdResult> GetClusterRankingReceivedRewardByUserIdAsync(
                Request.GetClusterRankingReceivedRewardByUserIdRequest request
        ) =>
            new GetClusterRankingReceivedRewardByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.GetClusterRankingReceivedRewardByUserIdResult>();
    #else
		public GetClusterRankingReceivedRewardByUserIdTask GetClusterRankingReceivedRewardByUserIdAsync(
                Request.GetClusterRankingReceivedRewardByUserIdRequest request
        )
		{
			return new GetClusterRankingReceivedRewardByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.GetClusterRankingReceivedRewardByUserIdResult> GetClusterRankingReceivedRewardByUserIdAsync(
                Request.GetClusterRankingReceivedRewardByUserIdRequest request
        ) =>
            new GetClusterRankingReceivedRewardByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class DeleteClusterRankingReceivedRewardByUserIdTask : Gs2RestSessionTask<DeleteClusterRankingReceivedRewardByUserIdRequest, DeleteClusterRankingReceivedRewardByUserIdResult>
        {
            public DeleteClusterRankingReceivedRewardByUserIdTask(IGs2Session session, RestSessionRequestFactory factory, DeleteClusterRankingReceivedRewardByUserIdRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DeleteClusterRankingReceivedRewardByUserIdRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "ranking2")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/{userId}/cluster/reward/received/{rankingName}/{clusterName}/{season}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{rankingName}", !string.IsNullOrEmpty(request.RankingName) ? request.RankingName.ToString() : "null");
                url = url.Replace("{clusterName}", !string.IsNullOrEmpty(request.ClusterName) ? request.ClusterName.ToString() : "null");
                url = url.Replace("{userId}", !string.IsNullOrEmpty(request.UserId) ? request.UserId.ToString() : "null");

                var sessionRequest = Factory.Delete(url);
                if (request.ContextStack != null)
                {
                    sessionRequest.AddQueryString("contextStack", request.ContextStack);
                }
                if (request.Season != null) {
                    sessionRequest.AddQueryString("season", $"{request.Season}");
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
		public IEnumerator DeleteClusterRankingReceivedRewardByUserId(
                Request.DeleteClusterRankingReceivedRewardByUserIdRequest request,
                UnityAction<AsyncResult<Result.DeleteClusterRankingReceivedRewardByUserIdResult>> callback
        ) =>
            new DeleteClusterRankingReceivedRewardByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.DeleteClusterRankingReceivedRewardByUserIdResult> DeleteClusterRankingReceivedRewardByUserIdFuture(
                Request.DeleteClusterRankingReceivedRewardByUserIdRequest request
        ) =>
            new DeleteClusterRankingReceivedRewardByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.DeleteClusterRankingReceivedRewardByUserIdResult> DeleteClusterRankingReceivedRewardByUserIdAsync(
                Request.DeleteClusterRankingReceivedRewardByUserIdRequest request
        ) =>
            new DeleteClusterRankingReceivedRewardByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.DeleteClusterRankingReceivedRewardByUserIdResult>();
    #else
		public DeleteClusterRankingReceivedRewardByUserIdTask DeleteClusterRankingReceivedRewardByUserIdAsync(
                Request.DeleteClusterRankingReceivedRewardByUserIdRequest request
        )
		{
			return new DeleteClusterRankingReceivedRewardByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.DeleteClusterRankingReceivedRewardByUserIdResult> DeleteClusterRankingReceivedRewardByUserIdAsync(
                Request.DeleteClusterRankingReceivedRewardByUserIdRequest request
        ) =>
            new DeleteClusterRankingReceivedRewardByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class CreateClusterRankingReceivedRewardByStampTaskTask : Gs2RestSessionTask<CreateClusterRankingReceivedRewardByStampTaskRequest, CreateClusterRankingReceivedRewardByStampTaskResult>
        {
            public CreateClusterRankingReceivedRewardByStampTaskTask(IGs2Session session, RestSessionRequestFactory factory, CreateClusterRankingReceivedRewardByStampTaskRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(CreateClusterRankingReceivedRewardByStampTaskRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "ranking2")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/stamp/ranking/cluster/reward/receive";

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
		public IEnumerator CreateClusterRankingReceivedRewardByStampTask(
                Request.CreateClusterRankingReceivedRewardByStampTaskRequest request,
                UnityAction<AsyncResult<Result.CreateClusterRankingReceivedRewardByStampTaskResult>> callback
        ) =>
            new CreateClusterRankingReceivedRewardByStampTaskTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.CreateClusterRankingReceivedRewardByStampTaskResult> CreateClusterRankingReceivedRewardByStampTaskFuture(
                Request.CreateClusterRankingReceivedRewardByStampTaskRequest request
        ) =>
            new CreateClusterRankingReceivedRewardByStampTaskTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.CreateClusterRankingReceivedRewardByStampTaskResult> CreateClusterRankingReceivedRewardByStampTaskAsync(
                Request.CreateClusterRankingReceivedRewardByStampTaskRequest request
        ) =>
            new CreateClusterRankingReceivedRewardByStampTaskTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.CreateClusterRankingReceivedRewardByStampTaskResult>();
    #else
		public CreateClusterRankingReceivedRewardByStampTaskTask CreateClusterRankingReceivedRewardByStampTaskAsync(
                Request.CreateClusterRankingReceivedRewardByStampTaskRequest request
        )
		{
			return new CreateClusterRankingReceivedRewardByStampTaskTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.CreateClusterRankingReceivedRewardByStampTaskResult> CreateClusterRankingReceivedRewardByStampTaskAsync(
                Request.CreateClusterRankingReceivedRewardByStampTaskRequest request
        ) =>
            new CreateClusterRankingReceivedRewardByStampTaskTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class DescribeClusterRankingsTask : Gs2RestSessionTask<DescribeClusterRankingsRequest, DescribeClusterRankingsResult>
        {
            public DescribeClusterRankingsTask(IGs2Session session, RestSessionRequestFactory factory, DescribeClusterRankingsRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DescribeClusterRankingsRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "ranking2")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/ranking/cluster/{rankingName}/{clusterName}/user/me";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{rankingName}", !string.IsNullOrEmpty(request.RankingName) ? request.RankingName.ToString() : "null");
                url = url.Replace("{clusterName}", !string.IsNullOrEmpty(request.ClusterName) ? request.ClusterName.ToString() : "null");

                var sessionRequest = Factory.Get(url);
                if (request.ContextStack != null)
                {
                    sessionRequest.AddQueryString("contextStack", request.ContextStack);
                }
                if (request.Season != null) {
                    sessionRequest.AddQueryString("season", $"{request.Season}");
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
		public IEnumerator DescribeClusterRankings(
                Request.DescribeClusterRankingsRequest request,
                UnityAction<AsyncResult<Result.DescribeClusterRankingsResult>> callback
        ) =>
            new DescribeClusterRankingsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.DescribeClusterRankingsResult> DescribeClusterRankingsFuture(
                Request.DescribeClusterRankingsRequest request
        ) =>
            new DescribeClusterRankingsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.DescribeClusterRankingsResult> DescribeClusterRankingsAsync(
                Request.DescribeClusterRankingsRequest request
        ) =>
            new DescribeClusterRankingsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.DescribeClusterRankingsResult>();
    #else
		public DescribeClusterRankingsTask DescribeClusterRankingsAsync(
                Request.DescribeClusterRankingsRequest request
        )
		{
			return new DescribeClusterRankingsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.DescribeClusterRankingsResult> DescribeClusterRankingsAsync(
                Request.DescribeClusterRankingsRequest request
        ) =>
            new DescribeClusterRankingsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class DescribeClusterRankingsByUserIdTask : Gs2RestSessionTask<DescribeClusterRankingsByUserIdRequest, DescribeClusterRankingsByUserIdResult>
        {
            public DescribeClusterRankingsByUserIdTask(IGs2Session session, RestSessionRequestFactory factory, DescribeClusterRankingsByUserIdRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DescribeClusterRankingsByUserIdRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "ranking2")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/ranking/cluster/{rankingName}/{clusterName}/user/{userId}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{userId}", !string.IsNullOrEmpty(request.UserId) ? request.UserId.ToString() : "null");
                url = url.Replace("{rankingName}", !string.IsNullOrEmpty(request.RankingName) ? request.RankingName.ToString() : "null");
                url = url.Replace("{clusterName}", !string.IsNullOrEmpty(request.ClusterName) ? request.ClusterName.ToString() : "null");

                var sessionRequest = Factory.Get(url);
                if (request.ContextStack != null)
                {
                    sessionRequest.AddQueryString("contextStack", request.ContextStack);
                }
                if (request.Season != null) {
                    sessionRequest.AddQueryString("season", $"{request.Season}");
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
		public IEnumerator DescribeClusterRankingsByUserId(
                Request.DescribeClusterRankingsByUserIdRequest request,
                UnityAction<AsyncResult<Result.DescribeClusterRankingsByUserIdResult>> callback
        ) =>
            new DescribeClusterRankingsByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.DescribeClusterRankingsByUserIdResult> DescribeClusterRankingsByUserIdFuture(
                Request.DescribeClusterRankingsByUserIdRequest request
        ) =>
            new DescribeClusterRankingsByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.DescribeClusterRankingsByUserIdResult> DescribeClusterRankingsByUserIdAsync(
                Request.DescribeClusterRankingsByUserIdRequest request
        ) =>
            new DescribeClusterRankingsByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.DescribeClusterRankingsByUserIdResult>();
    #else
		public DescribeClusterRankingsByUserIdTask DescribeClusterRankingsByUserIdAsync(
                Request.DescribeClusterRankingsByUserIdRequest request
        )
		{
			return new DescribeClusterRankingsByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.DescribeClusterRankingsByUserIdResult> DescribeClusterRankingsByUserIdAsync(
                Request.DescribeClusterRankingsByUserIdRequest request
        ) =>
            new DescribeClusterRankingsByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class GetClusterRankingTask : Gs2RestSessionTask<GetClusterRankingRequest, GetClusterRankingResult>
        {
            public GetClusterRankingTask(IGs2Session session, RestSessionRequestFactory factory, GetClusterRankingRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(GetClusterRankingRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "ranking2")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/ranking/cluster/{rankingName}/{clusterName}/user/me/rank";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{rankingName}", !string.IsNullOrEmpty(request.RankingName) ? request.RankingName.ToString() : "null");
                url = url.Replace("{clusterName}", !string.IsNullOrEmpty(request.ClusterName) ? request.ClusterName.ToString() : "null");

                var sessionRequest = Factory.Get(url);
                if (request.ContextStack != null)
                {
                    sessionRequest.AddQueryString("contextStack", request.ContextStack);
                }
                if (request.Season != null) {
                    sessionRequest.AddQueryString("season", $"{request.Season}");
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
		public IEnumerator GetClusterRanking(
                Request.GetClusterRankingRequest request,
                UnityAction<AsyncResult<Result.GetClusterRankingResult>> callback
        ) =>
            new GetClusterRankingTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.GetClusterRankingResult> GetClusterRankingFuture(
                Request.GetClusterRankingRequest request
        ) =>
            new GetClusterRankingTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.GetClusterRankingResult> GetClusterRankingAsync(
                Request.GetClusterRankingRequest request
        ) =>
            new GetClusterRankingTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.GetClusterRankingResult>();
    #else
		public GetClusterRankingTask GetClusterRankingAsync(
                Request.GetClusterRankingRequest request
        )
		{
			return new GetClusterRankingTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.GetClusterRankingResult> GetClusterRankingAsync(
                Request.GetClusterRankingRequest request
        ) =>
            new GetClusterRankingTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class GetClusterRankingByUserIdTask : Gs2RestSessionTask<GetClusterRankingByUserIdRequest, GetClusterRankingByUserIdResult>
        {
            public GetClusterRankingByUserIdTask(IGs2Session session, RestSessionRequestFactory factory, GetClusterRankingByUserIdRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(GetClusterRankingByUserIdRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "ranking2")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/ranking/cluster/{rankingName}/{clusterName}/user/{userId}/rank";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{rankingName}", !string.IsNullOrEmpty(request.RankingName) ? request.RankingName.ToString() : "null");
                url = url.Replace("{clusterName}", !string.IsNullOrEmpty(request.ClusterName) ? request.ClusterName.ToString() : "null");
                url = url.Replace("{userId}", !string.IsNullOrEmpty(request.UserId) ? request.UserId.ToString() : "null");

                var sessionRequest = Factory.Get(url);
                if (request.ContextStack != null)
                {
                    sessionRequest.AddQueryString("contextStack", request.ContextStack);
                }
                if (request.Season != null) {
                    sessionRequest.AddQueryString("season", $"{request.Season}");
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
		public IEnumerator GetClusterRankingByUserId(
                Request.GetClusterRankingByUserIdRequest request,
                UnityAction<AsyncResult<Result.GetClusterRankingByUserIdResult>> callback
        ) =>
            new GetClusterRankingByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.GetClusterRankingByUserIdResult> GetClusterRankingByUserIdFuture(
                Request.GetClusterRankingByUserIdRequest request
        ) =>
            new GetClusterRankingByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.GetClusterRankingByUserIdResult> GetClusterRankingByUserIdAsync(
                Request.GetClusterRankingByUserIdRequest request
        ) =>
            new GetClusterRankingByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.GetClusterRankingByUserIdResult>();
    #else
		public GetClusterRankingByUserIdTask GetClusterRankingByUserIdAsync(
                Request.GetClusterRankingByUserIdRequest request
        )
		{
			return new GetClusterRankingByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.GetClusterRankingByUserIdResult> GetClusterRankingByUserIdAsync(
                Request.GetClusterRankingByUserIdRequest request
        ) =>
            new GetClusterRankingByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class DescribeSubscribeRankingModelsTask : Gs2RestSessionTask<DescribeSubscribeRankingModelsRequest, DescribeSubscribeRankingModelsResult>
        {
            public DescribeSubscribeRankingModelsTask(IGs2Session session, RestSessionRequestFactory factory, DescribeSubscribeRankingModelsRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DescribeSubscribeRankingModelsRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "ranking2")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/model/subscribe";

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
		public IEnumerator DescribeSubscribeRankingModels(
                Request.DescribeSubscribeRankingModelsRequest request,
                UnityAction<AsyncResult<Result.DescribeSubscribeRankingModelsResult>> callback
        ) =>
            new DescribeSubscribeRankingModelsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.DescribeSubscribeRankingModelsResult> DescribeSubscribeRankingModelsFuture(
                Request.DescribeSubscribeRankingModelsRequest request
        ) =>
            new DescribeSubscribeRankingModelsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.DescribeSubscribeRankingModelsResult> DescribeSubscribeRankingModelsAsync(
                Request.DescribeSubscribeRankingModelsRequest request
        ) =>
            new DescribeSubscribeRankingModelsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.DescribeSubscribeRankingModelsResult>();
    #else
		public DescribeSubscribeRankingModelsTask DescribeSubscribeRankingModelsAsync(
                Request.DescribeSubscribeRankingModelsRequest request
        )
		{
			return new DescribeSubscribeRankingModelsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.DescribeSubscribeRankingModelsResult> DescribeSubscribeRankingModelsAsync(
                Request.DescribeSubscribeRankingModelsRequest request
        ) =>
            new DescribeSubscribeRankingModelsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class GetSubscribeRankingModelTask : Gs2RestSessionTask<GetSubscribeRankingModelRequest, GetSubscribeRankingModelResult>
        {
            public GetSubscribeRankingModelTask(IGs2Session session, RestSessionRequestFactory factory, GetSubscribeRankingModelRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(GetSubscribeRankingModelRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "ranking2")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/model/subscribe/{rankingName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{rankingName}", !string.IsNullOrEmpty(request.RankingName) ? request.RankingName.ToString() : "null");

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
		public IEnumerator GetSubscribeRankingModel(
                Request.GetSubscribeRankingModelRequest request,
                UnityAction<AsyncResult<Result.GetSubscribeRankingModelResult>> callback
        ) =>
            new GetSubscribeRankingModelTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.GetSubscribeRankingModelResult> GetSubscribeRankingModelFuture(
                Request.GetSubscribeRankingModelRequest request
        ) =>
            new GetSubscribeRankingModelTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.GetSubscribeRankingModelResult> GetSubscribeRankingModelAsync(
                Request.GetSubscribeRankingModelRequest request
        ) =>
            new GetSubscribeRankingModelTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.GetSubscribeRankingModelResult>();
    #else
		public GetSubscribeRankingModelTask GetSubscribeRankingModelAsync(
                Request.GetSubscribeRankingModelRequest request
        )
		{
			return new GetSubscribeRankingModelTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.GetSubscribeRankingModelResult> GetSubscribeRankingModelAsync(
                Request.GetSubscribeRankingModelRequest request
        ) =>
            new GetSubscribeRankingModelTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class DescribeSubscribeRankingModelMastersTask : Gs2RestSessionTask<DescribeSubscribeRankingModelMastersRequest, DescribeSubscribeRankingModelMastersResult>
        {
            public DescribeSubscribeRankingModelMastersTask(IGs2Session session, RestSessionRequestFactory factory, DescribeSubscribeRankingModelMastersRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DescribeSubscribeRankingModelMastersRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "ranking2")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/master/subscribe";

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
		public IEnumerator DescribeSubscribeRankingModelMasters(
                Request.DescribeSubscribeRankingModelMastersRequest request,
                UnityAction<AsyncResult<Result.DescribeSubscribeRankingModelMastersResult>> callback
        ) =>
            new DescribeSubscribeRankingModelMastersTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.DescribeSubscribeRankingModelMastersResult> DescribeSubscribeRankingModelMastersFuture(
                Request.DescribeSubscribeRankingModelMastersRequest request
        ) =>
            new DescribeSubscribeRankingModelMastersTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.DescribeSubscribeRankingModelMastersResult> DescribeSubscribeRankingModelMastersAsync(
                Request.DescribeSubscribeRankingModelMastersRequest request
        ) =>
            new DescribeSubscribeRankingModelMastersTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.DescribeSubscribeRankingModelMastersResult>();
    #else
		public DescribeSubscribeRankingModelMastersTask DescribeSubscribeRankingModelMastersAsync(
                Request.DescribeSubscribeRankingModelMastersRequest request
        )
		{
			return new DescribeSubscribeRankingModelMastersTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.DescribeSubscribeRankingModelMastersResult> DescribeSubscribeRankingModelMastersAsync(
                Request.DescribeSubscribeRankingModelMastersRequest request
        ) =>
            new DescribeSubscribeRankingModelMastersTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class CreateSubscribeRankingModelMasterTask : Gs2RestSessionTask<CreateSubscribeRankingModelMasterRequest, CreateSubscribeRankingModelMasterResult>
        {
            public CreateSubscribeRankingModelMasterTask(IGs2Session session, RestSessionRequestFactory factory, CreateSubscribeRankingModelMasterRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(CreateSubscribeRankingModelMasterRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "ranking2")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/master/subscribe";

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
                if (request.MinimumValue != null)
                {
                    jsonWriter.WritePropertyName("minimumValue");
                    jsonWriter.Write(request.MinimumValue.ToString());
                }
                if (request.MaximumValue != null)
                {
                    jsonWriter.WritePropertyName("maximumValue");
                    jsonWriter.Write(request.MaximumValue.ToString());
                }
                if (request.Sum != null)
                {
                    jsonWriter.WritePropertyName("sum");
                    jsonWriter.Write(request.Sum.ToString());
                }
                if (request.OrderDirection != null)
                {
                    jsonWriter.WritePropertyName("orderDirection");
                    jsonWriter.Write(request.OrderDirection);
                }
                if (request.EntryPeriodEventId != null)
                {
                    jsonWriter.WritePropertyName("entryPeriodEventId");
                    jsonWriter.Write(request.EntryPeriodEventId);
                }
                if (request.AccessPeriodEventId != null)
                {
                    jsonWriter.WritePropertyName("accessPeriodEventId");
                    jsonWriter.Write(request.AccessPeriodEventId);
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
		public IEnumerator CreateSubscribeRankingModelMaster(
                Request.CreateSubscribeRankingModelMasterRequest request,
                UnityAction<AsyncResult<Result.CreateSubscribeRankingModelMasterResult>> callback
        ) =>
            new CreateSubscribeRankingModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.CreateSubscribeRankingModelMasterResult> CreateSubscribeRankingModelMasterFuture(
                Request.CreateSubscribeRankingModelMasterRequest request
        ) =>
            new CreateSubscribeRankingModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.CreateSubscribeRankingModelMasterResult> CreateSubscribeRankingModelMasterAsync(
                Request.CreateSubscribeRankingModelMasterRequest request
        ) =>
            new CreateSubscribeRankingModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.CreateSubscribeRankingModelMasterResult>();
    #else
		public CreateSubscribeRankingModelMasterTask CreateSubscribeRankingModelMasterAsync(
                Request.CreateSubscribeRankingModelMasterRequest request
        )
		{
			return new CreateSubscribeRankingModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.CreateSubscribeRankingModelMasterResult> CreateSubscribeRankingModelMasterAsync(
                Request.CreateSubscribeRankingModelMasterRequest request
        ) =>
            new CreateSubscribeRankingModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class GetSubscribeRankingModelMasterTask : Gs2RestSessionTask<GetSubscribeRankingModelMasterRequest, GetSubscribeRankingModelMasterResult>
        {
            public GetSubscribeRankingModelMasterTask(IGs2Session session, RestSessionRequestFactory factory, GetSubscribeRankingModelMasterRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(GetSubscribeRankingModelMasterRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "ranking2")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/master/subscribe/{rankingName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{rankingName}", !string.IsNullOrEmpty(request.RankingName) ? request.RankingName.ToString() : "null");

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
		public IEnumerator GetSubscribeRankingModelMaster(
                Request.GetSubscribeRankingModelMasterRequest request,
                UnityAction<AsyncResult<Result.GetSubscribeRankingModelMasterResult>> callback
        ) =>
            new GetSubscribeRankingModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.GetSubscribeRankingModelMasterResult> GetSubscribeRankingModelMasterFuture(
                Request.GetSubscribeRankingModelMasterRequest request
        ) =>
            new GetSubscribeRankingModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.GetSubscribeRankingModelMasterResult> GetSubscribeRankingModelMasterAsync(
                Request.GetSubscribeRankingModelMasterRequest request
        ) =>
            new GetSubscribeRankingModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.GetSubscribeRankingModelMasterResult>();
    #else
		public GetSubscribeRankingModelMasterTask GetSubscribeRankingModelMasterAsync(
                Request.GetSubscribeRankingModelMasterRequest request
        )
		{
			return new GetSubscribeRankingModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.GetSubscribeRankingModelMasterResult> GetSubscribeRankingModelMasterAsync(
                Request.GetSubscribeRankingModelMasterRequest request
        ) =>
            new GetSubscribeRankingModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class UpdateSubscribeRankingModelMasterTask : Gs2RestSessionTask<UpdateSubscribeRankingModelMasterRequest, UpdateSubscribeRankingModelMasterResult>
        {
            public UpdateSubscribeRankingModelMasterTask(IGs2Session session, RestSessionRequestFactory factory, UpdateSubscribeRankingModelMasterRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(UpdateSubscribeRankingModelMasterRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "ranking2")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/master/subscribe/{rankingName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{rankingName}", !string.IsNullOrEmpty(request.RankingName) ? request.RankingName.ToString() : "null");

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
                if (request.MinimumValue != null)
                {
                    jsonWriter.WritePropertyName("minimumValue");
                    jsonWriter.Write(request.MinimumValue.ToString());
                }
                if (request.MaximumValue != null)
                {
                    jsonWriter.WritePropertyName("maximumValue");
                    jsonWriter.Write(request.MaximumValue.ToString());
                }
                if (request.Sum != null)
                {
                    jsonWriter.WritePropertyName("sum");
                    jsonWriter.Write(request.Sum.ToString());
                }
                if (request.OrderDirection != null)
                {
                    jsonWriter.WritePropertyName("orderDirection");
                    jsonWriter.Write(request.OrderDirection);
                }
                if (request.EntryPeriodEventId != null)
                {
                    jsonWriter.WritePropertyName("entryPeriodEventId");
                    jsonWriter.Write(request.EntryPeriodEventId);
                }
                if (request.AccessPeriodEventId != null)
                {
                    jsonWriter.WritePropertyName("accessPeriodEventId");
                    jsonWriter.Write(request.AccessPeriodEventId);
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
		public IEnumerator UpdateSubscribeRankingModelMaster(
                Request.UpdateSubscribeRankingModelMasterRequest request,
                UnityAction<AsyncResult<Result.UpdateSubscribeRankingModelMasterResult>> callback
        ) =>
            new UpdateSubscribeRankingModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.UpdateSubscribeRankingModelMasterResult> UpdateSubscribeRankingModelMasterFuture(
                Request.UpdateSubscribeRankingModelMasterRequest request
        ) =>
            new UpdateSubscribeRankingModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.UpdateSubscribeRankingModelMasterResult> UpdateSubscribeRankingModelMasterAsync(
                Request.UpdateSubscribeRankingModelMasterRequest request
        ) =>
            new UpdateSubscribeRankingModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.UpdateSubscribeRankingModelMasterResult>();
    #else
		public UpdateSubscribeRankingModelMasterTask UpdateSubscribeRankingModelMasterAsync(
                Request.UpdateSubscribeRankingModelMasterRequest request
        )
		{
			return new UpdateSubscribeRankingModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.UpdateSubscribeRankingModelMasterResult> UpdateSubscribeRankingModelMasterAsync(
                Request.UpdateSubscribeRankingModelMasterRequest request
        ) =>
            new UpdateSubscribeRankingModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class DeleteSubscribeRankingModelMasterTask : Gs2RestSessionTask<DeleteSubscribeRankingModelMasterRequest, DeleteSubscribeRankingModelMasterResult>
        {
            public DeleteSubscribeRankingModelMasterTask(IGs2Session session, RestSessionRequestFactory factory, DeleteSubscribeRankingModelMasterRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DeleteSubscribeRankingModelMasterRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "ranking2")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/master/subscribe/{rankingName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{rankingName}", !string.IsNullOrEmpty(request.RankingName) ? request.RankingName.ToString() : "null");

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
		public IEnumerator DeleteSubscribeRankingModelMaster(
                Request.DeleteSubscribeRankingModelMasterRequest request,
                UnityAction<AsyncResult<Result.DeleteSubscribeRankingModelMasterResult>> callback
        ) =>
            new DeleteSubscribeRankingModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.DeleteSubscribeRankingModelMasterResult> DeleteSubscribeRankingModelMasterFuture(
                Request.DeleteSubscribeRankingModelMasterRequest request
        ) =>
            new DeleteSubscribeRankingModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.DeleteSubscribeRankingModelMasterResult> DeleteSubscribeRankingModelMasterAsync(
                Request.DeleteSubscribeRankingModelMasterRequest request
        ) =>
            new DeleteSubscribeRankingModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.DeleteSubscribeRankingModelMasterResult>();
    #else
		public DeleteSubscribeRankingModelMasterTask DeleteSubscribeRankingModelMasterAsync(
                Request.DeleteSubscribeRankingModelMasterRequest request
        )
		{
			return new DeleteSubscribeRankingModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.DeleteSubscribeRankingModelMasterResult> DeleteSubscribeRankingModelMasterAsync(
                Request.DeleteSubscribeRankingModelMasterRequest request
        ) =>
            new DeleteSubscribeRankingModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class DescribeSubscribesTask : Gs2RestSessionTask<DescribeSubscribesRequest, DescribeSubscribesResult>
        {
            public DescribeSubscribesTask(IGs2Session session, RestSessionRequestFactory factory, DescribeSubscribesRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DescribeSubscribesRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "ranking2")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/me/subscribe/score";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");

                var sessionRequest = Factory.Get(url);
                if (request.ContextStack != null)
                {
                    sessionRequest.AddQueryString("contextStack", request.ContextStack);
                }
                if (request.RankingName != null) {
                    sessionRequest.AddQueryString("rankingName", $"{request.RankingName}");
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
		public IEnumerator DescribeSubscribes(
                Request.DescribeSubscribesRequest request,
                UnityAction<AsyncResult<Result.DescribeSubscribesResult>> callback
        ) =>
            new DescribeSubscribesTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.DescribeSubscribesResult> DescribeSubscribesFuture(
                Request.DescribeSubscribesRequest request
        ) =>
            new DescribeSubscribesTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.DescribeSubscribesResult> DescribeSubscribesAsync(
                Request.DescribeSubscribesRequest request
        ) =>
            new DescribeSubscribesTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.DescribeSubscribesResult>();
    #else
		public DescribeSubscribesTask DescribeSubscribesAsync(
                Request.DescribeSubscribesRequest request
        )
		{
			return new DescribeSubscribesTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.DescribeSubscribesResult> DescribeSubscribesAsync(
                Request.DescribeSubscribesRequest request
        ) =>
            new DescribeSubscribesTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class DescribeSubscribesByUserIdTask : Gs2RestSessionTask<DescribeSubscribesByUserIdRequest, DescribeSubscribesByUserIdResult>
        {
            public DescribeSubscribesByUserIdTask(IGs2Session session, RestSessionRequestFactory factory, DescribeSubscribesByUserIdRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DescribeSubscribesByUserIdRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "ranking2")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/{userId}/subscribe/score";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{userId}", !string.IsNullOrEmpty(request.UserId) ? request.UserId.ToString() : "null");

                var sessionRequest = Factory.Get(url);
                if (request.ContextStack != null)
                {
                    sessionRequest.AddQueryString("contextStack", request.ContextStack);
                }
                if (request.RankingName != null) {
                    sessionRequest.AddQueryString("rankingName", $"{request.RankingName}");
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
		public IEnumerator DescribeSubscribesByUserId(
                Request.DescribeSubscribesByUserIdRequest request,
                UnityAction<AsyncResult<Result.DescribeSubscribesByUserIdResult>> callback
        ) =>
            new DescribeSubscribesByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.DescribeSubscribesByUserIdResult> DescribeSubscribesByUserIdFuture(
                Request.DescribeSubscribesByUserIdRequest request
        ) =>
            new DescribeSubscribesByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.DescribeSubscribesByUserIdResult> DescribeSubscribesByUserIdAsync(
                Request.DescribeSubscribesByUserIdRequest request
        ) =>
            new DescribeSubscribesByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.DescribeSubscribesByUserIdResult>();
    #else
		public DescribeSubscribesByUserIdTask DescribeSubscribesByUserIdAsync(
                Request.DescribeSubscribesByUserIdRequest request
        )
		{
			return new DescribeSubscribesByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.DescribeSubscribesByUserIdResult> DescribeSubscribesByUserIdAsync(
                Request.DescribeSubscribesByUserIdRequest request
        ) =>
            new DescribeSubscribesByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class AddSubscribeTask : Gs2RestSessionTask<AddSubscribeRequest, AddSubscribeResult>
        {
            public AddSubscribeTask(IGs2Session session, RestSessionRequestFactory factory, AddSubscribeRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(AddSubscribeRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "ranking2")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/me/subscribe/{rankingName}/target/{targetUserId}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{rankingName}", !string.IsNullOrEmpty(request.RankingName) ? request.RankingName.ToString() : "null");
                url = url.Replace("{targetUserId}", !string.IsNullOrEmpty(request.TargetUserId) ? request.TargetUserId.ToString() : "null");

                var sessionRequest = Factory.Put(url);

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
		public IEnumerator AddSubscribe(
                Request.AddSubscribeRequest request,
                UnityAction<AsyncResult<Result.AddSubscribeResult>> callback
        ) =>
            new AddSubscribeTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.AddSubscribeResult> AddSubscribeFuture(
                Request.AddSubscribeRequest request
        ) =>
            new AddSubscribeTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.AddSubscribeResult> AddSubscribeAsync(
                Request.AddSubscribeRequest request
        ) =>
            new AddSubscribeTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.AddSubscribeResult>();
    #else
		public AddSubscribeTask AddSubscribeAsync(
                Request.AddSubscribeRequest request
        )
		{
			return new AddSubscribeTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.AddSubscribeResult> AddSubscribeAsync(
                Request.AddSubscribeRequest request
        ) =>
            new AddSubscribeTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class AddSubscribeByUserIdTask : Gs2RestSessionTask<AddSubscribeByUserIdRequest, AddSubscribeByUserIdResult>
        {
            public AddSubscribeByUserIdTask(IGs2Session session, RestSessionRequestFactory factory, AddSubscribeByUserIdRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(AddSubscribeByUserIdRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "ranking2")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/{userId}/subscribe/{rankingName}/target/{targetUserId}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{rankingName}", !string.IsNullOrEmpty(request.RankingName) ? request.RankingName.ToString() : "null");
                url = url.Replace("{userId}", !string.IsNullOrEmpty(request.UserId) ? request.UserId.ToString() : "null");
                url = url.Replace("{targetUserId}", !string.IsNullOrEmpty(request.TargetUserId) ? request.TargetUserId.ToString() : "null");

                var sessionRequest = Factory.Put(url);

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
		public IEnumerator AddSubscribeByUserId(
                Request.AddSubscribeByUserIdRequest request,
                UnityAction<AsyncResult<Result.AddSubscribeByUserIdResult>> callback
        ) =>
            new AddSubscribeByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.AddSubscribeByUserIdResult> AddSubscribeByUserIdFuture(
                Request.AddSubscribeByUserIdRequest request
        ) =>
            new AddSubscribeByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.AddSubscribeByUserIdResult> AddSubscribeByUserIdAsync(
                Request.AddSubscribeByUserIdRequest request
        ) =>
            new AddSubscribeByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.AddSubscribeByUserIdResult>();
    #else
		public AddSubscribeByUserIdTask AddSubscribeByUserIdAsync(
                Request.AddSubscribeByUserIdRequest request
        )
		{
			return new AddSubscribeByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.AddSubscribeByUserIdResult> AddSubscribeByUserIdAsync(
                Request.AddSubscribeByUserIdRequest request
        ) =>
            new AddSubscribeByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class DescribeSubscribeRankingScoresTask : Gs2RestSessionTask<DescribeSubscribeRankingScoresRequest, DescribeSubscribeRankingScoresResult>
        {
            public DescribeSubscribeRankingScoresTask(IGs2Session session, RestSessionRequestFactory factory, DescribeSubscribeRankingScoresRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DescribeSubscribeRankingScoresRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "ranking2")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/me/score/subscribe";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");

                var sessionRequest = Factory.Get(url);
                if (request.ContextStack != null)
                {
                    sessionRequest.AddQueryString("contextStack", request.ContextStack);
                }
                if (request.RankingName != null) {
                    sessionRequest.AddQueryString("rankingName", $"{request.RankingName}");
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
		public IEnumerator DescribeSubscribeRankingScores(
                Request.DescribeSubscribeRankingScoresRequest request,
                UnityAction<AsyncResult<Result.DescribeSubscribeRankingScoresResult>> callback
        ) =>
            new DescribeSubscribeRankingScoresTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.DescribeSubscribeRankingScoresResult> DescribeSubscribeRankingScoresFuture(
                Request.DescribeSubscribeRankingScoresRequest request
        ) =>
            new DescribeSubscribeRankingScoresTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.DescribeSubscribeRankingScoresResult> DescribeSubscribeRankingScoresAsync(
                Request.DescribeSubscribeRankingScoresRequest request
        ) =>
            new DescribeSubscribeRankingScoresTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.DescribeSubscribeRankingScoresResult>();
    #else
		public DescribeSubscribeRankingScoresTask DescribeSubscribeRankingScoresAsync(
                Request.DescribeSubscribeRankingScoresRequest request
        )
		{
			return new DescribeSubscribeRankingScoresTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.DescribeSubscribeRankingScoresResult> DescribeSubscribeRankingScoresAsync(
                Request.DescribeSubscribeRankingScoresRequest request
        ) =>
            new DescribeSubscribeRankingScoresTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class DescribeSubscribeRankingScoresByUserIdTask : Gs2RestSessionTask<DescribeSubscribeRankingScoresByUserIdRequest, DescribeSubscribeRankingScoresByUserIdResult>
        {
            public DescribeSubscribeRankingScoresByUserIdTask(IGs2Session session, RestSessionRequestFactory factory, DescribeSubscribeRankingScoresByUserIdRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DescribeSubscribeRankingScoresByUserIdRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "ranking2")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/{userId}/score/subscribe";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{userId}", !string.IsNullOrEmpty(request.UserId) ? request.UserId.ToString() : "null");

                var sessionRequest = Factory.Get(url);
                if (request.ContextStack != null)
                {
                    sessionRequest.AddQueryString("contextStack", request.ContextStack);
                }
                if (request.RankingName != null) {
                    sessionRequest.AddQueryString("rankingName", $"{request.RankingName}");
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
		public IEnumerator DescribeSubscribeRankingScoresByUserId(
                Request.DescribeSubscribeRankingScoresByUserIdRequest request,
                UnityAction<AsyncResult<Result.DescribeSubscribeRankingScoresByUserIdResult>> callback
        ) =>
            new DescribeSubscribeRankingScoresByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.DescribeSubscribeRankingScoresByUserIdResult> DescribeSubscribeRankingScoresByUserIdFuture(
                Request.DescribeSubscribeRankingScoresByUserIdRequest request
        ) =>
            new DescribeSubscribeRankingScoresByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.DescribeSubscribeRankingScoresByUserIdResult> DescribeSubscribeRankingScoresByUserIdAsync(
                Request.DescribeSubscribeRankingScoresByUserIdRequest request
        ) =>
            new DescribeSubscribeRankingScoresByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.DescribeSubscribeRankingScoresByUserIdResult>();
    #else
		public DescribeSubscribeRankingScoresByUserIdTask DescribeSubscribeRankingScoresByUserIdAsync(
                Request.DescribeSubscribeRankingScoresByUserIdRequest request
        )
		{
			return new DescribeSubscribeRankingScoresByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.DescribeSubscribeRankingScoresByUserIdResult> DescribeSubscribeRankingScoresByUserIdAsync(
                Request.DescribeSubscribeRankingScoresByUserIdRequest request
        ) =>
            new DescribeSubscribeRankingScoresByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class PutSubscribeRankingScoreTask : Gs2RestSessionTask<PutSubscribeRankingScoreRequest, PutSubscribeRankingScoreResult>
        {
            public PutSubscribeRankingScoreTask(IGs2Session session, RestSessionRequestFactory factory, PutSubscribeRankingScoreRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(PutSubscribeRankingScoreRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "ranking2")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/me/score/subscribe/{rankingName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{rankingName}", !string.IsNullOrEmpty(request.RankingName) ? request.RankingName.ToString() : "null");

                var sessionRequest = Factory.Put(url);

                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);
                jsonWriter.WriteObjectStart();
                if (request.Score != null)
                {
                    jsonWriter.WritePropertyName("score");
                    jsonWriter.Write(request.Score.ToString());
                }
                if (request.Metadata != null)
                {
                    jsonWriter.WritePropertyName("metadata");
                    jsonWriter.Write(request.Metadata);
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
		public IEnumerator PutSubscribeRankingScore(
                Request.PutSubscribeRankingScoreRequest request,
                UnityAction<AsyncResult<Result.PutSubscribeRankingScoreResult>> callback
        ) =>
            new PutSubscribeRankingScoreTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.PutSubscribeRankingScoreResult> PutSubscribeRankingScoreFuture(
                Request.PutSubscribeRankingScoreRequest request
        ) =>
            new PutSubscribeRankingScoreTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.PutSubscribeRankingScoreResult> PutSubscribeRankingScoreAsync(
                Request.PutSubscribeRankingScoreRequest request
        ) =>
            new PutSubscribeRankingScoreTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.PutSubscribeRankingScoreResult>();
    #else
		public PutSubscribeRankingScoreTask PutSubscribeRankingScoreAsync(
                Request.PutSubscribeRankingScoreRequest request
        )
		{
			return new PutSubscribeRankingScoreTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.PutSubscribeRankingScoreResult> PutSubscribeRankingScoreAsync(
                Request.PutSubscribeRankingScoreRequest request
        ) =>
            new PutSubscribeRankingScoreTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class PutSubscribeRankingScoreByUserIdTask : Gs2RestSessionTask<PutSubscribeRankingScoreByUserIdRequest, PutSubscribeRankingScoreByUserIdResult>
        {
            public PutSubscribeRankingScoreByUserIdTask(IGs2Session session, RestSessionRequestFactory factory, PutSubscribeRankingScoreByUserIdRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(PutSubscribeRankingScoreByUserIdRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "ranking2")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/{userId}/score/subscribe/{rankingName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{rankingName}", !string.IsNullOrEmpty(request.RankingName) ? request.RankingName.ToString() : "null");
                url = url.Replace("{userId}", !string.IsNullOrEmpty(request.UserId) ? request.UserId.ToString() : "null");

                var sessionRequest = Factory.Put(url);

                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);
                jsonWriter.WriteObjectStart();
                if (request.Score != null)
                {
                    jsonWriter.WritePropertyName("score");
                    jsonWriter.Write(request.Score.ToString());
                }
                if (request.Metadata != null)
                {
                    jsonWriter.WritePropertyName("metadata");
                    jsonWriter.Write(request.Metadata);
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
		public IEnumerator PutSubscribeRankingScoreByUserId(
                Request.PutSubscribeRankingScoreByUserIdRequest request,
                UnityAction<AsyncResult<Result.PutSubscribeRankingScoreByUserIdResult>> callback
        ) =>
            new PutSubscribeRankingScoreByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.PutSubscribeRankingScoreByUserIdResult> PutSubscribeRankingScoreByUserIdFuture(
                Request.PutSubscribeRankingScoreByUserIdRequest request
        ) =>
            new PutSubscribeRankingScoreByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.PutSubscribeRankingScoreByUserIdResult> PutSubscribeRankingScoreByUserIdAsync(
                Request.PutSubscribeRankingScoreByUserIdRequest request
        ) =>
            new PutSubscribeRankingScoreByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.PutSubscribeRankingScoreByUserIdResult>();
    #else
		public PutSubscribeRankingScoreByUserIdTask PutSubscribeRankingScoreByUserIdAsync(
                Request.PutSubscribeRankingScoreByUserIdRequest request
        )
		{
			return new PutSubscribeRankingScoreByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.PutSubscribeRankingScoreByUserIdResult> PutSubscribeRankingScoreByUserIdAsync(
                Request.PutSubscribeRankingScoreByUserIdRequest request
        ) =>
            new PutSubscribeRankingScoreByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class GetSubscribeRankingScoreTask : Gs2RestSessionTask<GetSubscribeRankingScoreRequest, GetSubscribeRankingScoreResult>
        {
            public GetSubscribeRankingScoreTask(IGs2Session session, RestSessionRequestFactory factory, GetSubscribeRankingScoreRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(GetSubscribeRankingScoreRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "ranking2")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/me/score/subscribe/{rankingName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{rankingName}", !string.IsNullOrEmpty(request.RankingName) ? request.RankingName.ToString() : "null");

                var sessionRequest = Factory.Get(url);
                if (request.ContextStack != null)
                {
                    sessionRequest.AddQueryString("contextStack", request.ContextStack);
                }
                if (request.Season != null) {
                    sessionRequest.AddQueryString("season", $"{request.Season}");
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
		public IEnumerator GetSubscribeRankingScore(
                Request.GetSubscribeRankingScoreRequest request,
                UnityAction<AsyncResult<Result.GetSubscribeRankingScoreResult>> callback
        ) =>
            new GetSubscribeRankingScoreTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.GetSubscribeRankingScoreResult> GetSubscribeRankingScoreFuture(
                Request.GetSubscribeRankingScoreRequest request
        ) =>
            new GetSubscribeRankingScoreTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.GetSubscribeRankingScoreResult> GetSubscribeRankingScoreAsync(
                Request.GetSubscribeRankingScoreRequest request
        ) =>
            new GetSubscribeRankingScoreTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.GetSubscribeRankingScoreResult>();
    #else
		public GetSubscribeRankingScoreTask GetSubscribeRankingScoreAsync(
                Request.GetSubscribeRankingScoreRequest request
        )
		{
			return new GetSubscribeRankingScoreTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.GetSubscribeRankingScoreResult> GetSubscribeRankingScoreAsync(
                Request.GetSubscribeRankingScoreRequest request
        ) =>
            new GetSubscribeRankingScoreTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class GetSubscribeRankingScoreByUserIdTask : Gs2RestSessionTask<GetSubscribeRankingScoreByUserIdRequest, GetSubscribeRankingScoreByUserIdResult>
        {
            public GetSubscribeRankingScoreByUserIdTask(IGs2Session session, RestSessionRequestFactory factory, GetSubscribeRankingScoreByUserIdRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(GetSubscribeRankingScoreByUserIdRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "ranking2")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/{userId}/score/subscribe/{rankingName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{rankingName}", !string.IsNullOrEmpty(request.RankingName) ? request.RankingName.ToString() : "null");
                url = url.Replace("{userId}", !string.IsNullOrEmpty(request.UserId) ? request.UserId.ToString() : "null");

                var sessionRequest = Factory.Get(url);
                if (request.ContextStack != null)
                {
                    sessionRequest.AddQueryString("contextStack", request.ContextStack);
                }
                if (request.Season != null) {
                    sessionRequest.AddQueryString("season", $"{request.Season}");
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
		public IEnumerator GetSubscribeRankingScoreByUserId(
                Request.GetSubscribeRankingScoreByUserIdRequest request,
                UnityAction<AsyncResult<Result.GetSubscribeRankingScoreByUserIdResult>> callback
        ) =>
            new GetSubscribeRankingScoreByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.GetSubscribeRankingScoreByUserIdResult> GetSubscribeRankingScoreByUserIdFuture(
                Request.GetSubscribeRankingScoreByUserIdRequest request
        ) =>
            new GetSubscribeRankingScoreByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.GetSubscribeRankingScoreByUserIdResult> GetSubscribeRankingScoreByUserIdAsync(
                Request.GetSubscribeRankingScoreByUserIdRequest request
        ) =>
            new GetSubscribeRankingScoreByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.GetSubscribeRankingScoreByUserIdResult>();
    #else
		public GetSubscribeRankingScoreByUserIdTask GetSubscribeRankingScoreByUserIdAsync(
                Request.GetSubscribeRankingScoreByUserIdRequest request
        )
		{
			return new GetSubscribeRankingScoreByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.GetSubscribeRankingScoreByUserIdResult> GetSubscribeRankingScoreByUserIdAsync(
                Request.GetSubscribeRankingScoreByUserIdRequest request
        ) =>
            new GetSubscribeRankingScoreByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class DeleteSubscribeRankingScoreByUserIdTask : Gs2RestSessionTask<DeleteSubscribeRankingScoreByUserIdRequest, DeleteSubscribeRankingScoreByUserIdResult>
        {
            public DeleteSubscribeRankingScoreByUserIdTask(IGs2Session session, RestSessionRequestFactory factory, DeleteSubscribeRankingScoreByUserIdRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DeleteSubscribeRankingScoreByUserIdRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "ranking2")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/{userId}/score/subscribe/{rankingName}/{season}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{rankingName}", !string.IsNullOrEmpty(request.RankingName) ? request.RankingName.ToString() : "null");
                url = url.Replace("{userId}", !string.IsNullOrEmpty(request.UserId) ? request.UserId.ToString() : "null");

                var sessionRequest = Factory.Delete(url);
                if (request.ContextStack != null)
                {
                    sessionRequest.AddQueryString("contextStack", request.ContextStack);
                }
                if (request.Season != null) {
                    sessionRequest.AddQueryString("season", $"{request.Season}");
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
		public IEnumerator DeleteSubscribeRankingScoreByUserId(
                Request.DeleteSubscribeRankingScoreByUserIdRequest request,
                UnityAction<AsyncResult<Result.DeleteSubscribeRankingScoreByUserIdResult>> callback
        ) =>
            new DeleteSubscribeRankingScoreByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.DeleteSubscribeRankingScoreByUserIdResult> DeleteSubscribeRankingScoreByUserIdFuture(
                Request.DeleteSubscribeRankingScoreByUserIdRequest request
        ) =>
            new DeleteSubscribeRankingScoreByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.DeleteSubscribeRankingScoreByUserIdResult> DeleteSubscribeRankingScoreByUserIdAsync(
                Request.DeleteSubscribeRankingScoreByUserIdRequest request
        ) =>
            new DeleteSubscribeRankingScoreByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.DeleteSubscribeRankingScoreByUserIdResult>();
    #else
		public DeleteSubscribeRankingScoreByUserIdTask DeleteSubscribeRankingScoreByUserIdAsync(
                Request.DeleteSubscribeRankingScoreByUserIdRequest request
        )
		{
			return new DeleteSubscribeRankingScoreByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.DeleteSubscribeRankingScoreByUserIdResult> DeleteSubscribeRankingScoreByUserIdAsync(
                Request.DeleteSubscribeRankingScoreByUserIdRequest request
        ) =>
            new DeleteSubscribeRankingScoreByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class VerifySubscribeRankingScoreTask : Gs2RestSessionTask<VerifySubscribeRankingScoreRequest, VerifySubscribeRankingScoreResult>
        {
            public VerifySubscribeRankingScoreTask(IGs2Session session, RestSessionRequestFactory factory, VerifySubscribeRankingScoreRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(VerifySubscribeRankingScoreRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "ranking2")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/{userId}/score/subscribe/{rankingName}/verify/{verifyType}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{rankingName}", !string.IsNullOrEmpty(request.RankingName) ? request.RankingName.ToString() : "null");
                url = url.Replace("{verifyType}", !string.IsNullOrEmpty(request.VerifyType) ? request.VerifyType.ToString() : "null");

                var sessionRequest = Factory.Post(url);

                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);
                jsonWriter.WriteObjectStart();
                if (request.Season != null)
                {
                    jsonWriter.WritePropertyName("season");
                    jsonWriter.Write(request.Season.ToString());
                }
                if (request.Score != null)
                {
                    jsonWriter.WritePropertyName("score");
                    jsonWriter.Write(request.Score.ToString());
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
		public IEnumerator VerifySubscribeRankingScore(
                Request.VerifySubscribeRankingScoreRequest request,
                UnityAction<AsyncResult<Result.VerifySubscribeRankingScoreResult>> callback
        ) =>
            new VerifySubscribeRankingScoreTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.VerifySubscribeRankingScoreResult> VerifySubscribeRankingScoreFuture(
                Request.VerifySubscribeRankingScoreRequest request
        ) =>
            new VerifySubscribeRankingScoreTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.VerifySubscribeRankingScoreResult> VerifySubscribeRankingScoreAsync(
                Request.VerifySubscribeRankingScoreRequest request
        ) =>
            new VerifySubscribeRankingScoreTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.VerifySubscribeRankingScoreResult>();
    #else
		public VerifySubscribeRankingScoreTask VerifySubscribeRankingScoreAsync(
                Request.VerifySubscribeRankingScoreRequest request
        )
		{
			return new VerifySubscribeRankingScoreTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.VerifySubscribeRankingScoreResult> VerifySubscribeRankingScoreAsync(
                Request.VerifySubscribeRankingScoreRequest request
        ) =>
            new VerifySubscribeRankingScoreTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class VerifySubscribeRankingScoreByUserIdTask : Gs2RestSessionTask<VerifySubscribeRankingScoreByUserIdRequest, VerifySubscribeRankingScoreByUserIdResult>
        {
            public VerifySubscribeRankingScoreByUserIdTask(IGs2Session session, RestSessionRequestFactory factory, VerifySubscribeRankingScoreByUserIdRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(VerifySubscribeRankingScoreByUserIdRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "ranking2")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/{userId}/score/subscribe/{rankingName}/verify/{verifyType}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{userId}", !string.IsNullOrEmpty(request.UserId) ? request.UserId.ToString() : "null");
                url = url.Replace("{rankingName}", !string.IsNullOrEmpty(request.RankingName) ? request.RankingName.ToString() : "null");
                url = url.Replace("{verifyType}", !string.IsNullOrEmpty(request.VerifyType) ? request.VerifyType.ToString() : "null");

                var sessionRequest = Factory.Post(url);

                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);
                jsonWriter.WriteObjectStart();
                if (request.Season != null)
                {
                    jsonWriter.WritePropertyName("season");
                    jsonWriter.Write(request.Season.ToString());
                }
                if (request.Score != null)
                {
                    jsonWriter.WritePropertyName("score");
                    jsonWriter.Write(request.Score.ToString());
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
		public IEnumerator VerifySubscribeRankingScoreByUserId(
                Request.VerifySubscribeRankingScoreByUserIdRequest request,
                UnityAction<AsyncResult<Result.VerifySubscribeRankingScoreByUserIdResult>> callback
        ) =>
            new VerifySubscribeRankingScoreByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.VerifySubscribeRankingScoreByUserIdResult> VerifySubscribeRankingScoreByUserIdFuture(
                Request.VerifySubscribeRankingScoreByUserIdRequest request
        ) =>
            new VerifySubscribeRankingScoreByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.VerifySubscribeRankingScoreByUserIdResult> VerifySubscribeRankingScoreByUserIdAsync(
                Request.VerifySubscribeRankingScoreByUserIdRequest request
        ) =>
            new VerifySubscribeRankingScoreByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.VerifySubscribeRankingScoreByUserIdResult>();
    #else
		public VerifySubscribeRankingScoreByUserIdTask VerifySubscribeRankingScoreByUserIdAsync(
                Request.VerifySubscribeRankingScoreByUserIdRequest request
        )
		{
			return new VerifySubscribeRankingScoreByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.VerifySubscribeRankingScoreByUserIdResult> VerifySubscribeRankingScoreByUserIdAsync(
                Request.VerifySubscribeRankingScoreByUserIdRequest request
        ) =>
            new VerifySubscribeRankingScoreByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class VerifySubscribeRankingScoreByStampTaskTask : Gs2RestSessionTask<VerifySubscribeRankingScoreByStampTaskRequest, VerifySubscribeRankingScoreByStampTaskResult>
        {
            public VerifySubscribeRankingScoreByStampTaskTask(IGs2Session session, RestSessionRequestFactory factory, VerifySubscribeRankingScoreByStampTaskRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(VerifySubscribeRankingScoreByStampTaskRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "ranking2")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/stamp/subscribe/score/verify";

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
		public IEnumerator VerifySubscribeRankingScoreByStampTask(
                Request.VerifySubscribeRankingScoreByStampTaskRequest request,
                UnityAction<AsyncResult<Result.VerifySubscribeRankingScoreByStampTaskResult>> callback
        ) =>
            new VerifySubscribeRankingScoreByStampTaskTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.VerifySubscribeRankingScoreByStampTaskResult> VerifySubscribeRankingScoreByStampTaskFuture(
                Request.VerifySubscribeRankingScoreByStampTaskRequest request
        ) =>
            new VerifySubscribeRankingScoreByStampTaskTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.VerifySubscribeRankingScoreByStampTaskResult> VerifySubscribeRankingScoreByStampTaskAsync(
                Request.VerifySubscribeRankingScoreByStampTaskRequest request
        ) =>
            new VerifySubscribeRankingScoreByStampTaskTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.VerifySubscribeRankingScoreByStampTaskResult>();
    #else
		public VerifySubscribeRankingScoreByStampTaskTask VerifySubscribeRankingScoreByStampTaskAsync(
                Request.VerifySubscribeRankingScoreByStampTaskRequest request
        )
		{
			return new VerifySubscribeRankingScoreByStampTaskTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.VerifySubscribeRankingScoreByStampTaskResult> VerifySubscribeRankingScoreByStampTaskAsync(
                Request.VerifySubscribeRankingScoreByStampTaskRequest request
        ) =>
            new VerifySubscribeRankingScoreByStampTaskTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class DescribeSubscribeRankingsTask : Gs2RestSessionTask<DescribeSubscribeRankingsRequest, DescribeSubscribeRankingsResult>
        {
            public DescribeSubscribeRankingsTask(IGs2Session session, RestSessionRequestFactory factory, DescribeSubscribeRankingsRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DescribeSubscribeRankingsRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "ranking2")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/ranking/subscribe/{rankingName}/user/me";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{rankingName}", !string.IsNullOrEmpty(request.RankingName) ? request.RankingName.ToString() : "null");

                var sessionRequest = Factory.Get(url);
                if (request.ContextStack != null)
                {
                    sessionRequest.AddQueryString("contextStack", request.ContextStack);
                }
                if (request.Season != null) {
                    sessionRequest.AddQueryString("season", $"{request.Season}");
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
		public IEnumerator DescribeSubscribeRankings(
                Request.DescribeSubscribeRankingsRequest request,
                UnityAction<AsyncResult<Result.DescribeSubscribeRankingsResult>> callback
        ) =>
            new DescribeSubscribeRankingsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.DescribeSubscribeRankingsResult> DescribeSubscribeRankingsFuture(
                Request.DescribeSubscribeRankingsRequest request
        ) =>
            new DescribeSubscribeRankingsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.DescribeSubscribeRankingsResult> DescribeSubscribeRankingsAsync(
                Request.DescribeSubscribeRankingsRequest request
        ) =>
            new DescribeSubscribeRankingsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.DescribeSubscribeRankingsResult>();
    #else
		public DescribeSubscribeRankingsTask DescribeSubscribeRankingsAsync(
                Request.DescribeSubscribeRankingsRequest request
        )
		{
			return new DescribeSubscribeRankingsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.DescribeSubscribeRankingsResult> DescribeSubscribeRankingsAsync(
                Request.DescribeSubscribeRankingsRequest request
        ) =>
            new DescribeSubscribeRankingsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class DescribeSubscribeRankingsByUserIdTask : Gs2RestSessionTask<DescribeSubscribeRankingsByUserIdRequest, DescribeSubscribeRankingsByUserIdResult>
        {
            public DescribeSubscribeRankingsByUserIdTask(IGs2Session session, RestSessionRequestFactory factory, DescribeSubscribeRankingsByUserIdRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DescribeSubscribeRankingsByUserIdRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "ranking2")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/ranking/subscribe/{rankingName}/user/{userId}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{userId}", !string.IsNullOrEmpty(request.UserId) ? request.UserId.ToString() : "null");
                url = url.Replace("{rankingName}", !string.IsNullOrEmpty(request.RankingName) ? request.RankingName.ToString() : "null");

                var sessionRequest = Factory.Get(url);
                if (request.ContextStack != null)
                {
                    sessionRequest.AddQueryString("contextStack", request.ContextStack);
                }
                if (request.Season != null) {
                    sessionRequest.AddQueryString("season", $"{request.Season}");
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
		public IEnumerator DescribeSubscribeRankingsByUserId(
                Request.DescribeSubscribeRankingsByUserIdRequest request,
                UnityAction<AsyncResult<Result.DescribeSubscribeRankingsByUserIdResult>> callback
        ) =>
            new DescribeSubscribeRankingsByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.DescribeSubscribeRankingsByUserIdResult> DescribeSubscribeRankingsByUserIdFuture(
                Request.DescribeSubscribeRankingsByUserIdRequest request
        ) =>
            new DescribeSubscribeRankingsByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.DescribeSubscribeRankingsByUserIdResult> DescribeSubscribeRankingsByUserIdAsync(
                Request.DescribeSubscribeRankingsByUserIdRequest request
        ) =>
            new DescribeSubscribeRankingsByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.DescribeSubscribeRankingsByUserIdResult>();
    #else
		public DescribeSubscribeRankingsByUserIdTask DescribeSubscribeRankingsByUserIdAsync(
                Request.DescribeSubscribeRankingsByUserIdRequest request
        )
		{
			return new DescribeSubscribeRankingsByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.DescribeSubscribeRankingsByUserIdResult> DescribeSubscribeRankingsByUserIdAsync(
                Request.DescribeSubscribeRankingsByUserIdRequest request
        ) =>
            new DescribeSubscribeRankingsByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class GetSubscribeRankingTask : Gs2RestSessionTask<GetSubscribeRankingRequest, GetSubscribeRankingResult>
        {
            public GetSubscribeRankingTask(IGs2Session session, RestSessionRequestFactory factory, GetSubscribeRankingRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(GetSubscribeRankingRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "ranking2")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/ranking/subscribe/{rankingName}/user/me/rank";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{rankingName}", !string.IsNullOrEmpty(request.RankingName) ? request.RankingName.ToString() : "null");

                var sessionRequest = Factory.Get(url);
                if (request.ContextStack != null)
                {
                    sessionRequest.AddQueryString("contextStack", request.ContextStack);
                }
                if (request.Season != null) {
                    sessionRequest.AddQueryString("season", $"{request.Season}");
                }
                if (request.ScorerUserId != null) {
                    sessionRequest.AddQueryString("scorerUserId", $"{request.ScorerUserId}");
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
		public IEnumerator GetSubscribeRanking(
                Request.GetSubscribeRankingRequest request,
                UnityAction<AsyncResult<Result.GetSubscribeRankingResult>> callback
        ) =>
            new GetSubscribeRankingTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.GetSubscribeRankingResult> GetSubscribeRankingFuture(
                Request.GetSubscribeRankingRequest request
        ) =>
            new GetSubscribeRankingTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.GetSubscribeRankingResult> GetSubscribeRankingAsync(
                Request.GetSubscribeRankingRequest request
        ) =>
            new GetSubscribeRankingTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.GetSubscribeRankingResult>();
    #else
		public GetSubscribeRankingTask GetSubscribeRankingAsync(
                Request.GetSubscribeRankingRequest request
        )
		{
			return new GetSubscribeRankingTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.GetSubscribeRankingResult> GetSubscribeRankingAsync(
                Request.GetSubscribeRankingRequest request
        ) =>
            new GetSubscribeRankingTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class GetSubscribeRankingByUserIdTask : Gs2RestSessionTask<GetSubscribeRankingByUserIdRequest, GetSubscribeRankingByUserIdResult>
        {
            public GetSubscribeRankingByUserIdTask(IGs2Session session, RestSessionRequestFactory factory, GetSubscribeRankingByUserIdRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(GetSubscribeRankingByUserIdRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "ranking2")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/ranking/subscribe/{rankingName}/user/{userId}/rank";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{rankingName}", !string.IsNullOrEmpty(request.RankingName) ? request.RankingName.ToString() : "null");
                url = url.Replace("{userId}", !string.IsNullOrEmpty(request.UserId) ? request.UserId.ToString() : "null");

                var sessionRequest = Factory.Get(url);
                if (request.ContextStack != null)
                {
                    sessionRequest.AddQueryString("contextStack", request.ContextStack);
                }
                if (request.Season != null) {
                    sessionRequest.AddQueryString("season", $"{request.Season}");
                }
                if (request.ScorerUserId != null) {
                    sessionRequest.AddQueryString("scorerUserId", $"{request.ScorerUserId}");
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
		public IEnumerator GetSubscribeRankingByUserId(
                Request.GetSubscribeRankingByUserIdRequest request,
                UnityAction<AsyncResult<Result.GetSubscribeRankingByUserIdResult>> callback
        ) =>
            new GetSubscribeRankingByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.GetSubscribeRankingByUserIdResult> GetSubscribeRankingByUserIdFuture(
                Request.GetSubscribeRankingByUserIdRequest request
        ) =>
            new GetSubscribeRankingByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.GetSubscribeRankingByUserIdResult> GetSubscribeRankingByUserIdAsync(
                Request.GetSubscribeRankingByUserIdRequest request
        ) =>
            new GetSubscribeRankingByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.GetSubscribeRankingByUserIdResult>();
    #else
		public GetSubscribeRankingByUserIdTask GetSubscribeRankingByUserIdAsync(
                Request.GetSubscribeRankingByUserIdRequest request
        )
		{
			return new GetSubscribeRankingByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.GetSubscribeRankingByUserIdResult> GetSubscribeRankingByUserIdAsync(
                Request.GetSubscribeRankingByUserIdRequest request
        ) =>
            new GetSubscribeRankingByUserIdTask(
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
                    .Replace("{service}", "ranking2")
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


        public class GetCurrentRankingMasterTask : Gs2RestSessionTask<GetCurrentRankingMasterRequest, GetCurrentRankingMasterResult>
        {
            public GetCurrentRankingMasterTask(IGs2Session session, RestSessionRequestFactory factory, GetCurrentRankingMasterRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(GetCurrentRankingMasterRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "ranking2")
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
		public IEnumerator GetCurrentRankingMaster(
                Request.GetCurrentRankingMasterRequest request,
                UnityAction<AsyncResult<Result.GetCurrentRankingMasterResult>> callback
        ) =>
            new GetCurrentRankingMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.GetCurrentRankingMasterResult> GetCurrentRankingMasterFuture(
                Request.GetCurrentRankingMasterRequest request
        ) =>
            new GetCurrentRankingMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.GetCurrentRankingMasterResult> GetCurrentRankingMasterAsync(
                Request.GetCurrentRankingMasterRequest request
        ) =>
            new GetCurrentRankingMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.GetCurrentRankingMasterResult>();
    #else
		public GetCurrentRankingMasterTask GetCurrentRankingMasterAsync(
                Request.GetCurrentRankingMasterRequest request
        )
		{
			return new GetCurrentRankingMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.GetCurrentRankingMasterResult> GetCurrentRankingMasterAsync(
                Request.GetCurrentRankingMasterRequest request
        ) =>
            new GetCurrentRankingMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class PreUpdateCurrentRankingMasterTask : Gs2RestSessionTask<PreUpdateCurrentRankingMasterRequest, PreUpdateCurrentRankingMasterResult>
        {
            public PreUpdateCurrentRankingMasterTask(IGs2Session session, RestSessionRequestFactory factory, PreUpdateCurrentRankingMasterRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(PreUpdateCurrentRankingMasterRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "ranking2")
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
		public IEnumerator PreUpdateCurrentRankingMaster(
                Request.PreUpdateCurrentRankingMasterRequest request,
                UnityAction<AsyncResult<Result.PreUpdateCurrentRankingMasterResult>> callback
        ) =>
            new PreUpdateCurrentRankingMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.PreUpdateCurrentRankingMasterResult> PreUpdateCurrentRankingMasterFuture(
                Request.PreUpdateCurrentRankingMasterRequest request
        ) =>
            new PreUpdateCurrentRankingMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.PreUpdateCurrentRankingMasterResult> PreUpdateCurrentRankingMasterAsync(
                Request.PreUpdateCurrentRankingMasterRequest request
        ) =>
            new PreUpdateCurrentRankingMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.PreUpdateCurrentRankingMasterResult>();
    #else
		public PreUpdateCurrentRankingMasterTask PreUpdateCurrentRankingMasterAsync(
                Request.PreUpdateCurrentRankingMasterRequest request
        )
		{
			return new PreUpdateCurrentRankingMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.PreUpdateCurrentRankingMasterResult> PreUpdateCurrentRankingMasterAsync(
                Request.PreUpdateCurrentRankingMasterRequest request
        ) =>
            new PreUpdateCurrentRankingMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class UpdateCurrentRankingMasterTask : Gs2RestSessionTask<UpdateCurrentRankingMasterRequest, UpdateCurrentRankingMasterResult>
        {
            public UpdateCurrentRankingMasterTask(IGs2Session session, RestSessionRequestFactory factory, UpdateCurrentRankingMasterRequest request) : base(session, factory, request)
            {
            }
#if GS2_ENABLE_UNITASK
            protected override async UniTask<UpdateCurrentRankingMasterResult> InvokeImpl()
#else
            protected override async Task<UpdateCurrentRankingMasterResult> InvokeImpl()
#endif
            {
                if (Request.Settings != null) {
                    var preTask = new PreUpdateCurrentRankingMasterTask(
                        Session,
                        Factory,
                        new PreUpdateCurrentRankingMasterRequest()
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

            protected override IGs2SessionRequest CreateRequest(UpdateCurrentRankingMasterRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "ranking2")
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
		public IEnumerator UpdateCurrentRankingMaster(
                Request.UpdateCurrentRankingMasterRequest request,
                UnityAction<AsyncResult<Result.UpdateCurrentRankingMasterResult>> callback
        ) =>
            new UpdateCurrentRankingMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.UpdateCurrentRankingMasterResult> UpdateCurrentRankingMasterFuture(
                Request.UpdateCurrentRankingMasterRequest request
        ) =>
            new UpdateCurrentRankingMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.UpdateCurrentRankingMasterResult> UpdateCurrentRankingMasterAsync(
                Request.UpdateCurrentRankingMasterRequest request
        ) =>
            new UpdateCurrentRankingMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.UpdateCurrentRankingMasterResult>();
    #else
		public UpdateCurrentRankingMasterTask UpdateCurrentRankingMasterAsync(
                Request.UpdateCurrentRankingMasterRequest request
        )
		{
			return new UpdateCurrentRankingMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.UpdateCurrentRankingMasterResult> UpdateCurrentRankingMasterAsync(
                Request.UpdateCurrentRankingMasterRequest request
        ) =>
            new UpdateCurrentRankingMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class UpdateCurrentRankingMasterFromGitHubTask : Gs2RestSessionTask<UpdateCurrentRankingMasterFromGitHubRequest, UpdateCurrentRankingMasterFromGitHubResult>
        {
            public UpdateCurrentRankingMasterFromGitHubTask(IGs2Session session, RestSessionRequestFactory factory, UpdateCurrentRankingMasterFromGitHubRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(UpdateCurrentRankingMasterFromGitHubRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "ranking2")
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
		public IEnumerator UpdateCurrentRankingMasterFromGitHub(
                Request.UpdateCurrentRankingMasterFromGitHubRequest request,
                UnityAction<AsyncResult<Result.UpdateCurrentRankingMasterFromGitHubResult>> callback
        ) =>
            new UpdateCurrentRankingMasterFromGitHubTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.UpdateCurrentRankingMasterFromGitHubResult> UpdateCurrentRankingMasterFromGitHubFuture(
                Request.UpdateCurrentRankingMasterFromGitHubRequest request
        ) =>
            new UpdateCurrentRankingMasterFromGitHubTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.UpdateCurrentRankingMasterFromGitHubResult> UpdateCurrentRankingMasterFromGitHubAsync(
                Request.UpdateCurrentRankingMasterFromGitHubRequest request
        ) =>
            new UpdateCurrentRankingMasterFromGitHubTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.UpdateCurrentRankingMasterFromGitHubResult>();
    #else
		public UpdateCurrentRankingMasterFromGitHubTask UpdateCurrentRankingMasterFromGitHubAsync(
                Request.UpdateCurrentRankingMasterFromGitHubRequest request
        )
		{
			return new UpdateCurrentRankingMasterFromGitHubTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.UpdateCurrentRankingMasterFromGitHubResult> UpdateCurrentRankingMasterFromGitHubAsync(
                Request.UpdateCurrentRankingMasterFromGitHubRequest request
        ) =>
            new UpdateCurrentRankingMasterFromGitHubTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class GetSubscribeTask : Gs2RestSessionTask<GetSubscribeRequest, GetSubscribeResult>
        {
            public GetSubscribeTask(IGs2Session session, RestSessionRequestFactory factory, GetSubscribeRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(GetSubscribeRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "ranking2")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/me/subscribe/{rankingName}/target/{targetUserId}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{rankingName}", !string.IsNullOrEmpty(request.RankingName) ? request.RankingName.ToString() : "null");
                url = url.Replace("{targetUserId}", !string.IsNullOrEmpty(request.TargetUserId) ? request.TargetUserId.ToString() : "null");

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
		public IEnumerator GetSubscribe(
                Request.GetSubscribeRequest request,
                UnityAction<AsyncResult<Result.GetSubscribeResult>> callback
        ) =>
            new GetSubscribeTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.GetSubscribeResult> GetSubscribeFuture(
                Request.GetSubscribeRequest request
        ) =>
            new GetSubscribeTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.GetSubscribeResult> GetSubscribeAsync(
                Request.GetSubscribeRequest request
        ) =>
            new GetSubscribeTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.GetSubscribeResult>();
    #else
		public GetSubscribeTask GetSubscribeAsync(
                Request.GetSubscribeRequest request
        )
		{
			return new GetSubscribeTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.GetSubscribeResult> GetSubscribeAsync(
                Request.GetSubscribeRequest request
        ) =>
            new GetSubscribeTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class GetSubscribeByUserIdTask : Gs2RestSessionTask<GetSubscribeByUserIdRequest, GetSubscribeByUserIdResult>
        {
            public GetSubscribeByUserIdTask(IGs2Session session, RestSessionRequestFactory factory, GetSubscribeByUserIdRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(GetSubscribeByUserIdRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "ranking2")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/{userId}/subscribe/{rankingName}/target/{targetUserId}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{rankingName}", !string.IsNullOrEmpty(request.RankingName) ? request.RankingName.ToString() : "null");
                url = url.Replace("{userId}", !string.IsNullOrEmpty(request.UserId) ? request.UserId.ToString() : "null");
                url = url.Replace("{targetUserId}", !string.IsNullOrEmpty(request.TargetUserId) ? request.TargetUserId.ToString() : "null");

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
		public IEnumerator GetSubscribeByUserId(
                Request.GetSubscribeByUserIdRequest request,
                UnityAction<AsyncResult<Result.GetSubscribeByUserIdResult>> callback
        ) =>
            new GetSubscribeByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.GetSubscribeByUserIdResult> GetSubscribeByUserIdFuture(
                Request.GetSubscribeByUserIdRequest request
        ) =>
            new GetSubscribeByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.GetSubscribeByUserIdResult> GetSubscribeByUserIdAsync(
                Request.GetSubscribeByUserIdRequest request
        ) =>
            new GetSubscribeByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.GetSubscribeByUserIdResult>();
    #else
		public GetSubscribeByUserIdTask GetSubscribeByUserIdAsync(
                Request.GetSubscribeByUserIdRequest request
        )
		{
			return new GetSubscribeByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.GetSubscribeByUserIdResult> GetSubscribeByUserIdAsync(
                Request.GetSubscribeByUserIdRequest request
        ) =>
            new GetSubscribeByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class DeleteSubscribeTask : Gs2RestSessionTask<DeleteSubscribeRequest, DeleteSubscribeResult>
        {
            public DeleteSubscribeTask(IGs2Session session, RestSessionRequestFactory factory, DeleteSubscribeRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DeleteSubscribeRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "ranking2")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/me/subscribe/{rankingName}/target/{targetUserId}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{rankingName}", !string.IsNullOrEmpty(request.RankingName) ? request.RankingName.ToString() : "null");
                url = url.Replace("{targetUserId}", !string.IsNullOrEmpty(request.TargetUserId) ? request.TargetUserId.ToString() : "null");

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
		public IEnumerator DeleteSubscribe(
                Request.DeleteSubscribeRequest request,
                UnityAction<AsyncResult<Result.DeleteSubscribeResult>> callback
        ) =>
            new DeleteSubscribeTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.DeleteSubscribeResult> DeleteSubscribeFuture(
                Request.DeleteSubscribeRequest request
        ) =>
            new DeleteSubscribeTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.DeleteSubscribeResult> DeleteSubscribeAsync(
                Request.DeleteSubscribeRequest request
        ) =>
            new DeleteSubscribeTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.DeleteSubscribeResult>();
    #else
		public DeleteSubscribeTask DeleteSubscribeAsync(
                Request.DeleteSubscribeRequest request
        )
		{
			return new DeleteSubscribeTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.DeleteSubscribeResult> DeleteSubscribeAsync(
                Request.DeleteSubscribeRequest request
        ) =>
            new DeleteSubscribeTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class DeleteSubscribeByUserIdTask : Gs2RestSessionTask<DeleteSubscribeByUserIdRequest, DeleteSubscribeByUserIdResult>
        {
            public DeleteSubscribeByUserIdTask(IGs2Session session, RestSessionRequestFactory factory, DeleteSubscribeByUserIdRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DeleteSubscribeByUserIdRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "ranking2")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/{userId}/subscribe/{rankingName}/target/{targetUserId}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{rankingName}", !string.IsNullOrEmpty(request.RankingName) ? request.RankingName.ToString() : "null");
                url = url.Replace("{userId}", !string.IsNullOrEmpty(request.UserId) ? request.UserId.ToString() : "null");
                url = url.Replace("{targetUserId}", !string.IsNullOrEmpty(request.TargetUserId) ? request.TargetUserId.ToString() : "null");

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
		public IEnumerator DeleteSubscribeByUserId(
                Request.DeleteSubscribeByUserIdRequest request,
                UnityAction<AsyncResult<Result.DeleteSubscribeByUserIdResult>> callback
        ) =>
            new DeleteSubscribeByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.DeleteSubscribeByUserIdResult> DeleteSubscribeByUserIdFuture(
                Request.DeleteSubscribeByUserIdRequest request
        ) =>
            new DeleteSubscribeByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.DeleteSubscribeByUserIdResult> DeleteSubscribeByUserIdAsync(
                Request.DeleteSubscribeByUserIdRequest request
        ) =>
            new DeleteSubscribeByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.DeleteSubscribeByUserIdResult>();
    #else
		public DeleteSubscribeByUserIdTask DeleteSubscribeByUserIdAsync(
                Request.DeleteSubscribeByUserIdRequest request
        )
		{
			return new DeleteSubscribeByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.DeleteSubscribeByUserIdResult> DeleteSubscribeByUserIdAsync(
                Request.DeleteSubscribeByUserIdRequest request
        ) =>
            new DeleteSubscribeByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif
	}
}