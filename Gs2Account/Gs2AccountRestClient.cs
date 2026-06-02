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
using Gs2.Gs2Account.Request;
using Gs2.Gs2Account.Result;
using Gs2.Util.LitJson;

namespace Gs2.Gs2Account
{
	public class Gs2AccountRestClient : AbstractGs2Client
	{
#if UNITY_2017_1_OR_NEWER
		private readonly CertificateHandler _certificateHandler;
#endif

		public static string Endpoint = "account";

        protected Gs2RestSession Gs2RestSession => (Gs2RestSession) Gs2Session;

		public Gs2AccountRestClient(Gs2RestSession Gs2RestSession) : base(Gs2RestSession)
		{

		}

#if UNITY_2017_1_OR_NEWER
		public Gs2AccountRestClient(Gs2RestSession gs2RestSession, CertificateHandler certificateHandler) : base(gs2RestSession)
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
                    .Replace("{service}", "account")
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
                    .Replace("{service}", "account")
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
                if (request.ChangePasswordIfTakeOver != null)
                {
                    jsonWriter.WritePropertyName("changePasswordIfTakeOver");
                    jsonWriter.Write(request.ChangePasswordIfTakeOver.ToString());
                }
                if (request.DifferentUserIdForLoginAndDataRetention != null)
                {
                    jsonWriter.WritePropertyName("differentUserIdForLoginAndDataRetention");
                    jsonWriter.Write(request.DifferentUserIdForLoginAndDataRetention.ToString());
                }
                if (request.CreateAccountScript != null)
                {
                    jsonWriter.WritePropertyName("createAccountScript");
                    request.CreateAccountScript.WriteJson(jsonWriter);
                }
                if (request.AuthenticationScript != null)
                {
                    jsonWriter.WritePropertyName("authenticationScript");
                    request.AuthenticationScript.WriteJson(jsonWriter);
                }
                if (request.CreateTakeOverScript != null)
                {
                    jsonWriter.WritePropertyName("createTakeOverScript");
                    request.CreateTakeOverScript.WriteJson(jsonWriter);
                }
                if (request.DoTakeOverScript != null)
                {
                    jsonWriter.WritePropertyName("doTakeOverScript");
                    request.DoTakeOverScript.WriteJson(jsonWriter);
                }
                if (request.BanScript != null)
                {
                    jsonWriter.WritePropertyName("banScript");
                    request.BanScript.WriteJson(jsonWriter);
                }
                if (request.UnBanScript != null)
                {
                    jsonWriter.WritePropertyName("unBanScript");
                    request.UnBanScript.WriteJson(jsonWriter);
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
                    .Replace("{service}", "account")
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
                    .Replace("{service}", "account")
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
                    .Replace("{service}", "account")
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
                if (request.ChangePasswordIfTakeOver != null)
                {
                    jsonWriter.WritePropertyName("changePasswordIfTakeOver");
                    jsonWriter.Write(request.ChangePasswordIfTakeOver.ToString());
                }
                if (request.CreateAccountScript != null)
                {
                    jsonWriter.WritePropertyName("createAccountScript");
                    request.CreateAccountScript.WriteJson(jsonWriter);
                }
                if (request.AuthenticationScript != null)
                {
                    jsonWriter.WritePropertyName("authenticationScript");
                    request.AuthenticationScript.WriteJson(jsonWriter);
                }
                if (request.CreateTakeOverScript != null)
                {
                    jsonWriter.WritePropertyName("createTakeOverScript");
                    request.CreateTakeOverScript.WriteJson(jsonWriter);
                }
                if (request.DoTakeOverScript != null)
                {
                    jsonWriter.WritePropertyName("doTakeOverScript");
                    request.DoTakeOverScript.WriteJson(jsonWriter);
                }
                if (request.BanScript != null)
                {
                    jsonWriter.WritePropertyName("banScript");
                    request.BanScript.WriteJson(jsonWriter);
                }
                if (request.UnBanScript != null)
                {
                    jsonWriter.WritePropertyName("unBanScript");
                    request.UnBanScript.WriteJson(jsonWriter);
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
                    .Replace("{service}", "account")
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
                    .Replace("{service}", "account")
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
                    .Replace("{service}", "account")
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
                    .Replace("{service}", "account")
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
                    .Replace("{service}", "account")
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
                    .Replace("{service}", "account")
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
                    .Replace("{service}", "account")
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
                    .Replace("{service}", "account")
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
                    .Replace("{service}", "account")
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


        public class DescribeAccountsTask : Gs2RestSessionTask<DescribeAccountsRequest, DescribeAccountsResult>
        {
            public DescribeAccountsTask(IGs2Session session, RestSessionRequestFactory factory, DescribeAccountsRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DescribeAccountsRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "account")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/account";

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
		public IEnumerator DescribeAccounts(
                Request.DescribeAccountsRequest request,
                UnityAction<AsyncResult<Result.DescribeAccountsResult>> callback
        ) =>
            new DescribeAccountsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.DescribeAccountsResult> DescribeAccountsFuture(
                Request.DescribeAccountsRequest request
        ) =>
            new DescribeAccountsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.DescribeAccountsResult> DescribeAccountsAsync(
                Request.DescribeAccountsRequest request
        ) =>
            new DescribeAccountsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.DescribeAccountsResult>();
    #else
		public DescribeAccountsTask DescribeAccountsAsync(
                Request.DescribeAccountsRequest request
        )
		{
			return new DescribeAccountsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.DescribeAccountsResult> DescribeAccountsAsync(
                Request.DescribeAccountsRequest request
        ) =>
            new DescribeAccountsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class CreateAccountTask : Gs2RestSessionTask<CreateAccountRequest, CreateAccountResult>
        {
            public CreateAccountTask(IGs2Session session, RestSessionRequestFactory factory, CreateAccountRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(CreateAccountRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "account")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/account";

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
		public IEnumerator CreateAccount(
                Request.CreateAccountRequest request,
                UnityAction<AsyncResult<Result.CreateAccountResult>> callback
        ) =>
            new CreateAccountTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.CreateAccountResult> CreateAccountFuture(
                Request.CreateAccountRequest request
        ) =>
            new CreateAccountTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.CreateAccountResult> CreateAccountAsync(
                Request.CreateAccountRequest request
        ) =>
            new CreateAccountTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.CreateAccountResult>();
    #else
		public CreateAccountTask CreateAccountAsync(
                Request.CreateAccountRequest request
        )
		{
			return new CreateAccountTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.CreateAccountResult> CreateAccountAsync(
                Request.CreateAccountRequest request
        ) =>
            new CreateAccountTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class UpdateTimeOffsetTask : Gs2RestSessionTask<UpdateTimeOffsetRequest, UpdateTimeOffsetResult>
        {
            public UpdateTimeOffsetTask(IGs2Session session, RestSessionRequestFactory factory, UpdateTimeOffsetRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(UpdateTimeOffsetRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "account")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/account/{userId}/time_offset";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{userId}", !string.IsNullOrEmpty(request.UserId) ? request.UserId.ToString() : "null");

                var sessionRequest = Factory.Put(url);

                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);
                jsonWriter.WriteObjectStart();
                if (request.TimeOffset != null)
                {
                    jsonWriter.WritePropertyName("timeOffset");
                    jsonWriter.Write(request.TimeOffset.ToString());
                }
                if (request.ContextStack != null)
                {
                    jsonWriter.WritePropertyName("contextStack");
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
		public IEnumerator UpdateTimeOffset(
                Request.UpdateTimeOffsetRequest request,
                UnityAction<AsyncResult<Result.UpdateTimeOffsetResult>> callback
        ) =>
            new UpdateTimeOffsetTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.UpdateTimeOffsetResult> UpdateTimeOffsetFuture(
                Request.UpdateTimeOffsetRequest request
        ) =>
            new UpdateTimeOffsetTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.UpdateTimeOffsetResult> UpdateTimeOffsetAsync(
                Request.UpdateTimeOffsetRequest request
        ) =>
            new UpdateTimeOffsetTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.UpdateTimeOffsetResult>();
    #else
		public UpdateTimeOffsetTask UpdateTimeOffsetAsync(
                Request.UpdateTimeOffsetRequest request
        )
		{
			return new UpdateTimeOffsetTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.UpdateTimeOffsetResult> UpdateTimeOffsetAsync(
                Request.UpdateTimeOffsetRequest request
        ) =>
            new UpdateTimeOffsetTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class UpdateBannedTask : Gs2RestSessionTask<UpdateBannedRequest, UpdateBannedResult>
        {
            public UpdateBannedTask(IGs2Session session, RestSessionRequestFactory factory, UpdateBannedRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(UpdateBannedRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "account")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/account/{userId}/banned";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{userId}", !string.IsNullOrEmpty(request.UserId) ? request.UserId.ToString() : "null");

                var sessionRequest = Factory.Put(url);

                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);
                jsonWriter.WriteObjectStart();
                if (request.Banned != null)
                {
                    jsonWriter.WritePropertyName("banned");
                    jsonWriter.Write(request.Banned.ToString());
                }
                if (request.ContextStack != null)
                {
                    jsonWriter.WritePropertyName("contextStack");
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
		public IEnumerator UpdateBanned(
                Request.UpdateBannedRequest request,
                UnityAction<AsyncResult<Result.UpdateBannedResult>> callback
        ) =>
            new UpdateBannedTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.UpdateBannedResult> UpdateBannedFuture(
                Request.UpdateBannedRequest request
        ) =>
            new UpdateBannedTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.UpdateBannedResult> UpdateBannedAsync(
                Request.UpdateBannedRequest request
        ) =>
            new UpdateBannedTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.UpdateBannedResult>();
    #else
		public UpdateBannedTask UpdateBannedAsync(
                Request.UpdateBannedRequest request
        )
		{
			return new UpdateBannedTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.UpdateBannedResult> UpdateBannedAsync(
                Request.UpdateBannedRequest request
        ) =>
            new UpdateBannedTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class AddBanTask : Gs2RestSessionTask<AddBanRequest, AddBanResult>
        {
            public AddBanTask(IGs2Session session, RestSessionRequestFactory factory, AddBanRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(AddBanRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "account")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/account/{userId}/ban";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{userId}", !string.IsNullOrEmpty(request.UserId) ? request.UserId.ToString() : "null");

                var sessionRequest = Factory.Post(url);

                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);
                jsonWriter.WriteObjectStart();
                if (request.BanStatus != null)
                {
                    jsonWriter.WritePropertyName("banStatus");
                    request.BanStatus.WriteJson(jsonWriter);
                }
                if (request.ContextStack != null)
                {
                    jsonWriter.WritePropertyName("contextStack");
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
		public IEnumerator AddBan(
                Request.AddBanRequest request,
                UnityAction<AsyncResult<Result.AddBanResult>> callback
        ) =>
            new AddBanTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.AddBanResult> AddBanFuture(
                Request.AddBanRequest request
        ) =>
            new AddBanTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.AddBanResult> AddBanAsync(
                Request.AddBanRequest request
        ) =>
            new AddBanTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.AddBanResult>();
    #else
		public AddBanTask AddBanAsync(
                Request.AddBanRequest request
        )
		{
			return new AddBanTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.AddBanResult> AddBanAsync(
                Request.AddBanRequest request
        ) =>
            new AddBanTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class RemoveBanTask : Gs2RestSessionTask<RemoveBanRequest, RemoveBanResult>
        {
            public RemoveBanTask(IGs2Session session, RestSessionRequestFactory factory, RemoveBanRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(RemoveBanRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "account")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/account/{userId}/ban/{banStatusName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{userId}", !string.IsNullOrEmpty(request.UserId) ? request.UserId.ToString() : "null");
                url = url.Replace("{banStatusName}", !string.IsNullOrEmpty(request.BanStatusName) ? request.BanStatusName.ToString() : "null");

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
		public IEnumerator RemoveBan(
                Request.RemoveBanRequest request,
                UnityAction<AsyncResult<Result.RemoveBanResult>> callback
        ) =>
            new RemoveBanTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.RemoveBanResult> RemoveBanFuture(
                Request.RemoveBanRequest request
        ) =>
            new RemoveBanTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.RemoveBanResult> RemoveBanAsync(
                Request.RemoveBanRequest request
        ) =>
            new RemoveBanTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.RemoveBanResult>();
    #else
		public RemoveBanTask RemoveBanAsync(
                Request.RemoveBanRequest request
        )
		{
			return new RemoveBanTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.RemoveBanResult> RemoveBanAsync(
                Request.RemoveBanRequest request
        ) =>
            new RemoveBanTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class GetAccountTask : Gs2RestSessionTask<GetAccountRequest, GetAccountResult>
        {
            public GetAccountTask(IGs2Session session, RestSessionRequestFactory factory, GetAccountRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(GetAccountRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "account")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/account/{userId}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{userId}", !string.IsNullOrEmpty(request.UserId) ? request.UserId.ToString() : "null");

                var sessionRequest = Factory.Get(url);
                if (request.ContextStack != null)
                {
                    sessionRequest.AddQueryString("contextStack", request.ContextStack);
                }
                if (request.IncludeLastAuthenticatedAt != null) {
                    sessionRequest.AddQueryString("includeLastAuthenticatedAt", $"{request.IncludeLastAuthenticatedAt}");
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
		public IEnumerator GetAccount(
                Request.GetAccountRequest request,
                UnityAction<AsyncResult<Result.GetAccountResult>> callback
        ) =>
            new GetAccountTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.GetAccountResult> GetAccountFuture(
                Request.GetAccountRequest request
        ) =>
            new GetAccountTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.GetAccountResult> GetAccountAsync(
                Request.GetAccountRequest request
        ) =>
            new GetAccountTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.GetAccountResult>();
    #else
		public GetAccountTask GetAccountAsync(
                Request.GetAccountRequest request
        )
		{
			return new GetAccountTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.GetAccountResult> GetAccountAsync(
                Request.GetAccountRequest request
        ) =>
            new GetAccountTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class DeleteAccountTask : Gs2RestSessionTask<DeleteAccountRequest, DeleteAccountResult>
        {
            public DeleteAccountTask(IGs2Session session, RestSessionRequestFactory factory, DeleteAccountRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DeleteAccountRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "account")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/account/{userId}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
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
		public IEnumerator DeleteAccount(
                Request.DeleteAccountRequest request,
                UnityAction<AsyncResult<Result.DeleteAccountResult>> callback
        ) =>
            new DeleteAccountTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.DeleteAccountResult> DeleteAccountFuture(
                Request.DeleteAccountRequest request
        ) =>
            new DeleteAccountTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.DeleteAccountResult> DeleteAccountAsync(
                Request.DeleteAccountRequest request
        ) =>
            new DeleteAccountTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.DeleteAccountResult>();
    #else
		public DeleteAccountTask DeleteAccountAsync(
                Request.DeleteAccountRequest request
        )
		{
			return new DeleteAccountTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.DeleteAccountResult> DeleteAccountAsync(
                Request.DeleteAccountRequest request
        ) =>
            new DeleteAccountTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class AuthenticationTask : Gs2RestSessionTask<AuthenticationRequest, AuthenticationResult>
        {
            public AuthenticationTask(IGs2Session session, RestSessionRequestFactory factory, AuthenticationRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(AuthenticationRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "account")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/account/{userId}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{userId}", !string.IsNullOrEmpty(request.UserId) ? request.UserId.ToString() : "null");

                var sessionRequest = Factory.Put(url);

                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);
                jsonWriter.WriteObjectStart();
                if (request.KeyId != null)
                {
                    jsonWriter.WritePropertyName("keyId");
                    jsonWriter.Write(request.KeyId);
                }
                if (request.Password != null)
                {
                    jsonWriter.WritePropertyName("password");
                    jsonWriter.Write(request.Password);
                }
                if (request.ContextStack != null)
                {
                    jsonWriter.WritePropertyName("contextStack");
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
                if (error.Errors.Count(v => v.code == "account.password.invalid") > 0) {
                    base.OnError(new Exception.PasswordIncorrectException(error));
                }
                else if (error.Errors.Count(v => v.code == "account.banned.infinity") > 0) {
                    base.OnError(new Exception.BannedInfinityException(error));
                }
                else {
                    base.OnError(error);
                }
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator Authentication(
                Request.AuthenticationRequest request,
                UnityAction<AsyncResult<Result.AuthenticationResult>> callback
        ) =>
            new AuthenticationTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.AuthenticationResult> AuthenticationFuture(
                Request.AuthenticationRequest request
        ) =>
            new AuthenticationTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.AuthenticationResult> AuthenticationAsync(
                Request.AuthenticationRequest request
        ) =>
            new AuthenticationTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.AuthenticationResult>();
    #else
		public AuthenticationTask AuthenticationAsync(
                Request.AuthenticationRequest request
        )
		{
			return new AuthenticationTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.AuthenticationResult> AuthenticationAsync(
                Request.AuthenticationRequest request
        ) =>
            new AuthenticationTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class DescribeTakeOversTask : Gs2RestSessionTask<DescribeTakeOversRequest, DescribeTakeOversResult>
        {
            public DescribeTakeOversTask(IGs2Session session, RestSessionRequestFactory factory, DescribeTakeOversRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DescribeTakeOversRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "account")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/account/me/takeover";

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
		public IEnumerator DescribeTakeOvers(
                Request.DescribeTakeOversRequest request,
                UnityAction<AsyncResult<Result.DescribeTakeOversResult>> callback
        ) =>
            new DescribeTakeOversTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.DescribeTakeOversResult> DescribeTakeOversFuture(
                Request.DescribeTakeOversRequest request
        ) =>
            new DescribeTakeOversTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.DescribeTakeOversResult> DescribeTakeOversAsync(
                Request.DescribeTakeOversRequest request
        ) =>
            new DescribeTakeOversTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.DescribeTakeOversResult>();
    #else
		public DescribeTakeOversTask DescribeTakeOversAsync(
                Request.DescribeTakeOversRequest request
        )
		{
			return new DescribeTakeOversTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.DescribeTakeOversResult> DescribeTakeOversAsync(
                Request.DescribeTakeOversRequest request
        ) =>
            new DescribeTakeOversTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class DescribeTakeOversByUserIdTask : Gs2RestSessionTask<DescribeTakeOversByUserIdRequest, DescribeTakeOversByUserIdResult>
        {
            public DescribeTakeOversByUserIdTask(IGs2Session session, RestSessionRequestFactory factory, DescribeTakeOversByUserIdRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DescribeTakeOversByUserIdRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "account")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/account/{userId}/takeover";

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
		public IEnumerator DescribeTakeOversByUserId(
                Request.DescribeTakeOversByUserIdRequest request,
                UnityAction<AsyncResult<Result.DescribeTakeOversByUserIdResult>> callback
        ) =>
            new DescribeTakeOversByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.DescribeTakeOversByUserIdResult> DescribeTakeOversByUserIdFuture(
                Request.DescribeTakeOversByUserIdRequest request
        ) =>
            new DescribeTakeOversByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.DescribeTakeOversByUserIdResult> DescribeTakeOversByUserIdAsync(
                Request.DescribeTakeOversByUserIdRequest request
        ) =>
            new DescribeTakeOversByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.DescribeTakeOversByUserIdResult>();
    #else
		public DescribeTakeOversByUserIdTask DescribeTakeOversByUserIdAsync(
                Request.DescribeTakeOversByUserIdRequest request
        )
		{
			return new DescribeTakeOversByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.DescribeTakeOversByUserIdResult> DescribeTakeOversByUserIdAsync(
                Request.DescribeTakeOversByUserIdRequest request
        ) =>
            new DescribeTakeOversByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class CreateTakeOverTask : Gs2RestSessionTask<CreateTakeOverRequest, CreateTakeOverResult>
        {
            public CreateTakeOverTask(IGs2Session session, RestSessionRequestFactory factory, CreateTakeOverRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(CreateTakeOverRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "account")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/account/me/takeover";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");

                var sessionRequest = Factory.Post(url);

                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);
                jsonWriter.WriteObjectStart();
                if (request.Type != null)
                {
                    jsonWriter.WritePropertyName("type");
                    jsonWriter.Write(request.Type.ToString());
                }
                if (request.UserIdentifier != null)
                {
                    jsonWriter.WritePropertyName("userIdentifier");
                    jsonWriter.Write(request.UserIdentifier);
                }
                if (request.Password != null)
                {
                    jsonWriter.WritePropertyName("password");
                    jsonWriter.Write(request.Password);
                }
                if (request.ContextStack != null)
                {
                    jsonWriter.WritePropertyName("contextStack");
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
		public IEnumerator CreateTakeOver(
                Request.CreateTakeOverRequest request,
                UnityAction<AsyncResult<Result.CreateTakeOverResult>> callback
        ) =>
            new CreateTakeOverTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.CreateTakeOverResult> CreateTakeOverFuture(
                Request.CreateTakeOverRequest request
        ) =>
            new CreateTakeOverTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.CreateTakeOverResult> CreateTakeOverAsync(
                Request.CreateTakeOverRequest request
        ) =>
            new CreateTakeOverTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.CreateTakeOverResult>();
    #else
		public CreateTakeOverTask CreateTakeOverAsync(
                Request.CreateTakeOverRequest request
        )
		{
			return new CreateTakeOverTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.CreateTakeOverResult> CreateTakeOverAsync(
                Request.CreateTakeOverRequest request
        ) =>
            new CreateTakeOverTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class CreateTakeOverByUserIdTask : Gs2RestSessionTask<CreateTakeOverByUserIdRequest, CreateTakeOverByUserIdResult>
        {
            public CreateTakeOverByUserIdTask(IGs2Session session, RestSessionRequestFactory factory, CreateTakeOverByUserIdRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(CreateTakeOverByUserIdRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "account")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/account/{userId}/takeover";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{userId}", !string.IsNullOrEmpty(request.UserId) ? request.UserId.ToString() : "null");

                var sessionRequest = Factory.Post(url);

                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);
                jsonWriter.WriteObjectStart();
                if (request.Type != null)
                {
                    jsonWriter.WritePropertyName("type");
                    jsonWriter.Write(request.Type.ToString());
                }
                if (request.UserIdentifier != null)
                {
                    jsonWriter.WritePropertyName("userIdentifier");
                    jsonWriter.Write(request.UserIdentifier);
                }
                if (request.Password != null)
                {
                    jsonWriter.WritePropertyName("password");
                    jsonWriter.Write(request.Password);
                }
                if (request.ContextStack != null)
                {
                    jsonWriter.WritePropertyName("contextStack");
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
		public IEnumerator CreateTakeOverByUserId(
                Request.CreateTakeOverByUserIdRequest request,
                UnityAction<AsyncResult<Result.CreateTakeOverByUserIdResult>> callback
        ) =>
            new CreateTakeOverByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.CreateTakeOverByUserIdResult> CreateTakeOverByUserIdFuture(
                Request.CreateTakeOverByUserIdRequest request
        ) =>
            new CreateTakeOverByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.CreateTakeOverByUserIdResult> CreateTakeOverByUserIdAsync(
                Request.CreateTakeOverByUserIdRequest request
        ) =>
            new CreateTakeOverByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.CreateTakeOverByUserIdResult>();
    #else
		public CreateTakeOverByUserIdTask CreateTakeOverByUserIdAsync(
                Request.CreateTakeOverByUserIdRequest request
        )
		{
			return new CreateTakeOverByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.CreateTakeOverByUserIdResult> CreateTakeOverByUserIdAsync(
                Request.CreateTakeOverByUserIdRequest request
        ) =>
            new CreateTakeOverByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class CreateTakeOverOpenIdConnectTask : Gs2RestSessionTask<CreateTakeOverOpenIdConnectRequest, CreateTakeOverOpenIdConnectResult>
        {
            public CreateTakeOverOpenIdConnectTask(IGs2Session session, RestSessionRequestFactory factory, CreateTakeOverOpenIdConnectRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(CreateTakeOverOpenIdConnectRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "account")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/account/me/takeover/openIdConnect";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");

                var sessionRequest = Factory.Post(url);

                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);
                jsonWriter.WriteObjectStart();
                if (request.Type != null)
                {
                    jsonWriter.WritePropertyName("type");
                    jsonWriter.Write(request.Type.ToString());
                }
                if (request.IdToken != null)
                {
                    jsonWriter.WritePropertyName("idToken");
                    jsonWriter.Write(request.IdToken);
                }
                if (request.ContextStack != null)
                {
                    jsonWriter.WritePropertyName("contextStack");
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
		public IEnumerator CreateTakeOverOpenIdConnect(
                Request.CreateTakeOverOpenIdConnectRequest request,
                UnityAction<AsyncResult<Result.CreateTakeOverOpenIdConnectResult>> callback
        ) =>
            new CreateTakeOverOpenIdConnectTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.CreateTakeOverOpenIdConnectResult> CreateTakeOverOpenIdConnectFuture(
                Request.CreateTakeOverOpenIdConnectRequest request
        ) =>
            new CreateTakeOverOpenIdConnectTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.CreateTakeOverOpenIdConnectResult> CreateTakeOverOpenIdConnectAsync(
                Request.CreateTakeOverOpenIdConnectRequest request
        ) =>
            new CreateTakeOverOpenIdConnectTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.CreateTakeOverOpenIdConnectResult>();
    #else
		public CreateTakeOverOpenIdConnectTask CreateTakeOverOpenIdConnectAsync(
                Request.CreateTakeOverOpenIdConnectRequest request
        )
		{
			return new CreateTakeOverOpenIdConnectTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.CreateTakeOverOpenIdConnectResult> CreateTakeOverOpenIdConnectAsync(
                Request.CreateTakeOverOpenIdConnectRequest request
        ) =>
            new CreateTakeOverOpenIdConnectTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class CreateTakeOverOpenIdConnectAndByUserIdTask : Gs2RestSessionTask<CreateTakeOverOpenIdConnectAndByUserIdRequest, CreateTakeOverOpenIdConnectAndByUserIdResult>
        {
            public CreateTakeOverOpenIdConnectAndByUserIdTask(IGs2Session session, RestSessionRequestFactory factory, CreateTakeOverOpenIdConnectAndByUserIdRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(CreateTakeOverOpenIdConnectAndByUserIdRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "account")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/account/{userId}/takeover/openIdConnect";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{userId}", !string.IsNullOrEmpty(request.UserId) ? request.UserId.ToString() : "null");

                var sessionRequest = Factory.Post(url);

                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);
                jsonWriter.WriteObjectStart();
                if (request.Type != null)
                {
                    jsonWriter.WritePropertyName("type");
                    jsonWriter.Write(request.Type.ToString());
                }
                if (request.IdToken != null)
                {
                    jsonWriter.WritePropertyName("idToken");
                    jsonWriter.Write(request.IdToken);
                }
                if (request.ContextStack != null)
                {
                    jsonWriter.WritePropertyName("contextStack");
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
		public IEnumerator CreateTakeOverOpenIdConnectAndByUserId(
                Request.CreateTakeOverOpenIdConnectAndByUserIdRequest request,
                UnityAction<AsyncResult<Result.CreateTakeOverOpenIdConnectAndByUserIdResult>> callback
        ) =>
            new CreateTakeOverOpenIdConnectAndByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.CreateTakeOverOpenIdConnectAndByUserIdResult> CreateTakeOverOpenIdConnectAndByUserIdFuture(
                Request.CreateTakeOverOpenIdConnectAndByUserIdRequest request
        ) =>
            new CreateTakeOverOpenIdConnectAndByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.CreateTakeOverOpenIdConnectAndByUserIdResult> CreateTakeOverOpenIdConnectAndByUserIdAsync(
                Request.CreateTakeOverOpenIdConnectAndByUserIdRequest request
        ) =>
            new CreateTakeOverOpenIdConnectAndByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.CreateTakeOverOpenIdConnectAndByUserIdResult>();
    #else
		public CreateTakeOverOpenIdConnectAndByUserIdTask CreateTakeOverOpenIdConnectAndByUserIdAsync(
                Request.CreateTakeOverOpenIdConnectAndByUserIdRequest request
        )
		{
			return new CreateTakeOverOpenIdConnectAndByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.CreateTakeOverOpenIdConnectAndByUserIdResult> CreateTakeOverOpenIdConnectAndByUserIdAsync(
                Request.CreateTakeOverOpenIdConnectAndByUserIdRequest request
        ) =>
            new CreateTakeOverOpenIdConnectAndByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class GetTakeOverTask : Gs2RestSessionTask<GetTakeOverRequest, GetTakeOverResult>
        {
            public GetTakeOverTask(IGs2Session session, RestSessionRequestFactory factory, GetTakeOverRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(GetTakeOverRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "account")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/account/me/takeover/type/{type}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{type}",request.Type != null ? request.Type.ToString() : "null");

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
		public IEnumerator GetTakeOver(
                Request.GetTakeOverRequest request,
                UnityAction<AsyncResult<Result.GetTakeOverResult>> callback
        ) =>
            new GetTakeOverTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.GetTakeOverResult> GetTakeOverFuture(
                Request.GetTakeOverRequest request
        ) =>
            new GetTakeOverTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.GetTakeOverResult> GetTakeOverAsync(
                Request.GetTakeOverRequest request
        ) =>
            new GetTakeOverTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.GetTakeOverResult>();
    #else
		public GetTakeOverTask GetTakeOverAsync(
                Request.GetTakeOverRequest request
        )
		{
			return new GetTakeOverTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.GetTakeOverResult> GetTakeOverAsync(
                Request.GetTakeOverRequest request
        ) =>
            new GetTakeOverTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class GetTakeOverByUserIdTask : Gs2RestSessionTask<GetTakeOverByUserIdRequest, GetTakeOverByUserIdResult>
        {
            public GetTakeOverByUserIdTask(IGs2Session session, RestSessionRequestFactory factory, GetTakeOverByUserIdRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(GetTakeOverByUserIdRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "account")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/account/{userId}/takeover/type/{type}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{userId}", !string.IsNullOrEmpty(request.UserId) ? request.UserId.ToString() : "null");
                url = url.Replace("{type}",request.Type != null ? request.Type.ToString() : "null");

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
		public IEnumerator GetTakeOverByUserId(
                Request.GetTakeOverByUserIdRequest request,
                UnityAction<AsyncResult<Result.GetTakeOverByUserIdResult>> callback
        ) =>
            new GetTakeOverByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.GetTakeOverByUserIdResult> GetTakeOverByUserIdFuture(
                Request.GetTakeOverByUserIdRequest request
        ) =>
            new GetTakeOverByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.GetTakeOverByUserIdResult> GetTakeOverByUserIdAsync(
                Request.GetTakeOverByUserIdRequest request
        ) =>
            new GetTakeOverByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.GetTakeOverByUserIdResult>();
    #else
		public GetTakeOverByUserIdTask GetTakeOverByUserIdAsync(
                Request.GetTakeOverByUserIdRequest request
        )
		{
			return new GetTakeOverByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.GetTakeOverByUserIdResult> GetTakeOverByUserIdAsync(
                Request.GetTakeOverByUserIdRequest request
        ) =>
            new GetTakeOverByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class UpdateTakeOverTask : Gs2RestSessionTask<UpdateTakeOverRequest, UpdateTakeOverResult>
        {
            public UpdateTakeOverTask(IGs2Session session, RestSessionRequestFactory factory, UpdateTakeOverRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(UpdateTakeOverRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "account")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/account/me/takeover/type/{type}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{type}",request.Type != null ? request.Type.ToString() : "null");

                var sessionRequest = Factory.Put(url);

                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);
                jsonWriter.WriteObjectStart();
                if (request.OldPassword != null)
                {
                    jsonWriter.WritePropertyName("oldPassword");
                    jsonWriter.Write(request.OldPassword);
                }
                if (request.Password != null)
                {
                    jsonWriter.WritePropertyName("password");
                    jsonWriter.Write(request.Password);
                }
                if (request.ContextStack != null)
                {
                    jsonWriter.WritePropertyName("contextStack");
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
                if (error.Errors.Count(v => v.code == "account.password.invalid") > 0) {
                    base.OnError(new Exception.PasswordIncorrectException(error));
                }
                else {
                    base.OnError(error);
                }
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator UpdateTakeOver(
                Request.UpdateTakeOverRequest request,
                UnityAction<AsyncResult<Result.UpdateTakeOverResult>> callback
        ) =>
            new UpdateTakeOverTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.UpdateTakeOverResult> UpdateTakeOverFuture(
                Request.UpdateTakeOverRequest request
        ) =>
            new UpdateTakeOverTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.UpdateTakeOverResult> UpdateTakeOverAsync(
                Request.UpdateTakeOverRequest request
        ) =>
            new UpdateTakeOverTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.UpdateTakeOverResult>();
    #else
		public UpdateTakeOverTask UpdateTakeOverAsync(
                Request.UpdateTakeOverRequest request
        )
		{
			return new UpdateTakeOverTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.UpdateTakeOverResult> UpdateTakeOverAsync(
                Request.UpdateTakeOverRequest request
        ) =>
            new UpdateTakeOverTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class UpdateTakeOverByUserIdTask : Gs2RestSessionTask<UpdateTakeOverByUserIdRequest, UpdateTakeOverByUserIdResult>
        {
            public UpdateTakeOverByUserIdTask(IGs2Session session, RestSessionRequestFactory factory, UpdateTakeOverByUserIdRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(UpdateTakeOverByUserIdRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "account")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/account/{userId}/takeover/type/{type}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{userId}", !string.IsNullOrEmpty(request.UserId) ? request.UserId.ToString() : "null");
                url = url.Replace("{type}",request.Type != null ? request.Type.ToString() : "null");

                var sessionRequest = Factory.Put(url);

                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);
                jsonWriter.WriteObjectStart();
                if (request.OldPassword != null)
                {
                    jsonWriter.WritePropertyName("oldPassword");
                    jsonWriter.Write(request.OldPassword);
                }
                if (request.Password != null)
                {
                    jsonWriter.WritePropertyName("password");
                    jsonWriter.Write(request.Password);
                }
                if (request.ContextStack != null)
                {
                    jsonWriter.WritePropertyName("contextStack");
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
                if (error.Errors.Count(v => v.code == "account.password.invalid") > 0) {
                    base.OnError(new Exception.PasswordIncorrectException(error));
                }
                else {
                    base.OnError(error);
                }
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator UpdateTakeOverByUserId(
                Request.UpdateTakeOverByUserIdRequest request,
                UnityAction<AsyncResult<Result.UpdateTakeOverByUserIdResult>> callback
        ) =>
            new UpdateTakeOverByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.UpdateTakeOverByUserIdResult> UpdateTakeOverByUserIdFuture(
                Request.UpdateTakeOverByUserIdRequest request
        ) =>
            new UpdateTakeOverByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.UpdateTakeOverByUserIdResult> UpdateTakeOverByUserIdAsync(
                Request.UpdateTakeOverByUserIdRequest request
        ) =>
            new UpdateTakeOverByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.UpdateTakeOverByUserIdResult>();
    #else
		public UpdateTakeOverByUserIdTask UpdateTakeOverByUserIdAsync(
                Request.UpdateTakeOverByUserIdRequest request
        )
		{
			return new UpdateTakeOverByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.UpdateTakeOverByUserIdResult> UpdateTakeOverByUserIdAsync(
                Request.UpdateTakeOverByUserIdRequest request
        ) =>
            new UpdateTakeOverByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class DeleteTakeOverTask : Gs2RestSessionTask<DeleteTakeOverRequest, DeleteTakeOverResult>
        {
            public DeleteTakeOverTask(IGs2Session session, RestSessionRequestFactory factory, DeleteTakeOverRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DeleteTakeOverRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "account")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/account/me/takeover/type/{type}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{type}",request.Type != null ? request.Type.ToString() : "null");

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
		public IEnumerator DeleteTakeOver(
                Request.DeleteTakeOverRequest request,
                UnityAction<AsyncResult<Result.DeleteTakeOverResult>> callback
        ) =>
            new DeleteTakeOverTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.DeleteTakeOverResult> DeleteTakeOverFuture(
                Request.DeleteTakeOverRequest request
        ) =>
            new DeleteTakeOverTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.DeleteTakeOverResult> DeleteTakeOverAsync(
                Request.DeleteTakeOverRequest request
        ) =>
            new DeleteTakeOverTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.DeleteTakeOverResult>();
    #else
		public DeleteTakeOverTask DeleteTakeOverAsync(
                Request.DeleteTakeOverRequest request
        )
		{
			return new DeleteTakeOverTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.DeleteTakeOverResult> DeleteTakeOverAsync(
                Request.DeleteTakeOverRequest request
        ) =>
            new DeleteTakeOverTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class DeleteTakeOverByUserIdentifierTask : Gs2RestSessionTask<DeleteTakeOverByUserIdentifierRequest, DeleteTakeOverByUserIdentifierResult>
        {
            public DeleteTakeOverByUserIdentifierTask(IGs2Session session, RestSessionRequestFactory factory, DeleteTakeOverByUserIdentifierRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DeleteTakeOverByUserIdentifierRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "account")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/takeover/type/{type}/userIdentifier/{userIdentifier}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{type}",request.Type != null ? request.Type.ToString() : "null");
                url = url.Replace("{userIdentifier}", !string.IsNullOrEmpty(request.UserIdentifier) ? request.UserIdentifier.ToString() : "null");

                var sessionRequest = Factory.Delete(url);
                if (request.ContextStack != null)
                {
                    sessionRequest.AddQueryString("contextStack", request.ContextStack);
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
		public IEnumerator DeleteTakeOverByUserIdentifier(
                Request.DeleteTakeOverByUserIdentifierRequest request,
                UnityAction<AsyncResult<Result.DeleteTakeOverByUserIdentifierResult>> callback
        ) =>
            new DeleteTakeOverByUserIdentifierTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.DeleteTakeOverByUserIdentifierResult> DeleteTakeOverByUserIdentifierFuture(
                Request.DeleteTakeOverByUserIdentifierRequest request
        ) =>
            new DeleteTakeOverByUserIdentifierTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.DeleteTakeOverByUserIdentifierResult> DeleteTakeOverByUserIdentifierAsync(
                Request.DeleteTakeOverByUserIdentifierRequest request
        ) =>
            new DeleteTakeOverByUserIdentifierTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.DeleteTakeOverByUserIdentifierResult>();
    #else
		public DeleteTakeOverByUserIdentifierTask DeleteTakeOverByUserIdentifierAsync(
                Request.DeleteTakeOverByUserIdentifierRequest request
        )
		{
			return new DeleteTakeOverByUserIdentifierTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.DeleteTakeOverByUserIdentifierResult> DeleteTakeOverByUserIdentifierAsync(
                Request.DeleteTakeOverByUserIdentifierRequest request
        ) =>
            new DeleteTakeOverByUserIdentifierTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class DeleteTakeOverByUserIdTask : Gs2RestSessionTask<DeleteTakeOverByUserIdRequest, DeleteTakeOverByUserIdResult>
        {
            public DeleteTakeOverByUserIdTask(IGs2Session session, RestSessionRequestFactory factory, DeleteTakeOverByUserIdRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DeleteTakeOverByUserIdRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "account")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/account/{userId}/takeover/type/{type}/takeover";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{userId}", !string.IsNullOrEmpty(request.UserId) ? request.UserId.ToString() : "null");
                url = url.Replace("{type}",request.Type != null ? request.Type.ToString() : "null");

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
		public IEnumerator DeleteTakeOverByUserId(
                Request.DeleteTakeOverByUserIdRequest request,
                UnityAction<AsyncResult<Result.DeleteTakeOverByUserIdResult>> callback
        ) =>
            new DeleteTakeOverByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.DeleteTakeOverByUserIdResult> DeleteTakeOverByUserIdFuture(
                Request.DeleteTakeOverByUserIdRequest request
        ) =>
            new DeleteTakeOverByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.DeleteTakeOverByUserIdResult> DeleteTakeOverByUserIdAsync(
                Request.DeleteTakeOverByUserIdRequest request
        ) =>
            new DeleteTakeOverByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.DeleteTakeOverByUserIdResult>();
    #else
		public DeleteTakeOverByUserIdTask DeleteTakeOverByUserIdAsync(
                Request.DeleteTakeOverByUserIdRequest request
        )
		{
			return new DeleteTakeOverByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.DeleteTakeOverByUserIdResult> DeleteTakeOverByUserIdAsync(
                Request.DeleteTakeOverByUserIdRequest request
        ) =>
            new DeleteTakeOverByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class DoTakeOverTask : Gs2RestSessionTask<DoTakeOverRequest, DoTakeOverResult>
        {
            public DoTakeOverTask(IGs2Session session, RestSessionRequestFactory factory, DoTakeOverRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DoTakeOverRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "account")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/takeover/type/{type}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{type}",request.Type != null ? request.Type.ToString() : "null");

                var sessionRequest = Factory.Post(url);

                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);
                jsonWriter.WriteObjectStart();
                if (request.UserIdentifier != null)
                {
                    jsonWriter.WritePropertyName("userIdentifier");
                    jsonWriter.Write(request.UserIdentifier);
                }
                if (request.Password != null)
                {
                    jsonWriter.WritePropertyName("password");
                    jsonWriter.Write(request.Password);
                }
                if (request.ContextStack != null)
                {
                    jsonWriter.WritePropertyName("contextStack");
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

            public override void OnError(Gs2.Core.Exception.Gs2Exception error)
            {
                if (error.Errors.Count(v => v.code == "account.password.invalid") > 0) {
                    base.OnError(new Exception.PasswordIncorrectException(error));
                }
                else {
                    base.OnError(error);
                }
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator DoTakeOver(
                Request.DoTakeOverRequest request,
                UnityAction<AsyncResult<Result.DoTakeOverResult>> callback
        ) =>
            new DoTakeOverTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.DoTakeOverResult> DoTakeOverFuture(
                Request.DoTakeOverRequest request
        ) =>
            new DoTakeOverTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.DoTakeOverResult> DoTakeOverAsync(
                Request.DoTakeOverRequest request
        ) =>
            new DoTakeOverTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.DoTakeOverResult>();
    #else
		public DoTakeOverTask DoTakeOverAsync(
                Request.DoTakeOverRequest request
        )
		{
			return new DoTakeOverTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.DoTakeOverResult> DoTakeOverAsync(
                Request.DoTakeOverRequest request
        ) =>
            new DoTakeOverTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class DoTakeOverOpenIdConnectTask : Gs2RestSessionTask<DoTakeOverOpenIdConnectRequest, DoTakeOverOpenIdConnectResult>
        {
            public DoTakeOverOpenIdConnectTask(IGs2Session session, RestSessionRequestFactory factory, DoTakeOverOpenIdConnectRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DoTakeOverOpenIdConnectRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "account")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/takeover/type/{type}/openIdConnect";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{type}",request.Type != null ? request.Type.ToString() : "null");

                var sessionRequest = Factory.Post(url);

                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);
                jsonWriter.WriteObjectStart();
                if (request.IdToken != null)
                {
                    jsonWriter.WritePropertyName("idToken");
                    jsonWriter.Write(request.IdToken);
                }
                if (request.ContextStack != null)
                {
                    jsonWriter.WritePropertyName("contextStack");
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
		public IEnumerator DoTakeOverOpenIdConnect(
                Request.DoTakeOverOpenIdConnectRequest request,
                UnityAction<AsyncResult<Result.DoTakeOverOpenIdConnectResult>> callback
        ) =>
            new DoTakeOverOpenIdConnectTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.DoTakeOverOpenIdConnectResult> DoTakeOverOpenIdConnectFuture(
                Request.DoTakeOverOpenIdConnectRequest request
        ) =>
            new DoTakeOverOpenIdConnectTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.DoTakeOverOpenIdConnectResult> DoTakeOverOpenIdConnectAsync(
                Request.DoTakeOverOpenIdConnectRequest request
        ) =>
            new DoTakeOverOpenIdConnectTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.DoTakeOverOpenIdConnectResult>();
    #else
		public DoTakeOverOpenIdConnectTask DoTakeOverOpenIdConnectAsync(
                Request.DoTakeOverOpenIdConnectRequest request
        )
		{
			return new DoTakeOverOpenIdConnectTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.DoTakeOverOpenIdConnectResult> DoTakeOverOpenIdConnectAsync(
                Request.DoTakeOverOpenIdConnectRequest request
        ) =>
            new DoTakeOverOpenIdConnectTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class GetAuthorizationUrlTask : Gs2RestSessionTask<GetAuthorizationUrlRequest, GetAuthorizationUrlResult>
        {
            public GetAuthorizationUrlTask(IGs2Session session, RestSessionRequestFactory factory, GetAuthorizationUrlRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(GetAuthorizationUrlRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "account")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/type/{type}/authorization/url";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{type}",request.Type != null ? request.Type.ToString() : "null");

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
		public IEnumerator GetAuthorizationUrl(
                Request.GetAuthorizationUrlRequest request,
                UnityAction<AsyncResult<Result.GetAuthorizationUrlResult>> callback
        ) =>
            new GetAuthorizationUrlTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.GetAuthorizationUrlResult> GetAuthorizationUrlFuture(
                Request.GetAuthorizationUrlRequest request
        ) =>
            new GetAuthorizationUrlTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.GetAuthorizationUrlResult> GetAuthorizationUrlAsync(
                Request.GetAuthorizationUrlRequest request
        ) =>
            new GetAuthorizationUrlTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.GetAuthorizationUrlResult>();
    #else
		public GetAuthorizationUrlTask GetAuthorizationUrlAsync(
                Request.GetAuthorizationUrlRequest request
        )
		{
			return new GetAuthorizationUrlTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.GetAuthorizationUrlResult> GetAuthorizationUrlAsync(
                Request.GetAuthorizationUrlRequest request
        ) =>
            new GetAuthorizationUrlTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class DescribePlatformIdsTask : Gs2RestSessionTask<DescribePlatformIdsRequest, DescribePlatformIdsResult>
        {
            public DescribePlatformIdsTask(IGs2Session session, RestSessionRequestFactory factory, DescribePlatformIdsRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DescribePlatformIdsRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "account")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/account/me/platformId";

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
		public IEnumerator DescribePlatformIds(
                Request.DescribePlatformIdsRequest request,
                UnityAction<AsyncResult<Result.DescribePlatformIdsResult>> callback
        ) =>
            new DescribePlatformIdsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.DescribePlatformIdsResult> DescribePlatformIdsFuture(
                Request.DescribePlatformIdsRequest request
        ) =>
            new DescribePlatformIdsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.DescribePlatformIdsResult> DescribePlatformIdsAsync(
                Request.DescribePlatformIdsRequest request
        ) =>
            new DescribePlatformIdsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.DescribePlatformIdsResult>();
    #else
		public DescribePlatformIdsTask DescribePlatformIdsAsync(
                Request.DescribePlatformIdsRequest request
        )
		{
			return new DescribePlatformIdsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.DescribePlatformIdsResult> DescribePlatformIdsAsync(
                Request.DescribePlatformIdsRequest request
        ) =>
            new DescribePlatformIdsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class DescribePlatformIdsByUserIdTask : Gs2RestSessionTask<DescribePlatformIdsByUserIdRequest, DescribePlatformIdsByUserIdResult>
        {
            public DescribePlatformIdsByUserIdTask(IGs2Session session, RestSessionRequestFactory factory, DescribePlatformIdsByUserIdRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DescribePlatformIdsByUserIdRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "account")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/account/{userId}/platformId";

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
		public IEnumerator DescribePlatformIdsByUserId(
                Request.DescribePlatformIdsByUserIdRequest request,
                UnityAction<AsyncResult<Result.DescribePlatformIdsByUserIdResult>> callback
        ) =>
            new DescribePlatformIdsByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.DescribePlatformIdsByUserIdResult> DescribePlatformIdsByUserIdFuture(
                Request.DescribePlatformIdsByUserIdRequest request
        ) =>
            new DescribePlatformIdsByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.DescribePlatformIdsByUserIdResult> DescribePlatformIdsByUserIdAsync(
                Request.DescribePlatformIdsByUserIdRequest request
        ) =>
            new DescribePlatformIdsByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.DescribePlatformIdsByUserIdResult>();
    #else
		public DescribePlatformIdsByUserIdTask DescribePlatformIdsByUserIdAsync(
                Request.DescribePlatformIdsByUserIdRequest request
        )
		{
			return new DescribePlatformIdsByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.DescribePlatformIdsByUserIdResult> DescribePlatformIdsByUserIdAsync(
                Request.DescribePlatformIdsByUserIdRequest request
        ) =>
            new DescribePlatformIdsByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class CreatePlatformIdTask : Gs2RestSessionTask<CreatePlatformIdRequest, CreatePlatformIdResult>
        {
            public CreatePlatformIdTask(IGs2Session session, RestSessionRequestFactory factory, CreatePlatformIdRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(CreatePlatformIdRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "account")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/account/me/platformId";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");

                var sessionRequest = Factory.Post(url);

                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);
                jsonWriter.WriteObjectStart();
                if (request.Type != null)
                {
                    jsonWriter.WritePropertyName("type");
                    jsonWriter.Write(request.Type.ToString());
                }
                if (request.UserIdentifier != null)
                {
                    jsonWriter.WritePropertyName("userIdentifier");
                    jsonWriter.Write(request.UserIdentifier);
                }
                if (request.ContextStack != null)
                {
                    jsonWriter.WritePropertyName("contextStack");
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
		public IEnumerator CreatePlatformId(
                Request.CreatePlatformIdRequest request,
                UnityAction<AsyncResult<Result.CreatePlatformIdResult>> callback
        ) =>
            new CreatePlatformIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.CreatePlatformIdResult> CreatePlatformIdFuture(
                Request.CreatePlatformIdRequest request
        ) =>
            new CreatePlatformIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.CreatePlatformIdResult> CreatePlatformIdAsync(
                Request.CreatePlatformIdRequest request
        ) =>
            new CreatePlatformIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.CreatePlatformIdResult>();
    #else
		public CreatePlatformIdTask CreatePlatformIdAsync(
                Request.CreatePlatformIdRequest request
        )
		{
			return new CreatePlatformIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.CreatePlatformIdResult> CreatePlatformIdAsync(
                Request.CreatePlatformIdRequest request
        ) =>
            new CreatePlatformIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class CreatePlatformIdByUserIdTask : Gs2RestSessionTask<CreatePlatformIdByUserIdRequest, CreatePlatformIdByUserIdResult>
        {
            public CreatePlatformIdByUserIdTask(IGs2Session session, RestSessionRequestFactory factory, CreatePlatformIdByUserIdRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(CreatePlatformIdByUserIdRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "account")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/account/{userId}/platformId";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{userId}", !string.IsNullOrEmpty(request.UserId) ? request.UserId.ToString() : "null");

                var sessionRequest = Factory.Post(url);

                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);
                jsonWriter.WriteObjectStart();
                if (request.Type != null)
                {
                    jsonWriter.WritePropertyName("type");
                    jsonWriter.Write(request.Type.ToString());
                }
                if (request.UserIdentifier != null)
                {
                    jsonWriter.WritePropertyName("userIdentifier");
                    jsonWriter.Write(request.UserIdentifier);
                }
                if (request.ContextStack != null)
                {
                    jsonWriter.WritePropertyName("contextStack");
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
		public IEnumerator CreatePlatformIdByUserId(
                Request.CreatePlatformIdByUserIdRequest request,
                UnityAction<AsyncResult<Result.CreatePlatformIdByUserIdResult>> callback
        ) =>
            new CreatePlatformIdByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.CreatePlatformIdByUserIdResult> CreatePlatformIdByUserIdFuture(
                Request.CreatePlatformIdByUserIdRequest request
        ) =>
            new CreatePlatformIdByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.CreatePlatformIdByUserIdResult> CreatePlatformIdByUserIdAsync(
                Request.CreatePlatformIdByUserIdRequest request
        ) =>
            new CreatePlatformIdByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.CreatePlatformIdByUserIdResult>();
    #else
		public CreatePlatformIdByUserIdTask CreatePlatformIdByUserIdAsync(
                Request.CreatePlatformIdByUserIdRequest request
        )
		{
			return new CreatePlatformIdByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.CreatePlatformIdByUserIdResult> CreatePlatformIdByUserIdAsync(
                Request.CreatePlatformIdByUserIdRequest request
        ) =>
            new CreatePlatformIdByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class GetPlatformIdTask : Gs2RestSessionTask<GetPlatformIdRequest, GetPlatformIdResult>
        {
            public GetPlatformIdTask(IGs2Session session, RestSessionRequestFactory factory, GetPlatformIdRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(GetPlatformIdRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "account")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/account/me/platformId/type/{type}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{type}",request.Type != null ? request.Type.ToString() : "null");

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
		public IEnumerator GetPlatformId(
                Request.GetPlatformIdRequest request,
                UnityAction<AsyncResult<Result.GetPlatformIdResult>> callback
        ) =>
            new GetPlatformIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.GetPlatformIdResult> GetPlatformIdFuture(
                Request.GetPlatformIdRequest request
        ) =>
            new GetPlatformIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.GetPlatformIdResult> GetPlatformIdAsync(
                Request.GetPlatformIdRequest request
        ) =>
            new GetPlatformIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.GetPlatformIdResult>();
    #else
		public GetPlatformIdTask GetPlatformIdAsync(
                Request.GetPlatformIdRequest request
        )
		{
			return new GetPlatformIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.GetPlatformIdResult> GetPlatformIdAsync(
                Request.GetPlatformIdRequest request
        ) =>
            new GetPlatformIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class GetPlatformIdByUserIdTask : Gs2RestSessionTask<GetPlatformIdByUserIdRequest, GetPlatformIdByUserIdResult>
        {
            public GetPlatformIdByUserIdTask(IGs2Session session, RestSessionRequestFactory factory, GetPlatformIdByUserIdRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(GetPlatformIdByUserIdRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "account")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/account/{userId}/platformId/type/{type}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{userId}", !string.IsNullOrEmpty(request.UserId) ? request.UserId.ToString() : "null");
                url = url.Replace("{type}",request.Type != null ? request.Type.ToString() : "null");

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
		public IEnumerator GetPlatformIdByUserId(
                Request.GetPlatformIdByUserIdRequest request,
                UnityAction<AsyncResult<Result.GetPlatformIdByUserIdResult>> callback
        ) =>
            new GetPlatformIdByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.GetPlatformIdByUserIdResult> GetPlatformIdByUserIdFuture(
                Request.GetPlatformIdByUserIdRequest request
        ) =>
            new GetPlatformIdByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.GetPlatformIdByUserIdResult> GetPlatformIdByUserIdAsync(
                Request.GetPlatformIdByUserIdRequest request
        ) =>
            new GetPlatformIdByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.GetPlatformIdByUserIdResult>();
    #else
		public GetPlatformIdByUserIdTask GetPlatformIdByUserIdAsync(
                Request.GetPlatformIdByUserIdRequest request
        )
		{
			return new GetPlatformIdByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.GetPlatformIdByUserIdResult> GetPlatformIdByUserIdAsync(
                Request.GetPlatformIdByUserIdRequest request
        ) =>
            new GetPlatformIdByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class FindPlatformIdTask : Gs2RestSessionTask<FindPlatformIdRequest, FindPlatformIdResult>
        {
            public FindPlatformIdTask(IGs2Session session, RestSessionRequestFactory factory, FindPlatformIdRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(FindPlatformIdRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "account")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/account/me/platformId/type/{type}/userIdentifier/{userIdentifier}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{type}",request.Type != null ? request.Type.ToString() : "null");
                url = url.Replace("{userIdentifier}", !string.IsNullOrEmpty(request.UserIdentifier) ? request.UserIdentifier.ToString() : "null");

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
		public IEnumerator FindPlatformId(
                Request.FindPlatformIdRequest request,
                UnityAction<AsyncResult<Result.FindPlatformIdResult>> callback
        ) =>
            new FindPlatformIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.FindPlatformIdResult> FindPlatformIdFuture(
                Request.FindPlatformIdRequest request
        ) =>
            new FindPlatformIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.FindPlatformIdResult> FindPlatformIdAsync(
                Request.FindPlatformIdRequest request
        ) =>
            new FindPlatformIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.FindPlatformIdResult>();
    #else
		public FindPlatformIdTask FindPlatformIdAsync(
                Request.FindPlatformIdRequest request
        )
		{
			return new FindPlatformIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.FindPlatformIdResult> FindPlatformIdAsync(
                Request.FindPlatformIdRequest request
        ) =>
            new FindPlatformIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class FindPlatformIdByUserIdTask : Gs2RestSessionTask<FindPlatformIdByUserIdRequest, FindPlatformIdByUserIdResult>
        {
            public FindPlatformIdByUserIdTask(IGs2Session session, RestSessionRequestFactory factory, FindPlatformIdByUserIdRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(FindPlatformIdByUserIdRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "account")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/account/{userId}/platformId/type/{type}/userIdentifier/{userIdentifier}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{userId}", !string.IsNullOrEmpty(request.UserId) ? request.UserId.ToString() : "null");
                url = url.Replace("{type}",request.Type != null ? request.Type.ToString() : "null");
                url = url.Replace("{userIdentifier}", !string.IsNullOrEmpty(request.UserIdentifier) ? request.UserIdentifier.ToString() : "null");

                var sessionRequest = Factory.Get(url);
                if (request.ContextStack != null)
                {
                    sessionRequest.AddQueryString("contextStack", request.ContextStack);
                }
                if (request.DontResolveDataOwner != null) {
                    sessionRequest.AddQueryString("dontResolveDataOwner", $"{request.DontResolveDataOwner}");
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
		public IEnumerator FindPlatformIdByUserId(
                Request.FindPlatformIdByUserIdRequest request,
                UnityAction<AsyncResult<Result.FindPlatformIdByUserIdResult>> callback
        ) =>
            new FindPlatformIdByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.FindPlatformIdByUserIdResult> FindPlatformIdByUserIdFuture(
                Request.FindPlatformIdByUserIdRequest request
        ) =>
            new FindPlatformIdByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.FindPlatformIdByUserIdResult> FindPlatformIdByUserIdAsync(
                Request.FindPlatformIdByUserIdRequest request
        ) =>
            new FindPlatformIdByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.FindPlatformIdByUserIdResult>();
    #else
		public FindPlatformIdByUserIdTask FindPlatformIdByUserIdAsync(
                Request.FindPlatformIdByUserIdRequest request
        )
		{
			return new FindPlatformIdByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.FindPlatformIdByUserIdResult> FindPlatformIdByUserIdAsync(
                Request.FindPlatformIdByUserIdRequest request
        ) =>
            new FindPlatformIdByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class DeletePlatformIdTask : Gs2RestSessionTask<DeletePlatformIdRequest, DeletePlatformIdResult>
        {
            public DeletePlatformIdTask(IGs2Session session, RestSessionRequestFactory factory, DeletePlatformIdRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DeletePlatformIdRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "account")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/account/me/platformId/type/{type}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{type}",request.Type != null ? request.Type.ToString() : "null");

                var sessionRequest = Factory.Delete(url);
                if (request.ContextStack != null)
                {
                    sessionRequest.AddQueryString("contextStack", request.ContextStack);
                }
                if (request.UserIdentifier != null) {
                    sessionRequest.AddQueryString("userIdentifier", $"{request.UserIdentifier}");
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
		public IEnumerator DeletePlatformId(
                Request.DeletePlatformIdRequest request,
                UnityAction<AsyncResult<Result.DeletePlatformIdResult>> callback
        ) =>
            new DeletePlatformIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.DeletePlatformIdResult> DeletePlatformIdFuture(
                Request.DeletePlatformIdRequest request
        ) =>
            new DeletePlatformIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.DeletePlatformIdResult> DeletePlatformIdAsync(
                Request.DeletePlatformIdRequest request
        ) =>
            new DeletePlatformIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.DeletePlatformIdResult>();
    #else
		public DeletePlatformIdTask DeletePlatformIdAsync(
                Request.DeletePlatformIdRequest request
        )
		{
			return new DeletePlatformIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.DeletePlatformIdResult> DeletePlatformIdAsync(
                Request.DeletePlatformIdRequest request
        ) =>
            new DeletePlatformIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class DeletePlatformIdByUserIdentifierTask : Gs2RestSessionTask<DeletePlatformIdByUserIdentifierRequest, DeletePlatformIdByUserIdentifierResult>
        {
            public DeletePlatformIdByUserIdentifierTask(IGs2Session session, RestSessionRequestFactory factory, DeletePlatformIdByUserIdentifierRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DeletePlatformIdByUserIdentifierRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "account")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/platformId/type/{type}/userIdentifier/{userIdentifier}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{type}",request.Type != null ? request.Type.ToString() : "null");
                url = url.Replace("{userIdentifier}", !string.IsNullOrEmpty(request.UserIdentifier) ? request.UserIdentifier.ToString() : "null");

                var sessionRequest = Factory.Delete(url);
                if (request.ContextStack != null)
                {
                    sessionRequest.AddQueryString("contextStack", request.ContextStack);
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
		public IEnumerator DeletePlatformIdByUserIdentifier(
                Request.DeletePlatformIdByUserIdentifierRequest request,
                UnityAction<AsyncResult<Result.DeletePlatformIdByUserIdentifierResult>> callback
        ) =>
            new DeletePlatformIdByUserIdentifierTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.DeletePlatformIdByUserIdentifierResult> DeletePlatformIdByUserIdentifierFuture(
                Request.DeletePlatformIdByUserIdentifierRequest request
        ) =>
            new DeletePlatformIdByUserIdentifierTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.DeletePlatformIdByUserIdentifierResult> DeletePlatformIdByUserIdentifierAsync(
                Request.DeletePlatformIdByUserIdentifierRequest request
        ) =>
            new DeletePlatformIdByUserIdentifierTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.DeletePlatformIdByUserIdentifierResult>();
    #else
		public DeletePlatformIdByUserIdentifierTask DeletePlatformIdByUserIdentifierAsync(
                Request.DeletePlatformIdByUserIdentifierRequest request
        )
		{
			return new DeletePlatformIdByUserIdentifierTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.DeletePlatformIdByUserIdentifierResult> DeletePlatformIdByUserIdentifierAsync(
                Request.DeletePlatformIdByUserIdentifierRequest request
        ) =>
            new DeletePlatformIdByUserIdentifierTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class DeletePlatformIdByUserIdTask : Gs2RestSessionTask<DeletePlatformIdByUserIdRequest, DeletePlatformIdByUserIdResult>
        {
            public DeletePlatformIdByUserIdTask(IGs2Session session, RestSessionRequestFactory factory, DeletePlatformIdByUserIdRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DeletePlatformIdByUserIdRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "account")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/account/{userId}/platformId/type/{type}/platformId";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{userId}", !string.IsNullOrEmpty(request.UserId) ? request.UserId.ToString() : "null");
                url = url.Replace("{type}",request.Type != null ? request.Type.ToString() : "null");

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
		public IEnumerator DeletePlatformIdByUserId(
                Request.DeletePlatformIdByUserIdRequest request,
                UnityAction<AsyncResult<Result.DeletePlatformIdByUserIdResult>> callback
        ) =>
            new DeletePlatformIdByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.DeletePlatformIdByUserIdResult> DeletePlatformIdByUserIdFuture(
                Request.DeletePlatformIdByUserIdRequest request
        ) =>
            new DeletePlatformIdByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.DeletePlatformIdByUserIdResult> DeletePlatformIdByUserIdAsync(
                Request.DeletePlatformIdByUserIdRequest request
        ) =>
            new DeletePlatformIdByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.DeletePlatformIdByUserIdResult>();
    #else
		public DeletePlatformIdByUserIdTask DeletePlatformIdByUserIdAsync(
                Request.DeletePlatformIdByUserIdRequest request
        )
		{
			return new DeletePlatformIdByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.DeletePlatformIdByUserIdResult> DeletePlatformIdByUserIdAsync(
                Request.DeletePlatformIdByUserIdRequest request
        ) =>
            new DeletePlatformIdByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class GetDataOwnerByUserIdTask : Gs2RestSessionTask<GetDataOwnerByUserIdRequest, GetDataOwnerByUserIdResult>
        {
            public GetDataOwnerByUserIdTask(IGs2Session session, RestSessionRequestFactory factory, GetDataOwnerByUserIdRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(GetDataOwnerByUserIdRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "account")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/account/{userId}/dataOwner";

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
		public IEnumerator GetDataOwnerByUserId(
                Request.GetDataOwnerByUserIdRequest request,
                UnityAction<AsyncResult<Result.GetDataOwnerByUserIdResult>> callback
        ) =>
            new GetDataOwnerByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.GetDataOwnerByUserIdResult> GetDataOwnerByUserIdFuture(
                Request.GetDataOwnerByUserIdRequest request
        ) =>
            new GetDataOwnerByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.GetDataOwnerByUserIdResult> GetDataOwnerByUserIdAsync(
                Request.GetDataOwnerByUserIdRequest request
        ) =>
            new GetDataOwnerByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.GetDataOwnerByUserIdResult>();
    #else
		public GetDataOwnerByUserIdTask GetDataOwnerByUserIdAsync(
                Request.GetDataOwnerByUserIdRequest request
        )
		{
			return new GetDataOwnerByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.GetDataOwnerByUserIdResult> GetDataOwnerByUserIdAsync(
                Request.GetDataOwnerByUserIdRequest request
        ) =>
            new GetDataOwnerByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class UpdateDataOwnerByUserIdTask : Gs2RestSessionTask<UpdateDataOwnerByUserIdRequest, UpdateDataOwnerByUserIdResult>
        {
            public UpdateDataOwnerByUserIdTask(IGs2Session session, RestSessionRequestFactory factory, UpdateDataOwnerByUserIdRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(UpdateDataOwnerByUserIdRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "account")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/account/{userId}/dataOwner";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{userId}", !string.IsNullOrEmpty(request.UserId) ? request.UserId.ToString() : "null");

                var sessionRequest = Factory.Put(url);

                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);
                jsonWriter.WriteObjectStart();
                if (request.DataOwnerName != null)
                {
                    jsonWriter.WritePropertyName("dataOwnerName");
                    jsonWriter.Write(request.DataOwnerName);
                }
                if (request.ContextStack != null)
                {
                    jsonWriter.WritePropertyName("contextStack");
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
		public IEnumerator UpdateDataOwnerByUserId(
                Request.UpdateDataOwnerByUserIdRequest request,
                UnityAction<AsyncResult<Result.UpdateDataOwnerByUserIdResult>> callback
        ) =>
            new UpdateDataOwnerByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.UpdateDataOwnerByUserIdResult> UpdateDataOwnerByUserIdFuture(
                Request.UpdateDataOwnerByUserIdRequest request
        ) =>
            new UpdateDataOwnerByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.UpdateDataOwnerByUserIdResult> UpdateDataOwnerByUserIdAsync(
                Request.UpdateDataOwnerByUserIdRequest request
        ) =>
            new UpdateDataOwnerByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.UpdateDataOwnerByUserIdResult>();
    #else
		public UpdateDataOwnerByUserIdTask UpdateDataOwnerByUserIdAsync(
                Request.UpdateDataOwnerByUserIdRequest request
        )
		{
			return new UpdateDataOwnerByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.UpdateDataOwnerByUserIdResult> UpdateDataOwnerByUserIdAsync(
                Request.UpdateDataOwnerByUserIdRequest request
        ) =>
            new UpdateDataOwnerByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class DeleteDataOwnerByUserIdTask : Gs2RestSessionTask<DeleteDataOwnerByUserIdRequest, DeleteDataOwnerByUserIdResult>
        {
            public DeleteDataOwnerByUserIdTask(IGs2Session session, RestSessionRequestFactory factory, DeleteDataOwnerByUserIdRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DeleteDataOwnerByUserIdRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "account")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/account/{userId}/dataOwner";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
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
		public IEnumerator DeleteDataOwnerByUserId(
                Request.DeleteDataOwnerByUserIdRequest request,
                UnityAction<AsyncResult<Result.DeleteDataOwnerByUserIdResult>> callback
        ) =>
            new DeleteDataOwnerByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.DeleteDataOwnerByUserIdResult> DeleteDataOwnerByUserIdFuture(
                Request.DeleteDataOwnerByUserIdRequest request
        ) =>
            new DeleteDataOwnerByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.DeleteDataOwnerByUserIdResult> DeleteDataOwnerByUserIdAsync(
                Request.DeleteDataOwnerByUserIdRequest request
        ) =>
            new DeleteDataOwnerByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.DeleteDataOwnerByUserIdResult>();
    #else
		public DeleteDataOwnerByUserIdTask DeleteDataOwnerByUserIdAsync(
                Request.DeleteDataOwnerByUserIdRequest request
        )
		{
			return new DeleteDataOwnerByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.DeleteDataOwnerByUserIdResult> DeleteDataOwnerByUserIdAsync(
                Request.DeleteDataOwnerByUserIdRequest request
        ) =>
            new DeleteDataOwnerByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class DescribeTakeOverTypeModelsTask : Gs2RestSessionTask<DescribeTakeOverTypeModelsRequest, DescribeTakeOverTypeModelsResult>
        {
            public DescribeTakeOverTypeModelsTask(IGs2Session session, RestSessionRequestFactory factory, DescribeTakeOverTypeModelsRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DescribeTakeOverTypeModelsRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "account")
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
		public IEnumerator DescribeTakeOverTypeModels(
                Request.DescribeTakeOverTypeModelsRequest request,
                UnityAction<AsyncResult<Result.DescribeTakeOverTypeModelsResult>> callback
        ) =>
            new DescribeTakeOverTypeModelsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.DescribeTakeOverTypeModelsResult> DescribeTakeOverTypeModelsFuture(
                Request.DescribeTakeOverTypeModelsRequest request
        ) =>
            new DescribeTakeOverTypeModelsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.DescribeTakeOverTypeModelsResult> DescribeTakeOverTypeModelsAsync(
                Request.DescribeTakeOverTypeModelsRequest request
        ) =>
            new DescribeTakeOverTypeModelsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.DescribeTakeOverTypeModelsResult>();
    #else
		public DescribeTakeOverTypeModelsTask DescribeTakeOverTypeModelsAsync(
                Request.DescribeTakeOverTypeModelsRequest request
        )
		{
			return new DescribeTakeOverTypeModelsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.DescribeTakeOverTypeModelsResult> DescribeTakeOverTypeModelsAsync(
                Request.DescribeTakeOverTypeModelsRequest request
        ) =>
            new DescribeTakeOverTypeModelsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class GetTakeOverTypeModelTask : Gs2RestSessionTask<GetTakeOverTypeModelRequest, GetTakeOverTypeModelResult>
        {
            public GetTakeOverTypeModelTask(IGs2Session session, RestSessionRequestFactory factory, GetTakeOverTypeModelRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(GetTakeOverTypeModelRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "account")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/model/{type}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{type}",request.Type != null ? request.Type.ToString() : "null");

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
		public IEnumerator GetTakeOverTypeModel(
                Request.GetTakeOverTypeModelRequest request,
                UnityAction<AsyncResult<Result.GetTakeOverTypeModelResult>> callback
        ) =>
            new GetTakeOverTypeModelTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.GetTakeOverTypeModelResult> GetTakeOverTypeModelFuture(
                Request.GetTakeOverTypeModelRequest request
        ) =>
            new GetTakeOverTypeModelTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.GetTakeOverTypeModelResult> GetTakeOverTypeModelAsync(
                Request.GetTakeOverTypeModelRequest request
        ) =>
            new GetTakeOverTypeModelTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.GetTakeOverTypeModelResult>();
    #else
		public GetTakeOverTypeModelTask GetTakeOverTypeModelAsync(
                Request.GetTakeOverTypeModelRequest request
        )
		{
			return new GetTakeOverTypeModelTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.GetTakeOverTypeModelResult> GetTakeOverTypeModelAsync(
                Request.GetTakeOverTypeModelRequest request
        ) =>
            new GetTakeOverTypeModelTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class DescribeTakeOverTypeModelMastersTask : Gs2RestSessionTask<DescribeTakeOverTypeModelMastersRequest, DescribeTakeOverTypeModelMastersResult>
        {
            public DescribeTakeOverTypeModelMastersTask(IGs2Session session, RestSessionRequestFactory factory, DescribeTakeOverTypeModelMastersRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DescribeTakeOverTypeModelMastersRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "account")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/master/model";

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
		public IEnumerator DescribeTakeOverTypeModelMasters(
                Request.DescribeTakeOverTypeModelMastersRequest request,
                UnityAction<AsyncResult<Result.DescribeTakeOverTypeModelMastersResult>> callback
        ) =>
            new DescribeTakeOverTypeModelMastersTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.DescribeTakeOverTypeModelMastersResult> DescribeTakeOverTypeModelMastersFuture(
                Request.DescribeTakeOverTypeModelMastersRequest request
        ) =>
            new DescribeTakeOverTypeModelMastersTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.DescribeTakeOverTypeModelMastersResult> DescribeTakeOverTypeModelMastersAsync(
                Request.DescribeTakeOverTypeModelMastersRequest request
        ) =>
            new DescribeTakeOverTypeModelMastersTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.DescribeTakeOverTypeModelMastersResult>();
    #else
		public DescribeTakeOverTypeModelMastersTask DescribeTakeOverTypeModelMastersAsync(
                Request.DescribeTakeOverTypeModelMastersRequest request
        )
		{
			return new DescribeTakeOverTypeModelMastersTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.DescribeTakeOverTypeModelMastersResult> DescribeTakeOverTypeModelMastersAsync(
                Request.DescribeTakeOverTypeModelMastersRequest request
        ) =>
            new DescribeTakeOverTypeModelMastersTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class CreateTakeOverTypeModelMasterTask : Gs2RestSessionTask<CreateTakeOverTypeModelMasterRequest, CreateTakeOverTypeModelMasterResult>
        {
            public CreateTakeOverTypeModelMasterTask(IGs2Session session, RestSessionRequestFactory factory, CreateTakeOverTypeModelMasterRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(CreateTakeOverTypeModelMasterRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "account")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/master/model";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");

                var sessionRequest = Factory.Post(url);

                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);
                jsonWriter.WriteObjectStart();
                if (request.Type != null)
                {
                    jsonWriter.WritePropertyName("type");
                    jsonWriter.Write(request.Type.ToString());
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
                if (request.OpenIdConnectSetting != null)
                {
                    jsonWriter.WritePropertyName("openIdConnectSetting");
                    request.OpenIdConnectSetting.WriteJson(jsonWriter);
                }
                if (request.ContextStack != null)
                {
                    jsonWriter.WritePropertyName("contextStack");
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
		public IEnumerator CreateTakeOverTypeModelMaster(
                Request.CreateTakeOverTypeModelMasterRequest request,
                UnityAction<AsyncResult<Result.CreateTakeOverTypeModelMasterResult>> callback
        ) =>
            new CreateTakeOverTypeModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.CreateTakeOverTypeModelMasterResult> CreateTakeOverTypeModelMasterFuture(
                Request.CreateTakeOverTypeModelMasterRequest request
        ) =>
            new CreateTakeOverTypeModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.CreateTakeOverTypeModelMasterResult> CreateTakeOverTypeModelMasterAsync(
                Request.CreateTakeOverTypeModelMasterRequest request
        ) =>
            new CreateTakeOverTypeModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.CreateTakeOverTypeModelMasterResult>();
    #else
		public CreateTakeOverTypeModelMasterTask CreateTakeOverTypeModelMasterAsync(
                Request.CreateTakeOverTypeModelMasterRequest request
        )
		{
			return new CreateTakeOverTypeModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.CreateTakeOverTypeModelMasterResult> CreateTakeOverTypeModelMasterAsync(
                Request.CreateTakeOverTypeModelMasterRequest request
        ) =>
            new CreateTakeOverTypeModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class GetTakeOverTypeModelMasterTask : Gs2RestSessionTask<GetTakeOverTypeModelMasterRequest, GetTakeOverTypeModelMasterResult>
        {
            public GetTakeOverTypeModelMasterTask(IGs2Session session, RestSessionRequestFactory factory, GetTakeOverTypeModelMasterRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(GetTakeOverTypeModelMasterRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "account")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/master/model/{type}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{type}",request.Type != null ? request.Type.ToString() : "null");

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
		public IEnumerator GetTakeOverTypeModelMaster(
                Request.GetTakeOverTypeModelMasterRequest request,
                UnityAction<AsyncResult<Result.GetTakeOverTypeModelMasterResult>> callback
        ) =>
            new GetTakeOverTypeModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.GetTakeOverTypeModelMasterResult> GetTakeOverTypeModelMasterFuture(
                Request.GetTakeOverTypeModelMasterRequest request
        ) =>
            new GetTakeOverTypeModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.GetTakeOverTypeModelMasterResult> GetTakeOverTypeModelMasterAsync(
                Request.GetTakeOverTypeModelMasterRequest request
        ) =>
            new GetTakeOverTypeModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.GetTakeOverTypeModelMasterResult>();
    #else
		public GetTakeOverTypeModelMasterTask GetTakeOverTypeModelMasterAsync(
                Request.GetTakeOverTypeModelMasterRequest request
        )
		{
			return new GetTakeOverTypeModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.GetTakeOverTypeModelMasterResult> GetTakeOverTypeModelMasterAsync(
                Request.GetTakeOverTypeModelMasterRequest request
        ) =>
            new GetTakeOverTypeModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class UpdateTakeOverTypeModelMasterTask : Gs2RestSessionTask<UpdateTakeOverTypeModelMasterRequest, UpdateTakeOverTypeModelMasterResult>
        {
            public UpdateTakeOverTypeModelMasterTask(IGs2Session session, RestSessionRequestFactory factory, UpdateTakeOverTypeModelMasterRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(UpdateTakeOverTypeModelMasterRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "account")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/master/model/{type}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{type}",request.Type != null ? request.Type.ToString() : "null");

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
                if (request.OpenIdConnectSetting != null)
                {
                    jsonWriter.WritePropertyName("openIdConnectSetting");
                    request.OpenIdConnectSetting.WriteJson(jsonWriter);
                }
                if (request.ContextStack != null)
                {
                    jsonWriter.WritePropertyName("contextStack");
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
		public IEnumerator UpdateTakeOverTypeModelMaster(
                Request.UpdateTakeOverTypeModelMasterRequest request,
                UnityAction<AsyncResult<Result.UpdateTakeOverTypeModelMasterResult>> callback
        ) =>
            new UpdateTakeOverTypeModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.UpdateTakeOverTypeModelMasterResult> UpdateTakeOverTypeModelMasterFuture(
                Request.UpdateTakeOverTypeModelMasterRequest request
        ) =>
            new UpdateTakeOverTypeModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.UpdateTakeOverTypeModelMasterResult> UpdateTakeOverTypeModelMasterAsync(
                Request.UpdateTakeOverTypeModelMasterRequest request
        ) =>
            new UpdateTakeOverTypeModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.UpdateTakeOverTypeModelMasterResult>();
    #else
		public UpdateTakeOverTypeModelMasterTask UpdateTakeOverTypeModelMasterAsync(
                Request.UpdateTakeOverTypeModelMasterRequest request
        )
		{
			return new UpdateTakeOverTypeModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.UpdateTakeOverTypeModelMasterResult> UpdateTakeOverTypeModelMasterAsync(
                Request.UpdateTakeOverTypeModelMasterRequest request
        ) =>
            new UpdateTakeOverTypeModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class DeleteTakeOverTypeModelMasterTask : Gs2RestSessionTask<DeleteTakeOverTypeModelMasterRequest, DeleteTakeOverTypeModelMasterResult>
        {
            public DeleteTakeOverTypeModelMasterTask(IGs2Session session, RestSessionRequestFactory factory, DeleteTakeOverTypeModelMasterRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DeleteTakeOverTypeModelMasterRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "account")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/master/model/{type}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{type}",request.Type != null ? request.Type.ToString() : "null");

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
		public IEnumerator DeleteTakeOverTypeModelMaster(
                Request.DeleteTakeOverTypeModelMasterRequest request,
                UnityAction<AsyncResult<Result.DeleteTakeOverTypeModelMasterResult>> callback
        ) =>
            new DeleteTakeOverTypeModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.DeleteTakeOverTypeModelMasterResult> DeleteTakeOverTypeModelMasterFuture(
                Request.DeleteTakeOverTypeModelMasterRequest request
        ) =>
            new DeleteTakeOverTypeModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.DeleteTakeOverTypeModelMasterResult> DeleteTakeOverTypeModelMasterAsync(
                Request.DeleteTakeOverTypeModelMasterRequest request
        ) =>
            new DeleteTakeOverTypeModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.DeleteTakeOverTypeModelMasterResult>();
    #else
		public DeleteTakeOverTypeModelMasterTask DeleteTakeOverTypeModelMasterAsync(
                Request.DeleteTakeOverTypeModelMasterRequest request
        )
		{
			return new DeleteTakeOverTypeModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.DeleteTakeOverTypeModelMasterResult> DeleteTakeOverTypeModelMasterAsync(
                Request.DeleteTakeOverTypeModelMasterRequest request
        ) =>
            new DeleteTakeOverTypeModelMasterTask(
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
                    .Replace("{service}", "account")
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
                    .Replace("{service}", "account")
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
                    .Replace("{service}", "account")
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
                    .Replace("{service}", "account")
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
                    .Replace("{service}", "account")
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
	}
}