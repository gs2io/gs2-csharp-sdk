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
        public string ShowcaseName { set; get; } = null!;
        public string Name { set; get; } = null!;
        public string Metadata { set; get; } = null!;
        public Gs2.Core.Model.VerifyAction[] VerifyActions { set; get; } = null!;
        public Gs2.Core.Model.ConsumeAction[] ConsumeActions { set; get; } = null!;
        public Gs2.Core.Model.AcquireAction[] AcquireActions { set; get; } = null!;
        public int? CurrentPurchaseCount { set; get; } = null!;
        public int? MaximumPurchaseCount { set; get; } = null!;
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
        public RandomDisplayItem WithVerifyActions(Gs2.Core.Model.VerifyAction[] verifyActions) {
            this.VerifyActions = verifyActions;
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
                .WithVerifyActions(!data.Keys.Contains("verifyActions") || data["verifyActions"] == null || !data["verifyActions"].IsArray ? null : data["verifyActions"].Cast<JsonData>().Select(v => {
                    return Gs2.Core.Model.VerifyAction.FromJson(v);
                }).ToArray())
                .WithConsumeActions(!data.Keys.Contains("consumeActions") || data["consumeActions"] == null || !data["consumeActions"].IsArray ? null : data["consumeActions"].Cast<JsonData>().Select(v => {
                    return Gs2.Core.Model.ConsumeAction.FromJson(v);
                }).ToArray())
                .WithAcquireActions(!data.Keys.Contains("acquireActions") || data["acquireActions"] == null || !data["acquireActions"].IsArray ? null : data["acquireActions"].Cast<JsonData>().Select(v => {
                    return Gs2.Core.Model.AcquireAction.FromJson(v);
                }).ToArray())
                .WithCurrentPurchaseCount(!data.Keys.Contains("currentPurchaseCount") || data["currentPurchaseCount"] == null ? null : (int?)(data["currentPurchaseCount"].ToString().Contains(".") ? (int)double.Parse(data["currentPurchaseCount"].ToString()) : int.Parse(data["currentPurchaseCount"].ToString())))
                .WithMaximumPurchaseCount(!data.Keys.Contains("maximumPurchaseCount") || data["maximumPurchaseCount"] == null ? null : (int?)(data["maximumPurchaseCount"].ToString().Contains(".") ? (int)double.Parse(data["maximumPurchaseCount"].ToString()) : int.Parse(data["maximumPurchaseCount"].ToString())));
        }

        public JsonData ToJson()
        {
            JsonData verifyActionsJsonData = null;
            if (VerifyActions != null && VerifyActions.Length > 0)
            {
                verifyActionsJsonData = new JsonData();
                foreach (var verifyAction in VerifyActions)
                {
                    verifyActionsJsonData.Add(verifyAction.ToJson());
                }
            }
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
                ["verifyActions"] = verifyActionsJsonData,
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
            if (VerifyActions != null) {
                writer.WritePropertyName("verifyActions");
                writer.WriteArrayStart();
                foreach (var verifyAction in VerifyActions)
                {
                    if (verifyAction != null) {
                        verifyAction.WriteJson(writer);
                    }
                }
                writer.WriteArrayEnd();
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
                writer.Write((CurrentPurchaseCount.ToString().Contains(".") ? (int)double.Parse(CurrentPurchaseCount.ToString()) : int.Parse(CurrentPurchaseCount.ToString())));
            }
            if (MaximumPurchaseCount != null) {
                writer.WritePropertyName("maximumPurchaseCount");
                writer.Write((MaximumPurchaseCount.ToString().Contains(".") ? (int)double.Parse(MaximumPurchaseCount.ToString()) : int.Parse(MaximumPurchaseCount.ToString())));
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
            if (VerifyActions == null && VerifyActions == other.VerifyActions)
            {
                // null and null
            }
            else
            {
                diff += VerifyActions.Length - other.VerifyActions.Length;
                for (var i = 0; i < VerifyActions.Length; i++)
                {
                    diff += VerifyActions[i].CompareTo(other.VerifyActions[i]);
                }
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

        public void Validate() {
            {
                if (ShowcaseName.Length > 128) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("randomDisplayItem", "showcase.randomDisplayItem.showcaseName.error.tooLong"),
                    });
                }
            }
            {
                if (Name.Length > 128) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("randomDisplayItem", "showcase.randomDisplayItem.name.error.tooLong"),
                    });
                }
            }
            {
                if (Metadata.Length > 2048) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("randomDisplayItem", "showcase.randomDisplayItem.metadata.error.tooLong"),
                    });
                }
            }
            {
                if (VerifyActions.Length > 10) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("randomDisplayItem", "showcase.randomDisplayItem.verifyActions.error.tooMany"),
                    });
                }
            }
            {
                if (ConsumeActions.Length > 10) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("randomDisplayItem", "showcase.randomDisplayItem.consumeActions.error.tooMany"),
                    });
                }
            }
            {
                if (AcquireActions.Length < 1) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("randomDisplayItem", "showcase.randomDisplayItem.acquireActions.error.tooFew"),
                    });
                }
                if (AcquireActions.Length > 100) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("randomDisplayItem", "showcase.randomDisplayItem.acquireActions.error.tooMany"),
                    });
                }
            }
            {
                if (CurrentPurchaseCount < 1) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("randomDisplayItem", "showcase.randomDisplayItem.currentPurchaseCount.error.invalid"),
                    });
                }
                if (CurrentPurchaseCount > 2147483646) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("randomDisplayItem", "showcase.randomDisplayItem.currentPurchaseCount.error.invalid"),
                    });
                }
            }
            {
                if (MaximumPurchaseCount < 1) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("randomDisplayItem", "showcase.randomDisplayItem.maximumPurchaseCount.error.invalid"),
                    });
                }
                if (MaximumPurchaseCount > 2147483646) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("randomDisplayItem", "showcase.randomDisplayItem.maximumPurchaseCount.error.invalid"),
                    });
                }
            }
        }

        public object Clone() {
            return new RandomDisplayItem {
                ShowcaseName = ShowcaseName,
                Name = Name,
                Metadata = Metadata,
                VerifyActions = VerifyActions.Clone() as Gs2.Core.Model.VerifyAction[],
                ConsumeActions = ConsumeActions.Clone() as Gs2.Core.Model.ConsumeAction[],
                AcquireActions = AcquireActions.Clone() as Gs2.Core.Model.AcquireAction[],
                CurrentPurchaseCount = CurrentPurchaseCount,
                MaximumPurchaseCount = MaximumPurchaseCount,
            };
        }
    }
}