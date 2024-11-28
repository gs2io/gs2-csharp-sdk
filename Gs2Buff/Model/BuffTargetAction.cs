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

namespace Gs2.Gs2Buff.Model
{

#if UNITY_2017_1_OR_NEWER
	[Preserve]
#endif
	public class BuffTargetAction : IComparable
	{
        public string TargetActionName { set; get; } = null!;
        public string TargetFieldName { set; get; } = null!;
        public Gs2.Gs2Buff.Model.BuffTargetGrn[] ConditionGrns { set; get; } = null!;
        public float? Rate { set; get; } = null!;
        public BuffTargetAction WithTargetActionName(string targetActionName) {
            this.TargetActionName = targetActionName;
            return this;
        }
        public BuffTargetAction WithTargetFieldName(string targetFieldName) {
            this.TargetFieldName = targetFieldName;
            return this;
        }
        public BuffTargetAction WithConditionGrns(Gs2.Gs2Buff.Model.BuffTargetGrn[] conditionGrns) {
            this.ConditionGrns = conditionGrns;
            return this;
        }
        public BuffTargetAction WithRate(float? rate) {
            this.Rate = rate;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static BuffTargetAction FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new BuffTargetAction()
                .WithTargetActionName(!data.Keys.Contains("targetActionName") || data["targetActionName"] == null ? null : data["targetActionName"].ToString())
                .WithTargetFieldName(!data.Keys.Contains("targetFieldName") || data["targetFieldName"] == null ? null : data["targetFieldName"].ToString())
                .WithConditionGrns(!data.Keys.Contains("conditionGrns") || data["conditionGrns"] == null || !data["conditionGrns"].IsArray ? null : data["conditionGrns"].Cast<JsonData>().Select(v => {
                    return Gs2.Gs2Buff.Model.BuffTargetGrn.FromJson(v);
                }).ToArray())
                .WithRate(!data.Keys.Contains("rate") || data["rate"] == null ? null : (float?)float.Parse(data["rate"].ToString()));
        }

        public JsonData ToJson()
        {
            JsonData conditionGrnsJsonData = null;
            if (ConditionGrns != null && ConditionGrns.Length > 0)
            {
                conditionGrnsJsonData = new JsonData();
                foreach (var conditionGrn in ConditionGrns)
                {
                    conditionGrnsJsonData.Add(conditionGrn.ToJson());
                }
            }
            return new JsonData {
                ["targetActionName"] = TargetActionName,
                ["targetFieldName"] = TargetFieldName,
                ["conditionGrns"] = conditionGrnsJsonData,
                ["rate"] = Rate,
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (TargetActionName != null) {
                writer.WritePropertyName("targetActionName");
                writer.Write(TargetActionName.ToString());
            }
            if (TargetFieldName != null) {
                writer.WritePropertyName("targetFieldName");
                writer.Write(TargetFieldName.ToString());
            }
            if (ConditionGrns != null) {
                writer.WritePropertyName("conditionGrns");
                writer.WriteArrayStart();
                foreach (var conditionGrn in ConditionGrns)
                {
                    if (conditionGrn != null) {
                        conditionGrn.WriteJson(writer);
                    }
                }
                writer.WriteArrayEnd();
            }
            if (Rate != null) {
                writer.WritePropertyName("rate");
                writer.Write(float.Parse(Rate.ToString()));
            }
            writer.WriteObjectEnd();
        }

        public int CompareTo(object obj)
        {
            var other = obj as BuffTargetAction;
            var diff = 0;
            if (TargetActionName == null && TargetActionName == other.TargetActionName)
            {
                // null and null
            }
            else
            {
                diff += TargetActionName.CompareTo(other.TargetActionName);
            }
            if (TargetFieldName == null && TargetFieldName == other.TargetFieldName)
            {
                // null and null
            }
            else
            {
                diff += TargetFieldName.CompareTo(other.TargetFieldName);
            }
            if (ConditionGrns == null && ConditionGrns == other.ConditionGrns)
            {
                // null and null
            }
            else
            {
                diff += ConditionGrns.Length - other.ConditionGrns.Length;
                for (var i = 0; i < ConditionGrns.Length; i++)
                {
                    diff += ConditionGrns[i].CompareTo(other.ConditionGrns[i]);
                }
            }
            if (Rate == null && Rate == other.Rate)
            {
                // null and null
            }
            else
            {
                diff += (int)(Rate - other.Rate);
            }
            return diff;
        }

        public void Validate() {
            {
                if (TargetActionName.Length > 128) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("buffTargetAction", "buff.buffTargetAction.targetActionName.error.tooLong"),
                    });
                }
            }
            {
                if (TargetFieldName.Length > 64) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("buffTargetAction", "buff.buffTargetAction.targetFieldName.error.tooLong"),
                    });
                }
            }
            {
                if (ConditionGrns.Length < 1) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("buffTargetAction", "buff.buffTargetAction.conditionGrns.error.tooFew"),
                    });
                }
                if (ConditionGrns.Length > 10) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("buffTargetAction", "buff.buffTargetAction.conditionGrns.error.tooMany"),
                    });
                }
            }
            {
                if (Rate < 0) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("buffTargetAction", "buff.buffTargetAction.rate.error.invalid"),
                    });
                }
                if (Rate > 1000000) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("buffTargetAction", "buff.buffTargetAction.rate.error.invalid"),
                    });
                }
            }
        }

        public object Clone() {
            return new BuffTargetAction {
                TargetActionName = TargetActionName,
                TargetFieldName = TargetFieldName,
                ConditionGrns = ConditionGrns.Clone() as Gs2.Gs2Buff.Model.BuffTargetGrn[],
                Rate = Rate,
            };
        }
    }
}