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
using Gs2.Gs2Formation.Request;
using Gs2.Gs2Formation.Result;
using Gs2.Util.LitJson;

namespace Gs2.Gs2Formation
{
	public class Gs2FormationRestClient : AbstractGs2Client
	{
#if UNITY_2017_1_OR_NEWER
		private readonly CertificateHandler _certificateHandler;
#endif

		public static string Endpoint = "formation";

        protected Gs2RestSession Gs2RestSession => (Gs2RestSession) Gs2Session;

		public Gs2FormationRestClient(Gs2RestSession Gs2RestSession) : base(Gs2RestSession)
		{

		}

#if UNITY_2017_1_OR_NEWER
		public Gs2FormationRestClient(Gs2RestSession gs2RestSession, CertificateHandler certificateHandler) : base(gs2RestSession)
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
                    .Replace("{service}", "formation")
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
                    .Replace("{service}", "formation")
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
                if (request.UpdateMoldScript != null)
                {
                    jsonWriter.WritePropertyName("updateMoldScript");
                    request.UpdateMoldScript.WriteJson(jsonWriter);
                }
                if (request.UpdateFormScript != null)
                {
                    jsonWriter.WritePropertyName("updateFormScript");
                    request.UpdateFormScript.WriteJson(jsonWriter);
                }
                if (request.UpdatePropertyFormScript != null)
                {
                    jsonWriter.WritePropertyName("updatePropertyFormScript");
                    request.UpdatePropertyFormScript.WriteJson(jsonWriter);
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
                    .Replace("{service}", "formation")
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
                    .Replace("{service}", "formation")
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
                    .Replace("{service}", "formation")
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
                if (request.UpdateMoldScript != null)
                {
                    jsonWriter.WritePropertyName("updateMoldScript");
                    request.UpdateMoldScript.WriteJson(jsonWriter);
                }
                if (request.UpdateFormScript != null)
                {
                    jsonWriter.WritePropertyName("updateFormScript");
                    request.UpdateFormScript.WriteJson(jsonWriter);
                }
                if (request.UpdatePropertyFormScript != null)
                {
                    jsonWriter.WritePropertyName("updatePropertyFormScript");
                    request.UpdatePropertyFormScript.WriteJson(jsonWriter);
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
                    .Replace("{service}", "formation")
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
                    .Replace("{service}", "formation")
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
                    .Replace("{service}", "formation")
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
                    .Replace("{service}", "formation")
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
                    .Replace("{service}", "formation")
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
                    .Replace("{service}", "formation")
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
                    .Replace("{service}", "formation")
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
                    .Replace("{service}", "formation")
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
                    .Replace("{service}", "formation")
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


        public class GetFormModelTask : Gs2RestSessionTask<GetFormModelRequest, GetFormModelResult>
        {
            public GetFormModelTask(IGs2Session session, RestSessionRequestFactory factory, GetFormModelRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(GetFormModelRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "formation")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/model/{moldModelName}/form";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{moldModelName}", !string.IsNullOrEmpty(request.MoldModelName) ? request.MoldModelName.ToString() : "null");

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
		public IEnumerator GetFormModel(
                Request.GetFormModelRequest request,
                UnityAction<AsyncResult<Result.GetFormModelResult>> callback
        ) =>
            new GetFormModelTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.GetFormModelResult> GetFormModelFuture(
                Request.GetFormModelRequest request
        ) =>
            new GetFormModelTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.GetFormModelResult> GetFormModelAsync(
                Request.GetFormModelRequest request
        ) =>
            new GetFormModelTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.GetFormModelResult>();
    #else
		public GetFormModelTask GetFormModelAsync(
                Request.GetFormModelRequest request
        )
		{
			return new GetFormModelTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.GetFormModelResult> GetFormModelAsync(
                Request.GetFormModelRequest request
        ) =>
            new GetFormModelTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class DescribeFormModelMastersTask : Gs2RestSessionTask<DescribeFormModelMastersRequest, DescribeFormModelMastersResult>
        {
            public DescribeFormModelMastersTask(IGs2Session session, RestSessionRequestFactory factory, DescribeFormModelMastersRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DescribeFormModelMastersRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "formation")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/master/model/form";

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
		public IEnumerator DescribeFormModelMasters(
                Request.DescribeFormModelMastersRequest request,
                UnityAction<AsyncResult<Result.DescribeFormModelMastersResult>> callback
        ) =>
            new DescribeFormModelMastersTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.DescribeFormModelMastersResult> DescribeFormModelMastersFuture(
                Request.DescribeFormModelMastersRequest request
        ) =>
            new DescribeFormModelMastersTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.DescribeFormModelMastersResult> DescribeFormModelMastersAsync(
                Request.DescribeFormModelMastersRequest request
        ) =>
            new DescribeFormModelMastersTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.DescribeFormModelMastersResult>();
    #else
		public DescribeFormModelMastersTask DescribeFormModelMastersAsync(
                Request.DescribeFormModelMastersRequest request
        )
		{
			return new DescribeFormModelMastersTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.DescribeFormModelMastersResult> DescribeFormModelMastersAsync(
                Request.DescribeFormModelMastersRequest request
        ) =>
            new DescribeFormModelMastersTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class CreateFormModelMasterTask : Gs2RestSessionTask<CreateFormModelMasterRequest, CreateFormModelMasterResult>
        {
            public CreateFormModelMasterTask(IGs2Session session, RestSessionRequestFactory factory, CreateFormModelMasterRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(CreateFormModelMasterRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "formation")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/master/model/form";

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
                if (request.Slots != null)
                {
                    jsonWriter.WritePropertyName("slots");
                    jsonWriter.WriteArrayStart();
                    foreach(var item in request.Slots)
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
		public IEnumerator CreateFormModelMaster(
                Request.CreateFormModelMasterRequest request,
                UnityAction<AsyncResult<Result.CreateFormModelMasterResult>> callback
        ) =>
            new CreateFormModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.CreateFormModelMasterResult> CreateFormModelMasterFuture(
                Request.CreateFormModelMasterRequest request
        ) =>
            new CreateFormModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.CreateFormModelMasterResult> CreateFormModelMasterAsync(
                Request.CreateFormModelMasterRequest request
        ) =>
            new CreateFormModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.CreateFormModelMasterResult>();
    #else
		public CreateFormModelMasterTask CreateFormModelMasterAsync(
                Request.CreateFormModelMasterRequest request
        )
		{
			return new CreateFormModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.CreateFormModelMasterResult> CreateFormModelMasterAsync(
                Request.CreateFormModelMasterRequest request
        ) =>
            new CreateFormModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class GetFormModelMasterTask : Gs2RestSessionTask<GetFormModelMasterRequest, GetFormModelMasterResult>
        {
            public GetFormModelMasterTask(IGs2Session session, RestSessionRequestFactory factory, GetFormModelMasterRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(GetFormModelMasterRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "formation")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/master/model/form/{formModelName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{formModelName}", !string.IsNullOrEmpty(request.FormModelName) ? request.FormModelName.ToString() : "null");

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
		public IEnumerator GetFormModelMaster(
                Request.GetFormModelMasterRequest request,
                UnityAction<AsyncResult<Result.GetFormModelMasterResult>> callback
        ) =>
            new GetFormModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.GetFormModelMasterResult> GetFormModelMasterFuture(
                Request.GetFormModelMasterRequest request
        ) =>
            new GetFormModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.GetFormModelMasterResult> GetFormModelMasterAsync(
                Request.GetFormModelMasterRequest request
        ) =>
            new GetFormModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.GetFormModelMasterResult>();
    #else
		public GetFormModelMasterTask GetFormModelMasterAsync(
                Request.GetFormModelMasterRequest request
        )
		{
			return new GetFormModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.GetFormModelMasterResult> GetFormModelMasterAsync(
                Request.GetFormModelMasterRequest request
        ) =>
            new GetFormModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class UpdateFormModelMasterTask : Gs2RestSessionTask<UpdateFormModelMasterRequest, UpdateFormModelMasterResult>
        {
            public UpdateFormModelMasterTask(IGs2Session session, RestSessionRequestFactory factory, UpdateFormModelMasterRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(UpdateFormModelMasterRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "formation")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/master/model/form/{formModelName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{formModelName}", !string.IsNullOrEmpty(request.FormModelName) ? request.FormModelName.ToString() : "null");

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
                if (request.Slots != null)
                {
                    jsonWriter.WritePropertyName("slots");
                    jsonWriter.WriteArrayStart();
                    foreach(var item in request.Slots)
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
		public IEnumerator UpdateFormModelMaster(
                Request.UpdateFormModelMasterRequest request,
                UnityAction<AsyncResult<Result.UpdateFormModelMasterResult>> callback
        ) =>
            new UpdateFormModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.UpdateFormModelMasterResult> UpdateFormModelMasterFuture(
                Request.UpdateFormModelMasterRequest request
        ) =>
            new UpdateFormModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.UpdateFormModelMasterResult> UpdateFormModelMasterAsync(
                Request.UpdateFormModelMasterRequest request
        ) =>
            new UpdateFormModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.UpdateFormModelMasterResult>();
    #else
		public UpdateFormModelMasterTask UpdateFormModelMasterAsync(
                Request.UpdateFormModelMasterRequest request
        )
		{
			return new UpdateFormModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.UpdateFormModelMasterResult> UpdateFormModelMasterAsync(
                Request.UpdateFormModelMasterRequest request
        ) =>
            new UpdateFormModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class DeleteFormModelMasterTask : Gs2RestSessionTask<DeleteFormModelMasterRequest, DeleteFormModelMasterResult>
        {
            public DeleteFormModelMasterTask(IGs2Session session, RestSessionRequestFactory factory, DeleteFormModelMasterRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DeleteFormModelMasterRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "formation")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/master/model/form/{formModelName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{formModelName}", !string.IsNullOrEmpty(request.FormModelName) ? request.FormModelName.ToString() : "null");

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
		public IEnumerator DeleteFormModelMaster(
                Request.DeleteFormModelMasterRequest request,
                UnityAction<AsyncResult<Result.DeleteFormModelMasterResult>> callback
        ) =>
            new DeleteFormModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.DeleteFormModelMasterResult> DeleteFormModelMasterFuture(
                Request.DeleteFormModelMasterRequest request
        ) =>
            new DeleteFormModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.DeleteFormModelMasterResult> DeleteFormModelMasterAsync(
                Request.DeleteFormModelMasterRequest request
        ) =>
            new DeleteFormModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.DeleteFormModelMasterResult>();
    #else
		public DeleteFormModelMasterTask DeleteFormModelMasterAsync(
                Request.DeleteFormModelMasterRequest request
        )
		{
			return new DeleteFormModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.DeleteFormModelMasterResult> DeleteFormModelMasterAsync(
                Request.DeleteFormModelMasterRequest request
        ) =>
            new DeleteFormModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class DescribeMoldModelsTask : Gs2RestSessionTask<DescribeMoldModelsRequest, DescribeMoldModelsResult>
        {
            public DescribeMoldModelsTask(IGs2Session session, RestSessionRequestFactory factory, DescribeMoldModelsRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DescribeMoldModelsRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "formation")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/model/mold";

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
		public IEnumerator DescribeMoldModels(
                Request.DescribeMoldModelsRequest request,
                UnityAction<AsyncResult<Result.DescribeMoldModelsResult>> callback
        ) =>
            new DescribeMoldModelsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.DescribeMoldModelsResult> DescribeMoldModelsFuture(
                Request.DescribeMoldModelsRequest request
        ) =>
            new DescribeMoldModelsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.DescribeMoldModelsResult> DescribeMoldModelsAsync(
                Request.DescribeMoldModelsRequest request
        ) =>
            new DescribeMoldModelsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.DescribeMoldModelsResult>();
    #else
		public DescribeMoldModelsTask DescribeMoldModelsAsync(
                Request.DescribeMoldModelsRequest request
        )
		{
			return new DescribeMoldModelsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.DescribeMoldModelsResult> DescribeMoldModelsAsync(
                Request.DescribeMoldModelsRequest request
        ) =>
            new DescribeMoldModelsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class GetMoldModelTask : Gs2RestSessionTask<GetMoldModelRequest, GetMoldModelResult>
        {
            public GetMoldModelTask(IGs2Session session, RestSessionRequestFactory factory, GetMoldModelRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(GetMoldModelRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "formation")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/model/mold/{moldModelName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{moldModelName}", !string.IsNullOrEmpty(request.MoldModelName) ? request.MoldModelName.ToString() : "null");

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
		public IEnumerator GetMoldModel(
                Request.GetMoldModelRequest request,
                UnityAction<AsyncResult<Result.GetMoldModelResult>> callback
        ) =>
            new GetMoldModelTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.GetMoldModelResult> GetMoldModelFuture(
                Request.GetMoldModelRequest request
        ) =>
            new GetMoldModelTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.GetMoldModelResult> GetMoldModelAsync(
                Request.GetMoldModelRequest request
        ) =>
            new GetMoldModelTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.GetMoldModelResult>();
    #else
		public GetMoldModelTask GetMoldModelAsync(
                Request.GetMoldModelRequest request
        )
		{
			return new GetMoldModelTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.GetMoldModelResult> GetMoldModelAsync(
                Request.GetMoldModelRequest request
        ) =>
            new GetMoldModelTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class DescribeMoldModelMastersTask : Gs2RestSessionTask<DescribeMoldModelMastersRequest, DescribeMoldModelMastersResult>
        {
            public DescribeMoldModelMastersTask(IGs2Session session, RestSessionRequestFactory factory, DescribeMoldModelMastersRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DescribeMoldModelMastersRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "formation")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/master/model/mold";

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
		public IEnumerator DescribeMoldModelMasters(
                Request.DescribeMoldModelMastersRequest request,
                UnityAction<AsyncResult<Result.DescribeMoldModelMastersResult>> callback
        ) =>
            new DescribeMoldModelMastersTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.DescribeMoldModelMastersResult> DescribeMoldModelMastersFuture(
                Request.DescribeMoldModelMastersRequest request
        ) =>
            new DescribeMoldModelMastersTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.DescribeMoldModelMastersResult> DescribeMoldModelMastersAsync(
                Request.DescribeMoldModelMastersRequest request
        ) =>
            new DescribeMoldModelMastersTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.DescribeMoldModelMastersResult>();
    #else
		public DescribeMoldModelMastersTask DescribeMoldModelMastersAsync(
                Request.DescribeMoldModelMastersRequest request
        )
		{
			return new DescribeMoldModelMastersTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.DescribeMoldModelMastersResult> DescribeMoldModelMastersAsync(
                Request.DescribeMoldModelMastersRequest request
        ) =>
            new DescribeMoldModelMastersTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class CreateMoldModelMasterTask : Gs2RestSessionTask<CreateMoldModelMasterRequest, CreateMoldModelMasterResult>
        {
            public CreateMoldModelMasterTask(IGs2Session session, RestSessionRequestFactory factory, CreateMoldModelMasterRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(CreateMoldModelMasterRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "formation")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/master/model/mold";

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
                if (request.FormModelName != null)
                {
                    jsonWriter.WritePropertyName("formModelName");
                    jsonWriter.Write(request.FormModelName);
                }
                if (request.InitialMaxCapacity != null)
                {
                    jsonWriter.WritePropertyName("initialMaxCapacity");
                    jsonWriter.Write(request.InitialMaxCapacity.ToString());
                }
                if (request.MaxCapacity != null)
                {
                    jsonWriter.WritePropertyName("maxCapacity");
                    jsonWriter.Write(request.MaxCapacity.ToString());
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
		public IEnumerator CreateMoldModelMaster(
                Request.CreateMoldModelMasterRequest request,
                UnityAction<AsyncResult<Result.CreateMoldModelMasterResult>> callback
        ) =>
            new CreateMoldModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.CreateMoldModelMasterResult> CreateMoldModelMasterFuture(
                Request.CreateMoldModelMasterRequest request
        ) =>
            new CreateMoldModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.CreateMoldModelMasterResult> CreateMoldModelMasterAsync(
                Request.CreateMoldModelMasterRequest request
        ) =>
            new CreateMoldModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.CreateMoldModelMasterResult>();
    #else
		public CreateMoldModelMasterTask CreateMoldModelMasterAsync(
                Request.CreateMoldModelMasterRequest request
        )
		{
			return new CreateMoldModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.CreateMoldModelMasterResult> CreateMoldModelMasterAsync(
                Request.CreateMoldModelMasterRequest request
        ) =>
            new CreateMoldModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class GetMoldModelMasterTask : Gs2RestSessionTask<GetMoldModelMasterRequest, GetMoldModelMasterResult>
        {
            public GetMoldModelMasterTask(IGs2Session session, RestSessionRequestFactory factory, GetMoldModelMasterRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(GetMoldModelMasterRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "formation")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/master/model/mold/{moldModelName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{moldModelName}", !string.IsNullOrEmpty(request.MoldModelName) ? request.MoldModelName.ToString() : "null");

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
		public IEnumerator GetMoldModelMaster(
                Request.GetMoldModelMasterRequest request,
                UnityAction<AsyncResult<Result.GetMoldModelMasterResult>> callback
        ) =>
            new GetMoldModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.GetMoldModelMasterResult> GetMoldModelMasterFuture(
                Request.GetMoldModelMasterRequest request
        ) =>
            new GetMoldModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.GetMoldModelMasterResult> GetMoldModelMasterAsync(
                Request.GetMoldModelMasterRequest request
        ) =>
            new GetMoldModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.GetMoldModelMasterResult>();
    #else
		public GetMoldModelMasterTask GetMoldModelMasterAsync(
                Request.GetMoldModelMasterRequest request
        )
		{
			return new GetMoldModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.GetMoldModelMasterResult> GetMoldModelMasterAsync(
                Request.GetMoldModelMasterRequest request
        ) =>
            new GetMoldModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class UpdateMoldModelMasterTask : Gs2RestSessionTask<UpdateMoldModelMasterRequest, UpdateMoldModelMasterResult>
        {
            public UpdateMoldModelMasterTask(IGs2Session session, RestSessionRequestFactory factory, UpdateMoldModelMasterRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(UpdateMoldModelMasterRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "formation")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/master/model/mold/{moldModelName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{moldModelName}", !string.IsNullOrEmpty(request.MoldModelName) ? request.MoldModelName.ToString() : "null");

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
                if (request.FormModelName != null)
                {
                    jsonWriter.WritePropertyName("formModelName");
                    jsonWriter.Write(request.FormModelName);
                }
                if (request.InitialMaxCapacity != null)
                {
                    jsonWriter.WritePropertyName("initialMaxCapacity");
                    jsonWriter.Write(request.InitialMaxCapacity.ToString());
                }
                if (request.MaxCapacity != null)
                {
                    jsonWriter.WritePropertyName("maxCapacity");
                    jsonWriter.Write(request.MaxCapacity.ToString());
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
		public IEnumerator UpdateMoldModelMaster(
                Request.UpdateMoldModelMasterRequest request,
                UnityAction<AsyncResult<Result.UpdateMoldModelMasterResult>> callback
        ) =>
            new UpdateMoldModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.UpdateMoldModelMasterResult> UpdateMoldModelMasterFuture(
                Request.UpdateMoldModelMasterRequest request
        ) =>
            new UpdateMoldModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.UpdateMoldModelMasterResult> UpdateMoldModelMasterAsync(
                Request.UpdateMoldModelMasterRequest request
        ) =>
            new UpdateMoldModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.UpdateMoldModelMasterResult>();
    #else
		public UpdateMoldModelMasterTask UpdateMoldModelMasterAsync(
                Request.UpdateMoldModelMasterRequest request
        )
		{
			return new UpdateMoldModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.UpdateMoldModelMasterResult> UpdateMoldModelMasterAsync(
                Request.UpdateMoldModelMasterRequest request
        ) =>
            new UpdateMoldModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class DeleteMoldModelMasterTask : Gs2RestSessionTask<DeleteMoldModelMasterRequest, DeleteMoldModelMasterResult>
        {
            public DeleteMoldModelMasterTask(IGs2Session session, RestSessionRequestFactory factory, DeleteMoldModelMasterRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DeleteMoldModelMasterRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "formation")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/master/model/mold/{moldModelName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{moldModelName}", !string.IsNullOrEmpty(request.MoldModelName) ? request.MoldModelName.ToString() : "null");

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
		public IEnumerator DeleteMoldModelMaster(
                Request.DeleteMoldModelMasterRequest request,
                UnityAction<AsyncResult<Result.DeleteMoldModelMasterResult>> callback
        ) =>
            new DeleteMoldModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.DeleteMoldModelMasterResult> DeleteMoldModelMasterFuture(
                Request.DeleteMoldModelMasterRequest request
        ) =>
            new DeleteMoldModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.DeleteMoldModelMasterResult> DeleteMoldModelMasterAsync(
                Request.DeleteMoldModelMasterRequest request
        ) =>
            new DeleteMoldModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.DeleteMoldModelMasterResult>();
    #else
		public DeleteMoldModelMasterTask DeleteMoldModelMasterAsync(
                Request.DeleteMoldModelMasterRequest request
        )
		{
			return new DeleteMoldModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.DeleteMoldModelMasterResult> DeleteMoldModelMasterAsync(
                Request.DeleteMoldModelMasterRequest request
        ) =>
            new DeleteMoldModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class DescribePropertyFormModelsTask : Gs2RestSessionTask<DescribePropertyFormModelsRequest, DescribePropertyFormModelsResult>
        {
            public DescribePropertyFormModelsTask(IGs2Session session, RestSessionRequestFactory factory, DescribePropertyFormModelsRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DescribePropertyFormModelsRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "formation")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/model/propertyForm";

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
		public IEnumerator DescribePropertyFormModels(
                Request.DescribePropertyFormModelsRequest request,
                UnityAction<AsyncResult<Result.DescribePropertyFormModelsResult>> callback
        ) =>
            new DescribePropertyFormModelsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.DescribePropertyFormModelsResult> DescribePropertyFormModelsFuture(
                Request.DescribePropertyFormModelsRequest request
        ) =>
            new DescribePropertyFormModelsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.DescribePropertyFormModelsResult> DescribePropertyFormModelsAsync(
                Request.DescribePropertyFormModelsRequest request
        ) =>
            new DescribePropertyFormModelsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.DescribePropertyFormModelsResult>();
    #else
		public DescribePropertyFormModelsTask DescribePropertyFormModelsAsync(
                Request.DescribePropertyFormModelsRequest request
        )
		{
			return new DescribePropertyFormModelsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.DescribePropertyFormModelsResult> DescribePropertyFormModelsAsync(
                Request.DescribePropertyFormModelsRequest request
        ) =>
            new DescribePropertyFormModelsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class GetPropertyFormModelTask : Gs2RestSessionTask<GetPropertyFormModelRequest, GetPropertyFormModelResult>
        {
            public GetPropertyFormModelTask(IGs2Session session, RestSessionRequestFactory factory, GetPropertyFormModelRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(GetPropertyFormModelRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "formation")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/model/propertyForm/{propertyFormModelName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{propertyFormModelName}", !string.IsNullOrEmpty(request.PropertyFormModelName) ? request.PropertyFormModelName.ToString() : "null");

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
		public IEnumerator GetPropertyFormModel(
                Request.GetPropertyFormModelRequest request,
                UnityAction<AsyncResult<Result.GetPropertyFormModelResult>> callback
        ) =>
            new GetPropertyFormModelTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.GetPropertyFormModelResult> GetPropertyFormModelFuture(
                Request.GetPropertyFormModelRequest request
        ) =>
            new GetPropertyFormModelTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.GetPropertyFormModelResult> GetPropertyFormModelAsync(
                Request.GetPropertyFormModelRequest request
        ) =>
            new GetPropertyFormModelTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.GetPropertyFormModelResult>();
    #else
		public GetPropertyFormModelTask GetPropertyFormModelAsync(
                Request.GetPropertyFormModelRequest request
        )
		{
			return new GetPropertyFormModelTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.GetPropertyFormModelResult> GetPropertyFormModelAsync(
                Request.GetPropertyFormModelRequest request
        ) =>
            new GetPropertyFormModelTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class DescribePropertyFormModelMastersTask : Gs2RestSessionTask<DescribePropertyFormModelMastersRequest, DescribePropertyFormModelMastersResult>
        {
            public DescribePropertyFormModelMastersTask(IGs2Session session, RestSessionRequestFactory factory, DescribePropertyFormModelMastersRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DescribePropertyFormModelMastersRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "formation")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/master/model/propertyForm";

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
		public IEnumerator DescribePropertyFormModelMasters(
                Request.DescribePropertyFormModelMastersRequest request,
                UnityAction<AsyncResult<Result.DescribePropertyFormModelMastersResult>> callback
        ) =>
            new DescribePropertyFormModelMastersTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.DescribePropertyFormModelMastersResult> DescribePropertyFormModelMastersFuture(
                Request.DescribePropertyFormModelMastersRequest request
        ) =>
            new DescribePropertyFormModelMastersTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.DescribePropertyFormModelMastersResult> DescribePropertyFormModelMastersAsync(
                Request.DescribePropertyFormModelMastersRequest request
        ) =>
            new DescribePropertyFormModelMastersTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.DescribePropertyFormModelMastersResult>();
    #else
		public DescribePropertyFormModelMastersTask DescribePropertyFormModelMastersAsync(
                Request.DescribePropertyFormModelMastersRequest request
        )
		{
			return new DescribePropertyFormModelMastersTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.DescribePropertyFormModelMastersResult> DescribePropertyFormModelMastersAsync(
                Request.DescribePropertyFormModelMastersRequest request
        ) =>
            new DescribePropertyFormModelMastersTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class CreatePropertyFormModelMasterTask : Gs2RestSessionTask<CreatePropertyFormModelMasterRequest, CreatePropertyFormModelMasterResult>
        {
            public CreatePropertyFormModelMasterTask(IGs2Session session, RestSessionRequestFactory factory, CreatePropertyFormModelMasterRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(CreatePropertyFormModelMasterRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "formation")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/master/model/propertyForm";

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
                if (request.Slots != null)
                {
                    jsonWriter.WritePropertyName("slots");
                    jsonWriter.WriteArrayStart();
                    foreach(var item in request.Slots)
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
		public IEnumerator CreatePropertyFormModelMaster(
                Request.CreatePropertyFormModelMasterRequest request,
                UnityAction<AsyncResult<Result.CreatePropertyFormModelMasterResult>> callback
        ) =>
            new CreatePropertyFormModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.CreatePropertyFormModelMasterResult> CreatePropertyFormModelMasterFuture(
                Request.CreatePropertyFormModelMasterRequest request
        ) =>
            new CreatePropertyFormModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.CreatePropertyFormModelMasterResult> CreatePropertyFormModelMasterAsync(
                Request.CreatePropertyFormModelMasterRequest request
        ) =>
            new CreatePropertyFormModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.CreatePropertyFormModelMasterResult>();
    #else
		public CreatePropertyFormModelMasterTask CreatePropertyFormModelMasterAsync(
                Request.CreatePropertyFormModelMasterRequest request
        )
		{
			return new CreatePropertyFormModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.CreatePropertyFormModelMasterResult> CreatePropertyFormModelMasterAsync(
                Request.CreatePropertyFormModelMasterRequest request
        ) =>
            new CreatePropertyFormModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class GetPropertyFormModelMasterTask : Gs2RestSessionTask<GetPropertyFormModelMasterRequest, GetPropertyFormModelMasterResult>
        {
            public GetPropertyFormModelMasterTask(IGs2Session session, RestSessionRequestFactory factory, GetPropertyFormModelMasterRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(GetPropertyFormModelMasterRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "formation")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/master/model/propertyForm/{propertyFormModelName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{propertyFormModelName}", !string.IsNullOrEmpty(request.PropertyFormModelName) ? request.PropertyFormModelName.ToString() : "null");

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
		public IEnumerator GetPropertyFormModelMaster(
                Request.GetPropertyFormModelMasterRequest request,
                UnityAction<AsyncResult<Result.GetPropertyFormModelMasterResult>> callback
        ) =>
            new GetPropertyFormModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.GetPropertyFormModelMasterResult> GetPropertyFormModelMasterFuture(
                Request.GetPropertyFormModelMasterRequest request
        ) =>
            new GetPropertyFormModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.GetPropertyFormModelMasterResult> GetPropertyFormModelMasterAsync(
                Request.GetPropertyFormModelMasterRequest request
        ) =>
            new GetPropertyFormModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.GetPropertyFormModelMasterResult>();
    #else
		public GetPropertyFormModelMasterTask GetPropertyFormModelMasterAsync(
                Request.GetPropertyFormModelMasterRequest request
        )
		{
			return new GetPropertyFormModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.GetPropertyFormModelMasterResult> GetPropertyFormModelMasterAsync(
                Request.GetPropertyFormModelMasterRequest request
        ) =>
            new GetPropertyFormModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class UpdatePropertyFormModelMasterTask : Gs2RestSessionTask<UpdatePropertyFormModelMasterRequest, UpdatePropertyFormModelMasterResult>
        {
            public UpdatePropertyFormModelMasterTask(IGs2Session session, RestSessionRequestFactory factory, UpdatePropertyFormModelMasterRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(UpdatePropertyFormModelMasterRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "formation")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/master/model/propertyForm/{propertyFormModelName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{propertyFormModelName}", !string.IsNullOrEmpty(request.PropertyFormModelName) ? request.PropertyFormModelName.ToString() : "null");

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
                if (request.Slots != null)
                {
                    jsonWriter.WritePropertyName("slots");
                    jsonWriter.WriteArrayStart();
                    foreach(var item in request.Slots)
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
		public IEnumerator UpdatePropertyFormModelMaster(
                Request.UpdatePropertyFormModelMasterRequest request,
                UnityAction<AsyncResult<Result.UpdatePropertyFormModelMasterResult>> callback
        ) =>
            new UpdatePropertyFormModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.UpdatePropertyFormModelMasterResult> UpdatePropertyFormModelMasterFuture(
                Request.UpdatePropertyFormModelMasterRequest request
        ) =>
            new UpdatePropertyFormModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.UpdatePropertyFormModelMasterResult> UpdatePropertyFormModelMasterAsync(
                Request.UpdatePropertyFormModelMasterRequest request
        ) =>
            new UpdatePropertyFormModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.UpdatePropertyFormModelMasterResult>();
    #else
		public UpdatePropertyFormModelMasterTask UpdatePropertyFormModelMasterAsync(
                Request.UpdatePropertyFormModelMasterRequest request
        )
		{
			return new UpdatePropertyFormModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.UpdatePropertyFormModelMasterResult> UpdatePropertyFormModelMasterAsync(
                Request.UpdatePropertyFormModelMasterRequest request
        ) =>
            new UpdatePropertyFormModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class DeletePropertyFormModelMasterTask : Gs2RestSessionTask<DeletePropertyFormModelMasterRequest, DeletePropertyFormModelMasterResult>
        {
            public DeletePropertyFormModelMasterTask(IGs2Session session, RestSessionRequestFactory factory, DeletePropertyFormModelMasterRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DeletePropertyFormModelMasterRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "formation")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/master/model/propertyForm/{propertyFormModelName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{propertyFormModelName}", !string.IsNullOrEmpty(request.PropertyFormModelName) ? request.PropertyFormModelName.ToString() : "null");

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
		public IEnumerator DeletePropertyFormModelMaster(
                Request.DeletePropertyFormModelMasterRequest request,
                UnityAction<AsyncResult<Result.DeletePropertyFormModelMasterResult>> callback
        ) =>
            new DeletePropertyFormModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.DeletePropertyFormModelMasterResult> DeletePropertyFormModelMasterFuture(
                Request.DeletePropertyFormModelMasterRequest request
        ) =>
            new DeletePropertyFormModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.DeletePropertyFormModelMasterResult> DeletePropertyFormModelMasterAsync(
                Request.DeletePropertyFormModelMasterRequest request
        ) =>
            new DeletePropertyFormModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.DeletePropertyFormModelMasterResult>();
    #else
		public DeletePropertyFormModelMasterTask DeletePropertyFormModelMasterAsync(
                Request.DeletePropertyFormModelMasterRequest request
        )
		{
			return new DeletePropertyFormModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.DeletePropertyFormModelMasterResult> DeletePropertyFormModelMasterAsync(
                Request.DeletePropertyFormModelMasterRequest request
        ) =>
            new DeletePropertyFormModelMasterTask(
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
                    .Replace("{service}", "formation")
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


        public class GetCurrentFormMasterTask : Gs2RestSessionTask<GetCurrentFormMasterRequest, GetCurrentFormMasterResult>
        {
            public GetCurrentFormMasterTask(IGs2Session session, RestSessionRequestFactory factory, GetCurrentFormMasterRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(GetCurrentFormMasterRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "formation")
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
		public IEnumerator GetCurrentFormMaster(
                Request.GetCurrentFormMasterRequest request,
                UnityAction<AsyncResult<Result.GetCurrentFormMasterResult>> callback
        ) =>
            new GetCurrentFormMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.GetCurrentFormMasterResult> GetCurrentFormMasterFuture(
                Request.GetCurrentFormMasterRequest request
        ) =>
            new GetCurrentFormMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.GetCurrentFormMasterResult> GetCurrentFormMasterAsync(
                Request.GetCurrentFormMasterRequest request
        ) =>
            new GetCurrentFormMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.GetCurrentFormMasterResult>();
    #else
		public GetCurrentFormMasterTask GetCurrentFormMasterAsync(
                Request.GetCurrentFormMasterRequest request
        )
		{
			return new GetCurrentFormMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.GetCurrentFormMasterResult> GetCurrentFormMasterAsync(
                Request.GetCurrentFormMasterRequest request
        ) =>
            new GetCurrentFormMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class PreUpdateCurrentFormMasterTask : Gs2RestSessionTask<PreUpdateCurrentFormMasterRequest, PreUpdateCurrentFormMasterResult>
        {
            public PreUpdateCurrentFormMasterTask(IGs2Session session, RestSessionRequestFactory factory, PreUpdateCurrentFormMasterRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(PreUpdateCurrentFormMasterRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "formation")
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
		public IEnumerator PreUpdateCurrentFormMaster(
                Request.PreUpdateCurrentFormMasterRequest request,
                UnityAction<AsyncResult<Result.PreUpdateCurrentFormMasterResult>> callback
        ) =>
            new PreUpdateCurrentFormMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.PreUpdateCurrentFormMasterResult> PreUpdateCurrentFormMasterFuture(
                Request.PreUpdateCurrentFormMasterRequest request
        ) =>
            new PreUpdateCurrentFormMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.PreUpdateCurrentFormMasterResult> PreUpdateCurrentFormMasterAsync(
                Request.PreUpdateCurrentFormMasterRequest request
        ) =>
            new PreUpdateCurrentFormMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.PreUpdateCurrentFormMasterResult>();
    #else
		public PreUpdateCurrentFormMasterTask PreUpdateCurrentFormMasterAsync(
                Request.PreUpdateCurrentFormMasterRequest request
        )
		{
			return new PreUpdateCurrentFormMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.PreUpdateCurrentFormMasterResult> PreUpdateCurrentFormMasterAsync(
                Request.PreUpdateCurrentFormMasterRequest request
        ) =>
            new PreUpdateCurrentFormMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class UpdateCurrentFormMasterTask : Gs2RestSessionTask<UpdateCurrentFormMasterRequest, UpdateCurrentFormMasterResult>
        {
            public UpdateCurrentFormMasterTask(IGs2Session session, RestSessionRequestFactory factory, UpdateCurrentFormMasterRequest request) : base(session, factory, request)
            {
            }
#if GS2_ENABLE_UNITASK
            protected override async UniTask<UpdateCurrentFormMasterResult> InvokeImpl()
#else
            protected override async Task<UpdateCurrentFormMasterResult> InvokeImpl()
#endif
            {
                if (Request.Settings != null) {
                    var preTask = new PreUpdateCurrentFormMasterTask(
                        Session,
                        Factory,
                        new PreUpdateCurrentFormMasterRequest()
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

            protected override IGs2SessionRequest CreateRequest(UpdateCurrentFormMasterRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "formation")
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
		public IEnumerator UpdateCurrentFormMaster(
                Request.UpdateCurrentFormMasterRequest request,
                UnityAction<AsyncResult<Result.UpdateCurrentFormMasterResult>> callback
        ) =>
            new UpdateCurrentFormMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.UpdateCurrentFormMasterResult> UpdateCurrentFormMasterFuture(
                Request.UpdateCurrentFormMasterRequest request
        ) =>
            new UpdateCurrentFormMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.UpdateCurrentFormMasterResult> UpdateCurrentFormMasterAsync(
                Request.UpdateCurrentFormMasterRequest request
        ) =>
            new UpdateCurrentFormMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.UpdateCurrentFormMasterResult>();
    #else
		public UpdateCurrentFormMasterTask UpdateCurrentFormMasterAsync(
                Request.UpdateCurrentFormMasterRequest request
        )
		{
			return new UpdateCurrentFormMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.UpdateCurrentFormMasterResult> UpdateCurrentFormMasterAsync(
                Request.UpdateCurrentFormMasterRequest request
        ) =>
            new UpdateCurrentFormMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class UpdateCurrentFormMasterFromGitHubTask : Gs2RestSessionTask<UpdateCurrentFormMasterFromGitHubRequest, UpdateCurrentFormMasterFromGitHubResult>
        {
            public UpdateCurrentFormMasterFromGitHubTask(IGs2Session session, RestSessionRequestFactory factory, UpdateCurrentFormMasterFromGitHubRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(UpdateCurrentFormMasterFromGitHubRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "formation")
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
		public IEnumerator UpdateCurrentFormMasterFromGitHub(
                Request.UpdateCurrentFormMasterFromGitHubRequest request,
                UnityAction<AsyncResult<Result.UpdateCurrentFormMasterFromGitHubResult>> callback
        ) =>
            new UpdateCurrentFormMasterFromGitHubTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.UpdateCurrentFormMasterFromGitHubResult> UpdateCurrentFormMasterFromGitHubFuture(
                Request.UpdateCurrentFormMasterFromGitHubRequest request
        ) =>
            new UpdateCurrentFormMasterFromGitHubTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.UpdateCurrentFormMasterFromGitHubResult> UpdateCurrentFormMasterFromGitHubAsync(
                Request.UpdateCurrentFormMasterFromGitHubRequest request
        ) =>
            new UpdateCurrentFormMasterFromGitHubTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.UpdateCurrentFormMasterFromGitHubResult>();
    #else
		public UpdateCurrentFormMasterFromGitHubTask UpdateCurrentFormMasterFromGitHubAsync(
                Request.UpdateCurrentFormMasterFromGitHubRequest request
        )
		{
			return new UpdateCurrentFormMasterFromGitHubTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.UpdateCurrentFormMasterFromGitHubResult> UpdateCurrentFormMasterFromGitHubAsync(
                Request.UpdateCurrentFormMasterFromGitHubRequest request
        ) =>
            new UpdateCurrentFormMasterFromGitHubTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class DescribeMoldsTask : Gs2RestSessionTask<DescribeMoldsRequest, DescribeMoldsResult>
        {
            public DescribeMoldsTask(IGs2Session session, RestSessionRequestFactory factory, DescribeMoldsRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DescribeMoldsRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "formation")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/me/mold";

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
		public IEnumerator DescribeMolds(
                Request.DescribeMoldsRequest request,
                UnityAction<AsyncResult<Result.DescribeMoldsResult>> callback
        ) =>
            new DescribeMoldsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.DescribeMoldsResult> DescribeMoldsFuture(
                Request.DescribeMoldsRequest request
        ) =>
            new DescribeMoldsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.DescribeMoldsResult> DescribeMoldsAsync(
                Request.DescribeMoldsRequest request
        ) =>
            new DescribeMoldsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.DescribeMoldsResult>();
    #else
		public DescribeMoldsTask DescribeMoldsAsync(
                Request.DescribeMoldsRequest request
        )
		{
			return new DescribeMoldsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.DescribeMoldsResult> DescribeMoldsAsync(
                Request.DescribeMoldsRequest request
        ) =>
            new DescribeMoldsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class DescribeMoldsByUserIdTask : Gs2RestSessionTask<DescribeMoldsByUserIdRequest, DescribeMoldsByUserIdResult>
        {
            public DescribeMoldsByUserIdTask(IGs2Session session, RestSessionRequestFactory factory, DescribeMoldsByUserIdRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DescribeMoldsByUserIdRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "formation")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/{userId}/mold";

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
		public IEnumerator DescribeMoldsByUserId(
                Request.DescribeMoldsByUserIdRequest request,
                UnityAction<AsyncResult<Result.DescribeMoldsByUserIdResult>> callback
        ) =>
            new DescribeMoldsByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.DescribeMoldsByUserIdResult> DescribeMoldsByUserIdFuture(
                Request.DescribeMoldsByUserIdRequest request
        ) =>
            new DescribeMoldsByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.DescribeMoldsByUserIdResult> DescribeMoldsByUserIdAsync(
                Request.DescribeMoldsByUserIdRequest request
        ) =>
            new DescribeMoldsByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.DescribeMoldsByUserIdResult>();
    #else
		public DescribeMoldsByUserIdTask DescribeMoldsByUserIdAsync(
                Request.DescribeMoldsByUserIdRequest request
        )
		{
			return new DescribeMoldsByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.DescribeMoldsByUserIdResult> DescribeMoldsByUserIdAsync(
                Request.DescribeMoldsByUserIdRequest request
        ) =>
            new DescribeMoldsByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class GetMoldTask : Gs2RestSessionTask<GetMoldRequest, GetMoldResult>
        {
            public GetMoldTask(IGs2Session session, RestSessionRequestFactory factory, GetMoldRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(GetMoldRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "formation")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/me/mold/{moldModelName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{moldModelName}", !string.IsNullOrEmpty(request.MoldModelName) ? request.MoldModelName.ToString() : "null");

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
		public IEnumerator GetMold(
                Request.GetMoldRequest request,
                UnityAction<AsyncResult<Result.GetMoldResult>> callback
        ) =>
            new GetMoldTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.GetMoldResult> GetMoldFuture(
                Request.GetMoldRequest request
        ) =>
            new GetMoldTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.GetMoldResult> GetMoldAsync(
                Request.GetMoldRequest request
        ) =>
            new GetMoldTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.GetMoldResult>();
    #else
		public GetMoldTask GetMoldAsync(
                Request.GetMoldRequest request
        )
		{
			return new GetMoldTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.GetMoldResult> GetMoldAsync(
                Request.GetMoldRequest request
        ) =>
            new GetMoldTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class GetMoldByUserIdTask : Gs2RestSessionTask<GetMoldByUserIdRequest, GetMoldByUserIdResult>
        {
            public GetMoldByUserIdTask(IGs2Session session, RestSessionRequestFactory factory, GetMoldByUserIdRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(GetMoldByUserIdRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "formation")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/{userId}/mold/{moldModelName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{userId}", !string.IsNullOrEmpty(request.UserId) ? request.UserId.ToString() : "null");
                url = url.Replace("{moldModelName}", !string.IsNullOrEmpty(request.MoldModelName) ? request.MoldModelName.ToString() : "null");

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
		public IEnumerator GetMoldByUserId(
                Request.GetMoldByUserIdRequest request,
                UnityAction<AsyncResult<Result.GetMoldByUserIdResult>> callback
        ) =>
            new GetMoldByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.GetMoldByUserIdResult> GetMoldByUserIdFuture(
                Request.GetMoldByUserIdRequest request
        ) =>
            new GetMoldByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.GetMoldByUserIdResult> GetMoldByUserIdAsync(
                Request.GetMoldByUserIdRequest request
        ) =>
            new GetMoldByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.GetMoldByUserIdResult>();
    #else
		public GetMoldByUserIdTask GetMoldByUserIdAsync(
                Request.GetMoldByUserIdRequest request
        )
		{
			return new GetMoldByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.GetMoldByUserIdResult> GetMoldByUserIdAsync(
                Request.GetMoldByUserIdRequest request
        ) =>
            new GetMoldByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class SetMoldCapacityByUserIdTask : Gs2RestSessionTask<SetMoldCapacityByUserIdRequest, SetMoldCapacityByUserIdResult>
        {
            public SetMoldCapacityByUserIdTask(IGs2Session session, RestSessionRequestFactory factory, SetMoldCapacityByUserIdRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(SetMoldCapacityByUserIdRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "formation")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/{userId}/mold/{moldModelName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{userId}", !string.IsNullOrEmpty(request.UserId) ? request.UserId.ToString() : "null");
                url = url.Replace("{moldModelName}", !string.IsNullOrEmpty(request.MoldModelName) ? request.MoldModelName.ToString() : "null");

                var sessionRequest = Factory.Put(url);

                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);
                jsonWriter.WriteObjectStart();
                if (request.Capacity != null)
                {
                    jsonWriter.WritePropertyName("capacity");
                    jsonWriter.Write(request.Capacity.ToString());
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
		public IEnumerator SetMoldCapacityByUserId(
                Request.SetMoldCapacityByUserIdRequest request,
                UnityAction<AsyncResult<Result.SetMoldCapacityByUserIdResult>> callback
        ) =>
            new SetMoldCapacityByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.SetMoldCapacityByUserIdResult> SetMoldCapacityByUserIdFuture(
                Request.SetMoldCapacityByUserIdRequest request
        ) =>
            new SetMoldCapacityByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.SetMoldCapacityByUserIdResult> SetMoldCapacityByUserIdAsync(
                Request.SetMoldCapacityByUserIdRequest request
        ) =>
            new SetMoldCapacityByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.SetMoldCapacityByUserIdResult>();
    #else
		public SetMoldCapacityByUserIdTask SetMoldCapacityByUserIdAsync(
                Request.SetMoldCapacityByUserIdRequest request
        )
		{
			return new SetMoldCapacityByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.SetMoldCapacityByUserIdResult> SetMoldCapacityByUserIdAsync(
                Request.SetMoldCapacityByUserIdRequest request
        ) =>
            new SetMoldCapacityByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class AddMoldCapacityByUserIdTask : Gs2RestSessionTask<AddMoldCapacityByUserIdRequest, AddMoldCapacityByUserIdResult>
        {
            public AddMoldCapacityByUserIdTask(IGs2Session session, RestSessionRequestFactory factory, AddMoldCapacityByUserIdRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(AddMoldCapacityByUserIdRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "formation")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/{userId}/mold/{moldModelName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{userId}", !string.IsNullOrEmpty(request.UserId) ? request.UserId.ToString() : "null");
                url = url.Replace("{moldModelName}", !string.IsNullOrEmpty(request.MoldModelName) ? request.MoldModelName.ToString() : "null");

                var sessionRequest = Factory.Post(url);

                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);
                jsonWriter.WriteObjectStart();
                if (request.Capacity != null)
                {
                    jsonWriter.WritePropertyName("capacity");
                    jsonWriter.Write(request.Capacity.ToString());
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
		public IEnumerator AddMoldCapacityByUserId(
                Request.AddMoldCapacityByUserIdRequest request,
                UnityAction<AsyncResult<Result.AddMoldCapacityByUserIdResult>> callback
        ) =>
            new AddMoldCapacityByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.AddMoldCapacityByUserIdResult> AddMoldCapacityByUserIdFuture(
                Request.AddMoldCapacityByUserIdRequest request
        ) =>
            new AddMoldCapacityByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.AddMoldCapacityByUserIdResult> AddMoldCapacityByUserIdAsync(
                Request.AddMoldCapacityByUserIdRequest request
        ) =>
            new AddMoldCapacityByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.AddMoldCapacityByUserIdResult>();
    #else
		public AddMoldCapacityByUserIdTask AddMoldCapacityByUserIdAsync(
                Request.AddMoldCapacityByUserIdRequest request
        )
		{
			return new AddMoldCapacityByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.AddMoldCapacityByUserIdResult> AddMoldCapacityByUserIdAsync(
                Request.AddMoldCapacityByUserIdRequest request
        ) =>
            new AddMoldCapacityByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class SubMoldCapacityTask : Gs2RestSessionTask<SubMoldCapacityRequest, SubMoldCapacityResult>
        {
            public SubMoldCapacityTask(IGs2Session session, RestSessionRequestFactory factory, SubMoldCapacityRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(SubMoldCapacityRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "formation")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/me/mold/{moldModelName}/sub";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{moldModelName}", !string.IsNullOrEmpty(request.MoldModelName) ? request.MoldModelName.ToString() : "null");

                var sessionRequest = Factory.Post(url);

                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);
                jsonWriter.WriteObjectStart();
                if (request.Capacity != null)
                {
                    jsonWriter.WritePropertyName("capacity");
                    jsonWriter.Write(request.Capacity.ToString());
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
		public IEnumerator SubMoldCapacity(
                Request.SubMoldCapacityRequest request,
                UnityAction<AsyncResult<Result.SubMoldCapacityResult>> callback
        ) =>
            new SubMoldCapacityTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.SubMoldCapacityResult> SubMoldCapacityFuture(
                Request.SubMoldCapacityRequest request
        ) =>
            new SubMoldCapacityTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.SubMoldCapacityResult> SubMoldCapacityAsync(
                Request.SubMoldCapacityRequest request
        ) =>
            new SubMoldCapacityTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.SubMoldCapacityResult>();
    #else
		public SubMoldCapacityTask SubMoldCapacityAsync(
                Request.SubMoldCapacityRequest request
        )
		{
			return new SubMoldCapacityTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.SubMoldCapacityResult> SubMoldCapacityAsync(
                Request.SubMoldCapacityRequest request
        ) =>
            new SubMoldCapacityTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class SubMoldCapacityByUserIdTask : Gs2RestSessionTask<SubMoldCapacityByUserIdRequest, SubMoldCapacityByUserIdResult>
        {
            public SubMoldCapacityByUserIdTask(IGs2Session session, RestSessionRequestFactory factory, SubMoldCapacityByUserIdRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(SubMoldCapacityByUserIdRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "formation")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/{userId}/mold/{moldModelName}/sub";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{userId}", !string.IsNullOrEmpty(request.UserId) ? request.UserId.ToString() : "null");
                url = url.Replace("{moldModelName}", !string.IsNullOrEmpty(request.MoldModelName) ? request.MoldModelName.ToString() : "null");

                var sessionRequest = Factory.Post(url);

                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);
                jsonWriter.WriteObjectStart();
                if (request.Capacity != null)
                {
                    jsonWriter.WritePropertyName("capacity");
                    jsonWriter.Write(request.Capacity.ToString());
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
		public IEnumerator SubMoldCapacityByUserId(
                Request.SubMoldCapacityByUserIdRequest request,
                UnityAction<AsyncResult<Result.SubMoldCapacityByUserIdResult>> callback
        ) =>
            new SubMoldCapacityByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.SubMoldCapacityByUserIdResult> SubMoldCapacityByUserIdFuture(
                Request.SubMoldCapacityByUserIdRequest request
        ) =>
            new SubMoldCapacityByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.SubMoldCapacityByUserIdResult> SubMoldCapacityByUserIdAsync(
                Request.SubMoldCapacityByUserIdRequest request
        ) =>
            new SubMoldCapacityByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.SubMoldCapacityByUserIdResult>();
    #else
		public SubMoldCapacityByUserIdTask SubMoldCapacityByUserIdAsync(
                Request.SubMoldCapacityByUserIdRequest request
        )
		{
			return new SubMoldCapacityByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.SubMoldCapacityByUserIdResult> SubMoldCapacityByUserIdAsync(
                Request.SubMoldCapacityByUserIdRequest request
        ) =>
            new SubMoldCapacityByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class DeleteMoldTask : Gs2RestSessionTask<DeleteMoldRequest, DeleteMoldResult>
        {
            public DeleteMoldTask(IGs2Session session, RestSessionRequestFactory factory, DeleteMoldRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DeleteMoldRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "formation")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/me/mold/{moldModelName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{moldModelName}", !string.IsNullOrEmpty(request.MoldModelName) ? request.MoldModelName.ToString() : "null");

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
		public IEnumerator DeleteMold(
                Request.DeleteMoldRequest request,
                UnityAction<AsyncResult<Result.DeleteMoldResult>> callback
        ) =>
            new DeleteMoldTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.DeleteMoldResult> DeleteMoldFuture(
                Request.DeleteMoldRequest request
        ) =>
            new DeleteMoldTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.DeleteMoldResult> DeleteMoldAsync(
                Request.DeleteMoldRequest request
        ) =>
            new DeleteMoldTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.DeleteMoldResult>();
    #else
		public DeleteMoldTask DeleteMoldAsync(
                Request.DeleteMoldRequest request
        )
		{
			return new DeleteMoldTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.DeleteMoldResult> DeleteMoldAsync(
                Request.DeleteMoldRequest request
        ) =>
            new DeleteMoldTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class DeleteMoldByUserIdTask : Gs2RestSessionTask<DeleteMoldByUserIdRequest, DeleteMoldByUserIdResult>
        {
            public DeleteMoldByUserIdTask(IGs2Session session, RestSessionRequestFactory factory, DeleteMoldByUserIdRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DeleteMoldByUserIdRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "formation")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/{userId}/mold/{moldModelName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{userId}", !string.IsNullOrEmpty(request.UserId) ? request.UserId.ToString() : "null");
                url = url.Replace("{moldModelName}", !string.IsNullOrEmpty(request.MoldModelName) ? request.MoldModelName.ToString() : "null");

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
		public IEnumerator DeleteMoldByUserId(
                Request.DeleteMoldByUserIdRequest request,
                UnityAction<AsyncResult<Result.DeleteMoldByUserIdResult>> callback
        ) =>
            new DeleteMoldByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.DeleteMoldByUserIdResult> DeleteMoldByUserIdFuture(
                Request.DeleteMoldByUserIdRequest request
        ) =>
            new DeleteMoldByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.DeleteMoldByUserIdResult> DeleteMoldByUserIdAsync(
                Request.DeleteMoldByUserIdRequest request
        ) =>
            new DeleteMoldByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.DeleteMoldByUserIdResult>();
    #else
		public DeleteMoldByUserIdTask DeleteMoldByUserIdAsync(
                Request.DeleteMoldByUserIdRequest request
        )
		{
			return new DeleteMoldByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.DeleteMoldByUserIdResult> DeleteMoldByUserIdAsync(
                Request.DeleteMoldByUserIdRequest request
        ) =>
            new DeleteMoldByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class AddCapacityByStampSheetTask : Gs2RestSessionTask<AddCapacityByStampSheetRequest, AddCapacityByStampSheetResult>
        {
            public AddCapacityByStampSheetTask(IGs2Session session, RestSessionRequestFactory factory, AddCapacityByStampSheetRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(AddCapacityByStampSheetRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "formation")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/stamp/mold/capacity/add";

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
		public IEnumerator AddCapacityByStampSheet(
                Request.AddCapacityByStampSheetRequest request,
                UnityAction<AsyncResult<Result.AddCapacityByStampSheetResult>> callback
        ) =>
            new AddCapacityByStampSheetTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.AddCapacityByStampSheetResult> AddCapacityByStampSheetFuture(
                Request.AddCapacityByStampSheetRequest request
        ) =>
            new AddCapacityByStampSheetTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.AddCapacityByStampSheetResult> AddCapacityByStampSheetAsync(
                Request.AddCapacityByStampSheetRequest request
        ) =>
            new AddCapacityByStampSheetTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.AddCapacityByStampSheetResult>();
    #else
		public AddCapacityByStampSheetTask AddCapacityByStampSheetAsync(
                Request.AddCapacityByStampSheetRequest request
        )
		{
			return new AddCapacityByStampSheetTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.AddCapacityByStampSheetResult> AddCapacityByStampSheetAsync(
                Request.AddCapacityByStampSheetRequest request
        ) =>
            new AddCapacityByStampSheetTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class SubCapacityByStampTaskTask : Gs2RestSessionTask<SubCapacityByStampTaskRequest, SubCapacityByStampTaskResult>
        {
            public SubCapacityByStampTaskTask(IGs2Session session, RestSessionRequestFactory factory, SubCapacityByStampTaskRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(SubCapacityByStampTaskRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "formation")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/stamp/mold/capacity/sub";

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
		public IEnumerator SubCapacityByStampTask(
                Request.SubCapacityByStampTaskRequest request,
                UnityAction<AsyncResult<Result.SubCapacityByStampTaskResult>> callback
        ) =>
            new SubCapacityByStampTaskTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.SubCapacityByStampTaskResult> SubCapacityByStampTaskFuture(
                Request.SubCapacityByStampTaskRequest request
        ) =>
            new SubCapacityByStampTaskTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.SubCapacityByStampTaskResult> SubCapacityByStampTaskAsync(
                Request.SubCapacityByStampTaskRequest request
        ) =>
            new SubCapacityByStampTaskTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.SubCapacityByStampTaskResult>();
    #else
		public SubCapacityByStampTaskTask SubCapacityByStampTaskAsync(
                Request.SubCapacityByStampTaskRequest request
        )
		{
			return new SubCapacityByStampTaskTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.SubCapacityByStampTaskResult> SubCapacityByStampTaskAsync(
                Request.SubCapacityByStampTaskRequest request
        ) =>
            new SubCapacityByStampTaskTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class SetCapacityByStampSheetTask : Gs2RestSessionTask<SetCapacityByStampSheetRequest, SetCapacityByStampSheetResult>
        {
            public SetCapacityByStampSheetTask(IGs2Session session, RestSessionRequestFactory factory, SetCapacityByStampSheetRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(SetCapacityByStampSheetRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "formation")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/stamp/mold/capacity/set";

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
		public IEnumerator SetCapacityByStampSheet(
                Request.SetCapacityByStampSheetRequest request,
                UnityAction<AsyncResult<Result.SetCapacityByStampSheetResult>> callback
        ) =>
            new SetCapacityByStampSheetTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.SetCapacityByStampSheetResult> SetCapacityByStampSheetFuture(
                Request.SetCapacityByStampSheetRequest request
        ) =>
            new SetCapacityByStampSheetTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.SetCapacityByStampSheetResult> SetCapacityByStampSheetAsync(
                Request.SetCapacityByStampSheetRequest request
        ) =>
            new SetCapacityByStampSheetTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.SetCapacityByStampSheetResult>();
    #else
		public SetCapacityByStampSheetTask SetCapacityByStampSheetAsync(
                Request.SetCapacityByStampSheetRequest request
        )
		{
			return new SetCapacityByStampSheetTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.SetCapacityByStampSheetResult> SetCapacityByStampSheetAsync(
                Request.SetCapacityByStampSheetRequest request
        ) =>
            new SetCapacityByStampSheetTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class DescribeFormsTask : Gs2RestSessionTask<DescribeFormsRequest, DescribeFormsResult>
        {
            public DescribeFormsTask(IGs2Session session, RestSessionRequestFactory factory, DescribeFormsRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DescribeFormsRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "formation")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/me/mold/{moldModelName}/form";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{moldModelName}", !string.IsNullOrEmpty(request.MoldModelName) ? request.MoldModelName.ToString() : "null");

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
		public IEnumerator DescribeForms(
                Request.DescribeFormsRequest request,
                UnityAction<AsyncResult<Result.DescribeFormsResult>> callback
        ) =>
            new DescribeFormsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.DescribeFormsResult> DescribeFormsFuture(
                Request.DescribeFormsRequest request
        ) =>
            new DescribeFormsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.DescribeFormsResult> DescribeFormsAsync(
                Request.DescribeFormsRequest request
        ) =>
            new DescribeFormsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.DescribeFormsResult>();
    #else
		public DescribeFormsTask DescribeFormsAsync(
                Request.DescribeFormsRequest request
        )
		{
			return new DescribeFormsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.DescribeFormsResult> DescribeFormsAsync(
                Request.DescribeFormsRequest request
        ) =>
            new DescribeFormsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class DescribeFormsByUserIdTask : Gs2RestSessionTask<DescribeFormsByUserIdRequest, DescribeFormsByUserIdResult>
        {
            public DescribeFormsByUserIdTask(IGs2Session session, RestSessionRequestFactory factory, DescribeFormsByUserIdRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DescribeFormsByUserIdRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "formation")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/{userId}/mold/{moldModelName}/form";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{moldModelName}", !string.IsNullOrEmpty(request.MoldModelName) ? request.MoldModelName.ToString() : "null");
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
		public IEnumerator DescribeFormsByUserId(
                Request.DescribeFormsByUserIdRequest request,
                UnityAction<AsyncResult<Result.DescribeFormsByUserIdResult>> callback
        ) =>
            new DescribeFormsByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.DescribeFormsByUserIdResult> DescribeFormsByUserIdFuture(
                Request.DescribeFormsByUserIdRequest request
        ) =>
            new DescribeFormsByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.DescribeFormsByUserIdResult> DescribeFormsByUserIdAsync(
                Request.DescribeFormsByUserIdRequest request
        ) =>
            new DescribeFormsByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.DescribeFormsByUserIdResult>();
    #else
		public DescribeFormsByUserIdTask DescribeFormsByUserIdAsync(
                Request.DescribeFormsByUserIdRequest request
        )
		{
			return new DescribeFormsByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.DescribeFormsByUserIdResult> DescribeFormsByUserIdAsync(
                Request.DescribeFormsByUserIdRequest request
        ) =>
            new DescribeFormsByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class GetFormTask : Gs2RestSessionTask<GetFormRequest, GetFormResult>
        {
            public GetFormTask(IGs2Session session, RestSessionRequestFactory factory, GetFormRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(GetFormRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "formation")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/me/mold/{moldModelName}/form/{index}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{moldModelName}", !string.IsNullOrEmpty(request.MoldModelName) ? request.MoldModelName.ToString() : "null");
                url = url.Replace("{index}",request.Index != null ? request.Index.ToString() : "null");

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
		public IEnumerator GetForm(
                Request.GetFormRequest request,
                UnityAction<AsyncResult<Result.GetFormResult>> callback
        ) =>
            new GetFormTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.GetFormResult> GetFormFuture(
                Request.GetFormRequest request
        ) =>
            new GetFormTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.GetFormResult> GetFormAsync(
                Request.GetFormRequest request
        ) =>
            new GetFormTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.GetFormResult>();
    #else
		public GetFormTask GetFormAsync(
                Request.GetFormRequest request
        )
		{
			return new GetFormTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.GetFormResult> GetFormAsync(
                Request.GetFormRequest request
        ) =>
            new GetFormTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class GetFormByUserIdTask : Gs2RestSessionTask<GetFormByUserIdRequest, GetFormByUserIdResult>
        {
            public GetFormByUserIdTask(IGs2Session session, RestSessionRequestFactory factory, GetFormByUserIdRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(GetFormByUserIdRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "formation")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/{userId}/mold/{moldModelName}/form/{index}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{userId}", !string.IsNullOrEmpty(request.UserId) ? request.UserId.ToString() : "null");
                url = url.Replace("{moldModelName}", !string.IsNullOrEmpty(request.MoldModelName) ? request.MoldModelName.ToString() : "null");
                url = url.Replace("{index}",request.Index != null ? request.Index.ToString() : "null");

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
		public IEnumerator GetFormByUserId(
                Request.GetFormByUserIdRequest request,
                UnityAction<AsyncResult<Result.GetFormByUserIdResult>> callback
        ) =>
            new GetFormByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.GetFormByUserIdResult> GetFormByUserIdFuture(
                Request.GetFormByUserIdRequest request
        ) =>
            new GetFormByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.GetFormByUserIdResult> GetFormByUserIdAsync(
                Request.GetFormByUserIdRequest request
        ) =>
            new GetFormByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.GetFormByUserIdResult>();
    #else
		public GetFormByUserIdTask GetFormByUserIdAsync(
                Request.GetFormByUserIdRequest request
        )
		{
			return new GetFormByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.GetFormByUserIdResult> GetFormByUserIdAsync(
                Request.GetFormByUserIdRequest request
        ) =>
            new GetFormByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class GetFormWithSignatureTask : Gs2RestSessionTask<GetFormWithSignatureRequest, GetFormWithSignatureResult>
        {
            public GetFormWithSignatureTask(IGs2Session session, RestSessionRequestFactory factory, GetFormWithSignatureRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(GetFormWithSignatureRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "formation")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/me/mold/{moldModelName}/form/{index}/signature";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{moldModelName}", !string.IsNullOrEmpty(request.MoldModelName) ? request.MoldModelName.ToString() : "null");
                url = url.Replace("{index}",request.Index != null ? request.Index.ToString() : "null");

                var sessionRequest = Factory.Get(url);
                if (request.ContextStack != null)
                {
                    sessionRequest.AddQueryString("contextStack", request.ContextStack);
                }
                if (request.KeyId != null) {
                    sessionRequest.AddQueryString("keyId", $"{request.KeyId}");
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
		public IEnumerator GetFormWithSignature(
                Request.GetFormWithSignatureRequest request,
                UnityAction<AsyncResult<Result.GetFormWithSignatureResult>> callback
        ) =>
            new GetFormWithSignatureTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.GetFormWithSignatureResult> GetFormWithSignatureFuture(
                Request.GetFormWithSignatureRequest request
        ) =>
            new GetFormWithSignatureTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.GetFormWithSignatureResult> GetFormWithSignatureAsync(
                Request.GetFormWithSignatureRequest request
        ) =>
            new GetFormWithSignatureTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.GetFormWithSignatureResult>();
    #else
		public GetFormWithSignatureTask GetFormWithSignatureAsync(
                Request.GetFormWithSignatureRequest request
        )
		{
			return new GetFormWithSignatureTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.GetFormWithSignatureResult> GetFormWithSignatureAsync(
                Request.GetFormWithSignatureRequest request
        ) =>
            new GetFormWithSignatureTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class GetFormWithSignatureByUserIdTask : Gs2RestSessionTask<GetFormWithSignatureByUserIdRequest, GetFormWithSignatureByUserIdResult>
        {
            public GetFormWithSignatureByUserIdTask(IGs2Session session, RestSessionRequestFactory factory, GetFormWithSignatureByUserIdRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(GetFormWithSignatureByUserIdRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "formation")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/{userId}/mold/{moldModelName}/form/{index}/signature";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{userId}", !string.IsNullOrEmpty(request.UserId) ? request.UserId.ToString() : "null");
                url = url.Replace("{moldModelName}", !string.IsNullOrEmpty(request.MoldModelName) ? request.MoldModelName.ToString() : "null");
                url = url.Replace("{index}",request.Index != null ? request.Index.ToString() : "null");

                var sessionRequest = Factory.Get(url);
                if (request.ContextStack != null)
                {
                    sessionRequest.AddQueryString("contextStack", request.ContextStack);
                }
                if (request.KeyId != null) {
                    sessionRequest.AddQueryString("keyId", $"{request.KeyId}");
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
		public IEnumerator GetFormWithSignatureByUserId(
                Request.GetFormWithSignatureByUserIdRequest request,
                UnityAction<AsyncResult<Result.GetFormWithSignatureByUserIdResult>> callback
        ) =>
            new GetFormWithSignatureByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.GetFormWithSignatureByUserIdResult> GetFormWithSignatureByUserIdFuture(
                Request.GetFormWithSignatureByUserIdRequest request
        ) =>
            new GetFormWithSignatureByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.GetFormWithSignatureByUserIdResult> GetFormWithSignatureByUserIdAsync(
                Request.GetFormWithSignatureByUserIdRequest request
        ) =>
            new GetFormWithSignatureByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.GetFormWithSignatureByUserIdResult>();
    #else
		public GetFormWithSignatureByUserIdTask GetFormWithSignatureByUserIdAsync(
                Request.GetFormWithSignatureByUserIdRequest request
        )
		{
			return new GetFormWithSignatureByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.GetFormWithSignatureByUserIdResult> GetFormWithSignatureByUserIdAsync(
                Request.GetFormWithSignatureByUserIdRequest request
        ) =>
            new GetFormWithSignatureByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class SetFormTask : Gs2RestSessionTask<SetFormRequest, SetFormResult>
        {
            public SetFormTask(IGs2Session session, RestSessionRequestFactory factory, SetFormRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(SetFormRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "formation")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/me/mold/{moldModelName}/form/{index}/raw";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{moldModelName}", !string.IsNullOrEmpty(request.MoldModelName) ? request.MoldModelName.ToString() : "null");
                url = url.Replace("{index}",request.Index != null ? request.Index.ToString() : "null");

                var sessionRequest = Factory.Put(url);

                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);
                jsonWriter.WriteObjectStart();
                if (request.Slots != null)
                {
                    jsonWriter.WritePropertyName("slots");
                    jsonWriter.WriteArrayStart();
                    foreach(var item in request.Slots)
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
		public IEnumerator SetForm(
                Request.SetFormRequest request,
                UnityAction<AsyncResult<Result.SetFormResult>> callback
        ) =>
            new SetFormTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.SetFormResult> SetFormFuture(
                Request.SetFormRequest request
        ) =>
            new SetFormTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.SetFormResult> SetFormAsync(
                Request.SetFormRequest request
        ) =>
            new SetFormTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.SetFormResult>();
    #else
		public SetFormTask SetFormAsync(
                Request.SetFormRequest request
        )
		{
			return new SetFormTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.SetFormResult> SetFormAsync(
                Request.SetFormRequest request
        ) =>
            new SetFormTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class SetFormByUserIdTask : Gs2RestSessionTask<SetFormByUserIdRequest, SetFormByUserIdResult>
        {
            public SetFormByUserIdTask(IGs2Session session, RestSessionRequestFactory factory, SetFormByUserIdRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(SetFormByUserIdRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "formation")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/{userId}/mold/{moldModelName}/form/{index}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{userId}", !string.IsNullOrEmpty(request.UserId) ? request.UserId.ToString() : "null");
                url = url.Replace("{moldModelName}", !string.IsNullOrEmpty(request.MoldModelName) ? request.MoldModelName.ToString() : "null");
                url = url.Replace("{index}",request.Index != null ? request.Index.ToString() : "null");

                var sessionRequest = Factory.Put(url);

                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);
                jsonWriter.WriteObjectStart();
                if (request.Slots != null)
                {
                    jsonWriter.WritePropertyName("slots");
                    jsonWriter.WriteArrayStart();
                    foreach(var item in request.Slots)
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
		public IEnumerator SetFormByUserId(
                Request.SetFormByUserIdRequest request,
                UnityAction<AsyncResult<Result.SetFormByUserIdResult>> callback
        ) =>
            new SetFormByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.SetFormByUserIdResult> SetFormByUserIdFuture(
                Request.SetFormByUserIdRequest request
        ) =>
            new SetFormByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.SetFormByUserIdResult> SetFormByUserIdAsync(
                Request.SetFormByUserIdRequest request
        ) =>
            new SetFormByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.SetFormByUserIdResult>();
    #else
		public SetFormByUserIdTask SetFormByUserIdAsync(
                Request.SetFormByUserIdRequest request
        )
		{
			return new SetFormByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.SetFormByUserIdResult> SetFormByUserIdAsync(
                Request.SetFormByUserIdRequest request
        ) =>
            new SetFormByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class SetFormWithSignatureTask : Gs2RestSessionTask<SetFormWithSignatureRequest, SetFormWithSignatureResult>
        {
            public SetFormWithSignatureTask(IGs2Session session, RestSessionRequestFactory factory, SetFormWithSignatureRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(SetFormWithSignatureRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "formation")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/me/mold/{moldModelName}/form/{index}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{moldModelName}", !string.IsNullOrEmpty(request.MoldModelName) ? request.MoldModelName.ToString() : "null");
                url = url.Replace("{index}",request.Index != null ? request.Index.ToString() : "null");

                var sessionRequest = Factory.Put(url);

                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);
                jsonWriter.WriteObjectStart();
                if (request.Slots != null)
                {
                    jsonWriter.WritePropertyName("slots");
                    jsonWriter.WriteArrayStart();
                    foreach(var item in request.Slots)
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
		public IEnumerator SetFormWithSignature(
                Request.SetFormWithSignatureRequest request,
                UnityAction<AsyncResult<Result.SetFormWithSignatureResult>> callback
        ) =>
            new SetFormWithSignatureTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.SetFormWithSignatureResult> SetFormWithSignatureFuture(
                Request.SetFormWithSignatureRequest request
        ) =>
            new SetFormWithSignatureTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.SetFormWithSignatureResult> SetFormWithSignatureAsync(
                Request.SetFormWithSignatureRequest request
        ) =>
            new SetFormWithSignatureTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.SetFormWithSignatureResult>();
    #else
		public SetFormWithSignatureTask SetFormWithSignatureAsync(
                Request.SetFormWithSignatureRequest request
        )
		{
			return new SetFormWithSignatureTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.SetFormWithSignatureResult> SetFormWithSignatureAsync(
                Request.SetFormWithSignatureRequest request
        ) =>
            new SetFormWithSignatureTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class AcquireActionsToFormPropertiesTask : Gs2RestSessionTask<AcquireActionsToFormPropertiesRequest, AcquireActionsToFormPropertiesResult>
        {
            public AcquireActionsToFormPropertiesTask(IGs2Session session, RestSessionRequestFactory factory, AcquireActionsToFormPropertiesRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(AcquireActionsToFormPropertiesRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "formation")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/{userId}/mold/{moldModelName}/form/{index}/stamp/delegate";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{userId}", !string.IsNullOrEmpty(request.UserId) ? request.UserId.ToString() : "null");
                url = url.Replace("{moldModelName}", !string.IsNullOrEmpty(request.MoldModelName) ? request.MoldModelName.ToString() : "null");
                url = url.Replace("{index}",request.Index != null ? request.Index.ToString() : "null");

                var sessionRequest = Factory.Post(url);

                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);
                jsonWriter.WriteObjectStart();
                if (request.AcquireAction != null)
                {
                    jsonWriter.WritePropertyName("acquireAction");
                    request.AcquireAction.WriteJson(jsonWriter);
                }
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
		public IEnumerator AcquireActionsToFormProperties(
                Request.AcquireActionsToFormPropertiesRequest request,
                UnityAction<AsyncResult<Result.AcquireActionsToFormPropertiesResult>> callback
        ) =>
            new AcquireActionsToFormPropertiesTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.AcquireActionsToFormPropertiesResult> AcquireActionsToFormPropertiesFuture(
                Request.AcquireActionsToFormPropertiesRequest request
        ) =>
            new AcquireActionsToFormPropertiesTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.AcquireActionsToFormPropertiesResult> AcquireActionsToFormPropertiesAsync(
                Request.AcquireActionsToFormPropertiesRequest request
        ) =>
            new AcquireActionsToFormPropertiesTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.AcquireActionsToFormPropertiesResult>();
    #else
		public AcquireActionsToFormPropertiesTask AcquireActionsToFormPropertiesAsync(
                Request.AcquireActionsToFormPropertiesRequest request
        )
		{
			return new AcquireActionsToFormPropertiesTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.AcquireActionsToFormPropertiesResult> AcquireActionsToFormPropertiesAsync(
                Request.AcquireActionsToFormPropertiesRequest request
        ) =>
            new AcquireActionsToFormPropertiesTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class DeleteFormTask : Gs2RestSessionTask<DeleteFormRequest, DeleteFormResult>
        {
            public DeleteFormTask(IGs2Session session, RestSessionRequestFactory factory, DeleteFormRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DeleteFormRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "formation")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/me/mold/{moldModelName}/form/{index}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{moldModelName}", !string.IsNullOrEmpty(request.MoldModelName) ? request.MoldModelName.ToString() : "null");
                url = url.Replace("{index}",request.Index != null ? request.Index.ToString() : "null");

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
		public IEnumerator DeleteForm(
                Request.DeleteFormRequest request,
                UnityAction<AsyncResult<Result.DeleteFormResult>> callback
        ) =>
            new DeleteFormTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.DeleteFormResult> DeleteFormFuture(
                Request.DeleteFormRequest request
        ) =>
            new DeleteFormTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.DeleteFormResult> DeleteFormAsync(
                Request.DeleteFormRequest request
        ) =>
            new DeleteFormTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.DeleteFormResult>();
    #else
		public DeleteFormTask DeleteFormAsync(
                Request.DeleteFormRequest request
        )
		{
			return new DeleteFormTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.DeleteFormResult> DeleteFormAsync(
                Request.DeleteFormRequest request
        ) =>
            new DeleteFormTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class DeleteFormByUserIdTask : Gs2RestSessionTask<DeleteFormByUserIdRequest, DeleteFormByUserIdResult>
        {
            public DeleteFormByUserIdTask(IGs2Session session, RestSessionRequestFactory factory, DeleteFormByUserIdRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DeleteFormByUserIdRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "formation")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/{userId}/mold/{moldModelName}/form/{index}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{userId}", !string.IsNullOrEmpty(request.UserId) ? request.UserId.ToString() : "null");
                url = url.Replace("{moldModelName}", !string.IsNullOrEmpty(request.MoldModelName) ? request.MoldModelName.ToString() : "null");
                url = url.Replace("{index}",request.Index != null ? request.Index.ToString() : "null");

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
		public IEnumerator DeleteFormByUserId(
                Request.DeleteFormByUserIdRequest request,
                UnityAction<AsyncResult<Result.DeleteFormByUserIdResult>> callback
        ) =>
            new DeleteFormByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.DeleteFormByUserIdResult> DeleteFormByUserIdFuture(
                Request.DeleteFormByUserIdRequest request
        ) =>
            new DeleteFormByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.DeleteFormByUserIdResult> DeleteFormByUserIdAsync(
                Request.DeleteFormByUserIdRequest request
        ) =>
            new DeleteFormByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.DeleteFormByUserIdResult>();
    #else
		public DeleteFormByUserIdTask DeleteFormByUserIdAsync(
                Request.DeleteFormByUserIdRequest request
        )
		{
			return new DeleteFormByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.DeleteFormByUserIdResult> DeleteFormByUserIdAsync(
                Request.DeleteFormByUserIdRequest request
        ) =>
            new DeleteFormByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class AcquireActionToFormPropertiesByStampSheetTask : Gs2RestSessionTask<AcquireActionToFormPropertiesByStampSheetRequest, AcquireActionToFormPropertiesByStampSheetResult>
        {
            public AcquireActionToFormPropertiesByStampSheetTask(IGs2Session session, RestSessionRequestFactory factory, AcquireActionToFormPropertiesByStampSheetRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(AcquireActionToFormPropertiesByStampSheetRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "formation")
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
		public IEnumerator AcquireActionToFormPropertiesByStampSheet(
                Request.AcquireActionToFormPropertiesByStampSheetRequest request,
                UnityAction<AsyncResult<Result.AcquireActionToFormPropertiesByStampSheetResult>> callback
        ) =>
            new AcquireActionToFormPropertiesByStampSheetTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.AcquireActionToFormPropertiesByStampSheetResult> AcquireActionToFormPropertiesByStampSheetFuture(
                Request.AcquireActionToFormPropertiesByStampSheetRequest request
        ) =>
            new AcquireActionToFormPropertiesByStampSheetTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.AcquireActionToFormPropertiesByStampSheetResult> AcquireActionToFormPropertiesByStampSheetAsync(
                Request.AcquireActionToFormPropertiesByStampSheetRequest request
        ) =>
            new AcquireActionToFormPropertiesByStampSheetTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.AcquireActionToFormPropertiesByStampSheetResult>();
    #else
		public AcquireActionToFormPropertiesByStampSheetTask AcquireActionToFormPropertiesByStampSheetAsync(
                Request.AcquireActionToFormPropertiesByStampSheetRequest request
        )
		{
			return new AcquireActionToFormPropertiesByStampSheetTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.AcquireActionToFormPropertiesByStampSheetResult> AcquireActionToFormPropertiesByStampSheetAsync(
                Request.AcquireActionToFormPropertiesByStampSheetRequest request
        ) =>
            new AcquireActionToFormPropertiesByStampSheetTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class SetFormByStampSheetTask : Gs2RestSessionTask<SetFormByStampSheetRequest, SetFormByStampSheetResult>
        {
            public SetFormByStampSheetTask(IGs2Session session, RestSessionRequestFactory factory, SetFormByStampSheetRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(SetFormByStampSheetRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "formation")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/stamp/form/set";

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
		public IEnumerator SetFormByStampSheet(
                Request.SetFormByStampSheetRequest request,
                UnityAction<AsyncResult<Result.SetFormByStampSheetResult>> callback
        ) =>
            new SetFormByStampSheetTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.SetFormByStampSheetResult> SetFormByStampSheetFuture(
                Request.SetFormByStampSheetRequest request
        ) =>
            new SetFormByStampSheetTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.SetFormByStampSheetResult> SetFormByStampSheetAsync(
                Request.SetFormByStampSheetRequest request
        ) =>
            new SetFormByStampSheetTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.SetFormByStampSheetResult>();
    #else
		public SetFormByStampSheetTask SetFormByStampSheetAsync(
                Request.SetFormByStampSheetRequest request
        )
		{
			return new SetFormByStampSheetTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.SetFormByStampSheetResult> SetFormByStampSheetAsync(
                Request.SetFormByStampSheetRequest request
        ) =>
            new SetFormByStampSheetTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class DescribePropertyFormsTask : Gs2RestSessionTask<DescribePropertyFormsRequest, DescribePropertyFormsResult>
        {
            public DescribePropertyFormsTask(IGs2Session session, RestSessionRequestFactory factory, DescribePropertyFormsRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DescribePropertyFormsRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "formation")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/me/property/{propertyFormModelName}/form";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{propertyFormModelName}", !string.IsNullOrEmpty(request.PropertyFormModelName) ? request.PropertyFormModelName.ToString() : "null");

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
		public IEnumerator DescribePropertyForms(
                Request.DescribePropertyFormsRequest request,
                UnityAction<AsyncResult<Result.DescribePropertyFormsResult>> callback
        ) =>
            new DescribePropertyFormsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.DescribePropertyFormsResult> DescribePropertyFormsFuture(
                Request.DescribePropertyFormsRequest request
        ) =>
            new DescribePropertyFormsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.DescribePropertyFormsResult> DescribePropertyFormsAsync(
                Request.DescribePropertyFormsRequest request
        ) =>
            new DescribePropertyFormsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.DescribePropertyFormsResult>();
    #else
		public DescribePropertyFormsTask DescribePropertyFormsAsync(
                Request.DescribePropertyFormsRequest request
        )
		{
			return new DescribePropertyFormsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.DescribePropertyFormsResult> DescribePropertyFormsAsync(
                Request.DescribePropertyFormsRequest request
        ) =>
            new DescribePropertyFormsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class DescribePropertyFormsByUserIdTask : Gs2RestSessionTask<DescribePropertyFormsByUserIdRequest, DescribePropertyFormsByUserIdResult>
        {
            public DescribePropertyFormsByUserIdTask(IGs2Session session, RestSessionRequestFactory factory, DescribePropertyFormsByUserIdRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DescribePropertyFormsByUserIdRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "formation")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/{userId}/property/{propertyFormModelName}/form";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{userId}", !string.IsNullOrEmpty(request.UserId) ? request.UserId.ToString() : "null");
                url = url.Replace("{propertyFormModelName}", !string.IsNullOrEmpty(request.PropertyFormModelName) ? request.PropertyFormModelName.ToString() : "null");

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
		public IEnumerator DescribePropertyFormsByUserId(
                Request.DescribePropertyFormsByUserIdRequest request,
                UnityAction<AsyncResult<Result.DescribePropertyFormsByUserIdResult>> callback
        ) =>
            new DescribePropertyFormsByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.DescribePropertyFormsByUserIdResult> DescribePropertyFormsByUserIdFuture(
                Request.DescribePropertyFormsByUserIdRequest request
        ) =>
            new DescribePropertyFormsByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.DescribePropertyFormsByUserIdResult> DescribePropertyFormsByUserIdAsync(
                Request.DescribePropertyFormsByUserIdRequest request
        ) =>
            new DescribePropertyFormsByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.DescribePropertyFormsByUserIdResult>();
    #else
		public DescribePropertyFormsByUserIdTask DescribePropertyFormsByUserIdAsync(
                Request.DescribePropertyFormsByUserIdRequest request
        )
		{
			return new DescribePropertyFormsByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.DescribePropertyFormsByUserIdResult> DescribePropertyFormsByUserIdAsync(
                Request.DescribePropertyFormsByUserIdRequest request
        ) =>
            new DescribePropertyFormsByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class GetPropertyFormTask : Gs2RestSessionTask<GetPropertyFormRequest, GetPropertyFormResult>
        {
            public GetPropertyFormTask(IGs2Session session, RestSessionRequestFactory factory, GetPropertyFormRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(GetPropertyFormRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "formation")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/me/property/{propertyFormModelName}/form/{propertyId}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{propertyFormModelName}", !string.IsNullOrEmpty(request.PropertyFormModelName) ? request.PropertyFormModelName.ToString() : "null");
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
		public IEnumerator GetPropertyForm(
                Request.GetPropertyFormRequest request,
                UnityAction<AsyncResult<Result.GetPropertyFormResult>> callback
        ) =>
            new GetPropertyFormTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.GetPropertyFormResult> GetPropertyFormFuture(
                Request.GetPropertyFormRequest request
        ) =>
            new GetPropertyFormTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.GetPropertyFormResult> GetPropertyFormAsync(
                Request.GetPropertyFormRequest request
        ) =>
            new GetPropertyFormTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.GetPropertyFormResult>();
    #else
		public GetPropertyFormTask GetPropertyFormAsync(
                Request.GetPropertyFormRequest request
        )
		{
			return new GetPropertyFormTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.GetPropertyFormResult> GetPropertyFormAsync(
                Request.GetPropertyFormRequest request
        ) =>
            new GetPropertyFormTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class GetPropertyFormByUserIdTask : Gs2RestSessionTask<GetPropertyFormByUserIdRequest, GetPropertyFormByUserIdResult>
        {
            public GetPropertyFormByUserIdTask(IGs2Session session, RestSessionRequestFactory factory, GetPropertyFormByUserIdRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(GetPropertyFormByUserIdRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "formation")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/{userId}/property/{propertyFormModelName}/form/{propertyId}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{userId}", !string.IsNullOrEmpty(request.UserId) ? request.UserId.ToString() : "null");
                url = url.Replace("{propertyFormModelName}", !string.IsNullOrEmpty(request.PropertyFormModelName) ? request.PropertyFormModelName.ToString() : "null");
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
		public IEnumerator GetPropertyFormByUserId(
                Request.GetPropertyFormByUserIdRequest request,
                UnityAction<AsyncResult<Result.GetPropertyFormByUserIdResult>> callback
        ) =>
            new GetPropertyFormByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.GetPropertyFormByUserIdResult> GetPropertyFormByUserIdFuture(
                Request.GetPropertyFormByUserIdRequest request
        ) =>
            new GetPropertyFormByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.GetPropertyFormByUserIdResult> GetPropertyFormByUserIdAsync(
                Request.GetPropertyFormByUserIdRequest request
        ) =>
            new GetPropertyFormByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.GetPropertyFormByUserIdResult>();
    #else
		public GetPropertyFormByUserIdTask GetPropertyFormByUserIdAsync(
                Request.GetPropertyFormByUserIdRequest request
        )
		{
			return new GetPropertyFormByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.GetPropertyFormByUserIdResult> GetPropertyFormByUserIdAsync(
                Request.GetPropertyFormByUserIdRequest request
        ) =>
            new GetPropertyFormByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class GetPropertyFormWithSignatureTask : Gs2RestSessionTask<GetPropertyFormWithSignatureRequest, GetPropertyFormWithSignatureResult>
        {
            public GetPropertyFormWithSignatureTask(IGs2Session session, RestSessionRequestFactory factory, GetPropertyFormWithSignatureRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(GetPropertyFormWithSignatureRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "formation")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/me/property/{propertyFormModelName}/form/{propertyId}/signature";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{propertyFormModelName}", !string.IsNullOrEmpty(request.PropertyFormModelName) ? request.PropertyFormModelName.ToString() : "null");
                url = url.Replace("{propertyId}", !string.IsNullOrEmpty(request.PropertyId) ? request.PropertyId.ToString() : "null");

                var sessionRequest = Factory.Get(url);
                if (request.ContextStack != null)
                {
                    sessionRequest.AddQueryString("contextStack", request.ContextStack);
                }
                if (request.KeyId != null) {
                    sessionRequest.AddQueryString("keyId", $"{request.KeyId}");
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
		public IEnumerator GetPropertyFormWithSignature(
                Request.GetPropertyFormWithSignatureRequest request,
                UnityAction<AsyncResult<Result.GetPropertyFormWithSignatureResult>> callback
        ) =>
            new GetPropertyFormWithSignatureTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.GetPropertyFormWithSignatureResult> GetPropertyFormWithSignatureFuture(
                Request.GetPropertyFormWithSignatureRequest request
        ) =>
            new GetPropertyFormWithSignatureTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.GetPropertyFormWithSignatureResult> GetPropertyFormWithSignatureAsync(
                Request.GetPropertyFormWithSignatureRequest request
        ) =>
            new GetPropertyFormWithSignatureTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.GetPropertyFormWithSignatureResult>();
    #else
		public GetPropertyFormWithSignatureTask GetPropertyFormWithSignatureAsync(
                Request.GetPropertyFormWithSignatureRequest request
        )
		{
			return new GetPropertyFormWithSignatureTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.GetPropertyFormWithSignatureResult> GetPropertyFormWithSignatureAsync(
                Request.GetPropertyFormWithSignatureRequest request
        ) =>
            new GetPropertyFormWithSignatureTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class GetPropertyFormWithSignatureByUserIdTask : Gs2RestSessionTask<GetPropertyFormWithSignatureByUserIdRequest, GetPropertyFormWithSignatureByUserIdResult>
        {
            public GetPropertyFormWithSignatureByUserIdTask(IGs2Session session, RestSessionRequestFactory factory, GetPropertyFormWithSignatureByUserIdRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(GetPropertyFormWithSignatureByUserIdRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "formation")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/{userId}/property/{propertyFormModelName}/form/{propertyId}/signature";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{userId}", !string.IsNullOrEmpty(request.UserId) ? request.UserId.ToString() : "null");
                url = url.Replace("{propertyFormModelName}", !string.IsNullOrEmpty(request.PropertyFormModelName) ? request.PropertyFormModelName.ToString() : "null");
                url = url.Replace("{propertyId}", !string.IsNullOrEmpty(request.PropertyId) ? request.PropertyId.ToString() : "null");

                var sessionRequest = Factory.Get(url);
                if (request.ContextStack != null)
                {
                    sessionRequest.AddQueryString("contextStack", request.ContextStack);
                }
                if (request.KeyId != null) {
                    sessionRequest.AddQueryString("keyId", $"{request.KeyId}");
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
		public IEnumerator GetPropertyFormWithSignatureByUserId(
                Request.GetPropertyFormWithSignatureByUserIdRequest request,
                UnityAction<AsyncResult<Result.GetPropertyFormWithSignatureByUserIdResult>> callback
        ) =>
            new GetPropertyFormWithSignatureByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.GetPropertyFormWithSignatureByUserIdResult> GetPropertyFormWithSignatureByUserIdFuture(
                Request.GetPropertyFormWithSignatureByUserIdRequest request
        ) =>
            new GetPropertyFormWithSignatureByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.GetPropertyFormWithSignatureByUserIdResult> GetPropertyFormWithSignatureByUserIdAsync(
                Request.GetPropertyFormWithSignatureByUserIdRequest request
        ) =>
            new GetPropertyFormWithSignatureByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.GetPropertyFormWithSignatureByUserIdResult>();
    #else
		public GetPropertyFormWithSignatureByUserIdTask GetPropertyFormWithSignatureByUserIdAsync(
                Request.GetPropertyFormWithSignatureByUserIdRequest request
        )
		{
			return new GetPropertyFormWithSignatureByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.GetPropertyFormWithSignatureByUserIdResult> GetPropertyFormWithSignatureByUserIdAsync(
                Request.GetPropertyFormWithSignatureByUserIdRequest request
        ) =>
            new GetPropertyFormWithSignatureByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class SetPropertyFormTask : Gs2RestSessionTask<SetPropertyFormRequest, SetPropertyFormResult>
        {
            public SetPropertyFormTask(IGs2Session session, RestSessionRequestFactory factory, SetPropertyFormRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(SetPropertyFormRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "formation")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/me/property/{propertyFormModelName}/form/{propertyId}/raw";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{propertyFormModelName}", !string.IsNullOrEmpty(request.PropertyFormModelName) ? request.PropertyFormModelName.ToString() : "null");
                url = url.Replace("{propertyId}", !string.IsNullOrEmpty(request.PropertyId) ? request.PropertyId.ToString() : "null");

                var sessionRequest = Factory.Put(url);

                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);
                jsonWriter.WriteObjectStart();
                if (request.Slots != null)
                {
                    jsonWriter.WritePropertyName("slots");
                    jsonWriter.WriteArrayStart();
                    foreach(var item in request.Slots)
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
		public IEnumerator SetPropertyForm(
                Request.SetPropertyFormRequest request,
                UnityAction<AsyncResult<Result.SetPropertyFormResult>> callback
        ) =>
            new SetPropertyFormTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.SetPropertyFormResult> SetPropertyFormFuture(
                Request.SetPropertyFormRequest request
        ) =>
            new SetPropertyFormTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.SetPropertyFormResult> SetPropertyFormAsync(
                Request.SetPropertyFormRequest request
        ) =>
            new SetPropertyFormTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.SetPropertyFormResult>();
    #else
		public SetPropertyFormTask SetPropertyFormAsync(
                Request.SetPropertyFormRequest request
        )
		{
			return new SetPropertyFormTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.SetPropertyFormResult> SetPropertyFormAsync(
                Request.SetPropertyFormRequest request
        ) =>
            new SetPropertyFormTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class SetPropertyFormByUserIdTask : Gs2RestSessionTask<SetPropertyFormByUserIdRequest, SetPropertyFormByUserIdResult>
        {
            public SetPropertyFormByUserIdTask(IGs2Session session, RestSessionRequestFactory factory, SetPropertyFormByUserIdRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(SetPropertyFormByUserIdRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "formation")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/{userId}/property/{propertyFormModelName}/form/{propertyId}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{userId}", !string.IsNullOrEmpty(request.UserId) ? request.UserId.ToString() : "null");
                url = url.Replace("{propertyFormModelName}", !string.IsNullOrEmpty(request.PropertyFormModelName) ? request.PropertyFormModelName.ToString() : "null");
                url = url.Replace("{propertyId}", !string.IsNullOrEmpty(request.PropertyId) ? request.PropertyId.ToString() : "null");

                var sessionRequest = Factory.Put(url);

                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);
                jsonWriter.WriteObjectStart();
                if (request.Slots != null)
                {
                    jsonWriter.WritePropertyName("slots");
                    jsonWriter.WriteArrayStart();
                    foreach(var item in request.Slots)
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
		public IEnumerator SetPropertyFormByUserId(
                Request.SetPropertyFormByUserIdRequest request,
                UnityAction<AsyncResult<Result.SetPropertyFormByUserIdResult>> callback
        ) =>
            new SetPropertyFormByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.SetPropertyFormByUserIdResult> SetPropertyFormByUserIdFuture(
                Request.SetPropertyFormByUserIdRequest request
        ) =>
            new SetPropertyFormByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.SetPropertyFormByUserIdResult> SetPropertyFormByUserIdAsync(
                Request.SetPropertyFormByUserIdRequest request
        ) =>
            new SetPropertyFormByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.SetPropertyFormByUserIdResult>();
    #else
		public SetPropertyFormByUserIdTask SetPropertyFormByUserIdAsync(
                Request.SetPropertyFormByUserIdRequest request
        )
		{
			return new SetPropertyFormByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.SetPropertyFormByUserIdResult> SetPropertyFormByUserIdAsync(
                Request.SetPropertyFormByUserIdRequest request
        ) =>
            new SetPropertyFormByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class SetPropertyFormWithSignatureTask : Gs2RestSessionTask<SetPropertyFormWithSignatureRequest, SetPropertyFormWithSignatureResult>
        {
            public SetPropertyFormWithSignatureTask(IGs2Session session, RestSessionRequestFactory factory, SetPropertyFormWithSignatureRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(SetPropertyFormWithSignatureRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "formation")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/me/property/{propertyFormModelName}/form/{propertyId}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{propertyFormModelName}", !string.IsNullOrEmpty(request.PropertyFormModelName) ? request.PropertyFormModelName.ToString() : "null");
                url = url.Replace("{propertyId}", !string.IsNullOrEmpty(request.PropertyId) ? request.PropertyId.ToString() : "null");

                var sessionRequest = Factory.Put(url);

                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);
                jsonWriter.WriteObjectStart();
                if (request.Slots != null)
                {
                    jsonWriter.WritePropertyName("slots");
                    jsonWriter.WriteArrayStart();
                    foreach(var item in request.Slots)
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
		public IEnumerator SetPropertyFormWithSignature(
                Request.SetPropertyFormWithSignatureRequest request,
                UnityAction<AsyncResult<Result.SetPropertyFormWithSignatureResult>> callback
        ) =>
            new SetPropertyFormWithSignatureTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.SetPropertyFormWithSignatureResult> SetPropertyFormWithSignatureFuture(
                Request.SetPropertyFormWithSignatureRequest request
        ) =>
            new SetPropertyFormWithSignatureTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.SetPropertyFormWithSignatureResult> SetPropertyFormWithSignatureAsync(
                Request.SetPropertyFormWithSignatureRequest request
        ) =>
            new SetPropertyFormWithSignatureTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.SetPropertyFormWithSignatureResult>();
    #else
		public SetPropertyFormWithSignatureTask SetPropertyFormWithSignatureAsync(
                Request.SetPropertyFormWithSignatureRequest request
        )
		{
			return new SetPropertyFormWithSignatureTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.SetPropertyFormWithSignatureResult> SetPropertyFormWithSignatureAsync(
                Request.SetPropertyFormWithSignatureRequest request
        ) =>
            new SetPropertyFormWithSignatureTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class AcquireActionsToPropertyFormPropertiesTask : Gs2RestSessionTask<AcquireActionsToPropertyFormPropertiesRequest, AcquireActionsToPropertyFormPropertiesResult>
        {
            public AcquireActionsToPropertyFormPropertiesTask(IGs2Session session, RestSessionRequestFactory factory, AcquireActionsToPropertyFormPropertiesRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(AcquireActionsToPropertyFormPropertiesRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "formation")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/{userId}/property/{propertyFormModelName}/form/{propertyId}/stamp/delegate";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{userId}", !string.IsNullOrEmpty(request.UserId) ? request.UserId.ToString() : "null");
                url = url.Replace("{propertyFormModelName}", !string.IsNullOrEmpty(request.PropertyFormModelName) ? request.PropertyFormModelName.ToString() : "null");
                url = url.Replace("{propertyId}", !string.IsNullOrEmpty(request.PropertyId) ? request.PropertyId.ToString() : "null");

                var sessionRequest = Factory.Post(url);

                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);
                jsonWriter.WriteObjectStart();
                if (request.AcquireAction != null)
                {
                    jsonWriter.WritePropertyName("acquireAction");
                    request.AcquireAction.WriteJson(jsonWriter);
                }
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
		public IEnumerator AcquireActionsToPropertyFormProperties(
                Request.AcquireActionsToPropertyFormPropertiesRequest request,
                UnityAction<AsyncResult<Result.AcquireActionsToPropertyFormPropertiesResult>> callback
        ) =>
            new AcquireActionsToPropertyFormPropertiesTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.AcquireActionsToPropertyFormPropertiesResult> AcquireActionsToPropertyFormPropertiesFuture(
                Request.AcquireActionsToPropertyFormPropertiesRequest request
        ) =>
            new AcquireActionsToPropertyFormPropertiesTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.AcquireActionsToPropertyFormPropertiesResult> AcquireActionsToPropertyFormPropertiesAsync(
                Request.AcquireActionsToPropertyFormPropertiesRequest request
        ) =>
            new AcquireActionsToPropertyFormPropertiesTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.AcquireActionsToPropertyFormPropertiesResult>();
    #else
		public AcquireActionsToPropertyFormPropertiesTask AcquireActionsToPropertyFormPropertiesAsync(
                Request.AcquireActionsToPropertyFormPropertiesRequest request
        )
		{
			return new AcquireActionsToPropertyFormPropertiesTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.AcquireActionsToPropertyFormPropertiesResult> AcquireActionsToPropertyFormPropertiesAsync(
                Request.AcquireActionsToPropertyFormPropertiesRequest request
        ) =>
            new AcquireActionsToPropertyFormPropertiesTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class DeletePropertyFormTask : Gs2RestSessionTask<DeletePropertyFormRequest, DeletePropertyFormResult>
        {
            public DeletePropertyFormTask(IGs2Session session, RestSessionRequestFactory factory, DeletePropertyFormRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DeletePropertyFormRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "formation")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/me/property/{propertyFormModelName}/form/{propertyId}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{propertyFormModelName}", !string.IsNullOrEmpty(request.PropertyFormModelName) ? request.PropertyFormModelName.ToString() : "null");
                url = url.Replace("{propertyId}", !string.IsNullOrEmpty(request.PropertyId) ? request.PropertyId.ToString() : "null");

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
		public IEnumerator DeletePropertyForm(
                Request.DeletePropertyFormRequest request,
                UnityAction<AsyncResult<Result.DeletePropertyFormResult>> callback
        ) =>
            new DeletePropertyFormTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.DeletePropertyFormResult> DeletePropertyFormFuture(
                Request.DeletePropertyFormRequest request
        ) =>
            new DeletePropertyFormTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.DeletePropertyFormResult> DeletePropertyFormAsync(
                Request.DeletePropertyFormRequest request
        ) =>
            new DeletePropertyFormTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.DeletePropertyFormResult>();
    #else
		public DeletePropertyFormTask DeletePropertyFormAsync(
                Request.DeletePropertyFormRequest request
        )
		{
			return new DeletePropertyFormTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.DeletePropertyFormResult> DeletePropertyFormAsync(
                Request.DeletePropertyFormRequest request
        ) =>
            new DeletePropertyFormTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class DeletePropertyFormByUserIdTask : Gs2RestSessionTask<DeletePropertyFormByUserIdRequest, DeletePropertyFormByUserIdResult>
        {
            public DeletePropertyFormByUserIdTask(IGs2Session session, RestSessionRequestFactory factory, DeletePropertyFormByUserIdRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DeletePropertyFormByUserIdRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "formation")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/{userId}/property/{propertyFormModelName}/form/{propertyId}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{userId}", !string.IsNullOrEmpty(request.UserId) ? request.UserId.ToString() : "null");
                url = url.Replace("{propertyFormModelName}", !string.IsNullOrEmpty(request.PropertyFormModelName) ? request.PropertyFormModelName.ToString() : "null");
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
		public IEnumerator DeletePropertyFormByUserId(
                Request.DeletePropertyFormByUserIdRequest request,
                UnityAction<AsyncResult<Result.DeletePropertyFormByUserIdResult>> callback
        ) =>
            new DeletePropertyFormByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.DeletePropertyFormByUserIdResult> DeletePropertyFormByUserIdFuture(
                Request.DeletePropertyFormByUserIdRequest request
        ) =>
            new DeletePropertyFormByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.DeletePropertyFormByUserIdResult> DeletePropertyFormByUserIdAsync(
                Request.DeletePropertyFormByUserIdRequest request
        ) =>
            new DeletePropertyFormByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.DeletePropertyFormByUserIdResult>();
    #else
		public DeletePropertyFormByUserIdTask DeletePropertyFormByUserIdAsync(
                Request.DeletePropertyFormByUserIdRequest request
        )
		{
			return new DeletePropertyFormByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.DeletePropertyFormByUserIdResult> DeletePropertyFormByUserIdAsync(
                Request.DeletePropertyFormByUserIdRequest request
        ) =>
            new DeletePropertyFormByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class AcquireActionToPropertyFormPropertiesByStampSheetTask : Gs2RestSessionTask<AcquireActionToPropertyFormPropertiesByStampSheetRequest, AcquireActionToPropertyFormPropertiesByStampSheetResult>
        {
            public AcquireActionToPropertyFormPropertiesByStampSheetTask(IGs2Session session, RestSessionRequestFactory factory, AcquireActionToPropertyFormPropertiesByStampSheetRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(AcquireActionToPropertyFormPropertiesByStampSheetRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "formation")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/stamp/property/form/acquire";

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
		public IEnumerator AcquireActionToPropertyFormPropertiesByStampSheet(
                Request.AcquireActionToPropertyFormPropertiesByStampSheetRequest request,
                UnityAction<AsyncResult<Result.AcquireActionToPropertyFormPropertiesByStampSheetResult>> callback
        ) =>
            new AcquireActionToPropertyFormPropertiesByStampSheetTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.AcquireActionToPropertyFormPropertiesByStampSheetResult> AcquireActionToPropertyFormPropertiesByStampSheetFuture(
                Request.AcquireActionToPropertyFormPropertiesByStampSheetRequest request
        ) =>
            new AcquireActionToPropertyFormPropertiesByStampSheetTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.AcquireActionToPropertyFormPropertiesByStampSheetResult> AcquireActionToPropertyFormPropertiesByStampSheetAsync(
                Request.AcquireActionToPropertyFormPropertiesByStampSheetRequest request
        ) =>
            new AcquireActionToPropertyFormPropertiesByStampSheetTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.AcquireActionToPropertyFormPropertiesByStampSheetResult>();
    #else
		public AcquireActionToPropertyFormPropertiesByStampSheetTask AcquireActionToPropertyFormPropertiesByStampSheetAsync(
                Request.AcquireActionToPropertyFormPropertiesByStampSheetRequest request
        )
		{
			return new AcquireActionToPropertyFormPropertiesByStampSheetTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.AcquireActionToPropertyFormPropertiesByStampSheetResult> AcquireActionToPropertyFormPropertiesByStampSheetAsync(
                Request.AcquireActionToPropertyFormPropertiesByStampSheetRequest request
        ) =>
            new AcquireActionToPropertyFormPropertiesByStampSheetTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif
	}
}