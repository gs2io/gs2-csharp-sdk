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
#if UNITY_2017_1_OR_NEWER
using UnityEngine.Events;
using UnityEngine.Networking;
    #if GS2_ENABLE_UNITASK
using Cysharp.Threading.Tasks;
    #endif
#else
using System.Web;
using System.Net.Http;
using System.Threading.Tasks;
#endif

using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Gs2.Core;
using Gs2.Core.Model;
using Gs2.Core.Net;
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
        )
		{
			var task = new DescribeNamespacesTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.DescribeNamespacesResult>(task.Result, task.Error));
        }

		public IFuture<Result.DescribeNamespacesResult> DescribeNamespacesFuture(
                Request.DescribeNamespacesRequest request
        )
		{
			return new DescribeNamespacesTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.DescribeNamespacesResult> DescribeNamespacesAsync(
                Request.DescribeNamespacesRequest request
        )
		{
            AsyncResult<Result.DescribeNamespacesResult> result = null;
			await DescribeNamespaces(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
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
		public async Task<Result.DescribeNamespacesResult> DescribeNamespacesAsync(
                Request.DescribeNamespacesRequest request
        )
		{
			var task = new DescribeNamespacesTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
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
        )
		{
			var task = new CreateNamespaceTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.CreateNamespaceResult>(task.Result, task.Error));
        }

		public IFuture<Result.CreateNamespaceResult> CreateNamespaceFuture(
                Request.CreateNamespaceRequest request
        )
		{
			return new CreateNamespaceTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.CreateNamespaceResult> CreateNamespaceAsync(
                Request.CreateNamespaceRequest request
        )
		{
            AsyncResult<Result.CreateNamespaceResult> result = null;
			await CreateNamespace(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
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
		public async Task<Result.CreateNamespaceResult> CreateNamespaceAsync(
                Request.CreateNamespaceRequest request
        )
		{
			var task = new CreateNamespaceTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
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
        )
		{
			var task = new GetNamespaceStatusTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.GetNamespaceStatusResult>(task.Result, task.Error));
        }

		public IFuture<Result.GetNamespaceStatusResult> GetNamespaceStatusFuture(
                Request.GetNamespaceStatusRequest request
        )
		{
			return new GetNamespaceStatusTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.GetNamespaceStatusResult> GetNamespaceStatusAsync(
                Request.GetNamespaceStatusRequest request
        )
		{
            AsyncResult<Result.GetNamespaceStatusResult> result = null;
			await GetNamespaceStatus(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
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
		public async Task<Result.GetNamespaceStatusResult> GetNamespaceStatusAsync(
                Request.GetNamespaceStatusRequest request
        )
		{
			var task = new GetNamespaceStatusTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
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
        )
		{
			var task = new GetNamespaceTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.GetNamespaceResult>(task.Result, task.Error));
        }

		public IFuture<Result.GetNamespaceResult> GetNamespaceFuture(
                Request.GetNamespaceRequest request
        )
		{
			return new GetNamespaceTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.GetNamespaceResult> GetNamespaceAsync(
                Request.GetNamespaceRequest request
        )
		{
            AsyncResult<Result.GetNamespaceResult> result = null;
			await GetNamespace(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
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
		public async Task<Result.GetNamespaceResult> GetNamespaceAsync(
                Request.GetNamespaceRequest request
        )
		{
			var task = new GetNamespaceTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
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
        )
		{
			var task = new UpdateNamespaceTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.UpdateNamespaceResult>(task.Result, task.Error));
        }

		public IFuture<Result.UpdateNamespaceResult> UpdateNamespaceFuture(
                Request.UpdateNamespaceRequest request
        )
		{
			return new UpdateNamespaceTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.UpdateNamespaceResult> UpdateNamespaceAsync(
                Request.UpdateNamespaceRequest request
        )
		{
            AsyncResult<Result.UpdateNamespaceResult> result = null;
			await UpdateNamespace(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
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
		public async Task<Result.UpdateNamespaceResult> UpdateNamespaceAsync(
                Request.UpdateNamespaceRequest request
        )
		{
			var task = new UpdateNamespaceTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
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
        )
		{
			var task = new DeleteNamespaceTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.DeleteNamespaceResult>(task.Result, task.Error));
        }

		public IFuture<Result.DeleteNamespaceResult> DeleteNamespaceFuture(
                Request.DeleteNamespaceRequest request
        )
		{
			return new DeleteNamespaceTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.DeleteNamespaceResult> DeleteNamespaceAsync(
                Request.DeleteNamespaceRequest request
        )
		{
            AsyncResult<Result.DeleteNamespaceResult> result = null;
			await DeleteNamespace(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
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
		public async Task<Result.DeleteNamespaceResult> DeleteNamespaceAsync(
                Request.DeleteNamespaceRequest request
        )
		{
			var task = new DeleteNamespaceTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
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
        )
		{
			var task = new DumpUserDataByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.DumpUserDataByUserIdResult>(task.Result, task.Error));
        }

		public IFuture<Result.DumpUserDataByUserIdResult> DumpUserDataByUserIdFuture(
                Request.DumpUserDataByUserIdRequest request
        )
		{
			return new DumpUserDataByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.DumpUserDataByUserIdResult> DumpUserDataByUserIdAsync(
                Request.DumpUserDataByUserIdRequest request
        )
		{
            AsyncResult<Result.DumpUserDataByUserIdResult> result = null;
			await DumpUserDataByUserId(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
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
		public async Task<Result.DumpUserDataByUserIdResult> DumpUserDataByUserIdAsync(
                Request.DumpUserDataByUserIdRequest request
        )
		{
			var task = new DumpUserDataByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
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
        )
		{
			var task = new CheckDumpUserDataByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.CheckDumpUserDataByUserIdResult>(task.Result, task.Error));
        }

		public IFuture<Result.CheckDumpUserDataByUserIdResult> CheckDumpUserDataByUserIdFuture(
                Request.CheckDumpUserDataByUserIdRequest request
        )
		{
			return new CheckDumpUserDataByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.CheckDumpUserDataByUserIdResult> CheckDumpUserDataByUserIdAsync(
                Request.CheckDumpUserDataByUserIdRequest request
        )
		{
            AsyncResult<Result.CheckDumpUserDataByUserIdResult> result = null;
			await CheckDumpUserDataByUserId(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
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
		public async Task<Result.CheckDumpUserDataByUserIdResult> CheckDumpUserDataByUserIdAsync(
                Request.CheckDumpUserDataByUserIdRequest request
        )
		{
			var task = new CheckDumpUserDataByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
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
        )
		{
			var task = new CleanUserDataByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.CleanUserDataByUserIdResult>(task.Result, task.Error));
        }

		public IFuture<Result.CleanUserDataByUserIdResult> CleanUserDataByUserIdFuture(
                Request.CleanUserDataByUserIdRequest request
        )
		{
			return new CleanUserDataByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.CleanUserDataByUserIdResult> CleanUserDataByUserIdAsync(
                Request.CleanUserDataByUserIdRequest request
        )
		{
            AsyncResult<Result.CleanUserDataByUserIdResult> result = null;
			await CleanUserDataByUserId(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
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
		public async Task<Result.CleanUserDataByUserIdResult> CleanUserDataByUserIdAsync(
                Request.CleanUserDataByUserIdRequest request
        )
		{
			var task = new CleanUserDataByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
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
        )
		{
			var task = new CheckCleanUserDataByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.CheckCleanUserDataByUserIdResult>(task.Result, task.Error));
        }

		public IFuture<Result.CheckCleanUserDataByUserIdResult> CheckCleanUserDataByUserIdFuture(
                Request.CheckCleanUserDataByUserIdRequest request
        )
		{
			return new CheckCleanUserDataByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.CheckCleanUserDataByUserIdResult> CheckCleanUserDataByUserIdAsync(
                Request.CheckCleanUserDataByUserIdRequest request
        )
		{
            AsyncResult<Result.CheckCleanUserDataByUserIdResult> result = null;
			await CheckCleanUserDataByUserId(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
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
		public async Task<Result.CheckCleanUserDataByUserIdResult> CheckCleanUserDataByUserIdAsync(
                Request.CheckCleanUserDataByUserIdRequest request
        )
		{
			var task = new CheckCleanUserDataByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
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
        )
		{
			var task = new PrepareImportUserDataByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.PrepareImportUserDataByUserIdResult>(task.Result, task.Error));
        }

		public IFuture<Result.PrepareImportUserDataByUserIdResult> PrepareImportUserDataByUserIdFuture(
                Request.PrepareImportUserDataByUserIdRequest request
        )
		{
			return new PrepareImportUserDataByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.PrepareImportUserDataByUserIdResult> PrepareImportUserDataByUserIdAsync(
                Request.PrepareImportUserDataByUserIdRequest request
        )
		{
            AsyncResult<Result.PrepareImportUserDataByUserIdResult> result = null;
			await PrepareImportUserDataByUserId(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
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
		public async Task<Result.PrepareImportUserDataByUserIdResult> PrepareImportUserDataByUserIdAsync(
                Request.PrepareImportUserDataByUserIdRequest request
        )
		{
			var task = new PrepareImportUserDataByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
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
        )
		{
			var task = new ImportUserDataByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.ImportUserDataByUserIdResult>(task.Result, task.Error));
        }

		public IFuture<Result.ImportUserDataByUserIdResult> ImportUserDataByUserIdFuture(
                Request.ImportUserDataByUserIdRequest request
        )
		{
			return new ImportUserDataByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.ImportUserDataByUserIdResult> ImportUserDataByUserIdAsync(
                Request.ImportUserDataByUserIdRequest request
        )
		{
            AsyncResult<Result.ImportUserDataByUserIdResult> result = null;
			await ImportUserDataByUserId(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
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
		public async Task<Result.ImportUserDataByUserIdResult> ImportUserDataByUserIdAsync(
                Request.ImportUserDataByUserIdRequest request
        )
		{
			var task = new ImportUserDataByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
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
        )
		{
			var task = new CheckImportUserDataByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.CheckImportUserDataByUserIdResult>(task.Result, task.Error));
        }

		public IFuture<Result.CheckImportUserDataByUserIdResult> CheckImportUserDataByUserIdFuture(
                Request.CheckImportUserDataByUserIdRequest request
        )
		{
			return new CheckImportUserDataByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.CheckImportUserDataByUserIdResult> CheckImportUserDataByUserIdAsync(
                Request.CheckImportUserDataByUserIdRequest request
        )
		{
            AsyncResult<Result.CheckImportUserDataByUserIdResult> result = null;
			await CheckImportUserDataByUserId(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
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
		public async Task<Result.CheckImportUserDataByUserIdResult> CheckImportUserDataByUserIdAsync(
                Request.CheckImportUserDataByUserIdRequest request
        )
		{
			var task = new CheckImportUserDataByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
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
        )
		{
			var task = new DescribeGuildModelMastersTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.DescribeGuildModelMastersResult>(task.Result, task.Error));
        }

		public IFuture<Result.DescribeGuildModelMastersResult> DescribeGuildModelMastersFuture(
                Request.DescribeGuildModelMastersRequest request
        )
		{
			return new DescribeGuildModelMastersTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.DescribeGuildModelMastersResult> DescribeGuildModelMastersAsync(
                Request.DescribeGuildModelMastersRequest request
        )
		{
            AsyncResult<Result.DescribeGuildModelMastersResult> result = null;
			await DescribeGuildModelMasters(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
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
		public async Task<Result.DescribeGuildModelMastersResult> DescribeGuildModelMastersAsync(
                Request.DescribeGuildModelMastersRequest request
        )
		{
			var task = new DescribeGuildModelMastersTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
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
        )
		{
			var task = new CreateGuildModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.CreateGuildModelMasterResult>(task.Result, task.Error));
        }

		public IFuture<Result.CreateGuildModelMasterResult> CreateGuildModelMasterFuture(
                Request.CreateGuildModelMasterRequest request
        )
		{
			return new CreateGuildModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.CreateGuildModelMasterResult> CreateGuildModelMasterAsync(
                Request.CreateGuildModelMasterRequest request
        )
		{
            AsyncResult<Result.CreateGuildModelMasterResult> result = null;
			await CreateGuildModelMaster(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
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
		public async Task<Result.CreateGuildModelMasterResult> CreateGuildModelMasterAsync(
                Request.CreateGuildModelMasterRequest request
        )
		{
			var task = new CreateGuildModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
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
        )
		{
			var task = new GetGuildModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.GetGuildModelMasterResult>(task.Result, task.Error));
        }

		public IFuture<Result.GetGuildModelMasterResult> GetGuildModelMasterFuture(
                Request.GetGuildModelMasterRequest request
        )
		{
			return new GetGuildModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.GetGuildModelMasterResult> GetGuildModelMasterAsync(
                Request.GetGuildModelMasterRequest request
        )
		{
            AsyncResult<Result.GetGuildModelMasterResult> result = null;
			await GetGuildModelMaster(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
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
		public async Task<Result.GetGuildModelMasterResult> GetGuildModelMasterAsync(
                Request.GetGuildModelMasterRequest request
        )
		{
			var task = new GetGuildModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
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
        )
		{
			var task = new UpdateGuildModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.UpdateGuildModelMasterResult>(task.Result, task.Error));
        }

		public IFuture<Result.UpdateGuildModelMasterResult> UpdateGuildModelMasterFuture(
                Request.UpdateGuildModelMasterRequest request
        )
		{
			return new UpdateGuildModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.UpdateGuildModelMasterResult> UpdateGuildModelMasterAsync(
                Request.UpdateGuildModelMasterRequest request
        )
		{
            AsyncResult<Result.UpdateGuildModelMasterResult> result = null;
			await UpdateGuildModelMaster(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
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
		public async Task<Result.UpdateGuildModelMasterResult> UpdateGuildModelMasterAsync(
                Request.UpdateGuildModelMasterRequest request
        )
		{
			var task = new UpdateGuildModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
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
        )
		{
			var task = new DeleteGuildModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.DeleteGuildModelMasterResult>(task.Result, task.Error));
        }

		public IFuture<Result.DeleteGuildModelMasterResult> DeleteGuildModelMasterFuture(
                Request.DeleteGuildModelMasterRequest request
        )
		{
			return new DeleteGuildModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.DeleteGuildModelMasterResult> DeleteGuildModelMasterAsync(
                Request.DeleteGuildModelMasterRequest request
        )
		{
            AsyncResult<Result.DeleteGuildModelMasterResult> result = null;
			await DeleteGuildModelMaster(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
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
		public async Task<Result.DeleteGuildModelMasterResult> DeleteGuildModelMasterAsync(
                Request.DeleteGuildModelMasterRequest request
        )
		{
			var task = new DeleteGuildModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
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
        )
		{
			var task = new DescribeGuildModelsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.DescribeGuildModelsResult>(task.Result, task.Error));
        }

		public IFuture<Result.DescribeGuildModelsResult> DescribeGuildModelsFuture(
                Request.DescribeGuildModelsRequest request
        )
		{
			return new DescribeGuildModelsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.DescribeGuildModelsResult> DescribeGuildModelsAsync(
                Request.DescribeGuildModelsRequest request
        )
		{
            AsyncResult<Result.DescribeGuildModelsResult> result = null;
			await DescribeGuildModels(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
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
		public async Task<Result.DescribeGuildModelsResult> DescribeGuildModelsAsync(
                Request.DescribeGuildModelsRequest request
        )
		{
			var task = new DescribeGuildModelsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
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
        )
		{
			var task = new GetGuildModelTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.GetGuildModelResult>(task.Result, task.Error));
        }

		public IFuture<Result.GetGuildModelResult> GetGuildModelFuture(
                Request.GetGuildModelRequest request
        )
		{
			return new GetGuildModelTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.GetGuildModelResult> GetGuildModelAsync(
                Request.GetGuildModelRequest request
        )
		{
            AsyncResult<Result.GetGuildModelResult> result = null;
			await GetGuildModel(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
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
		public async Task<Result.GetGuildModelResult> GetGuildModelAsync(
                Request.GetGuildModelRequest request
        )
		{
			var task = new GetGuildModelTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
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
        )
		{
			var task = new SearchGuildsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.SearchGuildsResult>(task.Result, task.Error));
        }

		public IFuture<Result.SearchGuildsResult> SearchGuildsFuture(
                Request.SearchGuildsRequest request
        )
		{
			return new SearchGuildsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.SearchGuildsResult> SearchGuildsAsync(
                Request.SearchGuildsRequest request
        )
		{
            AsyncResult<Result.SearchGuildsResult> result = null;
			await SearchGuilds(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
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
		public async Task<Result.SearchGuildsResult> SearchGuildsAsync(
                Request.SearchGuildsRequest request
        )
		{
			var task = new SearchGuildsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
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
        )
		{
			var task = new SearchGuildsByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.SearchGuildsByUserIdResult>(task.Result, task.Error));
        }

		public IFuture<Result.SearchGuildsByUserIdResult> SearchGuildsByUserIdFuture(
                Request.SearchGuildsByUserIdRequest request
        )
		{
			return new SearchGuildsByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.SearchGuildsByUserIdResult> SearchGuildsByUserIdAsync(
                Request.SearchGuildsByUserIdRequest request
        )
		{
            AsyncResult<Result.SearchGuildsByUserIdResult> result = null;
			await SearchGuildsByUserId(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
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
		public async Task<Result.SearchGuildsByUserIdResult> SearchGuildsByUserIdAsync(
                Request.SearchGuildsByUserIdRequest request
        )
		{
			var task = new SearchGuildsByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
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
        )
		{
			var task = new CreateGuildTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.CreateGuildResult>(task.Result, task.Error));
        }

		public IFuture<Result.CreateGuildResult> CreateGuildFuture(
                Request.CreateGuildRequest request
        )
		{
			return new CreateGuildTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.CreateGuildResult> CreateGuildAsync(
                Request.CreateGuildRequest request
        )
		{
            AsyncResult<Result.CreateGuildResult> result = null;
			await CreateGuild(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
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
		public async Task<Result.CreateGuildResult> CreateGuildAsync(
                Request.CreateGuildRequest request
        )
		{
			var task = new CreateGuildTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
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
        )
		{
			var task = new CreateGuildByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.CreateGuildByUserIdResult>(task.Result, task.Error));
        }

		public IFuture<Result.CreateGuildByUserIdResult> CreateGuildByUserIdFuture(
                Request.CreateGuildByUserIdRequest request
        )
		{
			return new CreateGuildByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.CreateGuildByUserIdResult> CreateGuildByUserIdAsync(
                Request.CreateGuildByUserIdRequest request
        )
		{
            AsyncResult<Result.CreateGuildByUserIdResult> result = null;
			await CreateGuildByUserId(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
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
		public async Task<Result.CreateGuildByUserIdResult> CreateGuildByUserIdAsync(
                Request.CreateGuildByUserIdRequest request
        )
		{
			var task = new CreateGuildByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
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
        )
		{
			var task = new GetGuildTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.GetGuildResult>(task.Result, task.Error));
        }

		public IFuture<Result.GetGuildResult> GetGuildFuture(
                Request.GetGuildRequest request
        )
		{
			return new GetGuildTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.GetGuildResult> GetGuildAsync(
                Request.GetGuildRequest request
        )
		{
            AsyncResult<Result.GetGuildResult> result = null;
			await GetGuild(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
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
		public async Task<Result.GetGuildResult> GetGuildAsync(
                Request.GetGuildRequest request
        )
		{
			var task = new GetGuildTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
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
        )
		{
			var task = new GetGuildByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.GetGuildByUserIdResult>(task.Result, task.Error));
        }

		public IFuture<Result.GetGuildByUserIdResult> GetGuildByUserIdFuture(
                Request.GetGuildByUserIdRequest request
        )
		{
			return new GetGuildByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.GetGuildByUserIdResult> GetGuildByUserIdAsync(
                Request.GetGuildByUserIdRequest request
        )
		{
            AsyncResult<Result.GetGuildByUserIdResult> result = null;
			await GetGuildByUserId(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
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
		public async Task<Result.GetGuildByUserIdResult> GetGuildByUserIdAsync(
                Request.GetGuildByUserIdRequest request
        )
		{
			var task = new GetGuildByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
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
        )
		{
			var task = new UpdateGuildTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.UpdateGuildResult>(task.Result, task.Error));
        }

		public IFuture<Result.UpdateGuildResult> UpdateGuildFuture(
                Request.UpdateGuildRequest request
        )
		{
			return new UpdateGuildTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.UpdateGuildResult> UpdateGuildAsync(
                Request.UpdateGuildRequest request
        )
		{
            AsyncResult<Result.UpdateGuildResult> result = null;
			await UpdateGuild(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
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
		public async Task<Result.UpdateGuildResult> UpdateGuildAsync(
                Request.UpdateGuildRequest request
        )
		{
			var task = new UpdateGuildTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
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
        )
		{
			var task = new UpdateGuildByGuildNameTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.UpdateGuildByGuildNameResult>(task.Result, task.Error));
        }

		public IFuture<Result.UpdateGuildByGuildNameResult> UpdateGuildByGuildNameFuture(
                Request.UpdateGuildByGuildNameRequest request
        )
		{
			return new UpdateGuildByGuildNameTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.UpdateGuildByGuildNameResult> UpdateGuildByGuildNameAsync(
                Request.UpdateGuildByGuildNameRequest request
        )
		{
            AsyncResult<Result.UpdateGuildByGuildNameResult> result = null;
			await UpdateGuildByGuildName(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
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
		public async Task<Result.UpdateGuildByGuildNameResult> UpdateGuildByGuildNameAsync(
                Request.UpdateGuildByGuildNameRequest request
        )
		{
			var task = new UpdateGuildByGuildNameTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
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
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator DeleteMember(
                Request.DeleteMemberRequest request,
                UnityAction<AsyncResult<Result.DeleteMemberResult>> callback
        )
		{
			var task = new DeleteMemberTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.DeleteMemberResult>(task.Result, task.Error));
        }

		public IFuture<Result.DeleteMemberResult> DeleteMemberFuture(
                Request.DeleteMemberRequest request
        )
		{
			return new DeleteMemberTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.DeleteMemberResult> DeleteMemberAsync(
                Request.DeleteMemberRequest request
        )
		{
            AsyncResult<Result.DeleteMemberResult> result = null;
			await DeleteMember(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
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
		public async Task<Result.DeleteMemberResult> DeleteMemberAsync(
                Request.DeleteMemberRequest request
        )
		{
			var task = new DeleteMemberTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
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
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator DeleteMemberByGuildName(
                Request.DeleteMemberByGuildNameRequest request,
                UnityAction<AsyncResult<Result.DeleteMemberByGuildNameResult>> callback
        )
		{
			var task = new DeleteMemberByGuildNameTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.DeleteMemberByGuildNameResult>(task.Result, task.Error));
        }

		public IFuture<Result.DeleteMemberByGuildNameResult> DeleteMemberByGuildNameFuture(
                Request.DeleteMemberByGuildNameRequest request
        )
		{
			return new DeleteMemberByGuildNameTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.DeleteMemberByGuildNameResult> DeleteMemberByGuildNameAsync(
                Request.DeleteMemberByGuildNameRequest request
        )
		{
            AsyncResult<Result.DeleteMemberByGuildNameResult> result = null;
			await DeleteMemberByGuildName(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
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
		public async Task<Result.DeleteMemberByGuildNameResult> DeleteMemberByGuildNameAsync(
                Request.DeleteMemberByGuildNameRequest request
        )
		{
			var task = new DeleteMemberByGuildNameTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
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
        )
		{
			var task = new UpdateMemberRoleTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.UpdateMemberRoleResult>(task.Result, task.Error));
        }

		public IFuture<Result.UpdateMemberRoleResult> UpdateMemberRoleFuture(
                Request.UpdateMemberRoleRequest request
        )
		{
			return new UpdateMemberRoleTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.UpdateMemberRoleResult> UpdateMemberRoleAsync(
                Request.UpdateMemberRoleRequest request
        )
		{
            AsyncResult<Result.UpdateMemberRoleResult> result = null;
			await UpdateMemberRole(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
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
		public async Task<Result.UpdateMemberRoleResult> UpdateMemberRoleAsync(
                Request.UpdateMemberRoleRequest request
        )
		{
			var task = new UpdateMemberRoleTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
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
        )
		{
			var task = new UpdateMemberRoleByGuildNameTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.UpdateMemberRoleByGuildNameResult>(task.Result, task.Error));
        }

		public IFuture<Result.UpdateMemberRoleByGuildNameResult> UpdateMemberRoleByGuildNameFuture(
                Request.UpdateMemberRoleByGuildNameRequest request
        )
		{
			return new UpdateMemberRoleByGuildNameTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.UpdateMemberRoleByGuildNameResult> UpdateMemberRoleByGuildNameAsync(
                Request.UpdateMemberRoleByGuildNameRequest request
        )
		{
            AsyncResult<Result.UpdateMemberRoleByGuildNameResult> result = null;
			await UpdateMemberRoleByGuildName(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
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
		public async Task<Result.UpdateMemberRoleByGuildNameResult> UpdateMemberRoleByGuildNameAsync(
                Request.UpdateMemberRoleByGuildNameRequest request
        )
		{
			var task = new UpdateMemberRoleByGuildNameTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
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
        )
		{
			var task = new BatchUpdateMemberRoleTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.BatchUpdateMemberRoleResult>(task.Result, task.Error));
        }

		public IFuture<Result.BatchUpdateMemberRoleResult> BatchUpdateMemberRoleFuture(
                Request.BatchUpdateMemberRoleRequest request
        )
		{
			return new BatchUpdateMemberRoleTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.BatchUpdateMemberRoleResult> BatchUpdateMemberRoleAsync(
                Request.BatchUpdateMemberRoleRequest request
        )
		{
            AsyncResult<Result.BatchUpdateMemberRoleResult> result = null;
			await BatchUpdateMemberRole(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
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
		public async Task<Result.BatchUpdateMemberRoleResult> BatchUpdateMemberRoleAsync(
                Request.BatchUpdateMemberRoleRequest request
        )
		{
			var task = new BatchUpdateMemberRoleTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
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
        )
		{
			var task = new BatchUpdateMemberRoleByGuildNameTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.BatchUpdateMemberRoleByGuildNameResult>(task.Result, task.Error));
        }

		public IFuture<Result.BatchUpdateMemberRoleByGuildNameResult> BatchUpdateMemberRoleByGuildNameFuture(
                Request.BatchUpdateMemberRoleByGuildNameRequest request
        )
		{
			return new BatchUpdateMemberRoleByGuildNameTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.BatchUpdateMemberRoleByGuildNameResult> BatchUpdateMemberRoleByGuildNameAsync(
                Request.BatchUpdateMemberRoleByGuildNameRequest request
        )
		{
            AsyncResult<Result.BatchUpdateMemberRoleByGuildNameResult> result = null;
			await BatchUpdateMemberRoleByGuildName(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
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
		public async Task<Result.BatchUpdateMemberRoleByGuildNameResult> BatchUpdateMemberRoleByGuildNameAsync(
                Request.BatchUpdateMemberRoleByGuildNameRequest request
        )
		{
			var task = new BatchUpdateMemberRoleByGuildNameTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
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
        )
		{
			var task = new DeleteGuildTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.DeleteGuildResult>(task.Result, task.Error));
        }

		public IFuture<Result.DeleteGuildResult> DeleteGuildFuture(
                Request.DeleteGuildRequest request
        )
		{
			return new DeleteGuildTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.DeleteGuildResult> DeleteGuildAsync(
                Request.DeleteGuildRequest request
        )
		{
            AsyncResult<Result.DeleteGuildResult> result = null;
			await DeleteGuild(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
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
		public async Task<Result.DeleteGuildResult> DeleteGuildAsync(
                Request.DeleteGuildRequest request
        )
		{
			var task = new DeleteGuildTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
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
        )
		{
			var task = new DeleteGuildByGuildNameTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.DeleteGuildByGuildNameResult>(task.Result, task.Error));
        }

		public IFuture<Result.DeleteGuildByGuildNameResult> DeleteGuildByGuildNameFuture(
                Request.DeleteGuildByGuildNameRequest request
        )
		{
			return new DeleteGuildByGuildNameTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.DeleteGuildByGuildNameResult> DeleteGuildByGuildNameAsync(
                Request.DeleteGuildByGuildNameRequest request
        )
		{
            AsyncResult<Result.DeleteGuildByGuildNameResult> result = null;
			await DeleteGuildByGuildName(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
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
		public async Task<Result.DeleteGuildByGuildNameResult> DeleteGuildByGuildNameAsync(
                Request.DeleteGuildByGuildNameRequest request
        )
		{
			var task = new DeleteGuildByGuildNameTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
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
        )
		{
			var task = new IncreaseMaximumCurrentMaximumMemberCountByGuildNameTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.IncreaseMaximumCurrentMaximumMemberCountByGuildNameResult>(task.Result, task.Error));
        }

		public IFuture<Result.IncreaseMaximumCurrentMaximumMemberCountByGuildNameResult> IncreaseMaximumCurrentMaximumMemberCountByGuildNameFuture(
                Request.IncreaseMaximumCurrentMaximumMemberCountByGuildNameRequest request
        )
		{
			return new IncreaseMaximumCurrentMaximumMemberCountByGuildNameTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.IncreaseMaximumCurrentMaximumMemberCountByGuildNameResult> IncreaseMaximumCurrentMaximumMemberCountByGuildNameAsync(
                Request.IncreaseMaximumCurrentMaximumMemberCountByGuildNameRequest request
        )
		{
            AsyncResult<Result.IncreaseMaximumCurrentMaximumMemberCountByGuildNameResult> result = null;
			await IncreaseMaximumCurrentMaximumMemberCountByGuildName(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
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
		public async Task<Result.IncreaseMaximumCurrentMaximumMemberCountByGuildNameResult> IncreaseMaximumCurrentMaximumMemberCountByGuildNameAsync(
                Request.IncreaseMaximumCurrentMaximumMemberCountByGuildNameRequest request
        )
		{
			var task = new IncreaseMaximumCurrentMaximumMemberCountByGuildNameTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
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
        )
		{
			var task = new DecreaseMaximumCurrentMaximumMemberCountTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.DecreaseMaximumCurrentMaximumMemberCountResult>(task.Result, task.Error));
        }

		public IFuture<Result.DecreaseMaximumCurrentMaximumMemberCountResult> DecreaseMaximumCurrentMaximumMemberCountFuture(
                Request.DecreaseMaximumCurrentMaximumMemberCountRequest request
        )
		{
			return new DecreaseMaximumCurrentMaximumMemberCountTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.DecreaseMaximumCurrentMaximumMemberCountResult> DecreaseMaximumCurrentMaximumMemberCountAsync(
                Request.DecreaseMaximumCurrentMaximumMemberCountRequest request
        )
		{
            AsyncResult<Result.DecreaseMaximumCurrentMaximumMemberCountResult> result = null;
			await DecreaseMaximumCurrentMaximumMemberCount(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
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
		public async Task<Result.DecreaseMaximumCurrentMaximumMemberCountResult> DecreaseMaximumCurrentMaximumMemberCountAsync(
                Request.DecreaseMaximumCurrentMaximumMemberCountRequest request
        )
		{
			var task = new DecreaseMaximumCurrentMaximumMemberCountTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
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
        )
		{
			var task = new DecreaseMaximumCurrentMaximumMemberCountByGuildNameTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.DecreaseMaximumCurrentMaximumMemberCountByGuildNameResult>(task.Result, task.Error));
        }

		public IFuture<Result.DecreaseMaximumCurrentMaximumMemberCountByGuildNameResult> DecreaseMaximumCurrentMaximumMemberCountByGuildNameFuture(
                Request.DecreaseMaximumCurrentMaximumMemberCountByGuildNameRequest request
        )
		{
			return new DecreaseMaximumCurrentMaximumMemberCountByGuildNameTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.DecreaseMaximumCurrentMaximumMemberCountByGuildNameResult> DecreaseMaximumCurrentMaximumMemberCountByGuildNameAsync(
                Request.DecreaseMaximumCurrentMaximumMemberCountByGuildNameRequest request
        )
		{
            AsyncResult<Result.DecreaseMaximumCurrentMaximumMemberCountByGuildNameResult> result = null;
			await DecreaseMaximumCurrentMaximumMemberCountByGuildName(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
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
		public async Task<Result.DecreaseMaximumCurrentMaximumMemberCountByGuildNameResult> DecreaseMaximumCurrentMaximumMemberCountByGuildNameAsync(
                Request.DecreaseMaximumCurrentMaximumMemberCountByGuildNameRequest request
        )
		{
			var task = new DecreaseMaximumCurrentMaximumMemberCountByGuildNameTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
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
        )
		{
			var task = new VerifyCurrentMaximumMemberCountTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.VerifyCurrentMaximumMemberCountResult>(task.Result, task.Error));
        }

		public IFuture<Result.VerifyCurrentMaximumMemberCountResult> VerifyCurrentMaximumMemberCountFuture(
                Request.VerifyCurrentMaximumMemberCountRequest request
        )
		{
			return new VerifyCurrentMaximumMemberCountTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.VerifyCurrentMaximumMemberCountResult> VerifyCurrentMaximumMemberCountAsync(
                Request.VerifyCurrentMaximumMemberCountRequest request
        )
		{
            AsyncResult<Result.VerifyCurrentMaximumMemberCountResult> result = null;
			await VerifyCurrentMaximumMemberCount(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
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
		public async Task<Result.VerifyCurrentMaximumMemberCountResult> VerifyCurrentMaximumMemberCountAsync(
                Request.VerifyCurrentMaximumMemberCountRequest request
        )
		{
			var task = new VerifyCurrentMaximumMemberCountTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
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
        )
		{
			var task = new VerifyCurrentMaximumMemberCountByGuildNameTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.VerifyCurrentMaximumMemberCountByGuildNameResult>(task.Result, task.Error));
        }

		public IFuture<Result.VerifyCurrentMaximumMemberCountByGuildNameResult> VerifyCurrentMaximumMemberCountByGuildNameFuture(
                Request.VerifyCurrentMaximumMemberCountByGuildNameRequest request
        )
		{
			return new VerifyCurrentMaximumMemberCountByGuildNameTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.VerifyCurrentMaximumMemberCountByGuildNameResult> VerifyCurrentMaximumMemberCountByGuildNameAsync(
                Request.VerifyCurrentMaximumMemberCountByGuildNameRequest request
        )
		{
            AsyncResult<Result.VerifyCurrentMaximumMemberCountByGuildNameResult> result = null;
			await VerifyCurrentMaximumMemberCountByGuildName(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
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
		public async Task<Result.VerifyCurrentMaximumMemberCountByGuildNameResult> VerifyCurrentMaximumMemberCountByGuildNameAsync(
                Request.VerifyCurrentMaximumMemberCountByGuildNameRequest request
        )
		{
			var task = new VerifyCurrentMaximumMemberCountByGuildNameTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
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
        )
		{
			var task = new VerifyIncludeMemberTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.VerifyIncludeMemberResult>(task.Result, task.Error));
        }

		public IFuture<Result.VerifyIncludeMemberResult> VerifyIncludeMemberFuture(
                Request.VerifyIncludeMemberRequest request
        )
		{
			return new VerifyIncludeMemberTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.VerifyIncludeMemberResult> VerifyIncludeMemberAsync(
                Request.VerifyIncludeMemberRequest request
        )
		{
            AsyncResult<Result.VerifyIncludeMemberResult> result = null;
			await VerifyIncludeMember(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
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
		public async Task<Result.VerifyIncludeMemberResult> VerifyIncludeMemberAsync(
                Request.VerifyIncludeMemberRequest request
        )
		{
			var task = new VerifyIncludeMemberTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
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
        )
		{
			var task = new VerifyIncludeMemberByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.VerifyIncludeMemberByUserIdResult>(task.Result, task.Error));
        }

		public IFuture<Result.VerifyIncludeMemberByUserIdResult> VerifyIncludeMemberByUserIdFuture(
                Request.VerifyIncludeMemberByUserIdRequest request
        )
		{
			return new VerifyIncludeMemberByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.VerifyIncludeMemberByUserIdResult> VerifyIncludeMemberByUserIdAsync(
                Request.VerifyIncludeMemberByUserIdRequest request
        )
		{
            AsyncResult<Result.VerifyIncludeMemberByUserIdResult> result = null;
			await VerifyIncludeMemberByUserId(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
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
		public async Task<Result.VerifyIncludeMemberByUserIdResult> VerifyIncludeMemberByUserIdAsync(
                Request.VerifyIncludeMemberByUserIdRequest request
        )
		{
			var task = new VerifyIncludeMemberByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
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
        )
		{
			var task = new SetMaximumCurrentMaximumMemberCountByGuildNameTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.SetMaximumCurrentMaximumMemberCountByGuildNameResult>(task.Result, task.Error));
        }

		public IFuture<Result.SetMaximumCurrentMaximumMemberCountByGuildNameResult> SetMaximumCurrentMaximumMemberCountByGuildNameFuture(
                Request.SetMaximumCurrentMaximumMemberCountByGuildNameRequest request
        )
		{
			return new SetMaximumCurrentMaximumMemberCountByGuildNameTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.SetMaximumCurrentMaximumMemberCountByGuildNameResult> SetMaximumCurrentMaximumMemberCountByGuildNameAsync(
                Request.SetMaximumCurrentMaximumMemberCountByGuildNameRequest request
        )
		{
            AsyncResult<Result.SetMaximumCurrentMaximumMemberCountByGuildNameResult> result = null;
			await SetMaximumCurrentMaximumMemberCountByGuildName(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
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
		public async Task<Result.SetMaximumCurrentMaximumMemberCountByGuildNameResult> SetMaximumCurrentMaximumMemberCountByGuildNameAsync(
                Request.SetMaximumCurrentMaximumMemberCountByGuildNameRequest request
        )
		{
			var task = new SetMaximumCurrentMaximumMemberCountByGuildNameTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
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
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator Assume(
                Request.AssumeRequest request,
                UnityAction<AsyncResult<Result.AssumeResult>> callback
        )
		{
			var task = new AssumeTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.AssumeResult>(task.Result, task.Error));
        }

		public IFuture<Result.AssumeResult> AssumeFuture(
                Request.AssumeRequest request
        )
		{
			return new AssumeTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.AssumeResult> AssumeAsync(
                Request.AssumeRequest request
        )
		{
            AsyncResult<Result.AssumeResult> result = null;
			await Assume(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
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
		public async Task<Result.AssumeResult> AssumeAsync(
                Request.AssumeRequest request
        )
		{
			var task = new AssumeTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
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
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator AssumeByUserId(
                Request.AssumeByUserIdRequest request,
                UnityAction<AsyncResult<Result.AssumeByUserIdResult>> callback
        )
		{
			var task = new AssumeByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.AssumeByUserIdResult>(task.Result, task.Error));
        }

		public IFuture<Result.AssumeByUserIdResult> AssumeByUserIdFuture(
                Request.AssumeByUserIdRequest request
        )
		{
			return new AssumeByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.AssumeByUserIdResult> AssumeByUserIdAsync(
                Request.AssumeByUserIdRequest request
        )
		{
            AsyncResult<Result.AssumeByUserIdResult> result = null;
			await AssumeByUserId(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
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
		public async Task<Result.AssumeByUserIdResult> AssumeByUserIdAsync(
                Request.AssumeByUserIdRequest request
        )
		{
			var task = new AssumeByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
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
        )
		{
			var task = new IncreaseMaximumCurrentMaximumMemberCountByStampSheetTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.IncreaseMaximumCurrentMaximumMemberCountByStampSheetResult>(task.Result, task.Error));
        }

		public IFuture<Result.IncreaseMaximumCurrentMaximumMemberCountByStampSheetResult> IncreaseMaximumCurrentMaximumMemberCountByStampSheetFuture(
                Request.IncreaseMaximumCurrentMaximumMemberCountByStampSheetRequest request
        )
		{
			return new IncreaseMaximumCurrentMaximumMemberCountByStampSheetTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.IncreaseMaximumCurrentMaximumMemberCountByStampSheetResult> IncreaseMaximumCurrentMaximumMemberCountByStampSheetAsync(
                Request.IncreaseMaximumCurrentMaximumMemberCountByStampSheetRequest request
        )
		{
            AsyncResult<Result.IncreaseMaximumCurrentMaximumMemberCountByStampSheetResult> result = null;
			await IncreaseMaximumCurrentMaximumMemberCountByStampSheet(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
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
		public async Task<Result.IncreaseMaximumCurrentMaximumMemberCountByStampSheetResult> IncreaseMaximumCurrentMaximumMemberCountByStampSheetAsync(
                Request.IncreaseMaximumCurrentMaximumMemberCountByStampSheetRequest request
        )
		{
			var task = new IncreaseMaximumCurrentMaximumMemberCountByStampSheetTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
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
        )
		{
			var task = new DecreaseMaximumCurrentMaximumMemberCountByStampTaskTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.DecreaseMaximumCurrentMaximumMemberCountByStampTaskResult>(task.Result, task.Error));
        }

		public IFuture<Result.DecreaseMaximumCurrentMaximumMemberCountByStampTaskResult> DecreaseMaximumCurrentMaximumMemberCountByStampTaskFuture(
                Request.DecreaseMaximumCurrentMaximumMemberCountByStampTaskRequest request
        )
		{
			return new DecreaseMaximumCurrentMaximumMemberCountByStampTaskTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.DecreaseMaximumCurrentMaximumMemberCountByStampTaskResult> DecreaseMaximumCurrentMaximumMemberCountByStampTaskAsync(
                Request.DecreaseMaximumCurrentMaximumMemberCountByStampTaskRequest request
        )
		{
            AsyncResult<Result.DecreaseMaximumCurrentMaximumMemberCountByStampTaskResult> result = null;
			await DecreaseMaximumCurrentMaximumMemberCountByStampTask(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
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
		public async Task<Result.DecreaseMaximumCurrentMaximumMemberCountByStampTaskResult> DecreaseMaximumCurrentMaximumMemberCountByStampTaskAsync(
                Request.DecreaseMaximumCurrentMaximumMemberCountByStampTaskRequest request
        )
		{
			var task = new DecreaseMaximumCurrentMaximumMemberCountByStampTaskTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
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
        )
		{
			var task = new SetMaximumCurrentMaximumMemberCountByStampSheetTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.SetMaximumCurrentMaximumMemberCountByStampSheetResult>(task.Result, task.Error));
        }

		public IFuture<Result.SetMaximumCurrentMaximumMemberCountByStampSheetResult> SetMaximumCurrentMaximumMemberCountByStampSheetFuture(
                Request.SetMaximumCurrentMaximumMemberCountByStampSheetRequest request
        )
		{
			return new SetMaximumCurrentMaximumMemberCountByStampSheetTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.SetMaximumCurrentMaximumMemberCountByStampSheetResult> SetMaximumCurrentMaximumMemberCountByStampSheetAsync(
                Request.SetMaximumCurrentMaximumMemberCountByStampSheetRequest request
        )
		{
            AsyncResult<Result.SetMaximumCurrentMaximumMemberCountByStampSheetResult> result = null;
			await SetMaximumCurrentMaximumMemberCountByStampSheet(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
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
		public async Task<Result.SetMaximumCurrentMaximumMemberCountByStampSheetResult> SetMaximumCurrentMaximumMemberCountByStampSheetAsync(
                Request.SetMaximumCurrentMaximumMemberCountByStampSheetRequest request
        )
		{
			var task = new SetMaximumCurrentMaximumMemberCountByStampSheetTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
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
        )
		{
			var task = new VerifyCurrentMaximumMemberCountByStampTaskTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.VerifyCurrentMaximumMemberCountByStampTaskResult>(task.Result, task.Error));
        }

		public IFuture<Result.VerifyCurrentMaximumMemberCountByStampTaskResult> VerifyCurrentMaximumMemberCountByStampTaskFuture(
                Request.VerifyCurrentMaximumMemberCountByStampTaskRequest request
        )
		{
			return new VerifyCurrentMaximumMemberCountByStampTaskTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.VerifyCurrentMaximumMemberCountByStampTaskResult> VerifyCurrentMaximumMemberCountByStampTaskAsync(
                Request.VerifyCurrentMaximumMemberCountByStampTaskRequest request
        )
		{
            AsyncResult<Result.VerifyCurrentMaximumMemberCountByStampTaskResult> result = null;
			await VerifyCurrentMaximumMemberCountByStampTask(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
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
		public async Task<Result.VerifyCurrentMaximumMemberCountByStampTaskResult> VerifyCurrentMaximumMemberCountByStampTaskAsync(
                Request.VerifyCurrentMaximumMemberCountByStampTaskRequest request
        )
		{
			var task = new VerifyCurrentMaximumMemberCountByStampTaskTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
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
        )
		{
			var task = new VerifyIncludeMemberByStampTaskTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.VerifyIncludeMemberByStampTaskResult>(task.Result, task.Error));
        }

		public IFuture<Result.VerifyIncludeMemberByStampTaskResult> VerifyIncludeMemberByStampTaskFuture(
                Request.VerifyIncludeMemberByStampTaskRequest request
        )
		{
			return new VerifyIncludeMemberByStampTaskTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.VerifyIncludeMemberByStampTaskResult> VerifyIncludeMemberByStampTaskAsync(
                Request.VerifyIncludeMemberByStampTaskRequest request
        )
		{
            AsyncResult<Result.VerifyIncludeMemberByStampTaskResult> result = null;
			await VerifyIncludeMemberByStampTask(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
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
		public async Task<Result.VerifyIncludeMemberByStampTaskResult> VerifyIncludeMemberByStampTaskAsync(
                Request.VerifyIncludeMemberByStampTaskRequest request
        )
		{
			var task = new VerifyIncludeMemberByStampTaskTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
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
        )
		{
			var task = new DescribeJoinedGuildsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.DescribeJoinedGuildsResult>(task.Result, task.Error));
        }

		public IFuture<Result.DescribeJoinedGuildsResult> DescribeJoinedGuildsFuture(
                Request.DescribeJoinedGuildsRequest request
        )
		{
			return new DescribeJoinedGuildsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.DescribeJoinedGuildsResult> DescribeJoinedGuildsAsync(
                Request.DescribeJoinedGuildsRequest request
        )
		{
            AsyncResult<Result.DescribeJoinedGuildsResult> result = null;
			await DescribeJoinedGuilds(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
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
		public async Task<Result.DescribeJoinedGuildsResult> DescribeJoinedGuildsAsync(
                Request.DescribeJoinedGuildsRequest request
        )
		{
			var task = new DescribeJoinedGuildsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
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
        )
		{
			var task = new DescribeJoinedGuildsByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.DescribeJoinedGuildsByUserIdResult>(task.Result, task.Error));
        }

		public IFuture<Result.DescribeJoinedGuildsByUserIdResult> DescribeJoinedGuildsByUserIdFuture(
                Request.DescribeJoinedGuildsByUserIdRequest request
        )
		{
			return new DescribeJoinedGuildsByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.DescribeJoinedGuildsByUserIdResult> DescribeJoinedGuildsByUserIdAsync(
                Request.DescribeJoinedGuildsByUserIdRequest request
        )
		{
            AsyncResult<Result.DescribeJoinedGuildsByUserIdResult> result = null;
			await DescribeJoinedGuildsByUserId(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
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
		public async Task<Result.DescribeJoinedGuildsByUserIdResult> DescribeJoinedGuildsByUserIdAsync(
                Request.DescribeJoinedGuildsByUserIdRequest request
        )
		{
			var task = new DescribeJoinedGuildsByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
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
        )
		{
			var task = new GetJoinedGuildTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.GetJoinedGuildResult>(task.Result, task.Error));
        }

		public IFuture<Result.GetJoinedGuildResult> GetJoinedGuildFuture(
                Request.GetJoinedGuildRequest request
        )
		{
			return new GetJoinedGuildTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.GetJoinedGuildResult> GetJoinedGuildAsync(
                Request.GetJoinedGuildRequest request
        )
		{
            AsyncResult<Result.GetJoinedGuildResult> result = null;
			await GetJoinedGuild(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
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
		public async Task<Result.GetJoinedGuildResult> GetJoinedGuildAsync(
                Request.GetJoinedGuildRequest request
        )
		{
			var task = new GetJoinedGuildTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
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
        )
		{
			var task = new GetJoinedGuildByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.GetJoinedGuildByUserIdResult>(task.Result, task.Error));
        }

		public IFuture<Result.GetJoinedGuildByUserIdResult> GetJoinedGuildByUserIdFuture(
                Request.GetJoinedGuildByUserIdRequest request
        )
		{
			return new GetJoinedGuildByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.GetJoinedGuildByUserIdResult> GetJoinedGuildByUserIdAsync(
                Request.GetJoinedGuildByUserIdRequest request
        )
		{
            AsyncResult<Result.GetJoinedGuildByUserIdResult> result = null;
			await GetJoinedGuildByUserId(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
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
		public async Task<Result.GetJoinedGuildByUserIdResult> GetJoinedGuildByUserIdAsync(
                Request.GetJoinedGuildByUserIdRequest request
        )
		{
			var task = new GetJoinedGuildByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
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
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator Withdrawal(
                Request.WithdrawalRequest request,
                UnityAction<AsyncResult<Result.WithdrawalResult>> callback
        )
		{
			var task = new WithdrawalTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.WithdrawalResult>(task.Result, task.Error));
        }

		public IFuture<Result.WithdrawalResult> WithdrawalFuture(
                Request.WithdrawalRequest request
        )
		{
			return new WithdrawalTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.WithdrawalResult> WithdrawalAsync(
                Request.WithdrawalRequest request
        )
		{
            AsyncResult<Result.WithdrawalResult> result = null;
			await Withdrawal(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
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
		public async Task<Result.WithdrawalResult> WithdrawalAsync(
                Request.WithdrawalRequest request
        )
		{
			var task = new WithdrawalTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
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
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator WithdrawalByUserId(
                Request.WithdrawalByUserIdRequest request,
                UnityAction<AsyncResult<Result.WithdrawalByUserIdResult>> callback
        )
		{
			var task = new WithdrawalByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.WithdrawalByUserIdResult>(task.Result, task.Error));
        }

		public IFuture<Result.WithdrawalByUserIdResult> WithdrawalByUserIdFuture(
                Request.WithdrawalByUserIdRequest request
        )
		{
			return new WithdrawalByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.WithdrawalByUserIdResult> WithdrawalByUserIdAsync(
                Request.WithdrawalByUserIdRequest request
        )
		{
            AsyncResult<Result.WithdrawalByUserIdResult> result = null;
			await WithdrawalByUserId(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
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
		public async Task<Result.WithdrawalByUserIdResult> WithdrawalByUserIdAsync(
                Request.WithdrawalByUserIdRequest request
        )
		{
			var task = new WithdrawalByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
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
        )
		{
			var task = new GetLastGuildMasterActivityTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.GetLastGuildMasterActivityResult>(task.Result, task.Error));
        }

		public IFuture<Result.GetLastGuildMasterActivityResult> GetLastGuildMasterActivityFuture(
                Request.GetLastGuildMasterActivityRequest request
        )
		{
			return new GetLastGuildMasterActivityTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.GetLastGuildMasterActivityResult> GetLastGuildMasterActivityAsync(
                Request.GetLastGuildMasterActivityRequest request
        )
		{
            AsyncResult<Result.GetLastGuildMasterActivityResult> result = null;
			await GetLastGuildMasterActivity(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
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
		public async Task<Result.GetLastGuildMasterActivityResult> GetLastGuildMasterActivityAsync(
                Request.GetLastGuildMasterActivityRequest request
        )
		{
			var task = new GetLastGuildMasterActivityTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
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
        )
		{
			var task = new GetLastGuildMasterActivityByGuildNameTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.GetLastGuildMasterActivityByGuildNameResult>(task.Result, task.Error));
        }

		public IFuture<Result.GetLastGuildMasterActivityByGuildNameResult> GetLastGuildMasterActivityByGuildNameFuture(
                Request.GetLastGuildMasterActivityByGuildNameRequest request
        )
		{
			return new GetLastGuildMasterActivityByGuildNameTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.GetLastGuildMasterActivityByGuildNameResult> GetLastGuildMasterActivityByGuildNameAsync(
                Request.GetLastGuildMasterActivityByGuildNameRequest request
        )
		{
            AsyncResult<Result.GetLastGuildMasterActivityByGuildNameResult> result = null;
			await GetLastGuildMasterActivityByGuildName(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
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
		public async Task<Result.GetLastGuildMasterActivityByGuildNameResult> GetLastGuildMasterActivityByGuildNameAsync(
                Request.GetLastGuildMasterActivityByGuildNameRequest request
        )
		{
			var task = new GetLastGuildMasterActivityByGuildNameTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
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
        )
		{
			var task = new PromoteSeniorMemberTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.PromoteSeniorMemberResult>(task.Result, task.Error));
        }

		public IFuture<Result.PromoteSeniorMemberResult> PromoteSeniorMemberFuture(
                Request.PromoteSeniorMemberRequest request
        )
		{
			return new PromoteSeniorMemberTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.PromoteSeniorMemberResult> PromoteSeniorMemberAsync(
                Request.PromoteSeniorMemberRequest request
        )
		{
            AsyncResult<Result.PromoteSeniorMemberResult> result = null;
			await PromoteSeniorMember(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
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
		public async Task<Result.PromoteSeniorMemberResult> PromoteSeniorMemberAsync(
                Request.PromoteSeniorMemberRequest request
        )
		{
			var task = new PromoteSeniorMemberTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
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
        )
		{
			var task = new PromoteSeniorMemberByGuildNameTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.PromoteSeniorMemberByGuildNameResult>(task.Result, task.Error));
        }

		public IFuture<Result.PromoteSeniorMemberByGuildNameResult> PromoteSeniorMemberByGuildNameFuture(
                Request.PromoteSeniorMemberByGuildNameRequest request
        )
		{
			return new PromoteSeniorMemberByGuildNameTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.PromoteSeniorMemberByGuildNameResult> PromoteSeniorMemberByGuildNameAsync(
                Request.PromoteSeniorMemberByGuildNameRequest request
        )
		{
            AsyncResult<Result.PromoteSeniorMemberByGuildNameResult> result = null;
			await PromoteSeniorMemberByGuildName(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
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
		public async Task<Result.PromoteSeniorMemberByGuildNameResult> PromoteSeniorMemberByGuildNameAsync(
                Request.PromoteSeniorMemberByGuildNameRequest request
        )
		{
			var task = new PromoteSeniorMemberByGuildNameTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
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
        )
		{
			var task = new ExportMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.ExportMasterResult>(task.Result, task.Error));
        }

		public IFuture<Result.ExportMasterResult> ExportMasterFuture(
                Request.ExportMasterRequest request
        )
		{
			return new ExportMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.ExportMasterResult> ExportMasterAsync(
                Request.ExportMasterRequest request
        )
		{
            AsyncResult<Result.ExportMasterResult> result = null;
			await ExportMaster(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
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
		public async Task<Result.ExportMasterResult> ExportMasterAsync(
                Request.ExportMasterRequest request
        )
		{
			var task = new ExportMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
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
        )
		{
			var task = new GetCurrentGuildMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.GetCurrentGuildMasterResult>(task.Result, task.Error));
        }

		public IFuture<Result.GetCurrentGuildMasterResult> GetCurrentGuildMasterFuture(
                Request.GetCurrentGuildMasterRequest request
        )
		{
			return new GetCurrentGuildMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.GetCurrentGuildMasterResult> GetCurrentGuildMasterAsync(
                Request.GetCurrentGuildMasterRequest request
        )
		{
            AsyncResult<Result.GetCurrentGuildMasterResult> result = null;
			await GetCurrentGuildMaster(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
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
		public async Task<Result.GetCurrentGuildMasterResult> GetCurrentGuildMasterAsync(
                Request.GetCurrentGuildMasterRequest request
        )
		{
			var task = new GetCurrentGuildMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class UpdateCurrentGuildMasterTask : Gs2RestSessionTask<UpdateCurrentGuildMasterRequest, UpdateCurrentGuildMasterResult>
        {
            public UpdateCurrentGuildMasterTask(IGs2Session session, RestSessionRequestFactory factory, UpdateCurrentGuildMasterRequest request) : base(session, factory, request)
            {
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
                if (request.Settings != null)
                {
                    jsonWriter.WritePropertyName("settings");
                    jsonWriter.Write(request.Settings);
                }
                if (request.ContextStack != null)
                {
                    jsonWriter.WritePropertyName("contextStack");
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
        )
		{
			var task = new UpdateCurrentGuildMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.UpdateCurrentGuildMasterResult>(task.Result, task.Error));
        }

		public IFuture<Result.UpdateCurrentGuildMasterResult> UpdateCurrentGuildMasterFuture(
                Request.UpdateCurrentGuildMasterRequest request
        )
		{
			return new UpdateCurrentGuildMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.UpdateCurrentGuildMasterResult> UpdateCurrentGuildMasterAsync(
                Request.UpdateCurrentGuildMasterRequest request
        )
		{
            AsyncResult<Result.UpdateCurrentGuildMasterResult> result = null;
			await UpdateCurrentGuildMaster(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
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
		public async Task<Result.UpdateCurrentGuildMasterResult> UpdateCurrentGuildMasterAsync(
                Request.UpdateCurrentGuildMasterRequest request
        )
		{
			var task = new UpdateCurrentGuildMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
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
        )
		{
			var task = new UpdateCurrentGuildMasterFromGitHubTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.UpdateCurrentGuildMasterFromGitHubResult>(task.Result, task.Error));
        }

		public IFuture<Result.UpdateCurrentGuildMasterFromGitHubResult> UpdateCurrentGuildMasterFromGitHubFuture(
                Request.UpdateCurrentGuildMasterFromGitHubRequest request
        )
		{
			return new UpdateCurrentGuildMasterFromGitHubTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.UpdateCurrentGuildMasterFromGitHubResult> UpdateCurrentGuildMasterFromGitHubAsync(
                Request.UpdateCurrentGuildMasterFromGitHubRequest request
        )
		{
            AsyncResult<Result.UpdateCurrentGuildMasterFromGitHubResult> result = null;
			await UpdateCurrentGuildMasterFromGitHub(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
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
		public async Task<Result.UpdateCurrentGuildMasterFromGitHubResult> UpdateCurrentGuildMasterFromGitHubAsync(
                Request.UpdateCurrentGuildMasterFromGitHubRequest request
        )
		{
			var task = new UpdateCurrentGuildMasterFromGitHubTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
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
        )
		{
			var task = new DescribeReceiveRequestsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.DescribeReceiveRequestsResult>(task.Result, task.Error));
        }

		public IFuture<Result.DescribeReceiveRequestsResult> DescribeReceiveRequestsFuture(
                Request.DescribeReceiveRequestsRequest request
        )
		{
			return new DescribeReceiveRequestsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.DescribeReceiveRequestsResult> DescribeReceiveRequestsAsync(
                Request.DescribeReceiveRequestsRequest request
        )
		{
            AsyncResult<Result.DescribeReceiveRequestsResult> result = null;
			await DescribeReceiveRequests(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
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
		public async Task<Result.DescribeReceiveRequestsResult> DescribeReceiveRequestsAsync(
                Request.DescribeReceiveRequestsRequest request
        )
		{
			var task = new DescribeReceiveRequestsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
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
        )
		{
			var task = new DescribeReceiveRequestsByGuildNameTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.DescribeReceiveRequestsByGuildNameResult>(task.Result, task.Error));
        }

		public IFuture<Result.DescribeReceiveRequestsByGuildNameResult> DescribeReceiveRequestsByGuildNameFuture(
                Request.DescribeReceiveRequestsByGuildNameRequest request
        )
		{
			return new DescribeReceiveRequestsByGuildNameTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.DescribeReceiveRequestsByGuildNameResult> DescribeReceiveRequestsByGuildNameAsync(
                Request.DescribeReceiveRequestsByGuildNameRequest request
        )
		{
            AsyncResult<Result.DescribeReceiveRequestsByGuildNameResult> result = null;
			await DescribeReceiveRequestsByGuildName(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
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
		public async Task<Result.DescribeReceiveRequestsByGuildNameResult> DescribeReceiveRequestsByGuildNameAsync(
                Request.DescribeReceiveRequestsByGuildNameRequest request
        )
		{
			var task = new DescribeReceiveRequestsByGuildNameTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
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
        )
		{
			var task = new GetReceiveRequestTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.GetReceiveRequestResult>(task.Result, task.Error));
        }

		public IFuture<Result.GetReceiveRequestResult> GetReceiveRequestFuture(
                Request.GetReceiveRequestRequest request
        )
		{
			return new GetReceiveRequestTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.GetReceiveRequestResult> GetReceiveRequestAsync(
                Request.GetReceiveRequestRequest request
        )
		{
            AsyncResult<Result.GetReceiveRequestResult> result = null;
			await GetReceiveRequest(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
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
		public async Task<Result.GetReceiveRequestResult> GetReceiveRequestAsync(
                Request.GetReceiveRequestRequest request
        )
		{
			var task = new GetReceiveRequestTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
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
        )
		{
			var task = new GetReceiveRequestByGuildNameTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.GetReceiveRequestByGuildNameResult>(task.Result, task.Error));
        }

		public IFuture<Result.GetReceiveRequestByGuildNameResult> GetReceiveRequestByGuildNameFuture(
                Request.GetReceiveRequestByGuildNameRequest request
        )
		{
			return new GetReceiveRequestByGuildNameTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.GetReceiveRequestByGuildNameResult> GetReceiveRequestByGuildNameAsync(
                Request.GetReceiveRequestByGuildNameRequest request
        )
		{
            AsyncResult<Result.GetReceiveRequestByGuildNameResult> result = null;
			await GetReceiveRequestByGuildName(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
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
		public async Task<Result.GetReceiveRequestByGuildNameResult> GetReceiveRequestByGuildNameAsync(
                Request.GetReceiveRequestByGuildNameRequest request
        )
		{
			var task = new GetReceiveRequestByGuildNameTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
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
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator AcceptRequest(
                Request.AcceptRequestRequest request,
                UnityAction<AsyncResult<Result.AcceptRequestResult>> callback
        )
		{
			var task = new AcceptRequestTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.AcceptRequestResult>(task.Result, task.Error));
        }

		public IFuture<Result.AcceptRequestResult> AcceptRequestFuture(
                Request.AcceptRequestRequest request
        )
		{
			return new AcceptRequestTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.AcceptRequestResult> AcceptRequestAsync(
                Request.AcceptRequestRequest request
        )
		{
            AsyncResult<Result.AcceptRequestResult> result = null;
			await AcceptRequest(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
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
		public async Task<Result.AcceptRequestResult> AcceptRequestAsync(
                Request.AcceptRequestRequest request
        )
		{
			var task = new AcceptRequestTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
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
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator AcceptRequestByGuildName(
                Request.AcceptRequestByGuildNameRequest request,
                UnityAction<AsyncResult<Result.AcceptRequestByGuildNameResult>> callback
        )
		{
			var task = new AcceptRequestByGuildNameTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.AcceptRequestByGuildNameResult>(task.Result, task.Error));
        }

		public IFuture<Result.AcceptRequestByGuildNameResult> AcceptRequestByGuildNameFuture(
                Request.AcceptRequestByGuildNameRequest request
        )
		{
			return new AcceptRequestByGuildNameTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.AcceptRequestByGuildNameResult> AcceptRequestByGuildNameAsync(
                Request.AcceptRequestByGuildNameRequest request
        )
		{
            AsyncResult<Result.AcceptRequestByGuildNameResult> result = null;
			await AcceptRequestByGuildName(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
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
		public async Task<Result.AcceptRequestByGuildNameResult> AcceptRequestByGuildNameAsync(
                Request.AcceptRequestByGuildNameRequest request
        )
		{
			var task = new AcceptRequestByGuildNameTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
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
        )
		{
			var task = new RejectRequestTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.RejectRequestResult>(task.Result, task.Error));
        }

		public IFuture<Result.RejectRequestResult> RejectRequestFuture(
                Request.RejectRequestRequest request
        )
		{
			return new RejectRequestTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.RejectRequestResult> RejectRequestAsync(
                Request.RejectRequestRequest request
        )
		{
            AsyncResult<Result.RejectRequestResult> result = null;
			await RejectRequest(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
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
		public async Task<Result.RejectRequestResult> RejectRequestAsync(
                Request.RejectRequestRequest request
        )
		{
			var task = new RejectRequestTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
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
        )
		{
			var task = new RejectRequestByGuildNameTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.RejectRequestByGuildNameResult>(task.Result, task.Error));
        }

		public IFuture<Result.RejectRequestByGuildNameResult> RejectRequestByGuildNameFuture(
                Request.RejectRequestByGuildNameRequest request
        )
		{
			return new RejectRequestByGuildNameTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.RejectRequestByGuildNameResult> RejectRequestByGuildNameAsync(
                Request.RejectRequestByGuildNameRequest request
        )
		{
            AsyncResult<Result.RejectRequestByGuildNameResult> result = null;
			await RejectRequestByGuildName(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
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
		public async Task<Result.RejectRequestByGuildNameResult> RejectRequestByGuildNameAsync(
                Request.RejectRequestByGuildNameRequest request
        )
		{
			var task = new RejectRequestByGuildNameTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
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
        )
		{
			var task = new DescribeSendRequestsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.DescribeSendRequestsResult>(task.Result, task.Error));
        }

		public IFuture<Result.DescribeSendRequestsResult> DescribeSendRequestsFuture(
                Request.DescribeSendRequestsRequest request
        )
		{
			return new DescribeSendRequestsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.DescribeSendRequestsResult> DescribeSendRequestsAsync(
                Request.DescribeSendRequestsRequest request
        )
		{
            AsyncResult<Result.DescribeSendRequestsResult> result = null;
			await DescribeSendRequests(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
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
		public async Task<Result.DescribeSendRequestsResult> DescribeSendRequestsAsync(
                Request.DescribeSendRequestsRequest request
        )
		{
			var task = new DescribeSendRequestsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
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
        )
		{
			var task = new DescribeSendRequestsByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.DescribeSendRequestsByUserIdResult>(task.Result, task.Error));
        }

		public IFuture<Result.DescribeSendRequestsByUserIdResult> DescribeSendRequestsByUserIdFuture(
                Request.DescribeSendRequestsByUserIdRequest request
        )
		{
			return new DescribeSendRequestsByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.DescribeSendRequestsByUserIdResult> DescribeSendRequestsByUserIdAsync(
                Request.DescribeSendRequestsByUserIdRequest request
        )
		{
            AsyncResult<Result.DescribeSendRequestsByUserIdResult> result = null;
			await DescribeSendRequestsByUserId(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
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
		public async Task<Result.DescribeSendRequestsByUserIdResult> DescribeSendRequestsByUserIdAsync(
                Request.DescribeSendRequestsByUserIdRequest request
        )
		{
			var task = new DescribeSendRequestsByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
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
        )
		{
			var task = new GetSendRequestTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.GetSendRequestResult>(task.Result, task.Error));
        }

		public IFuture<Result.GetSendRequestResult> GetSendRequestFuture(
                Request.GetSendRequestRequest request
        )
		{
			return new GetSendRequestTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.GetSendRequestResult> GetSendRequestAsync(
                Request.GetSendRequestRequest request
        )
		{
            AsyncResult<Result.GetSendRequestResult> result = null;
			await GetSendRequest(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
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
		public async Task<Result.GetSendRequestResult> GetSendRequestAsync(
                Request.GetSendRequestRequest request
        )
		{
			var task = new GetSendRequestTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
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
        )
		{
			var task = new GetSendRequestByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.GetSendRequestByUserIdResult>(task.Result, task.Error));
        }

		public IFuture<Result.GetSendRequestByUserIdResult> GetSendRequestByUserIdFuture(
                Request.GetSendRequestByUserIdRequest request
        )
		{
			return new GetSendRequestByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.GetSendRequestByUserIdResult> GetSendRequestByUserIdAsync(
                Request.GetSendRequestByUserIdRequest request
        )
		{
            AsyncResult<Result.GetSendRequestByUserIdResult> result = null;
			await GetSendRequestByUserId(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
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
		public async Task<Result.GetSendRequestByUserIdResult> GetSendRequestByUserIdAsync(
                Request.GetSendRequestByUserIdRequest request
        )
		{
			var task = new GetSendRequestByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
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
                if (request.ContextStack != null)
                {
                    jsonWriter.WritePropertyName("contextStack");
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
		public IEnumerator SendRequest(
                Request.SendRequestRequest request,
                UnityAction<AsyncResult<Result.SendRequestResult>> callback
        )
		{
			var task = new SendRequestTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.SendRequestResult>(task.Result, task.Error));
        }

		public IFuture<Result.SendRequestResult> SendRequestFuture(
                Request.SendRequestRequest request
        )
		{
			return new SendRequestTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.SendRequestResult> SendRequestAsync(
                Request.SendRequestRequest request
        )
		{
            AsyncResult<Result.SendRequestResult> result = null;
			await SendRequest(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
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
		public async Task<Result.SendRequestResult> SendRequestAsync(
                Request.SendRequestRequest request
        )
		{
			var task = new SendRequestTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
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
                if (request.ContextStack != null)
                {
                    jsonWriter.WritePropertyName("contextStack");
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
		public IEnumerator SendRequestByUserId(
                Request.SendRequestByUserIdRequest request,
                UnityAction<AsyncResult<Result.SendRequestByUserIdResult>> callback
        )
		{
			var task = new SendRequestByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.SendRequestByUserIdResult>(task.Result, task.Error));
        }

		public IFuture<Result.SendRequestByUserIdResult> SendRequestByUserIdFuture(
                Request.SendRequestByUserIdRequest request
        )
		{
			return new SendRequestByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.SendRequestByUserIdResult> SendRequestByUserIdAsync(
                Request.SendRequestByUserIdRequest request
        )
		{
            AsyncResult<Result.SendRequestByUserIdResult> result = null;
			await SendRequestByUserId(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
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
		public async Task<Result.SendRequestByUserIdResult> SendRequestByUserIdAsync(
                Request.SendRequestByUserIdRequest request
        )
		{
			var task = new SendRequestByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
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
        )
		{
			var task = new DeleteRequestTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.DeleteRequestResult>(task.Result, task.Error));
        }

		public IFuture<Result.DeleteRequestResult> DeleteRequestFuture(
                Request.DeleteRequestRequest request
        )
		{
			return new DeleteRequestTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.DeleteRequestResult> DeleteRequestAsync(
                Request.DeleteRequestRequest request
        )
		{
            AsyncResult<Result.DeleteRequestResult> result = null;
			await DeleteRequest(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
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
		public async Task<Result.DeleteRequestResult> DeleteRequestAsync(
                Request.DeleteRequestRequest request
        )
		{
			var task = new DeleteRequestTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
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
        )
		{
			var task = new DeleteRequestByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.DeleteRequestByUserIdResult>(task.Result, task.Error));
        }

		public IFuture<Result.DeleteRequestByUserIdResult> DeleteRequestByUserIdFuture(
                Request.DeleteRequestByUserIdRequest request
        )
		{
			return new DeleteRequestByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.DeleteRequestByUserIdResult> DeleteRequestByUserIdAsync(
                Request.DeleteRequestByUserIdRequest request
        )
		{
            AsyncResult<Result.DeleteRequestByUserIdResult> result = null;
			await DeleteRequestByUserId(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
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
		public async Task<Result.DeleteRequestByUserIdResult> DeleteRequestByUserIdAsync(
                Request.DeleteRequestByUserIdRequest request
        )
		{
			var task = new DeleteRequestByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
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
        )
		{
			var task = new DescribeIgnoreUsersTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.DescribeIgnoreUsersResult>(task.Result, task.Error));
        }

		public IFuture<Result.DescribeIgnoreUsersResult> DescribeIgnoreUsersFuture(
                Request.DescribeIgnoreUsersRequest request
        )
		{
			return new DescribeIgnoreUsersTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.DescribeIgnoreUsersResult> DescribeIgnoreUsersAsync(
                Request.DescribeIgnoreUsersRequest request
        )
		{
            AsyncResult<Result.DescribeIgnoreUsersResult> result = null;
			await DescribeIgnoreUsers(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
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
		public async Task<Result.DescribeIgnoreUsersResult> DescribeIgnoreUsersAsync(
                Request.DescribeIgnoreUsersRequest request
        )
		{
			var task = new DescribeIgnoreUsersTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
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
        )
		{
			var task = new DescribeIgnoreUsersByGuildNameTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.DescribeIgnoreUsersByGuildNameResult>(task.Result, task.Error));
        }

		public IFuture<Result.DescribeIgnoreUsersByGuildNameResult> DescribeIgnoreUsersByGuildNameFuture(
                Request.DescribeIgnoreUsersByGuildNameRequest request
        )
		{
			return new DescribeIgnoreUsersByGuildNameTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.DescribeIgnoreUsersByGuildNameResult> DescribeIgnoreUsersByGuildNameAsync(
                Request.DescribeIgnoreUsersByGuildNameRequest request
        )
		{
            AsyncResult<Result.DescribeIgnoreUsersByGuildNameResult> result = null;
			await DescribeIgnoreUsersByGuildName(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
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
		public async Task<Result.DescribeIgnoreUsersByGuildNameResult> DescribeIgnoreUsersByGuildNameAsync(
                Request.DescribeIgnoreUsersByGuildNameRequest request
        )
		{
			var task = new DescribeIgnoreUsersByGuildNameTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
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
        )
		{
			var task = new GetIgnoreUserTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.GetIgnoreUserResult>(task.Result, task.Error));
        }

		public IFuture<Result.GetIgnoreUserResult> GetIgnoreUserFuture(
                Request.GetIgnoreUserRequest request
        )
		{
			return new GetIgnoreUserTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.GetIgnoreUserResult> GetIgnoreUserAsync(
                Request.GetIgnoreUserRequest request
        )
		{
            AsyncResult<Result.GetIgnoreUserResult> result = null;
			await GetIgnoreUser(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
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
		public async Task<Result.GetIgnoreUserResult> GetIgnoreUserAsync(
                Request.GetIgnoreUserRequest request
        )
		{
			var task = new GetIgnoreUserTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
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
        )
		{
			var task = new GetIgnoreUserByGuildNameTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.GetIgnoreUserByGuildNameResult>(task.Result, task.Error));
        }

		public IFuture<Result.GetIgnoreUserByGuildNameResult> GetIgnoreUserByGuildNameFuture(
                Request.GetIgnoreUserByGuildNameRequest request
        )
		{
			return new GetIgnoreUserByGuildNameTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.GetIgnoreUserByGuildNameResult> GetIgnoreUserByGuildNameAsync(
                Request.GetIgnoreUserByGuildNameRequest request
        )
		{
            AsyncResult<Result.GetIgnoreUserByGuildNameResult> result = null;
			await GetIgnoreUserByGuildName(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
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
		public async Task<Result.GetIgnoreUserByGuildNameResult> GetIgnoreUserByGuildNameAsync(
                Request.GetIgnoreUserByGuildNameRequest request
        )
		{
			var task = new GetIgnoreUserByGuildNameTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
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
        )
		{
			var task = new AddIgnoreUserTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.AddIgnoreUserResult>(task.Result, task.Error));
        }

		public IFuture<Result.AddIgnoreUserResult> AddIgnoreUserFuture(
                Request.AddIgnoreUserRequest request
        )
		{
			return new AddIgnoreUserTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.AddIgnoreUserResult> AddIgnoreUserAsync(
                Request.AddIgnoreUserRequest request
        )
		{
            AsyncResult<Result.AddIgnoreUserResult> result = null;
			await AddIgnoreUser(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
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
		public async Task<Result.AddIgnoreUserResult> AddIgnoreUserAsync(
                Request.AddIgnoreUserRequest request
        )
		{
			var task = new AddIgnoreUserTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
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
        )
		{
			var task = new AddIgnoreUserByGuildNameTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.AddIgnoreUserByGuildNameResult>(task.Result, task.Error));
        }

		public IFuture<Result.AddIgnoreUserByGuildNameResult> AddIgnoreUserByGuildNameFuture(
                Request.AddIgnoreUserByGuildNameRequest request
        )
		{
			return new AddIgnoreUserByGuildNameTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.AddIgnoreUserByGuildNameResult> AddIgnoreUserByGuildNameAsync(
                Request.AddIgnoreUserByGuildNameRequest request
        )
		{
            AsyncResult<Result.AddIgnoreUserByGuildNameResult> result = null;
			await AddIgnoreUserByGuildName(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
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
		public async Task<Result.AddIgnoreUserByGuildNameResult> AddIgnoreUserByGuildNameAsync(
                Request.AddIgnoreUserByGuildNameRequest request
        )
		{
			var task = new AddIgnoreUserByGuildNameTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
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
        )
		{
			var task = new DeleteIgnoreUserTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.DeleteIgnoreUserResult>(task.Result, task.Error));
        }

		public IFuture<Result.DeleteIgnoreUserResult> DeleteIgnoreUserFuture(
                Request.DeleteIgnoreUserRequest request
        )
		{
			return new DeleteIgnoreUserTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.DeleteIgnoreUserResult> DeleteIgnoreUserAsync(
                Request.DeleteIgnoreUserRequest request
        )
		{
            AsyncResult<Result.DeleteIgnoreUserResult> result = null;
			await DeleteIgnoreUser(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
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
		public async Task<Result.DeleteIgnoreUserResult> DeleteIgnoreUserAsync(
                Request.DeleteIgnoreUserRequest request
        )
		{
			var task = new DeleteIgnoreUserTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
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
        )
		{
			var task = new DeleteIgnoreUserByGuildNameTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.DeleteIgnoreUserByGuildNameResult>(task.Result, task.Error));
        }

		public IFuture<Result.DeleteIgnoreUserByGuildNameResult> DeleteIgnoreUserByGuildNameFuture(
                Request.DeleteIgnoreUserByGuildNameRequest request
        )
		{
			return new DeleteIgnoreUserByGuildNameTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.DeleteIgnoreUserByGuildNameResult> DeleteIgnoreUserByGuildNameAsync(
                Request.DeleteIgnoreUserByGuildNameRequest request
        )
		{
            AsyncResult<Result.DeleteIgnoreUserByGuildNameResult> result = null;
			await DeleteIgnoreUserByGuildName(
                request,
                r => result = r
            );
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
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
		public async Task<Result.DeleteIgnoreUserByGuildNameResult> DeleteIgnoreUserByGuildNameAsync(
                Request.DeleteIgnoreUserByGuildNameRequest request
        )
		{
			var task = new DeleteIgnoreUserByGuildNameTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif
	}
}