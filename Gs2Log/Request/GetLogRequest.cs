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

using System;
using System.Collections.Generic;
using System.Linq;
using Gs2.Core.Control;
using Gs2.Core.Model;
using Gs2.Gs2Log.Model;
using Gs2.Util.LitJson;

#if UNITY_2017_1_OR_NEWER
using UnityEngine.Scripting;
#endif

namespace Gs2.Gs2Log.Request
{
#if UNITY_2017_1_OR_NEWER
	[Preserve]
#endif
	[System.Serializable]
	public class GetLogRequest : Gs2Request<GetLogRequest>
	{
         public string NamespaceName { set; get; } = null!;
         public string LogRequestId { set; get; } = null!;
         public long? Begin { set; get; } = null!;
         public long? End { set; get; } = null!;
        public GetLogRequest WithNamespaceName(string namespaceName) {
            this.NamespaceName = namespaceName;
            return this;
        }
        public GetLogRequest WithLogRequestId(string logRequestId) {
            this.LogRequestId = logRequestId;
            return this;
        }
        public GetLogRequest WithBegin(long? begin) {
            this.Begin = begin;
            return this;
        }
        public GetLogRequest WithEnd(long? end) {
            this.End = end;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static GetLogRequest FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new GetLogRequest()
                .WithNamespaceName(!data.Keys.Contains("namespaceName") || data["namespaceName"] == null ? null : data["namespaceName"].ToString())
                .WithLogRequestId(!data.Keys.Contains("logRequestId") || data["logRequestId"] == null ? null : data["logRequestId"].ToString())
                .WithBegin(!data.Keys.Contains("begin") || data["begin"] == null ? null : (long?)(data["begin"].ToString().Contains(".") ? (long)double.Parse(data["begin"].ToString()) : long.Parse(data["begin"].ToString())))
                .WithEnd(!data.Keys.Contains("end") || data["end"] == null ? null : (long?)(data["end"].ToString().Contains(".") ? (long)double.Parse(data["end"].ToString()) : long.Parse(data["end"].ToString())));
        }

        public override JsonData ToJson()
        {
            return new JsonData {
                ["namespaceName"] = NamespaceName,
                ["logRequestId"] = LogRequestId,
                ["begin"] = Begin,
                ["end"] = End,
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (NamespaceName != null) {
                writer.WritePropertyName("namespaceName");
                writer.Write(NamespaceName.ToString());
            }
            if (LogRequestId != null) {
                writer.WritePropertyName("logRequestId");
                writer.Write(LogRequestId.ToString());
            }
            if (Begin != null) {
                writer.WritePropertyName("begin");
                writer.Write((Begin.ToString().Contains(".") ? (long)double.Parse(Begin.ToString()) : long.Parse(Begin.ToString())));
            }
            if (End != null) {
                writer.WritePropertyName("end");
                writer.Write((End.ToString().Contains(".") ? (long)double.Parse(End.ToString()) : long.Parse(End.ToString())));
            }
            writer.WriteObjectEnd();
        }

        public override string UniqueKey() {
            var key = "";
            key += NamespaceName + ":";
            key += LogRequestId + ":";
            key += Begin + ":";
            key += End + ":";
            return key;
        }
    }
}