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

namespace Gs2.Gs2Exchange.Model
{

#if UNITY_2017_1_OR_NEWER
	[Preserve]
#endif
	public partial class LogRate : IComparable
	{
        public double? Base { set; get; }
        public double[] Logs { set; get; }
        public LogRate WithBase(double? base_) {
            this.Base = base_;
            return this;
        }
        public LogRate WithLogs(double[] logs) {
            this.Logs = logs;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static LogRate FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new LogRate()
                .WithBase(!data.Keys.Contains("base") || data["base"] == null ? null : Gs2.Core.Util.JsonValue.ToNullableDouble(data["base"].ToString()))
                .WithLogs(!data.Keys.Contains("logs") || data["logs"] == null || !data["logs"].IsArray ? null : data["logs"].Cast<JsonData>().Select(v => {
                    return double.Parse(v.ToString());
                }).ToArray());
        }

        public JsonData ToJson()
        {
            JsonData logsJsonData = null;
            if (Logs != null && Logs.Length > 0)
            {
                logsJsonData = new JsonData();
                foreach (var log in Logs)
                {
                    logsJsonData.Add(log);
                }
            }
            return new JsonData {
                ["base"] = Base,
                ["logs"] = logsJsonData,
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (Base != null) {
                writer.WritePropertyName("base");
                writer.Write(double.Parse(Base.ToString()));
            }
            if (Logs != null) {
                writer.WritePropertyName("logs");
                writer.WriteArrayStart();
                foreach (var log in Logs)
                {
                    writer.Write(double.Parse(log.ToString()));
                }
                writer.WriteArrayEnd();
            }
            writer.WriteObjectEnd();
        }

        public int CompareTo(object obj)
        {
            var other = obj as LogRate;
            if (ReferenceEquals(other, null))
            {
                if (ReferenceEquals(obj, null))
                {
                    return 1;
                }
                throw new ArgumentException("Object must be of type LogRate.", nameof(obj));
            }
            var diff = 0;
            diff = ModelComparer.Compare(Base, other.Base);
            if (diff != 0)
            {
                return diff;
            }
            diff = ModelComparer.CompareArray(Logs, other.Logs);
            if (diff != 0)
            {
                return diff;
            }
            return 0;
        }

        public void Validate() {
            {
                if (Base < 0) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("logRate", "exchange.logRate.base.error.invalid"),
                    });
                }
                if (Base > 10) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("logRate", "exchange.logRate.base.error.invalid"),
                    });
                }
            }
            {
                if (Logs.Length < 1) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("logRate", "exchange.logRate.logs.error.tooFew"),
                    });
                }
                if (Logs.Length > 10000) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("logRate", "exchange.logRate.logs.error.tooMany"),
                    });
                }
            }
        }

        public object Clone() {
            return new LogRate {
                Base = Base,
                Logs = Logs?.Clone() as double[],
            };
        }
    }
}