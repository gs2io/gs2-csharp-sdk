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
using Gs2.Gs2Enchant.Request;
using Gs2.Gs2Enchant.Result;
using Gs2.Util.LitJson;

namespace Gs2.Gs2Enchant
{
	public class Gs2EnchantRestClient : AbstractGs2Client
	{
#if UNITY_2017_1_OR_NEWER
		private readonly CertificateHandler _certificateHandler;
#endif

		public static string Endpoint = "enchant";

        protected Gs2RestSession Gs2RestSession => (Gs2RestSession) Gs2Session;

		public Gs2EnchantRestClient(Gs2RestSession Gs2RestSession) : base(Gs2RestSession)
		{

		}

#if UNITY_2017_1_OR_NEWER
		public Gs2EnchantRestClient(Gs2RestSession gs2RestSession, CertificateHandler certificateHandler) : base(gs2RestSession)
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
                    .Replace("{service}", "enchant")
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
                    .Replace("{service}", "enchant")
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
                    .Replace("{service}", "enchant")
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
                    .Replace("{service}", "enchant")
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
                    .Replace("{service}", "enchant")
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
                    .Replace("{service}", "enchant")
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
                    .Replace("{service}", "enchant")
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
                    .Replace("{service}", "enchant")
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
                    .Replace("{service}", "enchant")
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
                    .Replace("{service}", "enchant")
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
                    .Replace("{service}", "enchant")
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
                    .Replace("{service}", "enchant")
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
                    .Replace("{service}", "enchant")
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
                    .Replace("{service}", "enchant")
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


        public class DescribeBalanceParameterModelsTask : Gs2RestSessionTask<DescribeBalanceParameterModelsRequest, DescribeBalanceParameterModelsResult>
        {
            public DescribeBalanceParameterModelsTask(IGs2Session session, RestSessionRequestFactory factory, DescribeBalanceParameterModelsRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DescribeBalanceParameterModelsRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "enchant")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/model/balance";

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
		public IEnumerator DescribeBalanceParameterModels(
                Request.DescribeBalanceParameterModelsRequest request,
                UnityAction<AsyncResult<Result.DescribeBalanceParameterModelsResult>> callback
        ) =>
            new DescribeBalanceParameterModelsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.DescribeBalanceParameterModelsResult> DescribeBalanceParameterModelsFuture(
                Request.DescribeBalanceParameterModelsRequest request
        ) =>
            new DescribeBalanceParameterModelsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.DescribeBalanceParameterModelsResult> DescribeBalanceParameterModelsAsync(
                Request.DescribeBalanceParameterModelsRequest request
        ) =>
            new DescribeBalanceParameterModelsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.DescribeBalanceParameterModelsResult>();
    #else
		public DescribeBalanceParameterModelsTask DescribeBalanceParameterModelsAsync(
                Request.DescribeBalanceParameterModelsRequest request
        )
		{
			return new DescribeBalanceParameterModelsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.DescribeBalanceParameterModelsResult> DescribeBalanceParameterModelsAsync(
                Request.DescribeBalanceParameterModelsRequest request
        ) =>
            new DescribeBalanceParameterModelsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class GetBalanceParameterModelTask : Gs2RestSessionTask<GetBalanceParameterModelRequest, GetBalanceParameterModelResult>
        {
            public GetBalanceParameterModelTask(IGs2Session session, RestSessionRequestFactory factory, GetBalanceParameterModelRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(GetBalanceParameterModelRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "enchant")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/model/balance/{parameterName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{parameterName}", !string.IsNullOrEmpty(request.ParameterName) ? request.ParameterName.ToString() : "null");

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
		public IEnumerator GetBalanceParameterModel(
                Request.GetBalanceParameterModelRequest request,
                UnityAction<AsyncResult<Result.GetBalanceParameterModelResult>> callback
        ) =>
            new GetBalanceParameterModelTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.GetBalanceParameterModelResult> GetBalanceParameterModelFuture(
                Request.GetBalanceParameterModelRequest request
        ) =>
            new GetBalanceParameterModelTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.GetBalanceParameterModelResult> GetBalanceParameterModelAsync(
                Request.GetBalanceParameterModelRequest request
        ) =>
            new GetBalanceParameterModelTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.GetBalanceParameterModelResult>();
    #else
		public GetBalanceParameterModelTask GetBalanceParameterModelAsync(
                Request.GetBalanceParameterModelRequest request
        )
		{
			return new GetBalanceParameterModelTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.GetBalanceParameterModelResult> GetBalanceParameterModelAsync(
                Request.GetBalanceParameterModelRequest request
        ) =>
            new GetBalanceParameterModelTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class DescribeBalanceParameterModelMastersTask : Gs2RestSessionTask<DescribeBalanceParameterModelMastersRequest, DescribeBalanceParameterModelMastersResult>
        {
            public DescribeBalanceParameterModelMastersTask(IGs2Session session, RestSessionRequestFactory factory, DescribeBalanceParameterModelMastersRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DescribeBalanceParameterModelMastersRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "enchant")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/master/model/balance";

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
		public IEnumerator DescribeBalanceParameterModelMasters(
                Request.DescribeBalanceParameterModelMastersRequest request,
                UnityAction<AsyncResult<Result.DescribeBalanceParameterModelMastersResult>> callback
        ) =>
            new DescribeBalanceParameterModelMastersTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.DescribeBalanceParameterModelMastersResult> DescribeBalanceParameterModelMastersFuture(
                Request.DescribeBalanceParameterModelMastersRequest request
        ) =>
            new DescribeBalanceParameterModelMastersTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.DescribeBalanceParameterModelMastersResult> DescribeBalanceParameterModelMastersAsync(
                Request.DescribeBalanceParameterModelMastersRequest request
        ) =>
            new DescribeBalanceParameterModelMastersTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.DescribeBalanceParameterModelMastersResult>();
    #else
		public DescribeBalanceParameterModelMastersTask DescribeBalanceParameterModelMastersAsync(
                Request.DescribeBalanceParameterModelMastersRequest request
        )
		{
			return new DescribeBalanceParameterModelMastersTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.DescribeBalanceParameterModelMastersResult> DescribeBalanceParameterModelMastersAsync(
                Request.DescribeBalanceParameterModelMastersRequest request
        ) =>
            new DescribeBalanceParameterModelMastersTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class CreateBalanceParameterModelMasterTask : Gs2RestSessionTask<CreateBalanceParameterModelMasterRequest, CreateBalanceParameterModelMasterResult>
        {
            public CreateBalanceParameterModelMasterTask(IGs2Session session, RestSessionRequestFactory factory, CreateBalanceParameterModelMasterRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(CreateBalanceParameterModelMasterRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "enchant")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/master/model/balance";

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
                if (request.TotalValue != null)
                {
                    jsonWriter.WritePropertyName("totalValue");
                    jsonWriter.Write(request.TotalValue.ToString());
                }
                if (request.InitialValueStrategy != null)
                {
                    jsonWriter.WritePropertyName("initialValueStrategy");
                    jsonWriter.Write(request.InitialValueStrategy);
                }
                if (request.Parameters != null)
                {
                    jsonWriter.WritePropertyName("parameters");
                    jsonWriter.WriteArrayStart();
                    foreach(var item in request.Parameters)
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
		public IEnumerator CreateBalanceParameterModelMaster(
                Request.CreateBalanceParameterModelMasterRequest request,
                UnityAction<AsyncResult<Result.CreateBalanceParameterModelMasterResult>> callback
        ) =>
            new CreateBalanceParameterModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.CreateBalanceParameterModelMasterResult> CreateBalanceParameterModelMasterFuture(
                Request.CreateBalanceParameterModelMasterRequest request
        ) =>
            new CreateBalanceParameterModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.CreateBalanceParameterModelMasterResult> CreateBalanceParameterModelMasterAsync(
                Request.CreateBalanceParameterModelMasterRequest request
        ) =>
            new CreateBalanceParameterModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.CreateBalanceParameterModelMasterResult>();
    #else
		public CreateBalanceParameterModelMasterTask CreateBalanceParameterModelMasterAsync(
                Request.CreateBalanceParameterModelMasterRequest request
        )
		{
			return new CreateBalanceParameterModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.CreateBalanceParameterModelMasterResult> CreateBalanceParameterModelMasterAsync(
                Request.CreateBalanceParameterModelMasterRequest request
        ) =>
            new CreateBalanceParameterModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class GetBalanceParameterModelMasterTask : Gs2RestSessionTask<GetBalanceParameterModelMasterRequest, GetBalanceParameterModelMasterResult>
        {
            public GetBalanceParameterModelMasterTask(IGs2Session session, RestSessionRequestFactory factory, GetBalanceParameterModelMasterRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(GetBalanceParameterModelMasterRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "enchant")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/master/model/balance/{parameterName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{parameterName}", !string.IsNullOrEmpty(request.ParameterName) ? request.ParameterName.ToString() : "null");

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
		public IEnumerator GetBalanceParameterModelMaster(
                Request.GetBalanceParameterModelMasterRequest request,
                UnityAction<AsyncResult<Result.GetBalanceParameterModelMasterResult>> callback
        ) =>
            new GetBalanceParameterModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.GetBalanceParameterModelMasterResult> GetBalanceParameterModelMasterFuture(
                Request.GetBalanceParameterModelMasterRequest request
        ) =>
            new GetBalanceParameterModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.GetBalanceParameterModelMasterResult> GetBalanceParameterModelMasterAsync(
                Request.GetBalanceParameterModelMasterRequest request
        ) =>
            new GetBalanceParameterModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.GetBalanceParameterModelMasterResult>();
    #else
		public GetBalanceParameterModelMasterTask GetBalanceParameterModelMasterAsync(
                Request.GetBalanceParameterModelMasterRequest request
        )
		{
			return new GetBalanceParameterModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.GetBalanceParameterModelMasterResult> GetBalanceParameterModelMasterAsync(
                Request.GetBalanceParameterModelMasterRequest request
        ) =>
            new GetBalanceParameterModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class UpdateBalanceParameterModelMasterTask : Gs2RestSessionTask<UpdateBalanceParameterModelMasterRequest, UpdateBalanceParameterModelMasterResult>
        {
            public UpdateBalanceParameterModelMasterTask(IGs2Session session, RestSessionRequestFactory factory, UpdateBalanceParameterModelMasterRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(UpdateBalanceParameterModelMasterRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "enchant")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/master/model/balance/{parameterName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{parameterName}", !string.IsNullOrEmpty(request.ParameterName) ? request.ParameterName.ToString() : "null");

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
                if (request.TotalValue != null)
                {
                    jsonWriter.WritePropertyName("totalValue");
                    jsonWriter.Write(request.TotalValue.ToString());
                }
                if (request.InitialValueStrategy != null)
                {
                    jsonWriter.WritePropertyName("initialValueStrategy");
                    jsonWriter.Write(request.InitialValueStrategy);
                }
                if (request.Parameters != null)
                {
                    jsonWriter.WritePropertyName("parameters");
                    jsonWriter.WriteArrayStart();
                    foreach(var item in request.Parameters)
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
		public IEnumerator UpdateBalanceParameterModelMaster(
                Request.UpdateBalanceParameterModelMasterRequest request,
                UnityAction<AsyncResult<Result.UpdateBalanceParameterModelMasterResult>> callback
        ) =>
            new UpdateBalanceParameterModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.UpdateBalanceParameterModelMasterResult> UpdateBalanceParameterModelMasterFuture(
                Request.UpdateBalanceParameterModelMasterRequest request
        ) =>
            new UpdateBalanceParameterModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.UpdateBalanceParameterModelMasterResult> UpdateBalanceParameterModelMasterAsync(
                Request.UpdateBalanceParameterModelMasterRequest request
        ) =>
            new UpdateBalanceParameterModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.UpdateBalanceParameterModelMasterResult>();
    #else
		public UpdateBalanceParameterModelMasterTask UpdateBalanceParameterModelMasterAsync(
                Request.UpdateBalanceParameterModelMasterRequest request
        )
		{
			return new UpdateBalanceParameterModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.UpdateBalanceParameterModelMasterResult> UpdateBalanceParameterModelMasterAsync(
                Request.UpdateBalanceParameterModelMasterRequest request
        ) =>
            new UpdateBalanceParameterModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class DeleteBalanceParameterModelMasterTask : Gs2RestSessionTask<DeleteBalanceParameterModelMasterRequest, DeleteBalanceParameterModelMasterResult>
        {
            public DeleteBalanceParameterModelMasterTask(IGs2Session session, RestSessionRequestFactory factory, DeleteBalanceParameterModelMasterRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DeleteBalanceParameterModelMasterRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "enchant")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/master/model/balance/{parameterName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{parameterName}", !string.IsNullOrEmpty(request.ParameterName) ? request.ParameterName.ToString() : "null");

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
		public IEnumerator DeleteBalanceParameterModelMaster(
                Request.DeleteBalanceParameterModelMasterRequest request,
                UnityAction<AsyncResult<Result.DeleteBalanceParameterModelMasterResult>> callback
        ) =>
            new DeleteBalanceParameterModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.DeleteBalanceParameterModelMasterResult> DeleteBalanceParameterModelMasterFuture(
                Request.DeleteBalanceParameterModelMasterRequest request
        ) =>
            new DeleteBalanceParameterModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.DeleteBalanceParameterModelMasterResult> DeleteBalanceParameterModelMasterAsync(
                Request.DeleteBalanceParameterModelMasterRequest request
        ) =>
            new DeleteBalanceParameterModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.DeleteBalanceParameterModelMasterResult>();
    #else
		public DeleteBalanceParameterModelMasterTask DeleteBalanceParameterModelMasterAsync(
                Request.DeleteBalanceParameterModelMasterRequest request
        )
		{
			return new DeleteBalanceParameterModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.DeleteBalanceParameterModelMasterResult> DeleteBalanceParameterModelMasterAsync(
                Request.DeleteBalanceParameterModelMasterRequest request
        ) =>
            new DeleteBalanceParameterModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class DescribeRarityParameterModelsTask : Gs2RestSessionTask<DescribeRarityParameterModelsRequest, DescribeRarityParameterModelsResult>
        {
            public DescribeRarityParameterModelsTask(IGs2Session session, RestSessionRequestFactory factory, DescribeRarityParameterModelsRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DescribeRarityParameterModelsRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "enchant")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/model/rarity";

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
		public IEnumerator DescribeRarityParameterModels(
                Request.DescribeRarityParameterModelsRequest request,
                UnityAction<AsyncResult<Result.DescribeRarityParameterModelsResult>> callback
        ) =>
            new DescribeRarityParameterModelsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.DescribeRarityParameterModelsResult> DescribeRarityParameterModelsFuture(
                Request.DescribeRarityParameterModelsRequest request
        ) =>
            new DescribeRarityParameterModelsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.DescribeRarityParameterModelsResult> DescribeRarityParameterModelsAsync(
                Request.DescribeRarityParameterModelsRequest request
        ) =>
            new DescribeRarityParameterModelsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.DescribeRarityParameterModelsResult>();
    #else
		public DescribeRarityParameterModelsTask DescribeRarityParameterModelsAsync(
                Request.DescribeRarityParameterModelsRequest request
        )
		{
			return new DescribeRarityParameterModelsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.DescribeRarityParameterModelsResult> DescribeRarityParameterModelsAsync(
                Request.DescribeRarityParameterModelsRequest request
        ) =>
            new DescribeRarityParameterModelsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class GetRarityParameterModelTask : Gs2RestSessionTask<GetRarityParameterModelRequest, GetRarityParameterModelResult>
        {
            public GetRarityParameterModelTask(IGs2Session session, RestSessionRequestFactory factory, GetRarityParameterModelRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(GetRarityParameterModelRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "enchant")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/model/rarity/{parameterName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{parameterName}", !string.IsNullOrEmpty(request.ParameterName) ? request.ParameterName.ToString() : "null");

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
		public IEnumerator GetRarityParameterModel(
                Request.GetRarityParameterModelRequest request,
                UnityAction<AsyncResult<Result.GetRarityParameterModelResult>> callback
        ) =>
            new GetRarityParameterModelTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.GetRarityParameterModelResult> GetRarityParameterModelFuture(
                Request.GetRarityParameterModelRequest request
        ) =>
            new GetRarityParameterModelTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.GetRarityParameterModelResult> GetRarityParameterModelAsync(
                Request.GetRarityParameterModelRequest request
        ) =>
            new GetRarityParameterModelTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.GetRarityParameterModelResult>();
    #else
		public GetRarityParameterModelTask GetRarityParameterModelAsync(
                Request.GetRarityParameterModelRequest request
        )
		{
			return new GetRarityParameterModelTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.GetRarityParameterModelResult> GetRarityParameterModelAsync(
                Request.GetRarityParameterModelRequest request
        ) =>
            new GetRarityParameterModelTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class DescribeRarityParameterModelMastersTask : Gs2RestSessionTask<DescribeRarityParameterModelMastersRequest, DescribeRarityParameterModelMastersResult>
        {
            public DescribeRarityParameterModelMastersTask(IGs2Session session, RestSessionRequestFactory factory, DescribeRarityParameterModelMastersRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DescribeRarityParameterModelMastersRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "enchant")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/master/model/rarity";

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
		public IEnumerator DescribeRarityParameterModelMasters(
                Request.DescribeRarityParameterModelMastersRequest request,
                UnityAction<AsyncResult<Result.DescribeRarityParameterModelMastersResult>> callback
        ) =>
            new DescribeRarityParameterModelMastersTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.DescribeRarityParameterModelMastersResult> DescribeRarityParameterModelMastersFuture(
                Request.DescribeRarityParameterModelMastersRequest request
        ) =>
            new DescribeRarityParameterModelMastersTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.DescribeRarityParameterModelMastersResult> DescribeRarityParameterModelMastersAsync(
                Request.DescribeRarityParameterModelMastersRequest request
        ) =>
            new DescribeRarityParameterModelMastersTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.DescribeRarityParameterModelMastersResult>();
    #else
		public DescribeRarityParameterModelMastersTask DescribeRarityParameterModelMastersAsync(
                Request.DescribeRarityParameterModelMastersRequest request
        )
		{
			return new DescribeRarityParameterModelMastersTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.DescribeRarityParameterModelMastersResult> DescribeRarityParameterModelMastersAsync(
                Request.DescribeRarityParameterModelMastersRequest request
        ) =>
            new DescribeRarityParameterModelMastersTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class CreateRarityParameterModelMasterTask : Gs2RestSessionTask<CreateRarityParameterModelMasterRequest, CreateRarityParameterModelMasterResult>
        {
            public CreateRarityParameterModelMasterTask(IGs2Session session, RestSessionRequestFactory factory, CreateRarityParameterModelMasterRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(CreateRarityParameterModelMasterRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "enchant")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/master/model/rarity";

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
                if (request.MaximumParameterCount != null)
                {
                    jsonWriter.WritePropertyName("maximumParameterCount");
                    jsonWriter.Write(request.MaximumParameterCount.ToString());
                }
                if (request.ParameterCounts != null)
                {
                    jsonWriter.WritePropertyName("parameterCounts");
                    jsonWriter.WriteArrayStart();
                    foreach(var item in request.ParameterCounts)
                    {
                        item.WriteJson(jsonWriter);
                    }
                    jsonWriter.WriteArrayEnd();
                }
                if (request.Parameters != null)
                {
                    jsonWriter.WritePropertyName("parameters");
                    jsonWriter.WriteArrayStart();
                    foreach(var item in request.Parameters)
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
		public IEnumerator CreateRarityParameterModelMaster(
                Request.CreateRarityParameterModelMasterRequest request,
                UnityAction<AsyncResult<Result.CreateRarityParameterModelMasterResult>> callback
        ) =>
            new CreateRarityParameterModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.CreateRarityParameterModelMasterResult> CreateRarityParameterModelMasterFuture(
                Request.CreateRarityParameterModelMasterRequest request
        ) =>
            new CreateRarityParameterModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.CreateRarityParameterModelMasterResult> CreateRarityParameterModelMasterAsync(
                Request.CreateRarityParameterModelMasterRequest request
        ) =>
            new CreateRarityParameterModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.CreateRarityParameterModelMasterResult>();
    #else
		public CreateRarityParameterModelMasterTask CreateRarityParameterModelMasterAsync(
                Request.CreateRarityParameterModelMasterRequest request
        )
		{
			return new CreateRarityParameterModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.CreateRarityParameterModelMasterResult> CreateRarityParameterModelMasterAsync(
                Request.CreateRarityParameterModelMasterRequest request
        ) =>
            new CreateRarityParameterModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class GetRarityParameterModelMasterTask : Gs2RestSessionTask<GetRarityParameterModelMasterRequest, GetRarityParameterModelMasterResult>
        {
            public GetRarityParameterModelMasterTask(IGs2Session session, RestSessionRequestFactory factory, GetRarityParameterModelMasterRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(GetRarityParameterModelMasterRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "enchant")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/master/model/rarity/{parameterName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{parameterName}", !string.IsNullOrEmpty(request.ParameterName) ? request.ParameterName.ToString() : "null");

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
		public IEnumerator GetRarityParameterModelMaster(
                Request.GetRarityParameterModelMasterRequest request,
                UnityAction<AsyncResult<Result.GetRarityParameterModelMasterResult>> callback
        ) =>
            new GetRarityParameterModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.GetRarityParameterModelMasterResult> GetRarityParameterModelMasterFuture(
                Request.GetRarityParameterModelMasterRequest request
        ) =>
            new GetRarityParameterModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.GetRarityParameterModelMasterResult> GetRarityParameterModelMasterAsync(
                Request.GetRarityParameterModelMasterRequest request
        ) =>
            new GetRarityParameterModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.GetRarityParameterModelMasterResult>();
    #else
		public GetRarityParameterModelMasterTask GetRarityParameterModelMasterAsync(
                Request.GetRarityParameterModelMasterRequest request
        )
		{
			return new GetRarityParameterModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.GetRarityParameterModelMasterResult> GetRarityParameterModelMasterAsync(
                Request.GetRarityParameterModelMasterRequest request
        ) =>
            new GetRarityParameterModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class UpdateRarityParameterModelMasterTask : Gs2RestSessionTask<UpdateRarityParameterModelMasterRequest, UpdateRarityParameterModelMasterResult>
        {
            public UpdateRarityParameterModelMasterTask(IGs2Session session, RestSessionRequestFactory factory, UpdateRarityParameterModelMasterRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(UpdateRarityParameterModelMasterRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "enchant")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/master/model/rarity/{parameterName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{parameterName}", !string.IsNullOrEmpty(request.ParameterName) ? request.ParameterName.ToString() : "null");

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
                if (request.MaximumParameterCount != null)
                {
                    jsonWriter.WritePropertyName("maximumParameterCount");
                    jsonWriter.Write(request.MaximumParameterCount.ToString());
                }
                if (request.ParameterCounts != null)
                {
                    jsonWriter.WritePropertyName("parameterCounts");
                    jsonWriter.WriteArrayStart();
                    foreach(var item in request.ParameterCounts)
                    {
                        item.WriteJson(jsonWriter);
                    }
                    jsonWriter.WriteArrayEnd();
                }
                if (request.Parameters != null)
                {
                    jsonWriter.WritePropertyName("parameters");
                    jsonWriter.WriteArrayStart();
                    foreach(var item in request.Parameters)
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
		public IEnumerator UpdateRarityParameterModelMaster(
                Request.UpdateRarityParameterModelMasterRequest request,
                UnityAction<AsyncResult<Result.UpdateRarityParameterModelMasterResult>> callback
        ) =>
            new UpdateRarityParameterModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.UpdateRarityParameterModelMasterResult> UpdateRarityParameterModelMasterFuture(
                Request.UpdateRarityParameterModelMasterRequest request
        ) =>
            new UpdateRarityParameterModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.UpdateRarityParameterModelMasterResult> UpdateRarityParameterModelMasterAsync(
                Request.UpdateRarityParameterModelMasterRequest request
        ) =>
            new UpdateRarityParameterModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.UpdateRarityParameterModelMasterResult>();
    #else
		public UpdateRarityParameterModelMasterTask UpdateRarityParameterModelMasterAsync(
                Request.UpdateRarityParameterModelMasterRequest request
        )
		{
			return new UpdateRarityParameterModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.UpdateRarityParameterModelMasterResult> UpdateRarityParameterModelMasterAsync(
                Request.UpdateRarityParameterModelMasterRequest request
        ) =>
            new UpdateRarityParameterModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class DeleteRarityParameterModelMasterTask : Gs2RestSessionTask<DeleteRarityParameterModelMasterRequest, DeleteRarityParameterModelMasterResult>
        {
            public DeleteRarityParameterModelMasterTask(IGs2Session session, RestSessionRequestFactory factory, DeleteRarityParameterModelMasterRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DeleteRarityParameterModelMasterRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "enchant")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/master/model/rarity/{parameterName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{parameterName}", !string.IsNullOrEmpty(request.ParameterName) ? request.ParameterName.ToString() : "null");

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
		public IEnumerator DeleteRarityParameterModelMaster(
                Request.DeleteRarityParameterModelMasterRequest request,
                UnityAction<AsyncResult<Result.DeleteRarityParameterModelMasterResult>> callback
        ) =>
            new DeleteRarityParameterModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.DeleteRarityParameterModelMasterResult> DeleteRarityParameterModelMasterFuture(
                Request.DeleteRarityParameterModelMasterRequest request
        ) =>
            new DeleteRarityParameterModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.DeleteRarityParameterModelMasterResult> DeleteRarityParameterModelMasterAsync(
                Request.DeleteRarityParameterModelMasterRequest request
        ) =>
            new DeleteRarityParameterModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.DeleteRarityParameterModelMasterResult>();
    #else
		public DeleteRarityParameterModelMasterTask DeleteRarityParameterModelMasterAsync(
                Request.DeleteRarityParameterModelMasterRequest request
        )
		{
			return new DeleteRarityParameterModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.DeleteRarityParameterModelMasterResult> DeleteRarityParameterModelMasterAsync(
                Request.DeleteRarityParameterModelMasterRequest request
        ) =>
            new DeleteRarityParameterModelMasterTask(
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
                    .Replace("{service}", "enchant")
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


        public class GetCurrentParameterMasterTask : Gs2RestSessionTask<GetCurrentParameterMasterRequest, GetCurrentParameterMasterResult>
        {
            public GetCurrentParameterMasterTask(IGs2Session session, RestSessionRequestFactory factory, GetCurrentParameterMasterRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(GetCurrentParameterMasterRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "enchant")
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
		public IEnumerator GetCurrentParameterMaster(
                Request.GetCurrentParameterMasterRequest request,
                UnityAction<AsyncResult<Result.GetCurrentParameterMasterResult>> callback
        ) =>
            new GetCurrentParameterMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.GetCurrentParameterMasterResult> GetCurrentParameterMasterFuture(
                Request.GetCurrentParameterMasterRequest request
        ) =>
            new GetCurrentParameterMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.GetCurrentParameterMasterResult> GetCurrentParameterMasterAsync(
                Request.GetCurrentParameterMasterRequest request
        ) =>
            new GetCurrentParameterMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.GetCurrentParameterMasterResult>();
    #else
		public GetCurrentParameterMasterTask GetCurrentParameterMasterAsync(
                Request.GetCurrentParameterMasterRequest request
        )
		{
			return new GetCurrentParameterMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.GetCurrentParameterMasterResult> GetCurrentParameterMasterAsync(
                Request.GetCurrentParameterMasterRequest request
        ) =>
            new GetCurrentParameterMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class PreUpdateCurrentParameterMasterTask : Gs2RestSessionTask<PreUpdateCurrentParameterMasterRequest, PreUpdateCurrentParameterMasterResult>
        {
            public PreUpdateCurrentParameterMasterTask(IGs2Session session, RestSessionRequestFactory factory, PreUpdateCurrentParameterMasterRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(PreUpdateCurrentParameterMasterRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "enchant")
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
		public IEnumerator PreUpdateCurrentParameterMaster(
                Request.PreUpdateCurrentParameterMasterRequest request,
                UnityAction<AsyncResult<Result.PreUpdateCurrentParameterMasterResult>> callback
        ) =>
            new PreUpdateCurrentParameterMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.PreUpdateCurrentParameterMasterResult> PreUpdateCurrentParameterMasterFuture(
                Request.PreUpdateCurrentParameterMasterRequest request
        ) =>
            new PreUpdateCurrentParameterMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.PreUpdateCurrentParameterMasterResult> PreUpdateCurrentParameterMasterAsync(
                Request.PreUpdateCurrentParameterMasterRequest request
        ) =>
            new PreUpdateCurrentParameterMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.PreUpdateCurrentParameterMasterResult>();
    #else
		public PreUpdateCurrentParameterMasterTask PreUpdateCurrentParameterMasterAsync(
                Request.PreUpdateCurrentParameterMasterRequest request
        )
		{
			return new PreUpdateCurrentParameterMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.PreUpdateCurrentParameterMasterResult> PreUpdateCurrentParameterMasterAsync(
                Request.PreUpdateCurrentParameterMasterRequest request
        ) =>
            new PreUpdateCurrentParameterMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class UpdateCurrentParameterMasterTask : Gs2RestSessionTask<UpdateCurrentParameterMasterRequest, UpdateCurrentParameterMasterResult>
        {
            public UpdateCurrentParameterMasterTask(IGs2Session session, RestSessionRequestFactory factory, UpdateCurrentParameterMasterRequest request) : base(session, factory, request)
            {
            }
#if GS2_ENABLE_UNITASK
            protected override async UniTask<UpdateCurrentParameterMasterResult> InvokeImpl()
#else
            protected override async Task<UpdateCurrentParameterMasterResult> InvokeImpl()
#endif
            {
                if (Request.Settings != null) {
                    var preTask = new PreUpdateCurrentParameterMasterTask(
                        Session,
                        Factory,
                        new PreUpdateCurrentParameterMasterRequest()
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

            protected override IGs2SessionRequest CreateRequest(UpdateCurrentParameterMasterRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "enchant")
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
		public IEnumerator UpdateCurrentParameterMaster(
                Request.UpdateCurrentParameterMasterRequest request,
                UnityAction<AsyncResult<Result.UpdateCurrentParameterMasterResult>> callback
        ) =>
            new UpdateCurrentParameterMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.UpdateCurrentParameterMasterResult> UpdateCurrentParameterMasterFuture(
                Request.UpdateCurrentParameterMasterRequest request
        ) =>
            new UpdateCurrentParameterMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.UpdateCurrentParameterMasterResult> UpdateCurrentParameterMasterAsync(
                Request.UpdateCurrentParameterMasterRequest request
        ) =>
            new UpdateCurrentParameterMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.UpdateCurrentParameterMasterResult>();
    #else
		public UpdateCurrentParameterMasterTask UpdateCurrentParameterMasterAsync(
                Request.UpdateCurrentParameterMasterRequest request
        )
		{
			return new UpdateCurrentParameterMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.UpdateCurrentParameterMasterResult> UpdateCurrentParameterMasterAsync(
                Request.UpdateCurrentParameterMasterRequest request
        ) =>
            new UpdateCurrentParameterMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class UpdateCurrentParameterMasterFromGitHubTask : Gs2RestSessionTask<UpdateCurrentParameterMasterFromGitHubRequest, UpdateCurrentParameterMasterFromGitHubResult>
        {
            public UpdateCurrentParameterMasterFromGitHubTask(IGs2Session session, RestSessionRequestFactory factory, UpdateCurrentParameterMasterFromGitHubRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(UpdateCurrentParameterMasterFromGitHubRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "enchant")
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
		public IEnumerator UpdateCurrentParameterMasterFromGitHub(
                Request.UpdateCurrentParameterMasterFromGitHubRequest request,
                UnityAction<AsyncResult<Result.UpdateCurrentParameterMasterFromGitHubResult>> callback
        ) =>
            new UpdateCurrentParameterMasterFromGitHubTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.UpdateCurrentParameterMasterFromGitHubResult> UpdateCurrentParameterMasterFromGitHubFuture(
                Request.UpdateCurrentParameterMasterFromGitHubRequest request
        ) =>
            new UpdateCurrentParameterMasterFromGitHubTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.UpdateCurrentParameterMasterFromGitHubResult> UpdateCurrentParameterMasterFromGitHubAsync(
                Request.UpdateCurrentParameterMasterFromGitHubRequest request
        ) =>
            new UpdateCurrentParameterMasterFromGitHubTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.UpdateCurrentParameterMasterFromGitHubResult>();
    #else
		public UpdateCurrentParameterMasterFromGitHubTask UpdateCurrentParameterMasterFromGitHubAsync(
                Request.UpdateCurrentParameterMasterFromGitHubRequest request
        )
		{
			return new UpdateCurrentParameterMasterFromGitHubTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.UpdateCurrentParameterMasterFromGitHubResult> UpdateCurrentParameterMasterFromGitHubAsync(
                Request.UpdateCurrentParameterMasterFromGitHubRequest request
        ) =>
            new UpdateCurrentParameterMasterFromGitHubTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class DescribeBalanceParameterStatusesTask : Gs2RestSessionTask<DescribeBalanceParameterStatusesRequest, DescribeBalanceParameterStatusesResult>
        {
            public DescribeBalanceParameterStatusesTask(IGs2Session session, RestSessionRequestFactory factory, DescribeBalanceParameterStatusesRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DescribeBalanceParameterStatusesRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "enchant")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/me/status/balance";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");

                var sessionRequest = Factory.Get(url);
                if (request.ContextStack != null)
                {
                    sessionRequest.AddQueryString("contextStack", request.ContextStack);
                }
                if (request.ParameterName != null) {
                    sessionRequest.AddQueryString("parameterName", $"{request.ParameterName}");
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
		public IEnumerator DescribeBalanceParameterStatuses(
                Request.DescribeBalanceParameterStatusesRequest request,
                UnityAction<AsyncResult<Result.DescribeBalanceParameterStatusesResult>> callback
        ) =>
            new DescribeBalanceParameterStatusesTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.DescribeBalanceParameterStatusesResult> DescribeBalanceParameterStatusesFuture(
                Request.DescribeBalanceParameterStatusesRequest request
        ) =>
            new DescribeBalanceParameterStatusesTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.DescribeBalanceParameterStatusesResult> DescribeBalanceParameterStatusesAsync(
                Request.DescribeBalanceParameterStatusesRequest request
        ) =>
            new DescribeBalanceParameterStatusesTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.DescribeBalanceParameterStatusesResult>();
    #else
		public DescribeBalanceParameterStatusesTask DescribeBalanceParameterStatusesAsync(
                Request.DescribeBalanceParameterStatusesRequest request
        )
		{
			return new DescribeBalanceParameterStatusesTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.DescribeBalanceParameterStatusesResult> DescribeBalanceParameterStatusesAsync(
                Request.DescribeBalanceParameterStatusesRequest request
        ) =>
            new DescribeBalanceParameterStatusesTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class DescribeBalanceParameterStatusesByUserIdTask : Gs2RestSessionTask<DescribeBalanceParameterStatusesByUserIdRequest, DescribeBalanceParameterStatusesByUserIdResult>
        {
            public DescribeBalanceParameterStatusesByUserIdTask(IGs2Session session, RestSessionRequestFactory factory, DescribeBalanceParameterStatusesByUserIdRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DescribeBalanceParameterStatusesByUserIdRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "enchant")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/{userId}/status/balance";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{userId}", !string.IsNullOrEmpty(request.UserId) ? request.UserId.ToString() : "null");

                var sessionRequest = Factory.Get(url);
                if (request.ContextStack != null)
                {
                    sessionRequest.AddQueryString("contextStack", request.ContextStack);
                }
                if (request.ParameterName != null) {
                    sessionRequest.AddQueryString("parameterName", $"{request.ParameterName}");
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
		public IEnumerator DescribeBalanceParameterStatusesByUserId(
                Request.DescribeBalanceParameterStatusesByUserIdRequest request,
                UnityAction<AsyncResult<Result.DescribeBalanceParameterStatusesByUserIdResult>> callback
        ) =>
            new DescribeBalanceParameterStatusesByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.DescribeBalanceParameterStatusesByUserIdResult> DescribeBalanceParameterStatusesByUserIdFuture(
                Request.DescribeBalanceParameterStatusesByUserIdRequest request
        ) =>
            new DescribeBalanceParameterStatusesByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.DescribeBalanceParameterStatusesByUserIdResult> DescribeBalanceParameterStatusesByUserIdAsync(
                Request.DescribeBalanceParameterStatusesByUserIdRequest request
        ) =>
            new DescribeBalanceParameterStatusesByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.DescribeBalanceParameterStatusesByUserIdResult>();
    #else
		public DescribeBalanceParameterStatusesByUserIdTask DescribeBalanceParameterStatusesByUserIdAsync(
                Request.DescribeBalanceParameterStatusesByUserIdRequest request
        )
		{
			return new DescribeBalanceParameterStatusesByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.DescribeBalanceParameterStatusesByUserIdResult> DescribeBalanceParameterStatusesByUserIdAsync(
                Request.DescribeBalanceParameterStatusesByUserIdRequest request
        ) =>
            new DescribeBalanceParameterStatusesByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class GetBalanceParameterStatusTask : Gs2RestSessionTask<GetBalanceParameterStatusRequest, GetBalanceParameterStatusResult>
        {
            public GetBalanceParameterStatusTask(IGs2Session session, RestSessionRequestFactory factory, GetBalanceParameterStatusRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(GetBalanceParameterStatusRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "enchant")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/me/status/balance/{parameterName}/{propertyId}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{parameterName}", !string.IsNullOrEmpty(request.ParameterName) ? request.ParameterName.ToString() : "null");
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
		public IEnumerator GetBalanceParameterStatus(
                Request.GetBalanceParameterStatusRequest request,
                UnityAction<AsyncResult<Result.GetBalanceParameterStatusResult>> callback
        ) =>
            new GetBalanceParameterStatusTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.GetBalanceParameterStatusResult> GetBalanceParameterStatusFuture(
                Request.GetBalanceParameterStatusRequest request
        ) =>
            new GetBalanceParameterStatusTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.GetBalanceParameterStatusResult> GetBalanceParameterStatusAsync(
                Request.GetBalanceParameterStatusRequest request
        ) =>
            new GetBalanceParameterStatusTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.GetBalanceParameterStatusResult>();
    #else
		public GetBalanceParameterStatusTask GetBalanceParameterStatusAsync(
                Request.GetBalanceParameterStatusRequest request
        )
		{
			return new GetBalanceParameterStatusTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.GetBalanceParameterStatusResult> GetBalanceParameterStatusAsync(
                Request.GetBalanceParameterStatusRequest request
        ) =>
            new GetBalanceParameterStatusTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class GetBalanceParameterStatusByUserIdTask : Gs2RestSessionTask<GetBalanceParameterStatusByUserIdRequest, GetBalanceParameterStatusByUserIdResult>
        {
            public GetBalanceParameterStatusByUserIdTask(IGs2Session session, RestSessionRequestFactory factory, GetBalanceParameterStatusByUserIdRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(GetBalanceParameterStatusByUserIdRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "enchant")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/{userId}/status/balance/{parameterName}/{propertyId}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{userId}", !string.IsNullOrEmpty(request.UserId) ? request.UserId.ToString() : "null");
                url = url.Replace("{parameterName}", !string.IsNullOrEmpty(request.ParameterName) ? request.ParameterName.ToString() : "null");
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
		public IEnumerator GetBalanceParameterStatusByUserId(
                Request.GetBalanceParameterStatusByUserIdRequest request,
                UnityAction<AsyncResult<Result.GetBalanceParameterStatusByUserIdResult>> callback
        ) =>
            new GetBalanceParameterStatusByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.GetBalanceParameterStatusByUserIdResult> GetBalanceParameterStatusByUserIdFuture(
                Request.GetBalanceParameterStatusByUserIdRequest request
        ) =>
            new GetBalanceParameterStatusByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.GetBalanceParameterStatusByUserIdResult> GetBalanceParameterStatusByUserIdAsync(
                Request.GetBalanceParameterStatusByUserIdRequest request
        ) =>
            new GetBalanceParameterStatusByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.GetBalanceParameterStatusByUserIdResult>();
    #else
		public GetBalanceParameterStatusByUserIdTask GetBalanceParameterStatusByUserIdAsync(
                Request.GetBalanceParameterStatusByUserIdRequest request
        )
		{
			return new GetBalanceParameterStatusByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.GetBalanceParameterStatusByUserIdResult> GetBalanceParameterStatusByUserIdAsync(
                Request.GetBalanceParameterStatusByUserIdRequest request
        ) =>
            new GetBalanceParameterStatusByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class DeleteBalanceParameterStatusByUserIdTask : Gs2RestSessionTask<DeleteBalanceParameterStatusByUserIdRequest, DeleteBalanceParameterStatusByUserIdResult>
        {
            public DeleteBalanceParameterStatusByUserIdTask(IGs2Session session, RestSessionRequestFactory factory, DeleteBalanceParameterStatusByUserIdRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DeleteBalanceParameterStatusByUserIdRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "enchant")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/{userId}/status/balance/{parameterName}/{propertyId}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{userId}", !string.IsNullOrEmpty(request.UserId) ? request.UserId.ToString() : "null");
                url = url.Replace("{parameterName}", !string.IsNullOrEmpty(request.ParameterName) ? request.ParameterName.ToString() : "null");
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
		public IEnumerator DeleteBalanceParameterStatusByUserId(
                Request.DeleteBalanceParameterStatusByUserIdRequest request,
                UnityAction<AsyncResult<Result.DeleteBalanceParameterStatusByUserIdResult>> callback
        ) =>
            new DeleteBalanceParameterStatusByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.DeleteBalanceParameterStatusByUserIdResult> DeleteBalanceParameterStatusByUserIdFuture(
                Request.DeleteBalanceParameterStatusByUserIdRequest request
        ) =>
            new DeleteBalanceParameterStatusByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.DeleteBalanceParameterStatusByUserIdResult> DeleteBalanceParameterStatusByUserIdAsync(
                Request.DeleteBalanceParameterStatusByUserIdRequest request
        ) =>
            new DeleteBalanceParameterStatusByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.DeleteBalanceParameterStatusByUserIdResult>();
    #else
		public DeleteBalanceParameterStatusByUserIdTask DeleteBalanceParameterStatusByUserIdAsync(
                Request.DeleteBalanceParameterStatusByUserIdRequest request
        )
		{
			return new DeleteBalanceParameterStatusByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.DeleteBalanceParameterStatusByUserIdResult> DeleteBalanceParameterStatusByUserIdAsync(
                Request.DeleteBalanceParameterStatusByUserIdRequest request
        ) =>
            new DeleteBalanceParameterStatusByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class ReDrawBalanceParameterStatusByUserIdTask : Gs2RestSessionTask<ReDrawBalanceParameterStatusByUserIdRequest, ReDrawBalanceParameterStatusByUserIdResult>
        {
            public ReDrawBalanceParameterStatusByUserIdTask(IGs2Session session, RestSessionRequestFactory factory, ReDrawBalanceParameterStatusByUserIdRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(ReDrawBalanceParameterStatusByUserIdRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "enchant")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/{userId}/status/balance/{parameterName}/{propertyId}/redraw";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{userId}", !string.IsNullOrEmpty(request.UserId) ? request.UserId.ToString() : "null");
                url = url.Replace("{parameterName}", !string.IsNullOrEmpty(request.ParameterName) ? request.ParameterName.ToString() : "null");
                url = url.Replace("{propertyId}", !string.IsNullOrEmpty(request.PropertyId) ? request.PropertyId.ToString() : "null");

                var sessionRequest = Factory.Post(url);

                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);
                jsonWriter.WriteObjectStart();
                if (request.FixedParameterNames != null)
                {
                    jsonWriter.WritePropertyName("fixedParameterNames");
                    jsonWriter.WriteArrayStart();
                    foreach(var item in request.FixedParameterNames)
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
		public IEnumerator ReDrawBalanceParameterStatusByUserId(
                Request.ReDrawBalanceParameterStatusByUserIdRequest request,
                UnityAction<AsyncResult<Result.ReDrawBalanceParameterStatusByUserIdResult>> callback
        ) =>
            new ReDrawBalanceParameterStatusByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.ReDrawBalanceParameterStatusByUserIdResult> ReDrawBalanceParameterStatusByUserIdFuture(
                Request.ReDrawBalanceParameterStatusByUserIdRequest request
        ) =>
            new ReDrawBalanceParameterStatusByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.ReDrawBalanceParameterStatusByUserIdResult> ReDrawBalanceParameterStatusByUserIdAsync(
                Request.ReDrawBalanceParameterStatusByUserIdRequest request
        ) =>
            new ReDrawBalanceParameterStatusByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.ReDrawBalanceParameterStatusByUserIdResult>();
    #else
		public ReDrawBalanceParameterStatusByUserIdTask ReDrawBalanceParameterStatusByUserIdAsync(
                Request.ReDrawBalanceParameterStatusByUserIdRequest request
        )
		{
			return new ReDrawBalanceParameterStatusByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.ReDrawBalanceParameterStatusByUserIdResult> ReDrawBalanceParameterStatusByUserIdAsync(
                Request.ReDrawBalanceParameterStatusByUserIdRequest request
        ) =>
            new ReDrawBalanceParameterStatusByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class ReDrawBalanceParameterStatusByStampSheetTask : Gs2RestSessionTask<ReDrawBalanceParameterStatusByStampSheetRequest, ReDrawBalanceParameterStatusByStampSheetResult>
        {
            public ReDrawBalanceParameterStatusByStampSheetTask(IGs2Session session, RestSessionRequestFactory factory, ReDrawBalanceParameterStatusByStampSheetRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(ReDrawBalanceParameterStatusByStampSheetRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "enchant")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/stamp/balance/redraw";

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
		public IEnumerator ReDrawBalanceParameterStatusByStampSheet(
                Request.ReDrawBalanceParameterStatusByStampSheetRequest request,
                UnityAction<AsyncResult<Result.ReDrawBalanceParameterStatusByStampSheetResult>> callback
        ) =>
            new ReDrawBalanceParameterStatusByStampSheetTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.ReDrawBalanceParameterStatusByStampSheetResult> ReDrawBalanceParameterStatusByStampSheetFuture(
                Request.ReDrawBalanceParameterStatusByStampSheetRequest request
        ) =>
            new ReDrawBalanceParameterStatusByStampSheetTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.ReDrawBalanceParameterStatusByStampSheetResult> ReDrawBalanceParameterStatusByStampSheetAsync(
                Request.ReDrawBalanceParameterStatusByStampSheetRequest request
        ) =>
            new ReDrawBalanceParameterStatusByStampSheetTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.ReDrawBalanceParameterStatusByStampSheetResult>();
    #else
		public ReDrawBalanceParameterStatusByStampSheetTask ReDrawBalanceParameterStatusByStampSheetAsync(
                Request.ReDrawBalanceParameterStatusByStampSheetRequest request
        )
		{
			return new ReDrawBalanceParameterStatusByStampSheetTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.ReDrawBalanceParameterStatusByStampSheetResult> ReDrawBalanceParameterStatusByStampSheetAsync(
                Request.ReDrawBalanceParameterStatusByStampSheetRequest request
        ) =>
            new ReDrawBalanceParameterStatusByStampSheetTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class SetBalanceParameterStatusByUserIdTask : Gs2RestSessionTask<SetBalanceParameterStatusByUserIdRequest, SetBalanceParameterStatusByUserIdResult>
        {
            public SetBalanceParameterStatusByUserIdTask(IGs2Session session, RestSessionRequestFactory factory, SetBalanceParameterStatusByUserIdRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(SetBalanceParameterStatusByUserIdRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "enchant")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/{userId}/status/balance/{parameterName}/{propertyId}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{userId}", !string.IsNullOrEmpty(request.UserId) ? request.UserId.ToString() : "null");
                url = url.Replace("{parameterName}", !string.IsNullOrEmpty(request.ParameterName) ? request.ParameterName.ToString() : "null");
                url = url.Replace("{propertyId}", !string.IsNullOrEmpty(request.PropertyId) ? request.PropertyId.ToString() : "null");

                var sessionRequest = Factory.Put(url);

                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);
                jsonWriter.WriteObjectStart();
                if (request.ParameterValues != null)
                {
                    jsonWriter.WritePropertyName("parameterValues");
                    jsonWriter.WriteArrayStart();
                    foreach(var item in request.ParameterValues)
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
		public IEnumerator SetBalanceParameterStatusByUserId(
                Request.SetBalanceParameterStatusByUserIdRequest request,
                UnityAction<AsyncResult<Result.SetBalanceParameterStatusByUserIdResult>> callback
        ) =>
            new SetBalanceParameterStatusByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.SetBalanceParameterStatusByUserIdResult> SetBalanceParameterStatusByUserIdFuture(
                Request.SetBalanceParameterStatusByUserIdRequest request
        ) =>
            new SetBalanceParameterStatusByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.SetBalanceParameterStatusByUserIdResult> SetBalanceParameterStatusByUserIdAsync(
                Request.SetBalanceParameterStatusByUserIdRequest request
        ) =>
            new SetBalanceParameterStatusByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.SetBalanceParameterStatusByUserIdResult>();
    #else
		public SetBalanceParameterStatusByUserIdTask SetBalanceParameterStatusByUserIdAsync(
                Request.SetBalanceParameterStatusByUserIdRequest request
        )
		{
			return new SetBalanceParameterStatusByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.SetBalanceParameterStatusByUserIdResult> SetBalanceParameterStatusByUserIdAsync(
                Request.SetBalanceParameterStatusByUserIdRequest request
        ) =>
            new SetBalanceParameterStatusByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class SetBalanceParameterStatusByStampSheetTask : Gs2RestSessionTask<SetBalanceParameterStatusByStampSheetRequest, SetBalanceParameterStatusByStampSheetResult>
        {
            public SetBalanceParameterStatusByStampSheetTask(IGs2Session session, RestSessionRequestFactory factory, SetBalanceParameterStatusByStampSheetRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(SetBalanceParameterStatusByStampSheetRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "enchant")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/stamp/balance/set";

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
		public IEnumerator SetBalanceParameterStatusByStampSheet(
                Request.SetBalanceParameterStatusByStampSheetRequest request,
                UnityAction<AsyncResult<Result.SetBalanceParameterStatusByStampSheetResult>> callback
        ) =>
            new SetBalanceParameterStatusByStampSheetTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.SetBalanceParameterStatusByStampSheetResult> SetBalanceParameterStatusByStampSheetFuture(
                Request.SetBalanceParameterStatusByStampSheetRequest request
        ) =>
            new SetBalanceParameterStatusByStampSheetTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.SetBalanceParameterStatusByStampSheetResult> SetBalanceParameterStatusByStampSheetAsync(
                Request.SetBalanceParameterStatusByStampSheetRequest request
        ) =>
            new SetBalanceParameterStatusByStampSheetTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.SetBalanceParameterStatusByStampSheetResult>();
    #else
		public SetBalanceParameterStatusByStampSheetTask SetBalanceParameterStatusByStampSheetAsync(
                Request.SetBalanceParameterStatusByStampSheetRequest request
        )
		{
			return new SetBalanceParameterStatusByStampSheetTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.SetBalanceParameterStatusByStampSheetResult> SetBalanceParameterStatusByStampSheetAsync(
                Request.SetBalanceParameterStatusByStampSheetRequest request
        ) =>
            new SetBalanceParameterStatusByStampSheetTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class DescribeRarityParameterStatusesTask : Gs2RestSessionTask<DescribeRarityParameterStatusesRequest, DescribeRarityParameterStatusesResult>
        {
            public DescribeRarityParameterStatusesTask(IGs2Session session, RestSessionRequestFactory factory, DescribeRarityParameterStatusesRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DescribeRarityParameterStatusesRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "enchant")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/me/status/rarity";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");

                var sessionRequest = Factory.Get(url);
                if (request.ContextStack != null)
                {
                    sessionRequest.AddQueryString("contextStack", request.ContextStack);
                }
                if (request.ParameterName != null) {
                    sessionRequest.AddQueryString("parameterName", $"{request.ParameterName}");
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
		public IEnumerator DescribeRarityParameterStatuses(
                Request.DescribeRarityParameterStatusesRequest request,
                UnityAction<AsyncResult<Result.DescribeRarityParameterStatusesResult>> callback
        ) =>
            new DescribeRarityParameterStatusesTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.DescribeRarityParameterStatusesResult> DescribeRarityParameterStatusesFuture(
                Request.DescribeRarityParameterStatusesRequest request
        ) =>
            new DescribeRarityParameterStatusesTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.DescribeRarityParameterStatusesResult> DescribeRarityParameterStatusesAsync(
                Request.DescribeRarityParameterStatusesRequest request
        ) =>
            new DescribeRarityParameterStatusesTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.DescribeRarityParameterStatusesResult>();
    #else
		public DescribeRarityParameterStatusesTask DescribeRarityParameterStatusesAsync(
                Request.DescribeRarityParameterStatusesRequest request
        )
		{
			return new DescribeRarityParameterStatusesTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.DescribeRarityParameterStatusesResult> DescribeRarityParameterStatusesAsync(
                Request.DescribeRarityParameterStatusesRequest request
        ) =>
            new DescribeRarityParameterStatusesTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class DescribeRarityParameterStatusesByUserIdTask : Gs2RestSessionTask<DescribeRarityParameterStatusesByUserIdRequest, DescribeRarityParameterStatusesByUserIdResult>
        {
            public DescribeRarityParameterStatusesByUserIdTask(IGs2Session session, RestSessionRequestFactory factory, DescribeRarityParameterStatusesByUserIdRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DescribeRarityParameterStatusesByUserIdRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "enchant")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/{userId}/status/rarity";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{userId}", !string.IsNullOrEmpty(request.UserId) ? request.UserId.ToString() : "null");

                var sessionRequest = Factory.Get(url);
                if (request.ContextStack != null)
                {
                    sessionRequest.AddQueryString("contextStack", request.ContextStack);
                }
                if (request.ParameterName != null) {
                    sessionRequest.AddQueryString("parameterName", $"{request.ParameterName}");
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
		public IEnumerator DescribeRarityParameterStatusesByUserId(
                Request.DescribeRarityParameterStatusesByUserIdRequest request,
                UnityAction<AsyncResult<Result.DescribeRarityParameterStatusesByUserIdResult>> callback
        ) =>
            new DescribeRarityParameterStatusesByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.DescribeRarityParameterStatusesByUserIdResult> DescribeRarityParameterStatusesByUserIdFuture(
                Request.DescribeRarityParameterStatusesByUserIdRequest request
        ) =>
            new DescribeRarityParameterStatusesByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.DescribeRarityParameterStatusesByUserIdResult> DescribeRarityParameterStatusesByUserIdAsync(
                Request.DescribeRarityParameterStatusesByUserIdRequest request
        ) =>
            new DescribeRarityParameterStatusesByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.DescribeRarityParameterStatusesByUserIdResult>();
    #else
		public DescribeRarityParameterStatusesByUserIdTask DescribeRarityParameterStatusesByUserIdAsync(
                Request.DescribeRarityParameterStatusesByUserIdRequest request
        )
		{
			return new DescribeRarityParameterStatusesByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.DescribeRarityParameterStatusesByUserIdResult> DescribeRarityParameterStatusesByUserIdAsync(
                Request.DescribeRarityParameterStatusesByUserIdRequest request
        ) =>
            new DescribeRarityParameterStatusesByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class GetRarityParameterStatusTask : Gs2RestSessionTask<GetRarityParameterStatusRequest, GetRarityParameterStatusResult>
        {
            public GetRarityParameterStatusTask(IGs2Session session, RestSessionRequestFactory factory, GetRarityParameterStatusRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(GetRarityParameterStatusRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "enchant")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/me/status/rarity/{parameterName}/{propertyId}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{parameterName}", !string.IsNullOrEmpty(request.ParameterName) ? request.ParameterName.ToString() : "null");
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
		public IEnumerator GetRarityParameterStatus(
                Request.GetRarityParameterStatusRequest request,
                UnityAction<AsyncResult<Result.GetRarityParameterStatusResult>> callback
        ) =>
            new GetRarityParameterStatusTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.GetRarityParameterStatusResult> GetRarityParameterStatusFuture(
                Request.GetRarityParameterStatusRequest request
        ) =>
            new GetRarityParameterStatusTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.GetRarityParameterStatusResult> GetRarityParameterStatusAsync(
                Request.GetRarityParameterStatusRequest request
        ) =>
            new GetRarityParameterStatusTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.GetRarityParameterStatusResult>();
    #else
		public GetRarityParameterStatusTask GetRarityParameterStatusAsync(
                Request.GetRarityParameterStatusRequest request
        )
		{
			return new GetRarityParameterStatusTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.GetRarityParameterStatusResult> GetRarityParameterStatusAsync(
                Request.GetRarityParameterStatusRequest request
        ) =>
            new GetRarityParameterStatusTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class GetRarityParameterStatusByUserIdTask : Gs2RestSessionTask<GetRarityParameterStatusByUserIdRequest, GetRarityParameterStatusByUserIdResult>
        {
            public GetRarityParameterStatusByUserIdTask(IGs2Session session, RestSessionRequestFactory factory, GetRarityParameterStatusByUserIdRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(GetRarityParameterStatusByUserIdRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "enchant")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/{userId}/status/rarity/{parameterName}/{propertyId}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{userId}", !string.IsNullOrEmpty(request.UserId) ? request.UserId.ToString() : "null");
                url = url.Replace("{parameterName}", !string.IsNullOrEmpty(request.ParameterName) ? request.ParameterName.ToString() : "null");
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
		public IEnumerator GetRarityParameterStatusByUserId(
                Request.GetRarityParameterStatusByUserIdRequest request,
                UnityAction<AsyncResult<Result.GetRarityParameterStatusByUserIdResult>> callback
        ) =>
            new GetRarityParameterStatusByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.GetRarityParameterStatusByUserIdResult> GetRarityParameterStatusByUserIdFuture(
                Request.GetRarityParameterStatusByUserIdRequest request
        ) =>
            new GetRarityParameterStatusByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.GetRarityParameterStatusByUserIdResult> GetRarityParameterStatusByUserIdAsync(
                Request.GetRarityParameterStatusByUserIdRequest request
        ) =>
            new GetRarityParameterStatusByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.GetRarityParameterStatusByUserIdResult>();
    #else
		public GetRarityParameterStatusByUserIdTask GetRarityParameterStatusByUserIdAsync(
                Request.GetRarityParameterStatusByUserIdRequest request
        )
		{
			return new GetRarityParameterStatusByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.GetRarityParameterStatusByUserIdResult> GetRarityParameterStatusByUserIdAsync(
                Request.GetRarityParameterStatusByUserIdRequest request
        ) =>
            new GetRarityParameterStatusByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class DeleteRarityParameterStatusByUserIdTask : Gs2RestSessionTask<DeleteRarityParameterStatusByUserIdRequest, DeleteRarityParameterStatusByUserIdResult>
        {
            public DeleteRarityParameterStatusByUserIdTask(IGs2Session session, RestSessionRequestFactory factory, DeleteRarityParameterStatusByUserIdRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DeleteRarityParameterStatusByUserIdRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "enchant")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/{userId}/status/rarity/{parameterName}/{propertyId}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{userId}", !string.IsNullOrEmpty(request.UserId) ? request.UserId.ToString() : "null");
                url = url.Replace("{parameterName}", !string.IsNullOrEmpty(request.ParameterName) ? request.ParameterName.ToString() : "null");
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
		public IEnumerator DeleteRarityParameterStatusByUserId(
                Request.DeleteRarityParameterStatusByUserIdRequest request,
                UnityAction<AsyncResult<Result.DeleteRarityParameterStatusByUserIdResult>> callback
        ) =>
            new DeleteRarityParameterStatusByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.DeleteRarityParameterStatusByUserIdResult> DeleteRarityParameterStatusByUserIdFuture(
                Request.DeleteRarityParameterStatusByUserIdRequest request
        ) =>
            new DeleteRarityParameterStatusByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.DeleteRarityParameterStatusByUserIdResult> DeleteRarityParameterStatusByUserIdAsync(
                Request.DeleteRarityParameterStatusByUserIdRequest request
        ) =>
            new DeleteRarityParameterStatusByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.DeleteRarityParameterStatusByUserIdResult>();
    #else
		public DeleteRarityParameterStatusByUserIdTask DeleteRarityParameterStatusByUserIdAsync(
                Request.DeleteRarityParameterStatusByUserIdRequest request
        )
		{
			return new DeleteRarityParameterStatusByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.DeleteRarityParameterStatusByUserIdResult> DeleteRarityParameterStatusByUserIdAsync(
                Request.DeleteRarityParameterStatusByUserIdRequest request
        ) =>
            new DeleteRarityParameterStatusByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class ReDrawRarityParameterStatusByUserIdTask : Gs2RestSessionTask<ReDrawRarityParameterStatusByUserIdRequest, ReDrawRarityParameterStatusByUserIdResult>
        {
            public ReDrawRarityParameterStatusByUserIdTask(IGs2Session session, RestSessionRequestFactory factory, ReDrawRarityParameterStatusByUserIdRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(ReDrawRarityParameterStatusByUserIdRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "enchant")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/{userId}/status/rarity/{parameterName}/{propertyId}/redraw";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{userId}", !string.IsNullOrEmpty(request.UserId) ? request.UserId.ToString() : "null");
                url = url.Replace("{parameterName}", !string.IsNullOrEmpty(request.ParameterName) ? request.ParameterName.ToString() : "null");
                url = url.Replace("{propertyId}", !string.IsNullOrEmpty(request.PropertyId) ? request.PropertyId.ToString() : "null");

                var sessionRequest = Factory.Post(url);

                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);
                jsonWriter.WriteObjectStart();
                if (request.FixedParameterNames != null)
                {
                    jsonWriter.WritePropertyName("fixedParameterNames");
                    jsonWriter.WriteArrayStart();
                    foreach(var item in request.FixedParameterNames)
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
		public IEnumerator ReDrawRarityParameterStatusByUserId(
                Request.ReDrawRarityParameterStatusByUserIdRequest request,
                UnityAction<AsyncResult<Result.ReDrawRarityParameterStatusByUserIdResult>> callback
        ) =>
            new ReDrawRarityParameterStatusByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.ReDrawRarityParameterStatusByUserIdResult> ReDrawRarityParameterStatusByUserIdFuture(
                Request.ReDrawRarityParameterStatusByUserIdRequest request
        ) =>
            new ReDrawRarityParameterStatusByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.ReDrawRarityParameterStatusByUserIdResult> ReDrawRarityParameterStatusByUserIdAsync(
                Request.ReDrawRarityParameterStatusByUserIdRequest request
        ) =>
            new ReDrawRarityParameterStatusByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.ReDrawRarityParameterStatusByUserIdResult>();
    #else
		public ReDrawRarityParameterStatusByUserIdTask ReDrawRarityParameterStatusByUserIdAsync(
                Request.ReDrawRarityParameterStatusByUserIdRequest request
        )
		{
			return new ReDrawRarityParameterStatusByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.ReDrawRarityParameterStatusByUserIdResult> ReDrawRarityParameterStatusByUserIdAsync(
                Request.ReDrawRarityParameterStatusByUserIdRequest request
        ) =>
            new ReDrawRarityParameterStatusByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class ReDrawRarityParameterStatusByStampSheetTask : Gs2RestSessionTask<ReDrawRarityParameterStatusByStampSheetRequest, ReDrawRarityParameterStatusByStampSheetResult>
        {
            public ReDrawRarityParameterStatusByStampSheetTask(IGs2Session session, RestSessionRequestFactory factory, ReDrawRarityParameterStatusByStampSheetRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(ReDrawRarityParameterStatusByStampSheetRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "enchant")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/stamp/rarity/parameter/redraw";

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
		public IEnumerator ReDrawRarityParameterStatusByStampSheet(
                Request.ReDrawRarityParameterStatusByStampSheetRequest request,
                UnityAction<AsyncResult<Result.ReDrawRarityParameterStatusByStampSheetResult>> callback
        ) =>
            new ReDrawRarityParameterStatusByStampSheetTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.ReDrawRarityParameterStatusByStampSheetResult> ReDrawRarityParameterStatusByStampSheetFuture(
                Request.ReDrawRarityParameterStatusByStampSheetRequest request
        ) =>
            new ReDrawRarityParameterStatusByStampSheetTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.ReDrawRarityParameterStatusByStampSheetResult> ReDrawRarityParameterStatusByStampSheetAsync(
                Request.ReDrawRarityParameterStatusByStampSheetRequest request
        ) =>
            new ReDrawRarityParameterStatusByStampSheetTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.ReDrawRarityParameterStatusByStampSheetResult>();
    #else
		public ReDrawRarityParameterStatusByStampSheetTask ReDrawRarityParameterStatusByStampSheetAsync(
                Request.ReDrawRarityParameterStatusByStampSheetRequest request
        )
		{
			return new ReDrawRarityParameterStatusByStampSheetTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.ReDrawRarityParameterStatusByStampSheetResult> ReDrawRarityParameterStatusByStampSheetAsync(
                Request.ReDrawRarityParameterStatusByStampSheetRequest request
        ) =>
            new ReDrawRarityParameterStatusByStampSheetTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class AddRarityParameterStatusByUserIdTask : Gs2RestSessionTask<AddRarityParameterStatusByUserIdRequest, AddRarityParameterStatusByUserIdResult>
        {
            public AddRarityParameterStatusByUserIdTask(IGs2Session session, RestSessionRequestFactory factory, AddRarityParameterStatusByUserIdRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(AddRarityParameterStatusByUserIdRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "enchant")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/{userId}/status/rarity/{parameterName}/{propertyId}/add";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{userId}", !string.IsNullOrEmpty(request.UserId) ? request.UserId.ToString() : "null");
                url = url.Replace("{parameterName}", !string.IsNullOrEmpty(request.ParameterName) ? request.ParameterName.ToString() : "null");
                url = url.Replace("{propertyId}", !string.IsNullOrEmpty(request.PropertyId) ? request.PropertyId.ToString() : "null");

                var sessionRequest = Factory.Post(url);

                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);
                jsonWriter.WriteObjectStart();
                if (request.Count != null)
                {
                    jsonWriter.WritePropertyName("count");
                    jsonWriter.Write(request.Count.ToString());
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
		public IEnumerator AddRarityParameterStatusByUserId(
                Request.AddRarityParameterStatusByUserIdRequest request,
                UnityAction<AsyncResult<Result.AddRarityParameterStatusByUserIdResult>> callback
        ) =>
            new AddRarityParameterStatusByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.AddRarityParameterStatusByUserIdResult> AddRarityParameterStatusByUserIdFuture(
                Request.AddRarityParameterStatusByUserIdRequest request
        ) =>
            new AddRarityParameterStatusByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.AddRarityParameterStatusByUserIdResult> AddRarityParameterStatusByUserIdAsync(
                Request.AddRarityParameterStatusByUserIdRequest request
        ) =>
            new AddRarityParameterStatusByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.AddRarityParameterStatusByUserIdResult>();
    #else
		public AddRarityParameterStatusByUserIdTask AddRarityParameterStatusByUserIdAsync(
                Request.AddRarityParameterStatusByUserIdRequest request
        )
		{
			return new AddRarityParameterStatusByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.AddRarityParameterStatusByUserIdResult> AddRarityParameterStatusByUserIdAsync(
                Request.AddRarityParameterStatusByUserIdRequest request
        ) =>
            new AddRarityParameterStatusByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class AddRarityParameterStatusByStampSheetTask : Gs2RestSessionTask<AddRarityParameterStatusByStampSheetRequest, AddRarityParameterStatusByStampSheetResult>
        {
            public AddRarityParameterStatusByStampSheetTask(IGs2Session session, RestSessionRequestFactory factory, AddRarityParameterStatusByStampSheetRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(AddRarityParameterStatusByStampSheetRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "enchant")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/stamp/rarity/parameter/add";

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
		public IEnumerator AddRarityParameterStatusByStampSheet(
                Request.AddRarityParameterStatusByStampSheetRequest request,
                UnityAction<AsyncResult<Result.AddRarityParameterStatusByStampSheetResult>> callback
        ) =>
            new AddRarityParameterStatusByStampSheetTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.AddRarityParameterStatusByStampSheetResult> AddRarityParameterStatusByStampSheetFuture(
                Request.AddRarityParameterStatusByStampSheetRequest request
        ) =>
            new AddRarityParameterStatusByStampSheetTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.AddRarityParameterStatusByStampSheetResult> AddRarityParameterStatusByStampSheetAsync(
                Request.AddRarityParameterStatusByStampSheetRequest request
        ) =>
            new AddRarityParameterStatusByStampSheetTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.AddRarityParameterStatusByStampSheetResult>();
    #else
		public AddRarityParameterStatusByStampSheetTask AddRarityParameterStatusByStampSheetAsync(
                Request.AddRarityParameterStatusByStampSheetRequest request
        )
		{
			return new AddRarityParameterStatusByStampSheetTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.AddRarityParameterStatusByStampSheetResult> AddRarityParameterStatusByStampSheetAsync(
                Request.AddRarityParameterStatusByStampSheetRequest request
        ) =>
            new AddRarityParameterStatusByStampSheetTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class VerifyRarityParameterStatusTask : Gs2RestSessionTask<VerifyRarityParameterStatusRequest, VerifyRarityParameterStatusResult>
        {
            public VerifyRarityParameterStatusTask(IGs2Session session, RestSessionRequestFactory factory, VerifyRarityParameterStatusRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(VerifyRarityParameterStatusRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "enchant")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/me/status/rarity/{parameterName}/{propertyId}/verify/{verifyType}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{parameterName}", !string.IsNullOrEmpty(request.ParameterName) ? request.ParameterName.ToString() : "null");
                url = url.Replace("{propertyId}", !string.IsNullOrEmpty(request.PropertyId) ? request.PropertyId.ToString() : "null");
                url = url.Replace("{verifyType}", !string.IsNullOrEmpty(request.VerifyType) ? request.VerifyType.ToString() : "null");

                var sessionRequest = Factory.Post(url);

                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);
                jsonWriter.WriteObjectStart();
                if (request.ParameterValueName != null)
                {
                    jsonWriter.WritePropertyName("parameterValueName");
                    jsonWriter.Write(request.ParameterValueName);
                }
                if (request.ParameterCount != null)
                {
                    jsonWriter.WritePropertyName("parameterCount");
                    jsonWriter.Write(request.ParameterCount.ToString());
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
		public IEnumerator VerifyRarityParameterStatus(
                Request.VerifyRarityParameterStatusRequest request,
                UnityAction<AsyncResult<Result.VerifyRarityParameterStatusResult>> callback
        ) =>
            new VerifyRarityParameterStatusTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.VerifyRarityParameterStatusResult> VerifyRarityParameterStatusFuture(
                Request.VerifyRarityParameterStatusRequest request
        ) =>
            new VerifyRarityParameterStatusTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.VerifyRarityParameterStatusResult> VerifyRarityParameterStatusAsync(
                Request.VerifyRarityParameterStatusRequest request
        ) =>
            new VerifyRarityParameterStatusTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.VerifyRarityParameterStatusResult>();
    #else
		public VerifyRarityParameterStatusTask VerifyRarityParameterStatusAsync(
                Request.VerifyRarityParameterStatusRequest request
        )
		{
			return new VerifyRarityParameterStatusTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.VerifyRarityParameterStatusResult> VerifyRarityParameterStatusAsync(
                Request.VerifyRarityParameterStatusRequest request
        ) =>
            new VerifyRarityParameterStatusTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class VerifyRarityParameterStatusByUserIdTask : Gs2RestSessionTask<VerifyRarityParameterStatusByUserIdRequest, VerifyRarityParameterStatusByUserIdResult>
        {
            public VerifyRarityParameterStatusByUserIdTask(IGs2Session session, RestSessionRequestFactory factory, VerifyRarityParameterStatusByUserIdRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(VerifyRarityParameterStatusByUserIdRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "enchant")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/{userId}/status/rarity/{parameterName}/{propertyId}/verify/{verifyType}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{parameterName}", !string.IsNullOrEmpty(request.ParameterName) ? request.ParameterName.ToString() : "null");
                url = url.Replace("{userId}", !string.IsNullOrEmpty(request.UserId) ? request.UserId.ToString() : "null");
                url = url.Replace("{propertyId}", !string.IsNullOrEmpty(request.PropertyId) ? request.PropertyId.ToString() : "null");
                url = url.Replace("{verifyType}", !string.IsNullOrEmpty(request.VerifyType) ? request.VerifyType.ToString() : "null");

                var sessionRequest = Factory.Post(url);

                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);
                jsonWriter.WriteObjectStart();
                if (request.ParameterValueName != null)
                {
                    jsonWriter.WritePropertyName("parameterValueName");
                    jsonWriter.Write(request.ParameterValueName);
                }
                if (request.ParameterCount != null)
                {
                    jsonWriter.WritePropertyName("parameterCount");
                    jsonWriter.Write(request.ParameterCount.ToString());
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
		public IEnumerator VerifyRarityParameterStatusByUserId(
                Request.VerifyRarityParameterStatusByUserIdRequest request,
                UnityAction<AsyncResult<Result.VerifyRarityParameterStatusByUserIdResult>> callback
        ) =>
            new VerifyRarityParameterStatusByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.VerifyRarityParameterStatusByUserIdResult> VerifyRarityParameterStatusByUserIdFuture(
                Request.VerifyRarityParameterStatusByUserIdRequest request
        ) =>
            new VerifyRarityParameterStatusByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.VerifyRarityParameterStatusByUserIdResult> VerifyRarityParameterStatusByUserIdAsync(
                Request.VerifyRarityParameterStatusByUserIdRequest request
        ) =>
            new VerifyRarityParameterStatusByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.VerifyRarityParameterStatusByUserIdResult>();
    #else
		public VerifyRarityParameterStatusByUserIdTask VerifyRarityParameterStatusByUserIdAsync(
                Request.VerifyRarityParameterStatusByUserIdRequest request
        )
		{
			return new VerifyRarityParameterStatusByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.VerifyRarityParameterStatusByUserIdResult> VerifyRarityParameterStatusByUserIdAsync(
                Request.VerifyRarityParameterStatusByUserIdRequest request
        ) =>
            new VerifyRarityParameterStatusByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class VerifyRarityParameterStatusByStampTaskTask : Gs2RestSessionTask<VerifyRarityParameterStatusByStampTaskRequest, VerifyRarityParameterStatusByStampTaskResult>
        {
            public VerifyRarityParameterStatusByStampTaskTask(IGs2Session session, RestSessionRequestFactory factory, VerifyRarityParameterStatusByStampTaskRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(VerifyRarityParameterStatusByStampTaskRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "enchant")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/stamp/rarity/parameter/verify";

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
		public IEnumerator VerifyRarityParameterStatusByStampTask(
                Request.VerifyRarityParameterStatusByStampTaskRequest request,
                UnityAction<AsyncResult<Result.VerifyRarityParameterStatusByStampTaskResult>> callback
        ) =>
            new VerifyRarityParameterStatusByStampTaskTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.VerifyRarityParameterStatusByStampTaskResult> VerifyRarityParameterStatusByStampTaskFuture(
                Request.VerifyRarityParameterStatusByStampTaskRequest request
        ) =>
            new VerifyRarityParameterStatusByStampTaskTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.VerifyRarityParameterStatusByStampTaskResult> VerifyRarityParameterStatusByStampTaskAsync(
                Request.VerifyRarityParameterStatusByStampTaskRequest request
        ) =>
            new VerifyRarityParameterStatusByStampTaskTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.VerifyRarityParameterStatusByStampTaskResult>();
    #else
		public VerifyRarityParameterStatusByStampTaskTask VerifyRarityParameterStatusByStampTaskAsync(
                Request.VerifyRarityParameterStatusByStampTaskRequest request
        )
		{
			return new VerifyRarityParameterStatusByStampTaskTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.VerifyRarityParameterStatusByStampTaskResult> VerifyRarityParameterStatusByStampTaskAsync(
                Request.VerifyRarityParameterStatusByStampTaskRequest request
        ) =>
            new VerifyRarityParameterStatusByStampTaskTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class SetRarityParameterStatusByUserIdTask : Gs2RestSessionTask<SetRarityParameterStatusByUserIdRequest, SetRarityParameterStatusByUserIdResult>
        {
            public SetRarityParameterStatusByUserIdTask(IGs2Session session, RestSessionRequestFactory factory, SetRarityParameterStatusByUserIdRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(SetRarityParameterStatusByUserIdRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "enchant")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/{userId}/status/rarity/{parameterName}/{propertyId}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{userId}", !string.IsNullOrEmpty(request.UserId) ? request.UserId.ToString() : "null");
                url = url.Replace("{parameterName}", !string.IsNullOrEmpty(request.ParameterName) ? request.ParameterName.ToString() : "null");
                url = url.Replace("{propertyId}", !string.IsNullOrEmpty(request.PropertyId) ? request.PropertyId.ToString() : "null");

                var sessionRequest = Factory.Put(url);

                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);
                jsonWriter.WriteObjectStart();
                if (request.ParameterValues != null)
                {
                    jsonWriter.WritePropertyName("parameterValues");
                    jsonWriter.WriteArrayStart();
                    foreach(var item in request.ParameterValues)
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
		public IEnumerator SetRarityParameterStatusByUserId(
                Request.SetRarityParameterStatusByUserIdRequest request,
                UnityAction<AsyncResult<Result.SetRarityParameterStatusByUserIdResult>> callback
        ) =>
            new SetRarityParameterStatusByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.SetRarityParameterStatusByUserIdResult> SetRarityParameterStatusByUserIdFuture(
                Request.SetRarityParameterStatusByUserIdRequest request
        ) =>
            new SetRarityParameterStatusByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.SetRarityParameterStatusByUserIdResult> SetRarityParameterStatusByUserIdAsync(
                Request.SetRarityParameterStatusByUserIdRequest request
        ) =>
            new SetRarityParameterStatusByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.SetRarityParameterStatusByUserIdResult>();
    #else
		public SetRarityParameterStatusByUserIdTask SetRarityParameterStatusByUserIdAsync(
                Request.SetRarityParameterStatusByUserIdRequest request
        )
		{
			return new SetRarityParameterStatusByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.SetRarityParameterStatusByUserIdResult> SetRarityParameterStatusByUserIdAsync(
                Request.SetRarityParameterStatusByUserIdRequest request
        ) =>
            new SetRarityParameterStatusByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class SetRarityParameterStatusByStampSheetTask : Gs2RestSessionTask<SetRarityParameterStatusByStampSheetRequest, SetRarityParameterStatusByStampSheetResult>
        {
            public SetRarityParameterStatusByStampSheetTask(IGs2Session session, RestSessionRequestFactory factory, SetRarityParameterStatusByStampSheetRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(SetRarityParameterStatusByStampSheetRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "enchant")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/stamp/rarity/parameter/set";

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
		public IEnumerator SetRarityParameterStatusByStampSheet(
                Request.SetRarityParameterStatusByStampSheetRequest request,
                UnityAction<AsyncResult<Result.SetRarityParameterStatusByStampSheetResult>> callback
        ) =>
            new SetRarityParameterStatusByStampSheetTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.SetRarityParameterStatusByStampSheetResult> SetRarityParameterStatusByStampSheetFuture(
                Request.SetRarityParameterStatusByStampSheetRequest request
        ) =>
            new SetRarityParameterStatusByStampSheetTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.SetRarityParameterStatusByStampSheetResult> SetRarityParameterStatusByStampSheetAsync(
                Request.SetRarityParameterStatusByStampSheetRequest request
        ) =>
            new SetRarityParameterStatusByStampSheetTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.SetRarityParameterStatusByStampSheetResult>();
    #else
		public SetRarityParameterStatusByStampSheetTask SetRarityParameterStatusByStampSheetAsync(
                Request.SetRarityParameterStatusByStampSheetRequest request
        )
		{
			return new SetRarityParameterStatusByStampSheetTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.SetRarityParameterStatusByStampSheetResult> SetRarityParameterStatusByStampSheetAsync(
                Request.SetRarityParameterStatusByStampSheetRequest request
        ) =>
            new SetRarityParameterStatusByStampSheetTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif
	}
}