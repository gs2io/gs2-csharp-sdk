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

namespace Gs2.Gs2Showcase.Model
{

#if UNITY_2017_1_OR_NEWER
	[Preserve]
#endif
	public class RandomDisplayItem : IComparable
	{
        public string ShowcaseName { set; get; }
        public string Name { set; get; }
        public string Metadata { set; get; }
        public Gs2.Core.Model.ConsumeAction[] ConsumeActions { set; get; }
        public Gs2.Core.Model.AcquireAction[] AcquireActions { set; get; }
        public int? CurrentPurchaseCount { set; get; }
        public int? MaximumPurchaseCount { set; get; }

        public RandomDisplayItem WithShowcaseName(string showcaseName) {
            this.ShowcaseName = showcaseName;
            return this;
        }

        public RandomDisplayItem WithName(string name) {
            this.Name = name;
            return this;
        }

        public RandomDisplayItem WithMetadata(string metadata) {
            this.Metadata = metadata;
            return this;
        }

        public RandomDisplayItem WithConsumeActions(Gs2.Core.Model.ConsumeAction[] consumeActions) {
            this.ConsumeActions = consumeActions;
            return this;
        }

        public RandomDisplayItem WithAcquireActions(Gs2.Core.Model.AcquireAction[] acquireActions) {
            this.AcquireActions = acquireActions;
            return this;
        }

        public RandomDisplayItem WithCurrentPurchaseCount(int? currentPurchaseCount) {
            this.CurrentPurchaseCount = currentPurchaseCount;
            return this;
        }

        public RandomDisplayItem WithMaximumPurchaseCount(int? maximumPurchaseCount) {
            this.MaximumPurchaseCount = maximumPurchaseCount;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static RandomDisplayItem FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new RandomDisplayItem()
                .WithShowcaseName(!data.Keys.Contains("showcaseName") || data["showcaseName"] == null ? null : data["showcaseName"].ToString())
                .WithName(!data.Keys.Contains("name") || data["name"] == null ? null : data["name"].ToString())
                .WithMetadata(!data.Keys.Contains("metadata") || data["metadata"] == null ? null : data["metadata"].ToString())
                .WithConsumeActions(!data.Keys.Contains("consumeActions") || data["consumeActions"] == null || !data["consumeActions"].IsArray ? new Gs2.Core.Model.ConsumeAction[]{} : data["consumeActions"].Cast<JsonData>().Select(v => {
                    return Gs2.Core.Model.ConsumeAction.FromJson(v);
                }).ToArray())
                .WithAcquireActions(!data.Keys.Contains("acquireActions") || data["acquireActions"] == null || !data["acquireActions"].IsArray ? new Gs2.Core.Model.AcquireAction[]{} : data["acquireActions"].Cast<JsonData>().Select(v => {
                    return Gs2.Core.Model.AcquireAction.FromJson(v);
                }).ToArray())
                .WithCurrentPurchaseCount(!data.Keys.Contains("currentPurchaseCount") || data["currentPurchaseCount"] == null ? null : (int?)int.Parse(data["currentPurchaseCount"].ToString()))
                .WithMaximumPurchaseCount(!data.Keys.Contains("maximumPurchaseCount") || data["maximumPurchaseCount"] == null ? null : (int?)int.Parse(data["maximumPurchaseCount"].ToString()));
        }

        public JsonData ToJson()
        {
            JsonData consumeActionsJsonData = null;
            if (ConsumeActions != null && ConsumeActions.Length > 0)
            {
                consumeActionsJsonData = new JsonData();
                foreach (var consumeAction in ConsumeActions)
                {
                    consumeActionsJsonData.Add(consumeAction.ToJson());
                }
            }
            JsonData acquireActionsJsonData = null;
            if (AcquireActions != null && AcquireActions.Length > 0)
            {
                acquireActionsJsonData = new JsonData();
                foreach (var acquireAction in AcquireActions)
                {
                    acquireActionsJsonData.Add(acquireAction.ToJson());
                }
            }
            return new JsonData {
                ["showcaseName"] = ShowcaseName,
                ["name"] = Name,
                ["metadata"] = Metadata,
                ["consumeActions"] = consumeActionsJsonData,
                ["acquireActions"] = acquireActionsJsonData,
                ["currentPurchaseCount"] = CurrentPurchaseCount,
                ["maximumPurchaseCount"] = MaximumPurchaseCount,
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (ShowcaseName != null) {
                writer.WritePropertyName("showcaseName");
                writer.Write(ShowcaseName.ToString());
            }
            if (Name != null) {
                writer.WritePropertyName("name");
                writer.Write(Name.ToString());
            }
            if (Metadata != null) {
                writer.WritePropertyName("metadata");
                writer.Write(Metadata.ToString());
            }
            if (ConsumeActions != null) {
                writer.WritePropertyName("consumeActions");
                writer.WriteArrayStart();
                foreach (var consumeAction in ConsumeActions)
                {
                    if (consumeAction != null) {
                        consumeAction.WriteJson(writer);
                    }
                }
                writer.WriteArrayEnd();
            }
            if (AcquireActions != null) {
                writer.WritePropertyName("acquireActions");
                writer.WriteArrayStart();
                foreach (var acquireAction in AcquireActions)
                {
                    if (acquireAction != null) {
                        acquireAction.WriteJson(writer);
                    }
                }
                writer.WriteArrayEnd();
            }
            if (CurrentPurchaseCount != null) {
                writer.WritePropertyName("currentPurchaseCount");
                writer.Write(int.Parse(CurrentPurchaseCount.ToString()));
            }
            if (MaximumPurchaseCount != null) {
                writer.WritePropertyName("maximumPurchaseCount");
                writer.Write(int.Parse(MaximumPurchaseCount.ToString()));
            }
            writer.WriteObjectEnd();
        }

        public int CompareTo(object obj)
        {
            var other = obj as RandomDisplayItem;
            var diff = 0;
            if (ShowcaseName == null && ShowcaseName == other.ShowcaseName)
            {
                // null and null
            }
            else
            {
                diff += ShowcaseName.CompareTo(other.ShowcaseName);
            }
            if (Name == null && Name == other.Name)
            {
                // null and null
            }
            else
            {
                diff += Name.CompareTo(other.Name);
            }
            if (Metadata == null && Metadata == other.Metadata)
            {
                // null and null
            }
            else
            {
                diff += Metadata.CompareTo(other.Metadata);
            }
            if (ConsumeActions == null && ConsumeActions == other.ConsumeActions)
            {
                // null and null
            }
            else
            {
                diff += ConsumeActions.Length - other.ConsumeActions.Length;
                for (var i = 0; i < ConsumeActions.Length; i++)
                {
                    diff += ConsumeActions[i].CompareTo(other.ConsumeActions[i]);
                }
            }
            if (AcquireActions == null && AcquireActions == other.AcquireActions)
            {
                // null and null
            }
            else
            {
                diff += AcquireActions.Length - other.AcquireActions.Length;
                for (var i = 0; i < AcquireActions.Length; i++)
                {
                    diff += AcquireActions[i].CompareTo(other.AcquireActions[i]);
                }
            }
            if (CurrentPurchaseCount == null && CurrentPurchaseCount == other.CurrentPurchaseCount)
            {
                // null and null
            }
            else
            {
                diff += (int)(CurrentPurchaseCount - other.CurrentPurchaseCount);
            }
            if (MaximumPurchaseCount == null && MaximumPurchaseCount == other.MaximumPurchaseCount)
            {
                // null and null
            }
            else
            {
                diff += (int)(MaximumPurchaseCount - other.MaximumPurchaseCount);
            }
            return diff;
        }
    }
}