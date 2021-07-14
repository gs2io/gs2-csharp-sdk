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
using UnityEngine.Events;
using UnityEngine.Networking;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Gs2.Core;
using Gs2.Core.Model;
using Gs2.Core.Net;
using Gs2.Util.LitJson;namespace Gs2.Gs2Schedule
{
	public class Gs2ScheduleWebSocketClient : AbstractGs2Client
	{

		public static string Endpoint = "schedule";

        protected Gs2WebSocketSession Gs2WebSocketSession => (Gs2WebSocketSession) Gs2Session;

		public Gs2ScheduleWebSocketClient(Gs2WebSocketSession Gs2WebSocketSession) : base(Gs2WebSocketSession)
		{

		}

        private class CreateNamespaceTask : Gs2WebSocketSessionTask<Result.CreateNamespaceResult>
        {
			private readonly Request.CreateNamespaceRequest _request;

			public CreateNamespaceTask(Request.CreateNamespaceRequest request, UnityAction<AsyncResult<Result.CreateNamespaceResult>> userCallback) : base(userCallback)
			{
				_request = request;
			}

            protected override IEnumerator ExecuteImpl(Gs2Session gs2Session)
            {
                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);

                jsonWriter.WriteObjectStart();

                if (_request.Name != null)
                {
                    jsonWriter.WritePropertyName("name");
                    jsonWriter.Write(_request.Name.ToString());
                }
                if (_request.Description != null)
                {
                    jsonWriter.WritePropertyName("description");
                    jsonWriter.Write(_request.Description.ToString());
                }
                if (_request.LogSetting != null)
                {
                    jsonWriter.WritePropertyName("logSetting");
                    _request.LogSetting.WriteJson(jsonWriter);
                }
                if (_request.ContextStack != null)
                {
                    jsonWriter.WritePropertyName("contextStack");
                    jsonWriter.Write(_request.ContextStack.ToString());
                }
                if (_request.RequestId != null)
                {
                    jsonWriter.WritePropertyName("xGs2RequestId");
                    jsonWriter.Write(_request.RequestId);
                }

                jsonWriter.WritePropertyName("xGs2ClientId");
                jsonWriter.Write(gs2Session.Credential.ClientId);
                jsonWriter.WritePropertyName("xGs2ProjectToken");
                jsonWriter.Write(gs2Session.ProjectToken);

                jsonWriter.WritePropertyName("x_gs2");
                jsonWriter.WriteObjectStart();
                jsonWriter.WritePropertyName("service");
                jsonWriter.Write("schedule");
                jsonWriter.WritePropertyName("component");
                jsonWriter.Write("namespace");
                jsonWriter.WritePropertyName("function");
                jsonWriter.Write("createNamespace");
                jsonWriter.WritePropertyName("contentType");
                jsonWriter.Write("application/json");
                jsonWriter.WritePropertyName("requestId");
                jsonWriter.Write(Gs2SessionTaskId.ToString());
                jsonWriter.WriteObjectEnd();

                jsonWriter.WriteObjectEnd();

                ((Gs2WebSocketSession)gs2Session).Send(stringBuilder.ToString());

                return new EmptyCoroutine();
            }
        }

		public IEnumerator CreateNamespace(
                Request.CreateNamespaceRequest request,
                UnityAction<AsyncResult<Result.CreateNamespaceResult>> callback
        )
		{
			var task = new CreateNamespaceTask(request, callback);
			return Gs2WebSocketSession.Execute(task);
        }

        private class GetNamespaceTask : Gs2WebSocketSessionTask<Result.GetNamespaceResult>
        {
			private readonly Request.GetNamespaceRequest _request;

			public GetNamespaceTask(Request.GetNamespaceRequest request, UnityAction<AsyncResult<Result.GetNamespaceResult>> userCallback) : base(userCallback)
			{
				_request = request;
			}

            protected override IEnumerator ExecuteImpl(Gs2Session gs2Session)
            {
                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);

                jsonWriter.WriteObjectStart();

                if (_request.NamespaceName != null)
                {
                    jsonWriter.WritePropertyName("namespaceName");
                    jsonWriter.Write(_request.NamespaceName.ToString());
                }
                if (_request.ContextStack != null)
                {
                    jsonWriter.WritePropertyName("contextStack");
                    jsonWriter.Write(_request.ContextStack.ToString());
                }
                if (_request.RequestId != null)
                {
                    jsonWriter.WritePropertyName("xGs2RequestId");
                    jsonWriter.Write(_request.RequestId);
                }

                jsonWriter.WritePropertyName("xGs2ClientId");
                jsonWriter.Write(gs2Session.Credential.ClientId);
                jsonWriter.WritePropertyName("xGs2ProjectToken");
                jsonWriter.Write(gs2Session.ProjectToken);

                jsonWriter.WritePropertyName("x_gs2");
                jsonWriter.WriteObjectStart();
                jsonWriter.WritePropertyName("service");
                jsonWriter.Write("schedule");
                jsonWriter.WritePropertyName("component");
                jsonWriter.Write("namespace");
                jsonWriter.WritePropertyName("function");
                jsonWriter.Write("getNamespace");
                jsonWriter.WritePropertyName("contentType");
                jsonWriter.Write("application/json");
                jsonWriter.WritePropertyName("requestId");
                jsonWriter.Write(Gs2SessionTaskId.ToString());
                jsonWriter.WriteObjectEnd();

                jsonWriter.WriteObjectEnd();

                ((Gs2WebSocketSession)gs2Session).Send(stringBuilder.ToString());

                return new EmptyCoroutine();
            }
        }

		public IEnumerator GetNamespace(
                Request.GetNamespaceRequest request,
                UnityAction<AsyncResult<Result.GetNamespaceResult>> callback
        )
		{
			var task = new GetNamespaceTask(request, callback);
			return Gs2WebSocketSession.Execute(task);
        }

        private class UpdateNamespaceTask : Gs2WebSocketSessionTask<Result.UpdateNamespaceResult>
        {
			private readonly Request.UpdateNamespaceRequest _request;

			public UpdateNamespaceTask(Request.UpdateNamespaceRequest request, UnityAction<AsyncResult<Result.UpdateNamespaceResult>> userCallback) : base(userCallback)
			{
				_request = request;
			}

            protected override IEnumerator ExecuteImpl(Gs2Session gs2Session)
            {
                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);

                jsonWriter.WriteObjectStart();

                if (_request.NamespaceName != null)
                {
                    jsonWriter.WritePropertyName("namespaceName");
                    jsonWriter.Write(_request.NamespaceName.ToString());
                }
                if (_request.Description != null)
                {
                    jsonWriter.WritePropertyName("description");
                    jsonWriter.Write(_request.Description.ToString());
                }
                if (_request.LogSetting != null)
                {
                    jsonWriter.WritePropertyName("logSetting");
                    _request.LogSetting.WriteJson(jsonWriter);
                }
                if (_request.ContextStack != null)
                {
                    jsonWriter.WritePropertyName("contextStack");
                    jsonWriter.Write(_request.ContextStack.ToString());
                }
                if (_request.RequestId != null)
                {
                    jsonWriter.WritePropertyName("xGs2RequestId");
                    jsonWriter.Write(_request.RequestId);
                }

                jsonWriter.WritePropertyName("xGs2ClientId");
                jsonWriter.Write(gs2Session.Credential.ClientId);
                jsonWriter.WritePropertyName("xGs2ProjectToken");
                jsonWriter.Write(gs2Session.ProjectToken);

                jsonWriter.WritePropertyName("x_gs2");
                jsonWriter.WriteObjectStart();
                jsonWriter.WritePropertyName("service");
                jsonWriter.Write("schedule");
                jsonWriter.WritePropertyName("component");
                jsonWriter.Write("namespace");
                jsonWriter.WritePropertyName("function");
                jsonWriter.Write("updateNamespace");
                jsonWriter.WritePropertyName("contentType");
                jsonWriter.Write("application/json");
                jsonWriter.WritePropertyName("requestId");
                jsonWriter.Write(Gs2SessionTaskId.ToString());
                jsonWriter.WriteObjectEnd();

                jsonWriter.WriteObjectEnd();

                ((Gs2WebSocketSession)gs2Session).Send(stringBuilder.ToString());

                return new EmptyCoroutine();
            }
        }

		public IEnumerator UpdateNamespace(
                Request.UpdateNamespaceRequest request,
                UnityAction<AsyncResult<Result.UpdateNamespaceResult>> callback
        )
		{
			var task = new UpdateNamespaceTask(request, callback);
			return Gs2WebSocketSession.Execute(task);
        }

        private class DeleteNamespaceTask : Gs2WebSocketSessionTask<Result.DeleteNamespaceResult>
        {
			private readonly Request.DeleteNamespaceRequest _request;

			public DeleteNamespaceTask(Request.DeleteNamespaceRequest request, UnityAction<AsyncResult<Result.DeleteNamespaceResult>> userCallback) : base(userCallback)
			{
				_request = request;
			}

            protected override IEnumerator ExecuteImpl(Gs2Session gs2Session)
            {
                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);

                jsonWriter.WriteObjectStart();

                if (_request.NamespaceName != null)
                {
                    jsonWriter.WritePropertyName("namespaceName");
                    jsonWriter.Write(_request.NamespaceName.ToString());
                }
                if (_request.ContextStack != null)
                {
                    jsonWriter.WritePropertyName("contextStack");
                    jsonWriter.Write(_request.ContextStack.ToString());
                }
                if (_request.RequestId != null)
                {
                    jsonWriter.WritePropertyName("xGs2RequestId");
                    jsonWriter.Write(_request.RequestId);
                }

                jsonWriter.WritePropertyName("xGs2ClientId");
                jsonWriter.Write(gs2Session.Credential.ClientId);
                jsonWriter.WritePropertyName("xGs2ProjectToken");
                jsonWriter.Write(gs2Session.ProjectToken);

                jsonWriter.WritePropertyName("x_gs2");
                jsonWriter.WriteObjectStart();
                jsonWriter.WritePropertyName("service");
                jsonWriter.Write("schedule");
                jsonWriter.WritePropertyName("component");
                jsonWriter.Write("namespace");
                jsonWriter.WritePropertyName("function");
                jsonWriter.Write("deleteNamespace");
                jsonWriter.WritePropertyName("contentType");
                jsonWriter.Write("application/json");
                jsonWriter.WritePropertyName("requestId");
                jsonWriter.Write(Gs2SessionTaskId.ToString());
                jsonWriter.WriteObjectEnd();

                jsonWriter.WriteObjectEnd();

                ((Gs2WebSocketSession)gs2Session).Send(stringBuilder.ToString());

                return new EmptyCoroutine();
            }
        }

		public IEnumerator DeleteNamespace(
                Request.DeleteNamespaceRequest request,
                UnityAction<AsyncResult<Result.DeleteNamespaceResult>> callback
        )
		{
			var task = new DeleteNamespaceTask(request, callback);
			return Gs2WebSocketSession.Execute(task);
        }

        private class CreateEventMasterTask : Gs2WebSocketSessionTask<Result.CreateEventMasterResult>
        {
			private readonly Request.CreateEventMasterRequest _request;

			public CreateEventMasterTask(Request.CreateEventMasterRequest request, UnityAction<AsyncResult<Result.CreateEventMasterResult>> userCallback) : base(userCallback)
			{
				_request = request;
			}

            protected override IEnumerator ExecuteImpl(Gs2Session gs2Session)
            {
                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);

                jsonWriter.WriteObjectStart();

                if (_request.NamespaceName != null)
                {
                    jsonWriter.WritePropertyName("namespaceName");
                    jsonWriter.Write(_request.NamespaceName.ToString());
                }
                if (_request.Name != null)
                {
                    jsonWriter.WritePropertyName("name");
                    jsonWriter.Write(_request.Name.ToString());
                }
                if (_request.Description != null)
                {
                    jsonWriter.WritePropertyName("description");
                    jsonWriter.Write(_request.Description.ToString());
                }
                if (_request.Metadata != null)
                {
                    jsonWriter.WritePropertyName("metadata");
                    jsonWriter.Write(_request.Metadata.ToString());
                }
                if (_request.ScheduleType != null)
                {
                    jsonWriter.WritePropertyName("scheduleType");
                    jsonWriter.Write(_request.ScheduleType.ToString());
                }
                if (_request.AbsoluteBegin != null)
                {
                    jsonWriter.WritePropertyName("absoluteBegin");
                    jsonWriter.Write(_request.AbsoluteBegin.ToString());
                }
                if (_request.AbsoluteEnd != null)
                {
                    jsonWriter.WritePropertyName("absoluteEnd");
                    jsonWriter.Write(_request.AbsoluteEnd.ToString());
                }
                if (_request.RepeatType != null)
                {
                    jsonWriter.WritePropertyName("repeatType");
                    jsonWriter.Write(_request.RepeatType.ToString());
                }
                if (_request.RepeatBeginDayOfMonth != null)
                {
                    jsonWriter.WritePropertyName("repeatBeginDayOfMonth");
                    jsonWriter.Write(_request.RepeatBeginDayOfMonth.ToString());
                }
                if (_request.RepeatEndDayOfMonth != null)
                {
                    jsonWriter.WritePropertyName("repeatEndDayOfMonth");
                    jsonWriter.Write(_request.RepeatEndDayOfMonth.ToString());
                }
                if (_request.RepeatBeginDayOfWeek != null)
                {
                    jsonWriter.WritePropertyName("repeatBeginDayOfWeek");
                    jsonWriter.Write(_request.RepeatBeginDayOfWeek.ToString());
                }
                if (_request.RepeatEndDayOfWeek != null)
                {
                    jsonWriter.WritePropertyName("repeatEndDayOfWeek");
                    jsonWriter.Write(_request.RepeatEndDayOfWeek.ToString());
                }
                if (_request.RepeatBeginHour != null)
                {
                    jsonWriter.WritePropertyName("repeatBeginHour");
                    jsonWriter.Write(_request.RepeatBeginHour.ToString());
                }
                if (_request.RepeatEndHour != null)
                {
                    jsonWriter.WritePropertyName("repeatEndHour");
                    jsonWriter.Write(_request.RepeatEndHour.ToString());
                }
                if (_request.RelativeTriggerName != null)
                {
                    jsonWriter.WritePropertyName("relativeTriggerName");
                    jsonWriter.Write(_request.RelativeTriggerName.ToString());
                }
                if (_request.RelativeDuration != null)
                {
                    jsonWriter.WritePropertyName("relativeDuration");
                    jsonWriter.Write(_request.RelativeDuration.ToString());
                }
                if (_request.ContextStack != null)
                {
                    jsonWriter.WritePropertyName("contextStack");
                    jsonWriter.Write(_request.ContextStack.ToString());
                }
                if (_request.RequestId != null)
                {
                    jsonWriter.WritePropertyName("xGs2RequestId");
                    jsonWriter.Write(_request.RequestId);
                }

                jsonWriter.WritePropertyName("xGs2ClientId");
                jsonWriter.Write(gs2Session.Credential.ClientId);
                jsonWriter.WritePropertyName("xGs2ProjectToken");
                jsonWriter.Write(gs2Session.ProjectToken);

                jsonWriter.WritePropertyName("x_gs2");
                jsonWriter.WriteObjectStart();
                jsonWriter.WritePropertyName("service");
                jsonWriter.Write("schedule");
                jsonWriter.WritePropertyName("component");
                jsonWriter.Write("eventMaster");
                jsonWriter.WritePropertyName("function");
                jsonWriter.Write("createEventMaster");
                jsonWriter.WritePropertyName("contentType");
                jsonWriter.Write("application/json");
                jsonWriter.WritePropertyName("requestId");
                jsonWriter.Write(Gs2SessionTaskId.ToString());
                jsonWriter.WriteObjectEnd();

                jsonWriter.WriteObjectEnd();

                ((Gs2WebSocketSession)gs2Session).Send(stringBuilder.ToString());

                return new EmptyCoroutine();
            }
        }

		public IEnumerator CreateEventMaster(
                Request.CreateEventMasterRequest request,
                UnityAction<AsyncResult<Result.CreateEventMasterResult>> callback
        )
		{
			var task = new CreateEventMasterTask(request, callback);
			return Gs2WebSocketSession.Execute(task);
        }

        private class GetEventMasterTask : Gs2WebSocketSessionTask<Result.GetEventMasterResult>
        {
			private readonly Request.GetEventMasterRequest _request;

			public GetEventMasterTask(Request.GetEventMasterRequest request, UnityAction<AsyncResult<Result.GetEventMasterResult>> userCallback) : base(userCallback)
			{
				_request = request;
			}

            protected override IEnumerator ExecuteImpl(Gs2Session gs2Session)
            {
                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);

                jsonWriter.WriteObjectStart();

                if (_request.NamespaceName != null)
                {
                    jsonWriter.WritePropertyName("namespaceName");
                    jsonWriter.Write(_request.NamespaceName.ToString());
                }
                if (_request.EventName != null)
                {
                    jsonWriter.WritePropertyName("eventName");
                    jsonWriter.Write(_request.EventName.ToString());
                }
                if (_request.ContextStack != null)
                {
                    jsonWriter.WritePropertyName("contextStack");
                    jsonWriter.Write(_request.ContextStack.ToString());
                }
                if (_request.RequestId != null)
                {
                    jsonWriter.WritePropertyName("xGs2RequestId");
                    jsonWriter.Write(_request.RequestId);
                }

                jsonWriter.WritePropertyName("xGs2ClientId");
                jsonWriter.Write(gs2Session.Credential.ClientId);
                jsonWriter.WritePropertyName("xGs2ProjectToken");
                jsonWriter.Write(gs2Session.ProjectToken);

                jsonWriter.WritePropertyName("x_gs2");
                jsonWriter.WriteObjectStart();
                jsonWriter.WritePropertyName("service");
                jsonWriter.Write("schedule");
                jsonWriter.WritePropertyName("component");
                jsonWriter.Write("eventMaster");
                jsonWriter.WritePropertyName("function");
                jsonWriter.Write("getEventMaster");
                jsonWriter.WritePropertyName("contentType");
                jsonWriter.Write("application/json");
                jsonWriter.WritePropertyName("requestId");
                jsonWriter.Write(Gs2SessionTaskId.ToString());
                jsonWriter.WriteObjectEnd();

                jsonWriter.WriteObjectEnd();

                ((Gs2WebSocketSession)gs2Session).Send(stringBuilder.ToString());

                return new EmptyCoroutine();
            }
        }

		public IEnumerator GetEventMaster(
                Request.GetEventMasterRequest request,
                UnityAction<AsyncResult<Result.GetEventMasterResult>> callback
        )
		{
			var task = new GetEventMasterTask(request, callback);
			return Gs2WebSocketSession.Execute(task);
        }

        private class UpdateEventMasterTask : Gs2WebSocketSessionTask<Result.UpdateEventMasterResult>
        {
			private readonly Request.UpdateEventMasterRequest _request;

			public UpdateEventMasterTask(Request.UpdateEventMasterRequest request, UnityAction<AsyncResult<Result.UpdateEventMasterResult>> userCallback) : base(userCallback)
			{
				_request = request;
			}

            protected override IEnumerator ExecuteImpl(Gs2Session gs2Session)
            {
                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);

                jsonWriter.WriteObjectStart();

                if (_request.NamespaceName != null)
                {
                    jsonWriter.WritePropertyName("namespaceName");
                    jsonWriter.Write(_request.NamespaceName.ToString());
                }
                if (_request.EventName != null)
                {
                    jsonWriter.WritePropertyName("eventName");
                    jsonWriter.Write(_request.EventName.ToString());
                }
                if (_request.Description != null)
                {
                    jsonWriter.WritePropertyName("description");
                    jsonWriter.Write(_request.Description.ToString());
                }
                if (_request.Metadata != null)
                {
                    jsonWriter.WritePropertyName("metadata");
                    jsonWriter.Write(_request.Metadata.ToString());
                }
                if (_request.ScheduleType != null)
                {
                    jsonWriter.WritePropertyName("scheduleType");
                    jsonWriter.Write(_request.ScheduleType.ToString());
                }
                if (_request.AbsoluteBegin != null)
                {
                    jsonWriter.WritePropertyName("absoluteBegin");
                    jsonWriter.Write(_request.AbsoluteBegin.ToString());
                }
                if (_request.AbsoluteEnd != null)
                {
                    jsonWriter.WritePropertyName("absoluteEnd");
                    jsonWriter.Write(_request.AbsoluteEnd.ToString());
                }
                if (_request.RepeatType != null)
                {
                    jsonWriter.WritePropertyName("repeatType");
                    jsonWriter.Write(_request.RepeatType.ToString());
                }
                if (_request.RepeatBeginDayOfMonth != null)
                {
                    jsonWriter.WritePropertyName("repeatBeginDayOfMonth");
                    jsonWriter.Write(_request.RepeatBeginDayOfMonth.ToString());
                }
                if (_request.RepeatEndDayOfMonth != null)
                {
                    jsonWriter.WritePropertyName("repeatEndDayOfMonth");
                    jsonWriter.Write(_request.RepeatEndDayOfMonth.ToString());
                }
                if (_request.RepeatBeginDayOfWeek != null)
                {
                    jsonWriter.WritePropertyName("repeatBeginDayOfWeek");
                    jsonWriter.Write(_request.RepeatBeginDayOfWeek.ToString());
                }
                if (_request.RepeatEndDayOfWeek != null)
                {
                    jsonWriter.WritePropertyName("repeatEndDayOfWeek");
                    jsonWriter.Write(_request.RepeatEndDayOfWeek.ToString());
                }
                if (_request.RepeatBeginHour != null)
                {
                    jsonWriter.WritePropertyName("repeatBeginHour");
                    jsonWriter.Write(_request.RepeatBeginHour.ToString());
                }
                if (_request.RepeatEndHour != null)
                {
                    jsonWriter.WritePropertyName("repeatEndHour");
                    jsonWriter.Write(_request.RepeatEndHour.ToString());
                }
                if (_request.RelativeTriggerName != null)
                {
                    jsonWriter.WritePropertyName("relativeTriggerName");
                    jsonWriter.Write(_request.RelativeTriggerName.ToString());
                }
                if (_request.RelativeDuration != null)
                {
                    jsonWriter.WritePropertyName("relativeDuration");
                    jsonWriter.Write(_request.RelativeDuration.ToString());
                }
                if (_request.ContextStack != null)
                {
                    jsonWriter.WritePropertyName("contextStack");
                    jsonWriter.Write(_request.ContextStack.ToString());
                }
                if (_request.RequestId != null)
                {
                    jsonWriter.WritePropertyName("xGs2RequestId");
                    jsonWriter.Write(_request.RequestId);
                }

                jsonWriter.WritePropertyName("xGs2ClientId");
                jsonWriter.Write(gs2Session.Credential.ClientId);
                jsonWriter.WritePropertyName("xGs2ProjectToken");
                jsonWriter.Write(gs2Session.ProjectToken);

                jsonWriter.WritePropertyName("x_gs2");
                jsonWriter.WriteObjectStart();
                jsonWriter.WritePropertyName("service");
                jsonWriter.Write("schedule");
                jsonWriter.WritePropertyName("component");
                jsonWriter.Write("eventMaster");
                jsonWriter.WritePropertyName("function");
                jsonWriter.Write("updateEventMaster");
                jsonWriter.WritePropertyName("contentType");
                jsonWriter.Write("application/json");
                jsonWriter.WritePropertyName("requestId");
                jsonWriter.Write(Gs2SessionTaskId.ToString());
                jsonWriter.WriteObjectEnd();

                jsonWriter.WriteObjectEnd();

                ((Gs2WebSocketSession)gs2Session).Send(stringBuilder.ToString());

                return new EmptyCoroutine();
            }
        }

		public IEnumerator UpdateEventMaster(
                Request.UpdateEventMasterRequest request,
                UnityAction<AsyncResult<Result.UpdateEventMasterResult>> callback
        )
		{
			var task = new UpdateEventMasterTask(request, callback);
			return Gs2WebSocketSession.Execute(task);
        }

        private class DeleteEventMasterTask : Gs2WebSocketSessionTask<Result.DeleteEventMasterResult>
        {
			private readonly Request.DeleteEventMasterRequest _request;

			public DeleteEventMasterTask(Request.DeleteEventMasterRequest request, UnityAction<AsyncResult<Result.DeleteEventMasterResult>> userCallback) : base(userCallback)
			{
				_request = request;
			}

            protected override IEnumerator ExecuteImpl(Gs2Session gs2Session)
            {
                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);

                jsonWriter.WriteObjectStart();

                if (_request.NamespaceName != null)
                {
                    jsonWriter.WritePropertyName("namespaceName");
                    jsonWriter.Write(_request.NamespaceName.ToString());
                }
                if (_request.EventName != null)
                {
                    jsonWriter.WritePropertyName("eventName");
                    jsonWriter.Write(_request.EventName.ToString());
                }
                if (_request.ContextStack != null)
                {
                    jsonWriter.WritePropertyName("contextStack");
                    jsonWriter.Write(_request.ContextStack.ToString());
                }
                if (_request.RequestId != null)
                {
                    jsonWriter.WritePropertyName("xGs2RequestId");
                    jsonWriter.Write(_request.RequestId);
                }

                jsonWriter.WritePropertyName("xGs2ClientId");
                jsonWriter.Write(gs2Session.Credential.ClientId);
                jsonWriter.WritePropertyName("xGs2ProjectToken");
                jsonWriter.Write(gs2Session.ProjectToken);

                jsonWriter.WritePropertyName("x_gs2");
                jsonWriter.WriteObjectStart();
                jsonWriter.WritePropertyName("service");
                jsonWriter.Write("schedule");
                jsonWriter.WritePropertyName("component");
                jsonWriter.Write("eventMaster");
                jsonWriter.WritePropertyName("function");
                jsonWriter.Write("deleteEventMaster");
                jsonWriter.WritePropertyName("contentType");
                jsonWriter.Write("application/json");
                jsonWriter.WritePropertyName("requestId");
                jsonWriter.Write(Gs2SessionTaskId.ToString());
                jsonWriter.WriteObjectEnd();

                jsonWriter.WriteObjectEnd();

                ((Gs2WebSocketSession)gs2Session).Send(stringBuilder.ToString());

                return new EmptyCoroutine();
            }
        }

		public IEnumerator DeleteEventMaster(
                Request.DeleteEventMasterRequest request,
                UnityAction<AsyncResult<Result.DeleteEventMasterResult>> callback
        )
		{
			var task = new DeleteEventMasterTask(request, callback);
			return Gs2WebSocketSession.Execute(task);
        }

        private class GetTriggerTask : Gs2WebSocketSessionTask<Result.GetTriggerResult>
        {
			private readonly Request.GetTriggerRequest _request;

			public GetTriggerTask(Request.GetTriggerRequest request, UnityAction<AsyncResult<Result.GetTriggerResult>> userCallback) : base(userCallback)
			{
				_request = request;
			}

            protected override IEnumerator ExecuteImpl(Gs2Session gs2Session)
            {
                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);

                jsonWriter.WriteObjectStart();

                if (_request.NamespaceName != null)
                {
                    jsonWriter.WritePropertyName("namespaceName");
                    jsonWriter.Write(_request.NamespaceName.ToString());
                }
                if (_request.AccessToken != null)
                {
                    jsonWriter.WritePropertyName("accessToken");
                    jsonWriter.Write(_request.AccessToken.ToString());
                }
                if (_request.TriggerName != null)
                {
                    jsonWriter.WritePropertyName("triggerName");
                    jsonWriter.Write(_request.TriggerName.ToString());
                }
                if (_request.ContextStack != null)
                {
                    jsonWriter.WritePropertyName("contextStack");
                    jsonWriter.Write(_request.ContextStack.ToString());
                }
                if (_request.RequestId != null)
                {
                    jsonWriter.WritePropertyName("xGs2RequestId");
                    jsonWriter.Write(_request.RequestId);
                }
                if (_request.AccessToken != null)
                {
                    jsonWriter.WritePropertyName("xGs2AccessToken");
                    jsonWriter.Write(_request.AccessToken);
                }

                jsonWriter.WritePropertyName("xGs2ClientId");
                jsonWriter.Write(gs2Session.Credential.ClientId);
                jsonWriter.WritePropertyName("xGs2ProjectToken");
                jsonWriter.Write(gs2Session.ProjectToken);

                jsonWriter.WritePropertyName("x_gs2");
                jsonWriter.WriteObjectStart();
                jsonWriter.WritePropertyName("service");
                jsonWriter.Write("schedule");
                jsonWriter.WritePropertyName("component");
                jsonWriter.Write("trigger");
                jsonWriter.WritePropertyName("function");
                jsonWriter.Write("getTrigger");
                jsonWriter.WritePropertyName("contentType");
                jsonWriter.Write("application/json");
                jsonWriter.WritePropertyName("requestId");
                jsonWriter.Write(Gs2SessionTaskId.ToString());
                jsonWriter.WriteObjectEnd();

                jsonWriter.WriteObjectEnd();

                ((Gs2WebSocketSession)gs2Session).Send(stringBuilder.ToString());

                return new EmptyCoroutine();
            }
        }

		public IEnumerator GetTrigger(
                Request.GetTriggerRequest request,
                UnityAction<AsyncResult<Result.GetTriggerResult>> callback
        )
		{
			var task = new GetTriggerTask(request, callback);
			return Gs2WebSocketSession.Execute(task);
        }

        private class GetTriggerByUserIdTask : Gs2WebSocketSessionTask<Result.GetTriggerByUserIdResult>
        {
			private readonly Request.GetTriggerByUserIdRequest _request;

			public GetTriggerByUserIdTask(Request.GetTriggerByUserIdRequest request, UnityAction<AsyncResult<Result.GetTriggerByUserIdResult>> userCallback) : base(userCallback)
			{
				_request = request;
			}

            protected override IEnumerator ExecuteImpl(Gs2Session gs2Session)
            {
                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);

                jsonWriter.WriteObjectStart();

                if (_request.NamespaceName != null)
                {
                    jsonWriter.WritePropertyName("namespaceName");
                    jsonWriter.Write(_request.NamespaceName.ToString());
                }
                if (_request.UserId != null)
                {
                    jsonWriter.WritePropertyName("userId");
                    jsonWriter.Write(_request.UserId.ToString());
                }
                if (_request.TriggerName != null)
                {
                    jsonWriter.WritePropertyName("triggerName");
                    jsonWriter.Write(_request.TriggerName.ToString());
                }
                if (_request.ContextStack != null)
                {
                    jsonWriter.WritePropertyName("contextStack");
                    jsonWriter.Write(_request.ContextStack.ToString());
                }
                if (_request.RequestId != null)
                {
                    jsonWriter.WritePropertyName("xGs2RequestId");
                    jsonWriter.Write(_request.RequestId);
                }

                jsonWriter.WritePropertyName("xGs2ClientId");
                jsonWriter.Write(gs2Session.Credential.ClientId);
                jsonWriter.WritePropertyName("xGs2ProjectToken");
                jsonWriter.Write(gs2Session.ProjectToken);

                jsonWriter.WritePropertyName("x_gs2");
                jsonWriter.WriteObjectStart();
                jsonWriter.WritePropertyName("service");
                jsonWriter.Write("schedule");
                jsonWriter.WritePropertyName("component");
                jsonWriter.Write("trigger");
                jsonWriter.WritePropertyName("function");
                jsonWriter.Write("getTriggerByUserId");
                jsonWriter.WritePropertyName("contentType");
                jsonWriter.Write("application/json");
                jsonWriter.WritePropertyName("requestId");
                jsonWriter.Write(Gs2SessionTaskId.ToString());
                jsonWriter.WriteObjectEnd();

                jsonWriter.WriteObjectEnd();

                ((Gs2WebSocketSession)gs2Session).Send(stringBuilder.ToString());

                return new EmptyCoroutine();
            }
        }

		public IEnumerator GetTriggerByUserId(
                Request.GetTriggerByUserIdRequest request,
                UnityAction<AsyncResult<Result.GetTriggerByUserIdResult>> callback
        )
		{
			var task = new GetTriggerByUserIdTask(request, callback);
			return Gs2WebSocketSession.Execute(task);
        }

        private class TriggerByUserIdTask : Gs2WebSocketSessionTask<Result.TriggerByUserIdResult>
        {
			private readonly Request.TriggerByUserIdRequest _request;

			public TriggerByUserIdTask(Request.TriggerByUserIdRequest request, UnityAction<AsyncResult<Result.TriggerByUserIdResult>> userCallback) : base(userCallback)
			{
				_request = request;
			}

            protected override IEnumerator ExecuteImpl(Gs2Session gs2Session)
            {
                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);

                jsonWriter.WriteObjectStart();

                if (_request.NamespaceName != null)
                {
                    jsonWriter.WritePropertyName("namespaceName");
                    jsonWriter.Write(_request.NamespaceName.ToString());
                }
                if (_request.TriggerName != null)
                {
                    jsonWriter.WritePropertyName("triggerName");
                    jsonWriter.Write(_request.TriggerName.ToString());
                }
                if (_request.UserId != null)
                {
                    jsonWriter.WritePropertyName("userId");
                    jsonWriter.Write(_request.UserId.ToString());
                }
                if (_request.TriggerStrategy != null)
                {
                    jsonWriter.WritePropertyName("triggerStrategy");
                    jsonWriter.Write(_request.TriggerStrategy.ToString());
                }
                if (_request.Ttl != null)
                {
                    jsonWriter.WritePropertyName("ttl");
                    jsonWriter.Write(_request.Ttl.ToString());
                }
                if (_request.ContextStack != null)
                {
                    jsonWriter.WritePropertyName("contextStack");
                    jsonWriter.Write(_request.ContextStack.ToString());
                }
                if (_request.RequestId != null)
                {
                    jsonWriter.WritePropertyName("xGs2RequestId");
                    jsonWriter.Write(_request.RequestId);
                }

                jsonWriter.WritePropertyName("xGs2ClientId");
                jsonWriter.Write(gs2Session.Credential.ClientId);
                jsonWriter.WritePropertyName("xGs2ProjectToken");
                jsonWriter.Write(gs2Session.ProjectToken);

                jsonWriter.WritePropertyName("x_gs2");
                jsonWriter.WriteObjectStart();
                jsonWriter.WritePropertyName("service");
                jsonWriter.Write("schedule");
                jsonWriter.WritePropertyName("component");
                jsonWriter.Write("trigger");
                jsonWriter.WritePropertyName("function");
                jsonWriter.Write("triggerByUserId");
                jsonWriter.WritePropertyName("contentType");
                jsonWriter.Write("application/json");
                jsonWriter.WritePropertyName("requestId");
                jsonWriter.Write(Gs2SessionTaskId.ToString());
                jsonWriter.WriteObjectEnd();

                jsonWriter.WriteObjectEnd();

                ((Gs2WebSocketSession)gs2Session).Send(stringBuilder.ToString());

                return new EmptyCoroutine();
            }
        }

		public IEnumerator TriggerByUserId(
                Request.TriggerByUserIdRequest request,
                UnityAction<AsyncResult<Result.TriggerByUserIdResult>> callback
        )
		{
			var task = new TriggerByUserIdTask(request, callback);
			return Gs2WebSocketSession.Execute(task);
        }

        private class DeleteTriggerTask : Gs2WebSocketSessionTask<Result.DeleteTriggerResult>
        {
			private readonly Request.DeleteTriggerRequest _request;

			public DeleteTriggerTask(Request.DeleteTriggerRequest request, UnityAction<AsyncResult<Result.DeleteTriggerResult>> userCallback) : base(userCallback)
			{
				_request = request;
			}

            protected override IEnumerator ExecuteImpl(Gs2Session gs2Session)
            {
                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);

                jsonWriter.WriteObjectStart();

                if (_request.NamespaceName != null)
                {
                    jsonWriter.WritePropertyName("namespaceName");
                    jsonWriter.Write(_request.NamespaceName.ToString());
                }
                if (_request.AccessToken != null)
                {
                    jsonWriter.WritePropertyName("accessToken");
                    jsonWriter.Write(_request.AccessToken.ToString());
                }
                if (_request.TriggerName != null)
                {
                    jsonWriter.WritePropertyName("triggerName");
                    jsonWriter.Write(_request.TriggerName.ToString());
                }
                if (_request.ContextStack != null)
                {
                    jsonWriter.WritePropertyName("contextStack");
                    jsonWriter.Write(_request.ContextStack.ToString());
                }
                if (_request.RequestId != null)
                {
                    jsonWriter.WritePropertyName("xGs2RequestId");
                    jsonWriter.Write(_request.RequestId);
                }
                if (_request.AccessToken != null)
                {
                    jsonWriter.WritePropertyName("xGs2AccessToken");
                    jsonWriter.Write(_request.AccessToken);
                }

                jsonWriter.WritePropertyName("xGs2ClientId");
                jsonWriter.Write(gs2Session.Credential.ClientId);
                jsonWriter.WritePropertyName("xGs2ProjectToken");
                jsonWriter.Write(gs2Session.ProjectToken);

                jsonWriter.WritePropertyName("x_gs2");
                jsonWriter.WriteObjectStart();
                jsonWriter.WritePropertyName("service");
                jsonWriter.Write("schedule");
                jsonWriter.WritePropertyName("component");
                jsonWriter.Write("trigger");
                jsonWriter.WritePropertyName("function");
                jsonWriter.Write("deleteTrigger");
                jsonWriter.WritePropertyName("contentType");
                jsonWriter.Write("application/json");
                jsonWriter.WritePropertyName("requestId");
                jsonWriter.Write(Gs2SessionTaskId.ToString());
                jsonWriter.WriteObjectEnd();

                jsonWriter.WriteObjectEnd();

                ((Gs2WebSocketSession)gs2Session).Send(stringBuilder.ToString());

                return new EmptyCoroutine();
            }
        }

		public IEnumerator DeleteTrigger(
                Request.DeleteTriggerRequest request,
                UnityAction<AsyncResult<Result.DeleteTriggerResult>> callback
        )
		{
			var task = new DeleteTriggerTask(request, callback);
			return Gs2WebSocketSession.Execute(task);
        }

        private class DeleteTriggerByUserIdTask : Gs2WebSocketSessionTask<Result.DeleteTriggerByUserIdResult>
        {
			private readonly Request.DeleteTriggerByUserIdRequest _request;

			public DeleteTriggerByUserIdTask(Request.DeleteTriggerByUserIdRequest request, UnityAction<AsyncResult<Result.DeleteTriggerByUserIdResult>> userCallback) : base(userCallback)
			{
				_request = request;
			}

            protected override IEnumerator ExecuteImpl(Gs2Session gs2Session)
            {
                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);

                jsonWriter.WriteObjectStart();

                if (_request.NamespaceName != null)
                {
                    jsonWriter.WritePropertyName("namespaceName");
                    jsonWriter.Write(_request.NamespaceName.ToString());
                }
                if (_request.UserId != null)
                {
                    jsonWriter.WritePropertyName("userId");
                    jsonWriter.Write(_request.UserId.ToString());
                }
                if (_request.TriggerName != null)
                {
                    jsonWriter.WritePropertyName("triggerName");
                    jsonWriter.Write(_request.TriggerName.ToString());
                }
                if (_request.ContextStack != null)
                {
                    jsonWriter.WritePropertyName("contextStack");
                    jsonWriter.Write(_request.ContextStack.ToString());
                }
                if (_request.RequestId != null)
                {
                    jsonWriter.WritePropertyName("xGs2RequestId");
                    jsonWriter.Write(_request.RequestId);
                }

                jsonWriter.WritePropertyName("xGs2ClientId");
                jsonWriter.Write(gs2Session.Credential.ClientId);
                jsonWriter.WritePropertyName("xGs2ProjectToken");
                jsonWriter.Write(gs2Session.ProjectToken);

                jsonWriter.WritePropertyName("x_gs2");
                jsonWriter.WriteObjectStart();
                jsonWriter.WritePropertyName("service");
                jsonWriter.Write("schedule");
                jsonWriter.WritePropertyName("component");
                jsonWriter.Write("trigger");
                jsonWriter.WritePropertyName("function");
                jsonWriter.Write("deleteTriggerByUserId");
                jsonWriter.WritePropertyName("contentType");
                jsonWriter.Write("application/json");
                jsonWriter.WritePropertyName("requestId");
                jsonWriter.Write(Gs2SessionTaskId.ToString());
                jsonWriter.WriteObjectEnd();

                jsonWriter.WriteObjectEnd();

                ((Gs2WebSocketSession)gs2Session).Send(stringBuilder.ToString());

                return new EmptyCoroutine();
            }
        }

		public IEnumerator DeleteTriggerByUserId(
                Request.DeleteTriggerByUserIdRequest request,
                UnityAction<AsyncResult<Result.DeleteTriggerByUserIdResult>> callback
        )
		{
			var task = new DeleteTriggerByUserIdTask(request, callback);
			return Gs2WebSocketSession.Execute(task);
        }

        private class GetEventTask : Gs2WebSocketSessionTask<Result.GetEventResult>
        {
			private readonly Request.GetEventRequest _request;

			public GetEventTask(Request.GetEventRequest request, UnityAction<AsyncResult<Result.GetEventResult>> userCallback) : base(userCallback)
			{
				_request = request;
			}

            protected override IEnumerator ExecuteImpl(Gs2Session gs2Session)
            {
                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);

                jsonWriter.WriteObjectStart();

                if (_request.NamespaceName != null)
                {
                    jsonWriter.WritePropertyName("namespaceName");
                    jsonWriter.Write(_request.NamespaceName.ToString());
                }
                if (_request.EventName != null)
                {
                    jsonWriter.WritePropertyName("eventName");
                    jsonWriter.Write(_request.EventName.ToString());
                }
                if (_request.AccessToken != null)
                {
                    jsonWriter.WritePropertyName("accessToken");
                    jsonWriter.Write(_request.AccessToken.ToString());
                }
                if (_request.ContextStack != null)
                {
                    jsonWriter.WritePropertyName("contextStack");
                    jsonWriter.Write(_request.ContextStack.ToString());
                }
                if (_request.RequestId != null)
                {
                    jsonWriter.WritePropertyName("xGs2RequestId");
                    jsonWriter.Write(_request.RequestId);
                }
                if (_request.AccessToken != null)
                {
                    jsonWriter.WritePropertyName("xGs2AccessToken");
                    jsonWriter.Write(_request.AccessToken);
                }

                jsonWriter.WritePropertyName("xGs2ClientId");
                jsonWriter.Write(gs2Session.Credential.ClientId);
                jsonWriter.WritePropertyName("xGs2ProjectToken");
                jsonWriter.Write(gs2Session.ProjectToken);

                jsonWriter.WritePropertyName("x_gs2");
                jsonWriter.WriteObjectStart();
                jsonWriter.WritePropertyName("service");
                jsonWriter.Write("schedule");
                jsonWriter.WritePropertyName("component");
                jsonWriter.Write("event");
                jsonWriter.WritePropertyName("function");
                jsonWriter.Write("getEvent");
                jsonWriter.WritePropertyName("contentType");
                jsonWriter.Write("application/json");
                jsonWriter.WritePropertyName("requestId");
                jsonWriter.Write(Gs2SessionTaskId.ToString());
                jsonWriter.WriteObjectEnd();

                jsonWriter.WriteObjectEnd();

                ((Gs2WebSocketSession)gs2Session).Send(stringBuilder.ToString());

                return new EmptyCoroutine();
            }
        }

		public IEnumerator GetEvent(
                Request.GetEventRequest request,
                UnityAction<AsyncResult<Result.GetEventResult>> callback
        )
		{
			var task = new GetEventTask(request, callback);
			return Gs2WebSocketSession.Execute(task);
        }

        private class GetEventByUserIdTask : Gs2WebSocketSessionTask<Result.GetEventByUserIdResult>
        {
			private readonly Request.GetEventByUserIdRequest _request;

			public GetEventByUserIdTask(Request.GetEventByUserIdRequest request, UnityAction<AsyncResult<Result.GetEventByUserIdResult>> userCallback) : base(userCallback)
			{
				_request = request;
			}

            protected override IEnumerator ExecuteImpl(Gs2Session gs2Session)
            {
                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);

                jsonWriter.WriteObjectStart();

                if (_request.NamespaceName != null)
                {
                    jsonWriter.WritePropertyName("namespaceName");
                    jsonWriter.Write(_request.NamespaceName.ToString());
                }
                if (_request.EventName != null)
                {
                    jsonWriter.WritePropertyName("eventName");
                    jsonWriter.Write(_request.EventName.ToString());
                }
                if (_request.UserId != null)
                {
                    jsonWriter.WritePropertyName("userId");
                    jsonWriter.Write(_request.UserId.ToString());
                }
                if (_request.ContextStack != null)
                {
                    jsonWriter.WritePropertyName("contextStack");
                    jsonWriter.Write(_request.ContextStack.ToString());
                }
                if (_request.RequestId != null)
                {
                    jsonWriter.WritePropertyName("xGs2RequestId");
                    jsonWriter.Write(_request.RequestId);
                }

                jsonWriter.WritePropertyName("xGs2ClientId");
                jsonWriter.Write(gs2Session.Credential.ClientId);
                jsonWriter.WritePropertyName("xGs2ProjectToken");
                jsonWriter.Write(gs2Session.ProjectToken);

                jsonWriter.WritePropertyName("x_gs2");
                jsonWriter.WriteObjectStart();
                jsonWriter.WritePropertyName("service");
                jsonWriter.Write("schedule");
                jsonWriter.WritePropertyName("component");
                jsonWriter.Write("event");
                jsonWriter.WritePropertyName("function");
                jsonWriter.Write("getEventByUserId");
                jsonWriter.WritePropertyName("contentType");
                jsonWriter.Write("application/json");
                jsonWriter.WritePropertyName("requestId");
                jsonWriter.Write(Gs2SessionTaskId.ToString());
                jsonWriter.WriteObjectEnd();

                jsonWriter.WriteObjectEnd();

                ((Gs2WebSocketSession)gs2Session).Send(stringBuilder.ToString());

                return new EmptyCoroutine();
            }
        }

		public IEnumerator GetEventByUserId(
                Request.GetEventByUserIdRequest request,
                UnityAction<AsyncResult<Result.GetEventByUserIdResult>> callback
        )
		{
			var task = new GetEventByUserIdTask(request, callback);
			return Gs2WebSocketSession.Execute(task);
        }

        private class GetRawEventTask : Gs2WebSocketSessionTask<Result.GetRawEventResult>
        {
			private readonly Request.GetRawEventRequest _request;

			public GetRawEventTask(Request.GetRawEventRequest request, UnityAction<AsyncResult<Result.GetRawEventResult>> userCallback) : base(userCallback)
			{
				_request = request;
			}

            protected override IEnumerator ExecuteImpl(Gs2Session gs2Session)
            {
                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);

                jsonWriter.WriteObjectStart();

                if (_request.NamespaceName != null)
                {
                    jsonWriter.WritePropertyName("namespaceName");
                    jsonWriter.Write(_request.NamespaceName.ToString());
                }
                if (_request.EventName != null)
                {
                    jsonWriter.WritePropertyName("eventName");
                    jsonWriter.Write(_request.EventName.ToString());
                }
                if (_request.ContextStack != null)
                {
                    jsonWriter.WritePropertyName("contextStack");
                    jsonWriter.Write(_request.ContextStack.ToString());
                }
                if (_request.RequestId != null)
                {
                    jsonWriter.WritePropertyName("xGs2RequestId");
                    jsonWriter.Write(_request.RequestId);
                }

                jsonWriter.WritePropertyName("xGs2ClientId");
                jsonWriter.Write(gs2Session.Credential.ClientId);
                jsonWriter.WritePropertyName("xGs2ProjectToken");
                jsonWriter.Write(gs2Session.ProjectToken);

                jsonWriter.WritePropertyName("x_gs2");
                jsonWriter.WriteObjectStart();
                jsonWriter.WritePropertyName("service");
                jsonWriter.Write("schedule");
                jsonWriter.WritePropertyName("component");
                jsonWriter.Write("event");
                jsonWriter.WritePropertyName("function");
                jsonWriter.Write("getRawEvent");
                jsonWriter.WritePropertyName("contentType");
                jsonWriter.Write("application/json");
                jsonWriter.WritePropertyName("requestId");
                jsonWriter.Write(Gs2SessionTaskId.ToString());
                jsonWriter.WriteObjectEnd();

                jsonWriter.WriteObjectEnd();

                ((Gs2WebSocketSession)gs2Session).Send(stringBuilder.ToString());

                return new EmptyCoroutine();
            }
        }

		public IEnumerator GetRawEvent(
                Request.GetRawEventRequest request,
                UnityAction<AsyncResult<Result.GetRawEventResult>> callback
        )
		{
			var task = new GetRawEventTask(request, callback);
			return Gs2WebSocketSession.Execute(task);
        }
	}
}