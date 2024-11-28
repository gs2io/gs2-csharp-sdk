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
using Gs2.Core.Control;
using Gs2.Core.Model;
using Gs2.Gs2Showcase.Model;
using Gs2.Util.LitJson;

#if UNITY_2017_1_OR_NEWER
using UnityEngine.Scripting;
#endif

namespace Gs2.Gs2Showcase.Request
{
#if UNITY_2017_1_OR_NEWER
	[Preserve]
#endif
	[System.Serializable]
	public class RandomShowcaseBuyByUserIdRequest : Gs2Request<RandomShowcaseBuyByUserIdRequest>
	{
         public string NamespaceName { set; get; } = null!;
         public string ShowcaseName { set; get; } = null!;
         public string DisplayItemName { set; get; } = null!;
         public string UserId { set; get; } = null!;
         public int? Quantity { set; get; } = null!;
         public Gs2.Gs2Showcase.Model.Config[] Config { set; get; } = null!;
         public string TimeOffsetToken { set; get; } = null!;
        public string DuplicationAvoider { set; get; } = null!;
        public RandomShowcaseBuyByUserIdRequest WithNamespaceName(string namespaceName) {
            this.NamespaceName = namespaceName;
            return this;
        }
        public RandomShowcaseBuyByUserIdRequest WithShowcaseName(string showcaseName) {
            this.ShowcaseName = showcaseName;
            return this;
        }
        public RandomShowcaseBuyByUserIdRequest WithDisplayItemName(string displayItemName) {
            this.DisplayItemName = displayItemName;
            return this;
        }
        public RandomShowcaseBuyByUserIdRequest WithUserId(string userId) {
            this.UserId = userId;
            return this;
        }
        public RandomShowcaseBuyByUserIdRequest WithQuantity(int? quantity) {
            this.Quantity = quantity;
            return this;
        }
        public RandomShowcaseBuyByUserIdRequest WithConfig(Gs2.Gs2Showcase.Model.Config[] config) {
            this.Config = config;
            return this;
        }
        public RandomShowcaseBuyByUserIdRequest WithTimeOffsetToken(string timeOffsetToken) {
            this.TimeOffsetToken = timeOffsetToken;
            return this;
        }

        public RandomShowcaseBuyByUserIdRequest WithDuplicationAvoider(string duplicationAvoider) {
            this.DuplicationAvoider = duplicationAvoider;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static RandomShowcaseBuyByUserIdRequest FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new RandomShowcaseBuyByUserIdRequest()
                .WithNamespaceName(!data.Keys.Contains("namespaceName") || data["namespaceName"] == null ? null : data["namespaceName"].ToString())
                .WithShowcaseName(!data.Keys.Contains("showcaseName") || data["showcaseName"] == null ? null : data["showcaseName"].ToString())
                .WithDisplayItemName(!data.Keys.Contains("displayItemName") || data["displayItemName"] == null ? null : data["displayItemName"].ToString())
                .WithUserId(!data.Keys.Contains("userId") || data["userId"] == null ? null : data["userId"].ToString())
                .WithQuantity(!data.Keys.Contains("quantity") || data["quantity"] == null ? null : (int?)(data["quantity"].ToString().Contains(".") ? (int)double.Parse(data["quantity"].ToString()) : int.Parse(data["quantity"].ToString())))
                .WithConfig(!data.Keys.Contains("config") || data["config"] == null || !data["config"].IsArray ? null : data["config"].Cast<JsonData>().Select(v => {
                    return Gs2.Gs2Showcase.Model.Config.FromJson(v);
                }).ToArray())
                .WithTimeOffsetToken(!data.Keys.Contains("timeOffsetToken") || data["timeOffsetToken"] == null ? null : data["timeOffsetToken"].ToString());
        }

        public override JsonData ToJson()
        {
            JsonData configJsonData = null;
            if (Config != null && Config.Length > 0)
            {
                configJsonData = new JsonData();
                foreach (var confi in Config)
                {
                    configJsonData.Add(confi.ToJson());
                }
            }
            return new JsonData {
                ["namespaceName"] = NamespaceName,
                ["showcaseName"] = ShowcaseName,
                ["displayItemName"] = DisplayItemName,
                ["userId"] = UserId,
                ["quantity"] = Quantity,
                ["config"] = configJsonData,
                ["timeOffsetToken"] = TimeOffsetToken,
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (NamespaceName != null) {
                writer.WritePropertyName("namespaceName");
                writer.Write(NamespaceName.ToString());
            }
            if (ShowcaseName != null) {
                writer.WritePropertyName("showcaseName");
                writer.Write(ShowcaseName.ToString());
            }
            if (DisplayItemName != null) {
                writer.WritePropertyName("displayItemName");
                writer.Write(DisplayItemName.ToString());
            }
            if (UserId != null) {
                writer.WritePropertyName("userId");
                writer.Write(UserId.ToString());
            }
            if (Quantity != null) {
                writer.WritePropertyName("quantity");
                writer.Write((Quantity.ToString().Contains(".") ? (int)double.Parse(Quantity.ToString()) : int.Parse(Quantity.ToString())));
            }
            if (Config != null) {
                writer.WritePropertyName("config");
                writer.WriteArrayStart();
                foreach (var confi in Config)
                {
                    if (confi != null) {
                        confi.WriteJson(writer);
                    }
                }
                writer.WriteArrayEnd();
            }
            if (TimeOffsetToken != null) {
                writer.WritePropertyName("timeOffsetToken");
                writer.Write(TimeOffsetToken.ToString());
            }
            writer.WriteObjectEnd();
        }

        public override string UniqueKey() {
            var key = "";
            key += NamespaceName + ":";
            key += ShowcaseName + ":";
            key += DisplayItemName + ":";
            key += UserId + ":";
            key += Quantity + ":";
            key += Config + ":";
            key += TimeOffsetToken + ":";
            return key;
        }
    }
}