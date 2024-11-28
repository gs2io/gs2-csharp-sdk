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

namespace Gs2.Gs2Exchange.Model
{

#if UNITY_2017_1_OR_NEWER
	[Preserve]
#endif
	public class LogCost : IComparable
	{
        public double? Base { set; get; } = null!;
        public double[] Adds { set; get; } = null!;
        public double[] Subs { set; get; } = null!;
        public LogCost WithBase(double? base_) {
            this.Base = base_;
            return this;
        }
        public LogCost WithAdds(double[] adds) {
            this.Adds = adds;
            return this;
        }
        public LogCost WithSubs(double[] subs) {
            this.Subs = subs;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static LogCost FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new LogCost()
                .WithBase(!data.Keys.Contains("base") || data["base"] == null ? null : (double?)double.Parse(data["base"].ToString()))
                .WithAdds(!data.Keys.Contains("adds") || data["adds"] == null || !data["adds"].IsArray ? null : data["adds"].Cast<JsonData>().Select(v => {
                    return double.Parse(v.ToString());
                }).ToArray())
                .WithSubs(!data.Keys.Contains("subs") || data["subs"] == null || !data["subs"].IsArray ? null : data["subs"].Cast<JsonData>().Select(v => {
                    return double.Parse(v.ToString());
                }).ToArray());
        }

        public JsonData ToJson()
        {
            JsonData addsJsonData = null;
            if (Adds != null && Adds.Length > 0)
            {
                addsJsonData = new JsonData();
                foreach (var add in Adds)
                {
                    addsJsonData.Add(add);
                }
            }
            JsonData subsJsonData = null;
            if (Subs != null && Subs.Length > 0)
            {
                subsJsonData = new JsonData();
                foreach (var sub in Subs)
                {
                    subsJsonData.Add(sub);
                }
            }
            return new JsonData {
                ["base"] = Base,
                ["adds"] = addsJsonData,
                ["subs"] = subsJsonData,
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (Base != null) {
                writer.WritePropertyName("base");
                writer.Write(double.Parse(Base.ToString()));
            }
            if (Adds != null) {
                writer.WritePropertyName("adds");
                writer.WriteArrayStart();
                foreach (var add in Adds)
                {
                    writer.Write(double.Parse(add.ToString()));
                }
                writer.WriteArrayEnd();
            }
            if (Subs != null) {
                writer.WritePropertyName("subs");
                writer.WriteArrayStart();
                foreach (var sub in Subs)
                {
                    writer.Write(double.Parse(sub.ToString()));
                }
                writer.WriteArrayEnd();
            }
            writer.WriteObjectEnd();
        }

        public int CompareTo(object obj)
        {
            var other = obj as LogCost;
            var diff = 0;
            if (Base == null && Base == other.Base)
            {
                // null and null
            }
            else
            {
                diff += (int)(Base - other.Base);
            }
            if (Adds == null && Adds == other.Adds)
            {
                // null and null
            }
            else
            {
                diff += Adds.Length - other.Adds.Length;
                for (var i = 0; i < Adds.Length; i++)
                {
                    diff += (int)(Adds[i] - other.Adds[i]);
                }
            }
            if (Subs == null && Subs == other.Subs)
            {
                // null and null
            }
            else
            {
                diff += Subs.Length - other.Subs.Length;
                for (var i = 0; i < Subs.Length; i++)
                {
                    diff += (int)(Subs[i] - other.Subs[i]);
                }
            }
            return diff;
        }

        public void Validate() {
            {
                if (Base < 0) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("logCost", "exchange.logCost.base.error.invalid"),
                    });
                }
                if (Base > 10) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("logCost", "exchange.logCost.base.error.invalid"),
                    });
                }
            }
            {
                if (Adds.Length < 1) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("logCost", "exchange.logCost.adds.error.tooFew"),
                    });
                }
                if (Adds.Length > 10000) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("logCost", "exchange.logCost.adds.error.tooMany"),
                    });
                }
            }
            {
                if (Subs.Length > 10000) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("logCost", "exchange.logCost.subs.error.tooMany"),
                    });
                }
            }
        }

        public object Clone() {
            return new LogCost {
                Base = Base,
                Adds = Adds.Clone() as double[],
                Subs = Subs.Clone() as double[],
            };
        }
    }
}