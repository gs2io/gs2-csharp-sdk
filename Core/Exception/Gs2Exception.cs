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

using System.Collections.Generic;
using System.Linq;
using Gs2.Core.Model;
using Gs2.Util.LitJson;

namespace Gs2.Core.Exception
{
	public abstract class Gs2Exception : System.Exception
	{
		protected Gs2Exception(string message) : base(message)
		{
			try {
				errors = JsonMapper.ToObject<RequestError[]> (message);
			} catch (System.Exception) {
				errors = new RequestError[]{};
			}
		}
		
		protected Gs2Exception(string message, System.Exception innerException) : base(message, innerException) {
			try {
				errors = JsonMapper.ToObject<RequestError[]> (message);
			} catch (System.Exception) {
				errors = new RequestError[]{};
			}
		}

		protected Gs2Exception(RequestError[] errors) : base(string.Join(", ", errors.Select(v => v.ToString()).ToArray()))
		{
			this.errors = errors;
		}

		protected Gs2Exception(RequestError[] errors, System.Exception innerException) : base(string.Join(", ", errors.Select(v => v.ToString()).ToArray()), innerException) {
			this.errors = errors;
		}

		// ReSharper disable once InconsistentNaming
		public RequestError[] errors;

		public RequestError[] Errors
		{
			get { return errors; }
			set { errors = value; }
		}

		// ReSharper disable once InconsistentNaming
		public ResultMetadata metadata;

		public ResultMetadata Metadata
		{
			get { return metadata; }
			set { metadata = value; }
		}
		
		public abstract int StatusCode { get; }
		
		public abstract bool RecommendRetry { get; }
		public abstract bool RecommendAutoRetry { get; }

		public override string ToString() {
			if (this.errors != null && this.errors.Length > 0) {
				return string.Join(", ", this.errors.Select(v => v.ToString()).ToArray());
			}
			return base.ToString();
		}

		public static Gs2Exception ExtractError(string message, long statusCode)
		{
			if (statusCode == 200) {
				return null;
			}
			var errors = ParseErrors(message, out var metadata);
			var error = errors.Length > 0
				? CreateByStatus(statusCode, errors)
				: CreateByStatus(statusCode, message);
			if (metadata != null) {
				error.Metadata = metadata;
			}
			return error;
		}

		public static Gs2Exception ExtractError(string action, string message, long statusCode)
		{
			return Gs2ErrorResolver.ResolveAction(action, ExtractError(message, statusCode));
		}

		private static Gs2Exception CreateByStatus(long statusCode, RequestError[] errors)
		{
			switch (statusCode)
			{
				case 0:
					return new NoInternetConnectionException(errors);
				case 400:
					return new BadRequestException(errors);
				case 401:
					return new UnauthorizedException(errors);
				case 402:
					return new QuotaLimitExceededException(errors);
				case 404:
					return new NotFoundException(errors);
				case 409:
					return new ConflictException(errors);
				case 500:
					return new InternalServerErrorException(errors);
				case 502:
					return new BadGatewayException(errors);
				case 503:
					return new ServiceUnavailableException(errors);
				case 504:
					return new RequestTimeoutException(errors);
				default:
					return new UnknownException(errors);
			}
		}

		private static Gs2Exception CreateByStatus(long statusCode, string message)
		{
			switch (statusCode)
			{
				case 0:
					return new NoInternetConnectionException(message);
				case 400:
					return new BadRequestException(message);
				case 401:
					return new UnauthorizedException(message);
				case 402:
					return new QuotaLimitExceededException(message);
				case 404:
					return new NotFoundException(message);
				case 409:
					return new ConflictException(message);
				case 500:
					return new InternalServerErrorException(message);
				case 502:
					return new BadGatewayException(message);
				case 503:
					return new ServiceUnavailableException(message);
				case 504:
					return new RequestTimeoutException(message);
				default:
					return new UnknownException(message);
			}
		}

		private static RequestError[] ParseErrors(string message, out ResultMetadata metadata)
		{
			metadata = null;
			if (string.IsNullOrEmpty(message)) {
				return new RequestError[]{};
			}
			try {
				return ParseErrors(JsonMapper.ToObject(message), out metadata);
			} catch (System.Exception) {
				metadata = null;
				return new RequestError[]{};
			}
		}

		private static RequestError[] ParseErrors(JsonData data, out ResultMetadata metadata)
		{
			metadata = null;
			if (data == null) {
				return new RequestError[]{};
			}
			if (data.IsArray) {
				return ToRequestErrors(data);
			}
			if (!data.IsObject) {
				return new RequestError[]{};
			}
			if (data.Keys.Contains("metadata") && data["metadata"] != null && data["metadata"].IsObject) {
				metadata = ResultMetadata.FromJson(data["metadata"]);
			}
			if (data.Keys.Contains("errors") && data["errors"] != null && data["errors"].IsArray) {
				return ToRequestErrors(data["errors"]);
			}
			if (data.Keys.Contains("code") && data["code"] != null && data["code"].IsString) {
				return new[] { ToRequestError(data) };
			}
			if (data.Keys.Contains("message") && data["message"] != null && data["message"].IsString) {
				var errors = ParseErrors((string)data["message"], out var innerMetadata);
				if (metadata == null) {
					metadata = innerMetadata;
				}
				return errors;
			}
			return new RequestError[]{};
		}

		private static RequestError[] ToRequestErrors(JsonData data)
		{
			var errors = new List<RequestError>();
			for (var i = 0; i < data.Count; i++) {
				if (data[i] != null && data[i].IsObject) {
					errors.Add(ToRequestError(data[i]));
				}
			}
			return errors.ToArray();
		}

		private static RequestError ToRequestError(JsonData data)
		{
			return new RequestError(
				StringField(data, "component"),
				StringField(data, "message"),
				StringField(data, "code")
			);
		}

		private static string StringField(JsonData data, string key)
		{
			if (!data.Keys.Contains(key) || data[key] == null) {
				return null;
			}
			return data[key].IsString ? (string)data[key] : data[key].ToJson();
		}
	}
}
