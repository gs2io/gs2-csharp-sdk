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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Gs2.Core.Model;
using Gs2.Util.LitJson;
using UnityEngine.Scripting;

namespace Gs2.Gs2Realtime.Model
{

	[Preserve]
	public class ResponseCache : IComparable
	{
        public string Region { set; get; }
        public string ResponseCacheId { set; get; }
        public string RequestHash { set; get; }
        public string Result { set; get; }

        public ResponseCache WithRegion(string region) {
            this.Region = region;
            return this;
        }

        public ResponseCache WithResponseCacheId(string responseCacheId) {
            this.ResponseCacheId = responseCacheId;
            return this;
        }

        public ResponseCache WithRequestHash(string requestHash) {
            this.RequestHash = requestHash;
            return this;
        }

        public ResponseCache WithResult(string result) {
            this.Result = result;
            return this;
        }

    	[Preserve]
        public static ResponseCache FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new ResponseCache()
                .WithRegion(!data.Keys.Contains("region") || data["region"] == null ? null : data["region"].ToString())
                .WithResponseCacheId(!data.Keys.Contains("responseCacheId") || data["responseCacheId"] == null ? null : data["responseCacheId"].ToString())
                .WithRequestHash(!data.Keys.Contains("requestHash") || data["requestHash"] == null ? null : data["requestHash"].ToString())
                .WithResult(!data.Keys.Contains("result") || data["result"] == null ? null : data["result"].ToString());
        }

        public JsonData ToJson()
        {
            return new JsonData {
                ["region"] = Region,
                ["responseCacheId"] = ResponseCacheId,
                ["requestHash"] = RequestHash,
                ["result"] = Result,
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (Region != null) {
                writer.WritePropertyName("region");
                writer.Write(Region.ToString());
            }
            if (ResponseCacheId != null) {
                writer.WritePropertyName("responseCacheId");
                writer.Write(ResponseCacheId.ToString());
            }
            if (RequestHash != null) {
                writer.WritePropertyName("requestHash");
                writer.Write(RequestHash.ToString());
            }
            if (Result != null) {
                writer.WritePropertyName("result");
                writer.Write(Result.ToString());
            }
            writer.WriteObjectEnd();
        }

        public int CompareTo(object obj)
        {
            var other = obj as ResponseCache;
            var diff = 0;
            if (Region == null && Region == other.Region)
            {
                // null and null
            }
            else
            {
                diff += Region.CompareTo(other.Region);
            }
            if (ResponseCacheId == null && ResponseCacheId == other.ResponseCacheId)
            {
                // null and null
            }
            else
            {
                diff += ResponseCacheId.CompareTo(other.ResponseCacheId);
            }
            if (RequestHash == null && RequestHash == other.RequestHash)
            {
                // null and null
            }
            else
            {
                diff += RequestHash.CompareTo(other.RequestHash);
            }
            if (Result == null && Result == other.Result)
            {
                // null and null
            }
            else
            {
                diff += Result.CompareTo(other.Result);
            }
            return diff;
        }
    }
}