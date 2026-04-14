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
using Gs2.Core.Control;
using Gs2.Core.Model;
using Gs2.Gs2Log.Model;
using Gs2.Util.LitJson;

#if UNITY_2017_1_OR_NEWER
using UnityEngine.Scripting;
#endif

namespace Gs2.Gs2Log.Request
{
#if UNITY_2017_1_OR_NEWER
	[Preserve]
#endif
	[System.Serializable]
	public class CreateFacetModelRequest : Gs2Request<CreateFacetModelRequest>
	{
         public string NamespaceName { set; get; } = null!;
         public string Field { set; get; } = null!;
         public string Type { set; get; } = null!;
         public string DisplayName { set; get; } = null!;
         public int? Order { set; get; } = null!;
        public CreateFacetModelRequest WithNamespaceName(string namespaceName) {
            this.NamespaceName = namespaceName;
            return this;
        }
        public CreateFacetModelRequest WithField(string field) {
            this.Field = field;
            return this;
        }
        public CreateFacetModelRequest WithType(string type) {
            this.Type = type;
            return this;
        }
        public CreateFacetModelRequest WithDisplayName(string displayName) {
            this.DisplayName = displayName;
            return this;
        }
        public CreateFacetModelRequest WithOrder(int? order) {
            this.Order = order;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static CreateFacetModelRequest FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new CreateFacetModelRequest()
                .WithNamespaceName(!data.Keys.Contains("namespaceName") || data["namespaceName"] == null ? null : data["namespaceName"].ToString())
                .WithField(!data.Keys.Contains("field") || data["field"] == null ? null : data["field"].ToString())
                .WithType(!data.Keys.Contains("type") || data["type"] == null ? null : data["type"].ToString())
                .WithDisplayName(!data.Keys.Contains("displayName") || data["displayName"] == null ? null : data["displayName"].ToString())
                .WithOrder(!data.Keys.Contains("order") || data["order"] == null ? null : (int?)(data["order"].ToString().Contains(".") ? (int)double.Parse(data["order"].ToString()) : int.Parse(data["order"].ToString())));
        }

        public override JsonData ToJson()
        {
            return new JsonData {
                ["namespaceName"] = NamespaceName,
                ["field"] = Field,
                ["type"] = Type,
                ["displayName"] = DisplayName,
                ["order"] = Order,
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (NamespaceName != null) {
                writer.WritePropertyName("namespaceName");
                writer.Write(NamespaceName.ToString());
            }
            if (Field != null) {
                writer.WritePropertyName("field");
                writer.Write(Field.ToString());
            }
            if (Type != null) {
                writer.WritePropertyName("type");
                writer.Write(Type.ToString());
            }
            if (DisplayName != null) {
                writer.WritePropertyName("displayName");
                writer.Write(DisplayName.ToString());
            }
            if (Order != null) {
                writer.WritePropertyName("order");
                writer.Write((Order.ToString().Contains(".") ? (int)double.Parse(Order.ToString()) : int.Parse(Order.ToString())));
            }
            writer.WriteObjectEnd();
        }

        public override string UniqueKey() {
            var key = "";
            key += NamespaceName + ":";
            key += Field + ":";
            key += Type + ":";
            key += DisplayName + ":";
            key += Order + ":";
            return key;
        }
    }
}