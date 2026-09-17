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
using System.Text.RegularExpressions;
using Gs2.Core.Model;
using Gs2.Util.LitJson;
#if UNITY_2017_1_OR_NEWER
using UnityEngine.Scripting;
#endif

namespace Gs2.Gs2Friend.Model
{

#if UNITY_2017_1_OR_NEWER
	[Preserve]
#endif
	public partial class LogSetting : IComparable
	{
        public string LoggingNamespaceId { set; get; }
        public LogSetting WithLoggingNamespaceId(string loggingNamespaceId) {
            this.LoggingNamespaceId = loggingNamespaceId;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static LogSetting FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            if (data.IsString) {
                // The model arrived as the JSON text of itself rather than as
                // an object. A stamp sheet carries values the client supplied
                // as strings — a store receipt is one — so reading such a
                // request back finds the text where the object is expected.
                data = JsonMapper.ToObject(data.ToString());
            }
            return new LogSetting()
                .WithLoggingNamespaceId(!data.Keys.Contains("loggingNamespaceId") || data["loggingNamespaceId"] == null ? null : data["loggingNamespaceId"].ToString());
        }

        public JsonData ToJson()
        {
            return new JsonData {
                ["loggingNamespaceId"] = LoggingNamespaceId,
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (LoggingNamespaceId != null) {
                writer.WritePropertyName("loggingNamespaceId");
                writer.Write(LoggingNamespaceId.ToString());
            }
            writer.WriteObjectEnd();
        }

        public int CompareTo(object obj)
        {
            var other = obj as LogSetting;
            if (ReferenceEquals(other, null))
            {
                if (ReferenceEquals(obj, null))
                {
                    return 1;
                }
                throw new ArgumentException("Object must be of type LogSetting.", nameof(obj));
            }
            var diff = 0;
            diff = ModelComparer.Compare(LoggingNamespaceId, other.LoggingNamespaceId);
            if (diff != 0)
            {
                return diff;
            }
            return 0;
        }

        public void Validate() {
            {
                if (LoggingNamespaceId.Length > 1024) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("logSetting", "friend.logSetting.loggingNamespaceId.error.tooLong"),
                    });
                }
            }
        }

        public object Clone() {
            return new LogSetting {
                LoggingNamespaceId = LoggingNamespaceId,
            };
        }
    }
}