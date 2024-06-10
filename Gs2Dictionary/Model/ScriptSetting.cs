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

namespace Gs2.Gs2Dictionary.Model
{

#if UNITY_2017_1_OR_NEWER
	[Preserve]
#endif
	public class ScriptSetting : IComparable
	{
        public string TriggerScriptId { set; get; } = null!;
        public string DoneTriggerTargetType { set; get; } = null!;
        public string DoneTriggerScriptId { set; get; } = null!;
        public string DoneTriggerQueueNamespaceId { set; get; } = null!;
        public ScriptSetting WithTriggerScriptId(string triggerScriptId) {
            this.TriggerScriptId = triggerScriptId;
            return this;
        }
        public ScriptSetting WithDoneTriggerTargetType(string doneTriggerTargetType) {
            this.DoneTriggerTargetType = doneTriggerTargetType;
            return this;
        }
        public ScriptSetting WithDoneTriggerScriptId(string doneTriggerScriptId) {
            this.DoneTriggerScriptId = doneTriggerScriptId;
            return this;
        }
        public ScriptSetting WithDoneTriggerQueueNamespaceId(string doneTriggerQueueNamespaceId) {
            this.DoneTriggerQueueNamespaceId = doneTriggerQueueNamespaceId;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static ScriptSetting FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new ScriptSetting()
                .WithTriggerScriptId(!data.Keys.Contains("triggerScriptId") || data["triggerScriptId"] == null ? null : data["triggerScriptId"].ToString())
                .WithDoneTriggerTargetType(!data.Keys.Contains("doneTriggerTargetType") || data["doneTriggerTargetType"] == null ? null : data["doneTriggerTargetType"].ToString())
                .WithDoneTriggerScriptId(!data.Keys.Contains("doneTriggerScriptId") || data["doneTriggerScriptId"] == null ? null : data["doneTriggerScriptId"].ToString())
                .WithDoneTriggerQueueNamespaceId(!data.Keys.Contains("doneTriggerQueueNamespaceId") || data["doneTriggerQueueNamespaceId"] == null ? null : data["doneTriggerQueueNamespaceId"].ToString());
        }

        public JsonData ToJson()
        {
            return new JsonData {
                ["triggerScriptId"] = TriggerScriptId,
                ["doneTriggerTargetType"] = DoneTriggerTargetType,
                ["doneTriggerScriptId"] = DoneTriggerScriptId,
                ["doneTriggerQueueNamespaceId"] = DoneTriggerQueueNamespaceId,
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (TriggerScriptId != null) {
                writer.WritePropertyName("triggerScriptId");
                writer.Write(TriggerScriptId.ToString());
            }
            if (DoneTriggerTargetType != null) {
                writer.WritePropertyName("doneTriggerTargetType");
                writer.Write(DoneTriggerTargetType.ToString());
            }
            if (DoneTriggerScriptId != null) {
                writer.WritePropertyName("doneTriggerScriptId");
                writer.Write(DoneTriggerScriptId.ToString());
            }
            if (DoneTriggerQueueNamespaceId != null) {
                writer.WritePropertyName("doneTriggerQueueNamespaceId");
                writer.Write(DoneTriggerQueueNamespaceId.ToString());
            }
            writer.WriteObjectEnd();
        }

        public int CompareTo(object obj)
        {
            var other = obj as ScriptSetting;
            var diff = 0;
            if (TriggerScriptId == null && TriggerScriptId == other.TriggerScriptId)
            {
                // null and null
            }
            else
            {
                diff += TriggerScriptId.CompareTo(other.TriggerScriptId);
            }
            if (DoneTriggerTargetType == null && DoneTriggerTargetType == other.DoneTriggerTargetType)
            {
                // null and null
            }
            else
            {
                diff += DoneTriggerTargetType.CompareTo(other.DoneTriggerTargetType);
            }
            if (DoneTriggerScriptId == null && DoneTriggerScriptId == other.DoneTriggerScriptId)
            {
                // null and null
            }
            else
            {
                diff += DoneTriggerScriptId.CompareTo(other.DoneTriggerScriptId);
            }
            if (DoneTriggerQueueNamespaceId == null && DoneTriggerQueueNamespaceId == other.DoneTriggerQueueNamespaceId)
            {
                // null and null
            }
            else
            {
                diff += DoneTriggerQueueNamespaceId.CompareTo(other.DoneTriggerQueueNamespaceId);
            }
            return diff;
        }

        public void Validate() {
            {
                if (TriggerScriptId.Length > 1024) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("scriptSetting", "dictionary.scriptSetting.triggerScriptId.error.tooLong"),
                    });
                }
            }
            {
                switch (DoneTriggerTargetType) {
                    case "none":
                    case "gs2_script":
                    case "aws":
                        break;
                    default:
                        throw new Gs2.Core.Exception.BadRequestException(new [] {
                            new RequestError("scriptSetting", "dictionary.scriptSetting.doneTriggerTargetType.error.invalid"),
                        });
                }
            }
            {
                if (DoneTriggerScriptId.Length > 1024) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("scriptSetting", "dictionary.scriptSetting.doneTriggerScriptId.error.tooLong"),
                    });
                }
            }
            {
                if (DoneTriggerQueueNamespaceId.Length > 1024) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("scriptSetting", "dictionary.scriptSetting.doneTriggerQueueNamespaceId.error.tooLong"),
                    });
                }
            }
        }

        public object Clone() {
            return new ScriptSetting {
                TriggerScriptId = TriggerScriptId,
                DoneTriggerTargetType = DoneTriggerTargetType,
                DoneTriggerScriptId = DoneTriggerScriptId,
                DoneTriggerQueueNamespaceId = DoneTriggerQueueNamespaceId,
            };
        }
    }
}