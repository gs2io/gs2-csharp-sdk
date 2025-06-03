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
using Gs2.Gs2Freeze.Request;
using Gs2.Gs2Freeze.Result;
using Gs2.Util.LitJson;

namespace Gs2.Gs2Freeze
{
	public class Gs2FreezeRestClient : AbstractGs2Client
	{
#if UNITY_2017_1_OR_NEWER
		private readonly CertificateHandler _certificateHandler;
#endif

		public static string Endpoint = "freeze";

        protected Gs2RestSession Gs2RestSession => (Gs2RestSession) Gs2Session;

		public Gs2FreezeRestClient(Gs2RestSession Gs2RestSession) : base(Gs2RestSession)
		{

		}

#if UNITY_2017_1_OR_NEWER
		public Gs2FreezeRestClient(Gs2RestSession gs2RestSession, CertificateHandler certificateHandler) : base(gs2RestSession)
		{
			_certificateHandler = certificateHandler;
		}
#endif


        public class DescribeStagesTask : Gs2RestSessionTask<DescribeStagesRequest, DescribeStagesResult>
        {
            public DescribeStagesTask(IGs2Session session, RestSessionRequestFactory factory, DescribeStagesRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DescribeStagesRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "freeze")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/";

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
		public IEnumerator DescribeStages(
                Request.DescribeStagesRequest request,
                UnityAction<AsyncResult<Result.DescribeStagesResult>> callback
        )
		{
			var task = new DescribeStagesTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.DescribeStagesResult>(task.Result, task.Error));
        }

		public IFuture<Result.DescribeStagesResult> DescribeStagesFuture(
                Request.DescribeStagesRequest request
        )
		{
			return new DescribeStagesTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.DescribeStagesResult> DescribeStagesAsync(
                Request.DescribeStagesRequest request
        )
		{
            AsyncResult<Result.DescribeStagesResult> result = null;
			await DescribeStages(
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
		public DescribeStagesTask DescribeStagesAsync(
                Request.DescribeStagesRequest request
        )
		{
			return new DescribeStagesTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.DescribeStagesResult> DescribeStagesAsync(
                Request.DescribeStagesRequest request
        )
		{
			var task = new DescribeStagesTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class GetStageTask : Gs2RestSessionTask<GetStageRequest, GetStageResult>
        {
            public GetStageTask(IGs2Session session, RestSessionRequestFactory factory, GetStageRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(GetStageRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "freeze")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{stageName}";

                url = url.Replace("{stageName}", !string.IsNullOrEmpty(request.StageName) ? request.StageName.ToString() : "null");

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
		public IEnumerator GetStage(
                Request.GetStageRequest request,
                UnityAction<AsyncResult<Result.GetStageResult>> callback
        )
		{
			var task = new GetStageTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.GetStageResult>(task.Result, task.Error));
        }

		public IFuture<Result.GetStageResult> GetStageFuture(
                Request.GetStageRequest request
        )
		{
			return new GetStageTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.GetStageResult> GetStageAsync(
                Request.GetStageRequest request
        )
		{
            AsyncResult<Result.GetStageResult> result = null;
			await GetStage(
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
		public GetStageTask GetStageAsync(
                Request.GetStageRequest request
        )
		{
			return new GetStageTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.GetStageResult> GetStageAsync(
                Request.GetStageRequest request
        )
		{
			var task = new GetStageTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class PromoteStageTask : Gs2RestSessionTask<PromoteStageRequest, PromoteStageResult>
        {
            public PromoteStageTask(IGs2Session session, RestSessionRequestFactory factory, PromoteStageRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(PromoteStageRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "freeze")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{stageName}/promote";

                url = url.Replace("{stageName}", !string.IsNullOrEmpty(request.StageName) ? request.StageName.ToString() : "null");

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
		public IEnumerator PromoteStage(
                Request.PromoteStageRequest request,
                UnityAction<AsyncResult<Result.PromoteStageResult>> callback
        )
		{
			var task = new PromoteStageTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.PromoteStageResult>(task.Result, task.Error));
        }

		public IFuture<Result.PromoteStageResult> PromoteStageFuture(
                Request.PromoteStageRequest request
        )
		{
			return new PromoteStageTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.PromoteStageResult> PromoteStageAsync(
                Request.PromoteStageRequest request
        )
		{
            AsyncResult<Result.PromoteStageResult> result = null;
			await PromoteStage(
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
		public PromoteStageTask PromoteStageAsync(
                Request.PromoteStageRequest request
        )
		{
			return new PromoteStageTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.PromoteStageResult> PromoteStageAsync(
                Request.PromoteStageRequest request
        )
		{
			var task = new PromoteStageTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class RollbackStageTask : Gs2RestSessionTask<RollbackStageRequest, RollbackStageResult>
        {
            public RollbackStageTask(IGs2Session session, RestSessionRequestFactory factory, RollbackStageRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(RollbackStageRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "freeze")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{stageName}/rollback";

                url = url.Replace("{stageName}", !string.IsNullOrEmpty(request.StageName) ? request.StageName.ToString() : "null");

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
		public IEnumerator RollbackStage(
                Request.RollbackStageRequest request,
                UnityAction<AsyncResult<Result.RollbackStageResult>> callback
        )
		{
			var task = new RollbackStageTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.RollbackStageResult>(task.Result, task.Error));
        }

		public IFuture<Result.RollbackStageResult> RollbackStageFuture(
                Request.RollbackStageRequest request
        )
		{
			return new RollbackStageTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.RollbackStageResult> RollbackStageAsync(
                Request.RollbackStageRequest request
        )
		{
            AsyncResult<Result.RollbackStageResult> result = null;
			await RollbackStage(
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
		public RollbackStageTask RollbackStageAsync(
                Request.RollbackStageRequest request
        )
		{
			return new RollbackStageTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.RollbackStageResult> RollbackStageAsync(
                Request.RollbackStageRequest request
        )
		{
			var task = new RollbackStageTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class DescribeOutputsTask : Gs2RestSessionTask<DescribeOutputsRequest, DescribeOutputsResult>
        {
            public DescribeOutputsTask(IGs2Session session, RestSessionRequestFactory factory, DescribeOutputsRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DescribeOutputsRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "freeze")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{stageName}/progress/output";

                url = url.Replace("{stageName}", !string.IsNullOrEmpty(request.StageName) ? request.StageName.ToString() : "null");

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
		public IEnumerator DescribeOutputs(
                Request.DescribeOutputsRequest request,
                UnityAction<AsyncResult<Result.DescribeOutputsResult>> callback
        )
		{
			var task = new DescribeOutputsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.DescribeOutputsResult>(task.Result, task.Error));
        }

		public IFuture<Result.DescribeOutputsResult> DescribeOutputsFuture(
                Request.DescribeOutputsRequest request
        )
		{
			return new DescribeOutputsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.DescribeOutputsResult> DescribeOutputsAsync(
                Request.DescribeOutputsRequest request
        )
		{
            AsyncResult<Result.DescribeOutputsResult> result = null;
			await DescribeOutputs(
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
		public DescribeOutputsTask DescribeOutputsAsync(
                Request.DescribeOutputsRequest request
        )
		{
			return new DescribeOutputsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.DescribeOutputsResult> DescribeOutputsAsync(
                Request.DescribeOutputsRequest request
        )
		{
			var task = new DescribeOutputsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif


        public class GetOutputTask : Gs2RestSessionTask<GetOutputRequest, GetOutputResult>
        {
            public GetOutputTask(IGs2Session session, RestSessionRequestFactory factory, GetOutputRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(GetOutputRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "freeze")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/{stageName}/progress/output/{outputName}";

                url = url.Replace("{stageName}", !string.IsNullOrEmpty(request.StageName) ? request.StageName.ToString() : "null");
                url = url.Replace("{outputName}", !string.IsNullOrEmpty(request.OutputName) ? request.OutputName.ToString() : "null");

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
		public IEnumerator GetOutput(
                Request.GetOutputRequest request,
                UnityAction<AsyncResult<Result.GetOutputResult>> callback
        )
		{
			var task = new GetOutputTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
            yield return task;
            callback.Invoke(new AsyncResult<Result.GetOutputResult>(task.Result, task.Error));
        }

		public IFuture<Result.GetOutputResult> GetOutputFuture(
                Request.GetOutputRequest request
        )
		{
			return new GetOutputTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
			);
        }

    #if GS2_ENABLE_UNITASK
		public async UniTask<Result.GetOutputResult> GetOutputAsync(
                Request.GetOutputRequest request
        )
		{
            AsyncResult<Result.GetOutputResult> result = null;
			await GetOutput(
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
		public GetOutputTask GetOutputAsync(
                Request.GetOutputRequest request
        )
		{
			return new GetOutputTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public async Task<Result.GetOutputResult> GetOutputAsync(
                Request.GetOutputRequest request
        )
		{
			var task = new GetOutputTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
			    request
            );
			return await task.Invoke();
        }
#endif
	}
}