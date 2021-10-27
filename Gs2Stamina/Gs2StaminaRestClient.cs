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


        private class DescribeNamespacesTask : Gs2RestSessionTask<DescribeNamespacesRequest, DescribeNamespacesResult>
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
                if (request.PageToken != null) {
                    sessionRequest.AddQueryString("pageToken", $"{request.PageToken}");
                }
                if (request.Limit != null) {
                    sessionRequest.AddQueryString("limit", $"{request.Limit}");
                }

                if (request.RequestId != null)
                {
                    sessionRequest.AddHeader("X-GS2-REQUEST-ID", request.RequestId);
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
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.DescribeNamespacesResult>(task.Result, task.Error));
        }
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


        private class CreateNamespaceTask : Gs2RestSessionTask<CreateNamespaceRequest, CreateNamespaceResult>
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
                if (request.OverflowTriggerScript != null)
                {
                    jsonWriter.WritePropertyName("overflowTriggerScript");
                    request.OverflowTriggerScript.WriteJson(jsonWriter);
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

                if (request.RequestId != null)
                {
                    sessionRequest.AddHeader("X-GS2-REQUEST-ID", request.RequestId);
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
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.CreateNamespaceResult>(task.Result, task.Error));
        }
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


        private class GetNamespaceStatusTask : Gs2RestSessionTask<GetNamespaceStatusRequest, GetNamespaceStatusResult>
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

                if (request.RequestId != null)
                {
                    sessionRequest.AddHeader("X-GS2-REQUEST-ID", request.RequestId);
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
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.GetNamespaceStatusResult>(task.Result, task.Error));
        }
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


        private class GetNamespaceTask : Gs2RestSessionTask<GetNamespaceRequest, GetNamespaceResult>
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

                if (request.RequestId != null)
                {
                    sessionRequest.AddHeader("X-GS2-REQUEST-ID", request.RequestId);
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
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.GetNamespaceResult>(task.Result, task.Error));
        }
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


        private class UpdateNamespaceTask : Gs2RestSessionTask<UpdateNamespaceRequest, UpdateNamespaceResult>
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
                if (request.OverflowTriggerScript != null)
                {
                    jsonWriter.WritePropertyName("overflowTriggerScript");
                    request.OverflowTriggerScript.WriteJson(jsonWriter);
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

                if (request.RequestId != null)
                {
                    sessionRequest.AddHeader("X-GS2-REQUEST-ID", request.RequestId);
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
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.UpdateNamespaceResult>(task.Result, task.Error));
        }
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


        private class DeleteNamespaceTask : Gs2RestSessionTask<DeleteNamespaceRequest, DeleteNamespaceResult>
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

                if (request.RequestId != null)
                {
                    sessionRequest.AddHeader("X-GS2-REQUEST-ID", request.RequestId);
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
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.DeleteNamespaceResult>(task.Result, task.Error));
        }
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


        private class DescribeStaminaModelMastersTask : Gs2RestSessionTask<DescribeStaminaModelMastersRequest, DescribeStaminaModelMastersResult>
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
                if (request.PageToken != null) {
                    sessionRequest.AddQueryString("pageToken", $"{request.PageToken}");
                }
                if (request.Limit != null) {
                    sessionRequest.AddQueryString("limit", $"{request.Limit}");
                }

                if (request.RequestId != null)
                {
                    sessionRequest.AddHeader("X-GS2-REQUEST-ID", request.RequestId);
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
        )
		{
			var task = new DescribeStaminaModelMastersTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.DescribeStaminaModelMastersResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.DescribeStaminaModelMastersResult> DescribeStaminaModelMastersAsync(
                Request.DescribeStaminaModelMastersRequest request
        )
		{
			var task = new DescribeStaminaModelMastersTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        private class CreateStaminaModelMasterTask : Gs2RestSessionTask<CreateStaminaModelMasterRequest, CreateStaminaModelMasterResult>
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

                if (request.RequestId != null)
                {
                    sessionRequest.AddHeader("X-GS2-REQUEST-ID", request.RequestId);
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
        )
		{
			var task = new CreateStaminaModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.CreateStaminaModelMasterResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.CreateStaminaModelMasterResult> CreateStaminaModelMasterAsync(
                Request.CreateStaminaModelMasterRequest request
        )
		{
			var task = new CreateStaminaModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        private class GetStaminaModelMasterTask : Gs2RestSessionTask<GetStaminaModelMasterRequest, GetStaminaModelMasterResult>
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

                if (request.RequestId != null)
                {
                    sessionRequest.AddHeader("X-GS2-REQUEST-ID", request.RequestId);
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
        )
		{
			var task = new GetStaminaModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.GetStaminaModelMasterResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.GetStaminaModelMasterResult> GetStaminaModelMasterAsync(
                Request.GetStaminaModelMasterRequest request
        )
		{
			var task = new GetStaminaModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        private class UpdateStaminaModelMasterTask : Gs2RestSessionTask<UpdateStaminaModelMasterRequest, UpdateStaminaModelMasterResult>
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

                if (request.RequestId != null)
                {
                    sessionRequest.AddHeader("X-GS2-REQUEST-ID", request.RequestId);
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
        )
		{
			var task = new UpdateStaminaModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.UpdateStaminaModelMasterResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.UpdateStaminaModelMasterResult> UpdateStaminaModelMasterAsync(
                Request.UpdateStaminaModelMasterRequest request
        )
		{
			var task = new UpdateStaminaModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        private class DeleteStaminaModelMasterTask : Gs2RestSessionTask<DeleteStaminaModelMasterRequest, DeleteStaminaModelMasterResult>
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

                if (request.RequestId != null)
                {
                    sessionRequest.AddHeader("X-GS2-REQUEST-ID", request.RequestId);
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
        )
		{
			var task = new DeleteStaminaModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.DeleteStaminaModelMasterResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.DeleteStaminaModelMasterResult> DeleteStaminaModelMasterAsync(
                Request.DeleteStaminaModelMasterRequest request
        )
		{
			var task = new DeleteStaminaModelMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        private class DescribeMaxStaminaTableMastersTask : Gs2RestSessionTask<DescribeMaxStaminaTableMastersRequest, DescribeMaxStaminaTableMastersResult>
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

                if (request.RequestId != null)
                {
                    sessionRequest.AddHeader("X-GS2-REQUEST-ID", request.RequestId);
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
        )
		{
			var task = new DescribeMaxStaminaTableMastersTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.DescribeMaxStaminaTableMastersResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.DescribeMaxStaminaTableMastersResult> DescribeMaxStaminaTableMastersAsync(
                Request.DescribeMaxStaminaTableMastersRequest request
        )
		{
			var task = new DescribeMaxStaminaTableMastersTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        private class CreateMaxStaminaTableMasterTask : Gs2RestSessionTask<CreateMaxStaminaTableMasterRequest, CreateMaxStaminaTableMasterResult>
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

                if (request.RequestId != null)
                {
                    sessionRequest.AddHeader("X-GS2-REQUEST-ID", request.RequestId);
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
        )
		{
			var task = new CreateMaxStaminaTableMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.CreateMaxStaminaTableMasterResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.CreateMaxStaminaTableMasterResult> CreateMaxStaminaTableMasterAsync(
                Request.CreateMaxStaminaTableMasterRequest request
        )
		{
			var task = new CreateMaxStaminaTableMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        private class GetMaxStaminaTableMasterTask : Gs2RestSessionTask<GetMaxStaminaTableMasterRequest, GetMaxStaminaTableMasterResult>
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

                if (request.RequestId != null)
                {
                    sessionRequest.AddHeader("X-GS2-REQUEST-ID", request.RequestId);
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
        )
		{
			var task = new GetMaxStaminaTableMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.GetMaxStaminaTableMasterResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.GetMaxStaminaTableMasterResult> GetMaxStaminaTableMasterAsync(
                Request.GetMaxStaminaTableMasterRequest request
        )
		{
			var task = new GetMaxStaminaTableMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        private class UpdateMaxStaminaTableMasterTask : Gs2RestSessionTask<UpdateMaxStaminaTableMasterRequest, UpdateMaxStaminaTableMasterResult>
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

                if (request.RequestId != null)
                {
                    sessionRequest.AddHeader("X-GS2-REQUEST-ID", request.RequestId);
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
        )
		{
			var task = new UpdateMaxStaminaTableMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.UpdateMaxStaminaTableMasterResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.UpdateMaxStaminaTableMasterResult> UpdateMaxStaminaTableMasterAsync(
                Request.UpdateMaxStaminaTableMasterRequest request
        )
		{
			var task = new UpdateMaxStaminaTableMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        private class DeleteMaxStaminaTableMasterTask : Gs2RestSessionTask<DeleteMaxStaminaTableMasterRequest, DeleteMaxStaminaTableMasterResult>
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

                if (request.RequestId != null)
                {
                    sessionRequest.AddHeader("X-GS2-REQUEST-ID", request.RequestId);
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
        )
		{
			var task = new DeleteMaxStaminaTableMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.DeleteMaxStaminaTableMasterResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.DeleteMaxStaminaTableMasterResult> DeleteMaxStaminaTableMasterAsync(
                Request.DeleteMaxStaminaTableMasterRequest request
        )
		{
			var task = new DeleteMaxStaminaTableMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        private class DescribeRecoverIntervalTableMastersTask : Gs2RestSessionTask<DescribeRecoverIntervalTableMastersRequest, DescribeRecoverIntervalTableMastersResult>
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
                if (request.PageToken != null) {
                    sessionRequest.AddQueryString("pageToken", $"{request.PageToken}");
                }
                if (request.Limit != null) {
                    sessionRequest.AddQueryString("limit", $"{request.Limit}");
                }

                if (request.RequestId != null)
                {
                    sessionRequest.AddHeader("X-GS2-REQUEST-ID", request.RequestId);
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
        )
		{
			var task = new DescribeRecoverIntervalTableMastersTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.DescribeRecoverIntervalTableMastersResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.DescribeRecoverIntervalTableMastersResult> DescribeRecoverIntervalTableMastersAsync(
                Request.DescribeRecoverIntervalTableMastersRequest request
        )
		{
			var task = new DescribeRecoverIntervalTableMastersTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        private class CreateRecoverIntervalTableMasterTask : Gs2RestSessionTask<CreateRecoverIntervalTableMasterRequest, CreateRecoverIntervalTableMasterResult>
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

                if (request.RequestId != null)
                {
                    sessionRequest.AddHeader("X-GS2-REQUEST-ID", request.RequestId);
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
        )
		{
			var task = new CreateRecoverIntervalTableMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.CreateRecoverIntervalTableMasterResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.CreateRecoverIntervalTableMasterResult> CreateRecoverIntervalTableMasterAsync(
                Request.CreateRecoverIntervalTableMasterRequest request
        )
		{
			var task = new CreateRecoverIntervalTableMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        private class GetRecoverIntervalTableMasterTask : Gs2RestSessionTask<GetRecoverIntervalTableMasterRequest, GetRecoverIntervalTableMasterResult>
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

                if (request.RequestId != null)
                {
                    sessionRequest.AddHeader("X-GS2-REQUEST-ID", request.RequestId);
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
        )
		{
			var task = new GetRecoverIntervalTableMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.GetRecoverIntervalTableMasterResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.GetRecoverIntervalTableMasterResult> GetRecoverIntervalTableMasterAsync(
                Request.GetRecoverIntervalTableMasterRequest request
        )
		{
			var task = new GetRecoverIntervalTableMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        private class UpdateRecoverIntervalTableMasterTask : Gs2RestSessionTask<UpdateRecoverIntervalTableMasterRequest, UpdateRecoverIntervalTableMasterResult>
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

                if (request.RequestId != null)
                {
                    sessionRequest.AddHeader("X-GS2-REQUEST-ID", request.RequestId);
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
        )
		{
			var task = new UpdateRecoverIntervalTableMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.UpdateRecoverIntervalTableMasterResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.UpdateRecoverIntervalTableMasterResult> UpdateRecoverIntervalTableMasterAsync(
                Request.UpdateRecoverIntervalTableMasterRequest request
        )
		{
			var task = new UpdateRecoverIntervalTableMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        private class DeleteRecoverIntervalTableMasterTask : Gs2RestSessionTask<DeleteRecoverIntervalTableMasterRequest, DeleteRecoverIntervalTableMasterResult>
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

                if (request.RequestId != null)
                {
                    sessionRequest.AddHeader("X-GS2-REQUEST-ID", request.RequestId);
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
        )
		{
			var task = new DeleteRecoverIntervalTableMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.DeleteRecoverIntervalTableMasterResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.DeleteRecoverIntervalTableMasterResult> DeleteRecoverIntervalTableMasterAsync(
                Request.DeleteRecoverIntervalTableMasterRequest request
        )
		{
			var task = new DeleteRecoverIntervalTableMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        private class DescribeRecoverValueTableMastersTask : Gs2RestSessionTask<DescribeRecoverValueTableMastersRequest, DescribeRecoverValueTableMastersResult>
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
                if (request.PageToken != null) {
                    sessionRequest.AddQueryString("pageToken", $"{request.PageToken}");
                }
                if (request.Limit != null) {
                    sessionRequest.AddQueryString("limit", $"{request.Limit}");
                }

                if (request.RequestId != null)
                {
                    sessionRequest.AddHeader("X-GS2-REQUEST-ID", request.RequestId);
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
        )
		{
			var task = new DescribeRecoverValueTableMastersTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.DescribeRecoverValueTableMastersResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.DescribeRecoverValueTableMastersResult> DescribeRecoverValueTableMastersAsync(
                Request.DescribeRecoverValueTableMastersRequest request
        )
		{
			var task = new DescribeRecoverValueTableMastersTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        private class CreateRecoverValueTableMasterTask : Gs2RestSessionTask<CreateRecoverValueTableMasterRequest, CreateRecoverValueTableMasterResult>
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

                if (request.RequestId != null)
                {
                    sessionRequest.AddHeader("X-GS2-REQUEST-ID", request.RequestId);
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
        )
		{
			var task = new CreateRecoverValueTableMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.CreateRecoverValueTableMasterResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.CreateRecoverValueTableMasterResult> CreateRecoverValueTableMasterAsync(
                Request.CreateRecoverValueTableMasterRequest request
        )
		{
			var task = new CreateRecoverValueTableMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        private class GetRecoverValueTableMasterTask : Gs2RestSessionTask<GetRecoverValueTableMasterRequest, GetRecoverValueTableMasterResult>
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

                if (request.RequestId != null)
                {
                    sessionRequest.AddHeader("X-GS2-REQUEST-ID", request.RequestId);
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
        )
		{
			var task = new GetRecoverValueTableMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.GetRecoverValueTableMasterResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.GetRecoverValueTableMasterResult> GetRecoverValueTableMasterAsync(
                Request.GetRecoverValueTableMasterRequest request
        )
		{
			var task = new GetRecoverValueTableMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        private class UpdateRecoverValueTableMasterTask : Gs2RestSessionTask<UpdateRecoverValueTableMasterRequest, UpdateRecoverValueTableMasterResult>
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

                if (request.RequestId != null)
                {
                    sessionRequest.AddHeader("X-GS2-REQUEST-ID", request.RequestId);
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
        )
		{
			var task = new UpdateRecoverValueTableMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.UpdateRecoverValueTableMasterResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.UpdateRecoverValueTableMasterResult> UpdateRecoverValueTableMasterAsync(
                Request.UpdateRecoverValueTableMasterRequest request
        )
		{
			var task = new UpdateRecoverValueTableMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        private class DeleteRecoverValueTableMasterTask : Gs2RestSessionTask<DeleteRecoverValueTableMasterRequest, DeleteRecoverValueTableMasterResult>
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

                if (request.RequestId != null)
                {
                    sessionRequest.AddHeader("X-GS2-REQUEST-ID", request.RequestId);
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
        )
		{
			var task = new DeleteRecoverValueTableMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.DeleteRecoverValueTableMasterResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.DeleteRecoverValueTableMasterResult> DeleteRecoverValueTableMasterAsync(
                Request.DeleteRecoverValueTableMasterRequest request
        )
		{
			var task = new DeleteRecoverValueTableMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        private class ExportMasterTask : Gs2RestSessionTask<ExportMasterRequest, ExportMasterResult>
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

                if (request.RequestId != null)
                {
                    sessionRequest.AddHeader("X-GS2-REQUEST-ID", request.RequestId);
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
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.ExportMasterResult>(task.Result, task.Error));
        }
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


        private class GetCurrentStaminaMasterTask : Gs2RestSessionTask<GetCurrentStaminaMasterRequest, GetCurrentStaminaMasterResult>
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

                if (request.RequestId != null)
                {
                    sessionRequest.AddHeader("X-GS2-REQUEST-ID", request.RequestId);
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
        )
		{
			var task = new GetCurrentStaminaMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.GetCurrentStaminaMasterResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.GetCurrentStaminaMasterResult> GetCurrentStaminaMasterAsync(
                Request.GetCurrentStaminaMasterRequest request
        )
		{
			var task = new GetCurrentStaminaMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        private class UpdateCurrentStaminaMasterTask : Gs2RestSessionTask<UpdateCurrentStaminaMasterRequest, UpdateCurrentStaminaMasterResult>
        {
            public UpdateCurrentStaminaMasterTask(IGs2Session session, RestSessionRequestFactory factory, UpdateCurrentStaminaMasterRequest request) : base(session, factory, request)
            {
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

                if (request.RequestId != null)
                {
                    sessionRequest.AddHeader("X-GS2-REQUEST-ID", request.RequestId);
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
        )
		{
			var task = new UpdateCurrentStaminaMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.UpdateCurrentStaminaMasterResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.UpdateCurrentStaminaMasterResult> UpdateCurrentStaminaMasterAsync(
                Request.UpdateCurrentStaminaMasterRequest request
        )
		{
			var task = new UpdateCurrentStaminaMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        private class UpdateCurrentStaminaMasterFromGitHubTask : Gs2RestSessionTask<UpdateCurrentStaminaMasterFromGitHubRequest, UpdateCurrentStaminaMasterFromGitHubResult>
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

                if (request.RequestId != null)
                {
                    sessionRequest.AddHeader("X-GS2-REQUEST-ID", request.RequestId);
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
        )
		{
			var task = new UpdateCurrentStaminaMasterFromGitHubTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.UpdateCurrentStaminaMasterFromGitHubResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.UpdateCurrentStaminaMasterFromGitHubResult> UpdateCurrentStaminaMasterFromGitHubAsync(
                Request.UpdateCurrentStaminaMasterFromGitHubRequest request
        )
		{
			var task = new UpdateCurrentStaminaMasterFromGitHubTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        private class DescribeStaminaModelsTask : Gs2RestSessionTask<DescribeStaminaModelsRequest, DescribeStaminaModelsResult>
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

                if (request.RequestId != null)
                {
                    sessionRequest.AddHeader("X-GS2-REQUEST-ID", request.RequestId);
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
        )
		{
			var task = new DescribeStaminaModelsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.DescribeStaminaModelsResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.DescribeStaminaModelsResult> DescribeStaminaModelsAsync(
                Request.DescribeStaminaModelsRequest request
        )
		{
			var task = new DescribeStaminaModelsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        private class GetStaminaModelTask : Gs2RestSessionTask<GetStaminaModelRequest, GetStaminaModelResult>
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

                if (request.RequestId != null)
                {
                    sessionRequest.AddHeader("X-GS2-REQUEST-ID", request.RequestId);
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
        )
		{
			var task = new GetStaminaModelTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.GetStaminaModelResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.GetStaminaModelResult> GetStaminaModelAsync(
                Request.GetStaminaModelRequest request
        )
		{
			var task = new GetStaminaModelTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        private class DescribeStaminasTask : Gs2RestSessionTask<DescribeStaminasRequest, DescribeStaminasResult>
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

                if (request.RequestId != null)
                {
                    sessionRequest.AddHeader("X-GS2-REQUEST-ID", request.RequestId);
                }
                if (request.AccessToken != null)
                {
                    sessionRequest.AddHeader("X-GS2-ACCESS-TOKEN", request.AccessToken);
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
        )
		{
			var task = new DescribeStaminasTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.DescribeStaminasResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.DescribeStaminasResult> DescribeStaminasAsync(
                Request.DescribeStaminasRequest request
        )
		{
			var task = new DescribeStaminasTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        private class DescribeStaminasByUserIdTask : Gs2RestSessionTask<DescribeStaminasByUserIdRequest, DescribeStaminasByUserIdResult>
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

                if (request.RequestId != null)
                {
                    sessionRequest.AddHeader("X-GS2-REQUEST-ID", request.RequestId);
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
        )
		{
			var task = new DescribeStaminasByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.DescribeStaminasByUserIdResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.DescribeStaminasByUserIdResult> DescribeStaminasByUserIdAsync(
                Request.DescribeStaminasByUserIdRequest request
        )
		{
			var task = new DescribeStaminasByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        private class GetStaminaTask : Gs2RestSessionTask<GetStaminaRequest, GetStaminaResult>
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

                if (request.RequestId != null)
                {
                    sessionRequest.AddHeader("X-GS2-REQUEST-ID", request.RequestId);
                }
                if (request.AccessToken != null)
                {
                    sessionRequest.AddHeader("X-GS2-ACCESS-TOKEN", request.AccessToken);
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
        )
		{
			var task = new GetStaminaTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.GetStaminaResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.GetStaminaResult> GetStaminaAsync(
                Request.GetStaminaRequest request
        )
		{
			var task = new GetStaminaTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        private class GetStaminaByUserIdTask : Gs2RestSessionTask<GetStaminaByUserIdRequest, GetStaminaByUserIdResult>
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

                if (request.RequestId != null)
                {
                    sessionRequest.AddHeader("X-GS2-REQUEST-ID", request.RequestId);
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
        )
		{
			var task = new GetStaminaByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.GetStaminaByUserIdResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.GetStaminaByUserIdResult> GetStaminaByUserIdAsync(
                Request.GetStaminaByUserIdRequest request
        )
		{
			var task = new GetStaminaByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        private class UpdateStaminaByUserIdTask : Gs2RestSessionTask<UpdateStaminaByUserIdRequest, UpdateStaminaByUserIdResult>
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

                if (request.RequestId != null)
                {
                    sessionRequest.AddHeader("X-GS2-REQUEST-ID", request.RequestId);
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
        )
		{
			var task = new UpdateStaminaByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.UpdateStaminaByUserIdResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.UpdateStaminaByUserIdResult> UpdateStaminaByUserIdAsync(
                Request.UpdateStaminaByUserIdRequest request
        )
		{
			var task = new UpdateStaminaByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        private class ConsumeStaminaTask : Gs2RestSessionTask<ConsumeStaminaRequest, ConsumeStaminaResult>
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

                if (request.RequestId != null)
                {
                    sessionRequest.AddHeader("X-GS2-REQUEST-ID", request.RequestId);
                }
                if (request.AccessToken != null)
                {
                    sessionRequest.AddHeader("X-GS2-ACCESS-TOKEN", request.AccessToken);
                }

                AddHeader(
                    Session.Credential,
                    sessionRequest
                );

                return sessionRequest;
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator ConsumeStamina(
                Request.ConsumeStaminaRequest request,
                UnityAction<AsyncResult<Result.ConsumeStaminaResult>> callback
        )
		{
			var task = new ConsumeStaminaTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.ConsumeStaminaResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.ConsumeStaminaResult> ConsumeStaminaAsync(
                Request.ConsumeStaminaRequest request
        )
		{
			var task = new ConsumeStaminaTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        private class ConsumeStaminaByUserIdTask : Gs2RestSessionTask<ConsumeStaminaByUserIdRequest, ConsumeStaminaByUserIdResult>
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

                if (request.RequestId != null)
                {
                    sessionRequest.AddHeader("X-GS2-REQUEST-ID", request.RequestId);
                }

                AddHeader(
                    Session.Credential,
                    sessionRequest
                );

                return sessionRequest;
            }
        }

#if UNITY_2017_1_OR_NEWER
		public IEnumerator ConsumeStaminaByUserId(
                Request.ConsumeStaminaByUserIdRequest request,
                UnityAction<AsyncResult<Result.ConsumeStaminaByUserIdResult>> callback
        )
		{
			var task = new ConsumeStaminaByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.ConsumeStaminaByUserIdResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.ConsumeStaminaByUserIdResult> ConsumeStaminaByUserIdAsync(
                Request.ConsumeStaminaByUserIdRequest request
        )
		{
			var task = new ConsumeStaminaByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        private class RecoverStaminaByUserIdTask : Gs2RestSessionTask<RecoverStaminaByUserIdRequest, RecoverStaminaByUserIdResult>
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

                if (request.RequestId != null)
                {
                    sessionRequest.AddHeader("X-GS2-REQUEST-ID", request.RequestId);
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
        )
		{
			var task = new RecoverStaminaByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.RecoverStaminaByUserIdResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.RecoverStaminaByUserIdResult> RecoverStaminaByUserIdAsync(
                Request.RecoverStaminaByUserIdRequest request
        )
		{
			var task = new RecoverStaminaByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        private class RaiseMaxValueByUserIdTask : Gs2RestSessionTask<RaiseMaxValueByUserIdRequest, RaiseMaxValueByUserIdResult>
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

                if (request.RequestId != null)
                {
                    sessionRequest.AddHeader("X-GS2-REQUEST-ID", request.RequestId);
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
        )
		{
			var task = new RaiseMaxValueByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.RaiseMaxValueByUserIdResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.RaiseMaxValueByUserIdResult> RaiseMaxValueByUserIdAsync(
                Request.RaiseMaxValueByUserIdRequest request
        )
		{
			var task = new RaiseMaxValueByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        private class SetMaxValueByUserIdTask : Gs2RestSessionTask<SetMaxValueByUserIdRequest, SetMaxValueByUserIdResult>
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

                if (request.RequestId != null)
                {
                    sessionRequest.AddHeader("X-GS2-REQUEST-ID", request.RequestId);
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
        )
		{
			var task = new SetMaxValueByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.SetMaxValueByUserIdResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.SetMaxValueByUserIdResult> SetMaxValueByUserIdAsync(
                Request.SetMaxValueByUserIdRequest request
        )
		{
			var task = new SetMaxValueByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        private class SetRecoverIntervalByUserIdTask : Gs2RestSessionTask<SetRecoverIntervalByUserIdRequest, SetRecoverIntervalByUserIdResult>
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

                if (request.RequestId != null)
                {
                    sessionRequest.AddHeader("X-GS2-REQUEST-ID", request.RequestId);
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
        )
		{
			var task = new SetRecoverIntervalByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.SetRecoverIntervalByUserIdResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.SetRecoverIntervalByUserIdResult> SetRecoverIntervalByUserIdAsync(
                Request.SetRecoverIntervalByUserIdRequest request
        )
		{
			var task = new SetRecoverIntervalByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        private class SetRecoverValueByUserIdTask : Gs2RestSessionTask<SetRecoverValueByUserIdRequest, SetRecoverValueByUserIdResult>
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

                if (request.RequestId != null)
                {
                    sessionRequest.AddHeader("X-GS2-REQUEST-ID", request.RequestId);
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
        )
		{
			var task = new SetRecoverValueByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.SetRecoverValueByUserIdResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.SetRecoverValueByUserIdResult> SetRecoverValueByUserIdAsync(
                Request.SetRecoverValueByUserIdRequest request
        )
		{
			var task = new SetRecoverValueByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        private class SetMaxValueByStatusTask : Gs2RestSessionTask<SetMaxValueByStatusRequest, SetMaxValueByStatusResult>
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

                if (request.RequestId != null)
                {
                    sessionRequest.AddHeader("X-GS2-REQUEST-ID", request.RequestId);
                }
                if (request.AccessToken != null)
                {
                    sessionRequest.AddHeader("X-GS2-ACCESS-TOKEN", request.AccessToken);
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
        )
		{
			var task = new SetMaxValueByStatusTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.SetMaxValueByStatusResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.SetMaxValueByStatusResult> SetMaxValueByStatusAsync(
                Request.SetMaxValueByStatusRequest request
        )
		{
			var task = new SetMaxValueByStatusTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        private class SetRecoverIntervalByStatusTask : Gs2RestSessionTask<SetRecoverIntervalByStatusRequest, SetRecoverIntervalByStatusResult>
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

                if (request.RequestId != null)
                {
                    sessionRequest.AddHeader("X-GS2-REQUEST-ID", request.RequestId);
                }
                if (request.AccessToken != null)
                {
                    sessionRequest.AddHeader("X-GS2-ACCESS-TOKEN", request.AccessToken);
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
        )
		{
			var task = new SetRecoverIntervalByStatusTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.SetRecoverIntervalByStatusResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.SetRecoverIntervalByStatusResult> SetRecoverIntervalByStatusAsync(
                Request.SetRecoverIntervalByStatusRequest request
        )
		{
			var task = new SetRecoverIntervalByStatusTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        private class SetRecoverValueByStatusTask : Gs2RestSessionTask<SetRecoverValueByStatusRequest, SetRecoverValueByStatusResult>
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

                if (request.RequestId != null)
                {
                    sessionRequest.AddHeader("X-GS2-REQUEST-ID", request.RequestId);
                }
                if (request.AccessToken != null)
                {
                    sessionRequest.AddHeader("X-GS2-ACCESS-TOKEN", request.AccessToken);
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
        )
		{
			var task = new SetRecoverValueByStatusTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.SetRecoverValueByStatusResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.SetRecoverValueByStatusResult> SetRecoverValueByStatusAsync(
                Request.SetRecoverValueByStatusRequest request
        )
		{
			var task = new SetRecoverValueByStatusTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        private class DeleteStaminaByUserIdTask : Gs2RestSessionTask<DeleteStaminaByUserIdRequest, DeleteStaminaByUserIdResult>
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

                if (request.RequestId != null)
                {
                    sessionRequest.AddHeader("X-GS2-REQUEST-ID", request.RequestId);
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
        )
		{
			var task = new DeleteStaminaByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.DeleteStaminaByUserIdResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.DeleteStaminaByUserIdResult> DeleteStaminaByUserIdAsync(
                Request.DeleteStaminaByUserIdRequest request
        )
		{
			var task = new DeleteStaminaByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        private class RecoverStaminaByStampSheetTask : Gs2RestSessionTask<RecoverStaminaByStampSheetRequest, RecoverStaminaByStampSheetResult>
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

                if (request.RequestId != null)
                {
                    sessionRequest.AddHeader("X-GS2-REQUEST-ID", request.RequestId);
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
        )
		{
			var task = new RecoverStaminaByStampSheetTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.RecoverStaminaByStampSheetResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.RecoverStaminaByStampSheetResult> RecoverStaminaByStampSheetAsync(
                Request.RecoverStaminaByStampSheetRequest request
        )
		{
			var task = new RecoverStaminaByStampSheetTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        private class RaiseMaxValueByStampSheetTask : Gs2RestSessionTask<RaiseMaxValueByStampSheetRequest, RaiseMaxValueByStampSheetResult>
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

                if (request.RequestId != null)
                {
                    sessionRequest.AddHeader("X-GS2-REQUEST-ID", request.RequestId);
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
        )
		{
			var task = new RaiseMaxValueByStampSheetTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.RaiseMaxValueByStampSheetResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.RaiseMaxValueByStampSheetResult> RaiseMaxValueByStampSheetAsync(
                Request.RaiseMaxValueByStampSheetRequest request
        )
		{
			var task = new RaiseMaxValueByStampSheetTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        private class SetMaxValueByStampSheetTask : Gs2RestSessionTask<SetMaxValueByStampSheetRequest, SetMaxValueByStampSheetResult>
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

                if (request.RequestId != null)
                {
                    sessionRequest.AddHeader("X-GS2-REQUEST-ID", request.RequestId);
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
        )
		{
			var task = new SetMaxValueByStampSheetTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.SetMaxValueByStampSheetResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.SetMaxValueByStampSheetResult> SetMaxValueByStampSheetAsync(
                Request.SetMaxValueByStampSheetRequest request
        )
		{
			var task = new SetMaxValueByStampSheetTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        private class SetRecoverIntervalByStampSheetTask : Gs2RestSessionTask<SetRecoverIntervalByStampSheetRequest, SetRecoverIntervalByStampSheetResult>
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

                if (request.RequestId != null)
                {
                    sessionRequest.AddHeader("X-GS2-REQUEST-ID", request.RequestId);
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
        )
		{
			var task = new SetRecoverIntervalByStampSheetTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.SetRecoverIntervalByStampSheetResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.SetRecoverIntervalByStampSheetResult> SetRecoverIntervalByStampSheetAsync(
                Request.SetRecoverIntervalByStampSheetRequest request
        )
		{
			var task = new SetRecoverIntervalByStampSheetTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        private class SetRecoverValueByStampSheetTask : Gs2RestSessionTask<SetRecoverValueByStampSheetRequest, SetRecoverValueByStampSheetResult>
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

                if (request.RequestId != null)
                {
                    sessionRequest.AddHeader("X-GS2-REQUEST-ID", request.RequestId);
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
        )
		{
			var task = new SetRecoverValueByStampSheetTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.SetRecoverValueByStampSheetResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.SetRecoverValueByStampSheetResult> SetRecoverValueByStampSheetAsync(
                Request.SetRecoverValueByStampSheetRequest request
        )
		{
			var task = new SetRecoverValueByStampSheetTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        private class ConsumeStaminaByStampTaskTask : Gs2RestSessionTask<ConsumeStaminaByStampTaskRequest, ConsumeStaminaByStampTaskResult>
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

                if (request.RequestId != null)
                {
                    sessionRequest.AddHeader("X-GS2-REQUEST-ID", request.RequestId);
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
        )
		{
			var task = new ConsumeStaminaByStampTaskTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.ConsumeStaminaByStampTaskResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.ConsumeStaminaByStampTaskResult> ConsumeStaminaByStampTaskAsync(
                Request.ConsumeStaminaByStampTaskRequest request
        )
		{
			var task = new ConsumeStaminaByStampTaskTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif
	}
}