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
using Gs2.Util.LitJson;namespace Gs2.Gs2Watch
{
	public class Gs2WatchRestClient : AbstractGs2Client
	{
		private readonly CertificateHandler _certificateHandler;

		public static string Endpoint = "watch";

        protected Gs2RestSession Gs2RestSession => (Gs2RestSession) Gs2Session;

		/// <summary>
		/// コンストラクタ。
		/// </summary>
		/// <param name="Gs2RestSession">REST API 用セッション</param>
		public Gs2WatchRestClient(Gs2RestSession Gs2RestSession) : base(Gs2RestSession)
		{

		}

		/// <summary>
		/// コンストラクタ。
		/// </summary>
		/// <param name="gs2RestSession">REST API 用セッション</param>
		/// <param name="certificateHandler"></param>
		public Gs2WatchRestClient(Gs2RestSession gs2RestSession, CertificateHandler certificateHandler) : base(gs2RestSession)
		{
			_certificateHandler = certificateHandler;
		}

        private class GetChartTask : Gs2RestSessionTask<Result.GetChartResult>
        {
			private readonly Request.GetChartRequest _request;

			public GetChartTask(Request.GetChartRequest request, UnityAction<AsyncResult<Result.GetChartResult>> userCallback) : base(userCallback)
			{
				_request = request;
			}

            protected override IEnumerator ExecuteImpl(Gs2Session gs2Session)
            {
				UnityWebRequest.method = UnityWebRequest.kHttpVerbPOST;

                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "watch")
                    .Replace("{region}", gs2Session.Region.DisplayName())
                    + "/chart/{metrics}";

                url = url.Replace("{metrics}", !string.IsNullOrEmpty(_request.Metrics) ? _request.Metrics.ToString() : "null");

                UnityWebRequest.url = url;

                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);
                jsonWriter.WriteObjectStart();
                if (_request.Grn != null)
                {
                    jsonWriter.WritePropertyName("grn");
                    jsonWriter.Write(_request.Grn.ToString());
                }
                if (_request.Queries != null)
                {
                    jsonWriter.WritePropertyName("queries");
                    jsonWriter.WriteArrayStart();
                    foreach(var item in _request.Queries)
                    {
                        jsonWriter.Write(item);
                    }
                    jsonWriter.WriteArrayEnd();
                }
                if (_request.By != null)
                {
                    jsonWriter.WritePropertyName("by");
                    jsonWriter.Write(_request.By.ToString());
                }
                if (_request.Timeframe != null)
                {
                    jsonWriter.WritePropertyName("timeframe");
                    jsonWriter.Write(_request.Timeframe.ToString());
                }
                if (_request.Size != null)
                {
                    jsonWriter.WritePropertyName("size");
                    jsonWriter.Write(_request.Size.ToString());
                }
                if (_request.Format != null)
                {
                    jsonWriter.WritePropertyName("format");
                    jsonWriter.Write(_request.Format.ToString());
                }
                if (_request.Aggregator != null)
                {
                    jsonWriter.WritePropertyName("aggregator");
                    jsonWriter.Write(_request.Aggregator.ToString());
                }
                if (_request.Style != null)
                {
                    jsonWriter.WritePropertyName("style");
                    jsonWriter.Write(_request.Style.ToString());
                }
                if (_request.Title != null)
                {
                    jsonWriter.WritePropertyName("title");
                    jsonWriter.Write(_request.Title.ToString());
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
		public IEnumerator GetChart(
                Request.GetChartRequest request,
                UnityAction<AsyncResult<Result.GetChartResult>> callback
        )
		{
			var task = new GetChartTask(request, callback);
			if (_certificateHandler != null)
			{
				task.UnityWebRequest.certificateHandler = _certificateHandler;
			}
			return Gs2RestSession.Execute(task);
        }

        private class GetCumulativeTask : Gs2RestSessionTask<Result.GetCumulativeResult>
        {
			private readonly Request.GetCumulativeRequest _request;

			public GetCumulativeTask(Request.GetCumulativeRequest request, UnityAction<AsyncResult<Result.GetCumulativeResult>> userCallback) : base(userCallback)
			{
				_request = request;
			}

            protected override IEnumerator ExecuteImpl(Gs2Session gs2Session)
            {
				UnityWebRequest.method = UnityWebRequest.kHttpVerbPOST;

                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "watch")
                    .Replace("{region}", gs2Session.Region.DisplayName())
                    + "/cumulative/{name}";

                url = url.Replace("{name}", !string.IsNullOrEmpty(_request.Name) ? _request.Name.ToString() : "null");

                UnityWebRequest.url = url;

                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);
                jsonWriter.WriteObjectStart();
                if (_request.ResourceGrn != null)
                {
                    jsonWriter.WritePropertyName("resourceGrn");
                    jsonWriter.Write(_request.ResourceGrn.ToString());
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
		public IEnumerator GetCumulative(
                Request.GetCumulativeRequest request,
                UnityAction<AsyncResult<Result.GetCumulativeResult>> callback
        )
		{
			var task = new GetCumulativeTask(request, callback);
			if (_certificateHandler != null)
			{
				task.UnityWebRequest.certificateHandler = _certificateHandler;
			}
			return Gs2RestSession.Execute(task);
        }

        private class DescribeBillingActivitiesTask : Gs2RestSessionTask<Result.DescribeBillingActivitiesResult>
        {
			private readonly Request.DescribeBillingActivitiesRequest _request;

			public DescribeBillingActivitiesTask(Request.DescribeBillingActivitiesRequest request, UnityAction<AsyncResult<Result.DescribeBillingActivitiesResult>> userCallback) : base(userCallback)
			{
				_request = request;
			}

            protected override IEnumerator ExecuteImpl(Gs2Session gs2Session)
            {
				UnityWebRequest.method = UnityWebRequest.kHttpVerbGET;

                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "watch")
                    .Replace("{region}", gs2Session.Region.DisplayName())
                    + "/billingActivity/{year}/{month}";

                url = url.Replace("{year}",_request.Year != null ? _request.Year.ToString() : "null");
                url = url.Replace("{month}",_request.Month != null ? _request.Month.ToString() : "null");

                var queryStrings = new List<string> ();
                if (_request.ContextStack != null)
                {
                    queryStrings.Add(string.Format("{0}={1}", "contextStack", UnityWebRequest.EscapeURL(_request.ContextStack)));
                }
                if (_request.Service != null) {
                    queryStrings.Add(string.Format("{0}={1}", "service", UnityWebRequest.EscapeURL(_request.Service)));
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
		public IEnumerator DescribeBillingActivities(
                Request.DescribeBillingActivitiesRequest request,
                UnityAction<AsyncResult<Result.DescribeBillingActivitiesResult>> callback
        )
		{
			var task = new DescribeBillingActivitiesTask(request, callback);
			if (_certificateHandler != null)
			{
				task.UnityWebRequest.certificateHandler = _certificateHandler;
			}
			return Gs2RestSession.Execute(task);
        }

        private class GetBillingActivityTask : Gs2RestSessionTask<Result.GetBillingActivityResult>
        {
			private readonly Request.GetBillingActivityRequest _request;

			public GetBillingActivityTask(Request.GetBillingActivityRequest request, UnityAction<AsyncResult<Result.GetBillingActivityResult>> userCallback) : base(userCallback)
			{
				_request = request;
			}

            protected override IEnumerator ExecuteImpl(Gs2Session gs2Session)
            {
				UnityWebRequest.method = UnityWebRequest.kHttpVerbPOST;

                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "watch")
                    .Replace("{region}", gs2Session.Region.DisplayName())
                    + "/billingActivity/{year}/{month}/{service}/{activityType}";

                url = url.Replace("{year}",_request.Year != null ? _request.Year.ToString() : "null");
                url = url.Replace("{month}",_request.Month != null ? _request.Month.ToString() : "null");
                url = url.Replace("{service}", !string.IsNullOrEmpty(_request.Service) ? _request.Service.ToString() : "null");
                url = url.Replace("{activityType}", !string.IsNullOrEmpty(_request.ActivityType) ? _request.ActivityType.ToString() : "null");

                UnityWebRequest.url = url;

                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);
                jsonWriter.WriteObjectStart();
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
		public IEnumerator GetBillingActivity(
                Request.GetBillingActivityRequest request,
                UnityAction<AsyncResult<Result.GetBillingActivityResult>> callback
        )
		{
			var task = new GetBillingActivityTask(request, callback);
			if (_certificateHandler != null)
			{
				task.UnityWebRequest.certificateHandler = _certificateHandler;
			}
			return Gs2RestSession.Execute(task);
        }
	}
}