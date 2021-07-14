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
using Gs2.Util.LitJson;namespace Gs2.Gs2Log
{
	public class Gs2LogRestClient : AbstractGs2Client
	{
		private readonly CertificateHandler _certificateHandler;

		public static string Endpoint = "log";

        protected Gs2RestSession Gs2RestSession => (Gs2RestSession) Gs2Session;

		/// <summary>
		/// コンストラクタ。
		/// </summary>
		/// <param name="Gs2RestSession">REST API 用セッション</param>
		public Gs2LogRestClient(Gs2RestSession Gs2RestSession) : base(Gs2RestSession)
		{

		}

		/// <summary>
		/// コンストラクタ。
		/// </summary>
		/// <param name="gs2RestSession">REST API 用セッション</param>
		/// <param name="certificateHandler"></param>
		public Gs2LogRestClient(Gs2RestSession gs2RestSession, CertificateHandler certificateHandler) : base(gs2RestSession)
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
                    .Replace("{service}", "log")
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
                    .Replace("{service}", "log")
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
                if (_request.Type != null)
                {
                    jsonWriter.WritePropertyName("type");
                    jsonWriter.Write(_request.Type.ToString());
                }
                if (_request.GcpCredentialJson != null)
                {
                    jsonWriter.WritePropertyName("gcpCredentialJson");
                    jsonWriter.Write(_request.GcpCredentialJson.ToString());
                }
                if (_request.BigQueryDatasetName != null)
                {
                    jsonWriter.WritePropertyName("bigQueryDatasetName");
                    jsonWriter.Write(_request.BigQueryDatasetName.ToString());
                }
                if (_request.LogExpireDays != null)
                {
                    jsonWriter.WritePropertyName("logExpireDays");
                    jsonWriter.Write(_request.LogExpireDays.ToString());
                }
                if (_request.AwsRegion != null)
                {
                    jsonWriter.WritePropertyName("awsRegion");
                    jsonWriter.Write(_request.AwsRegion.ToString());
                }
                if (_request.AwsAccessKeyId != null)
                {
                    jsonWriter.WritePropertyName("awsAccessKeyId");
                    jsonWriter.Write(_request.AwsAccessKeyId.ToString());
                }
                if (_request.AwsSecretAccessKey != null)
                {
                    jsonWriter.WritePropertyName("awsSecretAccessKey");
                    jsonWriter.Write(_request.AwsSecretAccessKey.ToString());
                }
                if (_request.FirehoseStreamName != null)
                {
                    jsonWriter.WritePropertyName("firehoseStreamName");
                    jsonWriter.Write(_request.FirehoseStreamName.ToString());
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
                    .Replace("{service}", "log")
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
                    .Replace("{service}", "log")
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
                    .Replace("{service}", "log")
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
                if (_request.Type != null)
                {
                    jsonWriter.WritePropertyName("type");
                    jsonWriter.Write(_request.Type.ToString());
                }
                if (_request.GcpCredentialJson != null)
                {
                    jsonWriter.WritePropertyName("gcpCredentialJson");
                    jsonWriter.Write(_request.GcpCredentialJson.ToString());
                }
                if (_request.BigQueryDatasetName != null)
                {
                    jsonWriter.WritePropertyName("bigQueryDatasetName");
                    jsonWriter.Write(_request.BigQueryDatasetName.ToString());
                }
                if (_request.LogExpireDays != null)
                {
                    jsonWriter.WritePropertyName("logExpireDays");
                    jsonWriter.Write(_request.LogExpireDays.ToString());
                }
                if (_request.AwsRegion != null)
                {
                    jsonWriter.WritePropertyName("awsRegion");
                    jsonWriter.Write(_request.AwsRegion.ToString());
                }
                if (_request.AwsAccessKeyId != null)
                {
                    jsonWriter.WritePropertyName("awsAccessKeyId");
                    jsonWriter.Write(_request.AwsAccessKeyId.ToString());
                }
                if (_request.AwsSecretAccessKey != null)
                {
                    jsonWriter.WritePropertyName("awsSecretAccessKey");
                    jsonWriter.Write(_request.AwsSecretAccessKey.ToString());
                }
                if (_request.FirehoseStreamName != null)
                {
                    jsonWriter.WritePropertyName("firehoseStreamName");
                    jsonWriter.Write(_request.FirehoseStreamName.ToString());
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
                    .Replace("{service}", "log")
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

        private class QueryAccessLogTask : Gs2RestSessionTask<Result.QueryAccessLogResult>
        {
			private readonly Request.QueryAccessLogRequest _request;

			public QueryAccessLogTask(Request.QueryAccessLogRequest request, UnityAction<AsyncResult<Result.QueryAccessLogResult>> userCallback) : base(userCallback)
			{
				_request = request;
			}

            protected override IEnumerator ExecuteImpl(Gs2Session gs2Session)
            {
				UnityWebRequest.method = UnityWebRequest.kHttpVerbGET;

                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "log")
                    .Replace("{region}", gs2Session.Region.DisplayName())
                    + "/{namespaceName}/log/access";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(_request.NamespaceName) ? _request.NamespaceName.ToString() : "null");

                var queryStrings = new List<string> ();
                if (_request.ContextStack != null)
                {
                    queryStrings.Add(string.Format("{0}={1}", "contextStack", UnityWebRequest.EscapeURL(_request.ContextStack)));
                }
                if (_request.Service != null) {
                    queryStrings.Add(string.Format("{0}={1}", "service", UnityWebRequest.EscapeURL(_request.Service)));
                }
                if (_request.Method != null) {
                    queryStrings.Add(string.Format("{0}={1}", "method", UnityWebRequest.EscapeURL(_request.Method)));
                }
                if (_request.UserId != null) {
                    queryStrings.Add(string.Format("{0}={1}", "userId", UnityWebRequest.EscapeURL(_request.UserId)));
                }
                if (_request.Begin != null) {
                    queryStrings.Add(string.Format("{0}={1}", "begin", _request.Begin));
                }
                if (_request.End != null) {
                    queryStrings.Add(string.Format("{0}={1}", "end", _request.End));
                }
                if (_request.LongTerm != null) {
                    queryStrings.Add(string.Format("{0}={1}", "longTerm", _request.LongTerm));
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
		public IEnumerator QueryAccessLog(
                Request.QueryAccessLogRequest request,
                UnityAction<AsyncResult<Result.QueryAccessLogResult>> callback
        )
		{
			var task = new QueryAccessLogTask(request, callback);
			if (_certificateHandler != null)
			{
				task.UnityWebRequest.certificateHandler = _certificateHandler;
			}
			return Gs2RestSession.Execute(task);
        }

        private class CountAccessLogTask : Gs2RestSessionTask<Result.CountAccessLogResult>
        {
			private readonly Request.CountAccessLogRequest _request;

			public CountAccessLogTask(Request.CountAccessLogRequest request, UnityAction<AsyncResult<Result.CountAccessLogResult>> userCallback) : base(userCallback)
			{
				_request = request;
			}

            protected override IEnumerator ExecuteImpl(Gs2Session gs2Session)
            {
				UnityWebRequest.method = UnityWebRequest.kHttpVerbGET;

                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "log")
                    .Replace("{region}", gs2Session.Region.DisplayName())
                    + "/{namespaceName}/log/access/count";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(_request.NamespaceName) ? _request.NamespaceName.ToString() : "null");

                var queryStrings = new List<string> ();
                if (_request.ContextStack != null)
                {
                    queryStrings.Add(string.Format("{0}={1}", "contextStack", UnityWebRequest.EscapeURL(_request.ContextStack)));
                }
                if (_request.Service != null) {
                    queryStrings.Add(string.Format("{0}={1}", "service", UnityWebRequest.EscapeURL(_request.Service)));
                }
                if (_request.Method != null) {
                    queryStrings.Add(string.Format("{0}={1}", "method", UnityWebRequest.EscapeURL(_request.Method)));
                }
                if (_request.UserId != null) {
                    queryStrings.Add(string.Format("{0}={1}", "userId", UnityWebRequest.EscapeURL(_request.UserId)));
                }
                if (_request.Begin != null) {
                    queryStrings.Add(string.Format("{0}={1}", "begin", _request.Begin));
                }
                if (_request.End != null) {
                    queryStrings.Add(string.Format("{0}={1}", "end", _request.End));
                }
                if (_request.LongTerm != null) {
                    queryStrings.Add(string.Format("{0}={1}", "longTerm", _request.LongTerm));
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
		public IEnumerator CountAccessLog(
                Request.CountAccessLogRequest request,
                UnityAction<AsyncResult<Result.CountAccessLogResult>> callback
        )
		{
			var task = new CountAccessLogTask(request, callback);
			if (_certificateHandler != null)
			{
				task.UnityWebRequest.certificateHandler = _certificateHandler;
			}
			return Gs2RestSession.Execute(task);
        }

        private class QueryIssueStampSheetLogTask : Gs2RestSessionTask<Result.QueryIssueStampSheetLogResult>
        {
			private readonly Request.QueryIssueStampSheetLogRequest _request;

			public QueryIssueStampSheetLogTask(Request.QueryIssueStampSheetLogRequest request, UnityAction<AsyncResult<Result.QueryIssueStampSheetLogResult>> userCallback) : base(userCallback)
			{
				_request = request;
			}

            protected override IEnumerator ExecuteImpl(Gs2Session gs2Session)
            {
				UnityWebRequest.method = UnityWebRequest.kHttpVerbGET;

                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "log")
                    .Replace("{region}", gs2Session.Region.DisplayName())
                    + "/{namespaceName}/log/issue/stamp/sheet";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(_request.NamespaceName) ? _request.NamespaceName.ToString() : "null");

                var queryStrings = new List<string> ();
                if (_request.ContextStack != null)
                {
                    queryStrings.Add(string.Format("{0}={1}", "contextStack", UnityWebRequest.EscapeURL(_request.ContextStack)));
                }
                if (_request.Service != null) {
                    queryStrings.Add(string.Format("{0}={1}", "service", UnityWebRequest.EscapeURL(_request.Service)));
                }
                if (_request.Method != null) {
                    queryStrings.Add(string.Format("{0}={1}", "method", UnityWebRequest.EscapeURL(_request.Method)));
                }
                if (_request.UserId != null) {
                    queryStrings.Add(string.Format("{0}={1}", "userId", UnityWebRequest.EscapeURL(_request.UserId)));
                }
                if (_request.Action != null) {
                    queryStrings.Add(string.Format("{0}={1}", "action", UnityWebRequest.EscapeURL(_request.Action)));
                }
                if (_request.Begin != null) {
                    queryStrings.Add(string.Format("{0}={1}", "begin", _request.Begin));
                }
                if (_request.End != null) {
                    queryStrings.Add(string.Format("{0}={1}", "end", _request.End));
                }
                if (_request.LongTerm != null) {
                    queryStrings.Add(string.Format("{0}={1}", "longTerm", _request.LongTerm));
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
		public IEnumerator QueryIssueStampSheetLog(
                Request.QueryIssueStampSheetLogRequest request,
                UnityAction<AsyncResult<Result.QueryIssueStampSheetLogResult>> callback
        )
		{
			var task = new QueryIssueStampSheetLogTask(request, callback);
			if (_certificateHandler != null)
			{
				task.UnityWebRequest.certificateHandler = _certificateHandler;
			}
			return Gs2RestSession.Execute(task);
        }

        private class CountIssueStampSheetLogTask : Gs2RestSessionTask<Result.CountIssueStampSheetLogResult>
        {
			private readonly Request.CountIssueStampSheetLogRequest _request;

			public CountIssueStampSheetLogTask(Request.CountIssueStampSheetLogRequest request, UnityAction<AsyncResult<Result.CountIssueStampSheetLogResult>> userCallback) : base(userCallback)
			{
				_request = request;
			}

            protected override IEnumerator ExecuteImpl(Gs2Session gs2Session)
            {
				UnityWebRequest.method = UnityWebRequest.kHttpVerbGET;

                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "log")
                    .Replace("{region}", gs2Session.Region.DisplayName())
                    + "/{namespaceName}/log/issue/stamp/sheet/count";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(_request.NamespaceName) ? _request.NamespaceName.ToString() : "null");

                var queryStrings = new List<string> ();
                if (_request.ContextStack != null)
                {
                    queryStrings.Add(string.Format("{0}={1}", "contextStack", UnityWebRequest.EscapeURL(_request.ContextStack)));
                }
                if (_request.Service != null) {
                    queryStrings.Add(string.Format("{0}={1}", "service", UnityWebRequest.EscapeURL(_request.Service)));
                }
                if (_request.Method != null) {
                    queryStrings.Add(string.Format("{0}={1}", "method", UnityWebRequest.EscapeURL(_request.Method)));
                }
                if (_request.UserId != null) {
                    queryStrings.Add(string.Format("{0}={1}", "userId", UnityWebRequest.EscapeURL(_request.UserId)));
                }
                if (_request.Action != null) {
                    queryStrings.Add(string.Format("{0}={1}", "action", UnityWebRequest.EscapeURL(_request.Action)));
                }
                if (_request.Begin != null) {
                    queryStrings.Add(string.Format("{0}={1}", "begin", _request.Begin));
                }
                if (_request.End != null) {
                    queryStrings.Add(string.Format("{0}={1}", "end", _request.End));
                }
                if (_request.LongTerm != null) {
                    queryStrings.Add(string.Format("{0}={1}", "longTerm", _request.LongTerm));
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
		public IEnumerator CountIssueStampSheetLog(
                Request.CountIssueStampSheetLogRequest request,
                UnityAction<AsyncResult<Result.CountIssueStampSheetLogResult>> callback
        )
		{
			var task = new CountIssueStampSheetLogTask(request, callback);
			if (_certificateHandler != null)
			{
				task.UnityWebRequest.certificateHandler = _certificateHandler;
			}
			return Gs2RestSession.Execute(task);
        }

        private class QueryExecuteStampSheetLogTask : Gs2RestSessionTask<Result.QueryExecuteStampSheetLogResult>
        {
			private readonly Request.QueryExecuteStampSheetLogRequest _request;

			public QueryExecuteStampSheetLogTask(Request.QueryExecuteStampSheetLogRequest request, UnityAction<AsyncResult<Result.QueryExecuteStampSheetLogResult>> userCallback) : base(userCallback)
			{
				_request = request;
			}

            protected override IEnumerator ExecuteImpl(Gs2Session gs2Session)
            {
				UnityWebRequest.method = UnityWebRequest.kHttpVerbGET;

                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "log")
                    .Replace("{region}", gs2Session.Region.DisplayName())
                    + "/{namespaceName}/log/execute/stamp/sheet";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(_request.NamespaceName) ? _request.NamespaceName.ToString() : "null");

                var queryStrings = new List<string> ();
                if (_request.ContextStack != null)
                {
                    queryStrings.Add(string.Format("{0}={1}", "contextStack", UnityWebRequest.EscapeURL(_request.ContextStack)));
                }
                if (_request.Service != null) {
                    queryStrings.Add(string.Format("{0}={1}", "service", UnityWebRequest.EscapeURL(_request.Service)));
                }
                if (_request.Method != null) {
                    queryStrings.Add(string.Format("{0}={1}", "method", UnityWebRequest.EscapeURL(_request.Method)));
                }
                if (_request.UserId != null) {
                    queryStrings.Add(string.Format("{0}={1}", "userId", UnityWebRequest.EscapeURL(_request.UserId)));
                }
                if (_request.Action != null) {
                    queryStrings.Add(string.Format("{0}={1}", "action", UnityWebRequest.EscapeURL(_request.Action)));
                }
                if (_request.Begin != null) {
                    queryStrings.Add(string.Format("{0}={1}", "begin", _request.Begin));
                }
                if (_request.End != null) {
                    queryStrings.Add(string.Format("{0}={1}", "end", _request.End));
                }
                if (_request.LongTerm != null) {
                    queryStrings.Add(string.Format("{0}={1}", "longTerm", _request.LongTerm));
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
		public IEnumerator QueryExecuteStampSheetLog(
                Request.QueryExecuteStampSheetLogRequest request,
                UnityAction<AsyncResult<Result.QueryExecuteStampSheetLogResult>> callback
        )
		{
			var task = new QueryExecuteStampSheetLogTask(request, callback);
			if (_certificateHandler != null)
			{
				task.UnityWebRequest.certificateHandler = _certificateHandler;
			}
			return Gs2RestSession.Execute(task);
        }

        private class CountExecuteStampSheetLogTask : Gs2RestSessionTask<Result.CountExecuteStampSheetLogResult>
        {
			private readonly Request.CountExecuteStampSheetLogRequest _request;

			public CountExecuteStampSheetLogTask(Request.CountExecuteStampSheetLogRequest request, UnityAction<AsyncResult<Result.CountExecuteStampSheetLogResult>> userCallback) : base(userCallback)
			{
				_request = request;
			}

            protected override IEnumerator ExecuteImpl(Gs2Session gs2Session)
            {
				UnityWebRequest.method = UnityWebRequest.kHttpVerbGET;

                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "log")
                    .Replace("{region}", gs2Session.Region.DisplayName())
                    + "/{namespaceName}/log/execute/stamp/sheet/count";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(_request.NamespaceName) ? _request.NamespaceName.ToString() : "null");

                var queryStrings = new List<string> ();
                if (_request.ContextStack != null)
                {
                    queryStrings.Add(string.Format("{0}={1}", "contextStack", UnityWebRequest.EscapeURL(_request.ContextStack)));
                }
                if (_request.Service != null) {
                    queryStrings.Add(string.Format("{0}={1}", "service", UnityWebRequest.EscapeURL(_request.Service)));
                }
                if (_request.Method != null) {
                    queryStrings.Add(string.Format("{0}={1}", "method", UnityWebRequest.EscapeURL(_request.Method)));
                }
                if (_request.UserId != null) {
                    queryStrings.Add(string.Format("{0}={1}", "userId", UnityWebRequest.EscapeURL(_request.UserId)));
                }
                if (_request.Action != null) {
                    queryStrings.Add(string.Format("{0}={1}", "action", UnityWebRequest.EscapeURL(_request.Action)));
                }
                if (_request.Begin != null) {
                    queryStrings.Add(string.Format("{0}={1}", "begin", _request.Begin));
                }
                if (_request.End != null) {
                    queryStrings.Add(string.Format("{0}={1}", "end", _request.End));
                }
                if (_request.LongTerm != null) {
                    queryStrings.Add(string.Format("{0}={1}", "longTerm", _request.LongTerm));
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
		public IEnumerator CountExecuteStampSheetLog(
                Request.CountExecuteStampSheetLogRequest request,
                UnityAction<AsyncResult<Result.CountExecuteStampSheetLogResult>> callback
        )
		{
			var task = new CountExecuteStampSheetLogTask(request, callback);
			if (_certificateHandler != null)
			{
				task.UnityWebRequest.certificateHandler = _certificateHandler;
			}
			return Gs2RestSession.Execute(task);
        }

        private class QueryExecuteStampTaskLogTask : Gs2RestSessionTask<Result.QueryExecuteStampTaskLogResult>
        {
			private readonly Request.QueryExecuteStampTaskLogRequest _request;

			public QueryExecuteStampTaskLogTask(Request.QueryExecuteStampTaskLogRequest request, UnityAction<AsyncResult<Result.QueryExecuteStampTaskLogResult>> userCallback) : base(userCallback)
			{
				_request = request;
			}

            protected override IEnumerator ExecuteImpl(Gs2Session gs2Session)
            {
				UnityWebRequest.method = UnityWebRequest.kHttpVerbGET;

                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "log")
                    .Replace("{region}", gs2Session.Region.DisplayName())
                    + "/{namespaceName}/log/execute/stamp/task";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(_request.NamespaceName) ? _request.NamespaceName.ToString() : "null");

                var queryStrings = new List<string> ();
                if (_request.ContextStack != null)
                {
                    queryStrings.Add(string.Format("{0}={1}", "contextStack", UnityWebRequest.EscapeURL(_request.ContextStack)));
                }
                if (_request.Service != null) {
                    queryStrings.Add(string.Format("{0}={1}", "service", UnityWebRequest.EscapeURL(_request.Service)));
                }
                if (_request.Method != null) {
                    queryStrings.Add(string.Format("{0}={1}", "method", UnityWebRequest.EscapeURL(_request.Method)));
                }
                if (_request.UserId != null) {
                    queryStrings.Add(string.Format("{0}={1}", "userId", UnityWebRequest.EscapeURL(_request.UserId)));
                }
                if (_request.Action != null) {
                    queryStrings.Add(string.Format("{0}={1}", "action", UnityWebRequest.EscapeURL(_request.Action)));
                }
                if (_request.Begin != null) {
                    queryStrings.Add(string.Format("{0}={1}", "begin", _request.Begin));
                }
                if (_request.End != null) {
                    queryStrings.Add(string.Format("{0}={1}", "end", _request.End));
                }
                if (_request.LongTerm != null) {
                    queryStrings.Add(string.Format("{0}={1}", "longTerm", _request.LongTerm));
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
		public IEnumerator QueryExecuteStampTaskLog(
                Request.QueryExecuteStampTaskLogRequest request,
                UnityAction<AsyncResult<Result.QueryExecuteStampTaskLogResult>> callback
        )
		{
			var task = new QueryExecuteStampTaskLogTask(request, callback);
			if (_certificateHandler != null)
			{
				task.UnityWebRequest.certificateHandler = _certificateHandler;
			}
			return Gs2RestSession.Execute(task);
        }

        private class CountExecuteStampTaskLogTask : Gs2RestSessionTask<Result.CountExecuteStampTaskLogResult>
        {
			private readonly Request.CountExecuteStampTaskLogRequest _request;

			public CountExecuteStampTaskLogTask(Request.CountExecuteStampTaskLogRequest request, UnityAction<AsyncResult<Result.CountExecuteStampTaskLogResult>> userCallback) : base(userCallback)
			{
				_request = request;
			}

            protected override IEnumerator ExecuteImpl(Gs2Session gs2Session)
            {
				UnityWebRequest.method = UnityWebRequest.kHttpVerbGET;

                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "log")
                    .Replace("{region}", gs2Session.Region.DisplayName())
                    + "/{namespaceName}/log/execute/stamp/task/count";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(_request.NamespaceName) ? _request.NamespaceName.ToString() : "null");

                var queryStrings = new List<string> ();
                if (_request.ContextStack != null)
                {
                    queryStrings.Add(string.Format("{0}={1}", "contextStack", UnityWebRequest.EscapeURL(_request.ContextStack)));
                }
                if (_request.Service != null) {
                    queryStrings.Add(string.Format("{0}={1}", "service", UnityWebRequest.EscapeURL(_request.Service)));
                }
                if (_request.Method != null) {
                    queryStrings.Add(string.Format("{0}={1}", "method", UnityWebRequest.EscapeURL(_request.Method)));
                }
                if (_request.UserId != null) {
                    queryStrings.Add(string.Format("{0}={1}", "userId", UnityWebRequest.EscapeURL(_request.UserId)));
                }
                if (_request.Action != null) {
                    queryStrings.Add(string.Format("{0}={1}", "action", UnityWebRequest.EscapeURL(_request.Action)));
                }
                if (_request.Begin != null) {
                    queryStrings.Add(string.Format("{0}={1}", "begin", _request.Begin));
                }
                if (_request.End != null) {
                    queryStrings.Add(string.Format("{0}={1}", "end", _request.End));
                }
                if (_request.LongTerm != null) {
                    queryStrings.Add(string.Format("{0}={1}", "longTerm", _request.LongTerm));
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
		public IEnumerator CountExecuteStampTaskLog(
                Request.CountExecuteStampTaskLogRequest request,
                UnityAction<AsyncResult<Result.CountExecuteStampTaskLogResult>> callback
        )
		{
			var task = new CountExecuteStampTaskLogTask(request, callback);
			if (_certificateHandler != null)
			{
				task.UnityWebRequest.certificateHandler = _certificateHandler;
			}
			return Gs2RestSession.Execute(task);
        }
	}
}