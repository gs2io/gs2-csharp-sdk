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
	public class Gs2ScheduleRestClient : AbstractGs2Client
	{
		private readonly CertificateHandler _certificateHandler;

		public static string Endpoint = "schedule";

        protected Gs2RestSession Gs2RestSession => (Gs2RestSession) Gs2Session;

		/// <summary>
		/// コンストラクタ。
		/// </summary>
		/// <param name="Gs2RestSession">REST API 用セッション</param>
		public Gs2ScheduleRestClient(Gs2RestSession Gs2RestSession) : base(Gs2RestSession)
		{

		}

		/// <summary>
		/// コンストラクタ。
		/// </summary>
		/// <param name="gs2RestSession">REST API 用セッション</param>
		/// <param name="certificateHandler"></param>
		public Gs2ScheduleRestClient(Gs2RestSession gs2RestSession, CertificateHandler certificateHandler) : base(gs2RestSession)
		{
			_certificateHandler = certificateHandler;
		}

        private class DescribeNamespacesTask : Gs2RestSessionTask<Result.DescribeNamespacesResult>
        {
			private readonly Request.DescribeNamespacesRequest _request;

			public DescribeNamespacesTask(Request.DescribeNamespacesRequest request, UnityAction<AsyncResult<Result.DescribeNamespacesResult>> userCallback) : base(userCallback)
			{
				_request = request;
			}

            protected override IEnumerator ExecuteImpl(Gs2Session gs2Session)
            {
				UnityWebRequest.method = UnityWebRequest.kHttpVerbGET;

                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "schedule")
                    .Replace("{region}", gs2Session.Region.DisplayName())
                    + "/";

                var queryStrings = new List<string> ();
                if (_request.ContextStack != null)
                {
                    queryStrings.Add(string.Format("{0}={1}", "contextStack", UnityWebRequest.EscapeURL(_request.ContextStack)));
                }
                if (_request.PageToken != null) {
                    queryStrings.Add(string.Format("{0}={1}", "pageToken", UnityWebRequest.EscapeURL(_request.PageToken)));
                }
                if (_request.Limit != null) {
                    queryStrings.Add(string.Format("{0}={1}", "limit", _request.Limit));
                }
                url += "?" + string.Join("&", queryStrings.ToArray());

                UnityWebRequest.url = url;

                if (_request.RequestId != null)
                {
                    UnityWebRequest.SetRequestHeader("X-GS2-REQUEST-ID", _request.RequestId);
                }

                return Send((Gs2RestSession)gs2Session);
            }
        }

		/// <summary>
		/// <returns>IEnumerator</returns>
		/// <param name="callback">コールバックハンドラ</param>
		/// <param name="request">リクエストパラメータ</param>
		public IEnumerator DescribeNamespaces(
                Request.DescribeNamespacesRequest request,
                UnityAction<AsyncResult<Result.DescribeNamespacesResult>> callback
        )
		{
			var task = new DescribeNamespacesTask(request, callback);
			if (_certificateHandler != null)
			{
				task.UnityWebRequest.certificateHandler = _certificateHandler;
			}
			return Gs2RestSession.Execute(task);
        }

        private class CreateNamespaceTask : Gs2RestSessionTask<Result.CreateNamespaceResult>
        {
			private readonly Request.CreateNamespaceRequest _request;

			public CreateNamespaceTask(Request.CreateNamespaceRequest request, UnityAction<AsyncResult<Result.CreateNamespaceResult>> userCallback) : base(userCallback)
			{
				_request = request;
			}

            protected override IEnumerator ExecuteImpl(Gs2Session gs2Session)
            {
				UnityWebRequest.method = UnityWebRequest.kHttpVerbPOST;

                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "schedule")
                    .Replace("{region}", gs2Session.Region.DisplayName())
                    + "/";

                UnityWebRequest.url = url;

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
                jsonWriter.WriteObjectEnd();

                var body = stringBuilder.ToString();
                if (!string.IsNullOrEmpty(body))
                {
                    UnityWebRequest.uploadHandler = new UploadHandlerRaw(Encoding.UTF8.GetBytes(body));
                }
                UnityWebRequest.SetRequestHeader("Content-Type", "application/json");

                if (_request.RequestId != null)
                {
                    UnityWebRequest.SetRequestHeader("X-GS2-REQUEST-ID", _request.RequestId);
                }

                return Send((Gs2RestSession)gs2Session);
            }
        }

		/// <summary>
		/// <returns>IEnumerator</returns>
		/// <param name="callback">コールバックハンドラ</param>
		/// <param name="request">リクエストパラメータ</param>
		public IEnumerator CreateNamespace(
                Request.CreateNamespaceRequest request,
                UnityAction<AsyncResult<Result.CreateNamespaceResult>> callback
        )
		{
			var task = new CreateNamespaceTask(request, callback);
			if (_certificateHandler != null)
			{
				task.UnityWebRequest.certificateHandler = _certificateHandler;
			}
			return Gs2RestSession.Execute(task);
        }

        private class GetNamespaceStatusTask : Gs2RestSessionTask<Result.GetNamespaceStatusResult>
        {
			private readonly Request.GetNamespaceStatusRequest _request;

			public GetNamespaceStatusTask(Request.GetNamespaceStatusRequest request, UnityAction<AsyncResult<Result.GetNamespaceStatusResult>> userCallback) : base(userCallback)
			{
				_request = request;
			}

            protected override IEnumerator ExecuteImpl(Gs2Session gs2Session)
            {
				UnityWebRequest.method = UnityWebRequest.kHttpVerbGET;

                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "schedule")
                    .Replace("{region}", gs2Session.Region.DisplayName())
                    + "/{namespaceName}/status";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(_request.NamespaceName) ? _request.NamespaceName.ToString() : "null");

                var queryStrings = new List<string> ();
                if (_request.ContextStack != null)
                {
                    queryStrings.Add(string.Format("{0}={1}", "contextStack", UnityWebRequest.EscapeURL(_request.ContextStack)));
                }
                url += "?" + string.Join("&", queryStrings.ToArray());

                UnityWebRequest.url = url;

                if (_request.RequestId != null)
                {
                    UnityWebRequest.SetRequestHeader("X-GS2-REQUEST-ID", _request.RequestId);
                }

                return Send((Gs2RestSession)gs2Session);
            }
        }

		/// <summary>
		/// <returns>IEnumerator</returns>
		/// <param name="callback">コールバックハンドラ</param>
		/// <param name="request">リクエストパラメータ</param>
		public IEnumerator GetNamespaceStatus(
                Request.GetNamespaceStatusRequest request,
                UnityAction<AsyncResult<Result.GetNamespaceStatusResult>> callback
        )
		{
			var task = new GetNamespaceStatusTask(request, callback);
			if (_certificateHandler != null)
			{
				task.UnityWebRequest.certificateHandler = _certificateHandler;
			}
			return Gs2RestSession.Execute(task);
        }

        private class GetNamespaceTask : Gs2RestSessionTask<Result.GetNamespaceResult>
        {
			private readonly Request.GetNamespaceRequest _request;

			public GetNamespaceTask(Request.GetNamespaceRequest request, UnityAction<AsyncResult<Result.GetNamespaceResult>> userCallback) : base(userCallback)
			{
				_request = request;
			}

            protected override IEnumerator ExecuteImpl(Gs2Session gs2Session)
            {
				UnityWebRequest.method = UnityWebRequest.kHttpVerbGET;

                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "schedule")
                    .Replace("{region}", gs2Session.Region.DisplayName())
                    + "/{namespaceName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(_request.NamespaceName) ? _request.NamespaceName.ToString() : "null");

                var queryStrings = new List<string> ();
                if (_request.ContextStack != null)
                {
                    queryStrings.Add(string.Format("{0}={1}", "contextStack", UnityWebRequest.EscapeURL(_request.ContextStack)));
                }
                url += "?" + string.Join("&", queryStrings.ToArray());

                UnityWebRequest.url = url;

                if (_request.RequestId != null)
                {
                    UnityWebRequest.SetRequestHeader("X-GS2-REQUEST-ID", _request.RequestId);
                }

                return Send((Gs2RestSession)gs2Session);
            }
        }

		/// <summary>
		/// <returns>IEnumerator</returns>
		/// <param name="callback">コールバックハンドラ</param>
		/// <param name="request">リクエストパラメータ</param>
		public IEnumerator GetNamespace(
                Request.GetNamespaceRequest request,
                UnityAction<AsyncResult<Result.GetNamespaceResult>> callback
        )
		{
			var task = new GetNamespaceTask(request, callback);
			if (_certificateHandler != null)
			{
				task.UnityWebRequest.certificateHandler = _certificateHandler;
			}
			return Gs2RestSession.Execute(task);
        }

        private class UpdateNamespaceTask : Gs2RestSessionTask<Result.UpdateNamespaceResult>
        {
			private readonly Request.UpdateNamespaceRequest _request;

			public UpdateNamespaceTask(Request.UpdateNamespaceRequest request, UnityAction<AsyncResult<Result.UpdateNamespaceResult>> userCallback) : base(userCallback)
			{
				_request = request;
			}

            protected override IEnumerator ExecuteImpl(Gs2Session gs2Session)
            {
				UnityWebRequest.method = UnityWebRequest.kHttpVerbPUT;

                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "schedule")
                    .Replace("{region}", gs2Session.Region.DisplayName())
                    + "/{namespaceName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(_request.NamespaceName) ? _request.NamespaceName.ToString() : "null");

                UnityWebRequest.url = url;

                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);
                jsonWriter.WriteObjectStart();
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
                jsonWriter.WriteObjectEnd();

                var body = stringBuilder.ToString();
                if (!string.IsNullOrEmpty(body))
                {
                    UnityWebRequest.uploadHandler = new UploadHandlerRaw(Encoding.UTF8.GetBytes(body));
                }
                UnityWebRequest.SetRequestHeader("Content-Type", "application/json");

                if (_request.RequestId != null)
                {
                    UnityWebRequest.SetRequestHeader("X-GS2-REQUEST-ID", _request.RequestId);
                }

                return Send((Gs2RestSession)gs2Session);
            }
        }

		/// <summary>
		/// <returns>IEnumerator</returns>
		/// <param name="callback">コールバックハンドラ</param>
		/// <param name="request">リクエストパラメータ</param>
		public IEnumerator UpdateNamespace(
                Request.UpdateNamespaceRequest request,
                UnityAction<AsyncResult<Result.UpdateNamespaceResult>> callback
        )
		{
			var task = new UpdateNamespaceTask(request, callback);
			if (_certificateHandler != null)
			{
				task.UnityWebRequest.certificateHandler = _certificateHandler;
			}
			return Gs2RestSession.Execute(task);
        }

        private class DeleteNamespaceTask : Gs2RestSessionTask<Result.DeleteNamespaceResult>
        {
			private readonly Request.DeleteNamespaceRequest _request;

			public DeleteNamespaceTask(Request.DeleteNamespaceRequest request, UnityAction<AsyncResult<Result.DeleteNamespaceResult>> userCallback) : base(userCallback)
			{
				_request = request;
			}

            protected override IEnumerator ExecuteImpl(Gs2Session gs2Session)
            {
				UnityWebRequest.method = UnityWebRequest.kHttpVerbDELETE;

                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "schedule")
                    .Replace("{region}", gs2Session.Region.DisplayName())
                    + "/{namespaceName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(_request.NamespaceName) ? _request.NamespaceName.ToString() : "null");

                var queryStrings = new List<string> ();
                if (_request.ContextStack != null)
                {
                    queryStrings.Add(string.Format("{0}={1}", "contextStack", UnityWebRequest.EscapeURL(_request.ContextStack)));
                }
                url += "?" + string.Join("&", queryStrings.ToArray());

                UnityWebRequest.url = url;

                if (_request.RequestId != null)
                {
                    UnityWebRequest.SetRequestHeader("X-GS2-REQUEST-ID", _request.RequestId);
                }

                return Send((Gs2RestSession)gs2Session);
            }
        }

		/// <summary>
		/// <returns>IEnumerator</returns>
		/// <param name="callback">コールバックハンドラ</param>
		/// <param name="request">リクエストパラメータ</param>
		public IEnumerator DeleteNamespace(
                Request.DeleteNamespaceRequest request,
                UnityAction<AsyncResult<Result.DeleteNamespaceResult>> callback
        )
		{
			var task = new DeleteNamespaceTask(request, callback);
			if (_certificateHandler != null)
			{
				task.UnityWebRequest.certificateHandler = _certificateHandler;
			}
			return Gs2RestSession.Execute(task);
        }

        private class DescribeEventMastersTask : Gs2RestSessionTask<Result.DescribeEventMastersResult>
        {
			private readonly Request.DescribeEventMastersRequest _request;

			public DescribeEventMastersTask(Request.DescribeEventMastersRequest request, UnityAction<AsyncResult<Result.DescribeEventMastersResult>> userCallback) : base(userCallback)
			{
				_request = request;
			}

            protected override IEnumerator ExecuteImpl(Gs2Session gs2Session)
            {
				UnityWebRequest.method = UnityWebRequest.kHttpVerbGET;

                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "schedule")
                    .Replace("{region}", gs2Session.Region.DisplayName())
                    + "/{namespaceName}/master/event";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(_request.NamespaceName) ? _request.NamespaceName.ToString() : "null");

                var queryStrings = new List<string> ();
                if (_request.ContextStack != null)
                {
                    queryStrings.Add(string.Format("{0}={1}", "contextStack", UnityWebRequest.EscapeURL(_request.ContextStack)));
                }
                if (_request.PageToken != null) {
                    queryStrings.Add(string.Format("{0}={1}", "pageToken", UnityWebRequest.EscapeURL(_request.PageToken)));
                }
                if (_request.Limit != null) {
                    queryStrings.Add(string.Format("{0}={1}", "limit", _request.Limit));
                }
                url += "?" + string.Join("&", queryStrings.ToArray());

                UnityWebRequest.url = url;

                if (_request.RequestId != null)
                {
                    UnityWebRequest.SetRequestHeader("X-GS2-REQUEST-ID", _request.RequestId);
                }

                return Send((Gs2RestSession)gs2Session);
            }
        }

		/// <summary>
		/// <returns>IEnumerator</returns>
		/// <param name="callback">コールバックハンドラ</param>
		/// <param name="request">リクエストパラメータ</param>
		public IEnumerator DescribeEventMasters(
                Request.DescribeEventMastersRequest request,
                UnityAction<AsyncResult<Result.DescribeEventMastersResult>> callback
        )
		{
			var task = new DescribeEventMastersTask(request, callback);
			if (_certificateHandler != null)
			{
				task.UnityWebRequest.certificateHandler = _certificateHandler;
			}
			return Gs2RestSession.Execute(task);
        }

        private class CreateEventMasterTask : Gs2RestSessionTask<Result.CreateEventMasterResult>
        {
			private readonly Request.CreateEventMasterRequest _request;

			public CreateEventMasterTask(Request.CreateEventMasterRequest request, UnityAction<AsyncResult<Result.CreateEventMasterResult>> userCallback) : base(userCallback)
			{
				_request = request;
			}

            protected override IEnumerator ExecuteImpl(Gs2Session gs2Session)
            {
				UnityWebRequest.method = UnityWebRequest.kHttpVerbPOST;

                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "schedule")
                    .Replace("{region}", gs2Session.Region.DisplayName())
                    + "/{namespaceName}/master/event";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(_request.NamespaceName) ? _request.NamespaceName.ToString() : "null");

                UnityWebRequest.url = url;

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
                jsonWriter.WriteObjectEnd();

                var body = stringBuilder.ToString();
                if (!string.IsNullOrEmpty(body))
                {
                    UnityWebRequest.uploadHandler = new UploadHandlerRaw(Encoding.UTF8.GetBytes(body));
                }
                UnityWebRequest.SetRequestHeader("Content-Type", "application/json");

                if (_request.RequestId != null)
                {
                    UnityWebRequest.SetRequestHeader("X-GS2-REQUEST-ID", _request.RequestId);
                }

                return Send((Gs2RestSession)gs2Session);
            }
        }

		/// <summary>
		/// <returns>IEnumerator</returns>
		/// <param name="callback">コールバックハンドラ</param>
		/// <param name="request">リクエストパラメータ</param>
		public IEnumerator CreateEventMaster(
                Request.CreateEventMasterRequest request,
                UnityAction<AsyncResult<Result.CreateEventMasterResult>> callback
        )
		{
			var task = new CreateEventMasterTask(request, callback);
			if (_certificateHandler != null)
			{
				task.UnityWebRequest.certificateHandler = _certificateHandler;
			}
			return Gs2RestSession.Execute(task);
        }

        private class GetEventMasterTask : Gs2RestSessionTask<Result.GetEventMasterResult>
        {
			private readonly Request.GetEventMasterRequest _request;

			public GetEventMasterTask(Request.GetEventMasterRequest request, UnityAction<AsyncResult<Result.GetEventMasterResult>> userCallback) : base(userCallback)
			{
				_request = request;
			}

            protected override IEnumerator ExecuteImpl(Gs2Session gs2Session)
            {
				UnityWebRequest.method = UnityWebRequest.kHttpVerbGET;

                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "schedule")
                    .Replace("{region}", gs2Session.Region.DisplayName())
                    + "/{namespaceName}/master/event/{eventName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(_request.NamespaceName) ? _request.NamespaceName.ToString() : "null");
                url = url.Replace("{eventName}", !string.IsNullOrEmpty(_request.EventName) ? _request.EventName.ToString() : "null");

                var queryStrings = new List<string> ();
                if (_request.ContextStack != null)
                {
                    queryStrings.Add(string.Format("{0}={1}", "contextStack", UnityWebRequest.EscapeURL(_request.ContextStack)));
                }
                url += "?" + string.Join("&", queryStrings.ToArray());

                UnityWebRequest.url = url;

                if (_request.RequestId != null)
                {
                    UnityWebRequest.SetRequestHeader("X-GS2-REQUEST-ID", _request.RequestId);
                }

                return Send((Gs2RestSession)gs2Session);
            }
        }

		/// <summary>
		/// <returns>IEnumerator</returns>
		/// <param name="callback">コールバックハンドラ</param>
		/// <param name="request">リクエストパラメータ</param>
		public IEnumerator GetEventMaster(
                Request.GetEventMasterRequest request,
                UnityAction<AsyncResult<Result.GetEventMasterResult>> callback
        )
		{
			var task = new GetEventMasterTask(request, callback);
			if (_certificateHandler != null)
			{
				task.UnityWebRequest.certificateHandler = _certificateHandler;
			}
			return Gs2RestSession.Execute(task);
        }

        private class UpdateEventMasterTask : Gs2RestSessionTask<Result.UpdateEventMasterResult>
        {
			private readonly Request.UpdateEventMasterRequest _request;

			public UpdateEventMasterTask(Request.UpdateEventMasterRequest request, UnityAction<AsyncResult<Result.UpdateEventMasterResult>> userCallback) : base(userCallback)
			{
				_request = request;
			}

            protected override IEnumerator ExecuteImpl(Gs2Session gs2Session)
            {
				UnityWebRequest.method = UnityWebRequest.kHttpVerbPUT;

                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "schedule")
                    .Replace("{region}", gs2Session.Region.DisplayName())
                    + "/{namespaceName}/master/event/{eventName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(_request.NamespaceName) ? _request.NamespaceName.ToString() : "null");
                url = url.Replace("{eventName}", !string.IsNullOrEmpty(_request.EventName) ? _request.EventName.ToString() : "null");

                UnityWebRequest.url = url;

                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);
                jsonWriter.WriteObjectStart();
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
                jsonWriter.WriteObjectEnd();

                var body = stringBuilder.ToString();
                if (!string.IsNullOrEmpty(body))
                {
                    UnityWebRequest.uploadHandler = new UploadHandlerRaw(Encoding.UTF8.GetBytes(body));
                }
                UnityWebRequest.SetRequestHeader("Content-Type", "application/json");

                if (_request.RequestId != null)
                {
                    UnityWebRequest.SetRequestHeader("X-GS2-REQUEST-ID", _request.RequestId);
                }

                return Send((Gs2RestSession)gs2Session);
            }
        }

		/// <summary>
		/// <returns>IEnumerator</returns>
		/// <param name="callback">コールバックハンドラ</param>
		/// <param name="request">リクエストパラメータ</param>
		public IEnumerator UpdateEventMaster(
                Request.UpdateEventMasterRequest request,
                UnityAction<AsyncResult<Result.UpdateEventMasterResult>> callback
        )
		{
			var task = new UpdateEventMasterTask(request, callback);
			if (_certificateHandler != null)
			{
				task.UnityWebRequest.certificateHandler = _certificateHandler;
			}
			return Gs2RestSession.Execute(task);
        }

        private class DeleteEventMasterTask : Gs2RestSessionTask<Result.DeleteEventMasterResult>
        {
			private readonly Request.DeleteEventMasterRequest _request;

			public DeleteEventMasterTask(Request.DeleteEventMasterRequest request, UnityAction<AsyncResult<Result.DeleteEventMasterResult>> userCallback) : base(userCallback)
			{
				_request = request;
			}

            protected override IEnumerator ExecuteImpl(Gs2Session gs2Session)
            {
				UnityWebRequest.method = UnityWebRequest.kHttpVerbDELETE;

                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "schedule")
                    .Replace("{region}", gs2Session.Region.DisplayName())
                    + "/{namespaceName}/master/event/{eventName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(_request.NamespaceName) ? _request.NamespaceName.ToString() : "null");
                url = url.Replace("{eventName}", !string.IsNullOrEmpty(_request.EventName) ? _request.EventName.ToString() : "null");

                var queryStrings = new List<string> ();
                if (_request.ContextStack != null)
                {
                    queryStrings.Add(string.Format("{0}={1}", "contextStack", UnityWebRequest.EscapeURL(_request.ContextStack)));
                }
                url += "?" + string.Join("&", queryStrings.ToArray());

                UnityWebRequest.url = url;

                if (_request.RequestId != null)
                {
                    UnityWebRequest.SetRequestHeader("X-GS2-REQUEST-ID", _request.RequestId);
                }

                return Send((Gs2RestSession)gs2Session);
            }
        }

		/// <summary>
		/// <returns>IEnumerator</returns>
		/// <param name="callback">コールバックハンドラ</param>
		/// <param name="request">リクエストパラメータ</param>
		public IEnumerator DeleteEventMaster(
                Request.DeleteEventMasterRequest request,
                UnityAction<AsyncResult<Result.DeleteEventMasterResult>> callback
        )
		{
			var task = new DeleteEventMasterTask(request, callback);
			if (_certificateHandler != null)
			{
				task.UnityWebRequest.certificateHandler = _certificateHandler;
			}
			return Gs2RestSession.Execute(task);
        }

        private class DescribeTriggersTask : Gs2RestSessionTask<Result.DescribeTriggersResult>
        {
			private readonly Request.DescribeTriggersRequest _request;

			public DescribeTriggersTask(Request.DescribeTriggersRequest request, UnityAction<AsyncResult<Result.DescribeTriggersResult>> userCallback) : base(userCallback)
			{
				_request = request;
			}

            protected override IEnumerator ExecuteImpl(Gs2Session gs2Session)
            {
				UnityWebRequest.method = UnityWebRequest.kHttpVerbGET;

                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "schedule")
                    .Replace("{region}", gs2Session.Region.DisplayName())
                    + "/{namespaceName}/user/me/trigger";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(_request.NamespaceName) ? _request.NamespaceName.ToString() : "null");

                var queryStrings = new List<string> ();
                if (_request.ContextStack != null)
                {
                    queryStrings.Add(string.Format("{0}={1}", "contextStack", UnityWebRequest.EscapeURL(_request.ContextStack)));
                }
                if (_request.PageToken != null) {
                    queryStrings.Add(string.Format("{0}={1}", "pageToken", UnityWebRequest.EscapeURL(_request.PageToken)));
                }
                if (_request.Limit != null) {
                    queryStrings.Add(string.Format("{0}={1}", "limit", _request.Limit));
                }
                url += "?" + string.Join("&", queryStrings.ToArray());

                UnityWebRequest.url = url;

                if (_request.RequestId != null)
                {
                    UnityWebRequest.SetRequestHeader("X-GS2-REQUEST-ID", _request.RequestId);
                }
                if (_request.AccessToken != null)
                {
                    UnityWebRequest.SetRequestHeader("X-GS2-ACCESS-TOKEN", _request.AccessToken);
                }

                return Send((Gs2RestSession)gs2Session);
            }
        }

		/// <summary>
		/// <returns>IEnumerator</returns>
		/// <param name="callback">コールバックハンドラ</param>
		/// <param name="request">リクエストパラメータ</param>
		public IEnumerator DescribeTriggers(
                Request.DescribeTriggersRequest request,
                UnityAction<AsyncResult<Result.DescribeTriggersResult>> callback
        )
		{
			var task = new DescribeTriggersTask(request, callback);
			if (_certificateHandler != null)
			{
				task.UnityWebRequest.certificateHandler = _certificateHandler;
			}
			return Gs2RestSession.Execute(task);
        }

        private class DescribeTriggersByUserIdTask : Gs2RestSessionTask<Result.DescribeTriggersByUserIdResult>
        {
			private readonly Request.DescribeTriggersByUserIdRequest _request;

			public DescribeTriggersByUserIdTask(Request.DescribeTriggersByUserIdRequest request, UnityAction<AsyncResult<Result.DescribeTriggersByUserIdResult>> userCallback) : base(userCallback)
			{
				_request = request;
			}

            protected override IEnumerator ExecuteImpl(Gs2Session gs2Session)
            {
				UnityWebRequest.method = UnityWebRequest.kHttpVerbGET;

                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "schedule")
                    .Replace("{region}", gs2Session.Region.DisplayName())
                    + "/{namespaceName}/user/{userId}/trigger";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(_request.NamespaceName) ? _request.NamespaceName.ToString() : "null");
                url = url.Replace("{userId}", !string.IsNullOrEmpty(_request.UserId) ? _request.UserId.ToString() : "null");

                var queryStrings = new List<string> ();
                if (_request.ContextStack != null)
                {
                    queryStrings.Add(string.Format("{0}={1}", "contextStack", UnityWebRequest.EscapeURL(_request.ContextStack)));
                }
                if (_request.PageToken != null) {
                    queryStrings.Add(string.Format("{0}={1}", "pageToken", UnityWebRequest.EscapeURL(_request.PageToken)));
                }
                if (_request.Limit != null) {
                    queryStrings.Add(string.Format("{0}={1}", "limit", _request.Limit));
                }
                url += "?" + string.Join("&", queryStrings.ToArray());

                UnityWebRequest.url = url;

                if (_request.RequestId != null)
                {
                    UnityWebRequest.SetRequestHeader("X-GS2-REQUEST-ID", _request.RequestId);
                }

                return Send((Gs2RestSession)gs2Session);
            }
        }

		/// <summary>
		/// <returns>IEnumerator</returns>
		/// <param name="callback">コールバックハンドラ</param>
		/// <param name="request">リクエストパラメータ</param>
		public IEnumerator DescribeTriggersByUserId(
                Request.DescribeTriggersByUserIdRequest request,
                UnityAction<AsyncResult<Result.DescribeTriggersByUserIdResult>> callback
        )
		{
			var task = new DescribeTriggersByUserIdTask(request, callback);
			if (_certificateHandler != null)
			{
				task.UnityWebRequest.certificateHandler = _certificateHandler;
			}
			return Gs2RestSession.Execute(task);
        }

        private class GetTriggerTask : Gs2RestSessionTask<Result.GetTriggerResult>
        {
			private readonly Request.GetTriggerRequest _request;

			public GetTriggerTask(Request.GetTriggerRequest request, UnityAction<AsyncResult<Result.GetTriggerResult>> userCallback) : base(userCallback)
			{
				_request = request;
			}

            protected override IEnumerator ExecuteImpl(Gs2Session gs2Session)
            {
				UnityWebRequest.method = UnityWebRequest.kHttpVerbGET;

                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "schedule")
                    .Replace("{region}", gs2Session.Region.DisplayName())
                    + "/{namespaceName}/user/me/trigger/{triggerName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(_request.NamespaceName) ? _request.NamespaceName.ToString() : "null");
                url = url.Replace("{triggerName}", !string.IsNullOrEmpty(_request.TriggerName) ? _request.TriggerName.ToString() : "null");

                var queryStrings = new List<string> ();
                if (_request.ContextStack != null)
                {
                    queryStrings.Add(string.Format("{0}={1}", "contextStack", UnityWebRequest.EscapeURL(_request.ContextStack)));
                }
                url += "?" + string.Join("&", queryStrings.ToArray());

                UnityWebRequest.url = url;

                if (_request.RequestId != null)
                {
                    UnityWebRequest.SetRequestHeader("X-GS2-REQUEST-ID", _request.RequestId);
                }
                if (_request.AccessToken != null)
                {
                    UnityWebRequest.SetRequestHeader("X-GS2-ACCESS-TOKEN", _request.AccessToken);
                }

                return Send((Gs2RestSession)gs2Session);
            }
        }

		/// <summary>
		/// <returns>IEnumerator</returns>
		/// <param name="callback">コールバックハンドラ</param>
		/// <param name="request">リクエストパラメータ</param>
		public IEnumerator GetTrigger(
                Request.GetTriggerRequest request,
                UnityAction<AsyncResult<Result.GetTriggerResult>> callback
        )
		{
			var task = new GetTriggerTask(request, callback);
			if (_certificateHandler != null)
			{
				task.UnityWebRequest.certificateHandler = _certificateHandler;
			}
			return Gs2RestSession.Execute(task);
        }

        private class GetTriggerByUserIdTask : Gs2RestSessionTask<Result.GetTriggerByUserIdResult>
        {
			private readonly Request.GetTriggerByUserIdRequest _request;

			public GetTriggerByUserIdTask(Request.GetTriggerByUserIdRequest request, UnityAction<AsyncResult<Result.GetTriggerByUserIdResult>> userCallback) : base(userCallback)
			{
				_request = request;
			}

            protected override IEnumerator ExecuteImpl(Gs2Session gs2Session)
            {
				UnityWebRequest.method = UnityWebRequest.kHttpVerbGET;

                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "schedule")
                    .Replace("{region}", gs2Session.Region.DisplayName())
                    + "/{namespaceName}/user/{userId}/trigger/{triggerName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(_request.NamespaceName) ? _request.NamespaceName.ToString() : "null");
                url = url.Replace("{userId}", !string.IsNullOrEmpty(_request.UserId) ? _request.UserId.ToString() : "null");
                url = url.Replace("{triggerName}", !string.IsNullOrEmpty(_request.TriggerName) ? _request.TriggerName.ToString() : "null");

                var queryStrings = new List<string> ();
                if (_request.ContextStack != null)
                {
                    queryStrings.Add(string.Format("{0}={1}", "contextStack", UnityWebRequest.EscapeURL(_request.ContextStack)));
                }
                url += "?" + string.Join("&", queryStrings.ToArray());

                UnityWebRequest.url = url;

                if (_request.RequestId != null)
                {
                    UnityWebRequest.SetRequestHeader("X-GS2-REQUEST-ID", _request.RequestId);
                }

                return Send((Gs2RestSession)gs2Session);
            }
        }

		/// <summary>
		/// <returns>IEnumerator</returns>
		/// <param name="callback">コールバックハンドラ</param>
		/// <param name="request">リクエストパラメータ</param>
		public IEnumerator GetTriggerByUserId(
                Request.GetTriggerByUserIdRequest request,
                UnityAction<AsyncResult<Result.GetTriggerByUserIdResult>> callback
        )
		{
			var task = new GetTriggerByUserIdTask(request, callback);
			if (_certificateHandler != null)
			{
				task.UnityWebRequest.certificateHandler = _certificateHandler;
			}
			return Gs2RestSession.Execute(task);
        }

        private class TriggerByUserIdTask : Gs2RestSessionTask<Result.TriggerByUserIdResult>
        {
			private readonly Request.TriggerByUserIdRequest _request;

			public TriggerByUserIdTask(Request.TriggerByUserIdRequest request, UnityAction<AsyncResult<Result.TriggerByUserIdResult>> userCallback) : base(userCallback)
			{
				_request = request;
			}

            protected override IEnumerator ExecuteImpl(Gs2Session gs2Session)
            {
				UnityWebRequest.method = UnityWebRequest.kHttpVerbPOST;

                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "schedule")
                    .Replace("{region}", gs2Session.Region.DisplayName())
                    + "/{namespaceName}/user/{userId}/trigger/{triggerName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(_request.NamespaceName) ? _request.NamespaceName.ToString() : "null");
                url = url.Replace("{triggerName}", !string.IsNullOrEmpty(_request.TriggerName) ? _request.TriggerName.ToString() : "null");
                url = url.Replace("{userId}", !string.IsNullOrEmpty(_request.UserId) ? _request.UserId.ToString() : "null");

                UnityWebRequest.url = url;

                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);
                jsonWriter.WriteObjectStart();
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
                jsonWriter.WriteObjectEnd();

                var body = stringBuilder.ToString();
                if (!string.IsNullOrEmpty(body))
                {
                    UnityWebRequest.uploadHandler = new UploadHandlerRaw(Encoding.UTF8.GetBytes(body));
                }
                UnityWebRequest.SetRequestHeader("Content-Type", "application/json");

                if (_request.RequestId != null)
                {
                    UnityWebRequest.SetRequestHeader("X-GS2-REQUEST-ID", _request.RequestId);
                }

                return Send((Gs2RestSession)gs2Session);
            }
        }

		/// <summary>
		/// <returns>IEnumerator</returns>
		/// <param name="callback">コールバックハンドラ</param>
		/// <param name="request">リクエストパラメータ</param>
		public IEnumerator TriggerByUserId(
                Request.TriggerByUserIdRequest request,
                UnityAction<AsyncResult<Result.TriggerByUserIdResult>> callback
        )
		{
			var task = new TriggerByUserIdTask(request, callback);
			if (_certificateHandler != null)
			{
				task.UnityWebRequest.certificateHandler = _certificateHandler;
			}
			return Gs2RestSession.Execute(task);
        }

        private class DeleteTriggerTask : Gs2RestSessionTask<Result.DeleteTriggerResult>
        {
			private readonly Request.DeleteTriggerRequest _request;

			public DeleteTriggerTask(Request.DeleteTriggerRequest request, UnityAction<AsyncResult<Result.DeleteTriggerResult>> userCallback) : base(userCallback)
			{
				_request = request;
			}

            protected override IEnumerator ExecuteImpl(Gs2Session gs2Session)
            {
				UnityWebRequest.method = UnityWebRequest.kHttpVerbDELETE;

                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "schedule")
                    .Replace("{region}", gs2Session.Region.DisplayName())
                    + "/{namespaceName}/user/me/trigger/{triggerName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(_request.NamespaceName) ? _request.NamespaceName.ToString() : "null");
                url = url.Replace("{triggerName}", !string.IsNullOrEmpty(_request.TriggerName) ? _request.TriggerName.ToString() : "null");

                var queryStrings = new List<string> ();
                if (_request.ContextStack != null)
                {
                    queryStrings.Add(string.Format("{0}={1}", "contextStack", UnityWebRequest.EscapeURL(_request.ContextStack)));
                }
                url += "?" + string.Join("&", queryStrings.ToArray());

                UnityWebRequest.url = url;

                if (_request.RequestId != null)
                {
                    UnityWebRequest.SetRequestHeader("X-GS2-REQUEST-ID", _request.RequestId);
                }
                if (_request.AccessToken != null)
                {
                    UnityWebRequest.SetRequestHeader("X-GS2-ACCESS-TOKEN", _request.AccessToken);
                }

                return Send((Gs2RestSession)gs2Session);
            }
        }

		/// <summary>
		/// <returns>IEnumerator</returns>
		/// <param name="callback">コールバックハンドラ</param>
		/// <param name="request">リクエストパラメータ</param>
		public IEnumerator DeleteTrigger(
                Request.DeleteTriggerRequest request,
                UnityAction<AsyncResult<Result.DeleteTriggerResult>> callback
        )
		{
			var task = new DeleteTriggerTask(request, callback);
			if (_certificateHandler != null)
			{
				task.UnityWebRequest.certificateHandler = _certificateHandler;
			}
			return Gs2RestSession.Execute(task);
        }

        private class DeleteTriggerByUserIdTask : Gs2RestSessionTask<Result.DeleteTriggerByUserIdResult>
        {
			private readonly Request.DeleteTriggerByUserIdRequest _request;

			public DeleteTriggerByUserIdTask(Request.DeleteTriggerByUserIdRequest request, UnityAction<AsyncResult<Result.DeleteTriggerByUserIdResult>> userCallback) : base(userCallback)
			{
				_request = request;
			}

            protected override IEnumerator ExecuteImpl(Gs2Session gs2Session)
            {
				UnityWebRequest.method = UnityWebRequest.kHttpVerbDELETE;

                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "schedule")
                    .Replace("{region}", gs2Session.Region.DisplayName())
                    + "/{namespaceName}/user/{userId}/trigger/{triggerName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(_request.NamespaceName) ? _request.NamespaceName.ToString() : "null");
                url = url.Replace("{userId}", !string.IsNullOrEmpty(_request.UserId) ? _request.UserId.ToString() : "null");
                url = url.Replace("{triggerName}", !string.IsNullOrEmpty(_request.TriggerName) ? _request.TriggerName.ToString() : "null");

                var queryStrings = new List<string> ();
                if (_request.ContextStack != null)
                {
                    queryStrings.Add(string.Format("{0}={1}", "contextStack", UnityWebRequest.EscapeURL(_request.ContextStack)));
                }
                url += "?" + string.Join("&", queryStrings.ToArray());

                UnityWebRequest.url = url;

                if (_request.RequestId != null)
                {
                    UnityWebRequest.SetRequestHeader("X-GS2-REQUEST-ID", _request.RequestId);
                }

                return Send((Gs2RestSession)gs2Session);
            }
        }

		/// <summary>
		/// <returns>IEnumerator</returns>
		/// <param name="callback">コールバックハンドラ</param>
		/// <param name="request">リクエストパラメータ</param>
		public IEnumerator DeleteTriggerByUserId(
                Request.DeleteTriggerByUserIdRequest request,
                UnityAction<AsyncResult<Result.DeleteTriggerByUserIdResult>> callback
        )
		{
			var task = new DeleteTriggerByUserIdTask(request, callback);
			if (_certificateHandler != null)
			{
				task.UnityWebRequest.certificateHandler = _certificateHandler;
			}
			return Gs2RestSession.Execute(task);
        }

        private class DescribeEventsTask : Gs2RestSessionTask<Result.DescribeEventsResult>
        {
			private readonly Request.DescribeEventsRequest _request;

			public DescribeEventsTask(Request.DescribeEventsRequest request, UnityAction<AsyncResult<Result.DescribeEventsResult>> userCallback) : base(userCallback)
			{
				_request = request;
			}

            protected override IEnumerator ExecuteImpl(Gs2Session gs2Session)
            {
				UnityWebRequest.method = UnityWebRequest.kHttpVerbGET;

                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "schedule")
                    .Replace("{region}", gs2Session.Region.DisplayName())
                    + "/{namespaceName}/user/me/event";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(_request.NamespaceName) ? _request.NamespaceName.ToString() : "null");

                var queryStrings = new List<string> ();
                if (_request.ContextStack != null)
                {
                    queryStrings.Add(string.Format("{0}={1}", "contextStack", UnityWebRequest.EscapeURL(_request.ContextStack)));
                }
                url += "?" + string.Join("&", queryStrings.ToArray());

                UnityWebRequest.url = url;

                if (_request.RequestId != null)
                {
                    UnityWebRequest.SetRequestHeader("X-GS2-REQUEST-ID", _request.RequestId);
                }
                if (_request.AccessToken != null)
                {
                    UnityWebRequest.SetRequestHeader("X-GS2-ACCESS-TOKEN", _request.AccessToken);
                }

                return Send((Gs2RestSession)gs2Session);
            }
        }

		/// <summary>
		/// <returns>IEnumerator</returns>
		/// <param name="callback">コールバックハンドラ</param>
		/// <param name="request">リクエストパラメータ</param>
		public IEnumerator DescribeEvents(
                Request.DescribeEventsRequest request,
                UnityAction<AsyncResult<Result.DescribeEventsResult>> callback
        )
		{
			var task = new DescribeEventsTask(request, callback);
			if (_certificateHandler != null)
			{
				task.UnityWebRequest.certificateHandler = _certificateHandler;
			}
			return Gs2RestSession.Execute(task);
        }

        private class DescribeEventsByUserIdTask : Gs2RestSessionTask<Result.DescribeEventsByUserIdResult>
        {
			private readonly Request.DescribeEventsByUserIdRequest _request;

			public DescribeEventsByUserIdTask(Request.DescribeEventsByUserIdRequest request, UnityAction<AsyncResult<Result.DescribeEventsByUserIdResult>> userCallback) : base(userCallback)
			{
				_request = request;
			}

            protected override IEnumerator ExecuteImpl(Gs2Session gs2Session)
            {
				UnityWebRequest.method = UnityWebRequest.kHttpVerbGET;

                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "schedule")
                    .Replace("{region}", gs2Session.Region.DisplayName())
                    + "/{namespaceName}/user/{userId}/event";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(_request.NamespaceName) ? _request.NamespaceName.ToString() : "null");
                url = url.Replace("{userId}", !string.IsNullOrEmpty(_request.UserId) ? _request.UserId.ToString() : "null");

                var queryStrings = new List<string> ();
                if (_request.ContextStack != null)
                {
                    queryStrings.Add(string.Format("{0}={1}", "contextStack", UnityWebRequest.EscapeURL(_request.ContextStack)));
                }
                url += "?" + string.Join("&", queryStrings.ToArray());

                UnityWebRequest.url = url;

                if (_request.RequestId != null)
                {
                    UnityWebRequest.SetRequestHeader("X-GS2-REQUEST-ID", _request.RequestId);
                }

                return Send((Gs2RestSession)gs2Session);
            }
        }

		/// <summary>
		/// <returns>IEnumerator</returns>
		/// <param name="callback">コールバックハンドラ</param>
		/// <param name="request">リクエストパラメータ</param>
		public IEnumerator DescribeEventsByUserId(
                Request.DescribeEventsByUserIdRequest request,
                UnityAction<AsyncResult<Result.DescribeEventsByUserIdResult>> callback
        )
		{
			var task = new DescribeEventsByUserIdTask(request, callback);
			if (_certificateHandler != null)
			{
				task.UnityWebRequest.certificateHandler = _certificateHandler;
			}
			return Gs2RestSession.Execute(task);
        }

        private class DescribeRawEventsTask : Gs2RestSessionTask<Result.DescribeRawEventsResult>
        {
			private readonly Request.DescribeRawEventsRequest _request;

			public DescribeRawEventsTask(Request.DescribeRawEventsRequest request, UnityAction<AsyncResult<Result.DescribeRawEventsResult>> userCallback) : base(userCallback)
			{
				_request = request;
			}

            protected override IEnumerator ExecuteImpl(Gs2Session gs2Session)
            {
				UnityWebRequest.method = UnityWebRequest.kHttpVerbGET;

                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "schedule")
                    .Replace("{region}", gs2Session.Region.DisplayName())
                    + "/{namespaceName}/event";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(_request.NamespaceName) ? _request.NamespaceName.ToString() : "null");

                var queryStrings = new List<string> ();
                if (_request.ContextStack != null)
                {
                    queryStrings.Add(string.Format("{0}={1}", "contextStack", UnityWebRequest.EscapeURL(_request.ContextStack)));
                }
                url += "?" + string.Join("&", queryStrings.ToArray());

                UnityWebRequest.url = url;

                if (_request.RequestId != null)
                {
                    UnityWebRequest.SetRequestHeader("X-GS2-REQUEST-ID", _request.RequestId);
                }

                return Send((Gs2RestSession)gs2Session);
            }
        }

		/// <summary>
		/// <returns>IEnumerator</returns>
		/// <param name="callback">コールバックハンドラ</param>
		/// <param name="request">リクエストパラメータ</param>
		public IEnumerator DescribeRawEvents(
                Request.DescribeRawEventsRequest request,
                UnityAction<AsyncResult<Result.DescribeRawEventsResult>> callback
        )
		{
			var task = new DescribeRawEventsTask(request, callback);
			if (_certificateHandler != null)
			{
				task.UnityWebRequest.certificateHandler = _certificateHandler;
			}
			return Gs2RestSession.Execute(task);
        }

        private class GetEventTask : Gs2RestSessionTask<Result.GetEventResult>
        {
			private readonly Request.GetEventRequest _request;

			public GetEventTask(Request.GetEventRequest request, UnityAction<AsyncResult<Result.GetEventResult>> userCallback) : base(userCallback)
			{
				_request = request;
			}

            protected override IEnumerator ExecuteImpl(Gs2Session gs2Session)
            {
				UnityWebRequest.method = UnityWebRequest.kHttpVerbGET;

                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "schedule")
                    .Replace("{region}", gs2Session.Region.DisplayName())
                    + "/{namespaceName}/user/me/event/{eventName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(_request.NamespaceName) ? _request.NamespaceName.ToString() : "null");
                url = url.Replace("{eventName}", !string.IsNullOrEmpty(_request.EventName) ? _request.EventName.ToString() : "null");

                var queryStrings = new List<string> ();
                if (_request.ContextStack != null)
                {
                    queryStrings.Add(string.Format("{0}={1}", "contextStack", UnityWebRequest.EscapeURL(_request.ContextStack)));
                }
                url += "?" + string.Join("&", queryStrings.ToArray());

                UnityWebRequest.url = url;

                if (_request.RequestId != null)
                {
                    UnityWebRequest.SetRequestHeader("X-GS2-REQUEST-ID", _request.RequestId);
                }
                if (_request.AccessToken != null)
                {
                    UnityWebRequest.SetRequestHeader("X-GS2-ACCESS-TOKEN", _request.AccessToken);
                }

                return Send((Gs2RestSession)gs2Session);
            }
        }

		/// <summary>
		/// <returns>IEnumerator</returns>
		/// <param name="callback">コールバックハンドラ</param>
		/// <param name="request">リクエストパラメータ</param>
		public IEnumerator GetEvent(
                Request.GetEventRequest request,
                UnityAction<AsyncResult<Result.GetEventResult>> callback
        )
		{
			var task = new GetEventTask(request, callback);
			if (_certificateHandler != null)
			{
				task.UnityWebRequest.certificateHandler = _certificateHandler;
			}
			return Gs2RestSession.Execute(task);
        }

        private class GetEventByUserIdTask : Gs2RestSessionTask<Result.GetEventByUserIdResult>
        {
			private readonly Request.GetEventByUserIdRequest _request;

			public GetEventByUserIdTask(Request.GetEventByUserIdRequest request, UnityAction<AsyncResult<Result.GetEventByUserIdResult>> userCallback) : base(userCallback)
			{
				_request = request;
			}

            protected override IEnumerator ExecuteImpl(Gs2Session gs2Session)
            {
				UnityWebRequest.method = UnityWebRequest.kHttpVerbGET;

                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "schedule")
                    .Replace("{region}", gs2Session.Region.DisplayName())
                    + "/{namespaceName}/user/{userId}/event/{eventName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(_request.NamespaceName) ? _request.NamespaceName.ToString() : "null");
                url = url.Replace("{eventName}", !string.IsNullOrEmpty(_request.EventName) ? _request.EventName.ToString() : "null");
                url = url.Replace("{userId}", !string.IsNullOrEmpty(_request.UserId) ? _request.UserId.ToString() : "null");

                var queryStrings = new List<string> ();
                if (_request.ContextStack != null)
                {
                    queryStrings.Add(string.Format("{0}={1}", "contextStack", UnityWebRequest.EscapeURL(_request.ContextStack)));
                }
                url += "?" + string.Join("&", queryStrings.ToArray());

                UnityWebRequest.url = url;

                if (_request.RequestId != null)
                {
                    UnityWebRequest.SetRequestHeader("X-GS2-REQUEST-ID", _request.RequestId);
                }

                return Send((Gs2RestSession)gs2Session);
            }
        }

		/// <summary>
		/// <returns>IEnumerator</returns>
		/// <param name="callback">コールバックハンドラ</param>
		/// <param name="request">リクエストパラメータ</param>
		public IEnumerator GetEventByUserId(
                Request.GetEventByUserIdRequest request,
                UnityAction<AsyncResult<Result.GetEventByUserIdResult>> callback
        )
		{
			var task = new GetEventByUserIdTask(request, callback);
			if (_certificateHandler != null)
			{
				task.UnityWebRequest.certificateHandler = _certificateHandler;
			}
			return Gs2RestSession.Execute(task);
        }

        private class GetRawEventTask : Gs2RestSessionTask<Result.GetRawEventResult>
        {
			private readonly Request.GetRawEventRequest _request;

			public GetRawEventTask(Request.GetRawEventRequest request, UnityAction<AsyncResult<Result.GetRawEventResult>> userCallback) : base(userCallback)
			{
				_request = request;
			}

            protected override IEnumerator ExecuteImpl(Gs2Session gs2Session)
            {
				UnityWebRequest.method = UnityWebRequest.kHttpVerbGET;

                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "schedule")
                    .Replace("{region}", gs2Session.Region.DisplayName())
                    + "/{namespaceName}/event/{eventName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(_request.NamespaceName) ? _request.NamespaceName.ToString() : "null");
                url = url.Replace("{eventName}", !string.IsNullOrEmpty(_request.EventName) ? _request.EventName.ToString() : "null");

                var queryStrings = new List<string> ();
                if (_request.ContextStack != null)
                {
                    queryStrings.Add(string.Format("{0}={1}", "contextStack", UnityWebRequest.EscapeURL(_request.ContextStack)));
                }
                url += "?" + string.Join("&", queryStrings.ToArray());

                UnityWebRequest.url = url;

                if (_request.RequestId != null)
                {
                    UnityWebRequest.SetRequestHeader("X-GS2-REQUEST-ID", _request.RequestId);
                }

                return Send((Gs2RestSession)gs2Session);
            }
        }

		/// <summary>
		/// <returns>IEnumerator</returns>
		/// <param name="callback">コールバックハンドラ</param>
		/// <param name="request">リクエストパラメータ</param>
		public IEnumerator GetRawEvent(
                Request.GetRawEventRequest request,
                UnityAction<AsyncResult<Result.GetRawEventResult>> callback
        )
		{
			var task = new GetRawEventTask(request, callback);
			if (_certificateHandler != null)
			{
				task.UnityWebRequest.certificateHandler = _certificateHandler;
			}
			return Gs2RestSession.Execute(task);
        }

        private class ExportMasterTask : Gs2RestSessionTask<Result.ExportMasterResult>
        {
			private readonly Request.ExportMasterRequest _request;

			public ExportMasterTask(Request.ExportMasterRequest request, UnityAction<AsyncResult<Result.ExportMasterResult>> userCallback) : base(userCallback)
			{
				_request = request;
			}

            protected override IEnumerator ExecuteImpl(Gs2Session gs2Session)
            {
				UnityWebRequest.method = UnityWebRequest.kHttpVerbGET;

                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "schedule")
                    .Replace("{region}", gs2Session.Region.DisplayName())
                    + "/{namespaceName}/master/export";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(_request.NamespaceName) ? _request.NamespaceName.ToString() : "null");

                var queryStrings = new List<string> ();
                if (_request.ContextStack != null)
                {
                    queryStrings.Add(string.Format("{0}={1}", "contextStack", UnityWebRequest.EscapeURL(_request.ContextStack)));
                }
                url += "?" + string.Join("&", queryStrings.ToArray());

                UnityWebRequest.url = url;

                if (_request.RequestId != null)
                {
                    UnityWebRequest.SetRequestHeader("X-GS2-REQUEST-ID", _request.RequestId);
                }

                return Send((Gs2RestSession)gs2Session);
            }
        }

		/// <summary>
		/// <returns>IEnumerator</returns>
		/// <param name="callback">コールバックハンドラ</param>
		/// <param name="request">リクエストパラメータ</param>
		public IEnumerator ExportMaster(
                Request.ExportMasterRequest request,
                UnityAction<AsyncResult<Result.ExportMasterResult>> callback
        )
		{
			var task = new ExportMasterTask(request, callback);
			if (_certificateHandler != null)
			{
				task.UnityWebRequest.certificateHandler = _certificateHandler;
			}
			return Gs2RestSession.Execute(task);
        }

        private class GetCurrentEventMasterTask : Gs2RestSessionTask<Result.GetCurrentEventMasterResult>
        {
			private readonly Request.GetCurrentEventMasterRequest _request;

			public GetCurrentEventMasterTask(Request.GetCurrentEventMasterRequest request, UnityAction<AsyncResult<Result.GetCurrentEventMasterResult>> userCallback) : base(userCallback)
			{
				_request = request;
			}

            protected override IEnumerator ExecuteImpl(Gs2Session gs2Session)
            {
				UnityWebRequest.method = UnityWebRequest.kHttpVerbGET;

                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "schedule")
                    .Replace("{region}", gs2Session.Region.DisplayName())
                    + "/{namespaceName}/master";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(_request.NamespaceName) ? _request.NamespaceName.ToString() : "null");

                var queryStrings = new List<string> ();
                if (_request.ContextStack != null)
                {
                    queryStrings.Add(string.Format("{0}={1}", "contextStack", UnityWebRequest.EscapeURL(_request.ContextStack)));
                }
                url += "?" + string.Join("&", queryStrings.ToArray());

                UnityWebRequest.url = url;

                if (_request.RequestId != null)
                {
                    UnityWebRequest.SetRequestHeader("X-GS2-REQUEST-ID", _request.RequestId);
                }

                return Send((Gs2RestSession)gs2Session);
            }
        }

		/// <summary>
		/// <returns>IEnumerator</returns>
		/// <param name="callback">コールバックハンドラ</param>
		/// <param name="request">リクエストパラメータ</param>
		public IEnumerator GetCurrentEventMaster(
                Request.GetCurrentEventMasterRequest request,
                UnityAction<AsyncResult<Result.GetCurrentEventMasterResult>> callback
        )
		{
			var task = new GetCurrentEventMasterTask(request, callback);
			if (_certificateHandler != null)
			{
				task.UnityWebRequest.certificateHandler = _certificateHandler;
			}
			return Gs2RestSession.Execute(task);
        }

        private class UpdateCurrentEventMasterTask : Gs2RestSessionTask<Result.UpdateCurrentEventMasterResult>
        {
			private readonly Request.UpdateCurrentEventMasterRequest _request;

			public UpdateCurrentEventMasterTask(Request.UpdateCurrentEventMasterRequest request, UnityAction<AsyncResult<Result.UpdateCurrentEventMasterResult>> userCallback) : base(userCallback)
			{
				_request = request;
			}

            protected override IEnumerator ExecuteImpl(Gs2Session gs2Session)
            {
				UnityWebRequest.method = UnityWebRequest.kHttpVerbPUT;

                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "schedule")
                    .Replace("{region}", gs2Session.Region.DisplayName())
                    + "/{namespaceName}/master";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(_request.NamespaceName) ? _request.NamespaceName.ToString() : "null");

                UnityWebRequest.url = url;

                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);
                jsonWriter.WriteObjectStart();
                if (_request.Settings != null)
                {
                    jsonWriter.WritePropertyName("settings");
                    jsonWriter.Write(_request.Settings.ToString());
                }
                if (_request.ContextStack != null)
                {
                    jsonWriter.WritePropertyName("contextStack");
                    jsonWriter.Write(_request.ContextStack.ToString());
                }
                jsonWriter.WriteObjectEnd();

                var body = stringBuilder.ToString();
                if (!string.IsNullOrEmpty(body))
                {
                    UnityWebRequest.uploadHandler = new UploadHandlerRaw(Encoding.UTF8.GetBytes(body));
                }
                UnityWebRequest.SetRequestHeader("Content-Type", "application/json");

                if (_request.RequestId != null)
                {
                    UnityWebRequest.SetRequestHeader("X-GS2-REQUEST-ID", _request.RequestId);
                }

                return Send((Gs2RestSession)gs2Session);
            }
        }

		/// <summary>
		/// <returns>IEnumerator</returns>
		/// <param name="callback">コールバックハンドラ</param>
		/// <param name="request">リクエストパラメータ</param>
		public IEnumerator UpdateCurrentEventMaster(
                Request.UpdateCurrentEventMasterRequest request,
                UnityAction<AsyncResult<Result.UpdateCurrentEventMasterResult>> callback
        )
		{
			var task = new UpdateCurrentEventMasterTask(request, callback);
			if (_certificateHandler != null)
			{
				task.UnityWebRequest.certificateHandler = _certificateHandler;
			}
			return Gs2RestSession.Execute(task);
        }

        private class UpdateCurrentEventMasterFromGitHubTask : Gs2RestSessionTask<Result.UpdateCurrentEventMasterFromGitHubResult>
        {
			private readonly Request.UpdateCurrentEventMasterFromGitHubRequest _request;

			public UpdateCurrentEventMasterFromGitHubTask(Request.UpdateCurrentEventMasterFromGitHubRequest request, UnityAction<AsyncResult<Result.UpdateCurrentEventMasterFromGitHubResult>> userCallback) : base(userCallback)
			{
				_request = request;
			}

            protected override IEnumerator ExecuteImpl(Gs2Session gs2Session)
            {
				UnityWebRequest.method = UnityWebRequest.kHttpVerbPUT;

                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "schedule")
                    .Replace("{region}", gs2Session.Region.DisplayName())
                    + "/{namespaceName}/master/from_git_hub";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(_request.NamespaceName) ? _request.NamespaceName.ToString() : "null");

                UnityWebRequest.url = url;

                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);
                jsonWriter.WriteObjectStart();
                if (_request.CheckoutSetting != null)
                {
                    jsonWriter.WritePropertyName("checkoutSetting");
                    _request.CheckoutSetting.WriteJson(jsonWriter);
                }
                if (_request.ContextStack != null)
                {
                    jsonWriter.WritePropertyName("contextStack");
                    jsonWriter.Write(_request.ContextStack.ToString());
                }
                jsonWriter.WriteObjectEnd();

                var body = stringBuilder.ToString();
                if (!string.IsNullOrEmpty(body))
                {
                    UnityWebRequest.uploadHandler = new UploadHandlerRaw(Encoding.UTF8.GetBytes(body));
                }
                UnityWebRequest.SetRequestHeader("Content-Type", "application/json");

                if (_request.RequestId != null)
                {
                    UnityWebRequest.SetRequestHeader("X-GS2-REQUEST-ID", _request.RequestId);
                }

                return Send((Gs2RestSession)gs2Session);
            }
        }

		/// <summary>
		/// <returns>IEnumerator</returns>
		/// <param name="callback">コールバックハンドラ</param>
		/// <param name="request">リクエストパラメータ</param>
		public IEnumerator UpdateCurrentEventMasterFromGitHub(
                Request.UpdateCurrentEventMasterFromGitHubRequest request,
                UnityAction<AsyncResult<Result.UpdateCurrentEventMasterFromGitHubResult>> callback
        )
		{
			var task = new UpdateCurrentEventMasterFromGitHubTask(request, callback);
			if (_certificateHandler != null)
			{
				task.UnityWebRequest.certificateHandler = _certificateHandler;
			}
			return Gs2RestSession.Execute(task);
        }
	}
}