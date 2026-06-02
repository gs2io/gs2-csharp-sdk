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
using Gs2.Gs2Deploy.Request;
using Gs2.Gs2Deploy.Result;
using Gs2.Util.LitJson;

namespace Gs2.Gs2Deploy
{
	public class Gs2DeployRestClient : AbstractGs2Client
	{
#if UNITY_2017_1_OR_NEWER
		private readonly CertificateHandler _certificateHandler;
#endif

		public static string Endpoint = "deploy";

        protected Gs2RestSession Gs2RestSession => (Gs2RestSession) Gs2Session;

		public Gs2DeployRestClient(Gs2RestSession Gs2RestSession) : base(Gs2RestSession)
		{

		}

#if UNITY_2017_1_OR_NEWER
		public Gs2DeployRestClient(Gs2RestSession gs2RestSession, CertificateHandler certificateHandler) : base(gs2RestSession)
		{
			_certificateHandler = certificateHandler;
		}
#endif


        public class DescribeStacksTask : Gs2RestSessionTask<DescribeStacksRequest, DescribeStacksResult>
        {
            public DescribeStacksTask(IGs2Session session, RestSessionRequestFactory factory, DescribeStacksRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DescribeStacksRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "deploy")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/stack";

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
		public IEnumerator DescribeStacks(
                Request.DescribeStacksRequest request,
                UnityAction<AsyncResult<Result.DescribeStacksResult>> callback
        ) =>
            new DescribeStacksTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.DescribeStacksResult> DescribeStacksFuture(
                Request.DescribeStacksRequest request
        ) =>
            new DescribeStacksTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.DescribeStacksResult> DescribeStacksAsync(
                Request.DescribeStacksRequest request
        ) =>
            new DescribeStacksTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.DescribeStacksResult>();
    #else
		public DescribeStacksTask DescribeStacksAsync(
                Request.DescribeStacksRequest request
        )
		{
			return new DescribeStacksTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.DescribeStacksResult> DescribeStacksAsync(
                Request.DescribeStacksRequest request
        ) =>
            new DescribeStacksTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class PreCreateStackTask : Gs2RestSessionTask<PreCreateStackRequest, PreCreateStackResult>
        {
            public PreCreateStackTask(IGs2Session session, RestSessionRequestFactory factory, PreCreateStackRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(PreCreateStackRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "deploy")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/stack/pre";

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
		public IEnumerator PreCreateStack(
                Request.PreCreateStackRequest request,
                UnityAction<AsyncResult<Result.PreCreateStackResult>> callback
        ) =>
            new PreCreateStackTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.PreCreateStackResult> PreCreateStackFuture(
                Request.PreCreateStackRequest request
        ) =>
            new PreCreateStackTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.PreCreateStackResult> PreCreateStackAsync(
                Request.PreCreateStackRequest request
        ) =>
            new PreCreateStackTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.PreCreateStackResult>();
    #else
		public PreCreateStackTask PreCreateStackAsync(
                Request.PreCreateStackRequest request
        )
		{
			return new PreCreateStackTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.PreCreateStackResult> PreCreateStackAsync(
                Request.PreCreateStackRequest request
        ) =>
            new PreCreateStackTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class CreateStackTask : Gs2RestSessionTask<CreateStackRequest, CreateStackResult>
        {
            public CreateStackTask(IGs2Session session, RestSessionRequestFactory factory, CreateStackRequest request) : base(session, factory, request)
            {
            }
#if GS2_ENABLE_UNITASK
            protected override async UniTask<CreateStackResult> InvokeImpl()
#else
            protected override async Task<CreateStackResult> InvokeImpl()
#endif
            {
                if (Request.Template != null) {
                    var preTask = new PreCreateStackTask(
                        Session,
                        Factory,
                        new PreCreateStackRequest()
                            .WithContextStack(Request.ContextStack)
                    );
                    var preTaskResult = await preTask.Invoke();
#if UNITY_2017_1_OR_NEWER
                    using (var request = UnityWebRequest.Put(preTaskResult.UploadUrl, Request.Template))
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
                        httpRequestMessage.Content = new ByteArrayContent(System.Text.Encoding.UTF8.GetBytes(Request.Template));
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
                    Request.Template = null;
                }
                return await base.InvokeImpl();
            }

            protected override IGs2SessionRequest CreateRequest(CreateStackRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "deploy")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/stack";

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
                if (request.Mode != null)
                {
                    jsonWriter.WritePropertyName("mode");
                    jsonWriter.Write(request.Mode);
                }
                if (request.Template != null)
                {
                    jsonWriter.WritePropertyName("template");
                    jsonWriter.Write(request.Template);
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
		public IEnumerator CreateStack(
                Request.CreateStackRequest request,
                UnityAction<AsyncResult<Result.CreateStackResult>> callback
        ) =>
            new CreateStackTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.CreateStackResult> CreateStackFuture(
                Request.CreateStackRequest request
        ) =>
            new CreateStackTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.CreateStackResult> CreateStackAsync(
                Request.CreateStackRequest request
        ) =>
            new CreateStackTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.CreateStackResult>();
    #else
		public CreateStackTask CreateStackAsync(
                Request.CreateStackRequest request
        )
		{
			return new CreateStackTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.CreateStackResult> CreateStackAsync(
                Request.CreateStackRequest request
        ) =>
            new CreateStackTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class CreateStackFromGitHubTask : Gs2RestSessionTask<CreateStackFromGitHubRequest, CreateStackFromGitHubResult>
        {
            public CreateStackFromGitHubTask(IGs2Session session, RestSessionRequestFactory factory, CreateStackFromGitHubRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(CreateStackFromGitHubRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "deploy")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/stack/from_git_hub";

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
		public IEnumerator CreateStackFromGitHub(
                Request.CreateStackFromGitHubRequest request,
                UnityAction<AsyncResult<Result.CreateStackFromGitHubResult>> callback
        ) =>
            new CreateStackFromGitHubTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.CreateStackFromGitHubResult> CreateStackFromGitHubFuture(
                Request.CreateStackFromGitHubRequest request
        ) =>
            new CreateStackFromGitHubTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.CreateStackFromGitHubResult> CreateStackFromGitHubAsync(
                Request.CreateStackFromGitHubRequest request
        ) =>
            new CreateStackFromGitHubTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.CreateStackFromGitHubResult>();
    #else
		public CreateStackFromGitHubTask CreateStackFromGitHubAsync(
                Request.CreateStackFromGitHubRequest request
        )
		{
			return new CreateStackFromGitHubTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.CreateStackFromGitHubResult> CreateStackFromGitHubAsync(
                Request.CreateStackFromGitHubRequest request
        ) =>
            new CreateStackFromGitHubTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class PreValidateTask : Gs2RestSessionTask<PreValidateRequest, PreValidateResult>
        {
            public PreValidateTask(IGs2Session session, RestSessionRequestFactory factory, PreValidateRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(PreValidateRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "deploy")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/stack/validate/pre";

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
		public IEnumerator PreValidate(
                Request.PreValidateRequest request,
                UnityAction<AsyncResult<Result.PreValidateResult>> callback
        ) =>
            new PreValidateTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.PreValidateResult> PreValidateFuture(
                Request.PreValidateRequest request
        ) =>
            new PreValidateTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.PreValidateResult> PreValidateAsync(
                Request.PreValidateRequest request
        ) =>
            new PreValidateTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.PreValidateResult>();
    #else
		public PreValidateTask PreValidateAsync(
                Request.PreValidateRequest request
        )
		{
			return new PreValidateTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.PreValidateResult> PreValidateAsync(
                Request.PreValidateRequest request
        ) =>
            new PreValidateTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class ValidateTask : Gs2RestSessionTask<ValidateRequest, ValidateResult>
        {
            public ValidateTask(IGs2Session session, RestSessionRequestFactory factory, ValidateRequest request) : base(session, factory, request)
            {
            }
#if GS2_ENABLE_UNITASK
            protected override async UniTask<ValidateResult> InvokeImpl()
#else
            protected override async Task<ValidateResult> InvokeImpl()
#endif
            {
                if (Request.Template != null) {
                    var preTask = new PreValidateTask(
                        Session,
                        Factory,
                        new PreValidateRequest()
                            .WithContextStack(Request.ContextStack)
                    );
                    var preTaskResult = await preTask.Invoke();
#if UNITY_2017_1_OR_NEWER
                    using (var request = UnityWebRequest.Put(preTaskResult.UploadUrl, Request.Template))
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
                        httpRequestMessage.Content = new ByteArrayContent(System.Text.Encoding.UTF8.GetBytes(Request.Template));
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
                    Request.Template = null;
                }
                return await base.InvokeImpl();
            }

            protected override IGs2SessionRequest CreateRequest(ValidateRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "deploy")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/stack/validate";

                var sessionRequest = Factory.Post(url);

                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);
                jsonWriter.WriteObjectStart();
                if (request.Mode != null)
                {
                    jsonWriter.WritePropertyName("mode");
                    jsonWriter.Write(request.Mode);
                }
                if (request.Template != null)
                {
                    jsonWriter.WritePropertyName("template");
                    jsonWriter.Write(request.Template);
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
		public IEnumerator Validate(
                Request.ValidateRequest request,
                UnityAction<AsyncResult<Result.ValidateResult>> callback
        ) =>
            new ValidateTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.ValidateResult> ValidateFuture(
                Request.ValidateRequest request
        ) =>
            new ValidateTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.ValidateResult> ValidateAsync(
                Request.ValidateRequest request
        ) =>
            new ValidateTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.ValidateResult>();
    #else
		public ValidateTask ValidateAsync(
                Request.ValidateRequest request
        )
		{
			return new ValidateTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.ValidateResult> ValidateAsync(
                Request.ValidateRequest request
        ) =>
            new ValidateTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class GetStackStatusTask : Gs2RestSessionTask<GetStackStatusRequest, GetStackStatusResult>
        {
            public GetStackStatusTask(IGs2Session session, RestSessionRequestFactory factory, GetStackStatusRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(GetStackStatusRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "deploy")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/stack/{stackName}/status";

                url = url.Replace("{stackName}", !string.IsNullOrEmpty(request.StackName) ? request.StackName.ToString() : "null");

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
		public IEnumerator GetStackStatus(
                Request.GetStackStatusRequest request,
                UnityAction<AsyncResult<Result.GetStackStatusResult>> callback
        ) =>
            new GetStackStatusTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.GetStackStatusResult> GetStackStatusFuture(
                Request.GetStackStatusRequest request
        ) =>
            new GetStackStatusTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.GetStackStatusResult> GetStackStatusAsync(
                Request.GetStackStatusRequest request
        ) =>
            new GetStackStatusTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.GetStackStatusResult>();
    #else
		public GetStackStatusTask GetStackStatusAsync(
                Request.GetStackStatusRequest request
        )
		{
			return new GetStackStatusTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.GetStackStatusResult> GetStackStatusAsync(
                Request.GetStackStatusRequest request
        ) =>
            new GetStackStatusTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class GetStackTask : Gs2RestSessionTask<GetStackRequest, GetStackResult>
        {
            public GetStackTask(IGs2Session session, RestSessionRequestFactory factory, GetStackRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(GetStackRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "deploy")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/stack/{stackName}";

                url = url.Replace("{stackName}", !string.IsNullOrEmpty(request.StackName) ? request.StackName.ToString() : "null");

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
		public IEnumerator GetStack(
                Request.GetStackRequest request,
                UnityAction<AsyncResult<Result.GetStackResult>> callback
        ) =>
            new GetStackTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.GetStackResult> GetStackFuture(
                Request.GetStackRequest request
        ) =>
            new GetStackTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.GetStackResult> GetStackAsync(
                Request.GetStackRequest request
        ) =>
            new GetStackTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.GetStackResult>();
    #else
		public GetStackTask GetStackAsync(
                Request.GetStackRequest request
        )
		{
			return new GetStackTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.GetStackResult> GetStackAsync(
                Request.GetStackRequest request
        ) =>
            new GetStackTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class PreUpdateStackTask : Gs2RestSessionTask<PreUpdateStackRequest, PreUpdateStackResult>
        {
            public PreUpdateStackTask(IGs2Session session, RestSessionRequestFactory factory, PreUpdateStackRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(PreUpdateStackRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "deploy")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/stack/{stackName}/pre";

                url = url.Replace("{stackName}", !string.IsNullOrEmpty(request.StackName) ? request.StackName.ToString() : "null");

                var sessionRequest = Factory.Put(url);

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
		public IEnumerator PreUpdateStack(
                Request.PreUpdateStackRequest request,
                UnityAction<AsyncResult<Result.PreUpdateStackResult>> callback
        ) =>
            new PreUpdateStackTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.PreUpdateStackResult> PreUpdateStackFuture(
                Request.PreUpdateStackRequest request
        ) =>
            new PreUpdateStackTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.PreUpdateStackResult> PreUpdateStackAsync(
                Request.PreUpdateStackRequest request
        ) =>
            new PreUpdateStackTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.PreUpdateStackResult>();
    #else
		public PreUpdateStackTask PreUpdateStackAsync(
                Request.PreUpdateStackRequest request
        )
		{
			return new PreUpdateStackTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.PreUpdateStackResult> PreUpdateStackAsync(
                Request.PreUpdateStackRequest request
        ) =>
            new PreUpdateStackTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class UpdateStackTask : Gs2RestSessionTask<UpdateStackRequest, UpdateStackResult>
        {
            public UpdateStackTask(IGs2Session session, RestSessionRequestFactory factory, UpdateStackRequest request) : base(session, factory, request)
            {
            }
#if GS2_ENABLE_UNITASK
            protected override async UniTask<UpdateStackResult> InvokeImpl()
#else
            protected override async Task<UpdateStackResult> InvokeImpl()
#endif
            {
                if (Request.Template != null) {
                    var preTask = new PreUpdateStackTask(
                        Session,
                        Factory,
                        new PreUpdateStackRequest()
                            .WithContextStack(Request.ContextStack)
                            .WithStackName(Request.StackName)
                    );
                    var preTaskResult = await preTask.Invoke();
#if UNITY_2017_1_OR_NEWER
                    using (var request = UnityWebRequest.Put(preTaskResult.UploadUrl, Request.Template))
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
                        httpRequestMessage.Content = new ByteArrayContent(System.Text.Encoding.UTF8.GetBytes(Request.Template));
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
                    Request.Template = null;
                }
                return await base.InvokeImpl();
            }

            protected override IGs2SessionRequest CreateRequest(UpdateStackRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "deploy")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/stack/{stackName}";

                url = url.Replace("{stackName}", !string.IsNullOrEmpty(request.StackName) ? request.StackName.ToString() : "null");

                var sessionRequest = Factory.Put(url);

                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);
                jsonWriter.WriteObjectStart();
                if (request.Description != null)
                {
                    jsonWriter.WritePropertyName("description");
                    jsonWriter.Write(request.Description);
                }
                if (request.Mode != null)
                {
                    jsonWriter.WritePropertyName("mode");
                    jsonWriter.Write(request.Mode);
                }
                if (request.Template != null)
                {
                    jsonWriter.WritePropertyName("template");
                    jsonWriter.Write(request.Template);
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
		public IEnumerator UpdateStack(
                Request.UpdateStackRequest request,
                UnityAction<AsyncResult<Result.UpdateStackResult>> callback
        ) =>
            new UpdateStackTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.UpdateStackResult> UpdateStackFuture(
                Request.UpdateStackRequest request
        ) =>
            new UpdateStackTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.UpdateStackResult> UpdateStackAsync(
                Request.UpdateStackRequest request
        ) =>
            new UpdateStackTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.UpdateStackResult>();
    #else
		public UpdateStackTask UpdateStackAsync(
                Request.UpdateStackRequest request
        )
		{
			return new UpdateStackTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.UpdateStackResult> UpdateStackAsync(
                Request.UpdateStackRequest request
        ) =>
            new UpdateStackTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class PreChangeSetTask : Gs2RestSessionTask<PreChangeSetRequest, PreChangeSetResult>
        {
            public PreChangeSetTask(IGs2Session session, RestSessionRequestFactory factory, PreChangeSetRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(PreChangeSetRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "deploy")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/stack/{stackName}/pre";

                url = url.Replace("{stackName}", !string.IsNullOrEmpty(request.StackName) ? request.StackName.ToString() : "null");

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
		public IEnumerator PreChangeSet(
                Request.PreChangeSetRequest request,
                UnityAction<AsyncResult<Result.PreChangeSetResult>> callback
        ) =>
            new PreChangeSetTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.PreChangeSetResult> PreChangeSetFuture(
                Request.PreChangeSetRequest request
        ) =>
            new PreChangeSetTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.PreChangeSetResult> PreChangeSetAsync(
                Request.PreChangeSetRequest request
        ) =>
            new PreChangeSetTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.PreChangeSetResult>();
    #else
		public PreChangeSetTask PreChangeSetAsync(
                Request.PreChangeSetRequest request
        )
		{
			return new PreChangeSetTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.PreChangeSetResult> PreChangeSetAsync(
                Request.PreChangeSetRequest request
        ) =>
            new PreChangeSetTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class ChangeSetTask : Gs2RestSessionTask<ChangeSetRequest, ChangeSetResult>
        {
            public ChangeSetTask(IGs2Session session, RestSessionRequestFactory factory, ChangeSetRequest request) : base(session, factory, request)
            {
            }
#if GS2_ENABLE_UNITASK
            protected override async UniTask<ChangeSetResult> InvokeImpl()
#else
            protected override async Task<ChangeSetResult> InvokeImpl()
#endif
            {
                if (Request.Template != null) {
                    var preTask = new PreChangeSetTask(
                        Session,
                        Factory,
                        new PreChangeSetRequest()
                            .WithContextStack(Request.ContextStack)
                            .WithStackName(Request.StackName)
                    );
                    var preTaskResult = await preTask.Invoke();
#if UNITY_2017_1_OR_NEWER
                    using (var request = UnityWebRequest.Put(preTaskResult.UploadUrl, Request.Template))
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
                        httpRequestMessage.Content = new ByteArrayContent(System.Text.Encoding.UTF8.GetBytes(Request.Template));
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
                    Request.Template = null;
                }
                return await base.InvokeImpl();
            }

            protected override IGs2SessionRequest CreateRequest(ChangeSetRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "deploy")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/stack/{stackName}";

                url = url.Replace("{stackName}", !string.IsNullOrEmpty(request.StackName) ? request.StackName.ToString() : "null");

                var sessionRequest = Factory.Post(url);

                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);
                jsonWriter.WriteObjectStart();
                if (request.Mode != null)
                {
                    jsonWriter.WritePropertyName("mode");
                    jsonWriter.Write(request.Mode);
                }
                if (request.Template != null)
                {
                    jsonWriter.WritePropertyName("template");
                    jsonWriter.Write(request.Template);
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
		public IEnumerator ChangeSet(
                Request.ChangeSetRequest request,
                UnityAction<AsyncResult<Result.ChangeSetResult>> callback
        ) =>
            new ChangeSetTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.ChangeSetResult> ChangeSetFuture(
                Request.ChangeSetRequest request
        ) =>
            new ChangeSetTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.ChangeSetResult> ChangeSetAsync(
                Request.ChangeSetRequest request
        ) =>
            new ChangeSetTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.ChangeSetResult>();
    #else
		public ChangeSetTask ChangeSetAsync(
                Request.ChangeSetRequest request
        )
		{
			return new ChangeSetTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.ChangeSetResult> ChangeSetAsync(
                Request.ChangeSetRequest request
        ) =>
            new ChangeSetTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class UpdateStackFromGitHubTask : Gs2RestSessionTask<UpdateStackFromGitHubRequest, UpdateStackFromGitHubResult>
        {
            public UpdateStackFromGitHubTask(IGs2Session session, RestSessionRequestFactory factory, UpdateStackFromGitHubRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(UpdateStackFromGitHubRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "deploy")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/stack/{stackName}/from_git_hub";

                url = url.Replace("{stackName}", !string.IsNullOrEmpty(request.StackName) ? request.StackName.ToString() : "null");

                var sessionRequest = Factory.Put(url);

                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);
                jsonWriter.WriteObjectStart();
                if (request.Description != null)
                {
                    jsonWriter.WritePropertyName("description");
                    jsonWriter.Write(request.Description);
                }
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
		public IEnumerator UpdateStackFromGitHub(
                Request.UpdateStackFromGitHubRequest request,
                UnityAction<AsyncResult<Result.UpdateStackFromGitHubResult>> callback
        ) =>
            new UpdateStackFromGitHubTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.UpdateStackFromGitHubResult> UpdateStackFromGitHubFuture(
                Request.UpdateStackFromGitHubRequest request
        ) =>
            new UpdateStackFromGitHubTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.UpdateStackFromGitHubResult> UpdateStackFromGitHubAsync(
                Request.UpdateStackFromGitHubRequest request
        ) =>
            new UpdateStackFromGitHubTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.UpdateStackFromGitHubResult>();
    #else
		public UpdateStackFromGitHubTask UpdateStackFromGitHubAsync(
                Request.UpdateStackFromGitHubRequest request
        )
		{
			return new UpdateStackFromGitHubTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.UpdateStackFromGitHubResult> UpdateStackFromGitHubAsync(
                Request.UpdateStackFromGitHubRequest request
        ) =>
            new UpdateStackFromGitHubTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class DeleteStackTask : Gs2RestSessionTask<DeleteStackRequest, DeleteStackResult>
        {
            public DeleteStackTask(IGs2Session session, RestSessionRequestFactory factory, DeleteStackRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DeleteStackRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "deploy")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/stack/{stackName}";

                url = url.Replace("{stackName}", !string.IsNullOrEmpty(request.StackName) ? request.StackName.ToString() : "null");

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
		public IEnumerator DeleteStack(
                Request.DeleteStackRequest request,
                UnityAction<AsyncResult<Result.DeleteStackResult>> callback
        ) =>
            new DeleteStackTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.DeleteStackResult> DeleteStackFuture(
                Request.DeleteStackRequest request
        ) =>
            new DeleteStackTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.DeleteStackResult> DeleteStackAsync(
                Request.DeleteStackRequest request
        ) =>
            new DeleteStackTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.DeleteStackResult>();
    #else
		public DeleteStackTask DeleteStackAsync(
                Request.DeleteStackRequest request
        )
		{
			return new DeleteStackTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.DeleteStackResult> DeleteStackAsync(
                Request.DeleteStackRequest request
        ) =>
            new DeleteStackTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class ForceDeleteStackTask : Gs2RestSessionTask<ForceDeleteStackRequest, ForceDeleteStackResult>
        {
            public ForceDeleteStackTask(IGs2Session session, RestSessionRequestFactory factory, ForceDeleteStackRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(ForceDeleteStackRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "deploy")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/stack/{stackName}/force";

                url = url.Replace("{stackName}", !string.IsNullOrEmpty(request.StackName) ? request.StackName.ToString() : "null");

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
		public IEnumerator ForceDeleteStack(
                Request.ForceDeleteStackRequest request,
                UnityAction<AsyncResult<Result.ForceDeleteStackResult>> callback
        ) =>
            new ForceDeleteStackTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.ForceDeleteStackResult> ForceDeleteStackFuture(
                Request.ForceDeleteStackRequest request
        ) =>
            new ForceDeleteStackTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.ForceDeleteStackResult> ForceDeleteStackAsync(
                Request.ForceDeleteStackRequest request
        ) =>
            new ForceDeleteStackTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.ForceDeleteStackResult>();
    #else
		public ForceDeleteStackTask ForceDeleteStackAsync(
                Request.ForceDeleteStackRequest request
        )
		{
			return new ForceDeleteStackTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.ForceDeleteStackResult> ForceDeleteStackAsync(
                Request.ForceDeleteStackRequest request
        ) =>
            new ForceDeleteStackTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class DeleteStackResourcesTask : Gs2RestSessionTask<DeleteStackResourcesRequest, DeleteStackResourcesResult>
        {
            public DeleteStackResourcesTask(IGs2Session session, RestSessionRequestFactory factory, DeleteStackResourcesRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DeleteStackResourcesRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "deploy")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/stack/{stackName}/resources";

                url = url.Replace("{stackName}", !string.IsNullOrEmpty(request.StackName) ? request.StackName.ToString() : "null");

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
		public IEnumerator DeleteStackResources(
                Request.DeleteStackResourcesRequest request,
                UnityAction<AsyncResult<Result.DeleteStackResourcesResult>> callback
        ) =>
            new DeleteStackResourcesTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.DeleteStackResourcesResult> DeleteStackResourcesFuture(
                Request.DeleteStackResourcesRequest request
        ) =>
            new DeleteStackResourcesTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.DeleteStackResourcesResult> DeleteStackResourcesAsync(
                Request.DeleteStackResourcesRequest request
        ) =>
            new DeleteStackResourcesTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.DeleteStackResourcesResult>();
    #else
		public DeleteStackResourcesTask DeleteStackResourcesAsync(
                Request.DeleteStackResourcesRequest request
        )
		{
			return new DeleteStackResourcesTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.DeleteStackResourcesResult> DeleteStackResourcesAsync(
                Request.DeleteStackResourcesRequest request
        ) =>
            new DeleteStackResourcesTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class DeleteStackEntityTask : Gs2RestSessionTask<DeleteStackEntityRequest, DeleteStackEntityResult>
        {
            public DeleteStackEntityTask(IGs2Session session, RestSessionRequestFactory factory, DeleteStackEntityRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DeleteStackEntityRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "deploy")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/stack/{stackName}/entity";

                url = url.Replace("{stackName}", !string.IsNullOrEmpty(request.StackName) ? request.StackName.ToString() : "null");

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
		public IEnumerator DeleteStackEntity(
                Request.DeleteStackEntityRequest request,
                UnityAction<AsyncResult<Result.DeleteStackEntityResult>> callback
        ) =>
            new DeleteStackEntityTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.DeleteStackEntityResult> DeleteStackEntityFuture(
                Request.DeleteStackEntityRequest request
        ) =>
            new DeleteStackEntityTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.DeleteStackEntityResult> DeleteStackEntityAsync(
                Request.DeleteStackEntityRequest request
        ) =>
            new DeleteStackEntityTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.DeleteStackEntityResult>();
    #else
		public DeleteStackEntityTask DeleteStackEntityAsync(
                Request.DeleteStackEntityRequest request
        )
		{
			return new DeleteStackEntityTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.DeleteStackEntityResult> DeleteStackEntityAsync(
                Request.DeleteStackEntityRequest request
        ) =>
            new DeleteStackEntityTask(
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
                    .Replace("{service}", "deploy")
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


        public class DescribeResourcesTask : Gs2RestSessionTask<DescribeResourcesRequest, DescribeResourcesResult>
        {
            public DescribeResourcesTask(IGs2Session session, RestSessionRequestFactory factory, DescribeResourcesRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DescribeResourcesRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "deploy")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/stack/{stackName}/resource";

                url = url.Replace("{stackName}", !string.IsNullOrEmpty(request.StackName) ? request.StackName.ToString() : "null");

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
		public IEnumerator DescribeResources(
                Request.DescribeResourcesRequest request,
                UnityAction<AsyncResult<Result.DescribeResourcesResult>> callback
        ) =>
            new DescribeResourcesTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.DescribeResourcesResult> DescribeResourcesFuture(
                Request.DescribeResourcesRequest request
        ) =>
            new DescribeResourcesTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.DescribeResourcesResult> DescribeResourcesAsync(
                Request.DescribeResourcesRequest request
        ) =>
            new DescribeResourcesTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.DescribeResourcesResult>();
    #else
		public DescribeResourcesTask DescribeResourcesAsync(
                Request.DescribeResourcesRequest request
        )
		{
			return new DescribeResourcesTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.DescribeResourcesResult> DescribeResourcesAsync(
                Request.DescribeResourcesRequest request
        ) =>
            new DescribeResourcesTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class GetResourceTask : Gs2RestSessionTask<GetResourceRequest, GetResourceResult>
        {
            public GetResourceTask(IGs2Session session, RestSessionRequestFactory factory, GetResourceRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(GetResourceRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "deploy")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/stack/{stackName}/resource/{resourceName}";

                url = url.Replace("{stackName}", !string.IsNullOrEmpty(request.StackName) ? request.StackName.ToString() : "null");
                url = url.Replace("{resourceName}", !string.IsNullOrEmpty(request.ResourceName) ? request.ResourceName.ToString() : "null");

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
		public IEnumerator GetResource(
                Request.GetResourceRequest request,
                UnityAction<AsyncResult<Result.GetResourceResult>> callback
        ) =>
            new GetResourceTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.GetResourceResult> GetResourceFuture(
                Request.GetResourceRequest request
        ) =>
            new GetResourceTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.GetResourceResult> GetResourceAsync(
                Request.GetResourceRequest request
        ) =>
            new GetResourceTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.GetResourceResult>();
    #else
		public GetResourceTask GetResourceAsync(
                Request.GetResourceRequest request
        )
		{
			return new GetResourceTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.GetResourceResult> GetResourceAsync(
                Request.GetResourceRequest request
        ) =>
            new GetResourceTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class DescribeEventsTask : Gs2RestSessionTask<DescribeEventsRequest, DescribeEventsResult>
        {
            public DescribeEventsTask(IGs2Session session, RestSessionRequestFactory factory, DescribeEventsRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DescribeEventsRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "deploy")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/stack/{stackName}/event";

                url = url.Replace("{stackName}", !string.IsNullOrEmpty(request.StackName) ? request.StackName.ToString() : "null");

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
		public IEnumerator DescribeEvents(
                Request.DescribeEventsRequest request,
                UnityAction<AsyncResult<Result.DescribeEventsResult>> callback
        ) =>
            new DescribeEventsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.DescribeEventsResult> DescribeEventsFuture(
                Request.DescribeEventsRequest request
        ) =>
            new DescribeEventsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.DescribeEventsResult> DescribeEventsAsync(
                Request.DescribeEventsRequest request
        ) =>
            new DescribeEventsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.DescribeEventsResult>();
    #else
		public DescribeEventsTask DescribeEventsAsync(
                Request.DescribeEventsRequest request
        )
		{
			return new DescribeEventsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.DescribeEventsResult> DescribeEventsAsync(
                Request.DescribeEventsRequest request
        ) =>
            new DescribeEventsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class GetEventTask : Gs2RestSessionTask<GetEventRequest, GetEventResult>
        {
            public GetEventTask(IGs2Session session, RestSessionRequestFactory factory, GetEventRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(GetEventRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "deploy")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/stack/{stackName}/event/{eventName}";

                url = url.Replace("{stackName}", !string.IsNullOrEmpty(request.StackName) ? request.StackName.ToString() : "null");
                url = url.Replace("{eventName}", !string.IsNullOrEmpty(request.EventName) ? request.EventName.ToString() : "null");

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
		public IEnumerator GetEvent(
                Request.GetEventRequest request,
                UnityAction<AsyncResult<Result.GetEventResult>> callback
        ) =>
            new GetEventTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.GetEventResult> GetEventFuture(
                Request.GetEventRequest request
        ) =>
            new GetEventTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.GetEventResult> GetEventAsync(
                Request.GetEventRequest request
        ) =>
            new GetEventTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.GetEventResult>();
    #else
		public GetEventTask GetEventAsync(
                Request.GetEventRequest request
        )
		{
			return new GetEventTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
			    request
            );
        }
    #endif
#else
		public Task<Result.GetEventResult> GetEventAsync(
                Request.GetEventRequest request
        ) =>
            new GetEventTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class DescribeOutputsTask : Gs2RestSessionTask<DescribeOutputsRequest, DescribeOutputsResult>
        {
            public DescribeOutputsTask(IGs2Session session, RestSessionRequestFactory factory, DescribeOutputsRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(DescribeOutputsRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "deploy")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/stack/{stackName}/output";

                url = url.Replace("{stackName}", !string.IsNullOrEmpty(request.StackName) ? request.StackName.ToString() : "null");

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
        ) =>
            new DescribeOutputsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.DescribeOutputsResult> DescribeOutputsFuture(
                Request.DescribeOutputsRequest request
        ) =>
            new DescribeOutputsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.DescribeOutputsResult> DescribeOutputsAsync(
                Request.DescribeOutputsRequest request
        ) =>
            new DescribeOutputsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.DescribeOutputsResult>();
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
		public Task<Result.DescribeOutputsResult> DescribeOutputsAsync(
                Request.DescribeOutputsRequest request
        ) =>
            new DescribeOutputsTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif


        public class GetOutputTask : Gs2RestSessionTask<GetOutputRequest, GetOutputResult>
        {
            public GetOutputTask(IGs2Session session, RestSessionRequestFactory factory, GetOutputRequest request) : base(session, factory, request)
            {
            }

            protected override IGs2SessionRequest CreateRequest(GetOutputRequest request)
            {
                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "deploy")
                    .Replace("{region}", Session.Region.DisplayName())
                    + "/stack/{stackName}/output/{outputName}";

                url = url.Replace("{stackName}", !string.IsNullOrEmpty(request.StackName) ? request.StackName.ToString() : "null");
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
        ) =>
            new GetOutputTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToCoroutine(callback);

		public IFuture<Result.GetOutputResult> GetOutputFuture(
                Request.GetOutputRequest request
        ) =>
            new GetOutputTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().ToGs2Future();

    #if GS2_ENABLE_UNITASK
		public UniTask<Result.GetOutputResult> GetOutputAsync(
                Request.GetOutputRequest request
        ) =>
            new GetOutputTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_certificateHandler)),
                request
            ).Invoke().AsUniTask<Result.GetOutputResult>();
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
		public Task<Result.GetOutputResult> GetOutputAsync(
                Request.GetOutputRequest request
        ) =>
            new GetOutputTask(
                Gs2RestSession,
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
                request
            ).Invoke().AsTask();
#endif
	}
}