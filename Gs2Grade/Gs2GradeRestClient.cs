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
using Gs2.Gs2Grade.Request;
using Gs2.Gs2Grade.Result;
using Gs2.Util.LitJson;

namespace Gs2.Gs2Grade
{
	public class Gs2GradeRestClient : AbstractGs2Client
	{
#if UNITY_2017_1_OR_NEWER
		private readonly CertificateHandler _certificateHandler;
#endif

		public static string Endpoint = "grade";

        protected Gs2RestSession Gs2RestSession => (Gs2RestSession) Gs2Session;

		public Gs2GradeRestClient(Gs2RestSession Gs2RestSession) : base(Gs2RestSession)
		{

		}

#if UNITY_2017_1_OR_NEWER
		public Gs2GradeRestClient(Gs2RestSession gs2RestSession, CertificateHandler certificateHandler) : base(gs2RestSession)
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
                    .Replace("{service}", "grade")
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
                    .Replace("{service}", "grade")
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
                if (request.ChangeGradeScript != null)
                {
                    jsonWriter.WritePropertyName("changeGradeScript");
                    request.ChangeGradeScript.WriteJson(jsonWriter);
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
                    .Replace("{service}", "grade")
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
                    .Replace("{service}", "grade")
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
                    .Replace("{service}", "grade")
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
                if (request.ChangeGradeScript != null)
                {
                    jsonWriter.WritePropertyName("changeGradeScript");
                    request.ChangeGradeScript.WriteJson(jsonWriter);
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
                    .Replace("{service}", "grade")
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
                    .Replace("{service}", "grade")
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
                    .Replace("{service}", "grade")
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
                    .Replace("{service}", "grade")
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
                    .Replace("{service}", "grade")
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
                    .Replace("{service}", "grade")
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
                    .Replace("{service}", "grade")
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
                    .Replace("{service}", "grade")
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
                    .Replace("{service}", "grade")
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


        public class DescribeGradeModelMastersTask : Gs2RestSessionTask<DescribeGradeModelMastersRequest, DescribeGradeModelMastersResult>
        {
            public DescribeGradeModelMastersTask(IGs2Session session, RestSessionRequestFactory factory, DescribeGradeModelMastersRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DescribeGradeModelMastersRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "grade")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/master/model";

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
		public IEnumerator DescribeGradeModelMasters(
                Request.DescribeGradeModelMastersRequest request,
                UnityAction<AsyncResult<Result.DescribeGradeModelMastersResult>> callback
        ) =>
            new DescribeGradeModelMastersTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.DescribeGradeModelMastersResult> DescribeGradeModelMastersFuture(
                Request.DescribeGradeModelMastersRequest request
        ) =>
            new DescribeGradeModelMastersTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.DescribeGradeModelMastersResult> DescribeGradeModelMastersAsync(
                Request.DescribeGradeModelMastersRequest request
        ) =>
            new DescribeGradeModelMastersTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.DescribeGradeModelMastersResult>();
    #else
		public DescribeGradeModelMastersTask DescribeGradeModelMastersAsync(
                Request.DescribeGradeModelMastersRequest request
        )
		{
			return new DescribeGradeModelMastersTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.DescribeGradeModelMastersResult> DescribeGradeModelMastersAsync(
                Request.DescribeGradeModelMastersRequest request
        ) =>
            new DescribeGradeModelMastersTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class CreateGradeModelMasterTask : Gs2RestSessionTask<CreateGradeModelMasterRequest, CreateGradeModelMasterResult>
        {
            public CreateGradeModelMasterTask(IGs2Session session, RestSessionRequestFactory factory, CreateGradeModelMasterRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(CreateGradeModelMasterRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "grade")
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
                if (request.DefaultGrades != null)
                {
                    jsonWriter.WritePropertyName("defaultGrades");
                    jsonWriter.WriteArrayStart();
                    foreach(var item in request.DefaultGrades)
                    {
                        item.WriteJson(jsonWriter);
                    }
                    jsonWriter.WriteArrayEnd();
                }
                if (request.ExperienceModelId != null)
                {
                    jsonWriter.WritePropertyName("experienceModelId");
                    jsonWriter.Write(request.ExperienceModelId);
                }
                if (request.GradeEntries != null)
                {
                    jsonWriter.WritePropertyName("gradeEntries");
                    jsonWriter.WriteArrayStart();
                    foreach(var item in request.GradeEntries)
                    {
                        item.WriteJson(jsonWriter);
                    }
                    jsonWriter.WriteArrayEnd();
                }
                if (request.AcquireActionRates != null)
                {
                    jsonWriter.WritePropertyName("acquireActionRates");
                    jsonWriter.WriteArrayStart();
                    foreach(var item in request.AcquireActionRates)
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
		public IEnumerator CreateGradeModelMaster(
                Request.CreateGradeModelMasterRequest request,
                UnityAction<AsyncResult<Result.CreateGradeModelMasterResult>> callback
        ) =>
            new CreateGradeModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.CreateGradeModelMasterResult> CreateGradeModelMasterFuture(
                Request.CreateGradeModelMasterRequest request
        ) =>
            new CreateGradeModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.CreateGradeModelMasterResult> CreateGradeModelMasterAsync(
                Request.CreateGradeModelMasterRequest request
        ) =>
            new CreateGradeModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.CreateGradeModelMasterResult>();
    #else
		public CreateGradeModelMasterTask CreateGradeModelMasterAsync(
                Request.CreateGradeModelMasterRequest request
        )
		{
			return new CreateGradeModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.CreateGradeModelMasterResult> CreateGradeModelMasterAsync(
                Request.CreateGradeModelMasterRequest request
        ) =>
            new CreateGradeModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class GetGradeModelMasterTask : Gs2RestSessionTask<GetGradeModelMasterRequest, GetGradeModelMasterResult>
        {
            public GetGradeModelMasterTask(IGs2Session session, RestSessionRequestFactory factory, GetGradeModelMasterRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(GetGradeModelMasterRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "grade")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/master/model/{gradeName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{gradeName}", !string.IsNullOrEmpty(request.GradeName) ? request.GradeName.ToString() : "null");

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
		public IEnumerator GetGradeModelMaster(
                Request.GetGradeModelMasterRequest request,
                UnityAction<AsyncResult<Result.GetGradeModelMasterResult>> callback
        ) =>
            new GetGradeModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.GetGradeModelMasterResult> GetGradeModelMasterFuture(
                Request.GetGradeModelMasterRequest request
        ) =>
            new GetGradeModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.GetGradeModelMasterResult> GetGradeModelMasterAsync(
                Request.GetGradeModelMasterRequest request
        ) =>
            new GetGradeModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.GetGradeModelMasterResult>();
    #else
		public GetGradeModelMasterTask GetGradeModelMasterAsync(
                Request.GetGradeModelMasterRequest request
        )
		{
			return new GetGradeModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.GetGradeModelMasterResult> GetGradeModelMasterAsync(
                Request.GetGradeModelMasterRequest request
        ) =>
            new GetGradeModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class UpdateGradeModelMasterTask : Gs2RestSessionTask<UpdateGradeModelMasterRequest, UpdateGradeModelMasterResult>
        {
            public UpdateGradeModelMasterTask(IGs2Session session, RestSessionRequestFactory factory, UpdateGradeModelMasterRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(UpdateGradeModelMasterRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "grade")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/master/model/{gradeName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{gradeName}", !string.IsNullOrEmpty(request.GradeName) ? request.GradeName.ToString() : "null");

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
                if (request.DefaultGrades != null)
                {
                    jsonWriter.WritePropertyName("defaultGrades");
                    jsonWriter.WriteArrayStart();
                    foreach(var item in request.DefaultGrades)
                    {
                        item.WriteJson(jsonWriter);
                    }
                    jsonWriter.WriteArrayEnd();
                }
                if (request.ExperienceModelId != null)
                {
                    jsonWriter.WritePropertyName("experienceModelId");
                    jsonWriter.Write(request.ExperienceModelId);
                }
                if (request.GradeEntries != null)
                {
                    jsonWriter.WritePropertyName("gradeEntries");
                    jsonWriter.WriteArrayStart();
                    foreach(var item in request.GradeEntries)
                    {
                        item.WriteJson(jsonWriter);
                    }
                    jsonWriter.WriteArrayEnd();
                }
                if (request.AcquireActionRates != null)
                {
                    jsonWriter.WritePropertyName("acquireActionRates");
                    jsonWriter.WriteArrayStart();
                    foreach(var item in request.AcquireActionRates)
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
		public IEnumerator UpdateGradeModelMaster(
                Request.UpdateGradeModelMasterRequest request,
                UnityAction<AsyncResult<Result.UpdateGradeModelMasterResult>> callback
        ) =>
            new UpdateGradeModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.UpdateGradeModelMasterResult> UpdateGradeModelMasterFuture(
                Request.UpdateGradeModelMasterRequest request
        ) =>
            new UpdateGradeModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.UpdateGradeModelMasterResult> UpdateGradeModelMasterAsync(
                Request.UpdateGradeModelMasterRequest request
        ) =>
            new UpdateGradeModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.UpdateGradeModelMasterResult>();
    #else
		public UpdateGradeModelMasterTask UpdateGradeModelMasterAsync(
                Request.UpdateGradeModelMasterRequest request
        )
		{
			return new UpdateGradeModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.UpdateGradeModelMasterResult> UpdateGradeModelMasterAsync(
                Request.UpdateGradeModelMasterRequest request
        ) =>
            new UpdateGradeModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class DeleteGradeModelMasterTask : Gs2RestSessionTask<DeleteGradeModelMasterRequest, DeleteGradeModelMasterResult>
        {
            public DeleteGradeModelMasterTask(IGs2Session session, RestSessionRequestFactory factory, DeleteGradeModelMasterRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DeleteGradeModelMasterRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "grade")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/master/model/{gradeName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{gradeName}", !string.IsNullOrEmpty(request.GradeName) ? request.GradeName.ToString() : "null");

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
		public IEnumerator DeleteGradeModelMaster(
                Request.DeleteGradeModelMasterRequest request,
                UnityAction<AsyncResult<Result.DeleteGradeModelMasterResult>> callback
        ) =>
            new DeleteGradeModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.DeleteGradeModelMasterResult> DeleteGradeModelMasterFuture(
                Request.DeleteGradeModelMasterRequest request
        ) =>
            new DeleteGradeModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.DeleteGradeModelMasterResult> DeleteGradeModelMasterAsync(
                Request.DeleteGradeModelMasterRequest request
        ) =>
            new DeleteGradeModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.DeleteGradeModelMasterResult>();
    #else
		public DeleteGradeModelMasterTask DeleteGradeModelMasterAsync(
                Request.DeleteGradeModelMasterRequest request
        )
		{
			return new DeleteGradeModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.DeleteGradeModelMasterResult> DeleteGradeModelMasterAsync(
                Request.DeleteGradeModelMasterRequest request
        ) =>
            new DeleteGradeModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class DescribeGradeModelsTask : Gs2RestSessionTask<DescribeGradeModelsRequest, DescribeGradeModelsResult>
        {
            public DescribeGradeModelsTask(IGs2Session session, RestSessionRequestFactory factory, DescribeGradeModelsRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DescribeGradeModelsRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "grade")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/model";

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
		public IEnumerator DescribeGradeModels(
                Request.DescribeGradeModelsRequest request,
                UnityAction<AsyncResult<Result.DescribeGradeModelsResult>> callback
        ) =>
            new DescribeGradeModelsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.DescribeGradeModelsResult> DescribeGradeModelsFuture(
                Request.DescribeGradeModelsRequest request
        ) =>
            new DescribeGradeModelsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.DescribeGradeModelsResult> DescribeGradeModelsAsync(
                Request.DescribeGradeModelsRequest request
        ) =>
            new DescribeGradeModelsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.DescribeGradeModelsResult>();
    #else
		public DescribeGradeModelsTask DescribeGradeModelsAsync(
                Request.DescribeGradeModelsRequest request
        )
		{
			return new DescribeGradeModelsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.DescribeGradeModelsResult> DescribeGradeModelsAsync(
                Request.DescribeGradeModelsRequest request
        ) =>
            new DescribeGradeModelsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class GetGradeModelTask : Gs2RestSessionTask<GetGradeModelRequest, GetGradeModelResult>
        {
            public GetGradeModelTask(IGs2Session session, RestSessionRequestFactory factory, GetGradeModelRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(GetGradeModelRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "grade")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/model/{gradeName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{gradeName}", !string.IsNullOrEmpty(request.GradeName) ? request.GradeName.ToString() : "null");

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
		public IEnumerator GetGradeModel(
                Request.GetGradeModelRequest request,
                UnityAction<AsyncResult<Result.GetGradeModelResult>> callback
        ) =>
            new GetGradeModelTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.GetGradeModelResult> GetGradeModelFuture(
                Request.GetGradeModelRequest request
        ) =>
            new GetGradeModelTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.GetGradeModelResult> GetGradeModelAsync(
                Request.GetGradeModelRequest request
        ) =>
            new GetGradeModelTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.GetGradeModelResult>();
    #else
		public GetGradeModelTask GetGradeModelAsync(
                Request.GetGradeModelRequest request
        )
		{
			return new GetGradeModelTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.GetGradeModelResult> GetGradeModelAsync(
                Request.GetGradeModelRequest request
        ) =>
            new GetGradeModelTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class DescribeStatusesTask : Gs2RestSessionTask<DescribeStatusesRequest, DescribeStatusesResult>
        {
            public DescribeStatusesTask(IGs2Session session, RestSessionRequestFactory factory, DescribeStatusesRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DescribeStatusesRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "grade")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/me/status";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");

                var sessionRequest = Factory.Get(url);
                if (request.ContextStack != null)
                {
                    sessionRequest.AddQueryString("contextStack", request.ContextStack);
                }
                if (request.GradeName != null) {
                    sessionRequest.AddQueryString("gradeName", $"{request.GradeName}");
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
		public IEnumerator DescribeStatuses(
                Request.DescribeStatusesRequest request,
                UnityAction<AsyncResult<Result.DescribeStatusesResult>> callback
        ) =>
            new DescribeStatusesTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.DescribeStatusesResult> DescribeStatusesFuture(
                Request.DescribeStatusesRequest request
        ) =>
            new DescribeStatusesTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.DescribeStatusesResult> DescribeStatusesAsync(
                Request.DescribeStatusesRequest request
        ) =>
            new DescribeStatusesTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.DescribeStatusesResult>();
    #else
		public DescribeStatusesTask DescribeStatusesAsync(
                Request.DescribeStatusesRequest request
        )
		{
			return new DescribeStatusesTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.DescribeStatusesResult> DescribeStatusesAsync(
                Request.DescribeStatusesRequest request
        ) =>
            new DescribeStatusesTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class DescribeStatusesByUserIdTask : Gs2RestSessionTask<DescribeStatusesByUserIdRequest, DescribeStatusesByUserIdResult>
        {
            public DescribeStatusesByUserIdTask(IGs2Session session, RestSessionRequestFactory factory, DescribeStatusesByUserIdRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DescribeStatusesByUserIdRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "grade")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/{userId}/status";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{userId}", !string.IsNullOrEmpty(request.UserId) ? request.UserId.ToString() : "null");

                var sessionRequest = Factory.Get(url);
                if (request.ContextStack != null)
                {
                    sessionRequest.AddQueryString("contextStack", request.ContextStack);
                }
                if (request.GradeName != null) {
                    sessionRequest.AddQueryString("gradeName", $"{request.GradeName}");
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
		public IEnumerator DescribeStatusesByUserId(
                Request.DescribeStatusesByUserIdRequest request,
                UnityAction<AsyncResult<Result.DescribeStatusesByUserIdResult>> callback
        ) =>
            new DescribeStatusesByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.DescribeStatusesByUserIdResult> DescribeStatusesByUserIdFuture(
                Request.DescribeStatusesByUserIdRequest request
        ) =>
            new DescribeStatusesByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.DescribeStatusesByUserIdResult> DescribeStatusesByUserIdAsync(
                Request.DescribeStatusesByUserIdRequest request
        ) =>
            new DescribeStatusesByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.DescribeStatusesByUserIdResult>();
    #else
		public DescribeStatusesByUserIdTask DescribeStatusesByUserIdAsync(
                Request.DescribeStatusesByUserIdRequest request
        )
		{
			return new DescribeStatusesByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.DescribeStatusesByUserIdResult> DescribeStatusesByUserIdAsync(
                Request.DescribeStatusesByUserIdRequest request
        ) =>
            new DescribeStatusesByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class GetStatusTask : Gs2RestSessionTask<GetStatusRequest, GetStatusResult>
        {
            public GetStatusTask(IGs2Session session, RestSessionRequestFactory factory, GetStatusRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(GetStatusRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "grade")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/me/status/model/{gradeName}/property/{propertyId}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{gradeName}", !string.IsNullOrEmpty(request.GradeName) ? request.GradeName.ToString() : "null");
                url = url.Replace("{propertyId}", !string.IsNullOrEmpty(request.PropertyId) ? request.PropertyId.ToString() : "null");

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
		public IEnumerator GetStatus(
                Request.GetStatusRequest request,
                UnityAction<AsyncResult<Result.GetStatusResult>> callback
        ) =>
            new GetStatusTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.GetStatusResult> GetStatusFuture(
                Request.GetStatusRequest request
        ) =>
            new GetStatusTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.GetStatusResult> GetStatusAsync(
                Request.GetStatusRequest request
        ) =>
            new GetStatusTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.GetStatusResult>();
    #else
		public GetStatusTask GetStatusAsync(
                Request.GetStatusRequest request
        )
		{
			return new GetStatusTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.GetStatusResult> GetStatusAsync(
                Request.GetStatusRequest request
        ) =>
            new GetStatusTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class GetStatusByUserIdTask : Gs2RestSessionTask<GetStatusByUserIdRequest, GetStatusByUserIdResult>
        {
            public GetStatusByUserIdTask(IGs2Session session, RestSessionRequestFactory factory, GetStatusByUserIdRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(GetStatusByUserIdRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "grade")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/{userId}/status/model/{gradeName}/property/{propertyId}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{userId}", !string.IsNullOrEmpty(request.UserId) ? request.UserId.ToString() : "null");
                url = url.Replace("{gradeName}", !string.IsNullOrEmpty(request.GradeName) ? request.GradeName.ToString() : "null");
                url = url.Replace("{propertyId}", !string.IsNullOrEmpty(request.PropertyId) ? request.PropertyId.ToString() : "null");

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
		public IEnumerator GetStatusByUserId(
                Request.GetStatusByUserIdRequest request,
                UnityAction<AsyncResult<Result.GetStatusByUserIdResult>> callback
        ) =>
            new GetStatusByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.GetStatusByUserIdResult> GetStatusByUserIdFuture(
                Request.GetStatusByUserIdRequest request
        ) =>
            new GetStatusByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.GetStatusByUserIdResult> GetStatusByUserIdAsync(
                Request.GetStatusByUserIdRequest request
        ) =>
            new GetStatusByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.GetStatusByUserIdResult>();
    #else
		public GetStatusByUserIdTask GetStatusByUserIdAsync(
                Request.GetStatusByUserIdRequest request
        )
		{
			return new GetStatusByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.GetStatusByUserIdResult> GetStatusByUserIdAsync(
                Request.GetStatusByUserIdRequest request
        ) =>
            new GetStatusByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class AddGradeByUserIdTask : Gs2RestSessionTask<AddGradeByUserIdRequest, AddGradeByUserIdResult>
        {
            public AddGradeByUserIdTask(IGs2Session session, RestSessionRequestFactory factory, AddGradeByUserIdRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(AddGradeByUserIdRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "grade")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/{userId}/status/model/{gradeName}/property/{propertyId}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{userId}", !string.IsNullOrEmpty(request.UserId) ? request.UserId.ToString() : "null");
                url = url.Replace("{gradeName}", !string.IsNullOrEmpty(request.GradeName) ? request.GradeName.ToString() : "null");
                url = url.Replace("{propertyId}", !string.IsNullOrEmpty(request.PropertyId) ? request.PropertyId.ToString() : "null");

                var sessionRequest = Factory.Post(url);

                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);
                jsonWriter.WriteObjectStart();
                if (request.GradeValue != null)
                {
                    jsonWriter.WritePropertyName("gradeValue");
                    jsonWriter.Write(request.GradeValue.ToString());
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
		public IEnumerator AddGradeByUserId(
                Request.AddGradeByUserIdRequest request,
                UnityAction<AsyncResult<Result.AddGradeByUserIdResult>> callback
        ) =>
            new AddGradeByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.AddGradeByUserIdResult> AddGradeByUserIdFuture(
                Request.AddGradeByUserIdRequest request
        ) =>
            new AddGradeByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.AddGradeByUserIdResult> AddGradeByUserIdAsync(
                Request.AddGradeByUserIdRequest request
        ) =>
            new AddGradeByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.AddGradeByUserIdResult>();
    #else
		public AddGradeByUserIdTask AddGradeByUserIdAsync(
                Request.AddGradeByUserIdRequest request
        )
		{
			return new AddGradeByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.AddGradeByUserIdResult> AddGradeByUserIdAsync(
                Request.AddGradeByUserIdRequest request
        ) =>
            new AddGradeByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class SubGradeTask : Gs2RestSessionTask<SubGradeRequest, SubGradeResult>
        {
            public SubGradeTask(IGs2Session session, RestSessionRequestFactory factory, SubGradeRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(SubGradeRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "grade")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/me/status/model/{gradeName}/property/{propertyId}/sub";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{gradeName}", !string.IsNullOrEmpty(request.GradeName) ? request.GradeName.ToString() : "null");
                url = url.Replace("{propertyId}", !string.IsNullOrEmpty(request.PropertyId) ? request.PropertyId.ToString() : "null");

                var sessionRequest = Factory.Post(url);

                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);
                jsonWriter.WriteObjectStart();
                if (request.GradeValue != null)
                {
                    jsonWriter.WritePropertyName("gradeValue");
                    jsonWriter.Write(request.GradeValue.ToString());
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
		public IEnumerator SubGrade(
                Request.SubGradeRequest request,
                UnityAction<AsyncResult<Result.SubGradeResult>> callback
        ) =>
            new SubGradeTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.SubGradeResult> SubGradeFuture(
                Request.SubGradeRequest request
        ) =>
            new SubGradeTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.SubGradeResult> SubGradeAsync(
                Request.SubGradeRequest request
        ) =>
            new SubGradeTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.SubGradeResult>();
    #else
		public SubGradeTask SubGradeAsync(
                Request.SubGradeRequest request
        )
		{
			return new SubGradeTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.SubGradeResult> SubGradeAsync(
                Request.SubGradeRequest request
        ) =>
            new SubGradeTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class SubGradeByUserIdTask : Gs2RestSessionTask<SubGradeByUserIdRequest, SubGradeByUserIdResult>
        {
            public SubGradeByUserIdTask(IGs2Session session, RestSessionRequestFactory factory, SubGradeByUserIdRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(SubGradeByUserIdRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "grade")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/{userId}/status/model/{gradeName}/property/{propertyId}/sub";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{userId}", !string.IsNullOrEmpty(request.UserId) ? request.UserId.ToString() : "null");
                url = url.Replace("{gradeName}", !string.IsNullOrEmpty(request.GradeName) ? request.GradeName.ToString() : "null");
                url = url.Replace("{propertyId}", !string.IsNullOrEmpty(request.PropertyId) ? request.PropertyId.ToString() : "null");

                var sessionRequest = Factory.Post(url);

                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);
                jsonWriter.WriteObjectStart();
                if (request.GradeValue != null)
                {
                    jsonWriter.WritePropertyName("gradeValue");
                    jsonWriter.Write(request.GradeValue.ToString());
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
		public IEnumerator SubGradeByUserId(
                Request.SubGradeByUserIdRequest request,
                UnityAction<AsyncResult<Result.SubGradeByUserIdResult>> callback
        ) =>
            new SubGradeByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.SubGradeByUserIdResult> SubGradeByUserIdFuture(
                Request.SubGradeByUserIdRequest request
        ) =>
            new SubGradeByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.SubGradeByUserIdResult> SubGradeByUserIdAsync(
                Request.SubGradeByUserIdRequest request
        ) =>
            new SubGradeByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.SubGradeByUserIdResult>();
    #else
		public SubGradeByUserIdTask SubGradeByUserIdAsync(
                Request.SubGradeByUserIdRequest request
        )
		{
			return new SubGradeByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.SubGradeByUserIdResult> SubGradeByUserIdAsync(
                Request.SubGradeByUserIdRequest request
        ) =>
            new SubGradeByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class SetGradeByUserIdTask : Gs2RestSessionTask<SetGradeByUserIdRequest, SetGradeByUserIdResult>
        {
            public SetGradeByUserIdTask(IGs2Session session, RestSessionRequestFactory factory, SetGradeByUserIdRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(SetGradeByUserIdRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "grade")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/{userId}/status/model/{gradeName}/property/{propertyId}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{userId}", !string.IsNullOrEmpty(request.UserId) ? request.UserId.ToString() : "null");
                url = url.Replace("{gradeName}", !string.IsNullOrEmpty(request.GradeName) ? request.GradeName.ToString() : "null");
                url = url.Replace("{propertyId}", !string.IsNullOrEmpty(request.PropertyId) ? request.PropertyId.ToString() : "null");

                var sessionRequest = Factory.Put(url);

                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);
                jsonWriter.WriteObjectStart();
                if (request.GradeValue != null)
                {
                    jsonWriter.WritePropertyName("gradeValue");
                    jsonWriter.Write(request.GradeValue.ToString());
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
		public IEnumerator SetGradeByUserId(
                Request.SetGradeByUserIdRequest request,
                UnityAction<AsyncResult<Result.SetGradeByUserIdResult>> callback
        ) =>
            new SetGradeByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.SetGradeByUserIdResult> SetGradeByUserIdFuture(
                Request.SetGradeByUserIdRequest request
        ) =>
            new SetGradeByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.SetGradeByUserIdResult> SetGradeByUserIdAsync(
                Request.SetGradeByUserIdRequest request
        ) =>
            new SetGradeByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.SetGradeByUserIdResult>();
    #else
		public SetGradeByUserIdTask SetGradeByUserIdAsync(
                Request.SetGradeByUserIdRequest request
        )
		{
			return new SetGradeByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.SetGradeByUserIdResult> SetGradeByUserIdAsync(
                Request.SetGradeByUserIdRequest request
        ) =>
            new SetGradeByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class ApplyRankCapTask : Gs2RestSessionTask<ApplyRankCapRequest, ApplyRankCapResult>
        {
            public ApplyRankCapTask(IGs2Session session, RestSessionRequestFactory factory, ApplyRankCapRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(ApplyRankCapRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "grade")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/me/status/model/{gradeName}/property/{propertyId}/apply/rank/cap";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{gradeName}", !string.IsNullOrEmpty(request.GradeName) ? request.GradeName.ToString() : "null");
                url = url.Replace("{propertyId}", !string.IsNullOrEmpty(request.PropertyId) ? request.PropertyId.ToString() : "null");

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
		public IEnumerator ApplyRankCap(
                Request.ApplyRankCapRequest request,
                UnityAction<AsyncResult<Result.ApplyRankCapResult>> callback
        ) =>
            new ApplyRankCapTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.ApplyRankCapResult> ApplyRankCapFuture(
                Request.ApplyRankCapRequest request
        ) =>
            new ApplyRankCapTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.ApplyRankCapResult> ApplyRankCapAsync(
                Request.ApplyRankCapRequest request
        ) =>
            new ApplyRankCapTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.ApplyRankCapResult>();
    #else
		public ApplyRankCapTask ApplyRankCapAsync(
                Request.ApplyRankCapRequest request
        )
		{
			return new ApplyRankCapTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.ApplyRankCapResult> ApplyRankCapAsync(
                Request.ApplyRankCapRequest request
        ) =>
            new ApplyRankCapTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class ApplyRankCapByUserIdTask : Gs2RestSessionTask<ApplyRankCapByUserIdRequest, ApplyRankCapByUserIdResult>
        {
            public ApplyRankCapByUserIdTask(IGs2Session session, RestSessionRequestFactory factory, ApplyRankCapByUserIdRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(ApplyRankCapByUserIdRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "grade")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/{userId}/status/model/{gradeName}/property/{propertyId}/apply/rank/cap";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{userId}", !string.IsNullOrEmpty(request.UserId) ? request.UserId.ToString() : "null");
                url = url.Replace("{gradeName}", !string.IsNullOrEmpty(request.GradeName) ? request.GradeName.ToString() : "null");
                url = url.Replace("{propertyId}", !string.IsNullOrEmpty(request.PropertyId) ? request.PropertyId.ToString() : "null");

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
		public IEnumerator ApplyRankCapByUserId(
                Request.ApplyRankCapByUserIdRequest request,
                UnityAction<AsyncResult<Result.ApplyRankCapByUserIdResult>> callback
        ) =>
            new ApplyRankCapByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.ApplyRankCapByUserIdResult> ApplyRankCapByUserIdFuture(
                Request.ApplyRankCapByUserIdRequest request
        ) =>
            new ApplyRankCapByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.ApplyRankCapByUserIdResult> ApplyRankCapByUserIdAsync(
                Request.ApplyRankCapByUserIdRequest request
        ) =>
            new ApplyRankCapByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.ApplyRankCapByUserIdResult>();
    #else
		public ApplyRankCapByUserIdTask ApplyRankCapByUserIdAsync(
                Request.ApplyRankCapByUserIdRequest request
        )
		{
			return new ApplyRankCapByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.ApplyRankCapByUserIdResult> ApplyRankCapByUserIdAsync(
                Request.ApplyRankCapByUserIdRequest request
        ) =>
            new ApplyRankCapByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class DeleteStatusByUserIdTask : Gs2RestSessionTask<DeleteStatusByUserIdRequest, DeleteStatusByUserIdResult>
        {
            public DeleteStatusByUserIdTask(IGs2Session session, RestSessionRequestFactory factory, DeleteStatusByUserIdRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DeleteStatusByUserIdRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "grade")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/{userId}/status/model/{gradeName}/property/{propertyId}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{userId}", !string.IsNullOrEmpty(request.UserId) ? request.UserId.ToString() : "null");
                url = url.Replace("{gradeName}", !string.IsNullOrEmpty(request.GradeName) ? request.GradeName.ToString() : "null");
                url = url.Replace("{propertyId}", !string.IsNullOrEmpty(request.PropertyId) ? request.PropertyId.ToString() : "null");

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
		public IEnumerator DeleteStatusByUserId(
                Request.DeleteStatusByUserIdRequest request,
                UnityAction<AsyncResult<Result.DeleteStatusByUserIdResult>> callback
        ) =>
            new DeleteStatusByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.DeleteStatusByUserIdResult> DeleteStatusByUserIdFuture(
                Request.DeleteStatusByUserIdRequest request
        ) =>
            new DeleteStatusByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.DeleteStatusByUserIdResult> DeleteStatusByUserIdAsync(
                Request.DeleteStatusByUserIdRequest request
        ) =>
            new DeleteStatusByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.DeleteStatusByUserIdResult>();
    #else
		public DeleteStatusByUserIdTask DeleteStatusByUserIdAsync(
                Request.DeleteStatusByUserIdRequest request
        )
		{
			return new DeleteStatusByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.DeleteStatusByUserIdResult> DeleteStatusByUserIdAsync(
                Request.DeleteStatusByUserIdRequest request
        ) =>
            new DeleteStatusByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class VerifyGradeTask : Gs2RestSessionTask<VerifyGradeRequest, VerifyGradeResult>
        {
            public VerifyGradeTask(IGs2Session session, RestSessionRequestFactory factory, VerifyGradeRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(VerifyGradeRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "grade")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/me/status/{gradeName}/verify/grade/{verifyType}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{gradeName}", !string.IsNullOrEmpty(request.GradeName) ? request.GradeName.ToString() : "null");
                url = url.Replace("{verifyType}", !string.IsNullOrEmpty(request.VerifyType) ? request.VerifyType.ToString() : "null");

                var sessionRequest = Factory.Post(url);

                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);
                jsonWriter.WriteObjectStart();
                if (request.PropertyId != null)
                {
                    jsonWriter.WritePropertyName("propertyId");
                    jsonWriter.Write(request.PropertyId);
                }
                if (request.GradeValue != null)
                {
                    jsonWriter.WritePropertyName("gradeValue");
                    jsonWriter.Write(request.GradeValue.ToString());
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
		public IEnumerator VerifyGrade(
                Request.VerifyGradeRequest request,
                UnityAction<AsyncResult<Result.VerifyGradeResult>> callback
        ) =>
            new VerifyGradeTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.VerifyGradeResult> VerifyGradeFuture(
                Request.VerifyGradeRequest request
        ) =>
            new VerifyGradeTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.VerifyGradeResult> VerifyGradeAsync(
                Request.VerifyGradeRequest request
        ) =>
            new VerifyGradeTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.VerifyGradeResult>();
    #else
		public VerifyGradeTask VerifyGradeAsync(
                Request.VerifyGradeRequest request
        )
		{
			return new VerifyGradeTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.VerifyGradeResult> VerifyGradeAsync(
                Request.VerifyGradeRequest request
        ) =>
            new VerifyGradeTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class VerifyGradeByUserIdTask : Gs2RestSessionTask<VerifyGradeByUserIdRequest, VerifyGradeByUserIdResult>
        {
            public VerifyGradeByUserIdTask(IGs2Session session, RestSessionRequestFactory factory, VerifyGradeByUserIdRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(VerifyGradeByUserIdRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "grade")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/{userId}/status/{gradeName}/verify/grade/{verifyType}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{userId}", !string.IsNullOrEmpty(request.UserId) ? request.UserId.ToString() : "null");
                url = url.Replace("{gradeName}", !string.IsNullOrEmpty(request.GradeName) ? request.GradeName.ToString() : "null");
                url = url.Replace("{verifyType}", !string.IsNullOrEmpty(request.VerifyType) ? request.VerifyType.ToString() : "null");

                var sessionRequest = Factory.Post(url);

                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);
                jsonWriter.WriteObjectStart();
                if (request.PropertyId != null)
                {
                    jsonWriter.WritePropertyName("propertyId");
                    jsonWriter.Write(request.PropertyId);
                }
                if (request.GradeValue != null)
                {
                    jsonWriter.WritePropertyName("gradeValue");
                    jsonWriter.Write(request.GradeValue.ToString());
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
		public IEnumerator VerifyGradeByUserId(
                Request.VerifyGradeByUserIdRequest request,
                UnityAction<AsyncResult<Result.VerifyGradeByUserIdResult>> callback
        ) =>
            new VerifyGradeByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.VerifyGradeByUserIdResult> VerifyGradeByUserIdFuture(
                Request.VerifyGradeByUserIdRequest request
        ) =>
            new VerifyGradeByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.VerifyGradeByUserIdResult> VerifyGradeByUserIdAsync(
                Request.VerifyGradeByUserIdRequest request
        ) =>
            new VerifyGradeByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.VerifyGradeByUserIdResult>();
    #else
		public VerifyGradeByUserIdTask VerifyGradeByUserIdAsync(
                Request.VerifyGradeByUserIdRequest request
        )
		{
			return new VerifyGradeByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.VerifyGradeByUserIdResult> VerifyGradeByUserIdAsync(
                Request.VerifyGradeByUserIdRequest request
        ) =>
            new VerifyGradeByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class VerifyGradeUpMaterialTask : Gs2RestSessionTask<VerifyGradeUpMaterialRequest, VerifyGradeUpMaterialResult>
        {
            public VerifyGradeUpMaterialTask(IGs2Session session, RestSessionRequestFactory factory, VerifyGradeUpMaterialRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(VerifyGradeUpMaterialRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "grade")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/me/status/{gradeName}/verify/material/{verifyType}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{gradeName}", !string.IsNullOrEmpty(request.GradeName) ? request.GradeName.ToString() : "null");
                url = url.Replace("{verifyType}", !string.IsNullOrEmpty(request.VerifyType) ? request.VerifyType.ToString() : "null");

                var sessionRequest = Factory.Post(url);

                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);
                jsonWriter.WriteObjectStart();
                if (request.PropertyId != null)
                {
                    jsonWriter.WritePropertyName("propertyId");
                    jsonWriter.Write(request.PropertyId);
                }
                if (request.MaterialPropertyId != null)
                {
                    jsonWriter.WritePropertyName("materialPropertyId");
                    jsonWriter.Write(request.MaterialPropertyId);
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
		public IEnumerator VerifyGradeUpMaterial(
                Request.VerifyGradeUpMaterialRequest request,
                UnityAction<AsyncResult<Result.VerifyGradeUpMaterialResult>> callback
        ) =>
            new VerifyGradeUpMaterialTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.VerifyGradeUpMaterialResult> VerifyGradeUpMaterialFuture(
                Request.VerifyGradeUpMaterialRequest request
        ) =>
            new VerifyGradeUpMaterialTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.VerifyGradeUpMaterialResult> VerifyGradeUpMaterialAsync(
                Request.VerifyGradeUpMaterialRequest request
        ) =>
            new VerifyGradeUpMaterialTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.VerifyGradeUpMaterialResult>();
    #else
		public VerifyGradeUpMaterialTask VerifyGradeUpMaterialAsync(
                Request.VerifyGradeUpMaterialRequest request
        )
		{
			return new VerifyGradeUpMaterialTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.VerifyGradeUpMaterialResult> VerifyGradeUpMaterialAsync(
                Request.VerifyGradeUpMaterialRequest request
        ) =>
            new VerifyGradeUpMaterialTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class VerifyGradeUpMaterialByUserIdTask : Gs2RestSessionTask<VerifyGradeUpMaterialByUserIdRequest, VerifyGradeUpMaterialByUserIdResult>
        {
            public VerifyGradeUpMaterialByUserIdTask(IGs2Session session, RestSessionRequestFactory factory, VerifyGradeUpMaterialByUserIdRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(VerifyGradeUpMaterialByUserIdRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "grade")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/{userId}/status/{gradeName}/verify/material/{verifyType}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{userId}", !string.IsNullOrEmpty(request.UserId) ? request.UserId.ToString() : "null");
                url = url.Replace("{gradeName}", !string.IsNullOrEmpty(request.GradeName) ? request.GradeName.ToString() : "null");
                url = url.Replace("{verifyType}", !string.IsNullOrEmpty(request.VerifyType) ? request.VerifyType.ToString() : "null");

                var sessionRequest = Factory.Post(url);

                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);
                jsonWriter.WriteObjectStart();
                if (request.PropertyId != null)
                {
                    jsonWriter.WritePropertyName("propertyId");
                    jsonWriter.Write(request.PropertyId);
                }
                if (request.MaterialPropertyId != null)
                {
                    jsonWriter.WritePropertyName("materialPropertyId");
                    jsonWriter.Write(request.MaterialPropertyId);
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
		public IEnumerator VerifyGradeUpMaterialByUserId(
                Request.VerifyGradeUpMaterialByUserIdRequest request,
                UnityAction<AsyncResult<Result.VerifyGradeUpMaterialByUserIdResult>> callback
        ) =>
            new VerifyGradeUpMaterialByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.VerifyGradeUpMaterialByUserIdResult> VerifyGradeUpMaterialByUserIdFuture(
                Request.VerifyGradeUpMaterialByUserIdRequest request
        ) =>
            new VerifyGradeUpMaterialByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.VerifyGradeUpMaterialByUserIdResult> VerifyGradeUpMaterialByUserIdAsync(
                Request.VerifyGradeUpMaterialByUserIdRequest request
        ) =>
            new VerifyGradeUpMaterialByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.VerifyGradeUpMaterialByUserIdResult>();
    #else
		public VerifyGradeUpMaterialByUserIdTask VerifyGradeUpMaterialByUserIdAsync(
                Request.VerifyGradeUpMaterialByUserIdRequest request
        )
		{
			return new VerifyGradeUpMaterialByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.VerifyGradeUpMaterialByUserIdResult> VerifyGradeUpMaterialByUserIdAsync(
                Request.VerifyGradeUpMaterialByUserIdRequest request
        ) =>
            new VerifyGradeUpMaterialByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class AddGradeByStampSheetTask : Gs2RestSessionTask<AddGradeByStampSheetRequest, AddGradeByStampSheetResult>
        {
            public AddGradeByStampSheetTask(IGs2Session session, RestSessionRequestFactory factory, AddGradeByStampSheetRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(AddGradeByStampSheetRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "grade")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/stamp/grade/add";

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
		public IEnumerator AddGradeByStampSheet(
                Request.AddGradeByStampSheetRequest request,
                UnityAction<AsyncResult<Result.AddGradeByStampSheetResult>> callback
        ) =>
            new AddGradeByStampSheetTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.AddGradeByStampSheetResult> AddGradeByStampSheetFuture(
                Request.AddGradeByStampSheetRequest request
        ) =>
            new AddGradeByStampSheetTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.AddGradeByStampSheetResult> AddGradeByStampSheetAsync(
                Request.AddGradeByStampSheetRequest request
        ) =>
            new AddGradeByStampSheetTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.AddGradeByStampSheetResult>();
    #else
		public AddGradeByStampSheetTask AddGradeByStampSheetAsync(
                Request.AddGradeByStampSheetRequest request
        )
		{
			return new AddGradeByStampSheetTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.AddGradeByStampSheetResult> AddGradeByStampSheetAsync(
                Request.AddGradeByStampSheetRequest request
        ) =>
            new AddGradeByStampSheetTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class ApplyRankCapByStampSheetTask : Gs2RestSessionTask<ApplyRankCapByStampSheetRequest, ApplyRankCapByStampSheetResult>
        {
            public ApplyRankCapByStampSheetTask(IGs2Session session, RestSessionRequestFactory factory, ApplyRankCapByStampSheetRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(ApplyRankCapByStampSheetRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "grade")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/stamp/apply/rank/cap";

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
		public IEnumerator ApplyRankCapByStampSheet(
                Request.ApplyRankCapByStampSheetRequest request,
                UnityAction<AsyncResult<Result.ApplyRankCapByStampSheetResult>> callback
        ) =>
            new ApplyRankCapByStampSheetTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.ApplyRankCapByStampSheetResult> ApplyRankCapByStampSheetFuture(
                Request.ApplyRankCapByStampSheetRequest request
        ) =>
            new ApplyRankCapByStampSheetTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.ApplyRankCapByStampSheetResult> ApplyRankCapByStampSheetAsync(
                Request.ApplyRankCapByStampSheetRequest request
        ) =>
            new ApplyRankCapByStampSheetTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.ApplyRankCapByStampSheetResult>();
    #else
		public ApplyRankCapByStampSheetTask ApplyRankCapByStampSheetAsync(
                Request.ApplyRankCapByStampSheetRequest request
        )
		{
			return new ApplyRankCapByStampSheetTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.ApplyRankCapByStampSheetResult> ApplyRankCapByStampSheetAsync(
                Request.ApplyRankCapByStampSheetRequest request
        ) =>
            new ApplyRankCapByStampSheetTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class SubGradeByStampTaskTask : Gs2RestSessionTask<SubGradeByStampTaskRequest, SubGradeByStampTaskResult>
        {
            public SubGradeByStampTaskTask(IGs2Session session, RestSessionRequestFactory factory, SubGradeByStampTaskRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(SubGradeByStampTaskRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "grade")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/stamp/grade/sub";

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
		public IEnumerator SubGradeByStampTask(
                Request.SubGradeByStampTaskRequest request,
                UnityAction<AsyncResult<Result.SubGradeByStampTaskResult>> callback
        ) =>
            new SubGradeByStampTaskTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.SubGradeByStampTaskResult> SubGradeByStampTaskFuture(
                Request.SubGradeByStampTaskRequest request
        ) =>
            new SubGradeByStampTaskTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.SubGradeByStampTaskResult> SubGradeByStampTaskAsync(
                Request.SubGradeByStampTaskRequest request
        ) =>
            new SubGradeByStampTaskTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.SubGradeByStampTaskResult>();
    #else
		public SubGradeByStampTaskTask SubGradeByStampTaskAsync(
                Request.SubGradeByStampTaskRequest request
        )
		{
			return new SubGradeByStampTaskTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.SubGradeByStampTaskResult> SubGradeByStampTaskAsync(
                Request.SubGradeByStampTaskRequest request
        ) =>
            new SubGradeByStampTaskTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class MultiplyAcquireActionsByUserIdTask : Gs2RestSessionTask<MultiplyAcquireActionsByUserIdRequest, MultiplyAcquireActionsByUserIdResult>
        {
            public MultiplyAcquireActionsByUserIdTask(IGs2Session session, RestSessionRequestFactory factory, MultiplyAcquireActionsByUserIdRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(MultiplyAcquireActionsByUserIdRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "grade")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/{userId}/status/model/{gradeName}/property/{propertyId}/acquire/rate/{rateName}/multiply";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{userId}", !string.IsNullOrEmpty(request.UserId) ? request.UserId.ToString() : "null");
                url = url.Replace("{gradeName}", !string.IsNullOrEmpty(request.GradeName) ? request.GradeName.ToString() : "null");
                url = url.Replace("{propertyId}", !string.IsNullOrEmpty(request.PropertyId) ? request.PropertyId.ToString() : "null");
                url = url.Replace("{rateName}", !string.IsNullOrEmpty(request.RateName) ? request.RateName.ToString() : "null");

                var sessionRequest = Factory.Post(url);

                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);
                jsonWriter.WriteObjectStart();
                if (request.AcquireActions != null)
                {
                    jsonWriter.WritePropertyName("acquireActions");
                    jsonWriter.WriteArrayStart();
                    foreach(var item in request.AcquireActions)
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
		public IEnumerator MultiplyAcquireActionsByUserId(
                Request.MultiplyAcquireActionsByUserIdRequest request,
                UnityAction<AsyncResult<Result.MultiplyAcquireActionsByUserIdResult>> callback
        ) =>
            new MultiplyAcquireActionsByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.MultiplyAcquireActionsByUserIdResult> MultiplyAcquireActionsByUserIdFuture(
                Request.MultiplyAcquireActionsByUserIdRequest request
        ) =>
            new MultiplyAcquireActionsByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.MultiplyAcquireActionsByUserIdResult> MultiplyAcquireActionsByUserIdAsync(
                Request.MultiplyAcquireActionsByUserIdRequest request
        ) =>
            new MultiplyAcquireActionsByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.MultiplyAcquireActionsByUserIdResult>();
    #else
		public MultiplyAcquireActionsByUserIdTask MultiplyAcquireActionsByUserIdAsync(
                Request.MultiplyAcquireActionsByUserIdRequest request
        )
		{
			return new MultiplyAcquireActionsByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.MultiplyAcquireActionsByUserIdResult> MultiplyAcquireActionsByUserIdAsync(
                Request.MultiplyAcquireActionsByUserIdRequest request
        ) =>
            new MultiplyAcquireActionsByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class MultiplyAcquireActionsByStampSheetTask : Gs2RestSessionTask<MultiplyAcquireActionsByStampSheetRequest, MultiplyAcquireActionsByStampSheetResult>
        {
            public MultiplyAcquireActionsByStampSheetTask(IGs2Session session, RestSessionRequestFactory factory, MultiplyAcquireActionsByStampSheetRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(MultiplyAcquireActionsByStampSheetRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "grade")
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
                if (request.DryRun)
                {
                    sessionRequest.AddHeader("X-GS2-DRY-RUN", "true");
                }

                AddHeader(
                    Session.Credential,
                    sessionRequest
                );

                return sessionRequest;
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator MultiplyAcquireActionsByStampSheet(
                Request.MultiplyAcquireActionsByStampSheetRequest request,
                UnityAction<AsyncResult<Result.MultiplyAcquireActionsByStampSheetResult>> callback
        ) =>
            new MultiplyAcquireActionsByStampSheetTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.MultiplyAcquireActionsByStampSheetResult> MultiplyAcquireActionsByStampSheetFuture(
                Request.MultiplyAcquireActionsByStampSheetRequest request
        ) =>
            new MultiplyAcquireActionsByStampSheetTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.MultiplyAcquireActionsByStampSheetResult> MultiplyAcquireActionsByStampSheetAsync(
                Request.MultiplyAcquireActionsByStampSheetRequest request
        ) =>
            new MultiplyAcquireActionsByStampSheetTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.MultiplyAcquireActionsByStampSheetResult>();
    #else
		public MultiplyAcquireActionsByStampSheetTask MultiplyAcquireActionsByStampSheetAsync(
                Request.MultiplyAcquireActionsByStampSheetRequest request
        )
		{
			return new MultiplyAcquireActionsByStampSheetTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.MultiplyAcquireActionsByStampSheetResult> MultiplyAcquireActionsByStampSheetAsync(
                Request.MultiplyAcquireActionsByStampSheetRequest request
        ) =>
            new MultiplyAcquireActionsByStampSheetTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class VerifyGradeByStampTaskTask : Gs2RestSessionTask<VerifyGradeByStampTaskRequest, VerifyGradeByStampTaskResult>
        {
            public VerifyGradeByStampTaskTask(IGs2Session session, RestSessionRequestFactory factory, VerifyGradeByStampTaskRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(VerifyGradeByStampTaskRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "grade")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/stamp/grade/verify";

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
		public IEnumerator VerifyGradeByStampTask(
                Request.VerifyGradeByStampTaskRequest request,
                UnityAction<AsyncResult<Result.VerifyGradeByStampTaskResult>> callback
        ) =>
            new VerifyGradeByStampTaskTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.VerifyGradeByStampTaskResult> VerifyGradeByStampTaskFuture(
                Request.VerifyGradeByStampTaskRequest request
        ) =>
            new VerifyGradeByStampTaskTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.VerifyGradeByStampTaskResult> VerifyGradeByStampTaskAsync(
                Request.VerifyGradeByStampTaskRequest request
        ) =>
            new VerifyGradeByStampTaskTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.VerifyGradeByStampTaskResult>();
    #else
		public VerifyGradeByStampTaskTask VerifyGradeByStampTaskAsync(
                Request.VerifyGradeByStampTaskRequest request
        )
		{
			return new VerifyGradeByStampTaskTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.VerifyGradeByStampTaskResult> VerifyGradeByStampTaskAsync(
                Request.VerifyGradeByStampTaskRequest request
        ) =>
            new VerifyGradeByStampTaskTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class VerifyGradeUpMaterialByStampTaskTask : Gs2RestSessionTask<VerifyGradeUpMaterialByStampTaskRequest, VerifyGradeUpMaterialByStampTaskResult>
        {
            public VerifyGradeUpMaterialByStampTaskTask(IGs2Session session, RestSessionRequestFactory factory, VerifyGradeUpMaterialByStampTaskRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(VerifyGradeUpMaterialByStampTaskRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "grade")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/stamp/material/verify";

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
		public IEnumerator VerifyGradeUpMaterialByStampTask(
                Request.VerifyGradeUpMaterialByStampTaskRequest request,
                UnityAction<AsyncResult<Result.VerifyGradeUpMaterialByStampTaskResult>> callback
        ) =>
            new VerifyGradeUpMaterialByStampTaskTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.VerifyGradeUpMaterialByStampTaskResult> VerifyGradeUpMaterialByStampTaskFuture(
                Request.VerifyGradeUpMaterialByStampTaskRequest request
        ) =>
            new VerifyGradeUpMaterialByStampTaskTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.VerifyGradeUpMaterialByStampTaskResult> VerifyGradeUpMaterialByStampTaskAsync(
                Request.VerifyGradeUpMaterialByStampTaskRequest request
        ) =>
            new VerifyGradeUpMaterialByStampTaskTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.VerifyGradeUpMaterialByStampTaskResult>();
    #else
		public VerifyGradeUpMaterialByStampTaskTask VerifyGradeUpMaterialByStampTaskAsync(
                Request.VerifyGradeUpMaterialByStampTaskRequest request
        )
		{
			return new VerifyGradeUpMaterialByStampTaskTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.VerifyGradeUpMaterialByStampTaskResult> VerifyGradeUpMaterialByStampTaskAsync(
                Request.VerifyGradeUpMaterialByStampTaskRequest request
        ) =>
            new VerifyGradeUpMaterialByStampTaskTask(
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
                    .Replace("{service}", "grade")
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


        public class GetCurrentGradeMasterTask : Gs2RestSessionTask<GetCurrentGradeMasterRequest, GetCurrentGradeMasterResult>
        {
            public GetCurrentGradeMasterTask(IGs2Session session, RestSessionRequestFactory factory, GetCurrentGradeMasterRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(GetCurrentGradeMasterRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "grade")
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
		public IEnumerator GetCurrentGradeMaster(
                Request.GetCurrentGradeMasterRequest request,
                UnityAction<AsyncResult<Result.GetCurrentGradeMasterResult>> callback
        ) =>
            new GetCurrentGradeMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.GetCurrentGradeMasterResult> GetCurrentGradeMasterFuture(
                Request.GetCurrentGradeMasterRequest request
        ) =>
            new GetCurrentGradeMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.GetCurrentGradeMasterResult> GetCurrentGradeMasterAsync(
                Request.GetCurrentGradeMasterRequest request
        ) =>
            new GetCurrentGradeMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.GetCurrentGradeMasterResult>();
    #else
		public GetCurrentGradeMasterTask GetCurrentGradeMasterAsync(
                Request.GetCurrentGradeMasterRequest request
        )
		{
			return new GetCurrentGradeMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.GetCurrentGradeMasterResult> GetCurrentGradeMasterAsync(
                Request.GetCurrentGradeMasterRequest request
        ) =>
            new GetCurrentGradeMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class PreUpdateCurrentGradeMasterTask : Gs2RestSessionTask<PreUpdateCurrentGradeMasterRequest, PreUpdateCurrentGradeMasterResult>
        {
            public PreUpdateCurrentGradeMasterTask(IGs2Session session, RestSessionRequestFactory factory, PreUpdateCurrentGradeMasterRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(PreUpdateCurrentGradeMasterRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "grade")
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
		public IEnumerator PreUpdateCurrentGradeMaster(
                Request.PreUpdateCurrentGradeMasterRequest request,
                UnityAction<AsyncResult<Result.PreUpdateCurrentGradeMasterResult>> callback
        ) =>
            new PreUpdateCurrentGradeMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.PreUpdateCurrentGradeMasterResult> PreUpdateCurrentGradeMasterFuture(
                Request.PreUpdateCurrentGradeMasterRequest request
        ) =>
            new PreUpdateCurrentGradeMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.PreUpdateCurrentGradeMasterResult> PreUpdateCurrentGradeMasterAsync(
                Request.PreUpdateCurrentGradeMasterRequest request
        ) =>
            new PreUpdateCurrentGradeMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.PreUpdateCurrentGradeMasterResult>();
    #else
		public PreUpdateCurrentGradeMasterTask PreUpdateCurrentGradeMasterAsync(
                Request.PreUpdateCurrentGradeMasterRequest request
        )
		{
			return new PreUpdateCurrentGradeMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.PreUpdateCurrentGradeMasterResult> PreUpdateCurrentGradeMasterAsync(
                Request.PreUpdateCurrentGradeMasterRequest request
        ) =>
            new PreUpdateCurrentGradeMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class UpdateCurrentGradeMasterTask : Gs2RestSessionTask<UpdateCurrentGradeMasterRequest, UpdateCurrentGradeMasterResult>
        {
            public UpdateCurrentGradeMasterTask(IGs2Session session, RestSessionRequestFactory factory, UpdateCurrentGradeMasterRequest request) : base(session, factory, request)
            {
            }
#if GS2_ENABLE_UNITASK
            protected override async UniTask<UpdateCurrentGradeMasterResult> InvokeImpl()
#else
            protected override async Task<UpdateCurrentGradeMasterResult> InvokeImpl()
#endif
            {
                if (Request.Settings != null) {
                    var preTask = new PreUpdateCurrentGradeMasterTask(
                        Session,
                        Factory,
                        new PreUpdateCurrentGradeMasterRequest()
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

            protected override IGs2SessionRequest CreateRequest(UpdateCurrentGradeMasterRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "grade")
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
		public IEnumerator UpdateCurrentGradeMaster(
                Request.UpdateCurrentGradeMasterRequest request,
                UnityAction<AsyncResult<Result.UpdateCurrentGradeMasterResult>> callback
        ) =>
            new UpdateCurrentGradeMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.UpdateCurrentGradeMasterResult> UpdateCurrentGradeMasterFuture(
                Request.UpdateCurrentGradeMasterRequest request
        ) =>
            new UpdateCurrentGradeMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.UpdateCurrentGradeMasterResult> UpdateCurrentGradeMasterAsync(
                Request.UpdateCurrentGradeMasterRequest request
        ) =>
            new UpdateCurrentGradeMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.UpdateCurrentGradeMasterResult>();
    #else
		public UpdateCurrentGradeMasterTask UpdateCurrentGradeMasterAsync(
                Request.UpdateCurrentGradeMasterRequest request
        )
		{
			return new UpdateCurrentGradeMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.UpdateCurrentGradeMasterResult> UpdateCurrentGradeMasterAsync(
                Request.UpdateCurrentGradeMasterRequest request
        ) =>
            new UpdateCurrentGradeMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class UpdateCurrentGradeMasterFromGitHubTask : Gs2RestSessionTask<UpdateCurrentGradeMasterFromGitHubRequest, UpdateCurrentGradeMasterFromGitHubResult>
        {
            public UpdateCurrentGradeMasterFromGitHubTask(IGs2Session session, RestSessionRequestFactory factory, UpdateCurrentGradeMasterFromGitHubRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(UpdateCurrentGradeMasterFromGitHubRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "grade")
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
		public IEnumerator UpdateCurrentGradeMasterFromGitHub(
                Request.UpdateCurrentGradeMasterFromGitHubRequest request,
                UnityAction<AsyncResult<Result.UpdateCurrentGradeMasterFromGitHubResult>> callback
        ) =>
            new UpdateCurrentGradeMasterFromGitHubTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.UpdateCurrentGradeMasterFromGitHubResult> UpdateCurrentGradeMasterFromGitHubFuture(
                Request.UpdateCurrentGradeMasterFromGitHubRequest request
        ) =>
            new UpdateCurrentGradeMasterFromGitHubTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.UpdateCurrentGradeMasterFromGitHubResult> UpdateCurrentGradeMasterFromGitHubAsync(
                Request.UpdateCurrentGradeMasterFromGitHubRequest request
        ) =>
            new UpdateCurrentGradeMasterFromGitHubTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.UpdateCurrentGradeMasterFromGitHubResult>();
    #else
		public UpdateCurrentGradeMasterFromGitHubTask UpdateCurrentGradeMasterFromGitHubAsync(
                Request.UpdateCurrentGradeMasterFromGitHubRequest request
        )
		{
			return new UpdateCurrentGradeMasterFromGitHubTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.UpdateCurrentGradeMasterFromGitHubResult> UpdateCurrentGradeMasterFromGitHubAsync(
                Request.UpdateCurrentGradeMasterFromGitHubRequest request
        ) =>
            new UpdateCurrentGradeMasterFromGitHubTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif
	}
}