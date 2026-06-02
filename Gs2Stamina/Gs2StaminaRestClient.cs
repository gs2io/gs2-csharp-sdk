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
using Gs2.Gs2Stamina.Request;
using Gs2.Gs2Stamina.Result;
using Gs2.Util.LitJson;

namespace Gs2.Gs2Stamina
{
	public class Gs2StaminaRestClient : AbstractGs2Client
	{
#if UNITY_2017_1_OR_NEWER
		private readonly CertificateHandler _certificateHandler;
#endif

		public static string Endpoint = "stamina";

        protected Gs2RestSession Gs2RestSession => (Gs2RestSession) Gs2Session;

		public Gs2StaminaRestClient(Gs2RestSession Gs2RestSession) : base(Gs2RestSession)
		{

		}

#if UNITY_2017_1_OR_NEWER
		public Gs2StaminaRestClient(Gs2RestSession gs2RestSession, CertificateHandler certificateHandler) : base(gs2RestSession)
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
                    .Replace("{service}", "stamina")
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
                    .Replace("{service}", "stamina")
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
                if (request.OverflowTriggerScript != null)
                {
                    jsonWriter.WritePropertyName("overflowTriggerScript");
                    jsonWriter.Write(request.OverflowTriggerScript);
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
                    .Replace("{service}", "stamina")
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
                    .Replace("{service}", "stamina")
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
                    .Replace("{service}", "stamina")
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
                if (request.OverflowTriggerScript != null)
                {
                    jsonWriter.WritePropertyName("overflowTriggerScript");
                    jsonWriter.Write(request.OverflowTriggerScript);
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
                    .Replace("{service}", "stamina")
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
                    .Replace("{service}", "stamina")
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
                    .Replace("{service}", "stamina")
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
                    .Replace("{service}", "stamina")
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
                    .Replace("{service}", "stamina")
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
                    .Replace("{service}", "stamina")
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
                    .Replace("{service}", "stamina")
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
                    .Replace("{service}", "stamina")
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
                    .Replace("{service}", "stamina")
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


        public class DescribeStaminaModelMastersTask : Gs2RestSessionTask<DescribeStaminaModelMastersRequest, DescribeStaminaModelMastersResult>
        {
            public DescribeStaminaModelMastersTask(IGs2Session session, RestSessionRequestFactory factory, DescribeStaminaModelMastersRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DescribeStaminaModelMastersRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "stamina")
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
		public IEnumerator DescribeStaminaModelMasters(
                Request.DescribeStaminaModelMastersRequest request,
                UnityAction<AsyncResult<Result.DescribeStaminaModelMastersResult>> callback
        ) =>
            new DescribeStaminaModelMastersTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.DescribeStaminaModelMastersResult> DescribeStaminaModelMastersFuture(
                Request.DescribeStaminaModelMastersRequest request
        ) =>
            new DescribeStaminaModelMastersTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.DescribeStaminaModelMastersResult> DescribeStaminaModelMastersAsync(
                Request.DescribeStaminaModelMastersRequest request
        ) =>
            new DescribeStaminaModelMastersTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.DescribeStaminaModelMastersResult>();
    #else
		public DescribeStaminaModelMastersTask DescribeStaminaModelMastersAsync(
                Request.DescribeStaminaModelMastersRequest request
        )
		{
			return new DescribeStaminaModelMastersTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.DescribeStaminaModelMastersResult> DescribeStaminaModelMastersAsync(
                Request.DescribeStaminaModelMastersRequest request
        ) =>
            new DescribeStaminaModelMastersTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class CreateStaminaModelMasterTask : Gs2RestSessionTask<CreateStaminaModelMasterRequest, CreateStaminaModelMasterResult>
        {
            public CreateStaminaModelMasterTask(IGs2Session session, RestSessionRequestFactory factory, CreateStaminaModelMasterRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(CreateStaminaModelMasterRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "stamina")
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
                if (request.RecoverIntervalMinutes != null)
                {
                    jsonWriter.WritePropertyName("recoverIntervalMinutes");
                    jsonWriter.Write(request.RecoverIntervalMinutes.ToString());
                }
                if (request.RecoverValue != null)
                {
                    jsonWriter.WritePropertyName("recoverValue");
                    jsonWriter.Write(request.RecoverValue.ToString());
                }
                if (request.InitialCapacity != null)
                {
                    jsonWriter.WritePropertyName("initialCapacity");
                    jsonWriter.Write(request.InitialCapacity.ToString());
                }
                if (request.IsOverflow != null)
                {
                    jsonWriter.WritePropertyName("isOverflow");
                    jsonWriter.Write(request.IsOverflow.ToString());
                }
                if (request.MaxCapacity != null)
                {
                    jsonWriter.WritePropertyName("maxCapacity");
                    jsonWriter.Write(request.MaxCapacity.ToString());
                }
                if (request.MaxStaminaTableName != null)
                {
                    jsonWriter.WritePropertyName("maxStaminaTableName");
                    jsonWriter.Write(request.MaxStaminaTableName);
                }
                if (request.RecoverIntervalTableName != null)
                {
                    jsonWriter.WritePropertyName("recoverIntervalTableName");
                    jsonWriter.Write(request.RecoverIntervalTableName);
                }
                if (request.RecoverValueTableName != null)
                {
                    jsonWriter.WritePropertyName("recoverValueTableName");
                    jsonWriter.Write(request.RecoverValueTableName);
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
		public IEnumerator CreateStaminaModelMaster(
                Request.CreateStaminaModelMasterRequest request,
                UnityAction<AsyncResult<Result.CreateStaminaModelMasterResult>> callback
        ) =>
            new CreateStaminaModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.CreateStaminaModelMasterResult> CreateStaminaModelMasterFuture(
                Request.CreateStaminaModelMasterRequest request
        ) =>
            new CreateStaminaModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.CreateStaminaModelMasterResult> CreateStaminaModelMasterAsync(
                Request.CreateStaminaModelMasterRequest request
        ) =>
            new CreateStaminaModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.CreateStaminaModelMasterResult>();
    #else
		public CreateStaminaModelMasterTask CreateStaminaModelMasterAsync(
                Request.CreateStaminaModelMasterRequest request
        )
		{
			return new CreateStaminaModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.CreateStaminaModelMasterResult> CreateStaminaModelMasterAsync(
                Request.CreateStaminaModelMasterRequest request
        ) =>
            new CreateStaminaModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class GetStaminaModelMasterTask : Gs2RestSessionTask<GetStaminaModelMasterRequest, GetStaminaModelMasterResult>
        {
            public GetStaminaModelMasterTask(IGs2Session session, RestSessionRequestFactory factory, GetStaminaModelMasterRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(GetStaminaModelMasterRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "stamina")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/master/model/{staminaName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{staminaName}", !string.IsNullOrEmpty(request.StaminaName) ? request.StaminaName.ToString() : "null");

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
		public IEnumerator GetStaminaModelMaster(
                Request.GetStaminaModelMasterRequest request,
                UnityAction<AsyncResult<Result.GetStaminaModelMasterResult>> callback
        ) =>
            new GetStaminaModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.GetStaminaModelMasterResult> GetStaminaModelMasterFuture(
                Request.GetStaminaModelMasterRequest request
        ) =>
            new GetStaminaModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.GetStaminaModelMasterResult> GetStaminaModelMasterAsync(
                Request.GetStaminaModelMasterRequest request
        ) =>
            new GetStaminaModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.GetStaminaModelMasterResult>();
    #else
		public GetStaminaModelMasterTask GetStaminaModelMasterAsync(
                Request.GetStaminaModelMasterRequest request
        )
		{
			return new GetStaminaModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.GetStaminaModelMasterResult> GetStaminaModelMasterAsync(
                Request.GetStaminaModelMasterRequest request
        ) =>
            new GetStaminaModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class UpdateStaminaModelMasterTask : Gs2RestSessionTask<UpdateStaminaModelMasterRequest, UpdateStaminaModelMasterResult>
        {
            public UpdateStaminaModelMasterTask(IGs2Session session, RestSessionRequestFactory factory, UpdateStaminaModelMasterRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(UpdateStaminaModelMasterRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "stamina")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/master/model/{staminaName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{staminaName}", !string.IsNullOrEmpty(request.StaminaName) ? request.StaminaName.ToString() : "null");

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
                if (request.RecoverIntervalMinutes != null)
                {
                    jsonWriter.WritePropertyName("recoverIntervalMinutes");
                    jsonWriter.Write(request.RecoverIntervalMinutes.ToString());
                }
                if (request.RecoverValue != null)
                {
                    jsonWriter.WritePropertyName("recoverValue");
                    jsonWriter.Write(request.RecoverValue.ToString());
                }
                if (request.InitialCapacity != null)
                {
                    jsonWriter.WritePropertyName("initialCapacity");
                    jsonWriter.Write(request.InitialCapacity.ToString());
                }
                if (request.IsOverflow != null)
                {
                    jsonWriter.WritePropertyName("isOverflow");
                    jsonWriter.Write(request.IsOverflow.ToString());
                }
                if (request.MaxCapacity != null)
                {
                    jsonWriter.WritePropertyName("maxCapacity");
                    jsonWriter.Write(request.MaxCapacity.ToString());
                }
                if (request.MaxStaminaTableName != null)
                {
                    jsonWriter.WritePropertyName("maxStaminaTableName");
                    jsonWriter.Write(request.MaxStaminaTableName);
                }
                if (request.RecoverIntervalTableName != null)
                {
                    jsonWriter.WritePropertyName("recoverIntervalTableName");
                    jsonWriter.Write(request.RecoverIntervalTableName);
                }
                if (request.RecoverValueTableName != null)
                {
                    jsonWriter.WritePropertyName("recoverValueTableName");
                    jsonWriter.Write(request.RecoverValueTableName);
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
		public IEnumerator UpdateStaminaModelMaster(
                Request.UpdateStaminaModelMasterRequest request,
                UnityAction<AsyncResult<Result.UpdateStaminaModelMasterResult>> callback
        ) =>
            new UpdateStaminaModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.UpdateStaminaModelMasterResult> UpdateStaminaModelMasterFuture(
                Request.UpdateStaminaModelMasterRequest request
        ) =>
            new UpdateStaminaModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.UpdateStaminaModelMasterResult> UpdateStaminaModelMasterAsync(
                Request.UpdateStaminaModelMasterRequest request
        ) =>
            new UpdateStaminaModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.UpdateStaminaModelMasterResult>();
    #else
		public UpdateStaminaModelMasterTask UpdateStaminaModelMasterAsync(
                Request.UpdateStaminaModelMasterRequest request
        )
		{
			return new UpdateStaminaModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.UpdateStaminaModelMasterResult> UpdateStaminaModelMasterAsync(
                Request.UpdateStaminaModelMasterRequest request
        ) =>
            new UpdateStaminaModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class DeleteStaminaModelMasterTask : Gs2RestSessionTask<DeleteStaminaModelMasterRequest, DeleteStaminaModelMasterResult>
        {
            public DeleteStaminaModelMasterTask(IGs2Session session, RestSessionRequestFactory factory, DeleteStaminaModelMasterRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DeleteStaminaModelMasterRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "stamina")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/master/model/{staminaName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{staminaName}", !string.IsNullOrEmpty(request.StaminaName) ? request.StaminaName.ToString() : "null");

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
		public IEnumerator DeleteStaminaModelMaster(
                Request.DeleteStaminaModelMasterRequest request,
                UnityAction<AsyncResult<Result.DeleteStaminaModelMasterResult>> callback
        ) =>
            new DeleteStaminaModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.DeleteStaminaModelMasterResult> DeleteStaminaModelMasterFuture(
                Request.DeleteStaminaModelMasterRequest request
        ) =>
            new DeleteStaminaModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.DeleteStaminaModelMasterResult> DeleteStaminaModelMasterAsync(
                Request.DeleteStaminaModelMasterRequest request
        ) =>
            new DeleteStaminaModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.DeleteStaminaModelMasterResult>();
    #else
		public DeleteStaminaModelMasterTask DeleteStaminaModelMasterAsync(
                Request.DeleteStaminaModelMasterRequest request
        )
		{
			return new DeleteStaminaModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.DeleteStaminaModelMasterResult> DeleteStaminaModelMasterAsync(
                Request.DeleteStaminaModelMasterRequest request
        ) =>
            new DeleteStaminaModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class DescribeMaxStaminaTableMastersTask : Gs2RestSessionTask<DescribeMaxStaminaTableMastersRequest, DescribeMaxStaminaTableMastersResult>
        {
            public DescribeMaxStaminaTableMastersTask(IGs2Session session, RestSessionRequestFactory factory, DescribeMaxStaminaTableMastersRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DescribeMaxStaminaTableMastersRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "stamina")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/master/maxStaminaTable";

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
		public IEnumerator DescribeMaxStaminaTableMasters(
                Request.DescribeMaxStaminaTableMastersRequest request,
                UnityAction<AsyncResult<Result.DescribeMaxStaminaTableMastersResult>> callback
        ) =>
            new DescribeMaxStaminaTableMastersTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.DescribeMaxStaminaTableMastersResult> DescribeMaxStaminaTableMastersFuture(
                Request.DescribeMaxStaminaTableMastersRequest request
        ) =>
            new DescribeMaxStaminaTableMastersTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.DescribeMaxStaminaTableMastersResult> DescribeMaxStaminaTableMastersAsync(
                Request.DescribeMaxStaminaTableMastersRequest request
        ) =>
            new DescribeMaxStaminaTableMastersTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.DescribeMaxStaminaTableMastersResult>();
    #else
		public DescribeMaxStaminaTableMastersTask DescribeMaxStaminaTableMastersAsync(
                Request.DescribeMaxStaminaTableMastersRequest request
        )
		{
			return new DescribeMaxStaminaTableMastersTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.DescribeMaxStaminaTableMastersResult> DescribeMaxStaminaTableMastersAsync(
                Request.DescribeMaxStaminaTableMastersRequest request
        ) =>
            new DescribeMaxStaminaTableMastersTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class CreateMaxStaminaTableMasterTask : Gs2RestSessionTask<CreateMaxStaminaTableMasterRequest, CreateMaxStaminaTableMasterResult>
        {
            public CreateMaxStaminaTableMasterTask(IGs2Session session, RestSessionRequestFactory factory, CreateMaxStaminaTableMasterRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(CreateMaxStaminaTableMasterRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "stamina")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/master/maxStaminaTable";

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
                if (request.ExperienceModelId != null)
                {
                    jsonWriter.WritePropertyName("experienceModelId");
                    jsonWriter.Write(request.ExperienceModelId);
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
                if (request.DryRun)
                {
                    sessionRequest.AddHeader("X-GS2-DRY-RUN", "true");
                }

                AddHeader(
                    Session.Credential,
                    sessionRequest
                );

                return sessionRequest;
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator CreateMaxStaminaTableMaster(
                Request.CreateMaxStaminaTableMasterRequest request,
                UnityAction<AsyncResult<Result.CreateMaxStaminaTableMasterResult>> callback
        ) =>
            new CreateMaxStaminaTableMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.CreateMaxStaminaTableMasterResult> CreateMaxStaminaTableMasterFuture(
                Request.CreateMaxStaminaTableMasterRequest request
        ) =>
            new CreateMaxStaminaTableMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.CreateMaxStaminaTableMasterResult> CreateMaxStaminaTableMasterAsync(
                Request.CreateMaxStaminaTableMasterRequest request
        ) =>
            new CreateMaxStaminaTableMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.CreateMaxStaminaTableMasterResult>();
    #else
		public CreateMaxStaminaTableMasterTask CreateMaxStaminaTableMasterAsync(
                Request.CreateMaxStaminaTableMasterRequest request
        )
		{
			return new CreateMaxStaminaTableMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.CreateMaxStaminaTableMasterResult> CreateMaxStaminaTableMasterAsync(
                Request.CreateMaxStaminaTableMasterRequest request
        ) =>
            new CreateMaxStaminaTableMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class GetMaxStaminaTableMasterTask : Gs2RestSessionTask<GetMaxStaminaTableMasterRequest, GetMaxStaminaTableMasterResult>
        {
            public GetMaxStaminaTableMasterTask(IGs2Session session, RestSessionRequestFactory factory, GetMaxStaminaTableMasterRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(GetMaxStaminaTableMasterRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "stamina")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/master/maxStaminaTable/{maxStaminaTableName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{maxStaminaTableName}", !string.IsNullOrEmpty(request.MaxStaminaTableName) ? request.MaxStaminaTableName.ToString() : "null");

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
		public IEnumerator GetMaxStaminaTableMaster(
                Request.GetMaxStaminaTableMasterRequest request,
                UnityAction<AsyncResult<Result.GetMaxStaminaTableMasterResult>> callback
        ) =>
            new GetMaxStaminaTableMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.GetMaxStaminaTableMasterResult> GetMaxStaminaTableMasterFuture(
                Request.GetMaxStaminaTableMasterRequest request
        ) =>
            new GetMaxStaminaTableMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.GetMaxStaminaTableMasterResult> GetMaxStaminaTableMasterAsync(
                Request.GetMaxStaminaTableMasterRequest request
        ) =>
            new GetMaxStaminaTableMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.GetMaxStaminaTableMasterResult>();
    #else
		public GetMaxStaminaTableMasterTask GetMaxStaminaTableMasterAsync(
                Request.GetMaxStaminaTableMasterRequest request
        )
		{
			return new GetMaxStaminaTableMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.GetMaxStaminaTableMasterResult> GetMaxStaminaTableMasterAsync(
                Request.GetMaxStaminaTableMasterRequest request
        ) =>
            new GetMaxStaminaTableMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class UpdateMaxStaminaTableMasterTask : Gs2RestSessionTask<UpdateMaxStaminaTableMasterRequest, UpdateMaxStaminaTableMasterResult>
        {
            public UpdateMaxStaminaTableMasterTask(IGs2Session session, RestSessionRequestFactory factory, UpdateMaxStaminaTableMasterRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(UpdateMaxStaminaTableMasterRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "stamina")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/master/maxStaminaTable/{maxStaminaTableName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{maxStaminaTableName}", !string.IsNullOrEmpty(request.MaxStaminaTableName) ? request.MaxStaminaTableName.ToString() : "null");

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
                if (request.ExperienceModelId != null)
                {
                    jsonWriter.WritePropertyName("experienceModelId");
                    jsonWriter.Write(request.ExperienceModelId);
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
                if (request.DryRun)
                {
                    sessionRequest.AddHeader("X-GS2-DRY-RUN", "true");
                }

                AddHeader(
                    Session.Credential,
                    sessionRequest
                );

                return sessionRequest;
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator UpdateMaxStaminaTableMaster(
                Request.UpdateMaxStaminaTableMasterRequest request,
                UnityAction<AsyncResult<Result.UpdateMaxStaminaTableMasterResult>> callback
        ) =>
            new UpdateMaxStaminaTableMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.UpdateMaxStaminaTableMasterResult> UpdateMaxStaminaTableMasterFuture(
                Request.UpdateMaxStaminaTableMasterRequest request
        ) =>
            new UpdateMaxStaminaTableMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.UpdateMaxStaminaTableMasterResult> UpdateMaxStaminaTableMasterAsync(
                Request.UpdateMaxStaminaTableMasterRequest request
        ) =>
            new UpdateMaxStaminaTableMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.UpdateMaxStaminaTableMasterResult>();
    #else
		public UpdateMaxStaminaTableMasterTask UpdateMaxStaminaTableMasterAsync(
                Request.UpdateMaxStaminaTableMasterRequest request
        )
		{
			return new UpdateMaxStaminaTableMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.UpdateMaxStaminaTableMasterResult> UpdateMaxStaminaTableMasterAsync(
                Request.UpdateMaxStaminaTableMasterRequest request
        ) =>
            new UpdateMaxStaminaTableMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class DeleteMaxStaminaTableMasterTask : Gs2RestSessionTask<DeleteMaxStaminaTableMasterRequest, DeleteMaxStaminaTableMasterResult>
        {
            public DeleteMaxStaminaTableMasterTask(IGs2Session session, RestSessionRequestFactory factory, DeleteMaxStaminaTableMasterRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DeleteMaxStaminaTableMasterRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "stamina")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/master/maxStaminaTable/{maxStaminaTableName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{maxStaminaTableName}", !string.IsNullOrEmpty(request.MaxStaminaTableName) ? request.MaxStaminaTableName.ToString() : "null");

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
		public IEnumerator DeleteMaxStaminaTableMaster(
                Request.DeleteMaxStaminaTableMasterRequest request,
                UnityAction<AsyncResult<Result.DeleteMaxStaminaTableMasterResult>> callback
        ) =>
            new DeleteMaxStaminaTableMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.DeleteMaxStaminaTableMasterResult> DeleteMaxStaminaTableMasterFuture(
                Request.DeleteMaxStaminaTableMasterRequest request
        ) =>
            new DeleteMaxStaminaTableMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.DeleteMaxStaminaTableMasterResult> DeleteMaxStaminaTableMasterAsync(
                Request.DeleteMaxStaminaTableMasterRequest request
        ) =>
            new DeleteMaxStaminaTableMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.DeleteMaxStaminaTableMasterResult>();
    #else
		public DeleteMaxStaminaTableMasterTask DeleteMaxStaminaTableMasterAsync(
                Request.DeleteMaxStaminaTableMasterRequest request
        )
		{
			return new DeleteMaxStaminaTableMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.DeleteMaxStaminaTableMasterResult> DeleteMaxStaminaTableMasterAsync(
                Request.DeleteMaxStaminaTableMasterRequest request
        ) =>
            new DeleteMaxStaminaTableMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class DescribeRecoverIntervalTableMastersTask : Gs2RestSessionTask<DescribeRecoverIntervalTableMastersRequest, DescribeRecoverIntervalTableMastersResult>
        {
            public DescribeRecoverIntervalTableMastersTask(IGs2Session session, RestSessionRequestFactory factory, DescribeRecoverIntervalTableMastersRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DescribeRecoverIntervalTableMastersRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "stamina")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/master/recoverIntervalTable";

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
		public IEnumerator DescribeRecoverIntervalTableMasters(
                Request.DescribeRecoverIntervalTableMastersRequest request,
                UnityAction<AsyncResult<Result.DescribeRecoverIntervalTableMastersResult>> callback
        ) =>
            new DescribeRecoverIntervalTableMastersTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.DescribeRecoverIntervalTableMastersResult> DescribeRecoverIntervalTableMastersFuture(
                Request.DescribeRecoverIntervalTableMastersRequest request
        ) =>
            new DescribeRecoverIntervalTableMastersTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.DescribeRecoverIntervalTableMastersResult> DescribeRecoverIntervalTableMastersAsync(
                Request.DescribeRecoverIntervalTableMastersRequest request
        ) =>
            new DescribeRecoverIntervalTableMastersTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.DescribeRecoverIntervalTableMastersResult>();
    #else
		public DescribeRecoverIntervalTableMastersTask DescribeRecoverIntervalTableMastersAsync(
                Request.DescribeRecoverIntervalTableMastersRequest request
        )
		{
			return new DescribeRecoverIntervalTableMastersTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.DescribeRecoverIntervalTableMastersResult> DescribeRecoverIntervalTableMastersAsync(
                Request.DescribeRecoverIntervalTableMastersRequest request
        ) =>
            new DescribeRecoverIntervalTableMastersTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class CreateRecoverIntervalTableMasterTask : Gs2RestSessionTask<CreateRecoverIntervalTableMasterRequest, CreateRecoverIntervalTableMasterResult>
        {
            public CreateRecoverIntervalTableMasterTask(IGs2Session session, RestSessionRequestFactory factory, CreateRecoverIntervalTableMasterRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(CreateRecoverIntervalTableMasterRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "stamina")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/master/recoverIntervalTable";

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
                if (request.ExperienceModelId != null)
                {
                    jsonWriter.WritePropertyName("experienceModelId");
                    jsonWriter.Write(request.ExperienceModelId);
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
                if (request.DryRun)
                {
                    sessionRequest.AddHeader("X-GS2-DRY-RUN", "true");
                }

                AddHeader(
                    Session.Credential,
                    sessionRequest
                );

                return sessionRequest;
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator CreateRecoverIntervalTableMaster(
                Request.CreateRecoverIntervalTableMasterRequest request,
                UnityAction<AsyncResult<Result.CreateRecoverIntervalTableMasterResult>> callback
        ) =>
            new CreateRecoverIntervalTableMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.CreateRecoverIntervalTableMasterResult> CreateRecoverIntervalTableMasterFuture(
                Request.CreateRecoverIntervalTableMasterRequest request
        ) =>
            new CreateRecoverIntervalTableMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.CreateRecoverIntervalTableMasterResult> CreateRecoverIntervalTableMasterAsync(
                Request.CreateRecoverIntervalTableMasterRequest request
        ) =>
            new CreateRecoverIntervalTableMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.CreateRecoverIntervalTableMasterResult>();
    #else
		public CreateRecoverIntervalTableMasterTask CreateRecoverIntervalTableMasterAsync(
                Request.CreateRecoverIntervalTableMasterRequest request
        )
		{
			return new CreateRecoverIntervalTableMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.CreateRecoverIntervalTableMasterResult> CreateRecoverIntervalTableMasterAsync(
                Request.CreateRecoverIntervalTableMasterRequest request
        ) =>
            new CreateRecoverIntervalTableMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class GetRecoverIntervalTableMasterTask : Gs2RestSessionTask<GetRecoverIntervalTableMasterRequest, GetRecoverIntervalTableMasterResult>
        {
            public GetRecoverIntervalTableMasterTask(IGs2Session session, RestSessionRequestFactory factory, GetRecoverIntervalTableMasterRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(GetRecoverIntervalTableMasterRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "stamina")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/master/recoverIntervalTable/{recoverIntervalTableName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{recoverIntervalTableName}", !string.IsNullOrEmpty(request.RecoverIntervalTableName) ? request.RecoverIntervalTableName.ToString() : "null");

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
		public IEnumerator GetRecoverIntervalTableMaster(
                Request.GetRecoverIntervalTableMasterRequest request,
                UnityAction<AsyncResult<Result.GetRecoverIntervalTableMasterResult>> callback
        ) =>
            new GetRecoverIntervalTableMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.GetRecoverIntervalTableMasterResult> GetRecoverIntervalTableMasterFuture(
                Request.GetRecoverIntervalTableMasterRequest request
        ) =>
            new GetRecoverIntervalTableMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.GetRecoverIntervalTableMasterResult> GetRecoverIntervalTableMasterAsync(
                Request.GetRecoverIntervalTableMasterRequest request
        ) =>
            new GetRecoverIntervalTableMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.GetRecoverIntervalTableMasterResult>();
    #else
		public GetRecoverIntervalTableMasterTask GetRecoverIntervalTableMasterAsync(
                Request.GetRecoverIntervalTableMasterRequest request
        )
		{
			return new GetRecoverIntervalTableMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.GetRecoverIntervalTableMasterResult> GetRecoverIntervalTableMasterAsync(
                Request.GetRecoverIntervalTableMasterRequest request
        ) =>
            new GetRecoverIntervalTableMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class UpdateRecoverIntervalTableMasterTask : Gs2RestSessionTask<UpdateRecoverIntervalTableMasterRequest, UpdateRecoverIntervalTableMasterResult>
        {
            public UpdateRecoverIntervalTableMasterTask(IGs2Session session, RestSessionRequestFactory factory, UpdateRecoverIntervalTableMasterRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(UpdateRecoverIntervalTableMasterRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "stamina")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/master/recoverIntervalTable/{recoverIntervalTableName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{recoverIntervalTableName}", !string.IsNullOrEmpty(request.RecoverIntervalTableName) ? request.RecoverIntervalTableName.ToString() : "null");

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
                if (request.ExperienceModelId != null)
                {
                    jsonWriter.WritePropertyName("experienceModelId");
                    jsonWriter.Write(request.ExperienceModelId);
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
                if (request.DryRun)
                {
                    sessionRequest.AddHeader("X-GS2-DRY-RUN", "true");
                }

                AddHeader(
                    Session.Credential,
                    sessionRequest
                );

                return sessionRequest;
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator UpdateRecoverIntervalTableMaster(
                Request.UpdateRecoverIntervalTableMasterRequest request,
                UnityAction<AsyncResult<Result.UpdateRecoverIntervalTableMasterResult>> callback
        ) =>
            new UpdateRecoverIntervalTableMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.UpdateRecoverIntervalTableMasterResult> UpdateRecoverIntervalTableMasterFuture(
                Request.UpdateRecoverIntervalTableMasterRequest request
        ) =>
            new UpdateRecoverIntervalTableMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.UpdateRecoverIntervalTableMasterResult> UpdateRecoverIntervalTableMasterAsync(
                Request.UpdateRecoverIntervalTableMasterRequest request
        ) =>
            new UpdateRecoverIntervalTableMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.UpdateRecoverIntervalTableMasterResult>();
    #else
		public UpdateRecoverIntervalTableMasterTask UpdateRecoverIntervalTableMasterAsync(
                Request.UpdateRecoverIntervalTableMasterRequest request
        )
		{
			return new UpdateRecoverIntervalTableMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.UpdateRecoverIntervalTableMasterResult> UpdateRecoverIntervalTableMasterAsync(
                Request.UpdateRecoverIntervalTableMasterRequest request
        ) =>
            new UpdateRecoverIntervalTableMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class DeleteRecoverIntervalTableMasterTask : Gs2RestSessionTask<DeleteRecoverIntervalTableMasterRequest, DeleteRecoverIntervalTableMasterResult>
        {
            public DeleteRecoverIntervalTableMasterTask(IGs2Session session, RestSessionRequestFactory factory, DeleteRecoverIntervalTableMasterRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DeleteRecoverIntervalTableMasterRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "stamina")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/master/recoverIntervalTable/{recoverIntervalTableName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{recoverIntervalTableName}", !string.IsNullOrEmpty(request.RecoverIntervalTableName) ? request.RecoverIntervalTableName.ToString() : "null");

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
		public IEnumerator DeleteRecoverIntervalTableMaster(
                Request.DeleteRecoverIntervalTableMasterRequest request,
                UnityAction<AsyncResult<Result.DeleteRecoverIntervalTableMasterResult>> callback
        ) =>
            new DeleteRecoverIntervalTableMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.DeleteRecoverIntervalTableMasterResult> DeleteRecoverIntervalTableMasterFuture(
                Request.DeleteRecoverIntervalTableMasterRequest request
        ) =>
            new DeleteRecoverIntervalTableMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.DeleteRecoverIntervalTableMasterResult> DeleteRecoverIntervalTableMasterAsync(
                Request.DeleteRecoverIntervalTableMasterRequest request
        ) =>
            new DeleteRecoverIntervalTableMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.DeleteRecoverIntervalTableMasterResult>();
    #else
		public DeleteRecoverIntervalTableMasterTask DeleteRecoverIntervalTableMasterAsync(
                Request.DeleteRecoverIntervalTableMasterRequest request
        )
		{
			return new DeleteRecoverIntervalTableMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.DeleteRecoverIntervalTableMasterResult> DeleteRecoverIntervalTableMasterAsync(
                Request.DeleteRecoverIntervalTableMasterRequest request
        ) =>
            new DeleteRecoverIntervalTableMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class DescribeRecoverValueTableMastersTask : Gs2RestSessionTask<DescribeRecoverValueTableMastersRequest, DescribeRecoverValueTableMastersResult>
        {
            public DescribeRecoverValueTableMastersTask(IGs2Session session, RestSessionRequestFactory factory, DescribeRecoverValueTableMastersRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DescribeRecoverValueTableMastersRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "stamina")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/master/recoverValueTable";

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
		public IEnumerator DescribeRecoverValueTableMasters(
                Request.DescribeRecoverValueTableMastersRequest request,
                UnityAction<AsyncResult<Result.DescribeRecoverValueTableMastersResult>> callback
        ) =>
            new DescribeRecoverValueTableMastersTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.DescribeRecoverValueTableMastersResult> DescribeRecoverValueTableMastersFuture(
                Request.DescribeRecoverValueTableMastersRequest request
        ) =>
            new DescribeRecoverValueTableMastersTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.DescribeRecoverValueTableMastersResult> DescribeRecoverValueTableMastersAsync(
                Request.DescribeRecoverValueTableMastersRequest request
        ) =>
            new DescribeRecoverValueTableMastersTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.DescribeRecoverValueTableMastersResult>();
    #else
		public DescribeRecoverValueTableMastersTask DescribeRecoverValueTableMastersAsync(
                Request.DescribeRecoverValueTableMastersRequest request
        )
		{
			return new DescribeRecoverValueTableMastersTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.DescribeRecoverValueTableMastersResult> DescribeRecoverValueTableMastersAsync(
                Request.DescribeRecoverValueTableMastersRequest request
        ) =>
            new DescribeRecoverValueTableMastersTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class CreateRecoverValueTableMasterTask : Gs2RestSessionTask<CreateRecoverValueTableMasterRequest, CreateRecoverValueTableMasterResult>
        {
            public CreateRecoverValueTableMasterTask(IGs2Session session, RestSessionRequestFactory factory, CreateRecoverValueTableMasterRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(CreateRecoverValueTableMasterRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "stamina")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/master/recoverValueTable";

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
                if (request.ExperienceModelId != null)
                {
                    jsonWriter.WritePropertyName("experienceModelId");
                    jsonWriter.Write(request.ExperienceModelId);
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
                if (request.DryRun)
                {
                    sessionRequest.AddHeader("X-GS2-DRY-RUN", "true");
                }

                AddHeader(
                    Session.Credential,
                    sessionRequest
                );

                return sessionRequest;
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator CreateRecoverValueTableMaster(
                Request.CreateRecoverValueTableMasterRequest request,
                UnityAction<AsyncResult<Result.CreateRecoverValueTableMasterResult>> callback
        ) =>
            new CreateRecoverValueTableMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.CreateRecoverValueTableMasterResult> CreateRecoverValueTableMasterFuture(
                Request.CreateRecoverValueTableMasterRequest request
        ) =>
            new CreateRecoverValueTableMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.CreateRecoverValueTableMasterResult> CreateRecoverValueTableMasterAsync(
                Request.CreateRecoverValueTableMasterRequest request
        ) =>
            new CreateRecoverValueTableMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.CreateRecoverValueTableMasterResult>();
    #else
		public CreateRecoverValueTableMasterTask CreateRecoverValueTableMasterAsync(
                Request.CreateRecoverValueTableMasterRequest request
        )
		{
			return new CreateRecoverValueTableMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.CreateRecoverValueTableMasterResult> CreateRecoverValueTableMasterAsync(
                Request.CreateRecoverValueTableMasterRequest request
        ) =>
            new CreateRecoverValueTableMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class GetRecoverValueTableMasterTask : Gs2RestSessionTask<GetRecoverValueTableMasterRequest, GetRecoverValueTableMasterResult>
        {
            public GetRecoverValueTableMasterTask(IGs2Session session, RestSessionRequestFactory factory, GetRecoverValueTableMasterRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(GetRecoverValueTableMasterRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "stamina")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/master/recoverValueTable/{recoverValueTableName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{recoverValueTableName}", !string.IsNullOrEmpty(request.RecoverValueTableName) ? request.RecoverValueTableName.ToString() : "null");

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
		public IEnumerator GetRecoverValueTableMaster(
                Request.GetRecoverValueTableMasterRequest request,
                UnityAction<AsyncResult<Result.GetRecoverValueTableMasterResult>> callback
        ) =>
            new GetRecoverValueTableMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.GetRecoverValueTableMasterResult> GetRecoverValueTableMasterFuture(
                Request.GetRecoverValueTableMasterRequest request
        ) =>
            new GetRecoverValueTableMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.GetRecoverValueTableMasterResult> GetRecoverValueTableMasterAsync(
                Request.GetRecoverValueTableMasterRequest request
        ) =>
            new GetRecoverValueTableMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.GetRecoverValueTableMasterResult>();
    #else
		public GetRecoverValueTableMasterTask GetRecoverValueTableMasterAsync(
                Request.GetRecoverValueTableMasterRequest request
        )
		{
			return new GetRecoverValueTableMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.GetRecoverValueTableMasterResult> GetRecoverValueTableMasterAsync(
                Request.GetRecoverValueTableMasterRequest request
        ) =>
            new GetRecoverValueTableMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class UpdateRecoverValueTableMasterTask : Gs2RestSessionTask<UpdateRecoverValueTableMasterRequest, UpdateRecoverValueTableMasterResult>
        {
            public UpdateRecoverValueTableMasterTask(IGs2Session session, RestSessionRequestFactory factory, UpdateRecoverValueTableMasterRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(UpdateRecoverValueTableMasterRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "stamina")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/master/recoverValueTable/{recoverValueTableName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{recoverValueTableName}", !string.IsNullOrEmpty(request.RecoverValueTableName) ? request.RecoverValueTableName.ToString() : "null");

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
                if (request.ExperienceModelId != null)
                {
                    jsonWriter.WritePropertyName("experienceModelId");
                    jsonWriter.Write(request.ExperienceModelId);
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
                if (request.DryRun)
                {
                    sessionRequest.AddHeader("X-GS2-DRY-RUN", "true");
                }

                AddHeader(
                    Session.Credential,
                    sessionRequest
                );

                return sessionRequest;
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator UpdateRecoverValueTableMaster(
                Request.UpdateRecoverValueTableMasterRequest request,
                UnityAction<AsyncResult<Result.UpdateRecoverValueTableMasterResult>> callback
        ) =>
            new UpdateRecoverValueTableMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.UpdateRecoverValueTableMasterResult> UpdateRecoverValueTableMasterFuture(
                Request.UpdateRecoverValueTableMasterRequest request
        ) =>
            new UpdateRecoverValueTableMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.UpdateRecoverValueTableMasterResult> UpdateRecoverValueTableMasterAsync(
                Request.UpdateRecoverValueTableMasterRequest request
        ) =>
            new UpdateRecoverValueTableMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.UpdateRecoverValueTableMasterResult>();
    #else
		public UpdateRecoverValueTableMasterTask UpdateRecoverValueTableMasterAsync(
                Request.UpdateRecoverValueTableMasterRequest request
        )
		{
			return new UpdateRecoverValueTableMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.UpdateRecoverValueTableMasterResult> UpdateRecoverValueTableMasterAsync(
                Request.UpdateRecoverValueTableMasterRequest request
        ) =>
            new UpdateRecoverValueTableMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class DeleteRecoverValueTableMasterTask : Gs2RestSessionTask<DeleteRecoverValueTableMasterRequest, DeleteRecoverValueTableMasterResult>
        {
            public DeleteRecoverValueTableMasterTask(IGs2Session session, RestSessionRequestFactory factory, DeleteRecoverValueTableMasterRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DeleteRecoverValueTableMasterRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "stamina")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/master/recoverValueTable/{recoverValueTableName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{recoverValueTableName}", !string.IsNullOrEmpty(request.RecoverValueTableName) ? request.RecoverValueTableName.ToString() : "null");

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
		public IEnumerator DeleteRecoverValueTableMaster(
                Request.DeleteRecoverValueTableMasterRequest request,
                UnityAction<AsyncResult<Result.DeleteRecoverValueTableMasterResult>> callback
        ) =>
            new DeleteRecoverValueTableMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.DeleteRecoverValueTableMasterResult> DeleteRecoverValueTableMasterFuture(
                Request.DeleteRecoverValueTableMasterRequest request
        ) =>
            new DeleteRecoverValueTableMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.DeleteRecoverValueTableMasterResult> DeleteRecoverValueTableMasterAsync(
                Request.DeleteRecoverValueTableMasterRequest request
        ) =>
            new DeleteRecoverValueTableMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.DeleteRecoverValueTableMasterResult>();
    #else
		public DeleteRecoverValueTableMasterTask DeleteRecoverValueTableMasterAsync(
                Request.DeleteRecoverValueTableMasterRequest request
        )
		{
			return new DeleteRecoverValueTableMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.DeleteRecoverValueTableMasterResult> DeleteRecoverValueTableMasterAsync(
                Request.DeleteRecoverValueTableMasterRequest request
        ) =>
            new DeleteRecoverValueTableMasterTask(
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
                    .Replace("{service}", "stamina")
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


        public class GetCurrentStaminaMasterTask : Gs2RestSessionTask<GetCurrentStaminaMasterRequest, GetCurrentStaminaMasterResult>
        {
            public GetCurrentStaminaMasterTask(IGs2Session session, RestSessionRequestFactory factory, GetCurrentStaminaMasterRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(GetCurrentStaminaMasterRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "stamina")
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
		public IEnumerator GetCurrentStaminaMaster(
                Request.GetCurrentStaminaMasterRequest request,
                UnityAction<AsyncResult<Result.GetCurrentStaminaMasterResult>> callback
        ) =>
            new GetCurrentStaminaMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.GetCurrentStaminaMasterResult> GetCurrentStaminaMasterFuture(
                Request.GetCurrentStaminaMasterRequest request
        ) =>
            new GetCurrentStaminaMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.GetCurrentStaminaMasterResult> GetCurrentStaminaMasterAsync(
                Request.GetCurrentStaminaMasterRequest request
        ) =>
            new GetCurrentStaminaMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.GetCurrentStaminaMasterResult>();
    #else
		public GetCurrentStaminaMasterTask GetCurrentStaminaMasterAsync(
                Request.GetCurrentStaminaMasterRequest request
        )
		{
			return new GetCurrentStaminaMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.GetCurrentStaminaMasterResult> GetCurrentStaminaMasterAsync(
                Request.GetCurrentStaminaMasterRequest request
        ) =>
            new GetCurrentStaminaMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class PreUpdateCurrentStaminaMasterTask : Gs2RestSessionTask<PreUpdateCurrentStaminaMasterRequest, PreUpdateCurrentStaminaMasterResult>
        {
            public PreUpdateCurrentStaminaMasterTask(IGs2Session session, RestSessionRequestFactory factory, PreUpdateCurrentStaminaMasterRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(PreUpdateCurrentStaminaMasterRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "stamina")
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
		public IEnumerator PreUpdateCurrentStaminaMaster(
                Request.PreUpdateCurrentStaminaMasterRequest request,
                UnityAction<AsyncResult<Result.PreUpdateCurrentStaminaMasterResult>> callback
        ) =>
            new PreUpdateCurrentStaminaMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.PreUpdateCurrentStaminaMasterResult> PreUpdateCurrentStaminaMasterFuture(
                Request.PreUpdateCurrentStaminaMasterRequest request
        ) =>
            new PreUpdateCurrentStaminaMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.PreUpdateCurrentStaminaMasterResult> PreUpdateCurrentStaminaMasterAsync(
                Request.PreUpdateCurrentStaminaMasterRequest request
        ) =>
            new PreUpdateCurrentStaminaMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.PreUpdateCurrentStaminaMasterResult>();
    #else
		public PreUpdateCurrentStaminaMasterTask PreUpdateCurrentStaminaMasterAsync(
                Request.PreUpdateCurrentStaminaMasterRequest request
        )
		{
			return new PreUpdateCurrentStaminaMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.PreUpdateCurrentStaminaMasterResult> PreUpdateCurrentStaminaMasterAsync(
                Request.PreUpdateCurrentStaminaMasterRequest request
        ) =>
            new PreUpdateCurrentStaminaMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class UpdateCurrentStaminaMasterTask : Gs2RestSessionTask<UpdateCurrentStaminaMasterRequest, UpdateCurrentStaminaMasterResult>
        {
            public UpdateCurrentStaminaMasterTask(IGs2Session session, RestSessionRequestFactory factory, UpdateCurrentStaminaMasterRequest request) : base(session, factory, request)
            {
            }
#if GS2_ENABLE_UNITASK
            protected override async UniTask<UpdateCurrentStaminaMasterResult> InvokeImpl()
#else
            protected override async Task<UpdateCurrentStaminaMasterResult> InvokeImpl()
#endif
            {
                if (Request.Settings != null) {
                    var preTask = new PreUpdateCurrentStaminaMasterTask(
                        Session,
                        Factory,
                        new PreUpdateCurrentStaminaMasterRequest()
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

            protected override IGs2SessionRequest CreateRequest(UpdateCurrentStaminaMasterRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "stamina")
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
		public IEnumerator UpdateCurrentStaminaMaster(
                Request.UpdateCurrentStaminaMasterRequest request,
                UnityAction<AsyncResult<Result.UpdateCurrentStaminaMasterResult>> callback
        ) =>
            new UpdateCurrentStaminaMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.UpdateCurrentStaminaMasterResult> UpdateCurrentStaminaMasterFuture(
                Request.UpdateCurrentStaminaMasterRequest request
        ) =>
            new UpdateCurrentStaminaMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.UpdateCurrentStaminaMasterResult> UpdateCurrentStaminaMasterAsync(
                Request.UpdateCurrentStaminaMasterRequest request
        ) =>
            new UpdateCurrentStaminaMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.UpdateCurrentStaminaMasterResult>();
    #else
		public UpdateCurrentStaminaMasterTask UpdateCurrentStaminaMasterAsync(
                Request.UpdateCurrentStaminaMasterRequest request
        )
		{
			return new UpdateCurrentStaminaMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.UpdateCurrentStaminaMasterResult> UpdateCurrentStaminaMasterAsync(
                Request.UpdateCurrentStaminaMasterRequest request
        ) =>
            new UpdateCurrentStaminaMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class UpdateCurrentStaminaMasterFromGitHubTask : Gs2RestSessionTask<UpdateCurrentStaminaMasterFromGitHubRequest, UpdateCurrentStaminaMasterFromGitHubResult>
        {
            public UpdateCurrentStaminaMasterFromGitHubTask(IGs2Session session, RestSessionRequestFactory factory, UpdateCurrentStaminaMasterFromGitHubRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(UpdateCurrentStaminaMasterFromGitHubRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "stamina")
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
		public IEnumerator UpdateCurrentStaminaMasterFromGitHub(
                Request.UpdateCurrentStaminaMasterFromGitHubRequest request,
                UnityAction<AsyncResult<Result.UpdateCurrentStaminaMasterFromGitHubResult>> callback
        ) =>
            new UpdateCurrentStaminaMasterFromGitHubTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.UpdateCurrentStaminaMasterFromGitHubResult> UpdateCurrentStaminaMasterFromGitHubFuture(
                Request.UpdateCurrentStaminaMasterFromGitHubRequest request
        ) =>
            new UpdateCurrentStaminaMasterFromGitHubTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.UpdateCurrentStaminaMasterFromGitHubResult> UpdateCurrentStaminaMasterFromGitHubAsync(
                Request.UpdateCurrentStaminaMasterFromGitHubRequest request
        ) =>
            new UpdateCurrentStaminaMasterFromGitHubTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.UpdateCurrentStaminaMasterFromGitHubResult>();
    #else
		public UpdateCurrentStaminaMasterFromGitHubTask UpdateCurrentStaminaMasterFromGitHubAsync(
                Request.UpdateCurrentStaminaMasterFromGitHubRequest request
        )
		{
			return new UpdateCurrentStaminaMasterFromGitHubTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.UpdateCurrentStaminaMasterFromGitHubResult> UpdateCurrentStaminaMasterFromGitHubAsync(
                Request.UpdateCurrentStaminaMasterFromGitHubRequest request
        ) =>
            new UpdateCurrentStaminaMasterFromGitHubTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class DescribeStaminaModelsTask : Gs2RestSessionTask<DescribeStaminaModelsRequest, DescribeStaminaModelsResult>
        {
            public DescribeStaminaModelsTask(IGs2Session session, RestSessionRequestFactory factory, DescribeStaminaModelsRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DescribeStaminaModelsRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "stamina")
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
		public IEnumerator DescribeStaminaModels(
                Request.DescribeStaminaModelsRequest request,
                UnityAction<AsyncResult<Result.DescribeStaminaModelsResult>> callback
        ) =>
            new DescribeStaminaModelsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.DescribeStaminaModelsResult> DescribeStaminaModelsFuture(
                Request.DescribeStaminaModelsRequest request
        ) =>
            new DescribeStaminaModelsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.DescribeStaminaModelsResult> DescribeStaminaModelsAsync(
                Request.DescribeStaminaModelsRequest request
        ) =>
            new DescribeStaminaModelsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.DescribeStaminaModelsResult>();
    #else
		public DescribeStaminaModelsTask DescribeStaminaModelsAsync(
                Request.DescribeStaminaModelsRequest request
        )
		{
			return new DescribeStaminaModelsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.DescribeStaminaModelsResult> DescribeStaminaModelsAsync(
                Request.DescribeStaminaModelsRequest request
        ) =>
            new DescribeStaminaModelsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class GetStaminaModelTask : Gs2RestSessionTask<GetStaminaModelRequest, GetStaminaModelResult>
        {
            public GetStaminaModelTask(IGs2Session session, RestSessionRequestFactory factory, GetStaminaModelRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(GetStaminaModelRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "stamina")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/model/{staminaName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{staminaName}", !string.IsNullOrEmpty(request.StaminaName) ? request.StaminaName.ToString() : "null");

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
		public IEnumerator GetStaminaModel(
                Request.GetStaminaModelRequest request,
                UnityAction<AsyncResult<Result.GetStaminaModelResult>> callback
        ) =>
            new GetStaminaModelTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.GetStaminaModelResult> GetStaminaModelFuture(
                Request.GetStaminaModelRequest request
        ) =>
            new GetStaminaModelTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.GetStaminaModelResult> GetStaminaModelAsync(
                Request.GetStaminaModelRequest request
        ) =>
            new GetStaminaModelTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.GetStaminaModelResult>();
    #else
		public GetStaminaModelTask GetStaminaModelAsync(
                Request.GetStaminaModelRequest request
        )
		{
			return new GetStaminaModelTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.GetStaminaModelResult> GetStaminaModelAsync(
                Request.GetStaminaModelRequest request
        ) =>
            new GetStaminaModelTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class DescribeStaminasTask : Gs2RestSessionTask<DescribeStaminasRequest, DescribeStaminasResult>
        {
            public DescribeStaminasTask(IGs2Session session, RestSessionRequestFactory factory, DescribeStaminasRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DescribeStaminasRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "stamina")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/me/stamina";

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
		public IEnumerator DescribeStaminas(
                Request.DescribeStaminasRequest request,
                UnityAction<AsyncResult<Result.DescribeStaminasResult>> callback
        ) =>
            new DescribeStaminasTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.DescribeStaminasResult> DescribeStaminasFuture(
                Request.DescribeStaminasRequest request
        ) =>
            new DescribeStaminasTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.DescribeStaminasResult> DescribeStaminasAsync(
                Request.DescribeStaminasRequest request
        ) =>
            new DescribeStaminasTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.DescribeStaminasResult>();
    #else
		public DescribeStaminasTask DescribeStaminasAsync(
                Request.DescribeStaminasRequest request
        )
		{
			return new DescribeStaminasTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.DescribeStaminasResult> DescribeStaminasAsync(
                Request.DescribeStaminasRequest request
        ) =>
            new DescribeStaminasTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class DescribeStaminasByUserIdTask : Gs2RestSessionTask<DescribeStaminasByUserIdRequest, DescribeStaminasByUserIdResult>
        {
            public DescribeStaminasByUserIdTask(IGs2Session session, RestSessionRequestFactory factory, DescribeStaminasByUserIdRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DescribeStaminasByUserIdRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "stamina")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/{userId}/stamina";

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
		public IEnumerator DescribeStaminasByUserId(
                Request.DescribeStaminasByUserIdRequest request,
                UnityAction<AsyncResult<Result.DescribeStaminasByUserIdResult>> callback
        ) =>
            new DescribeStaminasByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.DescribeStaminasByUserIdResult> DescribeStaminasByUserIdFuture(
                Request.DescribeStaminasByUserIdRequest request
        ) =>
            new DescribeStaminasByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.DescribeStaminasByUserIdResult> DescribeStaminasByUserIdAsync(
                Request.DescribeStaminasByUserIdRequest request
        ) =>
            new DescribeStaminasByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.DescribeStaminasByUserIdResult>();
    #else
		public DescribeStaminasByUserIdTask DescribeStaminasByUserIdAsync(
                Request.DescribeStaminasByUserIdRequest request
        )
		{
			return new DescribeStaminasByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.DescribeStaminasByUserIdResult> DescribeStaminasByUserIdAsync(
                Request.DescribeStaminasByUserIdRequest request
        ) =>
            new DescribeStaminasByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class GetStaminaTask : Gs2RestSessionTask<GetStaminaRequest, GetStaminaResult>
        {
            public GetStaminaTask(IGs2Session session, RestSessionRequestFactory factory, GetStaminaRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(GetStaminaRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "stamina")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/me/stamina/{staminaName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{staminaName}", !string.IsNullOrEmpty(request.StaminaName) ? request.StaminaName.ToString() : "null");

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
		public IEnumerator GetStamina(
                Request.GetStaminaRequest request,
                UnityAction<AsyncResult<Result.GetStaminaResult>> callback
        ) =>
            new GetStaminaTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.GetStaminaResult> GetStaminaFuture(
                Request.GetStaminaRequest request
        ) =>
            new GetStaminaTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.GetStaminaResult> GetStaminaAsync(
                Request.GetStaminaRequest request
        ) =>
            new GetStaminaTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.GetStaminaResult>();
    #else
		public GetStaminaTask GetStaminaAsync(
                Request.GetStaminaRequest request
        )
		{
			return new GetStaminaTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.GetStaminaResult> GetStaminaAsync(
                Request.GetStaminaRequest request
        ) =>
            new GetStaminaTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class GetStaminaByUserIdTask : Gs2RestSessionTask<GetStaminaByUserIdRequest, GetStaminaByUserIdResult>
        {
            public GetStaminaByUserIdTask(IGs2Session session, RestSessionRequestFactory factory, GetStaminaByUserIdRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(GetStaminaByUserIdRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "stamina")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/{userId}/stamina/{staminaName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{staminaName}", !string.IsNullOrEmpty(request.StaminaName) ? request.StaminaName.ToString() : "null");
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
		public IEnumerator GetStaminaByUserId(
                Request.GetStaminaByUserIdRequest request,
                UnityAction<AsyncResult<Result.GetStaminaByUserIdResult>> callback
        ) =>
            new GetStaminaByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.GetStaminaByUserIdResult> GetStaminaByUserIdFuture(
                Request.GetStaminaByUserIdRequest request
        ) =>
            new GetStaminaByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.GetStaminaByUserIdResult> GetStaminaByUserIdAsync(
                Request.GetStaminaByUserIdRequest request
        ) =>
            new GetStaminaByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.GetStaminaByUserIdResult>();
    #else
		public GetStaminaByUserIdTask GetStaminaByUserIdAsync(
                Request.GetStaminaByUserIdRequest request
        )
		{
			return new GetStaminaByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.GetStaminaByUserIdResult> GetStaminaByUserIdAsync(
                Request.GetStaminaByUserIdRequest request
        ) =>
            new GetStaminaByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class UpdateStaminaByUserIdTask : Gs2RestSessionTask<UpdateStaminaByUserIdRequest, UpdateStaminaByUserIdResult>
        {
            public UpdateStaminaByUserIdTask(IGs2Session session, RestSessionRequestFactory factory, UpdateStaminaByUserIdRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(UpdateStaminaByUserIdRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "stamina")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/{userId}/stamina/{staminaName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{staminaName}", !string.IsNullOrEmpty(request.StaminaName) ? request.StaminaName.ToString() : "null");
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
                if (request.MaxValue != null)
                {
                    jsonWriter.WritePropertyName("maxValue");
                    jsonWriter.Write(request.MaxValue.ToString());
                }
                if (request.RecoverIntervalMinutes != null)
                {
                    jsonWriter.WritePropertyName("recoverIntervalMinutes");
                    jsonWriter.Write(request.RecoverIntervalMinutes.ToString());
                }
                if (request.RecoverValue != null)
                {
                    jsonWriter.WritePropertyName("recoverValue");
                    jsonWriter.Write(request.RecoverValue.ToString());
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
		public IEnumerator UpdateStaminaByUserId(
                Request.UpdateStaminaByUserIdRequest request,
                UnityAction<AsyncResult<Result.UpdateStaminaByUserIdResult>> callback
        ) =>
            new UpdateStaminaByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.UpdateStaminaByUserIdResult> UpdateStaminaByUserIdFuture(
                Request.UpdateStaminaByUserIdRequest request
        ) =>
            new UpdateStaminaByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.UpdateStaminaByUserIdResult> UpdateStaminaByUserIdAsync(
                Request.UpdateStaminaByUserIdRequest request
        ) =>
            new UpdateStaminaByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.UpdateStaminaByUserIdResult>();
    #else
		public UpdateStaminaByUserIdTask UpdateStaminaByUserIdAsync(
                Request.UpdateStaminaByUserIdRequest request
        )
		{
			return new UpdateStaminaByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.UpdateStaminaByUserIdResult> UpdateStaminaByUserIdAsync(
                Request.UpdateStaminaByUserIdRequest request
        ) =>
            new UpdateStaminaByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class ConsumeStaminaTask : Gs2RestSessionTask<ConsumeStaminaRequest, ConsumeStaminaResult>
        {
            public ConsumeStaminaTask(IGs2Session session, RestSessionRequestFactory factory, ConsumeStaminaRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(ConsumeStaminaRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "stamina")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/me/stamina/{staminaName}/consume";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{staminaName}", !string.IsNullOrEmpty(request.StaminaName) ? request.StaminaName.ToString() : "null");

                var sessionRequest = Factory.Post(url);

                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);
                jsonWriter.WriteObjectStart();
                if (request.ConsumeValue != null)
                {
                    jsonWriter.WritePropertyName("consumeValue");
                    jsonWriter.Write(request.ConsumeValue.ToString());
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
                if (error.Errors.Count(v => v.code == "stamina.stamina.insufficient") > 0) {
                    base.OnError(new Exception.InsufficientException(error));
                }
                else {
                    base.OnError(error);
                }
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator ConsumeStamina(
                Request.ConsumeStaminaRequest request,
                UnityAction<AsyncResult<Result.ConsumeStaminaResult>> callback
        ) =>
            new ConsumeStaminaTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.ConsumeStaminaResult> ConsumeStaminaFuture(
                Request.ConsumeStaminaRequest request
        ) =>
            new ConsumeStaminaTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.ConsumeStaminaResult> ConsumeStaminaAsync(
                Request.ConsumeStaminaRequest request
        ) =>
            new ConsumeStaminaTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.ConsumeStaminaResult>();
    #else
		public ConsumeStaminaTask ConsumeStaminaAsync(
                Request.ConsumeStaminaRequest request
        )
		{
			return new ConsumeStaminaTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.ConsumeStaminaResult> ConsumeStaminaAsync(
                Request.ConsumeStaminaRequest request
        ) =>
            new ConsumeStaminaTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class ConsumeStaminaByUserIdTask : Gs2RestSessionTask<ConsumeStaminaByUserIdRequest, ConsumeStaminaByUserIdResult>
        {
            public ConsumeStaminaByUserIdTask(IGs2Session session, RestSessionRequestFactory factory, ConsumeStaminaByUserIdRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(ConsumeStaminaByUserIdRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "stamina")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/{userId}/stamina/{staminaName}/consume";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{staminaName}", !string.IsNullOrEmpty(request.StaminaName) ? request.StaminaName.ToString() : "null");
                url = url.Replace("{userId}", !string.IsNullOrEmpty(request.UserId) ? request.UserId.ToString() : "null");

                var sessionRequest = Factory.Post(url);

                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);
                jsonWriter.WriteObjectStart();
                if (request.ConsumeValue != null)
                {
                    jsonWriter.WritePropertyName("consumeValue");
                    jsonWriter.Write(request.ConsumeValue.ToString());
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
                if (error.Errors.Count(v => v.code == "stamina.stamina.insufficient") > 0) {
                    base.OnError(new Exception.InsufficientException(error));
                }
                else {
                    base.OnError(error);
                }
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator ConsumeStaminaByUserId(
                Request.ConsumeStaminaByUserIdRequest request,
                UnityAction<AsyncResult<Result.ConsumeStaminaByUserIdResult>> callback
        ) =>
            new ConsumeStaminaByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.ConsumeStaminaByUserIdResult> ConsumeStaminaByUserIdFuture(
                Request.ConsumeStaminaByUserIdRequest request
        ) =>
            new ConsumeStaminaByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.ConsumeStaminaByUserIdResult> ConsumeStaminaByUserIdAsync(
                Request.ConsumeStaminaByUserIdRequest request
        ) =>
            new ConsumeStaminaByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.ConsumeStaminaByUserIdResult>();
    #else
		public ConsumeStaminaByUserIdTask ConsumeStaminaByUserIdAsync(
                Request.ConsumeStaminaByUserIdRequest request
        )
		{
			return new ConsumeStaminaByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.ConsumeStaminaByUserIdResult> ConsumeStaminaByUserIdAsync(
                Request.ConsumeStaminaByUserIdRequest request
        ) =>
            new ConsumeStaminaByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class ApplyStaminaTask : Gs2RestSessionTask<ApplyStaminaRequest, ApplyStaminaResult>
        {
            public ApplyStaminaTask(IGs2Session session, RestSessionRequestFactory factory, ApplyStaminaRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(ApplyStaminaRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "stamina")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/me/stamina/{staminaName}/apply";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{staminaName}", !string.IsNullOrEmpty(request.StaminaName) ? request.StaminaName.ToString() : "null");

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
		public IEnumerator ApplyStamina(
                Request.ApplyStaminaRequest request,
                UnityAction<AsyncResult<Result.ApplyStaminaResult>> callback
        ) =>
            new ApplyStaminaTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.ApplyStaminaResult> ApplyStaminaFuture(
                Request.ApplyStaminaRequest request
        ) =>
            new ApplyStaminaTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.ApplyStaminaResult> ApplyStaminaAsync(
                Request.ApplyStaminaRequest request
        ) =>
            new ApplyStaminaTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.ApplyStaminaResult>();
    #else
		public ApplyStaminaTask ApplyStaminaAsync(
                Request.ApplyStaminaRequest request
        )
		{
			return new ApplyStaminaTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.ApplyStaminaResult> ApplyStaminaAsync(
                Request.ApplyStaminaRequest request
        ) =>
            new ApplyStaminaTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class ApplyStaminaByUserIdTask : Gs2RestSessionTask<ApplyStaminaByUserIdRequest, ApplyStaminaByUserIdResult>
        {
            public ApplyStaminaByUserIdTask(IGs2Session session, RestSessionRequestFactory factory, ApplyStaminaByUserIdRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(ApplyStaminaByUserIdRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "stamina")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/{userId}/stamina/{staminaName}/apply";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{staminaName}", !string.IsNullOrEmpty(request.StaminaName) ? request.StaminaName.ToString() : "null");
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
		public IEnumerator ApplyStaminaByUserId(
                Request.ApplyStaminaByUserIdRequest request,
                UnityAction<AsyncResult<Result.ApplyStaminaByUserIdResult>> callback
        ) =>
            new ApplyStaminaByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.ApplyStaminaByUserIdResult> ApplyStaminaByUserIdFuture(
                Request.ApplyStaminaByUserIdRequest request
        ) =>
            new ApplyStaminaByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.ApplyStaminaByUserIdResult> ApplyStaminaByUserIdAsync(
                Request.ApplyStaminaByUserIdRequest request
        ) =>
            new ApplyStaminaByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.ApplyStaminaByUserIdResult>();
    #else
		public ApplyStaminaByUserIdTask ApplyStaminaByUserIdAsync(
                Request.ApplyStaminaByUserIdRequest request
        )
		{
			return new ApplyStaminaByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.ApplyStaminaByUserIdResult> ApplyStaminaByUserIdAsync(
                Request.ApplyStaminaByUserIdRequest request
        ) =>
            new ApplyStaminaByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class RecoverStaminaByUserIdTask : Gs2RestSessionTask<RecoverStaminaByUserIdRequest, RecoverStaminaByUserIdResult>
        {
            public RecoverStaminaByUserIdTask(IGs2Session session, RestSessionRequestFactory factory, RecoverStaminaByUserIdRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(RecoverStaminaByUserIdRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "stamina")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/{userId}/stamina/{staminaName}/recover";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{staminaName}", !string.IsNullOrEmpty(request.StaminaName) ? request.StaminaName.ToString() : "null");
                url = url.Replace("{userId}", !string.IsNullOrEmpty(request.UserId) ? request.UserId.ToString() : "null");

                var sessionRequest = Factory.Post(url);

                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);
                jsonWriter.WriteObjectStart();
                if (request.RecoverValue != null)
                {
                    jsonWriter.WritePropertyName("recoverValue");
                    jsonWriter.Write(request.RecoverValue.ToString());
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
		public IEnumerator RecoverStaminaByUserId(
                Request.RecoverStaminaByUserIdRequest request,
                UnityAction<AsyncResult<Result.RecoverStaminaByUserIdResult>> callback
        ) =>
            new RecoverStaminaByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.RecoverStaminaByUserIdResult> RecoverStaminaByUserIdFuture(
                Request.RecoverStaminaByUserIdRequest request
        ) =>
            new RecoverStaminaByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.RecoverStaminaByUserIdResult> RecoverStaminaByUserIdAsync(
                Request.RecoverStaminaByUserIdRequest request
        ) =>
            new RecoverStaminaByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.RecoverStaminaByUserIdResult>();
    #else
		public RecoverStaminaByUserIdTask RecoverStaminaByUserIdAsync(
                Request.RecoverStaminaByUserIdRequest request
        )
		{
			return new RecoverStaminaByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.RecoverStaminaByUserIdResult> RecoverStaminaByUserIdAsync(
                Request.RecoverStaminaByUserIdRequest request
        ) =>
            new RecoverStaminaByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class RaiseMaxValueByUserIdTask : Gs2RestSessionTask<RaiseMaxValueByUserIdRequest, RaiseMaxValueByUserIdResult>
        {
            public RaiseMaxValueByUserIdTask(IGs2Session session, RestSessionRequestFactory factory, RaiseMaxValueByUserIdRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(RaiseMaxValueByUserIdRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "stamina")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/{userId}/stamina/{staminaName}/raise";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{staminaName}", !string.IsNullOrEmpty(request.StaminaName) ? request.StaminaName.ToString() : "null");
                url = url.Replace("{userId}", !string.IsNullOrEmpty(request.UserId) ? request.UserId.ToString() : "null");

                var sessionRequest = Factory.Post(url);

                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);
                jsonWriter.WriteObjectStart();
                if (request.RaiseValue != null)
                {
                    jsonWriter.WritePropertyName("raiseValue");
                    jsonWriter.Write(request.RaiseValue.ToString());
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
		public IEnumerator RaiseMaxValueByUserId(
                Request.RaiseMaxValueByUserIdRequest request,
                UnityAction<AsyncResult<Result.RaiseMaxValueByUserIdResult>> callback
        ) =>
            new RaiseMaxValueByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.RaiseMaxValueByUserIdResult> RaiseMaxValueByUserIdFuture(
                Request.RaiseMaxValueByUserIdRequest request
        ) =>
            new RaiseMaxValueByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.RaiseMaxValueByUserIdResult> RaiseMaxValueByUserIdAsync(
                Request.RaiseMaxValueByUserIdRequest request
        ) =>
            new RaiseMaxValueByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.RaiseMaxValueByUserIdResult>();
    #else
		public RaiseMaxValueByUserIdTask RaiseMaxValueByUserIdAsync(
                Request.RaiseMaxValueByUserIdRequest request
        )
		{
			return new RaiseMaxValueByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.RaiseMaxValueByUserIdResult> RaiseMaxValueByUserIdAsync(
                Request.RaiseMaxValueByUserIdRequest request
        ) =>
            new RaiseMaxValueByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class DecreaseMaxValueTask : Gs2RestSessionTask<DecreaseMaxValueRequest, DecreaseMaxValueResult>
        {
            public DecreaseMaxValueTask(IGs2Session session, RestSessionRequestFactory factory, DecreaseMaxValueRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DecreaseMaxValueRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "stamina")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/me/stamina/{staminaName}/decrease";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{staminaName}", !string.IsNullOrEmpty(request.StaminaName) ? request.StaminaName.ToString() : "null");

                var sessionRequest = Factory.Post(url);

                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);
                jsonWriter.WriteObjectStart();
                if (request.DecreaseValue != null)
                {
                    jsonWriter.WritePropertyName("decreaseValue");
                    jsonWriter.Write(request.DecreaseValue.ToString());
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
		public IEnumerator DecreaseMaxValue(
                Request.DecreaseMaxValueRequest request,
                UnityAction<AsyncResult<Result.DecreaseMaxValueResult>> callback
        ) =>
            new DecreaseMaxValueTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.DecreaseMaxValueResult> DecreaseMaxValueFuture(
                Request.DecreaseMaxValueRequest request
        ) =>
            new DecreaseMaxValueTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.DecreaseMaxValueResult> DecreaseMaxValueAsync(
                Request.DecreaseMaxValueRequest request
        ) =>
            new DecreaseMaxValueTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.DecreaseMaxValueResult>();
    #else
		public DecreaseMaxValueTask DecreaseMaxValueAsync(
                Request.DecreaseMaxValueRequest request
        )
		{
			return new DecreaseMaxValueTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.DecreaseMaxValueResult> DecreaseMaxValueAsync(
                Request.DecreaseMaxValueRequest request
        ) =>
            new DecreaseMaxValueTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class DecreaseMaxValueByUserIdTask : Gs2RestSessionTask<DecreaseMaxValueByUserIdRequest, DecreaseMaxValueByUserIdResult>
        {
            public DecreaseMaxValueByUserIdTask(IGs2Session session, RestSessionRequestFactory factory, DecreaseMaxValueByUserIdRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DecreaseMaxValueByUserIdRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "stamina")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/{userId}/stamina/{staminaName}/decrease";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{staminaName}", !string.IsNullOrEmpty(request.StaminaName) ? request.StaminaName.ToString() : "null");
                url = url.Replace("{userId}", !string.IsNullOrEmpty(request.UserId) ? request.UserId.ToString() : "null");

                var sessionRequest = Factory.Post(url);

                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);
                jsonWriter.WriteObjectStart();
                if (request.DecreaseValue != null)
                {
                    jsonWriter.WritePropertyName("decreaseValue");
                    jsonWriter.Write(request.DecreaseValue.ToString());
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
		public IEnumerator DecreaseMaxValueByUserId(
                Request.DecreaseMaxValueByUserIdRequest request,
                UnityAction<AsyncResult<Result.DecreaseMaxValueByUserIdResult>> callback
        ) =>
            new DecreaseMaxValueByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.DecreaseMaxValueByUserIdResult> DecreaseMaxValueByUserIdFuture(
                Request.DecreaseMaxValueByUserIdRequest request
        ) =>
            new DecreaseMaxValueByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.DecreaseMaxValueByUserIdResult> DecreaseMaxValueByUserIdAsync(
                Request.DecreaseMaxValueByUserIdRequest request
        ) =>
            new DecreaseMaxValueByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.DecreaseMaxValueByUserIdResult>();
    #else
		public DecreaseMaxValueByUserIdTask DecreaseMaxValueByUserIdAsync(
                Request.DecreaseMaxValueByUserIdRequest request
        )
		{
			return new DecreaseMaxValueByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.DecreaseMaxValueByUserIdResult> DecreaseMaxValueByUserIdAsync(
                Request.DecreaseMaxValueByUserIdRequest request
        ) =>
            new DecreaseMaxValueByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class SetMaxValueByUserIdTask : Gs2RestSessionTask<SetMaxValueByUserIdRequest, SetMaxValueByUserIdResult>
        {
            public SetMaxValueByUserIdTask(IGs2Session session, RestSessionRequestFactory factory, SetMaxValueByUserIdRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(SetMaxValueByUserIdRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "stamina")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/{userId}/stamina/{staminaName}/set";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{staminaName}", !string.IsNullOrEmpty(request.StaminaName) ? request.StaminaName.ToString() : "null");
                url = url.Replace("{userId}", !string.IsNullOrEmpty(request.UserId) ? request.UserId.ToString() : "null");

                var sessionRequest = Factory.Post(url);

                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);
                jsonWriter.WriteObjectStart();
                if (request.MaxValue != null)
                {
                    jsonWriter.WritePropertyName("maxValue");
                    jsonWriter.Write(request.MaxValue.ToString());
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
		public IEnumerator SetMaxValueByUserId(
                Request.SetMaxValueByUserIdRequest request,
                UnityAction<AsyncResult<Result.SetMaxValueByUserIdResult>> callback
        ) =>
            new SetMaxValueByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.SetMaxValueByUserIdResult> SetMaxValueByUserIdFuture(
                Request.SetMaxValueByUserIdRequest request
        ) =>
            new SetMaxValueByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.SetMaxValueByUserIdResult> SetMaxValueByUserIdAsync(
                Request.SetMaxValueByUserIdRequest request
        ) =>
            new SetMaxValueByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.SetMaxValueByUserIdResult>();
    #else
		public SetMaxValueByUserIdTask SetMaxValueByUserIdAsync(
                Request.SetMaxValueByUserIdRequest request
        )
		{
			return new SetMaxValueByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.SetMaxValueByUserIdResult> SetMaxValueByUserIdAsync(
                Request.SetMaxValueByUserIdRequest request
        ) =>
            new SetMaxValueByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class SetRecoverIntervalByUserIdTask : Gs2RestSessionTask<SetRecoverIntervalByUserIdRequest, SetRecoverIntervalByUserIdResult>
        {
            public SetRecoverIntervalByUserIdTask(IGs2Session session, RestSessionRequestFactory factory, SetRecoverIntervalByUserIdRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(SetRecoverIntervalByUserIdRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "stamina")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/{userId}/stamina/{staminaName}/recoverInterval/set";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{staminaName}", !string.IsNullOrEmpty(request.StaminaName) ? request.StaminaName.ToString() : "null");
                url = url.Replace("{userId}", !string.IsNullOrEmpty(request.UserId) ? request.UserId.ToString() : "null");

                var sessionRequest = Factory.Post(url);

                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);
                jsonWriter.WriteObjectStart();
                if (request.RecoverIntervalMinutes != null)
                {
                    jsonWriter.WritePropertyName("recoverIntervalMinutes");
                    jsonWriter.Write(request.RecoverIntervalMinutes.ToString());
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
		public IEnumerator SetRecoverIntervalByUserId(
                Request.SetRecoverIntervalByUserIdRequest request,
                UnityAction<AsyncResult<Result.SetRecoverIntervalByUserIdResult>> callback
        ) =>
            new SetRecoverIntervalByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.SetRecoverIntervalByUserIdResult> SetRecoverIntervalByUserIdFuture(
                Request.SetRecoverIntervalByUserIdRequest request
        ) =>
            new SetRecoverIntervalByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.SetRecoverIntervalByUserIdResult> SetRecoverIntervalByUserIdAsync(
                Request.SetRecoverIntervalByUserIdRequest request
        ) =>
            new SetRecoverIntervalByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.SetRecoverIntervalByUserIdResult>();
    #else
		public SetRecoverIntervalByUserIdTask SetRecoverIntervalByUserIdAsync(
                Request.SetRecoverIntervalByUserIdRequest request
        )
		{
			return new SetRecoverIntervalByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.SetRecoverIntervalByUserIdResult> SetRecoverIntervalByUserIdAsync(
                Request.SetRecoverIntervalByUserIdRequest request
        ) =>
            new SetRecoverIntervalByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class SetRecoverValueByUserIdTask : Gs2RestSessionTask<SetRecoverValueByUserIdRequest, SetRecoverValueByUserIdResult>
        {
            public SetRecoverValueByUserIdTask(IGs2Session session, RestSessionRequestFactory factory, SetRecoverValueByUserIdRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(SetRecoverValueByUserIdRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "stamina")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/{userId}/stamina/{staminaName}/recoverValue/set";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{staminaName}", !string.IsNullOrEmpty(request.StaminaName) ? request.StaminaName.ToString() : "null");
                url = url.Replace("{userId}", !string.IsNullOrEmpty(request.UserId) ? request.UserId.ToString() : "null");

                var sessionRequest = Factory.Post(url);

                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);
                jsonWriter.WriteObjectStart();
                if (request.RecoverValue != null)
                {
                    jsonWriter.WritePropertyName("recoverValue");
                    jsonWriter.Write(request.RecoverValue.ToString());
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
		public IEnumerator SetRecoverValueByUserId(
                Request.SetRecoverValueByUserIdRequest request,
                UnityAction<AsyncResult<Result.SetRecoverValueByUserIdResult>> callback
        ) =>
            new SetRecoverValueByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.SetRecoverValueByUserIdResult> SetRecoverValueByUserIdFuture(
                Request.SetRecoverValueByUserIdRequest request
        ) =>
            new SetRecoverValueByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.SetRecoverValueByUserIdResult> SetRecoverValueByUserIdAsync(
                Request.SetRecoverValueByUserIdRequest request
        ) =>
            new SetRecoverValueByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.SetRecoverValueByUserIdResult>();
    #else
		public SetRecoverValueByUserIdTask SetRecoverValueByUserIdAsync(
                Request.SetRecoverValueByUserIdRequest request
        )
		{
			return new SetRecoverValueByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.SetRecoverValueByUserIdResult> SetRecoverValueByUserIdAsync(
                Request.SetRecoverValueByUserIdRequest request
        ) =>
            new SetRecoverValueByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class SetMaxValueByStatusTask : Gs2RestSessionTask<SetMaxValueByStatusRequest, SetMaxValueByStatusResult>
        {
            public SetMaxValueByStatusTask(IGs2Session session, RestSessionRequestFactory factory, SetMaxValueByStatusRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(SetMaxValueByStatusRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "stamina")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/me/stamina/{staminaName}/set";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{staminaName}", !string.IsNullOrEmpty(request.StaminaName) ? request.StaminaName.ToString() : "null");

                var sessionRequest = Factory.Post(url);

                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);
                jsonWriter.WriteObjectStart();
                if (request.KeyId != null)
                {
                    jsonWriter.WritePropertyName("keyId");
                    jsonWriter.Write(request.KeyId);
                }
                if (request.SignedStatusBody != null)
                {
                    jsonWriter.WritePropertyName("signedStatusBody");
                    jsonWriter.Write(request.SignedStatusBody);
                }
                if (request.SignedStatusSignature != null)
                {
                    jsonWriter.WritePropertyName("signedStatusSignature");
                    jsonWriter.Write(request.SignedStatusSignature);
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
		public IEnumerator SetMaxValueByStatus(
                Request.SetMaxValueByStatusRequest request,
                UnityAction<AsyncResult<Result.SetMaxValueByStatusResult>> callback
        ) =>
            new SetMaxValueByStatusTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.SetMaxValueByStatusResult> SetMaxValueByStatusFuture(
                Request.SetMaxValueByStatusRequest request
        ) =>
            new SetMaxValueByStatusTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.SetMaxValueByStatusResult> SetMaxValueByStatusAsync(
                Request.SetMaxValueByStatusRequest request
        ) =>
            new SetMaxValueByStatusTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.SetMaxValueByStatusResult>();
    #else
		public SetMaxValueByStatusTask SetMaxValueByStatusAsync(
                Request.SetMaxValueByStatusRequest request
        )
		{
			return new SetMaxValueByStatusTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.SetMaxValueByStatusResult> SetMaxValueByStatusAsync(
                Request.SetMaxValueByStatusRequest request
        ) =>
            new SetMaxValueByStatusTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class SetRecoverIntervalByStatusTask : Gs2RestSessionTask<SetRecoverIntervalByStatusRequest, SetRecoverIntervalByStatusResult>
        {
            public SetRecoverIntervalByStatusTask(IGs2Session session, RestSessionRequestFactory factory, SetRecoverIntervalByStatusRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(SetRecoverIntervalByStatusRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "stamina")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/me/stamina/{staminaName}/recoverInterval/set";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{staminaName}", !string.IsNullOrEmpty(request.StaminaName) ? request.StaminaName.ToString() : "null");

                var sessionRequest = Factory.Post(url);

                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);
                jsonWriter.WriteObjectStart();
                if (request.KeyId != null)
                {
                    jsonWriter.WritePropertyName("keyId");
                    jsonWriter.Write(request.KeyId);
                }
                if (request.SignedStatusBody != null)
                {
                    jsonWriter.WritePropertyName("signedStatusBody");
                    jsonWriter.Write(request.SignedStatusBody);
                }
                if (request.SignedStatusSignature != null)
                {
                    jsonWriter.WritePropertyName("signedStatusSignature");
                    jsonWriter.Write(request.SignedStatusSignature);
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
		public IEnumerator SetRecoverIntervalByStatus(
                Request.SetRecoverIntervalByStatusRequest request,
                UnityAction<AsyncResult<Result.SetRecoverIntervalByStatusResult>> callback
        ) =>
            new SetRecoverIntervalByStatusTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.SetRecoverIntervalByStatusResult> SetRecoverIntervalByStatusFuture(
                Request.SetRecoverIntervalByStatusRequest request
        ) =>
            new SetRecoverIntervalByStatusTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.SetRecoverIntervalByStatusResult> SetRecoverIntervalByStatusAsync(
                Request.SetRecoverIntervalByStatusRequest request
        ) =>
            new SetRecoverIntervalByStatusTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.SetRecoverIntervalByStatusResult>();
    #else
		public SetRecoverIntervalByStatusTask SetRecoverIntervalByStatusAsync(
                Request.SetRecoverIntervalByStatusRequest request
        )
		{
			return new SetRecoverIntervalByStatusTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.SetRecoverIntervalByStatusResult> SetRecoverIntervalByStatusAsync(
                Request.SetRecoverIntervalByStatusRequest request
        ) =>
            new SetRecoverIntervalByStatusTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class SetRecoverValueByStatusTask : Gs2RestSessionTask<SetRecoverValueByStatusRequest, SetRecoverValueByStatusResult>
        {
            public SetRecoverValueByStatusTask(IGs2Session session, RestSessionRequestFactory factory, SetRecoverValueByStatusRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(SetRecoverValueByStatusRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "stamina")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/me/stamina/{staminaName}/recoverValue/set";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{staminaName}", !string.IsNullOrEmpty(request.StaminaName) ? request.StaminaName.ToString() : "null");

                var sessionRequest = Factory.Post(url);

                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);
                jsonWriter.WriteObjectStart();
                if (request.KeyId != null)
                {
                    jsonWriter.WritePropertyName("keyId");
                    jsonWriter.Write(request.KeyId);
                }
                if (request.SignedStatusBody != null)
                {
                    jsonWriter.WritePropertyName("signedStatusBody");
                    jsonWriter.Write(request.SignedStatusBody);
                }
                if (request.SignedStatusSignature != null)
                {
                    jsonWriter.WritePropertyName("signedStatusSignature");
                    jsonWriter.Write(request.SignedStatusSignature);
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
		public IEnumerator SetRecoverValueByStatus(
                Request.SetRecoverValueByStatusRequest request,
                UnityAction<AsyncResult<Result.SetRecoverValueByStatusResult>> callback
        ) =>
            new SetRecoverValueByStatusTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.SetRecoverValueByStatusResult> SetRecoverValueByStatusFuture(
                Request.SetRecoverValueByStatusRequest request
        ) =>
            new SetRecoverValueByStatusTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.SetRecoverValueByStatusResult> SetRecoverValueByStatusAsync(
                Request.SetRecoverValueByStatusRequest request
        ) =>
            new SetRecoverValueByStatusTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.SetRecoverValueByStatusResult>();
    #else
		public SetRecoverValueByStatusTask SetRecoverValueByStatusAsync(
                Request.SetRecoverValueByStatusRequest request
        )
		{
			return new SetRecoverValueByStatusTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.SetRecoverValueByStatusResult> SetRecoverValueByStatusAsync(
                Request.SetRecoverValueByStatusRequest request
        ) =>
            new SetRecoverValueByStatusTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class DeleteStaminaByUserIdTask : Gs2RestSessionTask<DeleteStaminaByUserIdRequest, DeleteStaminaByUserIdResult>
        {
            public DeleteStaminaByUserIdTask(IGs2Session session, RestSessionRequestFactory factory, DeleteStaminaByUserIdRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DeleteStaminaByUserIdRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "stamina")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/{userId}/stamina/{staminaName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{staminaName}", !string.IsNullOrEmpty(request.StaminaName) ? request.StaminaName.ToString() : "null");
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
		public IEnumerator DeleteStaminaByUserId(
                Request.DeleteStaminaByUserIdRequest request,
                UnityAction<AsyncResult<Result.DeleteStaminaByUserIdResult>> callback
        ) =>
            new DeleteStaminaByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.DeleteStaminaByUserIdResult> DeleteStaminaByUserIdFuture(
                Request.DeleteStaminaByUserIdRequest request
        ) =>
            new DeleteStaminaByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.DeleteStaminaByUserIdResult> DeleteStaminaByUserIdAsync(
                Request.DeleteStaminaByUserIdRequest request
        ) =>
            new DeleteStaminaByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.DeleteStaminaByUserIdResult>();
    #else
		public DeleteStaminaByUserIdTask DeleteStaminaByUserIdAsync(
                Request.DeleteStaminaByUserIdRequest request
        )
		{
			return new DeleteStaminaByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.DeleteStaminaByUserIdResult> DeleteStaminaByUserIdAsync(
                Request.DeleteStaminaByUserIdRequest request
        ) =>
            new DeleteStaminaByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class VerifyStaminaValueTask : Gs2RestSessionTask<VerifyStaminaValueRequest, VerifyStaminaValueResult>
        {
            public VerifyStaminaValueTask(IGs2Session session, RestSessionRequestFactory factory, VerifyStaminaValueRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(VerifyStaminaValueRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "stamina")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/me/stamina/{staminaName}/value/verify/{verifyType}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{staminaName}", !string.IsNullOrEmpty(request.StaminaName) ? request.StaminaName.ToString() : "null");
                url = url.Replace("{verifyType}", !string.IsNullOrEmpty(request.VerifyType) ? request.VerifyType.ToString() : "null");

                var sessionRequest = Factory.Post(url);

                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);
                jsonWriter.WriteObjectStart();
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
		public IEnumerator VerifyStaminaValue(
                Request.VerifyStaminaValueRequest request,
                UnityAction<AsyncResult<Result.VerifyStaminaValueResult>> callback
        ) =>
            new VerifyStaminaValueTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.VerifyStaminaValueResult> VerifyStaminaValueFuture(
                Request.VerifyStaminaValueRequest request
        ) =>
            new VerifyStaminaValueTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.VerifyStaminaValueResult> VerifyStaminaValueAsync(
                Request.VerifyStaminaValueRequest request
        ) =>
            new VerifyStaminaValueTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.VerifyStaminaValueResult>();
    #else
		public VerifyStaminaValueTask VerifyStaminaValueAsync(
                Request.VerifyStaminaValueRequest request
        )
		{
			return new VerifyStaminaValueTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.VerifyStaminaValueResult> VerifyStaminaValueAsync(
                Request.VerifyStaminaValueRequest request
        ) =>
            new VerifyStaminaValueTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class VerifyStaminaValueByUserIdTask : Gs2RestSessionTask<VerifyStaminaValueByUserIdRequest, VerifyStaminaValueByUserIdResult>
        {
            public VerifyStaminaValueByUserIdTask(IGs2Session session, RestSessionRequestFactory factory, VerifyStaminaValueByUserIdRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(VerifyStaminaValueByUserIdRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "stamina")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/{userId}/stamina/{staminaName}/value/verify/{verifyType}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{userId}", !string.IsNullOrEmpty(request.UserId) ? request.UserId.ToString() : "null");
                url = url.Replace("{staminaName}", !string.IsNullOrEmpty(request.StaminaName) ? request.StaminaName.ToString() : "null");
                url = url.Replace("{verifyType}", !string.IsNullOrEmpty(request.VerifyType) ? request.VerifyType.ToString() : "null");

                var sessionRequest = Factory.Post(url);

                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);
                jsonWriter.WriteObjectStart();
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
		public IEnumerator VerifyStaminaValueByUserId(
                Request.VerifyStaminaValueByUserIdRequest request,
                UnityAction<AsyncResult<Result.VerifyStaminaValueByUserIdResult>> callback
        ) =>
            new VerifyStaminaValueByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.VerifyStaminaValueByUserIdResult> VerifyStaminaValueByUserIdFuture(
                Request.VerifyStaminaValueByUserIdRequest request
        ) =>
            new VerifyStaminaValueByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.VerifyStaminaValueByUserIdResult> VerifyStaminaValueByUserIdAsync(
                Request.VerifyStaminaValueByUserIdRequest request
        ) =>
            new VerifyStaminaValueByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.VerifyStaminaValueByUserIdResult>();
    #else
		public VerifyStaminaValueByUserIdTask VerifyStaminaValueByUserIdAsync(
                Request.VerifyStaminaValueByUserIdRequest request
        )
		{
			return new VerifyStaminaValueByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.VerifyStaminaValueByUserIdResult> VerifyStaminaValueByUserIdAsync(
                Request.VerifyStaminaValueByUserIdRequest request
        ) =>
            new VerifyStaminaValueByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class VerifyStaminaMaxValueTask : Gs2RestSessionTask<VerifyStaminaMaxValueRequest, VerifyStaminaMaxValueResult>
        {
            public VerifyStaminaMaxValueTask(IGs2Session session, RestSessionRequestFactory factory, VerifyStaminaMaxValueRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(VerifyStaminaMaxValueRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "stamina")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/me/stamina/{staminaName}/max/verify/{verifyType}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{staminaName}", !string.IsNullOrEmpty(request.StaminaName) ? request.StaminaName.ToString() : "null");
                url = url.Replace("{verifyType}", !string.IsNullOrEmpty(request.VerifyType) ? request.VerifyType.ToString() : "null");

                var sessionRequest = Factory.Post(url);

                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);
                jsonWriter.WriteObjectStart();
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
		public IEnumerator VerifyStaminaMaxValue(
                Request.VerifyStaminaMaxValueRequest request,
                UnityAction<AsyncResult<Result.VerifyStaminaMaxValueResult>> callback
        ) =>
            new VerifyStaminaMaxValueTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.VerifyStaminaMaxValueResult> VerifyStaminaMaxValueFuture(
                Request.VerifyStaminaMaxValueRequest request
        ) =>
            new VerifyStaminaMaxValueTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.VerifyStaminaMaxValueResult> VerifyStaminaMaxValueAsync(
                Request.VerifyStaminaMaxValueRequest request
        ) =>
            new VerifyStaminaMaxValueTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.VerifyStaminaMaxValueResult>();
    #else
		public VerifyStaminaMaxValueTask VerifyStaminaMaxValueAsync(
                Request.VerifyStaminaMaxValueRequest request
        )
		{
			return new VerifyStaminaMaxValueTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.VerifyStaminaMaxValueResult> VerifyStaminaMaxValueAsync(
                Request.VerifyStaminaMaxValueRequest request
        ) =>
            new VerifyStaminaMaxValueTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class VerifyStaminaMaxValueByUserIdTask : Gs2RestSessionTask<VerifyStaminaMaxValueByUserIdRequest, VerifyStaminaMaxValueByUserIdResult>
        {
            public VerifyStaminaMaxValueByUserIdTask(IGs2Session session, RestSessionRequestFactory factory, VerifyStaminaMaxValueByUserIdRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(VerifyStaminaMaxValueByUserIdRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "stamina")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/{userId}/stamina/{staminaName}/max/verify/{verifyType}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{userId}", !string.IsNullOrEmpty(request.UserId) ? request.UserId.ToString() : "null");
                url = url.Replace("{staminaName}", !string.IsNullOrEmpty(request.StaminaName) ? request.StaminaName.ToString() : "null");
                url = url.Replace("{verifyType}", !string.IsNullOrEmpty(request.VerifyType) ? request.VerifyType.ToString() : "null");

                var sessionRequest = Factory.Post(url);

                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);
                jsonWriter.WriteObjectStart();
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
		public IEnumerator VerifyStaminaMaxValueByUserId(
                Request.VerifyStaminaMaxValueByUserIdRequest request,
                UnityAction<AsyncResult<Result.VerifyStaminaMaxValueByUserIdResult>> callback
        ) =>
            new VerifyStaminaMaxValueByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.VerifyStaminaMaxValueByUserIdResult> VerifyStaminaMaxValueByUserIdFuture(
                Request.VerifyStaminaMaxValueByUserIdRequest request
        ) =>
            new VerifyStaminaMaxValueByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.VerifyStaminaMaxValueByUserIdResult> VerifyStaminaMaxValueByUserIdAsync(
                Request.VerifyStaminaMaxValueByUserIdRequest request
        ) =>
            new VerifyStaminaMaxValueByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.VerifyStaminaMaxValueByUserIdResult>();
    #else
		public VerifyStaminaMaxValueByUserIdTask VerifyStaminaMaxValueByUserIdAsync(
                Request.VerifyStaminaMaxValueByUserIdRequest request
        )
		{
			return new VerifyStaminaMaxValueByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.VerifyStaminaMaxValueByUserIdResult> VerifyStaminaMaxValueByUserIdAsync(
                Request.VerifyStaminaMaxValueByUserIdRequest request
        ) =>
            new VerifyStaminaMaxValueByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class VerifyStaminaRecoverIntervalMinutesTask : Gs2RestSessionTask<VerifyStaminaRecoverIntervalMinutesRequest, VerifyStaminaRecoverIntervalMinutesResult>
        {
            public VerifyStaminaRecoverIntervalMinutesTask(IGs2Session session, RestSessionRequestFactory factory, VerifyStaminaRecoverIntervalMinutesRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(VerifyStaminaRecoverIntervalMinutesRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "stamina")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/me/stamina/{staminaName}/recover/interval/verify/{verifyType}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{staminaName}", !string.IsNullOrEmpty(request.StaminaName) ? request.StaminaName.ToString() : "null");
                url = url.Replace("{verifyType}", !string.IsNullOrEmpty(request.VerifyType) ? request.VerifyType.ToString() : "null");

                var sessionRequest = Factory.Post(url);

                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);
                jsonWriter.WriteObjectStart();
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
		public IEnumerator VerifyStaminaRecoverIntervalMinutes(
                Request.VerifyStaminaRecoverIntervalMinutesRequest request,
                UnityAction<AsyncResult<Result.VerifyStaminaRecoverIntervalMinutesResult>> callback
        ) =>
            new VerifyStaminaRecoverIntervalMinutesTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.VerifyStaminaRecoverIntervalMinutesResult> VerifyStaminaRecoverIntervalMinutesFuture(
                Request.VerifyStaminaRecoverIntervalMinutesRequest request
        ) =>
            new VerifyStaminaRecoverIntervalMinutesTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.VerifyStaminaRecoverIntervalMinutesResult> VerifyStaminaRecoverIntervalMinutesAsync(
                Request.VerifyStaminaRecoverIntervalMinutesRequest request
        ) =>
            new VerifyStaminaRecoverIntervalMinutesTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.VerifyStaminaRecoverIntervalMinutesResult>();
    #else
		public VerifyStaminaRecoverIntervalMinutesTask VerifyStaminaRecoverIntervalMinutesAsync(
                Request.VerifyStaminaRecoverIntervalMinutesRequest request
        )
		{
			return new VerifyStaminaRecoverIntervalMinutesTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.VerifyStaminaRecoverIntervalMinutesResult> VerifyStaminaRecoverIntervalMinutesAsync(
                Request.VerifyStaminaRecoverIntervalMinutesRequest request
        ) =>
            new VerifyStaminaRecoverIntervalMinutesTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class VerifyStaminaRecoverIntervalMinutesByUserIdTask : Gs2RestSessionTask<VerifyStaminaRecoverIntervalMinutesByUserIdRequest, VerifyStaminaRecoverIntervalMinutesByUserIdResult>
        {
            public VerifyStaminaRecoverIntervalMinutesByUserIdTask(IGs2Session session, RestSessionRequestFactory factory, VerifyStaminaRecoverIntervalMinutesByUserIdRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(VerifyStaminaRecoverIntervalMinutesByUserIdRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "stamina")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/{userId}/stamina/{staminaName}/recover/interval/verify/{verifyType}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{userId}", !string.IsNullOrEmpty(request.UserId) ? request.UserId.ToString() : "null");
                url = url.Replace("{staminaName}", !string.IsNullOrEmpty(request.StaminaName) ? request.StaminaName.ToString() : "null");
                url = url.Replace("{verifyType}", !string.IsNullOrEmpty(request.VerifyType) ? request.VerifyType.ToString() : "null");

                var sessionRequest = Factory.Post(url);

                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);
                jsonWriter.WriteObjectStart();
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
		public IEnumerator VerifyStaminaRecoverIntervalMinutesByUserId(
                Request.VerifyStaminaRecoverIntervalMinutesByUserIdRequest request,
                UnityAction<AsyncResult<Result.VerifyStaminaRecoverIntervalMinutesByUserIdResult>> callback
        ) =>
            new VerifyStaminaRecoverIntervalMinutesByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.VerifyStaminaRecoverIntervalMinutesByUserIdResult> VerifyStaminaRecoverIntervalMinutesByUserIdFuture(
                Request.VerifyStaminaRecoverIntervalMinutesByUserIdRequest request
        ) =>
            new VerifyStaminaRecoverIntervalMinutesByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.VerifyStaminaRecoverIntervalMinutesByUserIdResult> VerifyStaminaRecoverIntervalMinutesByUserIdAsync(
                Request.VerifyStaminaRecoverIntervalMinutesByUserIdRequest request
        ) =>
            new VerifyStaminaRecoverIntervalMinutesByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.VerifyStaminaRecoverIntervalMinutesByUserIdResult>();
    #else
		public VerifyStaminaRecoverIntervalMinutesByUserIdTask VerifyStaminaRecoverIntervalMinutesByUserIdAsync(
                Request.VerifyStaminaRecoverIntervalMinutesByUserIdRequest request
        )
		{
			return new VerifyStaminaRecoverIntervalMinutesByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.VerifyStaminaRecoverIntervalMinutesByUserIdResult> VerifyStaminaRecoverIntervalMinutesByUserIdAsync(
                Request.VerifyStaminaRecoverIntervalMinutesByUserIdRequest request
        ) =>
            new VerifyStaminaRecoverIntervalMinutesByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class VerifyStaminaRecoverValueTask : Gs2RestSessionTask<VerifyStaminaRecoverValueRequest, VerifyStaminaRecoverValueResult>
        {
            public VerifyStaminaRecoverValueTask(IGs2Session session, RestSessionRequestFactory factory, VerifyStaminaRecoverValueRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(VerifyStaminaRecoverValueRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "stamina")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/me/stamina/{staminaName}/recover/value/verify/{verifyType}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{staminaName}", !string.IsNullOrEmpty(request.StaminaName) ? request.StaminaName.ToString() : "null");
                url = url.Replace("{verifyType}", !string.IsNullOrEmpty(request.VerifyType) ? request.VerifyType.ToString() : "null");

                var sessionRequest = Factory.Post(url);

                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);
                jsonWriter.WriteObjectStart();
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
		public IEnumerator VerifyStaminaRecoverValue(
                Request.VerifyStaminaRecoverValueRequest request,
                UnityAction<AsyncResult<Result.VerifyStaminaRecoverValueResult>> callback
        ) =>
            new VerifyStaminaRecoverValueTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.VerifyStaminaRecoverValueResult> VerifyStaminaRecoverValueFuture(
                Request.VerifyStaminaRecoverValueRequest request
        ) =>
            new VerifyStaminaRecoverValueTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.VerifyStaminaRecoverValueResult> VerifyStaminaRecoverValueAsync(
                Request.VerifyStaminaRecoverValueRequest request
        ) =>
            new VerifyStaminaRecoverValueTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.VerifyStaminaRecoverValueResult>();
    #else
		public VerifyStaminaRecoverValueTask VerifyStaminaRecoverValueAsync(
                Request.VerifyStaminaRecoverValueRequest request
        )
		{
			return new VerifyStaminaRecoverValueTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.VerifyStaminaRecoverValueResult> VerifyStaminaRecoverValueAsync(
                Request.VerifyStaminaRecoverValueRequest request
        ) =>
            new VerifyStaminaRecoverValueTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class VerifyStaminaRecoverValueByUserIdTask : Gs2RestSessionTask<VerifyStaminaRecoverValueByUserIdRequest, VerifyStaminaRecoverValueByUserIdResult>
        {
            public VerifyStaminaRecoverValueByUserIdTask(IGs2Session session, RestSessionRequestFactory factory, VerifyStaminaRecoverValueByUserIdRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(VerifyStaminaRecoverValueByUserIdRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "stamina")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/{userId}/stamina/{staminaName}/recover/value/verify/{verifyType}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{userId}", !string.IsNullOrEmpty(request.UserId) ? request.UserId.ToString() : "null");
                url = url.Replace("{staminaName}", !string.IsNullOrEmpty(request.StaminaName) ? request.StaminaName.ToString() : "null");
                url = url.Replace("{verifyType}", !string.IsNullOrEmpty(request.VerifyType) ? request.VerifyType.ToString() : "null");

                var sessionRequest = Factory.Post(url);

                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);
                jsonWriter.WriteObjectStart();
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
		public IEnumerator VerifyStaminaRecoverValueByUserId(
                Request.VerifyStaminaRecoverValueByUserIdRequest request,
                UnityAction<AsyncResult<Result.VerifyStaminaRecoverValueByUserIdResult>> callback
        ) =>
            new VerifyStaminaRecoverValueByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.VerifyStaminaRecoverValueByUserIdResult> VerifyStaminaRecoverValueByUserIdFuture(
                Request.VerifyStaminaRecoverValueByUserIdRequest request
        ) =>
            new VerifyStaminaRecoverValueByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.VerifyStaminaRecoverValueByUserIdResult> VerifyStaminaRecoverValueByUserIdAsync(
                Request.VerifyStaminaRecoverValueByUserIdRequest request
        ) =>
            new VerifyStaminaRecoverValueByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.VerifyStaminaRecoverValueByUserIdResult>();
    #else
		public VerifyStaminaRecoverValueByUserIdTask VerifyStaminaRecoverValueByUserIdAsync(
                Request.VerifyStaminaRecoverValueByUserIdRequest request
        )
		{
			return new VerifyStaminaRecoverValueByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.VerifyStaminaRecoverValueByUserIdResult> VerifyStaminaRecoverValueByUserIdAsync(
                Request.VerifyStaminaRecoverValueByUserIdRequest request
        ) =>
            new VerifyStaminaRecoverValueByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class VerifyStaminaOverflowValueTask : Gs2RestSessionTask<VerifyStaminaOverflowValueRequest, VerifyStaminaOverflowValueResult>
        {
            public VerifyStaminaOverflowValueTask(IGs2Session session, RestSessionRequestFactory factory, VerifyStaminaOverflowValueRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(VerifyStaminaOverflowValueRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "stamina")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/me/stamina/{staminaName}/overflow/value/verify/{verifyType}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{staminaName}", !string.IsNullOrEmpty(request.StaminaName) ? request.StaminaName.ToString() : "null");
                url = url.Replace("{verifyType}", !string.IsNullOrEmpty(request.VerifyType) ? request.VerifyType.ToString() : "null");

                var sessionRequest = Factory.Post(url);

                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);
                jsonWriter.WriteObjectStart();
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
		public IEnumerator VerifyStaminaOverflowValue(
                Request.VerifyStaminaOverflowValueRequest request,
                UnityAction<AsyncResult<Result.VerifyStaminaOverflowValueResult>> callback
        ) =>
            new VerifyStaminaOverflowValueTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.VerifyStaminaOverflowValueResult> VerifyStaminaOverflowValueFuture(
                Request.VerifyStaminaOverflowValueRequest request
        ) =>
            new VerifyStaminaOverflowValueTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.VerifyStaminaOverflowValueResult> VerifyStaminaOverflowValueAsync(
                Request.VerifyStaminaOverflowValueRequest request
        ) =>
            new VerifyStaminaOverflowValueTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.VerifyStaminaOverflowValueResult>();
    #else
		public VerifyStaminaOverflowValueTask VerifyStaminaOverflowValueAsync(
                Request.VerifyStaminaOverflowValueRequest request
        )
		{
			return new VerifyStaminaOverflowValueTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.VerifyStaminaOverflowValueResult> VerifyStaminaOverflowValueAsync(
                Request.VerifyStaminaOverflowValueRequest request
        ) =>
            new VerifyStaminaOverflowValueTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class VerifyStaminaOverflowValueByUserIdTask : Gs2RestSessionTask<VerifyStaminaOverflowValueByUserIdRequest, VerifyStaminaOverflowValueByUserIdResult>
        {
            public VerifyStaminaOverflowValueByUserIdTask(IGs2Session session, RestSessionRequestFactory factory, VerifyStaminaOverflowValueByUserIdRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(VerifyStaminaOverflowValueByUserIdRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "stamina")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/{userId}/stamina/{staminaName}/overflow/value/verify/{verifyType}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{userId}", !string.IsNullOrEmpty(request.UserId) ? request.UserId.ToString() : "null");
                url = url.Replace("{staminaName}", !string.IsNullOrEmpty(request.StaminaName) ? request.StaminaName.ToString() : "null");
                url = url.Replace("{verifyType}", !string.IsNullOrEmpty(request.VerifyType) ? request.VerifyType.ToString() : "null");

                var sessionRequest = Factory.Post(url);

                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);
                jsonWriter.WriteObjectStart();
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
		public IEnumerator VerifyStaminaOverflowValueByUserId(
                Request.VerifyStaminaOverflowValueByUserIdRequest request,
                UnityAction<AsyncResult<Result.VerifyStaminaOverflowValueByUserIdResult>> callback
        ) =>
            new VerifyStaminaOverflowValueByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.VerifyStaminaOverflowValueByUserIdResult> VerifyStaminaOverflowValueByUserIdFuture(
                Request.VerifyStaminaOverflowValueByUserIdRequest request
        ) =>
            new VerifyStaminaOverflowValueByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.VerifyStaminaOverflowValueByUserIdResult> VerifyStaminaOverflowValueByUserIdAsync(
                Request.VerifyStaminaOverflowValueByUserIdRequest request
        ) =>
            new VerifyStaminaOverflowValueByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.VerifyStaminaOverflowValueByUserIdResult>();
    #else
		public VerifyStaminaOverflowValueByUserIdTask VerifyStaminaOverflowValueByUserIdAsync(
                Request.VerifyStaminaOverflowValueByUserIdRequest request
        )
		{
			return new VerifyStaminaOverflowValueByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.VerifyStaminaOverflowValueByUserIdResult> VerifyStaminaOverflowValueByUserIdAsync(
                Request.VerifyStaminaOverflowValueByUserIdRequest request
        ) =>
            new VerifyStaminaOverflowValueByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class RecoverStaminaByStampSheetTask : Gs2RestSessionTask<RecoverStaminaByStampSheetRequest, RecoverStaminaByStampSheetResult>
        {
            public RecoverStaminaByStampSheetTask(IGs2Session session, RestSessionRequestFactory factory, RecoverStaminaByStampSheetRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(RecoverStaminaByStampSheetRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "stamina")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/stamina/recover";

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
		public IEnumerator RecoverStaminaByStampSheet(
                Request.RecoverStaminaByStampSheetRequest request,
                UnityAction<AsyncResult<Result.RecoverStaminaByStampSheetResult>> callback
        ) =>
            new RecoverStaminaByStampSheetTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.RecoverStaminaByStampSheetResult> RecoverStaminaByStampSheetFuture(
                Request.RecoverStaminaByStampSheetRequest request
        ) =>
            new RecoverStaminaByStampSheetTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.RecoverStaminaByStampSheetResult> RecoverStaminaByStampSheetAsync(
                Request.RecoverStaminaByStampSheetRequest request
        ) =>
            new RecoverStaminaByStampSheetTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.RecoverStaminaByStampSheetResult>();
    #else
		public RecoverStaminaByStampSheetTask RecoverStaminaByStampSheetAsync(
                Request.RecoverStaminaByStampSheetRequest request
        )
		{
			return new RecoverStaminaByStampSheetTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.RecoverStaminaByStampSheetResult> RecoverStaminaByStampSheetAsync(
                Request.RecoverStaminaByStampSheetRequest request
        ) =>
            new RecoverStaminaByStampSheetTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class RaiseMaxValueByStampSheetTask : Gs2RestSessionTask<RaiseMaxValueByStampSheetRequest, RaiseMaxValueByStampSheetResult>
        {
            public RaiseMaxValueByStampSheetTask(IGs2Session session, RestSessionRequestFactory factory, RaiseMaxValueByStampSheetRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(RaiseMaxValueByStampSheetRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "stamina")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/stamina/raise";

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
		public IEnumerator RaiseMaxValueByStampSheet(
                Request.RaiseMaxValueByStampSheetRequest request,
                UnityAction<AsyncResult<Result.RaiseMaxValueByStampSheetResult>> callback
        ) =>
            new RaiseMaxValueByStampSheetTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.RaiseMaxValueByStampSheetResult> RaiseMaxValueByStampSheetFuture(
                Request.RaiseMaxValueByStampSheetRequest request
        ) =>
            new RaiseMaxValueByStampSheetTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.RaiseMaxValueByStampSheetResult> RaiseMaxValueByStampSheetAsync(
                Request.RaiseMaxValueByStampSheetRequest request
        ) =>
            new RaiseMaxValueByStampSheetTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.RaiseMaxValueByStampSheetResult>();
    #else
		public RaiseMaxValueByStampSheetTask RaiseMaxValueByStampSheetAsync(
                Request.RaiseMaxValueByStampSheetRequest request
        )
		{
			return new RaiseMaxValueByStampSheetTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.RaiseMaxValueByStampSheetResult> RaiseMaxValueByStampSheetAsync(
                Request.RaiseMaxValueByStampSheetRequest request
        ) =>
            new RaiseMaxValueByStampSheetTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class DecreaseMaxValueByStampTaskTask : Gs2RestSessionTask<DecreaseMaxValueByStampTaskRequest, DecreaseMaxValueByStampTaskResult>
        {
            public DecreaseMaxValueByStampTaskTask(IGs2Session session, RestSessionRequestFactory factory, DecreaseMaxValueByStampTaskRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DecreaseMaxValueByStampTaskRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "stamina")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/stamina/decrease";

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
		public IEnumerator DecreaseMaxValueByStampTask(
                Request.DecreaseMaxValueByStampTaskRequest request,
                UnityAction<AsyncResult<Result.DecreaseMaxValueByStampTaskResult>> callback
        ) =>
            new DecreaseMaxValueByStampTaskTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.DecreaseMaxValueByStampTaskResult> DecreaseMaxValueByStampTaskFuture(
                Request.DecreaseMaxValueByStampTaskRequest request
        ) =>
            new DecreaseMaxValueByStampTaskTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.DecreaseMaxValueByStampTaskResult> DecreaseMaxValueByStampTaskAsync(
                Request.DecreaseMaxValueByStampTaskRequest request
        ) =>
            new DecreaseMaxValueByStampTaskTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.DecreaseMaxValueByStampTaskResult>();
    #else
		public DecreaseMaxValueByStampTaskTask DecreaseMaxValueByStampTaskAsync(
                Request.DecreaseMaxValueByStampTaskRequest request
        )
		{
			return new DecreaseMaxValueByStampTaskTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.DecreaseMaxValueByStampTaskResult> DecreaseMaxValueByStampTaskAsync(
                Request.DecreaseMaxValueByStampTaskRequest request
        ) =>
            new DecreaseMaxValueByStampTaskTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class SetMaxValueByStampSheetTask : Gs2RestSessionTask<SetMaxValueByStampSheetRequest, SetMaxValueByStampSheetResult>
        {
            public SetMaxValueByStampSheetTask(IGs2Session session, RestSessionRequestFactory factory, SetMaxValueByStampSheetRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(SetMaxValueByStampSheetRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "stamina")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/stamina/max/set";

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
		public IEnumerator SetMaxValueByStampSheet(
                Request.SetMaxValueByStampSheetRequest request,
                UnityAction<AsyncResult<Result.SetMaxValueByStampSheetResult>> callback
        ) =>
            new SetMaxValueByStampSheetTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.SetMaxValueByStampSheetResult> SetMaxValueByStampSheetFuture(
                Request.SetMaxValueByStampSheetRequest request
        ) =>
            new SetMaxValueByStampSheetTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.SetMaxValueByStampSheetResult> SetMaxValueByStampSheetAsync(
                Request.SetMaxValueByStampSheetRequest request
        ) =>
            new SetMaxValueByStampSheetTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.SetMaxValueByStampSheetResult>();
    #else
		public SetMaxValueByStampSheetTask SetMaxValueByStampSheetAsync(
                Request.SetMaxValueByStampSheetRequest request
        )
		{
			return new SetMaxValueByStampSheetTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.SetMaxValueByStampSheetResult> SetMaxValueByStampSheetAsync(
                Request.SetMaxValueByStampSheetRequest request
        ) =>
            new SetMaxValueByStampSheetTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class SetRecoverIntervalByStampSheetTask : Gs2RestSessionTask<SetRecoverIntervalByStampSheetRequest, SetRecoverIntervalByStampSheetResult>
        {
            public SetRecoverIntervalByStampSheetTask(IGs2Session session, RestSessionRequestFactory factory, SetRecoverIntervalByStampSheetRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(SetRecoverIntervalByStampSheetRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "stamina")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/stamina/recoverInterval/set";

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
		public IEnumerator SetRecoverIntervalByStampSheet(
                Request.SetRecoverIntervalByStampSheetRequest request,
                UnityAction<AsyncResult<Result.SetRecoverIntervalByStampSheetResult>> callback
        ) =>
            new SetRecoverIntervalByStampSheetTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.SetRecoverIntervalByStampSheetResult> SetRecoverIntervalByStampSheetFuture(
                Request.SetRecoverIntervalByStampSheetRequest request
        ) =>
            new SetRecoverIntervalByStampSheetTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.SetRecoverIntervalByStampSheetResult> SetRecoverIntervalByStampSheetAsync(
                Request.SetRecoverIntervalByStampSheetRequest request
        ) =>
            new SetRecoverIntervalByStampSheetTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.SetRecoverIntervalByStampSheetResult>();
    #else
		public SetRecoverIntervalByStampSheetTask SetRecoverIntervalByStampSheetAsync(
                Request.SetRecoverIntervalByStampSheetRequest request
        )
		{
			return new SetRecoverIntervalByStampSheetTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.SetRecoverIntervalByStampSheetResult> SetRecoverIntervalByStampSheetAsync(
                Request.SetRecoverIntervalByStampSheetRequest request
        ) =>
            new SetRecoverIntervalByStampSheetTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class SetRecoverValueByStampSheetTask : Gs2RestSessionTask<SetRecoverValueByStampSheetRequest, SetRecoverValueByStampSheetResult>
        {
            public SetRecoverValueByStampSheetTask(IGs2Session session, RestSessionRequestFactory factory, SetRecoverValueByStampSheetRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(SetRecoverValueByStampSheetRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "stamina")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/stamina/recoverValue/set";

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
		public IEnumerator SetRecoverValueByStampSheet(
                Request.SetRecoverValueByStampSheetRequest request,
                UnityAction<AsyncResult<Result.SetRecoverValueByStampSheetResult>> callback
        ) =>
            new SetRecoverValueByStampSheetTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.SetRecoverValueByStampSheetResult> SetRecoverValueByStampSheetFuture(
                Request.SetRecoverValueByStampSheetRequest request
        ) =>
            new SetRecoverValueByStampSheetTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.SetRecoverValueByStampSheetResult> SetRecoverValueByStampSheetAsync(
                Request.SetRecoverValueByStampSheetRequest request
        ) =>
            new SetRecoverValueByStampSheetTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.SetRecoverValueByStampSheetResult>();
    #else
		public SetRecoverValueByStampSheetTask SetRecoverValueByStampSheetAsync(
                Request.SetRecoverValueByStampSheetRequest request
        )
		{
			return new SetRecoverValueByStampSheetTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.SetRecoverValueByStampSheetResult> SetRecoverValueByStampSheetAsync(
                Request.SetRecoverValueByStampSheetRequest request
        ) =>
            new SetRecoverValueByStampSheetTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class ConsumeStaminaByStampTaskTask : Gs2RestSessionTask<ConsumeStaminaByStampTaskRequest, ConsumeStaminaByStampTaskResult>
        {
            public ConsumeStaminaByStampTaskTask(IGs2Session session, RestSessionRequestFactory factory, ConsumeStaminaByStampTaskRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(ConsumeStaminaByStampTaskRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "stamina")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/stamina/consume";

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
		public IEnumerator ConsumeStaminaByStampTask(
                Request.ConsumeStaminaByStampTaskRequest request,
                UnityAction<AsyncResult<Result.ConsumeStaminaByStampTaskResult>> callback
        ) =>
            new ConsumeStaminaByStampTaskTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.ConsumeStaminaByStampTaskResult> ConsumeStaminaByStampTaskFuture(
                Request.ConsumeStaminaByStampTaskRequest request
        ) =>
            new ConsumeStaminaByStampTaskTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.ConsumeStaminaByStampTaskResult> ConsumeStaminaByStampTaskAsync(
                Request.ConsumeStaminaByStampTaskRequest request
        ) =>
            new ConsumeStaminaByStampTaskTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.ConsumeStaminaByStampTaskResult>();
    #else
		public ConsumeStaminaByStampTaskTask ConsumeStaminaByStampTaskAsync(
                Request.ConsumeStaminaByStampTaskRequest request
        )
		{
			return new ConsumeStaminaByStampTaskTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.ConsumeStaminaByStampTaskResult> ConsumeStaminaByStampTaskAsync(
                Request.ConsumeStaminaByStampTaskRequest request
        ) =>
            new ConsumeStaminaByStampTaskTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class VerifyStaminaValueByStampTaskTask : Gs2RestSessionTask<VerifyStaminaValueByStampTaskRequest, VerifyStaminaValueByStampTaskResult>
        {
            public VerifyStaminaValueByStampTaskTask(IGs2Session session, RestSessionRequestFactory factory, VerifyStaminaValueByStampTaskRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(VerifyStaminaValueByStampTaskRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "stamina")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/stamina/value/verify";

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
		public IEnumerator VerifyStaminaValueByStampTask(
                Request.VerifyStaminaValueByStampTaskRequest request,
                UnityAction<AsyncResult<Result.VerifyStaminaValueByStampTaskResult>> callback
        ) =>
            new VerifyStaminaValueByStampTaskTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.VerifyStaminaValueByStampTaskResult> VerifyStaminaValueByStampTaskFuture(
                Request.VerifyStaminaValueByStampTaskRequest request
        ) =>
            new VerifyStaminaValueByStampTaskTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.VerifyStaminaValueByStampTaskResult> VerifyStaminaValueByStampTaskAsync(
                Request.VerifyStaminaValueByStampTaskRequest request
        ) =>
            new VerifyStaminaValueByStampTaskTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.VerifyStaminaValueByStampTaskResult>();
    #else
		public VerifyStaminaValueByStampTaskTask VerifyStaminaValueByStampTaskAsync(
                Request.VerifyStaminaValueByStampTaskRequest request
        )
		{
			return new VerifyStaminaValueByStampTaskTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.VerifyStaminaValueByStampTaskResult> VerifyStaminaValueByStampTaskAsync(
                Request.VerifyStaminaValueByStampTaskRequest request
        ) =>
            new VerifyStaminaValueByStampTaskTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class VerifyStaminaMaxValueByStampTaskTask : Gs2RestSessionTask<VerifyStaminaMaxValueByStampTaskRequest, VerifyStaminaMaxValueByStampTaskResult>
        {
            public VerifyStaminaMaxValueByStampTaskTask(IGs2Session session, RestSessionRequestFactory factory, VerifyStaminaMaxValueByStampTaskRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(VerifyStaminaMaxValueByStampTaskRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "stamina")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/stamina/max/verify";

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
		public IEnumerator VerifyStaminaMaxValueByStampTask(
                Request.VerifyStaminaMaxValueByStampTaskRequest request,
                UnityAction<AsyncResult<Result.VerifyStaminaMaxValueByStampTaskResult>> callback
        ) =>
            new VerifyStaminaMaxValueByStampTaskTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.VerifyStaminaMaxValueByStampTaskResult> VerifyStaminaMaxValueByStampTaskFuture(
                Request.VerifyStaminaMaxValueByStampTaskRequest request
        ) =>
            new VerifyStaminaMaxValueByStampTaskTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.VerifyStaminaMaxValueByStampTaskResult> VerifyStaminaMaxValueByStampTaskAsync(
                Request.VerifyStaminaMaxValueByStampTaskRequest request
        ) =>
            new VerifyStaminaMaxValueByStampTaskTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.VerifyStaminaMaxValueByStampTaskResult>();
    #else
		public VerifyStaminaMaxValueByStampTaskTask VerifyStaminaMaxValueByStampTaskAsync(
                Request.VerifyStaminaMaxValueByStampTaskRequest request
        )
		{
			return new VerifyStaminaMaxValueByStampTaskTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.VerifyStaminaMaxValueByStampTaskResult> VerifyStaminaMaxValueByStampTaskAsync(
                Request.VerifyStaminaMaxValueByStampTaskRequest request
        ) =>
            new VerifyStaminaMaxValueByStampTaskTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class VerifyStaminaRecoverIntervalMinutesByStampTaskTask : Gs2RestSessionTask<VerifyStaminaRecoverIntervalMinutesByStampTaskRequest, VerifyStaminaRecoverIntervalMinutesByStampTaskResult>
        {
            public VerifyStaminaRecoverIntervalMinutesByStampTaskTask(IGs2Session session, RestSessionRequestFactory factory, VerifyStaminaRecoverIntervalMinutesByStampTaskRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(VerifyStaminaRecoverIntervalMinutesByStampTaskRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "stamina")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/stamina/recover/interval/verify";

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
		public IEnumerator VerifyStaminaRecoverIntervalMinutesByStampTask(
                Request.VerifyStaminaRecoverIntervalMinutesByStampTaskRequest request,
                UnityAction<AsyncResult<Result.VerifyStaminaRecoverIntervalMinutesByStampTaskResult>> callback
        ) =>
            new VerifyStaminaRecoverIntervalMinutesByStampTaskTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.VerifyStaminaRecoverIntervalMinutesByStampTaskResult> VerifyStaminaRecoverIntervalMinutesByStampTaskFuture(
                Request.VerifyStaminaRecoverIntervalMinutesByStampTaskRequest request
        ) =>
            new VerifyStaminaRecoverIntervalMinutesByStampTaskTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.VerifyStaminaRecoverIntervalMinutesByStampTaskResult> VerifyStaminaRecoverIntervalMinutesByStampTaskAsync(
                Request.VerifyStaminaRecoverIntervalMinutesByStampTaskRequest request
        ) =>
            new VerifyStaminaRecoverIntervalMinutesByStampTaskTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.VerifyStaminaRecoverIntervalMinutesByStampTaskResult>();
    #else
		public VerifyStaminaRecoverIntervalMinutesByStampTaskTask VerifyStaminaRecoverIntervalMinutesByStampTaskAsync(
                Request.VerifyStaminaRecoverIntervalMinutesByStampTaskRequest request
        )
		{
			return new VerifyStaminaRecoverIntervalMinutesByStampTaskTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.VerifyStaminaRecoverIntervalMinutesByStampTaskResult> VerifyStaminaRecoverIntervalMinutesByStampTaskAsync(
                Request.VerifyStaminaRecoverIntervalMinutesByStampTaskRequest request
        ) =>
            new VerifyStaminaRecoverIntervalMinutesByStampTaskTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class VerifyStaminaRecoverValueByStampTaskTask : Gs2RestSessionTask<VerifyStaminaRecoverValueByStampTaskRequest, VerifyStaminaRecoverValueByStampTaskResult>
        {
            public VerifyStaminaRecoverValueByStampTaskTask(IGs2Session session, RestSessionRequestFactory factory, VerifyStaminaRecoverValueByStampTaskRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(VerifyStaminaRecoverValueByStampTaskRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "stamina")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/stamina/recover/value/verify";

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
		public IEnumerator VerifyStaminaRecoverValueByStampTask(
                Request.VerifyStaminaRecoverValueByStampTaskRequest request,
                UnityAction<AsyncResult<Result.VerifyStaminaRecoverValueByStampTaskResult>> callback
        ) =>
            new VerifyStaminaRecoverValueByStampTaskTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.VerifyStaminaRecoverValueByStampTaskResult> VerifyStaminaRecoverValueByStampTaskFuture(
                Request.VerifyStaminaRecoverValueByStampTaskRequest request
        ) =>
            new VerifyStaminaRecoverValueByStampTaskTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.VerifyStaminaRecoverValueByStampTaskResult> VerifyStaminaRecoverValueByStampTaskAsync(
                Request.VerifyStaminaRecoverValueByStampTaskRequest request
        ) =>
            new VerifyStaminaRecoverValueByStampTaskTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.VerifyStaminaRecoverValueByStampTaskResult>();
    #else
		public VerifyStaminaRecoverValueByStampTaskTask VerifyStaminaRecoverValueByStampTaskAsync(
                Request.VerifyStaminaRecoverValueByStampTaskRequest request
        )
		{
			return new VerifyStaminaRecoverValueByStampTaskTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.VerifyStaminaRecoverValueByStampTaskResult> VerifyStaminaRecoverValueByStampTaskAsync(
                Request.VerifyStaminaRecoverValueByStampTaskRequest request
        ) =>
            new VerifyStaminaRecoverValueByStampTaskTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class VerifyStaminaOverflowValueByStampTaskTask : Gs2RestSessionTask<VerifyStaminaOverflowValueByStampTaskRequest, VerifyStaminaOverflowValueByStampTaskResult>
        {
            public VerifyStaminaOverflowValueByStampTaskTask(IGs2Session session, RestSessionRequestFactory factory, VerifyStaminaOverflowValueByStampTaskRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(VerifyStaminaOverflowValueByStampTaskRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "stamina")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/stamina/overflow/value/verify";

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
		public IEnumerator VerifyStaminaOverflowValueByStampTask(
                Request.VerifyStaminaOverflowValueByStampTaskRequest request,
                UnityAction<AsyncResult<Result.VerifyStaminaOverflowValueByStampTaskResult>> callback
        ) =>
            new VerifyStaminaOverflowValueByStampTaskTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.VerifyStaminaOverflowValueByStampTaskResult> VerifyStaminaOverflowValueByStampTaskFuture(
                Request.VerifyStaminaOverflowValueByStampTaskRequest request
        ) =>
            new VerifyStaminaOverflowValueByStampTaskTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.VerifyStaminaOverflowValueByStampTaskResult> VerifyStaminaOverflowValueByStampTaskAsync(
                Request.VerifyStaminaOverflowValueByStampTaskRequest request
        ) =>
            new VerifyStaminaOverflowValueByStampTaskTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.VerifyStaminaOverflowValueByStampTaskResult>();
    #else
		public VerifyStaminaOverflowValueByStampTaskTask VerifyStaminaOverflowValueByStampTaskAsync(
                Request.VerifyStaminaOverflowValueByStampTaskRequest request
        )
		{
			return new VerifyStaminaOverflowValueByStampTaskTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.VerifyStaminaOverflowValueByStampTaskResult> VerifyStaminaOverflowValueByStampTaskAsync(
                Request.VerifyStaminaOverflowValueByStampTaskRequest request
        ) =>
            new VerifyStaminaOverflowValueByStampTaskTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif
	}
}