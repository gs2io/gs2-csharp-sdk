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

namespace Gs2.Gs2Inbox.Model
{
	[Preserve]
	public class ScriptSetting : IComparable
	{

        /** 実行前に使用する GS2-Script のスクリプト のGRN */
        public string triggerScriptId { set; get; }

        /**
         * 実行前に使用する GS2-Script のスクリプト のGRNを設定
         *
         * @param triggerScriptId 実行前に使用する GS2-Script のスクリプト のGRN
         * @return this
         */
        public ScriptSetting WithTriggerScriptId(string triggerScriptId) {
            this.triggerScriptId = triggerScriptId;
            return this;
        }

        /** 完了通知の通知先 */
        public string doneTriggerTargetType { set; get; }

        /**
         * 完了通知の通知先を設定
         *
         * @param doneTriggerTargetType 完了通知の通知先
         * @return this
         */
        public ScriptSetting WithDoneTriggerTargetType(string doneTriggerTargetType) {
            this.doneTriggerTargetType = doneTriggerTargetType;
            return this;
        }

        /** 完了時に使用する GS2-Script のスクリプト のGRN */
        public string doneTriggerScriptId { set; get; }

        /**
         * 完了時に使用する GS2-Script のスクリプト のGRNを設定
         *
         * @param doneTriggerScriptId 完了時に使用する GS2-Script のスクリプト のGRN
         * @return this
         */
        public ScriptSetting WithDoneTriggerScriptId(string doneTriggerScriptId) {
            this.doneTriggerScriptId = doneTriggerScriptId;
            return this;
        }

        /** 完了時に使用する GS2-JobQueue のネームスペース のGRN */
        public string doneTriggerQueueNamespaceId { set; get; }

        /**
         * 完了時に使用する GS2-JobQueue のネームスペース のGRNを設定
         *
         * @param doneTriggerQueueNamespaceId 完了時に使用する GS2-JobQueue のネームスペース のGRN
         * @return this
         */
        public ScriptSetting WithDoneTriggerQueueNamespaceId(string doneTriggerQueueNamespaceId) {
            this.doneTriggerQueueNamespaceId = doneTriggerQueueNamespaceId;
            return this;
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if(this.triggerScriptId != null)
            {
                writer.WritePropertyName("triggerScriptId");
                writer.Write(this.triggerScriptId);
            }
            if(this.doneTriggerTargetType != null)
            {
                writer.WritePropertyName("doneTriggerTargetType");
                writer.Write(this.doneTriggerTargetType);
            }
            if(this.doneTriggerScriptId != null)
            {
                writer.WritePropertyName("doneTriggerScriptId");
                writer.Write(this.doneTriggerScriptId);
            }
            if(this.doneTriggerQueueNamespaceId != null)
            {
                writer.WritePropertyName("doneTriggerQueueNamespaceId");
                writer.Write(this.doneTriggerQueueNamespaceId);
            }
            writer.WriteObjectEnd();
        }

    	[Preserve]
        public static ScriptSetting FromDict(JsonData data)
        {
            return new ScriptSetting()
                .WithTriggerScriptId(data.Keys.Contains("triggerScriptId") && data["triggerScriptId"] != null ? data["triggerScriptId"].ToString() : null)
                .WithDoneTriggerTargetType(data.Keys.Contains("doneTriggerTargetType") && data["doneTriggerTargetType"] != null ? data["doneTriggerTargetType"].ToString() : null)
                .WithDoneTriggerScriptId(data.Keys.Contains("doneTriggerScriptId") && data["doneTriggerScriptId"] != null ? data["doneTriggerScriptId"].ToString() : null)
                .WithDoneTriggerQueueNamespaceId(data.Keys.Contains("doneTriggerQueueNamespaceId") && data["doneTriggerQueueNamespaceId"] != null ? data["doneTriggerQueueNamespaceId"].ToString() : null);
        }

        public int CompareTo(object obj)
        {
            var other = obj as ScriptSetting;
            var diff = 0;
            if (triggerScriptId == null && triggerScriptId == other.triggerScriptId)
            {
                // null and null
            }
            else
            {
                diff += triggerScriptId.CompareTo(other.triggerScriptId);
            }
            if (doneTriggerTargetType == null && doneTriggerTargetType == other.doneTriggerTargetType)
            {
                // null and null
            }
            else
            {
                diff += doneTriggerTargetType.CompareTo(other.doneTriggerTargetType);
            }
            if (doneTriggerScriptId == null && doneTriggerScriptId == other.doneTriggerScriptId)
            {
                // null and null
            }
            else
            {
                diff += doneTriggerScriptId.CompareTo(other.doneTriggerScriptId);
            }
            if (doneTriggerQueueNamespaceId == null && doneTriggerQueueNamespaceId == other.doneTriggerQueueNamespaceId)
            {
                // null and null
            }
            else
            {
                diff += doneTriggerQueueNamespaceId.CompareTo(other.doneTriggerQueueNamespaceId);
            }
            return diff;
        }

        public JsonData ToDict()
        {
            var data = new JsonData();
            data["triggerScriptId"] = triggerScriptId;
            data["doneTriggerTargetType"] = doneTriggerTargetType;
            data["doneTriggerScriptId"] = doneTriggerScriptId;
            data["doneTriggerQueueNamespaceId"] = doneTriggerQueueNamespaceId;
            return data;
        }
	}
}