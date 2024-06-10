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
#if UNITY_2017_1_OR_NEWER
using UnityEngine.Scripting;
#endif

namespace Gs2.Gs2JobQueue.Model
{

#if UNITY_2017_1_OR_NEWER
	[Preserve]
#endif
	public class LogSetting : IComparable
	{
        public string LoggingNamespaceId { set; get; } = null!;
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
            var diff = 0;
            if (LoggingNamespaceId == null && LoggingNamespaceId == other.LoggingNamespaceId)
            {
                // null and null
            }
            else
            {
                diff += LoggingNamespaceId.CompareTo(other.LoggingNamespaceId);
            }
            return diff;
        }

        public void Validate() {
            {
                if (LoggingNamespaceId.Length > 1024) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("logSetting", "jobQueue.logSetting.loggingNamespaceId.error.tooLong"),
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