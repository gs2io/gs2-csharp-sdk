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
using Gs2.Gs2Watch.Request;
using Gs2.Gs2Watch.Result;
using Gs2.Util.LitJson;

namespace Gs2.Gs2Watch
{
	public class Gs2WatchRestClient : AbstractGs2Client
	{
#if UNITY_2017_1_OR_NEWER
		private readonly CertificateHandler _certificateHandler;
#endif

		public static string Endpoint = "watch";

        protected Gs2RestSession Gs2RestSession => (Gs2RestSession) Gs2Session;

		public Gs2WatchRestClient(Gs2RestSession Gs2RestSession) : base(Gs2RestSession)
		{

		}

#if UNITY_2017_1_OR_NEWER
		public Gs2WatchRestClient(Gs2RestSession gs2RestSession, CertificateHandler certificateHandler) : base(gs2RestSession)
		{
			_certificateHandler = certificateHandler;
		}
#endif


        public class GetChartTask : Gs2RestSessionTask<GetChartRequest, GetChartResult>
        {
            public GetChartTask(IGs2Session session, RestSessionRequestFactory factory, GetChartRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(GetChartRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "watch")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/chart/{metrics}";

                url = url.Replace("{metrics}", !string.IsNullOrEmpty(request.Metrics) ? request.Metrics.ToString() : "null");

                var sessionRequest = Factory.Post(url);

                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);
                jsonWriter.WriteObjectStart();
                if (request.Grn != null)
                {
                    jsonWriter.WritePropertyName("grn");
                    jsonWriter.Write(request.Grn);
                }
                if (request.Queries != null)
                {
                    jsonWriter.WritePropertyName("queries");
                    jsonWriter.WriteArrayStart();
                    foreach(var item in request.Queries)
                    {
                        jsonWriter.Write(item);
                    }
                    jsonWriter.WriteArrayEnd();
                }
                if (request.By != null)
                {
                    jsonWriter.WritePropertyName("by");
                    jsonWriter.Write(request.By);
                }
                if (request.Timeframe != null)
                {
                    jsonWriter.WritePropertyName("timeframe");
                    jsonWriter.Write(request.Timeframe);
                }
                if (request.Size != null)
                {
                    jsonWriter.WritePropertyName("size");
                    jsonWriter.Write(request.Size);
                }
                if (request.Format != null)
                {
                    jsonWriter.WritePropertyName("format");
                    jsonWriter.Write(request.Format);
                }
                if (request.Aggregator != null)
                {
                    jsonWriter.WritePropertyName("aggregator");
                    jsonWriter.Write(request.Aggregator);
                }
                if (request.Style != null)
                {
                    jsonWriter.WritePropertyName("style");
                    jsonWriter.Write(request.Style);
                }
                if (request.Title != null)
                {
                    jsonWriter.WritePropertyName("title");
                    jsonWriter.Write(request.Title);
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
		public IEnumerator GetChart(
                Request.GetChartRequest request,
                UnityAction<AsyncResult<Result.GetChartResult>> callback
        )
		{
			var task = new GetChartTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.GetChartResult>(task.Result, task.Error));
        }

		public IFuture<Result.GetChartResult> GetChartFuture(
                Request.GetChartRequest request
        )
		{
			return new GetChartTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.GetChartResult> GetChartAsync(
                Request.GetChartRequest request
        )
		{
            AsyncResult<Result.GetChartResult> result = null;
			await GetChart(
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
		public GetChartTask GetChartAsync(
                Request.GetChartRequest request
        )
		{
			return new GetChartTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.GetChartResult> GetChartAsync(
                Request.GetChartRequest request
        )
		{
			var task = new GetChartTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class GetCumulativeTask : Gs2RestSessionTask<GetCumulativeRequest, GetCumulativeResult>
        {
            public GetCumulativeTask(IGs2Session session, RestSessionRequestFactory factory, GetCumulativeRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(GetCumulativeRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "watch")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/cumulative/{name}";

                url = url.Replace("{name}", !string.IsNullOrEmpty(request.Name) ? request.Name.ToString() : "null");

                var sessionRequest = Factory.Post(url);

                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);
                jsonWriter.WriteObjectStart();
                if (request.ResourceGrn != null)
                {
                    jsonWriter.WritePropertyName("resourceGrn");
                    jsonWriter.Write(request.ResourceGrn);
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
		public IEnumerator GetCumulative(
                Request.GetCumulativeRequest request,
                UnityAction<AsyncResult<Result.GetCumulativeResult>> callback
        )
		{
			var task = new GetCumulativeTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.GetCumulativeResult>(task.Result, task.Error));
        }

		public IFuture<Result.GetCumulativeResult> GetCumulativeFuture(
                Request.GetCumulativeRequest request
        )
		{
			return new GetCumulativeTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.GetCumulativeResult> GetCumulativeAsync(
                Request.GetCumulativeRequest request
        )
		{
            AsyncResult<Result.GetCumulativeResult> result = null;
			await GetCumulative(
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
		public GetCumulativeTask GetCumulativeAsync(
                Request.GetCumulativeRequest request
        )
		{
			return new GetCumulativeTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.GetCumulativeResult> GetCumulativeAsync(
                Request.GetCumulativeRequest request
        )
		{
			var task = new GetCumulativeTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class DescribeBillingActivitiesTask : Gs2RestSessionTask<DescribeBillingActivitiesRequest, DescribeBillingActivitiesResult>
        {
            public DescribeBillingActivitiesTask(IGs2Session session, RestSessionRequestFactory factory, DescribeBillingActivitiesRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DescribeBillingActivitiesRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "watch")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/billingActivity/{year}/{month}";

                url = url.Replace("{year}",request.Year != null ? request.Year.ToString() : "null");
                url = url.Replace("{month}",request.Month != null ? request.Month.ToString() : "null");

                var sessionRequest = Factory.Get(url);
                if (request.ContextStack != null)
                {
                    sessionRequest.AddQueryString("contextStack", request.ContextStack);
                }
                if (request.Service != null) {
                    sessionRequest.AddQueryString("service", $"{request.Service}");
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
		public IEnumerator DescribeBillingActivities(
                Request.DescribeBillingActivitiesRequest request,
                UnityAction<AsyncResult<Result.DescribeBillingActivitiesResult>> callback
        )
		{
			var task = new DescribeBillingActivitiesTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.DescribeBillingActivitiesResult>(task.Result, task.Error));
        }

		public IFuture<Result.DescribeBillingActivitiesResult> DescribeBillingActivitiesFuture(
                Request.DescribeBillingActivitiesRequest request
        )
		{
			return new DescribeBillingActivitiesTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.DescribeBillingActivitiesResult> DescribeBillingActivitiesAsync(
                Request.DescribeBillingActivitiesRequest request
        )
		{
            AsyncResult<Result.DescribeBillingActivitiesResult> result = null;
			await DescribeBillingActivities(
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
		public DescribeBillingActivitiesTask DescribeBillingActivitiesAsync(
                Request.DescribeBillingActivitiesRequest request
        )
		{
			return new DescribeBillingActivitiesTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.DescribeBillingActivitiesResult> DescribeBillingActivitiesAsync(
                Request.DescribeBillingActivitiesRequest request
        )
		{
			var task = new DescribeBillingActivitiesTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class GetBillingActivityTask : Gs2RestSessionTask<GetBillingActivityRequest, GetBillingActivityResult>
        {
            public GetBillingActivityTask(IGs2Session session, RestSessionRequestFactory factory, GetBillingActivityRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(GetBillingActivityRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "watch")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/billingActivity/{year}/{month}/{service}/{activityType}";

                url = url.Replace("{year}",request.Year != null ? request.Year.ToString() : "null");
                url = url.Replace("{month}",request.Month != null ? request.Month.ToString() : "null");
                url = url.Replace("{service}", !string.IsNullOrEmpty(request.Service) ? request.Service.ToString() : "null");
                url = url.Replace("{activityType}", !string.IsNullOrEmpty(request.ActivityType) ? request.ActivityType.ToString() : "null");

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
		public IEnumerator GetBillingActivity(
                Request.GetBillingActivityRequest request,
                UnityAction<AsyncResult<Result.GetBillingActivityResult>> callback
        )
		{
			var task = new GetBillingActivityTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.GetBillingActivityResult>(task.Result, task.Error));
        }

		public IFuture<Result.GetBillingActivityResult> GetBillingActivityFuture(
                Request.GetBillingActivityRequest request
        )
		{
			return new GetBillingActivityTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.GetBillingActivityResult> GetBillingActivityAsync(
                Request.GetBillingActivityRequest request
        )
		{
            AsyncResult<Result.GetBillingActivityResult> result = null;
			await GetBillingActivity(
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
		public GetBillingActivityTask GetBillingActivityAsync(
                Request.GetBillingActivityRequest request
        )
		{
			return new GetBillingActivityTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.GetBillingActivityResult> GetBillingActivityAsync(
                Request.GetBillingActivityRequest request
        )
		{
			var task = new GetBillingActivityTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif
	}
}