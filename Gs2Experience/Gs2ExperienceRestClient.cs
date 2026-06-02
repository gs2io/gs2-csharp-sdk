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
using Gs2.Gs2Experience.Request;
using Gs2.Gs2Experience.Result;
using Gs2.Util.LitJson;

namespace Gs2.Gs2Experience
{
	public class Gs2ExperienceRestClient : AbstractGs2Client
	{
#if UNITY_2017_1_OR_NEWER
		private readonly CertificateHandler _certificateHandler;
#endif

		public static string Endpoint = "experience";

        protected Gs2RestSession Gs2RestSession => (Gs2RestSession) Gs2Session;

		public Gs2ExperienceRestClient(Gs2RestSession Gs2RestSession) : base(Gs2RestSession)
		{

		}

#if UNITY_2017_1_OR_NEWER
		public Gs2ExperienceRestClient(Gs2RestSession gs2RestSession, CertificateHandler certificateHandler) : base(gs2RestSession)
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
                    .Replace("{service}", "experience")
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
                    .Replace("{service}", "experience")
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
                if (request.RankCapScriptId != null)
                {
                    jsonWriter.WritePropertyName("rankCapScriptId");
                    jsonWriter.Write(request.RankCapScriptId);
                }
                if (request.ChangeExperienceScript != null)
                {
                    jsonWriter.WritePropertyName("changeExperienceScript");
                    request.ChangeExperienceScript.WriteJson(jsonWriter);
                }
                if (request.ChangeRankScript != null)
                {
                    jsonWriter.WritePropertyName("changeRankScript");
                    request.ChangeRankScript.WriteJson(jsonWriter);
                }
                if (request.ChangeRankCapScript != null)
                {
                    jsonWriter.WritePropertyName("changeRankCapScript");
                    request.ChangeRankCapScript.WriteJson(jsonWriter);
                }
                if (request.OverflowExperienceScript != null)
                {
                    jsonWriter.WritePropertyName("overflowExperienceScript");
                    jsonWriter.Write(request.OverflowExperienceScript);
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
                    .Replace("{service}", "experience")
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
                    .Replace("{service}", "experience")
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
                    .Replace("{service}", "experience")
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
                if (request.RankCapScriptId != null)
                {
                    jsonWriter.WritePropertyName("rankCapScriptId");
                    jsonWriter.Write(request.RankCapScriptId);
                }
                if (request.ChangeExperienceScript != null)
                {
                    jsonWriter.WritePropertyName("changeExperienceScript");
                    request.ChangeExperienceScript.WriteJson(jsonWriter);
                }
                if (request.ChangeRankScript != null)
                {
                    jsonWriter.WritePropertyName("changeRankScript");
                    request.ChangeRankScript.WriteJson(jsonWriter);
                }
                if (request.ChangeRankCapScript != null)
                {
                    jsonWriter.WritePropertyName("changeRankCapScript");
                    request.ChangeRankCapScript.WriteJson(jsonWriter);
                }
                if (request.OverflowExperienceScript != null)
                {
                    jsonWriter.WritePropertyName("overflowExperienceScript");
                    jsonWriter.Write(request.OverflowExperienceScript);
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
                    .Replace("{service}", "experience")
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
                    .Replace("{service}", "experience")
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
                    .Replace("{service}", "experience")
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
                    .Replace("{service}", "experience")
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
                    .Replace("{service}", "experience")
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
                    .Replace("{service}", "experience")
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
                    .Replace("{service}", "experience")
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
                    .Replace("{service}", "experience")
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
                    .Replace("{service}", "experience")
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


        public class DescribeExperienceModelMastersTask : Gs2RestSessionTask<DescribeExperienceModelMastersRequest, DescribeExperienceModelMastersResult>
        {
            public DescribeExperienceModelMastersTask(IGs2Session session, RestSessionRequestFactory factory, DescribeExperienceModelMastersRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DescribeExperienceModelMastersRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "experience")
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
		public IEnumerator DescribeExperienceModelMasters(
                Request.DescribeExperienceModelMastersRequest request,
                UnityAction<AsyncResult<Result.DescribeExperienceModelMastersResult>> callback
        ) =>
            new DescribeExperienceModelMastersTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.DescribeExperienceModelMastersResult> DescribeExperienceModelMastersFuture(
                Request.DescribeExperienceModelMastersRequest request
        ) =>
            new DescribeExperienceModelMastersTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.DescribeExperienceModelMastersResult> DescribeExperienceModelMastersAsync(
                Request.DescribeExperienceModelMastersRequest request
        ) =>
            new DescribeExperienceModelMastersTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.DescribeExperienceModelMastersResult>();
    #else
		public DescribeExperienceModelMastersTask DescribeExperienceModelMastersAsync(
                Request.DescribeExperienceModelMastersRequest request
        )
		{
			return new DescribeExperienceModelMastersTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.DescribeExperienceModelMastersResult> DescribeExperienceModelMastersAsync(
                Request.DescribeExperienceModelMastersRequest request
        ) =>
            new DescribeExperienceModelMastersTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class CreateExperienceModelMasterTask : Gs2RestSessionTask<CreateExperienceModelMasterRequest, CreateExperienceModelMasterResult>
        {
            public CreateExperienceModelMasterTask(IGs2Session session, RestSessionRequestFactory factory, CreateExperienceModelMasterRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(CreateExperienceModelMasterRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "experience")
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
                if (request.DefaultExperience != null)
                {
                    jsonWriter.WritePropertyName("defaultExperience");
                    jsonWriter.Write(request.DefaultExperience.ToString());
                }
                if (request.DefaultRankCap != null)
                {
                    jsonWriter.WritePropertyName("defaultRankCap");
                    jsonWriter.Write(request.DefaultRankCap.ToString());
                }
                if (request.MaxRankCap != null)
                {
                    jsonWriter.WritePropertyName("maxRankCap");
                    jsonWriter.Write(request.MaxRankCap.ToString());
                }
                if (request.RankThresholdName != null)
                {
                    jsonWriter.WritePropertyName("rankThresholdName");
                    jsonWriter.Write(request.RankThresholdName);
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
		public IEnumerator CreateExperienceModelMaster(
                Request.CreateExperienceModelMasterRequest request,
                UnityAction<AsyncResult<Result.CreateExperienceModelMasterResult>> callback
        ) =>
            new CreateExperienceModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.CreateExperienceModelMasterResult> CreateExperienceModelMasterFuture(
                Request.CreateExperienceModelMasterRequest request
        ) =>
            new CreateExperienceModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.CreateExperienceModelMasterResult> CreateExperienceModelMasterAsync(
                Request.CreateExperienceModelMasterRequest request
        ) =>
            new CreateExperienceModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.CreateExperienceModelMasterResult>();
    #else
		public CreateExperienceModelMasterTask CreateExperienceModelMasterAsync(
                Request.CreateExperienceModelMasterRequest request
        )
		{
			return new CreateExperienceModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.CreateExperienceModelMasterResult> CreateExperienceModelMasterAsync(
                Request.CreateExperienceModelMasterRequest request
        ) =>
            new CreateExperienceModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class GetExperienceModelMasterTask : Gs2RestSessionTask<GetExperienceModelMasterRequest, GetExperienceModelMasterResult>
        {
            public GetExperienceModelMasterTask(IGs2Session session, RestSessionRequestFactory factory, GetExperienceModelMasterRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(GetExperienceModelMasterRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "experience")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/master/model/{experienceName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{experienceName}", !string.IsNullOrEmpty(request.ExperienceName) ? request.ExperienceName.ToString() : "null");

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
		public IEnumerator GetExperienceModelMaster(
                Request.GetExperienceModelMasterRequest request,
                UnityAction<AsyncResult<Result.GetExperienceModelMasterResult>> callback
        ) =>
            new GetExperienceModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.GetExperienceModelMasterResult> GetExperienceModelMasterFuture(
                Request.GetExperienceModelMasterRequest request
        ) =>
            new GetExperienceModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.GetExperienceModelMasterResult> GetExperienceModelMasterAsync(
                Request.GetExperienceModelMasterRequest request
        ) =>
            new GetExperienceModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.GetExperienceModelMasterResult>();
    #else
		public GetExperienceModelMasterTask GetExperienceModelMasterAsync(
                Request.GetExperienceModelMasterRequest request
        )
		{
			return new GetExperienceModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.GetExperienceModelMasterResult> GetExperienceModelMasterAsync(
                Request.GetExperienceModelMasterRequest request
        ) =>
            new GetExperienceModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class UpdateExperienceModelMasterTask : Gs2RestSessionTask<UpdateExperienceModelMasterRequest, UpdateExperienceModelMasterResult>
        {
            public UpdateExperienceModelMasterTask(IGs2Session session, RestSessionRequestFactory factory, UpdateExperienceModelMasterRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(UpdateExperienceModelMasterRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "experience")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/master/model/{experienceName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{experienceName}", !string.IsNullOrEmpty(request.ExperienceName) ? request.ExperienceName.ToString() : "null");

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
                if (request.DefaultExperience != null)
                {
                    jsonWriter.WritePropertyName("defaultExperience");
                    jsonWriter.Write(request.DefaultExperience.ToString());
                }
                if (request.DefaultRankCap != null)
                {
                    jsonWriter.WritePropertyName("defaultRankCap");
                    jsonWriter.Write(request.DefaultRankCap.ToString());
                }
                if (request.MaxRankCap != null)
                {
                    jsonWriter.WritePropertyName("maxRankCap");
                    jsonWriter.Write(request.MaxRankCap.ToString());
                }
                if (request.RankThresholdName != null)
                {
                    jsonWriter.WritePropertyName("rankThresholdName");
                    jsonWriter.Write(request.RankThresholdName);
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
		public IEnumerator UpdateExperienceModelMaster(
                Request.UpdateExperienceModelMasterRequest request,
                UnityAction<AsyncResult<Result.UpdateExperienceModelMasterResult>> callback
        ) =>
            new UpdateExperienceModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.UpdateExperienceModelMasterResult> UpdateExperienceModelMasterFuture(
                Request.UpdateExperienceModelMasterRequest request
        ) =>
            new UpdateExperienceModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.UpdateExperienceModelMasterResult> UpdateExperienceModelMasterAsync(
                Request.UpdateExperienceModelMasterRequest request
        ) =>
            new UpdateExperienceModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.UpdateExperienceModelMasterResult>();
    #else
		public UpdateExperienceModelMasterTask UpdateExperienceModelMasterAsync(
                Request.UpdateExperienceModelMasterRequest request
        )
		{
			return new UpdateExperienceModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.UpdateExperienceModelMasterResult> UpdateExperienceModelMasterAsync(
                Request.UpdateExperienceModelMasterRequest request
        ) =>
            new UpdateExperienceModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class DeleteExperienceModelMasterTask : Gs2RestSessionTask<DeleteExperienceModelMasterRequest, DeleteExperienceModelMasterResult>
        {
            public DeleteExperienceModelMasterTask(IGs2Session session, RestSessionRequestFactory factory, DeleteExperienceModelMasterRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DeleteExperienceModelMasterRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "experience")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/master/model/{experienceName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{experienceName}", !string.IsNullOrEmpty(request.ExperienceName) ? request.ExperienceName.ToString() : "null");

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
		public IEnumerator DeleteExperienceModelMaster(
                Request.DeleteExperienceModelMasterRequest request,
                UnityAction<AsyncResult<Result.DeleteExperienceModelMasterResult>> callback
        ) =>
            new DeleteExperienceModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.DeleteExperienceModelMasterResult> DeleteExperienceModelMasterFuture(
                Request.DeleteExperienceModelMasterRequest request
        ) =>
            new DeleteExperienceModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.DeleteExperienceModelMasterResult> DeleteExperienceModelMasterAsync(
                Request.DeleteExperienceModelMasterRequest request
        ) =>
            new DeleteExperienceModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.DeleteExperienceModelMasterResult>();
    #else
		public DeleteExperienceModelMasterTask DeleteExperienceModelMasterAsync(
                Request.DeleteExperienceModelMasterRequest request
        )
		{
			return new DeleteExperienceModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.DeleteExperienceModelMasterResult> DeleteExperienceModelMasterAsync(
                Request.DeleteExperienceModelMasterRequest request
        ) =>
            new DeleteExperienceModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class DescribeExperienceModelsTask : Gs2RestSessionTask<DescribeExperienceModelsRequest, DescribeExperienceModelsResult>
        {
            public DescribeExperienceModelsTask(IGs2Session session, RestSessionRequestFactory factory, DescribeExperienceModelsRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DescribeExperienceModelsRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "experience")
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
		public IEnumerator DescribeExperienceModels(
                Request.DescribeExperienceModelsRequest request,
                UnityAction<AsyncResult<Result.DescribeExperienceModelsResult>> callback
        ) =>
            new DescribeExperienceModelsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.DescribeExperienceModelsResult> DescribeExperienceModelsFuture(
                Request.DescribeExperienceModelsRequest request
        ) =>
            new DescribeExperienceModelsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.DescribeExperienceModelsResult> DescribeExperienceModelsAsync(
                Request.DescribeExperienceModelsRequest request
        ) =>
            new DescribeExperienceModelsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.DescribeExperienceModelsResult>();
    #else
		public DescribeExperienceModelsTask DescribeExperienceModelsAsync(
                Request.DescribeExperienceModelsRequest request
        )
		{
			return new DescribeExperienceModelsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.DescribeExperienceModelsResult> DescribeExperienceModelsAsync(
                Request.DescribeExperienceModelsRequest request
        ) =>
            new DescribeExperienceModelsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class GetExperienceModelTask : Gs2RestSessionTask<GetExperienceModelRequest, GetExperienceModelResult>
        {
            public GetExperienceModelTask(IGs2Session session, RestSessionRequestFactory factory, GetExperienceModelRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(GetExperienceModelRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "experience")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/model/{experienceName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{experienceName}", !string.IsNullOrEmpty(request.ExperienceName) ? request.ExperienceName.ToString() : "null");

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
		public IEnumerator GetExperienceModel(
                Request.GetExperienceModelRequest request,
                UnityAction<AsyncResult<Result.GetExperienceModelResult>> callback
        ) =>
            new GetExperienceModelTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.GetExperienceModelResult> GetExperienceModelFuture(
                Request.GetExperienceModelRequest request
        ) =>
            new GetExperienceModelTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.GetExperienceModelResult> GetExperienceModelAsync(
                Request.GetExperienceModelRequest request
        ) =>
            new GetExperienceModelTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.GetExperienceModelResult>();
    #else
		public GetExperienceModelTask GetExperienceModelAsync(
                Request.GetExperienceModelRequest request
        )
		{
			return new GetExperienceModelTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.GetExperienceModelResult> GetExperienceModelAsync(
                Request.GetExperienceModelRequest request
        ) =>
            new GetExperienceModelTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class DescribeThresholdMastersTask : Gs2RestSessionTask<DescribeThresholdMastersRequest, DescribeThresholdMastersResult>
        {
            public DescribeThresholdMastersTask(IGs2Session session, RestSessionRequestFactory factory, DescribeThresholdMastersRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DescribeThresholdMastersRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "experience")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/master/threshold";

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
		public IEnumerator DescribeThresholdMasters(
                Request.DescribeThresholdMastersRequest request,
                UnityAction<AsyncResult<Result.DescribeThresholdMastersResult>> callback
        ) =>
            new DescribeThresholdMastersTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.DescribeThresholdMastersResult> DescribeThresholdMastersFuture(
                Request.DescribeThresholdMastersRequest request
        ) =>
            new DescribeThresholdMastersTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.DescribeThresholdMastersResult> DescribeThresholdMastersAsync(
                Request.DescribeThresholdMastersRequest request
        ) =>
            new DescribeThresholdMastersTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.DescribeThresholdMastersResult>();
    #else
		public DescribeThresholdMastersTask DescribeThresholdMastersAsync(
                Request.DescribeThresholdMastersRequest request
        )
		{
			return new DescribeThresholdMastersTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.DescribeThresholdMastersResult> DescribeThresholdMastersAsync(
                Request.DescribeThresholdMastersRequest request
        ) =>
            new DescribeThresholdMastersTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class CreateThresholdMasterTask : Gs2RestSessionTask<CreateThresholdMasterRequest, CreateThresholdMasterResult>
        {
            public CreateThresholdMasterTask(IGs2Session session, RestSessionRequestFactory factory, CreateThresholdMasterRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(CreateThresholdMasterRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "experience")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/master/threshold";

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
		public IEnumerator CreateThresholdMaster(
                Request.CreateThresholdMasterRequest request,
                UnityAction<AsyncResult<Result.CreateThresholdMasterResult>> callback
        ) =>
            new CreateThresholdMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.CreateThresholdMasterResult> CreateThresholdMasterFuture(
                Request.CreateThresholdMasterRequest request
        ) =>
            new CreateThresholdMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.CreateThresholdMasterResult> CreateThresholdMasterAsync(
                Request.CreateThresholdMasterRequest request
        ) =>
            new CreateThresholdMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.CreateThresholdMasterResult>();
    #else
		public CreateThresholdMasterTask CreateThresholdMasterAsync(
                Request.CreateThresholdMasterRequest request
        )
		{
			return new CreateThresholdMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.CreateThresholdMasterResult> CreateThresholdMasterAsync(
                Request.CreateThresholdMasterRequest request
        ) =>
            new CreateThresholdMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class GetThresholdMasterTask : Gs2RestSessionTask<GetThresholdMasterRequest, GetThresholdMasterResult>
        {
            public GetThresholdMasterTask(IGs2Session session, RestSessionRequestFactory factory, GetThresholdMasterRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(GetThresholdMasterRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "experience")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/master/threshold/{thresholdName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{thresholdName}", !string.IsNullOrEmpty(request.ThresholdName) ? request.ThresholdName.ToString() : "null");

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
		public IEnumerator GetThresholdMaster(
                Request.GetThresholdMasterRequest request,
                UnityAction<AsyncResult<Result.GetThresholdMasterResult>> callback
        ) =>
            new GetThresholdMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.GetThresholdMasterResult> GetThresholdMasterFuture(
                Request.GetThresholdMasterRequest request
        ) =>
            new GetThresholdMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.GetThresholdMasterResult> GetThresholdMasterAsync(
                Request.GetThresholdMasterRequest request
        ) =>
            new GetThresholdMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.GetThresholdMasterResult>();
    #else
		public GetThresholdMasterTask GetThresholdMasterAsync(
                Request.GetThresholdMasterRequest request
        )
		{
			return new GetThresholdMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.GetThresholdMasterResult> GetThresholdMasterAsync(
                Request.GetThresholdMasterRequest request
        ) =>
            new GetThresholdMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class UpdateThresholdMasterTask : Gs2RestSessionTask<UpdateThresholdMasterRequest, UpdateThresholdMasterResult>
        {
            public UpdateThresholdMasterTask(IGs2Session session, RestSessionRequestFactory factory, UpdateThresholdMasterRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(UpdateThresholdMasterRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "experience")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/master/threshold/{thresholdName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{thresholdName}", !string.IsNullOrEmpty(request.ThresholdName) ? request.ThresholdName.ToString() : "null");

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
		public IEnumerator UpdateThresholdMaster(
                Request.UpdateThresholdMasterRequest request,
                UnityAction<AsyncResult<Result.UpdateThresholdMasterResult>> callback
        ) =>
            new UpdateThresholdMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.UpdateThresholdMasterResult> UpdateThresholdMasterFuture(
                Request.UpdateThresholdMasterRequest request
        ) =>
            new UpdateThresholdMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.UpdateThresholdMasterResult> UpdateThresholdMasterAsync(
                Request.UpdateThresholdMasterRequest request
        ) =>
            new UpdateThresholdMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.UpdateThresholdMasterResult>();
    #else
		public UpdateThresholdMasterTask UpdateThresholdMasterAsync(
                Request.UpdateThresholdMasterRequest request
        )
		{
			return new UpdateThresholdMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.UpdateThresholdMasterResult> UpdateThresholdMasterAsync(
                Request.UpdateThresholdMasterRequest request
        ) =>
            new UpdateThresholdMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class DeleteThresholdMasterTask : Gs2RestSessionTask<DeleteThresholdMasterRequest, DeleteThresholdMasterResult>
        {
            public DeleteThresholdMasterTask(IGs2Session session, RestSessionRequestFactory factory, DeleteThresholdMasterRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DeleteThresholdMasterRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "experience")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/master/threshold/{thresholdName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{thresholdName}", !string.IsNullOrEmpty(request.ThresholdName) ? request.ThresholdName.ToString() : "null");

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
		public IEnumerator DeleteThresholdMaster(
                Request.DeleteThresholdMasterRequest request,
                UnityAction<AsyncResult<Result.DeleteThresholdMasterResult>> callback
        ) =>
            new DeleteThresholdMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.DeleteThresholdMasterResult> DeleteThresholdMasterFuture(
                Request.DeleteThresholdMasterRequest request
        ) =>
            new DeleteThresholdMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.DeleteThresholdMasterResult> DeleteThresholdMasterAsync(
                Request.DeleteThresholdMasterRequest request
        ) =>
            new DeleteThresholdMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.DeleteThresholdMasterResult>();
    #else
		public DeleteThresholdMasterTask DeleteThresholdMasterAsync(
                Request.DeleteThresholdMasterRequest request
        )
		{
			return new DeleteThresholdMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.DeleteThresholdMasterResult> DeleteThresholdMasterAsync(
                Request.DeleteThresholdMasterRequest request
        ) =>
            new DeleteThresholdMasterTask(
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
                    .Replace("{service}", "experience")
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


        public class GetCurrentExperienceMasterTask : Gs2RestSessionTask<GetCurrentExperienceMasterRequest, GetCurrentExperienceMasterResult>
        {
            public GetCurrentExperienceMasterTask(IGs2Session session, RestSessionRequestFactory factory, GetCurrentExperienceMasterRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(GetCurrentExperienceMasterRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "experience")
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
		public IEnumerator GetCurrentExperienceMaster(
                Request.GetCurrentExperienceMasterRequest request,
                UnityAction<AsyncResult<Result.GetCurrentExperienceMasterResult>> callback
        ) =>
            new GetCurrentExperienceMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.GetCurrentExperienceMasterResult> GetCurrentExperienceMasterFuture(
                Request.GetCurrentExperienceMasterRequest request
        ) =>
            new GetCurrentExperienceMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.GetCurrentExperienceMasterResult> GetCurrentExperienceMasterAsync(
                Request.GetCurrentExperienceMasterRequest request
        ) =>
            new GetCurrentExperienceMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.GetCurrentExperienceMasterResult>();
    #else
		public GetCurrentExperienceMasterTask GetCurrentExperienceMasterAsync(
                Request.GetCurrentExperienceMasterRequest request
        )
		{
			return new GetCurrentExperienceMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.GetCurrentExperienceMasterResult> GetCurrentExperienceMasterAsync(
                Request.GetCurrentExperienceMasterRequest request
        ) =>
            new GetCurrentExperienceMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class PreUpdateCurrentExperienceMasterTask : Gs2RestSessionTask<PreUpdateCurrentExperienceMasterRequest, PreUpdateCurrentExperienceMasterResult>
        {
            public PreUpdateCurrentExperienceMasterTask(IGs2Session session, RestSessionRequestFactory factory, PreUpdateCurrentExperienceMasterRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(PreUpdateCurrentExperienceMasterRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "experience")
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
		public IEnumerator PreUpdateCurrentExperienceMaster(
                Request.PreUpdateCurrentExperienceMasterRequest request,
                UnityAction<AsyncResult<Result.PreUpdateCurrentExperienceMasterResult>> callback
        ) =>
            new PreUpdateCurrentExperienceMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.PreUpdateCurrentExperienceMasterResult> PreUpdateCurrentExperienceMasterFuture(
                Request.PreUpdateCurrentExperienceMasterRequest request
        ) =>
            new PreUpdateCurrentExperienceMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.PreUpdateCurrentExperienceMasterResult> PreUpdateCurrentExperienceMasterAsync(
                Request.PreUpdateCurrentExperienceMasterRequest request
        ) =>
            new PreUpdateCurrentExperienceMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.PreUpdateCurrentExperienceMasterResult>();
    #else
		public PreUpdateCurrentExperienceMasterTask PreUpdateCurrentExperienceMasterAsync(
                Request.PreUpdateCurrentExperienceMasterRequest request
        )
		{
			return new PreUpdateCurrentExperienceMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.PreUpdateCurrentExperienceMasterResult> PreUpdateCurrentExperienceMasterAsync(
                Request.PreUpdateCurrentExperienceMasterRequest request
        ) =>
            new PreUpdateCurrentExperienceMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class UpdateCurrentExperienceMasterTask : Gs2RestSessionTask<UpdateCurrentExperienceMasterRequest, UpdateCurrentExperienceMasterResult>
        {
            public UpdateCurrentExperienceMasterTask(IGs2Session session, RestSessionRequestFactory factory, UpdateCurrentExperienceMasterRequest request) : base(session, factory, request)
            {
            }
#if GS2_ENABLE_UNITASK
            protected override async UniTask<UpdateCurrentExperienceMasterResult> InvokeImpl()
#else
            protected override async Task<UpdateCurrentExperienceMasterResult> InvokeImpl()
#endif
            {
                if (Request.Settings != null) {
                    var preTask = new PreUpdateCurrentExperienceMasterTask(
                        Session,
                        Factory,
                        new PreUpdateCurrentExperienceMasterRequest()
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

            protected override IGs2SessionRequest CreateRequest(UpdateCurrentExperienceMasterRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "experience")
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
		public IEnumerator UpdateCurrentExperienceMaster(
                Request.UpdateCurrentExperienceMasterRequest request,
                UnityAction<AsyncResult<Result.UpdateCurrentExperienceMasterResult>> callback
        ) =>
            new UpdateCurrentExperienceMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.UpdateCurrentExperienceMasterResult> UpdateCurrentExperienceMasterFuture(
                Request.UpdateCurrentExperienceMasterRequest request
        ) =>
            new UpdateCurrentExperienceMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.UpdateCurrentExperienceMasterResult> UpdateCurrentExperienceMasterAsync(
                Request.UpdateCurrentExperienceMasterRequest request
        ) =>
            new UpdateCurrentExperienceMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.UpdateCurrentExperienceMasterResult>();
    #else
		public UpdateCurrentExperienceMasterTask UpdateCurrentExperienceMasterAsync(
                Request.UpdateCurrentExperienceMasterRequest request
        )
		{
			return new UpdateCurrentExperienceMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.UpdateCurrentExperienceMasterResult> UpdateCurrentExperienceMasterAsync(
                Request.UpdateCurrentExperienceMasterRequest request
        ) =>
            new UpdateCurrentExperienceMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class UpdateCurrentExperienceMasterFromGitHubTask : Gs2RestSessionTask<UpdateCurrentExperienceMasterFromGitHubRequest, UpdateCurrentExperienceMasterFromGitHubResult>
        {
            public UpdateCurrentExperienceMasterFromGitHubTask(IGs2Session session, RestSessionRequestFactory factory, UpdateCurrentExperienceMasterFromGitHubRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(UpdateCurrentExperienceMasterFromGitHubRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "experience")
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
		public IEnumerator UpdateCurrentExperienceMasterFromGitHub(
                Request.UpdateCurrentExperienceMasterFromGitHubRequest request,
                UnityAction<AsyncResult<Result.UpdateCurrentExperienceMasterFromGitHubResult>> callback
        ) =>
            new UpdateCurrentExperienceMasterFromGitHubTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.UpdateCurrentExperienceMasterFromGitHubResult> UpdateCurrentExperienceMasterFromGitHubFuture(
                Request.UpdateCurrentExperienceMasterFromGitHubRequest request
        ) =>
            new UpdateCurrentExperienceMasterFromGitHubTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.UpdateCurrentExperienceMasterFromGitHubResult> UpdateCurrentExperienceMasterFromGitHubAsync(
                Request.UpdateCurrentExperienceMasterFromGitHubRequest request
        ) =>
            new UpdateCurrentExperienceMasterFromGitHubTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.UpdateCurrentExperienceMasterFromGitHubResult>();
    #else
		public UpdateCurrentExperienceMasterFromGitHubTask UpdateCurrentExperienceMasterFromGitHubAsync(
                Request.UpdateCurrentExperienceMasterFromGitHubRequest request
        )
		{
			return new UpdateCurrentExperienceMasterFromGitHubTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.UpdateCurrentExperienceMasterFromGitHubResult> UpdateCurrentExperienceMasterFromGitHubAsync(
                Request.UpdateCurrentExperienceMasterFromGitHubRequest request
        ) =>
            new UpdateCurrentExperienceMasterFromGitHubTask(
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
                    .Replace("{service}", "experience")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/me/status";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");

                var sessionRequest = Factory.Get(url);
                if (request.ContextStack != null)
                {
                    sessionRequest.AddQueryString("contextStack", request.ContextStack);
                }
                if (request.ExperienceName != null) {
                    sessionRequest.AddQueryString("experienceName", $"{request.ExperienceName}");
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
                    .Replace("{service}", "experience")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/{userId}/status";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{userId}", !string.IsNullOrEmpty(request.UserId) ? request.UserId.ToString() : "null");

                var sessionRequest = Factory.Get(url);
                if (request.ContextStack != null)
                {
                    sessionRequest.AddQueryString("contextStack", request.ContextStack);
                }
                if (request.ExperienceName != null) {
                    sessionRequest.AddQueryString("experienceName", $"{request.ExperienceName}");
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
                    .Replace("{service}", "experience")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/me/status/model/{experienceName}/property/{propertyId}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{experienceName}", !string.IsNullOrEmpty(request.ExperienceName) ? request.ExperienceName.ToString() : "null");
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
                    .Replace("{service}", "experience")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/{userId}/status/model/{experienceName}/property/{propertyId}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{userId}", !string.IsNullOrEmpty(request.UserId) ? request.UserId.ToString() : "null");
                url = url.Replace("{experienceName}", !string.IsNullOrEmpty(request.ExperienceName) ? request.ExperienceName.ToString() : "null");
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


        public class GetStatusWithSignatureTask : Gs2RestSessionTask<GetStatusWithSignatureRequest, GetStatusWithSignatureResult>
        {
            public GetStatusWithSignatureTask(IGs2Session session, RestSessionRequestFactory factory, GetStatusWithSignatureRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(GetStatusWithSignatureRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "experience")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/me/status/model/{experienceName}/property/{propertyId}/signature";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{experienceName}", !string.IsNullOrEmpty(request.ExperienceName) ? request.ExperienceName.ToString() : "null");
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
		public IEnumerator GetStatusWithSignature(
                Request.GetStatusWithSignatureRequest request,
                UnityAction<AsyncResult<Result.GetStatusWithSignatureResult>> callback
        ) =>
            new GetStatusWithSignatureTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.GetStatusWithSignatureResult> GetStatusWithSignatureFuture(
                Request.GetStatusWithSignatureRequest request
        ) =>
            new GetStatusWithSignatureTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.GetStatusWithSignatureResult> GetStatusWithSignatureAsync(
                Request.GetStatusWithSignatureRequest request
        ) =>
            new GetStatusWithSignatureTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.GetStatusWithSignatureResult>();
    #else
		public GetStatusWithSignatureTask GetStatusWithSignatureAsync(
                Request.GetStatusWithSignatureRequest request
        )
		{
			return new GetStatusWithSignatureTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.GetStatusWithSignatureResult> GetStatusWithSignatureAsync(
                Request.GetStatusWithSignatureRequest request
        ) =>
            new GetStatusWithSignatureTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class GetStatusWithSignatureByUserIdTask : Gs2RestSessionTask<GetStatusWithSignatureByUserIdRequest, GetStatusWithSignatureByUserIdResult>
        {
            public GetStatusWithSignatureByUserIdTask(IGs2Session session, RestSessionRequestFactory factory, GetStatusWithSignatureByUserIdRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(GetStatusWithSignatureByUserIdRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "experience")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/{userId}/status/model/{experienceName}/property/{propertyId}/signature";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{userId}", !string.IsNullOrEmpty(request.UserId) ? request.UserId.ToString() : "null");
                url = url.Replace("{experienceName}", !string.IsNullOrEmpty(request.ExperienceName) ? request.ExperienceName.ToString() : "null");
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
		public IEnumerator GetStatusWithSignatureByUserId(
                Request.GetStatusWithSignatureByUserIdRequest request,
                UnityAction<AsyncResult<Result.GetStatusWithSignatureByUserIdResult>> callback
        ) =>
            new GetStatusWithSignatureByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.GetStatusWithSignatureByUserIdResult> GetStatusWithSignatureByUserIdFuture(
                Request.GetStatusWithSignatureByUserIdRequest request
        ) =>
            new GetStatusWithSignatureByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.GetStatusWithSignatureByUserIdResult> GetStatusWithSignatureByUserIdAsync(
                Request.GetStatusWithSignatureByUserIdRequest request
        ) =>
            new GetStatusWithSignatureByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.GetStatusWithSignatureByUserIdResult>();
    #else
		public GetStatusWithSignatureByUserIdTask GetStatusWithSignatureByUserIdAsync(
                Request.GetStatusWithSignatureByUserIdRequest request
        )
		{
			return new GetStatusWithSignatureByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.GetStatusWithSignatureByUserIdResult> GetStatusWithSignatureByUserIdAsync(
                Request.GetStatusWithSignatureByUserIdRequest request
        ) =>
            new GetStatusWithSignatureByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class AddExperienceByUserIdTask : Gs2RestSessionTask<AddExperienceByUserIdRequest, AddExperienceByUserIdResult>
        {
            public AddExperienceByUserIdTask(IGs2Session session, RestSessionRequestFactory factory, AddExperienceByUserIdRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(AddExperienceByUserIdRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "experience")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/{userId}/status/model/{experienceName}/property/{propertyId}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{userId}", !string.IsNullOrEmpty(request.UserId) ? request.UserId.ToString() : "null");
                url = url.Replace("{experienceName}", !string.IsNullOrEmpty(request.ExperienceName) ? request.ExperienceName.ToString() : "null");
                url = url.Replace("{propertyId}", !string.IsNullOrEmpty(request.PropertyId) ? request.PropertyId.ToString() : "null");

                var sessionRequest = Factory.Post(url);

                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);
                jsonWriter.WriteObjectStart();
                if (request.ExperienceValue != null)
                {
                    jsonWriter.WritePropertyName("experienceValue");
                    jsonWriter.Write(request.ExperienceValue.ToString());
                }
                if (request.TruncateExperienceWhenRankUp != null)
                {
                    jsonWriter.WritePropertyName("truncateExperienceWhenRankUp");
                    jsonWriter.Write(request.TruncateExperienceWhenRankUp.ToString());
                }
                if (request.ContextStack != null)
                {
                    jsonWriter.WritePropertyName("contextStack");
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
		public IEnumerator AddExperienceByUserId(
                Request.AddExperienceByUserIdRequest request,
                UnityAction<AsyncResult<Result.AddExperienceByUserIdResult>> callback
        ) =>
            new AddExperienceByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.AddExperienceByUserIdResult> AddExperienceByUserIdFuture(
                Request.AddExperienceByUserIdRequest request
        ) =>
            new AddExperienceByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.AddExperienceByUserIdResult> AddExperienceByUserIdAsync(
                Request.AddExperienceByUserIdRequest request
        ) =>
            new AddExperienceByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.AddExperienceByUserIdResult>();
    #else
		public AddExperienceByUserIdTask AddExperienceByUserIdAsync(
                Request.AddExperienceByUserIdRequest request
        )
		{
			return new AddExperienceByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.AddExperienceByUserIdResult> AddExperienceByUserIdAsync(
                Request.AddExperienceByUserIdRequest request
        ) =>
            new AddExperienceByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class SubExperienceTask : Gs2RestSessionTask<SubExperienceRequest, SubExperienceResult>
        {
            public SubExperienceTask(IGs2Session session, RestSessionRequestFactory factory, SubExperienceRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(SubExperienceRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "experience")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/me/status/model/{experienceName}/property/{propertyId}/sub";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{experienceName}", !string.IsNullOrEmpty(request.ExperienceName) ? request.ExperienceName.ToString() : "null");
                url = url.Replace("{propertyId}", !string.IsNullOrEmpty(request.PropertyId) ? request.PropertyId.ToString() : "null");

                var sessionRequest = Factory.Post(url);

                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);
                jsonWriter.WriteObjectStart();
                if (request.ExperienceValue != null)
                {
                    jsonWriter.WritePropertyName("experienceValue");
                    jsonWriter.Write(request.ExperienceValue.ToString());
                }
                if (request.ContextStack != null)
                {
                    jsonWriter.WritePropertyName("contextStack");
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
		public IEnumerator SubExperience(
                Request.SubExperienceRequest request,
                UnityAction<AsyncResult<Result.SubExperienceResult>> callback
        ) =>
            new SubExperienceTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.SubExperienceResult> SubExperienceFuture(
                Request.SubExperienceRequest request
        ) =>
            new SubExperienceTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.SubExperienceResult> SubExperienceAsync(
                Request.SubExperienceRequest request
        ) =>
            new SubExperienceTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.SubExperienceResult>();
    #else
		public SubExperienceTask SubExperienceAsync(
                Request.SubExperienceRequest request
        )
		{
			return new SubExperienceTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.SubExperienceResult> SubExperienceAsync(
                Request.SubExperienceRequest request
        ) =>
            new SubExperienceTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class SubExperienceByUserIdTask : Gs2RestSessionTask<SubExperienceByUserIdRequest, SubExperienceByUserIdResult>
        {
            public SubExperienceByUserIdTask(IGs2Session session, RestSessionRequestFactory factory, SubExperienceByUserIdRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(SubExperienceByUserIdRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "experience")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/{userId}/status/model/{experienceName}/property/{propertyId}/sub";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{userId}", !string.IsNullOrEmpty(request.UserId) ? request.UserId.ToString() : "null");
                url = url.Replace("{experienceName}", !string.IsNullOrEmpty(request.ExperienceName) ? request.ExperienceName.ToString() : "null");
                url = url.Replace("{propertyId}", !string.IsNullOrEmpty(request.PropertyId) ? request.PropertyId.ToString() : "null");

                var sessionRequest = Factory.Post(url);

                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);
                jsonWriter.WriteObjectStart();
                if (request.ExperienceValue != null)
                {
                    jsonWriter.WritePropertyName("experienceValue");
                    jsonWriter.Write(request.ExperienceValue.ToString());
                }
                if (request.ContextStack != null)
                {
                    jsonWriter.WritePropertyName("contextStack");
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
		public IEnumerator SubExperienceByUserId(
                Request.SubExperienceByUserIdRequest request,
                UnityAction<AsyncResult<Result.SubExperienceByUserIdResult>> callback
        ) =>
            new SubExperienceByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.SubExperienceByUserIdResult> SubExperienceByUserIdFuture(
                Request.SubExperienceByUserIdRequest request
        ) =>
            new SubExperienceByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.SubExperienceByUserIdResult> SubExperienceByUserIdAsync(
                Request.SubExperienceByUserIdRequest request
        ) =>
            new SubExperienceByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.SubExperienceByUserIdResult>();
    #else
		public SubExperienceByUserIdTask SubExperienceByUserIdAsync(
                Request.SubExperienceByUserIdRequest request
        )
		{
			return new SubExperienceByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.SubExperienceByUserIdResult> SubExperienceByUserIdAsync(
                Request.SubExperienceByUserIdRequest request
        ) =>
            new SubExperienceByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class SetExperienceByUserIdTask : Gs2RestSessionTask<SetExperienceByUserIdRequest, SetExperienceByUserIdResult>
        {
            public SetExperienceByUserIdTask(IGs2Session session, RestSessionRequestFactory factory, SetExperienceByUserIdRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(SetExperienceByUserIdRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "experience")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/{userId}/status/model/{experienceName}/property/{propertyId}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{userId}", !string.IsNullOrEmpty(request.UserId) ? request.UserId.ToString() : "null");
                url = url.Replace("{experienceName}", !string.IsNullOrEmpty(request.ExperienceName) ? request.ExperienceName.ToString() : "null");
                url = url.Replace("{propertyId}", !string.IsNullOrEmpty(request.PropertyId) ? request.PropertyId.ToString() : "null");

                var sessionRequest = Factory.Put(url);

                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);
                jsonWriter.WriteObjectStart();
                if (request.ExperienceValue != null)
                {
                    jsonWriter.WritePropertyName("experienceValue");
                    jsonWriter.Write(request.ExperienceValue.ToString());
                }
                if (request.ContextStack != null)
                {
                    jsonWriter.WritePropertyName("contextStack");
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
		public IEnumerator SetExperienceByUserId(
                Request.SetExperienceByUserIdRequest request,
                UnityAction<AsyncResult<Result.SetExperienceByUserIdResult>> callback
        ) =>
            new SetExperienceByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.SetExperienceByUserIdResult> SetExperienceByUserIdFuture(
                Request.SetExperienceByUserIdRequest request
        ) =>
            new SetExperienceByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.SetExperienceByUserIdResult> SetExperienceByUserIdAsync(
                Request.SetExperienceByUserIdRequest request
        ) =>
            new SetExperienceByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.SetExperienceByUserIdResult>();
    #else
		public SetExperienceByUserIdTask SetExperienceByUserIdAsync(
                Request.SetExperienceByUserIdRequest request
        )
		{
			return new SetExperienceByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.SetExperienceByUserIdResult> SetExperienceByUserIdAsync(
                Request.SetExperienceByUserIdRequest request
        ) =>
            new SetExperienceByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class AddRankCapByUserIdTask : Gs2RestSessionTask<AddRankCapByUserIdRequest, AddRankCapByUserIdResult>
        {
            public AddRankCapByUserIdTask(IGs2Session session, RestSessionRequestFactory factory, AddRankCapByUserIdRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(AddRankCapByUserIdRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "experience")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/{userId}/status/model/{experienceName}/property/{propertyId}/cap";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{userId}", !string.IsNullOrEmpty(request.UserId) ? request.UserId.ToString() : "null");
                url = url.Replace("{experienceName}", !string.IsNullOrEmpty(request.ExperienceName) ? request.ExperienceName.ToString() : "null");
                url = url.Replace("{propertyId}", !string.IsNullOrEmpty(request.PropertyId) ? request.PropertyId.ToString() : "null");

                var sessionRequest = Factory.Post(url);

                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);
                jsonWriter.WriteObjectStart();
                if (request.RankCapValue != null)
                {
                    jsonWriter.WritePropertyName("rankCapValue");
                    jsonWriter.Write(request.RankCapValue.ToString());
                }
                if (request.ContextStack != null)
                {
                    jsonWriter.WritePropertyName("contextStack");
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
		public IEnumerator AddRankCapByUserId(
                Request.AddRankCapByUserIdRequest request,
                UnityAction<AsyncResult<Result.AddRankCapByUserIdResult>> callback
        ) =>
            new AddRankCapByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.AddRankCapByUserIdResult> AddRankCapByUserIdFuture(
                Request.AddRankCapByUserIdRequest request
        ) =>
            new AddRankCapByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.AddRankCapByUserIdResult> AddRankCapByUserIdAsync(
                Request.AddRankCapByUserIdRequest request
        ) =>
            new AddRankCapByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.AddRankCapByUserIdResult>();
    #else
		public AddRankCapByUserIdTask AddRankCapByUserIdAsync(
                Request.AddRankCapByUserIdRequest request
        )
		{
			return new AddRankCapByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.AddRankCapByUserIdResult> AddRankCapByUserIdAsync(
                Request.AddRankCapByUserIdRequest request
        ) =>
            new AddRankCapByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class SubRankCapTask : Gs2RestSessionTask<SubRankCapRequest, SubRankCapResult>
        {
            public SubRankCapTask(IGs2Session session, RestSessionRequestFactory factory, SubRankCapRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(SubRankCapRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "experience")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/me/status/model/{experienceName}/property/{propertyId}/cap/sub";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{experienceName}", !string.IsNullOrEmpty(request.ExperienceName) ? request.ExperienceName.ToString() : "null");
                url = url.Replace("{propertyId}", !string.IsNullOrEmpty(request.PropertyId) ? request.PropertyId.ToString() : "null");

                var sessionRequest = Factory.Post(url);

                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);
                jsonWriter.WriteObjectStart();
                if (request.RankCapValue != null)
                {
                    jsonWriter.WritePropertyName("rankCapValue");
                    jsonWriter.Write(request.RankCapValue.ToString());
                }
                if (request.ContextStack != null)
                {
                    jsonWriter.WritePropertyName("contextStack");
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
		public IEnumerator SubRankCap(
                Request.SubRankCapRequest request,
                UnityAction<AsyncResult<Result.SubRankCapResult>> callback
        ) =>
            new SubRankCapTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.SubRankCapResult> SubRankCapFuture(
                Request.SubRankCapRequest request
        ) =>
            new SubRankCapTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.SubRankCapResult> SubRankCapAsync(
                Request.SubRankCapRequest request
        ) =>
            new SubRankCapTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.SubRankCapResult>();
    #else
		public SubRankCapTask SubRankCapAsync(
                Request.SubRankCapRequest request
        )
		{
			return new SubRankCapTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.SubRankCapResult> SubRankCapAsync(
                Request.SubRankCapRequest request
        ) =>
            new SubRankCapTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class SubRankCapByUserIdTask : Gs2RestSessionTask<SubRankCapByUserIdRequest, SubRankCapByUserIdResult>
        {
            public SubRankCapByUserIdTask(IGs2Session session, RestSessionRequestFactory factory, SubRankCapByUserIdRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(SubRankCapByUserIdRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "experience")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/{userId}/status/model/{experienceName}/property/{propertyId}/cap/sub";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{userId}", !string.IsNullOrEmpty(request.UserId) ? request.UserId.ToString() : "null");
                url = url.Replace("{experienceName}", !string.IsNullOrEmpty(request.ExperienceName) ? request.ExperienceName.ToString() : "null");
                url = url.Replace("{propertyId}", !string.IsNullOrEmpty(request.PropertyId) ? request.PropertyId.ToString() : "null");

                var sessionRequest = Factory.Post(url);

                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);
                jsonWriter.WriteObjectStart();
                if (request.RankCapValue != null)
                {
                    jsonWriter.WritePropertyName("rankCapValue");
                    jsonWriter.Write(request.RankCapValue.ToString());
                }
                if (request.ContextStack != null)
                {
                    jsonWriter.WritePropertyName("contextStack");
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
		public IEnumerator SubRankCapByUserId(
                Request.SubRankCapByUserIdRequest request,
                UnityAction<AsyncResult<Result.SubRankCapByUserIdResult>> callback
        ) =>
            new SubRankCapByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.SubRankCapByUserIdResult> SubRankCapByUserIdFuture(
                Request.SubRankCapByUserIdRequest request
        ) =>
            new SubRankCapByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.SubRankCapByUserIdResult> SubRankCapByUserIdAsync(
                Request.SubRankCapByUserIdRequest request
        ) =>
            new SubRankCapByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.SubRankCapByUserIdResult>();
    #else
		public SubRankCapByUserIdTask SubRankCapByUserIdAsync(
                Request.SubRankCapByUserIdRequest request
        )
		{
			return new SubRankCapByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.SubRankCapByUserIdResult> SubRankCapByUserIdAsync(
                Request.SubRankCapByUserIdRequest request
        ) =>
            new SubRankCapByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class SetRankCapByUserIdTask : Gs2RestSessionTask<SetRankCapByUserIdRequest, SetRankCapByUserIdResult>
        {
            public SetRankCapByUserIdTask(IGs2Session session, RestSessionRequestFactory factory, SetRankCapByUserIdRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(SetRankCapByUserIdRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "experience")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/{userId}/status/model/{experienceName}/property/{propertyId}/cap";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{userId}", !string.IsNullOrEmpty(request.UserId) ? request.UserId.ToString() : "null");
                url = url.Replace("{experienceName}", !string.IsNullOrEmpty(request.ExperienceName) ? request.ExperienceName.ToString() : "null");
                url = url.Replace("{propertyId}", !string.IsNullOrEmpty(request.PropertyId) ? request.PropertyId.ToString() : "null");

                var sessionRequest = Factory.Put(url);

                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);
                jsonWriter.WriteObjectStart();
                if (request.RankCapValue != null)
                {
                    jsonWriter.WritePropertyName("rankCapValue");
                    jsonWriter.Write(request.RankCapValue.ToString());
                }
                if (request.ContextStack != null)
                {
                    jsonWriter.WritePropertyName("contextStack");
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
		public IEnumerator SetRankCapByUserId(
                Request.SetRankCapByUserIdRequest request,
                UnityAction<AsyncResult<Result.SetRankCapByUserIdResult>> callback
        ) =>
            new SetRankCapByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.SetRankCapByUserIdResult> SetRankCapByUserIdFuture(
                Request.SetRankCapByUserIdRequest request
        ) =>
            new SetRankCapByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.SetRankCapByUserIdResult> SetRankCapByUserIdAsync(
                Request.SetRankCapByUserIdRequest request
        ) =>
            new SetRankCapByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.SetRankCapByUserIdResult>();
    #else
		public SetRankCapByUserIdTask SetRankCapByUserIdAsync(
                Request.SetRankCapByUserIdRequest request
        )
		{
			return new SetRankCapByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.SetRankCapByUserIdResult> SetRankCapByUserIdAsync(
                Request.SetRankCapByUserIdRequest request
        ) =>
            new SetRankCapByUserIdTask(
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
                    .Replace("{service}", "experience")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/{userId}/status/model/{experienceName}/property/{propertyId}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{userId}", !string.IsNullOrEmpty(request.UserId) ? request.UserId.ToString() : "null");
                url = url.Replace("{experienceName}", !string.IsNullOrEmpty(request.ExperienceName) ? request.ExperienceName.ToString() : "null");
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


        public class VerifyRankTask : Gs2RestSessionTask<VerifyRankRequest, VerifyRankResult>
        {
            public VerifyRankTask(IGs2Session session, RestSessionRequestFactory factory, VerifyRankRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(VerifyRankRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "experience")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/me/status/{experienceName}/verify/rank/{verifyType}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{experienceName}", !string.IsNullOrEmpty(request.ExperienceName) ? request.ExperienceName.ToString() : "null");
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
                if (request.RankValue != null)
                {
                    jsonWriter.WritePropertyName("rankValue");
                    jsonWriter.Write(request.RankValue.ToString());
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
		public IEnumerator VerifyRank(
                Request.VerifyRankRequest request,
                UnityAction<AsyncResult<Result.VerifyRankResult>> callback
        ) =>
            new VerifyRankTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.VerifyRankResult> VerifyRankFuture(
                Request.VerifyRankRequest request
        ) =>
            new VerifyRankTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.VerifyRankResult> VerifyRankAsync(
                Request.VerifyRankRequest request
        ) =>
            new VerifyRankTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.VerifyRankResult>();
    #else
		public VerifyRankTask VerifyRankAsync(
                Request.VerifyRankRequest request
        )
		{
			return new VerifyRankTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.VerifyRankResult> VerifyRankAsync(
                Request.VerifyRankRequest request
        ) =>
            new VerifyRankTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class VerifyRankByUserIdTask : Gs2RestSessionTask<VerifyRankByUserIdRequest, VerifyRankByUserIdResult>
        {
            public VerifyRankByUserIdTask(IGs2Session session, RestSessionRequestFactory factory, VerifyRankByUserIdRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(VerifyRankByUserIdRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "experience")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/{userId}/status/{experienceName}/verify/rank/{verifyType}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{userId}", !string.IsNullOrEmpty(request.UserId) ? request.UserId.ToString() : "null");
                url = url.Replace("{experienceName}", !string.IsNullOrEmpty(request.ExperienceName) ? request.ExperienceName.ToString() : "null");
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
                if (request.RankValue != null)
                {
                    jsonWriter.WritePropertyName("rankValue");
                    jsonWriter.Write(request.RankValue.ToString());
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
		public IEnumerator VerifyRankByUserId(
                Request.VerifyRankByUserIdRequest request,
                UnityAction<AsyncResult<Result.VerifyRankByUserIdResult>> callback
        ) =>
            new VerifyRankByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.VerifyRankByUserIdResult> VerifyRankByUserIdFuture(
                Request.VerifyRankByUserIdRequest request
        ) =>
            new VerifyRankByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.VerifyRankByUserIdResult> VerifyRankByUserIdAsync(
                Request.VerifyRankByUserIdRequest request
        ) =>
            new VerifyRankByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.VerifyRankByUserIdResult>();
    #else
		public VerifyRankByUserIdTask VerifyRankByUserIdAsync(
                Request.VerifyRankByUserIdRequest request
        )
		{
			return new VerifyRankByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.VerifyRankByUserIdResult> VerifyRankByUserIdAsync(
                Request.VerifyRankByUserIdRequest request
        ) =>
            new VerifyRankByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class VerifyRankCapTask : Gs2RestSessionTask<VerifyRankCapRequest, VerifyRankCapResult>
        {
            public VerifyRankCapTask(IGs2Session session, RestSessionRequestFactory factory, VerifyRankCapRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(VerifyRankCapRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "experience")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/me/status/{experienceName}/verify/rankCap/{verifyType}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{experienceName}", !string.IsNullOrEmpty(request.ExperienceName) ? request.ExperienceName.ToString() : "null");
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
                if (request.RankCapValue != null)
                {
                    jsonWriter.WritePropertyName("rankCapValue");
                    jsonWriter.Write(request.RankCapValue.ToString());
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
		public IEnumerator VerifyRankCap(
                Request.VerifyRankCapRequest request,
                UnityAction<AsyncResult<Result.VerifyRankCapResult>> callback
        ) =>
            new VerifyRankCapTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.VerifyRankCapResult> VerifyRankCapFuture(
                Request.VerifyRankCapRequest request
        ) =>
            new VerifyRankCapTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.VerifyRankCapResult> VerifyRankCapAsync(
                Request.VerifyRankCapRequest request
        ) =>
            new VerifyRankCapTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.VerifyRankCapResult>();
    #else
		public VerifyRankCapTask VerifyRankCapAsync(
                Request.VerifyRankCapRequest request
        )
		{
			return new VerifyRankCapTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.VerifyRankCapResult> VerifyRankCapAsync(
                Request.VerifyRankCapRequest request
        ) =>
            new VerifyRankCapTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class VerifyRankCapByUserIdTask : Gs2RestSessionTask<VerifyRankCapByUserIdRequest, VerifyRankCapByUserIdResult>
        {
            public VerifyRankCapByUserIdTask(IGs2Session session, RestSessionRequestFactory factory, VerifyRankCapByUserIdRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(VerifyRankCapByUserIdRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "experience")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/{userId}/status/{experienceName}/verify/rankCap/{verifyType}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{userId}", !string.IsNullOrEmpty(request.UserId) ? request.UserId.ToString() : "null");
                url = url.Replace("{experienceName}", !string.IsNullOrEmpty(request.ExperienceName) ? request.ExperienceName.ToString() : "null");
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
                if (request.RankCapValue != null)
                {
                    jsonWriter.WritePropertyName("rankCapValue");
                    jsonWriter.Write(request.RankCapValue.ToString());
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
		public IEnumerator VerifyRankCapByUserId(
                Request.VerifyRankCapByUserIdRequest request,
                UnityAction<AsyncResult<Result.VerifyRankCapByUserIdResult>> callback
        ) =>
            new VerifyRankCapByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.VerifyRankCapByUserIdResult> VerifyRankCapByUserIdFuture(
                Request.VerifyRankCapByUserIdRequest request
        ) =>
            new VerifyRankCapByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.VerifyRankCapByUserIdResult> VerifyRankCapByUserIdAsync(
                Request.VerifyRankCapByUserIdRequest request
        ) =>
            new VerifyRankCapByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.VerifyRankCapByUserIdResult>();
    #else
		public VerifyRankCapByUserIdTask VerifyRankCapByUserIdAsync(
                Request.VerifyRankCapByUserIdRequest request
        )
		{
			return new VerifyRankCapByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.VerifyRankCapByUserIdResult> VerifyRankCapByUserIdAsync(
                Request.VerifyRankCapByUserIdRequest request
        ) =>
            new VerifyRankCapByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class AddExperienceByStampSheetTask : Gs2RestSessionTask<AddExperienceByStampSheetRequest, AddExperienceByStampSheetResult>
        {
            public AddExperienceByStampSheetTask(IGs2Session session, RestSessionRequestFactory factory, AddExperienceByStampSheetRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(AddExperienceByStampSheetRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "experience")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/stamp/experience/add";

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
		public IEnumerator AddExperienceByStampSheet(
                Request.AddExperienceByStampSheetRequest request,
                UnityAction<AsyncResult<Result.AddExperienceByStampSheetResult>> callback
        ) =>
            new AddExperienceByStampSheetTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.AddExperienceByStampSheetResult> AddExperienceByStampSheetFuture(
                Request.AddExperienceByStampSheetRequest request
        ) =>
            new AddExperienceByStampSheetTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.AddExperienceByStampSheetResult> AddExperienceByStampSheetAsync(
                Request.AddExperienceByStampSheetRequest request
        ) =>
            new AddExperienceByStampSheetTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.AddExperienceByStampSheetResult>();
    #else
		public AddExperienceByStampSheetTask AddExperienceByStampSheetAsync(
                Request.AddExperienceByStampSheetRequest request
        )
		{
			return new AddExperienceByStampSheetTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.AddExperienceByStampSheetResult> AddExperienceByStampSheetAsync(
                Request.AddExperienceByStampSheetRequest request
        ) =>
            new AddExperienceByStampSheetTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class SetExperienceByStampSheetTask : Gs2RestSessionTask<SetExperienceByStampSheetRequest, SetExperienceByStampSheetResult>
        {
            public SetExperienceByStampSheetTask(IGs2Session session, RestSessionRequestFactory factory, SetExperienceByStampSheetRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(SetExperienceByStampSheetRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "experience")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/stamp/experience/set";

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
		public IEnumerator SetExperienceByStampSheet(
                Request.SetExperienceByStampSheetRequest request,
                UnityAction<AsyncResult<Result.SetExperienceByStampSheetResult>> callback
        ) =>
            new SetExperienceByStampSheetTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.SetExperienceByStampSheetResult> SetExperienceByStampSheetFuture(
                Request.SetExperienceByStampSheetRequest request
        ) =>
            new SetExperienceByStampSheetTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.SetExperienceByStampSheetResult> SetExperienceByStampSheetAsync(
                Request.SetExperienceByStampSheetRequest request
        ) =>
            new SetExperienceByStampSheetTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.SetExperienceByStampSheetResult>();
    #else
		public SetExperienceByStampSheetTask SetExperienceByStampSheetAsync(
                Request.SetExperienceByStampSheetRequest request
        )
		{
			return new SetExperienceByStampSheetTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.SetExperienceByStampSheetResult> SetExperienceByStampSheetAsync(
                Request.SetExperienceByStampSheetRequest request
        ) =>
            new SetExperienceByStampSheetTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class SubExperienceByStampTaskTask : Gs2RestSessionTask<SubExperienceByStampTaskRequest, SubExperienceByStampTaskResult>
        {
            public SubExperienceByStampTaskTask(IGs2Session session, RestSessionRequestFactory factory, SubExperienceByStampTaskRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(SubExperienceByStampTaskRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "experience")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/stamp/experience/sub";

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
		public IEnumerator SubExperienceByStampTask(
                Request.SubExperienceByStampTaskRequest request,
                UnityAction<AsyncResult<Result.SubExperienceByStampTaskResult>> callback
        ) =>
            new SubExperienceByStampTaskTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.SubExperienceByStampTaskResult> SubExperienceByStampTaskFuture(
                Request.SubExperienceByStampTaskRequest request
        ) =>
            new SubExperienceByStampTaskTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.SubExperienceByStampTaskResult> SubExperienceByStampTaskAsync(
                Request.SubExperienceByStampTaskRequest request
        ) =>
            new SubExperienceByStampTaskTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.SubExperienceByStampTaskResult>();
    #else
		public SubExperienceByStampTaskTask SubExperienceByStampTaskAsync(
                Request.SubExperienceByStampTaskRequest request
        )
		{
			return new SubExperienceByStampTaskTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.SubExperienceByStampTaskResult> SubExperienceByStampTaskAsync(
                Request.SubExperienceByStampTaskRequest request
        ) =>
            new SubExperienceByStampTaskTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class AddRankCapByStampSheetTask : Gs2RestSessionTask<AddRankCapByStampSheetRequest, AddRankCapByStampSheetResult>
        {
            public AddRankCapByStampSheetTask(IGs2Session session, RestSessionRequestFactory factory, AddRankCapByStampSheetRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(AddRankCapByStampSheetRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "experience")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/stamp/rankCap/add";

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
		public IEnumerator AddRankCapByStampSheet(
                Request.AddRankCapByStampSheetRequest request,
                UnityAction<AsyncResult<Result.AddRankCapByStampSheetResult>> callback
        ) =>
            new AddRankCapByStampSheetTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.AddRankCapByStampSheetResult> AddRankCapByStampSheetFuture(
                Request.AddRankCapByStampSheetRequest request
        ) =>
            new AddRankCapByStampSheetTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.AddRankCapByStampSheetResult> AddRankCapByStampSheetAsync(
                Request.AddRankCapByStampSheetRequest request
        ) =>
            new AddRankCapByStampSheetTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.AddRankCapByStampSheetResult>();
    #else
		public AddRankCapByStampSheetTask AddRankCapByStampSheetAsync(
                Request.AddRankCapByStampSheetRequest request
        )
		{
			return new AddRankCapByStampSheetTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.AddRankCapByStampSheetResult> AddRankCapByStampSheetAsync(
                Request.AddRankCapByStampSheetRequest request
        ) =>
            new AddRankCapByStampSheetTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class SubRankCapByStampTaskTask : Gs2RestSessionTask<SubRankCapByStampTaskRequest, SubRankCapByStampTaskResult>
        {
            public SubRankCapByStampTaskTask(IGs2Session session, RestSessionRequestFactory factory, SubRankCapByStampTaskRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(SubRankCapByStampTaskRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "experience")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/stamp/rankCap/sub";

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
		public IEnumerator SubRankCapByStampTask(
                Request.SubRankCapByStampTaskRequest request,
                UnityAction<AsyncResult<Result.SubRankCapByStampTaskResult>> callback
        ) =>
            new SubRankCapByStampTaskTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.SubRankCapByStampTaskResult> SubRankCapByStampTaskFuture(
                Request.SubRankCapByStampTaskRequest request
        ) =>
            new SubRankCapByStampTaskTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.SubRankCapByStampTaskResult> SubRankCapByStampTaskAsync(
                Request.SubRankCapByStampTaskRequest request
        ) =>
            new SubRankCapByStampTaskTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.SubRankCapByStampTaskResult>();
    #else
		public SubRankCapByStampTaskTask SubRankCapByStampTaskAsync(
                Request.SubRankCapByStampTaskRequest request
        )
		{
			return new SubRankCapByStampTaskTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.SubRankCapByStampTaskResult> SubRankCapByStampTaskAsync(
                Request.SubRankCapByStampTaskRequest request
        ) =>
            new SubRankCapByStampTaskTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class SetRankCapByStampSheetTask : Gs2RestSessionTask<SetRankCapByStampSheetRequest, SetRankCapByStampSheetResult>
        {
            public SetRankCapByStampSheetTask(IGs2Session session, RestSessionRequestFactory factory, SetRankCapByStampSheetRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(SetRankCapByStampSheetRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "experience")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/stamp/rankCap/set";

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
		public IEnumerator SetRankCapByStampSheet(
                Request.SetRankCapByStampSheetRequest request,
                UnityAction<AsyncResult<Result.SetRankCapByStampSheetResult>> callback
        ) =>
            new SetRankCapByStampSheetTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.SetRankCapByStampSheetResult> SetRankCapByStampSheetFuture(
                Request.SetRankCapByStampSheetRequest request
        ) =>
            new SetRankCapByStampSheetTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.SetRankCapByStampSheetResult> SetRankCapByStampSheetAsync(
                Request.SetRankCapByStampSheetRequest request
        ) =>
            new SetRankCapByStampSheetTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.SetRankCapByStampSheetResult>();
    #else
		public SetRankCapByStampSheetTask SetRankCapByStampSheetAsync(
                Request.SetRankCapByStampSheetRequest request
        )
		{
			return new SetRankCapByStampSheetTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.SetRankCapByStampSheetResult> SetRankCapByStampSheetAsync(
                Request.SetRankCapByStampSheetRequest request
        ) =>
            new SetRankCapByStampSheetTask(
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
                    .Replace("{service}", "experience")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/{userId}/status/model/{experienceName}/property/{propertyId}/acquire/rate/{rateName}/multiply";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{userId}", !string.IsNullOrEmpty(request.UserId) ? request.UserId.ToString() : "null");
                url = url.Replace("{experienceName}", !string.IsNullOrEmpty(request.ExperienceName) ? request.ExperienceName.ToString() : "null");
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
                if (request.BaseRate != null)
                {
                    jsonWriter.WritePropertyName("baseRate");
                    jsonWriter.Write(request.BaseRate.ToString());
                }
                if (request.ContextStack != null)
                {
                    jsonWriter.WritePropertyName("contextStack");
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
                    .Replace("{service}", "experience")
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


        public class VerifyRankByStampTaskTask : Gs2RestSessionTask<VerifyRankByStampTaskRequest, VerifyRankByStampTaskResult>
        {
            public VerifyRankByStampTaskTask(IGs2Session session, RestSessionRequestFactory factory, VerifyRankByStampTaskRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(VerifyRankByStampTaskRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "experience")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/stamp/rank/verify";

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
		public IEnumerator VerifyRankByStampTask(
                Request.VerifyRankByStampTaskRequest request,
                UnityAction<AsyncResult<Result.VerifyRankByStampTaskResult>> callback
        ) =>
            new VerifyRankByStampTaskTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.VerifyRankByStampTaskResult> VerifyRankByStampTaskFuture(
                Request.VerifyRankByStampTaskRequest request
        ) =>
            new VerifyRankByStampTaskTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.VerifyRankByStampTaskResult> VerifyRankByStampTaskAsync(
                Request.VerifyRankByStampTaskRequest request
        ) =>
            new VerifyRankByStampTaskTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.VerifyRankByStampTaskResult>();
    #else
		public VerifyRankByStampTaskTask VerifyRankByStampTaskAsync(
                Request.VerifyRankByStampTaskRequest request
        )
		{
			return new VerifyRankByStampTaskTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.VerifyRankByStampTaskResult> VerifyRankByStampTaskAsync(
                Request.VerifyRankByStampTaskRequest request
        ) =>
            new VerifyRankByStampTaskTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class VerifyRankCapByStampTaskTask : Gs2RestSessionTask<VerifyRankCapByStampTaskRequest, VerifyRankCapByStampTaskResult>
        {
            public VerifyRankCapByStampTaskTask(IGs2Session session, RestSessionRequestFactory factory, VerifyRankCapByStampTaskRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(VerifyRankCapByStampTaskRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "experience")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/stamp/rankCap/verify";

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
		public IEnumerator VerifyRankCapByStampTask(
                Request.VerifyRankCapByStampTaskRequest request,
                UnityAction<AsyncResult<Result.VerifyRankCapByStampTaskResult>> callback
        ) =>
            new VerifyRankCapByStampTaskTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.VerifyRankCapByStampTaskResult> VerifyRankCapByStampTaskFuture(
                Request.VerifyRankCapByStampTaskRequest request
        ) =>
            new VerifyRankCapByStampTaskTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.VerifyRankCapByStampTaskResult> VerifyRankCapByStampTaskAsync(
                Request.VerifyRankCapByStampTaskRequest request
        ) =>
            new VerifyRankCapByStampTaskTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.VerifyRankCapByStampTaskResult>();
    #else
		public VerifyRankCapByStampTaskTask VerifyRankCapByStampTaskAsync(
                Request.VerifyRankCapByStampTaskRequest request
        )
		{
			return new VerifyRankCapByStampTaskTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.VerifyRankCapByStampTaskResult> VerifyRankCapByStampTaskAsync(
                Request.VerifyRankCapByStampTaskRequest request
        ) =>
            new VerifyRankCapByStampTaskTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif
	}
}