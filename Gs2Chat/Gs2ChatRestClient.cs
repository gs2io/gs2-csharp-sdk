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
using Gs2.Gs2Chat.Request;
using Gs2.Gs2Chat.Result;
using Gs2.Util.LitJson;

namespace Gs2.Gs2Chat
{
	public class Gs2ChatRestClient : AbstractGs2Client
	{
#if UNITY_2017_1_OR_NEWER
		private readonly CertificateHandler _certificateHandler;
#endif

		public static string Endpoint = "chat";

        protected Gs2RestSession Gs2RestSession => (Gs2RestSession) Gs2Session;

		public Gs2ChatRestClient(Gs2RestSession Gs2RestSession) : base(Gs2RestSession)
		{

		}

#if UNITY_2017_1_OR_NEWER
		public Gs2ChatRestClient(Gs2RestSession gs2RestSession, CertificateHandler certificateHandler) : base(gs2RestSession)
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
                    .Replace("{service}", "chat")
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
                    .Replace("{service}", "chat")
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
                if (request.AllowCreateRoom != null)
                {
                    jsonWriter.WritePropertyName("allowCreateRoom");
                    jsonWriter.Write(request.AllowCreateRoom.ToString());
                }
                if (request.MessageLifeTimeDays != null)
                {
                    jsonWriter.WritePropertyName("messageLifeTimeDays");
                    jsonWriter.Write(request.MessageLifeTimeDays.ToString());
                }
                if (request.PostMessageScript != null)
                {
                    jsonWriter.WritePropertyName("postMessageScript");
                    request.PostMessageScript.WriteJson(jsonWriter);
                }
                if (request.CreateRoomScript != null)
                {
                    jsonWriter.WritePropertyName("createRoomScript");
                    request.CreateRoomScript.WriteJson(jsonWriter);
                }
                if (request.DeleteRoomScript != null)
                {
                    jsonWriter.WritePropertyName("deleteRoomScript");
                    request.DeleteRoomScript.WriteJson(jsonWriter);
                }
                if (request.SubscribeRoomScript != null)
                {
                    jsonWriter.WritePropertyName("subscribeRoomScript");
                    request.SubscribeRoomScript.WriteJson(jsonWriter);
                }
                if (request.UnsubscribeRoomScript != null)
                {
                    jsonWriter.WritePropertyName("unsubscribeRoomScript");
                    request.UnsubscribeRoomScript.WriteJson(jsonWriter);
                }
                if (request.PostNotification != null)
                {
                    jsonWriter.WritePropertyName("postNotification");
                    request.PostNotification.WriteJson(jsonWriter);
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
                    .Replace("{service}", "chat")
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
                    .Replace("{service}", "chat")
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
                    .Replace("{service}", "chat")
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
                if (request.AllowCreateRoom != null)
                {
                    jsonWriter.WritePropertyName("allowCreateRoom");
                    jsonWriter.Write(request.AllowCreateRoom.ToString());
                }
                if (request.MessageLifeTimeDays != null)
                {
                    jsonWriter.WritePropertyName("messageLifeTimeDays");
                    jsonWriter.Write(request.MessageLifeTimeDays.ToString());
                }
                if (request.PostMessageScript != null)
                {
                    jsonWriter.WritePropertyName("postMessageScript");
                    request.PostMessageScript.WriteJson(jsonWriter);
                }
                if (request.CreateRoomScript != null)
                {
                    jsonWriter.WritePropertyName("createRoomScript");
                    request.CreateRoomScript.WriteJson(jsonWriter);
                }
                if (request.DeleteRoomScript != null)
                {
                    jsonWriter.WritePropertyName("deleteRoomScript");
                    request.DeleteRoomScript.WriteJson(jsonWriter);
                }
                if (request.SubscribeRoomScript != null)
                {
                    jsonWriter.WritePropertyName("subscribeRoomScript");
                    request.SubscribeRoomScript.WriteJson(jsonWriter);
                }
                if (request.UnsubscribeRoomScript != null)
                {
                    jsonWriter.WritePropertyName("unsubscribeRoomScript");
                    request.UnsubscribeRoomScript.WriteJson(jsonWriter);
                }
                if (request.PostNotification != null)
                {
                    jsonWriter.WritePropertyName("postNotification");
                    request.PostNotification.WriteJson(jsonWriter);
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
                    .Replace("{service}", "chat")
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
                    .Replace("{service}", "chat")
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
                    .Replace("{service}", "chat")
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
                    .Replace("{service}", "chat")
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
                    .Replace("{service}", "chat")
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
                    .Replace("{service}", "chat")
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
                    .Replace("{service}", "chat")
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
                    .Replace("{service}", "chat")
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
                    .Replace("{service}", "chat")
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


        public class DescribeRoomsTask : Gs2RestSessionTask<DescribeRoomsRequest, DescribeRoomsResult>
        {
            public DescribeRoomsTask(IGs2Session session, RestSessionRequestFactory factory, DescribeRoomsRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DescribeRoomsRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "chat")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/room";

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
		public IEnumerator DescribeRooms(
                Request.DescribeRoomsRequest request,
                UnityAction<AsyncResult<Result.DescribeRoomsResult>> callback
        ) =>
            new DescribeRoomsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.DescribeRoomsResult> DescribeRoomsFuture(
                Request.DescribeRoomsRequest request
        ) =>
            new DescribeRoomsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.DescribeRoomsResult> DescribeRoomsAsync(
                Request.DescribeRoomsRequest request
        ) =>
            new DescribeRoomsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.DescribeRoomsResult>();
    #else
		public DescribeRoomsTask DescribeRoomsAsync(
                Request.DescribeRoomsRequest request
        )
		{
			return new DescribeRoomsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.DescribeRoomsResult> DescribeRoomsAsync(
                Request.DescribeRoomsRequest request
        ) =>
            new DescribeRoomsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class CreateRoomTask : Gs2RestSessionTask<CreateRoomRequest, CreateRoomResult>
        {
            public CreateRoomTask(IGs2Session session, RestSessionRequestFactory factory, CreateRoomRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(CreateRoomRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "chat")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/room/user";

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
                if (request.Metadata != null)
                {
                    jsonWriter.WritePropertyName("metadata");
                    jsonWriter.Write(request.Metadata);
                }
                if (request.Password != null)
                {
                    jsonWriter.WritePropertyName("password");
                    jsonWriter.Write(request.Password);
                }
                if (request.WhiteListUserIds != null)
                {
                    jsonWriter.WritePropertyName("whiteListUserIds");
                    jsonWriter.WriteArrayStart();
                    foreach(var item in request.WhiteListUserIds)
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
                if (error.Errors.Count(v => v.code == "room.allowUserIds.notInclude") > 0) {
                    base.OnError(new Exception.NoAccessPrivilegesException(error));
                }
                else {
                    base.OnError(error);
                }
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator CreateRoom(
                Request.CreateRoomRequest request,
                UnityAction<AsyncResult<Result.CreateRoomResult>> callback
        ) =>
            new CreateRoomTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.CreateRoomResult> CreateRoomFuture(
                Request.CreateRoomRequest request
        ) =>
            new CreateRoomTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.CreateRoomResult> CreateRoomAsync(
                Request.CreateRoomRequest request
        ) =>
            new CreateRoomTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.CreateRoomResult>();
    #else
		public CreateRoomTask CreateRoomAsync(
                Request.CreateRoomRequest request
        )
		{
			return new CreateRoomTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.CreateRoomResult> CreateRoomAsync(
                Request.CreateRoomRequest request
        ) =>
            new CreateRoomTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class CreateRoomFromBackendTask : Gs2RestSessionTask<CreateRoomFromBackendRequest, CreateRoomFromBackendResult>
        {
            public CreateRoomFromBackendTask(IGs2Session session, RestSessionRequestFactory factory, CreateRoomFromBackendRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(CreateRoomFromBackendRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "chat")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/room";

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
                if (request.UserId != null)
                {
                    jsonWriter.WritePropertyName("userId");
                    jsonWriter.Write(request.UserId);
                }
                if (request.Metadata != null)
                {
                    jsonWriter.WritePropertyName("metadata");
                    jsonWriter.Write(request.Metadata);
                }
                if (request.Password != null)
                {
                    jsonWriter.WritePropertyName("password");
                    jsonWriter.Write(request.Password);
                }
                if (request.WhiteListUserIds != null)
                {
                    jsonWriter.WritePropertyName("whiteListUserIds");
                    jsonWriter.WriteArrayStart();
                    foreach(var item in request.WhiteListUserIds)
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
		public IEnumerator CreateRoomFromBackend(
                Request.CreateRoomFromBackendRequest request,
                UnityAction<AsyncResult<Result.CreateRoomFromBackendResult>> callback
        ) =>
            new CreateRoomFromBackendTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.CreateRoomFromBackendResult> CreateRoomFromBackendFuture(
                Request.CreateRoomFromBackendRequest request
        ) =>
            new CreateRoomFromBackendTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.CreateRoomFromBackendResult> CreateRoomFromBackendAsync(
                Request.CreateRoomFromBackendRequest request
        ) =>
            new CreateRoomFromBackendTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.CreateRoomFromBackendResult>();
    #else
		public CreateRoomFromBackendTask CreateRoomFromBackendAsync(
                Request.CreateRoomFromBackendRequest request
        )
		{
			return new CreateRoomFromBackendTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.CreateRoomFromBackendResult> CreateRoomFromBackendAsync(
                Request.CreateRoomFromBackendRequest request
        ) =>
            new CreateRoomFromBackendTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class GetRoomTask : Gs2RestSessionTask<GetRoomRequest, GetRoomResult>
        {
            public GetRoomTask(IGs2Session session, RestSessionRequestFactory factory, GetRoomRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(GetRoomRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "chat")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/room/{roomName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{roomName}", !string.IsNullOrEmpty(request.RoomName) ? request.RoomName.ToString() : "null");

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
		public IEnumerator GetRoom(
                Request.GetRoomRequest request,
                UnityAction<AsyncResult<Result.GetRoomResult>> callback
        ) =>
            new GetRoomTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.GetRoomResult> GetRoomFuture(
                Request.GetRoomRequest request
        ) =>
            new GetRoomTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.GetRoomResult> GetRoomAsync(
                Request.GetRoomRequest request
        ) =>
            new GetRoomTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.GetRoomResult>();
    #else
		public GetRoomTask GetRoomAsync(
                Request.GetRoomRequest request
        )
		{
			return new GetRoomTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.GetRoomResult> GetRoomAsync(
                Request.GetRoomRequest request
        ) =>
            new GetRoomTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class UpdateRoomTask : Gs2RestSessionTask<UpdateRoomRequest, UpdateRoomResult>
        {
            public UpdateRoomTask(IGs2Session session, RestSessionRequestFactory factory, UpdateRoomRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(UpdateRoomRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "chat")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/room/{roomName}/user";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{roomName}", !string.IsNullOrEmpty(request.RoomName) ? request.RoomName.ToString() : "null");

                var sessionRequest = Factory.Put(url);

                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);
                jsonWriter.WriteObjectStart();
                if (request.Metadata != null)
                {
                    jsonWriter.WritePropertyName("metadata");
                    jsonWriter.Write(request.Metadata);
                }
                if (request.Password != null)
                {
                    jsonWriter.WritePropertyName("password");
                    jsonWriter.Write(request.Password);
                }
                if (request.WhiteListUserIds != null)
                {
                    jsonWriter.WritePropertyName("whiteListUserIds");
                    jsonWriter.WriteArrayStart();
                    foreach(var item in request.WhiteListUserIds)
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
                if (error.Errors.Count(v => v.code == "room.allowUserIds.notInclude") > 0) {
                    base.OnError(new Exception.NoAccessPrivilegesException(error));
                }
                else {
                    base.OnError(error);
                }
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator UpdateRoom(
                Request.UpdateRoomRequest request,
                UnityAction<AsyncResult<Result.UpdateRoomResult>> callback
        ) =>
            new UpdateRoomTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.UpdateRoomResult> UpdateRoomFuture(
                Request.UpdateRoomRequest request
        ) =>
            new UpdateRoomTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.UpdateRoomResult> UpdateRoomAsync(
                Request.UpdateRoomRequest request
        ) =>
            new UpdateRoomTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.UpdateRoomResult>();
    #else
		public UpdateRoomTask UpdateRoomAsync(
                Request.UpdateRoomRequest request
        )
		{
			return new UpdateRoomTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.UpdateRoomResult> UpdateRoomAsync(
                Request.UpdateRoomRequest request
        ) =>
            new UpdateRoomTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class UpdateRoomFromBackendTask : Gs2RestSessionTask<UpdateRoomFromBackendRequest, UpdateRoomFromBackendResult>
        {
            public UpdateRoomFromBackendTask(IGs2Session session, RestSessionRequestFactory factory, UpdateRoomFromBackendRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(UpdateRoomFromBackendRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "chat")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/room/{roomName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{roomName}", !string.IsNullOrEmpty(request.RoomName) ? request.RoomName.ToString() : "null");

                var sessionRequest = Factory.Put(url);

                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);
                jsonWriter.WriteObjectStart();
                if (request.Metadata != null)
                {
                    jsonWriter.WritePropertyName("metadata");
                    jsonWriter.Write(request.Metadata);
                }
                if (request.Password != null)
                {
                    jsonWriter.WritePropertyName("password");
                    jsonWriter.Write(request.Password);
                }
                if (request.WhiteListUserIds != null)
                {
                    jsonWriter.WritePropertyName("whiteListUserIds");
                    jsonWriter.WriteArrayStart();
                    foreach(var item in request.WhiteListUserIds)
                    {
                        jsonWriter.Write(item);
                    }
                    jsonWriter.WriteArrayEnd();
                }
                if (request.UserId != null)
                {
                    jsonWriter.WritePropertyName("userId");
                    jsonWriter.Write(request.UserId);
                }
                if (request.ContextStack != null)
                {
                    jsonWriter.WritePropertyName("contextStack");
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
		public IEnumerator UpdateRoomFromBackend(
                Request.UpdateRoomFromBackendRequest request,
                UnityAction<AsyncResult<Result.UpdateRoomFromBackendResult>> callback
        ) =>
            new UpdateRoomFromBackendTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.UpdateRoomFromBackendResult> UpdateRoomFromBackendFuture(
                Request.UpdateRoomFromBackendRequest request
        ) =>
            new UpdateRoomFromBackendTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.UpdateRoomFromBackendResult> UpdateRoomFromBackendAsync(
                Request.UpdateRoomFromBackendRequest request
        ) =>
            new UpdateRoomFromBackendTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.UpdateRoomFromBackendResult>();
    #else
		public UpdateRoomFromBackendTask UpdateRoomFromBackendAsync(
                Request.UpdateRoomFromBackendRequest request
        )
		{
			return new UpdateRoomFromBackendTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.UpdateRoomFromBackendResult> UpdateRoomFromBackendAsync(
                Request.UpdateRoomFromBackendRequest request
        ) =>
            new UpdateRoomFromBackendTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class DeleteRoomTask : Gs2RestSessionTask<DeleteRoomRequest, DeleteRoomResult>
        {
            public DeleteRoomTask(IGs2Session session, RestSessionRequestFactory factory, DeleteRoomRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DeleteRoomRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "chat")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/room/{roomName}/user";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{roomName}", !string.IsNullOrEmpty(request.RoomName) ? request.RoomName.ToString() : "null");

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
                if (error.Errors.Count(v => v.code == "room.allowUserIds.notInclude") > 0) {
                    base.OnError(new Exception.NoAccessPrivilegesException(error));
                }
                else {
                    base.OnError(error);
                }
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator DeleteRoom(
                Request.DeleteRoomRequest request,
                UnityAction<AsyncResult<Result.DeleteRoomResult>> callback
        ) =>
            new DeleteRoomTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.DeleteRoomResult> DeleteRoomFuture(
                Request.DeleteRoomRequest request
        ) =>
            new DeleteRoomTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.DeleteRoomResult> DeleteRoomAsync(
                Request.DeleteRoomRequest request
        ) =>
            new DeleteRoomTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.DeleteRoomResult>();
    #else
		public DeleteRoomTask DeleteRoomAsync(
                Request.DeleteRoomRequest request
        )
		{
			return new DeleteRoomTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.DeleteRoomResult> DeleteRoomAsync(
                Request.DeleteRoomRequest request
        ) =>
            new DeleteRoomTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class DeleteRoomFromBackendTask : Gs2RestSessionTask<DeleteRoomFromBackendRequest, DeleteRoomFromBackendResult>
        {
            public DeleteRoomFromBackendTask(IGs2Session session, RestSessionRequestFactory factory, DeleteRoomFromBackendRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DeleteRoomFromBackendRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "chat")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/room/{roomName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{roomName}", !string.IsNullOrEmpty(request.RoomName) ? request.RoomName.ToString() : "null");

                var sessionRequest = Factory.Delete(url);
                if (request.ContextStack != null)
                {
                    sessionRequest.AddQueryString("contextStack", request.ContextStack);
                }
                if (request.UserId != null) {
                    sessionRequest.AddQueryString("userId", $"{request.UserId}");
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
		public IEnumerator DeleteRoomFromBackend(
                Request.DeleteRoomFromBackendRequest request,
                UnityAction<AsyncResult<Result.DeleteRoomFromBackendResult>> callback
        ) =>
            new DeleteRoomFromBackendTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.DeleteRoomFromBackendResult> DeleteRoomFromBackendFuture(
                Request.DeleteRoomFromBackendRequest request
        ) =>
            new DeleteRoomFromBackendTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.DeleteRoomFromBackendResult> DeleteRoomFromBackendAsync(
                Request.DeleteRoomFromBackendRequest request
        ) =>
            new DeleteRoomFromBackendTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.DeleteRoomFromBackendResult>();
    #else
		public DeleteRoomFromBackendTask DeleteRoomFromBackendAsync(
                Request.DeleteRoomFromBackendRequest request
        )
		{
			return new DeleteRoomFromBackendTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.DeleteRoomFromBackendResult> DeleteRoomFromBackendAsync(
                Request.DeleteRoomFromBackendRequest request
        ) =>
            new DeleteRoomFromBackendTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class DescribeMessagesTask : Gs2RestSessionTask<DescribeMessagesRequest, DescribeMessagesResult>
        {
            public DescribeMessagesTask(IGs2Session session, RestSessionRequestFactory factory, DescribeMessagesRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DescribeMessagesRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "chat")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/room/{roomName}/message";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{roomName}", !string.IsNullOrEmpty(request.RoomName) ? request.RoomName.ToString() : "null");

                var sessionRequest = Factory.Get(url);
                if (request.ContextStack != null)
                {
                    sessionRequest.AddQueryString("contextStack", request.ContextStack);
                }
                if (request.Password != null) {
                    sessionRequest.AddQueryString("password", $"{request.Password}");
                }
                if (request.Category != null) {
                    sessionRequest.AddQueryString("category", $"{request.Category}");
                }
                if (request.StartAt != null) {
                    sessionRequest.AddQueryString("startAt", $"{request.StartAt}");
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

            public override void OnError(Gs2.Core.Exception.Gs2Exception error)
            {
                if (error.Errors.Count(v => v.code == "room.allowUserIds.notInclude") > 0) {
                    base.OnError(new Exception.NoAccessPrivilegesException(error));
                }
                else if (error.Errors.Count(v => v.code == "room.password.require") > 0) {
                    base.OnError(new Exception.PasswordRequiredException(error));
                }
                else if (error.Errors.Count(v => v.code == "room.password.invalid") > 0) {
                    base.OnError(new Exception.PasswordIncorrectException(error));
                }
                else {
                    base.OnError(error);
                }
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator DescribeMessages(
                Request.DescribeMessagesRequest request,
                UnityAction<AsyncResult<Result.DescribeMessagesResult>> callback
        ) =>
            new DescribeMessagesTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.DescribeMessagesResult> DescribeMessagesFuture(
                Request.DescribeMessagesRequest request
        ) =>
            new DescribeMessagesTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.DescribeMessagesResult> DescribeMessagesAsync(
                Request.DescribeMessagesRequest request
        ) =>
            new DescribeMessagesTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.DescribeMessagesResult>();
    #else
		public DescribeMessagesTask DescribeMessagesAsync(
                Request.DescribeMessagesRequest request
        )
		{
			return new DescribeMessagesTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.DescribeMessagesResult> DescribeMessagesAsync(
                Request.DescribeMessagesRequest request
        ) =>
            new DescribeMessagesTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class DescribeMessagesByUserIdTask : Gs2RestSessionTask<DescribeMessagesByUserIdRequest, DescribeMessagesByUserIdResult>
        {
            public DescribeMessagesByUserIdTask(IGs2Session session, RestSessionRequestFactory factory, DescribeMessagesByUserIdRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DescribeMessagesByUserIdRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "chat")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/room/{roomName}/message/get";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{roomName}", !string.IsNullOrEmpty(request.RoomName) ? request.RoomName.ToString() : "null");

                var sessionRequest = Factory.Get(url);
                if (request.ContextStack != null)
                {
                    sessionRequest.AddQueryString("contextStack", request.ContextStack);
                }
                if (request.Password != null) {
                    sessionRequest.AddQueryString("password", $"{request.Password}");
                }
                if (request.Category != null) {
                    sessionRequest.AddQueryString("category", $"{request.Category}");
                }
                if (request.UserId != null) {
                    sessionRequest.AddQueryString("userId", $"{request.UserId}");
                }
                if (request.StartAt != null) {
                    sessionRequest.AddQueryString("startAt", $"{request.StartAt}");
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

            public override void OnError(Gs2.Core.Exception.Gs2Exception error)
            {
                if (error.Errors.Count(v => v.code == "room.allowUserIds.notInclude") > 0) {
                    base.OnError(new Exception.NoAccessPrivilegesException(error));
                }
                else if (error.Errors.Count(v => v.code == "room.password.require") > 0) {
                    base.OnError(new Exception.PasswordRequiredException(error));
                }
                else if (error.Errors.Count(v => v.code == "room.password.invalid") > 0) {
                    base.OnError(new Exception.PasswordIncorrectException(error));
                }
                else {
                    base.OnError(error);
                }
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator DescribeMessagesByUserId(
                Request.DescribeMessagesByUserIdRequest request,
                UnityAction<AsyncResult<Result.DescribeMessagesByUserIdResult>> callback
        ) =>
            new DescribeMessagesByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.DescribeMessagesByUserIdResult> DescribeMessagesByUserIdFuture(
                Request.DescribeMessagesByUserIdRequest request
        ) =>
            new DescribeMessagesByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.DescribeMessagesByUserIdResult> DescribeMessagesByUserIdAsync(
                Request.DescribeMessagesByUserIdRequest request
        ) =>
            new DescribeMessagesByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.DescribeMessagesByUserIdResult>();
    #else
		public DescribeMessagesByUserIdTask DescribeMessagesByUserIdAsync(
                Request.DescribeMessagesByUserIdRequest request
        )
		{
			return new DescribeMessagesByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.DescribeMessagesByUserIdResult> DescribeMessagesByUserIdAsync(
                Request.DescribeMessagesByUserIdRequest request
        ) =>
            new DescribeMessagesByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class DescribeLatestMessagesTask : Gs2RestSessionTask<DescribeLatestMessagesRequest, DescribeLatestMessagesResult>
        {
            public DescribeLatestMessagesTask(IGs2Session session, RestSessionRequestFactory factory, DescribeLatestMessagesRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DescribeLatestMessagesRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "chat")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/room/{roomName}/message/latest";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{roomName}", !string.IsNullOrEmpty(request.RoomName) ? request.RoomName.ToString() : "null");

                var sessionRequest = Factory.Get(url);
                if (request.ContextStack != null)
                {
                    sessionRequest.AddQueryString("contextStack", request.ContextStack);
                }
                if (request.Password != null) {
                    sessionRequest.AddQueryString("password", $"{request.Password}");
                }
                if (request.Category != null) {
                    sessionRequest.AddQueryString("category", $"{request.Category}");
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

            public override void OnError(Gs2.Core.Exception.Gs2Exception error)
            {
                if (error.Errors.Count(v => v.code == "room.allowUserIds.notInclude") > 0) {
                    base.OnError(new Exception.NoAccessPrivilegesException(error));
                }
                else if (error.Errors.Count(v => v.code == "room.password.require") > 0) {
                    base.OnError(new Exception.PasswordRequiredException(error));
                }
                else if (error.Errors.Count(v => v.code == "room.password.invalid") > 0) {
                    base.OnError(new Exception.PasswordIncorrectException(error));
                }
                else {
                    base.OnError(error);
                }
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator DescribeLatestMessages(
                Request.DescribeLatestMessagesRequest request,
                UnityAction<AsyncResult<Result.DescribeLatestMessagesResult>> callback
        ) =>
            new DescribeLatestMessagesTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.DescribeLatestMessagesResult> DescribeLatestMessagesFuture(
                Request.DescribeLatestMessagesRequest request
        ) =>
            new DescribeLatestMessagesTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.DescribeLatestMessagesResult> DescribeLatestMessagesAsync(
                Request.DescribeLatestMessagesRequest request
        ) =>
            new DescribeLatestMessagesTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.DescribeLatestMessagesResult>();
    #else
		public DescribeLatestMessagesTask DescribeLatestMessagesAsync(
                Request.DescribeLatestMessagesRequest request
        )
		{
			return new DescribeLatestMessagesTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.DescribeLatestMessagesResult> DescribeLatestMessagesAsync(
                Request.DescribeLatestMessagesRequest request
        ) =>
            new DescribeLatestMessagesTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class DescribeLatestMessagesByUserIdTask : Gs2RestSessionTask<DescribeLatestMessagesByUserIdRequest, DescribeLatestMessagesByUserIdResult>
        {
            public DescribeLatestMessagesByUserIdTask(IGs2Session session, RestSessionRequestFactory factory, DescribeLatestMessagesByUserIdRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DescribeLatestMessagesByUserIdRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "chat")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/room/{roomName}/message/latest/get";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{roomName}", !string.IsNullOrEmpty(request.RoomName) ? request.RoomName.ToString() : "null");

                var sessionRequest = Factory.Get(url);
                if (request.ContextStack != null)
                {
                    sessionRequest.AddQueryString("contextStack", request.ContextStack);
                }
                if (request.Password != null) {
                    sessionRequest.AddQueryString("password", $"{request.Password}");
                }
                if (request.Category != null) {
                    sessionRequest.AddQueryString("category", $"{request.Category}");
                }
                if (request.UserId != null) {
                    sessionRequest.AddQueryString("userId", $"{request.UserId}");
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

            public override void OnError(Gs2.Core.Exception.Gs2Exception error)
            {
                if (error.Errors.Count(v => v.code == "room.allowUserIds.notInclude") > 0) {
                    base.OnError(new Exception.NoAccessPrivilegesException(error));
                }
                else if (error.Errors.Count(v => v.code == "room.password.require") > 0) {
                    base.OnError(new Exception.PasswordRequiredException(error));
                }
                else if (error.Errors.Count(v => v.code == "room.password.invalid") > 0) {
                    base.OnError(new Exception.PasswordIncorrectException(error));
                }
                else {
                    base.OnError(error);
                }
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator DescribeLatestMessagesByUserId(
                Request.DescribeLatestMessagesByUserIdRequest request,
                UnityAction<AsyncResult<Result.DescribeLatestMessagesByUserIdResult>> callback
        ) =>
            new DescribeLatestMessagesByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.DescribeLatestMessagesByUserIdResult> DescribeLatestMessagesByUserIdFuture(
                Request.DescribeLatestMessagesByUserIdRequest request
        ) =>
            new DescribeLatestMessagesByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.DescribeLatestMessagesByUserIdResult> DescribeLatestMessagesByUserIdAsync(
                Request.DescribeLatestMessagesByUserIdRequest request
        ) =>
            new DescribeLatestMessagesByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.DescribeLatestMessagesByUserIdResult>();
    #else
		public DescribeLatestMessagesByUserIdTask DescribeLatestMessagesByUserIdAsync(
                Request.DescribeLatestMessagesByUserIdRequest request
        )
		{
			return new DescribeLatestMessagesByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.DescribeLatestMessagesByUserIdResult> DescribeLatestMessagesByUserIdAsync(
                Request.DescribeLatestMessagesByUserIdRequest request
        ) =>
            new DescribeLatestMessagesByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class PostTask : Gs2RestSessionTask<PostRequest, PostResult>
        {
            public PostTask(IGs2Session session, RestSessionRequestFactory factory, PostRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(PostRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "chat")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/room/{roomName}/message";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{roomName}", !string.IsNullOrEmpty(request.RoomName) ? request.RoomName.ToString() : "null");

                var sessionRequest = Factory.Post(url);

                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);
                jsonWriter.WriteObjectStart();
                if (request.Category != null)
                {
                    jsonWriter.WritePropertyName("category");
                    jsonWriter.Write(request.Category.ToString());
                }
                if (request.Metadata != null)
                {
                    jsonWriter.WritePropertyName("metadata");
                    jsonWriter.Write(request.Metadata);
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
                if (error.Errors.Count(v => v.code == "room.allowUserIds.notInclude") > 0) {
                    base.OnError(new Exception.NoAccessPrivilegesException(error));
                }
                else if (error.Errors.Count(v => v.code == "room.password.require") > 0) {
                    base.OnError(new Exception.PasswordRequiredException(error));
                }
                else if (error.Errors.Count(v => v.code == "room.password.invalid") > 0) {
                    base.OnError(new Exception.PasswordIncorrectException(error));
                }
                else {
                    base.OnError(error);
                }
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator Post(
                Request.PostRequest request,
                UnityAction<AsyncResult<Result.PostResult>> callback
        ) =>
            new PostTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.PostResult> PostFuture(
                Request.PostRequest request
        ) =>
            new PostTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.PostResult> PostAsync(
                Request.PostRequest request
        ) =>
            new PostTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.PostResult>();
    #else
		public PostTask PostAsync(
                Request.PostRequest request
        )
		{
			return new PostTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.PostResult> PostAsync(
                Request.PostRequest request
        ) =>
            new PostTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class PostByUserIdTask : Gs2RestSessionTask<PostByUserIdRequest, PostByUserIdResult>
        {
            public PostByUserIdTask(IGs2Session session, RestSessionRequestFactory factory, PostByUserIdRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(PostByUserIdRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "chat")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/room/{roomName}/message/user/{userId}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{roomName}", !string.IsNullOrEmpty(request.RoomName) ? request.RoomName.ToString() : "null");
                url = url.Replace("{userId}", !string.IsNullOrEmpty(request.UserId) ? request.UserId.ToString() : "null");

                var sessionRequest = Factory.Post(url);

                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);
                jsonWriter.WriteObjectStart();
                if (request.Category != null)
                {
                    jsonWriter.WritePropertyName("category");
                    jsonWriter.Write(request.Category.ToString());
                }
                if (request.Metadata != null)
                {
                    jsonWriter.WritePropertyName("metadata");
                    jsonWriter.Write(request.Metadata);
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
                if (error.Errors.Count(v => v.code == "room.allowUserIds.notInclude") > 0) {
                    base.OnError(new Exception.NoAccessPrivilegesException(error));
                }
                else if (error.Errors.Count(v => v.code == "room.password.require") > 0) {
                    base.OnError(new Exception.PasswordRequiredException(error));
                }
                else if (error.Errors.Count(v => v.code == "room.password.invalid") > 0) {
                    base.OnError(new Exception.PasswordIncorrectException(error));
                }
                else {
                    base.OnError(error);
                }
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator PostByUserId(
                Request.PostByUserIdRequest request,
                UnityAction<AsyncResult<Result.PostByUserIdResult>> callback
        ) =>
            new PostByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.PostByUserIdResult> PostByUserIdFuture(
                Request.PostByUserIdRequest request
        ) =>
            new PostByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.PostByUserIdResult> PostByUserIdAsync(
                Request.PostByUserIdRequest request
        ) =>
            new PostByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.PostByUserIdResult>();
    #else
		public PostByUserIdTask PostByUserIdAsync(
                Request.PostByUserIdRequest request
        )
		{
			return new PostByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.PostByUserIdResult> PostByUserIdAsync(
                Request.PostByUserIdRequest request
        ) =>
            new PostByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class GetMessageTask : Gs2RestSessionTask<GetMessageRequest, GetMessageResult>
        {
            public GetMessageTask(IGs2Session session, RestSessionRequestFactory factory, GetMessageRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(GetMessageRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "chat")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/room/{roomName}/message/{messageName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{roomName}", !string.IsNullOrEmpty(request.RoomName) ? request.RoomName.ToString() : "null");
                url = url.Replace("{messageName}", !string.IsNullOrEmpty(request.MessageName) ? request.MessageName.ToString() : "null");

                var sessionRequest = Factory.Get(url);
                if (request.ContextStack != null)
                {
                    sessionRequest.AddQueryString("contextStack", request.ContextStack);
                }
                if (request.Password != null) {
                    sessionRequest.AddQueryString("password", $"{request.Password}");
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

            public override void OnError(Gs2.Core.Exception.Gs2Exception error)
            {
                if (error.Errors.Count(v => v.code == "room.allowUserIds.notInclude") > 0) {
                    base.OnError(new Exception.NoAccessPrivilegesException(error));
                }
                else if (error.Errors.Count(v => v.code == "room.password.require") > 0) {
                    base.OnError(new Exception.PasswordRequiredException(error));
                }
                else if (error.Errors.Count(v => v.code == "room.password.invalid") > 0) {
                    base.OnError(new Exception.PasswordIncorrectException(error));
                }
                else {
                    base.OnError(error);
                }
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator GetMessage(
                Request.GetMessageRequest request,
                UnityAction<AsyncResult<Result.GetMessageResult>> callback
        ) =>
            new GetMessageTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.GetMessageResult> GetMessageFuture(
                Request.GetMessageRequest request
        ) =>
            new GetMessageTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.GetMessageResult> GetMessageAsync(
                Request.GetMessageRequest request
        ) =>
            new GetMessageTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.GetMessageResult>();
    #else
		public GetMessageTask GetMessageAsync(
                Request.GetMessageRequest request
        )
		{
			return new GetMessageTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.GetMessageResult> GetMessageAsync(
                Request.GetMessageRequest request
        ) =>
            new GetMessageTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class GetMessageByUserIdTask : Gs2RestSessionTask<GetMessageByUserIdRequest, GetMessageByUserIdResult>
        {
            public GetMessageByUserIdTask(IGs2Session session, RestSessionRequestFactory factory, GetMessageByUserIdRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(GetMessageByUserIdRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "chat")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/room/{roomName}/message/{messageName}/get";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{roomName}", !string.IsNullOrEmpty(request.RoomName) ? request.RoomName.ToString() : "null");
                url = url.Replace("{messageName}", !string.IsNullOrEmpty(request.MessageName) ? request.MessageName.ToString() : "null");

                var sessionRequest = Factory.Get(url);
                if (request.ContextStack != null)
                {
                    sessionRequest.AddQueryString("contextStack", request.ContextStack);
                }
                if (request.Password != null) {
                    sessionRequest.AddQueryString("password", $"{request.Password}");
                }
                if (request.UserId != null) {
                    sessionRequest.AddQueryString("userId", $"{request.UserId}");
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
                if (error.Errors.Count(v => v.code == "room.allowUserIds.notInclude") > 0) {
                    base.OnError(new Exception.NoAccessPrivilegesException(error));
                }
                else if (error.Errors.Count(v => v.code == "room.password.require") > 0) {
                    base.OnError(new Exception.PasswordRequiredException(error));
                }
                else if (error.Errors.Count(v => v.code == "room.password.invalid") > 0) {
                    base.OnError(new Exception.PasswordIncorrectException(error));
                }
                else {
                    base.OnError(error);
                }
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator GetMessageByUserId(
                Request.GetMessageByUserIdRequest request,
                UnityAction<AsyncResult<Result.GetMessageByUserIdResult>> callback
        ) =>
            new GetMessageByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.GetMessageByUserIdResult> GetMessageByUserIdFuture(
                Request.GetMessageByUserIdRequest request
        ) =>
            new GetMessageByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.GetMessageByUserIdResult> GetMessageByUserIdAsync(
                Request.GetMessageByUserIdRequest request
        ) =>
            new GetMessageByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.GetMessageByUserIdResult>();
    #else
		public GetMessageByUserIdTask GetMessageByUserIdAsync(
                Request.GetMessageByUserIdRequest request
        )
		{
			return new GetMessageByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.GetMessageByUserIdResult> GetMessageByUserIdAsync(
                Request.GetMessageByUserIdRequest request
        ) =>
            new GetMessageByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class DeleteMessageTask : Gs2RestSessionTask<DeleteMessageRequest, DeleteMessageResult>
        {
            public DeleteMessageTask(IGs2Session session, RestSessionRequestFactory factory, DeleteMessageRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DeleteMessageRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "chat")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/room/{roomName}/message/{messageName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{roomName}", !string.IsNullOrEmpty(request.RoomName) ? request.RoomName.ToString() : "null");
                url = url.Replace("{messageName}", !string.IsNullOrEmpty(request.MessageName) ? request.MessageName.ToString() : "null");

                var sessionRequest = Factory.Delete(url);
                if (request.ContextStack != null)
                {
                    sessionRequest.AddQueryString("contextStack", request.ContextStack);
                }
                if (request.UserId != null) {
                    sessionRequest.AddQueryString("userId", $"{request.UserId}");
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
		public IEnumerator DeleteMessage(
                Request.DeleteMessageRequest request,
                UnityAction<AsyncResult<Result.DeleteMessageResult>> callback
        ) =>
            new DeleteMessageTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.DeleteMessageResult> DeleteMessageFuture(
                Request.DeleteMessageRequest request
        ) =>
            new DeleteMessageTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.DeleteMessageResult> DeleteMessageAsync(
                Request.DeleteMessageRequest request
        ) =>
            new DeleteMessageTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.DeleteMessageResult>();
    #else
		public DeleteMessageTask DeleteMessageAsync(
                Request.DeleteMessageRequest request
        )
		{
			return new DeleteMessageTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.DeleteMessageResult> DeleteMessageAsync(
                Request.DeleteMessageRequest request
        ) =>
            new DeleteMessageTask(
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
                    .Replace("{service}", "chat")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/me/room/subscribe";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");

                var sessionRequest = Factory.Get(url);
                if (request.ContextStack != null)
                {
                    sessionRequest.AddQueryString("contextStack", request.ContextStack);
                }
                if (request.RoomNamePrefix != null) {
                    sessionRequest.AddQueryString("roomNamePrefix", $"{request.RoomNamePrefix}");
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
                    .Replace("{service}", "chat")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/{userId}/room/subscribe";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{userId}", !string.IsNullOrEmpty(request.UserId) ? request.UserId.ToString() : "null");

                var sessionRequest = Factory.Get(url);
                if (request.ContextStack != null)
                {
                    sessionRequest.AddQueryString("contextStack", request.ContextStack);
                }
                if (request.RoomNamePrefix != null) {
                    sessionRequest.AddQueryString("roomNamePrefix", $"{request.RoomNamePrefix}");
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


        public class DescribeSubscribesByRoomNameTask : Gs2RestSessionTask<DescribeSubscribesByRoomNameRequest, DescribeSubscribesByRoomNameResult>
        {
            public DescribeSubscribesByRoomNameTask(IGs2Session session, RestSessionRequestFactory factory, DescribeSubscribesByRoomNameRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DescribeSubscribesByRoomNameRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "chat")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/room/{roomName}/subscribe";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{roomName}", !string.IsNullOrEmpty(request.RoomName) ? request.RoomName.ToString() : "null");

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
		public IEnumerator DescribeSubscribesByRoomName(
                Request.DescribeSubscribesByRoomNameRequest request,
                UnityAction<AsyncResult<Result.DescribeSubscribesByRoomNameResult>> callback
        ) =>
            new DescribeSubscribesByRoomNameTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.DescribeSubscribesByRoomNameResult> DescribeSubscribesByRoomNameFuture(
                Request.DescribeSubscribesByRoomNameRequest request
        ) =>
            new DescribeSubscribesByRoomNameTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.DescribeSubscribesByRoomNameResult> DescribeSubscribesByRoomNameAsync(
                Request.DescribeSubscribesByRoomNameRequest request
        ) =>
            new DescribeSubscribesByRoomNameTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.DescribeSubscribesByRoomNameResult>();
    #else
		public DescribeSubscribesByRoomNameTask DescribeSubscribesByRoomNameAsync(
                Request.DescribeSubscribesByRoomNameRequest request
        )
		{
			return new DescribeSubscribesByRoomNameTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.DescribeSubscribesByRoomNameResult> DescribeSubscribesByRoomNameAsync(
                Request.DescribeSubscribesByRoomNameRequest request
        ) =>
            new DescribeSubscribesByRoomNameTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class SubscribeTask : Gs2RestSessionTask<SubscribeRequest, SubscribeResult>
        {
            public SubscribeTask(IGs2Session session, RestSessionRequestFactory factory, SubscribeRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(SubscribeRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "chat")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/me/room/{roomName}/subscribe";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{roomName}", !string.IsNullOrEmpty(request.RoomName) ? request.RoomName.ToString() : "null");

                var sessionRequest = Factory.Post(url);

                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);
                jsonWriter.WriteObjectStart();
                if (request.NotificationTypes != null)
                {
                    jsonWriter.WritePropertyName("notificationTypes");
                    jsonWriter.WriteArrayStart();
                    foreach(var item in request.NotificationTypes)
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
		public IEnumerator Subscribe(
                Request.SubscribeRequest request,
                UnityAction<AsyncResult<Result.SubscribeResult>> callback
        ) =>
            new SubscribeTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.SubscribeResult> SubscribeFuture(
                Request.SubscribeRequest request
        ) =>
            new SubscribeTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.SubscribeResult> SubscribeAsync(
                Request.SubscribeRequest request
        ) =>
            new SubscribeTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.SubscribeResult>();
    #else
		public SubscribeTask SubscribeAsync(
                Request.SubscribeRequest request
        )
		{
			return new SubscribeTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.SubscribeResult> SubscribeAsync(
                Request.SubscribeRequest request
        ) =>
            new SubscribeTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class SubscribeByUserIdTask : Gs2RestSessionTask<SubscribeByUserIdRequest, SubscribeByUserIdResult>
        {
            public SubscribeByUserIdTask(IGs2Session session, RestSessionRequestFactory factory, SubscribeByUserIdRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(SubscribeByUserIdRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "chat")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/{userId}/room/{roomName}/subscribe";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{roomName}", !string.IsNullOrEmpty(request.RoomName) ? request.RoomName.ToString() : "null");
                url = url.Replace("{userId}", !string.IsNullOrEmpty(request.UserId) ? request.UserId.ToString() : "null");

                var sessionRequest = Factory.Post(url);

                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);
                jsonWriter.WriteObjectStart();
                if (request.NotificationTypes != null)
                {
                    jsonWriter.WritePropertyName("notificationTypes");
                    jsonWriter.WriteArrayStart();
                    foreach(var item in request.NotificationTypes)
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
		public IEnumerator SubscribeByUserId(
                Request.SubscribeByUserIdRequest request,
                UnityAction<AsyncResult<Result.SubscribeByUserIdResult>> callback
        ) =>
            new SubscribeByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.SubscribeByUserIdResult> SubscribeByUserIdFuture(
                Request.SubscribeByUserIdRequest request
        ) =>
            new SubscribeByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.SubscribeByUserIdResult> SubscribeByUserIdAsync(
                Request.SubscribeByUserIdRequest request
        ) =>
            new SubscribeByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.SubscribeByUserIdResult>();
    #else
		public SubscribeByUserIdTask SubscribeByUserIdAsync(
                Request.SubscribeByUserIdRequest request
        )
		{
			return new SubscribeByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.SubscribeByUserIdResult> SubscribeByUserIdAsync(
                Request.SubscribeByUserIdRequest request
        ) =>
            new SubscribeByUserIdTask(
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
                    .Replace("{service}", "chat")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/me/room/{roomName}/subscribe";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{roomName}", !string.IsNullOrEmpty(request.RoomName) ? request.RoomName.ToString() : "null");

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
                    .Replace("{service}", "chat")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/{userId}/room/{roomName}/subscribe";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{roomName}", !string.IsNullOrEmpty(request.RoomName) ? request.RoomName.ToString() : "null");
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


        public class UpdateNotificationTypeTask : Gs2RestSessionTask<UpdateNotificationTypeRequest, UpdateNotificationTypeResult>
        {
            public UpdateNotificationTypeTask(IGs2Session session, RestSessionRequestFactory factory, UpdateNotificationTypeRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(UpdateNotificationTypeRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "chat")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/me/room/{roomName}/subscribe/notification";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{roomName}", !string.IsNullOrEmpty(request.RoomName) ? request.RoomName.ToString() : "null");

                var sessionRequest = Factory.Post(url);

                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);
                jsonWriter.WriteObjectStart();
                if (request.NotificationTypes != null)
                {
                    jsonWriter.WritePropertyName("notificationTypes");
                    jsonWriter.WriteArrayStart();
                    foreach(var item in request.NotificationTypes)
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
		public IEnumerator UpdateNotificationType(
                Request.UpdateNotificationTypeRequest request,
                UnityAction<AsyncResult<Result.UpdateNotificationTypeResult>> callback
        ) =>
            new UpdateNotificationTypeTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.UpdateNotificationTypeResult> UpdateNotificationTypeFuture(
                Request.UpdateNotificationTypeRequest request
        ) =>
            new UpdateNotificationTypeTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.UpdateNotificationTypeResult> UpdateNotificationTypeAsync(
                Request.UpdateNotificationTypeRequest request
        ) =>
            new UpdateNotificationTypeTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.UpdateNotificationTypeResult>();
    #else
		public UpdateNotificationTypeTask UpdateNotificationTypeAsync(
                Request.UpdateNotificationTypeRequest request
        )
		{
			return new UpdateNotificationTypeTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.UpdateNotificationTypeResult> UpdateNotificationTypeAsync(
                Request.UpdateNotificationTypeRequest request
        ) =>
            new UpdateNotificationTypeTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class UpdateNotificationTypeByUserIdTask : Gs2RestSessionTask<UpdateNotificationTypeByUserIdRequest, UpdateNotificationTypeByUserIdResult>
        {
            public UpdateNotificationTypeByUserIdTask(IGs2Session session, RestSessionRequestFactory factory, UpdateNotificationTypeByUserIdRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(UpdateNotificationTypeByUserIdRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "chat")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/{userId}/room/{roomName}/subscribe/notification";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{roomName}", !string.IsNullOrEmpty(request.RoomName) ? request.RoomName.ToString() : "null");
                url = url.Replace("{userId}", !string.IsNullOrEmpty(request.UserId) ? request.UserId.ToString() : "null");

                var sessionRequest = Factory.Post(url);

                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);
                jsonWriter.WriteObjectStart();
                if (request.NotificationTypes != null)
                {
                    jsonWriter.WritePropertyName("notificationTypes");
                    jsonWriter.WriteArrayStart();
                    foreach(var item in request.NotificationTypes)
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
		public IEnumerator UpdateNotificationTypeByUserId(
                Request.UpdateNotificationTypeByUserIdRequest request,
                UnityAction<AsyncResult<Result.UpdateNotificationTypeByUserIdResult>> callback
        ) =>
            new UpdateNotificationTypeByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.UpdateNotificationTypeByUserIdResult> UpdateNotificationTypeByUserIdFuture(
                Request.UpdateNotificationTypeByUserIdRequest request
        ) =>
            new UpdateNotificationTypeByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.UpdateNotificationTypeByUserIdResult> UpdateNotificationTypeByUserIdAsync(
                Request.UpdateNotificationTypeByUserIdRequest request
        ) =>
            new UpdateNotificationTypeByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.UpdateNotificationTypeByUserIdResult>();
    #else
		public UpdateNotificationTypeByUserIdTask UpdateNotificationTypeByUserIdAsync(
                Request.UpdateNotificationTypeByUserIdRequest request
        )
		{
			return new UpdateNotificationTypeByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.UpdateNotificationTypeByUserIdResult> UpdateNotificationTypeByUserIdAsync(
                Request.UpdateNotificationTypeByUserIdRequest request
        ) =>
            new UpdateNotificationTypeByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class UnsubscribeTask : Gs2RestSessionTask<UnsubscribeRequest, UnsubscribeResult>
        {
            public UnsubscribeTask(IGs2Session session, RestSessionRequestFactory factory, UnsubscribeRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(UnsubscribeRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "chat")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/me/room/{roomName}/subscribe";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{roomName}", !string.IsNullOrEmpty(request.RoomName) ? request.RoomName.ToString() : "null");

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
		public IEnumerator Unsubscribe(
                Request.UnsubscribeRequest request,
                UnityAction<AsyncResult<Result.UnsubscribeResult>> callback
        ) =>
            new UnsubscribeTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.UnsubscribeResult> UnsubscribeFuture(
                Request.UnsubscribeRequest request
        ) =>
            new UnsubscribeTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.UnsubscribeResult> UnsubscribeAsync(
                Request.UnsubscribeRequest request
        ) =>
            new UnsubscribeTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.UnsubscribeResult>();
    #else
		public UnsubscribeTask UnsubscribeAsync(
                Request.UnsubscribeRequest request
        )
		{
			return new UnsubscribeTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.UnsubscribeResult> UnsubscribeAsync(
                Request.UnsubscribeRequest request
        ) =>
            new UnsubscribeTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class UnsubscribeByUserIdTask : Gs2RestSessionTask<UnsubscribeByUserIdRequest, UnsubscribeByUserIdResult>
        {
            public UnsubscribeByUserIdTask(IGs2Session session, RestSessionRequestFactory factory, UnsubscribeByUserIdRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(UnsubscribeByUserIdRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "chat")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/{userId}/room/{roomName}/subscribe";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{roomName}", !string.IsNullOrEmpty(request.RoomName) ? request.RoomName.ToString() : "null");
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
		public IEnumerator UnsubscribeByUserId(
                Request.UnsubscribeByUserIdRequest request,
                UnityAction<AsyncResult<Result.UnsubscribeByUserIdResult>> callback
        ) =>
            new UnsubscribeByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.UnsubscribeByUserIdResult> UnsubscribeByUserIdFuture(
                Request.UnsubscribeByUserIdRequest request
        ) =>
            new UnsubscribeByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.UnsubscribeByUserIdResult> UnsubscribeByUserIdAsync(
                Request.UnsubscribeByUserIdRequest request
        ) =>
            new UnsubscribeByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.UnsubscribeByUserIdResult>();
    #else
		public UnsubscribeByUserIdTask UnsubscribeByUserIdAsync(
                Request.UnsubscribeByUserIdRequest request
        )
		{
			return new UnsubscribeByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.UnsubscribeByUserIdResult> UnsubscribeByUserIdAsync(
                Request.UnsubscribeByUserIdRequest request
        ) =>
            new UnsubscribeByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class DescribeCategoryModelsTask : Gs2RestSessionTask<DescribeCategoryModelsRequest, DescribeCategoryModelsResult>
        {
            public DescribeCategoryModelsTask(IGs2Session session, RestSessionRequestFactory factory, DescribeCategoryModelsRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DescribeCategoryModelsRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "chat")
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
		public IEnumerator DescribeCategoryModels(
                Request.DescribeCategoryModelsRequest request,
                UnityAction<AsyncResult<Result.DescribeCategoryModelsResult>> callback
        ) =>
            new DescribeCategoryModelsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.DescribeCategoryModelsResult> DescribeCategoryModelsFuture(
                Request.DescribeCategoryModelsRequest request
        ) =>
            new DescribeCategoryModelsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.DescribeCategoryModelsResult> DescribeCategoryModelsAsync(
                Request.DescribeCategoryModelsRequest request
        ) =>
            new DescribeCategoryModelsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.DescribeCategoryModelsResult>();
    #else
		public DescribeCategoryModelsTask DescribeCategoryModelsAsync(
                Request.DescribeCategoryModelsRequest request
        )
		{
			return new DescribeCategoryModelsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.DescribeCategoryModelsResult> DescribeCategoryModelsAsync(
                Request.DescribeCategoryModelsRequest request
        ) =>
            new DescribeCategoryModelsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class GetCategoryModelTask : Gs2RestSessionTask<GetCategoryModelRequest, GetCategoryModelResult>
        {
            public GetCategoryModelTask(IGs2Session session, RestSessionRequestFactory factory, GetCategoryModelRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(GetCategoryModelRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "chat")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/model/{category}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{category}",request.Category != null ? request.Category.ToString() : "null");

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
		public IEnumerator GetCategoryModel(
                Request.GetCategoryModelRequest request,
                UnityAction<AsyncResult<Result.GetCategoryModelResult>> callback
        ) =>
            new GetCategoryModelTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.GetCategoryModelResult> GetCategoryModelFuture(
                Request.GetCategoryModelRequest request
        ) =>
            new GetCategoryModelTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.GetCategoryModelResult> GetCategoryModelAsync(
                Request.GetCategoryModelRequest request
        ) =>
            new GetCategoryModelTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.GetCategoryModelResult>();
    #else
		public GetCategoryModelTask GetCategoryModelAsync(
                Request.GetCategoryModelRequest request
        )
		{
			return new GetCategoryModelTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.GetCategoryModelResult> GetCategoryModelAsync(
                Request.GetCategoryModelRequest request
        ) =>
            new GetCategoryModelTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class DescribeCategoryModelMastersTask : Gs2RestSessionTask<DescribeCategoryModelMastersRequest, DescribeCategoryModelMastersResult>
        {
            public DescribeCategoryModelMastersTask(IGs2Session session, RestSessionRequestFactory factory, DescribeCategoryModelMastersRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DescribeCategoryModelMastersRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "chat")
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
		public IEnumerator DescribeCategoryModelMasters(
                Request.DescribeCategoryModelMastersRequest request,
                UnityAction<AsyncResult<Result.DescribeCategoryModelMastersResult>> callback
        ) =>
            new DescribeCategoryModelMastersTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.DescribeCategoryModelMastersResult> DescribeCategoryModelMastersFuture(
                Request.DescribeCategoryModelMastersRequest request
        ) =>
            new DescribeCategoryModelMastersTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.DescribeCategoryModelMastersResult> DescribeCategoryModelMastersAsync(
                Request.DescribeCategoryModelMastersRequest request
        ) =>
            new DescribeCategoryModelMastersTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.DescribeCategoryModelMastersResult>();
    #else
		public DescribeCategoryModelMastersTask DescribeCategoryModelMastersAsync(
                Request.DescribeCategoryModelMastersRequest request
        )
		{
			return new DescribeCategoryModelMastersTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.DescribeCategoryModelMastersResult> DescribeCategoryModelMastersAsync(
                Request.DescribeCategoryModelMastersRequest request
        ) =>
            new DescribeCategoryModelMastersTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class CreateCategoryModelMasterTask : Gs2RestSessionTask<CreateCategoryModelMasterRequest, CreateCategoryModelMasterResult>
        {
            public CreateCategoryModelMasterTask(IGs2Session session, RestSessionRequestFactory factory, CreateCategoryModelMasterRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(CreateCategoryModelMasterRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "chat")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/master/model";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");

                var sessionRequest = Factory.Post(url);

                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);
                jsonWriter.WriteObjectStart();
                if (request.Category != null)
                {
                    jsonWriter.WritePropertyName("category");
                    jsonWriter.Write(request.Category.ToString());
                }
                if (request.Description != null)
                {
                    jsonWriter.WritePropertyName("description");
                    jsonWriter.Write(request.Description);
                }
                if (request.RejectAccessTokenPost != null)
                {
                    jsonWriter.WritePropertyName("rejectAccessTokenPost");
                    jsonWriter.Write(request.RejectAccessTokenPost);
                }
                if (request.ContextStack != null)
                {
                    jsonWriter.WritePropertyName("contextStack");
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
		public IEnumerator CreateCategoryModelMaster(
                Request.CreateCategoryModelMasterRequest request,
                UnityAction<AsyncResult<Result.CreateCategoryModelMasterResult>> callback
        ) =>
            new CreateCategoryModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.CreateCategoryModelMasterResult> CreateCategoryModelMasterFuture(
                Request.CreateCategoryModelMasterRequest request
        ) =>
            new CreateCategoryModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.CreateCategoryModelMasterResult> CreateCategoryModelMasterAsync(
                Request.CreateCategoryModelMasterRequest request
        ) =>
            new CreateCategoryModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.CreateCategoryModelMasterResult>();
    #else
		public CreateCategoryModelMasterTask CreateCategoryModelMasterAsync(
                Request.CreateCategoryModelMasterRequest request
        )
		{
			return new CreateCategoryModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.CreateCategoryModelMasterResult> CreateCategoryModelMasterAsync(
                Request.CreateCategoryModelMasterRequest request
        ) =>
            new CreateCategoryModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class GetCategoryModelMasterTask : Gs2RestSessionTask<GetCategoryModelMasterRequest, GetCategoryModelMasterResult>
        {
            public GetCategoryModelMasterTask(IGs2Session session, RestSessionRequestFactory factory, GetCategoryModelMasterRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(GetCategoryModelMasterRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "chat")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/master/model/{category}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{category}",request.Category != null ? request.Category.ToString() : "null");

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
		public IEnumerator GetCategoryModelMaster(
                Request.GetCategoryModelMasterRequest request,
                UnityAction<AsyncResult<Result.GetCategoryModelMasterResult>> callback
        ) =>
            new GetCategoryModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.GetCategoryModelMasterResult> GetCategoryModelMasterFuture(
                Request.GetCategoryModelMasterRequest request
        ) =>
            new GetCategoryModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.GetCategoryModelMasterResult> GetCategoryModelMasterAsync(
                Request.GetCategoryModelMasterRequest request
        ) =>
            new GetCategoryModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.GetCategoryModelMasterResult>();
    #else
		public GetCategoryModelMasterTask GetCategoryModelMasterAsync(
                Request.GetCategoryModelMasterRequest request
        )
		{
			return new GetCategoryModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.GetCategoryModelMasterResult> GetCategoryModelMasterAsync(
                Request.GetCategoryModelMasterRequest request
        ) =>
            new GetCategoryModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class UpdateCategoryModelMasterTask : Gs2RestSessionTask<UpdateCategoryModelMasterRequest, UpdateCategoryModelMasterResult>
        {
            public UpdateCategoryModelMasterTask(IGs2Session session, RestSessionRequestFactory factory, UpdateCategoryModelMasterRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(UpdateCategoryModelMasterRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "chat")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/master/model/{category}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{category}",request.Category != null ? request.Category.ToString() : "null");

                var sessionRequest = Factory.Put(url);

                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);
                jsonWriter.WriteObjectStart();
                if (request.Description != null)
                {
                    jsonWriter.WritePropertyName("description");
                    jsonWriter.Write(request.Description);
                }
                if (request.RejectAccessTokenPost != null)
                {
                    jsonWriter.WritePropertyName("rejectAccessTokenPost");
                    jsonWriter.Write(request.RejectAccessTokenPost);
                }
                if (request.ContextStack != null)
                {
                    jsonWriter.WritePropertyName("contextStack");
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
		public IEnumerator UpdateCategoryModelMaster(
                Request.UpdateCategoryModelMasterRequest request,
                UnityAction<AsyncResult<Result.UpdateCategoryModelMasterResult>> callback
        ) =>
            new UpdateCategoryModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.UpdateCategoryModelMasterResult> UpdateCategoryModelMasterFuture(
                Request.UpdateCategoryModelMasterRequest request
        ) =>
            new UpdateCategoryModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.UpdateCategoryModelMasterResult> UpdateCategoryModelMasterAsync(
                Request.UpdateCategoryModelMasterRequest request
        ) =>
            new UpdateCategoryModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.UpdateCategoryModelMasterResult>();
    #else
		public UpdateCategoryModelMasterTask UpdateCategoryModelMasterAsync(
                Request.UpdateCategoryModelMasterRequest request
        )
		{
			return new UpdateCategoryModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.UpdateCategoryModelMasterResult> UpdateCategoryModelMasterAsync(
                Request.UpdateCategoryModelMasterRequest request
        ) =>
            new UpdateCategoryModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class DeleteCategoryModelMasterTask : Gs2RestSessionTask<DeleteCategoryModelMasterRequest, DeleteCategoryModelMasterResult>
        {
            public DeleteCategoryModelMasterTask(IGs2Session session, RestSessionRequestFactory factory, DeleteCategoryModelMasterRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DeleteCategoryModelMasterRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "chat")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/master/model/{category}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{category}",request.Category != null ? request.Category.ToString() : "null");

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
		public IEnumerator DeleteCategoryModelMaster(
                Request.DeleteCategoryModelMasterRequest request,
                UnityAction<AsyncResult<Result.DeleteCategoryModelMasterResult>> callback
        ) =>
            new DeleteCategoryModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.DeleteCategoryModelMasterResult> DeleteCategoryModelMasterFuture(
                Request.DeleteCategoryModelMasterRequest request
        ) =>
            new DeleteCategoryModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.DeleteCategoryModelMasterResult> DeleteCategoryModelMasterAsync(
                Request.DeleteCategoryModelMasterRequest request
        ) =>
            new DeleteCategoryModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.DeleteCategoryModelMasterResult>();
    #else
		public DeleteCategoryModelMasterTask DeleteCategoryModelMasterAsync(
                Request.DeleteCategoryModelMasterRequest request
        )
		{
			return new DeleteCategoryModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.DeleteCategoryModelMasterResult> DeleteCategoryModelMasterAsync(
                Request.DeleteCategoryModelMasterRequest request
        ) =>
            new DeleteCategoryModelMasterTask(
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
                    .Replace("{service}", "chat")
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
                    .Replace("{service}", "chat")
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
                    .Replace("{service}", "chat")
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
                    .Replace("{service}", "chat")
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
                    .Replace("{service}", "chat")
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