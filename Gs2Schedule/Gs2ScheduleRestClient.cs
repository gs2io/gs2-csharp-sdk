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
using Gs2.Gs2Schedule.Request;
using Gs2.Gs2Schedule.Result;
using Gs2.Util.LitJson;

namespace Gs2.Gs2Schedule
{
	public class Gs2ScheduleRestClient : AbstractGs2Client
	{
#if UNITY_2017_1_OR_NEWER
		private readonly CertificateHandler _certificateHandler;
#endif

		public static string Endpoint = "schedule";

        protected Gs2RestSession Gs2RestSession => (Gs2RestSession) Gs2Session;

		public Gs2ScheduleRestClient(Gs2RestSession Gs2RestSession) : base(Gs2RestSession)
		{

		}

#if UNITY_2017_1_OR_NEWER
		public Gs2ScheduleRestClient(Gs2RestSession gs2RestSession, CertificateHandler certificateHandler) : base(gs2RestSession)
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
                    .Replace("{service}", "schedule")
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
                    .Replace("{service}", "schedule")
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
                    .Replace("{service}", "schedule")
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
                    .Replace("{service}", "schedule")
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
                    .Replace("{service}", "schedule")
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
                    .Replace("{service}", "schedule")
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


        private class DescribeEventMastersTask : Gs2RestSessionTask<DescribeEventMastersRequest, DescribeEventMastersResult>
        {
            public DescribeEventMastersTask(IGs2Session session, RestSessionRequestFactory factory, DescribeEventMastersRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DescribeEventMastersRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "schedule")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/master/event";

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
		public IEnumerator DescribeEventMasters(
                Request.DescribeEventMastersRequest request,
                UnityAction<AsyncResult<Result.DescribeEventMastersResult>> callback
        )
		{
			var task = new DescribeEventMastersTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.DescribeEventMastersResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.DescribeEventMastersResult> DescribeEventMastersAsync(
                Request.DescribeEventMastersRequest request
        )
		{
			var task = new DescribeEventMastersTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        private class CreateEventMasterTask : Gs2RestSessionTask<CreateEventMasterRequest, CreateEventMasterResult>
        {
            public CreateEventMasterTask(IGs2Session session, RestSessionRequestFactory factory, CreateEventMasterRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(CreateEventMasterRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "schedule")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/master/event";

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
                if (request.ScheduleType != null)
                {
                    jsonWriter.WritePropertyName("scheduleType");
                    jsonWriter.Write(request.ScheduleType);
                }
                if (request.AbsoluteBegin != null)
                {
                    jsonWriter.WritePropertyName("absoluteBegin");
                    jsonWriter.Write(request.AbsoluteBegin.ToString());
                }
                if (request.AbsoluteEnd != null)
                {
                    jsonWriter.WritePropertyName("absoluteEnd");
                    jsonWriter.Write(request.AbsoluteEnd.ToString());
                }
                if (request.RepeatType != null)
                {
                    jsonWriter.WritePropertyName("repeatType");
                    jsonWriter.Write(request.RepeatType);
                }
                if (request.RepeatBeginDayOfMonth != null)
                {
                    jsonWriter.WritePropertyName("repeatBeginDayOfMonth");
                    jsonWriter.Write(request.RepeatBeginDayOfMonth.ToString());
                }
                if (request.RepeatEndDayOfMonth != null)
                {
                    jsonWriter.WritePropertyName("repeatEndDayOfMonth");
                    jsonWriter.Write(request.RepeatEndDayOfMonth.ToString());
                }
                if (request.RepeatBeginDayOfWeek != null)
                {
                    jsonWriter.WritePropertyName("repeatBeginDayOfWeek");
                    jsonWriter.Write(request.RepeatBeginDayOfWeek);
                }
                if (request.RepeatEndDayOfWeek != null)
                {
                    jsonWriter.WritePropertyName("repeatEndDayOfWeek");
                    jsonWriter.Write(request.RepeatEndDayOfWeek);
                }
                if (request.RepeatBeginHour != null)
                {
                    jsonWriter.WritePropertyName("repeatBeginHour");
                    jsonWriter.Write(request.RepeatBeginHour.ToString());
                }
                if (request.RepeatEndHour != null)
                {
                    jsonWriter.WritePropertyName("repeatEndHour");
                    jsonWriter.Write(request.RepeatEndHour.ToString());
                }
                if (request.RelativeTriggerName != null)
                {
                    jsonWriter.WritePropertyName("relativeTriggerName");
                    jsonWriter.Write(request.RelativeTriggerName);
                }
                if (request.RelativeDuration != null)
                {
                    jsonWriter.WritePropertyName("relativeDuration");
                    jsonWriter.Write(request.RelativeDuration.ToString());
                }
                if (request.ContextStack != null)
                {
                    jsonWriter.WritePropertyName("contextStack");
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
		public IEnumerator CreateEventMaster(
                Request.CreateEventMasterRequest request,
                UnityAction<AsyncResult<Result.CreateEventMasterResult>> callback
        )
		{
			var task = new CreateEventMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.CreateEventMasterResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.CreateEventMasterResult> CreateEventMasterAsync(
                Request.CreateEventMasterRequest request
        )
		{
			var task = new CreateEventMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        private class GetEventMasterTask : Gs2RestSessionTask<GetEventMasterRequest, GetEventMasterResult>
        {
            public GetEventMasterTask(IGs2Session session, RestSessionRequestFactory factory, GetEventMasterRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(GetEventMasterRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "schedule")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/master/event/{eventName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{eventName}", !string.IsNullOrEmpty(request.EventName) ? request.EventName.ToString() : "null");

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
		public IEnumerator GetEventMaster(
                Request.GetEventMasterRequest request,
                UnityAction<AsyncResult<Result.GetEventMasterResult>> callback
        )
		{
			var task = new GetEventMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.GetEventMasterResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.GetEventMasterResult> GetEventMasterAsync(
                Request.GetEventMasterRequest request
        )
		{
			var task = new GetEventMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        private class UpdateEventMasterTask : Gs2RestSessionTask<UpdateEventMasterRequest, UpdateEventMasterResult>
        {
            public UpdateEventMasterTask(IGs2Session session, RestSessionRequestFactory factory, UpdateEventMasterRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(UpdateEventMasterRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "schedule")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/master/event/{eventName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{eventName}", !string.IsNullOrEmpty(request.EventName) ? request.EventName.ToString() : "null");

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
                if (request.ScheduleType != null)
                {
                    jsonWriter.WritePropertyName("scheduleType");
                    jsonWriter.Write(request.ScheduleType);
                }
                if (request.AbsoluteBegin != null)
                {
                    jsonWriter.WritePropertyName("absoluteBegin");
                    jsonWriter.Write(request.AbsoluteBegin.ToString());
                }
                if (request.AbsoluteEnd != null)
                {
                    jsonWriter.WritePropertyName("absoluteEnd");
                    jsonWriter.Write(request.AbsoluteEnd.ToString());
                }
                if (request.RepeatType != null)
                {
                    jsonWriter.WritePropertyName("repeatType");
                    jsonWriter.Write(request.RepeatType);
                }
                if (request.RepeatBeginDayOfMonth != null)
                {
                    jsonWriter.WritePropertyName("repeatBeginDayOfMonth");
                    jsonWriter.Write(request.RepeatBeginDayOfMonth.ToString());
                }
                if (request.RepeatEndDayOfMonth != null)
                {
                    jsonWriter.WritePropertyName("repeatEndDayOfMonth");
                    jsonWriter.Write(request.RepeatEndDayOfMonth.ToString());
                }
                if (request.RepeatBeginDayOfWeek != null)
                {
                    jsonWriter.WritePropertyName("repeatBeginDayOfWeek");
                    jsonWriter.Write(request.RepeatBeginDayOfWeek);
                }
                if (request.RepeatEndDayOfWeek != null)
                {
                    jsonWriter.WritePropertyName("repeatEndDayOfWeek");
                    jsonWriter.Write(request.RepeatEndDayOfWeek);
                }
                if (request.RepeatBeginHour != null)
                {
                    jsonWriter.WritePropertyName("repeatBeginHour");
                    jsonWriter.Write(request.RepeatBeginHour.ToString());
                }
                if (request.RepeatEndHour != null)
                {
                    jsonWriter.WritePropertyName("repeatEndHour");
                    jsonWriter.Write(request.RepeatEndHour.ToString());
                }
                if (request.RelativeTriggerName != null)
                {
                    jsonWriter.WritePropertyName("relativeTriggerName");
                    jsonWriter.Write(request.RelativeTriggerName);
                }
                if (request.RelativeDuration != null)
                {
                    jsonWriter.WritePropertyName("relativeDuration");
                    jsonWriter.Write(request.RelativeDuration.ToString());
                }
                if (request.ContextStack != null)
                {
                    jsonWriter.WritePropertyName("contextStack");
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
		public IEnumerator UpdateEventMaster(
                Request.UpdateEventMasterRequest request,
                UnityAction<AsyncResult<Result.UpdateEventMasterResult>> callback
        )
		{
			var task = new UpdateEventMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.UpdateEventMasterResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.UpdateEventMasterResult> UpdateEventMasterAsync(
                Request.UpdateEventMasterRequest request
        )
		{
			var task = new UpdateEventMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        private class DeleteEventMasterTask : Gs2RestSessionTask<DeleteEventMasterRequest, DeleteEventMasterResult>
        {
            public DeleteEventMasterTask(IGs2Session session, RestSessionRequestFactory factory, DeleteEventMasterRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DeleteEventMasterRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "schedule")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/master/event/{eventName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{eventName}", !string.IsNullOrEmpty(request.EventName) ? request.EventName.ToString() : "null");

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
		public IEnumerator DeleteEventMaster(
                Request.DeleteEventMasterRequest request,
                UnityAction<AsyncResult<Result.DeleteEventMasterResult>> callback
        )
		{
			var task = new DeleteEventMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.DeleteEventMasterResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.DeleteEventMasterResult> DeleteEventMasterAsync(
                Request.DeleteEventMasterRequest request
        )
		{
			var task = new DeleteEventMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        private class DescribeTriggersTask : Gs2RestSessionTask<DescribeTriggersRequest, DescribeTriggersResult>
        {
            public DescribeTriggersTask(IGs2Session session, RestSessionRequestFactory factory, DescribeTriggersRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DescribeTriggersRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "schedule")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/me/trigger";

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
		public IEnumerator DescribeTriggers(
                Request.DescribeTriggersRequest request,
                UnityAction<AsyncResult<Result.DescribeTriggersResult>> callback
        )
		{
			var task = new DescribeTriggersTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.DescribeTriggersResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.DescribeTriggersResult> DescribeTriggersAsync(
                Request.DescribeTriggersRequest request
        )
		{
			var task = new DescribeTriggersTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        private class DescribeTriggersByUserIdTask : Gs2RestSessionTask<DescribeTriggersByUserIdRequest, DescribeTriggersByUserIdResult>
        {
            public DescribeTriggersByUserIdTask(IGs2Session session, RestSessionRequestFactory factory, DescribeTriggersByUserIdRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DescribeTriggersByUserIdRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "schedule")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/{userId}/trigger";

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
		public IEnumerator DescribeTriggersByUserId(
                Request.DescribeTriggersByUserIdRequest request,
                UnityAction<AsyncResult<Result.DescribeTriggersByUserIdResult>> callback
        )
		{
			var task = new DescribeTriggersByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.DescribeTriggersByUserIdResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.DescribeTriggersByUserIdResult> DescribeTriggersByUserIdAsync(
                Request.DescribeTriggersByUserIdRequest request
        )
		{
			var task = new DescribeTriggersByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        private class GetTriggerTask : Gs2RestSessionTask<GetTriggerRequest, GetTriggerResult>
        {
            public GetTriggerTask(IGs2Session session, RestSessionRequestFactory factory, GetTriggerRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(GetTriggerRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "schedule")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/me/trigger/{triggerName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{triggerName}", !string.IsNullOrEmpty(request.TriggerName) ? request.TriggerName.ToString() : "null");

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
		public IEnumerator GetTrigger(
                Request.GetTriggerRequest request,
                UnityAction<AsyncResult<Result.GetTriggerResult>> callback
        )
		{
			var task = new GetTriggerTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.GetTriggerResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.GetTriggerResult> GetTriggerAsync(
                Request.GetTriggerRequest request
        )
		{
			var task = new GetTriggerTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        private class GetTriggerByUserIdTask : Gs2RestSessionTask<GetTriggerByUserIdRequest, GetTriggerByUserIdResult>
        {
            public GetTriggerByUserIdTask(IGs2Session session, RestSessionRequestFactory factory, GetTriggerByUserIdRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(GetTriggerByUserIdRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "schedule")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/{userId}/trigger/{triggerName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{userId}", !string.IsNullOrEmpty(request.UserId) ? request.UserId.ToString() : "null");
                url = url.Replace("{triggerName}", !string.IsNullOrEmpty(request.TriggerName) ? request.TriggerName.ToString() : "null");

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
		public IEnumerator GetTriggerByUserId(
                Request.GetTriggerByUserIdRequest request,
                UnityAction<AsyncResult<Result.GetTriggerByUserIdResult>> callback
        )
		{
			var task = new GetTriggerByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.GetTriggerByUserIdResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.GetTriggerByUserIdResult> GetTriggerByUserIdAsync(
                Request.GetTriggerByUserIdRequest request
        )
		{
			var task = new GetTriggerByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        private class TriggerByUserIdTask : Gs2RestSessionTask<TriggerByUserIdRequest, TriggerByUserIdResult>
        {
            public TriggerByUserIdTask(IGs2Session session, RestSessionRequestFactory factory, TriggerByUserIdRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(TriggerByUserIdRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "schedule")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/{userId}/trigger/{triggerName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{triggerName}", !string.IsNullOrEmpty(request.TriggerName) ? request.TriggerName.ToString() : "null");
                url = url.Replace("{userId}", !string.IsNullOrEmpty(request.UserId) ? request.UserId.ToString() : "null");

                var sessionRequest = Factory.Post(url);

                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);
                jsonWriter.WriteObjectStart();
                if (request.TriggerStrategy != null)
                {
                    jsonWriter.WritePropertyName("triggerStrategy");
                    jsonWriter.Write(request.TriggerStrategy);
                }
                if (request.Ttl != null)
                {
                    jsonWriter.WritePropertyName("ttl");
                    jsonWriter.Write(request.Ttl.ToString());
                }
                if (request.ContextStack != null)
                {
                    jsonWriter.WritePropertyName("contextStack");
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
		public IEnumerator TriggerByUserId(
                Request.TriggerByUserIdRequest request,
                UnityAction<AsyncResult<Result.TriggerByUserIdResult>> callback
        )
		{
			var task = new TriggerByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.TriggerByUserIdResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.TriggerByUserIdResult> TriggerByUserIdAsync(
                Request.TriggerByUserIdRequest request
        )
		{
			var task = new TriggerByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        private class DeleteTriggerTask : Gs2RestSessionTask<DeleteTriggerRequest, DeleteTriggerResult>
        {
            public DeleteTriggerTask(IGs2Session session, RestSessionRequestFactory factory, DeleteTriggerRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DeleteTriggerRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "schedule")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/me/trigger/{triggerName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{triggerName}", !string.IsNullOrEmpty(request.TriggerName) ? request.TriggerName.ToString() : "null");

                var sessionRequest = Factory.Delete(url);
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
		public IEnumerator DeleteTrigger(
                Request.DeleteTriggerRequest request,
                UnityAction<AsyncResult<Result.DeleteTriggerResult>> callback
        )
		{
			var task = new DeleteTriggerTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.DeleteTriggerResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.DeleteTriggerResult> DeleteTriggerAsync(
                Request.DeleteTriggerRequest request
        )
		{
			var task = new DeleteTriggerTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        private class DeleteTriggerByUserIdTask : Gs2RestSessionTask<DeleteTriggerByUserIdRequest, DeleteTriggerByUserIdResult>
        {
            public DeleteTriggerByUserIdTask(IGs2Session session, RestSessionRequestFactory factory, DeleteTriggerByUserIdRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DeleteTriggerByUserIdRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "schedule")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/{userId}/trigger/{triggerName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{userId}", !string.IsNullOrEmpty(request.UserId) ? request.UserId.ToString() : "null");
                url = url.Replace("{triggerName}", !string.IsNullOrEmpty(request.TriggerName) ? request.TriggerName.ToString() : "null");

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
		public IEnumerator DeleteTriggerByUserId(
                Request.DeleteTriggerByUserIdRequest request,
                UnityAction<AsyncResult<Result.DeleteTriggerByUserIdResult>> callback
        )
		{
			var task = new DeleteTriggerByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.DeleteTriggerByUserIdResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.DeleteTriggerByUserIdResult> DeleteTriggerByUserIdAsync(
                Request.DeleteTriggerByUserIdRequest request
        )
		{
			var task = new DeleteTriggerByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        private class DescribeEventsTask : Gs2RestSessionTask<DescribeEventsRequest, DescribeEventsResult>
        {
            public DescribeEventsTask(IGs2Session session, RestSessionRequestFactory factory, DescribeEventsRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DescribeEventsRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "schedule")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/me/event";

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
		public IEnumerator DescribeEvents(
                Request.DescribeEventsRequest request,
                UnityAction<AsyncResult<Result.DescribeEventsResult>> callback
        )
		{
			var task = new DescribeEventsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.DescribeEventsResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.DescribeEventsResult> DescribeEventsAsync(
                Request.DescribeEventsRequest request
        )
		{
			var task = new DescribeEventsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        private class DescribeEventsByUserIdTask : Gs2RestSessionTask<DescribeEventsByUserIdRequest, DescribeEventsByUserIdResult>
        {
            public DescribeEventsByUserIdTask(IGs2Session session, RestSessionRequestFactory factory, DescribeEventsByUserIdRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DescribeEventsByUserIdRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "schedule")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/{userId}/event";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
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
		public IEnumerator DescribeEventsByUserId(
                Request.DescribeEventsByUserIdRequest request,
                UnityAction<AsyncResult<Result.DescribeEventsByUserIdResult>> callback
        )
		{
			var task = new DescribeEventsByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.DescribeEventsByUserIdResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.DescribeEventsByUserIdResult> DescribeEventsByUserIdAsync(
                Request.DescribeEventsByUserIdRequest request
        )
		{
			var task = new DescribeEventsByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        private class DescribeRawEventsTask : Gs2RestSessionTask<DescribeRawEventsRequest, DescribeRawEventsResult>
        {
            public DescribeRawEventsTask(IGs2Session session, RestSessionRequestFactory factory, DescribeRawEventsRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DescribeRawEventsRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "schedule")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/event";

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
		public IEnumerator DescribeRawEvents(
                Request.DescribeRawEventsRequest request,
                UnityAction<AsyncResult<Result.DescribeRawEventsResult>> callback
        )
		{
			var task = new DescribeRawEventsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.DescribeRawEventsResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.DescribeRawEventsResult> DescribeRawEventsAsync(
                Request.DescribeRawEventsRequest request
        )
		{
			var task = new DescribeRawEventsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        private class GetEventTask : Gs2RestSessionTask<GetEventRequest, GetEventResult>
        {
            public GetEventTask(IGs2Session session, RestSessionRequestFactory factory, GetEventRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(GetEventRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "schedule")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/me/event/{eventName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{eventName}", !string.IsNullOrEmpty(request.EventName) ? request.EventName.ToString() : "null");

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
		public IEnumerator GetEvent(
                Request.GetEventRequest request,
                UnityAction<AsyncResult<Result.GetEventResult>> callback
        )
		{
			var task = new GetEventTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.GetEventResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.GetEventResult> GetEventAsync(
                Request.GetEventRequest request
        )
		{
			var task = new GetEventTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        private class GetEventByUserIdTask : Gs2RestSessionTask<GetEventByUserIdRequest, GetEventByUserIdResult>
        {
            public GetEventByUserIdTask(IGs2Session session, RestSessionRequestFactory factory, GetEventByUserIdRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(GetEventByUserIdRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "schedule")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/user/{userId}/event/{eventName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{eventName}", !string.IsNullOrEmpty(request.EventName) ? request.EventName.ToString() : "null");
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
		public IEnumerator GetEventByUserId(
                Request.GetEventByUserIdRequest request,
                UnityAction<AsyncResult<Result.GetEventByUserIdResult>> callback
        )
		{
			var task = new GetEventByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.GetEventByUserIdResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.GetEventByUserIdResult> GetEventByUserIdAsync(
                Request.GetEventByUserIdRequest request
        )
		{
			var task = new GetEventByUserIdTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        private class GetRawEventTask : Gs2RestSessionTask<GetRawEventRequest, GetRawEventResult>
        {
            public GetRawEventTask(IGs2Session session, RestSessionRequestFactory factory, GetRawEventRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(GetRawEventRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "schedule")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{namespaceName}/event/{eventName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(request.NamespaceName) ? request.NamespaceName.ToString() : "null");
                url = url.Replace("{eventName}", !string.IsNullOrEmpty(request.EventName) ? request.EventName.ToString() : "null");

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
		public IEnumerator GetRawEvent(
                Request.GetRawEventRequest request,
                UnityAction<AsyncResult<Result.GetRawEventResult>> callback
        )
		{
			var task = new GetRawEventTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.GetRawEventResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.GetRawEventResult> GetRawEventAsync(
                Request.GetRawEventRequest request
        )
		{
			var task = new GetRawEventTask(
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
                    .Replace("{service}", "schedule")
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


        private class GetCurrentEventMasterTask : Gs2RestSessionTask<GetCurrentEventMasterRequest, GetCurrentEventMasterResult>
        {
            public GetCurrentEventMasterTask(IGs2Session session, RestSessionRequestFactory factory, GetCurrentEventMasterRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(GetCurrentEventMasterRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "schedule")
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
		public IEnumerator GetCurrentEventMaster(
                Request.GetCurrentEventMasterRequest request,
                UnityAction<AsyncResult<Result.GetCurrentEventMasterResult>> callback
        )
		{
			var task = new GetCurrentEventMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.GetCurrentEventMasterResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.GetCurrentEventMasterResult> GetCurrentEventMasterAsync(
                Request.GetCurrentEventMasterRequest request
        )
		{
			var task = new GetCurrentEventMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        private class UpdateCurrentEventMasterTask : Gs2RestSessionTask<UpdateCurrentEventMasterRequest, UpdateCurrentEventMasterResult>
        {
            public UpdateCurrentEventMasterTask(IGs2Session session, RestSessionRequestFactory factory, UpdateCurrentEventMasterRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(UpdateCurrentEventMasterRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "schedule")
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
		public IEnumerator UpdateCurrentEventMaster(
                Request.UpdateCurrentEventMasterRequest request,
                UnityAction<AsyncResult<Result.UpdateCurrentEventMasterResult>> callback
        )
		{
			var task = new UpdateCurrentEventMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.UpdateCurrentEventMasterResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.UpdateCurrentEventMasterResult> UpdateCurrentEventMasterAsync(
                Request.UpdateCurrentEventMasterRequest request
        )
		{
			var task = new UpdateCurrentEventMasterTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        private class UpdateCurrentEventMasterFromGitHubTask : Gs2RestSessionTask<UpdateCurrentEventMasterFromGitHubRequest, UpdateCurrentEventMasterFromGitHubResult>
        {
            public UpdateCurrentEventMasterFromGitHubTask(IGs2Session session, RestSessionRequestFactory factory, UpdateCurrentEventMasterFromGitHubRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(UpdateCurrentEventMasterFromGitHubRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "schedule")
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
		public IEnumerator UpdateCurrentEventMasterFromGitHub(
                Request.UpdateCurrentEventMasterFromGitHubRequest request,
                UnityAction<AsyncResult<Result.UpdateCurrentEventMasterFromGitHubResult>> callback
        )
		{
			var task = new UpdateCurrentEventMasterFromGitHubTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.UpdateCurrentEventMasterFromGitHubResult>(task.Result, task.Error));
        }
#else
		public async Task<Result.UpdateCurrentEventMasterFromGitHubResult> UpdateCurrentEventMasterFromGitHubAsync(
                Request.UpdateCurrentEventMasterFromGitHubRequest request
        )
		{
			var task = new UpdateCurrentEventMasterFromGitHubTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif
	}
}