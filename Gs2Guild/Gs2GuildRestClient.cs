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
using Gs2.Gs2Guild.Request;
using Gs2.Gs2Guild.Result;
using Gs2.Util.LitJson;

namespace Gs2.Gs2Guild
{
	public class Gs2GuildRestClient : AbstractGs2Client
	{
#if UNITY_2017_1_OR_NEWER
		private readonly CertificateHandler _certificateHandler;
#endif

		public static string Endpoint = "guild";

        protected Gs2RestSession Gs2RestSession => (Gs2RestSession) Gs2Session;

		public Gs2GuildRestClient(Gs2RestSession Gs2RestSession) : base(Gs2RestSession)
		{

		}

#if UNITY_2017_1_OR_NEWER
		public Gs2GuildRestClient(Gs2RestSession gs2RestSession, CertificateHandler certificateHandler) : base(gs2RestSession)
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
                    .Replace("{service}", "guild")
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
                    .Replace("{service}", "guild")
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
                if (request.ChangeNotification != null)
                {
                    jsonWriter.WritePropertyName("changeNotification");
                    request.ChangeNotification.WriteJson(jsonWriter);
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
                if (request.ChangeMemberNotification != null)
                {
                    jsonWriter.WritePropertyName("changeMemberNotification");
                    request.ChangeMemberNotification.WriteJson(jsonWriter);
                }
                if (request.ChangeMemberNotificationIgnoreChangeMetadata != null)
                {
                    jsonWriter.WritePropertyName("changeMemberNotificationIgnoreChangeMetadata");
                    jsonWriter.Write(request.ChangeMemberNotificationIgnoreChangeMetadata.ToString());
                }
                if (request.ReceiveRequestNotification != null)
                {
                    jsonWriter.WritePropertyName("receiveRequestNotification");
                    request.ReceiveRequestNotification.WriteJson(jsonWriter);
                }
                if (request.RemoveRequestNotification != null)
                {
                    jsonWriter.WritePropertyName("removeRequestNotification");
                    request.RemoveRequestNotification.WriteJson(jsonWriter);
                }
                if (request.CreateGuildScript != null)
                {
                    jsonWriter.WritePropertyName("createGuildScript");
                    request.CreateGuildScript.WriteJson(jsonWriter);
                }
                if (request.UpdateGuildScript != null)
                {
                    jsonWriter.WritePropertyName("updateGuildScript");
                    request.UpdateGuildScript.WriteJson(jsonWriter);
                }
                if (request.JoinGuildScript != null)
                {
                    jsonWriter.WritePropertyName("joinGuildScript");
                    request.JoinGuildScript.WriteJson(jsonWriter);
                }
                if (request.ReceiveJoinRequestScript != null)
                {
                    jsonWriter.WritePropertyName("receiveJoinRequestScript");
                    request.ReceiveJoinRequestScript.WriteJson(jsonWriter);
                }
                if (request.LeaveGuildScript != null)
                {
                    jsonWriter.WritePropertyName("leaveGuildScript");
                    request.LeaveGuildScript.WriteJson(jsonWriter);
                }
                if (request.ChangeRoleScript != null)
                {
                    jsonWriter.WritePropertyName("changeRoleScript");
                    request.ChangeRoleScript.WriteJson(jsonWriter);
                }
                if (request.DeleteGuildScript != null)
                {
                    jsonWriter.WritePropertyName("deleteGuildScript");
                    request.DeleteGuildScript.WriteJson(jsonWriter);
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
                    .Replace("{service}", "guild")
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
                    .Replace("{service}", "guild")
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
                    .Replace("{service}", "guild")
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
                if (request.ChangeNotification != null)
                {
                    jsonWriter.WritePropertyName("changeNotification");
                    request.ChangeNotification.WriteJson(jsonWriter);
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
                if (request.ChangeMemberNotification != null)
                {
                    jsonWriter.WritePropertyName("changeMemberNotification");
                    request.ChangeMemberNotification.WriteJson(jsonWriter);
                }
                if (request.ChangeMemberNotificationIgnoreChangeMetadata != null)
                {
                    jsonWriter.WritePropertyName("changeMemberNotificationIgnoreChangeMetadata");
                    jsonWriter.Write(request.ChangeMemberNotificationIgnoreChangeMetadata.ToString());
                }
                if (request.ReceiveRequestNotification != null)
                {
                    jsonWriter.WritePropertyName("receiveRequestNotification");
                    request.ReceiveRequestNotification.WriteJson(jsonWriter);
                }
                if (request.RemoveRequestNotification != null)
                {
                    jsonWriter.WritePropertyName("removeRequestNotification");
                    request.RemoveRequestNotification.WriteJson(jsonWriter);
                }
                if (request.CreateGuildScript != null)
                {
                    jsonWriter.WritePropertyName("createGuildScript");
                    request.CreateGuildScript.WriteJson(jsonWriter);
                }
                if (request.UpdateGuildScript != null)
                {
                    jsonWriter.WritePropertyName("updateGuildScript");
                    request.UpdateGuildScript.WriteJson(jsonWriter);
                }
                if (request.JoinGuildScript != null)
                {
                    jsonWriter.WritePropertyName("joinGuildScript");
                    request.JoinGuildScript.WriteJson(jsonWriter);
                }
                if (request.ReceiveJoinRequestScript != null)
                {
                    jsonWriter.WritePropertyName("receiveJoinRequestScript");
                    request.ReceiveJoinRequestScript.WriteJson(jsonWriter);
                }
                if (request.LeaveGuildScript != null)
                {
                    jsonWriter.WritePropertyName("leaveGuildScript");
                    request.LeaveGuildScript.WriteJson(jsonWriter);
                }
                if (request.ChangeRoleScript != null)
                {
                    jsonWriter.WritePropertyName("changeRoleScript");
                    request.ChangeRoleScript.WriteJson(jsonWriter);
                }
                if (request.DeleteGuildScript != null)
                {
                    jsonWriter.WritePropertyName("deleteGuildScript");
                    request.DeleteGuildScript.WriteJson(jsonWriter);
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
                    .Replace("{service}", "guild")
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
                    .Replace("{service}", "guild")
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
                    .Replace("{service}", "guild")
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
                    .Replace("{service}", "guild")
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
                    .Replace("{service}", "guild")
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
                    .Replace("{service}", "guild")
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
                    .Replace("{service}", "guild")
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
                    .Replace("{service}", "guild")
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
                    .Replace("{service}", "guild")
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


        public class DescribeGuildModelMastersTask : Gs2RestSessionTask<DescribeGuildModelMastersRequest, DescribeGuildModelMastersResult>
        {
            public DescribeGuildModelMastersTask(IGs2Session session, RestSessionRequestFactory factory, DescribeGuildModelMastersRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DescribeGuildModelMastersRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "guild")
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
		public IEnumerator DescribeGuildModelMasters(
                Request.DescribeGuildModelMastersRequest request,
                UnityAction<AsyncResult<Result.DescribeGuildModelMastersResult>> callback
        ) =>
            new DescribeGuildModelMastersTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.DescribeGuildModelMastersResult> DescribeGuildModelMastersFuture(
                Request.DescribeGuildModelMastersRequest request
        ) =>
            new DescribeGuildModelMastersTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.DescribeGuildModelMastersResult> DescribeGuildModelMastersAsync(
                Request.DescribeGuildModelMastersRequest request
        ) =>
            new DescribeGuildModelMastersTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.DescribeGuildModelMastersResult>();
    #else
		public DescribeGuildModelMastersTask DescribeGuildModelMastersAsync(
                Request.DescribeGuildModelMastersRequest request
        )
		{
			return new DescribeGuildModelMastersTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.DescribeGuildModelMastersResult> DescribeGuildModelMastersAsync(
                Request.DescribeGuildModelMastersRequest request
        ) =>
            new DescribeGuildModelMastersTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class CreateGuildModelMasterTask : Gs2RestSessionTask<CreateGuildModelMasterRequest, CreateGuildModelMasterResult>
        {
            public CreateGuildModelMasterTask(IGs2Session session, RestSessionRequestFactory factory, CreateGuildModelMasterRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(CreateGuildModelMasterRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "guild")
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
                if (request.DefaultMaximumMemberCount != null)
                {
                    jsonWriter.WritePropertyName("defaultMaximumMemberCount");
                    jsonWriter.Write(request.DefaultMaximumMemberCount.ToString());
                }
                if (request.MaximumMemberCount != null)
                {
                    jsonWriter.WritePropertyName("maximumMemberCount");
                    jsonWriter.Write(request.MaximumMemberCount.ToString());
                }
                if (request.InactivityPeriodDays != null)
                {
                    jsonWriter.WritePropertyName("inactivityPeriodDays");
                    jsonWriter.Write(request.InactivityPeriodDays.ToString());
                }
                if (request.Roles != null)
                {
                    jsonWriter.WritePropertyName("roles");
                    jsonWriter.WriteArrayStart();
                    foreach(var item in request.Roles)
                    {
                        item.WriteJson(jsonWriter);
                    }
                    jsonWriter.WriteArrayEnd();
                }
                if (request.GuildMasterRole != null)
                {
                    jsonWriter.WritePropertyName("guildMasterRole");
                    jsonWriter.Write(request.GuildMasterRole);
                }
                if (request.GuildMemberDefaultRole != null)
                {
                    jsonWriter.WritePropertyName("guildMemberDefaultRole");
                    jsonWriter.Write(request.GuildMemberDefaultRole);
                }
                if (request.RejoinCoolTimeMinutes != null)
                {
                    jsonWriter.WritePropertyName("rejoinCoolTimeMinutes");
                    jsonWriter.Write(request.RejoinCoolTimeMinutes.ToString());
                }
                if (request.MaxConcurrentJoinGuilds != null)
                {
                    jsonWriter.WritePropertyName("maxConcurrentJoinGuilds");
                    jsonWriter.Write(request.MaxConcurrentJoinGuilds.ToString());
                }
                if (request.MaxConcurrentGuildMasterCount != null)
                {
                    jsonWriter.WritePropertyName("maxConcurrentGuildMasterCount");
                    jsonWriter.Write(request.MaxConcurrentGuildMasterCount.ToString());
                }
                if (request.ContextStack != null)
                {
                    jsonWriter.WritePropertyName("contextStack");
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
		public IEnumerator CreateGuildModelMaster(
                Request.CreateGuildModelMasterRequest request,
                UnityAction<AsyncResult<Result.CreateGuildModelMasterResult>> callback
        ) =>
            new CreateGuildModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.CreateGuildModelMasterResult> CreateGuildModelMasterFuture(
                Request.CreateGuildModelMasterRequest request
        ) =>
            new CreateGuildModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.CreateGuildModelMasterResult> CreateGuildModelMasterAsync(
                Request.CreateGuildModelMasterRequest request
        ) =>
            new CreateGuildModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.CreateGuildModelMasterResult>();
    #else
		public CreateGuildModelMasterTask CreateGuildModelMasterAsync(
                Request.CreateGuildModelMasterRequest request
        )
		{
			return new CreateGuildModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.CreateGuildModelMasterResult> CreateGuildModelMasterAsync(
                Request.CreateGuildModelMasterRequest request
        ) =>
            new CreateGuildModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class GetGuildModelMasterTask : Gs2RestSessionTask<GetGuildModelMasterRequest, GetGuildModelMasterResult>
        {
            public GetGuildModelMasterTask(IGs2Session session, RestSessionRequestFactory factory, GetGuildModelMasterRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(GetGuildModelMasterRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "guild")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/master/model/{guildModelName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{guildModelName}", !string.IsNullOrEmpty(request.GuildModelName) ? request.GuildModelName.ToString() : "null");

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
		public IEnumerator GetGuildModelMaster(
                Request.GetGuildModelMasterRequest request,
                UnityAction<AsyncResult<Result.GetGuildModelMasterResult>> callback
        ) =>
            new GetGuildModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.GetGuildModelMasterResult> GetGuildModelMasterFuture(
                Request.GetGuildModelMasterRequest request
        ) =>
            new GetGuildModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.GetGuildModelMasterResult> GetGuildModelMasterAsync(
                Request.GetGuildModelMasterRequest request
        ) =>
            new GetGuildModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.GetGuildModelMasterResult>();
    #else
		public GetGuildModelMasterTask GetGuildModelMasterAsync(
                Request.GetGuildModelMasterRequest request
        )
		{
			return new GetGuildModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.GetGuildModelMasterResult> GetGuildModelMasterAsync(
                Request.GetGuildModelMasterRequest request
        ) =>
            new GetGuildModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class UpdateGuildModelMasterTask : Gs2RestSessionTask<UpdateGuildModelMasterRequest, UpdateGuildModelMasterResult>
        {
            public UpdateGuildModelMasterTask(IGs2Session session, RestSessionRequestFactory factory, UpdateGuildModelMasterRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(UpdateGuildModelMasterRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "guild")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/master/model/{guildModelName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{guildModelName}", !string.IsNullOrEmpty(request.GuildModelName) ? request.GuildModelName.ToString() : "null");

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
                if (request.DefaultMaximumMemberCount != null)
                {
                    jsonWriter.WritePropertyName("defaultMaximumMemberCount");
                    jsonWriter.Write(request.DefaultMaximumMemberCount.ToString());
                }
                if (request.MaximumMemberCount != null)
                {
                    jsonWriter.WritePropertyName("maximumMemberCount");
                    jsonWriter.Write(request.MaximumMemberCount.ToString());
                }
                if (request.InactivityPeriodDays != null)
                {
                    jsonWriter.WritePropertyName("inactivityPeriodDays");
                    jsonWriter.Write(request.InactivityPeriodDays.ToString());
                }
                if (request.Roles != null)
                {
                    jsonWriter.WritePropertyName("roles");
                    jsonWriter.WriteArrayStart();
                    foreach(var item in request.Roles)
                    {
                        item.WriteJson(jsonWriter);
                    }
                    jsonWriter.WriteArrayEnd();
                }
                if (request.GuildMasterRole != null)
                {
                    jsonWriter.WritePropertyName("guildMasterRole");
                    jsonWriter.Write(request.GuildMasterRole);
                }
                if (request.GuildMemberDefaultRole != null)
                {
                    jsonWriter.WritePropertyName("guildMemberDefaultRole");
                    jsonWriter.Write(request.GuildMemberDefaultRole);
                }
                if (request.RejoinCoolTimeMinutes != null)
                {
                    jsonWriter.WritePropertyName("rejoinCoolTimeMinutes");
                    jsonWriter.Write(request.RejoinCoolTimeMinutes.ToString());
                }
                if (request.MaxConcurrentJoinGuilds != null)
                {
                    jsonWriter.WritePropertyName("maxConcurrentJoinGuilds");
                    jsonWriter.Write(request.MaxConcurrentJoinGuilds.ToString());
                }
                if (request.MaxConcurrentGuildMasterCount != null)
                {
                    jsonWriter.WritePropertyName("maxConcurrentGuildMasterCount");
                    jsonWriter.Write(request.MaxConcurrentGuildMasterCount.ToString());
                }
                if (request.ContextStack != null)
                {
                    jsonWriter.WritePropertyName("contextStack");
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
		public IEnumerator UpdateGuildModelMaster(
                Request.UpdateGuildModelMasterRequest request,
                UnityAction<AsyncResult<Result.UpdateGuildModelMasterResult>> callback
        ) =>
            new UpdateGuildModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.UpdateGuildModelMasterResult> UpdateGuildModelMasterFuture(
                Request.UpdateGuildModelMasterRequest request
        ) =>
            new UpdateGuildModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.UpdateGuildModelMasterResult> UpdateGuildModelMasterAsync(
                Request.UpdateGuildModelMasterRequest request
        ) =>
            new UpdateGuildModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.UpdateGuildModelMasterResult>();
    #else
		public UpdateGuildModelMasterTask UpdateGuildModelMasterAsync(
                Request.UpdateGuildModelMasterRequest request
        )
		{
			return new UpdateGuildModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.UpdateGuildModelMasterResult> UpdateGuildModelMasterAsync(
                Request.UpdateGuildModelMasterRequest request
        ) =>
            new UpdateGuildModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class DeleteGuildModelMasterTask : Gs2RestSessionTask<DeleteGuildModelMasterRequest, DeleteGuildModelMasterResult>
        {
            public DeleteGuildModelMasterTask(IGs2Session session, RestSessionRequestFactory factory, DeleteGuildModelMasterRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DeleteGuildModelMasterRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "guild")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/master/model/{guildModelName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{guildModelName}", !string.IsNullOrEmpty(request.GuildModelName) ? request.GuildModelName.ToString() : "null");

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
		public IEnumerator DeleteGuildModelMaster(
                Request.DeleteGuildModelMasterRequest request,
                UnityAction<AsyncResult<Result.DeleteGuildModelMasterResult>> callback
        ) =>
            new DeleteGuildModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.DeleteGuildModelMasterResult> DeleteGuildModelMasterFuture(
                Request.DeleteGuildModelMasterRequest request
        ) =>
            new DeleteGuildModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.DeleteGuildModelMasterResult> DeleteGuildModelMasterAsync(
                Request.DeleteGuildModelMasterRequest request
        ) =>
            new DeleteGuildModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.DeleteGuildModelMasterResult>();
    #else
		public DeleteGuildModelMasterTask DeleteGuildModelMasterAsync(
                Request.DeleteGuildModelMasterRequest request
        )
		{
			return new DeleteGuildModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.DeleteGuildModelMasterResult> DeleteGuildModelMasterAsync(
                Request.DeleteGuildModelMasterRequest request
        ) =>
            new DeleteGuildModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class DescribeGuildModelsTask : Gs2RestSessionTask<DescribeGuildModelsRequest, DescribeGuildModelsResult>
        {
            public DescribeGuildModelsTask(IGs2Session session, RestSessionRequestFactory factory, DescribeGuildModelsRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DescribeGuildModelsRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "guild")
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
		public IEnumerator DescribeGuildModels(
                Request.DescribeGuildModelsRequest request,
                UnityAction<AsyncResult<Result.DescribeGuildModelsResult>> callback
        ) =>
            new DescribeGuildModelsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.DescribeGuildModelsResult> DescribeGuildModelsFuture(
                Request.DescribeGuildModelsRequest request
        ) =>
            new DescribeGuildModelsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.DescribeGuildModelsResult> DescribeGuildModelsAsync(
                Request.DescribeGuildModelsRequest request
        ) =>
            new DescribeGuildModelsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.DescribeGuildModelsResult>();
    #else
		public DescribeGuildModelsTask DescribeGuildModelsAsync(
                Request.DescribeGuildModelsRequest request
        )
		{
			return new DescribeGuildModelsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.DescribeGuildModelsResult> DescribeGuildModelsAsync(
                Request.DescribeGuildModelsRequest request
        ) =>
            new DescribeGuildModelsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class GetGuildModelTask : Gs2RestSessionTask<GetGuildModelRequest, GetGuildModelResult>
        {
            public GetGuildModelTask(IGs2Session session, RestSessionRequestFactory factory, GetGuildModelRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(GetGuildModelRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "guild")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/model/{guildModelName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{guildModelName}", !string.IsNullOrEmpty(request.GuildModelName) ? request.GuildModelName.ToString() : "null");

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
		public IEnumerator GetGuildModel(
                Request.GetGuildModelRequest request,
                UnityAction<AsyncResult<Result.GetGuildModelResult>> callback
        ) =>
            new GetGuildModelTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.GetGuildModelResult> GetGuildModelFuture(
                Request.GetGuildModelRequest request
        ) =>
            new GetGuildModelTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.GetGuildModelResult> GetGuildModelAsync(
                Request.GetGuildModelRequest request
        ) =>
            new GetGuildModelTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.GetGuildModelResult>();
    #else
		public GetGuildModelTask GetGuildModelAsync(
                Request.GetGuildModelRequest request
        )
		{
			return new GetGuildModelTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.GetGuildModelResult> GetGuildModelAsync(
                Request.GetGuildModelRequest request
        ) =>
            new GetGuildModelTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class SearchGuildsTask : Gs2RestSessionTask<SearchGuildsRequest, SearchGuildsResult>
        {
            public SearchGuildsTask(IGs2Session session, RestSessionRequestFactory factory, SearchGuildsRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(SearchGuildsRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "guild")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/me/guild/{guildModelName}/search";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{guildModelName}", !string.IsNullOrEmpty(request.GuildModelName) ? request.GuildModelName.ToString() : "null");

                var sessionRequest = Factory.Post(url);

                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);
                jsonWriter.WriteObjectStart();
                if (request.DisplayName != null)
                {
                    jsonWriter.WritePropertyName("displayName");
                    jsonWriter.Write(request.DisplayName);
                }
                if (request.Attributes1 != null)
                {
                    jsonWriter.WritePropertyName("attributes1");
                    jsonWriter.WriteArrayStart();
                    foreach(var item in request.Attributes1)
                    {
                        jsonWriter.Write(item);
                    }
                    jsonWriter.WriteArrayEnd();
                }
                if (request.Attributes2 != null)
                {
                    jsonWriter.WritePropertyName("attributes2");
                    jsonWriter.WriteArrayStart();
                    foreach(var item in request.Attributes2)
                    {
                        jsonWriter.Write(item);
                    }
                    jsonWriter.WriteArrayEnd();
                }
                if (request.Attributes3 != null)
                {
                    jsonWriter.WritePropertyName("attributes3");
                    jsonWriter.WriteArrayStart();
                    foreach(var item in request.Attributes3)
                    {
                        jsonWriter.Write(item);
                    }
                    jsonWriter.WriteArrayEnd();
                }
                if (request.Attributes4 != null)
                {
                    jsonWriter.WritePropertyName("attributes4");
                    jsonWriter.WriteArrayStart();
                    foreach(var item in request.Attributes4)
                    {
                        jsonWriter.Write(item);
                    }
                    jsonWriter.WriteArrayEnd();
                }
                if (request.Attributes5 != null)
                {
                    jsonWriter.WritePropertyName("attributes5");
                    jsonWriter.WriteArrayStart();
                    foreach(var item in request.Attributes5)
                    {
                        jsonWriter.Write(item);
                    }
                    jsonWriter.WriteArrayEnd();
                }
                if (request.JoinPolicies != null)
                {
                    jsonWriter.WritePropertyName("joinPolicies");
                    jsonWriter.WriteArrayStart();
                    foreach(var item in request.JoinPolicies)
                    {
                        jsonWriter.Write(item);
                    }
                    jsonWriter.WriteArrayEnd();
                }
                if (request.IncludeFullMembersGuild != null)
                {
                    jsonWriter.WritePropertyName("includeFullMembersGuild");
                    jsonWriter.Write(request.IncludeFullMembersGuild.ToString());
                }
                if (request.OrderBy != null)
                {
                    jsonWriter.WritePropertyName("orderBy");
                    jsonWriter.Write(request.OrderBy);
                }
                if (request.PageToken != null)
                {
                    jsonWriter.WritePropertyName("pageToken");
                    jsonWriter.Write(request.PageToken);
                }
                if (request.Limit != null)
                {
                    jsonWriter.WritePropertyName("limit");
                    jsonWriter.Write(request.Limit.ToString());
                }
                if (request.ContextStack != null)
                {
                    jsonWriter.WritePropertyName("contextStack");
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
		public IEnumerator SearchGuilds(
                Request.SearchGuildsRequest request,
                UnityAction<AsyncResult<Result.SearchGuildsResult>> callback
        ) =>
            new SearchGuildsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.SearchGuildsResult> SearchGuildsFuture(
                Request.SearchGuildsRequest request
        ) =>
            new SearchGuildsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.SearchGuildsResult> SearchGuildsAsync(
                Request.SearchGuildsRequest request
        ) =>
            new SearchGuildsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.SearchGuildsResult>();
    #else
		public SearchGuildsTask SearchGuildsAsync(
                Request.SearchGuildsRequest request
        )
		{
			return new SearchGuildsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.SearchGuildsResult> SearchGuildsAsync(
                Request.SearchGuildsRequest request
        ) =>
            new SearchGuildsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class SearchGuildsByUserIdTask : Gs2RestSessionTask<SearchGuildsByUserIdRequest, SearchGuildsByUserIdResult>
        {
            public SearchGuildsByUserIdTask(IGs2Session session, RestSessionRequestFactory factory, SearchGuildsByUserIdRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(SearchGuildsByUserIdRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "guild")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/{userId}/guild/{guildModelName}/search";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{guildModelName}", !string.IsNullOrEmpty(request.GuildModelName) ? request.GuildModelName.ToString() : "null");
                url = url.Replace("{userId}", !string.IsNullOrEmpty(request.UserId) ? request.UserId.ToString() : "null");

                var sessionRequest = Factory.Post(url);

                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);
                jsonWriter.WriteObjectStart();
                if (request.DisplayName != null)
                {
                    jsonWriter.WritePropertyName("displayName");
                    jsonWriter.Write(request.DisplayName);
                }
                if (request.Attributes1 != null)
                {
                    jsonWriter.WritePropertyName("attributes1");
                    jsonWriter.WriteArrayStart();
                    foreach(var item in request.Attributes1)
                    {
                        jsonWriter.Write(item);
                    }
                    jsonWriter.WriteArrayEnd();
                }
                if (request.Attributes2 != null)
                {
                    jsonWriter.WritePropertyName("attributes2");
                    jsonWriter.WriteArrayStart();
                    foreach(var item in request.Attributes2)
                    {
                        jsonWriter.Write(item);
                    }
                    jsonWriter.WriteArrayEnd();
                }
                if (request.Attributes3 != null)
                {
                    jsonWriter.WritePropertyName("attributes3");
                    jsonWriter.WriteArrayStart();
                    foreach(var item in request.Attributes3)
                    {
                        jsonWriter.Write(item);
                    }
                    jsonWriter.WriteArrayEnd();
                }
                if (request.Attributes4 != null)
                {
                    jsonWriter.WritePropertyName("attributes4");
                    jsonWriter.WriteArrayStart();
                    foreach(var item in request.Attributes4)
                    {
                        jsonWriter.Write(item);
                    }
                    jsonWriter.WriteArrayEnd();
                }
                if (request.Attributes5 != null)
                {
                    jsonWriter.WritePropertyName("attributes5");
                    jsonWriter.WriteArrayStart();
                    foreach(var item in request.Attributes5)
                    {
                        jsonWriter.Write(item);
                    }
                    jsonWriter.WriteArrayEnd();
                }
                if (request.JoinPolicies != null)
                {
                    jsonWriter.WritePropertyName("joinPolicies");
                    jsonWriter.WriteArrayStart();
                    foreach(var item in request.JoinPolicies)
                    {
                        jsonWriter.Write(item);
                    }
                    jsonWriter.WriteArrayEnd();
                }
                if (request.IncludeFullMembersGuild != null)
                {
                    jsonWriter.WritePropertyName("includeFullMembersGuild");
                    jsonWriter.Write(request.IncludeFullMembersGuild.ToString());
                }
                if (request.OrderBy != null)
                {
                    jsonWriter.WritePropertyName("orderBy");
                    jsonWriter.Write(request.OrderBy);
                }
                if (request.PageToken != null)
                {
                    jsonWriter.WritePropertyName("pageToken");
                    jsonWriter.Write(request.PageToken);
                }
                if (request.Limit != null)
                {
                    jsonWriter.WritePropertyName("limit");
                    jsonWriter.Write(request.Limit.ToString());
                }
                if (request.ContextStack != null)
                {
                    jsonWriter.WritePropertyName("contextStack");
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
		public IEnumerator SearchGuildsByUserId(
                Request.SearchGuildsByUserIdRequest request,
                UnityAction<AsyncResult<Result.SearchGuildsByUserIdResult>> callback
        ) =>
            new SearchGuildsByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.SearchGuildsByUserIdResult> SearchGuildsByUserIdFuture(
                Request.SearchGuildsByUserIdRequest request
        ) =>
            new SearchGuildsByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.SearchGuildsByUserIdResult> SearchGuildsByUserIdAsync(
                Request.SearchGuildsByUserIdRequest request
        ) =>
            new SearchGuildsByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.SearchGuildsByUserIdResult>();
    #else
		public SearchGuildsByUserIdTask SearchGuildsByUserIdAsync(
                Request.SearchGuildsByUserIdRequest request
        )
		{
			return new SearchGuildsByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.SearchGuildsByUserIdResult> SearchGuildsByUserIdAsync(
                Request.SearchGuildsByUserIdRequest request
        ) =>
            new SearchGuildsByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class CreateGuildTask : Gs2RestSessionTask<CreateGuildRequest, CreateGuildResult>
        {
            public CreateGuildTask(IGs2Session session, RestSessionRequestFactory factory, CreateGuildRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(CreateGuildRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "guild")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/me/guild/{guildModelName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{guildModelName}", !string.IsNullOrEmpty(request.GuildModelName) ? request.GuildModelName.ToString() : "null");

                var sessionRequest = Factory.Post(url);

                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);
                jsonWriter.WriteObjectStart();
                if (request.DisplayName != null)
                {
                    jsonWriter.WritePropertyName("displayName");
                    jsonWriter.Write(request.DisplayName);
                }
                if (request.Attribute1 != null)
                {
                    jsonWriter.WritePropertyName("attribute1");
                    jsonWriter.Write(request.Attribute1.ToString());
                }
                if (request.Attribute2 != null)
                {
                    jsonWriter.WritePropertyName("attribute2");
                    jsonWriter.Write(request.Attribute2.ToString());
                }
                if (request.Attribute3 != null)
                {
                    jsonWriter.WritePropertyName("attribute3");
                    jsonWriter.Write(request.Attribute3.ToString());
                }
                if (request.Attribute4 != null)
                {
                    jsonWriter.WritePropertyName("attribute4");
                    jsonWriter.Write(request.Attribute4.ToString());
                }
                if (request.Attribute5 != null)
                {
                    jsonWriter.WritePropertyName("attribute5");
                    jsonWriter.Write(request.Attribute5.ToString());
                }
                if (request.Metadata != null)
                {
                    jsonWriter.WritePropertyName("metadata");
                    jsonWriter.Write(request.Metadata);
                }
                if (request.MemberMetadata != null)
                {
                    jsonWriter.WritePropertyName("memberMetadata");
                    jsonWriter.Write(request.MemberMetadata);
                }
                if (request.JoinPolicy != null)
                {
                    jsonWriter.WritePropertyName("joinPolicy");
                    jsonWriter.Write(request.JoinPolicy);
                }
                if (request.CustomRoles != null)
                {
                    jsonWriter.WritePropertyName("customRoles");
                    jsonWriter.WriteArrayStart();
                    foreach(var item in request.CustomRoles)
                    {
                        item.WriteJson(jsonWriter);
                    }
                    jsonWriter.WriteArrayEnd();
                }
                if (request.GuildMemberDefaultRole != null)
                {
                    jsonWriter.WritePropertyName("guildMemberDefaultRole");
                    jsonWriter.Write(request.GuildMemberDefaultRole);
                }
                if (request.ContextStack != null)
                {
                    jsonWriter.WritePropertyName("contextStack");
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
		public IEnumerator CreateGuild(
                Request.CreateGuildRequest request,
                UnityAction<AsyncResult<Result.CreateGuildResult>> callback
        ) =>
            new CreateGuildTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.CreateGuildResult> CreateGuildFuture(
                Request.CreateGuildRequest request
        ) =>
            new CreateGuildTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.CreateGuildResult> CreateGuildAsync(
                Request.CreateGuildRequest request
        ) =>
            new CreateGuildTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.CreateGuildResult>();
    #else
		public CreateGuildTask CreateGuildAsync(
                Request.CreateGuildRequest request
        )
		{
			return new CreateGuildTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.CreateGuildResult> CreateGuildAsync(
                Request.CreateGuildRequest request
        ) =>
            new CreateGuildTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class CreateGuildByUserIdTask : Gs2RestSessionTask<CreateGuildByUserIdRequest, CreateGuildByUserIdResult>
        {
            public CreateGuildByUserIdTask(IGs2Session session, RestSessionRequestFactory factory, CreateGuildByUserIdRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(CreateGuildByUserIdRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "guild")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/{userId}/guild/{guildModelName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{userId}", !string.IsNullOrEmpty(request.UserId) ? request.UserId.ToString() : "null");
                url = url.Replace("{guildModelName}", !string.IsNullOrEmpty(request.GuildModelName) ? request.GuildModelName.ToString() : "null");

                var sessionRequest = Factory.Post(url);

                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);
                jsonWriter.WriteObjectStart();
                if (request.DisplayName != null)
                {
                    jsonWriter.WritePropertyName("displayName");
                    jsonWriter.Write(request.DisplayName);
                }
                if (request.Attribute1 != null)
                {
                    jsonWriter.WritePropertyName("attribute1");
                    jsonWriter.Write(request.Attribute1.ToString());
                }
                if (request.Attribute2 != null)
                {
                    jsonWriter.WritePropertyName("attribute2");
                    jsonWriter.Write(request.Attribute2.ToString());
                }
                if (request.Attribute3 != null)
                {
                    jsonWriter.WritePropertyName("attribute3");
                    jsonWriter.Write(request.Attribute3.ToString());
                }
                if (request.Attribute4 != null)
                {
                    jsonWriter.WritePropertyName("attribute4");
                    jsonWriter.Write(request.Attribute4.ToString());
                }
                if (request.Attribute5 != null)
                {
                    jsonWriter.WritePropertyName("attribute5");
                    jsonWriter.Write(request.Attribute5.ToString());
                }
                if (request.Metadata != null)
                {
                    jsonWriter.WritePropertyName("metadata");
                    jsonWriter.Write(request.Metadata);
                }
                if (request.MemberMetadata != null)
                {
                    jsonWriter.WritePropertyName("memberMetadata");
                    jsonWriter.Write(request.MemberMetadata);
                }
                if (request.JoinPolicy != null)
                {
                    jsonWriter.WritePropertyName("joinPolicy");
                    jsonWriter.Write(request.JoinPolicy);
                }
                if (request.CustomRoles != null)
                {
                    jsonWriter.WritePropertyName("customRoles");
                    jsonWriter.WriteArrayStart();
                    foreach(var item in request.CustomRoles)
                    {
                        item.WriteJson(jsonWriter);
                    }
                    jsonWriter.WriteArrayEnd();
                }
                if (request.GuildMemberDefaultRole != null)
                {
                    jsonWriter.WritePropertyName("guildMemberDefaultRole");
                    jsonWriter.Write(request.GuildMemberDefaultRole);
                }
                if (request.ContextStack != null)
                {
                    jsonWriter.WritePropertyName("contextStack");
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
		public IEnumerator CreateGuildByUserId(
                Request.CreateGuildByUserIdRequest request,
                UnityAction<AsyncResult<Result.CreateGuildByUserIdResult>> callback
        ) =>
            new CreateGuildByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.CreateGuildByUserIdResult> CreateGuildByUserIdFuture(
                Request.CreateGuildByUserIdRequest request
        ) =>
            new CreateGuildByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.CreateGuildByUserIdResult> CreateGuildByUserIdAsync(
                Request.CreateGuildByUserIdRequest request
        ) =>
            new CreateGuildByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.CreateGuildByUserIdResult>();
    #else
		public CreateGuildByUserIdTask CreateGuildByUserIdAsync(
                Request.CreateGuildByUserIdRequest request
        )
		{
			return new CreateGuildByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.CreateGuildByUserIdResult> CreateGuildByUserIdAsync(
                Request.CreateGuildByUserIdRequest request
        ) =>
            new CreateGuildByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class GetGuildTask : Gs2RestSessionTask<GetGuildRequest, GetGuildResult>
        {
            public GetGuildTask(IGs2Session session, RestSessionRequestFactory factory, GetGuildRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(GetGuildRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "guild")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/me/guild/{guildModelName}/{guildName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{guildModelName}", !string.IsNullOrEmpty(request.GuildModelName) ? request.GuildModelName.ToString() : "null");
                url = url.Replace("{guildName}", !string.IsNullOrEmpty(request.GuildName) ? request.GuildName.ToString() : "null");

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
		public IEnumerator GetGuild(
                Request.GetGuildRequest request,
                UnityAction<AsyncResult<Result.GetGuildResult>> callback
        ) =>
            new GetGuildTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.GetGuildResult> GetGuildFuture(
                Request.GetGuildRequest request
        ) =>
            new GetGuildTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.GetGuildResult> GetGuildAsync(
                Request.GetGuildRequest request
        ) =>
            new GetGuildTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.GetGuildResult>();
    #else
		public GetGuildTask GetGuildAsync(
                Request.GetGuildRequest request
        )
		{
			return new GetGuildTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.GetGuildResult> GetGuildAsync(
                Request.GetGuildRequest request
        ) =>
            new GetGuildTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class GetGuildByUserIdTask : Gs2RestSessionTask<GetGuildByUserIdRequest, GetGuildByUserIdResult>
        {
            public GetGuildByUserIdTask(IGs2Session session, RestSessionRequestFactory factory, GetGuildByUserIdRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(GetGuildByUserIdRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "guild")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/{userId}/guild/{guildModelName}/{guildName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{userId}", !string.IsNullOrEmpty(request.UserId) ? request.UserId.ToString() : "null");
                url = url.Replace("{guildModelName}", !string.IsNullOrEmpty(request.GuildModelName) ? request.GuildModelName.ToString() : "null");
                url = url.Replace("{guildName}", !string.IsNullOrEmpty(request.GuildName) ? request.GuildName.ToString() : "null");

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
		public IEnumerator GetGuildByUserId(
                Request.GetGuildByUserIdRequest request,
                UnityAction<AsyncResult<Result.GetGuildByUserIdResult>> callback
        ) =>
            new GetGuildByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.GetGuildByUserIdResult> GetGuildByUserIdFuture(
                Request.GetGuildByUserIdRequest request
        ) =>
            new GetGuildByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.GetGuildByUserIdResult> GetGuildByUserIdAsync(
                Request.GetGuildByUserIdRequest request
        ) =>
            new GetGuildByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.GetGuildByUserIdResult>();
    #else
		public GetGuildByUserIdTask GetGuildByUserIdAsync(
                Request.GetGuildByUserIdRequest request
        )
		{
			return new GetGuildByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.GetGuildByUserIdResult> GetGuildByUserIdAsync(
                Request.GetGuildByUserIdRequest request
        ) =>
            new GetGuildByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class UpdateGuildTask : Gs2RestSessionTask<UpdateGuildRequest, UpdateGuildResult>
        {
            public UpdateGuildTask(IGs2Session session, RestSessionRequestFactory factory, UpdateGuildRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(UpdateGuildRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "guild")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/guild/{guildModelName}/me";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{guildModelName}", !string.IsNullOrEmpty(request.GuildModelName) ? request.GuildModelName.ToString() : "null");

                var sessionRequest = Factory.Put(url);

                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);
                jsonWriter.WriteObjectStart();
                if (request.DisplayName != null)
                {
                    jsonWriter.WritePropertyName("displayName");
                    jsonWriter.Write(request.DisplayName);
                }
                if (request.Attribute1 != null)
                {
                    jsonWriter.WritePropertyName("attribute1");
                    jsonWriter.Write(request.Attribute1.ToString());
                }
                if (request.Attribute2 != null)
                {
                    jsonWriter.WritePropertyName("attribute2");
                    jsonWriter.Write(request.Attribute2.ToString());
                }
                if (request.Attribute3 != null)
                {
                    jsonWriter.WritePropertyName("attribute3");
                    jsonWriter.Write(request.Attribute3.ToString());
                }
                if (request.Attribute4 != null)
                {
                    jsonWriter.WritePropertyName("attribute4");
                    jsonWriter.Write(request.Attribute4.ToString());
                }
                if (request.Attribute5 != null)
                {
                    jsonWriter.WritePropertyName("attribute5");
                    jsonWriter.Write(request.Attribute5.ToString());
                }
                if (request.Metadata != null)
                {
                    jsonWriter.WritePropertyName("metadata");
                    jsonWriter.Write(request.Metadata);
                }
                if (request.JoinPolicy != null)
                {
                    jsonWriter.WritePropertyName("joinPolicy");
                    jsonWriter.Write(request.JoinPolicy);
                }
                if (request.CustomRoles != null)
                {
                    jsonWriter.WritePropertyName("customRoles");
                    jsonWriter.WriteArrayStart();
                    foreach(var item in request.CustomRoles)
                    {
                        item.WriteJson(jsonWriter);
                    }
                    jsonWriter.WriteArrayEnd();
                }
                if (request.GuildMemberDefaultRole != null)
                {
                    jsonWriter.WritePropertyName("guildMemberDefaultRole");
                    jsonWriter.Write(request.GuildMemberDefaultRole);
                }
                if (request.ContextStack != null)
                {
                    jsonWriter.WritePropertyName("contextStack");
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
		public IEnumerator UpdateGuild(
                Request.UpdateGuildRequest request,
                UnityAction<AsyncResult<Result.UpdateGuildResult>> callback
        ) =>
            new UpdateGuildTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.UpdateGuildResult> UpdateGuildFuture(
                Request.UpdateGuildRequest request
        ) =>
            new UpdateGuildTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.UpdateGuildResult> UpdateGuildAsync(
                Request.UpdateGuildRequest request
        ) =>
            new UpdateGuildTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.UpdateGuildResult>();
    #else
		public UpdateGuildTask UpdateGuildAsync(
                Request.UpdateGuildRequest request
        )
		{
			return new UpdateGuildTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.UpdateGuildResult> UpdateGuildAsync(
                Request.UpdateGuildRequest request
        ) =>
            new UpdateGuildTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class UpdateGuildByGuildNameTask : Gs2RestSessionTask<UpdateGuildByGuildNameRequest, UpdateGuildByGuildNameResult>
        {
            public UpdateGuildByGuildNameTask(IGs2Session session, RestSessionRequestFactory factory, UpdateGuildByGuildNameRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(UpdateGuildByGuildNameRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "guild")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/guild/{guildModelName}/{guildName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{guildName}", !string.IsNullOrEmpty(request.GuildName) ? request.GuildName.ToString() : "null");
                url = url.Replace("{guildModelName}", !string.IsNullOrEmpty(request.GuildModelName) ? request.GuildModelName.ToString() : "null");

                var sessionRequest = Factory.Put(url);

                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);
                jsonWriter.WriteObjectStart();
                if (request.DisplayName != null)
                {
                    jsonWriter.WritePropertyName("displayName");
                    jsonWriter.Write(request.DisplayName);
                }
                if (request.Attribute1 != null)
                {
                    jsonWriter.WritePropertyName("attribute1");
                    jsonWriter.Write(request.Attribute1.ToString());
                }
                if (request.Attribute2 != null)
                {
                    jsonWriter.WritePropertyName("attribute2");
                    jsonWriter.Write(request.Attribute2.ToString());
                }
                if (request.Attribute3 != null)
                {
                    jsonWriter.WritePropertyName("attribute3");
                    jsonWriter.Write(request.Attribute3.ToString());
                }
                if (request.Attribute4 != null)
                {
                    jsonWriter.WritePropertyName("attribute4");
                    jsonWriter.Write(request.Attribute4.ToString());
                }
                if (request.Attribute5 != null)
                {
                    jsonWriter.WritePropertyName("attribute5");
                    jsonWriter.Write(request.Attribute5.ToString());
                }
                if (request.Metadata != null)
                {
                    jsonWriter.WritePropertyName("metadata");
                    jsonWriter.Write(request.Metadata);
                }
                if (request.JoinPolicy != null)
                {
                    jsonWriter.WritePropertyName("joinPolicy");
                    jsonWriter.Write(request.JoinPolicy);
                }
                if (request.CustomRoles != null)
                {
                    jsonWriter.WritePropertyName("customRoles");
                    jsonWriter.WriteArrayStart();
                    foreach(var item in request.CustomRoles)
                    {
                        item.WriteJson(jsonWriter);
                    }
                    jsonWriter.WriteArrayEnd();
                }
                if (request.GuildMemberDefaultRole != null)
                {
                    jsonWriter.WritePropertyName("guildMemberDefaultRole");
                    jsonWriter.Write(request.GuildMemberDefaultRole);
                }
                if (request.ContextStack != null)
                {
                    jsonWriter.WritePropertyName("contextStack");
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
                if (request.DryRun)
                {
                    sessionRequest.AddHeader("X-GS2-DRY-RUN", "true");
                }

                AddHeader(
                    Session.Credential,
                    sessionRequest
                );

                return sessionRequest;
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator UpdateGuildByGuildName(
                Request.UpdateGuildByGuildNameRequest request,
                UnityAction<AsyncResult<Result.UpdateGuildByGuildNameResult>> callback
        ) =>
            new UpdateGuildByGuildNameTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.UpdateGuildByGuildNameResult> UpdateGuildByGuildNameFuture(
                Request.UpdateGuildByGuildNameRequest request
        ) =>
            new UpdateGuildByGuildNameTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.UpdateGuildByGuildNameResult> UpdateGuildByGuildNameAsync(
                Request.UpdateGuildByGuildNameRequest request
        ) =>
            new UpdateGuildByGuildNameTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.UpdateGuildByGuildNameResult>();
    #else
		public UpdateGuildByGuildNameTask UpdateGuildByGuildNameAsync(
                Request.UpdateGuildByGuildNameRequest request
        )
		{
			return new UpdateGuildByGuildNameTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.UpdateGuildByGuildNameResult> UpdateGuildByGuildNameAsync(
                Request.UpdateGuildByGuildNameRequest request
        ) =>
            new UpdateGuildByGuildNameTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class DeleteMemberTask : Gs2RestSessionTask<DeleteMemberRequest, DeleteMemberResult>
        {
            public DeleteMemberTask(IGs2Session session, RestSessionRequestFactory factory, DeleteMemberRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DeleteMemberRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "guild")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/guild/{guildModelName}/me/member/{targetUserId}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{guildModelName}", !string.IsNullOrEmpty(request.GuildModelName) ? request.GuildModelName.ToString() : "null");
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

            public override void OnError(Gs2.Core.Exception.Gs2Exception error)
            {
                if (error.Errors.Count(v => v.code == "guild.member.master.require") > 0) {
                    base.OnError(new Exception.GuildMasterRequiredException(error));
                }
                else {
                    base.OnError(error);
                }
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator DeleteMember(
                Request.DeleteMemberRequest request,
                UnityAction<AsyncResult<Result.DeleteMemberResult>> callback
        ) =>
            new DeleteMemberTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.DeleteMemberResult> DeleteMemberFuture(
                Request.DeleteMemberRequest request
        ) =>
            new DeleteMemberTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.DeleteMemberResult> DeleteMemberAsync(
                Request.DeleteMemberRequest request
        ) =>
            new DeleteMemberTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.DeleteMemberResult>();
    #else
		public DeleteMemberTask DeleteMemberAsync(
                Request.DeleteMemberRequest request
        )
		{
			return new DeleteMemberTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.DeleteMemberResult> DeleteMemberAsync(
                Request.DeleteMemberRequest request
        ) =>
            new DeleteMemberTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class DeleteMemberByGuildNameTask : Gs2RestSessionTask<DeleteMemberByGuildNameRequest, DeleteMemberByGuildNameResult>
        {
            public DeleteMemberByGuildNameTask(IGs2Session session, RestSessionRequestFactory factory, DeleteMemberByGuildNameRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DeleteMemberByGuildNameRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "guild")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/guild/{guildModelName}/{guildName}/member/{targetUserId}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{guildModelName}", !string.IsNullOrEmpty(request.GuildModelName) ? request.GuildModelName.ToString() : "null");
                url = url.Replace("{guildName}", !string.IsNullOrEmpty(request.GuildName) ? request.GuildName.ToString() : "null");
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
                if (request.DryRun)
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
                if (error.Errors.Count(v => v.code == "guild.member.master.require") > 0) {
                    base.OnError(new Exception.GuildMasterRequiredException(error));
                }
                else {
                    base.OnError(error);
                }
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator DeleteMemberByGuildName(
                Request.DeleteMemberByGuildNameRequest request,
                UnityAction<AsyncResult<Result.DeleteMemberByGuildNameResult>> callback
        ) =>
            new DeleteMemberByGuildNameTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.DeleteMemberByGuildNameResult> DeleteMemberByGuildNameFuture(
                Request.DeleteMemberByGuildNameRequest request
        ) =>
            new DeleteMemberByGuildNameTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.DeleteMemberByGuildNameResult> DeleteMemberByGuildNameAsync(
                Request.DeleteMemberByGuildNameRequest request
        ) =>
            new DeleteMemberByGuildNameTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.DeleteMemberByGuildNameResult>();
    #else
		public DeleteMemberByGuildNameTask DeleteMemberByGuildNameAsync(
                Request.DeleteMemberByGuildNameRequest request
        )
		{
			return new DeleteMemberByGuildNameTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.DeleteMemberByGuildNameResult> DeleteMemberByGuildNameAsync(
                Request.DeleteMemberByGuildNameRequest request
        ) =>
            new DeleteMemberByGuildNameTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class UpdateMemberRoleTask : Gs2RestSessionTask<UpdateMemberRoleRequest, UpdateMemberRoleResult>
        {
            public UpdateMemberRoleTask(IGs2Session session, RestSessionRequestFactory factory, UpdateMemberRoleRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(UpdateMemberRoleRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "guild")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/guild/{guildModelName}/me/member/{targetUserId}/role";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{guildModelName}", !string.IsNullOrEmpty(request.GuildModelName) ? request.GuildModelName.ToString() : "null");
                url = url.Replace("{targetUserId}", !string.IsNullOrEmpty(request.TargetUserId) ? request.TargetUserId.ToString() : "null");

                var sessionRequest = Factory.Put(url);

                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);
                jsonWriter.WriteObjectStart();
                if (request.RoleName != null)
                {
                    jsonWriter.WritePropertyName("roleName");
                    jsonWriter.Write(request.RoleName);
                }
                if (request.ContextStack != null)
                {
                    jsonWriter.WritePropertyName("contextStack");
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
		public IEnumerator UpdateMemberRole(
                Request.UpdateMemberRoleRequest request,
                UnityAction<AsyncResult<Result.UpdateMemberRoleResult>> callback
        ) =>
            new UpdateMemberRoleTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.UpdateMemberRoleResult> UpdateMemberRoleFuture(
                Request.UpdateMemberRoleRequest request
        ) =>
            new UpdateMemberRoleTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.UpdateMemberRoleResult> UpdateMemberRoleAsync(
                Request.UpdateMemberRoleRequest request
        ) =>
            new UpdateMemberRoleTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.UpdateMemberRoleResult>();
    #else
		public UpdateMemberRoleTask UpdateMemberRoleAsync(
                Request.UpdateMemberRoleRequest request
        )
		{
			return new UpdateMemberRoleTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.UpdateMemberRoleResult> UpdateMemberRoleAsync(
                Request.UpdateMemberRoleRequest request
        ) =>
            new UpdateMemberRoleTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class UpdateMemberRoleByGuildNameTask : Gs2RestSessionTask<UpdateMemberRoleByGuildNameRequest, UpdateMemberRoleByGuildNameResult>
        {
            public UpdateMemberRoleByGuildNameTask(IGs2Session session, RestSessionRequestFactory factory, UpdateMemberRoleByGuildNameRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(UpdateMemberRoleByGuildNameRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "guild")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/guild/{guildModelName}/{guildName}/member/{targetUserId}/role";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{guildModelName}", !string.IsNullOrEmpty(request.GuildModelName) ? request.GuildModelName.ToString() : "null");
                url = url.Replace("{guildName}", !string.IsNullOrEmpty(request.GuildName) ? request.GuildName.ToString() : "null");
                url = url.Replace("{targetUserId}", !string.IsNullOrEmpty(request.TargetUserId) ? request.TargetUserId.ToString() : "null");

                var sessionRequest = Factory.Put(url);

                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);
                jsonWriter.WriteObjectStart();
                if (request.RoleName != null)
                {
                    jsonWriter.WritePropertyName("roleName");
                    jsonWriter.Write(request.RoleName);
                }
                if (request.ContextStack != null)
                {
                    jsonWriter.WritePropertyName("contextStack");
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
                if (request.DryRun)
                {
                    sessionRequest.AddHeader("X-GS2-DRY-RUN", "true");
                }

                AddHeader(
                    Session.Credential,
                    sessionRequest
                );

                return sessionRequest;
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator UpdateMemberRoleByGuildName(
                Request.UpdateMemberRoleByGuildNameRequest request,
                UnityAction<AsyncResult<Result.UpdateMemberRoleByGuildNameResult>> callback
        ) =>
            new UpdateMemberRoleByGuildNameTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.UpdateMemberRoleByGuildNameResult> UpdateMemberRoleByGuildNameFuture(
                Request.UpdateMemberRoleByGuildNameRequest request
        ) =>
            new UpdateMemberRoleByGuildNameTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.UpdateMemberRoleByGuildNameResult> UpdateMemberRoleByGuildNameAsync(
                Request.UpdateMemberRoleByGuildNameRequest request
        ) =>
            new UpdateMemberRoleByGuildNameTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.UpdateMemberRoleByGuildNameResult>();
    #else
		public UpdateMemberRoleByGuildNameTask UpdateMemberRoleByGuildNameAsync(
                Request.UpdateMemberRoleByGuildNameRequest request
        )
		{
			return new UpdateMemberRoleByGuildNameTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.UpdateMemberRoleByGuildNameResult> UpdateMemberRoleByGuildNameAsync(
                Request.UpdateMemberRoleByGuildNameRequest request
        ) =>
            new UpdateMemberRoleByGuildNameTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class BatchUpdateMemberRoleTask : Gs2RestSessionTask<BatchUpdateMemberRoleRequest, BatchUpdateMemberRoleResult>
        {
            public BatchUpdateMemberRoleTask(IGs2Session session, RestSessionRequestFactory factory, BatchUpdateMemberRoleRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(BatchUpdateMemberRoleRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "guild")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/guild/{guildModelName}/me/batch/member/role";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{guildModelName}", !string.IsNullOrEmpty(request.GuildModelName) ? request.GuildModelName.ToString() : "null");

                var sessionRequest = Factory.Put(url);

                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);
                jsonWriter.WriteObjectStart();
                if (request.Members != null)
                {
                    jsonWriter.WritePropertyName("members");
                    jsonWriter.WriteArrayStart();
                    foreach(var item in request.Members)
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
		public IEnumerator BatchUpdateMemberRole(
                Request.BatchUpdateMemberRoleRequest request,
                UnityAction<AsyncResult<Result.BatchUpdateMemberRoleResult>> callback
        ) =>
            new BatchUpdateMemberRoleTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.BatchUpdateMemberRoleResult> BatchUpdateMemberRoleFuture(
                Request.BatchUpdateMemberRoleRequest request
        ) =>
            new BatchUpdateMemberRoleTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.BatchUpdateMemberRoleResult> BatchUpdateMemberRoleAsync(
                Request.BatchUpdateMemberRoleRequest request
        ) =>
            new BatchUpdateMemberRoleTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.BatchUpdateMemberRoleResult>();
    #else
		public BatchUpdateMemberRoleTask BatchUpdateMemberRoleAsync(
                Request.BatchUpdateMemberRoleRequest request
        )
		{
			return new BatchUpdateMemberRoleTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.BatchUpdateMemberRoleResult> BatchUpdateMemberRoleAsync(
                Request.BatchUpdateMemberRoleRequest request
        ) =>
            new BatchUpdateMemberRoleTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class BatchUpdateMemberRoleByGuildNameTask : Gs2RestSessionTask<BatchUpdateMemberRoleByGuildNameRequest, BatchUpdateMemberRoleByGuildNameResult>
        {
            public BatchUpdateMemberRoleByGuildNameTask(IGs2Session session, RestSessionRequestFactory factory, BatchUpdateMemberRoleByGuildNameRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(BatchUpdateMemberRoleByGuildNameRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "guild")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/guild/{guildModelName}/{guildName}/batch/member/role";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{guildModelName}", !string.IsNullOrEmpty(request.GuildModelName) ? request.GuildModelName.ToString() : "null");
                url = url.Replace("{guildName}", !string.IsNullOrEmpty(request.GuildName) ? request.GuildName.ToString() : "null");

                var sessionRequest = Factory.Put(url);

                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);
                jsonWriter.WriteObjectStart();
                if (request.Members != null)
                {
                    jsonWriter.WritePropertyName("members");
                    jsonWriter.WriteArrayStart();
                    foreach(var item in request.Members)
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
                if (request.DryRun)
                {
                    sessionRequest.AddHeader("X-GS2-DRY-RUN", "true");
                }

                AddHeader(
                    Session.Credential,
                    sessionRequest
                );

                return sessionRequest;
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator BatchUpdateMemberRoleByGuildName(
                Request.BatchUpdateMemberRoleByGuildNameRequest request,
                UnityAction<AsyncResult<Result.BatchUpdateMemberRoleByGuildNameResult>> callback
        ) =>
            new BatchUpdateMemberRoleByGuildNameTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.BatchUpdateMemberRoleByGuildNameResult> BatchUpdateMemberRoleByGuildNameFuture(
                Request.BatchUpdateMemberRoleByGuildNameRequest request
        ) =>
            new BatchUpdateMemberRoleByGuildNameTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.BatchUpdateMemberRoleByGuildNameResult> BatchUpdateMemberRoleByGuildNameAsync(
                Request.BatchUpdateMemberRoleByGuildNameRequest request
        ) =>
            new BatchUpdateMemberRoleByGuildNameTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.BatchUpdateMemberRoleByGuildNameResult>();
    #else
		public BatchUpdateMemberRoleByGuildNameTask BatchUpdateMemberRoleByGuildNameAsync(
                Request.BatchUpdateMemberRoleByGuildNameRequest request
        )
		{
			return new BatchUpdateMemberRoleByGuildNameTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.BatchUpdateMemberRoleByGuildNameResult> BatchUpdateMemberRoleByGuildNameAsync(
                Request.BatchUpdateMemberRoleByGuildNameRequest request
        ) =>
            new BatchUpdateMemberRoleByGuildNameTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class DeleteGuildTask : Gs2RestSessionTask<DeleteGuildRequest, DeleteGuildResult>
        {
            public DeleteGuildTask(IGs2Session session, RestSessionRequestFactory factory, DeleteGuildRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DeleteGuildRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "guild")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/guild/{guildModelName}/me";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{guildModelName}", !string.IsNullOrEmpty(request.GuildModelName) ? request.GuildModelName.ToString() : "null");

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
		public IEnumerator DeleteGuild(
                Request.DeleteGuildRequest request,
                UnityAction<AsyncResult<Result.DeleteGuildResult>> callback
        ) =>
            new DeleteGuildTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.DeleteGuildResult> DeleteGuildFuture(
                Request.DeleteGuildRequest request
        ) =>
            new DeleteGuildTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.DeleteGuildResult> DeleteGuildAsync(
                Request.DeleteGuildRequest request
        ) =>
            new DeleteGuildTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.DeleteGuildResult>();
    #else
		public DeleteGuildTask DeleteGuildAsync(
                Request.DeleteGuildRequest request
        )
		{
			return new DeleteGuildTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.DeleteGuildResult> DeleteGuildAsync(
                Request.DeleteGuildRequest request
        ) =>
            new DeleteGuildTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class DeleteGuildByGuildNameTask : Gs2RestSessionTask<DeleteGuildByGuildNameRequest, DeleteGuildByGuildNameResult>
        {
            public DeleteGuildByGuildNameTask(IGs2Session session, RestSessionRequestFactory factory, DeleteGuildByGuildNameRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DeleteGuildByGuildNameRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "guild")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/guild/{guildModelName}/{guildName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{guildModelName}", !string.IsNullOrEmpty(request.GuildModelName) ? request.GuildModelName.ToString() : "null");
                url = url.Replace("{guildName}", !string.IsNullOrEmpty(request.GuildName) ? request.GuildName.ToString() : "null");

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
		public IEnumerator DeleteGuildByGuildName(
                Request.DeleteGuildByGuildNameRequest request,
                UnityAction<AsyncResult<Result.DeleteGuildByGuildNameResult>> callback
        ) =>
            new DeleteGuildByGuildNameTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.DeleteGuildByGuildNameResult> DeleteGuildByGuildNameFuture(
                Request.DeleteGuildByGuildNameRequest request
        ) =>
            new DeleteGuildByGuildNameTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.DeleteGuildByGuildNameResult> DeleteGuildByGuildNameAsync(
                Request.DeleteGuildByGuildNameRequest request
        ) =>
            new DeleteGuildByGuildNameTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.DeleteGuildByGuildNameResult>();
    #else
		public DeleteGuildByGuildNameTask DeleteGuildByGuildNameAsync(
                Request.DeleteGuildByGuildNameRequest request
        )
		{
			return new DeleteGuildByGuildNameTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.DeleteGuildByGuildNameResult> DeleteGuildByGuildNameAsync(
                Request.DeleteGuildByGuildNameRequest request
        ) =>
            new DeleteGuildByGuildNameTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class IncreaseMaximumCurrentMaximumMemberCountByGuildNameTask : Gs2RestSessionTask<IncreaseMaximumCurrentMaximumMemberCountByGuildNameRequest, IncreaseMaximumCurrentMaximumMemberCountByGuildNameResult>
        {
            public IncreaseMaximumCurrentMaximumMemberCountByGuildNameTask(IGs2Session session, RestSessionRequestFactory factory, IncreaseMaximumCurrentMaximumMemberCountByGuildNameRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(IncreaseMaximumCurrentMaximumMemberCountByGuildNameRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "guild")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/guild/{guildModelName}/{guildName}/currentMaximumMemberCount/increase";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{guildModelName}", !string.IsNullOrEmpty(request.GuildModelName) ? request.GuildModelName.ToString() : "null");
                url = url.Replace("{guildName}", !string.IsNullOrEmpty(request.GuildName) ? request.GuildName.ToString() : "null");

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
                if (request.DryRun)
                {
                    sessionRequest.AddHeader("X-GS2-DRY-RUN", "true");
                }

                AddHeader(
                    Session.Credential,
                    sessionRequest
                );

                return sessionRequest;
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator IncreaseMaximumCurrentMaximumMemberCountByGuildName(
                Request.IncreaseMaximumCurrentMaximumMemberCountByGuildNameRequest request,
                UnityAction<AsyncResult<Result.IncreaseMaximumCurrentMaximumMemberCountByGuildNameResult>> callback
        ) =>
            new IncreaseMaximumCurrentMaximumMemberCountByGuildNameTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.IncreaseMaximumCurrentMaximumMemberCountByGuildNameResult> IncreaseMaximumCurrentMaximumMemberCountByGuildNameFuture(
                Request.IncreaseMaximumCurrentMaximumMemberCountByGuildNameRequest request
        ) =>
            new IncreaseMaximumCurrentMaximumMemberCountByGuildNameTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.IncreaseMaximumCurrentMaximumMemberCountByGuildNameResult> IncreaseMaximumCurrentMaximumMemberCountByGuildNameAsync(
                Request.IncreaseMaximumCurrentMaximumMemberCountByGuildNameRequest request
        ) =>
            new IncreaseMaximumCurrentMaximumMemberCountByGuildNameTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.IncreaseMaximumCurrentMaximumMemberCountByGuildNameResult>();
    #else
		public IncreaseMaximumCurrentMaximumMemberCountByGuildNameTask IncreaseMaximumCurrentMaximumMemberCountByGuildNameAsync(
                Request.IncreaseMaximumCurrentMaximumMemberCountByGuildNameRequest request
        )
		{
			return new IncreaseMaximumCurrentMaximumMemberCountByGuildNameTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.IncreaseMaximumCurrentMaximumMemberCountByGuildNameResult> IncreaseMaximumCurrentMaximumMemberCountByGuildNameAsync(
                Request.IncreaseMaximumCurrentMaximumMemberCountByGuildNameRequest request
        ) =>
            new IncreaseMaximumCurrentMaximumMemberCountByGuildNameTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class DecreaseMaximumCurrentMaximumMemberCountTask : Gs2RestSessionTask<DecreaseMaximumCurrentMaximumMemberCountRequest, DecreaseMaximumCurrentMaximumMemberCountResult>
        {
            public DecreaseMaximumCurrentMaximumMemberCountTask(IGs2Session session, RestSessionRequestFactory factory, DecreaseMaximumCurrentMaximumMemberCountRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DecreaseMaximumCurrentMaximumMemberCountRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "guild")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/guild/{guildModelName}/me/currentMaximumMemberCount/decrease";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{guildModelName}", !string.IsNullOrEmpty(request.GuildModelName) ? request.GuildModelName.ToString() : "null");

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
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator DecreaseMaximumCurrentMaximumMemberCount(
                Request.DecreaseMaximumCurrentMaximumMemberCountRequest request,
                UnityAction<AsyncResult<Result.DecreaseMaximumCurrentMaximumMemberCountResult>> callback
        ) =>
            new DecreaseMaximumCurrentMaximumMemberCountTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.DecreaseMaximumCurrentMaximumMemberCountResult> DecreaseMaximumCurrentMaximumMemberCountFuture(
                Request.DecreaseMaximumCurrentMaximumMemberCountRequest request
        ) =>
            new DecreaseMaximumCurrentMaximumMemberCountTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.DecreaseMaximumCurrentMaximumMemberCountResult> DecreaseMaximumCurrentMaximumMemberCountAsync(
                Request.DecreaseMaximumCurrentMaximumMemberCountRequest request
        ) =>
            new DecreaseMaximumCurrentMaximumMemberCountTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.DecreaseMaximumCurrentMaximumMemberCountResult>();
    #else
		public DecreaseMaximumCurrentMaximumMemberCountTask DecreaseMaximumCurrentMaximumMemberCountAsync(
                Request.DecreaseMaximumCurrentMaximumMemberCountRequest request
        )
		{
			return new DecreaseMaximumCurrentMaximumMemberCountTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.DecreaseMaximumCurrentMaximumMemberCountResult> DecreaseMaximumCurrentMaximumMemberCountAsync(
                Request.DecreaseMaximumCurrentMaximumMemberCountRequest request
        ) =>
            new DecreaseMaximumCurrentMaximumMemberCountTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class DecreaseMaximumCurrentMaximumMemberCountByGuildNameTask : Gs2RestSessionTask<DecreaseMaximumCurrentMaximumMemberCountByGuildNameRequest, DecreaseMaximumCurrentMaximumMemberCountByGuildNameResult>
        {
            public DecreaseMaximumCurrentMaximumMemberCountByGuildNameTask(IGs2Session session, RestSessionRequestFactory factory, DecreaseMaximumCurrentMaximumMemberCountByGuildNameRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DecreaseMaximumCurrentMaximumMemberCountByGuildNameRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "guild")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/guild/{guildModelName}/{guildName}/currentMaximumMemberCount/decrease";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{guildModelName}", !string.IsNullOrEmpty(request.GuildModelName) ? request.GuildModelName.ToString() : "null");
                url = url.Replace("{guildName}", !string.IsNullOrEmpty(request.GuildName) ? request.GuildName.ToString() : "null");

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
                if (request.DryRun)
                {
                    sessionRequest.AddHeader("X-GS2-DRY-RUN", "true");
                }

                AddHeader(
                    Session.Credential,
                    sessionRequest
                );

                return sessionRequest;
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator DecreaseMaximumCurrentMaximumMemberCountByGuildName(
                Request.DecreaseMaximumCurrentMaximumMemberCountByGuildNameRequest request,
                UnityAction<AsyncResult<Result.DecreaseMaximumCurrentMaximumMemberCountByGuildNameResult>> callback
        ) =>
            new DecreaseMaximumCurrentMaximumMemberCountByGuildNameTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.DecreaseMaximumCurrentMaximumMemberCountByGuildNameResult> DecreaseMaximumCurrentMaximumMemberCountByGuildNameFuture(
                Request.DecreaseMaximumCurrentMaximumMemberCountByGuildNameRequest request
        ) =>
            new DecreaseMaximumCurrentMaximumMemberCountByGuildNameTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.DecreaseMaximumCurrentMaximumMemberCountByGuildNameResult> DecreaseMaximumCurrentMaximumMemberCountByGuildNameAsync(
                Request.DecreaseMaximumCurrentMaximumMemberCountByGuildNameRequest request
        ) =>
            new DecreaseMaximumCurrentMaximumMemberCountByGuildNameTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.DecreaseMaximumCurrentMaximumMemberCountByGuildNameResult>();
    #else
		public DecreaseMaximumCurrentMaximumMemberCountByGuildNameTask DecreaseMaximumCurrentMaximumMemberCountByGuildNameAsync(
                Request.DecreaseMaximumCurrentMaximumMemberCountByGuildNameRequest request
        )
		{
			return new DecreaseMaximumCurrentMaximumMemberCountByGuildNameTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.DecreaseMaximumCurrentMaximumMemberCountByGuildNameResult> DecreaseMaximumCurrentMaximumMemberCountByGuildNameAsync(
                Request.DecreaseMaximumCurrentMaximumMemberCountByGuildNameRequest request
        ) =>
            new DecreaseMaximumCurrentMaximumMemberCountByGuildNameTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class VerifyCurrentMaximumMemberCountTask : Gs2RestSessionTask<VerifyCurrentMaximumMemberCountRequest, VerifyCurrentMaximumMemberCountResult>
        {
            public VerifyCurrentMaximumMemberCountTask(IGs2Session session, RestSessionRequestFactory factory, VerifyCurrentMaximumMemberCountRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(VerifyCurrentMaximumMemberCountRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "guild")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/guild/{guildModelName}/me/currentMaximumMemberCount/verify";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{guildModelName}", !string.IsNullOrEmpty(request.GuildModelName) ? request.GuildModelName.ToString() : "null");

                var sessionRequest = Factory.Post(url);

                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);
                jsonWriter.WriteObjectStart();
                if (request.VerifyType != null)
                {
                    jsonWriter.WritePropertyName("verifyType");
                    jsonWriter.Write(request.VerifyType);
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
		public IEnumerator VerifyCurrentMaximumMemberCount(
                Request.VerifyCurrentMaximumMemberCountRequest request,
                UnityAction<AsyncResult<Result.VerifyCurrentMaximumMemberCountResult>> callback
        ) =>
            new VerifyCurrentMaximumMemberCountTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.VerifyCurrentMaximumMemberCountResult> VerifyCurrentMaximumMemberCountFuture(
                Request.VerifyCurrentMaximumMemberCountRequest request
        ) =>
            new VerifyCurrentMaximumMemberCountTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.VerifyCurrentMaximumMemberCountResult> VerifyCurrentMaximumMemberCountAsync(
                Request.VerifyCurrentMaximumMemberCountRequest request
        ) =>
            new VerifyCurrentMaximumMemberCountTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.VerifyCurrentMaximumMemberCountResult>();
    #else
		public VerifyCurrentMaximumMemberCountTask VerifyCurrentMaximumMemberCountAsync(
                Request.VerifyCurrentMaximumMemberCountRequest request
        )
		{
			return new VerifyCurrentMaximumMemberCountTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.VerifyCurrentMaximumMemberCountResult> VerifyCurrentMaximumMemberCountAsync(
                Request.VerifyCurrentMaximumMemberCountRequest request
        ) =>
            new VerifyCurrentMaximumMemberCountTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class VerifyCurrentMaximumMemberCountByGuildNameTask : Gs2RestSessionTask<VerifyCurrentMaximumMemberCountByGuildNameRequest, VerifyCurrentMaximumMemberCountByGuildNameResult>
        {
            public VerifyCurrentMaximumMemberCountByGuildNameTask(IGs2Session session, RestSessionRequestFactory factory, VerifyCurrentMaximumMemberCountByGuildNameRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(VerifyCurrentMaximumMemberCountByGuildNameRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "guild")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/guild/{guildModelName}/{guildName}/currentMaximumMemberCount/verify";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{guildModelName}", !string.IsNullOrEmpty(request.GuildModelName) ? request.GuildModelName.ToString() : "null");
                url = url.Replace("{guildName}", !string.IsNullOrEmpty(request.GuildName) ? request.GuildName.ToString() : "null");

                var sessionRequest = Factory.Post(url);

                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);
                jsonWriter.WriteObjectStart();
                if (request.VerifyType != null)
                {
                    jsonWriter.WritePropertyName("verifyType");
                    jsonWriter.Write(request.VerifyType);
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
                if (request.DryRun)
                {
                    sessionRequest.AddHeader("X-GS2-DRY-RUN", "true");
                }

                AddHeader(
                    Session.Credential,
                    sessionRequest
                );

                return sessionRequest;
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator VerifyCurrentMaximumMemberCountByGuildName(
                Request.VerifyCurrentMaximumMemberCountByGuildNameRequest request,
                UnityAction<AsyncResult<Result.VerifyCurrentMaximumMemberCountByGuildNameResult>> callback
        ) =>
            new VerifyCurrentMaximumMemberCountByGuildNameTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.VerifyCurrentMaximumMemberCountByGuildNameResult> VerifyCurrentMaximumMemberCountByGuildNameFuture(
                Request.VerifyCurrentMaximumMemberCountByGuildNameRequest request
        ) =>
            new VerifyCurrentMaximumMemberCountByGuildNameTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.VerifyCurrentMaximumMemberCountByGuildNameResult> VerifyCurrentMaximumMemberCountByGuildNameAsync(
                Request.VerifyCurrentMaximumMemberCountByGuildNameRequest request
        ) =>
            new VerifyCurrentMaximumMemberCountByGuildNameTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.VerifyCurrentMaximumMemberCountByGuildNameResult>();
    #else
		public VerifyCurrentMaximumMemberCountByGuildNameTask VerifyCurrentMaximumMemberCountByGuildNameAsync(
                Request.VerifyCurrentMaximumMemberCountByGuildNameRequest request
        )
		{
			return new VerifyCurrentMaximumMemberCountByGuildNameTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.VerifyCurrentMaximumMemberCountByGuildNameResult> VerifyCurrentMaximumMemberCountByGuildNameAsync(
                Request.VerifyCurrentMaximumMemberCountByGuildNameRequest request
        ) =>
            new VerifyCurrentMaximumMemberCountByGuildNameTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class VerifyIncludeMemberTask : Gs2RestSessionTask<VerifyIncludeMemberRequest, VerifyIncludeMemberResult>
        {
            public VerifyIncludeMemberTask(IGs2Session session, RestSessionRequestFactory factory, VerifyIncludeMemberRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(VerifyIncludeMemberRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "guild")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/guild/{guildModelName}/{guildName}/member/me/verify";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{guildModelName}", !string.IsNullOrEmpty(request.GuildModelName) ? request.GuildModelName.ToString() : "null");
                url = url.Replace("{guildName}", !string.IsNullOrEmpty(request.GuildName) ? request.GuildName.ToString() : "null");

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
		public IEnumerator VerifyIncludeMember(
                Request.VerifyIncludeMemberRequest request,
                UnityAction<AsyncResult<Result.VerifyIncludeMemberResult>> callback
        ) =>
            new VerifyIncludeMemberTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.VerifyIncludeMemberResult> VerifyIncludeMemberFuture(
                Request.VerifyIncludeMemberRequest request
        ) =>
            new VerifyIncludeMemberTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.VerifyIncludeMemberResult> VerifyIncludeMemberAsync(
                Request.VerifyIncludeMemberRequest request
        ) =>
            new VerifyIncludeMemberTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.VerifyIncludeMemberResult>();
    #else
		public VerifyIncludeMemberTask VerifyIncludeMemberAsync(
                Request.VerifyIncludeMemberRequest request
        )
		{
			return new VerifyIncludeMemberTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.VerifyIncludeMemberResult> VerifyIncludeMemberAsync(
                Request.VerifyIncludeMemberRequest request
        ) =>
            new VerifyIncludeMemberTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class VerifyIncludeMemberByUserIdTask : Gs2RestSessionTask<VerifyIncludeMemberByUserIdRequest, VerifyIncludeMemberByUserIdResult>
        {
            public VerifyIncludeMemberByUserIdTask(IGs2Session session, RestSessionRequestFactory factory, VerifyIncludeMemberByUserIdRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(VerifyIncludeMemberByUserIdRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "guild")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/guild/{guildModelName}/{guildName}/member/{userId}/verify";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{guildModelName}", !string.IsNullOrEmpty(request.GuildModelName) ? request.GuildModelName.ToString() : "null");
                url = url.Replace("{guildName}", !string.IsNullOrEmpty(request.GuildName) ? request.GuildName.ToString() : "null");
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
		public IEnumerator VerifyIncludeMemberByUserId(
                Request.VerifyIncludeMemberByUserIdRequest request,
                UnityAction<AsyncResult<Result.VerifyIncludeMemberByUserIdResult>> callback
        ) =>
            new VerifyIncludeMemberByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.VerifyIncludeMemberByUserIdResult> VerifyIncludeMemberByUserIdFuture(
                Request.VerifyIncludeMemberByUserIdRequest request
        ) =>
            new VerifyIncludeMemberByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.VerifyIncludeMemberByUserIdResult> VerifyIncludeMemberByUserIdAsync(
                Request.VerifyIncludeMemberByUserIdRequest request
        ) =>
            new VerifyIncludeMemberByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.VerifyIncludeMemberByUserIdResult>();
    #else
		public VerifyIncludeMemberByUserIdTask VerifyIncludeMemberByUserIdAsync(
                Request.VerifyIncludeMemberByUserIdRequest request
        )
		{
			return new VerifyIncludeMemberByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.VerifyIncludeMemberByUserIdResult> VerifyIncludeMemberByUserIdAsync(
                Request.VerifyIncludeMemberByUserIdRequest request
        ) =>
            new VerifyIncludeMemberByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class SetMaximumCurrentMaximumMemberCountByGuildNameTask : Gs2RestSessionTask<SetMaximumCurrentMaximumMemberCountByGuildNameRequest, SetMaximumCurrentMaximumMemberCountByGuildNameResult>
        {
            public SetMaximumCurrentMaximumMemberCountByGuildNameTask(IGs2Session session, RestSessionRequestFactory factory, SetMaximumCurrentMaximumMemberCountByGuildNameRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(SetMaximumCurrentMaximumMemberCountByGuildNameRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "guild")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/guild/{guildModelName}/{guildName}/currentMaximumMemberCount";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{guildName}", !string.IsNullOrEmpty(request.GuildName) ? request.GuildName.ToString() : "null");
                url = url.Replace("{guildModelName}", !string.IsNullOrEmpty(request.GuildModelName) ? request.GuildModelName.ToString() : "null");

                var sessionRequest = Factory.Put(url);

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
                if (request.DryRun)
                {
                    sessionRequest.AddHeader("X-GS2-DRY-RUN", "true");
                }

                AddHeader(
                    Session.Credential,
                    sessionRequest
                );

                return sessionRequest;
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator SetMaximumCurrentMaximumMemberCountByGuildName(
                Request.SetMaximumCurrentMaximumMemberCountByGuildNameRequest request,
                UnityAction<AsyncResult<Result.SetMaximumCurrentMaximumMemberCountByGuildNameResult>> callback
        ) =>
            new SetMaximumCurrentMaximumMemberCountByGuildNameTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.SetMaximumCurrentMaximumMemberCountByGuildNameResult> SetMaximumCurrentMaximumMemberCountByGuildNameFuture(
                Request.SetMaximumCurrentMaximumMemberCountByGuildNameRequest request
        ) =>
            new SetMaximumCurrentMaximumMemberCountByGuildNameTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.SetMaximumCurrentMaximumMemberCountByGuildNameResult> SetMaximumCurrentMaximumMemberCountByGuildNameAsync(
                Request.SetMaximumCurrentMaximumMemberCountByGuildNameRequest request
        ) =>
            new SetMaximumCurrentMaximumMemberCountByGuildNameTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.SetMaximumCurrentMaximumMemberCountByGuildNameResult>();
    #else
		public SetMaximumCurrentMaximumMemberCountByGuildNameTask SetMaximumCurrentMaximumMemberCountByGuildNameAsync(
                Request.SetMaximumCurrentMaximumMemberCountByGuildNameRequest request
        )
		{
			return new SetMaximumCurrentMaximumMemberCountByGuildNameTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.SetMaximumCurrentMaximumMemberCountByGuildNameResult> SetMaximumCurrentMaximumMemberCountByGuildNameAsync(
                Request.SetMaximumCurrentMaximumMemberCountByGuildNameRequest request
        ) =>
            new SetMaximumCurrentMaximumMemberCountByGuildNameTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class AssumeTask : Gs2RestSessionTask<AssumeRequest, AssumeResult>
        {
            public AssumeTask(IGs2Session session, RestSessionRequestFactory factory, AssumeRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(AssumeRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "guild")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/me/guild/{guildModelName}/{guildName}/assume";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{guildModelName}", !string.IsNullOrEmpty(request.GuildModelName) ? request.GuildModelName.ToString() : "null");
                url = url.Replace("{guildName}", !string.IsNullOrEmpty(request.GuildName) ? request.GuildName.ToString() : "null");

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

            public override void OnError(Gs2.Core.Exception.Gs2Exception error)
            {
                if (error.Errors.Count(v => v.code == "guild.member.notFound") > 0) {
                    base.OnError(new Exception.NotIncludedGuildMemberException(error));
                }
                else {
                    base.OnError(error);
                }
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator Assume(
                Request.AssumeRequest request,
                UnityAction<AsyncResult<Result.AssumeResult>> callback
        ) =>
            new AssumeTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.AssumeResult> AssumeFuture(
                Request.AssumeRequest request
        ) =>
            new AssumeTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.AssumeResult> AssumeAsync(
                Request.AssumeRequest request
        ) =>
            new AssumeTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.AssumeResult>();
    #else
		public AssumeTask AssumeAsync(
                Request.AssumeRequest request
        )
		{
			return new AssumeTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.AssumeResult> AssumeAsync(
                Request.AssumeRequest request
        ) =>
            new AssumeTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class AssumeByUserIdTask : Gs2RestSessionTask<AssumeByUserIdRequest, AssumeByUserIdResult>
        {
            public AssumeByUserIdTask(IGs2Session session, RestSessionRequestFactory factory, AssumeByUserIdRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(AssumeByUserIdRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "guild")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/{userId}/guild/{guildModelName}/{guildName}/assume";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{userId}", !string.IsNullOrEmpty(request.UserId) ? request.UserId.ToString() : "null");
                url = url.Replace("{guildModelName}", !string.IsNullOrEmpty(request.GuildModelName) ? request.GuildModelName.ToString() : "null");
                url = url.Replace("{guildName}", !string.IsNullOrEmpty(request.GuildName) ? request.GuildName.ToString() : "null");

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

            public override void OnError(Gs2.Core.Exception.Gs2Exception error)
            {
                if (error.Errors.Count(v => v.code == "guild.member.notFound") > 0) {
                    base.OnError(new Exception.NotIncludedGuildMemberException(error));
                }
                else {
                    base.OnError(error);
                }
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator AssumeByUserId(
                Request.AssumeByUserIdRequest request,
                UnityAction<AsyncResult<Result.AssumeByUserIdResult>> callback
        ) =>
            new AssumeByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.AssumeByUserIdResult> AssumeByUserIdFuture(
                Request.AssumeByUserIdRequest request
        ) =>
            new AssumeByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.AssumeByUserIdResult> AssumeByUserIdAsync(
                Request.AssumeByUserIdRequest request
        ) =>
            new AssumeByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.AssumeByUserIdResult>();
    #else
		public AssumeByUserIdTask AssumeByUserIdAsync(
                Request.AssumeByUserIdRequest request
        )
		{
			return new AssumeByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.AssumeByUserIdResult> AssumeByUserIdAsync(
                Request.AssumeByUserIdRequest request
        ) =>
            new AssumeByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class IncreaseMaximumCurrentMaximumMemberCountByStampSheetTask : Gs2RestSessionTask<IncreaseMaximumCurrentMaximumMemberCountByStampSheetRequest, IncreaseMaximumCurrentMaximumMemberCountByStampSheetResult>
        {
            public IncreaseMaximumCurrentMaximumMemberCountByStampSheetTask(IGs2Session session, RestSessionRequestFactory factory, IncreaseMaximumCurrentMaximumMemberCountByStampSheetRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(IncreaseMaximumCurrentMaximumMemberCountByStampSheetRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "guild")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/stamp/guild/currentMaximumMemberCount/add";

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
		public IEnumerator IncreaseMaximumCurrentMaximumMemberCountByStampSheet(
                Request.IncreaseMaximumCurrentMaximumMemberCountByStampSheetRequest request,
                UnityAction<AsyncResult<Result.IncreaseMaximumCurrentMaximumMemberCountByStampSheetResult>> callback
        ) =>
            new IncreaseMaximumCurrentMaximumMemberCountByStampSheetTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.IncreaseMaximumCurrentMaximumMemberCountByStampSheetResult> IncreaseMaximumCurrentMaximumMemberCountByStampSheetFuture(
                Request.IncreaseMaximumCurrentMaximumMemberCountByStampSheetRequest request
        ) =>
            new IncreaseMaximumCurrentMaximumMemberCountByStampSheetTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.IncreaseMaximumCurrentMaximumMemberCountByStampSheetResult> IncreaseMaximumCurrentMaximumMemberCountByStampSheetAsync(
                Request.IncreaseMaximumCurrentMaximumMemberCountByStampSheetRequest request
        ) =>
            new IncreaseMaximumCurrentMaximumMemberCountByStampSheetTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.IncreaseMaximumCurrentMaximumMemberCountByStampSheetResult>();
    #else
		public IncreaseMaximumCurrentMaximumMemberCountByStampSheetTask IncreaseMaximumCurrentMaximumMemberCountByStampSheetAsync(
                Request.IncreaseMaximumCurrentMaximumMemberCountByStampSheetRequest request
        )
		{
			return new IncreaseMaximumCurrentMaximumMemberCountByStampSheetTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.IncreaseMaximumCurrentMaximumMemberCountByStampSheetResult> IncreaseMaximumCurrentMaximumMemberCountByStampSheetAsync(
                Request.IncreaseMaximumCurrentMaximumMemberCountByStampSheetRequest request
        ) =>
            new IncreaseMaximumCurrentMaximumMemberCountByStampSheetTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class DecreaseMaximumCurrentMaximumMemberCountByStampTaskTask : Gs2RestSessionTask<DecreaseMaximumCurrentMaximumMemberCountByStampTaskRequest, DecreaseMaximumCurrentMaximumMemberCountByStampTaskResult>
        {
            public DecreaseMaximumCurrentMaximumMemberCountByStampTaskTask(IGs2Session session, RestSessionRequestFactory factory, DecreaseMaximumCurrentMaximumMemberCountByStampTaskRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DecreaseMaximumCurrentMaximumMemberCountByStampTaskRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "guild")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/stamp/guild/currentMaximumMemberCount/sub";

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
		public IEnumerator DecreaseMaximumCurrentMaximumMemberCountByStampTask(
                Request.DecreaseMaximumCurrentMaximumMemberCountByStampTaskRequest request,
                UnityAction<AsyncResult<Result.DecreaseMaximumCurrentMaximumMemberCountByStampTaskResult>> callback
        ) =>
            new DecreaseMaximumCurrentMaximumMemberCountByStampTaskTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.DecreaseMaximumCurrentMaximumMemberCountByStampTaskResult> DecreaseMaximumCurrentMaximumMemberCountByStampTaskFuture(
                Request.DecreaseMaximumCurrentMaximumMemberCountByStampTaskRequest request
        ) =>
            new DecreaseMaximumCurrentMaximumMemberCountByStampTaskTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.DecreaseMaximumCurrentMaximumMemberCountByStampTaskResult> DecreaseMaximumCurrentMaximumMemberCountByStampTaskAsync(
                Request.DecreaseMaximumCurrentMaximumMemberCountByStampTaskRequest request
        ) =>
            new DecreaseMaximumCurrentMaximumMemberCountByStampTaskTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.DecreaseMaximumCurrentMaximumMemberCountByStampTaskResult>();
    #else
		public DecreaseMaximumCurrentMaximumMemberCountByStampTaskTask DecreaseMaximumCurrentMaximumMemberCountByStampTaskAsync(
                Request.DecreaseMaximumCurrentMaximumMemberCountByStampTaskRequest request
        )
		{
			return new DecreaseMaximumCurrentMaximumMemberCountByStampTaskTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.DecreaseMaximumCurrentMaximumMemberCountByStampTaskResult> DecreaseMaximumCurrentMaximumMemberCountByStampTaskAsync(
                Request.DecreaseMaximumCurrentMaximumMemberCountByStampTaskRequest request
        ) =>
            new DecreaseMaximumCurrentMaximumMemberCountByStampTaskTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class SetMaximumCurrentMaximumMemberCountByStampSheetTask : Gs2RestSessionTask<SetMaximumCurrentMaximumMemberCountByStampSheetRequest, SetMaximumCurrentMaximumMemberCountByStampSheetResult>
        {
            public SetMaximumCurrentMaximumMemberCountByStampSheetTask(IGs2Session session, RestSessionRequestFactory factory, SetMaximumCurrentMaximumMemberCountByStampSheetRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(SetMaximumCurrentMaximumMemberCountByStampSheetRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "guild")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/stamp/guild/currentMaximumMemberCount/set";

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
		public IEnumerator SetMaximumCurrentMaximumMemberCountByStampSheet(
                Request.SetMaximumCurrentMaximumMemberCountByStampSheetRequest request,
                UnityAction<AsyncResult<Result.SetMaximumCurrentMaximumMemberCountByStampSheetResult>> callback
        ) =>
            new SetMaximumCurrentMaximumMemberCountByStampSheetTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.SetMaximumCurrentMaximumMemberCountByStampSheetResult> SetMaximumCurrentMaximumMemberCountByStampSheetFuture(
                Request.SetMaximumCurrentMaximumMemberCountByStampSheetRequest request
        ) =>
            new SetMaximumCurrentMaximumMemberCountByStampSheetTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.SetMaximumCurrentMaximumMemberCountByStampSheetResult> SetMaximumCurrentMaximumMemberCountByStampSheetAsync(
                Request.SetMaximumCurrentMaximumMemberCountByStampSheetRequest request
        ) =>
            new SetMaximumCurrentMaximumMemberCountByStampSheetTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.SetMaximumCurrentMaximumMemberCountByStampSheetResult>();
    #else
		public SetMaximumCurrentMaximumMemberCountByStampSheetTask SetMaximumCurrentMaximumMemberCountByStampSheetAsync(
                Request.SetMaximumCurrentMaximumMemberCountByStampSheetRequest request
        )
		{
			return new SetMaximumCurrentMaximumMemberCountByStampSheetTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.SetMaximumCurrentMaximumMemberCountByStampSheetResult> SetMaximumCurrentMaximumMemberCountByStampSheetAsync(
                Request.SetMaximumCurrentMaximumMemberCountByStampSheetRequest request
        ) =>
            new SetMaximumCurrentMaximumMemberCountByStampSheetTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class VerifyCurrentMaximumMemberCountByStampTaskTask : Gs2RestSessionTask<VerifyCurrentMaximumMemberCountByStampTaskRequest, VerifyCurrentMaximumMemberCountByStampTaskResult>
        {
            public VerifyCurrentMaximumMemberCountByStampTaskTask(IGs2Session session, RestSessionRequestFactory factory, VerifyCurrentMaximumMemberCountByStampTaskRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(VerifyCurrentMaximumMemberCountByStampTaskRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "guild")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/stamp/guild/currentMaximumMemberCount/verify";

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
		public IEnumerator VerifyCurrentMaximumMemberCountByStampTask(
                Request.VerifyCurrentMaximumMemberCountByStampTaskRequest request,
                UnityAction<AsyncResult<Result.VerifyCurrentMaximumMemberCountByStampTaskResult>> callback
        ) =>
            new VerifyCurrentMaximumMemberCountByStampTaskTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.VerifyCurrentMaximumMemberCountByStampTaskResult> VerifyCurrentMaximumMemberCountByStampTaskFuture(
                Request.VerifyCurrentMaximumMemberCountByStampTaskRequest request
        ) =>
            new VerifyCurrentMaximumMemberCountByStampTaskTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.VerifyCurrentMaximumMemberCountByStampTaskResult> VerifyCurrentMaximumMemberCountByStampTaskAsync(
                Request.VerifyCurrentMaximumMemberCountByStampTaskRequest request
        ) =>
            new VerifyCurrentMaximumMemberCountByStampTaskTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.VerifyCurrentMaximumMemberCountByStampTaskResult>();
    #else
		public VerifyCurrentMaximumMemberCountByStampTaskTask VerifyCurrentMaximumMemberCountByStampTaskAsync(
                Request.VerifyCurrentMaximumMemberCountByStampTaskRequest request
        )
		{
			return new VerifyCurrentMaximumMemberCountByStampTaskTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.VerifyCurrentMaximumMemberCountByStampTaskResult> VerifyCurrentMaximumMemberCountByStampTaskAsync(
                Request.VerifyCurrentMaximumMemberCountByStampTaskRequest request
        ) =>
            new VerifyCurrentMaximumMemberCountByStampTaskTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class VerifyIncludeMemberByStampTaskTask : Gs2RestSessionTask<VerifyIncludeMemberByStampTaskRequest, VerifyIncludeMemberByStampTaskResult>
        {
            public VerifyIncludeMemberByStampTaskTask(IGs2Session session, RestSessionRequestFactory factory, VerifyIncludeMemberByStampTaskRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(VerifyIncludeMemberByStampTaskRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "guild")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/stamp/guild/member/verify";

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
		public IEnumerator VerifyIncludeMemberByStampTask(
                Request.VerifyIncludeMemberByStampTaskRequest request,
                UnityAction<AsyncResult<Result.VerifyIncludeMemberByStampTaskResult>> callback
        ) =>
            new VerifyIncludeMemberByStampTaskTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.VerifyIncludeMemberByStampTaskResult> VerifyIncludeMemberByStampTaskFuture(
                Request.VerifyIncludeMemberByStampTaskRequest request
        ) =>
            new VerifyIncludeMemberByStampTaskTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.VerifyIncludeMemberByStampTaskResult> VerifyIncludeMemberByStampTaskAsync(
                Request.VerifyIncludeMemberByStampTaskRequest request
        ) =>
            new VerifyIncludeMemberByStampTaskTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.VerifyIncludeMemberByStampTaskResult>();
    #else
		public VerifyIncludeMemberByStampTaskTask VerifyIncludeMemberByStampTaskAsync(
                Request.VerifyIncludeMemberByStampTaskRequest request
        )
		{
			return new VerifyIncludeMemberByStampTaskTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.VerifyIncludeMemberByStampTaskResult> VerifyIncludeMemberByStampTaskAsync(
                Request.VerifyIncludeMemberByStampTaskRequest request
        ) =>
            new VerifyIncludeMemberByStampTaskTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class DescribeJoinedGuildsTask : Gs2RestSessionTask<DescribeJoinedGuildsRequest, DescribeJoinedGuildsResult>
        {
            public DescribeJoinedGuildsTask(IGs2Session session, RestSessionRequestFactory factory, DescribeJoinedGuildsRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DescribeJoinedGuildsRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "guild")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/me/joined";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");

                var sessionRequest = Factory.Get(url);
                if (request.ContextStack != null)
                {
                    sessionRequest.AddQueryString("contextStack", request.ContextStack);
                }
                if (request.GuildModelName != null) {
                    sessionRequest.AddQueryString("guildModelName", $"{request.GuildModelName}");
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
		public IEnumerator DescribeJoinedGuilds(
                Request.DescribeJoinedGuildsRequest request,
                UnityAction<AsyncResult<Result.DescribeJoinedGuildsResult>> callback
        ) =>
            new DescribeJoinedGuildsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.DescribeJoinedGuildsResult> DescribeJoinedGuildsFuture(
                Request.DescribeJoinedGuildsRequest request
        ) =>
            new DescribeJoinedGuildsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.DescribeJoinedGuildsResult> DescribeJoinedGuildsAsync(
                Request.DescribeJoinedGuildsRequest request
        ) =>
            new DescribeJoinedGuildsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.DescribeJoinedGuildsResult>();
    #else
		public DescribeJoinedGuildsTask DescribeJoinedGuildsAsync(
                Request.DescribeJoinedGuildsRequest request
        )
		{
			return new DescribeJoinedGuildsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.DescribeJoinedGuildsResult> DescribeJoinedGuildsAsync(
                Request.DescribeJoinedGuildsRequest request
        ) =>
            new DescribeJoinedGuildsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class DescribeJoinedGuildsByUserIdTask : Gs2RestSessionTask<DescribeJoinedGuildsByUserIdRequest, DescribeJoinedGuildsByUserIdResult>
        {
            public DescribeJoinedGuildsByUserIdTask(IGs2Session session, RestSessionRequestFactory factory, DescribeJoinedGuildsByUserIdRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DescribeJoinedGuildsByUserIdRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "guild")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/{userId}/joined";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{userId}", !string.IsNullOrEmpty(request.UserId) ? request.UserId.ToString() : "null");

                var sessionRequest = Factory.Get(url);
                if (request.ContextStack != null)
                {
                    sessionRequest.AddQueryString("contextStack", request.ContextStack);
                }
                if (request.GuildModelName != null) {
                    sessionRequest.AddQueryString("guildModelName", $"{request.GuildModelName}");
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
		public IEnumerator DescribeJoinedGuildsByUserId(
                Request.DescribeJoinedGuildsByUserIdRequest request,
                UnityAction<AsyncResult<Result.DescribeJoinedGuildsByUserIdResult>> callback
        ) =>
            new DescribeJoinedGuildsByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.DescribeJoinedGuildsByUserIdResult> DescribeJoinedGuildsByUserIdFuture(
                Request.DescribeJoinedGuildsByUserIdRequest request
        ) =>
            new DescribeJoinedGuildsByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.DescribeJoinedGuildsByUserIdResult> DescribeJoinedGuildsByUserIdAsync(
                Request.DescribeJoinedGuildsByUserIdRequest request
        ) =>
            new DescribeJoinedGuildsByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.DescribeJoinedGuildsByUserIdResult>();
    #else
		public DescribeJoinedGuildsByUserIdTask DescribeJoinedGuildsByUserIdAsync(
                Request.DescribeJoinedGuildsByUserIdRequest request
        )
		{
			return new DescribeJoinedGuildsByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.DescribeJoinedGuildsByUserIdResult> DescribeJoinedGuildsByUserIdAsync(
                Request.DescribeJoinedGuildsByUserIdRequest request
        ) =>
            new DescribeJoinedGuildsByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class GetJoinedGuildTask : Gs2RestSessionTask<GetJoinedGuildRequest, GetJoinedGuildResult>
        {
            public GetJoinedGuildTask(IGs2Session session, RestSessionRequestFactory factory, GetJoinedGuildRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(GetJoinedGuildRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "guild")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/me/joined/{guildModelName}/{guildName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{guildModelName}", !string.IsNullOrEmpty(request.GuildModelName) ? request.GuildModelName.ToString() : "null");
                url = url.Replace("{guildName}", !string.IsNullOrEmpty(request.GuildName) ? request.GuildName.ToString() : "null");

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
		public IEnumerator GetJoinedGuild(
                Request.GetJoinedGuildRequest request,
                UnityAction<AsyncResult<Result.GetJoinedGuildResult>> callback
        ) =>
            new GetJoinedGuildTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.GetJoinedGuildResult> GetJoinedGuildFuture(
                Request.GetJoinedGuildRequest request
        ) =>
            new GetJoinedGuildTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.GetJoinedGuildResult> GetJoinedGuildAsync(
                Request.GetJoinedGuildRequest request
        ) =>
            new GetJoinedGuildTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.GetJoinedGuildResult>();
    #else
		public GetJoinedGuildTask GetJoinedGuildAsync(
                Request.GetJoinedGuildRequest request
        )
		{
			return new GetJoinedGuildTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.GetJoinedGuildResult> GetJoinedGuildAsync(
                Request.GetJoinedGuildRequest request
        ) =>
            new GetJoinedGuildTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class GetJoinedGuildByUserIdTask : Gs2RestSessionTask<GetJoinedGuildByUserIdRequest, GetJoinedGuildByUserIdResult>
        {
            public GetJoinedGuildByUserIdTask(IGs2Session session, RestSessionRequestFactory factory, GetJoinedGuildByUserIdRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(GetJoinedGuildByUserIdRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "guild")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/{userId}/joined/{guildModelName}/{guildName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{userId}", !string.IsNullOrEmpty(request.UserId) ? request.UserId.ToString() : "null");
                url = url.Replace("{guildModelName}", !string.IsNullOrEmpty(request.GuildModelName) ? request.GuildModelName.ToString() : "null");
                url = url.Replace("{guildName}", !string.IsNullOrEmpty(request.GuildName) ? request.GuildName.ToString() : "null");

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
		public IEnumerator GetJoinedGuildByUserId(
                Request.GetJoinedGuildByUserIdRequest request,
                UnityAction<AsyncResult<Result.GetJoinedGuildByUserIdResult>> callback
        ) =>
            new GetJoinedGuildByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.GetJoinedGuildByUserIdResult> GetJoinedGuildByUserIdFuture(
                Request.GetJoinedGuildByUserIdRequest request
        ) =>
            new GetJoinedGuildByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.GetJoinedGuildByUserIdResult> GetJoinedGuildByUserIdAsync(
                Request.GetJoinedGuildByUserIdRequest request
        ) =>
            new GetJoinedGuildByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.GetJoinedGuildByUserIdResult>();
    #else
		public GetJoinedGuildByUserIdTask GetJoinedGuildByUserIdAsync(
                Request.GetJoinedGuildByUserIdRequest request
        )
		{
			return new GetJoinedGuildByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.GetJoinedGuildByUserIdResult> GetJoinedGuildByUserIdAsync(
                Request.GetJoinedGuildByUserIdRequest request
        ) =>
            new GetJoinedGuildByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class UpdateMemberMetadataTask : Gs2RestSessionTask<UpdateMemberMetadataRequest, UpdateMemberMetadataResult>
        {
            public UpdateMemberMetadataTask(IGs2Session session, RestSessionRequestFactory factory, UpdateMemberMetadataRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(UpdateMemberMetadataRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "guild")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/guild/{guildModelName}/guild/{guildName}/member/me/metadata";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{guildModelName}", !string.IsNullOrEmpty(request.GuildModelName) ? request.GuildModelName.ToString() : "null");
                url = url.Replace("{guildName}", !string.IsNullOrEmpty(request.GuildName) ? request.GuildName.ToString() : "null");

                var sessionRequest = Factory.Put(url);

                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);
                jsonWriter.WriteObjectStart();
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
		public IEnumerator UpdateMemberMetadata(
                Request.UpdateMemberMetadataRequest request,
                UnityAction<AsyncResult<Result.UpdateMemberMetadataResult>> callback
        ) =>
            new UpdateMemberMetadataTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.UpdateMemberMetadataResult> UpdateMemberMetadataFuture(
                Request.UpdateMemberMetadataRequest request
        ) =>
            new UpdateMemberMetadataTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.UpdateMemberMetadataResult> UpdateMemberMetadataAsync(
                Request.UpdateMemberMetadataRequest request
        ) =>
            new UpdateMemberMetadataTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.UpdateMemberMetadataResult>();
    #else
		public UpdateMemberMetadataTask UpdateMemberMetadataAsync(
                Request.UpdateMemberMetadataRequest request
        )
		{
			return new UpdateMemberMetadataTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.UpdateMemberMetadataResult> UpdateMemberMetadataAsync(
                Request.UpdateMemberMetadataRequest request
        ) =>
            new UpdateMemberMetadataTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class UpdateMemberMetadataByUserIdTask : Gs2RestSessionTask<UpdateMemberMetadataByUserIdRequest, UpdateMemberMetadataByUserIdResult>
        {
            public UpdateMemberMetadataByUserIdTask(IGs2Session session, RestSessionRequestFactory factory, UpdateMemberMetadataByUserIdRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(UpdateMemberMetadataByUserIdRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "guild")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/guild/{guildModelName}/guild/{guildName}/member/{userId}/metadata";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{guildModelName}", !string.IsNullOrEmpty(request.GuildModelName) ? request.GuildModelName.ToString() : "null");
                url = url.Replace("{guildName}", !string.IsNullOrEmpty(request.GuildName) ? request.GuildName.ToString() : "null");
                url = url.Replace("{userId}", !string.IsNullOrEmpty(request.UserId) ? request.UserId.ToString() : "null");

                var sessionRequest = Factory.Put(url);

                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);
                jsonWriter.WriteObjectStart();
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
		public IEnumerator UpdateMemberMetadataByUserId(
                Request.UpdateMemberMetadataByUserIdRequest request,
                UnityAction<AsyncResult<Result.UpdateMemberMetadataByUserIdResult>> callback
        ) =>
            new UpdateMemberMetadataByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.UpdateMemberMetadataByUserIdResult> UpdateMemberMetadataByUserIdFuture(
                Request.UpdateMemberMetadataByUserIdRequest request
        ) =>
            new UpdateMemberMetadataByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.UpdateMemberMetadataByUserIdResult> UpdateMemberMetadataByUserIdAsync(
                Request.UpdateMemberMetadataByUserIdRequest request
        ) =>
            new UpdateMemberMetadataByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.UpdateMemberMetadataByUserIdResult>();
    #else
		public UpdateMemberMetadataByUserIdTask UpdateMemberMetadataByUserIdAsync(
                Request.UpdateMemberMetadataByUserIdRequest request
        )
		{
			return new UpdateMemberMetadataByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.UpdateMemberMetadataByUserIdResult> UpdateMemberMetadataByUserIdAsync(
                Request.UpdateMemberMetadataByUserIdRequest request
        ) =>
            new UpdateMemberMetadataByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class WithdrawalTask : Gs2RestSessionTask<WithdrawalRequest, WithdrawalResult>
        {
            public WithdrawalTask(IGs2Session session, RestSessionRequestFactory factory, WithdrawalRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(WithdrawalRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "guild")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/me/joined/{guildModelName}/{guildName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{guildModelName}", !string.IsNullOrEmpty(request.GuildModelName) ? request.GuildModelName.ToString() : "null");
                url = url.Replace("{guildName}", !string.IsNullOrEmpty(request.GuildName) ? request.GuildName.ToString() : "null");

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

            public override void OnError(Gs2.Core.Exception.Gs2Exception error)
            {
                if (error.Errors.Count(v => v.code == "guild.member.master.require") > 0) {
                    base.OnError(new Exception.GuildMasterRequiredException(error));
                }
                else {
                    base.OnError(error);
                }
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator Withdrawal(
                Request.WithdrawalRequest request,
                UnityAction<AsyncResult<Result.WithdrawalResult>> callback
        ) =>
            new WithdrawalTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.WithdrawalResult> WithdrawalFuture(
                Request.WithdrawalRequest request
        ) =>
            new WithdrawalTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.WithdrawalResult> WithdrawalAsync(
                Request.WithdrawalRequest request
        ) =>
            new WithdrawalTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.WithdrawalResult>();
    #else
		public WithdrawalTask WithdrawalAsync(
                Request.WithdrawalRequest request
        )
		{
			return new WithdrawalTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.WithdrawalResult> WithdrawalAsync(
                Request.WithdrawalRequest request
        ) =>
            new WithdrawalTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class WithdrawalByUserIdTask : Gs2RestSessionTask<WithdrawalByUserIdRequest, WithdrawalByUserIdResult>
        {
            public WithdrawalByUserIdTask(IGs2Session session, RestSessionRequestFactory factory, WithdrawalByUserIdRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(WithdrawalByUserIdRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "guild")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/{userId}/joined/{guildModelName}/{guildName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{userId}", !string.IsNullOrEmpty(request.UserId) ? request.UserId.ToString() : "null");
                url = url.Replace("{guildModelName}", !string.IsNullOrEmpty(request.GuildModelName) ? request.GuildModelName.ToString() : "null");
                url = url.Replace("{guildName}", !string.IsNullOrEmpty(request.GuildName) ? request.GuildName.ToString() : "null");

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

            public override void OnError(Gs2.Core.Exception.Gs2Exception error)
            {
                if (error.Errors.Count(v => v.code == "guild.member.master.require") > 0) {
                    base.OnError(new Exception.GuildMasterRequiredException(error));
                }
                else {
                    base.OnError(error);
                }
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator WithdrawalByUserId(
                Request.WithdrawalByUserIdRequest request,
                UnityAction<AsyncResult<Result.WithdrawalByUserIdResult>> callback
        ) =>
            new WithdrawalByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.WithdrawalByUserIdResult> WithdrawalByUserIdFuture(
                Request.WithdrawalByUserIdRequest request
        ) =>
            new WithdrawalByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.WithdrawalByUserIdResult> WithdrawalByUserIdAsync(
                Request.WithdrawalByUserIdRequest request
        ) =>
            new WithdrawalByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.WithdrawalByUserIdResult>();
    #else
		public WithdrawalByUserIdTask WithdrawalByUserIdAsync(
                Request.WithdrawalByUserIdRequest request
        )
		{
			return new WithdrawalByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.WithdrawalByUserIdResult> WithdrawalByUserIdAsync(
                Request.WithdrawalByUserIdRequest request
        ) =>
            new WithdrawalByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class GetLastGuildMasterActivityTask : Gs2RestSessionTask<GetLastGuildMasterActivityRequest, GetLastGuildMasterActivityResult>
        {
            public GetLastGuildMasterActivityTask(IGs2Session session, RestSessionRequestFactory factory, GetLastGuildMasterActivityRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(GetLastGuildMasterActivityRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "guild")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/guild/{guildModelName}/me/activity/guildMaster/last";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{guildModelName}", !string.IsNullOrEmpty(request.GuildModelName) ? request.GuildModelName.ToString() : "null");

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
		public IEnumerator GetLastGuildMasterActivity(
                Request.GetLastGuildMasterActivityRequest request,
                UnityAction<AsyncResult<Result.GetLastGuildMasterActivityResult>> callback
        ) =>
            new GetLastGuildMasterActivityTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.GetLastGuildMasterActivityResult> GetLastGuildMasterActivityFuture(
                Request.GetLastGuildMasterActivityRequest request
        ) =>
            new GetLastGuildMasterActivityTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.GetLastGuildMasterActivityResult> GetLastGuildMasterActivityAsync(
                Request.GetLastGuildMasterActivityRequest request
        ) =>
            new GetLastGuildMasterActivityTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.GetLastGuildMasterActivityResult>();
    #else
		public GetLastGuildMasterActivityTask GetLastGuildMasterActivityAsync(
                Request.GetLastGuildMasterActivityRequest request
        )
		{
			return new GetLastGuildMasterActivityTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.GetLastGuildMasterActivityResult> GetLastGuildMasterActivityAsync(
                Request.GetLastGuildMasterActivityRequest request
        ) =>
            new GetLastGuildMasterActivityTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class GetLastGuildMasterActivityByGuildNameTask : Gs2RestSessionTask<GetLastGuildMasterActivityByGuildNameRequest, GetLastGuildMasterActivityByGuildNameResult>
        {
            public GetLastGuildMasterActivityByGuildNameTask(IGs2Session session, RestSessionRequestFactory factory, GetLastGuildMasterActivityByGuildNameRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(GetLastGuildMasterActivityByGuildNameRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "guild")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/guild/{guildModelName}/{guildName}/activity/guildMaster/last";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{guildModelName}", !string.IsNullOrEmpty(request.GuildModelName) ? request.GuildModelName.ToString() : "null");
                url = url.Replace("{guildName}", !string.IsNullOrEmpty(request.GuildName) ? request.GuildName.ToString() : "null");

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
		public IEnumerator GetLastGuildMasterActivityByGuildName(
                Request.GetLastGuildMasterActivityByGuildNameRequest request,
                UnityAction<AsyncResult<Result.GetLastGuildMasterActivityByGuildNameResult>> callback
        ) =>
            new GetLastGuildMasterActivityByGuildNameTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.GetLastGuildMasterActivityByGuildNameResult> GetLastGuildMasterActivityByGuildNameFuture(
                Request.GetLastGuildMasterActivityByGuildNameRequest request
        ) =>
            new GetLastGuildMasterActivityByGuildNameTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.GetLastGuildMasterActivityByGuildNameResult> GetLastGuildMasterActivityByGuildNameAsync(
                Request.GetLastGuildMasterActivityByGuildNameRequest request
        ) =>
            new GetLastGuildMasterActivityByGuildNameTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.GetLastGuildMasterActivityByGuildNameResult>();
    #else
		public GetLastGuildMasterActivityByGuildNameTask GetLastGuildMasterActivityByGuildNameAsync(
                Request.GetLastGuildMasterActivityByGuildNameRequest request
        )
		{
			return new GetLastGuildMasterActivityByGuildNameTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.GetLastGuildMasterActivityByGuildNameResult> GetLastGuildMasterActivityByGuildNameAsync(
                Request.GetLastGuildMasterActivityByGuildNameRequest request
        ) =>
            new GetLastGuildMasterActivityByGuildNameTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class PromoteSeniorMemberTask : Gs2RestSessionTask<PromoteSeniorMemberRequest, PromoteSeniorMemberResult>
        {
            public PromoteSeniorMemberTask(IGs2Session session, RestSessionRequestFactory factory, PromoteSeniorMemberRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(PromoteSeniorMemberRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "guild")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/guild/{guildModelName}/me/promote";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{guildModelName}", !string.IsNullOrEmpty(request.GuildModelName) ? request.GuildModelName.ToString() : "null");

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
		public IEnumerator PromoteSeniorMember(
                Request.PromoteSeniorMemberRequest request,
                UnityAction<AsyncResult<Result.PromoteSeniorMemberResult>> callback
        ) =>
            new PromoteSeniorMemberTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.PromoteSeniorMemberResult> PromoteSeniorMemberFuture(
                Request.PromoteSeniorMemberRequest request
        ) =>
            new PromoteSeniorMemberTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.PromoteSeniorMemberResult> PromoteSeniorMemberAsync(
                Request.PromoteSeniorMemberRequest request
        ) =>
            new PromoteSeniorMemberTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.PromoteSeniorMemberResult>();
    #else
		public PromoteSeniorMemberTask PromoteSeniorMemberAsync(
                Request.PromoteSeniorMemberRequest request
        )
		{
			return new PromoteSeniorMemberTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.PromoteSeniorMemberResult> PromoteSeniorMemberAsync(
                Request.PromoteSeniorMemberRequest request
        ) =>
            new PromoteSeniorMemberTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class PromoteSeniorMemberByGuildNameTask : Gs2RestSessionTask<PromoteSeniorMemberByGuildNameRequest, PromoteSeniorMemberByGuildNameResult>
        {
            public PromoteSeniorMemberByGuildNameTask(IGs2Session session, RestSessionRequestFactory factory, PromoteSeniorMemberByGuildNameRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(PromoteSeniorMemberByGuildNameRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "guild")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/guild/{guildModelName}/{guildName}/promote";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{guildModelName}", !string.IsNullOrEmpty(request.GuildModelName) ? request.GuildModelName.ToString() : "null");
                url = url.Replace("{guildName}", !string.IsNullOrEmpty(request.GuildName) ? request.GuildName.ToString() : "null");

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
                if (request.DryRun)
                {
                    sessionRequest.AddHeader("X-GS2-DRY-RUN", "true");
                }

                AddHeader(
                    Session.Credential,
                    sessionRequest
                );

                return sessionRequest;
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator PromoteSeniorMemberByGuildName(
                Request.PromoteSeniorMemberByGuildNameRequest request,
                UnityAction<AsyncResult<Result.PromoteSeniorMemberByGuildNameResult>> callback
        ) =>
            new PromoteSeniorMemberByGuildNameTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.PromoteSeniorMemberByGuildNameResult> PromoteSeniorMemberByGuildNameFuture(
                Request.PromoteSeniorMemberByGuildNameRequest request
        ) =>
            new PromoteSeniorMemberByGuildNameTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.PromoteSeniorMemberByGuildNameResult> PromoteSeniorMemberByGuildNameAsync(
                Request.PromoteSeniorMemberByGuildNameRequest request
        ) =>
            new PromoteSeniorMemberByGuildNameTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.PromoteSeniorMemberByGuildNameResult>();
    #else
		public PromoteSeniorMemberByGuildNameTask PromoteSeniorMemberByGuildNameAsync(
                Request.PromoteSeniorMemberByGuildNameRequest request
        )
		{
			return new PromoteSeniorMemberByGuildNameTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.PromoteSeniorMemberByGuildNameResult> PromoteSeniorMemberByGuildNameAsync(
                Request.PromoteSeniorMemberByGuildNameRequest request
        ) =>
            new PromoteSeniorMemberByGuildNameTask(
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
                    .Replace("{service}", "guild")
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


        public class GetCurrentGuildMasterTask : Gs2RestSessionTask<GetCurrentGuildMasterRequest, GetCurrentGuildMasterResult>
        {
            public GetCurrentGuildMasterTask(IGs2Session session, RestSessionRequestFactory factory, GetCurrentGuildMasterRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(GetCurrentGuildMasterRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "guild")
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
		public IEnumerator GetCurrentGuildMaster(
                Request.GetCurrentGuildMasterRequest request,
                UnityAction<AsyncResult<Result.GetCurrentGuildMasterResult>> callback
        ) =>
            new GetCurrentGuildMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.GetCurrentGuildMasterResult> GetCurrentGuildMasterFuture(
                Request.GetCurrentGuildMasterRequest request
        ) =>
            new GetCurrentGuildMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.GetCurrentGuildMasterResult> GetCurrentGuildMasterAsync(
                Request.GetCurrentGuildMasterRequest request
        ) =>
            new GetCurrentGuildMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.GetCurrentGuildMasterResult>();
    #else
		public GetCurrentGuildMasterTask GetCurrentGuildMasterAsync(
                Request.GetCurrentGuildMasterRequest request
        )
		{
			return new GetCurrentGuildMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.GetCurrentGuildMasterResult> GetCurrentGuildMasterAsync(
                Request.GetCurrentGuildMasterRequest request
        ) =>
            new GetCurrentGuildMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class PreUpdateCurrentGuildMasterTask : Gs2RestSessionTask<PreUpdateCurrentGuildMasterRequest, PreUpdateCurrentGuildMasterResult>
        {
            public PreUpdateCurrentGuildMasterTask(IGs2Session session, RestSessionRequestFactory factory, PreUpdateCurrentGuildMasterRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(PreUpdateCurrentGuildMasterRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "guild")
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
		public IEnumerator PreUpdateCurrentGuildMaster(
                Request.PreUpdateCurrentGuildMasterRequest request,
                UnityAction<AsyncResult<Result.PreUpdateCurrentGuildMasterResult>> callback
        ) =>
            new PreUpdateCurrentGuildMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.PreUpdateCurrentGuildMasterResult> PreUpdateCurrentGuildMasterFuture(
                Request.PreUpdateCurrentGuildMasterRequest request
        ) =>
            new PreUpdateCurrentGuildMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.PreUpdateCurrentGuildMasterResult> PreUpdateCurrentGuildMasterAsync(
                Request.PreUpdateCurrentGuildMasterRequest request
        ) =>
            new PreUpdateCurrentGuildMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.PreUpdateCurrentGuildMasterResult>();
    #else
		public PreUpdateCurrentGuildMasterTask PreUpdateCurrentGuildMasterAsync(
                Request.PreUpdateCurrentGuildMasterRequest request
        )
		{
			return new PreUpdateCurrentGuildMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.PreUpdateCurrentGuildMasterResult> PreUpdateCurrentGuildMasterAsync(
                Request.PreUpdateCurrentGuildMasterRequest request
        ) =>
            new PreUpdateCurrentGuildMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class UpdateCurrentGuildMasterTask : Gs2RestSessionTask<UpdateCurrentGuildMasterRequest, UpdateCurrentGuildMasterResult>
        {
            public UpdateCurrentGuildMasterTask(IGs2Session session, RestSessionRequestFactory factory, UpdateCurrentGuildMasterRequest request) : base(session, factory, request)
            {
            }
#if GS2_ENABLE_UNITASK
            protected override async UniTask<UpdateCurrentGuildMasterResult> InvokeImpl()
#else
            protected override async Task<UpdateCurrentGuildMasterResult> InvokeImpl()
#endif
            {
                if (Request.Settings != null) {
                    var preTask = new PreUpdateCurrentGuildMasterTask(
                        Session,
                        Factory,
                        new PreUpdateCurrentGuildMasterRequest()
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

            protected override IGs2SessionRequest CreateRequest(UpdateCurrentGuildMasterRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "guild")
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
		public IEnumerator UpdateCurrentGuildMaster(
                Request.UpdateCurrentGuildMasterRequest request,
                UnityAction<AsyncResult<Result.UpdateCurrentGuildMasterResult>> callback
        ) =>
            new UpdateCurrentGuildMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.UpdateCurrentGuildMasterResult> UpdateCurrentGuildMasterFuture(
                Request.UpdateCurrentGuildMasterRequest request
        ) =>
            new UpdateCurrentGuildMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.UpdateCurrentGuildMasterResult> UpdateCurrentGuildMasterAsync(
                Request.UpdateCurrentGuildMasterRequest request
        ) =>
            new UpdateCurrentGuildMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.UpdateCurrentGuildMasterResult>();
    #else
		public UpdateCurrentGuildMasterTask UpdateCurrentGuildMasterAsync(
                Request.UpdateCurrentGuildMasterRequest request
        )
		{
			return new UpdateCurrentGuildMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.UpdateCurrentGuildMasterResult> UpdateCurrentGuildMasterAsync(
                Request.UpdateCurrentGuildMasterRequest request
        ) =>
            new UpdateCurrentGuildMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class UpdateCurrentGuildMasterFromGitHubTask : Gs2RestSessionTask<UpdateCurrentGuildMasterFromGitHubRequest, UpdateCurrentGuildMasterFromGitHubResult>
        {
            public UpdateCurrentGuildMasterFromGitHubTask(IGs2Session session, RestSessionRequestFactory factory, UpdateCurrentGuildMasterFromGitHubRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(UpdateCurrentGuildMasterFromGitHubRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "guild")
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
		public IEnumerator UpdateCurrentGuildMasterFromGitHub(
                Request.UpdateCurrentGuildMasterFromGitHubRequest request,
                UnityAction<AsyncResult<Result.UpdateCurrentGuildMasterFromGitHubResult>> callback
        ) =>
            new UpdateCurrentGuildMasterFromGitHubTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.UpdateCurrentGuildMasterFromGitHubResult> UpdateCurrentGuildMasterFromGitHubFuture(
                Request.UpdateCurrentGuildMasterFromGitHubRequest request
        ) =>
            new UpdateCurrentGuildMasterFromGitHubTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.UpdateCurrentGuildMasterFromGitHubResult> UpdateCurrentGuildMasterFromGitHubAsync(
                Request.UpdateCurrentGuildMasterFromGitHubRequest request
        ) =>
            new UpdateCurrentGuildMasterFromGitHubTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.UpdateCurrentGuildMasterFromGitHubResult>();
    #else
		public UpdateCurrentGuildMasterFromGitHubTask UpdateCurrentGuildMasterFromGitHubAsync(
                Request.UpdateCurrentGuildMasterFromGitHubRequest request
        )
		{
			return new UpdateCurrentGuildMasterFromGitHubTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.UpdateCurrentGuildMasterFromGitHubResult> UpdateCurrentGuildMasterFromGitHubAsync(
                Request.UpdateCurrentGuildMasterFromGitHubRequest request
        ) =>
            new UpdateCurrentGuildMasterFromGitHubTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class DescribeReceiveRequestsTask : Gs2RestSessionTask<DescribeReceiveRequestsRequest, DescribeReceiveRequestsResult>
        {
            public DescribeReceiveRequestsTask(IGs2Session session, RestSessionRequestFactory factory, DescribeReceiveRequestsRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DescribeReceiveRequestsRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "guild")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/guild/{guildModelName}/me/inbox";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{guildModelName}", !string.IsNullOrEmpty(request.GuildModelName) ? request.GuildModelName.ToString() : "null");

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
		public IEnumerator DescribeReceiveRequests(
                Request.DescribeReceiveRequestsRequest request,
                UnityAction<AsyncResult<Result.DescribeReceiveRequestsResult>> callback
        ) =>
            new DescribeReceiveRequestsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.DescribeReceiveRequestsResult> DescribeReceiveRequestsFuture(
                Request.DescribeReceiveRequestsRequest request
        ) =>
            new DescribeReceiveRequestsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.DescribeReceiveRequestsResult> DescribeReceiveRequestsAsync(
                Request.DescribeReceiveRequestsRequest request
        ) =>
            new DescribeReceiveRequestsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.DescribeReceiveRequestsResult>();
    #else
		public DescribeReceiveRequestsTask DescribeReceiveRequestsAsync(
                Request.DescribeReceiveRequestsRequest request
        )
		{
			return new DescribeReceiveRequestsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.DescribeReceiveRequestsResult> DescribeReceiveRequestsAsync(
                Request.DescribeReceiveRequestsRequest request
        ) =>
            new DescribeReceiveRequestsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class DescribeReceiveRequestsByGuildNameTask : Gs2RestSessionTask<DescribeReceiveRequestsByGuildNameRequest, DescribeReceiveRequestsByGuildNameResult>
        {
            public DescribeReceiveRequestsByGuildNameTask(IGs2Session session, RestSessionRequestFactory factory, DescribeReceiveRequestsByGuildNameRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DescribeReceiveRequestsByGuildNameRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "guild")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/guild/{guildModelName}/{guildName}/inbox";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{guildModelName}", !string.IsNullOrEmpty(request.GuildModelName) ? request.GuildModelName.ToString() : "null");
                url = url.Replace("{guildName}", !string.IsNullOrEmpty(request.GuildName) ? request.GuildName.ToString() : "null");

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
		public IEnumerator DescribeReceiveRequestsByGuildName(
                Request.DescribeReceiveRequestsByGuildNameRequest request,
                UnityAction<AsyncResult<Result.DescribeReceiveRequestsByGuildNameResult>> callback
        ) =>
            new DescribeReceiveRequestsByGuildNameTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.DescribeReceiveRequestsByGuildNameResult> DescribeReceiveRequestsByGuildNameFuture(
                Request.DescribeReceiveRequestsByGuildNameRequest request
        ) =>
            new DescribeReceiveRequestsByGuildNameTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.DescribeReceiveRequestsByGuildNameResult> DescribeReceiveRequestsByGuildNameAsync(
                Request.DescribeReceiveRequestsByGuildNameRequest request
        ) =>
            new DescribeReceiveRequestsByGuildNameTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.DescribeReceiveRequestsByGuildNameResult>();
    #else
		public DescribeReceiveRequestsByGuildNameTask DescribeReceiveRequestsByGuildNameAsync(
                Request.DescribeReceiveRequestsByGuildNameRequest request
        )
		{
			return new DescribeReceiveRequestsByGuildNameTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.DescribeReceiveRequestsByGuildNameResult> DescribeReceiveRequestsByGuildNameAsync(
                Request.DescribeReceiveRequestsByGuildNameRequest request
        ) =>
            new DescribeReceiveRequestsByGuildNameTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class GetReceiveRequestTask : Gs2RestSessionTask<GetReceiveRequestRequest, GetReceiveRequestResult>
        {
            public GetReceiveRequestTask(IGs2Session session, RestSessionRequestFactory factory, GetReceiveRequestRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(GetReceiveRequestRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "guild")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/guild/{guildModelName}/me/inbox/{fromUserId}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{guildModelName}", !string.IsNullOrEmpty(request.GuildModelName) ? request.GuildModelName.ToString() : "null");
                url = url.Replace("{fromUserId}", !string.IsNullOrEmpty(request.FromUserId) ? request.FromUserId.ToString() : "null");

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
		public IEnumerator GetReceiveRequest(
                Request.GetReceiveRequestRequest request,
                UnityAction<AsyncResult<Result.GetReceiveRequestResult>> callback
        ) =>
            new GetReceiveRequestTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.GetReceiveRequestResult> GetReceiveRequestFuture(
                Request.GetReceiveRequestRequest request
        ) =>
            new GetReceiveRequestTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.GetReceiveRequestResult> GetReceiveRequestAsync(
                Request.GetReceiveRequestRequest request
        ) =>
            new GetReceiveRequestTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.GetReceiveRequestResult>();
    #else
		public GetReceiveRequestTask GetReceiveRequestAsync(
                Request.GetReceiveRequestRequest request
        )
		{
			return new GetReceiveRequestTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.GetReceiveRequestResult> GetReceiveRequestAsync(
                Request.GetReceiveRequestRequest request
        ) =>
            new GetReceiveRequestTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class GetReceiveRequestByGuildNameTask : Gs2RestSessionTask<GetReceiveRequestByGuildNameRequest, GetReceiveRequestByGuildNameResult>
        {
            public GetReceiveRequestByGuildNameTask(IGs2Session session, RestSessionRequestFactory factory, GetReceiveRequestByGuildNameRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(GetReceiveRequestByGuildNameRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "guild")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/guild/{guildModelName}/{guildName}/inbox/{fromUserId}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{guildModelName}", !string.IsNullOrEmpty(request.GuildModelName) ? request.GuildModelName.ToString() : "null");
                url = url.Replace("{guildName}", !string.IsNullOrEmpty(request.GuildName) ? request.GuildName.ToString() : "null");
                url = url.Replace("{fromUserId}", !string.IsNullOrEmpty(request.FromUserId) ? request.FromUserId.ToString() : "null");

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
		public IEnumerator GetReceiveRequestByGuildName(
                Request.GetReceiveRequestByGuildNameRequest request,
                UnityAction<AsyncResult<Result.GetReceiveRequestByGuildNameResult>> callback
        ) =>
            new GetReceiveRequestByGuildNameTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.GetReceiveRequestByGuildNameResult> GetReceiveRequestByGuildNameFuture(
                Request.GetReceiveRequestByGuildNameRequest request
        ) =>
            new GetReceiveRequestByGuildNameTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.GetReceiveRequestByGuildNameResult> GetReceiveRequestByGuildNameAsync(
                Request.GetReceiveRequestByGuildNameRequest request
        ) =>
            new GetReceiveRequestByGuildNameTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.GetReceiveRequestByGuildNameResult>();
    #else
		public GetReceiveRequestByGuildNameTask GetReceiveRequestByGuildNameAsync(
                Request.GetReceiveRequestByGuildNameRequest request
        )
		{
			return new GetReceiveRequestByGuildNameTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.GetReceiveRequestByGuildNameResult> GetReceiveRequestByGuildNameAsync(
                Request.GetReceiveRequestByGuildNameRequest request
        ) =>
            new GetReceiveRequestByGuildNameTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class AcceptRequestTask : Gs2RestSessionTask<AcceptRequestRequest, AcceptRequestResult>
        {
            public AcceptRequestTask(IGs2Session session, RestSessionRequestFactory factory, AcceptRequestRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(AcceptRequestRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "guild")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/guild/{guildModelName}/me/inbox/{fromUserId}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{guildModelName}", !string.IsNullOrEmpty(request.GuildModelName) ? request.GuildModelName.ToString() : "null");
                url = url.Replace("{fromUserId}", !string.IsNullOrEmpty(request.FromUserId) ? request.FromUserId.ToString() : "null");

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

            public override void OnError(Gs2.Core.Exception.Gs2Exception error)
            {
                if (error.Errors.Count(v => v.code == "user.joinedGuild.tooMany") > 0) {
                    base.OnError(new Exception.MaximumJoinedGuildsReachedException(error));
                }
                else if (error.Errors.Count(v => v.code == "guild.members.tooMany") > 0) {
                    base.OnError(new Exception.MaximumMembersReachedException(error));
                }
                else {
                    base.OnError(error);
                }
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator AcceptRequest(
                Request.AcceptRequestRequest request,
                UnityAction<AsyncResult<Result.AcceptRequestResult>> callback
        ) =>
            new AcceptRequestTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.AcceptRequestResult> AcceptRequestFuture(
                Request.AcceptRequestRequest request
        ) =>
            new AcceptRequestTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.AcceptRequestResult> AcceptRequestAsync(
                Request.AcceptRequestRequest request
        ) =>
            new AcceptRequestTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.AcceptRequestResult>();
    #else
		public AcceptRequestTask AcceptRequestAsync(
                Request.AcceptRequestRequest request
        )
		{
			return new AcceptRequestTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.AcceptRequestResult> AcceptRequestAsync(
                Request.AcceptRequestRequest request
        ) =>
            new AcceptRequestTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class AcceptRequestByGuildNameTask : Gs2RestSessionTask<AcceptRequestByGuildNameRequest, AcceptRequestByGuildNameResult>
        {
            public AcceptRequestByGuildNameTask(IGs2Session session, RestSessionRequestFactory factory, AcceptRequestByGuildNameRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(AcceptRequestByGuildNameRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "guild")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/guild/{guildModelName}/{guildName}/inbox/{fromUserId}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{guildModelName}", !string.IsNullOrEmpty(request.GuildModelName) ? request.GuildModelName.ToString() : "null");
                url = url.Replace("{guildName}", !string.IsNullOrEmpty(request.GuildName) ? request.GuildName.ToString() : "null");
                url = url.Replace("{fromUserId}", !string.IsNullOrEmpty(request.FromUserId) ? request.FromUserId.ToString() : "null");

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
                if (request.DryRun)
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
                if (error.Errors.Count(v => v.code == "user.joinedGuild.tooMany") > 0) {
                    base.OnError(new Exception.MaximumJoinedGuildsReachedException(error));
                }
                else if (error.Errors.Count(v => v.code == "guild.members.tooMany") > 0) {
                    base.OnError(new Exception.MaximumMembersReachedException(error));
                }
                else {
                    base.OnError(error);
                }
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator AcceptRequestByGuildName(
                Request.AcceptRequestByGuildNameRequest request,
                UnityAction<AsyncResult<Result.AcceptRequestByGuildNameResult>> callback
        ) =>
            new AcceptRequestByGuildNameTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.AcceptRequestByGuildNameResult> AcceptRequestByGuildNameFuture(
                Request.AcceptRequestByGuildNameRequest request
        ) =>
            new AcceptRequestByGuildNameTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.AcceptRequestByGuildNameResult> AcceptRequestByGuildNameAsync(
                Request.AcceptRequestByGuildNameRequest request
        ) =>
            new AcceptRequestByGuildNameTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.AcceptRequestByGuildNameResult>();
    #else
		public AcceptRequestByGuildNameTask AcceptRequestByGuildNameAsync(
                Request.AcceptRequestByGuildNameRequest request
        )
		{
			return new AcceptRequestByGuildNameTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.AcceptRequestByGuildNameResult> AcceptRequestByGuildNameAsync(
                Request.AcceptRequestByGuildNameRequest request
        ) =>
            new AcceptRequestByGuildNameTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class RejectRequestTask : Gs2RestSessionTask<RejectRequestRequest, RejectRequestResult>
        {
            public RejectRequestTask(IGs2Session session, RestSessionRequestFactory factory, RejectRequestRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(RejectRequestRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "guild")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/guild/{guildModelName}/me/inbox/{fromUserId}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{guildModelName}", !string.IsNullOrEmpty(request.GuildModelName) ? request.GuildModelName.ToString() : "null");
                url = url.Replace("{fromUserId}", !string.IsNullOrEmpty(request.FromUserId) ? request.FromUserId.ToString() : "null");

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
		public IEnumerator RejectRequest(
                Request.RejectRequestRequest request,
                UnityAction<AsyncResult<Result.RejectRequestResult>> callback
        ) =>
            new RejectRequestTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.RejectRequestResult> RejectRequestFuture(
                Request.RejectRequestRequest request
        ) =>
            new RejectRequestTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.RejectRequestResult> RejectRequestAsync(
                Request.RejectRequestRequest request
        ) =>
            new RejectRequestTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.RejectRequestResult>();
    #else
		public RejectRequestTask RejectRequestAsync(
                Request.RejectRequestRequest request
        )
		{
			return new RejectRequestTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.RejectRequestResult> RejectRequestAsync(
                Request.RejectRequestRequest request
        ) =>
            new RejectRequestTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class RejectRequestByGuildNameTask : Gs2RestSessionTask<RejectRequestByGuildNameRequest, RejectRequestByGuildNameResult>
        {
            public RejectRequestByGuildNameTask(IGs2Session session, RestSessionRequestFactory factory, RejectRequestByGuildNameRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(RejectRequestByGuildNameRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "guild")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/guild/{guildModelName}/{guildName}/inbox/{fromUserId}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{guildModelName}", !string.IsNullOrEmpty(request.GuildModelName) ? request.GuildModelName.ToString() : "null");
                url = url.Replace("{guildName}", !string.IsNullOrEmpty(request.GuildName) ? request.GuildName.ToString() : "null");
                url = url.Replace("{fromUserId}", !string.IsNullOrEmpty(request.FromUserId) ? request.FromUserId.ToString() : "null");

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
		public IEnumerator RejectRequestByGuildName(
                Request.RejectRequestByGuildNameRequest request,
                UnityAction<AsyncResult<Result.RejectRequestByGuildNameResult>> callback
        ) =>
            new RejectRequestByGuildNameTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.RejectRequestByGuildNameResult> RejectRequestByGuildNameFuture(
                Request.RejectRequestByGuildNameRequest request
        ) =>
            new RejectRequestByGuildNameTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.RejectRequestByGuildNameResult> RejectRequestByGuildNameAsync(
                Request.RejectRequestByGuildNameRequest request
        ) =>
            new RejectRequestByGuildNameTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.RejectRequestByGuildNameResult>();
    #else
		public RejectRequestByGuildNameTask RejectRequestByGuildNameAsync(
                Request.RejectRequestByGuildNameRequest request
        )
		{
			return new RejectRequestByGuildNameTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.RejectRequestByGuildNameResult> RejectRequestByGuildNameAsync(
                Request.RejectRequestByGuildNameRequest request
        ) =>
            new RejectRequestByGuildNameTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class DescribeSendRequestsTask : Gs2RestSessionTask<DescribeSendRequestsRequest, DescribeSendRequestsResult>
        {
            public DescribeSendRequestsTask(IGs2Session session, RestSessionRequestFactory factory, DescribeSendRequestsRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DescribeSendRequestsRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "guild")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/me/sendBox/guild/{guildModelName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{guildModelName}", !string.IsNullOrEmpty(request.GuildModelName) ? request.GuildModelName.ToString() : "null");

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
		public IEnumerator DescribeSendRequests(
                Request.DescribeSendRequestsRequest request,
                UnityAction<AsyncResult<Result.DescribeSendRequestsResult>> callback
        ) =>
            new DescribeSendRequestsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.DescribeSendRequestsResult> DescribeSendRequestsFuture(
                Request.DescribeSendRequestsRequest request
        ) =>
            new DescribeSendRequestsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.DescribeSendRequestsResult> DescribeSendRequestsAsync(
                Request.DescribeSendRequestsRequest request
        ) =>
            new DescribeSendRequestsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.DescribeSendRequestsResult>();
    #else
		public DescribeSendRequestsTask DescribeSendRequestsAsync(
                Request.DescribeSendRequestsRequest request
        )
		{
			return new DescribeSendRequestsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.DescribeSendRequestsResult> DescribeSendRequestsAsync(
                Request.DescribeSendRequestsRequest request
        ) =>
            new DescribeSendRequestsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class DescribeSendRequestsByUserIdTask : Gs2RestSessionTask<DescribeSendRequestsByUserIdRequest, DescribeSendRequestsByUserIdResult>
        {
            public DescribeSendRequestsByUserIdTask(IGs2Session session, RestSessionRequestFactory factory, DescribeSendRequestsByUserIdRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DescribeSendRequestsByUserIdRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "guild")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/{userId}/sendBox/guild/{guildModelName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{userId}", !string.IsNullOrEmpty(request.UserId) ? request.UserId.ToString() : "null");
                url = url.Replace("{guildModelName}", !string.IsNullOrEmpty(request.GuildModelName) ? request.GuildModelName.ToString() : "null");

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
		public IEnumerator DescribeSendRequestsByUserId(
                Request.DescribeSendRequestsByUserIdRequest request,
                UnityAction<AsyncResult<Result.DescribeSendRequestsByUserIdResult>> callback
        ) =>
            new DescribeSendRequestsByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.DescribeSendRequestsByUserIdResult> DescribeSendRequestsByUserIdFuture(
                Request.DescribeSendRequestsByUserIdRequest request
        ) =>
            new DescribeSendRequestsByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.DescribeSendRequestsByUserIdResult> DescribeSendRequestsByUserIdAsync(
                Request.DescribeSendRequestsByUserIdRequest request
        ) =>
            new DescribeSendRequestsByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.DescribeSendRequestsByUserIdResult>();
    #else
		public DescribeSendRequestsByUserIdTask DescribeSendRequestsByUserIdAsync(
                Request.DescribeSendRequestsByUserIdRequest request
        )
		{
			return new DescribeSendRequestsByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.DescribeSendRequestsByUserIdResult> DescribeSendRequestsByUserIdAsync(
                Request.DescribeSendRequestsByUserIdRequest request
        ) =>
            new DescribeSendRequestsByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class GetSendRequestTask : Gs2RestSessionTask<GetSendRequestRequest, GetSendRequestResult>
        {
            public GetSendRequestTask(IGs2Session session, RestSessionRequestFactory factory, GetSendRequestRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(GetSendRequestRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "guild")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/me/sendBox/guild/{guildModelName}/{targetGuildName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{guildModelName}", !string.IsNullOrEmpty(request.GuildModelName) ? request.GuildModelName.ToString() : "null");
                url = url.Replace("{targetGuildName}", !string.IsNullOrEmpty(request.TargetGuildName) ? request.TargetGuildName.ToString() : "null");

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
		public IEnumerator GetSendRequest(
                Request.GetSendRequestRequest request,
                UnityAction<AsyncResult<Result.GetSendRequestResult>> callback
        ) =>
            new GetSendRequestTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.GetSendRequestResult> GetSendRequestFuture(
                Request.GetSendRequestRequest request
        ) =>
            new GetSendRequestTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.GetSendRequestResult> GetSendRequestAsync(
                Request.GetSendRequestRequest request
        ) =>
            new GetSendRequestTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.GetSendRequestResult>();
    #else
		public GetSendRequestTask GetSendRequestAsync(
                Request.GetSendRequestRequest request
        )
		{
			return new GetSendRequestTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.GetSendRequestResult> GetSendRequestAsync(
                Request.GetSendRequestRequest request
        ) =>
            new GetSendRequestTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class GetSendRequestByUserIdTask : Gs2RestSessionTask<GetSendRequestByUserIdRequest, GetSendRequestByUserIdResult>
        {
            public GetSendRequestByUserIdTask(IGs2Session session, RestSessionRequestFactory factory, GetSendRequestByUserIdRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(GetSendRequestByUserIdRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "guild")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/{userId}/sendBox/guild/{guildModelName}/{targetGuildName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{userId}", !string.IsNullOrEmpty(request.UserId) ? request.UserId.ToString() : "null");
                url = url.Replace("{guildModelName}", !string.IsNullOrEmpty(request.GuildModelName) ? request.GuildModelName.ToString() : "null");
                url = url.Replace("{targetGuildName}", !string.IsNullOrEmpty(request.TargetGuildName) ? request.TargetGuildName.ToString() : "null");

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
		public IEnumerator GetSendRequestByUserId(
                Request.GetSendRequestByUserIdRequest request,
                UnityAction<AsyncResult<Result.GetSendRequestByUserIdResult>> callback
        ) =>
            new GetSendRequestByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.GetSendRequestByUserIdResult> GetSendRequestByUserIdFuture(
                Request.GetSendRequestByUserIdRequest request
        ) =>
            new GetSendRequestByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.GetSendRequestByUserIdResult> GetSendRequestByUserIdAsync(
                Request.GetSendRequestByUserIdRequest request
        ) =>
            new GetSendRequestByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.GetSendRequestByUserIdResult>();
    #else
		public GetSendRequestByUserIdTask GetSendRequestByUserIdAsync(
                Request.GetSendRequestByUserIdRequest request
        )
		{
			return new GetSendRequestByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.GetSendRequestByUserIdResult> GetSendRequestByUserIdAsync(
                Request.GetSendRequestByUserIdRequest request
        ) =>
            new GetSendRequestByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class SendRequestTask : Gs2RestSessionTask<SendRequestRequest, SendRequestResult>
        {
            public SendRequestTask(IGs2Session session, RestSessionRequestFactory factory, SendRequestRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(SendRequestRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "guild")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/me/sendBox/guild/{guildModelName}/{targetGuildName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{guildModelName}", !string.IsNullOrEmpty(request.GuildModelName) ? request.GuildModelName.ToString() : "null");
                url = url.Replace("{targetGuildName}", !string.IsNullOrEmpty(request.TargetGuildName) ? request.TargetGuildName.ToString() : "null");

                var sessionRequest = Factory.Put(url);

                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);
                jsonWriter.WriteObjectStart();
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

            public override void OnError(Gs2.Core.Exception.Gs2Exception error)
            {
                if (error.Errors.Count(v => v.code == "guild.members.tooMany") > 0) {
                    base.OnError(new Exception.MaximumMembersReachedException(error));
                }
                else if (error.Errors.Count(v => v.code == "user.joinedGuild.tooMany") > 0) {
                    base.OnError(new Exception.MaximumJoinedGuildsReachedException(error));
                }
                else if (error.Errors.Count(v => v.code == "guild.receiveRequests.tooMany") > 0) {
                    base.OnError(new Exception.MaximumReceiveRequestsReachedException(error));
                }
                else if (error.Errors.Count(v => v.code == "guild.sendRequests.tooMany") > 0) {
                    base.OnError(new Exception.MaximumSendRequestsReachedException(error));
                }
                else if (error.Errors.Count(v => v.code == "guild.sendRequests.notMeetJoinRequirements") > 0) {
                    base.OnError(new Exception.DotMeetJoinRequirementsException(error));
                }
                else {
                    base.OnError(error);
                }
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator SendRequest(
                Request.SendRequestRequest request,
                UnityAction<AsyncResult<Result.SendRequestResult>> callback
        ) =>
            new SendRequestTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.SendRequestResult> SendRequestFuture(
                Request.SendRequestRequest request
        ) =>
            new SendRequestTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.SendRequestResult> SendRequestAsync(
                Request.SendRequestRequest request
        ) =>
            new SendRequestTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.SendRequestResult>();
    #else
		public SendRequestTask SendRequestAsync(
                Request.SendRequestRequest request
        )
		{
			return new SendRequestTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.SendRequestResult> SendRequestAsync(
                Request.SendRequestRequest request
        ) =>
            new SendRequestTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class SendRequestByUserIdTask : Gs2RestSessionTask<SendRequestByUserIdRequest, SendRequestByUserIdResult>
        {
            public SendRequestByUserIdTask(IGs2Session session, RestSessionRequestFactory factory, SendRequestByUserIdRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(SendRequestByUserIdRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "guild")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/{userId}/sendBox/guild/{guildModelName}/{targetGuildName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{userId}", !string.IsNullOrEmpty(request.UserId) ? request.UserId.ToString() : "null");
                url = url.Replace("{guildModelName}", !string.IsNullOrEmpty(request.GuildModelName) ? request.GuildModelName.ToString() : "null");
                url = url.Replace("{targetGuildName}", !string.IsNullOrEmpty(request.TargetGuildName) ? request.TargetGuildName.ToString() : "null");

                var sessionRequest = Factory.Put(url);

                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);
                jsonWriter.WriteObjectStart();
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

            public override void OnError(Gs2.Core.Exception.Gs2Exception error)
            {
                if (error.Errors.Count(v => v.code == "guild.members.tooMany") > 0) {
                    base.OnError(new Exception.MaximumMembersReachedException(error));
                }
                else if (error.Errors.Count(v => v.code == "user.joinedGuild.tooMany") > 0) {
                    base.OnError(new Exception.MaximumJoinedGuildsReachedException(error));
                }
                else if (error.Errors.Count(v => v.code == "guild.receiveRequests.tooMany") > 0) {
                    base.OnError(new Exception.MaximumReceiveRequestsReachedException(error));
                }
                else if (error.Errors.Count(v => v.code == "guild.sendRequests.tooMany") > 0) {
                    base.OnError(new Exception.MaximumSendRequestsReachedException(error));
                }
                else if (error.Errors.Count(v => v.code == "guild.sendRequests.notMeetJoinRequirements") > 0) {
                    base.OnError(new Exception.DotMeetJoinRequirementsException(error));
                }
                else {
                    base.OnError(error);
                }
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator SendRequestByUserId(
                Request.SendRequestByUserIdRequest request,
                UnityAction<AsyncResult<Result.SendRequestByUserIdResult>> callback
        ) =>
            new SendRequestByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.SendRequestByUserIdResult> SendRequestByUserIdFuture(
                Request.SendRequestByUserIdRequest request
        ) =>
            new SendRequestByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.SendRequestByUserIdResult> SendRequestByUserIdAsync(
                Request.SendRequestByUserIdRequest request
        ) =>
            new SendRequestByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.SendRequestByUserIdResult>();
    #else
		public SendRequestByUserIdTask SendRequestByUserIdAsync(
                Request.SendRequestByUserIdRequest request
        )
		{
			return new SendRequestByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.SendRequestByUserIdResult> SendRequestByUserIdAsync(
                Request.SendRequestByUserIdRequest request
        ) =>
            new SendRequestByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class DeleteRequestTask : Gs2RestSessionTask<DeleteRequestRequest, DeleteRequestResult>
        {
            public DeleteRequestTask(IGs2Session session, RestSessionRequestFactory factory, DeleteRequestRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DeleteRequestRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "guild")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/me/sendBox/guild/{guildModelName}/{targetGuildName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{guildModelName}", !string.IsNullOrEmpty(request.GuildModelName) ? request.GuildModelName.ToString() : "null");
                url = url.Replace("{targetGuildName}", !string.IsNullOrEmpty(request.TargetGuildName) ? request.TargetGuildName.ToString() : "null");

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
		public IEnumerator DeleteRequest(
                Request.DeleteRequestRequest request,
                UnityAction<AsyncResult<Result.DeleteRequestResult>> callback
        ) =>
            new DeleteRequestTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.DeleteRequestResult> DeleteRequestFuture(
                Request.DeleteRequestRequest request
        ) =>
            new DeleteRequestTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.DeleteRequestResult> DeleteRequestAsync(
                Request.DeleteRequestRequest request
        ) =>
            new DeleteRequestTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.DeleteRequestResult>();
    #else
		public DeleteRequestTask DeleteRequestAsync(
                Request.DeleteRequestRequest request
        )
		{
			return new DeleteRequestTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.DeleteRequestResult> DeleteRequestAsync(
                Request.DeleteRequestRequest request
        ) =>
            new DeleteRequestTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class DeleteRequestByUserIdTask : Gs2RestSessionTask<DeleteRequestByUserIdRequest, DeleteRequestByUserIdResult>
        {
            public DeleteRequestByUserIdTask(IGs2Session session, RestSessionRequestFactory factory, DeleteRequestByUserIdRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DeleteRequestByUserIdRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "guild")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/{userId}/sendBox/guild/{guildModelName}/{targetGuildName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{userId}", !string.IsNullOrEmpty(request.UserId) ? request.UserId.ToString() : "null");
                url = url.Replace("{guildModelName}", !string.IsNullOrEmpty(request.GuildModelName) ? request.GuildModelName.ToString() : "null");
                url = url.Replace("{targetGuildName}", !string.IsNullOrEmpty(request.TargetGuildName) ? request.TargetGuildName.ToString() : "null");

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
		public IEnumerator DeleteRequestByUserId(
                Request.DeleteRequestByUserIdRequest request,
                UnityAction<AsyncResult<Result.DeleteRequestByUserIdResult>> callback
        ) =>
            new DeleteRequestByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.DeleteRequestByUserIdResult> DeleteRequestByUserIdFuture(
                Request.DeleteRequestByUserIdRequest request
        ) =>
            new DeleteRequestByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.DeleteRequestByUserIdResult> DeleteRequestByUserIdAsync(
                Request.DeleteRequestByUserIdRequest request
        ) =>
            new DeleteRequestByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.DeleteRequestByUserIdResult>();
    #else
		public DeleteRequestByUserIdTask DeleteRequestByUserIdAsync(
                Request.DeleteRequestByUserIdRequest request
        )
		{
			return new DeleteRequestByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.DeleteRequestByUserIdResult> DeleteRequestByUserIdAsync(
                Request.DeleteRequestByUserIdRequest request
        ) =>
            new DeleteRequestByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class DescribeIgnoreUsersTask : Gs2RestSessionTask<DescribeIgnoreUsersRequest, DescribeIgnoreUsersResult>
        {
            public DescribeIgnoreUsersTask(IGs2Session session, RestSessionRequestFactory factory, DescribeIgnoreUsersRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DescribeIgnoreUsersRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "guild")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/guild/{guildModelName}/me/ignore/user";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{guildModelName}", !string.IsNullOrEmpty(request.GuildModelName) ? request.GuildModelName.ToString() : "null");

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
		public IEnumerator DescribeIgnoreUsers(
                Request.DescribeIgnoreUsersRequest request,
                UnityAction<AsyncResult<Result.DescribeIgnoreUsersResult>> callback
        ) =>
            new DescribeIgnoreUsersTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.DescribeIgnoreUsersResult> DescribeIgnoreUsersFuture(
                Request.DescribeIgnoreUsersRequest request
        ) =>
            new DescribeIgnoreUsersTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.DescribeIgnoreUsersResult> DescribeIgnoreUsersAsync(
                Request.DescribeIgnoreUsersRequest request
        ) =>
            new DescribeIgnoreUsersTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.DescribeIgnoreUsersResult>();
    #else
		public DescribeIgnoreUsersTask DescribeIgnoreUsersAsync(
                Request.DescribeIgnoreUsersRequest request
        )
		{
			return new DescribeIgnoreUsersTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.DescribeIgnoreUsersResult> DescribeIgnoreUsersAsync(
                Request.DescribeIgnoreUsersRequest request
        ) =>
            new DescribeIgnoreUsersTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class DescribeIgnoreUsersByGuildNameTask : Gs2RestSessionTask<DescribeIgnoreUsersByGuildNameRequest, DescribeIgnoreUsersByGuildNameResult>
        {
            public DescribeIgnoreUsersByGuildNameTask(IGs2Session session, RestSessionRequestFactory factory, DescribeIgnoreUsersByGuildNameRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DescribeIgnoreUsersByGuildNameRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "guild")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/guild/{guildModelName}/{guildName}/ignore/user";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{guildModelName}", !string.IsNullOrEmpty(request.GuildModelName) ? request.GuildModelName.ToString() : "null");
                url = url.Replace("{guildName}", !string.IsNullOrEmpty(request.GuildName) ? request.GuildName.ToString() : "null");

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
		public IEnumerator DescribeIgnoreUsersByGuildName(
                Request.DescribeIgnoreUsersByGuildNameRequest request,
                UnityAction<AsyncResult<Result.DescribeIgnoreUsersByGuildNameResult>> callback
        ) =>
            new DescribeIgnoreUsersByGuildNameTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.DescribeIgnoreUsersByGuildNameResult> DescribeIgnoreUsersByGuildNameFuture(
                Request.DescribeIgnoreUsersByGuildNameRequest request
        ) =>
            new DescribeIgnoreUsersByGuildNameTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.DescribeIgnoreUsersByGuildNameResult> DescribeIgnoreUsersByGuildNameAsync(
                Request.DescribeIgnoreUsersByGuildNameRequest request
        ) =>
            new DescribeIgnoreUsersByGuildNameTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.DescribeIgnoreUsersByGuildNameResult>();
    #else
		public DescribeIgnoreUsersByGuildNameTask DescribeIgnoreUsersByGuildNameAsync(
                Request.DescribeIgnoreUsersByGuildNameRequest request
        )
		{
			return new DescribeIgnoreUsersByGuildNameTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.DescribeIgnoreUsersByGuildNameResult> DescribeIgnoreUsersByGuildNameAsync(
                Request.DescribeIgnoreUsersByGuildNameRequest request
        ) =>
            new DescribeIgnoreUsersByGuildNameTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class GetIgnoreUserTask : Gs2RestSessionTask<GetIgnoreUserRequest, GetIgnoreUserResult>
        {
            public GetIgnoreUserTask(IGs2Session session, RestSessionRequestFactory factory, GetIgnoreUserRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(GetIgnoreUserRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "guild")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/guild/{guildModelName}/me/ignore/user/{userId}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{guildModelName}", !string.IsNullOrEmpty(request.GuildModelName) ? request.GuildModelName.ToString() : "null");
                url = url.Replace("{userId}", !string.IsNullOrEmpty(request.UserId) ? request.UserId.ToString() : "null");

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
		public IEnumerator GetIgnoreUser(
                Request.GetIgnoreUserRequest request,
                UnityAction<AsyncResult<Result.GetIgnoreUserResult>> callback
        ) =>
            new GetIgnoreUserTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.GetIgnoreUserResult> GetIgnoreUserFuture(
                Request.GetIgnoreUserRequest request
        ) =>
            new GetIgnoreUserTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.GetIgnoreUserResult> GetIgnoreUserAsync(
                Request.GetIgnoreUserRequest request
        ) =>
            new GetIgnoreUserTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.GetIgnoreUserResult>();
    #else
		public GetIgnoreUserTask GetIgnoreUserAsync(
                Request.GetIgnoreUserRequest request
        )
		{
			return new GetIgnoreUserTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.GetIgnoreUserResult> GetIgnoreUserAsync(
                Request.GetIgnoreUserRequest request
        ) =>
            new GetIgnoreUserTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class GetIgnoreUserByGuildNameTask : Gs2RestSessionTask<GetIgnoreUserByGuildNameRequest, GetIgnoreUserByGuildNameResult>
        {
            public GetIgnoreUserByGuildNameTask(IGs2Session session, RestSessionRequestFactory factory, GetIgnoreUserByGuildNameRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(GetIgnoreUserByGuildNameRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "guild")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/guild/{guildModelName}/{guildName}/ignore/user/{userId}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{guildModelName}", !string.IsNullOrEmpty(request.GuildModelName) ? request.GuildModelName.ToString() : "null");
                url = url.Replace("{guildName}", !string.IsNullOrEmpty(request.GuildName) ? request.GuildName.ToString() : "null");
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
		public IEnumerator GetIgnoreUserByGuildName(
                Request.GetIgnoreUserByGuildNameRequest request,
                UnityAction<AsyncResult<Result.GetIgnoreUserByGuildNameResult>> callback
        ) =>
            new GetIgnoreUserByGuildNameTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.GetIgnoreUserByGuildNameResult> GetIgnoreUserByGuildNameFuture(
                Request.GetIgnoreUserByGuildNameRequest request
        ) =>
            new GetIgnoreUserByGuildNameTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.GetIgnoreUserByGuildNameResult> GetIgnoreUserByGuildNameAsync(
                Request.GetIgnoreUserByGuildNameRequest request
        ) =>
            new GetIgnoreUserByGuildNameTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.GetIgnoreUserByGuildNameResult>();
    #else
		public GetIgnoreUserByGuildNameTask GetIgnoreUserByGuildNameAsync(
                Request.GetIgnoreUserByGuildNameRequest request
        )
		{
			return new GetIgnoreUserByGuildNameTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.GetIgnoreUserByGuildNameResult> GetIgnoreUserByGuildNameAsync(
                Request.GetIgnoreUserByGuildNameRequest request
        ) =>
            new GetIgnoreUserByGuildNameTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class AddIgnoreUserTask : Gs2RestSessionTask<AddIgnoreUserRequest, AddIgnoreUserResult>
        {
            public AddIgnoreUserTask(IGs2Session session, RestSessionRequestFactory factory, AddIgnoreUserRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(AddIgnoreUserRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "guild")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/guild/{guildModelName}/me/ignore/user/{userId}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{guildModelName}", !string.IsNullOrEmpty(request.GuildModelName) ? request.GuildModelName.ToString() : "null");
                url = url.Replace("{userId}", !string.IsNullOrEmpty(request.UserId) ? request.UserId.ToString() : "null");

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
		public IEnumerator AddIgnoreUser(
                Request.AddIgnoreUserRequest request,
                UnityAction<AsyncResult<Result.AddIgnoreUserResult>> callback
        ) =>
            new AddIgnoreUserTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.AddIgnoreUserResult> AddIgnoreUserFuture(
                Request.AddIgnoreUserRequest request
        ) =>
            new AddIgnoreUserTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.AddIgnoreUserResult> AddIgnoreUserAsync(
                Request.AddIgnoreUserRequest request
        ) =>
            new AddIgnoreUserTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.AddIgnoreUserResult>();
    #else
		public AddIgnoreUserTask AddIgnoreUserAsync(
                Request.AddIgnoreUserRequest request
        )
		{
			return new AddIgnoreUserTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.AddIgnoreUserResult> AddIgnoreUserAsync(
                Request.AddIgnoreUserRequest request
        ) =>
            new AddIgnoreUserTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class AddIgnoreUserByGuildNameTask : Gs2RestSessionTask<AddIgnoreUserByGuildNameRequest, AddIgnoreUserByGuildNameResult>
        {
            public AddIgnoreUserByGuildNameTask(IGs2Session session, RestSessionRequestFactory factory, AddIgnoreUserByGuildNameRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(AddIgnoreUserByGuildNameRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "guild")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/guild/{guildModelName}/{guildName}/ignore/user/{userId}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{guildModelName}", !string.IsNullOrEmpty(request.GuildModelName) ? request.GuildModelName.ToString() : "null");
                url = url.Replace("{guildName}", !string.IsNullOrEmpty(request.GuildName) ? request.GuildName.ToString() : "null");
                url = url.Replace("{userId}", !string.IsNullOrEmpty(request.UserId) ? request.UserId.ToString() : "null");

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
		public IEnumerator AddIgnoreUserByGuildName(
                Request.AddIgnoreUserByGuildNameRequest request,
                UnityAction<AsyncResult<Result.AddIgnoreUserByGuildNameResult>> callback
        ) =>
            new AddIgnoreUserByGuildNameTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.AddIgnoreUserByGuildNameResult> AddIgnoreUserByGuildNameFuture(
                Request.AddIgnoreUserByGuildNameRequest request
        ) =>
            new AddIgnoreUserByGuildNameTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.AddIgnoreUserByGuildNameResult> AddIgnoreUserByGuildNameAsync(
                Request.AddIgnoreUserByGuildNameRequest request
        ) =>
            new AddIgnoreUserByGuildNameTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.AddIgnoreUserByGuildNameResult>();
    #else
		public AddIgnoreUserByGuildNameTask AddIgnoreUserByGuildNameAsync(
                Request.AddIgnoreUserByGuildNameRequest request
        )
		{
			return new AddIgnoreUserByGuildNameTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.AddIgnoreUserByGuildNameResult> AddIgnoreUserByGuildNameAsync(
                Request.AddIgnoreUserByGuildNameRequest request
        ) =>
            new AddIgnoreUserByGuildNameTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class DeleteIgnoreUserTask : Gs2RestSessionTask<DeleteIgnoreUserRequest, DeleteIgnoreUserResult>
        {
            public DeleteIgnoreUserTask(IGs2Session session, RestSessionRequestFactory factory, DeleteIgnoreUserRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DeleteIgnoreUserRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "guild")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/guild/{guildModelName}/me/ignore/user/{userId}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{guildModelName}", !string.IsNullOrEmpty(request.GuildModelName) ? request.GuildModelName.ToString() : "null");
                url = url.Replace("{userId}", !string.IsNullOrEmpty(request.UserId) ? request.UserId.ToString() : "null");

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
		public IEnumerator DeleteIgnoreUser(
                Request.DeleteIgnoreUserRequest request,
                UnityAction<AsyncResult<Result.DeleteIgnoreUserResult>> callback
        ) =>
            new DeleteIgnoreUserTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.DeleteIgnoreUserResult> DeleteIgnoreUserFuture(
                Request.DeleteIgnoreUserRequest request
        ) =>
            new DeleteIgnoreUserTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.DeleteIgnoreUserResult> DeleteIgnoreUserAsync(
                Request.DeleteIgnoreUserRequest request
        ) =>
            new DeleteIgnoreUserTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.DeleteIgnoreUserResult>();
    #else
		public DeleteIgnoreUserTask DeleteIgnoreUserAsync(
                Request.DeleteIgnoreUserRequest request
        )
		{
			return new DeleteIgnoreUserTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.DeleteIgnoreUserResult> DeleteIgnoreUserAsync(
                Request.DeleteIgnoreUserRequest request
        ) =>
            new DeleteIgnoreUserTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class DeleteIgnoreUserByGuildNameTask : Gs2RestSessionTask<DeleteIgnoreUserByGuildNameRequest, DeleteIgnoreUserByGuildNameResult>
        {
            public DeleteIgnoreUserByGuildNameTask(IGs2Session session, RestSessionRequestFactory factory, DeleteIgnoreUserByGuildNameRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DeleteIgnoreUserByGuildNameRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "guild")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/guild/{guildModelName}/{guildName}/ignore/user/{userId}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{guildModelName}", !string.IsNullOrEmpty(request.GuildModelName) ? request.GuildModelName.ToString() : "null");
                url = url.Replace("{guildName}", !string.IsNullOrEmpty(request.GuildName) ? request.GuildName.ToString() : "null");
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
		public IEnumerator DeleteIgnoreUserByGuildName(
                Request.DeleteIgnoreUserByGuildNameRequest request,
                UnityAction<AsyncResult<Result.DeleteIgnoreUserByGuildNameResult>> callback
        ) =>
            new DeleteIgnoreUserByGuildNameTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.DeleteIgnoreUserByGuildNameResult> DeleteIgnoreUserByGuildNameFuture(
                Request.DeleteIgnoreUserByGuildNameRequest request
        ) =>
            new DeleteIgnoreUserByGuildNameTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.DeleteIgnoreUserByGuildNameResult> DeleteIgnoreUserByGuildNameAsync(
                Request.DeleteIgnoreUserByGuildNameRequest request
        ) =>
            new DeleteIgnoreUserByGuildNameTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.DeleteIgnoreUserByGuildNameResult>();
    #else
		public DeleteIgnoreUserByGuildNameTask DeleteIgnoreUserByGuildNameAsync(
                Request.DeleteIgnoreUserByGuildNameRequest request
        )
		{
			return new DeleteIgnoreUserByGuildNameTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.DeleteIgnoreUserByGuildNameResult> DeleteIgnoreUserByGuildNameAsync(
                Request.DeleteIgnoreUserByGuildNameRequest request
        ) =>
            new DeleteIgnoreUserByGuildNameTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif
	}
}