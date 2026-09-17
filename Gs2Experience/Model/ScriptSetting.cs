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

namespace Gs2.Gs2Experience.Model
{

#if UNITY_2017_1_OR_NEWER
	[Preserve]
#endif
	public partial class ScriptSetting : IComparable
	{
        public string TriggerScriptId { set; get; }
        public string DoneTriggerTargetType { set; get; }
        public string DoneTriggerScriptId { set; get; }
        public string DoneTriggerQueueNamespaceId { set; get; }
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
            if (data.IsString) {
                // The model arrived as the JSON text of itself rather than as
                // an object. A stamp sheet carries values the client supplied
                // as strings — a store receipt is one — so reading such a
                // request back finds the text where the object is expected.
                data = JsonMapper.ToObject(data.ToString());
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
            if (ReferenceEquals(other, null))
            {
                if (ReferenceEquals(obj, null))
                {
                    return 1;
                }
                throw new ArgumentException("Object must be of type ScriptSetting.", nameof(obj));
            }
            var diff = 0;
            diff = ModelComparer.Compare(TriggerScriptId, other.TriggerScriptId);
            if (diff != 0)
            {
                return diff;
            }
            diff = ModelComparer.Compare(DoneTriggerTargetType, other.DoneTriggerTargetType);
            if (diff != 0)
            {
                return diff;
            }
            diff = ModelComparer.Compare(DoneTriggerScriptId, other.DoneTriggerScriptId);
            if (diff != 0)
            {
                return diff;
            }
            diff = ModelComparer.Compare(DoneTriggerQueueNamespaceId, other.DoneTriggerQueueNamespaceId);
            if (diff != 0)
            {
                return diff;
            }
            return 0;
        }

        public void Validate() {
            {
                if (TriggerScriptId.Length > 1024) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("scriptSetting", "experience.scriptSetting.triggerScriptId.error.tooLong"),
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
                            new RequestError("scriptSetting", "experience.scriptSetting.doneTriggerTargetType.error.invalid"),
                        });
                }
            }
            {
                if (DoneTriggerScriptId.Length > 1024) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("scriptSetting", "experience.scriptSetting.doneTriggerScriptId.error.tooLong"),
                    });
                }
            }
            {
                if (DoneTriggerQueueNamespaceId.Length > 1024) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("scriptSetting", "experience.scriptSetting.doneTriggerQueueNamespaceId.error.tooLong"),
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