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
using Gs2.Gs2Showcase.Request;
using Gs2.Gs2Showcase.Result;
using Gs2.Util.LitJson;

namespace Gs2.Gs2Showcase
{
	public class Gs2ShowcaseRestClient : AbstractGs2Client
	{
#if UNITY_2017_1_OR_NEWER
		private readonly CertificateHandler _certificateHandler;
#endif

		public static string Endpoint = "showcase";

        protected Gs2RestSession Gs2RestSession => (Gs2RestSession) Gs2Session;

		public Gs2ShowcaseRestClient(Gs2RestSession Gs2RestSession) : base(Gs2RestSession)
		{

		}

#if UNITY_2017_1_OR_NEWER
		public Gs2ShowcaseRestClient(Gs2RestSession gs2RestSession, CertificateHandler certificateHandler) : base(gs2RestSession)
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
                    .Replace("{service}", "showcase")
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
                    .Replace("{service}", "showcase")
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
                if (request.BuyScript != null)
                {
                    jsonWriter.WritePropertyName("buyScript");
                    request.BuyScript.WriteJson(jsonWriter);
                }
                if (request.QueueNamespaceId != null)
                {
                    jsonWriter.WritePropertyName("queueNamespaceId");
                    jsonWriter.Write(request.QueueNamespaceId);
                }
                if (request.KeyId != null)
                {
                    jsonWriter.WritePropertyName("keyId");
                    jsonWriter.Write(request.KeyId);
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
                    .Replace("{service}", "showcase")
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
                    .Replace("{service}", "showcase")
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
                    .Replace("{service}", "showcase")
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
                if (request.BuyScript != null)
                {
                    jsonWriter.WritePropertyName("buyScript");
                    request.BuyScript.WriteJson(jsonWriter);
                }
                if (request.LogSetting != null)
                {
                    jsonWriter.WritePropertyName("logSetting");
                    request.LogSetting.WriteJson(jsonWriter);
                }
                if (request.QueueNamespaceId != null)
                {
                    jsonWriter.WritePropertyName("queueNamespaceId");
                    jsonWriter.Write(request.QueueNamespaceId);
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
                    .Replace("{service}", "showcase")
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
                    .Replace("{service}", "showcase")
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
                    .Replace("{service}", "showcase")
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
                    .Replace("{service}", "showcase")
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
                    .Replace("{service}", "showcase")
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
                    .Replace("{service}", "showcase")
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
                    .Replace("{service}", "showcase")
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
                    .Replace("{service}", "showcase")
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
                    .Replace("{service}", "showcase")
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


        public class DescribeSalesItemMastersTask : Gs2RestSessionTask<DescribeSalesItemMastersRequest, DescribeSalesItemMastersResult>
        {
            public DescribeSalesItemMastersTask(IGs2Session session, RestSessionRequestFactory factory, DescribeSalesItemMastersRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DescribeSalesItemMastersRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "showcase")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/master/salesItem";

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
		public IEnumerator DescribeSalesItemMasters(
                Request.DescribeSalesItemMastersRequest request,
                UnityAction<AsyncResult<Result.DescribeSalesItemMastersResult>> callback
        ) =>
            new DescribeSalesItemMastersTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.DescribeSalesItemMastersResult> DescribeSalesItemMastersFuture(
                Request.DescribeSalesItemMastersRequest request
        ) =>
            new DescribeSalesItemMastersTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.DescribeSalesItemMastersResult> DescribeSalesItemMastersAsync(
                Request.DescribeSalesItemMastersRequest request
        ) =>
            new DescribeSalesItemMastersTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.DescribeSalesItemMastersResult>();
    #else
		public DescribeSalesItemMastersTask DescribeSalesItemMastersAsync(
                Request.DescribeSalesItemMastersRequest request
        )
		{
			return new DescribeSalesItemMastersTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.DescribeSalesItemMastersResult> DescribeSalesItemMastersAsync(
                Request.DescribeSalesItemMastersRequest request
        ) =>
            new DescribeSalesItemMastersTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class CreateSalesItemMasterTask : Gs2RestSessionTask<CreateSalesItemMasterRequest, CreateSalesItemMasterResult>
        {
            public CreateSalesItemMasterTask(IGs2Session session, RestSessionRequestFactory factory, CreateSalesItemMasterRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(CreateSalesItemMasterRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "showcase")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/master/salesItem";

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
                if (request.VerifyActions != null)
                {
                    jsonWriter.WritePropertyName("verifyActions");
                    jsonWriter.WriteArrayStart();
                    foreach(var item in request.VerifyActions)
                    {
                        if (item == null) {
                            jsonWriter.Write(null);
                        } else {
                            item.WriteJson(jsonWriter);
                        }
                    }
                    jsonWriter.WriteArrayEnd();
                }
                if (request.ConsumeActions != null)
                {
                    jsonWriter.WritePropertyName("consumeActions");
                    jsonWriter.WriteArrayStart();
                    foreach(var item in request.ConsumeActions)
                    {
                        item.WriteJson(jsonWriter);
                    }
                    jsonWriter.WriteArrayEnd();
                }
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
                if (request.DryRun)
                {
                    sessionRequest.AddHeader("X-GS2-DRY-RUN", "true");
                }

                AddHeader(
                    Session.Credential,
                    sessionRequest
                );

                return sessionRequest;
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator CreateSalesItemMaster(
                Request.CreateSalesItemMasterRequest request,
                UnityAction<AsyncResult<Result.CreateSalesItemMasterResult>> callback
        ) =>
            new CreateSalesItemMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.CreateSalesItemMasterResult> CreateSalesItemMasterFuture(
                Request.CreateSalesItemMasterRequest request
        ) =>
            new CreateSalesItemMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.CreateSalesItemMasterResult> CreateSalesItemMasterAsync(
                Request.CreateSalesItemMasterRequest request
        ) =>
            new CreateSalesItemMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.CreateSalesItemMasterResult>();
    #else
		public CreateSalesItemMasterTask CreateSalesItemMasterAsync(
                Request.CreateSalesItemMasterRequest request
        )
		{
			return new CreateSalesItemMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.CreateSalesItemMasterResult> CreateSalesItemMasterAsync(
                Request.CreateSalesItemMasterRequest request
        ) =>
            new CreateSalesItemMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class GetSalesItemMasterTask : Gs2RestSessionTask<GetSalesItemMasterRequest, GetSalesItemMasterResult>
        {
            public GetSalesItemMasterTask(IGs2Session session, RestSessionRequestFactory factory, GetSalesItemMasterRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(GetSalesItemMasterRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "showcase")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/master/salesItem/{salesItemName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{salesItemName}", !string.IsNullOrEmpty(request.SalesItemName) ? request.SalesItemName.ToString() : "null");

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
		public IEnumerator GetSalesItemMaster(
                Request.GetSalesItemMasterRequest request,
                UnityAction<AsyncResult<Result.GetSalesItemMasterResult>> callback
        ) =>
            new GetSalesItemMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.GetSalesItemMasterResult> GetSalesItemMasterFuture(
                Request.GetSalesItemMasterRequest request
        ) =>
            new GetSalesItemMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.GetSalesItemMasterResult> GetSalesItemMasterAsync(
                Request.GetSalesItemMasterRequest request
        ) =>
            new GetSalesItemMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.GetSalesItemMasterResult>();
    #else
		public GetSalesItemMasterTask GetSalesItemMasterAsync(
                Request.GetSalesItemMasterRequest request
        )
		{
			return new GetSalesItemMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.GetSalesItemMasterResult> GetSalesItemMasterAsync(
                Request.GetSalesItemMasterRequest request
        ) =>
            new GetSalesItemMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class UpdateSalesItemMasterTask : Gs2RestSessionTask<UpdateSalesItemMasterRequest, UpdateSalesItemMasterResult>
        {
            public UpdateSalesItemMasterTask(IGs2Session session, RestSessionRequestFactory factory, UpdateSalesItemMasterRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(UpdateSalesItemMasterRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "showcase")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/master/salesItem/{salesItemName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{salesItemName}", !string.IsNullOrEmpty(request.SalesItemName) ? request.SalesItemName.ToString() : "null");

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
                if (request.VerifyActions != null)
                {
                    jsonWriter.WritePropertyName("verifyActions");
                    jsonWriter.WriteArrayStart();
                    foreach(var item in request.VerifyActions)
                    {
                        if (item == null) {
                            jsonWriter.Write(null);
                        } else {
                            item.WriteJson(jsonWriter);
                        }
                    }
                    jsonWriter.WriteArrayEnd();
                }
                if (request.ConsumeActions != null)
                {
                    jsonWriter.WritePropertyName("consumeActions");
                    jsonWriter.WriteArrayStart();
                    foreach(var item in request.ConsumeActions)
                    {
                        item.WriteJson(jsonWriter);
                    }
                    jsonWriter.WriteArrayEnd();
                }
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
                if (request.DryRun)
                {
                    sessionRequest.AddHeader("X-GS2-DRY-RUN", "true");
                }

                AddHeader(
                    Session.Credential,
                    sessionRequest
                );

                return sessionRequest;
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator UpdateSalesItemMaster(
                Request.UpdateSalesItemMasterRequest request,
                UnityAction<AsyncResult<Result.UpdateSalesItemMasterResult>> callback
        ) =>
            new UpdateSalesItemMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.UpdateSalesItemMasterResult> UpdateSalesItemMasterFuture(
                Request.UpdateSalesItemMasterRequest request
        ) =>
            new UpdateSalesItemMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.UpdateSalesItemMasterResult> UpdateSalesItemMasterAsync(
                Request.UpdateSalesItemMasterRequest request
        ) =>
            new UpdateSalesItemMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.UpdateSalesItemMasterResult>();
    #else
		public UpdateSalesItemMasterTask UpdateSalesItemMasterAsync(
                Request.UpdateSalesItemMasterRequest request
        )
		{
			return new UpdateSalesItemMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.UpdateSalesItemMasterResult> UpdateSalesItemMasterAsync(
                Request.UpdateSalesItemMasterRequest request
        ) =>
            new UpdateSalesItemMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class DeleteSalesItemMasterTask : Gs2RestSessionTask<DeleteSalesItemMasterRequest, DeleteSalesItemMasterResult>
        {
            public DeleteSalesItemMasterTask(IGs2Session session, RestSessionRequestFactory factory, DeleteSalesItemMasterRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DeleteSalesItemMasterRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "showcase")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/master/salesItem/{salesItemName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{salesItemName}", !string.IsNullOrEmpty(request.SalesItemName) ? request.SalesItemName.ToString() : "null");

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
		public IEnumerator DeleteSalesItemMaster(
                Request.DeleteSalesItemMasterRequest request,
                UnityAction<AsyncResult<Result.DeleteSalesItemMasterResult>> callback
        ) =>
            new DeleteSalesItemMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.DeleteSalesItemMasterResult> DeleteSalesItemMasterFuture(
                Request.DeleteSalesItemMasterRequest request
        ) =>
            new DeleteSalesItemMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.DeleteSalesItemMasterResult> DeleteSalesItemMasterAsync(
                Request.DeleteSalesItemMasterRequest request
        ) =>
            new DeleteSalesItemMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.DeleteSalesItemMasterResult>();
    #else
		public DeleteSalesItemMasterTask DeleteSalesItemMasterAsync(
                Request.DeleteSalesItemMasterRequest request
        )
		{
			return new DeleteSalesItemMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.DeleteSalesItemMasterResult> DeleteSalesItemMasterAsync(
                Request.DeleteSalesItemMasterRequest request
        ) =>
            new DeleteSalesItemMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class DescribeSalesItemGroupMastersTask : Gs2RestSessionTask<DescribeSalesItemGroupMastersRequest, DescribeSalesItemGroupMastersResult>
        {
            public DescribeSalesItemGroupMastersTask(IGs2Session session, RestSessionRequestFactory factory, DescribeSalesItemGroupMastersRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DescribeSalesItemGroupMastersRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "showcase")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/master/group";

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
		public IEnumerator DescribeSalesItemGroupMasters(
                Request.DescribeSalesItemGroupMastersRequest request,
                UnityAction<AsyncResult<Result.DescribeSalesItemGroupMastersResult>> callback
        ) =>
            new DescribeSalesItemGroupMastersTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.DescribeSalesItemGroupMastersResult> DescribeSalesItemGroupMastersFuture(
                Request.DescribeSalesItemGroupMastersRequest request
        ) =>
            new DescribeSalesItemGroupMastersTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.DescribeSalesItemGroupMastersResult> DescribeSalesItemGroupMastersAsync(
                Request.DescribeSalesItemGroupMastersRequest request
        ) =>
            new DescribeSalesItemGroupMastersTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.DescribeSalesItemGroupMastersResult>();
    #else
		public DescribeSalesItemGroupMastersTask DescribeSalesItemGroupMastersAsync(
                Request.DescribeSalesItemGroupMastersRequest request
        )
		{
			return new DescribeSalesItemGroupMastersTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.DescribeSalesItemGroupMastersResult> DescribeSalesItemGroupMastersAsync(
                Request.DescribeSalesItemGroupMastersRequest request
        ) =>
            new DescribeSalesItemGroupMastersTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class CreateSalesItemGroupMasterTask : Gs2RestSessionTask<CreateSalesItemGroupMasterRequest, CreateSalesItemGroupMasterResult>
        {
            public CreateSalesItemGroupMasterTask(IGs2Session session, RestSessionRequestFactory factory, CreateSalesItemGroupMasterRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(CreateSalesItemGroupMasterRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "showcase")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/master/group";

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
                if (request.SalesItemNames != null)
                {
                    jsonWriter.WritePropertyName("salesItemNames");
                    jsonWriter.WriteArrayStart();
                    foreach(var item in request.SalesItemNames)
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
		public IEnumerator CreateSalesItemGroupMaster(
                Request.CreateSalesItemGroupMasterRequest request,
                UnityAction<AsyncResult<Result.CreateSalesItemGroupMasterResult>> callback
        ) =>
            new CreateSalesItemGroupMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.CreateSalesItemGroupMasterResult> CreateSalesItemGroupMasterFuture(
                Request.CreateSalesItemGroupMasterRequest request
        ) =>
            new CreateSalesItemGroupMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.CreateSalesItemGroupMasterResult> CreateSalesItemGroupMasterAsync(
                Request.CreateSalesItemGroupMasterRequest request
        ) =>
            new CreateSalesItemGroupMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.CreateSalesItemGroupMasterResult>();
    #else
		public CreateSalesItemGroupMasterTask CreateSalesItemGroupMasterAsync(
                Request.CreateSalesItemGroupMasterRequest request
        )
		{
			return new CreateSalesItemGroupMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.CreateSalesItemGroupMasterResult> CreateSalesItemGroupMasterAsync(
                Request.CreateSalesItemGroupMasterRequest request
        ) =>
            new CreateSalesItemGroupMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class GetSalesItemGroupMasterTask : Gs2RestSessionTask<GetSalesItemGroupMasterRequest, GetSalesItemGroupMasterResult>
        {
            public GetSalesItemGroupMasterTask(IGs2Session session, RestSessionRequestFactory factory, GetSalesItemGroupMasterRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(GetSalesItemGroupMasterRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "showcase")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/master/group/{salesItemGroupName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{salesItemGroupName}", !string.IsNullOrEmpty(request.SalesItemGroupName) ? request.SalesItemGroupName.ToString() : "null");

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
		public IEnumerator GetSalesItemGroupMaster(
                Request.GetSalesItemGroupMasterRequest request,
                UnityAction<AsyncResult<Result.GetSalesItemGroupMasterResult>> callback
        ) =>
            new GetSalesItemGroupMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.GetSalesItemGroupMasterResult> GetSalesItemGroupMasterFuture(
                Request.GetSalesItemGroupMasterRequest request
        ) =>
            new GetSalesItemGroupMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.GetSalesItemGroupMasterResult> GetSalesItemGroupMasterAsync(
                Request.GetSalesItemGroupMasterRequest request
        ) =>
            new GetSalesItemGroupMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.GetSalesItemGroupMasterResult>();
    #else
		public GetSalesItemGroupMasterTask GetSalesItemGroupMasterAsync(
                Request.GetSalesItemGroupMasterRequest request
        )
		{
			return new GetSalesItemGroupMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.GetSalesItemGroupMasterResult> GetSalesItemGroupMasterAsync(
                Request.GetSalesItemGroupMasterRequest request
        ) =>
            new GetSalesItemGroupMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class UpdateSalesItemGroupMasterTask : Gs2RestSessionTask<UpdateSalesItemGroupMasterRequest, UpdateSalesItemGroupMasterResult>
        {
            public UpdateSalesItemGroupMasterTask(IGs2Session session, RestSessionRequestFactory factory, UpdateSalesItemGroupMasterRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(UpdateSalesItemGroupMasterRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "showcase")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/master/group/{salesItemGroupName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{salesItemGroupName}", !string.IsNullOrEmpty(request.SalesItemGroupName) ? request.SalesItemGroupName.ToString() : "null");

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
                if (request.SalesItemNames != null)
                {
                    jsonWriter.WritePropertyName("salesItemNames");
                    jsonWriter.WriteArrayStart();
                    foreach(var item in request.SalesItemNames)
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
		public IEnumerator UpdateSalesItemGroupMaster(
                Request.UpdateSalesItemGroupMasterRequest request,
                UnityAction<AsyncResult<Result.UpdateSalesItemGroupMasterResult>> callback
        ) =>
            new UpdateSalesItemGroupMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.UpdateSalesItemGroupMasterResult> UpdateSalesItemGroupMasterFuture(
                Request.UpdateSalesItemGroupMasterRequest request
        ) =>
            new UpdateSalesItemGroupMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.UpdateSalesItemGroupMasterResult> UpdateSalesItemGroupMasterAsync(
                Request.UpdateSalesItemGroupMasterRequest request
        ) =>
            new UpdateSalesItemGroupMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.UpdateSalesItemGroupMasterResult>();
    #else
		public UpdateSalesItemGroupMasterTask UpdateSalesItemGroupMasterAsync(
                Request.UpdateSalesItemGroupMasterRequest request
        )
		{
			return new UpdateSalesItemGroupMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.UpdateSalesItemGroupMasterResult> UpdateSalesItemGroupMasterAsync(
                Request.UpdateSalesItemGroupMasterRequest request
        ) =>
            new UpdateSalesItemGroupMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class DeleteSalesItemGroupMasterTask : Gs2RestSessionTask<DeleteSalesItemGroupMasterRequest, DeleteSalesItemGroupMasterResult>
        {
            public DeleteSalesItemGroupMasterTask(IGs2Session session, RestSessionRequestFactory factory, DeleteSalesItemGroupMasterRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DeleteSalesItemGroupMasterRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "showcase")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/master/group/{salesItemGroupName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{salesItemGroupName}", !string.IsNullOrEmpty(request.SalesItemGroupName) ? request.SalesItemGroupName.ToString() : "null");

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
		public IEnumerator DeleteSalesItemGroupMaster(
                Request.DeleteSalesItemGroupMasterRequest request,
                UnityAction<AsyncResult<Result.DeleteSalesItemGroupMasterResult>> callback
        ) =>
            new DeleteSalesItemGroupMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.DeleteSalesItemGroupMasterResult> DeleteSalesItemGroupMasterFuture(
                Request.DeleteSalesItemGroupMasterRequest request
        ) =>
            new DeleteSalesItemGroupMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.DeleteSalesItemGroupMasterResult> DeleteSalesItemGroupMasterAsync(
                Request.DeleteSalesItemGroupMasterRequest request
        ) =>
            new DeleteSalesItemGroupMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.DeleteSalesItemGroupMasterResult>();
    #else
		public DeleteSalesItemGroupMasterTask DeleteSalesItemGroupMasterAsync(
                Request.DeleteSalesItemGroupMasterRequest request
        )
		{
			return new DeleteSalesItemGroupMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.DeleteSalesItemGroupMasterResult> DeleteSalesItemGroupMasterAsync(
                Request.DeleteSalesItemGroupMasterRequest request
        ) =>
            new DeleteSalesItemGroupMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class DescribeShowcaseMastersTask : Gs2RestSessionTask<DescribeShowcaseMastersRequest, DescribeShowcaseMastersResult>
        {
            public DescribeShowcaseMastersTask(IGs2Session session, RestSessionRequestFactory factory, DescribeShowcaseMastersRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DescribeShowcaseMastersRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "showcase")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/master/showcase";

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
		public IEnumerator DescribeShowcaseMasters(
                Request.DescribeShowcaseMastersRequest request,
                UnityAction<AsyncResult<Result.DescribeShowcaseMastersResult>> callback
        ) =>
            new DescribeShowcaseMastersTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.DescribeShowcaseMastersResult> DescribeShowcaseMastersFuture(
                Request.DescribeShowcaseMastersRequest request
        ) =>
            new DescribeShowcaseMastersTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.DescribeShowcaseMastersResult> DescribeShowcaseMastersAsync(
                Request.DescribeShowcaseMastersRequest request
        ) =>
            new DescribeShowcaseMastersTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.DescribeShowcaseMastersResult>();
    #else
		public DescribeShowcaseMastersTask DescribeShowcaseMastersAsync(
                Request.DescribeShowcaseMastersRequest request
        )
		{
			return new DescribeShowcaseMastersTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.DescribeShowcaseMastersResult> DescribeShowcaseMastersAsync(
                Request.DescribeShowcaseMastersRequest request
        ) =>
            new DescribeShowcaseMastersTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class CreateShowcaseMasterTask : Gs2RestSessionTask<CreateShowcaseMasterRequest, CreateShowcaseMasterResult>
        {
            public CreateShowcaseMasterTask(IGs2Session session, RestSessionRequestFactory factory, CreateShowcaseMasterRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(CreateShowcaseMasterRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "showcase")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/master/showcase";

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
                if (request.DisplayItems != null)
                {
                    jsonWriter.WritePropertyName("displayItems");
                    jsonWriter.WriteArrayStart();
                    foreach(var item in request.DisplayItems)
                    {
                        item.WriteJson(jsonWriter);
                    }
                    jsonWriter.WriteArrayEnd();
                }
                if (request.SalesPeriodEventId != null)
                {
                    jsonWriter.WritePropertyName("salesPeriodEventId");
                    jsonWriter.Write(request.SalesPeriodEventId);
                }
                if (request.ContextStack != null)
                {
                    jsonWriter.WritePropertyName("contextStack");
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
		public IEnumerator CreateShowcaseMaster(
                Request.CreateShowcaseMasterRequest request,
                UnityAction<AsyncResult<Result.CreateShowcaseMasterResult>> callback
        ) =>
            new CreateShowcaseMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.CreateShowcaseMasterResult> CreateShowcaseMasterFuture(
                Request.CreateShowcaseMasterRequest request
        ) =>
            new CreateShowcaseMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.CreateShowcaseMasterResult> CreateShowcaseMasterAsync(
                Request.CreateShowcaseMasterRequest request
        ) =>
            new CreateShowcaseMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.CreateShowcaseMasterResult>();
    #else
		public CreateShowcaseMasterTask CreateShowcaseMasterAsync(
                Request.CreateShowcaseMasterRequest request
        )
		{
			return new CreateShowcaseMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.CreateShowcaseMasterResult> CreateShowcaseMasterAsync(
                Request.CreateShowcaseMasterRequest request
        ) =>
            new CreateShowcaseMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class GetShowcaseMasterTask : Gs2RestSessionTask<GetShowcaseMasterRequest, GetShowcaseMasterResult>
        {
            public GetShowcaseMasterTask(IGs2Session session, RestSessionRequestFactory factory, GetShowcaseMasterRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(GetShowcaseMasterRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "showcase")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/master/showcase/{showcaseName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{showcaseName}", !string.IsNullOrEmpty(request.ShowcaseName) ? request.ShowcaseName.ToString() : "null");

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
		public IEnumerator GetShowcaseMaster(
                Request.GetShowcaseMasterRequest request,
                UnityAction<AsyncResult<Result.GetShowcaseMasterResult>> callback
        ) =>
            new GetShowcaseMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.GetShowcaseMasterResult> GetShowcaseMasterFuture(
                Request.GetShowcaseMasterRequest request
        ) =>
            new GetShowcaseMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.GetShowcaseMasterResult> GetShowcaseMasterAsync(
                Request.GetShowcaseMasterRequest request
        ) =>
            new GetShowcaseMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.GetShowcaseMasterResult>();
    #else
		public GetShowcaseMasterTask GetShowcaseMasterAsync(
                Request.GetShowcaseMasterRequest request
        )
		{
			return new GetShowcaseMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.GetShowcaseMasterResult> GetShowcaseMasterAsync(
                Request.GetShowcaseMasterRequest request
        ) =>
            new GetShowcaseMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class UpdateShowcaseMasterTask : Gs2RestSessionTask<UpdateShowcaseMasterRequest, UpdateShowcaseMasterResult>
        {
            public UpdateShowcaseMasterTask(IGs2Session session, RestSessionRequestFactory factory, UpdateShowcaseMasterRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(UpdateShowcaseMasterRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "showcase")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/master/showcase/{showcaseName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{showcaseName}", !string.IsNullOrEmpty(request.ShowcaseName) ? request.ShowcaseName.ToString() : "null");

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
                if (request.DisplayItems != null)
                {
                    jsonWriter.WritePropertyName("displayItems");
                    jsonWriter.WriteArrayStart();
                    foreach(var item in request.DisplayItems)
                    {
                        item.WriteJson(jsonWriter);
                    }
                    jsonWriter.WriteArrayEnd();
                }
                if (request.SalesPeriodEventId != null)
                {
                    jsonWriter.WritePropertyName("salesPeriodEventId");
                    jsonWriter.Write(request.SalesPeriodEventId);
                }
                if (request.ContextStack != null)
                {
                    jsonWriter.WritePropertyName("contextStack");
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
		public IEnumerator UpdateShowcaseMaster(
                Request.UpdateShowcaseMasterRequest request,
                UnityAction<AsyncResult<Result.UpdateShowcaseMasterResult>> callback
        ) =>
            new UpdateShowcaseMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.UpdateShowcaseMasterResult> UpdateShowcaseMasterFuture(
                Request.UpdateShowcaseMasterRequest request
        ) =>
            new UpdateShowcaseMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.UpdateShowcaseMasterResult> UpdateShowcaseMasterAsync(
                Request.UpdateShowcaseMasterRequest request
        ) =>
            new UpdateShowcaseMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.UpdateShowcaseMasterResult>();
    #else
		public UpdateShowcaseMasterTask UpdateShowcaseMasterAsync(
                Request.UpdateShowcaseMasterRequest request
        )
		{
			return new UpdateShowcaseMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.UpdateShowcaseMasterResult> UpdateShowcaseMasterAsync(
                Request.UpdateShowcaseMasterRequest request
        ) =>
            new UpdateShowcaseMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class DeleteShowcaseMasterTask : Gs2RestSessionTask<DeleteShowcaseMasterRequest, DeleteShowcaseMasterResult>
        {
            public DeleteShowcaseMasterTask(IGs2Session session, RestSessionRequestFactory factory, DeleteShowcaseMasterRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DeleteShowcaseMasterRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "showcase")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/master/showcase/{showcaseName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{showcaseName}", !string.IsNullOrEmpty(request.ShowcaseName) ? request.ShowcaseName.ToString() : "null");

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
		public IEnumerator DeleteShowcaseMaster(
                Request.DeleteShowcaseMasterRequest request,
                UnityAction<AsyncResult<Result.DeleteShowcaseMasterResult>> callback
        ) =>
            new DeleteShowcaseMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.DeleteShowcaseMasterResult> DeleteShowcaseMasterFuture(
                Request.DeleteShowcaseMasterRequest request
        ) =>
            new DeleteShowcaseMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.DeleteShowcaseMasterResult> DeleteShowcaseMasterAsync(
                Request.DeleteShowcaseMasterRequest request
        ) =>
            new DeleteShowcaseMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.DeleteShowcaseMasterResult>();
    #else
		public DeleteShowcaseMasterTask DeleteShowcaseMasterAsync(
                Request.DeleteShowcaseMasterRequest request
        )
		{
			return new DeleteShowcaseMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.DeleteShowcaseMasterResult> DeleteShowcaseMasterAsync(
                Request.DeleteShowcaseMasterRequest request
        ) =>
            new DeleteShowcaseMasterTask(
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
                    .Replace("{service}", "showcase")
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


        public class GetCurrentShowcaseMasterTask : Gs2RestSessionTask<GetCurrentShowcaseMasterRequest, GetCurrentShowcaseMasterResult>
        {
            public GetCurrentShowcaseMasterTask(IGs2Session session, RestSessionRequestFactory factory, GetCurrentShowcaseMasterRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(GetCurrentShowcaseMasterRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "showcase")
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
		public IEnumerator GetCurrentShowcaseMaster(
                Request.GetCurrentShowcaseMasterRequest request,
                UnityAction<AsyncResult<Result.GetCurrentShowcaseMasterResult>> callback
        ) =>
            new GetCurrentShowcaseMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.GetCurrentShowcaseMasterResult> GetCurrentShowcaseMasterFuture(
                Request.GetCurrentShowcaseMasterRequest request
        ) =>
            new GetCurrentShowcaseMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.GetCurrentShowcaseMasterResult> GetCurrentShowcaseMasterAsync(
                Request.GetCurrentShowcaseMasterRequest request
        ) =>
            new GetCurrentShowcaseMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.GetCurrentShowcaseMasterResult>();
    #else
		public GetCurrentShowcaseMasterTask GetCurrentShowcaseMasterAsync(
                Request.GetCurrentShowcaseMasterRequest request
        )
		{
			return new GetCurrentShowcaseMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.GetCurrentShowcaseMasterResult> GetCurrentShowcaseMasterAsync(
                Request.GetCurrentShowcaseMasterRequest request
        ) =>
            new GetCurrentShowcaseMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class PreUpdateCurrentShowcaseMasterTask : Gs2RestSessionTask<PreUpdateCurrentShowcaseMasterRequest, PreUpdateCurrentShowcaseMasterResult>
        {
            public PreUpdateCurrentShowcaseMasterTask(IGs2Session session, RestSessionRequestFactory factory, PreUpdateCurrentShowcaseMasterRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(PreUpdateCurrentShowcaseMasterRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "showcase")
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
		public IEnumerator PreUpdateCurrentShowcaseMaster(
                Request.PreUpdateCurrentShowcaseMasterRequest request,
                UnityAction<AsyncResult<Result.PreUpdateCurrentShowcaseMasterResult>> callback
        ) =>
            new PreUpdateCurrentShowcaseMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.PreUpdateCurrentShowcaseMasterResult> PreUpdateCurrentShowcaseMasterFuture(
                Request.PreUpdateCurrentShowcaseMasterRequest request
        ) =>
            new PreUpdateCurrentShowcaseMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.PreUpdateCurrentShowcaseMasterResult> PreUpdateCurrentShowcaseMasterAsync(
                Request.PreUpdateCurrentShowcaseMasterRequest request
        ) =>
            new PreUpdateCurrentShowcaseMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.PreUpdateCurrentShowcaseMasterResult>();
    #else
		public PreUpdateCurrentShowcaseMasterTask PreUpdateCurrentShowcaseMasterAsync(
                Request.PreUpdateCurrentShowcaseMasterRequest request
        )
		{
			return new PreUpdateCurrentShowcaseMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.PreUpdateCurrentShowcaseMasterResult> PreUpdateCurrentShowcaseMasterAsync(
                Request.PreUpdateCurrentShowcaseMasterRequest request
        ) =>
            new PreUpdateCurrentShowcaseMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class UpdateCurrentShowcaseMasterTask : Gs2RestSessionTask<UpdateCurrentShowcaseMasterRequest, UpdateCurrentShowcaseMasterResult>
        {
            public UpdateCurrentShowcaseMasterTask(IGs2Session session, RestSessionRequestFactory factory, UpdateCurrentShowcaseMasterRequest request) : base(session, factory, request)
            {
            }
#if GS2_ENABLE_UNITASK
            protected override async UniTask<UpdateCurrentShowcaseMasterResult> InvokeImpl()
#else
            protected override async Task<UpdateCurrentShowcaseMasterResult> InvokeImpl()
#endif
            {
                if (Request.Settings != null) {
                    var preTask = new PreUpdateCurrentShowcaseMasterTask(
                        Session,
                        Factory,
                        new PreUpdateCurrentShowcaseMasterRequest()
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

            protected override IGs2SessionRequest CreateRequest(UpdateCurrentShowcaseMasterRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "showcase")
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
		public IEnumerator UpdateCurrentShowcaseMaster(
                Request.UpdateCurrentShowcaseMasterRequest request,
                UnityAction<AsyncResult<Result.UpdateCurrentShowcaseMasterResult>> callback
        ) =>
            new UpdateCurrentShowcaseMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.UpdateCurrentShowcaseMasterResult> UpdateCurrentShowcaseMasterFuture(
                Request.UpdateCurrentShowcaseMasterRequest request
        ) =>
            new UpdateCurrentShowcaseMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.UpdateCurrentShowcaseMasterResult> UpdateCurrentShowcaseMasterAsync(
                Request.UpdateCurrentShowcaseMasterRequest request
        ) =>
            new UpdateCurrentShowcaseMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.UpdateCurrentShowcaseMasterResult>();
    #else
		public UpdateCurrentShowcaseMasterTask UpdateCurrentShowcaseMasterAsync(
                Request.UpdateCurrentShowcaseMasterRequest request
        )
		{
			return new UpdateCurrentShowcaseMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.UpdateCurrentShowcaseMasterResult> UpdateCurrentShowcaseMasterAsync(
                Request.UpdateCurrentShowcaseMasterRequest request
        ) =>
            new UpdateCurrentShowcaseMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class UpdateCurrentShowcaseMasterFromGitHubTask : Gs2RestSessionTask<UpdateCurrentShowcaseMasterFromGitHubRequest, UpdateCurrentShowcaseMasterFromGitHubResult>
        {
            public UpdateCurrentShowcaseMasterFromGitHubTask(IGs2Session session, RestSessionRequestFactory factory, UpdateCurrentShowcaseMasterFromGitHubRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(UpdateCurrentShowcaseMasterFromGitHubRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "showcase")
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
		public IEnumerator UpdateCurrentShowcaseMasterFromGitHub(
                Request.UpdateCurrentShowcaseMasterFromGitHubRequest request,
                UnityAction<AsyncResult<Result.UpdateCurrentShowcaseMasterFromGitHubResult>> callback
        ) =>
            new UpdateCurrentShowcaseMasterFromGitHubTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.UpdateCurrentShowcaseMasterFromGitHubResult> UpdateCurrentShowcaseMasterFromGitHubFuture(
                Request.UpdateCurrentShowcaseMasterFromGitHubRequest request
        ) =>
            new UpdateCurrentShowcaseMasterFromGitHubTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.UpdateCurrentShowcaseMasterFromGitHubResult> UpdateCurrentShowcaseMasterFromGitHubAsync(
                Request.UpdateCurrentShowcaseMasterFromGitHubRequest request
        ) =>
            new UpdateCurrentShowcaseMasterFromGitHubTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.UpdateCurrentShowcaseMasterFromGitHubResult>();
    #else
		public UpdateCurrentShowcaseMasterFromGitHubTask UpdateCurrentShowcaseMasterFromGitHubAsync(
                Request.UpdateCurrentShowcaseMasterFromGitHubRequest request
        )
		{
			return new UpdateCurrentShowcaseMasterFromGitHubTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.UpdateCurrentShowcaseMasterFromGitHubResult> UpdateCurrentShowcaseMasterFromGitHubAsync(
                Request.UpdateCurrentShowcaseMasterFromGitHubRequest request
        ) =>
            new UpdateCurrentShowcaseMasterFromGitHubTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class DescribeShowcasesTask : Gs2RestSessionTask<DescribeShowcasesRequest, DescribeShowcasesResult>
        {
            public DescribeShowcasesTask(IGs2Session session, RestSessionRequestFactory factory, DescribeShowcasesRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DescribeShowcasesRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "showcase")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/me/showcase";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");

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
		public IEnumerator DescribeShowcases(
                Request.DescribeShowcasesRequest request,
                UnityAction<AsyncResult<Result.DescribeShowcasesResult>> callback
        ) =>
            new DescribeShowcasesTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.DescribeShowcasesResult> DescribeShowcasesFuture(
                Request.DescribeShowcasesRequest request
        ) =>
            new DescribeShowcasesTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.DescribeShowcasesResult> DescribeShowcasesAsync(
                Request.DescribeShowcasesRequest request
        ) =>
            new DescribeShowcasesTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.DescribeShowcasesResult>();
    #else
		public DescribeShowcasesTask DescribeShowcasesAsync(
                Request.DescribeShowcasesRequest request
        )
		{
			return new DescribeShowcasesTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.DescribeShowcasesResult> DescribeShowcasesAsync(
                Request.DescribeShowcasesRequest request
        ) =>
            new DescribeShowcasesTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class DescribeShowcasesByUserIdTask : Gs2RestSessionTask<DescribeShowcasesByUserIdRequest, DescribeShowcasesByUserIdResult>
        {
            public DescribeShowcasesByUserIdTask(IGs2Session session, RestSessionRequestFactory factory, DescribeShowcasesByUserIdRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DescribeShowcasesByUserIdRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "showcase")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/{userId}/showcase";

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
		public IEnumerator DescribeShowcasesByUserId(
                Request.DescribeShowcasesByUserIdRequest request,
                UnityAction<AsyncResult<Result.DescribeShowcasesByUserIdResult>> callback
        ) =>
            new DescribeShowcasesByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.DescribeShowcasesByUserIdResult> DescribeShowcasesByUserIdFuture(
                Request.DescribeShowcasesByUserIdRequest request
        ) =>
            new DescribeShowcasesByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.DescribeShowcasesByUserIdResult> DescribeShowcasesByUserIdAsync(
                Request.DescribeShowcasesByUserIdRequest request
        ) =>
            new DescribeShowcasesByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.DescribeShowcasesByUserIdResult>();
    #else
		public DescribeShowcasesByUserIdTask DescribeShowcasesByUserIdAsync(
                Request.DescribeShowcasesByUserIdRequest request
        )
		{
			return new DescribeShowcasesByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.DescribeShowcasesByUserIdResult> DescribeShowcasesByUserIdAsync(
                Request.DescribeShowcasesByUserIdRequest request
        ) =>
            new DescribeShowcasesByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class GetShowcaseTask : Gs2RestSessionTask<GetShowcaseRequest, GetShowcaseResult>
        {
            public GetShowcaseTask(IGs2Session session, RestSessionRequestFactory factory, GetShowcaseRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(GetShowcaseRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "showcase")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/me/showcase/{showcaseName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{showcaseName}", !string.IsNullOrEmpty(request.ShowcaseName) ? request.ShowcaseName.ToString() : "null");

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
		public IEnumerator GetShowcase(
                Request.GetShowcaseRequest request,
                UnityAction<AsyncResult<Result.GetShowcaseResult>> callback
        ) =>
            new GetShowcaseTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.GetShowcaseResult> GetShowcaseFuture(
                Request.GetShowcaseRequest request
        ) =>
            new GetShowcaseTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.GetShowcaseResult> GetShowcaseAsync(
                Request.GetShowcaseRequest request
        ) =>
            new GetShowcaseTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.GetShowcaseResult>();
    #else
		public GetShowcaseTask GetShowcaseAsync(
                Request.GetShowcaseRequest request
        )
		{
			return new GetShowcaseTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.GetShowcaseResult> GetShowcaseAsync(
                Request.GetShowcaseRequest request
        ) =>
            new GetShowcaseTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class GetShowcaseByUserIdTask : Gs2RestSessionTask<GetShowcaseByUserIdRequest, GetShowcaseByUserIdResult>
        {
            public GetShowcaseByUserIdTask(IGs2Session session, RestSessionRequestFactory factory, GetShowcaseByUserIdRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(GetShowcaseByUserIdRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "showcase")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/{userId}/showcase/{showcaseName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{showcaseName}", !string.IsNullOrEmpty(request.ShowcaseName) ? request.ShowcaseName.ToString() : "null");
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
		public IEnumerator GetShowcaseByUserId(
                Request.GetShowcaseByUserIdRequest request,
                UnityAction<AsyncResult<Result.GetShowcaseByUserIdResult>> callback
        ) =>
            new GetShowcaseByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.GetShowcaseByUserIdResult> GetShowcaseByUserIdFuture(
                Request.GetShowcaseByUserIdRequest request
        ) =>
            new GetShowcaseByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.GetShowcaseByUserIdResult> GetShowcaseByUserIdAsync(
                Request.GetShowcaseByUserIdRequest request
        ) =>
            new GetShowcaseByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.GetShowcaseByUserIdResult>();
    #else
		public GetShowcaseByUserIdTask GetShowcaseByUserIdAsync(
                Request.GetShowcaseByUserIdRequest request
        )
		{
			return new GetShowcaseByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.GetShowcaseByUserIdResult> GetShowcaseByUserIdAsync(
                Request.GetShowcaseByUserIdRequest request
        ) =>
            new GetShowcaseByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class BuyTask : Gs2RestSessionTask<BuyRequest, BuyResult>
        {
            public BuyTask(IGs2Session session, RestSessionRequestFactory factory, BuyRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(BuyRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "showcase")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/me/showcase/{showcaseName}/{displayItemId}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{showcaseName}", !string.IsNullOrEmpty(request.ShowcaseName) ? request.ShowcaseName.ToString() : "null");
                url = url.Replace("{displayItemId}", !string.IsNullOrEmpty(request.DisplayItemId) ? request.DisplayItemId.ToString() : "null");

                var sessionRequest = Factory.Post(url);

                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);
                jsonWriter.WriteObjectStart();
                if (request.Quantity != null)
                {
                    jsonWriter.WritePropertyName("quantity");
                    jsonWriter.Write(request.Quantity.ToString());
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
		public IEnumerator Buy(
                Request.BuyRequest request,
                UnityAction<AsyncResult<Result.BuyResult>> callback
        ) =>
            new BuyTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.BuyResult> BuyFuture(
                Request.BuyRequest request
        ) =>
            new BuyTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.BuyResult> BuyAsync(
                Request.BuyRequest request
        ) =>
            new BuyTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.BuyResult>();
    #else
		public BuyTask BuyAsync(
                Request.BuyRequest request
        )
		{
			return new BuyTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.BuyResult> BuyAsync(
                Request.BuyRequest request
        ) =>
            new BuyTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class BuyByUserIdTask : Gs2RestSessionTask<BuyByUserIdRequest, BuyByUserIdResult>
        {
            public BuyByUserIdTask(IGs2Session session, RestSessionRequestFactory factory, BuyByUserIdRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(BuyByUserIdRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "showcase")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/{userId}/showcase/{showcaseName}/{displayItemId}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{showcaseName}", !string.IsNullOrEmpty(request.ShowcaseName) ? request.ShowcaseName.ToString() : "null");
                url = url.Replace("{displayItemId}", !string.IsNullOrEmpty(request.DisplayItemId) ? request.DisplayItemId.ToString() : "null");
                url = url.Replace("{userId}", !string.IsNullOrEmpty(request.UserId) ? request.UserId.ToString() : "null");

                var sessionRequest = Factory.Post(url);

                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);
                jsonWriter.WriteObjectStart();
                if (request.Quantity != null)
                {
                    jsonWriter.WritePropertyName("quantity");
                    jsonWriter.Write(request.Quantity.ToString());
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
		public IEnumerator BuyByUserId(
                Request.BuyByUserIdRequest request,
                UnityAction<AsyncResult<Result.BuyByUserIdResult>> callback
        ) =>
            new BuyByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.BuyByUserIdResult> BuyByUserIdFuture(
                Request.BuyByUserIdRequest request
        ) =>
            new BuyByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.BuyByUserIdResult> BuyByUserIdAsync(
                Request.BuyByUserIdRequest request
        ) =>
            new BuyByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.BuyByUserIdResult>();
    #else
		public BuyByUserIdTask BuyByUserIdAsync(
                Request.BuyByUserIdRequest request
        )
		{
			return new BuyByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.BuyByUserIdResult> BuyByUserIdAsync(
                Request.BuyByUserIdRequest request
        ) =>
            new BuyByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class DescribeRandomShowcaseMastersTask : Gs2RestSessionTask<DescribeRandomShowcaseMastersRequest, DescribeRandomShowcaseMastersResult>
        {
            public DescribeRandomShowcaseMastersTask(IGs2Session session, RestSessionRequestFactory factory, DescribeRandomShowcaseMastersRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DescribeRandomShowcaseMastersRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "showcase")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/master/random/showcase";

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
		public IEnumerator DescribeRandomShowcaseMasters(
                Request.DescribeRandomShowcaseMastersRequest request,
                UnityAction<AsyncResult<Result.DescribeRandomShowcaseMastersResult>> callback
        ) =>
            new DescribeRandomShowcaseMastersTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.DescribeRandomShowcaseMastersResult> DescribeRandomShowcaseMastersFuture(
                Request.DescribeRandomShowcaseMastersRequest request
        ) =>
            new DescribeRandomShowcaseMastersTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.DescribeRandomShowcaseMastersResult> DescribeRandomShowcaseMastersAsync(
                Request.DescribeRandomShowcaseMastersRequest request
        ) =>
            new DescribeRandomShowcaseMastersTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.DescribeRandomShowcaseMastersResult>();
    #else
		public DescribeRandomShowcaseMastersTask DescribeRandomShowcaseMastersAsync(
                Request.DescribeRandomShowcaseMastersRequest request
        )
		{
			return new DescribeRandomShowcaseMastersTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.DescribeRandomShowcaseMastersResult> DescribeRandomShowcaseMastersAsync(
                Request.DescribeRandomShowcaseMastersRequest request
        ) =>
            new DescribeRandomShowcaseMastersTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class CreateRandomShowcaseMasterTask : Gs2RestSessionTask<CreateRandomShowcaseMasterRequest, CreateRandomShowcaseMasterResult>
        {
            public CreateRandomShowcaseMasterTask(IGs2Session session, RestSessionRequestFactory factory, CreateRandomShowcaseMasterRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(CreateRandomShowcaseMasterRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "showcase")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/master/random/showcase";

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
                if (request.MaximumNumberOfChoice != null)
                {
                    jsonWriter.WritePropertyName("maximumNumberOfChoice");
                    jsonWriter.Write(request.MaximumNumberOfChoice.ToString());
                }
                if (request.DisplayItems != null)
                {
                    jsonWriter.WritePropertyName("displayItems");
                    jsonWriter.WriteArrayStart();
                    foreach(var item in request.DisplayItems)
                    {
                        item.WriteJson(jsonWriter);
                    }
                    jsonWriter.WriteArrayEnd();
                }
                if (request.BaseTimestamp != null)
                {
                    jsonWriter.WritePropertyName("baseTimestamp");
                    jsonWriter.Write(request.BaseTimestamp.ToString());
                }
                if (request.ResetIntervalHours != null)
                {
                    jsonWriter.WritePropertyName("resetIntervalHours");
                    jsonWriter.Write(request.ResetIntervalHours.ToString());
                }
                if (request.SalesPeriodEventId != null)
                {
                    jsonWriter.WritePropertyName("salesPeriodEventId");
                    jsonWriter.Write(request.SalesPeriodEventId);
                }
                if (request.ContextStack != null)
                {
                    jsonWriter.WritePropertyName("contextStack");
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
		public IEnumerator CreateRandomShowcaseMaster(
                Request.CreateRandomShowcaseMasterRequest request,
                UnityAction<AsyncResult<Result.CreateRandomShowcaseMasterResult>> callback
        ) =>
            new CreateRandomShowcaseMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.CreateRandomShowcaseMasterResult> CreateRandomShowcaseMasterFuture(
                Request.CreateRandomShowcaseMasterRequest request
        ) =>
            new CreateRandomShowcaseMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.CreateRandomShowcaseMasterResult> CreateRandomShowcaseMasterAsync(
                Request.CreateRandomShowcaseMasterRequest request
        ) =>
            new CreateRandomShowcaseMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.CreateRandomShowcaseMasterResult>();
    #else
		public CreateRandomShowcaseMasterTask CreateRandomShowcaseMasterAsync(
                Request.CreateRandomShowcaseMasterRequest request
        )
		{
			return new CreateRandomShowcaseMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.CreateRandomShowcaseMasterResult> CreateRandomShowcaseMasterAsync(
                Request.CreateRandomShowcaseMasterRequest request
        ) =>
            new CreateRandomShowcaseMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class GetRandomShowcaseMasterTask : Gs2RestSessionTask<GetRandomShowcaseMasterRequest, GetRandomShowcaseMasterResult>
        {
            public GetRandomShowcaseMasterTask(IGs2Session session, RestSessionRequestFactory factory, GetRandomShowcaseMasterRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(GetRandomShowcaseMasterRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "showcase")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/master/random/showcase/{showcaseName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{showcaseName}", !string.IsNullOrEmpty(request.ShowcaseName) ? request.ShowcaseName.ToString() : "null");

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
		public IEnumerator GetRandomShowcaseMaster(
                Request.GetRandomShowcaseMasterRequest request,
                UnityAction<AsyncResult<Result.GetRandomShowcaseMasterResult>> callback
        ) =>
            new GetRandomShowcaseMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.GetRandomShowcaseMasterResult> GetRandomShowcaseMasterFuture(
                Request.GetRandomShowcaseMasterRequest request
        ) =>
            new GetRandomShowcaseMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.GetRandomShowcaseMasterResult> GetRandomShowcaseMasterAsync(
                Request.GetRandomShowcaseMasterRequest request
        ) =>
            new GetRandomShowcaseMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.GetRandomShowcaseMasterResult>();
    #else
		public GetRandomShowcaseMasterTask GetRandomShowcaseMasterAsync(
                Request.GetRandomShowcaseMasterRequest request
        )
		{
			return new GetRandomShowcaseMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.GetRandomShowcaseMasterResult> GetRandomShowcaseMasterAsync(
                Request.GetRandomShowcaseMasterRequest request
        ) =>
            new GetRandomShowcaseMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class UpdateRandomShowcaseMasterTask : Gs2RestSessionTask<UpdateRandomShowcaseMasterRequest, UpdateRandomShowcaseMasterResult>
        {
            public UpdateRandomShowcaseMasterTask(IGs2Session session, RestSessionRequestFactory factory, UpdateRandomShowcaseMasterRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(UpdateRandomShowcaseMasterRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "showcase")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/master/random/showcase/{showcaseName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{showcaseName}", !string.IsNullOrEmpty(request.ShowcaseName) ? request.ShowcaseName.ToString() : "null");

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
                if (request.MaximumNumberOfChoice != null)
                {
                    jsonWriter.WritePropertyName("maximumNumberOfChoice");
                    jsonWriter.Write(request.MaximumNumberOfChoice.ToString());
                }
                if (request.DisplayItems != null)
                {
                    jsonWriter.WritePropertyName("displayItems");
                    jsonWriter.WriteArrayStart();
                    foreach(var item in request.DisplayItems)
                    {
                        item.WriteJson(jsonWriter);
                    }
                    jsonWriter.WriteArrayEnd();
                }
                if (request.BaseTimestamp != null)
                {
                    jsonWriter.WritePropertyName("baseTimestamp");
                    jsonWriter.Write(request.BaseTimestamp.ToString());
                }
                if (request.ResetIntervalHours != null)
                {
                    jsonWriter.WritePropertyName("resetIntervalHours");
                    jsonWriter.Write(request.ResetIntervalHours.ToString());
                }
                if (request.SalesPeriodEventId != null)
                {
                    jsonWriter.WritePropertyName("salesPeriodEventId");
                    jsonWriter.Write(request.SalesPeriodEventId);
                }
                if (request.ContextStack != null)
                {
                    jsonWriter.WritePropertyName("contextStack");
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
		public IEnumerator UpdateRandomShowcaseMaster(
                Request.UpdateRandomShowcaseMasterRequest request,
                UnityAction<AsyncResult<Result.UpdateRandomShowcaseMasterResult>> callback
        ) =>
            new UpdateRandomShowcaseMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.UpdateRandomShowcaseMasterResult> UpdateRandomShowcaseMasterFuture(
                Request.UpdateRandomShowcaseMasterRequest request
        ) =>
            new UpdateRandomShowcaseMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.UpdateRandomShowcaseMasterResult> UpdateRandomShowcaseMasterAsync(
                Request.UpdateRandomShowcaseMasterRequest request
        ) =>
            new UpdateRandomShowcaseMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.UpdateRandomShowcaseMasterResult>();
    #else
		public UpdateRandomShowcaseMasterTask UpdateRandomShowcaseMasterAsync(
                Request.UpdateRandomShowcaseMasterRequest request
        )
		{
			return new UpdateRandomShowcaseMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.UpdateRandomShowcaseMasterResult> UpdateRandomShowcaseMasterAsync(
                Request.UpdateRandomShowcaseMasterRequest request
        ) =>
            new UpdateRandomShowcaseMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class DeleteRandomShowcaseMasterTask : Gs2RestSessionTask<DeleteRandomShowcaseMasterRequest, DeleteRandomShowcaseMasterResult>
        {
            public DeleteRandomShowcaseMasterTask(IGs2Session session, RestSessionRequestFactory factory, DeleteRandomShowcaseMasterRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DeleteRandomShowcaseMasterRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "showcase")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/master/random/showcase/{showcaseName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{showcaseName}", !string.IsNullOrEmpty(request.ShowcaseName) ? request.ShowcaseName.ToString() : "null");

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
		public IEnumerator DeleteRandomShowcaseMaster(
                Request.DeleteRandomShowcaseMasterRequest request,
                UnityAction<AsyncResult<Result.DeleteRandomShowcaseMasterResult>> callback
        ) =>
            new DeleteRandomShowcaseMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.DeleteRandomShowcaseMasterResult> DeleteRandomShowcaseMasterFuture(
                Request.DeleteRandomShowcaseMasterRequest request
        ) =>
            new DeleteRandomShowcaseMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.DeleteRandomShowcaseMasterResult> DeleteRandomShowcaseMasterAsync(
                Request.DeleteRandomShowcaseMasterRequest request
        ) =>
            new DeleteRandomShowcaseMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.DeleteRandomShowcaseMasterResult>();
    #else
		public DeleteRandomShowcaseMasterTask DeleteRandomShowcaseMasterAsync(
                Request.DeleteRandomShowcaseMasterRequest request
        )
		{
			return new DeleteRandomShowcaseMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.DeleteRandomShowcaseMasterResult> DeleteRandomShowcaseMasterAsync(
                Request.DeleteRandomShowcaseMasterRequest request
        ) =>
            new DeleteRandomShowcaseMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class IncrementPurchaseCountTask : Gs2RestSessionTask<IncrementPurchaseCountRequest, IncrementPurchaseCountResult>
        {
            public IncrementPurchaseCountTask(IGs2Session session, RestSessionRequestFactory factory, IncrementPurchaseCountRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(IncrementPurchaseCountRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "showcase")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/random/showcase/user/me/status/{showcaseName}/{displayItemName}/purchase/count";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{showcaseName}", !string.IsNullOrEmpty(request.ShowcaseName) ? request.ShowcaseName.ToString() : "null");
                url = url.Replace("{displayItemName}", !string.IsNullOrEmpty(request.DisplayItemName) ? request.DisplayItemName.ToString() : "null");

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
		public IEnumerator IncrementPurchaseCount(
                Request.IncrementPurchaseCountRequest request,
                UnityAction<AsyncResult<Result.IncrementPurchaseCountResult>> callback
        ) =>
            new IncrementPurchaseCountTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.IncrementPurchaseCountResult> IncrementPurchaseCountFuture(
                Request.IncrementPurchaseCountRequest request
        ) =>
            new IncrementPurchaseCountTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.IncrementPurchaseCountResult> IncrementPurchaseCountAsync(
                Request.IncrementPurchaseCountRequest request
        ) =>
            new IncrementPurchaseCountTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.IncrementPurchaseCountResult>();
    #else
		public IncrementPurchaseCountTask IncrementPurchaseCountAsync(
                Request.IncrementPurchaseCountRequest request
        )
		{
			return new IncrementPurchaseCountTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.IncrementPurchaseCountResult> IncrementPurchaseCountAsync(
                Request.IncrementPurchaseCountRequest request
        ) =>
            new IncrementPurchaseCountTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class IncrementPurchaseCountByUserIdTask : Gs2RestSessionTask<IncrementPurchaseCountByUserIdRequest, IncrementPurchaseCountByUserIdResult>
        {
            public IncrementPurchaseCountByUserIdTask(IGs2Session session, RestSessionRequestFactory factory, IncrementPurchaseCountByUserIdRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(IncrementPurchaseCountByUserIdRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "showcase")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/random/showcase/user/{userId}/status/{showcaseName}/{displayItemName}/purchase/count";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{showcaseName}", !string.IsNullOrEmpty(request.ShowcaseName) ? request.ShowcaseName.ToString() : "null");
                url = url.Replace("{displayItemName}", !string.IsNullOrEmpty(request.DisplayItemName) ? request.DisplayItemName.ToString() : "null");
                url = url.Replace("{userId}", !string.IsNullOrEmpty(request.UserId) ? request.UserId.ToString() : "null");

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
		public IEnumerator IncrementPurchaseCountByUserId(
                Request.IncrementPurchaseCountByUserIdRequest request,
                UnityAction<AsyncResult<Result.IncrementPurchaseCountByUserIdResult>> callback
        ) =>
            new IncrementPurchaseCountByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.IncrementPurchaseCountByUserIdResult> IncrementPurchaseCountByUserIdFuture(
                Request.IncrementPurchaseCountByUserIdRequest request
        ) =>
            new IncrementPurchaseCountByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.IncrementPurchaseCountByUserIdResult> IncrementPurchaseCountByUserIdAsync(
                Request.IncrementPurchaseCountByUserIdRequest request
        ) =>
            new IncrementPurchaseCountByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.IncrementPurchaseCountByUserIdResult>();
    #else
		public IncrementPurchaseCountByUserIdTask IncrementPurchaseCountByUserIdAsync(
                Request.IncrementPurchaseCountByUserIdRequest request
        )
		{
			return new IncrementPurchaseCountByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.IncrementPurchaseCountByUserIdResult> IncrementPurchaseCountByUserIdAsync(
                Request.IncrementPurchaseCountByUserIdRequest request
        ) =>
            new IncrementPurchaseCountByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class DecrementPurchaseCountByUserIdTask : Gs2RestSessionTask<DecrementPurchaseCountByUserIdRequest, DecrementPurchaseCountByUserIdResult>
        {
            public DecrementPurchaseCountByUserIdTask(IGs2Session session, RestSessionRequestFactory factory, DecrementPurchaseCountByUserIdRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DecrementPurchaseCountByUserIdRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "showcase")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/random/showcase/user/{userId}/status/{showcaseName}/{displayItemName}/purchase/count/decrease";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{showcaseName}", !string.IsNullOrEmpty(request.ShowcaseName) ? request.ShowcaseName.ToString() : "null");
                url = url.Replace("{displayItemName}", !string.IsNullOrEmpty(request.DisplayItemName) ? request.DisplayItemName.ToString() : "null");
                url = url.Replace("{userId}", !string.IsNullOrEmpty(request.UserId) ? request.UserId.ToString() : "null");

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
		public IEnumerator DecrementPurchaseCountByUserId(
                Request.DecrementPurchaseCountByUserIdRequest request,
                UnityAction<AsyncResult<Result.DecrementPurchaseCountByUserIdResult>> callback
        ) =>
            new DecrementPurchaseCountByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.DecrementPurchaseCountByUserIdResult> DecrementPurchaseCountByUserIdFuture(
                Request.DecrementPurchaseCountByUserIdRequest request
        ) =>
            new DecrementPurchaseCountByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.DecrementPurchaseCountByUserIdResult> DecrementPurchaseCountByUserIdAsync(
                Request.DecrementPurchaseCountByUserIdRequest request
        ) =>
            new DecrementPurchaseCountByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.DecrementPurchaseCountByUserIdResult>();
    #else
		public DecrementPurchaseCountByUserIdTask DecrementPurchaseCountByUserIdAsync(
                Request.DecrementPurchaseCountByUserIdRequest request
        )
		{
			return new DecrementPurchaseCountByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.DecrementPurchaseCountByUserIdResult> DecrementPurchaseCountByUserIdAsync(
                Request.DecrementPurchaseCountByUserIdRequest request
        ) =>
            new DecrementPurchaseCountByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class IncrementPurchaseCountByStampTaskTask : Gs2RestSessionTask<IncrementPurchaseCountByStampTaskRequest, IncrementPurchaseCountByStampTaskResult>
        {
            public IncrementPurchaseCountByStampTaskTask(IGs2Session session, RestSessionRequestFactory factory, IncrementPurchaseCountByStampTaskRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(IncrementPurchaseCountByStampTaskRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "showcase")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/stamp/random/showcase/status/purchase/count";

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
		public IEnumerator IncrementPurchaseCountByStampTask(
                Request.IncrementPurchaseCountByStampTaskRequest request,
                UnityAction<AsyncResult<Result.IncrementPurchaseCountByStampTaskResult>> callback
        ) =>
            new IncrementPurchaseCountByStampTaskTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.IncrementPurchaseCountByStampTaskResult> IncrementPurchaseCountByStampTaskFuture(
                Request.IncrementPurchaseCountByStampTaskRequest request
        ) =>
            new IncrementPurchaseCountByStampTaskTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.IncrementPurchaseCountByStampTaskResult> IncrementPurchaseCountByStampTaskAsync(
                Request.IncrementPurchaseCountByStampTaskRequest request
        ) =>
            new IncrementPurchaseCountByStampTaskTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.IncrementPurchaseCountByStampTaskResult>();
    #else
		public IncrementPurchaseCountByStampTaskTask IncrementPurchaseCountByStampTaskAsync(
                Request.IncrementPurchaseCountByStampTaskRequest request
        )
		{
			return new IncrementPurchaseCountByStampTaskTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.IncrementPurchaseCountByStampTaskResult> IncrementPurchaseCountByStampTaskAsync(
                Request.IncrementPurchaseCountByStampTaskRequest request
        ) =>
            new IncrementPurchaseCountByStampTaskTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class DecrementPurchaseCountByStampSheetTask : Gs2RestSessionTask<DecrementPurchaseCountByStampSheetRequest, DecrementPurchaseCountByStampSheetResult>
        {
            public DecrementPurchaseCountByStampSheetTask(IGs2Session session, RestSessionRequestFactory factory, DecrementPurchaseCountByStampSheetRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DecrementPurchaseCountByStampSheetRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "showcase")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/stamp/random/showcase/status/purchase/count/decrease";

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
		public IEnumerator DecrementPurchaseCountByStampSheet(
                Request.DecrementPurchaseCountByStampSheetRequest request,
                UnityAction<AsyncResult<Result.DecrementPurchaseCountByStampSheetResult>> callback
        ) =>
            new DecrementPurchaseCountByStampSheetTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.DecrementPurchaseCountByStampSheetResult> DecrementPurchaseCountByStampSheetFuture(
                Request.DecrementPurchaseCountByStampSheetRequest request
        ) =>
            new DecrementPurchaseCountByStampSheetTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.DecrementPurchaseCountByStampSheetResult> DecrementPurchaseCountByStampSheetAsync(
                Request.DecrementPurchaseCountByStampSheetRequest request
        ) =>
            new DecrementPurchaseCountByStampSheetTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.DecrementPurchaseCountByStampSheetResult>();
    #else
		public DecrementPurchaseCountByStampSheetTask DecrementPurchaseCountByStampSheetAsync(
                Request.DecrementPurchaseCountByStampSheetRequest request
        )
		{
			return new DecrementPurchaseCountByStampSheetTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.DecrementPurchaseCountByStampSheetResult> DecrementPurchaseCountByStampSheetAsync(
                Request.DecrementPurchaseCountByStampSheetRequest request
        ) =>
            new DecrementPurchaseCountByStampSheetTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class ForceReDrawByUserIdTask : Gs2RestSessionTask<ForceReDrawByUserIdRequest, ForceReDrawByUserIdResult>
        {
            public ForceReDrawByUserIdTask(IGs2Session session, RestSessionRequestFactory factory, ForceReDrawByUserIdRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(ForceReDrawByUserIdRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "showcase")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/random/showcase/{showcaseName}/user/{userId}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{showcaseName}", !string.IsNullOrEmpty(request.ShowcaseName) ? request.ShowcaseName.ToString() : "null");
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
		public IEnumerator ForceReDrawByUserId(
                Request.ForceReDrawByUserIdRequest request,
                UnityAction<AsyncResult<Result.ForceReDrawByUserIdResult>> callback
        ) =>
            new ForceReDrawByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.ForceReDrawByUserIdResult> ForceReDrawByUserIdFuture(
                Request.ForceReDrawByUserIdRequest request
        ) =>
            new ForceReDrawByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.ForceReDrawByUserIdResult> ForceReDrawByUserIdAsync(
                Request.ForceReDrawByUserIdRequest request
        ) =>
            new ForceReDrawByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.ForceReDrawByUserIdResult>();
    #else
		public ForceReDrawByUserIdTask ForceReDrawByUserIdAsync(
                Request.ForceReDrawByUserIdRequest request
        )
		{
			return new ForceReDrawByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.ForceReDrawByUserIdResult> ForceReDrawByUserIdAsync(
                Request.ForceReDrawByUserIdRequest request
        ) =>
            new ForceReDrawByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class ForceReDrawByUserIdByStampSheetTask : Gs2RestSessionTask<ForceReDrawByUserIdByStampSheetRequest, ForceReDrawByUserIdByStampSheetResult>
        {
            public ForceReDrawByUserIdByStampSheetTask(IGs2Session session, RestSessionRequestFactory factory, ForceReDrawByUserIdByStampSheetRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(ForceReDrawByUserIdByStampSheetRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "showcase")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/stamp/random/showcase/status/redraw";

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
		public IEnumerator ForceReDrawByUserIdByStampSheet(
                Request.ForceReDrawByUserIdByStampSheetRequest request,
                UnityAction<AsyncResult<Result.ForceReDrawByUserIdByStampSheetResult>> callback
        ) =>
            new ForceReDrawByUserIdByStampSheetTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.ForceReDrawByUserIdByStampSheetResult> ForceReDrawByUserIdByStampSheetFuture(
                Request.ForceReDrawByUserIdByStampSheetRequest request
        ) =>
            new ForceReDrawByUserIdByStampSheetTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.ForceReDrawByUserIdByStampSheetResult> ForceReDrawByUserIdByStampSheetAsync(
                Request.ForceReDrawByUserIdByStampSheetRequest request
        ) =>
            new ForceReDrawByUserIdByStampSheetTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.ForceReDrawByUserIdByStampSheetResult>();
    #else
		public ForceReDrawByUserIdByStampSheetTask ForceReDrawByUserIdByStampSheetAsync(
                Request.ForceReDrawByUserIdByStampSheetRequest request
        )
		{
			return new ForceReDrawByUserIdByStampSheetTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.ForceReDrawByUserIdByStampSheetResult> ForceReDrawByUserIdByStampSheetAsync(
                Request.ForceReDrawByUserIdByStampSheetRequest request
        ) =>
            new ForceReDrawByUserIdByStampSheetTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class DescribeRandomDisplayItemsTask : Gs2RestSessionTask<DescribeRandomDisplayItemsRequest, DescribeRandomDisplayItemsResult>
        {
            public DescribeRandomDisplayItemsTask(IGs2Session session, RestSessionRequestFactory factory, DescribeRandomDisplayItemsRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DescribeRandomDisplayItemsRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "showcase")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/me/random/showcase/{showcaseName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{showcaseName}", !string.IsNullOrEmpty(request.ShowcaseName) ? request.ShowcaseName.ToString() : "null");

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
		public IEnumerator DescribeRandomDisplayItems(
                Request.DescribeRandomDisplayItemsRequest request,
                UnityAction<AsyncResult<Result.DescribeRandomDisplayItemsResult>> callback
        ) =>
            new DescribeRandomDisplayItemsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.DescribeRandomDisplayItemsResult> DescribeRandomDisplayItemsFuture(
                Request.DescribeRandomDisplayItemsRequest request
        ) =>
            new DescribeRandomDisplayItemsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.DescribeRandomDisplayItemsResult> DescribeRandomDisplayItemsAsync(
                Request.DescribeRandomDisplayItemsRequest request
        ) =>
            new DescribeRandomDisplayItemsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.DescribeRandomDisplayItemsResult>();
    #else
		public DescribeRandomDisplayItemsTask DescribeRandomDisplayItemsAsync(
                Request.DescribeRandomDisplayItemsRequest request
        )
		{
			return new DescribeRandomDisplayItemsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.DescribeRandomDisplayItemsResult> DescribeRandomDisplayItemsAsync(
                Request.DescribeRandomDisplayItemsRequest request
        ) =>
            new DescribeRandomDisplayItemsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class DescribeRandomDisplayItemsByUserIdTask : Gs2RestSessionTask<DescribeRandomDisplayItemsByUserIdRequest, DescribeRandomDisplayItemsByUserIdResult>
        {
            public DescribeRandomDisplayItemsByUserIdTask(IGs2Session session, RestSessionRequestFactory factory, DescribeRandomDisplayItemsByUserIdRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DescribeRandomDisplayItemsByUserIdRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "showcase")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/{userId}/random/showcase/{showcaseName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{showcaseName}", !string.IsNullOrEmpty(request.ShowcaseName) ? request.ShowcaseName.ToString() : "null");
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
		public IEnumerator DescribeRandomDisplayItemsByUserId(
                Request.DescribeRandomDisplayItemsByUserIdRequest request,
                UnityAction<AsyncResult<Result.DescribeRandomDisplayItemsByUserIdResult>> callback
        ) =>
            new DescribeRandomDisplayItemsByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.DescribeRandomDisplayItemsByUserIdResult> DescribeRandomDisplayItemsByUserIdFuture(
                Request.DescribeRandomDisplayItemsByUserIdRequest request
        ) =>
            new DescribeRandomDisplayItemsByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.DescribeRandomDisplayItemsByUserIdResult> DescribeRandomDisplayItemsByUserIdAsync(
                Request.DescribeRandomDisplayItemsByUserIdRequest request
        ) =>
            new DescribeRandomDisplayItemsByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.DescribeRandomDisplayItemsByUserIdResult>();
    #else
		public DescribeRandomDisplayItemsByUserIdTask DescribeRandomDisplayItemsByUserIdAsync(
                Request.DescribeRandomDisplayItemsByUserIdRequest request
        )
		{
			return new DescribeRandomDisplayItemsByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.DescribeRandomDisplayItemsByUserIdResult> DescribeRandomDisplayItemsByUserIdAsync(
                Request.DescribeRandomDisplayItemsByUserIdRequest request
        ) =>
            new DescribeRandomDisplayItemsByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class GetRandomDisplayItemTask : Gs2RestSessionTask<GetRandomDisplayItemRequest, GetRandomDisplayItemResult>
        {
            public GetRandomDisplayItemTask(IGs2Session session, RestSessionRequestFactory factory, GetRandomDisplayItemRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(GetRandomDisplayItemRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "showcase")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/me/random/showcase/{showcaseName}/displayItem/{displayItemName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{showcaseName}", !string.IsNullOrEmpty(request.ShowcaseName) ? request.ShowcaseName.ToString() : "null");
                url = url.Replace("{displayItemName}", !string.IsNullOrEmpty(request.DisplayItemName) ? request.DisplayItemName.ToString() : "null");

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
		public IEnumerator GetRandomDisplayItem(
                Request.GetRandomDisplayItemRequest request,
                UnityAction<AsyncResult<Result.GetRandomDisplayItemResult>> callback
        ) =>
            new GetRandomDisplayItemTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.GetRandomDisplayItemResult> GetRandomDisplayItemFuture(
                Request.GetRandomDisplayItemRequest request
        ) =>
            new GetRandomDisplayItemTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.GetRandomDisplayItemResult> GetRandomDisplayItemAsync(
                Request.GetRandomDisplayItemRequest request
        ) =>
            new GetRandomDisplayItemTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.GetRandomDisplayItemResult>();
    #else
		public GetRandomDisplayItemTask GetRandomDisplayItemAsync(
                Request.GetRandomDisplayItemRequest request
        )
		{
			return new GetRandomDisplayItemTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.GetRandomDisplayItemResult> GetRandomDisplayItemAsync(
                Request.GetRandomDisplayItemRequest request
        ) =>
            new GetRandomDisplayItemTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class GetRandomDisplayItemByUserIdTask : Gs2RestSessionTask<GetRandomDisplayItemByUserIdRequest, GetRandomDisplayItemByUserIdResult>
        {
            public GetRandomDisplayItemByUserIdTask(IGs2Session session, RestSessionRequestFactory factory, GetRandomDisplayItemByUserIdRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(GetRandomDisplayItemByUserIdRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "showcase")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/{userId}/random/showcase/{showcaseName}/displayItem/{displayItemName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{showcaseName}", !string.IsNullOrEmpty(request.ShowcaseName) ? request.ShowcaseName.ToString() : "null");
                url = url.Replace("{displayItemName}", !string.IsNullOrEmpty(request.DisplayItemName) ? request.DisplayItemName.ToString() : "null");
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
		public IEnumerator GetRandomDisplayItemByUserId(
                Request.GetRandomDisplayItemByUserIdRequest request,
                UnityAction<AsyncResult<Result.GetRandomDisplayItemByUserIdResult>> callback
        ) =>
            new GetRandomDisplayItemByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.GetRandomDisplayItemByUserIdResult> GetRandomDisplayItemByUserIdFuture(
                Request.GetRandomDisplayItemByUserIdRequest request
        ) =>
            new GetRandomDisplayItemByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.GetRandomDisplayItemByUserIdResult> GetRandomDisplayItemByUserIdAsync(
                Request.GetRandomDisplayItemByUserIdRequest request
        ) =>
            new GetRandomDisplayItemByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.GetRandomDisplayItemByUserIdResult>();
    #else
		public GetRandomDisplayItemByUserIdTask GetRandomDisplayItemByUserIdAsync(
                Request.GetRandomDisplayItemByUserIdRequest request
        )
		{
			return new GetRandomDisplayItemByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.GetRandomDisplayItemByUserIdResult> GetRandomDisplayItemByUserIdAsync(
                Request.GetRandomDisplayItemByUserIdRequest request
        ) =>
            new GetRandomDisplayItemByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class RandomShowcaseBuyTask : Gs2RestSessionTask<RandomShowcaseBuyRequest, RandomShowcaseBuyResult>
        {
            public RandomShowcaseBuyTask(IGs2Session session, RestSessionRequestFactory factory, RandomShowcaseBuyRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(RandomShowcaseBuyRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "showcase")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/me/random/showcase/{showcaseName}/{displayItemName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{showcaseName}", !string.IsNullOrEmpty(request.ShowcaseName) ? request.ShowcaseName.ToString() : "null");
                url = url.Replace("{displayItemName}", !string.IsNullOrEmpty(request.DisplayItemName) ? request.DisplayItemName.ToString() : "null");

                var sessionRequest = Factory.Post(url);

                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);
                jsonWriter.WriteObjectStart();
                if (request.Quantity != null)
                {
                    jsonWriter.WritePropertyName("quantity");
                    jsonWriter.Write(request.Quantity.ToString());
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
		public IEnumerator RandomShowcaseBuy(
                Request.RandomShowcaseBuyRequest request,
                UnityAction<AsyncResult<Result.RandomShowcaseBuyResult>> callback
        ) =>
            new RandomShowcaseBuyTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.RandomShowcaseBuyResult> RandomShowcaseBuyFuture(
                Request.RandomShowcaseBuyRequest request
        ) =>
            new RandomShowcaseBuyTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.RandomShowcaseBuyResult> RandomShowcaseBuyAsync(
                Request.RandomShowcaseBuyRequest request
        ) =>
            new RandomShowcaseBuyTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.RandomShowcaseBuyResult>();
    #else
		public RandomShowcaseBuyTask RandomShowcaseBuyAsync(
                Request.RandomShowcaseBuyRequest request
        )
		{
			return new RandomShowcaseBuyTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.RandomShowcaseBuyResult> RandomShowcaseBuyAsync(
                Request.RandomShowcaseBuyRequest request
        ) =>
            new RandomShowcaseBuyTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class RandomShowcaseBuyByUserIdTask : Gs2RestSessionTask<RandomShowcaseBuyByUserIdRequest, RandomShowcaseBuyByUserIdResult>
        {
            public RandomShowcaseBuyByUserIdTask(IGs2Session session, RestSessionRequestFactory factory, RandomShowcaseBuyByUserIdRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(RandomShowcaseBuyByUserIdRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "showcase")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/{userId}/random/showcase/{showcaseName}/{displayItemName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{showcaseName}", !string.IsNullOrEmpty(request.ShowcaseName) ? request.ShowcaseName.ToString() : "null");
                url = url.Replace("{displayItemName}", !string.IsNullOrEmpty(request.DisplayItemName) ? request.DisplayItemName.ToString() : "null");
                url = url.Replace("{userId}", !string.IsNullOrEmpty(request.UserId) ? request.UserId.ToString() : "null");

                var sessionRequest = Factory.Post(url);

                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);
                jsonWriter.WriteObjectStart();
                if (request.Quantity != null)
                {
                    jsonWriter.WritePropertyName("quantity");
                    jsonWriter.Write(request.Quantity.ToString());
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
		public IEnumerator RandomShowcaseBuyByUserId(
                Request.RandomShowcaseBuyByUserIdRequest request,
                UnityAction<AsyncResult<Result.RandomShowcaseBuyByUserIdResult>> callback
        ) =>
            new RandomShowcaseBuyByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.RandomShowcaseBuyByUserIdResult> RandomShowcaseBuyByUserIdFuture(
                Request.RandomShowcaseBuyByUserIdRequest request
        ) =>
            new RandomShowcaseBuyByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.RandomShowcaseBuyByUserIdResult> RandomShowcaseBuyByUserIdAsync(
                Request.RandomShowcaseBuyByUserIdRequest request
        ) =>
            new RandomShowcaseBuyByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.RandomShowcaseBuyByUserIdResult>();
    #else
		public RandomShowcaseBuyByUserIdTask RandomShowcaseBuyByUserIdAsync(
                Request.RandomShowcaseBuyByUserIdRequest request
        )
		{
			return new RandomShowcaseBuyByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.RandomShowcaseBuyByUserIdResult> RandomShowcaseBuyByUserIdAsync(
                Request.RandomShowcaseBuyByUserIdRequest request
        ) =>
            new RandomShowcaseBuyByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif
	}
}