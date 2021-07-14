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
using Gs2.Util.LitJson;namespace Gs2.Gs2Enhance
{
	public class Gs2EnhanceRestClient : AbstractGs2Client
	{
		private readonly CertificateHandler _certificateHandler;

		public static string Endpoint = "enhance";

        protected Gs2RestSession Gs2RestSession => (Gs2RestSession) Gs2Session;

		/// <summary>
		/// コンストラクタ。
		/// </summary>
		/// <param name="Gs2RestSession">REST API 用セッション</param>
		public Gs2EnhanceRestClient(Gs2RestSession Gs2RestSession) : base(Gs2RestSession)
		{

		}

		/// <summary>
		/// コンストラクタ。
		/// </summary>
		/// <param name="gs2RestSession">REST API 用セッション</param>
		/// <param name="certificateHandler"></param>
		public Gs2EnhanceRestClient(Gs2RestSession gs2RestSession, CertificateHandler certificateHandler) : base(gs2RestSession)
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
                    .Replace("{service}", "enhance")
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
                    .Replace("{service}", "enhance")
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
                if (_request.EnableDirectEnhance != null)
                {
                    jsonWriter.WritePropertyName("enableDirectEnhance");
                    jsonWriter.Write(_request.EnableDirectEnhance.ToString());
                }
                if (_request.QueueNamespaceId != null)
                {
                    jsonWriter.WritePropertyName("queueNamespaceId");
                    jsonWriter.Write(_request.QueueNamespaceId.ToString());
                }
                if (_request.KeyId != null)
                {
                    jsonWriter.WritePropertyName("keyId");
                    jsonWriter.Write(_request.KeyId.ToString());
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
                    .Replace("{service}", "enhance")
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
                    .Replace("{service}", "enhance")
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
                    .Replace("{service}", "enhance")
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
                if (_request.EnableDirectEnhance != null)
                {
                    jsonWriter.WritePropertyName("enableDirectEnhance");
                    jsonWriter.Write(_request.EnableDirectEnhance.ToString());
                }
                if (_request.QueueNamespaceId != null)
                {
                    jsonWriter.WritePropertyName("queueNamespaceId");
                    jsonWriter.Write(_request.QueueNamespaceId.ToString());
                }
                if (_request.KeyId != null)
                {
                    jsonWriter.WritePropertyName("keyId");
                    jsonWriter.Write(_request.KeyId.ToString());
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
                    .Replace("{service}", "enhance")
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

        private class DescribeRateModelsTask : Gs2RestSessionTask<Result.DescribeRateModelsResult>
        {
			private readonly Request.DescribeRateModelsRequest _request;

			public DescribeRateModelsTask(Request.DescribeRateModelsRequest request, UnityAction<AsyncResult<Result.DescribeRateModelsResult>> userCallback) : base(userCallback)
			{
				_request = request;
			}

            protected override IEnumerator ExecuteImpl(Gs2Session gs2Session)
            {
				UnityWebRequest.method = UnityWebRequest.kHttpVerbGET;

                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "enhance")
                    .Replace("{region}", gs2Session.Region.DisplayName())
                    + "/{namespaceName}/model";

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
		public IEnumerator DescribeRateModels(
                Request.DescribeRateModelsRequest request,
                UnityAction<AsyncResult<Result.DescribeRateModelsResult>> callback
        )
		{
			var task = new DescribeRateModelsTask(request, callback);
			if (_certificateHandler != null)
			{
				task.UnityWebRequest.certificateHandler = _certificateHandler;
			}
			return Gs2RestSession.Execute(task);
        }

        private class GetRateModelTask : Gs2RestSessionTask<Result.GetRateModelResult>
        {
			private readonly Request.GetRateModelRequest _request;

			public GetRateModelTask(Request.GetRateModelRequest request, UnityAction<AsyncResult<Result.GetRateModelResult>> userCallback) : base(userCallback)
			{
				_request = request;
			}

            protected override IEnumerator ExecuteImpl(Gs2Session gs2Session)
            {
				UnityWebRequest.method = UnityWebRequest.kHttpVerbGET;

                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "enhance")
                    .Replace("{region}", gs2Session.Region.DisplayName())
                    + "/{namespaceName}/model/{rateName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(_request.NamespaceName) ? _request.NamespaceName.ToString() : "null");
                url = url.Replace("{rateName}", !string.IsNullOrEmpty(_request.RateName) ? _request.RateName.ToString() : "null");

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
		public IEnumerator GetRateModel(
                Request.GetRateModelRequest request,
                UnityAction<AsyncResult<Result.GetRateModelResult>> callback
        )
		{
			var task = new GetRateModelTask(request, callback);
			if (_certificateHandler != null)
			{
				task.UnityWebRequest.certificateHandler = _certificateHandler;
			}
			return Gs2RestSession.Execute(task);
        }

        private class DescribeRateModelMastersTask : Gs2RestSessionTask<Result.DescribeRateModelMastersResult>
        {
			private readonly Request.DescribeRateModelMastersRequest _request;

			public DescribeRateModelMastersTask(Request.DescribeRateModelMastersRequest request, UnityAction<AsyncResult<Result.DescribeRateModelMastersResult>> userCallback) : base(userCallback)
			{
				_request = request;
			}

            protected override IEnumerator ExecuteImpl(Gs2Session gs2Session)
            {
				UnityWebRequest.method = UnityWebRequest.kHttpVerbGET;

                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "enhance")
                    .Replace("{region}", gs2Session.Region.DisplayName())
                    + "/{namespaceName}/master/model";

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
		public IEnumerator DescribeRateModelMasters(
                Request.DescribeRateModelMastersRequest request,
                UnityAction<AsyncResult<Result.DescribeRateModelMastersResult>> callback
        )
		{
			var task = new DescribeRateModelMastersTask(request, callback);
			if (_certificateHandler != null)
			{
				task.UnityWebRequest.certificateHandler = _certificateHandler;
			}
			return Gs2RestSession.Execute(task);
        }

        private class CreateRateModelMasterTask : Gs2RestSessionTask<Result.CreateRateModelMasterResult>
        {
			private readonly Request.CreateRateModelMasterRequest _request;

			public CreateRateModelMasterTask(Request.CreateRateModelMasterRequest request, UnityAction<AsyncResult<Result.CreateRateModelMasterResult>> userCallback) : base(userCallback)
			{
				_request = request;
			}

            protected override IEnumerator ExecuteImpl(Gs2Session gs2Session)
            {
				UnityWebRequest.method = UnityWebRequest.kHttpVerbPOST;

                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "enhance")
                    .Replace("{region}", gs2Session.Region.DisplayName())
                    + "/{namespaceName}/master/model";

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
                if (_request.TargetInventoryModelId != null)
                {
                    jsonWriter.WritePropertyName("targetInventoryModelId");
                    jsonWriter.Write(_request.TargetInventoryModelId.ToString());
                }
                if (_request.AcquireExperienceSuffix != null)
                {
                    jsonWriter.WritePropertyName("acquireExperienceSuffix");
                    jsonWriter.Write(_request.AcquireExperienceSuffix.ToString());
                }
                if (_request.MaterialInventoryModelId != null)
                {
                    jsonWriter.WritePropertyName("materialInventoryModelId");
                    jsonWriter.Write(_request.MaterialInventoryModelId.ToString());
                }
                if (_request.AcquireExperienceHierarchy != null)
                {
                    jsonWriter.WritePropertyName("acquireExperienceHierarchy");
                    jsonWriter.WriteArrayStart();
                    foreach(var item in _request.AcquireExperienceHierarchy)
                    {
                        jsonWriter.Write(item);
                    }
                    jsonWriter.WriteArrayEnd();
                }
                if (_request.ExperienceModelId != null)
                {
                    jsonWriter.WritePropertyName("experienceModelId");
                    jsonWriter.Write(_request.ExperienceModelId.ToString());
                }
                if (_request.BonusRates != null)
                {
                    jsonWriter.WritePropertyName("bonusRates");
                    jsonWriter.WriteArrayStart();
                    foreach(var item in _request.BonusRates)
                    {
                        if (item == null) {
                            jsonWriter.Write(null);
                        } else {
                            item.WriteJson(jsonWriter);
                        }
                    }
                    jsonWriter.WriteArrayEnd();
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
		public IEnumerator CreateRateModelMaster(
                Request.CreateRateModelMasterRequest request,
                UnityAction<AsyncResult<Result.CreateRateModelMasterResult>> callback
        )
		{
			var task = new CreateRateModelMasterTask(request, callback);
			if (_certificateHandler != null)
			{
				task.UnityWebRequest.certificateHandler = _certificateHandler;
			}
			return Gs2RestSession.Execute(task);
        }

        private class GetRateModelMasterTask : Gs2RestSessionTask<Result.GetRateModelMasterResult>
        {
			private readonly Request.GetRateModelMasterRequest _request;

			public GetRateModelMasterTask(Request.GetRateModelMasterRequest request, UnityAction<AsyncResult<Result.GetRateModelMasterResult>> userCallback) : base(userCallback)
			{
				_request = request;
			}

            protected override IEnumerator ExecuteImpl(Gs2Session gs2Session)
            {
				UnityWebRequest.method = UnityWebRequest.kHttpVerbGET;

                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "enhance")
                    .Replace("{region}", gs2Session.Region.DisplayName())
                    + "/{namespaceName}/master/model/{rateName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(_request.NamespaceName) ? _request.NamespaceName.ToString() : "null");
                url = url.Replace("{rateName}", !string.IsNullOrEmpty(_request.RateName) ? _request.RateName.ToString() : "null");

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
		public IEnumerator GetRateModelMaster(
                Request.GetRateModelMasterRequest request,
                UnityAction<AsyncResult<Result.GetRateModelMasterResult>> callback
        )
		{
			var task = new GetRateModelMasterTask(request, callback);
			if (_certificateHandler != null)
			{
				task.UnityWebRequest.certificateHandler = _certificateHandler;
			}
			return Gs2RestSession.Execute(task);
        }

        private class UpdateRateModelMasterTask : Gs2RestSessionTask<Result.UpdateRateModelMasterResult>
        {
			private readonly Request.UpdateRateModelMasterRequest _request;

			public UpdateRateModelMasterTask(Request.UpdateRateModelMasterRequest request, UnityAction<AsyncResult<Result.UpdateRateModelMasterResult>> userCallback) : base(userCallback)
			{
				_request = request;
			}

            protected override IEnumerator ExecuteImpl(Gs2Session gs2Session)
            {
				UnityWebRequest.method = UnityWebRequest.kHttpVerbPUT;

                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "enhance")
                    .Replace("{region}", gs2Session.Region.DisplayName())
                    + "/{namespaceName}/master/model/{rateName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(_request.NamespaceName) ? _request.NamespaceName.ToString() : "null");
                url = url.Replace("{rateName}", !string.IsNullOrEmpty(_request.RateName) ? _request.RateName.ToString() : "null");

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
                if (_request.TargetInventoryModelId != null)
                {
                    jsonWriter.WritePropertyName("targetInventoryModelId");
                    jsonWriter.Write(_request.TargetInventoryModelId.ToString());
                }
                if (_request.AcquireExperienceSuffix != null)
                {
                    jsonWriter.WritePropertyName("acquireExperienceSuffix");
                    jsonWriter.Write(_request.AcquireExperienceSuffix.ToString());
                }
                if (_request.MaterialInventoryModelId != null)
                {
                    jsonWriter.WritePropertyName("materialInventoryModelId");
                    jsonWriter.Write(_request.MaterialInventoryModelId.ToString());
                }
                if (_request.AcquireExperienceHierarchy != null)
                {
                    jsonWriter.WritePropertyName("acquireExperienceHierarchy");
                    jsonWriter.WriteArrayStart();
                    foreach(var item in _request.AcquireExperienceHierarchy)
                    {
                        jsonWriter.Write(item);
                    }
                    jsonWriter.WriteArrayEnd();
                }
                if (_request.ExperienceModelId != null)
                {
                    jsonWriter.WritePropertyName("experienceModelId");
                    jsonWriter.Write(_request.ExperienceModelId.ToString());
                }
                if (_request.BonusRates != null)
                {
                    jsonWriter.WritePropertyName("bonusRates");
                    jsonWriter.WriteArrayStart();
                    foreach(var item in _request.BonusRates)
                    {
                        if (item == null) {
                            jsonWriter.Write(null);
                        } else {
                            item.WriteJson(jsonWriter);
                        }
                    }
                    jsonWriter.WriteArrayEnd();
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
		public IEnumerator UpdateRateModelMaster(
                Request.UpdateRateModelMasterRequest request,
                UnityAction<AsyncResult<Result.UpdateRateModelMasterResult>> callback
        )
		{
			var task = new UpdateRateModelMasterTask(request, callback);
			if (_certificateHandler != null)
			{
				task.UnityWebRequest.certificateHandler = _certificateHandler;
			}
			return Gs2RestSession.Execute(task);
        }

        private class DeleteRateModelMasterTask : Gs2RestSessionTask<Result.DeleteRateModelMasterResult>
        {
			private readonly Request.DeleteRateModelMasterRequest _request;

			public DeleteRateModelMasterTask(Request.DeleteRateModelMasterRequest request, UnityAction<AsyncResult<Result.DeleteRateModelMasterResult>> userCallback) : base(userCallback)
			{
				_request = request;
			}

            protected override IEnumerator ExecuteImpl(Gs2Session gs2Session)
            {
				UnityWebRequest.method = UnityWebRequest.kHttpVerbDELETE;

                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "enhance")
                    .Replace("{region}", gs2Session.Region.DisplayName())
                    + "/{namespaceName}/master/model/{rateName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(_request.NamespaceName) ? _request.NamespaceName.ToString() : "null");
                url = url.Replace("{rateName}", !string.IsNullOrEmpty(_request.RateName) ? _request.RateName.ToString() : "null");

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
		public IEnumerator DeleteRateModelMaster(
                Request.DeleteRateModelMasterRequest request,
                UnityAction<AsyncResult<Result.DeleteRateModelMasterResult>> callback
        )
		{
			var task = new DeleteRateModelMasterTask(request, callback);
			if (_certificateHandler != null)
			{
				task.UnityWebRequest.certificateHandler = _certificateHandler;
			}
			return Gs2RestSession.Execute(task);
        }

        private class DirectEnhanceTask : Gs2RestSessionTask<Result.DirectEnhanceResult>
        {
			private readonly Request.DirectEnhanceRequest _request;

			public DirectEnhanceTask(Request.DirectEnhanceRequest request, UnityAction<AsyncResult<Result.DirectEnhanceResult>> userCallback) : base(userCallback)
			{
				_request = request;
			}

            protected override IEnumerator ExecuteImpl(Gs2Session gs2Session)
            {
				UnityWebRequest.method = UnityWebRequest.kHttpVerbPOST;

                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "enhance")
                    .Replace("{region}", gs2Session.Region.DisplayName())
                    + "/{namespaceName}/user/me/enhance/{rateName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(_request.NamespaceName) ? _request.NamespaceName.ToString() : "null");
                url = url.Replace("{rateName}", !string.IsNullOrEmpty(_request.RateName) ? _request.RateName.ToString() : "null");

                UnityWebRequest.url = url;

                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);
                jsonWriter.WriteObjectStart();
                if (_request.TargetItemSetId != null)
                {
                    jsonWriter.WritePropertyName("targetItemSetId");
                    jsonWriter.Write(_request.TargetItemSetId.ToString());
                }
                if (_request.Materials != null)
                {
                    jsonWriter.WritePropertyName("materials");
                    jsonWriter.WriteArrayStart();
                    foreach(var item in _request.Materials)
                    {
                        if (item == null) {
                            jsonWriter.Write(null);
                        } else {
                            item.WriteJson(jsonWriter);
                        }
                    }
                    jsonWriter.WriteArrayEnd();
                }
                if (_request.Config != null)
                {
                    jsonWriter.WritePropertyName("config");
                    jsonWriter.WriteArrayStart();
                    foreach(var item in _request.Config)
                    {
                        if (item == null) {
                            jsonWriter.Write(null);
                        } else {
                            item.WriteJson(jsonWriter);
                        }
                    }
                    jsonWriter.WriteArrayEnd();
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
		public IEnumerator DirectEnhance(
                Request.DirectEnhanceRequest request,
                UnityAction<AsyncResult<Result.DirectEnhanceResult>> callback
        )
		{
			var task = new DirectEnhanceTask(request, callback);
			if (_certificateHandler != null)
			{
				task.UnityWebRequest.certificateHandler = _certificateHandler;
			}
			return Gs2RestSession.Execute(task);
        }

        private class DirectEnhanceByUserIdTask : Gs2RestSessionTask<Result.DirectEnhanceByUserIdResult>
        {
			private readonly Request.DirectEnhanceByUserIdRequest _request;

			public DirectEnhanceByUserIdTask(Request.DirectEnhanceByUserIdRequest request, UnityAction<AsyncResult<Result.DirectEnhanceByUserIdResult>> userCallback) : base(userCallback)
			{
				_request = request;
			}

            protected override IEnumerator ExecuteImpl(Gs2Session gs2Session)
            {
				UnityWebRequest.method = UnityWebRequest.kHttpVerbPOST;

                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "enhance")
                    .Replace("{region}", gs2Session.Region.DisplayName())
                    + "/{namespaceName}/user/{userId}/enhance/{rateName}";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(_request.NamespaceName) ? _request.NamespaceName.ToString() : "null");
                url = url.Replace("{rateName}", !string.IsNullOrEmpty(_request.RateName) ? _request.RateName.ToString() : "null");
                url = url.Replace("{userId}", !string.IsNullOrEmpty(_request.UserId) ? _request.UserId.ToString() : "null");

                UnityWebRequest.url = url;

                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);
                jsonWriter.WriteObjectStart();
                if (_request.TargetItemSetId != null)
                {
                    jsonWriter.WritePropertyName("targetItemSetId");
                    jsonWriter.Write(_request.TargetItemSetId.ToString());
                }
                if (_request.Materials != null)
                {
                    jsonWriter.WritePropertyName("materials");
                    jsonWriter.WriteArrayStart();
                    foreach(var item in _request.Materials)
                    {
                        if (item == null) {
                            jsonWriter.Write(null);
                        } else {
                            item.WriteJson(jsonWriter);
                        }
                    }
                    jsonWriter.WriteArrayEnd();
                }
                if (_request.Config != null)
                {
                    jsonWriter.WritePropertyName("config");
                    jsonWriter.WriteArrayStart();
                    foreach(var item in _request.Config)
                    {
                        if (item == null) {
                            jsonWriter.Write(null);
                        } else {
                            item.WriteJson(jsonWriter);
                        }
                    }
                    jsonWriter.WriteArrayEnd();
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
		public IEnumerator DirectEnhanceByUserId(
                Request.DirectEnhanceByUserIdRequest request,
                UnityAction<AsyncResult<Result.DirectEnhanceByUserIdResult>> callback
        )
		{
			var task = new DirectEnhanceByUserIdTask(request, callback);
			if (_certificateHandler != null)
			{
				task.UnityWebRequest.certificateHandler = _certificateHandler;
			}
			return Gs2RestSession.Execute(task);
        }

        private class DirectEnhanceByStampSheetTask : Gs2RestSessionTask<Result.DirectEnhanceByStampSheetResult>
        {
			private readonly Request.DirectEnhanceByStampSheetRequest _request;

			public DirectEnhanceByStampSheetTask(Request.DirectEnhanceByStampSheetRequest request, UnityAction<AsyncResult<Result.DirectEnhanceByStampSheetResult>> userCallback) : base(userCallback)
			{
				_request = request;
			}

            protected override IEnumerator ExecuteImpl(Gs2Session gs2Session)
            {
				UnityWebRequest.method = UnityWebRequest.kHttpVerbPOST;

                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "enhance")
                    .Replace("{region}", gs2Session.Region.DisplayName())
                    + "/stamp/enhance/direct";

                UnityWebRequest.url = url;

                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);
                jsonWriter.WriteObjectStart();
                if (_request.StampSheet != null)
                {
                    jsonWriter.WritePropertyName("stampSheet");
                    jsonWriter.Write(_request.StampSheet.ToString());
                }
                if (_request.KeyId != null)
                {
                    jsonWriter.WritePropertyName("keyId");
                    jsonWriter.Write(_request.KeyId.ToString());
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
		public IEnumerator DirectEnhanceByStampSheet(
                Request.DirectEnhanceByStampSheetRequest request,
                UnityAction<AsyncResult<Result.DirectEnhanceByStampSheetResult>> callback
        )
		{
			var task = new DirectEnhanceByStampSheetTask(request, callback);
			if (_certificateHandler != null)
			{
				task.UnityWebRequest.certificateHandler = _certificateHandler;
			}
			return Gs2RestSession.Execute(task);
        }

        private class DescribeProgressesByUserIdTask : Gs2RestSessionTask<Result.DescribeProgressesByUserIdResult>
        {
			private readonly Request.DescribeProgressesByUserIdRequest _request;

			public DescribeProgressesByUserIdTask(Request.DescribeProgressesByUserIdRequest request, UnityAction<AsyncResult<Result.DescribeProgressesByUserIdResult>> userCallback) : base(userCallback)
			{
				_request = request;
			}

            protected override IEnumerator ExecuteImpl(Gs2Session gs2Session)
            {
				UnityWebRequest.method = UnityWebRequest.kHttpVerbGET;

                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "enhance")
                    .Replace("{region}", gs2Session.Region.DisplayName())
                    + "/{namespaceName}/progress";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(_request.NamespaceName) ? _request.NamespaceName.ToString() : "null");

                var queryStrings = new List<string> ();
                if (_request.ContextStack != null)
                {
                    queryStrings.Add(string.Format("{0}={1}", "contextStack", UnityWebRequest.EscapeURL(_request.ContextStack)));
                }
                if (_request.UserId != null) {
                    queryStrings.Add(string.Format("{0}={1}", "userId", UnityWebRequest.EscapeURL(_request.UserId)));
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
		public IEnumerator DescribeProgressesByUserId(
                Request.DescribeProgressesByUserIdRequest request,
                UnityAction<AsyncResult<Result.DescribeProgressesByUserIdResult>> callback
        )
		{
			var task = new DescribeProgressesByUserIdTask(request, callback);
			if (_certificateHandler != null)
			{
				task.UnityWebRequest.certificateHandler = _certificateHandler;
			}
			return Gs2RestSession.Execute(task);
        }

        private class CreateProgressByUserIdTask : Gs2RestSessionTask<Result.CreateProgressByUserIdResult>
        {
			private readonly Request.CreateProgressByUserIdRequest _request;

			public CreateProgressByUserIdTask(Request.CreateProgressByUserIdRequest request, UnityAction<AsyncResult<Result.CreateProgressByUserIdResult>> userCallback) : base(userCallback)
			{
				_request = request;
			}

            protected override IEnumerator ExecuteImpl(Gs2Session gs2Session)
            {
				UnityWebRequest.method = UnityWebRequest.kHttpVerbPOST;

                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "enhance")
                    .Replace("{region}", gs2Session.Region.DisplayName())
                    + "/{namespaceName}/user/{userId}/progress";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(_request.NamespaceName) ? _request.NamespaceName.ToString() : "null");
                url = url.Replace("{userId}", !string.IsNullOrEmpty(_request.UserId) ? _request.UserId.ToString() : "null");

                UnityWebRequest.url = url;

                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);
                jsonWriter.WriteObjectStart();
                if (_request.RateName != null)
                {
                    jsonWriter.WritePropertyName("rateName");
                    jsonWriter.Write(_request.RateName.ToString());
                }
                if (_request.TargetItemSetId != null)
                {
                    jsonWriter.WritePropertyName("targetItemSetId");
                    jsonWriter.Write(_request.TargetItemSetId.ToString());
                }
                if (_request.Materials != null)
                {
                    jsonWriter.WritePropertyName("materials");
                    jsonWriter.WriteArrayStart();
                    foreach(var item in _request.Materials)
                    {
                        if (item == null) {
                            jsonWriter.Write(null);
                        } else {
                            item.WriteJson(jsonWriter);
                        }
                    }
                    jsonWriter.WriteArrayEnd();
                }
                if (_request.Force != null)
                {
                    jsonWriter.WritePropertyName("force");
                    jsonWriter.Write(_request.Force.ToString());
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
		public IEnumerator CreateProgressByUserId(
                Request.CreateProgressByUserIdRequest request,
                UnityAction<AsyncResult<Result.CreateProgressByUserIdResult>> callback
        )
		{
			var task = new CreateProgressByUserIdTask(request, callback);
			if (_certificateHandler != null)
			{
				task.UnityWebRequest.certificateHandler = _certificateHandler;
			}
			return Gs2RestSession.Execute(task);
        }

        private class GetProgressTask : Gs2RestSessionTask<Result.GetProgressResult>
        {
			private readonly Request.GetProgressRequest _request;

			public GetProgressTask(Request.GetProgressRequest request, UnityAction<AsyncResult<Result.GetProgressResult>> userCallback) : base(userCallback)
			{
				_request = request;
			}

            protected override IEnumerator ExecuteImpl(Gs2Session gs2Session)
            {
				UnityWebRequest.method = UnityWebRequest.kHttpVerbGET;

                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "enhance")
                    .Replace("{region}", gs2Session.Region.DisplayName())
                    + "/{namespaceName}/user/me/progress";

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
		public IEnumerator GetProgress(
                Request.GetProgressRequest request,
                UnityAction<AsyncResult<Result.GetProgressResult>> callback
        )
		{
			var task = new GetProgressTask(request, callback);
			if (_certificateHandler != null)
			{
				task.UnityWebRequest.certificateHandler = _certificateHandler;
			}
			return Gs2RestSession.Execute(task);
        }

        private class GetProgressByUserIdTask : Gs2RestSessionTask<Result.GetProgressByUserIdResult>
        {
			private readonly Request.GetProgressByUserIdRequest _request;

			public GetProgressByUserIdTask(Request.GetProgressByUserIdRequest request, UnityAction<AsyncResult<Result.GetProgressByUserIdResult>> userCallback) : base(userCallback)
			{
				_request = request;
			}

            protected override IEnumerator ExecuteImpl(Gs2Session gs2Session)
            {
				UnityWebRequest.method = UnityWebRequest.kHttpVerbGET;

                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "enhance")
                    .Replace("{region}", gs2Session.Region.DisplayName())
                    + "/{namespaceName}/user/{userId}/progress";

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
		public IEnumerator GetProgressByUserId(
                Request.GetProgressByUserIdRequest request,
                UnityAction<AsyncResult<Result.GetProgressByUserIdResult>> callback
        )
		{
			var task = new GetProgressByUserIdTask(request, callback);
			if (_certificateHandler != null)
			{
				task.UnityWebRequest.certificateHandler = _certificateHandler;
			}
			return Gs2RestSession.Execute(task);
        }

        private class StartTask : Gs2RestSessionTask<Result.StartResult>
        {
			private readonly Request.StartRequest _request;

			public StartTask(Request.StartRequest request, UnityAction<AsyncResult<Result.StartResult>> userCallback) : base(userCallback)
			{
				_request = request;
			}

            protected override IEnumerator ExecuteImpl(Gs2Session gs2Session)
            {
				UnityWebRequest.method = UnityWebRequest.kHttpVerbPOST;

                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "enhance")
                    .Replace("{region}", gs2Session.Region.DisplayName())
                    + "/{namespaceName}/user/me/progress/rate/{rateName}/start";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(_request.NamespaceName) ? _request.NamespaceName.ToString() : "null");
                url = url.Replace("{rateName}", !string.IsNullOrEmpty(_request.RateName) ? _request.RateName.ToString() : "null");

                UnityWebRequest.url = url;

                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);
                jsonWriter.WriteObjectStart();
                if (_request.TargetItemSetId != null)
                {
                    jsonWriter.WritePropertyName("targetItemSetId");
                    jsonWriter.Write(_request.TargetItemSetId.ToString());
                }
                if (_request.Materials != null)
                {
                    jsonWriter.WritePropertyName("materials");
                    jsonWriter.WriteArrayStart();
                    foreach(var item in _request.Materials)
                    {
                        if (item == null) {
                            jsonWriter.Write(null);
                        } else {
                            item.WriteJson(jsonWriter);
                        }
                    }
                    jsonWriter.WriteArrayEnd();
                }
                if (_request.Force != null)
                {
                    jsonWriter.WritePropertyName("force");
                    jsonWriter.Write(_request.Force.ToString());
                }
                if (_request.Config != null)
                {
                    jsonWriter.WritePropertyName("config");
                    jsonWriter.WriteArrayStart();
                    foreach(var item in _request.Config)
                    {
                        if (item == null) {
                            jsonWriter.Write(null);
                        } else {
                            item.WriteJson(jsonWriter);
                        }
                    }
                    jsonWriter.WriteArrayEnd();
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
		public IEnumerator Start(
                Request.StartRequest request,
                UnityAction<AsyncResult<Result.StartResult>> callback
        )
		{
			var task = new StartTask(request, callback);
			if (_certificateHandler != null)
			{
				task.UnityWebRequest.certificateHandler = _certificateHandler;
			}
			return Gs2RestSession.Execute(task);
        }

        private class StartByUserIdTask : Gs2RestSessionTask<Result.StartByUserIdResult>
        {
			private readonly Request.StartByUserIdRequest _request;

			public StartByUserIdTask(Request.StartByUserIdRequest request, UnityAction<AsyncResult<Result.StartByUserIdResult>> userCallback) : base(userCallback)
			{
				_request = request;
			}

            protected override IEnumerator ExecuteImpl(Gs2Session gs2Session)
            {
				UnityWebRequest.method = UnityWebRequest.kHttpVerbPOST;

                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "enhance")
                    .Replace("{region}", gs2Session.Region.DisplayName())
                    + "/{namespaceName}/user/{userId}/progress/rate/{rateName}/start";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(_request.NamespaceName) ? _request.NamespaceName.ToString() : "null");
                url = url.Replace("{rateName}", !string.IsNullOrEmpty(_request.RateName) ? _request.RateName.ToString() : "null");
                url = url.Replace("{userId}", !string.IsNullOrEmpty(_request.UserId) ? _request.UserId.ToString() : "null");

                UnityWebRequest.url = url;

                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);
                jsonWriter.WriteObjectStart();
                if (_request.TargetItemSetId != null)
                {
                    jsonWriter.WritePropertyName("targetItemSetId");
                    jsonWriter.Write(_request.TargetItemSetId.ToString());
                }
                if (_request.Materials != null)
                {
                    jsonWriter.WritePropertyName("materials");
                    jsonWriter.WriteArrayStart();
                    foreach(var item in _request.Materials)
                    {
                        if (item == null) {
                            jsonWriter.Write(null);
                        } else {
                            item.WriteJson(jsonWriter);
                        }
                    }
                    jsonWriter.WriteArrayEnd();
                }
                if (_request.Force != null)
                {
                    jsonWriter.WritePropertyName("force");
                    jsonWriter.Write(_request.Force.ToString());
                }
                if (_request.Config != null)
                {
                    jsonWriter.WritePropertyName("config");
                    jsonWriter.WriteArrayStart();
                    foreach(var item in _request.Config)
                    {
                        if (item == null) {
                            jsonWriter.Write(null);
                        } else {
                            item.WriteJson(jsonWriter);
                        }
                    }
                    jsonWriter.WriteArrayEnd();
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
		public IEnumerator StartByUserId(
                Request.StartByUserIdRequest request,
                UnityAction<AsyncResult<Result.StartByUserIdResult>> callback
        )
		{
			var task = new StartByUserIdTask(request, callback);
			if (_certificateHandler != null)
			{
				task.UnityWebRequest.certificateHandler = _certificateHandler;
			}
			return Gs2RestSession.Execute(task);
        }

        private class EndTask : Gs2RestSessionTask<Result.EndResult>
        {
			private readonly Request.EndRequest _request;

			public EndTask(Request.EndRequest request, UnityAction<AsyncResult<Result.EndResult>> userCallback) : base(userCallback)
			{
				_request = request;
			}

            protected override IEnumerator ExecuteImpl(Gs2Session gs2Session)
            {
				UnityWebRequest.method = UnityWebRequest.kHttpVerbPOST;

                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "enhance")
                    .Replace("{region}", gs2Session.Region.DisplayName())
                    + "/{namespaceName}/user/me/progress/end";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(_request.NamespaceName) ? _request.NamespaceName.ToString() : "null");

                UnityWebRequest.url = url;

                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);
                jsonWriter.WriteObjectStart();
                if (_request.Config != null)
                {
                    jsonWriter.WritePropertyName("config");
                    jsonWriter.WriteArrayStart();
                    foreach(var item in _request.Config)
                    {
                        if (item == null) {
                            jsonWriter.Write(null);
                        } else {
                            item.WriteJson(jsonWriter);
                        }
                    }
                    jsonWriter.WriteArrayEnd();
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
		public IEnumerator End(
                Request.EndRequest request,
                UnityAction<AsyncResult<Result.EndResult>> callback
        )
		{
			var task = new EndTask(request, callback);
			if (_certificateHandler != null)
			{
				task.UnityWebRequest.certificateHandler = _certificateHandler;
			}
			return Gs2RestSession.Execute(task);
        }

        private class EndByUserIdTask : Gs2RestSessionTask<Result.EndByUserIdResult>
        {
			private readonly Request.EndByUserIdRequest _request;

			public EndByUserIdTask(Request.EndByUserIdRequest request, UnityAction<AsyncResult<Result.EndByUserIdResult>> userCallback) : base(userCallback)
			{
				_request = request;
			}

            protected override IEnumerator ExecuteImpl(Gs2Session gs2Session)
            {
				UnityWebRequest.method = UnityWebRequest.kHttpVerbPOST;

                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "enhance")
                    .Replace("{region}", gs2Session.Region.DisplayName())
                    + "/{namespaceName}/user/{userId}/progress/end";

                url = url.Replace("{namespaceName}", !string.IsNullOrEmpty(_request.NamespaceName) ? _request.NamespaceName.ToString() : "null");
                url = url.Replace("{userId}", !string.IsNullOrEmpty(_request.UserId) ? _request.UserId.ToString() : "null");

                UnityWebRequest.url = url;

                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);
                jsonWriter.WriteObjectStart();
                if (_request.Config != null)
                {
                    jsonWriter.WritePropertyName("config");
                    jsonWriter.WriteArrayStart();
                    foreach(var item in _request.Config)
                    {
                        if (item == null) {
                            jsonWriter.Write(null);
                        } else {
                            item.WriteJson(jsonWriter);
                        }
                    }
                    jsonWriter.WriteArrayEnd();
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
		public IEnumerator EndByUserId(
                Request.EndByUserIdRequest request,
                UnityAction<AsyncResult<Result.EndByUserIdResult>> callback
        )
		{
			var task = new EndByUserIdTask(request, callback);
			if (_certificateHandler != null)
			{
				task.UnityWebRequest.certificateHandler = _certificateHandler;
			}
			return Gs2RestSession.Execute(task);
        }

        private class DeleteProgressTask : Gs2RestSessionTask<Result.DeleteProgressResult>
        {
			private readonly Request.DeleteProgressRequest _request;

			public DeleteProgressTask(Request.DeleteProgressRequest request, UnityAction<AsyncResult<Result.DeleteProgressResult>> userCallback) : base(userCallback)
			{
				_request = request;
			}

            protected override IEnumerator ExecuteImpl(Gs2Session gs2Session)
            {
				UnityWebRequest.method = UnityWebRequest.kHttpVerbDELETE;

                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "enhance")
                    .Replace("{region}", gs2Session.Region.DisplayName())
                    + "/{namespaceName}/user/me/progress";

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
		public IEnumerator DeleteProgress(
                Request.DeleteProgressRequest request,
                UnityAction<AsyncResult<Result.DeleteProgressResult>> callback
        )
		{
			var task = new DeleteProgressTask(request, callback);
			if (_certificateHandler != null)
			{
				task.UnityWebRequest.certificateHandler = _certificateHandler;
			}
			return Gs2RestSession.Execute(task);
        }

        private class DeleteProgressByUserIdTask : Gs2RestSessionTask<Result.DeleteProgressByUserIdResult>
        {
			private readonly Request.DeleteProgressByUserIdRequest _request;

			public DeleteProgressByUserIdTask(Request.DeleteProgressByUserIdRequest request, UnityAction<AsyncResult<Result.DeleteProgressByUserIdResult>> userCallback) : base(userCallback)
			{
				_request = request;
			}

            protected override IEnumerator ExecuteImpl(Gs2Session gs2Session)
            {
				UnityWebRequest.method = UnityWebRequest.kHttpVerbDELETE;

                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "enhance")
                    .Replace("{region}", gs2Session.Region.DisplayName())
                    + "/{namespaceName}/user/{userId}/progress";

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
		public IEnumerator DeleteProgressByUserId(
                Request.DeleteProgressByUserIdRequest request,
                UnityAction<AsyncResult<Result.DeleteProgressByUserIdResult>> callback
        )
		{
			var task = new DeleteProgressByUserIdTask(request, callback);
			if (_certificateHandler != null)
			{
				task.UnityWebRequest.certificateHandler = _certificateHandler;
			}
			return Gs2RestSession.Execute(task);
        }

        private class CreateProgressByStampSheetTask : Gs2RestSessionTask<Result.CreateProgressByStampSheetResult>
        {
			private readonly Request.CreateProgressByStampSheetRequest _request;

			public CreateProgressByStampSheetTask(Request.CreateProgressByStampSheetRequest request, UnityAction<AsyncResult<Result.CreateProgressByStampSheetResult>> userCallback) : base(userCallback)
			{
				_request = request;
			}

            protected override IEnumerator ExecuteImpl(Gs2Session gs2Session)
            {
				UnityWebRequest.method = UnityWebRequest.kHttpVerbPOST;

                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "enhance")
                    .Replace("{region}", gs2Session.Region.DisplayName())
                    + "/stamp/progress/create";

                UnityWebRequest.url = url;

                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);
                jsonWriter.WriteObjectStart();
                if (_request.StampSheet != null)
                {
                    jsonWriter.WritePropertyName("stampSheet");
                    jsonWriter.Write(_request.StampSheet.ToString());
                }
                if (_request.KeyId != null)
                {
                    jsonWriter.WritePropertyName("keyId");
                    jsonWriter.Write(_request.KeyId.ToString());
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
		public IEnumerator CreateProgressByStampSheet(
                Request.CreateProgressByStampSheetRequest request,
                UnityAction<AsyncResult<Result.CreateProgressByStampSheetResult>> callback
        )
		{
			var task = new CreateProgressByStampSheetTask(request, callback);
			if (_certificateHandler != null)
			{
				task.UnityWebRequest.certificateHandler = _certificateHandler;
			}
			return Gs2RestSession.Execute(task);
        }

        private class DeleteProgressByStampTaskTask : Gs2RestSessionTask<Result.DeleteProgressByStampTaskResult>
        {
			private readonly Request.DeleteProgressByStampTaskRequest _request;

			public DeleteProgressByStampTaskTask(Request.DeleteProgressByStampTaskRequest request, UnityAction<AsyncResult<Result.DeleteProgressByStampTaskResult>> userCallback) : base(userCallback)
			{
				_request = request;
			}

            protected override IEnumerator ExecuteImpl(Gs2Session gs2Session)
            {
				UnityWebRequest.method = UnityWebRequest.kHttpVerbPOST;

                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "enhance")
                    .Replace("{region}", gs2Session.Region.DisplayName())
                    + "/stamp/progress/delete";

                UnityWebRequest.url = url;

                var stringBuilder = new StringBuilder();
                var jsonWriter = new JsonWriter(stringBuilder);
                jsonWriter.WriteObjectStart();
                if (_request.StampTask != null)
                {
                    jsonWriter.WritePropertyName("stampTask");
                    jsonWriter.Write(_request.StampTask.ToString());
                }
                if (_request.KeyId != null)
                {
                    jsonWriter.WritePropertyName("keyId");
                    jsonWriter.Write(_request.KeyId.ToString());
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
		public IEnumerator DeleteProgressByStampTask(
                Request.DeleteProgressByStampTaskRequest request,
                UnityAction<AsyncResult<Result.DeleteProgressByStampTaskResult>> callback
        )
		{
			var task = new DeleteProgressByStampTaskTask(request, callback);
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
                    .Replace("{service}", "enhance")
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

        private class GetCurrentRateMasterTask : Gs2RestSessionTask<Result.GetCurrentRateMasterResult>
        {
			private readonly Request.GetCurrentRateMasterRequest _request;

			public GetCurrentRateMasterTask(Request.GetCurrentRateMasterRequest request, UnityAction<AsyncResult<Result.GetCurrentRateMasterResult>> userCallback) : base(userCallback)
			{
				_request = request;
			}

            protected override IEnumerator ExecuteImpl(Gs2Session gs2Session)
            {
				UnityWebRequest.method = UnityWebRequest.kHttpVerbGET;

                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "enhance")
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
		public IEnumerator GetCurrentRateMaster(
                Request.GetCurrentRateMasterRequest request,
                UnityAction<AsyncResult<Result.GetCurrentRateMasterResult>> callback
        )
		{
			var task = new GetCurrentRateMasterTask(request, callback);
			if (_certificateHandler != null)
			{
				task.UnityWebRequest.certificateHandler = _certificateHandler;
			}
			return Gs2RestSession.Execute(task);
        }

        private class UpdateCurrentRateMasterTask : Gs2RestSessionTask<Result.UpdateCurrentRateMasterResult>
        {
			private readonly Request.UpdateCurrentRateMasterRequest _request;

			public UpdateCurrentRateMasterTask(Request.UpdateCurrentRateMasterRequest request, UnityAction<AsyncResult<Result.UpdateCurrentRateMasterResult>> userCallback) : base(userCallback)
			{
				_request = request;
			}

            protected override IEnumerator ExecuteImpl(Gs2Session gs2Session)
            {
				UnityWebRequest.method = UnityWebRequest.kHttpVerbPUT;

                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "enhance")
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
		public IEnumerator UpdateCurrentRateMaster(
                Request.UpdateCurrentRateMasterRequest request,
                UnityAction<AsyncResult<Result.UpdateCurrentRateMasterResult>> callback
        )
		{
			var task = new UpdateCurrentRateMasterTask(request, callback);
			if (_certificateHandler != null)
			{
				task.UnityWebRequest.certificateHandler = _certificateHandler;
			}
			return Gs2RestSession.Execute(task);
        }

        private class UpdateCurrentRateMasterFromGitHubTask : Gs2RestSessionTask<Result.UpdateCurrentRateMasterFromGitHubResult>
        {
			private readonly Request.UpdateCurrentRateMasterFromGitHubRequest _request;

			public UpdateCurrentRateMasterFromGitHubTask(Request.UpdateCurrentRateMasterFromGitHubRequest request, UnityAction<AsyncResult<Result.UpdateCurrentRateMasterFromGitHubResult>> userCallback) : base(userCallback)
			{
				_request = request;
			}

            protected override IEnumerator ExecuteImpl(Gs2Session gs2Session)
            {
				UnityWebRequest.method = UnityWebRequest.kHttpVerbPUT;

                var url = Gs2RestSession.EndpointHost
                    .Replace("{service}", "enhance")
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
		public IEnumerator UpdateCurrentRateMasterFromGitHub(
                Request.UpdateCurrentRateMasterFromGitHubRequest request,
                UnityAction<AsyncResult<Result.UpdateCurrentRateMasterFromGitHubResult>> callback
        )
		{
			var task = new UpdateCurrentRateMasterFromGitHubTask(request, callback);
			if (_certificateHandler != null)
			{
				task.UnityWebRequest.certificateHandler = _certificateHandler;
			}
			return Gs2RestSession.Execute(task);
        }
	}
}